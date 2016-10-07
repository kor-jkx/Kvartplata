// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.FrmCheck
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Forms;
using Kvartplata.Properties;
using NHibernate;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Classes
{
  public class FrmCheck : FrmBaseForm
  {
    private IContainer components = (IContainer) null;
    private Home home;
    private Company company;
    private LsClient client;
    private ISession session;
    private Panel pnBtn;
    private Button btnCancel;
    private Button btnOK;
    private ComboBox cmbChoise;
    private DateTimePicker dtmDate;
    private Label lblChoise;
    private Label lblDate;

    public FrmCheck()
    {
      this.InitializeComponent();
    }

    public FrmCheck(Company company, Home home)
    {
      this.InitializeComponent();
      this.home = home;
      this.company = company;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void FrmChoise_Shown(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      this.dtmDate.MinDate = KvrplHelper.GetCmpKvrClose(this.company, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk).PeriodName.Value.AddMonths(1);
      this.cmbChoise.DataSource = (object) this.session.CreateQuery(string.Format("select ls from LsClient ls left join fetch ls.Flat where ls.Company.CompanyId={2} and ls.Home.IdHome={0} and isnull((select p.ParamValue from ClientParam p where p.ClientId=ls.ClientId and p.Param.ParamId=107 and p.DBeg<='{1}' and p.DEnd>'{1}'),0) not in (4,5,6,7) " + Options.MainConditions1 + " order by ls.Complex.IdFk,lengthhome(ls.Flat.NFlat)", (object) this.home.IdHome, (object) KvrplHelper.DateToBaseFormat(DateTime.Now), (object) this.company.CompanyId)).List<LsClient>();
      this.cmbChoise.DisplayMember = "SmallAddress";
      this.cmbChoise.ValueMember = "ClientId";
    }

    public LsClient ReturnClient()
    {
      if (this.client != null)
        return this.client;
      return (LsClient) null;
    }

    public DateTime ReturnDate()
    {
      return this.dtmDate.Value;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.client = (LsClient) this.cmbChoise.SelectedItem;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnBtn = new Panel();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.cmbChoise = new ComboBox();
      this.dtmDate = new DateTimePicker();
      this.lblChoise = new Label();
      this.lblDate = new Label();
      this.pnBtn.SuspendLayout();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnCancel);
      this.pnBtn.Controls.Add((Control) this.btnOK);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 94);
      this.pnBtn.Margin = new Padding(4);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(308, 40);
      this.pnBtn.TabIndex = 0;
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Image = (Image) Resources.delete;
      this.btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCancel.Location = new Point(200, 5);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(96, 30);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Отмена";
      this.btnCancel.TextAlign = ContentAlignment.MiddleRight;
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOK.Image = (Image) Resources.Tick;
      this.btnOK.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnOK.Location = new Point(12, 5);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(60, 30);
      this.btnOK.TabIndex = 0;
      this.btnOK.Text = "ОК";
      this.btnOK.TextAlign = ContentAlignment.MiddleRight;
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.cmbChoise.FormattingEnabled = true;
      this.cmbChoise.Location = new Point(80, 23);
      this.cmbChoise.Name = "cmbChoise";
      this.cmbChoise.Size = new Size(189, 24);
      this.cmbChoise.TabIndex = 1;
      this.dtmDate.Location = new Point(80, 65);
      this.dtmDate.Name = "dtmDate";
      this.dtmDate.Size = new Size(189, 22);
      this.dtmDate.TabIndex = 2;
      this.lblChoise.AutoSize = true;
      this.lblChoise.Location = new Point(9, 26);
      this.lblChoise.Name = "lblChoise";
      this.lblChoise.Size = new Size(65, 16);
      this.lblChoise.TabIndex = 3;
      this.lblChoise.Text = "Лицевой";
      this.lblDate.AutoSize = true;
      this.lblDate.Location = new Point(12, 65);
      this.lblDate.Name = "lblDate";
      this.lblDate.Size = new Size(40, 16);
      this.lblDate.TabIndex = 4;
      this.lblDate.Text = "Дата";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(308, 134);
      this.Controls.Add((Control) this.lblDate);
      this.Controls.Add((Control) this.lblChoise);
      this.Controls.Add((Control) this.dtmDate);
      this.Controls.Add((Control) this.cmbChoise);
      this.Controls.Add((Control) this.pnBtn);
      this.Margin = new Padding(5);
      this.Name = "FrmCheck";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Выбор";
      this.Shown += new EventHandler(this.FrmChoise_Shown);
      this.pnBtn.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
