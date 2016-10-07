// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmMakeEntrance
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using NHibernate;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmMakeEntrance : FrmBaseForm
  {
    private IContainer components = (IContainer) null;
    private ISession session;
    private Company company;
    private Home home;
    private Label lblFlats;
    private TextBox txbFirst;
    private TextBox txbLast;
    private Label label1;
    private Panel pnBtn;
    private Label lblEntrance;
    private TextBox txbEntrance;
    private Button btnExit;
    private Button btnMake;
    public HelpProvider hp;

    public FrmMakeEntrance()
    {
      this.InitializeComponent();
    }

    public FrmMakeEntrance(Company company, Home home)
    {
      this.InitializeComponent();
      this.company = company;
      this.home = home;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void txbFirst_KeyPress(object sender, KeyPressEventArgs e)
    {
      if ((int) e.KeyChar == 8 || (int) e.KeyChar == 13 || (int) e.KeyChar >= 48 && (int) e.KeyChar <= 57)
        return;
      e.Handled = true;
    }

    private void btnMake_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Внести номер подъезда в выбранных квартирах?", "", MessageBoxButtons.OK, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      if (this.txbFirst.Text == "" || this.txbLast.Text == "" || this.txbEntrance.Text == "")
      {
        int num1 = (int) MessageBox.Show("Не все данные внесены", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        int int32_1;
        int int32_2;
        short int16;
        try
        {
          int32_1 = Convert.ToInt32(this.txbFirst.Text);
          int32_2 = Convert.ToInt32(this.txbLast.Text);
          int16 = Convert.ToInt16(this.txbEntrance.Text);
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show("Данные внесены некорректно", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        if (int32_1 > int32_2)
        {
          int num3 = (int) MessageBox.Show("Номер первой квартры больше номер последней", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          int num2 = int32_1;
          try
          {
            for (; num2 != int32_2 + 1 && (uint) int32_2 > 0U; ++num2)
            {
              this.session.CreateQuery(string.Format("update LsClient ls set Entrance={0} where ls.Company.CompanyId={2} and ls.Home.IdHome={3} " + Options.MainConditions1 + " and ls.Flat.IdFlat in (select IdFlat from Flat where Home.IdHome=ls.Home.IdHome and (NFlat='{1}' or NFlat='{1}a' or NFlat='{1}а' or NFlat like '%{1}-%'))", (object) int16, (object) num2, (object) this.company.CompanyId, (object) this.home.IdHome)).ExecuteUpdate();
              this.session.CreateQuery(string.Format("update Flat f set Entrance={0} where f.CompanyId={2} and f.Home.IdHome={3} and (f.NFlat='{1}' or f.NFlat='{1}a' or f.NFlat like '%{1}-%') and f.IdFlat in (select Flat.IdFlat from LsClient ls where 1=1 " + Options.MainConditions1 + "))", (object) int16, (object) num2, (object) this.company.CompanyId, (object) this.home.IdHome)).ExecuteUpdate();
            }
            int num4 = (int) MessageBox.Show("Операция выполнена успешно");
          }
          catch
          {
            int num4 = (int) MessageBox.Show("Невозможно внести изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
      }
    }

    private void FrmMakeEntrance_Load(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblFlats = new Label();
      this.txbFirst = new TextBox();
      this.txbLast = new TextBox();
      this.label1 = new Label();
      this.pnBtn = new Panel();
      this.btnMake = new Button();
      this.btnExit = new Button();
      this.lblEntrance = new Label();
      this.txbEntrance = new TextBox();
      this.hp = new HelpProvider();
      this.pnBtn.SuspendLayout();
      this.SuspendLayout();
      this.lblFlats.AutoSize = true;
      this.lblFlats.Location = new Point(12, 18);
      this.lblFlats.Name = "lblFlats";
      this.lblFlats.Size = new Size(116, 16);
      this.lblFlats.TabIndex = 0;
      this.lblFlats.Text = "Номера квартир";
      this.txbFirst.Location = new Point(137, 15);
      this.txbFirst.Name = "txbFirst";
      this.txbFirst.Size = new Size(100, 22);
      this.txbFirst.TabIndex = 1;
      this.txbFirst.KeyPress += new KeyPressEventHandler(this.txbFirst_KeyPress);
      this.txbLast.Location = new Point(270, 15);
      this.txbLast.Name = "txbLast";
      this.txbLast.Size = new Size(100, 22);
      this.txbLast.TabIndex = 2;
      this.txbLast.KeyPress += new KeyPressEventHandler(this.txbFirst_KeyPress);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(247, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(15, 16);
      this.label1.TabIndex = 3;
      this.label1.Text = "_";
      this.pnBtn.Controls.Add((Control) this.btnMake);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 81);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(379, 40);
      this.pnBtn.TabIndex = 4;
      this.btnMake.Image = (Image) Resources.Configure;
      this.btnMake.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnMake.Location = new Point(9, 5);
      this.btnMake.Name = "btnMake";
      this.btnMake.Size = new Size(113, 30);
      this.btnMake.TabIndex = 0;
      this.btnMake.Text = "Выполнить";
      this.btnMake.TextAlign = ContentAlignment.MiddleRight;
      this.btnMake.UseVisualStyleBackColor = true;
      this.btnMake.Click += new EventHandler(this.btnMake_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(287, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(80, 30);
      this.btnExit.TabIndex = 1;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.lblEntrance.AutoSize = true;
      this.lblEntrance.Location = new Point(12, 50);
      this.lblEntrance.Name = "lblEntrance";
      this.lblEntrance.Size = new Size(119, 16);
      this.lblEntrance.TabIndex = 5;
      this.lblEntrance.Text = "Номер подъезда";
      this.txbEntrance.Location = new Point(137, 47);
      this.txbEntrance.Name = "txbEntrance";
      this.txbEntrance.Size = new Size(100, 22);
      this.txbEntrance.TabIndex = 3;
      this.txbEntrance.KeyPress += new KeyPressEventHandler(this.txbFirst_KeyPress);
      this.hp.HelpNamespace = "Help.chm";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(379, 121);
      this.Controls.Add((Control) this.txbEntrance);
      this.Controls.Add((Control) this.lblEntrance);
      this.Controls.Add((Control) this.pnBtn);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txbLast);
      this.Controls.Add((Control) this.txbFirst);
      this.Controls.Add((Control) this.lblFlats);
      this.hp.SetHelpKeyword((Control) this, "kv142.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      this.Name = "FrmMakeEntrance";
      this.hp.SetShowHelp((Control) this, true);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Проставить подъезды";
      this.Load += new EventHandler(this.FrmMakeEntrance_Load);
      this.pnBtn.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
