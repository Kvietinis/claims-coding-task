using Claims.Infrastructure.Sql;
using Microsoft.Extensions.DependencyInjection;

namespace Claims.Auditing.Implementations
{
    public class SqlAuditingMigrationService : BaseMigrationService<AuditContext>
    {
        public SqlAuditingMigrationService(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
        }
    }
}
