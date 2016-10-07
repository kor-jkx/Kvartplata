// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.SndAddress
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class SndAddress
  {
    public virtual int SndAddressId { get; set; }

    public virtual int Account_Type { get; set; }

    public virtual int Account { get; set; }

    public virtual int Receipt_id { get; set; }

    public virtual string UploadDir { get; set; }

    public virtual string DownloadDir { get; set; }

    public virtual string EmailAdress { get; set; }

    public virtual string FTPAdress { get; set; }

    public virtual int Active { get; set; }

    public virtual bool Company { get; set; }

    public override bool Equals(object obj)
    {
      return obj != null && obj is SndAddress && this.SndAddressId == this.SndAddressId;
    }

    public override int GetHashCode()
    {
      int num = 13;
      int sndAddressId = this.SndAddressId;
      int hashCode = this.SndAddressId.GetHashCode();
      return num + hashCode;
    }

    public virtual object Clone()
    {
      return this.MemberwiseClone();
    }
  }
}
