using PE2.ECommerce.Services.Implementations;
using Xunit;

namespace PE2.ECommerce.Tests.Services;

public class AuthServiceTests
{
    [Fact]
    public async Task LoginAsync_ReturnsSuccess_ForValidUser()
    {
        using var context = TestDbFactory.CreateContext(nameof(LoginAsync_ReturnsSuccess_ForValidUser));
        var service = new AuthService(context);

        var result = await service.LoginAsync("tester", "test");

        Assert.True(result.Success);
        Assert.NotNull(result.UserId);
    }

    [Fact]
    public async Task LoginAsync_ReturnsError_ForInvalidPassword()
    {
        using var context = TestDbFactory.CreateContext(nameof(LoginAsync_ReturnsError_ForInvalidPassword));
        var service = new AuthService(context);

        var result = await service.LoginAsync("tester", "bad");

        Assert.False(result.Success);
        Assert.Equal("Invalid password", result.Error);
    }
}
