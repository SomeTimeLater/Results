namespace SomeTimeLater.Results.Failures;


public record ResourceConflict : DomainFailure
{
    internal ResourceConflict() : this("A resource conflict occurred") {}
    protected ResourceConflict(string message) : base(FailureCodes.Conflict, message) {}
}