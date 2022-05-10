using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
   
    public class Advertise
    {
        public int Id { get; set; }
        public string  Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public ICollection <Image> Images { get; set; }
    }
}
