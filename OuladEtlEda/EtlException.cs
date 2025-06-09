namespace OuladEtlEda;

public class EtlException : Exception
{
    public EtlException()
    {
    }

    public EtlException(string message) : base(message)
    {
    }

    public EtlException(string message, Exception inner) : base(message, inner)
    {
    }
}