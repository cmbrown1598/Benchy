namespace Benchy.Framework
{
    internal interface IExecutionOptionsValidator
    {
        bool Validate(IExecutionOptions options);
    }
}