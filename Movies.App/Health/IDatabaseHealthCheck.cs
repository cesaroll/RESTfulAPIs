namespace Movies.App.Health;

public interface IDatabaseHealthCheck
{
    Task<bool> CheckHealthAsync(CancellationToken cancellationToken = default);
}
