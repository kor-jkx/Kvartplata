// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Flat
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Flat
  {
    private int idFlat;
    private Home home;
    private short companyId;
    private string nFlat;
    private short entrance;

    public virtual int IdFlat
    {
      get
      {
        return this.idFlat;
      }
      set
      {
        this.idFlat = value;
      }
    }

    [Browsable(false)]
    public virtual Home Home
    {
      get
      {
        return this.home;
      }
      set
      {
        this.home = value;
      }
    }

    [Browsable(false)]
    public virtual short CompanyId
    {
      get
      {
        return this.companyId;
      }
      set
      {
        this.companyId = value;
      }
    }

    public virtual string NFlat
    {
      get
      {
        return this.nFlat;
      }
      set
      {
        this.nFlat = value;
      }
    }

    [Browsable(false)]
    public virtual short Entrance
    {
      get
      {
        return this.entrance;
      }
      set
      {
        this.entrance = value;
      }
    }
  }
}
