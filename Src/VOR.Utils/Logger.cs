using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VOR.Utils
{
    public class Logger
    {
        log4net.ILog log = log4net.LogManager.GetLogger("Logging");

        private static volatile Logger _current = null;
        private static object syncRoot = new object();

        //TEST LUDO

        public static Logger Current
        {
            get
            {
                if (_current == null)
                {
                    lock (syncRoot)
                    {
                        if (_current == null)
                        {
                            _current = new Logger();
                        }
                    }
                }

                return _current;
            }
        }

        public void Debug(string message)
        {
            if (this.log.IsDebugEnabled)

                this.log.Debug("[" + DateTime.Now.ToString() + "] " + ExtractInfo(message));
        }

        public void Info(string message)
        {
            if (this.log.IsInfoEnabled)

                this.log.Info("[" + DateTime.Now.ToString() + "] " + ExtractInfo(message));
        }

        public void Error(string message, System.Exception e)
        {
            if (this.log.IsErrorEnabled)

                this.log.Error("[" + DateTime.Now.ToString() + "] " + ExtractInfo(message), e);
        }

        public void Error(System.Exception e)
        {
            if (this.log.IsErrorEnabled)

                this.log.Error("[" + DateTime.Now.ToString() + "] " + ExtractInfo(string.Empty), e);
        }

        public void Error(string message)
        {
            if (this.log.IsErrorEnabled)

                this.log.Error("[" + DateTime.Now.ToString() + "] " + ExtractInfo(message));
        }

        private string ExtractInfo(string message)
        {
            StackFrame frame1 = new StackFrame(2, true);

            string methodName = frame1.GetMethod().Name;

            string className = frame1.GetMethod().ReflectedType.Name;

            string fileName = Path.GetFileName(frame1.GetFileName());

            string text = "File:{0} - Class: {1} - Method:{2} - {3}";

            return string.Format(text, fileName, className, methodName, message);
        }
    }
}