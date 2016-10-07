// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmPaymentOhl
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using Kvartplata.Smirnov.Forms;
using NHibernate;
using NHibernate.Criterion;
using SaveSettings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmPaymentOhl : Form
  {
    protected GridSettings MySettingsPayAddOhlGrid = new GridSettings();
    protected GridSettings MySettingsPayOhlGrid = new GridSettings();
    protected GridSettings MySettingsPayStornGrid = new GridSettings();
    private Kvartplata.Classes.FormStateSaver fss = new Kvartplata.Classes.FormStateSaver(FrmPaymentOhl.container);
    private IList<OhlPayment> ohlReverse = (IList<OhlPayment>) new List<OhlPayment>();
    private bool CorrcetHome = false;
    private bool ReversePay = false;
    private List<ListHome> _listHome = new List<ListHome>();
    private bool _checkUncheck = false;
    private bool PeriodPayChange = false;
    private bool PeriodChange = false;
    private Thread _thread = (Thread) null;
    private IContainer components = (IContainer) null;
    private static IContainer container;
    private readonly Kvartplata.Classes.LsClient _client;
    private ISession _session;
    private MessageWait messageWait;
    private GroupBox gbAttribute;
    private ComboBox cmbPayDoc;
    private Label lblPayDoc;
    private Kvartplata.Classes.MonthPicker mpPeriodPay;
    private Kvartplata.Classes.MonthPicker mpPeriod;
    private Label lblPeriodPay;
    private DateTimePicker dtmpDate;
    private Label lblDate;
    private ComboBox cmbPurpose;
    private Label lblPurpose;
    private ComboBox cmbSource;
    private Label lblSource;
    private Label lblPeriod;
    private Panel pnBtn;
    private Button btnSave;
    private Button btnExit;
    private DataGridView dgvPaymentsForAdd;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private Button btnClearRentPeni;
    private Button btnClearRent;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    private TabControl tcMain;
    private TabPage tbpView;
    private DataGridView dgvPayments;
    private TabPage tbpAdding;
    private GroupBox groupBox1;
    private Button btnFilter;
    private Label label4;
    private TextBox txbAccountFilter;
    private CheckBox chbNotEqualsSum;
    private TextBox txbBankFilter;
    private Label label1;
    private CheckBox chbRentNegative;
    private CheckBox chbPeniNegative;
    private Button reverseBut;
    private TabPage tabReverse;
    private DataGridView dvgReverse;
    private Button butSaveReverse;
    private Button butHmParam;
    private Panel panelHomeList;
    private Panel panelMain;
    private Panel panelYesNO;
    private Panel panelHmList;
    private DataGridView dvgHomeList;
    private Panel panelHeaderHm;
    private Button butCancelHmList;
    private Button butOkHmList;
    private Label labPay;
    private Label label2;
    private Button butCheckHome;
    private DataGridViewCheckBoxColumn check;
    private DataGridViewTextBoxColumn Address;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
    private Kvartplata.Classes.MaskDateColumn maskDateColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn21;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn22;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn23;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn24;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn25;
    private Kvartplata.Classes.MaskDateColumn maskDateColumn2;
    private DataGridViewTextBoxColumn Account;
    private DataGridViewTextBoxColumn Bank;
    private DataGridViewTextBoxColumn Rent;
    private DataGridViewTextBoxColumn RentPeni;
    private DataGridViewTextBoxColumn RentAll;
    private DataGridViewTextBoxColumn all;
    private DataGridViewTextBoxColumn RentForClients;
    private DataGridViewTextBoxColumn RentPeniForClient;
    private ContextMenuStrip cmsReverse;
    private ToolStripMenuItem TsMiReverse;
    private ContextMenuStrip cmsHomeList;
    private ToolStripMenuItem TsMiHomeList;
    private DataGridViewTextBoxColumn Account3;
    private DataGridViewTextBoxColumn BankName3;
    private DataGridViewTextBoxColumn Rent3;
    private DataGridViewTextBoxColumn RentPeni3;
    private DataGridViewTextBoxColumn RentAll3;
    private DataGridViewTextBoxColumn All3;
    private Kvartplata.Classes.MaskDateColumn maskDateColumn3;
    private Kvartplata.Classes.MaskDateColumn PeriodPay3;
    private Counters.Classes.MaskDateColumn Period1;
    private DataGridViewTextBoxColumn Account2;
    private DataGridViewTextBoxColumn Bank2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
    private DataGridViewTextBoxColumn Rentt;
    private DataGridViewTextBoxColumn Peni2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
    private Kvartplata.Classes.MaskDateColumn PaymentDate;
    private Kvartplata.Classes.MaskDateColumn PeriodPay;
    private Counters.Classes.MaskDateColumn Period;
    private DataGridViewTextBoxColumn RentForClients2;
    private DataGridViewTextBoxColumn RentPeniForClients2;
    private DataGridViewTextBoxColumn RentForClientsRev;
    private DataGridViewTextBoxColumn RentPeniForClientsRev;
    private Button butNullMinusPayment;

    public IList<OhlPayment> ohlPaymentsAdd { get; set; }

    public IList<OhlPayment> ohlPayments { get; set; }

    public IList<Payment> PaymentsAddList { get; set; }

    public FrmPaymentOhl(Kvartplata.Classes.LsClient client)
    {
      this._client = client;
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
      this._session = Kvartplata.Domain.CurrentSession;
      this.PaymentsAddList = (IList<Payment>) new List<Payment>();
      this.ohlPayments = (IList<OhlPayment>) new List<OhlPayment>();
      this.ohlPaymentsAdd = (IList<OhlPayment>) new List<OhlPayment>();
      this.panelHomeList.Width = 0;
    }

    [DllImport("Compute.dll", EntryPoint = "QRent", CharSet = CharSet.Ansi, SetLastError = true)]
    public static extern void Rent2(int period, [MarshalAs(UnmanagedType.LPStr)] string sclient, [MarshalAs(UnmanagedType.LPStr)] string connect, [MarshalAs(UnmanagedType.LPStr)] string UserS, [MarshalAs(UnmanagedType.LPStr)] string Passwd);

    private void FrmPaymentOhl_Load(object sender, EventArgs e)
    {
      IList<SourcePay> source = this._session.CreateCriteria(typeof (SourcePay)).AddOrder(Order.Asc("SourcePayName")).List<SourcePay>();
      if ((uint) source.Count > 0U)
      {
        this.cmbSource.DataSource = (object) source;
        this.cmbSource.ValueMember = "SourcePayId";
        this.cmbSource.DisplayMember = "SourcePayName";
        this.cmbSource.SelectedItem = (object) source.First<SourcePay>((Func<SourcePay, bool>) (x => (int) x.SourcePayId == 10));
        IList list1 = this._session.CreateCriteria(typeof (PurposePay)).AddOrder(Order.Asc("PurposePayId")).List();
        if ((uint) list1.Count > 0U)
        {
          this.cmbPurpose.DataSource = (object) list1;
          this.cmbPurpose.ValueMember = "PurposePayId";
          this.cmbPurpose.DisplayMember = "PurposePayName";
          IList list2 = this._session.CreateCriteria(typeof (PayDoc)).AddOrder(Order.Asc("PayDocId")).List();
          if ((uint) list2.Count > 0U)
          {
            this.cmbPayDoc.DataSource = (object) list2;
            this.cmbPayDoc.ValueMember = "PayDocId";
            this.cmbPayDoc.DisplayMember = "PayDocName";
            DateTime? nullable = Kvartplata.Classes.Options.PeriodPay;
            if (!nullable.HasValue)
            {
              Kvartplata.Classes.MonthPicker mpPeriod = this.mpPeriod;
              nullable = Kvartplata.Classes.Options.Period.PeriodName;
              DateTime dateTime = nullable.Value;
              mpPeriod.Value = dateTime;
            }
            else
            {
              Kvartplata.Classes.MonthPicker mpPeriod = this.mpPeriod;
              nullable = Kvartplata.Classes.Options.PeriodPay;
              DateTime dateTime = nullable.Value;
              mpPeriod.Value = dateTime;
            }
            nullable = Kvartplata.Classes.Options.MonthPay;
            if (!nullable.HasValue)
            {
              Kvartplata.Classes.MonthPicker mpPeriodPay = this.mpPeriodPay;
              nullable = Kvartplata.Classes.Options.Period.PeriodName;
              DateTime dateTime = nullable.Value.AddMonths(-1);
              mpPeriodPay.Value = dateTime;
            }
            else
            {
              Kvartplata.Classes.MonthPicker mpPeriodPay = this.mpPeriodPay;
              nullable = Kvartplata.Classes.Options.MonthPay;
              DateTime dateTime = nullable.Value;
              mpPeriodPay.Value = dateTime;
            }
            nullable = Kvartplata.Classes.Options.DatePay;
            if (nullable.HasValue)
            {
              DateTimePicker dtmpDate = this.dtmpDate;
              nullable = Kvartplata.Classes.Options.DatePay;
              DateTime dateTime = nullable.Value;
              dtmpDate.Value = dateTime;
            }
            this.MySettingsPayAddOhlGrid.ConfigFile = Kvartplata.Classes.Options.PathProfileAppData + "\\State\\config.xml";
            this.MySettingsPayOhlGrid.ConfigFile = Kvartplata.Classes.Options.PathProfileAppData + "\\State\\config.xml";
            this.MySettingsPayStornGrid.ConfigFile = Kvartplata.Classes.Options.PathProfileAppData + "\\State\\config.xml";
            this.MySettingsPayAddOhlGrid.GridName = "PayOhlGrid";
            this.MySettingsPayOhlGrid.GridName = "PayOhlViewGrid";
            this.MySettingsPayStornGrid.GridName = "PayOhlReverse";
            this.SettingGrid();
          }
          else
          {
            int num = (int) MessageBox.Show("Внесение записей невозможно. Заполните словарь типы платежных документов", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            this.Close();
          }
        }
        else
        {
          int num = (int) MessageBox.Show("Внесение записей невозможно. Заполните словарь назначений", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.Close();
        }
      }
      else
      {
        int num = (int) MessageBox.Show("Внесение записей невозможно. Заполните словарь источников", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.Close();
      }
    }

    private void FrmPaymentOhl_Shown(object sender, EventArgs e)
    {
    }

    private void btnClearRent_Click(object sender, EventArgs e)
    {
      IList<OhlPayment> dataSource = (IList<OhlPayment>) this.dgvPaymentsForAdd.DataSource;
      foreach (OhlPayment ohlPayment in (IEnumerable<OhlPayment>) this.ohlPaymentsAdd)
      {
        if (dataSource.Contains(ohlPayment))
          ohlPayment.Rent = 0.0;
      }
      this.dgvPaymentsForAdd.Refresh();
    }

    private void btnClearRentPeni_Click(object sender, EventArgs e)
    {
      IList<OhlPayment> dataSource = (IList<OhlPayment>) this.dgvPaymentsForAdd.DataSource;
      foreach (OhlPayment ohlPayment in (IEnumerable<OhlPayment>) this.ohlPaymentsAdd)
      {
        if (dataSource.Contains(ohlPayment))
          ohlPayment.RentPeni = 0.0;
      }
      this.dgvPaymentsForAdd.Refresh();
    }

    private void ShoeMessageWait()
    {
      if (this.messageWait == null)
        this.messageWait = new MessageWait("Подождите, идет обработка данных");
      int num = (int) this.messageWait.ShowDialog();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Платежи будут сохранены. Продолжить?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      if (this._thread != null)
      {
        this._thread.Abort();
        this._thread.Join(1000);
        this._thread = new Thread(new ThreadStart(this.ShoeMessageWait));
      }
      if (this._thread == null)
        this._thread = new Thread(new ThreadStart(this.ShoeMessageWait));
      this._thread.Start();
      try
      {
        this.Cursor = Cursors.WaitCursor;
        this.Enabled = false;
        IList list1 = this._session.CreateCriteria(typeof (Kvartplata.Classes.Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) new DateTime(this.mpPeriod.Value.Year, this.mpPeriod.Value.Month, 1))).List();
        IList list2 = this._session.CreateCriteria(typeof (Kvartplata.Classes.Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) new DateTime(this.mpPeriodPay.Value.Year, this.mpPeriodPay.Value.Month, 1))).List();
        if (list1.Count == 0 || list2.Count == 0)
        {
          int num = (int) MessageBox.Show("Введен некорректный период", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        Kvartplata.Classes.Period period1 = (Kvartplata.Classes.Period) list1[0];
        Kvartplata.Classes.Period period2 = (Kvartplata.Classes.Period) list2[0];
        this._session.CreateSQLQuery("call droptmp(1,'tmpKvrplOhlPmntPay',current user); create table " + Kvartplata.Classes.Options.Login + ".tmpKvrplOhlPmntPay (Payment_id integer NOT NULL, Period_id smallint NOT NULL, Client_id integer NOT NULL, SourcePay_id smallint NOT NULL, PurposePay_id smallint NOT NULL, Receipt_id smallint NOT NULL, Service_id smallint NOT NULL, Payment_date date NOT NULL, Month_id smallint NOT NULL, Packet_num char(25) NOT NULL, Payment_value numeric(9,2) NOT NULL, Payment_peni numeric(9,2) NOT NULL, Uname varchar(30) NOT NULL DEFAULT current user, Dedit timestamp NOT NULL DEFAULT current timestamp, supplier_id integer NOT NULL DEFAULT 0, PayDoc_id smallint NOT NULL DEFAULT 1, Recipient_id integer NOT NULL DEFAULT 0, OhlAccount_id integer NULL)").ExecuteUpdate();
        int num1 = 1;
        foreach (OhlPayment ohlPayment in this.ohlPaymentsAdd.Where<OhlPayment>((Func<OhlPayment, bool>) (x =>
        {
          if (x.Rent != 0.0)
            return x.Account != "ИТОГО";
          return false;
        })).ToList<OhlPayment>())
        {
          if (ohlPayment.Rent != 0.0 || ohlPayment.RentPeni != 0.0)
          {
            Payment payment = new Payment();
            payment.PaymentId = num1;
            payment.LsClient = this._client;
            payment.Period = period1;
            payment.PeriodPay = period2;
            payment.Service = this._session.Get<Kvartplata.Classes.Service>((object) Convert.ToInt16(0));
            payment.Receipt = this._session.Get<Receipt>((object) Convert.ToInt16(50));
            payment.SPay = (SourcePay) this.cmbSource.SelectedItem;
            payment.PPay = (PurposePay) this.cmbPurpose.SelectedItem;
            payment.PayDoc = (PayDoc) this.cmbPayDoc.SelectedItem;
            payment.PacketNum = "";
            payment.PaymentValue = Convert.ToDecimal(ohlPayment.Rent);
            payment.PaymentPeni = Convert.ToDecimal(ohlPayment.RentPeni);
            payment.PaymentDate = this.dtmpDate.Value;
            payment.Supplier = this._session.Get<Supplier>((object) ohlPayment.SupplierId);
            payment.UName = Kvartplata.Classes.Options.Login;
            payment.DEdit = DateTime.Now.Date;
            payment.RecipientId = this._session.CreateQuery("from BaseOrg where BaseOrgId=:id").SetParameter<int>("id", 0).UniqueResult<BaseOrg>();
            payment.OhlaccountId = new int?(ohlPayment.OhlAccountId);
            ohlPayment.PaymentId = payment.PaymentId;
            this._session.CreateSQLQuery(string.Format("INSERT INTO " + Kvartplata.Classes.Options.Login + ".tmpKvrplOhlPmntPay values(:p,{0},:clId,{1},{2},{3},{4},'{5}',{6},'',:pv,:pp,'{7}',getdate(),{8},{9},0,{10})", (object) period1.PeriodId, (object) payment.SPay.SourcePayId, (object) payment.PPay.PurposePayId, (object) payment.Receipt.ReceiptId, (object) payment.Service.ServiceId, (object) payment.PaymentDate.ToString("yyyy-MM-dd"), (object) payment.PeriodPay.PeriodId, (object) Kvartplata.Classes.Options.Login, (object) payment.Supplier.SupplierId, (object) payment.PayDoc.PayDocId, (object) payment.OhlaccountId)).SetParameter<int>("p", payment.PaymentId).SetParameter<string>("clId", payment.ClientId).SetParameter<Decimal>("pv", payment.PaymentValue).SetParameter<Decimal>("pp", payment.PaymentPeni).ExecuteUpdate();
            if (payment.Supplier != null);
            this._session.Flush();
            ++num1;
          }
        }
        this.LoadHomeOhl((IList<OhlPayment>) this.ohlPaymentsAdd.Where<OhlPayment>((Func<OhlPayment, bool>) (x =>
        {
          if (x.Rent != 0.0)
            return x.Account != "ИТОГО";
          return false;
        })).ToList<OhlPayment>());
        FrmPaymentOhl.Rent2(period1.PeriodId, "fnd", Kvartplata.Classes.Options.Alias, Kvartplata.Classes.Options.Login, Kvartplata.Classes.Options.Pwd);
        this._session.CreateSQLQuery("call droptmp(1,'tmpKvrplOhlPmnt',current user);").ExecuteUpdate();
        this._session.CreateSQLQuery("call droptmp(1,'tmpKvrplOhlPmntPay',current user);").ExecuteUpdate();
      }
      catch (Exception ex)
      {
        Kvartplata.Classes.KvrplHelper.WriteLog(ex, (Kvartplata.Classes.LsClient) null);
      }
      this.dgvPaymentsForAdd.Refresh();
      this.dgvPayments.Refresh();
      this.tcMain.SelectedIndex = 0;
      this.Cursor = Cursors.Default;
      this.Enabled = true;
      /*if (this.messageWait != null)
        this.messageWait.Invoke((Delegate) (() => this.messageWait.Close()));*/
      this._listHome.Clear();
      this.dvgHomeList.DataSource = (object) null;
    }

    private void dgvPaymentsForAdd_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      this.dgvPaymentsForAdd.Refresh();
      this.RefreshSum();
      this.dgvPaymentsForAdd.Refresh();
    }

    private void dgvPaymentsForAdd_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (((DataGridView) sender).DataSource == null)
        return;
      DataGridViewRow row = ((DataGridView) sender).Rows[e.RowIndex];
      if (((OhlPayment) row.DataBoundItem).NotEqualsSum)
        row.DefaultCellStyle.BackColor = Color.Red;
      if (row.Cells[0].Value != null && row.Cells["Account"].Value.ToString() == "ИТОГО")
        row.DefaultCellStyle.Font = new Font(this.dgvPaymentsForAdd.Font, FontStyle.Bold);
    }

    private void dgvPaymentsForAdd_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsPayAddOhlGrid.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsPayAddOhlGrid.Columns[this.MySettingsPayAddOhlGrid.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsPayAddOhlGrid.Save();
    }

    private void dgvPayments_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsPayOhlGrid.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsPayOhlGrid.Columns[this.MySettingsPayOhlGrid.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsPayOhlGrid.Save();
    }

    private void dvgReverse_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsPayStornGrid.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsPayStornGrid.Columns[this.MySettingsPayStornGrid.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsPayStornGrid.Save();
    }

    private void dgvPayments_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (((DataGridView) sender).DataSource == null)
        return;
      DataGridViewRow row = ((DataGridView) sender).Rows[e.RowIndex];
      if (((OhlPayment) row.DataBoundItem).NotEqualsSum)
        row.DefaultCellStyle.BackColor = Color.Red;
      if (row.Cells[0].Value != null && row.Cells["Account2"].Value.ToString() == "ИТОГО")
        row.DefaultCellStyle.Font = new Font(this.dgvPayments.Font, FontStyle.Bold);
    }

    private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      if (this.tcMain.SelectedTab == this.tabReverse)
      {
        this.btnClearRent.Visible = false;
        this.btnClearRentPeni.Visible = false;
        this.btnSave.Visible = false;
        this.butSaveReverse.Visible = true;
        this.reverseBut.Visible = false;
        this.butHmParam.Visible = false;
        this.butNullMinusPayment.Visible = false;
      }
      if (this.tcMain.SelectedTab == this.tabReverse && !this.ReversePay)
      {
        foreach (DataGridViewBand column in (BaseCollection) this.dvgReverse.Columns)
          column.ReadOnly = true;
        this.dvgReverse.Columns[6].Visible = true;
        this.dvgReverse.Columns[7].Visible = true;
        this.butSaveReverse.Visible = false;
        this.dvgReverse.AutoGenerateColumns = false;
        this.dvgReverse.DataSource = (object) this.ohlReverse.ToArray<OhlPayment>();
      }
      else
        this.ReversePay = false;
      if (this.tcMain.SelectedTab == this.tbpView)
      {
        this.btnClearRent.Visible = false;
        this.btnClearRentPeni.Visible = false;
        this.btnSave.Visible = false;
        this.butSaveReverse.Visible = false;
        this.reverseBut.Visible = true;
        this.butHmParam.Visible = false;
        this.GetData();
        this.dgvPayments.Refresh();
        this.PeriodChange = false;
        this.butNullMinusPayment.Visible = false;
      }
      if (this.tcMain.SelectedTab == this.tbpAdding)
      {
        this.btnClearRent.Visible = true;
        this.btnClearRentPeni.Visible = true;
        this.btnSave.Visible = true;
        this.butSaveReverse.Visible = false;
        this.reverseBut.Visible = false;
        this.butHmParam.Visible = true;
        this.butNullMinusPayment.Visible = true;
        this.GetAddData();
        this.dgvPaymentsForAdd.Refresh();
        this.PeriodPayChange = false;
      }
      this.RefreshSum();
      this.Cursor = Cursors.Default;
    }

    private void mpPeriodPay_ValueChanged(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      if (this.tcMain.SelectedIndex == 1)
      {
        this.GetAddData();
        this.dgvPaymentsForAdd.Refresh();
      }
      this.Cursor = Cursors.Default;
    }

    private void mpPeriod_ValueChanged(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      if (this.tcMain.SelectedIndex == 0)
      {
        this.GetData();
        this.dgvPayments.Refresh();
        this.btnFilter_Click((object) null, (EventArgs) null);
      }
      this.Cursor = Cursors.Default;
    }

    private void SettingGrid()
    {
      this.MySettingsPayAddOhlGrid.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvPaymentsForAdd.Columns)
        this.MySettingsPayAddOhlGrid.GetMySettings(column);
      this.MySettingsPayOhlGrid.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvPayments.Columns)
        this.MySettingsPayOhlGrid.GetMySettings(column);
      this.MySettingsPayStornGrid.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dvgReverse.Columns)
        this.MySettingsPayStornGrid.GetMySettings(column);
    }

    private void GetAddData()
    {
      ISession session = this._session;
      string format = "create table #4bln   (   period_id smallint,       client_id integer,       supplier_id integer,       perfomer_id integer,       rent numeric(15,2),       rent_peni numeric(15,2),       dept numeric(15,2)  );  insert into #4bln  select period_id, client_id, supplier_id, (select perfomer_id from dba.dcSupplier where supplier_id=v.supplier_id) as perfomer_id, rent, 0 as rent_peni, balance_in-payment as dept   from dba.lsBalance v   where Period_id={2} and perfomer_id<>-39999859       union all   select period_id, client_id, supplier_id, (select perfomer_id from dba.dcSupplier where supplier_id=v.supplier_id) as perfomer_id, 0 as rent, rent + correct as rent_peni, balance_in-payment as dept   from dba.lsBalancePeni v where Period_id={2} and perfomer_id<>-39999859;  commit;  delete from #4bln where rent=0 and rent_peni=0 and dept=0;commit;  create index ss on #4bln (period_id, client_id, supplier_id, perfomer_id);  create table #bln   (   period_id smallint,      client_id integer,      supplier_id integer,      perfomer_id integer,      rent numeric(15,2),      rent_peni numeric(15,2),      dept numeric(15,2)  );  insert into #bln    select period_id, client_id, supplier_id, perfomer_id, sum(rent), sum(rent_peni), sum(dept)       from #4bln    group by period_id, client_id, supplier_id, perfomer_id;  commit;  delete from #bln where rent=0 and rent_peni=0 and dept=0;commit;  create index ss on #bln (perfomer_id ); select supplier_id, account, typeaccount_id, sum(rent) rent, sum(rent_peni) rent_peni, sum(rent_all) rent_all, account_id, bank_name     from (select perfomer_id,                   isnull((select param_value from dba.hmParam where company_id=t.company_id and idhome=t.idhome and param_id=309 and cast('{0}' as date) between dbeg and dend),0) as account_id,                   (select typeaccount_id from dba.ohlAccounts where ohlaccount_id=account_id) as typeaccount_id,                   (select typeaccount_name from dba.dcohlTypeAccount b,dba.ohlAccounts a where a.ohlaccount_id=account_id and a.typeaccount_id=b.typeaccount_id) as account_type,                   (select account from dba.ohlAccounts where ohlaccount_id=account_id) as account,                   (select bank_id from dba.ohlAccounts where ohlaccount_id=account_id) as bank_id,                   (select namebank from dba.di_bank where idbank=bank_id) as bank_name,                 (select bik from dba.di_bank where idbank=bank_id) as bik,(select kor_sch from dba.di_bank where idbank=bank_id) as coraccount,                   rent, rent_peni, rent + rent_peni as rent_all, supplier_id              from #bln v, dba.lsClient ls, dba.lsArenda a, dba.lsClient t              where v.client_id=t.client_id and ls.client_id=a.client_id and a.idbaseorg=v.perfomer_id and ls.client_id={1}            ) as it   group by supplier_id, account, typeaccount_id, account_id, bank_name ; ";
      DateTime dateTime = Kvartplata.Classes.KvrplHelper.LastDay(this.mpPeriodPay.Value);
      string str = dateTime.ToString("yyyy-M-d");
      // ISSUE: variable of a boxed type
      int clientId = this._client.ClientId;
      dateTime = this.mpPeriodPay.Value;
      int year = dateTime.Year;
      dateTime = this.mpPeriodPay.Value;
      int month = dateTime.Month;
      int day = 1;
      // ISSUE: variable of a boxed type
      int periodId = Kvartplata.Classes.KvrplHelper.GetPeriod(new DateTime(year, month, day)).PeriodId;
      string queryString = string.Format(format, (object) str, (object) clientId, (object) periodId);
      IList list1 = session.CreateSQLQuery(queryString).List();
      this.ohlPaymentsAdd = (IList<OhlPayment>) new List<OhlPayment>();
      Decimal num1 = new Decimal();
      Decimal num2 = new Decimal();
      foreach (IList list2 in (IEnumerable) list1)
      {
        this.ohlPaymentsAdd.Add(new OhlPayment()
        {
          SupplierId = Convert.ToInt32(list2[0]),
          Account = Convert.ToString(list2[1]),
          TypeaccountId = Convert.ToInt32(list2[2]),
          Rent = Convert.ToDouble(list2[3]),
          RentPeni = Convert.ToDouble(list2[4]),
          RentAll = Convert.ToDouble(list2[5]),
          OhlAccountId = Convert.ToInt32(list2[6]),
          BankName = Convert.ToString(list2[7])
        });
        num1 += Convert.ToDecimal(list2[3]);
        num2 += Convert.ToDecimal(list2[4]);
      }
      this.dgvPaymentsForAdd.AutoGenerateColumns = false;
      this.ohlPaymentsAdd = (IList<OhlPayment>) this.ohlPaymentsAdd.OrderBy<OhlPayment, string>((Func<OhlPayment, string>) (x => x.Account)).ToList<OhlPayment>();
      IList<OhlPayment> ohlPaymentsAdd = this.ohlPaymentsAdd;
      OhlPayment ohlPayment = new OhlPayment();
      ohlPayment.Account = "ИТОГО";
      double num3 = Convert.ToDouble(num1.ToString("N"));
      ohlPayment.Rent = num3;
      double num4 = Convert.ToDouble(num2.ToString("N"));
      ohlPayment.RentPeni = num4;
      ohlPaymentsAdd.Add(ohlPayment);
      this.dgvPaymentsForAdd.DataSource = (object) this.ohlPaymentsAdd;
      this.MySettingsPayAddOhlGrid.GridName = "PayOhlGrid";
      this.SettingGrid();
      this.RefreshSum();
    }

    private void GetData()
    {
      DateTime dateTime1 = this.mpPeriod.Value;
      int year = dateTime1.Year;
      dateTime1 = this.mpPeriod.Value;
      int month = dateTime1.Month;
      int day = 1;
      int periodId = Kvartplata.Classes.KvrplHelper.GetPeriod(new DateTime(year, month, day)).PeriodId;
      IList<Payment> paymentList1 = this._session.CreateQuery("from Payment p where p.LsClient.ClientId=:cl and p.Period.PeriodId=:perId").SetParameter<int>("cl", this._client.ClientId).SetParameter<int>("perId", periodId).List<Payment>();
      this.ohlPayments = (IList<OhlPayment>) new List<OhlPayment>();
      this.ohlReverse = (IList<OhlPayment>) new List<OhlPayment>();
      Decimal num1 = new Decimal();
      Decimal num2 = new Decimal();
      Decimal num3 = new Decimal();
      Decimal num4 = new Decimal();
      Decimal num5 = new Decimal();
      Decimal num6 = new Decimal();
      IList list1 = this._session.CreateSQLQuery("select lsp.payment_id, lsp2.payment_value, lsp2.payment_peni, lsp2.Payment_id from dba.ohllsPayment ohl inner join dba.lsPayment lsp on ohl.payment_id_main=lsp.payment_id inner join dba.lsPayment lsp2 on ohl.payment_id=lsp2.payment_id where ohl.client_id=:cl and lsp.Period_id=:perId").SetParameter<int>("cl", this._client.ClientId).SetParameter<int>("perId", periodId).List();
      IList list2 = this._session.CreateSQLQuery("select lsp.payment_id from dba.ohllsPayment ohl inner join dba.lsPayment lsp on ohl.payment_id=lsp.payment_id where ohl.client_id=:cl and lsp.Period_id=:perId").SetParameter<int>("cl", this._client.ClientId).SetParameter<int>("perId", periodId).List();
      List<Payment> paymentList2 = new List<Payment>();
      foreach (IList list3 in (IEnumerable) list1)
      {
        Payment payment = this._session.CreateQuery("from Payment p where p.PaymentId=:pld").SetParameter("pld", list3[3]).UniqueResult<Payment>();
        paymentList2.Add(payment);
      }
      foreach (Payment payment in (IEnumerable<Payment>) paymentList1)
      {
        double num7 = 0.0;
        double num8 = 0.0;
        double num9 = 0.0;
        double num10 = 0.0;
        if (list2.Contains((object) payment.PaymentId))
        {
          ohlAccounts ohlAccounts = this._session.Get<ohlAccounts>((object) payment.OhlaccountId);
          IList<OhlPayment> ohlReverse = this.ohlReverse;
          OhlPayment ohlPayment = new OhlPayment();
          ohlPayment.SupplierId = payment.Supplier.SupplierId;
          ohlPayment.Account = ohlAccounts.Account;
          ohlPayment.TypeaccountId = ohlAccounts.TypeAccount.TypeAccountId;
          ohlPayment.Rent = Convert.ToDouble(payment.PaymentValue);
          ohlPayment.RentPeni = Convert.ToDouble(payment.PaymentPeni);
          ohlPayment.RentAll = Convert.ToDouble(payment.PaymentValue) + Convert.ToDouble(payment.PaymentPeni);
          ohlPayment.OhlAccountId = payment.OhlaccountId.Value;
          ohlPayment.BankName = ohlAccounts.Bank.BankName;
          DateTime? nullable = payment.PeriodPayValue;
          DateTime dateTime2 = nullable.Value;
          ohlPayment.PeriodPay = dateTime2;
          nullable = payment.Period.PeriodName;
          DateTime dateTime3 = nullable.Value;
          ohlPayment.Period = dateTime3;
          int paymentId = payment.PaymentId;
          ohlPayment.PaymentId = paymentId;
          DateTime paymentDate = payment.PaymentDate;
          ohlPayment.PaymentDate = paymentDate;
          ohlReverse.Add(ohlPayment);
        }
        else
        {
          foreach (IList list3 in (IEnumerable) this._session.CreateSQLQuery("SELECT sum(lsp.payment_value), sum(lsp.payment_peni) FROM DBA.lsPayment lsp where lsp.payment_id in (select payment_id from dba.ohllspayment where payment_id_main=:pid and client_id!=:cl)").SetParameter<int>("pid", payment.PaymentId).SetParameter<int>("cl", this._client.ClientId).List())
          {
            num7 = Math.Round(Convert.ToDouble(list3[0]), 2);
            num8 = Math.Round(Convert.ToDouble(list3[1]), 2);
          }
          if ((uint) list1.Count > 0U)
          {
            foreach (IList list3 in (IEnumerable) list1)
            {
              if (Convert.ToInt32(list3[0]) == payment.PaymentId)
              {
                num9 += Math.Round(Convert.ToDouble(list3[1]), 2);
                num10 += Math.Round(Convert.ToDouble(list3[2]), 2);
              }
            }
          }
          bool flag = false;
          double num11 = Math.Round(num7, 2);
          double num12 = Math.Round(num9, 2);
          double num13 = Math.Round(num8, 2);
          double num14 = Math.Round(num10, 2);
          if (Convert.ToDecimal(num11 + -num12) - payment.PaymentValue != Decimal.Zero)
            flag = true;
          if (Convert.ToDecimal(num13 + -num14) - payment.PaymentPeni != Decimal.Zero)
            flag = true;
          double num15 = Math.Round(num11, 2);
          double num16 = Math.Round(num13, 2);
          double num17 = Math.Round(num12, 2);
          double num18 = Math.Round(num14, 2);
          ohlAccounts ohlAccounts = this._session.Get<ohlAccounts>((object) payment.OhlaccountId);
          IList<OhlPayment> ohlPayments = this.ohlPayments;
          OhlPayment ohlPayment = new OhlPayment();
          ohlPayment.SupplierId = payment.Supplier.SupplierId;
          ohlPayment.Account = ohlAccounts.Account;
          ohlPayment.TypeaccountId = ohlAccounts.TypeAccount.TypeAccountId;
          ohlPayment.Rent = Convert.ToDouble(payment.PaymentValue);
          ohlPayment.RentPeni = Convert.ToDouble(payment.PaymentPeni);
          ohlPayment.RentAll = Convert.ToDouble(payment.PaymentValue) + Convert.ToDouble(payment.PaymentPeni);
          ohlPayment.OhlAccountId = payment.OhlaccountId.Value;
          ohlPayment.BankName = ohlAccounts.Bank.BankName;
          ohlPayment.PeriodPay = payment.PeriodPayValue.Value;
          ohlPayment.Period = payment.Period.PeriodName.Value;
          ohlPayment.RentForClients = num15;
          ohlPayment.RentPeniForClients = num16;
          ohlPayment.RentForClientsRev = num17;
          ohlPayment.RentPeniForClientsRev = num18;
          int num19 = flag ? 1 : 0;
          ohlPayment.NotEqualsSum = num19 != 0;
          int paymentId = payment.PaymentId;
          ohlPayment.PaymentId = paymentId;
          DateTime paymentDate = payment.PaymentDate;
          ohlPayment.PaymentDate = paymentDate;
          ohlPayments.Add(ohlPayment);
          num1 += payment.PaymentValue;
          num2 += payment.PaymentPeni;
          num3 += Convert.ToDecimal(num15);
          num4 += Convert.ToDecimal(num16);
          num5 += Convert.ToDecimal(num17);
          num6 += Convert.ToDecimal(num18);
        }
      }
      this.ohlPayments = (IList<OhlPayment>) this.ohlPayments.OrderBy<OhlPayment, string>((Func<OhlPayment, string>) (x => x.Account)).ToList<OhlPayment>();
      IList<OhlPayment> ohlPayments1 = this.ohlPayments;
      OhlPayment ohlPayment1 = new OhlPayment();
      ohlPayment1.Account = "ИТОГО";
      double num20 = Convert.ToDouble(num1.ToString("N"));
      ohlPayment1.Rent = num20;
      double num21 = Convert.ToDouble(num2.ToString("N"));
      ohlPayment1.RentPeni = num21;
      double num22 = Convert.ToDouble(num3.ToString("N"));
      ohlPayment1.RentForClients = num22;
      double num23 = Convert.ToDouble(num4.ToString("N"));
      ohlPayment1.RentPeniForClients = num23;
      double num24 = Convert.ToDouble(num5.ToString("N"));
      ohlPayment1.RentForClientsRev = num24;
      double num25 = Convert.ToDouble(num6.ToString("N"));
      ohlPayment1.RentPeniForClientsRev = num25;
      ohlPayments1.Add(ohlPayment1);
      this.dgvPayments.AutoGenerateColumns = false;
      this.dgvPayments.DataSource = (object) this.ohlPayments;
      this.dvgReverse.AutoGenerateColumns = false;
      this.dvgReverse.DataSource = (object) this.ohlReverse.ToArray<OhlPayment>();
      this.dgvPayments.Refresh();
    }

    private void RefreshSum()
    {
      double num1 = 0.0;
      double num2 = 0.0;
      if (this.tcMain.SelectedTab == this.tbpAdding)
      {
        if (this.dgvPaymentsForAdd.DataSource != null)
        {
          foreach (OhlPayment ohlPayment in (this.dgvPaymentsForAdd.DataSource as IList<OhlPayment>).Where<OhlPayment>((Func<OhlPayment, bool>) (x => x.Account != "ИТОГО")).ToList<OhlPayment>())
          {
            num1 += ohlPayment.Rent;
            num2 += ohlPayment.RentPeni;
            if (ohlPayment.RentAll == ohlPayment.RentForClients + ohlPayment.RentPeniForClients)
              ;
          }
        }
        this.ohlPaymentsAdd.First<OhlPayment>((Func<OhlPayment, bool>) (x => x.Account == "ИТОГО")).Rent = Convert.ToDouble(num1.ToString("N"));
        this.ohlPaymentsAdd.First<OhlPayment>((Func<OhlPayment, bool>) (x => x.Account == "ИТОГО")).RentPeni = Convert.ToDouble(num2.ToString("N"));
      }
      if (this.tcMain.SelectedTab == this.tbpView)
      {
        if (this.dgvPayments.DataSource != null)
        {
          foreach (OhlPayment ohlPayment in (this.dgvPayments.DataSource as IList<OhlPayment>).Where<OhlPayment>((Func<OhlPayment, bool>) (x => x.Account != "ИТОГО")).ToList<OhlPayment>())
          {
            num1 += ohlPayment.Rent;
            num2 += ohlPayment.RentPeni;
          }
        }
        this.ohlPayments.First<OhlPayment>((Func<OhlPayment, bool>) (x => x.Account == "ИТОГО")).Rent = Convert.ToDouble(num1.ToString("N"));
        this.ohlPayments.First<OhlPayment>((Func<OhlPayment, bool>) (x => x.Account == "ИТОГО")).RentPeni = Convert.ToDouble(num2.ToString("N"));
      }
      if (this.tcMain.SelectedTab != this.tabReverse)
        ;
    }

    private void btnFilter_Click(object sender, EventArgs e)
    {
      if (this.txbAccountFilter.Text != "")
      {
        if (this.tcMain.SelectedTab == this.tbpView)
          this.dgvPayments.DataSource = (object) this.ohlPayments.Select<OhlPayment, OhlPayment>((Func<OhlPayment, OhlPayment>) (x => x)).Where<OhlPayment>((Func<OhlPayment, bool>) (x =>
          {
            if (!(x.Account.Substring(x.Account.Length - 4, 4) == this.txbAccountFilter.Text))
              return x.Account == "ИТОГО";
            return true;
          })).ToList<OhlPayment>();
        if (this.tcMain.SelectedTab == this.tbpAdding)
          this.dgvPaymentsForAdd.DataSource = (object) this.ohlPaymentsAdd.Select<OhlPayment, OhlPayment>((Func<OhlPayment, OhlPayment>) (x => x)).Where<OhlPayment>((Func<OhlPayment, bool>) (x =>
          {
            if (!(x.Account != "") || !(x.Account.Substring(x.Account.Length - 4, 4) == this.txbAccountFilter.Text))
              return x.Account == "ИТОГО";
            return true;
          })).ToList<OhlPayment>();
      }
      else
      {
        if (this.tcMain.SelectedTab == this.tbpView)
          this.dgvPayments.DataSource = (object) this.ohlPayments;
        if (this.tcMain.SelectedTab == this.tbpAdding)
          this.dgvPaymentsForAdd.DataSource = (object) this.ohlPaymentsAdd;
      }
      if (this.txbBankFilter.Text != "")
      {
        if (this.tcMain.SelectedTab == this.tbpView)
          this.dgvPayments.DataSource = (object) this.ohlPayments.Select<OhlPayment, OhlPayment>((Func<OhlPayment, OhlPayment>) (x => x)).Where<OhlPayment>((Func<OhlPayment, bool>) (x =>
          {
            if (x.BankName == null || !x.BankName.ToLower().Contains(this.txbBankFilter.Text.ToLower()))
              return x.Account == "ИТОГО";
            return true;
          })).ToList<OhlPayment>();
        if (this.tcMain.SelectedTab == this.tbpAdding)
          this.dgvPaymentsForAdd.DataSource = (object) this.ohlPaymentsAdd.Select<OhlPayment, OhlPayment>((Func<OhlPayment, OhlPayment>) (x => x)).Where<OhlPayment>((Func<OhlPayment, bool>) (x =>
          {
            if (x.BankName == null || !x.BankName.ToLower().Contains(this.txbBankFilter.Text.ToLower()))
              return x.Account == "ИТОГО";
            return true;
          })).ToList<OhlPayment>();
      }
      this.RefreshSum();
    }

    private void chbNotEqualsSum_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chbNotEqualsSum.Checked)
      {
        this.chbPeniNegative.Checked = false;
        this.chbRentNegative.Checked = false;
        if (this.tcMain.SelectedTab == this.tbpAdding)
          this.dgvPaymentsForAdd.DataSource = (object) this.ohlPaymentsAdd.Where<OhlPayment>((Func<OhlPayment, bool>) (x => x.NotEqualsSum)).ToList<OhlPayment>();
        else
          this.dgvPayments.DataSource = (object) this.ohlPayments.Where<OhlPayment>((Func<OhlPayment, bool>) (x => x.NotEqualsSum)).ToList<OhlPayment>();
      }
      else if (this.tcMain.SelectedTab == this.tbpAdding)
        this.dgvPaymentsForAdd.DataSource = (object) this.ohlPaymentsAdd;
      else
        this.dgvPayments.DataSource = (object) this.ohlPayments;
    }

    private void chbRentNegative_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chbRentNegative.Checked)
      {
        this.chbPeniNegative.Checked = false;
        this.chbNotEqualsSum.Checked = false;
        if (this.tcMain.SelectedTab == this.tbpAdding)
          this.dgvPaymentsForAdd.DataSource = (object) this.ohlPaymentsAdd.Where<OhlPayment>((Func<OhlPayment, bool>) (x => x.Rent < 0.0)).ToList<OhlPayment>();
        else
          this.dgvPayments.DataSource = (object) this.ohlPayments.Where<OhlPayment>((Func<OhlPayment, bool>) (x => x.Rent < 0.0)).ToList<OhlPayment>();
      }
      else if (this.tcMain.SelectedTab == this.tbpAdding)
        this.dgvPaymentsForAdd.DataSource = (object) this.ohlPaymentsAdd;
      else
        this.dgvPayments.DataSource = (object) this.ohlPayments;
    }

    private void chbPeniNegative_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chbPeniNegative.Checked)
      {
        this.chbRentNegative.Checked = false;
        this.chbNotEqualsSum.Checked = false;
        if (this.tcMain.SelectedTab == this.tbpAdding)
          this.dgvPaymentsForAdd.DataSource = (object) this.ohlPaymentsAdd.Where<OhlPayment>((Func<OhlPayment, bool>) (x => x.RentPeni < 0.0)).ToList<OhlPayment>();
        else
          this.dgvPayments.DataSource = (object) this.ohlPayments.Where<OhlPayment>((Func<OhlPayment, bool>) (x => x.RentPeni < 0.0)).ToList<OhlPayment>();
      }
      else if (this.tcMain.SelectedTab == this.tbpAdding)
        this.dgvPaymentsForAdd.DataSource = (object) this.ohlPaymentsAdd;
      else
        this.dgvPayments.DataSource = (object) this.ohlPayments;
    }

    private void reverseBut_Click(object sender, EventArgs e)
    {
      DataGridViewSelectedRowCollection selectedRows = this.dgvPayments.SelectedRows;
      List<OhlPayment> ohlPaymentList1 = new List<OhlPayment>();
      foreach (DataGridViewRow dataGridViewRow in (BaseCollection) selectedRows)
      {
        OhlPayment dataBoundItem = (OhlPayment) dataGridViewRow.DataBoundItem;
        if (dataBoundItem.NotEqualsSum)
        {
          List<OhlPayment> ohlPaymentList2 = ohlPaymentList1;
          OhlPayment ohlPayment = new OhlPayment();
          ohlPayment.SupplierId = dataBoundItem.SupplierId;
          ohlPayment.Account = dataBoundItem.Account;
          ohlPayment.TypeaccountId = dataBoundItem.TypeaccountId;
          ohlPayment.Rent = dataBoundItem.Rent;
          ohlPayment.RentPeni = dataBoundItem.RentPeni;
          ohlPayment.RentAll = dataBoundItem.RentAll;
          ohlPayment.OhlAccountId = dataBoundItem.OhlAccountId;
          ohlPayment.BankName = dataBoundItem.BankName;
          ohlPayment.PeriodPay = dataBoundItem.PeriodPay;
          ohlPayment.Period = dataBoundItem.Period;
          ohlPayment.RentForClients = dataBoundItem.RentForClients;
          ohlPayment.RentPeniForClients = dataBoundItem.RentPeniForClients;
          ohlPayment.RentForClientsRev = dataBoundItem.RentForClientsRev;
          ohlPayment.RentPeniForClientsRev = dataBoundItem.RentPeniForClientsRev;
          int num = dataBoundItem.NotEqualsSum ? 1 : 0;
          ohlPayment.NotEqualsSum = num != 0;
          int paymentId = dataBoundItem.PaymentId;
          ohlPayment.PaymentId = paymentId;
          ohlPaymentList2.Add(ohlPayment);
        }
      }
      OhlPayment[] array = ohlPaymentList1.ToArray();
      foreach (OhlPayment ohlPayment in array)
      {
        ohlPayment.Rent = Math.Round(ohlPayment.RentForClients - ohlPayment.Rent, 2);
        ohlPayment.RentPeni = Math.Round(ohlPayment.RentPeniForClients - ohlPayment.RentPeni, 2);
      }
      this.dvgReverse.Columns[6].Visible = false;
      this.dvgReverse.Columns[7].Visible = false;
      foreach (DataGridViewBand column in (BaseCollection) this.dvgReverse.Columns)
        column.ReadOnly = false;
      this.dvgReverse.AutoGenerateColumns = false;
      this.dvgReverse.DataSource = (object) ((IEnumerable<OhlPayment>) array).ToArray<OhlPayment>();
      this.ReversePay = true;
      this.tcMain.SelectedIndex = 2;
    }

    private void butSaveReverse_Click(object sender, EventArgs e)
    {
      IList<OhlPayment> dataSource = (IList<OhlPayment>) this.dvgReverse.DataSource;
      ICriteria criteria1 = this._session.CreateCriteria(typeof (Kvartplata.Classes.Period));
      string propertyName1 = "PeriodName";
      DateTime dateTime1 = this.mpPeriod.Value;
      int year1 = dateTime1.Year;
      dateTime1 = this.mpPeriod.Value;
      int month1 = dateTime1.Month;
      int day1 = 1;
      // ISSUE: variable of a boxed type
      DateTime local1 = new DateTime(year1, month1, day1);
      SimpleExpression simpleExpression1 = Restrictions.Eq(propertyName1, (object) local1);
      IList list1 = criteria1.Add((ICriterion) simpleExpression1).List();
      ICriteria criteria2 = this._session.CreateCriteria(typeof (Kvartplata.Classes.Period));
      string propertyName2 = "PeriodName";
      DateTime dateTime2 = this.mpPeriodPay.Value;
      int year2 = dateTime2.Year;
      dateTime2 = this.mpPeriodPay.Value;
      int month2 = dateTime2.Month;
      int day2 = 1;
      // ISSUE: variable of a boxed type
      DateTime local2 = new DateTime(year2, month2, day2);
      SimpleExpression simpleExpression2 = Restrictions.Eq(propertyName2, (object) local2);
      IList list2 = criteria2.Add((ICriterion) simpleExpression2).List();
      Kvartplata.Classes.Period period1 = (Kvartplata.Classes.Period) list1[0];
      Kvartplata.Classes.Period period2 = (Kvartplata.Classes.Period) list2[0];
      PurposePay selectedItem1 = (PurposePay) this.cmbPurpose.SelectedItem;
      SourcePay selectedItem2 = (SourcePay) this.cmbSource.SelectedItem;
      Receipt receipt = this._session.Get<Receipt>((object) Convert.ToInt16(50));
      Kvartplata.Classes.Service service = this._session.Get<Kvartplata.Classes.Service>((object) Convert.ToInt16(0));
      DateTime dateTime3 = this.dtmpDate.Value;
      PayDoc selectedItem3 = (PayDoc) this.cmbPayDoc.SelectedItem;
      foreach (OhlPayment ohlPayment in dataSource.ToArray<OhlPayment>())
      {
        int val = this._session.CreateSQLQuery("select DBA.gen_id('lsPayment',1)").List<int>()[0];
        this._session.CreateSQLQuery(string.Format("INSERT INTO DBA.OhllsPayment(Payment_id_main,Client_id,Payment_id,Payment_value,Payment_peni) VALUES ( :p, :c, :id, :pv, :pp)")).SetParameter<int>("p", ohlPayment.PaymentId).SetParameter<int>("c", this._client.ClientId).SetParameter<int>("id", val).SetParameter<double>("pv", ohlPayment.Rent).SetParameter<double>("pp", ohlPayment.RentPeni).ExecuteUpdate();
        this._session.CreateSQLQuery(string.Format("INSERT INTO DBA.lsPayment(Payment_id, Period_id, Client_id, SourcePay_id, PurposePay_id, Receipt_id, Service_id, Payment_date,                            Month_id, Packet_num, Payment_value, Payment_peni, Uname, Dedit, supplier_id, PayDoc_id, Recipient_id, OhlAccount_id) VALUES (:id,{0},:c,{1},{2},{3},{4},'{5}',{6},'',:pv,:pp,'{7}',getdate(),{8},{9},0,{10}) ", (object) period1.PeriodId, (object) selectedItem2.SourcePayId, (object) 9, (object) receipt.ReceiptId, (object) service.ServiceId, (object) dateTime3.ToString("yyyy-MM-dd"), (object) period2.PeriodId, (object) Kvartplata.Classes.Options.Login, (object) ohlPayment.SupplierId, (object) selectedItem3.PayDocId, (object) ohlPayment.OhlAccountId)).SetParameter<int>("id", val).SetParameter<int>("c", this._client.ClientId).SetParameter<double>("pv", ohlPayment.Rent).SetParameter<double>("pp", ohlPayment.RentPeni).ExecuteUpdate();
      }
      this.dvgReverse.DataSource = (object) null;
      this.tcMain.SelectedTab = this.tbpView;
    }

    private void butHmParam_Click(object sender, EventArgs e)
    {
      this.panelHomeList.Width = this.Width / 2;
      this.GetListHome();
      this.CorrcetHome = true;
      this.dgvPaymentsForAdd_SelectionChanged((object) null, (EventArgs) null);
    }

    private void butCancelHmList_Click(object sender, EventArgs e)
    {
      this.panelHomeList.Width = 0;
    }

    private void dgvPaymentsForAdd_SelectionChanged(object sender, EventArgs e)
    {
      this.dvgHomeList.DataSource = (object) null;
      if (!this.CorrcetHome)
        return;
      ICriteria criteria = this._session.CreateCriteria(typeof (Kvartplata.Classes.Period));
      string propertyName = "PeriodName";
      DateTime dateTime = this.mpPeriodPay.Value;
      int year = dateTime.Year;
      dateTime = this.mpPeriodPay.Value;
      int month = dateTime.Month;
      int day = 1;
      // ISSUE: variable of a boxed type
      DateTime local = new DateTime(year, month, day);
      SimpleExpression simpleExpression = Restrictions.Eq(propertyName, (object) local);
      Kvartplata.Classes.Period period = (Kvartplata.Classes.Period) criteria.Add((ICriterion) simpleExpression).List()[0];
      DataGridViewSelectedRowCollection selectedRows = this.dgvPaymentsForAdd.SelectedRows;
      List<ListHome> listHomeList1 = new List<ListHome>();
      IEnumerator enumerator = selectedRows.GetEnumerator();
      try
      {
        if (enumerator.MoveNext())
        {
          OhlPayment p = (OhlPayment) ((DataGridViewRow) enumerator.Current).DataBoundItem;
          List<ListHome> source = new List<ListHome>();
          foreach (ListHome listHome1 in this._listHome.ToArray())
          {
            List<ListHome> listHomeList2 = source;
            ListHome listHome2 = new ListHome();
            int num = listHome1.check ? 1 : 0;
            listHome2.check = num != 0;
            Kvartplata.Classes.Home home = listHome1.Home;
            listHome2.Home = home;
            int ohlAccountId = listHome1.OhlAccountId;
            listHome2.OhlAccountId = ohlAccountId;
            listHomeList2.Add(listHome2);
          }
          listHomeList1.AddRange((IEnumerable<ListHome>) source.Where<ListHome>((Func<ListHome, bool>) (x => x.OhlAccountId == p.OhlAccountId)).ToArray<ListHome>());
          this.labPay.Text = p.Account;
        }
      }
      finally
      {
        IDisposable disposable = enumerator as IDisposable;
        if (disposable != null)
          disposable.Dispose();
      }
      if (listHomeList1.Count > 0)
        this.panelHomeList.Width = this.Width / 2;
      else
        this.panelHomeList.Width = 0;
      this.dvgHomeList.AutoGenerateColumns = false;
      this.dvgHomeList.DataSource = (object) listHomeList1;
    }

    private void butOkHmList_Click(object sender, EventArgs e)
    {
      List<ListHome> dataSource = this.dvgHomeList.DataSource as List<ListHome>;
      if (dataSource != null)
      {
        foreach (ListHome listHome1 in dataSource)
        {
          foreach (ListHome listHome2 in this._listHome)
          {
            if (listHome2.OhlAccountId == listHome1.OhlAccountId && listHome2.Home.IdHome == listHome1.Home.IdHome)
              listHome2.check = listHome1.check;
          }
        }
      }
      this.butOkHmList.Enabled = false;
    }

    private void LoadHomeOhl(IList<OhlPayment> ohlList)
    {
      this._session.CreateSQLQuery("call droptmp(1,'tmpKvrplOhlPmntHome',current user); create table " + Kvartplata.Classes.Options.Login + ".tmpKvrplOhlPmntHome (payment_id integer not null,idhome integer not null, ohlAccount_id integer not null)").ExecuteUpdate();
      ICriteria criteria = this._session.CreateCriteria(typeof (Kvartplata.Classes.Period));
      string propertyName = "PeriodName";
      DateTime dateTime = this.mpPeriodPay.Value;
      int year = dateTime.Year;
      dateTime = this.mpPeriodPay.Value;
      int month = dateTime.Month;
      int day = 1;
      // ISSUE: variable of a boxed type
      DateTime local = new DateTime(year, month, day);
      SimpleExpression simpleExpression = Restrictions.Eq(propertyName, (object) local);
      Kvartplata.Classes.Period period = (Kvartplata.Classes.Period) criteria.Add((ICriterion) simpleExpression).List()[0];
      IList<OhlPayment> ohlPaymentsAdd = this.ohlPaymentsAdd;
      List<OhlPayment> ohlPaymentList = new List<OhlPayment>();
      List<HomeParam> homeParamList = new List<HomeParam>();
      List<Kvartplata.Classes.Home> homeList = new List<Kvartplata.Classes.Home>();
      foreach (ListHome listHome in this._listHome)
      {
        if (listHome.check)
        {
          foreach (OhlPayment ohl in (IEnumerable<OhlPayment>) ohlList)
          {
            if (ohl.OhlAccountId == listHome.OhlAccountId)
              this._session.CreateSQLQuery("insert into " + Kvartplata.Classes.Options.Login + ".tmpKvrplOhlPmntHome values(" + (object) ohl.PaymentId + ", " + (object) listHome.Home.IdHome + ", " + (object) ohl.OhlAccountId + " )").ExecuteUpdate();
          }
        }
      }
      this._session.Flush();
    }

    private void GetListHome()
    {
      ICriteria criteria = this._session.CreateCriteria(typeof (Kvartplata.Classes.Period));
      string propertyName = "PeriodName";
      DateTime dateTime = this.mpPeriodPay.Value;
      int year = dateTime.Year;
      dateTime = this.mpPeriodPay.Value;
      int month = dateTime.Month;
      int day = 1;
      // ISSUE: variable of a boxed type
      DateTime local = new DateTime(year, month, day);
      SimpleExpression simpleExpression = Restrictions.Eq(propertyName, (object) local);
      Kvartplata.Classes.Period period = (Kvartplata.Classes.Period) criteria.Add((ICriterion) simpleExpression).List()[0];
      DataGridViewSelectedRowCollection selectedRows = this.dgvPaymentsForAdd.SelectedRows;
      List<OhlPayment> ohlPaymentList = new List<OhlPayment>();
      List<HomeParam> homeParamList = new List<HomeParam>();
      List<ListHome> source = new List<ListHome>();
      List<int> intList = new List<int>();
      if (this._thread != null)
      {
        this._thread.Abort();
        this._thread.Join(1000);
        this._thread = new Thread(new ThreadStart(this.ShoeMessageWait));
      }
      if (this._thread == null)
        this._thread = new Thread(new ThreadStart(this.ShoeMessageWait));
      this._thread.Start();
      foreach (DataGridViewRow dataGridViewRow in (BaseCollection) selectedRows)
      {
        OhlPayment dataBoundItem = (OhlPayment) dataGridViewRow.DataBoundItem;
        IList<Kvartplata.Classes.Home> homeList = this._session.CreateQuery("select hm.Home from HomeParam hm where hm.Param.ParamId=309 and hm.ParamValue=:ohlAccId and hm.DBeg<=:period and hm.DEnd>=:period ").SetParameter<double>("ohlAccId", Convert.ToDouble(dataBoundItem.OhlAccountId)).SetParameter<DateTime?>("period", period.PeriodName).List<Kvartplata.Classes.Home>();
        foreach (int num in (IEnumerable) this._session.CreateSQLQuery("select idhome from dba.lsclient ls inner join dba.lsbalance lb on lb.client_id=ls.client_id and period_id=:period and supplier_id=(select supplier_id from dba.dcSupplier where perfomer_id = (select idbaseorg from dba.lsarenda where client_id=:client))").SetParameter<int>("period", period.PeriodId).SetParameter<int>("client", this._client.ClientId).List())
          intList.Add(Convert.ToInt32(num));
        foreach (Kvartplata.Classes.Home home in (IEnumerable<Kvartplata.Classes.Home>) homeList)
        {
          if (intList.Contains(home.IdHome))
          {
            List<ListHome> listHomeList = source;
            ListHome listHome = new ListHome();
            listHome.Home = home;
            int ohlAccountId = dataBoundItem.OhlAccountId;
            listHome.OhlAccountId = ohlAccountId;
            int num = 0;
            listHome.check = num != 0;
            listHomeList.Add(listHome);
          }
        }
      }
      this._listHome.AddRange((IEnumerable<ListHome>) source.OrderBy<ListHome, string>((Func<ListHome, string>) (x => x.Home.Str.NameStr)).ThenBy<ListHome, string>((Func<ListHome, string>) (x => x.Home.NHome)).ToList<ListHome>());
      if (this.messageWait == null)
        return;
      /*this.messageWait.Invoke((Delegate) (() => this.messageWait.Close()));*/
    }

    private void butCheckHome_Click(object sender, EventArgs e)
    {
      if (!this._checkUncheck)
      {
        foreach (ListHome listHome in this.dvgHomeList.DataSource as List<ListHome>)
          listHome.check = true;
        this.butCheckHome.Text = "Снять все";
        this._checkUncheck = true;
        this.dvgHomeList.Refresh();
        this.dvgHomeList_CellValueChanged((object) null, (DataGridViewCellEventArgs) null);
      }
      else
      {
        if (!this._checkUncheck)
          return;
        foreach (ListHome listHome in this.dvgHomeList.DataSource as List<ListHome>)
          listHome.check = false;
        this.butCheckHome.Text = "Выделить все";
        this._checkUncheck = false;
        this.dvgHomeList.Refresh();
        this.dvgHomeList_CellValueChanged((object) null, (DataGridViewCellEventArgs) null);
      }
    }

    private void dgvPayments_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      foreach (DataGridViewBand row in (IEnumerable) this.dgvPayments.Rows)
        row.Selected = false;
      this.dgvPayments.Rows[e.RowIndex].Selected = true;
      this.dgvPayments.Focus();
    }

    private void TsMiReverse_Click(object sender, EventArgs e)
    {
      this.reverseBut_Click((object) null, (EventArgs) null);
    }

    private void TsMiHomeList_Click(object sender, EventArgs e)
    {
      this.butHmParam_Click((object) null, (EventArgs) null);
    }

    private void dgvPaymentsForAdd_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      foreach (DataGridViewBand row in (IEnumerable) this.dgvPaymentsForAdd.Rows)
        row.Selected = false;
      this.dgvPaymentsForAdd.Rows[e.RowIndex].Selected = true;
      this.dgvPaymentsForAdd.Focus();
    }

    private void dgvPaymentsForAdd_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
    {
      e.Control.KeyPress -= new KeyPressEventHandler(this.EditingControl_KeyPress);
      e.Control.KeyPress += new KeyPressEventHandler(this.EditingControl_KeyPress);
    }

    private void EditingControl_KeyPress(object sender, KeyPressEventArgs e)
    {
      char keyChar = e.KeyChar;
      if ((int) e.KeyChar >= 48 && (int) e.KeyChar <= 57 || ((int) keyChar == 8 || (int) keyChar == 44) || (int) keyChar == 45)
        return;
      e.Handled = true;
    }

    private void dgvPaymentsForAdd_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      if (e.Exception == null)
        return;
      int num = (int) MessageBox.Show("Неверный ввод данных");
    }

    private void dvgHomeList_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
    {
      e.Control.MouseClick -= new MouseEventHandler(this.MouseClick_Event);
      e.Control.MouseClick += new MouseEventHandler(this.MouseClick_Event);
    }

    private void MouseClick_Event(object sender, MouseEventArgs e)
    {
      List<ListHome> dataSource = this.dvgHomeList.DataSource as List<ListHome>;
      if (dataSource == null)
        return;
      List<ListHome> source = dataSource;
      Func<ListHome, bool> func = (Func<ListHome, bool>) (x => x.check);
      Func<ListHome, bool> predicate = null;
      if (source.Count<ListHome>(predicate) > 0)
        this.butOkHmList.Enabled = true;
      else
        this.butOkHmList.Enabled = false;
    }

    private void dvgHomeList_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
      this.dvgHomeList.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void dvgHomeList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      List<ListHome> dataSource = this.dvgHomeList.DataSource as List<ListHome>;
      if (dataSource == null)
        return;
      List<ListHome> source = dataSource;
      Func<ListHome, bool> func = (Func<ListHome, bool>) (x => x.check);
      Func<ListHome, bool> predicate = null;
      if (source.Count<ListHome>(predicate) > 0)
        this.butOkHmList.Enabled = true;
      else
        this.butOkHmList.Enabled = false;
    }

    private void butNullMinusPayment_Click(object sender, EventArgs e)
    {
      IList<OhlPayment> dataSource = (IList<OhlPayment>) this.dgvPaymentsForAdd.DataSource;
      foreach (OhlPayment ohlPayment in (IEnumerable<OhlPayment>) this.ohlPaymentsAdd)
      {
        if (dataSource.Contains(ohlPayment) && ohlPayment.Rent < 0.0)
          ohlPayment.Rent = 0.0;
      }
      this.RefreshSum();
      this.dgvPaymentsForAdd.Refresh();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmPaymentOhl));
      this.gbAttribute = new GroupBox();
      this.groupBox1 = new GroupBox();
      this.chbPeniNegative = new CheckBox();
      this.chbRentNegative = new CheckBox();
      this.txbBankFilter = new TextBox();
      this.label1 = new Label();
      this.chbNotEqualsSum = new CheckBox();
      this.btnFilter = new Button();
      this.label4 = new Label();
      this.txbAccountFilter = new TextBox();
      this.cmbPayDoc = new ComboBox();
      this.lblPayDoc = new Label();
      this.mpPeriodPay = new Kvartplata.Classes.MonthPicker();
      this.mpPeriod = new Kvartplata.Classes.MonthPicker();
      this.lblPeriodPay = new Label();
      this.dtmpDate = new DateTimePicker();
      this.lblDate = new Label();
      this.cmbPurpose = new ComboBox();
      this.lblPurpose = new Label();
      this.cmbSource = new ComboBox();
      this.lblSource = new Label();
      this.lblPeriod = new Label();
      this.butHmParam = new Button();
      this.reverseBut = new Button();
      this.pnBtn = new Panel();
      this.butNullMinusPayment = new Button();
      this.butSaveReverse = new Button();
      this.btnClearRentPeni = new Button();
      this.btnClearRent = new Button();
      this.btnSave = new Button();
      this.btnExit = new Button();
      this.dgvPaymentsForAdd = new DataGridView();
      this.Account = new DataGridViewTextBoxColumn();
      this.Bank = new DataGridViewTextBoxColumn();
      this.Rent = new DataGridViewTextBoxColumn();
      this.RentPeni = new DataGridViewTextBoxColumn();
      this.RentAll = new DataGridViewTextBoxColumn();
      this.all = new DataGridViewTextBoxColumn();
      this.RentForClients = new DataGridViewTextBoxColumn();
      this.RentPeniForClient = new DataGridViewTextBoxColumn();
      this.cmsHomeList = new ContextMenuStrip(this.components);
      this.TsMiHomeList = new ToolStripMenuItem();
      this.tcMain = new TabControl();
      this.tbpView = new TabPage();
      this.dgvPayments = new DataGridView();
      this.Account2 = new DataGridViewTextBoxColumn();
      this.Bank2 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
      this.Rentt = new DataGridViewTextBoxColumn();
      this.Peni2 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn11 = new DataGridViewTextBoxColumn();
      this.PaymentDate = new Kvartplata.Classes.MaskDateColumn();
      this.PeriodPay = new Kvartplata.Classes.MaskDateColumn();
      this.Period = new Counters.Classes.MaskDateColumn();
      this.RentForClients2 = new DataGridViewTextBoxColumn();
      this.RentPeniForClients2 = new DataGridViewTextBoxColumn();
      this.RentForClientsRev = new DataGridViewTextBoxColumn();
      this.RentPeniForClientsRev = new DataGridViewTextBoxColumn();
      this.cmsReverse = new ContextMenuStrip(this.components);
      this.TsMiReverse = new ToolStripMenuItem();
      this.tbpAdding = new TabPage();
      this.panelHomeList = new Panel();
      this.panelHmList = new Panel();
      this.dvgHomeList = new DataGridView();
      this.check = new DataGridViewCheckBoxColumn();
      this.Address = new DataGridViewTextBoxColumn();
      this.panelHeaderHm = new Panel();
      this.labPay = new Label();
      this.label2 = new Label();
      this.panelYesNO = new Panel();
      this.butCancelHmList = new Button();
      this.butCheckHome = new Button();
      this.butOkHmList = new Button();
      this.tabReverse = new TabPage();
      this.dvgReverse = new DataGridView();
      this.Account3 = new DataGridViewTextBoxColumn();
      this.BankName3 = new DataGridViewTextBoxColumn();
      this.Rent3 = new DataGridViewTextBoxColumn();
      this.RentPeni3 = new DataGridViewTextBoxColumn();
      this.RentAll3 = new DataGridViewTextBoxColumn();
      this.All3 = new DataGridViewTextBoxColumn();
      this.maskDateColumn3 = new Kvartplata.Classes.MaskDateColumn();
      this.PeriodPay3 = new Kvartplata.Classes.MaskDateColumn();
      this.Period1 = new Counters.Classes.MaskDateColumn();
      this.panelMain = new Panel();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
      this.maskDateColumn1 = new Kvartplata.Classes.MaskDateColumn();
      this.dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn10 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn12 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn13 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn14 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn15 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn16 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn17 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn18 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn19 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn20 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn21 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn22 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn23 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn24 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn25 = new DataGridViewTextBoxColumn();
      this.maskDateColumn2 = new Kvartplata.Classes.MaskDateColumn();
      this.gbAttribute.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.pnBtn.SuspendLayout();
      ((ISupportInitialize) this.dgvPaymentsForAdd).BeginInit();
      this.cmsHomeList.SuspendLayout();
      this.tcMain.SuspendLayout();
      this.tbpView.SuspendLayout();
      ((ISupportInitialize) this.dgvPayments).BeginInit();
      this.cmsReverse.SuspendLayout();
      this.tbpAdding.SuspendLayout();
      this.panelHomeList.SuspendLayout();
      this.panelHmList.SuspendLayout();
      ((ISupportInitialize) this.dvgHomeList).BeginInit();
      this.panelHeaderHm.SuspendLayout();
      this.panelYesNO.SuspendLayout();
      this.tabReverse.SuspendLayout();
      ((ISupportInitialize) this.dvgReverse).BeginInit();
      this.panelMain.SuspendLayout();
      this.SuspendLayout();
      this.gbAttribute.Controls.Add((Control) this.groupBox1);
      this.gbAttribute.Controls.Add((Control) this.cmbPayDoc);
      this.gbAttribute.Controls.Add((Control) this.lblPayDoc);
      this.gbAttribute.Controls.Add((Control) this.mpPeriodPay);
      this.gbAttribute.Controls.Add((Control) this.mpPeriod);
      this.gbAttribute.Controls.Add((Control) this.lblPeriodPay);
      this.gbAttribute.Controls.Add((Control) this.dtmpDate);
      this.gbAttribute.Controls.Add((Control) this.lblDate);
      this.gbAttribute.Controls.Add((Control) this.cmbPurpose);
      this.gbAttribute.Controls.Add((Control) this.lblPurpose);
      this.gbAttribute.Controls.Add((Control) this.cmbSource);
      this.gbAttribute.Controls.Add((Control) this.lblSource);
      this.gbAttribute.Controls.Add((Control) this.lblPeriod);
      this.gbAttribute.Dock = DockStyle.Top;
      this.gbAttribute.Location = new Point(0, 0);
      this.gbAttribute.Margin = new Padding(4);
      this.gbAttribute.Name = "gbAttribute";
      this.gbAttribute.Padding = new Padding(4);
      this.gbAttribute.Size = new Size(1163, 204);
      this.gbAttribute.TabIndex = 2;
      this.gbAttribute.TabStop = false;
      this.gbAttribute.Text = "Атрибуты";
      this.groupBox1.Controls.Add((Control) this.chbPeniNegative);
      this.groupBox1.Controls.Add((Control) this.chbRentNegative);
      this.groupBox1.Controls.Add((Control) this.txbBankFilter);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Controls.Add((Control) this.chbNotEqualsSum);
      this.groupBox1.Controls.Add((Control) this.btnFilter);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.txbAccountFilter);
      this.groupBox1.Location = new Point(615, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(541, 196);
      this.groupBox1.TabIndex = 27;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Фильтр";
      this.chbPeniNegative.AutoSize = true;
      this.chbPeniNegative.Location = new Point(194, 163);
      this.chbPeniNegative.Name = "chbPeniNegative";
      this.chbPeniNegative.Size = new Size(166, 20);
      this.chbPeniNegative.TabIndex = 7;
      this.chbPeniNegative.Text = "Отрицательные пени";
      this.chbPeniNegative.UseVisualStyleBackColor = true;
      this.chbPeniNegative.CheckedChanged += new EventHandler(this.chbPeniNegative_CheckedChanged);
      this.chbRentNegative.AutoSize = true;
      this.chbRentNegative.Location = new Point(6, 163);
      this.chbRentNegative.Name = "chbRentNegative";
      this.chbRentNegative.Size = new Size(182, 20);
      this.chbRentNegative.TabIndex = 6;
      this.chbRentNegative.Text = "Отрицательный платеж";
      this.chbRentNegative.UseVisualStyleBackColor = true;
      this.chbRentNegative.CheckedChanged += new EventHandler(this.chbRentNegative_CheckedChanged);
      this.txbBankFilter.Location = new Point(6, 88);
      this.txbBankFilter.Name = "txbBankFilter";
      this.txbBankFilter.Size = new Size(175, 22);
      this.txbBankFilter.TabIndex = 5;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 69);
      this.label1.Name = "label1";
      this.label1.Size = new Size(40, 16);
      this.label1.TabIndex = 4;
      this.label1.Text = "Банк";
      this.chbNotEqualsSum.AutoSize = true;
      this.chbNotEqualsSum.Location = new Point(6, 132);
      this.chbNotEqualsSum.Name = "chbNotEqualsSum";
      this.chbNotEqualsSum.Size = new Size(213, 20);
      this.chbNotEqualsSum.TabIndex = 3;
      this.chbNotEqualsSum.Text = "Только не совпавшие суммы";
      this.chbNotEqualsSum.UseVisualStyleBackColor = true;
      this.chbNotEqualsSum.CheckedChanged += new EventHandler(this.chbNotEqualsSum_CheckedChanged);
      this.btnFilter.Location = new Point(454, 163);
      this.btnFilter.Name = "btnFilter";
      this.btnFilter.Size = new Size(82, 27);
      this.btnFilter.TabIndex = 2;
      this.btnFilter.Text = "Фильтр";
      this.btnFilter.UseVisualStyleBackColor = true;
      this.btnFilter.Click += new EventHandler(this.btnFilter_Click);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 25);
      this.label4.Name = "label4";
      this.label4.Size = new Size(40, 16);
      this.label4.TabIndex = 1;
      this.label4.Text = "Счет";
      this.txbAccountFilter.Location = new Point(6, 44);
      this.txbAccountFilter.Name = "txbAccountFilter";
      this.txbAccountFilter.Size = new Size(100, 22);
      this.txbAccountFilter.TabIndex = 0;
      this.cmbPayDoc.FormattingEnabled = true;
      this.cmbPayDoc.Location = new Point(298, 155);
      this.cmbPayDoc.Margin = new Padding(4);
      this.cmbPayDoc.Name = "cmbPayDoc";
      this.cmbPayDoc.Size = new Size(294, 24);
      this.cmbPayDoc.TabIndex = 6;
      this.lblPayDoc.AutoSize = true;
      this.lblPayDoc.Location = new Point(294, 133);
      this.lblPayDoc.Margin = new Padding(4, 0, 4, 0);
      this.lblPayDoc.Name = "lblPayDoc";
      this.lblPayDoc.Size = new Size(188, 16);
      this.lblPayDoc.TabIndex = 26;
      this.lblPayDoc.Text = "Тип платежного документа";
      this.mpPeriodPay.CustomFormat = "MMMM yyyy";
      this.mpPeriodPay.Format = DateTimePickerFormat.Custom;
      this.mpPeriodPay.Location = new Point(13, 102);
      this.mpPeriodPay.Margin = new Padding(4);
      this.mpPeriodPay.MaxDate = new DateTime(2999, 12, 31, 0, 0, 0, 0);
      this.mpPeriodPay.Name = "mpPeriodPay";
      this.mpPeriodPay.OldMonth = 0;
      this.mpPeriodPay.ShowUpDown = true;
      this.mpPeriodPay.Size = new Size(185, 22);
      this.mpPeriodPay.TabIndex = 1;
      this.mpPeriodPay.ValueChanged += new EventHandler(this.mpPeriodPay_ValueChanged);
      this.mpPeriod.CustomFormat = "MMMM yyyy";
      this.mpPeriod.Format = DateTimePickerFormat.Custom;
      this.mpPeriod.Location = new Point(13, 44);
      this.mpPeriod.Margin = new Padding(4);
      this.mpPeriod.MaxDate = new DateTime(2999, 12, 31, 0, 0, 0, 0);
      this.mpPeriod.Name = "mpPeriod";
      this.mpPeriod.OldMonth = 0;
      this.mpPeriod.ShowUpDown = true;
      this.mpPeriod.Size = new Size(185, 22);
      this.mpPeriod.TabIndex = 0;
      this.mpPeriod.ValueChanged += new EventHandler(this.mpPeriod_ValueChanged);
      this.lblPeriodPay.AutoSize = true;
      this.lblPeriodPay.Location = new Point(9, 79);
      this.lblPeriodPay.Margin = new Padding(4, 0, 4, 0);
      this.lblPeriodPay.Name = "lblPeriodPay";
      this.lblPeriodPay.Size = new Size(77, 16);
      this.lblPeriodPay.TabIndex = 19;
      this.lblPeriodPay.Text = "Платеж за";
      this.dtmpDate.AllowDrop = true;
      this.dtmpDate.Location = new Point(13, 156);
      this.dtmpDate.Margin = new Padding(4);
      this.dtmpDate.MaxDate = new DateTime(2999, 12, 31, 0, 0, 0, 0);
      this.dtmpDate.Name = "dtmpDate";
      this.dtmpDate.Size = new Size(265, 22);
      this.dtmpDate.TabIndex = 2;
      this.lblDate.AutoSize = true;
      this.lblDate.Location = new Point(9, 133);
      this.lblDate.Margin = new Padding(4, 0, 4, 0);
      this.lblDate.Name = "lblDate";
      this.lblDate.Size = new Size(91, 16);
      this.lblDate.TabIndex = 17;
      this.lblDate.Text = "Дата оплаты";
      this.cmbPurpose.FormattingEnabled = true;
      this.cmbPurpose.Location = new Point(298, 101);
      this.cmbPurpose.Margin = new Padding(4);
      this.cmbPurpose.Name = "cmbPurpose";
      this.cmbPurpose.Size = new Size(294, 24);
      this.cmbPurpose.TabIndex = 5;
      this.lblPurpose.AutoSize = true;
      this.lblPurpose.Location = new Point(294, 81);
      this.lblPurpose.Margin = new Padding(4, 0, 4, 0);
      this.lblPurpose.Name = "lblPurpose";
      this.lblPurpose.Size = new Size(149, 16);
      this.lblPurpose.TabIndex = 15;
      this.lblPurpose.Text = "Назначение платежа";
      this.cmbSource.AutoCompleteMode = AutoCompleteMode.Append;
      this.cmbSource.AutoCompleteSource = AutoCompleteSource.CustomSource;
      this.cmbSource.FormattingEnabled = true;
      this.cmbSource.Location = new Point(298, 44);
      this.cmbSource.Margin = new Padding(4);
      this.cmbSource.Name = "cmbSource";
      this.cmbSource.Size = new Size(294, 24);
      this.cmbSource.TabIndex = 3;
      this.lblSource.AutoSize = true;
      this.lblSource.Location = new Point(294, 21);
      this.lblSource.Margin = new Padding(4, 0, 4, 0);
      this.lblSource.Name = "lblSource";
      this.lblSource.Size = new Size(71, 16);
      this.lblSource.TabIndex = 13;
      this.lblSource.Text = "Источник";
      this.lblPeriod.AutoSize = true;
      this.lblPeriod.Location = new Point(9, 22);
      this.lblPeriod.Margin = new Padding(5, 0, 5, 0);
      this.lblPeriod.Name = "lblPeriod";
      this.lblPeriod.Size = new Size(58, 16);
      this.lblPeriod.TabIndex = 12;
      this.lblPeriod.Text = "Период";
      this.butHmParam.Image = (Image) Resources.edit;
      this.butHmParam.ImageAlign = ContentAlignment.MiddleLeft;
      this.butHmParam.Location = new Point(203, 5);
      this.butHmParam.Name = "butHmParam";
      this.butHmParam.Size = new Size(161, 30);
      this.butHmParam.TabIndex = 9;
      this.butHmParam.Text = "Фильтр по домам";
      this.butHmParam.TextAlign = ContentAlignment.MiddleRight;
      this.butHmParam.UseVisualStyleBackColor = true;
      this.butHmParam.Visible = false;
      this.butHmParam.Click += new EventHandler(this.butHmParam_Click);
      this.reverseBut.Image = (Image) Resources.db_insert;
      this.reverseBut.ImageAlign = ContentAlignment.MiddleLeft;
      this.reverseBut.Location = new Point(3, 5);
      this.reverseBut.Name = "reverseBut";
      this.reverseBut.Size = new Size(185, 30);
      this.reverseBut.TabIndex = 8;
      this.reverseBut.Text = "Сторнировать платеж";
      this.reverseBut.TextAlign = ContentAlignment.MiddleRight;
      this.reverseBut.UseVisualStyleBackColor = true;
      this.reverseBut.Click += new EventHandler(this.reverseBut_Click);
      this.pnBtn.BorderStyle = BorderStyle.FixedSingle;
      this.pnBtn.Controls.Add((Control) this.butNullMinusPayment);
      this.pnBtn.Controls.Add((Control) this.butHmParam);
      this.pnBtn.Controls.Add((Control) this.butSaveReverse);
      this.pnBtn.Controls.Add((Control) this.reverseBut);
      this.pnBtn.Controls.Add((Control) this.btnClearRentPeni);
      this.pnBtn.Controls.Add((Control) this.btnClearRent);
      this.pnBtn.Controls.Add((Control) this.btnSave);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 777);
      this.pnBtn.Margin = new Padding(4);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(1163, 40);
      this.pnBtn.TabIndex = 3;
      this.butNullMinusPayment.Location = new Point(727, 5);
      this.butNullMinusPayment.Name = "butNullMinusPayment";
      this.butNullMinusPayment.Size = new Size(178, 30);
      this.butNullMinusPayment.TabIndex = 10;
      this.butNullMinusPayment.Text = "Обнулить \"-\" платежи";
      this.butNullMinusPayment.UseVisualStyleBackColor = true;
      this.butNullMinusPayment.Visible = false;
      this.butNullMinusPayment.Click += new EventHandler(this.butNullMinusPayment_Click);
      this.butSaveReverse.Image = (Image) Resources.Tick;
      this.butSaveReverse.ImageAlign = ContentAlignment.MiddleLeft;
      this.butSaveReverse.Location = new Point(4, 5);
      this.butSaveReverse.Name = "butSaveReverse";
      this.butSaveReverse.Size = new Size(111, 30);
      this.butSaveReverse.TabIndex = 4;
      this.butSaveReverse.Text = "Сохранить";
      this.butSaveReverse.TextAlign = ContentAlignment.MiddleRight;
      this.butSaveReverse.UseVisualStyleBackColor = true;
      this.butSaveReverse.Visible = false;
      this.butSaveReverse.Click += new EventHandler(this.butSaveReverse_Click);
      this.btnClearRentPeni.Location = new Point(545, 5);
      this.btnClearRentPeni.Margin = new Padding(4);
      this.btnClearRentPeni.Name = "btnClearRentPeni";
      this.btnClearRentPeni.Size = new Size(175, 30);
      this.btnClearRentPeni.TabIndex = 3;
      this.btnClearRentPeni.Text = "Обнулить пени";
      this.btnClearRentPeni.UseVisualStyleBackColor = true;
      this.btnClearRentPeni.Visible = false;
      this.btnClearRentPeni.Click += new EventHandler(this.btnClearRentPeni_Click);
      this.btnClearRent.Location = new Point(371, 5);
      this.btnClearRent.Margin = new Padding(4);
      this.btnClearRent.Name = "btnClearRent";
      this.btnClearRent.Size = new Size(166, 30);
      this.btnClearRent.TabIndex = 2;
      this.btnClearRent.Text = "Обнулить платежи";
      this.btnClearRent.UseVisualStyleBackColor = true;
      this.btnClearRent.Visible = false;
      this.btnClearRent.Click += new EventHandler(this.btnClearRent_Click);
      this.btnSave.Image = (Image) Resources.Tick;
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.Location = new Point(4, 5);
      this.btnSave.Margin = new Padding(4);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(110, 30);
      this.btnSave.TabIndex = 0;
      this.btnSave.Text = "Сохранить";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Visible = false;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(1069, 5);
      this.btnExit.Margin = new Padding(4);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(80, 30);
      this.btnExit.TabIndex = 1;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.dgvPaymentsForAdd.BackgroundColor = Color.AliceBlue;
      this.dgvPaymentsForAdd.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
      this.dgvPaymentsForAdd.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvPaymentsForAdd.Columns.AddRange((DataGridViewColumn) this.Account, (DataGridViewColumn) this.Bank, (DataGridViewColumn) this.Rent, (DataGridViewColumn) this.RentPeni, (DataGridViewColumn) this.RentAll, (DataGridViewColumn) this.all, (DataGridViewColumn) this.RentForClients, (DataGridViewColumn) this.RentPeniForClient);
      this.dgvPaymentsForAdd.ContextMenuStrip = this.cmsHomeList;
      this.dgvPaymentsForAdd.Dock = DockStyle.Fill;
      this.dgvPaymentsForAdd.GridColor = SystemColors.ControlDarkDark;
      this.dgvPaymentsForAdd.Location = new Point(3, 3);
      this.dgvPaymentsForAdd.Margin = new Padding(4);
      this.dgvPaymentsForAdd.Name = "dgvPaymentsForAdd";
      this.dgvPaymentsForAdd.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgvPaymentsForAdd.Size = new Size(765, 536);
      this.dgvPaymentsForAdd.TabIndex = 4;
      this.dgvPaymentsForAdd.CellEndEdit += new DataGridViewCellEventHandler(this.dgvPaymentsForAdd_CellEndEdit);
      this.dgvPaymentsForAdd.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvPaymentsForAdd_CellFormatting);
      this.dgvPaymentsForAdd.CellMouseDown += new DataGridViewCellMouseEventHandler(this.dgvPaymentsForAdd_CellMouseDown);
      this.dgvPaymentsForAdd.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvPaymentsForAdd_ColumnWidthChanged);
      this.dgvPaymentsForAdd.DataError += new DataGridViewDataErrorEventHandler(this.dgvPaymentsForAdd_DataError);
      this.dgvPaymentsForAdd.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgvPaymentsForAdd_EditingControlShowing);
      this.dgvPaymentsForAdd.SelectionChanged += new EventHandler(this.dgvPaymentsForAdd_SelectionChanged);
      this.Account.DataPropertyName = "Account";
      this.Account.HeaderText = "Счет";
      this.Account.Name = "Account";
      this.Account.ReadOnly = true;
      this.Account.Width = 65;
      this.Bank.DataPropertyName = "BankName";
      this.Bank.HeaderText = "Банк";
      this.Bank.Name = "Bank";
      this.Bank.ReadOnly = true;
      this.Bank.Width = 65;
      this.Rent.DataPropertyName = "Rent";
      this.Rent.HeaderText = "Платеж";
      this.Rent.Name = "Rent";
      this.Rent.Width = 83;
      this.RentPeni.DataPropertyName = "RentPeni";
      this.RentPeni.HeaderText = "Пени";
      this.RentPeni.Name = "RentPeni";
      this.RentPeni.Width = 67;
      this.RentAll.DataPropertyName = "RentAll";
      this.RentAll.HeaderText = "Итого";
      this.RentAll.Name = "RentAll";
      this.RentAll.ReadOnly = true;
      this.RentAll.Visible = false;
      this.RentAll.Width = 72;
      this.all.DataPropertyName = "All";
      this.all.HeaderText = "Итого";
      this.all.Name = "all";
      this.all.Width = 72;
      this.RentForClients.DataPropertyName = "RentForClients";
      this.RentForClients.HeaderText = "Сумма платежей по разбивке";
      this.RentForClients.Name = "RentForClients";
      this.RentForClients.ReadOnly = true;
      this.RentForClients.Visible = false;
      this.RentForClients.Width = 150;
      this.RentPeniForClient.DataPropertyName = "RentPeniForClients";
      this.RentPeniForClient.HeaderText = "Сумма пеней по разбивке";
      this.RentPeniForClient.Name = "RentPeniForClient";
      this.RentPeniForClient.ReadOnly = true;
      this.RentPeniForClient.Visible = false;
      this.RentPeniForClient.Width = 129;
      this.cmsHomeList.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.TsMiHomeList
      });
      this.cmsHomeList.Name = "cmsHomeList";
      this.cmsHomeList.Size = new Size(173, 26);
      this.TsMiHomeList.Name = "TsMiHomeList";
      this.TsMiHomeList.Size = new Size(172, 22);
      this.TsMiHomeList.Text = "Фильтр по домам";
      this.TsMiHomeList.Click += new EventHandler(this.TsMiHomeList_Click);
      this.tcMain.Controls.Add((Control) this.tbpView);
      this.tcMain.Controls.Add((Control) this.tbpAdding);
      this.tcMain.Controls.Add((Control) this.tabReverse);
      this.tcMain.Dock = DockStyle.Fill;
      this.tcMain.Location = new Point(0, 0);
      this.tcMain.Name = "tcMain";
      this.tcMain.SelectedIndex = 0;
      this.tcMain.Size = new Size(1161, 571);
      this.tcMain.TabIndex = 5;
      this.tcMain.SelectedIndexChanged += new EventHandler(this.tcMain_SelectedIndexChanged);
      this.tbpView.Controls.Add((Control) this.dgvPayments);
      this.tbpView.Location = new Point(4, 25);
      this.tbpView.Name = "tbpView";
      this.tbpView.Padding = new Padding(3);
      this.tbpView.Size = new Size(1153, 542);
      this.tbpView.TabIndex = 0;
      this.tbpView.Text = "Занесенные";
      this.tbpView.UseVisualStyleBackColor = true;
      this.dgvPayments.BackgroundColor = Color.AliceBlue;
      this.dgvPayments.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
      this.dgvPayments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvPayments.Columns.AddRange((DataGridViewColumn) this.Account2, (DataGridViewColumn) this.Bank2, (DataGridViewColumn) this.dataGridViewTextBoxColumn8, (DataGridViewColumn) this.Rentt, (DataGridViewColumn) this.Peni2, (DataGridViewColumn) this.dataGridViewTextBoxColumn11, (DataGridViewColumn) this.PaymentDate, (DataGridViewColumn) this.PeriodPay, (DataGridViewColumn) this.Period, (DataGridViewColumn) this.RentForClients2, (DataGridViewColumn) this.RentPeniForClients2, (DataGridViewColumn) this.RentForClientsRev, (DataGridViewColumn) this.RentPeniForClientsRev);
      this.dgvPayments.ContextMenuStrip = this.cmsReverse;
      this.dgvPayments.Dock = DockStyle.Fill;
      this.dgvPayments.GridColor = SystemColors.ControlDarkDark;
      this.dgvPayments.Location = new Point(3, 3);
      this.dgvPayments.Margin = new Padding(4);
      this.dgvPayments.Name = "dgvPayments";
      this.dgvPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgvPayments.Size = new Size(1147, 536);
      this.dgvPayments.TabIndex = 5;
      this.dgvPayments.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvPayments_CellFormatting);
      this.dgvPayments.CellMouseDown += new DataGridViewCellMouseEventHandler(this.dgvPayments_CellMouseDown);
      this.dgvPayments.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvPayments_ColumnWidthChanged);
      this.Account2.DataPropertyName = "Account";
      this.Account2.HeaderText = "Счет";
      this.Account2.Name = "Account2";
      this.Account2.ReadOnly = true;
      this.Account2.Width = 65;
      this.Bank2.DataPropertyName = "BankName";
      this.Bank2.HeaderText = "Банк";
      this.Bank2.Name = "Bank2";
      this.Bank2.ReadOnly = true;
      this.Bank2.Width = 65;
      this.dataGridViewTextBoxColumn8.DataPropertyName = "Rent";
      this.dataGridViewTextBoxColumn8.HeaderText = "Платеж";
      this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
      this.dataGridViewTextBoxColumn8.ReadOnly = true;
      this.dataGridViewTextBoxColumn8.Width = 83;
      this.Rentt.DataPropertyName = "RentPeni";
      this.Rentt.HeaderText = "Пени";
      this.Rentt.Name = "Rentt";
      this.Rentt.ReadOnly = true;
      this.Rentt.Width = 67;
      this.Peni2.DataPropertyName = "RentAll";
      this.Peni2.HeaderText = "Итого";
      this.Peni2.Name = "Peni2";
      this.Peni2.ReadOnly = true;
      this.Peni2.Visible = false;
      this.Peni2.Width = 72;
      this.dataGridViewTextBoxColumn11.DataPropertyName = "All";
      this.dataGridViewTextBoxColumn11.HeaderText = "Итого";
      this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
      this.dataGridViewTextBoxColumn11.ReadOnly = true;
      this.dataGridViewTextBoxColumn11.Width = 72;
      this.PaymentDate.DataPropertyName = "PaymentDate";
      this.PaymentDate.HeaderText = "Дата оплаты";
      this.PaymentDate.Name = "PaymentDate";
      this.PaymentDate.ReadOnly = true;
      this.PeriodPay.DataPropertyName = "PeriodPay";
      this.PeriodPay.HeaderText = "Платеж за";
      this.PeriodPay.Name = "PeriodPay";
      this.PeriodPay.ReadOnly = true;
      this.Period.DataPropertyName = "Period";
      this.Period.HeaderText = "Период";
      this.Period.Name = "Period";
      this.Period.ReadOnly = true;
      this.Period.Visible = false;
      this.RentForClients2.DataPropertyName = "RentForClients";
      this.RentForClients2.HeaderText = "Сумма платежей по разбивке";
      this.RentForClients2.Name = "RentForClients2";
      this.RentForClients2.ReadOnly = true;
      this.RentForClients2.Width = 150;
      this.RentPeniForClients2.DataPropertyName = "RentPeniForClients";
      this.RentPeniForClients2.HeaderText = "Сумма пеней по разбивке";
      this.RentPeniForClients2.Name = "RentPeniForClients2";
      this.RentPeniForClients2.ReadOnly = true;
      this.RentPeniForClients2.Width = 129;
      this.RentForClientsRev.DataPropertyName = "RentForClientsRev";
      this.RentForClientsRev.HeaderText = "Платеж сторно";
      this.RentForClientsRev.Name = "RentForClientsRev";
      this.RentForClientsRev.ReadOnly = true;
      this.RentPeniForClientsRev.DataPropertyName = "RentPeniForClientsRev";
      this.RentPeniForClientsRev.HeaderText = "Пени сторно";
      this.RentPeniForClientsRev.Name = "RentPeniForClientsRev";
      this.RentPeniForClientsRev.ReadOnly = true;
      this.cmsReverse.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.TsMiReverse
      });
      this.cmsReverse.Name = "cmsReverse";
      this.cmsReverse.Size = new Size(196, 26);
      this.TsMiReverse.Name = "TsMiReverse";
      this.TsMiReverse.Size = new Size(195, 22);
      this.TsMiReverse.Text = "Сторнировать платеж";
      this.TsMiReverse.Click += new EventHandler(this.TsMiReverse_Click);
      this.tbpAdding.Controls.Add((Control) this.dgvPaymentsForAdd);
      this.tbpAdding.Controls.Add((Control) this.panelHomeList);
      this.tbpAdding.Location = new Point(4, 25);
      this.tbpAdding.Name = "tbpAdding";
      this.tbpAdding.Padding = new Padding(3);
      this.tbpAdding.Size = new Size(1153, 542);
      this.tbpAdding.TabIndex = 1;
      this.tbpAdding.Text = "Для занесения";
      this.tbpAdding.UseVisualStyleBackColor = true;
      this.panelHomeList.BorderStyle = BorderStyle.FixedSingle;
      this.panelHomeList.Controls.Add((Control) this.panelHmList);
      this.panelHomeList.Controls.Add((Control) this.panelHeaderHm);
      this.panelHomeList.Controls.Add((Control) this.panelYesNO);
      this.panelHomeList.Dock = DockStyle.Right;
      this.panelHomeList.Location = new Point(768, 3);
      this.panelHomeList.Name = "panelHomeList";
      this.panelHomeList.Size = new Size(382, 536);
      this.panelHomeList.TabIndex = 6;
      this.panelHmList.Controls.Add((Control) this.dvgHomeList);
      this.panelHmList.Dock = DockStyle.Fill;
      this.panelHmList.Location = new Point(0, 49);
      this.panelHmList.Name = "panelHmList";
      this.panelHmList.Size = new Size(380, 445);
      this.panelHmList.TabIndex = 2;
      this.dvgHomeList.BackgroundColor = Color.AliceBlue;
      this.dvgHomeList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dvgHomeList.Columns.AddRange((DataGridViewColumn) this.check, (DataGridViewColumn) this.Address);
      this.dvgHomeList.Dock = DockStyle.Fill;
      this.dvgHomeList.Location = new Point(0, 0);
      this.dvgHomeList.Name = "dvgHomeList";
      this.dvgHomeList.Size = new Size(380, 445);
      this.dvgHomeList.TabIndex = 0;
      this.dvgHomeList.CellContentClick += new DataGridViewCellEventHandler(this.dvgHomeList_CellContentClick);
      this.dvgHomeList.CellValueChanged += new DataGridViewCellEventHandler(this.dvgHomeList_CellValueChanged);
      this.dvgHomeList.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dvgHomeList_EditingControlShowing);
      this.check.DataPropertyName = "check";
      this.check.HeaderText = "";
      this.check.Name = "check";
      this.Address.DataPropertyName = "Address";
      this.Address.HeaderText = "Адрес";
      this.Address.Name = "Address";
      this.Address.ReadOnly = true;
      this.Address.Width = 400;
      this.panelHeaderHm.Controls.Add((Control) this.labPay);
      this.panelHeaderHm.Controls.Add((Control) this.label2);
      this.panelHeaderHm.Dock = DockStyle.Top;
      this.panelHeaderHm.Location = new Point(0, 0);
      this.panelHeaderHm.Name = "panelHeaderHm";
      this.panelHeaderHm.Size = new Size(380, 49);
      this.panelHeaderHm.TabIndex = 1;
      this.labPay.AutoSize = true;
      this.labPay.Location = new Point(62, 12);
      this.labPay.Name = "labPay";
      this.labPay.Size = new Size(0, 16);
      this.labPay.TabIndex = 1;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(16, 12);
      this.label2.Name = "label2";
      this.label2.Size = new Size(40, 16);
      this.label2.TabIndex = 0;
      this.label2.Text = "Счет";
      this.panelYesNO.Controls.Add((Control) this.butCancelHmList);
      this.panelYesNO.Controls.Add((Control) this.butCheckHome);
      this.panelYesNO.Controls.Add((Control) this.butOkHmList);
      this.panelYesNO.Dock = DockStyle.Bottom;
      this.panelYesNO.Location = new Point(0, 494);
      this.panelYesNO.Name = "panelYesNO";
      this.panelYesNO.Size = new Size(380, 40);
      this.panelYesNO.TabIndex = 0;
      this.butCancelHmList.Location = new Point(462, 6);
      this.butCancelHmList.Name = "butCancelHmList";
      this.butCancelHmList.Size = new Size(108, 30);
      this.butCancelHmList.TabIndex = 1;
      this.butCancelHmList.Text = "Отмена";
      this.butCancelHmList.UseVisualStyleBackColor = true;
      this.butCancelHmList.Click += new EventHandler(this.butCancelHmList_Click);
      this.butCheckHome.Location = new Point(136, 6);
      this.butCheckHome.Name = "butCheckHome";
      this.butCheckHome.Size = new Size(112, 30);
      this.butCheckHome.TabIndex = 2;
      this.butCheckHome.Text = "Выделить все";
      this.butCheckHome.UseVisualStyleBackColor = true;
      this.butCheckHome.Click += new EventHandler(this.butCheckHome_Click);
      this.butOkHmList.Enabled = false;
      this.butOkHmList.Image = (Image) Resources.Tick;
      this.butOkHmList.ImageAlign = ContentAlignment.MiddleLeft;
      this.butOkHmList.Location = new Point(19, 6);
      this.butOkHmList.Name = "butOkHmList";
      this.butOkHmList.Size = new Size(111, 30);
      this.butOkHmList.TabIndex = 0;
      this.butOkHmList.Text = "Сохранить";
      this.butOkHmList.TextAlign = ContentAlignment.MiddleRight;
      this.butOkHmList.UseVisualStyleBackColor = true;
      this.butOkHmList.Click += new EventHandler(this.butOkHmList_Click);
      this.tabReverse.Controls.Add((Control) this.dvgReverse);
      this.tabReverse.Location = new Point(4, 25);
      this.tabReverse.Name = "tabReverse";
      this.tabReverse.Size = new Size(1153, 542);
      this.tabReverse.TabIndex = 2;
      this.tabReverse.Text = "Сторнирование";
      this.tabReverse.UseVisualStyleBackColor = true;
      this.dvgReverse.BackgroundColor = Color.AliceBlue;
      this.dvgReverse.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dvgReverse.Columns.AddRange((DataGridViewColumn) this.Account3, (DataGridViewColumn) this.BankName3, (DataGridViewColumn) this.Rent3, (DataGridViewColumn) this.RentPeni3, (DataGridViewColumn) this.RentAll3, (DataGridViewColumn) this.All3, (DataGridViewColumn) this.maskDateColumn3, (DataGridViewColumn) this.PeriodPay3, (DataGridViewColumn) this.Period1);
      this.dvgReverse.Dock = DockStyle.Fill;
      this.dvgReverse.Location = new Point(0, 0);
      this.dvgReverse.Name = "dvgReverse";
      this.dvgReverse.Size = new Size(1153, 542);
      this.dvgReverse.TabIndex = 0;
      this.dvgReverse.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dvgReverse_ColumnWidthChanged);
      this.Account3.DataPropertyName = "Account";
      this.Account3.HeaderText = "Счет";
      this.Account3.Name = "Account3";
      this.Account3.ReadOnly = true;
      this.BankName3.DataPropertyName = "BankName";
      this.BankName3.HeaderText = "Банк";
      this.BankName3.Name = "BankName3";
      this.BankName3.ReadOnly = true;
      this.Rent3.DataPropertyName = "Rent";
      this.Rent3.HeaderText = "Платеж";
      this.Rent3.Name = "Rent3";
      this.RentPeni3.DataPropertyName = "RentPeni";
      this.RentPeni3.HeaderText = "Пени";
      this.RentPeni3.Name = "RentPeni3";
      this.RentAll3.DataPropertyName = "RentAll";
      this.RentAll3.HeaderText = "Итого";
      this.RentAll3.Name = "RentAll3";
      this.RentAll3.ReadOnly = true;
      this.RentAll3.Visible = false;
      this.All3.DataPropertyName = "All";
      this.All3.HeaderText = "Итого";
      this.All3.Name = "All3";
      this.maskDateColumn3.DataPropertyName = "PaymentDate";
      this.maskDateColumn3.HeaderText = "Дата оплаты";
      this.maskDateColumn3.Name = "maskDateColumn3";
      this.maskDateColumn3.ReadOnly = true;
      this.PeriodPay3.DataPropertyName = "PeriodPay";
      this.PeriodPay3.HeaderText = "Платеж за";
      this.PeriodPay3.Name = "PeriodPay3";
      this.PeriodPay3.Resizable = DataGridViewTriState.True;
      this.PeriodPay3.SortMode = DataGridViewColumnSortMode.Automatic;
      this.Period1.DataPropertyName = "Period";
      this.Period1.HeaderText = "Период";
      this.Period1.Name = "Period1";
      this.panelMain.BorderStyle = BorderStyle.FixedSingle;
      this.panelMain.Controls.Add((Control) this.tcMain);
      this.panelMain.Dock = DockStyle.Fill;
      this.panelMain.Location = new Point(0, 204);
      this.panelMain.Name = "panelMain";
      this.panelMain.Size = new Size(1163, 573);
      this.panelMain.TabIndex = 7;
      this.dataGridViewTextBoxColumn1.DataPropertyName = "Account";
      this.dataGridViewTextBoxColumn1.HeaderText = "Счет";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.ReadOnly = true;
      this.dataGridViewTextBoxColumn1.Width = 280;
      this.dataGridViewTextBoxColumn2.DataPropertyName = "Rent";
      this.dataGridViewTextBoxColumn2.HeaderText = "Начисления";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn2.ReadOnly = true;
      this.dataGridViewTextBoxColumn2.Width = 280;
      this.dataGridViewTextBoxColumn3.DataPropertyName = "RentPeni";
      this.dataGridViewTextBoxColumn3.HeaderText = "Пени";
      this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
      this.dataGridViewTextBoxColumn3.ReadOnly = true;
      this.dataGridViewTextBoxColumn3.Width = 280;
      this.dataGridViewTextBoxColumn4.DataPropertyName = "RentAll";
      this.dataGridViewTextBoxColumn4.HeaderText = "Итого";
      this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
      this.dataGridViewTextBoxColumn4.ReadOnly = true;
      this.dataGridViewTextBoxColumn4.Visible = false;
      this.dataGridViewTextBoxColumn4.Width = 280;
      this.dataGridViewTextBoxColumn5.DataPropertyName = "All";
      this.dataGridViewTextBoxColumn5.HeaderText = "Column1";
      this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
      this.dataGridViewTextBoxColumn5.ReadOnly = true;
      this.dataGridViewTextBoxColumn5.Width = 224;
      this.dataGridViewTextBoxColumn6.DataPropertyName = "RentAll";
      this.dataGridViewTextBoxColumn6.HeaderText = "Итого";
      this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
      this.dataGridViewTextBoxColumn6.ReadOnly = true;
      this.dataGridViewTextBoxColumn6.Visible = false;
      this.dataGridViewTextBoxColumn6.Width = 72;
      this.dataGridViewTextBoxColumn7.DataPropertyName = "All";
      this.dataGridViewTextBoxColumn7.HeaderText = "Итого";
      this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
      this.dataGridViewTextBoxColumn7.Width = 72;
      this.maskDateColumn1.DataPropertyName = "PeriodPay";
      this.maskDateColumn1.HeaderText = "Платеж за";
      this.maskDateColumn1.Name = "maskDateColumn1";
      this.maskDateColumn1.ReadOnly = true;
      this.dataGridViewTextBoxColumn9.DataPropertyName = "RentPeniForClients";
      this.dataGridViewTextBoxColumn9.HeaderText = "Сумма пеней по разбивке";
      this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
      this.dataGridViewTextBoxColumn9.ReadOnly = true;
      this.dataGridViewTextBoxColumn9.Width = 129;
      this.dataGridViewTextBoxColumn10.DataPropertyName = "RentForClientsRev";
      this.dataGridViewTextBoxColumn10.HeaderText = "Платеж сторно";
      this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
      this.dataGridViewTextBoxColumn10.ReadOnly = true;
      this.dataGridViewTextBoxColumn12.DataPropertyName = "Account";
      this.dataGridViewTextBoxColumn12.HeaderText = "Счет";
      this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
      this.dataGridViewTextBoxColumn12.ReadOnly = true;
      this.dataGridViewTextBoxColumn12.Width = 65;
      this.dataGridViewTextBoxColumn13.DataPropertyName = "BankName";
      this.dataGridViewTextBoxColumn13.HeaderText = "Банк";
      this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
      this.dataGridViewTextBoxColumn13.ReadOnly = true;
      this.dataGridViewTextBoxColumn13.Width = 65;
      this.dataGridViewTextBoxColumn14.DataPropertyName = "Rent";
      this.dataGridViewTextBoxColumn14.HeaderText = "Платеж";
      this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
      this.dataGridViewTextBoxColumn14.Width = 83;
      this.dataGridViewTextBoxColumn15.DataPropertyName = "RentPeni";
      this.dataGridViewTextBoxColumn15.HeaderText = "Пени";
      this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
      this.dataGridViewTextBoxColumn15.Width = 67;
      this.dataGridViewTextBoxColumn16.DataPropertyName = "RentAll";
      this.dataGridViewTextBoxColumn16.HeaderText = "Итого";
      this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
      this.dataGridViewTextBoxColumn16.ReadOnly = true;
      this.dataGridViewTextBoxColumn16.Visible = false;
      this.dataGridViewTextBoxColumn16.Width = 72;
      this.dataGridViewTextBoxColumn17.DataPropertyName = "All";
      this.dataGridViewTextBoxColumn17.HeaderText = "Итого";
      this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
      this.dataGridViewTextBoxColumn17.Width = 72;
      this.dataGridViewTextBoxColumn18.DataPropertyName = "RentForClients";
      this.dataGridViewTextBoxColumn18.HeaderText = "Сумма платежей по разбивке";
      this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
      this.dataGridViewTextBoxColumn18.ReadOnly = true;
      this.dataGridViewTextBoxColumn18.Width = 150;
      this.dataGridViewTextBoxColumn19.DataPropertyName = "RentPeniForClients";
      this.dataGridViewTextBoxColumn19.HeaderText = "Сумма пеней по разбивке";
      this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
      this.dataGridViewTextBoxColumn19.ReadOnly = true;
      this.dataGridViewTextBoxColumn19.Width = 129;
      this.dataGridViewTextBoxColumn20.DataPropertyName = "Account";
      this.dataGridViewTextBoxColumn20.HeaderText = "Счет";
      this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
      this.dataGridViewTextBoxColumn20.ReadOnly = true;
      this.dataGridViewTextBoxColumn21.DataPropertyName = "BankName";
      this.dataGridViewTextBoxColumn21.HeaderText = "Банк";
      this.dataGridViewTextBoxColumn21.Name = "dataGridViewTextBoxColumn21";
      this.dataGridViewTextBoxColumn21.ReadOnly = true;
      this.dataGridViewTextBoxColumn22.DataPropertyName = "Rent";
      this.dataGridViewTextBoxColumn22.HeaderText = "Платеж";
      this.dataGridViewTextBoxColumn22.Name = "dataGridViewTextBoxColumn22";
      this.dataGridViewTextBoxColumn23.DataPropertyName = "RentPeni";
      this.dataGridViewTextBoxColumn23.HeaderText = "Пени";
      this.dataGridViewTextBoxColumn23.Name = "dataGridViewTextBoxColumn23";
      this.dataGridViewTextBoxColumn24.DataPropertyName = "RentAll";
      this.dataGridViewTextBoxColumn24.HeaderText = "Итого";
      this.dataGridViewTextBoxColumn24.Name = "dataGridViewTextBoxColumn24";
      this.dataGridViewTextBoxColumn24.ReadOnly = true;
      this.dataGridViewTextBoxColumn24.Visible = false;
      this.dataGridViewTextBoxColumn25.DataPropertyName = "All";
      this.dataGridViewTextBoxColumn25.HeaderText = "Итого";
      this.dataGridViewTextBoxColumn25.Name = "dataGridViewTextBoxColumn25";
      this.maskDateColumn2.DataPropertyName = "PeriodPay";
      this.maskDateColumn2.HeaderText = "Платеж за";
      this.maskDateColumn2.Name = "maskDateColumn2";
      this.maskDateColumn2.Resizable = DataGridViewTriState.True;
      this.maskDateColumn2.SortMode = DataGridViewColumnSortMode.Automatic;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(1163, 817);
      this.Controls.Add((Control) this.panelMain);
      this.Controls.Add((Control) this.pnBtn);
      this.Controls.Add((Control) this.gbAttribute);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmPaymentOhl";
      this.Text = "Платежи";
      this.Load += new EventHandler(this.FrmPaymentOhl_Load);
      this.Shown += new EventHandler(this.FrmPaymentOhl_Shown);
      this.gbAttribute.ResumeLayout(false);
      this.gbAttribute.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.pnBtn.ResumeLayout(false);
      ((ISupportInitialize) this.dgvPaymentsForAdd).EndInit();
      this.cmsHomeList.ResumeLayout(false);
      this.tcMain.ResumeLayout(false);
      this.tbpView.ResumeLayout(false);
      ((ISupportInitialize) this.dgvPayments).EndInit();
      this.cmsReverse.ResumeLayout(false);
      this.tbpAdding.ResumeLayout(false);
      this.panelHomeList.ResumeLayout(false);
      this.panelHmList.ResumeLayout(false);
      ((ISupportInitialize) this.dvgHomeList).EndInit();
      this.panelHeaderHm.ResumeLayout(false);
      this.panelHeaderHm.PerformLayout();
      this.panelYesNO.ResumeLayout(false);
      this.tabReverse.ResumeLayout(false);
      ((ISupportInitialize) this.dvgReverse).EndInit();
      this.panelMain.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
