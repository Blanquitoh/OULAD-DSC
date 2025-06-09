using System;
using System.Threading.Tasks;
using OuladEtlEda.Models;

namespace OuladEtlEda;

/// <summary>
/// Domain validator for <see cref="Assessment"/> entities.
/// </summary>
public class AssessDomainValidator : IDomainValidator<Assessment>
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
