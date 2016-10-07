// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Options
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace Kvartplata.Classes
{
  public sealed class Options
  {
    private static List<string> _hostList = new List<string>();
    private static string _docHomeUri = "http://doc.irc76.ru";
    public static string CurrentDomainPath = "";
    public static string PrefixWindow = "";
    private static int _city;
    private static int _raion;
    private static Company _company;
    private static string _baseName;
    private static string _login;
    private static string _pwd;
    private static bool _roundArea;
    private static string _alias;
    private static string _host;
    private static string _provider;
    private static string _pathProfile;
    private static string _pathProfileAppData;
    private static bool _newLogin;
    private static bool _viewEdit;
    private static short _spacingSaldo;
    private static string _sortService;
    private static bool _countersInPays;
    private static bool _showOldPays;
    private static bool _rentMsp;
    private static bool _offerSum;

    public static string DocHomeUri
    {
      get
      {
        return Options._docHomeUri;
      }
      set
      {
        Options._docHomeUri = value;
      }
    }

    public static int City
    {
      get
      {
        return Options._city;
      }
      set
      {
        Options._city = value;
      }
    }

    public static int Raion
    {
      get
      {
        return Options._raion;
      }
      set
      {
        Options._raion = value;
      }
    }

    public static Company Company
    {
      get
      {
        return Options._company;
      }
      set
      {
        Options._company = value;
      }
    }

    public static string BaseName
    {
      get
      {
        return Options._baseName;
      }
      set
      {
        Options._baseName = value;
      }
    }

    public static string Login
    {
      get
      {
        return Options._login;
      }
      set
      {
        Options._login = value;
      }
    }

    public static string Pwd
    {
      get
      {
        return Options._pwd;
      }
      set
      {
        Options._pwd = value;
      }
    }

    public static Period Period { get; set; }

    public static Complex Complex { get; set; }

    public static Complex ComplexPasp { get; set; }

    public static Complex ComplexPrior { get; set; }

    public static Complex ComplexArenda { get; set; }

    public static Complex ComplexKapRemont { get; set; }

    public static IList<Kvartplata.Classes.Proxy> Proxy { get; set; }

    public static Period MaxPeriod { get; set; }

    public static Period MinPeriod { get; set; }

    public static bool RoundArea
    {
      get
      {
        return Options._roundArea;
      }
      set
      {
        Options._roundArea = value;
      }
    }

    public static string Alias
    {
      get
      {
        return Options._alias;
      }
      set
      {
        Options._alias = value;
      }
    }

    public static string Provider
    {
      get
      {
        return Options._provider;
      }
      set
      {
        Options._provider = value;
      }
    }

    public static string Host
    {
      get
      {
        return Options._host;
      }
      set
      {
        Options._host = value;
      }
    }

    public static List<string> HostList
    {
      get
      {
        return Options._hostList;
      }
    }

    public static string PathProfile
    {
      get
      {
        return Options._pathProfile;
      }
      set
      {
        Options._pathProfile = value;
      }
    }

    public static string PathProfileAppData
    {
      get
      {
        return Options._pathProfileAppData;
      }
      set
      {
        Options._pathProfileAppData = value;
      }
    }

    public static bool NewLogin
    {
      get
      {
        return Options._newLogin;
      }
      set
      {
        Options._newLogin = value;
      }
    }

    public static bool ViewEdit
    {
      get
      {
        return Options._viewEdit;
      }
      set
      {
        Options._viewEdit = value;
      }
    }

    public static bool Arenda { get; set; }

    public static bool Kvartplata { get; set; }

    public static bool Overhaul { get; set; }

    public static short SpacingSaldo
    {
      get
      {
        return Options._spacingSaldo;
      }
      set
      {
        Options._spacingSaldo = value;
      }
    }

    public static DateTime? PeriodPay { get; set; }

    public static DateTime? MonthPay { get; set; }

    public static DateTime? DatePay { get; set; }

    public static SourcePay SourcePay { get; set; }

    public static PurposePay PurposePay { get; set; }

    public static PayDoc PayDoc { get; set; }

    public static bool Scanner { get; set; }

    public static string Packet { get; set; }

    public static bool ArrangeRent { get; set; }

    public static int PeniBalance { get; set; }

    public static int CounterDay { get; set; }

    public static string SortService
    {
      get
      {
        return Options._sortService;
      }
      set
      {
        Options._sortService = value;
      }
    }

    public static Period CounterPeriod { get; set; }

    public static bool Error { get; set; }

    public static bool ShowAnotherOrg { get; set; }

    public static string MainConditions1 { get; set; }

    public static string MainConditions2 { get; set; }

    public static string HomeType { get; set; }

    public static string Separator { get; set; }

    public static bool CountersInPays
    {
      get
      {
        return Options._countersInPays;
      }
      set
      {
        Options._countersInPays = value;
      }
    }

    public static bool ShowOldPays
    {
      get
      {
        return Options._showOldPays;
      }
      set
      {
        Options._showOldPays = value;
      }
    }

    public static bool RentMSP
    {
      get
      {
        return Options._rentMsp;
      }
      set
      {
        Options._rentMsp = value;
      }
    }

    public static bool OfferSum
    {
      get
      {
        return Options._offerSum;
      }
      set
      {
        Options._offerSum = value;
      }
    }

    public static bool CollectiveDevice { get; set; }

    public static short FillCorrectSum { get; set; }

    public static short TypeFio { get; set; }

    public static int AddressSending { get; set; }

    public static void GetPrimaryOptions()
    {
    }

    public static void GetAllOptions()
    {
      try
      {
        Options.CurrentDomainPath = AppDomain.CurrentDomain.BaseDirectory;
        WindowsIdentity.GetCurrent();
        RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Volatile Environment\\");
        Options._pathProfileAppData = registryKey1 != null ? (string) registryKey1.GetValue("APPDATA") : "";
        Options._pathProfileAppData += "\\Bit";
        if (!Directory.Exists(Options._pathProfileAppData))
          Directory.CreateDirectory(Options._pathProfileAppData);
        Options._pathProfileAppData += "\\Kvartplata";
        if (!Directory.Exists(Options._pathProfileAppData))
          Directory.CreateDirectory(Options._pathProfileAppData);
        Options._pathProfileAppData += "\\";
        if (File.Exists(Options._pathProfileAppData + "user.dat"))
        {
          StreamReader streamReader = new StreamReader(Options._pathProfileAppData + "user.dat");
          Options._login = streamReader.ReadLine();
          streamReader.Close();
        }
        else
          Options._login = "";
        Options._pathProfile = AppDomain.CurrentDomain.BaseDirectory + "Configuration\\";
        if (!Directory.Exists(Options._pathProfile))
          Directory.CreateDirectory(Options._pathProfile);
        Options._pathProfile = Options._login != "" ? Options._pathProfile + Options._login + "\\" : AppDomain.CurrentDomain.BaseDirectory;
        if (!Directory.Exists(Options._pathProfile))
          Directory.CreateDirectory(Options._pathProfile);
        if (!File.Exists(Options._pathProfile + Application.ProductName + ".exe.config"))
          File.Copy(AppDomain.CurrentDomain.BaseDirectory + Application.ProductName + ".exe.config", Options._pathProfile + Application.ProductName + ".exe.config");
        try
        {
          Options._city = Convert.ToInt32(Options.ConfigValue("city"));
        }
        catch (Exception ex)
        {
          Options._city = 1;
        }
        try
        {
          Options._raion = Convert.ToInt32(Options.ConfigValue("raion"));
        }
        catch (Exception ex)
        {
          Options._raion = 0;
        }
        try
        {
          Options._company = new Company();
          Options._company.CompanyId = Convert.ToInt16(Options.ConfigValue("company"));
        }
        catch (Exception ex)
        {
        }
        Options._baseName = Options.ConfigValue("base");
        Options._alias = Options.ConfigValue("odbcdns");
        RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\ODBC\\ODBC.INI\\" + Options._alias) ?? Registry.LocalMachine.OpenSubKey("Software\\ODBC\\ODBC.INI\\" + Options._alias);
        Options._baseName = (registryKey2 != null ? (string) registryKey2.GetValue("EngineName") : Options._baseName) ?? (registryKey2 != null ? (string) registryKey2.GetValue("ServerName") : Options._alias);
        Options._provider = registryKey2 != null ? (((string) registryKey2.GetValue("Driver")).IndexOf("dbodbc10.dll") != -1 ? "SAOLEDB.10" : "ASAProv") : "ASAProv";
        Options._host = Options.ConfigValue("host");
        if (Options._host != "")
        {
          Options._host = "host=" + Options._host;
          Options._hostList.Add(Options._host);
        }
        else
          Options._hostList.Add("");
        for (int index = 1; index != -1; ++index)
        {
          Options._host = Options.ConfigValueEmpty("host" + (object) index);
          if (Options._host != null && Options._host != "")
          {
            Options._host = "host=" + Options._host;
            Options._hostList.Add(Options._host);
          }
          if (Options._host == null)
            index = -2;
        }
        if (Options.ConfigValue("round_area") != "")
          Options._roundArea = Convert.ToBoolean(Options.ConfigValue("round_area"));
        try
        {
          Options._sortService = Convert.ToInt32(Options.ConfigValue("sort_service")) != 1 ? " s.ServiceId" : " s.ServiceName";
        }
        catch (Exception ex)
        {
          Options._sortService = " s.ServiceId";
        }
        try
        {
          Options._viewEdit = Convert.ToInt32(Options.ConfigValue("view_edit")) == 1;
        }
        catch (Exception ex)
        {
          Options._viewEdit = false;
        }
        Options._spacingSaldo = !(Options.ConfigValue("spacing_saldo") != "") ? (short) 0 : Convert.ToInt16(Options.ConfigValue("spacing_saldo"));
        try
        {
          Options.Separator = !(Options.ConfigValue("separator") != "") ? KvrplHelper.ChangeSeparator(".") : Options.ConfigValue("separator");
        }
        catch (Exception ex)
        {
          Options.Separator = KvrplHelper.ChangeSeparator(".");
        }
        if (Options.ConfigValue("counters_in_pays") != "")
          Options._countersInPays = Convert.ToBoolean(Options.ConfigValue("counters_in_pays"));
        if (Options.ConfigValue("show_old_pays") != "")
          Options._showOldPays = Convert.ToBoolean(Options.ConfigValue("show_old_pays"));
        if (Options.ConfigValue("rent_msp") != "")
          Options._rentMsp = Convert.ToBoolean(Options.ConfigValue("rent_msp"));
        if (Options.ConfigValue("offer_sum") != "")
          Options._offerSum = Convert.ToBoolean(Options.ConfigValue("offer_sum"));
        if (Options.ConfigValue("fill_correctsum") != "")
          Options.FillCorrectSum = Convert.ToInt16(Options.ConfigValue("fill_correctsum"));
        Options.AddressSending = !(Options.ConfigValue("address_sending") != "") ? 0 : (int) Convert.ToInt16(Options.ConfigValue("address_sending"));
        Options.PrefixWindow = Options.ConfigValue("prefix_window");
        Options._docHomeUri = Options.ConfigValue("doc_home_uri");
        if (!(Options._docHomeUri == ""))
          return;
        Options.AddNewConfigValue("doc_home_uri", "http://doc.irc76.ru", true);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void SaveConnectOptions()
    {
      try
      {
        Options._pathProfile = Options._pathProfile == AppDomain.CurrentDomain.BaseDirectory || Options._newLogin ? AppDomain.CurrentDomain.BaseDirectory + "Configuration\\" + Options._login + "\\" : Options._pathProfile;
        if (!Directory.Exists(Options._pathProfile))
          Directory.CreateDirectory(Options._pathProfile);
        StreamWriter streamWriter = new StreamWriter(Options._pathProfileAppData + "user.dat", false, Encoding.Default);
        streamWriter.WriteLine(Options._login);
        streamWriter.Close();
        if (!File.Exists(Options._pathProfile + Application.ProductName + ".exe.config"))
          File.Copy(AppDomain.CurrentDomain.BaseDirectory + Application.ProductName + ".exe.config", Options._pathProfile + Application.ProductName + ".exe.config");
        if (Options._newLogin)
          Options.GetAllOptions();
        Options.ChangeConfigValue("base", Options._baseName, true);
        Options.ChangeConfigValue("login", Options._login, true);
        Options.ChangeConfigValue("pwd", Options._pwd, true);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void SaveAllOptions()
    {
      Options.ChangeConfigValue("base", Options._baseName, true);
      Options.ChangeConfigValue("login", Options._login, true);
      Options.ChangeConfigValue("city", Options._city.ToString(), true);
      Options.ChangeConfigValue("raion", Options._raion.ToString(), true);
      Options.ChangeConfigValue("company", Options._company.ToString(), true);
      ConfigurationManager.RefreshSection("connectionStrings");
    }

    public static string ConfigValue(string name)
    {
      try
      {
        return ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap() { ExeConfigFilename = Options._pathProfile + "\\" + Application.ProductName + ".exe.config" }, ConfigurationUserLevel.None).AppSettings.Settings[name].Value;
      }
      catch
      {
        return "";
      }
    }

    public static string ConfigValueEmpty(string name)
    {
      try
      {
        return ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap() { ExeConfigFilename = Options._pathProfile + "\\" + Application.ProductName + ".exe.config" }, ConfigurationUserLevel.None).AppSettings.Settings[name].Value;
      }
      catch
      {
        return (string) null;
      }
    }

    public static bool ChangeConfigValue(string name, string value, bool priznak)
    {
      try
      {
        System.Configuration.Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap() { ExeConfigFilename = Options._pathProfile + "\\" + Application.ProductName + ".exe.config" }, ConfigurationUserLevel.None);
        if (priznak)
          configuration.AppSettings.Settings[name].Value = value;
        else
          configuration.ConnectionStrings.ConnectionStrings["Sybase"].ConnectionString = value;
        configuration.Save();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static bool AddNewConfigValue(string name, string value, bool priznak)
    {
      try
      {
        System.Configuration.Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap() { ExeConfigFilename = Options._pathProfile + "\\" + Application.ProductName + ".exe.config" }, ConfigurationUserLevel.None);
        configuration.AppSettings.Settings.Add(name, value);
        configuration.Save(ConfigurationSaveMode.Modified, true);
        ConfigurationManager.RefreshSection("appSettings");
      }
      catch
      {
        return false;
      }
      return true;
    }

    private static Options.RegFindValue RegFind(RegistryKey key, string find)
    {
      if (key == null || string.IsNullOrEmpty(find))
        return (Options.RegFindValue) null;
      string[] valueNames = key.GetValueNames();
      if ((uint) valueNames.Length > 0U)
      {
        foreach (string str in valueNames)
        {
          if (str.IndexOf(find, StringComparison.OrdinalIgnoreCase) != -1)
            return new Options.RegFindValue(key, str, (string) null, Options.RegFindIn.Property);
          object obj = key.GetValue(str, (object) null, RegistryValueOptions.DoNotExpandEnvironmentNames);
          if (obj is string && ((string) obj).IndexOf(find, StringComparison.OrdinalIgnoreCase) != -1)
            return new Options.RegFindValue(key, str, (string) obj, Options.RegFindIn.Value);
        }
      }
      string[] subKeyNames = key.GetSubKeyNames();
      Options.RegFindValue regFindValue = (Options.RegFindValue) null;
      if ((uint) subKeyNames.Length > 0U)
      {
        foreach (string name in subKeyNames)
        {
          try
          {
            regFindValue = Options.RegFind(key.OpenSubKey(name, RegistryKeyPermissionCheck.ReadSubTree), find);
          }
          catch (Exception ex)
          {
          }
          if (regFindValue != null)
            return regFindValue;
        }
      }
      key.Close();
      return (Options.RegFindValue) null;
    }

    private enum RegFindIn
    {
      Property,
      Value,
    }

    private class RegFindValue
    {
      private string mProps;
      private string mVal;
      private Options.RegFindIn mWhereFound;
      private RegistryKey regKey;

      public RegistryKey Key
      {
        get
        {
          return this.regKey;
        }
      }

      public string Property
      {
        get
        {
          return this.mProps;
        }
      }

      public string Value
      {
        get
        {
          return this.mVal;
        }
      }

      public Options.RegFindIn WhereFound
      {
        get
        {
          return this.mWhereFound;
        }
      }

      public RegFindValue(RegistryKey key, string props, string val, Options.RegFindIn where)
      {
        this.regKey = key;
        this.mProps = props;
        this.mVal = val;
        this.mWhereFound = where;
      }
    }
  }
}
