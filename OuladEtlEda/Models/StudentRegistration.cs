using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuladEtlEda.Models;

[Table("studentRegistration")]
public class StudentRegistration
{
    [Key]
    [Column(Order = 0)]
    [ForeignKey(nameof(Course))]
    public string CodeModule { get; set; } = null!;

    [Key]
    [Column(Order = 1)]
    [ForeignKey(nameof(Course))]
    public string CodePresentation { get; set; } = null!;

    [Key]
    [Column(Order = 2)]
    public int IdStudent { get; set; }

    public int? DateRegistration { get; set; }

    public int? DateUnregistration { get; set; }

    public Course? Course { get; set; }

    [ForeignKey("CodeModule,CodePresentation,IdStudent")]
    public StudentInfo? StudentInfo { get; set; }
}
