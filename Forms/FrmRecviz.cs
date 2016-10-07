// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmRecviz
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
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
  public class FrmRecviz : Form
  {
    private readonly FormStateSaver fss = new FormStateSaver(FrmRecviz.ic);
    private short number = 0;
    private IContainer components = (IContainer) null;
    private static IContainer ic;
    private readonly Period closedPeriod;
    private bool InsertRecord;
    private HomesPhones OldHomesPhones;
    private Company currentCompany;
    private CompanyParam currentParam;
    private bool oldTime;
    private ISession session;
    private Transfer transfer;
    private Panel pnBtn;
    private Button btnExit;
    private Button btnSave;
    private Button btnAdd;
    private Button btnDelete;
    private Button btnOldTime;
    private ContextMenuStrip cmenu;
    private ToolStripMenuItem скопироватьЗаписиВВыбранныеОбъектыToolStripMenuItem;
    private Panel panel1;
    private TabControl tcntrl;
    private TabPage tpMain;
    private Label lblCompanyId;
    private TextBox txtCompanyId;
    private TextBox txtWorkTime;
    private Label lblWorkTime;
    private TextBox txtAddress;
    private Label lblAddress;
    private TextBox txtNameCompany;
    private Label lblCompanyName;
    private TabPage tpParam;
    private DataGridView dgvCmpParam;
    private Label lblKvrCloseValue;
    private Label lblKvrClose;
    private Label lblPastTime;
    private Timer tmr;
    private CheckBox cbArchive;
    private TextBox txtSmallName;
    private Label lblSmallName;
    private Label lblManager;
    private TextBox txtPlace;
    private Label lblPlace;
    private TextBox txtCash;
    private Label lblCash;
    private Label lblName;
    private ToolStripMenuItem удалитьЗаписиИзВыбранныхОбъектовToolStripMenuItem;
    private MonthPicker mpCurrentPeriod;
    private TextBox txtOSZN;
    private Label lblOSZN;
    private HelpProvider hp;
    private Label lblDedit;
    private TextBox txbDedit;
    private Label lblUname;
    private TextBox txbUname;
    private TextBox txbManager;
    private TabPage tpService;
    private DataGridView dgvServAndPhone;
    private DataGridViewTextBoxColumn Dbeg;
    private DataGridViewTextBoxColumn Dend;
    private DataGridViewComboBoxColumn Service;
    private DataGridViewComboBoxColumn Receipt;
    private DataGridViewTextBoxColumn Phone;
    private DataGridViewTextBoxColumn Note;
    private Label lblOhlEnd;
    private TextBox txbOhlEnd;
    private Label lblOhlBeg;
    private TextBox txbOhlBeg;

    public FrmRecviz()
    {
      this.fss.ParentForm = (Form) this;
      this.InitializeComponent();
    }

    public FrmRecviz(Company cmp)
    {
      this.fss.ParentForm = (Form) this;
      this.InitializeComponent();
      this.session = Domain.CurrentSession;
      this.currentCompany = cmp;
      this.closedPeriod = KvrplHelper.GetCmpKvrClose(this.currentCompany, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk);
      this.lblKvrCloseValue.Text = this.closedPeriod.PeriodName.Value.ToString("MM.yyyy");
      this.lblName.Text = this.currentCompany.CompanyName;
      if (!KvrplHelper.CheckProxy(39, 1, this.currentCompany, false))
        this.tpParam.Dispose();
      if (Options.ViewEdit)
        return;
      this.lblUname.Visible = false;
      this.lblDedit.Visible = false;
      this.txbUname.Visible = false;
      this.txbDedit.Visible = false;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void FrmRecviz_Load(object sender, EventArgs e)
    {
      this.LoadCompany();
      this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
    }

    private void LoadCompany()
    {
      this.currentCompany = this.session.Get<Company>((object) this.currentCompany.CompanyId);
      this.transfer = this.session.CreateQuery("from Transfer where Company=:cm").SetParameter<Company>("cm", this.currentCompany).UniqueResult<Transfer>();
      this.txtNameCompany.DataBindings.Add(new Binding("Text", (object) this.currentCompany, "CompanyName"));
      this.txtSmallName.DataBindings.Add(new Binding("Text", (object) this.currentCompany, "CompanySName"));
      this.txbManager.Text = this.currentCompany.Manager.BaseOrgName;
      this.txtAddress.DataBindings.Add(new Binding("Text", (object) this.currentCompany, "Address"));
      this.txtWorkTime.DataBindings.Add(new Binding("Text", (object) this.currentCompany, "WorkTime"));
      this.txtCash.DataBindings.Add(new Binding("Text", (object) this.currentCompany, "WorkTimeCash"));
      this.txtPlace.DataBindings.Add(new Binding("Text", (object) this.currentCompany, "WorkPlaceCash"));
      this.txtCompanyId.DataBindings.Add(new Binding("Text", (object) this.currentCompany, "CompanyId"));
      this.txtOSZN.DataBindings.Add(new Binding("Text", (object) this.currentCompany, "SocOrgId"));
      this.txbUname.DataBindings.Add(new Binding("Text", (object) this.currentCompany, "UName"));
      this.txbDedit.DataBindings.Add(new Binding("Text", (object) this.currentCompany, "DEdit"));
      if (Options.Overhaul)
      {
        this.txbOhlBeg.Text = this.transfer.OhlBeg.ToString();
        this.txbOhlEnd.Text = this.transfer.OhlEnd.ToString();
        this.txbOhlBeg.Enabled = true;
        this.txbOhlEnd.Enabled = true;
      }
      else
      {
        this.lblOhlBeg.Visible = false;
        this.lblOhlEnd.Visible = false;
        this.txbOhlBeg.Visible = false;
        this.txbOhlEnd.Visible = false;
      }
      if (Convert.ToInt32(KvrplHelper.BaseValue(1, Options.Company)) == 35)
      {
        this.lblOhlBeg.Visible = false;
        this.lblOhlEnd.Visible = false;
        this.txbOhlBeg.Visible = false;
        this.txbOhlEnd.Visible = false;
      }
      if (this.IsCompanyEmpty(this.currentCompany))
        this.txtCompanyId.ReadOnly = true;
      this.session.Clear();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      if (this.tcntrl.SelectedTab == this.tpMain & this.currentCompany != null)
      {
        if (this.txtNameCompany.Text == "")
        {
          int num = (int) MessageBox.Show("Вы не ввели название!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        if (this.txtWorkTime.Text == "")
        {
          int num = (int) MessageBox.Show("Вы не ввели время работы!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        if (this.txtAddress.Text == "")
        {
          int num = (int) MessageBox.Show("Вы не ввели адрес!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        if (this.txtSmallName.Text == "")
        {
          int num = (int) MessageBox.Show("Вы не ввели краткое название!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        if (this.txtOSZN.Text.Length > 2)
          this.txtOSZN.Text = this.txtOSZN.Text.Substring(1, 2);
        if (this.currentCompany.WorkTimeCash == null)
          this.currentCompany.WorkTimeCash = "";
        if (this.currentCompany.WorkPlaceCash == null)
          this.currentCompany.WorkPlaceCash = "";
        this.currentCompany.UName = Options.Login;
        this.currentCompany.DEdit = DateTime.Now.Date;
        this.session.SaveOrUpdateCopy((object) this.currentCompany);
        if (Convert.ToInt32(KvrplHelper.BaseValue(1, Options.Company)) == 35)
        {
          this.transfer.OhlBeg = new int?(Convert.ToInt32(this.txbOhlBeg.Text));
          this.transfer.OhlEnd = new int?(Convert.ToInt32(this.txbOhlEnd.Text));
          this.transfer.UName = Options.Login;
          this.transfer.DEdit = DateTime.Now.Date;
          this.session.SaveOrUpdate((object) this.transfer);
        }
        try
        {
          this.session.Flush();
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Невозможно сохранить изменения.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        this.txbUname.Text = this.currentCompany.UName;
        this.txbDedit.Text = this.currentCompany.DEdit.ToShortDateString();
      }
      if (this.tcntrl.SelectedTab == this.tpParam)
      {
        if (!KvrplHelper.CheckProxy(39, 2, this.currentCompany, true))
          return;
        this.btnAdd.Enabled = true;
        this.btnDelete.Enabled = true;
        this.btnSave.Enabled = false;
        this.SaveParam();
      }
      if (this.tcntrl.SelectedTab != this.tpService || !KvrplHelper.CheckProxy(39, 2, this.currentCompany, true))
        return;
      this.btnAdd.Enabled = true;
      this.btnDelete.Enabled = true;
      this.btnSave.Enabled = false;
      if (this.SaveServiceAndPhone())
        this.LoadServiceAndPhone();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      this.btnAdd.Enabled = false;
      this.btnDelete.Enabled = false;
      this.InsertRecord = true;
      if (this.tcntrl.SelectedTab == this.tpMain)
      {
        this.Clear();
        this.session = Domain.CurrentSession;
        IList list = this.session.CreateQuery("select max(CompanyId) from Company").List();
        if (list != null)
          this.number = Convert.ToInt16(list[0]);
        this.number = (short) ((int) this.number + 1);
        this.currentCompany = new Company(this.number, this.currentCompany.Raion);
        this.session.Clear();
        this.LoadCompany();
      }
      if (this.tcntrl.SelectedTab == this.tpParam)
      {
        if (!KvrplHelper.CheckProxy(39, 2, this.currentCompany, true))
          return;
        this.InsertParam();
      }
      if (this.tcntrl.SelectedTab == this.tpService)
      {
        if (!KvrplHelper.CheckProxy(39, 2, this.currentCompany, true))
          return;
        this.InsertServiceAndPhones(false);
      }
      this.btnAdd.Enabled = false;
      this.btnDelete.Enabled = false;
      this.btnSave.Enabled = true;
    }

    private void tcntrl_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.tcntrl.SelectedTab == this.tpMain)
      {
        this.cbArchive.Visible = false;
        this.btnOldTime.Enabled = false;
        this.btnAdd.Enabled = false;
        this.btnDelete.Enabled = false;
        this.btnSave.Enabled = false;
      }
      if (this.tcntrl.SelectedTab == this.tpParam)
      {
        this.cbArchive.Visible = true;
        this.btnOldTime.Enabled = true;
        this.btnAdd.Enabled = true;
        this.btnDelete.Enabled = true;
        this.btnSave.Enabled = false;
        this.LoadList();
      }
      if (this.tcntrl.SelectedTab != this.tpService)
        return;
      this.cbArchive.Visible = true;
      this.btnOldTime.Enabled = true;
      this.btnAdd.Enabled = true;
      this.btnDelete.Enabled = true;
      this.btnSave.Enabled = false;
      this.LoadServiceAndPhone();
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      if (this.tcntrl.SelectedTab == this.tpMain)
      {
        if (this.IsCompanyEmpty(this.currentCompany))
        {
          int num = (int) MessageBox.Show("К участку привязан список домов.Удаление невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return;
        }
        if (MessageBox.Show("Вы уверены, что хотите удалить это домоуправление?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
        {
          try
          {
            this.session.Delete((object) this.currentCompany);
            this.session.Flush();
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Невозможно удалить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
          this.session.Clear();
          this.Clear();
          this.txtCompanyId.Text = "";
          this.currentCompany = (Company) null;
        }
      }
      if (this.tcntrl.SelectedTab == this.tpParam && this.dgvCmpParam.RowCount > 0 && this.dgvCmpParam.CurrentRow.Index >= 0)
      {
        if (!KvrplHelper.CheckProxy(39, 2, this.currentCompany, true))
          return;
        this.DeleteParam();
      }
      if (this.tcntrl.SelectedTab != this.tpService || this.dgvServAndPhone.RowCount <= 0 || this.dgvServAndPhone.CurrentRow.Index < 0 || !KvrplHelper.CheckProxy(39, 2, this.currentCompany, true) || !this.DeleteServAndPhone())
        return;
      this.LoadServiceAndPhone();
    }

    private bool IsCompanyEmpty(Company cmp)
    {
      this.session = Domain.CurrentSession;
      IList list = this.session.CreateCriteria(typeof (Home)).Add((ICriterion) Restrictions.Eq("Company", (object) cmp)).List();
      this.session.Clear();
      return (uint) list.Count > 0U;
    }

    private void Clear()
    {
      this.txtNameCompany.DataBindings.Clear();
      this.txtSmallName.DataBindings.Clear();
      this.txtAddress.DataBindings.Clear();
      this.txtCompanyId.DataBindings.Clear();
      this.txtWorkTime.DataBindings.Clear();
      this.txtCash.DataBindings.Clear();
      this.txtPlace.DataBindings.Clear();
      this.txtSmallName.Text = "";
      this.txtAddress.Text = "";
      this.txtNameCompany.Text = "";
      this.txtWorkTime.Text = "";
      this.txtCash.Text = "";
      this.txtSmallName.Text = "";
      this.txtPlace.Text = "";
      this.txtOSZN.Text = "";
      this.txbUname.Text = "";
      this.txbDedit.Text = "";
    }

    private void скопироватьЗаписиВВыбранныеОбъектыToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.dgvCmpParam.Rows.Count > 0 && this.dgvCmpParam.CurrentRow.Index >= 0 && this.tcntrl.SelectedTab == this.tpParam)
      {
        FrmChooseObject frmChooseObject = new FrmChooseObject((CompanyParam) this.dgvCmpParam.Rows[this.dgvCmpParam.CurrentRow.Index].DataBoundItem);
        frmChooseObject.Save = true;
        int num = (int) frmChooseObject.ShowDialog();
        frmChooseObject.Dispose();
      }
      if (this.dgvServAndPhone.Rows.Count <= 0 || this.dgvServAndPhone.CurrentRow.Index < 0 || this.tcntrl.SelectedTab != this.tpService)
        return;
      FrmChooseObject frmChooseObject1 = new FrmChooseObject((HomesPhones) this.dgvServAndPhone.Rows[this.dgvServAndPhone.CurrentRow.Index].DataBoundItem, true);
      frmChooseObject1.Save = true;
      int num1 = (int) frmChooseObject1.ShowDialog();
      frmChooseObject1.Dispose();
      this.LoadServiceAndPhone();
    }

    private void dtmpCurrentPeriod_ValueChanged(object sender, EventArgs e)
    {
      Options.Period = KvrplHelper.SaveCurrentPeriod(this.mpCurrentPeriod.Value);
      if (this.tcntrl.SelectedTab != this.tpParam)
        return;
      this.LoadList();
    }

    private void tmr_Tick(object sender, EventArgs e)
    {
      if (this.lblPastTime.ForeColor == Color.DarkOrange)
        this.lblPastTime.ForeColor = this.BackColor;
      else
        this.lblPastTime.ForeColor = Color.DarkOrange;
    }

    private void txtNameCompany_Click(object sender, EventArgs e)
    {
      this.btnSave.Enabled = true;
    }

    private void dgvCmpParam_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.btnSave.Enabled = true;
    }

    private void удалитьЗаписиИзВыбранныхОбъектовToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.dgvCmpParam.Rows.Count > 0 && this.dgvCmpParam.CurrentRow.Index >= 0 && this.tcntrl.SelectedTab == this.tpParam)
      {
        FrmChooseObject frmChooseObject = new FrmChooseObject((CompanyParam) this.dgvCmpParam.Rows[this.dgvCmpParam.CurrentRow.Index].DataBoundItem);
        int num = (int) frmChooseObject.ShowDialog();
        frmChooseObject.Dispose();
      }
      if (this.dgvServAndPhone.Rows.Count <= 0 || this.dgvServAndPhone.CurrentRow.Index < 0 || this.tcntrl.SelectedTab != this.tpService)
        return;
      FrmChooseObject frmChooseObject1 = new FrmChooseObject((HomesPhones) this.dgvServAndPhone.Rows[this.dgvServAndPhone.CurrentRow.Index].DataBoundItem, false);
      frmChooseObject1.Save = false;
      int num1 = (int) frmChooseObject1.ShowDialog();
      frmChooseObject1.Dispose();
      this.LoadServiceAndPhone();
    }

    private void dgvCmpParam_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.RowIndex == -1 || e.ColumnIndex == -1)
        return;
      ((DataGridView) sender).Rows[e.RowIndex].Selected = true;
      ((DataGridView) sender).CurrentCell = ((DataGridView) sender).Rows[e.RowIndex].Cells[e.ColumnIndex];
    }

    private void dgvCmpParam_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    private void txbOhlBeg_Click(object sender, EventArgs e)
    {
      this.btnSave.Enabled = true;
    }

    private void txbOhlEnd_Click(object sender, EventArgs e)
    {
      this.btnSave.Enabled = true;
    }

    private void LoadList()
    {
      this.btnSave.Enabled = false;
      this.btnAdd.Enabled = true;
      this.btnDelete.Enabled = true;
      int index = 0;
      if (this.dgvCmpParam.Rows.Count > 0)
        index = this.dgvCmpParam.CurrentRow.Index;
      this.dgvCmpParam.Columns.Clear();
      this.dgvCmpParam.DataSource = (object) null;
      this.session = Domain.CurrentSession;
      IList<CompanyParam> companyParamList1 = (IList<CompanyParam>) new List<CompanyParam>();
      IList<CompanyParam> companyParamList2;
      if (!this.oldTime)
      {
        string str = "";
        if (!this.cbArchive.Checked)
          str = " and cp.DEnd >= '{1}'";
        companyParamList2 = this.session.CreateQuery(string.Format("select cp from CompanyParam cp,Param p,Period per where cp.Param=p and cp.Period=per and cp.Company.CompanyId={0} and per.PeriodId=0 " + str + " and p.Areal=0 and Param_type in (0,1) order by p.ParamId,DBeg", (object) this.currentCompany.CompanyId, (object) KvrplHelper.DateToBaseFormat(this.closedPeriod.PeriodName.Value.AddMonths(1)))).List<CompanyParam>();
      }
      else
        companyParamList2 = this.session.CreateQuery(string.Format("select cp from CompanyParam cp,Param p,Period per where cp.Param=p and cp.Period=per and cp.Company.CompanyId={0} and per.PeriodId={1} and p.Areal=0 and Param_type in (0,1) order by p.ParamId,DBeg", (object) this.currentCompany.CompanyId, (object) Options.Period.PeriodId)).List<CompanyParam>();
      this.dgvCmpParam.DataSource = (object) companyParamList2;
      this.SetView();
      if (index <= this.dgvCmpParam.Rows.Count - 1)
        this.dgvCmpParam.CurrentCell = this.dgvCmpParam.Rows[index].Cells[0];
      else if (this.dgvCmpParam.Rows.Count > 0)
        this.dgvCmpParam.CurrentCell = this.dgvCmpParam.Rows[this.dgvCmpParam.Rows.Count - 1].Cells[0];
      this.session.Clear();
      this.dgvCmpParam.Focus();
    }

    private void SetView()
    {
      IList<Param> objList1 = (IList<Param>) new List<Param>();
      IList<Param> objList2;
      if (!this.oldTime)
        objList2 = this.session.CreateCriteria(typeof (Param)).Add((ICriterion) Restrictions.In("Param_type", (ICollection) new short[2]
        {
          (short) 0,
          (short) 1
        })).Add((ICriterion) Restrictions.Eq("Areal", (object) Convert.ToInt16(0))).List<Param>();
      else
        objList2 = this.session.CreateCriteria(typeof (Param)).Add((ICriterion) Restrictions.In("Param_type", (ICollection) new short[2]
        {
          (short) 0,
          (short) 1
        })).Add((ICriterion) Restrictions.Not((ICriterion) Restrictions.In("ParamId", (ICollection) new short[3]
        {
          (short) 206,
          (short) 207,
          (short) 208
        }))).Add((ICriterion) Restrictions.Eq("Areal", (object) Convert.ToInt16(0))).List<Param>();
      KvrplHelper.AddMaskDateColumn(this.dgvCmpParam, 0, "Дата начала", "DBeg");
      KvrplHelper.AddMaskDateColumn(this.dgvCmpParam, 1, "Дата окончания", "DEnd");
      if (objList2.Count > 0)
        KvrplHelper.AddComboBoxColumn(this.dgvCmpParam, 2, (IList) objList2, "ParamId", "ParamName", "Параметр", "ParamName", 250, 250);
      else
        KvrplHelper.AddComboBoxColumn(this.dgvCmpParam, 2, (IList) null, (string) null, (string) null, "Параметр", "ParamName", 250, 250);
      KvrplHelper.AddTextBoxColumn(this.dgvCmpParam, 3, "Значение", "Value", 90, false);
      KvrplHelper.ViewEdit(this.dgvCmpParam);
      foreach (DataGridViewRow row in (IEnumerable) this.dgvCmpParam.Rows)
      {
        row.Cells["DBeg"].Value = (object) ((CompanyParam) row.DataBoundItem).DBeg;
        row.Cells["DEnd"].Value = (object) ((CompanyParam) row.DataBoundItem).DEnd;
        if (((CompanyParam) row.DataBoundItem).Param != null)
        {
          row.Cells["ParamName"].Value = (object) ((CompanyParam) row.DataBoundItem).Param.ParamId;
          IList<AdmTbl> admTblList = this.session.CreateQuery(string.Format("select s from AdmTbl s,ParamRelation r where s.TableId=r.TableId and r.ParamId={0}", (object) ((CompanyParam) row.DataBoundItem).ParamId)).List<AdmTbl>();
          if (admTblList.Count > 0)
          {
            if (admTblList[0].ClassName != null)
            {
              try
              {
                DataGridViewComboBoxCell viewComboBoxCell = new DataGridViewComboBoxCell();
                viewComboBoxCell.DisplayStyleForCurrentCellOnly = true;
                viewComboBoxCell.ValueMember = admTblList[0].ClassNameId;
                viewComboBoxCell.DisplayMember = admTblList[0].ClassNameName;
                string str = "";
                if (Convert.ToInt32(row.Cells["ParamName"].Value) == 214)
                  str = " where SchemeType=8";
                viewComboBoxCell.DataSource = (object) this.session.CreateQuery(string.Format("from {0}" + str, (object) admTblList[0].ClassName)).List();
                viewComboBoxCell.ValueType = typeof (short);
                row.Cells["Value"] = (DataGridViewCell) viewComboBoxCell;
              }
              catch
              {
              }
            }
            row.Cells["Value"].Value = (object) (short) ((CompanyParam) row.DataBoundItem).ParamValue;
          }
          else
            row.Cells["Value"].Value = (object) ((CompanyParam) row.DataBoundItem).ParamValue;
        }
      }
    }

    private void dgvCmpParam_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (!this.dgvCmpParam.IsCurrentCellDirty)
        return;
      this.dgvCmpParam.CommitEdit(DataGridViewDataErrorContexts.Commit);
      if (this.dgvCmpParam.CurrentCell.ColumnIndex == this.dgvCmpParam.Rows[this.dgvCmpParam.CurrentRow.Index].Cells["ParamName"].ColumnIndex)
      {
        IList<AdmTbl> admTblList = this.session.CreateQuery(string.Format("select s from AdmTbl s,ParamRelation r where s.TableId=r.TableId and r.ParamId={0}", this.dgvCmpParam.Rows[this.dgvCmpParam.CurrentRow.Index].Cells["ParamName"].Value)).List<AdmTbl>();
        if (admTblList.Count > 0)
        {
          if (admTblList[0].ClassName != null)
          {
            try
            {
              DataGridViewComboBoxCell viewComboBoxCell = new DataGridViewComboBoxCell();
              viewComboBoxCell.DisplayStyleForCurrentCellOnly = true;
              viewComboBoxCell.ValueMember = admTblList[0].ClassNameId;
              viewComboBoxCell.DisplayMember = admTblList[0].ClassNameName;
              string str = "";
              if (Convert.ToInt32(this.dgvCmpParam.Rows[this.dgvCmpParam.CurrentRow.Index].Cells["ParamName"].Value) == 214)
                str = " where SchemeType=8";
              viewComboBoxCell.DataSource = (object) this.session.CreateQuery(string.Format("from {0}" + str, (object) admTblList[0].ClassName)).List();
              viewComboBoxCell.ValueType = typeof (short);
              this.dgvCmpParam.CurrentRow.Cells["Value"] = (DataGridViewCell) viewComboBoxCell;
            }
            catch (Exception ex)
            {
              KvrplHelper.WriteLog(ex, (LsClient) null);
            }
          }
        }
        else
          this.dgvCmpParam.Rows[this.dgvCmpParam.CurrentRow.Index].Cells["Value"] = (DataGridViewCell) new DataGridViewTextBoxCell();
      }
    }

    private void button1_Click_1(object sender, EventArgs e)
    {
      if (!this.oldTime)
      {
        this.lblPastTime.Visible = true;
        this.btnOldTime.BackColor = Color.DarkOrange;
        this.oldTime = true;
        this.tmr.Start();
      }
      else
      {
        this.tmr.Stop();
        this.lblPastTime.Visible = false;
        this.btnOldTime.BackColor = this.pnBtn.BackColor;
        this.oldTime = false;
      }
      this.LoadList();
    }

    private void dgvCmpParam_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (this.dgvCmpParam.DataSource == null)
        return;
      DataGridViewRow row = (sender as DataGridView).Rows[e.RowIndex];
      DateTime dbeg = ((CompanyParam) row.DataBoundItem).DBeg;
      DateTime? periodName = this.closedPeriod.PeriodName;
      DateTime dateTime1 = periodName.Value;
      DateTime dateTime2 = dateTime1.AddMonths(2);
      int num;
      if (dbeg < dateTime2)
      {
        DateTime dend = ((CompanyParam) row.DataBoundItem).DEnd;
        periodName = this.closedPeriod.PeriodName;
        dateTime1 = periodName.Value;
        DateTime dateTime3 = dateTime1.AddMonths(1);
        num = dend >= dateTime3 ? 1 : 0;
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

    private void InsertParam()
    {
      CompanyParam companyParam1 = new CompanyParam();
      companyParam1.Company = this.currentCompany;
      if (!this.oldTime)
      {
        DateTime dateTime1 = this.closedPeriod.PeriodName.Value.AddMonths(1);
        DateTime? periodName = Options.Period.PeriodName;
        DateTime dateTime2 = periodName.Value;
        if (dateTime1 <= dateTime2)
        {
          CompanyParam companyParam2 = companyParam1;
          periodName = Options.Period.PeriodName;
          DateTime dateTime3 = periodName.Value;
          companyParam2.DBeg = dateTime3;
        }
        else
        {
          CompanyParam companyParam2 = companyParam1;
          periodName = this.closedPeriod.PeriodName;
          DateTime dateTime3 = periodName.Value.AddMonths(1);
          companyParam2.DBeg = dateTime3;
        }
        companyParam1.DEnd = Convert.ToDateTime("31.12.2999");
      }
      else
      {
        CompanyParam companyParam2 = companyParam1;
        DateTime? periodName = this.closedPeriod.PeriodName;
        DateTime dateTime1 = periodName.Value;
        companyParam2.DBeg = dateTime1;
        CompanyParam companyParam3 = companyParam1;
        periodName = this.closedPeriod.PeriodName;
        DateTime dateTime2 = periodName.Value.AddMonths(1).AddDays(-1.0);
        companyParam3.DEnd = dateTime2;
      }
      IList<CompanyParam> companyParamList = (IList<CompanyParam>) new List<CompanyParam>();
      if ((uint) this.dgvCmpParam.Rows.Count > 0U)
        companyParamList = (IList<CompanyParam>) (this.dgvCmpParam.DataSource as List<CompanyParam>);
      companyParamList.Add(companyParam1);
      this.dgvCmpParam.Columns.Clear();
      this.dgvCmpParam.DataSource = (object) null;
      this.dgvCmpParam.DataSource = (object) companyParamList;
      this.SetView();
      this.dgvCmpParam.Rows[this.dgvCmpParam.Rows.Count - 1].Selected = true;
    }

    private void SaveParam()
    {
      if (this.dgvCmpParam.Rows.Count <= 0 || this.dgvCmpParam.CurrentRow == null)
        return;
      try
      {
        this.currentParam = (CompanyParam) this.dgvCmpParam.CurrentRow.DataBoundItem;
        this.currentParam.DBeg = Convert.ToDateTime(this.dgvCmpParam.CurrentRow.Cells["DBeg"].Value);
        this.currentParam.DEnd = Convert.ToDateTime(this.dgvCmpParam.CurrentRow.Cells["DEnd"].Value);
        this.currentParam.ParamValue = Convert.ToDouble(KvrplHelper.ChangeSeparator(this.dgvCmpParam.CurrentRow.Cells["Value"].Value.ToString()));
      }
      catch
      {
        int num = (int) MessageBox.Show("Проверьте правильность введенных данных и параметра", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      int int32 = Convert.ToInt32(this.dgvCmpParam.CurrentRow.Cells["ParamName"].Value);
      if (this.oldTime && (int32 == 201 || int32 == 202 || (int32 == 203 || int32 == 204) || (int32 == 206 || int32 == 207 || int32 == 208) || int32 == 210))
      {
        int num1 = (int) MessageBox.Show("Этот параметр нельзя менять в прошлом времени", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        DateTime dbeg1 = this.currentParam.DBeg;
        DateTime dend1 = this.currentParam.DEnd;
        if (false)
        {
          int num2 = (int) MessageBox.Show("Дата начала или дата окончания не введена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (this.currentParam.DBeg > this.currentParam.DEnd)
        {
          int num3 = (int) MessageBox.Show("Дата начала больше даты окончания!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          if (this.InsertRecord)
            return;
          this.LoadList();
        }
        else
        {
          DateTime dend2 = this.currentParam.DEnd;
          DateTime? periodName = this.closedPeriod.PeriodName;
          DateTime dateTime1 = periodName.Value.AddMonths(1);
          DateTime dateTime2 = dateTime1.AddDays(-1.0);
          if (dend2 < dateTime2 && !this.oldTime)
          {
            int num3 = (int) MessageBox.Show("Дата окончания должна быть больше закрытого периода!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            if (this.InsertRecord)
              return;
            this.LoadList();
          }
          else
          {
            DateTime dbeg2 = this.currentParam.DBeg;
            periodName = this.closedPeriod.PeriodName;
            dateTime1 = periodName.Value;
            dateTime1 = dateTime1.AddMonths(1);
            DateTime dateTime3 = dateTime1.AddDays(-1.0);
            if (dbeg2 < dateTime3 && !this.oldTime && this.InsertRecord)
            {
              int num3 = (int) MessageBox.Show("Дата начала должна быть больше закрытого периода!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              if (this.InsertRecord)
                return;
              this.LoadList();
            }
            else
            {
              DateTime dbeg3 = this.currentParam.DBeg;
              periodName = this.closedPeriod.PeriodName;
              dateTime1 = periodName.Value;
              DateTime dateTime4 = dateTime1.AddMonths(1);
              int num3;
              if (!(dbeg3 >= dateTime4))
              {
                DateTime dend3 = this.currentParam.DEnd;
                periodName = this.closedPeriod.PeriodName;
                dateTime1 = periodName.Value;
                DateTime dateTime5 = dateTime1.AddMonths(1);
                if (!(dend3 >= dateTime5))
                {
                  num3 = 0;
                  goto label_20;
                }
              }
              num3 = this.oldTime ? 1 : 0;
label_20:
              if (num3 != 0)
              {
                int num4 = (int) MessageBox.Show("Дата начала и дата окончания должны быть меньше закрытого периода!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                if (this.InsertRecord)
                  return;
                this.LoadList();
              }
              else if (Options.Period.PeriodId <= this.closedPeriod.PeriodId && this.oldTime)
              {
                int num4 = (int) MessageBox.Show("Невозможно внести изменения в закрытом периоде!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.LoadList();
              }
              else
              {
                DateTime dbeg4 = this.currentParam.DBeg;
                dateTime1 = DateTime.Now;
                DateTime dateTime5 = dateTime1.AddYears(-3);
                int num4;
                if (!(dbeg4 <= dateTime5))
                {
                  DateTime dbeg5 = this.currentParam.DBeg;
                  dateTime1 = DateTime.Now;
                  DateTime dateTime6 = dateTime1.AddYears(3);
                  num4 = dbeg5 >= dateTime6 ? 1 : 0;
                }
                else
                  num4 = 1;
                if (num4 != 0 && MessageBox.Show("Дата начала отличается от текущей более, чем на 3 года. Продолжить", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                {
                  this.LoadList();
                }
                else
                {
                  this.currentParam.Param = this.session.Get<Param>(this.dgvCmpParam.CurrentRow.Cells["ParamName"].Value);
                  this.currentParam.UName = Options.Login;
                  CompanyParam currentParam = this.currentParam;
                  dateTime1 = DateTime.Now;
                  DateTime date = dateTime1.Date;
                  currentParam.DEdit = date;
                  this.currentParam.Period = this.oldTime ? Options.Period : this.session.Get<Period>((object) 0);
                  try
                  {
                    if (this.InsertRecord)
                    {
                      this.InsertRecord = false;
                      this.session.Save((object) this.currentParam);
                    }
                    else
                    {
                      IList<CompanyParam> companyParamList1 = (IList<CompanyParam>) new List<CompanyParam>();
                      IList<CompanyParam> companyParamList2;
                      if (!this.oldTime)
                      {
                        string str = "";
                        if (!this.cbArchive.Checked)
                          str = " and cp.DEnd >= '{1}'";
                        ISession session = this.session;
                        string format = "select cp from CompanyParam cp,Param p,Period per where cp.Param=p and cp.Period=per and cp.Company.CompanyId={0} and per.PeriodId=0 " + str + " and p.Areal=0 and Param_type in (0,1) order by p.ParamId,DBeg";
                        // ISSUE: variable of a boxed type
                        short companyId = this.currentCompany.CompanyId;
                        periodName = this.closedPeriod.PeriodName;
                        dateTime1 = periodName.Value;
                        string baseFormat = KvrplHelper.DateToBaseFormat(dateTime1.AddMonths(1));
                        string queryString = string.Format(format, (object) companyId, (object) baseFormat);
                        companyParamList2 = session.CreateQuery(queryString).List<CompanyParam>();
                      }
                      else
                        companyParamList2 = this.session.CreateQuery(string.Format("select cp from CompanyParam cp,Param p,Period per where cp.Param=p and cp.Period=per and cp.Company.CompanyId={0} and per.PeriodId={1} and p.Areal=0 and Param_type in (0,1) order by p.ParamId,DBeg", (object) this.currentCompany.CompanyId, (object) Options.Period.PeriodId)).List<CompanyParam>();
                      CompanyParam companyParam = companyParamList2[this.dgvCmpParam.CurrentRow.Index];
                      DateTime dbeg5 = this.currentParam.DBeg;
                      periodName = this.closedPeriod.PeriodName;
                      dateTime1 = periodName.Value;
                      DateTime dateTime6 = dateTime1.AddMonths(1);
                      if (dbeg5 < dateTime6 && !this.oldTime && (this.currentParam.DBeg != companyParam.DBeg || this.currentParam.ParamValue != companyParam.ParamValue || this.currentParam.Param != companyParam.Param))
                      {
                        int num5 = (int) MessageBox.Show("Запись из закрытого периода. Разрешено редактирование только даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        this.LoadList();
                        return;
                      }
                      if (Convert.ToInt32(this.dgvCmpParam.CurrentRow.Cells["ParamName"].Value) == 206 || Convert.ToInt32(this.dgvCmpParam.CurrentRow.Cells["ParamName"].Value) == 207 || Convert.ToInt32(this.dgvCmpParam.CurrentRow.Cells["ParamName"].Value) == 208)
                      {
                        this.currentParam.DBeg = KvrplHelper.FirstDay(this.currentParam.DBeg);
                        this.currentParam.DEnd = KvrplHelper.LastDay(this.currentParam.DEnd);
                      }
                      if (Convert.ToInt32(this.dgvCmpParam.CurrentRow.Cells["ParamName"].Value) == 201 || Convert.ToInt32(this.dgvCmpParam.CurrentRow.Cells["ParamName"].Value) == 204 || Convert.ToInt32(this.dgvCmpParam.CurrentRow.Cells["ParamName"].Value) == 210)
                      {
                        this.currentParam.DBeg = KvrplHelper.FirstDay(this.currentParam.DBeg);
                        this.currentParam.DEnd = KvrplHelper.LastDay(this.currentParam.DEnd);
                      }
                      this.session.GetNamedQuery("UpdateCompanyParam").SetDateTime("dbeg1", this.currentParam.DBeg).SetDateTime("dend", this.currentParam.DEnd).SetParameter<double>("pv", this.currentParam.ParamValue).SetEntity("param1", (object) this.currentParam.Param).SetDateTime("dbeg2", companyParam.DBeg).SetEntity("company", (object) this.currentParam.Company).SetEntity("period", (object) companyParam.Period).SetEntity("param2", (object) companyParam.Param).ExecuteUpdate();
                    }
                    this.session.Flush();
                  }
                  catch (Exception ex)
                  {
                    int num5 = (int) MessageBox.Show("Невозможно сохранить изменения.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    KvrplHelper.WriteLog(ex, (LsClient) null);
                  }
                  this.session.Clear();
                  this.LoadList();
                }
              }
            }
          }
        }
      }
    }

    private void DeleteParam()
    {
      CompanyParam companyParam = new CompanyParam();
      CompanyParam dataBoundItem = this.dgvCmpParam.CurrentRow.DataBoundItem as CompanyParam;
      if (!this.oldTime && (dataBoundItem.DBeg <= this.closedPeriod.PeriodName.Value || dataBoundItem.DEnd <= this.closedPeriod.PeriodName.Value))
      {
        int num1 = (int) MessageBox.Show("Запись принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.oldTime && this.mpCurrentPeriod.Value <= this.closedPeriod.PeriodName.Value)
      {
        int num2 = (int) MessageBox.Show("Запись принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
          return;
        try
        {
          this.session.Delete((object) dataBoundItem);
          this.session.Flush();
        }
        catch (Exception ex)
        {
          int num3 = (int) MessageBox.Show("Невозможно удалить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        this.session.Clear();
        this.LoadList();
      }
    }

    private void cbArchive_Click(object sender, EventArgs e)
    {
      if (this.oldTime)
        return;
      this.LoadList();
    }

    private void LoadServiceAndPhone()
    {
      IList<HomesPhones> homesPhonesList = this.session.CreateQuery(string.Format("from HomesPhones where Company.CompanyId={0} and PhonesServ.Idservice!=8 and Home.IdHome=0 and ClientId=0 order by DBeg desc,PhonesServ.Idservice,Receipt.ReceiptId", (object) this.currentCompany.CompanyId)).List<HomesPhones>();
      this.dgvServAndPhone.AutoGenerateColumns = false;
      this.dgvServAndPhone.DataSource = (object) homesPhonesList;
      this.SetViewServiceAndPhone(false);
    }

    private void SetViewServiceAndPhone(bool ohldoc = false)
    {
      IList<Di_PhonesServ> diPhonesServList1 = (IList<Di_PhonesServ>) new List<Di_PhonesServ>();
      IList<Di_PhonesServ> diPhonesServList2 = this.session.CreateCriteria(typeof (Di_PhonesServ)).Add((ICriterion) Restrictions.Not((ICriterion) Restrictions.Eq("Idservice", (object) 8))).Add((ICriterion) Restrictions.Eq("ViewService", (object) Convert.ToInt16(0))).AddOrder(Order.Asc("Nameservice")).List<Di_PhonesServ>();
      IList<Kvartplata.Classes.Receipt> receiptList = this.session.CreateCriteria(typeof (Kvartplata.Classes.Receipt)).Add((ICriterion) Restrictions.Not((ICriterion) Restrictions.Eq("ReceiptId", (object) Convert.ToInt16(0)))).AddOrder(Order.Asc("ReceiptId")).List<Kvartplata.Classes.Receipt>();
      ((DataGridViewComboBoxColumn) this.dgvServAndPhone.Columns["Service"]).DataSource = (object) diPhonesServList2;
      ((DataGridViewComboBoxColumn) this.dgvServAndPhone.Columns["Service"]).DisplayMember = "Nameservice";
      ((DataGridViewComboBoxColumn) this.dgvServAndPhone.Columns["Service"]).ValueMember = "Idservice";
      ((DataGridViewComboBoxColumn) this.dgvServAndPhone.Columns["Receipt"]).DataSource = (object) receiptList;
      ((DataGridViewComboBoxColumn) this.dgvServAndPhone.Columns["Receipt"]).DisplayMember = "ReceiptName";
      ((DataGridViewComboBoxColumn) this.dgvServAndPhone.Columns["Receipt"]).ValueMember = "ReceiptId";
      this.dgvServAndPhone.Columns["Phone"].HeaderText = "Телефон(Email)";
      this.dgvServAndPhone.Columns["Note"].HeaderText = "Примечание";
      foreach (DataGridViewRow row in (IEnumerable) this.dgvServAndPhone.Rows)
      {
        row.Cells["DBeg"].Value = (object) ((HomesPhones) row.DataBoundItem).DBeg;
        row.Cells["DEnd"].Value = (object) ((HomesPhones) row.DataBoundItem).DEnd;
        if (((HomesPhones) row.DataBoundItem).PhonesServ != null)
          row.Cells["Service"].Value = (object) ((HomesPhones) row.DataBoundItem).PhonesServ.Idservice;
        if (((HomesPhones) row.DataBoundItem).Receipt != null)
          row.Cells["Receipt"].Value = (object) ((HomesPhones) row.DataBoundItem).Receipt.ReceiptId;
      }
    }

    private void InsertServiceAndPhones(bool ohlDoc = false)
    {
      HomesPhones homesPhones = new HomesPhones();
      homesPhones.Home = new Home() { IdHome = 0 };
      homesPhones.Company = this.session.Get<Company>((object) this.currentCompany.CompanyId);
      homesPhones.ClientId = 0;
      homesPhones.DBeg = Options.Period.PeriodName.Value;
      homesPhones.DEnd = Convert.ToDateTime("31.12.2999");
      homesPhones.Receipt = this.session.Get<Kvartplata.Classes.Receipt>((object) Convert.ToInt16(1));
      int num = this.dgvServAndPhone.CurrentRow == null ? 0 : (((HomesPhones) this.dgvServAndPhone.Rows[this.dgvServAndPhone.CurrentRow.Index].DataBoundItem).PhonesServ != null ? 1 : 0);
      homesPhones.PhonesServ = num == 0 ? (Di_PhonesServ) null : this.session.Get<Di_PhonesServ>((object) ((HomesPhones) this.dgvServAndPhone.Rows[this.dgvServAndPhone.CurrentRow.Index].DataBoundItem).PhonesServ.Idservice);
      IList<HomesPhones> homesPhonesList = (IList<HomesPhones>) new List<HomesPhones>();
      if ((uint) this.dgvServAndPhone.Rows.Count > 0U)
        homesPhonesList = (IList<HomesPhones>) (this.dgvServAndPhone.DataSource as List<HomesPhones>);
      homesPhonesList.Add(homesPhones);
      this.dgvServAndPhone.DataSource = (object) null;
      this.dgvServAndPhone.DataSource = (object) homesPhonesList;
      this.InsertRecord = true;
      this.SetViewServiceAndPhone(ohlDoc);
    }

    private bool SaveServiceAndPhone()
    {
      this.session.Clear();
      if (this.dgvServAndPhone.CurrentRow == null)
        return true;
      DataGridViewRow dataGridViewRow = new DataGridViewRow();
      DataGridViewRow row = this.dgvServAndPhone.Rows[this.dgvServAndPhone.CurrentRow.Index];
      HomesPhones homesPhones1 = new HomesPhones();
      HomesPhones dataBoundItem = (HomesPhones) row.DataBoundItem;
      if (row.Cells["Service"].Value != null)
      {
        dataBoundItem.PhonesServ = this.session.Get<Di_PhonesServ>(row.Cells["Service"].Value);
        try
        {
          dataBoundItem.DBeg = Convert.ToDateTime(row.Cells["DBeg"].Value);
          dataBoundItem.DEnd = Convert.ToDateTime(row.Cells["DEnd"].Value);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Некорректно введены даты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (dataBoundItem.DBeg > dataBoundItem.DEnd)
        {
          int num = (int) MessageBox.Show("Дата начала больше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        DateTime dbeg = dataBoundItem.DBeg;
        DateTime dateTime1 = KvrplHelper.GetCmpKvrClose(dataBoundItem.Company, 101, 0).PeriodName.Value;
        DateTime dateTime2 = dateTime1.AddMonths(1);
        int num1;
        if (dbeg < dateTime2)
        {
          DateTime dend = dataBoundItem.DEnd;
          dateTime1 = KvrplHelper.GetCmpKvrClose(dataBoundItem.Company, 101, 0).PeriodName.Value;
          dateTime1 = dateTime1.AddMonths(1);
          DateTime dateTime3 = dateTime1.AddDays(-1.0);
          num1 = dend < dateTime3 ? 1 : 0;
        }
        else
          num1 = 0;
        if (num1 != 0)
        {
          int num2 = (int) MessageBox.Show("Невозможно ввести запись в закрытый период", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (row.Cells["Receipt"].Value != null)
          dataBoundItem.Receipt = this.session.Get<Kvartplata.Classes.Receipt>((object) Convert.ToInt16(row.Cells["Receipt"].Value));
        dataBoundItem.Home = this.session.Get<Home>((object) 0);
        dataBoundItem.Phone = row.Cells["Phone"].Value != null ? (string) row.Cells["Phone"].Value : "";
        dataBoundItem.Note = row.Cells["Note"].Value != null ? (string) row.Cells["Note"].Value : "";
        dataBoundItem.UName = Options.Login;
        HomesPhones homesPhones2 = dataBoundItem;
        dateTime1 = DateTime.Now;
        DateTime date = dateTime1.Date;
        homesPhones2.DEdit = date;
        if (this.InsertRecord)
        {
          if (dataBoundItem.Home == null)
          {
            try
            {
              this.session.CreateSQLQuery("insert into hmReceipt (IdService, Phone, Note, DBeg, DEnd, UName, DEdit, Company_id, Client_id, IdHome, Receipt_id) values(:idservice, :phone, :note, :dbeg, :dend,:uname, :dedit,:codeu,:idclient ,:idhome, :rec)").SetParameter<int>("idclient", dataBoundItem.ClientId).SetParameter<int>("idservice", dataBoundItem.PhonesServ.Idservice).SetParameter<string>("phone", dataBoundItem.Phone).SetParameter<string>("note", dataBoundItem.Note).SetParameter<DateTime>("dbeg", dataBoundItem.DBeg).SetParameter<DateTime>("dend", dataBoundItem.DEnd).SetParameter<string>("uname", dataBoundItem.UName).SetParameter<DateTime>("dedit", dataBoundItem.DEdit).SetParameter<short>("codeu", dataBoundItem.Company.CompanyId).SetParameter<int>("idhome", 0).SetParameter<short>("rec", dataBoundItem.Receipt.ReceiptId).ExecuteUpdate();
              this.session.Flush();
            }
            catch (Exception ex)
            {
              this.session.Clear();
              this.session = Domain.CurrentSession;
              int num2 = (int) MessageBox.Show("Не могу сохранить текущую запись", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              KvrplHelper.WriteLog(ex, (LsClient) null);
              return true;
            }
          }
          else
          {
            try
            {
              this.session.Save((object) dataBoundItem);
              this.session.Flush();
            }
            catch (Exception ex)
            {
              this.session.Clear();
              this.session = Domain.CurrentSession;
              int num2 = (int) MessageBox.Show("Не могу сохранить текущую запись", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              KvrplHelper.WriteLog(ex, (LsClient) null);
              return true;
            }
          }
        }
        if (!this.InsertRecord && this.OldHomesPhones != null)
        {
          int val = 0;
          if (dataBoundItem.Home != null)
            val = dataBoundItem.Home.IdHome;
          try
          {
            this.session.CreateSQLQuery("update hmReceipt hp set IdService = :idservice, Phone=:phone, Note=:note, DBeg=:dbeg, DEnd=:dend, UName=:uname, DEdit=:dedit where hp.Company_id=:codeu and hp.IdHome=:idhome and hp.Client_Id=0 and hp.Receipt_id=:rec and hp.Idservice=:oldidservice and hp.DBeg=:olddbeg").SetParameter<int>("idservice", dataBoundItem.PhonesServ.Idservice).SetParameter<string>("phone", dataBoundItem.Phone).SetParameter<string>("note", dataBoundItem.Note).SetParameter<DateTime>("dbeg", dataBoundItem.DBeg).SetParameter<DateTime>("dend", dataBoundItem.DEnd).SetParameter<string>("uname", dataBoundItem.UName).SetParameter<DateTime>("dedit", dataBoundItem.DEdit).SetParameter<short>("codeu", dataBoundItem.Company.CompanyId).SetParameter<int>("idhome", val).SetParameter<short>("rec", dataBoundItem.Receipt.ReceiptId).SetParameter<int>("oldidservice", this.OldHomesPhones.PhonesServ.Idservice).SetParameter<DateTime>("olddbeg", this.OldHomesPhones.DBeg).ExecuteUpdate();
            this.session.Flush();
          }
          catch
          {
            this.session.Clear();
            this.session = Domain.CurrentSession;
            int num2 = (int) MessageBox.Show("Не могу сохранить текущую запись", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return true;
          }
        }
        this.session.Clear();
        this.InsertRecord = false;
        return true;
      }
      int num3 = (int) MessageBox.Show("Не выбрана служба!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private void dgvServAndPhone_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      if (this.dgvServAndPhone.CurrentRow == null)
        return;
      this.OldHomesPhones = new HomesPhones();
      this.OldHomesPhones.Company = ((HomesPhones) this.dgvServAndPhone.Rows[this.dgvServAndPhone.CurrentRow.Index].DataBoundItem).Company;
      this.OldHomesPhones.Home = ((HomesPhones) this.dgvServAndPhone.Rows[this.dgvServAndPhone.CurrentRow.Index].DataBoundItem).Home;
      this.OldHomesPhones.PhonesServ = ((HomesPhones) this.dgvServAndPhone.Rows[this.dgvServAndPhone.CurrentRow.Index].DataBoundItem).PhonesServ;
      this.OldHomesPhones.Phone = ((HomesPhones) this.dgvServAndPhone.Rows[this.dgvServAndPhone.CurrentRow.Index].DataBoundItem).Phone;
      this.OldHomesPhones.Note = ((HomesPhones) this.dgvServAndPhone.Rows[this.dgvServAndPhone.CurrentRow.Index].DataBoundItem).Note;
      this.OldHomesPhones.DBeg = ((HomesPhones) this.dgvServAndPhone.Rows[this.dgvServAndPhone.CurrentRow.Index].DataBoundItem).DBeg;
      this.OldHomesPhones.DEnd = ((HomesPhones) this.dgvServAndPhone.Rows[this.dgvServAndPhone.CurrentRow.Index].DataBoundItem).DEnd;
      this.btnSave.Enabled = true;
    }

    private bool DeleteServAndPhone()
    {
      if (this.dgvServAndPhone.Rows.Count > 0 && this.dgvServAndPhone.CurrentRow.Index >= 0)
      {
        if (MessageBox.Show("Вы уверены, что хотите удалить запись", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
          return false;
        DataGridViewRow dataGridViewRow = new DataGridViewRow();
        DataGridViewRow row = this.dgvServAndPhone.Rows[this.dgvServAndPhone.CurrentRow.Index];
        HomesPhones homesPhones = new HomesPhones();
        HomesPhones dataBoundItem = (HomesPhones) row.DataBoundItem;
        int index = this.dgvServAndPhone.CurrentRow.Index;
        if (true)
        {
          this.session.Clear();
          this.session = Domain.CurrentSession;
          using (ITransaction transaction = this.session.BeginTransaction())
          {
            try
            {
              this.session.Delete((object) dataBoundItem);
              this.session.Flush();
              transaction.Commit();
            }
            catch (Exception ex)
            {
              int num = (int) MessageBox.Show("Не могу удалить текущую запись!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              KvrplHelper.WriteLog(ex, (LsClient) null);
              transaction.Rollback();
              return false;
            }
          }
          this.session.Clear();
        }
      }
      return true;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmRecviz));
      this.pnBtn = new Panel();
      this.btnOldTime = new Button();
      this.btnDelete = new Button();
      this.btnAdd = new Button();
      this.btnSave = new Button();
      this.btnExit = new Button();
      this.cmenu = new ContextMenuStrip(this.components);
      this.скопироватьЗаписиВВыбранныеОбъектыToolStripMenuItem = new ToolStripMenuItem();
      this.удалитьЗаписиИзВыбранныхОбъектовToolStripMenuItem = new ToolStripMenuItem();
      this.panel1 = new Panel();
      this.mpCurrentPeriod = new MonthPicker();
      this.lblName = new Label();
      this.cbArchive = new CheckBox();
      this.lblPastTime = new Label();
      this.lblKvrCloseValue = new Label();
      this.lblKvrClose = new Label();
      this.tcntrl = new TabControl();
      this.tpParam = new TabPage();
      this.dgvCmpParam = new DataGridView();
      this.tpMain = new TabPage();
      this.lblOhlEnd = new Label();
      this.txbOhlEnd = new TextBox();
      this.lblOhlBeg = new Label();
      this.txbOhlBeg = new TextBox();
      this.txbManager = new TextBox();
      this.lblDedit = new Label();
      this.txbDedit = new TextBox();
      this.lblUname = new Label();
      this.txbUname = new TextBox();
      this.txtOSZN = new TextBox();
      this.lblOSZN = new Label();
      this.lblManager = new Label();
      this.txtPlace = new TextBox();
      this.lblPlace = new Label();
      this.txtCash = new TextBox();
      this.lblCash = new Label();
      this.txtSmallName = new TextBox();
      this.lblSmallName = new Label();
      this.lblCompanyId = new Label();
      this.txtCompanyId = new TextBox();
      this.txtWorkTime = new TextBox();
      this.lblWorkTime = new Label();
      this.txtAddress = new TextBox();
      this.lblAddress = new Label();
      this.txtNameCompany = new TextBox();
      this.lblCompanyName = new Label();
      this.tpService = new TabPage();
      this.dgvServAndPhone = new DataGridView();
      this.Dbeg = new DataGridViewTextBoxColumn();
      this.Dend = new DataGridViewTextBoxColumn();
      this.Service = new DataGridViewComboBoxColumn();
      this.Receipt = new DataGridViewComboBoxColumn();
      this.Phone = new DataGridViewTextBoxColumn();
      this.Note = new DataGridViewTextBoxColumn();
      this.tmr = new Timer(this.components);
      this.hp = new HelpProvider();
      this.pnBtn.SuspendLayout();
      this.cmenu.SuspendLayout();
      this.panel1.SuspendLayout();
      this.tcntrl.SuspendLayout();
      this.tpParam.SuspendLayout();
      ((ISupportInitialize) this.dgvCmpParam).BeginInit();
      this.tpMain.SuspendLayout();
      this.tpService.SuspendLayout();
      ((ISupportInitialize) this.dgvServAndPhone).BeginInit();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnOldTime);
      this.pnBtn.Controls.Add((Control) this.btnDelete);
      this.pnBtn.Controls.Add((Control) this.btnAdd);
      this.pnBtn.Controls.Add((Control) this.btnSave);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 450);
      this.pnBtn.Margin = new Padding(4);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(988, 40);
      this.pnBtn.TabIndex = 1;
      this.btnOldTime.Cursor = Cursors.Default;
      this.btnOldTime.Image = (Image) Resources.time_24;
      this.btnOldTime.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnOldTime.Location = new Point(340, 5);
      this.btnOldTime.Name = "btnOldTime";
      this.btnOldTime.Size = new Size(143, 30);
      this.btnOldTime.TabIndex = 4;
      this.btnOldTime.Text = "Прошлое время";
      this.btnOldTime.TextAlign = ContentAlignment.MiddleRight;
      this.btnOldTime.UseVisualStyleBackColor = true;
      this.btnOldTime.Click += new EventHandler(this.button1_Click_1);
      this.btnDelete.Image = (Image) Resources.minus;
      this.btnDelete.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnDelete.Location = new Point(118, 5);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(100, 30);
      this.btnDelete.TabIndex = 2;
      this.btnDelete.Text = "Удалить";
      this.btnDelete.TextAlign = ContentAlignment.MiddleRight;
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnAdd.Image = (Image) Resources.plus;
      this.btnAdd.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnAdd.Location = new Point(7, 5);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(105, 30);
      this.btnAdd.TabIndex = 0;
      this.btnAdd.Text = "Добавить";
      this.btnAdd.TextAlign = ContentAlignment.MiddleRight;
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnSave.Enabled = false;
      this.btnSave.Image = (Image) Resources.Tick;
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.Location = new Point(224, 5);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(110, 30);
      this.btnSave.TabIndex = 1;
      this.btnSave.Text = "Сохранить";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(892, 5);
      this.btnExit.Margin = new Padding(4);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(83, 30);
      this.btnExit.TabIndex = 3;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.button1_Click);
      this.cmenu.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.скопироватьЗаписиВВыбранныеОбъектыToolStripMenuItem,
        (ToolStripItem) this.удалитьЗаписиИзВыбранныхОбъектовToolStripMenuItem
      });
      this.cmenu.Name = "contextMenuStrip1";
      this.cmenu.Size = new Size(314, 48);
      this.скопироватьЗаписиВВыбранныеОбъектыToolStripMenuItem.Image = (Image) Resources.add_var;
      this.скопироватьЗаписиВВыбранныеОбъектыToolStripMenuItem.Name = "скопироватьЗаписиВВыбранныеОбъектыToolStripMenuItem";
      this.скопироватьЗаписиВВыбранныеОбъектыToolStripMenuItem.Size = new Size(313, 22);
      this.скопироватьЗаписиВВыбранныеОбъектыToolStripMenuItem.Text = "Скопировать записи в выбранные объекты";
      this.скопироватьЗаписиВВыбранныеОбъектыToolStripMenuItem.Click += new EventHandler(this.скопироватьЗаписиВВыбранныеОбъектыToolStripMenuItem_Click);
      this.удалитьЗаписиИзВыбранныхОбъектовToolStripMenuItem.Image = (Image) Resources.minus;
      this.удалитьЗаписиИзВыбранныхОбъектовToolStripMenuItem.Name = "удалитьЗаписиИзВыбранныхОбъектовToolStripMenuItem";
      this.удалитьЗаписиИзВыбранныхОбъектовToolStripMenuItem.Size = new Size(313, 22);
      this.удалитьЗаписиИзВыбранныхОбъектовToolStripMenuItem.Text = "Удалить записи из выбранных объектов";
      this.удалитьЗаписиИзВыбранныхОбъектовToolStripMenuItem.Click += new EventHandler(this.удалитьЗаписиИзВыбранныхОбъектовToolStripMenuItem_Click);
      this.panel1.Controls.Add((Control) this.mpCurrentPeriod);
      this.panel1.Controls.Add((Control) this.lblName);
      this.panel1.Controls.Add((Control) this.cbArchive);
      this.panel1.Controls.Add((Control) this.lblPastTime);
      this.panel1.Controls.Add((Control) this.lblKvrCloseValue);
      this.panel1.Controls.Add((Control) this.lblKvrClose);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(988, 55);
      this.panel1.TabIndex = 2;
      this.mpCurrentPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.mpCurrentPeriod.CustomFormat = "MMMM yyyy";
      this.mpCurrentPeriod.Format = DateTimePickerFormat.Custom;
      this.mpCurrentPeriod.Location = new Point(835, 4);
      this.mpCurrentPeriod.Name = "mpCurrentPeriod";
      this.mpCurrentPeriod.OldMonth = 0;
      this.mpCurrentPeriod.ShowUpDown = true;
      this.mpCurrentPeriod.Size = new Size(140, 22);
      this.mpCurrentPeriod.TabIndex = 18;
      this.mpCurrentPeriod.ValueChanged += new EventHandler(this.dtmpCurrentPeriod_ValueChanged);
      this.lblName.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblName.Location = new Point(4, 4);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(491, 32);
      this.lblName.TabIndex = 17;
      this.lblName.Text = "label1";
      this.cbArchive.AutoSize = true;
      this.cbArchive.Location = new Point(2, 35);
      this.cbArchive.Name = "cbArchive";
      this.cbArchive.Size = new Size(66, 20);
      this.cbArchive.TabIndex = 16;
      this.cbArchive.Text = "Архив";
      this.cbArchive.UseVisualStyleBackColor = true;
      this.cbArchive.Click += new EventHandler(this.cbArchive_Click);
      this.lblPastTime.AutoSize = true;
      this.lblPastTime.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblPastTime.ForeColor = Color.DarkOrange;
      this.lblPastTime.Location = new Point(74, 36);
      this.lblPastTime.Name = "lblPastTime";
      this.lblPastTime.Size = new Size(199, 16);
      this.lblPastTime.TabIndex = 15;
      this.lblPastTime.Text = "Режим прошлого времени";
      this.lblPastTime.Visible = false;
      this.lblKvrCloseValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblKvrCloseValue.AutoSize = true;
      this.lblKvrCloseValue.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblKvrCloseValue.Location = new Point(910, 36);
      this.lblKvrCloseValue.Name = "lblKvrCloseValue";
      this.lblKvrCloseValue.Size = new Size(43, 16);
      this.lblKvrCloseValue.TabIndex = 14;
      this.lblKvrCloseValue.Text = "закр";
      this.lblKvrClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblKvrClose.AutoSize = true;
      this.lblKvrClose.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblKvrClose.Location = new Point(765, 36);
      this.lblKvrClose.Name = "lblKvrClose";
      this.lblKvrClose.Size = new Size(139, 16);
      this.lblKvrClose.TabIndex = 13;
      this.lblKvrClose.Text = "Закрытый период";
      this.tcntrl.Controls.Add((Control) this.tpParam);
      this.tcntrl.Controls.Add((Control) this.tpMain);
      this.tcntrl.Controls.Add((Control) this.tpService);
      this.tcntrl.Dock = DockStyle.Fill;
      this.tcntrl.Location = new Point(0, 55);
      this.tcntrl.Name = "tcntrl";
      this.tcntrl.SelectedIndex = 0;
      this.tcntrl.Size = new Size(988, 395);
      this.tcntrl.TabIndex = 3;
      this.tcntrl.SelectedIndexChanged += new EventHandler(this.tcntrl_SelectedIndexChanged);
      this.tpParam.Controls.Add((Control) this.dgvCmpParam);
      this.tpParam.Location = new Point(4, 25);
      this.tpParam.Name = "tpParam";
      this.tpParam.Padding = new Padding(3);
      this.tpParam.Size = new Size(980, 366);
      this.tpParam.TabIndex = 1;
      this.tpParam.Text = "Параметры";
      this.tpParam.UseVisualStyleBackColor = true;
      this.dgvCmpParam.BackgroundColor = Color.AliceBlue;
      this.dgvCmpParam.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvCmpParam.ContextMenuStrip = this.cmenu;
      this.dgvCmpParam.Dock = DockStyle.Fill;
      this.dgvCmpParam.Location = new Point(3, 3);
      this.dgvCmpParam.Name = "dgvCmpParam";
      this.dgvCmpParam.Size = new Size(974, 360);
      this.dgvCmpParam.TabIndex = 0;
      this.dgvCmpParam.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvCmpParam_CellBeginEdit);
      this.dgvCmpParam.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvCmpParam_CellFormatting);
      this.dgvCmpParam.CellMouseDown += new DataGridViewCellMouseEventHandler(this.dgvCmpParam_CellMouseDown);
      this.dgvCmpParam.CurrentCellDirtyStateChanged += new EventHandler(this.dgvCmpParam_CurrentCellDirtyStateChanged);
      this.dgvCmpParam.DataError += new DataGridViewDataErrorEventHandler(this.dgvCmpParam_DataError);
      this.tpMain.Controls.Add((Control) this.lblOhlEnd);
      this.tpMain.Controls.Add((Control) this.txbOhlEnd);
      this.tpMain.Controls.Add((Control) this.lblOhlBeg);
      this.tpMain.Controls.Add((Control) this.txbOhlBeg);
      this.tpMain.Controls.Add((Control) this.txbManager);
      this.tpMain.Controls.Add((Control) this.lblDedit);
      this.tpMain.Controls.Add((Control) this.txbDedit);
      this.tpMain.Controls.Add((Control) this.lblUname);
      this.tpMain.Controls.Add((Control) this.txbUname);
      this.tpMain.Controls.Add((Control) this.txtOSZN);
      this.tpMain.Controls.Add((Control) this.lblOSZN);
      this.tpMain.Controls.Add((Control) this.lblManager);
      this.tpMain.Controls.Add((Control) this.txtPlace);
      this.tpMain.Controls.Add((Control) this.lblPlace);
      this.tpMain.Controls.Add((Control) this.txtCash);
      this.tpMain.Controls.Add((Control) this.lblCash);
      this.tpMain.Controls.Add((Control) this.txtSmallName);
      this.tpMain.Controls.Add((Control) this.lblSmallName);
      this.tpMain.Controls.Add((Control) this.lblCompanyId);
      this.tpMain.Controls.Add((Control) this.txtCompanyId);
      this.tpMain.Controls.Add((Control) this.txtWorkTime);
      this.tpMain.Controls.Add((Control) this.lblWorkTime);
      this.tpMain.Controls.Add((Control) this.txtAddress);
      this.tpMain.Controls.Add((Control) this.lblAddress);
      this.tpMain.Controls.Add((Control) this.txtNameCompany);
      this.tpMain.Controls.Add((Control) this.lblCompanyName);
      this.tpMain.Location = new Point(4, 25);
      this.tpMain.Name = "tpMain";
      this.tpMain.Padding = new Padding(3);
      this.tpMain.Size = new Size(980, 366);
      this.tpMain.TabIndex = 0;
      this.tpMain.Text = "Основные";
      this.tpMain.UseVisualStyleBackColor = true;
      this.lblOhlEnd.AutoSize = true;
      this.lblOhlEnd.Location = new Point(285, 266);
      this.lblOhlEnd.Name = "lblOhlEnd";
      this.lblOhlEnd.Size = new Size(124, 16);
      this.lblOhlEnd.TabIndex = 41;
      this.lblOhlEnd.Text = "Конечный период";
      this.txbOhlEnd.Enabled = false;
      this.txbOhlEnd.Location = new Point(415, 263);
      this.txbOhlEnd.Name = "txbOhlEnd";
      this.txbOhlEnd.Size = new Size(100, 22);
      this.txbOhlEnd.TabIndex = 40;
      this.txbOhlEnd.Click += new EventHandler(this.txbOhlEnd_Click);
      this.lblOhlBeg.AutoSize = true;
      this.lblOhlBeg.Location = new Point(8, 266);
      this.lblOhlBeg.Name = "lblOhlBeg";
      this.lblOhlBeg.Size = new Size(133, 16);
      this.lblOhlBeg.TabIndex = 39;
      this.lblOhlBeg.Text = "Начальный период";
      this.txbOhlBeg.Enabled = false;
      this.txbOhlBeg.Location = new Point(174, 263);
      this.txbOhlBeg.Name = "txbOhlBeg";
      this.txbOhlBeg.Size = new Size(105, 22);
      this.txbOhlBeg.TabIndex = 38;
      this.txbOhlBeg.Click += new EventHandler(this.txbOhlBeg_Click);
      this.txbManager.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txbManager.Enabled = false;
      this.txbManager.Location = new Point(174, 95);
      this.txbManager.Name = "txbManager";
      this.txbManager.Size = new Size(805, 22);
      this.txbManager.TabIndex = 37;
      this.lblDedit.AutoSize = true;
      this.lblDedit.Location = new Point(257, 298);
      this.lblDedit.Name = "lblDedit";
      this.lblDedit.Size = new Size(152, 16);
      this.lblDedit.TabIndex = 36;
      this.lblDedit.Text = "Дата редактирования";
      this.txbDedit.Enabled = false;
      this.txbDedit.Location = new Point(415, 295);
      this.txbDedit.Name = "txbDedit";
      this.txbDedit.Size = new Size(100, 22);
      this.txbDedit.TabIndex = 35;
      this.lblUname.AutoSize = true;
      this.lblUname.Location = new Point(8, 298);
      this.lblUname.Name = "lblUname";
      this.lblUname.Size = new Size(103, 16);
      this.lblUname.TabIndex = 34;
      this.lblUname.Text = "Пользователь";
      this.txbUname.Enabled = false;
      this.txbUname.Location = new Point(135, 295);
      this.txbUname.Name = "txbUname";
      this.txbUname.Size = new Size(100, 22);
      this.txbUname.TabIndex = 33;
      this.txtOSZN.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtOSZN.Location = new Point(174, 235);
      this.txtOSZN.Name = "txtOSZN";
      this.txtOSZN.Size = new Size(805, 22);
      this.txtOSZN.TabIndex = 32;
      this.txtOSZN.Click += new EventHandler(this.txtNameCompany_Click);
      this.lblOSZN.AutoSize = true;
      this.lblOSZN.Location = new Point(8, 238);
      this.lblOSZN.Name = "lblOSZN";
      this.lblOSZN.Size = new Size(137, 16);
      this.lblOSZN.TabIndex = 31;
      this.lblOSZN.Text = "Код орган-и в ОСЗН";
      this.lblManager.AutoSize = true;
      this.lblManager.Location = new Point(8, 96);
      this.lblManager.Name = "lblManager";
      this.lblManager.Size = new Size(164, 16);
      this.lblManager.TabIndex = 30;
      this.lblManager.Text = "Управляющая компания";
      this.txtPlace.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtPlace.Location = new Point(174, 207);
      this.txtPlace.Name = "txtPlace";
      this.txtPlace.Size = new Size(805, 22);
      this.txtPlace.TabIndex = 6;
      this.txtPlace.Click += new EventHandler(this.txtNameCompany_Click);
      this.lblPlace.AutoSize = true;
      this.lblPlace.Location = new Point(8, 210);
      this.lblPlace.Name = "lblPlace";
      this.lblPlace.Size = new Size(100, 16);
      this.lblPlace.TabIndex = 28;
      this.lblPlace.Text = "Место оплаты";
      this.txtCash.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtCash.Location = new Point(174, 179);
      this.txtCash.Name = "txtCash";
      this.txtCash.Size = new Size(805, 22);
      this.txtCash.TabIndex = 5;
      this.txtCash.Click += new EventHandler(this.txtNameCompany_Click);
      this.lblCash.AutoSize = true;
      this.lblCash.Location = new Point(8, 182);
      this.lblCash.Name = "lblCash";
      this.lblCash.Size = new Size(141, 16);
      this.lblCash.TabIndex = 26;
      this.lblCash.Text = "Время работы кассы";
      this.txtSmallName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSmallName.Location = new Point(174, 65);
      this.txtSmallName.Name = "txtSmallName";
      this.txtSmallName.Size = new Size(805, 22);
      this.txtSmallName.TabIndex = 1;
      this.txtSmallName.Click += new EventHandler(this.txtNameCompany_Click);
      this.lblSmallName.AutoSize = true;
      this.lblSmallName.Location = new Point(8, 68);
      this.lblSmallName.Name = "lblSmallName";
      this.lblSmallName.Size = new Size(129, 16);
      this.lblSmallName.TabIndex = 24;
      this.lblSmallName.Text = "Краткое название";
      this.lblCompanyId.AutoSize = true;
      this.lblCompanyId.Location = new Point(8, 12);
      this.lblCompanyId.Name = "lblCompanyId";
      this.lblCompanyId.Size = new Size(121, 16);
      this.lblCompanyId.TabIndex = 23;
      this.lblCompanyId.Text = "Код организации";
      this.txtCompanyId.Location = new Point(135, 9);
      this.txtCompanyId.Name = "txtCompanyId";
      this.txtCompanyId.Size = new Size(100, 22);
      this.txtCompanyId.TabIndex = 5;
      this.txtWorkTime.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtWorkTime.Location = new Point(174, 151);
      this.txtWorkTime.Name = "txtWorkTime";
      this.txtWorkTime.Size = new Size(805, 22);
      this.txtWorkTime.TabIndex = 4;
      this.txtWorkTime.Click += new EventHandler(this.txtNameCompany_Click);
      this.lblWorkTime.AutoSize = true;
      this.lblWorkTime.Location = new Point(8, 154);
      this.lblWorkTime.Name = "lblWorkTime";
      this.lblWorkTime.Size = new Size(100, 16);
      this.lblWorkTime.TabIndex = 18;
      this.lblWorkTime.Text = "Время работы";
      this.txtAddress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtAddress.Location = new Point(174, 123);
      this.txtAddress.Name = "txtAddress";
      this.txtAddress.Size = new Size(805, 22);
      this.txtAddress.TabIndex = 3;
      this.txtAddress.Click += new EventHandler(this.txtNameCompany_Click);
      this.lblAddress.AutoSize = true;
      this.lblAddress.Location = new Point(8, 126);
      this.lblAddress.Name = "lblAddress";
      this.lblAddress.Size = new Size(48, 16);
      this.lblAddress.TabIndex = 16;
      this.lblAddress.Text = "Адрес";
      this.txtNameCompany.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtNameCompany.Location = new Point(174, 37);
      this.txtNameCompany.Name = "txtNameCompany";
      this.txtNameCompany.Size = new Size(805, 22);
      this.txtNameCompany.TabIndex = 0;
      this.txtNameCompany.Click += new EventHandler(this.txtNameCompany_Click);
      this.lblCompanyName.AutoSize = true;
      this.lblCompanyName.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.lblCompanyName.Location = new Point(8, 40);
      this.lblCompanyName.Name = "lblCompanyName";
      this.lblCompanyName.Size = new Size(163, 16);
      this.lblCompanyName.TabIndex = 12;
      this.lblCompanyName.Text = "Название организации";
      this.tpService.Controls.Add((Control) this.dgvServAndPhone);
      this.tpService.Location = new Point(4, 25);
      this.tpService.Name = "tpService";
      this.tpService.Padding = new Padding(3);
      this.tpService.Size = new Size(980, 366);
      this.tpService.TabIndex = 2;
      this.tpService.Text = "Службы и телефоны";
      this.tpService.UseVisualStyleBackColor = true;
      this.dgvServAndPhone.BackgroundColor = Color.AliceBlue;
      this.dgvServAndPhone.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvServAndPhone.Columns.AddRange((DataGridViewColumn) this.Dbeg, (DataGridViewColumn) this.Dend, (DataGridViewColumn) this.Service, (DataGridViewColumn) this.Receipt, (DataGridViewColumn) this.Phone, (DataGridViewColumn) this.Note);
      this.dgvServAndPhone.ContextMenuStrip = this.cmenu;
      this.dgvServAndPhone.Dock = DockStyle.Fill;
      this.dgvServAndPhone.Location = new Point(3, 3);
      this.dgvServAndPhone.Name = "dgvServAndPhone";
      this.dgvServAndPhone.Size = new Size(974, 360);
      this.dgvServAndPhone.TabIndex = 1;
      this.dgvServAndPhone.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvServAndPhone_CellBeginEdit);
      this.Dbeg.DataPropertyName = "DBeg";
      gridViewCellStyle1.Format = "dd.MM.yyyy";
      this.Dbeg.DefaultCellStyle = gridViewCellStyle1;
      this.Dbeg.HeaderText = "Дата начала";
      this.Dbeg.Name = "Dbeg";
      this.Dend.DataPropertyName = "DEnd";
      gridViewCellStyle2.Format = "dd.MM.yyyy";
      this.Dend.DefaultCellStyle = gridViewCellStyle2;
      this.Dend.HeaderText = "Дата окончания";
      this.Dend.Name = "Dend";
      this.Service.HeaderText = "Служба";
      this.Service.Name = "Service";
      this.Service.Resizable = DataGridViewTriState.True;
      this.Service.SortMode = DataGridViewColumnSortMode.Automatic;
      this.Receipt.HeaderText = "Квитанция";
      this.Receipt.Name = "Receipt";
      this.Phone.DataPropertyName = "Phone";
      this.Phone.HeaderText = "Телефон";
      this.Phone.Name = "Phone";
      this.Note.DataPropertyName = "Note";
      this.Note.HeaderText = "Примечание";
      this.Note.Name = "Note";
      this.tmr.Interval = 1000;
      this.tmr.Tick += new EventHandler(this.tmr_Tick);
      this.hp.HelpNamespace = "Help.chm";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(988, 490);
      this.Controls.Add((Control) this.tcntrl);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.pnBtn);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.hp.SetHelpKeyword((Control) this, "kv131.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmRecviz";
      this.hp.SetShowHelp((Control) this, true);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Реквизиты домоуправления";
      this.Load += new EventHandler(this.FrmRecviz_Load);
      this.pnBtn.ResumeLayout(false);
      this.cmenu.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.tcntrl.ResumeLayout(false);
      this.tpParam.ResumeLayout(false);
      ((ISupportInitialize) this.dgvCmpParam).EndInit();
      this.tpMain.ResumeLayout(false);
      this.tpMain.PerformLayout();
      this.tpService.ResumeLayout(false);
      ((ISupportInitialize) this.dgvServAndPhone).EndInit();
      this.ResumeLayout(false);
    }
  }
}
