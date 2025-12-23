using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace WorkshopApp.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

       
        [Display(Name = "Студент")]
        public int StudentId { get; set; }

        [ForeignKey(nameof(StudentId))]
        [Display(Name = "Студент")]
        public Student Student { get; set; }

       
        [Display(Name = "Предмет")]
        public int CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        [Display(Name = "Предмет")]
        public Course Course { get; set; }

       
        [Display(Name = "Семестар")]
        [MaxLength(10)]
        public string Semester { get; set; }

        [Display(Name = "Година")]
        public int? Year { get; set; }

        [Display(Name = "Оценка")]
        public int Grade { get; set; }

       
        [Display(Name = "Семинарска")]
        [MaxLength(255)]
        public string SeminalUrl { get; set; }

        [NotMapped]
        [Display(Name = "Семинарска (фајл)")]
        public IFormFile SeminalFile { get; set; }

      
        [Display(Name = "Проект")]
        [MaxLength(255)]
        public string ProjectUrl { get; set; }

        [NotMapped]
        [Display(Name = "Проект (фајл)")]
        public IFormFile ProjectFile { get; set; }

        [Display(Name = "Поени од испит")]
        public int ExamPoints { get; set; }

        [Display(Name = "Поени од семинарска")]
        public int SeminalPoints { get; set; }

        [Display(Name = "Поени од проект")]
        public int ProjectPoints { get; set; }

        [Display(Name = "Дополнителни поени")]
        public int AdditionalPoints { get; set; }

     
        [Display(Name = "Датум на положување")]
        [DataType(DataType.Date)]
        public DateTime FinishDate { get; set; }
    }
}
