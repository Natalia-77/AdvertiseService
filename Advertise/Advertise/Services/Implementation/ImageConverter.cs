using AdvertisePudlish.Services.Abstractions;
using System.Drawing;

namespace AdvertisePudlish.Services.Implementation
{
    public class ImageConverter : IImageConverter
    {
        public Bitmap FromBase64StringToImage(string base64String)
        {
            byte[] byteBuffer = Convert.FromBase64String(base64String);
            try
            {
                using (MemoryStream memoryStream = new MemoryStream(byteBuffer))
                {
                    memoryStream.Position = 0;
                    Image imgReturn;
                    imgReturn = Image.FromStream(memoryStream);
                    memoryStream.Close();
                    byteBuffer = null;
                    return new Bitmap(imgReturn);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
