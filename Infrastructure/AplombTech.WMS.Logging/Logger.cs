using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.Logging
{
    public static class Logger
    {
        public static void LogError(Exception ex, string contextualMessage = null)
        {
            try
            {
                if (contextualMessage != null)
                {
                    var annotatedException = new Exception(contextualMessage + " error type is " + ex.GetType().Name, ex);
                    Elmah.ErrorSignal.FromCurrentContext().Raise(annotatedException);
                }
                else
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
            catch (Exception)
            {

            }
        }

        public static void Log(string contextualMessage = null)
        {
            try
            {
                if (contextualMessage != null)
                {
                    //connect log4 and write message.
                }
                else
                {
                    //connect log4 and write message.+contextualMessage
                }

            }
            catch (Exception ex)
            {
                LogError(ex, "Unable to write log file by used log4.");
            }
        }
    }

}
