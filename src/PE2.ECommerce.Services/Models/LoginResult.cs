namespace PE2.ECommerce.Services.Models;

public record LoginResult(bool Success, string? Error, int? UserId = null, string? Role = null);
