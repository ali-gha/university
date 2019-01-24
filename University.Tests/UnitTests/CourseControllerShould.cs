using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using University.Api.Controllers;
using University.BusinessLayer.Models;
using University.ServiceLayer.Abstracts;
using University.ViewModels.CourseViewModels;
using Xunit;

namespace University.Tests.UnitTests
{
    public class CourseControllerShould
    {
        Mock<ICourseService> _courseService;
        Mock<IMapper> _mapper;
        List<CourseViewModel> coursesViewModel;
        List<Course> courses;
        public CourseControllerShould()
        {
            coursesViewModel = new List<CourseViewModel>
            {
                new CourseViewModel
                {
                    Id = 1,
                    Name = "English",
                    TeacherName = "William Shakespeare"
                },
                new CourseViewModel
                {
                    Id = 2,
                    Name = "Psychology",
                    TeacherName = "Sigmund Freud"
                },
                new CourseViewModel
                {
                    Id = 3,
                    Name = "Philosophy",
                    TeacherName = "Friedrich Wilhelm Nietzsche"
                }

            };
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

            _courseService = new Mock<ICourseService>();
            _courseService.Setup(x => x.GetAllAsync()).ReturnsAsync(courses);
            _courseService.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(courses.Find(x => x.Id == 1));
            _courseService.Setup(x => x.GetByIdAsyncAsNotTracked(It.IsAny<int>())).ReturnsAsync(courses.Find(x => x.Id == 1));
            _courseService.Setup(x => x.CreateAsync(It.IsAny<Course>())).ReturnsAsync(1);
            _courseService.Setup(x => x.UpdateAsync(It.IsAny<Course>())).ReturnsAsync(1);
            _courseService.Setup(x => x.DeleteAsync(It.IsAny<Course>())).ReturnsAsync(1);
            _mapper = new Mock<IMapper>();
            _mapper.Setup(m => m.Map<List<CourseViewModel>>(It.IsAny<List<Course>>())).Returns(coursesViewModel);
            _mapper.Setup(m => m.Map<CourseViewModel>(It.IsAny<Course>())).Returns(coursesViewModel.Find(x => x.Id == 1));
        }
        [Fact]
        public async Task ReturnAllCourses()
        {
            var courseCtrl = new CourseController(_mapper.Object, _courseService.Object);
            var actual = await courseCtrl.Get();

            var okResult = Assert.IsType<OkObjectResult>(actual.Result);
            var items = Assert.IsType<List<CourseViewModel>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }
        
        [Fact]
        public async Task ReturnCourseById()
        {
            var courseCtrl = new CourseController(_mapper.Object, _courseService.Object);
            var actual = await courseCtrl.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(actual.Result);
            var course = Assert.IsType<CourseViewModel>(okResult.Value);
            Assert.Equal("English", course.Name);
        }
        
        [Fact]
        public async Task ReturnCreatedAtRouteResponseWhenCourseCreated()
        {
            var newCourseviewModelForCreate = new CourseForCreateViewModel
            {
                Name = "Film Study",
                TeacherName = "Stanley Kubrick"
            };
            var newCourse = new Course
            {
                Id = 4,
                Name = "Film Study",
                TeacherName = "Stanley Kubrick"
            };
            _mapper.Setup(m => m.Map<Course>(It.IsAny<CourseForCreateViewModel>()))
                .Returns(newCourse);

            var courseCtrl = new CourseController(_mapper.Object, _courseService.Object);
            var actual = await courseCtrl.Post(newCourseviewModelForCreate);

            var okResult = Assert.IsType<CreatedAtRouteResult>(actual);
            var course = Assert.IsType<CourseForCreateViewModel>(okResult.Value);
            Assert.Equal("Film Study", course.Name);
        }
        
        [Fact]
        public async Task ReturnNoContentResponseWhenCourseUpdated()
        {
            var newCourseviewModelForCreate = new CourseForUpdateViewModel
            {
                TeacherName = "Jacques Marie Émile Lacan"
            };
            _mapper.Setup(m => m.Map<Course>(It.IsAny<CourseForCreateViewModel>()))
                .Returns(courses.Find(x => x.Id == 2));

            var courseCtrl = new CourseController(_mapper.Object, _courseService.Object);
            var actual = await courseCtrl.Put(2,newCourseviewModelForCreate);

            var okResult = Assert.IsType<NoContentResult>(actual);
        }

        [Fact]
        public async Task ReturnNoContentResponseWhenCourseDeleted()
        {
            var courseCtrl = new CourseController(_mapper.Object, _courseService.Object);
            var actual = await courseCtrl.Delete(1);

            var okResult = Assert.IsType<NoContentResult>(actual);
        }

        [Fact]
        public async Task ReturnBadRequestResponseIfRequestBodyIsNull()
        {
            var courseCtrl = new CourseController(_mapper.Object, _courseService.Object);
            var actual = await courseCtrl.Post(null);

            var notFoundResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.Equal(400, notFoundResult.StatusCode);
            Assert.Equal("Request body is null.", notFoundResult.Value);

            _courseService.Verify(x => x.CreateAsync(It.IsAny<Course>()), Times.Never);
        }
        [Fact]
        public async Task ReturnNotFoundResponseIfEntityDoesNotExistUpdateMethod()
        {
            _courseService.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);
            var courseCtrl = new CourseController(_mapper.Object, _courseService.Object);
            var actual = await courseCtrl.Put(1,new CourseForUpdateViewModel());

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actual);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("Entity does not exist.", notFoundResult.Value);

            _courseService.Verify(x => x.UpdateAsync(It.IsAny<Course>()), Times.Never);
        }
        [Fact]
        public async Task ReturnNotFoundResponseIfEntityDoesNotExistDeleteMethod()
        {
            _courseService.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);
            var courseCtrl = new CourseController(_mapper.Object, _courseService.Object);
            var actual = await courseCtrl.Delete(1);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actual);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("Entity does not exist.", notFoundResult.Value);

            _courseService.Verify(x => x.DeleteAsync(It.IsAny<Course>()), Times.Never);
        }
    }
}