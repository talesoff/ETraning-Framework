using ETE.Engine;
using ETE.MVC;
using ETR.Simulator;
using FrameworkSample.Script;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace FrameworkSample.ViewModel
{
    public class FPSMeter
    {
        public float FPS { get; set; }
    }

    public class LogInfo
    {
        public string Log { get; set; }
        public DateTime Time { get; set; }
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

        /// <summary>
        /// 시뮬레이션 작업 결과를 저장하기 위한 버퍼
        /// 시뮬레이션 쓰레드에서 기록합니다.
        /// </summary>
        private List<LogInfo> LogBuffer = new List<LogInfo>();

        /// <summary>
        /// 시뮬레이션 객체를 저장하기 위한 버퍼
        /// 시뮬레이션 쓰레드에서 기록합니다.
        /// </summary>
        private List<SimulationObject> ObjectBuffer = new List<SimulationObject>();

        /// <summary>
        /// UI 연동 콜렉션
        /// FPSListView의 ItemSource입니다.
        /// </summary>
        public ObservableCollection<LogInfo> LogList { get; private set; }

        /// <summary>
        /// UI 연동 콜렉션
        /// LogListView의 ItemSource입니다.
        /// </summary>
        public ObservableCollection<FPSMeter> FPSList { get; private set; }
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
            LogList = new ObservableCollection<LogInfo>();
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
            AddObject();
        }

        private float scale = 1;
        public void AddObject()
        {
            // Model에 시뮬레이션 오브젝트를 추가하고 컴포넌트를 연결합니다.
            SimulationObject SimObj = model.AddObject();
            SimObj.Name = string.Format("Triangle{0}", SimObj.GetInstanceID().ToString("D3"));
            SimObj.AddComponent<GLDrawComponent>();
            SimObj.AddComponent<TransformBehavior>();
            SimObj.GetComponent<Transform>().LocalScale = new ETE.Geometry.Vector3(scale, scale, scale);
            scale *= 0.95f;

            // 오브젝트 관리
            ObjectBuffer.Add(SimObj);
            // 작업 결과를 버퍼에 추가
            LogBuffer.Add(new LogInfo() { Log = "Add " + SimObj.Name, Time = DateTime.Now });
        }

        public void RemoveObject()
        {
            int count = ObjectBuffer.Count;
            if (count < 1)
            {
                return;
            }

            SimulationObject SimObj = ObjectBuffer[count - 1];

            // 오브젝트 관리
            ObjectBuffer.Remove(SimObj);
            // 작업 결과를 버퍼에 추가
            LogBuffer.Add(new LogInfo() { Log = "Remove " + SimObj.Name, Time = DateTime.Now });

            model.RemoveObject(SimObj);
        }

        public void Save(Stream stream)
        {
            if (null != App)
            {
                JsonSerializer js = new JsonSerializer();
                js.stream = stream;
                model.Save(js);
            }
        }

        public void Load(Stream stream)
        {
            if (null != App)
            {
                JsonSerializer js = new JsonSerializer();
                js.stream = stream;
                model.Load(js);
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
            ObjectBuffer.Clear();
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

        public void Reset()
        {
            App = null;
            model = null;
            controller = null;
            view = null;
            ObjectBuffer.Clear();
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

        /// <summary>
        /// UI 쓰레드에서 이 메소드를 호출합니다.
        /// 호출되면 LogList에 LogBuffer내용을 복사합니다.
        /// </summary>
        /// <returns>마지막에 기록한 작업 결과</returns>
        public LogInfo UpdateLog()
        {
            LogInfo last = null;
            lock (LogBuffer)
            {
                count = LogBuffer.Count;
                foreach (LogInfo log in LogBuffer)
                {
                    LogList.Add(log);
                    last = log;
                }
                LogBuffer.Clear();
                return last;
            }
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
