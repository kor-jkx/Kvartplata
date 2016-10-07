// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Pers
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class Pers
  {
    public virtual short Code { get; set; }

    public virtual int Id { get; set; }

    public virtual string Family { get; set; }

    public virtual string Name { get; set; }

    public virtual string LastName { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      Pers pers = obj as Pers;
      return pers != null && ((int) this.Code == (int) pers.Code && this.Id == pers.Id);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int code = (int) this.Code;
      int hashCode1 = this.Code.GetHashCode();
      int num2 = num1 + hashCode1;
      int id = this.Id;
      int hashCode2 = this.Id.GetHashCode();
      return num2 + hashCode2;
    }
  }
}
