﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020056.DomainModels
{
    public class ProductPhoto
    {
        public long PhotoID { get; set; }
        public int ProductID { get; set; }
        public string Photo { get; set; } = "";
        public string Description { get; set; } = "";
        public string DisplayOrder { get; set; } 
        public bool IsHidden { get; set; }

    }
}
