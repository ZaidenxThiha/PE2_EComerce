using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using PE2.ECommerce.Domain.Entities;
using PE2.ECommerce.Services.Interfaces;

namespace Exercise1.WinForms.UI;

public class ItemManagementForm : Form
{
    private readonly IItemService _itemService;
    private readonly DataGridView _grid = new() { Dock = DockStyle.Top, Height = 260, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false };
    private readonly TextBox _name = new() { PlaceholderText = "Item name" };
    private readonly TextBox _size = new() { PlaceholderText = "Size" };
    private readonly NumericUpDown _stock = new() { Minimum = 0, Maximum = 10000, DecimalPlaces = 0 };
    private readonly NumericUpDown _price = new() { Minimum = 0, Maximum = 1000, DecimalPlaces = 2, Increment = 0.5M };
    private readonly CheckBox _isActive = new() { Text = "Active", Checked = true };
    private readonly Button _save = new() { Text = "Save" };
    private readonly Button _delete = new() { Text = "Delete" };
    private readonly Label _status = new() { Dock = DockStyle.Bottom, Height = 24 };
    private int _currentId;

    public ItemManagementForm(IItemService itemService)
    {
        _itemService = itemService;
        Text = "Items";
        Width = 720;
        Height = 520;

        _grid.AutoGenerateColumns = false;
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Item.ItemName), HeaderText = "Name", Width = 220 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Item.Size), HeaderText = "Size" });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Item.UnitInStock), HeaderText = "Stock" });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Item.UnitPrice), HeaderText = "Price" });
        _grid.Columns.Add(new DataGridViewCheckBoxColumn { DataPropertyName = nameof(Item.IsActive), HeaderText = "Active" });
        _grid.SelectionChanged += (_, _) => LoadFromSelection();

        var formLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 6,
            Padding = new Padding(10)
        };

        formLayout.Controls.Add(new Label { Text = "Name" }, 0, 0);
        formLayout.Controls.Add(_name, 1, 0);
        formLayout.Controls.Add(new Label { Text = "Size" }, 0, 1);
        formLayout.Controls.Add(_size, 1, 1);
        formLayout.Controls.Add(new Label { Text = "Stock" }, 0, 2);
        formLayout.Controls.Add(_stock, 1, 2);
        formLayout.Controls.Add(new Label { Text = "Price" }, 0, 3);
        formLayout.Controls.Add(_price, 1, 3);
        formLayout.Controls.Add(_isActive, 1, 4);
        var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill };
        buttons.Controls.Add(_save);
        buttons.Controls.Add(_delete);
        formLayout.Controls.Add(buttons, 1, 5);

        _save.Click += async (_, _) => await SaveAsync();
        _delete.Click += async (_, _) => await DeleteAsync();

        Controls.Add(formLayout);
        Controls.Add(_grid);
        Controls.Add(_status);

        Shown += async (_, _) => await RefreshAsync();
    }

    private async Task RefreshAsync()
    {
        _status.Text = "Loading...";
        var items = await _itemService.GetAsync();
        _grid.DataSource = new BindingSource { DataSource = items };
        _status.Text = $"Loaded {items.Count} items";
    }

    private void LoadFromSelection()
    {
        if (_grid.CurrentRow?.DataBoundItem is not Item item)
        {
            return;
        }

        _currentId = item.ItemId;
        _name.Text = item.ItemName;
        _size.Text = item.Size;
        _stock.Value = item.UnitInStock;
        _price.Value = item.UnitPrice;
        _isActive.Checked = item.IsActive;
    }

    private async Task SaveAsync()
    {
        var item = new Item
        {
            ItemId = _currentId,
            ItemName = _name.Text.Trim(),
            Size = _size.Text.Trim(),
            UnitInStock = (int)_stock.Value,
            UnitPrice = _price.Value,
            IsActive = _isActive.Checked
        };

        await _itemService.SaveAsync(item);
        await RefreshAsync();
        _status.Text = "Saved";
    }

    private async Task DeleteAsync()
    {
        if (_currentId == 0)
        {
            _status.Text = "Select an item first";
            return;
        }

        if (MessageBox.Show("Delete selected item?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
        {
            return;
        }

        await _itemService.DeleteAsync(_currentId);
        _currentId = 0;
        await RefreshAsync();
    }
}
