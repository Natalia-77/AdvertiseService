using System.ComponentModel;

namespace AdvertisePudlish.Models
{
    public class OperationType
    {
        /// <summary>
        /// Find by title
        /// </summary>
        [DefaultValue(null)]
        public string Title { get; set; }

    }
}
