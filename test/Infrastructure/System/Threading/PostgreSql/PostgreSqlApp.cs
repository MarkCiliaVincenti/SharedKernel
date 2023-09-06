﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.EntityFrameworkCore.PostgreSQL;
using SharedKernel.Infrastructure.EntityFrameworkCore.PostgreSQL.System.Threading;
using SharedKernel.Integration.Tests.Data;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.PostgreSql;
using SharedKernel.Testing.Infrastructure;

namespace SharedKernel.Integration.Tests.System.Threading.PostgreSql;

public class PostgreSqlApp : InfrastructureTestCase<FakeStartup>
{
    protected override string GetJsonFile()
    {
        return "System/Threading/PostgreSql/appsettings.postgreSql.json";
    }

    public override void BeforeStart()
    {
        var dbContext = GetRequiredServiceOnNewScope<PostgreSqlSharedKernelDbContext>();
        //dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        var connection = Configuration.GetConnectionString("RepositoryConnectionString")!;

        return services
            .AddEntityFrameworkCorePostgreSqlUnitOfWorkAsync<ISharedKernelUnitOfWork, PostgreSqlSharedKernelDbContext>(connection)
            .AddPostgreSqlMutex(connection);
    }
}
