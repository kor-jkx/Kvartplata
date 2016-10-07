// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.CalendarEditingControl
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.Windows.Forms;

namespace Kvartplata.Classes
{
  internal class CalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
  {
    private bool valueChanged = false;
    private DataGridView dataGridView;
    private int rowIndex;

    public object EditingControlFormattedValue
    {
      get
      {
        return (object) this.Value.ToShortDateString();
      }
      set
      {
        if (!(value is string))
          return;
        this.Value = DateTime.Parse((string) value);
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

    public CalendarEditingControl()
    {
      this.Format = DateTimePickerFormat.Short;
    }

    public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
    {
      return this.EditingControlFormattedValue;
    }

    public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
    {
      this.Font = dataGridViewCellStyle.Font;
      this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
      this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
    }

    public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
    {
      switch (key & Keys.KeyCode)
      {
        case Keys.Prior:
        case Keys.Next:
        case Keys.End:
        case Keys.Home:
        case Keys.Left:
        case Keys.Up:
        case Keys.Right:
        case Keys.Down:
          return true;
        default:
          return !dataGridViewWantsInputKey;
      }
    }

    public void PrepareEditingControlForEdit(bool selectAll)
    {
    }

    protected override void OnValueChanged(EventArgs eventargs)
    {
      this.valueChanged = true;
      this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
      base.OnValueChanged(eventargs);
    }
  }
}
