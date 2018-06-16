using ExpensesMVC2018.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ExpensesMVC2018.DataAccess
{
    public class Expenses
    {
        private SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DougDB"].ToString());

        /// <summary>
        /// Check reader values for null before casting to string
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private String GetStringValue(SqlDataReader reader, String columnName)
        {
            return (reader[columnName] != null && reader[columnName] != DBNull.Value) ? Convert.ToString(reader[columnName]) : null;
        }

        /// <summary>
        /// Check reader values for null before casting to int
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private int GetIntValue(SqlDataReader reader, String columnName)
        {
            return (reader[columnName] != null && reader[columnName] != DBNull.Value) ? Convert.ToInt32(reader[columnName]) : 0;
        }

        /// <summary>
        /// Add new Expense
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string AddExpense(Expense model)
        {
            using (SqlCommand cmd = new SqlCommand("AddExpense", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", model.UserId);
                cmd.Parameters.AddWithValue("@ExpenseDate", model.ExpenseDate);
                cmd.Parameters.AddWithValue("@ExpenseType", model.ExpenseType);
                cmd.Parameters.AddWithValue("@ExpenseAmount", model.ExpenseAmount);
                cmd.Parameters.AddWithValue("@BranchCode", model.BranchCode);
                cmd.Parameters.AddWithValue("@VendorName", model.VendorName);
                cmd.Parameters.AddWithValue("@LastFourCcNumber", model.LastFourCcNumber);
                cmd.Parameters.AddWithValue("@Notes", model.Notes);
                cmd.Parameters.AddWithValue("@ReceiptFileName", model.ReceiptFileName);
                //cmd.Parameters.AddWithValue("@ReceiptImage", model.ReceiptImage.InputStream);
                cmd.Parameters.AddWithValue("@ReceiptImage", SqlDbType.VarBinary).Value = model.ConvertedReceiptImage; // or is type binary?

                connection.Open();

                string result = cmd.ExecuteNonQuery().ToString();

                return result;
            }
        }

        public List<Expense> GetExpenses()
        {
            List<Expense> expenses = new List<Expense>();

            using (var DataWrapper = new Utilities.DataWrapper())
            {
                var reader = DataWrapper.Query("GetExpenses");

                while (reader.Read())
                {
                    expenses.Add(
                        new Expense()
                        {
                            ExpenseId = GetIntValue(reader, "ExpenseId"),
                            UserId = GetIntValue(reader, "UserId"),
                            ExpenseDate = Convert.ToDateTime(reader["ExpenseDate"]),
                            ExpenseType = GetStringValue(reader, "ExpenseType"),
                            ExpenseAmount = Convert.ToDouble(reader["ExpenseAmount"]),
                            BranchCode = GetStringValue(reader, "BranchCode"),
                            VendorName = GetStringValue(reader, "VendorName"),
                            LastFourCcNumber = GetStringValue(reader, "LastFourCcNumber"),
                            Notes = GetStringValue(reader, "Notes"),
                            ReceiptFileName = GetStringValue(reader, "ReceiptFileName"),
                            DisplayReceiptImage = (Byte[])reader["Receipt"]
                        });
                }
            }

            return expenses;
        }
    }
}