using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuladEtlEda.Models;

[Table("vle")]
public class Vle
{
    [Key]
    public int IdSite { get; set; }

    [ForeignKey(nameof(Course))]
    public string CodeModule { get; set; } = null!;

    [ForeignKey(nameof(Course))]
    public string CodePresentation { get; set; } = null!;

    public string? ActivityType { get; set; }

    public int? WeekFrom { get; set; }

    public int? WeekTo { get; set; }

    public Course? Course { get; set; }
    public ICollection<StudentVle> StudentVles { get; set; } = new List<StudentVle>();
}
