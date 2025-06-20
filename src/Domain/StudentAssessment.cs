using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuladEtlEda.Domain;

[Table("studentAssessment")]
public class StudentAssessment : ICourseEntity
{
    [Column(Order = 3)] public int IdAssessment { get; set; }

    [Column(Order = 2)] public int IdStudent { get; set; }

    public int? DateSubmitted { get; set; }

    public bool IsBanked { get; set; }

    public float? Score { get; set; }

    [ForeignKey("CodeModule,CodePresentation,IdAssessment")]
    public Assessment? Assessment { get; set; }

    [ForeignKey("CodeModule,CodePresentation,IdStudent")]
    public StudentInfo? StudentInfo { get; set; }

    [Column(Order = 0, TypeName = "varchar(45)")]
    [MaxLength(45)]
    public string CodeModule { get; set; } = null!;

    [Column(Order = 1, TypeName = "varchar(45)")]
    [MaxLength(45)]
    public string CodePresentation { get; set; } = null!;
}