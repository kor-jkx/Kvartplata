// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmTariffs
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Forms.Controls;
using Kvartplata.Properties;
using NHibernate;
using SaveSettings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmTariffs : Form
  {
    private GridSettings MySettings = new GridSettings();
    private Dictionary<short, Service> _updAdmId = new Dictionary<short, Service>();
    private IList companyList = (IList) new ArrayList();
    private IList<Company> companyNormList = (IList<Company>) new List<Company>();
    private short company_id = 0;
    private FormStateSaver fss = new FormStateSaver(FrmTariffs.ic);
    private bool isEdit = false;
    private bool isPast = false;
    private bool isPastNorm = false;
    private IList serviceList = (IList) new ArrayList();
    private IList tariffCostPartList = (IList) new ArrayList();
    private bool serviceReadOnly = false;
    private bool normVariantReadOnly = false;
    private bool tarifNormReadOnly = false;
    private IContainer components = (IContainer) null;
    private static IContainer ic;
    private DateTime? closedPeriod;
    private Service curService;
    private Service curSrvSost;
    private BaseOrg manager;
    private Service newService;
    private Service newSrvSost;
    private ISession session;
    private Company _company;
    private Panel pnlService;
    private SplitContainer splitContainerService;
    private TreeView tvService;
    private ToolStrip toolStripService;
    private ToolStrip toolStrip1;
    private ToolStripButton tsbService;
    private ToolStripButton tsbTariffs;
    private ToolStripButton tsbAddService;
    private ToolStripButton tsbAddComponent;
    private ToolStripButton tsbEditServ;
    private ToolStripButton tsbDelete;
    private Panel pnlTariff;
    private GroupBox groupBox1;
    private ComboBox cbServ;
    private Label lblService;
    private Button btnLast;
    private Label lblCompany;
    private ComboBox cbCompany;
    private DataGridView dgvTariffCostByPart;
    private DataSet dsMain;
    private DataTable dtUnitMeasuring;
    private DataColumn dcUnitMeasuring_id;
    private DataColumn dcUnitMeasuring_name;
    private DataColumn dcBaseTariff;
    private DataColumn dcBaseTarif_name;
    private DataTable dtBaseTariff;
    private SplitContainer splitContainer1;
    private ToolStripButton tsbNorm;
    private Panel pnlNorm;
    private GroupBox groupBox2;
    private Button btnPastNorm;
    private Label label1;
    private ComboBox cbCompanyNorm;
    private Label label3;
    private ComboBox cbServNorm;
    private SplitContainer splitContainer2;
    private SplitContainer splitContainer3;
    private ContextMenuStrip MenuForTariffs;
    private ToolStripMenuItem tsmCopy;
    private ToolStripMenuItem tsmCopyInPast;
    private DataTable dtCounter;
    private DataColumn dataColumn1;
    private DataColumn dataColumn2;
    private Label lblPast;
    private Label lblPastNorm;
    private ToolStripButton tsbExit;
    private CalendarColumn calendarColumn1;
    private CalendarColumn calendarColumn2;
    private CalendarColumn calendarColumn3;
    private CalendarColumn calendarColumn4;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
    private CalendarColumn calendarColumn5;
    private CalendarColumn calendarColumn6;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
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
    private DataTable dtBaseTariff2;
    private DataColumn dataColumn3;
    private DataColumn dataColumn4;
    private UCNormCost ucNormCost1;
    private UCTariffCost ucTariffCost1;
    private NumericUpDown nServ;
    private NumericUpDown nNormServ;
    private CheckBox cbArchiv;
    private CheckBox cbNormArchive;
    public HelpProvider hp;
    private Timer tmr;
    private DataGridViewTextBoxColumn serviceColumnPart;
    private DataGridViewTextBoxColumn dbegColumnPart;
    private DataGridViewTextBoxColumn dendColumnPart;
    private DataGridViewButtonColumn SchemaColumnPart;
    private DataGridViewTextBoxColumn CostColumnPart;
    private DataGridViewTextBoxColumn EoTariffColumnPart;
    private DataGridViewTextBoxColumn CTariffColumnPart;
    private MonthPicker mpCurrentPeriod;
    private MonthPicker mpCurrentPeriodNorm;
    private Panel pnManager;
    private Label lblManager;
    private ComboBox cmbManager;
    private UCNorm ucNorm1;
    private UСVariant uсVariant1;

    public Service CurService
    {
      get
      {
        return this.curService;
      }
      set
      {
        if (this.curService == value)
          return;
        if (this.uсVariant1.tsbApplay.Enabled)
        {
          if (MessageBox.Show("Изменения не сохранены! Вернуться к предыдущей услуге и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
          {
            this.tvService.SelectedNode = this.tvService.Nodes.Find(this.curService.ServiceId.ToString(), true)[0];
            this.uсVariant1.SaveData();
            return;
          }
          this.uсVariant1.LoadData();
          this.uсVariant1.CancelEnabled();
        }
        if (this.ucNorm1.tsbApplay.Enabled)
        {
          if (MessageBox.Show("Изменения не сохранены! Вернуться к предыдущей услуге и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
          {
            this.tvService.SelectedNode = this.tvService.Nodes.Find(this.curService.ServiceId.ToString(), true)[0];
            this.ucNorm1.SaveData();
            return;
          }
          this.ucNorm1.LoadData();
          this.ucNorm1.CancelEnabled();
        }
        this.curService = value;
        if (value != null)
        {
          this.cbServ.SelectedItem = (object) value;
          this.cbServNorm.SelectedItem = (object) value;
          this.uсVariant1.CurService = this.CurService;
          this.uсVariant1.LoadData();
          this.ucNorm1.CurService = this.CurService;
          this.ucNorm1.LoadData();
        }
      }
    }

    public short Company_id
    {
      get
      {
        return this.company_id;
      }
      set
      {
        this.company_id = value;
        this.cbCompany.SelectedItem = (object) this.session.Get<Company>((object) value);
        Period closedPeriod = this.GetClosedPeriod();
        this.closedPeriod = !closedPeriod.PeriodName.HasValue ? new DateTime?() : new DateTime?(closedPeriod.PeriodName.Value);
        this.ucNormCost1.closedPeriod = this.closedPeriod;
        this.ucTariffCost1.closedPeriod = this.closedPeriod;
        if (this.cbCompanyNorm.SelectedItem != null)
          this.ucNormCost1.Company_id = this.company_id;
        if (this.cbCompany.SelectedItem == null)
          return;
        this.ucTariffCost1.Company_id = this.company_id;
      }
    }

    public FrmTariffs(Company currentCompany)
    {
      this.InitializeComponent();
      this._company = currentCompany;
      this.CheckAccess();
      this.session = Domain.CurrentSession;
      this.ucNorm1.session = Domain.CurrentSession;
      this.ucNorm1.tsbExit.Visible = false;
      this.ucNorm1.LoadSettings();
      this.uсVariant1.session = Domain.CurrentSession;
      this.uсVariant1.tsbExit.Visible = false;
      this.uсVariant1.LoadSettings();
      this.ucNormCost1.session = Domain.CurrentSession;
      this.ucNormCost1.tsbExit.Visible = false;
      this.ucNormCost1.LoadSettings();
      this.ucTariffCost1.session = Domain.CurrentSession;
      this.ucTariffCost1.tsbExit.Visible = false;
      this.ucTariffCost1.LoadSettings();
      this.fss.ParentForm = (Form) this;
      this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
      this.SetGridConfigFileSettings();
      this.LoadSettings();
    }

    private void CheckAccess()
    {
      this.serviceReadOnly = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(32, this._company, false));
      this.normVariantReadOnly = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(67, this._company, false));
      this.tarifNormReadOnly = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(33, this._company, false));
      this.toolStripService.Visible = this.serviceReadOnly;
      this.uсVariant1.toolStrip1.Visible = this.normVariantReadOnly;
      this.uсVariant1.dgvBase.ReadOnly = !this.normVariantReadOnly;
      this.ucNorm1.toolStrip1.Visible = this.normVariantReadOnly;
      this.ucNorm1.dgvBase.ReadOnly = !this.normVariantReadOnly;
      this.ucNormCost1.toolStrip1.Visible = this.tarifNormReadOnly;
      this.ucTariffCost1.toolStrip1.Visible = this.tarifNormReadOnly;
      this.ucNormCost1.dgvBase.ReadOnly = !this.tarifNormReadOnly;
      this.ucTariffCost1.dgvBase.ReadOnly = !this.tarifNormReadOnly;
    }

    public void SetGridConfigFileSettings()
    {
      this.MySettings.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
    }

    protected object SelectRow(object curObject, IList objectsList, DataGridView dgvBase)
    {
      if (curObject != null)
      {
        int curObject1 = KvrplHelper.FindCurObject(objectsList, curObject);
        if (curObject1 >= 0)
        {
          dgvBase.CurrentCell = dgvBase.Rows[curObject1].Cells[dgvBase.Columns.Count - 1];
          dgvBase.Rows[curObject1].Selected = true;
        }
      }
      else if (dgvBase.CurrentRow != null && dgvBase.CurrentRow.Index < objectsList.Count)
        curObject = objectsList[dgvBase.CurrentRow.Index];
      return curObject;
    }

    private void BuildTree()
    {
      foreach (Service service1 in (IEnumerable) this.serviceList)
      {
        TreeNode node = new TreeNode(service1.ServiceId.ToString() + "  " + service1.ServiceName);
        node.Name = service1.ServiceId.ToString();
        node.Tag = (object) service1;
        foreach (Service service2 in (IEnumerable) service1.ChildService)
          node.Nodes.Add(new TreeNode(service2.ServiceName)
          {
            Tag = (object) service2
          });
        this.tvService.Nodes.Add(node);
      }
    }

    private void LoadCb()
    {
      this.cbServ.Items.Clear();
      this.cbServNorm.Items.Clear();
      List<Service> source = new List<Service>();
      foreach (Service service in (IEnumerable) this.serviceList)
        source.Add(service);
      IOrderedEnumerable<Service> orderedEnumerable = source.OrderBy<Service, short>((Func<Service, short>) (_l => _l.ServiceId));
      if (Options.SortService == " s.ServiceName")
        orderedEnumerable = source.OrderBy<Service, string>((Func<Service, string>) (_l => _l.ServiceName));
      foreach (Service service in (IEnumerable<Service>) orderedEnumerable)
      {
        this.cbServ.Items.Add((object) service);
        this.cbServNorm.Items.Add((object) service);
      }
    }

    private Service GetServiceByName(string name)
    {
      Service service1 = (Service) null;
      foreach (Service service2 in (IEnumerable) this.serviceList)
      {
        if (service2.ServiceName.CompareTo(name) == 0)
          service1 = service2;
      }
      return service1;
    }

    private Period GetClosedPeriod()
    {
      return this.session.Get<Period>((object) Convert.ToInt32(this.session.CreateQuery(string.Format("select max(p.Period_id) from ClosedPeriod p where (p.Complex_id = {0} or p.Complex_id = {2}) and p.Company_id = {1}", (object) Options.ComplexPasp.ComplexId, (object) this.Company_id, (object) Options.ComplexPrior.IdFk)).UniqueResult()));
    }

    private void LoadSettings()
    {
      this.MySettings.GridName = "TariffCostByPart";
      this.MySettings.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvTariffCostByPart.Columns)
        this.MySettings.GetMySettings(column);
    }

    private void frmTariffs_Load(object sender, EventArgs e)
    {
      if (this.session != null && this.session.IsOpen)
        this.session.Clear();
      if (!KvrplHelper.CheckProxy(32, 1, (Company) null, false))
      {
        this.tsbTariffs.Checked = true;
        this.tsbTariffs_Click(sender, e);
      }
      this.Cursor = Cursors.WaitCursor;
      this.serviceList = this.session.CreateQuery("from Service s where s.Root=0 and s.ServiceId>0 order by " + Options.SortService).List();
      this.LoadCb();
      this.BuildTree();
      this.tvService.ExpandAll();
      this.tvService.CollapseAll();
      IList<BaseOrg> baseOrgList = this.session.CreateQuery("select new BaseOrg(b.BaseOrgId,b.NameOrgMin) from BaseOrg b where BaseOrgId in (select Manager.BaseOrgId from Company) order by b.NameOrgMin").List<BaseOrg>();
      this.cmbManager.DisplayMember = "NameOrgMin";
      this.cmbManager.ValueMember = "BaseOrgId";
      this.cmbManager.DataSource = (object) baseOrgList;
      BaseOrg baseOrg1 = new BaseOrg();
      BaseOrg baseOrg2;
      try
      {
        baseOrg2 = this.session.Get<BaseOrg>((object) Options.Company.Manager.BaseOrgId);
      }
      catch (Exception ex)
      {
        baseOrg2 = this.session.CreateQuery(string.Format("select c.Manager from Company c where c.CompanyId={0}", (object) Options.Company.CompanyId)).List<BaseOrg>()[0];
      }
      if (baseOrg2 != null)
      {
        this.cmbManager.SelectedValue = (object) baseOrg2.BaseOrgId;
        this.uсVariant1.Manager = baseOrg2;
        this.ucNorm1.Manager = baseOrg2;
      }
      this.companyList = this.session.CreateQuery("from Company order by CompanyId").List();
      this.cbCompany.DisplayMember = "CompanyName";
      this.cbCompany.ValueMember = "CompanyId";
      this.cbCompany.DataSource = (object) this.companyList;
      this.cbServNorm.DisplayMember = "ServiceName";
      this.cbServNorm.ValueMember = "ServiceId";
      this.cbServ.DisplayMember = "ServiceName";
      this.cbServ.ValueMember = "ServiceId";
      this.session.Clear();
      this.companyNormList = this.session.CreateQuery("from Company order by CompanyId").List<Company>();
      this.cbCompanyNorm.DisplayMember = "CompanyName";
      this.cbCompanyNorm.ValueMember = "CompanyId";
      this.cbCompanyNorm.DataSource = (object) this.companyNormList;
      short num1 = 0;
      Company company1 = new Company();
      short num2;
      try
      {
        num2 = Convert.ToInt16(this.session.CreateQuery(string.Format("select cp.ParamValue from CompanyParam cp where cp.Company.CompanyId={1} and cp.Period.PeriodId=0 and cp.Param.ParamId=201 and DBeg<='{0}' and DEnd>='{0}'", (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), (object) Options.Company.CompanyId)).UniqueResult());
        if ((int) num2 == 0)
          num2 = Options.Company.CompanyId;
      }
      catch (Exception ex)
      {
        num2 = Options.Company.CompanyId;
      }
      Company company2 = this.session.Get<Company>((object) num2);
      if (company2 != null)
        this.cbCompany.SelectedValue = (object) company2.CompanyId;
      this.session.Clear();
      num1 = (short) 0;
      company1 = new Company();
      short num3;
      try
      {
        num3 = Convert.ToInt16(this.session.CreateQuery(string.Format("select cp.ParamValue from CompanyParam cp where cp.Company.CompanyId={1} and cp.Period.PeriodId=0 and cp.Param.ParamId=204 and DBeg<='{0}' and DEnd>='{0}'", (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), (object) Options.Company.CompanyId)).UniqueResult());
        if ((int) num3 == 0)
          num3 = Options.Company.CompanyId;
      }
      catch (Exception ex)
      {
        num3 = Options.Company.CompanyId;
      }
      Company company3 = this.session.Get<Company>((object) num3);
      if (company3 != null)
        this.cbCompanyNorm.SelectedValue = (object) company3.CompanyId;
      if (this.tvService.Nodes.Count > 0)
        this.tvService.SelectedNode = this.tvService.Nodes[0];
      ((DataGridViewComboBoxColumn) this.ucTariffCost1.dgvBase.Columns["UnitMeasuring_id"]).DataSource = (object) this.session.CreateQuery("from dcUnitMeasuring ").List();
      ((DataGridViewComboBoxColumn) this.ucTariffCost1.dgvBase.Columns["IsVat"]).DataSource = (object) this.session.CreateQuery("from YesNo order by YesNoId").List();
      foreach (BaseTariff baseTariff in (IEnumerable) this.session.CreateQuery("from BaseTariff ").List())
      {
        KvrplHelper.AddRow(this.dtBaseTariff, baseTariff.BaseTariff_id, baseTariff.BaseTariff_name);
        if (baseTariff.BaseTariff_id == 1 || baseTariff.BaseTariff_id == 2 || baseTariff.BaseTariff_id == 3)
          KvrplHelper.AddRow(this.dtBaseTariff2, baseTariff.BaseTariff_id, baseTariff.BaseTariff_name);
      }
      ((DataGridViewComboBoxColumn) this.ucTariffCost1.dgvBase.Columns["BaseTariffMSP_id"]).DataSource = (object) this.dtBaseTariff2;
      ((DataGridViewComboBoxColumn) this.ucTariffCost1.dgvBase.Columns["BaseTariff_id"]).DataSource = (object) this.dtBaseTariff;
      Period closedPeriod = this.GetClosedPeriod();
      DateTime? nullable1 = closedPeriod.PeriodName;
      if (nullable1.HasValue)
      {
        nullable1 = closedPeriod.PeriodName;
        this.closedPeriod = new DateTime?(nullable1.Value);
      }
      else
        this.closedPeriod = new DateTime?();
      UСVariant uсVariant1 = this.uсVariant1;
      UCNorm ucNorm1 = this.ucNorm1;
      UCNormCost ucNormCost1 = this.ucNormCost1;
      this.ucTariffCost1.closedPeriod = nullable1 = this.closedPeriod;
      DateTime? nullable2;
      nullable1 = nullable2 = nullable1;
      ucNormCost1.closedPeriod = nullable2;
      DateTime? nullable3;
      nullable1 = nullable3 = nullable1;
      ucNorm1.closedPeriod = nullable3;
      DateTime? nullable4 = nullable1;
      uсVariant1.closedPeriod = nullable4;
      this.uсVariant1.CurService = this.ucNorm1.CurService = this.ucNormCost1.CurService = this.ucTariffCost1.CurService = this.CurService;
      this.uсVariant1.LoadData();
      this.ucNorm1.LoadData();
      this.Cursor = Cursors.Default;
      this._updAdmId.Clear();
      if (KvrplHelper.CheckProxy(67, 1, Options.Company, false))
        return;
      this.uсVariant1.Visible = false;
      this.ucNorm1.Visible = false;
      this.pnManager.Visible = false;
    }

    private void tsbService_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 1, (Company) null, true))
      {
        this.tsbService.Checked = false;
      }
      else
      {
        this.CurService = this.CurService;
        if (this.ucTariffCost1.tsbApplay.Enabled)
        {
          if (MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
          {
            this.ucTariffCost1.SaveData();
            this.tsbService.Checked = false;
            return;
          }
          this.ucTariffCost1.LoadData();
          this.ucTariffCost1.CancelEnabled();
        }
        if (this.ucNormCost1.tsbApplay.Enabled)
        {
          if (MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
          {
            this.ucNormCost1.SaveData();
            this.tsbService.Checked = false;
            return;
          }
          this.ucNormCost1.LoadData();
          this.ucNormCost1.CancelEnabled();
        }
        this.tsbTariffs.Checked = false;
        this.tsbNorm.Checked = false;
        this.pnlService.BringToFront();
      }
    }

    private void tsbTariffs_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 1, this.session.Get<Company>((object) this.company_id), true))
      {
        this.tsbTariffs.Checked = false;
      }
      else
      {
        this.ucTariffCost1.CurService = this.CurService;
        if (this.uсVariant1.tsbApplay.Enabled || this.ucNorm1.tsbApplay.Enabled)
        {
          if (MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
          {
            if (this.uсVariant1.tsbApplay.Enabled)
              this.uсVariant1.SaveData();
            if (this.ucNorm1.tsbApplay.Enabled)
              this.ucNorm1.SaveData();
            this.tsbTariffs.Checked = false;
            return;
          }
          this.uсVariant1.LoadData();
          this.uсVariant1.CancelEnabled();
          this.ucNorm1.LoadData();
          this.ucNorm1.CancelEnabled();
        }
        if (this.ucNormCost1.tsbApplay.Enabled)
        {
          if (MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
          {
            this.ucNorm1.SaveData();
            this.tsbTariffs.Checked = false;
            return;
          }
          this.ucNormCost1.LoadData();
          this.ucNormCost1.CancelEnabled();
        }
        this.tsbService.Checked = false;
        this.tsbNorm.Checked = false;
        this.pnlTariff.BringToFront();
      }
    }

    private void tsbNorm_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 1, this.session.Get<Company>((object) this.company_id), true))
      {
        this.tsbNorm.Checked = false;
      }
      else
      {
        this.ucNormCost1.CurService = this.CurService;
        if (this.ucTariffCost1.tsbApplay.Enabled)
        {
          if (MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
          {
            this.ucTariffCost1.SaveData();
            this.tsbNorm.Checked = false;
            return;
          }
          this.ucTariffCost1.LoadData();
          this.ucTariffCost1.CancelEnabled();
        }
        if (this.uсVariant1.tsbApplay.Enabled || this.ucNorm1.tsbApplay.Enabled)
        {
          if (MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
          {
            if (this.uсVariant1.tsbApplay.Enabled)
              this.uсVariant1.SaveData();
            if (this.ucNorm1.tsbApplay.Enabled)
              this.ucNorm1.SaveData();
            this.tsbNorm.Checked = false;
            return;
          }
          this.uсVariant1.LoadData();
          this.uсVariant1.CancelEnabled();
          this.ucNorm1.LoadData();
          this.ucNorm1.CancelEnabled();
        }
        this.tsbService.Checked = false;
        this.tsbTariffs.Checked = false;
        this.pnlNorm.BringToFront();
      }
    }

    private void cbServ_SelectedValueChanged(object sender, EventArgs e)
    {
      if (this.cbServ.SelectedItem == null)
        return;
      this.nServ.Value = (Decimal) ((Service) this.cbServ.SelectedItem).ServiceId;
      this.CurService = (Service) this.cbServ.SelectedItem;
      this.ucTariffCost1.CurService = (Service) this.cbServ.SelectedItem;
      if (this.ucTariffCost1.CurService != (Service) this.cbServ.SelectedItem)
      {
        this.CurService = this.ucTariffCost1.CurService;
        this.cbServ.SelectedItem = (object) this.ucTariffCost1.CurService;
      }
    }

    private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cbCompany.SelectedItem == null)
        return;
      if (this.tsbTariffs.Checked && !KvrplHelper.CheckProxy(33, 1, (Company) this.cbCompany.SelectedItem, true))
        this.cbCompany.SelectedItem = (object) this.session.Get<Company>((object) this.Company_id);
      else
        this.Company_id = ((Company) this.cbCompany.SelectedItem).CompanyId;
      if ((int) this.ucTariffCost1.Company_id != (int) this.Company_id)
        this.cbCompany.SelectedItem = (object) this.session.Get<Company>((object) this.ucTariffCost1.Company_id);
    }

    private void btnLast_Click(object sender, EventArgs e)
    {
      this.dgvTariffCostByPart.RowCount = this.tariffCostPartList.Count;
      this.dgvTariffCostByPart.Refresh();
      this.isPast = !this.isPast;
      this.ucTariffCost1.IsPast = this.isPast;
      if (this.ucTariffCost1.IsPast == this.isPast)
      {
        if (this.isPast)
        {
          this.tmr.Start();
          this.groupBox1.Height += 18;
          this.btnLast.BackColor = Color.DarkOrange;
        }
        else
        {
          this.tmr.Stop();
          this.groupBox1.Height -= 18;
          this.btnLast.Text = "Прошлое время";
          this.btnLast.BackColor = this.BackColor;
        }
      }
      else
        this.isPast = !this.isPast;
    }

    private void mpCurrentPeriod_ValueChanged(object sender, EventArgs e)
    {
      Options.Period = KvrplHelper.SaveCurrentPeriod(this.mpCurrentPeriod.Value);
      if (!this.isPast)
        return;
      this.dgvTariffCostByPart.RowCount = this.tariffCostPartList.Count;
      this.dgvTariffCostByPart.Refresh();
      this.ucTariffCost1.IsPast = this.isPast;
    }

    private void cbCompanyNorm_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cbCompanyNorm.SelectedItem == null)
        return;
      if (this.tsbNorm.Checked && !KvrplHelper.CheckProxy(33, 1, this.session.Get<Company>((object) this.Company_id), true))
        this.cbCompanyNorm.SelectedItem = (object) this.session.Get<Company>((object) this.Company_id);
      else
        this.Company_id = ((Company) this.cbCompanyNorm.SelectedItem).CompanyId;
      if ((int) this.ucNormCost1.Company_id != (int) this.Company_id)
        this.cbCompanyNorm.SelectedItem = (object) this.session.Get<Company>((object) this.ucNormCost1.Company_id);
    }

    private void cbServNorm_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cbServNorm.SelectedItem == null)
        return;
      this.nNormServ.Value = (Decimal) ((Service) this.cbServNorm.SelectedItem).ServiceId;
      this.CurService = (Service) this.cbServNorm.SelectedItem;
      this.ucNormCost1.CurService = (Service) this.cbServNorm.SelectedItem;
      if (this.ucNormCost1.CurService != (Service) this.cbServNorm.SelectedItem)
      {
        this.CurService = this.ucNormCost1.CurService;
        this.cbServNorm.SelectedItem = (object) this.ucNormCost1.CurService;
      }
    }

    private void btmPastNorm_Click(object sender, EventArgs e)
    {
      this.isPastNorm = !this.isPastNorm;
      this.ucNormCost1.IsPast = this.isPastNorm;
      if (this.ucNormCost1.IsPast == this.isPastNorm)
      {
        if (this.isPastNorm)
        {
          this.tmr.Start();
          this.groupBox2.Height += 18;
          this.btnPastNorm.BackColor = Color.DarkOrange;
        }
        else
        {
          this.tmr.Stop();
          this.groupBox2.Height -= 18;
          this.btnPastNorm.Text = "Прошлое время";
          this.btnPastNorm.BackColor = this.BackColor;
        }
      }
      else
        this.isPastNorm = !this.isPastNorm;
    }

    private void mpCurrentPeriodNorm_ValueChanged(object sender, EventArgs e)
    {
      Options.Period = KvrplHelper.SaveCurrentPeriod(this.mpCurrentPeriodNorm.Value);
      if (!this.isPastNorm)
        return;
      this.ucNormCost1.IsPast = this.isPastNorm;
    }

    private void tsbExit_Click(object sender, EventArgs e)
    {
      if ((this.uсVariant1.tsbApplay.Enabled || this.ucNorm1.tsbApplay.Enabled || this.ucNormCost1.tsbApplay.Enabled || this.ucTariffCost1.tsbApplay.Enabled) && MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
      {
        this.tsbExit.Checked = false;
        if (this.uсVariant1.tsbApplay.Enabled)
          this.uсVariant1.SaveData();
        else if (this.ucNorm1.tsbApplay.Enabled)
          this.ucNorm1.SaveData();
        if (this.ucNormCost1.tsbApplay.Enabled)
        {
          this.ucNormCost1.SaveData();
        }
        else
        {
          if (!this.ucTariffCost1.tsbApplay.Enabled)
            return;
          this.ucTariffCost1.SaveData();
        }
      }
      else
        this.Close();
    }

    private void ucTariffCost1_TariffCostPartListChanged(object sender, EventArgs e)
    {
      if (this.ucTariffCost1.TariffCostPartList == null)
        return;
      this.tariffCostPartList = this.ucTariffCost1.TariffCostPartList;
      this.dgvTariffCostByPart.RowCount = this.tariffCostPartList.Count;
      this.dgvTariffCostByPart.Refresh();
    }

    private void ucTariffCost1_ApplayClick(object sender, EventArgs e)
    {
      this.dgvTariffCostByPart.EndEdit();
      this.dgvTariffCostByPart.Refresh();
    }

    private void ucTariffCost1_CancelClick(object sender, EventArgs e)
    {
      this.dgvTariffCostByPart.EndEdit();
      this.dgvTariffCostByPart.Refresh();
    }

    private void ucTariffCost1_ObjectsListChanged(object sender, EventArgs e)
    {
      if (!this.ucTariffCost1.IsCopy)
        return;
      this.isPast = !this.isPast;
      this.groupBox1.Height += 18;
      this.ucTariffCost1.IsCopy = false;
    }

    private void uсVariant1_ObjectsListChanged(object sender, EventArgs e)
    {
      IList list = this.session.CreateQuery(string.Format("from Tariff where Service_id={0} and Manager.BaseOrgId={1}", (object) this.curService.ServiceId, (object) this.uсVariant1.Manager.BaseOrgId)).List();
      if (list.Count == 0)
        this.ucTariffCost1.toolStrip1.Enabled = false;
      else
        this.ucTariffCost1.toolStrip1.Enabled = true;
      list.Insert(0, (object) new Tariff()
      {
        Tariff_id = int.MaxValue
      });
      ((DataGridViewComboBoxColumn) this.ucTariffCost1.dgvBase.Columns["Variant"]).DataSource = (object) list;
      this.ucTariffCost1.LoadData();
    }

    private void ucNormCost1_ObjectsListChanged(object sender, EventArgs e)
    {
      if (!this.ucNormCost1.IsCopy)
        return;
      this.isPastNorm = !this.isPastNorm;
      this.groupBox2.Height += 18;
      this.ucNormCost1.IsCopy = false;
    }

    private void ucNorm1_ObjectsListChanged(object sender, EventArgs e)
    {
      IList list = this.session.CreateQuery(string.Format("from Norm where Service_id={0} and Manager.BaseOrgId={1}", (object) this.curService.ServiceId, (object) this.ucNorm1.Manager.BaseOrgId)).List();
      list.Insert(0, (object) new Norm()
      {
        Norm_id = int.MaxValue
      });
      ((DataGridViewComboBoxColumn) this.ucNormCost1.dgvBase.Columns["Norm"]).DataSource = (object) list;
      this.ucNormCost1.LoadData();
    }

    private void dgvTariffCostByPart_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.tariffCostPartList.Count <= 0)
        return;
      if (this.closedPeriod.HasValue)
      {
        int num;
        if (this.isPast || !(((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Dbeg < this.closedPeriod.Value.AddMonths(2)) || !(((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Dend >= this.closedPeriod.Value.AddMonths(1)))
        {
          if (this.isPast)
          {
            DateTime? periodName = ((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Period.PeriodName;
            DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
            num = periodName.HasValue ? (periodName.HasValue ? (periodName.GetValueOrDefault() == dateTime ? 1 : 0) : 1) : 0;
          }
          else
            num = 0;
        }
        else
          num = 1;
        if (num != 0)
          this.dgvTariffCostByPart.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.PapayaWhip;
        else
          this.dgvTariffCostByPart.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      }
      try
      {
        if (this.dgvTariffCostByPart.Columns[e.ColumnIndex].Name == "dbegColumnPart")
          e.Value = (object) ((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Dbeg;
        else if (this.dgvTariffCostByPart.Columns[e.ColumnIndex].Name == "dendColumnPart")
          e.Value = (object) ((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Dend;
        else if (this.dgvTariffCostByPart.Columns[e.ColumnIndex].Name == "CostColumnPart")
          e.Value = (object) KvrplHelper.ChangeSeparator(((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Cost.ToString());
        else if (this.dgvTariffCostByPart.Columns[e.ColumnIndex].Name == "SchemaColumnPart")
          e.Value = (object) ((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Scheme;
        else if (this.dgvTariffCostByPart.Columns[e.ColumnIndex].Name == "SchemaParamColumnPart")
          e.Value = (object) ((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).SchemeParam;
        else if (this.dgvTariffCostByPart.Columns[e.ColumnIndex].Name == "serviceColumnPart")
          e.Value = (object) ((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Service.ServiceName;
        else if (this.dgvTariffCostByPart.Columns[e.ColumnIndex].Name == "EoTariffColumnPart")
          e.Value = (object) KvrplHelper.ChangeSeparator(((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Cost_eo.ToString());
        else if (this.dgvTariffCostByPart.Columns[e.ColumnIndex].Name == "CTariffColumnPart")
          e.Value = (object) KvrplHelper.ChangeSeparator(((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Cost_c.ToString());
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void dgvTariffCostByPart_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.tariffCostPartList.Count > 0)
      {
        if (!this.isPast && this.dgvTariffCostByPart.Columns[e.ColumnIndex].Name == "dbegColumnPart" && this.closedPeriod.Value.AddMonths(1) <= ((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Dbeg && this.closedPeriod.Value.AddMonths(1) > Convert.ToDateTime(e.Value))
        {
          int num = (int) MessageBox.Show("Попытка изменить дату начала на дату, принадлежащую закрытому периоду!", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        if (this.dgvTariffCostByPart.Columns[e.ColumnIndex].Name == "dendColumnPart" && ((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Dbeg > Convert.ToDateTime(e.Value))
        {
          int num = (int) MessageBox.Show("Дата окончания не может быть меньше даты начала!", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        int num1;
        if (e.RowIndex < 0 || this.closedPeriod.HasValue)
        {
          if (this.closedPeriod.Value.AddMonths(1) <= ((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Dbeg || this.isPast)
          {
            if (this.isPast)
            {
              DateTime? periodName = ((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Period.PeriodName;
              if (periodName.HasValue)
              {
                DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
                periodName = ((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Period.PeriodName;
                if ((periodName.HasValue ? (dateTime <= periodName.GetValueOrDefault() ? 1 : 0) : 0) != 0)
                  goto label_11;
              }
            }
            else
              goto label_11;
          }
          num1 = this.dgvTariffCostByPart.Columns[e.ColumnIndex].Name == "dendColumnPart" ? 1 : 0;
          goto label_12;
        }
label_11:
        num1 = 1;
label_12:
        if (num1 != 0)
        {
          try
          {
            if (this.dgvTariffCostByPart.Columns[e.ColumnIndex].Name == "dbegColumnPart")
              ((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Dbeg = Convert.ToDateTime(e.Value);
            else if (this.dgvTariffCostByPart.Columns[e.ColumnIndex].Name == "dendColumnPart" && e.Value != null)
              ((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Dend = Convert.ToDateTime(e.Value);
            else if (this.dgvTariffCostByPart.Columns[e.ColumnIndex].Name == "CostColumnPart")
            {
              if (e.Value == null || e.Value.ToString() == "")
                ((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Cost = new double?(0.0);
              else
                ((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Cost = new double?(Convert.ToDouble(KvrplHelper.ChangeSeparator(e.Value.ToString())));
              double num2 = 0.0;
              foreach (cmpTariffCost tariffCostPart in (IEnumerable) this.tariffCostPartList)
              {
                double? cost = tariffCostPart.Cost;
                if (cost.HasValue)
                {
                  double num3 = num2;
                  cost = tariffCostPart.Cost;
                  double num4 = cost.Value;
                  num2 = num3 + num4;
                }
              }
              this.ucTariffCost1.dgvBase.CurrentRow.Cells["Cost"].Value = (object) num2;
            }
            else if (this.dgvTariffCostByPart.Columns[e.ColumnIndex].Name == "SchemaColumnPart")
              ((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Scheme = new short?(Convert.ToInt16(e.Value));
            else if (this.dgvTariffCostByPart.Columns[e.ColumnIndex].Name == "SchemaParamColumnPart")
              ((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).SchemeParam = new short?(Convert.ToInt16(e.Value));
            else if (this.dgvTariffCostByPart.Columns[e.ColumnIndex].Name == "EoTariffColumnPart")
            {
              if (e.Value == null || e.Value == (object) "")
                ((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Cost_eo = new double?(0.0);
              else
                ((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Cost_eo = new double?(Convert.ToDouble(KvrplHelper.ChangeSeparator(e.Value.ToString())));
              double num2 = 0.0;
              foreach (cmpTariffCost tariffCostPart in (IEnumerable) this.tariffCostPartList)
              {
                double? costEo = tariffCostPart.Cost_eo;
                if (costEo.HasValue)
                {
                  double num3 = num2;
                  costEo = tariffCostPart.Cost_eo;
                  double num4 = costEo.Value;
                  num2 = num3 + num4;
                }
              }
              this.ucTariffCost1.dgvBase.CurrentRow.Cells["Cost_eo"].Value = (object) num2;
            }
            else if (this.dgvTariffCostByPart.Columns[e.ColumnIndex].Name == "CTariffColumnPart")
            {
              if (e.Value == null || e.Value == (object) "")
                ((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Cost_c = new double?(0.0);
              else
                ((cmpTariffCost) this.tariffCostPartList[e.RowIndex]).Cost_c = new double?(Convert.ToDouble(KvrplHelper.ChangeSeparator(e.Value.ToString())));
              double num2 = 0.0;
              foreach (cmpTariffCost tariffCostPart in (IEnumerable) this.tariffCostPartList)
              {
                double? costC = tariffCostPart.Cost_c;
                if (costC.HasValue)
                {
                  double num3 = num2;
                  costC = tariffCostPart.Cost_c;
                  double num4 = costC.Value;
                  num2 = num3 + num4;
                }
              }
              this.ucTariffCost1.dgvBase.CurrentRow.Cells["Cost_c"].Value = (object) num2;
            }
          }
          catch (Exception ex)
          {
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
        }
        else
        {
          int num5 = (int) MessageBox.Show("Попытка внести изменения в закрытом периоде!", "Внимание!", MessageBoxButtons.OK);
        }
      }
      this.ucTariffCost1.TariffCostPartList = this.tariffCostPartList;
    }

    private void tvService_AfterSelect(object sender, TreeViewEventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      if ((Service) e.Node.Tag != null)
      {
        short? root = ((Service) e.Node.Tag).Root;
        int? nullable = root.HasValue ? new int?((int) root.GetValueOrDefault()) : new int?();
        int num = 0;
        if (nullable.GetValueOrDefault() == num && nullable.HasValue)
        {
          this.CurService = (Service) e.Node.Tag;
          this.curSrvSost = (Service) null;
        }
        else
        {
          this.CurService = (Service) e.Node.Parent.Tag;
          this.curSrvSost = (Service) e.Node.Tag;
        }
      }
      this.Cursor = Cursors.Default;
    }

    private void tsbAddService_Click(object sender, EventArgs e)
    {
      Service service1 = new Service(Convert.ToInt16((int) Convert.ToInt16(this.session.CreateQuery("select max(srv.ServiceId) from Service srv where root=0 and ServiceId>0").UniqueResult()) + 1), "Новая услуга");
      TreeNode node = new TreeNode();
      node.Text = service1.ServiceName;
      node.Tag = (object) service1;
      object obj = this.session.CreateQuery(string.Format("select max(srv.ServiceId) from Service srv where root={0} and ServiceId>0", (object) service1.ServiceId)).UniqueResult();
      Service service2 = new Service((int) Convert.ToInt16(obj) <= 0 ? Convert.ToInt16(100 * (int) service1.ServiceId + 1) : Convert.ToInt16((int) Convert.ToInt16(obj) + 1), "Новая составляющая", new short?(service1.ServiceId));
      node.Nodes.Add(new TreeNode()
      {
        Text = service2.ServiceName,
        Tag = (object) service2
      });
      this.tvService.Nodes.Add(node);
      this.tvService.SelectedNode.Expand();
      this.newService = service1;
      this.newSrvSost = service2;
      this.tsbEditServ.Enabled = true;
      node.BeginEdit();
    }

    private void tvService_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
    {
      if (e.Label != null)
        ((Service) e.Node.Tag).ServiceName = e.Label;
      this.tvService.SelectedNode = e.Node;
      if (this.curSrvSost != null)
      {
        this.curSrvSost.Uname = Options.Login;
        this.curSrvSost.Dedit = DateTime.Now;
      }
      else
      {
        if (!this._updAdmId.ContainsKey(this.curService.ServiceId))
          this._updAdmId.Add(this.curService.ServiceId, this.curService);
        this.curService.Uname = Options.Login;
        this.curService.Dedit = DateTime.Now;
      }
    }

    private void tsbEditServ_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      this.isEdit = true;
      string str = "";
      if (this.tvService.SelectedNode == null)
        this.tvService.SelectedNode = this.tvService.Nodes[0];
      this.tvService.SelectedNode.EndEdit(false);
      if (this.newService != null)
      {
        this.newService.Uname = Options.Login;
        this.newService.Dedit = DateTime.Now;
        this.newSrvSost.Uname = Options.Login;
        this.newSrvSost.Dedit = DateTime.Now;
        this.session.Save((object) this.newService);
        this.session.Save((object) this.newSrvSost);
        this.serviceList.Add((object) this.newService);
        this.newService.ChildService.Add((object) this.newSrvSost);
        str = "insert into DBA.dcService(Service_id,service_name,root,Uname,Dedit) values(" + (object) this.newService.ServiceId + ",'" + this.newService.ServiceName + "'," + (object) this.newService.Root.Value + ",'" + this.newService.Uname + "','" + (object) this.newService.Dedit + "')";
        this.tvService.SelectedNode.Text = this.newService.ServiceId.ToString() + "  " + this.tvService.SelectedNode.Text;
        this.newService = (Service) null;
        this.newSrvSost = (Service) null;
        this.isEdit = false;
      }
      else if (this.newSrvSost != null)
      {
        this.newSrvSost.Uname = Options.Login;
        this.newSrvSost.Dedit = DateTime.Now;
        this.curService.ChildService.Add((object) this.newSrvSost);
        this.session.CreateSQLQuery("insert into DBA.dcService(Service_id,service_name,root,Uname,Dedit) values(" + (object) this.newSrvSost.ServiceId + ",'" + this.newSrvSost.ServiceName + "'," + (object) this.newSrvSost.Root.Value + ",'" + this.newSrvSost.Uname + "','" + KvrplHelper.DateToBaseFormat(this.newSrvSost.Dedit) + "')").ExecuteUpdate();
        this.newSrvSost = (Service) null;
        this.isEdit = false;
      }
      else
      {
        Service service1 = new Service();
        Service service2 = new Service();
        Service service3;
        if (this.tvService.SelectedNode.Parent == null)
        {
          service3 = (Service) this.serviceList[this.tvService.SelectedNode.Index];
        }
        else
        {
          service3 = (Service) this.serviceList[this.tvService.SelectedNode.Parent.Index];
          foreach (Service service4 in (IEnumerable) service3.ChildService)
            this.session.Update((object) service4);
        }
        this.session.Update((object) service3);
      }
      this.session.Flush();
      this.LoadCb();
      this.tsbEditServ.Enabled = false;
      this.tsbAddComponent.Enabled = true;
      this.tsbAddService.Enabled = true;
      this.tsbDelete.Enabled = true;
      this._updAdmId.Clear();
    }

    private void tsbAddComponent_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      if (this.tvService.SelectedNode != null)
      {
        short? root = ((Service) this.tvService.SelectedNode.Tag).Root;
        int? nullable = root.HasValue ? new int?((int) root.GetValueOrDefault()) : new int?();
        int num1 = 0;
        Service service1 = nullable.GetValueOrDefault() == num1 && nullable.HasValue ? (Service) this.tvService.SelectedNode.Tag : (Service) this.tvService.SelectedNode.Parent.Tag;
        object obj = this.session.CreateQuery(string.Format("select max(srv.ServiceId) from Service srv where root={0} and ServiceId>0", (object) service1.ServiceId)).UniqueResult();
        Service service2 = new Service((int) Convert.ToInt16(obj) <= 0 ? Convert.ToInt16(100 * (int) service1.ServiceId + 1) : Convert.ToInt16((int) Convert.ToInt16(obj) + 1), "Новая составляющая", new short?(service1.ServiceId));
        TreeNode node = new TreeNode(service2.ServiceName);
        node.Tag = (object) service2;
        root = ((Service) this.tvService.SelectedNode.Tag).Root;
        nullable = root.HasValue ? new int?((int) root.GetValueOrDefault()) : new int?();
        int num2 = 0;
        if (nullable.GetValueOrDefault() != num2 || !nullable.HasValue)
        {
          this.tvService.SelectedNode.Parent.Nodes.Add(node);
          this.tvService.SelectedNode.Parent.Expand();
        }
        else
        {
          this.tvService.SelectedNode.Nodes.Add(node);
          this.tvService.SelectedNode.Expand();
        }
        this.newSrvSost = service2;
        this.tsbEditServ.Enabled = true;
        node.BeginEdit();
      }
      else
      {
        int num = (int) MessageBox.Show("Для создания составляющей выберите услугу!", "Внимание!", MessageBoxButtons.OK);
      }
    }

    private void tsbDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true) || MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      using (ITransaction transaction = this.session.BeginTransaction())
      {
        try
        {
          string str = "";
          Service service = this.session.Get<Service>((object) ((Service) this.tvService.SelectedNode.Tag).ServiceId);
          short? root = ((Service) this.tvService.SelectedNode.Tag).Root;
          int? nullable = root.HasValue ? new int?((int) root.GetValueOrDefault()) : new int?();
          int num = 0;
          if (nullable.GetValueOrDefault() != num || !nullable.HasValue)
            this.session.Get<Service>((object) ((Service) this.tvService.SelectedNode.Parent.Tag).ServiceId).ChildService.Remove((object) service);
          else
            str = "delete from DBA.dcService where Service_id=" + (object) service.ServiceId;
          if (service != null)
            this.session.Delete((object) service);
          transaction.Commit();
          this.serviceList.Remove((object) (Service) this.tvService.SelectedNode.Tag);
          this.tvService.SelectedNode.Remove();
          this.LoadCb();
          this.session.Flush();
        }
        catch (Exception ex)
        {
          transaction.Rollback();
          int num = (int) MessageBox.Show("Невозможно удалить запись!", "Внимание!", MessageBoxButtons.OK);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
    }

    private void tvService_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
    {
      this.tsbEditServ.Enabled = true;
      this.tsbAddComponent.Enabled = false;
      this.tsbAddService.Enabled = false;
      this.tsbDelete.Enabled = false;
    }

    private void nServ_ValueChanged(object sender, EventArgs e)
    {
      Service service = (Service) null;
      try
      {
        service = this.session.Get<Service>((object) Convert.ToInt16(this.nServ.Value));
      }
      catch
      {
      }
      if (service != null)
      {
        this.cbServ.SelectedItem = (object) service;
      }
      else
      {
        int num = (int) MessageBox.Show("Услуги с номером " + this.nServ.Value.ToString() + " в списке услуг нет!");
        this.nServ.Value = (Decimal) this.curService.ServiceId;
      }
    }

    private void nNormServ_ValueChanged(object sender, EventArgs e)
    {
      Service service = (Service) null;
      try
      {
        service = this.session.Get<Service>((object) Convert.ToInt16(this.nNormServ.Value));
      }
      catch
      {
      }
      if (service != null)
      {
        this.cbServNorm.SelectedItem = (object) service;
      }
      else
      {
        int num = (int) MessageBox.Show("Услуги с номером " + this.nNormServ.Value.ToString() + " в списке услуг нет!");
        this.nNormServ.Value = (Decimal) this.curService.ServiceId;
      }
    }

    private void dgvTariffCostByPart_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
    }

    private void cbArchiv_CheckedChanged(object sender, EventArgs e)
    {
      this.ucTariffCost1.IsArchive = this.cbArchiv.Checked;
      this.ucTariffCost1.LoadData();
    }

    private void cbNormArchive_CheckedChanged(object sender, EventArgs e)
    {
      this.ucNormCost1.IsArchive = this.cbNormArchive.Checked;
      this.ucNormCost1.LoadData();
    }

    private void uсVariant1_Load(object sender, EventArgs e)
    {
    }

    private void dgvTariffCostByPart_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.ucTariffCost1.dgvBase_CellBeginEdit((object) null, (DataGridViewCellCancelEventArgs) null);
    }

    private void dgvTariffCostByPart_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettings.FindByName(e.Column.Name) < 0)
        return;
      this.MySettings.Columns[this.MySettings.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettings.Save();
    }

    private void ucTariffCost1_Load(object sender, EventArgs e)
    {
    }

    private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
    }

    private void tsmCopy_Click(object sender, EventArgs e)
    {
    }

    private void tmr_Tick(object sender, EventArgs e)
    {
      if (this.pnlTariff.Visible)
      {
        if (this.isPast)
        {
          if (this.lblPast.ForeColor == Color.DarkOrange)
            this.lblPast.ForeColor = this.BackColor;
          else
            this.lblPast.ForeColor = Color.DarkOrange;
        }
        else
          this.lblPast.ForeColor = this.BackColor;
      }
      if (!this.pnlNorm.Visible)
        return;
      if (this.isPastNorm)
      {
        if (this.lblPastNorm.ForeColor == Color.DarkOrange)
          this.lblPastNorm.ForeColor = this.BackColor;
        else
          this.lblPastNorm.ForeColor = Color.DarkOrange;
      }
      else
        this.lblPastNorm.ForeColor = this.BackColor;
    }

    private void dgvTariffCostByPart_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      short id = 0;
      if (e.ColumnIndex <= 0 || e.RowIndex < 0 || !(this.dgvTariffCostByPart.Columns[e.ColumnIndex].Name == "SchemaColumnPart"))
        return;
      if (this.dgvTariffCostByPart.CurrentRow.Cells["SchemaColumnPart"].Value != null)
        id = Convert.ToInt16(this.dgvTariffCostByPart.CurrentRow.Cells["SchemaColumnPart"].Value);
      FrmScheme frmScheme = new FrmScheme((short) 5, id);
      if (frmScheme.ShowDialog() == DialogResult.OK)
        id = frmScheme.CurrentId();
      this.dgvTariffCostByPart.CurrentRow.Cells["SchemaColumnPart"].Value = (object) id;
      frmScheme.Dispose();
      this.ucTariffCost1.tsbAdd.Enabled = false;
      this.ucTariffCost1.tsbApplay.Enabled = true;
      this.ucTariffCost1.tsbCancel.Enabled = true;
      this.ucTariffCost1.tsbDelete.Enabled = false;
    }

    private void cmbManager_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (this.cmbManager.SelectedItem == null)
        return;
      this.uсVariant1.Manager = (BaseOrg) this.cmbManager.SelectedItem;
      this.ucNorm1.Manager = (BaseOrg) this.cmbManager.SelectedItem;
      if (this.uсVariant1.Manager.BaseOrgId != ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId)
        this.cmbManager.SelectedItem = (object) this.session.Get<Company>((object) this.ucTariffCost1.Company_id);
    }

    private void MenuForTariffs_Opening(object sender, CancelEventArgs e)
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
      this.components = (IContainer) new Container();
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle6 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle7 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle8 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle9 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle10 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle11 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle12 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle13 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle14 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle15 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle16 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle17 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle18 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle19 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle20 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle21 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle22 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle23 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle24 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle25 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle26 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle27 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle28 = new DataGridViewCellStyle();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmTariffs));
      this.pnlService = new Panel();
      this.splitContainerService = new SplitContainer();
      this.splitContainer3 = new SplitContainer();
      this.tvService = new TreeView();
      this.toolStripService = new ToolStrip();
      this.tsbAddService = new ToolStripButton();
      this.tsbAddComponent = new ToolStripButton();
      this.tsbEditServ = new ToolStripButton();
      this.tsbDelete = new ToolStripButton();
      this.splitContainer2 = new SplitContainer();
      this.uсVariant1 = new UСVariant();
      this.pnManager = new Panel();
      this.cmbManager = new ComboBox();
      this.lblManager = new Label();
      this.ucNorm1 = new UCNorm();
      this.dsMain = new DataSet();
      this.dtUnitMeasuring = new DataTable();
      this.dcUnitMeasuring_id = new DataColumn();
      this.dcUnitMeasuring_name = new DataColumn();
      this.dtBaseTariff = new DataTable();
      this.dcBaseTariff = new DataColumn();
      this.dcBaseTarif_name = new DataColumn();
      this.dtCounter = new DataTable();
      this.dataColumn1 = new DataColumn();
      this.dataColumn2 = new DataColumn();
      this.dtBaseTariff2 = new DataTable();
      this.dataColumn3 = new DataColumn();
      this.dataColumn4 = new DataColumn();
      this.toolStrip1 = new ToolStrip();
      this.tsbService = new ToolStripButton();
      this.tsbTariffs = new ToolStripButton();
      this.tsbNorm = new ToolStripButton();
      this.tsbExit = new ToolStripButton();
      this.pnlTariff = new Panel();
      this.splitContainer1 = new SplitContainer();
      this.ucTariffCost1 = new UCTariffCost();
      this.groupBox1 = new GroupBox();
      this.mpCurrentPeriod = new MonthPicker();
      this.cbArchiv = new CheckBox();
      this.nServ = new NumericUpDown();
      this.lblPast = new Label();
      this.btnLast = new Button();
      this.lblCompany = new Label();
      this.cbCompany = new ComboBox();
      this.lblService = new Label();
      this.cbServ = new ComboBox();
      this.dgvTariffCostByPart = new DataGridView();
      this.serviceColumnPart = new DataGridViewTextBoxColumn();
      this.dbegColumnPart = new DataGridViewTextBoxColumn();
      this.dendColumnPart = new DataGridViewTextBoxColumn();
      this.SchemaColumnPart = new DataGridViewButtonColumn();
      this.CostColumnPart = new DataGridViewTextBoxColumn();
      this.EoTariffColumnPart = new DataGridViewTextBoxColumn();
      this.CTariffColumnPart = new DataGridViewTextBoxColumn();
      this.MenuForTariffs = new ContextMenuStrip(this.components);
      this.tsmCopy = new ToolStripMenuItem();
      this.tsmCopyInPast = new ToolStripMenuItem();
      this.pnlNorm = new Panel();
      this.ucNormCost1 = new UCNormCost();
      this.groupBox2 = new GroupBox();
      this.mpCurrentPeriodNorm = new MonthPicker();
      this.cbNormArchive = new CheckBox();
      this.nNormServ = new NumericUpDown();
      this.lblPastNorm = new Label();
      this.btnPastNorm = new Button();
      this.label1 = new Label();
      this.cbCompanyNorm = new ComboBox();
      this.label3 = new Label();
      this.cbServNorm = new ComboBox();
      this.hp = new HelpProvider();
      this.tmr = new Timer(this.components);
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
      this.calendarColumn1 = new CalendarColumn();
      this.calendarColumn2 = new CalendarColumn();
      this.dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn10 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn11 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn12 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn13 = new DataGridViewTextBoxColumn();
      this.calendarColumn3 = new CalendarColumn();
      this.calendarColumn4 = new CalendarColumn();
      this.dataGridViewTextBoxColumn14 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn15 = new DataGridViewTextBoxColumn();
      this.calendarColumn5 = new CalendarColumn();
      this.calendarColumn6 = new CalendarColumn();
      this.dataGridViewTextBoxColumn16 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn17 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn18 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn19 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn20 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn21 = new DataGridViewTextBoxColumn();
      this.pnlService.SuspendLayout();
      this.splitContainerService.Panel1.SuspendLayout();
      this.splitContainerService.Panel2.SuspendLayout();
      this.splitContainerService.SuspendLayout();
      this.splitContainer3.Panel1.SuspendLayout();
      this.splitContainer3.SuspendLayout();
      this.toolStripService.SuspendLayout();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      this.pnManager.SuspendLayout();
      this.dsMain.BeginInit();
      this.dtUnitMeasuring.BeginInit();
      this.dtBaseTariff.BeginInit();
      this.dtCounter.BeginInit();
      this.dtBaseTariff2.BeginInit();
      this.toolStrip1.SuspendLayout();
      this.pnlTariff.SuspendLayout();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.nServ.BeginInit();
      ((ISupportInitialize) this.dgvTariffCostByPart).BeginInit();
      this.MenuForTariffs.SuspendLayout();
      this.pnlNorm.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.nNormServ.BeginInit();
      this.SuspendLayout();
      this.pnlService.Controls.Add((Control) this.splitContainerService);
      this.pnlService.Dock = DockStyle.Fill;
      this.pnlService.Location = new Point(86, 0);
      this.pnlService.Margin = new Padding(4);
      this.pnlService.Name = "pnlService";
      this.pnlService.Size = new Size(938, 513);
      this.pnlService.TabIndex = 0;
      this.splitContainerService.Dock = DockStyle.Fill;
      this.splitContainerService.Location = new Point(0, 0);
      this.splitContainerService.Margin = new Padding(4);
      this.splitContainerService.Name = "splitContainerService";
      this.splitContainerService.Panel1.AutoScroll = true;
      this.splitContainerService.Panel1.Controls.Add((Control) this.splitContainer3);
      this.splitContainerService.Panel1.Controls.Add((Control) this.toolStripService);
      this.splitContainerService.Panel2.Controls.Add((Control) this.splitContainer2);
      this.splitContainerService.Size = new Size(938, 513);
      this.splitContainerService.SplitterDistance = 312;
      this.splitContainerService.SplitterWidth = 5;
      this.splitContainerService.TabIndex = 0;
      this.splitContainer3.Dock = DockStyle.Fill;
      this.splitContainer3.Location = new Point(0, 48);
      this.splitContainer3.Margin = new Padding(4);
      this.splitContainer3.Name = "splitContainer3";
      this.splitContainer3.Orientation = Orientation.Horizontal;
      this.splitContainer3.Panel1.Controls.Add((Control) this.tvService);
      this.splitContainer3.Panel2.BackColor = SystemColors.AppWorkspace;
      this.splitContainer3.Panel2Collapsed = true;
      this.splitContainer3.Size = new Size(312, 465);
      this.splitContainer3.SplitterDistance = 440;
      this.splitContainer3.SplitterWidth = 5;
      this.splitContainer3.TabIndex = 2;
      this.tvService.Dock = DockStyle.Fill;
      this.tvService.FullRowSelect = true;
      this.tvService.HideSelection = false;
      this.tvService.LabelEdit = true;
      this.tvService.Location = new Point(0, 0);
      this.tvService.Margin = new Padding(4);
      this.tvService.Name = "tvService";
      this.tvService.Size = new Size(312, 465);
      this.tvService.TabIndex = 1;
      this.tvService.BeforeLabelEdit += new NodeLabelEditEventHandler(this.tvService_BeforeLabelEdit);
      this.tvService.AfterLabelEdit += new NodeLabelEditEventHandler(this.tvService_AfterLabelEdit);
      this.tvService.AfterSelect += new TreeViewEventHandler(this.tvService_AfterSelect);
      this.toolStripService.Font = new Font("Tahoma", 10f);
      this.toolStripService.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.tsbAddService,
        (ToolStripItem) this.tsbAddComponent,
        (ToolStripItem) this.tsbEditServ,
        (ToolStripItem) this.tsbDelete
      });
      this.toolStripService.LayoutStyle = ToolStripLayoutStyle.Flow;
      this.toolStripService.Location = new Point(0, 0);
      this.toolStripService.Name = "toolStripService";
      this.toolStripService.Size = new Size(312, 48);
      this.toolStripService.TabIndex = 0;
      this.toolStripService.Text = "toolStrip1";
      this.tsbAddService.Image = (Image) Resources.add_rootitem;
      this.tsbAddService.ImageTransparentColor = Color.Magenta;
      this.tsbAddService.Name = "tsbAddService";
      this.tsbAddService.Size = new Size(72, 21);
      this.tsbAddService.Text = "Услуга";
      this.tsbAddService.Click += new EventHandler(this.tsbAddService_Click);
      this.tsbAddComponent.Image = (Image) Resources.add_item;
      this.tsbAddComponent.ImageTransparentColor = Color.Magenta;
      this.tsbAddComponent.Name = "tsbAddComponent";
      this.tsbAddComponent.Size = new Size(125, 21);
      this.tsbAddComponent.Text = "Составляющая";
      this.tsbAddComponent.Click += new EventHandler(this.tsbAddComponent_Click);
      this.tsbEditServ.Enabled = false;
      this.tsbEditServ.Image = (Image) Resources.edit_item;
      this.tsbEditServ.ImageTransparentColor = Color.Magenta;
      this.tsbEditServ.Name = "tsbEditServ";
      this.tsbEditServ.Size = new Size(99, 21);
      this.tsbEditServ.Text = "Сохранить";
      this.tsbEditServ.Click += new EventHandler(this.tsbEditServ_Click);
      this.tsbDelete.Image = (Image) Resources.delete_item;
      this.tsbDelete.ImageTransparentColor = Color.Magenta;
      this.tsbDelete.Name = "tsbDelete";
      this.tsbDelete.Size = new Size(82, 21);
      this.tsbDelete.Text = "Удалить";
      this.tsbDelete.Click += new EventHandler(this.tsbDelete_Click);
      this.splitContainer2.Dock = DockStyle.Fill;
      this.splitContainer2.Location = new Point(0, 0);
      this.splitContainer2.Margin = new Padding(4);
      this.splitContainer2.Name = "splitContainer2";
      this.splitContainer2.Orientation = Orientation.Horizontal;
      this.splitContainer2.Panel1.Controls.Add((Control) this.uсVariant1);
      this.splitContainer2.Panel1.Controls.Add((Control) this.pnManager);
      this.splitContainer2.Panel2.Controls.Add((Control) this.ucNorm1);
      this.splitContainer2.Size = new Size(621, 513);
      this.splitContainer2.SplitterDistance = 268;
      this.splitContainer2.SplitterWidth = 5;
      this.splitContainer2.TabIndex = 3;
      this.uсVariant1.Dock = DockStyle.Fill;
      this.uсVariant1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.uсVariant1.Location = new Point(0, 48);
      this.uсVariant1.Manager = (BaseOrg) null;
      this.uсVariant1.Margin = new Padding(4);
      this.uсVariant1.Name = "uсVariant1";
      this.uсVariant1.Size = new Size(621, 220);
      this.uсVariant1.TabIndex = 1;
      this.uсVariant1.ObjectsListChanged += new EventHandler(this.uсVariant1_ObjectsListChanged);
      this.pnManager.Controls.Add((Control) this.cmbManager);
      this.pnManager.Controls.Add((Control) this.lblManager);
      this.pnManager.Dock = DockStyle.Top;
      this.pnManager.Location = new Point(0, 0);
      this.pnManager.Name = "pnManager";
      this.pnManager.Size = new Size(621, 48);
      this.pnManager.TabIndex = 0;
      this.cmbManager.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbManager.FormattingEnabled = true;
      this.cmbManager.Location = new Point(181, 12);
      this.cmbManager.Name = "cmbManager";
      this.cmbManager.Size = new Size(428, 24);
      this.cmbManager.TabIndex = 1;
      this.cmbManager.SelectionChangeCommitted += new EventHandler(this.cmbManager_SelectionChangeCommitted);
      this.lblManager.AutoSize = true;
      this.lblManager.Location = new Point(6, 15);
      this.lblManager.Name = "lblManager";
      this.lblManager.Size = new Size(169, 17);
      this.lblManager.TabIndex = 0;
      this.lblManager.Text = "Управляющая компания";
      this.ucNorm1.Dock = DockStyle.Fill;
      this.ucNorm1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.ucNorm1.Location = new Point(0, 0);
      this.ucNorm1.Manager = (BaseOrg) null;
      this.ucNorm1.Margin = new Padding(4);
      this.ucNorm1.Name = "ucNorm1";
      this.ucNorm1.Size = new Size(621, 240);
      this.ucNorm1.TabIndex = 0;
      this.ucNorm1.ObjectsListChanged += new EventHandler(this.ucNorm1_ObjectsListChanged);
      this.dsMain.DataSetName = "NewDataSet";
      this.dsMain.Tables.AddRange(new DataTable[4]
      {
        this.dtUnitMeasuring,
        this.dtBaseTariff,
        this.dtCounter,
        this.dtBaseTariff2
      });
      this.dtUnitMeasuring.Columns.AddRange(new DataColumn[2]
      {
        this.dcUnitMeasuring_id,
        this.dcUnitMeasuring_name
      });
      this.dtUnitMeasuring.TableName = "Table1";
      this.dcUnitMeasuring_id.ColumnName = "UnitMeasuring_id";
      this.dcUnitMeasuring_id.DataType = typeof (int);
      this.dcUnitMeasuring_name.ColumnName = "UnitMeasuring_name";
      this.dtBaseTariff.Columns.AddRange(new DataColumn[2]
      {
        this.dcBaseTariff,
        this.dcBaseTarif_name
      });
      this.dtBaseTariff.TableName = "Table2";
      this.dcBaseTariff.ColumnName = "BaseTariff_id";
      this.dcBaseTariff.DataType = typeof (int);
      this.dcBaseTarif_name.ColumnName = "BaseTariff_name";
      this.dtCounter.Columns.AddRange(new DataColumn[2]
      {
        this.dataColumn1,
        this.dataColumn2
      });
      this.dtCounter.TableName = "Table3";
      this.dataColumn1.ColumnName = "Id";
      this.dataColumn1.DataType = typeof (int);
      this.dataColumn2.ColumnName = "Name";
      this.dtBaseTariff2.Columns.AddRange(new DataColumn[2]
      {
        this.dataColumn3,
        this.dataColumn4
      });
      this.dtBaseTariff2.TableName = "Table4";
      this.dataColumn3.Caption = "BaseTariffMSP_id";
      this.dataColumn3.ColumnName = "BaseTariffMSP_id";
      this.dataColumn3.DataType = typeof (int);
      this.dataColumn4.ColumnName = "BaseTariff_name";
      this.toolStrip1.Dock = DockStyle.Left;
      this.toolStrip1.Font = new Font("Tahoma", 10f);
      this.toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
      this.toolStrip1.ImageScalingSize = new Size(48, 48);
      this.toolStrip1.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.tsbService,
        (ToolStripItem) this.tsbTariffs,
        (ToolStripItem) this.tsbNorm,
        (ToolStripItem) this.tsbExit
      });
      this.toolStrip1.Location = new Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Padding = new Padding(0, 12, 0, 0);
      this.toolStrip1.Size = new Size(86, 513);
      this.toolStrip1.TabIndex = 1;
      this.toolStrip1.Text = "toolStrip1";
      this.toolStrip1.ItemClicked += new ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
      this.tsbService.BackColor = SystemColors.Control;
      this.tsbService.Checked = true;
      this.tsbService.CheckOnClick = true;
      this.tsbService.CheckState = CheckState.Checked;
      this.tsbService.Image = (Image) Resources.allClasses;
      this.tsbService.ImageAlign = ContentAlignment.TopCenter;
      this.tsbService.ImageTransparentColor = Color.Magenta;
      this.tsbService.Name = "tsbService";
      this.tsbService.Size = new Size(85, 69);
      this.tsbService.Text = "Словари";
      this.tsbService.TextAlign = ContentAlignment.BottomCenter;
      this.tsbService.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tsbService.Click += new EventHandler(this.tsbService_Click);
      this.tsbTariffs.CheckOnClick = true;
      this.tsbTariffs.Image = (Image) Resources.money;
      this.tsbTariffs.ImageTransparentColor = Color.Magenta;
      this.tsbTariffs.Name = "tsbTariffs";
      this.tsbTariffs.Size = new Size(85, 69);
      this.tsbTariffs.Text = "Тарифы";
      this.tsbTariffs.TextAlign = ContentAlignment.BottomCenter;
      this.tsbTariffs.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tsbTariffs.Click += new EventHandler(this.tsbTariffs_Click);
      this.tsbNorm.CheckOnClick = true;
      this.tsbNorm.Image = (Image) Resources.percent;
      this.tsbNorm.ImageTransparentColor = Color.Magenta;
      this.tsbNorm.Name = "tsbNorm";
      this.tsbNorm.Size = new Size(85, 69);
      this.tsbNorm.Text = "Нормативы";
      this.tsbNorm.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tsbNorm.Click += new EventHandler(this.tsbNorm_Click);
      this.tsbExit.Image = (Image) Resources.Exit;
      this.tsbExit.ImageTransparentColor = Color.Magenta;
      this.tsbExit.Name = "tsbExit";
      this.tsbExit.Size = new Size(85, 69);
      this.tsbExit.Text = "Выход";
      this.tsbExit.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tsbExit.Click += new EventHandler(this.tsbExit_Click);
      this.pnlTariff.Controls.Add((Control) this.splitContainer1);
      this.pnlTariff.Dock = DockStyle.Fill;
      this.pnlTariff.Location = new Point(86, 0);
      this.pnlTariff.Margin = new Padding(4);
      this.pnlTariff.Name = "pnlTariff";
      this.pnlTariff.Size = new Size(938, 513);
      this.pnlTariff.TabIndex = 2;
      this.splitContainer1.Dock = DockStyle.Fill;
      this.splitContainer1.Location = new Point(0, 0);
      this.splitContainer1.Margin = new Padding(4);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = Orientation.Horizontal;
      this.splitContainer1.Panel1.Controls.Add((Control) this.ucTariffCost1);
      this.splitContainer1.Panel1.Controls.Add((Control) this.groupBox1);
      this.splitContainer1.Panel2.Controls.Add((Control) this.dgvTariffCostByPart);
      this.splitContainer1.Size = new Size(938, 513);
      this.splitContainer1.SplitterDistance = 368;
      this.splitContainer1.SplitterWidth = 5;
      this.splitContainer1.TabIndex = 2;
      this.ucTariffCost1.Company_id = (short) 0;
      this.ucTariffCost1.CurService = (Service) null;
      this.ucTariffCost1.Dock = DockStyle.Fill;
      this.ucTariffCost1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.ucTariffCost1.IsPast = false;
      this.ucTariffCost1.Location = new Point(0, 82);
      this.ucTariffCost1.Margin = new Padding(4);
      this.ucTariffCost1.Name = "ucTariffCost1";
      this.ucTariffCost1.Size = new Size(938, 286);
      this.ucTariffCost1.TabIndex = 1;
      this.ucTariffCost1.TariffCostPartList = (IList) null;
      this.ucTariffCost1.TariffCostPartListChanged += new EventHandler(this.ucTariffCost1_TariffCostPartListChanged);
      this.ucTariffCost1.ApplayClick += new EventHandler(this.ucTariffCost1_ApplayClick);
      this.ucTariffCost1.CancelClick += new EventHandler(this.ucTariffCost1_CancelClick);
      this.ucTariffCost1.ObjectsListChanged += new EventHandler(this.ucTariffCost1_ObjectsListChanged);
      this.ucTariffCost1.Load += new EventHandler(this.ucTariffCost1_Load);
      this.groupBox1.BackColor = SystemColors.Control;
      this.groupBox1.Controls.Add((Control) this.mpCurrentPeriod);
      this.groupBox1.Controls.Add((Control) this.cbArchiv);
      this.groupBox1.Controls.Add((Control) this.nServ);
      this.groupBox1.Controls.Add((Control) this.lblPast);
      this.groupBox1.Controls.Add((Control) this.btnLast);
      this.groupBox1.Controls.Add((Control) this.lblCompany);
      this.groupBox1.Controls.Add((Control) this.cbCompany);
      this.groupBox1.Controls.Add((Control) this.lblService);
      this.groupBox1.Controls.Add((Control) this.cbServ);
      this.groupBox1.Dock = DockStyle.Top;
      this.groupBox1.Location = new Point(0, 0);
      this.groupBox1.Margin = new Padding(4);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new Padding(4);
      this.groupBox1.Size = new Size(938, 82);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.mpCurrentPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.mpCurrentPeriod.CustomFormat = "MMMM yyyy";
      this.mpCurrentPeriod.Format = DateTimePickerFormat.Custom;
      this.mpCurrentPeriod.Location = new Point(785, 14);
      this.mpCurrentPeriod.Name = "mpCurrentPeriod";
      this.mpCurrentPeriod.OldMonth = 0;
      this.mpCurrentPeriod.ShowUpDown = true;
      this.mpCurrentPeriod.Size = new Size(140, 23);
      this.mpCurrentPeriod.TabIndex = 11;
      this.mpCurrentPeriod.ValueChanged += new EventHandler(this.mpCurrentPeriod_ValueChanged);
      this.cbArchiv.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cbArchiv.AutoSize = true;
      this.cbArchiv.Location = new Point(694, 50);
      this.cbArchiv.Name = "cbArchiv";
      this.cbArchiv.Size = new Size(65, 21);
      this.cbArchiv.TabIndex = 10;
      this.cbArchiv.Text = "Архив";
      this.cbArchiv.UseVisualStyleBackColor = true;
      this.cbArchiv.CheckedChanged += new EventHandler(this.cbArchiv_CheckedChanged);
      this.nServ.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.nServ.Location = new Point(598, 50);
      NumericUpDown nServ = this.nServ;
      int[] bits1 = new int[4];
      bits1[0] = 300;
      Decimal num1 = new Decimal(bits1);
      nServ.Maximum = num1;
      this.nServ.Name = "nServ";
      this.nServ.Size = new Size(73, 23);
      this.nServ.TabIndex = 9;
      this.nServ.ValueChanged += new EventHandler(this.nServ_ValueChanged);
      this.lblPast.AutoSize = true;
      this.lblPast.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblPast.ForeColor = Color.DarkOrange;
      this.lblPast.Location = new Point(323, 78);
      this.lblPast.Margin = new Padding(4, 0, 4, 0);
      this.lblPast.Name = "lblPast";
      this.lblPast.Size = new Size(199, 16);
      this.lblPast.TabIndex = 8;
      this.lblPast.Text = "Режим прошлого времени";
      this.btnLast.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnLast.BackColor = SystemColors.Control;
      this.btnLast.Image = (Image) Resources.time_24;
      this.btnLast.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnLast.Location = new Point(770, 45);
      this.btnLast.Margin = new Padding(4);
      this.btnLast.Name = "btnLast";
      this.btnLast.Size = new Size(155, 30);
      this.btnLast.TabIndex = 7;
      this.btnLast.Text = "Прошлое время";
      this.btnLast.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.btnLast.UseVisualStyleBackColor = true;
      this.btnLast.Click += new EventHandler(this.btnLast_Click);
      this.lblCompany.AutoSize = true;
      this.lblCompany.Location = new Point(8, 20);
      this.lblCompany.Margin = new Padding(4, 0, 4, 0);
      this.lblCompany.Name = "lblCompany";
      this.lblCompany.Size = new Size(74, 17);
      this.lblCompany.TabIndex = 6;
      this.lblCompany.Text = "Компания";
      this.cbCompany.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cbCompany.FormattingEnabled = true;
      this.cbCompany.Location = new Point(93, 15);
      this.cbCompany.Margin = new Padding(4);
      this.cbCompany.Name = "cbCompany";
      this.cbCompany.Size = new Size(672, 24);
      this.cbCompany.TabIndex = 5;
      this.cbCompany.SelectedIndexChanged += new EventHandler(this.cbCompany_SelectedIndexChanged);
      this.lblService.AutoSize = true;
      this.lblService.Location = new Point(8, 52);
      this.lblService.Margin = new Padding(4, 0, 4, 0);
      this.lblService.Name = "lblService";
      this.lblService.Size = new Size(52, 17);
      this.lblService.TabIndex = 3;
      this.lblService.Text = "Услуга";
      this.cbServ.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cbServ.FormattingEnabled = true;
      this.cbServ.Location = new Point(91, 50);
      this.cbServ.Margin = new Padding(4);
      this.cbServ.Name = "cbServ";
      this.cbServ.Size = new Size(491, 24);
      this.cbServ.TabIndex = 0;
      this.cbServ.SelectedValueChanged += new EventHandler(this.cbServ_SelectedValueChanged);
      this.dgvTariffCostByPart.AllowUserToAddRows = false;
      this.dgvTariffCostByPart.AllowUserToDeleteRows = false;
      this.dgvTariffCostByPart.BackgroundColor = Color.AliceBlue;
      this.dgvTariffCostByPart.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvTariffCostByPart.Columns.AddRange((DataGridViewColumn) this.serviceColumnPart, (DataGridViewColumn) this.dbegColumnPart, (DataGridViewColumn) this.dendColumnPart, (DataGridViewColumn) this.SchemaColumnPart, (DataGridViewColumn) this.CostColumnPart, (DataGridViewColumn) this.EoTariffColumnPart, (DataGridViewColumn) this.CTariffColumnPart);
      this.dgvTariffCostByPart.Dock = DockStyle.Fill;
      this.dgvTariffCostByPart.Location = new Point(0, 0);
      this.dgvTariffCostByPart.Margin = new Padding(4);
      this.dgvTariffCostByPart.Name = "dgvTariffCostByPart";
      this.dgvTariffCostByPart.Size = new Size(938, 140);
      this.dgvTariffCostByPart.TabIndex = 1;
      this.dgvTariffCostByPart.VirtualMode = true;
      this.dgvTariffCostByPart.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvTariffCostByPart_CellBeginEdit);
      this.dgvTariffCostByPart.CellClick += new DataGridViewCellEventHandler(this.dgvTariffCostByPart_CellClick);
      this.dgvTariffCostByPart.CellContentClick += new DataGridViewCellEventHandler(this.dgvTariffCostByPart_CellContentClick);
      this.dgvTariffCostByPart.CellValueNeeded += new DataGridViewCellValueEventHandler(this.dgvTariffCostByPart_CellValueNeeded);
      this.dgvTariffCostByPart.CellValuePushed += new DataGridViewCellValueEventHandler(this.dgvTariffCostByPart_CellValuePushed);
      this.dgvTariffCostByPart.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvTariffCostByPart_ColumnWidthChanged);
      this.serviceColumnPart.HeaderText = "Составляющая";
      this.serviceColumnPart.Name = "serviceColumnPart";
      this.serviceColumnPart.Width = 300;
      gridViewCellStyle1.Format = "d";
      gridViewCellStyle1.NullValue = (object) null;
      this.dbegColumnPart.DefaultCellStyle = gridViewCellStyle1;
      this.dbegColumnPart.HeaderText = "Дата начала действия";
      this.dbegColumnPart.Name = "dbegColumnPart";
      this.dbegColumnPart.ReadOnly = true;
      this.dbegColumnPart.Visible = false;
      this.dbegColumnPart.Width = 120;
      gridViewCellStyle2.Format = "d";
      gridViewCellStyle2.NullValue = (object) null;
      this.dendColumnPart.DefaultCellStyle = gridViewCellStyle2;
      this.dendColumnPart.HeaderText = "Дата окнчания действия";
      this.dendColumnPart.Name = "dendColumnPart";
      this.dendColumnPart.ReadOnly = true;
      this.dendColumnPart.Visible = false;
      this.dendColumnPart.Width = 120;
      this.SchemaColumnPart.HeaderText = "Схема";
      this.SchemaColumnPart.Name = "SchemaColumnPart";
      this.SchemaColumnPart.ReadOnly = true;
      this.SchemaColumnPart.Resizable = DataGridViewTriState.True;
      this.SchemaColumnPart.SortMode = DataGridViewColumnSortMode.Automatic;
      this.SchemaColumnPart.Width = 60;
      gridViewCellStyle3.Format = "N4";
      gridViewCellStyle3.NullValue = (object) null;
      this.CostColumnPart.DefaultCellStyle = gridViewCellStyle3;
      this.CostColumnPart.HeaderText = "Тариф";
      this.CostColumnPart.Name = "CostColumnPart";
      this.CostColumnPart.Width = 80;
      gridViewCellStyle4.Format = "N4";
      gridViewCellStyle4.NullValue = (object) null;
      this.EoTariffColumnPart.DefaultCellStyle = gridViewCellStyle4;
      this.EoTariffColumnPart.HeaderText = "Экономически обоснованный тариф";
      this.EoTariffColumnPart.Name = "EoTariffColumnPart";
      this.EoTariffColumnPart.Width = 120;
      gridViewCellStyle5.Format = "N4";
      gridViewCellStyle5.NullValue = (object) null;
      this.CTariffColumnPart.DefaultCellStyle = gridViewCellStyle5;
      this.CTariffColumnPart.HeaderText = "Тариф для компенсаций";
      this.CTariffColumnPart.Name = "CTariffColumnPart";
      this.MenuForTariffs.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsmCopy,
        (ToolStripItem) this.tsmCopyInPast
      });
      this.MenuForTariffs.Name = "MenuForTariffs";
      this.MenuForTariffs.Size = new Size(68, 48);
      this.MenuForTariffs.Opening += new CancelEventHandler(this.MenuForTariffs_Opening);
      this.tsmCopy.Name = "tsmCopy";
      this.tsmCopy.Size = new Size(67, 22);
      this.tsmCopy.Click += new EventHandler(this.tsmCopy_Click);
      this.tsmCopyInPast.Name = "tsmCopyInPast";
      this.tsmCopyInPast.Size = new Size(67, 22);
      this.pnlNorm.Controls.Add((Control) this.ucNormCost1);
      this.pnlNorm.Controls.Add((Control) this.groupBox2);
      this.pnlNorm.Dock = DockStyle.Fill;
      this.pnlNorm.Location = new Point(86, 0);
      this.pnlNorm.Margin = new Padding(4);
      this.pnlNorm.Name = "pnlNorm";
      this.pnlNorm.Size = new Size(938, 513);
      this.pnlNorm.TabIndex = 3;
      this.ucNormCost1.Company_id = (short) 0;
      this.ucNormCost1.CurService = (Service) null;
      this.ucNormCost1.Dock = DockStyle.Fill;
      this.ucNormCost1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.ucNormCost1.IsPast = false;
      this.ucNormCost1.Location = new Point(0, 82);
      this.ucNormCost1.Margin = new Padding(4);
      this.ucNormCost1.Name = "ucNormCost1";
      this.ucNormCost1.Size = new Size(938, 431);
      this.ucNormCost1.TabIndex = 2;
      this.ucNormCost1.ObjectsListChanged += new EventHandler(this.ucNormCost1_ObjectsListChanged);
      this.groupBox2.BackColor = SystemColors.Control;
      this.groupBox2.Controls.Add((Control) this.mpCurrentPeriodNorm);
      this.groupBox2.Controls.Add((Control) this.cbNormArchive);
      this.groupBox2.Controls.Add((Control) this.nNormServ);
      this.groupBox2.Controls.Add((Control) this.lblPastNorm);
      this.groupBox2.Controls.Add((Control) this.btnPastNorm);
      this.groupBox2.Controls.Add((Control) this.label1);
      this.groupBox2.Controls.Add((Control) this.cbCompanyNorm);
      this.groupBox2.Controls.Add((Control) this.label3);
      this.groupBox2.Controls.Add((Control) this.cbServNorm);
      this.groupBox2.Dock = DockStyle.Top;
      this.groupBox2.Location = new Point(0, 0);
      this.groupBox2.Margin = new Padding(4);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Padding = new Padding(4);
      this.groupBox2.Size = new Size(938, 82);
      this.groupBox2.TabIndex = 1;
      this.groupBox2.TabStop = false;
      this.mpCurrentPeriodNorm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.mpCurrentPeriodNorm.CustomFormat = "MMMM yyyy";
      this.mpCurrentPeriodNorm.Format = DateTimePickerFormat.Custom;
      this.mpCurrentPeriodNorm.Location = new Point(766, 17);
      this.mpCurrentPeriodNorm.Name = "mpCurrentPeriodNorm";
      this.mpCurrentPeriodNorm.OldMonth = 0;
      this.mpCurrentPeriodNorm.ShowUpDown = true;
      this.mpCurrentPeriodNorm.Size = new Size(152, 23);
      this.mpCurrentPeriodNorm.TabIndex = 12;
      this.mpCurrentPeriodNorm.ValueChanged += new EventHandler(this.mpCurrentPeriodNorm_ValueChanged);
      this.cbNormArchive.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cbNormArchive.AutoSize = true;
      this.cbNormArchive.Location = new Point(671, 50);
      this.cbNormArchive.Name = "cbNormArchive";
      this.cbNormArchive.Size = new Size(65, 21);
      this.cbNormArchive.TabIndex = 11;
      this.cbNormArchive.Text = "Архив";
      this.cbNormArchive.UseVisualStyleBackColor = true;
      this.cbNormArchive.CheckedChanged += new EventHandler(this.cbNormArchive_CheckedChanged);
      this.nNormServ.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.nNormServ.Location = new Point(578, 49);
      NumericUpDown nNormServ = this.nNormServ;
      int[] bits2 = new int[4];
      bits2[0] = 300;
      Decimal num2 = new Decimal(bits2);
      nNormServ.Maximum = num2;
      this.nNormServ.Name = "nNormServ";
      this.nNormServ.Size = new Size(68, 23);
      this.nNormServ.TabIndex = 10;
      this.nNormServ.ValueChanged += new EventHandler(this.nNormServ_ValueChanged);
      this.lblPastNorm.AutoSize = true;
      this.lblPastNorm.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblPastNorm.ForeColor = Color.DarkOrange;
      this.lblPastNorm.Location = new Point(341, 80);
      this.lblPastNorm.Margin = new Padding(4, 0, 4, 0);
      this.lblPastNorm.Name = "lblPastNorm";
      this.lblPastNorm.Size = new Size(199, 16);
      this.lblPastNorm.TabIndex = 9;
      this.lblPastNorm.Text = "Режим прошлого времени";
      this.btnPastNorm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPastNorm.BackColor = SystemColors.Control;
      this.btnPastNorm.Image = (Image) Resources.time_24;
      this.btnPastNorm.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnPastNorm.Location = new Point(766, 44);
      this.btnPastNorm.Margin = new Padding(4);
      this.btnPastNorm.Name = "btnPastNorm";
      this.btnPastNorm.Size = new Size(152, 30);
      this.btnPastNorm.TabIndex = 7;
      this.btnPastNorm.Text = "Прошлое время";
      this.btnPastNorm.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.btnPastNorm.UseVisualStyleBackColor = true;
      this.btnPastNorm.Click += new EventHandler(this.btmPastNorm_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 20);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(74, 17);
      this.label1.TabIndex = 6;
      this.label1.Text = "Компания";
      this.cbCompanyNorm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cbCompanyNorm.FormattingEnabled = true;
      this.cbCompanyNorm.Location = new Point(93, 15);
      this.cbCompanyNorm.Margin = new Padding(4);
      this.cbCompanyNorm.Name = "cbCompanyNorm";
      this.cbCompanyNorm.Size = new Size(643, 24);
      this.cbCompanyNorm.TabIndex = 5;
      this.cbCompanyNorm.SelectedIndexChanged += new EventHandler(this.cbCompanyNorm_SelectedIndexChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(8, 52);
      this.label3.Margin = new Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(52, 17);
      this.label3.TabIndex = 3;
      this.label3.Text = "Услуга";
      this.cbServNorm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cbServNorm.FormattingEnabled = true;
      this.cbServNorm.Location = new Point(93, 48);
      this.cbServNorm.Margin = new Padding(4);
      this.cbServNorm.Name = "cbServNorm";
      this.cbServNorm.Size = new Size(469, 24);
      this.cbServNorm.TabIndex = 0;
      this.cbServNorm.SelectedIndexChanged += new EventHandler(this.cbServNorm_SelectedIndexChanged);
      this.hp.HelpNamespace = "Help.chm";
      this.tmr.Interval = 1000;
      this.tmr.Tick += new EventHandler(this.tmr_Tick);
      gridViewCellStyle6.Format = "d";
      gridViewCellStyle6.NullValue = (object) null;
      this.dataGridViewTextBoxColumn1.DefaultCellStyle = gridViewCellStyle6;
      this.dataGridViewTextBoxColumn1.HeaderText = "Период";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.ReadOnly = true;
      this.dataGridViewTextBoxColumn1.Visible = false;
      this.dataGridViewTextBoxColumn1.Width = 200;
      gridViewCellStyle7.Format = "d";
      gridViewCellStyle7.NullValue = (object) null;
      this.dataGridViewTextBoxColumn2.DefaultCellStyle = gridViewCellStyle7;
      this.dataGridViewTextBoxColumn2.HeaderText = "Дата начала действия";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn2.ReadOnly = true;
      this.dataGridViewTextBoxColumn2.Visible = false;
      this.dataGridViewTextBoxColumn2.Width = 60;
      gridViewCellStyle8.Format = "d";
      gridViewCellStyle8.NullValue = (object) null;
      this.dataGridViewTextBoxColumn3.DefaultCellStyle = gridViewCellStyle8;
      this.dataGridViewTextBoxColumn3.HeaderText = "Дата окончания действия";
      this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
      this.dataGridViewTextBoxColumn3.ReadOnly = true;
      this.dataGridViewTextBoxColumn3.Visible = false;
      this.dataGridViewTextBoxColumn3.Width = 50;
      gridViewCellStyle9.Format = "N5";
      gridViewCellStyle9.NullValue = (object) null;
      this.dataGridViewTextBoxColumn4.DefaultCellStyle = gridViewCellStyle9;
      this.dataGridViewTextBoxColumn4.HeaderText = "Значение нормы для начислений";
      this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
      this.dataGridViewTextBoxColumn4.ReadOnly = true;
      this.dataGridViewTextBoxColumn4.Width = 60;
      gridViewCellStyle10.Format = "N5";
      gridViewCellStyle10.NullValue = (object) null;
      this.dataGridViewTextBoxColumn5.DefaultCellStyle = gridViewCellStyle10;
      this.dataGridViewTextBoxColumn5.HeaderText = "Значение нормы для льгот";
      this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
      this.dataGridViewTextBoxColumn5.ReadOnly = true;
      this.dataGridViewTextBoxColumn5.Visible = false;
      this.dataGridViewTextBoxColumn5.Width = 60;
      gridViewCellStyle11.Format = "d";
      gridViewCellStyle11.NullValue = (object) null;
      this.dataGridViewTextBoxColumn6.DefaultCellStyle = gridViewCellStyle11;
      this.dataGridViewTextBoxColumn6.HeaderText = "Период";
      this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
      this.dataGridViewTextBoxColumn6.ReadOnly = true;
      this.dataGridViewTextBoxColumn6.Visible = false;
      this.dataGridViewTextBoxColumn6.Width = 200;
      gridViewCellStyle12.Format = "N4";
      gridViewCellStyle12.NullValue = (object) null;
      this.dataGridViewTextBoxColumn7.DefaultCellStyle = gridViewCellStyle12;
      this.dataGridViewTextBoxColumn7.HeaderText = "цена";
      this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
      this.dataGridViewTextBoxColumn7.ReadOnly = true;
      this.dataGridViewTextBoxColumn7.Width = 60;
      gridViewCellStyle13.Format = "d";
      this.calendarColumn1.DefaultCellStyle = gridViewCellStyle13;
      this.calendarColumn1.HeaderText = "Дата начала действия";
      this.calendarColumn1.Name = "calendarColumn1";
      this.calendarColumn1.Resizable = DataGridViewTriState.True;
      this.calendarColumn1.SortMode = DataGridViewColumnSortMode.Automatic;
      this.calendarColumn1.Width = 120;
      gridViewCellStyle14.Format = "d";
      this.calendarColumn2.DefaultCellStyle = gridViewCellStyle14;
      this.calendarColumn2.HeaderText = "Окончание действия";
      this.calendarColumn2.Name = "calendarColumn2";
      this.calendarColumn2.Resizable = DataGridViewTriState.True;
      this.calendarColumn2.SortMode = DataGridViewColumnSortMode.Automatic;
      this.calendarColumn2.Width = 120;
      gridViewCellStyle15.Format = "d";
      gridViewCellStyle15.NullValue = (object) null;
      this.dataGridViewTextBoxColumn8.DefaultCellStyle = gridViewCellStyle15;
      this.dataGridViewTextBoxColumn8.HeaderText = "Схема";
      this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
      this.dataGridViewTextBoxColumn8.ReadOnly = true;
      this.dataGridViewTextBoxColumn8.Width = 50;
      gridViewCellStyle16.Format = "N4";
      gridViewCellStyle16.NullValue = (object) null;
      this.dataGridViewTextBoxColumn9.DefaultCellStyle = gridViewCellStyle16;
      this.dataGridViewTextBoxColumn9.HeaderText = "Экономически обоснованный";
      this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
      this.dataGridViewTextBoxColumn9.Width = 60;
      gridViewCellStyle17.Format = "N4";
      gridViewCellStyle17.NullValue = (object) null;
      this.dataGridViewTextBoxColumn10.DefaultCellStyle = gridViewCellStyle17;
      this.dataGridViewTextBoxColumn10.HeaderText = "Тариф для компенсаций";
      this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
      this.dataGridViewTextBoxColumn10.ReadOnly = true;
      this.dataGridViewTextBoxColumn10.Visible = false;
      this.dataGridViewTextBoxColumn10.Width = 60;
      gridViewCellStyle18.Format = "N4";
      gridViewCellStyle18.NullValue = (object) null;
      this.dataGridViewTextBoxColumn11.DefaultCellStyle = gridViewCellStyle18;
      this.dataGridViewTextBoxColumn11.HeaderText = "Составляющая";
      this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
      this.dataGridViewTextBoxColumn11.Width = 200;
      gridViewCellStyle19.Format = "d";
      gridViewCellStyle19.NullValue = (object) null;
      this.dataGridViewTextBoxColumn12.DefaultCellStyle = gridViewCellStyle19;
      this.dataGridViewTextBoxColumn12.HeaderText = "Дата начала действия";
      this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
      this.dataGridViewTextBoxColumn12.ReadOnly = true;
      this.dataGridViewTextBoxColumn12.Width = 120;
      gridViewCellStyle20.Format = "d";
      gridViewCellStyle20.NullValue = (object) null;
      this.dataGridViewTextBoxColumn13.DefaultCellStyle = gridViewCellStyle20;
      this.dataGridViewTextBoxColumn13.HeaderText = "Дата окнчания действия";
      this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
      this.dataGridViewTextBoxColumn13.ReadOnly = true;
      this.dataGridViewTextBoxColumn13.Visible = false;
      this.dataGridViewTextBoxColumn13.Width = 120;
      this.calendarColumn3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      gridViewCellStyle21.Format = "d";
      this.calendarColumn3.DefaultCellStyle = gridViewCellStyle21;
      this.calendarColumn3.HeaderText = "Дата начала действия";
      this.calendarColumn3.Name = "calendarColumn3";
      this.calendarColumn3.Resizable = DataGridViewTriState.True;
      this.calendarColumn3.SortMode = DataGridViewColumnSortMode.Automatic;
      this.calendarColumn4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      gridViewCellStyle22.Format = "d";
      this.calendarColumn4.DefaultCellStyle = gridViewCellStyle22;
      this.calendarColumn4.HeaderText = "Дата окончания действия";
      this.calendarColumn4.Name = "calendarColumn4";
      this.calendarColumn4.Resizable = DataGridViewTriState.True;
      this.calendarColumn4.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dataGridViewTextBoxColumn14.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      gridViewCellStyle23.Format = "N4";
      gridViewCellStyle23.NullValue = (object) null;
      this.dataGridViewTextBoxColumn14.DefaultCellStyle = gridViewCellStyle23;
      this.dataGridViewTextBoxColumn14.HeaderText = "Тариф";
      this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
      this.dataGridViewTextBoxColumn15.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      gridViewCellStyle24.Format = "N5";
      gridViewCellStyle24.NullValue = (object) null;
      this.dataGridViewTextBoxColumn15.DefaultCellStyle = gridViewCellStyle24;
      this.dataGridViewTextBoxColumn15.HeaderText = "Схема";
      this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
      this.dataGridViewTextBoxColumn15.ReadOnly = true;
      gridViewCellStyle25.Format = "d";
      this.calendarColumn5.DefaultCellStyle = gridViewCellStyle25;
      this.calendarColumn5.HeaderText = "Дата начала действия";
      this.calendarColumn5.Name = "calendarColumn5";
      this.calendarColumn5.Resizable = DataGridViewTriState.True;
      this.calendarColumn5.SortMode = DataGridViewColumnSortMode.Automatic;
      this.calendarColumn5.Width = 120;
      gridViewCellStyle26.Format = "d";
      this.calendarColumn6.DefaultCellStyle = gridViewCellStyle26;
      this.calendarColumn6.HeaderText = "Окончание действия";
      this.calendarColumn6.Name = "calendarColumn6";
      this.calendarColumn6.Resizable = DataGridViewTriState.True;
      this.calendarColumn6.SortMode = DataGridViewColumnSortMode.Automatic;
      this.calendarColumn6.Width = 120;
      gridViewCellStyle27.Format = "N4";
      gridViewCellStyle27.NullValue = (object) null;
      this.dataGridViewTextBoxColumn16.DefaultCellStyle = gridViewCellStyle27;
      this.dataGridViewTextBoxColumn16.HeaderText = "Экономически обоснованный тариф";
      this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
      gridViewCellStyle28.Format = "N4";
      gridViewCellStyle28.NullValue = (object) null;
      this.dataGridViewTextBoxColumn17.DefaultCellStyle = gridViewCellStyle28;
      this.dataGridViewTextBoxColumn17.HeaderText = "Тариф для компенсаций";
      this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
      this.dataGridViewTextBoxColumn18.HeaderText = "Наименование";
      this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
      this.dataGridViewTextBoxColumn18.Width = 250;
      this.dataGridViewTextBoxColumn19.HeaderText = "Номер";
      this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
      this.dataGridViewTextBoxColumn19.Width = 60;
      this.dataGridViewTextBoxColumn20.HeaderText = "Наименование";
      this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
      this.dataGridViewTextBoxColumn20.Width = 250;
      this.dataGridViewTextBoxColumn21.HeaderText = "Номер варианта";
      this.dataGridViewTextBoxColumn21.Name = "dataGridViewTextBoxColumn21";
      this.dataGridViewTextBoxColumn21.Width = 60;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1024, 513);
      this.Controls.Add((Control) this.pnlService);
      this.Controls.Add((Control) this.pnlNorm);
      this.Controls.Add((Control) this.pnlTariff);
      this.Controls.Add((Control) this.toolStrip1);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.hp.SetHelpKeyword((Control) this, "kv51.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmTariffs";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Услуги, тарифы, нормативы";
      this.Load += new EventHandler(this.frmTariffs_Load);
      this.pnlService.ResumeLayout(false);
      this.splitContainerService.Panel1.ResumeLayout(false);
      this.splitContainerService.Panel1.PerformLayout();
      this.splitContainerService.Panel2.ResumeLayout(false);
      this.splitContainerService.ResumeLayout(false);
      this.splitContainer3.Panel1.ResumeLayout(false);
      this.splitContainer3.ResumeLayout(false);
      this.toolStripService.ResumeLayout(false);
      this.toolStripService.PerformLayout();
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel2.ResumeLayout(false);
      this.splitContainer2.ResumeLayout(false);
      this.pnManager.ResumeLayout(false);
      this.pnManager.PerformLayout();
      this.dsMain.EndInit();
      this.dtUnitMeasuring.EndInit();
      this.dtBaseTariff.EndInit();
      this.dtCounter.EndInit();
      this.dtBaseTariff2.EndInit();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.pnlTariff.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.nServ.EndInit();
      ((ISupportInitialize) this.dgvTariffCostByPart).EndInit();
      this.MenuForTariffs.ResumeLayout(false);
      this.pnlNorm.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.nNormServ.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
