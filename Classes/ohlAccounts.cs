// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.ohlAccounts
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class ohlAccounts
  {
    private BaseOrg _ownerAccount;
    private Bank _bank;
    private string _account;
    private int _ohlAccountsId;
    private dcohlTypeAccount _typeAccount;
    private int _code_sbrf;
    private int _complexId;

    public virtual int ComplexId
    {
      get
      {
        return this._complexId;
      }
      set
      {
        this._complexId = value;
      }
    }

    public virtual int CodeSbrf
    {
      get
      {
        return this._code_sbrf;
      }
      set
      {
        this._code_sbrf = value;
      }
    }

    public virtual int ohlAccountsId
    {
      get
      {
        return this._ohlAccountsId;
      }
      set
      {
        this._ohlAccountsId = value;
      }
    }

    public virtual string Account
    {
      get
      {
        return this._account;
      }
      set
      {
        this._account = value;
      }
    }

    [Browsable(false)]
    public virtual Bank Bank
    {
      get
      {
        return this._bank;
      }
      set
      {
        this._bank = value;
      }
    }

    [Browsable(false)]
    public virtual BaseOrg OwnerAccount
    {
      get
      {
        return this._ownerAccount;
      }
      set
      {
        this._ownerAccount = value;
      }
    }

    [Browsable(false)]
    public virtual dcohlTypeAccount TypeAccount
    {
      get
      {
        return this._typeAccount;
      }
      set
      {
        this._typeAccount = value;
      }
    }

    public virtual string ohlAccountsName
    {
      get
      {
        return this.Account + " " + this.Bank.BankName + " " + this.TypeAccount.TypeAccountName;
      }
    }

    public virtual string UName { get; set; }

    public virtual DateTime DEdit { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      ohlAccounts ohlAccounts = obj as ohlAccounts;
      return ohlAccounts != null && this.ohlAccountsId == ohlAccounts.ohlAccountsId;
    }

    public override int GetHashCode()
    {
      int num = 13;
      int ohlAccountsId = this.ohlAccountsId;
      int hashCode = this.ohlAccountsId.GetHashCode();
      return num + hashCode;
    }
  }
}
