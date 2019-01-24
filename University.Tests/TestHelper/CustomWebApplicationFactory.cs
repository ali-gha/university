using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using University.Api;
using University.BusinessLayer.Models;
using University.DataLayer.Context;

namespace University.Tests.TestHelper
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkSqlite()
                    .BuildServiceProvider();
                ServiceCollectionExtensions.UseStaticRegistration = false;
                services.AddSingleton(provider =>
                {
                    var options = new DbContextOptionsBuilder<UniversityContext>()
                        .UseSqlite("Data Source=Database.db")
                        .UseInternalServiceProvider(serviceProvider)
                        .Options;
                    var universityContext = new UniversityContext(options);
                    universityContext.Database.EnsureCreated();
                    SeedDatabase(universityContext);
                    return universityContext;
                });
            });
        }

        private void SeedDatabase(UniversityContext _ctx)
        {
            _ctx.Database.EnsureCreated();
            var courses = new List<Course>
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
                    TeacherName = "Baruch Spinoza",
                    Location = "Building 5 Room 501"
                }
            };
            var students = new List<Student>
            {
                new Student
                {
                    Name = "Thomas Stearns Eliot",
                    Age = 18
                },
                new Student
                {
                    Name = "Jacques Marie Émile Lacan",
                    Age = 21
                },
                new Student
                {
                    Name = "Friedrich Nietzsche",
                    Age = 22
                },
                new Student
                {
                    Name = "Quentin Tarantino",
                    Age = 18
                },
                new Student
                {
                    Name = "Lars von Trier",
                    Age = 21
                },
                new Student
                {
                    Name = "Christopher Nolan",
                    Age = 22
                }
            };
            var courseStudents = new List<CourseStudent>
            {
                new CourseStudent
                {
                    Course = courses[0],
                    Student = students[0]
                },
                new CourseStudent
                {
                    Course = courses[1],
                    Student = students[1]
                },
                new CourseStudent
                {
                    Course = courses[2],
                    Student = students[2]
                },
                new CourseStudent
                {
                    Course = courses[2],
                    Student = students[0]
                },
                new CourseStudent
                {
                    Course = courses[1],
                    Student = students[0]
                },
                new CourseStudent
                {
                    Course = courses[1],
                    Student = students[2]
                }
            };
            _ctx.AddRange(courses);
            _ctx.AddRange(students);
            _ctx.AddRange(courseStudents);
            _ctx.SaveChanges();
        }
    }
}