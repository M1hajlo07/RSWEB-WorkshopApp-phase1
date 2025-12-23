using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WorkshopApp.Models;

namespace WorkshopApp.ViewModels
{
    public class UnEnrollStudents
    {
        
        public int? CourseId { get; set; }
        public int? Year { get; set; }
        public string Semester { get; set; }

        public SelectList CoursesList { get; set; } = new SelectList(new List<Course>(), "Id", "Title");
        public SelectList Years { get; set; } = new SelectList(new List<int>());
        public SelectList Semesters { get; set; } = new SelectList(new List<string>());

        
        [Display(Name = "Студенти")]
        public IList<int> SelectedEnrollments { get; set; } = new List<int>();

        public SelectList Enrollments { get; set; } = new SelectList(new List<Enrollment>(), "Id", "Id");

        
        [Display(Name = "Дата на завршување")]
        [DataType(DataType.Date)]
        public DateTime? FinishDate { get; set; }
    }
}
