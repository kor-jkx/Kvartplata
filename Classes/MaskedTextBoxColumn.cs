// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.MaskedTextBoxColumn
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.Windows.Forms;

namespace Kvartplata.Classes
{
  public class MaskedTextBoxColumn : DataGridViewColumn
  {
    public override DataGridViewCell CellTemplate
    {
      get
      {
        return base.CellTemplate;
      }
      set
      {
        if (value != null && !value.GetType().IsAssignableFrom(typeof (MaskedTextBoxCell)))
          throw new InvalidCastException("Must be a CalendarCell");
        base.CellTemplate = value;
      }
    }

    public MaskedTextBoxColumn()
      : base((DataGridViewCell) new MaskedTextBoxCell())
    {
    }
  }
}
