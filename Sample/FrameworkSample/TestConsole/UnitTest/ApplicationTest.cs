using ETE.Engine;
using ETE.Simulator;
using System;
using System.Reflection;
using System.Threading;

namespace TestConsole.UnitTest
{
    public class ApplicationTest
    {
        private static Application app;

        public static void ApplicationRun()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
            Console.WriteLine("시뮬레이션 종료     : Ctrl + Q");
            Console.WriteLine("예외 처리 테스트    : Ctrl + X");
            Console.WriteLine("컴포넌트 활성/비활성: E");

            Scene scene = new Scene();
            SimulationObject simObj = null;
            LifecycleUserComponent lcCom = null;
            for (int i = 0; i < 2; i++)
            {
                simObj = scene.AddObject();
                simObj.Name = "Obj" + i.ToString();
                lcCom = simObj.AddComponent<LifecycleUserComponent>();
            }

            AsyncScheduler sc = new AsyncScheduler();
            sc.AddRepetitionTask(new Action(() =>
            {
                if (true == ConsoleKeyboard.IsKeyDown(ConsoleKey.X) && true == ConsoleKeyboard.IsKeyDown(ConsoleModifiers.Control))
                {
                    Console.WriteLine("예외(Exception) 처리를 테스트합니다.");
                    Console.WriteLine("{0:H}", 0);
                }

                if (true == ConsoleKeyboard.IsKeyDown(ConsoleKey.E))
                {
                    Console.WriteLine("컴포넌트를 활성/비활성화합니다.");
                    Application.Current.Scheduler.Run(()=>
                    {
                        lcCom.Enabled = !lcCom.Enabled;
                    });
                }
            }));

            app = new Application();
            app.UnhandledException += App_UnhandledExceptionHandler;
            app.Scheduler = sc;

            scene.AddSubSystem(new UserSubsystem());

            app.LoadScene(scene);
            app.Start();

            bool isRun = true;
            while (true == isRun)
            {
                if (true == ConsoleKeyboard.IsKeyDown(ConsoleKey.Q) && true == ConsoleKeyboard.IsKeyDown(ConsoleModifiers.Control))
                {
                    Console.WriteLine("시뮬레이션 엔진을 종료합니다.");
                    isRun = false;
                }
                Thread.Sleep(16);
            }
            app.Stop();
        }

        private static void App_UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            Console.WriteLine(ex.ToString());
        }
    }
}
