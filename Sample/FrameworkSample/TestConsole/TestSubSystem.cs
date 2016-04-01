using ETE.Engine;
using System;
using System.Reflection;

namespace TestConsole
{
    public class TestSubSystem : BaseSubSystem
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

        public override void Loaded()
        {
            WriteLog(MethodBase.GetCurrentMethod());
        }

        public override void Run()
        {
            EventAggregator.PublishMessage<LifeCycleComponent>("CustomEvent", null);
            EventAggregator.PublishMessage<LifeCycleComponent>("CustomEvent", 321);
        }

        public override void Unloaded()
        {
            WriteLog(MethodBase.GetCurrentMethod());
        }
    }
}
