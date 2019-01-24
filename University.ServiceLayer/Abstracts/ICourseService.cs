using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using University.BusinessLayer.Models;

namespace University.ServiceLayer.Abstracts
{
    public interface ICourseService : IGenericService<Course>
    {
        // new Task<List<Course>> GetAllAsync();
        new Task<Course> GetByIdAsync(int id);
        new Task<Course> GetByIdAsyncAsNotTracked(int id);
    }
}