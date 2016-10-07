// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmAttribute
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmAttribute : Form
  {
    private FormStateSaver formStateSaver = new FormStateSaver(FrmAttribute.container);
    private string strInputScanner = "";
    private string lastStrClientId = "";
    private string lastStrReceipt = "";
    private bool EndChangedScanner = false;
    private int _serviceId = 0;
    private Logger log = LogManager.GetCurrentClassLogger();
    public IList<Payment> Pays = (IList<Payment>) new List<Payment>();
    private IContainer components = (IContainer) null;
    private ISession session;
    private Payment currentPay;
    private int paymentId;
    private IList paymentList;
    private LsClient currentLs;
    private static IContainer container;
    private bool Peni;
    private IList<Counter> counterList;
    private FrmCurrentPayment frmCurrentPayment;
    private Payment StrahPayment;
    private bool insertId;
    private int city;
    private bool changeText;
    private string org;
    private bool changeDog;
    private string dog;
    private short lastReceipt;
    private bool scanNewCode;
    private bool saveNullVolume;
    private Company _company;
    private Panel pnBtn;
    private Button btnExit;
    private Button btnSave;
    private GroupBox gbAttribute;
    private Label lblPeriodPay;
    private DateTimePicker dtmpDate;
    private Label lblDate;
    private ComboBox cmbPurpose;
    private Label lblPurpose;
    private ComboBox cmbSource;
    private Label lblSource;
    private Label lblPeriod;
    private Label lblPacket;
    private MonthPicker mpPeriodPay;
    private MonthPicker mpPeriod;
    private Timer tmr1;
    private CheckBox chbScanner;
    private Button btnFind;
    public HelpProvider hp;
    private Panel pnKv;
    private Panel pnArenda;
    private GroupBox gbCounters;
    private DataGridView dgvEvidence;
    private Panel pnEvidence;
    private Button btnLoadCounters;
    private Panel pnMain;
    private ComboBox cmbSupplier;
    private Label lblSupplier;
    private Label lblFIO;
    private TextBox tbItog;
    private Label lblItog;
    private TextBox txtStrah;
    private Label lblStrah;
    private TextBox txtClientId;
    private Label lblReceipt;
    private ComboBox cmbReceipt;
    private ComboBox cmbService;
    private Label lblService;
    private TextBox txtPeni;
    private Label lblPeni;
    private TextBox txtSum;
    private Label lblValue;
    private Label lblIdlic;
    private GroupBox gbVid;
    private RadioButton rbAr;
    private RadioButton rbKv;
    private Label lblPayDoc;
    private ComboBox cmbPayDoc;
    private ComboBox cmbArSupplier;
    private Label lblArSupplier;
    private TextBox txbArItog;
    private Label lblArItog;
    private Label lblArReceipt;
    private ComboBox cmbArReceipt;
    private ComboBox cmbArService;
    private Label lblArService;
    private TextBox txbArPeni;
    private Label lblArPeni;
    private TextBox txbArValue;
    private Label lblArValue;
    private ListBox lbOrg;
    private ComboBox cmbO;
    private Label lblNum;
    private Label lblOrg;
    private ListBox lbDog;
    private ComboBox cmbDog;
    private Label lblAr;
    private DataGridView dgvDistribute;
    private TextBox txbPacket;
    private TextBox txbNumber;
    private Label lblNumber;
    private GroupBox gbArrange;
    private RadioButton rbRent;
    private RadioButton rbBalance;
    private Label lblArClient;
    private TextBox txbArClient;
    private ComboBox cmbPerfomer;
    private Label lblPerfomer;
    private ComboBox cmbRecipient;
    private Label lblRecipient;
    private ComboBox cmbArPerfomer;
    private Label lblArPerfomer;
    private ComboBox cmbArRecipient;
    private Label lblArRecipient;
    private ComboBox cmbRecipientAuto;
    private Label lblRecipientAuto;
    private ComboBox cmbArRecipientAuto;
    private Label label1;

    public short Operation { get; set; }

    public FrmAttribute()
    {
      this.InitializeComponent();
      this.formStateSaver.ParentForm = (Form) this;
    }

    public FrmAttribute(int id, LsClient client, Company company)
    {
      this.paymentId = id;
      this._company = company;
      if (this._company == null)
      {
        this._company = new Company();
        this._company.CompanyId = (short) 0;
      }
      this.currentLs = client;
      if (id == 0 && client != null)
        this.insertId = true;
      this.InitializeComponent();
      this.formStateSaver.ParentForm = (Form) this;
    }

    private void FrmAttribute_Load(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      this.strInputScanner = "";
      if (!Options.Kvartplata)
      {
        this.gbVid.Visible = false;
        this.rbAr.Checked = true;
      }
      if (!Options.Arenda)
        this.gbVid.Visible = false;
      IList<SourcePay> sourcePayList = this.session.CreateCriteria(typeof (SourcePay)).AddOrder(Order.Asc("SourcePayName")).List<SourcePay>();
      if ((uint) sourcePayList.Count > 0U)
      {
        this.cmbSource.DataSource = (object) sourcePayList;
        this.cmbSource.ValueMember = "SourcePayId";
        this.cmbSource.DisplayMember = "SourcePayName";
        IList list1 = this.session.CreateCriteria(typeof (PurposePay)).AddOrder(Order.Asc("PurposePayId")).List();
        if ((uint) list1.Count > 0U)
        {
          this.cmbPurpose.DataSource = (object) list1;
          this.cmbPurpose.ValueMember = "PurposePayId";
          this.cmbPurpose.DisplayMember = "PurposePayName";
          IList list2 = this.session.CreateCriteria(typeof (PayDoc)).AddOrder(Order.Asc("PayDocId")).List();
          if ((uint) list2.Count > 0U)
          {
            this.cmbPayDoc.DataSource = (object) list2;
            this.cmbPayDoc.ValueMember = "PayDocId";
            this.cmbPayDoc.DisplayMember = "PayDocName";
            IList<Service> serviceList1 = this.session.CreateQuery(string.Format("select s from Service s where s.Root=0 and s.ServiceId<>0 order by " + Options.SortService)).List<Service>();
            IList<Service> serviceList2 = this.session.CreateQuery(string.Format("select s from Service s where s.Root=0 and s.ServiceId<>0 order by " + Options.SortService)).List<Service>();
            if ((uint) serviceList1.Count > 0U)
            {
              serviceList1.Insert(0, new Service()
              {
                ServiceId = (short) 0,
                ServiceName = "общая"
              });
              this.cmbService.DataSource = (object) serviceList1;
              this.cmbService.ValueMember = "ServiceId";
              this.cmbService.DisplayMember = "ServiceName";
              this.cmbService.SelectedIndex = 0;
              serviceList2.Insert(0, new Service((short) 0, "общая"));
              this.cmbArService.DataSource = (object) serviceList2;
              this.cmbArService.ValueMember = "ServiceId";
              this.cmbArService.DisplayMember = "ServiceName";
              this.cmbArService.SelectedIndex = 0;
              IList<BaseOrg> baseOrgList1 = (IList<BaseOrg>) new List<BaseOrg>();
              IList<BaseOrg> baseOrgList2 = (IList<BaseOrg>) new List<BaseOrg>();
              try
              {
                baseOrgList1 = this.session.CreateQuery(string.Format("select distinct new BaseOrg(b.Recipient.BaseOrgId,b.Recipient.NameOrgMin) from Supplier b where b.SupplierId in (select SupplierOrg.SupplierId from CmpSupplier) and b.Recipient.BaseOrgId<>0 order by b.Recipient.NameOrgMin")).List<BaseOrg>();
                baseOrgList2 = this.session.CreateQuery(string.Format("select distinct new BaseOrg(b.Recipient.BaseOrgId,b.Recipient.NameOrgMin) from Supplier b where b.SupplierId in (select SupplierOrg.SupplierId from CmpSupplier) and b.Recipient.BaseOrgId<>0 order by b.Recipient.NameOrgMin")).List<BaseOrg>();
              }
              catch
              {
              }
              this.city = this.currentLs == null ? Options.City : Convert.ToInt32(KvrplHelper.BaseValue(1, this.currentLs.Company));
              baseOrgList1.Insert(0, new BaseOrg(0, "Не определено"));
              this.cmbRecipient.ValueMember = "BaseOrgId";
              this.cmbRecipient.DisplayMember = "NameOrgMin";
              this.cmbRecipient.SelectedIndex = 0;
              this.cmbRecipient_SelectionChangeCommitted((object) null, (EventArgs) null);
              baseOrgList2.Insert(0, new BaseOrg(0, "Не определено"));
              this.cmbArRecipient.DataSource = (object) baseOrgList2;
              this.cmbArRecipient.ValueMember = "BaseOrgId";
              this.cmbArRecipient.DisplayMember = "NameOrgMin";
              this.cmbArRecipient.SelectedValue = (object) 0;
              this.cmbArRecipient_SelectionChangeCommitted((object) null, (EventArgs) null);
              this.cmbRecipientAuto.DataSource = (object) this.session.CreateQuery(string.Format("select new BaseOrg(BaseOrgId,NameOrgMin) from BaseOrg where BaseOrgId in (SELECT IDBASEORG FROM Postaver) order by NameOrgMin")).List<BaseOrg>();
              this.cmbRecipientAuto.ValueMember = "BaseOrgId";
              this.cmbRecipientAuto.DisplayMember = "NameOrgMin";
              this.cmbRecipientAuto.SelectedIndex = 0;
              this.cmbArRecipientAuto.DataSource = (object) this.session.CreateQuery(string.Format("select new BaseOrg(BaseOrgId,NameOrgMin) from BaseOrg where BaseOrgId in (SELECT IDBASEORG FROM Postaver) order by NameOrgMin")).List<BaseOrg>();
              this.cmbArRecipientAuto.ValueMember = "BaseOrgId";
              this.cmbArRecipientAuto.DisplayMember = "NameOrgMin";
              if (this.paymentId == 0)
              {
                this.cmbRecipientAuto.SelectedValue = (object) this.session.CreateQuery("from BaseOrg where BaseOrgId=:id").SetParameter<int>("id", 0).UniqueResult<BaseOrg>().BaseOrgId;
                this.cmbArRecipientAuto.SelectedValue = (object) this.session.CreateQuery("from BaseOrg where BaseOrgId=:id").SetParameter<int>("id", 0).UniqueResult<BaseOrg>().BaseOrgId;
              }
              else
              {
                Payment payment = this.session.Get<Payment>((object) this.paymentId);
                this.cmbRecipientAuto.SelectedValue = (object) this.session.CreateQuery("from BaseOrg where BaseOrgId=:id").SetParameter<BaseOrg>("id", payment.RecipientId).UniqueResult<BaseOrg>().BaseOrgId;
                this.cmbArRecipientAuto.SelectedValue = (object) this.session.CreateQuery("from BaseOrg where BaseOrgId=:id").SetParameter<BaseOrg>("id", payment.RecipientId).UniqueResult<BaseOrg>().BaseOrgId;
              }
              this.cmbReceipt.DataSource = (object) this.session.CreateCriteria(typeof (Receipt)).Add((ICriterion) Restrictions.Not((ICriterion) Restrictions.Eq("ReceiptId", (object) Convert.ToInt16(0)))).AddOrder(Order.Asc("ReceiptId")).List<Receipt>();
              this.cmbReceipt.ValueMember = "ReceiptId";
              this.cmbReceipt.DisplayMember = "ReceiptName";
              this.cmbReceipt.SelectedIndex = 0;
              this.cmbArReceipt.DataSource = (object) this.session.CreateCriteria(typeof (Receipt)).Add((ICriterion) Restrictions.Not((ICriterion) Restrictions.Eq("ReceiptId", (object) Convert.ToInt16(0)))).AddOrder(Order.Asc("ReceiptId")).List<Receipt>();
              this.cmbArReceipt.ValueMember = "ReceiptId";
              this.cmbArReceipt.DisplayMember = "ReceiptName";
              this.cmbArReceipt.SelectedIndex = 0;
              IList<BaseOrg> baseOrgList3 = this.session.CreateQuery("select new BaseOrg(b.BaseOrgId,b.NameOrgMin) from BaseOrg b where b.BaseOrgId in (select BaseOrg.BaseOrgId from LsArenda) or b.BaseOrgId=0 order by b.NameOrgMin").List<BaseOrg>();
              this.cmbO.DisplayMember = "NameOrgMin";
              this.cmbO.ValueMember = "BaseOrgId";
              this.cmbO.DataSource = (object) baseOrgList3;
              this.cmbO.SelectedValue = (object) 0;
              IList<LsArenda> lsArendaList = this.session.CreateQuery("select new LsArenda(b.LsClient,b.DogovorNum) from LsArenda b order by b.DogovorNum").List<LsArenda>();
              lsArendaList.Insert(0, new LsArenda(new LsClient(0, (Flat) null), ""));
              this.cmbDog.DisplayMember = "DogovorNum";
              this.cmbDog.ValueMember = "Status";
              this.cmbDog.DataSource = (object) lsArendaList;
              this.cmbDog.SelectedValue = (object) 0;
              if (this.currentLs != null && this.currentLs.Complex.IdFk == Options.ComplexArenda.IdFk)
                this.rbAr.Checked = true;
              if (!Options.CountersInPays)
                this.gbCounters.Visible = false;
              else
                this.gbCounters.Visible = true;
              if (this.paymentId == 0)
              {
                DateTime? nullable = Options.PeriodPay;
                if (!nullable.HasValue)
                {
                  MonthPicker mpPeriod = this.mpPeriod;
                  nullable = Options.Period.PeriodName;
                  DateTime dateTime = nullable.Value;
                  mpPeriod.Value = dateTime;
                }
                else
                {
                  MonthPicker mpPeriod = this.mpPeriod;
                  nullable = Options.PeriodPay;
                  DateTime dateTime = nullable.Value;
                  mpPeriod.Value = dateTime;
                }
                nullable = Options.MonthPay;
                if (!nullable.HasValue)
                {
                  MonthPicker mpPeriodPay = this.mpPeriodPay;
                  nullable = Options.Period.PeriodName;
                  DateTime dateTime = nullable.Value.AddMonths(-1);
                  mpPeriodPay.Value = dateTime;
                }
                else
                {
                  MonthPicker mpPeriodPay = this.mpPeriodPay;
                  nullable = Options.MonthPay;
                  DateTime dateTime = nullable.Value;
                  mpPeriodPay.Value = dateTime;
                }
                nullable = Options.DatePay;
                if (nullable.HasValue)
                {
                  DateTimePicker dtmpDate = this.dtmpDate;
                  nullable = Options.DatePay;
                  DateTime dateTime = nullable.Value;
                  dtmpDate.Value = dateTime;
                }
                if (this.currentLs != null)
                {
                  this.txtClientId.Text = this.currentLs.ClientId.ToString();
                  this.txbArClient.Text = this.currentLs.ClientId.ToString();
                  this.cmbDog.SelectedValue = (object) this.currentLs.ClientId;
                }
                if (Options.SourcePay != null)
                  this.cmbSource.SelectedValue = (object) Options.SourcePay.SourcePayId;
                if (Options.PurposePay != null)
                  this.cmbPurpose.SelectedValue = (object) Options.PurposePay.PurposePayId;
                if (Options.PayDoc != null)
                  this.cmbPayDoc.SelectedValue = (object) Options.PayDoc.PayDocId;
                if (Options.Scanner)
                  this.chbScanner.Checked = true;
                if (Options.Packet == null && this.rbKv.Checked)
                  this.UpdatePacket();
                else if (this.rbKv.Checked)
                  this.txbPacket.Text = Options.Packet;
                int num = Options.ArrangeRent ? 1 : 0;
                if (true)
                  this.rbRent.Checked = Options.ArrangeRent;
              }
              else
              {
                this.paymentList = this.session.CreateCriteria(typeof (Payment)).Add((ICriterion) Restrictions.Eq("PaymentId", (object) this.paymentId)).List();
                this.currentPay = (Payment) this.paymentList[0];
                this.currentLs = this.currentPay.LsClient;
                if (this.currentPay != null)
                {
                  this.gbVid.Visible = false;
                  if (this.currentPay.LsClient != null && this.currentPay.LsClient.Complex.IdFk == Options.ComplexArenda.IdFk)
                    this.rbAr.Checked = true;
                }
                try
                {
                  this.cmbSource.SelectedValue = (object) this.currentPay.SPay.SourcePayId;
                  this.cmbPurpose.SelectedValue = (object) this.currentPay.PPay.PurposePayId;
                  this.cmbPayDoc.SelectedValue = (object) this.currentPay.PayDoc.PayDocId;
                  this.cmbRecipientAuto.SelectedValue = (object) this.currentPay.RecipientId.BaseOrgId;
                  if ((uint) this.cmbReceipt.SelectedIndex > 0U)
                    this.cmbService.Enabled = true;
                  this.txbPacket.DataBindings.Add(new Binding("Text", (object) this.paymentList, "PacketNum"));
                  this.txbNumber.DataBindings.Add(new Binding("Text", (object) this.paymentList, "PacketNum"));
                  this.dtmpDate.DataBindings.Add(new Binding("Value", (object) this.paymentList, "PaymentDate"));
                  this.mpPeriodPay.Value = this.currentPay.PeriodPay.PeriodName.Value;
                  this.mpPeriod.Value = this.currentPay.Period.PeriodName.Value;
                  if (this.rbKv.Checked)
                  {
                    this.cmbRecipient_SelectionChangeCommitted((object) null, (EventArgs) null);
                    this.txtClientId.DataBindings.Add(new Binding("Text", (object) this.paymentList, "ClientId"));
                    this.txtSum.DataBindings.Add(new Binding("Text", (object) this.paymentList, "PaymentValue"));
                    this.txtPeni.DataBindings.Add(new Binding("Text", (object) this.paymentList, "PaymentPeni"));
                    this.cmbService.SelectedValue = (object) this.currentPay.Service.ServiceId;
                    this.cmbReceipt.SelectedValue = (object) this.currentPay.Receipt.ReceiptId;
                    this.cmbRecipient.SelectedValue = (object) this.currentPay.Supplier.Recipient.BaseOrgId;
                    this.cmbPerfomer.SelectedValue = (object) this.currentPay.Supplier.Perfomer.BaseOrgId;
                  }
                  if (this.rbAr.Checked)
                  {
                    this.cmbDog.SelectedValue = (object) this.currentPay.LsClient.ClientId;
                    this.txbArClient.Text = this.currentPay.LsClient.ClientId.ToString();
                    this.txbArValue.DataBindings.Add(new Binding("Text", (object) this.paymentList, "PaymentValue"));
                    this.txbArPeni.DataBindings.Add(new Binding("Text", (object) this.paymentList, "PaymentPeni"));
                    this.cmbArService.SelectedValue = (object) this.currentPay.Service.ServiceId;
                    this.cmbArReceipt.SelectedValue = (object) this.currentPay.Receipt.ReceiptId;
                    this.cmbArRecipient.SelectedValue = (object) this.currentPay.Supplier.Recipient.BaseOrgId;
                    this.cmbArPerfomer.SelectedValue = (object) this.currentPay.Supplier.Perfomer.BaseOrgId;
                    this.cmbO.Enabled = false;
                    this.cmbDog.Enabled = false;
                    this.txbArClient.Enabled = false;
                    this.AddAddressFIO();
                  }
                }
                catch (Exception ex)
                {
                  KvrplHelper.WriteLog(ex, (LsClient) null);
                }
                this.txtClientId.ReadOnly = true;
                if ((int) this.currentPay.PPay.PurposePayId == 9)
                {
                  if (KvrplHelper.CheckProxy(85, 2, Options.Company, false))
                  {
                    this.pnMain.Enabled = true;
                    this.pnEvidence.Enabled = true;
                    this.gbAttribute.Enabled = true;
                  }
                  else
                  {
                    this.pnMain.Enabled = false;
                    this.pnEvidence.Enabled = false;
                    this.gbAttribute.Enabled = false;
                  }
                }
                if ((int) this.currentPay.PPay.PurposePayId == 31 || (int) this.currentPay.PPay.PurposePayId == 39)
                {
                  this.pnMain.Enabled = false;
                  this.pnEvidence.Enabled = false;
                  this.gbAttribute.Enabled = false;
                }
              }
              if (this.city == 35)
              {
                this.cmbReceipt.SelectedValue = (object) this.session.Get<Receipt>((object) Convert.ToInt16(50)).ReceiptId;
                this.cmbReceipt.Enabled = false;
                this.cmbRecipient.SelectedValue = (object) this.session.Get<BaseOrg>((object) Convert.ToInt32(-39999859)).BaseOrgId;
                this.cmbRecipient_SelectionChangeCommitted((object) null, (EventArgs) null);
                this.cmbPerfomer.SelectedValue = (object) this.session.Get<BaseOrg>((object) Convert.ToInt32(-39999859)).BaseOrgId;
                this.cmbArReceipt.SelectedValue = (object) this.session.Get<Receipt>((object) Convert.ToInt16(50)).ReceiptId;
                this.cmbArReceipt.Enabled = false;
                this.cmbArRecipient.SelectedValue = (object) this.session.Get<BaseOrg>((object) Convert.ToInt32(-39999859)).BaseOrgId;
                this.cmbArRecipient_SelectionChangeCommitted((object) null, (EventArgs) null);
                this.cmbArPerfomer.SelectedValue = (object) this.session.Get<BaseOrg>((object) Convert.ToInt32(-39999859)).BaseOrgId;
              }
              if (this.city != 4)
              {
                this.txtStrah.Visible = false;
                this.lblStrah.Visible = false;
              }
              if (!Options.Scanner)
              {
                this.txtStrah.Visible = false;
                this.lblStrah.Visible = false;
              }
              this.changeText = true;
              this.changeDog = true;
              this.session.Clear();
              this.scanNewCode = false;
            }
            else
            {
              int num = (int) MessageBox.Show("Внесение записей невозможно. Заполните словарь услуг", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              this.Close();
            }
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

    private void FrmAttribute_FormClosed(object sender, FormClosedEventArgs e)
    {
      Options.PeriodPay = new DateTime?(this.mpPeriod.Value);
      Options.MonthPay = new DateTime?(this.mpPeriodPay.Value);
      Options.DatePay = new DateTime?(this.dtmpDate.Value);
      Options.SourcePay = (SourcePay) this.cmbSource.SelectedItem;
      Options.PurposePay = (PurposePay) this.cmbPurpose.SelectedItem;
      Options.PayDoc = (PayDoc) this.cmbPayDoc.SelectedItem;
      if (this.rbKv.Checked)
        Options.Packet = this.txbPacket.Text;
      Options.ArrangeRent = this.rbRent.Checked;
      this.tmr1.Stop();
      if (Options.Scanner && this.txtClientId.Text != "" && MessageBox.Show("Вы хотите сохранить текущий платеж?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        this.btnSave_Click((object) null, (EventArgs) null);
      if (this.frmCurrentPayment == null || !this.frmCurrentPayment.IsOpen)
        return;
      this.frmCurrentPayment.Close();
      this.frmCurrentPayment.Dispose();
    }

    private void FrmAttribute_Shown(object sender, EventArgs e)
    {
      if (this.rbKv.Checked)
        this.txtClientId.Focus();
      else
        this.cmbO.Focus();
    }

    private void FrmAttribute_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Multiply)
        this.btnSave_Click(sender, (EventArgs) e);
      if (e.KeyValue == 48 && e.KeyCode == Keys.D0 && (Options.Scanner && this.strInputScanner == "") && !this.txtClientId.Focused)
        this.scanNewCode = true;
      else
        this.scanNewCode = false;
    }

    private void FrmAttribute_KeyPress(object sender, KeyPressEventArgs e)
    {
      if ((int) e.KeyChar == 77 && this.city == 6 && Options.Scanner && this.strInputScanner == "" && !this.txtClientId.Focused)
      {
        this.txtClientId.Focus();
        this.txtClientId.Text = "M";
        this.txtClientId.SelectionStart = 1;
      }
      if (!this.scanNewCode || !Options.Scanner || !(this.strInputScanner == "") || this.txtClientId.Focused)
        return;
      this.txtClientId.Focus();
      this.txtClientId.Text = "0";
      this.txtClientId.SelectionStart = 1;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      this.EndChangedScanner = false;
      this.session.Clear();
      this.session = Domain.CurrentSession;
      if (this.rbAr.Checked)
      {
        this.SaveArenda();
      }
      else
      {
        try
        {
          this.currentLs = KvrplHelper.FindLs(Convert.ToInt32(this.txtClientId.Text));
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
          this.currentLs = (LsClient) null;
        }
        if (this.currentLs == null)
        {
          try
          {
            this.currentLs = KvrplHelper.FindLsByOldId(Convert.ToInt32(this.txtClientId.Text));
          }
          catch
          {
            this.currentLs = (LsClient) null;
          }
        }
        if (this.currentLs == null)
        {
          int num1 = (int) MessageBox.Show("Введенный лицевой не существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          string str = "";
          if ((uint) this._serviceId > 0U)
            str = string.Format(" and (s.Service.ServiceId={0} or s.Service.ServiceId=(select Root from Service where ServiceId={0}))", (object) this._serviceId);
          try
          {
            if ((uint) ((BaseOrg) this.cmbRecipient.SelectedItem).BaseOrgId > 0U)
            {
              if (this.session.CreateQuery(string.Format("Select distinct d.Recipient from CmpSupplier s,Supplier d where s.SupplierOrg.SupplierId=d.SupplierId and d.Recipient.BaseOrgId=" + (object) ((BaseOrg) this.cmbRecipient.SelectedItem).BaseOrgId + " and s.Company.CompanyId=isnull((select ParamValue from CompanyParam where Period.PeriodId=0 and Company.CompanyId={0} and DBeg<='{2}' and DEnd>='{2}' and Param.ParamId=211),0) " + str + " order by d.Recipient.NameOrgMin", (object) this.currentLs.Company.CompanyId, (object) this._serviceId, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.GetCmpKvrClose(this.currentLs.Company, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk).PeriodName.Value.AddMonths(1)))).UniqueResult<BaseOrg>() == null)
              {
                int num2 = (int) MessageBox.Show("Данный получатель не относится к этому лицевому!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
              }
            }
            if ((uint) ((BaseOrg) this.cmbPerfomer.SelectedItem).BaseOrgId > 0U)
            {
              if (this.session.CreateQuery(string.Format("Select distinct d.Perfomer from CmpSupplier s,Supplier d where s.SupplierOrg.SupplierId=d.SupplierId  and d.Perfomer.BaseOrgId=" + (object) ((BaseOrg) this.cmbPerfomer.SelectedItem).BaseOrgId + " and s.Company.CompanyId=isnull((select ParamValue from CompanyParam where Period.PeriodId=0 and Company.CompanyId={0} and DBeg<='{2}' and DEnd>='{2}'and Param.ParamId=211),0) " + str + " order by d.Perfomer.NameOrgMin", (object) this.currentLs.Company.CompanyId, (object) this._serviceId, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.GetCmpKvrClose(this.currentLs.Company, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk).PeriodName.Value.AddMonths(1)))).UniqueResult<BaseOrg>() == null)
              {
                int num2 = (int) MessageBox.Show("Данный исполнитель не относится к этому лицевому!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
              }
            }
          }
          catch (Exception ex)
          {
            int num2 = (int) MessageBox.Show("Неизвестная ошибка, проверьте данные!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
          if (this.txtClientId.Text == "")
          {
            int num3 = (int) MessageBox.Show("Лицевой счет не введен", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else
          {
            if (this.txtSum.Text == "" || this.txtPeni.Text == "" || this.txtSum.Text == "0" || this.txtPeni.Text == "0")
            {
              if (this.txtSum.Text == "" && this.txtPeni.Text == "" || this.txtSum.Text == "0" && this.txtPeni.Text == "0")
              {
                int num2 = (int) MessageBox.Show("Сумма не введена", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
              }
              if (this.txtSum.Text == "")
                this.txtSum.Text = "0";
              if (this.txtPeni.Text == "")
                this.txtPeni.Text = "0";
            }
            if (this.txbPacket.Text == "")
            {
              int num4 = (int) MessageBox.Show("Пачка не введена", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
              IList list1 = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) this.mpPeriod.Value)).List();
              IList list2 = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) this.mpPeriodPay.Value)).List();
              if (list1.Count == 0 || list2.Count == 0)
              {
                int num2 = (int) MessageBox.Show("Введен некорректный период", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              }
              else
              {
                Period period1 = (Period) list1[0];
                Period period2 = (Period) list2[0];
                if ((Convert.ToInt32(KvrplHelper.BaseValue(1, this.currentLs.Company)) == 35 || Convert.ToInt32(this.cmbReceipt.SelectedValue) == 50) && (Convert.ToInt32(this.cmbRecipient.SelectedValue) == 0 && Convert.ToInt32(this.cmbPerfomer.SelectedValue) == 0))
                {
                  int num5 = (int) MessageBox.Show("Выберите поставщика и/или исполнителя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                  if (!KvrplHelper.CheckProxy(41, 2, this.currentLs.Company, true))
                    return;
                  DateTime? periodName1 = period1.PeriodName;
                  DateTime? periodName2 = KvrplHelper.GetKvrClose(this.currentLs.ClientId, Options.Complex, Options.ComplexPrior).PeriodName;
                  if (periodName1.HasValue & periodName2.HasValue && periodName1.GetValueOrDefault() <= periodName2.GetValueOrDefault())
                  {
                    int num6 = (int) MessageBox.Show("Невозможно внести платеж в закрытый период!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  }
                  else
                  {
                    periodName2 = period1.PeriodName;
                    DateTime dateTime1 = periodName2.Value;
                    DateTime now = DateTime.Now;
                    DateTime dateTime2 = now.AddYears(-3);
                    if (dateTime1 <= dateTime2)
                    {
                      int num7 = (int) MessageBox.Show("Некорректный период", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    else
                    {
                      try
                      {
                        Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.txtSum.Text));
                        Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.txtPeni.Text));
                      }
                      catch (Exception ex)
                      {
                        int num8 = (int) MessageBox.Show("Введенные суммы некорректны", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                      }
                      Payment payment = new Payment();
                      Payment currentPay1;
                      if (this.currentPay == null)
                      {
                        this.currentPay = new Payment();
                        try
                        {
                          this.currentPay.PaymentId = this.session.CreateSQLQuery("select DBA.gen_id('lsPayment',1)").List<int>()[0];
                          this.currentPay.LsClient = this.currentLs;
                          this.currentPay.Period = period1;
                          this.currentPay.PeriodPay = period2;
                          this.currentPay.Service = (Service) this.cmbService.SelectedItem;
                          this.currentPay.Receipt = this.session.Get<Receipt>(this.cmbReceipt.SelectedValue);
                          this.currentPay.SPay = (SourcePay) this.cmbSource.SelectedItem;
                          this.currentPay.PPay = (PurposePay) this.cmbPurpose.SelectedItem;
                          this.currentPay.PayDoc = (PayDoc) this.cmbPayDoc.SelectedItem;
                          this.currentPay.PacketNum = this.txbPacket.Text;
                          this.currentPay.PaymentValue = Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.txtSum.Text));
                          this.currentPay.PaymentPeni = Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.txtPeni.Text));
                          this.currentPay.PaymentDate = this.dtmpDate.Value;
                          this.currentPay.Supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(this.cmbRecipient.SelectedValue), (object) Convert.ToInt32(this.cmbPerfomer.SelectedValue))).List<Supplier>()[0];
                          if (this.currentPay.Supplier.Recipient.BaseOrgId == 0 && this.currentPay.Supplier.Perfomer.BaseOrgId == 0)
                            this.currentPay.Supplier = this.session.Get<Supplier>((object) 0);
                          this.currentPay.UName = Options.Login;
                          Payment currentPay2 = this.currentPay;
                          now = DateTime.Now;
                          DateTime date = now.Date;
                          currentPay2.DEdit = date;
                        }
                        catch (Exception ex)
                        {
                          this.log.Error("Возникла ошибка к классе FrmAttribute.cs ошибка ввода данных --> " + ex.Message);
                          int num8 = (int) MessageBox.Show("Ошибка ввода данных! Проверьте данныe.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }
                        int val = 0;
                        if ((BaseOrg) this.cmbRecipientAuto.SelectedItem != null)
                          val = ((BaseOrg) this.cmbRecipientAuto.SelectedItem).BaseOrgId;
                        this.currentPay.RecipientId = this.session.CreateQuery("from BaseOrg where BaseOrgId=:id").SetParameter<int>("id", val).UniqueResult<BaseOrg>();
                        if (this.session.CreateQuery("from Payment where Period.PeriodId=:per and LsClient.ClientId=:client and PeriodPay.PeriodId=:pper and SPay.SourcePayId=:spay and PPay.PurposePayId=:ppay and Service.ServiceId=:serv and Receipt.ReceiptId=:receipt and PaymentDate=:data and PacketNum = :pack and Supplier.SupplierId=:supl and PayDoc.PayDocId=:doc and PaymentValue=:value and PaymentPeni=:peni").SetParameter<int>("per", this.currentPay.Period.PeriodId).SetParameter<int>("client", this.currentPay.LsClient.ClientId).SetParameter<int>("pper", this.currentPay.PeriodPay.PeriodId).SetParameter<short>("spay", this.currentPay.SPay.SourcePayId).SetParameter<short>("ppay", this.currentPay.PPay.PurposePayId).SetParameter<short>("serv", this.currentPay.Service.ServiceId).SetParameter<short>("receipt", this.currentPay.Receipt.ReceiptId).SetParameter<string>("pack", this.currentPay.PacketNum).SetParameter<int>("supl", this.currentPay.Supplier.SupplierId).SetParameter<short>("doc", this.currentPay.PayDoc.PayDocId).SetDecimal("value", this.currentPay.PaymentValue).SetDecimal("peni", this.currentPay.PaymentPeni).SetDateTime("data", this.currentPay.PaymentDate.Date).List<Payment>().Count > 0 && MessageBox.Show("Такой платеж уже существует. Продолжить сохранение?", "Внимание", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        {
                          this.txtClientId.Focus();
                          this.currentPay = (Payment) null;
                          return;
                        }
                        this.session.Save((object) this.currentPay);
                        currentPay1 = this.currentPay;
                        this.currentPay = (Payment) null;
                        if (this.StrahPayment != null && this.txtStrah.Text != "")
                        {
                          Decimal num8 = new Decimal();
                          try
                          {
                            num8 = Convert.ToDecimal(this.txtStrah.Text);
                          }
                          catch
                          {
                          }
                          this.StrahPayment.PaymentValue = num8;
                          this.StrahPayment.Supplier = this.session.Get<Supplier>((object) 0);
                          if (num8 > Decimal.Zero)
                          {
                            this.session.Save((object) this.StrahPayment);
                            this.Pays.Add(this.StrahPayment);
                          }
                        }
                        this.StrahPayment = (Payment) null;
                        if (sender != null)
                        {
                          this.txtClientId.Clear();
                          this.txtSum.Clear();
                          this.txtPeni.Clear();
                          this.tbItog.Clear();
                          this.lastStrClientId = "";
                          if (this.insertId)
                            this.txtClientId.Text = this.currentLs.ClientId.ToString();
                        }
                        this.txtClientId.Focus();
                      }
                      else
                      {
                        this.currentPay.Period = period1;
                        this.currentPay.PeriodPay = period2;
                        this.currentPay.UName = Options.Login;
                        Payment currentPay2 = this.currentPay;
                        now = DateTime.Now;
                        DateTime date = now.Date;
                        currentPay2.DEdit = date;
                        int val = 0;
                        if ((BaseOrg) this.cmbRecipientAuto.SelectedItem != null)
                          val = ((BaseOrg) this.cmbRecipientAuto.SelectedItem).BaseOrgId;
                        this.currentPay.RecipientId = this.session.CreateQuery("from BaseOrg where BaseOrgId=:id").SetParameter<int>("id", val).UniqueResult<BaseOrg>();
                        this.currentPay.Supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(this.cmbRecipient.SelectedValue), (object) Convert.ToInt32(this.cmbPerfomer.SelectedValue))).List<Supplier>()[0];
                        if (this.currentPay.Supplier.Recipient.BaseOrgId == 0 && this.currentPay.Supplier.Perfomer.BaseOrgId == 0)
                          this.currentPay.Supplier = this.session.Get<Supplier>((object) 0);
                        this.session.Update((object) this.currentPay);
                        this.Pays.Clear();
                        currentPay1 = this.currentPay;
                      }
                      try
                      {
                        this.session.Flush();
                        this.Pays.Add(currentPay1);
                      }
                      catch (Exception ex)
                      {
                        if (this.currentPay == null && ex.InnerException != null && ex.InnerException.Message.ToLower().IndexOf("primary key for table 'lspayment' is not unique") != -1)
                        {
                          KvrplHelper.ResetGeners("lsPayment", "Payment_id");
                          int num8 = (int) MessageBox.Show("Была устранена ошибка генерации уникального поля! Введите текущий платеж заново!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        else
                        {
                          int num9 = (int) MessageBox.Show("Не удалось сохранить изменения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }
                        KvrplHelper.WriteLog(ex, (LsClient) null);
                      }
                      if (this.dgvEvidence.Visible && this.dgvEvidence.Rows.Count > 0)
                      {
                        if (!this.saveNullVolume && MessageBox.Show("Сохранить счетчики с нулевым расходом?", "Внимание!", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                          this.saveNullVolume = true;
                        foreach (DataGridViewRow row in (IEnumerable) this.dgvEvidence.Rows)
                        {
                          this.session = Domain.CurrentSession;
                          this.dgvEvidence.CurrentCell = row.Cells[0];
                          Evidence dataBoundItem = (Evidence) row.DataBoundItem;
                          if (this.dgvEvidence.CurrentRow.Cells["DBeg"].Value != null)
                          {
                            dataBoundItem.DBeg = Convert.ToDateTime(this.dgvEvidence.CurrentRow.Cells["DBeg"].Value);
                            if (this.dgvEvidence.CurrentRow.Cells["DEnd"].Value != null)
                            {
                              dataBoundItem.DEnd = Convert.ToDateTime(this.dgvEvidence.CurrentRow.Cells["DEnd"].Value);
                              if (this.dgvEvidence.CurrentRow.Cells["Past"].Value != null && this.dgvEvidence.CurrentRow.Cells["Current"].Value != null)
                              {
                                try
                                {
                                  dataBoundItem.Past = Convert.ToDouble(this.dgvEvidence.CurrentRow.Cells["Past"].Value);
                                  dataBoundItem.Current = Convert.ToDouble(this.dgvEvidence.CurrentRow.Cells["Current"].Value);
                                  if (dataBoundItem.Current < dataBoundItem.Past)
                                  {
                                    if (this.city == 3)
                                    {
                                      int num8 = (int) MessageBox.Show("Невозможно ввести отрицательный расход", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                      break;
                                    }
                                    if (MessageBox.Show("Настоящие показания меньше предыдущих. Продолжить?", "Внимание!!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand) == DialogResult.Cancel || !KvrplHelper.IsGoodEvidence(dataBoundItem, this.session))
                                      break;
                                  }
                                  if (dataBoundItem.Current - 500.0 > dataBoundItem.Past)
                                  {
                                    if (MessageBox.Show("Слишком большой расход. Продолжить?", "Внимание!!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand) == DialogResult.Cancel)
                                      break;
                                  }
                                }
                                catch (Exception ex)
                                {
                                  int num8 = (int) MessageBox.Show("Показания введены некорректно", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                  KvrplHelper.WriteLog(ex, (LsClient) null);
                                  break;
                                }
                                if (dataBoundItem.DEnd < dataBoundItem.DBeg)
                                {
                                  int num8 = (int) MessageBox.Show("Дата настоящего меньше даты предыдущего", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                  break;
                                }
                                Counter counter = this.session.Get<Counter>((object) dataBoundItem.Counter.CounterId);
                                if (counter.TypeCounter != null)
                                {
                                  double num8 = Math.Pow(10.0, (double) counter.TypeCounter.CDigit);
                                  if (dataBoundItem.Current >= num8 || dataBoundItem.Past >= num8)
                                  {
                                    int num9 = (int) MessageBox.Show("Показания не соответствуют разрядности счетчика", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                    break;
                                  }
                                }
                                Period period3 = this.session.CreateQuery(string.Format("from Period where PeriodName='{0}'", (object) KvrplHelper.DateToBaseFormat(this.mpPeriod.Value))).List<Period>()[0];
                                dataBoundItem.UName = Options.Login;
                                dataBoundItem.DEdit = DateTime.Now.Date;
                                try
                                {
                                  if (Convert.ToDecimal(dataBoundItem.Current) - Convert.ToDecimal(dataBoundItem.Past) != Decimal.Zero || this.saveNullVolume && Convert.ToDecimal(dataBoundItem.Current) - Convert.ToDecimal(dataBoundItem.Past) == Decimal.Zero)
                                  {
                                    if (dataBoundItem.Period == null)
                                    {
                                      dataBoundItem.Period = period3;
                                      this.session.Save((object) dataBoundItem);
                                    }
                                    else
                                    {
                                      dataBoundItem.Period = period3;
                                      this.session.Update((object) dataBoundItem);
                                    }
                                  }
                                  this.session.Flush();
                                }
                                catch (Exception ex)
                                {
                                  KvrplHelper.WriteLog(ex, (LsClient) null);
                                }
                                this.session.Clear();
                              }
                              else
                              {
                                int num8 = (int) MessageBox.Show("Показания не введены", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                break;
                              }
                            }
                            else
                            {
                              int num8 = (int) MessageBox.Show("Дата настоящих показаний не введена", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                              break;
                            }
                          }
                          else
                          {
                            int num8 = (int) MessageBox.Show("Дата предыдущих показаний не введена", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            break;
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
    }

    private void btnFind_Click(object sender, EventArgs e)
    {
      FrmBigSearch frmBigSearch = new FrmBigSearch();
      int num = (int) frmBigSearch.ShowDialog();
      if (frmBigSearch.client == null)
        return;
      this.currentLs = frmBigSearch.client;
      frmBigSearch.Dispose();
      this.txtClientId.Text = this.currentLs.ClientId.ToString();
      this.txtClientId_KeyDown(sender, new KeyEventArgs(Keys.Return));
    }

    private void btnLoadCounters_Click(object sender, EventArgs e)
    {
      if (this.currentLs == null || !KvrplHelper.CheckProxy(42, 2, this.currentLs.Company, true))
        return;
      DateTime? periodName = KvrplHelper.GetCmpKvrClose(this.currentLs.Company, Options.ComplexPasp.ComplexId, Options.ComplexPrior.IdFk).PeriodName;
      DateTime dateTime1 = periodName.Value;
      periodName = Options.Period.PeriodName;
      DateTime dateTime2 = periodName.Value;
      if (dateTime1 >= dateTime2)
      {
        int num = (int) MessageBox.Show("Невозможно перенести счетчики в закрытый период", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (MessageBox.Show("Внести показания из предыдущего периода", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
          return;
        if (this.dgvEvidence.Rows.Count > 0)
        {
          if (MessageBox.Show("В текущем периоде обнаружены показания счетчиков. Все равно внести показания?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            return;
          foreach (DataGridViewRow row in (IEnumerable) this.dgvEvidence.Rows)
          {
            this.session = Domain.CurrentSession;
            Evidence evidence = new Evidence();
            Evidence dataBoundItem = (Evidence) row.DataBoundItem;
            try
            {
              this.session.Delete((object) dataBoundItem);
              this.session.Flush();
            }
            catch (Exception ex)
            {
              KvrplHelper.WriteLog(ex, (LsClient) null);
            }
            this.session.Clear();
          }
        }
        this.session = Domain.CurrentSession;
        foreach (Counter counter in (IEnumerable<Counter>) this.counterList)
        {
          IList<Evidence> evidenceList = this.session.CreateQuery(string.Format("select e from Evidence e where e.Counter.CounterId={0} and e.Period.PeriodId=(select max(Period.PeriodId) from Evidence where Counter=e.Counter)", (object) counter.CounterId)).List<Evidence>();
          Evidence evidence = new Evidence();
          evidence.Counter = counter;
          evidence.Period = Options.Period;
          if (evidenceList.Count > 0)
          {
            evidence.Past = evidenceList[0].Current;
            evidence.DBeg = evidenceList[0].DEnd.AddDays(1.0);
            evidence.Current = evidenceList[0].Current;
          }
          else
          {
            evidence.Past = 0.0;
            evidence.DBeg = DateTime.Now;
            evidence.Current = 0.0;
          }
          evidence.DEnd = DateTime.Now;
          evidence.UName = Options.Login;
          evidence.DEdit = DateTime.Now;
          try
          {
            this.session.Save((object) evidence);
            this.session.Flush();
          }
          catch (Exception ex)
          {
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
        }
        this.session.Clear();
        this.LoadCounters();
      }
    }

    private void cmbService_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (this.currentPay == null)
        return;
      this.currentPay.Service = (Service) this.cmbService.SelectedItem;
    }

    private void cmbSource_SelectionChangeCommitted_1(object sender, EventArgs e)
    {
      if (this.currentPay != null)
        this.currentPay.SPay = (SourcePay) this.cmbSource.SelectedItem;
      if (!this.rbKv.Checked)
        return;
      this.UpdatePacket();
    }

    private void cmbPurpose_SelectionChangeCommitted_1(object sender, EventArgs e)
    {
      if (this.currentPay == null)
        return;
      this.currentPay.PPay = (PurposePay) this.cmbPurpose.SelectedItem;
    }

    private void cmbSupplier_SelectionChangeCommitted(object sender, EventArgs e)
    {
    }

    private void cmbReceipt_SelectionChangeCommitted(object sender, EventArgs e)
    {
      try
      {
        if (Convert.ToInt32(this.cmbReceipt.SelectedValue) == 0)
        {
          this.cmbService.SelectedIndex = 0;
          this.cmbService.Enabled = false;
        }
        else
        {
          try
          {
            this.currentLs = KvrplHelper.FindLs(Convert.ToInt32(this.txtClientId.Text));
            if (!this.chbScanner.Checked)
              this.FindSum();
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Лицевой счет не введен или введен неверно.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return;
          }
          IList<Service> serviceList = this.session.CreateQuery(string.Format("select DISTINCT s from Service s, CmpHmReceipt sp where sp.Service.ServiceId=s.ServiceId and s.Root=0 and sp.Company.CompanyId={0} and sp.Receipt.ReceiptId={1} and sp.Complex.IdFk={2} order by " + Options.SortService, (object) this.currentLs.Company.CompanyId, (object) Convert.ToInt32(this.cmbReceipt.SelectedValue), (object) Options.Complex.IdFk)).List<Service>();
          if (serviceList.Count == 0)
            serviceList = this.session.CreateQuery(string.Format("select s from Service s, ServiceParam sp where sp.Service_id=s.ServiceId and s.Root=0 and sp.Company_id={0} and sp.Receipt_id={1} and sp.Complex.IdFk={2} order by " + Options.SortService, (object) this.currentLs.Company.CompanyId, (object) Convert.ToInt32(this.cmbReceipt.SelectedValue), (object) Options.Complex.IdFk)).List<Service>();
          if ((uint) serviceList.Count > 0U)
          {
            serviceList.Insert(0, new Service()
            {
              ServiceId = (short) 0,
              ServiceName = "общая"
            });
            this.cmbService.DataSource = (object) serviceList;
            this.cmbService.ValueMember = "ServiceId";
            this.cmbService.DisplayMember = "ServiceName";
            this.cmbService.SelectedIndex = 0;
            this.cmbService.Enabled = true;
          }
          else
          {
            this.cmbService.SelectedIndex = 0;
            this.cmbService.Enabled = false;
          }
        }
        if (this.currentPay == null)
          return;
        this.currentPay.Receipt = (Receipt) this.cmbReceipt.SelectedItem;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void cmbArService_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (this.currentPay == null)
        return;
      this.currentPay.Service = (Service) this.cmbArService.SelectedItem;
    }

    private void cmbArSupplier_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (this.currentPay == null)
        return;
      this.currentPay.Supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(this.cmbArRecipient.SelectedValue), (object) Convert.ToInt32(this.cmbArPerfomer.SelectedValue))).List<Supplier>()[0];
    }

    private void cmbRecipient_SelectionChangeCommitted(object sender, EventArgs e)
    {
      IList<BaseOrg> baseOrgList = (IList<BaseOrg>) new List<BaseOrg>();
      if ((uint) Convert.ToInt32(this.cmbRecipient.SelectedValue) > 0U)
      {
        this.cmbPerfomer.DataSource = (object) this.session.CreateQuery(string.Format("select distinct new BaseOrg(b.Perfomer.BaseOrgId,b.Perfomer.NameOrgMin) from Supplier b where b.SupplierId in (select SupplierOrg.SupplierId from CmpSupplier) and b.Recipient.BaseOrgId={0} and b.Perfomer.BaseOrgId<>0 order by b.Perfomer.NameOrgMin", (object) Convert.ToInt32(this.cmbRecipient.SelectedValue))).List<BaseOrg>();
        this.cmbPerfomer.DisplayMember = "NameOrgMin";
        this.cmbPerfomer.ValueMember = "BaseOrgId";
      }
      else
        this.loadPerfomerCB();
    }

    private void cmbArRecipient_SelectionChangeCommitted(object sender, EventArgs e)
    {
      IList<BaseOrg> baseOrgList = this.session.CreateQuery(string.Format("select distinct new BaseOrg(b.Perfomer.BaseOrgId,b.Perfomer.NameOrgMin) from Supplier b where b.SupplierId in (select SupplierOrg.SupplierId from CmpSupplier) and b.Recipient.BaseOrgId={0} and b.Perfomer.BaseOrgId<>0 order by b.Perfomer.NameOrgMin", (object) Convert.ToInt32(this.cmbArRecipient.SelectedValue))).List<BaseOrg>();
      if (Convert.ToInt32(this.cmbArRecipient.SelectedValue) == 0)
        baseOrgList.Insert(0, new BaseOrg(0, "Не определено"));
      this.cmbArPerfomer.DataSource = (object) baseOrgList;
      this.cmbArPerfomer.DisplayMember = "NameOrgMin";
      this.cmbArPerfomer.ValueMember = "BaseOrgId";
      this.cmbArPerfomer.SelectedIndex = 0;
    }

    private void cmbPayDoc_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (this.currentPay == null)
        return;
      this.currentPay.PayDoc = (PayDoc) this.cmbPayDoc.SelectedItem;
    }

    private void cmbO_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.changeText = false;
      if (this.cmbO.DroppedDown)
        this.lbOrg.Visible = false;
      if (this.cmbO.SelectedItem == null || this.lbOrg.Visible)
        return;
      this.ShowAllLease();
      this.cmbDog.SelectedValue = (object) 0;
      this.lblAr.Text = "";
      this.txbArClient.Clear();
      this.txbArValue.Clear();
      this.txbArPeni.Clear();
      this.currentLs = (LsClient) null;
    }

    private void cmbDog_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.changeDog = false;
      if (this.cmbDog.DroppedDown)
        this.lbDog.Visible = false;
      if (this.cmbDog.SelectedItem == null || this.lbDog.Visible)
        return;
      this.currentLs = ((LsArenda) this.cmbDog.SelectedItem).LsClient;
      this.AddAddressFIO();
      this.cmbO.SelectedValue = (object) 0;
      this.txbArClient.Clear();
      this.ViewList(false);
      if (!this.chbScanner.Checked)
        this.FindSum();
    }

    private void cmbArReceipt_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (Convert.ToInt32(this.cmbArReceipt.SelectedValue) == 0)
      {
        this.cmbArService.SelectedIndex = 0;
        this.cmbArService.Enabled = false;
      }
      else
      {
        IList<Service> serviceList1 = (IList<Service>) new List<Service>();
        Company company = new Company();
        IList<Service> serviceList2 = this.session.CreateQuery(string.Format("select s from Service s, ServiceParam sp where sp.Service_id=s.ServiceId and s.Root=0 and sp.Company_id={0} and sp.Receipt_id={1} and sp.Complex.IdFk={2} order by " + Options.SortService, (object) (this.currentLs == null ? (Options.Company == null ? this.session.CreateQuery("from Company where CompanyId=(select min(CompanyId) from Company)").List<Company>()[0] : Options.Company) : this.currentLs.Company).CompanyId, (object) Convert.ToInt32(this.cmbArReceipt.SelectedValue), (object) Options.ComplexArenda.IdFk)).List<Service>();
        if ((uint) serviceList2.Count > 0U)
        {
          serviceList2.Insert(0, new Service()
          {
            ServiceId = (short) 0,
            ServiceName = "общая"
          });
          this.cmbArService.DataSource = (object) serviceList2;
          this.cmbArService.ValueMember = "ServiceId";
          this.cmbArService.DisplayMember = "ServiceName";
          this.cmbArService.SelectedIndex = 0;
          this.cmbArService.Enabled = true;
        }
        else
        {
          this.cmbArService.SelectedIndex = 0;
          this.cmbArService.Enabled = false;
        }
      }
      if (this.currentPay == null)
        return;
      this.currentPay.Receipt = (Receipt) this.cmbArReceipt.SelectedItem;
    }

    private void cmbReceipt_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      if (this.cmbService.Enabled)
        this.cmbService.Focus();
      else
        this.txtSum.Focus();
    }

    private void cmbService_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.txtSum.Focus();
    }

    private void cmbSource_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.txbPacket.Focus();
    }

    private void cmbPurpose_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.cmbPayDoc.Focus();
    }

    private void cmbO_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Down)
        this.changeText = false;
      if (e.KeyCode != Keys.Return)
        return;
      if (this.cmbO.SelectedValue != null)
      {
        this.cmbDog.Focus();
      }
      else
      {
        this.cmbO.SelectedValue = (object) 0;
        this.lbOrg.Visible = false;
      }
    }

    private void cmbO_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Down || !this.cmbO.Focused)
        return;
      this.lbOrg.Focus();
      this.lbOrg.SelectedIndex = 0;
      this.cmbO.SelectedValue = (object) ((BaseOrg) this.lbOrg.SelectedItem).BaseOrgId;
    }

    private void cmbDog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Down)
        this.changeDog = false;
      if (e.KeyCode != Keys.Return)
        return;
      if (this.cmbDog.SelectedValue != null)
      {
        if ((uint) Convert.ToInt32(this.cmbDog.SelectedValue) > 0U)
          this.txbNumber.Focus();
        else
          this.txbArClient.Focus();
      }
      else
      {
        this.cmbDog.SelectedValue = (object) 0;
        this.lbDog.Visible = false;
        this.txbArClient.Clear();
      }
    }

    private void cmbPayDoc_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return && e.KeyCode != Keys.Tab)
        return;
      if (this.rbKv.Checked)
        this.txtClientId.Focus();
      else
        this.cmbO.Focus();
    }

    private void cmbArReceipt_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.cmbArService.Focus();
    }

    private void cmbArService_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      if (this.cmbArService.Enabled)
        this.cmbArService.Focus();
      else
        this.cmbSupplier.Focus();
    }

    private void cmbArSupplier_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.txbArValue.Focus();
    }

    private void cmbDog_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Down || !this.cmbDog.Focused)
        return;
      this.lbDog.Focus();
      this.lbDog.SelectedIndex = 0;
      this.cmbDog.SelectedValue = (object) ((LsArenda) this.lbDog.SelectedItem).Status;
    }

    private void cmbO_TextChanged(object sender, EventArgs e)
    {
      try
      {
        if (!this.cmbO.Focused)
          return;
        if (!this.changeText)
          this.changeText = true;
        else if (this.cmbO.Text.Length > 0)
        {
          this.org = this.cmbO.Text;
          int selectionStart = this.cmbO.SelectionStart;
          IList<BaseOrg> baseOrgList1 = (IList<BaseOrg>) new List<BaseOrg>();
          IList<BaseOrg> baseOrgList2 = this.session.CreateQuery(string.Format("select new BaseOrg(b.BaseOrgId,b.NameOrgMin) from BaseOrg b where (b.BaseOrgId in (select BaseOrg.BaseOrgId from LsArenda) or b.BaseOrgId=0) and NameOrgMin like '%{0}%' order by b.NameOrgMin", (object) this.cmbO.Text)).List<BaseOrg>();
          this.changeText = false;
          this.cmbO.Text = this.org;
          this.cmbO.SelectionStart = selectionStart;
          this.lbOrg.DataSource = (object) null;
          this.lbOrg.DisplayMember = "NameOrgMin";
          this.lbOrg.ValueMember = "BaseOrgId";
          this.lbOrg.DataSource = (object) baseOrgList2;
          this.lbOrg.SelectedIndex = -1;
          if (baseOrgList2.Count > 0)
            this.lbOrg.Visible = true;
          this.changeText = true;
        }
        else
          this.lbOrg.Visible = false;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void cmbDog_TextChanged(object sender, EventArgs e)
    {
      try
      {
        if (!this.cmbDog.Focused)
          return;
        if (!this.changeDog)
          this.changeDog = true;
        else if (this.cmbDog.Text.Length > 0)
        {
          this.dog = this.cmbDog.Text;
          int selectionStart = this.cmbDog.SelectionStart;
          IList<LsArenda> lsArendaList1 = (IList<LsArenda>) new List<LsArenda>();
          IList<LsArenda> lsArendaList2 = this.session.CreateQuery(string.Format("select new LsArenda(b.LsClient,b.DogovorNum) from LsArenda b where b.DogovorNum like '%{0}%' order by b.DogovorNum", (object) this.cmbDog.Text)).List<LsArenda>();
          this.changeDog = false;
          this.cmbDog.Text = this.dog;
          this.cmbDog.SelectionStart = selectionStart;
          this.lbDog.DataSource = (object) null;
          this.lbDog.DisplayMember = "DogovorNum";
          this.lbDog.ValueMember = "Status";
          this.lbDog.DataSource = (object) lsArendaList2;
          this.lbDog.SelectedIndex = -1;
          if (lsArendaList2.Count > 0)
            this.lbDog.Visible = true;
          this.changeDog = true;
        }
        else
          this.lbDog.Visible = false;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void txtValue_Leave(object sender, EventArgs e)
    {
      this.txtSum.Text = KvrplHelper.ChangeSeparator(this.txtSum.Text);
      this.tbItog.Text = this.txtSum.Text;
    }

    private void txtPeni_Leave(object sender, EventArgs e)
    {
      this.txtPeni.Text = KvrplHelper.ChangeSeparator(this.txtPeni.Text);
      try
      {
        this.tbItog.Text = this.txtSum.Text != "" ? (Convert.ToDouble(this.txtSum.Text) + Convert.ToDouble(this.txtPeni.Text)).ToString() : this.txtPeni.Text;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void txtValue_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Return)
        this.txtPeni.Focus();
      if (e.KeyCode != Keys.Down || !this.dgvEvidence.Visible || this.dgvEvidence.Rows.Count <= 0)
        return;
      this.dgvEvidence.Focus();
      this.dgvEvidence.CurrentCell = this.dgvEvidence.Rows[0].Cells["Current"];
    }

    private void txtPeni_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Return)
        this.txbArPeni_Leave(sender, (EventArgs) e);
      if (e.KeyCode != Keys.Down || !this.dgvEvidence.Visible || this.dgvEvidence.Rows.Count <= 0)
        return;
      this.dgvEvidence.Focus();
      this.dgvEvidence.CurrentCell = this.dgvEvidence.Rows[0].Cells["Current"];
    }

    private void txbPacket_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.cmbPurpose.Focus();
    }

    private void txtClientId_KeyUp(object sender, KeyEventArgs e)
    {
    }

    private void txtClientId_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Tab)
      {
        try
        {
          if (!this.chbScanner.Checked || this.city != 4)
            this.currentLs = !this.rbKv.Checked ? KvrplHelper.FindLs(Convert.ToInt32(this.txbArClient.Text)) : KvrplHelper.FindLs(Convert.ToInt32(this.txtClientId.Text));
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Лицевой счет не введен или введен неверно.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return;
        }
        if (this.currentLs != null)
        {
          this.city = Convert.ToInt32(KvrplHelper.BaseValue(1, this.currentLs.Company));
          if (Options.CountersInPays && this.currentLs != null && !this.chbScanner.Checked)
            this.LoadCounters();
          if ((!this.chbScanner.Checked || this.city != 4) && this.currentLs != null)
          {
            this.AddAddressFIO();
            if (this.rbKv.Checked)
            {
              this.ShowCurrentPay(this.currentLs);
            }
            else
            {
              this.cmbO.SelectedValue = (object) 0;
              this.lbOrg.Visible = false;
              this.cmbDog.SelectedValue = (object) 0;
              this.lbDog.Visible = false;
            }
          }
          if (!this.chbScanner.Checked)
            this.FindSum();
          if (!this.chbScanner.Checked && e.KeyCode == Keys.Return)
          {
            if (this.rbKv.Checked)
              this.txtSum.Focus();
            else
              this.txbNumber.Focus();
          }
        }
        else
        {
          int num = (int) MessageBox.Show("Лицевой счет не найден.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return;
        }
      }
      if (e.KeyCode != Keys.Down || !this.dgvEvidence.Visible || this.dgvEvidence.Rows.Count <= 0)
        return;
      this.dgvEvidence.Focus();
      this.dgvEvidence.CurrentCell = this.dgvEvidence.Rows[0].Cells["Current"];
    }

    private void txbArValue_Leave(object sender, EventArgs e)
    {
      try
      {
        this.txbArValue.Text = Decimal.Round(Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.txbArValue.Text)), 2).ToString();
      }
      catch (Exception ex)
      {
        return;
      }
      this.txbArValue.Text = KvrplHelper.ChangeSeparator(this.txbArValue.Text);
      try
      {
        this.txbArItog.Text = !(this.txbArValue.Text != "") || !(this.txbArPeni.Text != "") ? (this.txbArValue.Text != "" ? Convert.ToDecimal(this.txbArValue.Text).ToString() : (this.txbArPeni.Text != "" ? Convert.ToDecimal(this.txbArPeni.Text).ToString() : 0.ToString())) : Convert.ToString(Convert.ToDecimal(this.txbArValue.Text) + Convert.ToDecimal(this.txbArPeni.Text));
      }
      catch
      {
      }
      if (!this.dgvDistribute.Visible || !(this.txbArValue.Text != "") || this.dgvDistribute.Rows.Count <= 1)
        return;
      if (this.dgvDistribute.Rows.Count == 2)
      {
        this.dgvDistribute.Rows[0].Cells["Summ"].Value = (object) this.txbArValue.Text;
        this.dgvDistribute.Rows[1].Cells["Summ"].Value = (object) this.txbArValue.Text;
      }
      else if (Convert.ToDecimal(this.txbArValue.Text) != Decimal.Zero && MessageBox.Show("Разбить суммы по договорам", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
        this.DivisionSumm((short) 1);
    }

    private void txbArPeni_Leave(object sender, EventArgs e)
    {
      try
      {
        this.txbArPeni.Text = Decimal.Round(Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.txbArPeni.Text)), 2).ToString();
      }
      catch (Exception ex)
      {
        return;
      }
      this.txbArPeni.Text = KvrplHelper.ChangeSeparator(this.txbArPeni.Text);
      try
      {
        this.txbArItog.Text = !(this.txbArValue.Text != "") || !(this.txbArPeni.Text != "") ? (this.txbArValue.Text != "" ? Convert.ToDecimal(this.txbArValue.Text).ToString() : (this.txbArPeni.Text != "" ? Convert.ToDecimal(this.txbArPeni.Text).ToString() : 0.ToString())) : Convert.ToString(Convert.ToDecimal(this.txbArValue.Text) + Convert.ToDecimal(this.txbArPeni.Text));
      }
      catch
      {
      }
      if (!this.dgvDistribute.Visible || !(this.txbArPeni.Text != "") || this.dgvDistribute.Rows.Count <= 1)
        return;
      if (this.dgvDistribute.Rows.Count == 2)
      {
        this.dgvDistribute.Rows[0].Cells["SummPeni"].Value = (object) this.txbArPeni.Text;
        this.dgvDistribute.Rows[1].Cells["SummPeni"].Value = (object) this.txbArPeni.Text;
      }
      else if (Convert.ToDecimal(this.txbArPeni.Text) != Decimal.Zero && Convert.ToDecimal(this.txbArPeni.Text) != Convert.ToDecimal(this.dgvDistribute.Rows[this.dgvDistribute.Rows.Count - 1].Cells["SummPeni"].Value) && MessageBox.Show("Разбить пени по договорам", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
        this.DivisionSumm((short) 2);
    }

    private void txbArValue_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.txbArPeni.Focus();
    }

    private void txbPacket_KeyPress(object sender, KeyPressEventArgs e)
    {
      if ((int) e.KeyChar == 8 || (int) e.KeyChar == 13 || ((int) e.KeyChar == 44 || (int) e.KeyChar == 46) || (int) e.KeyChar >= 48 && (int) e.KeyChar <= 57)
        return;
      e.Handled = true;
    }

    private void txbNumber_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.txbArValue.Focus();
    }

    private void txbArValue_KeyPress(object sender, KeyPressEventArgs e)
    {
      if ((int) e.KeyChar == 8 || (int) e.KeyChar == 13 || ((int) e.KeyChar == 44 || (int) e.KeyChar == 45) || (int) e.KeyChar == 46 || (int) e.KeyChar >= 48 && (int) e.KeyChar <= 57)
        return;
      e.Handled = true;
    }

    private void mpPeriod_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.mpPeriodPay.Focus();
    }

    private void mpPeriodPay_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.dtmpDate.Focus();
    }

    private void mpPeriod_ValueChanged(object sender, EventArgs e)
    {
      if (!this.dgvEvidence.Visible || this.currentLs == null)
        return;
      this.LoadCounters();
    }

    private void mpPeriodPay_ValueChanged(object sender, EventArgs e)
    {
      if (this.rbAr.Checked && this.dgvDistribute.Visible && this.rbRent.Checked)
      {
        this.ShowAllLease();
        if (this.txbArValue.Text != "" || this.txbArPeni.Text != "")
          this.ChangeDivision();
      }
      if (this.chbScanner.Checked)
        return;
      this.FindSum();
    }

    private void dgvEvidence_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (((DataGridView) sender).DataSource == null)
        return;
      DataGridViewRow row = ((DataGridView) sender).Rows[e.RowIndex];
      if (((Evidence) row.DataBoundItem).Period != null && ((Evidence) row.DataBoundItem).Period.PeriodId == KvrplHelper.GetKvrClose(this.currentLs.ClientId, Options.Complex, Options.ComplexPrior).PeriodId + 1)
      {
        row.DefaultCellStyle.BackColor = Color.PapayaWhip;
        row.DefaultCellStyle.ForeColor = Color.Black;
        row.DefaultCellStyle.Font = new Font(this.dgvEvidence.Font, FontStyle.Regular);
      }
      else if (((Evidence) row.DataBoundItem).Period != null)
      {
        row.DefaultCellStyle.BackColor = Color.White;
        row.DefaultCellStyle.ForeColor = Color.Gray;
        row.DefaultCellStyle.Font = new Font(this.dgvEvidence.Font, FontStyle.Regular);
      }
      else
      {
        row.DefaultCellStyle.BackColor = Color.White;
        row.DefaultCellStyle.ForeColor = Color.Black;
        row.DefaultCellStyle.Font = new Font(this.dgvEvidence.Font, FontStyle.Italic);
      }
    }

    private void dgvEvidence_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    private void dgvDistribute_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (((DataGridView) sender).DataSource == null)
        return;
      DataGridViewRow row = this.dgvDistribute.Rows[e.RowIndex];
      if (row.Cells[0].Value != null && row.Cells["DogovorNum"].Value.ToString() == "ИТОГО")
        row.DefaultCellStyle.Font = new Font(this.dgvDistribute.Font, FontStyle.Bold);
    }

    private void dgvDistribute_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      Decimal num1 = new Decimal();
      if (this.dgvDistribute.CurrentCell != this.dgvDistribute.Rows[e.RowIndex].Cells[e.ColumnIndex])
        return;
      try
      {
        this.dgvDistribute.CurrentRow.Cells["Summ"].Value = (object) Decimal.Round(Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.dgvDistribute.CurrentRow.Cells["Summ"].Value.ToString())), 2);
      }
      catch (Exception ex)
      {
        int num2 = (int) MessageBox.Show("Введенное значение некорректно", "", MessageBoxButtons.OK);
        this.dgvDistribute.Rows[e.RowIndex].Cells["Summ"].Value = (object) 0;
      }
      Decimal num3 = new Decimal();
      foreach (DataGridViewRow row in (IEnumerable) this.dgvDistribute.Rows)
      {
        if (row.Cells["DogovorNum"].Value.ToString() != "ИТОГО")
          num3 += Decimal.Round(Convert.ToDecimal(row.Cells["Summ"].Value), 2);
      }
      this.dgvDistribute.Rows[this.dgvDistribute.Rows.Count - 1].Cells["Summ"].Value = (object) num3;
    }

    private void dgvEvidence_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.saveNullVolume = true;
    }

    private void lbOrg_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Up && this.lbOrg.SelectedIndex == 0 && this.lbOrg.Focused)
      {
        this.lbOrg.SelectedIndex = -1;
        this.cmbO.Focus();
        this.cmbO.Text = this.org;
        this.cmbO.SelectionStart = this.org.Length;
      }
      if (e.KeyCode != Keys.Return)
        return;
      this.cmbO.Focus();
      this.lbOrg.Visible = false;
      this.ShowAllLease();
      this.cmbDog.SelectedValue = (object) 0;
      this.lblAr.Text = "";
      this.txbArClient.Clear();
      this.currentLs = (LsClient) null;
    }

    private void lbOrg_SelectedValueChanged(object sender, EventArgs e)
    {
      if (!this.lbOrg.Focused)
        return;
      try
      {
        this.cmbO.SelectedValue = (object) ((BaseOrg) this.lbOrg.SelectedItem).BaseOrgId;
      }
      catch
      {
      }
    }

    private void lbDog_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Up && this.lbDog.SelectedIndex == 0 && this.lbDog.Focused)
      {
        this.lbDog.SelectedIndex = -1;
        this.cmbDog.Focus();
        this.cmbDog.Text = this.dog;
        this.cmbDog.SelectionStart = this.dog.Length;
      }
      if (e.KeyCode != Keys.Return)
        return;
      this.cmbDog.Focus();
      this.lbDog.Visible = false;
      this.currentLs = ((LsArenda) this.cmbDog.SelectedItem).LsClient;
      this.AddAddressFIO();
      this.cmbO.SelectedValue = (object) 0;
      this.txbArClient.Clear();
      this.ViewList(false);
      if (!this.chbScanner.Checked)
        this.FindSum();
    }

    private void lbDog_SelectedValueChanged(object sender, EventArgs e)
    {
      if (!this.lbDog.Focused)
        return;
      try
      {
        this.cmbDog.SelectedValue = (object) ((LsArenda) this.lbDog.SelectedItem).Status;
      }
      catch
      {
      }
    }

    private void lbOrg_MouseUp_1(object sender, MouseEventArgs e)
    {
      this.cmbO.Focus();
      this.lbOrg.Visible = false;
      this.ShowAllLease();
      this.cmbDog.SelectedValue = (object) 0;
      this.lblAr.Text = "";
      this.currentLs = (LsClient) null;
    }

    private void lbDog_MouseUp(object sender, MouseEventArgs e)
    {
      this.cmbDog.Focus();
      this.lbDog.Visible = false;
      this.currentLs = ((LsArenda) this.cmbDog.SelectedItem).LsClient;
      this.AddAddressFIO();
      this.cmbO.SelectedValue = (object) 0;
      this.ViewList(false);
    }

    private void tmr1_Tick(object sender, EventArgs e)
    {
    }

    private void chbScanner_CheckedChanged(object sender, EventArgs e)
    {
      Options.Scanner = this.chbScanner.Checked;
      if (!Options.Scanner)
      {
        this.txtStrah.Visible = false;
        this.lblStrah.Visible = false;
      }
      else
      {
        this.txtStrah.Visible = true;
        this.lblStrah.Visible = true;
      }
      this.txtClientId.Focus();
    }

    private void dtmpDate_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.cmbSource.Focus();
    }

    private void dtmpDate_ValueChanged(object sender, EventArgs e)
    {
      if (!this.rbKv.Checked)
        return;
      this.UpdatePacket();
    }

    private void rbAr_CheckedChanged(object sender, EventArgs e)
    {
      if (this.rbAr.Checked)
      {
        this.lblAr.Text = "";
        this.lblFIO.Text = "";
        this.pnArenda.BringToFront();
        this.lblPacket.Visible = false;
        this.txbPacket.Visible = false;
        this.txbNumber.Text = "";
        this.cmbO.Focus();
      }
      else
      {
        this.lblAr.Text = "";
        this.lblFIO.Text = "";
        this.pnKv.BringToFront();
        this.lblPacket.Visible = true;
        this.txbPacket.Visible = true;
        this.txtClientId.Focus();
      }
    }

    private void rbBalance_CheckedChanged(object sender, EventArgs e)
    {
      if (!((RadioButton) sender).Checked || !this.dgvDistribute.Visible || !(this.txbArValue.Text != "") && !(this.txbArPeni.Text != ""))
        return;
      this.ChangeDivision();
    }

    private void LoadCounters()
    {
      try
      {
        this.session.Clear();
        this.saveNullVolume = false;
        this.counterList = (IList<Counter>) new List<Counter>();
        this.session = Domain.CurrentSession;
        this.counterList = this.session.CreateQuery(string.Format("select c from Counter c left join fetch c.Service where c.Complex.ComplexId={0} and c.LsClient.ClientId={1} and c.BaseCounter.Id=2 order by isnull(c.ArchivesDate,'2999-12-31') desc,c.Service.ServiceId", (object) Options.Complex.ComplexId, (object) this.currentLs.ClientId)).List<Counter>();
        IList<Evidence> evidenceList1 = (IList<Evidence>) new List<Evidence>();
        foreach (Counter counter in (IEnumerable<Counter>) this.counterList)
        {
          Period period = this.session.CreateQuery(string.Format("from Period where PeriodName='{0}'", (object) KvrplHelper.DateToBaseFormat(this.mpPeriod.Value))).List<Period>()[0];
          IList<Evidence> evidenceList2 = this.session.CreateCriteria(typeof (Evidence)).Add((ICriterion) Restrictions.Eq("Period", (object) period)).Add((ICriterion) Restrictions.Eq("Counter", (object) counter)).List<Evidence>();
          if (evidenceList2.Count > 0)
          {
            foreach (Evidence evidence in (IEnumerable<Evidence>) evidenceList2)
            {
              evidence.Counter = counter;
              evidenceList1.Add(evidence);
            }
          }
          else
          {
            DateTime? nullable = period.PeriodName;
            DateTime dateTime1 = nullable.Value;
            nullable = KvrplHelper.GetKvrClose(this.currentLs.ClientId, Options.Complex, Options.ComplexPrior).PeriodName;
            DateTime dateTime2 = nullable.Value;
            DateTime dend;
            int num;
            if (dateTime1 > dateTime2)
            {
              nullable = counter.ArchivesDate;
              if (nullable.HasValue)
              {
                nullable = counter.ArchivesDate;
                dend = Options.Period.PeriodName.Value;
                num = nullable.HasValue ? (nullable.GetValueOrDefault() > dend ? 1 : 0) : 0;
              }
              else
                num = 1;
            }
            else
              num = 0;
            if (num != 0)
            {
              IList<Evidence> evidenceList3 = this.session.CreateQuery(string.Format("select cp from Evidence cp where cp.Counter.CounterId={1} and  cp.DBeg=(select max(DBeg) from Evidence where Counter.CounterId=cp.Counter.CounterId and Period.PeriodId<{0}) order by cp.Period.PeriodId desc,cp.DEnd desc", (object) Options.Period.PeriodId, (object) counter.CounterId)).List<Evidence>();
              Evidence evidence1 = new Evidence();
              if (evidenceList3.Count > 0)
              {
                evidence1 = evidenceList3[0];
                evidence1.Counter = counter;
                evidence1.Period = (Period) null;
                Evidence evidence2 = evidence1;
                dend = evidence1.DEnd;
                DateTime dateTime3 = dend.AddDays(1.0);
                evidence2.DBeg = dateTime3;
                evidence1.Past = evidence1.Current;
              }
              else
              {
                evidence1.Counter = counter;
                Evidence evidence2 = evidence1;
                nullable = counter.SetDate;
                DateTime dateTime3;
                if (!nullable.HasValue)
                {
                  nullable = period.PeriodName;
                  dateTime3 = KvrplHelper.FirstDay(nullable.Value);
                }
                else
                {
                  nullable = counter.SetDate;
                  dateTime3 = nullable.Value;
                }
                evidence2.DBeg = dateTime3;
                evidence1.Past = counter.EvidenceStart;
                evidence1.Current = counter.EvidenceStart;
              }
              Evidence evidence3 = evidence1;
              DateTime dateTime4;
              if (Convert.ToInt32(KvrplHelper.BaseValue(31, this.currentLs.Company)) != 1)
              {
                dateTime4 = DateTime.Now;
              }
              else
              {
                nullable = period.PeriodName;
                dateTime4 = KvrplHelper.LastDay(nullable.Value);
              }
              evidence3.DEnd = dateTime4;
              evidenceList1.Add(evidence1);
            }
          }
        }
        this.dgvEvidence.Columns.Clear();
        this.dgvEvidence.DataSource = (object) null;
        this.dgvEvidence.DataSource = (object) evidenceList1;
        KvrplHelper.AddMaskDateColumn(this.dgvEvidence, 0, "Дата настоящего", "DEnd");
        KvrplHelper.AddMaskDateColumn(this.dgvEvidence, 1, "Дата предыдущего", "DBeg");
        KvrplHelper.AddTextBoxColumn(this.dgvEvidence, 0, "Предыдущие показания", "Past", 90, false);
        KvrplHelper.AddTextBoxColumn(this.dgvEvidence, 0, "Настоящие показания", "Current", 90, false);
        this.dgvEvidence.Columns["CounterNum"].HeaderText = "Номер счетчика";
        this.dgvEvidence.Columns["Volume"].HeaderText = "Расход";
        this.dgvEvidence.Columns["ServiceName"].HeaderText = "Услуга";
        this.dgvEvidence.Columns["ServiceName"].DisplayIndex = 0;
        this.dgvEvidence.Columns["CounterNum"].DisplayIndex = 1;
        this.dgvEvidence.Columns["DBeg"].DisplayIndex = 2;
        this.dgvEvidence.Columns["Past"].DisplayIndex = 3;
        this.dgvEvidence.Columns["Current"].DisplayIndex = 5;
        this.dgvEvidence.Columns["DEnd"].DisplayIndex = 4;
        this.dgvEvidence.Columns["ServiceName"].Visible = false;
        this.dgvEvidence.Columns["Volume"].ReadOnly = true;
        this.dgvEvidence.Columns["ServiceName"].Width = 200;
        this.dgvEvidence.Columns["DEdit"].Visible = false;
        this.dgvEvidence.Columns["UName"].Visible = false;
        this.session = Domain.CurrentSession;
        KvrplHelper.AddComboBoxColumn(this.dgvEvidence, 0, (IList) this.session.CreateCriteria(typeof (Service)).Add((ICriterion) Restrictions.Eq("Root", (object) Convert.ToInt16(0))).AddOrder(Order.Asc("ServiceName")).List<Service>(), "ServiceId", "ServiceName", "Услуга", "Service", 140, 140);
        this.dgvEvidence.Columns["CounterNum"].ReadOnly = true;
        this.dgvEvidence.Columns["Service"].ReadOnly = true;
        foreach (DataGridViewRow row in (IEnumerable) this.dgvEvidence.Rows)
        {
          row.Cells["DBeg"].Value = (object) ((Evidence) row.DataBoundItem).DBeg;
          row.Cells["DEnd"].Value = (object) ((Evidence) row.DataBoundItem).DEnd;
          row.Cells["Past"].Value = (object) ((Evidence) row.DataBoundItem).Past;
          row.Cells["Current"].Value = (object) ((Evidence) row.DataBoundItem).Current;
          if (((Evidence) row.DataBoundItem).Counter != null && ((Evidence) row.DataBoundItem).Counter.Service != null)
            row.Cells["Service"].Value = (object) ((Evidence) row.DataBoundItem).Counter.Service.ServiceId;
        }
        this.session.Clear();
        this.dgvEvidence.Refresh();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void DecodeStrInputScanner()
    {
      string str1 = this.strInputScanner.Remove(2, this.strInputScanner.Length - 2);
      if (this.strInputScanner.Length == 22 || this.strInputScanner.Length == 24)
      {
        if (this.strInputScanner.IndexOf('\r') != -1)
          this.strInputScanner = this.strInputScanner.Remove(this.strInputScanner.IndexOf('\r'), 1);
        this.txtClientId.Text = this.lastStrClientId;
        if (this.lastStrClientId != "")
        {
          int num = (int) MessageBox.Show("");
          this.btnSave_Click((object) null, (EventArgs) null);
        }
        this.txtSum.Text = "";
        this.txtPeni.Text = "";
        this.lblFIO.Text = "";
        string str2 = this.strInputScanner.Remove(0, 2);
        this.txtClientId.Text = str2.Remove(9, str2.Length - 9);
        string str3 = str2.Remove(0, 9);
        string str4 = str3.Remove(4, str3.Length - 4);
        double num1 = Convert.ToDouble(str3.Remove(0, 4)) / 100.0;
        this.txtSum.Text = num1.ToString();
        this.mpPeriodPay.Value = Convert.ToDateTime(string.Format("1.{0}.{1}", (object) str4.Remove(2, 2), (object) str4.Remove(0, 2)));
        this.strInputScanner = "";
        this.lastStrClientId = this.txtClientId.Text;
        if (str1 == "00" || str1 == "01")
          this.cmbReceipt.SelectedValue = (object) Convert.ToInt16(1);
        else
          this.cmbReceipt.SelectedValue = (object) Convert.ToInt16(str1);
        this.tbItog.Text = num1.ToString();
        IList<LsClient> lsClientList = this.session.CreateQuery(string.Format("from LsClient where ClientId={0}", (object) this.txtClientId.Text)).List<LsClient>();
        if (lsClientList.Count > 0)
        {
          this.currentLs = lsClientList[0];
          this.AddAddressFIO();
          if (Options.CountersInPays)
            this.LoadCounters();
          this.ShowCurrentPay(lsClientList[0]);
        }
      }
      if (str1 == "99" && this.strInputScanner.Length == 18)
      {
        if (this.strInputScanner.IndexOf('\r') != -1)
          this.strInputScanner = this.strInputScanner.Remove(this.strInputScanner.IndexOf('\r'), 1);
        this.Peni = true;
        string str2 = this.strInputScanner.Remove(0, 2);
        this.txtClientId.Text = str2.Remove(9, str2.Length - 9);
        string str3 = str2.Remove(9, str2.Length - 9);
        string str4 = str2.Remove(0, 9);
        if ((this.lastStrClientId != str3 || this.lastStrClientId == "") && this.Peni)
        {
          this.txtClientId.Text = "";
          int num = (int) MessageBox.Show("Введите сначала основной платеж, а затем платеж по пеням!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.strInputScanner = "";
          return;
        }
        double num1 = Convert.ToDouble(str4) / 100.0;
        this.txtPeni.Text = num1.ToString();
        try
        {
          this.tbItog.Text = this.txtSum.Text != "" ? (Convert.ToDouble(this.txtSum.Text) + num1).ToString() : num1.ToString();
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        this.strInputScanner = "";
      }
      if (!(str1 == "99") || this.strInputScanner.Length != 20)
        return;
      if (this.strInputScanner.IndexOf('\r') != -1)
        this.strInputScanner = this.strInputScanner.Remove(this.strInputScanner.IndexOf('\r'), 1);
      this.Peni = true;
      string str5 = this.strInputScanner.Remove(0, 2);
      this.txtClientId.Text = str5.Remove(9, str5.Length - 9);
      string str6 = str5.Remove(9, str5.Length - 9);
      string str7 = str5.Remove(0, 9);
      if ((this.lastStrClientId != str6 || this.lastStrClientId == "") && this.Peni)
      {
        this.txtClientId.Text = "";
        int num = (int) MessageBox.Show("Введите сначала основной платеж, а затем платеж по пеням!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.strInputScanner = "";
      }
      else
      {
        double num1 = 1.0;
        if (str7.Substring(0, 1) == "1")
          num1 = -1.0;
        string str2 = str7.Remove(0, 1);
        double num2 = num1 * Convert.ToDouble(str2) / 100.0;
        this.txtPeni.Text = num2.ToString();
        try
        {
          this.tbItog.Text = this.txtSum.Text != "" ? (Convert.ToDouble(this.txtSum.Text) + num2).ToString() : num2.ToString();
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        this.strInputScanner = "";
      }
    }

    private void AddAddressFIO()
    {
      this.lblFIO.Text = "";
      IQuery query = this.session.CreateQuery(string.Format("select new Address(c.ClientId,d.NameStr,h.NHome,h.HomeKorp,f.NFlat,c.SurFlat) from Home h, Str d, Flat f,LsClient c where h.Str=d and c.Home=h and c.Flat=f and c.ClientId={0} ", (object) this.currentLs.ClientId));
      Address address = new Address();
      try
      {
        address = (Address) query.List()[0];
      }
      catch
      {
      }
      this.currentLs.Fio = !KvrplHelper.CheckProxy(48, 1, this.currentLs.Company, false) ? "" : KvrplHelper.GetFio1(this.currentLs.ClientId);
      this.lblFIO.Text = this.currentLs.Fio;
      this.lblFIO.Text = this.lblFIO.Text + " " + string.Format("{0} д.{1} {2} кв.{3} {4}", (object) address.Str, (object) address.Number, (object) (address.Korp == "" || address.Korp == "0" || address.Korp == null ? "" : "к." + address.Korp), (object) address.Flat, (object) (address.SurFlat == "" || address.SurFlat == "0" || address.SurFlat == null ? "" : "комн." + address.SurFlat));
      if ((uint) this.currentLs.ClientId > 0U)
        this.lblAr.Text = "ЛС №" + this.currentLs.ClientId.ToString() + "  " + this.lblFIO.Text;
      else
        this.lblAr.Text = "";
      this.dgvDistribute.Visible = false;
      if (!this.rbAr.Checked)
        return;
      this.txbNumber.Focus();
    }

    private void ShowAllLease()
    {
      this.txbNumber.Focus();
      Decimal num1 = new Decimal();
      Decimal num2 = new Decimal();
      Decimal num3 = new Decimal();
      Decimal num4 = new Decimal();
      IList<LsArenda> lsArendaList = this.session.CreateQuery(string.Format("select new LsArenda(a.LsClient,a.DogovorNum,(select sum(Rent) from Balance where Period.PeriodId={1} and LsClient.ClientId=a.LsClient.ClientId),(select sum(Rent) from BalancePeni where Period.PeriodId={1} and LsClient.ClientId=a.LsClient.ClientId),(select sum(BalanceOut) from Balance where Period.PeriodId=(select max(Period.PeriodId) from CompanyPeriod where Company.CompanyId=l.Company.CompanyId and Complex.IdFk in ({2},{3})) and LsClient.ClientId=a.LsClient.ClientId),(select sum(BalanceOut) from BalancePeni where Period.PeriodId=(select max(Period.PeriodId) from CompanyPeriod where Company.CompanyId=l.Company.CompanyId and Complex.IdFk in ({2},{3})) and LsClient.ClientId=a.LsClient.ClientId),(select max(Period.PeriodId-MonthDept.PeriodId) from Balance where Period.PeriodId=(select max(Period.PeriodId) from CompanyPeriod where Company.CompanyId=l.Company.CompanyId and Complex.IdFk in ({2},{3})) and LsClient.ClientId=a.LsClient.ClientId)) from LsArenda a,LsClient l where a.LsClient=l and a.BaseOrg.BaseOrgId={0} order by a.DogovorNum", (object) ((BaseOrg) this.cmbO.SelectedItem).BaseOrgId, (object) this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) this.mpPeriodPay.Value)).List<Period>()[0].PeriodId, (object) Options.Complex.IdFk, (object) Options.ComplexPrior.IdFk)).List<LsArenda>();
      if (lsArendaList.Count == 0)
      {
        int num5 = (int) MessageBox.Show("У организации нет ни одного договора. Невозможно внести платеж.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.cmbO.Focus();
        this.ViewList(false);
      }
      else
      {
        foreach (LsArenda lsArenda in (IEnumerable<LsArenda>) lsArendaList)
        {
          if (lsArenda.Balance > 0.0)
            num1 += Convert.ToDecimal(lsArenda.Balance);
          if (lsArenda.Peni > 0.0)
            num2 += Convert.ToDecimal(lsArenda.Peni);
          if (lsArenda.Rent > 0.0)
            num3 += Convert.ToDecimal(lsArenda.Rent);
          if (lsArenda.RentPeni > 0.0)
            num4 += Convert.ToDecimal(lsArenda.RentPeni);
        }
        lsArendaList.Insert(lsArendaList.Count, new LsArenda((LsClient) null, "ИТОГО", Convert.ToDouble(num3), Convert.ToDouble(num4), Convert.ToDouble(num1), Convert.ToDouble(num2), new int?()));
        this.dgvDistribute.DataSource = (object) null;
        this.dgvDistribute.Columns.Clear();
        this.dgvDistribute.DataSource = (object) lsArendaList;
        this.SetViewDistribute();
        this.ViewList(true);
      }
    }

    private void SetViewDistribute()
    {
      this.dgvDistribute.Columns["NameOrg"].Visible = false;
      this.dgvDistribute.Columns["KumiNum"].Visible = false;
      this.dgvDistribute.Columns["Status"].Visible = false;
      this.dgvDistribute.Columns["DogovorNum"].HeaderText = "№ договора";
      this.dgvDistribute.Columns["NumLs"].HeaderText = "№ ЛС";
      this.dgvDistribute.Columns["Balance"].HeaderText = "К оплате на текущий момент";
      this.dgvDistribute.Columns["Peni"].HeaderText = "Пени на текущий момент";
      this.dgvDistribute.Columns["Rent"].HeaderText = "Начислено";
      this.dgvDistribute.Columns["RentPeni"].HeaderText = "Начислено пени";
      this.dgvDistribute.Columns["Months"].HeaderText = "Месяцев долга";
      this.dgvDistribute.Columns["DogovorNum"].ReadOnly = true;
      this.dgvDistribute.Columns["NumLs"].ReadOnly = true;
      this.dgvDistribute.Columns["Balance"].ReadOnly = true;
      this.dgvDistribute.Columns["Peni"].ReadOnly = true;
      this.dgvDistribute.Columns["Rent"].ReadOnly = true;
      this.dgvDistribute.Columns["RentPeni"].ReadOnly = true;
      this.dgvDistribute.Columns["Months"].ReadOnly = true;
      this.dgvDistribute.Columns["DogovorNum"].DisplayIndex = 0;
      this.dgvDistribute.Columns["NumLs"].DisplayIndex = 1;
      this.dgvDistribute.Columns["Rent"].DisplayIndex = 2;
      this.dgvDistribute.Columns["RentPeni"].DisplayIndex = 3;
      this.dgvDistribute.Columns["Balance"].DisplayIndex = 4;
      this.dgvDistribute.Columns["Peni"].DisplayIndex = 5;
      this.dgvDistribute.Columns["Months"].DisplayIndex = 6;
      KvrplHelper.AddTextBoxColumn(this.dgvDistribute, 7, "Сумма", "Summ", 100, false);
      KvrplHelper.AddTextBoxColumn(this.dgvDistribute, 8, "Сумма пеней", "SummPeni", 100, false);
      if (this.dgvDistribute.Rows.Count <= 0)
        return;
      this.dgvDistribute.Rows[this.dgvDistribute.Rows.Count - 1].ReadOnly = true;
    }

    private void SaveArenda()
    {
      this.session.Clear();
      this.session = Domain.CurrentSession;
      if (this.txbArValue.Text == "")
        this.txbArValue.Text = "0";
      if (this.txbArPeni.Text == "")
        this.txbArPeni.Text = "0";
      if (Options.City != 35)
      {
        if (((BaseOrg) this.cmbO.SelectedItem).BaseOrgId == 0 && this.currentLs == null)
        {
          int num = (int) MessageBox.Show("Выберите организацию или договор", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.cmbO.Focus();
          return;
        }
        if (((BaseOrg) this.cmbO.SelectedItem).BaseOrgId != 0 && !this.dgvDistribute.Visible)
        {
          int num = (int) MessageBox.Show("У организации нет ни одного договора. Невозможно внести платеж.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.cmbO.Focus();
          return;
        }
      }
      Decimal num1 = new Decimal();
      Decimal num2 = new Decimal();
      Decimal num3;
      Decimal num4;
      try
      {
        num3 = Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.txbArValue.Text));
        num4 = Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.txbArPeni.Text));
      }
      catch (Exception ex)
      {
        int num5 = (int) MessageBox.Show("Введенные суммы некорректны", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txbArValue.Focus();
        return;
      }
      if (num3 == Decimal.Zero && num4 == Decimal.Zero)
      {
        int num5 = (int) MessageBox.Show("Суммы не введены", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txbArValue.Focus();
      }
      else
      {
        IList list1 = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) this.mpPeriod.Value)).List();
        IList list2 = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) this.mpPeriodPay.Value)).List();
        if (list1.Count == 0 || list2.Count == 0)
        {
          int num5 = (int) MessageBox.Show("Введен некорректный период", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.mpPeriod.Focus();
        }
        else if (this.txbNumber.Text == "")
        {
          int num5 = (int) MessageBox.Show("Номер платежного документа не заполнен", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.txbPacket.Focus();
        }
        else
        {
          Period period1 = (Period) list1[0];
          Period period2 = (Period) list2[0];
          Period period3 = new Period();
          Period kvrClose;
          if (this.currentLs != null)
          {
            kvrClose = KvrplHelper.GetKvrClose(this.currentLs.ClientId, Options.Complex, Options.ComplexPrior);
          }
          else
          {
            kvrClose = this.session.Get<Period>((object) Convert.ToInt32(this.session.CreateQuery(string.Format("select max(Period.PeriodId) from CompanyPeriod where Complex.IdFk in ({0},{1})", (object) Options.Complex.IdFk, (object) Options.ComplexPasp.IdFk)).List()[0]));
            this.currentLs = KvrplHelper.FindLs(Convert.ToInt32(this.txbArClient.Text));
          }
          string str = "";
          if ((uint) this._serviceId > 0U)
            str = string.Format(" and (s.Service.ServiceId={0} or s.Service.ServiceId=(select Root from Service where ServiceId={0}))", (object) this._serviceId);
          DateTime? periodName;
          DateTime now;
          try
          {
            if ((uint) ((BaseOrg) this.cmbArRecipient.SelectedItem).BaseOrgId > 0U)
            {
              ISession session = this.session;
              string format = "Select distinct d.Recipient from CmpSupplier s,Supplier d where s.SupplierOrg.SupplierId=d.SupplierId and d.Recipient.BaseOrgId=" + (object) ((BaseOrg) this.cmbArRecipient.SelectedItem).BaseOrgId + " and s.Company.CompanyId=isnull((select ParamValue from CompanyParam where Period.PeriodId=0 and Company.CompanyId={0} and DBeg<='{2}' and DEnd>='{2}' and Param.ParamId=211),0) " + str + " order by d.Recipient.NameOrgMin";
              // ISSUE: variable of a boxed type
              short companyId = this.currentLs.Company.CompanyId;
              // ISSUE: variable of a boxed type
              int serviceId = this._serviceId;
              periodName = KvrplHelper.GetCmpKvrClose(this.currentLs.Company, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk).PeriodName;
              now = periodName.Value;
              string baseFormat = KvrplHelper.DateToBaseFormat(now.AddMonths(1));
              string queryString = string.Format(format, (object) companyId, (object) serviceId, (object) baseFormat);
              if (session.CreateQuery(queryString).UniqueResult<BaseOrg>() == null)
              {
                int num5 = (int) MessageBox.Show("Данный получатель не относится к этому лицевому!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
              }
            }
            if ((uint) ((BaseOrg) this.cmbArPerfomer.SelectedItem).BaseOrgId > 0U)
            {
              ISession session = this.session;
              string format = "Select distinct d.Perfomer from CmpSupplier s,Supplier d where s.SupplierOrg.SupplierId=d.SupplierId  and d.Perfomer.BaseOrgId=" + (object) ((BaseOrg) this.cmbArPerfomer.SelectedItem).BaseOrgId + " and s.Company.CompanyId=isnull((select ParamValue from CompanyParam where Period.PeriodId=0 and Company.CompanyId={0} and DBeg<='{2}' and DEnd>='{2}'and Param.ParamId=211),0) " + str + " order by d.Perfomer.NameOrgMin";
              // ISSUE: variable of a boxed type
              short companyId = this.currentLs.Company.CompanyId;
              // ISSUE: variable of a boxed type
              int serviceId = this._serviceId;
              periodName = KvrplHelper.GetCmpKvrClose(this.currentLs.Company, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk).PeriodName;
              now = periodName.Value;
              string baseFormat = KvrplHelper.DateToBaseFormat(now.AddMonths(1));
              string queryString = string.Format(format, (object) companyId, (object) serviceId, (object) baseFormat);
              if (session.CreateQuery(queryString).UniqueResult<BaseOrg>() == null)
              {
                int num5 = (int) MessageBox.Show("Данный исполнитель не относится к этому лицевому!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
              }
            }
          }
          catch (Exception ex)
          {
            int num5 = (int) MessageBox.Show("Неизвестная ошибка, проверьте данные!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
          periodName = period1.PeriodName;
          now = kvrClose.PeriodName.Value;
          if (periodName.HasValue && periodName.GetValueOrDefault() <= now)
          {
            int num5 = (int) MessageBox.Show("Невозможно внести платеж в закрытый период!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            this.mpPeriod.Focus();
          }
          else
          {
            if (this.dgvDistribute.Visible)
            {
              if (Convert.ToDecimal(this.dgvDistribute.Rows[this.dgvDistribute.Rows.Count - 1].Cells["Summ"].Value) != Convert.ToDecimal(this.txbArValue.Text) && MessageBox.Show("Сумма платежа не равна сумме разбиения по лицевым. Принять сумму разбиения?", "Внимание!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel || Convert.ToDecimal(this.dgvDistribute.Rows[this.dgvDistribute.Rows.Count - 1].Cells["SummPeni"].Value) != Convert.ToDecimal(this.txbArPeni.Text) && MessageBox.Show("Сумма пеней не равна сумме разбиения по лицевым. Принять сумму разбиения?", "Внимание!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;
              if (this.cmbArRecipientAuto.SelectedIndex == 0)
              {
                int num5 = (int) MessageBox.Show("Не выбран поставщик автоматический", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
              }
              this.Cursor = Cursors.WaitCursor;
              using (ITransaction transaction = this.session.BeginTransaction())
              {
                try
                {
                  foreach (DataGridViewRow row in (IEnumerable) this.dgvDistribute.Rows)
                  {
                    if (row.Index != this.dgvDistribute.Rows.Count - 1 && (Convert.ToDecimal(row.Cells["Summ"].Value) != Decimal.Zero || Convert.ToDecimal(row.Cells["SummPeni"].Value) != Decimal.Zero))
                    {
                      this.currentPay = new Payment();
                      this.currentPay.PaymentId = this.session.CreateSQLQuery("select DBA.gen_id('lsPayment',1)").List<int>()[0];
                      this.currentPay.LsClient = ((LsArenda) row.DataBoundItem).LsClient;
                      this.currentPay.Period = period1;
                      this.currentPay.PeriodPay = period2;
                      this.currentPay.Service = (Service) this.cmbArService.SelectedItem;
                      this.currentPay.Receipt = this.session.Get<Receipt>(this.cmbArReceipt.SelectedValue);
                      this.currentPay.SPay = (SourcePay) this.cmbSource.SelectedItem;
                      this.currentPay.PPay = (PurposePay) this.cmbPurpose.SelectedItem;
                      this.currentPay.PayDoc = (PayDoc) this.cmbPayDoc.SelectedItem;
                      this.currentPay.PacketNum = this.txbNumber.Text;
                      this.currentPay.PaymentValue = Convert.ToDecimal(row.Cells["Summ"].Value);
                      this.currentPay.PaymentPeni = Convert.ToDecimal(row.Cells["SummPeni"].Value);
                      this.currentPay.PaymentDate = this.dtmpDate.Value;
                      this.currentPay.Supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(this.cmbArRecipient.SelectedValue), (object) Convert.ToInt32(this.cmbArPerfomer.SelectedValue))).List<Supplier>()[0];
                      this.currentPay.UName = Options.Login;
                      Payment currentPay = this.currentPay;
                      now = DateTime.Now;
                      DateTime date = now.Date;
                      currentPay.DEdit = date;
                      this.currentPay.RecipientId = this.session.CreateQuery("from BaseOrg where BaseOrgId=:id").SetParameter<int>("id", ((BaseOrg) this.cmbArRecipientAuto.SelectedItem).BaseOrgId).UniqueResult<BaseOrg>();
                      this.session.Save((object) this.currentPay);
                      this.session.Flush();
                      this.Pays.Add(this.currentPay);
                      this.currentPay = (Payment) null;
                    }
                  }
                  transaction.Commit();
                }
                catch (Exception ex)
                {
                  transaction.Rollback();
                  int num5 = (int) MessageBox.Show("Не удалось сохранить изменения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  KvrplHelper.WriteLog(ex, (LsClient) null);
                }
                this.changeText = false;
                this.ClearArFields();
                this.cmbO.Focus();
              }
            }
            else
            {
              this.Cursor = Cursors.WaitCursor;
              BaseOrg selectedItem = (BaseOrg) this.cmbArRecipientAuto.SelectedItem;
              if (this.currentPay == null)
              {
                this.currentPay = new Payment();
                this.currentPay.PaymentId = this.session.CreateSQLQuery("select DBA.gen_id('lsPayment',1)").List<int>()[0];
                this.currentPay.LsClient = this.currentLs;
                this.currentPay.Period = period1;
                this.currentPay.PeriodPay = period2;
                this.currentPay.Service = (Service) this.cmbArService.SelectedItem;
                this.currentPay.Receipt = this.session.Get<Receipt>(this.cmbArReceipt.SelectedValue);
                this.currentPay.SPay = (SourcePay) this.cmbSource.SelectedItem;
                this.currentPay.PPay = (PurposePay) this.cmbPurpose.SelectedItem;
                this.currentPay.PayDoc = (PayDoc) this.cmbPayDoc.SelectedItem;
                this.currentPay.PacketNum = this.txbNumber.Text;
                this.currentPay.PaymentValue = Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.txbArValue.Text));
                this.currentPay.PaymentPeni = Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.txbArPeni.Text));
                this.currentPay.PaymentDate = this.dtmpDate.Value;
                this.currentPay.Supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(this.cmbArRecipient.SelectedValue), (object) Convert.ToInt32(this.cmbArPerfomer.SelectedValue))).List<Supplier>()[0];
                this.currentPay.UName = Options.Login;
                Payment currentPay = this.currentPay;
                now = DateTime.Now;
                DateTime date = now.Date;
                currentPay.DEdit = date;
                this.currentPay.RecipientId = this.session.CreateQuery("from BaseOrg where BaseOrgId=:id").SetParameter<int>("id", selectedItem == null ? 0 : selectedItem.BaseOrgId).UniqueResult<BaseOrg>();
                this.session.Save((object) this.currentPay);
                this.currentPay = (Payment) null;
                this.changeText = false;
                this.cmbO.Focus();
                this.ClearArFields();
              }
              else
              {
                this.currentPay.Service = (Service) this.cmbArService.SelectedItem;
                this.currentPay.Receipt = this.session.Get<Receipt>(this.cmbArReceipt.SelectedValue);
                this.currentPay.SPay = (SourcePay) this.cmbSource.SelectedItem;
                this.currentPay.PPay = (PurposePay) this.cmbPurpose.SelectedItem;
                this.currentPay.PayDoc = (PayDoc) this.cmbPayDoc.SelectedItem;
                this.currentPay.PacketNum = this.txbNumber.Text;
                this.currentPay.PaymentValue = Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.txbArValue.Text));
                this.currentPay.PaymentPeni = Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.txbArPeni.Text));
                this.currentPay.PaymentDate = this.dtmpDate.Value;
                this.currentPay.UName = Options.Login;
                Payment currentPay = this.currentPay;
                now = DateTime.Now;
                DateTime date = now.Date;
                currentPay.DEdit = date;
                this.currentPay.RecipientId = this.session.CreateQuery("from BaseOrg where BaseOrgId=:id").SetParameter<int>("id", selectedItem == null ? 0 : selectedItem.BaseOrgId).UniqueResult<BaseOrg>();
                this.currentPay.Supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(this.cmbArRecipient.SelectedValue), (object) Convert.ToInt32(this.cmbArPerfomer.SelectedValue))).List<Supplier>()[0];
                this.session.Update((object) this.currentPay);
                this.Pays.Clear();
              }
              try
              {
                this.session.Flush();
                this.Pays.Add(this.currentPay);
              }
              catch (Exception ex)
              {
                int num5 = (int) MessageBox.Show("Не удалось сохранить изменения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                KvrplHelper.WriteLog(ex, (LsClient) null);
              }
            }
            this.Cursor = Cursors.Default;
          }
        }
      }
    }

    private void DivisionSumm(short code)
    {
      if (this.txbArValue.Text == "")
        this.txbArValue.Text = "0";
      if (this.txbArPeni.Text == "")
        this.txbArPeni.Text = "0";
      Decimal num1 = new Decimal();
      Decimal num2 = new Decimal();
      Decimal num3 = new Decimal();
      Decimal num4 = new Decimal();
      Decimal num5 = new Decimal();
      short num6 = 1;
      bool flag1 = false;
      bool flag2 = false;
      string index1 = "";
      string index2 = "";
      double num7 = 0.0;
      if ((int) code == 1)
      {
        index1 = !this.rbBalance.Checked ? "Rent" : "Balance";
        index2 = "Summ";
        num1 = Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.txbArValue.Text));
      }
      if ((int) code == 2)
      {
        index1 = !this.rbBalance.Checked ? "RentPeni" : "Peni";
        index2 = "SummPeni";
        num1 = Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.txbArPeni.Text));
      }
      foreach (DataGridViewRow row in (IEnumerable) this.dgvDistribute.Rows)
      {
        if (Convert.ToDecimal(row.Cells[index1].Value) >= Decimal.Zero)
          flag1 = true;
        if (Convert.ToDecimal(row.Cells[index1].Value) < Decimal.Zero)
          flag2 = true;
      }
      if (flag1 && !flag2)
        num6 = (short) 1;
      if (flag1 & flag2)
        num6 = (short) 2;
      if (!flag1 & flag2)
        num6 = (short) 3;
      foreach (LsArenda lsArenda in (IEnumerable<LsArenda>) (this.dgvDistribute.DataSource as IList<LsArenda>))
      {
        if (lsArenda.DogovorNum != "ИТОГО")
        {
          if ((int) code == 1)
            num7 = !this.rbBalance.Checked ? lsArenda.Rent : lsArenda.Balance;
          if ((int) code == 2)
            num7 = !this.rbBalance.Checked ? lsArenda.RentPeni : lsArenda.Peni;
          if (num7 > 0.0)
          {
            num2 += Convert.ToDecimal(num7);
            num4 += Convert.ToDecimal(num7);
          }
        }
      }
      if (num1 == num2)
      {
        foreach (DataGridViewRow row in (IEnumerable) this.dgvDistribute.Rows)
        {
          if (row.Cells["DogovorNum"].Value.ToString() != "ИТОГО")
            row.Cells[index2].Value = row.Cells[index1].Value;
        }
      }
      if (num1 > num2)
      {
        if ((int) num6 == 1 || (int) num6 == 3)
        {
          int index3 = 0;
          foreach (DataGridViewRow row in (IEnumerable) this.dgvDistribute.Rows)
          {
            if (row.Cells["DogovorNum"].Value.ToString() != "ИТОГО")
            {
              if (Convert.ToDecimal(row.Cells[index1].Value) > Decimal.Zero && index3 == 0)
                index3 = row.Index;
              Decimal num8 = !(num2 != Decimal.Zero) ? Decimal.Round(num1 / (Decimal) (this.dgvDistribute.Rows.Count - 1), 2) : Decimal.Round(Convert.ToDecimal(row.Cells[index1].Value) / num2 * num1, 2);
              num3 += num8;
              row.Cells[index2].Value = (object) num8;
            }
          }
          if (num3 != num1)
            this.dgvDistribute.Rows[index3].Cells[index2].Value = (object) (Convert.ToDecimal(this.dgvDistribute.Rows[index3].Cells[index2].Value) + num1 - num3);
        }
        if ((int) num6 == 2)
        {
          int index3 = 0;
          foreach (DataGridViewRow row in (IEnumerable) this.dgvDistribute.Rows)
          {
            if (row.Cells["DogovorNum"].Value.ToString() != "ИТОГО")
            {
              Decimal num8;
              if (Convert.ToDecimal(row.Cells[index1].Value) > Decimal.Zero)
              {
                if (index3 == 0)
                  index3 = row.Index;
                num8 = Decimal.Round(Convert.ToDecimal(row.Cells[index1].Value) / num4 * num1, 2);
              }
              else
                num8 = new Decimal();
              num3 += num8;
              row.Cells[index2].Value = (object) num8;
            }
          }
          if (num3 != num1)
            this.dgvDistribute.Rows[index3].Cells[index2].Value = (object) (Convert.ToDecimal(this.dgvDistribute.Rows[index3].Cells[index2].Value) + num1 - num3);
        }
      }
      if (num1 < num2)
      {
        Decimal num8 = num1;
        foreach (DataGridViewRow row in (IEnumerable) this.dgvDistribute.Rows)
        {
          if (Convert.ToDecimal(row.Cells[index1].Value) < num8)
          {
            if (row.Cells["DogovorNum"].Value.ToString() != "ИТОГО")
            {
              if (Convert.ToDecimal(row.Cells[index1].Value) > Decimal.Zero)
              {
                row.Cells[index2].Value = row.Cells[index1].Value;
                num8 -= Convert.ToDecimal(row.Cells[index1].Value);
              }
              else
                row.Cells[index2].Value = (object) 0;
            }
          }
          else if (row.Cells["DogovorNum"].Value.ToString() != "ИТОГО")
          {
            row.Cells[index2].Value = (object) num8;
            num8 = new Decimal();
          }
        }
      }
      Decimal num9 = new Decimal();
      foreach (DataGridViewRow row in (IEnumerable) this.dgvDistribute.Rows)
      {
        if (row.Cells["DogovorNum"].Value.ToString() != "ИТОГО")
          num9 += Decimal.Round(Convert.ToDecimal(row.Cells[index2].Value), 2);
      }
      this.dgvDistribute.Rows[this.dgvDistribute.Rows.Count - 1].Cells[index2].Value = (object) num9;
    }

    private void DivisionPeni()
    {
      if (this.txbArPeni.Text == "")
        this.txbArPeni.Text = "0";
      Decimal num1 = Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.txbArPeni.Text));
      Decimal num2 = new Decimal();
      Decimal num3 = new Decimal();
      Decimal num4 = new Decimal();
      Decimal num5 = new Decimal();
      short num6 = 1;
      bool flag1 = false;
      bool flag2 = false;
      string index1 = !this.rbBalance.Checked ? "RentPeni" : "Peni";
      foreach (DataGridViewRow row in (IEnumerable) this.dgvDistribute.Rows)
      {
        if (Convert.ToDecimal(row.Cells[index1].Value) >= Decimal.Zero)
          flag1 = true;
        if (Convert.ToDecimal(row.Cells[index1].Value) < Decimal.Zero)
          flag2 = true;
      }
      if (flag1 && !flag2)
        num6 = (short) 1;
      if (flag1 & flag2)
        num6 = (short) 2;
      if (!flag1 & flag2)
        num6 = (short) 3;
      foreach (LsArenda lsArenda in (IEnumerable<LsArenda>) (this.dgvDistribute.DataSource as IList<LsArenda>))
      {
        if (lsArenda.DogovorNum != "ИТОГО")
        {
          double num7 = !this.rbBalance.Checked ? lsArenda.RentPeni : lsArenda.Peni;
          num2 += Convert.ToDecimal(num7);
          if (num7 > 0.0)
            num4 += Convert.ToDecimal(num7);
        }
      }
      if (num1 == num2)
      {
        foreach (DataGridViewRow row in (IEnumerable) this.dgvDistribute.Rows)
        {
          if (row.Cells["DogovorNum"].Value.ToString() != "ИТОГО")
            row.Cells["SummPeni"].Value = row.Cells[index1].Value;
        }
      }
      if (num1 > num2)
      {
        if ((int) num6 == 1 || (int) num6 == 3)
        {
          foreach (DataGridViewRow row in (IEnumerable) this.dgvDistribute.Rows)
          {
            if (row.Cells["DogovorNum"].Value.ToString() != "ИТОГО")
            {
              Decimal num7 = !(num2 != Decimal.Zero) ? Decimal.Round(num1 / (Decimal) (this.dgvDistribute.Rows.Count - 1), 2) : Decimal.Round(Convert.ToDecimal(row.Cells[index1].Value) / num2 * num1, 2);
              num3 += num7;
              row.Cells["SummPeni"].Value = (object) num7;
            }
          }
          if (num3 != num1)
            this.dgvDistribute.Rows[0].Cells["SummPeni"].Value = (object) (Convert.ToDecimal(this.dgvDistribute.Rows[0].Cells["SummPeni"].Value) + num1 - num3);
        }
        if ((int) num6 == 2)
        {
          int index2 = 0;
          foreach (DataGridViewRow row in (IEnumerable) this.dgvDistribute.Rows)
          {
            if (row.Cells["DogovorNum"].Value.ToString() != "ИТОГО")
            {
              Decimal num7;
              if (Convert.ToDecimal(row.Cells[index1].Value) > Decimal.Zero)
              {
                if (index2 == 0)
                  index2 = row.Index;
                num7 = Decimal.Round(Convert.ToDecimal(row.Cells["BalancePeni"].Value) / num4 * num1, 2);
              }
              else
                num7 = new Decimal();
              num3 += num7;
              row.Cells["SummPeni"].Value = (object) num7;
            }
          }
          if (num3 != num1)
            this.dgvDistribute.Rows[index2].Cells["SummPeni"].Value = (object) (Convert.ToDecimal(this.dgvDistribute.Rows[0].Cells["SummPeni"].Value) + num1 - num3);
        }
      }
      if (num1 < num2)
      {
        Decimal num7 = num1;
        foreach (DataGridViewRow row in (IEnumerable) this.dgvDistribute.Rows)
        {
          if (Convert.ToDecimal(row.Cells[index1].Value) < num7)
          {
            if (row.Cells["DogovorNum"].Value.ToString() != "ИТОГО")
            {
              row.Cells["SummPeni"].Value = row.Cells[index1].Value;
              num7 -= Convert.ToDecimal(row.Cells[index1].Value);
            }
          }
          else if (row.Cells["DogovorNum"].Value.ToString() != "ИТОГО")
          {
            row.Cells["SummPeni"].Value = (object) num7;
            num7 = new Decimal();
          }
        }
      }
      Decimal num8 = new Decimal();
      foreach (DataGridViewRow row in (IEnumerable) this.dgvDistribute.Rows)
      {
        if (row.Cells["DogovorNum"].Value.ToString() != "ИТОГО")
          num8 += Decimal.Round(Convert.ToDecimal(row.Cells["SummPeni"].Value), 2);
      }
      this.dgvDistribute.Rows[this.dgvDistribute.Rows.Count - 1].Cells["SummPeni"].Value = (object) num8;
    }

    private void ClearArFields()
    {
      this.cmbO.SelectedValue = (object) 0;
      this.cmbDog.SelectedIndex = 0;
      this.txbArClient.Clear();
      this.dgvDistribute.Visible = false;
      this.lblAr.Text = "";
      this.txbArValue.Clear();
      this.txbArPeni.Clear();
      this.txbArItog.Clear();
      this.txbNumber.Clear();
    }

    private void ViewList(bool value)
    {
      this.dgvDistribute.Visible = value;
    }

    private int DecodeStrScan(string ch, short n)
    {
      int num = 0;
      for (short index = 0; (int) index < (int) n; ++index)
        num = num * 43 + this.DecodeOneDigit(ch.Substring((int) index, 1));
      return num;
    }

    private int DecodeOneDigit(string ch)
    {
      int num = (int) ch[0];
      return !(ch == "0") ? (!(ch == "1") ? (!(ch == "2") ? (!(ch == "3") ? (!(ch == "4") ? (!(ch == "5") ? (!(ch == "6") ? (!(ch == "7") ? (!(ch == "8") ? (!(ch == "9") ? (!(ch == "A") ? (!(ch == "B") ? (!(ch == "C") ? (!(ch == "D") ? (!(ch == "E") ? (!(ch == "F") ? (!(ch == "G") ? (!(ch == "H") ? (!(ch == "I") ? (!(ch == "J") ? (!(ch == "K") ? (!(ch == "L") ? (!(ch == "M") ? (!(ch == "N") ? (!(ch == "O") ? (!(ch == "P") ? (!(ch == "Q") ? (!(ch == "R") ? (!(ch == "S") ? (!(ch == "T") ? (!(ch == "U") ? (!(ch == "V") ? (!(ch == "W") ? (!(ch == "X") ? (!(ch == "Y") ? (!(ch == "Z") ? (!(ch == "$") ? (!(ch == "%") ? (!(ch == " ") ? (!(ch == "-") && num != 189 ? (!(ch == "+") ? (!(ch == ".") && num != 190 ? (!(ch == "/") && num != 191 ? 0 : 42) : 41) : 40) : 39) : 38) : 37) : 36) : 35) : 34) : 33) : 32) : 31) : 30) : 29) : 28) : 27) : 26) : 25) : 24) : 23) : 22) : 21) : 20) : 19) : 18) : 17) : 16) : 15) : 14) : 13) : 12) : 11) : 10) : 9) : 8) : 7) : 6) : 5) : 4) : 3) : 2) : 1) : 0;
    }

    private void ChangeDivision()
    {
      if (this.dgvDistribute.Rows.Count <= 1)
        return;
      if (this.dgvDistribute.Rows.Count == 2)
      {
        this.dgvDistribute.Rows[0].Cells["Summ"].Value = (object) this.txbArValue.Text;
        this.dgvDistribute.Rows[1].Cells["Summ"].Value = (object) this.txbArValue.Text;
      }
      else
      {
        if (!(Convert.ToDecimal(this.txbArValue.Text) != Decimal.Zero) || MessageBox.Show("Разбить суммы по договорам", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
          return;
        this.DivisionSumm((short) 1);
        this.DivisionSumm((short) 2);
      }
    }

    private void ShowCurrentPay(LsClient client)
    {
      if (!Options.ShowOldPays)
        return;
      if (this.frmCurrentPayment == null)
        this.frmCurrentPayment = new FrmCurrentPayment();
      this.frmCurrentPayment.lsClient = client;
      this.frmCurrentPayment.period = KvrplHelper.GetPeriod(Convert.ToDateTime(this.mpPeriod.Text));
      this.frmCurrentPayment.PForm = this;
      if (!this.frmCurrentPayment.IsOpen)
      {
        if (!this.frmCurrentPayment.IsDisposed)
        {
          this.frmCurrentPayment.Show();
        }
        else
        {
          this.frmCurrentPayment = new FrmCurrentPayment();
          this.frmCurrentPayment.lsClient = client;
          this.frmCurrentPayment.period = KvrplHelper.GetPeriod(Convert.ToDateTime(this.mpPeriod.Text));
          this.frmCurrentPayment.PForm = this;
          this.frmCurrentPayment.Show();
        }
      }
      else
      {
        this.frmCurrentPayment.Clear();
        this.frmCurrentPayment.LoadCurrentPayment();
      }
    }

    private void FindSum()
    {
      if (!Options.OfferSum || this.currentLs == null)
        return;
      IList<Period> periodList = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) this.mpPeriodPay.Value)).List<Period>();
      IList list1 = this.session.CreateSQLQuery(this.QueryFindSum("lsBalance", periodList[0])).List();
      if (this.rbKv.Checked)
      {
        if (list1.Count > 0)
          this.txtSum.Text = ((object[]) list1[0])[0].ToString();
        else
          this.txtSum.Text = "";
        IList list2 = this.session.CreateSQLQuery(this.QueryFindSum("lsBalancePeni", periodList[0])).List();
        if (list2.Count > 0)
          this.txtPeni.Text = ((object[]) list2[0])[0].ToString();
        else
          this.txtPeni.Text = "";
        if (list1.Count > 0 && list2.Count > 0)
          this.tbItog.Text = Convert.ToString(Convert.ToDouble(((object[]) list1[0])[0]) + Convert.ToDouble(((object[]) list2[0])[0]));
        else if (list1.Count > 0)
          this.tbItog.Text = this.txtSum.Text;
        else if (list2.Count > 0)
          this.tbItog.Text = this.txtPeni.Text;
        else
          this.tbItog.Text = "";
      }
      if (this.rbAr.Checked)
      {
        if (list1.Count > 0)
          this.txbArValue.Text = ((object[]) list1[0])[0].ToString();
        else
          this.txbArValue.Text = "";
        IList list2 = this.session.CreateSQLQuery(this.QueryFindSum("lsBalancePeni", periodList[0])).List();
        if (list2.Count > 0)
          this.txbArPeni.Text = ((object[]) list2[0])[0].ToString();
        else
          this.txbArPeni.Text = "";
        if (list1.Count > 0 && list2.Count > 0)
          this.txbArItog.Text = Convert.ToString(Convert.ToDouble(((object[]) list1[0])[0]) + Convert.ToDouble(((object[]) list2[0])[0]));
        else if (list1.Count > 0)
          this.txbArItog.Text = this.txbArValue.Text;
        else if (list2.Count > 0)
          this.txbArItog.Text = this.txbArPeni.Text;
        else
          this.txbArItog.Text = "";
      }
    }

    private string QueryFindSum(string tblName, Period periodPay)
    {
      int num = 1;
      if (this.rbKv.Checked)
        num = Convert.ToInt32(this.cmbReceipt.SelectedValue);
      if (this.rbAr.Checked)
        num = Convert.ToInt32(this.cmbArReceipt.SelectedValue);
      return string.Format("select sum(balance_out),(select isnull((select receipt_id from dba.cmphmReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and ls.idhome=sp.idhome and sp.complex_id=ls.complex_id and '{3}' between dbeg and dend and ls.client_id=t.client_id and                                                                                                        (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.supplier_id=t.supplier_id),                                isnull((select receipt_id from dba.cmphmReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and ls.idhome=sp.idhome and sp.complex_id=ls.complex_id and '{3}' between dbeg and dend and ls.client_id=t.client_id and sp.service_id=0 and sp.supplier_id=t.supplier_id),                                isnull((select receipt_id from dba.cmphmReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and ls.idhome=sp.idhome and sp.complex_id=ls.complex_id and '{3}' between dbeg and dend and ls.client_id=t.client_id and                                                                                                         (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.supplier_id=0),                                isnull((select receipt_id from dba.cmphmReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and sp.idhome=0 and sp.complex_id=ls.complex_id and '{3}' between dbeg and dend and ls.client_id=t.client_id and                                                                                                         (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.supplier_id=t.supplier_id),                                isnull((select receipt_id from dba.cmphmReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and sp.idhome=0 and sp.complex_id=ls.complex_id and '{3}' between dbeg and dend and ls.client_id=t.client_id and sp.service_id=0 and sp.supplier_id=t.supplier_id),                                isnull((select receipt_id from dba.cmphmReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and sp.idhome=0 and sp.complex_id=ls.complex_id and '{3}' between dbeg and dend and ls.client_id=t.client_id and                                                                                                          (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.supplier_id=0),                               isnull((select receipt_id from dba.cmpServiceParam sp,dba.lsClient ls where ls.company_id=sp.company_id and ls.client_id=t.client_id and                                                                                                   (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.complex_id=ls.complex_id),                               isnull((if t.service_id=0 and (select count(receipt_id) from dba.cmpReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and ls.client_id=t.client_id)=1 then                                           (select receipt_id from dba.cmpReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and ls.client_id=t.client_id) else 0 endif),                               0))))))))) as receipt_id from " + tblName + " t where period_id={0} and client_id={1} and receipt_id={2}", (object) periodPay.PeriodId, (object) this.currentLs.ClientId, (object) num, (object) KvrplHelper.DateToBaseFormat(periodPay.PeriodName.Value));
    }

    private void UpdatePacket()
    {
      if (this.dtmpDate == null)
        return;
      DateTime dateTime = this.dtmpDate.Value;
      DateTime? periodName = Options.Period.PeriodName;
      if (periodName.HasValue && dateTime < periodName.GetValueOrDefault())
        this.txbPacket.Text = Convert.ToString(Convert.ToInt32(this.cmbSource.SelectedValue) * 100 + this.dtmpDate.Value.Day + 50);
      else
        this.txbPacket.Text = Convert.ToString(Convert.ToInt32(this.cmbSource.SelectedValue) * 100 + this.dtmpDate.Value.Day);
    }

    private void DecodeStrInputScannerProt()
    {
      string str1 = this.strInputScanner.Remove(2, this.strInputScanner.Length - 2);
      if ((!(str1 == "00") && !(str1 == "01") && (!(str1 == "02") && !(str1 == "26")) || this.strInputScanner.Length != 22 && this.strInputScanner.Length != 23) && (!(str1 == "70") && !(str1 == "71") || this.strInputScanner.Length != 18 && this.strInputScanner.Length != 19))
        return;
      int val1 = 0;
      if (str1 == "00" || str1 == "01")
        val1 = 1;
      if (str1 == "02")
        val1 = 2;
      bool flag = false;
      if (str1 == "00" || str1 == "01" || str1 == "02" || str1 == "26")
        flag = true;
      if ((flag && this.strInputScanner.Length == 22 || !flag && this.strInputScanner.Length == 18) && this.strInputScanner.IndexOf('\r') != -1)
        this.strInputScanner = this.strInputScanner.Remove(this.strInputScanner.IndexOf('\r'), 1);
      this.txtClientId.Text = this.lastStrClientId;
      if (this.lastStrClientId != "")
        this.btnSave_Click((object) null, (EventArgs) null);
      this.txtSum.Text = "";
      this.txtPeni.Text = "";
      this.txtStrah.Text = "";
      double num1 = 0.0;
      IList<LsClient> lsClientList1 = (IList<LsClient>) new List<LsClient>();
      if (str1 != "26")
      {
        if (str1 == "02")
        {
          this.cmbPurpose.SelectedValue = (object) Convert.ToInt16(1);
          this.cmbReceipt.SelectedValue = (object) Convert.ToInt16(2);
        }
        else
        {
          this.cmbPurpose.SelectedValue = (object) Convert.ToInt16(1);
          this.cmbReceipt.SelectedValue = (object) Convert.ToInt16(1);
        }
      }
      else
      {
        this.cmbPurpose.SelectedValue = (object) Convert.ToInt16(5);
        this.cmbReceipt.SelectedValue = (object) Convert.ToInt16(1);
      }
      double num2;
      IList<LsClient> lsClientList2;
      if (flag)
      {
        string str2 = this.strInputScanner.Remove(0, 2);
        this.txtClientId.Text = str2.Remove(9, str2.Length - 9);
        string str3 = str2.Remove(0, 9);
        string str4 = str3.Remove(4, str3.Length - 4);
        num2 = Convert.ToDouble(str3.Remove(0, 4)) / 100.0;
        this.txtSum.Text = num2.ToString();
        DateTime dateTime1 = this.mpPeriodPay.Value;
        int int16_1 = (int) Convert.ToInt16(str4.Remove(2, 2));
        int int16_2 = (int) Convert.ToInt16(str4.Remove(0, 2));
        this.mpPeriodPay.OldMonth = 0;
        this.mpPeriodPay.Value = Convert.ToDateTime(string.Format("1.{0}.{1}", (object) int16_1, (object) int16_2));
        MonthPicker mpPeriod = this.mpPeriod;
        string format = "1.{0}.{1}";
        DateTime dateTime2 = this.mpPeriod.Value;
        // ISSUE: variable of a boxed type
        int month = dateTime2.Month;
        dateTime2 = this.mpPeriod.Value;
        // ISSUE: variable of a boxed type
        int year = dateTime2.Year;
        DateTime dateTime3 = Convert.ToDateTime(string.Format(format, (object) month, (object) year));
        mpPeriod.Value = dateTime3;
        this.strInputScanner = "";
        this.lastStrClientId = this.txtClientId.Text;
        this.tbItog.Text = num2.ToString();
        lsClientList2 = this.session.CreateQuery(string.Format("from LsClient where ClientId={0}", (object) this.txtClientId.Text)).List<LsClient>();
      }
      else
      {
        this.strInputScanner.Remove(0, this.strInputScanner.Length - 2);
        string str2 = this.strInputScanner.Remove(this.strInputScanner.Length - 2, 2);
        string str3 = str2.Remove(0, str2.Length - 1);
        string str4 = str2.Remove(str2.Length - 1, 1);
        int num3 = (int) str3[0] + 2000 - 71;
        string str5 = str4.Remove(0, str4.Length - 1);
        string str6 = str4.Remove(str4.Length - 1, 1);
        if (str5 == "A")
          str5 = "10";
        if (str5 == "B")
          str5 = "11";
        if (str5 == "C")
          str5 = "12";
        this.mpPeriodPay.Value = Convert.ToDateTime(string.Format("1.{0}.{1}", (object) str5, (object) num3));
        int int32 = Convert.ToInt32(str6.Remove(0, str6.Length - 6));
        IList<LsClient> lsClientList3 = this.session.CreateQuery(string.Format("from LsClient where OldId={0}", (object) int32)).List<LsClient>();
        this.txtClientId.Text = lsClientList3.Count > 0 ? Convert.ToString(lsClientList3[0].ClientId) : "";
        num2 = Convert.ToDouble(str6.Remove(str6.Length - 6, 6).Remove(0, 2)) / 100.0;
        this.txtSum.Text = num2.ToString();
        this.strInputScanner = "";
        this.lastStrClientId = this.txtClientId.Text;
        lsClientList2 = this.session.CreateQuery(string.Format("from LsClient where OldId={0}", (object) int32)).List<LsClient>();
      }
      if (lsClientList2.Count > 0)
      {
        this.currentLs = lsClientList2[0];
        this.AddAddressFIO();
        if (Options.CountersInPays)
          this.LoadCounters();
        this.ShowCurrentPay(lsClientList2[0]);
      }
      if (str1 != "26")
      {
        if (lsClientList2.Count > 0)
        {
          try
          {
            IList list = this.session.CreateSQLQuery("select service_id from DBA.cmpServiceParam  where company_id=:compId and receipt_id=:recId").SetParameter<short>("compId", this.currentLs.Company.CompanyId).SetParameter<int>("recId", val1).List();
            List<int> intList = new List<int>();
            foreach (object obj1 in (IEnumerable) list)
            {
              foreach (object obj2 in (IEnumerable) this.session.CreateSQLQuery("select service_id from DBA.dcService where root = :ser").SetParameter<string>("ser", obj1.ToString()).List())
                intList.Add(Convert.ToInt32(obj2));
            }
            foreach (int val2 in intList)
              num1 += Math.Round(Convert.ToDouble(this.session.CreateSQLQuery(string.Format("select sum(Rent) from DBA.lsRentPeni where Period_id={0} and client_id={1} and service_id = :ser and code!=-1", (object) KvrplHelper.GetPeriod(Convert.ToDateTime(this.mpPeriodPay.Text)).PeriodId, (object) lsClientList2[0].ClientId)).SetParameter<int>("ser", val2).UniqueResult()), 2);
          }
          catch
          {
          }
        }
        if (num1 > 0.0)
        {
          num2 -= num1;
          this.txtSum.Text = num2.ToString();
          this.txtPeni.Text = num1.ToString();
        }
      }
      if (str1 == "71")
      {
        double num3 = 0.0;
        try
        {
          IList list = this.session.CreateSQLQuery(string.Format("select Param_Value from DBA.LsParam c where c.Param_Id={0} and c.Client_Id={1} and c.Period_Id={2} and c.DBeg <= '{3}' and c.DEnd >= '{4}'  ", (object) 62, (object) (lsClientList2.Count > 0 ? lsClientList2[0].ClientId : 0), (object) 0, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(Convert.ToDateTime(this.mpPeriodPay.Text))), (object) KvrplHelper.DateToBaseFormat(Convert.ToDateTime(this.mpPeriodPay.Text)))).List();
          if (list.Count > 0)
          {
            double num4 = 0.0;
            try
            {
              num4 = Convert.ToDouble(this.session.CreateSQLQuery(string.Format("select Cost from DBA.cmpTariff where Tariff_id = {0} and Company_id = {1} and  Service_Id={2} and Period_Id={3} and DBeg <= '{4}' and DEnd >= '{5}'", (object) Convert.ToInt32(list[0]), (object) lsClientList2[0].Company.CompanyId, (object) 26, (object) 0, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(Convert.ToDateTime(this.mpPeriodPay.Text))), (object) KvrplHelper.DateToBaseFormat(Convert.ToDateTime(this.mpPeriodPay.Text)))).UniqueResult());
            }
            catch
            {
            }
            double num5 = 0.0;
            try
            {
              num5 = Convert.ToDouble(this.session.CreateSQLQuery(string.Format("select Param_Value from DBA.LsParam c where c.Param_Id={0} and c.Client_Id={1} and c.Period_Id={2} and c.DBeg <= '{3}' and c.DEnd >= '{4}'  ", (object) 2, (object) (lsClientList2.Count > 0 ? lsClientList2[0].ClientId : 0), (object) 0, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(Convert.ToDateTime(this.mpPeriodPay.Text))), (object) KvrplHelper.DateToBaseFormat(Convert.ToDateTime(this.mpPeriodPay.Text)))).UniqueResult());
            }
            catch (Exception ex)
            {
              KvrplHelper.WriteLog(ex, (LsClient) null);
            }
            num3 = num4 * num5;
          }
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        if (num3 > 0.0)
        {
          num2 -= num3;
          this.txtSum.Text = num2.ToString();
          this.txtStrah.Text = num3.ToString();
        }
        try
        {
          this.tbItog.Text = (num2 + num1 + num3).ToString();
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        if (num3 > 0.0 && lsClientList2.Count > 0)
        {
          this.StrahPayment = new Payment();
          this.StrahPayment.PaymentId = this.session.CreateSQLQuery("select DBA.gen_id('lsPayment',1)").List<int>()[0];
          this.StrahPayment.LsClient = lsClientList2[0];
          this.StrahPayment.Period = KvrplHelper.GetPeriod(Convert.ToDateTime(this.mpPeriod.Text));
          this.StrahPayment.PeriodPay = KvrplHelper.GetPeriod(Convert.ToDateTime(this.mpPeriodPay.Text));
          this.StrahPayment.Service = this.session.Get<Service>((object) (short) 0);
          this.StrahPayment.Receipt = this.session.Get<Receipt>((object) (short) val1);
          this.StrahPayment.SPay = (SourcePay) this.cmbSource.SelectedItem;
          this.StrahPayment.PPay = this.session.Get<PurposePay>((object) (short) 5);
          this.StrahPayment.PayDoc = (PayDoc) this.cmbPayDoc.SelectedItem;
          this.StrahPayment.PacketNum = this.txbPacket.Text;
          this.StrahPayment.PaymentValue = Convert.ToDecimal(num3);
          this.StrahPayment.PaymentPeni = Decimal.Zero;
          this.StrahPayment.PaymentDate = this.dtmpDate.Value;
          this.StrahPayment.Supplier = this.session.Get<Supplier>((object) 0);
          this.StrahPayment.UName = Options.Login;
          this.StrahPayment.DEdit = DateTime.Now.Date;
        }
      }
    }

    private void DecodeStrInputScannerTut()
    {
      if (this.strInputScanner.Length != 13 && this.strInputScanner.Length != 16)
        return;
      string str1 = this.strInputScanner.Remove(1, this.strInputScanner.Length - 1);
      int num1;
      if (str1 == "M" && this.strInputScanner.Length == 16)
      {
        if (this.strInputScanner.IndexOf('\r') != -1)
          this.strInputScanner = this.strInputScanner.Remove(this.strInputScanner.IndexOf('\r'), 1);
        this.txtClientId.Text = this.lastStrClientId;
        if (this.lastStrClientId != "")
          this.btnSave_Click((object) null, (EventArgs) null);
        this.txtSum.Text = "";
        this.txtPeni.Text = "";
        string str2 = this.strInputScanner.Remove(0, 1);
        this.txtClientId.Text = this.DecodeStrScan(str2.Substring(0, 6), (short) 6).ToString();
        string str3 = str2.Remove(0, 6);
        string str4 = this.DecodeStrScan(str3.Substring(0, 2), (short) 2).ToString();
        string str5 = str3.Remove(0, 2);
        Period period = this.session.Get<Period>((object) Convert.ToInt32(str4));
        this.mpPeriodPay.OldMonth = 0;
        this.mpPeriodPay.Value = period.PeriodName.Value;
        num1 = this.DecodeStrScan(str5.Substring(0, 5), (short) 5);
        string str6 = num1.ToString();
        string str7 = str5.Remove(0, 5);
        double num2 = Convert.ToDouble(str6) / 100.0;
        this.txtSum.Text = num2.ToString();
        if (str7 == "00")
          this.cmbReceipt.SelectedValue = (object) Convert.ToInt16(1);
        else
          this.cmbReceipt.SelectedValue = (object) Convert.ToInt16(str7);
        this.strInputScanner = "";
        this.lastStrClientId = this.txtClientId.Text;
        this.tbItog.Text = num2.ToString();
        IList<LsClient> lsClientList = this.session.CreateQuery(string.Format("from LsClient where ClientId={0}", (object) this.txtClientId.Text)).List<LsClient>();
        if (lsClientList.Count > 0)
        {
          this.currentLs = lsClientList[0];
          this.AddAddressFIO();
          if (Options.CountersInPays)
            this.LoadCounters();
          this.ShowCurrentPay(lsClientList[0]);
        }
      }
      if (str1 == "P" && this.strInputScanner.Length == 13)
      {
        if (this.strInputScanner.IndexOf('\r') != -1)
          this.strInputScanner = this.strInputScanner.Remove(this.strInputScanner.IndexOf('\r'), 1);
        this.Peni = true;
        string str2 = this.strInputScanner.Remove(0, 1);
        TextBox txtClientId = this.txtClientId;
        num1 = this.DecodeStrScan(str2.Substring(0, 6), (short) 6);
        string str3 = num1.ToString();
        txtClientId.Text = str3;
        num1 = this.DecodeStrScan(str2.Substring(0, 6), (short) 6);
        string str4 = num1.ToString();
        string str5 = str2.Remove(0, 6);
        if ((this.lastStrClientId != str4 || this.lastStrClientId == "") && this.Peni)
        {
          this.txtClientId.Text = "";
          int num2 = (int) MessageBox.Show("Введите сначала основной платеж, а затем платеж по пеням!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.strInputScanner = "";
        }
        else
        {
          num1 = this.DecodeStrScan(str5.Substring(0, 5), (short) 5);
          string str6 = num1.ToString();
          string str7 = str5.Remove(0, 5);
          double num2 = 1.0;
          if (str7.Substring(0, 1) == "M")
            num2 = -1.0;
          double num3 = num2 * Convert.ToDouble(str6) / 100.0;
          this.txtPeni.Text = num3.ToString();
          try
          {
            this.tbItog.Text = this.txtSum.Text != "" ? (Convert.ToDouble(this.txtSum.Text) + num3).ToString() : num3.ToString();
          }
          catch
          {
          }
          this.strInputScanner = "";
        }
      }
    }

    private void txtClientId_TextChanged(object sender, EventArgs e)
    {
      if (!Options.Scanner)
        return;
      if (this.lastStrClientId != "" && !this.EndChangedScanner)
      {
        this.txtClientId.Text = this.txtClientId.Text.Replace(this.lastStrClientId, "");
        this.txtClientId.SelectionStart = 1;
        this.EndChangedScanner = true;
      }
      this.strInputScanner = this.txtClientId.Text;
      if (this.strInputScanner.IndexOf('\r') != -1)
        this.strInputScanner = this.strInputScanner.Remove(this.strInputScanner.IndexOf('\r'), 1);
      if (this.strInputScanner.IndexOf(Convert.ToChar(16)) != -1)
        this.strInputScanner = this.strInputScanner.Remove(this.strInputScanner.IndexOf(Convert.ToChar(16)), 1);
      if ((this.city == 1 || this.city == 5 || this.city == 15) && this.strInputScanner.Length >= 18)
      {
        Options.Scanner = false;
        this.DecodeStrInputScanner();
        Options.Scanner = true;
      }
      if (this.city == 4)
      {
        if (this.strInputScanner.Length >= 22)
        {
          Options.Scanner = false;
          this.DecodeStrInputScannerProt();
          Options.Scanner = true;
        }
        if (this.strInputScanner.Length >= 18)
        {
          Options.Scanner = false;
          this.DecodeStrInputScannerProt();
          Options.Scanner = true;
        }
      }
      if (this.city == 6)
      {
        if (this.strInputScanner.Length >= 18 || this.strInputScanner.Length >= 18 && this.strInputScanner.Substring(0, 2) == "99")
        {
          Options.Scanner = false;
          this.DecodeStrInputScanner();
          Options.Scanner = true;
        }
        if (this.strInputScanner.Length >= 13)
        {
          Options.Scanner = false;
          this.DecodeStrInputScannerTut();
          Options.Scanner = true;
        }
      }
    }

    private void cmbService_SelectedIndexChanged(object sender, EventArgs e)
    {
      Service selectedItem = (Service) this.cmbService.SelectedItem;
      if (selectedItem != null)
        this._serviceId = (int) selectedItem.ServiceId;
      this.loadRecipientCB();
      this.loadPerfomerCB();
    }

    private void loadRecipientCB()
    {
      string str = "";
      if ((uint) this._serviceId > 0U)
        str = string.Format(" and (s.Service.ServiceId={0} or s.Service.ServiceId=(select Root from Service where ServiceId={0}))", (object) this._serviceId);
      IList<BaseOrg> baseOrgList1 = (IList<BaseOrg>) new List<BaseOrg>();
      if ((int) this._company.CompanyId == 0)
      {
        IList<BaseOrg> baseOrgList2 = this.session.CreateQuery(string.Format("select distinct new BaseOrg(b.Recipient.BaseOrgId,b.Recipient.NameOrgMin) from Supplier b where b.SupplierId in (select SupplierOrg.SupplierId from CmpSupplier) and b.Recipient.BaseOrgId<>0 order by b.Recipient.NameOrgMin")).List<BaseOrg>();
        baseOrgList2.Insert(0, new BaseOrg(0, "Не определено"));
        this.cmbRecipient.DataSource = (object) baseOrgList2;
        this.cmbRecipient.ValueMember = "BaseOrgId";
        this.cmbRecipient.DisplayMember = "NameOrgMin";
        this.cmbRecipient.SelectedIndex = 0;
      }
      else
      {
        IList<BaseOrg> baseOrgList2 = this.session.CreateQuery(string.Format("Select distinct d.Recipient from CmpSupplier s,Supplier d where s.SupplierOrg.SupplierId=d.SupplierId and d.Recipient.BaseOrgId<>0 and s.Company.CompanyId=isnull((select ParamValue from CompanyParam where Period.PeriodId=0 and Company.CompanyId={0} and DBeg<='{2}' and DEnd>='{2}' and Param.ParamId=211),0) " + str + " order by d.Recipient.NameOrgMin", (object) this._company.CompanyId, (object) this._serviceId, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.GetCmpKvrClose(this._company, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk).PeriodName.Value.AddMonths(1)))).List<BaseOrg>();
        baseOrgList2.Insert(0, new BaseOrg(0, "Не определено"));
        this.cmbRecipient.DataSource = (object) baseOrgList2;
        this.cmbRecipient.ValueMember = "BaseOrgId";
        this.cmbRecipient.DisplayMember = "NameOrgMin";
      }
    }

    private void loadPerfomerCB()
    {
      string str = "";
      if ((uint) this._serviceId > 0U)
        str = string.Format(" and (s.Service.ServiceId={0} or s.Service.ServiceId=(select Root from Service where ServiceId={0}))", (object) this._serviceId);
      IList<BaseOrg> baseOrgList1 = (IList<BaseOrg>) new List<BaseOrg>();
      if ((int) this._company.CompanyId == 0)
      {
        IList<BaseOrg> baseOrgList2 = this.session.CreateQuery(string.Format("select distinct new BaseOrg(b.Perfomer.BaseOrgId,b.Perfomer.NameOrgMin) from Supplier b where b.SupplierId in (select SupplierOrg.SupplierId from CmpSupplier) and b.Recipient.BaseOrgId={0} and b.Perfomer.BaseOrgId<>0 order by b.Perfomer.NameOrgMin", (object) Convert.ToInt32(this.cmbRecipient.SelectedValue))).List<BaseOrg>();
        baseOrgList2.Insert(0, new BaseOrg(0, "Не определено"));
        this.cmbPerfomer.DataSource = (object) baseOrgList2;
        this.cmbPerfomer.DisplayMember = "NameOrgMin";
        this.cmbPerfomer.ValueMember = "BaseOrgId";
        this.cmbPerfomer.SelectedIndex = 0;
      }
      else
      {
        IList<BaseOrg> baseOrgList2 = this.session.CreateQuery(string.Format("Select distinct d.Perfomer from CmpSupplier s,Supplier d where s.SupplierOrg.SupplierId=d.SupplierId  and d.Perfomer.BaseOrgId<>0 and s.Company.CompanyId=isnull((select ParamValue from CompanyParam where Period.PeriodId=0 and Company.CompanyId={0} and DBeg<='{2}' and DEnd>='{2}'and Param.ParamId=211),0) " + str + " order by d.Perfomer.NameOrgMin", (object) this._company.CompanyId, (object) this._serviceId, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.GetCmpKvrClose(this._company, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk).PeriodName.Value.AddMonths(1)))).List<BaseOrg>();
        baseOrgList2.Insert(0, new BaseOrg(0, "Не определено"));
        this.cmbPerfomer.DataSource = (object) baseOrgList2;
        this.cmbPerfomer.DisplayMember = "NameOrgMin";
        this.cmbPerfomer.ValueMember = "BaseOrgId";
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
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmAttribute));
      this.pnBtn = new Panel();
      this.btnFind = new Button();
      this.btnSave = new Button();
      this.btnExit = new Button();
      this.gbAttribute = new GroupBox();
      this.txbPacket = new TextBox();
      this.cmbPayDoc = new ComboBox();
      this.lblPayDoc = new Label();
      this.chbScanner = new CheckBox();
      this.mpPeriodPay = new MonthPicker();
      this.mpPeriod = new MonthPicker();
      this.lblPacket = new Label();
      this.lblPeriodPay = new Label();
      this.dtmpDate = new DateTimePicker();
      this.lblDate = new Label();
      this.cmbPurpose = new ComboBox();
      this.lblPurpose = new Label();
      this.cmbSource = new ComboBox();
      this.lblSource = new Label();
      this.lblPeriod = new Label();
      this.gbVid = new GroupBox();
      this.rbKv = new RadioButton();
      this.rbAr = new RadioButton();
      this.tmr1 = new Timer(this.components);
      this.hp = new HelpProvider();
      this.pnKv = new Panel();
      this.gbCounters = new GroupBox();
      this.dgvEvidence = new DataGridView();
      this.pnEvidence = new Panel();
      this.btnLoadCounters = new Button();
      this.pnMain = new Panel();
      this.cmbRecipientAuto = new ComboBox();
      this.lblRecipientAuto = new Label();
      this.cmbPerfomer = new ComboBox();
      this.lblPerfomer = new Label();
      this.cmbRecipient = new ComboBox();
      this.lblRecipient = new Label();
      this.cmbSupplier = new ComboBox();
      this.lblSupplier = new Label();
      this.lblFIO = new Label();
      this.tbItog = new TextBox();
      this.lblItog = new Label();
      this.txtStrah = new TextBox();
      this.lblStrah = new Label();
      this.txtClientId = new TextBox();
      this.lblReceipt = new Label();
      this.cmbReceipt = new ComboBox();
      this.cmbService = new ComboBox();
      this.lblService = new Label();
      this.txtPeni = new TextBox();
      this.lblPeni = new Label();
      this.txtSum = new TextBox();
      this.lblValue = new Label();
      this.lblIdlic = new Label();
      this.pnArenda = new Panel();
      this.lbOrg = new ListBox();
      this.cmbArRecipientAuto = new ComboBox();
      this.label1 = new Label();
      this.cmbArPerfomer = new ComboBox();
      this.lblArPerfomer = new Label();
      this.cmbArRecipient = new ComboBox();
      this.lblArRecipient = new Label();
      this.dgvDistribute = new DataGridView();
      this.gbArrange = new GroupBox();
      this.rbRent = new RadioButton();
      this.rbBalance = new RadioButton();
      this.txbNumber = new TextBox();
      this.lblAr = new Label();
      this.cmbDog = new ComboBox();
      this.lblNum = new Label();
      this.lbDog = new ListBox();
      this.lblOrg = new Label();
      this.cmbO = new ComboBox();
      this.cmbArSupplier = new ComboBox();
      this.lblArSupplier = new Label();
      this.txbArItog = new TextBox();
      this.lblArItog = new Label();
      this.lblArReceipt = new Label();
      this.cmbArReceipt = new ComboBox();
      this.cmbArService = new ComboBox();
      this.lblArService = new Label();
      this.txbArPeni = new TextBox();
      this.lblArPeni = new Label();
      this.txbArValue = new TextBox();
      this.lblArValue = new Label();
      this.lblNumber = new Label();
      this.lblArClient = new Label();
      this.txbArClient = new TextBox();
      this.pnBtn.SuspendLayout();
      this.gbAttribute.SuspendLayout();
      this.gbVid.SuspendLayout();
      this.pnKv.SuspendLayout();
      this.gbCounters.SuspendLayout();
      ((ISupportInitialize) this.dgvEvidence).BeginInit();
      this.pnEvidence.SuspendLayout();
      this.pnMain.SuspendLayout();
      this.pnArenda.SuspendLayout();
      ((ISupportInitialize) this.dgvDistribute).BeginInit();
      this.gbArrange.SuspendLayout();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnFind);
      this.pnBtn.Controls.Add((Control) this.btnSave);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 722);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(970, 40);
      this.pnBtn.TabIndex = 1;
      this.btnFind.Image = (Image) Resources.search_24;
      this.btnFind.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnFind.Location = new Point((int) sbyte.MaxValue, 5);
      this.btnFind.Name = "btnFind";
      this.btnFind.Size = new Size(153, 30);
      this.btnFind.TabIndex = 2;
      this.btnFind.Text = "Найти по адресу";
      this.btnFind.TextAlign = ContentAlignment.MiddleRight;
      this.btnFind.UseVisualStyleBackColor = true;
      this.btnFind.Click += new EventHandler(this.btnFind_Click);
      this.btnSave.Image = (Image) Resources.Tick;
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.Location = new Point(12, 5);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(109, 30);
      this.btnSave.TabIndex = 0;
      this.btnSave.Text = "Сохранить";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(885, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(75, 30);
      this.btnExit.TabIndex = 1;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.gbAttribute.Controls.Add((Control) this.txbPacket);
      this.gbAttribute.Controls.Add((Control) this.cmbPayDoc);
      this.gbAttribute.Controls.Add((Control) this.lblPayDoc);
      this.gbAttribute.Controls.Add((Control) this.chbScanner);
      this.gbAttribute.Controls.Add((Control) this.mpPeriodPay);
      this.gbAttribute.Controls.Add((Control) this.mpPeriod);
      this.gbAttribute.Controls.Add((Control) this.lblPacket);
      this.gbAttribute.Controls.Add((Control) this.lblPeriodPay);
      this.gbAttribute.Controls.Add((Control) this.dtmpDate);
      this.gbAttribute.Controls.Add((Control) this.lblDate);
      this.gbAttribute.Controls.Add((Control) this.cmbPurpose);
      this.gbAttribute.Controls.Add((Control) this.lblPurpose);
      this.gbAttribute.Controls.Add((Control) this.cmbSource);
      this.gbAttribute.Controls.Add((Control) this.lblSource);
      this.gbAttribute.Controls.Add((Control) this.lblPeriod);
      this.gbAttribute.Dock = DockStyle.Top;
      this.gbAttribute.Location = new Point(0, 44);
      this.gbAttribute.Name = "gbAttribute";
      this.gbAttribute.Size = new Size(970, 215);
      this.gbAttribute.TabIndex = 1;
      this.gbAttribute.TabStop = false;
      this.gbAttribute.Text = "Атрибуты";
      this.txbPacket.Location = new Point(296, 83);
      this.txbPacket.Name = "txbPacket";
      this.txbPacket.Size = new Size(100, 22);
      this.txbPacket.TabIndex = 4;
      this.txbPacket.KeyDown += new KeyEventHandler(this.txbPacket_KeyDown);
      this.txbPacket.KeyPress += new KeyPressEventHandler(this.txbPacket_KeyPress);
      this.cmbPayDoc.FormattingEnabled = true;
      this.cmbPayDoc.Location = new Point(296, 181);
      this.cmbPayDoc.Name = "cmbPayDoc";
      this.cmbPayDoc.Size = new Size(265, 24);
      this.cmbPayDoc.TabIndex = 6;
      this.cmbPayDoc.SelectionChangeCommitted += new EventHandler(this.cmbPayDoc_SelectionChangeCommitted);
      this.cmbPayDoc.KeyDown += new KeyEventHandler(this.cmbPayDoc_KeyDown);
      this.lblPayDoc.AutoSize = true;
      this.lblPayDoc.Location = new Point(293, 162);
      this.lblPayDoc.Name = "lblPayDoc";
      this.lblPayDoc.Size = new Size(188, 16);
      this.lblPayDoc.TabIndex = 26;
      this.lblPayDoc.Text = "Тип платежного документа";
      this.chbScanner.AutoSize = true;
      this.chbScanner.Location = new Point(10, 172);
      this.chbScanner.Name = "chbScanner";
      this.chbScanner.Size = new Size(202, 20);
      this.chbScanner.TabIndex = 30;
      this.chbScanner.Text = "Ввод сканером штрих-кода";
      this.chbScanner.UseVisualStyleBackColor = true;
      this.chbScanner.CheckedChanged += new EventHandler(this.chbScanner_CheckedChanged);
      this.mpPeriodPay.CustomFormat = "MMMM yyyy";
      this.mpPeriodPay.Format = DateTimePickerFormat.Custom;
      this.mpPeriodPay.Location = new Point(10, 83);
      this.mpPeriodPay.MaxDate = new DateTime(2999, 12, 31, 0, 0, 0, 0);
      this.mpPeriodPay.Name = "mpPeriodPay";
      this.mpPeriodPay.OldMonth = 0;
      this.mpPeriodPay.ShowUpDown = true;
      this.mpPeriodPay.Size = new Size(140, 22);
      this.mpPeriodPay.TabIndex = 1;
      this.mpPeriodPay.ValueChanged += new EventHandler(this.mpPeriodPay_ValueChanged);
      this.mpPeriodPay.KeyDown += new KeyEventHandler(this.mpPeriodPay_KeyDown);
      this.mpPeriod.CustomFormat = "MMMM yyyy";
      this.mpPeriod.Format = DateTimePickerFormat.Custom;
      this.mpPeriod.Location = new Point(10, 36);
      this.mpPeriod.MaxDate = new DateTime(2999, 12, 31, 0, 0, 0, 0);
      this.mpPeriod.Name = "mpPeriod";
      this.mpPeriod.OldMonth = 0;
      this.mpPeriod.ShowUpDown = true;
      this.mpPeriod.Size = new Size(140, 22);
      this.mpPeriod.TabIndex = 0;
      this.mpPeriod.ValueChanged += new EventHandler(this.mpPeriod_ValueChanged);
      this.mpPeriod.KeyDown += new KeyEventHandler(this.mpPeriod_KeyDown);
      this.lblPacket.AutoSize = true;
      this.lblPacket.Location = new Point(293, 63);
      this.lblPacket.Name = "lblPacket";
      this.lblPacket.Size = new Size(49, 16);
      this.lblPacket.TabIndex = 23;
      this.lblPacket.Text = "Пачка";
      this.lblPeriodPay.AutoSize = true;
      this.lblPeriodPay.Location = new Point(7, 64);
      this.lblPeriodPay.Name = "lblPeriodPay";
      this.lblPeriodPay.Size = new Size(77, 16);
      this.lblPeriodPay.TabIndex = 19;
      this.lblPeriodPay.Text = "Платеж за";
      this.dtmpDate.AllowDrop = true;
      this.dtmpDate.Location = new Point(10, (int) sbyte.MaxValue);
      this.dtmpDate.MaxDate = new DateTime(2999, 12, 31, 0, 0, 0, 0);
      this.dtmpDate.Name = "dtmpDate";
      this.dtmpDate.Size = new Size(200, 22);
      this.dtmpDate.TabIndex = 2;
      this.dtmpDate.ValueChanged += new EventHandler(this.dtmpDate_ValueChanged);
      this.dtmpDate.KeyDown += new KeyEventHandler(this.dtmpDate_KeyDown);
      this.lblDate.AutoSize = true;
      this.lblDate.Location = new Point(7, 108);
      this.lblDate.Name = "lblDate";
      this.lblDate.Size = new Size(91, 16);
      this.lblDate.TabIndex = 17;
      this.lblDate.Text = "Дата оплаты";
      this.cmbPurpose.FormattingEnabled = true;
      this.cmbPurpose.Location = new Point(296, 129);
      this.cmbPurpose.Name = "cmbPurpose";
      this.cmbPurpose.Size = new Size(265, 24);
      this.cmbPurpose.TabIndex = 5;
      this.cmbPurpose.SelectionChangeCommitted += new EventHandler(this.cmbPurpose_SelectionChangeCommitted_1);
      this.cmbPurpose.KeyDown += new KeyEventHandler(this.cmbPurpose_KeyDown);
      this.lblPurpose.AutoSize = true;
      this.lblPurpose.Location = new Point(293, 110);
      this.lblPurpose.Name = "lblPurpose";
      this.lblPurpose.Size = new Size(149, 16);
      this.lblPurpose.TabIndex = 15;
      this.lblPurpose.Text = "Назначение платежа";
      this.cmbSource.AutoCompleteMode = AutoCompleteMode.Append;
      this.cmbSource.AutoCompleteSource = AutoCompleteSource.CustomSource;
      this.cmbSource.FormattingEnabled = true;
      this.cmbSource.Location = new Point(296, 36);
      this.cmbSource.Name = "cmbSource";
      this.cmbSource.Size = new Size(265, 24);
      this.cmbSource.TabIndex = 3;
      this.cmbSource.SelectionChangeCommitted += new EventHandler(this.cmbSource_SelectionChangeCommitted_1);
      this.cmbSource.KeyDown += new KeyEventHandler(this.cmbSource_KeyDown);
      this.lblSource.AutoSize = true;
      this.lblSource.Location = new Point(293, 17);
      this.lblSource.Name = "lblSource";
      this.lblSource.Size = new Size(71, 16);
      this.lblSource.TabIndex = 13;
      this.lblSource.Text = "Источник";
      this.lblPeriod.AutoSize = true;
      this.lblPeriod.Location = new Point(7, 18);
      this.lblPeriod.Margin = new Padding(4, 0, 4, 0);
      this.lblPeriod.Name = "lblPeriod";
      this.lblPeriod.Size = new Size(58, 16);
      this.lblPeriod.TabIndex = 12;
      this.lblPeriod.Text = "Период";
      this.gbVid.Controls.Add((Control) this.rbKv);
      this.gbVid.Controls.Add((Control) this.rbAr);
      this.gbVid.Dock = DockStyle.Top;
      this.gbVid.Location = new Point(0, 0);
      this.gbVid.Name = "gbVid";
      this.gbVid.Size = new Size(970, 44);
      this.gbVid.TabIndex = 25;
      this.gbVid.TabStop = false;
      this.gbVid.Tag = (object) "0";
      this.gbVid.Text = "Вид платежа";
      this.rbKv.AutoSize = true;
      this.rbKv.Checked = true;
      this.rbKv.Location = new Point(15, 16);
      this.rbKv.Name = "rbKv";
      this.rbKv.Size = new Size(104, 20);
      this.rbKv.TabIndex = 30;
      this.rbKv.TabStop = true;
      this.rbKv.Text = "Квартплата";
      this.rbKv.UseVisualStyleBackColor = true;
      this.rbAr.AutoSize = true;
      this.rbAr.Location = new Point(184, 16);
      this.rbAr.Name = "rbAr";
      this.rbAr.Size = new Size(75, 20);
      this.rbAr.TabIndex = 30;
      this.rbAr.Text = "Аренда";
      this.rbAr.UseVisualStyleBackColor = true;
      this.rbAr.CheckedChanged += new EventHandler(this.rbAr_CheckedChanged);
      this.tmr1.Interval = 10;
      this.tmr1.Tick += new EventHandler(this.tmr1_Tick);
      this.hp.HelpNamespace = "Help.chm";
      this.pnKv.Controls.Add((Control) this.gbCounters);
      this.pnKv.Controls.Add((Control) this.pnMain);
      this.pnKv.Dock = DockStyle.Fill;
      this.pnKv.Location = new Point(0, 259);
      this.pnKv.Name = "pnKv";
      this.pnKv.Size = new Size(970, 463);
      this.pnKv.TabIndex = 21;
      this.gbCounters.Controls.Add((Control) this.dgvEvidence);
      this.gbCounters.Controls.Add((Control) this.pnEvidence);
      this.gbCounters.Dock = DockStyle.Fill;
      this.gbCounters.Location = new Point(0, 193);
      this.gbCounters.Name = "gbCounters";
      this.gbCounters.Size = new Size(970, 270);
      this.gbCounters.TabIndex = 23;
      this.gbCounters.TabStop = false;
      this.gbCounters.Text = "Счетчики";
      this.dgvEvidence.BackgroundColor = Color.AliceBlue;
      this.dgvEvidence.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
      this.dgvEvidence.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvEvidence.Dock = DockStyle.Fill;
      this.dgvEvidence.GridColor = SystemColors.ControlDarkDark;
      this.dgvEvidence.Location = new Point(3, 18);
      this.dgvEvidence.Name = "dgvEvidence";
      this.dgvEvidence.Size = new Size(964, 209);
      this.dgvEvidence.TabIndex = 1;
      this.dgvEvidence.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvEvidence_CellBeginEdit);
      this.dgvEvidence.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvEvidence_CellFormatting);
      this.dgvEvidence.DataError += new DataGridViewDataErrorEventHandler(this.dgvEvidence_DataError);
      this.pnEvidence.Controls.Add((Control) this.btnLoadCounters);
      this.pnEvidence.Dock = DockStyle.Bottom;
      this.pnEvidence.Location = new Point(3, 227);
      this.pnEvidence.Name = "pnEvidence";
      this.pnEvidence.Size = new Size(964, 40);
      this.pnEvidence.TabIndex = 20;
      this.pnEvidence.Visible = false;
      this.btnLoadCounters.Image = (Image) Resources.DateTime;
      this.btnLoadCounters.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnLoadCounters.Location = new Point(9, 3);
      this.btnLoadCounters.Name = "btnLoadCounters";
      this.btnLoadCounters.Size = new Size(141, 30);
      this.btnLoadCounters.TabIndex = 22;
      this.btnLoadCounters.Text = "Взять счетчики";
      this.btnLoadCounters.TextAlign = ContentAlignment.MiddleRight;
      this.btnLoadCounters.UseVisualStyleBackColor = true;
      this.btnLoadCounters.Click += new EventHandler(this.btnLoadCounters_Click);
      this.pnMain.Controls.Add((Control) this.cmbRecipientAuto);
      this.pnMain.Controls.Add((Control) this.lblRecipientAuto);
      this.pnMain.Controls.Add((Control) this.cmbPerfomer);
      this.pnMain.Controls.Add((Control) this.lblPerfomer);
      this.pnMain.Controls.Add((Control) this.cmbRecipient);
      this.pnMain.Controls.Add((Control) this.lblRecipient);
      this.pnMain.Controls.Add((Control) this.cmbSupplier);
      this.pnMain.Controls.Add((Control) this.lblSupplier);
      this.pnMain.Controls.Add((Control) this.lblFIO);
      this.pnMain.Controls.Add((Control) this.tbItog);
      this.pnMain.Controls.Add((Control) this.lblItog);
      this.pnMain.Controls.Add((Control) this.txtStrah);
      this.pnMain.Controls.Add((Control) this.lblStrah);
      this.pnMain.Controls.Add((Control) this.txtClientId);
      this.pnMain.Controls.Add((Control) this.lblReceipt);
      this.pnMain.Controls.Add((Control) this.cmbReceipt);
      this.pnMain.Controls.Add((Control) this.cmbService);
      this.pnMain.Controls.Add((Control) this.lblService);
      this.pnMain.Controls.Add((Control) this.txtPeni);
      this.pnMain.Controls.Add((Control) this.lblPeni);
      this.pnMain.Controls.Add((Control) this.txtSum);
      this.pnMain.Controls.Add((Control) this.lblValue);
      this.pnMain.Controls.Add((Control) this.lblIdlic);
      this.pnMain.Dock = DockStyle.Top;
      this.pnMain.Location = new Point(0, 0);
      this.pnMain.Name = "pnMain";
      this.pnMain.Size = new Size(970, 193);
      this.pnMain.TabIndex = 0;
      this.cmbRecipientAuto.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbRecipientAuto.FormattingEnabled = true;
      this.cmbRecipientAuto.Location = new Point(632, 20);
      this.cmbRecipientAuto.Name = "cmbRecipientAuto";
      this.cmbRecipientAuto.Size = new Size(331, 24);
      this.cmbRecipientAuto.TabIndex = 45;
      this.lblRecipientAuto.AutoSize = true;
      this.lblRecipientAuto.Location = new Point(629, 4);
      this.lblRecipientAuto.Name = "lblRecipientAuto";
      this.lblRecipientAuto.Size = new Size(192, 16);
      this.lblRecipientAuto.TabIndex = 46;
      this.lblRecipientAuto.Text = "Поставщик автоматический";
      this.cmbPerfomer.FormattingEnabled = true;
      this.cmbPerfomer.Location = new Point(375, 112);
      this.cmbPerfomer.Name = "cmbPerfomer";
      this.cmbPerfomer.Size = new Size(186, 24);
      this.cmbPerfomer.TabIndex = 43;
      this.lblPerfomer.AutoSize = true;
      this.lblPerfomer.Location = new Point(372, 94);
      this.lblPerfomer.Name = "lblPerfomer";
      this.lblPerfomer.Size = new Size(95, 16);
      this.lblPerfomer.TabIndex = 44;
      this.lblPerfomer.Text = "Исполнитель";
      this.cmbRecipient.FormattingEnabled = true;
      this.cmbRecipient.Location = new Point(181, 112);
      this.cmbRecipient.Name = "cmbRecipient";
      this.cmbRecipient.Size = new Size(188, 24);
      this.cmbRecipient.TabIndex = 41;
      this.cmbRecipient.SelectionChangeCommitted += new EventHandler(this.cmbRecipient_SelectionChangeCommitted);
      this.lblRecipient.AutoSize = true;
      this.lblRecipient.Location = new Point(178, 94);
      this.lblRecipient.Name = "lblRecipient";
      this.lblRecipient.Size = new Size(88, 16);
      this.lblRecipient.TabIndex = 42;
      this.lblRecipient.Text = "Получатель";
      this.cmbSupplier.FormattingEnabled = true;
      this.cmbSupplier.Location = new Point(296, 21);
      this.cmbSupplier.Name = "cmbSupplier";
      this.cmbSupplier.Size = new Size(144, 24);
      this.cmbSupplier.TabIndex = 3;
      this.cmbSupplier.Visible = false;
      this.cmbSupplier.SelectionChangeCommitted += new EventHandler(this.cmbSupplier_SelectionChangeCommitted);
      this.lblSupplier.AutoSize = true;
      this.lblSupplier.Location = new Point(293, 4);
      this.lblSupplier.Name = "lblSupplier";
      this.lblSupplier.Size = new Size(80, 16);
      this.lblSupplier.TabIndex = 40;
      this.lblSupplier.Text = "Поставщик";
      this.lblSupplier.Visible = false;
      this.lblFIO.AutoSize = true;
      this.lblFIO.Location = new Point(129, 23);
      this.lblFIO.Name = "lblFIO";
      this.lblFIO.Size = new Size(0, 16);
      this.lblFIO.TabIndex = 39;
      this.tbItog.Enabled = false;
      this.tbItog.Location = new Point(457, 158);
      this.tbItog.Name = "tbItog";
      this.tbItog.Size = new Size(97, 22);
      this.tbItog.TabIndex = 38;
      this.lblItog.AutoSize = true;
      this.lblItog.Location = new Point(454, 139);
      this.lblItog.Name = "lblItog";
      this.lblItog.Size = new Size(50, 16);
      this.lblItog.TabIndex = 37;
      this.lblItog.Text = "Итого:";
      this.txtStrah.Enabled = false;
      this.txtStrah.Location = new Point(298, 158);
      this.txtStrah.Name = "txtStrah";
      this.txtStrah.Size = new Size(100, 22);
      this.txtStrah.TabIndex = 35;
      this.lblStrah.AutoSize = true;
      this.lblStrah.Location = new Point(295, 139);
      this.lblStrah.Name = "lblStrah";
      this.lblStrah.Size = new Size(77, 16);
      this.lblStrah.TabIndex = 36;
      this.lblStrah.Text = "Страховка";
      this.txtClientId.Location = new Point(8, 23);
      this.txtClientId.Name = "txtClientId";
      this.txtClientId.Size = new Size(100, 22);
      this.txtClientId.TabIndex = 0;
      this.txtClientId.TextChanged += new EventHandler(this.txtClientId_TextChanged);
      this.txtClientId.KeyDown += new KeyEventHandler(this.txtClientId_KeyDown);
      this.txtClientId.KeyUp += new KeyEventHandler(this.txtClientId_KeyUp);
      this.lblReceipt.AutoSize = true;
      this.lblReceipt.Location = new Point(7, 48);
      this.lblReceipt.Name = "lblReceipt";
      this.lblReceipt.Size = new Size(273, 16);
      this.lblReceipt.TabIndex = 34;
      this.lblReceipt.Text = "Квитанция, по которой поступил платеж";
      this.cmbReceipt.FormattingEnabled = true;
      this.cmbReceipt.Location = new Point(8, 67);
      this.cmbReceipt.Name = "cmbReceipt";
      this.cmbReceipt.Size = new Size(265, 24);
      this.cmbReceipt.TabIndex = 1;
      this.cmbReceipt.SelectionChangeCommitted += new EventHandler(this.cmbReceipt_SelectionChangeCommitted);
      this.cmbReceipt.KeyDown += new KeyEventHandler(this.cmbReceipt_KeyDown);
      this.cmbService.Enabled = false;
      this.cmbService.FormattingEnabled = true;
      this.cmbService.Location = new Point(10, 112);
      this.cmbService.Name = "cmbService";
      this.cmbService.Size = new Size(154, 24);
      this.cmbService.TabIndex = 2;
      this.cmbService.SelectedIndexChanged += new EventHandler(this.cmbService_SelectedIndexChanged);
      this.cmbService.SelectionChangeCommitted += new EventHandler(this.cmbService_SelectionChangeCommitted);
      this.cmbService.KeyDown += new KeyEventHandler(this.cmbService_KeyDown);
      this.lblService.AutoSize = true;
      this.lblService.Location = new Point(7, 94);
      this.lblService.Name = "lblService";
      this.lblService.Size = new Size(54, 16);
      this.lblService.TabIndex = 33;
      this.lblService.Text = "Услуга";
      this.txtPeni.Location = new Point(114, 158);
      this.txtPeni.Name = "txtPeni";
      this.txtPeni.Size = new Size(100, 22);
      this.txtPeni.TabIndex = 5;
      this.txtPeni.KeyDown += new KeyEventHandler(this.txtPeni_KeyDown);
      this.txtPeni.KeyPress += new KeyPressEventHandler(this.txbArValue_KeyPress);
      this.txtPeni.Leave += new EventHandler(this.txtPeni_Leave);
      this.lblPeni.AutoSize = true;
      this.lblPeni.Location = new Point(113, 139);
      this.lblPeni.Name = "lblPeni";
      this.lblPeni.Size = new Size(42, 16);
      this.lblPeni.TabIndex = 32;
      this.lblPeni.Text = "Пени";
      this.txtSum.Location = new Point(10, 158);
      this.txtSum.Name = "txtSum";
      this.txtSum.Size = new Size(100, 22);
      this.txtSum.TabIndex = 4;
      this.txtSum.KeyDown += new KeyEventHandler(this.txtValue_KeyDown);
      this.txtSum.KeyPress += new KeyPressEventHandler(this.txbArValue_KeyPress);
      this.txtSum.Leave += new EventHandler(this.txtValue_Leave);
      this.lblValue.AutoSize = true;
      this.lblValue.Location = new Point(7, 139);
      this.lblValue.Name = "lblValue";
      this.lblValue.Size = new Size(51, 16);
      this.lblValue.TabIndex = 31;
      this.lblValue.Text = "Сумма";
      this.lblIdlic.AutoSize = true;
      this.lblIdlic.Location = new Point(5, 4);
      this.lblIdlic.Name = "lblIdlic";
      this.lblIdlic.Size = new Size(65, 16);
      this.lblIdlic.TabIndex = 30;
      this.lblIdlic.Text = "Лицевой";
      this.pnArenda.Controls.Add((Control) this.lbOrg);
      this.pnArenda.Controls.Add((Control) this.cmbArRecipientAuto);
      this.pnArenda.Controls.Add((Control) this.label1);
      this.pnArenda.Controls.Add((Control) this.cmbArPerfomer);
      this.pnArenda.Controls.Add((Control) this.lblArPerfomer);
      this.pnArenda.Controls.Add((Control) this.cmbArRecipient);
      this.pnArenda.Controls.Add((Control) this.lblArRecipient);
      this.pnArenda.Controls.Add((Control) this.dgvDistribute);
      this.pnArenda.Controls.Add((Control) this.gbArrange);
      this.pnArenda.Controls.Add((Control) this.txbNumber);
      this.pnArenda.Controls.Add((Control) this.lblAr);
      this.pnArenda.Controls.Add((Control) this.cmbDog);
      this.pnArenda.Controls.Add((Control) this.lblNum);
      this.pnArenda.Controls.Add((Control) this.lbDog);
      this.pnArenda.Controls.Add((Control) this.lblOrg);
      this.pnArenda.Controls.Add((Control) this.cmbO);
      this.pnArenda.Controls.Add((Control) this.cmbArSupplier);
      this.pnArenda.Controls.Add((Control) this.lblArSupplier);
      this.pnArenda.Controls.Add((Control) this.txbArItog);
      this.pnArenda.Controls.Add((Control) this.lblArItog);
      this.pnArenda.Controls.Add((Control) this.lblArReceipt);
      this.pnArenda.Controls.Add((Control) this.cmbArReceipt);
      this.pnArenda.Controls.Add((Control) this.cmbArService);
      this.pnArenda.Controls.Add((Control) this.lblArService);
      this.pnArenda.Controls.Add((Control) this.txbArPeni);
      this.pnArenda.Controls.Add((Control) this.lblArPeni);
      this.pnArenda.Controls.Add((Control) this.txbArValue);
      this.pnArenda.Controls.Add((Control) this.lblArValue);
      this.pnArenda.Controls.Add((Control) this.lblNumber);
      this.pnArenda.Controls.Add((Control) this.lblArClient);
      this.pnArenda.Controls.Add((Control) this.txbArClient);
      this.pnArenda.Dock = DockStyle.Fill;
      this.pnArenda.Location = new Point(0, 259);
      this.pnArenda.Name = "pnArenda";
      this.pnArenda.Size = new Size(970, 463);
      this.pnArenda.TabIndex = 1;
      this.lbOrg.FormattingEnabled = true;
      this.lbOrg.ItemHeight = 16;
      this.lbOrg.Location = new Point(288, 139);
      this.lbOrg.Name = "lbOrg";
      this.lbOrg.Size = new Size(664, 84);
      this.lbOrg.TabIndex = 57;
      this.lbOrg.Visible = false;
      this.lbOrg.SelectedValueChanged += new EventHandler(this.lbOrg_SelectedValueChanged);
      this.lbOrg.KeyUp += new KeyEventHandler(this.lbOrg_KeyUp);
      this.lbOrg.MouseUp += new MouseEventHandler(this.lbOrg_MouseUp_1);
      this.cmbArRecipientAuto.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbArRecipientAuto.FormattingEnabled = true;
      this.cmbArRecipientAuto.Location = new Point(393, 199);
      this.cmbArRecipientAuto.Name = "cmbArRecipientAuto";
      this.cmbArRecipientAuto.Size = new Size(565, 24);
      this.cmbArRecipientAuto.TabIndex = 72;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(390, 183);
      this.label1.Name = "label1";
      this.label1.Size = new Size(192, 16);
      this.label1.TabIndex = 73;
      this.label1.Text = "Поставщик автоматический";
      this.cmbArPerfomer.FormattingEnabled = true;
      this.cmbArPerfomer.Location = new Point(657, 108);
      this.cmbArPerfomer.Name = "cmbArPerfomer";
      this.cmbArPerfomer.Size = new Size(187, 24);
      this.cmbArPerfomer.TabIndex = 71;
      this.lblArPerfomer.AutoSize = true;
      this.lblArPerfomer.Location = new Point(658, 90);
      this.lblArPerfomer.Name = "lblArPerfomer";
      this.lblArPerfomer.Size = new Size(95, 16);
      this.lblArPerfomer.TabIndex = 70;
      this.lblArPerfomer.Text = "Исполнитель";
      this.cmbArRecipient.FormattingEnabled = true;
      this.cmbArRecipient.Location = new Point(487, 108);
      this.cmbArRecipient.Name = "cmbArRecipient";
      this.cmbArRecipient.Size = new Size(152, 24);
      this.cmbArRecipient.TabIndex = 69;
      this.cmbArRecipient.SelectionChangeCommitted += new EventHandler(this.cmbArRecipient_SelectionChangeCommitted);
      this.lblArRecipient.AutoSize = true;
      this.lblArRecipient.Location = new Point(484, 86);
      this.lblArRecipient.Name = "lblArRecipient";
      this.lblArRecipient.Size = new Size(88, 16);
      this.lblArRecipient.TabIndex = 68;
      this.lblArRecipient.Text = "Получатель";
      this.dgvDistribute.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dgvDistribute.BackgroundColor = Color.AliceBlue;
      this.dgvDistribute.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvDistribute.Location = new Point(6, 231);
      this.dgvDistribute.Name = "dgvDistribute";
      this.dgvDistribute.Size = new Size(957, 226);
      this.dgvDistribute.TabIndex = 65;
      this.dgvDistribute.Visible = false;
      this.dgvDistribute.CellEndEdit += new DataGridViewCellEventHandler(this.dgvDistribute_CellEndEdit);
      this.dgvDistribute.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvDistribute_CellFormatting);
      this.gbArrange.Controls.Add((Control) this.rbRent);
      this.gbArrange.Controls.Add((Control) this.rbBalance);
      this.gbArrange.Location = new Point(12, 182);
      this.gbArrange.Name = "gbArrange";
      this.gbArrange.Size = new Size(289, 43);
      this.gbArrange.TabIndex = 66;
      this.gbArrange.TabStop = false;
      this.gbArrange.Text = "Распределять сумму";
      this.rbRent.AutoSize = true;
      this.rbRent.Location = new Point(136, 18);
      this.rbRent.Name = "rbRent";
      this.rbRent.Size = new Size(134, 20);
      this.rbRent.TabIndex = 1;
      this.rbRent.Text = "По начислениям";
      this.rbRent.UseVisualStyleBackColor = true;
      this.rbRent.CheckedChanged += new EventHandler(this.rbBalance_CheckedChanged);
      this.rbBalance.AutoSize = true;
      this.rbBalance.Checked = true;
      this.rbBalance.Location = new Point(9, 18);
      this.rbBalance.Name = "rbBalance";
      this.rbBalance.Size = new Size(94, 20);
      this.rbBalance.TabIndex = 0;
      this.rbBalance.TabStop = true;
      this.rbBalance.Text = "По долгам";
      this.rbBalance.UseVisualStyleBackColor = true;
      this.rbBalance.CheckedChanged += new EventHandler(this.rbBalance_CheckedChanged);
      this.txbNumber.Location = new Point(15, 157);
      this.txbNumber.Name = "txbNumber";
      this.txbNumber.Size = new Size(100, 22);
      this.txbNumber.TabIndex = 3;
      this.txbNumber.KeyDown += new KeyEventHandler(this.txbNumber_KeyDown);
      this.txbNumber.KeyPress += new KeyPressEventHandler(this.txbPacket_KeyPress);
      this.lblAr.AutoSize = true;
      this.lblAr.Location = new Point(423, 57);
      this.lblAr.Name = "lblAr";
      this.lblAr.Size = new Size(0, 16);
      this.lblAr.TabIndex = 64;
      this.cmbDog.AutoCompleteSource = AutoCompleteSource.ListItems;
      this.cmbDog.FormattingEnabled = true;
      this.cmbDog.Location = new Point(134, 54);
      this.cmbDog.Name = "cmbDog";
      this.cmbDog.Size = new Size(90, 24);
      this.cmbDog.TabIndex = 1;
      this.cmbDog.SelectionChangeCommitted += new EventHandler(this.cmbDog_SelectionChangeCommitted);
      this.cmbDog.TextChanged += new EventHandler(this.cmbDog_TextChanged);
      this.cmbDog.KeyDown += new KeyEventHandler(this.cmbDog_KeyDown);
      this.cmbDog.KeyUp += new KeyEventHandler(this.cmbDog_KeyUp);
      this.lblNum.AutoSize = true;
      this.lblNum.Location = new Point(12, 57);
      this.lblNum.Name = "lblNum";
      this.lblNum.Size = new Size(116, 16);
      this.lblNum.TabIndex = 59;
      this.lblNum.Text = "Номер договора";
      this.lbDog.FormattingEnabled = true;
      this.lbDog.ItemHeight = 16;
      this.lbDog.Location = new Point(132, 67);
      this.lbDog.Name = "lbDog";
      this.lbDog.Size = new Size(118, 100);
      this.lbDog.TabIndex = 62;
      this.lbDog.Visible = false;
      this.lbDog.SelectedValueChanged += new EventHandler(this.lbDog_SelectedValueChanged);
      this.lbDog.KeyUp += new KeyEventHandler(this.lbDog_KeyUp);
      this.lbDog.MouseUp += new MouseEventHandler(this.lbDog_MouseUp);
      this.lblOrg.AutoSize = true;
      this.lblOrg.Location = new Point(9, 23);
      this.lblOrg.Name = "lblOrg";
      this.lblOrg.Size = new Size(95, 16);
      this.lblOrg.TabIndex = 58;
      this.lblOrg.Text = "Организация";
      this.cmbO.AutoCompleteSource = AutoCompleteSource.ListItems;
      this.cmbO.FormattingEnabled = true;
      this.cmbO.Location = new Point(110, 20);
      this.cmbO.Name = "cmbO";
      this.cmbO.Size = new Size(664, 24);
      this.cmbO.TabIndex = 0;
      this.cmbO.SelectionChangeCommitted += new EventHandler(this.cmbO_SelectionChangeCommitted);
      this.cmbO.TextChanged += new EventHandler(this.cmbO_TextChanged);
      this.cmbO.KeyDown += new KeyEventHandler(this.cmbO_KeyDown);
      this.cmbO.KeyUp += new KeyEventHandler(this.cmbO_KeyUp);
      this.cmbArSupplier.FormattingEnabled = true;
      this.cmbArSupplier.Location = new Point(426, 66);
      this.cmbArSupplier.Name = "cmbArSupplier";
      this.cmbArSupplier.Size = new Size(204, 24);
      this.cmbArSupplier.TabIndex = 55;
      this.cmbArSupplier.Visible = false;
      this.cmbArSupplier.SelectionChangeCommitted += new EventHandler(this.cmbArSupplier_SelectionChangeCommitted);
      this.cmbArSupplier.KeyDown += new KeyEventHandler(this.cmbArSupplier_KeyDown);
      this.lblArSupplier.AutoSize = true;
      this.lblArSupplier.Location = new Point(423, 48);
      this.lblArSupplier.Name = "lblArSupplier";
      this.lblArSupplier.Size = new Size(80, 16);
      this.lblArSupplier.TabIndex = 54;
      this.lblArSupplier.Text = "Поставщик";
      this.lblArSupplier.Visible = false;
      this.txbArItog.Enabled = false;
      this.txbArItog.Location = new Point(618, 157);
      this.txbArItog.Name = "txbArItog";
      this.txbArItog.Size = new Size(97, 22);
      this.txbArItog.TabIndex = 53;
      this.lblArItog.AutoSize = true;
      this.lblArItog.Location = new Point(615, 138);
      this.lblArItog.Name = "lblArItog";
      this.lblArItog.Size = new Size(50, 16);
      this.lblArItog.TabIndex = 52;
      this.lblArItog.Text = "Итого:";
      this.lblArReceipt.AutoSize = true;
      this.lblArReceipt.Location = new Point(9, 90);
      this.lblArReceipt.Name = "lblArReceipt";
      this.lblArReceipt.Size = new Size(273, 16);
      this.lblArReceipt.TabIndex = 51;
      this.lblArReceipt.Text = "Квитанция, по которой поступил платеж";
      this.cmbArReceipt.FormattingEnabled = true;
      this.cmbArReceipt.Location = new Point(10, 108);
      this.cmbArReceipt.Name = "cmbArReceipt";
      this.cmbArReceipt.Size = new Size(265, 24);
      this.cmbArReceipt.TabIndex = 43;
      this.cmbArReceipt.SelectionChangeCommitted += new EventHandler(this.cmbArReceipt_SelectionChangeCommitted);
      this.cmbArReceipt.KeyDown += new KeyEventHandler(this.cmbArReceipt_KeyDown);
      this.cmbArService.Enabled = false;
      this.cmbArService.FormattingEnabled = true;
      this.cmbArService.Location = new Point(299, 108);
      this.cmbArService.Name = "cmbArService";
      this.cmbArService.Size = new Size(168, 24);
      this.cmbArService.TabIndex = 44;
      this.cmbArService.SelectionChangeCommitted += new EventHandler(this.cmbArService_SelectionChangeCommitted);
      this.cmbArService.KeyDown += new KeyEventHandler(this.cmbArService_KeyDown);
      this.lblArService.AutoSize = true;
      this.lblArService.Location = new Point(299, 91);
      this.lblArService.Name = "lblArService";
      this.lblArService.Size = new Size(54, 16);
      this.lblArService.TabIndex = 50;
      this.lblArService.Text = "Услуга";
      this.txbArPeni.Location = new Point(393, 157);
      this.txbArPeni.Name = "txbArPeni";
      this.txbArPeni.Size = new Size(100, 22);
      this.txbArPeni.TabIndex = 5;
      this.txbArPeni.KeyDown += new KeyEventHandler(this.txtPeni_KeyDown);
      this.txbArPeni.KeyPress += new KeyPressEventHandler(this.txbArValue_KeyPress);
      this.txbArPeni.Leave += new EventHandler(this.txbArPeni_Leave);
      this.lblArPeni.AutoSize = true;
      this.lblArPeni.Location = new Point(390, 138);
      this.lblArPeni.Name = "lblArPeni";
      this.lblArPeni.Size = new Size(42, 16);
      this.lblArPeni.TabIndex = 49;
      this.lblArPeni.Text = "Пени";
      this.txbArValue.Location = new Point(253, 157);
      this.txbArValue.Name = "txbArValue";
      this.txbArValue.Size = new Size(100, 22);
      this.txbArValue.TabIndex = 4;
      this.txbArValue.KeyDown += new KeyEventHandler(this.txbArValue_KeyDown);
      this.txbArValue.KeyPress += new KeyPressEventHandler(this.txbArValue_KeyPress);
      this.txbArValue.Leave += new EventHandler(this.txbArValue_Leave);
      this.lblArValue.AutoSize = true;
      this.lblArValue.Location = new Point(250, 138);
      this.lblArValue.Name = "lblArValue";
      this.lblArValue.Size = new Size(51, 16);
      this.lblArValue.TabIndex = 48;
      this.lblArValue.Text = "Сумма";
      this.lblNumber.AutoSize = true;
      this.lblNumber.Location = new Point(12, 138);
      this.lblNumber.Name = "lblNumber";
      this.lblNumber.Size = new Size(206, 16);
      this.lblNumber.TabIndex = 32;
      this.lblNumber.Text = "Номер платежного поручения";
      this.lblArClient.AutoSize = true;
      this.lblArClient.Location = new Point(236, 57);
      this.lblArClient.Name = "lblArClient";
      this.lblArClient.Size = new Size(65, 16);
      this.lblArClient.TabIndex = 67;
      this.lblArClient.Text = "Лицевой";
      this.txbArClient.Location = new Point(307, 54);
      this.txbArClient.Name = "txbArClient";
      this.txbArClient.Size = new Size(100, 22);
      this.txbArClient.TabIndex = 2;
      this.txbArClient.KeyDown += new KeyEventHandler(this.txtClientId_KeyDown);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(970, 762);
      this.Controls.Add((Control) this.pnKv);
      this.Controls.Add((Control) this.pnArenda);
      this.Controls.Add((Control) this.gbAttribute);
      this.Controls.Add((Control) this.gbVid);
      this.Controls.Add((Control) this.pnBtn);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.hp.SetHelpKeyword((Control) this, "kv71.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.Margin = new Padding(4);
      this.Name = "FrmAttribute";
      this.hp.SetShowHelp((Control) this, true);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Атрибуты платежей";
      this.FormClosed += new FormClosedEventHandler(this.FrmAttribute_FormClosed);
      this.Load += new EventHandler(this.FrmAttribute_Load);
      this.Shown += new EventHandler(this.FrmAttribute_Shown);
      this.KeyDown += new KeyEventHandler(this.FrmAttribute_KeyDown);
      this.KeyPress += new KeyPressEventHandler(this.FrmAttribute_KeyPress);
      this.pnBtn.ResumeLayout(false);
      this.gbAttribute.ResumeLayout(false);
      this.gbAttribute.PerformLayout();
      this.gbVid.ResumeLayout(false);
      this.gbVid.PerformLayout();
      this.pnKv.ResumeLayout(false);
      this.gbCounters.ResumeLayout(false);
      ((ISupportInitialize) this.dgvEvidence).EndInit();
      this.pnEvidence.ResumeLayout(false);
      this.pnMain.ResumeLayout(false);
      this.pnMain.PerformLayout();
      this.pnArenda.ResumeLayout(false);
      this.pnArenda.PerformLayout();
      ((ISupportInitialize) this.dgvDistribute).EndInit();
      this.gbArrange.ResumeLayout(false);
      this.gbArrange.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
