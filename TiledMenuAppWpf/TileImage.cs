using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledMenuAppWpf
{
    public class TileImage
    {
        private string path;
        public string Path { get { return path; } set { path = value; } }

        private int row;
        public int Row { get { return row; } set { row = value; } }

        private int col;
        public int Col { get { return col; } set { col = value; } }

        public TileImage(string location)
        {
            Path = location;
            Row = 0;
            Col = 0;
        }
    }
}
