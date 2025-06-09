using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuladEtlEda.Models;

[Table("courses")]
public class Course
{
    [Key]
    [Column(Order = 0, TypeName = "varchar(8)")]
    [MaxLength(8)]
    public string CodeModule { get; set; } = null!;

    [Key]
    [Column(Order = 1, TypeName = "varchar(8)")]
    [MaxLength(8)]
    public string CodePresentation { get; set; } = null!;

    public int ModulePresentationLength { get; set; }

    public ICollection<StudentInfo> Students { get; set; } = new List<StudentInfo>();
    public ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
    public ICollection<StudentRegistration> Registrations { get; set; } = new List<StudentRegistration>();
    public ICollection<Vle> Vles { get; set; } = new List<Vle>();
}