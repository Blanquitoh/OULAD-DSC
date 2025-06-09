using Xunit;
using OuladEtlEda;

namespace OuladEtlEda.Tests.MapperTests;

public class CategoricalOrdinalMapperTests
{
    [Fact]
    public void Same_value_returns_same_id()
    {
        var mapper = new CategoricalOrdinalMapper();
        var id1 = mapper.GetOrAdd("col", "A");
        var id2 = mapper.GetOrAdd("col", "A");
        Assert.Equal(id1, id2);
    }
}
