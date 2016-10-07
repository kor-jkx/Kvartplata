// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmList
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmList : Form
  {
    private IContainer components = (IContainer) null;
    private IList<Payment> listPayment;
    public Payment payment;
    private Panel panel1;
    private Button btnOk;
    private Button btnExit;
    private DataGridView dgvList;

    public FrmList()
    {
      this.InitializeComponent();
    }

    public FrmList(IList<Payment> listPayment)
    {
      this.listPayment = listPayment;
      this.InitializeComponent();
    }

    private void FrmList_Shown(object sender, EventArgs e)
    {
      this.dgvList.DataSource = (object) this.listPayment;
      this.dgvList.Columns["ClientId"].HeaderText = "Лицевой счет";
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvList.Columns)
      {
        if ((uint) column.Index > 0U)
          column.Visible = false;
      }
      this.dgvList.Focus();
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (this.dgvList.CurrentRow == null)
        return;
      this.payment = (Payment) this.dgvList.CurrentRow.DataBoundItem;
      this.Close();
    }

    private void dgvList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      this.btnOk_Click(sender, (EventArgs) e);
    }

    private void dgvList_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.btnOk_Click(sender, (EventArgs) e);
    }

    private void dgvList_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmList));
      this.panel1 = new Panel();
      this.btnOk = new Button();
      this.btnExit = new Button();
      this.dgvList = new DataGridView();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.dgvList).BeginInit();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.btnOk);
      this.panel1.Controls.Add((Control) this.btnExit);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 165);
      this.panel1.Margin = new Padding(4);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(417, 40);
      this.panel1.TabIndex = 0;
      this.btnOk.Image = (Image) Resources.Tick;
      this.btnOk.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnOk.Location = new Point(12, 5);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(64, 30);
      this.btnOk.TabIndex = 3;
      this.btnOk.Text = "ОК";
      this.btnOk.TextAlign = ContentAlignment.MiddleRight;
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.delete;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(315, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(90, 30);
      this.btnExit.TabIndex = 2;
      this.btnExit.Text = "Отмена";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.dgvList.BackgroundColor = Color.AliceBlue;
      this.dgvList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvList.Dock = DockStyle.Fill;
      this.dgvList.Location = new Point(0, 0);
      this.dgvList.Name = "dgvList";
      this.dgvList.Size = new Size(417, 165);
      this.dgvList.TabIndex = 1;
      this.dgvList.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dgvList_CellMouseDoubleClick);
      this.dgvList.DataError += new DataGridViewDataErrorEventHandler(this.dgvList_DataError);
      this.dgvList.KeyDown += new KeyEventHandler(this.dgvList_KeyDown);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(417, 205);
      this.Controls.Add((Control) this.dgvList);
      this.Controls.Add((Control) this.panel1);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmList";
      this.Text = "Результаты поиска";
      this.Shown += new EventHandler(this.FrmList_Shown);
      this.panel1.ResumeLayout(false);
      ((ISupportInitialize) this.dgvList).EndInit();
      this.ResumeLayout(false);
    }
  }
}
