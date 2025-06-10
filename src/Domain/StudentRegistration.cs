using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuladEtlEda.Domain;

[Table("studentRegistration")]
public class StudentRegistration : ICourseEntity
{
    [Column(Order = 2)] public int IdStudent { get; set; }

    public int? DateRegistration { get; set; }

    public int? DateUnregistration { get; set; }

    [ForeignKey("CodeModule,CodePresentation")]
    public Course? Course { get; set; }

    [ForeignKey("IdStudent")]
    public StudentInfo? StudentInfo { get; set; }

    [Column(Order = 0, TypeName = "varchar(45)")]
    [MaxLength(45)]
    public string CodeModule { get; set; } = null!;

    [Column(Order = 1, TypeName = "varchar(45)")]
    [MaxLength(45)]
    public string CodePresentation { get; set; } = null!;
}
