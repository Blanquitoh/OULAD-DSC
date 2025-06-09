using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuladEtlEda.Models;

[Table("studentVle")]
public class StudentVle
{
    [Key]
    [Column(Order = 0)]
    [ForeignKey(nameof(Vle))]
    public int IdSite { get; set; }

    [Key]
    [Column(Order = 1)]
    public int IdStudent { get; set; }

    [Key]
    [Column(Order = 2)]
    [MaxLength(8)]
    public string CodeModule { get; set; } = null!;

    [Key]
    [Column(Order = 3)]
    [MaxLength(8)]
    public string CodePresentation { get; set; } = null!;

    public int? Date { get; set; }

    public int SumClick { get; set; }

    public Vle? Vle { get; set; }

    [ForeignKey("CodeModule,CodePresentation,IdStudent")]
    public StudentInfo? StudentInfo { get; set; }
}
