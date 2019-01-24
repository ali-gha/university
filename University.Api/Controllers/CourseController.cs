using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using University.BusinessLayer.Models;
using University.ServiceLayer.Abstracts;
using University.ViewModels.CourseViewModels;

namespace University.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;

        public CourseController(IMapper mapper, ICourseService courseService)
        {
            _courseService = courseService;
            if (_courseService == null)
                throw new ArgumentNullException("CourseService");
            _mapper = mapper;
            if (_mapper == null)
                throw new ArgumentNullException("AutoMapper");

        }
        [HttpGet]
        public async Task<ActionResult<List<Course>>> Get()
        {
            var courses = await _courseService.GetAllAsync();
            var result = _mapper.Map<List<CourseViewModel>>(courses);
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetCourseById")]
        public async Task<ActionResult<Course>> Get(int id)
        {
            var course = await _courseService.GetByIdAsyncAsNotTracked(id);
            if (course == null)
                return NotFound("Entity does not exist.");
            var result = _mapper.Map<CourseViewModel>(course);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CourseForCreateViewModel courseViewModelForCreate)
        {
            if (courseViewModelForCreate == null)
                return BadRequest("Request body is null.");
            var course = new Course();
            _mapper.Map(courseViewModelForCreate, course);
            if (await _courseService.CreateAsync(course) > 0)
                return CreatedAtRoute("GetCourseById", new { Id = course.Id }, courseViewModelForCreate);

            return StatusCode(500);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CourseForUpdateViewModel courseViewModelForUpdate)
        {
            if (courseViewModelForUpdate == null)
                return BadRequest("Request body is null.");
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
                return NotFound("Entity does not exist.");
            _mapper.Map(courseViewModelForUpdate, course);
            foreach (var item in course.CourseStudents)
            {
                item.GPA = courseViewModelForUpdate.Students.FirstOrDefault(x => x.StudentId == item.StudentId).GPA;
            }
            if (await _courseService.UpdateAsync(course) > 0)
            {
                return NoContent();
            }
            return StatusCode(500);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
                return NotFound("Entity does not exist.");
            if (await _courseService.DeleteAsync(course) > 0)
            {
                return NoContent();
            }

            return StatusCode(500);
        }
    }
}
