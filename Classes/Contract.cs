// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Contract
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Contract
  {
    public virtual int ContractId { get; set; }

    public virtual string ContractNum { get; set; }

    [Browsable(false)]
    public virtual BaseOrg Manager { get; set; }

    [Browsable(false)]
    public virtual BaseOrg BaseOrg { get; set; }

    [Browsable(false)]
    public virtual DateTime DBeg { get; set; }

    [Browsable(false)]
    public virtual DateTime DEnd { get; set; }

    public virtual string UName { get; set; }

    public virtual DateTime DEdit { get; set; }

    [Browsable(false)]
    public virtual bool IsEdit { get; set; }

    public Contract()
    {
    }

    public Contract(int id, string num)
    {
      this.ContractId = id;
      this.ContractNum = num;
    }
  }
}
