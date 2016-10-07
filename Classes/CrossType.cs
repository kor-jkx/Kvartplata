// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.CrossType
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class CrossType
  {
    public virtual short CrossTypeId { get; set; }

    public virtual string CrossTypeName { get; set; }

    public virtual string UName { get; set; }

    public virtual DateTime DEdit { get; set; }

    public CrossType()
    {
    }

    public CrossType(short crossTypeId, string crossTypeName)
    {
      this.CrossTypeId = crossTypeId;
      this.CrossTypeName = crossTypeName;
    }
  }
}
