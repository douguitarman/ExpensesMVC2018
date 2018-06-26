using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
//using Dapper;

namespace ExpensesMVC2018.Utilities
{
    public static class Logger
    {
        private static string _file { get; set; }

        private static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DougDB"].ToString());

        public static void SetLogFileName(string pathname)
        {
            Console.WriteLine(pathname);
            _file = pathname;
        }

        public static void Information(string infomsg)
        {
            new Thread(() => AsyncInfo(infomsg)).Start();
            //AsyncInfo(infomsg);
        }

        private static void AsyncInfo(string infoMsg)
        {
            try
            {

                int appId = int.Parse(ConfigurationManager.AppSettings["Application"].ToString());
                int source = int.Parse(ConfigurationManager.AppSettings["Environment"].ToString());
                string sourceMachine = Environment.MachineName;
                int severity = 1;       // informational
                int logLevel = int.Parse(ConfigurationManager.AppSettings["LogLevel"].ToString());

                using (SqlCommand cmd = new SqlCommand("sp_LogError", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ApplicationId", appId);
                    cmd.Parameters.AddWithValue("@Source", source);
                    cmd.Parameters.AddWithValue("@SourceMachine", sourceMachine);
                    cmd.Parameters.AddWithValue("@Severity", severity);
                    cmd.Parameters.AddWithValue("@LogLevel", logLevel);
                    cmd.Parameters.AddWithValue("@Message", infoMsg);
                    cmd.Parameters.AddWithValue("@StackTrace", String.Empty);

                    connection.Open();

                    string result = cmd.ExecuteNonQuery().ToString();
                }

                //using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["global"].ConnectionString))
                //{
                //    db.Open();
                //    db.Execute("LogInsert", new
                //    {
                //        ApplicationId = appid,
                //        Source = source,
                //        SourceMachine = sourcemachine,
                //        Severity = severity,
                //        LogLevel = loglevel,
                //        Message = infomsg,
                //        StackTrace = ""
                //    }, commandType: CommandType.StoredProcedure);

                //}

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public static void Error(string infoMsg, string stackTrace)
        {
            new Thread(() => AsyncError(infoMsg, stackTrace)).Start();
        }
        private static void AsyncError(string infoMsg, string stackTrace)
        {
            try
            {

                int appId = int.Parse(ConfigurationManager.AppSettings["Application"].ToString());
                int source = int.Parse(ConfigurationManager.AppSettings["Environment"].ToString());
                string sourceMachine = Environment.MachineName;
                int severity = 9;       // fatal
                int logLevel = int.Parse(ConfigurationManager.AppSettings["LogLevel"].ToString());

                using (SqlCommand cmd = new SqlCommand("sp_LogError", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ApplicationId", appId);
                    cmd.Parameters.AddWithValue("@Source", source);
                    cmd.Parameters.AddWithValue("@SourceMachine", sourceMachine);
                    cmd.Parameters.AddWithValue("@Severity", severity);
                    cmd.Parameters.AddWithValue("@LogLevel", logLevel);
                    cmd.Parameters.AddWithValue("@Message", infoMsg);
                    cmd.Parameters.AddWithValue("@StackTrace", stackTrace);

                    connection.Open();

                    string result = cmd.ExecuteNonQuery().ToString();
                }

                //using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["global"].ConnectionString))
                //{
                //    db.Open();
                //    db.Execute("LogInsert", new
                //    {
                //        ApplicationId = appid,
                //        Source = source,
                //        SourceMachine = sourcemachine,
                //        Severity = severity,
                //        LogLevel = loglevel,
                //        Message = infomsg,
                //        StackTrace = stacktrace
                //    }, commandType: CommandType.StoredProcedure);

                //}

            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}