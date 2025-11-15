using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using PE2.ECommerce.Services.Interfaces;

namespace Exercise1.WinForms.UI;

public class ReportsForm : Form
{
    private readonly IReportingService _reportingService;
    private readonly NumericUpDown _month = new() { Minimum = 1, Maximum = 12, Value = DateTime.Today.Month };
    private readonly NumericUpDown _year = new() { Minimum = 2020, Maximum = 2100, Value = DateTime.Today.Year };
    private readonly Button _load = new() { Text = "Load" };
    private readonly DataGridView _orders = new() { Dock = DockStyle.Top, Height = 200 };
    private readonly DataGridView _bestItems = new() { Dock = DockStyle.Top, Height = 200 };
    private readonly DataGridView _agents = new() { Dock = DockStyle.Fill };

    public ReportsForm(IReportingService reportingService)
    {
        _reportingService = reportingService;
        Text = "Reports";
        Width = 900;
        Height = 720;

        var filterPanel = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40 };
        filterPanel.Controls.Add(new Label { Text = "Month" });
        filterPanel.Controls.Add(_month);
        filterPanel.Controls.Add(new Label { Text = "Year" });
        filterPanel.Controls.Add(_year);
        filterPanel.Controls.Add(_load);
        _load.Click += async (_, _) => await LoadDataAsync();

        Controls.Add(_agents);
        Controls.Add(_bestItems);
        Controls.Add(_orders);
        Controls.Add(filterPanel);

        Shown += async (_, _) => await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var orderSummaries = await _reportingService.GetOrderSummariesAsync((int)_month.Value, (int)_year.Value);
        var bestItems = await _reportingService.GetBestItemsAsync();
        var agents = await _reportingService.GetAgentPurchasesAsync();
        _orders.DataSource = orderSummaries;
        _bestItems.DataSource = bestItems;
        _agents.DataSource = agents;
    }
}
