// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.MaskedTextBoxCell
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System.Windows.Forms;

namespace Kvartplata.Classes
{
  public class MaskedTextBoxCell : DataGridViewTextBoxCell
  {
    public override System.Type EditType
    {
      get
      {
        return typeof (MaskedTextBoxEditingControl);
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
        return (object) "";
      }
    }

    public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
    {
      base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
      MaskedTextBoxEditingControl editingControl = this.DataGridView.EditingControl as MaskedTextBoxEditingControl;
      editingControl.Mask = "00000000000000000000";
      if (this.Value != null)
        editingControl.Text = this.Value.ToString();
      else
        editingControl.Text = "";
    }
  }
}
