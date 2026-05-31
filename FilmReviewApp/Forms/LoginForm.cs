using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FilmReviewApp.Forms
{
    public class LoginForm : Form
    {
        private bool _isAdmin;
        private bool _loginSuccessful;

        private const string AdminUsername = "admin";
        private const string AdminPassword = "admin123";

        private Panel _mainPanel;
        private Label _titleLabel;
        private Label _subtitleLabel;
        private Button _btnAdminLogin;
        private Button _btnUserContinue;

        private Panel _adminPanel;
        private Label _lblUsername;
        private TextBox _txtUsername;
        private Label _lblPassword;
        private TextBox _txtPassword;
        private Button _btnLogin;
        private Button _btnBack;
        private Label _lblError;

        public bool IsAdmin => _isAdmin;

        public bool LoginSuccessful => _loginSuccessful;

        public LoginForm()
        {
            _isAdmin = false;
            _loginSuccessful = false;
            SetupForm();
            SetupControls();
        }

        private void SetupForm()
        {
            Text = "🎬 Film Değerlendirme Uygulaması - Giriş";
            Size = new Size(520, 480);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.FromArgb(25, 25, 35);
        }

        private void SetupControls()
        {
            _mainPanel = new Panel();
            _mainPanel.Size = new Size(440, 400);
            _mainPanel.Location = new Point(32, 20);
            _mainPanel.BackColor = Color.FromArgb(35, 35, 50);
            _mainPanel.Paint += MainPanel_Paint;
            Controls.Add(_mainPanel);

            Label iconLabel = new Label();
            iconLabel.Text = "🎬";
            iconLabel.Font = new Font("Segoe UI Emoji", 36);
            iconLabel.TextAlign = ContentAlignment.MiddleCenter;
            iconLabel.Size = new Size(440, 70);
            iconLabel.Location = new Point(0, 15);
            iconLabel.BackColor = Color.Transparent;
            _mainPanel.Controls.Add(iconLabel);

            _titleLabel = new Label();
            _titleLabel.Text = "Film Değerlendirme Uygulaması";
            _titleLabel.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            _titleLabel.ForeColor = Color.Gold;
            _titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            _titleLabel.Size = new Size(440, 35);
            _titleLabel.Location = new Point(0, 85);
            _titleLabel.BackColor = Color.Transparent;
            _mainPanel.Controls.Add(_titleLabel);

            _subtitleLabel = new Label();
            _subtitleLabel.Text = "Devam etmek için giriş yönteminizi seçin";
            _subtitleLabel.Font = new Font("Segoe UI", 10);
            _subtitleLabel.ForeColor = Color.Silver;
            _subtitleLabel.TextAlign = ContentAlignment.MiddleCenter;
            _subtitleLabel.Size = new Size(440, 25);
            _subtitleLabel.Location = new Point(0, 120);
            _subtitleLabel.BackColor = Color.Transparent;
            _mainPanel.Controls.Add(_subtitleLabel);

            Panel separator = new Panel();
            separator.Size = new Size(360, 1);
            separator.Location = new Point(40, 155);
            separator.BackColor = Color.FromArgb(60, 60, 80);
            _mainPanel.Controls.Add(separator);

            _btnAdminLogin = CreateStyledButton("🔐  Admin Girişi", Color.FromArgb(220, 20, 60));
            _btnAdminLogin.Location = new Point(70, 175);
            _btnAdminLogin.Size = new Size(300, 50);
            _btnAdminLogin.Click += BtnAdminLogin_Click;
            _mainPanel.Controls.Add(_btnAdminLogin);

            _btnUserContinue = CreateStyledButton("👤  Kullanıcı Olarak Devam Et", Color.FromArgb(50, 120, 200));
            _btnUserContinue.Location = new Point(70, 240);
            _btnUserContinue.Size = new Size(300, 50);
            _btnUserContinue.Click += BtnUserContinue_Click;
            _mainPanel.Controls.Add(_btnUserContinue);

            Label infoLabel = new Label();
            infoLabel.Text = "Admin: Film ekle/sil  |  Kullanıcı: Film izle ve yorum yap";
            infoLabel.Font = new Font("Segoe UI", 8);
            infoLabel.ForeColor = Color.FromArgb(120, 120, 140);
            infoLabel.TextAlign = ContentAlignment.MiddleCenter;
            infoLabel.Size = new Size(440, 20);
            infoLabel.Location = new Point(0, 310);
            infoLabel.BackColor = Color.Transparent;
            _mainPanel.Controls.Add(infoLabel);

            _adminPanel = new Panel();
            _adminPanel.Size = new Size(440, 400);
            _adminPanel.Location = new Point(32, 20);
            _adminPanel.BackColor = Color.FromArgb(35, 35, 50);
            _adminPanel.Visible = false;
            _adminPanel.Paint += MainPanel_Paint;
            Controls.Add(_adminPanel);

            Label adminIcon = new Label();
            adminIcon.Text = "🔐";
            adminIcon.Font = new Font("Segoe UI Emoji", 30);
            adminIcon.TextAlign = ContentAlignment.MiddleCenter;
            adminIcon.Size = new Size(440, 55);
            adminIcon.Location = new Point(0, 15);
            adminIcon.BackColor = Color.Transparent;
            _adminPanel.Controls.Add(adminIcon);

            Label adminTitle = new Label();
            adminTitle.Text = "Admin Girişi";
            adminTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            adminTitle.ForeColor = Color.Gold;
            adminTitle.TextAlign = ContentAlignment.MiddleCenter;
            adminTitle.Size = new Size(440, 30);
            adminTitle.Location = new Point(0, 70);
            adminTitle.BackColor = Color.Transparent;
            _adminPanel.Controls.Add(adminTitle);

            Panel separator2 = new Panel();
            separator2.Size = new Size(360, 1);
            separator2.Location = new Point(40, 110);
            separator2.BackColor = Color.FromArgb(60, 60, 80);
            _adminPanel.Controls.Add(separator2);

            Label credentialHint = new Label();
            credentialHint.Text = "Kullanıcı Adı: admin  |  Şifre: admin123";
            credentialHint.Font = new Font("Segoe UI", 9, FontStyle.Italic);
            credentialHint.ForeColor = Color.FromArgb(140, 140, 160);
            credentialHint.TextAlign = ContentAlignment.MiddleCenter;
            credentialHint.Size = new Size(440, 20);
            credentialHint.Location = new Point(0, 113);
            credentialHint.BackColor = Color.Transparent;
            _adminPanel.Controls.Add(credentialHint);

            _lblUsername = new Label();
            _lblUsername.Text = "Kullanıcı Adı:";
            _lblUsername.Font = new Font("Segoe UI", 10);
            _lblUsername.ForeColor = Color.White;
            _lblUsername.Location = new Point(70, 125);
            _lblUsername.Size = new Size(300, 22);
            _lblUsername.BackColor = Color.Transparent;
            _adminPanel.Controls.Add(_lblUsername);

            _txtUsername = new TextBox();
            _txtUsername.Font = new Font("Segoe UI", 11);
            _txtUsername.BackColor = Color.FromArgb(45, 45, 65);
            _txtUsername.ForeColor = Color.White;
            _txtUsername.BorderStyle = BorderStyle.FixedSingle;
            _txtUsername.Location = new Point(70, 148);
            _txtUsername.Size = new Size(300, 28);
            _adminPanel.Controls.Add(_txtUsername);

            _lblPassword = new Label();
            _lblPassword.Text = "Şifre:";
            _lblPassword.Font = new Font("Segoe UI", 10);
            _lblPassword.ForeColor = Color.White;
            _lblPassword.Location = new Point(70, 185);
            _lblPassword.Size = new Size(300, 22);
            _lblPassword.BackColor = Color.Transparent;
            _adminPanel.Controls.Add(_lblPassword);

            _txtPassword = new TextBox();
            _txtPassword.Font = new Font("Segoe UI", 11);
            _txtPassword.BackColor = Color.FromArgb(45, 45, 65);
            _txtPassword.ForeColor = Color.White;
            _txtPassword.BorderStyle = BorderStyle.FixedSingle;
            _txtPassword.UseSystemPasswordChar = true;
            _txtPassword.Location = new Point(70, 208);
            _txtPassword.Size = new Size(300, 28);
            _txtPassword.KeyDown += TxtPassword_KeyDown;
            _adminPanel.Controls.Add(_txtPassword);

            _lblError = new Label();
            _lblError.Text = "";
            _lblError.Font = new Font("Segoe UI", 9);
            _lblError.ForeColor = Color.FromArgb(255, 80, 80);
            _lblError.TextAlign = ContentAlignment.MiddleCenter;
            _lblError.Size = new Size(300, 20);
            _lblError.Location = new Point(70, 242);
            _lblError.BackColor = Color.Transparent;
            _lblError.Visible = false;
            _adminPanel.Controls.Add(_lblError);

            _btnLogin = CreateStyledButton("Giriş Yap", Color.FromArgb(34, 139, 34));
            _btnLogin.Location = new Point(70, 270);
            _btnLogin.Size = new Size(300, 45);
            _btnLogin.Click += BtnLogin_Click;
            _adminPanel.Controls.Add(_btnLogin);

            _btnBack = CreateStyledButton("← Geri Dön", Color.FromArgb(80, 80, 100));
            _btnBack.Location = new Point(70, 325);
            _btnBack.Size = new Size(300, 40);
            _btnBack.Click += BtnBack_Click;
            _adminPanel.Controls.Add(_btnBack);
        }

        private Button CreateStyledButton(string text, Color baseColor)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btn.ForeColor = Color.White;
            btn.BackColor = baseColor;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;

            Color hoverColor = ControlPaint.Light(baseColor, 0.15f);
            btn.MouseEnter += (s, e) => btn.BackColor = hoverColor;
            btn.MouseLeave += (s, e) => btn.BackColor = baseColor;

            return btn;
        }

        private void MainPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel == null) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (Pen pen = new Pen(Color.FromArgb(60, 60, 80), 1))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
            }
        }

        private void BtnAdminLogin_Click(object sender, EventArgs e)
        {
            _mainPanel.Visible = false;
            _adminPanel.Visible = true;
            _txtUsername.Clear();
            _txtPassword.Clear();
            _lblError.Visible = false;
            _txtUsername.Focus();
        }

        private void BtnUserContinue_Click(object sender, EventArgs e)
        {
            _isAdmin = false;
            _loginSuccessful = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = _txtUsername.Text.Trim();
            string password = _txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Lütfen kullanıcı adı ve şifre girin.");
                return;
            }

            if (username == AdminUsername && password == AdminPassword)
            {
                _isAdmin = true;
                _loginSuccessful = true;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                ShowError("Kullanıcı adı veya şifre hatalı!");
                _txtPassword.Clear();
                _txtPassword.Focus();
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            _adminPanel.Visible = false;
            _mainPanel.Visible = true;
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                BtnLogin_Click(sender, e);
            }
        }

        private void ShowError(string message)
        {
            _lblError.Text = message;
            _lblError.Visible = true;
        }
    }
}
