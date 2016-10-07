// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.DI_RULEORG
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class DI_RULEORG
  {
    private int _idrul;
    private string _namerul;

    public virtual int Idrul
    {
      get
      {
        return this._idrul;
      }
      set
      {
        this._idrul = value;
      }
    }

    public virtual string Namerul
    {
      get
      {
        return this._namerul;
      }
      set
      {
        this._namerul = value;
      }
    }

    public virtual string DI_RULEORGName
    {
      get
      {
        return this._namerul;
      }
      set
      {
        this._namerul = value;
      }
    }

    public virtual int DI_RULEORGId
    {
      get
      {
        return this._idrul;
      }
      set
      {
        this._idrul = value;
      }
    }
  }
}
