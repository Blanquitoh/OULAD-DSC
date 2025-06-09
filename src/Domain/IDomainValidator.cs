namespace OuladEtlEda.Domain;

public interface IDomainValidator<T>
{
    Task ValidateAsync(T entity);
}