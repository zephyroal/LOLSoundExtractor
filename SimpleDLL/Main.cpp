#include <iostream>

extern "C" __declspec(dllexport) int add(int x, int y)
{
	return x+y;
}
