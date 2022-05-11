

namespace AdvertisePublish.Models
{
    public class CreateAdvertiseViewModel
    {
       // private DateTime dateCreate = DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm");

        //public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime DateCreate { get; set; }
        public List<ImageViewModel> Images { get; set; }
    }
}
