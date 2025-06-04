using System;
using System.Timers;
using MySql.Data.MySqlClient;
using S7.Net;

namespace PLCDataToMySQL
{
    class Program
    {
        private static System.Timers.Timer _timer;
        private static Plc _plc;

        // PLC连接参数（根据图片中的CPU 1511-1 PN设置）
        private static string plcIp = "10.80.138.253"; // 本地仿真IP
        private static CpuType cpuType = CpuType.S71500;
        private static short rack = 0;
        private static short slot = 1; // S7-1500通常使用slot 1

        // 数据库连接参数
        private static string dbServer = "localhost";
        private static string dbName = "plctest";
        private static string dbUser = "root";
        private static string dbPassword = "123456";

        static void Main(string[] args)
        {
            Console.WriteLine("PLC数据采集系统启动...");
            Console.WriteLine($"目标PLC: {plcIp} (CPU 1511-1 PN)");

            // 初始化PLC连接
            _plc = new Plc(cpuType, plcIp, rack, slot);

            // 数据库连接字符串
            string connectionString = $"server={dbServer};port=3306;database={dbName};user={dbUser};password={dbPassword};";

            try
            {
                // 测试PLC连接
                Console.WriteLine("尝试连接PLC...");
                _plc.Open();

                if (_plc.IsConnected)
                {
                    Console.WriteLine("PLC连接成功");
                    Console.WriteLine("数据块结构：");
                    Console.WriteLine(" - Static_1: Array[0..5] of Int (DB1.DBW0-DBW10)");
                    Console.WriteLine(" - Static_2: Array[0..2] of Real (DB1.DBD12-DBD20)");
                    Console.WriteLine(" - Static_3: Array[0..2] of Bool (DB1.DBX24.0-DBX24.2)");

                    // 每秒执行一次数据采集
                    _timer = new System.Timers.Timer(1000);
                    _timer.Elapsed += (sender, e) => ProcessPLCData(connectionString);
                    _timer.AutoReset = true;
                    _timer.Enabled = true;

                    Console.WriteLine("\n数据采集中... 按Enter键停止");
                    Console.ReadLine();

                    _timer.Stop();
                    _timer.Dispose();
                }
                else
                {
                    Console.WriteLine("PLC连接失败！请检查：");
                    Console.WriteLine("1. TIA Portal中PLC是否处于运行状态");
                    Console.WriteLine("2. PLCSIM Advanced是否已启动");
                    Console.WriteLine("3. 防火墙设置是否允许连接");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"初始化错误: {ex.Message}");
            }
            finally
            {
                if (_plc.IsConnected)
                {
                    _plc.Close();
                    Console.WriteLine("已断开PLC连接");
                }
            }
        }

        static void ProcessPLCData(string connectionString)
        {
            DateTime now = DateTime.Now;
            string tableName = $"plc_data_{now:yyyyMMdd}";
            string timestamp = now.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                if (!_plc.IsConnected)
                {
                    _plc.Open();
                }

                // 1. 读取PLC数据（根据图片中的偏移量）
                // Static_1: Array[0..5] of Int (DB1.DBW0-DBW10)
                short[] static1 = (short[])_plc.Read(DataType.DataBlock, 1, 0, VarType.Int, 6);

                // Static_2: Array[0..2] of Real (DB1.DBD12-DBD20)
                float[] static2 = (float[])_plc.Read(DataType.DataBlock, 1, 12, VarType.Real, 3);

                // Static_3: Array[0..2] of Bool (DB1.DBX24.0-DBX24.2)
                bool[] static3 = new bool[3];
                for (int i = 0; i < 3; i++)
                {
                    static3[i] = (bool)_plc.Read(DataType.DataBlock, 1, 24, VarType.Bit, 1, (byte)i);
                }

                // 2. 准备数据库操作
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // 创建表（如果不存在）
                    string createTableSql = $@"
                        CREATE TABLE IF NOT EXISTS `{tableName}` (
                            `id` INT AUTO_INCREMENT PRIMARY KEY,
                            `timestamp` DATETIME NOT NULL,
                            `static1_0` SMALLINT COMMENT 'Static_1[0]',
                            `static1_1` SMALLINT COMMENT 'Static_1[1]',
                            `static1_2` SMALLINT COMMENT 'Static_1[2]',
                            `static1_3` SMALLINT COMMENT 'Static_1[3]',
                            `static1_4` SMALLINT COMMENT 'Static_1[4]',
                            `static1_5` SMALLINT COMMENT 'Static_1[5]',
                            `static2_0` FLOAT COMMENT 'Static_2[0]',
                            `static2_1` FLOAT COMMENT 'Static_2[1]',
                            `static2_2` FLOAT COMMENT 'Static_2[2]',
                            `static3_0` BOOLEAN COMMENT 'Static_3[0] (DBX24.0)',
                            `static3_1` BOOLEAN COMMENT 'Static_3[1] (DBX24.1)',
                            `static3_2` BOOLEAN COMMENT 'Static_3[2] (DBX24.2)',
                            INDEX `idx_timestamp` (`timestamp`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='PLC数据采集记录';";

                    new MySqlCommand(createTableSql, conn).ExecuteNonQuery();

                    // 插入数据
                    string insertSql = $@"
                        INSERT INTO `{tableName}` (
                            `timestamp`, 
                            `static1_0`, `static1_1`, `static1_2`, `static1_3`, `static1_4`, `static1_5`,
                            `static2_0`, `static2_1`, `static2_2`,
                            `static3_0`, `static3_1`, `static3_2`
                        ) VALUES (
                            @ts, @s10, @s11, @s12, @s13, @s14, @s15,
                            @s20, @s21, @s22,
                            @s30, @s31, @s32
                        )";

                    using (MySqlCommand cmd = new MySqlCommand(insertSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ts", timestamp);

                        // Static_1参数
                        for (int i = 0; i < 6; i++)
                        {
                            cmd.Parameters.AddWithValue($"@s1{i}", static1[i]);
                        }

                        // Static_2参数
                        for (int i = 0; i < 3; i++)
                        {
                            cmd.Parameters.AddWithValue($"@s2{i}", static2[i]);
                        }

                        // Static_3参数
                        for (int i = 0; i < 3; i++)
                        {
                            cmd.Parameters.AddWithValue($"@s3{i}", static3[i]);
                        }

                        cmd.ExecuteNonQuery();
                        Console.WriteLine($"[{timestamp}] 数据已存储到 {tableName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{timestamp}] 错误: {ex.Message}");
            }
        }
    }
}