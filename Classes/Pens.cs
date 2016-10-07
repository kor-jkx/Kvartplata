// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Pens
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class Pens
  {
    private int pensTypeId;
    private string pensTypeName;

    public virtual int PensTypeId
    {
      get
      {
        return this.pensTypeId;
      }
      set
      {
        this.pensTypeId = value;
      }
    }

    public virtual string PensTypeName
    {
      get
      {
        return this.pensTypeName;
      }
      set
      {
        this.pensTypeName = value;
      }
    }
  }
}
