// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.LsAbsence
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class LsAbsence
  {
    private Absence absence;
    private DateTime? dBeg;
    private LsClient lsClient;
    private short motive;
    private Period period;
    private Person person;

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

    [Browsable(false)]
    public virtual Absence Absence
    {
      get
      {
        return this.absence;
      }
      set
      {
        this.absence = value;
      }
    }

    [Browsable(false)]
    public virtual DateTime? DBeg
    {
      get
      {
        return this.dBeg;
      }
      set
      {
        this.dBeg = value;
      }
    }

    [Browsable(false)]
    public virtual DateTime? DEnd { get; set; }

    [Browsable(false)]
    public virtual Person Person
    {
      get
      {
        return this.person;
      }
      set
      {
        this.person = value;
      }
    }

    public virtual string Document { get; set; }

    public virtual string UName { get; set; }

    public virtual DateTime DEdit { get; set; }

    [Browsable(false)]
    public virtual bool IsEdit { get; set; }

    [Browsable(false)]
    public virtual int OldHashCode { get; set; }

    [Browsable(false)]
    public virtual YesNo OnOff { get; set; }

    [Browsable(false)]
    public virtual short Motive { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      LsAbsence lsAbsence = obj as LsAbsence;
      if (lsAbsence == null)
        return false;
      int num;
      if (this.lsClient.ClientId == lsAbsence.LsClient.ClientId && this.period.PeriodId == lsAbsence.Period.PeriodId)
      {
        DateTime? dBeg = this.dBeg;
        DateTime? dbeg = lsAbsence.DBeg;
        if ((dBeg.HasValue == dbeg.HasValue ? (dBeg.HasValue ? (dBeg.GetValueOrDefault() == dbeg.GetValueOrDefault() ? 1 : 0) : 1) : 0) != 0 && this.person.PersonId == lsAbsence.Person.PersonId && (int) this.absence.Absence_id == (int) lsAbsence.Absence.Absence_id)
        {
          num = (int) this.Motive == (int) lsAbsence.Motive ? 1 : 0;
          goto label_8;
        }
      }
      num = 0;
label_8:
      return num != 0;
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
      int num4 = num3 + hashCode2 + (!this.dBeg.HasValue ? 0 : this.dBeg.GetHashCode());
      int personId = this.person.PersonId;
      num2 = this.person.PersonId;
      int hashCode3 = num2.GetHashCode();
      int num5 = num4 + hashCode3;
      int absenceId = (int) this.absence.Absence_id;
      short num6 = this.absence.Absence_id;
      int hashCode4 = num6.GetHashCode();
      int num7 = num5 + hashCode4;
      int motive = (int) this.Motive;
      num6 = this.Motive;
      int hashCode5 = num6.GetHashCode();
      return num7 + hashCode5;
    }
  }
}
