using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using University.DataLayer.Context;
using University.ServiceLayer.Abstracts;
using University.ServiceLayer.Concretes;

namespace University.ServiceLayer
{
    public static class ServiceLayerExtension
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection serviceCollection,IConfiguration Configuration)
        {
            serviceCollection.AddDbContext<UniversityContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            serviceCollection.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            serviceCollection.AddScoped<ICourseService, CourseService>();
            serviceCollection.AddScoped<IStudentService, StudentService>();
            serviceCollection.AddScoped<ISeedDb, SeedDb>();
            return serviceCollection;
        }
    }
}