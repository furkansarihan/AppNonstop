using System;
using System.Collections.Generic;
using System.Text;

namespace Nonstop.Forms.Game
{
    public class NonstopTime
    {
        uint startTime; // init position of NonstopTime object
        uint pauseStartTime;
        uint pausedTime = 0;
        public uint currentMillis { get; set; } // for calculate
        public string currentSecond { get; set; } // for display
        public string currentMinute { get; set; } // for display

        public NonstopTime(uint start)
        {
            this.startTime = start;
            updateTime();
        }
        public void updateTime()
        {
            this.currentMillis = Urho.Time.SystemTime - startTime - pausedTime;
            this.currentSecond = ((this.currentMillis / 1000) % 60).ToString();
            this.currentMinute = (this.currentMillis / 60000).ToString();
        }
        public string getDisplayableTime()
        {
            return this.currentMinute + ":" + this.currentSecond;
        }
        public void pauseStart()
        {
            this.pauseStartTime = Urho.Time.SystemTime;
        }
        public void pauseEnd()
        {
            this.pausedTime += Urho.Time.SystemTime - this.pauseStartTime;
        }
        public void refreshTime(uint newMillis)
        {

        }
    }
}
