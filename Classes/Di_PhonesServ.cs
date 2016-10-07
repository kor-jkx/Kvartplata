// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Di_PhonesServ
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class Di_PhonesServ
  {
    private int idservice;
    private string nameservice;
    private string uname;
    private DateTime dedit;

    public virtual int Idservice
    {
      get
      {
        return this.idservice;
      }
      set
      {
        if (this.idservice == value)
          return;
        this.idservice = value;
      }
    }

    public virtual string Nameservice
    {
      get
      {
        return this.nameservice;
      }
      set
      {
        if (this.nameservice == value)
          return;
        this.nameservice = value;
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
        if (this.uname == value)
          return;
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
        if (this.dedit == value)
          return;
        this.dedit = value;
      }
    }

    public virtual short ViewService { get; set; }
  }
}
