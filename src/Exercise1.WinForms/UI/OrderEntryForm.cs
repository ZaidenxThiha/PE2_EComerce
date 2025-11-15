using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PE2.ECommerce.Domain.Entities;
using PE2.ECommerce.Services.Interfaces;

namespace Exercise1.WinForms.UI;

public class OrderEntryForm : Form
{
    private readonly IAgentService _agentService;
    private readonly IItemService _itemService;
    private readonly IOrderService _orderService;

    private readonly ComboBox _agentCombo = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly DateTimePicker _orderDate = new() { Value = DateTime.Today };
    private readonly ComboBox _itemCombo = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly NumericUpDown _quantity = new() { Minimum = 1, Maximum = 500 };
    private readonly Button _addLine = new() { Text = "Add Item" };
    private readonly DataGridView _grid = new() { Dock = DockStyle.Fill, AutoGenerateColumns = false };
    private readonly Button _saveOrder = new() { Text = "Save Order" };
    private readonly BindingList<OrderLine> _lines = new();

    private List<Agent> _agents = new();
    private List<Item> _items = new();
    private int _userId;

    public OrderEntryForm(IAgentService agentService, IItemService itemService, IOrderService orderService)
    {
        _agentService = agentService;
        _itemService = itemService;
        _orderService = orderService;
        Text = "Order Entry";
        Width = 860;
        Height = 600;

        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(OrderLine.ItemName), HeaderText = "Item", Width = 220 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(OrderLine.Quantity), HeaderText = "Quantity" });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(OrderLine.UnitAmount), HeaderText = "Unit Price" });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(OrderLine.Total), HeaderText = "Line Total" });
        _grid.DataSource = _lines;

        var header = new TableLayoutPanel { Dock = DockStyle.Top, Height = 120, ColumnCount = 4, Padding = new Padding(10) };
        header.Controls.Add(new Label { Text = "Agent" }, 0, 0);
        header.Controls.Add(_agentCombo, 1, 0);
        header.Controls.Add(new Label { Text = "Order Date" }, 2, 0);
        header.Controls.Add(_orderDate, 3, 0);
        header.Controls.Add(new Label { Text = "Item" }, 0, 1);
        header.Controls.Add(_itemCombo, 1, 1);
        header.Controls.Add(new Label { Text = "Quantity" }, 2, 1);
        header.Controls.Add(_quantity, 3, 1);
        header.Controls.Add(_addLine, 3, 2);

        _addLine.Click += (_, _) => AddLine();
        _saveOrder.Dock = DockStyle.Bottom;
        _saveOrder.Height = 40;
        _saveOrder.Click += async (_, _) => await SaveOrderAsync();

        Controls.Add(_grid);
        Controls.Add(_saveOrder);
        Controls.Add(header);

        Shown += async (_, _) => await LoadReferenceDataAsync();
    }

    public void SetUser(int userId) => _userId = userId;

    private async Task LoadReferenceDataAsync()
    {
        _agents = new List<Agent>(await _agentService.GetAsync());
        _items = new List<Item>(await _itemService.GetAsync());
        _agentCombo.DataSource = _agents;
        _agentCombo.DisplayMember = nameof(Agent.AgentName);
        _agentCombo.ValueMember = nameof(Agent.AgentId);
        _itemCombo.DataSource = _items;
        _itemCombo.DisplayMember = nameof(Item.ItemName);
        _itemCombo.ValueMember = nameof(Item.ItemId);
    }

    private void AddLine()
    {
        if (_itemCombo.SelectedItem is not Item item)
        {
            return;
        }

        var quantity = (int)_quantity.Value;
        var existing = _lines.FirstOrDefault(l => l.ItemId == item.ItemId);
        if (existing is not null)
        {
            existing.Quantity += quantity;
            _grid.Refresh();
            return;
        }

        _lines.Add(new OrderLine
        {
            ItemId = item.ItemId,
            ItemName = item.ItemName,
            Quantity = quantity,
            UnitAmount = item.UnitPrice
        });
    }

    private async Task SaveOrderAsync()
    {
        if (_agentCombo.SelectedItem is not Agent agent)
        {
            MessageBox.Show("Select an agent");
            return;
        }

        if (_lines.Count == 0)
        {
            MessageBox.Show("Add at least one line");
            return;
        }

        var order = new Order
        {
            AgentId = agent.AgentId,
            CreatedBy = _userId,
            OrderDate = _orderDate.Value,
            Details = _lines.Select(l => new OrderDetail
            {
                ItemId = l.ItemId,
                Quantity = l.Quantity,
                UnitAmount = l.UnitAmount
            }).ToList()
        };

        await _orderService.CreateAsync(order);
        MessageBox.Show($"Order {order.OrderId} saved");
        _lines.Clear();
    }

    private sealed class OrderLine
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitAmount { get; set; }
        public decimal Total => Quantity * UnitAmount;
    }
}
