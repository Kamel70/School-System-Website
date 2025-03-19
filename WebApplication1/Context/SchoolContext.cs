using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Context
{
    public class SchoolContext:IdentityDbContext<ApplicationUser>
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Courses_Studs> Courses_Studs { get; set; }
        public SchoolContext():base(){}
        public SchoolContext(DbContextOptions options) : base(options) { }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=KAMEL-LAPTOP\\SQLEXPRESS; Database= School; Trusted_Connection=true; TrustServerCertificate=true");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Instructor>()
                .HasOne(i => i.Department)
                .WithMany(d => d.Instructors)
                .HasForeignKey(i => i.DeptID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Students)
                .HasForeignKey(s => s.DepartmentID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Courses_Studs>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.Courses_Studs)
                .HasForeignKey(sc => sc.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
