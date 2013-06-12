namespace Benchy.Internal
{
    internal interface IExecutionOptionsValidator
    {
        bool Validate(IExecutionOptions options);
    }
}