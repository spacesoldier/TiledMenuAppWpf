using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TiledMenuAppWpf
{
    class ImageLoadHelper
    {
        public static void loadImages(TileImageViewModel model, MainWindow window, string namePrefix)
        {
            for (int i = 0; i< model.GridSize; i++)
            {
                for (int j = 0; j< model.GridSize; j++)
                {
                    object obj = window.FindName(namePrefix + i+""+j);
                    if (obj is Image)
                    {
                        Image img = (Image)obj;

                        BitmapImage src = new BitmapImage();
                        src.BeginInit();
                        src.UriSource = new Uri(model.Images[i][j].Path, UriKind.Relative);
                        src.CacheOption = BitmapCacheOption.OnLoad; ;
                        src.EndInit();

                        img.Source = src;
                    }
                }
            }
        }
    }
}
