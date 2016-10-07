// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmPayment
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using Kvartplata.StaticResourse;
using NHibernate;
using NHibernate.Criterion;
using SaveSettings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmPayment : Form
  {
    private FormStateSaver formStateSaver = new FormStateSaver(FrmPayment.container);
    protected GridSettings MySettingsPayment = new GridSettings();
    private bool changecbReceipt = false;
    private IContainer components = (IContainer) null;
    private Raion raion;
    private Company company;
    private Home home;
    public LsClient client;
    private ISession session;
    private int level;
    private static IContainer container;
    private Panel pnButton;
    private Button btnExit;
    private Button btnAdd;
    private Button btnDelete;
    private Button btnEdit;
    private Panel pnTop;
    private DataGridView dgvPayment;
    private TextBox txtSearch;
    private Label lblSearch;
    private Button btnPacket;
    private MonthPicker mpCurrentPeriod;
    private ContextMenuStrip cmsPacket;
    private ToolStripMenuItem miSum;
    private ToolStripMenuItem miDelete;
    private ToolStripMenuItem miChange;
    private Button btnSearch;
    private Label lblSum;
    private TextBox txtSum;
    private Button btnReport;
    private ContextMenuStrip cmReport;
    private ToolStripMenuItem miPlat;
    public HelpProvider hp;
    private CheckBox cbShowAnotherOrg;
    private TextBox txbDog;
    private Label lblDog;
    private CheckBox cbArhiv;
    private GroupBox gbSorter;
    private RadioButton rbPacket;
    private RadioButton rbPaymentDate;
    private RadioButton rbLs;
    private Label lblPurpose;
    private ComboBox cmbPurposeFilter;
    private ComboBox cbReceipt;
    private Label label1;

    public IList<Payment> paymentList { get; set; }

    public FrmPayment()
    {
      this.InitializeComponent();
      this.formStateSaver.ParentForm = (Form) this;
      this.SetGridConfigFileSettings();
    }

    public FrmPayment(Raion r, Company c, Home h, LsClient ls, int level)
    {
      this.raion = r;
      this.home = h;
      this.company = c;
      this.client = ls;
      this.level = level;
      this.formStateSaver.ParentForm = (Form) this;
      this.InitializeComponent();
      this.SetGridConfigFileSettings();
      this.cbShowAnotherOrg.Checked = Options.ShowAnotherOrg;
    }

    private void FrmPayment_Load(object sender, EventArgs e)
    {
      this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
      ToolTip toolTip = new ToolTip();
      toolTip.SetToolTip((Control) this.txtSearch, "Введите номер счета и нажмите Enter");
      toolTip.SetToolTip((Control) this.lblSearch, "Введите номер счета и нажмите Enter");
      this.MySettingsPayment.GridName = "Payment";
      this.MySettingsPayment.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
      if (!Options.Arenda)
      {
        this.txbDog.Visible = false;
        this.lblDog.Visible = false;
        this.btnSearch.Location = new Point(406, 5);
      }
      if (this.level != 4)
      {
        this.cbArhiv.Visible = false;
      }
      else
      {
        this.cbArhiv.Visible = true;
        this.gbSorter.Visible = false;
      }
      this.LoadSettingsPayment();
      IList list = this.session.CreateCriteria(typeof (PurposePay)).AddOrder(Order.Asc("PurposePayId")).List();
      list.Insert(0, (object) new PurposePay()
      {
        PurposePayId = (short) 0,
        PurposePayName = ""
      });
      this.cmbPurposeFilter.DataSource = (object) list;
      this.cmbPurposeFilter.ValueMember = "PurposePayId";
      this.cmbPurposeFilter.DisplayMember = "PurposePayName";
      List<Receipt> receiptList = new List<Receipt>();
      receiptList.Add(new Receipt());
      receiptList.AddRange((IEnumerable<Receipt>) this.session.CreateQuery("select r from Receipt r").List<Receipt>());
      this.cbReceipt.DataSource = (object) receiptList;
      this.cbReceipt.DisplayMember = "ReceiptName";
      this.cbReceipt.ValueMember = "ReceiptId";
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnEnter_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(41, 2, this.company, true))
        return;
      FrmAttribute frmAttribute = new FrmAttribute(0, this.client, this.company);
      frmAttribute.Operation = (short) 1;
      int num = (int) frmAttribute.ShowDialog();
      if (this.level != 4)
        this.AddToPaymentList(frmAttribute.Pays);
      frmAttribute.Dispose();
      if (this.level != 4)
        return;
      this.LoadPaymentList();
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.dgvPayment.Rows.Count <= 0 || this.dgvPayment.CurrentRow == null)
          return;
        if (!this.session.IsOpen)
          this.session = Domain.CurrentSession;
        if (!KvrplHelper.CheckProxy(41, 2, this.company, true))
          return;
        Payment dataBoundItem = this.dgvPayment.CurrentRow.DataBoundItem as Payment;
        DateTime periodName1 = dataBoundItem.PeriodName;
        DateTime? periodName2 = KvrplHelper.GetKvrClose(dataBoundItem.LsClient.ClientId, Options.Complex, Options.ComplexPrior).PeriodName;
        if (periodName2.HasValue && periodName1 <= periodName2.GetValueOrDefault())
        {
          int num1 = (int) MessageBox.Show("Платежи закрытого периода не подлежат удалению!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          if (MessageBox.Show("Вы уверены, что хотите удалить этот платеж?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            return;
          try
          {
            if (dataBoundItem.Supplier.Recipient.BaseOrgId == 0 && dataBoundItem.Supplier.Perfomer.BaseOrgId == 0)
              dataBoundItem.Supplier = this.session.Get<Supplier>((object) 0);
          }
          catch (Exception ex)
          {
          }
          if ((int) dataBoundItem.PPay.PurposePayId == 9 && !KvrplHelper.CheckProxy(85, 2, Options.Company, false))
          {
            int num2 = (int) MessageBox.Show("Платеж не подлежит удалению!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else if ((int) dataBoundItem.PPay.PurposePayId == 31 || (int) dataBoundItem.PPay.PurposePayId == 39)
          {
            int num3 = (int) MessageBox.Show("Платеж не подлежит удалению!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else
          {
            this.session.Delete((object) dataBoundItem);
            this.session.Flush();
            this.session.Clear();
            if (this.level != 4)
            {
              this.DeleteFromPaymentList(dataBoundItem);
            }
            else
            {
              this.LoadPaymentList();
              this.LoadSettingsPayment();
            }
          }
        }
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.dgvPayment.Rows.Count <= 0 || this.dgvPayment.CurrentRow == null || !KvrplHelper.CheckProxy(41, 2, this.company, true))
        return;
      Payment dataBoundItem = this.dgvPayment.CurrentRow.DataBoundItem as Payment;
      if (dataBoundItem.Period.PeriodId <= KvrplHelper.GetKvrClose(dataBoundItem.LsClient.ClientId, Options.Complex, Options.ComplexPrior).PeriodId)
      {
        int num1 = (int) MessageBox.Show("Платежи закрытого периода не подлежат редактированию!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        FrmAttribute frmAttribute = new FrmAttribute(dataBoundItem.PaymentId, this.client, this.company);
        frmAttribute.Operation = (short) 2;
        int num2 = (int) frmAttribute.ShowDialog();
        if (this.level != 4)
          this.UpdatePaymentList(frmAttribute.Pays);
        frmAttribute.Dispose();
        if (this.level == 4)
        {
          this.LoadPaymentList();
          this.LoadSettingsPayment();
        }
      }
    }

    private void btnPacketSum_Click(object sender, EventArgs e)
    {
      string str = "";
      string text = "";
      double num1 = 0.0;
      double num2 = 0.0;
      InputBox.InputValue("Номер пачки", "Введите номер пачки", "", "", ref str, 0, 99000000);
      this.session = Domain.CurrentSession;
      foreach (object[] objArray in (IEnumerable) this.session.CreateQuery(string.Format("select isnull(sum(p.PaymentValue),0)+isnull(sum(p.PaymentPeni),0),sum(p.PaymentPeni),c.CompanyId,count(*) as count from Payment p,LsClient ls,Company c where p.LsClient.Company=c and p.LsClient=ls and p.PacketNum='{0}' and p.Period.PeriodId={1} " + Options.MainConditions1 + " group by c.CompanyId order by c.CompanyId", (object) str, (object) Options.Period.PeriodId)).List())
      {
        text = text + "По ЛУ №" + objArray[2].ToString() + " всего " + objArray[3].ToString() + " платежей на сумму " + objArray[0].ToString() + " руб.(в т.ч. пени " + objArray[1] + " руб.)\n";
        num1 += Convert.ToDouble(objArray[0]);
        num2 += Convert.ToDouble(objArray[1]);
      }
      if (str != "")
      {
        if (text == "")
          text = "Платежей с указанным номером пачки не существует.";
        int num3 = (int) MessageBox.Show(text, "Итого сумма по пачке №" + str.ToString() + ": " + num1.ToString() + " руб. (в т.ч. пени " + num2.ToString() + " руб.)", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      this.session.Clear();
    }

    private void btnPacket_Click(object sender, EventArgs e)
    {
      this.cmsPacket.Show((Control) this, this.btnPacket.Left + this.btnPacket.Width, this.pnButton.Top + this.btnPacket.Top);
    }

    private void button1_Click(object sender, EventArgs e)
    {
      KeyEventArgs e1 = new KeyEventArgs(Keys.Return);
      if (this.txtSearch.Text != "")
        this.txtSearch_KeyDown(sender, e1);
      if (this.txtSum.Text != "")
        this.txtSum_KeyDown(sender, e1);
      if (!(this.txbDog.Text != ""))
        return;
      this.txbDog_KeyDown(sender, e1);
    }

    private void btnReport_Click(object sender, EventArgs e)
    {
      this.cmReport.Show((Control) this, this.btnReport.Left + this.btnReport.Width, this.pnButton.Top + this.btnReport.Top);
    }

    public void SetGridConfigFileSettings()
    {
      this.MySettingsPayment.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
    }

    private void LoadSettingsPayment()
    {
      this.MySettingsPayment.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvPayment.Columns)
        this.MySettingsPayment.GetMySettings(column);
    }

    private void AddToPaymentList(IList<Payment> pays)
    {
      IList<Payment> paymentList = (IList<Payment>) new List<Payment>();
      if ((uint) this.dgvPayment.Rows.Count > 0U)
        paymentList = (IList<Payment>) (this.dgvPayment.DataSource as List<Payment>);
      foreach (Payment pay in (IEnumerable<Payment>) pays)
        paymentList.Add(pay);
      this.dgvPayment.Columns.Clear();
      this.dgvPayment.DataSource = (object) null;
      this.dgvPayment.DataSource = (object) paymentList;
      this.SetDgvPaymentsView();
      this.LoadSettingsPayment();
    }

    private void MakeDgvPaymentColumns()
    {
      this.dgvPayment.Columns["PaymentDate"].HeaderText = "Дата оплаты";
      this.dgvPayment.Columns["PacketNum"].HeaderText = Options.Kvartplata ? (Options.Arenda ? "Номер пачки / платежного документа" : "Номер пачки") : "Номер платежного документа";
      this.dgvPayment.Columns["PaymentValue"].HeaderText = "Сумма оплаты";
      this.dgvPayment.Columns["PaymentPeni"].HeaderText = "Пени";
      this.dgvPayment.Columns["ClientId"].HeaderText = "Лицевой счет";
      this.dgvPayment.Columns["PeriodPayValue"].HeaderText = "Месяц оплаты";
      this.dgvPayment.Columns["SourceName"].HeaderText = "Источник";
      this.dgvPayment.Columns["PurposeName"].HeaderText = "Назначение";
      this.dgvPayment.Columns["ServiceName"].HeaderText = "Услуга";
      this.dgvPayment.Columns["SupplierName"].HeaderText = "Поставщик";
      this.dgvPayment.Columns["ReceiptName"].HeaderText = "Квитанция";
      this.dgvPayment.Columns["ClientId"].DisplayIndex = 0;
      this.dgvPayment.Columns["OldId"].DisplayIndex = 1;
      this.dgvPayment.Columns["PaymentDate"].DisplayIndex = 2;
      this.dgvPayment.Columns["SupplierName"].DisplayIndex = 3;
      this.dgvPayment.Columns["PaymentValue"].DisplayIndex = 4;
      this.dgvPayment.Columns["PaymentPeni"].DisplayIndex = 5;
      this.dgvPayment.Columns["Payment"].DisplayIndex = 6;
      this.dgvPayment.Columns["PacketNum"].DisplayIndex = 7;
      this.dgvPayment.Columns["SourceName"].DisplayIndex = 8;
      this.dgvPayment.Columns["PurposeName"].DisplayIndex = 9;
      if (Options.City != 4)
      {
        this.dgvPayment.Columns["ReceiptName"].DisplayIndex = 3;
        this.dgvPayment.Columns["ServiceName"].DisplayIndex = 4;
        this.dgvPayment.Columns["PeriodPayValue"].DisplayIndex = 10;
      }
      else
      {
        this.dgvPayment.Columns["PeriodPayValue"].DisplayIndex = 3;
        this.dgvPayment.Columns["ReceiptName"].DisplayIndex = 10;
        this.dgvPayment.Columns["ServiceName"].DisplayIndex = 11;
      }
      KvrplHelper.ViewEdit(this.dgvPayment);
    }

    private void DeleteFromPaymentList(Payment currentpay)
    {
      if ((uint) this.dgvPayment.Rows.Count <= 0U)
        return;
      this.paymentList.Remove(currentpay);
      this.dgvPayment.Columns.Clear();
      this.cmbPurposeFilter_SelectedValueChanged((object) null, (EventArgs) null);
    }

    private void UpdatePaymentList(IList<Payment> pays)
    {
      if (pays.Count <= 0)
        return;
      int index1 = this.dgvPayment.CurrentRow != null ? this.dgvPayment.CurrentRow.Index : -1;
      for (int index2 = 0; index2 < this.paymentList.Count; ++index2)
      {
        if (index2 == index1)
        {
          if (pays[0].PaymentId == this.paymentList[index2].PaymentId)
          {
            this.paymentList[index2].Period = pays[0].Period;
            this.paymentList[index2].PeriodPay = pays[0].PeriodPay;
            this.paymentList[index2].Service = pays[0].Service;
            this.paymentList[index2].Receipt = pays[0].Receipt;
            this.paymentList[index2].SPay = pays[0].SPay;
            this.paymentList[index2].PPay = pays[0].PPay;
            this.paymentList[index2].PacketNum = pays[0].PacketNum;
            this.paymentList[index2].PaymentValue = pays[0].PaymentValue;
            this.paymentList[index2].PaymentPeni = pays[0].PaymentPeni;
            this.paymentList[index2].PaymentDate = pays[0].PaymentDate;
            this.paymentList[index2].RecipientId = pays[0].RecipientId;
            this.paymentList[index2].UName = pays[0].UName;
            this.paymentList[index2].DEdit = pays[0].DEdit;
            this.paymentList[index2].Supplier = pays[0].Supplier;
            break;
          }
          break;
        }
      }
      this.dgvPayment.Columns.Clear();
      this.cmbPurposeFilter_SelectedValueChanged((object) null, (EventArgs) null);
      this.dgvPayment.CurrentCell = this.dgvPayment.Rows[index1].Cells[0];
    }

    private void LoadPaymentList()
    {
      string str1 = "";
      string str2 = "";
      string str3 = "";
      if (!this.cbShowAnotherOrg.Checked)
        str1 = " and p.Service not in (select Service_id from ServiceParam where Company_id=p.LsClient.Company.CompanyId and SendRent=1)";
      if (!this.cbArhiv.Checked)
        str2 = " and p.Period.PeriodId={0}";
      this.session = Domain.CurrentSession;
      this.paymentList = (IList<Payment>) new List<Payment>();
      if (this.rbLs.Checked)
        str3 = "ls.ClientId";
      if (this.rbPacket.Checked)
        str3 = "p.PacketNum";
      if (this.rbPaymentDate.Checked)
        str3 = "p.PaymentDate";
      switch (this.level)
      {
        case 0:
          this.paymentList = this.session.CreateQuery(string.Format("select p from Payment p left join fetch p.LsClient ls left join fetch p.PeriodPay left join fetch p.SPay left join fetch p.PPay left join fetch p.Service where p.Period.PeriodId={0} " + Options.MainConditions1 + str1 + " order by " + str3, (object) Options.Period.PeriodId)).List<Payment>();
          break;
        case 1:
          this.paymentList = this.session.CreateQuery(string.Format("select p from Payment p left join fetch p.LsClient ls left join fetch ls.Company c left join fetch p.PeriodPay left join fetch p.SPay left join fetch p.PPay left join fetch p.Service s where p.Period.PeriodId={0} and c.Raion.IdRnn={1}" + Options.MainConditions1 + str1 + " order by " + str3, (object) Options.Period.PeriodId, (object) this.raion.IdRnn)).List<Payment>();
          break;
        case 2:
          this.paymentList = this.session.CreateQuery(string.Format("select p from Payment p left join fetch p.LsClient ls left join fetch p.PeriodPay left join fetch p.SPay left join fetch p.PPay left join fetch p.Service s where p.Period.PeriodId={0} and ls.Company.CompanyId={1}" + Options.MainConditions1 + str1 + " order by " + str3, (object) Options.Period.PeriodId, (object) this.company.CompanyId)).List<Payment>();
          break;
        case 3:
          this.paymentList = this.session.CreateQuery(string.Format("select p from Payment p left join fetch p.LsClient ls left join fetch p.PeriodPay left join fetch p.SPay left join fetch p.PPay left join fetch p.Service s where p.Period.PeriodId={0} and ls.Company.CompanyId={1} and ls.Home.IdHome={2}" + Options.MainConditions1 + str1 + " order by " + str3, (object) Options.Period.PeriodId, (object) this.company.CompanyId, (object) this.home.IdHome)).List<Payment>();
          break;
        case 4:
          try
          {
            this.paymentList = this.session.CreateQuery(string.Format("select p from Payment p left join fetch p.PeriodPay left join fetch p.SPay left join fetch p.PPay left join fetch p.Service s where p.LsClient.ClientId={1}" + str2 + str1 + " order by p.Period.PeriodId desc,p.PaymentDate desc", (object) Options.Period.PeriodId, (object) this.client.ClientId)).List<Payment>();
            break;
          }
          catch
          {
            break;
          }
      }
      if (this.client != null && this.paymentList.Count > 0 && !this.cbArhiv.Checked)
      {
        IList list = this.session.CreateQuery(string.Format("select sum(p.PaymentValue),sum(p.PaymentPeni) from Payment p where p.LsClient.ClientId={0} and p.Period.PeriodId={1}" + str1, (object) this.client.ClientId, (object) Options.Period.PeriodId)).List();
        this.paymentList.Insert(this.paymentList.Count, new Payment()
        {
          PaymentId = 0,
          PaymentValue = Convert.ToDecimal(((object[]) list[0])[0].ToString()),
          PaymentPeni = Convert.ToDecimal(((object[]) list[0])[1].ToString())
        });
      }
      this.dgvPayment.Columns.Clear();
      this.dgvPayment.DataSource = (object) null;
      Receipt selectedItem = (Receipt) this.cbReceipt.SelectedItem;
      if (selectedItem != null && selectedItem.ReceiptName != null)
      {
        foreach (Payment payment in this.paymentList.ToArray<Payment>())
        {
          if (!payment.ReceiptName.Equals(selectedItem.ReceiptName))
            this.paymentList.Remove(payment);
        }
      }
      this.dgvPayment.DataSource = (object) this.paymentList;
      this.SetDgvPaymentsView();
      this.LoadSettingsPayment();
    }

    private void SetDgvPaymentsView()
    {
      if (!Options.Kvartplata)
        KvrplHelper.AddTextBoxColumn(this.dgvPayment, 1, "Договор", "OldId", 90, true);
      else if (!Options.Arenda)
        KvrplHelper.AddTextBoxColumn(this.dgvPayment, 1, "Короткий л/с", "OldId", 90, true);
      else
        KvrplHelper.AddTextBoxColumn(this.dgvPayment, 1, "Короткий л/с / Договор", "OldId", 90, true);
      this.dgvPayment.Columns["PaymentDate"].HeaderText = "Дата оплаты";
      this.dgvPayment.Columns["PacketNum"].HeaderText = Options.Kvartplata ? (Options.Arenda ? "Номер пачки / платежного документа" : "Номер пачки") : "Номер платежного документа";
      this.dgvPayment.Columns["PaymentValue"].HeaderText = "Сумма оплаты";
      this.dgvPayment.Columns["PaymentPeni"].HeaderText = "Пени";
      this.dgvPayment.Columns["ClientId"].HeaderText = "Лицевой счет";
      this.dgvPayment.Columns["PeriodPayValue"].HeaderText = "Месяц оплаты";
      this.dgvPayment.Columns["SourceName"].HeaderText = "Источник";
      this.dgvPayment.Columns["PurposeName"].HeaderText = "Назначение";
      this.dgvPayment.Columns["ServiceName"].HeaderText = "Услуга";
      this.dgvPayment.Columns["SupplierName"].HeaderText = "Получатель - Исполнитель";
      this.dgvPayment.Columns["RecipientId"].Visible = false;
      this.dgvPayment.Columns["ReceiptName"].HeaderText = "Квитанция";
      this.dgvPayment.Columns["ClientId"].DisplayIndex = 0;
      this.dgvPayment.Columns["OldId"].DisplayIndex = 1;
      this.dgvPayment.Columns["OhlaccountId"].Visible = false;
      KvrplHelper.AddComboBoxColumn(this.dgvPayment, 4, (IList) null, "BaseOrgId", "NameOrgMinDop", "Получатель автоматический", "RecipientId2", 180, 180);
      this.dgvPayment.Columns["PaymentDate"].DisplayIndex = 2;
      this.dgvPayment.Columns["SupplierName"].DisplayIndex = 3;
      this.dgvPayment.Columns["RecipientId2"].DisplayIndex = 4;
      this.dgvPayment.Columns["PaymentValue"].DisplayIndex = 5;
      this.dgvPayment.Columns["PaymentPeni"].DisplayIndex = 6;
      this.dgvPayment.Columns["PacketNum"].DisplayIndex = 7;
      this.dgvPayment.Columns["SourceName"].DisplayIndex = 8;
      this.dgvPayment.Columns["PurposeName"].DisplayIndex = 9;
      if (Options.City != 4)
      {
        this.dgvPayment.Columns["ReceiptName"].DisplayIndex = 3;
        this.dgvPayment.Columns["ServiceName"].DisplayIndex = 5;
      }
      else
      {
        this.dgvPayment.Columns["PeriodPayValue"].DisplayIndex = 3;
        this.dgvPayment.Columns["ReceiptName"].DisplayIndex = 11;
      }
      KvrplHelper.ViewEdit(this.dgvPayment);
      if (Options.City != 4)
        KvrplHelper.AddTextBoxColumn(this.dgvPayment, 9, "Итого", "Payment", 90, false);
      else
        KvrplHelper.AddTextBoxColumn(this.dgvPayment, 8, "Итого", "Payment", 90, false);
      if (!this.cbArhiv.Checked)
      {
        this.dgvPayment.Columns["PeriodValue"].Visible = false;
      }
      else
      {
        this.dgvPayment.Columns["ClientId"].Visible = false;
        this.dgvPayment.Columns["OldId"].Visible = false;
        this.dgvPayment.Columns["PeriodValue"].Visible = true;
        this.dgvPayment.Columns["PeriodValue"].HeaderText = "Период";
        this.dgvPayment.Columns["PeriodValue"].DisplayIndex = 0;
      }
      IList<BaseOrg> baseOrgList = this.session.CreateQuery(string.Format("select new BaseOrg(BaseOrgId,NameOrgMin) from BaseOrg where BaseOrgId in (SELECT IDBASEORG FROM Postaver) order by NameOrgMin")).List<BaseOrg>();
      foreach (DataGridViewRow row in (IEnumerable) this.dgvPayment.Rows)
      {
        if (row.DataBoundItem != null)
        {
          row.Cells["Payment"].Value = (object) (Convert.ToDecimal(((Payment) row.DataBoundItem).PaymentValue) + Convert.ToDecimal(((Payment) row.DataBoundItem).PaymentPeni));
          if (((Payment) row.DataBoundItem).LsClient != null)
          {
            if (((Payment) row.DataBoundItem).LsClient.Complex.IdFk == Options.Complex.IdFk)
            {
              row.Cells["OldId"].Value = (object) ((Payment) row.DataBoundItem).LsClient.OldId;
            }
            else
            {
              LsArenda lsArenda = this.session.CreateCriteria(typeof (LsArenda)).Add((ICriterion) Restrictions.Eq("LsClient.ClientId", (object) ((Payment) row.DataBoundItem).LsClient.ClientId)).UniqueResult<LsArenda>();
              if (lsArenda != null)
                row.Cells["OldId"].Value = (object) lsArenda.DogovorNum;
            }
          }
          row.Cells["RecipientId2"] = (DataGridViewCell) new DataGridViewComboBoxCell()
          {
            DisplayStyleForCurrentCellOnly = true,
            ValueMember = "BaseOrgId",
            DisplayMember = "NameOrgMin",
            DataSource = (object) baseOrgList
          };
          if (((Payment) row.DataBoundItem).RecipientId != null)
            row.Cells["RecipientId2"].Value = (object) ((Payment) row.DataBoundItem).RecipientId.BaseOrgId;
        }
      }
    }

    private void dgvPayment_CellMouseDoubleClick_1(object sender, DataGridViewCellMouseEventArgs e)
    {
      this.btnEdit_Click(sender, (EventArgs) e);
    }

    private void dgvPayment_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
    {
      e.Row.Cells[1].Value = (object) this.company;
      e.Row.Cells[0].Value = (object) DateTime.Now;
      e.Row.Cells["PostalCode"].Value = (object) "98052-6399";
      e.Row.Cells["Region"].Value = (object) "NA";
      e.Row.Cells["Country"].Value = (object) "USA";
    }

    private void dgvPayment_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (((DataGridView) sender).DataSource == null)
        return;
      DataGridViewRow row = this.dgvPayment.Rows[e.RowIndex];
      if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == "ИТОГО")
        row.DefaultCellStyle.Font = new Font(this.dgvPayment.Font, FontStyle.Bold);
    }

    private void dgvPayment_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsPayment.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsPayment.Columns[this.MySettingsPayment.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsPayment.Save();
    }

    private void dgvPayment_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    private void dtmpCurrentPeriod_ValueChanged(object sender, EventArgs e)
    {
      Options.Period = KvrplHelper.SaveCurrentPeriod(this.mpCurrentPeriod.Value);
      this.Cursor = Cursors.WaitCursor;
      this.LoadPaymentList();
      this.Cursor = Cursors.Default;
    }

    private void txtSearch_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      int int32;
      try
      {
        int32 = Convert.ToInt32(this.txtSearch.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Некорректный формат", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      foreach (DataGridViewRow row in (IEnumerable) this.dgvPayment.Rows)
      {
        if (row.Cells["ClientId"].Value.ToString() != "ИТОГО" && Convert.ToInt32(row.Cells["ClientId"].Value) == int32 || row.Cells["ClientId"].Value.ToString() != "ИТОГО" && Convert.ToString(row.Cells["OldId"].Value) == int32.ToString())
        {
          this.dgvPayment.CurrentCell = row.Cells["ClientId"];
          return;
        }
      }
      int num1 = (int) MessageBox.Show("Лицевой счет не найден!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void miDelete_Click(object sender, EventArgs e)
    {
      FrmChangeAttribute frmChangeAttribute = new FrmChangeAttribute(this.raion, this.company, 2);
      int num = (int) frmChangeAttribute.ShowDialog();
      frmChangeAttribute.Close();
      this.LoadPaymentList();
    }

    private void miChange_Click(object sender, EventArgs e)
    {
      FrmChangeAttribute frmChangeAttribute = new FrmChangeAttribute(this.raion, this.company, 1);
      int num = (int) frmChangeAttribute.ShowDialog();
      frmChangeAttribute.Close();
      this.LoadPaymentList();
    }

    private void cmsPacket_Opening(object sender, CancelEventArgs e)
    {
    }

    private void txtSum_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      Decimal num1 = new Decimal();
      IList<Payment> listPayment = (IList<Payment>) new List<Payment>();
      Decimal num2;
      try
      {
        num2 = Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.txtSum.Text));
      }
      catch (Exception ex)
      {
        int num3 = (int) MessageBox.Show("Некорректный формат", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      foreach (DataGridViewRow row in (IEnumerable) this.dgvPayment.Rows)
      {
        if (row.Cells["ClientId"].Value.ToString() != "ИТОГО" && Convert.ToDecimal(row.Cells["PaymentValue"].Value) == num2)
          listPayment.Add((Payment) row.DataBoundItem);
      }
      if (listPayment.Count > 0)
      {
        FrmList frmList = new FrmList(listPayment);
        int num3 = (int) frmList.ShowDialog();
        if (frmList.payment != null)
        {
          foreach (DataGridViewRow row in (IEnumerable) this.dgvPayment.Rows)
          {
            if ((row.Cells["ClientId"].Value.ToString() != "ИТОГО" && Convert.ToInt32(row.Cells["ClientId"].Value) == frmList.payment.LsClient.ClientId || row.Cells["ClientId"].Value.ToString() != "ИТОГО" && Convert.ToInt32(row.Cells["OldId"].Value) == frmList.payment.LsClient.ClientId) && Convert.ToDecimal(row.Cells["PaymentValue"].Value) == num2)
            {
              this.dgvPayment.CurrentCell = row.Cells["ClientId"];
              return;
            }
          }
        }
        frmList.Dispose();
      }
      else
      {
        int num4 = (int) MessageBox.Show("Платеж с такой суммой не найден!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void miPlat_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(34, 1, this.company, true))
        return;
      int Rnn = 0;
      short num1 = 0;
      int Home = 0;
      int Lic = 0;
      if (this.raion != null)
        Rnn = this.raion.IdRnn;
      if (this.company != null)
        num1 = this.company.CompanyId;
      if (this.home != null)
        Home = this.home.IdHome;
      if (this.client != null)
        Lic = this.client.ClientId;
      int num2 = !KvrplHelper.CheckProxy(32, 2, this.company, false) ? 0 : 1;
      int admin = !Options.Kvartplata || !Options.Arenda ? (!Options.Kvartplata ? num2 + 10 : num2 + 0) : num2 + 20;
      this.Cursor = Cursors.WaitCursor;
      this.session = Domain.CurrentSession;
      this.session.Clear();
      try
      {
        CallDll.Rep(Options.City, 0, Rnn, (int) num1, Home, Lic, Convert.ToInt32(((ToolStripItem) sender).Tag), Options.Period.PeriodId, admin, Options.Alias, Options.Login, Options.Pwd);
      }
      catch (Exception ex)
      {
        int num3 = (int) MessageBox.Show("Невозможно вызвать библиотеку отчетов!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      this.Cursor = Cursors.Default;
    }

    private void cbShowAnotherOrg_CheckedChanged(object sender, EventArgs e)
    {
      Options.ShowAnotherOrg = this.cbShowAnotherOrg.Checked;
      this.LoadPaymentList();
    }

    private void txbDog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      string text = this.txbDog.Text;
      foreach (DataGridViewRow row in (IEnumerable) this.dgvPayment.Rows)
      {
        if (row.Cells["ClientId"].Value.ToString() != "ИТОГО" && Convert.ToString(row.Cells["OldId"].Value) == text)
        {
          this.dgvPayment.CurrentCell = row.Cells["ClientId"];
          return;
        }
      }
      int num = (int) MessageBox.Show("Договор не найден!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void cbArhiv_CheckedChanged(object sender, EventArgs e)
    {
      this.LoadPaymentList();
    }

    private void bindingSource1_CurrentChanged(object sender, EventArgs e)
    {
    }

    private void rbLs_Click(object sender, EventArgs e)
    {
      this.LoadPaymentList();
    }

    private void cmbPurposeFilter_SelectedValueChanged(object sender, EventArgs e)
    {
      if ((int) ((PurposePay) this.cmbPurposeFilter.SelectedItem).PurposePayId == 0)
      {
        this.dgvPayment.Columns.Clear();
        this.dgvPayment.DataSource = (object) null;
        this.dgvPayment.DataSource = (object) this.paymentList;
      }
      else
      {
        this.dgvPayment.Columns.Clear();
        this.dgvPayment.DataSource = (object) null;
        this.dgvPayment.DataSource = (object) this.paymentList.Where<Payment>((Func<Payment, bool>) (x => (int) x.PPay.PurposePayId == (int) ((PurposePay) this.cmbPurposeFilter.SelectedItem).PurposePayId)).ToList<Payment>();
      }
      this.SetDgvPaymentsView();
      this.LoadSettingsPayment();
    }

    private void cbReceipt_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.LoadPaymentList();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmPayment));
      this.pnButton = new Panel();
      this.btnReport = new Button();
      this.btnPacket = new Button();
      this.btnEdit = new Button();
      this.btnDelete = new Button();
      this.btnAdd = new Button();
      this.btnExit = new Button();
      this.pnTop = new Panel();
      this.cbReceipt = new ComboBox();
      this.label1 = new Label();
      this.lblPurpose = new Label();
      this.cmbPurposeFilter = new ComboBox();
      this.gbSorter = new GroupBox();
      this.rbPacket = new RadioButton();
      this.rbPaymentDate = new RadioButton();
      this.rbLs = new RadioButton();
      this.cbArhiv = new CheckBox();
      this.txbDog = new TextBox();
      this.lblDog = new Label();
      this.cbShowAnotherOrg = new CheckBox();
      this.txtSum = new TextBox();
      this.lblSum = new Label();
      this.btnSearch = new Button();
      this.txtSearch = new TextBox();
      this.lblSearch = new Label();
      this.dgvPayment = new DataGridView();
      this.cmsPacket = new ContextMenuStrip(this.components);
      this.miSum = new ToolStripMenuItem();
      this.miDelete = new ToolStripMenuItem();
      this.miChange = new ToolStripMenuItem();
      this.cmReport = new ContextMenuStrip(this.components);
      this.miPlat = new ToolStripMenuItem();
      this.hp = new HelpProvider();
      this.mpCurrentPeriod = new MonthPicker();
      this.pnButton.SuspendLayout();
      this.pnTop.SuspendLayout();
      this.gbSorter.SuspendLayout();
      ((ISupportInitialize) this.dgvPayment).BeginInit();
      this.cmsPacket.SuspendLayout();
      this.cmReport.SuspendLayout();
      this.SuspendLayout();
      this.pnButton.Controls.Add((Control) this.btnReport);
      this.pnButton.Controls.Add((Control) this.btnPacket);
      this.pnButton.Controls.Add((Control) this.btnEdit);
      this.pnButton.Controls.Add((Control) this.btnDelete);
      this.pnButton.Controls.Add((Control) this.btnAdd);
      this.pnButton.Controls.Add((Control) this.btnExit);
      this.pnButton.Dock = DockStyle.Bottom;
      this.pnButton.Location = new Point(0, 507);
      this.pnButton.Name = "pnButton";
      this.pnButton.Size = new Size(1138, 40);
      this.pnButton.TabIndex = 1;
      this.btnReport.Image = (Image) Resources.file_text_32;
      this.btnReport.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnReport.Location = new Point(464, 5);
      this.btnReport.Name = "btnReport";
      this.btnReport.Size = new Size(90, 29);
      this.btnReport.TabIndex = 9;
      this.btnReport.Text = "Отчет";
      this.btnReport.TextAlign = ContentAlignment.MiddleRight;
      this.btnReport.UseVisualStyleBackColor = true;
      this.btnReport.Click += new EventHandler(this.btnReport_Click);
      this.btnPacket.Image = (Image) Resources.briefcase;
      this.btnPacket.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnPacket.Location = new Point(373, 5);
      this.btnPacket.Name = "btnPacket";
      this.btnPacket.Size = new Size(85, 30);
      this.btnPacket.TabIndex = 4;
      this.btnPacket.Text = "Пачка";
      this.btnPacket.TextAlign = ContentAlignment.MiddleRight;
      this.btnPacket.UseVisualStyleBackColor = true;
      this.btnPacket.Click += new EventHandler(this.btnPacket_Click);
      this.btnEdit.Image = (Image) Resources.edit;
      this.btnEdit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnEdit.Location = new Point(120, 5);
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(145, 30);
      this.btnEdit.TabIndex = 1;
      this.btnEdit.Text = "Редактировать";
      this.btnEdit.TextAlign = ContentAlignment.MiddleRight;
      this.btnEdit.UseVisualStyleBackColor = true;
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnDelete.Image = (Image) Resources.minus;
      this.btnDelete.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnDelete.Location = new Point(271, 5);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(96, 30);
      this.btnDelete.TabIndex = 2;
      this.btnDelete.Text = "Удалить";
      this.btnDelete.TextAlign = ContentAlignment.MiddleRight;
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnAdd.Image = (Image) Resources.plus;
      this.btnAdd.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnAdd.Location = new Point(12, 5);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(102, 30);
      this.btnAdd.TabIndex = 0;
      this.btnAdd.Text = "Добавить";
      this.btnAdd.TextAlign = ContentAlignment.MiddleRight;
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnEnter_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(1051, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(75, 30);
      this.btnExit.TabIndex = 3;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.pnTop.Controls.Add((Control) this.cbReceipt);
      this.pnTop.Controls.Add((Control) this.label1);
      this.pnTop.Controls.Add((Control) this.lblPurpose);
      this.pnTop.Controls.Add((Control) this.cmbPurposeFilter);
      this.pnTop.Controls.Add((Control) this.gbSorter);
      this.pnTop.Controls.Add((Control) this.cbArhiv);
      this.pnTop.Controls.Add((Control) this.txbDog);
      this.pnTop.Controls.Add((Control) this.lblDog);
      this.pnTop.Controls.Add((Control) this.cbShowAnotherOrg);
      this.pnTop.Controls.Add((Control) this.txtSum);
      this.pnTop.Controls.Add((Control) this.lblSum);
      this.pnTop.Controls.Add((Control) this.btnSearch);
      this.pnTop.Controls.Add((Control) this.mpCurrentPeriod);
      this.pnTop.Controls.Add((Control) this.txtSearch);
      this.pnTop.Controls.Add((Control) this.lblSearch);
      this.pnTop.Dock = DockStyle.Top;
      this.pnTop.Location = new Point(0, 0);
      this.pnTop.Name = "pnTop";
      this.pnTop.Size = new Size(1138, 152);
      this.pnTop.TabIndex = 2;
      this.cbReceipt.FormattingEnabled = true;
      this.cbReceipt.Location = new Point(166, 71);
      this.cbReceipt.Name = "cbReceipt";
      this.cbReceipt.Size = new Size(388, 24);
      this.cbReceipt.TabIndex = 28;
      this.cbReceipt.SelectedIndexChanged += new EventHandler(this.cbReceipt_SelectedIndexChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(11, 72);
      this.label1.Name = "label1";
      this.label1.Size = new Size(142, 16);
      this.label1.TabIndex = 27;
      this.label1.Text = "Выбор по квитанции";
      this.lblPurpose.AutoSize = true;
      this.lblPurpose.Location = new Point(11, 45);
      this.lblPurpose.Name = "lblPurpose";
      this.lblPurpose.Size = new Size(149, 16);
      this.lblPurpose.TabIndex = 26;
      this.lblPurpose.Text = "Назначение платежа";
      this.cmbPurposeFilter.FormattingEnabled = true;
      this.cmbPurposeFilter.Location = new Point(166, 40);
      this.cmbPurposeFilter.Name = "cmbPurposeFilter";
      this.cmbPurposeFilter.Size = new Size(176, 24);
      this.cmbPurposeFilter.TabIndex = 25;
      this.cmbPurposeFilter.SelectedValueChanged += new EventHandler(this.cmbPurposeFilter_SelectedValueChanged);
      this.gbSorter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gbSorter.Controls.Add((Control) this.rbPacket);
      this.gbSorter.Controls.Add((Control) this.rbPaymentDate);
      this.gbSorter.Controls.Add((Control) this.rbLs);
      this.gbSorter.Location = new Point(12, 100);
      this.gbSorter.Name = "gbSorter";
      this.gbSorter.Size = new Size(1114, 45);
      this.gbSorter.TabIndex = 24;
      this.gbSorter.TabStop = false;
      this.gbSorter.Text = "Сортировать";
      this.rbPacket.AutoSize = true;
      this.rbPacket.Location = new Point(404, 19);
      this.rbPacket.Name = "rbPacket";
      this.rbPacket.Size = new Size(138, 20);
      this.rbPacket.TabIndex = 2;
      this.rbPacket.Text = "По номеру пачки";
      this.rbPacket.UseVisualStyleBackColor = true;
      this.rbPacket.Click += new EventHandler(this.rbLs_Click);
      this.rbPaymentDate.AutoSize = true;
      this.rbPaymentDate.Location = new Point(239, 19);
      this.rbPaymentDate.Name = "rbPaymentDate";
      this.rbPaymentDate.Size = new Size(129, 20);
      this.rbPaymentDate.TabIndex = 1;
      this.rbPaymentDate.Text = "По дате оплаты";
      this.rbPaymentDate.UseVisualStyleBackColor = true;
      this.rbPaymentDate.Click += new EventHandler(this.rbLs_Click);
      this.rbLs.AutoSize = true;
      this.rbLs.Checked = true;
      this.rbLs.Location = new Point(6, 19);
      this.rbLs.Name = "rbLs";
      this.rbLs.Size = new Size(202, 20);
      this.rbLs.TabIndex = 0;
      this.rbLs.TabStop = true;
      this.rbLs.Text = "По номеру лицевого счета";
      this.rbLs.UseVisualStyleBackColor = true;
      this.rbLs.Click += new EventHandler(this.rbLs_Click);
      this.cbArhiv.AutoSize = true;
      this.cbArhiv.Location = new Point(700, 41);
      this.cbArhiv.Name = "cbArhiv";
      this.cbArhiv.Size = new Size(66, 20);
      this.cbArhiv.TabIndex = 24;
      this.cbArhiv.Text = "Архив";
      this.cbArhiv.UseVisualStyleBackColor = true;
      this.cbArhiv.CheckedChanged += new EventHandler(this.cbArhiv_CheckedChanged);
      this.txbDog.Location = new Point(458, 9);
      this.txbDog.Name = "txbDog";
      this.txbDog.Size = new Size(96, 22);
      this.txbDog.TabIndex = 23;
      this.txbDog.KeyDown += new KeyEventHandler(this.txbDog_KeyDown);
      this.lblDog.AutoSize = true;
      this.lblDog.Location = new Point(395, 12);
      this.lblDog.Name = "lblDog";
      this.lblDog.Size = new Size(63, 16);
      this.lblDog.TabIndex = 22;
      this.lblDog.Text = "Договор";
      this.cbShowAnotherOrg.AutoSize = true;
      this.cbShowAnotherOrg.Location = new Point(355, 41);
      this.cbShowAnotherOrg.Name = "cbShowAnotherOrg";
      this.cbShowAnotherOrg.Size = new Size(339, 20);
      this.cbShowAnotherOrg.TabIndex = 21;
      this.cbShowAnotherOrg.Text = "Показать информацию сторонних организаций";
      this.cbShowAnotherOrg.UseVisualStyleBackColor = true;
      this.cbShowAnotherOrg.CheckedChanged += new EventHandler(this.cbShowAnotherOrg_CheckedChanged);
      this.txtSum.Location = new Point(280, 9);
      this.txtSum.Name = "txtSum";
      this.txtSum.Size = new Size(100, 22);
      this.txtSum.TabIndex = 17;
      this.txtSum.KeyDown += new KeyEventHandler(this.txtSum_KeyDown);
      this.lblSum.AutoSize = true;
      this.lblSum.Location = new Point(233, 12);
      this.lblSum.Name = "lblSum";
      this.lblSum.Size = new Size(51, 16);
      this.lblSum.TabIndex = 16;
      this.lblSum.Text = "Сумма";
      this.btnSearch.Image = (Image) Resources.search_24;
      this.btnSearch.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSearch.Location = new Point(573, 5);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new Size(83, 30);
      this.btnSearch.TabIndex = 15;
      this.btnSearch.Text = "Поиск";
      this.btnSearch.TextAlign = ContentAlignment.MiddleRight;
      this.btnSearch.UseVisualStyleBackColor = true;
      this.btnSearch.Click += new EventHandler(this.button1_Click);
      this.txtSearch.Location = new Point(120, 9);
      this.txtSearch.Name = "txtSearch";
      this.txtSearch.Size = new Size(96, 22);
      this.txtSearch.TabIndex = 13;
      this.txtSearch.KeyDown += new KeyEventHandler(this.txtSearch_KeyDown);
      this.lblSearch.AutoSize = true;
      this.lblSearch.Location = new Point(9, 12);
      this.lblSearch.Name = "lblSearch";
      this.lblSearch.Size = new Size(108, 16);
      this.lblSearch.TabIndex = 12;
      this.lblSearch.Text = "Найти лицевой";
      this.dgvPayment.BackgroundColor = Color.AliceBlue;
      this.dgvPayment.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvPayment.Dock = DockStyle.Fill;
      this.dgvPayment.Location = new Point(0, 152);
      this.dgvPayment.Margin = new Padding(4);
      this.dgvPayment.Name = "dgvPayment";
      this.dgvPayment.ReadOnly = true;
      this.dgvPayment.Size = new Size(1138, 355);
      this.dgvPayment.TabIndex = 0;
      this.dgvPayment.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvPayment_CellFormatting);
      this.dgvPayment.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dgvPayment_CellMouseDoubleClick_1);
      this.dgvPayment.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvPayment_ColumnWidthChanged);
      this.dgvPayment.DataError += new DataGridViewDataErrorEventHandler(this.dgvPayment_DataError);
      this.dgvPayment.DefaultValuesNeeded += new DataGridViewRowEventHandler(this.dgvPayment_DefaultValuesNeeded);
      this.cmsPacket.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.miSum,
        (ToolStripItem) this.miDelete,
        (ToolStripItem) this.miChange
      });
      this.cmsPacket.Name = "cmsPacket";
      this.cmsPacket.Size = new Size(220, 70);
      this.cmsPacket.Opening += new CancelEventHandler(this.cmsPacket_Opening);
      this.miSum.Name = "miSum";
      this.miSum.Size = new Size(219, 22);
      this.miSum.Text = "Сведения по пачке";
      this.miSum.Click += new EventHandler(this.btnPacketSum_Click);
      this.miDelete.Name = "miDelete";
      this.miDelete.Size = new Size(219, 22);
      this.miDelete.Text = "Удалить пачку";
      this.miDelete.Click += new EventHandler(this.miDelete_Click);
      this.miChange.Name = "miChange";
      this.miChange.Size = new Size(219, 22);
      this.miChange.Text = "Изменить атрибуты пачки";
      this.miChange.Click += new EventHandler(this.miChange_Click);
      this.cmReport.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.miPlat
      });
      this.cmReport.Name = "cmReport";
      this.cmReport.Size = new Size(267, 26);
      this.miPlat.Name = "miPlat";
      this.miPlat.Size = new Size(266, 22);
      this.miPlat.Tag = (object) "30";
      this.miPlat.Text = "Ведомость поступивших платежей";
      this.miPlat.Click += new EventHandler(this.miPlat_Click);
      this.hp.HelpNamespace = "Help.chm";
      this.mpCurrentPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.mpCurrentPeriod.CustomFormat = "MMMM yyyy";
      this.mpCurrentPeriod.Format = DateTimePickerFormat.Custom;
      this.mpCurrentPeriod.Location = new Point(986, 9);
      this.mpCurrentPeriod.Name = "mpCurrentPeriod";
      this.mpCurrentPeriod.OldMonth = 0;
      this.mpCurrentPeriod.ShowUpDown = true;
      this.mpCurrentPeriod.Size = new Size(140, 22);
      this.mpCurrentPeriod.TabIndex = 14;
      this.mpCurrentPeriod.ValueChanged += new EventHandler(this.dtmpCurrentPeriod_ValueChanged);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(1138, 547);
      this.Controls.Add((Control) this.dgvPayment);
      this.Controls.Add((Control) this.pnButton);
      this.Controls.Add((Control) this.pnTop);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.hp.SetHelpKeyword((Control) this, "kv71.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmPayment";
      this.hp.SetShowHelp((Control) this, true);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Платежи";
      this.Load += new EventHandler(this.FrmPayment_Load);
      this.pnButton.ResumeLayout(false);
      this.pnTop.ResumeLayout(false);
      this.pnTop.PerformLayout();
      this.gbSorter.ResumeLayout(false);
      this.gbSorter.PerformLayout();
      ((ISupportInitialize) this.dgvPayment).EndInit();
      this.cmsPacket.ResumeLayout(false);
      this.cmReport.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
