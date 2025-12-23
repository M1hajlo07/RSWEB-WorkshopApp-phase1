using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkshopApp.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Display(Name = "Име")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Презиме")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Display(Name = "Степен")]
        [MaxLength(50)]
        public string Degree { get; set; }

        [Display(Name = "Звање")]
        [MaxLength(25)]
        public string AcademicRank { get; set; }

        [Display(Name = "Канцеларија")]
        [MaxLength(10)]
        public string OfficeNumber { get; set; }

        [Display(Name = "Дата на вработување")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        [Display(Name = "Слика на профил")]
        public string profilePicture { get; set; }

        [Display(Name = "Име и Презиме")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        

        [InverseProperty("FirstTeacher")]
        [Display(Name = "Предмети")]
        public ICollection<Course> FirstCourses { get; set; } = new List<Course>();

        [InverseProperty("SecondTeacher")]
        public ICollection<Course> SecondCourses { get; set; } = new List<Course>();
    }
}
