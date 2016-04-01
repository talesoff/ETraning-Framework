using ETE.Engine;
using ETE.ViewModel;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace GLSample
{
    /// <summary>
    /// SecondWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SecondWindow : Window
    {
        public static SecondWindow Current;
        DispatcherTimer FPSTimer;

        bool isRun = false;
        Thread simulationWorker;
        
        public SecondWindow()
        {
            InitializeComponent();

            this.Loaded += SecondWindow_Loaded;
            this.Closing += SecondWindow_Closing;
            this.Closed += SecondWindow_Closed;
            this.SourceInitialized += SecondWindow_SourceInitialized;

            LayoutRoot.DataContext = ETRViewModel.Instance;

            FPSTimer = new DispatcherTimer();
            FPSTimer.Tick += FPSTimer_Tick;
            FPSTimer.Interval = TimeSpan.FromSeconds(1);
            FPSTimer.Start();
        }

        private void SecondWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Current = this;
        }

        private void SecondWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void SecondWindow_Closed(object sender, EventArgs e)
        {
            isRun = false;
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            ETRViewModel.Instance.Pause();

            PauseButton.IsEnabled = false;
            ResumeButton.IsEnabled = true;
        }

        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            ETRViewModel.Instance.Resume();

            PauseButton.IsEnabled = true;
            ResumeButton.IsEnabled = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".json";
            dlg.Filter = "JSON (*.json)|*.json|Scene (*.scene)|*.scene|All files(*.*)|*.*";

            bool? result = dlg.ShowDialog();
            if (true == result.Value)
            {
                string fileName = dlg.FileName;

                using (FileStream stream = new FileStream(fileName, FileMode.Create))
                {
                    ETRViewModel.Instance.Save(stream);
                }
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".json";
            dlg.Filter = "JSON (*.json)|*.json|Scene (*.scene)|*.scene|All files(*.*)|*.*";

            bool? result = dlg.ShowDialog();
            if (true == result.Value)
            {
                string fileName = dlg.FileName;
                using (FileStream stream = new FileStream(fileName, FileMode.Open))
                {
                    ETRViewModel.Instance.Load(stream);
                }
            }
        }

        /// <summary>
        /// 스타트 버튼을 클릭했을 때 쓰레드를 생성하고 루프를 돌면서 함수를 호출합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.IsEnabled = false;

            isRun = true;
            simulationWorker = new Thread(() =>
                {
                    try
                    {
                        Stopwatch sw = new Stopwatch();

                        // 쓰레드가 생성될 때 엔진을 시작하도록 합니다.
                        ETRViewModel.Instance.Start();

                        // while 문을 돌면서 계속 함수를 호출합니다.
                        while (true == isRun)
                        {
                            sw.Restart();
                            ETRViewModel.Instance.AppUpdate();
                            while (sw.Elapsed.TotalMilliseconds < 1000.0f / ETRViewModel.FPS)
                            {
                                Thread.Sleep(0);
                            }
                        }

                        // while 문이 종료될 때 엔진을 종료하도록 합니다. 
                        ETRViewModel.Instance.Stop();
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.Message);
                        Trace.WriteLine(ex.StackTrace);

                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
                        }), null);
                    }
                    finally
                    {

                    }
                }
            );
            simulationWorker.Name = "SimulationWorker";
            simulationWorker.Priority = ThreadPriority.Highest;
            simulationWorker.Start();

            PauseButton.IsEnabled = true;
            ResumeButton.IsEnabled = false;
            StopButton.IsEnabled = true;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            StopButton.IsEnabled = false;

            isRun = false;
            simulationWorker.Join();
            simulationWorker = null;
            
            double h = Application.Current.MainWindow.ActualHeight;
            Application.Current.MainWindow.Height = h + 1;
            Application.Current.MainWindow.Height = h;

            StartButton.IsEnabled = true;
            PauseButton.IsEnabled = false;
            ResumeButton.IsEnabled = false;
        }

        private void FPSTimer_Tick(object sender, EventArgs e)
        {
            //if(true == FPSMeter.IsChecked)
            {
                float fps = ETRViewModel.Instance.UpdateFPS();
                if (true == ETRViewModel.Instance.IsPlaying)
                {
                    TotalElapsedText.Text = TimeSpan.FromSeconds((double)Time.TotalElapsedSeconds).ToString(@"hh\:mm\:ss");
                    FPSText.Text = fps.ToString("F2");                   
                }
                else
                {
                    TotalElapsedText.Text = "--:--:--";
                    FPSText.Text = "--.--";
                }
            }
        }

        private void SecondWindow_SourceInitialized(object sender, EventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            HwndSource source = HwndSource.FromHwnd(helper.Handle);
            source.AddHook(WndProc);
        }

        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MOVE = 0xF010;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_SYSCOMMAND:
                    int command = wParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                    {
                        handled = true;
                    }
                    break;
                default:
                    break;
            }
            return IntPtr.Zero;
        }
    }
}
