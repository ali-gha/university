using System;

namespace University.ServiceLayer.Abstracts
{
    public interface ISeedDb
    {
         bool EnsureCreatedDb(IServiceProvider serviceProvider);
    }
}