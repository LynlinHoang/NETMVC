﻿using SV20T1020056.DomainModels;

namespace SV20T1020056.Web.Models
{
    public class OrderDetailModel
    {
        public Order Order { get; set; }
        public List<OrderDetail> Details { get; set; } 
    }
}
