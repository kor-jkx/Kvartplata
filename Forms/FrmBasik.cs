// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmBasik
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
  public class FrmBasik : Form
  {
    private IContainer components = (IContainer) null;
    private Panel pnBtn;
    private Button btnExit;
    public DataGridView dgv;

    public FrmBasik()
    {
      this.InitializeComponent();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmBasik));
      this.pnBtn = new Panel();
      this.btnExit = new Button();
      this.dgv = new DataGridView();
      this.pnBtn.SuspendLayout();
      ((ISupportInitialize) this.dgv).BeginInit();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 282);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(726, 40);
      this.pnBtn.TabIndex = 0;
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(633, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(82, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.dgv.BackgroundColor = Color.AliceBlue;
      this.dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgv.Dock = DockStyle.Fill;
      this.dgv.Location = new Point(0, 0);
      this.dgv.Name = "dgv";
      this.dgv.Size = new Size(726, 282);
      this.dgv.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(726, 322);
      this.Controls.Add((Control) this.dgv);
      this.Controls.Add((Control) this.pnBtn);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmBasik";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "FrmBasik";
      this.pnBtn.ResumeLayout(false);
      ((ISupportInitialize) this.dgv).EndInit();
      this.ResumeLayout(false);
    }
  }
}
