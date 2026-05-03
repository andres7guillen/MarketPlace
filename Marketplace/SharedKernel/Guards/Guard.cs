using CSharpFunctionalExtensions;

namespace SharedKernel.Guards;

public static class Guard
{
    public static Result<T> AgainstNullOrEmpty<T>(T value, string message)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            return Result.Failure<T>(message);

        return Result.Success(value);
    }
}
