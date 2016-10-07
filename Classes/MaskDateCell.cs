// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.MaskDateCell
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.Windows.Forms;

namespace Kvartplata.Classes
{
  public class MaskDateCell : DataGridViewTextBoxCell
  {
    public override System.Type EditType
    {
      get
      {
        return typeof (MaskDateEditingControl);
      }
    }

    public override System.Type ValueType
    {
      get
      {
        return typeof (string);
      }
    }

    public override object DefaultNewRowValue
    {
      get
      {
        return (object) DateTime.Now.ToShortDateString();
      }
    }

    public MaskDateCell()
    {
      this.Style.Format = "d";
    }

    public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
    {
      base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
      MaskDateEditingControl editingControl = this.DataGridView.EditingControl as MaskDateEditingControl;
      if (this.Value != null)
        editingControl.Text = this.Value.ToString();
      else
        editingControl.Text = "";
    }
  }
}
