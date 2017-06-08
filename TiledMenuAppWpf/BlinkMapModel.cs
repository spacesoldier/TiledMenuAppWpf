using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using MyExtensions;
using System.Windows.Media.Animation;

namespace TiledMenuAppWpf
{
    public delegate void blinkStepDelegate();
    public delegate void changeSelectDelegate();

    public class BlinkMapModel
    {
        public int GridSize { get; set; }

        public Position CurrentBlinking { get; set; }
        public Position CurrentSelected { get; set; }

        public MainWindow window;
        public MainWindow Window { get { return window; } set { window = value;  } }

        private DispatcherTimer timer;

        private bool blinkerState;
        private bool blinking;
        private int blinkCounter;

        private int inputValue;
        private int inputTreshold;

        findElementDelegate findElement;
        changeSelectDelegate changeTile;

        public ValueExtractDelegate getPortValue { get; set; }
        public void connectValueSource(ValueExtractDelegate source)
        {
            getPortValue = source;
        }

        public blinkStepDelegate BlinkStepFunc;

        public BlinkMapModel(MainWindow owner, int size)
        {
            window = owner;
            GridSize = size;
            CurrentBlinking = new Position(-1,-1);
            CurrentSelected = new Position(-1, -1);

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Start();

            inputValue = 0;
            inputTreshold = 100;
            
            blinkerState = false;
            blinkCounter = 0;
            blinking = true;

            findElement = new findElementDelegate(window.findElement);
            BlinkStepFunc = new blinkStepDelegate(this.blinkStep);
            changeTile = new changeSelectDelegate(this.changeSelect);
        }

        private void changeSelect()
        {
            if (CurrentSelected.Row == -1 && CurrentSelected.Col == -1)
            {
                // here we select row and start blinking tiles from it
                CurrentSelected.Row = CurrentBlinking.Row;
            } else if (CurrentSelected.Col == -1)
            {
                // here we select one option from the chosen row
                CurrentSelected.Col = CurrentBlinking.Col;
                stopBlinking();
            } else
            {
                stopBlinking();
            }
        }

        public void userInput(int value)
        {
            inputValue = value;
            if (inputValue > inputTreshold)
            {
                changeSelect();
                //window.Dispatcher.BeginInvoke(changeTile);
            }
        }

        private void stopBlinking()
        {
            timer.Stop();
            window.changeStyle(CurrentBlinking, 3);
            blinking = false;
            blinkCounter = 0;
        }

        private void blinkFirstColumn()
        {
            if (blinking)
            {
                blinkCounter++;
                if (blinkCounter > 5)
                {
                    CurrentBlinking.Row++;
                    blinkCounter = 0;
                }

                if (CurrentBlinking.Row > 2)
                {
                    CurrentBlinking.Row = 0;
                }

                if (blinkerState)
                {
                    window.changeStyle(CurrentBlinking, 0);
                    blinkerState = false;
                }
                else
                {
                    window.changeStyle(CurrentBlinking, 1);
                    blinkerState = true;
                }
            }
        }

        private void blinkSelectedRow()
        {
            if (blinking)
            {
                blinkCounter++;
                if (blinkCounter > 5)
                {
                    CurrentBlinking.Col++;
                    blinkCounter = 0;
                }

                if (CurrentBlinking.Col > 2)
                {
                    CurrentBlinking.Col = 0;
                }

                if (blinkerState)
                {
                    window.changeStyle(CurrentBlinking, 0);
                    blinkerState = false;
                }
                else
                {
                    window.changeStyle(CurrentBlinking, 1);
                    blinkerState = true;
                }
            }
        }

        private void checkPort()
        {
            if (getPortValue != null)
            {
                int val = getPortValue();
                if (val > inputTreshold)
                {
                    //window.Dispatcher.BeginInvoke(changeTile);
                    changeSelect();
                }
            }
        }

        public void blinkStep()
        {
            if (CurrentBlinking.Row == -1 && CurrentBlinking.Col == -1)
            {
                CurrentBlinking.Row = 0;
                CurrentBlinking.Col = 0;
                window.changeStyle(CurrentBlinking, 1);
                blinkerState = true;
            }
            else
            {
                //checkPort();
                if (CurrentSelected.Row == -1 && CurrentSelected.Col == -1)
                {
                    blinkFirstColumn();
                }
                else if (CurrentSelected.Col == -1)
                {
                    blinkSelectedRow();
                }
            }
        }

        private void timerTick(object sender, EventArgs e)
        {
            blinkStep();
            //window.Dispatcher.BeginInvoke(this.BlinkStepFunc);
        }

        public void setDefaultStyle(object sender, RoutedEventArgs e)
        {
            if (sender is Border)
            {
                Style newStyle = (Style)window.TryFindResource("blinkingBorder");
                ((Border)sender).Style = newStyle;
            }
        }

        public void initModel()
        {
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    window.changeStyle(new Position(i, j), 0);
                }
            }
        }
    }
}
