using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IDishService
    {
        public int Create(int restaurantId, CreateDishDto dto);

        public DishDto GetById(int restaurantId, int dishId);
        public List<DishDto> GetAll(int restaurantId);

        public void Delete(int restaurantId, int dishId);


    }
    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;
        public DishService(RestaurantDbContext context, IMapper mapper, IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public DishDto GetById(int restaurantId, int dishId)
        {
            var restaurant = _context.Restaurants.FirstOrDefault(r => r.Id == restaurantId);
            if (restaurant is null)
                throw new NotFoundExceptions("Restaurant not found");

            var dish = _context.Dishes.FirstOrDefault(d => d.Id == dishId);
            if (dish is null || restaurant.Id != restaurantId)
            {
                throw new NotFoundExceptions("Dish not found");
            }

            var dishDto = _mapper.Map<DishDto>(dish);
            return dishDto;
        }

        public void Delete(int restaurantId, int dishId)
        {
            var restaurant = _context.Restaurants.Include(d => d.Dishes).FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant is null)
                throw new NotFoundExceptions("Restaurant not found");

            var dish = _context.Dishes.FirstOrDefault(d => d.Id == dishId);
            if (dish is null || restaurant.Id != restaurantId)
            {
                throw new NotFoundExceptions("Dish not found");
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, dish,
                new DishOperationRequirment(DishOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            _context.Dishes.RemoveRange(dish);
            _context.SaveChanges();

        }

        public List<DishDto> GetAll(int restaurantId)
        {
            var restaurant = _context.Restaurants
                .Include(d => d.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant is null)
                throw new NotFoundExceptions("Restaurant not found");

            var dishDtos = _mapper.Map<List<DishDto>>(restaurant.Dishes);
            return dishDtos;
        }


        public int Create(int restaurantId, CreateDishDto dto)
        {
            var restaurant = _context.Restaurants.FirstOrDefault(r => r.Id == restaurantId);
            if (restaurant is null)
                throw new NotFoundExceptions("Restaurant not found");

            var dishEntity = _mapper.Map<Dish>(dto);
            dishEntity.CreatedById = _userContextService.GetUserId;
            dishEntity.RestaurantId = restaurantId;
            _context.Dishes.Add(dishEntity);
            _context.SaveChanges();


            return dishEntity.Id;


        }
    }
}
