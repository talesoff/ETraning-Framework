using ETE.Engine;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TestConsole.UnitTest
{
    class SimulationEngine2015
    {
        public static void Run()
        {
            bool exit = false;
            do
            {
                Console.Clear();

                Console.WriteLine("-- 시뮬레이션 엔진 단위시험");
                Console.WriteLine("");

                Console.WriteLine("-- 장면관리자");
                Console.Write("  1: 객체 추가");
                Console.Write("  2: 객체 삭제");
                Console.Write("  3: 객체 모두 검색");
                Console.Write("  4: 컴포넌트 모두 검색");
                Console.Write("  5: 서브시스템");
                Console.Write("  6: 서브시스템 순서");
                Console.WriteLine("");
                Console.WriteLine("");

                Console.WriteLine("-- 스케줄러");
                Console.Write("  7: 시작/종료");
                Console.Write("  8: 일시정지/다시시작");
                Console.Write("  9: 비동기");
                Console.WriteLine("");
                Console.WriteLine("");

                Console.WriteLine("-- 컴포넌트 관리자");
                Console.Write("  10: 컴포넌트 추가");
                Console.Write("  11: 컴포넌트 검색");
                Console.Write("  12: 컴포넌트 집합 검색");
                Console.Write("  13: 컴포넌트 전체 검색");
                Console.Write("  14: 컴포넌트 삭제");
                Console.Write("  15: 컴포넌트 인터페이스 확인");
                Console.WriteLine("");
                Console.WriteLine("");

                Console.WriteLine("-- 이벤트 관리자");
                Console.Write("  16: 생명주기 생성>>>삭제");
                Console.Write("  17: 생명주기 집합 생성>>>삭제");
                Console.Write("  18: 사용자 컴포넌트 생성>>>삭제");
                Console.Write("  19: 이벤트 사용자 컴포넌트>>>사용자 컴포넌트 ");
                Console.Write("  20: 이벤트 서브시스템>>>컴포넌트");
                Console.WriteLine("");
                Console.WriteLine("");

                Console.WriteLine("-- ");
                Console.WriteLine(" A - 전체");
                Console.WriteLine(" Q - 종료");
                Console.WriteLine("");

                Console.Write("메뉴의 번호를 입력하세요: ");
                string line = Console.ReadLine();
                line = line.Trim().ToLower();

                Console.Clear();

                switch (line)
                {
                    case "a":
                        for (int i = 1; i <= 20; i++)
                        {
                            Console.WriteLine("테스트 " + i.ToString());
                            Console.WriteLine("--------------------------------------------------------------------------------");

                            RunTest(i.ToString());

                            Console.WriteLine("");
                            Console.WriteLine("아무키나 누르면 다음으로 진행합니다.");
                            ConsoleKeyInfo info = Console.ReadKey(true);
                            if (ConsoleKey.Escape == info.Key)
                            {
                                Console.WriteLine("전체 테스트를 중지합니다.");
                                break;
                            }
                            Console.Clear();
                        }

                        break;

                    case "q":
                        Console.WriteLine("단위시험을 종료합니다.");
                        exit = true;
                        break;

                    default:
                        if (false == RunTest(line))
                        {
                            continue;
                        }
                        break;
                }

                if (false == exit)
                {
                    Console.WriteLine("");
                    Console.WriteLine("아무키나 누르면 다음으로 진행합니다.");
                    Console.ReadKey(true);
                }
            } while (false == exit);

            Console.WriteLine("종료");
        }

        private static bool RunTest(string key)
        {
            bool ret = true;
            switch (key)
            {
                case "1":
                    ObjectAddTest();
                    break;

                case "2":
                    ObjectDestroyTest();
                    break;

                case "3":
                    ObjectFindAllTest();
                    break;

                case "4":
                    ObjectFindAllComponentsTest();
                    break;

                case "5":
                    SubSystemTest();
                    break;

                case "6":
                    SubSystemOrderTest();
                    break;

                case "7":
                    SchedulerTest();
                    break;

                case "8":
                    SchedulerPauseTest();
                    break;

                case "9":
                    SchedulerAsyncTest();
                    break;

                case "10":
                    ComponentAddTest();
                    break;

                case "11":
                    ComponentGetTest();
                    break;

                case "12":
                    ComponentGetMultipleTest();
                    break;

                case "13":
                    ComponentGetAllTest();
                    break;

                case "14":
                    ComponentDestroyTest();
                    break;

                case "15":
                    ComponentHierarchyTest();
                    break;

                case "16":
                    EventLifecycleTest();
                    break;

                case "17":
                    EventLifecycleGroupTest();
                    break;

                case "18":
                    EventLifecycleOrderTest();
                    break;

                case "19":
                    EventCustomTest();
                    break;

                case "20":
                    EventSubsystemToComponentTest();
                    break;

                default:
                    ret = false;
                    break;
            }

            return ret;
        }

        public static void ObjectAddTest()
        {
            Scene s = new Scene();
            SimulationObject o = s.AddObject();
            Console.WriteLine(o.GetType().Name);
            Console.WriteLine(o);
        }

        public static void ObjectDestroyTest()
        {
            Scene s = new Scene();
            SimulationObject o = s.AddObject();
            ETE.Engine.Object.DestroyImmediate(o);
            Console.WriteLine(o.GetType().Name);
            Console.WriteLine(o);
        }

        public static void ObjectFindAllTest()
        {
            Application a = new Application();
            Scene s = new Scene();
            s.AddObject();
            s.AddObject();

            a.LoadScene(s);
            ManualScheduler sc = new ManualScheduler();
            a.Scheduler = sc;
            a.Start();

            IList<SimulationObject> objs = SimulationObject.FindObjectsOfType<SimulationObject>();
            Console.WriteLine(objs.Count);
            foreach (object obj in objs)
            {
                Console.WriteLine(obj.GetType().Name);
            }

            a.Stop();
        }

        public static void ObjectFindAllComponentsTest()
        {
            Application a = new Application();
            Scene s = new Scene();
            s.AddObject();
            s.AddObject();

            a.LoadScene(s);
            ManualScheduler sc = new ManualScheduler();
            a.Scheduler = sc;
            a.Start();

            IList<Transform> objs = SimulationObject.FindObjectsOfType<Transform>();
            Console.WriteLine(objs.Count);
            foreach (object obj in objs)
            {
                Console.WriteLine(obj.GetType().Name);
            }

            a.Stop();
        }

        public static void SubSystemTest()
        {
            Application a = new Application();
            Scene s = new Scene();
            s.AddSubSystem(new TestSubSystem() { Log = true });

            a.LoadScene(s);
            ManualScheduler sc = new ManualScheduler();
            a.Scheduler = sc;

            a.Start();

            a.Stop();
        }

        public static void SubSystemOrderTest()
        {
            Application a = new Application();
            Scene s = new Scene();
            TestSubSystem ss0 = new TestSubSystem0() { Order = 2, Log = true };
            TestSubSystem ss1 = new TestSubSystem1() { Order = 0, Log = true };
            TestSubSystem ss2 = new TestSubSystem2() { Order = 1, Log = true };
            s.AddSubSystem(ss0);
            s.AddSubSystem(ss1);
            s.AddSubSystem(ss2);

            a.LoadScene(s);
            ManualScheduler sc = new ManualScheduler();
            a.Scheduler = sc;

            a.Start();

            a.Stop();
        }

        public static void SchedulerTest()
        {
            Application a = new Application();
            Scene s = new Scene();

            a.LoadScene(s);
            ManualScheduler sc = new ManualScheduler();
            sc.Log = true;
            a.Scheduler = sc;

            a.Start();
            a.Stop();
        }

        public static void SchedulerPauseTest()
        {
            Application a = new Application();
            Scene s = new Scene();

            a.LoadScene(s);
            ManualScheduler sc = new ManualScheduler();
            sc.Log = true;
            a.Scheduler = sc;

            a.Start();
            a.Pause();
            a.Resume();
            a.Stop();
        }

        public static void SchedulerAsyncTest()
        {
            Application a = new Application();
            Scene s = new Scene();

            a.LoadScene(s);
            AsyncScheduler sc = new AsyncScheduler();
            a.Scheduler = sc;

            a.Start();
            Console.WriteLine(sc.HasThreadAccess);
            a.Stop();
        }

        public static void ComponentAddTest()
        {
            Scene s = new Scene();
            SimulationObject o = s.AddObject();
            LifeCycleComponent c = o.AddComponent<LifeCycleComponent>();
            Console.WriteLine(c.GetType().Name);
            Console.WriteLine(c);
        }

        public static void ComponentGetTest()
        {
            Scene s = new Scene();
            SimulationObject o = s.AddObject();
            o.AddComponent<LifeCycleComponent>();
            LifeCycleComponent c = o.GetComponent<LifeCycleComponent>();
            Console.WriteLine(c.GetType().Name);
        }

        public static void ComponentGetMultipleTest()
        {
            Scene s = new Scene();
            SimulationObject o = s.AddObject();
            o.AddComponent<LifeCycleComponent>();
            o.AddComponent<LifeCycleComponent>();
            o.AddComponent<LifeCycleComponent>();
            IList<LifeCycleComponent> c = o.GetComponents<LifeCycleComponent>();
            Console.WriteLine(c.Count);
            foreach (object obj in c)
            {
                Console.WriteLine(obj.GetType().Name);
            }
        }

        public static void ComponentGetAllTest()
        {
            Scene s = new Scene();
            SimulationObject o = s.AddObject();
            o.AddComponent<LifeCycleComponent>();
            IList<Component> c = o.GetComponents<Component>();
            Console.WriteLine(c.Count);
            foreach (object obj in c)
            {
                Console.WriteLine(obj.GetType().Name);
            }
        }

        public static void ComponentDestroyTest()
        {
            Scene s = new Scene();
            SimulationObject o = s.AddObject();
            LifeCycleComponent c = o.AddComponent<LifeCycleComponent>();
            ETE.Engine.Object.DestroyImmediate(c);
            Console.WriteLine(c.GetType().Name);
            Console.WriteLine(c);
        }

        public static void ComponentHierarchyTest()
        {
            Type t = typeof(LifeCycleUserComponent);
            string s = "";
            while (null != t)
            {
                Console.Write(s);
                Console.WriteLine(t);

                s += "-";
                t = t.BaseType;
            }
        }

        public static void EventLifecycleTest()
        {
            Application a = new Application();
            Scene s = new Scene();
            SimulationObject o = s.AddObject();
            LifeCycleComponent c = o.AddComponent<LifeCycleComponent>();
            c.Log = true;

            a.LoadScene(s);
            ManualScheduler sc = new ManualScheduler();
            a.Scheduler = sc;

            a.Start();
            sc.ManualUpdate();
            a.Stop();
        }

        public static void EventLifecycleGroupTest()
        {
            Application a = new Application();
            Scene s = new Scene();
            SimulationObject o = s.AddObject();
            LifeCycleComponent c0 = o.AddComponent<LifeCycleComponent0>();
            c0.Log = true;
            LifeCycleComponent c1 = o.AddComponent<LifeCycleComponent1>();
            c1.Log = true;

            a.LoadScene(s);
            ManualScheduler sc = new ManualScheduler();
            a.Scheduler = sc;

            a.Start();
            sc.ManualUpdate();
            a.Stop();
        }

        public static void EventLifecycleOrderTest()
        {
            Application a = new Application();
            Scene s = new Scene();
            SimulationObject o = s.AddObject();
            LifeCycleUserComponent c0 = o.AddComponent<LifeCycleUserComponent>();
            c0.Log = true;
            LifeCycleComponent c1 = o.AddComponent<LifeCycleComponent>();
            c1.Log = true;

            a.LoadScene(s);
            ManualScheduler sc = new ManualScheduler();
            a.Scheduler = sc;

            a.Start();
            sc.ManualUpdate();
            a.Stop();
        }

        public static void EventCustomTest()
        {
            Application a = new Application();
            Scene s = new Scene();
            s.AddSubSystem(new UserSubsystem());
            SimulationObject o = s.AddObject();
            LifeCycleUserComponent c0 = o.AddComponent<LifeCycleUserComponent>();

            a.LoadScene(s);
            ManualScheduler sc = new ManualScheduler();
            a.Scheduler = sc;

            a.Start();
            sc.ManualUpdate();
            a.Stop();
        }

        public static void EventSubsystemToComponentTest()
        {
            Application a = new Application();
            Scene s = new Scene();
            s.AddSubSystem(new TestSubSystem());
            SimulationObject o = s.AddObject();
            LifeCycleComponent c = o.AddComponent<LifeCycleComponent>();

            a.LoadScene(s);
            ManualScheduler sc = new ManualScheduler();
            a.Scheduler = sc;

            a.Start();
            sc.ManualUpdate();
            a.Stop();
        }
    }

    class TestSubSystem0 : TestSubSystem
    { }

    class TestSubSystem1 : TestSubSystem
    { }

    class TestSubSystem2 : TestSubSystem
    { }

    class LifeCycleComponent0 : LifeCycleComponent
    { }

    class LifeCycleComponent1 : LifeCycleComponent
    { }

}
