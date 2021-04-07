using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TestSensors
{
    public class DatasetWriterController
    {
        private StreamWriter dataSetFile;

        public DatasetWriterController(string fileName)
        {
            dataSetFile = new StreamWriter(fileName);
            dataSetFile.AutoFlush = true;
        }

        public void writeData(IEnumerable<IEnumerable<short>> data)
        {
            var frame = data.Skip(
                Array.FindLastIndex(data.SkipLast(1).ToArray(), d => d.All(dd => dd == 0)));

            var adjustedFrame =
                Enumerable.Repeat(Enumerable.Repeat<short>(0, 2), 50).ToArray();

            var frameArray = frame.ToArray();
            for (int i = 0; i < (frameArray.Length > 50 ? adjustedFrame.Length : frameArray.Length); i++)
            {
                adjustedFrame[i] = frameArray[i];
            }

            var resStr = "";

            for (int i = 0; i < adjustedFrame.Length; i++)
            {
                resStr += adjustedFrame[i].ToArray()[0].ToString() + ',';
            }
            for (int i = 0; i < adjustedFrame.Length; i++)
            {
                resStr += adjustedFrame[i].ToArray()[1].ToString() + ',';
            }

            //var str = string.Join(',', adjustedFrame.Select(d =>
            //    string.Join(',', d.Select(dd => dd.ToString()))).ToArray()) + '\n';
            Console.WriteLine(resStr + '\n');
            //var array = System.Text.Encoding.Default.GetBytes(str);
            dataSetFile.Write(resStr + "FirstStrong" + '\n');
        }

        ~DatasetWriterController()
        {
            dataSetFile.Close();
        }
    }
}