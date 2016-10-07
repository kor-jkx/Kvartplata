// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.hmWorkDistribute
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class hmWorkDistribute
  {
    private int WorkDistribute_id;
    private Company company;
    private Home home;

    public virtual int WorkDistribute
    {
      get
      {
        return this.WorkDistribute_id;
      }
      set
      {
        this.WorkDistribute_id = value;
      }
    }

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

    public virtual Period Period { get; set; }

    public virtual Service Service { get; set; }

    public virtual int Scheme { get; set; }

    public virtual Decimal Rent { get; set; }

    public virtual Decimal ParamValue { get; set; }

    public virtual string Uname { get; set; }

    public virtual DateTime? Dedit { get; set; }
  }
}
