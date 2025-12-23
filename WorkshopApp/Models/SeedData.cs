using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkshopApp.Data;

namespace WorkshopApp.Models
{
    public class SeedData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new WorkshopAppContext(
            serviceProvider.GetRequiredService<
            DbContextOptions<WorkshopAppContext>>()))
            {
                
                if (context.Teacher.Any() || context.Student.Any() || context.Course.Any())
                {
                    return; 
                }
                context.Student.AddRange(
            new Student { StudentId = "101/2022", FirstName = "Михајло", LastName = "Наумоски", EnrollmentDate = DateTime.Parse("2022-09-12"), AcquiredCredits = 180, CurrentSemestar = 7, EducationLevel = "Додипломски" },
            new Student { StudentId = "100/2022", FirstName = "Давид", LastName = "Наумовски", EnrollmentDate = DateTime.Parse("2022-09-12"), AcquiredCredits = 180, CurrentSemestar = 7, EducationLevel = "Додипломски" },
            new Student { StudentId = "103/2022", FirstName = "Томи", LastName = "Николоски", EnrollmentDate = DateTime.Parse("2022-09-12"), AcquiredCredits = 126, CurrentSemestar = 7, EducationLevel = "Додипломски" },
            new Student { StudentId = "1/2022", FirstName = "Викторија", LastName = "Пројкова", EnrollmentDate = DateTime.Parse("2022-09-12"), AcquiredCredits = 144, CurrentSemestar = 7, EducationLevel = "Додипломски" },
            new Student { StudentId = "94/2020", FirstName = "Сара", LastName = "Мисајлеска", EnrollmentDate = DateTime.Parse("2022-09-12"), AcquiredCredits = 132, CurrentSemestar = 7, EducationLevel = "Додипломски" },
            new Student { StudentId = "98/2022", FirstName = "Ангела", LastName = "Настовска", EnrollmentDate = DateTime.Parse("2022-09-12"), AcquiredCredits = 174, CurrentSemestar = 7, EducationLevel = "Додипломски" }



                );
                context.SaveChanges();
                context.Teacher.AddRange(

             new Teacher { FirstName = "Перо", LastName = "Латкоски", Degree = "Доктор на науки", AcademicRank = "Редовен професор", OfficeNumber = "ТК", HireDate = DateTime.Parse("2006-01-01") },
             new Teacher { FirstName = "Валентин", LastName = "Раковиќ", Degree = "Доктор на науки", AcademicRank = "Вонреден професор", OfficeNumber = "ТК", HireDate = DateTime.Parse("2017-01-01") },
             new Teacher { FirstName = "Владимир", LastName = "Атанасовски", Degree = "Доктор на науки", AcademicRank = "Доцент", OfficeNumber = "ТК", HireDate = DateTime.Parse("2015-01-01") },
             new Teacher { FirstName = "Горан", LastName = "Јакимовски", Degree = "Доктор на науки", AcademicRank = "Вонреден професор", OfficeNumber = "ТК", HireDate = DateTime.Parse("2019-01-01") },
             new Teacher { FirstName = "Даниел", LastName = "Денковски", Degree = "Доктор на науки", AcademicRank = "Вонреден професор", OfficeNumber = "121b", HireDate = DateTime.Parse("2017-01-01") }

                );
                context.SaveChanges();
                context.Course.AddRange(
                new Course
                {
                    Title = "Развој на серверски WEB апликации",
                    Credits = 6,
                    Semester = 7,
                    Programme = "КТИ,ТКИИ",
                    EducationLevel = "Додипломски",
                    FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Даниел" && d.LastName == "Денковски").Id,
                    SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Перо" && d.LastName == "Латкоски").Id
                },
            new Course
            {
                Title = "Основи на WEB програмирање",
                Credits = 6,
                Semester = 5,
                Programme = "КТИ,ТКИИ",
                EducationLevel = "Додипломски",
                FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Владимир" && d.LastName == "Атанасовски").Id,
                SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Валентин" && d.LastName == "Раковиќ").Id
            },
            new Course
            {
                Title = "Виртуелизација и контејнер системи",
                Credits = 6,
                Semester = 7,
                Programme = "КТИ",
                EducationLevel = "Додипломски",
                FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Даниел" && d.LastName == "Денковски").Id,
                SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Валентин" && d.LastName == "Раковиќ").Id
            }

                );
                context.SaveChanges();
                context.Enrollment.AddRange(
                    new Enrollment { StudentId = 1, CourseId = 1, Semester = "7", Year = 2025, Grade = 10, SeminalUrl = "", ProjectUrl = "", ExamPoints = 100, SeminalPoints = 0, ProjectPoints = 100, AdditionalPoints = 0, FinishDate = DateTime.Parse("2025-06-05") },
                    new Enrollment { StudentId = 1, CourseId = 2, Semester = "5", Year = 2024, Grade = 8, SeminalUrl = "", ProjectUrl = "", ExamPoints = 78, SeminalPoints = 0, ProjectPoints = 100, AdditionalPoints = 0, FinishDate = DateTime.Parse("2024-06-05") },
                    new Enrollment { StudentId = 1, CourseId = 3, Semester = "7", Year = 2025, Grade = 10, SeminalUrl = "", ProjectUrl = "", ExamPoints = 100, SeminalPoints = 90, ProjectPoints = 100, AdditionalPoints = 0, FinishDate = DateTime.Parse("2025-06-05") },
                    new Enrollment { StudentId = 2, CourseId = 1, Semester = "7", Year = 2025, Grade = 10, SeminalUrl = "", ProjectUrl = "", ExamPoints = 100, SeminalPoints = 0, ProjectPoints = 100, AdditionalPoints = 0, FinishDate = DateTime.Parse("2025-05-05") },
                    new Enrollment { StudentId = 2, CourseId = 2, Semester = "5", Year = 2024, Grade = 6, SeminalUrl = "", ProjectUrl = "", ExamPoints = 53, SeminalPoints = 0, ProjectPoints = 0, AdditionalPoints = 0, FinishDate = DateTime.Parse("2024-06-05") },
                    new Enrollment { StudentId = 3, CourseId = 3, Semester = "7", Year = 2025, Grade = 10, SeminalUrl = "", ProjectUrl = "", ExamPoints = 100, SeminalPoints = 64, ProjectPoints = 100, AdditionalPoints = 0, FinishDate = DateTime.Parse("2025-10-05") },
                    new Enrollment { StudentId = 3, CourseId = 1, Semester = "7", Year = 2025, Grade = 9, SeminalUrl = "", ProjectUrl = "", ExamPoints = 70, SeminalPoints = 100, ProjectPoints = 100, AdditionalPoints = 0, FinishDate = DateTime.Parse("2025-06-05") },
                    new Enrollment { StudentId = 3, CourseId = 2, Semester = "5", Year = 2024, Grade = 10, SeminalUrl = "", ProjectUrl = "", ExamPoints = 100, SeminalPoints = 0, ProjectPoints = 100, AdditionalPoints = 5, FinishDate = DateTime.Parse("2024-06-05") },
                    new Enrollment { StudentId = 4, CourseId = 1, Semester = "7", Year = 2025, Grade = 10, SeminalUrl = "", ProjectUrl = "", ExamPoints = 100, SeminalPoints = 0, ProjectPoints = 100, AdditionalPoints = 0, FinishDate = DateTime.Parse("2025-06-05") },
                    new Enrollment { StudentId = 5, CourseId = 2, Semester = "5", Year = 2024, Grade = 10, SeminalUrl = "", ProjectUrl = "", ExamPoints = 100, SeminalPoints = 0, ProjectPoints = 50, AdditionalPoints = 0, FinishDate = DateTime.Parse("2024-06-05") },
                    new Enrollment { StudentId = 6, CourseId = 1, Semester = "7", Year = 2025, Grade = 7, SeminalUrl = "", ProjectUrl = "", ExamPoints = 68, SeminalPoints = 0, ProjectPoints = 100, AdditionalPoints = 0, FinishDate = DateTime.Parse("2025-06-05") },
                    new Enrollment { StudentId = 6, CourseId = 2, Semester = "5", Year = 2024, Grade = 10, SeminalUrl = "", ProjectUrl = "", ExamPoints = 100, SeminalPoints = 0, ProjectPoints = 100, AdditionalPoints = 0, FinishDate = DateTime.Parse("2024-06-05") }
);
                context.SaveChanges();


            }
        }
    }
}

    

