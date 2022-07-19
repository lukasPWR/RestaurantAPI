using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Entities
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string  Description { get; set; }
        public decimal Price { get; set; }
        public int? CreatedById { get; set; }


        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public virtual User CreatedBy { get; set; }
    }
}
