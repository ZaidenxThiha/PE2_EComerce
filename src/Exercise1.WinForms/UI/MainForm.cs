using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace Exercise1.WinForms.UI;

public class MainForm : Form
{
    private readonly IServiceProvider _services;
    private int _userId;
    private string _userName = string.Empty;

    public MainForm(IServiceProvider services)
    {
        _services = services;
        Text = "PE2 E-Commerce - Dashboard";
        Width = 960;
        Height = 640;

        var layout = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true,
            Padding = new Padding(20)
        };

        layout.Controls.Add(CreateNavButton("Items", (_, _) => ShowForm<ItemManagementForm>()));
        layout.Controls.Add(CreateNavButton("Agents", (_, _) => ShowForm<AgentManagementForm>()));
        layout.Controls.Add(CreateNavButton("Orders", (_, _) => ShowOrderForm()));
        layout.Controls.Add(CreateNavButton("Reports", (_, _) => ShowForm<ReportsForm>()));

        Controls.Add(layout);
    }

    public void SetUserContext(int userId, string userName)
    {
        _userId = userId;
        _userName = userName;
        Text = $"PE2 E-Commerce - Welcome {_userName}";
    }

    private Button CreateNavButton(string text, EventHandler handler)
    {
        var button = new Button
        {
            Text = text,
            Width = 200,
            Height = 80,
            Margin = new Padding(10),
            Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold)
        };
        button.Click += handler;
        return button;
    }

    private void ShowForm<T>() where T : Form
    {
        var form = ActivatorUtilities.CreateInstance<T>(_services);
        form.StartPosition = FormStartPosition.CenterParent;
        form.ShowDialog(this);
    }

    private void ShowOrderForm()
    {
        var orderForm = ActivatorUtilities.CreateInstance<OrderEntryForm>(_services);
        orderForm.SetUser(_userId);
        orderForm.StartPosition = FormStartPosition.CenterParent;
        orderForm.ShowDialog(this);
    }
}
