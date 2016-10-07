// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Home
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Home
  {
    private int idHome;
    private string nHome;
    private string homeKorp;
    private int division;
    private Company company;
    private Str str;
    private int numEntrance;
    private int? yearBuild;
    private Mwl mwl;
    private int? reu;
    private int homeType;
    private short archive;

    public virtual int IdHome
    {
      get
      {
        return this.idHome;
      }
      set
      {
        this.idHome = value;
      }
    }

    public virtual string NHome
    {
      get
      {
        return this.nHome;
      }
      set
      {
        this.nHome = value;
      }
    }

    public virtual string HomeKorp
    {
      get
      {
        return this.homeKorp;
      }
      set
      {
        this.homeKorp = value;
      }
    }

    public virtual int Division
    {
      get
      {
        return this.division;
      }
      set
      {
        this.division = value;
      }
    }

    [Browsable(false)]
    public virtual Company Company
    {
      get
      {
        return this.company;
      }
      set
      {
        this.company = value;
      }
    }

    [Browsable(false)]
    public virtual Str Str
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

    [Browsable(false)]
    public virtual int NumEntrance
    {
      get
      {
        return this.numEntrance;
      }
      set
      {
        this.numEntrance = value;
      }
    }

    public virtual int? YearBuild
    {
      get
      {
        return this.yearBuild;
      }
      set
      {
        this.yearBuild = value;
      }
    }

    [Browsable(false)]
    public virtual Mwl Mwl
    {
      get
      {
        return this.mwl;
      }
      set
      {
        this.mwl = value;
      }
    }

    [Browsable(false)]
    public virtual int? Reu
    {
      get
      {
        return this.reu;
      }
      set
      {
        this.reu = value;
      }
    }

    [Browsable(false)]
    public virtual int HomeType
    {
      get
      {
        return this.homeType;
      }
      set
      {
        this.homeType = value;
      }
    }

    [Browsable(false)]
    public virtual short Archive
    {
      get
      {
        return this.archive;
      }
      set
      {
        this.archive = value;
      }
    }

    [Browsable(false)]
    public virtual string NumFloor { get; set; }

    public virtual string NameStr
    {
      get
      {
        if (this.str != null && this.str.NameStr != null)
          return this.str.NameStr;
        return "";
      }
    }

    public virtual string Address
    {
      get
      {
        if (this.str == null)
          return "";
        if (!(this.homeKorp != ""))
          return this.str.NameStr + " д." + this.nHome;
        return this.str.NameStr + " д." + this.nHome + " корп." + this.homeKorp;
      }
    }

    public Home()
    {
    }

    public Home(int id, string num, string korp)
    {
      this.idHome = id;
      this.nHome = num;
      if (!(korp != ""))
        return;
      this.nHome = this.nHome + " корп." + korp;
    }

    public Home(int idHome, Str str, string home, string homeKorp, int division, int? yearBuild, short company)
    {
      this.idHome = idHome;
      this.str = str;
      this.nHome = home;
      this.homeKorp = homeKorp;
      this.division = division;
      this.yearBuild = yearBuild;
      this.company = new Company(company, "");
    }
  }
}
