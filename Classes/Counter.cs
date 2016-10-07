// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Counter
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Counter
  {
    private int counterId;
    private Service service;
    private Company company;
    private Home home;
    private LsClient lsClient;
    private string counterNum;
    private BaseCounter baseCounter;
    private TypeCounter typeCounter;
    private string series;
    private string notice;
    private DateTime? archivesDate;
    private Complex complex;
    private DateTime? setDate;
    private DateTime? removeDate;
    private DateTime? auditDate;
    private double evidenceStart;
    private string uName;
    private DateTime dEdit;
    private double coeffTrans;
    private CounterLocation location;

    public virtual int CounterId
    {
      get
      {
        return this.counterId;
      }
      set
      {
        this.counterId = value;
      }
    }

    [Browsable(false)]
    public virtual Counter MainCounter { get; set; }

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
    public virtual Company Company
    {
      get
      {
        return this.company;
      }
      set
      {
        this.company = value;
      }
    }

    [Browsable(false)]
    public virtual Home Home
    {
      get
      {
        return this.home;
      }
      set
      {
        this.home = value;
      }
    }

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

    public virtual string CounterNum
    {
      get
      {
        return this.counterNum;
      }
      set
      {
        this.counterNum = value;
      }
    }

    [Browsable(false)]
    public virtual BaseCounter BaseCounter
    {
      get
      {
        return this.baseCounter;
      }
      set
      {
        this.baseCounter = value;
      }
    }

    [Browsable(false)]
    public virtual TypeCounter TypeCounter
    {
      get
      {
        return this.typeCounter;
      }
      set
      {
        this.typeCounter = value;
      }
    }

    public virtual string Series
    {
      get
      {
        return this.series;
      }
      set
      {
        this.series = value;
      }
    }

    public virtual string Notice
    {
      get
      {
        return this.notice;
      }
      set
      {
        this.notice = value;
      }
    }

    public virtual DateTime? ArchivesDate
    {
      get
      {
        return this.archivesDate;
      }
      set
      {
        this.archivesDate = value;
      }
    }

    [Browsable(false)]
    public virtual Complex Complex
    {
      get
      {
        return this.complex;
      }
      set
      {
        this.complex = value;
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
    public virtual DateTime? SetDate
    {
      get
      {
        return this.setDate;
      }
      set
      {
        this.setDate = value;
      }
    }

    [Browsable(false)]
    public virtual DateTime? RemoveDate
    {
      get
      {
        return this.removeDate;
      }
      set
      {
        this.removeDate = value;
      }
    }

    public virtual DateTime? AuditDate
    {
      get
      {
        return this.auditDate;
      }
      set
      {
        this.auditDate = value;
      }
    }

    [Browsable(false)]
    public virtual double EvidenceStart
    {
      get
      {
        return this.evidenceStart;
      }
      set
      {
        this.evidenceStart = value;
      }
    }

    [Browsable(false)]
    public virtual double CoeffTrans
    {
      get
      {
        return this.coeffTrans;
      }
      set
      {
        this.coeffTrans = value;
      }
    }

    [Browsable(false)]
    public virtual bool IsEdit { get; set; }

    [Browsable(false)]
    public virtual CounterLocation Location { get; set; }

    [Browsable(false)]
    public virtual DateTime? DopDate { get; set; }

    public virtual string ServiceName
    {
      get
      {
        return this.service.ServiceName;
      }
    }

    public virtual string MainCounterInfo
    {
      get
      {
        try
        {
          return "№" + this.counterNum.ToString() + "   " + this.service.ServiceName;
        }
        catch
        {
          return "";
        }
      }
    }

    public virtual string AllInfo
    {
      get
      {
        try
        {
          return "№" + this.counterNum.ToString() + "   " + this.service.ServiceName + "(" + this.baseCounter.Name + ")";
        }
        catch
        {
          return "";
        }
      }
    }

    public virtual string AdrAndNum
    {
      get
      {
        if (this.home != null)
          return this.home.Address + "   №" + this.counterNum.ToString() + "(" + this.service.ServiceName + ")";
        return "№" + this.counterNum.ToString() + "(" + this.service.ServiceName + ")";
      }
    }

    public virtual string FlatAndNum
    {
      get
      {
        return this.lsClient.ClientId.ToString() + "  кв. " + this.lsClient.Flat.NFlat + "  №" + this.counterNum.ToString() + "(" + this.service.ServiceName + ")";
      }
    }

    public virtual string CounterNum1
    {
      get
      {
        int num;
        if (this.archivesDate.HasValue)
        {
          DateTime? archivesDate = this.archivesDate;
          DateTime dateTime = Options.Period.PeriodName.Value;
          num = archivesDate.HasValue ? (archivesDate.GetValueOrDefault() >= dateTime ? 1 : 0) : 0;
        }
        else
          num = 1;
        if (num != 0)
          return this.counterNum;
        return this.counterNum + " (арх)";
      }
    }

    public Counter()
    {
    }

    public Counter(int id, string num)
    {
      this.counterId = id;
      this.counterNum = num;
    }

    public Counter(int id, string num, LsClient client, Home home, Service service, DateTime auditDate, DateTime dopDate)
    {
      this.counterId = id;
      this.counterNum = num;
      this.lsClient = client;
      this.home = home;
      this.service = service;
      this.auditDate = new DateTime?(auditDate);
      this.DopDate = new DateTime?(dopDate);
    }
  }
}
