using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Services
{
    public enum ResourceOperation
    {
        Create,
        Read,
        Update,
        Delete

    }

    public class ResourceOperationReuirement: IAuthorizationRequirement
    {
        public ResourceOperation ResourceOperation{ get; set; }

        public ResourceOperationReuirement(ResourceOperation resourceOperation)
        {
            ResourceOperation = resourceOperation;
        }
    }
}
