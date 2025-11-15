using PE2.ECommerce.Services.Implementations;
using Xunit;

namespace PE2.ECommerce.Tests.Services;

public class ReportingServiceTests
{
    [Fact]
    public async Task GetOrderSummaries_ReturnsData()
    {
        using var context = TestDbFactory.CreateContext(nameof(GetOrderSummaries_ReturnsData));
        var service = new ReportingService(context);

        var summaries = await service.GetOrderSummariesAsync(DateTime.Today.Month, DateTime.Today.Year);

        Assert.NotEmpty(summaries);
    }
}
