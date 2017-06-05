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
    
    public class BlinkMapModel
    {
        private Style blinkStyle;
        private Style noBlinkStyle;
        private Style selectedStyle;

        private string nameKey;
        private string blinkName;
        public string Blink {   get { return blinkName;  }
                                set {
                                        blinkName = value;
                                        Style newStyle = (Style)window.TryFindResource(blinkName);
                                        if (newStyle != null){
                                            blinkStyle = newStyle;
                                        }
                                    }
                            }

        private string noBlinkName;
        public string NoBlink {
                                    get { return noBlinkName; }
                                    set
                                    {
                                        noBlinkName = value;
                                        Style newStyle = (Style)window.TryFindResource(noBlinkName);
                                        if (newStyle != null)
                                        {
                                            noBlinkStyle = newStyle;
                                        }
                                    }
                                }

        private string selectedName;
        public string Selected
        {
            get { return selectedName; }
            set
            {
                selectedName = value;
                Style newStyle = (Style)window.TryFindResource(selectedName);
                if (newStyle != null)
                {
                    selectedStyle = newStyle;
                }
            }
        }

        public int GridSize { get; set; }

        public Position CurrentBlinking { get; set; }
        public Position CurrentSelected { get; set; }

        public MainWindow window;
        public MainWindow Window { get { return window; } set { window = value;  } }

        private DispatcherTimer timer;

        private bool blinkerState;
        
        private int blinkCounter;

        private int inputValue;
        private int inputTreshold;

        public BlinkMapModel(MainWindow owner, int size, string namePrefix)
        {
            window = owner;
            GridSize = size;
            nameKey = namePrefix;
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
        }

        private void changeSelect()
        {
            if (CurrentSelected.Row == -1 && CurrentSelected.Col == -1)
            {
                // here we select row and start blinking tiles from it
            } else
            {
                // here we select one option from the chosen row
            }
        }


        public void userInput(int value)
        {
            inputValue = value;
            if (inputValue > inputTreshold)
            {
                changeSelect();
            }
        }

        private void blinkFirstColumn()
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
                changeStyle(CurrentBlinking, 0);
                blinkerState = false;
            }
            else
            {
                changeStyle(CurrentBlinking, 1);
                blinkerState = true;
            }
        }

        private void timerTick(object sender, EventArgs e)
        {
            if (CurrentBlinking.Row == -1 && CurrentBlinking.Col == -1)
            {
                CurrentBlinking.Row = 0;
                CurrentBlinking.Col = 0;
                changeStyle(CurrentBlinking, 1);
                blinkerState = true;
            } else
            {
                blinkFirstColumn();
            }
        }

        public void changeStyle(Position pos, int state)
        {
            object obj = window.FindName(nameKey + CurrentBlinking.Row + "" + CurrentBlinking.Col);
            Border border = null;
            if (obj is Border)
            {
                border = (Border)obj;
                
                switch (state)
                {
                    case 0:
                        border.Style = noBlinkStyle;
                        border.Refresh();
                        break;
                    case 1:
                        border.Style = blinkStyle;
                        border.Refresh();
                        break;
                    case 2:
                        border.Style = selectedStyle;
                        border.Refresh();
                        break;
                }
            }
            
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
                    object obj = window.FindName(nameKey + i + "" + j);
                    if (obj is Border)
                    {
                        Border border = (Border)obj;
                        border.Style = noBlinkStyle;
                    }
                }
            }
        }
    }
}
