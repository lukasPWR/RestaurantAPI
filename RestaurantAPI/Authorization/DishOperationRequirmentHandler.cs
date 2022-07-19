using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Authorization
{
    public class DishOperationRequirmentHandler :AuthorizationHandler<DishOperationRequirment,Dish>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DishOperationRequirment requirement, Dish dish)
        {

            if (requirement.DishOperation == DishOperation.Read ||
                requirement.DishOperation == DishOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (dish.CreatedById == int.Parse(userId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
