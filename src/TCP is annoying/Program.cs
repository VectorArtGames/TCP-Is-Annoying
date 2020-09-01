using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCP_is_annoying.Base;

namespace TCP_is_annoying
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task.Run(NetMaster.Run);
            Thread.Sleep(-1);
        }
    }
}
