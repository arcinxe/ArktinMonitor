using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace ArktinMonitor.Helpers
{
    public static class Base64Converter
    {
        public static string ImageToBase64(Image image,
              System.Drawing.Imaging.ImageFormat format)
        {
            using (var ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                var imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                var base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public static Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            var imageBytes = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                // Convert byte[] to Image
                ms.Write(imageBytes, 0, imageBytes.Length);
                var image = Image.FromStream(ms, true);
                return image;
            }
        }

        public static string PngFileToBase64(string filepath)
        {
            try
            {
                var img = Image.FromFile(filepath);
                var result = ImageToBase64(img, ImageFormat.Png);
                return result;
            }
            catch (Exception e)
            {
                LocalLogger.Log(nameof(PngFileToBase64), e);
                return null;
            }
        }
    }
}