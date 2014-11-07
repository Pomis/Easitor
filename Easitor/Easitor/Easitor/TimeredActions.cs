using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Easitor
{
    public class TimeredActions:EditorModel
    {
        DispatcherTimer Timer = new DispatcherTimer();
        int ElapsedTime = 0;
        public TimeredActions()
        {
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            Timer.Tick +=Timer_Tick;
            Timer.Start();
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            ElapsedTime++;
            MovePanels();
            
        }

        void MovePanels()
        {
            if (IsLeftPanelExpanding)
            {
                LeftColumnWidth += EXPANDING_SPEED;
                if (LeftColumnWidth >= MAX_LEFT_COLUMN_WIDTH)
                {
                    IsLeftPanelExpanding = false;
                    LeftPanelButtonBackground = GRAY_5;
                    LeftPanelButtonImage = "UI/HistoryBright.png";
                }
            }
            if (IsLeftPanelShrinking)
            {
                LeftColumnWidth -= EXPANDING_SPEED;
                if (LeftColumnWidth<=0)
                {
                    IsLeftPanelShrinking = false;
                    LeftPanelButtonBackground = GRAY_2;
                    LeftPanelButtonImage = "UI/HistoryDark.png";
                }
            }
            if (IsRightPanelExpanding)
            {
                RightColumnWidth += EXPANDING_SPEED;
                if (RightColumnWidth >= MAX_RIGHT_COLUMN_WIDTH)
                {
                    IsRightPanelExpanding = false;
                    RightPanelButtonBackground = GRAY_5;
                    RightPanelButtonImage = "UI/LayersBright.png";
                }
            }
            if (IsRightPanelShrinking)
            {
                RightColumnWidth -= EXPANDING_SPEED;
                if (RightColumnWidth <= 0)
                {
                    IsRightPanelShrinking = false;
                    RightPanelButtonBackground = GRAY_2;
                    RightPanelButtonImage = "UI/LayersDark.png";
                }
            }
            
        }
    }
}
