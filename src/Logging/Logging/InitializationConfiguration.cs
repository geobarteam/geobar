using System;
using System.Collections.Generic;

namespace GeoBar.Framework.Logging
{
    public class InitializationConfiguration
    {
        public Func<IDictionary<string, object>, string> PropertiesFormatter { get; set; }

        public Func<Exception, string> ExceptionFormatter { get; set; }

    }
}
