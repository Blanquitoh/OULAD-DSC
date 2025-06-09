namespace OuladEtlEda.Domain.Validators;

public class CourseEntityValidator<T> : IDomainValidator<T> where T : ICourseEntity
{
    public virtual Task ValidateAsync(T entity)
    {
        if (entity == null)
            throw new DomainException("Entity cannot be null");
        if (string.IsNullOrWhiteSpace(entity.CodeModule))
            throw new DomainException("CodeModule is required");
        if (string.IsNullOrWhiteSpace(entity.CodePresentation))
            throw new DomainException("CodePresentation is required");
        return Task.CompletedTask;
    }
}