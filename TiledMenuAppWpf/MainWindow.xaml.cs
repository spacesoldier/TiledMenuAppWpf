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

namespace TiledMenuAppWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TileImageViewModel imagesModel;
        public TileImageViewModel ImagesModel { get { return imagesModel; }  set { imagesModel = value; } }

        public MainWindow()
        {
            ImagesModel = new TileImageViewModel("img");

            InitializeComponent();
            closeBtn.Visibility = Visibility.Hidden;
            closeBtn.Opacity = 1;
            settingsBtn.Visibility = Visibility.Hidden;
            settingsBtn.Opacity = 1;
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
    }
}
