#include "stdafx.h"

struct Vector3
{
	float x;
	float y;
	float z;
};

extern "C" NATIVE_EXPORT_API void __stdcall GeometryGetVectors(Vector3* ver, int size)
{
	for (int i = 0; i < size; i++)
	{
		ver[i].x *= 2;
		ver[i].y *= 2;
		ver[i].z *= 2;
	}
}