﻿// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Reg
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class Reg
  {
    public virtual int RegionId { get; set; }

    public virtual int Level { get; set; }

    public virtual int PrinRegion { get; set; }

    public virtual string RegionName { get; set; }

    public Reg()
    {
    }

    public Reg(int id, string name)
    {
      this.RegionId = id;
      this.RegionName = name;
    }
  }
}
