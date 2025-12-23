using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using WorkshopApp.Models;

namespace WorkshopApp.ViewModels
{
    public class CourseStudentsViewModel
    {
        public Course Course { get; set; }

        public IEnumerable<int> SelectedStudents { get; set; } = new List<int>();

        public IEnumerable<SelectListItem> StudentList { get; set; } = new List<SelectListItem>();
    }
}
