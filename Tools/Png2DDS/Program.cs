using System;
using System.Runtime.InteropServices;
using ETModel;

namespace Png2DDS
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "";

            fileName = "PNG_To_DDS.cmd";

            ProcessHelper.Run(fileName, "",
                waitExit: true);
        }
    }
}