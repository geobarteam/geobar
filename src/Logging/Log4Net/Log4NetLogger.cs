﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoSimple.Logging;

namespace GoSimple.Logging.Log4Net
{
    public class Log4NetLogger : ILog
    {
        public void Log(string source, LogLevel level, string message, string correlationId = null)
        {
            Log4NetAdapter logger = LogFactory.Obtain(source);
            switch (level)
            {
                case LogLevel.Debug:
                    {
                        if (logger.IsDebugEnabled)
                        {
                            logger.Debug(message);
                        }
                        break;
                    }
                case LogLevel.Info:
                    {
                        if (logger.IsInfoEnabled)
                        {
                            logger.Info(message);
                        }
                        break;
                    }
                case LogLevel.Warning:
                    {
                        if (logger.IsWarnEnabled)
                        {
                            logger.Warn(message);
                        }
                        break;
                    }
                case LogLevel.Error:
                    {
                        if (logger.IsErrorEnabled)
                        {
                            logger.Error(message);
                        }
                        break;
                    }
                case LogLevel.All:
                    {
                        logger.Log(source, LogLevel.All, message);
                        break;
                    }
                case LogLevel.Emergency:
                    {
                        logger.Log(source, LogLevel.Emergency,message);
                        break;
                    }
                default:
                    {
                        logger.Log(source, LogLevel.All, message);
                        break;
                    }
            }
        }

       
    }
}
