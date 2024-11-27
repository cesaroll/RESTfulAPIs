using Microsoft.Extensions.Diagnostics.HealthChecks;
using Movies.App.Health;

namespace Movies.Api.Health;

public class DatabaseHealthCheck : IHealthCheck
{
    public const string Name = "Database";
    private readonly IDatabaseHealthCheck _dbHealthCheck;

    public DatabaseHealthCheck(IDatabaseHealthCheck dbHealthCheck)
    {
        _dbHealthCheck = dbHealthCheck;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
        => await _dbHealthCheck.CheckHealthAsync(default)
        ? HealthCheckResult.Healthy()
        : HealthCheckResult.Unhealthy();
}
