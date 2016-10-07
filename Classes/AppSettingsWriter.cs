// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.AppSettingsWriter
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.Xml;

namespace Kvartplata.Classes
{
  public class AppSettingsWriter
  {
    private string _ConfigFileName = "Kvartplata.exe.config";
    private XmlDocument _Document = (XmlDocument) null;

    public string this[string key]
    {
      set
      {
        XmlNode newChild = this._Document.DocumentElement.SelectSingleNode("/configuration/appSettings/add[@key=\"" + key + "\"]");
        if (newChild != null)
          newChild.Attributes.GetNamedItem("value").Value = value;
        else
          newChild = this._Document.CreateNode(XmlNodeType.Element, "add", "");
        XmlNode node1 = this._Document.CreateNode(XmlNodeType.Attribute, "key", "");
        node1.Value = key;
        newChild.Attributes.SetNamedItem(node1);
        XmlNode node2 = this._Document.CreateNode(XmlNodeType.Attribute, "value", "");
        node2.Value = value;
        newChild.Attributes.SetNamedItem(node2);
        XmlNode xmlNode = this._Document.DocumentElement.SelectSingleNode("/configuration/appSettings");
        if (xmlNode == null)
          throw new InvalidOperationException("Cannot append " + value + " to appSettings");
        xmlNode.AppendChild(newChild);
      }
    }

    public AppSettingsWriter(string configFileName)
    {
      this._ConfigFileName = configFileName;
      this._Document = new XmlDocument();
      this._Document.Load(this._ConfigFileName);
    }

    public void Save()
    {
      this._Document.Save(this._ConfigFileName);
    }
  }
}
