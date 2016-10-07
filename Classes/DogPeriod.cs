// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.DogPeriod
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class DogPeriod
  {
    [Browsable(false)]
    public virtual LsClient LsClient { get; set; }

    [Browsable(false)]
    public virtual DateTime DBeg { get; set; }

    [Browsable(false)]
    public virtual DateTime DEnd { get; set; }

    [Browsable(false)]
    public virtual bool IsEdit { get; set; }

    [Browsable(false)]
    public virtual int OldHashCode { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      DogPeriod dogPeriod = obj as DogPeriod;
      return dogPeriod != null && (this.LsClient.ClientId == dogPeriod.LsClient.ClientId && this.DBeg == dogPeriod.DBeg);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int clientId = this.LsClient.ClientId;
      int hashCode1 = this.LsClient.ClientId.GetHashCode();
      int num2 = num1 + hashCode1;
      DateTime dbeg = this.DBeg;
      int hashCode2 = this.DBeg.GetHashCode();
      return num2 + hashCode2;
    }
  }
}
