using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
   // [Authorize]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;
        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromRoute] int restaurantId, [FromBody] CreateDishDto dto)
        {
           var newDishId =  await _dishService.Create(restaurantId, dto);

           return Created($"api/restaurant/{restaurantId}/dish/{newDishId}", null);
           
        }

        [HttpDelete("{dishId}")]

        public async  Task<ActionResult> Delete([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            await _dishService.Delete(restaurantId, dishId);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<List<DishDto>>> GetAll([FromRoute] int restaurantId, [FromQuery] DishQuery query)
        {
            var dishes =  await _dishService.GetAll(restaurantId, query);
            return Ok(dishes);
        }

        [HttpGet("{dishId}")]
        public async Task<ActionResult<DishDto>> GetById([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            var dish = await _dishService.GetById(restaurantId, dishId);
            return Ok(dish);
        }
    }
}
