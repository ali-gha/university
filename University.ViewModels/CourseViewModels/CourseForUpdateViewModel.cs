using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using University.ViewModels.CourseStudentViewModels;

namespace University.ViewModels.CourseViewModels
{
    public class CourseForUpdateViewModel
    {
        public CourseForUpdateViewModel()
        {
            Students = new List<CourseStudentForUpdateViewModel>();
        }
        [Required]
        public string Name { get; set; }
        [Required]
        public string TeacherName { get; set; }
        [Required]
        public string Location { get; set; }

        public ICollection<CourseStudentForUpdateViewModel> Students { get; set; }
    }
}