using System.Collections.Generic;

namespace University.BusinessLayer.Models
{
    public class Course
    {
        public Course()
        {
            CourseStudents = new List<CourseStudent>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string TeacherName { get; set; }
        public string Location { get; set; }
        public virtual ICollection<CourseStudent> CourseStudents { get; set; }
    }
}