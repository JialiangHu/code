# 导入常用的数据处理与机器学习相关库
import pandas as pd  # 用于读取和处理表格数据
import numpy as np   # 用于数值运算
from sklearn.model_selection import train_test_split  # 划分训练/测试集
from sklearn.preprocessing import LabelEncoder, OneHotEncoder, StandardScaler
from sklearn.compose import ColumnTransformer
from sklearn.pipeline import Pipeline
from sklearn.impute import SimpleImputer
from sklearn.ensemble import RandomForestRegressor           # 随机森林（回归问题）
from sklearn.linear_model import LinearRegression           # 线性回归（也可选）
from sklearn.metrics import mean_squared_error, r2_score    # 评估指标

import matplotlib
matplotlib.use('TkAgg')  # 或 'Qt5Agg'
import matplotlib.pyplot as plt


import seaborn as sns
import warnings
warnings.filterwarnings('ignore')

# 设置 matplotlib 显示中文（可选，如果数据中有中文需要显示的话）
plt.rcParams['font.sans-serif'] = ['SimHei']  # 用来正常显示中文标签
plt.rcParams['axes.unicode_minus'] = False    # 用来正常显示负号

# 检查文件路径是否正确，然后加载 Excel 文件
file_path = r'C:\Users\28626\Desktop\archive\bi.csv'  # 使用原始字符串，避免转义问题

# 尝试读取 Excel 文件
df = pd.read_csv(file_path,encoding = 'gbk')  # 如果是 .csv 文件，用 read_csv
print("文件读取成功！")


##下面这部分是对数据展示，查看数据的大致特征
# # 查看数据前几行，确认内容
# print("✅ 数据加载成功！以下是前 5 行数据：")
# print(df.head())
#
#
# # 查看数据基本信息：列名、非空数量、数据类型等
# print("\n=== 数据基本信息 ===")
# print(df.info())
#
# # 查看数据集行数和列数
# print(f"\n数据集共有 {df.shape[0]} 行，{df.shape[1]} 列")
#
# # 查看数据集前 5 行，了解数据大致内容
# print("\n=== 数据前 5 行 ===")
# print(df.head())
#
# # 查看数据集后 5 行（可选）
# print("\n=== 数据后 5 行 ===")
# print(df.tail())
#
# # 查看数值型特征的统计描述（比如均值、标准差、最小值、最大值等）
# print("\n=== 数值型特征的统计描述 ===")
# print(df.describe())  # 只展示数值列的统计信息
#
# # 检查每列的缺失值情况
# print("\n=== 各列缺失值数量 ===")
# print(df.isnull().sum())
#
# # 查看数据集中都有哪些列名（特征名称）
# print("\n=== 所有列名（特征）===")
# print(df.columns.tolist())


# 处理缺失值
# 用中位数填充（适合有离群点的成绩数据）
df['Python'] = df['Python'].fillna(df['Python'].median())


# 统一分类变量格式
# 性别统一为Male/Female
df['gender'] = df['gender'].str.capitalize().replace({'M': 'Male', 'F': 'Female'})
# 教育背景统一（示例）
edu_map = {'HighSchool': 'High School', 'Diplomaaa': 'Diploma'}
df['prevEducation'] = df['prevEducation'].replace(edu_map)


# 特征编码(将文字转为数字)
# 对分类变量进行标签编码（也可用独热编码）
from sklearn.preprocessing import LabelEncoder
le = LabelEncoder()
df['gender_encoded'] = le.fit_transform(df['gender'])  # Male=1, Female=0
df['edu_encoded'] = le.fit_transform(df['prevEducation'])  # High School=0, Diploma=1,...
# 查看编码映射（重要！）
print("教育背景编码映射:", dict(zip(le.classes_, le.transform(le.classes_))))


# 特征选择与目标变量定义
# 选择特征（数值型+编码后的分类变量）
features = ['Age', 'entryEXAM', 'studyHOURS', 'gender_encoded', 'edu_encoded']
X = df[features]
# 目标变量（假设预测Python成绩）
y = df['Python']

# 划分训练集和测试集
from sklearn.model_selection import train_test_split
# 按7:3划分（random_state保证可复现）
X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.3, random_state=42
)
print("训练集样本:", X_train.shape[0], "测试集样本:", X_test.shape[0])



# 如果是回归问题，预测python具体分数  选择这部分代码
from sklearn.linear_model import LinearRegression
from sklearn.metrics import mean_squared_error, r2_score

# 训练模型
lr = LinearRegression()
lr.fit(X_train, y_train)

# 预测测试集
y_pred = lr.predict(X_test)

# 评估
print("MSE:", mean_squared_error(y_test, y_pred))
print("R²:", r2_score(y_test, y_pred))  # 越接近1越好

#随机森林
from sklearn.ensemble import RandomForestRegressor
rf = RandomForestRegressor(n_estimators=100, random_state=42)
rf.fit(X_train, y_train)
# 评估
y_pred_rf = rf.predict(X_test)
print("RF R²:", r2_score(y_test, y_pred_rf))


# 如果是分类问题判断是否优秀
# 定义优秀标准（假设Python≥80分为优秀）
df['is_excellent'] = (df['Python'] >= 80).astype(int)
y_cls = df['is_excellent']
# 重新划分数据集
X_train, X_test, y_train, y_test = train_test_split(
    X, y_cls, test_size=0.3, random_state=42
)
# 训练逻辑回归
from sklearn.linear_model import LogisticRegression
logreg = LogisticRegression()
logreg.fit(X_train, y_train)
# 评估
from sklearn.metrics import accuracy_score, confusion_matrix
y_pred_cls = logreg.predict(X_test)
print("准确率:", accuracy_score(y_test, y_pred_cls))
print("混淆矩阵:\n", confusion_matrix(y_test, y_pred_cls))


#随机森林分类
from sklearn.ensemble import RandomForestClassifier
rfc = RandomForestClassifier(n_estimators=100)
rfc.fit(X_train, y_train)
# 评估
y_pred_rfc = rfc.predict(X_test)
print("RF准确率:", accuracy_score(y_test, y_pred_rfc))
# 查看哪些特征最重要
importances = rf.feature_importances_
feat_importance = pd.DataFrame({
    'Feature': X.columns,
    'Importance': importances
}).sort_values('Importance', ascending=False)

print(feat_importance)
# 可视化
plt.barh(feat_importance['Feature'], feat_importance['Importance'])
plt.title('Feature Importance')
plt.show()


#超参数调优(以随机森林为例)
from sklearn.model_selection import GridSearchCV
# 定义参数网格
param_grid = {
    'n_estimators': [50, 100, 200],
    'max_depth': [None, 5, 10]
}
# 网格搜索
grid_search = GridSearchCV(
    estimator=RandomForestRegressor(random_state=42),
    param_grid=param_grid,
    cv=5  # 5折交叉验证
)
grid_search.fit(X_train, y_train)
# 最佳参数
print("最佳参数:", grid_search.best_params_)
best_model = grid_search.best_estimator_

# 直接使用网格搜索得到的最佳模型（推荐）
best_model = grid_search.best_estimator_

# 评估最终模型性能
y_pred = best_model.predict(X_test)
print("优化后模型 R²:", r2_score(y_test, y_pred))


#模型部署与使用
import joblib
# 保存随机森林模型
joblib.dump(rf, 'python_score_predictor.pkl')
# 加载模型（在新环境中）
loaded_model = joblib.load('python_score_predictor.pkl')