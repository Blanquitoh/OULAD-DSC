using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuladEtlEda.Domain;

[Table("vle")]
public class Vle : ICourseEntity
{
    [Key] public int IdSite { get; set; }

    [MaxLength(32)]
    [Column(TypeName = "varchar(32)")]
    public string? ActivityType { get; set; }

    public int? ActivityTypeOrdinal { get; set; }

    public int? WeekFrom { get; set; }

    public int? WeekTo { get; set; }

    [ForeignKey("CodeModule,CodePresentation")]
    public Course? Course { get; set; }

    public ICollection<StudentVle> StudentVles { get; set; } = new List<StudentVle>();

    [MaxLength(8)]
    [Column(TypeName = "varchar(8)")]
    public string CodeModule { get; set; } = null!;

    [MaxLength(8)]
    [Column(TypeName = "varchar(8)")]
    public string CodePresentation { get; set; } = null!;
}