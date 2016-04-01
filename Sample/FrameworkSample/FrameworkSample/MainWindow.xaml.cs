using ETR.Simulator;
using System;
using System.Windows;
using System.Windows.Interop;

namespace FrameworkSample
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        WindowInteropHelper helper;

        public MainWindow()
        {
            InitializeComponent();
            helper = new WindowInteropHelper(this);

            this.Loaded += MainWindow_Loaded;
            this.SizeChanged += MainWindow_SizeChanged;
            this.StateChanged += MainWindow_StateChanged;
            this.Closed += MainWindow_Closed;
            this.LocationChanged += MainWindow_LocationChanged;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Vector res = new System.Windows.Vector(this.ActualWidth, this.ActualHeight);
            PresentationSource presentationsource = PresentationSource.FromVisual(this);
            res = presentationsource.CompositionTarget.TransformToDevice.Transform(res);

            GraphicsSubsystem.NativeHandle = helper.Handle;
            GraphicsSubsystem.WindowWidth = (int)res.X;
            GraphicsSubsystem.WindowHeight = (int)res.Y;

            SecondWindow second = new SecondWindow();
            second.Top = this.Top;
            second.Left = this.Left + this.ActualWidth;
            second.Owner = this;
            second.Show();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            //if (false == ETRViewModel.Instance.IsPlaying)
            //{
            //    return;
            //}

            switch (WindowState)
            {
                case WindowState.Minimized:
                    //ETE.Engine.Application.Current.Pause();
                    break;

                case WindowState.Normal:
                    //ETE.Engine.Application.Current.Resume();
                    break;

                case WindowState.Maximized:
                    //ETE.Engine.Application.Current.Resume();
                    break;
            }
        }

        void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            System.Windows.Vector res = WindowUtil.TransformToDevice(new Vector(ActualWidth, this.ActualHeight), this);
            GraphicsSubsystem.WindowWidth = (int)res.X;
            GraphicsSubsystem.WindowHeight = (int)res.Y;

            RECT rc;
            WindowUtil.GetWindowRect(helper.Handle, out rc);

            Vector v = WindowUtil.TransformFromDevice(new Vector(rc.Right, rc.Top), this);
            
            SecondWindow second = SecondWindow.Current;
            if (null != second)
            {
                if (this.WindowState == WindowState.Normal)
                {
                    second.Left = v.X;
                    second.Top = v.Y;
                }
                else if (this.WindowState == WindowState.Maximized)
                {
                    Vector titleArea = WindowUtil.TransformToDevice(new Vector(0, (SystemParameters.CaptionHeight + SystemParameters.FocusBorderHeight)), this);

                    second.Left = v.X - second.ActualWidth;
                    second.Top = v.Y + titleArea.Y;
                }
            }
        }

        private void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            SecondWindow second = SecondWindow.Current;
            if (null != second)
            {
                second.Left = this.Left + this.ActualWidth;
                second.Top = this.Top;
            }
        }
    }
}
