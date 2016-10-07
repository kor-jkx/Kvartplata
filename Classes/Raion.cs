// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Raion
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class Raion
  {
    private int idRnn;
    private string rnn;

    public virtual int IdRnn
    {
      get
      {
        return this.idRnn;
      }
      set
      {
        this.idRnn = value;
      }
    }

    public virtual string Rnn
    {
      get
      {
        return this.rnn;
      }
      set
      {
        this.rnn = value;
      }
    }
  }
}
