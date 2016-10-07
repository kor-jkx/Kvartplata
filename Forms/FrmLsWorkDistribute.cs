// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmLsWorkDistribute
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using NHibernate;
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
  public class FrmLsWorkDistribute : Form
  {
    private FormStateSaver fss = new FormStateSaver(FrmLsWorkDistribute.ic);
    protected GridSettings MySettingsLsWDGrid = new GridSettings();
    private bool listInsert = false;
    private bool changes = false;
    private IList<BaseOrg> recipientsList = (IList<BaseOrg>) new List<BaseOrg>();
    private IList<BaseOrg> perfomersList = (IList<BaseOrg>) new List<BaseOrg>();
    private IContainer components = (IContainer) null;
    private hmWorkDistribute workDistribute;
    private Period period;
    private ISession session;
    private Company company;
    private Home home;
    private bool insertRecord;
    private Period monthClosed;
    private Period NextMonthClosed;
    private static IContainer ic;
    private DataGridView dgvLsWorkDistribute;
    private Panel pnBottom;
    private Button bntExit;
    private Button btnSave;
    private Button btnDelete;
    private Button btnAdd;
    private Button btnClientListAdd;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
    private Button btnClientListDel;

    public IList<LsClient> LsClientsList { get; set; }

    public FrmLsWorkDistribute()
    {
      this.InitializeComponent();
      this.period = Options.Period;
      this.fss.ParentForm = (Form) this;
    }

    public FrmLsWorkDistribute(hmWorkDistribute workDistribute, Company company, Home home)
    {
      this.InitializeComponent();
      this.workDistribute = workDistribute;
      this.period = Options.Period;
      this.company = company;
      this.home = home;
      this.fss.ParentForm = (Form) this;
    }

    private void FrmLsWorkDistribute_Load(object sender, EventArgs e)
    {
      this.monthClosed = KvrplHelper.GetCmpKvrClose(this.company, Options.ComplexPasp.ComplexId, Options.ComplexPrior.IdFk);
      this.NextMonthClosed = KvrplHelper.GetNextPeriod(this.monthClosed);
      this.MySettingsLsWDGrid.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
      this.LoadSettingsLsWDGrid();
      this.LoadLsWorkDistribute();
      this.SetViewLsWorkDistribute();
    }

    private void FrmLsWorkDistribute_Shown(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(78, 2, this.company, false))
      {
        this.dgvLsWorkDistribute.ReadOnly = true;
        this.btnSave.Enabled = false;
        this.btnAdd.Enabled = false;
        this.btnDelete.Enabled = false;
        this.btnClientListAdd.Enabled = false;
      }
      this.LoadSettingsLsWDGrid();
    }

    private void bntExit_Click(object sender, EventArgs e)
    {
      if (!this.changes)
        this.Close();
      else if (MessageBox.Show("Есть несохраненные изменения! Сохранить изменения?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
      {
        for (int index = 0; index < this.dgvLsWorkDistribute.Rows.Count; ++index)
        {
          this.dgvLsWorkDistribute.CurrentCell = this.dgvLsWorkDistribute.Rows[index].Cells[1];
          this.SaveLsWorkDistribute();
        }
        this.changes = false;
        this.LoadLsWorkDistribute();
        this.SetViewLsWorkDistribute();
      }
      else
        this.Close();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      this.insertRecord = true;
      lsWorkDistribute lsWorkDistribute = new lsWorkDistribute();
      lsWorkDistribute.WorkDistribute = this.workDistribute;
      lsWorkDistribute.Period = Options.Period;
      lsWorkDistribute.Scheme = 1;
      lsWorkDistribute.MonthCnt = 1;
      IList<lsWorkDistribute> lsWorkDistributeList = (IList<lsWorkDistribute>) new List<lsWorkDistribute>();
      if ((uint) this.dgvLsWorkDistribute.Rows.Count > 0U)
        lsWorkDistributeList = (IList<lsWorkDistribute>) (this.dgvLsWorkDistribute.DataSource as List<lsWorkDistribute>);
      lsWorkDistributeList.Add(lsWorkDistribute);
      this.dgvLsWorkDistribute.Columns.Clear();
      this.dgvLsWorkDistribute.DataSource = (object) null;
      this.dgvLsWorkDistribute.DataSource = (object) lsWorkDistributeList;
      this.SetViewLsWorkDistribute();
      this.dgvLsWorkDistribute.CurrentCell = this.dgvLsWorkDistribute.Rows[this.dgvLsWorkDistribute.Rows.Count - 1].Cells[2];
      this.changes = true;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.listInsert)
      {
        for (int index = 0; index < this.dgvLsWorkDistribute.Rows.Count; ++index)
        {
          this.insertRecord = true;
          this.dgvLsWorkDistribute.CurrentCell = this.dgvLsWorkDistribute.Rows[index].Cells[1];
          this.SaveLsWorkDistribute();
        }
        this.listInsert = false;
      }
      else
        this.SaveLsWorkDistribute();
      this.LoadLsWorkDistribute();
      this.SetViewLsWorkDistribute();
      this.btnAdd.Enabled = true;
      this.btnDelete.Enabled = true;
      this.changes = false;
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.dgvLsWorkDistribute.Rows.Count <= 0 || this.dgvLsWorkDistribute.CurrentRow == null || MessageBox.Show("Удалить запись?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      this.session = Domain.CurrentSession;
      lsWorkDistribute dataBoundItem = (lsWorkDistribute) this.dgvLsWorkDistribute.CurrentRow.DataBoundItem;
      if (dataBoundItem.Period.PeriodId <= this.monthClosed.PeriodId)
      {
        int num1 = (int) MessageBox.Show("Невозможно удалить запись из закрытого месяца", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        try
        {
          this.session.Delete((object) dataBoundItem);
          this.session.Flush();
          this.session.Clear();
          this.LoadLsWorkDistribute();
          this.SetViewLsWorkDistribute();
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show("Невозможно удалить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
    }

    private void btnClientListAdd_Click(object sender, EventArgs e)
    {
      FrmChooseObject frmChooseObject = new FrmChooseObject(this.workDistribute, this.recipientsList, this.perfomersList);
      int num1 = (int) frmChooseObject.ShowDialog();
      this.LsClientsList = frmChooseObject.LsClientList;
      Decimal rate = frmChooseObject.Rate;
      Decimal comission = frmChooseObject.Comission;
      int monthCount = frmChooseObject.MonthCount;
      int schemeId = frmChooseObject.SchemeId;
      int performerId = frmChooseObject.performerId;
      int recipientId = frmChooseObject.recipientId;
      frmChooseObject.Dispose();
      IList<lsWorkDistribute> lsWorkDistributeList = (IList<lsWorkDistribute>) new List<lsWorkDistribute>();
      if ((uint) this.dgvLsWorkDistribute.Rows.Count > 0U)
        lsWorkDistributeList = (IList<lsWorkDistribute>) (this.dgvLsWorkDistribute.DataSource as List<lsWorkDistribute>);
      if (this.LsClientsList != null)
      {
        foreach (LsClient lsClients in (IEnumerable<LsClient>) this.LsClientsList)
        {
          List<int> intList = new List<int>() { 2, 5, 6, 7, 8, 12, 14, 15, 17, 18, 21, 25, 28, 29, 31, 33, 35, 38, 39 };
          IList<ClientParam> source = this.session.CreateQuery("select cp from ClientParam cp left join fetch cp.Param p where p.ParamId=:pi and cp.ClientId=:clientId and cp.Period.PeriodId=0 and cp.DBeg<=:per and cp.DEnd>:per").SetParameter<short>("pi", Convert.ToInt16(104)).SetParameter<int>("clientId", lsClients.ClientId).SetParameter<DateTime?>("per", Options.Period.PeriodName).List<ClientParam>();
          if ((uint) source.Count > 0U)
          {
            RightDoc rightDoc = this.session.Get<RightDoc>((object) Convert.ToInt16(source.First<ClientParam>().ParamValue));
            lsWorkDistribute lsWorkDistribute = new lsWorkDistribute();
            lsWorkDistribute.WorkDistribute = this.workDistribute;
            lsWorkDistribute.RightDocs = rightDoc;
            lsWorkDistribute.Period = Options.Period;
            lsWorkDistribute.Scheme = schemeId;
            lsWorkDistribute.Client = lsClients;
            int num2 = !intList.Contains((int) rightDoc.RightDocId) ? 1 : (lsWorkDistribute.Client.Complex.ComplexId == 110 ? 1 : 0);
            lsWorkDistribute.MonthCnt = num2 == 0 ? monthCount : 1;
            lsWorkDistribute.Rate = rate;
            lsWorkDistribute.Comission = comission;
            ISession session = this.session;
            string format = "select distinct d.Perfomer from CmpSupplier s, Supplier d where s.SupplierOrg.SupplierId = d.SupplierId and s.Service.ServiceId={0} and d.Recipient.BaseOrgId=0 and d.Perfomer.BaseOrgId<>0  and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={1} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{3}' and Param.ParamId=211)";
            object[] objArray = new object[4]{ (object) this.workDistribute.Service.ServiceId, (object) this.workDistribute.Company.CompanyId, null, null };
            int index1 = 2;
            DateTime? periodName = this.NextMonthClosed.PeriodName;
            string baseFormat1 = KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(periodName.Value));
            objArray[index1] = (object) baseFormat1;
            int index2 = 3;
            periodName = this.NextMonthClosed.PeriodName;
            string baseFormat2 = KvrplHelper.DateToBaseFormat(periodName.Value);
            objArray[index2] = (object) baseFormat2;
            string queryString = string.Format(format, objArray);
            session.CreateQuery(queryString).List<BaseOrg>();
            lsWorkDistribute.Supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) recipientId, (object) performerId)).List<Supplier>()[0];
            lsWorkDistributeList.Add(lsWorkDistribute);
          }
        }
        this.dgvLsWorkDistribute.Columns.Clear();
        this.dgvLsWorkDistribute.DataSource = (object) null;
        this.dgvLsWorkDistribute.DataSource = (object) lsWorkDistributeList;
        this.SetViewLsWorkDistribute();
      }
      this.listInsert = true;
      this.changes = true;
      this.btnSave.Enabled = true;
    }

    private void btnClientListDel_Click(object sender, EventArgs e)
    {
      FrmChooseObject frmChooseObject = new FrmChooseObject(this.workDistribute);
      int num1 = (int) frmChooseObject.ShowDialog();
      this.LsClientsList = frmChooseObject.LsClientList;
      this.session = Domain.CurrentSession;
      try
      {
        string str = string.Format("delete from lsWorkDistribute where WorkDistribute_id={0} and period_id={1} and client_id in (", (object) this.workDistribute.WorkDistribute, (object) Options.Period.PeriodId);
        for (int index = 0; index < this.LsClientsList.Count - 1; ++index)
          str = str + this.LsClientsList[index].ClientId.ToString() + ",";
        this.session.CreateQuery(str + (object) this.LsClientsList[this.LsClientsList.Count - 1].ClientId + ")").ExecuteUpdate();
        this.session.Clear();
        this.LoadLsWorkDistribute();
        this.SetViewLsWorkDistribute();
      }
      catch (Exception ex)
      {
        int num2 = (int) MessageBox.Show("Невозможно удалить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void LoadLsWorkDistribute()
    {
      this.session = Domain.CurrentSession;
      IList<lsWorkDistribute> lsWorkDistributeList = this.session.CreateQuery("select w from lsWorkDistribute w where w.WorkDistribute=:workD and w.Period=:period").SetParameter<hmWorkDistribute>("workD", this.workDistribute).SetParameter<Period>("period", this.period).List<lsWorkDistribute>();
      this.dgvLsWorkDistribute.Columns.Clear();
      this.dgvLsWorkDistribute.DataSource = (object) null;
      this.dgvLsWorkDistribute.DataSource = (object) lsWorkDistributeList;
    }

    private void SetViewLsWorkDistribute()
    {
      this.session = Domain.CurrentSession;
      try
      {
        this.recipientsList = this.session.CreateQuery(string.Format("select distinct d.Recipient from CmpSupplier s, Supplier d where s.SupplierOrg.SupplierId = d.SupplierId and s.Service.ServiceId={0}  and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={1} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{3}' and Param.ParamId=211) order by d.Recipient.NameOrgMin", (object) this.workDistribute.Service.ServiceId, (object) this.workDistribute.Company.CompanyId, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(this.NextMonthClosed.PeriodName.Value)), (object) KvrplHelper.DateToBaseFormat(this.NextMonthClosed.PeriodName.Value))).List<BaseOrg>();
        this.recipientsList.Insert(0, new BaseOrg(0, ""));
        this.perfomersList = this.session.CreateQuery(string.Format("select distinct d.Perfomer from CmpSupplier s, Supplier d where s.SupplierOrg.SupplierId = d.SupplierId and s.Service.ServiceId={0}  and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={1} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{3}' and Param.ParamId=211) order by 1", (object) this.workDistribute.Service.ServiceId, (object) this.workDistribute.Company.CompanyId, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(this.NextMonthClosed.PeriodName.Value)), (object) KvrplHelper.DateToBaseFormat(this.NextMonthClosed.PeriodName.Value))).List<BaseOrg>();
      }
      catch
      {
      }
      Service service = this.session.Get<Service>((object) this.workDistribute.Service.ServiceId);
      KvrplHelper.AddMaskDateColumn(this.dgvLsWorkDistribute, 0, "Период", "Periods");
      KvrplHelper.AddTextBoxColumn(this.dgvLsWorkDistribute, 1, "Услуга", "Service", 180, true);
      KvrplHelper.AddComboBoxColumn(this.dgvLsWorkDistribute, 2, (IList) null, "ClientId", "ClientId", "Лицевой", "ClientId", 180, 180);
      KvrplHelper.AddComboBoxColumn(this.dgvLsWorkDistribute, 3, (IList) this.recipientsList, "BaseOrgId", "NameOrgMin", "Получатель", "Recipient", 7, 100);
      KvrplHelper.AddComboBoxColumn(this.dgvLsWorkDistribute, 4, (IList) null, (string) null, (string) null, "Исполнитель", "Perfomer", 160, 120);
      KvrplHelper.AddComboBoxColumn(this.dgvLsWorkDistribute, 5, (IList) null, "RightDocId", "RightDocName", "Документ", "RightDocss", 180, 180);
      this.dgvLsWorkDistribute.Columns["Periods"].DisplayIndex = 0;
      this.dgvLsWorkDistribute.Columns["Service"].DisplayIndex = 1;
      this.dgvLsWorkDistribute.Columns["ClientId"].DisplayIndex = 2;
      this.dgvLsWorkDistribute.Columns["Recipient"].DisplayIndex = 3;
      this.dgvLsWorkDistribute.Columns["Perfomer"].DisplayIndex = 4;
      this.dgvLsWorkDistribute.Columns["RightDocss"].DisplayIndex = 5;
      this.dgvLsWorkDistribute.Columns["Rent"].DisplayIndex = 6;
      this.dgvLsWorkDistribute.Columns["ParamValue"].DisplayIndex = 7;
      this.dgvLsWorkDistribute.Columns["Scheme"].DisplayIndex = 8;
      this.dgvLsWorkDistribute.Columns["MonthCnt"].DisplayIndex = 9;
      this.dgvLsWorkDistribute.Columns["Rate"].DisplayIndex = 10;
      this.dgvLsWorkDistribute.Columns["Comission"].DisplayIndex = 11;
      this.dgvLsWorkDistribute.Columns["RentCorrect"].DisplayIndex = 12;
      this.dgvLsWorkDistribute.Columns["RentPercentCorrect"].DisplayIndex = 13;
      this.dgvLsWorkDistribute.Columns["Uname"].DisplayIndex = 14;
      this.dgvLsWorkDistribute.Columns["Dedit"].DisplayIndex = 15;
      this.dgvLsWorkDistribute.Columns["WorkDistribute"].HeaderText = "Код записи";
      this.dgvLsWorkDistribute.Columns["WorkDistribute"].Visible = false;
      this.dgvLsWorkDistribute.Columns["RightDocss"].ReadOnly = true;
      this.dgvLsWorkDistribute.Columns["Client"].Visible = false;
      this.dgvLsWorkDistribute.Columns["Period"].Visible = false;
      this.dgvLsWorkDistribute.Columns["Supplier"].Visible = false;
      this.dgvLsWorkDistribute.Columns["RightDocs"].Visible = false;
      this.dgvLsWorkDistribute.Columns["Rent"].HeaderText = "Сумма";
      this.dgvLsWorkDistribute.Columns["Rent"].ReadOnly = true;
      this.dgvLsWorkDistribute.Columns["ParamValue"].HeaderText = "Параметр";
      this.dgvLsWorkDistribute.Columns["Scheme"].HeaderText = "Схема";
      this.dgvLsWorkDistribute.Columns["Scheme"].ReadOnly = true;
      this.dgvLsWorkDistribute.Columns["Periods"].ReadOnly = true;
      this.dgvLsWorkDistribute.Columns["MonthCnt"].HeaderText = "Кол-во месяцев";
      this.dgvLsWorkDistribute.Columns["Rate"].HeaderText = "Процент";
      this.dgvLsWorkDistribute.Columns["Comission"].HeaderText = "Комиссия";
      this.dgvLsWorkDistribute.Columns["ParamValue"].ReadOnly = true;
      this.dgvLsWorkDistribute.Columns["RentCorrect"].HeaderText = "Корректировка начислений";
      this.dgvLsWorkDistribute.Columns["RentPercentCorrect"].HeaderText = "Корректировка начислений процентов";
      KvrplHelper.ViewEdit(this.dgvLsWorkDistribute);
      foreach (DataGridViewRow row in (IEnumerable) this.dgvLsWorkDistribute.Rows)
      {
        DataGridViewComboBoxCell viewComboBoxCell1 = new DataGridViewComboBoxCell();
        viewComboBoxCell1.DisplayStyleForCurrentCellOnly = true;
        viewComboBoxCell1.ValueMember = "ClientId";
        viewComboBoxCell1.DisplayMember = "ClientId";
        IList<LsClient> lsClientList1 = (IList<LsClient>) new List<LsClient>();
        if (((lsWorkDistribute) row.DataBoundItem).Client != null)
        {
          IList<LsClient> lsClientList2 = this.session.CreateQuery(string.Format("select ls from LsClient ls where ls.ClientId=:client and ls.Company=:comp")).SetParameter<int>("client", ((lsWorkDistribute) row.DataBoundItem).Client.ClientId).SetParameter<Company>("comp", this.company).List<LsClient>();
          viewComboBoxCell1.DataSource = (object) lsClientList2;
          row.Cells["ClientId"] = (DataGridViewCell) viewComboBoxCell1;
          row.Cells["ClientId"].Value = (object) ((lsWorkDistribute) row.DataBoundItem).Client.ClientId;
        }
        else
        {
          IList<LsClient> lsClientList2 = this.session.CreateQuery(string.Format("select ls from LsClient ls where ls.Company=:comp  and ls.Home=:hom order by ClientId")).SetParameter<Company>("comp", this.company).SetParameter<Home>("hom", this.home).List<LsClient>();
          viewComboBoxCell1.DataSource = (object) lsClientList2;
          row.Cells["ClientId"] = (DataGridViewCell) viewComboBoxCell1;
        }
        DataGridViewComboBoxCell viewComboBoxCell2 = new DataGridViewComboBoxCell();
        viewComboBoxCell2.DisplayStyleForCurrentCellOnly = true;
        viewComboBoxCell2.ValueMember = "RightDocId";
        viewComboBoxCell2.DisplayMember = "RightDocName";
        IList<RightDoc> rightDocList1 = (IList<RightDoc>) new List<RightDoc>();
        IList<RightDoc> rightDocList2 = this.session.CreateQuery(string.Format("from RightDoc")).List<RightDoc>();
        viewComboBoxCell2.DataSource = (object) rightDocList2;
        row.Cells["RightDocss"] = (DataGridViewCell) viewComboBoxCell2;
        if (((lsWorkDistribute) row.DataBoundItem).RightDocs != null)
          row.Cells["RightDocss"].Value = (object) ((lsWorkDistribute) row.DataBoundItem).RightDocs.RightDocId;
        if (((lsWorkDistribute) row.DataBoundItem).Supplier != null)
        {
          row.Cells["Recipient"].Value = (object) ((lsWorkDistribute) row.DataBoundItem).Supplier.Recipient.BaseOrgId;
          this.perfomersList = this.session.CreateQuery(string.Format("select distinct d.Perfomer from CmpSupplier s, Supplier d where s.SupplierOrg.SupplierId = d.SupplierId and s.Service.ServiceId={0} and d.Recipient.BaseOrgId={4} and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={1} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{3}' and Param.ParamId=211)", (object) this.workDistribute.Service.ServiceId, (object) this.workDistribute.Company.CompanyId, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(this.NextMonthClosed.PeriodName.Value)), (object) KvrplHelper.DateToBaseFormat(this.NextMonthClosed.PeriodName.Value), (object) ((lsWorkDistribute) row.DataBoundItem).Supplier.Recipient.BaseOrgId)).List<BaseOrg>();
          if (((lsWorkDistribute) row.DataBoundItem).Supplier.Recipient.BaseOrgId == 0)
          {
            this.perfomersList.Insert(0, new BaseOrg(0, ""));
          }
          else
          {
            IList<BaseOrg> baseOrgList = this.session.CreateQuery(string.Format("select distinct d.Perfomer from CmpSupplier s, Supplier d where s.SupplierOrg.SupplierId = d.SupplierId and s.Service.ServiceId={0} and d.Recipient.BaseOrgId={4} and d.Perfomer.BaseOrgId=0  and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={1} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{3}' and Param.ParamId=211)", (object) this.workDistribute.Service.ServiceId, (object) this.workDistribute.Company.CompanyId, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(this.NextMonthClosed.PeriodName.Value)), (object) KvrplHelper.DateToBaseFormat(this.NextMonthClosed.PeriodName.Value), (object) ((lsWorkDistribute) row.DataBoundItem).Supplier.Recipient.BaseOrgId)).List<BaseOrg>();
            if (baseOrgList.Count > 0)
              this.perfomersList.Insert(0, baseOrgList[0]);
          }
          row.Cells["Perfomer"] = (DataGridViewCell) new DataGridViewComboBoxCell()
          {
            DisplayStyleForCurrentCellOnly = true,
            ValueMember = "BaseOrgId",
            DisplayMember = "NameOrgMinDop",
            DataSource = (object) this.perfomersList
          };
          row.Cells["Perfomer"].Value = (object) ((lsWorkDistribute) row.DataBoundItem).Supplier.Perfomer.BaseOrgId;
        }
        else
        {
          IList<BaseOrg> baseOrgList = this.session.CreateQuery(string.Format("select distinct d.Perfomer from CmpSupplier s, Supplier d where s.SupplierOrg.SupplierId = d.SupplierId and s.Service.ServiceId={0} and d.Recipient.BaseOrgId=0 and d.Perfomer.BaseOrgId<>0  and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={1} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{3}' and Param.ParamId=211)", (object) this.workDistribute.Service.ServiceId, (object) this.workDistribute.Company.CompanyId, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(this.NextMonthClosed.PeriodName.Value)), (object) KvrplHelper.DateToBaseFormat(this.NextMonthClosed.PeriodName.Value))).List<BaseOrg>();
          baseOrgList.Insert(0, new BaseOrg(0, ""));
          row.Cells["Perfomer"] = (DataGridViewCell) new DataGridViewComboBoxCell()
          {
            DisplayStyleForCurrentCellOnly = true,
            ValueMember = "BaseOrgId",
            DisplayMember = "NameOrgMinDop",
            DataSource = (object) baseOrgList
          };
        }
        int scheme = ((lsWorkDistribute) row.DataBoundItem).Scheme;
        if (true)
          row.Cells["Scheme"].Value = (object) ((lsWorkDistribute) row.DataBoundItem).Scheme;
        row.Cells["Service"].Value = (object) service.ServiceName;
        row.Cells["Periods"].Value = (object) ((lsWorkDistribute) row.DataBoundItem).Period.PeriodName.Value;
        this.dgvLsWorkDistribute.Columns.Add("Flat", "Квартира");
        this.dgvLsWorkDistribute.Columns["Flat"].DisplayIndex = 1;
        LsClient client = ((lsWorkDistribute) row.DataBoundItem).Client;
        if (client != null)
        {
          IList list = this.session.CreateSQLQuery(string.Format("select distinct f.NFLAT  from lsClient cl  inner join FLATS f on f.IDFLAT=cl.IdFlat  where cl.Client_Id={0} ", (object) client.ClientId)).List();
          string str = "";
          foreach (object obj in (IEnumerable) list)
            str = str + " " + obj.ToString();
          row.Cells["Flat"].Value = (object) str;
        }
      }
      this.MySettingsLsWDGrid.GridName = "LsWDGrid";
      this.changes = false;
      this.btnSave.Enabled = false;
    }

    private void SaveLsWorkDistribute()
    {
      if (this.dgvLsWorkDistribute.Rows.Count > 0 && this.dgvLsWorkDistribute.CurrentRow != null)
      {
        lsWorkDistribute dataBoundItem = (lsWorkDistribute) this.dgvLsWorkDistribute.CurrentRow.DataBoundItem;
        if (dataBoundItem.Period.PeriodId <= this.monthClosed.PeriodId)
        {
          int num = (int) MessageBox.Show("Невозможно внести изменения в закрытом периоде", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        if (this.dgvLsWorkDistribute.CurrentRow.Cells["ClientId"].Value != null)
        {
          dataBoundItem.Client = this.session.Get<LsClient>(this.dgvLsWorkDistribute.CurrentRow.Cells["ClientId"].Value);
          if (this.dgvLsWorkDistribute.CurrentRow.Cells["Recipient"].Value != null && Convert.ToInt32(this.dgvLsWorkDistribute.CurrentRow.Cells["Recipient"].Value) != 0 || this.dgvLsWorkDistribute.CurrentRow.Cells["Perfomer"].Value != null && (uint) Convert.ToInt32(this.dgvLsWorkDistribute.CurrentRow.Cells["Perfomer"].Value) > 0U)
          {
            dataBoundItem.Supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(this.dgvLsWorkDistribute.CurrentRow.Cells["Recipient"].Value), (object) Convert.ToInt32(this.dgvLsWorkDistribute.CurrentRow.Cells["Perfomer"].Value))).List<Supplier>()[0];
            if (this.dgvLsWorkDistribute.CurrentRow.Cells["Rent"].Value != null)
            {
              try
              {
                dataBoundItem.Rent = Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.dgvLsWorkDistribute.CurrentRow.Cells["Rent"].Value.ToString()));
              }
              catch (Exception ex)
              {
                int num = (int) MessageBox.Show("Некорректный формат суммы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
              }
              if (this.dgvLsWorkDistribute.CurrentRow.Cells["MonthCnt"].Value != null)
              {
                dataBoundItem.MonthCnt = (int) this.dgvLsWorkDistribute.CurrentRow.Cells["MonthCnt"].Value;
                if (this.dgvLsWorkDistribute.CurrentRow.Cells["Rate"].Value != null)
                {
                  try
                  {
                    dataBoundItem.Rate = Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.dgvLsWorkDistribute.CurrentRow.Cells["Rate"].Value.ToString()));
                  }
                  catch (Exception ex)
                  {
                    int num = (int) MessageBox.Show("Некорректный формат процента", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                  }
                  dataBoundItem.Period = Options.Period;
                  dataBoundItem.Scheme = (int) Convert.ToInt16(this.dgvLsWorkDistribute.CurrentRow.Cells["Scheme"].Value);
                  if (this.dgvLsWorkDistribute.CurrentRow.Cells["RightDocss"].Value != null)
                  {
                    dataBoundItem.RightDocs = this.session.Get<RightDoc>(this.dgvLsWorkDistribute.CurrentRow.Cells["RightDocss"].Value);
                    if (this.session.CreateQuery(string.Format("from lsWorkDistribute where WorkDistribute.WorkDistribute={0} and Period.PeriodId={1} and Client.ClientId={2} and Supplier.SupplierId={3}", (object) dataBoundItem.WorkDistribute.WorkDistribute, (object) dataBoundItem.Period.PeriodId, (object) dataBoundItem.Client.ClientId, (object) dataBoundItem.Supplier.SupplierId)).List<lsWorkDistribute>().Count > 0)
                      this.insertRecord = false;
                    dataBoundItem.Uname = Options.Login;
                    dataBoundItem.Dedit = new DateTime?(DateTime.Now.Date);
                    try
                    {
                      if (this.insertRecord)
                      {
                        this.insertRecord = false;
                        this.session.Save((object) dataBoundItem);
                      }
                      else
                        this.session.Update((object) dataBoundItem);
                      this.session.Flush();
                    }
                    catch (Exception ex)
                    {
                      int num = (int) MessageBox.Show("Невозможно сохранить изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                      KvrplHelper.WriteLog(ex, (LsClient) null);
                    }
                  }
                  else
                  {
                    int num = (int) MessageBox.Show("Введите документ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                  }
                }
                else
                {
                  int num = (int) MessageBox.Show("Введите процент", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  return;
                }
              }
              else
              {
                int num = (int) MessageBox.Show("Введите кол-во месяцев", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
              }
            }
            else
            {
              int num = (int) MessageBox.Show("Введите сумму", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
          }
          else
          {
            int num = (int) MessageBox.Show("Выберите поставщика", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        else
        {
          int num = (int) MessageBox.Show("Выберите лицевой счет", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
      }
      this.session.Clear();
    }

    private void dgvLsWorkDistribute_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex <= 0 || e.RowIndex < 0 || !(this.dgvLsWorkDistribute.Columns[e.ColumnIndex].Name == "Scheme"))
        return;
      int num = ((lsWorkDistribute) this.dgvLsWorkDistribute.CurrentRow.DataBoundItem).Scheme;
      FrmScheme frmScheme = new FrmScheme((short) 13, (short) num);
      if (frmScheme.ShowDialog() == DialogResult.OK)
        num = (int) frmScheme.CurrentId();
      if (KvrplHelper.CheckProxy(78, 2, this.company, false))
        this.dgvLsWorkDistribute.CurrentRow.Cells["Scheme"].Value = (object) num;
      frmScheme.Dispose();
    }

    private void dgvLsWorkDistribute_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      this.changes = true;
      this.btnSave.Enabled = true;
      if (!(this.dgvLsWorkDistribute.Columns[e.ColumnIndex].Name == "ClientId"))
        return;
      RightDoc rightDoc = this.session.Get<RightDoc>((object) Convert.ToInt16(this.session.CreateQuery("select cp from ClientParam cp left join fetch cp.Param p where p.ParamId=:pi and cp.ClientId=:clientId and cp.Period.PeriodId=0 and cp.DBeg<=:per and cp.DEnd>:per").SetParameter<short>("pi", Convert.ToInt16(104)).SetParameter<int>("clientId", this.session.Get<LsClient>(this.dgvLsWorkDistribute[e.ColumnIndex, e.RowIndex].Value).ClientId).SetParameter<DateTime?>("per", Options.Period.PeriodName).List<ClientParam>().First<ClientParam>().ParamValue));
      this.dgvLsWorkDistribute.Rows[e.RowIndex].Cells["RightDocss"].Value = (object) rightDoc.RightDocId;
    }

    public void LoadSettingsLsWDGrid()
    {
      this.MySettingsLsWDGrid.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvLsWorkDistribute.Columns)
        this.MySettingsLsWDGrid.GetMySettings(column);
    }

    private void dgvLsWorkDistribute_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsLsWDGrid.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsLsWDGrid.Columns[this.MySettingsLsWDGrid.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsLsWDGrid.Save();
    }

    private void dgvLsWorkDistribute_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
    {
      if (!(((DataGridView) sender).Columns[((DataGridView) sender).CurrentCell.ColumnIndex].Name == "Rate"))
        return;
      e.Control.KeyPress += new KeyPressEventHandler(this.CheckDataGrid_KeyPress);
    }

    private void CheckDataGrid_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || (int) e.KeyChar == 44 || (int) e.KeyChar == 8)
        return;
      e.Handled = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmLsWorkDistribute));
      this.dgvLsWorkDistribute = new DataGridView();
      this.pnBottom = new Panel();
      this.btnClientListDel = new Button();
      this.btnClientListAdd = new Button();
      this.btnSave = new Button();
      this.bntExit = new Button();
      this.btnDelete = new Button();
      this.btnAdd = new Button();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
      ((ISupportInitialize) this.dgvLsWorkDistribute).BeginInit();
      this.pnBottom.SuspendLayout();
      this.SuspendLayout();
      this.dgvLsWorkDistribute.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dgvLsWorkDistribute.BackgroundColor = Color.AliceBlue;
      this.dgvLsWorkDistribute.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvLsWorkDistribute.Location = new Point(0, 0);
      this.dgvLsWorkDistribute.Margin = new Padding(3, 2, 3, 2);
      this.dgvLsWorkDistribute.Name = "dgvLsWorkDistribute";
      this.dgvLsWorkDistribute.Size = new Size(1257, 376);
      this.dgvLsWorkDistribute.TabIndex = 1;
      this.dgvLsWorkDistribute.CellClick += new DataGridViewCellEventHandler(this.dgvLsWorkDistribute_CellClick);
      this.dgvLsWorkDistribute.CellValueChanged += new DataGridViewCellEventHandler(this.dgvLsWorkDistribute_CellValueChanged);
      this.dgvLsWorkDistribute.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvLsWorkDistribute_ColumnWidthChanged);
      this.dgvLsWorkDistribute.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgvLsWorkDistribute_EditingControlShowing);
      this.pnBottom.Controls.Add((Control) this.btnClientListDel);
      this.pnBottom.Controls.Add((Control) this.btnClientListAdd);
      this.pnBottom.Controls.Add((Control) this.btnSave);
      this.pnBottom.Controls.Add((Control) this.bntExit);
      this.pnBottom.Controls.Add((Control) this.btnDelete);
      this.pnBottom.Controls.Add((Control) this.btnAdd);
      this.pnBottom.Dock = DockStyle.Bottom;
      this.pnBottom.Location = new Point(0, 375);
      this.pnBottom.Margin = new Padding(4);
      this.pnBottom.Name = "pnBottom";
      this.pnBottom.Size = new Size(1257, 45);
      this.pnBottom.TabIndex = 2;
      this.btnClientListDel.Location = new Point(528, 6);
      this.btnClientListDel.Name = "btnClientListDel";
      this.btnClientListDel.Size = new Size(146, 30);
      this.btnClientListDel.TabIndex = 8;
      this.btnClientListDel.Text = "Удаление списком";
      this.btnClientListDel.UseVisualStyleBackColor = true;
      this.btnClientListDel.Click += new EventHandler(this.btnClientListDel_Click);
      this.btnClientListAdd.Location = new Point(370, 6);
      this.btnClientListAdd.Name = "btnClientListAdd";
      this.btnClientListAdd.Size = new Size(152, 30);
      this.btnClientListAdd.TabIndex = 7;
      this.btnClientListAdd.Text = "Добавить списком";
      this.btnClientListAdd.UseVisualStyleBackColor = true;
      this.btnClientListAdd.Click += new EventHandler(this.btnClientListAdd_Click);
      this.btnSave.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.btnSave.Image = (Image) Resources.Tick;
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.Location = new Point(253, 6);
      this.btnSave.Margin = new Padding(4);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(110, 30);
      this.btnSave.TabIndex = 6;
      this.btnSave.Text = "Сохранить";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.bntExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.bntExit.DialogResult = DialogResult.Cancel;
      this.bntExit.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.bntExit.Image = (Image) Resources.Exit;
      this.bntExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.bntExit.Location = new Point(1164, 6);
      this.bntExit.Margin = new Padding(4);
      this.bntExit.Name = "bntExit";
      this.bntExit.Size = new Size(80, 30);
      this.bntExit.TabIndex = 1;
      this.bntExit.Text = "Выход";
      this.bntExit.TextAlign = ContentAlignment.MiddleRight;
      this.bntExit.UseVisualStyleBackColor = true;
      this.bntExit.Click += new EventHandler(this.bntExit_Click);
      this.btnDelete.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.btnDelete.Image = (Image) Resources.minus;
      this.btnDelete.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnDelete.Location = new Point(135, 6);
      this.btnDelete.Margin = new Padding(4);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(110, 30);
      this.btnDelete.TabIndex = 5;
      this.btnDelete.Text = "Удалить";
      this.btnDelete.TextAlign = ContentAlignment.MiddleRight;
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnAdd.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.btnAdd.Image = (Image) Resources.plus;
      this.btnAdd.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnAdd.Location = new Point(16, 6);
      this.btnAdd.Margin = new Padding(4);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(110, 30);
      this.btnAdd.TabIndex = 4;
      this.btnAdd.Text = "Добавить";
      this.btnAdd.TextAlign = ContentAlignment.MiddleRight;
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.dataGridViewTextBoxColumn1.DataPropertyName = "Rent";
      this.dataGridViewTextBoxColumn1.HeaderText = "Rent";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.Width = 120;
      this.dataGridViewTextBoxColumn2.DataPropertyName = "ParamValue";
      this.dataGridViewTextBoxColumn2.HeaderText = "RentBeg";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn2.ReadOnly = true;
      this.dataGridViewTextBoxColumn2.Width = 119;
      this.dataGridViewTextBoxColumn3.DataPropertyName = "ParamValue";
      this.dataGridViewTextBoxColumn3.HeaderText = "ParamValue";
      this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
      this.dataGridViewTextBoxColumn3.ReadOnly = true;
      this.dataGridViewTextBoxColumn3.Width = 120;
      this.dataGridViewTextBoxColumn4.DataPropertyName = "Scheme";
      this.dataGridViewTextBoxColumn4.HeaderText = "Scheme";
      this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
      this.dataGridViewTextBoxColumn4.ReadOnly = true;
      this.dataGridViewTextBoxColumn4.Width = 120;
      this.dataGridViewTextBoxColumn5.DataPropertyName = "MonthCnt";
      this.dataGridViewTextBoxColumn5.HeaderText = "Month";
      this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
      this.dataGridViewTextBoxColumn5.Width = 119;
      this.dataGridViewTextBoxColumn6.DataPropertyName = "Rate";
      this.dataGridViewTextBoxColumn6.HeaderText = "Rate";
      this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
      this.dataGridViewTextBoxColumn6.Width = 120;
      this.dataGridViewTextBoxColumn7.DataPropertyName = "Comission";
      this.dataGridViewTextBoxColumn7.HeaderText = "Comission";
      this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
      this.dataGridViewTextBoxColumn7.Width = 119;
      this.dataGridViewTextBoxColumn8.HeaderText = "Дата редактирования";
      this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
      this.dataGridViewTextBoxColumn8.Width = 120;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1257, 420);
      this.Controls.Add((Control) this.pnBottom);
      this.Controls.Add((Control) this.dgvLsWorkDistribute);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(5);
      this.Name = "FrmLsWorkDistribute";
      this.Text = "Распределение сумм по лс по оплате работ";
      this.Load += new EventHandler(this.FrmLsWorkDistribute_Load);
      this.Shown += new EventHandler(this.FrmLsWorkDistribute_Shown);
      ((ISupportInitialize) this.dgvLsWorkDistribute).EndInit();
      this.pnBottom.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
