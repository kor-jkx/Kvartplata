// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.MSPDoc
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class MSPDoc
  {
    private short mspDocId;
    private string mspDocName;

    public virtual short MSPDocId
    {
      get
      {
        return this.mspDocId;
      }
      set
      {
        this.mspDocId = value;
      }
    }

    public virtual string MSPDocName
    {
      get
      {
        return this.mspDocName;
      }
      set
      {
        this.mspDocName = value;
      }
    }
  }
}
