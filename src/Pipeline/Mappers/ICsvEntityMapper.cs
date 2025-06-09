namespace OuladEtlEda.Pipeline.Mappers;

public interface ICsvEntityMapper<TCsv, TEntity>
{
    TEntity Map(TCsv csv);
}