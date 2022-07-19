using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Authorization
{
    public class MinimumDishesRequirmentHandler: AuthorizationHandler<MinimumDishesRequirment>
    {
        private readonly RestaurantDbContext _dbContext;

        public MinimumDishesRequirmentHandler(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumDishesRequirment requirement)
        {
            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var dishesCreatedCount = _dbContext.Dishes.Count(d => d.CreatedById == userId);

            if (dishesCreatedCount >= requirement.MinimumDishes)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
