using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Documents;
using SharedKernel.Infrastructure.NPOI.Documents.Excel;

namespace SharedKernel.Infrastructure.NPOI;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddDotNetDatabaseReader(this IServiceCollection services)
    {
        return services
            .AddTransient<IExcelReader, NpoiExcelReader>();
    }
}
