using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledMenuAppWpf
{
    public class ComPortDescriptor
    {
        public string Name { get; set; }
        public int BaudRate { get; set; }

        private BackgroundWorker backgroundWorker = new BackgroundWorker();

        public ComPortDescriptor(string name, int baudRate)
        {
            Name = name;
            BaudRate = baudRate;
        }

        public bool checkAvailable()
        {
            bool result = false;

            return result;
        }
    }
}
