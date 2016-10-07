// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.DistrictHome
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class DistrictHome
  {
    public virtual int DistrictId { get; set; }

    public virtual Home Home { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      DistrictHome districtHome = obj as DistrictHome;
      return districtHome != null && (this.DistrictId == districtHome.DistrictId && this.Home.IdHome == districtHome.Home.IdHome);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int districtId = this.DistrictId;
      int num2 = this.DistrictId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int idHome = this.Home.IdHome;
      num2 = this.Home.IdHome;
      int hashCode2 = num2.GetHashCode();
      return num3 + hashCode2;
    }
  }
}
