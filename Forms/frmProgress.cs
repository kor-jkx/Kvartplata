// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.frmProgress
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class frmProgress : Form
  {
    private int progress = 0;
    private IContainer components = (IContainer) null;
    public ProgressBar progressBar;

    public int Progress
    {
      get
      {
        return this.progress;
      }
      set
      {
        this.progress = value;
        this.progressBar.Value = value;
      }
    }

    public frmProgress(int max)
    {
      this.InitializeComponent();
      this.progressBar.Value = 0;
      this.progress = 0;
      this.progressBar.Maximum = max;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (frmProgress));
      this.progressBar = new ProgressBar();
      this.SuspendLayout();
      this.progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.progressBar.Location = new Point(4, 12);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new Size(285, 23);
      this.progressBar.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(292, 46);
      this.Controls.Add((Control) this.progressBar);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = "frmProgress";
      this.Text = "Выполнено";
      this.ResumeLayout(false);
    }
  }
}
