﻿using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Services;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.PostgreSql.DbContexts;

public class PostgreSqlSharedKernelDbContext : EntityFrameworkDbContext, IPostgreSqlSharedKernelUnitOfWork
{
    public PostgreSqlSharedKernelDbContext(DbContextOptions<PostgreSqlSharedKernelDbContext> options,
        IClassValidatorService? classValidatorService = default, IAuditableService? auditableService = default) : base(
        options, "skr", typeof(SharedKernelEntityFrameworkDbContext).Assembly, classValidatorService, auditableService)
    {
    }
}