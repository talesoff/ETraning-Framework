using System.Runtime.InteropServices;
using System.Text;

namespace ETE.Simulator
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct VERSION
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string major;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string minor;
    }

    public class TextNative
    {
        [DllImport("GLNative.dll", CharSet = CharSet.Ansi)]
        public static extern void TextSetVersionStr(string ver);

        [DllImport("GLNative.dll", CharSet = CharSet.Ansi)]
        public static extern void TextGetVersionStr(StringBuilder buffer, int size);

        [DllImport("GLNative.dll")]
        public static extern void TextSetVersion(ref VERSION ver);

        [DllImport("GLNative.dll")]
        public static extern void TextGetVersion(ref VERSION ver);
    }

    public static class TextNativeTest
    {
        public static void Run()
        {
            TextNative.TextSetVersionStr("1.0.0.1");
            StringBuilder sb = new StringBuilder(256);
            TextNative.TextGetVersionStr(sb, 256);
            System.Diagnostics.Debug.WriteLine(sb.ToString());

            VERSION setVer = new VERSION();
            setVer.major = "가나다라";
            setVer.minor = "ABCD1234";
            TextNative.TextSetVersion(ref setVer);

            VERSION getVer = new VERSION();
            TextNative.TextGetVersion(ref getVer);
            System.Diagnostics.Debug.WriteLine(getVer.major + ", " + getVer.minor);
        }
    }
}
