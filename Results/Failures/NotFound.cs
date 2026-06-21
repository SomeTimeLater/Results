namespace Results.Failures;

public record NotFound : DomainFailure
{
    internal NotFound() : this("Resource not found") {}
    protected NotFound(string message) : base(FailureCodes.NotFound, message) {}
}