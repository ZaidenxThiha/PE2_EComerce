using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using PE2.ECommerce.Domain.Entities;
using PE2.ECommerce.Services.Interfaces;

namespace Exercise1.WinForms.UI;

public class AgentManagementForm : Form
{
    private readonly IAgentService _agentService;
    private readonly DataGridView _grid = new() { Dock = DockStyle.Fill, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
    private readonly TextBox _name = new() { PlaceholderText = "Agent name" };
    private readonly TextBox _address = new() { PlaceholderText = "Address" };
    private readonly TextBox _phone = new() { PlaceholderText = "Phone" };
    private readonly TextBox _email = new() { PlaceholderText = "Email" };
    private readonly Button _save = new() { Text = "Save" };
    private int _id;

    public AgentManagementForm(IAgentService agentService)
    {
        _agentService = agentService;
        Text = "Agents";
        Width = 720;
        Height = 520;

        _grid.AutoGenerateColumns = false;
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Agent.AgentName), HeaderText = "Name", Width = 220 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Agent.Address), HeaderText = "Address", Width = 200 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Agent.Phone), HeaderText = "Phone" });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Agent.Email), HeaderText = "Email", Width = 180 });
        _grid.SelectionChanged += (_, _) => LoadFromSelection();

        var formLayout = new TableLayoutPanel { Dock = DockStyle.Right, Width = 300, ColumnCount = 1, RowCount = 5, Padding = new Padding(10) };
        formLayout.Controls.Add(_name);
        formLayout.Controls.Add(_address);
        formLayout.Controls.Add(_phone);
        formLayout.Controls.Add(_email);
        formLayout.Controls.Add(_save);

        _save.Click += async (_, _) => await SaveAsync();

        Controls.Add(_grid);
        Controls.Add(formLayout);

        Shown += async (_, _) => await RefreshAsync();
    }

    private async Task RefreshAsync()
    {
        var agents = await _agentService.GetAsync();
        _grid.DataSource = agents;
    }

    private void LoadFromSelection()
    {
        if (_grid.CurrentRow?.DataBoundItem is not Agent agent)
        {
            return;
        }

        _id = agent.AgentId;
        _name.Text = agent.AgentName;
        _address.Text = agent.Address;
        _phone.Text = agent.Phone;
        _email.Text = agent.Email;
    }

    private async Task SaveAsync()
    {
        var agent = new Agent
        {
            AgentId = _id,
            AgentName = _name.Text.Trim(),
            Address = _address.Text.Trim(),
            Phone = _phone.Text.Trim(),
            Email = _email.Text.Trim()
        };

        await _agentService.SaveAsync(agent);
        _id = 0;
        await RefreshAsync();
    }
}
