using Microsoft.EntityFrameworkCore;
using OuladEtlEda.Domain;

namespace OuladEtlEda.DataAccess;

public class OuladContext : DbContext
{
    public OuladContext(DbContextOptions<OuladContext> options) : base(options)
    {
    }

    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Assessment> Assessments => Set<Assessment>();
    public DbSet<StudentAssessment> StudentAssessments => Set<StudentAssessment>();
    public DbSet<StudentInfo> StudentInfos => Set<StudentInfo>();
    public DbSet<StudentRegistration> StudentRegistrations => Set<StudentRegistration>();
    public DbSet<Vle> Vles => Set<Vle>();
    public DbSet<StudentVle> StudentVles => Set<StudentVle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(c => new { c.CodeModule, c.CodePresentation });
            entity.ConfigureCourseEntity();
        });
        modelBuilder.Entity<Assessment>(entity =>
        {
            entity.ConfigureCourseEntity();
            entity.HasOne(a => a.Course)
                .WithMany(c => c.Assessments)
                .HasForeignKey(a => new { a.CodeModule, a.CodePresentation })
                .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<Vle>(entity =>
        {
            entity.ConfigureCourseEntity();
            entity.HasOne(v => v.Course)
                .WithMany(c => c.Vles)
                .HasForeignKey(v => new { v.CodeModule, v.CodePresentation })
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<StudentInfo>(entity =>
        {
            entity.HasKey(s => new { s.CodeModule, s.CodePresentation, s.IdStudent });
            entity.ConfigureCourseEntity();
            entity.HasOne(s => s.Course)
                .WithMany(c => c.Students)
                .HasForeignKey(s => new { s.CodeModule, s.CodePresentation })
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<StudentRegistration>(entity =>
        {
            entity.HasKey(r => new { r.CodeModule, r.CodePresentation, r.IdStudent });
            entity.ConfigureCourseEntity();
            entity.HasOne(r => r.Course)
                .WithMany(c => c.Registrations)
                .HasForeignKey(r => new { r.CodeModule, r.CodePresentation })
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(r => r.StudentInfo)
                .WithMany(s => s.Registrations)
                .HasForeignKey(r => new { r.CodeModule, r.CodePresentation, r.IdStudent })
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<StudentAssessment>(entity =>
        {
            entity.HasKey(sa => new { sa.IdAssessment, sa.IdStudent, sa.CodeModule, sa.CodePresentation });
            entity.ConfigureCourseEntity();
            entity.HasOne(sa => sa.Assessment)
                .WithMany(a => a.StudentAssessments)
                .HasForeignKey(sa => sa.IdAssessment)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(sa => sa.StudentInfo)
                .WithMany(si => si.Assessments)
                .HasForeignKey(sa => new { sa.CodeModule, sa.CodePresentation, sa.IdStudent })
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<StudentVle>(entity =>
        {
            entity.HasKey(sv => new { sv.IdSite, sv.IdStudent, sv.CodeModule, sv.CodePresentation });
            entity.ConfigureCourseEntity();
            entity.HasOne(sv => sv.Vle)
                .WithMany(v => v.StudentVles)
                .HasForeignKey(sv => sv.IdSite)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(sv => sv.StudentInfo)
                .WithMany(si => si.StudentVles)
                .HasForeignKey(sv => new { sv.CodeModule, sv.CodePresentation, sv.IdStudent })
                .OnDelete(DeleteBehavior.Restrict);
        });

        // store enums as integers
        modelBuilder.Entity<StudentInfo>().Property(s => s.Gender).HasConversion<int>();
        modelBuilder.Entity<StudentInfo>().Property(s => s.AgeBand).HasConversion<int>();
        modelBuilder.Entity<StudentInfo>().Property(s => s.Disability).HasConversion<int>();
        modelBuilder.Entity<StudentInfo>().Property(s => s.FinalResult).HasConversion<int>();
        modelBuilder.Entity<StudentInfo>().Property(s => s.HighestEducation).HasConversion<int>();
    }
}