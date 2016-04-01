#include "stdafx.h"

#include <string>

const int LEN = 64;
#pragma pack(push, 1)
struct VERSION
{
	char major[LEN];
	char minor[LEN];
};
#pragma pack(pop)   

std::string g_versionStr = "";
VERSION g_version = {};

extern "C" NATIVE_EXPORT_API void __stdcall TextSetVersionStr(const char* ver)
{
	g_versionStr = ver;
}

extern "C" NATIVE_EXPORT_API void __stdcall TextGetVersionStr(char* buffer, int size)
{
	strcpy_s(buffer, size, g_versionStr.c_str());
}

extern "C" NATIVE_EXPORT_API void __stdcall TextSetVersion(const VERSION* ver)
{
	strcpy_s(g_version.major, LEN, ver->major);
	strcpy_s(g_version.minor, LEN, ver->minor);
}

extern "C" NATIVE_EXPORT_API void __stdcall TextGetVersion(VERSION* ver)
{
	strcpy_s(ver->major, LEN, g_version.major);
	strcpy_s(ver->minor, LEN, g_version.minor);
}



