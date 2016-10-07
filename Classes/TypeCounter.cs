// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.TypeCounter
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class TypeCounter
  {
    private short typeCounter_id;
    private string typeCounter_name;
    private short cDigit;
    private string uname;
    private DateTime dedit;

    public virtual short TypeCounter_id
    {
      get
      {
        return this.typeCounter_id;
      }
      set
      {
        if ((int) this.typeCounter_id == (int) value)
          return;
        this.typeCounter_id = value;
      }
    }

    public virtual string TypeCounter_name
    {
      get
      {
        return this.typeCounter_name;
      }
      set
      {
        if (this.typeCounter_name == value)
          return;
        this.typeCounter_name = value;
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

    public virtual short CDigit
    {
      get
      {
        return this.cDigit;
      }
      set
      {
        if ((int) this.cDigit == (int) value)
          return;
        this.cDigit = value;
      }
    }
  }
}
