﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerProj
{
    internal class Program
    {
        static void Main()
        {
            Server server = new Server("127.0.0.1", 4444);
            server.Start();
            server.Stop();
            
        }
    }
}