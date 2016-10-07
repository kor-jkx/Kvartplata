// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Seal
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Seal
  {
    [Browsable(false)]
    public virtual Counter Counter { get; set; }

    [Browsable(false)]
    public virtual short SealId { get; set; }

    public virtual string Inspector { get; set; }

    [Browsable(false)]
    public virtual TypeSeal TypeSeal { get; set; }

    public virtual string Number { get; set; }

    [Browsable(false)]
    public virtual DateTime Date { get; set; }

    [Browsable(false)]
    public virtual DateTime? RemoveDate { get; set; }

    public virtual string Note { get; set; }

    public virtual string UName { get; set; }

    public virtual DateTime DEdit { get; set; }

    [Browsable(false)]
    public virtual bool IsEdit { get; set; }

    [Browsable(false)]
    public virtual int OldHashCode { get; set; }

    [Browsable(false)]
    public virtual int? ClientId
    {
      get
      {
        if (this.Counter != null && this.Counter.LsClient != null)
          return new int?(this.Counter.LsClient.ClientId);
        return new int?();
      }
    }

    [Browsable(false)]
    public virtual Home Home
    {
      get
      {
        if (this.Counter != null && this.Counter.Home != null)
          return this.Counter.Home;
        return (Home) null;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      Seal seal = obj as Seal;
      return seal != null && (this.Counter.CounterId == seal.Counter.CounterId && (int) this.SealId == (int) seal.SealId);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int counterId = this.Counter.CounterId;
      int hashCode1 = this.Counter.CounterId.GetHashCode();
      int num2 = num1 + hashCode1;
      int sealId = (int) this.SealId;
      int hashCode2 = this.SealId.GetHashCode();
      return num2 + hashCode2;
    }
  }
}
