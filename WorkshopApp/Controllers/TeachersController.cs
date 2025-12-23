using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkshopApp.Data;
using WorkshopApp.Models;
using WorkshopApp.ViewModels;

namespace WorkshopApp.Controllers
{
    public class TeachersController : Controller
    {
        private readonly WorkshopAppContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeachersController(WorkshopAppContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Teachers
        public async Task<IActionResult> Index(string SearchFullName, string SearchRank, string SearchDegree)
        {
           
            var rankQuery = await _context.Teacher
                .OrderBy(t => t.AcademicRank)
                .Select(t => t.AcademicRank)
                .Distinct()
                .ToListAsync();

            var degreeQuery = await _context.Teacher
                .OrderBy(t => t.Degree)
                .Select(t => t.Degree)
                .Distinct()
                .ToListAsync();

            
            var teachers = await _context.Teacher.ToListAsync();

            
            if (!string.IsNullOrEmpty(SearchFullName))
                teachers = teachers
                    .Where(t => t.FullName.Contains(SearchFullName, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            if (!string.IsNullOrEmpty(SearchRank))
                teachers = teachers
                    .Where(t => t.AcademicRank == SearchRank)
                    .ToList();

            if (!string.IsNullOrEmpty(SearchDegree))
                teachers = teachers
                    .Where(t => t.Degree.Contains(SearchDegree, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            
            var viewmodel = new TeacherFilterViewModel
            {
                Teachers = teachers,
                Ranks = new SelectList(rankQuery),
                Degrees = new SelectList(degreeQuery)
            };

            return View(viewmodel);
        }


        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _context.Teacher.FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null) return NotFound();

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _context.Teacher.FindAsync(id);
            if (teacher == null) return NotFound();

            return View(teacher);
        }

        // POST: Teachers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile ProfileImage, [Bind("Id,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate,profilePicture")] Teacher teacher)
        {
            if (id != teacher.Id) return NotFound();

            if (ProfileImage != null)
            {
                teacher.profilePicture = await UploadedFileAsync(ProfileImage);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction("Details", new { id = teacher.Id });
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _context.Teacher.FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null) return NotFound();

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           
            var teacher = await _context.Teacher
                .Include(t => t.FirstCourses)
                .Include(t => t.SecondCourses)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
                return NotFound();

            
            if ((teacher.FirstCourses != null && teacher.FirstCourses.Any()) ||
                (teacher.SecondCourses != null && teacher.SecondCourses.Any()))
            {
                
                TempData["ErrorMessage"] = "Не можете да го избришете професорот, бидејќи е назначен на курс.";
                return RedirectToAction(nameof(Index));
            }

            
            _context.Teacher.Remove(teacher);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        private async Task<string> UploadedFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var allowedContentTypes = new[] { "image/jpeg", "image/png" };

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new InvalidOperationException("Only JPG, JPEG, and PNG images are allowed.");

            if (!allowedContentTypes.Contains(file.ContentType))
                throw new InvalidOperationException("Invalid image format.");

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "teacherimages");

           
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }
        public async Task<IActionResult> Teachercourses(int? id, string SearchTitle, string CourseSemester, string CourseProgram)
        {
            IQueryable<Course> courses = _context.Course.AsQueryable();
            IQueryable<int> semesterQuery = _context.Course.OrderBy(c => c.Semester).Select(m => m.Semester).Distinct();
            IQueryable<string> programQuery = _context.Course.OrderBy(c => c.Programme).Select(g => g.Programme).Distinct();

            if (!string.IsNullOrEmpty(SearchTitle))
            {
                courses = courses.Where(t => t.Title.Contains(SearchTitle));
            }

            if (!string.IsNullOrEmpty(CourseProgram))
            {
                courses = courses.Where(s => s.Programme == CourseProgram);
            }

            if (!string.IsNullOrEmpty(CourseSemester))
            {
                courses = courses.Where(s => s.Semester.ToString().Equals(CourseSemester));
            }

            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .Include(c => c.FirstCourses)
                  .Include(d => d.SecondCourses)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }


            CourseFilterViewModel viewmodel = new CourseFilterViewModel
            {
                Teacher = teacher,
                Courses = await courses.ToListAsync(),
                ProgramList = new SelectList(await programQuery.ToListAsync()),
                SemesterList = new SelectList(await semesterQuery.ToListAsync())
            };
            
            return View(viewmodel);
        }


        private bool TeacherExists(int id)
        {
            return _context.Teacher.Any(e => e.Id == id);
        }
    }
}
