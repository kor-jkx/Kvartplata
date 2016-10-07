// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.MaskDateColumn
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.Windows.Forms;

namespace Kvartplata.Classes
{
  public class MaskDateColumn : DataGridViewColumn
  {
    public override DataGridViewCell CellTemplate
    {
      get
      {
        return base.CellTemplate;
      }
      set
      {
        if (value != null && !value.GetType().IsAssignableFrom(typeof (MaskDateCell)))
          throw new InvalidCastException("Must be a MaskDateCell");
        base.CellTemplate = value;
      }
    }

    public MaskDateColumn()
      : base((DataGridViewCell) new MaskDateCell())
    {
    }
  }
}
