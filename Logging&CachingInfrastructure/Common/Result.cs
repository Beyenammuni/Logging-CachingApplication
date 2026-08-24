namespace Logging_CachingInfrastructure.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string? Error { get; }

        protected Result(bool isSuccess, string? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new Result(true, null);
        public static Result Failure(string error) => new Result(false, error);
    }

    public class Result<T> : Result
    {
        public T? Value { get; }

        protected internal Result(T value) : base(true, null)
        {
            Value = value;
        }

        protected internal Result(string error) : base(false, error)
        {
            Value = default;
        }

        public static Result<T> Success(T value) => new Result<T>(value);
        public new static Result<T> Failure(string error) => new Result<T>(error);
    }
}
