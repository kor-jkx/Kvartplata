// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Tariff
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class Tariff
  {
    private int tariff_id;
    private string tariff_name;
    private int tariff_num;
    private Service service;
    private string uname;
    private DateTime dedit;
    private BaseOrg manager;

    public virtual int Tariff_id
    {
      get
      {
        return this.tariff_id;
      }
      set
      {
        if (this.tariff_id == value)
          return;
        this.tariff_id = value;
      }
    }

    public virtual string Tariff_name
    {
      get
      {
        return this.tariff_name;
      }
      set
      {
        if (this.tariff_name == value)
          return;
        this.tariff_name = value;
      }
    }

    public virtual int Tariff_num
    {
      get
      {
        return this.tariff_num;
      }
      set
      {
        if (this.tariff_num == value)
          return;
        this.tariff_num = value;
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

    public virtual int? Counter_id { get; set; }

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

    public virtual BaseOrg Manager
    {
      get
      {
        return this.manager;
      }
      set
      {
        this.manager = value;
      }
    }

    public Tariff()
    {
    }

    public Tariff(Service service, string name, int id, int num)
    {
      this.tariff_id = id;
      this.Service = service;
      this.Tariff_name = name;
      this.tariff_num = num;
    }
  }
}
