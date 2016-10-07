// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.LsFamily
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class LsFamily
  {
    private int familyId;
    private LsClient lsClient;
    private string familyName;
    private string uname;
    private DateTime dedit;

    public virtual int FamilyId
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

    public virtual string FamilyName
    {
      get
      {
        return this.familyName;
      }
      set
      {
        this.familyName = value;
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
  }
}
