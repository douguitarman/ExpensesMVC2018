using ExpensesMVC2018.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpensesMVC2018.Extensions
{
    public static class LogError
    {
        public static void LogTheError(this Exception value, string ExtendedErrorMessage = null, string EventLogSource = null, string LogName = null)
        {
            Logger.Error(value.Message, value.StackTrace);
        }
    }
}