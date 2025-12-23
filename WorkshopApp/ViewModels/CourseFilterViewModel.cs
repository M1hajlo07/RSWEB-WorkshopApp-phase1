using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using WorkshopApp.Models;

namespace WorkshopApp.ViewModels
{
    public class CourseFilterViewModel
    {
        public IList<Course> Courses { get; set; } = new List<Course>();

        public int CourseSemester { get; set; }

        public SelectList SemesterList { get; set; }

        public string CourseProgram { get; set; }

        public SelectList ProgramList { get; set; }

        public string SearchTitle { get; set; }

        public Teacher Teacher { get; set; }
    }
}
