#define _CRT_SECURE_NO_WARNINGS 1
#include <stdio.h>
//冒泡排序
//思想：相邻的两个元素比较，如果不满足顺序就交换
//void BubbleSort(int* arr,int sz)
//{
//	int i = 0;
//	for (i = 0; i < sz-1; i++)
//	{
//		int flag = 1;//假设已经有序
//		int j = 0;
//		for (j = 0;j<sz-i-1;j++)
//		{
//			//一对元素的比较
//			if (arr[j] > arr[j + 1])
//			{
//				//交换
//				int tmp = arr[j];
//				arr[j] = arr[j + 1];
//				arr[j + 1] = tmp;
//				flag = 0;
//			}
//			if (flag == 1)
//			{
//				break;
//			}//这一步是为了避免本身有序的时候，时间的浪费
//		}
//	}
//}
//int main()
//{
//	int arr[] = { 9,8,7,6,5,4,3,2,1,0 };//降序
//	//我们想把数组进行排序，排位升序
//	int sz = sizeof(arr) / sizeof(arr[0]);
//	BubbleSort(arr,sz);
//	int n = 0;
//	for (int n = 0; n < sz; n++)
//	{
//		printf("%d ", arr[n]);
//	}
//	return 0;
//}


//二级指针
//int main()
//{
//	int a = 10;
//	int* p = &a;//取出a的地址
//	//p是指针变量，是一级指针
//	int* * pp = &p;//pp是二级指针,二级指针变量是用来存放一级指针变量的地址
//	int** * ppp = &pp;//ppp是三级指针
//
//	printf("%d\n", **pp);
//	return 0;
//}

//指针数组   本质上是个数组
//整形数组  存放整形数据的数组（数组中的每个元素是整形元素）
//字符数组  存放字符数据的数组（数组中的每个元素是字符类型）
//指针数组  存放指针的数组     （数组中的每个元素是指针类型）


//指针数组模拟出二位数组的效果，但是不是二维数组
//int main()
//{
//	int arr1[] = { 1,2,3,4,5 };
//	int arr2[] = { 2,3,4,5,6 };
//	int arr3[] = { 3,4,5,6,7 };
//	int* arr[3] = { arr1,arr2,arr3 };
//	int i = 0;
//	for (i = 0; i < 3; i++)
//	{
//		int j = 0;
//		for (j = 0; j < 5; j++)
//		{
//			printf("%d ", arr[i][j]);
//		}
//		printf("\n");
//	}
//	return 0;
//}

//字符指针变量
//int main()
//{
//	const char* p = "abcedf";//不是把字符串abcedf\0存放在p中，而是把第一个字符的地址存放在p中
//	printf("%c\n", *p);
//
//	//1.把字符串想象为一个字符数组,但是这个数组是不能修改的
//	//2.当常量字符串出现在表达式中的时候，他的值是第一个字符的地址
//	
//	printf("%c\n", "abcdef"[3]);// 和printf("%c\n,p[3]);等价
//	//p[3] = 'q';这个改的操作是不允许的
//
//
//	return 0;
//}

//数组指针变量
//存放的是数组的地址，能够指向数组的指针变量
//int main()
//{
//	int arr[10] = { 1,2,3,4,5,6,7,8,9,10 };
//	int (*p)[10] = &arr;
//
//	printf("%p\n", arr);
//	printf("%p\n", arr + 1);//数据
//
//	printf("%p\n", p);
//	printf("%p\n", p+1);//整个数组
//	return 0;
//}

