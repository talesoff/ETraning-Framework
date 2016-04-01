using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

using ETE.MVC;
using ETE.Engine;
using ETE.Geometry;
using ETE.Render;
using ETE.Render.Processor;
using ETE.Render.Data;
using ETE.Render.Component;
using ETE.GTImporter;

namespace GTImporterSample
{
    internal class SimulationApplication
    {
        private static SimulationApplication instance;

        public static SimulationApplication Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SimulationApplication();
                }
                return instance;
            }
        }

        Application App;
        Model model;
        Controller controller;
        
        bool isRun;
        Thread simulationWorker;

        public float FPS = 60.0f;

        public bool getisRun()
        {
            return isRun;
        }

        public Application getApp()
        {
            return App;
        }

        public Model getModel()
        {
            return model;
        }

        private SimulationApplication()
        {
            if (null == App)
            {
                createApp();
            }
        }

        private void Run()
        {
            isRun = true;
            simulationWorker = new Thread(() =>
            {
                Stopwatch sw = new Stopwatch();

                App.Start();

                while (true == isRun)
                {
                    sw.Restart();
                    AppUpdate();
                    while (sw.Elapsed.TotalMilliseconds < 1000.0f / 60.0f)
                    {
                        Thread.Sleep(0);
                    }
                }

                App.Stop();
            }
            );
            simulationWorker.Name = "SimulationWorker";
            simulationWorker.Priority = ThreadPriority.Highest;
            
            simulationWorker.Start();
        }

        private void AppUpdate()
        {
            if (null != App)
            {
                App.Update();
            }
        }

        public void Start()
        {
            if (null != App)
            {
                createModel();
            }

            this.Run();
        }

        public void Stop()
        {
            if (null == App)
            {
                return;
            }

            isRun = false;
            simulationWorker.Join();
            simulationWorker = null;
        }
        

        private void createApp()
        {
            App = new Application();

            model = App.SetModel<Model>();
            controller = App.SetController<Controller>();

            controller.AddProcessor<UserUpdateProcessor>();
            controller.AddProcessor<UserLateUpdateProcessor>();
            controller.AddProcessor<CameraProcess>();

            controller.SetOrder(typeof(UserUpdateProcessor), 0);
            controller.SetOrder(typeof(UserLateUpdateProcessor), 1);
            controller.SetOrder(typeof(CameraProcess), 2);
        }

        private void createModel()
        {
            loadAllFiles();
            
			// Create Camera
            {
                //Create Camera Components
                SimulationObject cameraObject = model.AddObject();
                cameraObject.Name = "cameraObj";

                Transform transform_camera = cameraObject.GetComponent<Transform>();
                transform_camera.Position = new Vector3(0.0f, 40.0f, 250.0f);

                ETE.Render.Component.Camera comp_camera = cameraObject.AddComponent<ETE.Render.Component.Camera>();
                comp_camera.TargetDisplay = 0;
                comp_camera.BackGround_Color = new Vector4(0.2f, 0.2f, 0.4f, 1.0f);
                comp_camera.CameraOrder = 0;
                comp_camera.Near = 2.0f;
                comp_camera.Far = 10000.0f;
                comp_camera.FOV = 35.0f;
                comp_camera.Aspect_Ratio = 1.7021f;
            }

            createGTModel();
        }

        private void loadAllFiles()
        {
            Model model = this.model;
            
            List<string> path_gtm_list = new List<string>();


            // 로딩하고자 하는 모든 gtm 파일 리스트
            path_gtm_list.Add("knight.gtm");


            // 모든 gtm 파일 로딩
            if (RenderAsset.getRenderAsset() != null)
            {
                foreach (string path_gtm in path_gtm_list)
                {
                    RenderAsset.Load(path_gtm);
                }
            }
        }

        private void createGTModel()
        {
            string path_gtm = "knight.gtm";
            string path_gtb = "knight.gtb";
            
            if ((path_gtm != null) && (path_gtb != null))
            {
                var stream = new FileStream(path_gtb, FileMode.Open);

                GTBoneImporter boneImporter = GTBoneImporter.GetAtPath(stream, path_gtb);

                if (boneImporter != null)
                {
                    //input
                    GTBoneUtility gtBoneUtil = new GTBoneUtility(boneImporter.GTBoneInfo);                    

                    //output 
                    SimulationObject[] simObjArray = gtBoneUtil.getSimultionObjectArray(model, path_gtm);

                    
                    if((simObjArray != null) && (simObjArray.Length > 0))
                    {
                        // set position
                        Transform transform = simObjArray[0].GetComponent<Transform>().Root;
                        transform.Position = new Vector3(0, 0, 0);
                    }
                }

                stream.Close();
            }
        }

        public void print(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
