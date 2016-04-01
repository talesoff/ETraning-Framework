// GLNative.cpp : DLL 응용 프로그램을 위해 내보낸 함수를 정의합니다.
//

#include "stdafx.h"

#include "GLES2/gl2.h"
#include "GeometryNative.h"

GLuint g_vertexShader = 0;
GLuint g_fragmentShader = 0;
GLuint g_programObject = 0;

int g_width = 0;
int g_height = 0;

GLint iMVP = -1;
GLint iPosition = -1;
GLint iColors = -1;

GLuint LoadShader(GLenum type, const GLchar *shaderSrc)
{
	GLuint shader;
	GLint compiled;
	// Create the shader object
	shader = glCreateShader(type);
	if (shader == 0)
		return 0;

	// Load the shader source
	glShaderSource(shader, 1, &shaderSrc, NULL);

	// Compile the shader
	glCompileShader(shader);

	// Check the compile status
	glGetShaderiv(shader, GL_COMPILE_STATUS, &compiled);
	if (!compiled)
	{
		glDeleteShader(shader);
		return 0;
	}
	return shader;
}

void InitShader()
{
	GLchar vShaderStr[] =
		"attribute vec4 vPosition; \n"
		"attribute vec3 vColor; \n"
		"uniform mat4 mMVP; \n"
		"varying vec4 fragColor; \n"
		"void main() \n"
		"{ \n"
		" fragColor = vec4(vColor, 1.0); \n"
		" gl_Position = mMVP * vPosition; \n"
		"} \n";
	GLchar fShaderStr[] =
		"precision mediump float; \n"
		"varying vec4 fragColor; \n"
		"void main() \n"
		"{ \n"
		" //gl_FragColor = vec4(0.5, 0.5, 0.5, 1.0); \n"
		" gl_FragColor = fragColor; \n"
		"} \n";
	GLint linked;

	// Load the vertex/fragment shaders
	g_vertexShader = LoadShader(GL_VERTEX_SHADER, vShaderStr);
	g_fragmentShader = LoadShader(GL_FRAGMENT_SHADER, fShaderStr);

	// Create the program object
	g_programObject = glCreateProgram();
	if (g_programObject == 0)
		return;
	glAttachShader(g_programObject, g_vertexShader);
	glAttachShader(g_programObject, g_fragmentShader);

	// Link the program
	glLinkProgram(g_programObject);
	
	// Check the link status
	glGetProgramiv(g_programObject, GL_LINK_STATUS, &linked);
	if (!linked)
	{
		glDeleteProgram(g_programObject);
		return;
	}

	iPosition = glGetAttribLocation(g_programObject, "vPosition");
	iColors = glGetAttribLocation(g_programObject, "vColor");
	iMVP = glGetUniformLocation(g_programObject, "mMVP");
}

void UninitShader()
{
	glDeleteShader(g_fragmentShader);
	glDeleteShader(g_vertexShader);
	glDeleteProgram(g_programObject);

	g_fragmentShader = 0;
	g_vertexShader = 0;
	g_programObject = 0;
}

extern "C" void NATIVE_EXPORT_API __stdcall GLStart()
{
	InitShader();
}

extern "C" void NATIVE_EXPORT_API __stdcall GLStop()
{
	UninitShader();
}

extern "C" void NATIVE_EXPORT_API __stdcall GLResize(int width, int height)
{
	if (g_width != width || g_height != height)
	{
		g_width = width;
		g_height = height;
		glViewport(0, 0, g_width, g_height);
	}
}

extern "C" void NATIVE_EXPORT_API __stdcall GLClear()
{
	glClearColor(0.1f, 0.1f, 0.2f, 1.0f);
	glClear(GL_COLOR_BUFFER_BIT);
}

extern "C" void NATIVE_EXPORT_API __stdcall GLDraw(NativeMatrix m)
{
	GLfloat vVertices[] = { 0.0f, 0.5f, 0.0f,
							-0.5f, -0.5f, 0.0f,
							0.5f, -0.5f, 0.0f };

	GLfloat vColors[] = { 1.0f, 0.0f, 0.0f,
						  0.0f, 1.0f, 0.0f,
						  0.0f, 0.0f, 1.0f };


	glUseProgram(g_programObject);

	// vertices
	glVertexAttribPointer(iPosition, 3, GL_FLOAT, GL_FALSE, 0, vVertices);
	glEnableVertexAttribArray(iPosition);

	// colors
	glVertexAttribPointer(iColors, 3, GL_FLOAT, GL_FALSE, 0, vColors);
	glEnableVertexAttribArray(iColors);

	// mvp
	glUniformMatrix4fv(iMVP, 1, GL_FALSE, m.m);

	glDrawArrays(GL_TRIANGLES, 0, 3);
}

extern "C" void NATIVE_EXPORT_API __stdcall GLFlush()
{
	glFlush();
}
