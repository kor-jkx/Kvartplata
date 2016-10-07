// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Bank
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Bank
  {
    private int bankId;
    private string bankName;
    private Reg region;

    public virtual int BankId
    {
      get
      {
        return this.bankId;
      }
      set
      {
        this.bankId = value;
      }
    }

    public virtual string BankName
    {
      get
      {
        return this.bankName;
      }
      set
      {
        this.bankName = value;
      }
    }

    public virtual string NameMin { get; set; }

    public virtual string BIK { get; set; }

    public virtual string KorSch { get; set; }

    public virtual string INN { get; set; }

    [Browsable(false)]
    public virtual Reg Reg
    {
      get
      {
        return this.region;
      }
      set
      {
        this.region = value;
      }
    }
  }
}
