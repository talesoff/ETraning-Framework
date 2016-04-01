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

namespace RenderTest_JK
{
    class Scene_Make
    {
        Application App;
        Model model;
        Controller controller;
        View view;

        bool isRun;
        Thread simulationWorker;

        public float FPS = 60.0f;

        public Scene_Make()
        {
            isRun = true;
            simulationWorker = new Thread(() =>
            {
                Stopwatch sw = new Stopwatch();

                Start();
                while (true == isRun)
                {
                    sw.Restart();
                    AppUpdate();
                    while (sw.Elapsed.TotalMilliseconds < 1000.0f / 60.0f)
                    {
                        Thread.Sleep(0);
                    }
                }
                
            }
            );
            simulationWorker.Name = "SimulationWorker";
            simulationWorker.Priority = ThreadPriority.Highest;
            simulationWorker.Start();

        }

        private void createScene()
        {
            App = new Application();

            model = App.SetModel<Model>();
            controller = App.SetController<Controller>();
            view = App.AddView<View>();

            GraphicsSubsystem renderingP = App.AddSubSystem<GraphicsSubsystem>();

            // Model
            model.AddProcessor<UserUpdateProcessor>();
            model.AddProcessor<UserLateUpdateProcessor>();
            model.AddProcessor<CameraProcess>();

            // Controller
            List<Type> processorOrder = new List<Type>();
            processorOrder.Add(typeof(UserUpdateProcessor));
            processorOrder.Add(typeof(UserLateUpdateProcessor));
            processorOrder.Add(typeof(CameraProcess));
            controller.SetOrder(processorOrder);
            
            // View
            view.AddSubSystem(renderingP, 0);
        }

        private void createObjects()
        {
            //Create Render Components_1
            SimulationObject testObject = model.AddObject();
            testObject.Name = "testObj";

            Transform transform_test = testObject.GetComponent<Transform>();
            transform_test.Position = new Vector3(0.0f, 0.0f, 0.0f);

            MeshFilter meshfilter_test = testObject.AddComponent<MeshFilter>();
            meshfilter_test.mesh = new Mesh("filepath", "Mesh");

            Material material_test = new Material(new Shader("filepath"));
            material_test.MainTexture = new Texture2D("filepath");
            material_test.Color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);

            MeshRenderer meshrenderer = testObject.AddComponent<MeshRenderer>();
            meshrenderer.material = material_test;

            //Create Render Components_2
            SimulationObject testObject_2 = model.AddObject();
            testObject_2.Name = "testObj_2";

            Transform transform_test_2 = testObject_2.GetComponent<Transform>();
            transform_test_2.Position = new Vector3(0.0f, 0.0f, 0.0f);

            MeshFilter meshfilter_test_2 = testObject_2.AddComponent<MeshFilter>();
            meshfilter_test_2.mesh = new Mesh("filepath", "Mesh");

            Material material_test_2 = new Material(new Shader("filepath"));
            material_test_2.MainTexture = new Texture2D("filepath");
            material_test_2.Color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);

            MeshRenderer meshrenderer_2 = testObject_2.AddComponent<MeshRenderer>();
            meshrenderer_2.material = material_test_2;
            
            //Create Camera Components
            SimulationObject cameraObject = model.AddObject();
            cameraObject.Name = "cameraObj";

            Transform transform_camera = cameraObject.GetComponent<Transform>();
            transform_camera.Position = new Vector3(0.0f, 0.0f, 0.0f);

            ETE.Render.Component.Camera comp_camera = cameraObject.AddComponent<ETE.Render.Component.Camera>();
            //comp_camera.Root = testObject;
            comp_camera.TargetDisplay = 0;  //아직 EGL 개발되지않음
            comp_camera.BackGround_Color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
            comp_camera.CameraOrder = 0;
        }

        private void Ascheduler_StartEvent(object sender, EventArgs e)
        {
            if (null == App)
            {
                createScene();
                createObjects();
            }

            App.Start();
        }

        private void Ascheduler_StopEvent(object sender, EventArgs e)
        {
            if (null != App)
            {
                App.Stop();
            }
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
                createObjects();
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
            App = null;
        }
    }
}