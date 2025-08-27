namespace Domain.SeedWork.Core
{
    public class BaseResult
    {
        public Failure? Failure { get; protected set; }
        public bool IsSuccess => Failure == null;
        public bool IsFailure => !IsSuccess;

        protected BaseResult() { }
        protected BaseResult(Failure failure) { Failure = failure; }

        public static BaseResult AsSuccess() => new();
        public static BaseResult AsFailure(Failure failure) => new(failure);
    }
}
