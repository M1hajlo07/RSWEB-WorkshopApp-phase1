using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WorkshopApp.Models;

namespace WorkshopApp.ViewModels
{
    public class EnrollStudent
    {
        public Enrollment Enrollments { get; set; }

        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        [Display(Name = "Студенти")]
        public IEnumerable<int> SelectedStudents { get; set; } = new List<int>();

        public int Year { get; set; }

        public string Semester { get; set; }

        public SelectList Courses { get; set; } = new SelectList(new List<Course>(), "Id", "Title");

        public SelectList StudentsList { get; set; } = new SelectList(new List<Student>(), "Id", "FullNamePlusIndex");
    }
}
