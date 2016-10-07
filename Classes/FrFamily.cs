// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.FrFamily
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class FrFamily
  {
    private LsFamily lsFamily;
    private Person person;
    private Period period;
    private DateTime dBeg;
    private DateTime dEnd;
    private short onOff;
    private string uname;
    private DateTime dedit;

    [Browsable(false)]
    public virtual LsFamily LsFamily
    {
      get
      {
        return this.lsFamily;
      }
      set
      {
        this.lsFamily = value;
      }
    }

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
    public virtual DateTime DBeg
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
    public virtual DateTime DEnd
    {
      get
      {
        return this.dEnd;
      }
      set
      {
        this.dEnd = value;
      }
    }

    [Browsable(false)]
    public virtual short OnOff
    {
      get
      {
        return this.onOff;
      }
      set
      {
        this.onOff = value;
      }
    }

    public virtual string Uname
    {
      get
      {
        return this.uname;
      }
      set
      {
        this.uname = value;
      }
    }

    public virtual DateTime Dedit
    {
      get
      {
        return this.dedit;
      }
      set
      {
        this.dedit = new DateTime();
        this.dedit = DateTime.Now.Date;
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
      FrFamily frFamily = obj as FrFamily;
      return frFamily != null && (this.lsFamily.FamilyId == frFamily.lsFamily.FamilyId && this.period.PeriodId == frFamily.Period.PeriodId && this.dBeg == frFamily.DBeg && this.person.PersonId == frFamily.person.PersonId);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int familyId = this.lsFamily.FamilyId;
      int num2 = this.lsFamily.FamilyId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int periodId = this.period.PeriodId;
      num2 = this.period.PeriodId;
      int hashCode2 = num2.GetHashCode();
      int num4 = num3 + hashCode2;
      DateTime dBeg = this.dBeg;
      int hashCode3 = this.dBeg.GetHashCode();
      int num5 = num4 + hashCode3;
      int personId = this.person.PersonId;
      num2 = this.person.PersonId;
      int hashCode4 = num2.GetHashCode();
      return num5 + hashCode4;
    }
  }
}
