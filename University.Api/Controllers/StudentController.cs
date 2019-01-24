using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using University.BusinessLayer.Models;
using University.ServiceLayer.Abstracts;
using University.ViewModels.StudentViewModels;

namespace University.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;

        public StudentController(IMapper mapper, IStudentService studentService)
        {
            _studentService = studentService;
            if (_studentService == null)
                throw new ArgumentNullException("StudentService");
            _mapper = mapper;
            if (_mapper == null)
                throw new ArgumentNullException("AutoMapper");

        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var students = await _studentService.GetAllAsync();
            var result = _mapper.Map<List<StudentViewModel>>(students);
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetStudentById")]
        public async Task<ActionResult> Get(int id)
        {
            var student = await _studentService.GetByIdAsyncAsNotTracked(id);
            if (student == null)
                return NotFound("Entity does not exist.");
            var result = _mapper.Map<StudentViewModel>(student);
            return Ok(result);
        }

        [HttpGet("checkStudentName/{name}")]
        public async Task<IActionResult> Get(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }
            if (await _studentService.FindStudentByName(name))
            {
                return Ok(true);
            }

            return Ok(false);
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] StudentForCreateViewModel studentViewModelForCreate)
        {
            if (studentViewModelForCreate == null)
                return BadRequest("Request body is null.");
            var student = new Student();
            student = _mapper.Map<Student>(studentViewModelForCreate);

            if (await _studentService.CreateAsync(student) > 0)
                return CreatedAtRoute("GetStudentById", new { Id = student.Id }, studentViewModelForCreate);

            return StatusCode(500);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] StudentForUpdateViewModel studentViewModelForUpdate)
        {
            if (studentViewModelForUpdate == null)
                return BadRequest("Request body is null.");
            var student = await _studentService.GetByIdAsync(id);
            if (student == null)
                return NotFound("Entity does not exist.");
            if (student.Name != studentViewModelForUpdate.Name)
                if (await _studentService.FindStudentByName(studentViewModelForUpdate.Name))
                    return BadRequest("The student name is already taken.");
            _mapper.Map(studentViewModelForUpdate, student);
            if (await _studentService.UpdateAsync(student) > 0)
            {
                return NoContent();
            }
            return StatusCode(500);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null)
                return NotFound("Entity does not exist.");
            if (await _studentService.DeleteAsync(student) > 0)
            {
                return NoContent();
            }

            return StatusCode(500);
        }
    }
}