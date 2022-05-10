﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AdvertiseId { get; set; }
        public virtual Advertise Advertise { get; set; }
    }
}
