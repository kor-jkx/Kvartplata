﻿// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.SourcePay
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class SourcePay
  {
    private short sourcePayId;
    private string sourcePayName;
    private string uname;
    private DateTime dedit;

    public virtual short SourcePayId
    {
      get
      {
        return this.sourcePayId;
      }
      set
      {
        this.sourcePayId = value;
      }
    }

    public virtual string SourcePayName
    {
      get
      {
        return this.sourcePayName;
      }
      set
      {
        this.sourcePayName = value;
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
  }
}
