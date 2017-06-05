using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledMenuAppWpf
{
    public class Position
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public Position(int row, int col) { Row = row; Col = col; }
    }
}
