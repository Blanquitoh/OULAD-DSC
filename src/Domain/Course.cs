using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuladEtlEda.Domain;

[Table("courses")]
public class Course : ICourseEntity
{
    public int ModulePresentationLength { get; set; }

    public ICollection<StudentInfo> Students { get; set; } = new List<StudentInfo>();
    public ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
    public ICollection<Vle> Vles { get; set; } = new List<Vle>();

    [Key]
    [Column(Order = 0, TypeName = "varchar(45)")]
    [MaxLength(45)]
    public string CodeModule { get; set; } = null!;

    [Key]
    [Column(Order = 1, TypeName = "varchar(45)")]
    [MaxLength(45)]
    public string CodePresentation { get; set; } = null!;
}