using Microsoft.EntityFrameworkCore;
using OuladEtlEda.Models;

namespace OuladEtlEda;

public class OuladContext : DbContext
{
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Assessment> Assessments => Set<Assessment>();
    public DbSet<StudentAssessment> StudentAssessments => Set<StudentAssessment>();
    public DbSet<StudentInfo> StudentInfos => Set<StudentInfo>();
    public DbSet<StudentRegistration> StudentRegistrations => Set<StudentRegistration>();
    public DbSet<Vle> Vles => Set<Vle>();
    public DbSet<StudentVle> StudentVles => Set<StudentVle>();

    public OuladContext(DbContextOptions<OuladContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Course>().HasKey(c => new { c.CodeModule, c.CodePresentation });
        modelBuilder.Entity<Assessment>().HasOne(a => a.Course)
            .WithMany(c => c.Assessments)
            .HasForeignKey(a => new { a.CodeModule, a.CodePresentation });

        modelBuilder.Entity<Vle>().HasOne(v => v.Course)
            .WithMany(c => c.Vles)
            .HasForeignKey(v => new { v.CodeModule, v.CodePresentation });

        modelBuilder.Entity<StudentInfo>().HasKey(s => new { s.CodeModule, s.CodePresentation, s.IdStudent });
        modelBuilder.Entity<StudentInfo>().HasOne(s => s.Course)
            .WithMany(c => c.Students)
            .HasForeignKey(s => new { s.CodeModule, s.CodePresentation });

        modelBuilder.Entity<StudentRegistration>().HasKey(r => new { r.CodeModule, r.CodePresentation, r.IdStudent });
        modelBuilder.Entity<StudentRegistration>().HasOne(r => r.Course)
            .WithMany(c => c.Registrations)
            .HasForeignKey(r => new { r.CodeModule, r.CodePresentation });
        modelBuilder.Entity<StudentRegistration>().HasOne(r => r.StudentInfo)
            .WithMany(s => s.Registrations)
            .HasForeignKey(r => new { r.CodeModule, r.CodePresentation, r.IdStudent });

        modelBuilder.Entity<StudentAssessment>().HasKey(sa => new { sa.IdAssessment, sa.IdStudent, sa.CodeModule, sa.CodePresentation });
        modelBuilder.Entity<StudentAssessment>().HasOne(sa => sa.Assessment)
            .WithMany(a => a.StudentAssessments)
            .HasForeignKey(sa => sa.IdAssessment);
        modelBuilder.Entity<StudentAssessment>().HasOne(sa => sa.StudentInfo)
            .WithMany(si => si.Assessments)
            .HasForeignKey(sa => new { sa.CodeModule, sa.CodePresentation, sa.IdStudent });

        modelBuilder.Entity<StudentVle>().HasKey(sv => new { sv.IdSite, sv.IdStudent, sv.CodeModule, sv.CodePresentation });
        modelBuilder.Entity<StudentVle>().HasOne(sv => sv.Vle)
            .WithMany(v => v.StudentVles)
            .HasForeignKey(sv => sv.IdSite);
        modelBuilder.Entity<StudentVle>().HasOne(sv => sv.StudentInfo)
            .WithMany(si => si.StudentVles)
            .HasForeignKey(sv => new { sv.CodeModule, sv.CodePresentation, sv.IdStudent });

        // store enums as integers
        modelBuilder.Entity<StudentInfo>().Property(s => s.Gender).HasConversion<int>();
        modelBuilder.Entity<StudentInfo>().Property(s => s.AgeBand).HasConversion<int>();
        modelBuilder.Entity<StudentInfo>().Property(s => s.Disability).HasConversion<int>();
        modelBuilder.Entity<StudentInfo>().Property(s => s.FinalResult).HasConversion<int>();
        modelBuilder.Entity<StudentInfo>().Property(s => s.HighestEducation).HasConversion<int>();
    }
}
