using AForge.Video;
using System.IO;
using System.Net;

namespace SmartHouseSystem
{
   public class CameraViewHandler
    {
        MJPEGStream stream;
        public CameraViewHandler()
        {
            stream = new MJPEGStream("http://192.168.1.120/image/jpeg.cgi");
            stream.NewFrame += Stream_NewFrame;
        }

        private void Stream_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

        }
        public async void getFrame()
        {
            string sourceURL = "http://192.168.1.108/image/jpeg.cgi";
            byte[] buffer = new byte[1280 * 800];
            int read, total = 0;
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(sourceURL);
            WebResponse resp = await req.GetResponseAsync();

            Stream stream = resp.GetResponseStream();

            while ((read = stream.Read(buffer, total, 1000)) != 0)
            {
                total += read;
            }
            //mscorlib
            //      Bitmap bmp = (Bitmap)Bitmap.FromStream(new MemoryStream(buffer, 0, total));

        }
    }
}
