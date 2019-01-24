using System.ComponentModel.DataAnnotations;

namespace University.ViewModels.StudentViewModels
{
    public class StudentForUpdateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        public double GPA { get; set; }
    }
}