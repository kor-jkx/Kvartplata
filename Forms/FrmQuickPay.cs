// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmQuickPay
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using NHibernate;
using NHibernate.Criterion;
using SaveSettings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmQuickPay : FrmBaseForm
  {
    protected GridSettings MySettingsRent = new GridSettings();
    private FormStateSaver fss = new FormStateSaver(FrmQuickPay.ic);
    private IContainer components = (IContainer) null;
    private LsClient client;
    private ISession session;
    private static IContainer ic;
    private Decimal payItog;
    private Decimal mainSumma;
    private IList<Counter> counters;
    private Period monthClosed;
    private Panel pnBtn;
    private Button btnExit;
    private ComboBox cmbFlat;
    private ComboBox cmbStreet;
    private Label lblStreet;
    private Label lblClient;
    private GroupBox gbAdr;
    private Label lblFlat;
    private Label lblHome;
    private ComboBox cmbHome;
    private TextBox txtClient;
    private MonthPicker mpCurrentPeriod;
    private Label lblSumma;
    private TextBox txbSumma;
    private DataGridView dgvRent;
    private Button btnSave;
    private Button btnCalc;
    private Button btnCommission;
    private MaskedTextBox txbPacket;
    private Label lblPacket;
    private ComboBox cmbSource;
    private Label lblSource;
    private Label lblFIO;
    private DataGridView dgvEvidence;

    public FrmQuickPay()
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
    }

    public FrmQuickPay(LsClient client)
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
      this.client = client;
    }

    [DllImport("Receipts.dll", CharSet = CharSet.Ansi, SetLastError = true)]
    public static extern void WReceipt(int Level, int Raion, int Company, int City, int Home, int Lic, double GlMec, string connect, string UserS, string Passwd, bool Arenda, bool Show);

    [DllImport("Compute.dll", EntryPoint = "QRent", CharSet = CharSet.Ansi, SetLastError = true)]
    public static extern void Rent(int period, [MarshalAs(UnmanagedType.LPStr)] string sclient, [MarshalAs(UnmanagedType.LPStr)] string connect, [MarshalAs(UnmanagedType.LPStr)] string UserS, [MarshalAs(UnmanagedType.LPStr)] string Passwd);

    private void FrmQuickPay_Load(object sender, EventArgs e)
    {
      this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
      InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("ru-RU"));
      this.session = Domain.CurrentSession;
      IList<Str> strList = this.session.CreateQuery("from Str where IdStr in (select Str.IdStr from Home) order by NameStr").List<Str>();
      strList.Insert(0, new Str()
      {
        IdStr = 0,
        NameStr = ""
      });
      this.cmbStreet.DataSource = (object) strList;
      this.cmbStreet.DisplayMember = "NameStr";
      this.cmbStreet.ValueMember = "IdStr";
      this.session.Clear();
      Graphics graphics = this.CreateGraphics();
      if ((double) graphics.DpiX > 96.0)
        this.Font = new Font("Sans Serif", 9f, FontStyle.Regular);
      graphics.Dispose();
      this.MySettingsRent.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
      IList list = this.session.CreateCriteria(typeof (SourcePay)).AddOrder(Order.Asc("SourcePayName")).List();
      if ((uint) list.Count > 0U)
      {
        this.cmbSource.DataSource = (object) list;
        this.cmbSource.ValueMember = "SourcePayId";
        this.cmbSource.DisplayMember = "SourcePayName";
      }
      if (Options.SourcePay != null)
        this.cmbSource.SelectedValue = (object) Options.SourcePay.SourcePayId;
      if (Options.Packet == null)
        this.cmbSource_SelectionChangeCommitted((object) null, (EventArgs) null);
      else
        this.txbPacket.Text = Options.Packet;
      if (this.client == null)
        return;
      this.Cursor = Cursors.WaitCursor;
      this.txtClient.Text = this.client.ClientId.ToString();
      SendKeys.Send("{ENTER}");
      this.Cursor = Cursors.Default;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void txtClient_KeyDown(object sender, KeyEventArgs e)
    {
      if (this.txtClient.Text.Length == 1)
      {
        this.cmbStreet.SelectedIndex = 0;
        this.cmbHome.DataSource = (object) null;
        this.cmbFlat.DataSource = (object) null;
      }
      if (e.KeyCode != Keys.Return && e.KeyCode != Keys.Tab)
        return;
      if (this.txtClient.Text != "")
      {
        try
        {
          this.client = KvrplHelper.FindLs(Convert.ToInt32(this.txtClient.Text));
        }
        catch
        {
          int num = (int) MessageBox.Show("Лицевой счет не введен или введен неверно.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.txtClient.Focus();
          return;
        }
        if (this.client != null)
        {
          this.Cursor = Cursors.WaitCursor;
          this.ShowInfo();
          this.ShowLicInfo();
          this.LoadCounters();
          this.Cursor = Cursors.Default;
        }
        else
        {
          int num1 = (int) MessageBox.Show("Лицевой счет не найден", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
      }
      else
        this.cmbStreet.Focus();
    }

    private void txtClient_KeyPress(object sender, KeyPressEventArgs e)
    {
      if ((int) e.KeyChar == 8 || (int) e.KeyChar == 13 || (int) e.KeyChar == 45 || (int) e.KeyChar >= 48 && (int) e.KeyChar <= 57)
        return;
      e.Handled = true;
    }

    private void cmbStreet_KeyDown(object sender, KeyEventArgs e)
    {
      if (this.cmbStreet.Text.Length >= 1)
        this.txtClient.Clear();
      if (e.KeyCode != Keys.Return || !(this.txtClient.Text == "") || !(this.cmbStreet.Text == ""))
        return;
      this.txtClient.Focus();
    }

    private void cmbStreet_SelectedValueChanged(object sender, EventArgs e)
    {
      if (!(this.cmbStreet.Text != ""))
        return;
      this.cmbStreet_SelectionChangeCommitted((object) null, (EventArgs) null);
      this.cmbHome.Focus();
    }

    private void cmbStreet_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.cmbHome.DataSource = (object) null;
      this.cmbFlat.DataSource = (object) null;
      this.session = Domain.CurrentSession;
      Str selectedItem = (Str) this.cmbStreet.SelectedItem;
      IList<Home> homeList = this.session.CreateQuery(string.Format("select distinct new Home(h.IdHome,h.NHome,h.HomeKorp) from Home h where h.Str.IdStr={0} order by DBA.LENGTHHOME(h.NHome),DBA.LENGTHHOME(h.HomeKorp)", (object) (selectedItem != null ? selectedItem.IdStr : 0))).List<Home>();
      homeList.Insert(0, new Home()
      {
        IdHome = 0,
        NHome = ""
      });
      this.cmbHome.DataSource = (object) homeList;
      this.cmbHome.DisplayMember = "NHome";
      this.cmbHome.ValueMember = "IdHome";
      this.session.Clear();
    }

    private void cmbHome_SelectedValueChanged(object sender, EventArgs e)
    {
      if (!(this.cmbHome.Text != "") || !(this.cmbHome.Text != "Counters.Classes.Home"))
        return;
      this.cmbHome_SelectionChangeCommitted((object) null, (EventArgs) null);
      this.cmbFlat.Focus();
    }

    private void cmbHome_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.cmbFlat.DataSource = (object) null;
      this.session = Domain.CurrentSession;
      Home selectedItem = (Home) this.cmbHome.SelectedItem;
      IList<Flat> flatList = this.session.CreateQuery(string.Format("select f from Flat f where f.Home.IdHome={0} order by DBA.LENGTHHOME(f.NFlat)", (object) (selectedItem != null ? selectedItem.IdHome : 0))).List<Flat>();
      flatList.Insert(0, new Flat()
      {
        IdFlat = 0,
        NFlat = ""
      });
      this.cmbFlat.DataSource = (object) flatList;
      this.cmbFlat.DisplayMember = "NFlat";
      this.cmbFlat.ValueMember = "IdFlat";
      this.session.Clear();
    }

    private void cmbFlat_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return || (!(this.cmbFlat.Text != "") || !(this.cmbFlat.Text != "Counters.Classes.Flat")))
        return;
      this.session = Domain.CurrentSession;
      try
      {
        this.client = this.session.CreateCriteria(typeof (LsClient)).Add((ICriterion) Restrictions.Eq("Flat", (object) (Flat) this.cmbFlat.SelectedItem)).List<LsClient>()[0];
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Лицевой счет не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      this.session.Clear();
      this.Cursor = Cursors.WaitCursor;
      this.ShowLicInfo();
      this.ShowInfo();
      this.LoadCounters();
      this.Cursor = Cursors.Default;
    }

    private void cmbFlat_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      Flat flat = new Flat();
      Flat selectedItem;
      try
      {
        this.client = this.session.CreateCriteria(typeof (LsClient)).Add((ICriterion) Restrictions.Eq("Flat", (object) (Flat) this.cmbFlat.SelectedItem)).List<LsClient>()[0];
        selectedItem = (Flat) this.cmbFlat.SelectedItem;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Лицевой счет не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      this.session.Clear();
      this.Cursor = Cursors.WaitCursor;
      this.ShowLicInfo();
      this.ShowInfo();
      this.LoadCounters();
      this.Cursor = Cursors.Default;
      try
      {
        this.cmbFlat.SelectedValue = (object) selectedItem.IdFlat;
      }
      catch
      {
      }
    }

    private void ShowInfo()
    {
      this.payItog = new Decimal();
      this.txbSumma.Focus();
      IList list1 = this.session.CreateSQLQuery(string.Format("select serv.service_id,service_name,supplier_id,(select nameorg_min from base_org where idbaseorg=supplier_id) as nameorg, balin,rent,rentpast,    (select sum(rent) from DBA.lsRentMSP rm,DBA.dcMSP m where rm.msp_id=m.msp_id and rm.period_id={0} and client_id={1} and code=0 and rm.service_id in (select service_id from DBA.dcService where root=serv.service_id)) as msp,    (select sum(rent) from DBA.lsRentMSP rm,DBA.dcMSP m where rm.msp_id=m.msp_id and rm.period_id={0} and client_id={1} and code=0 and m.mspperiod_id<={0} and rm.service_id in (select service_id from DBA.dcService where root=serv.service_id)) as msppay,    (select sum(rent) from DBA.lsRentMSP rm,DBA.dcMSP m where rm.msp_id=m.msp_id and rm.period_id={0} and client_id={1} and code<>0 and rm.service_id in (select service_id from DBA.dcService where root=serv.service_id)) as msppast,    (select sum(rent) from DBA.lsRentMSP rm,DBA.dcMSP m where rm.msp_id=m.msp_id and rm.period_id={0} and client_id={1} and code<>0 and m.mspperiod_id<={0} and rm.service_id in (select service_id from DBA.dcService where root=serv.service_id)) as msppastpay,    isnull(rent,0)+isnull(rentpast,0)-isnull(msp,0)+isnull(msppay,0)-isnull(msppast,0)+isnull(msppastpay,0)+isnull(rentcomp,0) as preditog,(if preditog<>0 then preditog else null endif) as itog,pay,balout,0 as s from DBA.dcService serv left outer join     (select root,sum(b.Balance_in) as balin, sum(b.Rent)-sum(b.Rent_past) as rent,sum(b.Rent_past) as rentpast,    sum(b.Payment) as pay,sum(b.Subsidy) as subs,sum(b.Balance_out) as balout,sum(b.Rent_comp) as rentcomp,b.supplier_id from DBA.lsBalance b, DBA.dcService s where b.service_id=s.service_id and b.period_id={0} and b.client_id={1} group by root, b.supplier_id) as my on my.root=serv.service_id, DBA.cmpServiceParam sp where serv.root=0 and sp.service_id=serv.service_id and sp.company_id={2}     and (isnull(balin,0)<>0 or isnull(rent,0)<>0 or isnull(msp,0)<>0 or isnull(msppay,0)<>0 or isnull(itog,0)<>0 or isnull(pay,0)<>0 or isnull(balout,0)<>0) order by serv.service_id,supplier_id", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) this.client.Company.CompanyId)).List();
      DataTable dataTable = new DataTable("Balance");
      dataTable.Columns.Add("№", System.Type.GetType("System.Int32"));
      dataTable.Columns.Add("Услуга", System.Type.GetType("System.String"));
      dataTable.Columns.Add("№ пост", System.Type.GetType("System.Int32"));
      dataTable.Columns.Add("Поставщик", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Входящее", System.Type.GetType("System.Decimal"));
      dataTable.Columns.Add("Начислено", System.Type.GetType("System.Decimal"));
      dataTable.Columns.Add("Перер-т", System.Type.GetType("System.Decimal"));
      dataTable.Columns.Add("МСП", System.Type.GetType("System.Decimal"));
      dataTable.Columns.Add("МСП выплачено", System.Type.GetType("System.Decimal"));
      dataTable.Columns.Add("Перер-т по МСП", System.Type.GetType("System.Decimal"));
      dataTable.Columns.Add("Перер-т по МСП выплачено", System.Type.GetType("System.Decimal"));
      dataTable.Columns.Add("Предварительный итог", System.Type.GetType("System.Decimal"));
      dataTable.Columns.Add("Итого начисл.", System.Type.GetType("System.Decimal"));
      dataTable.Columns.Add("Оплачено", System.Type.GetType("System.Decimal"));
      dataTable.Columns.Add("Исходящее", System.Type.GetType("System.Decimal"));
      dataTable.Columns.Add("Сумма оплаты", System.Type.GetType("System.String"));
      IList list2 = this.session.CreateQuery(string.Format("select sum(b.BalanceIn) as balin,sum(b.Rent)-sum(b.RentPast) as rent,sum(b.RentPast) as rentpast,(select sum(rm.RentMain) from RentMSP rm where rm.Period.PeriodId={0} and rm.LsClient.ClientId={1} and rm.Code=0) as msp,   (select sum(rm.RentMain) from RentMSP rm where rm.Period.PeriodId={0} and rm.LsClient.ClientId={1} and rm.Code<>0) as msppast,   sum(b.Rent)-isnull((select sum(rm.RentMain) from RentMSP rm where rm.Period.PeriodId={0} and rm.LsClient.ClientId={1} and rm.Code=0 and rm.MSP.MSP_id in (select MSP_id from DcMSP where MSPPeriod.PeriodId>{0})),0)-isnull((select sum(rm.RentMain) from RentMSP rm where rm.Period.PeriodId={0} and rm.LsClient.ClientId={1} and rm.Code<>0 and rm.MSP.MSP_id in (select MSP_id from DcMSP where MSPPeriod.PeriodId>{0})),0)+sum(b.RentComp) as itog,sum(b.Payment) as pay,sum(b.BalanceOut) as balout,   (select sum(o.Pay) from Overpay o where o.Period.PeriodId={0} and o.LsClient.ClientId={1} and o.Code<10) as overpay, sum(b.RentComp) as rentcomp from Balance b where b.Period.PeriodId={0} and b.LsClient.ClientId={1}", (object) Options.Period.PeriodId, (object) this.client.ClientId)).List();
      object[] objArray1 = new object[16];
      objArray1[1] = (object) "ИТОГО";
      objArray1[4] = (object) Convert.ToDecimal(((object[]) list2[0])[0]);
      objArray1[5] = (object) Convert.ToDecimal(((object[]) list2[0])[1]);
      objArray1[6] = (object) Convert.ToDecimal(((object[]) list2[0])[2]);
      objArray1[13] = (object) Convert.ToDecimal(((object[]) list2[0])[6]);
      objArray1[14] = (object) Convert.ToDecimal(((object[]) list2[0])[7]);
      objArray1[15] = (object) 0;
      foreach (object[] objArray2 in (IEnumerable) list1)
        dataTable.Rows.Add(objArray2);
      dataTable.Rows.Add(objArray1);
      this.dgvRent.DataSource = (object) dataTable;
      this.session.Clear();
      this.SetViewRent();
      this.MySettingsRent.GridName = "RentNevin";
      this.LoadSettingsRent();
    }

    private void LoadSettingsRent()
    {
      this.MySettingsRent.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvRent.Columns)
        this.MySettingsRent.GetMySettings(column);
    }

    private void dgvRent_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsRent.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsRent.Columns[this.MySettingsRent.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsRent.Save();
    }

    private void SetViewRent()
    {
      DataGridViewCellStyle gridViewCellStyle = new DataGridViewCellStyle();
      gridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvRent.Columns)
      {
        if (column.Index != 0 && column.Index != 1 && column.Index != 3)
        {
          column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight;
          column.HeaderCell.Style = gridViewCellStyle;
          column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        if (column.Name != "Сумма оплаты")
          column.ReadOnly = true;
        else
          column.DefaultCellStyle.Format = "C2";
        if (column.Name == "Исходящее" || column.Name == "Сумма оплаты")
          column.DefaultCellStyle.Font = new Font(this.dgvRent.Font, FontStyle.Bold);
      }
      this.dgvRent.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft;
      this.dgvRent.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
      this.dgvRent.Columns[2].Visible = false;
      this.dgvRent.Columns[7].Visible = false;
      this.dgvRent.Columns[8].Visible = false;
      this.dgvRent.Columns[9].Visible = false;
      this.dgvRent.Columns[10].Visible = false;
      this.dgvRent.Columns[11].Visible = false;
      this.dgvRent.Columns[12].Visible = false;
      this.dgvRent.Rows[this.dgvRent.Rows.Count - 2].ReadOnly = true;
    }

    private void mpCurrentPeriod_ValueChanged(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      Options.Period = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) this.mpCurrentPeriod.Value)).List<Period>()[0];
      this.session.Clear();
      if (this.client == null)
        return;
      this.ShowInfo();
      if (this.lblFIO.Text != "")
        this.LoadCounters();
    }

    private void txbSumma_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return && e.KeyCode != Keys.Tab || this.dgvRent.Rows.Count <= 0)
        return;
      if (this.txbSumma.Text != "")
      {
        foreach (DataGridViewRow row in (IEnumerable) this.dgvRent.Rows)
        {
          if (row.Index != this.dgvRent.Rows.Count - 1)
            row.Cells["Сумма оплаты"].Value = (object) "0";
        }
        try
        {
          this.mainSumma = Convert.ToDecimal(this.txbSumma.Text);
          if (this.mainSumma == Convert.ToDecimal(((object[]) this.session.CreateQuery(string.Format("select sum(b.BalanceIn) as balin,sum(b.Rent)-sum(b.RentPast) as rent,sum(b.RentPast) as rentpast,(select sum(rm.RentMain) from RentMSP rm where rm.Period.PeriodId={0} and rm.LsClient.ClientId={1} and rm.Code=0) as msp,   (select sum(rm.RentMain) from RentMSP rm where rm.Period.PeriodId={0} and rm.LsClient.ClientId={1} and rm.Code<>0) as msppast,   sum(b.Rent)-isnull((select sum(rm.RentMain) from RentMSP rm where rm.Period.PeriodId={0} and rm.LsClient.ClientId={1} and rm.Code=0 and rm.MSP.MSP_id in (select MSP_id from DcMSP where MSPPeriod.PeriodId>{0})),0)-isnull((select sum(rm.RentMain) from RentMSP rm where rm.Period.PeriodId={0} and rm.LsClient.ClientId={1} and rm.Code<>0 and rm.MSP.MSP_id in (select MSP_id from DcMSP where MSPPeriod.PeriodId>{0})),0)+sum(b.RentComp) as itog,sum(b.Payment) as pay,sum(b.BalanceOut) as balout,   (select sum(o.Pay) from Overpay o where o.Period.PeriodId={0} and o.LsClient.ClientId={1} and o.Code<10) as overpay, sum(b.RentComp) as rentcomp from Balance b where b.Period.PeriodId={0} and b.LsClient.ClientId={1}", (object) Options.Period.PeriodId, (object) this.client.ClientId)).List()[0])[7]))
          {
            foreach (DataGridViewRow row in (IEnumerable) this.dgvRent.Rows)
              row.Cells["Сумма оплаты"].Value = row.Cells["Исходящее"].Value;
            this.txbSumma.Text = "";
          }
          else
          {
            int num = (int) MessageBox.Show("Разнесите сумму платежа самостоятельно", "", MessageBoxButtons.OK);
          }
        }
        catch
        {
          return;
        }
      }
      this.dgvRent.Focus();
      this.dgvRent.CurrentCell = this.dgvRent.Rows[0].Cells["Сумма оплаты"];
    }

    private void txbSumma_KeyPress(object sender, KeyPressEventArgs e)
    {
      if ((int) e.KeyChar == 8 || (int) e.KeyChar == 13 || ((int) e.KeyChar == 44 || (int) e.KeyChar == 45) || (int) e.KeyChar == 46 || (int) e.KeyChar >= 48 && (int) e.KeyChar <= 57)
        return;
      e.Handled = true;
    }

    private void dgvRent_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (((DataGridView) sender).DataSource == null)
        return;
      DataGridViewRow row = this.dgvRent.Rows[e.RowIndex];
      if (row.Cells[1].Value != null && row.Cells[1].Value.ToString() == "ИТОГО")
      {
        row.DefaultCellStyle.Font = new Font(this.dgvRent.Font, FontStyle.Bold);
        row.DefaultCellStyle.BackColor = Color.PapayaWhip;
      }
    }

    private void dgvRent_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgvRent.CurrentRow == null)
        return;
      if (this.dgvRent.CurrentRow.Cells["Сумма оплаты"].Value != null)
      {
        try
        {
          this.payItog = new Decimal();
          foreach (DataGridViewRow row in (IEnumerable) this.dgvRent.Rows)
          {
            if (row.Cells["Сумма оплаты"].Value != null && row.Cells["Услуга"].Value != null && row.Cells["Услуга"].Value.ToString() != "ИТОГО" && Convert.ToDecimal(row.Cells["Сумма оплаты"].Value) != Decimal.Zero)
              this.payItog = this.payItog + Convert.ToDecimal(row.Cells["Сумма оплаты"].Value);
          }
          this.dgvRent.Rows[this.dgvRent.Rows.Count - 2].Cells["Сумма оплаты"].Value = (object) this.payItog;
          this.txbSumma.Text = Convert.ToString(this.mainSumma - this.payItog);
        }
        catch
        {
          this.dgvRent.CurrentRow.Cells["Сумма оплаты"].Value = (object) 0;
        }
      }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.client == null || this.dgvRent.Rows.Count == 2)
        return;
      if (Options.Period.PeriodId <= this.monthClosed.PeriodId)
      {
        int num1 = (int) MessageBox.Show("Невозможно сохранить запись в закрытом периоде", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (MessageBox.Show("Сохранить внесенный платеж и показания?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
          return;
        this.session.Clear();
        using (ITransaction transaction = this.session.BeginTransaction())
        {
          try
          {
            foreach (DataGridViewRow row in (IEnumerable) this.dgvRent.Rows)
            {
              if (row.Cells["Сумма оплаты"].Value != null && row.Cells["Услуга"].Value != null && row.Cells["Услуга"].Value.ToString() != "ИТОГО" && Convert.ToDecimal(row.Cells["Сумма оплаты"].Value) != Decimal.Zero)
              {
                Payment payment = new Payment();
                IList<int> intList = this.session.CreateSQLQuery("select DBA.gen_id('lsPayment',1)").List<int>();
                payment.PaymentId = intList[0];
                payment.DEdit = DateTime.Now.Date;
                payment.LsClient = this.client;
                payment.PacketNum = this.txbPacket.Text;
                payment.PaymentDate = DateTime.Now.Date;
                payment.PaymentPeni = Decimal.Zero;
                payment.PaymentValue = Convert.ToDecimal(row.Cells["Сумма оплаты"].Value);
                payment.Period = Options.Period;
                payment.PeriodPay = Options.Period;
                payment.PPay = this.session.Get<PurposePay>((object) Convert.ToInt16(1));
                payment.Receipt = this.session.Get<Receipt>((object) Convert.ToInt16(1));
                payment.Service = this.session.Get<Service>((object) Convert.ToInt16(row.Cells["№"].Value));
                payment.SPay = (SourcePay) this.cmbSource.SelectedItem;
                payment.Supplier = this.session.Get<Supplier>((object) Convert.ToInt32(row.Cells["№ пост"].Value));
                payment.UName = Options.Login;
                this.session.Save((object) payment);
              }
            }
            this.session.Flush();
            transaction.Commit();
            try
            {
              this.SaveEvidence();
            }
            catch
            {
              int num2 = (int) MessageBox.Show("Не удалось сохранить показания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            if (MessageBox.Show("Рассчитать лицевой?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
              this.btnCalc_Click((object) null, (EventArgs) null);
            if (MessageBox.Show("Вывести поручение на экран?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
              this.btnCommission_Click((object) null, (EventArgs) null);
            this.LoadCounters();
          }
          catch (Exception ex)
          {
            int num2 = (int) MessageBox.Show("Не удалось сохранить платеж", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            transaction.Rollback();
          }
        }
      }
    }

    private void btnCalc_Click(object sender, EventArgs e)
    {
      if (this.client == null || !KvrplHelper.CheckProxy(35, 2, this.client.Company, true))
        return;
      this.Cursor = Cursors.WaitCursor;
      try
      {
        FrmQuickPay.Rent(Options.Period.PeriodId, "and client_id in (" + this.client.ClientId.ToString() + ")", Options.Alias, Options.Login, Options.Pwd);
        this.ShowInfo();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Невозможно вызвать библиотеку расчета!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      this.Cursor = Cursors.Default;
    }

    private void btnCommission_Click(object sender, EventArgs e)
    {
      if (this.client == null || !KvrplHelper.CheckProxy(34, 1, this.client.Company, true))
        return;
      this.Cursor = Cursors.WaitCursor;
      this.session = Domain.CurrentSession;
      Raion raion = this.session.CreateQuery(string.Format("select c.Raion from Company c where c.CompanyId={0}", (object) this.client.Company.CompanyId)).List<Raion>()[0];
      this.session.Clear();
      try
      {
        FrmQuickPay.WReceipt(4, raion.IdRnn, (int) this.client.Company.CompanyId, Options.City, this.client.Home.IdHome, this.client.ClientId, Options.Period.PeriodName.Value.ToOADate(), Options.Alias, Options.Login, Options.Pwd, false, false);
      }
      catch
      {
        int num = (int) MessageBox.Show("Невозможно вызвать библиотеку печати!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      this.Cursor = Cursors.Default;
    }

    private void cmbSource_SelectionChangeCommitted(object sender, EventArgs e)
    {
      DateTime now = DateTime.Now;
      DateTime? periodName = Options.Period.PeriodName;
      if (periodName.HasValue && now < periodName.GetValueOrDefault())
        this.txbPacket.Text = Convert.ToString(Convert.ToInt32(this.cmbSource.SelectedValue) * 100 + DateTime.Now.Day + 50);
      else
        this.txbPacket.Text = Convert.ToString(Convert.ToInt32(this.cmbSource.SelectedValue) * 100 + DateTime.Now.Day);
    }

    private void ShowLicInfo()
    {
      this.lblFIO.Text = "";
      Address address1 = new Address();
      Address address2 = this.session.CreateQuery(string.Format("select new Address(c.ClientId,d.NameStr,h.NHome,h.HomeKorp,f.NFlat,c.SurFlat) from Home h, Str d, Flat f,LsClient c where h.Str=d and c.Home=h and c.Flat=f and c.ClientId={0} ", (object) this.client.ClientId)).List<Address>()[0];
      Label lblFio = this.lblFIO;
      string[] strArray = new string[5];
      strArray[0] = this.client.Fio;
      strArray[1] = " ";
      int index1 = 2;
      string str1 = string.Format("{0} д.{1} {2} кв.{3} {4}", (object) address2.Str, (object) address2.Number, (object) (address2.Korp == "" || address2.Korp == "0" || address2.Korp == null ? "" : "к." + address2.Korp), (object) address2.Flat, (object) (address2.SurFlat == "" || address2.SurFlat == "0" || address2.SurFlat == null ? "" : "комн." + address2.SurFlat));
      strArray[index1] = str1;
      int index2 = 3;
      string str2 = "  №";
      strArray[index2] = str2;
      int index3 = 4;
      string str3 = this.client.ClientId.ToString();
      strArray[index3] = str3;
      string str4 = string.Concat(strArray);
      lblFio.Text = str4;
      this.monthClosed = KvrplHelper.GetCmpKvrClose(this.client.Company, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk);
    }

    private void LoadCounters()
    {
      this.dgvEvidence.Columns.Clear();
      this.dgvEvidence.DataSource = (object) null;
      this.session.Clear();
      this.counters = (IList<Counter>) new List<Counter>();
      this.counters = this.session.CreateQuery(string.Format("select c from Counter c left join fetch c.Service where c.Complex.ComplexId={0} and c.LsClient.ClientId={1} and c.BaseCounter.Id=2 order by isnull(c.ArchivesDate,'2999-12-31') desc,c.Service.ServiceId,regulatefld(c.CounterNum)", (object) Options.Complex.ComplexId, (object) this.client.ClientId)).List<Counter>();
      IList<Evidence> evidenceList1 = (IList<Evidence>) new List<Evidence>();
      foreach (Counter counter in (IEnumerable<Counter>) this.counters)
      {
        IList<Evidence> evidenceList2 = this.session.CreateCriteria(typeof (Evidence)).Add((ICriterion) Restrictions.Eq("Period", (object) Options.Period)).Add((ICriterion) Restrictions.Eq("Counter", (object) counter)).AddOrder(Order.Asc("DBeg")).List<Evidence>();
        if (evidenceList2.Count > 0)
        {
          foreach (Evidence evidence in (IEnumerable<Evidence>) evidenceList2)
            evidenceList1.Add(evidence);
        }
        else
        {
          DateTime? nullable = Options.Period.PeriodName;
          DateTime dateTime1 = nullable.Value;
          nullable = KvrplHelper.GetKvrClose(this.client.ClientId, Options.Complex, Options.ComplexPrior).PeriodName;
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
            IList<Evidence> evidenceList3 = this.session.CreateQuery(string.Format("select cp from Evidence cp where cp.Counter.CounterId={1} and cp.DBeg=(select max(DBeg) from Evidence where Counter.CounterId=cp.Counter.CounterId and Period.PeriodId<{0}) order by cp.Period.PeriodId desc,cp.DEnd desc", (object) Options.Period.PeriodId, (object) counter.CounterId)).List<Evidence>();
            Evidence evidence1 = new Evidence();
            if (evidenceList3.Count > 0)
            {
              evidence1 = evidenceList3[0];
              evidence1.Period = (Period) null;
              Evidence evidence2 = evidence1;
              dend = evidence1.DEnd;
              DateTime dateTime3 = dend.AddDays(1.0);
              evidence2.DBeg = dateTime3;
              evidence1.Past = evidence1.Current;
            }
            else
            {
              Evidence evidence2 = evidence1;
              nullable = counter.SetDate;
              DateTime dateTime3;
              if (!nullable.HasValue)
              {
                nullable = Options.Period.PeriodName;
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
            evidence1.Counter = counter;
            Evidence evidence3 = evidence1;
            DateTime dateTime4;
            if (Convert.ToInt32(KvrplHelper.BaseValue(31, this.client.Company)) != 1)
            {
              dateTime4 = DateTime.Now;
            }
            else
            {
              nullable = Options.Period.PeriodName;
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

    private void dgvEvidence_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (((DataGridView) sender).DataSource == null)
        return;
      DataGridViewRow row = ((DataGridView) sender).Rows[e.RowIndex];
      if (((Evidence) row.DataBoundItem).Period != null && ((Evidence) row.DataBoundItem).Period.PeriodId == this.monthClosed.PeriodId + 1)
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

    private void dgvRent_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    private void SaveEvidence()
    {
      this.session.Clear();
      if (this.dgvEvidence.Rows.Count > 0)
      {
        foreach (DataGridViewRow row in (IEnumerable) this.dgvEvidence.Rows)
        {
          this.session = Domain.CurrentSession;
          this.dgvEvidence.CurrentCell = row.Cells[0];
          Evidence dataBoundItem = (Evidence) row.DataBoundItem;
          if (dataBoundItem.IsEdit)
          {
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
                  }
                  catch (Exception ex)
                  {
                    int num = (int) MessageBox.Show("Показания введены некорректно", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                  }
                  if (dataBoundItem.DEnd < dataBoundItem.DBeg)
                  {
                    int num = (int) MessageBox.Show("Дата настоящего меньше даты предыдущего", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                  }
                  Counter counter = this.session.Get<Counter>((object) dataBoundItem.Counter.CounterId);
                  if (counter.TypeCounter != null)
                  {
                    double num1 = Math.Pow(10.0, (double) counter.TypeCounter.CDigit);
                    if (dataBoundItem.Current >= num1 || dataBoundItem.Past >= num1)
                    {
                      int num2 = (int) MessageBox.Show("Показания не соответствуют разрядности счетчика", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                      return;
                    }
                  }
                  dataBoundItem.UName = Options.Login;
                  dataBoundItem.DEdit = DateTime.Now.Date;
                  dataBoundItem.IsEdit = false;
                  try
                  {
                    if (Convert.ToDecimal(dataBoundItem.Current) - Convert.ToDecimal(dataBoundItem.Past) != Decimal.Zero || Convert.ToDecimal(dataBoundItem.Current) - Convert.ToDecimal(dataBoundItem.Past) == Decimal.Zero)
                    {
                      if (dataBoundItem.Period == null)
                      {
                        dataBoundItem.Period = Options.Period;
                        this.session.Save((object) dataBoundItem);
                      }
                      else
                      {
                        dataBoundItem.Period = Options.Period;
                        this.session.Update((object) dataBoundItem);
                      }
                    }
                    this.session.Flush();
                  }
                  catch (Exception ex)
                  {
                  }
                  this.session.Clear();
                }
                else
                {
                  int num = (int) MessageBox.Show("Показания не введены", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  return;
                }
              }
              else
              {
                int num = (int) MessageBox.Show("Дата настоящих показаний не введена", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
              }
            }
            else
            {
              int num = (int) MessageBox.Show("Дата предыдущих показаний не введена", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
          }
        }
      }
      this.session.Clear();
    }

    private void dgvEvidence_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      ((Evidence) this.dgvEvidence.CurrentRow.DataBoundItem).IsEdit = true;
    }

    private void cmbFlat_SelectedValueChanged(object sender, EventArgs e)
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
      DataGridViewCellStyle gridViewCellStyle = new DataGridViewCellStyle();
      this.pnBtn = new Panel();
      this.btnCommission = new Button();
      this.btnCalc = new Button();
      this.btnSave = new Button();
      this.btnExit = new Button();
      this.cmbFlat = new ComboBox();
      this.cmbStreet = new ComboBox();
      this.lblStreet = new Label();
      this.lblClient = new Label();
      this.gbAdr = new GroupBox();
      this.lblFlat = new Label();
      this.lblHome = new Label();
      this.cmbHome = new ComboBox();
      this.txtClient = new TextBox();
      this.mpCurrentPeriod = new MonthPicker();
      this.lblSumma = new Label();
      this.txbSumma = new TextBox();
      this.dgvRent = new DataGridView();
      this.txbPacket = new MaskedTextBox();
      this.lblPacket = new Label();
      this.cmbSource = new ComboBox();
      this.lblSource = new Label();
      this.lblFIO = new Label();
      this.dgvEvidence = new DataGridView();
      this.pnBtn.SuspendLayout();
      this.gbAdr.SuspendLayout();
      ((ISupportInitialize) this.dgvRent).BeginInit();
      ((ISupportInitialize) this.dgvEvidence).BeginInit();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnCommission);
      this.pnBtn.Controls.Add((Control) this.btnCalc);
      this.pnBtn.Controls.Add((Control) this.btnSave);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 579);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(1123, 40);
      this.pnBtn.TabIndex = 4;
      this.btnCommission.Image = (Image) Resources.notepad_32;
      this.btnCommission.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCommission.Location = new Point(254, 5);
      this.btnCommission.Name = "btnCommission";
      this.btnCommission.Size = new Size(122, 30);
      this.btnCommission.TabIndex = 3;
      this.btnCommission.Text = "Поручение";
      this.btnCommission.TextAlign = ContentAlignment.MiddleRight;
      this.btnCommission.UseVisualStyleBackColor = true;
      this.btnCommission.Click += new EventHandler(this.btnCommission_Click);
      this.btnCalc.Image = (Image) Resources.calc_32;
      this.btnCalc.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCalc.Location = new Point(137, 5);
      this.btnCalc.Name = "btnCalc";
      this.btnCalc.Size = new Size(101, 30);
      this.btnCalc.TabIndex = 2;
      this.btnCalc.Text = "Расчет";
      this.btnCalc.TextAlign = ContentAlignment.MiddleRight;
      this.btnCalc.UseVisualStyleBackColor = true;
      this.btnCalc.Click += new EventHandler(this.btnCalc_Click);
      this.btnSave.Image = (Image) Resources.Tick;
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.Location = new Point(15, 5);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(107, 30);
      this.btnSave.TabIndex = 1;
      this.btnSave.Text = "Сохранить";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(1030, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(81, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.button1_Click);
      this.cmbFlat.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
      this.cmbFlat.AutoCompleteSource = AutoCompleteSource.ListItems;
      this.cmbFlat.FormattingEnabled = true;
      this.cmbFlat.Location = new Point(122, 83);
      this.cmbFlat.Name = "cmbFlat";
      this.cmbFlat.Size = new Size(100, 24);
      this.cmbFlat.TabIndex = 3;
      this.cmbFlat.SelectionChangeCommitted += new EventHandler(this.cmbFlat_SelectionChangeCommitted);
      this.cmbFlat.KeyDown += new KeyEventHandler(this.cmbFlat_KeyDown);
      this.cmbStreet.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
      this.cmbStreet.AutoCompleteSource = AutoCompleteSource.ListItems;
      this.cmbStreet.Location = new Point(122, 15);
      this.cmbStreet.Name = "cmbStreet";
      this.cmbStreet.Size = new Size(249, 24);
      this.cmbStreet.TabIndex = 1;
      this.cmbStreet.SelectionChangeCommitted += new EventHandler(this.cmbStreet_SelectionChangeCommitted);
      this.cmbStreet.SelectedValueChanged += new EventHandler(this.cmbStreet_SelectedValueChanged);
      this.cmbStreet.KeyDown += new KeyEventHandler(this.cmbStreet_KeyDown);
      this.lblStreet.AutoSize = true;
      this.lblStreet.Location = new Point(6, 18);
      this.lblStreet.Name = "lblStreet";
      this.lblStreet.Size = new Size(49, 16);
      this.lblStreet.TabIndex = 7;
      this.lblStreet.Text = "Улица";
      this.lblClient.AutoSize = true;
      this.lblClient.Location = new Point(12, 9);
      this.lblClient.Name = "lblClient";
      this.lblClient.Size = new Size(98, 16);
      this.lblClient.TabIndex = 20;
      this.lblClient.Text = "Лицевой счет";
      this.gbAdr.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gbAdr.Controls.Add((Control) this.lblStreet);
      this.gbAdr.Controls.Add((Control) this.cmbFlat);
      this.gbAdr.Controls.Add((Control) this.cmbStreet);
      this.gbAdr.Controls.Add((Control) this.lblFlat);
      this.gbAdr.Controls.Add((Control) this.lblHome);
      this.gbAdr.Controls.Add((Control) this.cmbHome);
      this.gbAdr.Location = new Point(15, 31);
      this.gbAdr.Name = "gbAdr";
      this.gbAdr.Size = new Size(1096, 120);
      this.gbAdr.TabIndex = 19;
      this.gbAdr.TabStop = false;
      this.gbAdr.Text = "Адрес";
      this.lblFlat.AutoSize = true;
      this.lblFlat.Location = new Point(6, 86);
      this.lblFlat.Name = "lblFlat";
      this.lblFlat.Size = new Size(71, 16);
      this.lblFlat.TabIndex = 10;
      this.lblFlat.Text = "Квартира";
      this.lblHome.AutoSize = true;
      this.lblHome.Location = new Point(6, 51);
      this.lblHome.Name = "lblHome";
      this.lblHome.Size = new Size(34, 16);
      this.lblHome.TabIndex = 8;
      this.lblHome.Text = "Дом";
      this.cmbHome.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
      this.cmbHome.AutoCompleteSource = AutoCompleteSource.ListItems;
      this.cmbHome.FormattingEnabled = true;
      this.cmbHome.Location = new Point(122, 48);
      this.cmbHome.Name = "cmbHome";
      this.cmbHome.Size = new Size(100, 24);
      this.cmbHome.TabIndex = 2;
      this.cmbHome.SelectionChangeCommitted += new EventHandler(this.cmbHome_SelectionChangeCommitted);
      this.cmbHome.SelectedValueChanged += new EventHandler(this.cmbHome_SelectedValueChanged);
      this.txtClient.Location = new Point(137, 6);
      this.txtClient.Name = "txtClient";
      this.txtClient.Size = new Size(100, 22);
      this.txtClient.TabIndex = 0;
      this.txtClient.KeyDown += new KeyEventHandler(this.txtClient_KeyDown);
      this.txtClient.KeyPress += new KeyPressEventHandler(this.txtClient_KeyPress);
      this.mpCurrentPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.mpCurrentPeriod.CustomFormat = "MMMM yyyy";
      this.mpCurrentPeriod.Format = DateTimePickerFormat.Custom;
      this.mpCurrentPeriod.Location = new Point(971, 6);
      this.mpCurrentPeriod.Name = "mpCurrentPeriod";
      this.mpCurrentPeriod.OldMonth = 0;
      this.mpCurrentPeriod.ShowUpDown = true;
      this.mpCurrentPeriod.Size = new Size(140, 22);
      this.mpCurrentPeriod.TabIndex = 21;
      this.mpCurrentPeriod.ValueChanged += new EventHandler(this.mpCurrentPeriod_ValueChanged);
      this.lblSumma.AutoSize = true;
      this.lblSumma.Location = new Point(12, 189);
      this.lblSumma.Name = "lblSumma";
      this.lblSumma.Size = new Size(102, 16);
      this.lblSumma.TabIndex = 22;
      this.lblSumma.Text = "Сумма оплаты";
      this.txbSumma.Location = new Point(137, 186);
      this.txbSumma.Name = "txbSumma";
      this.txbSumma.Size = new Size(100, 22);
      this.txbSumma.TabIndex = 23;
      this.txbSumma.KeyDown += new KeyEventHandler(this.txbSumma_KeyDown);
      this.txbSumma.KeyPress += new KeyPressEventHandler(this.txbSumma_KeyPress);
      this.dgvRent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dgvRent.BackgroundColor = Color.AliceBlue;
      this.dgvRent.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvRent.Location = new Point(15, 214);
      this.dgvRent.Name = "dgvRent";
      gridViewCellStyle.NullValue = (object) null;
      this.dgvRent.RowsDefaultCellStyle = gridViewCellStyle;
      this.dgvRent.Size = new Size(1096, 175);
      this.dgvRent.TabIndex = 24;
      this.dgvRent.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvRent_CellFormatting);
      this.dgvRent.CellEndEdit += new DataGridViewCellEventHandler(this.dgvRent_CellEndEdit);
      this.dgvRent.DataError += new DataGridViewDataErrorEventHandler(this.dgvRent_DataError);
      this.dgvRent.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvRent_ColumnWidthChanged);
      this.txbPacket.Location = new Point(732, 186);
      this.txbPacket.Mask = "000000";
      this.txbPacket.Name = "txbPacket";
      this.txbPacket.PromptChar = ' ';
      this.txbPacket.Size = new Size(100, 22);
      this.txbPacket.TabIndex = 26;
      this.lblPacket.AutoSize = true;
      this.lblPacket.Location = new Point(677, 189);
      this.lblPacket.Name = "lblPacket";
      this.lblPacket.Size = new Size(49, 16);
      this.lblPacket.TabIndex = 28;
      this.lblPacket.Text = "Пачка";
      this.cmbSource.AutoCompleteMode = AutoCompleteMode.Append;
      this.cmbSource.AutoCompleteSource = AutoCompleteSource.CustomSource;
      this.cmbSource.FormattingEnabled = true;
      this.cmbSource.Location = new Point(362, 186);
      this.cmbSource.Name = "cmbSource";
      this.cmbSource.Size = new Size(265, 24);
      this.cmbSource.TabIndex = 25;
      this.cmbSource.SelectionChangeCommitted += new EventHandler(this.cmbSource_SelectionChangeCommitted);
      this.lblSource.AutoSize = true;
      this.lblSource.Location = new Point(285, 189);
      this.lblSource.Name = "lblSource";
      this.lblSource.Size = new Size(71, 16);
      this.lblSource.TabIndex = 27;
      this.lblSource.Text = "Источник";
      this.lblFIO.AutoSize = true;
      this.lblFIO.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblFIO.Location = new Point(12, 163);
      this.lblFIO.Name = "lblFIO";
      this.lblFIO.Size = new Size(0, 16);
      this.lblFIO.TabIndex = 29;
      this.dgvEvidence.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dgvEvidence.BackgroundColor = Color.AliceBlue;
      this.dgvEvidence.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvEvidence.Location = new Point(15, 393);
      this.dgvEvidence.Name = "dgvEvidence";
      this.dgvEvidence.Size = new Size(1096, 180);
      this.dgvEvidence.TabIndex = 30;
      this.dgvEvidence.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvEvidence_CellBeginEdit);
      this.dgvEvidence.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvEvidence_CellFormatting);
      this.dgvEvidence.DataError += new DataGridViewDataErrorEventHandler(this.dgvRent_DataError);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(1123, 619);
      this.Controls.Add((Control) this.dgvEvidence);
      this.Controls.Add((Control) this.lblFIO);
      this.Controls.Add((Control) this.txbPacket);
      this.Controls.Add((Control) this.lblPacket);
      this.Controls.Add((Control) this.cmbSource);
      this.Controls.Add((Control) this.lblSource);
      this.Controls.Add((Control) this.dgvRent);
      this.Controls.Add((Control) this.txbSumma);
      this.Controls.Add((Control) this.lblSumma);
      this.Controls.Add((Control) this.mpCurrentPeriod);
      this.Controls.Add((Control) this.lblClient);
      this.Controls.Add((Control) this.gbAdr);
      this.Controls.Add((Control) this.txtClient);
      this.Controls.Add((Control) this.pnBtn);
      this.Name = "FrmQuickPay";
      this.Text = "Поручение";
      this.Load += new EventHandler(this.FrmQuickPay_Load);
      this.pnBtn.ResumeLayout(false);
      this.gbAdr.ResumeLayout(false);
      this.gbAdr.PerformLayout();
      ((ISupportInitialize) this.dgvRent).EndInit();
      ((ISupportInitialize) this.dgvEvidence).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
