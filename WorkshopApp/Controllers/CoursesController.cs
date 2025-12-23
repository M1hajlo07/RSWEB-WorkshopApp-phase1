using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkshopApp.Data;
using WorkshopApp.Models;
using WorkshopApp.ViewModels;

namespace WorkshopApp.Controllers
{
    public class CoursesController : Controller
    {
        private readonly WorkshopAppContext _context;

        public CoursesController(WorkshopAppContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index(string SearchTitle, string CourseSemester, string CourseProgram)
        {
            IQueryable<Course> courses = _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher)
                .Include(c => c.Students).ThenInclude(e => e.Student)
                .AsQueryable();

            IQueryable<int> semesterQuery = _context.Course.OrderBy(c => c.Semester).Select(c => c.Semester).Distinct();
            IQueryable<string> programQuery = _context.Course.OrderBy(c => c.Programme).Select(c => c.Programme).Distinct();

            if (!string.IsNullOrEmpty(SearchTitle))
                courses = courses.Where(c => c.Title.Contains(SearchTitle));

            if (!string.IsNullOrEmpty(CourseProgram))
                courses = courses.Where(c => c.Programme == CourseProgram);

            if (!string.IsNullOrEmpty(CourseSemester))
                courses = courses.Where(c => c.Semester.ToString() == CourseSemester);

            var viewmodel = new CourseFilterViewModel
            {
                Courses = await courses.ToListAsync(),
                ProgramList = new SelectList(await programQuery.ToListAsync()),
                SemesterList = new SelectList(await semesterQuery.ToListAsync())
            };

            return View(viewmodel);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher)
                .Include(c => c.Students).ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) return NotFound();

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName");
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName");
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Credits,Semester,Programme,EducationLevel,FirstTeacherId,SecondTeacherId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.SecondTeacherId);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var course = await _context.Course
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) return NotFound();

            var students = await _context.Student.ToListAsync();

            var viewmodel = new CourseStudentsViewModel
            {
                Course = course,
                StudentList = new MultiSelectList(students, "Id", "FullName"),
                SelectedStudents = course.Students.Select(e => e.StudentId)
            };

            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.SecondTeacherId);

            return View(viewmodel);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseStudentsViewModel viewmodel)
        {
            if (id != viewmodel.Course.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewmodel.Course);
                    await _context.SaveChangesAsync();

                    
                    var listStudents = viewmodel.SelectedStudents ?? new List<int>();

                    
                    var toBeRemoved = _context.Enrollment
                        .Where(e => e.CourseId == id && !listStudents.Contains(e.StudentId));
                    _context.Enrollment.RemoveRange(toBeRemoved);

                    
                    var existingStudentIds = _context.Enrollment
                        .Where(e => e.CourseId == id)
                        .Select(e => e.StudentId)
                        .ToList();

                    var newStudents = listStudents.Except(existingStudentIds);
                    foreach (var studentId in newStudents)
                    {
                        _context.Enrollment.Add(new Enrollment { CourseId = id, StudentId = studentId });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(viewmodel.Course.Id)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", viewmodel.Course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", viewmodel.Course.SecondTeacherId);

            return View(viewmodel);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) return NotFound();

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Courses/EnrollmentsCourse/5
        public async Task<IActionResult> EnrollmentsCourse(int? id)
        {
            if (id == null) return NotFound();

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher)
                .Include(c => c.Students).ThenInclude(e => e.Student)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) return NotFound();

            return View(course);
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(c => c.Id == id);
        }
    }
}
