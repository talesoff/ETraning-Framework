using System;
using System.Runtime.InteropServices;

namespace ETR.Simulator
{
    public class EGLNative
    {
        [DllImport("GLNative.dll")]
        public static extern bool EGLCreate(IntPtr handle);

        [DllImport("GLNative.dll")]
        public static extern void EGLFlush();

        [DllImport("GLNative.dll")]
        public static extern void EGLDestroy();
    }
}
