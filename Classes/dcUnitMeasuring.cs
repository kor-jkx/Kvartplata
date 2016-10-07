// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.dcUnitMeasuring
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class dcUnitMeasuring
  {
    private int unitMeasuring_id;
    private string unitMeasuring_name;

    public virtual int UnitMeasuring_id
    {
      get
      {
        return this.unitMeasuring_id;
      }
      set
      {
        if (this.unitMeasuring_id == value)
          return;
        this.unitMeasuring_id = value;
      }
    }

    public virtual string UnitMeasuring_name
    {
      get
      {
        return this.unitMeasuring_name;
      }
      set
      {
        if (this.unitMeasuring_name == value)
          return;
        this.unitMeasuring_name = value;
      }
    }
  }
}
