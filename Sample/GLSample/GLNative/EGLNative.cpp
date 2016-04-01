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

extern "C" bool NATIVE_EXPORT_API __stdcall EGLCreate(void* handle)
{
	g_hWnd = (HWND)handle;

	eglDisplay = eglGetDisplay(EGL_DEFAULT_DISPLAY);

	EGLint  ver_maj;
	EGLint  ver_min;
	if (!eglInitialize(eglDisplay, &ver_maj, &ver_min))
	{
		return false;
	}

	EGLint configsReturned = 0;
	EGLBoolean r = EGL_FALSE;
	eglChooseConfig(eglDisplay, configurationAttributes, &eglConfig, 1, &configsReturned);
	eglSurface = eglCreateWindowSurface(eglDisplay, eglConfig, g_hWnd, NULL);
	eglContext = eglCreateContext(eglDisplay, eglConfig, EGL_NO_CONTEXT, contextAttributes);
	eglMakeCurrent(eglDisplay, eglSurface, eglSurface, eglContext);
	eglSwapInterval(eglDisplay, 0);

	return true;
}

extern "C" void NATIVE_EXPORT_API __stdcall EGLFlush()
{
	eglSwapBuffers(eglDisplay, eglSurface);
}

extern "C" void NATIVE_EXPORT_API __stdcall EGLDestroy()
{
	eglMakeCurrent(eglDisplay, 0, 0, 0);
	eglDestroyContext(eglDisplay, eglContext);
	eglDestroySurface(eglDisplay, eglSurface);
	eglTerminate(eglDisplay);
}