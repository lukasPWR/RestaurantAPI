using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace RestaurantAPI.Authorization
{
    public class MinimumAgeRequirmentHandler: AuthorizationHandler<MinimumAgeRequirment>
    {
        private readonly ILogger<MinimumAgeRequirmentHandler> _logger;

        public MinimumAgeRequirmentHandler(ILogger<MinimumAgeRequirmentHandler> logger)
        {
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirment requirement)
        {
           var dateOfBirth =  DateTime.Parse(context.User.FindFirst(c => c.Type == "DateOfBirth").Value);

           var userEmail = context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;

           _logger.LogInformation($"User: {userEmail} with date of birth: {dateOfBirth}");

           if (dateOfBirth.AddYears(requirement.MinimumAge) <= DateTime.Now)
           {
               _logger.LogInformation("Authorizaton succedded");
               context.Succeed(requirement);
           }
           else
           {
               _logger.LogInformation("Authorization failed");
           }
           return Task.CompletedTask;
        }
    }
}
