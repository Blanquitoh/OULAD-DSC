using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OuladEtlEda.Domain;

namespace OuladEtlEda.DataAccess;

public static class ModelBuilderExtensions
{
    public static void ConfigureCourseEntity<TEntity>(this EntityTypeBuilder<TEntity> entity)
        where TEntity : class, ICourseEntity
    {
        entity.Property(e => e.CodeModule)
            .HasMaxLength(45)
            .IsUnicode(false);
        entity.Property(e => e.CodePresentation)
            .HasMaxLength(45)
            .IsUnicode(false);
    }
}