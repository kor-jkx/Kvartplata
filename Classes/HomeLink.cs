// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.HomeLink
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  internal class HomeLink
  {
    private DateTime dBeg;
    private DateTime dEnd;
    private Home home;
    private Company company;

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

    public virtual DateTime DBeg
    {
      get
      {
        return this.dBeg;
      }
      set
      {
        this.dBeg = new DateTime();
        this.dBeg = value;
      }
    }

    public virtual DateTime DEnd
    {
      get
      {
        return this.dEnd;
      }
      set
      {
        this.dEnd = new DateTime();
        this.dEnd = value;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      HomeLink homeLink = obj as HomeLink;
      return homeLink != null && ((int) this.company.CompanyId == (int) homeLink.Company.CompanyId && this.home.IdHome == homeLink.Home.IdHome && this.dBeg == homeLink.DBeg);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int companyId = (int) this.company.CompanyId;
      int hashCode1 = this.company.CompanyId.GetHashCode();
      int num2 = num1 + hashCode1;
      int idHome = this.home.IdHome;
      int hashCode2 = this.home.IdHome.GetHashCode();
      int num3 = num2 + hashCode2;
      DateTime dBeg = this.dBeg;
      int hashCode3 = this.dBeg.GetHashCode();
      return num3 + hashCode3;
    }
  }
}
