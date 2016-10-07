// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.FormStateSaver
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Kvartplata.Classes
{
  public class FormStateSaver : Component
  {
    private IContainer components = (IContainer) null;
    private Form parentForm;

    public Form ParentForm
    {
      get
      {
        return this.parentForm;
      }
      set
      {
        if (this.parentForm == value)
          return;
        if (this.parentForm != null)
        {
          this.saveState();
          this.parentForm.Load -= new EventHandler(this.parentForm_Load);
          this.parentForm.FormClosed -= new FormClosedEventHandler(this.parentForm_FormClosed);
        }
        this.parentForm = value;
        this.loadState();
        this.parentForm.Load += new EventHandler(this.parentForm_Load);
        this.parentForm.FormClosed += new FormClosedEventHandler(this.parentForm_FormClosed);
      }
    }

    public FormStateSaver(IContainer container)
    {
      if (container != null)
        container.Add((IComponent) this);
      this.InitializeComponent();
    }

    protected virtual void saveState()
    {
      if (this.ParentForm == null)
        return;
      if (this.ParentForm.Name == string.Empty)
        return;
      try
      {
        if (!Directory.Exists(Options.PathProfileAppData + "\\State"))
          Directory.CreateDirectory(Options.PathProfileAppData + "\\State");
        StreamWriter streamWriter1 = new StreamWriter(Options.PathProfileAppData + "\\State\\fss_" + this.ParentForm.Name + ".dat");
        this.ParentForm.WindowState = this.ParentForm.WindowState == FormWindowState.Minimized ? FormWindowState.Normal : this.ParentForm.WindowState;
        StreamWriter streamWriter2 = streamWriter1;
        int num = this.ParentForm.Left;
        string str1 = num.ToString();
        streamWriter2.WriteLine(str1);
        StreamWriter streamWriter3 = streamWriter1;
        num = this.ParentForm.Top;
        string str2 = num.ToString();
        streamWriter3.WriteLine(str2);
        StreamWriter streamWriter4 = streamWriter1;
        num = this.ParentForm.Width;
        string str3 = num.ToString();
        streamWriter4.WriteLine(str3);
        StreamWriter streamWriter5 = streamWriter1;
        num = this.ParentForm.Height;
        string str4 = num.ToString();
        streamWriter5.WriteLine(str4);
        streamWriter1.Close();
      }
      catch (Exception ex)
      {
      }
    }

    protected virtual void loadState()
    {
      if (this.ParentForm == null)
        return;
      if (this.ParentForm.Name == string.Empty)
        return;
      try
      {
        StreamReader streamReader = new StreamReader(Options.PathProfileAppData + "\\State\\fss_" + this.ParentForm.Name + ".dat");
        this.ParentForm.Left = int.Parse(streamReader.ReadLine());
        this.ParentForm.Top = int.Parse(streamReader.ReadLine());
        this.ParentForm.Width = int.Parse(streamReader.ReadLine());
        this.ParentForm.Height = int.Parse(streamReader.ReadLine());
        streamReader.Close();
      }
      catch (Exception ex)
      {
      }
    }

    private void parentForm_Load(object sender, EventArgs e)
    {
      this.loadState();
    }

    private void parentForm_FormClosed(object sender, EventArgs e)
    {
      this.saveState();
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
    }
  }
}
