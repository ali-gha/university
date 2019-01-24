using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using University.BusinessLayer.Models;
using University.DataLayer.Context;
using University.ServiceLayer.Concretes;
using University.Tests.TestHelper;
using Xunit;

namespace University.Tests.UnitTests
{
    public class CourseServiceShould
    {
        Mock<UniversityContext> _dbcontext;
        Mock<DbSet<Course>> _courseDbSet;
        List<Course> courses;
        public CourseServiceShould()
        {
            _dbcontext = new Mock<UniversityContext>();
            courses = new List<Course>
            {
                new Course
                {
                    Id = 1,
                    Name = "English",
                    TeacherName = "William Shakespeare"
                },
                new Course
                {
                    Id = 2,
                    Name = "Psychology",
                    TeacherName = "Sigmund Freud"
                },
                new Course
                {
                    Id = 3,
                    Name = "Philosophy",
                    TeacherName = "Friedrich Wilhelm Nietzsche"
                }

            };

            _courseDbSet = courses.ToAsyncDbSetMock();

            _dbcontext.Setup(x => x.Set<Course>()).Returns(_courseDbSet.Object);

            _dbcontext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        }
        [Fact]
        public async Task ReturnAllCourses()
        {
            var service = new CourseService(_dbcontext.Object);
            var actual = await service.GetAllAsync();

            Assert.Equal(3, actual.Count());
            Assert.Equal("English", actual.First().Name);

        }
        [Fact]
        public async Task ReturnCourseById()
        {
            _courseDbSet.Setup(x => x.FindAsync(It.IsAny<int>())).ReturnsAsync(courses.Where(x => x.Id == 1).FirstOrDefault());

            var service = new CourseService(_dbcontext.Object);
            var actual = await service.GetByIdAsync(1);

            Assert.Equal("English", actual.Name);
        }

        [Fact]
        public async Task CreateCourse()
        {
            var newCourse = new Course
            {
                Id = 4,
                Name = "Film Study",
                TeacherName = "Stanley Kubrick"
            };
            _courseDbSet.Setup(x => x.AddAsync(It.IsAny<Course>(), It.IsAny<CancellationToken>())).Callback(() =>
                {
                    courses.Add(newCourse);
                }).Returns((Course model, CancellationToken token) => Task.FromResult((EntityEntry<Course>)null));

            var service = new CourseService(_dbcontext.Object);
            var actual = await service.CreateAsync(newCourse);

            Assert.Equal(1, actual);
            Assert.Equal(4, courses.Count());
            Assert.Equal("Film Study", courses.Find(x => x.Id == 4).Name);
        }

        [Fact]
        public async Task UpdateCourse()
        {
            var course = courses.Find(x => x.Id == 2);

            _courseDbSet.Setup(x => x.Update(It.IsAny<Course>())).Callback(() =>
                {
                    course.TeacherName = "Jacques Marie Émile Lacan";
                }).Returns((Course model) => ((EntityEntry<Course>)null));
            var service = new CourseService(_dbcontext.Object);
            var actual = await service.UpdateAsync(course);

            Assert.Equal(1, actual);
            Assert.Equal(3, courses.Count());
            Assert.Equal("Jacques Marie Émile Lacan", courses.Find(x => x.Id == 2).TeacherName);
        }

        [Fact]
        public async Task DeleteCourse()
        {
            var course = courses[0];
            _courseDbSet.Setup(x => x.Remove(It.IsAny<Course>())).Callback(() =>
                {
                    courses.Remove(course);
                }).Returns((Course model) => ((EntityEntry<Course>)null));
            var service = new CourseService(_dbcontext.Object);
            var actual = await service.DeleteAsync(course);

            Assert.Equal(1, actual);
            Assert.Equal(2, courses.Count());
        }

        [Fact]
        public async Task ThrowExceptionIfInputIsNullForDeleteMethod()
        {
            var service = new CourseService(_dbcontext.Object);
            ArgumentNullException ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.DeleteAsync(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: entity"
               , ex.Message);

            _dbcontext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
        
        [Fact]
        public async Task ThrowExceptionIfInputIsNullForUpdateMethod()
        {
           
             var service = new CourseService(_dbcontext.Object);
            ArgumentNullException ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.UpdateAsync(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: entity"
               , ex.Message);

            _dbcontext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
        
        [Fact]
        public async Task ThrowExceptionIfInputIsNullForCreateMethod()
        {
            var service = new CourseService(_dbcontext.Object);
            ArgumentNullException ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.CreateAsync(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: entity"
               , ex.Message);

            _dbcontext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
