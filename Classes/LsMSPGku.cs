// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.LsMSPGku
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class LsMSPGku
  {
    private LsClient lsClient;
    private DcMSP mspId;
    private Person person;
    private Period period;
    private DateTime dBeg;
    private DateTime dEnd;
    private MspDocument mspDocumentId;
    private Person holder;
    private LsFamily familyId;
    private short onOff;
    private short onOffTmpSq;
    private string uname;
    private DateTime dedit;
    private int acceptId;

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
    public virtual DcMSP MSPId
    {
      get
      {
        return this.mspId;
      }
      set
      {
        this.mspId = value;
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
    public virtual MspDocument MSPDocumentId
    {
      get
      {
        return this.mspDocumentId;
      }
      set
      {
        this.mspDocumentId = value;
      }
    }

    [Browsable(false)]
    public virtual Person Holder
    {
      get
      {
        return this.holder;
      }
      set
      {
        this.holder = value;
      }
    }

    [Browsable(false)]
    public virtual LsFamily FamilyId
    {
      get
      {
        return this.familyId;
      }
      set
      {
        this.familyId = value;
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

    [Browsable(false)]
    public virtual short OnOffTmpSq
    {
      get
      {
        return this.onOffTmpSq;
      }
      set
      {
        this.onOffTmpSq = value;
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
        this.dedit = value;
      }
    }

    [Browsable(false)]
    public virtual int AcceptId
    {
      get
      {
        return this.acceptId;
      }
      set
      {
        this.acceptId = value;
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
      LsMSPGku lsMspGku = obj as LsMSPGku;
      return lsMspGku != null && (this.lsClient.ClientId == lsMspGku.LsClient.ClientId && this.period.PeriodId == lsMspGku.Period.PeriodId && (this.dBeg == lsMspGku.DBeg && this.mspId.MSP_id == lsMspGku.MSPId.MSP_id) && this.person.PersonId == lsMspGku.person.PersonId);
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
      DateTime dBeg = this.dBeg;
      int hashCode3 = this.dBeg.GetHashCode();
      int num5 = num4 + hashCode3;
      int mspId = this.mspId.MSP_id;
      num2 = this.mspId.MSP_id;
      int hashCode4 = num2.GetHashCode();
      int num6 = num5 + hashCode4;
      int personId = this.person.PersonId;
      num2 = this.person.PersonId;
      int hashCode5 = num2.GetHashCode();
      return num6 + hashCode5;
    }
  }
}
