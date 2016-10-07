// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.TextBoxWithPromting
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class TextBoxWithPromting : TextBox
  {
    private string promting;

    public virtual string PlaceHolder
    {
      get
      {
        return this.promting;
      }
      set
      {
        this.promting = value;
        this.Text = this.promting;
      }
    }

    public TextBoxWithPromting()
    {
      this.ForeColor = Color.DarkGray;
    }

    protected override void OnClick(EventArgs e)
    {
      if (this.ForeColor == Color.DarkGray)
      {
        this.Text = "";
        this.ForeColor = Color.Black;
      }
      base.OnClick(e);
    }

    protected override void OnLeave(EventArgs e)
    {
      if (this.Text == "")
      {
        this.Text = this.promting;
        this.ForeColor = Color.DarkGray;
      }
      base.OnLeave(e);
    }

    public virtual void NewSize(string strText, int width, int height)
    {
      this.Text = strText;
      this.promting = strText;
      this.Width = width;
      this.Height = height;
    }
  }
}
