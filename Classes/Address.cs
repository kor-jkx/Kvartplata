// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Address
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  internal class Address
  {
    private int clientLsId;
    private string str;
    private string number;
    private string korp;
    private string flat;
    private string surFlat;

    public virtual int ClientLsId
    {
      get
      {
        return this.clientLsId;
      }
      set
      {
        this.clientLsId = value;
      }
    }

    public virtual string Str
    {
      get
      {
        return this.str;
      }
      set
      {
        this.str = value;
      }
    }

    public virtual string Number
    {
      get
      {
        return this.number;
      }
      set
      {
        this.number = value;
      }
    }

    public virtual string Korp
    {
      get
      {
        return this.korp;
      }
      set
      {
        this.korp = value;
      }
    }

    public virtual string Flat
    {
      get
      {
        return this.flat;
      }
      set
      {
        this.flat = value;
      }
    }

    public virtual string SurFlat
    {
      get
      {
        return this.surFlat;
      }
      set
      {
        this.surFlat = value;
      }
    }

    public Address()
    {
    }

    public Address(int clientLsId, string str, string number, string korp, string flat, string surFlat)
    {
      this.str = str;
      this.number = number;
      this.korp = korp;
      this.clientLsId = clientLsId;
      this.flat = flat;
      this.surFlat = surFlat;
    }

    public Address(string str, string number, string korp)
    {
      this.str = str;
      this.number = number;
      this.korp = korp;
    }

    public Address(string flat, string surFlat, int clientLsId)
    {
      this.flat = flat;
      this.surFlat = surFlat;
      this.clientLsId = clientLsId;
    }
  }
}
