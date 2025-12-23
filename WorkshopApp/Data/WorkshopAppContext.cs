using Microsoft.EntityFrameworkCore;
using WorkshopApp.Models;

namespace WorkshopApp.Data
{
    public class WorkshopAppContext : DbContext
    {
        public WorkshopAppContext(DbContextOptions<WorkshopAppContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Student { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<Course> Course { get; set; }

        
        public DbSet<Course> FirstCourses { get; set; }
        public DbSet<Course> SecondCourses { get; set; }

        public DbSet<Enrollment> Enrollment { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            
            builder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Courses)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

           
            builder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Students)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

          
            builder.Entity<Course>()
                .HasOne(c => c.FirstTeacher)
                .WithMany(t => t.FirstCourses)
                .HasForeignKey(c => c.FirstTeacherId)
                .OnDelete(DeleteBehavior.Restrict);

           
            builder.Entity<Course>()
                .HasOne(c => c.SecondTeacher)
                .WithMany(t => t.SecondCourses)
                .HasForeignKey(c => c.SecondTeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
