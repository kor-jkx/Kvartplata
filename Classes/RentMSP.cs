// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.RentMSP
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class RentMSP
  {
    private Period period;
    private LsClient lsClient;
    private Service service;
    private Supplier supplier;
    private Period month;
    private short code;
    private int motive;
    private DcMSP msp;
    private Person person;
    private double volume;
    private double rent;
    private float mspPeople;
    private short rentType;

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
    public virtual Supplier Supplier
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

    [Browsable(false)]
    public virtual Period Month
    {
      get
      {
        return this.month;
      }
      set
      {
        this.month = value;
      }
    }

    [Browsable(false)]
    public virtual short Code
    {
      get
      {
        return this.code;
      }
      set
      {
        this.code = value;
      }
    }

    [Browsable(false)]
    public virtual int Motive
    {
      get
      {
        return this.motive;
      }
      set
      {
        this.motive = value;
      }
    }

    [Browsable(false)]
    public virtual DcMSP MSP
    {
      get
      {
        return this.msp;
      }
      set
      {
        this.msp = value;
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
    public virtual double Volume
    {
      get
      {
        return this.volume;
      }
      set
      {
        this.volume = value;
      }
    }

    [Browsable(false)]
    public virtual double RentMain
    {
      get
      {
        return this.rent;
      }
      set
      {
        this.rent = value;
      }
    }

    [Browsable(false)]
    public virtual float MSPPeople
    {
      get
      {
        return this.mspPeople;
      }
      set
      {
        this.mspPeople = value;
      }
    }

    [Browsable(false)]
    public virtual short RentType { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      RentMSP rentMsp = obj as RentMSP;
      return rentMsp != null && (this.lsClient.ClientId == rentMsp.LsClient.ClientId && this.period.PeriodId == rentMsp.Period.PeriodId && ((int) this.service.ServiceId == (int) rentMsp.Service.ServiceId && this.supplier.SupplierId == rentMsp.Supplier.SupplierId) && (this.month.PeriodId == rentMsp.Month.PeriodId && (int) this.code == (int) rentMsp.Code && (this.motive == rentMsp.Motive && this.msp.MSP_id == rentMsp.MSP.MSP_id)) && this.person.PersonId == rentMsp.Person.PersonId && (int) this.rentType == (int) rentMsp.rentType);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int clientId = this.lsClient.ClientId;
      int num2 = this.lsClient.ClientId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int periodId1 = this.period.PeriodId;
      num2 = this.period.PeriodId;
      int hashCode2 = num2.GetHashCode();
      int num4 = num3 + hashCode2;
      int serviceId = (int) this.service.ServiceId;
      int hashCode3 = this.service.ServiceId.GetHashCode();
      int num5 = num4 + hashCode3;
      int supplierId = this.supplier.SupplierId;
      num2 = this.supplier.SupplierId;
      int hashCode4 = num2.GetHashCode();
      int num6 = num5 + hashCode4;
      int periodId2 = this.month.PeriodId;
      num2 = this.month.PeriodId;
      int hashCode5 = num2.GetHashCode();
      int num7 = num6 + hashCode5;
      int code = (int) this.code;
      int hashCode6 = this.code.GetHashCode();
      int num8 = num7 + hashCode6;
      int motive = this.motive;
      int hashCode7 = this.motive.GetHashCode();
      int num9 = num8 + hashCode7;
      int mspId = this.msp.MSP_id;
      num2 = this.msp.MSP_id;
      int hashCode8 = num2.GetHashCode();
      int num10 = num9 + hashCode8;
      int personId = this.person.PersonId;
      num2 = this.person.PersonId;
      int hashCode9 = num2.GetHashCode();
      int num11 = num10 + hashCode9;
      int rentType = (int) this.rentType;
      int hashCode10 = this.rentType.GetHashCode();
      return num11 + hashCode10;
    }
  }
}
