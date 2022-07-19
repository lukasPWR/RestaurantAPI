using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class MinimumDishesRequirment : IAuthorizationRequirement
    {
        public int MinimumDishes { get;}

        public MinimumDishesRequirment(int minimumDishes)
        {
            MinimumDishes = minimumDishes;
        }
    }
}
