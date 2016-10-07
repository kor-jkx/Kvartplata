// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Form1
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Forms;
using System.ComponentModel;
using System.Windows.Forms;

namespace Kvartplata.Classes
{
  public class Form1 : FrmBaseForm
  {
    private IContainer components = (IContainer) null;

    public Form1()
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
      this.components = (IContainer) new Container();
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Text = "Form1";
    }
  }
}
