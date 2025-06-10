using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuladEtlEda.Domain;

[Table("assessments")]
public class Assessment : ICourseEntity
{
    [Key]
    [Column(Order = 0)]
    public int IdAssessment { get; set; }

    [MaxLength(20)]
    [Column(TypeName = "varchar(20)")]
    public string? AssessmentType { get; set; }

    public int? AssessmentTypeOrdinal { get; set; }

    public int? Date { get; set; }

    public decimal Weight { get; set; }

    [ForeignKey("CodeModule,CodePresentation")]
    public Course? Course { get; set; }

    public ICollection<StudentAssessment> StudentAssessments { get; set; } = new List<StudentAssessment>();

    [Key]
    [Column(Order = 1, TypeName = "varchar(8)")]
    [MaxLength(8)]
    public string CodeModule { get; set; } = null!;

    [Key]
    [Column(Order = 2, TypeName = "varchar(8)")]
    [MaxLength(8)]
    public string CodePresentation { get; set; } = null!;
}