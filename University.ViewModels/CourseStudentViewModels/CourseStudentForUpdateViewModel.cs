using System.ComponentModel.DataAnnotations;

namespace University.ViewModels.CourseStudentViewModels
{
    public class CourseStudentForUpdateViewModel
    {
        [Required]
        public int? StudentId { get; set; }
        public double GPA { get; set; }
    }
}