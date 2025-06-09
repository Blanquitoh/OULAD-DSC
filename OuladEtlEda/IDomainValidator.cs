namespace OuladEtlEda;

public interface IDomainValidator<T>
{
    Task ValidateAsync(T entity);
}