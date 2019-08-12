// DllHelloWorld.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include <stdio.h>
#include <string>

extern "C"
{
	int test;
	__declspec(dllexport) int DisplayHelloFromDLL()
	{
		return test = 987;

	}
}
//https://www.codeproject.com/Articles/9826/How-to-create-a-DLL-library-in-C-and-then-use-it-w
//Please note that __declspec(dllexport) is an obligatory prefix which makes DLL functions available from an external application.
// extern “C”(with brackets) is also very important, it shows that all code within brackets is available from “outside”.Although code will compile even without this statement, during runtime, you’ll get a very unpleasant error.So, do not forget to include it.
