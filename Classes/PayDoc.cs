﻿// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.PayDoc
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class PayDoc
  {
    public virtual short PayDocId { get; set; }

    public virtual string PayDocName { get; set; }

    public virtual string UName { get; set; }

    public virtual DateTime DEdit { get; set; }
  }
}
