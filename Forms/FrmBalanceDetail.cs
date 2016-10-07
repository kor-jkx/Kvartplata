// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmBalanceDetail
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using NHibernate;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmBalanceDetail : FrmBaseForm1
  {
    private static IContainer ic = (IContainer) null;
    private FormStateSaver fss = new FormStateSaver(FrmBalanceDetail.ic);
    private IContainer components = (IContainer) null;
    private Service service;
    private LsClient client;
    private ISession session;
    private bool isBalance;
    private int setupPeriod;
    private int setupValue;
    private DataGridView dgvBalanceDetail;

    public FrmBalanceDetail()
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
    }

    public FrmBalanceDetail(Service service, LsClient client, bool isBalance)
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
      this.service = service;
      this.client = client;
      this.isBalance = isBalance;
    }

    private void FrmBalanceDetail_Shown(object sender, EventArgs e)
    {
      this.dgvBalanceDetail.Columns.Clear();
      this.dgvBalanceDetail.DataSource = (object) null;
      IList list1 = (IList) new ArrayList();
      this.session = Domain.CurrentSession;
      DataTable dataTable = new DataTable("Balance");
      try
      {
        this.setupPeriod = Convert.ToInt32(this.session.CreateSQLQuery(string.Format("select setup_value from fksetup where setup_id=3 and manager_id=(select manager_id from dcCompany where company_id={0})", (object) this.client.Company.CompanyId)).List()[0]);
      }
      catch
      {
        this.setupPeriod = 1201;
      }
      try
      {
        this.setupValue = Convert.ToInt32(this.session.CreateSQLQuery(string.Format("select setup_value from fksetup where setup_id=4 and manager_id=(select manager_id from dcCompany where company_id={0})", (object) this.client.Company.CompanyId)).List()[0]);
      }
      catch
      {
        this.setupValue = 0;
      }
      string str = "(if (" + (object) this.setupPeriod + " <= {0} and " + (object) this.setupValue + "<>2) then if " + (object) this.setupValue + " =1 then isnull(rent,0)+isnull(rent_past,0)+isnull(rent_comp,0) else isnull(rent,0)+isnull(rent_past,0)-isnull(msppast,0)+isnull(msppastpay,0)+isnull(rent_comp,0) endif else  isnull(rent,0)+isnull(rent_past,0)-isnull(msp,0)+isnull(msppay,0)-isnull(msppast,0)+isnull(msppastpay,0)+isnull(rent_comp,0) endif) ";
      IList list2;
      if (this.isBalance)
      {
        list2 = this.session.CreateSQLQuery(string.Format("select s.service_id,s.service_name,(select nameorg_min from base_org where idbaseorg=bo.recipient_id) as namerec,(select nameorg_min from base_org where idbaseorg=bo.perfomer_id) as nameper,balance_in,rent-rent_past as rent,rent_past,(select sum(rent_vat) from DBA.lsRent rm where rm.period_id={0} and client_id={1} and rm.service_id=s.service_id) as nds,(select sum(rent) from DBA.lsRentMSP rm,DBA.dcMSP m where rm.msp_id=m.msp_id and rm.period_id={0} and client_id={1} and code=0 and rm.service_id=s.service_id) as msp,(select sum(rent) from lsRentMSP rm,dcMSP m where rm.msp_id=m.msp_id and rm.period_id={0} and client_id={1} and code=0 and m.mspperiod_id<={0} and rm.service_id=s.service_id) as msppay,(select sum(rent) from DBA.lsRentMSP rm,DBA.dcMSP m where rm.msp_id=m.msp_id and rm.period_id={0} and client_id={1} and code<>0 and rm.service_id=s.service_id) as msppast,(select sum(rent) from lsRentMSP rm,dcMSP m where rm.msp_id=m.msp_id and rm.period_id={0} and client_id={1} and code<>0 and m.mspperiod_id<={0} and rm.service_id=s.service_id) as msppastpay," + str + " as preditog,(if preditog<>0 then preditog else null endif) as itog,payment,balance_out from dcService s left outer join lsBalance b on b.service_id=s.service_id and b.period_id={0} and b.client_id={1} left outer join dcSupplier bo on b.supplier_id=bo.supplier_id where s.root={2}", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) this.service.ServiceId)).List();
        this.session.Clear();
        dataTable.Columns.Add("№", System.Type.GetType("System.Int32"));
        dataTable.Columns.Add("Услуга", System.Type.GetType("System.String"));
        dataTable.Columns.Add("Получатель", System.Type.GetType("System.String"));
        dataTable.Columns.Add("Исполнитель", System.Type.GetType("System.String"));
        dataTable.Columns.Add("Входящее", System.Type.GetType("System.Double"));
        dataTable.Columns.Add("Начислено", System.Type.GetType("System.Double"));
        dataTable.Columns.Add("Перерасчет", System.Type.GetType("System.Double"));
        dataTable.Columns.Add("НДС", System.Type.GetType("System.Double"));
        dataTable.Columns.Add("МСП", System.Type.GetType("System.Double"));
        dataTable.Columns.Add("Выплаченная льгота", System.Type.GetType("System.Double"));
        dataTable.Columns.Add("Перерасчет по МСП", System.Type.GetType("System.Double"));
        dataTable.Columns.Add("Перерасчет по выплаченной льготе", System.Type.GetType("System.Double"));
        dataTable.Columns.Add("Предварительный итог", System.Type.GetType("System.Double"));
        dataTable.Columns.Add("Итого начисл.", System.Type.GetType("System.Double"));
        dataTable.Columns.Add("Оплачено", System.Type.GetType("System.Double"));
        dataTable.Columns.Add("Исходящее", System.Type.GetType("System.Double"));
      }
      else
      {
        this.session = Domain.CurrentSession;
        list2 = this.session.CreateSQLQuery(string.Format("select s.service_id,s.service_name,(select nameorg_min from base_org where idbaseorg=bo.recipient_id) as namerec,(select nameorg_min from base_org where idbaseorg=bo.perfomer_id) as nameper,balance_in,rent,correct,payment,balance_out from dcService s left outer join lsBalancePeni b on b.service_id=s.service_id and b.period_id={0} and b.client_id={1} left outer join dcSupplier bo on b.supplier_id=bo.supplier_id where s.root={2}", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) this.service.ServiceId)).List();
        this.session.Clear();
        dataTable.Columns.Add("№", System.Type.GetType("System.Int32"));
        dataTable.Columns.Add("Услуга", System.Type.GetType("System.String"));
        dataTable.Columns.Add("Получатель", System.Type.GetType("System.String"));
        dataTable.Columns.Add("Исполнитель", System.Type.GetType("System.String"));
        dataTable.Columns.Add("Входящее", System.Type.GetType("System.Double"));
        dataTable.Columns.Add("Начислено", System.Type.GetType("System.Double"));
        dataTable.Columns.Add("Коррект-ки", System.Type.GetType("System.Double"));
        dataTable.Columns.Add("Оплачено", System.Type.GetType("System.Double"));
        dataTable.Columns.Add("Исходящее", System.Type.GetType("System.Double"));
      }
      foreach (object[] objArray in (IEnumerable) list2)
        dataTable.Rows.Add(objArray);
      this.dgvBalanceDetail.DataSource = (object) dataTable;
      DataGridViewCellStyle gridViewCellStyle = new DataGridViewCellStyle();
      gridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvBalanceDetail.Columns)
      {
        if (column.Index != 0 && column.Index != 1 && column.Index != 2 && column.Index != 3)
        {
          column.Width = 85;
          column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight;
          column.HeaderCell.Style = gridViewCellStyle;
          column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        else if (column.Index == 0)
        {
          this.dgvBalanceDetail.Columns[0].Width = 50;
          this.dgvBalanceDetail.Columns[0].DefaultCellStyle.BackColor = Color.PapayaWhip;
        }
        else
        {
          this.dgvBalanceDetail.Columns[1].Width = 140;
          this.dgvBalanceDetail.Columns[1].DefaultCellStyle.BackColor = Color.PapayaWhip;
          this.dgvBalanceDetail.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft;
        }
      }
      this.dgvBalanceDetail.Columns[2].Width = 240;
      this.dgvBalanceDetail.Columns[3].Width = 240;
      if (this.isBalance)
      {
        this.dgvBalanceDetail.Columns[7].Visible = false;
        this.dgvBalanceDetail.Columns[9].Visible = false;
        this.dgvBalanceDetail.Columns[11].Visible = false;
        this.dgvBalanceDetail.Columns[12].Visible = false;
      }
      this.dgvBalanceDetail.ReadOnly = true;
      this.dgvBalanceDetail.Focus();
    }

    private void dgvBalanceDetail_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmBalanceDetail));
      this.dgvBalanceDetail = new DataGridView();
      ((ISupportInitialize) this.dgvBalanceDetail).BeginInit();
      this.SuspendLayout();
      this.dgvBalanceDetail.BackgroundColor = Color.AliceBlue;
      this.dgvBalanceDetail.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvBalanceDetail.Dock = DockStyle.Fill;
      this.dgvBalanceDetail.Location = new Point(0, 0);
      this.dgvBalanceDetail.Name = "dgvBalanceDetail";
      this.dgvBalanceDetail.Size = new Size(565, 282);
      this.dgvBalanceDetail.TabIndex = 1;
      this.dgvBalanceDetail.DataError += new DataGridViewDataErrorEventHandler(this.dgvBalanceDetail_DataError);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(565, 322);
      this.Controls.Add((Control) this.dgvBalanceDetail);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = "FrmBalanceDetail";
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Начисления по составляющим";
      this.Shown += new EventHandler(this.FrmBalanceDetail_Shown);
      this.Controls.SetChildIndex((Control) this.dgvBalanceDetail, 0);
      ((ISupportInitialize) this.dgvBalanceDetail).EndInit();
      this.ResumeLayout(false);
    }
  }
}
