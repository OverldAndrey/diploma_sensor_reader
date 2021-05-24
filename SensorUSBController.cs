using System;
using System.IO.Ports;

namespace TestSensors
{
    public class SensorUSBController
    {
        public event EventHandler<SensorUSBEventArgs> sensorDataReceived;
        SerialPort sp;

        public SensorUSBController(string port)
        {
            sp = new SerialPort(port, 115200, Parity.None, 8, StopBits.One);
                
            sp.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived);
            sp.Open();
        }
        
        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var sensorData = sp.ReadLine();
            var eventArgs = new SensorUSBEventArgs();
            eventArgs.sensorData = sensorData;
            sensorDataReceived?.Invoke(this, eventArgs);
            eventArgs = null;
        }
    }

    public class SensorUSBEventArgs : EventArgs
    {
        public string sensorData { get; set; }
    }
}