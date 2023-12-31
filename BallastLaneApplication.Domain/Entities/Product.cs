﻿using BallastLaneApplication.Domain.Entities.Base;

namespace BallastLaneApplication.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string UserId { get; set; }
    }
}