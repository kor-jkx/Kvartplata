// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.BaseTariff
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class BaseTariff
  {
    private int baseTariff_id;
    private string baseTariff_name;

    public virtual int BaseTariff_id
    {
      get
      {
        return this.baseTariff_id;
      }
      set
      {
        if (this.baseTariff_id == value)
          return;
        this.baseTariff_id = value;
      }
    }

    public virtual string BaseTariff_name
    {
      get
      {
        return this.baseTariff_name;
      }
      set
      {
        if (this.baseTariff_name == value)
          return;
        this.baseTariff_name = value;
      }
    }
  }
}
