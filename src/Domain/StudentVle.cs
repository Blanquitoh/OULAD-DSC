using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuladEtlEda.Domain;

[Table("studentVle")]
public class StudentVle : ICourseEntity
{
    [Column(Order = 3)]
    public int IdSite { get; set; }

    [Key]
    [Column(Order = 2)]
    public int IdStudent { get; set; }

    public int? Date { get; set; }

    public int SumClick { get; set; }

    [ForeignKey("CodeModule,CodePresentation,IdSite")]
    public Vle? Vle { get; set; }

    [ForeignKey("CodeModule,CodePresentation,IdStudent")]
    public StudentInfo? StudentInfo { get; set; }

    [Key]
    [Column(Order = 0, TypeName = "varchar(8)")]
    [MaxLength(8)]
    public string CodeModule { get; set; } = null!;

    [Key]
    [Column(Order = 1, TypeName = "varchar(8)")]
    [MaxLength(8)]
    public string CodePresentation { get; set; } = null!;
}