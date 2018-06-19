using ExpensesMVC2018.Models;
using ExpensesMVC2018.Utilities;
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
                cmd.Parameters.AddWithValue("@ExpenseTypeId", model.ExpenseTypeId);
                cmd.Parameters.AddWithValue("@ExpenseAmount", model.ExpenseAmount);
                cmd.Parameters.AddWithValue("@BranchCode", model.BranchCode);
                cmd.Parameters.AddWithValue("@VendorName", model.VendorName);
                cmd.Parameters.AddWithValue("@LastFourCcNumber", model.LastFourCcNumber);
                cmd.Parameters.AddWithValue("@Note", Helpers.SqlNullHandler(model.Note));
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
                            ExpenseId = Helpers.GetIntValue(reader, "ExpenseId"),
                            UserId = Helpers.GetIntValue(reader, "UserId"),
                            ExpenseDate = Convert.ToDateTime(reader["ExpenseDate"]),
                            ExpenseTypeId = Helpers.GetIntValue(reader, "ExpenseTypeId"),
                            ExpenseTypeDescription = Helpers.GetStringValue(reader, "ExpenseTypeDescription"),
                            ExpenseAmount = Convert.ToDouble(reader["ExpenseAmount"]),
                            BranchCode = Helpers.GetStringValue(reader, "BranchCode"),
                            VendorName = Helpers.GetStringValue(reader, "VendorName"),
                            LastFourCcNumber = Helpers.GetStringValue(reader, "LastFourCcNumber"),
                            //Note = Helpers.GetStringValue(reader, "Notes"),
                            Notes = Helpers.GetListValue(reader, "Notes"),
                            StatusId = Helpers.GetIntValue(reader, "StatusId"),
                            StatusDescription = Helpers.GetStringValue(reader, "StatusDescription"),
                            ReceiptFileName = Helpers.GetStringValue(reader, "ReceiptFileName"),
                            DisplayReceiptImage = (Byte[])reader["Receipt"]
                        });
                }
            }

            return expenses;
        }

        public static List<ExpenseType> GetExpenseTypes()
        {
            List<ExpenseType> expenseTypes = new List<ExpenseType>();

            using (var DataWrapper = new Utilities.DataWrapper())
            {
                var reader = DataWrapper.Query("GetExpenseTypes");

                while (reader.Read())
                {
                    expenseTypes.Add(
                        new ExpenseType()
                        {
                            ExpenseTypeId = (int)reader["ExpenseTypeId"],
                            ExpenseTypeDescription = (string)reader["ExpenseTypeDescription"]
                        });
                }
            }

            return expenseTypes;
        }
    }
}