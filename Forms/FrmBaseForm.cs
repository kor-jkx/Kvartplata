﻿// Decompiled with JetBrains decompiler
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
        //private ComponentResourceManager AcomponentResourceManager = new ComponentResourceManager(typeof(FrmBaseForm));
        //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBaseForm));
        public FrmBaseForm()
    {
            //Icon = (System.Drawing.Icon)this.resources.GetObject("$this.Icon");
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
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(765, 322);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
            
            //this.Icon = (Icon)AcomponentResourceManager.GetObject("$this.Icon");
            this.Margin = new Padding(4, 4, 4, 4);
      this.Name = "FrmBaseForm";
      this.Text = "Ввод сумм квартального перерасчета";
      this.ResumeLayout(false);
    }
  }
}
