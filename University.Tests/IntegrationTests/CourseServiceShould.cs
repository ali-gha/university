using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using University.BusinessLayer.Models;
using University.DataLayer.Context;
using University.ServiceLayer.Concretes;
using Xunit;

namespace University.Tests.IntegrationTests
{
    public class CourseServiceShould 
    {

        [Fact]
        public async Task ReturnAllCourses()
        {
            var options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "Get-all_database")
                .Options;
            using (var context = new UniversityContext(options))
            {
                List<Course> courses = new List<Course>()
                {
                    new Course
                    {
                        Name = "English",
                        TeacherName = "William Shakespeare",
                        Location = "Building 3 Room 301"
                    },
                    new Course
                    {
                        Name = "Psychology",
                        TeacherName = "Sigmund Freud",
                        Location = "Building 4 Room 401"
                    },
                    new Course
                    {
                        Name = "Philosophy",
                        TeacherName = "Friedrich Nietzsche",
                        Location = "Building 5 Room 501"
                    }
                };
                await context.Courses.AddRangeAsync(courses);
                await context.SaveChangesAsync();
            }

            using (var context = new UniversityContext(options))
            {
                var service = new CourseService(context);
                var courses = await service.GetAllAsync();
                Assert.Equal(3, courses.Count());
            }
        }

        [Fact]
        public async Task ReturnCourseById()
        {
            var options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "Get-by-id_database")
                .Options;
            using (var context = new UniversityContext(options))
            {
                List<Course> courses = new List<Course>()
                {
                    new Course
                    {
                        Name = "English",
                        TeacherName = "William Shakespeare",
                        Location = "Building 3 Room 301"
                    },
                    new Course
                    {
                        Name = "Psychology",
                        TeacherName = "Sigmund Freud",
                        Location = "Building 4 Room 401"
                    },
                    new Course
                    {
                        Name = "Philosophy",
                        TeacherName = "Friedrich Nietzsche",
                        Location = "Building 5 Room 501"
                    }
                };
                await context.Courses.AddRangeAsync(courses);
                await context.SaveChangesAsync();
            }

            using (var context = new UniversityContext(options))
            {
                var service = new CourseService(context);
                var course = await service.GetByIdAsync(1);
                Assert.Equal("English", course.Name);
            }
        }

        [Fact]
        public async Task CreateCourse()
        {
            var options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "Add_to_database")
                .Options;

            using (var context = new UniversityContext(options))
            {
                var curse = new Course
                {
                    Name = "English",
                    TeacherName = "William Shakespeare",
                    Location = "Building 3 Room 301"
                };
                var service = new CourseService(context);
                await service.CreateAsync(curse);
            }

            using (var context = new UniversityContext(options))
            {
                Assert.Equal(1, context.Courses.Count());
                Assert.Equal("English", context.Courses.Single().Name);
            }
        }

        [Fact]
        public async Task UpdateCourse()
        {
            var options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "Update_to_database")
                .Options;

            using (var context = new UniversityContext(options))
            {
                List<Course> courses = new List<Course>()
                {
                    new Course
                    {
                        Name = "English",
                        TeacherName = "William Shakespeare",
                        Location = "Building 3 Room 301"
                    },
                    new Course
                    {
                        Name = "Psychology",
                        TeacherName = "Sigmund Freud",
                        Location = "Building 4 Room 401"
                    },
                    new Course
                    {
                        Name = "Philosophy",
                        TeacherName = "Friedrich Nietzsche",
                        Location = "Building 5 Room 501"
                    }
                };
                await context.Courses.AddRangeAsync(courses);
                await context.SaveChangesAsync();
            }

            using (var context = new UniversityContext(options))
            {
                var service = new CourseService(context);
                var course = await context.Courses.FirstAsync();
                course.TeacherName = "Jacques Lacan";
                await service.UpdateAsync(course);
            }

            using (var context = new UniversityContext(options))
            {
                var course = await context.Courses.FirstAsync();
                Assert.Equal("Jacques Lacan", course.TeacherName);
            }
        }

        [Fact]
        public async Task DeleteCourse()
        {
            var options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "Delete_from_database")
                .Options;

            using (var context = new UniversityContext(options))
            {
                List<Course> courses = new List<Course>()
                {
                    new Course
                    {
                        Name = "English",
                        TeacherName = "William Shakespeare",
                        Location = "Building 3 Room 301"
                    },
                    new Course
                    {
                        Name = "Psychology",
                        TeacherName = "Sigmund Freud",
                        Location = "Building 4 Room 401"
                    },
                    new Course
                    {
                        Name = "Philosophy",
                        TeacherName = "Friedrich Nietzsche",
                        Location = "Building 5 Room 501"
                    }
                };
                await context.Courses.AddRangeAsync(courses);
                await context.SaveChangesAsync();
            }

            using (var context = new UniversityContext(options))
            {
                var service = new CourseService(context);
                var course = await context.Courses.FirstAsync();
                await service.DeleteAsync(course);
            }

            using (var context = new UniversityContext(options))
            {
                Assert.Equal(2,context.Courses.Count());
            }
        }

        [Fact]
        public async Task ThrowExceptionIfInputIsNullForDeleteMethod()
        {
            var options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "Delete-exception_database")
                .Options;

            using (var context = new UniversityContext(options))
            {
                var service = new CourseService(context);
                ArgumentNullException ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.DeleteAsync(null));
                Assert.Equal("Value cannot be null.\r\nParameter name: entity"
                    , ex.Message);
            }
            
        }

        [Fact]
        public async Task ThrowExceptionIfInputIsNullForUpdateMethod()
        {
            var options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "Update-exception_database")
                .Options;

            using (var context = new UniversityContext(options))
            {
                 var service = new CourseService(context);
                ArgumentNullException ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.UpdateAsync(null));
                Assert.Equal("Value cannot be null.\r\nParameter name: entity"
                    , ex.Message);
            }
        }

        [Fact]
        public async Task ThrowExceptionIfInputIsNullForCreateMethod()
        {
            var options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "Create-exception_database")
                .Options;

            using (var context = new UniversityContext(options))
            {
                var service = new CourseService(context);
                ArgumentNullException ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.CreateAsync(null));
                Assert.Equal("Value cannot be null.\r\nParameter name: entity"
                    , ex.Message);
            }
        }
    }
}
