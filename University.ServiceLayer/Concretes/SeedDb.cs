using System;
using Microsoft.Extensions.DependencyInjection;
using University.DataLayer.Context;
using University.ServiceLayer.Abstracts;

namespace University.ServiceLayer.Concretes
{
    public class SeedDb : ISeedDb
    {
        public bool EnsureCreatedDb(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<UniversityContext>();
            return context.Database.EnsureCreated();
        }
    }
}