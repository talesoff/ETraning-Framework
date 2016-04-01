using ETE.Engine;
using System;
using System.Reflection;

namespace TestConsole
{
    class LifeCycleUserComponent : UserComponent
    {
        public bool Log
        {
            get;
            set;
        }

        private void WriteLog(MethodBase mb)
        {
            if (true == Log)
            {
                String log = String.Format("{0}.{1}", this.GetType().Name, mb.Name);
                Console.WriteLine(log);
            }
        }

        protected void Awake()
        {
            WriteLog(MethodBase.GetCurrentMethod());
        }

        protected void OnEnable()
        {
            WriteLog(MethodBase.GetCurrentMethod());

        }

        protected void Start()
        {
            WriteLog(MethodBase.GetCurrentMethod());
        }

        protected void Update()
        {
            SendMessage("CustomEvent");
            SendMessage("CustomEvent", 123);
        }

        protected void OnDisable()
        {
            WriteLog(MethodBase.GetCurrentMethod());
        }

        protected void OnDestroy()
        {
            WriteLog(MethodBase.GetCurrentMethod());
        }

        protected void Asleep()
        {
            WriteLog(MethodBase.GetCurrentMethod());
        }

        protected void CustomEvent()
        {
            MethodBase mb = MethodBase.GetCurrentMethod();
            string log = String.Format("{0}.{1}()", this.GetType().Name, mb.Name);
            Console.WriteLine(log);
        }

        protected void CustomEvent(int i)
        {
            MethodBase mb = MethodBase.GetCurrentMethod();
            string log = String.Format("{0}.{1}({2})", this.GetType().Name, mb.Name, i);
            Console.WriteLine(log);
        }
    }
}
