using System.Collections.Generic;
using University.ViewModels.StudentViewModels;

namespace University.ViewModels.CourseViewModels
{
    public class CourseViewModel
    {
        public CourseViewModel()
        {
            Students = new List<StudentViewModel>();

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string TeacherName { get; set; }
        public string Location { get; set; }
        public List<StudentViewModel> Students { get; set; }
    }
}