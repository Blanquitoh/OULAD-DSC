using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuladEtlEda.Domain;

[Table("studentInfo")]
public class StudentInfo : ICourseEntity
{
    [Key] [Column(Order = 2)] public int IdStudent { get; set; }

    public Gender Gender { get; set; }

    [MaxLength(45)]
    [Column(TypeName = "varchar(45)")]
    public string Region { get; set; } = null!;

    public int? RegionOrdinal { get; set; }

    public EducationLevel HighestEducation { get; set; }

    [MaxLength(16)]
    [Column(TypeName = "varchar(16)")]
    public string ImdBand { get; set; } = null!;

    public int? ImdBandOrdinal { get; set; }

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

    [Column(Order = 0, TypeName = "varchar(45)")]
    [MaxLength(45)]
    public string CodeModule { get; set; } = null!;

    [Column(Order = 1, TypeName = "varchar(45)")]
    [MaxLength(45)]
    public string CodePresentation { get; set; } = null!;
}