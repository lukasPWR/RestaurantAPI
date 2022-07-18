using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Authorization
{
    public class MinimumRestaurantsRequirmentHandler : AuthorizationHandler<MinimumRestaurantsRequirment>
    {
        private readonly RestaurantDbContext _context;
        public MinimumRestaurantsRequirmentHandler(RestaurantDbContext context)
        {
            _context = context;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumRestaurantsRequirment requirement)
        {
            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var restaurantsCreatedCount = _context.Restaurants.Count(c => c.CreatedById == userId);

            if (restaurantsCreatedCount >= requirement.MinimumRestaurants)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
