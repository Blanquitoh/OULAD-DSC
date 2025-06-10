using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuladEtlEda.Domain;

[Table("studentVle")]
public class StudentVle : ICourseEntity
{
    [Key]
    [Column(Order = 0)]
    public int IdSite { get; set; }

    [Key]
    [Column(Order = 1)]
    public int IdStudent { get; set; }

    public int Date { get; set; }

    public int SumClick { get; set; }

    [ForeignKey("IdSite,CodeModule,CodePresentation")]
    public Vle? Vle { get; set; }

    [ForeignKey("CodeModule,CodePresentation,IdStudent")]
    public StudentInfo? StudentInfo { get; set; }

    [Key]
    [Column(Order = 2, TypeName = "varchar(45)")]
    [MaxLength(45)]
    public string CodeModule { get; set; } = null!;

    [Key]
    [Column(Order = 3, TypeName = "varchar(45)")]
    [MaxLength(45)]
    public string CodePresentation { get; set; } = null!;
}