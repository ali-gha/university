using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using University.BusinessLayer.Models;
using University.ViewModels.CourseStudentViewModels;
using University.ViewModels.CourseViewModels;
using University.ViewModels.StudentViewModels;

namespace University.Api.Mappings
{
    public class AutoMapperMapping : Profile
    {
        public AutoMapperMapping()
        {
            CreateMap<Course, CourseViewModel>()
            .ForMember(dest => dest.Students, opt => opt.MapFrom(src => GetRegisteredStudent(src)));
            CreateMap<CourseForCreateViewModel, Course>()
            .ForMember(dest => dest.CourseStudents, opt =>
                opt.ResolveUsing(FillCourseStudentForCreate));
            CreateMap<CourseForUpdateViewModel, Course>()
            .ForMember(dest => dest.CourseStudents, opt =>
                opt.ResolveUsing(FillCourseStudentForUpdate));
            CreateMap<StudentForCreateViewModel, Student>();
            CreateMap<StudentForUpdateViewModel, Student>();
            CreateMap<CourseStudentForCreateViewModel, Student>();
            CreateMap<CourseStudentForUpdateViewModel, Student>();

            CreateMap<Student, StudentViewModel>()
            .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Name));
        }

        public List<CourseStudent> TryUpdateManyToMany<TKey>(List<CourseStudent> source, IEnumerable<CourseStudent> currentItems, IEnumerable<CourseStudent> newItems, Func<CourseStudent, TKey> getKey) 
        {
            source = source.Except(Except(currentItems, newItems, getKey)).ToList();
            source.AddRange(Except(newItems, currentItems, getKey));
            return source;
        }

        private IEnumerable<CourseStudent> Except<TKey>(IEnumerable<CourseStudent> items, IEnumerable<CourseStudent> other, Func<CourseStudent, TKey> getKeyFunc)
        {
            return items
                .GroupJoin(other, getKeyFunc, getKeyFunc, (item, tempItems) => new { item, tempItems })
                .SelectMany(t => t.tempItems.DefaultIfEmpty(), (t, temp) => new { t, temp })
                .Where(t => ReferenceEquals(null, t.temp) || t.temp.Equals(default(CourseStudent)))
                .Select(t => t.t.item);
        }
        
        private List<CourseStudent> FillCourseStudentForUpdate(CourseForUpdateViewModel coVM, Course course)
        {
            var newItems = coVM.Students.GroupBy(x => x.StudentId).Select(x => x.First()).Select(x => new CourseStudent(){
                CourseId = course.Id,
                StudentId = x.StudentId.Value,
            }).ToList();
            var currentItems = course.CourseStudents;
            return TryUpdateManyToMany(course.CourseStudents.ToList(), currentItems, newItems, x => x.StudentId);
        }

        private List<CourseStudent> FillCourseStudentForCreate(CourseForCreateViewModel coVM, Course course)
        {
            var courseStudents = new List<CourseStudent>();
            foreach (var item in coVM.Students.GroupBy(x => x.StudentId).Select(x => x.First()).ToList())
            {
                if (item.StudentId != null)
                    courseStudents.Add(new CourseStudent()
                    {
                        Course = course,
                        StudentId = item.StudentId.Value
                    });
            }
            return courseStudents;
        }

        private List<StudentViewModel> GetRegisteredStudent(Course src)
        {
            return src.CourseStudents.Select(x => new StudentViewModel(){
                Age = x.Student.Age,
                GPA = x.GPA,
                StudentId = x.StudentId,
                StudentName = x.Student.Name
            }).ToList();
        }
    }
}