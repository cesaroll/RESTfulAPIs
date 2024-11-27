using Microsoft.Extensions.Logging;
using Movies.Db;

namespace Movies.App.Health;

public class DatabaseHealthCheck : IDatabaseHealthCheck
{
    private readonly MoviesDbContext _dbContext;
    private readonly ILogger<DatabaseHealthCheck> _logger;

    public DatabaseHealthCheck(
        MoviesDbContext dbContext, ILogger<DatabaseHealthCheck> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<bool> CheckHealthAsync(CancellationToken cancellationToken = default)
    {
        try {

            return await _dbContext.Database.CanConnectAsync(cancellationToken);

        } catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot connect to DB");
            return false;
        }
    }
}
