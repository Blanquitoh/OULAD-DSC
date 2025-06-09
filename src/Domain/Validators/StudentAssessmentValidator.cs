namespace OuladEtlEda.Domain.Validators;

public class StudentAssessmentValidator : IDomainValidator<StudentAssessment>
{
    public Task ValidateAsync(StudentAssessment entity)
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
