using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TiledMenuAppWpf
{
    public static class ExtensionMethods
    {
        private static Action EmptyDelegate = delegate () { };

        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }

    public delegate object findElementDelegate(string name);
    public delegate void styleChangeDelegate(Position pos, int state);

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TileImageViewModel imagesModel;
        public TileImageViewModel ImagesModel { get { return imagesModel; }  set { imagesModel = value; } }

        public static BlinkMapModel blinkMapModel;
        public static BlinkMapModel BlinkMapModel { get { return blinkMapModel; } set { blinkMapModel = value; } }

        public ComPortManager SerialPortManager { get;  }

        private Style blinkStyle;
        private Style noBlinkStyle;
        private Style selectedStyle;
        private string nameKey;
        private string blinkName;
        public string Blink
        {
            get { return blinkName; }
            set
            {
                blinkName = value;
                Style newStyle = (Style)this.TryFindResource(blinkName);
                if (newStyle != null)
                {
                    blinkStyle = newStyle;
                }
            }
        }

        private string noBlinkName;
        public string NoBlink
        {
            get { return noBlinkName; }
            set
            {
                noBlinkName = value;
                Style newStyle = (Style)this.TryFindResource(noBlinkName);
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
                Style newStyle = (Style)this.TryFindResource(selectedName);
                if (newStyle != null)
                {
                    selectedStyle = newStyle;
                }
            }
        }


        styleChangeDelegate styleChange;

        public MainWindow()
        {
            ImagesModel = new TileImageViewModel(3);
            ImagesModel.readFolder("img");

            styleChange = this.setStyleToElem;

            InitializeComponent();

            ImageLoadHelper.loadImages(ImagesModel, this, "img");
            blinkMapModel = new BlinkMapModel(this,3);
            this.nameKey = "border";
            this.Blink = "blinkingBorder";
            this.NoBlink = "hiddenBorder";
            this.Selected = "selectedBorder";
            blinkMapModel.initModel();

            closeBtn.Visibility = Visibility.Hidden;
            closeBtn.Opacity = 1;
            settingsBtn.Visibility = Visibility.Hidden;
            settingsBtn.Opacity = 1;

            SerialPortManager = new ComPortManager(Dispatcher);
            //SerialPortManager.connectHandler(blinkMapModel.userInput);
            //SerialPortManager.connectValueSource(blinkMapModel.userInput);
            SerialPortManager.connectToModel(blinkMapModel);
            SerialPortManager.startConnection();
            //blinkMapModel.connectValueSource(new ValueExtractDelegate(SerialPortManager.getActivePort().getCurrValue));
        }

        public void setStyleToElem(Position pos, int state)
        {
            object obj = this.FindName(nameKey + pos.Row + "" + pos.Col);

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

        public void changeStyle(Position pos, int state)
        {
            this.Dispatcher.BeginInvoke(styleChange,pos,state);
            //App.Current.Dispatcher.BeginInvoke(styleChange, pos, state);
        }
        public object findElement(string name)
        {
            object obj = this.FindName(name);
            return obj;
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void settingsBtnAreaEnter(object sender, RoutedEventArgs e)
        {
            settingsBtn.Visibility = Visibility.Visible;
            settingsBtn.Opacity = 0;
        }

        private void settingsBtnAreaLeave(object sender, RoutedEventArgs e)
        {
            settingsBtn.Visibility = Visibility.Hidden;
            settingsBtn.Opacity = 1;
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            closeBtn.Visibility = Visibility.Visible;
            closeBtn.Opacity = 0;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            closeBtn.Visibility = Visibility.Hidden;
            closeBtn.Opacity = 1;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //blinkMapModel.initModel();
        }

        public void switchTile(int input)
        {
            blinkMapModel.userInput(input);
        }
    }
}
