using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using University.Api;
using University.Api.Controllers;
using University.BusinessLayer.Models;
using University.DataLayer.Context;
using University.ServiceLayer.Concretes;
using University.Tests.TestHelper;
using University.ViewModels.CourseViewModels;
using Xunit;

namespace University.Tests.IntegrationTests
{
    public class CourseControllerShould :  IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly IMapper _mapper;
        public CourseControllerShould(WebApplicationFactory<Startup> testServer)
        {
            testServer.CreateClient();
            _mapper = testServer.Server.Host.Services.CreateScope().ServiceProvider.GetRequiredService<IMapper>();
        }
        [Fact]
        public async Task ReturnAllCourses()
        {
            var options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "Get-all_controller")
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
                var ctrl = new CourseController(_mapper, service);
                var courses = await ctrl.Get();
                var okResult = Assert.IsType<OkObjectResult>(courses.Result);
                var items = Assert.IsType<List<CourseViewModel>>(okResult.Value);
                Assert.Equal(3, items.Count());
            }
        }

        [Fact]
        public async Task ReturnCourseById()
        {
            var options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "Get-by-id_controller")
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
                var ctrl = new CourseController(_mapper, service);
                var courses = await ctrl.Get(context.Courses.First().Id);
                var okResult = Assert.IsType<OkObjectResult>(courses.Result);
                var item = Assert.IsType<CourseViewModel>(okResult.Value);
                Assert.Equal("English", item.Name);
            }
        }

        [Fact]
        public async Task CreateCourse()
        {
            var options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "Add_to_controller")
                .Options;

            using (var context = new UniversityContext(options))
            {
                var courseViewModelForCreate = new CourseForCreateViewModel()
                {
                    Name = "English",
                    TeacherName = "William Shakespeare",
                };
                var service = new CourseService(context);
                var ctrl = new CourseController(_mapper, service);
                var result = await ctrl.Post(courseViewModelForCreate);
                var okResult = Assert.IsType<CreatedAtRouteResult>(result);
                var course = Assert.IsType<CourseForCreateViewModel>(okResult.Value);
                Assert.Equal("English", course.Name);
            }
        }

        [Fact]
        public async Task UpdateCourse()
        {
            var options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "Update_to_controller")
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
                var courseViewModelForCreate = new CourseForUpdateViewModel()
                {
                    Name = "English",
                    TeacherName = "Jacques Lacan",
                    Location = "Building 3 Room 301"
                };
                var ctrl = new CourseController(_mapper, service);
                var result = await ctrl.Put(context.Courses.First().Id, courseViewModelForCreate);
                var okResult = Assert.IsType<NoContentResult>(result);
                Assert.Equal(204, okResult.StatusCode);
            }
        }

        [Fact]
        public async Task DeleteCourse()
        {
            var options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "Delete_from_controller")
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
                var ctrl = new CourseController(_mapper, service);
                var result = await ctrl.Delete(context.Courses.First().Id);
                var okResult = Assert.IsType<NoContentResult>(result);
                Assert.Equal(204, okResult.StatusCode);
            }
        }

        [Fact]
        public async Task ReturnNotFoundResponseIfEntityDoesNotExistDeleteMethod()
        {
            var options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "Delete-exception_controller")
                .Options;

            using (var context = new UniversityContext(options))
            {
                var service = new CourseService(context);
                var ctrl = new CourseController(_mapper, service);
                var actual = await ctrl.Delete(1);
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(actual);
                Assert.Equal(404, notFoundResult.StatusCode);
                Assert.Equal("Entity does not exist.", notFoundResult.Value);
            }

        }

        [Fact]
        public async Task ReturnBadRequestResponseIfRequestBodyIsNullUpdateMethod()
        {
            var options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "Update-exception_controller")
                .Options;

            using (var context = new UniversityContext(options))
            {
                var service = new CourseService(context);
                var ctrl = new CourseController(_mapper, service);
                var actual = await ctrl.Put(1, null);
                var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actual);
                Assert.Equal(400, badRequestObjectResult.StatusCode);
                Assert.Equal("Request body is null.", badRequestObjectResult.Value);
            }
        }

        [Fact]
        public async Task ReturnNotFoundResponseIfEntityDoesNotExistUpdateMethod()
        {
            var options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "Update-exception_controller")
                .Options;

            using (var context = new UniversityContext(options))
            {
                var courseViewModelForCreate = new CourseForUpdateViewModel()
                {
                    Name = "English",
                    TeacherName = "Jacques Lacan",
                    Location = "Building 3 Room 301"
                };
                var service = new CourseService(context);
                var ctrl = new CourseController(_mapper, service);
                var actual = await ctrl.Put(1, courseViewModelForCreate);
                var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(actual);
                Assert.Equal(404, notFoundObjectResult.StatusCode);
                Assert.Equal("Entity does not exist.", notFoundObjectResult.Value);
            }
        }

        [Fact]
        public async Task ReturnBadRequestResponseIfRequestBodyIsNullCreateMethod()
        {
            var options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "Create-exception_controller")
                .Options;

            using (var context = new UniversityContext(options))
            {
                var service = new CourseService(context);
                var ctrl = new CourseController(_mapper, service);
                var actual = await ctrl.Post(null);
                var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actual);
                Assert.Equal(400, badRequestObjectResult.StatusCode);
                Assert.Equal("Request body is null.", badRequestObjectResult.Value);
            }
        }
    }
}
