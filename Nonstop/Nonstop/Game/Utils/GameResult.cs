using System;
using System.Collections.Generic;
using System.Text;

namespace Nonstop.Forms.Game.Utils
{
    public class GameResult
    {
        int totalTap = 0;
        int currentMiss = 0;

        public GameResult()
        {

        }

        public void setTotalTap(int tt)
        {
            totalTap = tt;
        }
        
        public void incraseCurrentMiss()
        {
            currentMiss = currentMiss + 1;
        }

        public string getUIScore()
        {
            if (currentMiss != 0)
            {
                return "%" + ((100 * (totalTap - currentMiss)) / totalTap).ToString();
            }
            else
            {
                return "%100";
            }
            
        }
    }
}
