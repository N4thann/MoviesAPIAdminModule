namespace Domain.SeedWork.Core
{
    public sealed class Result<T> : BaseResult
    {
        public T? Success { get; private set; }

        private Result(T success) : base() { Success = success; }
        private Result(Failure failure) : base(failure) { }

        public static Result<T> AsSuccess(T success) => new(success);
        public static new Result<T> AsFailure(Failure failure) => new(failure);

        public Result<U> Map<U>(Func<T, U> transform)
        {
            return IsSuccess ? Result<U>.AsSuccess(transform(Success!)) : Result<U>.AsFailure(Failure!);
        }

        public Result<U> Bind<U>(Func<T, Result<U>> nextOperation)
        {
            return IsSuccess ? nextOperation(Success!) : Result<U>.AsFailure(Failure!);
        }
    }
}
