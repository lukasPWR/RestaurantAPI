using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public PagedResult<DishDto> GetAll(int restaurantId, DishQuery query);

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

        public PagedResult<DishDto> GetAll(int restaurantId, DishQuery query)
        {

            var restaurant = _context.Restaurants
                .Include(d => d.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant is null)
                throw new NotFoundExceptions("Restaurant not found");

            var baseQuery = restaurant.Dishes.Where(d => query.SearchPhrase == null ||
                                                         (d.Name.ToLower().Contains(query.SearchPhrase.ToLower()) ||
                                                          d.Price.ToString().Contains(query.SearchPhrase)));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelector = new Dictionary<string, Expression<Func<Dish, object>>>()
                {
                    { nameof(Dish.Name), d => d.Name },
                    { nameof(Dish.Price), d => d.Price }

                };
                var selectedColumn = columnsSelector[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC
                    ? baseQuery.OrderBy(d => d.Name)
                    : baseQuery.OrderByDescending(d => d.Price);
            }

            var dish = baseQuery.Skip(query.PageSize * (query.PageNumber - 1)).Take(query.PageSize).ToList();
            var totalItemsCount = baseQuery.Count();

            var dishDtos = _mapper.Map<List<DishDto>>(dish);
            var result = new PagedResult<DishDto>(dishDtos, totalItemsCount, query.PageSize, query.PageNumber);

            return result;
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
