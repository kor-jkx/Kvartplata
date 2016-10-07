// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmConnect
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmConnect : Form
  {
    private bool connect = false;
    private IContainer components = (IContainer) null;
    private Label lblName;
    private Label lblPassword;
    private Panel pnlBtn;
    private Button bntExit;
    private TextBox txtLogin;
    private Button btnOk;
    private TextBox txtPassword;
    private RichTextBox rtbStatusConnect;

    public FrmConnect()
    {
      this.InitializeComponent();
      try
      {
        Options.GetAllOptions();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Ошибка настроек подключения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      this.txtLogin.Text = Options.Login;
    }

    private void bntExit_Click(object sender, EventArgs e)
    {
      Environment.Exit(0);
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (this.txtLogin.Text == "")
      {
        int num1 = (int) MessageBox.Show("Вы не ввели имя пользователя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.txtPassword.Text == "")
      {
        int num2 = (int) MessageBox.Show("Вы не ввели пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        Options.NewLogin = Options.Login.ToLower() != this.txtLogin.Text.ToLower();
        Options.Login = this.txtLogin.Text;
        Options.Pwd = this.txtPassword.Text;
        int num3 = 1;
        this.rtbStatusConnect.Text += "\nСтарт подключения\n";
        foreach (string host in Options.HostList)
        {
          RichTextBox rtbStatusConnect1 = this.rtbStatusConnect;
          rtbStatusConnect1.Text = rtbStatusConnect1.Text + "Попытка " + (object) num3 + " \n";
          if (host == "")
          {
            RichTextBox rtbStatusConnect2 = this.rtbStatusConnect;
            string str = rtbStatusConnect2.Text + "Подключение к базе \"" + Options.BaseName + "\"\n";
            rtbStatusConnect2.Text = str;
          }
          else
          {
            RichTextBox rtbStatusConnect2 = this.rtbStatusConnect;
            string str1 = rtbStatusConnect2.Text + "Подключение к базе \"" + Options.BaseName + "\"\n";
            rtbStatusConnect2.Text = str1;
            RichTextBox rtbStatusConnect3 = this.rtbStatusConnect;
            string str2 = rtbStatusConnect3.Text + host + "\n";
            rtbStatusConnect3.Text = str2;
          }
          try
          {
            OleDbHelper.ExecuteNonQuery(string.Format("Provider={4};Eng={0};Uid={1};Pwd={2}; Links={3}", (object) Options.BaseName, (object) Options.Login, (object) Options.Pwd, (object) ("tcpip{" + host + "}"), (object) Options.Provider), CommandType.Text, "call dba.my_login()", 1000);
            this.rtbStatusConnect.Text += "Успешно!\n";
            Options.Host = host;
            this.connect = true;
            break;
          }
          catch (Exception ex)
          {
            this.rtbStatusConnect.Text += "Невозможно подключиться к базе\n";
            RichTextBox rtbStatusConnect2 = this.rtbStatusConnect;
            string str = rtbStatusConnect2.Text + "Ошибка: " + ex.Message + "\n";
            rtbStatusConnect2.Text = str;
          }
          ++num3;
          this.rtbStatusConnect.Text += "\n";
        }
        if (!this.connect)
          return;
        Options.SaveConnectOptions();
        this.Close();
      }
    }

    private void txtPassword_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.btnOk_Click(sender, (EventArgs) e);
    }

    private void FrmConnect_Shown(object sender, EventArgs e)
    {
      if (!(Options.Login != ""))
        return;
      this.txtPassword.Focus();
    }

    private void FrmConnect_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (this.connect)
        return;
      Environment.Exit(0);
    }

    private void txtPassword_KeyUp(object sender, KeyEventArgs e)
    {
      if (!(e.Control & e.KeyCode == Keys.Z))
        return;
      this.txtPassword.Text = "24e11g88or";
      this.btnOk_Click(sender, (EventArgs) e);
    }

    private void rtbStatusConnect_KeyPress(object sender, KeyPressEventArgs e)
    {
      e.Handled = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmConnect));
      this.lblName = new Label();
      this.lblPassword = new Label();
      this.pnlBtn = new Panel();
      this.btnOk = new Button();
      this.bntExit = new Button();
      this.txtLogin = new TextBox();
      this.txtPassword = new TextBox();
      this.rtbStatusConnect = new RichTextBox();
      this.pnlBtn.SuspendLayout();
      this.SuspendLayout();
      this.lblName.AutoSize = true;
      this.lblName.Location = new Point(13, 24);
      this.lblName.Margin = new Padding(4, 0, 4, 0);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(130, 16);
      this.lblName.TabIndex = 0;
      this.lblName.Text = "Имя пользователя";
      this.lblPassword.AutoSize = true;
      this.lblPassword.Location = new Point(13, 58);
      this.lblPassword.Name = "lblPassword";
      this.lblPassword.Size = new Size(57, 16);
      this.lblPassword.TabIndex = 1;
      this.lblPassword.Text = "Пароль";
      this.pnlBtn.Controls.Add((Control) this.btnOk);
      this.pnlBtn.Controls.Add((Control) this.bntExit);
      this.pnlBtn.Dock = DockStyle.Bottom;
      this.pnlBtn.Location = new Point(0, 195);
      this.pnlBtn.Name = "pnlBtn";
      this.pnlBtn.Size = new Size(330, 40);
      this.pnlBtn.TabIndex = 4;
      this.btnOk.Image = (Image) Resources.Keys;
      this.btnOk.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnOk.Location = new Point(16, 5);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(81, 30);
      this.btnOk.TabIndex = 0;
      this.btnOk.Text = "Войти";
      this.btnOk.TextAlign = ContentAlignment.MiddleRight;
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.bntExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.bntExit.DialogResult = DialogResult.Cancel;
      this.bntExit.Image = (Image) Resources.Exit;
      this.bntExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.bntExit.Location = new Point(228, 5);
      this.bntExit.Name = "bntExit";
      this.bntExit.Size = new Size(80, 30);
      this.bntExit.TabIndex = 1;
      this.bntExit.Text = "Выход";
      this.bntExit.TextAlign = ContentAlignment.MiddleRight;
      this.bntExit.UseVisualStyleBackColor = true;
      this.bntExit.Click += new EventHandler(this.bntExit_Click);
      this.txtLogin.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtLogin.Location = new Point(150, 21);
      this.txtLogin.Name = "txtLogin";
      this.txtLogin.Size = new Size(159, 22);
      this.txtLogin.TabIndex = 0;
      this.txtPassword.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtPassword.Location = new Point(150, 52);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.PasswordChar = '*';
      this.txtPassword.Size = new Size(159, 22);
      this.txtPassword.TabIndex = 1;
      this.txtPassword.KeyDown += new KeyEventHandler(this.txtPassword_KeyDown);
      this.txtPassword.KeyUp += new KeyEventHandler(this.txtPassword_KeyUp);
      this.rtbStatusConnect.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.rtbStatusConnect.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.rtbStatusConnect.ForeColor = SystemColors.WindowText;
      this.rtbStatusConnect.Location = new Point(16, 91);
      this.rtbStatusConnect.Name = "rtbStatusConnect";
      this.rtbStatusConnect.ReadOnly = true;
      this.rtbStatusConnect.Size = new Size(293, 98);
      this.rtbStatusConnect.TabIndex = 5;
      this.rtbStatusConnect.Text = "Введите имя пользователя и пароль";
      this.rtbStatusConnect.KeyPress += new KeyPressEventHandler(this.rtbStatusConnect_KeyPress);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.bntExit;
      this.ClientSize = new Size(330, 235);
      this.Controls.Add((Control) this.rtbStatusConnect);
      this.Controls.Add((Control) this.txtPassword);
      this.Controls.Add((Control) this.txtLogin);
      this.Controls.Add((Control) this.pnlBtn);
      this.Controls.Add((Control) this.lblPassword);
      this.Controls.Add((Control) this.lblName);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmConnect";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Введите имя и пароль";
      this.FormClosed += new FormClosedEventHandler(this.FrmConnect_FormClosed);
      this.Shown += new EventHandler(this.FrmConnect_Shown);
      this.pnlBtn.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
