// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.City
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class City
  {
    private int cityId;
    private string cityName;
    private int genId;

    public virtual int CityId
    {
      get
      {
        return this.cityId;
      }
      set
      {
        this.cityId = value;
      }
    }

    public virtual string CityName
    {
      get
      {
        return this.cityName;
      }
      set
      {
        this.cityName = value;
      }
    }

    public virtual int GenId
    {
      get
      {
        return this.genId;
      }
      set
      {
        this.genId = value;
      }
    }
  }
}
