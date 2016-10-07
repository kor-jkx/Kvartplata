// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.KvrplHelper
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using GenUinKv;
using Kvartplata.Forms;
using Kvartplata.Smirnov;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Kvartplata.Classes
{
  public sealed class KvrplHelper
  {
    public static Logger log = LogManager.GetCurrentClassLogger();

    [DllImport("snhdd.dll", CharSet = CharSet.Ansi, SetLastError = true)]
    private static extern string SerialNumber();

    [DllImport("sndhdd.dll", EntryPoint = "GetIdeSN", CharSet = CharSet.Ansi, SetLastError = true)]
    private static extern void GetIdeSn(ref string s);

    public static string DateToBaseFormat(DateTime dt)
    {
      return string.Format("{0}-{1}-{2}", (object) dt.Year, (object) dt.Month, (object) dt.Day);
    }

    public static string DateTimeToBaseFormat(DateTime dt)
    {
      return string.Format("{0}-{1}-{2} {3}:{4}:{5}.{6}", (object) dt.Year, (object) dt.Month, (object) dt.Day, (object) dt.Hour, (object) dt.Minute, (object) dt.Second, (object) dt.Millisecond);
    }

    public static string ChangeSeparator(string smBuf)
    {
      string str1 = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToString();
      string str2 = "";
      if (str1 == ".")
        str2 = ",";
      else if (str1 == ",")
        str2 = ".";
      if ((uint) smBuf.Length <= 0U || smBuf.IndexOf(str2) == -1)
        return smBuf;
      int startIndex = smBuf.IndexOf(str2);
      smBuf = smBuf.Remove(startIndex, 1);
      return smBuf.Insert(startIndex, str1);
    }

    public static string ChangeSeparator(string smBuf, string separator)
    {
      string str = "";
      if (separator == ".")
        str = ",";
      else if (separator == ",")
        str = ".";
      if ((uint) smBuf.Length <= 0U || smBuf.IndexOf(str) == -1)
        return smBuf;
      int startIndex = smBuf.IndexOf(str);
      smBuf = smBuf.Remove(startIndex, 1);
      return smBuf.Insert(startIndex, separator);
    }

    public static string DecimalSeparator()
    {
      return CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
    }

    public static DateTime FirstDay(DateTime dt)
    {
      return Convert.ToDateTime(string.Format("{0}.{1}.{2}", (object) 1, (object) dt.Month, (object) dt.Year));
    }

    public static DateTime LastDay(DateTime dt)
    {
      return Convert.ToDateTime(string.Format("{0}.{1}.{2}", (object) DateTime.DaysInMonth(dt.Year, dt.Month), (object) dt.Month, (object) dt.Year));
    }

    public static LsClient FindLs(int clientId)
    {
      ISession currentSession = Domain.CurrentSession;
      IList list1 = currentSession.CreateQuery(string.Format("from LsClient ls where ClientId={0}" + Options.MainConditions1, (object) clientId)).List();
      currentSession.Clear();
      if ((uint) list1.Count > 0U)
        return (LsClient) list1[0];
      IList list2 = currentSession.CreateQuery(string.Format("from LsClient ls where OldId={0}" + Options.MainConditions1, (object) clientId)).List();
      if ((uint) list2.Count > 0U)
        return (LsClient) list2[0];
      return (LsClient) null;
    }

    public static LsClient FindLsByOldId(int clientId)
    {
      ISession currentSession = Domain.CurrentSession;
      IList list = currentSession.CreateCriteria(typeof (LsClient)).Add((ICriterion) Restrictions.Eq("OldId", (object) clientId)).List();
      currentSession.Clear();
      if ((uint) list.Count > 0U)
        return (LsClient) list[0];
      return (LsClient) null;
    }

    public static Period GetKvrClose(int clientId, Complex complex1, Complex complex2)
    {
      ISession currentSession = Domain.CurrentSession;
      IList list1 = currentSession.CreateCriteria(typeof (LsClient)).Add((ICriterion) Restrictions.Eq("ClientId", (object) clientId)).CreateCriteria("Company").List();
      IList<Period> periodList = currentSession.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) Convert.ToDateTime("2007.12.01"))).List<Period>();
      if ((uint) list1.Count <= 0U)
        return periodList[0];
      Company company = ((LsClient) list1[0]).Company;
      currentSession.CreateCriteria(typeof (CompanyPeriod)).Add((ICriterion) Restrictions.Eq("Company", (object) company)).Add((ICriterion) Restrictions.Eq("Complex", (object) complex1)).List();
      IList list2 = complex2 != null ? currentSession.CreateQuery(string.Format("select max(Period.PeriodId) from CompanyPeriod where Company.CompanyId={0} and (Complex.IdFk={1} or Complex.IdFk={2})", (object) company.CompanyId, (object) complex1.IdFk, (object) complex2.IdFk)).List() : currentSession.CreateQuery(string.Format("select Period.PeriodId from CompanyPeriod where Company.CompanyId={0} and Complex.IdFk={1}", (object) company.CompanyId, (object) complex1.IdFk)).List();
      currentSession.Clear();
      if (list2.Count == 0 || (uint) Convert.ToInt32(list2[0]) <= 0U)
        return periodList[0];
      Period period = new Period();
      return currentSession.Get<Period>(list2[0]);
    }

    public static Period GetNextPeriod(Period period)
    {
      ISession currentSession = Domain.CurrentSession;
      ICriteria criteria = currentSession.CreateCriteria(typeof (Period));
      criteria.Add((ICriterion) Restrictions.Eq("PeriodName", (object) period.PeriodName.Value.AddMonths(1)));
      IList list = criteria.List();
      currentSession.Clear();
      if ((uint) list.Count > 0U)
        return (Period) list[0];
      return (Period) null;
    }

    public static Period GetPrevPeriod(Period period)
    {
      ISession currentSession = Domain.CurrentSession;
      ICriteria criteria = currentSession.CreateCriteria(typeof (Period));
      criteria.Add((ICriterion) Restrictions.Eq("PeriodName", (object) period.PeriodName.Value.AddMonths(-1)));
      IList list = criteria.List();
      currentSession.Clear();
      if ((uint) list.Count > 0U)
        return (Period) list[0];
      return (Period) null;
    }

    public static Period GetPeriod(DateTime PeriodName)
    {
      ISession currentSession = Domain.CurrentSession;
      ICriteria criteria = currentSession.CreateCriteria(typeof (Period));
      criteria.Add((ICriterion) Restrictions.Eq("PeriodName", (object) PeriodName));
      IList list = criteria.List();
      currentSession.Clear();
      if ((uint) list.Count > 0U)
        return (Period) list[0];
      return (Period) null;
    }

    public static Period SaveCurrentPeriod(DateTime currentPeriod)
    {
      ISession currentSession = Domain.CurrentSession;
      IList list = currentSession.CreateQuery(string.Format("from Period where PeriodName<='{0}' and PeriodName>'{1}'", (object) KvrplHelper.DateToBaseFormat(currentPeriod), (object) KvrplHelper.DateToBaseFormat(currentPeriod.AddMonths(-1)))).List();
      currentSession.Clear();
      if ((uint) list.Count > 0U)
        return (Period) list[0];
      return (Period) null;
    }

    public static DateTime GetLastDayPeriod(DateTime period)
    {
      try
      {
        return new DateTime(period.Year, period.Month, DateTime.DaysInMonth(period.Year, period.Month));
      }
      catch
      {
        return new DateTime(period.Year, period.Month, 31);
      }
    }

    public static Period GetCmpKvrClose(Company cmp, int complex1, int complex2)
    {
      ISession currentSession = Domain.CurrentSession;
      IList<Period> periodList = currentSession.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) Convert.ToDateTime("2007.12.01"))).List<Period>();
      IList list = currentSession.CreateQuery(string.Format("select max(Period.PeriodId) from CompanyPeriod where Company.CompanyId={0} and (Complex.IdFk={1} or Complex.IdFk={2})", (object) cmp.CompanyId, (object) complex1, (object) complex2)).List();
      if (list.Count != 0 && (uint) Convert.ToInt32(list[0]) > 0U)
        return currentSession.Get<Period>(list[0]);
      return periodList[0];
    }

    public static void AddRow(DataTable table, int id, string name)
    {
      DataRow row = table.NewRow();
      row[0] = (object) id;
      row[1] = (object) name;
      table.Rows.Add(row);
    }

    public static int FindCurObject(IList list, object curObject)
    {
      int num = 0;
      foreach (object obj in (IEnumerable) list)
      {
        if (obj.Equals(curObject))
          return num;
        ++num;
      }
      return -1;
    }

    public static int GenLsClient(int idtype, int idcity, int codeu, int idhome, int idrnn, int idfond)
    {
      ISession currentSession = Domain.CurrentSession;
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      string str1 = "";
      string str2 = "";
      string str3 = "";
      string str4 = "";
      string str5 = "";
      string str6 = "";
      bool flag = false;
      int num4 = currentSession.Get<City>((object) idcity).GenId;
      switch (idtype)
      {
        case 1:
          str2 = "client_id";
          str3 = "DBA.lsclient";
          str4 = string.Format("idhome={0}", (object) idhome);
          str5 = "idlic";
          str6 = "DBA.tenant";
          break;
        case 2:
          str2 = "idsubsident";
          str3 = "DBA.tenant_s";
          str4 = string.Format("idlic in (select idlic from DBA.tenant where idhome={0})", (object) idhome);
          if (num4 == 5)
          {
            num4 = 4;
            break;
          }
          break;
      }
      int num5 = codeu;
      num1 = codeu;
      switch (num4 - 1)
      {
        case 0:
        case 3:
        case 4:
          num2 = idrnn;
          break;
        case 1:
          num2 = idhome;
          break;
        case 2:
          num2 = Convert.ToInt32(num5.ToString().Remove(1, num5.ToString().Length - 1));
          break;
      }
      switch (num4 - 1)
      {
        case 0:
        case 2:
          if (idfond != 1 && idfond != 5)
          {
            int num6 = (int) currentSession.CreateSQLQuery(string.Format("select isnull(max({0}),0) as lic from {1} where {2}>{3}9{4}0000 and {2}<{3}9{4}9999", (object) str2, (object) str3, (object) str2, (object) num2, (object) num5)).UniqueResult();
            num3 = num6 != 0 ? num6 + 1 : num2 * 100000000 + 90000000 + num5 * 10000 + 1;
            if (num2 * 100000000 + 90000000 + num5 * 10000 + 9999 - num3 > 0)
              flag = true;
            break;
          }
          break;
        case 1:
          int num7 = (int) currentSession.CreateSQLQuery(string.Format("select isnull(max({0}),0) as lic from {1} where {2}", (object) str2, (object) str3, (object) str4)).UniqueResult();
          if (num2 < 100000)
          {
            if (num7 < num2 * 10000)
              num7 += num2 * 10000;
            num3 = num7 + 1;
            if (num2 * 10000 + 9999 - num3 > 0)
            {
              flag = true;
              break;
            }
            break;
          }
          if (num7 < num2)
            num7 += num2 * 1000;
          num3 = num7 + 1;
          if (num2 * 1000 + 999 - num3 > 0)
            flag = true;
          break;
        case 3:
          for (int index = 0; index <= 9; ++index)
          {
            int num6 = (int) currentSession.CreateSQLQuery(string.Format("select isnull(max({0}),0) as lic from {1} where {2} > {3} and {0} <= {4}", (object) str2, (object) str3, (object) str2, (object) (num5 * 100000 + index * 10000), (object) (num5 * 100000 + index * 10000 + 9999))).UniqueResult();
            num3 = num6 != 0 ? num6 + 1 : num5 * 100000 + index * 10000 + 1;
            if (num5 * 100000 + index * 10000 + 9999 - num3 > 0)
              flag = true;
            if (flag)
              break;
          }
          break;
        case 4:
          int num8 = !(num5.ToString().Remove(2, num5.ToString().Length - 2).Remove(0, 1) == "0") ? 0 : 9;
          for (int index1 = 0; index1 <= num8; ++index1)
          {
            if (num5.ToString().Remove(2, num5.ToString().Length - 2).Remove(0, 1) == "0" && (uint) index1 > 0U)
              ++num5;
            for (int index2 = 0; index2 <= 9; ++index2)
            {
              int int32_1 = Convert.ToInt32(currentSession.CreateSQLQuery(string.Format("select isnull(max(substr({0},1,8)),0) as lic from  {1} where substr({0},1,8)>{2} and substr({0},1,8)<={3}", (object) str2, (object) str3, (object) (num5 * 10000 + index2 * 1000), (object) (num5 * 10000 + index2 * 1000 + 999))).List()[0]);
              num3 = int32_1 != 0 ? int32_1 + 1 : num5 * 10000 + index2 * 1000 + 1;
              if (num5 * 10000 + index2 * 1000 + 999 - num3 > 0)
                flag = true;
              if (flag)
              {
                int num6 = num3 * 10;
                str1 = num6.ToString();
                int num9 = num6 % 1000000000 / 100000000 * 1 + num6 % 100000000 / 10000000 * 2 + num6 % 10000000 / 1000000 * 3 + num6 % 1000000 / 100000 * 4 + num6 % 100000 / 10000 * 5 + num6 % 10000 / 1000 * 6 + num6 % 1000 / 100 * 7 + num6 % 100 / 10 * 8;
                string str7 = num9.ToString().Remove(0, num9.ToString().Length - 1);
                num3 = num6 + Convert.ToInt32(str7);
                int int32_2 = Convert.ToInt32(currentSession.CreateSQLQuery(string.Format("select {0} from {1} where {0} = {2}", (object) str2, (object) str3, (object) num3)).UniqueResult());
                int num10 = 0;
                try
                {
                  num10 = Convert.ToInt32(currentSession.CreateSQLQuery(string.Format("select {0} from {1} where {0} = {2}", (object) str5, (object) str6, (object) num3)).UniqueResult());
                }
                catch (Exception ex)
                {
                  KvrplHelper.WriteLog(ex, (LsClient) null);
                }
                if (int32_2 > 0)
                  flag = false;
                if (num10 > 0)
                {
                  int int32_3 = Convert.ToInt32(currentSession.CreateSQLQuery(string.Format("select isnull(max(substr({0},1,8)),0) as lic from  {1} where substr({0},1,8)>{2} and substr({0},1,8)<={3}", (object) str5, (object) str6, (object) (num5 * 10000 + index2 * 1000), (object) (num5 * 10000 + index2 * 1000 + 999))).List()[0]);
                  num3 = int32_3 != 0 ? int32_3 + 1 : num5 * 10000 + index2 * 1000 + 1;
                  if (num5 * 10000 + index2 * 1000 + 999 - num3 > 0)
                    flag = true;
                  if (flag)
                  {
                    int num11 = num3 * 10;
                    str1 = num11.ToString();
                    int num12 = num11 % 1000000000 / 100000000 * 1 + num11 % 100000000 / 10000000 * 2 + num11 % 10000000 / 1000000 * 3 + num11 % 1000000 / 100000 * 4 + num11 % 100000 / 10000 * 5 + num11 % 10000 / 1000 * 6 + num11 % 1000 / 100 * 7 + num11 % 100 / 10 * 8;
                    string str8 = num12.ToString().Remove(0, num12.ToString().Length - 1);
                    num3 = num11 + Convert.ToInt32(str8);
                    if (Convert.ToInt32(currentSession.CreateSQLQuery(string.Format("select {0} from {1} where {0} = {2}", (object) str2, (object) str3, (object) num3)).UniqueResult()) > 0)
                    {
                      flag = false;
                      continue;
                    }
                  }
                  break;
                }
                break;
              }
            }
            if (flag)
              break;
          }
          break;
        case 5:
          Transfer transfer = currentSession.CreateQuery("from Transfer tf where tf.Company.CompanyId=:cmp").SetParameter<int>("cmp", codeu).UniqueResult<Transfer>();
          object obj1 = currentSession.CreateQuery("select max(ls.SupplierClientId) from SupplierClient ls where ls.Supplier.BaseOrgId=-39999859 and ls.SupplierClientId between :beg and :end").SetParameter<int?>("beg", transfer.OhlBeg).SetParameter<int?>("end", transfer.OhlEnd).UniqueResult();
          int? nullable;
          object obj2;
          if (obj1 == null)
          {
            nullable = transfer.OhlBeg;
            int num6 = 10;
            obj2 = (object) (nullable.HasValue ? new int?(nullable.GetValueOrDefault() + num6) : new int?());
          }
          else
            obj2 = (object) (Convert.ToInt32(obj1.ToString().Substring(0, obj1.ToString().Length - 1) + "0") + 10);
          int num13 = (int) obj2 % 100000000 / 10000000 * 1 + (int) obj2 % 10000000 / 1000000 * 2 + (int) obj2 % 1000000 / 100000 * 3 + (int) obj2 % 100000 / 10000 * 4 + (int) obj2 % 10000 / 1000 * 5 + (int) obj2 % 1000 / 100 * 6 + (int) obj2 % 100 / 10 * 7;
          object obj3 = (object) ((int) obj2 + Convert.ToInt32(num13.ToString().Remove(0, num13.ToString().Length - 1)));
          int int32 = Convert.ToInt32(obj3);
          nullable = transfer.OhlEnd;
          int valueOrDefault = nullable.GetValueOrDefault();
          if (int32 <= valueOrDefault || !nullable.HasValue)
            return (int) obj3;
          flag = false;
          int num14 = (int) MessageBox.Show("Исчерпан лимит лицевых для компании!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          break;
        case 6:
          for (int index = 0; index <= 7; ++index)
          {
            int num6 = (int) currentSession.CreateSQLQuery(string.Format("select isnull(max({0}),0) as lic from {1} where {2} > {3} and {0} <= {4}", (object) str2, (object) str3, (object) str2, (object) (num5 * 1000 + index * 10000), (object) (num5 * 1000 + index * 10000 + 9999))).UniqueResult();
            num3 = num6 != 0 ? num6 + 1 : num5 * 1000 + index * 10000 + 1;
            if (num5 * 1000 + index * 10000 + 9999 - num3 > 0)
              flag = true;
            if (flag)
              break;
          }
          break;
      }
      if (flag)
        return num3;
      return 0;
    }

    public static Period GetKvrClose(Home home, Complex complex1, Complex complex2, int company_id)
    {
      ISession currentSession = Domain.CurrentSession;
      IList<Period> periodList = currentSession.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) Convert.ToDateTime("2007.12.01"))).List<Period>();
      IList<LsClient> lsClientList = currentSession.CreateQuery(string.Format("from LsClient l where l.Home.IdHome={0} and l.Company.CompanyId={1}", (object) home.IdHome, (object) company_id)).List<LsClient>();
      if (lsClientList.Count <= 0)
        return periodList[0];
      IList list1 = currentSession.CreateCriteria(typeof (LsClient)).Add((ICriterion) Restrictions.Eq("ClientId", (object) lsClientList[0].ClientId)).CreateCriteria("Company").List();
      if ((uint) list1.Count <= 0U)
        return periodList[0];
      Company company = ((LsClient) list1[0]).Company;
      IList list2 = complex2 != null ? currentSession.CreateQuery(string.Format("select max(Period.PeriodId) from CompanyPeriod where Company.CompanyId={0} and (Complex.ComplexId={1} or Complex.IdFk={2})", (object) company.CompanyId, (object) complex1.IdFk, (object) complex2.IdFk)).List() : currentSession.CreateQuery(string.Format("select Period.PeriodId from CompanyPeriod where Company.CompanyId={0} and Complex.IdFk={1}", (object) company.CompanyId, (object) complex1.IdFk)).List();
      currentSession.Clear();
      if (list2.Count != 0 && (uint) Convert.ToInt32(list2[0]) > 0U)
        return currentSession.Get<Period>(list2[0]);
      return periodList[0];
    }

    public static Period GetKvrClose(Company company, Complex complex1, Complex complex2)
    {
      ISession currentSession = Domain.CurrentSession;
      IList<Period> periodList = currentSession.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) Convert.ToDateTime("2007.12.01"))).List<Period>();
      IList list1 = (IList) new ArrayList();
      IList list2 = complex2 != null ? currentSession.CreateQuery(string.Format("select max(Period.PeriodId) from CompanyPeriod where Company.CompanyId={0} and (Complex.ComplexId={1} or Complex.IdFk={2})", (object) company.CompanyId, (object) complex1.IdFk, (object) complex2.IdFk)).List() : currentSession.CreateQuery(string.Format("select Period.PeriodId from CompanyPeriod where Company.CompanyId={0} and Complex.IdFk={1}", (object) company.CompanyId, (object) complex1.IdFk)).List();
      currentSession.Clear();
      if (list2.Count != 0 && (uint) Convert.ToInt32(list2[0]) > 0U)
        return currentSession.Get<Period>(list2[0]);
      return periodList[0];
    }

    public static string GetFio(LsClient Client)
    {
      ISession currentSession = Domain.CurrentSession;
      CompanyParam companyParam = (CompanyParam) null;
      try
      {
        companyParam = currentSession.CreateQuery(string.Format("from CompanyParam c where c.Param.ParamId = 202 and c.Company.CompanyId={0}", (object) Client.Company.CompanyId)).List<CompanyParam>()[0];
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      if (companyParam == null)
        return "";
      DateTime? periodName;
      int num;
      if (companyParam.Period.PeriodId != 0)
      {
        periodName = companyParam.Period.PeriodName;
        DateTime dateTime1 = periodName.Value;
        periodName = Options.Period.PeriodName;
        DateTime dateTime2 = periodName.Value;
        num = dateTime1 <= dateTime2 ? 1 : 0;
      }
      else
        num = 1;
      if (num == 0)
        return "";
      ISession session1 = currentSession;
      string format1 = "from Person p where p.LsClient.ClientId={0} and  p.Relation.RelationId=1 and  (p.Reg.RegId = 1 or p.Reg.RegId = 2) and p.FirstPropDate<='{1}' and  (p.Archive in (0,5) or (p.Archive in (1,2)  and isnull(p.OutToDate,'9999-12-31')>'{1}' and isnull(p.DieDate,'9999-12-31')>'{1}')) ";
      // ISSUE: variable of a boxed type
      int clientId1 = Client.ClientId;
      periodName = Options.Period.PeriodName;
      string baseFormat1 = KvrplHelper.DateToBaseFormat(periodName.Value);
      string queryString1 = string.Format(format1, (object) clientId1, (object) baseFormat1);
      IList<Person> personList = session1.CreateQuery(queryString1).List<Person>();
      if (personList.Count > 0)
        return string.Format("{0} {1}.{2}.", (object) personList[0].Family, personList[0].Name.Length > 0 ? (object) personList[0].Name.Remove(1, personList[0].Name.Length - 1) : (object) "", personList[0].LastName.Length > 0 ? (object) personList[0].LastName.Remove(1, personList[0].LastName.Length - 1) : (object) "");
      ISession session2 = currentSession;
      string format2 = "from Owner o where o.LsClient.ClientId={0} and  o.Relation.RelationId=1 and  o.FirstPropDate<='{1}' and  (o.Archive in (0,5) or (o.Archive in (1,2)  and isnull(o.OutToDate,'9999-12-31')>'{1}')) ";
      // ISSUE: variable of a boxed type
      int clientId2 = Client.ClientId;
      periodName = Options.Period.PeriodName;
      string baseFormat2 = KvrplHelper.DateToBaseFormat(periodName.Value);
      string queryString2 = string.Format(format2, (object) clientId2, (object) baseFormat2);
      IList<Owner> ownerList1 = session2.CreateQuery(queryString2).List<Owner>();
      if (ownerList1.Count > 0)
        return string.Format("{0} {1}.{2}.", (object) ownerList1[0].Family, ownerList1[0].Name.Length > 0 ? (object) ownerList1[0].Name.Remove(1, ownerList1[0].Name.Length - 1) : (object) "", ownerList1[0].LastName.Length > 0 ? (object) ownerList1[0].LastName.Remove(1, ownerList1[0].LastName.Length - 1) : (object) "");
      ISession session3 = currentSession;
      string format3 = "from Owner o where o.LsClient.ClientId={0} and  o.FirstPropDate<='{1}' and  (o.Archive in (0,5) or (o.Archive in (1,2)  and isnull(o.OutToDate,'9999-12-31')>'{1}')) ";
      // ISSUE: variable of a boxed type
      int clientId3 = Client.ClientId;
      periodName = Options.Period.PeriodName;
      string baseFormat3 = KvrplHelper.DateToBaseFormat(periodName.Value);
      string queryString3 = string.Format(format3, (object) clientId3, (object) baseFormat3);
      IList<Owner> ownerList2 = session3.CreateQuery(queryString3).List<Owner>();
      if (ownerList2.Count > 0)
        return string.Format("{0} {1}.{2}.", (object) ownerList2[0].Family, ownerList2[0].Name.Length > 0 ? (object) ownerList2[0].Name.Remove(1, ownerList2[0].Name.Length - 1) : (object) "", ownerList2[0].LastName.Length > 0 ? (object) ownerList2[0].LastName.Remove(1, ownerList2[0].LastName.Length - 1) : (object) "");
      return "";
    }

    public static string GetFio1(int ClientId)
    {
      ISession currentSession = Domain.CurrentSession;
      IList list = (IList) new ArrayList();
      try
      {
        list = currentSession.CreateSQLQuery("select * from systable where table_name='frPers'").List();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      string commandText = list.Count != 0 ? string.Format(" select (if (select complex_id from dba.LsClient where client_id={0})=100           then (if exists(select idform from DBA.form_a where idlic={0} and rodstv=1 and typepropis in (1,2) and firstpropdate <='{1}' and (archive in (0,5) or                               (archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}' and isnull(diedate,'2999-12-31')>'{1}')  ) )                   and isnull((select first param_value from cmpParam cp,lsClient ls where cp.company_id=ls.company_id and period_id=0 and param_id=215 and ls.client_id={0} and '{1}' between dbeg and dend),0)=0                   then (select first (select family||' '||substr(name,1,1)||'. '||substr(lastname,1,1)||'.' from dba.frPers where code=1 and id=form_a.idform)                                       from DBA.form_a where idlic={0} and rodstv=1 and typepropis in (1,2) and firstpropdate <='{1}' and (archive in (0,5) or                               (archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}' and isnull(diedate,'2999-12-31')>'{1}')) order by 1)                   else (if exists(select owner from DBA.owners where idlic={0} and rodstv=1 and FirstPropDate<='{1}' and (Archive in (0,5) or (Archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}') ) )                           then (select first (select family||' '||substr(name,1,1)||'. '||substr(lastname,1,1)||'.' from dba.frPers where code=2 and id=owners.owner)                                                   from DBA.owners where idlic={0} and rodstv=1 and FirstPropDate<='{1}' and (Archive in (0,5) or (Archive in (1,2) and                                                                           isnull(outtodate,'2999-12-31')>'{1}') ) order by 1)                           else (if exists(select owner from DBA.owners where idlic={0} and FirstPropDate<='{1}' and (Archive in (0,5) or (Archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}' ) ))                           then (select first (select family||' '||substr(name,1,1)||'. '||substr(lastname,1,1)||'.' from dba.frPers where code=2 and id=owners.owner)                                   from DBA.owners where idlic={0} and FirstPropDate<='{1}' and (Archive in (0,5) or (Archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}' ) ) order by 1)                           else ''                         endif)                  endif)              endif)           else isnull((select dogovor_num||'   '||b.nameorg_min from dba.base_org b,dba.LsArenda a where a.idbaseorg=b.idbaseorg and a.client_id={0}),'') endif) as fio ", (object) ClientId, (object) KvrplHelper.DateToBaseFormat(DateTime.Now)) : string.Format(" select (if (select complex_id from dba.LsClient where client_id={0})=100 then (if exists(select idform from DBA.form_a where idlic={0} and rodstv=1 and typepropis in (1,2) and firstpropdate <='{1}' and (archive in (0,5) or (archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}' and isnull(diedate,'2999-12-31')>'{1}')) )       and isnull((select first param_value from cmpParam cp,lsClient ls where cp.company_id=ls.company_id and period_id=0 and param_id=215 and ls.client_id={0} and '{1}' between dbeg and dend),0)=0 then      (select first family||' '||substr(name,1,1)||'. '||substr(lastname,1,1)||'.' from DBA.form_a where idlic={0} and rodstv=1 and typepropis in (1,2) and firstpropdate <='{1}' and (archive in (0,5) or (archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}' and isnull(diedate,'2999-12-31')>'{1}')) order by 1)    else (if exists(select owner from DBA.owners where idlic={0} and rodstv=1 and FirstPropDate<='{1}' and (Archive in (0,5) or (Archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}') ) ) then            (select first family||' '||substr(name,1,1)||'. '||substr(lastname,1,1)||'.' from DBA.owners where idlic={0} and rodstv=1 and FirstPropDate<='{1}' and (Archive in (0,5) or (Archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}') ) order by 1)          else (if exists(select owner from DBA.owners where idlic={0} and FirstPropDate<='{1}' and (Archive in (0,5) or (Archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}' ) )) then                  (select first family||' '||substr(name,1,1)||'. '||substr(lastname,1,1)||'.' from DBA.owners where idlic={0} and FirstPropDate<='{1}' and (Archive in (0,5) or (Archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}' ) ) order by 1)                else ''                endif)          endif)   endif) else isnull((select dogovor_num||'   '||b.nameorg_min from dba.base_org b,dba.LsArenda a where a.idbaseorg=b.idbaseorg and a.client_id={0}),'') endif) as fio ", (object) ClientId, (object) KvrplHelper.DateToBaseFormat(DateTime.Now));
      string.Format("     select family||' '||substr(name,1,1)||'. '||substr(lastname,1,1)||'.' from DBA.form_a where idlic={0} and rodstv=1 and typepropis in (1,2) and firstpropdate <='{1}' and (archive in (0,5) or (archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}' and isnull(diedate,'2999-12-31')>'{1}'))      union select family||' '||substr(name,1,1)||'. '||substr(lastname,1,1)||'.' from DBA.owners where idlic={0} and rodstv=1 and FirstPropDate<='{1}' and (Archive in (0,5) or (Archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}') )     union select family||' '||substr(name,1,1)||'. '||substr(lastname,1,1)||'.' from DBA.owners where idlic={0} and FirstPropDate<='{1}' and (Archive in (0,5) or (Archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}' ) )  ", (object) ClientId, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value));
      OleDbDataReader oleDbDataReader = OleDbHelper.ExecuteReader(string.Format("Provider={4};Eng={0};Uid={1};Pwd={2}; Links={3}", (object) Options.BaseName, (object) Options.Login, (object) Options.Pwd, (object) ("tcpip{" + Options.Host + "}"), (object) Options.Provider), CommandType.Text, commandText, 1000);
      string str = "";
      if (oleDbDataReader.Read())
        str = Convert.ToString(oleDbDataReader[0]);
      oleDbDataReader.Close();
      return str;
    }

    public static IList<string> GetFio2(short companyId, int homeId)
    {
      ISession currentSession = Domain.CurrentSession;
      string str = "";
      IList list = (IList) new ArrayList();
      try
      {
        list = currentSession.CreateSQLQuery("select * from systable where table_name='frPers'").List();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      if (!Options.Kvartplata)
        str = string.Format(" and ls.complex_id={0}", (object) Options.ComplexArenda.IdFk);
      if (!Options.Arenda)
        str = string.Format(" and ls.complex_id={0}", (object) Options.Complex.IdFk);
      OleDbDataReader oleDbDataReader = OleDbHelper.ExecuteReader(string.Format("Provider={4};Eng={0};Uid={1};Pwd={2}; Links={3}", (object) Options.BaseName, (object) Options.Login, (object) Options.Pwd, (object) ("tcpip{" + Options.Host + "}"), (object) Options.Provider), CommandType.Text, list.Count != 0 ? string.Format(" select (if (select complex_id from dba.LsClient where client_id=ls.client_id)=100 then (if exists(select idform from DBA.form_a where idlic=ls.client_id and rodstv=1 and typepropis in (1,2) and firstpropdate <='{1}' and (archive in (0,5) or (archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}' and isnull(diedate,'2999-12-31')>'{1}')) )       and isnull((select first param_value from cmpParam cp,lsClient ls where cp.company_id=ls.company_id and period_id=0 and param_id=215 and ls.client_id=ls.client_id and '{1}' between dbeg and dend),0)=0 then      (select first (select family||' '||substr(name,1,1)||'. '||substr(lastname,1,1)||'.' from dba.frPers where code=1 and id=form_a.idform) from DBA.form_a where idlic=ls.client_id and rodstv=1 and typepropis in (1,2) and firstpropdate <='{1}' and (archive in (0,5) or (archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}' and isnull(diedate,'2999-12-31')>'{1}')) order by 1)    else (if exists(select owner from DBA.owners where idlic=ls.client_id and rodstv=1 and FirstPropDate<='{1}' and (Archive in (0,5) or (Archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}') ) ) then            (select first (select family||' '||substr(name,1,1)||'. '||substr(lastname,1,1)||'.' from dba.frPers where code=2 and id=owners.owner) from DBA.owners where idlic=ls.client_id and rodstv=1 and FirstPropDate<='{1}' and (Archive in (0,5) or (Archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}') ) order by 1)          else (if exists(select owner from DBA.owners where idlic=ls.client_id and FirstPropDate<='{1}' and (Archive in (0,5) or (Archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}' ) )) then                  (select first (select family||' '||substr(name,1,1)||'. '||substr(lastname,1,1)||'.' from dba.frPers where code=2 and id=owners.owner) from DBA.owners where idlic=ls.client_id and FirstPropDate<='{1}' and (Archive in (0,5) or (Archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}' ) ) order by 1)                else ''                endif)          endif)   endif) else isnull((select dogovor_num||'   '||b.nameorg_min from dba.base_org b,dba.LsArenda a where a.idbaseorg=b.idbaseorg and a.client_id=ls.client_id),'') endif) as fio         from LsClient ls,Homes h,Flats f  where ls.Company_Id={2} and ls.idhome=h.idhome and h.IdHome={0} and ls.idFlat=f.Idflat " + str + " and isnull((select first p.Param_Value from lsParam p where p.period_id=0 and p.Client_Id=ls.Client_Id and p.Param_Id=107 and p.DBeg<='{1}' and p.DEnd>'{1}'),0) not in (6,7) order by ls.Complex_id,DBA.LENGTHHOME(NFlat),f.IdFlat,DBA.LENGTHHOME(ls.NumberRoom)", (object) homeId, (object) KvrplHelper.DateToBaseFormat(DateTime.Now), (object) companyId) : string.Format(" select (if (select complex_id from dba.LsClient where client_id=ls.client_id)=100 then (if exists(select idform from DBA.form_a where idlic=ls.client_id and rodstv=1 and typepropis in (1,2) and firstpropdate <='{1}' and (archive in (0,5) or (archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}' and isnull(diedate,'2999-12-31')>'{1}')) )       and isnull((select first param_value from cmpParam cp,lsClient ls where cp.company_id=ls.company_id and period_id=0 and param_id=215 and ls.client_id=ls.client_id and '{1}' between dbeg and dend),0)=0 then      (select first family||' '||substr(name,1,1)||'. '||substr(lastname,1,1)||'.' from DBA.form_a where idlic=ls.client_id and rodstv=1 and typepropis in (1,2) and firstpropdate <='{1}' and (archive in (0,5) or (archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}' and isnull(diedate,'2999-12-31')>'{1}')) order by 1)    else (if exists(select owner from DBA.owners where idlic=ls.client_id and rodstv=1 and FirstPropDate<='{1}' and (Archive in (0,5) or (Archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}') ) ) then            (select first family||' '||substr(name,1,1)||'. '||substr(lastname,1,1)||'.' from DBA.owners where idlic=ls.client_id and rodstv=1 and FirstPropDate<='{1}' and (Archive in (0,5) or (Archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}') ) order by 1)          else (if exists(select owner from DBA.owners where idlic=ls.client_id and FirstPropDate<='{1}' and (Archive in (0,5) or (Archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}' ) )) then                  (select first family||' '||substr(name,1,1)||'. '||substr(lastname,1,1)||'.' from DBA.owners where idlic=ls.client_id and FirstPropDate<='{1}' and (Archive in (0,5) or (Archive in (1,2) and isnull(outtodate,'2999-12-31')>'{1}' ) ) order by 1)                else ''                endif)          endif)   endif) else isnull((select dogovor_num||'   '||b.nameorg_min from dba.base_org b,dba.LsArenda a where a.idbaseorg=b.idbaseorg and a.client_id=ls.client_id),'') endif) as fio from LsClient ls,Homes h,Flats f  where ls.Company_Id={2} and ls.idhome=h.idhome and h.IdHome={0} and ls.idFlat=f.Idflat " + str + " and isnull((select first p.Param_Value from lsParam p where p.period_id=0 and p.Client_Id=ls.Client_Id and p.Param_Id=107 and p.DBeg<='{1}' and p.DEnd>'{1}'),0) not in (6,7) order by ls.Complex_id,DBA.LENGTHHOME(NFlat),f.IdFlat,DBA.LENGTHHOME(ls.NumberRoom)", (object) homeId, (object) KvrplHelper.DateToBaseFormat(DateTime.Now), (object) companyId), 1000);
      IList<string> stringList = (IList<string>) new List<string>();
      while (oleDbDataReader.Read())
        stringList.Add(Convert.ToString(oleDbDataReader[0]));
      oleDbDataReader.Close();
      return stringList;
    }

    public static bool CheckProxy(int operation, int minproxy, Company cmp, bool show)
    {
      bool flag = false;
      foreach (Proxy proxy in (IEnumerable<Proxy>) Options.Proxy)
      {
        if (proxy.Operation.OprId == operation)
        {
          if ((int) proxy.Company.CompanyId == 0)
            flag = proxy.ProxyOpr >= minproxy;
          else if (proxy.Areal == 1)
          {
            ISession currentSession = Domain.CurrentSession;
            IList<Transfer> transferList = (IList<Transfer>) new List<Transfer>();
            if (currentSession.CreateCriteria(typeof (Transfer)).Add((ICriterion) Restrictions.Eq("KvrCmp", (object) proxy.Company.CompanyId)).Add((ICriterion) Restrictions.Eq("Company", (object) cmp)).List<Transfer>().Count > 0 || proxy.Operation.OprId == 32)
              flag = proxy.ProxyOpr >= minproxy;
          }
          else if (cmp != null && (int) proxy.Company.CompanyId == (int) cmp.CompanyId)
            flag = proxy.ProxyOpr >= minproxy;
        }
      }
      if (flag)
        return true;
      if (show)
      {
        int num = (int) MessageBox.Show("У вас недостаточно прав для данной операции", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      return false;
    }

    public static string GetPin(int ClientLsId)
    {
      int[] numArray = new int[9]{ 103, 107, 109, 113, (int) sbyte.MaxValue, 131, 137, 139, 149 };
      string str1 = (ClientLsId % 1000000000 / 100000000 * numArray[0] + ClientLsId % 100000000 / 10000000 * numArray[1] + ClientLsId % 10000000 / 1000000 * numArray[2] + ClientLsId % 1000000 / 100000 * numArray[3] + ClientLsId % 100000 / 10000 * numArray[4] + ClientLsId % 10000 / 1000 * numArray[5] + ClientLsId % 1000 / 100 * numArray[6] + ClientLsId % 100 / 10 * numArray[7] + Convert.ToInt32(ClientLsId % 10) * numArray[8] - 36).ToString();
      string str2 = "";
      for (int index = 3; index >= str1.Length; --index)
        str2 += "0";
      return str2 + str1;
    }

    public static CmpParam GetMainCompanyParam(Company company, int paramId)
    {
      ISession currentSession = Domain.CurrentSession;
      CmpParam cmpParam1 = new CmpParam();
      cmpParam1.Company_id = 0;
      Period nextPeriod = KvrplHelper.GetNextPeriod(KvrplHelper.GetCmpKvrClose(currentSession.Get<Company>((object) company.CompanyId), Options.ComplexPasp.ComplexId, Options.ComplexPrior.IdFk));
      try
      {
        ISession session1 = currentSession;
        string format1 = " from CmpParam cm where cm.Company_id = {0} and cm.Period.PeriodName=(select max(c.Period.PeriodName) from CmpParam c where c.Period.PeriodId <> 0  and c.Param_id={2} and c.Dbeg <= '{3}' and c.Dend >= '{4}'  and Period.PeriodName >= '{4}')  and cm.Param_id={2} and cm.Dbeg <= '{3}' and cm.Dend >= '{4}' ";
        object[] objArray1 = new object[5]{ (object) company.CompanyId, (object) 0, (object) paramId, null, null };
        int index1 = 3;
        DateTime? periodName = nextPeriod.PeriodName;
        string baseFormat1 = KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(periodName.Value));
        objArray1[index1] = (object) baseFormat1;
        int index2 = 4;
        periodName = nextPeriod.PeriodName;
        string baseFormat2 = KvrplHelper.DateToBaseFormat(periodName.Value);
        objArray1[index2] = (object) baseFormat2;
        string queryString1 = string.Format(format1, objArray1);
        IList<CmpParam> cmpParamList = session1.CreateQuery(queryString1).List<CmpParam>();
        CmpParam cmpParam2;
        if (cmpParamList.Count <= 0)
        {
          ISession session2 = currentSession;
          string format2 = " from CmpParam where Company_id = {0} and Period.PeriodId={1}  and Param_id={2} and Dbeg <= '{3}' and Dend >= '{4}'";
          object[] objArray2 = new object[5]{ (object) company.CompanyId, (object) 0, (object) paramId, null, null };
          int index3 = 3;
          periodName = nextPeriod.PeriodName;
          string baseFormat3 = KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(periodName.Value));
          objArray2[index3] = (object) baseFormat3;
          int index4 = 4;
          periodName = nextPeriod.PeriodName;
          string baseFormat4 = KvrplHelper.DateToBaseFormat(periodName.Value);
          objArray2[index4] = (object) baseFormat4;
          string queryString2 = string.Format(format2, objArray2);
          cmpParam2 = session2.CreateQuery(queryString2).List<CmpParam>()[0];
        }
        else
          cmpParam2 = cmpParamList[0];
        cmpParam1 = cmpParam2;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      return cmpParam1;
    }

    public static void AddComboBoxColumn(DataGridView dataGridView, int colNum, IList listSource, string value, string display, string header, string name, int dropDownWidth, int width)
    {
      DataGridViewComboBoxColumn viewComboBoxColumn = new DataGridViewComboBoxColumn();
      viewComboBoxColumn.DropDownWidth = dropDownWidth;
      viewComboBoxColumn.Width = width;
      viewComboBoxColumn.MaxDropDownItems = 7;
      viewComboBoxColumn.DataSource = (object) listSource;
      viewComboBoxColumn.ValueMember = value;
      viewComboBoxColumn.DisplayMember = display;
      viewComboBoxColumn.HeaderText = header;
      viewComboBoxColumn.Name = name;
      viewComboBoxColumn.DisplayStyleForCurrentCellOnly = true;
      dataGridView.Columns.Insert(colNum, (DataGridViewColumn) viewComboBoxColumn);
    }

    public static void AddComboBoxColumn(DataGridView dataGridView, int colNum, DataTable listSource, string value, string display, string header, string name, int dropDownWidth)
    {
      DataGridViewComboBoxColumn viewComboBoxColumn = new DataGridViewComboBoxColumn();
      viewComboBoxColumn.DropDownWidth = dropDownWidth;
      viewComboBoxColumn.Width = 120;
      viewComboBoxColumn.MaxDropDownItems = 7;
      viewComboBoxColumn.DataSource = (object) listSource;
      viewComboBoxColumn.ValueMember = value;
      viewComboBoxColumn.DisplayMember = display;
      viewComboBoxColumn.HeaderText = header;
      viewComboBoxColumn.Name = name;
      viewComboBoxColumn.DisplayStyleForCurrentCellOnly = true;
      dataGridView.Columns.Insert(colNum, (DataGridViewColumn) viewComboBoxColumn);
    }

    public static void AddTextBoxColumn(DataGridView dataGridView, int colNum, string header, string name, int width, bool readOnly)
    {
      DataGridViewTextBoxColumn viewTextBoxColumn = new DataGridViewTextBoxColumn();
      viewTextBoxColumn.Width = width;
      viewTextBoxColumn.HeaderText = header;
      viewTextBoxColumn.Name = name;
      viewTextBoxColumn.ReadOnly = readOnly;
      dataGridView.Columns.Insert(colNum, (DataGridViewColumn) viewTextBoxColumn);
    }

    public static void AddButtonColumn(DataGridView dataGridView, int colNum, string header, string name, int width)
    {
      DataGridViewButtonColumn viewButtonColumn = new DataGridViewButtonColumn();
      viewButtonColumn.Width = width;
      viewButtonColumn.HeaderText = header;
      viewButtonColumn.Name = name;
      dataGridView.Columns.Insert(colNum, (DataGridViewColumn) viewButtonColumn);
    }

    public static void AddCalendarColumn(DataGridView dataGridView, int colNum, string header, string name)
    {
      CalendarColumn calendarColumn = new CalendarColumn();
      calendarColumn.HeaderText = header;
      calendarColumn.Name = name;
      dataGridView.Columns.Insert(colNum, (DataGridViewColumn) calendarColumn);
    }

    public static void AddMaskDateColumn(DataGridView dataGridView, int colNum, string header, string name)
    {
      MaskDateColumn maskDateColumn = new MaskDateColumn();
      maskDateColumn.HeaderText = header;
      maskDateColumn.Name = name;
      dataGridView.Columns.Insert(colNum, (DataGridViewColumn) maskDateColumn);
    }

    public static string FioOut(string name, string lastname, string family, DateTime? outtodate, DateTime? borndate)
    {
      string str = family + " " + name + " " + lastname;
      if (outtodate.HasValue)
        str = str + " (" + outtodate.Value.ToShortDateString() + ")";
      if (borndate.HasValue)
        str = str + " - " + borndate.Value.ToShortDateString() + "г.р.";
      return str;
    }

    public static bool CheckAuthorization(bool mix, bool oldDll = false)
    {
      string input1 = "";
      try
      {
        GetInformations getInformations = new GetInformations();
        string input2;
        string md5Hash1;
        try
        {
          string str1 = "";
          if (mix)
          {
            KvrplHelper.log.Info("Меняем номер hdd ->: " + str1);
            string str2 = getInformations.GetHddSerial().Trim(' ');
            int index = 0;
            while (index < str2.Length)
            {
              string str3 = str1;
              char ch = str2[index + 1];
              string str4 = ch.ToString();
              ch = str2[index];
              string str5 = ch.ToString();
              str1 = str3 + str4 + str5;
              index += 2;
            }
          }
          else
            str1 = getInformations.GetHddSerial().Trim(' ');
          string str6 = str1.Trim(' ');
          if (oldDll)
            str6 = KvrplHelper.SerialNumber();
          KvrplHelper.log.Info("Изначальный номер ->: " + str6);
          input2 = str6;
          KvrplHelper.log.Info("Первый номер ->: " + input2);
          string str7 = str6;
          string str8 = str6;
          string str9 = str6;
          if (input2 != "")
          {
            KvrplHelper.log.Info("-->>>hddSerial: noempty");
            if (input2.Equals(str7) && input2.Equals(str8) && input2.Equals(str9))
            {
              md5Hash1 = KvrplHelper.getMd5Hash(input2);
            }
            else
            {
              input1 = getInformations.GetOSKey();
              md5Hash1 = KvrplHelper.getMd5Hash(input1);
            }
            KvrplHelper.log.Info("-->>>hash: " + md5Hash1);
          }
          else
          {
            string s1 = "";
            string s2 = "";
            string s3 = "";
            string s4 = "";
            KvrplHelper.log.Info("-->>> genIdSN: empty");
            KvrplHelper.GetIdeSn(ref s1);
            KvrplHelper.GetIdeSn(ref s2);
            KvrplHelper.GetIdeSn(ref s3);
            KvrplHelper.GetIdeSn(ref s4);
            KvrplHelper.log.Info("-->>> genIdSN: " + s1);
            if (s1 != "")
            {
              if (s1.Equals(s2) && s1.Equals(s3) && s1.Equals(s4))
              {
                md5Hash1 = KvrplHelper.getMd5Hash(s1);
              }
              else
              {
                input1 = getInformations.GetOSKey();
                md5Hash1 = KvrplHelper.getMd5Hash(input1);
              }
            }
            else
            {
              input1 = getInformations.GetOSKey();
              md5Hash1 = KvrplHelper.getMd5Hash(input1);
            }
          }
        }
        catch (Exception ex)
        {
          KvrplHelper.log.Info("-->>> genIdSN: error1");
          int num = (int) MessageBox.Show(ex.Message);
          return false;
        }
        string data;
        try
        {
          KvrplHelper.log.Info("-->>> skey.dat: read");
          data = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "skey.dat");
          KvrplHelper.log.Info("-->>> skey.dat: read = " + data);
        }
        catch
        {
          KvrplHelper.log.Info("-->>> genIdSN: error2");
          return false;
        }
        string str = Crypt.Decrypt(data, "RFVtgbYHN");
        KvrplHelper.log.Info("-->>> hddSerialEncrypt: " + str);
        if (input2 != "" || input1 != "")
          return str == md5Hash1;
        string s = "";
        try
        {
          KvrplHelper.GetIdeSn(ref s);
          string md5Hash2 = KvrplHelper.getMd5Hash(s);
          if (s != "")
            return str == md5Hash2;
          return KvrplHelper.getMd5Hash("-1") == str;
        }
        catch (Exception ex)
        {
          KvrplHelper.log.Info("-->>> genIdSN: error3");
          int num = (int) MessageBox.Show(ex.Message);
          return false;
        }
      }
      catch (Exception ex)
      {
        KvrplHelper.log.Info("-->>> genIdSN: error4");
        int num = (int) MessageBox.Show("Отсутствует библиотека GenUinKv.dll!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        KvrplHelper.WriteLog(ex, (LsClient) null);
        return false;
      }
    }

    public static string getMd5Hash(string input)
    {
      byte[] hash = MD5.Create().ComputeHash(Encoding.Default.GetBytes(input));
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < hash.Length; ++index)
        stringBuilder.Append(hash[index].ToString("x2"));
      return stringBuilder.ToString();
    }

    private static bool verifyMd5Hash(string input, string hash)
    {
      return StringComparer.OrdinalIgnoreCase.Compare(KvrplHelper.getMd5Hash(input), hash) == 0;
    }

    public static LsClient NewLsClient(Raion raion, Company company, Home home, Complex complex)
    {
      string str = "";
      int num1 = 0;
      LsClient lsClient1 = new LsClient();
      DateTime dt = DateTime.Now;
      ISession currentSession = Domain.CurrentSession;
      int int32_1 = Convert.ToInt32(KvrplHelper.BaseValue(1, company));
      if (InputBox.InputValue("Ввод количества лицевых счетов", "Значение :", "", "", ref str, 0, 1000))
      {
        int int32_2 = Convert.ToInt32(str);
        LsClient lsClient2 = new LsClient();
        IList<Flat> flatList = (IList<Flat>) new List<Flat>();
        if (complex.ComplexId == Options.Complex.ComplexId)
          flatList = currentSession.CreateCriteria(typeof (Flat)).Add((ICriterion) Restrictions.Eq("Home", (object) home)).List<Flat>();
        if (complex.ComplexId == Options.ComplexArenda.ComplexId)
          flatList = currentSession.CreateCriteria(typeof (Flat)).Add((ICriterion) Restrictions.Eq("CompanyId", (object) Convert.ToInt16(company.CompanyId))).Add((ICriterion) Restrictions.Eq("Home", (object) home)).Add((ICriterion) Restrictions.Eq("NFlat", (object) "аренда")).List<Flat>();
        if (flatList.Count == 0)
        {
          if (complex.ComplexId == Options.Complex.ComplexId)
          {
            int num2 = (int) MessageBox.Show("Сначала необходимо завести квартиры!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          if (complex.ComplexId == Options.ComplexArenda.ComplexId)
          {
            int num3 = (int) MessageBox.Show("Сначала необходимо завести квартиру 'аренда'!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          return (LsClient) null;
        }
        if (currentSession.CreateQuery(string.Format("from LsClient ls where ls.Home.IdHome = {0} and ls.Company.CompanyId={1} " + Options.MainConditions1 + " order by ls.Complex.IdFk,DBA.LENGTHHOME(Flat.NFlat),Flat.IdFlat", (object) home.IdHome, (object) company.CompanyId)).List<LsClient>().Count > 0 && MessageBox.Show("Занести параметры, услуги, поставщиков и привязку к домовому счетчику с другого лицевого счета?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
        {
          FrmCheck frmCheck = new FrmCheck(company, home);
          int num2 = (int) frmCheck.ShowDialog();
          lsClient1 = frmCheck.ReturnClient();
          dt = frmCheck.ReturnDate();
          frmCheck.Dispose();
        }
        for (int index = 0; index < int32_2; ++index)
        {
          currentSession.Clear();
          num1 = KvrplHelper.GenLsClient(1, int32_1, (int) company.CompanyId, home.IdHome, raion.IdRnn, 0);
          LsClient lsClient3 = new LsClient();
          lsClient3.ClientId = num1;
          lsClient3.Company = company;
          lsClient3.Home = home;
          lsClient3.Flat = flatList[0];
          lsClient3.Family = "";
          lsClient3.Name = "";
          lsClient3.FName = "";
          lsClient3.SurFlat = "";
          lsClient3.Uname = Options.Login;
          lsClient3.Dedit = DateTime.Now;
          lsClient3.Phone = "";
          lsClient3.Remark = "";
          lsClient3.Note = "";
          lsClient3.Complex = complex;
          lsClient3.Locality = "";
          lsClient3.OldId = int32_1 != 4 ? new int?(num1) : new int?(Convert.ToInt32(currentSession.CreateSQLQuery("select DBA.gen_id('LsClient',1)").UniqueResult()));
          try
          {
            if (num1 == 0)
              throw new Exception();
            string connectionString = string.Format("Provider={4};Eng={0};Uid={1};Pwd={2}; Links={3}", (object) Options.BaseName, (object) Options.Login, (object) Options.Pwd, (object) "tcpip{}", (object) Options.Provider);
            currentSession.Save((object) lsClient3);
            currentSession.Flush();
            string nflat = lsClient3.Flat.NFlat;
            string lit = "";
            KvrplHelper.GetGoodNFlat(ref nflat, ref lit);
            if (int32_1 != 35 && (uint) currentSession.CreateSQLQuery("select 1 from systable where table_name='tenant'").List().Count > 0U)
            {
              string commandText = string.Format(" insert into Tenant(idlic,codeu,idhome,idflat,flat,uname,dedit,lic,idrnn,lit)  select {0},{1},{2},{3},{4},'{5}','{6}',0,{7},'{8}'", (object) lsClient3.ClientId, (object) lsClient3.Company.CompanyId, (object) lsClient3.Home.IdHome, (object) lsClient3.Flat.IdFlat, (object) nflat, (object) lsClient3.Uname, (object) KvrplHelper.DateTimeToBaseFormat(lsClient3.Dedit), (object) raion.IdRnn, (object) lit);
              OleDbHelper.ExecuteNonQuery(connectionString, CommandType.Text, commandText, 1000);
            }
            if (currentSession.Get<City>((object) int32_1).GenId == 6)
            {
              string commandText = string.Format("INSERT INTO DBA.cmpSupplierClient(Company_id, Client_id, Supplier_id, Supplier_client, Uname, Dedit) VALUES({0}, {1}, {2}, {3}, '{4}', getdate())", (object) company.CompanyId, (object) lsClient3.ClientId, (object) -39999859, (object) num1.ToString(), (object) Options.Login);
              try
              {
                OleDbHelper.ExecuteNonQuery(connectionString, CommandType.Text, commandText, 1000);
              }
              catch (Exception ex)
              {
              }
            }
          }
          catch (Exception ex)
          {
            currentSession.Clear();
            int num2 = (int) MessageBox.Show("Невозможно добавить запись!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            KvrplHelper.WriteLog(ex, (LsClient) null);
            num1 = 0;
          }
          if (lsClient1 != null && (uint) lsClient1.ClientId > 0U)
          {
            try
            {
              if (complex.ComplexId == Options.Complex.ComplexId)
                currentSession.CreateSQLQuery(string.Format("insert into lsParam(Client_id,DBeg,DEnd,Param_value,Period_id,Param_id,Uname,Dedit) select {0},'{2}','2999-12-31',Param_value,0,Param_Id,user,today() from lsParam where period_id=0 and Client_id={1} and DEnd>today()", (object) lsClient3.ClientId, (object) lsClient1.ClientId, (object) KvrplHelper.DateToBaseFormat(dt))).ExecuteUpdate();
              if (complex.ComplexId == Options.ComplexArenda.ComplexId)
                currentSession.CreateSQLQuery(string.Format("insert into lsParam(Client_id,DBeg,DEnd,Param_value,Period_id,Param_id,Uname,Dedit) select {0},'{2}','2999-12-31',Param_value,0,Param_Id,user,today() from lsParam where param_id in(2,42,103,104,107,14,15,16,27,28,37,38,65,117,59,53) and period_id=0 and Client_id={1} and DEnd>today()", (object) lsClient3.ClientId, (object) lsClient1.ClientId, (object) KvrplHelper.DateToBaseFormat(dt))).ExecuteUpdate();
              currentSession.CreateSQLQuery(string.Format("insert into lsService(Client_id,period_id,service_id,DBeg,DEnd,tariff_id,norm_id,complex_id,Uname,Dedit) select {0},0,service_id,'{2}','2999-12-31',tariff_id,norm_id,complex_id,user,today() from lsService where period_id=0 and Client_id={1} and DEnd>today() and complex_id={3}", (object) lsClient3.ClientId, (object) lsClient1.ClientId, (object) KvrplHelper.DateToBaseFormat(dt), (object) Options.Complex.ComplexId)).ExecuteUpdate();
              currentSession.CreateSQLQuery(string.Format("insert into lsSupplier(Client_id,period_id,service_id,DBeg,DEnd,supplier_id,Uname,Dedit) select {0},0,service_id,'{2}','2999-12-31',supplier_id,user,today() from lsSupplier where period_id=0 and Client_id={1} and DEnd>today()", (object) lsClient3.ClientId, (object) lsClient1.ClientId, (object) KvrplHelper.DateToBaseFormat(dt), (object) Options.Complex.ComplexId)).ExecuteUpdate();
              currentSession.CreateSQLQuery(string.Format("insert into cntrRelation(Client_id,period_id,counter_id,DBeg,DEnd,onoff,Uname,Dedit) select {0},0,counter_id,'{2}','2999-12-31',onoff,user,today() from cntrRelation where period_id=0 and Client_id={1} and DEnd>today()", (object) lsClient3.ClientId, (object) lsClient1.ClientId, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.FirstDay(dt)), (object) Options.Complex.ComplexId)).ExecuteUpdate();
            }
            catch (Exception ex)
            {
              int num2 = (int) MessageBox.Show("Не удалось скопировать данные из другого лицевого счета", "", MessageBoxButtons.OK);
              KvrplHelper.WriteLog(ex, (LsClient) null);
            }
          }
        }
      }
      try
      {
        return currentSession.Get<LsClient>((object) num1);
      }
      catch (Exception ex)
      {
        return (LsClient) null;
      }
    }

    public static void ResetGeners(string tableName, string tableKeyId)
    {
      ISession currentSession = Domain.CurrentSession;
      currentSession.Clear();
      currentSession.CreateSQLQuery(string.Format("update DBA.Geners set KeyValue=(select max({1})-(select MultiplKod from DBA.KodBd where valueKod=1) from {0}) where TblName='{0}'", (object) tableName, (object) tableKeyId)).ExecuteUpdate();
      currentSession.Clear();
    }

    public static void WriteError(string formName, string dgvName, DataGridViewDataErrorEventArgs e)
    {
      Options.Error = true;
      StreamWriter streamWriter = new StreamWriter((Stream) File.Open(Options.PathProfile + "\\Kvartplata_error.log", FileMode.Append, FileAccess.Write), Encoding.Default);
      streamWriter.WriteLine("\n" + DateTime.Now.ToString() + ": " + formName + "  " + dgvName + "\nВозникла ошибка:\n\n" + e.Exception.Message + "\n\n" + (object) e.Exception.GetType());
      streamWriter.Close();
    }

    public static void WriteError(string formName, string dgvName, DataGridViewDataErrorEventArgs e, int clientId)
    {
      Options.Error = true;
      StreamWriter streamWriter = new StreamWriter((Stream) File.Open(Options.PathProfile + "\\Kvartplata_error.log", FileMode.Append, FileAccess.Write), Encoding.Default);
      streamWriter.WriteLine("\n" + DateTime.Now.ToString() + ": " + formName + "  " + dgvName + "\nВозникла ошибка:\n" + e.Exception.Message + "\n" + (object) e.Exception.GetType());
      streamWriter.WriteLine(string.Format("Номер л/с : {0}", (object) clientId));
      streamWriter.Close();
    }

    public static void GetGoodNFlat(ref string nflat, ref string lit)
    {
      try
      {
        Convert.ToInt32(nflat);
        lit = "";
        return;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        if (nflat == "аренда")
        {
          lit = "аренда";
          nflat = "0";
          return;
        }
      }
      string str1 = "";
      string str2 = "";
      foreach (char ch in nflat)
      {
        int num;
        switch (ch)
        {
          case '0':
          case '1':
          case '2':
          case '3':
          case '4':
          case '5':
          case '6':
          case '7':
          case '8':
            num = 0;
            break;
          default:
            num = (int) ch != 57 ? 1 : 0;
            break;
        }
        if (num != 0)
        {
          str2 = nflat.Remove(0, nflat.IndexOf(ch));
          break;
        }
        str1 += ch.ToString();
      }
      nflat = !(str1 != "") ? "0" : str1;
      lit = str2;
    }

    public static void GetFamily(Person person, int code, bool full)
    {
      ISession currentSession = Domain.CurrentSession;
      IList list = (IList) new ArrayList();
      try
      {
        list = currentSession.CreateSQLQuery("select * from systable where table_name='frPers'").List();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      if (list.Count <= 0)
        return;
      IList<Pers> persList = currentSession.CreateQuery(string.Format("from Pers where Id={0} and Code={1}", (object) person.PersonId, (object) code)).List<Pers>();
      if ((uint) persList.Count > 0U)
      {
        if (KvrplHelper.CheckProxy(48, 1, person.LsClient.Company, false) | full)
        {
          person.Family = persList[0].Family;
          person.Name = persList[0].Name;
          person.LastName = persList[0].LastName;
        }
        else
        {
          person.Family = persList[0].Id.ToString();
          person.Name = "";
          person.LastName = "";
        }
      }
      else
      {
        person.Family = "";
        person.Name = "";
        person.LastName = "";
      }
    }

    public static void GetOwnerFamily(Owner owner, int code)
    {
      ISession currentSession = Domain.CurrentSession;
      IList list = (IList) new ArrayList();
      try
      {
        list = currentSession.CreateSQLQuery("select * from systable where table_name='frPers'").List();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      if (list.Count <= 0)
        return;
      IList<Pers> persList = currentSession.CreateQuery(string.Format("from Pers where Id={0} and Code={1}", (object) owner.OwnerId, (object) code)).List<Pers>();
      currentSession.Clear();
      if ((uint) persList.Count > 0U)
      {
        owner.Family = persList[0].Family;
        owner.Name = persList[0].Name;
        owner.LastName = persList[0].LastName;
      }
      else
      {
        owner.Family = "";
        owner.Name = "";
        owner.LastName = "";
      }
    }

    public static bool CloseCard(LsClient client, DateTime dateArchive)
    {
      ISession currentSession = Domain.CurrentSession;
      ITransaction transaction = currentSession.BeginTransaction();
      currentSession.Clear();
      try
      {
        currentSession.CreateQuery("delete from ClientParam where ClientId=:client and Period.PeriodId=0 and DBeg>:dend").SetParameter<string>("dend", KvrplHelper.DateToBaseFormat(dateArchive.AddDays(-1.0))).SetParameter<int>("client", client.ClientId).ExecuteUpdate();
        currentSession.CreateQuery("delete from LsService where Client.ClientId=:client and Period.PeriodId=0 and Complex.IdFk=:compl and DBeg>:dend").SetParameter<string>("dend", KvrplHelper.DateToBaseFormat(dateArchive.AddDays(-1.0))).SetParameter<int>("client", client.ClientId).SetParameter<int>("compl", Options.Complex.IdFk).ExecuteUpdate();
        currentSession.CreateQuery("delete from LsServiceParam where LsClient.ClientId=:client and Period.PeriodId=0 and DBeg>:dend").SetParameter<string>("dend", KvrplHelper.DateToBaseFormat(dateArchive.AddDays(-1.0))).SetParameter<int>("client", client.ClientId).ExecuteUpdate();
        currentSession.CreateQuery("delete from LsSupplier where LsClient.ClientId=:client and Period.PeriodId=0 and DBeg>:dend").SetParameter<string>("dend", KvrplHelper.DateToBaseFormat(dateArchive.AddDays(-1.0))).SetParameter<int>("client", client.ClientId).ExecuteUpdate();
        currentSession.CreateQuery("delete from FrFamily where LsFamily.FamilyId in (select FamilyId from LsFamily where LsClient.ClientId=:client) and Period.PeriodId=0 and DBeg>:dend").SetParameter<string>("dend", KvrplHelper.DateToBaseFormat(dateArchive.AddDays(-1.0))).SetParameter<int>("client", client.ClientId).ExecuteUpdate();
        currentSession.CreateQuery("delete from LsMSPGku where LsClient.ClientId=:client and Period.PeriodId=0 and DBeg>:dend").SetParameter<string>("dend", KvrplHelper.DateToBaseFormat(dateArchive.AddDays(-1.0))).SetParameter<int>("client", client.ClientId).ExecuteUpdate();
        currentSession.CreateQuery("delete from CounterRelation where LsClient.ClientId=:client and Period.PeriodId=0 and DBeg>:dend").SetParameter<string>("dend", KvrplHelper.DateToBaseFormat(dateArchive.AddDays(-1.0))).SetParameter<int>("client", client.ClientId).ExecuteUpdate();
        currentSession.CreateQuery(string.Format("delete from Counter where LsClient.ClientId=:client and Complex.IdFk=:compl and isnull(SetDate,'2000-01-01')>'{0}'", (object) KvrplHelper.DateToBaseFormat(dateArchive))).SetParameter<int>("compl", Options.Complex.IdFk).SetParameter<int>("client", client.ClientId).ExecuteUpdate();
        currentSession.CreateQuery("update ClientParam set DEnd=:dend,Dedit=today(),Uname=:uname where ClientId=:client and Period.PeriodId=0 and DBeg<=:dend and DEnd>:dend").SetParameter<string>("dend", KvrplHelper.DateToBaseFormat(dateArchive.AddDays(-1.0))).SetParameter<string>("uname", Options.Login).SetParameter<int>("client", client.ClientId).ExecuteUpdate();
        currentSession.Save((object) new ClientParam()
        {
          ClientId = client.ClientId,
          DBeg = dateArchive,
          DEnd = Convert.ToDateTime("2999-12-31"),
          Period = currentSession.Get<Period>((object) 0),
          Param = currentSession.Get<Param>((object) Convert.ToInt16(107)),
          ParamValue = 4.0,
          Dedit = DateTime.Now,
          Uname = Options.Login
        });
        currentSession.Flush();
        currentSession.CreateQuery("update LsService set DEnd=:dend,Dedit=today(),Uname=:uname where Client.ClientId=:client and Period.PeriodId=0 and Complex.IdFk=:compl and DBeg<=:dend and DEnd>:dend").SetParameter<string>("dend", KvrplHelper.DateToBaseFormat(dateArchive.AddDays(-1.0))).SetParameter<string>("uname", Options.Login).SetParameter<int>("client", client.ClientId).SetParameter<int>("compl", Options.Complex.IdFk).ExecuteUpdate();
        currentSession.CreateQuery("update LsServiceParam set DEnd=:dend,DEdit=today(),UName=:uname where LsClient.ClientId=:client and Period.PeriodId=0 and DBeg<=:dend and DEnd>:dend").SetParameter<string>("dend", KvrplHelper.DateToBaseFormat(dateArchive.AddDays(-1.0))).SetParameter<string>("uname", Options.Login).SetParameter<int>("client", client.ClientId).ExecuteUpdate();
        currentSession.CreateQuery("update LsSupplier set DEnd=:dend,Dedit=today(),Uname=:uname where LsClient.ClientId=:client and Period.PeriodId=0 and DBeg<=:dend and DEnd>:dend ").SetParameter<string>("dend", KvrplHelper.DateToBaseFormat(dateArchive.AddDays(-1.0))).SetParameter<string>("uname", Options.Login).SetParameter<int>("client", client.ClientId).ExecuteUpdate();
        currentSession.CreateQuery("update FrFamily set DEnd=:dend,Dedit=today(),Uname=:uname where LsFamily.FamilyId in (select FamilyId from LsFamily where LsClient.ClientId=:client) and Period.PeriodId=0 and DBeg<=:dend and DEnd>:dend").SetParameter<string>("dend", KvrplHelper.DateToBaseFormat(dateArchive.AddDays(-1.0))).SetParameter<string>("uname", Options.Login).SetParameter<int>("client", client.ClientId).ExecuteUpdate();
        currentSession.CreateQuery("update LsMSPGku set DEnd=:dend,Dedit=today(),Uname=:uname where LsClient.ClientId=:client and Period.PeriodId=0 and DBeg<=:dend and DEnd>:dend").SetParameter<string>("dend", KvrplHelper.DateToBaseFormat(dateArchive.AddDays(-1.0))).SetParameter<string>("uname", Options.Login).SetParameter<int>("client", client.ClientId).ExecuteUpdate();
        currentSession.CreateQuery("update CounterRelation set DEnd=:dend,DEdit=today(),UName=:uname where LsClient.ClientId=:client and Period.PeriodId=0 and DBeg<=:dend and DEnd>:dend").SetParameter<string>("dend", KvrplHelper.DateToBaseFormat(dateArchive.AddDays(-1.0))).SetParameter<string>("uname", Options.Login).SetParameter<int>("client", client.ClientId).ExecuteUpdate();
        currentSession.CreateQuery(string.Format("update Counter set ArchivesDate='{0}' where LsClient.ClientId=:client and Complex.IdFk=:compl and isnull(ArchivesDate,'2999-12-31')>'{0}'", (object) KvrplHelper.DateToBaseFormat(dateArchive))).SetParameter<int>("compl", Options.Complex.IdFk).SetParameter<int>("client", client.ClientId).ExecuteUpdate();
        transaction.Commit();
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        transaction.Rollback();
        return false;
      }
    }

    public static string BaseValue(int id, Company company)
    {
      ISession currentSession = Domain.CurrentSession;
      try
      {
        IList list1 = (IList) new ArrayList();
        if (company != null)
          list1 = currentSession.CreateSQLQuery(string.Format("select setup_value from fkSetup where setup_id={0} and manager_id=(select manager_id from dcCompany where company_id={1}) and dbeg<='{2}' and dend>='{2}'", (object) id, (object) company.CompanyId, (object) KvrplHelper.DateToBaseFormat(DateTime.Now))).List();
        if (list1 != null && list1.Count > 0)
          return list1[0].ToString();
        IList list2 = currentSession.CreateSQLQuery(string.Format("select setup_value from fkSetup where setup_id={0} and manager_id=0 and dbeg<='{2}' and dend>='{2}'", (object) id, (object) company, (object) KvrplHelper.DateToBaseFormat(DateTime.Now))).List();
        if (list2.Count > 0)
          return list2[0].ToString();
        return "0";
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        return "0";
      }
    }

    public static void ViewEdit(DataGridView dgv)
    {
      try
      {
        if (Options.ViewEdit)
        {
          dgv.Columns["UName"].HeaderText = "Пользователь";
          dgv.Columns["DEdit"].HeaderText = "Дата редактирования";
          dgv.Columns["UName"].ReadOnly = true;
          dgv.Columns["DEdit"].ReadOnly = true;
        }
        else
        {
          dgv.Columns["UName"].Visible = false;
          dgv.Columns["DEdit"].Visible = false;
        }
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static bool CheckLicence(int fk)
    {
      string str1 = "";
      ISession currentSession = Domain.CurrentSession;
      try
      {
        string str2;
        try
        {
          StreamReader streamReader = new StreamReader("skey.dat");
          str2 = streamReader.ReadLine();
          streamReader.Close();
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
          return false;
        }
        IList list1 = currentSession.CreateSQLQuery(string.Format("select idbaseorg,license_id from dba.lcCheck where fk_id={1} and skey='{0}'", (object) str2, (object) fk)).List();
        if (list1.Count <= 0)
          return false;
        IList list2 = currentSession.CreateSQLQuery(string.Format("select skey,controlsum from dba.lcCheck where fk_id={1} and idbaseorg={0} order by license_id", (object) Convert.ToInt32(((object[]) list1[0])[0]), (object) fk)).List();
        foreach (object[] objArray in (IEnumerable) list2)
          str1 = str1 + objArray[0] + " + ";
        return KvrplHelper.getMd5Hash(str1 + "diffence" + fk.ToString()).Equals(Convert.ToString(((object[]) list2[0])[1]));
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        return false;
      }
    }

    public static void WriteNote(Company company, Home home, LsClient client, short codeOperation, string Text, string OldText, string textnote, DateTime monthClosed, bool saveNote)
    {
      try
      {
        ISession currentSession = Domain.CurrentSession;
        NoteBook noteBook = new NoteBook();
        noteBook.Company = company;
        noteBook.IdHome = home != null ? home.IdHome : 0;
        noteBook.ClientId = client != null ? client.ClientId : 0;
        noteBook.DBeg = DateTime.Now;
        noteBook.DEnd = Convert.ToDateTime("31.12.2999");
        noteBook.Text = Text;
        noteBook.Note = textnote;
        noteBook.TypeNoteBook = currentSession.Get<TypeNoteBook>((object) 1);
        noteBook.UName = Options.Login;
        noteBook.DEdit = DateTime.Now;
        short num = client != null ? Convert.ToInt16(currentSession.CreateQuery(string.Format("select max(NoteId) from NoteBook where ClientId={0}", (object) client.ClientId)).UniqueResult()) : Convert.ToInt16(currentSession.CreateQuery(string.Format("select max(NoteId) from NoteBook where Company.CompanyId={0} and ClientId=0", (object) company.CompanyId)).UniqueResult());
        noteBook.NoteId = (int) Convert.ToInt16((int) num + 1);
        if ((int) codeOperation == 1)
        {
          currentSession.Save((object) noteBook);
          currentSession.Flush();
        }
        if ((int) codeOperation == 2)
        {
          IList<NoteBook> noteBookList1 = (IList<NoteBook>) new List<NoteBook>();
          try
          {
            IList<NoteBook> noteBookList2 = currentSession.CreateQuery(string.Format("from NoteBook where Company.CompanyId={0} and IdHome={1} and ClientId={2} and Text=:notetext and DBeg>=:notedate", (object) company.CompanyId, (object) noteBook.IdHome, (object) noteBook.ClientId, (object) OldText, (object) monthClosed.AddMonths(1))).SetParameter<string>("notetext", OldText).SetDateTime("notedate", monthClosed.AddMonths(1)).List<NoteBook>();
            if (noteBookList2.Count > 0)
            {
              if (!saveNote)
                noteBook.Note = noteBookList2[0].Note;
              currentSession.CreateQuery("update NoteBook set Text=:text,Note=:note,UName=:uname, DEdit=:dedit where Company.CompanyId=:company and IdHome=:home and ClientId=:client and Text=:notetext and DBeg>=:notedate").SetParameter<string>("text", noteBook.Text).SetParameter<string>("note", noteBook.Note).SetParameter<string>("uname", noteBook.UName).SetParameter<DateTime>("dedit", noteBook.DEdit).SetParameter<Company>("company", company).SetParameter<int>("home", noteBook.IdHome).SetParameter<int>("client", noteBook.ClientId).SetParameter<string>("notetext", OldText).SetDateTime("notedate", monthClosed.AddMonths(1)).ExecuteUpdate();
            }
            else if (Text.Substring(16, 20) == "внесено отсутствие с")
            {
              currentSession.Save((object) noteBook);
              currentSession.Flush();
            }
          }
          catch (Exception ex)
          {
            currentSession.CreateQuery("update NoteBook set Text=:text,Note=:note,UName=:uname, DEdit=:dedit where Company.CompanyId=:company and IdHome=:home and ClientId=:client and Text=:notetext and DBeg>=:notedate").SetParameter<string>("text", noteBook.Text).SetParameter<string>("note", noteBook.Note).SetParameter<string>("uname", noteBook.UName).SetParameter<DateTime>("dedit", noteBook.DEdit).SetParameter<Company>("company", company).SetParameter<int>("home", noteBook.IdHome).SetParameter<int>("client", noteBook.ClientId).SetParameter<string>("notetext", OldText).SetDateTime("notedate", monthClosed.AddMonths(1)).ExecuteUpdate();
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
        }
        if ((int) codeOperation != 3)
          return;
        currentSession.CreateQuery("delete from NoteBook where Company.CompanyId=:company and IdHome=:home and ClientId=:client and Text=:notetext and DBeg>=:notedate").SetParameter<Company>("company", company).SetParameter<int>("home", noteBook.IdHome).SetParameter<int>("client", noteBook.ClientId).SetParameter<string>("notetext", Text).SetDateTime("notedate", monthClosed.AddMonths(1)).ExecuteUpdate();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void SaveServiceToNoteBook(LsService service, short code, string notetext, bool PastTime, DateTime monthClosed)
    {
      if (!PastTime)
        return;
      try
      {
        string Text;
        if (PastTime)
          Text = "В прошлом времени занесена услуга " + service.ServiceName + " c " + service.DBeg.ToShortDateString() + " по " + service.DEnd.ToShortDateString();
        else
          Text = "Занесена услуга " + service.ServiceName + " c " + service.DBeg.ToShortDateString() + " по " + service.DEnd.ToShortDateString();
        if (service.Tariff != null)
          Text = Text + " с тарифом №" + (object) service.Tariff.Tariff_num + " (" + service.Tariff.Tariff_name + ")";
        if (service.Norm != null)
          Text = Text + ", с нормативом №" + (object) service.Norm.Norm_num + " (" + service.Norm.Norm_name + ")";
        KvrplHelper.WriteNote(service.Client.Company, service.Client.Home, service.Client, code, Text, "", notetext, monthClosed, false);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void ChangeServiceToNoteBook(LsService service, LsService oldLsService, short code, bool PastTime, DateTime monthClosed)
    {
      if (!PastTime)
        return;
      try
      {
        ISession currentSession = Domain.CurrentSession;
        string Text;
        string OldText;
        if (PastTime)
        {
          Text = "В прошлом времени занесена услуга " + service.ServiceName + " c " + service.DBeg.ToShortDateString() + " по " + service.DEnd.ToShortDateString();
          OldText = "В прошлом времени занесена услуга " + oldLsService.ServiceName + " c " + oldLsService.DBeg.ToShortDateString() + " по " + oldLsService.DEnd.ToShortDateString();
        }
        else
        {
          Text = "Занесена услуга " + service.ServiceName + " c " + service.DBeg.ToShortDateString() + " по " + service.DEnd.ToShortDateString();
          OldText = "Занесена услуга " + oldLsService.ServiceName + " c " + oldLsService.DBeg.ToShortDateString() + " по " + oldLsService.DEnd.ToShortDateString();
        }
        if (service.Tariff != null)
          Text = Text + " с тарифом №" + (object) service.Tariff.Tariff_num + " (" + service.Tariff.Tariff_name + ")";
        if (service.Norm != null)
          Text = Text + ", с нормативом №" + (object) service.Norm.Norm_num + " (" + service.Norm.Norm_name + ")";
        if (oldLsService.Tariff != null)
        {
          oldLsService.Tariff = currentSession.Get<Tariff>((object) oldLsService.Tariff.Tariff_id);
          OldText = OldText + " с тарифом №" + (object) oldLsService.Tariff.Tariff_num + " (" + oldLsService.Tariff.Tariff_name + ")";
        }
        if (oldLsService.Norm != null)
        {
          oldLsService.Norm = currentSession.Get<Norm>((object) oldLsService.Norm.Norm_id);
          OldText = OldText + ", с нормативом №" + (object) oldLsService.Norm.Norm_num + " (" + oldLsService.Norm.Norm_name + ")";
        }
        KvrplHelper.WriteNote(service.Client.Company, service.Client.Home, service.Client, code, Text, OldText, "", monthClosed, false);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void DeleteServiceFromNoteBook(LsService service, short code, bool PastTime, DateTime monthClosed)
    {
      if (!PastTime)
        return;
      try
      {
        string Text;
        if (PastTime)
          Text = "В прошлом времени занесена услуга " + service.ServiceName + " c " + service.DBeg.ToShortDateString() + " по " + service.DEnd.ToShortDateString();
        else
          Text = "Занесена услуга " + service.ServiceName + " c " + service.DBeg.ToShortDateString() + " по " + service.DEnd.ToShortDateString();
        if (service.Tariff != null)
          Text = Text + " с тарифом №" + (object) service.Tariff.Tariff_num + " (" + service.Tariff.Tariff_name + ")";
        if (service.Norm != null)
          Text = Text + ", с нормативом №" + (object) service.Norm.Norm_num + " (" + service.Norm.Norm_name + ")";
        KvrplHelper.WriteNote(service.Client.Company, service.Client.Home, service.Client, code, Text, "", "", monthClosed, false);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void SaveParamToNoteBook(ClientParam param, LsClient client, short code, string value, string note, bool PastTime, DateTime monthClosed)
    {
      if (!PastTime)
        return;
      try
      {
        ISession currentSession = Domain.CurrentSession;
        param.Param = currentSession.Get<Param>((object) param.Param.ParamId);
        string Text;
        if (PastTime)
          Text = "В прошлом времени по параметру " + param.ParamName + " c " + param.DBeg.ToShortDateString() + " по " + param.DEnd.ToShortDateString() + " внесено значение " + value;
        else
          Text = "По параметру " + param.ParamName + " c " + param.DBeg.ToShortDateString() + " по " + param.DEnd.ToShortDateString() + " внесено значение " + value;
        KvrplHelper.WriteNote(client.Company, client.Home, client, code, Text, "", note, monthClosed, false);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void ChangeParamToNoteBook(ClientParam param, ClientParam oldClientParam, LsClient client, short code, string value, bool PastTime, DateTime monthClosed)
    {
      if (!PastTime)
        return;
      try
      {
        string str = "";
        ISession currentSession = Domain.CurrentSession;
        param.Param = currentSession.Get<Param>((object) param.Param.ParamId);
        IList<AdmTbl> admTblList = currentSession.CreateQuery(string.Format("select s from AdmTbl s,ParamRelation r where s.TableId=r.TableId and r.ParamId={0}", (object) param.ParamId)).List<AdmTbl>();
        if (admTblList.Count > 0)
        {
          if (admTblList[0].ClassName != null)
          {
            IList list1 = (IList) new ArrayList();
            IList list2 = currentSession.CreateQuery(string.Format("select {4} from {0} {1} where {2}={3}", (object) admTblList[0].ClassName, (object) (admTblList[0].ClassName != "Tariff" ? "" : " where Service.ServiceId=26"), (object) admTblList[0].ClassNameId, (object) oldClientParam.ParamValue, (object) admTblList[0].ClassNameName)).List();
            if (list2.Count > 0)
              str = list2[0].ToString();
          }
        }
        else
          str = oldClientParam.ParamValue.ToString();
        string Text;
        string OldText;
        if (PastTime)
        {
          Text = "В прошлом времени по параметру " + param.ParamName + " c " + param.DBeg.ToShortDateString() + " по " + param.DEnd.ToShortDateString() + " внесено значение " + value;
          OldText = "В прошлом времени по параметру " + oldClientParam.ParamName + " c " + oldClientParam.DBeg.ToShortDateString() + " по " + oldClientParam.DEnd.ToShortDateString() + " внесено значение " + str;
        }
        else
        {
          Text = "По параметру " + param.ParamName + " c " + param.DBeg.ToShortDateString() + " по " + param.DEnd.ToShortDateString() + " внесено значение " + value;
          OldText = "По параметру " + oldClientParam.ParamName + " c " + oldClientParam.DBeg.ToShortDateString() + " по " + oldClientParam.DEnd.ToShortDateString() + " внесено значение " + str;
        }
        KvrplHelper.WriteNote(client.Company, client.Home, client, code, Text, OldText, "", monthClosed, false);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void DeleteParamFromNoteBook(ClientParam param, LsClient client, short code, string value, bool PastTime, DateTime monthClosed)
    {
      if (!PastTime)
        return;
      try
      {
        ISession currentSession = Domain.CurrentSession;
        param.Param = currentSession.Get<Param>((object) param.Param.ParamId);
        string Text;
        if (PastTime)
        {
          string[] strArray = new string[8];
          strArray[0] = "В прошлом времени по параметру ";
          strArray[1] = param.ParamName;
          strArray[2] = " c ";
          int index1 = 3;
          DateTime dateTime = param.DBeg;
          string shortDateString1 = dateTime.ToShortDateString();
          strArray[index1] = shortDateString1;
          int index2 = 4;
          string str1 = " по ";
          strArray[index2] = str1;
          int index3 = 5;
          dateTime = param.DEnd;
          string shortDateString2 = dateTime.ToShortDateString();
          strArray[index3] = shortDateString2;
          int index4 = 6;
          string str2 = " внесено значение ";
          strArray[index4] = str2;
          int index5 = 7;
          string str3 = value;
          strArray[index5] = str3;
          Text = string.Concat(strArray);
        }
        else
        {
          string[] strArray = new string[8];
          strArray[0] = "По параметру ";
          strArray[1] = param.ParamName;
          strArray[2] = " c ";
          int index1 = 3;
          DateTime dateTime = param.DBeg;
          string shortDateString1 = dateTime.ToShortDateString();
          strArray[index1] = shortDateString1;
          int index2 = 4;
          string str1 = " по ";
          strArray[index2] = str1;
          int index3 = 5;
          dateTime = param.DEnd;
          string shortDateString2 = dateTime.ToShortDateString();
          strArray[index3] = shortDateString2;
          int index4 = 6;
          string str2 = " внесено значение ";
          strArray[index4] = str2;
          int index5 = 7;
          string str3 = value;
          strArray[index5] = str3;
          Text = string.Concat(strArray);
        }
        KvrplHelper.WriteNote(client.Company, client.Home, client, code, Text, "", "", monthClosed, false);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void SaveSupplierToNoteBook(LsSupplier supplier, LsClient client, short code, string notetext, bool PastTime, DateTime monthClosed)
    {
      if (!PastTime)
        return;
      try
      {
        ISession currentSession = Domain.CurrentSession;
        supplier.Supplier = currentSession.Get<Supplier>((object) supplier.Supplier.SupplierId);
        Service service = currentSession.Get<Service>((object) supplier.Service.Root);
        client = currentSession.Get<LsClient>((object) client.ClientId);
        string Text;
        if (PastTime)
          Text = "В прошлом времени по услуге " + service.ServiceName + ", составляющей " + supplier.Service.ServiceName + " c " + supplier.DBeg.ToShortDateString() + " по " + supplier.DEnd.ToShortDateString() + " занесен поставщик " + supplier.Supplier.Recipient.NameOrgMin + " - " + supplier.Supplier.Perfomer.NameOrgMin;
        else
          Text = "По услуге " + service.ServiceName + ", составляющей " + supplier.Service.ServiceName + " c " + supplier.DBeg.ToShortDateString() + " по " + supplier.DEnd.ToShortDateString() + " занесен поставщик " + supplier.Supplier.Recipient.NameOrgMin + " - " + supplier.Supplier.Perfomer.NameOrgMin;
        KvrplHelper.WriteNote(client.Company, client.Home, client, code, Text, "", notetext, monthClosed, false);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void ChangeSupplierToNoteBook(LsSupplier supplier, LsSupplier oldSupplier, LsClient client, short code, bool PastTime, DateTime monthClosed)
    {
      if (!PastTime)
        return;
      try
      {
        ISession currentSession = Domain.CurrentSession;
        Service service = currentSession.Get<Service>((object) supplier.Service.Root);
        client = currentSession.Get<LsClient>((object) client.ClientId);
        oldSupplier.Supplier = currentSession.Get<Supplier>((object) oldSupplier.Supplier.SupplierId);
        string Text;
        string OldText;
        if (PastTime)
        {
          Text = "В прошлом времени по услуге " + service.ServiceName + ", составляющей " + supplier.Service.ServiceName + " c " + supplier.DBeg.ToShortDateString() + " по " + supplier.DEnd.ToShortDateString() + " занесен поставщик " + supplier.Supplier.Recipient.NameOrgMin + " - " + supplier.Supplier.Perfomer.NameOrgMin;
          OldText = "В прошлом времени по услуге " + service.ServiceName + ", составляющей " + oldSupplier.Service.ServiceName + " c " + oldSupplier.DBeg.ToShortDateString() + " по " + oldSupplier.DEnd.ToShortDateString() + " занесен поставщик " + oldSupplier.Supplier.Recipient.NameOrgMin + " - " + oldSupplier.Supplier.Perfomer.NameOrgMin;
        }
        else
        {
          Text = "По услуге " + service.ServiceName + ", составляющей " + supplier.Service.ServiceName + " c " + supplier.DBeg.ToShortDateString() + " по " + supplier.DEnd.ToShortDateString() + " занесен поставщик " + supplier.Supplier.Recipient.NameOrgMin + " - " + supplier.Supplier.Perfomer.NameOrgMin;
          OldText = "По услуге " + service.ServiceName + ", составляющей " + oldSupplier.Service.ServiceName + " c " + oldSupplier.DBeg.ToShortDateString() + " по " + oldSupplier.DEnd.ToShortDateString() + " занесен поставщик " + oldSupplier.Supplier.Recipient.NameOrgMin + " - " + oldSupplier.Supplier.Perfomer.NameOrgMin;
        }
        KvrplHelper.WriteNote(client.Company, client.Home, client, code, Text, OldText, "", monthClosed, false);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void DeleteSupplierFromNoteBook(LsSupplier supplier, LsClient client, short code, bool PastTime, DateTime monthClosed)
    {
      if (!PastTime)
        return;
      try
      {
        ISession currentSession = Domain.CurrentSession;
        supplier.Supplier = currentSession.Get<Supplier>((object) supplier.Supplier.SupplierId);
        client = currentSession.Get<LsClient>((object) client.ClientId);
        Service service = currentSession.Get<Service>((object) supplier.Service.Root);
        string Text;
        if (PastTime)
          Text = "В прошлом времени по услуге " + service.ServiceName + ", составляющей " + supplier.Service.ServiceName + " c " + supplier.DBeg.ToShortDateString() + " по " + supplier.DEnd.ToShortDateString() + " занесен поставщик " + supplier.Supplier.Recipient.NameOrgMin + " - " + supplier.Supplier.Perfomer.NameOrgMin;
        else
          Text = "По услуге " + service.ServiceName + ", составляющей " + supplier.Service.ServiceName + " c " + supplier.DBeg.ToShortDateString() + " по " + supplier.DEnd.ToShortDateString() + " занесен поставщик " + supplier.Supplier.Recipient.NameOrgMin + " - " + supplier.Supplier.Perfomer.NameOrgMin;
        KvrplHelper.WriteNote(client.Company, client.Home, client, code, Text, "", "", monthClosed, false);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void SaveQualityToNoteBook(LsQuality quality, LsQuality oldQuality, LsClient client, short code, DateTime monthClosed)
    {
      try
      {
        ISession currentSession = Domain.CurrentSession;
        Service service1 = new Service();
        Service service2 = currentSession.Get<Service>((object) quality.Quality.Service_id);
        string str1 = "";
        DateTime dateTime;
        if ((int) code == 2)
        {
          service2 = currentSession.Get<Service>((object) oldQuality.Quality.Service_id);
          string[] strArray = new string[6]{ "Качество с ", oldQuality.DBeg.ToShortDateString(), " по ", null, null, null };
          int index1 = 3;
          dateTime = oldQuality.DEnd.Value;
          string shortDateString = dateTime.ToShortDateString();
          strArray[index1] = shortDateString;
          int index2 = 4;
          string str2 = ". Услуга: ";
          strArray[index2] = str2;
          int index3 = 5;
          string serviceName = service2.ServiceName;
          strArray[index3] = serviceName;
          str1 = string.Concat(strArray);
        }
        Company company = client.Company;
        Home home = client.Home;
        LsClient client1 = client;
        int num1 = (int) code;
        string[] strArray1 = new string[6];
        strArray1[0] = "Качество с ";
        int index4 = 1;
        dateTime = quality.DBeg;
        string shortDateString1 = dateTime.ToShortDateString();
        strArray1[index4] = shortDateString1;
        int index5 = 2;
        string str3 = " по ";
        strArray1[index5] = str3;
        int index6 = 3;
        dateTime = quality.DEnd.Value;
        string shortDateString2 = dateTime.ToShortDateString();
        strArray1[index6] = shortDateString2;
        int index7 = 4;
        string str4 = ". Услуга: ";
        strArray1[index7] = str4;
        int index8 = 5;
        string serviceName1 = service2.ServiceName;
        strArray1[index8] = serviceName1;
        string Text = string.Concat(strArray1);
        string OldText = str1;
        string[] strArray2 = new string[5]{ quality.Quality.Quality_name, " №", quality.Quality.DocNumber, " от ", null };
        int index9 = 4;
        dateTime = quality.Quality.DocDate;
        string shortDateString3 = dateTime.ToShortDateString();
        strArray2[index9] = shortDateString3;
        string textnote = string.Concat(strArray2);
        DateTime monthClosed1 = monthClosed;
        int num2 = 0;
        KvrplHelper.WriteNote(company, home, client1, (short) num1, Text, OldText, textnote, monthClosed1, num2 != 0);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void DeleteQualityFromNoteBook(LsQuality quality, LsClient client, short code, DateTime monthClosed)
    {
      try
      {
        ISession currentSession = Domain.CurrentSession;
        Service service1 = new Service();
        Service service2 = currentSession.Get<Service>((object) quality.Quality.Service_id);
        KvrplHelper.WriteNote(client.Company, client.Home, client, code, "Качество с " + quality.DBeg.ToShortDateString() + " по " + quality.DEnd.Value.ToShortDateString() + ". Услуга: " + service2.ServiceName, "", "", monthClosed, 0 != 0);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void SaveServiceParamToNoteBook(LsServiceParam serviceparam, LsClient client, short code, string value, string notetext, bool PastTime, DateTime monthClosed)
    {
      if (!PastTime)
        return;
      try
      {
        ISession currentSession = Domain.CurrentSession;
        serviceparam.Param = currentSession.Get<Param>((object) serviceparam.Param.ParamId);
        string Text;
        if (PastTime)
          Text = "В прошлом времени по параметру " + serviceparam.Param.ParamName + " услуги " + serviceparam.Service.ServiceName + " c " + serviceparam.DBeg.ToShortDateString() + " по " + serviceparam.DEnd.ToShortDateString() + " внесено значение " + value;
        else
          Text = "По параметру " + serviceparam.Param.ParamName + " услуги " + serviceparam.Service.ServiceName + " c " + serviceparam.DBeg.ToShortDateString() + " по " + serviceparam.DEnd.ToShortDateString() + " внесено значение " + value;
        KvrplHelper.WriteNote(client.Company, client.Home, client, code, Text, "", notetext, monthClosed, false);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void ChangeServiceParamToNoteBook(LsServiceParam serviceParam, LsServiceParam oldServiceParam, LsClient client, short code, string value, bool PastTime, DateTime monthClosed)
    {
      if (!PastTime)
        return;
      try
      {
        string str = "";
        ISession currentSession = Domain.CurrentSession;
        serviceParam.Param = currentSession.Get<Param>((object) serviceParam.Param.ParamId);
        IList<AdmTbl> admTblList = currentSession.CreateQuery(string.Format("select s from AdmTbl s,ParamRelation r where s.TableId=r.TableId and r.ParamId={0}", (object) oldServiceParam.Param.ParamId)).List<AdmTbl>();
        if (admTblList.Count > 0)
        {
          if (admTblList[0].ClassName != null)
          {
            IList list1 = (IList) new ArrayList();
            IList list2 = currentSession.CreateQuery(string.Format("select {4} from {0} {1} where {2}={3}", (object) admTblList[0].ClassName, (object) (admTblList[0].ClassName != "Tariff" ? "" : " where Service.ServiceId=26"), (object) admTblList[0].ClassNameId, (object) oldServiceParam.ParamValue, (object) admTblList[0].ClassNameName)).List();
            if (list2.Count > 0)
              str = list2[0].ToString();
          }
        }
        else
          str = oldServiceParam.ParamValue.ToString();
        string Text;
        string OldText;
        if (PastTime)
        {
          Text = "В прошлом времени по параметру " + serviceParam.Param.ParamName + " услуги " + serviceParam.Service.ServiceName + " c " + serviceParam.DBeg.ToShortDateString() + " по " + serviceParam.DEnd.ToShortDateString() + " внесено значение " + value;
          OldText = "В прошлом времени по параметру " + oldServiceParam.Param.ParamName + " услуги " + oldServiceParam.Service.ServiceName + " c " + oldServiceParam.DBeg.ToShortDateString() + " по " + oldServiceParam.DEnd.ToShortDateString() + " внесено значение " + str;
        }
        else
        {
          Text = "По параметру " + serviceParam.Param.ParamName + " услуги " + serviceParam.Service.ServiceName + " c " + serviceParam.DBeg.ToShortDateString() + " по " + serviceParam.DEnd.ToShortDateString() + " внесено значение " + value;
          OldText = "По параметру " + oldServiceParam.Param.ParamName + " услуги " + oldServiceParam.Service.ServiceName + " c " + oldServiceParam.DBeg.ToShortDateString() + " по " + oldServiceParam.DEnd.ToShortDateString() + " внесено значение " + str;
        }
        KvrplHelper.WriteNote(client.Company, client.Home, client, code, Text, OldText, "", monthClosed, false);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void DeleteServiceParamFromNoteBook(LsServiceParam serviceparam, LsClient client, short code, string value, bool PastTime, DateTime monthClosed)
    {
      if (!PastTime)
        return;
      try
      {
        ISession currentSession = Domain.CurrentSession;
        serviceparam.Param = currentSession.Get<Param>((object) serviceparam.Param.ParamId);
        string Text;
        if (PastTime)
          Text = "В прошлом времени по параметру " + serviceparam.Param.ParamName + " услуги " + serviceparam.Service.ServiceName + " c " + serviceparam.DBeg.ToShortDateString() + " по " + serviceparam.DEnd.ToShortDateString() + " внесено значение " + value;
        else
          Text = "По параметру " + serviceparam.Param.ParamName + " услуги " + serviceparam.Service.ServiceName + " c " + serviceparam.DBeg.ToShortDateString() + " по " + serviceparam.DEnd.ToShortDateString() + " внесено значение " + value;
        KvrplHelper.WriteNote(client.Company, client.Home, client, code, Text, "", "", monthClosed, false);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void SaveCorrectRentToNoteBook(CorrectRent rent, LsClient client, short code, DateTime monthClosed)
    {
      try
      {
        string Text = "Внесены ручные корректировки по услуге " + rent.Service.ServiceName + " за " + rent.Month.PeriodName.Value.ToShortDateString() + ", сумма " + (object) rent.RentMain + ", объем " + (object) rent.Volume;
        KvrplHelper.WriteNote(client.Company, client.Home, client, code, Text, "", rent.Note, monthClosed, false);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void SaveCorrectPeniToNoteBook(CorrectPeni peni, LsClient client, short code, DateTime monthClosed)
    {
      try
      {
        string Text = "Внесены ручные корректировки по услуге " + peni.Service.ServiceName + " за " + peni.Period.PeriodName.Value.ToShortDateString() + ", сумма " + (object) peni.Correct;
        KvrplHelper.WriteNote(client.Company, client.Home, client, code, Text, "", peni.Note, monthClosed, false);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void ChangeCorrectRentToNoteBook(CorrectRent rent, CorrectRent oldRent, LsClient client, short code, DateTime monthClosed)
    {
      try
      {
        string Text = "Внесены ручные корректировки по услуге " + rent.Service.ServiceName + " за " + rent.Month.PeriodName.Value.ToShortDateString() + ", сумма " + (object) rent.RentMain + ", объем " + (object) rent.Volume;
        string OldText = "Внесены ручные корректировки по услуге " + oldRent.Service.ServiceName + " за " + oldRent.Month.PeriodName.Value.ToShortDateString() + ", сумма " + (object) oldRent.RentMain + ", объем " + (object) oldRent.Volume;
        KvrplHelper.WriteNote(client.Company, client.Home, client, code, Text, OldText, rent.Note, monthClosed, false);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void ChangeCorrectPeniToNoteBook(CorrectPeni peni, CorrectPeni oldPeni, LsClient client, short code, DateTime monthClosed)
    {
      try
      {
        string Text = "Внесены ручные корректировки по услуге " + peni.Service.ServiceName + " за " + peni.Period.PeriodName.Value.ToShortDateString() + ", сумма " + (object) peni.Correct ?? "";
        string OldText = "Внесены ручные корректировки по услуге " + oldPeni.Service.ServiceName + " за " + oldPeni.Period.PeriodName.Value.ToShortDateString() + ", сумма " + (object) oldPeni.Correct ?? "";
        KvrplHelper.WriteNote(client.Company, client.Home, client, code, Text, OldText, peni.Note, monthClosed, false);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void DeleteCorrectRentfromNoteBook(CorrectRent rent, LsClient client, short code, DateTime monthClosed)
    {
      try
      {
        string Text = "Внесены ручные корректировки по услуге " + rent.Service.ServiceName + " за " + rent.Month.PeriodName.Value.ToShortDateString() + ", сумма " + (object) rent.RentMain + ", объем " + (object) rent.Volume;
        KvrplHelper.WriteNote(client.Company, client.Home, client, code, Text, "", rent.Note, monthClosed, false);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void SaveRentMSPToNoteBook(RentMSP rent, LsClient client, short code, DateTime monthClosed)
    {
      try
      {
        ISession currentSession = Domain.CurrentSession;
        rent.Person = currentSession.Get<Person>((object) rent.Person.PersonId);
        rent.MSP = currentSession.Get<DcMSP>((object) rent.MSP.MSP_id);
        KvrplHelper.GetFamily(rent.Person, 1, false);
        string Text = "Внесены ручные корректировки по льготе " + rent.MSP.MSP_name + " по услуге " + rent.Service.ServiceName + " за " + rent.Month.PeriodName.Value.ToShortDateString() + " на льготника " + rent.Person.FIO + ", сумма " + (object) rent.RentMain + ", объем " + (object) rent.Volume;
        KvrplHelper.WriteNote(client.Company, client.Home, client, code, Text, "", "", monthClosed, false);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void ChangeRentMSPToNoteBook(RentMSP rent, RentMSP oldRent, LsClient client, short code, DateTime monthClosed)
    {
      try
      {
        ISession currentSession = Domain.CurrentSession;
        rent.Person = currentSession.Get<Person>((object) rent.Person.PersonId);
        rent.MSP = currentSession.Get<DcMSP>((object) rent.MSP.MSP_id);
        KvrplHelper.GetFamily(rent.Person, 1, false);
        KvrplHelper.GetFamily(oldRent.Person, 1, false);
        string Text = "Внесены ручные корректировки по льготе " + rent.MSP.MSP_name + " по услуге " + rent.Service.ServiceName + " за " + rent.Month.PeriodName.Value.ToShortDateString() + " на льготника " + rent.Person.FIO + ", сумма " + (object) rent.RentMain + ", объем " + (object) rent.Volume;
        string OldText = "Внесены ручные корректировки по льготе " + oldRent.MSP.MSP_name + " по услуге " + oldRent.Service.ServiceName + " за " + oldRent.Month.PeriodName.Value.ToShortDateString() + " на льготника " + oldRent.Person.FIO + ", сумма " + (object) oldRent.RentMain + ", объем " + (object) oldRent.Volume;
        KvrplHelper.WriteNote(client.Company, client.Home, client, code, Text, OldText, "", monthClosed, false);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void SaveTariffFromNoteBook(cmpTariffCost tariff, cmpTariffCost oldTariff, Company company, short code, string value, string notetext, bool PastTime, DateTime monthClosed)
    {
      if (!PastTime)
        return;
      try
      {
        ISession currentSession = Domain.CurrentSession;
        currentSession.CreateQuery("select cp.Company from CompanyParam cp where cp.Param.ParamId=201 and cp.DBeg<=:dbeg and cp.DEnd>=:dend and cp.ParamValue=:value and cp.Company.CompanyId<>:value").SetDateTime("dbeg", tariff.Dbeg).SetDateTime("dend", tariff.Dend).SetParameter<short>("value", company.CompanyId).List<Company>();
        currentSession.CreateQuery(string.Format("select ls.Client from LsService ls where ls.Tariff.Tariff_id={0} and ls.DBeg<=:dend and ls.DEnd>=:dbeg and ls.Complex.IdFk={1}", (object) tariff.Tariff_id, (object) Options.Complex.IdFk)).SetDateTime("dbeg", tariff.Dbeg).SetDateTime("dend", tariff.Dend).List<LsClient>();
        string str1 = "";
        string str2;
        if (PastTime)
        {
          str2 = "В прошлом времени в справочник 'Тарифы' внесен тариф " + (object) tariff.Cost + " по услуге " + tariff.Service.ServiceName + ", варианту № " + value + " c " + tariff.Dbeg.ToShortDateString() + " по " + tariff.Dend.ToShortDateString();
          if ((int) code == 2)
            str1 = "В прошлом времени в справочник 'Тарифы' внесен тариф " + (object) oldTariff.Cost + " по услуге " + oldTariff.Service.ServiceName + ", варианту № " + value + " c " + oldTariff.Dbeg.ToShortDateString() + " по " + oldTariff.Dend.ToShortDateString();
        }
        else
        {
          str2 = "В справочник 'Тарифы' внесен тариф " + (object) tariff.Cost + " по услуге " + tariff.Service.ServiceName + ", варианту № " + value + " c " + tariff.Dbeg.ToShortDateString() + " по " + tariff.Dend.ToShortDateString();
          if ((int) code == 2)
            str1 = "В справочник 'Тарифы' внесен тариф " + (object) oldTariff.Cost + " по услуге " + oldTariff.Service.ServiceName + ", варианту № " + value + " c " + oldTariff.Dbeg.ToShortDateString() + " по " + oldTariff.Dend.ToShortDateString();
        }
        if (Options.City != 28)
        {
          KvrplHelper.WriteNote(company, (Home) null, (LsClient) null, code, str2, str1, notetext, monthClosed, false);
        }
        else
        {
          if ((int) code == 1)
            currentSession.CreateSQLQuery(string.Format("insert into lsNoteBook select isnull((select max(note_id) from lsNotebook where client_id=ls.client_id),0)+1,(select company_id from lsClient where client_id=ls.client_id),(select idhome from lsClient where client_id=ls.client_id),ls.client_id,today(),'2999-12-31',:text,:note,user,today(),1 from DBA.lsService ls inner join DBA.lsClient lsclient1_ on ls.Client_Id=lsclient1_.Client_id where ls.Tariff_id={0} and ls.DBeg<=:dend and ls.DEnd>=:dbeg and ls.Complex_id={1}", (object) tariff.Tariff_id, (object) Options.Complex.IdFk, (object) str2, (object) notetext)).SetDateTime("dbeg", tariff.Dbeg).SetDateTime("dend", tariff.Dend).SetParameter<string>("text", str2).SetParameter<string>("note", notetext).ExecuteUpdate();
          if ((int) code == 2)
            currentSession.CreateSQLQuery(string.Format("update lsNoteBook set note_text=:text,UName=:uname, DEdit=:dedit where client_id in (select ls.client_id from DBA.lsService ls inner join DBA.lsClient lsclient1_ on ls.Client_Id=lsclient1_.Client_id where ls.Tariff_id={0} and ls.DBeg<=:dend and ls.DEnd>=:dbeg and ls.Complex_id={1}) and note_text=:notetext and DBeg>=:notedate", (object) tariff.Tariff_id, (object) Options.Complex.IdFk)).SetDateTime("dbeg", tariff.Dbeg).SetDateTime("dend", tariff.Dend).SetParameter<string>("text", str2).SetParameter<string>("uname", Options.Login).SetParameter<DateTime>("dedit", DateTime.Now).SetParameter<string>("notetext", str1).SetDateTime("notedate", monthClosed.AddMonths(1)).ExecuteUpdate();
          if ((int) code == 3)
            currentSession.CreateSQLQuery(string.Format("delete from lsNoteBook where client_id in (select ls.client_id from DBA.lsService ls inner join DBA.lsClient lsclient1_ on ls.Client_Id=lsclient1_.Client_id where ls.Tariff_id={0} and ls.DBeg<=:dend and ls.DEnd>=:dbeg and ls.Complex_id={1}) and note_text=:notetext and DBeg>=:notedate", (object) tariff.Tariff_id, (object) Options.Complex.IdFk)).SetParameter<string>("notetext", str2).SetDateTime("notedate", monthClosed.AddMonths(1)).SetDateTime("dbeg", tariff.Dbeg).SetDateTime("dend", tariff.Dend).ExecuteUpdate();
        }
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void SaveNormFromNoteBook(CmpNorm norm, CmpNorm oldNorm, Company company, short code, string value, string notetext, bool PastTime, DateTime monthClosed)
    {
      if (!PastTime)
        return;
      try
      {
        ISession currentSession = Domain.CurrentSession;
        currentSession.CreateQuery("select cp.Company from CompanyParam cp where cp.Param.ParamId=204 and cp.DBeg<=:dbeg and cp.DEnd>=:dend and cp.ParamValue=:value and cp.Company.CompanyId<>:value").SetDateTime("dbeg", norm.Dbeg).SetDateTime("dend", norm.Dend.Value).SetParameter<short>("value", company.CompanyId).List<Company>();
        string str1 = "";
        string str2;
        if (PastTime)
        {
          object[] objArray1 = new object[8]{ (object) "В прошлом времени в справочник 'Нормативы' внесен норматив ", (object) norm.Norm_value, (object) " по услуге ", (object) value, (object) " c ", null, null, null };
          int index1 = 5;
          DateTime dbeg = norm.Dbeg;
          string shortDateString1 = dbeg.ToShortDateString();
          objArray1[index1] = (object) shortDateString1;
          int index2 = 6;
          string str3 = " по ";
          objArray1[index2] = (object) str3;
          int index3 = 7;
          dbeg = norm.Dend.Value;
          string shortDateString2 = dbeg.ToShortDateString();
          objArray1[index3] = (object) shortDateString2;
          str2 = string.Concat(objArray1);
          if ((int) code == 2)
          {
            object[] objArray2 = new object[8]{ (object) "В прошлом времени в справочник 'Нормативы' внесен норматив ", (object) oldNorm.Norm_value, (object) " по услуге ", (object) value, (object) " c ", null, null, null };
            int index4 = 5;
            dbeg = oldNorm.Dbeg;
            string shortDateString3 = dbeg.ToShortDateString();
            objArray2[index4] = (object) shortDateString3;
            int index5 = 6;
            string str4 = " по ";
            objArray2[index5] = (object) str4;
            int index6 = 7;
            dbeg = oldNorm.Dend.Value;
            string shortDateString4 = dbeg.ToShortDateString();
            objArray2[index6] = (object) shortDateString4;
            str1 = string.Concat(objArray2);
          }
        }
        else
        {
          object[] objArray1 = new object[8]{ (object) "В справочник 'Нормативы' внесен норматив ", (object) norm.Norm_value, (object) " по услуге ", (object) value, (object) " c ", null, null, null };
          int index1 = 5;
          DateTime dbeg = norm.Dbeg;
          string shortDateString1 = dbeg.ToShortDateString();
          objArray1[index1] = (object) shortDateString1;
          int index2 = 6;
          string str3 = " по ";
          objArray1[index2] = (object) str3;
          int index3 = 7;
          dbeg = norm.Dend.Value;
          string shortDateString2 = dbeg.ToShortDateString();
          objArray1[index3] = (object) shortDateString2;
          str2 = string.Concat(objArray1);
          if ((int) code == 2)
          {
            object[] objArray2 = new object[8]{ (object) "В справочник 'Нормативы' внесен норматив ", (object) oldNorm.Norm_value, (object) " по услуге ", (object) value, (object) " c ", null, null, null };
            int index4 = 5;
            dbeg = oldNorm.Dbeg;
            string shortDateString3 = dbeg.ToShortDateString();
            objArray2[index4] = (object) shortDateString3;
            int index5 = 6;
            string str4 = " по ";
            objArray2[index5] = (object) str4;
            int index6 = 7;
            dbeg = oldNorm.Dend.Value;
            string shortDateString4 = dbeg.ToShortDateString();
            objArray2[index6] = (object) shortDateString4;
            str1 = string.Concat(objArray2);
          }
        }
        if (Options.City != 28)
        {
          KvrplHelper.WriteNote(company, (Home) null, (LsClient) null, code, str2, str1, notetext, monthClosed, false);
        }
        else
        {
          if ((int) code == 1)
            currentSession.CreateSQLQuery(string.Format("insert into lsNoteBook select isnull((select max(note_id) from lsNotebook where client_id=ls.client_id),0)+1,(select company_id from lsClient where client_id=ls.client_id),(select idhome from lsClient where client_id=ls.client_id),ls.client_id,today(),'2999-12-31',:text,:note,user,today(),1 from DBA.lsService ls inner join DBA.lsClient lsclient1_ on ls.Client_Id=lsclient1_.Client_id where ls.Norm_id={0} and ls.DBeg<=:dend and ls.DEnd>=:dbeg and ls.Complex_id={1}", (object) norm.Norm.Norm_id, (object) Options.Complex.IdFk, (object) str2, (object) notetext)).SetDateTime("dbeg", norm.Dbeg).SetDateTime("dend", norm.Dend.Value).SetParameter<string>("text", str2).SetParameter<string>("note", notetext).ExecuteUpdate();
          if ((int) code == 2)
            currentSession.CreateSQLQuery(string.Format("update lsNoteBook set note_text=:text,UName=:uname, DEdit=:dedit where client_id in (select ls.client_id from DBA.lsService ls inner join DBA.lsClient lsclient1_ on ls.Client_Id=lsclient1_.Client_id where ls.Norm_id={0} and ls.DBeg<=:dend and ls.DEnd>=:dbeg and ls.Complex_id={1}) and note_text=:notetext and DBeg>=:notedate", (object) norm.Norm.Norm_id, (object) Options.Complex.IdFk)).SetDateTime("dbeg", norm.Dbeg).SetDateTime("dend", norm.Dend.Value).SetParameter<string>("text", str2).SetParameter<string>("uname", Options.Login).SetParameter<DateTime>("dedit", DateTime.Now).SetParameter<string>("notetext", str1).SetDateTime("notedate", monthClosed.AddMonths(1)).ExecuteUpdate();
          if ((int) code == 3)
            currentSession.CreateSQLQuery(string.Format("delete from lsNoteBook where client_id in (select ls.client_id from DBA.lsService ls inner join DBA.lsClient lsclient1_ on ls.Client_Id=lsclient1_.Client_id where ls.Norm_id={0} and ls.DBeg<=:dend and ls.DEnd>=:dbeg and ls.Complex_id={1}) and note_text=:notetext and DBeg>=:notedate", (object) norm.Norm.Norm_id, (object) Options.Complex.IdFk)).SetParameter<string>("notetext", str2).SetDateTime("notedate", monthClosed.AddMonths(1)).SetDateTime("dbeg", norm.Dbeg).SetDateTime("dend", norm.Dend.Value).ExecuteUpdate();
        }
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void SaveMainCounterToNoteBook(CounterRelation counterRelation, CounterRelation oldCounterRelation, LsClient client, short code, string notetext, bool PastTime, DateTime monthClosed)
    {
      if (!PastTime)
        return;
      try
      {
        string OldText = "";
        ISession currentSession = Domain.CurrentSession;
        counterRelation.Counter = currentSession.Get<Counter>((object) counterRelation.Counter.CounterId);
        counterRelation.Period = currentSession.Get<Period>((object) counterRelation.Period.PeriodId);
        string Text;
        if (PastTime)
        {
          string[] strArray1 = new string[12];
          strArray1[0] = "В периоде ";
          strArray1[1] = counterRelation.Period.PeriodName.Value.Month.ToString();
          strArray1[2] = ".";
          int index1 = 3;
          DateTime? periodName = counterRelation.Period.PeriodName;
          DateTime dateTime = periodName.Value;
          string str1 = dateTime.Year.ToString();
          strArray1[index1] = str1;
          int index2 = 4;
          string str2 = " на лицевом ";
          strArray1[index2] = str2;
          int index3 = 5;
          string str3 = client.ClientId.ToString();
          strArray1[index3] = str3;
          int index4 = 6;
          string str4 = " занесена привязка к счетчику ";
          strArray1[index4] = str4;
          int index5 = 7;
          string allInfo1 = counterRelation.Counter.AllInfo;
          strArray1[index5] = allInfo1;
          int index6 = 8;
          string str5 = " c ";
          strArray1[index6] = str5;
          int index7 = 9;
          dateTime = counterRelation.DBeg;
          string shortDateString1 = dateTime.ToShortDateString();
          strArray1[index7] = shortDateString1;
          int index8 = 10;
          string str6 = " по ";
          strArray1[index8] = str6;
          int index9 = 11;
          dateTime = counterRelation.DEnd;
          string shortDateString2 = dateTime.ToShortDateString();
          strArray1[index9] = shortDateString2;
          Text = string.Concat(strArray1);
          if ((int) code == 2)
          {
            string[] strArray2 = new string[12];
            strArray2[0] = "В ";
            int index10 = 1;
            periodName = counterRelation.Period.PeriodName;
            dateTime = periodName.Value;
            string str7 = dateTime.Month.ToString();
            strArray2[index10] = str7;
            int index11 = 2;
            string str8 = ".";
            strArray2[index11] = str8;
            int index12 = 3;
            periodName = counterRelation.Period.PeriodName;
            dateTime = periodName.Value;
            int num = dateTime.Year;
            string str9 = num.ToString();
            strArray2[index12] = str9;
            int index13 = 4;
            string str10 = " на лицевом ";
            strArray2[index13] = str10;
            int index14 = 5;
            num = client.ClientId;
            string str11 = num.ToString();
            strArray2[index14] = str11;
            int index15 = 6;
            string str12 = " занесена привязка к счетчику ";
            strArray2[index15] = str12;
            int index16 = 7;
            string allInfo2 = oldCounterRelation.Counter.AllInfo;
            strArray2[index16] = allInfo2;
            int index17 = 8;
            string str13 = " c ";
            strArray2[index17] = str13;
            int index18 = 9;
            dateTime = oldCounterRelation.DBeg;
            string shortDateString3 = dateTime.ToShortDateString();
            strArray2[index18] = shortDateString3;
            int index19 = 10;
            string str14 = " по ";
            strArray2[index19] = str14;
            int index20 = 11;
            dateTime = oldCounterRelation.DEnd;
            string shortDateString4 = dateTime.ToShortDateString();
            strArray2[index20] = shortDateString4;
            OldText = string.Concat(strArray2);
          }
        }
        else
        {
          string[] strArray1 = new string[8];
          strArray1[0] = "На лицевом ";
          int index1 = 1;
          int clientId = client.ClientId;
          string str1 = clientId.ToString();
          strArray1[index1] = str1;
          int index2 = 2;
          string str2 = " занесена привязка к счетчику ";
          strArray1[index2] = str2;
          int index3 = 3;
          string allInfo1 = counterRelation.Counter.AllInfo;
          strArray1[index3] = allInfo1;
          int index4 = 4;
          string str3 = " c ";
          strArray1[index4] = str3;
          int index5 = 5;
          DateTime dateTime = counterRelation.DBeg;
          string shortDateString1 = dateTime.ToShortDateString();
          strArray1[index5] = shortDateString1;
          int index6 = 6;
          string str4 = " по ";
          strArray1[index6] = str4;
          int index7 = 7;
          dateTime = counterRelation.DEnd;
          string shortDateString2 = dateTime.ToShortDateString();
          strArray1[index7] = shortDateString2;
          Text = string.Concat(strArray1);
          if ((int) code == 2)
          {
            string[] strArray2 = new string[8];
            strArray2[0] = "На лицевом ";
            int index8 = 1;
            clientId = client.ClientId;
            string str5 = clientId.ToString();
            strArray2[index8] = str5;
            int index9 = 2;
            string str6 = " занесена привязка к счетчику ";
            strArray2[index9] = str6;
            int index10 = 3;
            string allInfo2 = oldCounterRelation.Counter.AllInfo;
            strArray2[index10] = allInfo2;
            int index11 = 4;
            string str7 = " c ";
            strArray2[index11] = str7;
            int index12 = 5;
            dateTime = oldCounterRelation.DBeg;
            string shortDateString3 = dateTime.ToShortDateString();
            strArray2[index12] = shortDateString3;
            int index13 = 6;
            string str8 = " по ";
            strArray2[index13] = str8;
            int index14 = 7;
            dateTime = oldCounterRelation.DEnd;
            string shortDateString4 = dateTime.ToShortDateString();
            strArray2[index14] = shortDateString4;
            OldText = string.Concat(strArray2);
          }
        }
        KvrplHelper.WriteNote(client.Company, client.Home, (LsClient) null, code, Text, OldText, notetext, monthClosed, false);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static void SaveAbsenceFromNoteBook(LsAbsence absence, LsAbsence oldAbsence, Company company, short code, string notetext, DateTime monthClosed)
    {
      try
      {
        ISession currentSession = Domain.CurrentSession;
        absence.LsClient = currentSession.Get<LsClient>((object) absence.LsClient.ClientId);
        IList<LsClient> lsClientList = currentSession.CreateQuery("select distinct cr.LsClient from CounterRelation cr where cr.Counter.CounterId in (select Counter.CounterId from CounterRelation where LsClient.ClientId=:client and period_id=0 and dbeg<=:dbeg and dend>=:dend) and cr.LsClient.ClientId<>:client and cr.DBeg<=:dbeg and cr.DEnd>=:dend and cr.OnOff.YesNoId=1").SetInt32("client", absence.LsClient.ClientId).SetDateTime("dbeg", absence.DEnd.Value).SetDateTime("dend", absence.DBeg.Value).List<LsClient>();
        string str = "";
        string Text = "На ЛС " + absence.LsClient.ClientId.ToString() + " внесено отсутствие с " + absence.DBeg.Value.ToShortDateString() + " по " + absence.DEnd.Value.ToShortDateString();
        if ((int) code == 2)
          str = "На ЛС " + oldAbsence.LsClient.ClientId.ToString() + " внесено отсутствие с " + oldAbsence.DBeg.Value.ToShortDateString() + " по " + oldAbsence.DEnd.Value.ToShortDateString();
        foreach (LsClient client in (IEnumerable<LsClient>) lsClientList)
          KvrplHelper.WriteNote(absence.LsClient.Company, absence.LsClient.Home, client, code, Text, str, notetext, monthClosed, false);
        if ((int) code != 2)
          return;
        IQuery query1 = currentSession.CreateQuery("select distinct cr.LsClient from CounterRelation cr where cr.Counter.CounterId in    (select Counter.CounterId from CounterRelation where LsClient.ClientId=:client and period_id=0 and dbeg<=:dbeg and dend>=:dend) and cr.LsClient.ClientId<>:client and cr.DBeg<=:dbeg and cr.DEnd>=:dend and cr.OnOff.YesNoId=1   and cr.LsClient.ClientId not in (select crl.LsClient.ClientId from CounterRelation crl where crl.Counter.CounterId in (select Counter.CounterId from CounterRelation where LsClient.ClientId=:client and period_id=0 and dbeg<=:ndbeg and dend>=:ndend) and crl.LsClient.ClientId<>:client and crl.DBeg<=:ndbeg and crl.DEnd>=:ndend and crl.OnOff.YesNoId=1)").SetInt32("client", absence.LsClient.ClientId);
        string name1 = "dbeg";
        DateTime? nullable = oldAbsence.DEnd;
        DateTime val1 = nullable.Value;
        IQuery query2 = query1.SetDateTime(name1, val1);
        string name2 = "dend";
        nullable = oldAbsence.DBeg;
        DateTime val2 = nullable.Value;
        IQuery query3 = query2.SetDateTime(name2, val2);
        string name3 = "ndbeg";
        nullable = absence.DEnd;
        DateTime val3 = nullable.Value;
        IQuery query4 = query3.SetDateTime(name3, val3);
        string name4 = "ndend";
        nullable = absence.DBeg;
        DateTime val4 = nullable.Value;
        foreach (LsClient client in (IEnumerable<LsClient>) query4.SetDateTime(name4, val4).List<LsClient>())
          KvrplHelper.WriteNote(absence.LsClient.Company, absence.LsClient.Home, client, (short) 3, str, "", notetext, monthClosed, false);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    public static string GetTariffInfo(int tariff)
    {
      Tariff tariff1 = Domain.CurrentSession.Get<Tariff>((object) tariff);
      return "№" + tariff1.Tariff_num.ToString() + " " + tariff1.Tariff_name;
    }

    public static string GetNormInfo(int n)
    {
      Norm norm = Domain.CurrentSession.Get<Norm>((object) n);
      return "№" + norm.Norm_num.ToString() + "  " + norm.Norm_name;
    }

    public static void CheckAll(CheckedListBox checkedListBox)
    {
      for (int index = 0; index < checkedListBox.Items.Count; ++index)
        checkedListBox.SetItemCheckState(index, CheckState.Checked);
    }

    public static void UnCheckAll(CheckedListBox checkedListBox)
    {
      for (int index = 0; index < checkedListBox.Items.Count; ++index)
        checkedListBox.SetItemCheckState(index, CheckState.Unchecked);
    }

    public static void WriteLog(Exception exp, LsClient client)
    {
      try
      {
        string message = " Возникла ошибка: " + exp.Message + "\n";
        if (exp.InnerException != null)
          message = message + exp.InnerException.Message + "\n";
        if (client != null)
          message = message + "Номер л/с: " + client.ClientId.ToString() + "\n";
        MainForm.log.Error(message);
      }
      catch
      {
      }
    }

    public static void CheckAll(DataGridView dgv, Button btn)
    {
      if (btn.Text == "Выделить все")
      {
        foreach (DataGridViewRow row in (IEnumerable) dgv.Rows)
          row.Cells["Check"].Value = (object) true;
        btn.Text = "Снять все";
      }
      else
      {
        foreach (DataGridViewRow row in (IEnumerable) dgv.Rows)
          row.Cells["Check"].Value = (object) false;
        btn.Text = "Выделить все";
      }
    }

    public static bool IsGoodEvidence(Evidence evidence, ISession session)
    {
      DateTime dt = evidence.DBeg;
      DateTime dend = evidence.DEnd;
      int num1 = dend.Subtract(dt).Days + 1;
      DateTime dateTime;
      for (; dt < dend; dt = dateTime.AddDays(1.0))
      {
        dateTime = KvrplHelper.LastDay(dt);
        int num2 = !(dateTime < dend) ? dend.Day - dt.Day + 1 : dateTime.Day - dt.Day + 1;
        IList<DetailEvidence> detailEvidenceList = session.CreateQuery(string.Format("from DetailEvidence where Counter.CounterId={0} and Period.PeriodId<{2} and Month.PeriodId=(select PeriodId from Period where PeriodName='{1}') and Type=0 order by Period.PeriodId desc", (object) evidence.Counter.CounterId, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.FirstDay(dt)), (object) Options.Period.PeriodId)).List<DetailEvidence>();
        double num3 = detailEvidenceList.Count <= 0 ? 0.0 : detailEvidenceList[0].Evidence;
        if (Math.Round(Math.Round((evidence.Current - evidence.Past) * (double) num2 / (double) num1, 8) + num3, 8) < -5E-08)
        {
          int num4 = (int) MessageBox.Show("В одном из периодов получается отрицательный расход. Проверьте даты и правильность показаний", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
      }
      return true;
    }

    public static int MinMaxClosedPeriod(string minmax, int Level, Raion raion, Company company, ISession session)
    {
      string str = "select " + minmax + "(Period.PeriodId) from CompanyPeriod where (Complex.IdFk in (100,101,102,108)";
      string queryString = Convert.ToInt32(KvrplHelper.BaseValue(25, Options.Company)) != 1 ? str + ")" : str + " or Complex.IdFk=104)";
      if (Level == 1)
        queryString += string.Format(" and Company.Raion.IdRnn={0}", (object) raion.IdRnn);
      if (Level >= 2)
        queryString += string.Format(" and Company.CompanyId={0}", (object) company.CompanyId);
      int num = 0;
      try
      {
        num = Convert.ToInt32(session.CreateQuery(queryString).List()[0]);
      }
      catch
      {
      }
      return num;
    }

    public static void ViewErrorLic(IList list, string message, string fileMessage, short type, SaveFileDialog sfd)
    {
      if (MessageBox.Show(message, "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
      {
        string text = "";
        foreach (object obj in (IEnumerable) list)
        {
          switch (type)
          {
            case 1:
              text = text + ((object[]) obj)[0].ToString() + " ";
              break;
            case 2:
              text = text + "Лицевой: " + ((object[]) obj)[0].ToString() + ", ФИО: " + ((object[]) obj)[1].ToString() + ", льгота:" + ((object[]) obj)[2].ToString() + "\n";
              break;
            case 3:
              text = text + ((LsClient) obj).ClientId.ToString() + " ";
              break;
            case 4:
              text = text + "№" + ((LsArenda) obj).DogovorNum + " (" + ((LsArenda) obj).LsClient.ClientId.ToString() + ")   ";
              break;
          }
        }
        int num1 = (int) MessageBox.Show(text, "", MessageBoxButtons.OK);
        if (MessageBox.Show("Сохранить лицевые в файл?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
          return;
        if (sfd.ShowDialog() == DialogResult.OK)
        {
          try
          {
            StreamWriter streamWriter1 = new StreamWriter((Stream) File.Open(sfd.FileName, FileMode.Append, FileAccess.Write), Encoding.Default);
            streamWriter1.WriteLine(DateTime.Now.ToString() + ": " + fileMessage);
            int clientId;
            foreach (object obj in (IEnumerable) list)
            {
              switch (type)
              {
                case 1:
                  streamWriter1.WriteLine(((object[]) obj)[0].ToString());
                  break;
                case 2:
                  streamWriter1.WriteLine("Лицевой: " + ((object[]) obj)[0].ToString() + ", ФИО: " + ((object[]) obj)[1].ToString() + ", льгота: " + ((object[]) obj)[2].ToString() + (((object[]) obj)[3] != null ? ", дата прописки: " + Convert.ToDateTime(((object[]) obj)[3]).ToShortDateString() : " ") + (((object[]) obj)[4] != null ? ", дата выписки: " + Convert.ToDateTime(((object[]) obj)[4]).ToShortDateString() : " ") + (((object[]) obj)[5] != null ? ", дата смерти: " + Convert.ToDateTime(((object[]) obj)[5]).ToShortDateString() : " "));
                  break;
                case 3:
                  StreamWriter streamWriter2 = streamWriter1;
                  clientId = ((LsClient) obj).ClientId;
                  string str1 = clientId.ToString();
                  streamWriter2.WriteLine(str1);
                  break;
                case 4:
                  StreamWriter streamWriter3 = streamWriter1;
                  string[] strArray = new string[5]{ "№", ((LsArenda) obj).DogovorNum, " (", null, null };
                  int index1 = 3;
                  clientId = ((LsArenda) obj).LsClient.ClientId;
                  string str2 = clientId.ToString();
                  strArray[index1] = str2;
                  int index2 = 4;
                  string str3 = ")";
                  strArray[index2] = str3;
                  string str4 = string.Concat(strArray);
                  streamWriter3.WriteLine(str4);
                  break;
              }
            }
            streamWriter1.Close();
          }
          catch (Exception ex)
          {
            int num2 = (int) MessageBox.Show("Не удалось сохранить данные в файл");
            KvrplHelper.WriteLog(ex, (LsClient) null);
            return;
          }
          int num3 = (int) MessageBox.Show("Данные сохранены");
        }
      }
      else
      {
        if ((int) type != 1)
          return;
        int num = (int) MessageBox.Show("Используйте отчеты из библиотеки отчетов или сверку итогов на форме расчета для поиска ошибок.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    public static string Prc_FillReceipt(string tbl, string strPeriod)
    {
      return "create index rcpt on " + tbl + " (client_id,service_id,supplier_id);update " + tbl + " t   set receipt_id=isnull((select receipt_id from dba.cmpServiceParam sp,dba.lsClient ls where ls.company_id=sp.company_id and ls.client_id=t.client_id and                                                                                              (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and ls.complex_id=sp.complex_id),                   isnull((if t.service_id=0 and (select count(receipt_id) from dba.cmpReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and ls.client_id=t.client_id)=1 then                                           (select receipt_id from dba.cmpReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and ls.client_id=t.client_id) else 0 endif),0))   where not exists(select * from systable where table_name='cmphmReceipt' and creator=1);commit;if exists(select * from systable where table_name='cmphmReceipt' and creator=1) then   update " + tbl + " t     set receipt_id=isnull((select receipt_id from dba.cmphmReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and ls.idhome=sp.idhome and sp.complex_id=ls.complex_id and " + strPeriod + " between dbeg and dend and ls.client_id=t.client_id and                                                                                             (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.supplier_id=t.supplier_id),                     isnull((select receipt_id from dba.cmphmReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and ls.idhome=sp.idhome and sp.complex_id=ls.complex_id and " + strPeriod + " between dbeg and dend and ls.client_id=t.client_id and sp.service_id=0 and sp.supplier_id=t.supplier_id),                       isnull((select receipt_id from dba.cmphmReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and ls.idhome=sp.idhome and sp.complex_id=ls.complex_id and " + strPeriod + " between dbeg and dend and ls.client_id=t.client_id and                                                                                                 (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.supplier_id=0),                         isnull((select receipt_id from dba.cmphmReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and sp.idhome=0 and sp.complex_id=ls.complex_id and " + strPeriod + " between dbeg and dend and ls.client_id=t.client_id and                                                                                                   (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.supplier_id=t.supplier_id),                           isnull((select receipt_id from dba.cmphmReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and sp.idhome=0 and sp.complex_id=ls.complex_id and " + strPeriod + " between dbeg and dend and ls.client_id=t.client_id and sp.service_id=0 and sp.supplier_id=t.supplier_id),                             isnull((select receipt_id from dba.cmphmReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and sp.idhome=0 and sp.complex_id=ls.complex_id and " + strPeriod + " between dbeg and dend and ls.client_id=t.client_id and                                                                                                       (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.supplier_id=0),                               isnull((select receipt_id from dba.cmpServiceParam sp,dba.lsClient ls where ls.company_id=sp.company_id and ls.client_id=t.client_id and                                                                                                  (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.complex_id=ls.complex_id),                                 isnull((if t.service_id=0 and (select count(receipt_id) from dba.cmpReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and ls.client_id=t.client_id)=1 then                                            (select receipt_id from dba.cmpReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and ls.client_id=t.client_id) else 0 endif),                                 0))))))));commit;end if; commit;";
    }

    public static string NS_lsOwner(int typeId, int code, string period, string periodv, string client, string fdat)
    {
      string str1 = "";
      string str2 = "";
      string str3 = "";
      switch (typeId)
      {
        case 1:
        case 3:
          str1 = "ymd(year(" + periodv + "),month(" + periodv + ")+1,1)-1";
          break;
        case 2:
          str1 = "(if " + fdat + "<=" + periodv + "-1 then " + periodv + "-1 else ymd(year(" + fdat + "),month(" + fdat + ")+1,1)-1 endif)";
          break;
      }
      string str4 = " and typepropis in (1,2) and firstpropdate<=" + fdat + " and regdate<=" + str1 + " and (archive in (0,5) or (archive in (1,2) and isnull(outtodate,'2999-12-31')>" + fdat + " and isnull(diedate,'2999-12-31')>" + fdat + ") or (archive in (1,2) and regoutdate>" + str1 + "))";
      string str5 = " and firstpropdate<=" + fdat + " and regdate<=" + str1 + " and (archive in (0,5) or (archive in (1,2) and isnull(outtodate,'2999-12-31')>" + fdat + ") or (archive in (1,2) and regoutdate>" + str1 + "))";
      if (code == 1)
      {
        str2 = KvrplHelper.NS_frPers(1, "fff.idform") + "||' '||" + KvrplHelper.NS_frPers(2, "fff.idform") + "||' '||" + KvrplHelper.NS_frPers(3, "fff.idform");
        str3 = KvrplHelper.NS_frPers(4, "ooo.owner") + "||' '||" + KvrplHelper.NS_frPers(5, "ooo.owner") + "||' '||" + KvrplHelper.NS_frPers(6, "ooo.owner");
      }
      if (code == 2)
      {
        str2 = "trim(" + KvrplHelper.NS_frPers(1, "fff.idform") + "||' '||substr(" + KvrplHelper.NS_frPers(2, "fff.idform") + ",1,1)||(if " + KvrplHelper.NS_frPers(2, "fff.idform") + "<>'' then '.' else '' endif)||substr(" + KvrplHelper.NS_frPers(3, "fff.idform") + ",1,1)||(if " + KvrplHelper.NS_frPers(3, "fff.idform") + "<> '' then '.' else '' endif))";
        str3 = "trim(" + KvrplHelper.NS_frPers(4, "ooo.owner") + "||' '||substr(" + KvrplHelper.NS_frPers(5, "ooo.owner") + ",1,1)||(if " + KvrplHelper.NS_frPers(5, "ooo.owner") + "<>'' then '.' else '' endif)||substr(" + KvrplHelper.NS_frPers(6, "ooo.owner") + ",1,1)||(if " + KvrplHelper.NS_frPers(6, "ooo.owner") + "<> '' then '.' else '' endif))";
      }
      return "(if exists(select idform from form_a where idlic=" + client + " and rodstv=1" + str4 + ") then     (select first " + str2 + " from form_a fff where idlic=" + client + " and rodstv=1" + str4 + ")  else (if exists(select owner from owners where idlic=" + client + " and rodstv=1" + str5 + ") then           (select first " + str3 + " from owners ooo where idlic=" + client + " and rodstv=1" + str5 + ")        else (if exists(select owner from owners where idlic=" + client + str5 + ") then                 (select first " + str3 + " from owners ooo where idlic=" + client + str5 + ")                          else ''              endif)        endif) endif)";
    }

    public static string NS_frPers(int id, string sid)
    {
      string str1 = "";
      string str2 = "";
      switch (id)
      {
        case 1:
        case 4:
          str1 = "family";
          break;
        case 2:
        case 5:
          str1 = "name";
          break;
        case 3:
        case 6:
          str1 = "lastname";
          break;
      }
      switch (Options.TypeFio)
      {
        case 0:
          if (id >= 1 && id <= 3)
            str2 = "(select " + str1 + " from form_a where idform=" + sid + ")";
          if (id >= 4 && id <= 6)
          {
            str2 = "(select " + str1 + " from owners where owner=" + sid + ")";
            break;
          }
          break;
        case 1:
          if (id >= 1 && id <= 3)
            str2 = "(select " + str1 + " from frPers where code=1 and id=" + sid + ")";
          if (id >= 4 && id <= 6)
          {
            str2 = "(select " + str1 + " from frPers where code=2 and id=" + sid + ")";
            break;
          }
          break;
      }
      return str2;
    }

    public static EnumAccess CheckReadOnly(int operationAccess, Company cmp, bool show)
    {
      if (new List<int>() { 1, 5, 6, 9, 10, 17, 18, 19, 20, 25, 26, 29, 31, 32, 34, 35, 36 }.Contains(Options.City) && operationAccess == 32)
      {
        if (show)
        {
          int num = (int) MessageBox.Show("У вас недостаточно прав для данной операции", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        return EnumAccess.NoAccess;
      }
      if (KvrplHelper.CheckProxy(operationAccess, 2, cmp, false))
        return EnumAccess.ReadEdit;
      if (KvrplHelper.CheckProxy(operationAccess, 1, cmp, false))
        return EnumAccess.ReadOnly;
      return KvrplHelper.CheckProxy(operationAccess, 0, cmp, false) ? EnumAccess.NoAccess : EnumAccess.Default;
    }

    public static bool AccessToReadOnly(EnumAccess ac)
    {
      switch (ac)
      {
        case EnumAccess.NoAccess:
          return false;
        case EnumAccess.ReadOnly:
          return false;
        case EnumAccess.ReadEdit:
          return true;
        default:
          return false;
      }
    }

    public static bool AccessToCompany(int operationAccess, Company cmp, int minproxy)
    {
      if (Options.Proxy.FirstOrDefault<Proxy>((Func<Proxy, bool>) (x =>
      {
        if (x.Operation.OprId == operationAccess)
          return (uint) x.Company.CompanyId > 0U;
        return false;
      })) == null)
        return true;
      foreach (Proxy proxy in Options.Proxy.Where<Proxy>((Func<Proxy, bool>) (x =>
      {
        if (x.Operation.OprId == operationAccess)
          return (uint) x.Company.CompanyId > 0U;
        return false;
      })).ToList<Proxy>())
      {
        if ((int) proxy.Company.CompanyId == (int) cmp.CompanyId)
        {
          if (proxy.Areal == 1)
          {
            ISession currentSession = Domain.CurrentSession;
            IList<Transfer> transferList = (IList<Transfer>) new List<Transfer>();
            if (currentSession.CreateCriteria(typeof (Transfer)).Add((ICriterion) Restrictions.Eq("KvrCmp", (object) proxy.Company.CompanyId)).Add((ICriterion) Restrictions.Eq("Company", (object) cmp)).List<Transfer>().Count > 0 || proxy.Operation.OprId == operationAccess)
              return proxy.ProxyOpr >= minproxy;
          }
          else if (cmp != null && (int) proxy.Company.CompanyId == (int) cmp.CompanyId && proxy.Operation.OprId == operationAccess)
            return proxy.ProxyOpr >= minproxy;
        }
      }
      return false;
    }
  }
}
