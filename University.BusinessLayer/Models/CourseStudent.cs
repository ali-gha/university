namespace University.BusinessLayer.Models
{
    public class CourseStudent
    {
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        public double GPA { get; set; }
    }
}