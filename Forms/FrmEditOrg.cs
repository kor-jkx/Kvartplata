// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmEditOrg
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Forms.Controls;
using Kvartplata.Properties;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmEditOrg : FrmBaseForm
  {
    private FormStateSaver fss = new FormStateSaver(FrmEditOrg.container);
    private bool create = true;
    private string innChange = "";
    private IContainer components = (IContainer) null;
    private static IContainer container;
    private BaseOrgPerson oldPerson;
    private BaseOrg org;
    private ISession session;
    private Panel pnBtn;
    private Button btnExit;
    private Button btnSave;
    private Label lblNameOrg;
    private Label lblSmallName;
    private TextBox txbNameOrg;
    private TextBox txbSmallName;
    private GroupBox gbBoss;
    private TextBox txbLastName;
    private Label lblLastName;
    private TextBox txbName;
    private Label lblName;
    private Label lblFamily;
    private TextBox txbFamily;
    private Label lblTypeWork;
    private TextBox txbTypeWork;
    private Label lblINN;
    private TextBox txbINN;
    private Label lblOKPO;
    private TextBox txbOKPO;
    private Label lblOKONH;
    private TextBox txbOKONH;
    private GroupBox gbBank;
    private TextBox txbSch;
    private Label lblSch;
    private Label lblBank;
    private ComboBox cmbBank;
    private GroupBox gbUa;
    private Label lblUFlat;
    private Label lblUKorp;
    private Label lblUHome;
    private UCRegion ucRegion;
    private TextBox txbUPost;
    private Label lblUPost;
    private TextBox txbUInd;
    private Label lblUInd;
    private TextBox txbUDop;
    private TextBox txbFax;
    private TextBox txbPhone;
    private Label lblUDop;
    private Label lblFax;
    private Label lblPhone;
    private TextBox txbUFlat;
    private TextBox txbUKorp;
    private TextBox txbUHome;
    private GroupBox gbPa;
    private TextBox txbPDop;
    private Label lblPDop;
    private TextBox txbPFlat;
    private TextBox txbPKorp;
    private TextBox txbPHome;
    private Label lblPFlat;
    private Label lblPKorp;
    private Label lblPHome;
    private UCRegion ucRegionPost;
    private TextBox txbPPost;
    private Label lblPPost;
    private TextBox txbPInd;
    private Label lblPInd;
    private ToolStrip ts;
    private ToolStripButton tsbApplay;
    private ToolStripButton tsbSupplier;
    private TextBox txbKPP;
    private Label lblKPP;
    public HelpProvider hp;
    private Button btnCopyAdr;
    private Label lblAdditional;
    private TextBox txbAdditional;
    private Button btnShowPersons;
    private GroupBox gbPersons;
    private DataGridView dgvPersons;
    private Panel pnPersons;
    private ToolStrip toolStrip1;
    private ToolStripButton tsbAddPerson;
    private ToolStripButton tsbSavePerson;
    private ToolStripButton tsbDeletePerson;
    private TextBox txbOGRN;
    private Label lblOGRN;
    private Label lblAddressPriority;
    private ComboBox cmbAddressPriority;
    private TextBox tbKbk;
    private Label label1;

    public FrmEditOrg()
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
    }

    public FrmEditOrg(BaseOrg org)
    {
      this.InitializeComponent();
      this.org = org;
      this.fss.ParentForm = (Form) this;
      this.create = false;
    }

    public FrmEditOrg(BaseOrg org, bool edit)
    {
      this.InitializeComponent();
      this.gbUa.Enabled = edit;
      this.gbBank.Enabled = edit;
      this.txbOKONH.Enabled = edit;
      this.txbOGRN.Enabled = edit;
      this.txbINN.Enabled = edit;
      this.txbKPP.Enabled = edit;
      this.txbOKPO.Enabled = edit;
      this.txbTypeWork.Enabled = edit;
      this.txbFamily.Enabled = edit;
      this.txbName.Enabled = edit;
      this.txbLastName.Enabled = edit;
      this.txbSmallName.Enabled = edit;
      this.txbNameOrg.Enabled = edit;
      this.tsbSupplier.Enabled = edit;
      this.txbAdditional.Enabled = edit;
      this.tbKbk.Enabled = edit;
      this.org = org;
      this.fss.ParentForm = (Form) this;
    }

    private void FrmEditOrg_Load(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      if (this.org != null)
      {
        this.session = Domain.CurrentSession;
        this.org = this.session.Get<BaseOrg>((object) this.org.BaseOrgId);
        this.session.Clear();
      }
      else
        this.org = new BaseOrg();
      this.txbNameOrg.DataBindings.Add(new Binding("Text", (object) this.org, "BaseOrgName", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbSmallName.DataBindings.Add(new Binding("Text", (object) this.org, "NameOrgMin", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbFamily.DataBindings.Add(new Binding("Text", (object) this.org, "BossFam", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbName.DataBindings.Add(new Binding("Text", (object) this.org, "BossName", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbLastName.DataBindings.Add(new Binding("Text", (object) this.org, "BossLastName", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbTypeWork.DataBindings.Add(new Binding("Text", (object) this.org, "TypeWork", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbINN.DataBindings.Add(new Binding("Text", (object) this.org, "INN", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbKPP.DataBindings.Add(new Binding("Text", (object) this.org, "KPP", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbOKPO.DataBindings.Add(new Binding("Text", (object) this.org, "OKPO", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbOKONH.DataBindings.Add(new Binding("Text", (object) this.org, "OKONH", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbOGRN.DataBindings.Add(new Binding("Text", (object) this.org, "OGRN", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbSch.DataBindings.Add(new Binding("Text", (object) this.org, "RSch", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbUInd.DataBindings.Add(new Binding("Text", (object) this.org, "UInd", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbUPost.DataBindings.Add(new Binding("Text", (object) this.org, "UPost", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbUHome.DataBindings.Add(new Binding("Text", (object) this.org, "UHome", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbUKorp.DataBindings.Add(new Binding("Text", (object) this.org, "UKorp", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbUFlat.DataBindings.Add(new Binding("Text", (object) this.org, "UFlat", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbPhone.DataBindings.Add(new Binding("Text", (object) this.org, "Phone", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbFax.DataBindings.Add(new Binding("Text", (object) this.org, "Fax", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbUDop.DataBindings.Add(new Binding("Text", (object) this.org, "UDop", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbAdditional.DataBindings.Add(new Binding("Text", (object) this.org, "AdditionalCode", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbPInd.DataBindings.Add(new Binding("Text", (object) this.org, "PInd", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbPPost.DataBindings.Add(new Binding("Text", (object) this.org, "PPost", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbPHome.DataBindings.Add(new Binding("Text", (object) this.org, "PHome", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbPKorp.DataBindings.Add(new Binding("Text", (object) this.org, "PKorp", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbPFlat.DataBindings.Add(new Binding("Text", (object) this.org, "PFlat", false, DataSourceUpdateMode.OnPropertyChanged));
      this.txbPDop.DataBindings.Add(new Binding("Text", (object) this.org, "PDop", false, DataSourceUpdateMode.OnPropertyChanged));
      this.tbKbk.DataBindings.Add(new Binding("Text", (object) this.org, "Kbk", false, DataSourceUpdateMode.OnPropertyChanged));
      this.cmbAddressPriority.SelectedIndex = this.org.AdressPriority;
      if (!this.create)
        this.innChange = this.txbINN.Text;
      IList<Bank> bankList1 = (IList<Bank>) new List<Bank>();
      IList<Bank> bankList2 = this.session.CreateCriteria(typeof (Bank)).Add((ICriterion) Restrictions.Not((ICriterion) Restrictions.Eq("BankId", (object) 0))).AddOrder(Order.Asc("BankName")).List<Bank>();
      bankList2.Insert(0, new Bank()
      {
        BankId = 0,
        BankName = ""
      });
      this.cmbBank.DataSource = (object) bankList2;
      this.cmbBank.DisplayMember = "BankName";
      this.cmbBank.ValueMember = "BankId";
      if (this.org.Bank != null)
        this.cmbBank.SelectedValue = (object) this.org.Bank.BankId;
      this.ucRegion.LoadSoato();
      if (this.org.UStreet == 0)
      {
        if (this.org.UCity == null || this.org.UCity.RegionId == 0)
        {
          this.ucRegion.Clear();
        }
        else
        {
          IList list = this.session.CreateSQLQuery(string.Format("select idregion as id3,idprin as id2,isnull((select prinnumstr from dba.di_soato_ru where numstr=id2),-1) as id1,       (select level from dba.di_soato_ru where numstr=id2) as l2,(select level from dba.di_soato_ru where numstr=id1) as l1,       isnull((if urov=3 then id3 else -1 endif),-1)  as idcity,       isnull((case urov when 1 then -1 when 2 then id3 else (if l2=2 then id2 else -1 endif) end),-1) as idraion,       isnull((if urov=1 then id3 else  (if l2=2 then id1 else id2 endif) endif),-1) as idarea from dba.di_region_all r where idregion={0}", (object) this.org.UCity.RegionId)).List();
          if (list.Count == 0)
            this.ucRegion.Clear();
          else
            this.ucRegion.Fix(Convert.ToInt32(((object[]) list[0])[7]), Convert.ToInt32(((object[]) list[0])[6]), Convert.ToInt32(((object[]) list[0])[5]), 0);
        }
      }
      else
      {
        IList list = this.session.CreateSQLQuery(string.Format("select idarea,idcity,idraion from dba.di_str_all where idstr={0}", (object) this.org.UStreet)).List();
        if (list.Count == 0)
          this.ucRegion.Clear();
        else
          this.ucRegion.Fix(Convert.ToInt32(((object[]) list[0])[0]), Convert.ToInt32(((object[]) list[0])[2]), Convert.ToInt32(((object[]) list[0])[1]), this.org.UStreet);
      }
      this.ucRegionPost.LoadSoato();
      this.PostFix();
      this.session.Clear();
      this.txbNameOrg.Focus();
      this.LoadPersons();
    }

    private void FrmEditOrg_Shown(object sender, EventArgs e)
    {
      this.txbNameOrg.Focus();
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(75, 2, Options.Company, true))
        return;
      this.session = Domain.CurrentSession;
      this.session.Clear();
      Control control = new Control();
      Control activeControl = this.ActiveControl;
      this.txbSmallName.Focus();
      this.txbNameOrg.Focus();
      activeControl.Focus();
      if (this.org.BaseOrgName == null || this.org.BaseOrgName == "" || this.org.NameOrgMin == null || this.org.NameOrgMin == "")
      {
        int num1 = (int) MessageBox.Show("Введите название и краткое название организации", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.org.INN == null || this.org.INN == "")
      {
        int num2 = (int) MessageBox.Show("Введите ИНН", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (!this.create)
        {
          if (!(this.innChange != "") || !this.innChange.Equals(this.txbINN.Text))
          {
            ICriteria criteria = this.session.CreateCriteria(typeof (BaseOrg)).Add((ICriterion) Restrictions.Eq("INN", (object) this.org.INN));
            int baseOrgId = this.org.BaseOrgId;
            if ((uint) this.org.BaseOrgId > 0U)
              criteria.Add((ICriterion) Restrictions.Not((ICriterion) Restrictions.Eq("BaseOrgId", (object) this.org.BaseOrgId)));
            if (criteria.List<BaseOrg>().Count > 0)
            {
              int num3 = (int) MessageBox.Show("Такой ИНН уже заведен!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              return;
            }
          }
        }
        else
        {
          ICriteria criteria = this.session.CreateCriteria(typeof (BaseOrg)).Add((ICriterion) Restrictions.Eq("INN", (object) this.org.INN));
          int baseOrgId = this.org.BaseOrgId;
          if ((uint) this.org.BaseOrgId > 0U)
            criteria.Add((ICriterion) Restrictions.Not((ICriterion) Restrictions.Eq("BaseOrgId", (object) this.org.BaseOrgId)));
          if (criteria.List<BaseOrg>().Count > 0)
          {
            int num3 = (int) MessageBox.Show("Такой ИНН уже заведен!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return;
          }
        }
        if (this.org.UDop == null)
          this.org.UDop = "";
        if (this.org.BossFam == null)
          this.org.BossFam = "";
        if (this.org.BossLastName == null)
          this.org.BossLastName = "";
        if (this.org.BossName == null)
          this.org.BossName = "";
        if (this.org.Phone == null)
          this.org.Phone = "";
        if (this.org.UInd == null)
          this.org.UInd = "";
        if (this.org.UPost == null)
          this.org.UPost = "";
        if (this.org.Fax == null)
          this.org.Fax = "";
        if (this.org.RSch == null)
          this.org.RSch = "";
        if (this.org.INN == null)
          this.org.INN = "";
        if (this.org.KPP == null)
          this.org.KPP = "";
        if (this.org.OKONH == null)
          this.org.OKONH = "";
        if (this.org.OKPO == null)
          this.org.OKPO = "";
        if (this.org.OGRN == null)
          this.org.OGRN = "";
        if (this.org.UHome == null)
          this.org.UHome = "";
        if (this.org.UKorp == null)
          this.org.UKorp = "";
        if (this.org.UFlat == null)
          this.org.UFlat = "";
        int ustreet = this.org.UStreet;
        if (false)
          this.org.UStreet = 0;
        if (this.org.Kbk == null)
          this.org.Kbk = "";
        this.org.Bank = this.session.Get<Bank>(this.cmbBank.SelectedValue);
        this.org.UCity = this.ucRegion.ReturnRegion();
        this.org.UStreet = this.ucRegion.ReturnStreet();
        if (this.org.PDop == null)
          this.org.PDop = "";
        if (this.org.PInd == null)
          this.org.PInd = "";
        if (this.org.PPost == null)
          this.org.PPost = "";
        if (this.org.PHome == null)
          this.org.PHome = "";
        if (this.org.PKorp == null)
          this.org.PKorp = "";
        if (this.org.PFlat == null)
          this.org.PFlat = "";
        int pstreet = this.org.PStreet;
        if (false)
          this.org.PStreet = 0;
        if (this.org.AdditionalCode == null)
          this.org.AdditionalCode = "";
        this.org.PCity = this.ucRegionPost.ReturnRegion();
        this.org.PStreet = this.ucRegionPost.ReturnStreet();
        this.org.AdressPriority = this.cmbAddressPriority.SelectedIndex;
        try
        {
          int baseOrgId = this.org.BaseOrgId;
          if ((uint) this.org.BaseOrgId > 0U)
          {
            this.session.Update((object) this.org);
          }
          else
          {
            this.org.BaseOrgId = this.session.CreateSQLQuery("Select DBA.Gen_id('Base_Org',1)").UniqueResult<int>();
            this.session.Save((object) this.org);
          }
          this.session.Flush();
        }
        catch (Exception ex)
        {
          if (ex.InnerException != null && ex.InnerException.Message.ToLower().IndexOf("primary key for table 'base_org' is not unique") != -1)
          {
            this.org.BaseOrgId = 0;
            KvrplHelper.ResetGeners("base_org", "idbaseorg");
            int num3 = (int) MessageBox.Show("Была устранена ошибка генерации уникального поля! Сохраните организацию заново!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          else
          {
            int num4 = (int) MessageBox.Show("Не удалось сохранить внесенные изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        this.session.Clear();
      }
    }

    private void tsbSupplier_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(75, 2, Options.Company, true))
        return;
      if (this.org.BaseOrgId == 0)
      {
        int num1 = (int) MessageBox.Show("Сначала сохраните организацию", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (MessageBox.Show("Вы действительно хотите объявить организацию поставщиком?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
          return;
        this.session.Clear();
        if (this.session.CreateQuery("from Postaver where IDBASEORG=:id").SetParameter<int>("id", this.org.BaseOrgId).List<Postaver>().Count > 0)
        {
          int num2 = (int) MessageBox.Show("Организация уже объявлена поставщиком", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          try
          {
            Postaver postaver = new Postaver();
            postaver.IDBASEORG = this.org.BaseOrgId;
            int num3 = this.session.CreateSQLQuery("Select DBA.Gen_id('Postaver',1)").UniqueResult<int>();
            postaver.IDPOSTAVER = num3;
            this.session.Save((object) postaver);
            this.session.Flush();
          }
          catch (Exception ex)
          {
            int num3 = (int) MessageBox.Show("Не удалось объявить организацию поставщиком", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            KvrplHelper.WriteLog(ex, (LsClient) null);
            return;
          }
          int num4 = (int) MessageBox.Show("Организация успешно добавлена в список поставщиков", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.session.Clear();
        }
      }
    }

    private void btnCopyAdr_Click(object sender, EventArgs e)
    {
      this.org.PInd = this.org.UInd;
      this.org.PPost = this.org.UPost;
      this.org.PPos = this.org.UPos;
      this.org.PCity = this.ucRegion.ReturnRegion();
      this.org.PStreet = this.ucRegion.ReturnStreet();
      this.org.PKorp = this.org.UKorp;
      this.org.PHome = this.org.UHome;
      this.org.PFlat = this.org.UFlat;
      this.txbPInd.Text = this.org.UInd;
      this.txbPPost.Text = this.org.UPost;
      this.txbPKorp.Text = this.org.UKorp;
      this.txbPHome.Text = this.org.UHome;
      this.txbPFlat.Text = this.org.UFlat;
      this.PostFix();
    }

    private void PostFix()
    {
      if (this.org.PStreet == 0)
      {
        if (this.org.PCity == null || this.org.PCity.RegionId == 0)
        {
          this.ucRegionPost.Clear();
        }
        else
        {
          IList list = this.session.CreateSQLQuery(string.Format("select idregion as id3,idprin as id2,isnull((select prinnumstr from di_soato_ru where numstr=id2),-1) as id1,       (select level from di_soato_ru where numstr=id2) as l2,(select level from di_soato_ru where numstr=id1) as l1,       isnull((if urov=3 then id3 else -1 endif),-1)  as idcity,       isnull((case urov when 1 then -1 when 2 then id3 else (if l2=2 then id2 else -1 endif) end),-1) as idraion,       isnull((if urov=1 then id3 else  (if l2=2 then id1 else id2 endif) endif),-1) as idarea from di_region_all r where idregion={0}", (object) this.org.PCity.RegionId)).List();
          if (list.Count == 0)
            this.ucRegionPost.Clear();
          else
            this.ucRegionPost.Fix(Convert.ToInt32(((object[]) list[0])[7]), Convert.ToInt32(((object[]) list[0])[6]), Convert.ToInt32(((object[]) list[0])[5]), 0);
        }
      }
      else
      {
        IList list = this.session.CreateSQLQuery(string.Format("select idarea,idcity,idraion from di_str_all where idstr={0}", (object) this.org.PStreet)).List();
        if (list.Count == 0)
          this.ucRegionPost.Clear();
        else
          this.ucRegionPost.Fix(Convert.ToInt32(((object[]) list[0])[0]), Convert.ToInt32(((object[]) list[0])[2]), Convert.ToInt32(((object[]) list[0])[1]), this.org.PStreet);
      }
    }

    private void btnShowPersons_Click(object sender, EventArgs e)
    {
      if ((uint) this.org.BaseOrgId > 0U)
      {
        FrmOrgPersons frmOrgPersons = new FrmOrgPersons(this.org);
        int num = (int) frmOrgPersons.ShowDialog();
        frmOrgPersons.Dispose();
      }
      else
      {
        int num1 = (int) MessageBox.Show("Сначала сохраните организацию!");
      }
    }

    private void LoadPersons()
    {
      this.tsbAddPerson.Enabled = true;
      this.tsbSavePerson.Enabled = false;
      this.tsbDeletePerson.Enabled = true;
      this.dgvPersons.DataSource = (object) null;
      this.dgvPersons.Columns.Clear();
      try
      {
        if ((uint) this.org.BaseOrgId > 0U)
          this.dgvPersons.DataSource = (object) this.session.CreateCriteria(typeof (BaseOrgPerson)).Add((ICriterion) Restrictions.Eq("BaseOrg", (object) this.org)).List<BaseOrgPerson>();
        this.SetViewPersons();
      }
      catch
      {
      }
    }

    private void SetViewPersons()
    {
      this.dgvPersons.Columns["Family"].HeaderText = "Фамилия";
      this.dgvPersons.Columns["Name"].HeaderText = "Имя";
      this.dgvPersons.Columns["LastName"].HeaderText = "Отчество";
      this.dgvPersons.Columns["PlaceWork"].HeaderText = "Должность";
      this.dgvPersons.Columns["Phone"].HeaderText = "Телефон";
      this.dgvPersons.Columns["ICQ"].HeaderText = "ICQ";
      this.dgvPersons.Columns["EMail"].HeaderText = "Почта";
      this.dgvPersons.Columns["Note"].HeaderText = "Примечания";
      KvrplHelper.AddComboBoxColumn(this.dgvPersons, 7, (IList) this.session.CreateCriteria<YesNo>().List<YesNo>(), "YesNoId", "YesNoName", "Печатать в отчетах", "IncRep", 2, 80);
      foreach (DataGridViewRow row in (IEnumerable) this.dgvPersons.Rows)
      {
        if (((BaseOrgPerson) row.DataBoundItem).IncRep != null)
          row.Cells["IncRep"].Value = (object) ((BaseOrgPerson) row.DataBoundItem).IncRep.YesNoId;
      }
    }

    private void btnAddPerson_Click(object sender, EventArgs e)
    {
      BaseOrgPerson baseOrgPerson = new BaseOrgPerson();
      baseOrgPerson.BaseOrg = this.org;
      baseOrgPerson.IncRep = this.session.Get<YesNo>((object) Convert.ToInt16(0));
      IList<BaseOrgPerson> baseOrgPersonList = (IList<BaseOrgPerson>) new List<BaseOrgPerson>();
      if ((uint) this.dgvPersons.Rows.Count > 0U)
        baseOrgPersonList = (IList<BaseOrgPerson>) (this.dgvPersons.DataSource as List<BaseOrgPerson>);
      baseOrgPersonList.Add(baseOrgPerson);
      this.dgvPersons.Columns.Clear();
      this.dgvPersons.DataSource = (object) null;
      this.dgvPersons.DataSource = (object) baseOrgPersonList;
      this.SetViewPersons();
      this.dgvPersons.CurrentCell = this.dgvPersons.Rows[this.dgvPersons.Rows.Count - 1].Cells[0];
    }

    private void btnSavePerson_Click(object sender, EventArgs e)
    {
      bool flag = false;
      foreach (DataGridViewRow row in (IEnumerable) this.dgvPersons.Rows)
      {
        this.dgvPersons.CurrentCell = row.Cells["Family"];
        row.Selected = true;
        if (((BaseOrgPerson) row.DataBoundItem).IsEdit && !this.SavePerson())
          flag = true;
        ((BaseOrgPerson) row.DataBoundItem).IsEdit = false;
      }
      this.tsbAddPerson.Enabled = true;
      this.tsbDeletePerson.Enabled = true;
      if (flag)
        return;
      this.LoadPersons();
    }

    private bool SavePerson()
    {
      if (this.dgvPersons.Rows.Count > 0 && this.dgvPersons.CurrentRow.Index >= 0)
      {
        this.session = Domain.CurrentSession;
        BaseOrgPerson dataBoundItem = (BaseOrgPerson) this.dgvPersons.CurrentRow.DataBoundItem;
        if (dataBoundItem.Family == null || dataBoundItem.Name == null)
        {
          int num = (int) MessageBox.Show("Введите имя и фамилию", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
        int personId = dataBoundItem.PersonId;
        bool flag;
        if ((uint) dataBoundItem.PersonId > 0U)
        {
          flag = false;
        }
        else
        {
          IList<int> intList = this.session.CreateSQLQuery("select DBA.gen_id('base_org_persons',1)").List<int>();
          dataBoundItem.PersonId = intList[0];
          flag = true;
        }
        if (this.dgvPersons.CurrentRow.Cells["IncRep"].Value != null)
          dataBoundItem.IncRep = this.session.Get<YesNo>((object) Convert.ToInt16(this.dgvPersons.CurrentRow.Cells["IncRep"].Value));
        if (dataBoundItem.EMail == null)
          dataBoundItem.EMail = "";
        if (dataBoundItem.ICQ == null)
          dataBoundItem.ICQ = "";
        if (dataBoundItem.LastName == null)
          dataBoundItem.LastName = "";
        if (dataBoundItem.Note == null)
          dataBoundItem.Note = "";
        if (dataBoundItem.Phone == null)
          dataBoundItem.Phone = "";
        if (dataBoundItem.PlaceWork == null)
          dataBoundItem.PlaceWork = "";
        try
        {
          if (flag)
            this.session.Save((object) dataBoundItem);
          else
            this.session.Update((object) dataBoundItem);
          this.session.Flush();
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Невозможно сохранить изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        this.session.Clear();
      }
      return true;
    }

    private void dgvPersons_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      ((BaseOrgPerson) this.dgvPersons.CurrentRow.DataBoundItem).IsEdit = true;
      this.tsbDeletePerson.Enabled = false;
      this.tsbSavePerson.Enabled = true;
    }

    private void dgvPersons_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      BaseOrgPerson dataBoundItem = (BaseOrgPerson) this.dgvPersons.CurrentRow.DataBoundItem;
      dataBoundItem.IsEdit = true;
      if (this.dgvPersons.CurrentCell.Value == null || !(this.dgvPersons.Columns[e.ColumnIndex].Name == "IncRep"))
        return;
      try
      {
        dataBoundItem.IncRep = this.session.Get<YesNo>(this.dgvPersons.CurrentRow.Cells["IncRep"].Value);
      }
      catch
      {
      }
    }

    private void btnDeletePerson_Click(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      if (this.dgvPersons.Rows.Count > 0 && this.dgvPersons.CurrentRow.Index >= 0)
      {
        BaseOrgPerson dataBoundItem = (BaseOrgPerson) this.dgvPersons.CurrentRow.DataBoundItem;
        if (MessageBox.Show("Вы уверены, что хотите удалить запись", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
        {
          try
          {
            this.session.Delete((object) dataBoundItem);
            this.session.Flush();
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Невозможно удалить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
          this.LoadPersons();
        }
      }
      this.session.Clear();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmEditOrg));
      this.hp = new HelpProvider();
      this.btnExit = new Button();
      this.txbOGRN = new TextBox();
      this.lblOGRN = new Label();
      this.gbPersons = new GroupBox();
      this.dgvPersons = new DataGridView();
      this.pnPersons = new Panel();
      this.toolStrip1 = new ToolStrip();
      this.tsbAddPerson = new ToolStripButton();
      this.tsbSavePerson = new ToolStripButton();
      this.tsbDeletePerson = new ToolStripButton();
      this.txbAdditional = new TextBox();
      this.lblAdditional = new Label();
      this.txbKPP = new TextBox();
      this.lblKPP = new Label();
      this.ts = new ToolStrip();
      this.tsbApplay = new ToolStripButton();
      this.tsbSupplier = new ToolStripButton();
      this.gbPa = new GroupBox();
      this.btnCopyAdr = new Button();
      this.txbPDop = new TextBox();
      this.lblPDop = new Label();
      this.txbPFlat = new TextBox();
      this.txbPKorp = new TextBox();
      this.txbPHome = new TextBox();
      this.lblPFlat = new Label();
      this.lblPKorp = new Label();
      this.lblPHome = new Label();
      this.ucRegionPost = new UCRegion();
      this.txbPPost = new TextBox();
      this.lblPPost = new Label();
      this.txbPInd = new TextBox();
      this.lblPInd = new Label();
      this.gbUa = new GroupBox();
      this.txbUDop = new TextBox();
      this.txbFax = new TextBox();
      this.txbPhone = new TextBox();
      this.lblUDop = new Label();
      this.lblFax = new Label();
      this.lblPhone = new Label();
      this.txbUFlat = new TextBox();
      this.txbUKorp = new TextBox();
      this.txbUHome = new TextBox();
      this.lblUFlat = new Label();
      this.lblUKorp = new Label();
      this.lblUHome = new Label();
      this.ucRegion = new UCRegion();
      this.txbUPost = new TextBox();
      this.lblUPost = new Label();
      this.txbUInd = new TextBox();
      this.lblUInd = new Label();
      this.gbBank = new GroupBox();
      this.txbSch = new TextBox();
      this.lblSch = new Label();
      this.lblBank = new Label();
      this.cmbBank = new ComboBox();
      this.txbOKONH = new TextBox();
      this.lblOKONH = new Label();
      this.txbOKPO = new TextBox();
      this.lblOKPO = new Label();
      this.txbINN = new TextBox();
      this.lblINN = new Label();
      this.txbTypeWork = new TextBox();
      this.lblTypeWork = new Label();
      this.gbBoss = new GroupBox();
      this.txbLastName = new TextBox();
      this.lblLastName = new Label();
      this.txbName = new TextBox();
      this.lblName = new Label();
      this.lblFamily = new Label();
      this.txbFamily = new TextBox();
      this.txbSmallName = new TextBox();
      this.txbNameOrg = new TextBox();
      this.lblSmallName = new Label();
      this.lblNameOrg = new Label();
      this.pnBtn = new Panel();
      this.btnSave = new Button();
      this.btnShowPersons = new Button();
      this.lblAddressPriority = new Label();
      this.cmbAddressPriority = new ComboBox();
      this.tbKbk = new TextBox();
      this.label1 = new Label();
      this.gbPersons.SuspendLayout();
      ((ISupportInitialize) this.dgvPersons).BeginInit();
      this.pnPersons.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.ts.SuspendLayout();
      this.gbPa.SuspendLayout();
      this.gbUa.SuspendLayout();
      this.gbBank.SuspendLayout();
      this.gbBoss.SuspendLayout();
      this.pnBtn.SuspendLayout();
      this.SuspendLayout();
      this.hp.HelpNamespace = "Help.chm";
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) componentResourceManager.GetObject("btnExit.Image");
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(664, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(79, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.txbOGRN.Location = new Point(280, 176);
      this.txbOGRN.Name = "txbOGRN";
      this.txbOGRN.Size = new Size(169, 22);
      this.txbOGRN.TabIndex = 25;
      this.lblOGRN.AutoSize = true;
      this.lblOGRN.Location = new Point(231, 178);
      this.lblOGRN.Name = "lblOGRN";
      this.lblOGRN.Size = new Size(44, 16);
      this.lblOGRN.TabIndex = 24;
      this.lblOGRN.Text = "ОГРН";
      this.gbPersons.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.gbPersons.Controls.Add((Control) this.dgvPersons);
      this.gbPersons.Controls.Add((Control) this.pnPersons);
      this.gbPersons.Location = new Point(10, 694);
      this.gbPersons.Name = "gbPersons";
      this.gbPersons.Size = new Size(745, 208);
      this.gbPersons.TabIndex = 23;
      this.gbPersons.TabStop = false;
      this.gbPersons.Text = "Список сотрудников";
      this.gbPersons.Visible = false;
      this.dgvPersons.BackgroundColor = Color.AliceBlue;
      this.dgvPersons.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvPersons.Dock = DockStyle.Fill;
      this.dgvPersons.Location = new Point(3, 47);
      this.dgvPersons.Name = "dgvPersons";
      this.dgvPersons.Size = new Size(739, 158);
      this.dgvPersons.TabIndex = 0;
      this.dgvPersons.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvPersons_CellBeginEdit);
      this.dgvPersons.CellEndEdit += new DataGridViewCellEventHandler(this.dgvPersons_CellEndEdit);
      this.pnPersons.Controls.Add((Control) this.toolStrip1);
      this.pnPersons.Dock = DockStyle.Top;
      this.pnPersons.Location = new Point(3, 18);
      this.pnPersons.Name = "pnPersons";
      this.pnPersons.Size = new Size(739, 29);
      this.pnPersons.TabIndex = 1;
      this.toolStrip1.Font = new Font("Tahoma", 10f);
      this.toolStrip1.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.tsbAddPerson,
        (ToolStripItem) this.tsbSavePerson,
        (ToolStripItem) this.tsbDeletePerson
      });
      this.toolStrip1.Location = new Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new Size(739, 25);
      this.toolStrip1.TabIndex = 3;
      this.toolStrip1.Text = "toolStrip1";
      this.tsbAddPerson.Image = (Image) Resources.add_var;
      this.tsbAddPerson.ImageTransparentColor = Color.Magenta;
      this.tsbAddPerson.Name = "tsbAddPerson";
      this.tsbAddPerson.Size = new Size(91, 22);
      this.tsbAddPerson.Text = "Добавить";
      this.tsbAddPerson.Click += new EventHandler(this.btnAddPerson_Click);
      this.tsbSavePerson.Enabled = false;
      this.tsbSavePerson.Image = (Image) Resources.Applay_var;
      this.tsbSavePerson.ImageTransparentColor = Color.Magenta;
      this.tsbSavePerson.Name = "tsbSavePerson";
      this.tsbSavePerson.Size = new Size(99, 22);
      this.tsbSavePerson.Text = "Сохранить";
      this.tsbSavePerson.Click += new EventHandler(this.btnSavePerson_Click);
      this.tsbDeletePerson.Image = (Image) Resources.delete_var;
      this.tsbDeletePerson.ImageTransparentColor = Color.Magenta;
      this.tsbDeletePerson.Name = "tsbDeletePerson";
      this.tsbDeletePerson.Size = new Size(82, 22);
      this.tsbDeletePerson.Text = "Удалить";
      this.tsbDeletePerson.Click += new EventHandler(this.btnDeletePerson_Click);
      this.txbAdditional.Location = new Point(252, 629);
      this.txbAdditional.Name = "txbAdditional";
      this.txbAdditional.Size = new Size(182, 22);
      this.txbAdditional.TabIndex = 21;
      this.lblAdditional.AutoSize = true;
      this.lblAdditional.Location = new Point(11, 632);
      this.lblAdditional.Name = "lblAdditional";
      this.lblAdditional.Size = new Size(235, 16);
      this.lblAdditional.TabIndex = 20;
      this.lblAdditional.Text = "Дополнительный код организации";
      this.txbKPP.Location = new Point(223, 150);
      this.txbKPP.Name = "txbKPP";
      this.txbKPP.Size = new Size(113, 22);
      this.txbKPP.TabIndex = 19;
      this.lblKPP.AutoSize = true;
      this.lblKPP.Location = new Point(181, 153);
      this.lblKPP.Name = "lblKPP";
      this.lblKPP.Size = new Size(36, 16);
      this.lblKPP.TabIndex = 18;
      this.lblKPP.Text = "КПП";
      this.ts.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsbApplay,
        (ToolStripItem) this.tsbSupplier
      });
      this.ts.Location = new Point(0, 0);
      this.ts.Name = "ts";
      this.ts.Size = new Size(755, 25);
      this.ts.TabIndex = 17;
      this.ts.Text = "toolStrip1";
      this.tsbApplay.Font = new Font("Tahoma", 10f);
      this.tsbApplay.Image = (Image) Resources.Applay_var;
      this.tsbApplay.ImageTransparentColor = Color.Magenta;
      this.tsbApplay.Name = "tsbApplay";
      this.tsbApplay.Size = new Size(99, 22);
      this.tsbApplay.Text = "Сохранить";
      this.tsbApplay.Click += new EventHandler(this.btnSave_Click);
      this.tsbSupplier.Font = new Font("Tahoma", 10f);
      this.tsbSupplier.Image = (Image) Resources.Van;
      this.tsbSupplier.ImageTransparentColor = Color.Magenta;
      this.tsbSupplier.Name = "tsbSupplier";
      this.tsbSupplier.Size = new Size(183, 22);
      this.tsbSupplier.Text = "Объявить поставщиком";
      this.tsbSupplier.Click += new EventHandler(this.tsbSupplier_Click);
      this.gbPa.Controls.Add((Control) this.btnCopyAdr);
      this.gbPa.Controls.Add((Control) this.txbPDop);
      this.gbPa.Controls.Add((Control) this.lblPDop);
      this.gbPa.Controls.Add((Control) this.txbPFlat);
      this.gbPa.Controls.Add((Control) this.txbPKorp);
      this.gbPa.Controls.Add((Control) this.txbPHome);
      this.gbPa.Controls.Add((Control) this.lblPFlat);
      this.gbPa.Controls.Add((Control) this.lblPKorp);
      this.gbPa.Controls.Add((Control) this.lblPHome);
      this.gbPa.Controls.Add((Control) this.ucRegionPost);
      this.gbPa.Controls.Add((Control) this.txbPPost);
      this.gbPa.Controls.Add((Control) this.lblPPost);
      this.gbPa.Controls.Add((Control) this.txbPInd);
      this.gbPa.Controls.Add((Control) this.lblPInd);
      this.gbPa.Location = new Point(12, 426);
      this.gbPa.Name = "gbPa";
      this.gbPa.Size = new Size(741, 203);
      this.gbPa.TabIndex = 16;
      this.gbPa.TabStop = false;
      this.gbPa.Text = "Почтовый адрес организации";
      this.btnCopyAdr.Location = new Point(9, 21);
      this.btnCopyAdr.Name = "btnCopyAdr";
      this.btnCopyAdr.Size = new Size(222, 23);
      this.btnCopyAdr.TabIndex = 20;
      this.btnCopyAdr.Text = "Скопировать из юридического";
      this.btnCopyAdr.UseVisualStyleBackColor = true;
      this.btnCopyAdr.Click += new EventHandler(this.btnCopyAdr_Click);
      this.txbPDop.AcceptsReturn = true;
      this.txbPDop.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txbPDop.Location = new Point((int) sbyte.MaxValue, 175);
      this.txbPDop.Name = "txbPDop";
      this.txbPDop.Size = new Size(608, 22);
      this.txbPDop.TabIndex = 16;
      this.lblPDop.AutoSize = true;
      this.lblPDop.Location = new Point(6, 178);
      this.lblPDop.Name = "lblPDop";
      this.lblPDop.Size = new Size(115, 16);
      this.lblPDop.TabIndex = 13;
      this.lblPDop.Text = "Доп. почт. адрес";
      this.txbPFlat.Location = new Point(427, 147);
      this.txbPFlat.Name = "txbPFlat";
      this.txbPFlat.Size = new Size(100, 22);
      this.txbPFlat.TabIndex = 10;
      this.txbPKorp.AcceptsReturn = true;
      this.txbPKorp.Location = new Point(226, 147);
      this.txbPKorp.Name = "txbPKorp";
      this.txbPKorp.Size = new Size(100, 22);
      this.txbPKorp.TabIndex = 9;
      this.txbPHome.Location = new Point(48, 147);
      this.txbPHome.Name = "txbPHome";
      this.txbPHome.Size = new Size(100, 22);
      this.txbPHome.TabIndex = 8;
      this.lblPFlat.AutoSize = true;
      this.lblPFlat.Location = new Point(350, 150);
      this.lblPFlat.Name = "lblPFlat";
      this.lblPFlat.Size = new Size(71, 16);
      this.lblPFlat.TabIndex = 7;
      this.lblPFlat.Text = "Квартира";
      this.lblPKorp.AutoSize = true;
      this.lblPKorp.Location = new Point(165, 150);
      this.lblPKorp.Name = "lblPKorp";
      this.lblPKorp.Size = new Size(55, 16);
      this.lblPKorp.TabIndex = 6;
      this.lblPKorp.Text = "Корпус";
      this.lblPHome.AutoSize = true;
      this.lblPHome.Location = new Point(8, 150);
      this.lblPHome.Name = "lblPHome";
      this.lblPHome.Size = new Size(34, 16);
      this.lblPHome.TabIndex = 5;
      this.lblPHome.Text = "Дом";
      this.ucRegionPost.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.ucRegionPost.Location = new Point(2, 73);
      this.ucRegionPost.Margin = new Padding(4);
      this.ucRegionPost.Name = "ucRegionPost";
      this.ucRegionPost.Size = new Size(632, 73);
      this.ucRegionPost.TabIndex = 4;
      this.txbPPost.Location = new Point(462, 50);
      this.txbPPost.Name = "txbPPost";
      this.txbPPost.Size = new Size(171, 22);
      this.txbPPost.TabIndex = 3;
      this.lblPPost.AutoSize = true;
      this.lblPPost.Location = new Point(348, 56);
      this.lblPPost.Name = "lblPPost";
      this.lblPPost.Size = new Size(108, 16);
      this.lblPPost.TabIndex = 2;
      this.lblPPost.Text = "Почтовый ящик";
      this.txbPInd.Location = new Point(79, 52);
      this.txbPInd.Name = "txbPInd";
      this.txbPInd.Size = new Size(238, 22);
      this.txbPInd.TabIndex = 1;
      this.lblPInd.AutoSize = true;
      this.lblPInd.Location = new Point(6, 56);
      this.lblPInd.Name = "lblPInd";
      this.lblPInd.Size = new Size(56, 16);
      this.lblPInd.TabIndex = 0;
      this.lblPInd.Text = "Индекс";
      this.gbUa.Controls.Add((Control) this.txbUDop);
      this.gbUa.Controls.Add((Control) this.txbFax);
      this.gbUa.Controls.Add((Control) this.txbPhone);
      this.gbUa.Controls.Add((Control) this.lblUDop);
      this.gbUa.Controls.Add((Control) this.lblFax);
      this.gbUa.Controls.Add((Control) this.lblPhone);
      this.gbUa.Controls.Add((Control) this.txbUFlat);
      this.gbUa.Controls.Add((Control) this.txbUKorp);
      this.gbUa.Controls.Add((Control) this.txbUHome);
      this.gbUa.Controls.Add((Control) this.lblUFlat);
      this.gbUa.Controls.Add((Control) this.lblUKorp);
      this.gbUa.Controls.Add((Control) this.lblUHome);
      this.gbUa.Controls.Add((Control) this.ucRegion);
      this.gbUa.Controls.Add((Control) this.txbUPost);
      this.gbUa.Controls.Add((Control) this.lblUPost);
      this.gbUa.Controls.Add((Control) this.txbUInd);
      this.gbUa.Controls.Add((Control) this.lblUInd);
      this.gbUa.Location = new Point(10, 250);
      this.gbUa.Name = "gbUa";
      this.gbUa.Size = new Size(741, 177);
      this.gbUa.TabIndex = 15;
      this.gbUa.TabStop = false;
      this.gbUa.Text = "Юридический адрес организации";
      this.txbUDop.Location = new Point(114, 147);
      this.txbUDop.Name = "txbUDop";
      this.txbUDop.Size = new Size(621, 22);
      this.txbUDop.TabIndex = 16;
      this.txbFax.Location = new Point(635, 21);
      this.txbFax.Name = "txbFax";
      this.txbFax.Size = new Size(101, 22);
      this.txbFax.TabIndex = 15;
      this.txbPhone.Location = new Point(475, 21);
      this.txbPhone.Name = "txbPhone";
      this.txbPhone.Size = new Size(98, 22);
      this.txbPhone.TabIndex = 14;
      this.lblUDop.AutoSize = true;
      this.lblUDop.Location = new Point(6, 150);
      this.lblUDop.Name = "lblUDop";
      this.lblUDop.Size = new Size(102, 16);
      this.lblUDop.TabIndex = 13;
      this.lblUDop.Text = "Доп. юр. адрес";
      this.lblFax.AutoSize = true;
      this.lblFax.Location = new Point(588, 24);
      this.lblFax.Name = "lblFax";
      this.lblFax.Size = new Size(41, 16);
      this.lblFax.TabIndex = 12;
      this.lblFax.Text = "Факс";
      this.lblPhone.AutoSize = true;
      this.lblPhone.Location = new Point(402, 24);
      this.lblPhone.Name = "lblPhone";
      this.lblPhone.Size = new Size(68, 16);
      this.lblPhone.TabIndex = 11;
      this.lblPhone.Text = "Телефон";
      this.txbUFlat.Location = new Point(425, 116);
      this.txbUFlat.Name = "txbUFlat";
      this.txbUFlat.Size = new Size(100, 22);
      this.txbUFlat.TabIndex = 10;
      this.txbUKorp.Location = new Point(224, 116);
      this.txbUKorp.Name = "txbUKorp";
      this.txbUKorp.Size = new Size(100, 22);
      this.txbUKorp.TabIndex = 9;
      this.txbUHome.Location = new Point(46, 116);
      this.txbUHome.Name = "txbUHome";
      this.txbUHome.Size = new Size(100, 22);
      this.txbUHome.TabIndex = 8;
      this.lblUFlat.AutoSize = true;
      this.lblUFlat.Location = new Point(348, 119);
      this.lblUFlat.Name = "lblUFlat";
      this.lblUFlat.Size = new Size(71, 16);
      this.lblUFlat.TabIndex = 7;
      this.lblUFlat.Text = "Квартира";
      this.lblUKorp.AutoSize = true;
      this.lblUKorp.Location = new Point(163, 119);
      this.lblUKorp.Name = "lblUKorp";
      this.lblUKorp.Size = new Size(55, 16);
      this.lblUKorp.TabIndex = 6;
      this.lblUKorp.Text = "Корпус";
      this.lblUHome.AutoSize = true;
      this.lblUHome.Location = new Point(6, 119);
      this.lblUHome.Name = "lblUHome";
      this.lblUHome.Size = new Size(34, 16);
      this.lblUHome.TabIndex = 5;
      this.lblUHome.Text = "Дом";
      this.ucRegion.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.ucRegion.Location = new Point(2, 42);
      this.ucRegion.Margin = new Padding(4);
      this.ucRegion.Name = "ucRegion";
      this.ucRegion.Size = new Size(632, 73);
      this.ucRegion.TabIndex = 4;
      this.txbUPost.Location = new Point(247, 19);
      this.txbUPost.Name = "txbUPost";
      this.txbUPost.Size = new Size(145, 22);
      this.txbUPost.TabIndex = 3;
      this.lblUPost.AutoSize = true;
      this.lblUPost.Location = new Point(171, 22);
      this.lblUPost.Name = "lblUPost";
      this.lblUPost.Size = new Size(78, 16);
      this.lblUPost.TabIndex = 2;
      this.lblUPost.Text = "Почт. ящик";
      this.txbUInd.Location = new Point(79, 21);
      this.txbUInd.Name = "txbUInd";
      this.txbUInd.Size = new Size(83, 22);
      this.txbUInd.TabIndex = 1;
      this.lblUInd.AutoSize = true;
      this.lblUInd.Location = new Point(6, 25);
      this.lblUInd.Name = "lblUInd";
      this.lblUInd.Size = new Size(56, 16);
      this.lblUInd.TabIndex = 0;
      this.lblUInd.Text = "Индекс";
      this.gbBank.Controls.Add((Control) this.txbSch);
      this.gbBank.Controls.Add((Control) this.lblSch);
      this.gbBank.Controls.Add((Control) this.lblBank);
      this.gbBank.Controls.Add((Control) this.cmbBank);
      this.gbBank.Location = new Point(10, 200);
      this.gbBank.Name = "gbBank";
      this.gbBank.Size = new Size(741, 50);
      this.gbBank.TabIndex = 14;
      this.gbBank.TabStop = false;
      this.gbBank.Text = "Банк";
      this.txbSch.Location = new Point(521, 22);
      this.txbSch.Name = "txbSch";
      this.txbSch.Size = new Size(194, 22);
      this.txbSch.TabIndex = 3;
      this.lblSch.AutoSize = true;
      this.lblSch.Location = new Point(402, 23);
      this.lblSch.Name = "lblSch";
      this.lblSch.Size = new Size(113, 16);
      this.lblSch.TabIndex = 2;
      this.lblSch.Text = "Расчетный счет";
      this.lblBank.AutoSize = true;
      this.lblBank.Location = new Point(6, 23);
      this.lblBank.Name = "lblBank";
      this.lblBank.Size = new Size(40, 16);
      this.lblBank.TabIndex = 1;
      this.lblBank.Text = "Банк";
      this.cmbBank.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBank.FormattingEnabled = true;
      this.cmbBank.Location = new Point(52, 20);
      this.cmbBank.Name = "cmbBank";
      this.cmbBank.Size = new Size(330, 24);
      this.cmbBank.TabIndex = 0;
      this.txbOKONH.Location = new Point(69, 176);
      this.txbOKONH.Name = "txbOKONH";
      this.txbOKONH.Size = new Size(135, 22);
      this.txbOKONH.TabIndex = 13;
      this.lblOKONH.AutoSize = true;
      this.lblOKONH.Location = new Point(9, 178);
      this.lblOKONH.Name = "lblOKONH";
      this.lblOKONH.Size = new Size(54, 16);
      this.lblOKONH.TabIndex = 12;
      this.lblOKONH.Text = "ОКОНХ";
      this.txbOKPO.Location = new Point(400, 150);
      this.txbOKPO.Name = "txbOKPO";
      this.txbOKPO.Size = new Size(145, 22);
      this.txbOKPO.TabIndex = 11;
      this.lblOKPO.AutoSize = true;
      this.lblOKPO.Location = new Point(348, 154);
      this.lblOKPO.Name = "lblOKPO";
      this.lblOKPO.Size = new Size(46, 16);
      this.lblOKPO.TabIndex = 10;
      this.lblOKPO.Text = "ОКПО";
      this.txbINN.Location = new Point(53, 150);
      this.txbINN.Name = "txbINN";
      this.txbINN.Size = new Size(119, 22);
      this.txbINN.TabIndex = 9;
      this.lblINN.AutoSize = true;
      this.lblINN.Location = new Point(9, 154);
      this.lblINN.Name = "lblINN";
      this.lblINN.Size = new Size(38, 16);
      this.lblINN.TabIndex = 8;
      this.lblINN.Text = "ИНН";
      this.txbTypeWork.Location = new Point(142, 125);
      this.txbTypeWork.Name = "txbTypeWork";
      this.txbTypeWork.Size = new Size(609, 22);
      this.txbTypeWork.TabIndex = 7;
      this.lblTypeWork.AutoSize = true;
      this.lblTypeWork.Location = new Point(9, 128);
      this.lblTypeWork.Name = "lblTypeWork";
      this.lblTypeWork.Size = new Size((int) sbyte.MaxValue, 16);
      this.lblTypeWork.TabIndex = 6;
      this.lblTypeWork.Text = "Вид деятельности";
      this.gbBoss.Controls.Add((Control) this.txbLastName);
      this.gbBoss.Controls.Add((Control) this.lblLastName);
      this.gbBoss.Controls.Add((Control) this.txbName);
      this.gbBoss.Controls.Add((Control) this.lblName);
      this.gbBoss.Controls.Add((Control) this.lblFamily);
      this.gbBoss.Controls.Add((Control) this.txbFamily);
      this.gbBoss.Location = new Point(10, 79);
      this.gbBoss.Name = "gbBoss";
      this.gbBoss.Size = new Size(741, 46);
      this.gbBoss.TabIndex = 5;
      this.gbBoss.TabStop = false;
      this.gbBoss.Text = "Руководитель";
      this.txbLastName.Location = new Point(557, 18);
      this.txbLastName.Name = "txbLastName";
      this.txbLastName.Size = new Size(160, 22);
      this.txbLastName.TabIndex = 5;
      this.lblLastName.AutoSize = true;
      this.lblLastName.Location = new Point(480, 21);
      this.lblLastName.Name = "lblLastName";
      this.lblLastName.Size = new Size(71, 16);
      this.lblLastName.TabIndex = 4;
      this.lblLastName.Text = "Отчество";
      this.txbName.Location = new Point(298, 18);
      this.txbName.Name = "txbName";
      this.txbName.Size = new Size(160, 22);
      this.txbName.TabIndex = 3;
      this.lblName.AutoSize = true;
      this.lblName.Location = new Point(258, 21);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(34, 16);
      this.lblName.TabIndex = 2;
      this.lblName.Text = "Имя";
      this.lblFamily.AutoSize = true;
      this.lblFamily.Location = new Point(8, 21);
      this.lblFamily.Name = "lblFamily";
      this.lblFamily.Size = new Size(67, 16);
      this.lblFamily.TabIndex = 1;
      this.lblFamily.Text = "Фамилия";
      this.txbFamily.Location = new Point(81, 17);
      this.txbFamily.Name = "txbFamily";
      this.txbFamily.Size = new Size(160, 22);
      this.txbFamily.TabIndex = 0;
      this.txbSmallName.Location = new Point(170, 54);
      this.txbSmallName.Name = "txbSmallName";
      this.txbSmallName.Size = new Size(581, 22);
      this.txbSmallName.TabIndex = 4;
      this.txbNameOrg.Location = new Point(170, 28);
      this.txbNameOrg.Name = "txbNameOrg";
      this.txbNameOrg.Size = new Size(581, 22);
      this.txbNameOrg.TabIndex = 3;
      this.lblSmallName.AutoSize = true;
      this.lblSmallName.Location = new Point(9, 57);
      this.lblSmallName.Name = "lblSmallName";
      this.lblSmallName.Size = new Size(129, 16);
      this.lblSmallName.TabIndex = 2;
      this.lblSmallName.Text = "Краткое название";
      this.lblNameOrg.AutoSize = true;
      this.lblNameOrg.Location = new Point(9, 31);
      this.lblNameOrg.Name = "lblNameOrg";
      this.lblNameOrg.Size = new Size(163, 16);
      this.lblNameOrg.TabIndex = 1;
      this.lblNameOrg.Text = "Название организации";
      this.pnBtn.Controls.Add((Control) this.btnSave);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 988);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(755, 40);
      this.pnBtn.TabIndex = 0;
      this.btnSave.Image = (Image) componentResourceManager.GetObject("btnSave.Image");
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.Location = new Point(12, 5);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(106, 30);
      this.btnSave.TabIndex = 1;
      this.btnSave.Text = "Сохранить";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Visible = false;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnShowPersons.Location = new Point(461, 629);
      this.btnShowPersons.Name = "btnShowPersons";
      this.btnShowPersons.Size = new Size(253, 23);
      this.btnShowPersons.TabIndex = 22;
      this.btnShowPersons.Text = "Cписок сотрудников";
      this.btnShowPersons.UseVisualStyleBackColor = true;
      this.btnShowPersons.Click += new EventHandler(this.btnShowPersons_Click);
      this.lblAddressPriority.AutoSize = true;
      this.lblAddressPriority.Location = new Point(14, 668);
      this.lblAddressPriority.Name = "lblAddressPriority";
      this.lblAddressPriority.Size = new Size(261, 16);
      this.lblAddressPriority.TabIndex = 26;
      this.lblAddressPriority.Text = "Приоритетный адрес отправки счетов";
      this.cmbAddressPriority.FormattingEnabled = true;
      this.cmbAddressPriority.Items.AddRange(new object[2]
      {
        (object) "Юридический",
        (object) "Почтовый"
      });
      this.cmbAddressPriority.Location = new Point(281, 665);
      this.cmbAddressPriority.Name = "cmbAddressPriority";
      this.cmbAddressPriority.Size = new Size(187, 24);
      this.cmbAddressPriority.TabIndex = 27;
      this.tbKbk.Location = new Point(510, 177);
      this.tbKbk.Name = "tbKbk";
      this.tbKbk.Size = new Size(241, 22);
      this.tbKbk.TabIndex = 28;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(471, 180);
      this.label1.Name = "label1";
      this.label1.Size = new Size(33, 16);
      this.label1.TabIndex = 29;
      this.label1.Text = "КБК";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(754, 1045);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.tbKbk);
      this.Controls.Add((Control) this.cmbAddressPriority);
      this.Controls.Add((Control) this.lblAddressPriority);
      this.Controls.Add((Control) this.txbOGRN);
      this.Controls.Add((Control) this.lblOGRN);
      this.Controls.Add((Control) this.gbPersons);
      this.Controls.Add((Control) this.txbAdditional);
      this.Controls.Add((Control) this.lblAdditional);
      this.Controls.Add((Control) this.txbKPP);
      this.Controls.Add((Control) this.lblKPP);
      this.Controls.Add((Control) this.ts);
      this.Controls.Add((Control) this.gbPa);
      this.Controls.Add((Control) this.gbUa);
      this.Controls.Add((Control) this.gbBank);
      this.Controls.Add((Control) this.txbOKONH);
      this.Controls.Add((Control) this.lblOKONH);
      this.Controls.Add((Control) this.txbOKPO);
      this.Controls.Add((Control) this.lblOKPO);
      this.Controls.Add((Control) this.txbINN);
      this.Controls.Add((Control) this.lblINN);
      this.Controls.Add((Control) this.txbTypeWork);
      this.Controls.Add((Control) this.lblTypeWork);
      this.Controls.Add((Control) this.gbBoss);
      this.Controls.Add((Control) this.txbSmallName);
      this.Controls.Add((Control) this.txbNameOrg);
      this.Controls.Add((Control) this.lblSmallName);
      this.Controls.Add((Control) this.lblNameOrg);
      this.Controls.Add((Control) this.pnBtn);
      this.Controls.Add((Control) this.btnShowPersons);
      this.hp.SetHelpKeyword((Control) this, "kv515.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      this.Name = "FrmEditOrg";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Организация";
      this.Load += new EventHandler(this.FrmEditOrg_Load);
      this.Shown += new EventHandler(this.FrmEditOrg_Shown);
      this.gbPersons.ResumeLayout(false);
      ((ISupportInitialize) this.dgvPersons).EndInit();
      this.pnPersons.ResumeLayout(false);
      this.pnPersons.PerformLayout();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ts.ResumeLayout(false);
      this.ts.PerformLayout();
      this.gbPa.ResumeLayout(false);
      this.gbPa.PerformLayout();
      this.gbUa.ResumeLayout(false);
      this.gbUa.PerformLayout();
      this.gbBank.ResumeLayout(false);
      this.gbBank.PerformLayout();
      this.gbBoss.ResumeLayout(false);
      this.gbBoss.PerformLayout();
      this.pnBtn.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
