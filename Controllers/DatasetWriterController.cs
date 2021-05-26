using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TestSensors
{
    public class DatasetWriterController
    {
        private StreamWriter dataSetFile;

        private int sensorQty = 0;

        public DatasetWriterController(string fileName, int _sensorQty)
        {
            dataSetFile = new StreamWriter(fileName);
            dataSetFile.AutoFlush = true;

            // var zeroStr = "";
            // var fullStr = "";
            //
            // for (int i = 0; i < _sensorQty * 200; i++)
            // {
            //     zeroStr += "0,";
            //     fullStr += "1024,";
            // }
            //
            // for (int i = 0; i < 100; i++)
            // {
            //     dataSetFile.Write(zeroStr + "OtherLow" + '\n');
            //     dataSetFile.Write(fullStr + "OtherHigh" + '\n');
            // }

            sensorQty = _sensorQty;
        }

        public void writeData(IEnumerable<IEnumerable<short>> data)
        {
            // var frame = data.Skip(
            //     Array.FindLastIndex(data.SkipLast(1).ToArray(), d => d.All(dd => dd == 0)));
            //
            // var adjustedFrame =
            //     Enumerable.Repeat(Enumerable.Repeat<short>(0, 2), 50).ToArray();
            //
            // var frameArray = frame.ToArray();
            // for (int i = 0; i < (frameArray.Length > 50 ? adjustedFrame.Length : frameArray.Length); i++)
            // {
            //     adjustedFrame[i] = frameArray[i];
            // }
            var adjustedFrame = data.Select(d => d.ToArray()).ToArray();

            var resStr = "";

            for (int j = 0; j < sensorQty; j++)
            {
                for (int i = 0; i < adjustedFrame.Length; i++)
                {
                    resStr += adjustedFrame[i][j].ToString() + ',';
                }
            }

            adjustedFrame = null;

            // for (int i = 0; i < adjustedFrame.Length; i++)
            // {
            //     resStr += adjustedFrame[i].ToArray()[0].ToString() + ',';
            // }
            // for (int i = 0; i < adjustedFrame.Length; i++)
            // {
            //     resStr += adjustedFrame[i].ToArray()[1].ToString() + ',';
            // }

            //var str = string.Join(',', adjustedFrame.Select(d =>
            //    string.Join(',', d.Select(dd => dd.ToString()))).ToArray()) + '\n';
            Console.WriteLine(resStr + '\n');
            //var array = System.Text.Encoding.Default.GetBytes(str);
            dataSetFile.Write(resStr + "ShakeHold" + '\n');
        }

        ~DatasetWriterController()
        {
            dataSetFile.Close();
        }
    }
}