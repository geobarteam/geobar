using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace GoSimple.Logging
{
    public enum LogLevel
    {
        All = 0,
        Debug = 1,
        Info = 2,
        Warning = 3,
        Error = 4,
        Emergency = 5
    }
}
