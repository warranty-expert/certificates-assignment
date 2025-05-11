namespace InsuranceCertificates.Utils;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    public static Result Ok()
    {
        return new Result(true);
    }

    public static Result Fail()
    {
        return new Result(false);
    }

    public static Result<T, E> Ok<T, E>(T value)
    {
        return new Result<T, E>(value, true, default);
    }

    public static Result<T, E> Fail<T, E>(E error)
    {
        return new Result<T, E>(default, false, error);
    }
}

public class Result<T, E> : Result
{
    private readonly T _resultValue;
    private readonly E _error;

    public T Value
    {
        get
        {
            if (!IsSuccess)
            {
                throw new InvalidOperationException();
            }

            return _resultValue;
        }
    }

    public E Error
    {
        get
        {
            if (IsSuccess)
            {
                throw new InvalidOperationException();
            }

            return _error;
        }
    }

    protected internal Result(T resultValue, bool isSuccess, E error)
        : base(isSuccess)
    {
        _resultValue = resultValue;
        _error = error;
    }
}
