// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.MaskDateEditingControl
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Kvartplata.Classes
{
  public class MaskDateEditingControl : MaskedTextBox, IDataGridViewEditingControl
  {
    private bool valueChanged = false;
    private IContainer components = (IContainer) null;
    private DataGridView dataGridView;
    private int rowIndex;

    public object EditingControlFormattedValue
    {
      get
      {
        try
        {
          if (Convert.ToDateTime(this.Text) <= Convert.ToDateTime("31.12.2999"))
            return (object) this.Text;
          return (object) "31.12.2999";
        }
        catch (Exception ex)
        {
          return (object) this.Text;
        }
      }
      set
      {
        if (!(value is string))
          return;
        this.Text = (string) value;
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

    public MaskDateEditingControl()
    {
      this.Mask = "00/00/0000";
      this.PromptChar = '_';
    }

    public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
    {
      return this.EditingControlFormattedValue;
    }

    public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
    {
      this.Font = dataGridViewCellStyle.Font;
      this.BackColor = dataGridViewCellStyle.BackColor;
      this.ForeColor = dataGridViewCellStyle.ForeColor;
    }

    public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
    {
      return true;
    }

    public void PrepareEditingControlForEdit(bool selectAll)
    {
    }

    protected override void OnTextChanged(EventArgs eventargs)
    {
      this.valueChanged = true;
      if (this.EditingControlDataGridView != null)
        this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
      base.OnTextChanged(eventargs);
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
