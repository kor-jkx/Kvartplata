// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.ListHome
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;

namespace Kvartplata.Forms
{
  public class ListHome
  {
    private string _adr;

    public virtual Home Home { get; set; }

    public virtual int OhlAccountId { get; set; }

    public virtual bool check { get; set; }

    public virtual string Address
    {
      get
      {
        return this.Home.Address;
      }
      set
      {
        this._adr = this.Home.Address;
      }
    }
  }
}
