// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.TypeSeal
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class TypeSeal
  {
    private int typeSealId;
    private string typeSealName;
    private string uname;
    private DateTime dedit;

    public virtual int TypeSealId
    {
      get
      {
        return this.typeSealId;
      }
      set
      {
        if (this.typeSealId == value)
          return;
        this.typeSealId = value;
      }
    }

    public virtual string TypeSealName
    {
      get
      {
        return this.typeSealName;
      }
      set
      {
        if (this.typeSealName == value)
          return;
        this.typeSealName = value;
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

    public TypeSeal()
    {
    }

    public TypeSeal(int typeSealId, string typeSealName)
    {
      this.typeSealId = typeSealId;
      this.typeSealName = typeSealName;
    }
  }
}
