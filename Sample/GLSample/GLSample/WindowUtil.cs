using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;

namespace GLSample
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public int Width
        {
            get
            {
                return Right - Left;
            }
        }

        public int Height
        {
            get
            {
                return Bottom - Top;
            }
        }

    }

    public class WindowUtil
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        
        public static Vector TransformToDevice(Vector v, Visual visual)
        {
            Vector r = new System.Windows.Vector(v.X, v.Y);
            PresentationSource presentationsource = PresentationSource.FromVisual(visual);
            r = presentationsource.CompositionTarget.TransformToDevice.Transform(r);
            return r;
        }

        public static Vector TransformFromDevice(Vector v, Visual visual)
        {
            Vector r = new System.Windows.Vector(v.X, v.Y);
            PresentationSource presentationsource = PresentationSource.FromVisual(visual);
            r = presentationsource.CompositionTarget.TransformFromDevice.Transform(r);
            return r;
        }
    }
}
