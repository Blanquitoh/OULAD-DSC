using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuladEtlEda.Domain;

[Table("vle")]
public class Vle : ICourseEntity
{
    [Key]
    [Column(Order = 0)]
    public int IdSite { get; set; }

    [MaxLength(45)]
    [Column(TypeName = "varchar(45)")]
    public string ActivityType { get; set; } = null!;

    public int? ActivityTypeOrdinal { get; set; }

    public int? WeekFrom { get; set; }

    public int? WeekTo { get; set; }

    [ForeignKey("CodeModule,CodePresentation")]
    public Course? Course { get; set; }


    [Column(Order = 1, TypeName = "varchar(45)")]
    [MaxLength(45)]
    public string CodeModule { get; set; } = null!;

    [Column(Order = 2, TypeName = "varchar(45)")]
    [MaxLength(45)]
    public string CodePresentation { get; set; } = null!;
}