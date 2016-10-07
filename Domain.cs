// Decompiled with JetBrains decompiler
// Type: Kvartplata.Domain
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Windows.Forms;

namespace Kvartplata
{
  public static class Domain
  {
    private const string sessionKey = "NHib.SessionKey";
    private static ISessionFactory sessionFactory;
    private static string _connectionString;

    public static ISession CurrentSession
    {
      get
      {
        return Domain.GetSession(true);
      }
    }

    public static string ConnectionString
    {
      get
      {
        return Domain._connectionString;
      }
    }

    public static void Init()
    {
      if (Domain.sessionFactory != null)
      {
        Domain.sessionFactory.Close();
        Domain.sessionFactory = (ISessionFactory) null;
      }
      string str = "Provider=" + Options.Provider + ";eng=" + Options.BaseName + ";Links=TCPIP{" + Options.Host + "};uid=" + Options.Login + ";pwd=" + Options.Pwd;
      Domain._connectionString = str;
      Configuration configuration = new Configuration();
      configuration.Configure("hibernate.cfg.xml");
      configuration.SetProperty("connection.connection_string", str);
      try
      {
        Domain.sessionFactory = configuration.BuildSessionFactory();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Невозможно подключиться к базе!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        System.Environment.Exit(0);
      }
    }

    public static void Close()
    {
      ISession session = Domain.GetSession(false);
      if (session == null)
        return;
      session.Close();
    }

    private static ISession GetSession(bool getNewIfNotExists)
    {
      ISession session;
      if (HttpContext.Current != null)
      {
        HttpContext current = HttpContext.Current;
        session = current.Items[(object) "NHib.SessionKey"] as ISession;
        if (session == null & getNewIfNotExists)
        {
          session = Domain.sessionFactory.OpenSession();
          current.Items[(object) "NHib.SessionKey"] = (object) session;
        }
      }
      else
      {
        session = CallContext.GetData("NHib.SessionKey") as ISession;
        if (session == null & getNewIfNotExists)
        {
          session = Domain.sessionFactory.OpenSession();
          CallContext.SetData("NHib.SessionKey", (object) session);
        }
      }
      return session;
    }
  }
}
