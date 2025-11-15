using System;

namespace PE2.ECommerce.Services.Models;

public record OrderSummaryModel(int OrderId, DateTime OrderDate, string AgentName, decimal TotalAmount, string CreatedBy);
