
using System;
using System.Windows;
using System.Windows.Interop;

using System.Collections.ObjectModel;
using System.Collections.Generic;

using ETE.MVC;
using ETE.Engine;
using ETE.Geometry;
using ETE.Render;
using ETE.Render.Processor;


namespace GTImporterSample
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
            renderSubsystem = SimulationApplication.Instance.getApp().AddSubsystem<GraphicsSubsystem>();
           
            InitializeComponent();

            Window_ListBox.ItemsSource = list_ViewWindow;

            this.Sample_Button.Click += SampleButton_Click;

            this.SizeChanged += MainWindow_SizeChanged;
            this.LocationChanged += MainWindow_LocationChanged;

            this.Closed += MainWindow_Closed;

            AddView();
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
            closeAllView();
        }

        private void closeAllView()
        {
            if (SimulationApplication.Instance.getisRun() == true)
                SimulationApplication.Instance.Stop();

            foreach (WindowItem item in list_ViewWindow)
            {
                item.window.Close();
            }

            System.Windows.Application.Current.Shutdown();
        }
        
        void AddView()
        {
            if (windowIndex >= 8 || SimulationApplication.Instance.getisRun() == true)
                return;

            GLDrawWindow newWindow = new GLDrawWindow(windowIndex);
            WindowItem item = new WindowItem();

            newWindow.Title = "Window_" + windowIndex;
            item.Title = "Window_" + windowIndex;
            item.Index = windowIndex;
            item.window = newWindow;
            list_ViewWindow.Add(item);

            newWindow.Left = this.Left + this.ActualWidth + (newWindow.Width * ((list_ViewWindow.Count-1)%3));
            newWindow.Top = this.Top + (newWindow.Height * ((list_ViewWindow.Count-1)/3));

            newWindow.Show();

            View temView = SimulationApplication.Instance.getApp().AddView<View>();

            EGLSubsystem eglSubsystem = SimulationApplication.Instance.getApp().AddSubsystem<EGLSubsystem>();
            EGLSwapBuffersSubsystem eglSwapBuffersSubsystem = SimulationApplication.Instance.getApp().AddSubsystem<EGLSwapBuffersSubsystem>();

            // eglSubsystem에 windowIndex 주입
            eglSubsystem.TargetDisplay = windowIndex;

            temView.AddSubsystem(eglSubsystem);
            temView.AddSubsystem(renderSubsystem);
            temView.AddSubsystem(eglSwapBuffersSubsystem);

            temView.SetOrder(eglSubsystem, 1);
            temView.SetOrder(renderSubsystem, 2);
            temView.SetOrder(eglSwapBuffersSubsystem, 3);

            windowIndex++;
        }

                
        void SampleButton_Click(object sender, RoutedEventArgs e)
        {           
            if (SimulationApplication.Instance.getisRun() == false)
              SimulationApplication.Instance.Start();
        }
    }
}
