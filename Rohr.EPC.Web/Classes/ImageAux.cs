using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Configuration;

namespace Rohr.EPC.Web.Classes
{
    public static class ImageAux
    {
        // ToDid:  -> web.config
        public static String UrlUpload()
        {
            var Res = new System.Text.StringBuilder(ConfigurationManager.AppSettings["DirDownload"]);

            return VirtualPathUtility.ToAbsolute(Res.ToString());
        }

        public static Bitmap ResizeImage(this Bitmap img,
            Int32 width, Int32 height, Boolean crop)
        {
            Int32 ImgW = img.Width, ImgH = img.Height;

            float PercW = width, PercH = height;       // Garante float
            PercW /= ImgW;
            PercH /= ImgH;

            float Mult = crop ?
               Math.Max(PercW, PercH) : Math.Min(PercW, PercH);

            Int32 NewW = (Int32)Math.Round((Mult * (float)ImgW));
            Int32 NewH = (Int32)Math.Round((Mult * (float)ImgH));

            Bitmap newImage = null;

            if (Mult <= 1)     // Não ampliar !
            {
                // Gerando nova imagem com máximo de qualidade
                newImage = new Bitmap(NewW, NewH);
                using (Graphics gr = Graphics.FromImage(newImage))
                {
                    gr.SmoothingMode = SmoothingMode.HighQuality;
                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    gr.DrawImage(img, new Rectangle(0, 0, NewW, NewH));
                }
            }
            else
                newImage = new Bitmap(img);

            if (crop)
            {
                Int32 OffW = Math.Max((NewW - width) / 2, 0);
                Int32 OffH = Math.Max((NewH - height) / 2, 0);

                // Se clonar o que não tem da execption !
                NewW = Math.Min(width, newImage.Width - OffW);
                NewH = Math.Min(height, newImage.Height - OffH);
                if (newImage != null)
                    newImage = newImage.Clone(new Rectangle(OffW, OffH, NewW, NewH), img.PixelFormat);
                else
                {
                    using (Graphics gr = Graphics.FromImage(newImage))
                    {
                        gr.SmoothingMode = SmoothingMode.HighQuality;
                        gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        gr.DrawImage(img, new Rectangle(0, 0, NewW, NewH));
                        //gr.DrawImage(img, new Rectangle(0, 0, NewW, NewH),
                        //    new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
                    }
                }
            }
            return newImage;
        }

        // http://stackoverflow.com/questions/41665/bmp-to-jpg-png-in-c-sharp

        /// <summary>
        /// Grava Imagem especificando caminho e qualidade
        /// </summary>
        /// <param name="img"></param>
        /// <param name="filePath"></param>
        /// <param name="quality">The range of useful values for the quality category is from 0 to 100</param>
        public static void SaveAsJpeg(this Image img, String filePath, Int64 quality)
        {
            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            img.Save(filePath, GetEncoder(ImageFormat.Jpeg), encoderParameters);
        }

        static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.Single(codec => codec.FormatID == format.Guid);
        }
    }
}