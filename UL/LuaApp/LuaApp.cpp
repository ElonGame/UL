// LuaApp.cpp: 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include "lua.hpp"



static void traceback(lua_State *L)
{
	const char *msg = lua_tostring(L, -1);
	if (msg)
	{
		/*打印发生错误时的堆栈信息,msg是错误信息,附加到打印的堆栈最前端
		1是指从第一层开始回溯
		这个函数之后,lua_tostring(L,-1)即可获取完整的堆栈和错误信息.
		*/
		printf("%s", msg);
		luaL_traceback(L, L, msg, 1);
	}
	else
	{
		return;
	}
}

void PrintLuaError(lua_State* L, int sErr)
{
	if (sErr == 0)
	{
		return;
	}
	const char* error;
	char sErrorType[256] = { 0 };
	switch (sErr)
	{
	case LUA_ERRSYNTAX://编译时错误  
					   /*const char *buf = "mylib.myfun()2222";类似这行语句可以引起编译时错误*/
		sprintf_s(sErrorType, sizeof(sErrorType), "%s", "syntax error during pre-compilation");
		break;
	case LUA_ERRMEM://内存错误  
		sprintf_s(sErrorType, sizeof(sErrorType), "%s", "memory allocation error");
		break;
	case LUA_ERRRUN://运行时错误  
					/*const char *buf = "my222lib.myfun()";类似这行语句可以引起运行时错误，my222lib实际上不存在这样的库，返回的值是nil*/
		sprintf_s(sErrorType, sizeof(sErrorType), "%s", "a runtime error");
		break;
	case LUA_YIELD://线程被挂起错误  
		sprintf_s(sErrorType, sizeof(sErrorType), "%s", "Thread has Suspended");
		break;
	case LUA_ERRERR://在进行错误处理时发生错误  
		sprintf_s(sErrorType, sizeof(sErrorType), "%s", "error while running the error handler function");
		break;
	default:
		break;
	}
	traceback(L);
}
int main()
{
	lua_State *L = luaL_newstate();
	luaL_openlibs(L);
	int error = luaL_dofile(L, "Lua//main.lua");
	if (error!=0)
	{
		PrintLuaError(L, error);
	}
	error = lua_getglobal(L, "main");
	if (error != 0)
	{
		PrintLuaError(L, error);
	}
	error = lua_pcall(L, 0, 0, 0);
	if (error != 0)
	{
		PrintLuaError(L, error);
	}
    return 0;
}

