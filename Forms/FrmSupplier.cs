// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmSupplier
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Forms.Controls;
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
  public class FrmSupplier : Form
  {
    private readonly FormStateSaver fss = new FormStateSaver(FrmSupplier.ic);
    protected GridSettings MySettingsSupplier = new GridSettings();
    private int curIdx = -1;
    private IList serviceList = (IList) new ArrayList();
    private IContainer components = (IContainer) null;
    private static IContainer ic;
    private Supplier curOrg;
    private string lastPer;
    private string lastRec;
    private IList<Supplier> orgList;
    private ISession session;
    private bool _readOnly;
    private bool _readOnlyBaseOrg;
    private Company _company;
    private DataSet dsMain;
    private DataTable dtServ;
    private DataColumn dataColumn1;
    private DataColumn dataColumn2;
    private Panel pnBtn;
    private Button btnExit;
    public HelpProvider hp;
    private Panel pnUp;
    private ComboBox cmbCompany;
    private Label lblCompany;
    private Label lblRec;
    private TextBox txbRec;
    private ToolStrip tsSupplier;
    private Panel pnCmpSupplier;
    private SplitContainer splitContainer1;
    private Panel pnFiltr;
    private CheckBox cbINN;
    private DataGridView dgvBaseOrg;
    private UCSupplier ucSupplier1;
    private Panel pnSupplier;
    private ToolStripButton tsbSupplier;
    private ToolStripButton tsbCmpSupplier;
    private Label lblPerfomer;
    private Label lblRecipient;
    private ComboBox cmbPerfomer;
    private ComboBox cmbRecipient;
    private GroupBox gbUp;
    private DataGridView dgvSupplier;
    public ToolStrip toolStrip1;
    public ToolStripButton tsbAdd;
    public ToolStripButton tsbApplay;
    public ToolStripButton tsbCancel;
    public ToolStripButton tsbDelete;
    public ToolStripButton tsbExit;
    private TextBox txbPer;
    private Label lblPer;
    private CheckBox cbPerINN;

    public FrmSupplier(Company companyTemp)
    {
      this.InitializeComponent();
      this._company = companyTemp;
      this.CheckAccess();
      this.ucSupplier1.tsbExit.Visible = false;
      this.fss.ParentForm = (Form) this;
      this.session = Domain.CurrentSession;
      this.MySettingsSupplier.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
      this.LoadCmbRecipietnsAndCmbPerformer();
      this.ucSupplier1.session = Domain.CurrentSession;
      this.ucSupplier1.LoadSettings();
      this.ucSupplier1.tsbExit.Click += new EventHandler(this.tsbExit_Click);
      this.ucSupplier1.FillGrid();
      DataTable table = new DataTable();
      table.Columns.Add(new DataColumn("ID", typeof (int)));
      table.Columns.Add(new DataColumn("Name"));
      KvrplHelper.AddRow(table, 0, "нет");
      KvrplHelper.AddRow(table, 1, "да");
      ((DataGridViewComboBoxColumn) this.ucSupplier1.dgvBase.Columns["Vat"]).DataSource = (object) table;
      ((DataGridViewComboBoxColumn) this.ucSupplier1.dgvBase.Columns["Service"]).DataSource = (object) this.session.CreateQuery("from Service s where s.Root=0 and s.ServiceId>0 order by " + Options.SortService).List();
      IList<BaseOrg> baseOrgList1 = this.session.CreateQuery("select new BaseOrg(BaseOrgId,NameOrgMin) from BaseOrg where BaseOrgId in (select Recipient.BaseOrgId from Supplier) and BaseOrgId<>0 order by NameOrgMin").List<BaseOrg>();
      ((DataGridViewComboBoxColumn) this.ucSupplier1.dgvBase.Columns["Recipient"]).DataSource = (object) baseOrgList1;
      baseOrgList1.Insert(0, new BaseOrg(0, ""));
      IList<BaseOrg> baseOrgList2 = this.session.CreateQuery("select new BaseOrg(BaseOrgId,NameOrgMin) from BaseOrg where BaseOrgId in (select Perfomer.BaseOrgId from Supplier where Recipient.BaseOrgId=0) and BaseOrgId<>0 order by NameOrgMin").List<BaseOrg>();
      ((DataGridViewComboBoxColumn) this.ucSupplier1.dgvBase.Columns["Perfomer"]).DataSource = (object) baseOrgList2;
      baseOrgList2.Insert(0, new BaseOrg(0, ""));
      this.cmbCompany.DataSource = (object) this.session.CreateQuery("from Company order by CompanyId").List<Company>();
      this.cmbCompany.ValueMember = "CompanyId";
      this.cmbCompany.DisplayMember = "CompanyName";
      Company company = new Company();
      short num;
      try
      {
        num = Convert.ToInt16(this.session.CreateQuery(string.Format("select cp.ParamValue from CompanyParam cp where cp.Company.CompanyId={1} and cp.Period.PeriodId=0 and cp.Param.ParamId=211 and DBeg<='{0}' and DEnd>='{0}'", (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), (object) Options.Company.CompanyId)).UniqueResult());
        if ((int) num == 0)
          num = Options.Company.CompanyId;
      }
      catch (Exception ex)
      {
        num = Options.Company.CompanyId;
      }
      this.cmbCompany.SelectedValue = (object) num;
      this.ucSupplier1.Company = this.session.Get<Company>((object) num);
    }

    private void CheckAccess()
    {
      this._readOnlyBaseOrg = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(33, this._company, false));
      this._readOnly = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(75, this._company, false));
      this.dgvSupplier.ReadOnly = !this._readOnly;
      this.toolStrip1.Visible = this._readOnly;
      this.dgvBaseOrg.ReadOnly = !this._readOnlyBaseOrg;
      this.ucSupplier1.toolStrip1.Visible = this._readOnlyBaseOrg;
      this.ucSupplier1.dgvBase.ReadOnly = !this._readOnlyBaseOrg;
    }

    private void FrmSupplier_Shown(object sender, EventArgs e)
    {
      this.LoadSupplier();
      this.LoadList();
      if (KvrplHelper.CheckProxy(32, 1, (Company) null, false))
        return;
      this.tsbCmpSupplier.Checked = true;
      this.tsbCmpSupplier_Click(sender, e);
    }

    private void tsbCancel_Click(object sender, EventArgs e)
    {
      this.LoadSupplier();
    }

    private void tsbDelete_Click(object sender, EventArgs e)
    {
      if (this.dgvSupplier.Rows.Count <= 0 || this.dgvSupplier.CurrentRow == null)
        return;
      this.session.Clear();
      if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
      {
        Supplier dataBoundItem = (Supplier) this.dgvSupplier.CurrentRow.DataBoundItem;
        if (dataBoundItem.Recipient.BaseOrgId == 0)
          dataBoundItem.Recipient = this.session.Get<BaseOrg>((object) 0);
        if (dataBoundItem.Perfomer.BaseOrgId == 0)
          dataBoundItem.Perfomer = this.session.Get<BaseOrg>((object) 0);
        try
        {
          this.session.Delete((object) dataBoundItem);
          this.session.Flush();
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Не могу удалить текущую запись!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      this.session.Clear();
      this.LoadSupplier();
    }

    private void tsbExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void LoadList()
    {
      this.dgvBaseOrg.Columns.Clear();
      this.dgvBaseOrg.DataSource = (object) null;
      this.orgList = this.session.CreateQuery("from Supplier order by Recipient.NameOrgMin,Perfomer.NameOrgMin").List<Supplier>();
      this.dgvBaseOrg.DataSource = (object) this.orgList;
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvBaseOrg.Columns)
      {
        if (column.Name != "PerName" && column.Name != "RecName")
        {
          column.Visible = false;
        }
        else
        {
          if (column.Name == "PerName")
            column.HeaderText = "Исполнитель";
          if (column.Name == "RecName")
            column.HeaderText = "Получатель";
          column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
      }
    }

    private void dgvBaseOrg_SelectionChanged(object sender, EventArgs e)
    {
      if (this.dgvBaseOrg.CurrentRow == null || this.dgvBaseOrg.CurrentRow.Index >= this.orgList.Count)
        return;
      if (this.curIdx != this.dgvBaseOrg.CurrentRow.Index)
      {
        this.curOrg = this.orgList[this.dgvBaseOrg.CurrentRow.Index];
        this.ucSupplier1.CurOrg = this.curOrg;
        this.ucSupplier1.FillGrid();
      }
      if (this.ucSupplier1.CurOrg != this.orgList[this.dgvBaseOrg.CurrentRow.Index])
      {
        int curIdx = this.curIdx;
        this.curIdx = this.dgvBaseOrg.CurrentRow.Index;
        this.dgvBaseOrg.CurrentCell = this.dgvBaseOrg.Rows[curIdx].Cells[2];
        this.curIdx = curIdx;
        this.dgvBaseOrg.Rows[curIdx].Selected = true;
      }
      this.curIdx = this.dgvBaseOrg.CurrentRow.Index;
    }

    private void cbINN_Click(object sender, EventArgs e)
    {
      this.dgvBaseOrg.Columns.Clear();
      this.dgvBaseOrg.DataSource = (object) null;
      this.ucSupplier1.dgvBase.Rows.Clear();
      this.ucSupplier1.dgvBase.DataSource = (object) null;
      string str = "";
      if (this.cbINN.Checked)
        str = " and Recipient.INN<>''";
      if (this.cbPerINN.Checked)
        str += " and Perfomer.INN<>''";
      this.orgList = this.session.CreateQuery("from Supplier where Recipient.NameOrgMin like '%" + this.txbRec.Text + "%' and Perfomer.NameOrgMin like '%" + this.txbPer.Text + "%' " + str + " order by Recipient.NameOrgMin,Perfomer.NameOrgMin").List<Supplier>();
      if (this.orgList.Count <= 0)
        return;
      this.dgvBaseOrg.DataSource = (object) this.orgList;
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvBaseOrg.Columns)
      {
        if (column.Name != "PerName" && column.Name != "RecName")
        {
          column.Visible = false;
        }
        else
        {
          if (column.Name == "PerName")
            column.HeaderText = "Исполнитель";
          if (column.Name == "RecName")
            column.HeaderText = "Получатель";
          column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
      }
      this.curIdx = 1;
      this.dgvBaseOrg.CurrentCell = this.dgvBaseOrg.Rows[0].Cells["RecName"];
      this.dgvBaseOrg_SelectionChanged(sender, e);
    }

    private void dgvBaseOrg_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    private void cmbCompany_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (this.cmbCompany.SelectedItem == null)
        return;
      if (this.tsbCmpSupplier.Checked && !KvrplHelper.CheckProxy(33, 1, (Company) this.cmbCompany.SelectedItem, true))
      {
        this.cmbCompany.SelectedItem = (object) this.session.Get<Company>((object) this.ucSupplier1.Company.CompanyId);
        this.cmbCompany.SelectedValue = (object) this.ucSupplier1.Company.CompanyId;
      }
      else
        this.ucSupplier1.Company = (Company) this.cmbCompany.SelectedItem;
      if ((int) this.ucSupplier1.Company.CompanyId != (int) ((Company) this.cmbCompany.SelectedItem).CompanyId)
        this.cmbCompany.SelectedValue = (object) this.ucSupplier1.Company.CompanyId;
    }

    private void tsbCmpSupplier_Click(object sender, EventArgs e)
    {
      this.tsbCmpSupplier.Checked = true;
      this.tsbSupplier.Checked = false;
      if (!KvrplHelper.CheckProxy(33, 1, this.ucSupplier1.Company, true))
      {
        this.tsbCmpSupplier.Checked = false;
      }
      else
      {
        this.pnCmpSupplier.BringToFront();
        this.tsbCmpSupplier.Checked = true;
      }
    }

    private void txbSupplier_KeyUp(object sender, KeyEventArgs e)
    {
      this.dgvBaseOrg.Columns.Clear();
      this.dgvBaseOrg.DataSource = (object) null;
      this.ucSupplier1.dgvBase.Rows.Clear();
      this.ucSupplier1.dgvBase.DataSource = (object) null;
      this.session = Domain.CurrentSession;
      string str = "";
      if (this.cbINN.Checked)
        str = " and Recipient.INN<>''";
      if (this.cbPerINN.Checked)
        str += " and Perfomer.INN<>''";
      this.orgList = (IList<Supplier>) new List<Supplier>();
      this.orgList = this.session.CreateQuery("from Supplier where Recipient.NameOrgMin like '%" + this.txbRec.Text + "%' and Perfomer.NameOrgMin like '%" + this.txbPer.Text + "%' " + str + " order by Recipient.NameOrgMin,Perfomer.NameOrgMin").List<Supplier>();
      if (this.orgList.Count > 0)
      {
        this.dgvBaseOrg.DataSource = (object) this.orgList;
        this.lastRec = this.txbRec.Text;
        this.lastPer = this.txbPer.Text;
        foreach (DataGridViewColumn column in (BaseCollection) this.dgvBaseOrg.Columns)
        {
          if (column.Name != "PerName" && column.Name != "RecName")
          {
            column.Visible = false;
          }
          else
          {
            if (column.Name == "PerName")
              column.HeaderText = "Исполнитель";
            if (column.Name == "RecName")
              column.HeaderText = "Получатель";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
          }
        }
      }
      this.curIdx = 1;
      if (this.dgvBaseOrg.Rows.Count > 0)
        this.dgvBaseOrg.CurrentCell = this.dgvBaseOrg.Rows[0].Cells["RecName"];
      this.dgvBaseOrg_SelectionChanged(sender, (EventArgs) e);
    }

    private void LoadSupplier()
    {
      this.dgvSupplier.Columns.Clear();
      this.dgvSupplier.DataSource = (object) null;
      this.session.Clear();
      string str = "";
      if ((uint) Convert.ToInt32(this.cmbRecipient.SelectedValue) > 0U)
        str = string.Format(" and Recipient.BaseOrgId={0}", (object) ((BaseOrg) this.cmbRecipient.SelectedItem).BaseOrgId);
      if ((uint) Convert.ToInt32(this.cmbPerfomer.SelectedValue) > 0U)
        str += string.Format(" and Perfomer.BaseOrgId={0}", (object) ((BaseOrg) this.cmbPerfomer.SelectedItem).BaseOrgId);
      this.dgvSupplier.DataSource = (object) this.session.CreateQuery("from Supplier where 1=1 " + str + " order by Recipient.NameOrgMin,Perfomer.NameOrgMin").List<Supplier>();
      this.MySettingsSupplier.GridName = "dcSupplier";
      this.SetViewSupplier();
    }

    private void SetViewSupplier()
    {
      this.dgvSupplier.Columns["RecName"].Visible = false;
      this.dgvSupplier.Columns["PerName"].Visible = false;
      if (Convert.ToInt32(this.cmbPerfomer.SelectedValue) == 0)
      {
        IList<BaseOrg> baseOrgList = this.session.CreateQuery("select new BaseOrg(BaseOrgId,NameOrgMin) from BaseOrg where BaseOrgId<>0 order by NameOrgMin").List<BaseOrg>();
        baseOrgList.Insert(0, new BaseOrg(0, ""));
        KvrplHelper.AddComboBoxColumn(this.dgvSupplier, 1, (IList) baseOrgList, "BaseOrgId", "NameOrgMin", "Исполнитель", "Perfomer", 40, 300);
      }
      this.dgvSupplier.Columns["SupplierId"].Visible = false;
      if (Convert.ToInt32(this.cmbRecipient.SelectedValue) == 0)
      {
        IList<BaseOrg> baseOrgList = this.session.CreateQuery("select new BaseOrg(BaseOrgId,NameOrgMin) from BaseOrg where BaseOrgId<>0 order by NameOrgMin").List<BaseOrg>();
        baseOrgList.Insert(0, new BaseOrg(0, ""));
        KvrplHelper.AddComboBoxColumn(this.dgvSupplier, 1, (IList) baseOrgList, "BaseOrgId", "NameOrgMin", "Получатель", "Recipient", 40, 300);
      }
      KvrplHelper.ViewEdit(this.dgvSupplier);
      foreach (DataGridViewRow row in (IEnumerable) this.dgvSupplier.Rows)
      {
        if (this.dgvSupplier.Columns["Recipient"] != null && ((Supplier) row.DataBoundItem).Recipient != null)
          row.Cells["Recipient"].Value = (object) ((Supplier) row.DataBoundItem).Recipient.BaseOrgId;
        if (this.dgvSupplier.Columns["Perfomer"] != null && ((Supplier) row.DataBoundItem).Perfomer != null)
          row.Cells["Perfomer"].Value = (object) ((Supplier) row.DataBoundItem).Perfomer.BaseOrgId;
      }
      this.LoadSettingsSupplier();
    }

    private void LoadSettingsSupplier()
    {
      this.MySettingsSupplier.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvSupplier.Columns)
        this.MySettingsSupplier.GetMySettings(column);
    }

    private void SaveSupplier()
    {
      if (this.dgvSupplier.Rows.Count <= 0 || this.dgvSupplier.CurrentRow.Index < 0)
        return;
      this.session.Clear();
      bool flag1 = false;
      Supplier dataBoundItem = (Supplier) this.dgvSupplier.CurrentRow.DataBoundItem;
      int supplierId = dataBoundItem.SupplierId;
      bool flag2 = dataBoundItem.SupplierId == 0;
      if (this.session.CreateCriteria(typeof (Supplier)).Add((ICriterion) Restrictions.Eq("Recipient", (object) dataBoundItem.Recipient)).Add((ICriterion) Restrictions.Eq("Perfomer", (object) dataBoundItem.Perfomer)).List<Supplier>().Count > 0)
      {
        int num1 = (int) MessageBox.Show("Сохранение невозможно. Такая пара уже есть.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (dataBoundItem.Recipient == null)
          dataBoundItem.Recipient = this.session.Get<BaseOrg>((object) Convert.ToInt32(this.dgvSupplier.CurrentRow.Cells["Recipient"].Value));
        if (dataBoundItem.Perfomer == null)
          dataBoundItem.Perfomer = this.session.Get<BaseOrg>((object) Convert.ToInt32(this.dgvSupplier.CurrentRow.Cells["Perfomer"].Value));
        dataBoundItem.UName = Options.Login;
        dataBoundItem.DEdit = DateTime.Now.Date;
        try
        {
          if (flag2)
          {
            flag1 = false;
            IList<int> intList = this.session.CreateSQLQuery("select DBA.gen_id('dcSupplier',1)").List<int>();
            dataBoundItem.SupplierId = intList[0];
            this.session.Save((object) dataBoundItem);
          }
          else
            this.session.Update((object) dataBoundItem);
          this.session.Flush();
        }
        catch (Exception ex)
        {
          if (ex.InnerException != null && ex.InnerException.Message.ToLower().IndexOf("primary key for table 'dcsupplier' is not unique") != -1)
          {
            KvrplHelper.ResetGeners("dcSupplier", "Supplier_Id");
            KvrplHelper.WriteLog(ex, (LsClient) null);
            int num2 = (int) MessageBox.Show("Была устранена ошибка генерации уникального поля! Введите запись заново!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          else
          {
            int num2 = (int) MessageBox.Show("Невозможно сохранить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
        }
      }
    }

    private void tsbAdd_Click(object sender, EventArgs e)
    {
      this.tsbApplay.Enabled = true;
      this.tsbCancel.Enabled = true;
      this.tsbDelete.Enabled = false;
      Supplier supplier = new Supplier();
      if ((uint) Convert.ToInt32(this.cmbRecipient.SelectedValue) > 0U)
        supplier.Recipient = (BaseOrg) this.cmbRecipient.SelectedItem;
      if ((uint) Convert.ToInt32(this.cmbPerfomer.SelectedValue) > 0U)
        supplier.Perfomer = (BaseOrg) this.cmbPerfomer.SelectedItem;
      if (Convert.ToInt32(this.cmbRecipient.SelectedValue) != 0 && (uint) Convert.ToInt32(this.cmbPerfomer.SelectedValue) > 0U)
        supplier.IsEdit = true;
      IList<Supplier> supplierList = (IList<Supplier>) new List<Supplier>();
      if ((uint) this.dgvSupplier.Rows.Count > 0U)
        supplierList = (IList<Supplier>) (this.dgvSupplier.DataSource as List<Supplier>);
      supplierList.Add(supplier);
      this.dgvSupplier.Columns.Clear();
      this.dgvSupplier.DataSource = (object) null;
      this.dgvSupplier.DataSource = (object) supplierList;
      this.SetViewSupplier();
      try
      {
        if (this.dgvSupplier.Columns["Recipient"] != null && this.dgvSupplier.Columns["Recipient"].Visible)
          this.dgvSupplier.CurrentCell = this.dgvSupplier.Rows[this.dgvSupplier.Rows.Count - 1].Cells["Recipient"];
        else if (this.dgvSupplier.Columns["Perfomer"] != null && this.dgvSupplier.Columns["Perfomer"].Visible)
        {
          this.dgvSupplier.CurrentCell = this.dgvSupplier.Rows[this.dgvSupplier.Rows.Count - 1].Cells["Perfomer"];
        }
        else
        {
          if (this.dgvSupplier.Columns["UName"] == null || !this.dgvSupplier.Columns["UName"].Visible)
            return;
          this.dgvSupplier.CurrentCell = this.dgvSupplier.Rows[this.dgvSupplier.Rows.Count - 1].Cells["UName"];
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void cmbPerfomer_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.LoadSupplier();
    }

    private void dgvSupplier_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsSupplier.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsSupplier.Columns[this.MySettingsSupplier.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsSupplier.Save();
    }

    private void dgvSupplier_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.tsbDelete.Enabled = false;
      this.tsbApplay.Enabled = true;
      this.tsbCancel.Enabled = true;
      ((Supplier) this.dgvSupplier.CurrentRow.DataBoundItem).IsEdit = true;
    }

    private void dgvSupplier_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgvSupplier.CurrentRow == null)
        return;
      Supplier dataBoundItem = (Supplier) this.dgvSupplier.CurrentRow.DataBoundItem;
      dataBoundItem.IsEdit = true;
      if (this.dgvSupplier.CurrentCell.Value != null)
      {
        try
        {
          string name = this.dgvSupplier.Columns[e.ColumnIndex].Name;
          if (!(name == "Recipient"))
          {
            if (name == "Perfomer")
              dataBoundItem.Perfomer = this.session.Get<BaseOrg>((object) Convert.ToInt32(this.dgvSupplier.CurrentRow.Cells["Perfomer"].Value));
          }
          else
            dataBoundItem.Recipient = this.session.Get<BaseOrg>((object) Convert.ToInt32(this.dgvSupplier.CurrentRow.Cells["Recipient"].Value));
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
    }

    private void tsbApplay_Click(object sender, EventArgs e)
    {
      this.dgvSupplier.EndEdit();
      foreach (DataGridViewRow row in (IEnumerable) this.dgvSupplier.Rows)
      {
        try
        {
          if (this.dgvSupplier.Columns["Recipient"] != null && this.dgvSupplier.Columns["Recipient"].Visible)
            this.dgvSupplier.CurrentCell = this.dgvSupplier.Rows[this.dgvSupplier.Rows.Count - 1].Cells["Recipient"];
          else if (this.dgvSupplier.Columns["Perfomer"] != null && this.dgvSupplier.Columns["Perfomer"].Visible)
            this.dgvSupplier.CurrentCell = this.dgvSupplier.Rows[this.dgvSupplier.Rows.Count - 1].Cells["Perfomer"];
          else if (this.dgvSupplier.Columns["UName"] != null && this.dgvSupplier.Columns["UName"].Visible)
            this.dgvSupplier.CurrentCell = this.dgvSupplier.Rows[this.dgvSupplier.Rows.Count - 1].Cells["UName"];
        }
        catch (Exception ex)
        {
        }
        row.Selected = true;
        if (((Supplier) row.DataBoundItem).IsEdit)
          this.SaveSupplier();
        ((Supplier) row.DataBoundItem).IsEdit = false;
      }
      this.tsbAdd.Enabled = true;
      this.tsbDelete.Enabled = true;
      this.tsbApplay.Enabled = false;
      this.tsbCancel.Enabled = false;
      this.LoadSupplier();
    }

    private void tsbSupplier_Click(object sender, EventArgs e)
    {
      this.tsbCmpSupplier.Checked = false;
      this.tsbSupplier.Checked = true;
      if (!KvrplHelper.CheckProxy(32, 1, (Company) null, true))
        this.tsbSupplier.Checked = false;
      else
        this.pnSupplier.BringToFront();
    }

    private void LoadCmbRecipietnsAndCmbPerformer()
    {
      IList<BaseOrg> baseOrgList1 = this.session.CreateQuery("select new BaseOrg(BaseOrgId,NameOrgMin) from BaseOrg where BaseOrgId in (SELECT IDBASEORG FROM Postaver) and BaseOrgId<>0 order by NameOrgMin").List<BaseOrg>();
      baseOrgList1.Insert(0, new BaseOrg(0, ""));
      this.cmbRecipient.DataSource = (object) baseOrgList1;
      this.cmbRecipient.DisplayMember = "NameOrgMin";
      this.cmbRecipient.ValueMember = "BaseOrgId";
      IList<BaseOrg> baseOrgList2 = this.session.CreateQuery("select new BaseOrg(BaseOrgId,NameOrgMin) from BaseOrg where BaseOrgId in (SELECT IDBASEORG FROM Postaver) and BaseOrgId<>0 order by NameOrgMin").List<BaseOrg>();
      baseOrgList2.Insert(0, new BaseOrg(0, ""));
      this.cmbPerfomer.DataSource = (object) baseOrgList2;
      this.cmbPerfomer.DisplayMember = "NameOrgMin";
      this.cmbPerfomer.ValueMember = "BaseOrgId";
    }

    private void dgvSupplier_CellValueChanged(object sender, DataGridViewCellEventArgs e)
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmSupplier));
      this.dsMain = new DataSet();
      this.dtServ = new DataTable();
      this.dataColumn1 = new DataColumn();
      this.dataColumn2 = new DataColumn();
      this.pnBtn = new Panel();
      this.btnExit = new Button();
      this.hp = new HelpProvider();
      this.pnUp = new Panel();
      this.txbPer = new TextBox();
      this.lblPer = new Label();
      this.txbRec = new TextBox();
      this.lblRec = new Label();
      this.cmbCompany = new ComboBox();
      this.lblCompany = new Label();
      this.tsSupplier = new ToolStrip();
      this.tsbSupplier = new ToolStripButton();
      this.tsbCmpSupplier = new ToolStripButton();
      this.pnCmpSupplier = new Panel();
      this.splitContainer1 = new SplitContainer();
      this.dgvBaseOrg = new DataGridView();
      this.pnFiltr = new Panel();
      this.cbPerINN = new CheckBox();
      this.cbINN = new CheckBox();
      this.ucSupplier1 = new UCSupplier();
      this.pnSupplier = new Panel();
      this.dgvSupplier = new DataGridView();
      this.toolStrip1 = new ToolStrip();
      this.tsbAdd = new ToolStripButton();
      this.tsbApplay = new ToolStripButton();
      this.tsbCancel = new ToolStripButton();
      this.tsbDelete = new ToolStripButton();
      this.tsbExit = new ToolStripButton();
      this.gbUp = new GroupBox();
      this.cmbPerfomer = new ComboBox();
      this.lblRecipient = new Label();
      this.cmbRecipient = new ComboBox();
      this.lblPerfomer = new Label();
      this.dsMain.BeginInit();
      this.dtServ.BeginInit();
      this.pnBtn.SuspendLayout();
      this.pnUp.SuspendLayout();
      this.tsSupplier.SuspendLayout();
      this.pnCmpSupplier.SuspendLayout();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      ((ISupportInitialize) this.dgvBaseOrg).BeginInit();
      this.pnFiltr.SuspendLayout();
      this.pnSupplier.SuspendLayout();
      ((ISupportInitialize) this.dgvSupplier).BeginInit();
      this.toolStrip1.SuspendLayout();
      this.gbUp.SuspendLayout();
      this.SuspendLayout();
      this.dsMain.DataSetName = "NewDataSet";
      this.dsMain.Tables.AddRange(new DataTable[1]
      {
        this.dtServ
      });
      this.dtServ.Columns.AddRange(new DataColumn[2]
      {
        this.dataColumn1,
        this.dataColumn2
      });
      this.dtServ.TableName = "Table1";
      this.dataColumn1.ColumnName = "ID";
      this.dataColumn1.DataType = typeof (short);
      this.dataColumn2.ColumnName = "Name";
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(188, 539);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(1050, 40);
      this.pnBtn.TabIndex = 0;
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(961, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(77, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.hp.HelpNamespace = "Help.chm";
      this.pnUp.BackColor = SystemColors.ButtonFace;
      this.pnUp.Controls.Add((Control) this.txbPer);
      this.pnUp.Controls.Add((Control) this.lblPer);
      this.pnUp.Controls.Add((Control) this.txbRec);
      this.pnUp.Controls.Add((Control) this.lblRec);
      this.pnUp.Controls.Add((Control) this.cmbCompany);
      this.pnUp.Controls.Add((Control) this.lblCompany);
      this.pnUp.Dock = DockStyle.Top;
      this.pnUp.Location = new Point(0, 0);
      this.pnUp.Name = "pnUp";
      this.pnUp.Size = new Size(1050, 93);
      this.pnUp.TabIndex = 1;
      this.txbPer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txbPer.Location = new Point(105, 65);
      this.txbPer.Name = "txbPer";
      this.txbPer.Size = new Size(817, 23);
      this.txbPer.TabIndex = 8;
      this.txbPer.KeyUp += new KeyEventHandler(this.txbSupplier_KeyUp);
      this.lblPer.AutoSize = true;
      this.lblPer.Location = new Point(4, 68);
      this.lblPer.Name = "lblPer";
      this.lblPer.Size = new Size(95, 17);
      this.lblPer.TabIndex = 7;
      this.lblPer.Text = "Исполнитель";
      this.txbRec.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txbRec.Location = new Point(105, 37);
      this.txbRec.Name = "txbRec";
      this.txbRec.Size = new Size(817, 23);
      this.txbRec.TabIndex = 6;
      this.txbRec.KeyUp += new KeyEventHandler(this.txbSupplier_KeyUp);
      this.lblRec.AutoSize = true;
      this.lblRec.Location = new Point(4, 40);
      this.lblRec.Name = "lblRec";
      this.lblRec.Size = new Size(87, 17);
      this.lblRec.TabIndex = 5;
      this.lblRec.Text = "Получатель";
      this.cmbCompany.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbCompany.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCompany.FormattingEnabled = true;
      this.cmbCompany.Location = new Point(103, 8);
      this.cmbCompany.Margin = new Padding(4);
      this.cmbCompany.Name = "cmbCompany";
      this.cmbCompany.Size = new Size(819, 24);
      this.cmbCompany.TabIndex = 4;
      this.cmbCompany.SelectionChangeCommitted += new EventHandler(this.cmbCompany_SelectionChangeCommitted);
      this.lblCompany.AutoSize = true;
      this.lblCompany.Location = new Point(4, 11);
      this.lblCompany.Margin = new Padding(4, 0, 4, 0);
      this.lblCompany.Name = "lblCompany";
      this.lblCompany.Size = new Size(74, 17);
      this.lblCompany.TabIndex = 3;
      this.lblCompany.Text = "Компания";
      this.tsSupplier.BackColor = SystemColors.Control;
      this.tsSupplier.Dock = DockStyle.Left;
      this.tsSupplier.Font = new Font("Tahoma", 10f);
      this.tsSupplier.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsbSupplier,
        (ToolStripItem) this.tsbCmpSupplier
      });
      this.tsSupplier.Location = new Point(0, 0);
      this.tsSupplier.Name = "tsSupplier";
      this.tsSupplier.Size = new Size(188, 579);
      this.tsSupplier.TabIndex = 2;
      this.tsSupplier.Text = "Поставщики";
      this.tsbSupplier.Checked = true;
      this.tsbSupplier.CheckState = CheckState.Checked;
      this.tsbSupplier.Image = (Image) Resources.allClasses;
      this.tsbSupplier.ImageScaling = ToolStripItemImageScaling.None;
      this.tsbSupplier.ImageTransparentColor = Color.Magenta;
      this.tsbSupplier.Name = "tsbSupplier";
      this.tsbSupplier.Size = new Size(185, 69);
      this.tsbSupplier.Text = "Поставщики";
      this.tsbSupplier.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tsbSupplier.Click += new EventHandler(this.tsbSupplier_Click);
      this.tsbCmpSupplier.Image = (Image) Resources.documents;
      this.tsbCmpSupplier.ImageScaling = ToolStripItemImageScaling.None;
      this.tsbCmpSupplier.ImageTransparentColor = Color.Magenta;
      this.tsbCmpSupplier.Name = "tsbCmpSupplier";
      this.tsbCmpSupplier.Size = new Size(185, 69);
      this.tsbCmpSupplier.Text = "Поставщики по компаниям";
      this.tsbCmpSupplier.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tsbCmpSupplier.Click += new EventHandler(this.tsbCmpSupplier_Click);
      this.pnCmpSupplier.BackColor = SystemColors.Control;
      this.pnCmpSupplier.Controls.Add((Control) this.splitContainer1);
      this.pnCmpSupplier.Controls.Add((Control) this.pnUp);
      this.pnCmpSupplier.Dock = DockStyle.Fill;
      this.pnCmpSupplier.Location = new Point(188, 0);
      this.pnCmpSupplier.Name = "pnCmpSupplier";
      this.pnCmpSupplier.Size = new Size(1050, 539);
      this.pnCmpSupplier.TabIndex = 3;
      this.splitContainer1.Dock = DockStyle.Fill;
      this.splitContainer1.Location = new Point(0, 93);
      this.splitContainer1.Margin = new Padding(4);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = Orientation.Horizontal;
      this.splitContainer1.Panel1.Controls.Add((Control) this.dgvBaseOrg);
      this.splitContainer1.Panel1.Controls.Add((Control) this.pnFiltr);
      this.splitContainer1.Panel2.Controls.Add((Control) this.ucSupplier1);
      this.splitContainer1.Size = new Size(1050, 446);
      this.splitContainer1.SplitterDistance = 131;
      this.splitContainer1.SplitterWidth = 5;
      this.splitContainer1.TabIndex = 3;
      this.dgvBaseOrg.AllowUserToAddRows = false;
      this.dgvBaseOrg.AllowUserToDeleteRows = false;
      this.dgvBaseOrg.BackgroundColor = Color.AliceBlue;
      this.dgvBaseOrg.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvBaseOrg.Dock = DockStyle.Fill;
      this.dgvBaseOrg.Location = new Point(0, 33);
      this.dgvBaseOrg.Margin = new Padding(4);
      this.dgvBaseOrg.MultiSelect = false;
      this.dgvBaseOrg.Name = "dgvBaseOrg";
      this.dgvBaseOrg.ReadOnly = true;
      this.dgvBaseOrg.Size = new Size(1050, 98);
      this.dgvBaseOrg.TabIndex = 1;
      this.dgvBaseOrg.DataError += new DataGridViewDataErrorEventHandler(this.dgvBaseOrg_DataError);
      this.dgvBaseOrg.SelectionChanged += new EventHandler(this.dgvBaseOrg_SelectionChanged);
      this.pnFiltr.Controls.Add((Control) this.cbPerINN);
      this.pnFiltr.Controls.Add((Control) this.cbINN);
      this.pnFiltr.Dock = DockStyle.Top;
      this.pnFiltr.Location = new Point(0, 0);
      this.pnFiltr.Name = "pnFiltr";
      this.pnFiltr.Size = new Size(1050, 33);
      this.pnFiltr.TabIndex = 0;
      this.cbPerINN.AutoSize = true;
      this.cbPerINN.Location = new Point(184, 3);
      this.cbPerINN.Name = "cbPerINN";
      this.cbPerINN.Size = new Size(160, 21);
      this.cbPerINN.TabIndex = 1;
      this.cbPerINN.Text = "Исполнители с ИНН";
      this.cbPerINN.UseVisualStyleBackColor = true;
      this.cbPerINN.Click += new EventHandler(this.cbINN_Click);
      this.cbINN.AutoSize = true;
      this.cbINN.Location = new Point(12, 3);
      this.cbINN.Name = "cbINN";
      this.cbINN.Size = new Size(152, 21);
      this.cbINN.TabIndex = 0;
      this.cbINN.Text = "Получатели с ИНН";
      this.cbINN.UseVisualStyleBackColor = true;
      this.cbINN.Click += new EventHandler(this.cbINN_Click);
      this.ucSupplier1.Company = (Company) null;
      this.ucSupplier1.CurOrg = (Supplier) null;
      this.ucSupplier1.Dock = DockStyle.Fill;
      this.ucSupplier1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.ucSupplier1.Location = new Point(0, 0);
      this.ucSupplier1.Margin = new Padding(5);
      this.ucSupplier1.Name = "ucSupplier1";
      this.ucSupplier1.Size = new Size(1050, 310);
      this.ucSupplier1.TabIndex = 2;
      this.pnSupplier.BackColor = SystemColors.Control;
      this.pnSupplier.Controls.Add((Control) this.dgvSupplier);
      this.pnSupplier.Controls.Add((Control) this.toolStrip1);
      this.pnSupplier.Controls.Add((Control) this.gbUp);
      this.pnSupplier.Dock = DockStyle.Fill;
      this.pnSupplier.Location = new Point(188, 0);
      this.pnSupplier.Name = "pnSupplier";
      this.pnSupplier.Size = new Size(1050, 539);
      this.pnSupplier.TabIndex = 3;
      this.dgvSupplier.BackgroundColor = Color.AliceBlue;
      this.dgvSupplier.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvSupplier.Dock = DockStyle.Fill;
      this.dgvSupplier.Location = new Point(0, 101);
      this.dgvSupplier.Name = "dgvSupplier";
      this.dgvSupplier.Size = new Size(1050, 438);
      this.dgvSupplier.TabIndex = 4;
      this.dgvSupplier.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvSupplier_CellBeginEdit);
      this.dgvSupplier.CellEndEdit += new DataGridViewCellEventHandler(this.dgvSupplier_CellEndEdit);
      this.dgvSupplier.CellValueChanged += new DataGridViewCellEventHandler(this.dgvSupplier_CellValueChanged);
      this.dgvSupplier.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvSupplier_ColumnWidthChanged);
      this.dgvSupplier.DataError += new DataGridViewDataErrorEventHandler(this.dgvBaseOrg_DataError);
      this.toolStrip1.Font = new Font("Tahoma", 10f);
      this.toolStrip1.Items.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.tsbAdd,
        (ToolStripItem) this.tsbApplay,
        (ToolStripItem) this.tsbCancel,
        (ToolStripItem) this.tsbDelete,
        (ToolStripItem) this.tsbExit
      });
      this.toolStrip1.LayoutStyle = ToolStripLayoutStyle.Flow;
      this.toolStrip1.Location = new Point(0, 77);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Padding = new Padding(0, 0, 2, 0);
      this.toolStrip1.Size = new Size(1050, 24);
      this.toolStrip1.TabIndex = 3;
      this.toolStrip1.Text = "toolStrip1";
      this.tsbAdd.Image = (Image) Resources.add_var;
      this.tsbAdd.ImageTransparentColor = Color.Magenta;
      this.tsbAdd.Name = "tsbAdd";
      this.tsbAdd.Size = new Size(91, 21);
      this.tsbAdd.Text = "Добавить";
      this.tsbAdd.Click += new EventHandler(this.tsbAdd_Click);
      this.tsbApplay.Enabled = false;
      this.tsbApplay.Image = (Image) Resources.Applay_var;
      this.tsbApplay.ImageTransparentColor = Color.Magenta;
      this.tsbApplay.Name = "tsbApplay";
      this.tsbApplay.Size = new Size(99, 21);
      this.tsbApplay.Text = "Сохранить";
      this.tsbApplay.Click += new EventHandler(this.tsbApplay_Click);
      this.tsbCancel.Enabled = false;
      this.tsbCancel.Image = (Image) Resources.undo;
      this.tsbCancel.ImageTransparentColor = Color.Magenta;
      this.tsbCancel.Name = "tsbCancel";
      this.tsbCancel.Size = new Size(77, 21);
      this.tsbCancel.Text = "Отмена";
      this.tsbCancel.Click += new EventHandler(this.tsbCancel_Click);
      this.tsbDelete.Image = (Image) Resources.delete_var;
      this.tsbDelete.ImageTransparentColor = Color.Magenta;
      this.tsbDelete.Name = "tsbDelete";
      this.tsbDelete.Size = new Size(82, 21);
      this.tsbDelete.Text = "Удалить";
      this.tsbDelete.Click += new EventHandler(this.tsbDelete_Click);
      this.tsbExit.Image = (Image) Resources.Exit;
      this.tsbExit.ImageTransparentColor = Color.Magenta;
      this.tsbExit.Name = "tsbExit";
      this.tsbExit.Size = new Size(70, 21);
      this.tsbExit.Text = "Выход";
      this.tsbExit.Visible = false;
      this.gbUp.Controls.Add((Control) this.cmbPerfomer);
      this.gbUp.Controls.Add((Control) this.lblRecipient);
      this.gbUp.Controls.Add((Control) this.cmbRecipient);
      this.gbUp.Controls.Add((Control) this.lblPerfomer);
      this.gbUp.Dock = DockStyle.Top;
      this.gbUp.Location = new Point(0, 0);
      this.gbUp.Name = "gbUp";
      this.gbUp.Size = new Size(1050, 77);
      this.gbUp.TabIndex = 1;
      this.gbUp.TabStop = false;
      this.cmbPerfomer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbPerfomer.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbPerfomer.FormattingEnabled = true;
      this.cmbPerfomer.Location = new Point(115, 45);
      this.cmbPerfomer.Name = "cmbPerfomer";
      this.cmbPerfomer.Size = new Size(923, 24);
      this.cmbPerfomer.TabIndex = 3;
      this.cmbPerfomer.SelectionChangeCommitted += new EventHandler(this.cmbPerfomer_SelectionChangeCommitted);
      this.lblRecipient.AutoSize = true;
      this.lblRecipient.Location = new Point(9, 19);
      this.lblRecipient.Name = "lblRecipient";
      this.lblRecipient.Size = new Size(87, 17);
      this.lblRecipient.TabIndex = 0;
      this.lblRecipient.Text = "Получатель";
      this.cmbRecipient.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbRecipient.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbRecipient.FormattingEnabled = true;
      this.cmbRecipient.Location = new Point(115, 15);
      this.cmbRecipient.Name = "cmbRecipient";
      this.cmbRecipient.Size = new Size(923, 24);
      this.cmbRecipient.TabIndex = 2;
      this.cmbRecipient.SelectionChangeCommitted += new EventHandler(this.cmbPerfomer_SelectionChangeCommitted);
      this.lblPerfomer.AutoSize = true;
      this.lblPerfomer.Location = new Point(9, 51);
      this.lblPerfomer.Name = "lblPerfomer";
      this.lblPerfomer.Size = new Size(95, 17);
      this.lblPerfomer.TabIndex = 1;
      this.lblPerfomer.Text = "Исполнитель";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(1238, 579);
      this.Controls.Add((Control) this.pnSupplier);
      this.Controls.Add((Control) this.pnCmpSupplier);
      this.Controls.Add((Control) this.pnBtn);
      this.Controls.Add((Control) this.tsSupplier);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.hp.SetHelpKeyword((Control) this, "kv510.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmSupplier";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Поставщики по услугам";
      this.Shown += new EventHandler(this.FrmSupplier_Shown);
      this.dsMain.EndInit();
      this.dtServ.EndInit();
      this.pnBtn.ResumeLayout(false);
      this.pnUp.ResumeLayout(false);
      this.pnUp.PerformLayout();
      this.tsSupplier.ResumeLayout(false);
      this.tsSupplier.PerformLayout();
      this.pnCmpSupplier.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.dgvBaseOrg).EndInit();
      this.pnFiltr.ResumeLayout(false);
      this.pnFiltr.PerformLayout();
      this.pnSupplier.ResumeLayout(false);
      this.pnSupplier.PerformLayout();
      ((ISupportInitialize) this.dgvSupplier).EndInit();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.gbUp.ResumeLayout(false);
      this.gbUp.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
