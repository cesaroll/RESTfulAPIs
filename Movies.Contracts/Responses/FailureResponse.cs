namespace Movies.Contracts.Responses;

public class FailureResponse
{
    public required string Type { get; init; }
    public required string Title { get; init; }
    public required int Status { get; init; }
    public required string Detail { get; init; }

    public required IEnumerable<ValidationResponse> Errors { get; init; }
}

public class ValidationResponse
{
    public required string PopertyName { get; init; }
    public required string Message { get; init; }
}