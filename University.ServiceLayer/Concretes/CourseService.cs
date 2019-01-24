using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using University.BusinessLayer.Models;
using University.DataLayer.Context;
using University.ServiceLayer.Abstracts;
using System.Linq.Dynamic.Core;

namespace University.ServiceLayer.Concretes
{
    public class CourseService : GenericService<Course>, ICourseService
    {
        private readonly UniversityContext _ctx;
        private readonly DbSet<Course> dbSet;
        public CourseService(UniversityContext ctx) : base(ctx)
        {
            _ctx = ctx;
            if (_ctx == null)
                throw new ArgumentNullException(nameof(UniversityContext));
            dbSet = _ctx.Set<Course>();
        }

        // public override async Task<List<Course>> GetAllAsync()
        // {
        //     return await dbSet.Include(x => x.CourseStudents).ThenInclude(s => s.Student).ToListAsync();
        // }
        public override async Task<Course> GetByIdAsync(int id)
        {
            return await dbSet.Include(x => x.CourseStudents).FirstOrDefaultAsync(x => x.Id == id);
        }
        public override async Task<Course> GetByIdAsyncAsNotTracked(int id)
        {
            return await dbSet.AsNoTracking().Where($"Id = {id}").Include(x => x.CourseStudents).ThenInclude(s => s.Student).SingleOrDefaultAsync();
        }
    }
}