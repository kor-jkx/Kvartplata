﻿// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.FlatCategory
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class FlatCategory
  {
    private short idCat;
    private string category;

    public virtual short FlatCategoryId
    {
      get
      {
        return this.idCat;
      }
      set
      {
        this.idCat = value;
      }
    }

    public virtual string FlatCategoryName
    {
      get
      {
        return this.category;
      }
      set
      {
        this.category = value;
      }
    }
  }
}
