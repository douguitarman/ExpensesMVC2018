using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace ExpensesMVC2018.Utilities
{
    public class Helpers
    {
        /// <summary>
        /// Create a new byte array from Image
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] CreateArrayFromImage(Image img)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                img.Save(memoryStream, ImageFormat.Jpeg);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Convert and resize an uploaded image
        /// </summary>
        /// <param name="myFile"></param>
        /// <returns></returns>
        public static byte[] ConvertResizeImage(HttpPostedFileBase myFile)
        {
            using (Stream objStream = myFile.InputStream)
            {
                byte[] receipt = ImageResizer.ImageResize(objStream);

                return receipt;
            }
        }

        /// <summary>
        /// Check reader values for null before casting to string
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static String GetStringValue(SqlDataReader reader, String columnName)
        {
            return (reader[columnName] != null && reader[columnName] != DBNull.Value) ? Convert.ToString(reader[columnName]) : null;
        }

        public static List<string> GetListValue(SqlDataReader reader, String columnName)
        {
            return (reader[columnName] != null && reader[columnName] != DBNull.Value) ? Convert.ToString(reader[columnName]).Split(',').ToList() : null;
        }

        /// <summary>
        /// Check reader values for null before casting to int
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static int GetIntValue(SqlDataReader reader, String columnName)
        {
            return (reader[columnName] != null && reader[columnName] != DBNull.Value) ? Convert.ToInt32(reader[columnName]) : 0;
        }

        public static DateTime GetDatetimeValue(SqlDataReader reader, String columnName)
        {
            return (reader[columnName] != null && reader[columnName] != DBNull.Value) ? Convert.ToDateTime(reader[columnName]) : DateTime.Now;
        }

        /// <summary>
        /// Handle null values when inserting or updating data via Sql Stored Procedure call.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static object SqlNullHandler(object instance)
        {
            if (instance != null)
                return instance;

            return DBNull.Value;
        }
    }
}