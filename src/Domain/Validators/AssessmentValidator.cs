using OuladEtlEda.Domain;

namespace OuladEtlEda.Domain.Validators;

public class AssessmentValidator : IDomainValidator<Assessment>
{
    public Task ValidateAsync(Assessment entity)
    {
        if (entity == null)
            throw new DomainException("Entity cannot be null");

        if (string.IsNullOrWhiteSpace(entity.CodeModule))
            throw new DomainException("CodeModule is required");

        if (string.IsNullOrWhiteSpace(entity.CodePresentation))
            throw new DomainException("CodePresentation is required");

        if (entity.Weight < 0)
            throw new DomainException("Weight cannot be negative");

        return Task.CompletedTask;
    }
}