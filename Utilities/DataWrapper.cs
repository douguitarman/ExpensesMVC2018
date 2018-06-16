using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ExpensesMVC2018.Utilities
{
    public class DataWrapper : IDisposable
    {
        SqlConnection _connection;
        SqlCommand _command;

        public DataWrapper()
        {
            string theConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DougDB"].ConnectionString;

            _connection = new SqlConnection(theConnectionString);
            _command = new SqlCommand() { Connection = _connection };
        }

        public SqlDataReader Query(string query)
        {
            _connection.Open();

            _command.CommandText = query;
            _command.CommandType = CommandType.StoredProcedure;

            var reader = _command.ExecuteReader();

            return reader;
        }

        public SqlDataReader QueryWithParams(string query, string paramName, int paramId)
        {
            _connection.Open();

            _command.CommandText = query;
            _command.CommandType = CommandType.StoredProcedure;
            _command.Parameters.AddWithValue(paramName, paramId);

            var reader = _command.ExecuteReader();

            return reader;
        }

        public int NonQuery(string query, string paramName, int paramId)
        {
            _connection.Open();

            _command.CommandText = query;
            _command.CommandType = CommandType.StoredProcedure;
            _command.Parameters.AddWithValue(paramName, paramId);

            var result = _command.ExecuteNonQuery();

            return result;
        }

        //Scalar and NonQuery methods omitted.

        public void Dispose()
        {
            if (_connection != null) _connection.Close();
        }
    }
}