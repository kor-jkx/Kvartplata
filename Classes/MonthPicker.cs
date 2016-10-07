// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.MonthPicker
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.Windows.Forms;

namespace Kvartplata.Classes
{
  public class MonthPicker : DateTimePicker
  {
    private int oldMonth;

    public virtual int OldMonth
    {
      get
      {
        return this.oldMonth;
      }
      set
      {
        this.oldMonth = value;
      }
    }

    public MonthPicker()
    {
      this.Format = DateTimePickerFormat.Custom;
      this.ShowUpDown = true;
      this.CustomFormat = "MMMM yyyy";
      DateTime? periodName;
      int month;
      if (Options.Period != null)
      {
        periodName = Options.Period.PeriodName;
        month = periodName.Value.Month;
      }
      else
        month = this.Value.Month;
      this.oldMonth = month;
      this.Width = 140;
      if (Options.MinPeriod != null)
      {
        periodName = Options.MinPeriod.PeriodName;
        this.MinDate = periodName.Value;
      }
      if (Options.MaxPeriod == null)
        return;
      periodName = Options.MaxPeriod.PeriodName;
      this.MaxDate = periodName.Value;
    }

    protected override void OnValueChanged(EventArgs eventargs)
    {
      base.OnValueChanged(eventargs);
      DateTime dateTime;
      int num1;
      if (this.oldMonth == 12)
      {
        dateTime = this.Value;
        num1 = dateTime.Month == 1 ? 1 : 0;
      }
      else
        num1 = 0;
      if (num1 != 0)
      {
        dateTime = this.Value;
        this.oldMonth = dateTime.Month;
        try
        {
          string str1 = "01.";
          string str2 = Convert.ToString(this.oldMonth);
          string str3 = ".";
          dateTime = this.Value;
          string str4 = Convert.ToString(dateTime.Year + 1);
          this.Value = Convert.ToDateTime(str1 + str2 + str3 + str4);
        }
        catch (Exception ex)
        {
        }
      }
      int num2;
      if (this.oldMonth == 1)
      {
        dateTime = this.Value;
        num2 = dateTime.Month == 12 ? 1 : 0;
      }
      else
        num2 = 0;
      if (num2 != 0)
      {
        dateTime = this.Value;
        this.oldMonth = dateTime.Month;
        try
        {
          string str1 = "01.";
          string str2 = Convert.ToString(this.oldMonth);
          string str3 = ".";
          dateTime = this.Value;
          string str4 = Convert.ToString(dateTime.Year - 1);
          this.Value = Convert.ToDateTime(str1 + str2 + str3 + str4);
        }
        catch (Exception ex)
        {
        }
      }
      dateTime = this.Value;
      this.oldMonth = dateTime.Month;
    }
  }
}
