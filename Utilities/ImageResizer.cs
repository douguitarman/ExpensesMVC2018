using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace ExpensesMVC2018.Utilities
{
    public class ImageResizer
    {
        /// <summary>  
        /// Save image as jpeg  
        /// </summary>  
        /// <param name="path">path where to save</param>  
        /// <param name="img">image to save</param>  
        public static void SaveJpeg(string path, Image img)
        {
            var QualityParam = new EncoderParameter(Encoder.Quality, 100L);
            var JpegCodec = GetEncoderInfo("image/jpeg");

            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = QualityParam;
            img.Save(path, JpegCodec, encoderParams);
        }

        /// <summary>  
        /// get codec info by mime type  
        /// </summary>  
        /// <param name="mimeType"></param>  
        /// <returns></returns>  
        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            return ImageCodecInfo.GetImageEncoders().FirstOrDefault(t => t.MimeType == mimeType);
        }

        /// <summary>
        /// Resize image using percentage and maintain aspect ratio
        /// </summary>
        /// <param name="image"></param>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public static Bitmap ResizeImage(Image image, double percentage)
        {
            double PercentageToUse = (percentage / 100.0);

            int ImageWidth = (int)(image.Width * PercentageToUse);

            int ImageHeight = (int)(image.Height * PercentageToUse);

            var NewRectangle = new Rectangle(0, 0, ImageWidth, ImageHeight);
            var NewImage = new Bitmap(ImageWidth, ImageHeight);

            NewImage.SetResolution(72, 72);

            using (var graphics = Graphics.FromImage(NewImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, NewRectangle, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return NewImage;
        }

        public static void DoImageResize(string pFilePath, string pFileName, string pOutputFileName)
        {

            System.Drawing.Image image;
            image = System.Drawing.Image.FromFile(pFilePath + pFileName);

            //pWidth = imgBef.Width * 50 / 100;
            //pHeight = imgBef.Height * 50 / 100;

            System.Drawing.Image ImageResized;
            ImageResized = ResizeImage(image, 15); //Imager.Resize(imgBef, pWidth, pHeight, true);

            //System.Drawing.Image _img2;
            //_img2 = Imager.PutOnCanvas(_imgR, pWidth, pHeight, System.Drawing.Color.White);

            //Save JPEG  
            SaveJpeg(pFilePath + pOutputFileName, ImageResized);

        }

        /// <summary>
        /// Create and resize image from stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ImageResize(Stream stream)
        {

            // Create new image from incoming stream
            System.Drawing.Image ImageFromStream;
            //imgBef = System.Drawing.Image.FromFile(pFilePath);
            ImageFromStream = System.Drawing.Image.FromStream(stream);

            // Create resized image
            System.Drawing.Image ResizedImage;
            ResizedImage = ResizeImage(ImageFromStream, 15); //Imager.Resize(imgBef, pWidth, pHeight, true);

            //Save JPEG  
            //SaveJpeg(pFilePath + pOutputFileName, _imgR);

            // Return image as a byte array
            // Make this a utility method? return ConvertedImage();
            return Helpers.CreateArrayFromImage(ResizedImage);
            //using (MemoryStream memoryStream = new MemoryStream())
            //{
            //    _imgR.Save(memoryStream, ImageFormat.Jpeg);
            //    return memoryStream.ToArray();
            //}

            //Byte[] result = (Byte[])new ImageConverter().ConvertTo(_imgR, typeof(Byte[]));

            //return result;

        }
    }
}