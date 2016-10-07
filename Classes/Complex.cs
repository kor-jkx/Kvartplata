// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Complex
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class Complex
  {
    private int idFk;
    private int complexId;
    private string complexName;

    public virtual int IdFk
    {
      get
      {
        return this.idFk;
      }
      set
      {
        this.idFk = value;
      }
    }

    public virtual int ComplexId
    {
      get
      {
        return this.complexId;
      }
      set
      {
        this.complexId = value;
      }
    }

    public virtual string ComplexName { get; set; }

    public Complex()
    {
    }

    public Complex(int id, string name)
    {
      this.complexId = id;
      this.IdFk = id;
      this.ComplexName = name;
    }
  }
}
