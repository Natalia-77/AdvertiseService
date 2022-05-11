

namespace AdvertisePublish.Models
{
    public class CreateAdvertiseViewModel
    {

        //public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime DateCreate { get; set; }
        public List<ImageViewModel> Images { get; set; }
    }
}
