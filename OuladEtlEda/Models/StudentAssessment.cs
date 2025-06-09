using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuladEtlEda.Models;

[Table("studentAssessment")]
public class StudentAssessment
{
    [Key]
    [Column(Order = 0)]
    [ForeignKey(nameof(Assessment))]
    public int IdAssessment { get; set; }

    [Key]
    [Column(Order = 1)]
    public int IdStudent { get; set; }

    [Key]
    [Column(Order = 2, TypeName = "varchar(8)")]
    [MaxLength(8)]
    public string CodeModule { get; set; } = null!;

    [Key]
    [Column(Order = 3, TypeName = "varchar(8)")]
    [MaxLength(8)]
    public string CodePresentation { get; set; } = null!;

    public int? DateSubmitted { get; set; }

    public bool IsBanked { get; set; }

    public float? Score { get; set; }

    public Assessment? Assessment { get; set; }

    [ForeignKey("CodeModule,CodePresentation,IdStudent")]
    public StudentInfo? StudentInfo { get; set; }
}
