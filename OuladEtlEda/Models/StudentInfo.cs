using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuladEtlEda.Models;

[Table("studentInfo")]
public class StudentInfo
{
    [Key]
    [Column(Order = 0)]
    public string CodeModule { get; set; } = null!;

    [Key]
    [Column(Order = 1)]
    public string CodePresentation { get; set; } = null!;

    [Key]
    [Column(Order = 2)]
    public int IdStudent { get; set; }

    public Gender Gender { get; set; }

    public string? Region { get; set; }

    public EducationLevel HighestEducation { get; set; }

    public string? ImdBand { get; set; }

    public AgeBand AgeBand { get; set; }

    public int NumOfPrevAttempts { get; set; }

    public int StudiedCredits { get; set; }

    public Disability Disability { get; set; }

    public FinalResult FinalResult { get; set; }

    [ForeignKey("CodeModule,CodePresentation")]
    public Course? Course { get; set; }
    public ICollection<StudentRegistration> Registrations { get; set; } = new List<StudentRegistration>();
    public ICollection<StudentAssessment> Assessments { get; set; } = new List<StudentAssessment>();
    public ICollection<StudentVle> StudentVles { get; set; } = new List<StudentVle>();
}
