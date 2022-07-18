using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Services;

namespace RestaurantAPI.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationReuirement, Restaurant>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationReuirement requirement,
            Restaurant restaurant)
        {
            if (requirement.ResourceOperation == ResourceOperation.Read ||
                requirement.ResourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (restaurant.CreatedById == int.Parse(userId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
