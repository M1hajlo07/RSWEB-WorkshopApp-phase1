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
    public class EnrollmentsController : Controller
    {
        private readonly WorkshopAppContext _context;
        private readonly IWebHostEnvironment webHostingEnvironment;

        public EnrollmentsController(WorkshopAppContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            webHostingEnvironment = hostingEnvironment;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index(string SearchIndex, string SearchTitle, string SearchSemester, int? SearchYear)
        {
            IQueryable<Enrollment> enrollments = _context.Enrollment;
            if (!string.IsNullOrEmpty(SearchIndex))
            {
                enrollments = enrollments.Where(s => s.Student.StudentId.Contains(SearchIndex));
            }

            if (!string.IsNullOrEmpty(SearchTitle))
            {
                enrollments = enrollments.Where(c => c.Course.Title.Contains(SearchTitle));
            }

            IQueryable<string> semesters = enrollments.OrderBy(m => m.Semester).Select(m => m.Semester).Distinct();
            IQueryable<int?> years = enrollments.OrderBy(m => m.Year).Select(m => m.Year).Distinct();

            if (SearchYear != null)
            {
                enrollments = enrollments.Where(e => e.Year == SearchYear);
            }

            if (!string.IsNullOrEmpty(SearchSemester))
            {
                enrollments = enrollments.Where(e => e.Semester == SearchSemester);
            }

            enrollments = enrollments.Include(e => e.Student).Include(e => e.Course);

            var viewmodel = new EnrollmentFilterViewModel
            {
                Enrollments = await enrollments.ToListAsync(),
                Years = new SelectList(await years.ToListAsync()),
                Semesters = new SelectList(await semesters.ToListAsync())
            };


            return View(viewmodel);
        }


        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollments/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id");
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Id");
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,CourseId,Semester,Year,Grade,SeminalUrl,ProjectUrl,ExamPoints,SeminalPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Id", enrollment.StudentId);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enrollment == null)
            {
                return NotFound();
            }

            
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);

            return View(enrollment);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,CourseId,Semester,Year,Grade,SeminalUrl,ProjectUrl,ExamPoints,SeminalPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (id != enrollment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Id", enrollment.StudentId);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollment.FindAsync(id);
            _context.Enrollment.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        //GET: Enrollents/CreateEnrollment

        /* public IActionResult CreateEnrollment()
         {
             ViewData["courseID"] = new SelectList(_context.Course, "Id", "Title");
             ViewData["studentID"] = new SelectList(_context.Student, "Id", "FullName");
             return View();
         }

         //POST: Enrollments/CreateEnrollment

         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> CreateEnrollment([Bind("Id,CourseId,StudentId,Semester,Year")] Enrollment enrollment)
         {
             if (ModelState.IsValid)
             {
                 _context.Add(enrollment);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
             }
             ViewData["courseID"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
             ViewData["studentID"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
             return View(enrollment);
         }*/

        // GET: Enrollments/StudentEdit/5

        public async Task<IActionResult> StudentEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["courseID"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["studentID"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }

        // POST: Enrollments/StudentEdit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentEdit(
    int id,
    [Bind("Id,CourseId,StudentId,Semester,Year,Grade,SeminalUrl,ProjectUrl,ExamPoints,SeminalPoints,ProjectPoints,AdditionalPoints,FinishDate,ProjectFile,SeminalFile")]
    Enrollment enrollment)
        {
            if (id != enrollment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               
                if (enrollment.ProjectFile != null)
                {
                    enrollment.ProjectUrl = UploadFile(enrollment.ProjectFile);
                }

                
                if (enrollment.SeminalFile != null)
                {
                    enrollment.SeminalUrl = UploadFile(enrollment.SeminalFile);
                }

                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction("Index", "Students");
            }

            ViewData["courseID"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["studentID"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }


        private string UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                throw new Exception("Invalid file type");

            string uploadsFolder = Path.Combine(webHostingEnvironment.WebRootPath, "projects");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + extension;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return uniqueFileName; 
        }

        public async Task<IActionResult> TeacherEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["courseID"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["studentID"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }

        // POST: Enrollments/TeacherEdit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TeacherEdit(int id, [Bind("Id,CourseId,StudentId,Semester,Year,Grade,SeminalUrl,ProjectUrl,ExamPoints,SeminalPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (id != enrollment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Teachers");
            }
            ViewData["courseID"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["studentID"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }


        private bool EnrollmentExists(int id)
        {
            return _context.Enrollment.Any(e => e.Id == id);
        }

        public async Task<IActionResult> StudentEnrollment()
        {
            IQueryable<Course> courses = _context.Course;
            IEnumerable<Student> students = _context.Student;

            EnrollStudent viewmodel = new EnrollStudent
            {
                Courses = new SelectList(await courses.ToListAsync(), "Id", "Title"),
                StudentsList = new SelectList(students.OrderBy(s => s.FullName).ToList(), "Id", "FullName"),

            };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentEnrollment(EnrollStudent enrollstudent)
        {
            var course = await _context.Course.FirstOrDefaultAsync(c => c.Id == enrollstudent.CourseId);
            if (course == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                foreach (int sS in enrollstudent.SelectedStudents)
                {
                    Enrollment enroll = await _context.Enrollment
                        .FirstOrDefaultAsync(c => c.CourseId == enrollstudent.CourseId && c.StudentId == sS);
                    if (enroll == null)
                    {

                        _context.Add(new Enrollment { StudentId = sS, CourseId = enrollstudent.CourseId, Year = enrollstudent.Enrollments.Year, Semester = enrollstudent.Enrollments.Semester });
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { Title = course.Title });
            }
            return RedirectToAction(nameof(Index));
        }

        // GET
        // GET: StudentUnEnrollment
        public async Task<IActionResult> StudentUnEnrollment(int? courseId, int? year, string semester)
        {
            var courses = await _context.Course.ToListAsync();

          
            IQueryable<Enrollment> enrollments = _context.Enrollment.Include(e => e.Student);

          
            if (courseId != null)
                enrollments = enrollments.Where(e => e.CourseId == courseId);

            if (year != null)
                enrollments = enrollments.Where(e => e.Year == year);

            if (!string.IsNullOrEmpty(semester))
                enrollments = enrollments.Where(e => e.Semester == semester);

            
            var model = new UnEnrollStudents
            {
                CourseId = courseId,
                Year = year,
                Semester = semester,
                CoursesList = new SelectList(courses, "Id", "Title", courseId),
                Years = new SelectList(await _context.Enrollment.Select(e => e.Year).Distinct().ToListAsync(), year),
                Semesters = new SelectList(await _context.Enrollment.Select(e => e.Semester).Distinct().ToListAsync(), semester),
                Enrollments = (courseId != null)
                    ? new SelectList(await enrollments.OrderBy(e => e.Student.StudentId).ToListAsync(), "Id", "Student.FullName")
                    : new SelectList(new List<Enrollment>(), "Id", "Student.FullName") 
            };

            return View(model);
        }


        // POST: delete selected students and redirect to Enroll page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentUnEnrollment(UnEnrollStudents entry)
        {
            if (ModelState.IsValid && entry.SelectedEnrollments != null && entry.SelectedEnrollments.Any())
            {
                foreach (var eId in entry.SelectedEnrollments)
                {
                    var enrollment = await _context.Enrollment.FindAsync(eId);
                    if (enrollment != null)
                    {
                        _context.Enrollment.Remove(enrollment);
                    }
                }
                await _context.SaveChangesAsync();
            }

            
            return RedirectToAction("Index", "Enrollments");
        }






    }
}
