using ETE.Engine;
using ETE.Simulator;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace TestConsole.UnitTest
{
    class LifecycleUserComponent2 : LifecycleUserComponent
    { }
    

    public class SceneTest
    {
        static void SimObjectEnableTest()
        {
            Scene DummyScene = new Scene();
            SimulationObject SimObj = DummyScene.AddObject();
            LifecycleUserComponent lcCom = SimObj.AddComponent<LifecycleUserComponent>();

            Console.WriteLine("0");
            DummyScene.Update();

            Console.WriteLine("1");
            SimObj.SetActive(false);
            DummyScene.Update();

            Console.WriteLine("2");
            SimObj.SetActive(true);
            DummyScene.Update();
        }

        public static void FindObjectsTest()
        {
            Scene scene = new Scene();
            SimulationObject obj = scene.AddObject();
            LifecycleUserComponent com = obj.AddComponent<LifecycleUserComponent>();

            Application app = new Application();
            app.LoadScene(scene);

            //IList<SimulationObject> objs = scene.FindObjectsOfType<SimulationObject>();
            //IList<Component> coms = scene.FindObjectsOfType<Component>();
        }

        public static void DestroyTest()
        {
            Scene DummyScene = new Scene();
            SimulationObject SimObj = DummyScene.AddObject();
            LifecycleUserComponent lc = SimObj.AddComponent<LifecycleUserComponent>();
            SimObj.AddComponent<LifecycleUserComponent2>();

            Console.WriteLine("0");
            DummyScene.Update();

            Console.WriteLine("1");
            //ETE.Engine.Object.DestroyImmediate(SimObj);
            //DummyScene.Stop();
            ETE.Engine.Object.Destroy(lc);

            DummyScene.Update();
        }

        public static void LifecycleTest()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);

            Console.WriteLine("0 Result: 0, Awake, OnEnable, Start, 1, OnDisable, OnDestroy");
            {
                Scene DummyScene = new Scene();
                SimulationObject SimObj = DummyScene.AddObject();
                LifecycleUserComponent lcCom = SimObj.AddComponent<LifecycleUserComponent>();

                Console.WriteLine("0");
                DummyScene.Update();

                Console.WriteLine("1");
                ETE.Engine.Object.DestroyImmediate(SimObj);
            }

            Console.ReadKey(true);
            Console.WriteLine("\n");
            Console.WriteLine("1 Result: 0, Awake, 1, OnDestroy");
            {
                Scene DummyScene = new Scene();
                SimulationObject SimObj = DummyScene.AddObject();
                LifecycleUserComponent lcCom = SimObj.AddComponent<LifecycleUserComponent>();

                Console.WriteLine("0");
                lcCom.Enabled = false;
                DummyScene.Update();

                Console.WriteLine("1");
                ETE.Engine.Object.DestroyImmediate(SimObj);
            }

            Console.ReadKey(true);
            Console.WriteLine("\n");
            Console.WriteLine("2 Result: 0, 1");
            {
                Scene DummyScene = new Scene();
                SimulationObject SimObj = DummyScene.AddObject();
                LifecycleUserComponent lcCom = SimObj.AddComponent<LifecycleUserComponent>();

                Console.WriteLine("0");
                SimObj.SetActive(false);
                DummyScene.Update();

                Console.WriteLine("1");
                ETE.Engine.Object.DestroyImmediate(SimObj);
            }

            Console.ReadKey(true);
            Console.WriteLine("\n");
            Console.WriteLine("3 Result: 0, 1, Awake, OnEnable, Start, 2, OnDisable, OnDestroy");
            {
                Scene DummyScene = new Scene();
                SimulationObject SimObj = DummyScene.AddObject();
                LifecycleUserComponent lcCom = SimObj.AddComponent<LifecycleUserComponent>();

                Console.WriteLine("0");
                SimObj.SetActive(false);
                DummyScene.Update();

                Console.WriteLine("1");
                SimObj.SetActive(true);
                DummyScene.Update();

                Console.WriteLine("2");
                ETE.Engine.Object.DestroyImmediate(SimObj);
            }

            Console.ReadKey(true);
            Console.WriteLine("\n");
            Console.WriteLine("4 Result: 0, Awake, OnEnable, Start, 1, OnDisable, 2, OnDestroy");
            {
                Scene DummyScene = new Scene();
                SimulationObject SimObj = DummyScene.AddObject();
                LifecycleUserComponent lcCom = SimObj.AddComponent<LifecycleUserComponent>();

                Console.WriteLine("0");
                DummyScene.Update();

                Console.WriteLine("1");
                lcCom.Enabled = false;

                Console.WriteLine("2");
                ETE.Engine.Object.DestroyImmediate(SimObj);
            }

            Console.ReadKey(true);
            Console.WriteLine("\n");
            Console.WriteLine("5 Result: 0, Awake, OnEnable, Start, 1, OnDisable, 2, OnDestroy");
            {
                Scene DummyScene = new Scene();
                SimulationObject SimObj = DummyScene.AddObject();
                LifecycleUserComponent lcCom = SimObj.AddComponent<LifecycleUserComponent>();

                Console.WriteLine("0");
                DummyScene.Update();

                Console.WriteLine("1");
                SimObj.SetActive(false);

                Console.WriteLine("2");
                ETE.Engine.Object.DestroyImmediate(SimObj);
            }

            Console.ReadKey(true);
            Console.WriteLine("\n");
            Console.WriteLine("6 Result: 0, Awake, OnEnable, Start, 1, OnDisable, 2, 3, 4, OnEnable, 5, OnDisable, 6, 7, OnEnable, 8, OnDisable, OnDestroy");
            {
                Scene DummyScene = new Scene();
                SimulationObject SimObj = DummyScene.AddObject();
                LifecycleUserComponent lcCom = SimObj.AddComponent<LifecycleUserComponent>();

                Console.WriteLine("0");
                DummyScene.Update();

                Console.WriteLine("1");
                SimObj.SetActive(false);

                Console.WriteLine("2");
                lcCom.Enabled = false;

                Console.WriteLine("3");
                SimObj.SetActive(true);

                Console.WriteLine("4");
                lcCom.Enabled = true;

                Console.WriteLine("5");
                lcCom.Enabled = false;
                SimObj.SetActive(false);

                Console.WriteLine("6");
                lcCom.Enabled = true;

                Console.WriteLine("7");
                SimObj.SetActive(true);

                Console.WriteLine("8");
                ETE.Engine.Object.DestroyImmediate(SimObj);
            }

            Console.ReadKey(true);
            Console.WriteLine("\n");
            Console.WriteLine("7 Result: 0, Awake, OnEnable, Awake, OnEnable, Start, Start, 1, OnDisable, OnDestroy, 2, OnDisable, OnDestroy");
            {
                Scene DummyScene = new Scene();

                SimulationObject SimObj1 = DummyScene.AddObject();
                SimObj1.Name = "Obj1";
                LifecycleUserComponent lcCom1 = SimObj1.AddComponent<LifecycleUserComponent>();

                SimulationObject SimObj2 = DummyScene.AddObject();
                SimObj2.Name = "Obj2";
                LifecycleUserComponent lcCom2 = SimObj2.AddComponent<LifecycleUserComponent>();

                Console.WriteLine("0");
                DummyScene.Update();

                Console.WriteLine("1");
                ETE.Engine.Object.DestroyImmediate(SimObj1);

                Console.WriteLine("2");
                ETE.Engine.Object.DestroyImmediate(SimObj2);
            }


            Console.ReadKey(true);
            Console.WriteLine("\n");
            Console.WriteLine("8 Result: 0, 1, 2, Awake, OnEanble, Start, 3, OnDisable, OnDestroy");
            {
                Scene DummyScene = new Scene();
                SimulationObject SimObj = DummyScene.AddObject();
                LifecycleUserComponent lcCom = SimObj.AddComponent<LifecycleUserComponent>();

                lcCom.Enabled = false;
                SimObj.SetActive(false);
                Console.WriteLine("0");
                DummyScene.Update();

                lcCom.Enabled = true;
                Console.WriteLine("1");
                DummyScene.Update();

                SimObj.SetActive(true);
                Console.WriteLine("2");
                DummyScene.Update();

                Console.WriteLine("3");
                DummyScene.Stop();
            }
        }
    }
}
