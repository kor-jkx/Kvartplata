// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.CorrectPeni
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class CorrectPeni
  {
    private Period period;
    private LsClient lsClient;
    private Service service;
    private int supplier;
    private double correct;
    private string note;
    private string uName;
    private DateTime dEdit;

    [Browsable(false)]
    public virtual LsClient LsClient
    {
      get
      {
        return this.lsClient;
      }
      set
      {
        this.lsClient = value;
      }
    }

    [Browsable(false)]
    public virtual Period Period
    {
      get
      {
        return this.period;
      }
      set
      {
        this.period = value;
      }
    }

    public virtual Service Service
    {
      get
      {
        return this.service;
      }
      set
      {
        this.service = value;
      }
    }

    [Browsable(false)]
    public virtual int Supplier
    {
      get
      {
        return this.supplier;
      }
      set
      {
        this.supplier = value;
      }
    }

    public virtual double Correct
    {
      get
      {
        return this.correct;
      }
      set
      {
        this.correct = value;
      }
    }

    public virtual string Note
    {
      get
      {
        return this.note;
      }
      set
      {
        this.note = value;
      }
    }

    public virtual string UName
    {
      get
      {
        return this.uName;
      }
      set
      {
        this.uName = value;
      }
    }

    public virtual DateTime DEdit
    {
      get
      {
        return this.dEdit;
      }
      set
      {
        this.dEdit = value;
      }
    }

    [Browsable(false)]
    public virtual bool IsEdit { get; set; }

    [Browsable(false)]
    public virtual int OldHashCode { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      CorrectPeni correctPeni = obj as CorrectPeni;
      return correctPeni != null && (this.lsClient.ClientId == correctPeni.LsClient.ClientId && this.period.PeriodId == correctPeni.Period.PeriodId && ((int) this.service.ServiceId == (int) correctPeni.Service.ServiceId && this.supplier == correctPeni.Supplier) && this.note == correctPeni.Note);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int clientId = this.lsClient.ClientId;
      int num2 = this.lsClient.ClientId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int periodId = this.period.PeriodId;
      num2 = this.period.PeriodId;
      int hashCode2 = num2.GetHashCode();
      int num4 = num3 + hashCode2;
      int serviceId = (int) this.service.ServiceId;
      int hashCode3 = this.service.ServiceId.GetHashCode();
      int num5 = num4 + hashCode3;
      int supplier = this.supplier;
      int hashCode4 = this.supplier.GetHashCode();
      return num5 + hashCode4 + (this.note == null ? 0 : this.note.GetHashCode());
    }
  }
}
