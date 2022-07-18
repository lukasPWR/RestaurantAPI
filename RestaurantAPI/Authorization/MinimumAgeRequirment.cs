using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class MinimumAgeRequirment :IAuthorizationRequirement
    {
        public int MinimumAge { get; }

        public MinimumAgeRequirment(int minimumAge)
        {
            MinimumAge = minimumAge;
        }


    }
}
