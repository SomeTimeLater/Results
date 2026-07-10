namespace SomeTimeLater.Results.Failures;

public record DomainFailure : Failure
{
    protected DomainFailure(int code, string message) : base(code, message) {}
    
    public new static DomainFailure Create(int code, string message) => new (code, message);
}