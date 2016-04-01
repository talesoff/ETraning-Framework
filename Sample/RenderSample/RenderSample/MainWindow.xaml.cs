using ETE.Render;
using System;
using System.Windows;
using System.Windows.Interop;

using System.Collections.ObjectModel;
using System.Collections.Generic;

using ETE.MVC;
using ETE.Engine;
using ETE.Geometry;
using ETE.Render.Processor;

namespace RenderSample
{
    // Window Item
    public class WindowItem
    {
        public string Title { get; set; }
        public int Index { get; set; }
        public Window window { get; set; }
    }
    
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

        public ObservableCollection<WindowItem> list_ViewWindow = new ObservableCollection<WindowItem>();
        GraphicsSubsystem renderSubsystem = null;
        int windowIndex = 0;
                
        public MainWindow()
        {
            // renderSubsystem은 모든 View에 동일한 인스턴스를 등록할 것이므로 하나만 생성
            renderSubsystem = SimulationApplication.Instance.getApp().AddSubsystem<GraphicsSubsystem>();
           
            InitializeComponent();

            Window_ListBox.ItemsSource = list_ViewWindow;

            this.AddView_Button.Click += AddViewButton_Click;
            this.RemoveView_Button.Click += RemoveViewButton_Click;
            this.RemoveAll_Button.Click += RemoveAllButton_Click;
            this.Show_Button.Click += ShowButton_Click;
            this.Hide_Button.Click += HideButton_Click;
            this.Start_Button.Click += StartButton_Click;
            this.Stop_Button.Click += StopButton_Click;

            this.SizeChanged += MainWindow_SizeChanged;
            this.LocationChanged += MainWindow_LocationChanged;

            this.Closed += MainWindow_Closed;

        }

        void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (WindowItem item in list_ViewWindow)
            {
                item.window.Left = this.Left + this.ActualWidth + (item.window.Width * ((list_ViewWindow.IndexOf(item)) % 3));
                item.window.Top = this.Top + (item.window.Height * ((list_ViewWindow.IndexOf(item)) / 3));
            }
        }

        private void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            foreach(WindowItem item in list_ViewWindow)
            {
                item.window.Left = this.Left + this.ActualWidth + (item.window.Width * ((list_ViewWindow.IndexOf(item)) % 3));
                item.window.Top = this.Top + (item.window.Height * ((list_ViewWindow.IndexOf(item)) / 3));
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (SimulationApplication.Instance.getisRun() == true)
                SimulationApplication.Instance.Stop();

            foreach(WindowItem item in list_ViewWindow)
            {
                item.window.Close();
            }

            System.Windows.Application.Current.Shutdown();
        }

        void AddViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (windowIndex >= 4 || SimulationApplication.Instance.getisRun() == true)
                return;

            // GL Rendering을 위한 새로운 window 창 생성
            GLDrawWindow newWindow = new GLDrawWindow(windowIndex);
            WindowItem item = new WindowItem();

            newWindow.Title = "Window_" + windowIndex;
            item.Title = "Window_" + windowIndex;
            item.Index = windowIndex;
            item.window = newWindow;
            list_ViewWindow.Add(item);

            newWindow.Left = this.Left + this.ActualWidth + (newWindow.Width * ((list_ViewWindow.Count-1)%3));
            newWindow.Top = this.Top + (newWindow.Height * ((list_ViewWindow.Count-1)/3));

            newWindow.Show(); // 새로 생긴 창을 Show 해줘야 Handle이 할당됨
            
            // Subsystem 생성
            EGLSubsystem eglSubsystem = SimulationApplication.Instance.getApp().AddSubsystem<EGLSubsystem>();
            EGLSwapBuffersSubsystem eglSwapBuffersSubsystem = SimulationApplication.Instance.getApp().AddSubsystem<EGLSwapBuffersSubsystem>();

            // eglSubsystem에 windowIndex 주입
            eglSubsystem.TargetDisplay = windowIndex;

            // View 구성
            View temView = SimulationApplication.Instance.getApp().AddView<View>();

            temView.AddSubsystem(eglSubsystem);
            temView.AddSubsystem(renderSubsystem);
            temView.AddSubsystem(eglSwapBuffersSubsystem);

            temView.SetOrder(eglSubsystem, 1);
            temView.SetOrder(renderSubsystem, 2);
            temView.SetOrder(eglSwapBuffersSubsystem, 3);
                        
            windowIndex++;
        }

        void RemoveViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (SimulationApplication.Instance.getisRun() == true)
                return;

            if (Window_ListBox.SelectedItem != null)
            {
                (Window_ListBox.SelectedItem as WindowItem).window.Close();
                list_ViewWindow.Remove(Window_ListBox.SelectedItem as WindowItem);
            }
        }

        void RemoveAllButton_Click(object sender, RoutedEventArgs e)
        {
            if (SimulationApplication.Instance.getisRun() == true)
                return;

            if(list_ViewWindow.Count != 0)
            {
                foreach(WindowItem wi in list_ViewWindow)
                {
                    wi.window.Close();
                }
                list_ViewWindow.Clear();                                
            }
        }

        void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            if(Window_ListBox.SelectedItem != null)
            {
                (Window_ListBox.SelectedItem as WindowItem).window.Show();
            }
        }

        void HideButton_Click(object sender, RoutedEventArgs e)
        {
            if (Window_ListBox.SelectedItem != null)
            {
                (Window_ListBox.SelectedItem as WindowItem).window.Hide();
            }
        }
        
        void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (SimulationApplication.Instance.getisRun() == true)
                return;

            SimulationApplication.Instance.Start();
        }

        void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (SimulationApplication.Instance.getisRun() == false)
                return;

            SimulationApplication.Instance.Stop();
        }
    }
}
