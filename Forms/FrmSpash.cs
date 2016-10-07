// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmSpash
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Properties;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmSpash : Form
  {
    private IContainer components = (IContainer) null;
    private Panel panel2;
    private Label lblVersion;
    private Label label2;
    private Label label1;
    private Panel panel1;
    private ProgressBar progressBar1;

    public FrmSpash()
    {
      this.InitializeComponent();
    }

    public void SetMaxValue(int Value)
    {
      this.progressBar1.Maximum = Value;
    }

    public void ChangeValue(int Value)
    {
      try
      {
        this.progressBar1.Value = Value;
        this.Refresh();
      }
      catch
      {
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmSpash));
      this.panel2 = new Panel();
      this.lblVersion = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.panel1 = new Panel();
      this.progressBar1 = new ProgressBar();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.panel2.BackColor = Color.Transparent;
      this.panel2.BorderStyle = BorderStyle.FixedSingle;
      this.panel2.Controls.Add((Control) this.lblVersion);
      this.panel2.Controls.Add((Control) this.label2);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Controls.Add((Control) this.panel1);
      this.panel2.Controls.Add((Control) this.progressBar1);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(411, (int) sbyte.MaxValue);
      this.panel2.TabIndex = 5;
      this.lblVersion.AutoSize = true;
      this.lblVersion.Font = new Font("Tahoma", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.lblVersion.Location = new Point(88, 67);
      this.lblVersion.Name = "lblVersion";
      this.lblVersion.Size = new Size(0, 13);
      this.lblVersion.TabIndex = 9;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Tahoma", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label2.Location = new Point(249, 67);
      this.label2.Name = "label2";
      this.label2.Size = new Size(143, 13);
      this.label2.TabIndex = 8;
      this.label2.Text = "© ООО \"БИТ\" г. Ярославль";
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Verdana", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.label1.Location = new Point(88, 11);
      this.label1.Name = "label1";
      this.label1.Size = new Size(318, 36);
      this.label1.TabIndex = 7;
      this.label1.Text = "\"Расчет жилищно-коммунальных \r\n платежей - \"Квартплата\"";
      this.panel1.BackgroundImage = (Image) Resources.world_64;
      this.panel1.BackgroundImageLayout = ImageLayout.Center;
      this.panel1.Location = new Point(3, 11);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(79, 69);
      this.panel1.TabIndex = 6;
      this.progressBar1.Location = new Point(128, 91);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new Size(129, 23);
      this.progressBar1.Step = 100;
      this.progressBar1.Style = ProgressBarStyle.Marquee;
      this.progressBar1.TabIndex = 5;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackgroundImageLayout = ImageLayout.None;
      this.ClientSize = new Size(411, (int) sbyte.MaxValue);
      this.Controls.Add((Control) this.panel2);
      this.DoubleBuffered = true;
      this.FormBorderStyle = FormBorderStyle.None;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = "FrmSpash";
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "FrmSpash";
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
