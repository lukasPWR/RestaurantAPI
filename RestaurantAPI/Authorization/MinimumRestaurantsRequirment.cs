using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class MinimumRestaurantsRequirment : IAuthorizationRequirement
    {
        public int MinimumRestaurants { get;}

        public MinimumRestaurantsRequirment(int restaurantsRequirment)
        {
            MinimumRestaurants = restaurantsRequirment;
        }
    }
}
