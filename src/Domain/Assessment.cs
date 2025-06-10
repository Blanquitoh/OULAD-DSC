using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuladEtlEda.Domain;

[Table("assessments")]
public class Assessment : ICourseEntity
{
    [Key]
    [Column(Order = 3)]
    public int IdAssessment { get; set; }

    [MaxLength(20)]
    [Column(TypeName = "varchar(20)")]
    public string AssessmentType { get; set; } = null!;

    public AssessmentType AssessmentTypeEnum { get; set; }

    public int? AssessmentTypeOrdinal { get; set; }

    public int? Date { get; set; }

    public decimal Weight { get; set; }

    [ForeignKey("CodeModule,CodePresentation")]
    public Course? Course { get; set; }


    [Key]
    [Column(Order = 0, TypeName = "varchar(45)")]
    [MaxLength(45)]
    public string CodeModule { get; set; } = null!;

    [Key]
    [Column(Order = 1, TypeName = "varchar(45)")]
    [MaxLength(45)]
    public string CodePresentation { get; set; } = null!;
}
