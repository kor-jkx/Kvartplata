// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmSelectPeriod
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmSelectPeriod : Form
  {
    private IContainer components = (IContainer) null;
    public Period period;
    private ISession session;
    private DateTimePicker dtmpPeriod;
    private Panel pnBtn;
    private Button btnCancel;
    private Button btnOK;

    public FrmSelectPeriod()
    {
      this.InitializeComponent();
      this.dtmpPeriod.Value = Options.Period.PeriodName.Value.AddMonths(-1);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      IList<Period> periodList = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) this.dtmpPeriod.Value)).List<Period>();
      if ((uint) periodList.Count > 0U)
        this.period = periodList[0];
      this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.period = (Period) null;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmSelectPeriod));
      this.dtmpPeriod = new DateTimePicker();
      this.pnBtn = new Panel();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.pnBtn.SuspendLayout();
      this.SuspendLayout();
      this.dtmpPeriod.CustomFormat = "MMMM yyyy";
      this.dtmpPeriod.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.dtmpPeriod.Format = DateTimePickerFormat.Custom;
      this.dtmpPeriod.Location = new Point(13, 13);
      this.dtmpPeriod.Margin = new Padding(4);
      this.dtmpPeriod.Name = "dtmpPeriod";
      this.dtmpPeriod.ShowUpDown = true;
      this.dtmpPeriod.Size = new Size(161, 26);
      this.dtmpPeriod.TabIndex = 0;
      this.pnBtn.Controls.Add((Control) this.btnCancel);
      this.pnBtn.Controls.Add((Control) this.btnOK);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 51);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(252, 40);
      this.pnBtn.TabIndex = 1;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Image = (Image) Resources.delete;
      this.btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCancel.Location = new Point(153, 5);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(87, 30);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Отмена";
      this.btnCancel.TextAlign = ContentAlignment.MiddleRight;
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOK.Image = (Image) Resources.Tick;
      this.btnOK.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnOK.Location = new Point(12, 5);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(58, 30);
      this.btnOK.TabIndex = 0;
      this.btnOK.Text = "ОК";
      this.btnOK.TextAlign = ContentAlignment.MiddleRight;
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(252, 91);
      this.Controls.Add((Control) this.pnBtn);
      this.Controls.Add((Control) this.dtmpPeriod);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmSelectPeriod";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Выбор месяца";
      this.pnBtn.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
