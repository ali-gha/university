using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using University.BusinessLayer.Models;
using University.DataLayer.Context;
using University.ServiceLayer.Abstracts;

namespace University.ServiceLayer.Concretes
{
    public class StudentService : GenericService<Student>, IStudentService
    {
        private readonly UniversityContext _ctx;
        private readonly DbSet<Student> dbSet;

        public StudentService(UniversityContext ctx) : base(ctx)
        {
            _ctx = ctx;
            if (_ctx == null)
                throw new ArgumentNullException(nameof(UniversityContext));
            dbSet = _ctx.Set<Student>();
        }

        public async Task<bool> FindStudentByName(string name)
        {
            return await dbSet.AnyAsync(x => x.Name.Equals(name));
        }
    }
}