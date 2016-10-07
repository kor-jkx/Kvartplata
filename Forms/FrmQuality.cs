// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmQuality
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
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmQuality : Form
  {
    private readonly FormStateSaver fss = new FormStateSaver(FrmQuality.ic);
    protected GridSettings MySettings = new GridSettings();
    private IList companyList = (IList) new ArrayList();
    private IList qualityList = (IList) new ArrayList();
    private IList serviceList = (IList) new ArrayList();
    private IList<Service> sostList = (IList<Service>) new List<Service>();
    private bool _readOnly = false;
    private bool _OnOff = false;
    private IContainer components = (IContainer) null;
    private static IContainer ic;
    private readonly ISession session;
    private Quality curAuality;
    private Quality newQuality;
    private BaseOrg perfomer;
    private BaseOrg recipient;
    private short service_id;
    private Company _company;
    private GroupBox groupBox1;
    private ToolStrip toolStrip1;
    private ToolStripButton tsbAdd;
    private ToolStripButton tsbApplay;
    private ToolStripButton tsbDelete;
    private ComboBox cmbService;
    private ComboBox cmbCompany;
    private Label lblService;
    private Label lblCountry;
    private ToolStripButton tsbCancel;
    private Button btnExit;
    private DataSet dsMain;
    private DataTable dtSupplier;
    private DataColumn dataColumn1;
    private DataColumn dataColumn2;
    private Panel pnBtn;
    private DataGridView dgvQuality;
    public HelpProvider hp;
    private ComboBox cmbSost;
    private Label lblSost;
    private DataGridViewTextBoxColumn Quality_name;
    private DataGridViewTextBoxColumn DocNumber;
    private MaskDateColumn DocDate;
    private DataGridViewButtonColumn Legislation_id;
    private DataGridViewTextBoxColumn Coeff;
    private DataGridViewTextBoxColumn Quantity_hour;
    private DataGridViewTextBoxColumn Quantity_degree;
    private DataGridViewComboBoxColumn Supplier_id;
    private DataGridViewComboBoxColumn Perfom;
    private DataGridViewTextBoxColumn UName;
    private DataGridViewTextBoxColumn DEdit;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private MaskDateColumn maskDateColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripButton tsbOnOffSupplier;

    public short Company_id { get; set; }

    private short Service_id
    {
      get
      {
        return this.service_id;
      }
      set
      {
        this.service_id = value;
        IList list = this.session.CreateQuery(string.Format("select distinct s.Recipient from CmpSupplier cs, Supplier s where cs.SupplierOrg.SupplierId=s.SupplierId and s.Recipient.BaseOrgId<>0 and (cs.Service.ServiceId=:id or cs.Service.ServiceId=(select Root from Service where ServiceId=:id)) and cs.Service.ServiceId<>0 and cs.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId=:cmp and Period.PeriodId=0 and DBeg<='{0}' and DEnd>='{0}' and Param.ParamId=211)", (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value))).SetInt16("id", this.service_id).SetInt16("cmp", this.Company_id).List();
        list.Insert(0, (object) new BaseOrg(0, ""));
        this.dtSupplier.Rows.Clear();
        foreach (BaseOrg baseOrg in (IEnumerable) list)
          KvrplHelper.AddRow(this.dtSupplier, baseOrg.BaseOrgId, baseOrg.NameOrgMin);
        this.GetQuality();
      }
    }

    public FrmQuality(Company company)
    {
      this.InitializeComponent();
      this._company = company;
      this.CheckAccess();
      this.fss.ParentForm = (Form) this;
      this.Company_id = (short) 0;
      this.session = Domain.CurrentSession;
      if (!Options.ViewEdit)
      {
        this.dgvQuality.Columns["Uname"].Visible = false;
        this.dgvQuality.Columns["Dedit"].Visible = false;
      }
      this.MySettings.GridName = "Quality";
      this.SetGridConfigFileSettings();
      this.LoadSettings();
    }

    private void CheckAccess()
    {
      this._readOnly = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(43, this._company, false));
      this.toolStrip1.Visible = this._readOnly;
      this.dgvQuality.ReadOnly = !this._readOnly;
    }

    private void GetQuality()
    {
      if (this.session != null && this.session.IsOpen)
        this.session.Clear();
      this.qualityList.Clear();
      this.qualityList = this.session.CreateQuery(string.Format("from Quality where company_id={0} and service_id={1} order by DocDate desc", (object) this.Company_id, (object) this.Service_id)).List();
      foreach (Quality quality in (IEnumerable) this.qualityList)
      {
        if (quality.Supplier != null)
        {
          quality.Recipient = quality.Supplier.Recipient;
          quality.Perfomer = quality.Supplier.Perfomer;
        }
      }
      this.dgvQuality.RowCount = this.qualityList.Count;
      if (this.curAuality != null)
      {
        int curObject = KvrplHelper.FindCurObject(this.qualityList, (object) this.curAuality);
        if (curObject >= 0)
        {
          this.dgvQuality.CurrentCell = this.dgvQuality.Rows[curObject].Cells["Quality_name"];
          this.dgvQuality.Rows[curObject].Selected = true;
        }
      }
      else if (this.dgvQuality.CurrentRow != null && this.dgvQuality.CurrentRow.Index < this.qualityList.Count)
        this.curAuality = (Quality) this.qualityList[this.dgvQuality.CurrentRow.Index];
      this.LoadPerfomersList();
      this.dgvQuality.Refresh();
    }

    private void LoadPerfomersList()
    {
      foreach (DataGridViewRow row in (IEnumerable) this.dgvQuality.Rows)
      {
        int num = 0;
        if (((Quality) this.qualityList[row.Index]).Supplier != null)
          num = ((Quality) this.qualityList[row.Index]).Supplier.Recipient.BaseOrgId;
        ISession session1 = this.session;
        string format1 = "select distinct s.Perfomer from CmpSupplier cs, Supplier s where cs.SupplierOrg.SupplierId=s.SupplierId and s.Perfomer.BaseOrgId<>0 and s.Recipient.BaseOrgId={1} and (cs.Service.ServiceId=:id or cs.Service.ServiceId=(select Root from Service where ServiceId=:id)) and cs.Service.ServiceId<>0 and cs.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId=:cmp and Period.PeriodId=0 and DBeg<='{0}' and DEnd>='{0}' and Param.ParamId=211)";
        DateTime? periodName = Options.Period.PeriodName;
        string baseFormat1 = KvrplHelper.DateToBaseFormat(periodName.Value);
        // ISSUE: variable of a boxed type
        int local1 = num;
        string queryString1 = string.Format(format1, (object) baseFormat1, (object) local1);
        IList<BaseOrg> baseOrgList1 = session1.CreateQuery(queryString1).SetInt16("id", this.service_id).SetInt16("cmp", this.Company_id).List<BaseOrg>();
        if (num == 0)
        {
          baseOrgList1.Insert(0, new BaseOrg(0, ""));
        }
        else
        {
          ISession session2 = this.session;
          string format2 = "select distinct s.Perfomer from CmpSupplier cs, Supplier s where cs.SupplierOrg.SupplierId=s.SupplierId and s.Perfomer.BaseOrgId=0 and s.Recipient.BaseOrgId={1} and (cs.Service.ServiceId=:id or cs.Service.ServiceId=(select Root from Service where ServiceId=:id)) and cs.Service.ServiceId<>0 and cs.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId=:cmp and Period.PeriodId=0 and DBeg<='{0}' and DEnd>='{0}' and Param.ParamId=211)";
          periodName = Options.Period.PeriodName;
          string baseFormat2 = KvrplHelper.DateToBaseFormat(periodName.Value);
          // ISSUE: variable of a boxed type
          int local2 = num;
          string queryString2 = string.Format(format2, (object) baseFormat2, (object) local2);
          IList<BaseOrg> baseOrgList2 = session2.CreateQuery(queryString2).SetInt16("id", this.service_id).SetInt16("cmp", this.Company_id).List<BaseOrg>();
          if (baseOrgList2.Count > 0)
            baseOrgList1.Insert(0, baseOrgList2[0]);
        }
        row.Cells["Perfom"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "BaseOrgId",
          DisplayMember = "NameOrgMinDop",
          DataSource = (object) baseOrgList1
        };
        if (((Quality) this.qualityList[row.Index]).Supplier != null)
          row.Cells["Perfom"].Value = (object) ((Quality) this.qualityList[row.Index]).Supplier.Perfomer.BaseOrgId;
      }
    }

    private void LoadSettings()
    {
      this.MySettings.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvQuality.Columns)
        this.MySettings.GetMySettings(column);
    }

    private bool Proverka()
    {
      try
      {
        if (this.session.CreateCriteria(typeof (LsQuality)).Add((ICriterion) Restrictions.Eq("Quality", (object) this.curAuality)).Add((ICriterion) Restrictions.Le("Period.PeriodId", (object) KvrplHelper.GetCmpKvrClose(this.session.Get<Company>((object) this.Company_id), Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk).PeriodId)).List().Count > 0)
        {
          int num = (int) MessageBox.Show("В закрытых периодах на карточках есть записи по качеству привязанные к этой!", "Внимание!", MessageBoxButtons.OK);
          return false;
        }
      }
      catch
      {
      }
      return true;
    }

    public void SetGridConfigFileSettings()
    {
      this.MySettings.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
    }

    private void FrmQuality_Load(object sender, EventArgs e)
    {
      if (this.session != null && this.session.IsOpen)
        this.session.Clear();
      this.serviceList = this.session.CreateQuery("from Service s where s.Root=0 and s.ServiceId>0 order by " + Options.SortService).List();
      foreach (Service service in (IEnumerable) this.serviceList)
        this.cmbService.Items.Add((object) service.ServiceName);
      this.companyList = this.session.CreateQuery("from Company order by CompanyId").List();
      foreach (Company company in (IEnumerable) this.companyList)
        this.cmbCompany.Items.Add((object) company.CompanyName);
      this.session.CreateQuery("from Deliver where IDBASEORG in (SELECT IDBASEORG FROM Postaver) order by NAMEORG").List();
      if (this.session.Get<Company>((object) this.Company_id) != null)
        this.cmbCompany.Text = this.session.Get<Company>((object) Convert.ToInt16(this.session.CreateQuery("select cp from CompanyParam cp where cp.Company.CompanyId=:cmp and cp.Param=201 and :date between DBeg and DEnd").SetParameter<short>("cmp", this.Company_id).SetParameter<DateTime?>("date", Options.Period.PeriodName).UniqueResult<CompanyParam>().ParamValue)).CompanyName;
      else
        this.toolStrip1.Enabled = false;
    }

    private void dgvQuality_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.qualityList.Count <= 0)
        return;
      if (this.dgvQuality.Columns[e.ColumnIndex].Name == "Quality_name")
        e.Value = (object) ((Quality) this.qualityList[e.RowIndex]).Quality_name;
      else if (this.dgvQuality.Columns[e.ColumnIndex].Name == "DocNumber")
        e.Value = (object) ((Quality) this.qualityList[e.RowIndex]).DocNumber;
      else if (this.dgvQuality.Columns[e.ColumnIndex].Name == "DocDate")
        e.Value = (object) ((Quality) this.qualityList[e.RowIndex]).DocDate;
      else if (this.dgvQuality.Columns[e.ColumnIndex].Name == "Legislation_id")
        e.Value = (object) ((Quality) this.qualityList[e.RowIndex]).Legislation_id;
      else if (this.dgvQuality.Columns[e.ColumnIndex].Name == "Coeff")
        e.Value = (object) ((Quality) this.qualityList[e.RowIndex]).Coeff;
      else if (this.dgvQuality.Columns[e.ColumnIndex].Name == "Quantity_hour")
        e.Value = (object) ((Quality) this.qualityList[e.RowIndex]).Quantity_hour;
      else if (this.dgvQuality.Columns[e.ColumnIndex].Name == "Quantity_degree")
        e.Value = (object) ((Quality) this.qualityList[e.RowIndex]).Quantity_degree;
      else if (this.dgvQuality.Columns[e.ColumnIndex].Name == "Supplier_id")
      {
        if (((Quality) this.qualityList[e.RowIndex]).Recipient != null)
          e.Value = (object) ((Quality) this.qualityList[e.RowIndex]).Recipient.BaseOrgId;
      }
      else if (this.dgvQuality.Columns[e.ColumnIndex].Name == "Perfom")
      {
        if (((Quality) this.qualityList[e.RowIndex]).Perfomer != null)
          e.Value = (object) ((Quality) this.qualityList[e.RowIndex]).Perfomer.BaseOrgId;
      }
      else if (this.dgvQuality.Columns[e.ColumnIndex].Name == "UName")
        e.Value = (object) ((Quality) this.qualityList[e.RowIndex]).Uname;
      else if (this.dgvQuality.Columns[e.ColumnIndex].Name == "DEdit")
        e.Value = (object) ((Quality) this.qualityList[e.RowIndex]).Dedit.ToShortDateString();
    }

    private void dgvQuality_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      if (this.tsbApplay.Enabled && this.newQuality == null && !this.Proverka())
        return;
      try
      {
        if (this.dgvQuality.Columns[e.ColumnIndex].Name == "Quality_name")
          ((Quality) this.qualityList[e.RowIndex]).Quality_name = e.Value.ToString();
        else if (this.dgvQuality.Columns[e.ColumnIndex].Name == "DocNumber")
          ((Quality) this.qualityList[e.RowIndex]).DocNumber = e.Value.ToString();
        else if (this.dgvQuality.Columns[e.ColumnIndex].Name == "DocDate" && e.Value != null)
          ((Quality) this.qualityList[e.RowIndex]).DocDate = Convert.ToDateTime(e.Value);
        else if (this.dgvQuality.Columns[e.ColumnIndex].Name == "Coeff" && e.Value != null)
          ((Quality) this.qualityList[e.RowIndex]).Coeff = Convert.ToDouble(KvrplHelper.ChangeSeparator(e.Value.ToString()));
        else if (this.dgvQuality.Columns[e.ColumnIndex].Name == "Quantity_hour" && e.Value != (object) "")
          ((Quality) this.qualityList[e.RowIndex]).Quantity_hour = Convert.ToInt16(e.Value);
        else if (this.dgvQuality.Columns[e.ColumnIndex].Name == "Quantity_degree")
          ((Quality) this.qualityList[e.RowIndex]).Quantity_degree = Convert.ToInt16(e.Value);
        else if (this.dgvQuality.Columns[e.ColumnIndex].Name == "Supplier_id")
          ((Quality) this.qualityList[e.RowIndex]).Recipient = this.session.Get<BaseOrg>((object) Convert.ToInt32(e.Value));
        else if (this.dgvQuality.Columns[e.ColumnIndex].Name == "Perfom")
          ((Quality) this.qualityList[e.RowIndex]).Perfomer = this.session.Get<BaseOrg>((object) Convert.ToInt32(e.Value));
      }
      catch
      {
        int num = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
        return;
      }
      if (this.curAuality != null)
      {
        this.curAuality.Uname = Options.Login;
        this.curAuality.Dedit = DateTime.Now;
      }
    }

    private void dgvQuality_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex <= 0 || !(this.dgvQuality.Columns[e.ColumnIndex].Name == "Legislation_id") || this.dgvQuality.CurrentRow == null || !this.Proverka())
        return;
      FrmLegislation frmLegislation = new FrmLegislation();
      if (((Quality) this.qualityList[e.RowIndex]).Legislation_id.HasValue)
        frmLegislation.Legislation_id = ((Quality) this.qualityList[e.RowIndex]).Legislation_id.Value;
      if (frmLegislation.ShowDialog() == DialogResult.OK)
      {
        if ((int) frmLegislation.Legislation_id == -1)
          ((Quality) this.qualityList[e.RowIndex]).Legislation_id = new short?();
        else
          ((Quality) this.qualityList[e.RowIndex]).Legislation_id = new short?(frmLegislation.Legislation_id);
        this.tsbAdd.Enabled = false;
        this.tsbApplay.Enabled = true;
        this.tsbCancel.Enabled = true;
        this.tsbDelete.Enabled = false;
      }
    }

    private void dgvQuality_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.tsbAdd.Enabled = false;
      this.tsbApplay.Enabled = true;
      this.tsbCancel.Enabled = true;
      this.tsbDelete.Enabled = false;
    }

    private void dgvQuality_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettings.FindByName(e.Column.Name) < 0)
        return;
      this.MySettings.Columns[this.MySettings.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettings.Save();
    }

    private void dgvQuality_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    private void dgvQuality_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (!this.dgvQuality.IsCurrentCellDirty || this.dgvQuality.CurrentCell.ColumnIndex != this.dgvQuality.Rows[this.dgvQuality.CurrentRow.Index].Cells["Supplier_id"].ColumnIndex)
        return;
      this.dgvQuality.CommitEdit(DataGridViewDataErrorContexts.Commit);
      IList<BaseOrg> baseOrgList1 = this.session.CreateQuery(string.Format("select distinct s.Perfomer from CmpSupplier cs, Supplier s where cs.SupplierOrg.SupplierId=s.SupplierId and s.Perfomer.BaseOrgId<>0 and s.Recipient.BaseOrgId={1} and (cs.Service.ServiceId=:id or cs.Service.ServiceId=(select Root from Service where ServiceId=:id)) and cs.Service.ServiceId<>0 and cs.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId=:cmp and Period.PeriodId=0 and DBeg<='{0}' and DEnd>='{0}' and Param.ParamId=211)", (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), (object) Convert.ToInt32(this.dgvQuality.CurrentRow.Cells["Supplier_id"].Value))).SetInt16("id", this.service_id).SetInt16("cmp", this.Company_id).List<BaseOrg>();
      if (Convert.ToInt32(this.dgvQuality.CurrentRow.Cells["Supplier_id"].Value) == 0)
      {
        baseOrgList1.Insert(0, new BaseOrg(0, ""));
      }
      else
      {
        IList<BaseOrg> baseOrgList2 = this.session.CreateQuery(string.Format("select distinct s.Perfomer from CmpSupplier cs, Supplier s where cs.SupplierOrg.SupplierId=s.SupplierId and s.Perfomer.BaseOrgId=0 and s.Recipient.BaseOrgId={1} and (cs.Service.ServiceId=:id or cs.Service.ServiceId=(select Root from Service where ServiceId=:id)) and cs.Service.ServiceId<>0 and cs.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId=:cmp and Period.PeriodId=0 and DBeg<='{0}' and DEnd>='{0}' and Param.ParamId=211)", (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), (object) Convert.ToInt32(this.dgvQuality.CurrentRow.Cells["Supplier_id"].Value))).SetInt16("id", this.service_id).SetInt16("cmp", this.Company_id).List<BaseOrg>();
        if (baseOrgList2.Count > 0)
          baseOrgList1.Insert(0, baseOrgList2[0]);
      }
      this.dgvQuality.CurrentRow.Cells["Perfom"] = (DataGridViewCell) new DataGridViewComboBoxCell()
      {
        DisplayStyleForCurrentCellOnly = true,
        ValueMember = "BaseOrgId",
        DisplayMember = "NameOrgMinDop",
        DataSource = (object) baseOrgList1
      };
      this.dgvQuality.CurrentRow.Cells["Perfom"].Value = (object) baseOrgList1[0].BaseOrgId;
    }

    private void dgvQuality_SelectionChanged(object sender, EventArgs e)
    {
      if (this.dgvQuality.CurrentRow == null || this.dgvQuality.CurrentRow.Index >= this.qualityList.Count)
        return;
      this.curAuality = (Quality) this.qualityList[this.dgvQuality.CurrentRow.Index];
    }

    private void cmbCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(43, 1, (Company) this.companyList[this.cmbCompany.SelectedIndex], true))
      {
        this.cmbCompany.Text = this.session.Get<Company>((object) this.Company_id).CompanyName;
      }
      else
      {
        if ((int) this.Company_id == (int) ((Company) this.companyList[this.cmbCompany.SelectedIndex]).CompanyId)
          return;
        if (this.tsbApplay.Enabled)
        {
          if (MessageBox.Show("Изменения не сохранены! Вернуться к предыдущей компании и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
          {
            this.cmbCompany.Text = this.session.Get<Company>((object) this.Company_id).CompanyName;
            this.tsbApplay_Click(sender, e);
            return;
          }
          this.tsbCancel_Click(sender, e);
        }
        this.Company_id = ((Company) this.companyList[this.cmbCompany.SelectedIndex]).CompanyId;
        IList list = this.session.CreateQuery(string.Format("select distinct s.Recipient from CmpSupplier cs, Supplier s where cs.SupplierOrg.SupplierId=s.SupplierId and s.Recipient.BaseOrgId<>0 and (cs.Service.ServiceId=:id or cs.Service.ServiceId=(select Root from Service where ServiceId=:id)) and cs.Service.ServiceId<>0 and cs.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId=:cmp and Period.PeriodId=0 and DBeg<='{0}' and DEnd>='{0}' and Param.ParamId=211)", (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value))).SetInt16("id", this.service_id).SetInt16("cmp", this.Company_id).List();
        list.Insert(0, (object) new BaseOrg(0, ""));
        this.dtSupplier.Rows.Clear();
        foreach (BaseOrg baseOrg in (IEnumerable) list)
          KvrplHelper.AddRow(this.dtSupplier, baseOrg.BaseOrgId, baseOrg.NameOrgMin);
        this.GetQuality();
        this.toolStrip1.Enabled = true;
      }
    }

    private void cmbService_SelectedIndexChanged(object sender, EventArgs e)
    {
      if ((int) this.Service_id == (int) ((Service) this.serviceList[this.cmbService.SelectedIndex]).ServiceId)
        return;
      if (this.tsbApplay.Enabled)
      {
        if (MessageBox.Show("Изменения не сохранены! Вернуться к предыдущей услуге и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
          this.cmbService.Text = this.session.Get<Service>((object) this.Service_id).ServiceName;
          this.tsbApplay_Click(sender, e);
          return;
        }
        this.tsbCancel_Click(sender, e);
      }
      this.Service_id = ((Service) this.serviceList[this.cmbService.SelectedIndex]).ServiceId;
    }

    private void cmbService_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.sostList = this.session.CreateQuery(string.Format("from Service s where s.Root={0} order by " + Options.SortService, (object) ((Service) this.serviceList[this.cmbService.SelectedIndex]).ServiceId)).List<Service>();
      this.sostList.Insert(0, new Service(Convert.ToInt16(0), ""));
      this.cmbSost.DataSource = (object) this.sostList;
      this.cmbSost.DisplayMember = "ServiceName";
      this.cmbSost.ValueMember = "ServiceId";
    }

    private void cmbSost_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if ((int) this.Service_id == (int) ((Service) this.cmbSost.SelectedItem).ServiceId)
        return;
      this.Service_id = (uint) ((Service) this.cmbSost.SelectedItem).ServiceId <= 0U ? ((Service) this.serviceList[this.cmbService.SelectedIndex]).ServiceId : ((Service) this.cmbSost.SelectedItem).ServiceId;
    }

    private void tsbAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(43, 2, this.session.Get<Company>((object) this.Company_id), true))
        return;
      this.newQuality = new Quality(this.session.CreateSQLQuery("Select DBA.Gen_id('cmpQuality',1)").UniqueResult<int>(), "Акт", this.service_id, this.Company_id);
      this.newQuality.DocDate = DateTime.Now;
      this.qualityList.Add((object) this.newQuality);
      this.dgvQuality.RowCount = this.qualityList.Count;
      this.tsbAdd.Enabled = false;
      this.tsbApplay.Enabled = true;
      this.tsbCancel.Enabled = true;
      this.tsbDelete.Enabled = false;
      if (this.dgvQuality.Rows.Count > 0 && this.dgvQuality.CurrentRow != null)
        this.dgvQuality.CurrentCell = this.dgvQuality.Rows[this.dgvQuality.Rows.Count - 1].Cells[0];
      this.LoadPerfomersList();
      this.dgvQuality.Columns[7].Visible = false;
      this.dgvQuality.Columns[8].Visible = false;
      this.dgvQuality.Refresh();
      this.tsbOnOffSupplier.Visible = true;
      this.dgvQuality.Focus();
      this._OnOff = true;
      this.tsbOnOffSupplier_Click((object) null, (EventArgs) null);
    }

    private void tsbApplay_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(43, 2, this.session.Get<Company>((object) this.Company_id), true))
        return;
      this.dgvQuality.EndEdit();
      if (this.newQuality != null)
      {
        if (this.newQuality.Quality_name == null)
        {
          int num = (int) MessageBox.Show("Введите наименование.", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        DateTime docDate = this.newQuality.DocDate;
        if (false)
        {
          int num = (int) MessageBox.Show("Укажите дату составления акта.", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        if (this.newQuality.DocNumber == null)
        {
          int num = (int) MessageBox.Show("Укажите номер акта.", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        try
        {
          this.newQuality.Uname = Options.Login;
          this.newQuality.Dedit = DateTime.Now;
          try
          {
            this.newQuality.Supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(this.dgvQuality.CurrentRow.Cells["Supplier_id"].Value), (object) Convert.ToInt32(this.dgvQuality.CurrentRow.Cells["Perfom"].Value))).List<Supplier>()[0];
          }
          catch
          {
            int num = (int) MessageBox.Show("Пара получатель-исполнитель не найдена", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          this.session.Save((object) this.newQuality);
          this.session.Flush();
          this.recipient = (BaseOrg) null;
          this.perfomer = (BaseOrg) null;
          this.session.Refresh((object) this.newQuality);
          this.GetQuality();
          this.curAuality = (Quality) this.qualityList[this.dgvQuality.CurrentRow.Index];
          this.newQuality = (Quality) null;
          this.tsbAdd.Enabled = true;
          this.tsbApplay.Enabled = false;
          this.tsbCancel.Enabled = false;
          this.tsbDelete.Enabled = true;
        }
        catch (Exception ex)
        {
          if (ex.InnerException != null && ex.InnerException.Message.ToLower().IndexOf("primary key for table 'cmpquality' is not unique") != -1)
          {
            KvrplHelper.ResetGeners("cmpQuality", "Quality_id");
            int num = (int) MessageBox.Show("Была устранена ошибка генерации уникального поля! Введите запись заново!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          else
          {
            int num1 = (int) MessageBox.Show("Ошибка вставки! Проверьте правильность ввода данных.", "Внимание!", MessageBoxButtons.OK);
          }
        }
      }
      else
      {
        try
        {
          try
          {
            this.curAuality.Supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(this.dgvQuality.CurrentRow.Cells["Supplier_id"].Value), (object) Convert.ToInt32(this.dgvQuality.CurrentRow.Cells["Perfom"].Value))).List<Supplier>()[0];
          }
          catch
          {
            int num = (int) MessageBox.Show("Пара получатель-исполнитель не найдена", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          this.session.Flush();
          this.recipient = (BaseOrg) null;
          this.perfomer = (BaseOrg) null;
          this.tsbAdd.Enabled = true;
          this.tsbApplay.Enabled = false;
          this.tsbCancel.Enabled = false;
          this.tsbDelete.Enabled = true;
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Не удалось внести изменения! Проверьте правильность ввода данных.", "Внимание!", MessageBoxButtons.OK);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      this.dgvQuality.Columns[7].Visible = true;
      this.dgvQuality.Columns[8].Visible = true;
      this.dgvQuality.Refresh();
      this.tsbOnOffSupplier.Visible = false;
    }

    private void tsbDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(43, 2, this.session.Get<Company>((object) this.Company_id), true) || MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      if (this.curAuality != null)
      {
        try
        {
          this.session.Delete((object) this.curAuality);
          this.session.Flush();
          this.curAuality = (Quality) null;
          this.GetQuality();
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Не удалось удалить запись!", "Внимание!", MessageBoxButtons.OK);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
    }

    private void tsbCancel_Click(object sender, EventArgs e)
    {
      this.dgvQuality.EndEdit();
      if (this.newQuality != null)
      {
        if (this.newQuality != null)
          this.qualityList.Remove((object) this.newQuality);
      }
      else
      {
        foreach (Quality quality in (IEnumerable) this.qualityList)
          this.session.Refresh((object) quality);
      }
      this.recipient = (BaseOrg) null;
      this.perfomer = (BaseOrg) null;
      this.GetQuality();
      this.tsbAdd.Enabled = true;
      this.tsbApplay.Enabled = false;
      this.tsbCancel.Enabled = false;
      this.tsbDelete.Enabled = true;
      this.dgvQuality.Columns[7].Visible = true;
      this.dgvQuality.Columns[8].Visible = true;
      this.dgvQuality.Refresh();
      this.tsbOnOffSupplier.Visible = false;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void tsbOnOffSupplier_Click(object sender, EventArgs e)
    {
      if (!this._OnOff)
      {
        this.dgvQuality.Columns[7].Visible = true;
        this.dgvQuality.Columns[8].Visible = true;
        this.dgvQuality.Refresh();
        this.tsbOnOffSupplier.Text = " Выкл Получатель/Исполнитель";
        this._OnOff = true;
      }
      else
      {
        this.dgvQuality.Columns[7].Visible = false;
        this.dgvQuality.Columns[8].Visible = false;
        this.dgvQuality.Refresh();
        this.tsbOnOffSupplier.Text = " Вкл Получатель/Исполнитель";
        this._OnOff = false;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmQuality));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      this.groupBox1 = new GroupBox();
      this.cmbSost = new ComboBox();
      this.lblSost = new Label();
      this.cmbService = new ComboBox();
      this.cmbCompany = new ComboBox();
      this.lblService = new Label();
      this.lblCountry = new Label();
      this.btnExit = new Button();
      this.toolStrip1 = new ToolStrip();
      this.tsbAdd = new ToolStripButton();
      this.tsbApplay = new ToolStripButton();
      this.tsbCancel = new ToolStripButton();
      this.tsbDelete = new ToolStripButton();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.tsbOnOffSupplier = new ToolStripButton();
      this.dsMain = new DataSet();
      this.dtSupplier = new DataTable();
      this.dataColumn1 = new DataColumn();
      this.dataColumn2 = new DataColumn();
      this.pnBtn = new Panel();
      this.dgvQuality = new DataGridView();
      this.Quality_name = new DataGridViewTextBoxColumn();
      this.DocNumber = new DataGridViewTextBoxColumn();
      this.DocDate = new MaskDateColumn();
      this.Legislation_id = new DataGridViewButtonColumn();
      this.Coeff = new DataGridViewTextBoxColumn();
      this.Quantity_hour = new DataGridViewTextBoxColumn();
      this.Quantity_degree = new DataGridViewTextBoxColumn();
      this.Supplier_id = new DataGridViewComboBoxColumn();
      this.Perfom = new DataGridViewComboBoxColumn();
      this.UName = new DataGridViewTextBoxColumn();
      this.DEdit = new DataGridViewTextBoxColumn();
      this.hp = new HelpProvider();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.maskDateColumn1 = new MaskDateColumn();
      this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
      this.groupBox1.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.dsMain.BeginInit();
      this.dtSupplier.BeginInit();
      this.pnBtn.SuspendLayout();
      ((ISupportInitialize) this.dgvQuality).BeginInit();
      this.SuspendLayout();
      this.groupBox1.Controls.Add((Control) this.cmbSost);
      this.groupBox1.Controls.Add((Control) this.lblSost);
      this.groupBox1.Controls.Add((Control) this.cmbService);
      this.groupBox1.Controls.Add((Control) this.cmbCompany);
      this.groupBox1.Controls.Add((Control) this.lblService);
      this.groupBox1.Controls.Add((Control) this.lblCountry);
      this.groupBox1.Dock = DockStyle.Top;
      this.groupBox1.Location = new Point(0, 0);
      this.groupBox1.Margin = new Padding(4);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new Padding(4);
      this.groupBox1.Size = new Size(1178, 104);
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.cmbSost.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbSost.FormattingEnabled = true;
      this.cmbSost.Location = new Point(123, 73);
      this.cmbSost.Margin = new Padding(4);
      this.cmbSost.Name = "cmbSost";
      this.cmbSost.Size = new Size(1042, 24);
      this.cmbSost.TabIndex = 5;
      this.cmbSost.SelectionChangeCommitted += new EventHandler(this.cmbSost_SelectionChangeCommitted);
      this.lblSost.AutoSize = true;
      this.lblSost.Location = new Point(8, 76);
      this.lblSost.Margin = new Padding(4, 0, 4, 0);
      this.lblSost.Name = "lblSost";
      this.lblSost.Size = new Size(107, 17);
      this.lblSost.TabIndex = 4;
      this.lblSost.Text = "Составляющая";
      this.cmbService.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbService.FormattingEnabled = true;
      this.cmbService.Location = new Point(123, 44);
      this.cmbService.Margin = new Padding(4);
      this.cmbService.Name = "cmbService";
      this.cmbService.Size = new Size(1042, 24);
      this.cmbService.TabIndex = 3;
      this.cmbService.SelectedIndexChanged += new EventHandler(this.cmbService_SelectedIndexChanged);
      this.cmbService.SelectionChangeCommitted += new EventHandler(this.cmbService_SelectionChangeCommitted);
      this.cmbCompany.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbCompany.FormattingEnabled = true;
      this.cmbCompany.Location = new Point(123, 15);
      this.cmbCompany.Margin = new Padding(4);
      this.cmbCompany.Name = "cmbCompany";
      this.cmbCompany.Size = new Size(1042, 24);
      this.cmbCompany.TabIndex = 2;
      this.cmbCompany.SelectedIndexChanged += new EventHandler(this.cmbCompany_SelectedIndexChanged);
      this.lblService.AutoSize = true;
      this.lblService.Location = new Point(8, 48);
      this.lblService.Margin = new Padding(4, 0, 4, 0);
      this.lblService.Name = "lblService";
      this.lblService.Size = new Size(52, 17);
      this.lblService.TabIndex = 1;
      this.lblService.Text = "Услуга";
      this.lblCountry.AutoSize = true;
      this.lblCountry.Location = new Point(8, 20);
      this.lblCountry.Margin = new Padding(4, 0, 4, 0);
      this.lblCountry.Name = "lblCountry";
      this.lblCountry.Size = new Size(74, 17);
      this.lblCountry.TabIndex = 0;
      this.lblCountry.Text = "Компания";
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.Location = new Point(1079, 5);
      this.btnExit.Margin = new Padding(4);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(86, 30);
      this.btnExit.TabIndex = 4;
      this.btnExit.Text = "Выход";
      this.btnExit.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.toolStrip1.Font = new Font("Tahoma", 10f);
      this.toolStrip1.Items.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.tsbAdd,
        (ToolStripItem) this.tsbApplay,
        (ToolStripItem) this.tsbCancel,
        (ToolStripItem) this.tsbDelete,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.tsbOnOffSupplier
      });
      this.toolStrip1.Location = new Point(0, 104);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new Size(1178, 25);
      this.toolStrip1.TabIndex = 3;
      this.toolStrip1.Text = "toolStrip1";
      this.tsbAdd.Image = (Image) Resources.add_var;
      this.tsbAdd.ImageTransparentColor = Color.Magenta;
      this.tsbAdd.Name = "tsbAdd";
      this.tsbAdd.Size = new Size(91, 22);
      this.tsbAdd.Text = "Добавить";
      this.tsbAdd.Click += new EventHandler(this.tsbAdd_Click);
      this.tsbApplay.Enabled = false;
      this.tsbApplay.Image = (Image) Resources.Applay_var;
      this.tsbApplay.ImageTransparentColor = Color.Magenta;
      this.tsbApplay.Name = "tsbApplay";
      this.tsbApplay.Size = new Size(99, 22);
      this.tsbApplay.Text = "Сохранить";
      this.tsbApplay.Click += new EventHandler(this.tsbApplay_Click);
      this.tsbCancel.Enabled = false;
      this.tsbCancel.Image = (Image) Resources.undo;
      this.tsbCancel.ImageTransparentColor = Color.Magenta;
      this.tsbCancel.Name = "tsbCancel";
      this.tsbCancel.Size = new Size(77, 22);
      this.tsbCancel.Text = "Отмена";
      this.tsbCancel.Click += new EventHandler(this.tsbCancel_Click);
      this.tsbDelete.Image = (Image) Resources.delete_var;
      this.tsbDelete.ImageTransparentColor = Color.Magenta;
      this.tsbDelete.Name = "tsbDelete";
      this.tsbDelete.Size = new Size(82, 22);
      this.tsbDelete.Text = "Удалить";
      this.tsbDelete.Click += new EventHandler(this.tsbDelete_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(6, 25);
      this.tsbOnOffSupplier.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.tsbOnOffSupplier.Image = (Image) componentResourceManager.GetObject("tsbOnOffSupplier.Image");
      this.tsbOnOffSupplier.ImageTransparentColor = Color.Magenta;
      this.tsbOnOffSupplier.Name = "tsbOnOffSupplier";
      this.tsbOnOffSupplier.Size = new Size(212, 22);
      this.tsbOnOffSupplier.Text = " Вкл Получатель/Исполнитель";
      this.tsbOnOffSupplier.Visible = false;
      this.tsbOnOffSupplier.Click += new EventHandler(this.tsbOnOffSupplier_Click);
      this.dsMain.DataSetName = "NewDataSet";
      this.dsMain.Tables.AddRange(new DataTable[1]
      {
        this.dtSupplier
      });
      this.dtSupplier.Columns.AddRange(new DataColumn[2]
      {
        this.dataColumn1,
        this.dataColumn2
      });
      this.dtSupplier.TableName = "Table1";
      this.dataColumn1.ColumnName = "ID";
      this.dataColumn1.DataType = typeof (int);
      this.dataColumn2.ColumnName = "NAME";
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 567);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(1178, 40);
      this.pnBtn.TabIndex = 4;
      this.dgvQuality.AccessibleRole = AccessibleRole.None;
      this.dgvQuality.AllowUserToAddRows = false;
      this.dgvQuality.AllowUserToDeleteRows = false;
      this.dgvQuality.BackgroundColor = Color.AliceBlue;
      this.dgvQuality.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvQuality.Columns.AddRange((DataGridViewColumn) this.Quality_name, (DataGridViewColumn) this.DocNumber, (DataGridViewColumn) this.DocDate, (DataGridViewColumn) this.Legislation_id, (DataGridViewColumn) this.Coeff, (DataGridViewColumn) this.Quantity_hour, (DataGridViewColumn) this.Quantity_degree, (DataGridViewColumn) this.Supplier_id, (DataGridViewColumn) this.Perfom, (DataGridViewColumn) this.UName, (DataGridViewColumn) this.DEdit);
      this.dgvQuality.Dock = DockStyle.Fill;
      this.dgvQuality.Location = new Point(0, 129);
      this.dgvQuality.Margin = new Padding(4);
      this.dgvQuality.Name = "dgvQuality";
      this.dgvQuality.Size = new Size(1178, 438);
      this.dgvQuality.TabIndex = 5;
      this.dgvQuality.VirtualMode = true;
      this.dgvQuality.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvQuality_CellBeginEdit);
      this.dgvQuality.CellClick += new DataGridViewCellEventHandler(this.dgvQuality_CellClick);
      this.dgvQuality.CellValueNeeded += new DataGridViewCellValueEventHandler(this.dgvQuality_CellValueNeeded);
      this.dgvQuality.CellValuePushed += new DataGridViewCellValueEventHandler(this.dgvQuality_CellValuePushed);
      this.dgvQuality.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvQuality_ColumnWidthChanged);
      this.dgvQuality.CurrentCellDirtyStateChanged += new EventHandler(this.dgvQuality_CurrentCellDirtyStateChanged);
      this.dgvQuality.DataError += new DataGridViewDataErrorEventHandler(this.dgvQuality_DataError);
      this.dgvQuality.SelectionChanged += new EventHandler(this.dgvQuality_SelectionChanged);
      this.Quality_name.HeaderText = "Наименование";
      this.Quality_name.Name = "Quality_name";
      this.Quality_name.Width = 94;
      this.DocNumber.HeaderText = "Номер акта";
      this.DocNumber.Name = "DocNumber";
      this.DocNumber.Width = 94;
      gridViewCellStyle1.Format = "d";
      this.DocDate.DefaultCellStyle = gridViewCellStyle1;
      this.DocDate.HeaderText = "Дата составления акта";
      this.DocDate.Name = "DocDate";
      this.DocDate.Resizable = DataGridViewTriState.True;
      this.DocDate.SortMode = DataGridViewColumnSortMode.Automatic;
      this.DocDate.Width = 94;
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.Legislation_id.DefaultCellStyle = gridViewCellStyle2;
      this.Legislation_id.FlatStyle = FlatStyle.System;
      this.Legislation_id.HeaderText = "Основания";
      this.Legislation_id.Name = "Legislation_id";
      this.Legislation_id.Resizable = DataGridViewTriState.True;
      this.Legislation_id.SortMode = DataGridViewColumnSortMode.Automatic;
      this.Legislation_id.Text = "";
      this.Legislation_id.Width = 95;
      this.Coeff.HeaderText = "Коэффициент";
      this.Coeff.Name = "Coeff";
      this.Coeff.Width = 94;
      this.Quantity_hour.HeaderText = "Часы";
      this.Quantity_hour.Name = "Quantity_hour";
      this.Quantity_hour.Width = 94;
      this.Quantity_degree.HeaderText = "Градусы";
      this.Quantity_degree.Name = "Quantity_degree";
      this.Quantity_degree.Width = 94;
      this.Supplier_id.DataSource = (object) this.dsMain;
      this.Supplier_id.DisplayMember = "Table1.NAME";
      this.Supplier_id.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      this.Supplier_id.HeaderText = "Получатель";
      this.Supplier_id.Name = "Supplier_id";
      this.Supplier_id.Resizable = DataGridViewTriState.True;
      this.Supplier_id.SortMode = DataGridViewColumnSortMode.Automatic;
      this.Supplier_id.ValueMember = "Table1.ID";
      this.Supplier_id.Width = 94;
      this.Perfom.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      this.Perfom.HeaderText = "Исполнитель";
      this.Perfom.Name = "Perfom";
      this.Perfom.SortMode = DataGridViewColumnSortMode.Automatic;
      this.UName.HeaderText = "Пользователь";
      this.UName.Name = "UName";
      this.UName.ReadOnly = true;
      this.DEdit.HeaderText = "Дата редактирования";
      this.DEdit.Name = "DEdit";
      this.DEdit.ReadOnly = true;
      this.hp.HelpNamespace = "Help.chm";
      this.dataGridViewTextBoxColumn1.HeaderText = "Наименование";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.Width = 94;
      this.dataGridViewTextBoxColumn2.HeaderText = "Номер акта";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn2.Width = 94;
      gridViewCellStyle3.Format = "d";
      this.maskDateColumn1.DefaultCellStyle = gridViewCellStyle3;
      this.maskDateColumn1.HeaderText = "Дата составления акта";
      this.maskDateColumn1.Name = "maskDateColumn1";
      this.maskDateColumn1.Resizable = DataGridViewTriState.True;
      this.maskDateColumn1.SortMode = DataGridViewColumnSortMode.Automatic;
      this.maskDateColumn1.Width = 94;
      this.dataGridViewTextBoxColumn3.HeaderText = "Коэффициент";
      this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
      this.dataGridViewTextBoxColumn3.Width = 94;
      this.dataGridViewTextBoxColumn4.HeaderText = "Часы";
      this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
      this.dataGridViewTextBoxColumn4.Width = 94;
      this.dataGridViewTextBoxColumn5.HeaderText = "Градусы";
      this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
      this.dataGridViewTextBoxColumn5.Width = 94;
      this.dataGridViewTextBoxColumn6.HeaderText = "Пользователь";
      this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
      this.dataGridViewTextBoxColumn6.ReadOnly = true;
      this.dataGridViewTextBoxColumn7.HeaderText = "Дата редактирования";
      this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
      this.dataGridViewTextBoxColumn7.ReadOnly = true;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(1178, 607);
      this.Controls.Add((Control) this.dgvQuality);
      this.Controls.Add((Control) this.pnBtn);
      this.Controls.Add((Control) this.toolStrip1);
      this.Controls.Add((Control) this.groupBox1);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.hp.SetHelpKeyword((Control) this, "kv54.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmQuality";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Акты-документы на недопоставку ";
      this.Load += new EventHandler(this.FrmQuality_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.dsMain.EndInit();
      this.dtSupplier.EndInit();
      this.pnBtn.ResumeLayout(false);
      ((ISupportInitialize) this.dgvQuality).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
