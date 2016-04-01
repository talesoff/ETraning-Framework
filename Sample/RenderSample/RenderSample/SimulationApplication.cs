using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

using ETE.Engine;
using ETE.Geometry;
using ETE.MVC;

using ETE.Render;
using ETE.Render.Processor;
using ETE.Render.Data;
using ETE.Render.Component;

namespace RenderSample
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
                createScene();
            }
        }

        private void Run()
        {
            isRun = true;
            simulationWorker = new Thread(() =>
            {
                Stopwatch sw = new Stopwatch();
                Stopwatch fpsSw = new Stopwatch();

                App.Start();

                Console.WriteLine("Application Thread - Console WriteLine");
                System.Diagnostics.Debug.WriteLine("Application Thread - Debug WriteLine");

                while (true == isRun)
                {
                    sw.Restart();
                    fpsSw.Restart();

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
            if (null == App)
            {
                createScene();
            }
            createModel();

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
        

        private void createScene()
        {
            App = new Application();

            model = App.SetModel<Model>();
            controller = App.SetController<Controller>();
            
            // Controller
            controller.AddProcessor<UserUpdateProcessor>();
            controller.AddProcessor<UserLateUpdateProcessor>();
            controller.AddProcessor<CameraProcess>();
            
            controller.SetOrder(typeof(UserUpdateProcessor), 0);
            controller.SetOrder(typeof(UserLateUpdateProcessor), 1);
            controller.SetOrder(typeof(CameraProcess), 2);
        }
        
        private void createModel()
        {
            //-----------------------------------
            //scene에서 사용할 모든 GTM 파일을 적으세요
            //mesh & material pool 구성
            if (RenderAsset.getRenderAsset() != null)
            {             
                // 객체 로드
                RenderAsset.Load("tidus/tidus.gtm", typeof(Mesh));
                RenderAsset.Load("tidus/tidus.gtm", typeof(Material));
                RenderAsset.Load("shader/lighting.sh", typeof(Shader));

                //RenderAsset.Load("tidus/tidus.gtm", typeof(Mesh));
                //RenderAsset.Load("tidus/tidus.gtm", typeof(Material));
                //RenderAsset.Load("shader/lighting.sh", typeof(Shader));

                // 데이터 취득
                Mesh[] meshArray = RenderAsset.GetObjectsOfTypeAll(typeof(Mesh)) as Mesh[];
                Material[] materialArray = RenderAsset.GetObjectsOfTypeAll(typeof(Material)) as Material[];
                Shader[] shaderArray = RenderAsset.GetObjectsOfTypeAll(typeof(Shader)) as Shader[];
                
                // Create Model(Create Mesh by Model)
                int meshLength = meshArray.GetLength(0);
              
                for(int i=0; i<meshLength; ++i)
                {
                    SimulationObject testObject = model.AddObject();
                    testObject.Name = meshArray[i].getMeshName() + "_Node";
              
                    Transform transform_test = testObject.GetComponent<Transform>();
                    transform_test.Position = new Vector3(0.0f, 0.0f, 0.0f);
              
                    MeshFilter meshfilter_test = testObject.AddComponent<MeshFilter>();
                    meshfilter_test.mesh = meshArray[i];
              
                    Material material_test = materialArray[i];
                    material_test.Shader = shaderArray[0];
              
                    MeshRenderer meshrenderer = testObject.AddComponent<MeshRenderer>();
                    meshrenderer.material = material_test;

                    //UserControl은 단순히 모델을 회전시켜주는 역할을 합니다.
                    UserControl.MeshControl mc = testObject.AddComponent<UserControl.MeshControl>();
                }

                // Create Cube(Create Mesh by Script)
                SimulationObject CubeObject = model.AddObject();
                CubeObject.Name = "CreateMesh_Test";

                Transform transform_Cube = CubeObject.GetComponent<Transform>();
                transform_Cube.Position = new Vector3(0.0f, 0.0f, 0.0f);

                UserControl.CreateMesh cm = CubeObject.AddComponent<UserControl.CreateMesh>();

                // Create Camera
                // 현재 있는 View 개수만큼 Camera를 생성합니다.
                int[] targetNumbers = ETE.Render.EGL.GLUserControlManager.Instance.getTargetNumbers();
                foreach(int targetnumber in targetNumbers)
                {
                    SimulationObject cameraObject = SimulationApplication.Instance.getModel().AddObject();
                    cameraObject.Name = "cameraObj";
                    
                    ETE.Render.Component.Camera comp_camera = cameraObject.AddComponent<ETE.Render.Component.Camera>();
                    comp_camera.TargetDisplay = targetnumber;
                    comp_camera.BackGround_Color = new Vector4(0.2f, 0.2f, 0.4f, 1.0f);
                    comp_camera.CameraOrder = 0;
                    comp_camera.Near = 2.0f;
                    comp_camera.Far = 10000.0f;
                    comp_camera.FOV = 35.0f;
                    comp_camera.Aspect_Ratio = 1.7021f;

                    // Camera의 위치 설정
                    Transform transform_camera = cameraObject.GetComponent<Transform>();
                    if (targetnumber == 0)
                    {
                        transform_camera.Position = new Vector3(0.0f, 40.0f, 250.0f);
                        transform_camera.EulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                    }
                    else if (targetnumber == 1)
                    {
                        transform_camera.Position = new Vector3(0.0f, 250.0f, 0.0f);
                        transform_camera.EulerAngles = new Vector3(-90.0f, 0.0f, 0.0f);
                    }
                    else if (targetnumber == 2)
                    {
                        transform_camera.Position = new Vector3(250.0f, 40.0f, 0.0f);
                        transform_camera.EulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                    }
                    else if (targetnumber == 3)
                    {
                        transform_camera.Position = new Vector3(130.0f, 200.0f, 130.0f);
                        transform_camera.EulerAngles = new Vector3(-45.0f, 45.0f, 0.0f);
                    }
                }
            }
        }
    }
}
