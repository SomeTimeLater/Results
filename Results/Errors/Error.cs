using SomeTimeLater.Results.Failures;

namespace SomeTimeLater.Results.Errors;

public abstract record Error
{
    public Failure Failure { get; }
    public Type FailureType => Failure.GetType();
    public int Code => Failure.Code;
    
    protected Error(Failure failure) 
    {
        Failure = failure;
    }
    
    public bool IsFailureType<TFailure>() 
        where  TFailure : Failure
        => FailureType == typeof(TFailure);

    public abstract string ToLogMessage();
}

public abstract record Error<T> : Error
{
    public Type ExpectedOutputType => typeof(T);

    protected Error(Failure failure) 
        : base(failure)
    {
    }
    
    public bool IsExpectedOutputType<TOutput>() 
        => IsExpectedOutputType(typeof(TOutput));
    
    public bool IsExpectedOutputType(Type type) 
        => ExpectedOutputType == type;
}