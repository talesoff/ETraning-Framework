using ETE.Engine;
using ETE.MVC;
using ETE.Script;
using ETE.Simulator;
using ETR.Simulator;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace ETE.ViewModel
{
    public class FPSMeter
    {
        public float FPS
        {
            get; set;
        }
    }

    public class ETRViewModel : INotifyPropertyChanged
    {
        Application App;

        Model model;
        Controller controller;
        View view;

        public static float FPS
        {
            get { return 60.0f; }
        }

        public ObservableCollection<FPSMeter> FPSList { get; set; }
        public float FPSThreshold
        {
            get
            {
                return FPS - 1f;
            }
        }

        private int count = 0;
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
                NotifyPropertyChanged();
            }
        }

        private static ETRViewModel instance = null;
        public static ETRViewModel Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new ETRViewModel();
                }
                return instance;
            }
        }

        private ETRViewModel()
        {
            FPSList = new ObservableCollection<FPSMeter>();
        }

        /// <summary>
        /// 코어 엔진의 어플리케이션을 업데이트 합니다.
        /// 쓰레드에서 호출됩니다.
        /// </summary>
        public void AppUpdate()
        {
            if (null != App)
            {
                App.Update();
            }
        }

        private void MakeDefault()
        {
            App = new Application();
            model = App.SetModel<Model>();
            controller = App.SetController<Controller>();
            view = App.AddView<View>();
        }

        private void MakeDefaultSystem()
        {
            // Controller에 프로세서를 연결하고 실행 순서를 지정합니다.
            controller.AddProcessor<UserUpdateProcessor>();
            controller.AddProcessor<UserLateUpdateProcessor>();
            controller.AddProcessor<RenderProcessor>();

            controller.SetOrder(typeof(UserUpdateProcessor), 0);
            controller.SetOrder(typeof(UserLateUpdateProcessor), 1);
            controller.SetOrder(typeof(RenderProcessor), 2);

            // Subsystem을 생성하고 View에 Subsystem을 추가합니다.
            GraphicsSubsystem gss = App.AddSubsystem<GraphicsSubsystem>();
            view.AddSubsystem(gss);
            view.SetOrder(gss, 0);
        }

        private void MakeSimulator()
        {
            MakeDefault();
            MakeDefaultSystem();

            // Model에 시뮬레이션 오브젝트를 추가하고 컴포넌트를 연결합니다.
            SimulationObject SimObj = model.AddObject();
            SimObj.Name = "Triangle";
            SimObj.AddComponent<GLDrawComponent>();
            SimObj.AddComponent<TransformBehavior>();
        }

        public void Save(Stream stream)
        {
            if (null != App)
            {
                JsonSerializer js = new JsonSerializer();
                js.stream = stream;
                App.Model.Save(js);
            }
        }

        public void Load(Stream stream)
        {
            if (null == App)
            {
                MakeDefault();

                JsonSerializer js = new JsonSerializer();
                js.stream = stream;

                App.Model.Load(js);

                MakeDefaultSystem();
            }
        }

        public void Start()
        {
            if (null == App)
            {
                MakeSimulator();
            }

            App.Start();
        }

        public void Stop()
        {
            if (null == App)
            {
                return;
            }

            App.Stop();
            Reset();
        }

        public void Pause()
        {
            if (null == App)
            {
                return;
            }

            App.Pause();
        }

        public void Resume()
        {
            if (null == App)
            {
                return;
            }

            App.Resume();
        }

        public bool IsPlaying
        {
            get
            {
                if (null == App)
                {
                    return false;
                }

                return App.IsPlaying;
            }
        }

        private void Reset()
        {
            App = null;
            model = null;
            controller = null;
            view = null;
        }

        public float UpdateFPS()
        {
            float fps = Time.FPS;
            if (fps > 0 && fps < FPSThreshold)
            {
                //Debugger.Break();
            }
            FPSMeter fpsMeter = null;
            if (FPSList.Count > 90)
            {
                fpsMeter = FPSList[90];
                FPSList.RemoveAt(90);
            }

            if (null == fpsMeter)
            {
                fpsMeter = new FPSMeter();
            }
            fpsMeter.FPS = fps;
            FPSList.Insert(0, fpsMeter);

            return fps;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
