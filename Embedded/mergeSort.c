#include <stdio.h>
#include <stdlib.h>
#include "mergeSort.h"

void merge(float* a, float* aux, unsigned int left, unsigned int right, unsigned int rightEnd)
{
	unsigned int leftEnd = right-1;
	unsigned int temp = left;
	unsigned int num = rightEnd - left+1;

	while((left<=leftEnd)&&(right<=rightEnd))
	{
		if(a[left] <= a[right])
			aux[temp++] = a[left++];
		else
			aux[temp++] = a[right++];
	}

	while(left <= leftEnd)
		aux[temp++] = a[left++];

	while(right <= rightEnd)
	{
		aux[temp++] = a[right++];
	}

	for (unsigned int i=1; i<=num; i++, rightEnd--)
		a[rightEnd] = aux[rightEnd];
}

void mergeSort(float* a, float* aux, unsigned int left, unsigned int right)
{
	if(left < right)
	{
		unsigned int center = (left + right) / 2;

		mergeSort(a, aux, left, center);
		mergeSort(a, aux,center + 1, right);
		merge(a, aux, left, center + 1, right);
	}
}

void mergeSort_sortFloatArray(float* buffer, unsigned int n)
{
	float* tempBuffer = (float*)malloc(sizeof(float) * n);
	mergeSort(buffer, tempBuffer, 0, n-1);
	free(tempBuffer);
}