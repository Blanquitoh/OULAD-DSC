using System.Threading.Tasks;

namespace OuladEtlEda;

/// <summary>
/// Validates domain entities.
/// </summary>
public interface IDomainValidator<T>
{
    /// <summary>
    /// Validates the specified entity.
    /// </summary>
    Task ValidateAsync(T entity);
}
