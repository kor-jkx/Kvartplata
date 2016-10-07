// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmReceipts
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using FastReport;
using FastReport.Data;
using Kvartplata.Classes;
using Kvartplata.Properties;
using NHibernate;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmReceipts : FrmBaseForm
  {
    private FormStateSaver fss = new FormStateSaver(FrmReceipts.container);
    private string MP_Global1 = "";
    private string MP_Global2 = "";
    private IContainer components = (IContainer) null;
    private static IContainer container;
    private short level;
    private int city;
    private Raion raion;
    private Company company;
    private Home home;
    private LsClient client;
    private ISession session;
    private int socSaldo;
    private int peniAlg;
    private int peniFree;
    private int peniParam;
    private int peniBegMonth;
    private int peniOldDept;
    private int peniBalance;
    private int rentMonth;
    private int mspMonth;
    private string uTmp;
    private const string tmp = "tmpReceipt";
    private int[] receipts;
    private short showPersData;
    private short typeFio;
    private const string NS_RightDoc_id = "(2,5,6,7,8,12,14,15,17,18,21,25,28,29,31,33,34)";
    private string strPeni;
    private Panel pnBtn;
    private Button btnExit;
    private Button btnPrint;
    private DataSet dataSet1;
    private Button btnPrint2;
    private Button button1;
    private FastReport.Report report1;
    private Button button2;
    private GroupBox gbChoice;
    private GroupBox gbBetween;
    private GroupBox gbPeni;
    private GroupBox gbDebt;
    private Panel panel1;
    private MonthPicker mpCurrentPeriod;
    private GroupBox gbReceipt;
    private GroupBox gbDop;
    private Panel panel2;
    private RadioButton rbChoice1;
    private RadioButton rbChoice2;
    private RadioButton rbChoice3;
    private RadioButton rbChoice4;
    private RadioButton rbChoice5;
    private Label lblLast;
    private Label lblFirst;
    private TextBox txbMax;
    private TextBox txbMin;
    private RadioButton rbPeni1;
    private RadioButton rbPeni2;
    private RadioButton rbPeni3;
    private RadioButton rbPeni4;
    private RadioButton rbDebt1;
    private RadioButton rbDebt2;
    private RadioButton rbDebt3;
    private RadioButton rbDept4;
    private CheckBox cbOnly;
    private ComboBox cmbReceipt;
    private CheckBox cbQuarter;
    private CheckBox cbDebt;
    private CheckBox cbCounter;
    private CheckBox cbTwoPage;
    private CheckBox cbCode;
    private CheckBox cbClaim;
    private CheckBox cbOther;
    private CheckBox cbOne;
    private CheckBox cbRezak;
    private CheckBox cbUslBank;
    private CheckBox cbManyOwners;
    private CheckBox cbYur;
    private CheckBox cbClose;
    private CheckBox cbDebit;
    private CheckBox cbPlat;
    private CheckBox cbMonth;
    private DateTimePicker dtpSrok;
    private Label lblProc;
    private TextBox txbProc;
    private Label lblSrok;
    private OpenFileDialog fdTwoPage;
    private TextBox txbTwoPage;
    private TextBox txbFromFile;
    private Label lblFromFile;
    private OpenFileDialog fdFromFile;
    private CheckedListBox clbDistrict;
    private ComboBox cmbGroup;
    private Button btnView;
    private GroupBox gbCounters;
    private RadioButton rbCounter3;
    private RadioButton rbCounter2;
    private RadioButton rbCounter1;
    private CheckBox cbOnlyUch;
    private CheckBox cbOnlyRent;

    public FrmReceipts()
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
    }

    public FrmReceipts(short l, int city, Raion r, Company c, Home h, LsClient lc)
    {
      this.level = l;
      this.city = city;
      this.raion = r;
      this.company = c;
      this.home = h;
      this.client = lc;
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      this.report1 = new Report();
      this.report1.Load("D:\\StarTeam\\newkvrpl\\Kvartplata\\Reports\\N10.frx");
      this.report1.Show();
    }

    private void btnPrint2_Click(object sender, EventArgs e)
    {
    }

    private void button1_Click(object sender, EventArgs e)
    {
      OleDbConnection selectConnection = new OleDbConnection(string.Format("Provider={4};Eng={0};Uid={1};Pwd={2}; Links={3}", (object) Options.BaseName, (object) Options.Login, (object) Options.Pwd, (object) ("tcpip{" + Options.Host + "}"), (object) Options.Provider));
      DataSet dataSet = new DataSet();
      dataSet.Locale = CultureInfo.InvariantCulture;
      new OleDbDataAdapter("select * from LsClient where client_id in (663000170,663000188)", selectConnection).Fill(dataSet, "Client");
      new OleDbDataAdapter("select * from lsParam where param_id=1 and client_id in (663000170,663000188)", selectConnection).Fill(dataSet, "Param");
      DataRelation relation1 = new DataRelation("ClientParam", dataSet.Tables["Client"].Columns["Client_id"], dataSet.Tables["Param"].Columns["Client_id"]);
      dataSet.Relations.Add(relation1);
      new OleDbDataAdapter("select * from lsParam where param_id=2 and client_id in (663000170,663000188)", selectConnection).Fill(dataSet, "Service");
      DataRelation relation2 = new DataRelation("ClientService", dataSet.Tables["Client"].Columns["Client_id"], dataSet.Tables["Service"].Columns["Client_id"]);
      dataSet.Relations.Add(relation2);
      try
      {
        this.report1 = new Report();
        this.report1.Load("D:\\StarTeam\\newkvrpl\\Kvartplata\\Reports\\N7.frx");
        this.report1.RegisterData(dataSet);
        (this.report1.FindObject("Data1") as DataBand).DataSource = this.report1.GetDataSource("Client");
        (this.report1.FindObject("Data2") as DataBand).DataSource = this.report1.GetDataSource("Param");
        (this.report1.FindObject("Data3") as DataBand).DataSource = this.report1.GetDataSource("Service");
        (this.report1.FindObject("Data2") as DataBand).Relation = (FastReport.Data.Relation) this.report1.FindObject("ClientParam");
        (this.report1.FindObject("Data2") as DataBand).Relation.Enabled = true;
        (this.report1.FindObject("Data3") as DataBand).Relation = (FastReport.Data.Relation) this.report1.FindObject("ClientService");
        (this.report1.FindObject("Data3") as DataBand).Relation.Enabled = true;
        this.report1.Show();
      }
      catch
      {
      }
    }

    private void button2_Click(object sender, EventArgs e)
    {
      OleDbConnection selectConnection = new OleDbConnection(string.Format("Provider={4};Eng={0};Uid={1};Pwd={2}; Links={3}", (object) Options.BaseName, (object) Options.Login, (object) Options.Pwd, (object) ("tcpip{" + Options.Host + "}"), (object) Options.Provider));
      DataSet dataSet = new DataSet();
      dataSet.Locale = CultureInfo.InvariantCulture;
      new OleDbDataAdapter("select * from LsClient where client_id in (663000170,663000188)", selectConnection).Fill(dataSet, "Client");
      new OleDbDataAdapter("select * from lsParam where client_id in (663000170,663000188) order by param_id", selectConnection).Fill(dataSet, "Param");
      DataRelation relation = new DataRelation("ClientParam", dataSet.Tables["Client"].Columns["Client_id"], dataSet.Tables["Param"].Columns["Client_id"]);
      dataSet.Relations.Add(relation);
      try
      {
        this.report1 = new Report();
        this.report1.Load("D:\\StarTeam\\newkvrpl\\Kvartplata\\Reports\\N9.frx");
        this.report1.RegisterData(dataSet);
        (this.report1.FindObject("Data1") as DataBand).DataSource = this.report1.GetDataSource("Client");
        (this.report1.FindObject("Data2") as DataBand).DataSource = this.report1.GetDataSource("Param");
        (this.report1.FindObject("Data2") as DataBand).Relation = (FastReport.Data.Relation) this.report1.FindObject("ClientParam");
        (this.report1.FindObject("Data2") as DataBand).Relation.Enabled = true;
        (this.report1.GetDataSource("Param") as TableDataSource).SelectCommand = "select * from lsParam where client_id=11111 and dbeg<=today() and dend>=today() order by param_id";
        this.report1.Show();
      }
      catch
      {
      }
    }

    private void mpCurrentPeriod_ValueChanged(object sender, EventArgs e)
    {
      Options.Period = KvrplHelper.SaveCurrentPeriod(this.mpCurrentPeriod.Value);
    }

    private void FrmReceipts_Load(object sender, EventArgs e)
    {
      this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
      this.session = Domain.CurrentSession;
      if (this.city != 3 && this.city != 5)
        this.gbDebt.Controls[3].Dispose();
      switch (this.level)
      {
        case 3:
          this.gbChoice.Controls[4].Dispose();
          this.gbChoice.Controls[3].Dispose();
          break;
        case 4:
          this.gbChoice.Enabled = false;
          break;
      }
      this.UpdateSrok();
      if (this.city == 3 || this.city == 14)
        this.gbReceipt.Visible = false;
      if (this.city == 28 && (int) this.level == 4)
        this.cbOnly.Visible = true;
      if (this.city == 1 && (this.company.Manager.BaseOrgId == 1091 || this.company.Manager.BaseOrgId == 2414 || this.company.Manager.BaseOrgId == 2415))
        ((RadioButton) this.gbPeni.Controls[3]).Checked = true;
      this.LoadList();
      if (this.city == 2)
        this.cbDebt.Enabled = true;
      if (this.city == 7 || this.city == 28)
        this.cbPlat.Checked = false;
      this.SetSetup();
      this.receipts = new int[7];
      for (int index = 0; index < 7; ++index)
        this.receipts[index] = 0;
      this.showPersData = !KvrplHelper.CheckProxy(48, 1, this.company, false) ? (short) 0 : (short) 1;
      this.typeFio = Convert.ToInt16(this.session.CreateSQLQuery("select count(*) as cnt from systable where table_name='frPers' and creator=1").List()[0]);
    }

    private void SetSetup()
    {
      this.mspMonth = Convert.ToInt32(KvrplHelper.BaseValue(3, this.company));
      this.rentMonth = Convert.ToInt32(KvrplHelper.BaseValue(5, this.company));
      this.peniBalance = Convert.ToInt32(KvrplHelper.BaseValue(18, this.company));
      this.peniOldDept = Convert.ToInt32(KvrplHelper.BaseValue(19, this.company));
      this.peniBegMonth = Convert.ToInt32(KvrplHelper.BaseValue(20, this.company));
      this.peniParam = Convert.ToInt32(KvrplHelper.BaseValue(21, this.company));
      this.peniFree = Convert.ToInt32(KvrplHelper.BaseValue(22, this.company));
      this.peniAlg = Convert.ToInt32(KvrplHelper.BaseValue(23, this.company));
      this.socSaldo = Convert.ToInt32(KvrplHelper.BaseValue(25, this.company));
    }

    private void radioButton5_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void cbPlat_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void UpdateSrok()
    {
      switch (this.city)
      {
        case 23:
        case 24:
        case 2:
          Options.Period.PeriodName.Value.AddDays(24.0);
          break;
        case 6:
          this.dtpSrok.Value = Options.Period.PeriodName.Value.AddDays(19.0);
          break;
        default:
          Options.Period.PeriodName.Value.AddDays(9.0);
          break;
      }
    }

    private void LoadList()
    {
      try
      {
        this.cmbReceipt.DataSource = (object) this.session.CreateQuery("select r from CmpReceipt cr,Receipt r where r.ReceiptId=cr.ReceiptId and r.ReceiptId<>0 " + (this.company == null ? " and cr.CompanyId = (select min CompanyId from CmpReceipt)" : string.Format(" and cr.CompanyId={0}", (object) this.company.CompanyId)) + " order by r.ReceiptId").List<Receipt>();
        this.cmbReceipt.DisplayMember = "ReceiptName";
        this.cmbReceipt.ValueMember = "ReceiptId";
      }
      catch
      {
      }
    }

    private void btnView_Click(object sender, EventArgs e)
    {
      if (this.cmbReceipt.SelectedItem == null)
      {
        int num1 = (int) MessageBox.Show("Выберите квитанцию для печати", "Ошибка!", MessageBoxButtons.OK);
      }
      else
      {
        string strReceipt = " and receipt_id in (" + this.cmbReceipt.SelectedValue.ToString() + ")";
        this.receipts[0] = Convert.ToInt32(this.cmbReceipt.SelectedValue);
        string queryString = "";
        this.uTmp = Options.Login + ".tmpReceipt";
        string strPeriod = string.Format("cast('{0}' as date)", (object) KvrplHelper.DateTimeToBaseFormat(Options.Period.PeriodName.Value));
        string strLDPeriod = string.Format("cast('{0}' as date)", (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(Options.Period.PeriodName.Value)));
        string strPeriodId = Options.Period.PeriodId.ToString();
        if (!this.IsClosedPeriod())
          return;
        OleDbConnection oleDbConnection = new OleDbConnection(string.Format("Provider={4};Eng={0};Uid={1};Pwd={2}; Links={3}", (object) Options.BaseName, (object) Options.Login, (object) Options.Pwd, (object) ("tcpip{" + Options.Host + "}"), (object) Options.Provider));
        this.report1 = new Report();
        this.report1.Load(AppDomain.CurrentDomain.BaseDirectory + "Receipts1\\KvitYarDze.frx");
        this.report1.Dictionary.Connections[0].ConnectionString = string.Format("Dsn={0};uid={1};pwd={2}", (object) Options.Alias, (object) Options.Login, (object) Options.Pwd);
        string str1 = !this.cbUslBank.Checked || !(this.txbProc.Text != "") ? " 0 as bankproc, " : "(if out>0 then " + this.txbProc.Text + "*out/100 else '+edtProc.Text+'*peni/100 endif) as bankproc,";
        string str2 = this.MainUsl();
        try
        {
          string str3 = (int) this.level <= 1 ? "" : " and sp.company_id=" + this.company.CompanyId.ToString();
          if (this.txbFromFile.Text != "")
            queryString = "insert into " + this.uTmp + "printserv select distinct ls.client_id,ls.idhome,sp.service_id as root,s.service_id,isnull(ss.supplier_id,0),0   from lsClient ls,cmpServiceParam sp,dcService s,cmpSupplier ss," + this.uTmp + "lic lic   where ls.complex_id=100 and ls.client_id=lic.client_id      and sp.company_id=ls.company_id and sp.complex_id=100 and sp.service_id=s.root      and ss.company_id=(select first param_value from cmpParam where period_id=0 and dbeg<=" + strPeriod + " and dend>=" + strPeriod + " and param_id=211 and company_id=ls.company_id)      and (ss.service_id=sp.service_id or ss.service_id=(select crossservice_id from cmpCrossService where company_id=sp.company_id and dbeg<=" + strPeriod + " and dend>=" + strPeriod + " and service_id=sp.service_id and crosstype_id=6))      and (ss.supplier_id in (select supplier_id from lsBalance where client_id=ls.client_id) or ss.supplier_id in (select supplier_id from lsSupplier where client_id=ls.client_id)) union all select ls.client_id,ls.idhome,sp.service_id as root,s.service_id,0,0   from lsClient ls,cmpServiceParam sp,dcService s," + this.uTmp + "lic lic   where ls.complex_id=100 and ls.client_id=lic.client_id      and sp.company_id=ls.company_id and sp.complex_id=100 and sp.service_id=s.root ";
          else
            queryString = "call droptmp(1,'tmpReceiptnabor',current user); call droptmp(1,'tmpReceiptprintserv',current user); create " + this.MP_Global1 + " table " + this.uTmp + "nabor(company_id integer not null,root integer,service_id integer,supplier_id integer)" + this.MP_Global2 + "; create index company_id on " + this.uTmp + "nabor (company_id); create index supplier_id on " + this.uTmp + "nabor (supplier_id); create index sost on " + this.uTmp + "nabor (company_id,supplier_id);create index sost1 on " + this.uTmp + "nabor (root,service_id,supplier_id);create " + this.MP_Global1 + " table " + this.uTmp + "printserv(client_id integer,idhome integer,root integer,service_id integer,supplier_id integer,receipt_id integer)" + this.MP_Global2 + "; create index client_id on " + this.uTmp + "printserv (client_id);create index root on " + this.uTmp + "printserv (root); create index clr on " + this.uTmp + "printserv (client_id,receipt_id);create index css on " + this.uTmp + "printserv (client_id,service_id,supplier_id); create index crs on " + this.uTmp + "printserv (client_id,root,service_id,supplier_id);insert into " + this.uTmp + "nabor select distinct sp.company_id,s.root as root,s.service_id,ss.supplier_id   from dcService s,cmpServiceParam sp, cmpSupplier ss  where  sp.service_id=s.root and sp.complex_id=100 " + str3 + "  and ss.company_id=(select first param_value from cmpParam where period_id=0 and dbeg<=" + strPeriod + " and dend>=" + strPeriod + " and param_id=211 and company_id=sp.company_id)   and (ss.service_id=sp.service_id or ss.service_id=(select crossservice_id from cmpCrossService where company_id=sp.company_id and dbeg<=" + strPeriod + " and dend>=" + strPeriod + " and service_id=sp.service_id and crosstype_id=6)) and ss.supplier_id<>0 union all  select sp.company_id,s.root as root,s.service_id,0    from dcService s,cmpServiceParam sp where  sp.service_id=s.root " + str3 + " and sp.complex_id=100;  call droptmp(1,'tmpReceiptlssupl',current user); create  " + this.MP_Global1 + " table " + this.uTmp + "lssupl(client_id integer not null,supplier_id integer)" + this.MP_Global2 + ";  create index client_id on " + this.uTmp + "lssupl (client_id);  create index supplier_id on " + this.uTmp + "lssupl (supplier_id); create index sost on " + this.uTmp + "lssupl (client_id,supplier_id); ";
          this.session.CreateSQLQuery(queryString).ExecuteUpdate();
          queryString = "insert into " + this.uTmp + "lssupl select b.client_id,b.supplier_id  from lsBalance b,dcCompany c,lsClient ls,flats f where b.period_id=" + strPeriodId + " and b.client_id=ls.client_id and ls.complex_id=100                 and ls.company_id=c.company_id and ls.idflat=f.idflat and " + str2 + "; insert into " + this.uTmp + "lssupl select b.client_id,b.supplier_id  from lsBalancePeni b,dcCompany c,lsClient ls,flats f where b.period_id=" + strPeriodId + " and b.client_id=ls.client_id and ls.complex_id=100                 and ls.company_id=c.company_id and ls.idflat=f.idflat and " + str2 + "; insert into " + this.uTmp + "lssupl select distinct b.client_id,b.supplier_id from lsSupplier b,dcCompany c,lsClient ls,flats f where b.client_id=ls.client_id and ls.complex_id=100                 and ls.company_id=c.company_id and ls.idflat=f.idflat and " + str2 + "; ";
          this.session.CreateSQLQuery(queryString).ExecuteUpdate();
          queryString = "insert into " + this.uTmp + "printserv  select ls.client_id,ls.idhome,tmp.root as root,tmp.service_id,tmp.supplier_id,0 from " + this.uTmp + "nabor tmp,dcCompany c,lsClient ls,flats f where ls.company_id=tmp.company_id  and (exists (select 1 from " + this.uTmp + "lssupl where client_id=ls.client_id and supplier_id=tmp.supplier_id) or supplier_id=0) and ls.complex_id=100  and ls.company_id=c.company_id and ls.idflat=f.idflat  and " + str2 + ";";
          this.session.CreateSQLQuery(queryString).ExecuteUpdate();
          this.session.CreateSQLQuery(KvrplHelper.Prc_FillReceipt(this.uTmp + "printserv", strPeriod)).ExecuteUpdate();
          if ((Receipt) this.cmbReceipt.SelectedItem != null)
            this.session.CreateSQLQuery("delete from " + this.uTmp + "printserv where receipt_id  not in (" + ((Receipt) this.cmbReceipt.SelectedItem).ReceiptId.ToString() + ")").ExecuteUpdate();
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        string strUsl = str2 + this.MinMaxUsl() + this.DopUsl(strReceipt, strPeriodId) + this.DebtUsl();
        if (this.cbClose.Checked)
        {
          queryString = "4,5";
          if ((int) this.level != 4)
            queryString += ",3";
        }
        else if ((int) this.level != 4 && !this.cbOnlyUch.Checked)
          queryString = "3";
        if (this.cbYur.Checked)
          queryString = !(queryString == "") ? queryString + ",8" : queryString + "8";
        if (queryString != "")
          strUsl = strUsl + " and isnull((select param_value from lsparam where client_id=ls.client_id and period_id=0 and dbeg<=" + strPeriod + " and dend>=" + strPeriod + " and param_id=107),0) not in (" + queryString + ")";
        if (this.cbOnlyUch.Checked)
          strUsl = strUsl + " and isnull((select first param_value from lsparam where client_id=ls.client_id and period_id=0 and dbeg<=" + strPeriod + " and dend>=" + strPeriod + " and param_id=107),0)=3";
        if (this.cbOnlyRent.Checked)
        {
          string str3 = strUsl + " and (";
          for (int index = 0; index <= 1; ++index)
            str3 = str3 + "isnull((select sum(isnull(rent,0)-isnull(rent_past,0)) from lsBalance b," + this.uTmp + "printserv r where period_id=" + strPeriodId + " and b.client_id=ls.client_id       and r.client_id=ls.client_id and b.service_id=r.service_id and r.supplier_id=b.supplier_id and r.receipt_id in (" + this.receipts[index].ToString() + ") ),0)<>0 or ";
          strUsl = str3.Substring(0, str3.Length - 3) + " )";
        }
        this.BuildList(strUsl, strPeriod);
        this.CuttingMachineSorter();
        if (this.session.CreateSQLQuery("select * from " + this.uTmp + "printlic").List().Count == 0)
        {
          int num2 = (int) MessageBox.Show("Нет данных для отчета");
        }
        else
        {
          this.LoadHomevol(strPeriodId, strPeriod);
          this.MakeQuery(strReceipt, strPeriod, strPeriodId, strLDPeriod);
        }
      }
    }

    private bool IsClosedPeriod()
    {
      string queryString = "";
      if (this.socSaldo != 1)
        queryString = " and complex_id<>104";
      switch (this.level)
      {
        case 0:
        case 1:
          queryString = "select min(period_id) as period_id from cmpPeriod where company_id in (select company_id from dcCompany where rnn_id=" + this.raion.IdRnn.ToString() + ")" + queryString;
          break;
        case 2:
        case 3:
          queryString = "select min(period_id) as period_id from cmpPeriod where company_id=" + this.company.CompanyId.ToString() + queryString;
          break;
      }
      if ((int) this.level >= 4 || Options.Period.PeriodId <= Convert.ToInt32(this.session.CreateSQLQuery(queryString).List()[0]) || this.city == 3)
        return true;
      int num = (int) MessageBox.Show("Невозможно распечатать квитанцию, месяц не закрыт", "Ошибка!", MessageBoxButtons.OK);
      return false;
    }

    private void txbProc_KeyPress(object sender, KeyPressEventArgs e)
    {
      if ((int) e.KeyChar == 8 || (int) e.KeyChar == 13 || ((int) e.KeyChar == 44 || (int) e.KeyChar == 45) || (int) e.KeyChar == 46 || (int) e.KeyChar >= 48 && (int) e.KeyChar <= 57)
        return;
      e.Handled = true;
    }

    private string MainUsl()
    {
      string str = "";
      if ((int) this.level > 0)
        str = " c.rnn_id=" + this.raion.IdRnn.ToString();
      if ((int) this.level > 1)
        str = str + " and ls.company_id=" + this.company.CompanyId.ToString();
      if ((int) this.level > 2)
        str = str + " and ls.idhome=" + this.home.IdHome.ToString();
      if ((int) this.level > 3)
        str = str + " and ls.client_id=" + this.client.ClientId.ToString();
      return str;
    }

    private string MinMaxUsl()
    {
      string str1 = "";
      string str2 = "";
      if (this.txbMin.Text != "0" && this.txbMax.Text != "0" || this.cmbGroup.Text != "")
      {
        if (this.rbChoice2.Checked)
          str1 = str1 + " and ls.client_id>=" + this.txbMin.Text + " and ls.client_id<=" + this.txbMax.Text;
        if (this.rbChoice3.Checked)
          str1 = str1 + " and lengthhome(f.nflat)>=" + this.txbMin.Text + " and lengthhome(f.nflat)<=" + this.txbMax.Text;
        if (this.rbChoice4.Checked)
        {
          for (int index = 0; index <= this.clbDistrict.Items.Count - 1; ++index)
            str2 = str2.Substring(2, str2.Length);
          if (str2 != "")
            str1 = str1 + " and ls.idhome in (select idhome from districthome where iddistrict in (" + str2 + "))";
        }
        if (this.rbChoice5.Checked)
          str1 = str1 + " and (select kodhome from homes hm where idhome=ls.idhome)>=" + this.txbMin.Text + " and (select kodhome from homes hm where idhome=ls.idhome)<=" + this.txbMax.Text;
      }
      return str1;
    }

    private string DopUsl(string strReceipt, string strPeriodId)
    {
      string str1 = "";
      if (!this.cbMonth.Checked)
      {
        if (this.cbPlat.Checked)
        {
          string str2 = str1 + " and (";
          for (int index = 0; index < 7; ++index)
            str2 = str2 + "isnull((select sum(isnull(balance_out,0)) from lsBalance b," + this.uTmp + "printserv r where period_id=" + strPeriodId + " and b.client_id=ls.client_id       and r.client_id=ls.client_id and b.service_id=r.service_id and r.supplier_id=b.supplier_id and r.receipt_id in (" + this.receipts[index].ToString() + ")),0)>0 or ";
          str1 = str2.Remove(str2.Length - 3, 2) + " )";
        }
        if (this.cbDebit.Checked)
        {
          string str2 = str1 + " and (" + "isnull((select sum(isnull(balance_out,0)) from lsBalance b" + this.uTmp + "printserv r where period_id=" + strPeriodId + " and b.client_id=ls.client_id       and r.client_id=ls.client_id and b.service_id=r.service_id and r.supplier_id=b.supplier_id " + strReceipt + " ),0)<0 or  " + strReceipt + " ),0)<0 or ";
          str1 = str2.Remove(str2.Length - 3, 2) + "  ) and isnull((select sum(isnull(balance_out,0)) from lsBalancePeni b where period_id=" + strPeriodId + " and b.client_id=ls.client_id ),0)<=0";
        }
      }
      else
      {
        if (this.cbPlat.Checked)
        {
          string str2 = str1 + " and (";
          for (int index = 0; index < 7; ++index)
            str2 = str2 + "isnull((select sum(isnull(rent,0)) from lsBalance b," + this.uTmp + "printserv r where period_id=" + strPeriodId + " and b.client_id=ls.client_id       and r.client_id=ls.client_id and b.service_id=r.service_id and r.supplier_id=b.supplier_id and r.receipt_id in (" + this.receipts[index].ToString() + ")),0)>0 or" + this.receipts[index].ToString() + ")),0)>0 or ";
          str1 = str2.Remove(str2.Length - 3, 2) + " )";
        }
        if (this.cbDebit.Checked)
        {
          string str2 = str1 + " and (";
          for (int index = 0; index < 7; ++index)
            str2 = str2 + "isnull((select sum(isnull(rent,0)) from lsBalance b," + this.uTmp + "printserv r where period_id=" + strPeriodId + " and b.client_id=ls.client_id       and r.client_id=ls.client_id and b.service_id=r.service_id and r.supplier_id=b.supplier_id " + strReceipt + " ),0)<0 or ";
          str1 = str2.Remove(str2.Length - 3, 2) + " ) and (select sum(isnull(rent,0)) from lsBalancePeni where client_id=ls.client_id and period_id=" + strPeriodId + ")<=0";
        }
      }
      return str1;
    }

    private string DebtUsl()
    {
      string str1 = "";
      string str2 = "";
      if (this.city == 3)
        str2 = " and service_id in (select s.service_id from dcService s,cmpServiceParam sp where sp.service_id=s.root and sp.receipt_id in (1,6) and sp.company_id=ls.company_id)";
      if (!this.rbDebt1.Checked)
      {
        if (this.rbDebt2.Checked)
          str1 = str1 + " and (select max(period_id-month_dept_id) from lsBalance where period_id=" + Options.Period.PeriodId.ToString() + " and client_id=ls.client_id" + str2 + ")<=2";
        if (this.rbDebt3.Checked)
        {
          if (this.gbDebt.Controls.Count == 3)
            str1 = str1 + " and (select max(period_id-month_dept_id) from lsBalance where period_id=" + Options.Period.PeriodId.ToString() + " and client_id=ls.client_id" + str2 + ")>2";
          else
            str1 = str1 + " and (select max(period_id-month_dept_id) from lsBalance where period_id=" + Options.Period.PeriodId.ToString() + " and client_id=ls.client_id" + str2 + ") between 3 and 5 ";
        }
        if (this.rbDept4.Checked)
          str1 = str1 + " and (select max(period_id-month_dept_id) from lsBalance where period_id=" + Options.Period.PeriodId.ToString() + " and client_id=ls.client_id" + str2 + ")>=6";
      }
      return str1;
    }

    private void BuildList(string strUsl, string strPeriod)
    {
      try
      {
        this.session.CreateSQLQuery("call droptmp(1,'tmpReceiptprintlic',current user);call droptmp(1,'tmpReceiptprintsortlic',current user);create " + this.MP_Global1 + " table " + this.uTmp + "printlic(num integer not null default autoincrement primary key,client_id integer,uname username)" + this.MP_Global2 + ";create " + this.MP_Global1 + " table " + this.uTmp + "printsortlic(num integer not null default autoincrement primary key,client_id integer)" + this.MP_Global2 + ";create index client_id on " + this.uTmp + "printlic (client_id);create index num on " + this.uTmp + "printlic (num);create index client_id on " + this.uTmp + "printsortlic (client_id)").ExecuteUpdate();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      try
      {
        this.session.CreateSQLQuery("insert into " + this.uTmp + "printsortlic (client_id) select distinct client_id from (select ls.client_id from lsClient ls,di_str d,homes h,homelink hl,flats f,dcCompany c                         where " + strUsl + " and c.company_id=ls.company_id and hl.idhome=h.idhome and hl.codeu=ls.company_id and h.idhome=ls.idhome and h.idstr=d.idstr and ls.complex_id=100                                 and ls.idflat=f.idflat and ls.client_id not in (select client_id from lsParam where period_id=0 and param_id=54 and dbeg<=" + strPeriod + " and dend>=" + strPeriod + " and param_value=1 and " + this.level.ToString() + "<>4) order by ls.company_id,d.str,lengthhome(h.home),h.home,h.home_korp,lengthhome(f.nflat),lengthhome(ls.numberroom)) as my").ExecuteUpdate();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void CuttingMachineSorter()
    {
      if (this.cbRezak.Checked)
      {
        int num1;
        try
        {
          num1 = Convert.ToInt32(this.session.CreateSQLQuery("select max(num) from " + this.uTmp + "printsortlic").List()[0]);
        }
        catch (Exception ex)
        {
          num1 = 0;
        }
        int num2 = num1 % 2 != 0 ? num1 / 2 + 1 : num1 / 2;
        for (int index = 1; index <= num2; ++index)
          this.session.CreateSQLQuery("insert into " + this.uTmp + "printlic (client_id,uname) select client_id,user                                    from " + this.uTmp + "printsortlic t where (num=" + index.ToString() + " or num=" + (index + num2).ToString() + ") order by num ").ExecuteUpdate();
      }
      else
        this.session.CreateSQLQuery("insert into " + this.uTmp + "printlic (client_id,uname) select client_id,user                                    from " + this.uTmp + "printsortlic t order by num ").ExecuteUpdate();
    }

    private void MakeQuery(string strReceipt, string strPeriod, string strPeriodId, string strLDPeriod)
    {
      string str1 = "";
      string str2 = "";
      string str3 = ",'' as nameorg ";
      string str4 = "";
      int int32 = Convert.ToInt32(strPeriodId);
      string str5;
      if (!this.cbQuarter.Checked)
      {
        str5 = strPeriodId;
      }
      else
      {
        string[] strArray = new string[5];
        int index1 = 0;
        int num = int32 - 2;
        string str6 = num.ToString();
        strArray[index1] = str6;
        int index2 = 1;
        string str7 = ",";
        strArray[index2] = str7;
        int index3 = 2;
        num = int32 - 1;
        string str8 = num.ToString();
        strArray[index3] = str8;
        int index4 = 3;
        string str9 = ",";
        strArray[index4] = str9;
        int index5 = 4;
        string str10 = int32.ToString();
        strArray[index5] = str10;
        str5 = string.Concat(strArray);
      }
      str1 = this.cbOnly.Checked ? " and service_id=ss.service_id and supplier_id=supl.supplier_id" : " and service_id in (select service_id from dcService where root=r.service_id)";
      string str11;
      if (!this.cbManyOwners.Checked)
        str11 = "(if " + this.showPersData.ToString() + "=1 then " + KvrplHelper.NS_lsOwner(1, 2, strPeriod, strPeriod, "ls.client_id", strLDPeriod) + " else ' ' endif) as fio,";
      else
        str11 = "(if " + this.showPersData.ToString() + "=1 then " + this.NS_lsOwners(1, 2, strPeriod, strPeriod, "ls.client_id", strLDPeriod) + " else ' ' endif) as fio,";
      try
      {
        TableDataSource dataSource1 = this.report1.GetDataSource("Main") as TableDataSource;
        dataSource1.SelectCommand = "select ls.client_id,cast(ls.idhome as integer) as idhome,cast('" + KvrplHelper.DateTimeToBaseFormat(Options.Period.PeriodName.Value) + "' as date) as period," + str4 + str11 + "ls.numberroom,ls.idhome,ls.floor,d.str,h.home,h.home_korp,f.nflat,c.company_name,c.company_sname,c.address,c.worktime,c.worktimecash,        cast(isnull(" + this.NS_lsPerson_cnt(1, 1, strPeriod, strPeriod, "ls.client_id", strLDPeriod) + ",0) as smallint) as numjilec,         (select first phone from hmReceipt kh,di_phonesserv ps where kh.idservice=ps.idservice and idhome=h.idhome and company_id=ls.company_id and (kh.idservice=1 or nameservice like '%бухгалтери%' or nameservice like '%Бухгалтери%') and dbeg<=" + strPeriod + " and dend>=" + strPeriod + ") as phoneacc,        (select first phone from hmReceipt kh,di_phonesserv ps where kh.idservice=ps.idservice and idhome=h.idhome and company_id=ls.company_id and (kh.idservice=1 or nameservice like '%интернет%' or nameservice like '%Интернет%') and dbeg<=" + strPeriod + " and dend>=" + strPeriod + ") as email,        (select first note from hmReceipt kh,di_phonesserv ps where kh.idservice=ps.idservice and idhome=h.idhome and company_id=ls.company_id and (kh.idservice=1 or nameservice like '%интернет%' or nameservice like '%Интернет%') and dbeg<=" + strPeriod + " and dend>=" + strPeriod + ") as site,       (select first note from hmReceipt kh,di_phonesserv ps where kh.idservice=ps.idservice and idhome=h.idhome and company_id=ls.company_id and (kh.idservice=1 or nameservice like '%бухгалтери%' or nameservice like '%Бухгалтери%') and dbeg<=" + strPeriod + " and dend>=" + strPeriod + ") as messages,       (select list(note) from hmReceipt kh,di_phonesserv ps where kh.idservice=ps.idservice and idhome=h.idhome and company_id=ls.company_id and kh.client_id=0                  and kh.idservice<>1 and not (nameservice like '%бухгалтери%' or nameservice like '%Бухгалтери%' or nameservice like '%интернет%' or nameservice like '%Интернет%') and dbeg<=" + strPeriod + " and dend>=" + strPeriod + ") as homemess,       (select list(note) from hmReceipt kh,di_phonesserv ps where kh.idservice=ps.idservice and idhome=h.idhome and company_id=ls.company_id and kh.client_id=ls.client_id                  and kh.idservice<>1 and not (nameservice like '%бухгалтери%' or nameservice like '%Бухгалтери%' or nameservice like '%интернет%' or nameservice like '%Интернет%') and dbeg<=" + strPeriod + " and dend>=" + strPeriod + ") as clientmess,       (if clientmess<>'' then clientmess||' '||homemess else homemess endif) as mess,       (select sum(payment_value) from lsPayment where period_id=" + strPeriodId + " and client_id=ls.client_id and purposepay_id=3 and receipt_id=r.receipt_id) as sumstar,        (select sum(payment_value) from lsPayment where period_id=" + strPeriodId + " and client_id=ls.client_id and purposepay_id=2) as sumcomp,        cast(isnull(" + this.NS_lsPerson_cnt(1, 2, strPeriod, strPeriod, "ls.client_id", strLDPeriod) + ",0) as smallint) as tmpnumjilec," + this.NS_lsParam_value(1, 2, strPeriod, "ls.client_id", strLDPeriod) + " as flpl, (if flpl<>0 then flpl else " + this.NS_lsParam_value(1, 2, strPeriod, "ls.client_id", strPeriod) + " endif) as flatpl,        (select sum(incoming) from soc_saldo where period=" + strPeriod + " and idlic=ls.client_id and numserv in (select root from " + this.uTmp + "printserv r where r.client_id=ls.client_id " + strReceipt + " and supplier_id=0)) as incsoc,       (select isnull(sum(calc),0)+isnull(sum(past),0) from soc_saldo where period=" + strPeriod + " and idlic=ls.client_id and numserv in (select root from " + this.uTmp + "printserv r where r.client_id=ls.client_id " + strReceipt + " and supplier_id=0)) as calcsoc,       (select sum(pay) from soc_saldo where period=" + strPeriod + " and idlic=ls.client_id and numserv in (select root from " + this.uTmp + "printserv r where r.client_id=ls.client_id " + strReceipt + " and supplier_id=0)) as topaysoc,       (select sum(outcoming) from soc_saldo where period=" + strPeriod + " and idlic=ls.client_id and numserv in (select root from " + this.uTmp + "printserv r where r.client_id=ls.client_id " + strReceipt + " and supplier_id=0)) as outsoc,       (select sum(subsum) from jilsub where period=" + strPeriod + " and idlic=ls.client_id and codedoc not in (2010,2011,2012,3010,3011) and (" + this.cmbReceipt.SelectedValue.ToString() + " in (1,2)  and " + this.city.ToString() + "=1)) as subsid, months(" + strPeriod + ",1) as nextperiod,months(" + strPeriod + ",-1) as befperiod,c.company_id,c.manager_id,bo.nameorg_min as shiefregion,'' as bik, '' as coraccount,'' as schet, ";
        if (this.city == 1 || this.city == 5 || (this.city == 6 || this.city == 9) || (this.city == 10 || this.city == 11 || (this.city == 18 || this.city == 23)) || (this.city == 24 || this.city == 25 || (this.city == 26 || this.city == 29)) || this.city == 30)
          dataSource1.SelectCommand = dataSource1.SelectCommand + " r.supplier_id as manager,(select list (MSP_id) from lsMSPGKU lm where dbeg<=" + strLDPeriod + " and isnull(dend,'2999-12-31')>=" + strLDPeriod + " and client_id=ls.client_id and isnull((select codesoc from dcMSP where msp_id=lm.msp_id),0)=0) as codehelp, (select list(codesoc) from lsMSPGKU lm,dcMSP dm where lm.msp_id=dm.msp_id and dbeg<=" + strLDPeriod + " and isnull(dend,'2999-12-31')>=" + strLDPeriod + " and client_id=ls.client_id and isnull((select codesoc from dcMSP where msp_id=lm.msp_id),0)<>0) as codesoc ";
        else
          dataSource1.SelectCommand = dataSource1.SelectCommand + " (select list (MSP_id) from lsMSPGKU where dbeg<=" + strLDPeriod + " and isnull(dend,'2999-12-31')>" + strLDPeriod + " and client_id=ls.client_id) as codehelp ";
        dataSource1.SelectCommand = dataSource1.SelectCommand + " from lsClient ls,dcCompany c left outer join base_org bo on c.manager_id=bo.idbaseorg,di_str d,homes h,homelink hl,flats f," + this.uTmp + "printlic p,cmpReceipt r  where c.company_id=ls.company_id and ls.client_id=p.client_id and hl.idhome=h.idhome and hl.codeu=ls.company_id and        h.idhome=ls.idhome and h.idstr=d.idstr and ls.idflat=f.idflat and p.uname='" + Options.Login + "' and r.company_id=ls.company_id " + strReceipt + " order by p.num";
        string str6 = "select (select client_id from lsClient where client_id=:client_id) as client_id,r.company_id,r.service_id,p.period_value,t.basetariffmsp_id,         isnull((select first receipt_id from " + this.uTmp + "printserv where root=r.service_id),0) as receipt_id,         (if isnull(t.tariff_id,0)<>0 then r.service_id else isnull((select first crossservice_id from cmpCrossService ccs where company_id=:company_id and ccs.dbeg<=" + strPeriod + " and ccs.dend>=" + strPeriod + " and service_id=r.service_id and crosstype_id=6),r.service_id) endif) as crserv,          isnull((select first service_id from cmpCrossService where company_id=:company_id and dbeg<=" + strPeriod + " and dend>=" + strPeriod + " and crossservice_id=r.service_id and crosstype_id=6),0) servcr,         isnull((if gr=0  then (if isnull((select unitmeasuring_name from dcUnitMeasuring where unitmeasuring_id=t.unitmeasuring_id),'')<>'' then          (select first unitmeasuring_name from dcUnitMeasuring where unitmeasuring_id=t.unitmeasuring_id) else          (select first unitmeasuring_name from dcUnitMeasuring where unitmeasuring_id=(select first unitmeasuring_id from cmpTariff ctr where (service_id=crserv and " + this.city.ToString() + "<>7) or (service_id=r.service_id and " + this.city.ToString() + "=7))) endif)          else if t.unitmeasuring_id is not null then (select unitmeasuring_name from dcUnitMeasuring where unitmeasuring_id=t.unitmeasuring_id)          else (select first unitmeasuring_name from dcUnitMeasuring where unitmeasuring_id=(select first unitmeasuring_id from cmpTariff ctr where service_id=gr)) endif endif),'') as edizm,          (select first setup_value from fkSetup where manager_id=:manager_id and setup_id=3 and dbeg<=" + strPeriod + " and dend>=" + strPeriod + ") as setup_day,         (select first setup_value from fkSetup where manager_id=:manager_id and setup_id=4 and dbeg<=" + strPeriod + " and dend>=" + strPeriod + ") as setup_val,         isnull((select sum(isnull(balance_in,0)) from lsBalance b," + this.uTmp + "printserv prc1 where period_id=p.period_id and b.client_id=:client_id and b.client_id=prc1.client_id and prc1.root=r.service_id and b.service_id=prc1.service_id and b.supplier_id=prc1.supplier_id),0) as dolg,         isnull((select sum(isnull(balance_out,0)) from lsBalance b," + this.uTmp + "printserv prc1 where period_id=p.period_id and b.client_id=:client_id and b.client_id=prc1.client_id and prc1.root=r.service_id and b.service_id=prc1.service_id and b.supplier_id=prc1.supplier_id),0) as out,         isnull((select sum(isnull(payment,0)) from lsBalance b," + this.uTmp + "printserv prc1 where period_id=p.period_id and b.client_id=:client_id and b.client_id=prc1.client_id and prc1.root=r.service_id and b.service_id=prc1.service_id and b.supplier_id=prc1.supplier_id),0) as pay,         isnull((select sum(isnull(rent_comp,0)) from lsBalance b," + this.uTmp + "printserv prc1 where period_id=p.period_id and b.client_id=:client_id and b.client_id=prc1.client_id and prc1.root=r.service_id and b.service_id=prc1.service_id and b.supplier_id=prc1.supplier_id),0) as sumcomp,         isnull((select sum(isnull(rent,0)) from lsRentMSP b," + this.uTmp + "printserv prc1 where period_id=" + strPeriodId + " and b.client_id=:client_id and b.client_id=prc1.client_id                 and prc1.root=r.service_id and b.service_id=prc1.service_id and b.supplier_id=prc1.supplier_id and msp_id in (select msp_id from dcMsp where mspperiod_id>" + strPeriodId + ")),0) as help1,         isnull((select sum(isnull(rent,0)) from lsRentMSP b," + this.uTmp + "printserv prc1 where b.period_id=p.period_id and b.client_id=:client_id and code<>0 and b.client_id=prc1.client_id                 and prc1.root=r.service_id and b.service_id=prc1.service_id and b.supplier_id=prc1.supplier_id and msp_id in (select msp_id from dcMsp where mspperiod_id>p.period_id)),0) as pastmsp,          isnull((select sum(isnull(rent,0)) from lsRentMSP b," + this.uTmp + "printserv prc1 where period_id=p.period_id and b.client_id=:client_id and b.client_id=prc1.client_id                  and prc1.root=r.service_id and b.service_id=prc1.service_id and b.supplier_id=prc1.supplier_id and msp_id in (select msp_id from dcMsp where mspperiod_id<=p.period_id)),0) as comphelp,          isnull((if setup_day<=" + strPeriodId + " then if setup_val=1 then 0 else help1 endif else help1 endif),0) as help,         r.group_num as gr,          isnull((select first param_value from lsParam where period_id=0 and client_id=:client_id and dbeg<=" + strPeriod + " and dend>=" + strPeriod + " and param_id=104),0) as doc,         isnull((select first coeff from cntrDetail where period_id=" + strPeriodId + " and month_id=" + strPeriodId + " and counter_id in (select counter_id from cntrRelation where client_id=:client_id and period_id=0 and dbeg<=" + strPeriod + " and dend>" + strPeriod + ")          and service_id=r.service_id),1)*isnull((select param_value from lsServiceParam where client_id=:client_id and period_id=0 and dbeg<=" + strPeriod + " and dend>=" + strPeriod + " and service_id=r.service_id and param_id=401),1) as coeff,         isnull((select max(volume) from lsRent b where period_id=" + strPeriodId + " and b.client_id=:client_id and service_id in (select service_id from " + this.uTmp + "printserv where client_id=b.client_id and root=servcr) and code=0 and rent_type=0),0)+         isnull((select max(volume) from lsRent b where period_id=" + strPeriodId + " and b.client_id=:client_id and service_id in (select service_id from " + this.uTmp + "printserv where client_id=b.client_id and root=servcr) and code=0 and rent_type=1),0) as odnvolume,         isnull((select sum(isnull(rent,0))-sum(isnull(rent_past,0)) from lsBalance b where period_id=" + strPeriodId + " and client_id=:client_id and service_id in (select service_id from " + this.uTmp + "printserv where client_id=b.client_id and root=servcr)),0) as odncalc,         isnull((select sum(isnull(balance_out,0)) from lsBalance b where period_id=" + strPeriodId + " and client_id=:client_id and service_id in (select service_id from " + this.uTmp + "printserv where client_id=b.client_id and root=servcr)),0) as odnout,         isnull((select sum(isnull(balance_in,0)) from lsBalance b where period_id=" + strPeriodId + " and client_id=:client_id and service_id in (select service_id from " + this.uTmp + "printserv where client_id=b.client_id and root=servcr)),0) as odndolg,         isnull((select sum(isnull(payment,0)) from lsBalance b where period_id=" + strPeriodId + " and client_id=:client_id and service_id in (select service_id from " + this.uTmp + "printserv where client_id=b.client_id and root=servcr)),0) as odnpay,         isnull((select sum(isnull(rent,0)) from lsRentMSP b where period_id=" + strPeriodId + " and client_id=:client_id and service_id in                 (select service_id from " + this.uTmp + "printserv where client_id=b.client_id and root=servcr) and msp_id in (select msp_id from dcMsp where mspperiod_id>" + strPeriodId + ")),0)  as odnhelp1,         isnull((if setup_day<=" + strPeriodId + " then if setup_val=1 then 0 else odnhelp1 endif else odnhelp1 endif),0) as odnhelp,         isnull((select sum(isnull(rent,0)) from lsRentMSP b where period_id=p.period_id and client_id=:client_id and service_id in                 (select service_id from " + this.uTmp + "printserv where client_id=b.client_id and root=servcr) and msp_id in (select msp_id from dcMsp where mspperiod_id<=p.period_id)),0) as odncomphelp,          isnull((select sum(isnull(rent,0)) from lsRent b where period_id=" + strPeriodId + " and client_id=:client_id and service_id in (select service_id from " + this.uTmp + "printserv where client_id=b.client_id and root=servcr) and code<>0),0) as odncalcpast,         isnull(t.cost_eo,0) as tax_eo,         (select list(distinct rent_type+1) from lsRent b where period_id=" + strPeriodId + " and client_id=:client_id and service_id in (select service_id from " + this.uTmp + "printserv where client_id=b.client_id and root=r.service_id)) as typevol,         (select list(distinct rent_type+1) from lsRent b where period_id=" + strPeriodId + " and client_id=:client_id and service_id in (select service_id from " + this.uTmp + "printserv where client_id=b.client_id and root=servcr)) as typeodnvol,         isnull((select first n.norm_value from cmpNorm n,lsService sss where sss.norm_id=n.norm_id          and n.company_id=(select first param_value from cmpParam cpr where cpr.company_id=:company_id and cpr.period_id=0 and param_id=204 and cpr.dbeg<=" + strPeriod + " and cpr.dend>=" + strPeriod + ")          and n.dbeg<=" + strLDPeriod + " and n.dend>=" + strPeriod + " and n.period_id=0 and sss.client_id=:client_id and sss.service_id=servcr          and sss.dbeg<=" + strLDPeriod + " and sss.dend>=" + strPeriod + " and sss.period_id=0 order by sss.dbeg,n.dbeg),0) as odnnorm,         (select first evidence                 from cntrDetailEvidence ev,cntrCounter cn,cntrRelation cr          where ev.counter_id=cn.counter_id and ev.period_id=" + strPeriodId + " and ev.month_id=" + strPeriodId + " and cn.counter_id=cr.counter_id and cn.service_id=r.service_id and cn.basecounter_id in (1,4)          and cr.client_id=:client_id and cr.period_id=0 and cr.dbeg<=" + strLDPeriod + " and cr.dend>=" + strPeriod + " ) as homecountervol, ";
        if (this.city != 28)
          str6 = str6 + "(if (" + this.city.ToString() + "<>1 or r.service_id not in (71,72,73,74,78)) and (r.service_id=crserv) then isnull((select n.norm_value from cmpNorm n where s.norm_id=n.norm_id and n.company_id=(select param_value from cmpParam where company_id=r.company_id and period_id=0 and param_id=204 and dbeg<=" + strPeriod + " and dend>=" + strPeriod + ")and n.dbeg<=" + strPeriod + " and n.dend>" + strPeriod + " and n.period_id=0),0) else 0 endif) as norm, ";
        if (this.city == 1 || this.city == 9 || (this.city == 10 || this.city == 11) || (this.city == 18 || this.city == 23 || (this.city == 24 || this.city == 25)) || (this.city == 26 || this.city == 29) || this.city == 30)
          str6 = str6 + "isnull((if r.service_id not in (20,71,72,73,74,78,95) or (r.service_id=20 and isnull((select group_num from cmpServiceParam where company_id=r.company_id and service_id=21 and complex_id=r.complex_id),0)=0)          or (r.service_id=95 and isnull((select group_num from cmpServiceParam where company_id=r.company_id and service_id=96 and complex_id=r.complex_id),0)=0)         then (if doc not in (2,5,6,7,8,12,14,15,17,18,21,25,28,29,31,33,34)  then (if r.service_id=3 then                    (select sum(cost) from cmpTariff where company_id=t.company_id and period_id=0 and tariff_id=s.tariff_id and dbeg=t.dbeg and dend=t.dend                    and service_id in (select service_id from " + this.uTmp + "printserv where client_id=:client_id and root=r.service_id) and scheme<>3) else                          (if t.scheme<>129 then t.cost else 0 endif)endif) else t.cost endif)         else 0 endif),0) as tax,         (if isnull(boilservice_id,0)<>0 or r.service_id<>crserv then isnull((select max(volume) from lsRent where period_id=" + strPeriodId + " and client_id=:client_id                  and service_id in (select service_id from dcService where root=r.service_id) and code=0 and rent_type=0),0)+                 isnull((select max(volume) from lsRent where period_id=" + strPeriodId + " and client_id=:client_id                  and service_id in (select service_id from dcService where root=r.service_id) and code=0 and rent_type=1),0) else 0 endif) as volume,         isnull((select sum(isnull(b.rent-b.rent_past,0)) from lsBalance b," + this.uTmp + "printserv prc1 where period_id=p.period_id and b.client_id=:client_id and b.client_id=prc1.client_id and prc1.root=r.service_id and b.service_id=prc1.service_id and b.supplier_id=prc1.supplier_id),0) as calc,0 as fullcalc,         isnull((select sum(isnull(b.rent_past,0)) from lsBalance b," + this.uTmp + "printserv prc1 where period_id=p.period_id and b.client_id=:client_id and b.client_id=prc1.client_id and prc1.root=r.service_id and b.service_id=prc1.service_id and b.supplier_id=prc1.supplier_id),0) as pastrent,         (if " + this.city.ToString() + "<> 29 then isnull((select sum(isnull(b.rent,0)) from lsRent b," + this.uTmp + "printserv prc1 where b.period_id=" + strPeriodId + "                 and b.client_id=:client_id and b.client_id=prc1.client_id and prc1.root=r.service_id and b.service_id=prc1.service_id and b.supplier_id=prc1.supplier_id and b.code=1),0) else          isnull((select sum(isnull(b.rent,0)) from lsRent b," + this.uTmp + "printserv prc1 where b.period_id=" + strPeriodId + "                 and b.client_id=:client_id and b.client_id=prc1.client_id and prc1.root=r.service_id and b.service_id=prc1.service_id and b.supplier_id=prc1.supplier_id and b.code=3),0) endif) as pastrent1 ";
        string str7 = " and (select first receipt_id from " + this.uTmp + "printserv where client_id=:client_id and (service_id=b.service_id or root=b.service_id) and supplier_id=b.supplier_id)=my.receipt_id";
        if (this.rbPeni1.Checked)
        {
          if (!this.cbMonth.Checked)
            str2 = " isnull((select sum(isnull(balance_out,0)) from lsBalancePeni where client_id=:client_id and period_id=" + strPeriodId + " and service_id=0),0) as penigenout,  isnull((select sum(isnull(payment,0)) from lsBalancePeni where client_id=:client_id and period_id=" + strPeriodId + " and service_id=0),0) as penigenpay,  isnull((select sum(isnull(rent,0)) from lsBalancePeni where client_id=:client_id and period_id=" + strPeriodId + " and service_id=0),0) as penigencalc,  isnull((select sum(isnull(balance_in,0)) from lsBalancePeni where client_id=:client_id and period_id=" + strPeriodId + " and service_id=0),0) as penigenin,  isnull((select sum(isnull(correct,0)) from lsBalancePeni where client_id=:client_id and period_id=" + strPeriodId + " and service_id=0),0) as penigencorr, isnull((select sum(isnull(balance_out,0)) from lsBalancePeni b where client_id=:client_id and period_id=" + strPeriodId + str7 + "),0) as peniout,isnull((select sum(isnull(balance_in,0)) from lsBalancePeni b where client_id=:client_id and period_id=" + strPeriodId + str7 + "),0) as peniin,isnull((select sum(isnull(payment,0)) from lsBalancePeni b where client_id=:client_id and period_id=" + strPeriodId + str7 + "),0) as penipay,isnull((select sum(isnull(rent,0)) from lsBalancePeni b where client_id=:client_id and period_id=" + strPeriodId + str7 + "),0) as penicalc,isnull((select sum(isnull(correct,0)) from lsBalancePeni b where client_id=:client_id and period_id=" + strPeriodId + str7 + "),0) as penicorr,";
          else
            str2 = " isnull((select sum(isnull(rent,0)) from lsBalancePeni b where client_id=:client_id and period_id=" + strPeriodId + " and service_id=0),0) as penigenout,0 as penigenin,  isnull((select sum(isnull(payment,0)) from lsBalancePeni b where client_id=:client_id and period_id=" + strPeriodId + " and service_id=0),0) as penigenpay,  isnull((select sum(isnull(rent,0)) from lsBalancePeni b where client_id=:client_id and period_id=" + strPeriodId + " and service_id=0),0) as penigencalc,  isnull((select sum(isnull(correct,0)) from lsBalancePeni b where client_id=:client_id and period_id=" + strPeriodId + " and service_id=0),0) as penigencorr, isnull((select sum(isnull(rent,0)) from lsBalancePeni b where client_id=:client_id and period_id=" + strPeriodId + str7 + "),0) as peniout,0 as peniin,isnull((select sum(isnull(payment,0)) from lsBalancePeni b where client_id=:client_id and period_id=" + strPeriodId + str7 + "),0) as penipay,isnull((select sum(isnull(rent,0)) from lsBalancePeni b where client_id=:client_id and period_id=" + strPeriodId + str7 + "),0) as penicalc,isnull((select sum(isnull(correct,0)) from lsBalancePeni b where client_id=:client_id and period_id=" + strPeriodId + str7 + "),0) as penicorr,";
        }
        if (this.rbPeni2.Checked)
          str2 = " 0.00 as penigenout,0.00 as penigenin,0.00 as penigenpay,0.00 as penigencalc,0.00 as penigencorr,0.00 as peniout,0.00 as peniin,0.00 as penicalc,0.00 as penicorr,0.00 as penipay, ";
        if (this.rbPeni3.Checked)
          str2 = " isnull((select sum(balance_out) from " + this.uTmp + "balance where client_id=:client_id and service_id=0),0) as penigenout,  isnull((select sum(balance_in) from " + this.uTmp + "balance where client_id=:client_id and service_id=0),0) as penigenin,  isnull((select sum(payment) from " + this.uTmp + "balance where client_id=:client_id and service_id=0),0) as penigenpay,  isnull((select sum(rent) from " + this.uTmp + "balance where client_id=:client_id and service_id=0),0) as penigencalc,  isnull((select sum(correct) from " + this.uTmp + "balance where client_id=:client_id and service_id=0),0) as penigencorr, isnull((select sum(isnull(balance_out,0)) from " + this.uTmp + "balance b where client_id=:client_id " + str7 + "),0) as peniout,isnull((select sum(isnull(balance_in,0)) from " + this.uTmp + "balance b where client_id=:client_id " + str7 + "),0) as peniin,isnull((select sum(isnull(payment,0)) from " + this.uTmp + "balance b where client_id=:client_id " + str7 + "),0) as penipay,isnull((select sum(isnull(rent,0)) from " + this.uTmp + "balance b where client_id=:client_id " + str7 + "),0) as penicalc,isnull((select sum(isnull(correct,0)) from " + this.uTmp + "balance b where client_id=:client_id " + str7 + "),0) as penicorr,";
        if (this.rbPeni4.Checked)
        {
          if (!this.cbMonth.Checked)
            str2 = " isnull((select sum(isnull(balance_out,0)+isnull(rent_full,0)) from lsBalancePeni where client_id=:client_id and period_id=" + strPeriodId + " and service_id=0),0) as penigenout,isnull((select sum(isnull(balance_in,0)) from lsBalancePeni where client_id=:client_id and period_id=" + strPeriodId + " and service_id=0),0) as penigenin, isnull((select sum(isnull(payment,0)) from lsBalancePeni where client_id=:client_id and period_id=" + strPeriodId + " and service_id=0),0) as penigenpay, isnull((select sum(isnull(rent,0)) from lsBalancePeni where client_id=:client_id and period_id=" + strPeriodId + " and service_id=0),0) as penigencalc, isnull((select sum(isnull(correct,0)) from lsBalancePeni where client_id=:client_id and period_id=" + strPeriodId + " and service_id=0),0) as penigencorr, isnull((select sum(isnull(balance_out,0)+isnull(rent_full,0)) from lsBalancePeni b where client_id=:client_id and period_id=" + strPeriodId + str7 + "),0) as peniout, isnull((select sum(isnull(balance_in,0)) from lsBalancePeni b where client_id=:client_id and period_id=" + strPeriodId + str7 + "),0) as peniin,isnull((select sum(isnull(payment,0)) from lsBalancePeni b where client_id=:client_id and period_id=" + strPeriodId + str7 + "),0) as penipay,isnull((select sum(isnull(rent,0)) from lsBalancePeni b where client_id=:client_id and period_id=" + strPeriodId + str7 + "),0) as penicalc,isnull((select sum(isnull(correct,0)) from lsBalancePeni b where client_id=:client_id and period_id=" + strPeriodId + str7 + "),0) as penicorr,";
          else
            str2 = " isnull((select sum(isnull(rent,0)) from lsBalancePeni where client_id=:client_id and period_id=" + strPeriodId + " and service_id=0),0) as penigenout,0 as penigenin, isnull((select sum(isnull(payment,0)) from lsBalancePeni where client_id=:client_id and period_id=" + strPeriodId + " and service_id=0),0) as penigenpay,  isnull((select sum(isnull(rent,0)) from lsBalancePeni where client_id=:client_id and period_id=" + strPeriodId + " and service_id=0),0) as penigencalc,  isnull((select sum(isnull(correct,0)) from lsBalancePeni where client_id=:client_id and period_id=" + strPeriodId + " and service_id=0),0) as penigencorr, isnull((select sum(isnull(rent,0)) from lsBalancePeni where client_id=:client_id and period_id=" + strPeriodId + "and service_id in (select root from " + this.uTmp + "printserv where client_id=:client_id and receipt_id=my.receipt_id)),0) as peniout, isnull((select sum(isnull(balance_in,0)) from lsBalancePeni where client_id=:client_id and period_id=" + strPeriodId + "and service_id in (select root from " + this.uTmp + "printserv where client_id=:client_id and receipt_id=my.receipt_id)),0) as peniin,isnull((select sum(isnull(payment,0)) from lsBalancePeni where client_id=:client_id and period_id=" + strPeriodId + "and service_id in (select root from " + this.uTmp + "printserv where client_id=:client_id and receipt_id=my.receipt_id)),0) as penipay,isnull((select sum(isnull(rent,0)) from lsBalancePeni where client_id=:client_id and period_id=" + strPeriodId + "and service_id in (select root from " + this.uTmp + "printserv where client_id=:client_id and receipt_id=my.receipt_id)),0) as penicalc,isnull((select sum(isnull(correct,0)) from lsBalancePeni where client_id=:client_id and period_id=" + strPeriodId + "and service_id in (select root from " + this.uTmp + "printserv where client_id=:client_id and receipt_id=my.receipt_id)),0) as penicorr,";
        }
        string str8 = this.cbOnly.Checked ? " left outer join dcService ss on r.service_id=ss.root left outer join lsBalance supl on supl.period_id=" + strPeriodId + " and supl.client_id=client_id and supl.service_id=ss.service_id and supl.service_id in (select service_id from dcService where root=r.service_id)" : " ";
        string str9 = str6 + "from dcPeriod p left outer join  cmpServiceParam r on p.period_id in (" + str5 + ")" + this.DateTaxUsl((short) 1, strPeriod) + str8 + " where r.company_id=:company_id and r.complex_id=100 ";
        if (this.city != 2 && !this.cbOnly.Checked)
          str9 += " and r.sendrent=0";
        TableDataSource dataSource2 = this.report1.GetDataSource("Itog") as TableDataSource;
        string[] strArray = new string[35];
        strArray[0] = "select my.receipt_id, sum(my.calc) as calc,sum(my.fullcalc) as fullcalc,sum(my.help) as help,sum(my.pastmsp) as pasthelp,sum(my.pastrent) as past,sum(my.pastrent1) as past1, calc-help+past as itog,sum(comphelp) as comphelp,       sum(my.sumcomp) as sumcomp, sum(my.pay)+isnull((if my.receipt_id=1 then (select sum(isnull(payment,0)) from lsBalance where period_id=";
        strArray[1] = strPeriodId;
        strArray[2] = " and client_id=:client_id and service_id=0) else 0 endif),0) as pay,       (if '";
        int index1 = 3;
        bool flag = this.cbMonth.Checked;
        string str10 = flag.ToString();
        strArray[index1] = str10;
        int index2 = 4;
        string str12 = "'='False' then sum(my.out)+       (if my.receipt_id=1 then isnull((select sum(isnull(balance_out,0)) from lsBalance where period_id=";
        strArray[index2] = str12;
        int index3 = 5;
        string str13 = strPeriodId;
        strArray[index3] = str13;
        int index4 = 6;
        string str14 = " and client_id=:client_id and service_id=0),0) else 0 endif) else itog+sumcomp endif) as out2,        (if '";
        strArray[index4] = str14;
        int index5 = 7;
        flag = this.cbUslBank.Checked;
        string str15 = flag.ToString();
        strArray[index5] = str15;
        int index6 = 8;
        string str16 = "'='False' then 0 else ";
        strArray[index6] = str16;
        int index7 = 9;
        string str17 = KvrplHelper.ChangeSeparator(this.txbProc.Text, ".");
        strArray[index7] = str17;
        int index8 = 10;
        string str18 = "*out2/100 endif) as bankproc,        (if '";
        strArray[index8] = str18;
        int index9 = 11;
        flag = this.cbMonth.Checked;
        string str19 = flag.ToString();
        strArray[index9] = str19;
        int index10 = 12;
        string str20 = "'='False' then sum(my.dolg)+isnull((if my.receipt_id=1 then (select sum(isnull(balance_in,0)) from lsBalance where period_id=";
        strArray[index10] = str20;
        int index11 = 13;
        string str21 = strPeriodId;
        strArray[index11] = str21;
        int index12 = 14;
        string str22 = " and client_id=:client_id and service_id=0) else 0 endif),0) else 0 endif) as dolg,       isnull((select sum(isnull(balance_in,0)) from lsBalance where period_id=";
        strArray[index12] = str22;
        int index13 = 15;
        string str23 = strPeriodId;
        strArray[index13] = str23;
        int index14 = 16;
        string str24 = " and client_id=:client_id and service_id in (select service_id from dcService where root=32)),0) as dolg32,       (if '";
        strArray[index14] = str24;
        int index15 = 17;
        flag = this.rbDept4.Checked;
        string str25 = flag.ToString();
        strArray[index15] = str25;
        int index16 = 18;
        string str26 = "'='True' then isnull((select sum(isnull(balance_out,0)+isnull(rent_full,0)) from lsBalancePeni where period_id=";
        strArray[index16] = str26;
        int index17 = 19;
        string str27 = strPeriodId;
        strArray[index17] = str27;
        int index18 = 20;
        string str28 = " and client_id=:client_id and service_id=32),0) else                 isnull((select sum(isnull(balance_out,0)) from lsBalancePeni where period_id=";
        strArray[index18] = str28;
        int index19 = 21;
        string str29 = strPeriodId;
        strArray[index19] = str29;
        int index20 = 22;
        string str30 = " and client_id=:client_id and service_id=32),0) endif) as peni32,       isnull((select sum(isnull(balance_in,0)) from lsBalance where period_id=";
        strArray[index20] = str30;
        int index21 = 23;
        string str31 = strPeriodId;
        strArray[index21] = str31;
        int index22 = 24;
        string str32 = "-2 and client_id=:client_id),0) as dolg22,       (select first receipt_name from dcReceipt where receipt_id=my.receipt_id) as name, ";
        strArray[index22] = str32;
        int index23 = 25;
        string str33 = str2;
        strArray[index23] = str33;
        int index24 = 26;
        string str34 = "       (select first ' р/с '||account||' в '||namebank from cmpreceipt cr left outer join di_bank b on cr.bank_id=b.idbank where company_id=my.company_id and cr.receipt_id=my.receipt_id) as schet,        (select first kor_sch from cmpreceipt cr, di_bank b where cr.bank_id=b.idbank and company_id=my.company_id and cr.receipt_id=my.receipt_id) as coraccount,        (select first bik from cmpreceipt cr, di_bank b where cr.bank_id=b.idbank and company_id=my.company_id and cr.receipt_id=my.receipt_id) as bik,        (select first namebank  from cmpreceipt cr, di_bank b where cr.bank_id=b.idbank and company_id=my.company_id and cr.receipt_id=my.receipt_id) as namebank,        (select first inn from cmpreceipt cr, base_org b where cr.supplier_id=b.idbaseorg and company_id=my.company_id and cr.receipt_id=my.receipt_id) as inn,        (select first kpp from cmpreceipt cr, base_org b where cr.supplier_id=b.idbaseorg and company_id=my.company_id and cr.receipt_id=my.receipt_id) as kpp,        (if ";
        strArray[index24] = str34;
        int index25 = 27;
        string str35 = this.city.ToString();
        strArray[index25] = str35;
        int index26 = 28;
        string str36 = "<> 28 then (select nameorg_min from cmpreceipt cr, base_org b where cr.supplier_id=b.idbaseorg and company_id=my.company_id and cr.receipt_id=my.receipt_id) else       isnull((select list(cast(' '||(select nameorg_min from base_org where idbaseorg=manager_id) as char(100))) from hmDogovor where idhome=:idhome and dbeg<=";
        strArray[index26] = str36;
        int index27 = 29;
        string str37 = strPeriod;
        strArray[index27] = str37;
        int index28 = 30;
        string str38 = " and dend>=";
        strArray[index28] = str38;
        int index29 = 31;
        string str39 = strPeriod;
        strArray[index29] = str39;
        int index30 = 32;
        string str40 = "),'')endif) as shiefregion,        (select account from cmpreceipt cr where company_id=my.company_id and cr.receipt_id=my.receipt_id) as account,sum(my.odncalc) as odncalc,sum(my.odncalcpast) as odnpast,        sum(my.odncalc)-sum(my.odnhelp)+sum(my.odncalcpast) as odnitog,sum(odncomphelp) as odncomphelp from (";
        strArray[index30] = str40;
        int index31 = 33;
        string str41 = str9;
        strArray[index31] = str41;
        int index32 = 34;
        string str42 = ") as my,cmpServiceParam sp where sp.service_id=my.service_id and sp.company_id=:company_id and sp.complex_id=100 and my.receipt_id<>0 group by my.receipt_id,penigenout,penigenpay,penigencalc,my.company_id order by my.receipt_id";
        strArray[index32] = str42;
        string str43 = string.Concat(strArray);
        dataSource2.SelectCommand = str43;
        string str44 = "select my.client_id,sp.service_id,sp.printshow as sname,sp.sorter,sp.group_num,my.receipt_id,(if sp.crossservice_id = 1 then sum(my.tax) else 0 endif) as tax,      (if sp.crossservice_id = 1 then list(distinct my.edizm) else '' endif) as edizm, (if sp.crossservice_id = 1 then sum(my.norm) else 0 endif) as quota,     (if sp.crossservice_id = 1 then sum(my.volume) else 0 endif) as volume,(if sp.crossservice_id = 1 then sum(out) else 0 endif) as out,(if sp.crossservice_id = 1 then sum(my.calc) else 0 endif) as calc,     (if sp.crossservice_id = 1 then sum(my.fullcalc) else 0 endif) as fullcalc,(if sp.crossservice_id = 1 then sum(my.help) else 0 endif) as help,(if sp.crossservice_id = 1 then sum(my.pastmsp) else 0 endif) as pasthelp,     (if sp.crossservice_id = 1 then sum(my.pastrent) else 0 endif) as past, (if sp.crossservice_id = 1 then sum(my.pastrent1) else 0 endif) as past1, calc-help+past as itog,     (if sp.crossservice_id = 1 then sum(comphelp) else 0 endif) as comphelp,(if sp.crossservice_id = 1 then sum(coeff) else 0 endif) as coeff,(if sp.crossservice_id = 1 then sum(dolg) else 0 endif) as dolg,     (if sp.crossservice_id = 1 then sum(pay) else 0 endif) as pay, (if " + this.city.ToString() + "<>23 then my.period_value else " + strPeriod + " endif) as period_value " + str3 + ",(if sp.crossservice_id = 1 then sum(odnvolume) else 0 endif) as odnvolume,     (if sp.crossservice_id = 1 then sum(odncalc) else 0 endif) as odncalc,(if sp.crossservice_id = 1 then sum(odnhelp) else 0 endif) as odnhelp,(if sp.crossservice_id = 1 then sum(odncalcpast) else 0 endif) as odnpast,     odncalc-odnhelp+odnpast as odnitog,(if sp.crossservice_id = 1 then sum(my.odncomphelp) else 0 endif) as odncomphelp,     (if sp.crossservice_id = 1 then sum(my.sumcomp) else 0 endif) as sumcomp,sp.boilservice_id,(if sp.crossservice_id = 1 then list(distinct my.typevol) else '' endif) as typevol,      (if sp.crossservice_id = 1 then list(distinct my.typeodnvol) else '' endif) as typeodnvol,sp.crossservice_id " + ",(select first e.evidence_current from cntrRelation r,cntrCounter cnt left outer join cntrEvidence e on cnt.counter_id=e.counter_id and         e.dbeg=(select max(dbeg) from cntrEvidence where period_id=" + strPeriodId + " and counter_id=cnt.counter_id and (evidence_past<>0 or evidence_current<>0))         and e.period_id=" + strPeriodId + " where r.client_id=:client_id and         r.dbeg<=" + strLDPeriod + " and r.dend>=" + strPeriod + "        and r.period_id=0 and r.counter_id=cnt.counter_id and cnt.service_id=sp.service_id and isnull(cnt.archives_date," + strLDPeriod + ")>" + strPeriod + " and cnt.basecounter_id in (1,4) and onoff=1) as evidence_home, (select first e.evidence_past from cntrRelation r,cntrCounter cnt left outer join cntrEvidence e on cnt.counter_id=e.counter_id and         e.dbeg=(select max(dbeg) from cntrEvidence where period_id=" + strPeriodId + " and counter_id=cnt.counter_id and (evidence_past<>0 or evidence_current<>0))         and e.period_id=" + strPeriodId + " where r.client_id=:client_id and         r.dbeg<=" + strLDPeriod + " and r.dend>=" + strPeriod + "        and r.period_id=0 and r.counter_id=cnt.counter_id and cnt.service_id=sp.service_id and isnull(cnt.archives_date," + strLDPeriod + ")>" + strPeriod + " and cnt.basecounter_id=1 and onoff=1) as evidence_home_past,        (if boilservice_id=1 then isnull((select first homevol from " + this.uTmp + "homevol where company_id=:company_id and idhome=(select idhome from lsClient where client_id=:client_id) and service_id=sp.service_id and homevol<>0),0) else 0 endif) as homevol,        isnull((select odnhomevol from " + this.uTmp + "homevol where company_id=:company_id and idhome=:idhome and service_id=sp.service_id),0) as odnhomevol,         (if sp.crossservice_id = 1 then sum(my.odnnorm) else 0 endif) as odnnorm,        sum(homecountervol) as homecountervol,list(distinct my.basetariffmsp_id) as basetariffmsp_id, '' as printrass ";
        string str45 = str44 + "from (" + str9 + ") as my,cmpServiceParam sp where sp.company_id=:company_id  and (sp.service_id not in (select service_id from cmpCrossService ccs where company_id=:company_id and complex_id=100 and ccs.dbeg<=" + strPeriod + " and ccs.dend>=" + strPeriod + " and crosstype_id=6) or (" + this.city.ToString() + " not in (2,7,16,23,24,28) and (:manager_id <>2474) ))     and (my.service_id=sp.service_id or sp.service_id=my.gr) and sp.complex_id=100 and my.receipt_id<>0 and (group_num=0 or sp.calcalone=1) group by my.client_id,sp.service_id,nameorg,sp.printshow,sp.group_num,sp.sorter,my.receipt_id,period_value,sp.crossservice_id,sp.boilservice_id ";
        if (this.city != 14 && (!this.cbQuarter.Checked || this.city == 23))
          str45 = this.city == 2 || this.city == 7 || (this.city == 16 || this.city == 23) || this.city == 24 || this.city == 28 ? str45 + " having calc<>0 or itog<>0 or odncalc<>0 or odnitog<>0 or (group_num=0 and crossservice_id=0)" : str45 + " having calc<>0 or itog<>0 or (group_num=0 and crossservice_id=0)";
        int num;
        if (this.city != 28)
        {
          if (this.city != 5);
          num = 1;
        }
        else
          num = this.city != 2 ? 0 : (!this.cbQuarter.Checked ? 1 : 0);
        if (num != 0)
          str45 = str45 + " or (select count(*) from cntrCounter cntr where cntr.client_id=:client_id and cntr.complex_id=100 and isnull(cntr.archives_date," + strLDPeriod + ")>" + strPeriod + " and cntr.Service_id=sp.service_id)>0 or (select count(*) from cntrRelation cr,cntrCounter cntr where cr.client_id=:client_id and cr.dbeg<=" + strLDPeriod + " and cr.dend>=" + strPeriod + " and cr.period_id=0 and cr.counter_id=cntr.counter_id and isnull(cntr.archives_date," + strLDPeriod + ")>" + strPeriod + " and cntr.basecounter_id=3 and onoff=1 and cntr.Service_id=sp.service_id)>0 or evidence_home<>0 or evidence_home_past<>0";
        if (this.cbUslBank.Checked)
          str45 += " union all select :client_id as client_id,0 as service_id,'Услуги банка, почты' as sname,100 as sorter,0 as group_num,1 as receipt_id, 0 as tax,'' as edizm, 0 as quota,0 as volume,0 as out,0 as calc,0 as fullcalc,0 as help,0 as pasthelp,0 as past,0 as past1,:bankproc as itog,0 as comphelp,0 as coeff,0 as dolg,0 as pay,'2000-01-01','' as nameorg ,0 as odnvolume,0 as odncalc,0 as odnhelp,0 as odnpast,0 as odnitog,0 as odncomphelp,0 as sumcomp,0 as boilservice_id,'' typevol,'' as typeodnvol,0 as crossservice_id,0 as evidence_home,null as evidence_home_past,0 as homevol,0 as odnhomevol,0 as counterhomevol,0 as odnnorm,'' as basetariffmsp_id,0 as printrass ";
        (this.report1.GetDataSource("Tarif") as TableDataSource).SelectCommand = str45 + " order by 6,4,23";
        if (this.rbCounter1.Checked)
          str44 = "";
        if (this.rbCounter2.Checked)
          str44 = " and period_id<=" + strPeriodId;
        if (this.rbCounter3.Checked)
          str44 = " and period_id=" + strPeriodId;
        string str46 = " and (sp.service_id in (select root from " + this.uTmp + "printserv r where r.client_id=:client_id " + strReceipt + " ) or  sp.service_id in (select crossservice_id from cmpCrossService where company_id=:company_id and " + strPeriod + " between dbeg and dend and crosstype_id=1  and service_id in (select root from " + this.uTmp + "printserv r where r.client_id=:client_id" + strReceipt + " and r.supplier_id=0)))";
        string str47;
        if (!this.cbOnly.Checked)
          str47 = "";
        else
          str47 = "(select first '<b>'||supplier_client||'</b> '||nameorg_min from base_org bo left outer join cmpSupplierClient sc on bo.idbaseorg=sc.supplier_id and sc.company_id=:company_id and sc.client_id=client_id and sc.client_id=:client_id           where bo.idbaseorg in (select supplier_id from lsSupplier where period_id=0 and client_id=:client_id and service_id in (select service_id from dcService where root=sp.service_id) and dbeg<=" + strLDPeriod + " and dend>=" + strLDPeriod + ")) as nameorg,";
        string str48 = "select sp.client_id, sp.service_id,cast(regulatefld(sp.counter_num) as char(20)) as counter_num,        (if e.evidence_current is null and e.evidence_past is null and not exists(select * from cntrEvidence where counter_id=sp.counter_id) then sp.evidence_start else e.evidence_current endif) as evidence_current,       (if e.evidence_current is null and e.evidence_past is null and not exists(select * from cntrEvidence where counter_id=sp.counter_id) then sp.evidence_start else e.evidence_past endif) as evidence_past, e.dBeg, e.dEnd,         (select service_name from dcService where service_id=sp.service_id) as name, " + str47 + "        isnull((if (evidence_current-evidence_past > 0 or evidence_past-evidence_current<=5000) then evidence_current-evidence_past else power(10,length(evidence_past)-5)+evidence_current-evidence_past endif),0) as volume,          sp.audit_date,days(sp.audit_date,30) auditend  from cntrCounter sp left outer join cntrEvidence e on sp.counter_id=e.counter_id and e.dbeg=(select max(dbeg) from cntrEvidence where counter_id=sp.counter_id and (evidence_past<>0 or evidence_current<>0)" + str44 + ")                 and e.period_id=(select max(period_id) from cntrEvidence where counter_id=sp.counter_id and dbeg=e.dbeg " + str44 + "),cmpServiceParam r where sp.client_id=:client_id and sp.complex_id=100 and isnull(sp.archives_date," + strLDPeriod + ")>" + strPeriod + " and r.company_id=:company_id and r.complex_id=100 and r.Service_id=sp.service_id" + str46;
        if (!this.cbOnly.Checked)
          str48 += " and isnull((select sendrent from cmpServiceParam where company_id=sp.company_id and complex_id=100 and service_id=sp.service_id),0)=0 ";
        string str49 = str48 + " union all select :client_id as client_id,sp.service_id,cast(regulatefld(sp.counter_num) as char(20)) as counter_num,(if e.evidence_current is null and e.evidence_past is null then sp.evidence_start else e.evidence_current endif) as evidence_current,         (if e.evidence_current is null and e.evidence_past is null then sp.evidence_start else e.evidence_past endif) as evidence_past,e.dbeg,e.dend,         (select service_name from dcService where service_id=sp.service_id) as name, " + str47 + "         isnull((if (evidence_current-evidence_past > 0 or evidence_past-evidence_current<=5000) then evidence_current-evidence_past else power(10,length(evidence_past)-5)+evidence_current-evidence_past endif),0) as volume,          sp.audit_date,days(sp.audit_date,30) auditend          from cntrRelation cr,cntrCounter sp left outer join cntrEvidence e on sp.counter_id=e.counter_id and                  e.dbeg=(select max(dbeg) from cntrEvidence where counter_id=sp.counter_id and (evidence_past<>0 or evidence_current<>0)" + str44 + ")                  and e.period_id=(select max(period_id) from cntrEvidence where counter_id=sp.counter_id and dbeg=e.dbeg " + str44 + ")         ,cmpServiceParam r where cr.client_id=:client_id and cr.dbeg<=" + strLDPeriod + " and cr.dend>=" + strPeriod + "                 and cr.period_id=0 and cr.counter_id=sp.counter_id and isnull(sp.archives_date," + strLDPeriod + ")>" + strPeriod + " and sp.basecounter_id=3 and onoff=1 and r.company_id=:company_id and r.complex_id=100 and r.Service_id=sp.service_id" + str46;
        if (!this.cbOnly.Checked)
          str49 += " and isnull((select sendrent from cmpServiceParam where company_id=sp.company_id and complex_id=100 and service_id=sp.service_id),0)=0 ";
        for (int index33 = 1; index33 <= 3; ++index33)
          str49 += " union all select :client_id as client_id,5 as service_id,'99999999999999999999' as counter_num,0 as evidence_current,0 as evidence_past,null as dBeg,null as DEnd,'' as name, null as volume,null as audit_date,null as auditend ";
        for (int index33 = 1; index33 <= 3; ++index33)
          str49 += " union all select :client_id as client_id,10 as service_id,'99999999999999999999' as counter_num,0 as evidence_current,0 as evidence_past,null as dBeg,null as DEnd,'' as name, null as volume,null as audit_date,null as auditend ";
        (this.report1.GetDataSource("Counters") as TableDataSource).SelectCommand = str49 + " union all select :client_id as client_id,17 as service_id,'99999999999999999999' as counter_num,0 as evidence_current,0 as evidence_past,null as dBeg,null as DEnd,'' as name, null as volume,null as audit_date,null as auditend " + " order by 1,2,3";
        string str50 = "select sum(pay) as socpay,(if " + this.showPersData.ToString() + "=1 then (select family||' '||substr(name,1,1)||'. '||substr(lastname,1,1)||'. ' from form_a where idform=idpers) else cast(idpers as char(20)) endif) as socfio, (select first receipt_id from " + this.uTmp + "printserv where client_id=:client_id and root=numserv and supplier_id=0 order by 1) as receipt_id from soc_saldo where period=" + strPeriod + " and idlic=:client_id and numserv in (select root from " + this.uTmp + "printserv r where r.client_id=:client_id" + strReceipt + "  and supplier_id=0) and pay<>0 group by receipt_id,idpers ";
        for (int receipt = this.receipts[0]; receipt <= this.receipts[0] + 5; ++receipt)
        {
          for (int index33 = 1; index33 <= 6; ++index33)
            str50 = str50 + " union all select 0 as socpay, '' as socfio," + receipt.ToString() + " as receipt_id";
        }
        (this.report1.GetDataSource("MSP") as TableDataSource).SelectCommand = str50 + " order by 3,1 desc";
        (this.report1.GetDataSource("DetailEvidence") as TableDataSource).SelectCommand = "select min(dbeg) as start1,(if start1<" + strPeriod + " then (if max(dend)>=" + strPeriod + " then days(" + strPeriod + ",-1) else max(dend) endif) else start1 endif) as finish1,max(dend) as finish2,        if finish2>=" + strPeriod + " then (if min(dbeg)<" + strPeriod + " then " + strPeriod + " else min(dbeg) endif)  else finish2 endif as start2,(if evidence1<>0 then finish1-start1+1 else 0 endif) as kol1,        if evidence2<>0 then finish2-start2+1 else 0 endif as kol2,s.service_id as serv,        isnull((select sum(evidence_current-evidence_past) from cntrEvidence where period_id=" + strPeriodId + " and counter_id in (select counter_id from cntrCounter where client_id=:client_id and service_id=serv)),0) as evidence,        isnull((select sum(evidence) from cntrDetailEvidence where period_id=" + strPeriodId + " and counter_id in (select counter_id from cntrCounter where client_id=:client_id and service_id=serv) and month_id=" + strPeriodId + " and evidence_type=0),0) as evidence2,evidence-evidence2 as evidence1,         isnull((select sum(evidence) from cntrDetailEvidence where period_id=" + strPeriodId + " and counter_id in (select counter_id from cntrCounter where client_id=:client_id and service_id=serv) and month_id=" + strPeriodId + " and evidence_type=1),0) as verification,          isnull((select (if (min(dbeg)<=" + strLDPeriod + " and max(dend)>=" + strPeriod + ") then (if max(dend)<" + strLDPeriod + " then max(dend) else " + strLDPeriod + " endif) - (if min(dbeg)>" + strPeriod + " then min(dbeg) else " + strPeriod + " endif)+1 else null endif) from cntrAudit where counter_id in (select counter_id from cntrCounter where client_id=:client_id and service_id=serv)),0) as kol3,         (if start1>months(" + strPeriod + ",-1) then (select first coeff from cntrDetail where period_id in (" + strPeriodId + "-1," + strPeriodId + ") and month_id=" + strPeriodId + "-1 and counter_id in (select counter_id from cntrRelation where client_id=:client_id         and period_id=0 and dbeg<=months(" + strPeriod + ",-1) and dend>months(" + strPeriod + ",-1)) and service_id=serv order by period_id desc) else null endif) as coef1,        (select first coeff from cntrDetail where period_id=" + strPeriodId + " and month_id=" + strPeriodId + " and counter_id in (select counter_id from cntrRelation where client_id=:client_id and period_id=0 and dbeg<=" + strLDPeriod + " and dend>" + strPeriod + ") and service_id=serv) as coef2 from dcService s left outer join cntrEvidence e on s.service_id=(select service_id from cntrCounter where counter_id=e.counter_id) and e.period_id=" + strPeriodId + " and e.counter_id in (select counter_id from cntrCounter where client_id=:client_id and isnull(archives_date,'2999-12-31')>=" + strPeriod + "),        cmpServiceParam r where s.service_id in (select service_id from cntrCounter where client_id=:client_id and isnull(archives_date,'2999-12-31')>=" + strPeriod + ") and r.company_id=:company_id and r.complex_id=100 and r.service_id=s.service_id         and (serv in (select root from " + this.uTmp + "printserv r where r.client_id=:client_id " + strReceipt + " and r.supplier_id=0) or         serv in (select crossservice_id from cmpCrossService where company_id=:company_id and " + strPeriod + " between dbeg and dend and crosstype_id=1         and service_id in (select root from " + this.uTmp + "printserv r where r.client_id=:client_id" + strReceipt + ")))group by serv";
        this.report1.Show();
      }
      catch
      {
      }
    }

    private string NextTarif(FastReport.Data.TableDataSource tarif)
    {
      tarif.Next();
      return "";
    }

    private string NS_lsOwners(int typeId, int code, string period, string periodv, string client, string fdat)
    {
      string str1 = "";
      string str2 = "";
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
      string str3 = " and firstpropdate<=" + fdat + " and regdate<=" + str1 + " and (archive in (0,5) or (archive in (1,2) and isnull(outtodate,'2999-12-31')>" + fdat + ") or (archive in (1,2) and regoutdate>" + str1 + "))";
      if (code == 1)
        str2 = this.NS_frPers(4, "ooo.owner") + "||' '||" + this.NS_frPers(5, "ooo.owner") + "||' '||" + this.NS_frPers(6, "ooo.owner");
      if (code == 2)
        str2 = "trim(" + this.NS_frPers(4, "ooo.owner") + "||' '||substr(" + this.NS_frPers(5, "ooo.owner") + ",1,1)||(if " + this.NS_frPers(5, "ooo.owner") + "<>'' then '.' else '' endif)||substr(" + this.NS_frPers(6, "ooo.owner") + ",1,1)||(if " + this.NS_frPers(6, "ooo.owner") + "<> '' then '.' else '' endif))";
      return "(if " + this.NS_lsParam_value(typeId, 104, period, client, fdat) + " in (2,5,6,7,8,12,14,15,17,18,21,25,28,29,31,33,34) then     isnull((select list(" + str2 + ") from owners ooo where idlic=" + client + str3 + ")," + KvrplHelper.NS_lsOwner(typeId, code, period, periodv, client, fdat) + ") else " + KvrplHelper.NS_lsOwner(typeId, code, period, periodv, client, fdat) + " endif)";
    }

    private string NS_frPers(int id, string sid)
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
      switch (this.typeFio)
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

    private string NS_lsParam_value(int typeId, int paramId, string period, string client, string fdat)
    {
      string[] strArray = new string[9];
      strArray[0] = "isnull((select first ms_p.param_value         from lsParam ms_p         where ms_p.period_id=";
      int index1 = 1;
      string str1 = this.NS_Period(typeId, "(select max(ms_p2.period_id) from lsParam ms_p2 where ms_p2.period_id %zn% " + period + " and ms_p2.client_id=ms_p.client_id and " + fdat + " between ms_p2.dbeg and ms_p2.dend and ms_p2.param_id=" + paramId.ToString() + ")");
      strArray[index1] = str1;
      int index2 = 2;
      string str2 = " and               ms_p.client_id=";
      strArray[index2] = str2;
      int index3 = 3;
      string str3 = client;
      strArray[index3] = str3;
      int index4 = 4;
      string str4 = " and ";
      strArray[index4] = str4;
      int index5 = 5;
      string str5 = fdat;
      strArray[index5] = str5;
      int index6 = 6;
      string str6 = " between ms_p.dbeg and ms_p.dend and ms_p.param_id=";
      strArray[index6] = str6;
      int index7 = 7;
      string str7 = paramId.ToString();
      strArray[index7] = str7;
      int index8 = 8;
      string str8 = "),0)";
      strArray[index8] = str8;
      return string.Concat(strArray);
    }

    private string NS_Period(int typeId, string txt)
    {
      switch (typeId)
      {
        case 1:
          txt = "0";
          break;
        case 2:
          txt = txt.Replace("%zn%", "<");
          break;
        case 3:
          txt = txt.Replace("%zn%", "<=");
          break;
      }
      return txt;
    }

    private string NS_lsPerson_cnt(int typeId, int propId, string period, string periodv, string client, string fdat)
    {
      string str = "";
      if (typeId == 1 || typeId == 3)
        str = "ymd(year(" + periodv + "),month(" + periodv + ")+1,1)-1";
      if (typeId == 2)
        str = "(if " + fdat + "<=" + periodv + "-1 then " + periodv + "-1 else ymd(year(" + fdat + "),month(" + fdat + ")+1,1)-1 endif)";
      return "(if " + this.NS_cmpParam_value(typeId, 202, period, client, fdat) + "=0 then   " + this.NS_lsParam_value(typeId, 100 + propId, period, client, fdat) + " else   (select count(ms_p.idform) from form_a ms_p    where ms_p.idlic=" + client + " and ms_p.typepropis=" + propId.ToString() + " and ms_p.firstpropdate<=" + fdat + " and ms_p.regdate<=" + str + " and          (ms_p.archive in (0,5) or          (ms_p.archive in (1,2) and isnull(ms_p.outtodate,'2999-12-31')>" + fdat + " and isnull(ms_p.diedate,'2999-12-31')>" + fdat + ") or          (ms_p.archive in (1,2) and ms_p.regoutdate>" + str + "))) endif)";
    }

    private string NS_cmpParam_value(int typeId, int paramId, string period, string client, string fdat)
    {
      string[] strArray = new string[9];
      strArray[0] = "isnull((select first ms_p.param_value         from cmpParam ms_p,lsClient ms_l         where ms_p.company_id=ms_l.company_id and               ms_p.period_id=";
      int index1 = 1;
      string str1 = this.NS_Period(typeId, "(select max(ms_p2.period_id) from cmpParam ms_p2 where ms_p2.period_id %zn% " + period + " and ms_p2.company_id=ms_p.company_id and ms_p2.param_id=ms_p.param_id and " + fdat + " between ms_p2.dbeg and ms_p2.dend)");
      strArray[index1] = str1;
      int index2 = 2;
      string str2 = " and               ms_p.param_id=";
      strArray[index2] = str2;
      int index3 = 3;
      string str3 = paramId.ToString();
      strArray[index3] = str3;
      int index4 = 4;
      string str4 = " and ms_l.client_id=";
      strArray[index4] = str4;
      int index5 = 5;
      string str5 = client;
      strArray[index5] = str5;
      int index6 = 6;
      string str6 = " and ";
      strArray[index6] = str6;
      int index7 = 7;
      string str7 = fdat;
      strArray[index7] = str7;
      int index8 = 8;
      string str8 = " between ms_p.dbeg and ms_p.dend),0)";
      strArray[index8] = str8;
      return string.Concat(strArray);
    }

    private string Peni()
    {
      string str = "";
      string strPeniServ = this.peniBalance != 2 ? " and service_id=r.service_id" : " and service_id in (select service_id from dcservice where root=r.service_id)";
      if (this.rbPeni1.Checked || this.rbDept4.Checked)
      {
        if (!this.cbMonth.Checked)
        {
          string strField = "balance_out";
          if (this.rbDept4.Checked)
            strField = "balance_out+rent_full";
          str = str + this.PeniDetail(strPeniServ, strField) + " as peniout, " + this.PeniDetail(strPeniServ, "balance_in") + " as peniin," + this.PeniDetail(strPeniServ, "payment") + " as penipay," + this.PeniDetail(strPeniServ, "rent") + " as penicalc," + this.PeniDetail(strPeniServ, "correct") + " as penicorr ";
          this.strPeni = this.PeniDetail(" and service_id=0", strField) + " as penigenout, " + this.PeniDetail(" and service_id=0", "payment") + " as penigenpay, " + this.PeniDetail(" and service_id=0", "rent") + " as penigencalc, " + this.PeniDetail(" and service_id=0", "balance_in") + " as penigenin, " + this.PeniDetail(" and service_id=0", "correct") + " as penigencorr, ";
        }
        else
        {
          str = str + this.PeniDetail(strPeniServ, "rent") + " as peniout, 0 as peniin," + this.PeniDetail(strPeniServ, "payment") + " as penipay," + this.PeniDetail(strPeniServ, "rent") + " as penicalc," + this.PeniDetail(strPeniServ, "correct") + " as penicorr ";
          this.strPeni = this.PeniDetail(" and service_id=0", "rent") + " as penigenout,0 as penigenin, " + this.PeniDetail(" and service_id=0", "payment") + " as penigenpay, " + this.PeniDetail(" and service_id=0", "rent") + " as penigencalc,  " + this.PeniDetail(" and service_id=0", "correct") + " as penigencorr, ";
        }
      }
      if (this.rbPeni2.Checked)
      {
        str += "0 as peniout,0 as peniin,0 as penicalc,0 as penicorr,0 as penipay ";
        this.strPeni = " 0 as penigenout,0 as penigenin,0 as penigenpay,0 as penigencalc,0 as penigencorr,";
      }
      if (this.rbPeni3.Checked)
      {
        str = str + "isnull((select sum(isnull(balance_out,0))+sum(isnull(rent_full,0)) from " + this.uTmp + "balance where client_id=:client  " + strPeniServ + "),0) as peniout,               isnull((select sum(isnull(balance_in,0)) from " + this.uTmp + "balance where client_id=:client  " + strPeniServ + "),0) as peniin,               isnull((select sum(isnull(payment,0)) from " + this.uTmp + "balance where client_id=:client  " + strPeniServ + "),0) as penipay,               isnull((select sum(isnull(rent,0)) from " + this.uTmp + "balance where client_id=:client  ' + StrPeniServ + '),0) as penicalc,               isnull((select sum(isnull(correct,0)) from " + this.uTmp + "balance where client_id=:client  ' + StrPeniServ + '),0) as penicorr ";
        this.strPeni = " isnull((select sum(balance_out) from " + this.uTmp + "balance where client_id=:client and service_id=0),0) as penigenout,  isnull((select sum(balance_in) from " + this.uTmp + "balance where client_id=:client and service_id=0),0) as penigenin,  isnull((select sum(payment) from " + this.uTmp + "balance where client_id=:client and service_id=0),0) as penigenpay,  isnull((select sum(rent) from " + this.uTmp + "balance where client_id=:client and service_id=0),0) as penigencalc,  isnull((select sum(correct) from " + this.uTmp + "balance where client_id=:client and service_id=0),0) as penigencorr, ";
      }
      return str;
    }

    private string PeniDetail(string strPeniServ, string strField)
    {
      return "isnull((select sum(isnull(" + strField + ",0)) from lsBalancePeni where client_id=:client and period_id=" + Options.Period.PeriodId.ToString() + strPeniServ + "),0) ";
    }

    private string DateTax(short order, string strPeriod, string strLDPeriod)
    {
      string str = (int) order != 1 ? " order by dbeg desc" : " order by dbeg";
      return "          left outer join lsService s on s.client_id=:client  and s.period_id=0 and r.service_id=s.service_id           and s.dbeg<=isnull((select first dbeg from lsService ss where ss.client_id=:client  and ss.period_id=0 and r.service_id=ss.service_id                 and ((dbeg<=" + strPeriod + " and dend>=" + strPeriod + ") or (dbeg<=" + strLDPeriod + " and dend>=" + strPeriod + ")) and ss.complex_id=100 " + str + ")," + strPeriod + ")          and s.dend>=isnull((select first dbeg from lsService ss where ss.client_id=:client and ss.period_id=0 and r.service_id=ss.service_id                 and ((dbeg<=" + strPeriod + " and dend>=" + strPeriod + ") or (dbeg<=" + strLDPeriod + " and dend>=" + strLDPeriod + ")) and ss.complex_id=100 " + str + ")," + strPeriod + ")          and s.complex_id=100           left outer join cmpTariff t on s.service_id=t.service_id and s.complex_id=t.complex_id and t.company_id=(select param_value from cmpParam where company_id=r.company_id and period_id=0 and param_id=201 and dbeg<=" + strPeriod + " and dend>=" + strPeriod + ")          and t.period_id=0 and s.tariff_id=t.tariff_id           and t.dbeg<=isnull((select first dbeg from cmpTariff tt where s.tariff_id=tt.tariff_id and s.service_id=tt.service_id and tt.company_id=t.company_id and tt.complex_id=100 and tt.period_id=0                 and ((dbeg<=" + strPeriod + " and dend>=" + strPeriod + ") or (dbeg<=" + strLDPeriod + " and dend>=" + strPeriod + "))" + str + ")," + strPeriod + ")          and t.dend>=isnull((select first dbeg from cmpTariff tt where s.tariff_id=tt.tariff_id and s.service_id=tt.service_id and tt.company_id=t.company_id and tt.complex_id=100 and tt.period_id=0                 and ((dbeg<=" + strPeriod + " and dend>=" + strPeriod + ") or (dbeg<=" + strLDPeriod + " and dend>=" + strLDPeriod + "))" + str + ")," + strPeriod + ")";
    }

    private void report1_LoadBaseReport(object sender, FastReport.CustomLoadEventArgs e)
    {
    }

    private void fdFromFile_FileOk(object sender, CancelEventArgs e)
    {
    }

    private string DateTaxUsl(short order, string strPeriod)
    {
      string str = "";
      switch (order)
      {
        case 1:
          str = " order by dbeg";
          break;
        case 2:
          str = " order by dbeg desc";
          break;
      }
      return "          left outer join lsService s on s.client_id=:client_id and s.period_id=0 and r.service_id=s.service_id          and s.dbeg<=isnull((select first dbeg from lsService ss where ss.client_id=:client_id and ss.period_id=0 and r.service_id=ss.service_id                 and ((dbeg<=p.period_value and dend>=p.period_value) or (dbeg<months(p.period_value,1) and dend>months(p.period_value,1))) and ss.complex_id=100 " + str + "),p.period_value)          and s.dend>=isnull((select first dbeg from lsService ss where ss.client_id=:client_id and ss.period_id=0 and r.service_id=ss.service_id                 and ((dbeg<=p.period_value and dend>=p.period_value) or (dbeg<months(p.period_value,1) and dend>months(p.period_value,1))) and ss.complex_id=100 " + str + "),p.period_value)          and s.complex_id=100           left outer join cmpTariff t on s.complex_id=t.complex_id and t.company_id=(select param_value from cmpParam where company_id=:company_id and period_id=0 and param_id=201 and dbeg<=" + strPeriod + " and dend>=" + strPeriod + ")          and t.period_id=0 and s.tariff_id=t.tariff_id and t.service_id=r.service_id           and t.dbeg<=isnull((select first dbeg from cmpTariff tt where s.tariff_id=tt.tariff_id and s.service_id=tt.service_id and tt.company_id=t.company_id and tt.complex_id=100 and tt.period_id=0                 and ((dbeg<=p.period_value and dend>=p.period_value) or (dbeg<months(p.period_value,1) and dend>months(p.period_value,1)))" + str + "),p.period_value)          and t.dend>=isnull((select first dbeg from cmpTariff tt where s.tariff_id=tt.tariff_id and s.service_id=tt.service_id and tt.company_id=t.company_id and tt.complex_id=100 and tt.period_id=0                 and ((dbeg<=p.period_value and dend>=p.period_value) or (dbeg<months(p.period_value,1) and dend>months(p.period_value,1)))" + str + "),p.period_value)";
    }

    private void LoadHomevol(string strPeriodId, string strPeriod)
    {
      this.session.CreateSQLQuery("call my_login").ExecuteUpdate();
      this.session.CreateSQLQuery("call droptmp(1,'tmpReceipthomevol',current user);create " + this.MP_Global1 + " table " + this.uTmp + "homevol(company_id integer,idhome integer,service_id integer,homevol numeric(15,5),odnhomevol numeric(15,5),hvnorm numeric(15,5),hvcntr numeric(15,5),ohvnorm numeric(15,5),ohvcntr numeric(15,5))" + this.MP_Global2 + ";create index ch on " + this.uTmp + "homevol (company_id,idhome);create index chs on " + this.uTmp + "homevol (company_id,idhome,service_id);").ExecuteUpdate();
      this.session.CreateSQLQuery("call droptmp(1,'tmpReceipthomeserv',current user);create " + this.MP_Global1 + " table " + this.uTmp + "homeserv(idhome integer,root smallint,service_id smallint,supplier_id integer) " + this.MP_Global2 + "; insert into " + this.uTmp + "homeserv select distinct idhome,root,service_id,supplier_id from " + this.uTmp + "printserv;commit; create index sost on " + this.uTmp + "homeserv (idhome,root,service_id,supplier_id);").ExecuteUpdate();
      try
      {
        this.session.CreateSQLQuery("insert into " + this.uTmp + "homevol select my.company_id,my.idhome,ssp.service_id, sum(homevol),sum(odnhomevol),sum(hvnorm),sum(hvcntr),sum(ohvnorm),sum(ohvcntr) from (select ls.company_id,ls.idhome,service_id,           (select service_id from cmpCrossservice where company_id=ls.company_id and " + strPeriod + " between dbeg and dend and crossservice_id=sp.service_id and crosstype_id=6) as servcr, group_num,           sum(isnull((select sum(volume) from lsRent b where period_id=" + strPeriodId + "  and b.client_id=ls.client_id  and month_id=" + strPeriodId + " and rent_type=0                  and b.service_id=(select first lb.service_id from lsRent lb, " + this.uTmp + "homeserv tmp where period_id=" + strPeriodId + " and client_id=b.client_id and ls.idhome=tmp.idhome and lb.service_id=tmp.service_id and root=sp.service_id and lb.volume<>0 order by lb.service_id) group by b.client_id),0)) as hvnorm,          sum(isnull((select sum(volume) from lsRent b where period_id=" + strPeriodId + "  and b.client_id=ls.client_id  and month_id=" + strPeriodId + " and rent_type=1                   and b.service_id=(select first lb.service_id from lsRent lb, " + this.uTmp + "homeserv tmp where period_id=" + strPeriodId + " and client_id=b.client_id and ls.idhome=tmp.idhome and lb.service_id=tmp.service_id and root=sp.service_id and lb.volume<>0 order by lb.service_id) group by b.client_id),0)) as hvcntr,           hvnorm+hvcntr as homevol,           sum(isnull((select sum(volume) from lsRent b where period_id=" + strPeriodId + "  and b.client_id=ls.client_id  and month_id=" + strPeriodId + " and rent_type=0                  and b.service_id=(select first lb.service_id from lsRent lb, " + this.uTmp + "homeserv tmp where period_id=" + strPeriodId + " and client_id=b.client_id and ls.idhome=tmp.idhome and lb.service_id=tmp.service_id and root=servcr and lb.volume<>0 order by lb.service_id) group by b.client_id),0)) as ohvnorm,           sum(isnull((select sum(volume) from lsRent b where period_id=" + strPeriodId + "  and b.client_id=ls.client_id  and month_id=" + strPeriodId + " and rent_type=1                   and b.service_id=(select first lb.service_id from lsRent lb, " + this.uTmp + "homeserv tmp where period_id=" + strPeriodId + " and client_id=b.client_id and ls.idhome=tmp.idhome and lb.service_id=tmp.service_id and root=servcr and lb.volume<>0 order by lb.service_id) group by b.client_id),0)) as ohvcntr,           ohvnorm+ohvcntr as odnhomevol from lsClient ls,cmpServiceParam sp where ls.company_id=sp.company_id and sp.complex_id=100 and isnull(sp.boilservice_id,0)=1 and idhome in (select idhome from " + this.uTmp + "homeserv) group by ls.company_id,ls.idhome,service_id,sp.group_num )as my,cmpServiceParam ssp where ssp.company_id=my.company_id and ssp.complex_id=100 and (my.service_id=ssp.service_id or my.group_num=ssp.service_id) group by my.company_id,my.idhome,ssp.service_id;call droptmp(1,'tmpReceipthomeserv',current user);").ExecuteUpdate();
      }
      catch
      {
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmReceipts));
      this.dataSet1 = new DataSet();
      this.report1 = new Report();
      this.fdTwoPage = new OpenFileDialog();
      this.fdFromFile = new OpenFileDialog();
      this.panel2 = new Panel();
      this.txbFromFile = new TextBox();
      this.lblFromFile = new Label();
      this.gbDop = new GroupBox();
      this.cbOnlyRent = new CheckBox();
      this.cbOnlyUch = new CheckBox();
      this.txbTwoPage = new TextBox();
      this.dtpSrok = new DateTimePicker();
      this.lblProc = new Label();
      this.txbProc = new TextBox();
      this.lblSrok = new Label();
      this.cbQuarter = new CheckBox();
      this.cbDebt = new CheckBox();
      this.cbCounter = new CheckBox();
      this.cbTwoPage = new CheckBox();
      this.cbCode = new CheckBox();
      this.cbClaim = new CheckBox();
      this.cbOther = new CheckBox();
      this.cbOne = new CheckBox();
      this.cbRezak = new CheckBox();
      this.cbUslBank = new CheckBox();
      this.cbManyOwners = new CheckBox();
      this.cbYur = new CheckBox();
      this.cbClose = new CheckBox();
      this.cbDebit = new CheckBox();
      this.cbPlat = new CheckBox();
      this.cbMonth = new CheckBox();
      this.gbReceipt = new GroupBox();
      this.cbOnly = new CheckBox();
      this.cmbReceipt = new ComboBox();
      this.gbDebt = new GroupBox();
      this.rbDebt1 = new RadioButton();
      this.rbDebt2 = new RadioButton();
      this.rbDebt3 = new RadioButton();
      this.rbDept4 = new RadioButton();
      this.gbPeni = new GroupBox();
      this.rbPeni1 = new RadioButton();
      this.rbPeni2 = new RadioButton();
      this.rbPeni3 = new RadioButton();
      this.rbPeni4 = new RadioButton();
      this.gbBetween = new GroupBox();
      this.clbDistrict = new CheckedListBox();
      this.cmbGroup = new ComboBox();
      this.txbMax = new TextBox();
      this.txbMin = new TextBox();
      this.lblLast = new Label();
      this.lblFirst = new Label();
      this.gbChoice = new GroupBox();
      this.rbChoice1 = new RadioButton();
      this.rbChoice2 = new RadioButton();
      this.rbChoice3 = new RadioButton();
      this.rbChoice4 = new RadioButton();
      this.rbChoice5 = new RadioButton();
      this.pnBtn = new Panel();
      this.btnView = new Button();
      this.button2 = new Button();
      this.button1 = new Button();
      this.btnPrint2 = new Button();
      this.btnPrint = new Button();
      this.btnExit = new Button();
      this.panel1 = new Panel();
      this.mpCurrentPeriod = new MonthPicker();
      this.gbCounters = new GroupBox();
      this.rbCounter3 = new RadioButton();
      this.rbCounter2 = new RadioButton();
      this.rbCounter1 = new RadioButton();
      this.dataSet1.BeginInit();
      this.report1.BeginInit();
      this.panel2.SuspendLayout();
      this.gbDop.SuspendLayout();
      this.gbReceipt.SuspendLayout();
      this.gbDebt.SuspendLayout();
      this.gbPeni.SuspendLayout();
      this.gbBetween.SuspendLayout();
      this.gbChoice.SuspendLayout();
      this.pnBtn.SuspendLayout();
      this.panel1.SuspendLayout();
      this.gbCounters.SuspendLayout();
      this.SuspendLayout();
      this.dataSet1.DataSetName = "NewDataSet";
      this.report1.ReportResourceString = componentResourceManager.GetString("report1.ReportResourceString");
      this.report1.LoadBaseReport += new CustomLoadEventHandler(this.report1_LoadBaseReport);
      this.fdTwoPage.FileName = "openFileDialog1";
      this.fdFromFile.FileName = "openFileDialog1";
      this.fdFromFile.FileOk += new CancelEventHandler(this.fdFromFile_FileOk);
      this.panel2.Controls.Add((Control) this.txbFromFile);
      this.panel2.Controls.Add((Control) this.lblFromFile);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 516);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(935, 46);
      this.panel2.TabIndex = 8;
      this.txbFromFile.Location = new Point(209, 12);
      this.txbFromFile.Name = "txbFromFile";
      this.txbFromFile.Size = new Size(100, 22);
      this.txbFromFile.TabIndex = 1;
      this.lblFromFile.AutoSize = true;
      this.lblFromFile.Location = new Point(9, 15);
      this.lblFromFile.Name = "lblFromFile";
      this.lblFromFile.Size = new Size(194, 16);
      this.lblFromFile.TabIndex = 0;
      this.lblFromFile.Text = "Печать квитанций из файла";
      this.gbDop.Controls.Add((Control) this.cbOnlyRent);
      this.gbDop.Controls.Add((Control) this.cbOnlyUch);
      this.gbDop.Controls.Add((Control) this.txbTwoPage);
      this.gbDop.Controls.Add((Control) this.dtpSrok);
      this.gbDop.Controls.Add((Control) this.lblProc);
      this.gbDop.Controls.Add((Control) this.txbProc);
      this.gbDop.Controls.Add((Control) this.lblSrok);
      this.gbDop.Controls.Add((Control) this.cbQuarter);
      this.gbDop.Controls.Add((Control) this.cbDebt);
      this.gbDop.Controls.Add((Control) this.cbCounter);
      this.gbDop.Controls.Add((Control) this.cbTwoPage);
      this.gbDop.Controls.Add((Control) this.cbCode);
      this.gbDop.Controls.Add((Control) this.cbClaim);
      this.gbDop.Controls.Add((Control) this.cbOther);
      this.gbDop.Controls.Add((Control) this.cbOne);
      this.gbDop.Controls.Add((Control) this.cbRezak);
      this.gbDop.Controls.Add((Control) this.cbUslBank);
      this.gbDop.Controls.Add((Control) this.cbManyOwners);
      this.gbDop.Controls.Add((Control) this.cbYur);
      this.gbDop.Controls.Add((Control) this.cbClose);
      this.gbDop.Controls.Add((Control) this.cbDebit);
      this.gbDop.Controls.Add((Control) this.cbPlat);
      this.gbDop.Controls.Add((Control) this.cbMonth);
      this.gbDop.Dock = DockStyle.Top;
      this.gbDop.Location = new Point(0, 314);
      this.gbDop.Name = "gbDop";
      this.gbDop.Size = new Size(935, 202);
      this.gbDop.TabIndex = 7;
      this.gbDop.TabStop = false;
      this.gbDop.Text = "Дополнительные условия";
      this.cbOnlyRent.AutoSize = true;
      this.cbOnlyRent.Location = new Point(11, 166);
      this.cbOnlyRent.Name = "cbOnlyRent";
      this.cbOnlyRent.Size = new Size(148, 17);
      this.cbOnlyRent.TabIndex = 22;
      this.cbOnlyRent.Text = "Только с начислениями";
      this.cbOnlyRent.UseVisualStyleBackColor = true;
      this.cbOnlyUch.AutoSize = true;
      this.cbOnlyUch.Location = new Point(11, 116);
      this.cbOnlyUch.Name = "cbOnlyUch";
      this.cbOnlyUch.Size = new Size(107, 17);
      this.cbOnlyUch.TabIndex = 21;
      this.cbOnlyUch.Text = "Только учетные";
      this.cbOnlyUch.UseVisualStyleBackColor = true;
      this.txbTwoPage.Location = new Point(763, 42);
      this.txbTwoPage.Name = "txbTwoPage";
      this.txbTwoPage.Size = new Size(100, 22);
      this.txbTwoPage.TabIndex = 20;
      this.dtpSrok.Location = new Point(378, 44);
      this.dtpSrok.Name = "dtpSrok";
      this.dtpSrok.Size = new Size(164, 22);
      this.dtpSrok.TabIndex = 19;
      this.lblProc.AutoSize = true;
      this.lblProc.Location = new Point(439, 22);
      this.lblProc.Name = "lblProc";
      this.lblProc.Size = new Size(20, 16);
      this.lblProc.TabIndex = 18;
      this.lblProc.Text = "%";
      this.txbProc.Location = new Point(405, 19);
      this.txbProc.Name = "txbProc";
      this.txbProc.Size = new Size(22, 22);
      this.txbProc.TabIndex = 17;
      this.txbProc.Text = "3";
      this.txbProc.KeyPress += new KeyPressEventHandler(this.txbProc_KeyPress);
      this.lblSrok.AutoSize = true;
      this.lblSrok.Location = new Point(281, 45);
      this.lblSrok.Name = "lblSrok";
      this.lblSrok.Size = new Size(91, 16);
      this.lblSrok.TabIndex = 16;
      this.lblSrok.Text = "Срок оплаты";
      this.cbQuarter.AutoSize = true;
      this.cbQuarter.Location = new Point(588, 122);
      this.cbQuarter.Name = "cbQuarter";
      this.cbQuarter.Size = new Size(92, 17);
      this.cbQuarter.TabIndex = 15;
      this.cbQuarter.Text = "Квартальная";
      this.cbQuarter.UseVisualStyleBackColor = true;
      this.cbDebt.AutoSize = true;
      this.cbDebt.Location = new Point(588, 96);
      this.cbDebt.Name = "cbDebt";
      this.cbDebt.Size = new Size(100, 17);
      this.cbDebt.TabIndex = 14;
      this.cbDebt.Text = "С должниками";
      this.cbDebt.UseVisualStyleBackColor = true;
      this.cbCounter.AutoSize = true;
      this.cbCounter.Location = new Point(588, 70);
      this.cbCounter.Name = "cbCounter";
      this.cbCounter.Size = new Size(101, 17);
      this.cbCounter.TabIndex = 13;
      this.cbCounter.Text = "Со счетчиками";
      this.cbCounter.UseVisualStyleBackColor = true;
      this.cbTwoPage.AutoSize = true;
      this.cbTwoPage.Location = new Point(588, 44);
      this.cbTwoPage.Name = "cbTwoPage";
      this.cbTwoPage.Size = new Size(136, 17);
      this.cbTwoPage.TabIndex = 12;
      this.cbTwoPage.Text = "Двусторонняя печать";
      this.cbTwoPage.UseVisualStyleBackColor = true;
      this.cbCode.AutoSize = true;
      this.cbCode.Location = new Point(588, 18);
      this.cbCode.Name = "cbCode";
      this.cbCode.Size = new Size(111, 17);
      this.cbCode.TabIndex = 11;
      this.cbCode.Text = "Без штрих - кода";
      this.cbCode.UseVisualStyleBackColor = true;
      this.cbClaim.AutoSize = true;
      this.cbClaim.Location = new Point(284, 140);
      this.cbClaim.Name = "cbClaim";
      this.cbClaim.Size = new Size(146, 17);
      this.cbClaim.TabIndex = 10;
      this.cbClaim.Text = "Претензионное письмо";
      this.cbClaim.UseVisualStyleBackColor = true;
      this.cbOther.AutoSize = true;
      this.cbOther.Location = new Point(284, 116);
      this.cbOther.Name = "cbOther";
      this.cbOther.Size = new Size(177, 17);
      this.cbOther.TabIndex = 9;
      this.cbOther.Text = "Использовать другой шаблон";
      this.cbOther.UseVisualStyleBackColor = true;
      this.cbOne.AutoSize = true;
      this.cbOne.Location = new Point(284, 90);
      this.cbOne.Name = "cbOne";
      this.cbOne.Size = new Size(161, 17);
      this.cbOne.TabIndex = 8;
      this.cbOne.Text = "Лист для одной квитанции";
      this.cbOne.UseVisualStyleBackColor = true;
      this.cbRezak.AutoSize = true;
      this.cbRezak.Location = new Point(284, 64);
      this.cbRezak.Name = "cbRezak";
      this.cbRezak.Size = new Size(140, 17);
      this.cbRezak.TabIndex = 7;
      this.cbRezak.Text = "Сортировка под резак";
      this.cbRezak.UseVisualStyleBackColor = true;
      this.cbUslBank.AutoSize = true;
      this.cbUslBank.Location = new Point(284, 21);
      this.cbUslBank.Name = "cbUslBank";
      this.cbUslBank.Size = new Size(95, 17);
      this.cbUslBank.TabIndex = 6;
      this.cbUslBank.Text = "Услуги банка";
      this.cbUslBank.UseVisualStyleBackColor = true;
      this.cbManyOwners.AutoSize = true;
      this.cbManyOwners.Location = new Point(588, 145);
      this.cbManyOwners.Name = "cbManyOwners";
      this.cbManyOwners.Size = new Size(116, 17);
      this.cbManyOwners.TabIndex = 5;
      this.cbManyOwners.Text = "Все плательщики";
      this.cbManyOwners.UseVisualStyleBackColor = true;
      this.cbYur.AutoSize = true;
      this.cbYur.Checked = true;
      this.cbYur.CheckState = CheckState.Checked;
      this.cbYur.Location = new Point(12, 140);
      this.cbYur.Name = "cbYur";
      this.cbYur.Size = new Size(83, 17);
      this.cbYur.TabIndex = 4;
      this.cbYur.Text = "Без юр.лиц";
      this.cbYur.UseVisualStyleBackColor = true;
      this.cbClose.AutoSize = true;
      this.cbClose.Location = new Point(12, 90);
      this.cbClose.Name = "cbClose";
      this.cbClose.Size = new Size(158, 17);
      this.cbClose.TabIndex = 3;
      this.cbClose.Text = "Без закрытых и архивных";
      this.cbClose.UseVisualStyleBackColor = true;
      this.cbDebit.AutoSize = true;
      this.cbDebit.Location = new Point(12, 64);
      this.cbDebit.Name = "cbDebit";
      this.cbDebit.Size = new Size(84, 17);
      this.cbDebit.TabIndex = 2;
      this.cbDebit.Text = "Дебиторам";
      this.cbDebit.UseVisualStyleBackColor = true;
      this.cbPlat.AutoSize = true;
      this.cbPlat.Checked = true;
      this.cbPlat.CheckState = CheckState.Checked;
      this.cbPlat.Location = new Point(12, 42);
      this.cbPlat.Name = "cbPlat";
      this.cbPlat.Size = new Size(142, 17);
      this.cbPlat.TabIndex = 1;
      this.cbPlat.Text = "Только плательщикам";
      this.cbPlat.UseVisualStyleBackColor = true;
      this.cbPlat.CheckedChanged += new EventHandler(this.cbPlat_CheckedChanged);
      this.cbMonth.AutoSize = true;
      this.cbMonth.Location = new Point(12, 21);
      this.cbMonth.Name = "cbMonth";
      this.cbMonth.Size = new Size(74, 17);
      this.cbMonth.TabIndex = 0;
      this.cbMonth.Text = "За месяц";
      this.cbMonth.UseVisualStyleBackColor = true;
      this.gbReceipt.Controls.Add((Control) this.cbOnly);
      this.gbReceipt.Controls.Add((Control) this.cmbReceipt);
      this.gbReceipt.Dock = DockStyle.Top;
      this.gbReceipt.Location = new Point(0, 270);
      this.gbReceipt.Name = "gbReceipt";
      this.gbReceipt.Size = new Size(935, 44);
      this.gbReceipt.TabIndex = 6;
      this.gbReceipt.TabStop = false;
      this.gbReceipt.Text = "Квитанция";
      this.cbOnly.AutoSize = true;
      this.cbOnly.Location = new Point(335, 18);
      this.cbOnly.Name = "cbOnly";
      this.cbOnly.Size = new Size(238, 17);
      this.cbOnly.TabIndex = 1;
      this.cbOnly.Text = "Поручение на перевод денежных средств";
      this.cbOnly.UseVisualStyleBackColor = true;
      this.cbOnly.Visible = false;
      this.cmbReceipt.FormattingEnabled = true;
      this.cmbReceipt.Location = new Point(35, 14);
      this.cmbReceipt.Name = "cmbReceipt";
      this.cmbReceipt.Size = new Size((int) byte.MaxValue, 24);
      this.cmbReceipt.TabIndex = 0;
      this.gbDebt.Controls.Add((Control) this.rbDebt1);
      this.gbDebt.Controls.Add((Control) this.rbDebt2);
      this.gbDebt.Controls.Add((Control) this.rbDebt3);
      this.gbDebt.Controls.Add((Control) this.rbDept4);
      this.gbDebt.Dock = DockStyle.Top;
      this.gbDebt.Location = new Point(0, 216);
      this.gbDebt.Name = "gbDebt";
      this.gbDebt.Size = new Size(935, 54);
      this.gbDebt.TabIndex = 4;
      this.gbDebt.TabStop = false;
      this.gbDebt.Text = "Выбор долга";
      this.rbDebt1.AutoSize = true;
      this.rbDebt1.Checked = true;
      this.rbDebt1.Location = new Point(15, 33);
      this.rbDebt1.Name = "rbDebt1";
      this.rbDebt1.Size = new Size(169, 17);
      this.rbDebt1.TabIndex = 4;
      this.rbDebt1.TabStop = true;
      this.rbDebt1.Text = "Всем (независимо от долга)";
      this.rbDebt1.UseVisualStyleBackColor = true;
      this.rbDebt2.AutoSize = true;
      this.rbDebt2.Location = new Point(284, 33);
      this.rbDebt2.Name = "rbDebt2";
      this.rbDebt2.Size = new Size(145, 17);
      this.rbDebt2.TabIndex = 3;
      this.rbDebt2.TabStop = true;
      this.rbDebt2.Text = "От 2-х месяцев (включ.)";
      this.rbDebt2.UseVisualStyleBackColor = true;
      this.rbDebt3.AutoSize = true;
      this.rbDebt3.Location = new Point(529, 33);
      this.rbDebt3.Name = "rbDebt3";
      this.rbDebt3.Size = new Size(102, 17);
      this.rbDebt3.TabIndex = 2;
      this.rbDebt3.TabStop = true;
      this.rbDebt3.Text = "От 3-х месяцев";
      this.rbDebt3.UseVisualStyleBackColor = true;
      this.rbDept4.AutoSize = true;
      this.rbDept4.Location = new Point(704, 33);
      this.rbDept4.Name = "rbDept4";
      this.rbDept4.Size = new Size(116, 17);
      this.rbDept4.TabIndex = 1;
      this.rbDept4.TabStop = true;
      this.rbDept4.Text = "Свыше 6 месяцев";
      this.rbDept4.UseVisualStyleBackColor = true;
      this.gbPeni.Controls.Add((Control) this.rbPeni1);
      this.gbPeni.Controls.Add((Control) this.rbPeni2);
      this.gbPeni.Controls.Add((Control) this.rbPeni3);
      this.gbPeni.Controls.Add((Control) this.rbPeni4);
      this.gbPeni.Dock = DockStyle.Top;
      this.gbPeni.Location = new Point(0, 130);
      this.gbPeni.Name = "gbPeni";
      this.gbPeni.Size = new Size(935, 42);
      this.gbPeni.TabIndex = 3;
      this.gbPeni.TabStop = false;
      this.gbPeni.Text = "Начисление пеней";
      this.rbPeni1.AutoSize = true;
      this.rbPeni1.Checked = true;
      this.rbPeni1.Location = new Point(12, 16);
      this.rbPeni1.Name = "rbPeni1";
      this.rbPeni1.Size = new Size(144, 17);
      this.rbPeni1.TabIndex = 3;
      this.rbPeni1.TabStop = true;
      this.rbPeni1.Text = "Пени на дату оплаты (*)";
      this.rbPeni1.UseVisualStyleBackColor = true;
      this.rbPeni2.AutoSize = true;
      this.rbPeni2.Location = new Point(284, 16);
      this.rbPeni2.Name = "rbPeni2";
      this.rbPeni2.Size = new Size(114, 17);
      this.rbPeni2.TabIndex = 2;
      this.rbPeni2.TabStop = true;
      this.rbPeni2.Text = "Не печатать пени";
      this.rbPeni2.UseVisualStyleBackColor = true;
      this.rbPeni3.AutoSize = true;
      this.rbPeni3.Enabled = false;
      this.rbPeni3.Location = new Point(559, 16);
      this.rbPeni3.Name = "rbPeni3";
      this.rbPeni3.Size = new Size(128, 17);
      this.rbPeni3.TabIndex = 1;
      this.rbPeni3.TabStop = true;
      this.rbPeni3.Text = "Пени на дату печати";
      this.rbPeni3.UseVisualStyleBackColor = true;
      this.rbPeni4.AutoSize = true;
      this.rbPeni4.Location = new Point(777, 16);
      this.rbPeni4.Name = "rbPeni4";
      this.rbPeni4.Size = new Size(92, 17);
      this.rbPeni4.TabIndex = 0;
      this.rbPeni4.TabStop = true;
      this.rbPeni4.Text = "Полные пени";
      this.rbPeni4.UseVisualStyleBackColor = true;
      this.gbBetween.Controls.Add((Control) this.clbDistrict);
      this.gbBetween.Controls.Add((Control) this.cmbGroup);
      this.gbBetween.Controls.Add((Control) this.txbMax);
      this.gbBetween.Controls.Add((Control) this.txbMin);
      this.gbBetween.Controls.Add((Control) this.lblLast);
      this.gbBetween.Controls.Add((Control) this.lblFirst);
      this.gbBetween.Dock = DockStyle.Top;
      this.gbBetween.Location = new Point(0, 83);
      this.gbBetween.Name = "gbBetween";
      this.gbBetween.Size = new Size(935, 47);
      this.gbBetween.TabIndex = 2;
      this.gbBetween.TabStop = false;
      this.clbDistrict.FormattingEnabled = true;
      this.clbDistrict.Location = new Point(604, 15);
      this.clbDistrict.Name = "clbDistrict";
      this.clbDistrict.Size = new Size(120, 21);
      this.clbDistrict.TabIndex = 5;
      this.cmbGroup.FormattingEnabled = true;
      this.cmbGroup.Location = new Point(378, 15);
      this.cmbGroup.Name = "cmbGroup";
      this.cmbGroup.Size = new Size(171, 24);
      this.cmbGroup.TabIndex = 4;
      this.txbMax.Location = new Point(238, 15);
      this.txbMax.Name = "txbMax";
      this.txbMax.Size = new Size(100, 22);
      this.txbMax.TabIndex = 3;
      this.txbMin.Location = new Point(65, 15);
      this.txbMin.Name = "txbMin";
      this.txbMin.Size = new Size(100, 22);
      this.txbMin.TabIndex = 2;
      this.lblLast.AutoSize = true;
      this.lblLast.Location = new Point(206, 18);
      this.lblLast.Name = "lblLast";
      this.lblLast.Size = new Size(26, 16);
      this.lblLast.TabIndex = 1;
      this.lblLast.Text = "По";
      this.lblFirst.AutoSize = true;
      this.lblFirst.Location = new Point(42, 18);
      this.lblFirst.Name = "lblFirst";
      this.lblFirst.Size = new Size(17, 16);
      this.lblFirst.TabIndex = 0;
      this.lblFirst.Text = "C";
      this.gbChoice.Controls.Add((Control) this.rbChoice1);
      this.gbChoice.Controls.Add((Control) this.rbChoice2);
      this.gbChoice.Controls.Add((Control) this.rbChoice3);
      this.gbChoice.Controls.Add((Control) this.rbChoice4);
      this.gbChoice.Controls.Add((Control) this.rbChoice5);
      this.gbChoice.Dock = DockStyle.Top;
      this.gbChoice.Location = new Point(0, 38);
      this.gbChoice.Name = "gbChoice";
      this.gbChoice.Size = new Size(935, 45);
      this.gbChoice.TabIndex = 1;
      this.gbChoice.TabStop = false;
      this.gbChoice.Text = "Печатать";
      this.rbChoice1.AutoSize = true;
      this.rbChoice1.Checked = true;
      this.rbChoice1.Location = new Point(15, 18);
      this.rbChoice1.Name = "rbChoice1";
      this.rbChoice1.Size = new Size(44, 17);
      this.rbChoice1.TabIndex = 8;
      this.rbChoice1.TabStop = true;
      this.rbChoice1.Text = "Все";
      this.rbChoice1.UseVisualStyleBackColor = true;
      this.rbChoice1.CheckedChanged += new EventHandler(this.radioButton5_CheckedChanged);
      this.rbChoice2.AutoSize = true;
      this.rbChoice2.Location = new Point(215, 18);
      this.rbChoice2.Name = "rbChoice2";
      this.rbChoice2.Size = new Size(54, 17);
      this.rbChoice2.TabIndex = 6;
      this.rbChoice2.TabStop = true;
      this.rbChoice2.Text = "Счета";
      this.rbChoice2.UseVisualStyleBackColor = true;
      this.rbChoice3.AutoSize = true;
      this.rbChoice3.Location = new Point(380, 18);
      this.rbChoice3.Name = "rbChoice3";
      this.rbChoice3.Size = new Size(75, 17);
      this.rbChoice3.TabIndex = 4;
      this.rbChoice3.TabStop = true;
      this.rbChoice3.Text = "Квартиры";
      this.rbChoice3.UseVisualStyleBackColor = true;
      this.rbChoice4.AutoSize = true;
      this.rbChoice4.Location = new Point(597, 18);
      this.rbChoice4.Name = "rbChoice4";
      this.rbChoice4.Size = new Size(67, 17);
      this.rbChoice4.TabIndex = 2;
      this.rbChoice4.TabStop = true;
      this.rbChoice4.Text = "Участки";
      this.rbChoice4.UseVisualStyleBackColor = true;
      this.rbChoice5.AutoSize = true;
      this.rbChoice5.Location = new Point(815, 18);
      this.rbChoice5.Name = "rbChoice5";
      this.rbChoice5.Size = new Size(63, 17);
      this.rbChoice5.TabIndex = 0;
      this.rbChoice5.TabStop = true;
      this.rbChoice5.Text = "Индекс";
      this.rbChoice5.UseVisualStyleBackColor = true;
      this.pnBtn.Controls.Add((Control) this.btnView);
      this.pnBtn.Controls.Add((Control) this.button2);
      this.pnBtn.Controls.Add((Control) this.button1);
      this.pnBtn.Controls.Add((Control) this.btnPrint2);
      this.pnBtn.Controls.Add((Control) this.btnPrint);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 562);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(935, 40);
      this.pnBtn.TabIndex = 0;
      this.btnView.Location = new Point(467, 8);
      this.btnView.Name = "btnView";
      this.btnView.Size = new Size(130, 23);
      this.btnView.TabIndex = 5;
      this.btnView.Text = "На экран";
      this.btnView.UseVisualStyleBackColor = true;
      this.btnView.Click += new EventHandler(this.btnView_Click);
      this.button2.Location = new Point(323, 8);
      this.button2.Name = "button2";
      this.button2.Size = new Size(75, 23);
      this.button2.TabIndex = 4;
      this.button2.Text = "button2";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.button1.Location = new Point(215, 8);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 3;
      this.button1.Text = "button1";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.btnPrint2.Location = new Point(93, 8);
      this.btnPrint2.Name = "btnPrint2";
      this.btnPrint2.Size = new Size(116, 27);
      this.btnPrint2.TabIndex = 2;
      this.btnPrint2.Text = "Печать2";
      this.btnPrint2.UseVisualStyleBackColor = true;
      this.btnPrint2.Click += new EventHandler(this.btnPrint2_Click);
      this.btnPrint.Location = new Point(12, 5);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(75, 30);
      this.btnPrint.TabIndex = 1;
      this.btnPrint.Text = "Печать";
      this.btnPrint.UseVisualStyleBackColor = true;
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(839, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(84, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.panel1.Controls.Add((Control) this.mpCurrentPeriod);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(935, 38);
      this.panel1.TabIndex = 5;
      this.mpCurrentPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.mpCurrentPeriod.CustomFormat = "MMMM yyyy";
      this.mpCurrentPeriod.Format = DateTimePickerFormat.Custom;
      this.mpCurrentPeriod.Location = new Point(783, 10);
      this.mpCurrentPeriod.Name = "mpCurrentPeriod";
      this.mpCurrentPeriod.OldMonth = 0;
      this.mpCurrentPeriod.ShowUpDown = true;
      this.mpCurrentPeriod.Size = new Size(140, 22);
      this.mpCurrentPeriod.TabIndex = 0;
      this.mpCurrentPeriod.ValueChanged += new EventHandler(this.mpCurrentPeriod_ValueChanged);
      this.gbCounters.Controls.Add((Control) this.rbCounter3);
      this.gbCounters.Controls.Add((Control) this.rbCounter2);
      this.gbCounters.Controls.Add((Control) this.rbCounter1);
      this.gbCounters.Dock = DockStyle.Top;
      this.gbCounters.Location = new Point(0, 172);
      this.gbCounters.Name = "gbCounters";
      this.gbCounters.Size = new Size(935, 44);
      this.gbCounters.TabIndex = 9;
      this.gbCounters.TabStop = false;
      this.gbCounters.Text = "Показания приборов учета";
      this.rbCounter3.AutoSize = true;
      this.rbCounter3.Location = new Point(559, 18);
      this.rbCounter3.Name = "rbCounter3";
      this.rbCounter3.Size = new Size(176, 17);
      this.rbCounter3.TabIndex = 2;
      this.rbCounter3.Text = "Переданные в месяце печати";
      this.rbCounter3.UseVisualStyleBackColor = true;
      this.rbCounter2.AutoSize = true;
      this.rbCounter2.Location = new Point(284, 21);
      this.rbCounter2.Name = "rbCounter2";
      this.rbCounter2.Size = new Size(168, 17);
      this.rbCounter2.TabIndex = 1;
      this.rbCounter2.Text = "Последние на месяц печати";
      this.rbCounter2.UseVisualStyleBackColor = true;
      this.rbCounter1.AutoSize = true;
      this.rbCounter1.Checked = true;
      this.rbCounter1.Location = new Point(15, 18);
      this.rbCounter1.Name = "rbCounter1";
      this.rbCounter1.Size = new Size(146, 17);
      this.rbCounter1.TabIndex = 0;
      this.rbCounter1.TabStop = true;
      this.rbCounter1.Text = "Последние переданные";
      this.rbCounter1.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(935, 602);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.gbDop);
      this.Controls.Add((Control) this.gbReceipt);
      this.Controls.Add((Control) this.gbDebt);
      this.Controls.Add((Control) this.gbCounters);
      this.Controls.Add((Control) this.pnBtn);
      this.Controls.Add((Control) this.gbPeni);
      this.Controls.Add((Control) this.gbBetween);
      this.Controls.Add((Control) this.gbChoice);
      this.Controls.Add((Control) this.panel1);
      this.Name = "FrmReceipts";
      this.Text = "Печать квитанций";
      this.Load += new EventHandler(this.FrmReceipts_Load);
      this.dataSet1.EndInit();
      this.report1.EndInit();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.gbDop.ResumeLayout(false);
      this.gbDop.PerformLayout();
      this.gbReceipt.ResumeLayout(false);
      this.gbReceipt.PerformLayout();
      this.gbDebt.ResumeLayout(false);
      this.gbDebt.PerformLayout();
      this.gbPeni.ResumeLayout(false);
      this.gbPeni.PerformLayout();
      this.gbBetween.ResumeLayout(false);
      this.gbBetween.PerformLayout();
      this.gbChoice.ResumeLayout(false);
      this.gbChoice.PerformLayout();
      this.pnBtn.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.gbCounters.ResumeLayout(false);
      this.gbCounters.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
