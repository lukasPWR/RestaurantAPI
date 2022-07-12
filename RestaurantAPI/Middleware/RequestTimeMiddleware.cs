﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RestaurantAPI.Middleware
{
    public class RequestTimeMiddleware: IMiddleware
    {
        private readonly ILogger<RequestTimeMiddleware> _logger;
        private Stopwatch _stopwatch;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _logger = logger;
            _stopwatch = new Stopwatch();
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopwatch.Start();
            await next.Invoke(context);
            _stopwatch.Stop();

           var elapsedMiliseconds = _stopwatch.ElapsedMilliseconds;

           if (elapsedMiliseconds / 1000 > 4)
           {
               var message =
                   $"Request [{context.Request.Method}] at {context.Request.Path} took {elapsedMiliseconds} ms";

               _logger.LogInformation(message); 
            }
        }
    }
}
