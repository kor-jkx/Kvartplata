// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.MainForm
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Counters;
using Kvartplata.Classes;
using Kvartplata.Properties;
using Kvartplata.StaticResourse;
using Microsoft.Office.Interop.Excel;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using NLog.Targets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class MainForm : Form
  {
    public static Logger log = LogManager.GetCurrentClassLogger();
    private FormStateSaver formStateSaver = new FormStateSaver(MainForm.container);
    private int[] currentRow = new int[5];
    private bool _newCheckCrt = false;
    private IContainer components = (IContainer) null;
    private ISession session;
    private short VisibleProperty;
    public Company currentCompany;
    public Raion currentRaion;
    public Home currentHome;
    public LsClient currentLs;
    private Period closedPeriod;
    private static IContainer container;
    private bool noReloadList;
    private string str1;
    private FrmSpash frmSplash;
    private Microsoft.Office.Interop.Excel.Application ObjExcel;
    private Workbook ObjWorkBook;
    private Worksheet ObjWorkSheet;
    private bool closedPasp;
    private bool closedKvr;
    private ToolStripMenuItem tsmiTools;
    private Panel pnBtn;
    private System.Windows.Forms.Button btnExit;
    private Panel pnUp;
    private System.Windows.Forms.Button btnUp;
    private System.Windows.Forms.Label lblCaption;
    private ToolStripMenuItem tsmiPayment;
    private ToolStripMenuItem tsmiSearch;
    private System.Windows.Forms.Label lblKvrCloseValue;
    private System.Windows.Forms.Label lblKvrClose;
    private ToolStripMenuItem tsmiHouseAdministration;
    private ToolStripMenuItem tsmiDictionary;
    private ToolStripMenuItem tsmiTariffs;
    private ToolStripMenuItem tsmiFacilities;
    private ToolStripMenuItem tsmiReceipts;
    private ToolStripMenuItem tsmiAbsence;
    private ToolStripMenuItem tsmiQuality;
    private ToolStripMenuItem tsmiCounters;
    private ToolStripMenuItem tsmiSourcePayment;
    private ToolStripMenuItem tsmiPurposePayment;
    private ToolStripMenuItem tsmiParameters;
    private ToolStripMenuItem tsmiTypeDocuments;
    private ToolStripMenuItem tsmiTypeCounters;
    private ToolStripMenuItem tsmiSuppliers;
    private ToolStripMenuItem tsmiDicReceipts;
    private ToolStripMenuItem tsmiServiceOrganizations;
    private ToolStripMenuItem tsmiCalculationPeriod;
    private ToolStripMenuItem tsmiCalcPerPayment;
    private ToolStripMenuItem tsmiPeni;
    private ToolStripMenuItem tsmiHome;
    private ToolStripMenuItem tsmiCalculation;
    private ToolStripMenuItem tsmiReports;
    private System.Windows.Forms.Label lblPaspClosed;
    private System.Windows.Forms.Label lblPeniClosed;
    private System.Windows.Forms.Label lblPeni;
    private System.Windows.Forms.Label lblPasp;
    private System.Windows.Forms.Label lblKv;
    private ToolStripMenuItem tsmiFastSearch;
    private ToolStripMenuItem tsmiAdvancedSearch;
    private ToolStripMenuItem tsmiAbout;
    private ToolStripMenuItem tsmiDicServices;
    private ToolStripMenuItem tsmiCalcPerPaymentCloseMonth;
    private ToolStripMenuItem tsmiCalcPerPaymentOpenMonth;
    private ToolStripMenuItem tsmiPeniCloseMonth;
    private ToolStripMenuItem tsmiPeniOpenMonth;
    private ProgressBar pbar;
    private MonthPicker dtmpCurrentPeriod;
    private ToolStripMenuItem tsmiRecvisitsAndParameters;
    private ToolStripMenuItem tsmiOperations;
    private MonthPicker mpCurrentPeriod;
    private ToolStripMenuItem tsmiHelp;
    public HelpProvider hp;
    private SaveFileDialog sfCRT;
    private ToolStripMenuItem tsmiFlatSearch;
    private ToolStripMenuItem tsmiStreetSearch;
    private DataGridView dgvMainList;
    private System.Windows.Forms.Label lblOszn;
    private System.Windows.Forms.Label lblOsznClosed;
    private ToolStripMenuItem tsmiSocialProtection;
    private ToolStripMenuItem tsmiSocialProtectionCloseMonth;
    private ToolStripMenuItem tsmiSocialProtectionOpenMonth;
    private ToolStripMenuItem tsmiLoadUnLoad;
    private OpenFileDialog fdHelpLoad;
    private ToolStripMenuItem tsmiLoadFacilities;
    private ToolStripMenuItem tsmiInsuranceUnLoad;
    private SaveFileDialog sfdStrahUnLoad;
    private ToolStripMenuItem tsmiExecute;
    private OpenFileDialog fdSQL;
    private ToolStripMenuItem tsmiCharge;
    private ToolStripMenuItem tsmiChargeCloseMonth;
    private ToolStripMenuItem tsmiChargeOpenMonth;
    private System.Windows.Forms.Label lblKvr;
    private ToolStripMenuItem tsmiExecuteScript;
    private ToolStripMenuItem tsmiBanks;
    private ToolStripMenuItem tsmiOrganizations;
    private ToolStripMenuItem tsmiTypeLocationCounter;
    private ToolStripMenuItem tsmiTypeSeals;
    private SaveFileDialog sfd;
    private ToolStripMenuItem tsmiExternalData;
    private ToolStripMenuItem tsmiPrimaryDocuments;
    private ToolStripMenuItem tsmiPrimaryDocCloseMonth;
    private ToolStripMenuItem tsmiPrimaryDocOpenMonth;
    private System.Windows.Forms.Label lblPriorClosed;
    private System.Windows.Forms.Label lblPrior;
    private ToolStripMenuItem tsmiNoteBook;
    private ToolStripMenuItem tsmiTypeBindingServices;
    private ToolStripMenuItem tsmiBindingServices;
    private ToolStripMenuItem tsmiEnterTariffs;
    private ToolStripMenuItem tsmiHomeParametrs;
    private ToolStripMenuItem tsmiMakeEntrance;
    private ToolStripMenuItem tsmiTypeNoteBook;
    private ToolStripMenuItem tsmiLoadPayments;
    private ToolStripMenuItem tsmiContractSearch;
    private ToolStripMenuItem tsmiPrintBill;
    private System.Windows.Forms.Button btnWOW;
    private ToolStripMenuItem tsmiNewLsClient;
    private ToolStripMenuItem tsmiLsClientClose;
    private ToolStripMenuItem tsmiLsKvartplata;
    private ToolStripMenuItem tsmiLsArenda;
    private ToolStripMenuItem tsmiPrintPolicy;
    private ToolStripMenuItem tsmiGuilds;
    private ToolStripMenuItem tsmiPersonenNumberSearch;
    private ToolStripMenuItem tsmiContractOrganization;
    private ToolStripMenuItem tsmiUZP;
    private ToolStripMenuItem tsmiDataExchangeSocialProtection;
    private ToolStripMenuItem tsmiInspectionContracts;
    private ToolStripMenuItem tsmiSnilsSearch;
    private ToolStripMenuItem tsmiSprAccounts;
    private ToolStripMenuItem tsmiOldReceipt;
    private ToolStripMenuItem tsmiNewReceipt;
    private ToolStripMenuItem капитальныйРемонтToolStripMenuItem;
    private ToolStripMenuItem tsmiCheckOverhaul;
    private ToolStripMenuItem asdaToolStripMenuItem;
    private ToolStripMenuItem tsmiTypeAccount;
    private ToolStripMenuItem tsmiRcptAccount;
    public MenuStrip msMainMenu;
    private ToolStripMenuItem tsmiL_U_hmReceipt;

    public MainForm()
    {
      MainForm.log.Info("Старт!!!");
      this.formStateSaver.ParentForm = (Form) this;
      MainForm.log.Info("Проверяем лицензию!!!");
      if (!KvrplHelper.CheckAuthorization(false, false) && !KvrplHelper.CheckAuthorization(true, false) && !KvrplHelper.CheckAuthorization(false, true))
      {
        int num = (int) MessageBox.Show("Приложение не может запуститься на данном компьютере, потому что отсутствует ключ лицензирования! Чтобы получить ключ, необходимо обратиться к разработчику", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        Environment.Exit(0);
      }
      MainForm.log.Info("Подключаемся к базе!!!");
      FrmConnect frmConnect = new FrmConnect();
      int num1 = (int) frmConnect.ShowDialog();
      frmConnect.Dispose();
      Domain.Init();
      this.InitializeComponent();
      try
      {
        Options.Kvartplata = KvrplHelper.CheckLicence(100);
        Options.Arenda = KvrplHelper.CheckLicence(110);
        Options.CollectiveDevice = KvrplHelper.CheckLicence(112);
        Options.Overhaul = KvrplHelper.CheckLicence(113);
        if (!Options.Kvartplata && !Options.Arenda && !Options.CollectiveDevice && !Options.Overhaul)
        {
          int num2 = (int) MessageBox.Show("Программа не прошла проверку на подлинность ключей лицензирования! Обратитесь к разработчику.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          Environment.Exit(0);
        }
        if (Options.PrefixWindow != "")
        {
          this.Name = Options.PrefixWindow + "_" + this.Name;
          this.Text = Options.PrefixWindow + "_" + this.Text;
        }
        this.VisibleProperty = (short) 0;
        this.session = Domain.CurrentSession;
        OleDbDataReader oleDbDataReader1 = OleDbHelper.ExecuteReader(string.Format("Provider={4};Eng={0};Uid={1};Pwd={2}; Links={3}", (object) Options.BaseName, (object) Options.Login, (object) Options.Pwd, (object) ("tcpip{" + Options.Host + "}"), (object) Options.Provider), CommandType.Text, "select next_connection (null)", 1000);
        string str1 = "";
        int num3 = 0;
        string str2 = "%EXE=%Kvartplata.exe;%";
        if (oleDbDataReader1.Read())
          str1 = Convert.ToString(oleDbDataReader1[0]);
        oleDbDataReader1.Close();
        while (str1 != "")
        {
          string commandText = string.Format("select (if (connection_property('AppInfo','{0}') like '{2}') and            (connection_property('userid','{0}')='{1}') then 1 else 0 endif)", (object) str1, (object) Options.Login, (object) str2);
          try
          {
            OleDbDataReader oleDbDataReader2 = OleDbHelper.ExecuteReader(string.Format("Provider={4};Eng={0};Uid={1};Pwd={2}; Links={3}", (object) Options.BaseName, (object) Options.Login, (object) Options.Pwd, (object) ("tcpip{" + Options.Host + "}"), (object) Options.Provider), CommandType.Text, commandText, 1000);
            if (oleDbDataReader2.Read() && Convert.ToInt32(oleDbDataReader2[0]) == 1)
              ++num3;
            oleDbDataReader2.Close();
            OleDbDataReader oleDbDataReader3 = OleDbHelper.ExecuteReader(string.Format("Provider={4};Eng={0};Uid={1};Pwd={2}; Links={3}", (object) Options.BaseName, (object) Options.Login, (object) Options.Pwd, (object) ("tcpip{" + Options.Host + "}"), (object) Options.Provider), CommandType.Text, "select next_connection (" + str1 + ")", 1000);
            if (oleDbDataReader3.Read())
              str1 = Convert.ToString(oleDbDataReader3[0]);
            oleDbDataReader3.Close();
          }
          catch (Exception ex)
          {
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
        }
        if (num3 > 2)
        {
          int num2 = (int) MessageBox.Show("Такое приложение уже запущенно этим пользователем", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          Environment.Exit(0);
        }
        Options.Complex = this.session.Get<Complex>((object) 100);
        Options.ComplexPasp = this.session.Get<Complex>((object) 102);
        Options.ComplexPrior = this.session.Get<Complex>((object) 108);
        Options.ComplexArenda = this.session.Get<Complex>((object) 110);
        Options.ComplexKapRemont = this.session.Get<Complex>((object) 113);
        Options.MaxPeriod = this.session.CreateQuery("from Period where PeriodName=(select max(p.PeriodName) from Period p)").List<Period>()[0];
        Options.MinPeriod = this.session.CreateQuery("from Period where PeriodName=(select min(p.PeriodName) from Period p where p.PeriodId<>0 )").List<Period>()[0];
        Options.Period = KvrplHelper.SaveCurrentPeriod(DateTime.Now);
        Options.Company = this.session.Get<Company>((object) Options.Company.CompanyId);
        Options.MainConditions1 = string.Format(" and ls.Complex.IdFk in ({0},{1})", (object) Options.Complex.IdFk, (object) Options.ComplexArenda.IdFk);
        Options.MainConditions2 = string.Format(" and ls.complex_id in ({0},{1})", (object) Options.Complex.IdFk, (object) Options.ComplexArenda.IdFk);
        Options.HomeType = " and h.HomeType in (1,2)";
        if (!Options.Kvartplata)
        {
          Options.MainConditions1 = string.Format(" and ls.Complex.IdFk={0}", (object) Options.ComplexArenda.IdFk);
          Options.MainConditions2 = string.Format(" and ls.complex_id={0}", (object) Options.ComplexArenda.IdFk);
          Options.HomeType = " and h.HomeType in (1,2)";
          this.tsmiReceipts.Visible = false;
        }
        if (!Options.Arenda)
        {
          Options.MainConditions1 = string.Format(" and ls.Complex.IdFk={0}", (object) Options.Complex.IdFk);
          Options.MainConditions2 = string.Format(" and ls.complex_id={0}", (object) Options.Complex.IdFk);
          Options.HomeType = " and h.HomeType=1";
          this.tsmiPrintBill.Visible = false;
        }
        try
        {
          Options.TypeFio = Convert.ToInt16(this.session.CreateSQLQuery("select count(*) as cnt from systable where table_name='frPers' and creator=1").List()[0]);
        }
        catch
        {
          Options.TypeFio = (short) 0;
        }
        int city = Options.City;
        if (Options.Company != null)
        {
          try
          {
            city = Convert.ToInt32(KvrplHelper.BaseValue(1, Options.Company));
            if (city == 0)
              city = Options.City;
          }
          catch (Exception ex)
          {
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
          IList<Company> companyList = this.session.CreateCriteria(typeof (Company)).Add((ICriterion) Restrictions.Eq("CompanyId", (object) Options.Company.CompanyId)).List<Company>();
          if (companyList.Count > 0)
          {
            this.currentCompany = companyList[0];
            Options.Company = this.currentCompany;
            this.currentRaion = this.currentCompany.Raion;
            this.VisibleProperty = (short) 2;
          }
        }
        else if ((uint) Options.Raion > 0U)
        {
          IList<Raion> raionList = this.session.CreateCriteria(typeof (Raion)).Add((ICriterion) Restrictions.Eq("IdRnn", (object) Options.Raion)).List<Raion>();
          if (raionList.Count > 0)
          {
            this.currentRaion = raionList[0];
            this.VisibleProperty = (short) 1;
          }
        }
        new ToolTip().SetToolTip((Control) this.btnUp, "На уровень вверх");
        this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
        this.str1 = "Word-echo,empty phrase";
        if (Convert.ToInt32(KvrplHelper.BaseValue(25, Options.Company)) != 1)
          this.HideInformation();
        this.ShowButtonsForCity(city);
        Options.Proxy = this.session.CreateQuery(string.Format("select p from Proxy p,Operation o where p.Operation=o and p.UserName='{0}' and (o.FKId=100 or o.FKId=113) order by o.OprId", (object) Options.Login)).List<Proxy>();
        this.session.Clear();
        if (!Options.Arenda)
        {
          this.tsmiContractSearch.ShortcutKeys = Keys.None;
          this.tsmiContractSearch.Visible = false;
          this.tsmiInspectionContracts.Visible = false;
        }
        if (Options.Overhaul)
          this.tsmiCheckOverhaul.Visible = true;
        else
          this.tsmiCheckOverhaul.Visible = false;
        if (KvrplHelper.CheckProxy(83, 1, this.currentCompany, false) || KvrplHelper.CheckProxy(84, 1, this.currentCompany, false))
          this.tsmiSprAccounts.Visible = true;
        else
          this.tsmiSprAccounts.Visible = false;
        this.LoadGrid();
        if ((int) this.VisibleProperty == 1)
          this.ShowPeriod4Company();
        try
        {
          MainForm.log.Info("FileTarget ");
          ((FileTarget) LogManager.Configuration.FindTargetByName("file")).FileName = (NLog.Layouts.Layout) ("${basedir}/Configuration/" + Options.Login.ToString() + "/Kvartplata_error.log");
        }
        catch (Exception ex)
        {
          MainForm.log.Info<Exception>(ex.InnerException);
          int num2 = (int) MessageBox.Show(ex.Message);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      catch (Exception ex)
      {
        MainForm.log.Info<Exception>(ex.InnerException);
        int num2 = (int) MessageBox.Show(ex.Message);
      }
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (MessageBox.Show("Вы уверены, что хотите выйти из программы?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
      {
        if (Options.Error && MessageBox.Show("При выполнении программы были обнаружены ошибки. Открыть файл для просмотра ошибок?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
          Process.Start(Options.PathProfile + "\\Kvartplata_error.log");
        Domain.Close();
      }
      else
        e.Cancel = true;
    }

    private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.F12 && KvrplHelper.CheckProxy(42, 2, (Company) null, true))
      {
        try
        {
          new FrmMain(Options.Provider, Options.BaseName, Options.Host, Options.Login, Options.Pwd, Convert.ToInt16(Options.Complex.ComplexId)).Show();
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      if (e.KeyCode != Keys.F11 || Options.City != 28)
        return;
      FrmQuickPay frmQuickPay = new FrmQuickPay();
      int num = (int) frmQuickPay.ShowDialog();
      frmQuickPay.Dispose();
    }

    private void MainForm_Shown(object sender, EventArgs e)
    {
      MainForm.log.Info("Начало показа формы!!!");
      this.dgvMainList.Focus();
      MainForm.log.Info("Конец показа формы!!!");
    }

    private void btnWOW_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(34, 1, this.currentCompany, true))
        return;
      this.Cursor = Cursors.WaitCursor;
      FrmReceipts frmReceipts = new FrmReceipts(this.VisibleProperty, this.currentCompany == null ? Options.City : Convert.ToInt32(KvrplHelper.BaseValue(1, this.currentCompany)), this.currentRaion, this.currentCompany, this.currentHome, this.currentLs);
      int num = (int) frmReceipts.ShowDialog();
      frmReceipts.Dispose();
      this.Cursor = Cursors.Default;
      DateTime dateTime1 = this.mpCurrentPeriod.Value;
      DateTime? periodName = Options.Period.PeriodName;
      DateTime dateTime2 = periodName.Value;
      if (!(dateTime1 != dateTime2))
        return;
      this.mpCurrentPeriod.OldMonth = 0;
      MonthPicker mpCurrentPeriod = this.mpCurrentPeriod;
      periodName = Options.Period.PeriodName;
      DateTime dateTime3 = periodName.Value;
      mpCurrentPeriod.Value = dateTime3;
    }

    private void btnArenda_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(34, 1, this.currentCompany, true))
        return;
      this.Cursor = Cursors.WaitCursor;
      FrmBills frmBills = new FrmBills(this.VisibleProperty, this.currentCompany == null ? Options.City : Convert.ToInt32(KvrplHelper.BaseValue(1, this.currentCompany)), this.currentRaion, this.currentCompany, this.currentHome, this.currentLs);
      int num = (int) frmBills.ShowDialog();
      frmBills.Dispose();
      this.Cursor = Cursors.Default;
      DateTime dateTime1 = this.mpCurrentPeriod.Value;
      DateTime? periodName = Options.Period.PeriodName;
      DateTime dateTime2 = periodName.Value;
      if (!(dateTime1 != dateTime2))
        return;
      this.mpCurrentPeriod.OldMonth = 0;
      MonthPicker mpCurrentPeriod = this.mpCurrentPeriod;
      periodName = Options.Period.PeriodName;
      DateTime dateTime3 = periodName.Value;
      mpCurrentPeriod.Value = dateTime3;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnUp_Click(object sender, EventArgs e)
    {
      switch (this.VisibleProperty)
      {
        case 1:
          this.currentRaion = (Raion) null;
          this.VisibleProperty = (short) 0;
          break;
        case 2:
          this.currentLs = (LsClient) null;
          this.currentHome = (Home) null;
          this.currentCompany = (Company) null;
          this.VisibleProperty = (short) 1;
          break;
        case 3:
          this.currentLs = (LsClient) null;
          this.currentHome = (Home) null;
          this.VisibleProperty = (short) 2;
          break;
        case 4:
          this.currentLs = (LsClient) null;
          this.VisibleProperty = (short) 3;
          break;
      }
      if (!this.noReloadList)
        this.LoadGrid();
      else
        this.noReloadList = false;
      if (this.dgvMainList.Rows.Count > 0)
      {
        try
        {
          this.dgvMainList.CurrentCell = !this.dgvMainList.Rows[0].Cells[0].Visible ? this.dgvMainList.Rows[this.currentRow[(int) this.VisibleProperty]].Cells[5] : this.dgvMainList.Rows[this.currentRow[(int) this.VisibleProperty]].Cells[0];
        }
        catch
        {
          this.dgvMainList.CurrentCell = this.dgvMainList.Rows[0].Cells[0];
        }
      }
      this.currentRow[(int) this.VisibleProperty] = 0;
      if ((int) this.VisibleProperty == 1)
        this.ShowPeriod4Company();
      this.dgvMainList.Focus();
    }

    private void dgvMainList_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Return)
        this.NextLevel(true);
      if (e.KeyCode != Keys.Back)
        return;
      this.btnUp_Click(sender, (EventArgs) e);
    }

    private void dgvMainList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left)
        return;
      this.NextLevel(false);
    }

    private void dgvMainList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
    }

    private void dgvMainList_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Return && this.dgvMainList.Rows.Count > 0)
        this.dgvMainList.CurrentCell = !this.dgvMainList.Rows[0].Cells[0].Visible ? this.dgvMainList.Rows[0].Cells[5] : this.dgvMainList.Rows[0].Cells[0];
      if ((int) this.VisibleProperty != 1 || e.KeyCode != Keys.Up && e.KeyCode != Keys.Down)
        return;
      this.ShowPeriod4Company();
    }

    private void dgvMainList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if ((int) this.VisibleProperty != 1)
        return;
      this.ShowPeriod4Company();
    }

    private void dgvMainList_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    private void dgvMainList_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (this.dgvMainList.DataSource == null)
        return;
      DataGridViewRow row = (sender as DataGridView).Rows[e.RowIndex];
      if ((int) this.VisibleProperty == 2 && this.tsmiDictionary.Enabled && !this.tsmiHome.Enabled)
      {
        try
        {
          if ((int) ((Home) row.DataBoundItem).Company.CompanyId == (int) this.currentCompany.CompanyId)
            row.DefaultCellStyle.ForeColor = Color.Black;
          else
            row.DefaultCellStyle.ForeColor = Color.Gray;
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      if ((int) this.VisibleProperty == 3 && this.tsmiHome.Enabled)
      {
        try
        {
          if (((LsClient) row.DataBoundItem).Status != 4 && ((LsClient) row.DataBoundItem).Status != 5)
            row.DefaultCellStyle.ForeColor = Color.Black;
          else
            row.DefaultCellStyle.ForeColor = Color.Gray;
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
    }

    private void tsmiTools_Click(object sender, EventArgs e)
    {
      Company company = Options.Company;
      Raion currentRaion = this.currentRaion;
      FrmOptions frmOptions = new FrmOptions();
      int num = (int) frmOptions.ShowDialog();
      frmOptions.Dispose();
      if (company != null && Options.Company != null && (int) company.CompanyId != (int) Options.Company.CompanyId || currentRaion != null && currentRaion.IdRnn != Options.Raion)
      {
        if (Options.Company != null)
        {
          IList<Company> companyList = this.session.CreateCriteria(typeof (Company)).Add((ICriterion) Restrictions.Eq("CompanyId", (object) Options.Company.CompanyId)).List<Company>();
          if (companyList.Count > 0)
          {
            this.currentCompany = companyList[0];
            Options.Company = this.currentCompany;
            this.currentRaion = this.currentCompany.Raion;
            this.VisibleProperty = (short) 2;
          }
        }
        else if ((uint) Options.Raion > 0U)
        {
          IList<Raion> raionList = this.session.CreateCriteria(typeof (Raion)).Add((ICriterion) Restrictions.Eq("IdRnn", (object) Options.Raion)).List<Raion>();
          if (raionList.Count > 0)
          {
            this.currentRaion = raionList[0];
            this.VisibleProperty = (short) 1;
          }
        }
        else
          this.VisibleProperty = (short) 0;
      }
      this.session.Clear();
      this.LoadGrid();
    }

    private void tsmiReports_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(34, 1, this.currentCompany, true))
        return;
      this.Cursor = Cursors.WaitCursor;
      int num1 = !KvrplHelper.CheckProxy(32, 2, this.currentCompany, false) ? 0 : 1;
      int admin = !Options.Kvartplata || !Options.Arenda ? (!Options.Kvartplata ? num1 + 10 : num1 + 0) : num1 + 20;
      short num2;
      int idcity;
      if (this.currentCompany != null)
      {
        num2 = this.currentCompany.CompanyId;
        idcity = Convert.ToInt32(KvrplHelper.BaseValue(1, this.currentCompany));
      }
      else
      {
        num2 = (short) 0;
        idcity = Options.City;
      }
      int Home = this.currentHome == null ? 0 : this.currentHome.IdHome;
      int Rnn = this.currentRaion == null || (int) num2 != 0 ? 0 : (int) Convert.ToInt16(this.currentRaion.IdRnn);
      try
      {
        CallDll.Rep(idcity, 0, Rnn, (int) num2, Home, 0, 0, Options.Period.PeriodId, admin, Options.Alias, Options.Login, Options.Pwd);
      }
      catch (Exception ex)
      {
        int num3 = (int) MessageBox.Show("Невозможно вызвать библиотеку отчетов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      this.Cursor = Cursors.Default;
    }

    private void tsmiPayment_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      if (this.currentHome != null && this.currentHome.IdHome == 1)
      {
        int num1 = (int) MessageBox.Show("Ввод платежей возможен только из лицевых", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        FrmPayment frmPayment = new FrmPayment(this.currentRaion, this.currentCompany, this.currentHome, this.currentLs, (int) this.VisibleProperty);
        int num2 = (int) frmPayment.ShowDialog();
        frmPayment.Dispose();
      }
      this.Cursor = Cursors.Default;
      DateTime dateTime1 = this.mpCurrentPeriod.Value;
      DateTime? periodName = Options.Period.PeriodName;
      DateTime dateTime2 = periodName.Value;
      if (dateTime1 != dateTime2)
      {
        this.mpCurrentPeriod.OldMonth = 0;
        MonthPicker mpCurrentPeriod = this.mpCurrentPeriod;
        periodName = Options.Period.PeriodName;
        DateTime dateTime3 = periodName.Value;
        mpCurrentPeriod.Value = dateTime3;
      }
      this.dgvMainList.Focus();
    }

    private void tsmiCounters_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(42, 1, this.currentCompany, true))
        return;
      this.Cursor = Cursors.WaitCursor;
      FrmCounters frmCounters = new FrmCounters(this.VisibleProperty, this.currentCompany, this.currentHome);
      int num = (int) frmCounters.ShowDialog();
      frmCounters.Dispose();
      this.Cursor = Cursors.Default;
      if (!(this.mpCurrentPeriod.Value != Options.Period.PeriodName.Value))
        return;
      this.mpCurrentPeriod.OldMonth = 0;
      this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
    }

    private void tsmiCalculation_Click(object sender, EventArgs e)
    {
      int num1 = this.CRTCheck();
      if ((uint) num1 > 0U)
      {
        if (num1 == -1)
        {
          int num2 = (int) MessageBox.Show("Ошибка лицензии, у Вас превышено допустипое количество лицевых!\nДля уточнения настроек лицензии обратитесь в службу поддержки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        if (num1 <= 0)
          return;
        int num3 = (int) MessageBox.Show("Проверка файла инициализации дает сбой, обратитесь к разработчику для устранения ошибки!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (KvrplHelper.MinMaxClosedPeriod("min", (int) this.VisibleProperty, this.currentRaion, this.currentCompany, this.session) >= Options.Period.PeriodId && MessageBox.Show("Месяц закрыт. Продолжить?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
          return;
        int num2;
        if (KvrplHelper.MinMaxClosedPeriod("max", (int) this.VisibleProperty, this.currentRaion, this.currentCompany, this.session) + 2 <= Options.Period.PeriodId)
        {
          string[] strArray = new string[5];
          strArray[0] = "Выбран месяц: ";
          int index1 = 1;
          DateTime? periodName = Options.Period.PeriodName;
          string str1 = periodName.Value.ToString("MMMM", (IFormatProvider) CultureInfo.CurrentCulture);
          strArray[index1] = str1;
          int index2 = 2;
          string str2 = " ";
          strArray[index2] = str2;
          int index3 = 3;
          periodName = Options.Period.PeriodName;
          string str3 = periodName.Value.Year.ToString();
          strArray[index3] = str3;
          int index4 = 4;
          string str4 = ". Продолжить?";
          strArray[index4] = str4;
          num2 = MessageBox.Show(string.Concat(strArray), "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel ? 1 : 0;
        }
        else
          num2 = 0;
        if (num2 != 0)
          return;
        this.Cursor = Cursors.WaitCursor;
        string str5 = "";
        string.Format(" select client_id from DBA.lsClient c  where c.company_id in  (select (if a.codeu = 0 then  c.company_id   else (if a.codeu <> 0 then  ( if a.areal=1 then   (select company_id from DBA.cmpTransfer where kvr_cmp = a.codeu)  else  a.codeu  endif )endif)endif)from DBA.adm_proxyopr a where username='{0}' and proxyopr=2 and idopr=35) ", (object) Options.Login);
        switch (this.VisibleProperty)
        {
          case 0:
            str5 = " and client_id in (select client_id from DBA.lsclient)";
            break;
          case 1:
            str5 = string.Format(" and client_id in (select client_id from DBA.lsclient c where c.company_id in (select company_id from DBA.dcCompany where Rnn_id={0}) )", (object) this.currentRaion.IdRnn);
            break;
          case 2:
            str5 = string.Format(" and client_id in (select client_id from DBA.lsclient c where c.company_id={0} )", (object) this.currentCompany.CompanyId);
            break;
          case 3:
            str5 = string.Format(" and client_id in (select client_id from DBA.lsclient c where c.IdHome={0} and c.company_id={1} )", (object) this.currentHome.IdHome, (object) this.currentCompany.CompanyId);
            break;
        }
        if (this._newCheckCrt && ((int) this.VisibleProperty == 0 || (int) this.VisibleProperty == 1))
        {
          IList list1 = (IList) new ArrayList();
          IList list2 = this.currentRaion == null || (uint) this.currentRaion.IdRnn <= 0U ? this.session.CreateQuery(string.Format("select c from Company c,Raion r,Transfer t where c.Raion=r and t.Company=c and t.KvrCmp is not null order by c.CompanyId")).List() : this.session.CreateQuery(string.Format("select c from Company c,Raion r,Transfer t where c.Raion=r and r.IdRnn={0} and t.Company=c and t.KvrCmp is not null order by c.CompanyId", (object) this.currentRaion.IdRnn)).List();
          IList list3 = (IList) new ArrayList();
          foreach (Company cmp in (IEnumerable) list2)
          {
            if (KvrplHelper.AccessToCompany(97, cmp, 1))
              list3.Add((object) cmp);
          }
          string str1 = " and client_id in (select client_id from DBA.lsclient c where c.company_id in ( ";
          foreach (Company company in (IEnumerable) list3)
            str1 = str1 + (object) company.CompanyId + ",";
          str5 = str1.Remove(str1.LastIndexOf(","), 1) + ")";
        }
        try
        {
          CallDll.Rent(Options.Period.PeriodId, !Options.RentMSP ? str5 : "***" + str5, Options.Alias, Options.Login, Options.Pwd);
        }
        catch (Exception ex)
        {
          int num3 = (int) MessageBox.Show("Невозможно выполнить расчет. Библиотека не найдена.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        this.Cursor = Cursors.Default;
      }
    }

    private void tsmiReceipts_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(34, 1, this.currentCompany, true))
        return;
      this.Cursor = Cursors.WaitCursor;
      int Raion = this.currentRaion == null ? 0 : (int) Convert.ToInt16(this.currentRaion.IdRnn);
      short num1;
      int City;
      if (this.currentCompany != null)
      {
        num1 = this.currentCompany.CompanyId;
        City = Convert.ToInt32(KvrplHelper.BaseValue(1, this.currentCompany));
        if (City == 0)
          City = Options.City;
      }
      else
      {
        num1 = (short) 0;
        City = Options.City;
      }
      int Home = this.currentHome == null ? 0 : this.currentHome.IdHome;
      int Lic = this.currentLs == null ? 0 : this.currentLs.ClientId;
      try
      {
        CallDll.WReceipt((int) this.VisibleProperty, Raion, (int) num1, City, Home, Lic, Options.Period.PeriodName.Value.ToOADate(), Options.Alias, Options.Login, Options.Pwd, Convert.ToInt32(((ToolStripItem) sender).Tag));
      }
      catch (Exception ex)
      {
        int num2 = (int) MessageBox.Show("Невозможно выполнить печать квитанций", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      this.Cursor = Cursors.Default;
    }

    private void tsmiPrintBill_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(34, 1, this.currentCompany, true))
        return;
      this.Cursor = Cursors.WaitCursor;
      FrmBills frmBills = new FrmBills(this.VisibleProperty, this.currentCompany == null ? Options.City : Convert.ToInt32(KvrplHelper.BaseValue(1, this.currentCompany)), this.currentRaion, this.currentCompany, this.currentHome, this.currentLs);
      int num = (int) frmBills.ShowDialog();
      frmBills.Dispose();
      this.Cursor = Cursors.Default;
      DateTime dateTime1 = this.mpCurrentPeriod.Value;
      DateTime? periodName = Options.Period.PeriodName;
      DateTime dateTime2 = periodName.Value;
      if (dateTime1 != dateTime2)
      {
        this.mpCurrentPeriod.OldMonth = 0;
        MonthPicker mpCurrentPeriod = this.mpCurrentPeriod;
        periodName = Options.Period.PeriodName;
        DateTime dateTime3 = periodName.Value;
        mpCurrentPeriod.Value = dateTime3;
      }
      this.Cursor = Cursors.Default;
    }

    private void tsmiPrintPolicy_Click(object sender, EventArgs e)
    {
      FrmPolicy frmPolicy = new FrmPolicy(this.VisibleProperty, this.currentRaion, this.currentCompany, this.currentHome, this.currentLs);
      int num = (int) frmPolicy.ShowDialog();
      frmPolicy.Dispose();
      if (!(this.mpCurrentPeriod.Value != Options.Period.PeriodName.Value))
        return;
      this.mpCurrentPeriod.OldMonth = 0;
      this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
    }

    private void tsmiUzp_Click(object sender, EventArgs e)
    {
      FrmUZP frmUzp = new FrmUZP();
      int num = (int) frmUzp.ShowDialog();
      frmUzp.Dispose();
      if (!(this.mpCurrentPeriod.Value != Options.Period.PeriodName.Value))
        return;
      this.mpCurrentPeriod.OldMonth = 0;
      this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
    }

    private void tsmiExternalData_Click(object sender, EventArgs e)
    {
      try
      {
        CallDll.View(0, Options.Period.PeriodName.Value.ToOADate());
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Невозможно осуществить доступ к внешним данным", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void tsmiAbout_Click(object sender, EventArgs e)
    {
      FrmAboutProgr frmAboutProgr = new FrmAboutProgr();
      int num = (int) frmAboutProgr.ShowDialog();
      frmAboutProgr.Dispose();
    }

    private void tsmiHelp_Click(object sender, EventArgs e)
    {
    }

    private void tsmiTariffs_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      FrmTariffs frmTariffs = new FrmTariffs(this.currentCompany);
      frmTariffs.Company_id = this.currentCompany.CompanyId;
      int num = (int) frmTariffs.ShowDialog();
      frmTariffs.Dispose();
      if (this.mpCurrentPeriod.Value != Options.Period.PeriodName.Value)
      {
        this.mpCurrentPeriod.OldMonth = 0;
        this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
      }
      this.Cursor = Cursors.Default;
    }

    private void tsmiFacilities_Click(object sender, EventArgs e)
    {
      FrmMSP frmMsp = new FrmMSP(this.currentCompany);
      int num = (int) frmMsp.ShowDialog();
      frmMsp.Dispose();
    }

    private void tsmiAbsence_Click(object sender, EventArgs e)
    {
      FrmAbsence frmAbsence = new FrmAbsence(this.currentCompany);
      int num = (int) frmAbsence.ShowDialog();
      frmAbsence.Dispose();
    }

    private void tsmiQuality_Click(object sender, EventArgs e)
    {
      FrmQuality frmQuality = new FrmQuality(this.currentCompany);
      frmQuality.Company_id = this.currentCompany.CompanyId;
      int num = (int) frmQuality.ShowDialog();
      frmQuality.Dispose();
    }

    private void tsmiSourcePayment_Click(object sender, EventArgs e)
    {
      FrmSourcePay frmSourcePay = new FrmSourcePay(this.currentCompany);
      int num = (int) frmSourcePay.ShowDialog();
      frmSourcePay.Dispose();
    }

    private void tsmiPurposePayment_Click(object sender, EventArgs e)
    {
      FrmPurposePay frmPurposePay = new FrmPurposePay(this.currentCompany);
      int num = (int) frmPurposePay.ShowDialog();
      frmPurposePay.Dispose();
    }

    private void tsmiParameters_Click(object sender, EventArgs e)
    {
      FrmParam frmParam = new FrmParam(this.currentCompany);
      frmParam.Company_id = this.currentCompany.CompanyId;
      int num = (int) frmParam.ShowDialog();
      frmParam.Dispose();
    }

    private void tsmiTypeDocuments_Click(object sender, EventArgs e)
    {
      FrmRightDoc frmRightDoc = new FrmRightDoc(this.currentCompany);
      int num = (int) frmRightDoc.ShowDialog();
      frmRightDoc.Dispose();
    }

    private void tsmiTypeCounters_Click(object sender, EventArgs e)
    {
      FrmTypeCounter frmTypeCounter = new FrmTypeCounter(this.currentCompany);
      int num = (int) frmTypeCounter.ShowDialog();
      frmTypeCounter.Dispose();
    }

    private void tsmiSuppliers_Click(object sender, EventArgs e)
    {
      FrmSupplier frmSupplier = new FrmSupplier(this.currentCompany);
      int num = (int) frmSupplier.ShowDialog();
      frmSupplier.Dispose();
    }

    private void tsmiDicReceipts_Click(object sender, EventArgs e)
    {
      FrmReceipt frmReceipt = new FrmReceipt(this.currentCompany);
      int num = (int) frmReceipt.ShowDialog();
      frmReceipt.Dispose();
    }

    private void tsmiServiceOrganizations_Click(object sender, EventArgs e)
    {
      FrmServiceParam frmServiceParam = new FrmServiceParam(this.currentCompany);
      frmServiceParam.Company_id = this.currentCompany.CompanyId;
      int num = (int) frmServiceParam.ShowDialog();
      frmServiceParam.Dispose();
    }

    private void tsmiDicServices_Click(object sender, EventArgs e)
    {
      FrmDi_PhonesServ frmDiPhonesServ = new FrmDi_PhonesServ(this.currentCompany);
      int num = (int) frmDiPhonesServ.ShowDialog();
      frmDiPhonesServ.Dispose();
    }

    private void tsmiBanks_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      FrmBank frmBank = new FrmBank(this.currentCompany);
      int num = (int) frmBank.ShowDialog();
      frmBank.Dispose();
      this.Cursor = Cursors.Default;
    }

    private void tsmiOrganizations_Click(object sender, EventArgs e)
    {
      FrmOrg frmOrg = new FrmOrg(this.currentCompany);
      int num = (int) frmOrg.ShowDialog();
      frmOrg.Dispose();
    }

    private void tsmiTypeLocationCounter_Click(object sender, EventArgs e)
    {
      FrmCoeffLocation frmCoeffLocation = new FrmCoeffLocation(this.currentCompany);
      int num = (int) frmCoeffLocation.ShowDialog();
      frmCoeffLocation.Dispose();
    }

    private void tsmiTypeSeals_Click(object sender, EventArgs e)
    {
      FrmTypeSeal frmTypeSeal = new FrmTypeSeal(this.currentCompany);
      int num = (int) frmTypeSeal.ShowDialog();
      frmTypeSeal.Dispose();
    }

    private void tsmiTypeBindingServices_Click(object sender, EventArgs e)
    {
    }

    private void tsmiBindingServices_Click(object sender, EventArgs e)
    {
      FrmCrossService frmCrossService = new FrmCrossService(this.currentCompany);
      int num = (int) frmCrossService.ShowDialog();
      frmCrossService.Dispose();
    }

    private void tsmiTypeNoteBook_Click(object sender, EventArgs e)
    {
      FrmTypeNoteBook frmTypeNoteBook = new FrmTypeNoteBook(this.currentCompany);
      int num = (int) frmTypeNoteBook.ShowDialog();
      frmTypeNoteBook.Dispose();
    }

    private void tsmiGuilds_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      FrmGuild frmGuild = new FrmGuild(this.currentCompany);
      int num = (int) frmGuild.ShowDialog();
      frmGuild.Dispose();
      this.Cursor = Cursors.Default;
    }

    private void tsmiContractOrganization_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      FrmContract frmContract = new FrmContract(this.currentCompany);
      int num = (int) frmContract.ShowDialog();
      frmContract.Dispose();
      this.Cursor = Cursors.Default;
    }

    private void tsmiSprAccounts_Click(object sender, EventArgs e)
    {
      bool readOnly = false;
      if (KvrplHelper.CheckProxy(83, 1, this.currentCompany, false) || KvrplHelper.CheckProxy(84, 1, this.currentCompany, false))
        readOnly = true;
      if (KvrplHelper.CheckProxy(83, 2, this.currentCompany, false) || KvrplHelper.CheckProxy(84, 2, this.currentCompany, false))
        readOnly = false;
      this.Cursor = Cursors.WaitCursor;
      FrmSprAccounts frmSprAccounts = new FrmSprAccounts(readOnly, this.currentCompany, -1);
      int num = (int) frmSprAccounts.ShowDialog();
      frmSprAccounts.Dispose();
      this.Cursor = Cursors.Default;
    }

    private void tsmiFastSearch_Click(object sender, EventArgs e)
    {
      FrmSearch frmSearch = new FrmSearch(0, this.currentCompany, (Home) null);
      int num = (int) frmSearch.ShowDialog();
      if (frmSearch.lsClient != null)
      {
        this.currentLs = frmSearch.lsClient;
        this.VisibleProperty = (short) 4;
        Options.Company = this.currentLs.Company;
        frmSearch.Dispose();
        this.LoadGrid();
      }
      else
      {
        if (frmSearch.company == null)
          return;
        this.currentCompany = frmSearch.company;
        Options.Company = this.currentCompany;
        this.VisibleProperty = (short) 2;
        this.currentRaion = this.currentCompany.Raion;
        frmSearch.Dispose();
        this.LoadGrid();
      }
    }

    private void tsmiFlatSearch_Click(object sender, EventArgs e)
    {
      if ((int) this.VisibleProperty != 3)
        return;
      FrmSearch frmSearch = new FrmSearch(2, this.currentCompany, this.currentHome);
      int num = (int) frmSearch.ShowDialog();
      if (frmSearch.lsClient != null)
      {
        this.currentLs = frmSearch.lsClient;
        this.VisibleProperty = (short) 4;
        frmSearch.Dispose();
        this.LoadGrid();
      }
      frmSearch.Dispose();
    }

    private void tsmiAdvancedSearch_Click(object sender, EventArgs e)
    {
      FrmBigSearch frmBigSearch = new FrmBigSearch();
      int num = (int) frmBigSearch.ShowDialog();
      if (frmBigSearch.client != null)
      {
        if ((frmBigSearch.client.Company == null ? Options.City : Convert.ToInt32(KvrplHelper.BaseValue(1, frmBigSearch.client.Company))) != 9)
        {
          this.currentLs = frmBigSearch.client;
          this.VisibleProperty = (short) 4;
          frmBigSearch.Dispose();
          this.LoadGrid();
        }
        else
        {
          this.currentLs = frmBigSearch.client;
          this.currentHome = this.session.Get<Home>((object) this.currentLs.Home.IdHome);
          this.currentCompany = this.session.Get<Company>((object) this.currentLs.Company.CompanyId);
          Options.Company = this.currentCompany;
          this.currentRaion = this.session.Get<Raion>((object) this.currentCompany.Raion.IdRnn);
          this.VisibleProperty = (short) 3;
          frmBigSearch.Dispose();
          this.LoadGrid();
        }
      }
      if (frmBigSearch.CurrentHome == null)
        return;
      this.currentHome = this.session.Get<Home>((object) frmBigSearch.CurrentHome.IdHome);
      this.currentCompany = this.session.Get<Company>((object) this.currentHome.Company.CompanyId);
      Options.Company = this.currentCompany;
      this.currentRaion = this.session.Get<Raion>((object) this.currentCompany.Raion.IdRnn);
      this.VisibleProperty = (short) 3;
      frmBigSearch.Dispose();
      this.LoadGrid();
    }

    private void tsmiStreetSearch_Click(object sender, EventArgs e)
    {
      if ((int) this.VisibleProperty != 2 || this.dgvMainList.Rows.Count == 0)
        return;
      FrmStrSearch frmStrSearch = new FrmStrSearch(this.dgvMainList);
      int num = (int) frmStrSearch.ShowDialog();
      frmStrSearch.Dispose();
    }

    private void tsmiContractSearch_Click(object sender, EventArgs e)
    {
      FrmSearchDog frmSearchDog = new FrmSearchDog();
      int num = (int) frmSearchDog.ShowDialog();
      if (frmSearchDog.Client != null)
      {
        this.currentLs = frmSearchDog.Client;
        this.VisibleProperty = (short) 4;
        this.LoadGrid();
      }
      frmSearchDog.Dispose();
    }

    private void tsmiPersonenNumberOrSnilsSearch_Click(object sender, EventArgs e)
    {
      FrmSearchTabNum frmSearchTabNum = new FrmSearchTabNum(Convert.ToInt16(((ToolStripItem) sender).Tag));
      int num = (int) frmSearchTabNum.ShowDialog();
      if (frmSearchTabNum.Client != null)
      {
        this.currentLs = frmSearchTabNum.Client;
        this.VisibleProperty = (short) 4;
        this.LoadGrid();
      }
      frmSearchTabNum.Dispose();
    }

    private void tsmiPrimaryDocCloseMonth_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(65, 1, this.currentCompany, true))
        return;
      string format1 = "Вы действительно хотите закрыть месяц {0} по вводу первичных документов?";
      DateTime dateTime1 = KvrplHelper.GetNextPeriod(KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.ComplexPrior.IdFk, 0)).PeriodName.Value;
      string str1 = dateTime1.ToString("MM/yyyy");
      if (MessageBox.Show(string.Format(format1, (object) str1), "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      bool flag1 = true;
      DateTime PeriodName = new DateTime(1, 1, 1);
      if (KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.ComplexPrior.IdFk, 0).PeriodId == -1)
      {
        string format2 = "{0}.{1}";
        dateTime1 = Options.Period.PeriodName.Value;
        // ISSUE: variable of a boxed type
        int month = dateTime1.Month;
        dateTime1 = Options.Period.PeriodName.Value;
        // ISSUE: variable of a boxed type
        int year = dateTime1.Year;
        string s_val = string.Format(format2, (object) month, (object) year);
        if (this.session.CreateQuery(string.Format("from CompanyPeriod where Company.CompanyId={0} and Complex.ComplexId={1}", (object) this.currentCompany.CompanyId, (object) 108)).List<CompanyPeriod>().Count == 0)
        {
          if (!InputBox.Query("Ввод закрытого месяца", "Значение в формате (ММ.ГГГГ) :", ref s_val))
            return;
          try
          {
            DateTime dateTime2 = new DateTime();
            PeriodName = Convert.ToDateTime(string.Format("01.{0}", (object) s_val));
            flag1 = false;
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Неправильный формат ввода даты!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            KvrplHelper.WriteLog(ex, (LsClient) null);
            return;
          }
        }
      }
      Period period1 = new Period();
      Period closedPeriod = this.closedPeriod;
      CompanyPeriod companyPeriod1 = new CompanyPeriod();
      companyPeriod1.Company = this.session.Get<Company>((object) this.currentCompany.CompanyId);
      companyPeriod1.Uname = Options.Login;
      companyPeriod1.Dedit = DateTime.Now;
      IList<CompanyPeriod> companyPeriodList = (IList<CompanyPeriod>) new List<CompanyPeriod>();
      companyPeriod1.Complex = this.session.Get<Complex>((object) 108);
      bool flag2 = this.session.CreateQuery(string.Format("from CompanyPeriod where Company.CompanyId={0} and Complex.IdFk={1}", (object) this.currentCompany.CompanyId, (object) 108)).List<CompanyPeriod>().Count > 0;
      CompanyPeriod companyPeriod2 = companyPeriod1;
      DateTime? periodName;
      Period period2;
      if (!(PeriodName != Convert.ToDateTime("01.01.0001")))
      {
        DateTime dateTime2 = KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.ComplexPrior.IdFk, 0).PeriodName.Value;
        periodName = closedPeriod.PeriodName;
        DateTime dateTime3 = periodName.Value;
        period2 = dateTime2 < dateTime3 ? this.session.Get<Period>((object) KvrplHelper.GetNextPeriod(KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.ComplexPrior.IdFk, 0)).PeriodId) : this.session.Get<Period>((object) KvrplHelper.GetNextPeriod(KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.ComplexPrior.IdFk, 0)).PeriodId);
      }
      else
        period2 = this.session.Get<Period>((object) KvrplHelper.GetPeriod(PeriodName).PeriodId);
      companyPeriod2.Period = period2;
      try
      {
        if (companyPeriod1.Period != null)
        {
          if (flag2)
            this.session.Update((object) companyPeriod1);
          else
            this.session.Save((object) companyPeriod1);
          this.session.Flush();
          System.Windows.Forms.Label lblPriorClosed = this.lblPriorClosed;
          periodName = companyPeriod1.Period.PeriodName;
          dateTime1 = periodName.Value;
          string str2 = dateTime1.ToString("MM/yyyy");
          lblPriorClosed.Text = str2;
        }
        else
        {
          int num = (int) MessageBox.Show("Не удалось осуществить предварительное закрытие, т.к. он превышает закрытый месяц по квартплате!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Не удалось осуществить предварительное закрытие", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void tsmiPrimaryDocOpenMonth_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(66, 1, this.currentCompany, true))
        return;
      string format = "Вы действительно хотите открыть месяц {0} по вводу первичных документов?";
      DateTime? periodName = KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.ComplexPrior.IdFk, 0).PeriodName;
      DateTime dateTime = periodName.Value;
      string str1 = dateTime.ToString("MM/yyyy");
      if (MessageBox.Show(string.Format(format, (object) str1), "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      CompanyPeriod companyPeriod = new CompanyPeriod();
      companyPeriod.Company = this.session.Get<Company>((object) this.currentCompany.CompanyId);
      companyPeriod.Complex = this.session.Get<Complex>((object) 108);
      companyPeriod.Period = KvrplHelper.GetPrevPeriod(KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.ComplexPrior.IdFk, 0));
      companyPeriod.Uname = Options.Login;
      companyPeriod.Dedit = DateTime.Now;
      IList<CompanyPeriod> companyPeriodList = (IList<CompanyPeriod>) new List<CompanyPeriod>();
      if (this.session.CreateQuery(string.Format("from CompanyPeriod where Company.CompanyId={0} and Complex.ComplexId={1}", (object) this.currentCompany.CompanyId, (object) -100)).List<CompanyPeriod>().Count > 0)
      {
        int num1 = (int) MessageBox.Show("Период закрыт по расщеплению платежей!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        try
        {
          this.session.Update((object) companyPeriod);
          this.session.Flush();
          System.Windows.Forms.Label lblPriorClosed = this.lblPriorClosed;
          periodName = companyPeriod.Period.PeriodName;
          dateTime = periodName.Value;
          string str2 = dateTime.ToString("MM/yyyy");
          lblPriorClosed.Text = str2;
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show("Не удалось открыть месяц!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
    }

    private void tsmiChargeCloseMonth_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(36, 1, this.currentCompany, true))
        return;
      string str1 = sender == null ? "Необходимо сначала закрыть месяц по начислениям! \n" : "";
      string format1 = "{1}Вы действительно хотите закрыть месяц {0} по начислениям?";
      DateTime? periodName = KvrplHelper.GetNextPeriod(KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.ComplexPasp.ComplexId, 0)).PeriodName;
      DateTime dateTime1 = periodName.Value;
      string str2 = dateTime1.ToString("MM/yyyy");
      string str3 = str1;
      if (MessageBox.Show(string.Format(format1, (object) str2, (object) str3), "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
      {
        bool flag1 = true;
        DateTime PeriodName = new DateTime(1, 1, 1);
        if (KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.ComplexPasp.ComplexId, 0).PeriodId == -1)
        {
          string format2 = "{0}.{1}";
          periodName = Options.Period.PeriodName;
          dateTime1 = periodName.Value;
          // ISSUE: variable of a boxed type
          int month = dateTime1.Month;
          periodName = Options.Period.PeriodName;
          dateTime1 = periodName.Value;
          // ISSUE: variable of a boxed type
          int year = dateTime1.Year;
          string s_val = string.Format(format2, (object) month, (object) year);
          if (this.session.CreateQuery(string.Format("from CompanyPeriod where Company.CompanyId={0} and Complex.ComplexId={1}", (object) this.currentCompany.CompanyId, (object) 102)).List<CompanyPeriod>().Count == 0)
          {
            if (!InputBox.Query("Ввод закрытого месяца", "Значение в формате (ММ.ГГГГ) :", ref s_val))
            {
              this.closedPasp = false;
              return;
            }
            try
            {
              DateTime dateTime2 = new DateTime();
              PeriodName = Convert.ToDateTime(string.Format("01.{0}", (object) s_val));
              flag1 = false;
            }
            catch (Exception ex)
            {
              int num = (int) MessageBox.Show("Неправильный формат ввода даты!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              KvrplHelper.WriteLog(ex, (LsClient) null);
              this.closedPasp = false;
              return;
            }
          }
        }
        Period period = new Period();
        period = this.closedPeriod;
        CompanyPeriod companyPeriod = new CompanyPeriod();
        companyPeriod.Company = this.session.Get<Company>((object) this.currentCompany.CompanyId);
        companyPeriod.Uname = Options.Login;
        companyPeriod.Dedit = DateTime.Now;
        IList<CompanyPeriod> companyPeriodList = (IList<CompanyPeriod>) new List<CompanyPeriod>();
        companyPeriod.Complex = this.session.Get<Complex>((object) 102);
        bool flag2 = this.session.CreateQuery(string.Format("from CompanyPeriod where Company.CompanyId={0} and Complex.ComplexId={1}", (object) this.currentCompany.CompanyId, (object) 102)).List<CompanyPeriod>().Count > 0;
        companyPeriod.Period = PeriodName != Convert.ToDateTime("01.01.0001") ? this.session.Get<Period>((object) KvrplHelper.GetPeriod(PeriodName).PeriodId) : this.session.Get<Period>((object) KvrplHelper.GetNextPeriod(KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.ComplexPasp.ComplexId, 0)).PeriodId);
        int city = Options.City;
        if (this.currentCompany != null)
        {
          try
          {
            if (Convert.ToInt32(KvrplHelper.BaseValue(1, Options.Company)) == 0)
              city = Options.City;
          }
          catch (Exception ex)
          {
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
        }
        if (!this.CheckInOut(companyPeriod.Period) || !this.CheckRent(companyPeriod.Period) || !this.CheckRentPast(companyPeriod.Period))
        {
          this.closedPasp = false;
        }
        else
        {
          try
          {
            if (flag2)
              this.session.Update((object) companyPeriod);
            else
              this.session.Save((object) companyPeriod);
            this.session.Flush();
            System.Windows.Forms.Label lblPaspClosed = this.lblPaspClosed;
            periodName = companyPeriod.Period.PeriodName;
            dateTime1 = periodName.Value;
            string str4 = dateTime1.ToString("MM/yyyy");
            lblPaspClosed.Text = str4;
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Не удалось закрыть месяц!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            KvrplHelper.WriteLog(ex, (LsClient) null);
            this.closedPasp = false;
            return;
          }
          this.closedPasp = true;
        }
      }
      else
        this.closedPasp = false;
    }

    private void tsmiChargeOpenMonth_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(36, 1, this.currentCompany, true) || MessageBox.Show(string.Format("Вы действительно хотите открыть месяц {0} по начислениям?", (object) this.lblPaspClosed.Text), "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      CompanyPeriod companyPeriod = new CompanyPeriod();
      companyPeriod.Company = this.session.Get<Company>((object) this.currentCompany.CompanyId);
      companyPeriod.Complex = this.session.Get<Complex>((object) 102);
      companyPeriod.Period = KvrplHelper.GetPrevPeriod(KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.ComplexPasp.ComplexId, 0));
      companyPeriod.Uname = Options.Login;
      companyPeriod.Dedit = DateTime.Now;
      IList<CompanyPeriod> companyPeriodList1 = (IList<CompanyPeriod>) new List<CompanyPeriod>();
      IList<CompanyPeriod> companyPeriodList2 = this.session.CreateQuery(string.Format("from CompanyPeriod where Company.CompanyId={0} and Complex.ComplexId={1}", (object) this.currentCompany.CompanyId, (object) 100)).List<CompanyPeriod>();
      if (companyPeriodList2.Count > 0 && (companyPeriod.Period != null && companyPeriodList2[0].Period.PeriodId > companyPeriod.Period.PeriodId))
      {
        int num1 = (int) MessageBox.Show("Необходимо сначала открыть месяц по оплате!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        try
        {
          this.session.Update((object) companyPeriod);
          this.session.Flush();
          this.lblPaspClosed.Text = companyPeriod.Period.PeriodName.Value.ToString("MM/yyyy");
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show("Не удалось открыть месяц по начислениям!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        try
        {
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show("Не удалось открыть месяц по предварительному закрытию!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
    }

    private void tsmiCalcPerPaymentCloseMonth_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(36, 1, this.currentCompany, true))
        return;
      string str1 = sender == null ? "Необходимо сначала закрыть месяц по оплате! \n" : "";
      string format1 = "{1}Вы действительно хотите закрыть месяц {0} по оплате?";
      DateTime? periodName = KvrplHelper.GetNextPeriod(KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.Complex.ComplexId, 0)).PeriodName;
      DateTime dateTime1 = periodName.Value;
      string str2 = dateTime1.ToString("MM/yyyy");
      string str3 = str1;
      if (MessageBox.Show(string.Format(format1, (object) str2, (object) str3), "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
      {
        bool flag1 = true;
        DateTime PeriodName = new DateTime(1, 1, 1);
        if (KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.Complex.ComplexId, 0).PeriodId == -1)
        {
          string format2 = "{0}.{1}";
          periodName = Options.Period.PeriodName;
          dateTime1 = periodName.Value;
          // ISSUE: variable of a boxed type
          int month = dateTime1.Month;
          periodName = Options.Period.PeriodName;
          dateTime1 = periodName.Value;
          // ISSUE: variable of a boxed type
          int year = dateTime1.Year;
          string s_val = string.Format(format2, (object) month, (object) year);
          if (this.session.CreateQuery(string.Format("from CompanyPeriod where Company.CompanyId={0} and Complex.ComplexId={1}", (object) this.currentCompany.CompanyId, (object) 100)).List<CompanyPeriod>().Count == 0)
          {
            if (!InputBox.Query("Ввод закрытого месяца", "Значение в формате (ММ.ГГГГ) :", ref s_val))
            {
              this.closedKvr = false;
              return;
            }
            try
            {
              DateTime dateTime2 = new DateTime();
              PeriodName = Convert.ToDateTime(string.Format("01.{0}", (object) s_val));
              flag1 = false;
            }
            catch
            {
              int num = (int) MessageBox.Show("Неправильный формат ввода даты!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.closedKvr = false;
              return;
            }
          }
        }
        Period period = new Period();
        period = this.closedPeriod;
        CompanyPeriod companyPeriod = new CompanyPeriod();
        companyPeriod.Company = this.session.Get<Company>((object) this.currentCompany.CompanyId);
        companyPeriod.Uname = Options.Login;
        companyPeriod.Dedit = DateTime.Now;
        IList<CompanyPeriod> companyPeriodList1 = (IList<CompanyPeriod>) new List<CompanyPeriod>();
        companyPeriod.Complex = this.session.Get<Complex>((object) 100);
        bool flag2 = this.session.CreateQuery(string.Format("from CompanyPeriod where Company.CompanyId={0} and Complex.ComplexId={1}", (object) this.currentCompany.CompanyId, (object) 100)).List<CompanyPeriod>().Count > 0;
        companyPeriod.Period = PeriodName != Convert.ToDateTime("01.01.0001") ? this.session.Get<Period>((object) KvrplHelper.GetPeriod(PeriodName).PeriodId) : this.session.Get<Period>((object) KvrplHelper.GetNextPeriod(KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.Complex.ComplexId, 0)).PeriodId);
        IList<CompanyPeriod> companyPeriodList2 = (IList<CompanyPeriod>) new List<CompanyPeriod>();
        IList<CompanyPeriod> companyPeriodList3 = this.session.CreateQuery(string.Format("from CompanyPeriod where Company.CompanyId={0} and Complex.ComplexId={1}", (object) this.currentCompany.CompanyId, (object) 102)).List<CompanyPeriod>();
        if (companyPeriodList3.Count > 0)
        {
          if (companyPeriod.Period != null && companyPeriodList3[0].Period.PeriodId < companyPeriod.Period.PeriodId)
          {
            this.closedPasp = true;
            this.tsmiChargeCloseMonth_Click((object) null, (EventArgs) null);
            if (!this.closedPasp)
            {
              int num = (int) MessageBox.Show("Закрытый месяц по оплате не может быть больше закрытого месяца по начислениям!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.closedKvr = false;
              return;
            }
          }
          if (!this.CheckPayment(companyPeriod.Period) || !this.CheckBalance(companyPeriod.Period))
          {
            this.closedKvr = false;
          }
          else
          {
            int num1 = Options.City;
            if (this.currentCompany != null)
            {
              try
              {
                num1 = Convert.ToInt32(KvrplHelper.BaseValue(1, Options.Company));
                if (num1 == 0)
                  num1 = Options.City;
              }
              catch (Exception ex)
              {
                KvrplHelper.WriteLog(ex, (LsClient) null);
              }
            }
            if (num1 == 3)
              this.session.CreateSQLQuery(string.Format("delete from lsRent where period_id={0} and code=0       and exists(select * from lsBalance where client_id=lsRent.client_id and service_id=lsRent.service_id and supplier_id=lsRent.supplier_id and service_id in (select service_id from dcService where service_id=26 or root=26) and balance_out>0); update lsBalance t set balance_out=balance_out-rent+isnull((select sum(rent) from lsRent where period_id={0} and client_id=t.client_id and service_id=t.service_id and supplier_id=t.supplier_id),0),      rent= isnull((select sum(rent) from lsRent where period_id={0} and client_id=t.client_id and service_id=t.service_id and supplier_id=t.supplier_id),0)where service_id in (select service_id from dcService where service_id=26 or root=26) and balance_out>0;delete from lsBalance where period_id={0} and balance_in=0 and rent=0 and rent_past=0 and msp=0 and msp_past=0 and payment=0 and payment_past=0 and subsidy=0 and balance_out=0 and rent_comp=0", (object) companyPeriod.Period.PeriodId)).ExecuteUpdate();
            try
            {
              if (flag2)
                this.session.Update((object) companyPeriod);
              else
                this.session.Save((object) companyPeriod);
              this.session.Flush();
              System.Windows.Forms.Label lblKvrCloseValue = this.lblKvrCloseValue;
              periodName = companyPeriod.Period.PeriodName;
              dateTime1 = periodName.Value;
              string str4 = dateTime1.ToString("MM/yyyy");
              lblKvrCloseValue.Text = str4;
            }
            catch (Exception ex)
            {
              int num2 = (int) MessageBox.Show("Не удалось закрыть месяц!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              KvrplHelper.WriteLog(ex, (LsClient) null);
              this.closedKvr = false;
            }
          }
        }
        else
        {
          int num3 = (int) MessageBox.Show("Закройте сначала месяц по начислениям!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
      else
        this.closedKvr = false;
    }

    private void tsmiCalcPerPaymentOpenMonth_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(36, 1, this.currentCompany, true) || MessageBox.Show(string.Format("Вы действительно хотите открыть месяц {0} по оплате?", (object) this.lblKvrCloseValue.Text), "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      CompanyPeriod companyPeriod = new CompanyPeriod();
      companyPeriod.Company = this.session.Get<Company>((object) this.currentCompany.CompanyId);
      companyPeriod.Complex = Options.Complex;
      companyPeriod.Period = KvrplHelper.GetPrevPeriod(KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.Complex.ComplexId, 0));
      companyPeriod.Uname = Options.Login;
      companyPeriod.Dedit = DateTime.Now;
      string connectionString = string.Format("Provider={4};Eng={0};Uid={1};Pwd={2}; Links={3}", (object) Options.BaseName, (object) Options.Login, (object) Options.Pwd, (object) "tcpip{}", (object) Options.Provider);
      IList<CompanyPeriod> companyPeriodList1 = (IList<CompanyPeriod>) new List<CompanyPeriod>();
      IList<CompanyPeriod> companyPeriodList2 = this.session.CreateQuery(string.Format("from CompanyPeriod where Company.CompanyId={0} and Complex.ComplexId={1}", (object) this.currentCompany.CompanyId, (object) 101)).List<CompanyPeriod>();
      if (companyPeriodList2.Count > 0 && (companyPeriod.Period != null && companyPeriodList2[0].Period.PeriodId > companyPeriod.Period.PeriodId))
      {
        int num1 = (int) MessageBox.Show("Необходимо сначала открыть месяц по пеням!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        IList<CompanyPeriod> companyPeriodList3 = (IList<CompanyPeriod>) new List<CompanyPeriod>();
        if (this.session.CreateQuery(string.Format("from CompanyPeriod where Company.CompanyId={0} and Complex.ComplexId={1}", (object) this.currentCompany.CompanyId, (object) -100)).List<CompanyPeriod>().Count > 0)
        {
          int num2 = (int) MessageBox.Show("Период закрыт по расщеплению платежей!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          if (Options.City == 3 && MessageBox.Show("Страховка уже просчитана. Открытие месяца может привести к ошибкам в расчете страховки. Вы действительно хотите открыть месяц?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            return;
          try
          {
            this.session.Update((object) companyPeriod);
            this.session.Flush();
            this.closedPeriod = companyPeriod.Period;
            this.lblKvrCloseValue.Text = companyPeriod.Period.PeriodName.Value.ToString("MM/yyyy");
            try
            {
              OleDbHelper.ExecuteNonQuery(connectionString, CommandType.Text, string.Format("update manager set monthclosed='{0}' where codeu={1}", (object) KvrplHelper.DateToBaseFormat(companyPeriod.Period.PeriodName.Value), (object) this.currentCompany.CompanyId), 1000);
            }
            catch (Exception ex)
            {
              KvrplHelper.WriteLog(ex, (LsClient) null);
            }
          }
          catch (Exception ex)
          {
            int num3 = (int) MessageBox.Show("Не удалось открыть месяц по квартплате!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
          try
          {
          }
          catch (Exception ex)
          {
            int num3 = (int) MessageBox.Show("Не удалось открыть месяц по предварительному закрытию", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
        }
      }
    }

    private void tsmiPeniCloseMonth_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(36, 1, this.currentCompany, true))
        return;
      string format1 = "Вы действительно хотите закрыть месяц {0} по пеням?";
      DateTime? periodName = KvrplHelper.GetNextPeriod(KvrplHelper.GetCmpKvrClose(this.currentCompany, 101, 0)).PeriodName;
      DateTime dateTime1 = periodName.Value;
      string str1 = dateTime1.ToString("MM/yyyy");
      if (MessageBox.Show(string.Format(format1, (object) str1), "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      bool flag1 = true;
      DateTime PeriodName = new DateTime(1, 1, 1);
      if (KvrplHelper.GetCmpKvrClose(this.currentCompany, 101, 0).PeriodId == -1)
      {
        string format2 = "{0}.{1}";
        periodName = Options.Period.PeriodName;
        dateTime1 = periodName.Value;
        // ISSUE: variable of a boxed type
        int month = dateTime1.Month;
        periodName = Options.Period.PeriodName;
        dateTime1 = periodName.Value;
        // ISSUE: variable of a boxed type
        int year = dateTime1.Year;
        string s_val = string.Format(format2, (object) month, (object) year);
        if (this.session.CreateQuery(string.Format("from CompanyPeriod where Company.CompanyId={0} and Complex.ComplexId={1}", (object) this.currentCompany.CompanyId, (object) 101)).List<CompanyPeriod>().Count == 0)
        {
          if (!InputBox.Query("Ввод закрытого месяца", "Значение в формате (ММ.ГГГГ) :", ref s_val))
            return;
          try
          {
            DateTime dateTime2 = new DateTime();
            PeriodName = Convert.ToDateTime(string.Format("01.{0}", (object) s_val));
            flag1 = false;
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Неправильный формат ввода даты!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            KvrplHelper.WriteLog(ex, (LsClient) null);
            return;
          }
        }
      }
      Period period = new Period();
      period = this.closedPeriod;
      CompanyPeriod companyPeriod1 = new CompanyPeriod();
      companyPeriod1.Company = this.session.Get<Company>((object) this.currentCompany.CompanyId);
      companyPeriod1.Uname = Options.Login;
      companyPeriod1.Dedit = DateTime.Now;
      IList<CompanyPeriod> companyPeriodList1 = (IList<CompanyPeriod>) new List<CompanyPeriod>();
      companyPeriod1.Complex = this.session.Get<Complex>((object) 101);
      bool flag2 = this.session.CreateQuery(string.Format("from CompanyPeriod where Company.CompanyId={0} and Complex.ComplexId={1}", (object) this.currentCompany.CompanyId, (object) 101)).List<CompanyPeriod>().Count > 0;
      companyPeriod1.Period = PeriodName != Convert.ToDateTime("01.01.0001") ? this.session.Get<Period>((object) KvrplHelper.GetPeriod(PeriodName).PeriodId) : this.session.Get<Period>((object) KvrplHelper.GetNextPeriod(KvrplHelper.GetCmpKvrClose(this.currentCompany, 101, 0)).PeriodId);
      IList<CompanyPeriod> companyPeriodList2 = (IList<CompanyPeriod>) new List<CompanyPeriod>();
      IList<CompanyPeriod> companyPeriodList3 = this.session.CreateQuery(string.Format("from CompanyPeriod where Company.CompanyId={0} and Complex.ComplexId={1}", (object) this.currentCompany.CompanyId, (object) 100)).List<CompanyPeriod>();
      if (companyPeriodList3.Count > 0)
      {
        if (companyPeriod1.Period != null && companyPeriodList3[0].Period.PeriodId < companyPeriod1.Period.PeriodId)
        {
          this.closedKvr = true;
          this.tsmiCalcPerPaymentCloseMonth_Click((object) null, (EventArgs) null);
          if (!this.closedKvr)
          {
            int num = (int) MessageBox.Show("Закрытый месяц по пеням не может быть больше закрытого месяца по оплате!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
        }
        if (!this.CheckPeniRent(companyPeriod1.Period) || !this.CheckPeniPay(companyPeriod1.Period) || !this.CheckPeniBalance(companyPeriod1.Period))
          return;
        try
        {
          if (flag2)
            this.session.Update((object) companyPeriod1);
          else
            this.session.Save((object) companyPeriod1);
          this.session.Flush();
          System.Windows.Forms.Label lblPeniClosed = this.lblPeniClosed;
          periodName = companyPeriod1.Period.PeriodName;
          dateTime1 = periodName.Value;
          string str2 = dateTime1.ToString("MM/yyyy");
          lblPeniClosed.Text = str2;
          this.session.Clear();
          IList<CompanyPeriod> companyPeriodList4 = this.session.CreateCriteria(typeof (CompanyPeriod)).Add((ICriterion) Restrictions.Eq("Company", (object) this.currentCompany)).Add((ICriterion) Restrictions.Eq("Complex", (object) Options.ComplexPrior)).List<CompanyPeriod>();
          if (companyPeriodList4.Count > 0)
          {
            if (companyPeriodList4[0].Period.PeriodId != companyPeriod1.Period.PeriodId)
            {
              companyPeriodList4[0].Period = companyPeriod1.Period;
              this.session.Update((object) companyPeriodList4[0]);
            }
          }
          else
          {
            CompanyPeriod companyPeriod2 = new CompanyPeriod();
            CompanyPeriod companyPeriod3 = companyPeriod1;
            companyPeriod3.Complex = Options.ComplexPrior;
            this.session.Save((object) companyPeriod3);
          }
          this.session.Flush();
          System.Windows.Forms.Label lblPriorClosed = this.lblPriorClosed;
          periodName = companyPeriod1.Period.PeriodName;
          dateTime1 = periodName.Value;
          string str3 = dateTime1.ToString("MM/yyyy");
          lblPriorClosed.Text = str3;
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Не удалось закрыть месяц!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      else
      {
        int num1 = (int) MessageBox.Show("Закройте сначала месяц по оплате!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void tsmiPeniOpenMonth_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(36, 1, this.currentCompany, true) || MessageBox.Show(string.Format("Вы действительно хотите открыть месяц {0} по пеням?", (object) this.lblPeniClosed.Text), "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      CompanyPeriod companyPeriod = new CompanyPeriod();
      companyPeriod.Company = this.session.Get<Company>((object) this.currentCompany.CompanyId);
      companyPeriod.Complex = this.session.Get<Complex>((object) 101);
      companyPeriod.Period = KvrplHelper.GetPrevPeriod(KvrplHelper.GetCmpKvrClose(this.currentCompany, 101, 0));
      companyPeriod.Uname = Options.Login;
      companyPeriod.Dedit = DateTime.Now;
      IList<CompanyPeriod> companyPeriodList = (IList<CompanyPeriod>) new List<CompanyPeriod>();
      if (this.session.CreateQuery(string.Format("from CompanyPeriod where Company.CompanyId={0} and Complex.ComplexId={1}", (object) this.currentCompany.CompanyId, (object) -100)).List<CompanyPeriod>().Count > 0)
      {
        int num1 = (int) MessageBox.Show("Период закрыт по расщеплению платежей!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        try
        {
          this.session.Update((object) companyPeriod);
          this.session.Flush();
          this.lblPeniClosed.Text = companyPeriod.Period.PeriodName.Value.ToString("MM/yyyy");
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show("Не удалось открыть месяц по пеням!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
    }

    private void tsmiSocialProtectionCloseMonth_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(36, 1, this.currentCompany, true))
        return;
      string format1 = "Вы действительно хотите закрыть месяц {0} по соцзащите?";
      DateTime? periodName = KvrplHelper.GetNextPeriod(KvrplHelper.GetCmpKvrClose(this.currentCompany, 104, 0)).PeriodName;
      DateTime dateTime1 = periodName.Value;
      string str1 = dateTime1.ToString("MM/yyyy");
      if (MessageBox.Show(string.Format(format1, (object) str1), "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      bool flag1 = true;
      DateTime PeriodName = new DateTime(1, 1, 1);
      if (KvrplHelper.GetCmpKvrClose(this.currentCompany, 104, 0).PeriodId == -1)
      {
        string format2 = "{0}.{1}";
        periodName = Options.Period.PeriodName;
        dateTime1 = periodName.Value;
        // ISSUE: variable of a boxed type
        int month = dateTime1.Month;
        periodName = Options.Period.PeriodName;
        dateTime1 = periodName.Value;
        // ISSUE: variable of a boxed type
        int year = dateTime1.Year;
        string s_val = string.Format(format2, (object) month, (object) year);
        if (this.session.CreateQuery(string.Format("from CompanyPeriod where Company.CompanyId={0} and Complex.ComplexId={1}", (object) this.currentCompany.CompanyId, (object) 104)).List<CompanyPeriod>().Count == 0)
        {
          if (!InputBox.Query("Ввод закрытого месяца", "Значение в формате (ММ.ГГГГ) :", ref s_val))
            return;
          try
          {
            DateTime dateTime2 = new DateTime();
            PeriodName = Convert.ToDateTime(string.Format("01.{0}", (object) s_val));
            flag1 = false;
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Неправильный формат ввода даты!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            KvrplHelper.WriteLog(ex, (LsClient) null);
            return;
          }
        }
      }
      Period period1 = new Period();
      Period closedPeriod = this.closedPeriod;
      CompanyPeriod companyPeriod1 = new CompanyPeriod();
      companyPeriod1.Company = this.session.Get<Company>((object) this.currentCompany.CompanyId);
      companyPeriod1.Uname = Options.Login;
      companyPeriod1.Dedit = DateTime.Now;
      IList<CompanyPeriod> companyPeriodList = (IList<CompanyPeriod>) new List<CompanyPeriod>();
      companyPeriod1.Complex = this.session.Get<Complex>((object) 104);
      bool flag2 = this.session.CreateQuery(string.Format("from CompanyPeriod where Company.CompanyId={0} and Complex.ComplexId={1}", (object) this.currentCompany.CompanyId, (object) 104)).List<CompanyPeriod>().Count > 0;
      CompanyPeriod companyPeriod2 = companyPeriod1;
      Period period2;
      if (!(PeriodName != Convert.ToDateTime("01.01.0001")))
      {
        periodName = KvrplHelper.GetCmpKvrClose(this.currentCompany, 104, 0).PeriodName;
        DateTime dateTime2 = periodName.Value;
        periodName = closedPeriod.PeriodName;
        DateTime dateTime3 = periodName.Value;
        period2 = dateTime2 < dateTime3 ? this.session.Get<Period>((object) KvrplHelper.GetNextPeriod(KvrplHelper.GetCmpKvrClose(this.currentCompany, 104, 0)).PeriodId) : this.session.Get<Period>((object) KvrplHelper.GetNextPeriod(KvrplHelper.GetCmpKvrClose(this.currentCompany, 104, 0)).PeriodId);
      }
      else
        period2 = this.session.Get<Period>((object) KvrplHelper.GetPeriod(PeriodName).PeriodId);
      companyPeriod2.Period = period2;
      string str2 = string.Format("Provider={4};Eng={0};Uid={1};Pwd={2}; Links={3}", (object) Options.BaseName, (object) Options.Login, (object) Options.Pwd, (object) "tcpip{}", (object) Options.Provider);
      string str3 = Options.ConfigValue("soc_saldo");
      int num1 = Options.City;
      if (this.currentCompany != null)
      {
        try
        {
          num1 = Convert.ToInt32(KvrplHelper.BaseValue(1, Options.Company));
          if (num1 == 0)
            num1 = Options.City;
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      if (num1 == 2 || num1 == 23 || num1 == 24)
      {
        string str4 = "";
        if (num1 == 23)
        {
          string format2 = " and (select sum(pay) from soc_saldo where period=v.period and idlic=v.idlic and idpers=v.idpers and codehelp=v.codehelp)=0  and (select sum(outcoming) from soc_saldo where period=v.period and idlic=v.idlic and idpers=v.idpers and codehelp=v.codehelp)<>0  and idform not in (select idform_msp from lsMspGKU where period_id=0 and client_id=v.idlic and cast('{0}' as date) between dbeg and dend and msp_id=codehelp) ";
          periodName = companyPeriod1.Period.PeriodName;
          string baseFormat = KvrplHelper.DateToBaseFormat(periodName.Value);
          str4 = string.Format(format2, (object) baseFormat);
        }
        ISession session = this.session;
        string format3 = "select distinct v.idlic, (if " + Options.TypeFio.ToString() + "<>0 then (select family from frPers where code=1 and id=v.idpers)||' '||substr((select name from frPers where code=1 and id=v.idpers),1,1)||'. '||substr((select lastname from frPers where code=1 and id=v.idpers),1,1)||'.' else cast(family||' '||substr(name,1,1)||'. '||substr(lastname,1,1)||'.' as char(60)) endif) as fio,codehelp,firstpropdate,outtodate,diedate from soc_saldo v,form_a f where v.idpers=f.idform and period=cast('{0}' as date) and codehelp in (select codehelp from soc_cat) and not exists(select * from soc_file f,soc_zac z where f.id=z.idfile and f.period=v.period and z.iddd1=v.idpers and iddd2 in (select msp_id from dcMSP where codesoc = (select codesoc from dcMSP where msp_id=v.codehelp)))" + str4;
        periodName = companyPeriod1.Period.PeriodName;
        string baseFormat1 = KvrplHelper.DateToBaseFormat(periodName.Value);
        string queryString = string.Format(format3, (object) baseFormat1);
        IList list = session.CreateSQLQuery(queryString).List();
        if (list.Count > 0)
        {
          KvrplHelper.ViewErrorLic(list, "Обнаружено " + list.Count.ToString() + " льготников, отсутствующих в файле обмена.\n Показать льготников?", "Льготники, отсутствующие в файле обмена", (short) 2, this.sfd);
          if (MessageBox.Show("Продолжить проверку?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            return;
        }
      }
      if (str3 == "true" && (!this.CheckSocRent(companyPeriod1.Period) || !this.CheckSocBalance(companyPeriod1.Period)))
        return;
      try
      {
        if (companyPeriod1.Period != null)
        {
          if (flag2)
            this.session.Update((object) companyPeriod1);
          else
            this.session.Save((object) companyPeriod1);
          this.session.Flush();
          System.Windows.Forms.Label lblOsznClosed = this.lblOsznClosed;
          periodName = companyPeriod1.Period.PeriodName;
          dateTime1 = periodName.Value;
          string str4 = dateTime1.ToString("MM/yyyy");
          lblOsznClosed.Text = str4;
          try
          {
            string connectionString = str2;
            int num2 = 1;
            string format2 = "update manager set socclosed='{0}' where codeu={1}";
            periodName = companyPeriod1.Period.PeriodName;
            string baseFormat = KvrplHelper.DateToBaseFormat(periodName.Value);
            // ISSUE: variable of a boxed type
            short companyId = this.currentCompany.CompanyId;
            string commandText = string.Format(format2, (object) baseFormat, (object) companyId);
            int executeTimeout = 1000;
            OleDbHelper.ExecuteNonQuery(connectionString, (CommandType) num2, commandText, executeTimeout);
          }
          catch (Exception ex)
          {
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
        }
        else
        {
          int num3 = (int) MessageBox.Show("Не удалось закрыть месяц по соцзащите, т.к. он превышает закрытый месяц по квартплате!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
      catch (Exception ex)
      {
        int num2 = (int) MessageBox.Show("Не удалось закрыть месяц!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void tsmiSocialProtectionOpenMonth_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(36, 1, this.currentCompany, true) || MessageBox.Show(string.Format("Вы действительно хотите открыть месяц {0} по соцзащите?", (object) this.lblOsznClosed.Text), "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      CompanyPeriod companyPeriod = new CompanyPeriod();
      companyPeriod.Company = this.session.Get<Company>((object) this.currentCompany.CompanyId);
      companyPeriod.Complex = this.session.Get<Complex>((object) 104);
      companyPeriod.Period = KvrplHelper.GetPrevPeriod(KvrplHelper.GetCmpKvrClose(this.currentCompany, 104, 0));
      companyPeriod.Uname = Options.Login;
      companyPeriod.Dedit = DateTime.Now;
      IList<CompanyPeriod> companyPeriodList = (IList<CompanyPeriod>) new List<CompanyPeriod>();
      if (this.session.CreateQuery(string.Format("from CompanyPeriod where Company.CompanyId={0} and Complex.ComplexId={1}", (object) this.currentCompany.CompanyId, (object) -100)).List<CompanyPeriod>().Count > 0)
      {
        int num1 = (int) MessageBox.Show("Период закрыт по расщеплению платежей!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        string connectionString = string.Format("Provider={4};Eng={0};Uid={1};Pwd={2}; Links={3}", (object) Options.BaseName, (object) Options.Login, (object) Options.Pwd, (object) "tcpip{}", (object) Options.Provider);
        try
        {
          this.session.Update((object) companyPeriod);
          this.session.Flush();
          this.lblOsznClosed.Text = companyPeriod.Period.PeriodName.Value.ToString("MM/yyyy");
          try
          {
            OleDbHelper.ExecuteNonQuery(connectionString, CommandType.Text, string.Format("update manager set socclosed='{0}' where codeu={1}", (object) KvrplHelper.DateToBaseFormat(companyPeriod.Period.PeriodName.Value), (object) this.currentCompany.CompanyId), 1000);
          }
          catch (Exception ex)
          {
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show("Не удалось открыть месяц по соцзащите!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
    }

    private void tsmiRecvisitsAndParameters_Click(object sender, EventArgs e)
    {
      FrmRecviz frmRecviz = new FrmRecviz(this.currentCompany);
      int num = (int) frmRecviz.ShowDialog();
      frmRecviz.Dispose();
      if (!(this.mpCurrentPeriod.Value != Options.Period.PeriodName.Value))
        return;
      this.mpCurrentPeriod.OldMonth = 0;
      this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
    }

    private void tsmiOperations_Click(object sender, EventArgs e)
    {
      FrmMassChooseObject massChooseObject = new FrmMassChooseObject();
      massChooseObject.CurrentCompany = this.currentCompany;
      massChooseObject.MonthClosed = KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk);
      int num = (int) massChooseObject.ShowDialog();
      massChooseObject.Dispose();
    }

    private void tsmiNoteBook_Click(object sender, EventArgs e)
    {
      new FrmNoteBook(this.currentCompany, (LsClient) null).Show();
    }

    private void tsmiEnterTariffs_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 1, this.currentCompany, true))
        return;
      new FrmNewHomesTariff(this.currentCompany).Show();
    }

    private void tsmiInspectionContracts_Click(object sender, EventArgs e)
    {
      string str = "";
      if (this.currentRaion != null)
        str = str + str + string.Format(" and cmp.Raion.IdRnn={0}", (object) this.currentRaion.IdRnn);
      if (this.currentCompany != null)
        str = str + str + string.Format(" and cmp.CompanyId={0}", (object) this.currentCompany.CompanyId);
      if (this.currentHome != null)
        str = str + str + string.Format(" and c.Home.IdHome={0}", (object) this.currentHome.IdHome);
      IList<LsArenda> lsArendaList = this.session.CreateQuery(string.Format("select a from LsArenda a left outer join a.LsClient c left outer join c.Company cmp where 1=1 " + str + " and a.DEnd>=today() and a.DEnd<months(today(),2)", (object) KvrplHelper.DateToBaseFormat(this.closedPeriod.PeriodName.Value.AddMonths(1)), (object) KvrplHelper.DateToBaseFormat(this.closedPeriod.PeriodName.Value.AddMonths(3)))).List<LsArenda>();
      if (lsArendaList.Count > 0)
      {
        KvrplHelper.ViewErrorLic((IList) lsArendaList, "Найдены договоры, требующие перезаключения", "Найдены договоры, требующие перезаключения", (short) 4, this.sfd);
      }
      else
      {
        int num = (int) MessageBox.Show("Договоры, требующие перезаключения, не найдены", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.None);
      }
    }

    private void tsmiHomeParametrs_Click(object sender, EventArgs e)
    {
      FrmHomeParam frmHomeParam = new FrmHomeParam(this.currentHome, this.currentCompany);
      int num = (int) frmHomeParam.ShowDialog();
      frmHomeParam.Dispose();
      if (!(this.mpCurrentPeriod.Value != Options.Period.PeriodName.Value))
        return;
      this.mpCurrentPeriod.OldMonth = 0;
      this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
    }

    private void tsmiMakeEntrance_Click(object sender, EventArgs e)
    {
      FrmMakeEntrance frmMakeEntrance = new FrmMakeEntrance(this.currentCompany, this.currentHome);
      int num = (int) frmMakeEntrance.ShowDialog();
      frmMakeEntrance.Dispose();
    }

    private void tsmiNewLsClient_Click(object sender, EventArgs e)
    {
      if (!Options.Kvartplata)
        KvrplHelper.NewLsClient(this.currentRaion, this.currentCompany, this.currentHome, Options.ComplexArenda);
      if (!Options.Arenda)
        KvrplHelper.NewLsClient(this.currentRaion, this.currentCompany, this.currentHome, Options.Complex);
      this.LoadGrid();
    }

    private void tsmiCreateNewLsClient_Click(object sender, EventArgs e)
    {
      KvrplHelper.NewLsClient(this.currentRaion, this.currentCompany, this.currentHome, this.session.Get<Complex>((object) Convert.ToInt32(((ToolStripItem) sender).Tag)));
      this.LoadGrid();
    }

    private void tsmiLsClientClose_Click(object sender, EventArgs e)
    {
      FrmChooseObject frmChooseObject = new FrmChooseObject(((LsClient) this.dgvMainList.Rows[0].DataBoundItem).ClientId, (ClientParam) null);
      frmChooseObject.Save = false;
      frmChooseObject.CodeOperation = (short) 4;
      int num = (int) frmChooseObject.ShowDialog();
      frmChooseObject.Dispose();
    }

    private void tsmiLoadFacilities_Click(object sender, EventArgs e)
    {
      string str1 = "Закачать суммы по льготам в ";
      int num1 = Options.Period.PeriodName.Value.Month;
      string str2 = num1.ToString();
      string str3 = ".";
      num1 = Options.Period.PeriodName.Value.Year;
      string str4 = num1.ToString();
      if (MessageBox.Show(str1 + str2 + str3 + str4, "Закачка льгот", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK || this.fdHelpLoad.ShowDialog() != DialogResult.OK)
        return;
      string fileName = this.fdHelpLoad.FileName;
      string str5 = fileName.Remove(0, fileName.LastIndexOf("\\") + 1);
      string str6 = fileName.Remove(fileName.LastIndexOf("\\"), fileName.Length - fileName.LastIndexOf("\\"));
      string connectionString1 = string.Format("Provider={4};Eng={0};Uid={1};Pwd={2}; Links={3}", (object) Options.BaseName, (object) Options.Login, (object) Options.Pwd, (object) "tcpip{}", (object) Options.Provider);
      try
      {
        DataSet dataSet = OleDbHelper.ExecuteDataset(new OleDbConnection(string.Format("Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; Extended Properties=dBASE III", (object) str6)), CommandType.Text, "select * from " + str5, 10000);
        try
        {
          OleDbHelper.ExecuteNonQuery(connectionString1, CommandType.Text, "drop table tmpcomphelp", 1000);
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        OleDbHelper.ExecuteNonQuery(connectionString1, CommandType.Text, "create table tmpcomphelp(help numeric(15,2),llchet integer) ", 1000);
        OleDbHelper.ExecuteNonQuery(connectionString1, CommandType.Text, "delete from tmpcomphelp ", 1000);
        if ((uint) dataSet.Tables[0].Rows.Count > 0U)
        {
          foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          {
            OleDbCommand oleDbCommand = new OleDbCommand();
            oleDbCommand.Connection = new OleDbConnection(connectionString1);
            oleDbCommand.CommandType = CommandType.Text;
            oleDbCommand.CommandText = "insert into tmpcomphelp values(:help,:lic)";
            oleDbCommand.Parameters.Add("help", OleDbType.Currency).Value = row["help"];
            oleDbCommand.Parameters.Add("lic", OleDbType.Integer).Value = row["llchet"];
            oleDbCommand.Connection.Open();
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.Connection.Close();
          }
        }
        OleDbHelper.ExecuteNonQuery(connectionString1, CommandType.Text, "delete from tmpcomphelp where isnull(llchet,0) not in(select client_id from lsclient)", 1000);
        string connectionString2 = connectionString1;
        int num2 = 1;
        string format1 = "delete from jilsub where period = '{0}' and codedoc=2010 and subdate<>'{0}'";
        DateTime? periodName = Options.Period.PeriodName;
        string baseFormat1 = KvrplHelper.DateToBaseFormat(periodName.Value);
        string commandText1 = string.Format(format1, (object) baseFormat1);
        int executeTimeout1 = 1000;
        OleDbHelper.ExecuteNonQuery(connectionString2, (CommandType) num2, commandText1, executeTimeout1);
        string connectionString3 = connectionString1;
        int num3 = 1;
        string format2 = "insert into jilsub select '{0}',llchet,sum(help) as subsum,2010,'{1}',user,today(),0 from tmpcomphelp group by llchet";
        periodName = Options.Period.PeriodName;
        string baseFormat2 = KvrplHelper.DateToBaseFormat(periodName.Value);
        periodName = Options.Period.PeriodName;
        string baseFormat3 = KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(periodName.Value));
        string commandText2 = string.Format(format2, (object) baseFormat2, (object) baseFormat3);
        int executeTimeout2 = 10000;
        OleDbHelper.ExecuteNonQuery(connectionString3, (CommandType) num3, commandText2, executeTimeout2);
        OleDbHelper.ExecuteNonQuery(connectionString1, CommandType.Text, "drop table tmpcomphelp", 1000);
        int num4 = (int) MessageBox.Show("Закачка прошла успешно", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      catch (Exception ex1)
      {
        int num2 = (int) MessageBox.Show("Закачка прошла c ошибками", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        KvrplHelper.WriteLog(ex1, (LsClient) null);
        try
        {
          OleDbHelper.ExecuteNonQuery(connectionString1, CommandType.Text, string.Format("delete from jilsub where period = '{0}' and codedoc=2010 and subdate<>'{0}'", (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value)), 1000);
          OleDbHelper.ExecuteNonQuery(connectionString1, CommandType.Text, "drop table tmpcomphelp", 1000);
        }
        catch (Exception ex2)
        {
          KvrplHelper.WriteLog(ex2, (LsClient) null);
        }
      }
    }

    private void tsmiInsuranceUnLoad_Click(object sender, EventArgs e)
    {
      string str1 = "Выгрузить суммы по страховке за ";
      int num1 = Options.Period.PeriodName.Value.Month;
      string str2 = num1.ToString();
      string str3 = ".";
      num1 = Options.Period.PeriodName.Value.Year;
      string str4 = num1.ToString();
      if (MessageBox.Show(str1 + str2 + str3 + str4, "Выгрузка", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      if (this.sfdStrahUnLoad.ShowDialog() != DialogResult.OK)
        return;
      try
      {
        string fileName = this.sfdStrahUnLoad.FileName;
        string str5 = fileName.Remove(0, fileName.LastIndexOf("\\") + 1);
        string str6 = str5.Remove(str5.LastIndexOf("."));
        string str7 = fileName.Remove(fileName.LastIndexOf("\\"), fileName.Length - fileName.LastIndexOf("\\"));
        string connectionString = string.Format("Provider={4};Eng={0};Uid={1};Pwd={2}; Links={3}", (object) Options.BaseName, (object) Options.Login, (object) Options.Pwd, (object) "tcpip{}", (object) Options.Provider);
        string commandText = string.Format("select cast(str as char(40)) as STREET,cast(home as char(12)) as DM,       cast(home_korp as char(6)) as KR, cast(h.liter as char(6)) as ST,       cast( (select nflat from flats where idflat=lc.idflat) as char(10)) as FLAT,       cast((if isnull(p.param_value,0)=1 then 0 else 1 endif) as char(1)) as COM,       cast(if isnull(p.param_value,0)=2 then (select param_value from lsParam where client_id=lc.client_id and param_id=1 and dbeg<='{0}' and dend>='{0}')           else (select param_value from lsParam where client_id=lc.client_id and param_id=2 and dbeg<='{0}' and dend>='{0}') endif  as numeric(6,2)) as OPL,       cast (payment_value as numeric(7,2)) as PAY,payment_date as DATA,cast(lp.client_id as char(10)) as PCNT,      (if exists(select idform from DBA.form_a where idlic=lc.client_id and rodstv=1 and typepropis in (1,2) and firstpropdate <='{2}' and (archive in (0,5) or (archive in (1,2) and isnull(outtodate,'2999-12-31')>'{2}' and isnull(diedate,'2999-12-31')>'{2}')) ) then      (select first family||' '||name||' '||lastname from DBA.form_a where idlic=lc.client_id and rodstv=1 and typepropis in (1,2) and firstpropdate <='{2}' and (archive in (0,5) or (archive in (1,2) and isnull(outtodate,'2999-12-31')>'{2}' and isnull(diedate,'2999-12-31')>'{2}')) order by 1)       else (if exists(select owner from DBA.owners where idlic=lc.client_id and rodstv=1 and FirstPropDate<='{2}' and (Archive in (0,5) or (Archive in (1,2) and isnull(outtodate,'2999-12-31')>'{2}') ) ) then            (select first family||' '||name||' '||lastname from DBA.owners where idlic=lc.client_id and rodstv=1 and FirstPropDate<='{2}' and (Archive in (0,5) or (Archive in (1,2) and isnull(outtodate,'2999-12-31')>'{2}') ) order by 1)             else (if exists(select owner from DBA.owners where idlic=lc.client_id and FirstPropDate<='{2}' and (Archive in (0,5) or (Archive in (1,2) and isnull(outtodate,'2999-12-31')>'{2}' ) )) then                  (select first family||' '||name||' '||lastname from DBA.owners where idlic=lc.client_id and FirstPropDate<='{2}' and (Archive in (0,5) or (Archive in (1,2) and isnull(outtodate,'2999-12-31')>'{2}' ) ) order by 1)                else ''                endif)          endif)   endif) as FIO from lsPayment lp,lsclient lc left outer join lsParam p on p.client_id=lc.client_id and p.param_id=103 and dbeg<='{0}' and dend>='{0}' ,homes h,di_str ds, where lp.period_id={1} and lp.purposepay_id=5 and lp.payment_value<>0       and lp.client_id=lc.client_id and lc.idhome=h.idhome and h.idstr=ds.idstr", (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), (object) Options.Period.PeriodId, (object) KvrplHelper.DateToBaseFormat(DateTime.Now));
        DataSet dataSet = new DataSet();
        OleDbHelper.FillDataset(connectionString, CommandType.Text, commandText, 1000, dataSet, new string[1]{ str6 });
        OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + str7 + ";Extended Properties=dBase III");
        oleDbConnection.Open();
        OleDbCommand oleDbCommand1 = new OleDbCommand("drop table " + str6);
        oleDbCommand1.Connection = oleDbConnection;
        try
        {
          oleDbCommand1.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        new OleDbCommand("create table " + str6 + " (STREET varchar(40),DM varchar(12),KR varchar(6),ST varchar(6),FLAT varchar(10),COM varchar(1),OPL numeric(6,2),PAY numeric(7,2),DATA date,PCNT varchar(10),FIO varchar(70))")
        {
          Connection = oleDbConnection
        }.ExecuteNonQuery();
        if ((uint) dataSet.Tables[0].Rows.Count > 0U)
        {
          foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          {
            OleDbCommand oleDbCommand2 = new OleDbCommand();
            oleDbCommand2.Connection = oleDbConnection;
            oleDbCommand2.CommandType = CommandType.Text;
            oleDbCommand2.CommandText = "insert into " + str6 + "(street,dm,kr,st,flat,com,opl,pay,data,pcnt,fio) values(:street,:dm,:kr,:st,:flat,:com,:opl,:pay,:data,:pcnt,:fio)";
            oleDbCommand2.Parameters.Add("street", OleDbType.VarChar).Value = row["STREET"];
            oleDbCommand2.Parameters.Add("dm", OleDbType.VarChar).Value = row["DM"];
            oleDbCommand2.Parameters.Add("kr", OleDbType.VarChar).Value = row["KR"];
            oleDbCommand2.Parameters.Add("st", OleDbType.VarChar).Value = row["ST"];
            oleDbCommand2.Parameters.Add("flat", OleDbType.VarChar).Value = row["FLAT"];
            oleDbCommand2.Parameters.Add("com", OleDbType.VarChar).Value = row["COM"];
            oleDbCommand2.Parameters.Add("opl", OleDbType.Currency).Value = row["OPL"];
            oleDbCommand2.Parameters.Add("pay", OleDbType.Currency).Value = row["PAY"];
            oleDbCommand2.Parameters.Add("data", OleDbType.Date).Value = row["DATA"];
            oleDbCommand2.Parameters.Add("pcnt", OleDbType.VarChar).Value = row["PCNT"];
            oleDbCommand2.Parameters.Add("fio", OleDbType.VarChar).Value = row["FIO"];
            oleDbCommand2.ExecuteNonQuery();
          }
        }
        oleDbConnection.Close();
        int num2 = (int) MessageBox.Show("Выгрузка прошла успешно", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      catch (Exception ex)
      {
        int num2 = (int) MessageBox.Show("Выгрузка прошла c ошибками", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void tsmiLoadPayments_Click(object sender, EventArgs e)
    {
      FrmLoadPays frmLoadPays = new FrmLoadPays();
      int num = (int) frmLoadPays.ShowDialog();
      frmLoadPays.Dispose();
    }

    private void tsmiDataExchangeSocialProtection_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      int idcity = this.currentCompany == null ? Options.City : Convert.ToInt32(KvrplHelper.BaseValue(1, this.currentCompany));
      if (this.currentRaion != null)
      {
        try
        {
          CallDll.Soc(idcity, this.currentRaion.IdRnn, Options.Period.PeriodName.Value.ToOADate(), Options.Alias, Options.Login, Options.Pwd);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Невозможно вызвать библиотеку", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      else
      {
        int num1 = (int) MessageBox.Show("Необходимо выбрать район", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      this.Cursor = Cursors.Default;
    }

    private void tsmiExecuteScript_Click(object sender, EventArgs e)
    {
      this.fdSQL.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
      if (MessageBox.Show("Выполнить скрипт?", "Выполнение скриптов", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK || this.fdSQL.ShowDialog() != DialogResult.OK)
        return;
      FileStream fileStream = new FileStream(this.fdSQL.FileName, FileMode.Open);
      string end = new StreamReader((Stream) fileStream).ReadToEnd();
      fileStream.Close();
      string connectionString = string.Format("Provider={4};Eng={0};Uid={1};Pwd={2}; Links={3}", (object) Options.BaseName, (object) Options.Login, (object) Options.Pwd, (object) "tcpip{}", (object) Options.Provider);
      try
      {
        OleDbHelper.ExecuteNonQuery(connectionString, CommandType.Text, end, 10000);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Скрипт был выполнен с ошибкой: \n" + ex.Message + "\n\nОбратитесь к разработчику.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        KvrplHelper.WriteLog(ex, (LsClient) null);
        return;
      }
      int num1 = (int) MessageBox.Show("Операция завершена", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private bool CheckInOut(Period period)
    {
      IList list1 = (IList) new ArrayList();
      IList list2 = this.session.CreateSQLQuery(string.Format("select client_id,     isnull((select sum(balance_in) from lsBalance where period_id={0} and client_id=b.client_id and service_id not in (select s.service_id from dcService s, cmpServiceParam sp where s.root=sp.service_id and sp.company_id={1}  and sp.complex_id=b.complex_id and sendrent=1)),0) as balin,     isnull((select sum(balance_out) from lsBalance where period_id={0}-1 and client_id=b.client_id and service_id not in (select s.service_id from dcService s, cmpServiceParam sp where s.root=sp.service_id and sp.company_id={1} and sp.complex_id=b.complex_id and sendrent=1)),0) as balout from lsClient b where b.company_id={1} group by client_id,complex_id having balin<>balout", (object) period.PeriodId, (object) this.currentCompany.CompanyId)).List();
      if (list2.Count <= 0)
        return true;
      KvrplHelper.ViewErrorLic(list2, "Обнаружено " + list2.Count.ToString() + " лицевых где исходящее предыдущего месяца не равно входящему текущего. Месяц не может быть закрыт по начислениям.\n Показать лицевые счета?", "Исходящее предыдущего месяца не равно входящему текущего", (short) 1, this.sfd);
      return false;
    }

    private bool CheckRent(Period period)
    {
      IList list1 = (IList) new ArrayList();
      IList list2 = this.session.CreateSQLQuery(string.Format("select client_id,     isnull((select sum(rent) from lsRent where period_id={0} and client_id=b.client_id and code=0 and service_id not in (select s.service_id from dcService s, cmpServiceParam sp where s.root=sp.service_id and sp.company_id={1} and sp.complex_id=b.complex_id and sendrent=1)),0) as rent,     isnull((select sum(rent)-sum(rent_past) from lsBalance where period_id={0} and client_id=b.client_id and service_id not in (select s.service_id from dcService s, cmpServiceParam sp where s.root=sp.service_id and sp.company_id={1} and sp.complex_id=b.complex_id and sendrent=1)),0) as balance from lsClient b where b.company_id={1} group by client_id,complex_id having rent<>balance", (object) period.PeriodId, (object) this.currentCompany.CompanyId)).List();
      if (list2.Count <= 0)
        return true;
      KvrplHelper.ViewErrorLic(list2, "Обнаружено " + list2.Count.ToString() + " лицевых где расходятся начисления в сальдовочных и начислительных таблицах. Месяц не может быть закрыт по начислениям.\n Показать лицевые счета?", "Расходятся начисления в сальдовочных и начислительных таблицах", (short) 1, this.sfd);
      return false;
    }

    private bool CheckRentPast(Period period)
    {
      IList list1 = (IList) new ArrayList();
      IList list2 = this.session.CreateSQLQuery(string.Format("select client_id,     isnull((select sum(rent) from lsRent where period_id={0} and client_id=b.client_id and code not in (0,5,6) and service_id not in (select s.service_id from dcService s, cmpServiceParam sp where s.root=sp.service_id and sp.company_id={1} and sp.complex_id=b.complex_id and sendrent=1)),0) as rent,     isnull((select sum(rent_past) from lsBalance where period_id={0} and client_id=b.client_id and service_id not in (select s.service_id from dcService s, cmpServiceParam sp where s.root=sp.service_id and sp.company_id={1} and sp.complex_id=b.complex_id and sendrent=1)),0) as balance from lsClient b where b.company_id={1} group by client_id,complex_id having rent<>balance", (object) period.PeriodId, (object) this.currentCompany.CompanyId)).List();
      if (list2.Count <= 0)
        return true;
      KvrplHelper.ViewErrorLic(list2, "Обнаружено " + list2.Count.ToString() + " лицевых где расходятся перерасчеты в сальдовочных и начислительных таблицах. Месяц не может быть закрыт по начислениям.\n Показать лицевые счета?", "Расходятся перерасчеты в сальдовочных и начислительных таблицах", (short) 1, this.sfd);
      return false;
    }

    private bool CheckBalance(Period period)
    {
      IList list1 = (IList) new ArrayList();
      IList list2 = this.session.CreateSQLQuery(string.Format("select ls.client_id,sum(balance_in)+sum(rent)-sum(msp)-sum(payment)+sum(rent_comp) as balout1,        sum(balance_out) as balout2 from lsBalance b,lsClient ls where b.client_id=ls.client_id and period_id={0} and ls.company_id={1}  and service_id not in (select s.service_id from dcService s, cmpServiceParam sp where s.root=sp.service_id and sp.company_id={1} and sp.complex_id=ls.complex_id and sendrent=1) group by ls.client_id having balout1<>balout2", (object) period.PeriodId, (object) this.currentCompany.CompanyId)).List();
      if (list2.Count <= 0)
        return true;
      KvrplHelper.ViewErrorLic(list2, "Обнаружено " + list2.Count.ToString() + " лицевых где не сходится сальдо. Месяц не может быть закрыт по оплате.\n Показать лицевые счета?", "Не сходится сальдо", (short) 1, this.sfd);
      return false;
    }

    private bool CheckPayment(Period period)
    {
      IList list1 = (IList) new ArrayList();
      IList list2 = this.session.CreateSQLQuery(string.Format("select client_id, isnull((select sum(pay) from lsPay where period_id={0} and client_id=b.client_id and service_id not in (select s.service_id from dcService s, cmpServiceParam sp where s.root=sp.service_id and sp.company_id={1} and sp.complex_id=b.complex_id and sendrent=1)),0) as pay,isnull((select sum(payment) from lsBalance where period_id={0} and client_id=b.client_id and service_id not in (select s.service_id from dcService s, cmpServiceParam sp where s.root=sp.service_id and sp.company_id={1} and sp.complex_id=b.complex_id and sendrent=1)),0) as balancepay, isnull((select sum(payment_value) from lsPayment where period_id={0} and client_id=b.client_id and purposepay_id in (1,2,3,7,9,21,22) and service_id not in (select sp.service_id from cmpServiceParam sp where sp.company_id={1} and sp.complex_id=b.complex_id and sendrent=1)),0)+isnull((select sum(pay) from lsOverpay where period_id={0}-1 and client_id=b.client_id and code<10 and service_id not in (select sp.service_id from cmpServiceParam sp where sp.company_id={1} and sp.complex_id=b.complex_id and sendrent=1)),0) as payment1, balancepay+isnull((select sum(pay) from lsOverpay where period_id={0} and client_id=b.client_id and code<10 and service_id not in (select sp.service_id from cmpServiceParam sp where sp.company_id={1} and sp.complex_id=b.complex_id and sendrent=1)),0) as payment2 from lsClient b where b.company_id={1} group by client_id,complex_id having pay<>balancepay or payment1<>payment2", (object) period.PeriodId, (object) this.currentCompany.CompanyId)).List();
      if (list2.Count <= 0)
        return true;
      KvrplHelper.ViewErrorLic(list2, "Обнаружено " + list2.Count.ToString() + " лицевых где не сходятся платежи. Месяц не может быть закрыт по оплате.\n Показать лицевые счета?", "Не сходятся платежи", (short) 1, this.sfd);
      return false;
    }

    private bool CheckPeniRent(Period period)
    {
      IList list1 = (IList) new ArrayList();
      IList list2 = this.session.CreateSQLQuery(string.Format("select client_id,isnull((select sum(rent) from lsRentPeni where period_id={0} and client_id=b.client_id and code>=0),0) as rent,        isnull((select sum(rent) from lsBalancePeni where period_id={0} and client_id=b.client_id),0) as balance from lsClient b where b.company_id={1} group by client_id having rent<>balance", (object) period.PeriodId, (object) this.currentCompany.CompanyId)).List();
      if (list2.Count <= 0)
        return true;
      KvrplHelper.ViewErrorLic(list2, "Обнаружено " + list2.Count.ToString() + " лицевых где расходятся начисления пеней в сальдовочных и начислительных таблицах. Месяц не может быть закрыт по пеням.\n Показать лицевые счета?", "Расходятся начисления пеней в сальдовочных и начислительных таблицах", (short) 1, this.sfd);
      return false;
    }

    private bool CheckPeniPay(Period period)
    {
      IList list1 = (IList) new ArrayList();
      IList list2 = this.session.CreateSQLQuery(string.Format("select client_id,isnull((select sum(pay) from lsPayPeni where period_id={0} and client_id=b.client_id),0) as pay,        isnull((select sum(payment_peni) from lsPayment where period_id={0} and client_id=b.client_id),0) as lspay,        isnull((select sum(pay) from lsOverpay where period_id={0}-1 and client_id=b.client_id and code>=10),0) as predoverpay,       isnull((select sum(pay) from lsOverpay where period_id={0} and client_id=b.client_id and code>=10),0) as overpay,       isnull((select sum(Payment) from lsBalancePeni where period_id={0} and client_id=b.client_id),0) as balance from lsClient b where b.company_id={1} group by client_id having predoverpay+lspay<>pay+overpay or balance<>pay", (object) period.PeriodId, (object) this.currentCompany.CompanyId)).List();
      if (list2.Count <= 0)
        return true;
      KvrplHelper.ViewErrorLic(list2, "Обнаружено " + list2.Count.ToString() + " лицевых где расходится оплата пеней. Месяц не может быть закрыт по пеням.\n Показать лицевые счета?", "Расходится оплата пеней", (short) 1, this.sfd);
      return false;
    }

    private bool CheckPeniBalance(Period period)
    {
      IList list1 = (IList) new ArrayList();
      IList list2 = this.session.CreateSQLQuery(string.Format("select client_id,sum(balance_in)+sum(rent)-sum(payment)+sum(correct) as balout1,        sum(balance_out) as balout2 from lsBalancePeni b where period_id={0} and client_id in (select client_id from LsClient where company_id={1}) group by client_id having balout1<>balout2", (object) period.PeriodId, (object) this.currentCompany.CompanyId)).List();
      if (list2.Count <= 0)
        return true;
      KvrplHelper.ViewErrorLic(list2, "Обнаружено " + list2.Count.ToString() + " лицевых где не сходится сальдо по пеням. Месяц не может быть закрыт по пеням.\n Показать лицевые счета?", "Не сходится сальдо по пеням", (short) 1, this.sfd);
      return false;
    }

    private bool CheckSocRent(Period period)
    {
      IList list1 = (IList) new ArrayList();
      IList list2 = this.session.CreateSQLQuery(string.Format("select client_id,isnull((select sum(calc) from Soc_Saldo where period='{0}' and idlic=b.client_id ),0) as soc,         isnull((select sum(rent) from lsRentMSP where period_id={1} and client_id=b.client_id),0) as rent,        isnull((select sum(msp) from lsBalance where period_id={1} and client_id=b.client_id),0) as balance from lsClient b where b.company_id={2} group by client_id having rent<>balance+soc", (object) KvrplHelper.DateToBaseFormat(period.PeriodName.Value), (object) period.PeriodId, (object) this.currentCompany.CompanyId)).List();
      if (list2.Count <= 0)
        return true;
      KvrplHelper.ViewErrorLic(list2, "Обнаружено " + list2.Count.ToString() + " лицевых где расходятся начисления по МСП в сальдовочных и начислительных таблицах. Месяц не может быть закрыт по начислениям.\n Показать лицевые счета?", "Расходятся начисления по МСП в сальдовочных и начислительных таблицах", (short) 1, this.sfd);
      return false;
    }

    private bool CheckSocBalance(Period period)
    {
      IList list1 = (IList) new ArrayList();
      IList list2 = this.session.CreateSQLQuery(string.Format("select idlic,sum(incoming)+sum(calc)+sum(corr)-sum(pay) as out1,sum(outcoming) as out2 from Soc_Saldo b where period='{0}' and idlic in (select client_id from LsClient where company_id={1}) group by idlic having out1<>out2", (object) KvrplHelper.DateToBaseFormat(period.PeriodName.Value), (object) this.currentCompany.CompanyId)).List();
      if (list2.Count <= 0)
        return true;
      KvrplHelper.ViewErrorLic(list2, "Обнаружено " + list2.Count.ToString() + " лицевых где не сходится сальдо по МСП. Месяц не может быть закрыт по начислениям.\n Показать лицевые счета?", "Не сходится сальдо по МСП", (short) 1, this.sfd);
      return false;
    }

    private void LoadGrid()
    {
      this.session = Domain.CurrentSession;
      IList list1 = (IList) new ArrayList();
      IList list2 = (IList) new ArrayList();
      this.tsmiReports.Enabled = KvrplHelper.CheckProxy(34, 1, this.currentCompany, false);
      this.tsmiPayment.Enabled = KvrplHelper.CheckProxy(41, 1, this.currentCompany, false);
      this.tsmiCounters.Enabled = KvrplHelper.CheckProxy(42, 1, this.currentCompany, false);
      this.tsmiCalculation.Enabled = KvrplHelper.CheckProxy(35, 2, this.currentCompany, false);
      this.tsmiPrimaryDocCloseMonth.Enabled = KvrplHelper.CheckProxy(65, 1, this.currentCompany, false);
      this.tsmiPrimaryDocOpenMonth.Enabled = KvrplHelper.CheckProxy(66, 1, this.currentCompany, false);
      this.tsmiCalculationPeriod.Enabled = true;
      if (!KvrplHelper.CheckProxy(65, 1, this.currentCompany, false) && !KvrplHelper.CheckProxy(66, 1, this.currentCompany, false))
      {
        this.tsmiPrimaryDocuments.Visible = false;
        this.lblPrior.Visible = false;
        this.lblPriorClosed.Visible = false;
        this.lblKvrClose.Left = this.lblPrior.Left + 10;
        if (!KvrplHelper.CheckProxy(36, 1, this.currentCompany, false))
          this.tsmiCalculationPeriod.Enabled = false;
      }
      else
      {
        this.tsmiPrimaryDocuments.Visible = true;
        this.lblPrior.Visible = true;
        this.lblPriorClosed.Visible = true;
        this.lblKvrClose.Left = this.lblPrior.Left - 100;
      }
      this.tsmiTools.Enabled = KvrplHelper.CheckProxy(37, 1, this.currentCompany, false);
      this.tsmiDataExchangeSocialProtection.Enabled = KvrplHelper.CheckProxy(49, 2, this.currentCompany, false);
      this.tsmiExternalData.Visible = KvrplHelper.CheckProxy(64, 1, this.currentCompany, false);
      switch (this.VisibleProperty)
      {
        case 0:
          this.dgvMainList.DataSource = (object) null;
          this.lblCaption.Text = "Список районов";
          this.lblKvrCloseValue.Text = "";
          this.ShowClosedPeriod(false);
          this.tsmiHouseAdministration.Enabled = false;
          this.tsmiCalculationPeriod.Enabled = false;
          this.tsmiHome.Enabled = false;
          this.tsmiCounters.Enabled = false;
          this.tsmiDictionary.Enabled = false;
          this.dgvMainList.DataSource = Options.City != 1 ? (object) this.session.CreateQuery("from Raion order by IdRnn").List() : (object) this.session.CreateQuery("from Raion where idrnn<>7 order by IdRnn").List();
          if ((uint) this.dgvMainList.RowCount > 0U)
          {
            this.dgvMainList.Columns[0].HeaderText = "Код района";
            this.dgvMainList.Columns[1].HeaderText = "Район";
            break;
          }
          break;
        case 1:
          this.dgvMainList.DataSource = (object) null;
          this.currentRaion = this.session.Get<Raion>((object) this.currentRaion.IdRnn);
          this.lblCaption.Text = this.currentRaion.Rnn;
          this.lblKvrCloseValue.Text = "";
          this.lblPaspClosed.Text = "";
          this.lblPeniClosed.Text = "";
          this.lblOsznClosed.Text = "";
          this.lblPriorClosed.Text = "";
          this.tsmiHouseAdministration.Enabled = false;
          this.tsmiDictionary.Enabled = false;
          this.tsmiCalculationPeriod.Enabled = false;
          this.tsmiHome.Enabled = false;
          this.tsmiCounters.Enabled = false;
          IList list3 = (uint) this.currentRaion.IdRnn <= 0U ? this.session.CreateQuery(string.Format("select c from Company c,Raion r,Transfer t where c.Raion=r and t.Company=c and t.KvrCmp is not null order by c.CompanyId")).List() : this.session.CreateQuery(string.Format("select c from Company c,Raion r,Transfer t where c.Raion=r and r.IdRnn={0} and t.Company=c and t.KvrCmp is not null order by c.CompanyId", (object) this.currentRaion.IdRnn)).List();
          IList list4 = (IList) new ArrayList();
          foreach (Company cmp in (IEnumerable) list3)
          {
            if (KvrplHelper.AccessToCompany(97, cmp, 1))
              list4.Add((object) cmp);
          }
          this.dgvMainList.DataSource = (object) list4;
          if ((uint) this.dgvMainList.RowCount > 0U)
          {
            this.dgvMainList.Columns[0].HeaderText = "Код";
            this.dgvMainList.Columns[1].HeaderText = "Управляющая компания";
            this.dgvMainList.Columns[2].Visible = false;
            this.dgvMainList.Columns[3].Visible = false;
            this.dgvMainList.Columns[4].Visible = false;
            this.dgvMainList.Columns[5].Visible = false;
            this.dgvMainList.Columns[6].Visible = false;
            this.dgvMainList.Columns[7].Visible = false;
            this.dgvMainList.Columns[8].Visible = false;
            this.dgvMainList.Columns[9].Visible = false;
            this.dgvMainList.Columns[10].Visible = false;
          }
          this.ShowClosedPeriod(true);
          this.lblPrior.Visible = false;
          break;
        case 2:
          this.dgvMainList.DataSource = (object) null;
          this.Cursor = Cursors.WaitCursor;
          this.lblCaption.Text = this.currentCompany.CompanyName + " (" + (object) this.currentCompany.CompanyId + ")";
          list2 = this.session.CreateCriteria(typeof (Complex)).Add((ICriterion) Restrictions.Eq("ComplexId", (object) 100)).List();
          this.closedPeriod = KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.Complex.ComplexId, 0);
          System.Windows.Forms.Label lblKvrCloseValue1 = this.lblKvrCloseValue;
          DateTime dateTime1 = this.closedPeriod.PeriodName.Value;
          string str1 = dateTime1.ToString("MM/yyyy");
          lblKvrCloseValue1.Text = str1;
          System.Windows.Forms.Label lblPaspClosed = this.lblPaspClosed;
          dateTime1 = KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.ComplexPasp.ComplexId, 0).PeriodName.Value;
          string str2 = dateTime1.ToString("MM/yyyy");
          lblPaspClosed.Text = str2;
          System.Windows.Forms.Label lblPeniClosed1 = this.lblPeniClosed;
          DateTime? periodName1 = KvrplHelper.GetCmpKvrClose(this.currentCompany, 101, 0).PeriodName;
          dateTime1 = periodName1.Value;
          string str3 = dateTime1.ToString("MM/yyyy");
          lblPeniClosed1.Text = str3;
          System.Windows.Forms.Label lblOsznClosed1 = this.lblOsznClosed;
          periodName1 = KvrplHelper.GetCmpKvrClose(this.currentCompany, 104, 0).PeriodName;
          dateTime1 = periodName1.Value;
          string str4 = dateTime1.ToString("MM/yyyy");
          lblOsznClosed1.Text = str4;
          System.Windows.Forms.Label lblPriorClosed1 = this.lblPriorClosed;
          periodName1 = KvrplHelper.GetCmpKvrClose(this.currentCompany, 108, 0).PeriodName;
          dateTime1 = periodName1.Value;
          string str5 = dateTime1.ToString("MM/yyyy");
          lblPriorClosed1.Text = str5;
          this.tsmiHouseAdministration.Enabled = true;
          this.tsmiDictionary.Enabled = true;
          this.tsmiHome.Enabled = false;
          this.tsmiCounters.Enabled = true;
          IList list5 = this.session.CreateQuery(string.Format("select distinct new Home(h.IdHome,h.Str,h.NHome,h.HomeKorp,h.Division,h.YearBuild,h.Company.CompanyId) from Home h, Transfer t, HomeLink hl where hl.Home=h and hl.Company=t.Company and hl.Company.CompanyId={0} and t.KvrCmp is not null  " + Options.HomeType + " order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome),h.HomeKorp,h.IdHome", (object) this.currentCompany.CompanyId)).List();
          this.ShowClosedPeriod(true);
          this.dgvMainList.DataSource = (object) list5;
          if (this.dgvMainList.Columns.Count > 0)
          {
            this.dgvMainList.Columns[0].Visible = false;
            this.dgvMainList.Columns["Address"].Visible = false;
            this.dgvMainList.Columns[5].DisplayIndex = 0;
            this.dgvMainList.Columns["NameStr"].HeaderText = "Улица";
            this.dgvMainList.Columns["NHome"].HeaderText = "Дом";
            this.dgvMainList.Columns["HomeKorp"].HeaderText = "Корпус";
            this.dgvMainList.Columns["Division"].HeaderText = "Группа дома";
            this.dgvMainList.Columns["YearBuild"].HeaderText = "Год постройки";
          }
          this.Cursor = Cursors.Default;
          break;
        case 3:
          this.dgvMainList.DataSource = (object) null;
          this.lblCaption.Text = this.currentHome.Address;
          this.tsmiHouseAdministration.Enabled = true;
          this.tsmiDictionary.Enabled = true;
          this.tsmiHome.Enabled = true;
          this.tsmiCounters.Enabled = true;
          this.closedPeriod = KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.Complex.ComplexId, 0);
          this.lblPaspClosed.Text = KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.ComplexPasp.ComplexId, 0).PeriodName.Value.ToString("MM/yyyy");
          System.Windows.Forms.Label lblPeniClosed2 = this.lblPeniClosed;
          DateTime? periodName2 = KvrplHelper.GetCmpKvrClose(this.currentCompany, 101, 0).PeriodName;
          string str6 = periodName2.Value.ToString("MM/yyyy");
          lblPeniClosed2.Text = str6;
          System.Windows.Forms.Label lblOsznClosed2 = this.lblOsznClosed;
          periodName2 = KvrplHelper.GetCmpKvrClose(this.currentCompany, 104, 0).PeriodName;
          DateTime dateTime2 = periodName2.Value;
          string str7 = dateTime2.ToString("MM/yyyy");
          lblOsznClosed2.Text = str7;
          System.Windows.Forms.Label lblKvrCloseValue2 = this.lblKvrCloseValue;
          periodName2 = this.closedPeriod.PeriodName;
          dateTime2 = periodName2.Value;
          string str8 = dateTime2.ToString("MM/yyyy");
          lblKvrCloseValue2.Text = str8;
          System.Windows.Forms.Label lblPriorClosed2 = this.lblPriorClosed;
          periodName2 = KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.ComplexPrior.IdFk, 0).PeriodName;
          dateTime2 = periodName2.Value;
          string str9 = dateTime2.ToString("MM/yyyy");
          lblPriorClosed2.Text = str9;
          if (!Options.Kvartplata || !Options.Arenda)
          {
            this.tsmiLsArenda.Visible = false;
            this.tsmiLsKvartplata.Visible = false;
          }
          IList<LsClient> lsClientList = this.session.CreateQuery(string.Format("select new LsClient(ls.ClientId,ls.SurFlat,ls.Company,h,f,ls.Locality,ls.Complex) from LsClient ls,Home h,Flat f where ls.Company.CompanyId={2} and ls.Home=h and h.IdHome={0} and ls.Flat=f " + Options.MainConditions1 + " and isnull((select p.ParamValue from ClientParam p where p.ClientId=ls.ClientId and p.Param.ParamId=107 and p.DBeg<='{1}' and p.DEnd>'{1}' and p.Period.PeriodId=0),0) not in (6,7) order by ls.Complex.IdFk,DBA.LENGTHHOME(NFlat),f.IdFlat,DBA.LENGTHHOME(ls.SurFlat)", (object) this.currentHome.IdHome, (object) KvrplHelper.DateToBaseFormat(DateTime.Now), (object) this.currentCompany.CompanyId)).List<LsClient>();
          IList list6 = this.session.CreateQuery(string.Format("select (select p.ParamValue from ClientParam p where p.ClientId=ls.ClientId and p.Param.ParamId=107 and p.DBeg<='{1}' and p.DEnd>'{1}' and p.Period.PeriodId=0) as pr from LsClient ls,Home h,Flat f where ls.Company.CompanyId={2} and ls.Home=h and h.IdHome={0} and ls.Flat=f " + Options.MainConditions1 + "  and isnull((select p.ParamValue from ClientParam p where p.ClientId=ls.ClientId and p.Param.ParamId=107 and p.DBeg<='{1}' and p.DEnd>'{1}' and p.Period.PeriodId=0),0) not in (6,7) order by ls.Complex.IdFk,DBA.LENGTHHOME(NFlat),f.IdFlat,DBA.LENGTHHOME(ls.SurFlat)", (object) this.currentHome.IdHome, (object) KvrplHelper.DateToBaseFormat(DateTime.Now), (object) this.currentCompany.CompanyId)).List();
          this.pbar.Visible = true;
          this.pbar.Value = 0;
          this.pbar.Step = 0;
          this.pbar.Minimum = 0;
          this.pbar.Maximum = lsClientList.Count;
          short num1 = 0;
          IList<string> fio2 = KvrplHelper.GetFio2(this.currentCompany.CompanyId, this.currentHome.IdHome);
          bool flag = KvrplHelper.CheckProxy(48, 1, this.currentCompany, false);
          foreach (LsClient lsClient in (IEnumerable<LsClient>) lsClientList)
          {
            ++this.pbar.Value;
            if (flag)
              lsClient.Fio = fio2[(int) num1];
            lsClient.Status = Convert.ToInt32(list6[(int) num1]);
            ++num1;
          }
          this.dgvMainList.DataSource = (object) lsClientList;
          this.pbar.Visible = false;
          this.dgvMainList.Columns["ClientId"].HeaderText = "Лицевой счет";
          this.dgvMainList.Columns["Fio"].HeaderText = "ФИО квартиросъемщика";
          this.dgvMainList.Columns["NFlat"].HeaderText = "№ квартиры";
          this.dgvMainList.Columns["SurFlat"].HeaderText = "№ комнаты";
          this.dgvMainList.Columns["FullAddress"].Visible = false;
          this.dgvMainList.Columns["SmallAddress"].Visible = false;
          this.dgvMainList.Columns["Family"].Visible = false;
          this.dgvMainList.Columns["Name"].Visible = false;
          this.dgvMainList.Columns["FName"].Visible = false;
          this.dgvMainList.Columns["Phone"].Visible = false;
          this.dgvMainList.Columns["Floor"].Visible = false;
          this.dgvMainList.Columns["Entrance"].Visible = false;
          this.dgvMainList.Columns["Remark"].Visible = false;
          this.dgvMainList.Columns["OldId"].Visible = false;
          this.dgvMainList.Columns["Status"].Visible = false;
          break;
        case 4:
          this.ShowClosedPeriod(true);
          this.tsmiHome.Enabled = true;
          for (int index = 0; index < System.Windows.Forms.Application.OpenForms.Count; ++index)
          {
            if (System.Windows.Forms.Application.OpenForms[index].Name == "FrmClientCard")
              System.Windows.Forms.Application.OpenForms[index].Dispose();
          }
          FrmClientCard frmClientCard = new FrmClientCard(this.currentLs);
          int num2 = (int) frmClientCard.ShowDialog();
          this.currentLs = frmClientCard.Client;
          frmClientCard.Dispose();
          this.currentHome = this.session.Get<Home>((object) this.currentLs.Home.IdHome);
          this.currentCompany = this.session.Get<Company>((object) this.currentLs.Company.CompanyId);
          Options.Company = this.currentCompany;
          this.currentRaion = this.session.Get<Raion>((object) this.currentCompany.Raion.IdRnn);
          if (this.mpCurrentPeriod.Value != Options.Period.PeriodName.Value)
          {
            this.mpCurrentPeriod.OldMonth = 0;
            this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
          }
          this.btnUp_Click((object) null, (EventArgs) null);
          break;
      }
      this.session.Clear();
    }

    private void ShowSplashWnd()
    {
      try
      {
        this.frmSplash = new FrmSpash();
        this.frmSplash.Show();
        System.Windows.Forms.Application.DoEvents();
        this.frmSplash.SetMaxValue(1000000);
        for (int index = 0; index < 1000000; ++index)
        {
          Thread.Sleep(50);
          this.frmSplash.ChangeValue(index);
        }
        this.frmSplash.Close();
      }
      catch (ThreadAbortException ex)
      {
        Thread.ResetAbort();
      }
    }

    private void ShowPeriod4Company()
    {
      if (this.dgvMainList.CurrentRow == null && this.dgvMainList.Rows.Count > 0)
      {
        try
        {
          this.dgvMainList.CurrentCell = !this.dgvMainList.Rows[0].Cells[0].Visible ? this.dgvMainList.Rows[0].Cells[5] : this.dgvMainList.Rows[0].Cells[0];
        }
        catch
        {
        }
      }
      if (this.dgvMainList.CurrentRow == null || this.dgvMainList.Rows.Count <= 0)
        return;
      DateTime? periodName1 = KvrplHelper.GetCmpKvrClose((Company) this.dgvMainList.CurrentRow.DataBoundItem, Options.Complex.ComplexId, 0).PeriodName;
      this.lblKvrCloseValue.Text = periodName1.HasValue ? periodName1.Value.ToString("MM/yyyy") : "";
      DateTime? periodName2 = KvrplHelper.GetCmpKvrClose((Company) this.dgvMainList.CurrentRow.DataBoundItem, Options.ComplexPasp.ComplexId, 0).PeriodName;
      this.lblPaspClosed.Text = periodName2.HasValue ? periodName2.Value.ToString("MM/yyyy") : "";
      DateTime? periodName3 = KvrplHelper.GetCmpKvrClose((Company) this.dgvMainList.CurrentRow.DataBoundItem, 101, 0).PeriodName;
      this.lblPeniClosed.Text = periodName3.HasValue ? periodName3.Value.ToString("MM/yyyy") : "";
      DateTime? periodName4 = KvrplHelper.GetCmpKvrClose((Company) this.dgvMainList.CurrentRow.DataBoundItem, 104, 0).PeriodName;
      this.lblOsznClosed.Text = periodName4.HasValue ? periodName4.Value.ToString("MM/yyyy") : "";
      if (!KvrplHelper.CheckProxy(65, 1, (Company) this.dgvMainList.CurrentRow.DataBoundItem, false) && !KvrplHelper.CheckProxy(66, 1, (Company) this.dgvMainList.CurrentRow.DataBoundItem, false))
      {
        this.lblPrior.Visible = false;
        this.lblPriorClosed.Visible = false;
        this.lblKvrClose.Left = this.lblPrior.Left + 10;
      }
      else
      {
        DateTime? periodName5 = KvrplHelper.GetCmpKvrClose((Company) this.dgvMainList.CurrentRow.DataBoundItem, Options.ComplexPrior.IdFk, 0).PeriodName;
        this.lblPriorClosed.Text = periodName5.HasValue ? periodName5.Value.ToString("MM/yyyy") : "";
        this.lblPrior.Visible = true;
        this.lblPriorClosed.Visible = true;
        this.lblKvrClose.Left = this.lblPrior.Left - 100;
      }
    }

    private void NextLevel(bool pressEnter)
    {
      if (this.dgvMainList.CurrentRow == null)
        return;
      this.currentRow[(int) this.VisibleProperty] = this.dgvMainList.CurrentRow.Index;
      if (pressEnter && (int) this.VisibleProperty == 3 && this.currentRow[3] > 0)
        --this.currentRow[3];
      switch (this.VisibleProperty)
      {
        case 0:
          this.VisibleProperty = (short) 1;
          this.currentRaion = this.dgvMainList.CurrentRow.DataBoundItem as Raion;
          break;
        case 1:
          this.VisibleProperty = (short) 2;
          this.currentCompany = this.dgvMainList.CurrentRow.DataBoundItem as Company;
          Options.Company = this.currentCompany;
          break;
        case 2:
          this.VisibleProperty = (short) 3;
          this.currentHome = this.dgvMainList.CurrentRow.DataBoundItem as Home;
          break;
        case 3:
          this.VisibleProperty = (short) 4;
          this.currentLs = this.dgvMainList.CurrentRow.DataBoundItem as LsClient;
          break;
      }
      this.LoadGrid();
      if ((int) this.VisibleProperty == 1)
        this.ShowPeriod4Company();
    }

    private void dtmpCurrentPeriod_ValueChanged(object sender, EventArgs e)
    {
      Options.Period = KvrplHelper.SaveCurrentPeriod(this.mpCurrentPeriod.Value);
    }

    private bool IsClosedPeriod()
    {
      string str = "select min(Period.PeriodId) from CompanyPeriod where (Complex.IdFk in (100,101,102,108)";
      string queryString = Convert.ToInt32(KvrplHelper.BaseValue(25, Options.Company)) != 1 ? str + ")" : str + " or Complex.IdFk=104)";
      if ((int) this.VisibleProperty == 1)
        queryString += string.Format(" and Company.Raion.IdRnn={0}", (object) this.currentRaion.IdRnn);
      if ((int) this.VisibleProperty >= 2)
        queryString += string.Format(" and Company.CompanyId={0}", (object) this.currentCompany.CompanyId);
      int num = 0;
      try
      {
        num = Convert.ToInt32(this.session.CreateQuery(queryString).List()[0]);
      }
      catch
      {
      }
      return num >= Options.Period.PeriodId;
    }

    private int CRTCheck()
    {
      string str1 = "";
      int num1 = 0;
      string path = !File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Configuration\\" + Options.Login + "\\kvrpl.crt") ? AppDomain.CurrentDomain.BaseDirectory + "kvrpl.crt" : AppDomain.CurrentDomain.BaseDirectory + "Configuration\\" + Options.Login + "\\kvrpl.crt";
      string end;
      try
      {
        StreamReader streamReader = new StreamReader(path);
        end = streamReader.ReadToEnd();
        streamReader.Close();
      }
      catch
      {
        int num2 = (int) MessageBox.Show("Файл kvrpl.crt не найден. Обратитесь к разработчику", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return -1;
      }
      for (int index = 0; index < end.Length - 1; ++index)
      {
        char ch = index <= this.str1.Length ? this.str1[index] : (index % this.str1.Length != 0 ? this.str1[index % this.str1.Length] : this.str1[this.str1.Length]);
        str1 += Convert.ToChar((int) Encoding.ASCII.GetBytes(end)[index] ^ (int) Encoding.ASCII.GetBytes(new char[1]{ ch })[0]).ToString();
        if (index < end.Length - 1)
          num1 += (index + 1) * (int) Encoding.ASCII.GetBytes(end)[index];
      }
      int num3 = num1 % 13;
      if ((int) str1[0] == 90)
      {
        this._newCheckCrt = true;
        string str2 = "";
        List<string> stringList = new List<string>();
        string[] strArray = str1.Split('|');
        str2 = strArray[1];
        int int32 = Convert.ToInt32(strArray[2]);
        for (int index = 3; index < strArray.Length; ++index)
          stringList.Add(strArray[index]);
        IList list1 = (IList) new ArrayList();
        IList list2 = this.currentRaion == null || (uint) this.currentRaion.IdRnn <= 0U ? this.session.CreateQuery(string.Format("select c from Company c,Raion r,Transfer t where c.Raion=r and t.Company=c and t.KvrCmp is not null order by c.CompanyId")).List() : this.session.CreateQuery(string.Format("select c from Company c,Raion r,Transfer t where c.Raion=r and r.IdRnn={0} and t.Company=c and t.KvrCmp is not null order by c.CompanyId", (object) this.currentRaion.IdRnn)).List();
        IList list3 = (IList) new ArrayList();
        foreach (Company cmp in (IEnumerable) list2)
        {
          if (KvrplHelper.AccessToCompany(97, cmp, 1))
            list3.Add((object) cmp);
        }
        string str3 = " and lc.company_id in (";
        if (list3.Count == 0)
          return -1;
        foreach (Company company in (IEnumerable) list3)
        {
          if (!stringList.Contains(company.CompanyId.ToString()))
            return -1;
          str3 = str3 + (object) company.CompanyId + ",";
        }
        return this.session.CreateSQLQuery(string.Format("select count(lc.client_id)                        from DBA.lsclient lc,DBA.lsparam lp                        where lc.client_id=lp.client_id and lp.period_id=0 and param_id=107                                and dbeg<='{0}' and dend>='{0}' and param_value<3 " + (str3.Remove(str3.LastIndexOf(","), 1) + ")") ?? "", (object) KvrplHelper.DateToBaseFormat(DateTime.Now))).UniqueResult<int>() > int32 ? -1 : 0;
      }
      IList list = this.session.CreateSQLQuery(string.Format("select multiplkod,(select count(lc.client_id)                        from DBA.lsclient lc,DBA.lsparam lp                        where lc.client_id=lp.client_id and lp.period_id=0 and param_id=107                                and dbeg<='{0}' and dend>='{0}' and param_value<3) as lic from DBA.kodbd", (object) KvrplHelper.DateToBaseFormat(DateTime.Now))).List();
      if (str1.Substring(0, str1.IndexOf("|")) != ((object[]) list[0])[0].ToString() || Convert.ToInt32(str1.Substring(str1.IndexOf("|") + 1, str1.Length - str1.IndexOf("|") - 1)) < Convert.ToInt32(((object[]) list[0])[1]))
        return Convert.ToInt32(((object[]) list[0])[1]);
      return 0;
    }

    private void CRTCreate(int multiplkod)
    {
      try
      {
        string str1 = "Word-echo,empty phrase";
        string str2 = "Код базы: " + multiplkod.ToString() + "\n";
        IList<Company> companyList = (IList<Company>) new List<Company>();
        foreach (Company company in (IEnumerable<Company>) this.session.CreateCriteria(typeof (Company)).List<Company>())
          str2 = str2 + " Company: " + company.CompanyId.ToString() + " IdRnn: " + company.Raion.IdRnn.ToString() + " Пользователь: " + company.UName + "\n";
        foreach (object[] objArray in (IEnumerable) this.session.CreateQuery(string.Format("select s.NameStr,h.NHome,h.HomeKorp,count(lc.ClientId) from Str s,Home h, LsClient lc, ClientParam cp where h.Str=s and lc.Home=h and lc.ClientId=cp.ClientId and cp.Period.PeriodId=0 and cp.Param.ParamId=107 and DBeg<='{0}' and DEnd>='{0}' and cp.ParamValue<3 group by s.NameStr,h.NHome,h.HomeKorp", (object) KvrplHelper.DateToBaseFormat(DateTime.Now))).List())
          str2 = str2 + " Адрес :" + objArray[0].ToString() + " д. " + objArray[1].ToString() + " " + objArray[2].ToString() + " Лицевых: " + objArray[3].ToString() + "\n";
        foreach (object[] objArray in (IEnumerable) this.session.CreateQuery(string.Format("select c.CompanyId,count(lc.ClientId) from Company c, LsClient lc, ClientParam cp where lc.Company=c and lc.ClientId=cp.ClientId and cp.Period.PeriodId=0 and cp.Param.ParamId=107 and DBeg<='{0}' and DEnd>='{0}' and cp.ParamValue<3 group by c.CompanyId", (object) KvrplHelper.DateToBaseFormat(DateTime.Now))).List())
          str2 = str2 + " Номер участка :" + objArray[0].ToString() + " Лицевых: " + objArray[1].ToString() + "\n";
        FileStream fileStream1 = new FileStream("KvrplOut.txt", FileMode.Create, FileAccess.Write);
        StreamWriter streamWriter1 = new StreamWriter((Stream) fileStream1);
        streamWriter1.WriteLine(str2);
        streamWriter1.Close();
        fileStream1.Close();
        int num1 = 0;
        string str3 = "";
        int index;
        for (index = 0; index < str2.Length - 1; ++index)
        {
          char ch1 = index <= str1.Length - 1 ? str1[index] : ((index + 1) % str1.Length != 0 ? str1[(index + 1) % str1.Length] : str1[str1.Length - 1]);
          char ch2 = str2[index];
          str3 += Convert.ToChar((int) Encoding.ASCII.GetBytes(new char[1]{ ch2 })[0] ^ (int) Encoding.ASCII.GetBytes(new char[1]{ ch1 })[0]).ToString();
          num1 += (index + 1) * (int) Convert.ToChar((int) Encoding.ASCII.GetBytes(new char[1]{ ch2 })[0] ^ (int) Encoding.ASCII.GetBytes(new char[1]{ ch1 })[0]);
        }
        int num2 = num1 % 13;
        char ch = index <= str1.Length ? str1[index] : (index % (str1.Length - 1) != 0 ? str1[index % (str1.Length - 1)] : str1[str1.Length - 1]);
        string str4 = str3 + Convert.ToChar((int) Encoding.ASCII.GetBytes(num2.ToString())[0] ^ (int) Encoding.ASCII.GetBytes(new char[1]{ ch })[0]).ToString();
        FileStream fileStream2 = new FileStream("KvrplOut.crt", FileMode.Create, FileAccess.Write);
        StreamWriter streamWriter2 = new StreamWriter((Stream) fileStream2);
        streamWriter2.WriteLine(str4);
        streamWriter2.Close();
        fileStream2.Close();
      }
      catch (EndOfStreamException ex)
      {
        int num = (int) MessageBox.Show(ex.Message + " " + (object) ex.InnerException + " " + ex.Source);
      }
      catch (AccessViolationException ex)
      {
        int num = (int) MessageBox.Show(ex.Message + " " + (object) ex.InnerException + " " + ex.Source);
      }
      catch (IOException ex)
      {
        int num = (int) MessageBox.Show(ex.Message + " " + (object) ex.InnerException + " " + ex.Source);
      }
    }

    private void HideInformation()
    {
      this.lblOszn.Visible = false;
      this.lblOsznClosed.Visible = false;
      this.tsmiSocialProtection.Visible = false;
      this.tsmiSocialProtection.Enabled = false;
    }

    private void ShowClosedPeriod(bool value)
    {
      this.lblKvrClose.Visible = value;
      this.lblPasp.Visible = value;
      this.lblPeni.Visible = value;
      this.lblOszn.Visible = value;
      this.lblOsznClosed.Visible = value;
      this.lblKvrCloseValue.Visible = value;
      this.lblPaspClosed.Visible = value;
      this.lblPeniClosed.Visible = value;
      this.lblPrior.Visible = value;
      this.lblPriorClosed.Visible = value;
      this.lblKvr.Visible = value;
      int city = Options.City;
      if (this.currentCompany != null)
        city = Convert.ToInt32(KvrplHelper.BaseValue(1, this.currentCompany));
      if (this.dgvMainList.CurrentRow == null && this.dgvMainList.Rows.Count > 0)
      {
        try
        {
          this.dgvMainList.CurrentCell = !this.dgvMainList.Rows[0].Cells[0].Visible ? this.dgvMainList.Rows[0].Cells[5] : this.dgvMainList.Rows[0].Cells[0];
        }
        catch
        {
        }
      }
      if (((int) this.VisibleProperty != 1 || this.dgvMainList.CurrentRow == null) && Convert.ToInt32(KvrplHelper.BaseValue(25, Options.Company)) != 1 || (int) this.VisibleProperty == 1 && this.dgvMainList.CurrentRow != null && Convert.ToInt32(KvrplHelper.BaseValue(25, (Company) this.dgvMainList.CurrentRow.DataBoundItem)) != 1)
      {
        this.HideInformation();
      }
      else
      {
        this.tsmiSocialProtection.Visible = true;
        this.tsmiSocialProtection.Enabled = true;
      }
      this.ShowButtonsForCity(city);
    }

    private void ShowButtonsForCity(int city)
    {
      switch (city)
      {
        case 16:
          this.tsmiUZP.Visible = true;
          break;
        case 23:
        case 24:
          this.tsmiUZP.Visible = true;
          break;
        case 3:
          this.tsmiPrintPolicy.Visible = true;
          break;
        case 4:
          this.tsmiLoadFacilities.Enabled = true;
          this.tsmiInsuranceUnLoad.Enabled = true;
          break;
        case 15:
          this.tsmiLoadPayments.Enabled = true;
          break;
      }
    }

    private void ViewErrorLic(IList list, string message, string fileMessage, short type)
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
          }
        }
        int num1 = (int) MessageBox.Show(text, "", MessageBoxButtons.OK);
        if (MessageBox.Show("Сохранить лицевые в файл?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
          return;
        if (this.sfd.ShowDialog() == DialogResult.OK)
        {
          try
          {
            StreamWriter streamWriter = new StreamWriter((Stream) File.Open(this.sfd.FileName, FileMode.Append, FileAccess.Write), Encoding.Default);
            streamWriter.WriteLine(DateTime.Now.ToString() + ": " + fileMessage);
            foreach (object obj in (IEnumerable) list)
            {
              switch (type)
              {
                case 1:
                  streamWriter.WriteLine(((object[]) obj)[0].ToString());
                  break;
                case 2:
                  streamWriter.WriteLine("Лицевой: " + ((object[]) obj)[0].ToString() + ", ФИО: " + ((object[]) obj)[1].ToString() + ", льгота: " + ((object[]) obj)[2].ToString());
                  break;
              }
            }
            streamWriter.Close();
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
        int num = (int) MessageBox.Show("Используйте отчеты из библиотеки отчетов или сверку итогов на форме расчета для поиска ошибок.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void tsmiCheckOverhaul_Click(object sender, EventArgs e)
    {
      int num = (int) new FrmSelectAccountForOverhaul(this.currentCompany).ShowDialog();
    }

    private void tsmiNewReceipt_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(34, 1, this.currentCompany, true))
        return;
      this.Cursor = Cursors.WaitCursor;
      int num1 = !KvrplHelper.CheckProxy(32, 2, this.currentCompany, false) ? 0 : 1;
      int admin = !Options.Kvartplata || !Options.Arenda ? (!Options.Kvartplata ? num1 + 10 : num1 + 0) : num1 + 20;
      short num2;
      int idcity;
      if (this.currentCompany != null)
      {
        num2 = this.currentCompany.CompanyId;
        idcity = Convert.ToInt32(KvrplHelper.BaseValue(1, this.currentCompany));
      }
      else
      {
        num2 = (short) 0;
        idcity = Options.City;
      }
      int Home = this.currentHome == null ? 0 : this.currentHome.IdHome;
      int Rnn = this.currentRaion == null || (int) num2 != 0 ? 0 : (int) Convert.ToInt16(this.currentRaion.IdRnn);
      try
      {
        CallDll.Rep(idcity, 0, Rnn, (int) num2, Home, 0, 32, Options.Period.PeriodId, admin, Options.Alias, Options.Login, Options.Pwd);
      }
      catch (Exception ex)
      {
        int num3 = (int) MessageBox.Show("Невозможно вызвать библиотеку отчетов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      this.Cursor = Cursors.Default;
    }

    private void asdaToolStripMenuItem_Click(object sender, EventArgs e)
    {
      foreach (int val in (IEnumerable<int>) new List<int>() { 1044, 1443, 1843, 2243, 2643, 3043, 3443, 3843, 4243, 4643, 5043, 5443, 5843, 6243, 6643, 7043, 7443, 7843, 8243, 8643, 8664 })
      {
        IList<SupplierClient> supplierClientList = this.session.CreateQuery("from SupplierClient ls where ls.Supplier.BaseOrgId=-39999859 and ls.Company.CompanyId=:cmp").SetParameter<int>("cmp", val).List<SupplierClient>();
        string str = "C:\\" + val.ToString() + ".xlsx";
        try
        {
          this.ObjExcel = (Microsoft.Office.Interop.Excel.Application) new ApplicationClass();
          this.ObjWorkBook = this.ObjExcel.Workbooks.Add((object) Missing.Value);
          this.ObjWorkSheet = (Worksheet) this.ObjWorkBook.Sheets[(object) 1];
          int num = 1;
          foreach (SupplierClient supplierClient in (IEnumerable<SupplierClient>) supplierClientList)
          {
            this.ObjExcel.Cells[(object) num, (object) 1] = (object) supplierClient.SupplierClientId;
            this.ObjExcel.Cells[(object) num, (object) 2] = (object) KvrplHelper.GetPin(Convert.ToInt32(supplierClient.SupplierClientId));
            ++num;
          }
          this.ObjWorkBook.SaveAs((object) str, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, XlSaveAsAccessMode.xlNoChange, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(ex.Message, "Error");
        }
      }
    }

    private void tsmiTypeAccount_Click(object sender, EventArgs e)
    {
      int num = (int) new FrmDcOnlTypeAccount(this.currentCompany).ShowDialog();
    }

    private void tsmiRcptAccount_Click(object sender, EventArgs e)
    {
      int homeid = 0;
      if (this.currentHome != null)
        homeid = this.currentHome.IdHome;
      int num = (int) new FrmRcptAccounts(this.currentCompany, homeid).ShowDialog();
    }

    private void tsmiL_U_hmReceipt_Click(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MainForm));
      this.msMainMenu = new MenuStrip();
      this.tsmiDictionary = new ToolStripMenuItem();
      this.tsmiTariffs = new ToolStripMenuItem();
      this.tsmiFacilities = new ToolStripMenuItem();
      this.tsmiAbsence = new ToolStripMenuItem();
      this.tsmiQuality = new ToolStripMenuItem();
      this.tsmiSourcePayment = new ToolStripMenuItem();
      this.tsmiPurposePayment = new ToolStripMenuItem();
      this.tsmiParameters = new ToolStripMenuItem();
      this.tsmiTypeDocuments = new ToolStripMenuItem();
      this.tsmiTypeCounters = new ToolStripMenuItem();
      this.tsmiSuppliers = new ToolStripMenuItem();
      this.tsmiDicReceipts = new ToolStripMenuItem();
      this.tsmiServiceOrganizations = new ToolStripMenuItem();
      this.tsmiDicServices = new ToolStripMenuItem();
      this.tsmiBanks = new ToolStripMenuItem();
      this.tsmiOrganizations = new ToolStripMenuItem();
      this.tsmiTypeLocationCounter = new ToolStripMenuItem();
      this.tsmiTypeSeals = new ToolStripMenuItem();
      this.tsmiTypeBindingServices = new ToolStripMenuItem();
      this.tsmiBindingServices = new ToolStripMenuItem();
      this.tsmiTypeNoteBook = new ToolStripMenuItem();
      this.tsmiGuilds = new ToolStripMenuItem();
      this.tsmiContractOrganization = new ToolStripMenuItem();
      this.tsmiSprAccounts = new ToolStripMenuItem();
      this.tsmiTypeAccount = new ToolStripMenuItem();
      this.tsmiRcptAccount = new ToolStripMenuItem();
      this.tsmiTools = new ToolStripMenuItem();
      this.tsmiReports = new ToolStripMenuItem();
      this.tsmiPayment = new ToolStripMenuItem();
      this.tsmiCounters = new ToolStripMenuItem();
      this.tsmiCalculation = new ToolStripMenuItem();
      this.tsmiReceipts = new ToolStripMenuItem();
      this.tsmiOldReceipt = new ToolStripMenuItem();
      this.tsmiNewReceipt = new ToolStripMenuItem();
      this.tsmiPrintBill = new ToolStripMenuItem();
      this.tsmiSearch = new ToolStripMenuItem();
      this.tsmiFastSearch = new ToolStripMenuItem();
      this.tsmiFlatSearch = new ToolStripMenuItem();
      this.tsmiAdvancedSearch = new ToolStripMenuItem();
      this.tsmiStreetSearch = new ToolStripMenuItem();
      this.tsmiContractSearch = new ToolStripMenuItem();
      this.tsmiPersonenNumberSearch = new ToolStripMenuItem();
      this.tsmiSnilsSearch = new ToolStripMenuItem();
      this.tsmiPrintPolicy = new ToolStripMenuItem();
      this.tsmiUZP = new ToolStripMenuItem();
      this.tsmiCalculationPeriod = new ToolStripMenuItem();
      this.tsmiPrimaryDocuments = new ToolStripMenuItem();
      this.tsmiPrimaryDocCloseMonth = new ToolStripMenuItem();
      this.tsmiPrimaryDocOpenMonth = new ToolStripMenuItem();
      this.tsmiCharge = new ToolStripMenuItem();
      this.tsmiChargeCloseMonth = new ToolStripMenuItem();
      this.tsmiChargeOpenMonth = new ToolStripMenuItem();
      this.tsmiCalcPerPayment = new ToolStripMenuItem();
      this.tsmiCalcPerPaymentCloseMonth = new ToolStripMenuItem();
      this.tsmiCalcPerPaymentOpenMonth = new ToolStripMenuItem();
      this.tsmiPeni = new ToolStripMenuItem();
      this.tsmiPeniCloseMonth = new ToolStripMenuItem();
      this.tsmiPeniOpenMonth = new ToolStripMenuItem();
      this.tsmiSocialProtection = new ToolStripMenuItem();
      this.tsmiSocialProtectionCloseMonth = new ToolStripMenuItem();
      this.tsmiSocialProtectionOpenMonth = new ToolStripMenuItem();
      this.tsmiHouseAdministration = new ToolStripMenuItem();
      this.tsmiRecvisitsAndParameters = new ToolStripMenuItem();
      this.tsmiOperations = new ToolStripMenuItem();
      this.tsmiNoteBook = new ToolStripMenuItem();
      this.tsmiEnterTariffs = new ToolStripMenuItem();
      this.tsmiInspectionContracts = new ToolStripMenuItem();
      this.tsmiHome = new ToolStripMenuItem();
      this.tsmiHomeParametrs = new ToolStripMenuItem();
      this.tsmiMakeEntrance = new ToolStripMenuItem();
      this.tsmiNewLsClient = new ToolStripMenuItem();
      this.tsmiLsKvartplata = new ToolStripMenuItem();
      this.tsmiLsArenda = new ToolStripMenuItem();
      this.tsmiLsClientClose = new ToolStripMenuItem();
      this.tsmiLoadUnLoad = new ToolStripMenuItem();
      this.tsmiLoadFacilities = new ToolStripMenuItem();
      this.tsmiInsuranceUnLoad = new ToolStripMenuItem();
      this.tsmiLoadPayments = new ToolStripMenuItem();
      this.tsmiDataExchangeSocialProtection = new ToolStripMenuItem();
      this.tsmiL_U_hmReceipt = new ToolStripMenuItem();
      this.tsmiExternalData = new ToolStripMenuItem();
      this.tsmiAbout = new ToolStripMenuItem();
      this.tsmiHelp = new ToolStripMenuItem();
      this.tsmiExecute = new ToolStripMenuItem();
      this.tsmiExecuteScript = new ToolStripMenuItem();
      this.капитальныйРемонтToolStripMenuItem = new ToolStripMenuItem();
      this.tsmiCheckOverhaul = new ToolStripMenuItem();
      this.asdaToolStripMenuItem = new ToolStripMenuItem();
      this.pnBtn = new Panel();
      this.btnWOW = new System.Windows.Forms.Button();
      this.pbar = new ProgressBar();
      this.btnExit = new System.Windows.Forms.Button();
      this.pnUp = new Panel();
      this.lblPriorClosed = new System.Windows.Forms.Label();
      this.lblPrior = new System.Windows.Forms.Label();
      this.lblKvr = new System.Windows.Forms.Label();
      this.lblOsznClosed = new System.Windows.Forms.Label();
      this.lblOszn = new System.Windows.Forms.Label();
      this.mpCurrentPeriod = new MonthPicker();
      this.lblPeni = new System.Windows.Forms.Label();
      this.lblPasp = new System.Windows.Forms.Label();
      this.lblKv = new System.Windows.Forms.Label();
      this.lblPeniClosed = new System.Windows.Forms.Label();
      this.lblPaspClosed = new System.Windows.Forms.Label();
      this.lblKvrCloseValue = new System.Windows.Forms.Label();
      this.lblKvrClose = new System.Windows.Forms.Label();
      this.lblCaption = new System.Windows.Forms.Label();
      this.btnUp = new System.Windows.Forms.Button();
      this.dgvMainList = new DataGridView();
      this.hp = new HelpProvider();
      this.sfCRT = new SaveFileDialog();
      this.fdHelpLoad = new OpenFileDialog();
      this.sfdStrahUnLoad = new SaveFileDialog();
      this.fdSQL = new OpenFileDialog();
      this.sfd = new SaveFileDialog();
      this.dtmpCurrentPeriod = new MonthPicker();
      this.msMainMenu.SuspendLayout();
      this.pnBtn.SuspendLayout();
      this.pnUp.SuspendLayout();
      ((ISupportInitialize) this.dgvMainList).BeginInit();
      this.SuspendLayout();
      this.msMainMenu.BackgroundImageLayout = ImageLayout.None;
      this.msMainMenu.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.msMainMenu.GripMargin = new Padding(2);
      this.msMainMenu.Items.AddRange(new ToolStripItem[21]
      {
        (ToolStripItem) this.tsmiDictionary,
        (ToolStripItem) this.tsmiTools,
        (ToolStripItem) this.tsmiReports,
        (ToolStripItem) this.tsmiPayment,
        (ToolStripItem) this.tsmiCounters,
        (ToolStripItem) this.tsmiCalculation,
        (ToolStripItem) this.tsmiReceipts,
        (ToolStripItem) this.tsmiPrintBill,
        (ToolStripItem) this.tsmiSearch,
        (ToolStripItem) this.tsmiPrintPolicy,
        (ToolStripItem) this.tsmiUZP,
        (ToolStripItem) this.tsmiCalculationPeriod,
        (ToolStripItem) this.tsmiHouseAdministration,
        (ToolStripItem) this.tsmiHome,
        (ToolStripItem) this.tsmiLoadUnLoad,
        (ToolStripItem) this.tsmiExternalData,
        (ToolStripItem) this.tsmiAbout,
        (ToolStripItem) this.tsmiHelp,
        (ToolStripItem) this.tsmiExecute,
        (ToolStripItem) this.капитальныйРемонтToolStripMenuItem,
        (ToolStripItem) this.asdaToolStripMenuItem
      });
      this.msMainMenu.LayoutStyle = ToolStripLayoutStyle.Flow;
      this.msMainMenu.Location = new System.Drawing.Point(0, 0);
      this.msMainMenu.Name = "msMainMenu";
      this.msMainMenu.Padding = new Padding(8, 2, 0, 2);
      this.msMainMenu.Size = new Size(1055, 44);
      this.msMainMenu.Stretch = false;
      this.msMainMenu.TabIndex = 2;
      this.msMainMenu.Text = "menuStrip1";
      this.tsmiDictionary.DropDownItems.AddRange(new ToolStripItem[25]
      {
        (ToolStripItem) this.tsmiTariffs,
        (ToolStripItem) this.tsmiFacilities,
        (ToolStripItem) this.tsmiAbsence,
        (ToolStripItem) this.tsmiQuality,
        (ToolStripItem) this.tsmiSourcePayment,
        (ToolStripItem) this.tsmiPurposePayment,
        (ToolStripItem) this.tsmiParameters,
        (ToolStripItem) this.tsmiTypeDocuments,
        (ToolStripItem) this.tsmiTypeCounters,
        (ToolStripItem) this.tsmiSuppliers,
        (ToolStripItem) this.tsmiDicReceipts,
        (ToolStripItem) this.tsmiServiceOrganizations,
        (ToolStripItem) this.tsmiDicServices,
        (ToolStripItem) this.tsmiBanks,
        (ToolStripItem) this.tsmiOrganizations,
        (ToolStripItem) this.tsmiTypeLocationCounter,
        (ToolStripItem) this.tsmiTypeSeals,
        (ToolStripItem) this.tsmiTypeBindingServices,
        (ToolStripItem) this.tsmiBindingServices,
        (ToolStripItem) this.tsmiTypeNoteBook,
        (ToolStripItem) this.tsmiGuilds,
        (ToolStripItem) this.tsmiContractOrganization,
        (ToolStripItem) this.tsmiSprAccounts,
        (ToolStripItem) this.tsmiTypeAccount,
        (ToolStripItem) this.tsmiRcptAccount
      });
      this.tsmiDictionary.Enabled = false;
      this.tsmiDictionary.Name = "tsmiDictionary";
      this.tsmiDictionary.Size = new Size(97, 20);
      this.tsmiDictionary.Text = "Справочники";
      this.tsmiTariffs.Name = "tsmiTariffs";
      this.tsmiTariffs.Size = new Size(261, 22);
      this.tsmiTariffs.Text = "Тарифы";
      this.tsmiTariffs.Click += new EventHandler(this.tsmiTariffs_Click);
      this.tsmiFacilities.Name = "tsmiFacilities";
      this.tsmiFacilities.Size = new Size(261, 22);
      this.tsmiFacilities.Text = "Льготы";
      this.tsmiFacilities.Click += new EventHandler(this.tsmiFacilities_Click);
      this.tsmiAbsence.Name = "tsmiAbsence";
      this.tsmiAbsence.Size = new Size(261, 22);
      this.tsmiAbsence.Text = "Отсутствие";
      this.tsmiAbsence.Click += new EventHandler(this.tsmiAbsence_Click);
      this.tsmiQuality.Name = "tsmiQuality";
      this.tsmiQuality.Size = new Size(261, 22);
      this.tsmiQuality.Text = "Качество";
      this.tsmiQuality.Click += new EventHandler(this.tsmiQuality_Click);
      this.tsmiSourcePayment.Name = "tsmiSourcePayment";
      this.tsmiSourcePayment.Size = new Size(261, 22);
      this.tsmiSourcePayment.Text = "Источники платежей";
      this.tsmiSourcePayment.Click += new EventHandler(this.tsmiSourcePayment_Click);
      this.tsmiPurposePayment.Name = "tsmiPurposePayment";
      this.tsmiPurposePayment.Size = new Size(261, 22);
      this.tsmiPurposePayment.Text = "Назначение платежей";
      this.tsmiPurposePayment.Click += new EventHandler(this.tsmiPurposePayment_Click);
      this.tsmiParameters.Name = "tsmiParameters";
      this.tsmiParameters.Size = new Size(261, 22);
      this.tsmiParameters.Text = "Параметры";
      this.tsmiParameters.Click += new EventHandler(this.tsmiParameters_Click);
      this.tsmiTypeDocuments.Name = "tsmiTypeDocuments";
      this.tsmiTypeDocuments.Size = new Size(261, 22);
      this.tsmiTypeDocuments.Text = "Виды документов";
      this.tsmiTypeDocuments.Click += new EventHandler(this.tsmiTypeDocuments_Click);
      this.tsmiTypeCounters.Name = "tsmiTypeCounters";
      this.tsmiTypeCounters.Size = new Size(261, 22);
      this.tsmiTypeCounters.Text = "Типы счетчиков";
      this.tsmiTypeCounters.Click += new EventHandler(this.tsmiTypeCounters_Click);
      this.tsmiSuppliers.Name = "tsmiSuppliers";
      this.tsmiSuppliers.Size = new Size(261, 22);
      this.tsmiSuppliers.Text = "Поставщики";
      this.tsmiSuppliers.Click += new EventHandler(this.tsmiSuppliers_Click);
      this.tsmiDicReceipts.Name = "tsmiDicReceipts";
      this.tsmiDicReceipts.Size = new Size(261, 22);
      this.tsmiDicReceipts.Text = "Квитанция";
      this.tsmiDicReceipts.Click += new EventHandler(this.tsmiDicReceipts_Click);
      this.tsmiServiceOrganizations.Name = "tsmiServiceOrganizations";
      this.tsmiServiceOrganizations.Size = new Size(261, 22);
      this.tsmiServiceOrganizations.Text = "Услуги организаций";
      this.tsmiServiceOrganizations.Click += new EventHandler(this.tsmiServiceOrganizations_Click);
      this.tsmiDicServices.Name = "tsmiDicServices";
      this.tsmiDicServices.Size = new Size(261, 22);
      this.tsmiDicServices.Text = "Службы";
      this.tsmiDicServices.Click += new EventHandler(this.tsmiDicServices_Click);
      this.tsmiBanks.Name = "tsmiBanks";
      this.tsmiBanks.Size = new Size(261, 22);
      this.tsmiBanks.Text = "Банки";
      this.tsmiBanks.Click += new EventHandler(this.tsmiBanks_Click);
      this.tsmiOrganizations.Name = "tsmiOrganizations";
      this.tsmiOrganizations.Size = new Size(261, 22);
      this.tsmiOrganizations.Text = "Организации";
      this.tsmiOrganizations.Click += new EventHandler(this.tsmiOrganizations_Click);
      this.tsmiTypeLocationCounter.Name = "tsmiTypeLocationCounter";
      this.tsmiTypeLocationCounter.Size = new Size(261, 22);
      this.tsmiTypeLocationCounter.Text = "Типы расположения счетчиков";
      this.tsmiTypeLocationCounter.Click += new EventHandler(this.tsmiTypeLocationCounter_Click);
      this.tsmiTypeSeals.Name = "tsmiTypeSeals";
      this.tsmiTypeSeals.Size = new Size(261, 22);
      this.tsmiTypeSeals.Text = "Типы пломб на счетчиках";
      this.tsmiTypeSeals.Click += new EventHandler(this.tsmiTypeSeals_Click);
      this.tsmiTypeBindingServices.Name = "tsmiTypeBindingServices";
      this.tsmiTypeBindingServices.Size = new Size(261, 22);
      this.tsmiTypeBindingServices.Text = "Типы связывания услуг";
      this.tsmiTypeBindingServices.Visible = false;
      this.tsmiTypeBindingServices.Click += new EventHandler(this.tsmiTypeBindingServices_Click);
      this.tsmiBindingServices.Name = "tsmiBindingServices";
      this.tsmiBindingServices.Size = new Size(261, 22);
      this.tsmiBindingServices.Text = "Связанные услуги";
      this.tsmiBindingServices.Click += new EventHandler(this.tsmiBindingServices_Click);
      this.tsmiTypeNoteBook.Name = "tsmiTypeNoteBook";
      this.tsmiTypeNoteBook.Size = new Size(261, 22);
      this.tsmiTypeNoteBook.Text = "Типы записей в блокноте";
      this.tsmiTypeNoteBook.Click += new EventHandler(this.tsmiTypeNoteBook_Click);
      this.tsmiGuilds.Name = "tsmiGuilds";
      this.tsmiGuilds.Size = new Size(261, 22);
      this.tsmiGuilds.Text = "Цехи";
      this.tsmiGuilds.Click += new EventHandler(this.tsmiGuilds_Click);
      this.tsmiContractOrganization.Name = "tsmiContractOrganization";
      this.tsmiContractOrganization.Size = new Size(261, 22);
      this.tsmiContractOrganization.Text = "Договоры организаций";
      this.tsmiContractOrganization.Click += new EventHandler(this.tsmiContractOrganization_Click);
      this.tsmiSprAccounts.Name = "tsmiSprAccounts";
      this.tsmiSprAccounts.Size = new Size(261, 22);
      this.tsmiSprAccounts.Text = "Счета";
      this.tsmiSprAccounts.Click += new EventHandler(this.tsmiSprAccounts_Click);
      this.tsmiTypeAccount.Name = "tsmiTypeAccount";
      this.tsmiTypeAccount.Size = new Size(261, 22);
      this.tsmiTypeAccount.Text = "Типы счетов";
      this.tsmiTypeAccount.Click += new EventHandler(this.tsmiTypeAccount_Click);
      this.tsmiRcptAccount.Name = "tsmiRcptAccount";
      this.tsmiRcptAccount.Size = new Size(261, 22);
      this.tsmiRcptAccount.Text = "Квитанции и счета";
      this.tsmiRcptAccount.Click += new EventHandler(this.tsmiRcptAccount_Click);
      this.tsmiTools.Name = "tsmiTools";
      this.tsmiTools.Size = new Size(81, 20);
      this.tsmiTools.Text = "Настройки";
      this.tsmiTools.Click += new EventHandler(this.tsmiTools_Click);
      this.tsmiReports.Name = "tsmiReports";
      this.tsmiReports.Size = new Size(64, 20);
      this.tsmiReports.Text = "Отчеты";
      this.tsmiReports.Click += new EventHandler(this.tsmiReports_Click);
      this.tsmiPayment.Name = "tsmiPayment";
      this.tsmiPayment.Size = new Size(72, 20);
      this.tsmiPayment.Text = "Платежи";
      this.tsmiPayment.Click += new EventHandler(this.tsmiPayment_Click);
      this.tsmiCounters.Name = "tsmiCounters";
      this.tsmiCounters.Size = new Size(75, 20);
      this.tsmiCounters.Text = "Счетчики";
      this.tsmiCounters.Click += new EventHandler(this.tsmiCounters_Click);
      this.tsmiCalculation.Name = "tsmiCalculation";
      this.tsmiCalculation.Size = new Size(60, 20);
      this.tsmiCalculation.Text = "Расчет";
      this.tsmiCalculation.Click += new EventHandler(this.tsmiCalculation_Click);
      this.tsmiReceipts.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsmiOldReceipt,
        (ToolStripItem) this.tsmiNewReceipt
      });
      this.tsmiReceipts.Name = "tsmiReceipts";
      this.tsmiReceipts.Size = new Size((int) sbyte.MaxValue, 20);
      this.tsmiReceipts.Tag = (object) "100";
      this.tsmiReceipts.Text = "Печать квитанций";
      this.tsmiOldReceipt.Name = "tsmiOldReceipt";
      this.tsmiOldReceipt.Size = new Size(155, 22);
      this.tsmiOldReceipt.Tag = (object) "100";
      this.tsmiOldReceipt.Text = "Прежняя";
      this.tsmiOldReceipt.Click += new EventHandler(this.tsmiReceipts_Click);
      this.tsmiNewReceipt.Name = "tsmiNewReceipt";
      this.tsmiNewReceipt.Size = new Size(155, 22);
      this.tsmiNewReceipt.Tag = (object) "32";
      this.tsmiNewReceipt.Text = "Обновленная";
      this.tsmiNewReceipt.Click += new EventHandler(this.tsmiNewReceipt_Click);
      this.tsmiPrintBill.Name = "tsmiPrintBill";
      this.tsmiPrintBill.Size = new Size(106, 20);
      this.tsmiPrintBill.Tag = (object) "110";
      this.tsmiPrintBill.Text = "Печать счетов";
      this.tsmiPrintBill.Click += new EventHandler(this.tsmiPrintBill_Click);
      this.tsmiSearch.DropDownItems.AddRange(new ToolStripItem[7]
      {
        (ToolStripItem) this.tsmiFastSearch,
        (ToolStripItem) this.tsmiFlatSearch,
        (ToolStripItem) this.tsmiAdvancedSearch,
        (ToolStripItem) this.tsmiStreetSearch,
        (ToolStripItem) this.tsmiContractSearch,
        (ToolStripItem) this.tsmiPersonenNumberSearch,
        (ToolStripItem) this.tsmiSnilsSearch
      });
      this.tsmiSearch.Name = "tsmiSearch";
      this.tsmiSearch.Size = new Size(54, 20);
      this.tsmiSearch.Text = "Поиск";
      this.tsmiFastSearch.Name = "tsmiFastSearch";
      this.tsmiFastSearch.ShortcutKeys = Keys.F2;
      this.tsmiFastSearch.Size = new Size(269, 22);
      this.tsmiFastSearch.Text = "Быстрый поиск";
      this.tsmiFastSearch.Click += new EventHandler(this.tsmiFastSearch_Click);
      this.tsmiFlatSearch.Name = "tsmiFlatSearch";
      this.tsmiFlatSearch.ShortcutKeys = Keys.F3;
      this.tsmiFlatSearch.Size = new Size(269, 22);
      this.tsmiFlatSearch.Text = "Поиск по номеру квартиры";
      this.tsmiFlatSearch.Click += new EventHandler(this.tsmiFlatSearch_Click);
      this.tsmiAdvancedSearch.Name = "tsmiAdvancedSearch";
      this.tsmiAdvancedSearch.ShortcutKeys = Keys.F4;
      this.tsmiAdvancedSearch.Size = new Size(269, 22);
      this.tsmiAdvancedSearch.Text = "Расширенный поиск";
      this.tsmiAdvancedSearch.Click += new EventHandler(this.tsmiAdvancedSearch_Click);
      this.tsmiStreetSearch.Name = "tsmiStreetSearch";
      this.tsmiStreetSearch.ShortcutKeys = Keys.F5;
      this.tsmiStreetSearch.Size = new Size(269, 22);
      this.tsmiStreetSearch.Text = "Поиск по улице и дому";
      this.tsmiStreetSearch.Visible = false;
      this.tsmiStreetSearch.Click += new EventHandler(this.tsmiStreetSearch_Click);
      this.tsmiContractSearch.Name = "tsmiContractSearch";
      this.tsmiContractSearch.ShortcutKeys = Keys.F6;
      this.tsmiContractSearch.Size = new Size(269, 22);
      this.tsmiContractSearch.Text = "Поиск договора";
      this.tsmiContractSearch.Click += new EventHandler(this.tsmiContractSearch_Click);
      this.tsmiPersonenNumberSearch.Name = "tsmiPersonenNumberSearch";
      this.tsmiPersonenNumberSearch.ShortcutKeys = Keys.F7;
      this.tsmiPersonenNumberSearch.Size = new Size(269, 22);
      this.tsmiPersonenNumberSearch.Tag = (object) "1";
      this.tsmiPersonenNumberSearch.Text = "Поиск по табельному номеру";
      this.tsmiPersonenNumberSearch.Click += new EventHandler(this.tsmiPersonenNumberOrSnilsSearch_Click);
      this.tsmiSnilsSearch.Name = "tsmiSnilsSearch";
      this.tsmiSnilsSearch.ShortcutKeys = Keys.F8;
      this.tsmiSnilsSearch.Size = new Size(269, 22);
      this.tsmiSnilsSearch.Tag = (object) "2";
      this.tsmiSnilsSearch.Text = "Поиск по снилс";
      this.tsmiSnilsSearch.Click += new EventHandler(this.tsmiPersonenNumberOrSnilsSearch_Click);
      this.tsmiPrintPolicy.Name = "tsmiPrintPolicy";
      this.tsmiPrintPolicy.Size = new Size(114, 20);
      this.tsmiPrintPolicy.Text = "Печать полисов";
      this.tsmiPrintPolicy.Visible = false;
      this.tsmiPrintPolicy.Click += new EventHandler(this.tsmiPrintPolicy_Click);
      this.tsmiUZP.Name = "tsmiUZP";
      this.tsmiUZP.Size = new Size(42, 20);
      this.tsmiUZP.Text = "УЗП";
      this.tsmiUZP.Visible = false;
      this.tsmiUZP.Click += new EventHandler(this.tsmiUzp_Click);
      this.tsmiCalculationPeriod.DropDownItems.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.tsmiPrimaryDocuments,
        (ToolStripItem) this.tsmiCharge,
        (ToolStripItem) this.tsmiCalcPerPayment,
        (ToolStripItem) this.tsmiPeni,
        (ToolStripItem) this.tsmiSocialProtection
      });
      this.tsmiCalculationPeriod.Name = "tsmiCalculationPeriod";
      this.tsmiCalculationPeriod.Size = new Size(129, 20);
      this.tsmiCalculationPeriod.Text = "Расчётный период";
      this.tsmiPrimaryDocuments.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsmiPrimaryDocCloseMonth,
        (ToolStripItem) this.tsmiPrimaryDocOpenMonth
      });
      this.tsmiPrimaryDocuments.Name = "tsmiPrimaryDocuments";
      this.tsmiPrimaryDocuments.Size = new Size(244, 22);
      this.tsmiPrimaryDocuments.Text = "Ввод первичных документов";
      this.tsmiPrimaryDocCloseMonth.Name = "tsmiPrimaryDocCloseMonth";
      this.tsmiPrimaryDocCloseMonth.Size = new Size(165, 22);
      this.tsmiPrimaryDocCloseMonth.Text = "Закрыть месяц";
      this.tsmiPrimaryDocCloseMonth.Click += new EventHandler(this.tsmiPrimaryDocCloseMonth_Click);
      this.tsmiPrimaryDocOpenMonth.Name = "tsmiPrimaryDocOpenMonth";
      this.tsmiPrimaryDocOpenMonth.Size = new Size(165, 22);
      this.tsmiPrimaryDocOpenMonth.Text = "Открыть месяц";
      this.tsmiPrimaryDocOpenMonth.Click += new EventHandler(this.tsmiPrimaryDocOpenMonth_Click);
      this.tsmiCharge.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsmiChargeCloseMonth,
        (ToolStripItem) this.tsmiChargeOpenMonth
      });
      this.tsmiCharge.Name = "tsmiCharge";
      this.tsmiCharge.Size = new Size(244, 22);
      this.tsmiCharge.Text = "Начисления";
      this.tsmiChargeCloseMonth.Name = "tsmiChargeCloseMonth";
      this.tsmiChargeCloseMonth.Size = new Size(165, 22);
      this.tsmiChargeCloseMonth.Text = "Закрыть месяц";
      this.tsmiChargeCloseMonth.Click += new EventHandler(this.tsmiChargeCloseMonth_Click);
      this.tsmiChargeOpenMonth.Name = "tsmiChargeOpenMonth";
      this.tsmiChargeOpenMonth.Size = new Size(165, 22);
      this.tsmiChargeOpenMonth.Text = "Открыть месяц";
      this.tsmiChargeOpenMonth.Click += new EventHandler(this.tsmiChargeOpenMonth_Click);
      this.tsmiCalcPerPayment.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsmiCalcPerPaymentCloseMonth,
        (ToolStripItem) this.tsmiCalcPerPaymentOpenMonth
      });
      this.tsmiCalcPerPayment.Name = "tsmiCalcPerPayment";
      this.tsmiCalcPerPayment.Size = new Size(244, 22);
      this.tsmiCalcPerPayment.Text = "Оплата";
      this.tsmiCalcPerPaymentCloseMonth.Name = "tsmiCalcPerPaymentCloseMonth";
      this.tsmiCalcPerPaymentCloseMonth.Size = new Size(165, 22);
      this.tsmiCalcPerPaymentCloseMonth.Text = "Закрыть месяц";
      this.tsmiCalcPerPaymentCloseMonth.Click += new EventHandler(this.tsmiCalcPerPaymentCloseMonth_Click);
      this.tsmiCalcPerPaymentOpenMonth.Name = "tsmiCalcPerPaymentOpenMonth";
      this.tsmiCalcPerPaymentOpenMonth.Size = new Size(165, 22);
      this.tsmiCalcPerPaymentOpenMonth.Text = "Открыть месяц";
      this.tsmiCalcPerPaymentOpenMonth.Click += new EventHandler(this.tsmiCalcPerPaymentOpenMonth_Click);
      this.tsmiPeni.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsmiPeniCloseMonth,
        (ToolStripItem) this.tsmiPeniOpenMonth
      });
      this.tsmiPeni.Name = "tsmiPeni";
      this.tsmiPeni.Size = new Size(244, 22);
      this.tsmiPeni.Text = "Пени";
      this.tsmiPeniCloseMonth.Name = "tsmiPeniCloseMonth";
      this.tsmiPeniCloseMonth.Size = new Size(165, 22);
      this.tsmiPeniCloseMonth.Text = "Закрыть месяц";
      this.tsmiPeniCloseMonth.Click += new EventHandler(this.tsmiPeniCloseMonth_Click);
      this.tsmiPeniOpenMonth.Name = "tsmiPeniOpenMonth";
      this.tsmiPeniOpenMonth.Size = new Size(165, 22);
      this.tsmiPeniOpenMonth.Text = "Открыть месяц";
      this.tsmiPeniOpenMonth.Click += new EventHandler(this.tsmiPeniOpenMonth_Click);
      this.tsmiSocialProtection.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsmiSocialProtectionCloseMonth,
        (ToolStripItem) this.tsmiSocialProtectionOpenMonth
      });
      this.tsmiSocialProtection.Name = "tsmiSocialProtection";
      this.tsmiSocialProtection.Size = new Size(244, 22);
      this.tsmiSocialProtection.Text = "Соцзащита";
      this.tsmiSocialProtectionCloseMonth.Name = "tsmiSocialProtectionCloseMonth";
      this.tsmiSocialProtectionCloseMonth.Size = new Size(165, 22);
      this.tsmiSocialProtectionCloseMonth.Text = "Закрыть месяц";
      this.tsmiSocialProtectionCloseMonth.Click += new EventHandler(this.tsmiSocialProtectionCloseMonth_Click);
      this.tsmiSocialProtectionOpenMonth.Name = "tsmiSocialProtectionOpenMonth";
      this.tsmiSocialProtectionOpenMonth.Size = new Size(165, 22);
      this.tsmiSocialProtectionOpenMonth.Text = "Открыть месяц";
      this.tsmiSocialProtectionOpenMonth.Click += new EventHandler(this.tsmiSocialProtectionOpenMonth_Click);
      this.tsmiHouseAdministration.DropDownItems.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.tsmiRecvisitsAndParameters,
        (ToolStripItem) this.tsmiOperations,
        (ToolStripItem) this.tsmiNoteBook,
        (ToolStripItem) this.tsmiEnterTariffs,
        (ToolStripItem) this.tsmiInspectionContracts
      });
      this.tsmiHouseAdministration.Enabled = false;
      this.tsmiHouseAdministration.Name = "tsmiHouseAdministration";
      this.tsmiHouseAdministration.Size = new Size(120, 20);
      this.tsmiHouseAdministration.Text = "Домоуправление";
      this.tsmiRecvisitsAndParameters.Name = "tsmiRecvisitsAndParameters";
      this.tsmiRecvisitsAndParameters.Size = new Size(218, 22);
      this.tsmiRecvisitsAndParameters.Text = "Реквизиты и параметры";
      this.tsmiRecvisitsAndParameters.Click += new EventHandler(this.tsmiRecvisitsAndParameters_Click);
      this.tsmiOperations.Name = "tsmiOperations";
      this.tsmiOperations.Size = new Size(218, 22);
      this.tsmiOperations.Text = "Операции";
      this.tsmiOperations.Click += new EventHandler(this.tsmiOperations_Click);
      this.tsmiNoteBook.Name = "tsmiNoteBook";
      this.tsmiNoteBook.Size = new Size(218, 22);
      this.tsmiNoteBook.Text = "Блокнот бухгалтера";
      this.tsmiNoteBook.Click += new EventHandler(this.tsmiNoteBook_Click);
      this.tsmiEnterTariffs.Name = "tsmiEnterTariffs";
      this.tsmiEnterTariffs.Size = new Size(218, 22);
      this.tsmiEnterTariffs.Text = "Ввод тарифов";
      this.tsmiEnterTariffs.Click += new EventHandler(this.tsmiEnterTariffs_Click);
      this.tsmiInspectionContracts.Name = "tsmiInspectionContracts";
      this.tsmiInspectionContracts.Size = new Size(218, 22);
      this.tsmiInspectionContracts.Text = "Проверка договоров";
      this.tsmiInspectionContracts.Click += new EventHandler(this.tsmiInspectionContracts_Click);
      this.tsmiHome.DropDownItems.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.tsmiHomeParametrs,
        (ToolStripItem) this.tsmiMakeEntrance,
        (ToolStripItem) this.tsmiNewLsClient,
        (ToolStripItem) this.tsmiLsClientClose
      });
      this.tsmiHome.Name = "tsmiHome";
      this.tsmiHome.Size = new Size(44, 20);
      this.tsmiHome.Text = "Дом";
      this.tsmiHomeParametrs.Name = "tsmiHomeParametrs";
      this.tsmiHomeParametrs.Size = new Size(296, 22);
      this.tsmiHomeParametrs.Text = "Характеристики, параметры, службы";
      this.tsmiHomeParametrs.Click += new EventHandler(this.tsmiHomeParametrs_Click);
      this.tsmiMakeEntrance.Name = "tsmiMakeEntrance";
      this.tsmiMakeEntrance.Size = new Size(296, 22);
      this.tsmiMakeEntrance.Text = "Проставить подъезды";
      this.tsmiMakeEntrance.Click += new EventHandler(this.tsmiMakeEntrance_Click);
      this.tsmiNewLsClient.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsmiLsKvartplata,
        (ToolStripItem) this.tsmiLsArenda
      });
      this.tsmiNewLsClient.Name = "tsmiNewLsClient";
      this.tsmiNewLsClient.Size = new Size(296, 22);
      this.tsmiNewLsClient.Text = "Создать";
      this.tsmiNewLsClient.Click += new EventHandler(this.tsmiNewLsClient_Click);
      this.tsmiLsKvartplata.Name = "tsmiLsKvartplata";
      this.tsmiLsKvartplata.Size = new Size(202, 22);
      this.tsmiLsKvartplata.Tag = (object) "100";
      this.tsmiLsKvartplata.Text = "Лицевые квартплаты";
      this.tsmiLsKvartplata.Click += new EventHandler(this.tsmiCreateNewLsClient_Click);
      this.tsmiLsArenda.Name = "tsmiLsArenda";
      this.tsmiLsArenda.Size = new Size(202, 22);
      this.tsmiLsArenda.Tag = (object) "110";
      this.tsmiLsArenda.Text = "Лицевые аренды";
      this.tsmiLsArenda.Click += new EventHandler(this.tsmiCreateNewLsClient_Click);
      this.tsmiLsClientClose.Name = "tsmiLsClientClose";
      this.tsmiLsClientClose.Size = new Size(296, 22);
      this.tsmiLsClientClose.Text = "Закрыть";
      this.tsmiLsClientClose.Click += new EventHandler(this.tsmiLsClientClose_Click);
      this.tsmiLoadUnLoad.DropDownItems.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.tsmiLoadFacilities,
        (ToolStripItem) this.tsmiInsuranceUnLoad,
        (ToolStripItem) this.tsmiLoadPayments,
        (ToolStripItem) this.tsmiDataExchangeSocialProtection,
        (ToolStripItem) this.tsmiL_U_hmReceipt
      });
      this.tsmiLoadUnLoad.Name = "tsmiLoadUnLoad";
      this.tsmiLoadUnLoad.Size = new Size(137, 20);
      this.tsmiLoadUnLoad.Text = "Подгрузка/выгрузка";
      this.tsmiLoadFacilities.Enabled = false;
      this.tsmiLoadFacilities.Name = "tsmiLoadFacilities";
      this.tsmiLoadFacilities.Size = new Size((int) byte.MaxValue, 22);
      this.tsmiLoadFacilities.Text = "Подкачка льгот";
      this.tsmiLoadFacilities.Click += new EventHandler(this.tsmiLoadFacilities_Click);
      this.tsmiInsuranceUnLoad.Enabled = false;
      this.tsmiInsuranceUnLoad.Name = "tsmiInsuranceUnLoad";
      this.tsmiInsuranceUnLoad.Size = new Size((int) byte.MaxValue, 22);
      this.tsmiInsuranceUnLoad.Text = "Выгрузка данных о страховке";
      this.tsmiInsuranceUnLoad.Click += new EventHandler(this.tsmiInsuranceUnLoad_Click);
      this.tsmiLoadPayments.Enabled = false;
      this.tsmiLoadPayments.Name = "tsmiLoadPayments";
      this.tsmiLoadPayments.Size = new Size((int) byte.MaxValue, 22);
      this.tsmiLoadPayments.Text = "Закачка платежей";
      this.tsmiLoadPayments.Click += new EventHandler(this.tsmiLoadPayments_Click);
      this.tsmiDataExchangeSocialProtection.Name = "tsmiDataExchangeSocialProtection";
      this.tsmiDataExchangeSocialProtection.Size = new Size((int) byte.MaxValue, 22);
      this.tsmiDataExchangeSocialProtection.Text = "Обмен данными с соцзащитой";
      this.tsmiDataExchangeSocialProtection.Click += new EventHandler(this.tsmiDataExchangeSocialProtection_Click);
      this.tsmiL_U_hmReceipt.Name = "tsmiL_U_hmReceipt";
      this.tsmiL_U_hmReceipt.Size = new Size((int) byte.MaxValue, 22);
      this.tsmiL_U_hmReceipt.Text = "Квитанций на Email";
      this.tsmiL_U_hmReceipt.Visible = false;
      this.tsmiL_U_hmReceipt.Click += new EventHandler(this.tsmiL_U_hmReceipt_Click);
      this.tsmiExternalData.Name = "tsmiExternalData";
      this.tsmiExternalData.Size = new Size(120, 20);
      this.tsmiExternalData.Text = "Внешние данные";
      this.tsmiExternalData.Visible = false;
      this.tsmiExternalData.Click += new EventHandler(this.tsmiExternalData_Click);
      this.tsmiAbout.Name = "tsmiAbout";
      this.tsmiAbout.Size = new Size(96, 20);
      this.tsmiAbout.Text = "О программе";
      this.tsmiAbout.Click += new EventHandler(this.tsmiAbout_Click);
      this.tsmiHelp.Name = "tsmiHelp";
      this.tsmiHelp.ShortcutKeys = Keys.F1;
      this.tsmiHelp.Size = new Size(69, 20);
      this.tsmiHelp.Text = "Справка";
      this.tsmiHelp.Visible = false;
      this.tsmiHelp.Click += new EventHandler(this.tsmiHelp_Click);
      this.tsmiExecute.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.tsmiExecuteScript
      });
      this.tsmiExecute.Name = "tsmiExecute";
      this.tsmiExecute.Size = new Size(88, 20);
      this.tsmiExecute.Text = "Выполнить ";
      this.tsmiExecuteScript.Name = "tsmiExecuteScript";
      this.tsmiExecuteScript.Size = new Size(183, 22);
      this.tsmiExecuteScript.Text = "Выполнить скрипт";
      this.tsmiExecuteScript.Click += new EventHandler(this.tsmiExecuteScript_Click);
      this.капитальныйРемонтToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.tsmiCheckOverhaul
      });
      this.капитальныйРемонтToolStripMenuItem.Name = "капитальныйРемонтToolStripMenuItem";
      this.капитальныйРемонтToolStripMenuItem.Size = new Size(144, 20);
      this.капитальныйРемонтToolStripMenuItem.Text = "Капитальный ремонт";
      this.tsmiCheckOverhaul.Name = "tsmiCheckOverhaul";
      this.tsmiCheckOverhaul.Size = new Size(323, 22);
      this.tsmiCheckOverhaul.Text = "Проверка лицевых капитального ремонта";
      this.tsmiCheckOverhaul.Click += new EventHandler(this.tsmiCheckOverhaul_Click);
      this.asdaToolStripMenuItem.Name = "asdaToolStripMenuItem";
      this.asdaToolStripMenuItem.Size = new Size(132, 20);
      this.asdaToolStripMenuItem.Text = "Выгрузка пинкодов";
      this.asdaToolStripMenuItem.Visible = false;
      this.asdaToolStripMenuItem.Click += new EventHandler(this.asdaToolStripMenuItem_Click);
      this.pnBtn.Controls.Add((Control) this.btnWOW);
      this.pnBtn.Controls.Add((Control) this.pbar);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new System.Drawing.Point(0, 500);
      this.pnBtn.Margin = new Padding(4);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(1055, 40);
      this.pnBtn.TabIndex = 3;
      this.btnWOW.Location = new System.Drawing.Point(178, 5);
      this.btnWOW.Name = "btnWOW";
      this.btnWOW.Size = new Size(57, 30);
      this.btnWOW.TabIndex = 2;
      this.btnWOW.Text = "WOW";
      this.btnWOW.UseVisualStyleBackColor = true;
      this.btnWOW.Visible = false;
      this.btnWOW.Click += new EventHandler(this.btnWOW_Click);
      this.pbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pbar.Location = new System.Drawing.Point(849, 5);
      this.pbar.Name = "pbar";
      this.pbar.Size = new Size(100, 23);
      this.pbar.Style = ProgressBarStyle.Continuous;
      this.pbar.TabIndex = 1;
      this.pbar.Visible = false;
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new System.Drawing.Point(956, 5);
      this.btnExit.Margin = new Padding(4);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(83, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.pnUp.Controls.Add((Control) this.lblPriorClosed);
      this.pnUp.Controls.Add((Control) this.lblPrior);
      this.pnUp.Controls.Add((Control) this.lblKvr);
      this.pnUp.Controls.Add((Control) this.lblOsznClosed);
      this.pnUp.Controls.Add((Control) this.lblOszn);
      this.pnUp.Controls.Add((Control) this.mpCurrentPeriod);
      this.pnUp.Controls.Add((Control) this.lblPeni);
      this.pnUp.Controls.Add((Control) this.lblPasp);
      this.pnUp.Controls.Add((Control) this.lblKv);
      this.pnUp.Controls.Add((Control) this.lblPeniClosed);
      this.pnUp.Controls.Add((Control) this.lblPaspClosed);
      this.pnUp.Controls.Add((Control) this.lblKvrCloseValue);
      this.pnUp.Controls.Add((Control) this.lblKvrClose);
      this.pnUp.Controls.Add((Control) this.lblCaption);
      this.pnUp.Controls.Add((Control) this.btnUp);
      this.pnUp.Dock = DockStyle.Top;
      this.pnUp.Location = new System.Drawing.Point(0, 44);
      this.pnUp.Margin = new Padding(4);
      this.pnUp.Name = "pnUp";
      this.pnUp.Size = new Size(1055, 56);
      this.pnUp.TabIndex = 5;
      this.lblPriorClosed.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblPriorClosed.AutoSize = true;
      this.lblPriorClosed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.lblPriorClosed.ForeColor = SystemColors.ControlText;
      this.lblPriorClosed.Location = new System.Drawing.Point(625, 36);
      this.lblPriorClosed.Name = "lblPriorClosed";
      this.lblPriorClosed.Size = new Size(36, 16);
      this.lblPriorClosed.TabIndex = 21;
      this.lblPriorClosed.Text = "Prior";
      this.lblPrior.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblPrior.AutoSize = true;
      this.lblPrior.ForeColor = SystemColors.ControlText;
      this.lblPrior.Location = new System.Drawing.Point(567, 36);
      this.lblPrior.Name = "lblPrior";
      this.lblPrior.Size = new Size(61, 16);
      this.lblPrior.TabIndex = 20;
      this.lblPrior.Text = "Первич.";
      this.lblKvr.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblKvr.AutoSize = true;
      this.lblKvr.ForeColor = SystemColors.ControlText;
      this.lblKvr.Location = new System.Drawing.Point(773, 36);
      this.lblKvr.Name = "lblKvr";
      this.lblKvr.Size = new Size(37, 16);
      this.lblKvr.TabIndex = 19;
      this.lblKvr.Text = "Опл.";
      this.lblOsznClosed.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblOsznClosed.AutoSize = true;
      this.lblOsznClosed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.lblOsznClosed.ForeColor = SystemColors.ControlText;
      this.lblOsznClosed.Location = new System.Drawing.Point(1003, 36);
      this.lblOsznClosed.Name = "lblOsznClosed";
      this.lblOsznClosed.Size = new Size(38, 16);
      this.lblOsznClosed.TabIndex = 18;
      this.lblOsznClosed.Text = "Oszn";
      this.lblOszn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblOszn.AutoSize = true;
      this.lblOszn.ForeColor = SystemColors.ControlText;
      this.lblOszn.Location = new System.Drawing.Point(960, 36);
      this.lblOszn.Name = "lblOszn";
      this.lblOszn.Size = new Size(46, 16);
      this.lblOszn.TabIndex = 17;
      this.lblOszn.Text = "ОСЗН";
      this.mpCurrentPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.mpCurrentPeriod.CustomFormat = "MMMM yyyy";
      this.mpCurrentPeriod.Format = DateTimePickerFormat.Custom;
      this.mpCurrentPeriod.Location = new System.Drawing.Point(899, 8);
      this.mpCurrentPeriod.MinDate = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.mpCurrentPeriod.Name = "mpCurrentPeriod";
      this.mpCurrentPeriod.OldMonth = 0;
      this.mpCurrentPeriod.ShowUpDown = true;
      this.mpCurrentPeriod.Size = new Size(140, 22);
      this.mpCurrentPeriod.TabIndex = 16;
      this.mpCurrentPeriod.ValueChanged += new EventHandler(this.dtmpCurrentPeriod_ValueChanged);
      this.lblPeni.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblPeni.AutoSize = true;
      this.lblPeni.ForeColor = SystemColors.ControlText;
      this.lblPeni.Location = new System.Drawing.Point(862, 36);
      this.lblPeni.Name = "lblPeni";
      this.lblPeni.Size = new Size(42, 16);
      this.lblPeni.TabIndex = 15;
      this.lblPeni.Text = "Пени";
      this.lblPasp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblPasp.AutoSize = true;
      this.lblPasp.ForeColor = SystemColors.ControlText;
      this.lblPasp.Location = new System.Drawing.Point(683, 36);
      this.lblPasp.Name = "lblPasp";
      this.lblPasp.Size = new Size(37, 16);
      this.lblPasp.TabIndex = 14;
      this.lblPasp.Text = "Нач.";
      this.lblKv.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblKv.AutoSize = true;
      this.lblKv.Location = new System.Drawing.Point(810, 11);
      this.lblKv.Name = "lblKv";
      this.lblKv.Size = new Size(56, 16);
      this.lblKv.TabIndex = 13;
      this.lblKv.Text = "cvcvcxv";
      this.lblKv.Visible = false;
      this.lblPeniClosed.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblPeniClosed.AutoSize = true;
      this.lblPeniClosed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.lblPeniClosed.ForeColor = SystemColors.ControlText;
      this.lblPeniClosed.Location = new System.Drawing.Point(904, 36);
      this.lblPeniClosed.Name = "lblPeniClosed";
      this.lblPeniClosed.Size = new Size(35, 16);
      this.lblPeniClosed.TabIndex = 12;
      this.lblPeniClosed.Text = "Peni";
      this.lblPaspClosed.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblPaspClosed.AutoSize = true;
      this.lblPaspClosed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.lblPaspClosed.ForeColor = SystemColors.ControlText;
      this.lblPaspClosed.Location = new System.Drawing.Point(717, 36);
      this.lblPaspClosed.Name = "lblPaspClosed";
      this.lblPaspClosed.Size = new Size(40, 16);
      this.lblPaspClosed.TabIndex = 11;
      this.lblPaspClosed.Text = "Pasp";
      this.lblKvrCloseValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblKvrCloseValue.AutoSize = true;
      this.lblKvrCloseValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.lblKvrCloseValue.ForeColor = SystemColors.ControlText;
      this.lblKvrCloseValue.Location = new System.Drawing.Point(807, 36);
      this.lblKvrCloseValue.Name = "lblKvrCloseValue";
      this.lblKvrCloseValue.Size = new Size(49, 16);
      this.lblKvrCloseValue.TabIndex = 10;
      this.lblKvrCloseValue.Text = "Kvartpl";
      this.lblKvrClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblKvrClose.AutoSize = true;
      this.lblKvrClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblKvrClose.Location = new System.Drawing.Point(473, 36);
      this.lblKvrClose.Name = "lblKvrClose";
      this.lblKvrClose.Size = new Size(100, 16);
      this.lblKvrClose.TabIndex = 9;
      this.lblKvrClose.Text = "Закр. месяц:";
      this.lblCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblCaption.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblCaption.Location = new System.Drawing.Point(46, 6);
      this.lblCaption.Margin = new Padding(4, 0, 4, 0);
      this.lblCaption.Name = "lblCaption";
      this.lblCaption.Size = new Size(629, 46);
      this.lblCaption.TabIndex = 6;
      this.btnUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.btnUp.Image = (Image) Resources.up_one_level1;
      this.btnUp.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnUp.Location = new System.Drawing.Point(4, 3);
      this.btnUp.Margin = new Padding(4);
      this.btnUp.Name = "btnUp";
      this.btnUp.Size = new Size(34, 33);
      this.btnUp.TabIndex = 2;
      this.btnUp.TextAlign = ContentAlignment.MiddleRight;
      this.btnUp.UseVisualStyleBackColor = true;
      this.btnUp.Click += new EventHandler(this.btnUp_Click);
      this.dgvMainList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.dgvMainList.BackgroundColor = Color.AliceBlue;
      this.dgvMainList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvMainList.Dock = DockStyle.Fill;
      this.hp.SetHelpKeyword((Control) this.dgvMainList, "kv31.html");
      this.hp.SetHelpNavigator((Control) this.dgvMainList, HelpNavigator.Topic);
      this.dgvMainList.Location = new System.Drawing.Point(0, 100);
      this.dgvMainList.Margin = new Padding(4);
      this.dgvMainList.Name = "dgvMainList";
      this.dgvMainList.ReadOnly = true;
      this.hp.SetShowHelp((Control) this.dgvMainList, true);
      this.dgvMainList.Size = new Size(1055, 400);
      this.dgvMainList.TabIndex = 6;
      this.dgvMainList.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvMainList_CellFormatting_1);
      this.dgvMainList.CellMouseClick += new DataGridViewCellMouseEventHandler(this.dgvMainList_CellMouseClick);
      this.dgvMainList.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dgvMainList_CellMouseDoubleClick);
      this.dgvMainList.DataError += new DataGridViewDataErrorEventHandler(this.dgvMainList_DataError);
      this.dgvMainList.KeyDown += new KeyEventHandler(this.dgvMainList_KeyDown);
      this.dgvMainList.KeyUp += new KeyEventHandler(this.dgvMainList_KeyUp);
      this.hp.HelpNamespace = "Help.chm";
      this.sfCRT.Filter = "\"Файлы (*.crt)|*.crt\"";
      this.fdHelpLoad.Filter = "dbf files (*.dbf)|*.dbf";
      this.fdHelpLoad.InitialDirectory = "C:\\\\";
      this.sfdStrahUnLoad.Filter = "dbf files (*.dbf)|*.dbf";
      this.sfdStrahUnLoad.InitialDirectory = "C:\\\\";
      this.fdSQL.Filter = "sql files (*.sql)|*.sql";
      this.fdSQL.InitialDirectory = "C:\\\\";
      this.sfd.Filter = "txt files (*.txt)|*.txt";
      this.dtmpCurrentPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.dtmpCurrentPeriod.CustomFormat = "MMMM yyyy";
      this.dtmpCurrentPeriod.Format = DateTimePickerFormat.Custom;
      this.dtmpCurrentPeriod.Location = new System.Drawing.Point(764, 6);
      this.dtmpCurrentPeriod.MaxDate = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtmpCurrentPeriod.MinDate = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dtmpCurrentPeriod.Name = "dtmpCurrentPeriod";
      this.dtmpCurrentPeriod.OldMonth = 1;
      this.dtmpCurrentPeriod.ShowUpDown = true;
      this.dtmpCurrentPeriod.Size = new Size(141, 20);
      this.dtmpCurrentPeriod.TabIndex = 16;
      this.dtmpCurrentPeriod.ValueChanged += new EventHandler(this.dtmpCurrentPeriod_ValueChanged);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1055, 540);
      this.Controls.Add((Control) this.dgvMainList);
      this.Controls.Add((Control) this.pnUp);
      this.Controls.Add((Control) this.pnBtn);
      this.Controls.Add((Control) this.msMainMenu);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.hp.SetHelpKeyword((Control) this, "kv1.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      //this.Icon = (System.Drawing.Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.MainMenuStrip = this.msMainMenu;
      this.Margin = new Padding(4);
      this.Name = "MainForm";
      this.hp.SetShowHelp((Control) this, true);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Расчет жилищно-коммунальных платежей";
      this.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
      this.Shown += new EventHandler(this.MainForm_Shown);
      this.KeyDown += new KeyEventHandler(this.MainForm_KeyDown);
      this.msMainMenu.ResumeLayout(false);
      this.msMainMenu.PerformLayout();
      this.pnBtn.ResumeLayout(false);
      this.pnUp.ResumeLayout(false);
      this.pnUp.PerformLayout();
      ((ISupportInitialize) this.dgvMainList).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
