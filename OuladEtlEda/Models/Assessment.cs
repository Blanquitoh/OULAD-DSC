using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuladEtlEda.Models;

[Table("assessments")]
public class Assessment
{
    [Key]
    public int IdAssessment { get; set; }

    [ForeignKey(nameof(Course))]
    public string CodeModule { get; set; } = null!;

    [ForeignKey(nameof(Course))]
    public string CodePresentation { get; set; } = null!;

    public string? AssessmentType { get; set; }

    public int? Date { get; set; }

    public int Weight { get; set; }

    public Course? Course { get; set; }
    public ICollection<StudentAssessment> StudentAssessments { get; set; } = new List<StudentAssessment>();
}
