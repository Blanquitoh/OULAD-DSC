using OuladEtlEda.Pipeline;
using Xunit;

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

    [Fact]
    public void Different_value_returns_incremented_id()
    {
        var mapper = new CategoricalOrdinalMapper();
        var id1 = mapper.GetOrAdd("col", "A");
        var id2 = mapper.GetOrAdd("col", "B");
        Assert.Equal(id1 + 1, id2);
    }

    [Fact]
    public void Same_value_in_different_columns_independent()
    {
        var mapper = new CategoricalOrdinalMapper();
        var id1 = mapper.GetOrAdd("col1", "A");
        var id2 = mapper.GetOrAdd("col2", "A");
        Assert.Equal(0, id1);
        Assert.Equal(0, id2);
    }
}