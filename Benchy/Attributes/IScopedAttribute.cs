namespace Benchy.Attributes
{
    interface IScopedAttribute
    {
        ExecutionScope ExecutionScope { get; }
    }
}