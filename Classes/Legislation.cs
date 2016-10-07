// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Legislation
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class Legislation
  {
    public virtual short Legislation_id { get; set; }

    public virtual string Service_name { get; set; }

    public virtual string OverShoot { get; set; }

    public virtual string Normal { get; set; }

    public virtual string Description { get; set; }

    public virtual string Measure { get; set; }

    public virtual double Standart { get; set; }

    public virtual double Percent { get; set; }
  }
}
