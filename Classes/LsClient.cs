// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.LsClient
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class LsClient
  {
    private int clientId;
    private string fio;
    private string surFlat;
    private Company company;
    private Home home;
    private Flat flat;
    private string uname;
    private DateTime dedit;
    private int? oldId;
    private string family;
    private string name;
    private string fName;
    private string phone;
    private short? floor;
    private short? entrance;
    private string remark;
    private string note;
    private Complex complex;

    public virtual int ClientId
    {
      get
      {
        return this.clientId;
      }
      set
      {
        this.clientId = value;
      }
    }

    public virtual string Fio
    {
      get
      {
        return this.fio;
      }
      set
      {
        this.fio = value;
      }
    }

    public virtual int? OldId
    {
      get
      {
        return this.oldId;
      }
      set
      {
        this.oldId = value;
      }
    }

    public virtual string Family
    {
      get
      {
        return this.family;
      }
      set
      {
        this.family = value;
      }
    }

    public virtual string Name
    {
      get
      {
        return this.name;
      }
      set
      {
        this.name = value;
      }
    }

    public virtual string FName
    {
      get
      {
        return this.fName;
      }
      set
      {
        this.fName = value;
      }
    }

    public virtual string Phone
    {
      get
      {
        return this.phone;
      }
      set
      {
        this.phone = value;
      }
    }

    public virtual short? Floor
    {
      get
      {
        return this.floor;
      }
      set
      {
        this.floor = value;
      }
    }

    public virtual short? Entrance
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

    public virtual string Remark
    {
      get
      {
        return this.remark;
      }
      set
      {
        this.remark = value;
      }
    }

    [Browsable(false)]
    public virtual string Note
    {
      get
      {
        return this.note;
      }
      set
      {
        this.note = value;
      }
    }

    [Browsable(false)]
    public virtual Flat Flat
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

    public virtual string NFlat
    {
      get
      {
        return this.flat.NFlat;
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
    public virtual Complex Complex
    {
      get
      {
        return this.complex;
      }
      set
      {
        this.complex = value;
      }
    }

    [Browsable(false)]
    public virtual string Locality { get; set; }

    [Browsable(false)]
    public virtual string DogovorNum { get; set; }

    public virtual string SmallAddress
    {
      get
      {
        string str1 = this.clientId.ToString();
        if (this.DogovorNum != null && this.DogovorNum != "")
          str1 = str1 + " (" + this.DogovorNum + ")";
        string str2;
        if (this.surFlat != null && this.surFlat != "0" && this.surFlat != "" && this.surFlat != " ")
          str2 = str1 + "   кв." + this.flat.NFlat + " комн." + this.surFlat;
        else
          str2 = str1 + "   кв." + this.flat.NFlat;
        if (Options.City == 4)
        {
          for (int index = this.flat.NFlat.Length + this.surFlat.Length; index <= 7; ++index)
            str2 += " ";
          str2 += KvrplHelper.GetFio1(this.clientId);
        }
        return str2;
      }
    }

    public virtual string FullAddress { get; set; }

    [Browsable(false)]
    public virtual string Uname
    {
      get
      {
        return this.uname;
      }
      set
      {
        if (this.uname == value)
          return;
        this.uname = value;
      }
    }

    [Browsable(false)]
    public virtual DateTime Dedit
    {
      get
      {
        return this.dedit;
      }
      set
      {
        if (this.dedit == value)
          return;
        this.dedit = value;
      }
    }

    public virtual int Status { get; set; }

    public LsClient()
    {
    }

    public LsClient(int clientId, string surFlat, string nFlat, string fio)
    {
      this.clientId = clientId;
      this.surFlat = surFlat;
      this.fio = fio;
    }

    public LsClient(int clientId, string surFlat, string nFlat, string family, string name, string fName)
    {
      this.clientId = clientId;
      this.family = family;
      this.name = name;
      this.fName = fName;
      this.surFlat = surFlat;
    }

    public LsClient(int clientId, string surFlat, Company company, Home home, Flat flat, string locality, Complex complex)
    {
      this.clientId = clientId;
      this.surFlat = surFlat;
      this.company = company;
      this.home = home;
      this.flat = flat;
      this.Locality = locality;
      this.complex = complex;
      if (complex.ComplexId == Options.ComplexArenda.ComplexId)
      {
        if (locality != "" && surFlat != "")
          this.surFlat = locality + " комн." + surFlat;
        else
          this.surFlat = locality + surFlat;
      }
      else
        this.surFlat = surFlat;
    }

    public LsClient(int clientId, string surFlat, Flat flat, string dogovorNum, Complex complex)
    {
      this.clientId = clientId;
      this.surFlat = surFlat;
      this.flat = flat;
      this.DogovorNum = dogovorNum;
      this.complex = complex;
      this.surFlat = surFlat;
    }

    public LsClient(int clientId, Flat flat)
    {
      this.clientId = clientId;
      this.flat = flat;
    }

    public LsClient(int clientId, Flat flat, string surFlat)
    {
      this.clientId = clientId;
      this.flat = flat;
      this.surFlat = surFlat;
    }

    public virtual string GetStrFlat()
    {
      return string.Format("{0}      {1}             {2}", (object) this.clientId, (object) this.flat.NFlat, (object) this.surFlat);
    }
  }
}
