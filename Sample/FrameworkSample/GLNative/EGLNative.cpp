#include "stdafx.h"

#include "EGL/egl.h"
#include <string>

EGLDisplay eglDisplay = 0;
EGLConfig eglConfig = 0;
EGLSurface eglSurface = 0;
EGLContext eglContext = 0;

const EGLint configurationAttributes[] =
{
	EGL_SURFACE_TYPE, EGL_WINDOW_BIT,
	EGL_RENDERABLE_TYPE, EGL_OPENGL_ES2_BIT,
	EGL_NONE
};

EGLint contextAttributes[] =
{
	EGL_CONTEXT_CLIENT_VERSION, 2,
	EGL_NONE
};

HWND g_hWnd = NULL;

bool testEGLError(const char* functionLastCalled, int line)
{
	/*	eglGetError returns the last error that occurred using EGL, not necessarily the status of the last called function. The user has to
	check after every single EGL call or at least once every frame. Usually this would be for debugging only, but for this example
	it is enabled always.
	*/
	EGLint lastError = eglGetError();
	if (lastError != EGL_SUCCESS)
	{
		char stringBuffer[256];
		sprintf_s(stringBuffer, "EGL last error: %d, %s(%d)", lastError, functionLastCalled, line);
		OutputDebugStringA(stringBuffer);
		return false;
	}

	return true;
}

extern "C" bool NATIVE_EXPORT_API __stdcall EGLCreate(void* handle)
{
	g_hWnd = (HWND)handle;

	eglDisplay = eglGetDisplay(EGL_DEFAULT_DISPLAY);
	testEGLError(__FUNCTION__, __LINE__);

	EGLint  ver_maj;
	EGLint  ver_min;
	if (!eglInitialize(eglDisplay, &ver_maj, &ver_min))
	{
		testEGLError(__FUNCTION__, __LINE__);
		return false;
	}

	EGLint configsReturned = 0;
	EGLBoolean r = EGL_FALSE;
	r = eglChooseConfig(eglDisplay, configurationAttributes, &eglConfig, 1, &configsReturned);
	testEGLError(__FUNCTION__, __LINE__);
	eglSurface = eglCreateWindowSurface(eglDisplay, eglConfig, g_hWnd, NULL);
	testEGLError(__FUNCTION__, __LINE__);
	eglContext = eglCreateContext(eglDisplay, eglConfig, EGL_NO_CONTEXT, contextAttributes);
	testEGLError(__FUNCTION__, __LINE__);
	r = eglMakeCurrent(eglDisplay, eglSurface, eglSurface, eglContext);
	testEGLError(__FUNCTION__, __LINE__);
	r = eglSwapInterval(eglDisplay, 0);
	testEGLError(__FUNCTION__, __LINE__);

	return true;
}

extern "C" void NATIVE_EXPORT_API __stdcall EGLFlush()
{
	eglSwapBuffers(eglDisplay, eglSurface);
}

extern "C" void NATIVE_EXPORT_API __stdcall EGLDestroy()
{
	EGLBoolean r = EGL_TRUE;
	r = eglMakeCurrent(eglDisplay, 0, 0, 0);
	testEGLError(__FUNCTION__, __LINE__);
	r = eglDestroyContext(eglDisplay, eglContext);
	testEGLError(__FUNCTION__, __LINE__);
	r = eglDestroySurface(eglDisplay, eglSurface);
	testEGLError(__FUNCTION__, __LINE__);
	r = eglTerminate(eglDisplay);
	testEGLError(__FUNCTION__, __LINE__);
}