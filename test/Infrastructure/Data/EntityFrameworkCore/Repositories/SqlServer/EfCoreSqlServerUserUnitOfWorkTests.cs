﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.EntityFrameworkCore.SqlServer.Data;
using SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.SqlServer;

[Collection("DockerHook")]
public class EfCoreSqlServerUserUnitOfWorkTests : UserUnitOfWorkTests<EfCoreUserRepository, ISharedKernelEntityFrameworkUnitOfWork>
{
    protected override string GetJsonFile()
    {
        return "Data/EntityFrameworkCore/Repositories/SqlServer/appsettings.sqlServer.json";
    }

    public override void BeforeStart()
    {
        var dbContext = GetRequiredServiceOnNewScope<SharedKernelEntityFrameworkDbContext>();
        //dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        var connection = Configuration.GetConnectionString("RepositoryConnectionString")!;
        return services
            .AddEntityFrameworkCoreSqlServerUnitOfWork<ISharedKernelEntityFrameworkUnitOfWork,
                SharedKernelEntityFrameworkDbContext>(connection)
            .AddTransient<EfCoreUserRepository>();
    }
}