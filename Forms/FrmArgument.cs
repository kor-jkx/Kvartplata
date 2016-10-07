// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmArgument
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmArgument : FrmBaseForm
  {
    private IContainer components = (IContainer) null;
    private TextBox txbText;
    private Panel pnBtn;
    private Button btnExit;
    private Button btnOK;

    public FrmArgument()
    {
      this.InitializeComponent();
    }

    public string Argument()
    {
      return this.txbText.Text;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnOK_Click(object sender, EventArgs e)
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmArgument));
      this.txbText = new TextBox();
      this.pnBtn = new Panel();
      this.btnOK = new Button();
      this.btnExit = new Button();
      this.pnBtn.SuspendLayout();
      this.SuspendLayout();
      this.txbText.Dock = DockStyle.Fill;
      this.txbText.Location = new Point(0, 0);
      this.txbText.Multiline = true;
      this.txbText.Name = "txbText";
      this.txbText.Size = new Size(389, 142);
      this.txbText.TabIndex = 1;
      this.pnBtn.Controls.Add((Control) this.btnOK);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 102);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(389, 40);
      this.pnBtn.TabIndex = 2;
      this.btnOK.Image = (Image) Resources.Tick;
      this.btnOK.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnOK.Location = new Point(12, 5);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(58, 30);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.TextAlign = ContentAlignment.MiddleRight;
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(293, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(85, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(389, 142);
      this.Controls.Add((Control) this.pnBtn);
      this.Controls.Add((Control) this.txbText);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = "FrmArgument";
      this.Text = "Введите основание";
      this.pnBtn.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
