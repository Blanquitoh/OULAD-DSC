using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuladEtlEda.Domain;

[Table("assessments")]
public class Assessment : ICourseEntity
{
    [Key] public int IdAssessment { get; set; }

    [MaxLength(8)]
    [Column(TypeName = "varchar(8)")]
    public string CodeModule { get; set; } = null!;

    [MaxLength(8)]
    [Column(TypeName = "varchar(8)")]
    public string CodePresentation { get; set; } = null!;

    public string? AssessmentType { get; set; }

    public int? Date { get; set; }

    public decimal Weight { get; set; }

    [ForeignKey("CodeModule,CodePresentation")]
    public Course? Course { get; set; }

    public ICollection<StudentAssessment> StudentAssessments { get; set; } = new List<StudentAssessment>();
}
