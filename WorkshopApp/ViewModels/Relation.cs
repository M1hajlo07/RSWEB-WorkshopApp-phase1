using System.Collections.Generic;
using WorkshopApp.Models;

namespace WorkshopApp.ViewModels
{
    public class Relation
    {
        public IEnumerable<Course> Courses { get; set; } = new List<Course>();

        public IEnumerable<Teacher> Teachers { get; set; } = new List<Teacher>();
    }
}
