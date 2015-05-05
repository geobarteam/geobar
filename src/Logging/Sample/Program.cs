using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoSimple.Logging;
using GoSimple.Logging.Log4Net;

namespace GoSimple.Logging.Log4net.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Initialize(new Log4NetLogger());
            Logger.Debug(null, "Logger Initialized!");
            Console.ReadLine();
        }
    }
}
