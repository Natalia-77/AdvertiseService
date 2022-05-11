using AdvertisePublish.Models;

namespace AdvertisePudlish.Models
{
    public class AdvertiseViewModel
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime DateCreate { get; set; }
        public List<ImageViewModel> ImageList { get; set; }
    }
}
