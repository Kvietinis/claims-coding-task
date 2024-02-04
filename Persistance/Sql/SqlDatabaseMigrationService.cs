using Claims.Infrastructure.Sql;
using Microsoft.Extensions.DependencyInjection;

namespace Claims.Persistance.Sql
{
    public class SqlDatabaseMigrationService : BaseMigrationService<ClaimsSqlDbContext>
    {
        public SqlDatabaseMigrationService(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
        }
    }
}
