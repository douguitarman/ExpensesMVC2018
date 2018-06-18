using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace ExpensesMVC2018.Utilities
{
    public class Helpers
    {
        public static byte[] CreateArrayFromImage(Image img)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                img.Save(memoryStream, ImageFormat.Jpeg);
                return memoryStream.ToArray();
            }
        }
    }
}