// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.Controls.UCLsSupplier
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
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms.Controls
{
  public class UCLsSupplier : UserControl
  {
    private bool pastTime = false;
    protected GridSettings MySettingsLsSupplier = new GridSettings();
    private IContainer components = (IContainer) null;
    private LsService lsService;
    private LsClient lsClient;
    private bool InsertRecord;
    private Period currentPeriod;
    private ISession session;
    private Period MonthClosed;
    private LsSupplier OldLsSupplier;
    private DateTime LastDayMonthClosed;
    private Period NextMonthClosed;
    private int InsertServiceCount;
    private IList<LsSupplier> OldListLsSupplier;
    private bool InitSupplier;
    private Panel panel1;
    private Label label1;
    private Button btnSave;
    private Button btnDelete;
    private Button btnAdd;
    private Panel pnUp;
    private DataGridView dgvLsSupplier;
    private Label lblPastTime;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem;
    private ToolStripMenuItem удалитьЗаписьИзВыбранныхОбъектовToolStripMenuItem;
    private ToolStripMenuItem обновитьЗаписьВВыбранныхОбъектахToolStripMenuItem;
    private CheckBox chbArhiv;
    private Button btnPastTime;
    private Timer tmr1;
    private ToolStripMenuItem miInPatTime;

    public LsService LsService
    {
      get
      {
        return this.lsService;
      }
      set
      {
        this.lsService = value;
      }
    }

    public Period CurrentPeriod
    {
      get
      {
        return this.currentPeriod;
      }
      set
      {
        this.currentPeriod = value;
      }
    }

    public LsClient LsClient
    {
      get
      {
        return this.lsClient;
      }
      set
      {
        this.lsClient = value;
      }
    }

    public bool PastTime
    {
      get
      {
        return this.pastTime;
      }
      set
      {
        this.lblPastTime.Visible = value;
        this.pastTime = value;
      }
    }

    public UCLsSupplier()
    {
      this.InitializeComponent();
      ToolTip toolTip = new ToolTip();
      toolTip.SetToolTip((Control) this.btnAdd, "Добавить нового поставщика");
      toolTip.SetToolTip((Control) this.btnDelete, "Удалить текущего поставщика");
      toolTip.SetToolTip((Control) this.btnSave, "Сохранить изменения");
    }

    public void RefreshGrid()
    {
      this.dgvLsSupplier.Refresh();
    }

    public void Clear()
    {
      this.dgvLsSupplier.DataSource = (object) null;
      this.dgvLsSupplier.Columns.Clear();
    }

    public void LoadLsSupplier()
    {
      if (this.lsService == null || this.lsService.Service == null)
        return;
      this.btnAdd.Enabled = true;
      this.btnDelete.Enabled = true;
      this.btnSave.Enabled = false;
      this.session = Domain.CurrentSession;
      this.MonthClosed = KvrplHelper.GetKvrClose(this.lsClient.ClientId, Options.ComplexPasp, Options.ComplexPrior);
      this.NextMonthClosed = KvrplHelper.GetNextPeriod(this.MonthClosed);
      this.LastDayMonthClosed = KvrplHelper.GetLastDayPeriod(this.MonthClosed.PeriodName.Value);
      this.InsertServiceCount = 0;
      this.InsertRecord = false;
      this.InitSupplier = true;
      this.session.CreateQuery("from Service").List();
      IList<LsSupplier> lsSupplierList1 = (IList<LsSupplier>) new List<LsSupplier>();
      IList<LsSupplier> lsSupplierList2;
      if (!this.PastTime)
      {
        if (this.chbArhiv.Checked)
          lsSupplierList2 = this.session.CreateQuery(string.Format("select l from LsSupplier l, Service s where l.Service.ServiceId = s.ServiceId and  l.Period.PeriodId={0} and s.Root = {1} and l.LsClient.ClientId={2} ", (object) 0, (object) this.lsService.Service.ServiceId, (object) this.lsClient.ClientId)).List<LsSupplier>();
        else
          lsSupplierList2 = this.session.CreateQuery(string.Format("select l from LsSupplier l, Service s where l.Service.ServiceId = s.ServiceId and  l.Period.PeriodId={0} and s.Root = {1} and l.LsClient.ClientId={2} and  l.DEnd >= '{3}' ", (object) 0, (object) this.lsService.Service.ServiceId, (object) this.lsClient.ClientId, (object) KvrplHelper.DateToBaseFormat(this.NextMonthClosed.PeriodName.Value))).List<LsSupplier>();
      }
      else
        lsSupplierList2 = !this.chbArhiv.Checked ? this.session.CreateQuery(string.Format("select l from LsSupplier l, Service s where l.Service.ServiceId = s.ServiceId and  l.Period.PeriodId={0} and s.Root = {1} and l.LsClient.ClientId={2}", (object) this.CurrentPeriod.PeriodId, (object) this.lsService.Service.ServiceId, (object) this.lsClient.ClientId)).List<LsSupplier>() : this.session.CreateQuery(string.Format("select l from LsSupplier l, Service s where l.Service.ServiceId = s.ServiceId and  l.Period.PeriodId!={0} and s.Root = {1} and l.LsClient.ClientId={2}", (object) 0, (object) this.lsService.Service.ServiceId, (object) this.lsClient.ClientId)).List<LsSupplier>();
      this.dgvLsSupplier.Columns.Clear();
      this.dgvLsSupplier.DataSource = (object) null;
      this.dgvLsSupplier.DataSource = (object) lsSupplierList2;
      this.OldListLsSupplier = (IList<LsSupplier>) new List<LsSupplier>();
      foreach (LsSupplier lsSupplier in (List<LsSupplier>) this.dgvLsSupplier.DataSource)
      {
        lsSupplier.IsEdit = false;
        lsSupplier.OldHashCode = lsSupplier.GetHashCode();
        lsSupplier.IsInsert = false;
      }
      this.OldListLsSupplier = (IList<LsSupplier>) (this.dgvLsSupplier.DataSource as List<LsSupplier>);
      this.dgvLsSupplierComboBoxField();
      this.MySettingsLsSupplier.GridName = "LsSupplier";
      this.LoadSettings();
      this.InitSupplier = false;
    }

    private void dgvLsSupplierComboBoxField()
    {
      KvrplHelper.AddMaskDateColumn(this.dgvLsSupplier, 0, "Дата начала", "MDBeg");
      KvrplHelper.AddMaskDateColumn(this.dgvLsSupplier, 1, "Дата окончания", "MDEnd");
      IList<Service> serviceList = this.session.CreateCriteria(typeof (Service)).Add((ICriterion) Restrictions.Not((ICriterion) Restrictions.Eq("ServiceId", (object) this.lsService.Service.ServiceId))).Add((ICriterion) Restrictions.Eq("Root", (object) this.lsService.Service.ServiceId)).AddOrder(Order.Asc("ServiceId")).List<Service>();
      DataGridViewComboBoxCell viewComboBoxCell = new DataGridViewComboBoxCell();
      viewComboBoxCell.DisplayStyleForCurrentCellOnly = true;
      viewComboBoxCell.ValueMember = "ServiceId";
      viewComboBoxCell.DisplayMember = "ServiceName";
      viewComboBoxCell.DataSource = (object) serviceList;
      DataGridViewColumn dataGridViewColumn = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn.CellTemplate = (DataGridViewCell) viewComboBoxCell;
      dataGridViewColumn.HeaderText = "Составляющая услуги";
      dataGridViewColumn.Name = "SName";
      this.dgvLsSupplier.Columns.Insert(1, dataGridViewColumn);
      IList<BaseOrg> baseOrgList1 = this.session.CreateQuery(string.Format("select distinct d.Recipient from CmpSupplier s, Supplier d where s.SupplierOrg.SupplierId = d.SupplierId and s.Service.ServiceId={0} and d.Recipient.BaseOrgId<>0  and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={1} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{3}' and Param.ParamId=211) order by d.Recipient.NameOrgMin", (object) this.lsService.Service.ServiceId, (object) this.lsClient.Company.CompanyId, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(this.NextMonthClosed.PeriodName.Value)), (object) KvrplHelper.DateToBaseFormat(this.NextMonthClosed.PeriodName.Value))).List<BaseOrg>();
      baseOrgList1.Insert(0, new BaseOrg(0, ""));
      KvrplHelper.AddComboBoxColumn(this.dgvLsSupplier, 2, (IList) baseOrgList1, "BaseOrgId", "NameOrgMin", "Получатель", "Recipient", 7, 100);
      KvrplHelper.AddComboBoxColumn(this.dgvLsSupplier, 3, (IList) null, (string) null, (string) null, "Исполнитель", "Perfomer", 160, 120);
      KvrplHelper.ViewEdit(this.dgvLsSupplier);
      foreach (DataGridViewRow row in (IEnumerable) this.dgvLsSupplier.Rows)
      {
        DataGridViewCell cell1 = row.Cells["MDBeg"];
        DateTime dateTime = ((LsSupplier) row.DataBoundItem).DBeg;
        string shortDateString1 = dateTime.ToShortDateString();
        cell1.Value = (object) shortDateString1;
        DataGridViewCell cell2 = row.Cells["MDEnd"];
        dateTime = ((LsSupplier) row.DataBoundItem).DEnd;
        string shortDateString2 = dateTime.ToShortDateString();
        cell2.Value = (object) shortDateString2;
        if (((LsSupplier) row.DataBoundItem).Service != null)
          row.Cells["SName"].Value = (object) ((LsSupplier) row.DataBoundItem).Service.ServiceId;
        DateTime? periodName;
        if (((LsSupplier) row.DataBoundItem).Supplier != null)
        {
          row.Cells["Recipient"].Value = (object) ((LsSupplier) row.DataBoundItem).Supplier.Recipient.BaseOrgId;
          ISession session1 = this.session;
          string format1 = "select distinct d.Perfomer from CmpSupplier s, Supplier d where s.SupplierOrg.SupplierId = d.SupplierId and s.Service.ServiceId={0} and d.Recipient.BaseOrgId={4} and d.Perfomer.BaseOrgId<>0  and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={1} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{3}' and Param.ParamId=211)";
          object[] objArray1 = new object[5];
          objArray1[0] = (object) this.lsService.Service.ServiceId;
          objArray1[1] = (object) this.lsClient.Company.CompanyId;
          int index1 = 2;
          periodName = this.NextMonthClosed.PeriodName;
          string baseFormat1 = KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(periodName.Value));
          objArray1[index1] = (object) baseFormat1;
          int index2 = 3;
          periodName = this.NextMonthClosed.PeriodName;
          string baseFormat2 = KvrplHelper.DateToBaseFormat(periodName.Value);
          objArray1[index2] = (object) baseFormat2;
          int index3 = 4;
          // ISSUE: variable of a boxed type
          int baseOrgId1 = ((LsSupplier) row.DataBoundItem).Supplier.Recipient.BaseOrgId;
          objArray1[index3] = (object) baseOrgId1;
          string queryString1 = string.Format(format1, objArray1);
          IList<BaseOrg> baseOrgList2 = session1.CreateQuery(queryString1).List<BaseOrg>();
          if (((LsSupplier) row.DataBoundItem).Supplier.Recipient.BaseOrgId == 0)
          {
            baseOrgList2.Insert(0, new BaseOrg(0, ""));
          }
          else
          {
            ISession session2 = this.session;
            string format2 = "select distinct d.Perfomer from CmpSupplier s, Supplier d where s.SupplierOrg.SupplierId = d.SupplierId and s.Service.ServiceId={0} and d.Recipient.BaseOrgId={4} and d.Perfomer.BaseOrgId=0  and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={1} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{3}' and Param.ParamId=211)";
            object[] objArray2 = new object[5];
            objArray2[0] = (object) this.lsService.Service.ServiceId;
            objArray2[1] = (object) this.lsClient.Company.CompanyId;
            int index4 = 2;
            periodName = this.NextMonthClosed.PeriodName;
            string baseFormat3 = KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(periodName.Value));
            objArray2[index4] = (object) baseFormat3;
            int index5 = 3;
            periodName = this.NextMonthClosed.PeriodName;
            string baseFormat4 = KvrplHelper.DateToBaseFormat(periodName.Value);
            objArray2[index5] = (object) baseFormat4;
            int index6 = 4;
            // ISSUE: variable of a boxed type
            int baseOrgId2 = ((LsSupplier) row.DataBoundItem).Supplier.Recipient.BaseOrgId;
            objArray2[index6] = (object) baseOrgId2;
            string queryString2 = string.Format(format2, objArray2);
            IList<BaseOrg> baseOrgList3 = session2.CreateQuery(queryString2).List<BaseOrg>();
            if (baseOrgList3.Count > 0)
              baseOrgList2.Insert(0, baseOrgList3[0]);
          }
          row.Cells["Perfomer"] = (DataGridViewCell) new DataGridViewComboBoxCell()
          {
            DisplayStyleForCurrentCellOnly = true,
            ValueMember = "BaseOrgId",
            DisplayMember = "NameOrgMinDop",
            DataSource = (object) baseOrgList2
          };
          row.Cells["Perfomer"].Value = (object) ((LsSupplier) row.DataBoundItem).Supplier.Perfomer.BaseOrgId;
        }
        else
        {
          ISession session = this.session;
          string format = "select distinct d.Perfomer from CmpSupplier s, Supplier d where s.SupplierOrg.SupplierId = d.SupplierId and s.Service.ServiceId={0} and d.Recipient.BaseOrgId=0 and d.Perfomer.BaseOrgId<>0  and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={1} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{3}' and Param.ParamId=211)";
          object[] objArray = new object[4]{ (object) this.lsService.Service.ServiceId, (object) this.lsClient.Company.CompanyId, null, null };
          int index1 = 2;
          periodName = this.NextMonthClosed.PeriodName;
          string baseFormat1 = KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(periodName.Value));
          objArray[index1] = (object) baseFormat1;
          int index2 = 3;
          periodName = this.NextMonthClosed.PeriodName;
          string baseFormat2 = KvrplHelper.DateToBaseFormat(periodName.Value);
          objArray[index2] = (object) baseFormat2;
          string queryString = string.Format(format, objArray);
          IList<BaseOrg> baseOrgList2 = session.CreateQuery(queryString).List<BaseOrg>();
          baseOrgList2.Insert(0, new BaseOrg(0, ""));
          row.Cells["Perfomer"] = (DataGridViewCell) new DataGridViewComboBoxCell()
          {
            DisplayStyleForCurrentCellOnly = true,
            ValueMember = "BaseOrgId",
            DisplayMember = "NameOrgMinDop",
            DataSource = (object) baseOrgList2
          };
        }
      }
    }

    private void InsertLsSupplier()
    {
      if (this.lsService == null)
      {
        int num = (int) MessageBox.Show("Не выбрана услуга!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        LsSupplier lsSupplier = new LsSupplier();
        lsSupplier.LsClient = this.lsClient;
        lsSupplier.IsInsert = true;
        lsSupplier.IsEdit = true;
        if (this.PastTime)
        {
          lsSupplier.Period = this.CurrentPeriod;
          lsSupplier.DBeg = this.MonthClosed.PeriodName.Value;
          lsSupplier.DEnd = KvrplHelper.GetLastDayPeriod(this.MonthClosed.PeriodName.Value);
        }
        else
        {
          lsSupplier.Period = this.session.Get<Period>((object) 0);
          lsSupplier.DBeg = !(this.CurrentPeriod.PeriodName.Value <= this.MonthClosed.PeriodName.Value) ? this.CurrentPeriod.PeriodName.Value : this.NextMonthClosed.PeriodName.Value.Date;
          lsSupplier.DEnd = Convert.ToDateTime("31.12.2999");
          if (this.lsClient.Complex.IdFk == Options.ComplexArenda.IdFk)
          {
            IList<LsArenda> lsArendaList = this.session.CreateCriteria(typeof (LsArenda)).Add((ICriterion) Restrictions.Eq("LsClient.ClientId", (object) this.lsClient.ClientId)).List<LsArenda>();
            if (lsArendaList.Count > 0)
            {
              if (lsArendaList[0].DBeg <= lsSupplier.DBeg && lsArendaList[0].DEnd >= lsSupplier.DBeg)
                lsSupplier.DEnd = lsArendaList[0].DEnd;
              else if (lsArendaList[0].DBeg > lsSupplier.DBeg)
              {
                lsSupplier.DEnd = lsArendaList[0].DEnd;
                lsSupplier.DBeg = lsArendaList[0].DBeg;
              }
            }
          }
        }
        if (this.dgvLsSupplier.CurrentRow != null)
        {
          if (this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index].Cells["SName"].Value != null)
            lsSupplier.Service = this.session.Get<Service>((object) (short) this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index].Cells["SName"].Value);
        }
        else
        {
          lsSupplier.Supplier = (Supplier) null;
          lsSupplier.Service = (Service) null;
        }
        IList<LsSupplier> lsSupplierList = (IList<LsSupplier>) new List<LsSupplier>();
        if ((uint) this.dgvLsSupplier.Rows.Count > 0U)
          lsSupplierList = (IList<LsSupplier>) (this.dgvLsSupplier.DataSource as List<LsSupplier>);
        lsSupplierList.Add(lsSupplier);
        this.dgvLsSupplier.Columns.Clear();
        this.dgvLsSupplier.DataSource = (object) null;
        this.dgvLsSupplier.DataSource = (object) lsSupplierList;
        this.dgvLsSupplierComboBoxField();
        this.dgvLsSupplier.Rows[this.dgvLsSupplier.Rows.Count - 1].Selected = true;
        this.InsertRecord = true;
        this.dgvLsSupplier.CurrentCell = this.dgvLsSupplier.Rows[this.dgvLsSupplier.Rows.Count - 1].Cells[0];
        this.LoadSettings();
      }
    }

    private void InsertAllLsSupplier(DateTime dBeg, DateTime dEnd)
    {
      if (this.lsService == null)
      {
        int num = (int) MessageBox.Show("Не выбрана услуга!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        IList<Service> serviceList = this.session.CreateCriteria(typeof (Service)).Add((ICriterion) Restrictions.Not((ICriterion) Restrictions.Eq("ServiceId", (object) this.lsService.Service.ServiceId))).Add((ICriterion) Restrictions.Eq("Root", (object) this.lsService.Service.ServiceId)).AddOrder(Order.Asc("Root")).List<Service>();
        IList<LsSupplier> lsSupplierList = (IList<LsSupplier>) new List<LsSupplier>();
        if ((uint) this.dgvLsSupplier.Rows.Count > 0U)
          lsSupplierList = (IList<LsSupplier>) (this.dgvLsSupplier.DataSource as List<LsSupplier>);
        this.InsertServiceCount = serviceList.Count;
        foreach (Service service in (IEnumerable<Service>) serviceList)
        {
          LsSupplier lsSupplier = new LsSupplier();
          lsSupplier.LsClient = this.lsClient;
          lsSupplier.Service = service;
          lsSupplier.IsInsert = true;
          lsSupplier.IsEdit = true;
          if (this.PastTime)
          {
            lsSupplier.Period = this.CurrentPeriod;
            lsSupplier.DBeg = dBeg;
            lsSupplier.DEnd = KvrplHelper.GetLastDayPeriod(this.MonthClosed.PeriodName.Value);
          }
          else
          {
            lsSupplier.Period = this.session.Get<Period>((object) 0);
            lsSupplier.DBeg = dBeg;
            lsSupplier.DEnd = dEnd;
          }
          lsSupplier.Supplier = (Supplier) null;
          lsSupplierList.Add(lsSupplier);
        }
        this.dgvLsSupplier.Columns.Clear();
        this.dgvLsSupplier.DataSource = (object) null;
        this.dgvLsSupplier.DataSource = (object) lsSupplierList;
        this.dgvLsSupplierComboBoxField();
        this.dgvLsSupplier.Rows[this.dgvLsSupplier.Rows.Count - 1].Selected = true;
        this.InsertRecord = true;
        this.dgvLsSupplier.CurrentCell = this.dgvLsSupplier.Rows[this.dgvLsSupplier.Rows.Count - 1].Cells[0];
        this.LoadSettings();
      }
    }

    private bool SaveAllLsSupplier()
    {
      this.session.Clear();
      string text = "";
      foreach (DataGridViewRow row in (IEnumerable) this.dgvLsSupplier.Rows)
      {
        if (row.Index >= this.dgvLsSupplier.Rows.Count - this.InsertServiceCount)
        {
          LsSupplier lsSupplier1 = new LsSupplier();
          LsSupplier dataBoundItem = (LsSupplier) row.DataBoundItem;
          dataBoundItem.Uname = Options.Login;
          LsSupplier lsSupplier2 = dataBoundItem;
          DateTime dateTime1 = DateTime.Now;
          DateTime date = dateTime1.Date;
          lsSupplier2.Dedit = date;
          try
          {
            dataBoundItem.DBeg = Convert.ToDateTime(row.Cells["MDBeg"].Value);
          }
          catch
          {
            text += string.Format("Составляющая: {0}  {1}", row.Cells["SName"].FormattedValue, (object) " Некорректный формат даты! \n");
            goto label_60;
          }
          try
          {
            dataBoundItem.DEnd = Convert.ToDateTime(row.Cells["MDEnd"].Value);
          }
          catch
          {
            text += string.Format("Составляющая: {0}  {1}", row.Cells["SName"].FormattedValue, (object) " Некорректный формат даты! \n");
            goto label_60;
          }
          DateTime dbeg1 = dataBoundItem.DBeg;
          dateTime1 = DateTime.Now;
          DateTime dateTime2 = dateTime1.AddYears(-3);
          int num1;
          if (!(dbeg1 <= dateTime2))
          {
            DateTime dbeg2 = dataBoundItem.DBeg;
            dateTime1 = DateTime.Now;
            DateTime dateTime3 = dateTime1.AddYears(3);
            num1 = dbeg2 >= dateTime3 ? 1 : 0;
          }
          else
            num1 = 1;
          if (num1 != 0)
          {
            text += string.Format("Составляющая: {0}  {1}", row.Cells["SName"].FormattedValue, (object) " Некорректная дата начала! \n");
          }
          else
          {
            dataBoundItem.Period = !this.PastTime ? this.session.Get<Period>((object) 0) : this.CurrentPeriod;
            if (row.Cells["Recipient"].Value != null && Convert.ToInt32(row.Cells["Recipient"].Value) != 0 || row.Cells["Perfomer"].Value != null && (uint) Convert.ToInt32(row.Cells["Perfomer"].Value) > 0U)
            {
              dataBoundItem.Supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(row.Cells["Recipient"].Value), (object) Convert.ToInt32(row.Cells["Perfomer"].Value))).List<Supplier>()[0];
              if (row.Cells["SName"].Value != null)
              {
                dataBoundItem.Service = this.session.Get<Service>((object) (short) row.Cells["SName"].Value);
                if (dataBoundItem.DBeg > dataBoundItem.DEnd)
                {
                  this.session.Clear();
                  this.session = Domain.CurrentSession;
                  text += string.Format("Составляющая: {0}  {1}", row.Cells["SName"].FormattedValue, (object) " Не могу сохранить текущую запись, т.к. дата начала больше даты окончания! \n");
                }
                else
                {
                  DateTime? periodName;
                  if (!this.PastTime)
                  {
                    if (!this.InsertRecord && dataBoundItem.DBeg <= this.LastDayMonthClosed && dataBoundItem.DBeg != this.OldLsSupplier.DBeg)
                    {
                      this.session.Clear();
                      this.session = Domain.CurrentSession;
                      text += string.Format("Составляющая: {0}  {1}", row.Cells["SName"].FormattedValue, (object) " Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду! \n");
                      goto label_60;
                    }
                    else if (!this.InsertRecord && (this.OldLsSupplier.DBeg < this.LastDayMonthClosed && this.OldLsSupplier.DEnd < this.LastDayMonthClosed))
                    {
                      this.session.Clear();
                      this.session = Domain.CurrentSession;
                      text += string.Format("Составляющая: {0}  {1}", row.Cells["SName"].FormattedValue, (object) " Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду! \n");
                      goto label_60;
                    }
                    else if (!this.InsertRecord && (this.OldLsSupplier.DBeg < this.LastDayMonthClosed && (this.OldLsSupplier.DBeg != dataBoundItem.DBeg || this.OldLsSupplier.Service != dataBoundItem.Service || this.OldLsSupplier.Supplier != dataBoundItem.Supplier) || this.OldLsSupplier.DEnd < this.LastDayMonthClosed && (this.OldLsSupplier.DEnd != dataBoundItem.DEnd || this.OldLsSupplier.Service != dataBoundItem.Service || this.OldLsSupplier.Supplier != dataBoundItem.Supplier)))
                    {
                      this.session.Clear();
                      this.session = Domain.CurrentSession;
                      text += string.Format("Составляющая: {0}  {1}", row.Cells["SName"].FormattedValue, (object) " Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду! \n");
                      goto label_60;
                    }
                    else if (dataBoundItem.DEnd < this.LastDayMonthClosed)
                    {
                      this.session.Clear();
                      this.session = Domain.CurrentSession;
                      text += string.Format("Составляющая: {0}  {1}", row.Cells["SName"].FormattedValue, (object) " Не могу сохранить текущую запись, т.к. дата окончания принадлежит закрытому периоду! \n");
                      goto label_60;
                    }
                    else
                    {
                      int num2;
                      if (this.InsertRecord)
                      {
                        dateTime1 = dataBoundItem.DBeg;
                        periodName = this.NextMonthClosed.PeriodName;
                        num2 = periodName.HasValue ? (dateTime1 < periodName.GetValueOrDefault() ? 1 : 0) : 0;
                      }
                      else
                        num2 = 0;
                      if (num2 != 0)
                      {
                        this.session.Clear();
                        this.session = Domain.CurrentSession;
                        text += string.Format("Составляющая: {0}  {1}", row.Cells["SName"].FormattedValue, (object) " Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду! \n");
                        goto label_60;
                      }
                    }
                  }
                  else
                  {
                    DateTime dbeg2 = dataBoundItem.DBeg;
                    periodName = this.MonthClosed.PeriodName;
                    DateTime lastDayPeriod1 = KvrplHelper.GetLastDayPeriod(periodName.Value);
                    int num2;
                    if (!(dbeg2 > lastDayPeriod1))
                    {
                      DateTime dend = dataBoundItem.DEnd;
                      periodName = this.MonthClosed.PeriodName;
                      DateTime lastDayPeriod2 = KvrplHelper.GetLastDayPeriod(periodName.Value);
                      num2 = dend > lastDayPeriod2 ? 1 : 0;
                    }
                    else
                      num2 = 1;
                    if (num2 != 0)
                    {
                      this.session.Clear();
                      this.session = Domain.CurrentSession;
                      text += string.Format("Составляющая: {0}  {1}", row.Cells["SName"].FormattedValue, (object) " Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду! \n");
                      goto label_60;
                    }
                    else
                    {
                      periodName = this.CurrentPeriod.PeriodName;
                      DateTime dateTime3 = periodName.Value;
                      periodName = this.MonthClosed.PeriodName;
                      DateTime lastDayPeriod2 = KvrplHelper.GetLastDayPeriod(periodName.Value);
                      if (dateTime3 < lastDayPeriod2)
                      {
                        this.session.Clear();
                        this.session = Domain.CurrentSession;
                        text += string.Format("Составляющая: {0}  {1}", row.Cells["SName"].FormattedValue, (object) " Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду! \n");
                        goto label_60;
                      }
                    }
                  }
                  if (this.InsertRecord)
                  {
                    using (ITransaction transaction = this.session.BeginTransaction())
                    {
                      try
                      {
                        this.session.Save((object) dataBoundItem);
                        this.session.Flush();
                        transaction.Commit();
                        if (this.PastTime && Convert.ToInt32(KvrplHelper.BaseValue(32, this.lsClient.Company)) == 1)
                        {
                          if (Convert.ToInt32(KvrplHelper.BaseValue(1, this.lsClient.Company)) == 28)
                          {
                            if (MessageBox.Show("Внести основание?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                              FrmArgument frmArgument = new FrmArgument();
                              int num2 = (int) frmArgument.ShowDialog();
                              LsSupplier supplier = dataBoundItem;
                              LsClient lsClient = dataBoundItem.LsClient;
                              int num3 = 1;
                              string notetext = frmArgument.Argument();
                              int num4 = this.PastTime ? 1 : 0;
                              periodName = this.MonthClosed.PeriodName;
                              DateTime monthClosed = periodName.Value;
                              KvrplHelper.SaveSupplierToNoteBook(supplier, lsClient, (short) num3, notetext, num4 != 0, monthClosed);
                              frmArgument.Dispose();
                            }
                          }
                          else
                          {
                            LsSupplier supplier = dataBoundItem;
                            LsClient lsClient = dataBoundItem.LsClient;
                            int num2 = 1;
                            string notetext = "";
                            int num3 = this.PastTime ? 1 : 0;
                            periodName = this.MonthClosed.PeriodName;
                            DateTime monthClosed = periodName.Value;
                            KvrplHelper.SaveSupplierToNoteBook(supplier, lsClient, (short) num2, notetext, num3 != 0, monthClosed);
                          }
                        }
                      }
                      catch (Exception ex)
                      {
                        this.session.Clear();
                        this.session = Domain.CurrentSession;
                        transaction.Rollback();
                        text += string.Format("Составляющая: {0}  {1}", row.Cells["SName"].FormattedValue, (object) " Не могу сохранить текущую запись! \n");
                        goto label_60;
                      }
                    }
                  }
                  if (this.OldLsSupplier != null && !this.InsertRecord)
                  {
                    try
                    {
                      this.session.CreateSQLQuery("update DBA.LsSupplier set  service_id=:service_id,  dbeg=:dbeg, dend=:dend, supplier_id=:supplier_id, uname=:uname, dedit=:dedit  where client_id=:client_id and period_id=:period_id and service_id=:service1_id and dbeg=:dbeg1").SetParameter<short>("service_id", dataBoundItem.Service.ServiceId).SetParameter<string>("dbeg", KvrplHelper.DateToBaseFormat(dataBoundItem.DBeg)).SetParameter<string>("dend", KvrplHelper.DateToBaseFormat(dataBoundItem.DEnd)).SetParameter<int>("supplier_id", dataBoundItem.Supplier.SupplierId).SetParameter<string>("uname", Options.Login).SetParameter<string>("dedit", KvrplHelper.DateToBaseFormat(DateTime.Now)).SetParameter<int>("client_id", this.lsService.Client.ClientId).SetParameter<int>("period_id", dataBoundItem.Period.PeriodId).SetParameter<short>("service1_id", this.OldLsSupplier.Service.ServiceId).SetParameter<DateTime>("dbeg1", this.OldLsSupplier.DBeg).ExecuteUpdate();
                      if (Convert.ToInt32(KvrplHelper.BaseValue(32, this.lsClient.Company)) == 1)
                      {
                        LsSupplier supplier = dataBoundItem;
                        LsSupplier oldLsSupplier = this.OldLsSupplier;
                        LsClient lsClient = dataBoundItem.LsClient;
                        int num2 = 2;
                        int num3 = this.PastTime ? 1 : 0;
                        periodName = this.MonthClosed.PeriodName;
                        DateTime monthClosed = periodName.Value;
                        KvrplHelper.ChangeSupplierToNoteBook(supplier, oldLsSupplier, lsClient, (short) num2, num3 != 0, monthClosed);
                      }
                    }
                    catch
                    {
                      this.session.Clear();
                      this.session = Domain.CurrentSession;
                      text += string.Format("Составляющая: {0}  {1}", row.Cells["SName"].FormattedValue, (object) " Не могу сохранить текущую запись! \n");
                      goto label_60;
                    }
                  }
                }
              }
              else
                text += string.Format("Составляющая: {0}  {1}", row.Cells["SName"].FormattedValue, (object) " Не выбрана составляющая услуги! \n");
            }
            else
              text += string.Format("Составляющая: {0}  {1}", row.Cells["SName"].FormattedValue, (object) " Не выбран поставщик! \n");
          }
        }
label_60:;
      }
      if (text != "")
      {
        int num = (int) MessageBox.Show(text);
      }
      return true;
    }

    private bool SaveLsSupplier()
    {
      if (this.dgvLsSupplier.CurrentRow == null || this.OldLsSupplier == null)
        return false;
      this.session.Clear();
      DataGridViewRow dataGridViewRow = new DataGridViewRow();
      DataGridViewRow row = this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index];
      LsSupplier lsSupplier = new LsSupplier();
      lsSupplier.LsClient = this.lsClient;
      lsSupplier.Uname = Options.Login;
      lsSupplier.Dedit = DateTime.Now.Date;
      try
      {
        lsSupplier.DBeg = Convert.ToDateTime(row.Cells["MDBeg"].Value);
      }
      catch
      {
        int num = (int) MessageBox.Show("Некорректный формат даты!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      try
      {
        lsSupplier.DEnd = Convert.ToDateTime(row.Cells["MDEnd"].Value);
      }
      catch
      {
        int num = (int) MessageBox.Show("Некорректный формат даты!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      DateTime dateTime1;
      int num1;
      if (lsSupplier.DBeg != this.OldLsSupplier.DBeg)
      {
        DateTime dbeg1 = lsSupplier.DBeg;
        dateTime1 = DateTime.Now;
        DateTime dateTime2 = dateTime1.AddYears(-3);
        if (!(dbeg1 <= dateTime2))
        {
          DateTime dbeg2 = lsSupplier.DBeg;
          dateTime1 = DateTime.Now;
          DateTime dateTime3 = dateTime1.AddYears(3);
          num1 = dbeg2 >= dateTime3 ? 1 : 0;
        }
        else
          num1 = 1;
      }
      else
        num1 = 0;
      if (num1 != 0 && MessageBox.Show("Дата начала отличается от текущей более, чем на 3 года. Продолжить сохранение?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
        return false;
      lsSupplier.Period = !this.PastTime ? this.session.Get<Period>((object) 0) : this.CurrentPeriod;
      if (row.Cells["Recipient"].Value != null && Convert.ToInt32(row.Cells["Recipient"].Value) != 0 || row.Cells["Perfomer"].Value != null && (uint) Convert.ToInt32(row.Cells["Perfomer"].Value) > 0U)
      {
        lsSupplier.Supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(row.Cells["Recipient"].Value), (object) Convert.ToInt32(row.Cells["Perfomer"].Value))).List<Supplier>()[0];
        if (row.Cells["SName"].Value != null)
        {
          lsSupplier.Service = this.session.Get<Service>((object) (short) row.Cells["SName"].Value);
          if (lsSupplier.DBeg > lsSupplier.DEnd)
          {
            int num2 = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. дата начала больше даты окончания", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.session.Clear();
            this.session = Domain.CurrentSession;
            return !this.InsertRecord;
          }
          DateTime? periodName;
          if (!this.PastTime)
          {
            if (!this.InsertRecord && lsSupplier.DBeg <= this.LastDayMonthClosed && lsSupplier.DBeg != this.OldLsSupplier.DBeg)
            {
              int num2 = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.session.Clear();
              this.session = Domain.CurrentSession;
              return true;
            }
            if (!this.InsertRecord && (this.OldLsSupplier.DBeg < this.LastDayMonthClosed && this.OldLsSupplier.DEnd < this.LastDayMonthClosed))
            {
              int num2 = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.session.Clear();
              this.session = Domain.CurrentSession;
              return true;
            }
            if (!this.InsertRecord && (this.OldLsSupplier.DBeg < this.LastDayMonthClosed && (this.OldLsSupplier.DBeg != lsSupplier.DBeg || (int) this.OldLsSupplier.Service.ServiceId != (int) lsSupplier.Service.ServiceId || this.OldLsSupplier.Supplier.SupplierId != lsSupplier.Supplier.SupplierId) || this.OldLsSupplier.DEnd < this.LastDayMonthClosed && (this.OldLsSupplier.DEnd != lsSupplier.DEnd || (int) this.OldLsSupplier.Service.ServiceId != (int) lsSupplier.Service.ServiceId || this.OldLsSupplier.Supplier.SupplierId != lsSupplier.Supplier.SupplierId)))
            {
              int num2 = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.session.Clear();
              this.session = Domain.CurrentSession;
              return true;
            }
            if (lsSupplier.DEnd < this.LastDayMonthClosed)
            {
              int num2 = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. дата окончания принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.session.Clear();
              this.session = Domain.CurrentSession;
              return !this.InsertRecord;
            }
            int num3;
            if (this.InsertRecord)
            {
              dateTime1 = lsSupplier.DBeg;
              periodName = this.NextMonthClosed.PeriodName;
              num3 = periodName.HasValue ? (dateTime1 < periodName.GetValueOrDefault() ? 1 : 0) : 0;
            }
            else
              num3 = 0;
            if (num3 != 0)
            {
              int num2 = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.session.Clear();
              this.session = Domain.CurrentSession;
              return false;
            }
          }
          else
          {
            DateTime dbeg = lsSupplier.DBeg;
            periodName = this.MonthClosed.PeriodName;
            DateTime lastDayPeriod1 = KvrplHelper.GetLastDayPeriod(periodName.Value);
            int num2;
            if (!(dbeg > lastDayPeriod1))
            {
              DateTime dend = lsSupplier.DEnd;
              periodName = this.MonthClosed.PeriodName;
              DateTime lastDayPeriod2 = KvrplHelper.GetLastDayPeriod(periodName.Value);
              num2 = dend > lastDayPeriod2 ? 1 : 0;
            }
            else
              num2 = 1;
            if (num2 != 0)
            {
              int num3 = (int) MessageBox.Show("Не могу сохранить текущую запись! Проверьте корректность введённых дат", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.session.Clear();
              this.session = Domain.CurrentSession;
              return !this.InsertRecord;
            }
            periodName = this.CurrentPeriod.PeriodName;
            DateTime dateTime2 = periodName.Value;
            periodName = this.MonthClosed.PeriodName;
            DateTime lastDayPeriod3 = KvrplHelper.GetLastDayPeriod(periodName.Value);
            if (dateTime2 < lastDayPeriod3)
            {
              int num3 = (int) MessageBox.Show("Не могу сохранить текущую запись! Проверьте корректность введённых дат", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.session.Clear();
              this.session = Domain.CurrentSession;
              return !this.InsertRecord;
            }
            periodName = this.MonthClosed.PeriodName;
            if (KvrplHelper.GetLastDayPeriod(periodName.Value) - lsSupplier.DBeg > new TimeSpan(730, 0, 0, 0) && MessageBox.Show("Дата начала отличается от даты закрытого периода более, чем на 2 года. Продолжить сохранение? ", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
              this.session.Clear();
              this.session = Domain.CurrentSession;
              return false;
            }
          }
          if (this.InsertRecord)
          {
            using (ITransaction transaction = this.session.BeginTransaction())
            {
              try
              {
                this.session.Save((object) lsSupplier);
                this.session.Flush();
                transaction.Commit();
                if (this.PastTime && Convert.ToInt32(KvrplHelper.BaseValue(32, this.lsClient.Company)) == 1)
                {
                  if (Convert.ToInt32(KvrplHelper.BaseValue(1, this.lsClient.Company)) == 28)
                  {
                    if (MessageBox.Show("Внести основание?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                      FrmArgument frmArgument = new FrmArgument();
                      int num2 = (int) frmArgument.ShowDialog();
                      LsSupplier supplier = lsSupplier;
                      LsClient lsClient = lsSupplier.LsClient;
                      int num3 = 1;
                      string notetext = frmArgument.Argument();
                      int num4 = this.PastTime ? 1 : 0;
                      periodName = this.MonthClosed.PeriodName;
                      DateTime monthClosed = periodName.Value;
                      KvrplHelper.SaveSupplierToNoteBook(supplier, lsClient, (short) num3, notetext, num4 != 0, monthClosed);
                      frmArgument.Dispose();
                    }
                  }
                  else
                  {
                    LsSupplier supplier = lsSupplier;
                    LsClient lsClient = lsSupplier.LsClient;
                    int num2 = 1;
                    string notetext = "";
                    int num3 = this.PastTime ? 1 : 0;
                    periodName = this.MonthClosed.PeriodName;
                    DateTime monthClosed = periodName.Value;
                    KvrplHelper.SaveSupplierToNoteBook(supplier, lsClient, (short) num2, notetext, num3 != 0, monthClosed);
                  }
                }
              }
              catch (Exception ex)
              {
                this.session.Clear();
                this.session = Domain.CurrentSession;
                int num2 = (int) MessageBox.Show("Не могу сохранить текущую запись", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                transaction.Rollback();
                return true;
              }
            }
          }
          if (this.OldLsSupplier != null && !this.InsertRecord)
          {
            try
            {
              this.session.CreateSQLQuery("update DBA.LsSupplier set  service_id=:service_id,  dbeg=:dbeg, dend=:dend, supplier_id=:supplier_id, uname=:uname, dedit=:dedit  where client_id=:client_id and period_id=:period_id and service_id=:service1_id and dbeg=:dbeg1").SetParameter<short>("service_id", lsSupplier.Service.ServiceId).SetParameter<string>("dbeg", KvrplHelper.DateToBaseFormat(lsSupplier.DBeg)).SetParameter<string>("dend", KvrplHelper.DateToBaseFormat(lsSupplier.DEnd)).SetParameter<int>("supplier_id", lsSupplier.Supplier.SupplierId).SetParameter<string>("uname", Options.Login).SetParameter<string>("dedit", KvrplHelper.DateToBaseFormat(DateTime.Now)).SetParameter<int>("client_id", this.lsService.Client.ClientId).SetParameter<int>("period_id", lsSupplier.Period.PeriodId).SetParameter<short>("service1_id", this.OldLsSupplier.Service.ServiceId).SetParameter<DateTime>("dbeg1", this.OldLsSupplier.DBeg).ExecuteUpdate();
              if (Convert.ToInt32(KvrplHelper.BaseValue(32, this.lsClient.Company)) == 1)
              {
                LsSupplier supplier = lsSupplier;
                LsSupplier oldLsSupplier = this.OldLsSupplier;
                LsClient lsClient = lsSupplier.LsClient;
                int num2 = 2;
                int num3 = this.PastTime ? 1 : 0;
                periodName = this.MonthClosed.PeriodName;
                DateTime monthClosed = periodName.Value;
                KvrplHelper.ChangeSupplierToNoteBook(supplier, oldLsSupplier, lsClient, (short) num2, num3 != 0, monthClosed);
              }
            }
            catch
            {
              this.session.Clear();
              this.session = Domain.CurrentSession;
              int num2 = (int) MessageBox.Show("Не могу сохранить текущую запись", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return true;
            }
          }
          this.InsertRecord = false;
          return true;
        }
        int num5 = (int) MessageBox.Show("Не выбрана составляющая услуги!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      int num6 = (int) MessageBox.Show("Не выбран поставщик!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return false;
    }

    private bool DeleteLsSupplier()
    {
      if (this.dgvLsSupplier.Rows.Count <= 0 || this.dgvLsSupplier.CurrentRow == null || this.dgvLsSupplier.CurrentRow.Index < 0 || MessageBox.Show("Вы уверены, что хотите удалить запись", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return false;
      DataGridViewRow dataGridViewRow = new DataGridViewRow();
      DataGridViewRow row1 = this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index];
      LsSupplier lsSupplier = new LsSupplier();
      LsSupplier dataBoundItem = (LsSupplier) row1.DataBoundItem;
      int index = this.dgvLsSupplier.CurrentRow.Index;
      bool flag = true;
      foreach (DataGridViewRow row2 in (IEnumerable) this.dgvLsSupplier.Rows)
      {
        if (row2.Index != index && dataBoundItem.DBeg == Convert.ToDateTime(row2.Cells["MDBeg"].Value) && (int) dataBoundItem.Service.ServiceId == (int) Convert.ToInt16(row2.Cells["SName"].Value))
        {
          flag = false;
          break;
        }
      }
      if (flag)
      {
        DateTime? periodName;
        if (!this.PastTime)
        {
          if (dataBoundItem.DBeg <= this.LastDayMonthClosed || dataBoundItem.DEnd <= this.LastDayMonthClosed)
          {
            int num = (int) MessageBox.Show("Не могу удалить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.session.Clear();
            this.session = Domain.CurrentSession;
            return false;
          }
        }
        else
        {
          DateTime dateTime1 = this.CurrentPeriod.PeriodName.Value;
          periodName = this.MonthClosed.PeriodName;
          DateTime dateTime2 = periodName.Value;
          if (dateTime1 <= dateTime2)
          {
            int num = (int) MessageBox.Show("Не могу удалить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.session.Clear();
            this.session = Domain.CurrentSession;
            return false;
          }
        }
        this.session.Clear();
        this.session = Domain.CurrentSession;
        using (ITransaction transaction = this.session.BeginTransaction())
        {
          try
          {
            this.session.Delete((object) dataBoundItem);
            this.session.Flush();
            transaction.Commit();
            if (Convert.ToInt32(KvrplHelper.BaseValue(32, this.lsClient.Company)) == 1)
            {
              LsSupplier supplier = dataBoundItem;
              LsClient lsClient = dataBoundItem.LsClient;
              int num1 = 3;
              int num2 = this.PastTime ? 1 : 0;
              periodName = this.MonthClosed.PeriodName;
              DateTime monthClosed = periodName.Value;
              KvrplHelper.DeleteSupplierFromNoteBook(supplier, lsClient, (short) num1, num2 != 0, monthClosed);
            }
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Не могу удалить текущую запись!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.session.Clear();
            this.session = Domain.CurrentSession;
            transaction.Rollback();
            return false;
          }
        }
        this.session.Clear();
      }
      return true;
    }

    private void SortGridLsSupplierColumns()
    {
      if ((uint) this.dgvLsSupplier.Columns.Count <= 0U)
        return;
      this.dgvLsSupplier.Columns[1].DisplayIndex = 3;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(40, 2, this.lsClient.Company, true))
        return;
      this.InsertServiceCount = 0;
      switch (MessageBox.Show("Вы хотите добавить все составляющие?", "Внимание", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Yes:
          DateTime dateTime = new DateTime();
          DateTime dEnd = Convert.ToDateTime("2999-12-31");
          DateTime s_val = !this.PastTime ? (this.CurrentPeriod != null ? this.CurrentPeriod.PeriodName.Value : dateTime) : this.MonthClosed.PeriodName.Value;
          if (this.lsClient.Complex.IdFk == Options.ComplexArenda.IdFk)
          {
            IList<LsArenda> lsArendaList = this.session.CreateCriteria(typeof (LsArenda)).Add((ICriterion) Restrictions.Eq("LsClient.ClientId", (object) this.lsClient.ClientId)).List<LsArenda>();
            if (lsArendaList.Count > 0)
            {
              if (lsArendaList[0].DBeg <= s_val && lsArendaList[0].DEnd >= s_val)
                dEnd = lsArendaList[0].DEnd;
              else if (lsArendaList[0].DBeg > s_val)
              {
                dEnd = lsArendaList[0].DEnd;
                s_val = lsArendaList[0].DBeg;
              }
            }
          }
          if (!InputBox.Query("Введите дату начала действия", "Значение в формате ДД.ММ.ГГГГ", ref s_val))
            break;
          this.InsertAllLsSupplier(s_val, dEnd);
          break;
        case DialogResult.No:
          this.InsertLsSupplier();
          break;
      }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(40, 2, this.lsClient.Company, true))
        return;
      if (this.InsertServiceCount > 0)
      {
        if (MessageBox.Show("Все незаполненные записи будут удалены после сохранения! Продолжить?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK || !this.SaveAllLsSupplier())
          return;
        this.LoadLsSupplier();
      }
      else
      {
        bool flag = false;
        foreach (DataGridViewRow row in (IEnumerable) this.dgvLsSupplier.Rows)
        {
          if (((LsSupplier) row.DataBoundItem).IsEdit)
          {
            this.OldLsSupplier = new LsSupplier();
            foreach (LsSupplier lsSupplier in (IEnumerable<LsSupplier>) this.OldListLsSupplier)
            {
              if (lsSupplier.OldHashCode == ((LsSupplier) row.DataBoundItem).OldHashCode)
              {
                this.OldLsSupplier = lsSupplier;
                break;
              }
            }
            this.InsertRecord = ((LsSupplier) row.DataBoundItem).IsInsert;
            this.dgvLsSupplier.Rows[row.Index].Selected = true;
            this.dgvLsSupplier.CurrentCell = row.Cells[0];
            Application.DoEvents();
            if (!this.SaveLsSupplier())
              flag = true;
            else
              ((LsSupplier) row.DataBoundItem).IsEdit = false;
          }
        }
        if (!flag)
          this.LoadLsSupplier();
      }
    }

    private void dgvLsSupplier_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      if (this.dgvLsSupplier.CurrentRow == null)
        return;
      this.OldLsSupplier = new LsSupplier();
      this.OldLsSupplier.LsClient = ((LsSupplier) this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index].DataBoundItem).LsClient;
      this.OldLsSupplier.DBeg = ((LsSupplier) this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index].DataBoundItem).DBeg;
      this.OldLsSupplier.DEnd = ((LsSupplier) this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index].DataBoundItem).DEnd;
      this.OldLsSupplier.Period = ((LsSupplier) this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index].DataBoundItem).Period;
      this.OldLsSupplier.Service = ((LsSupplier) this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index].DataBoundItem).Service == null ? (Service) null : ((LsSupplier) this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index].DataBoundItem).Service;
      this.OldLsSupplier.Supplier = ((LsSupplier) this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index].DataBoundItem).Supplier == null ? (Supplier) null : ((LsSupplier) this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index].DataBoundItem).Supplier;
      this.OldLsSupplier.Uname = ((LsSupplier) this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index].DataBoundItem).Uname;
      this.btnSave.Enabled = true;
      ((LsSupplier) this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index].DataBoundItem).IsEdit = true;
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(40, 2, this.lsClient.Company, true) || !this.DeleteLsSupplier())
        return;
      this.LoadLsSupplier();
    }

    private void dgvLsSupplier_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (this.dgvLsSupplier.DataSource == null)
        return;
      DataGridViewRow row = (sender as DataGridView).Rows[e.RowIndex];
      DateTime dbeg = ((LsSupplier) row.DataBoundItem).DBeg;
      DateTime? periodName = this.NextMonthClosed.PeriodName;
      DateTime dateTime1 = KvrplHelper.LastDay(periodName.Value);
      int num;
      if (dbeg <= dateTime1)
      {
        DateTime dend = ((LsSupplier) row.DataBoundItem).DEnd;
        periodName = this.NextMonthClosed.PeriodName;
        DateTime dateTime2 = periodName.Value;
        num = dend >= dateTime2 ? 1 : 0;
      }
      else
        num = 0;
      if (num != 0)
      {
        row.DefaultCellStyle.BackColor = Color.PapayaWhip;
        row.DefaultCellStyle.ForeColor = Color.Black;
      }
      else
      {
        row.DefaultCellStyle.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
        row.DefaultCellStyle.ForeColor = Color.Gray;
      }
    }

    private void dgvLsSupplier_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.RowIndex == -1 || e.ColumnIndex == -1)
        return;
      this.dgvLsSupplier.Rows[e.RowIndex].Selected = true;
      this.dgvLsSupplier.CurrentCell = this.dgvLsSupplier.Rows[e.RowIndex].Cells[e.ColumnIndex];
    }

    private void скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem_Click(object sender, EventArgs e)
    {
      bool flag = true;
      short num1;
      if (sender.ToString() == "Удалить запись из выбранных объектов")
      {
        flag = false;
        num1 = (short) 2;
      }
      else if (sender.ToString() == "Обновить запись в выбранных объектах")
      {
        num1 = (short) 3;
      }
      else
      {
        flag = true;
        num1 = (short) 1;
      }
      if (this.dgvLsSupplier.Rows.Count <= 0 || this.dgvLsSupplier.CurrentRow.Index < 0)
        return;
      if (!this.PastTime)
      {
        if (((LsSupplier) this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index].DataBoundItem).DEnd > this.LastDayMonthClosed || (int) num1 == 3 && ((LsSupplier) this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index].DataBoundItem).DEnd >= this.LastDayMonthClosed)
        {
          if (((LsSupplier) this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index].DataBoundItem).DBeg <= this.LastDayMonthClosed)
            ((LsSupplier) this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index].DataBoundItem).DBeg = this.LastDayMonthClosed.AddDays(1.0);
          FrmChooseObject frmChooseObject = new FrmChooseObject((LsSupplier) this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index].DataBoundItem);
          frmChooseObject.Save = flag;
          frmChooseObject.CodeOperation = num1;
          frmChooseObject.MonthClosed = this.MonthClosed;
          int num2 = (int) frmChooseObject.ShowDialog();
          frmChooseObject.Dispose();
        }
        else
        {
          int num3 = (int) MessageBox.Show("Не могу выполнить операцию, так как запись принадлежит закрытому периоду!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
      }
      else if (((LsSupplier) this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index].DataBoundItem).Period.PeriodId > this.MonthClosed.PeriodId)
      {
        FrmChooseObject frmChooseObject = new FrmChooseObject((LsSupplier) this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index].DataBoundItem);
        frmChooseObject.Save = flag;
        frmChooseObject.CodeOperation = num1;
        frmChooseObject.MonthClosed = this.MonthClosed;
        int num2 = (int) frmChooseObject.ShowDialog();
        frmChooseObject.Dispose();
      }
      else
      {
        int num4 = (int) MessageBox.Show("Не могу выполнить операцию, так как запись принадлежит закрытому периоду!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void chbArhiv_CheckedChanged(object sender, EventArgs e)
    {
      this.LoadLsSupplier();
    }

    private void btnPastTime_Click(object sender, EventArgs e)
    {
      if (!this.PastTime)
      {
        this.btnPastTime.BackColor = Color.DarkOrange;
        this.PastTime = true;
        this.tmr1.Start();
      }
      else
      {
        this.btnPastTime.BackColor = this.panel1.BackColor;
        this.PastTime = false;
        this.tmr1.Stop();
      }
      this.LoadLsSupplier();
      this.miInPatTime.Visible = !this.PastTime;
    }

    private void tmr1_Tick(object sender, EventArgs e)
    {
      if (this.PastTime)
      {
        if (this.lblPastTime.ForeColor == Color.DarkOrange)
          this.lblPastTime.ForeColor = this.pnUp.BackColor;
        else
          this.lblPastTime.ForeColor = Color.DarkOrange;
      }
      else
        this.lblPastTime.ForeColor = this.pnUp.BackColor;
    }

    private void dgvLsSupplier_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsLsSupplier.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsLsSupplier.Columns[this.MySettingsLsSupplier.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsLsSupplier.Save();
    }

    public void LoadSettings()
    {
      this.MySettingsLsSupplier.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvLsSupplier.Columns)
        this.MySettingsLsSupplier.GetMySettings(column);
    }

    private void dgvLsSupplier_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    private void dgvLsSupplier_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgvLsSupplier.Rows.Count <= 0 || this.dgvLsSupplier.CurrentRow == null || this.dgvLsSupplier.CurrentRow.Index < 0 || this.InitSupplier)
        return;
      this.btnDelete.Enabled = false;
    }

    private void скопироватьВПрошлоеВремяToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (Options.Period.PeriodId < this.MonthClosed.PeriodId + 1)
      {
        int num1 = (int) MessageBox.Show("Невозможно скопировать запись в закрытом периоде", "Ошибка", MessageBoxButtons.OK);
      }
      else
      {
        this.session.Clear();
        try
        {
          LsSupplier dataBoundItem = (LsSupplier) this.dgvLsSupplier.CurrentRow.DataBoundItem;
          LsSupplier lsSupplier1 = new LsSupplier();
          lsSupplier1.LsClient = this.LsClient;
          lsSupplier1.Period = Options.Period;
          LsSupplier lsSupplier2 = lsSupplier1;
          DateTime? periodName = this.MonthClosed.PeriodName;
          DateTime dateTime1 = periodName.Value;
          lsSupplier2.DBeg = dateTime1;
          LsSupplier lsSupplier3 = lsSupplier1;
          periodName = this.MonthClosed.PeriodName;
          DateTime dateTime2 = periodName.Value;
          dateTime2 = dateTime2.AddMonths(1);
          DateTime dateTime3 = dateTime2.AddDays(-1.0);
          lsSupplier3.DEnd = dateTime3;
          lsSupplier1.Supplier = dataBoundItem.Supplier;
          lsSupplier1.Service = dataBoundItem.Service;
          lsSupplier1.Uname = Options.Login;
          this.session.Save((object) lsSupplier1);
          this.session.Flush();
          int num2 = (int) MessageBox.Show("Запись успешно сохранена", "", MessageBoxButtons.OK);
          if (Convert.ToInt32(KvrplHelper.BaseValue(32, this.LsClient.Company)) != 1)
            return;
          if (Convert.ToInt32(KvrplHelper.BaseValue(1, this.lsClient.Company)) == 28)
          {
            if (MessageBox.Show("Внести основание?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
              FrmArgument frmArgument = new FrmArgument();
              int num3 = (int) frmArgument.ShowDialog();
              LsSupplier supplier = lsSupplier1;
              LsClient lsClient = this.LsClient;
              int num4 = 1;
              string notetext = frmArgument.Argument();
              int num5 = 1;
              periodName = this.MonthClosed.PeriodName;
              DateTime monthClosed = periodName.Value;
              KvrplHelper.SaveSupplierToNoteBook(supplier, lsClient, (short) num4, notetext, num5 != 0, monthClosed);
              frmArgument.Dispose();
            }
          }
          else
          {
            LsSupplier supplier = lsSupplier1;
            LsClient lsClient = this.LsClient;
            int num3 = 1;
            string notetext = "";
            int num4 = 1;
            periodName = this.MonthClosed.PeriodName;
            DateTime monthClosed = periodName.Value;
            KvrplHelper.SaveSupplierToNoteBook(supplier, lsClient, (short) num3, notetext, num4 != 0, monthClosed);
          }
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show("Не удалось скопировать запись в прошлое время. Воспользуйтесь ручным вводом", "Ошибка", MessageBoxButtons.OK);
        }
      }
    }

    private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
    {
    }

    private void dgvLsSupplier_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (!this.dgvLsSupplier.IsCurrentCellDirty)
        return;
      this.dgvLsSupplier.CommitEdit(DataGridViewDataErrorContexts.Commit);
      if (this.dgvLsSupplier.CurrentCell.ColumnIndex == this.dgvLsSupplier.Rows[this.dgvLsSupplier.CurrentRow.Index].Cells["Recipient"].ColumnIndex)
      {
        this.session = Domain.CurrentSession;
        IList<BaseOrg> baseOrgList1 = this.session.CreateQuery(string.Format("select distinct new BaseOrg(d.Perfomer.BaseOrgId,d.Perfomer.NameOrgMin) from CmpSupplier s, Supplier d where s.SupplierOrg.SupplierId = d.SupplierId and d.Perfomer.BaseOrgId<>0 and s.Service.ServiceId={0} and d.Recipient.BaseOrgId={4}   and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={1} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{3}' and Param.ParamId=211) order by d.Perfomer.NameOrgMin", (object) this.lsService.Service.ServiceId, (object) this.lsClient.Company.CompanyId, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(this.NextMonthClosed.PeriodName.Value)), (object) KvrplHelper.DateToBaseFormat(this.NextMonthClosed.PeriodName.Value), (object) Convert.ToInt32(this.dgvLsSupplier.CurrentRow.Cells["Recipient"].Value))).List<BaseOrg>();
        if (Convert.ToInt32(this.dgvLsSupplier.CurrentRow.Cells["Recipient"].Value) == 0)
        {
          baseOrgList1.Insert(0, new BaseOrg(0, ""));
        }
        else
        {
          IList<BaseOrg> baseOrgList2 = this.session.CreateQuery(string.Format("select distinct new BaseOrg(d.Perfomer.BaseOrgId,d.Perfomer.NameOrgMin) from CmpSupplier s, Supplier d where s.SupplierOrg.SupplierId = d.SupplierId and d.Perfomer.BaseOrgId=0 and s.Service.ServiceId={0} and d.Recipient.BaseOrgId={4}   and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={1} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{3}' and Param.ParamId=211) order by d.Perfomer.NameOrgMin", (object) this.lsService.Service.ServiceId, (object) this.lsClient.Company.CompanyId, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(this.NextMonthClosed.PeriodName.Value)), (object) KvrplHelper.DateToBaseFormat(this.NextMonthClosed.PeriodName.Value), (object) Convert.ToInt32(this.dgvLsSupplier.CurrentRow.Cells["Recipient"].Value))).List<BaseOrg>();
          if (baseOrgList2.Count > 0)
            baseOrgList1.Insert(0, baseOrgList2[0]);
        }
        this.session.Clear();
        this.dgvLsSupplier.CurrentRow.Cells["Perfomer"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "BaseOrgId",
          DisplayMember = "NameOrgMinDop",
          DataSource = (object) baseOrgList1
        };
        this.dgvLsSupplier.CurrentRow.Cells["Perfomer"].Value = (object) baseOrgList1[0].BaseOrgId;
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
      this.panel1 = new Panel();
      this.btnPastTime = new Button();
      this.btnAdd = new Button();
      this.btnSave = new Button();
      this.btnDelete = new Button();
      this.label1 = new Label();
      this.pnUp = new Panel();
      this.chbArhiv = new CheckBox();
      this.lblPastTime = new Label();
      this.dgvLsSupplier = new DataGridView();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem = new ToolStripMenuItem();
      this.удалитьЗаписьИзВыбранныхОбъектовToolStripMenuItem = new ToolStripMenuItem();
      this.обновитьЗаписьВВыбранныхОбъектахToolStripMenuItem = new ToolStripMenuItem();
      this.miInPatTime = new ToolStripMenuItem();
      this.tmr1 = new Timer(this.components);
      this.panel1.SuspendLayout();
      this.pnUp.SuspendLayout();
      ((ISupportInitialize) this.dgvLsSupplier).BeginInit();
      this.contextMenuStrip1.SuspendLayout();
      this.SuspendLayout();
      this.panel1.BackColor = Color.Transparent;
      this.panel1.Controls.Add((Control) this.btnPastTime);
      this.panel1.Controls.Add((Control) this.btnAdd);
      this.panel1.Controls.Add((Control) this.btnSave);
      this.panel1.Controls.Add((Control) this.btnDelete);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 188);
      this.panel1.Margin = new Padding(4);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(905, 38);
      this.panel1.TabIndex = 0;
      this.btnPastTime.Image = (Image) Resources.time_24;
      this.btnPastTime.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnPastTime.Location = new Point(357, 4);
      this.btnPastTime.Name = "btnPastTime";
      this.btnPastTime.Size = new Size(152, 30);
      this.btnPastTime.TabIndex = 10;
      this.btnPastTime.Text = "Прошлое время";
      this.btnPastTime.TextAlign = ContentAlignment.MiddleRight;
      this.btnPastTime.UseVisualStyleBackColor = true;
      this.btnPastTime.Click += new EventHandler(this.btnPastTime_Click);
      this.btnAdd.Image = (Image) Resources.plus;
      this.btnAdd.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnAdd.Location = new Point(6, 4);
      this.btnAdd.Margin = new Padding(4);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(111, 30);
      this.btnAdd.TabIndex = 3;
      this.btnAdd.Text = "Добавить";
      this.btnAdd.TextAlign = ContentAlignment.MiddleRight;
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnSave.Image = (Image) Resources.Tick;
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.Location = new Point(237, 4);
      this.btnSave.Margin = new Padding(4);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(113, 30);
      this.btnSave.TabIndex = 2;
      this.btnSave.Text = "Сохранить";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnDelete.Image = (Image) Resources.minus;
      this.btnDelete.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnDelete.Location = new Point(123, 4);
      this.btnDelete.Margin = new Padding(4);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(108, 30);
      this.btnDelete.TabIndex = 1;
      this.btnDelete.Text = "Удалить";
      this.btnDelete.TextAlign = ContentAlignment.MiddleRight;
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.label1.Location = new Point(4, 6);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(98, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "Поставщики";
      this.pnUp.BackColor = Color.Transparent;
      this.pnUp.BorderStyle = BorderStyle.FixedSingle;
      this.pnUp.Controls.Add((Control) this.chbArhiv);
      this.pnUp.Controls.Add((Control) this.lblPastTime);
      this.pnUp.Controls.Add((Control) this.label1);
      this.pnUp.Dock = DockStyle.Top;
      this.pnUp.Location = new Point(0, 0);
      this.pnUp.Margin = new Padding(4);
      this.pnUp.Name = "pnUp";
      this.pnUp.Size = new Size(905, 30);
      this.pnUp.TabIndex = 2;
      this.chbArhiv.AutoSize = true;
      this.chbArhiv.Location = new Point(129, 5);
      this.chbArhiv.Margin = new Padding(4);
      this.chbArhiv.Name = "chbArhiv";
      this.chbArhiv.Size = new Size(65, 21);
      this.chbArhiv.TabIndex = 2;
      this.chbArhiv.Text = "Архив";
      this.chbArhiv.UseVisualStyleBackColor = true;
      this.chbArhiv.CheckedChanged += new EventHandler(this.chbArhiv_CheckedChanged);
      this.lblPastTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblPastTime.AutoSize = true;
      this.lblPastTime.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblPastTime.ForeColor = Color.DarkOrange;
      this.lblPastTime.Location = new Point(683, 5);
      this.lblPastTime.Margin = new Padding(4, 0, 4, 0);
      this.lblPastTime.Name = "lblPastTime";
      this.lblPastTime.Size = new Size(201, 17);
      this.lblPastTime.TabIndex = 1;
      this.lblPastTime.Text = "Режим прошлого времени";
      this.lblPastTime.Visible = false;
      this.dgvLsSupplier.BackgroundColor = Color.AliceBlue;
      this.dgvLsSupplier.BorderStyle = BorderStyle.None;
      this.dgvLsSupplier.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvLsSupplier.ContextMenuStrip = this.contextMenuStrip1;
      this.dgvLsSupplier.Dock = DockStyle.Fill;
      this.dgvLsSupplier.Location = new Point(0, 30);
      this.dgvLsSupplier.Margin = new Padding(4);
      this.dgvLsSupplier.Name = "dgvLsSupplier";
      this.dgvLsSupplier.Size = new Size(905, 158);
      this.dgvLsSupplier.TabIndex = 3;
      this.dgvLsSupplier.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvLsSupplier_CellBeginEdit);
      this.dgvLsSupplier.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvLsSupplier_CellFormatting);
      this.dgvLsSupplier.CellMouseDown += new DataGridViewCellMouseEventHandler(this.dgvLsSupplier_CellMouseDown);
      this.dgvLsSupplier.CellValueChanged += new DataGridViewCellEventHandler(this.dgvLsSupplier_CellValueChanged);
      this.dgvLsSupplier.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvLsSupplier_ColumnWidthChanged);
      this.dgvLsSupplier.CurrentCellDirtyStateChanged += new EventHandler(this.dgvLsSupplier_CurrentCellDirtyStateChanged);
      this.dgvLsSupplier.DataError += new DataGridViewDataErrorEventHandler(this.dgvLsSupplier_DataError);
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem,
        (ToolStripItem) this.удалитьЗаписьИзВыбранныхОбъектовToolStripMenuItem,
        (ToolStripItem) this.обновитьЗаписьВВыбранныхОбъектахToolStripMenuItem,
        (ToolStripItem) this.miInPatTime
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new Size(331, 92);
      this.contextMenuStrip1.Opening += new CancelEventHandler(this.contextMenuStrip1_Opening);
      this.скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem.Image = (Image) Resources.add_var;
      this.скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem.Name = "скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem";
      this.скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem.Size = new Size(330, 22);
      this.скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem.Text = "Скопировать запись в выбранные объекты";
      this.скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem.Click += new EventHandler(this.скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem_Click);
      this.удалитьЗаписьИзВыбранныхОбъектовToolStripMenuItem.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.удалитьЗаписьИзВыбранныхОбъектовToolStripMenuItem.Image = (Image) Resources.minus;
      this.удалитьЗаписьИзВыбранныхОбъектовToolStripMenuItem.Name = "удалитьЗаписьИзВыбранныхОбъектовToolStripMenuItem";
      this.удалитьЗаписьИзВыбранныхОбъектовToolStripMenuItem.Size = new Size(330, 22);
      this.удалитьЗаписьИзВыбранныхОбъектовToolStripMenuItem.Text = "Удалить запись из выбранных объектов";
      this.удалитьЗаписьИзВыбранныхОбъектовToolStripMenuItem.Click += new EventHandler(this.скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem_Click);
      this.обновитьЗаписьВВыбранныхОбъектахToolStripMenuItem.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.обновитьЗаписьВВыбранныхОбъектахToolStripMenuItem.Image = (Image) Resources.redo;
      this.обновитьЗаписьВВыбранныхОбъектахToolStripMenuItem.Name = "обновитьЗаписьВВыбранныхОбъектахToolStripMenuItem";
      this.обновитьЗаписьВВыбранныхОбъектахToolStripMenuItem.Size = new Size(330, 22);
      this.обновитьЗаписьВВыбранныхОбъектахToolStripMenuItem.Text = "Обновить запись в выбранных объектах";
      this.обновитьЗаписьВВыбранныхОбъектахToolStripMenuItem.Click += new EventHandler(this.скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem_Click);
      this.miInPatTime.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.miInPatTime.Name = "miInPatTime";
      this.miInPatTime.Size = new Size(330, 22);
      this.miInPatTime.Text = "Скопировать в прошлое время";
      this.miInPatTime.Click += new EventHandler(this.скопироватьВПрошлоеВремяToolStripMenuItem_Click);
      this.tmr1.Interval = 1000;
      this.tmr1.Tick += new EventHandler(this.tmr1_Tick);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.dgvLsSupplier);
      this.Controls.Add((Control) this.pnUp);
      this.Controls.Add((Control) this.panel1);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.Margin = new Padding(4);
      this.Name = "UCLsSupplier";
      this.Size = new Size(905, 226);
      this.panel1.ResumeLayout(false);
      this.pnUp.ResumeLayout(false);
      this.pnUp.PerformLayout();
      ((ISupportInitialize) this.dgvLsSupplier).EndInit();
      this.contextMenuStrip1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
