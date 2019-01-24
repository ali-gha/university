using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using University.Tests.TestHelper;
using University.ViewModels.CourseStudentViewModels;
using University.ViewModels.CourseViewModels;
using Xunit;
namespace University.Tests.IntegrationTests
{
    [Collection("Database collection")]
    public class CourseRestApiShould 
    {
        public HttpClient Client { get; }
        public CourseRestApiShould(SetupFixture fixture)
        {
            Client = fixture.Client;
        }
        [Fact]
        public async Task ReturnAllCourses()
        {
            var response = await Client.GetAsync("api/Course");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Psychology", responseString);
        }

        [Fact]
        public async Task ReturnCourseById()
        {
            var response = await Client.GetAsync("api/Course/2");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Sigmund Freud", responseString);
        }

        [Fact]
        public async Task ReturnNotFoundResponseIfCourseNotExist()
        {
            var response = await Client.GetAsync("api/Course/40");

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("Entity does not exist.", responseString);
        }

        [Fact]
        public async Task ReturnUnprocessableEntityResponseIfInputIsNull()
        {
            var response = await Client.PostAsJsonAsync<CourseForCreateViewModel>("/api/course", null);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task ReturnUnprocessableEntityResponseIfModelstateNotValid()
        {
            var courseToAdd = new CourseForCreateViewModel();
            var response = await Client.PostAsJsonAsync<CourseForCreateViewModel>("/api/course", courseToAdd);

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Contains("The Name field is required.", responseString);
            Assert.Contains("The TeacherName field is required.", responseString);
            Assert.Contains("The Location field is required.", responseString);
        }

        [Fact]
        public async Task ReturnCreatedAtRouteResponseIfEverythingIsFine()
        {
            var courseToAdd = new CourseForCreateViewModel()
            {
                Name = "Film Study",
                TeacherName = "Stanley Kubrick",
                Location = "Building 6 Room 601",
                Students = new List<CourseStudentForCreateViewModel>()
                {
                    new CourseStudentForCreateViewModel()
                    {
                        StudentId = 4
                    },
                    new CourseStudentForCreateViewModel()
                    {
                        StudentId = 5
                    },
                    new CourseStudentForCreateViewModel()
                    {
                        StudentId = 5
                    }
                }
            };
            var response = await Client.PostAsJsonAsync<CourseForCreateViewModel>("/api/course", courseToAdd);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
        }

        [Fact]
        public async Task ReturnNoContentResponseAfterEntityUpdated()
        {
            var courseToUpdate = new CourseForUpdateViewModel()
            {
                Name = "Philosophy",
                TeacherName = "Baruch Spinoza",
                Location = "Building 5 Room 550",
                Students = new List<CourseStudentForUpdateViewModel>()
                {
                    new CourseStudentForUpdateViewModel(){
                        StudentId = 3,
                        GPA = 2.5
                    },
                    new CourseStudentForUpdateViewModel(){
                        StudentId = 4,
                        GPA = 0
                    }
                },
            };
            var response = await Client.PutAsJsonAsync<CourseForUpdateViewModel>("/api/course/3", courseToUpdate);
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task ReturnNoContentResponseAfterEntityDeleted()
        {
            var response = await Client.DeleteAsync("/api/course/1");
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
