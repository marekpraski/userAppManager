using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniwersalnyDesktop
{

    public class TimerEventArgs
    {
        public int time { get; set; }
    }


    public class Class1
    {            int i = 1;
        public delegate void timerEventHandler(object sender, TimerEventArgs args);
        public event timerEventHandler timerEvent;

        public void timer()
        {

            TimerEventArgs args = new TimerEventArgs();
            for (int k = 0; k < 3; k++)
            {
                args.time = i;
                OnTimerEvent(args);
                System.Threading.Thread.Sleep(1000);
                i++;
            }

        }

        protected virtual void OnTimerEvent(TimerEventArgs args)
        {
            if (timerEvent != null)
            {
                timerEvent(this, args);
            }
        }
    }
}
