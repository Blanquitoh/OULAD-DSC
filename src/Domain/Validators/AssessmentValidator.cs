namespace OuladEtlEda.Domain.Validators;

public class AssessmentValidator : CourseEntityValidator<Assessment>
{
    public override async Task ValidateAsync(Assessment entity)
    {
        await base.ValidateAsync(entity);

        if (entity.Weight < 0)
            throw new DomainException("Weight cannot be negative");
    }
}