﻿// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.TypeArendaDocument
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class TypeArendaDocument
  {
    public virtual int TypeDocument_id { get; set; }

    public virtual string TypeDocument_name { get; set; }

    public virtual string Uname { get; set; }

    public virtual DateTime Dedit { get; set; }

    public TypeArendaDocument()
    {
    }

    public TypeArendaDocument(int id, string name)
    {
      this.TypeDocument_id = id;
      this.TypeDocument_name = name;
    }
  }
}