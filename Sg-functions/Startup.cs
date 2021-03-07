using DataAccess.DbContexts;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sg_functions.Helpers;
using System;
using System.Collections.Generic;
using System.Text;


[assembly: FunctionsStartup(typeof(Sg_functions.Startup))]
namespace Sg_functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.Services.BuildServiceProvider().GetService<IConfiguration>();
            var connString = configuration.GetConnectionString("SqlServerConnection");
            builder.Services.AddDbContext<SGContext>(x =>
             {
                 x.UseSqlServer(connString
                 , options => options.EnableRetryOnFailure());
             });
            builder.Services.AddTransient<SGContext>();
            builder.Services.AddTransient<PlantCareHelper>();

        }
    }
}