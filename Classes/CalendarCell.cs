// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.CalendarCell
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.Windows.Forms;

namespace Kvartplata.Classes
{
  public class CalendarCell : DataGridViewTextBoxCell
  {
    public override System.Type EditType
    {
      get
      {
        return typeof (CalendarEditingControl);
      }
    }

    public override System.Type ValueType
    {
      get
      {
        return typeof (DateTime);
      }
    }

    public override object DefaultNewRowValue
    {
      get
      {
        return (object) DateTime.Now;
      }
    }

    public CalendarCell()
    {
      this.Style.Format = "d";
    }

    public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
    {
      base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
      CalendarEditingControl editingControl = this.DataGridView.EditingControl as CalendarEditingControl;
      if (this.Value == null || !(Convert.ToDateTime(this.Value) != Convert.ToDateTime("01.01.0001")))
        return;
      editingControl.Value = (DateTime) this.Value;
    }
  }
}
