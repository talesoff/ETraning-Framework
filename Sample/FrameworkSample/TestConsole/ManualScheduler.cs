using System;
using System.Reflection;

namespace TestConsole
{
    public class ManualScheduler : ETE.Engine.Scheduler
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

        public override void Pause()
        {
            InvokeWillPauseEvent();
            WriteLog(MethodBase.GetCurrentMethod());
        }

        public override void Resume()
        {
            InvokeResumeEvent();
            WriteLog(MethodBase.GetCurrentMethod());
        }

        public void ManualUpdate()
        {
            base.ProcessTask();
            base.ProcessRepetitionTask();
            WriteLog(MethodBase.GetCurrentMethod());
        }

        public override void Start()
        {
            SetSchedulerThreadID();
            InvokeStartEvent();
            WriteLog(MethodBase.GetCurrentMethod());
        }

        public override void Stop()
        {
            InvokeEndEvent();
            WriteLog(MethodBase.GetCurrentMethod());
        }
    }
}
