// FSBCaller.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "FSBDll.h"
#pragma comment(lib,"FSBDll.lib")

int _tmain(int argc, _TCHAR* argv[])
{
	int k=fnFSBDll();
	char *strCommend[4]={
		"CallFSBDLL",
		"-d",
		"F:\\LOLSound\\Sound3",
		"F:\\LOLSound\\GameMusicEvents_bank00.fsb"
	};
	ExtractFDBFile(4,strCommend);
	const char *Buffer=GetOutBuffer();
	return 0;
}

