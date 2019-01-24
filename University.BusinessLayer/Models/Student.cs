using System.Collections.Generic;

namespace University.BusinessLayer.Models
{
    public class Student
    {
        public Student()
        {
            CourseStudents = new List<CourseStudent>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public virtual ICollection<CourseStudent> CourseStudents { get; set; }
    }
}