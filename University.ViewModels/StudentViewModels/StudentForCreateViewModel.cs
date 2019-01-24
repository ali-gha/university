using System.ComponentModel.DataAnnotations;

namespace University.ViewModels.StudentViewModels
{
    public class StudentForCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int? Age { get; set; }
    }
}