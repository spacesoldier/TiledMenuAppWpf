using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TiledMenuAppWpf
{
    public class TileImageViewModel
    {
        private List<TileImage> images;
        public List<TileImage> Images { get { return images; } set { images = value; } }

        public TileImageViewModel(string dirName)
        {
            List <FileInfo> files = FileLoadHelper.findPictures(dirName);

            Images = new List<TileImage>();

            foreach (FileInfo f in files)
            {
                TileImage img = new TileImage(f.Directory.Name+"/"+f.Name);

                Images.Add(img);
            }
        }
    }
}
