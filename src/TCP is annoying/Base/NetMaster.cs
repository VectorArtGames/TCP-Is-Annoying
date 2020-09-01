using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCP_is_annoying.Core.Net;

namespace TCP_is_annoying.Base
{
    public class NetMaster
    {
        public static void Run()
        {
            var n = new Network();
			n.Initialize();
        }
    }
}
