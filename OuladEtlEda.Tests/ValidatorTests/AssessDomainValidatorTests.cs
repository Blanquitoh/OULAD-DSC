using System.Threading.Tasks;
using OuladEtlEda;
using OuladEtlEda.Models;
using Xunit;

namespace OuladEtlEda.Tests.ValidatorTests;

public class AssessDomainValidatorTests
{
    [Fact]
    public async Task Throws_if_foreign_keys_null()
    {
        var validator = new AssessDomainValidator();
        var entity = new Assessment
        {
            CodeModule = null!,
            CodePresentation = null!,
            Weight = 1
        };

        await Assert.ThrowsAsync<DomainException>(() => validator.ValidateAsync(entity));
    }
}
