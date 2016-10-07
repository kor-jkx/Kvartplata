// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmRegion
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Forms.Controls;
using Kvartplata.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmRegion : FrmBaseForm
  {
    private IContainer components = (IContainer) null;
    private Reg region;
    private Panel pnBtn;
    private Button btnCancel;
    private Button btnOk;
    private UCRegion ucRegion;

    public FrmRegion()
    {
      this.InitializeComponent();
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void ucSoato1_Load(object sender, EventArgs e)
    {
    }

    private void FrmSoato_Load(object sender, EventArgs e)
    {
      this.ucRegion.LoadSoato();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      this.region = this.ucRegion.ReturnRegion();
      this.Close();
    }

    public Reg ReturnRegion()
    {
      return this.region;
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
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.ucRegion = new UCRegion();
      this.pnBtn.SuspendLayout();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnOk);
      this.pnBtn.Controls.Add((Control) this.btnCancel);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 85);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(636, 40);
      this.pnBtn.TabIndex = 0;
      this.btnOk.Image = (Image) Resources.Tick;
      this.btnOk.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnOk.Location = new Point(12, 5);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(58, 30);
      this.btnOk.TabIndex = 1;
      this.btnOk.Text = "OK";
      this.btnOk.TextAlign = ContentAlignment.MiddleRight;
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.Image = (Image) Resources.delete;
      this.btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCancel.Location = new Point(538, 5);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(86, 30);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "Отмена";
      this.btnCancel.TextAlign = ContentAlignment.MiddleRight;
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnExit_Click);
      this.ucRegion.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.ucRegion.Location = new Point(0, 2);
      this.ucRegion.Margin = new Padding(4);
      this.ucRegion.Name = "ucRegion";
      this.ucRegion.Size = new Size(637, 81);
      this.ucRegion.TabIndex = 1;
      this.ucRegion.Load += new EventHandler(this.ucSoato1_Load);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(636, 125);
      this.Controls.Add((Control) this.ucRegion);
      this.Controls.Add((Control) this.pnBtn);
      this.Name = "FrmRegion";
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Выберите населенный пункт";
      this.Load += new EventHandler(this.FrmSoato_Load);
      this.pnBtn.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
