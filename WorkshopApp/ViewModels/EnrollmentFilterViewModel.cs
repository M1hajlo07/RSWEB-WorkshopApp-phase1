using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using WorkshopApp.Models;

namespace WorkshopApp.ViewModels
{
    public class EnrollmentFilterViewModel
    {
        public IList<Course> Courses { get; set; } = new List<Course>();

        public IList<Student> Students { get; set; } = new List<Student>();

        public IList<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public int SearchSemester { get; set; }

        public SelectList Semesters { get; set; }

        public int SearchYear { get; set; }

        public SelectList Years { get; set; }

        public string SearchIndex { get; set; }

        public string SearchTitle { get; set; }
    }
}
