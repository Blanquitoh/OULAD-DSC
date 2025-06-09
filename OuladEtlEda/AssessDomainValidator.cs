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
            throw new ArgumentNullException(nameof(entity));

        if (string.IsNullOrWhiteSpace(entity.CodeModule))
            throw new ArgumentException("CodeModule is required", nameof(entity));

        if (string.IsNullOrWhiteSpace(entity.CodePresentation))
            throw new ArgumentException("CodePresentation is required", nameof(entity));

        if (entity.Weight < 0)
            throw new ArgumentException("Weight cannot be negative", nameof(entity));

        return Task.CompletedTask;
    }
}
