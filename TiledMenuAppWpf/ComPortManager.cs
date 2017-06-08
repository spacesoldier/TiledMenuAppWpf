using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace TiledMenuAppWpf
{
    public class ComPortManager
    {
        public List<ComPortDescriptor> PortsAvailable { get; }
        private int activePort;
        public ComPortDescriptor getActivePort()
        {
            return PortsAvailable.ElementAt(activePort);
        }

        private Dispatcher UIDispatcher;

        public ComPortManager(Dispatcher uiDispatcher)
        {
            PortsAvailable = new List<ComPortDescriptor>();
            UIDispatcher = uiDispatcher;
            activePort = -1;
            scanPorts();
        }

        public void scanPorts()
        {
            for (int i=1; i<20; i++)
            {
                ComPortDescriptor newPort = new ComPortDescriptor("COM" + i, 9600, UIDispatcher);
                if (newPort.checkAvailable())
                {
                    PortsAvailable.Add(newPort);
                }
            }
        }

        public void connectToModel(BlinkMapModel model)
        {
            foreach (ComPortDescriptor port in PortsAvailable)
            {
                port.Model = model;
            }
        }

        public void connectValueSource(Action<int> method)
        {
            foreach (ComPortDescriptor port in PortsAvailable)
            {
                port.callback = method;
            }
        }

        public void connectHandler(ValueReportDelegate valDelegate)
        {
            foreach (ComPortDescriptor port in PortsAvailable)
            {
                port.setHandler(valDelegate);
            }
        }

        public void startConnection()
        {
            if (PortsAvailable.Count > 0)
            {
                bool opened = PortsAvailable.ElementAt(0).Open();
                if (opened)
                {
                    activePort = 0;
                    Console.WriteLine("OPENED");
                } else
                {
                    Console.WriteLine("DID NOT OPEN THE PORT");
                }
            }
        }
    }
}
