using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NegotiationsAPI.Application.Services;
using NegotiationsAPI.Application.Interfaces;
using NegotiationsAPI.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegotiationsAPI.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<INegotiationService, NegotiationService>();

            return services;
        }
    }
}
