// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.MaskedTextBoxEditingControl
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.Windows.Forms;

namespace Kvartplata.Classes
{
  internal class MaskedTextBoxEditingControl : MaskedTextBox, IDataGridViewEditingControl
  {
    private bool valueChanged = false;
    private DataGridView dataGridView;
    private int rowIndex;

    public object EditingControlFormattedValue
    {
      get
      {
        return (object) this.Text;
      }
      set
      {
        if (!(value is string))
          return;
        this.Text = value.ToString();
      }
    }

    public int EditingControlRowIndex
    {
      get
      {
        return this.rowIndex;
      }
      set
      {
        this.rowIndex = value;
      }
    }

    public bool RepositionEditingControlOnValueChange
    {
      get
      {
        return false;
      }
    }

    public DataGridView EditingControlDataGridView
    {
      get
      {
        return this.dataGridView;
      }
      set
      {
        this.dataGridView = value;
      }
    }

    public bool EditingControlValueChanged
    {
      get
      {
        return this.valueChanged;
      }
      set
      {
        this.valueChanged = value;
      }
    }

    public Cursor EditingPanelCursor
    {
      get
      {
        return this.Cursor;
      }
    }

    public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
    {
      return this.EditingControlFormattedValue;
    }

    public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
    {
      this.Font = dataGridViewCellStyle.Font;
      this.ForeColor = dataGridViewCellStyle.ForeColor;
      this.BackColor = dataGridViewCellStyle.BackColor;
    }

    public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
    {
      return true;
    }

    public void PrepareEditingControlForEdit(bool selectAll)
    {
    }

    protected override void OnTextChanged(EventArgs e)
    {
      this.valueChanged = true;
      this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
      base.OnTextChanged(e);
    }
  }
}
