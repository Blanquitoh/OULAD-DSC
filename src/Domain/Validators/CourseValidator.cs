namespace OuladEtlEda.Domain.Validators;

public class CourseValidator : IDomainValidator<Course>
{
    public Task ValidateAsync(Course entity)
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
