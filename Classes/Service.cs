// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Service
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Iesi.Collections;
using System;

namespace Kvartplata.Classes
{
#pragma warning disable CS0659 // Тип переопределяет Object.Equals(object o), но не переопределяет Object.GetHashCode()
    public class Service
#pragma warning restore CS0659 // Тип переопределяет Object.Equals(object o), но не переопределяет Object.GetHashCode()
    {
    private ISet childService = (ISet) new HashedSet();
    private short serviceid;
    private string servicename;
    private short? root;
    private string uname;
    private DateTime dedit;

    public virtual short ServiceId
    {
      get
      {
        return this.serviceid;
      }
      set
      {
        if ((int) this.serviceid == (int) value)
          return;
        this.serviceid = value;
      }
    }

    public virtual string ServiceName
    {
      get
      {
        return this.servicename;
      }
      set
      {
        if (this.servicename == value)
          return;
        this.servicename = value;
      }
    }

    public virtual short? Root
    {
      get
      {
        return this.root;
      }
      set
      {
        short? nullable1 = this.root;
        int? nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
        nullable1 = value;
        int? nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
        if (nullable2.GetValueOrDefault() == nullable3.GetValueOrDefault() && nullable2.HasValue == nullable3.HasValue)
          return;
        this.root = value;
      }
    }

    public virtual string Uname
    {
      get
      {
        return this.uname;
      }
      set
      {
        this.uname = value;
      }
    }

    public virtual DateTime Dedit
    {
      get
      {
        return this.dedit;
      }
      set
      {
        this.dedit = value;
      }
    }

    public virtual ISet ChildService
    {
      get
      {
        return this.childService;
      }
      set
      {
        this.childService = value;
      }
    }

    public Service()
    {
    }

    public Service(short ServiceId, string name)
    {
      this.serviceid = ServiceId;
      this.servicename = name;
      this.root = new short?((short) 0);
    }

    public Service(short ServiceId, string name, short? root)
    {
      this.serviceid = ServiceId;
      this.servicename = name;
      this.root = root;
    }

    public Service(short ServiceId, string name, short? root, ISet child)
    {
      this.serviceid = ServiceId;
      this.servicename = name;
      this.root = root;
      this.childService = child;
    }

    public Service(ISet child)
    {
      this.childService = child;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      Service service = obj as Service;
      return service != null && (int) this.ServiceId == (int) service.ServiceId;
    }
  }
}
