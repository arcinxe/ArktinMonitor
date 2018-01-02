using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArktinMonitor.Helpers
{
    public static class ScreenCapture
    {
      public static void CaptureScreenToFile(string filePath)
        {
            //Bitmap captureBitmap = new Bitmap(int width, int height, PixelFormat);
            //Creating a Rectangle object which will  
            //capture our Current Screen
            var captureRectangle = Screen.AllScreens[0].Bounds;

            //Creating a new Bitmap object
            var captureBitmap = new Bitmap(captureRectangle.Width/2, captureRectangle.Height/2, PixelFormat.Format32bppArgb);

            //Creating a New Graphics Object
            var captureGraphics = Graphics.FromImage(captureBitmap);

            //Copying Image from The Screen
            captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);

            //Saving the Image File (I am here Saving it in My E drive).
            captureBitmap.Save(Path.Combine(filePath), ImageFormat.Png);
        }

    }
}
