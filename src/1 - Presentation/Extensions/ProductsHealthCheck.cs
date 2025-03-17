using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyApiV8.Domain.Interfaces.Repositories;

namespace MyApiV8.Extensions;

public class ProductsHealthCheck(IProductRepository productRepository) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var products = await productRepository.GetAllAsync();

        try
        {
            return products.Any()
                ? HealthCheckResult.Healthy("Checks if there are products in the database.")
                : HealthCheckResult.Unhealthy("There are no products in the database.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(ex.Message);
        }
    }
}
