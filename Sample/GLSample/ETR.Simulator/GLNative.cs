using ETE.Geometry;
using System;
using System.Runtime.InteropServices;

namespace ETE.Simulator
{
    public class GLNative
    {
        [DllImport("GLNative.dll")]
        public static extern void GLStart();

        [DllImport("GLNative.dll")]
        public static extern void GLResize(int width, int height);

        [DllImport("GLNative.dll")]
        public static extern void GLClear();

        [DllImport("GLNative.dll")]
        public static extern void GLDraw(Matrix4x4 pos);

        [DllImport("GLNative.dll")]
        public static extern void GLFlush();
    }
}
