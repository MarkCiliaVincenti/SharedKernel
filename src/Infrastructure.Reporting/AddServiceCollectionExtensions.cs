using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Reporting;

namespace SharedKernel.Infrastructure.Reporting;

/// <summary>  </summary>
public static class AddServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddReportingRenderer(this IServiceCollection services)
    {
        return services
            .AddTransient<IReportRenderer, ReportRenderer>();
    }
}
