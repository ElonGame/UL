#include "stdafx.h"
#include "String.h"
#include <windows.h>
using namespace System;
String::String(const wchar_t* c)
{
	int size = wcslen(c) + 1;
	_v = new wchar_t[size];
	wmemset(_v, 0, size);
	wsprintf(_v, _T("%s"), c);
	_length = size - 1;
}

String::String(const char* wc)
{
	//��ȡ���뻺���С
	int sBufSize = strlen(wc);
	//��ȡ��������С
	//VC++ Ĭ��ʹ��ANSI����ȡ��һ������ΪCP_ACP
	DWORD dBufSize = MultiByteToWideChar(CP_ACP, 0, wc, sBufSize, NULL, 0)+1;
	_v = new wchar_t[dBufSize];
	wmemset(_v, 0, dBufSize);

	//����ת��
	int nRet = MultiByteToWideChar(CP_ACP, 0, wc, sBufSize, _v, dBufSize);

	_length = dBufSize-1;
}