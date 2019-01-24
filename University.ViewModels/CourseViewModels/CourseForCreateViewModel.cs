using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using University.ViewModels.CourseStudentViewModels;

namespace University.ViewModels.CourseViewModels
{
    public class CourseForCreateViewModel
    {
        public CourseForCreateViewModel()
        {
            Students = new List<CourseStudentForCreateViewModel>();
        }
        [Required]
        public string Name { get; set; }
        [Required]
        public string TeacherName { get; set; }
        [Required]
        public string Location { get; set; }

        public ICollection<CourseStudentForCreateViewModel> Students { get; set; }
    }
}