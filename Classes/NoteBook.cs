// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.NoteBook
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class NoteBook
  {
    [Browsable(false)]
    public virtual int NoteId { get; set; }

    [Browsable(false)]
    public virtual Company Company { get; set; }

    [Browsable(false)]
    public virtual int IdHome { get; set; }

    [Browsable(false)]
    public virtual int ClientId { get; set; }

    [Browsable(false)]
    public virtual DateTime DBeg { get; set; }

    [Browsable(false)]
    public virtual DateTime DEnd { get; set; }

    public virtual string Text { get; set; }

    public virtual string Note { get; set; }

    public virtual string UName { get; set; }

    public virtual DateTime DEdit { get; set; }

    [Browsable(false)]
    public virtual int OldHashCode { get; set; }

    [Browsable(false)]
    public virtual bool IsEdit { get; set; }

    [Browsable(false)]
    public virtual TypeNoteBook TypeNoteBook { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      NoteBook noteBook = obj as NoteBook;
      return noteBook != null && (this.ClientId == noteBook.ClientId && this.NoteId == noteBook.NoteId && this.IdHome == noteBook.IdHome && (int) this.Company.CompanyId == (int) noteBook.Company.CompanyId);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int companyId = (int) this.Company.CompanyId;
      int hashCode1 = this.Company.CompanyId.GetHashCode();
      int num2 = num1 + hashCode1;
      int idHome = this.IdHome;
      int hashCode2 = this.IdHome.GetHashCode();
      int num3 = num2 + hashCode2;
      int clientId = this.ClientId;
      int hashCode3 = this.ClientId.GetHashCode();
      int num4 = num3 + hashCode3;
      int noteId = this.NoteId;
      int hashCode4 = this.NoteId.GetHashCode();
      return num4 + hashCode4;
    }
  }
}
