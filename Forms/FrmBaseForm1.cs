// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmBaseForm1
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public partial class FrmBaseForm1 : FrmBaseForm
  {
    private FormStateSaver fss = new FormStateSaver(FrmBaseForm1.container);
    private IContainer components = (IContainer) null;
    private static IContainer container;
    private Panel pnBtn;
    private Button btnExit;

    public FrmBaseForm1()
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
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
      this.btnExit = new Button();
      this.pnBtn.SuspendLayout();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 282);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(521, 40);
      this.pnBtn.TabIndex = 0;
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(427, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(82, 30);
      this.btnExit.TabIndex = 1;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(521, 322);
      this.Controls.Add((Control) this.pnBtn);
      this.Name = "FrmBaseForm1";
      this.Text = "FrmBaseForm1";
      this.pnBtn.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
