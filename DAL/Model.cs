using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WpfApp1.DAL
{
    public partial class Model : DbContext
    {
        public Model()
            : base("name=Entity")
        {
        }

        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Point> Points { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<TimeTeacher> TimeTeachers { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>()
                .HasMany(e => e.Students)
                .WithRequired(e => e.Class)
                .HasForeignKey(e => e.IdClass);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.Points)
                .WithRequired(e => e.Item)
                .HasForeignKey(e => e.IdItem);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.Points)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.IdStudent);

            modelBuilder.Entity<Teacher>()
                .HasMany(e => e.Points)
                .WithRequired(e => e.Teacher)
                .HasForeignKey(e => e.IdTeacher);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Teachers)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.IdUser);

            modelBuilder.Entity<User>()
                .HasMany(e => e.TimeTeachers)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.IdUser);
        }
    }
}
