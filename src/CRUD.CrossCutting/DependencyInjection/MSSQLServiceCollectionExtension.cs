using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CRUD.Infrastructure.Repositories.Context;


namespace CRUD.CrossCutting.DependencyInjection
{
    public static class MSSQLServiceCollectionExtension
    {
        public static IServiceCollection AddMSSQL(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Infrastructure.Repositories.Context.AppContext>(option => option.UseSqlServer(configuration.GetConnectionString("ServerConnection")));
            
            return services;
        }
    }
}