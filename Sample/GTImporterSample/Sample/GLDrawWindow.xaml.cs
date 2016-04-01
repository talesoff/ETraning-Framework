using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Text;

using ETE.Render.Processor;

namespace GTImporterSample
{
    /// <summary>
    /// GLDrawWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GLDrawWindow : Window
    {

        public GLDrawWindow(int _targetDisplay)
        {
            SourceInitialized += MainWindow_SourceInitialized;
            InitializeComponent();
            
            GL_WindowsFormsHost.Child = ETE.Render.EGL.GLUserControlManager.Instance.addGLUserControl(_targetDisplay);            
        }

        void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            WindowInteropHelper wih = new WindowInteropHelper(this);
            int style = GetWindowLong(wih.Handle, GWL_STYLE);
            SetWindowLong(wih.Handle, GWL_STYLE, style & ~WS_SYSMENU);
        }

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x00080000;

        [DllImport("user32.dll")]
        private extern static int SetWindowLong(IntPtr hwnd, int index, int value);
        [DllImport("user32.dll")]
        private extern static int GetWindowLong(IntPtr hwnd, int index);
    }
}
