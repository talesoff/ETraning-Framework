#pragma once

typedef union
{
	float m[16];
	float uvnw[4][4];
	struct { float u[4], v[4], n[4], w[4]; };
	struct { float m11, m21, m31, m41, m12, m22, m32, m42, m13, m23, m33, m43, m14, m24, m34, m44; };
} NativeMatrix;

typedef struct _NativeVector3
{
	float x, y, z;
} NativeVector3;