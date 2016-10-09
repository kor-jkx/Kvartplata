// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmBaseForm
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public partial class FrmBaseForm : Form
    {
        private IContainer components = (IContainer) null;
    public FrmBaseForm()
    {
            this.InitializeComponent();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.SuspendLayout();
            // 
            // FrmBaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 273);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmBaseForm";
            this.Text = "FrmBaseForm";
            this.ResumeLayout(false);

    }
  }
}
