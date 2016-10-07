// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.cmpTariffCost
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class cmpTariffCost
  {
    private int company_id;
    private Period period;
    private int tariff_id;
    private Service service;
    private DateTime dbeg;
    private DateTime dend;
    private short? scheme;
    private short? schemeParam;
    private double? cost;
    private int? unitMeasuring_id;
    private int? baseTariff_id;
    private double? cost_eo;
    private double? cost_c;
    private string uname;
    private DateTime dedit;
    private int? baseTariffMSP_id;
    private YesNo isVat;

    public virtual int Company_id
    {
      get
      {
        return this.company_id;
      }
      set
      {
        this.company_id = value;
      }
    }

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

    public virtual int Tariff_id
    {
      get
      {
        return this.tariff_id;
      }
      set
      {
        this.tariff_id = value;
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

    public virtual DateTime Dbeg
    {
      get
      {
        return this.dbeg;
      }
      set
      {
        this.dbeg = value;
      }
    }

    public virtual DateTime Dend
    {
      get
      {
        return this.dend;
      }
      set
      {
        this.dend = value;
      }
    }

    public virtual short? Scheme
    {
      get
      {
        return this.scheme;
      }
      set
      {
        this.scheme = value;
      }
    }

    public virtual short? SchemeParam
    {
      get
      {
        return this.schemeParam;
      }
      set
      {
        this.schemeParam = value;
      }
    }

    public virtual double? Cost
    {
      get
      {
        return this.cost;
      }
      set
      {
        this.cost = value;
      }
    }

    public virtual int? UnitMeasuring_id
    {
      get
      {
        return this.unitMeasuring_id;
      }
      set
      {
        int? unitMeasuringId = this.unitMeasuring_id;
        int? nullable = value;
        if (unitMeasuringId.GetValueOrDefault() == nullable.GetValueOrDefault() && unitMeasuringId.HasValue == nullable.HasValue)
          return;
        this.unitMeasuring_id = value;
      }
    }

    public virtual int? BaseTariff_id
    {
      get
      {
        return this.baseTariff_id;
      }
      set
      {
        int? baseTariffId = this.baseTariff_id;
        int? nullable = value;
        if (baseTariffId.GetValueOrDefault() == nullable.GetValueOrDefault() && baseTariffId.HasValue == nullable.HasValue)
          return;
        this.baseTariff_id = value;
      }
    }

    public virtual double? Cost_eo
    {
      get
      {
        return this.cost_eo;
      }
      set
      {
        double? costEo = this.cost_eo;
        double? nullable = value;
        if (costEo.GetValueOrDefault() == nullable.GetValueOrDefault() && costEo.HasValue == nullable.HasValue)
          return;
        this.cost_eo = value;
      }
    }

    public virtual double? Cost_c
    {
      get
      {
        return this.cost_c;
      }
      set
      {
        double? costC = this.cost_c;
        double? nullable = value;
        if (costC.GetValueOrDefault() == nullable.GetValueOrDefault() && costC.HasValue == nullable.HasValue)
          return;
        this.cost_c = value;
      }
    }

    public virtual int Complex_id
    {
      get
      {
        return Options.Complex.ComplexId;
      }
      set
      {
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

    public virtual int? BaseTariffMSP_id
    {
      get
      {
        return this.baseTariffMSP_id;
      }
      set
      {
        int? baseTariffMspId = this.baseTariffMSP_id;
        int? nullable = value;
        if (baseTariffMspId.GetValueOrDefault() == nullable.GetValueOrDefault() && baseTariffMspId.HasValue == nullable.HasValue)
          return;
        this.baseTariffMSP_id = value;
      }
    }

    [Browsable(false)]
    public virtual int OldHashCode { get; set; }

    public virtual string AllInfo { get; set; }

    public virtual YesNo IsVat { get; set; }

    public cmpTariffCost()
    {
    }

    public cmpTariffCost(int id)
    {
      this.tariff_id = id;
    }

    public cmpTariffCost(int id, double cost, short scheme, short schemeparam)
    {
      this.tariff_id = id;
      this.cost = new double?(cost);
      this.scheme = new short?(scheme);
      this.schemeParam = new short?(schemeparam);
    }

    public cmpTariffCost(int id, double cost, long scheme, int schemeparam)
    {
      this.tariff_id = id;
      this.cost = new double?(cost);
      try
      {
        this.scheme = new short?(Convert.ToInt16(scheme));
      }
      catch
      {
        this.scheme = new short?((short) 100);
      }
      this.schemeParam = new short?(Convert.ToInt16(schemeparam));
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      cmpTariffCost cmpTariffCost = obj as cmpTariffCost;
      return cmpTariffCost != null && ((int) this.service.ServiceId == (int) cmpTariffCost.service.ServiceId && this.period.PeriodId == cmpTariffCost.period.PeriodId && (this.dbeg == cmpTariffCost.dbeg && this.tariff_id == cmpTariffCost.tariff_id) && this.company_id == cmpTariffCost.company_id);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int companyId = this.company_id;
      int hashCode1 = this.company_id.GetHashCode();
      int num2 = num1 + hashCode1 + (this.period == null ? 0 : this.period.GetHashCode()) + (this.service == null ? 0 : this.service.GetHashCode());
      int tariffId = this.tariff_id;
      int hashCode2 = this.tariff_id.GetHashCode();
      return num2 + hashCode2;
    }

    public virtual void Copy(cmpTariffCost obj)
    {
      this.baseTariff_id = obj.baseTariff_id;
      this.baseTariffMSP_id = obj.baseTariffMSP_id;
      this.company_id = obj.company_id;
      this.cost = obj.cost;
      this.cost_c = obj.cost_c;
      this.cost_eo = obj.cost_eo;
      this.dend = Convert.ToDateTime("31.12.2999");
      this.period = obj.period;
      this.scheme = obj.scheme;
      this.schemeParam = obj.schemeParam;
      this.service = obj.service;
      this.tariff_id = obj.tariff_id;
      this.unitMeasuring_id = obj.unitMeasuring_id;
      this.dbeg = obj.dbeg;
      this.IsVat = obj.IsVat;
    }
  }
}
