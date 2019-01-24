using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using University.Tests.TestHelper;
using University.ViewModels.StudentViewModels;
using Xunit;
namespace University.Tests.IntegrationTests
{
    [Collection("Database collection")]
    public class StudentRestApiShould
    {
        public HttpClient Client { get; }
        public StudentRestApiShould(SetupFixture fixture)
        {
            Client = fixture.Client;
        }
        [Fact]
        public async Task ReturnAllStudents()
        {
            var response = await Client.GetAsync("api/Student");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Jacques Marie Émile Lacan", responseString);
        }

        [Fact]
        public async Task ReturnStudentById()
        {
            var response = await Client.GetAsync("api/Student/2");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Jacques Marie Émile Lacan", responseString);
        }

        [Fact]
        public async Task ReturnNotFoundResponseIfStudentNotExist()
        {
            var response = await Client.GetAsync("api/Student/40");

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("Entity does not exist.", responseString);
        }

        [Fact]
        public async Task ReturnUnprocessableEntityResponseIfModelstateNotValid()
        {
            var studentToAdd = new StudentForCreateViewModel();
            var response = await Client.PostAsJsonAsync<StudentForCreateViewModel>("/api/Student", studentToAdd);

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Contains("The Name field is required.", responseString);
            Assert.Contains("The Age field is required.", responseString);
        }

        [Fact]
        public async Task ReturnCreatedAtRouteResponseIfEverythingIsFine()
        {
            var studentToAdd = new StudentForCreateViewModel()
            {
                Name = "Steve Jobs",
                Age = 22,
            };
            var response = await Client.PostAsJsonAsync<StudentForCreateViewModel>("/api/Student", studentToAdd);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
        }

        [Fact]
        public async Task ReturnNoContentResponseAfterEntityUpdated()
        {
            var studentToUpdate = new StudentForUpdateViewModel()
            {
                Name = "Scott Hanselman",
                Age = 22,
                GPA = 2.4
            };
            var response = await Client.PutAsJsonAsync<StudentForUpdateViewModel>("/api/Student/3", studentToUpdate);
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task ReturnNoContentResponseAfterEntityDeleted()
        {
            var response = await Client.DeleteAsync("/api/Student/1");
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}