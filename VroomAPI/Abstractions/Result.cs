namespace VroomAPI.Abstractions {

    public class Result {
        protected Result(bool isSucess, Error error) {
            if (isSucess && error != Error.None ||
                !isSucess && error == Error.None) {
                throw new ArgumentException("Invalid exception", nameof(error));
            }

            IsSucess = isSucess;
            Error = error;
        }

        public bool IsSucess { get; }
        public bool IsFailure => !IsSucess;
        public Error Error { get; }

        public static Result Success() => new Result(true, Error.None);
        public static Result Failure(Error error) => new Result(false, error);
    }

    public class Result<T> : Result {
        private readonly T? _value;

        private Result(T? value, bool isSucess, Error error) : base(isSucess, error) {
            _value = value;
        }

        public T Value => IsSucess 
            ? _value! 
            : throw new InvalidOperationException("The value of a failure result can't be accessed.");

        public static Result<T> Success(T value) => new(value, true, Error.None);
        public static new Result<T> Failure(Error error) => new(default, false, error);
    }
}
