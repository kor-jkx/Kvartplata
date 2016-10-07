// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.RightDoc
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class RightDoc
  {
    private short rightDocId;
    private string rightDocName;
    private string uname;
    private DateTime dedit;

    public virtual short RightDocId
    {
      get
      {
        return this.rightDocId;
      }
      set
      {
        if ((int) this.rightDocId == (int) value)
          return;
        this.rightDocId = value;
      }
    }

    public virtual string RightDocName
    {
      get
      {
        return this.rightDocName;
      }
      set
      {
        if (this.rightDocName == value)
          return;
        this.rightDocName = value;
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
        if (this.uname == value)
          return;
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
        if (this.dedit == value)
          return;
        this.dedit = value;
      }
    }
  }
}
