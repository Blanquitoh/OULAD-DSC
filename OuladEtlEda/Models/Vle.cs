using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuladEtlEda.Models;

[Table("vle")]
public class Vle
{
    [Key]
    public int IdSite { get; set; }

    [MaxLength(8)]
    public string CodeModule { get; set; } = null!;

    [MaxLength(8)]
    public string CodePresentation { get; set; } = null!;

    public string? ActivityType { get; set; }

    public int? WeekFrom { get; set; }

    public int? WeekTo { get; set; }

    [ForeignKey("CodeModule,CodePresentation")]
    public Course? Course { get; set; }
    public ICollection<StudentVle> StudentVles { get; set; } = new List<StudentVle>();
}
