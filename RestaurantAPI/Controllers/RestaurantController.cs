using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController] // walidacja
    
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromBody] UpdateRestaurantDto dto, [FromRoute] int id)
        {
           

             await _restaurantService.Update(dto, id);

             return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
           await _restaurantService.Delete(id);
            
           return NotFound();
        }

        [HttpDelete("DeleteByName/{name}")]
        public async Task<ActionResult> DeleteOnName([FromRoute] string name)
        {
           await _restaurantService.DeleteOnName(name);
            return NotFound();
        }
        [HttpGet]
       
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll([FromQuery] RestaurantQuery query)
        {
            var restaurantsDtos = await _restaurantService.GetAll(query);

            return Ok(restaurantsDtos);
        }
        [HttpGet("{id}")]
        
        public async Task<ActionResult<RestaurantDto>> Get([FromRoute] int id)
        {
           var restaurant = await _restaurantService.GetById(id);
            

            return Ok(restaurant);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]

        public async Task<ActionResult> CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            var userId =  int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
           var id = await _restaurantService.Create(dto);
            

            return Created($"/api/restaurant/{id}", null);
        }

       
       

    }
}
