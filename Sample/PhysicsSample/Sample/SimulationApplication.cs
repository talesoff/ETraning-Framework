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
using ETE.Physics;

namespace PhysicsSample
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

        public void Start(int selectIndex)
        {
            if (null == App)
            {
                createApp();
            }

            createModel(selectIndex);

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

            controller.AddProcessor<PhysicsProcessor>();
            controller.AddProcessor<UserUpdateProcessor>();
            controller.AddProcessor<UserLateUpdateProcessor>();
            controller.AddProcessor<CameraProcess>();

            controller.SetOrder(typeof(PhysicsProcessor), 0);
            controller.SetOrder(typeof(UserUpdateProcessor), 1);
            controller.SetOrder(typeof(UserLateUpdateProcessor), 2);
            controller.SetOrder(typeof(CameraProcess), 3);


            // IMPORTANT !!
            // physics component order setting
            bool isPhysicsOrder = true;
            if (isPhysicsOrder == true) 
            {
                int order = -110;

                //collider order setting
                ComponentImporter.SetExecutionOrder(typeof(BoxCollider), order);

                //rigidbody order setting
                ComponentImporter.SetExecutionOrder(typeof(Rigidbody), order + 1);

                //joint order setting
                ComponentImporter.SetExecutionOrder(typeof(FixedJoint), order + 2);
                ComponentImporter.SetExecutionOrder(typeof(HingeJoint), order + 2);
                ComponentImporter.SetExecutionOrder(typeof(CharacterJoint), order + 2);
            }
        }

        private void createModel(int selectIndex)
        {
            loadAllFiles();
            
			// Create Camera
            {
                //Create Camera Components
                SimulationObject cameraObject = model.AddObject();
                cameraObject.Name = "cameraObj";

                Transform transform_camera = cameraObject.GetComponent<Transform>();
                transform_camera.Position = new Vector3(0.0f, 0.0f, 25.0f);

                ETE.Render.Component.Camera comp_camera = cameraObject.AddComponent<ETE.Render.Component.Camera>();
                comp_camera.TargetDisplay = 0;
                comp_camera.BackGround_Color = new Vector4(0.2f, 0.2f, 0.4f, 1.0f);
                comp_camera.CameraOrder = 0;
                comp_camera.Near = 2.0f;
                comp_camera.Far = 10000.0f;
                comp_camera.FOV = 35.0f;
                comp_camera.Aspect_Ratio = 1.7021f;
            }

            if (selectIndex == 1)
            {
                PhysicsScene01();
            }
            else if(selectIndex == 2)
            {
                PhysicsScene02();
            }
        }

        private void loadAllFiles()
        {
            Model model = this.model;
            
            List<string> path_gtm_list = new List<string>();


            // 로딩하고자 하는 모든 gtm 파일 리스트
            path_gtm_list.Add("box.gtm");


            // 모든 gtm 파일 로딩
            if (RenderAsset.getRenderAsset() != null)
            {
                foreach (string path_gtm in path_gtm_list)
                {
                    RenderAsset.Load(path_gtm);
                }
            }
        }

        private void PhysicsScene01()
        {
            SimulationObject simObj = null;
            Rigidbody rigidbody = null;
            BoxCollider boxCollider = null;

            Mesh[] meshArray = RenderAsset.Load("box.gtm", typeof(Mesh)) as Mesh[];
            Material[] materialArray = RenderAsset.Load("box.gtm", typeof(Material)) as Material[];
            MeshFilter meshFilter = null;
            MeshRenderer meshRender = null;

            Mesh mesh = meshArray[0];
            Material material = materialArray[0];

            Shader shader = new Shader();
            material.Shader = shader;


            // static body
            {
                simObj = model.AddObject();
                simObj.Name = "GroundObj";

                Transform transform = simObj.GetComponent<Transform>();
                transform.Position = new Vector3(0, -1, 0);
                transform.LocalScale = new Vector3(50, 0.1f, 50);

                //add render component
                meshFilter = simObj.AddComponent<MeshFilter>();
                meshFilter.mesh = mesh;

                meshRender = simObj.AddComponent<MeshRenderer>();
                meshRender.material = material;

                //add physics component
                rigidbody = simObj.AddComponent<Rigidbody>();
                rigidbody.mass = 0;

                boxCollider = simObj.AddComponent<BoxCollider>();
                boxCollider.size = new Vector3(1, 1, 1);
            }


            // dynamic body
            for (int i = 0; i < 10; i++)
            {
                simObj = model.AddObject();
                if (simObj != null)
                {
                    simObj.Name = "simObj" + i;

                    Transform transform = simObj.GetComponent<Transform>();
                    transform.Position = new Vector3(0, 20, 0);

                    //add render component
                    meshFilter = simObj.AddComponent<MeshFilter>();
                    meshFilter.mesh = mesh;

                    meshRender = simObj.AddComponent<MeshRenderer>();
                    meshRender.material = material;

                    //add physics component
                    rigidbody = simObj.AddComponent<Rigidbody>();
                    rigidbody.detectCollisions = false; // 충돌 이벤트 발생 하지 않음

                    boxCollider = simObj.AddComponent<BoxCollider>();
                    boxCollider.size = new Vector3(1, 1, 1);
                }
            }
        }

        private void PhysicsScene02()
        {
            SimulationObject simObject = null;
            Rigidbody rigidbody = null;
            BoxCollider boxCollider = null;

            Mesh[] meshArray = RenderAsset.Load("box.gtm", typeof(Mesh)) as Mesh[];
            Material[] materialArray = RenderAsset.Load("box.gtm", typeof(Material)) as Material[];
            MeshFilter meshFilter = null;
            MeshRenderer meshRender = null;

            Mesh mesh = meshArray[0];
            Material material = materialArray[0];

            Shader shader = new Shader();
            material.Shader = shader;


            bool enableFixedJoint = true;
            bool enableHingeJoint = true;
            bool enableCharacterJoint = true;

 

            // fixedjoint test
            if (enableFixedJoint)
            {
                Transform transform = null;

                // Root1
                SimulationObject Root = model.AddObject();
                if (Root != null)
                {
                    Root.Name = "MyRoot1";

                    transform = Root.GetComponent<Transform>();
                    transform.Position = new Vector3(-5, 0, 0);

                    //add render component
                    meshFilter = Root.AddComponent<MeshFilter>();
                    meshFilter.mesh = mesh;

                    meshRender = Root.AddComponent<MeshRenderer>();
                    meshRender.material = material;

                    //add physics component
                    rigidbody = Root.AddComponent<Rigidbody>();
                    rigidbody.mass = 0;

                    boxCollider = Root.AddComponent<BoxCollider>();
                    boxCollider.size = new Vector3(1, 1, 1);
                }

                // simObject1
                simObject = model.AddObject();
                simObject.Name = "simObject1";


                transform = simObject.GetComponent<Transform>();
                transform.Position = new Vector3(-5, 0, 0);


                //add render component
                meshFilter = simObject.AddComponent<MeshFilter>();
                meshFilter.mesh = mesh;

                meshRender = simObject.AddComponent<MeshRenderer>();
                meshRender.material = material;

                //add physics component
                rigidbody = simObject.AddComponent<Rigidbody>();
                        
                boxCollider = simObject.AddComponent<BoxCollider>();
                boxCollider.size = new Vector3(1, 1, 1);

                FixedJoint fixedjoint = Root.AddComponent<FixedJoint>();
                if (fixedjoint != null)
                {
                    // 연결 대상과 연결 지점 설정
                    fixedjoint.connectedBody = simObject.GetComponent<Rigidbody>();
                    fixedjoint.connectedAnchor = new Vector3(0, -1.5f, 0);
                }
            }



            // hingejoint test
            if (enableHingeJoint)
            {
                Transform transform = null;

                // Root2
                SimulationObject Root = model.AddObject();
                if (Root != null)
                {
                    Root.Name = "MyRoot2";

                    transform = Root.GetComponent<Transform>();
                    transform.Position = new Vector3(0, 0, 0);

                    //add render component
                    meshFilter = Root.AddComponent<MeshFilter>();
                    meshFilter.mesh = mesh;

                    meshRender = Root.AddComponent<MeshRenderer>();
                    meshRender.material = material;

                    //add physics component
                    rigidbody = Root.AddComponent<Rigidbody>();
                    rigidbody.mass = 0;

                    boxCollider = Root.AddComponent<BoxCollider>();
                    boxCollider.size = new Vector3(1, 1, 1);
                }

                // simObject2
                simObject = model.AddObject();
                simObject.Name = "simObject2";

                
                transform = simObject.GetComponent<Transform>();
                transform.Position = new Vector3(0, 0, 0);


                //add render component
                meshFilter = simObject.AddComponent<MeshFilter>();
                meshFilter.mesh = mesh;

                meshRender = simObject.AddComponent<MeshRenderer>();
                meshRender.material = material;

                //add physics component
                rigidbody = simObject.AddComponent<Rigidbody>();

                boxCollider = simObject.AddComponent<BoxCollider>();
                boxCollider.size = new Vector3(1, 1, 1);

                HingeJoint hingejoint = null;
                hingejoint = Root.AddComponent<HingeJoint>();
                if (hingejoint != null)
                {
                    // 연결 대상 설정
                    hingejoint.connectedBody = simObject.GetComponent<Rigidbody>();

                    // 연결 지점 설정
                    hingejoint.anchor = new Vector3(0, 0, 0);
                    hingejoint.connectedAnchor = new Vector3(0, 1.5f, 0);
                    
                    // 회전축 지정
                    hingejoint.axis = new Vector3(0, 0, 1);
                    
                    // 회전 최소/최대각 지정
                    hingejoint.useLimits = true;

                    JointLimits jointlimits = hingejoint.limits;
                    jointlimits.min = -70 * ((float)Math.PI / 180.0f);
                    jointlimits.max = 70 * ((float)Math.PI / 180.0f);
                    jointlimits.minBounce = 1.0f;
                    jointlimits.maxBounce = 1.0f;
                    hingejoint.limits = jointlimits;
                }
            }



            // characterjoint test
            if (enableCharacterJoint)
            {
                Transform transform = null;

                // Root3
                SimulationObject Root = model.AddObject();
                if (Root != null)
                {
                    Root.Name = "MyRoot3";

                    transform = Root.GetComponent<Transform>();
                    transform.Position = new Vector3(5, 0, 0);

                    //add render component
                    meshFilter = Root.AddComponent<MeshFilter>();
                    meshFilter.mesh = mesh;

                    meshRender = Root.AddComponent<MeshRenderer>();
                    meshRender.material = material;

                    //add physics component
                    rigidbody = Root.AddComponent<Rigidbody>();
                    rigidbody.mass = 0;

                    boxCollider = Root.AddComponent<BoxCollider>();
                    boxCollider.size = new Vector3(1, 1, 1);
                }

                // simObject3
                simObject = model.AddObject();
                simObject.Name = "simObject3";
                
                transform = simObject.GetComponent<Transform>();
                transform.Position = new Vector3(5, 0, 0);
                
                //add render component
                meshFilter = simObject.AddComponent<MeshFilter>();
                meshFilter.mesh = mesh;

                meshRender = simObject.AddComponent<MeshRenderer>();
                meshRender.material = material;

                //add physics component
                rigidbody = simObject.AddComponent<Rigidbody>();

                boxCollider = simObject.AddComponent<BoxCollider>();
                boxCollider.size = new Vector3(1, 1, 1);
                                
                CharacterJoint characterjoint = null;
                characterjoint = Root.AddComponent<CharacterJoint>();
                if (characterjoint != null)
                {
                    // 연결 대상 설정
                    characterjoint.connectedBody = simObject.GetComponent<Rigidbody>();

                    // 연결 지점 설정
                    characterjoint.anchor = new Vector3(0, -1.5f, 0);
                    characterjoint.connectedAnchor = new Vector3(0, 1.5f, 0);

                    // 회전축 및 회전 제한각 설정
                    characterjoint.axis = new Vector3(0, 0, 1); // twist&swing axis
                    characterjoint.setLimit(((float)Math.PI) * 0.2f, ((float)Math.PI) * 0.2f, 0);
                }
            }
        }

        public void print(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
