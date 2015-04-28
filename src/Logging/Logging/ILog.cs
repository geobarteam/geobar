using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoBar.Framework.Logging
{
    public interface ILog
    {
        void Log(string source, LogLevel level, string message, string correlationId = null);
    }
}
