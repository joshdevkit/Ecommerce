namespace Ecommerce.Domain.Common
{
    public class Result<T>
    {
        public bool Success { get; private set; }
        public string? Message { get; private set; }
        public T? Value { get; private set; }

        private Result(bool success, T? value = default, string? message = null)
        {
            Success = success;
            Value = value;
            Message = message;
        }

        public static Result<T> Ok(T value, string? message = null)
        {
            return new Result<T>(true, value, message);
        }

        public static Result<T> Fail(string message)
        {
            return new Result<T>(false, default, message);
        }
    }
}
