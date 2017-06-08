using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Windows.Threading;

namespace TiledMenuAppWpf
{
    public delegate void ValueReportDelegate(int value);
    public delegate int ValueExtractDelegate();

    public class ComPortDescriptor
    {
        private SerialPort _serialPort = new SerialPort();
        private int _baudRate = 9600;
        private int _dataBits = 8;
        private Handshake _handshake = Handshake.None;
        private Parity _parity = Parity.None;
        private string _portName = "COM1";
        private StopBits _stopBits = StopBits.One;

        public int PortNumber { get; set; }
        public string Name { get { return _portName; } set { _portName = value; } }
        public int BaudRate { get { return _baudRate; } set { _baudRate = value; } }

        /// <summary> 
        /// Holds data received until we get a terminator. 
        /// </summary> 
        private string tString = string.Empty;
        private int currValue;
        public int getCurrValue()
        {
            return currValue;
        }
        public BlinkMapModel Model { get; set; }
        public Action<int> callback { get; set; }
        /// <summary> 
        /// End of transmition byte in this case EOT (ASCII 4). 
        /// </summary> 
        //private byte _terminator = 0x4;
        private string _terminator = "\r\n";

        public int DataBits { get { return _dataBits; } set { _dataBits = value; } }
        public Handshake Handshake { get { return _handshake; } set { _handshake = value; } }
        public Parity Parity { get { return _parity; } set { _parity = value; } }
        public bool PortStatus { get { return _serialPort.IsOpen; } }

        ValueReportDelegate valReport;
        public Dispatcher UIDispatcher { get; set; }

        public ComPortDescriptor(string name, int baudRate, Dispatcher ui)
        {
            Name = name;
            BaudRate = baudRate;
            PortNumber = Int32.Parse(name.Substring(3));
            UIDispatcher = ui;
        }

        public void setHandler(ValueReportDelegate valDelegate)
        {
            valReport = valDelegate;
        }

        public bool checkAvailable()
        {
            try
            {
                if (Open())
                {
                    _serialPort.Close();
                } else
                {
                    return false;
                }
            } catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public bool Open()
        {
            try
            {
                _serialPort.BaudRate = _baudRate;
                _serialPort.DataBits = _dataBits;
                _serialPort.Handshake = _handshake;
                _serialPort.Parity = _parity;
                _serialPort.PortName = _portName;
                _serialPort.StopBits = _stopBits;
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(_serialPort_DataReceived);
                _serialPort.Open();
            }
            catch {
                return false;
            }
            return true;
        }

        void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            tString = string.Empty;
            //Initialize a buffer to hold the received data 
            byte[] buffer = new byte[_serialPort.ReadBufferSize];

            //There is no accurate method for checking how many bytes are read 
            //unless you check the return from the Read method 
            int bytesRead = _serialPort.Read(buffer, 0, buffer.Length);

            //For the example assume the data we are received is ASCII data. 
            tString += Encoding.ASCII.GetString(buffer, 0, bytesRead);
            //Check if string contains the terminator  
            if (tString.IndexOf(_terminator) > -1)
            {
                //If tString does contain terminator we cannot assume that it is the last character received 
                int msgCount = Regex.Matches(tString, _terminator).Count;
                if (msgCount > 1)
                {
                    tString = tString.Substring(0, tString.LastIndexOf(_terminator));
                    tString = tString.Substring(tString.LastIndexOf(_terminator)+_terminator.Length);
                } else
                {
                    tString = tString.Substring(0, tString.IndexOf(_terminator));
                }

                //Do something with workingString
                currValue = Int32.Parse(tString);
                //if (callback != null)
                //{
                //    callback(currValue);
                //}
                /*
                if (Model != null)
                {
                    Model.userInput(currValue);
                }
                */
                //valReport(Int32.Parse(tString));
                //UIDispatcher.BeginInvoke(new ValueReportDelegate(), Int32.Parse(tString));
                //Console.WriteLine(workingString);


            }
        }

        public void changeBaudRate(int newRate)
        {
            _baudRate = newRate;
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
                Open();
            }
        }

        public void closePort()
        {
            _serialPort.Close();
        }
    }
}
