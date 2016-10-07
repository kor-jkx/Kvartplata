// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Proxy
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class Proxy
  {
    private string userName;
    private Operation operation;
    private Company company;
    private int areal;
    private int proxyOpr;
    private string uName;
    private DateTime dEdit;

    public virtual string UserName
    {
      get
      {
        return this.userName;
      }
      set
      {
        this.userName = value;
      }
    }

    public virtual Operation Operation
    {
      get
      {
        return this.operation;
      }
      set
      {
        this.operation = value;
      }
    }

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

    public virtual int Areal
    {
      get
      {
        return this.areal;
      }
      set
      {
        this.areal = value;
      }
    }

    public virtual int ProxyOpr
    {
      get
      {
        return this.proxyOpr;
      }
      set
      {
        this.proxyOpr = value;
      }
    }

    public virtual string UName
    {
      get
      {
        return this.uName;
      }
      set
      {
        this.uName = value;
      }
    }

    public virtual DateTime DEdit
    {
      get
      {
        return this.dEdit;
      }
      set
      {
        this.dEdit = value;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      Proxy proxy = obj as Proxy;
      return proxy != null && (this.userName == proxy.UserName && this.operation.OprId == proxy.Operation.OprId && (int) this.company.CompanyId == (int) proxy.Company.CompanyId);
    }

    public override int GetHashCode()
    {
      int num1 = 13 + (this.userName == null ? 0 : this.userName.GetHashCode());
      int oprId = this.operation.OprId;
      int hashCode1 = this.operation.OprId.GetHashCode();
      int num2 = num1 + hashCode1;
      int companyId = (int) this.company.CompanyId;
      int hashCode2 = this.company.CompanyId.GetHashCode();
      return num2 + hashCode2;
    }
  }
}
