#define _CRT_SECURE_NO_WARNINGS 1
#include <stdio.h>

//计算n的阶乘，以及计算1！+2!+3!+4!+5!+...+n!
int main()
{
	int flag = 1;
	int i = 0;
	int j = 0;
	int sum = 0;
	int n = 0;
	scanf("%d", &n);
	for (j = 1; j <= n; j++)
	{
		flag = 1;
		for (i = 1; i <= j; i++)
		{
			flag *= i;
		}
		sum += flag;
	}
	printf("%d\n", sum); 
	return 0;
}