// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.MspDocument
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class MspDocument
  {
    private int mspDocumentId;
    private Person person;
    private MSPDoc mspDoc;
    private string series;
    private string number;
    private string source;
    private DateTime dateIssue;
    private string picPath;
    private string uname;
    private DateTime dedit;

    public virtual int MSPDocumentId
    {
      get
      {
        return this.mspDocumentId;
      }
      set
      {
        this.mspDocumentId = value;
      }
    }

    public virtual MSPDoc MSPDoc
    {
      get
      {
        return this.mspDoc;
      }
      set
      {
        this.mspDoc = value;
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

    public virtual string Series
    {
      get
      {
        return this.series;
      }
      set
      {
        this.series = value;
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

    public virtual string Source
    {
      get
      {
        return this.source;
      }
      set
      {
        this.source = value;
      }
    }

    public virtual DateTime DateIssue
    {
      get
      {
        return this.dateIssue;
      }
      set
      {
        this.dateIssue = value;
      }
    }

    public virtual string PicPath
    {
      get
      {
        return this.picPath;
      }
      set
      {
        this.picPath = value;
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
        this.dedit = DateTime.Now.Date;
      }
    }

    public virtual string MSPDocName
    {
      get
      {
        return this.mspDoc.MSPDocName;
      }
    }
  }
}
