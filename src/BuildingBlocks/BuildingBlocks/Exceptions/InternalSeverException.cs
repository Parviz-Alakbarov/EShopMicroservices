namespace BuildingBlocks.Exceptions;

public class InternalSeverException : Exception
{
    public InternalSeverException(string message) : base(message) { }
    public InternalSeverException(string message, string details) : base(message)
    {
        Details = details;
    }
    public string? Details { get; }
}
