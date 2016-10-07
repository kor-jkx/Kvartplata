// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmCurrentPayment
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmCurrentPayment : Form
  {
    private static IContainer ic = (IContainer) null;
    private FormStateSaver fss = new FormStateSaver(FrmCurrentPayment.ic);
    private IContainer components = (IContainer) null;
    private ISession session;
    public bool IsOpen;
    private Panel pnButtons;
    private Button btnExit;
    private DataGridView dgvCurrentPayment;

    public LsClient lsClient { get; set; }

    public Period period { get; set; }

    public FrmAttribute PForm { get; set; }

    public FrmCurrentPayment()
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
      this.IsOpen = false;
      this.TopMost = true;
    }

    public void LoadCurrentPayment()
    {
      this.session = Domain.CurrentSession;
      this.dgvCurrentPayment.DataSource = (object) null;
      this.dgvCurrentPayment.Columns.Clear();
      this.dgvCurrentPayment.DataSource = (object) this.session.CreateCriteria(typeof (Payment)).Add((ICriterion) Restrictions.Eq("LsClient", (object) this.lsClient)).Add((ICriterion) Restrictions.Eq("Period", (object) this.period)).List<Payment>();
      this.dgvCurrentPayment.Columns[0].HeaderText = "Лицевой";
      this.dgvCurrentPayment.Columns[3].HeaderText = "Источник";
      this.dgvCurrentPayment.Columns[4].HeaderText = "Назначение";
      this.dgvCurrentPayment.Columns[5].HeaderText = "Дата оплаты";
      this.dgvCurrentPayment.Columns[6].HeaderText = "Номер пачки";
      this.dgvCurrentPayment.Columns["PaymentValue"].HeaderText = "Сумма оплаты";
      this.dgvCurrentPayment.Columns[8].HeaderText = "Пени";
      this.dgvCurrentPayment.Columns["ServiceName"].HeaderText = "Услуга";
      this.dgvCurrentPayment.Columns[1].Visible = false;
      this.dgvCurrentPayment.Columns[2].Visible = false;
      this.dgvCurrentPayment.Columns["UName"].Visible = false;
      this.dgvCurrentPayment.Columns["Dedit"].Visible = false;
      this.dgvCurrentPayment.Columns["SupplierName"].Visible = false;
      this.dgvCurrentPayment.Columns["ReceiptName"].Visible = false;
      this.IsOpen = true;
      if (this.PForm == null)
        return;
      this.PForm.Focus();
    }

    public void Clear()
    {
      this.dgvCurrentPayment.DataSource = (object) null;
      this.dgvCurrentPayment.Refresh();
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void FrmCurrentPayment_Shown(object sender, EventArgs e)
    {
      this.LoadCurrentPayment();
    }

    private void dgvCurrentPayment_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    private void FrmCurrentPayment_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.IsOpen = false;
      this.Clear();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmCurrentPayment));
      this.pnButtons = new Panel();
      this.btnExit = new Button();
      this.dgvCurrentPayment = new DataGridView();
      this.pnButtons.SuspendLayout();
      ((ISupportInitialize) this.dgvCurrentPayment).BeginInit();
      this.SuspendLayout();
      this.pnButtons.Controls.Add((Control) this.btnExit);
      this.pnButtons.Dock = DockStyle.Bottom;
      this.pnButtons.Location = new Point(0, 149);
      this.pnButtons.Margin = new Padding(4);
      this.pnButtons.Name = "pnButtons";
      this.pnButtons.Size = new Size(581, 41);
      this.pnButtons.TabIndex = 0;
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(498, 3);
      this.btnExit.Margin = new Padding(4);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(79, 34);
      this.btnExit.TabIndex = 2;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.dgvCurrentPayment.BackgroundColor = Color.AliceBlue;
      this.dgvCurrentPayment.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvCurrentPayment.Dock = DockStyle.Fill;
      this.dgvCurrentPayment.Location = new Point(0, 0);
      this.dgvCurrentPayment.Name = "dgvCurrentPayment";
      this.dgvCurrentPayment.Size = new Size(581, 149);
      this.dgvCurrentPayment.TabIndex = 1;
      this.dgvCurrentPayment.DataError += new DataGridViewDataErrorEventHandler(this.dgvCurrentPayment_DataError);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(581, 190);
      this.Controls.Add((Control) this.dgvCurrentPayment);
      this.Controls.Add((Control) this.pnButtons);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmCurrentPayment";
      this.Text = "Текущие платежи";
      this.FormClosed += new FormClosedEventHandler(this.FrmCurrentPayment_FormClosed);
      this.Shown += new EventHandler(this.FrmCurrentPayment_Shown);
      this.pnButtons.ResumeLayout(false);
      ((ISupportInitialize) this.dgvCurrentPayment).EndInit();
      this.ResumeLayout(false);
    }
  }
}
