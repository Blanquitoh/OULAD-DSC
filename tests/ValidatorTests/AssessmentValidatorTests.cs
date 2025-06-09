using System.Threading.Tasks;
using OuladEtlEda.Domain;
using OuladEtlEda.Domain.Validators;
using Xunit;

namespace OuladEtlEda.Tests.ValidatorTests;

public class AssessmentValidatorTests
{
    [Fact]
    public async Task Throws_if_foreign_keys_null()
    {
        var validator = new AssessmentValidator();
        var entity = new Assessment
        {
            CodeModule = null!,
            CodePresentation = null!,
            Weight = 1
        };

        await Assert.ThrowsAsync<DomainException>(() => validator.ValidateAsync(entity));
    }
}