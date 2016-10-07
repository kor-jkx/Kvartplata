// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Transfer
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class Transfer
  {
    private Company _company;
    private Raion _raion;
    private short? _kvrCmp;
    private short? _paspCmp;
    private short? _farmCmp;
    private short? _armyCmp;
    private string _uName;
    private DateTime _dEdit;
    private int? _ohlBeg;
    private int? _ohlEnd;

    public virtual Company Company
    {
      get
      {
        return this._company;
      }
      set
      {
        this._company = value;
      }
    }

    public virtual Raion Raion
    {
      get
      {
        return this._raion;
      }
      set
      {
        this._raion = value;
      }
    }

    public virtual short? KvrCmp
    {
      get
      {
        return this._kvrCmp;
      }
      set
      {
        this._kvrCmp = value;
      }
    }

    public virtual short? PaspCmp
    {
      get
      {
        return this._paspCmp;
      }
      set
      {
        this._paspCmp = value;
      }
    }

    public virtual short? FarmCmp
    {
      get
      {
        return this._farmCmp;
      }
      set
      {
        this._farmCmp = value;
      }
    }

    public virtual short? ArmyCmp
    {
      get
      {
        return this._armyCmp;
      }
      set
      {
        this._armyCmp = value;
      }
    }

    public virtual int? OhlBeg
    {
      get
      {
        return this._ohlBeg;
      }
      set
      {
        this._ohlBeg = value;
      }
    }

    public virtual int? OhlEnd
    {
      get
      {
        return this._ohlEnd;
      }
      set
      {
        this._ohlEnd = value;
      }
    }

    public virtual string UName
    {
      get
      {
        return this._uName;
      }
      set
      {
        this._uName = value;
      }
    }

    public virtual DateTime DEdit
    {
      get
      {
        return this._dEdit;
      }
      set
      {
        this._dEdit = value;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      Transfer transfer = obj as Transfer;
      return transfer != null && (int) this._company.CompanyId == (int) transfer.Company.CompanyId;
    }

    public override int GetHashCode()
    {
      int num = 13;
      int companyId = (int) this._company.CompanyId;
      int hashCode = this._company.CompanyId.GetHashCode();
      return num + hashCode;
    }
  }
}
