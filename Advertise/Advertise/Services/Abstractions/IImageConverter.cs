using System.Drawing;

namespace AdvertisePudlish.Services.Abstractions
{
    public interface IImageConverter
    {
        public Bitmap FromBase64StringToImage(string base64String);
    }
}
