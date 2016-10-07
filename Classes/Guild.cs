// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Guild
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Guild
  {
    public virtual int GuildId { get; set; }

    public virtual string GuildName { get; set; }

    [Browsable(false)]
    public virtual BaseOrg BaseOrg { get; set; }

    [Browsable(false)]
    public virtual string UName { get; set; }

    [Browsable(false)]
    public virtual DateTime DEdit { get; set; }

    [Browsable(false)]
    public virtual bool IsEdit { get; set; }

    public Guild()
    {
    }

    public Guild(int id, string name)
    {
      this.GuildId = id;
      this.GuildName = name;
    }
  }
}
