// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.PurposePay
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class PurposePay
  {
    private short purposePayId;
    private string purposePayName;
    private string uname;
    private DateTime dedit;

    public virtual short PurposePayId
    {
      get
      {
        return this.purposePayId;
      }
      set
      {
        this.purposePayId = value;
      }
    }

    public virtual string PurposePayName
    {
      get
      {
        return this.purposePayName;
      }
      set
      {
        this.purposePayName = value;
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
