using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.WebApiRestful.Authentication.Service;
using Sample.WebApiRestful.Data;
using Sample.WebApiRestful.Data.Abstract;
using Sample.WebApiRestful.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.WebApiRestful.Infrastucture.Configuration
{
    public static class ConfigurationService
    {
        public static void RegisterContextDb(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<SampleWebApiContext> (options => 
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        public static void RegisterDI(this IServiceCollection service)
        {
            service.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            service.AddScoped<IDapperHelper, DapperHelper>();

            service.AddScoped<ICategoryService, CategoryService>();
            service.AddScoped<IUserService, UserService>();
            service.AddScoped<ITokenHandler, TokenHandler>();
        }
    }
}
