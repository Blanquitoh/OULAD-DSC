using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuladEtlEda.Domain;

[Table("studentAssessment")]
public class StudentAssessment : ICourseEntity
{
    [Key]
    [Column(Order = 0)]
    public int IdAssessment { get; set; }

    [Key] [Column(Order = 1)] public int IdStudent { get; set; }

    public int? DateSubmitted { get; set; }

    public bool IsBanked { get; set; }

    public float? Score { get; set; }

    [ForeignKey("IdAssessment,CodeModule,CodePresentation")]
    public Assessment? Assessment { get; set; }

    [ForeignKey("CodeModule,CodePresentation,IdStudent")]
    public StudentInfo? StudentInfo { get; set; }

    [Key]
    [Column(Order = 2, TypeName = "varchar(8)")]
    [MaxLength(8)]
    public string CodeModule { get; set; } = null!;

    [Key]
    [Column(Order = 3, TypeName = "varchar(8)")]
    [MaxLength(8)]
    public string CodePresentation { get; set; } = null!;
}