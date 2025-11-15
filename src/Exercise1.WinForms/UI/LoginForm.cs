using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using PE2.ECommerce.Services.Interfaces;

namespace Exercise1.WinForms.UI;

public class LoginForm : Form
{
    private readonly IServiceProvider _services;
    private readonly IAuthService _authService;

    private readonly TextBox _userName = new() { PlaceholderText = "Username" };
    private readonly TextBox _password = new() { PlaceholderText = "Password", UseSystemPasswordChar = true };
    private readonly Button _loginButton = new() { Text = "Login", Dock = DockStyle.Top, Height = 40 };
    private readonly Label _status = new() { Dock = DockStyle.Top, ForeColor = Color.DarkRed, Height = 30, TextAlign = ContentAlignment.MiddleCenter };

    public LoginForm(IServiceProvider services, IAuthService authService)
    {
        _services = services;
        _authService = authService;
        Text = "PE2 E-Commerce - Login";
        Size = new Size(360, 240);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 5,
            ColumnCount = 1,
            Padding = new Padding(20)
        };

        _userName.Dock = DockStyle.Top;
        _password.Dock = DockStyle.Top;
        _loginButton.Click += async (_, _) => await HandleLoginAsync();

        panel.Controls.Add(new Label { Text = "Enter your credentials", Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleCenter, Height = 30 });
        panel.Controls.Add(_userName);
        panel.Controls.Add(_password);
        panel.Controls.Add(_loginButton);
        panel.Controls.Add(_status);

        Controls.Add(panel);
    }

    private async Task HandleLoginAsync()
    {
        _status.Text = string.Empty;
        _loginButton.Enabled = false;
        try
        {
            var userName = _userName.Text.Trim();
            var password = _password.Text;
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                _status.Text = "Username and password are required";
                return;
            }

            var result = await _authService.LoginAsync(userName, password);
            if (!result.Success || result.UserId is null)
            {
                _status.Text = result.Error ?? "Unable to login";
                return;
            }

            var mainForm = _services.GetRequiredService<MainForm>();
            mainForm.SetUserContext(result.UserId.Value, userName);
            Hide();
            mainForm.FormClosed += (_, _) => Close();
            mainForm.Show();
        }
        catch (Exception ex)
        {
            _status.Text = ex.Message;
        }
        finally
        {
            _loginButton.Enabled = true;
        }
    }
}
