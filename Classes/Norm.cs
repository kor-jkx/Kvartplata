// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Norm
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Iesi.Collections;
using System;

namespace Kvartplata.Classes
{
  public class Norm
  {
    private ISet cmpNorms = (ISet) new HashedSet();
    private int norm_id;
    private string norm_name;
    private int norm_num;
    private Service service;
    private string uname;
    private DateTime dedit;
    private BaseOrg manager;

    public virtual int Norm_id
    {
      get
      {
        return this.norm_id;
      }
      set
      {
        if (this.norm_id == value)
          return;
        this.norm_id = value;
      }
    }

    public virtual string Norm_name
    {
      get
      {
        return this.norm_name;
      }
      set
      {
        if (this.norm_name == value)
          return;
        this.norm_name = value;
      }
    }

    public virtual int Norm_num
    {
      get
      {
        return this.norm_num;
      }
      set
      {
        if (this.norm_num == value)
          return;
        this.norm_num = value;
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
        if (this.service == value)
          return;
        this.service = value;
      }
    }

    public virtual ISet CmpNorms
    {
      get
      {
        return this.cmpNorms;
      }
      set
      {
        this.cmpNorms = value;
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

    public Norm()
    {
    }

    public Norm(Service service, string name, int id, int num)
    {
      this.service = service;
      this.norm_name = name;
      this.norm_id = id;
      this.norm_num = num;
    }
  }
}
