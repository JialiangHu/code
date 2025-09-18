#define _CRT_SECURE_NO_WARNINGS 1
#include <stdio.h>
//二位数组传参，形参写的是二维数组
//void Print(int arr[3][5], int r, int c)
//{
//	int i = 0;
//	for (i = 0; i < r; i++)
//	{
//		for (int j = 0; j < c; j++)
//		{
//			printf("%d ", arr[i][j]);
//		}
//		printf("\n");
//	}
//}

//void print(int (*arr)[5],int r,int c)
//{
//	int i = 0;
//	for (i = 0; i < r; i++)
//	{
//		for (int j = 0; j < c; j++)
//		{
//			printf("%d ", arr[i][j]);
//		}
//		printf("\n");
//	}
//}
//int main()
//{
//	int arr[3][5] = { {1,2,3,4,5},{2,3,4,5,6},{3,4,5,6,7} };
//	Print(arr,3,5);//打印arr数组的内容
//	return 0;
//}

//一维数组传参，形参可以是数组，也可以是指针为什么？
//写成数组更直观，为了方便理解
//写成指针是因为数组传参，传递的是数组第一个元素的地址

//二维数组传参，形参写成数组也是可以的，但是能写成指针吗？可以！
//二维数组其实是元素都为一维数组的数组
//对于二维数组来说，首元素其实就是第一行，首元素地址其实就是第一行的地址


//函数指针变量  存放函数地址的
int  add(int x, int y)
{
	return x + y;
}
int main()
{
	int a = 0;
	int* pa = &a;//整形指针

	int arr[10] = { 0 };
	int (*parr)[10] = &arr;//数组指针

	printf("%p\n", &add);
	printf("%p\n", add);
	
	//函数名是函数的地址，&函数名仍然是函数的地址，和数组指针有区别

	int (*pf)(int , int) = &add;//pf就是函数指针变量
	int ret = add(3, 5);
	printf("%d\n", ret);

	int ret2 = (*pf)(4, 5);
	printf("%d\n", ret2);

	int (*pf2)(int, int) = add;
	int ret3 = (*pf)(5, 6);
	printf("%d\n", ret3);

	int ret4 = pf(6, 6);
	printf("%d\n", ret4);

	return 0;
}