using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkshopApp.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Display(Name = "Наслов")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Display(Name = "Кредити")]
        public int Credits { get; set; }

        [Display(Name = "Семестар")]
        public int Semester { get; set; }

        [Display(Name = "Програма")]
        [MaxLength(100)]
        public string Programme { get; set; }

        [Display(Name = "Ниво")]
        [MaxLength(25)]
        public string EducationLevel { get; set; }

        [Display(Name = "Студенти")]
        public ICollection<Enrollment> Students { get; set; } = new List<Enrollment>();

       
        [Display(Name = "Професор 1")]
        public int FirstTeacherId { get; set; }

        [ForeignKey(nameof(FirstTeacherId))]
        public Teacher FirstTeacher { get; set; }


        [Display(Name = "Професор 2")]
        public int SecondTeacherId { get; set; }

        [ForeignKey(nameof(SecondTeacherId))]
        public Teacher SecondTeacher { get; set; }

    
        [NotMapped]
        [Display(Name = "Професор 1")]
        public string FirstTeacherName => FirstTeacher?.FullName;

        [NotMapped]
        [Display(Name = "Професор 2")]
        public string SecondTeacherName => SecondTeacher?.FullName;
    }
}
