// LuaApp.cpp: 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include "lua.hpp"

int main()
{
	lua_State *L = luaL_newstate();
	luaL_openlibs(L);
	int error = luaL_dofile(L, "Lua//main.lua");
	if (error!=0)
	{
		printf("出错\n");
	}
	lua_getglobal(L, "main");
	lua_pcall(L, 0, 0, 0);

    return 0;
}

