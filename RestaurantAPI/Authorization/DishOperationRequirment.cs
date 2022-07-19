using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{

    public enum DishOperation
    {
        Create,
        Read,
        Delete
    }
    public class DishOperationRequirment : IAuthorizationRequirement
    {
        public DishOperation DishOperation { get;  }

        public DishOperationRequirment(DishOperation dishOperation)
        {
            DishOperation = dishOperation;
        }
    }
}
