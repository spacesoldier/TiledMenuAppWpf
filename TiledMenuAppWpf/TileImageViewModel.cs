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
        private List<List<TileImage>> images;
        public List<List<TileImage>> Images { get { return images; } set { images = value; } }

        private int gridSize;
        public int GridSize { get { return gridSize;  } set { gridSize = value; } }

        public TileImageViewModel(int size)
        {
            gridSize = size;
            images = new List<List<TileImage>>();
            for (int i = 0; i < gridSize; i++)
            {
                images.Add(new List<TileImage>());
                for (int j = 0; j < gridSize; j++)
                {
                    images[i].Add(new TileImage("", ""));
                }
            }
        }

        public TileImageViewModel(TileImageViewModel sourceModel)
        {
            gridSize = sourceModel.GridSize;
            images = new List<List<TileImage>>();
            for (int i = 0; i < gridSize; i++)
            {
                images.Add(new List<TileImage>());
                for (int j = 0; j < gridSize; j++)
                {
                    images[i].Add(sourceModel.Images[i][j]);
                }
            }
        }

        public void readFolder(string dirName)
        {
            List<FileInfo> files = FileLoadHelper.findPictures(dirName);

            foreach (FileInfo f in files)
            {
                TileImage img = new TileImage(f.Directory.Name, f.Name);
                Images[img.Row][img.Col] = img;
            }
        }
    }
}
