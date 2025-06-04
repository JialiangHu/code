using System;
using System.Timers;
using MySql.Data.MySqlClient;

namespace GasStationDataInsert
{
    class Program
    {
        private static System.Timers.Timer _timer; // 定时器
        private static Random _random = new Random(); // 随机数生成器

        static void Main(string[] args)
        {
            // 数据库连接字符串
            string connectionString = "server=localhost;port=3306;database=plctest;user=root;password=123456;";

            // 每秒执行一次插入
            _timer = new System.Timers.Timer(1000); // 设置间隔为1秒
            _timer.Elapsed += (sender, e) => InsertData(connectionString); // 绑定事件
            _timer.AutoReset = true; // 自动重复
            _timer.Enabled = true; // 启用定时器

            Console.WriteLine("开始执行插入操作，每秒插入一条数据。按 Enter 键退出...");
            Console.ReadLine(); // 按 Enter 键退出程序
            _timer.Stop();
            _timer.Dispose();
        }

        public static void InsertData(string connectionString)
        {
            // 当前时间和表名
            DateTime now = DateTime.Now;
            string tableName = $"airsupply_station_plcdata_{now:yyyyMMdd}"; // 动态表名
            string currentTime = now.ToString("yyyy-MM-dd HH:mm:ss"); // 当前时间字符串

            // 生成随机数数据
            float randomValue() => (float)Math.Round(_random.NextDouble() * 5, 2); // 生成 0~5 的随机数，保留 2 位小数

            // 创建表的 SQL 语句
            string createTableSql = $@"
                CREATE TABLE IF NOT EXISTS {tableName} (
                    id INT AUTO_INCREMENT PRIMARY KEY COMMENT '唯一标识符',
                    record_time DATETIME COMMENT '时间',
                    secondary_suction_pressure FLOAT COMMENT '二级吸压',
                    secondary_cylinder_pressure FLOAT COMMENT '二级缸压',
                    secondary_discharge_pressure FLOAT COMMENT '二级排压',
                    tertiary_suction_pressure FLOAT COMMENT '三级吸压',
                    tertiary_cylinder_pressure FLOAT COMMENT '三级缸压',
                    tertiary_discharge_pressure FLOAT COMMENT '三级排压',
                    quaternary_suction_pressure FLOAT COMMENT '四级吸压',
                    quaternary_cylinder_pressure FLOAT COMMENT '四级缸压',
                    quaternary_discharge_pressure FLOAT COMMENT '四级排压',
                    lubrication_oil_pressure FLOAT COMMENT '润滑油压',
                    cooling_water_inlet_pressure FLOAT COMMENT '冷却水进口压力',
                    cooling_water_outlet_pressure FLOAT COMMENT '冷却水出口压力',
                    primary_suction_pressure FLOAT COMMENT '一级吸压（空，备用）',
                    primary_cylinder_pressure FLOAT COMMENT '一级缸压（空，备用）',
                    secondary_inlet_temperature FLOAT COMMENT '二级进温',
                    secondary_cylinder_temperature FLOAT COMMENT '二级缸温',
                    secondary_discharge_temperature FLOAT COMMENT '二级排温',
                    tertiary_inlet_temperature FLOAT COMMENT '三级进温',
                    tertiary_cylinder_temperature FLOAT COMMENT '三级缸温',
                    tertiary_discharge_temperature FLOAT COMMENT '三级排温',
                    quaternary_inlet_temperature FLOAT COMMENT '四级进温',
                    quaternary_cylinder_temperature FLOAT COMMENT '四级缸温',
                    quaternary_discharge_temperature FLOAT COMMENT '四级排温',
                    main_bearing1 FLOAT COMMENT '主机轴承1',
                    main_bearing2 FLOAT COMMENT '主机轴承2',
                    oil_circuit_temperature FLOAT COMMENT '油路温度',
                    cooling_water_inlet_temperature FLOAT COMMENT '冷却水进口温度',
                    cooling_water_outlet_temperature FLOAT COMMENT '冷却水出口温度',
                    primary_inlet_temperature FLOAT COMMENT '一级进温（空，备用）',
                    primary_cylinder_temperature FLOAT COMMENT '一级缸温（空，备用）',
                    screw_dew_point_temperature FLOAT COMMENT '螺杆机露点',
                    screw_primary_discharge_pressure FLOAT COMMENT '螺杆:一级排气压力',
                    screw_primary_suction_pressure FLOAT COMMENT '螺杆:一级吸气压力',
                    screw_primary_cylinder_pressure FLOAT COMMENT '螺杆:一级缸内压力',
                    screw_primary_discharge_temperature FLOAT COMMENT '螺杆:一级排气温度',
                    screw_primary_inlet_temperature FLOAT COMMENT '螺杆:一级进气温度',
                    screw_param6 FLOAT COMMENT '螺杆参数6（空）',
                    screw_param7 FLOAT COMMENT '螺杆参数7（空）',
                    screw_primary_cylinder_temperature FLOAT COMMENT '螺杆:一级缸内温度',
                    dryer_dew_point_temperature FLOAT COMMENT '干燥器露点温度',
                    dryer_inlet_temperature FLOAT COMMENT '干燥器进气温度',
                    dryer_outlet_pressure FLOAT COMMENT '干燥器出口压力',
                    v1168_0_1_running FLOAT COMMENT 'V1168.0-1运行',
                    wushuju FLOAT COMMENT '空',
                    piston_phase_A_voltage FLOAT COMMENT '活塞机A相电压',
                    piston_phase_B_voltage FLOAT COMMENT '活塞机B相电压',
                    piston_phase_C_voltage FLOAT COMMENT '活塞机C相电压',
                    piston_phase_A_current FLOAT COMMENT '活塞机A相电流',
                    piston_phase_B_current FLOAT COMMENT '活塞机B相电流',
                    piston_phase_C_current FLOAT COMMENT '活塞机C相电流',
                    piston_active_power FLOAT COMMENT '活塞机总有功功率',
                    piston_reactive_power FLOAT COMMENT '活塞机总无功功率',
                    screw_phase_A_voltage FLOAT COMMENT '螺杆机A相电压',
                    screw_phase_B_voltage FLOAT COMMENT '螺杆机B相电压',
                    screw_phase_C_voltage FLOAT COMMENT '螺杆机C相电压',
                    screw_phase_A_current FLOAT COMMENT '螺杆机A相电流',
                    screw_phase_B_current FLOAT COMMENT '螺杆机B相电流',
                    screw_phase_C_current FLOAT COMMENT '螺杆机C相电流',
                    screw_active_power FLOAT COMMENT '螺杆机总有功功率',
                    screw_reactive_power FLOAT COMMENT '螺杆机总无功功率'
                ) COMMENT='供气站传感器数据表';";

            // 插入数据的 SQL 语句
            string insertSql = $@"
                INSERT INTO {tableName} (
                    record_time,
                    secondary_suction_pressure,
                    secondary_cylinder_pressure,
                    secondary_discharge_pressure,
                    tertiary_suction_pressure,
                    tertiary_cylinder_pressure,
                    tertiary_discharge_pressure,
                    quaternary_suction_pressure,
                    quaternary_cylinder_pressure,
                    quaternary_discharge_pressure,
                    lubrication_oil_pressure,
                    cooling_water_inlet_pressure,
                    cooling_water_outlet_pressure,
                    primary_suction_pressure,
                    primary_cylinder_pressure,
                    secondary_inlet_temperature,
                    secondary_cylinder_temperature,
                    secondary_discharge_temperature,
                    tertiary_inlet_temperature,
                    tertiary_cylinder_temperature,
                    tertiary_discharge_temperature,
                    quaternary_inlet_temperature,
                    quaternary_cylinder_temperature,
                    quaternary_discharge_temperature,
                    main_bearing1,
                    main_bearing2,
                    oil_circuit_temperature,
                    cooling_water_inlet_temperature,
                    cooling_water_outlet_temperature,
                    primary_inlet_temperature,
                    primary_cylinder_temperature,
                    screw_dew_point_temperature,
                    screw_primary_discharge_pressure,
                    screw_primary_suction_pressure,
                    screw_primary_cylinder_pressure,
                    screw_primary_discharge_temperature,
                    screw_primary_inlet_temperature,
                    screw_param6,
                    screw_param7,
                    screw_primary_cylinder_temperature,
                    dryer_dew_point_temperature,
                    dryer_inlet_temperature,
                    dryer_outlet_pressure,
                    v1168_0_1_running,
                    wushuju,
                    piston_phase_A_voltage,
                    piston_phase_B_voltage,
                    piston_phase_C_voltage,
                    piston_phase_A_current,
                    piston_phase_B_current,
                    piston_phase_C_current,
                    piston_active_power,
                    piston_reactive_power,
                    screw_phase_A_voltage,
                    screw_phase_B_voltage,
                    screw_phase_C_voltage,
                    screw_phase_A_current,
                    screw_phase_B_current,
                    screw_phase_C_current,
                    screw_active_power,
                    screw_reactive_power
                )
                VALUES (
                    @record_time, @val1, @val2, @val3, @val4, @val5, @val6, @val7, @val8, @val9,
                    @val10, @val11, @val12, @val13, @val14, @val15, @val16, @val17, @val18, @val19,
                    @val20, @val21, @val22, @val23, @val24, @val25, @val26, @val27, @val28, @val29,
                    @val30, @val31, @val32, @val33, @val34, @val35, @val36, @val37, @val38, @val39,
                    @val40, @val41, @val42, @val43, @val44, @val45, @val46, @val47, @val48,@val49,
                    @val50,@val51,@val52,@val53,@val54,@val55,@val56,@val57,@val58,@val59,@val60
                );";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // 动态创建表
                    using (MySqlCommand createTableCmd = new MySqlCommand(createTableSql, connection))
                    {
                        createTableCmd.ExecuteNonQuery();
                    }

                    // 动态插入数据
                    using (MySqlCommand insertCmd = new MySqlCommand(insertSql, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@record_time", currentTime);
                        for (int i = 1; i <= 60; i++) // 绑定 60 个随机数参数
                        {
                            insertCmd.Parameters.AddWithValue($"@val{i}", randomValue());
                        }

                        insertCmd.ExecuteNonQuery();
                        Console.WriteLine($"[{currentTime}] 数据成功插入到表 {tableName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("插入数据时发生错误: " + ex.Message);
            }
        }
    }
}
