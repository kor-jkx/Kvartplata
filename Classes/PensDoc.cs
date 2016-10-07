// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.PensDoc
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class PensDoc
  {
    private int idPensDoc;
    private Person person;
    private Pens pens;
    private DateTime dBeg;
    private DateTime dEnd;
    private string seriaPens;
    private DateTime? datePens;
    private string outPens;
    private string uname;
    private DateTime dedit;

    public virtual int IdPensDoc
    {
      get
      {
        return this.idPensDoc;
      }
      set
      {
        this.idPensDoc = value;
      }
    }

    public virtual Person Person
    {
      get
      {
        return this.person;
      }
      set
      {
        this.person = value;
      }
    }

    public virtual Pens Pens
    {
      get
      {
        return this.pens;
      }
      set
      {
        this.pens = value;
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

    public virtual string SeriaPens
    {
      get
      {
        return this.seriaPens;
      }
      set
      {
        this.seriaPens = value;
      }
    }

    public virtual DateTime? DatePens
    {
      get
      {
        return this.datePens;
      }
      set
      {
        this.datePens = new DateTime?(new DateTime());
        this.datePens = value;
      }
    }

    public virtual string OutPens
    {
      get
      {
        return this.outPens;
      }
      set
      {
        this.outPens = value;
      }
    }

    public virtual string Uname
    {
      get
      {
        return this.uname;
      }
      set
      {
        this.uname = value;
      }
    }

    public virtual DateTime Dedit
    {
      get
      {
        return this.dedit;
      }
      set
      {
        this.dedit = new DateTime();
        this.dedit = value;
      }
    }
  }
}
