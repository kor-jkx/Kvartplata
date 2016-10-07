// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmReceipt
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
  public class FrmReceipt : Form
  {
    protected GridSettings MySettingsCmpHmReceipt = new GridSettings();
    private IList companyList = (IList) new ArrayList();
    private short company_id = 0;
    private FormStateSaver fss = new FormStateSaver(FrmReceipt.ic);
    private IList<ReceiptParam> rcParams = (IList<ReceiptParam>) new List<ReceiptParam>();
    private BindingSource bs = new BindingSource();
    private bool _readOnlyService = false;
    private bool _readOnly = false;
    private IContainer components = (IContainer) null;
    private static IContainer ic;
    private Complex Complex;
    private ReceiptParam OldReceiptParam;
    private Kvartplata.Classes.Period monthclosed;
    private DateTime nextMonthClosed;
    private IList<CmpHmReceipt> oldListReceipts;
    private CmpHmReceipt oldReceipt;
    private ISession session;
    private bool PastTime;
    private Kvartplata.Classes.Period CurrentPeriod;
    private Kvartplata.Classes.Period MonthClosed;
    private Kvartplata.Classes.Period NextMonthClosed;
    private DateTime LastDayMonthClosed;
    private bool Arhiv;
    private int SelectedIndexHomeParam;
    private Company _company;
    private DataColumn dataColumn2;
    private DataColumn dataColumn1;
    private DataTable dtYesNo;
    private DataSet dsMain;
    private ToolStrip toolStrip1;
    private ToolStripButton tsbReceipt;
    private ToolStripButton tsbCmpReceipt;
    private ToolStripButton tsbExit;
    private Panel pnlReceipt;
    private Panel pnlCmpReceipt;
    private GroupBox groupBox1;
    private ComboBox cbCompany;
    private Label label1;
    private UCReceipt ucReceipt1;
    private UCCmpReceipt ucCmpReceipt1;
    public HelpProvider hp;
    private ToolStripButton tsbHmReceipt;
    private Panel pnlHmReceipt;
    private Label lblSost;
    private Label lblService;
    private Label lblHome;
    private Label lblReceipt;
    private Label lblCompany;
    private Label lblSupplier;
    private ComboBox cmbRecipient;
    private ComboBox cmbSost;
    private ComboBox cmbService;
    private ComboBox cmbHome;
    private ComboBox cmbReceipt;
    private ComboBox cmbCompany;
    private DataGridView dgvHmReceipt;
    private Panel pnUp;
    private ToolStrip toolStrip2;
    private ToolStripButton tsbAdd;
    private ToolStripButton tsbApply;
    private ToolStripButton tsbCancel;
    private ToolStripButton tsbDelete;
    private CheckBox cbArchive;
    private ComboBox cmbComplex;
    private Label lblComplex;
    private ComboBox cmbPerfomer;
    private Label lblPerfomer;
    private ToolStripButton tdbReceiptParam;
    private Panel pnReceiptParam;
    private ToolStrip toolStrip3;
    private ToolStripButton tsbAddReceiptParam;
    private ToolStripButton tsbApplayReceiptParam;
    private ToolStripButton tsbCancelReceiptParam;
    private ToolStripButton tsbDeleteReceiptParam;
    private ToolStripButton toolStripButton4;
    private DataGridView dgvReceiptParam;
    private GroupBox groupBox2;
    private ComboBox cmbCompanyParam;
    private Label label2;
    private MaskDateColumn maskDateColumn1;
    private MaskDateColumn maskDateColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private CheckBox chbArhiv;
    private Button btnPastTime;
    private Label lblPastTime;
    private Timer tmr;
    private DataGridViewComboBoxColumn Receipt;
    private DataGridViewComboBoxColumn Param;
    private DataGridViewTextBoxColumn PValue;
    private MaskDateColumn DBeg;
    private MaskDateColumn Period;
    private MaskDateColumn DEnd;
    private DataGridViewTextBoxColumn UName;
    private DataGridViewTextBoxColumn DEdit;

    public short Company_id
    {
      get
      {
        return this.company_id;
      }
      set
      {
        this.company_id = value;
        Company company = this.session.Get<Company>((object) value);
        this.cbCompany.SelectedItem = (object) company;
        this.cmbCompanyParam.SelectedItem = (object) company;
        this.ucReceipt1.LoadData();
        if (this.cbCompany.SelectedItem == null)
          return;
        this.ucCmpReceipt1.Company_id = this.company_id;
      }
    }

    public bool InsertRecord { get; set; }

    public ReceiptParam newObject { get; set; }

    public ReceiptParam curObject { get; set; }

    public FrmReceipt(Company company)
    {
      this.InitializeComponent();
      this._company = company;
      this.CheckAccess();
      this.fss.ParentForm = (Form) this;
      this.session = Domain.CurrentSession;
      this.ucCmpReceipt1.session = Domain.CurrentSession;
      this.ucReceipt1.session = Domain.CurrentSession;
      this.ucReceipt1.tsbExit.Visible = false;
      this.ucReceipt1.LoadSettings();
      this.Company_id = Options.Company.CompanyId;
      this.ucCmpReceipt1.tsbExit.Visible = false;
      this.ucCmpReceipt1.LoadSettings();
      this.companyList = this.session.CreateQuery("from Company order by CompanyId").List();
      this.cbCompany.DataSource = (object) this.companyList;
      this.cbCompany.DisplayMember = "CompanyName";
      this.cbCompany.ValueMember = "CompanyId";
      this.cmbCompanyParam.DataSource = (object) this.companyList;
      this.cmbCompanyParam.DisplayMember = "CompanyName";
      this.cmbCompanyParam.ValueMember = "CompanyId";
      this.cmbCompanyParam.SelectedValue = (object) this.Company_id;
      this.MySettingsCmpHmReceipt.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
      this.MySettingsCmpHmReceipt.GridName = "CmpHmReceipt";
      this.tsbHmReceipt.Text = "Разделение по \n квитанциям";
      if (Options.Kvartplata && Options.Arenda)
      {
        IList<Complex> complexList = (IList<Complex>) new List<Complex>();
        complexList.Insert(0, new Complex(100, "Квартплата"));
        complexList.Insert(1, new Complex(110, "Аренда"));
        this.cmbComplex.DataSource = (object) complexList;
        this.cmbComplex.DisplayMember = "ComplexName";
        this.cmbComplex.ValueMember = "ComplexId";
        this.Complex = Options.Complex;
      }
      else
      {
        this.lblComplex.Visible = false;
        this.cmbComplex.Visible = false;
        this.Complex = !Options.Kvartplata ? Options.ComplexArenda : Options.Complex;
      }
      this.bs.DataSource = (object) this.rcParams;
      this.dgvReceiptParam.DataSource = (object) this.bs;
    }

    private void CheckAccess()
    {
      this._readOnlyService = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(32, this._company, false));
      this._readOnly = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(33, this._company, false));
      this.ucReceipt1.toolStrip1.Visible = this._readOnlyService;
      this.ucReceipt1.dgvBase.ReadOnly = !this._readOnlyService;
      this.ucCmpReceipt1.toolStrip1.Visible = this._readOnly;
      this.ucCmpReceipt1.dgvBase.ReadOnly = !this._readOnly;
      this.dgvHmReceipt.ReadOnly = !this._readOnly;
      this.dgvReceiptParam.ReadOnly = !this._readOnly;
      this.toolStrip2.Visible = this._readOnly;
      this.toolStrip3.Visible = this._readOnly;
    }

    private void FrmReceipt_Load(object sender, EventArgs e)
    {
      if (this.session != null && this.session.IsOpen)
        this.session.Clear();
      this.session = Domain.CurrentSession;
      IList list1 = this.session.CreateQuery("from Bank order by BankName").List();
      if (list1.Count == 0)
        list1.Insert(0, (object) new Bank());
      ((DataGridViewComboBoxColumn) this.ucCmpReceipt1.dgvBase.Columns["Bank_id"]).DataSource = (object) list1;
      ((DataGridViewComboBoxColumn) this.ucCmpReceipt1.dgvBase.Columns["Supplier_id"]).DataSource = (object) this.session.CreateQuery("from Deliver where IDBASEORG in (SELECT IDBASEORG FROM Postaver) order by NAMEORG").List();
      ((DataGridViewComboBoxColumn) this.ucCmpReceipt1.dgvBase.Columns["Seller"]).DataSource = (object) this.session.CreateQuery("from Deliver where IDBASEORG in (SELECT IDBASEORG FROM Postaver) order by NAMEORG").List();
      ((DataGridViewComboBoxColumn) this.ucCmpReceipt1.dgvBase.Columns["Consignor"]).DataSource = (object) this.session.CreateQuery("from Deliver where IDBASEORG in (SELECT IDBASEORG FROM Postaver) order by NAMEORG").List();
      IList list2 = this.session.CreateQuery("from Receipt order by ReceiptName").List();
      list2.Insert(0, (object) new Kvartplata.Classes.Receipt());
      ((DataGridViewComboBoxColumn) this.ucCmpReceipt1.dgvBase.Columns["Receipt_id"]).DataSource = (object) list2;
      if (KvrplHelper.CheckProxy(32, 1, (Company) null, false))
        return;
      this.tsbCmpReceipt.Checked = true;
      this.tsbCmpReceipt_Click(sender, e);
    }

    private void tsbReceipt_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 1, (Company) null, true))
      {
        this.tsbReceipt.Checked = false;
      }
      else
      {
        if (this.ucCmpReceipt1.tsbApplay.Enabled)
        {
          if (MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
          {
            this.ucCmpReceipt1.SaveData();
            this.tsbReceipt.Checked = false;
            return;
          }
          this.ucCmpReceipt1.LoadData();
          this.ucCmpReceipt1.CancelEnabled();
        }
        else
        {
          this.ucReceipt1.LoadData();
          this.ucCmpReceipt1.LoadData();
        }
        this.tsbCmpReceipt.Checked = false;
        this.tsbHmReceipt.Checked = false;
        this.tdbReceiptParam.Checked = false;
        this.pnlReceipt.BringToFront();
      }
    }

    private void tsbCmpReceipt_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 1, this.session.Get<Company>((object) this.Company_id), true))
      {
        this.tsbCmpReceipt.Checked = false;
      }
      else
      {
        if (this.ucReceipt1.tsbApplay.Enabled)
        {
          if (MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
          {
            this.ucReceipt1.SaveData();
            this.tsbCmpReceipt.Checked = false;
            this.tsbHmReceipt.Checked = false;
            return;
          }
          this.ucReceipt1.LoadData();
          this.ucReceipt1.CancelEnabled();
        }
        else
        {
          this.ucReceipt1.LoadData();
          this.ucCmpReceipt1.LoadData();
        }
        this.tsbReceipt.Checked = false;
        this.tsbHmReceipt.Checked = false;
        this.tdbReceiptParam.Checked = false;
        this.pnlCmpReceipt.BringToFront();
      }
    }

    private void tsbExit_Click(object sender, EventArgs e)
    {
      if ((this.ucReceipt1.tsbApplay.Enabled || this.ucCmpReceipt1.tsbApplay.Enabled) && MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
      {
        this.tsbExit.Checked = false;
        if (this.ucReceipt1.tsbApplay.Enabled)
        {
          this.ucReceipt1.SaveData();
        }
        else
        {
          if (!this.ucCmpReceipt1.tsbApplay.Enabled)
            return;
          this.ucCmpReceipt1.SaveData();
        }
      }
      else
        this.Close();
    }

    private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cbCompany.SelectedItem == null)
        return;
      if (this.tsbCmpReceipt.Checked && !KvrplHelper.CheckProxy(33, 1, (Company) this.cbCompany.SelectedItem, true))
      {
        this.cbCompany.SelectedItem = (object) this.session.Get<Company>((object) this.Company_id);
        this.cbCompany.SelectedValue = (object) this.Company_id;
      }
      else
        this.ucCmpReceipt1.Company_id = ((Company) this.cbCompany.SelectedItem).CompanyId;
      if ((int) this.ucCmpReceipt1.Company_id != (int) ((Company) this.cbCompany.SelectedItem).CompanyId)
        this.cbCompany.SelectedItem = (object) this.session.Get<Company>((object) this.ucCmpReceipt1.Company_id);
    }

    private void ucReceipt1_ObjectsListChanged(object sender, EventArgs e)
    {
      IList list = this.session.CreateQuery("from Receipt order by ReceiptName").List();
      list.Insert(0, (object) new Kvartplata.Classes.Receipt());
      ((DataGridViewComboBoxColumn) this.ucCmpReceipt1.dgvBase.Columns["Receipt_id"]).DataSource = (object) list;
    }

    private void ucReceipt1_CurObjectChanged(object sender, EventArgs e)
    {
    }

    private void toolStripButton1_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 1, this.session.Get<Company>((object) this.Company_id), true))
      {
        this.tsbHmReceipt.Checked = false;
      }
      else
      {
        if (this.ucReceipt1.tsbApplay.Enabled || this.ucCmpReceipt1.tsbApplay.Enabled)
        {
          if (this.ucReceipt1.tsbApplay.Enabled)
          {
            if (MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
              this.ucReceipt1.SaveData();
              this.tsbCmpReceipt.Checked = false;
              this.tsbHmReceipt.Checked = false;
              return;
            }
            this.ucReceipt1.LoadData();
            this.ucReceipt1.CancelEnabled();
          }
          if (this.ucCmpReceipt1.tsbApplay.Enabled)
          {
            if (MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
              this.ucCmpReceipt1.SaveData();
              this.tsbHmReceipt.Checked = false;
              this.tsbReceipt.Checked = false;
              return;
            }
            this.ucCmpReceipt1.LoadData();
            this.ucCmpReceipt1.CancelEnabled();
          }
        }
        else
          this.LoadData();
        this.tsbReceipt.Checked = false;
        this.tsbCmpReceipt.Checked = false;
        this.tdbReceiptParam.Checked = false;
        this.pnlHmReceipt.BringToFront();
      }
    }

    private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
    }

    private void dgvHmReceipt_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, this.dgvHmReceipt.Name, e);
    }

    private void tdbReceiptParam_Click(object sender, EventArgs e)
    {
      this.pnReceiptParam.BringToFront();
      this.rcParams = (IList<ReceiptParam>) new List<ReceiptParam>();
      this.LoadReceiptParam();
      this.tsbReceipt.Checked = false;
      this.tsbHmReceipt.Checked = false;
      this.tsbCmpReceipt.Checked = false;
    }

    private void LoadReceiptParam()
    {
      this.session = Domain.CurrentSession;
      this.CurrentPeriod = Options.Period;
      this.MonthClosed = new Kvartplata.Classes.Period();
      this.MonthClosed = KvrplHelper.GetKvrClose((Company) this.cmbCompanyParam.SelectedItem, Options.ComplexPasp, Options.ComplexPrior);
      this.NextMonthClosed = new Kvartplata.Classes.Period();
      this.NextMonthClosed = KvrplHelper.GetNextPeriod(this.MonthClosed);
      this.LastDayMonthClosed = new DateTime();
      this.LastDayMonthClosed = KvrplHelper.GetLastDayPeriod(this.MonthClosed.PeriodName.Value);
      this.rcParams = (this.PastTime ? (!this.Arhiv ? this.session.CreateQuery(string.Format("select h from ReceiptParam h, Param p where h.Param.ParamId=p.ParamId and   h.Company.CompanyId={0} and h.Period.PeriodId={1} and  p.Param_type=7 order by h.Param.ParamId, h.DBeg ", (object) ((Company) this.cmbCompanyParam.SelectedItem).CompanyId, (object) this.CurrentPeriod.PeriodId)) : this.session.CreateQuery(string.Format("select h from ReceiptParam h, Param p where h.Param.ParamId=p.ParamId and  h.Company.CompanyId={0} and h.Period.PeriodId!=0 and  p.Param_type=7 order by h.Param.ParamId, h.Period.PeriodName ", (object) ((Company) this.cmbCompanyParam.SelectedItem).CompanyId))) : (!this.Arhiv ? this.session.CreateQuery(string.Format("select h from ReceiptParam h, Param p where h.Param.ParamId=p.ParamId and  h.Company.CompanyId={0} and h.Period.PeriodId={1} and p.Param_type=7 and h.DEnd >= '{2}'  order by h.Param.ParamId, h.DBeg ", (object) ((Company) this.cmbCompanyParam.SelectedItem).CompanyId, (object) 0, (object) KvrplHelper.DateToBaseFormat(this.NextMonthClosed.PeriodName.Value))) : this.session.CreateQuery(string.Format("select h from ReceiptParam h, Param p where h.Param.ParamId=p.ParamId and  h.Company.CompanyId={0} and h.Period.PeriodId={1} and p.Param_type=7 order by h.Param.ParamId, h.DBeg ", (object) ((Company) this.cmbCompanyParam.SelectedItem).CompanyId, (object) 0)))).List<ReceiptParam>();
      this.dgvReceiptParam.AutoGenerateColumns = false;
      this.bs.ResetBindings(true);
      this.SelectRow();
      this.SetViewdgvReceiptParam();
    }

    protected void SelectRow()
    {
      if (this.curObject != null)
      {
        int curObject = KvrplHelper.FindCurObject((IList) this.rcParams.ToList<ReceiptParam>(), (object) this.curObject);
        if (curObject < 0)
          return;
        this.dgvReceiptParam.CurrentCell = this.dgvReceiptParam.Rows[curObject].Cells[1];
        this.dgvReceiptParam.Rows[curObject].Selected = true;
      }
      else if (this.dgvReceiptParam.CurrentRow != null && this.dgvReceiptParam.CurrentRow.Index < this.rcParams.Count)
        this.curObject = (ReceiptParam) this.dgvReceiptParam.Rows[this.dgvReceiptParam.CurrentRow.Index].DataBoundItem;
    }

    private void SetViewdgvReceiptParam()
    {
      IList<Kvartplata.Classes.Param> objList = this.session.CreateQuery("from Param where Param_type = 7").List<Kvartplata.Classes.Param>();
      IList<Kvartplata.Classes.Receipt> receiptList = this.session.CreateQuery("from Receipt").List<Kvartplata.Classes.Receipt>();
      for (int index = 0; index < this.dgvReceiptParam.Rows.Count; ++index)
      {
        DataGridViewRow row = this.dgvReceiptParam.Rows[index];
        ((DataGridViewComboBoxCell) this.dgvReceiptParam["Param", row.Index]).DataSource = (object) objList;
        ((DataGridViewComboBoxCell) this.dgvReceiptParam["Param", row.Index]).DisplayMember = "ParamName";
        ((DataGridViewComboBoxCell) this.dgvReceiptParam["Param", row.Index]).ValueMember = "ParamId";
        ((DataGridViewComboBoxCell) this.dgvReceiptParam["Receipt", row.Index]).DataSource = (object) receiptList;
        ((DataGridViewComboBoxCell) this.dgvReceiptParam["Receipt", row.Index]).DisplayMember = "ReceiptName";
        ((DataGridViewComboBoxCell) this.dgvReceiptParam["Receipt", row.Index]).ValueMember = "ReceiptId";
        if (row.DataBoundItem != null)
        {
          Kvartplata.Classes.Param obj = ((ReceiptParam) row.DataBoundItem).Param;
          if (obj != null)
            row.Cells["Param"].Value = (object) obj.ParamId;
          row.Cells["DBeg"].Value = (object) ((ReceiptParam) row.DataBoundItem).DBeg;
          row.Cells["DEnd"].Value = (object) ((ReceiptParam) row.DataBoundItem).DEnd;
          if (this.PastTime)
          {
            this.dgvReceiptParam.Columns["Period"].Visible = true;
            row.Cells["Period"].Value = (object) ((ReceiptParam) row.DataBoundItem).Period.PeriodName.Value.ToShortDateString();
          }
          else
            this.dgvReceiptParam.Columns["Period"].Visible = false;
          Kvartplata.Classes.Receipt receipt = ((ReceiptParam) row.DataBoundItem).Receipt;
          if (receipt != null)
            row.Cells["Receipt"].Value = (object) receipt.ReceiptId;
          if (obj != null)
          {
            IList<AdmTbl> admTblList = this.session.CreateQuery(string.Format("select s from AdmTbl s,ParamRelation r where s.TableId=r.TableId and r.ParamId={0}", (object) obj.ParamId)).List<AdmTbl>();
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
                  if ((int) obj.ParamId == 304)
                    str = " where SchemeType=11";
                  if ((int) obj.ParamId == 827 || (int) obj.ParamId == 828)
                    str = " where SchemeType=18";
                  viewComboBoxCell.DataSource = (object) this.session.CreateQuery(string.Format("from {0}" + str, (object) admTblList[0].ClassName)).List();
                  viewComboBoxCell.ValueType = typeof (short);
                  row.Cells["PValue"] = (DataGridViewCell) viewComboBoxCell;
                }
                catch
                {
                }
              }
              if ((int) obj.ParamId == 305 || (int) obj.ParamId == 309)
              {
                int num = (int) obj.ParamId != 309 ? 0 : (!this.dgvReceiptParam.CurrentCell.IsInEditMode ? 1 : 0);
                row.Cells["PValue"].Value = num == 0 ? (object) (int) ((ReceiptParam) row.DataBoundItem).ParamValue : (object) (int) ((ReceiptParam) row.DataBoundItem).ParamValue;
              }
              else
                row.Cells["PValue"].Value = (object) (short) ((ReceiptParam) row.DataBoundItem).ParamValue;
            }
            else
              row.Cells["PValue"].Value = (object) ((ReceiptParam) row.DataBoundItem).ParamValue;
          }
        }
      }
      if (this.dgvReceiptParam.Rows.Count > 0 && this.SelectedIndexHomeParam == -1)
        this.SelectedIndexHomeParam = 0;
      if (this.SelectedIndexHomeParam == -1 || this.SelectedIndexHomeParam >= this.dgvReceiptParam.Rows.Count)
        return;
      if (!this.InsertRecord)
      {
        this.dgvReceiptParam.Rows[this.SelectedIndexHomeParam].Selected = true;
        this.dgvReceiptParam.CurrentCell = this.dgvReceiptParam.Rows[this.SelectedIndexHomeParam].Cells[0];
      }
      else
      {
        this.dgvReceiptParam.Rows[this.dgvReceiptParam.Rows.Count - 1].Selected = true;
        this.dgvReceiptParam.CurrentCell = this.dgvReceiptParam.Rows[this.dgvReceiptParam.Rows.Count - 1].Cells[0];
        this.SelectedIndexHomeParam = this.dgvReceiptParam.Rows.Count - 1;
      }
    }

    private void cmbCompanyParam_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbCompanyParam.SelectedItem == null)
        return;
      if (this.tdbReceiptParam.Checked && !KvrplHelper.CheckProxy(33, 1, (Company) this.cmbCompanyParam.SelectedItem, true))
      {
        this.cmbCompanyParam.SelectedItem = (object) this.session.Get<Company>((object) this.Company_id);
        this.cmbCompanyParam.SelectedValue = (object) this.Company_id;
      }
      this.LoadReceiptParam();
    }

    private void dgvReceiptParam_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (!this.dgvReceiptParam.IsCurrentCellDirty)
        return;
      this.dgvReceiptParam.CommitEdit(DataGridViewDataErrorContexts.Commit);
      if (this.dgvReceiptParam.CurrentCell.ColumnIndex == this.dgvReceiptParam.Rows[this.dgvReceiptParam.CurrentRow.Index].Cells["Param"].ColumnIndex)
      {
        IList<AdmTbl> admTblList = this.session.CreateQuery(string.Format("select s from AdmTbl s,ParamRelation r where s.TableId=r.TableId and r.ParamId={0}", this.dgvReceiptParam.Rows[this.dgvReceiptParam.CurrentRow.Index].Cells["Param"].Value)).List<AdmTbl>();
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
              if (Convert.ToInt32(this.dgvReceiptParam.Rows[this.dgvReceiptParam.CurrentRow.Index].Cells["Param"].Value) == 304)
                str = " where SchemeType =11";
              if (Convert.ToInt32(this.dgvReceiptParam.Rows[this.dgvReceiptParam.CurrentRow.Index].Cells["Param"].Value) == 827 || Convert.ToInt32(this.dgvReceiptParam.Rows[this.dgvReceiptParam.CurrentRow.Index].Cells["Param"].Value) == 828)
                str = " where SchemeType=18";
              viewComboBoxCell.DataSource = (object) this.session.CreateQuery(string.Format("from {0}" + str, (object) admTblList[0].ClassName)).List();
              viewComboBoxCell.ValueType = typeof (short);
              this.dgvReceiptParam.CurrentRow.Cells["PValue"] = (DataGridViewCell) viewComboBoxCell;
            }
            catch (Exception ex)
            {
              KvrplHelper.WriteLog(ex, (LsClient) null);
            }
          }
        }
        else
          this.dgvReceiptParam.Rows[this.dgvReceiptParam.CurrentRow.Index].Cells["PValue"] = (DataGridViewCell) new DataGridViewTextBoxCell();
      }
    }

    private void dgvReceiptParam_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      if (this.dgvReceiptParam.CurrentRow != null)
      {
        this.OldReceiptParam = new ReceiptParam();
        this.OldReceiptParam.Company = ((ReceiptParam) this.dgvReceiptParam.Rows[this.dgvReceiptParam.CurrentRow.Index].DataBoundItem).Company;
        this.OldReceiptParam.Receipt = ((ReceiptParam) this.dgvReceiptParam.Rows[this.dgvReceiptParam.CurrentRow.Index].DataBoundItem).Receipt;
        this.OldReceiptParam.DBeg = ((ReceiptParam) this.dgvReceiptParam.Rows[this.dgvReceiptParam.CurrentRow.Index].DataBoundItem).DBeg;
        this.OldReceiptParam.DEnd = ((ReceiptParam) this.dgvReceiptParam.Rows[this.dgvReceiptParam.CurrentRow.Index].DataBoundItem).DEnd;
        this.OldReceiptParam.Param = ((ReceiptParam) this.dgvReceiptParam.Rows[this.dgvReceiptParam.CurrentRow.Index].DataBoundItem).Param;
        this.OldReceiptParam.Period = ((ReceiptParam) this.dgvReceiptParam.Rows[this.dgvReceiptParam.CurrentRow.Index].DataBoundItem).Period;
        try
        {
          this.OldReceiptParam.ParamValue = Convert.ToDouble(this.dgvReceiptParam.Rows[this.dgvReceiptParam.CurrentRow.Index].Cells["PValue"].Value);
        }
        catch
        {
          this.OldReceiptParam.ParamValue = 0.0;
          this.dgvReceiptParam.Rows[this.dgvReceiptParam.CurrentRow.Index].Cells["PValue"].Value = (object) 0;
        }
      }
      this.tsbAddReceiptParam.Enabled = false;
      this.tsbApplayReceiptParam.Enabled = true;
      this.tsbCancelReceiptParam.Enabled = true;
      this.tsbDeleteReceiptParam.Enabled = false;
    }

    private void tsbApplayReceiptParam_Click(object sender, EventArgs e)
    {
      this.session.Clear();
      DataGridViewRow dataGridViewRow = new DataGridViewRow();
      DataGridViewRow row = this.dgvReceiptParam.Rows[this.dgvReceiptParam.CurrentRow.Index];
      ReceiptParam receiptParam = new ReceiptParam();
      ReceiptParam dataBoundItem = (ReceiptParam) row.DataBoundItem;
      if (row.Cells["Param"].Value == null)
      {
        int num1 = (int) MessageBox.Show("Не выбран параметр!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        dataBoundItem.Company = (Company) this.cmbCompanyParam.SelectedItem;
        dataBoundItem.Receipt = this.session.Get<Kvartplata.Classes.Receipt>((object) Convert.ToInt16(row.Cells["Receipt"].Value));
        dataBoundItem.Period = !this.PastTime ? this.session.Get<Kvartplata.Classes.Period>((object) 0) : this.CurrentPeriod;
        dataBoundItem.Uname = Options.Login;
        dataBoundItem.Dedit = DateTime.Now.Date;
        int num2 = 0;
        if (dataBoundItem.Param != null)
          num2 = (int) dataBoundItem.Param.ParamId;
        dataBoundItem.Param = this.session.Get<Kvartplata.Classes.Param>((object) Convert.ToInt16(row.Cells["Param"].Value));
        try
        {
          dataBoundItem.ParamValue = Convert.ToDouble(KvrplHelper.ChangeSeparator(row.Cells["PValue"].Value.ToString()));
        }
        catch
        {
          int num3 = (int) MessageBox.Show("Некорректный формат значения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        try
        {
          dataBoundItem.DBeg = Convert.ToDateTime(row.Cells["DBeg"].Value);
        }
        catch
        {
          int num3 = (int) MessageBox.Show("Некорректный формат даты!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        DateTime dateTime1 = dataBoundItem.DBeg;
        if (dateTime1.Day != 1)
        {
          int num4 = (int) MessageBox.Show("Некорректный формат даты! Должно быть первое число месяца.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          try
          {
            dataBoundItem.DEnd = Convert.ToDateTime(row.Cells["DEnd"].Value);
          }
          catch
          {
            int num3 = (int) MessageBox.Show("Некорректный формат даты!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
          if (dataBoundItem.DBeg > dataBoundItem.DEnd)
          {
            int num3 = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. дата начала больше даты окончания", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.session.Clear();
            this.session = Domain.CurrentSession;
          }
          else if (((int) dataBoundItem.Param.ParamId == 827 || (int) dataBoundItem.Param.ParamId == 828) && (dataBoundItem.DBeg != KvrplHelper.FirstDay(dataBoundItem.DBeg) || dataBoundItem.DEnd != KvrplHelper.LastDay(dataBoundItem.DEnd)))
          {
            int num3 = (int) MessageBox.Show("Дата начала должна быть 1-е число месяца, дата оканчания последние число месяца", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            this.session.Clear();
            this.session = Domain.CurrentSession;
          }
          else
          {
            if (!this.PastTime)
            {
              if (!this.InsertRecord && dataBoundItem.DBeg <= this.LastDayMonthClosed && dataBoundItem.DBeg != this.OldReceiptParam.DBeg)
              {
                int num3 = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.session.Clear();
                this.session = Domain.CurrentSession;
                return;
              }
              if (!this.InsertRecord && (this.OldReceiptParam.DBeg < this.LastDayMonthClosed && this.OldReceiptParam.DEnd < this.LastDayMonthClosed))
              {
                int num3 = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.session.Clear();
                this.session = Domain.CurrentSession;
                return;
              }
              if (!this.InsertRecord && (this.OldReceiptParam.DBeg < this.LastDayMonthClosed && (this.OldReceiptParam.DBeg != dataBoundItem.DBeg || (int) this.OldReceiptParam.Param.ParamId != (int) dataBoundItem.Param.ParamId || this.OldReceiptParam.ParamValue != dataBoundItem.ParamValue) || this.OldReceiptParam.DEnd < this.LastDayMonthClosed && (this.OldReceiptParam.DEnd != dataBoundItem.DEnd || (int) this.OldReceiptParam.Param.ParamId != (int) dataBoundItem.Param.ParamId || this.OldReceiptParam.ParamValue != dataBoundItem.ParamValue)))
              {
                int num3 = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.session.Clear();
                this.session = Domain.CurrentSession;
                return;
              }
              if (dataBoundItem.DEnd < this.LastDayMonthClosed)
              {
                int num3 = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. дата окончания принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.session.Clear();
                this.session = Domain.CurrentSession;
                if (this.InsertRecord);
                return;
              }
              int num5;
              if (this.InsertRecord)
              {
                dateTime1 = dataBoundItem.DBeg;
                DateTime? periodName = this.NextMonthClosed.PeriodName;
                num5 = periodName.HasValue ? (dateTime1 < periodName.GetValueOrDefault() ? 1 : 0) : 0;
              }
              else
                num5 = 0;
              if (num5 != 0)
              {
                int num3 = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.session.Clear();
                this.session = Domain.CurrentSession;
                return;
              }
            }
            else
            {
              if ((int) dataBoundItem.Param.ParamId == 827 || (int) dataBoundItem.Param.ParamId == 828)
              {
                int num3 = (int) MessageBox.Show("Этот параметр нельзя менять в прошлом времени!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.session.Clear();
                this.session = Domain.CurrentSession;
                return;
              }
              DateTime dbeg = dataBoundItem.DBeg;
              DateTime? periodName = this.MonthClosed.PeriodName;
              DateTime lastDayPeriod1 = KvrplHelper.GetLastDayPeriod(periodName.Value);
              int num5;
              if (!(dbeg > lastDayPeriod1))
              {
                DateTime dend = dataBoundItem.DEnd;
                periodName = this.MonthClosed.PeriodName;
                DateTime lastDayPeriod2 = KvrplHelper.GetLastDayPeriod(periodName.Value);
                num5 = dend > lastDayPeriod2 ? 1 : 0;
              }
              else
                num5 = 1;
              if (num5 != 0)
              {
                int num3 = (int) MessageBox.Show("Не могу сохранить текущую запись! Проверьте корректность введённых дат.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.session.Clear();
                this.session = Domain.CurrentSession;
                return;
              }
              periodName = this.CurrentPeriod.PeriodName;
              DateTime dateTime2 = periodName.Value;
              periodName = this.MonthClosed.PeriodName;
              DateTime lastDayPeriod3 = KvrplHelper.GetLastDayPeriod(periodName.Value);
              if (dateTime2 < lastDayPeriod3)
              {
                int num3 = (int) MessageBox.Show("Не могу сохранить текущую запись! Проверьте корректность введённых дат.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.session.Clear();
                this.session = Domain.CurrentSession;
                return;
              }
              periodName = this.MonthClosed.PeriodName;
              if (KvrplHelper.GetLastDayPeriod(periodName.Value) - dataBoundItem.DBeg > new TimeSpan(730, 0, 0, 0) && MessageBox.Show("Дата начала отличается от даты закрытого периода более, чем на 2 года. Продолжить сохранение? ", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
              {
                this.session.Clear();
                this.session = Domain.CurrentSession;
                return;
              }
            }
            if (this.newObject != null)
            {
              using (ITransaction transaction = this.session.BeginTransaction())
              {
                try
                {
                  this.session.Save((object) dataBoundItem);
                  this.session.Flush();
                  transaction.Commit();
                }
                catch (Exception ex)
                {
                  this.session.Clear();
                  this.session = Domain.CurrentSession;
                  int num3 = (int) MessageBox.Show("Не могу сохранить текущую запись", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  KvrplHelper.WriteLog(ex, (LsClient) null);
                  transaction.Rollback();
                }
              }
            }
            if (this.newObject == null)
            {
              using (ITransaction transaction = this.session.BeginTransaction())
              {
                try
                {
                  ISQLQuery sqlQuery = this.session.CreateSQLQuery("UPDATE DBA.rcptParam cmp set DBeg= :dbeg, Param_Id= :paramid, DEnd= :dend, Receipt_id= :Receipt_idNew, Param_Value= :paramvalue  where cmp.Company_Id= :companyid and cmp.Period_Id= :periodid  and cmp.Dbeg = :olddbeg and cmp.Param_Id = :oldparamid and cmp.Receipt_id= :Receipt_id");
                  string name1 = "dbeg";
                  dateTime1 = dataBoundItem.DBeg;
                  DateTime date1 = dateTime1.Date;
                  IQuery query1 = sqlQuery.SetParameter<DateTime>(name1, date1).SetParameter<short>("paramid", dataBoundItem.Param.ParamId);
                  string name2 = "dend";
                  dateTime1 = dataBoundItem.DEnd;
                  DateTime date2 = dateTime1.Date;
                  IQuery query2 = query1.SetParameter<DateTime>(name2, date2).SetParameter<double>("paramvalue", dataBoundItem.ParamValue).SetParameter<short>("companyid", dataBoundItem.Company.CompanyId).SetParameter<int>("periodid", dataBoundItem.Period.PeriodId);
                  string name3 = "olddbeg";
                  dateTime1 = this.OldReceiptParam.DBeg;
                  DateTime date3 = dateTime1.Date;
                  query2.SetParameter<DateTime>(name3, date3).SetParameter<short>("oldparamid", this.OldReceiptParam.Param.ParamId).SetParameter<short>("Receipt_idNew", dataBoundItem.Receipt.ReceiptId).SetParameter<short>("Receipt_id", this.OldReceiptParam.Receipt.ReceiptId).ExecuteUpdate();
                  transaction.Commit();
                }
                catch
                {
                  this.session.Clear();
                  this.session = Domain.CurrentSession;
                  int num3 = (int) MessageBox.Show("Не могу сохранить текущую запись", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  transaction.Rollback();
                  return;
                }
              }
            }
            this.InsertRecord = false;
            this.tsbAddReceiptParam.Enabled = true;
            this.tsbApplayReceiptParam.Enabled = false;
            this.tsbCancelReceiptParam.Enabled = false;
            this.tsbDeleteReceiptParam.Enabled = true;
          }
        }
      }
    }

    private void tsbAddReceiptParam_Click(object sender, EventArgs e)
    {
      this.InsertRecord = true;
      this.newObject = new ReceiptParam();
      this.newObject.Period = this.session.Get<Kvartplata.Classes.Period>((object) 0);
      this.newObject.Company = (Company) this.cmbCompanyParam.SelectedItem;
      this.newObject.DEnd = new DateTime(2999, 12, 31);
      DateTime? periodName = this.GetOpenPeriod().PeriodName;
      if (periodName.HasValue)
        this.newObject.DBeg = periodName.Value;
      if (this.rcParams == null)
        ;
      this.rcParams.Add(this.newObject);
      this.bs.Add((object) this.newObject);
      this.bs.ResetBindings(true);
      this.dgvReceiptParam.ReadOnly = false;
      this.SetViewdgvReceiptParam();
      this.tsbAddReceiptParam.Enabled = false;
      this.tsbApplayReceiptParam.Enabled = true;
      this.tsbCancelReceiptParam.Enabled = true;
      this.tsbDeleteReceiptParam.Enabled = false;
    }

    private void tsbCancelReceiptParam_Click(object sender, EventArgs e)
    {
      this.LoadReceiptParam();
      this.tsbAddReceiptParam.Enabled = true;
      this.tsbApplayReceiptParam.Enabled = false;
      this.tsbCancelReceiptParam.Enabled = false;
      this.tsbDeleteReceiptParam.Enabled = true;
      this.dgvReceiptParam.EndEdit();
      if (this.newObject != null)
      {
        if (this.newObject.Param == null)
          this.newObject.Param = new Kvartplata.Classes.Param();
        if (this.newObject.Receipt == null)
          this.newObject.Receipt = new Kvartplata.Classes.Receipt();
        this.rcParams.Remove(this.newObject);
        this.bs.Remove((object) this.newObject);
        this.newObject = (ReceiptParam) null;
      }
      else
      {
        foreach (object rcParam in (IEnumerable<ReceiptParam>) this.rcParams)
          ;
        this.dgvReceiptParam.Refresh();
      }
    }

    private void LoadData()
    {
      this.companyList = this.session.CreateQuery("from Company order by CompanyId").List();
      this.cmbCompany.DataSource = (object) this.companyList;
      this.cmbCompany.SelectedValue = (object) this.Company_id;
      this.cmbCompany_SelectionChangeCommitted((object) null, (EventArgs) null);
    }

    private void LoadReceipts()
    {
      this.cmbReceipt.DataSource = (object) this.session.CreateQuery(string.Format("Select r from Receipt r,CmpReceipt cr where r.ReceiptId=cr.ReceiptId and cr.CompanyId={0}", (object) ((Company) this.cmbCompany.SelectedItem).CompanyId)).List<Kvartplata.Classes.Receipt>();
    }

    private void cmbCompany_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.monthclosed = KvrplHelper.GetCmpKvrClose((Company) this.cmbCompany.SelectedItem, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk);
      this.nextMonthClosed = this.monthclosed.PeriodName.Value.AddMonths(1);
      if (this.cmbCompany.SelectedItem != null)
      {
        if (!KvrplHelper.CheckProxy(33, 1, (Company) this.cmbCompany.SelectedItem, true))
        {
          this.cmbCompany.SelectedItem = (object) this.session.Get<Company>((object) this.Company_id);
          if ((int) this.Company_id != (int) ((Company) this.cmbCompany.SelectedItem).CompanyId)
            this.cmbCompany.SelectedItem = (object) this.session.Get<Company>((object) this.Company_id);
          this.cmbCompany.SelectedValue = (object) this.Company_id;
          return;
        }
        this.Company_id = ((Company) this.cmbCompany.SelectedItem).CompanyId;
      }
      this.LoadReceipts();
      this.LoadHomes();
      this.LoadService();
      this.LoadSupplier();
      this.LoadPerfomer();
      this.LoadList();
    }

    private void cmbService_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.LoadSost();
      this.LoadSupplier();
      this.LoadList();
    }

    private void cmbReceipt_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.LoadList();
    }

    private void LoadHomes()
    {
      IList<Home> homeList = this.session.CreateQuery(string.Format(" select distinct new Home(h.IdHome,h.Str,h.NHome,h.HomeKorp,h.Division,h.YearBuild,h.Company.CompanyId) from Home h,HomeLink hl where hl.Home=h and hl.Company.CompanyId={0} " + Options.HomeType + " order by h.Str.NameStr,dba.lengthhome(h.NHome),dba.lengthhome(h.HomeKorp)", (object) ((Company) this.cmbCompany.SelectedItem).CompanyId)).List<Home>();
      homeList.Insert(0, new Home(0, "", ""));
      this.cmbHome.DataSource = (object) homeList;
    }

    private void LoadService()
    {
      IList<Service> serviceList = this.session.CreateQuery(string.Format("select s from Service s,ServiceParam sp where s.ServiceId=sp.Service_id and sp.Company_id={0} and sp.Complex.IdFk={1} and s.Root=0 and s.ServiceId<>0 order by " + Options.SortService, (object) ((Company) this.cmbCompany.SelectedItem).CompanyId, (object) Options.Complex.IdFk)).List<Service>();
      serviceList.Insert(0, new Service((short) 0, ""));
      this.cmbService.DataSource = (object) serviceList;
    }

    private void LoadSost()
    {
      IList<Service> serviceList = this.session.CreateQuery(string.Format("from Service s where s.Root={0} and s.Root<>0 order by ServiceId", (object) ((Service) this.cmbService.SelectedItem).ServiceId)).List<Service>();
      serviceList.Insert(0, new Service((short) 0, ""));
      this.cmbSost.DataSource = (object) serviceList;
    }

    private void LoadSupplier()
    {
      string str = "";
      if ((Service) this.cmbService.SelectedItem != null && (uint) ((Service) this.cmbService.SelectedItem).ServiceId > 0U)
        str = string.Format(" and s.Service.ServiceId={0}", (object) ((Service) this.cmbService.SelectedItem).ServiceId);
      IList<BaseOrg> baseOrgList = this.session.CreateQuery(string.Format("Select distinct d.Recipient from CmpSupplier s,Supplier d where s.SupplierOrg.SupplierId=d.SupplierId and d.Recipient.BaseOrgId<>0 and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={0} and Period.PeriodId=0 and DBeg<='{1}' and DEnd>='{1}' and Param.ParamId=211) " + str + " order by d.Recipient.NameOrgMin", (object) ((Company) this.cmbCompany.SelectedItem).CompanyId, (object) KvrplHelper.DateToBaseFormat(this.nextMonthClosed))).List<BaseOrg>();
      baseOrgList.Insert(0, new BaseOrg(0, ""));
      this.cmbRecipient.DataSource = (object) baseOrgList;
    }

    private void LoadPerfomer()
    {
      string str = "";
      if ((Service) this.cmbService.SelectedItem != null && (uint) ((Service) this.cmbService.SelectedItem).ServiceId > 0U)
        str = string.Format(" and s.Service.ServiceId={0}", (object) ((Service) this.cmbService.SelectedItem).ServiceId);
      IList<BaseOrg> baseOrgList = this.session.CreateQuery(string.Format("Select distinct d.Perfomer from CmpSupplier s,Supplier d where s.SupplierOrg.SupplierId=d.SupplierId and d.Perfomer.BaseOrgId<>0  and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={0} and Period.PeriodId=0 and DBeg<='{1}' and DEnd>='{1}' and Param.ParamId=211) " + str + " order by d.Perfomer.NameOrgMin", (object) ((Company) this.cmbCompany.SelectedItem).CompanyId, (object) KvrplHelper.DateToBaseFormat(this.nextMonthClosed), (object) Convert.ToInt32(this.cmbPerfomer.SelectedValue))).List<BaseOrg>();
      baseOrgList.Insert(0, new BaseOrg(0, ""));
      this.cmbPerfomer.DataSource = (object) baseOrgList;
    }

    private void LoadList()
    {
      this.Cursor = Cursors.WaitCursor;
      this.tsbAdd.Enabled = true;
      this.tsbApply.Enabled = false;
      this.tsbCancel.Enabled = false;
      this.tsbDelete.Enabled = true;
      this.dgvHmReceipt.DataSource = (object) null;
      this.dgvHmReceipt.Columns.Clear();
      IList<CmpHmReceipt> first = (IList<CmpHmReceipt>) new List<CmpHmReceipt>();
      IList<CmpHmReceipt> cmpHmReceiptList1 = (IList<CmpHmReceipt>) new List<CmpHmReceipt>();
      IList<CmpHmReceipt> cmpHmReceiptList2 = (IList<CmpHmReceipt>) new List<CmpHmReceipt>();
      string str1 = string.Format("select chr from CmpHmReceipt chr where chr.HomeId=0 and chr.Company.CompanyId={0} and chr.Receipt.ReceiptId={1} and chr.Complex.IdFk={2} ", (object) ((Company) this.cmbCompany.SelectedItem).CompanyId, (object) ((Kvartplata.Classes.Receipt) this.cmbReceipt.SelectedItem).ReceiptId, (object) this.Complex.IdFk);
      string str2 = string.Format("select chr from CmpHmReceipt chr,Home h where chr.HomeId=h.IdHome and chr.Company.CompanyId={0} and chr.Receipt.ReceiptId={1} and chr.Complex.IdFk={2} ", (object) ((Company) this.cmbCompany.SelectedItem).CompanyId, (object) ((Kvartplata.Classes.Receipt) this.cmbReceipt.SelectedItem).ReceiptId, (object) this.Complex.IdFk);
      if ((Home) this.cmbHome.SelectedItem != null && (uint) ((Home) this.cmbHome.SelectedItem).IdHome > 0U)
        str2 += string.Format(" and chr.HomeId={0}", (object) ((Home) this.cmbHome.SelectedItem).IdHome);
      if ((Service) this.cmbService.SelectedItem != null && (uint) ((Service) this.cmbService.SelectedItem).ServiceId > 0U)
      {
        if ((Service) this.cmbSost.SelectedItem != null && (uint) ((Service) this.cmbSost.SelectedItem).ServiceId > 0U)
        {
          str1 += string.Format(" and chr.Service.ServiceId={0}", (object) ((Service) this.cmbSost.SelectedItem).ServiceId);
          str2 += string.Format(" and chr.Service.ServiceId={0}", (object) ((Service) this.cmbSost.SelectedItem).ServiceId);
        }
        else
        {
          str1 += string.Format(" and (chr.Service.ServiceId={0} or chr.Service.ServiceId in (Select ServiceId from Service where Root={0}))", (object) ((Service) this.cmbService.SelectedItem).ServiceId);
          str2 += string.Format(" and (chr.Service.ServiceId={0} or chr.Service.ServiceId in (Select ServiceId from Service where Root={0}))", (object) ((Service) this.cmbService.SelectedItem).ServiceId);
        }
      }
      if ((BaseOrg) this.cmbRecipient.SelectedItem != null && (uint) ((BaseOrg) this.cmbRecipient.SelectedItem).BaseOrgId > 0U)
      {
        str1 += string.Format(" and chr.Supplier.Recipient.BaseOrgId={0}", (object) ((BaseOrg) this.cmbRecipient.SelectedItem).BaseOrgId);
        str2 += string.Format(" and chr.Supplier.Recipient.BaseOrgId={0}", (object) ((BaseOrg) this.cmbRecipient.SelectedItem).BaseOrgId);
      }
      if ((BaseOrg) this.cmbPerfomer.SelectedItem != null && (uint) ((BaseOrg) this.cmbPerfomer.SelectedItem).BaseOrgId > 0U)
      {
        str1 += string.Format(" and chr.Supplier.Perfomer.BaseOrgId={0}", (object) ((BaseOrg) this.cmbPerfomer.SelectedItem).BaseOrgId);
        str2 += string.Format(" and chr.Supplier.Perfomer.BaseOrgId={0}", (object) ((BaseOrg) this.cmbPerfomer.SelectedItem).BaseOrgId);
      }
      string str3 = str2 + " and h.IdHome<>0 ";
      if (!this.cbArchive.Checked)
      {
        string str4 = str1;
        string format1 = " and chr.DEnd>='{0}' ";
        DateTime? periodName = KvrplHelper.GetCmpKvrClose((Company) this.cmbCompany.SelectedItem, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk).PeriodName;
        string baseFormat1 = KvrplHelper.DateToBaseFormat(periodName.Value.AddMonths(1));
        string str5 = string.Format(format1, (object) baseFormat1);
        str1 = str4 + str5;
        string str6 = str3;
        string format2 = " and chr.DEnd>='{0}' ";
        periodName = KvrplHelper.GetCmpKvrClose((Company) this.cmbCompany.SelectedItem, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk).PeriodName;
        string baseFormat2 = KvrplHelper.DateToBaseFormat(periodName.Value.AddMonths(1));
        string str7 = string.Format(format2, (object) baseFormat2);
        str3 = str6 + str7;
      }
      string queryString1 = str1 + " order by chr.Service.Root,chr.Service.ServiceId,chr.Supplier.SupplierId";
      string queryString2 = str3 + " order by h.Str.NameStr,dba.lengthhome(h.NHome),dba.lengthhome(h.HomeKorp),chr.Service.Root,chr.Service.ServiceId,chr.Supplier.SupplierId";
      if ((Home) this.cmbHome.SelectedItem == null || ((Home) this.cmbHome.SelectedItem).IdHome == 0)
        first = this.session.CreateQuery(queryString1).List<CmpHmReceipt>();
      IList<CmpHmReceipt> cmpHmReceiptList3 = this.session.CreateQuery(queryString2).List<CmpHmReceipt>();
      this.dgvHmReceipt.DataSource = (object) first.Concat<CmpHmReceipt>((IEnumerable<CmpHmReceipt>) cmpHmReceiptList3).ToList<CmpHmReceipt>();
      this.SetViewReceiptList();
      this.session.Clear();
      this.oldListReceipts = (IList<CmpHmReceipt>) new List<CmpHmReceipt>();
      if ((Home) this.cmbHome.SelectedItem == null || ((Home) this.cmbHome.SelectedItem).IdHome == 0)
        first = this.session.CreateQuery(queryString1).List<CmpHmReceipt>();
      IList<CmpHmReceipt> cmpHmReceiptList4 = this.session.CreateQuery(queryString2).List<CmpHmReceipt>();
      this.oldListReceipts = (IList<CmpHmReceipt>) first.Concat<CmpHmReceipt>((IEnumerable<CmpHmReceipt>) cmpHmReceiptList4).ToList<CmpHmReceipt>();
      int index = 0;
      foreach (CmpHmReceipt cmpHmReceipt in (List<CmpHmReceipt>) this.dgvHmReceipt.DataSource)
      {
        cmpHmReceipt.IsEdit = false;
        cmpHmReceipt.OldHashCode = cmpHmReceipt.GetHashCode();
        this.oldListReceipts[index].IsEdit = false;
        this.oldListReceipts[index].OldHashCode = cmpHmReceipt.OldHashCode;
        ++index;
      }
      this.Cursor = Cursors.Default;
    }

    private void SetViewReceiptList()
    {
      IList<Service> serviceList1 = (IList<Service>) new List<Service>();
      IList<BaseOrg> baseOrgList1 = (IList<BaseOrg>) new List<BaseOrg>();
      if ((BaseOrg) this.cmbPerfomer.SelectedItem == null || ((BaseOrg) this.cmbPerfomer.SelectedItem).BaseOrgId == 0)
        KvrplHelper.AddComboBoxColumn(this.dgvHmReceipt, 0, (IList) null, "BaseOrgId", "NameOrgMin", "Исполнитель", "Perfomer", 160, 120);
      if ((BaseOrg) this.cmbRecipient.SelectedItem == null || ((BaseOrg) this.cmbRecipient.SelectedItem).BaseOrgId == 0)
        KvrplHelper.AddComboBoxColumn(this.dgvHmReceipt, 0, (IList) null, "BaseOrgId", "NameOrgMin", "Получатель", "Recipient", 160, 120);
      if ((Service) this.cmbService.SelectedItem == null || (int) ((Service) this.cmbService.SelectedItem).ServiceId == 0 || (Service) this.cmbSost.SelectedItem == null || (int) ((Service) this.cmbSost.SelectedItem).ServiceId == 0)
        KvrplHelper.AddComboBoxColumn(this.dgvHmReceipt, 0, (IList) null, (string) null, (string) null, "Составляющая", "Sost", 160, 120);
      if ((Service) this.cmbService.SelectedItem == null || (int) ((Service) this.cmbService.SelectedItem).ServiceId == 0)
      {
        serviceList1 = this.session.CreateQuery(string.Format("select s from Service s,ServiceParam sp where s.ServiceId=sp.Service_id and sp.Company_id={0} and sp.Complex.IdFk={1} and s.Root=0 and s.ServiceId<>0 order by " + Options.SortService, (object) ((Company) this.cmbCompany.SelectedItem).CompanyId, (object) Options.Complex.IdFk)).List<Service>();
        serviceList1.Insert(0, new Service((short) 0, ""));
        KvrplHelper.AddComboBoxColumn(this.dgvHmReceipt, 0, (IList) serviceList1, "ServiceId", "ServiceName", "Услуга", "Service", 160, 120);
      }
      if ((Home) this.cmbHome.SelectedItem == null || ((Home) this.cmbHome.SelectedItem).IdHome == 0)
      {
        IList<Home> homeList = this.session.CreateQuery(string.Format(" select distinct new Home(h.IdHome,h.Str,h.NHome,h.HomeKorp,0,0,h.Company.CompanyId) from Home h left outer join h.Str,HomeLink hl where hl.Home=h and hl.Company.CompanyId={0} " + Options.HomeType + " order by h.Str.NameStr,dba.lengthhome(h.NHome),dba.lengthhome(h.HomeKorp)", (object) ((Company) this.cmbCompany.SelectedItem).CompanyId)).List<Home>();
        homeList.Insert(0, new Home(0, "", ""));
        KvrplHelper.AddComboBoxColumn(this.dgvHmReceipt, 0, (IList) homeList, "IdHome", "Address", "Дом", "Home", 160, 120);
      }
      KvrplHelper.AddMaskDateColumn(this.dgvHmReceipt, 0, "Дата начала", "DBeg");
      KvrplHelper.AddMaskDateColumn(this.dgvHmReceipt, 1, "Дата окончания", "DEnd");
      KvrplHelper.ViewEdit(this.dgvHmReceipt);
      foreach (DataGridViewRow row in (IEnumerable) this.dgvHmReceipt.Rows)
      {
        row.Cells["DBeg"].Value = (object) ((CmpHmReceipt) row.DataBoundItem).DBeg;
        row.Cells["DEnd"].Value = (object) ((CmpHmReceipt) row.DataBoundItem).DEnd;
        if (this.dgvHmReceipt.Columns["Home"] != null)
          row.Cells["Home"].Value = (object) ((CmpHmReceipt) row.DataBoundItem).HomeId;
        if (((CmpHmReceipt) row.DataBoundItem).Service != null && (uint) ((CmpHmReceipt) row.DataBoundItem).Service.ServiceId > 0U)
        {
          Service service = this.session.Get<Service>((object) ((CmpHmReceipt) row.DataBoundItem).Service.ServiceId);
          if (this.dgvHmReceipt.Columns["Sost"] != null)
          {
            IList<Service> serviceList2 = (IList<Service>) new List<Service>();
            short serviceId = service.ServiceId;
            short? root = service.Root;
            int? nullable = root.HasValue ? new int?((int) root.GetValueOrDefault()) : new int?();
            int num = 0;
            if (nullable.GetValueOrDefault() != num || !nullable.HasValue)
            {
              root = service.Root;
              serviceId = root.Value;
            }
            if (serviceList1.IndexOf(service) != -1)
            {
              row.Cells["Service"].Value = (object) ((CmpHmReceipt) row.DataBoundItem).Service.ServiceId;
              IList<Service> serviceList3 = this.session.CreateQuery(string.Format("select s from Service s where s.Root={0} order by " + Options.SortService, (object) serviceId)).List<Service>();
              row.Cells["Sost"] = (DataGridViewCell) new DataGridViewComboBoxCell()
              {
                DisplayStyleForCurrentCellOnly = true,
                ValueMember = "ServiceId",
                DisplayMember = "ServiceName",
                DataSource = (object) serviceList3
              };
            }
            else
            {
              IList<Service> serviceList3 = this.session.CreateQuery(string.Format("select s from Service s where s.Root={0} order by " + Options.SortService, (object) serviceId)).List<Service>();
              row.Cells["Sost"] = (DataGridViewCell) new DataGridViewComboBoxCell()
              {
                DisplayStyleForCurrentCellOnly = true,
                ValueMember = "ServiceId",
                DisplayMember = "ServiceName",
                DataSource = (object) serviceList3
              };
              row.Cells["Sost"].Value = (object) ((CmpHmReceipt) row.DataBoundItem).Service.ServiceId;
              if (this.dgvHmReceipt.Columns["Service"] != null)
              {
                DataGridViewCell cell = row.Cells["Service"];
                root = service.Root;
                // ISSUE: variable of a boxed type
                short local = root.Value;
                cell.Value = (object) local;
              }
            }
          }
        }
        DateTime? periodName;
        DateTime dateTime;
        if (this.dgvHmReceipt.Columns["Recipient"] != null)
        {
          string str = "";
          if (this.dgvHmReceipt.Columns["Service"] != null && (uint) Convert.ToInt16(row.Cells["Service"].Value) > 0U)
            str = string.Format(" and (s.Service.ServiceId={0} or s.Service.ServiceId=(select Root from Service where ServiceId={0}))", (object) Convert.ToInt16(row.Cells["Service"].Value));
          ISession session = this.session;
          string format = "Select distinct d.Recipient from CmpSupplier s,Supplier d where s.SupplierOrg.SupplierId=d.SupplierId and d.Recipient.BaseOrgId<>0 and s.Company.CompanyId=isnull((select ParamValue from CompanyParam where Period.PeriodId=0 and Company.CompanyId={0} and DBeg<='{1}' and DEnd>='{1}' and Param.ParamId=211),0) " + str + "order by d.Recipient.NameOrgMin";
          // ISSUE: variable of a boxed type
          short companyId = ((Company) this.cmbCompany.SelectedItem).CompanyId;
          periodName = KvrplHelper.GetCmpKvrClose((Company) this.cmbCompany.SelectedItem, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk).PeriodName;
          dateTime = periodName.Value;
          string baseFormat = KvrplHelper.DateToBaseFormat(dateTime.AddMonths(1));
          string queryString = string.Format(format, (object) companyId, (object) baseFormat);
          IList<BaseOrg> baseOrgList2 = session.CreateQuery(queryString).List<BaseOrg>();
          baseOrgList2.Insert(0, new BaseOrg(0, ""));
          row.Cells["Recipient"] = (DataGridViewCell) new DataGridViewComboBoxCell()
          {
            DisplayStyleForCurrentCellOnly = true,
            ValueMember = "BaseOrgId",
            DisplayMember = "NameOrgMin",
            DataSource = (object) baseOrgList2
          };
          if (((CmpHmReceipt) row.DataBoundItem).Supplier != null)
            row.Cells["Recipient"].Value = (object) ((CmpHmReceipt) row.DataBoundItem).Supplier.Recipient.BaseOrgId;
        }
        if (this.dgvHmReceipt.Columns["Perfomer"] != null)
        {
          string str1 = "";
          string str2 = "";
          if (this.dgvHmReceipt.Columns["Service"] != null && (uint) Convert.ToInt16(row.Cells["Service"].Value) > 0U)
            str1 = string.Format(" and (s.Service.ServiceId={0} or s.Service.ServiceId=(select Root from Service where ServiceId={0}))", (object) Convert.ToInt16(row.Cells["Service"].Value));
          int num = this.dgvHmReceipt.Columns["Recipient"] == null ? ((BaseOrg) this.cmbRecipient.SelectedItem).BaseOrgId : Convert.ToInt32(row.Cells["Recipient"].Value);
          if ((uint) num > 0U)
            str2 = "and d.Recipient.BaseOrgId={2}";
          string format = "Select distinct d.Perfomer from CmpSupplier s,Supplier d where s.SupplierOrg.SupplierId=d.SupplierId " + str2 + " and d.Perfomer.BaseOrgId<>0 and s.Company.CompanyId=isnull((select ParamValue from CompanyParam where Period.PeriodId=0 and Company.CompanyId={0} and DBeg<='{1}' and DEnd>='{1}' and Param.ParamId=211),0) " + str1 + "order by d.Perfomer.NameOrgMin";
          // ISSUE: variable of a boxed type
          short companyId = ((Company) this.cmbCompany.SelectedItem).CompanyId;
          periodName = KvrplHelper.GetCmpKvrClose((Company) this.cmbCompany.SelectedItem, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk).PeriodName;
          dateTime = periodName.Value;
          string baseFormat = KvrplHelper.DateToBaseFormat(dateTime.AddMonths(1));
          // ISSUE: variable of a boxed type
          int local = num;
          IList<BaseOrg> baseOrgList2 = this.session.CreateQuery(string.Format(format, (object) companyId, (object) baseFormat, (object) local)).List<BaseOrg>();
          baseOrgList2.Insert(0, new BaseOrg(0, ""));
          row.Cells["Perfomer"] = (DataGridViewCell) new DataGridViewComboBoxCell()
          {
            DisplayStyleForCurrentCellOnly = true,
            ValueMember = "BaseOrgId",
            DisplayMember = "NameOrgMin",
            DataSource = (object) baseOrgList2
          };
          if (((CmpHmReceipt) row.DataBoundItem).Supplier != null)
            row.Cells["Perfomer"].Value = (object) ((CmpHmReceipt) row.DataBoundItem).Supplier.Perfomer.BaseOrgId;
        }
      }
      this.LoadSettingsCmpHmReceipt();
    }

    private void dgvHmReceipt_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (!this.dgvHmReceipt.IsCurrentCellDirty)
        return;
      this.dgvHmReceipt.CommitEdit(DataGridViewDataErrorContexts.Commit);
      if (this.dgvHmReceipt.Columns["Service"] != null && this.dgvHmReceipt.CurrentCell.ColumnIndex == this.dgvHmReceipt.Rows[this.dgvHmReceipt.CurrentRow.Index].Cells["Service"].ColumnIndex)
      {
        this.session.Clear();
        short num1 = 0;
        if (this.dgvHmReceipt.CurrentRow.Cells["Service"].Value != null)
          num1 = Convert.ToInt16(this.dgvHmReceipt.CurrentRow.Cells["Service"].Value);
        IList<Service> serviceList = this.session.CreateQuery(string.Format("select s from Service s where s.Root={0} order by " + Options.SortService, (object) num1)).List<Service>();
        this.dgvHmReceipt.CurrentRow.Cells["Sost"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "ServiceId",
          DisplayMember = "ServiceName",
          DataSource = (object) serviceList
        };
        if (this.dgvHmReceipt.Columns["Perfomer"] != null)
        {
          string str = "";
          if ((uint) num1 > 0U)
            str = string.Format(" and (s.Service.ServiceId={0} or s.Service.ServiceId=(select Root from Service where ServiceId={0}))", (object) Convert.ToInt16(this.dgvHmReceipt.CurrentRow.Cells["Service"].Value));
          IList<BaseOrg> baseOrgList = this.session.CreateQuery(string.Format("Select distinct d.Perfomer from CmpSupplier s,Supplier d where s.SupplierOrg.SupplierId=d.SupplierId  and d.Perfomer.BaseOrgId<>0 and s.Company.CompanyId=isnull((select ParamValue from CompanyParam where Period.PeriodId=0 and Company.CompanyId={0} and DBeg<='{2}' and DEnd>='{2}'and Param.ParamId=211),0) " + str + " order by d.Perfomer.NameOrgMin", (object) ((Company) this.cmbCompany.SelectedItem).CompanyId, (object) num1, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.GetCmpKvrClose((Company) this.cmbCompany.SelectedItem, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk).PeriodName.Value.AddMonths(1)))).List<BaseOrg>();
          baseOrgList.Insert(0, new BaseOrg(0, ""));
          this.dgvHmReceipt.CurrentRow.Cells["Perfomer"] = (DataGridViewCell) new DataGridViewComboBoxCell()
          {
            DisplayStyleForCurrentCellOnly = true,
            ValueMember = "BaseOrgId",
            DisplayMember = "NameOrgMin",
            DataSource = (object) baseOrgList
          };
        }
        if (this.dgvHmReceipt.Columns["Recipient"] != null)
        {
          int num2 = 0;
          num2 = this.dgvHmReceipt.Columns["Recipient"] == null ? ((BaseOrg) this.cmbRecipient.SelectedItem).BaseOrgId : Convert.ToInt32(this.dgvHmReceipt.CurrentRow.Cells["Recipient"].Value);
          string str = "";
          if ((uint) num1 > 0U)
            str = string.Format(" and (s.Service.ServiceId={0} or s.Service.ServiceId=(select Root from Service where ServiceId={0}))", (object) Convert.ToInt16(this.dgvHmReceipt.CurrentRow.Cells["Service"].Value));
          IList<BaseOrg> baseOrgList = this.session.CreateQuery(string.Format("Select distinct d.Recipient from CmpSupplier s,Supplier d where s.SupplierOrg.SupplierId=d.SupplierId and d.Recipient.BaseOrgId<>0 and s.Company.CompanyId=isnull((select ParamValue from CompanyParam where Period.PeriodId=0 and Company.CompanyId={0} and DBeg<='{2}' and DEnd>='{2}' and Param.ParamId=211),0) " + str + " order by d.Recipient.NameOrgMin", (object) ((Company) this.cmbCompany.SelectedItem).CompanyId, (object) num1, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.GetCmpKvrClose((Company) this.cmbCompany.SelectedItem, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk).PeriodName.Value.AddMonths(1)))).List<BaseOrg>();
          baseOrgList.Insert(0, new BaseOrg(0, ""));
          this.dgvHmReceipt.CurrentRow.Cells["Recipient"] = (DataGridViewCell) new DataGridViewComboBoxCell()
          {
            DisplayStyleForCurrentCellOnly = true,
            ValueMember = "BaseOrgId",
            DisplayMember = "NameOrgMin",
            DataSource = (object) baseOrgList
          };
        }
      }
      if (this.dgvHmReceipt.Columns["Recipient"] != null && this.dgvHmReceipt.Columns["Perfomer"] != null && this.dgvHmReceipt.CurrentCell.ColumnIndex == this.dgvHmReceipt.Rows[this.dgvHmReceipt.CurrentRow.Index].Cells["Recipient"].ColumnIndex)
      {
        string str = "";
        if (this.dgvHmReceipt.Columns["Service"] != null && this.dgvHmReceipt.CurrentRow.Cells["Service"].Value != null)
          str = string.Format(" and (s.Service.ServiceId={0} or s.Service.ServiceId=(select Root from Service where ServiceId={0}))", (object) Convert.ToInt16(this.dgvHmReceipt.CurrentRow.Cells["Service"].Value));
        IList<BaseOrg> baseOrgList = this.session.CreateQuery(string.Format("Select distinct d.Perfomer from CmpSupplier s,Supplier d where s.SupplierOrg.SupplierId=d.SupplierId and d.Recipient.BaseOrgId={2} and d.Perfomer.BaseOrgId<>0 and s.Company.CompanyId=isnull((select ParamValue from CompanyParam where Period.PeriodId=0 and Company.CompanyId={0} and DBeg<='{1}' and DEnd>='{1}' and Param.ParamId=211),0) " + str + " order by d.Perfomer.NameOrgMin", (object) ((Company) this.cmbCompany.SelectedItem).CompanyId, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.GetCmpKvrClose((Company) this.cmbCompany.SelectedItem, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk).PeriodName.Value.AddMonths(1)), (object) Convert.ToInt32(this.dgvHmReceipt.CurrentRow.Cells["Recipient"].Value))).List<BaseOrg>();
        this.session.CreateQuery(string.Format("select ParamValue from CompanyParam where Period.PeriodId=0 and Company.CompanyId={0} and DBeg<='{1}' and DEnd>='{1}' and Param.ParamId=211 ", (object) ((Company) this.cmbCompany.SelectedItem).CompanyId, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.GetCmpKvrClose((Company) this.cmbCompany.SelectedItem, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk).PeriodName.Value), (object) Convert.ToInt32(this.dgvHmReceipt.CurrentRow.Cells["Recipient"].Value))).UniqueResult();
        if (Convert.ToInt32(this.dgvHmReceipt.CurrentRow.Cells["Recipient"].Value) == 0)
          baseOrgList.Insert(0, new BaseOrg(0, ""));
        this.dgvHmReceipt.CurrentRow.Cells["Perfomer"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "BaseOrgId",
          DisplayMember = "NameOrgMin",
          DataSource = (object) baseOrgList
        };
        this.dgvHmReceipt.CurrentRow.Cells["Perfomer"].Value = (object) baseOrgList[0].BaseOrgId;
      }
    }

    private void tsbAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, (Company) this.cmbCompany.SelectedItem, true))
        return;
      CmpHmReceipt cmpHmReceipt1 = new CmpHmReceipt();
      cmpHmReceipt1.Company = (Company) this.cmbCompany.SelectedItem;
      cmpHmReceipt1.Complex = this.Complex;
      if ((Kvartplata.Classes.Receipt) this.cmbReceipt.SelectedItem != null && (uint) ((Kvartplata.Classes.Receipt) this.cmbReceipt.SelectedItem).ReceiptId > 0U)
        cmpHmReceipt1.Receipt = (Kvartplata.Classes.Receipt) this.cmbReceipt.SelectedItem;
      if ((Home) this.cmbHome.SelectedItem != null && (uint) ((Home) this.cmbHome.SelectedItem).IdHome > 0U)
        cmpHmReceipt1.HomeId = ((Home) this.cmbHome.SelectedItem).IdHome;
      if ((Service) this.cmbService.SelectedItem != null && (uint) ((Service) this.cmbService.SelectedItem).ServiceId > 0U)
      {
        int num = (Service) this.cmbSost.SelectedItem == null ? 0 : ((uint) ((Service) this.cmbSost.SelectedItem).ServiceId > 0U ? 1 : 0);
        cmpHmReceipt1.Service = num == 0 ? (Service) this.cmbService.SelectedItem : (Service) this.cmbSost.SelectedItem;
      }
      if ((BaseOrg) this.cmbRecipient.SelectedItem != null && (BaseOrg) this.cmbPerfomer.SelectedItem != null && ((BaseOrg) this.cmbRecipient.SelectedItem).BaseOrgId != 0 && (uint) ((BaseOrg) this.cmbPerfomer.SelectedItem).BaseOrgId > 0U)
      {
        try
        {
          cmpHmReceipt1.Supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) ((BaseOrg) this.cmbRecipient.SelectedItem).BaseOrgId, (object) ((BaseOrg) this.cmbPerfomer.SelectedItem).BaseOrgId)).List<Supplier>()[0];
        }
        catch
        {
          int num = (int) MessageBox.Show("Пара получатель - исполнитель не найдена. Невозможно завести запись.");
          return;
        }
      }
      Kvartplata.Classes.Period cmpKvrClose = KvrplHelper.GetCmpKvrClose(cmpHmReceipt1.Company, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk);
      DateTime? periodName = cmpKvrClose.PeriodName;
      DateTime dateTime1 = periodName.Value;
      DateTime dateTime2 = dateTime1.AddMonths(1);
      periodName = Options.Period.PeriodName;
      DateTime dateTime3 = periodName.Value;
      if (dateTime2 <= dateTime3)
      {
        CmpHmReceipt cmpHmReceipt2 = cmpHmReceipt1;
        periodName = Options.Period.PeriodName;
        DateTime dateTime4 = periodName.Value;
        cmpHmReceipt2.DBeg = dateTime4;
      }
      else
      {
        CmpHmReceipt cmpHmReceipt2 = cmpHmReceipt1;
        periodName = cmpKvrClose.PeriodName;
        dateTime1 = periodName.Value;
        DateTime dateTime4 = dateTime1.AddMonths(1);
        cmpHmReceipt2.DBeg = dateTime4;
      }
      cmpHmReceipt1.DEnd = Convert.ToDateTime("2999-12-31");
      cmpHmReceipt1.IsEdit = true;
      IList<CmpHmReceipt> cmpHmReceiptList = (IList<CmpHmReceipt>) new List<CmpHmReceipt>();
      if ((uint) this.dgvHmReceipt.Rows.Count > 0U)
        cmpHmReceiptList = (IList<CmpHmReceipt>) (this.dgvHmReceipt.DataSource as List<CmpHmReceipt>);
      cmpHmReceiptList.Add(cmpHmReceipt1);
      this.dgvHmReceipt.Columns.Clear();
      this.dgvHmReceipt.DataSource = (object) null;
      this.dgvHmReceipt.DataSource = (object) cmpHmReceiptList;
      this.SetViewReceiptList();
      this.dgvHmReceipt.CurrentCell = this.dgvHmReceipt.Rows[this.dgvHmReceipt.Rows.Count - 1].Cells[0];
      this.tsbApply.Enabled = true;
      this.tsbDelete.Enabled = false;
    }

    private void tsbApply_Click(object sender, EventArgs e)
    {
      bool flag = false;
      foreach (DataGridViewRow row in (IEnumerable) this.dgvHmReceipt.Rows)
      {
        this.dgvHmReceipt.CurrentCell = row.Cells["DBeg"];
        row.Selected = true;
        if (((CmpHmReceipt) row.DataBoundItem).IsEdit)
        {
          this.oldReceipt = new CmpHmReceipt();
          foreach (CmpHmReceipt oldListReceipt in (IEnumerable<CmpHmReceipt>) this.oldListReceipts)
          {
            if (oldListReceipt.OldHashCode == ((CmpHmReceipt) row.DataBoundItem).OldHashCode)
            {
              this.oldReceipt = oldListReceipt;
              break;
            }
          }
          if (!this.SaveReceipt())
            flag = true;
          ((CmpHmReceipt) row.DataBoundItem).IsEdit = false;
        }
      }
      this.tsbAdd.Enabled = true;
      this.tsbDelete.Enabled = true;
      this.tsbApply.Enabled = false;
      if (flag)
        return;
      this.LoadList();
    }

    private bool SaveReceipt()
    {
      if (!KvrplHelper.CheckProxy(33, 2, (Company) this.cmbCompany.SelectedItem, true))
        return false;
      this.session.Clear();
      CmpHmReceipt dataBoundItem = (CmpHmReceipt) this.dgvHmReceipt.CurrentRow.DataBoundItem;
      bool flag = string.IsNullOrEmpty(dataBoundItem.UName);
      if (this.dgvHmReceipt.CurrentRow.Cells["DBeg"].Value != null)
      {
        try
        {
          dataBoundItem.DBeg = KvrplHelper.FirstDay(Convert.ToDateTime(this.dgvHmReceipt.CurrentRow.Cells["DBeg"].Value));
        }
        catch
        {
          int num = (int) MessageBox.Show("Проверьте правильность введенных дат", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (this.dgvHmReceipt.CurrentRow.Cells["DEnd"].Value != null)
        {
          try
          {
            dataBoundItem.DEnd = KvrplHelper.LastDay(Convert.ToDateTime(this.dgvHmReceipt.CurrentRow.Cells["DEnd"].Value));
          }
          catch
          {
            int num = (int) MessageBox.Show("Проверьте правильность введенных дат", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
          DateTime dateTime = KvrplHelper.LastDay(this.monthclosed.PeriodName.Value);
          int homeId = dataBoundItem.HomeId;
          if (false)
            dataBoundItem.HomeId = Convert.ToInt32(this.dgvHmReceipt.CurrentRow.Cells["Home"].Value);
          if (dataBoundItem.Supplier == null)
          {
            try
            {
              dataBoundItem.Supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(this.dgvHmReceipt.CurrentRow.Cells["Recipient"].Value), (object) Convert.ToInt32(this.dgvHmReceipt.CurrentRow.Cells["Perfomer"].Value))).List<Supplier>()[0];
            }
            catch
            {
              int num = (int) MessageBox.Show("Пара получатель - исполнитель не найдена");
              return false;
            }
          }
          if (dataBoundItem.Service == null)
            dataBoundItem.Service = this.dgvHmReceipt.CurrentRow.Cells["Sost"].Value != null ? this.session.Get<Service>((object) Convert.ToInt16(this.dgvHmReceipt.CurrentRow.Cells["Sost"].Value)) : this.session.Get<Service>((object) Convert.ToInt16(this.dgvHmReceipt.CurrentRow.Cells["Service"].Value));
          if (flag && (dataBoundItem.DBeg <= dateTime || dataBoundItem.DEnd <= dateTime))
          {
            int num = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
          }
          if (!flag && (this.oldReceipt.DBeg <= dateTime && this.oldReceipt.DEnd < dateTime || dataBoundItem.DEnd < dateTime || this.oldReceipt.DBeg > dateTime && dataBoundItem.DBeg <= dateTime || this.oldReceipt.DBeg <= dateTime && (this.oldReceipt.DBeg != dataBoundItem.DBeg || this.oldReceipt.HomeId != dataBoundItem.HomeId || (int) this.oldReceipt.Service.ServiceId != (int) dataBoundItem.Service.ServiceId || this.oldReceipt.Supplier.SupplierId != dataBoundItem.Supplier.SupplierId)))
          {
            int num = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
          }
          if ((uint) dataBoundItem.Service.ServiceId > 0U)
          {
            short? root = dataBoundItem.Service.Root;
            int? nullable = root.HasValue ? new int?((int) root.GetValueOrDefault()) : new int?();
            int num1 = 0;
            if (nullable.GetValueOrDefault() == num1 && nullable.HasValue)
            {
              if (this.session.CreateQuery(string.Format("select chr from CmpHmReceipt chr where chr.Company.CompanyId={0} and chr.HomeId={3} and chr.Complex.IdFk={1} and chr.DBeg<=:dend and chr.DEnd>=:dbeg and (select Root from Service where ServiceId=chr.Service.ServiceId)={2} and chr.Supplier.SupplierId={4} ", (object) dataBoundItem.Company.CompanyId, (object) dataBoundItem.Complex.IdFk, (object) dataBoundItem.Service.ServiceId, (object) dataBoundItem.HomeId, (object) dataBoundItem.Supplier.SupplierId)).SetDateTime("dbeg", dataBoundItem.DBeg).SetDateTime("dend", dataBoundItem.DEnd).List<CmpHmReceipt>().Count > 0)
              {
                int num2 = (int) MessageBox.Show("Невозможно завести запись по услуге, потому что существует запись по составляющей этой услуги", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
              }
            }
            else if (this.session.CreateQuery(string.Format("select chr from CmpHmReceipt chr where chr.Company.CompanyId={0} and chr.HomeId={3} and chr.Complex.IdFk={1} and chr.DBeg<=:dend and chr.DEnd>=:dbeg and (select Root from Service where ServiceId={2})=chr.Service.ServiceId and chr.Supplier.SupplierId={4} ", (object) dataBoundItem.Company.CompanyId, (object) dataBoundItem.Complex.IdFk, (object) dataBoundItem.Service.ServiceId, (object) dataBoundItem.HomeId, (object) dataBoundItem.Supplier.SupplierId)).SetDateTime("dbeg", dataBoundItem.DBeg).SetDateTime("dend", dataBoundItem.DEnd).List<CmpHmReceipt>().Count > 0)
            {
              int num2 = (int) MessageBox.Show("Невозможно завести запись по составляющей, потому что уже существует запись по услуге", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return false;
            }
          }
          dataBoundItem.UName = Options.Login;
          dataBoundItem.DEdit = DateTime.Now.Date;
          try
          {
            if (flag)
            {
              this.session.Save((object) dataBoundItem);
              this.session.Flush();
            }
            else
              this.session.CreateQuery("update CmpHmReceipt set Receipt = :newreceipt,HomeId=:newhome,Service=:newservice,Supplier.SupplierId=:newsupplier,DBeg=:newbeg,DEnd=:newend,UName=:uname,DEdit=:dedit where Company.CompanyId=:company and Complex.IdFk=:complex and Receipt.ReceiptId=:receipt and HomeId=:home and Service.ServiceId=:service and Supplier.SupplierId=:supplier and DBeg=:dbeg").SetParameter<short>("company", this.oldReceipt.Company.CompanyId).SetParameter<int>("complex", this.oldReceipt.Complex.IdFk).SetParameter<int>("home", this.oldReceipt.HomeId).SetParameter<short>("service", this.oldReceipt.Service.ServiceId).SetParameter<int>("supplier", this.oldReceipt.Supplier.SupplierId).SetParameter<short>("receipt", this.oldReceipt.Receipt.ReceiptId).SetDateTime("dbeg", dataBoundItem.DBeg).SetParameter<Kvartplata.Classes.Receipt>("newreceipt", dataBoundItem.Receipt).SetParameter<int>("newhome", dataBoundItem.HomeId).SetParameter<Service>("newservice", dataBoundItem.Service).SetParameter<int>("newsupplier", dataBoundItem.Supplier.SupplierId).SetDateTime("newbeg", dataBoundItem.DBeg).SetDateTime("newend", dataBoundItem.DEnd).SetParameter<string>("uname", dataBoundItem.UName).SetParameter<DateTime>("dedit", dataBoundItem.DEdit).ExecuteUpdate();
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Произошла ошибка. Данные не сохранены", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
          this.session.Clear();
          this.tsbAdd.Enabled = true;
          this.tsbApply.Enabled = false;
          this.tsbDelete.Enabled = true;
          this.tsbCancel.Enabled = false;
          return true;
        }
        int num3 = (int) MessageBox.Show("Дата окончания не введена", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      int num4 = (int) MessageBox.Show("Дата начала не введена", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    public void LoadSettingsCmpHmReceipt()
    {
      this.MySettingsCmpHmReceipt.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvHmReceipt.Columns)
        this.MySettingsCmpHmReceipt.GetMySettings(column);
    }

    private void dgvHmReceipt_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsCmpHmReceipt.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsCmpHmReceipt.Columns[this.MySettingsCmpHmReceipt.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsCmpHmReceipt.Save();
    }

    private void dgvHmReceipt_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.tsbApply.Enabled = true;
      this.tsbDelete.Enabled = false;
      this.tsbCancel.Enabled = true;
      ((CmpHmReceipt) this.dgvHmReceipt.CurrentRow.DataBoundItem).IsEdit = true;
    }

    private void dgvHmReceipt_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgvHmReceipt.CurrentRow == null)
        return;
      CmpHmReceipt dataBoundItem = (CmpHmReceipt) this.dgvHmReceipt.CurrentRow.DataBoundItem;
      dataBoundItem.IsEdit = true;
      if (this.dgvHmReceipt.CurrentCell.Value != null || this.dgvHmReceipt.Columns[e.ColumnIndex].Name == "Perfomer" || this.dgvHmReceipt.Columns[e.ColumnIndex].Name == "Recipient")
      {
        try
        {
          string name = this.dgvHmReceipt.Columns[e.ColumnIndex].Name;
          // ISSUE: reference to a compiler-generated method
          uint stringHash = PrivateImplementationDetails.ComputeStringHash(name);
          if (stringHash <= 1370133295U)
          {
            if ((int) stringHash != 1115145125)
            {
              if ((int) stringHash != 1298162816)
              {
                if ((int) stringHash == 1370133295)
                {
                  if (name == "DBeg")
                  {
                    try
                    {
                      dataBoundItem.DBeg = Convert.ToDateTime(this.dgvHmReceipt.CurrentRow.Cells["DBeg"].Value);
                    }
                    catch
                    {
                    }
                  }
                }
              }
              else if (name == "DEnd")
              {
                try
                {
                  dataBoundItem.DEnd = Convert.ToDateTime(this.dgvHmReceipt.CurrentRow.Cells["DEnd"].Value);
                }
                catch
                {
                }
              }
            }
            else if (name == "Perfomer")
              dataBoundItem.Supplier = ((BaseOrg) this.cmbRecipient.SelectedItem).BaseOrgId != 0 ? this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) ((BaseOrg) this.cmbRecipient.SelectedItem).BaseOrgId, (object) Convert.ToInt32(this.dgvHmReceipt.CurrentRow.Cells["Perfomer"].Value))).List<Supplier>()[0] : this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(this.dgvHmReceipt.CurrentRow.Cells["Recipient"].Value), (object) Convert.ToInt32(this.dgvHmReceipt.CurrentRow.Cells["Perfomer"].Value))).List<Supplier>()[0];
          }
          else if (stringHash <= 2119655400U)
          {
            if ((int) stringHash != 1391791790)
            {
              if ((int) stringHash == 2119655400)
              {
                if (name == "Sost")
                  dataBoundItem.Service = this.session.Get<Service>(this.dgvHmReceipt.CurrentRow.Cells["Sost"].Value);
              }
            }
            else if (name == "Home")
              dataBoundItem.HomeId = Convert.ToInt32(this.dgvHmReceipt.CurrentRow.Cells["Home"].Value);
          }
          else if ((int) stringHash != -1237072124)
          {
            if ((int) stringHash == -627760104)
            {
              if (name == "Recipient")
                dataBoundItem.Supplier = ((BaseOrg) this.cmbPerfomer.SelectedItem).BaseOrgId != 0 ? this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(this.dgvHmReceipt.CurrentRow.Cells["Recipient"].Value), (object) ((BaseOrg) this.cmbPerfomer.SelectedItem).BaseOrgId)).List<Supplier>()[0] : this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(this.dgvHmReceipt.CurrentRow.Cells["Recipient"].Value), (object) Convert.ToInt32(this.dgvHmReceipt.CurrentRow.Cells["Perfomer"].Value))).List<Supplier>()[0];
            }
          }
          else if (name == "Service")
            dataBoundItem.Service = this.session.Get<Service>(this.dgvHmReceipt.CurrentRow.Cells["Service"].Value);
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
    }

    private void dgvHmReceipt_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (this.dgvHmReceipt.DataSource == null)
        return;
      DataGridViewRow row = (sender as DataGridView).Rows[e.RowIndex];
      if (((CmpHmReceipt) row.DataBoundItem).DBeg <= KvrplHelper.LastDay(this.nextMonthClosed) && ((CmpHmReceipt) row.DataBoundItem).DEnd >= this.nextMonthClosed)
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

    private void cbArchive_CheckedChanged(object sender, EventArgs e)
    {
      this.LoadList();
    }

    private void tsbDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, (Company) this.cmbCompany.SelectedItem, true) || (this.dgvHmReceipt.Rows.Count <= 0 || this.dgvHmReceipt.CurrentRow == null))
        return;
      this.session.Clear();
      if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
      {
        CmpHmReceipt cmpHmReceipt = new CmpHmReceipt();
        CmpHmReceipt dataBoundItem = (CmpHmReceipt) this.dgvHmReceipt.CurrentRow.DataBoundItem;
        if (dataBoundItem.Supplier.SupplierId == 0)
          dataBoundItem.Supplier = this.session.Get<Supplier>((object) 0);
        if (dataBoundItem.DBeg <= KvrplHelper.LastDay(this.monthclosed.PeriodName.Value) || dataBoundItem.DEnd <= KvrplHelper.LastDay(this.monthclosed.PeriodName.Value))
        {
          int num = (int) MessageBox.Show("Не могу удалить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
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
      this.LoadList();
    }

    private void tsbCancel_Click(object sender, EventArgs e)
    {
      this.LoadList();
    }

    private void cmbComplex_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.Complex = (Complex) this.cmbComplex.SelectedItem;
      this.LoadList();
    }

    private void dgvReceiptParam_SelectionChanged(object sender, EventArgs e)
    {
      if (this.dgvReceiptParam.CurrentRow == null || this.dgvReceiptParam.CurrentRow.Index >= this.rcParams.Count)
        return;
      this.curObject = (ReceiptParam) this.dgvReceiptParam.Rows[this.dgvReceiptParam.CurrentRow.Index].DataBoundItem;
    }

    private void tsbDeleteReceiptParam_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      if (this.curObject != null)
      {
        try
        {
          this.session.Delete((object) this.curObject);
          this.session.Flush();
          this.bs.RemoveCurrent();
          this.bs.EndEdit();
          this.dgvReceiptParam.Update();
          this.curObject = (ReceiptParam) null;
          this.LoadReceiptParam();
        }
        catch
        {
          int num = (int) MessageBox.Show("Удаление невозможно! Существуют данные, ссылающиеся на эту запись.", "Внимание!", MessageBoxButtons.OK);
          this.session.Clear();
          this.session = Domain.CurrentSession;
          this.curObject = (ReceiptParam) null;
          this.LoadReceiptParam();
        }
      }
    }

    private void btnPastTime_Click(object sender, EventArgs e)
    {
      if (!this.PastTime)
      {
        this.PastTime = true;
        this.lblPastTime.Visible = true;
        this.btnPastTime.BackColor = Color.DarkOrange;
        this.tmr.Start();
      }
      else
      {
        this.lblPastTime.Visible = false;
        this.btnPastTime.BackColor = this.pnUp.BackColor;
        this.PastTime = false;
        this.tmr.Stop();
      }
      this.LoadReceiptParam();
    }

    private void tmr_Tick(object sender, EventArgs e)
    {
      if (this.lblPastTime.ForeColor == Color.DarkOrange)
        this.lblPastTime.ForeColor = this.BackColor;
      else
        this.lblPastTime.ForeColor = Color.DarkOrange;
    }

    private void chbArhiv_CheckedChanged(object sender, EventArgs e)
    {
      this.Arhiv = this.chbArhiv.Checked;
      this.LoadReceiptParam();
    }

    private void dgvReceiptParam_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (this.dgvReceiptParam.DataSource == null)
        return;
      DataGridViewRow row = (sender as DataGridView).Rows[e.RowIndex];
      try
      {
        DateTime dbeg = ((ReceiptParam) row.DataBoundItem).DBeg;
        DateTime? periodName = this.NextMonthClosed.PeriodName;
        DateTime dateTime1 = KvrplHelper.LastDay(periodName.Value);
        int num;
        if (dbeg <= dateTime1)
        {
          DateTime dend = ((ReceiptParam) row.DataBoundItem).DEnd;
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
      catch (Exception ex)
      {
      }
    }

    private void dgvReceiptParam_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.RowIndex == -1 || e.ColumnIndex == -1)
        return;
      this.dgvReceiptParam.Rows[e.RowIndex].Selected = true;
      this.dgvReceiptParam.CurrentCell = this.dgvReceiptParam.Rows[e.RowIndex].Cells[e.ColumnIndex];
      this.SelectedIndexHomeParam = this.dgvReceiptParam.Rows[e.RowIndex].Index;
    }

    private Kvartplata.Classes.Period GetClosedPeriod()
    {
      return this.session.Get<Kvartplata.Classes.Period>((object) Convert.ToInt32(this.session.CreateQuery(string.Format("select max(p.Period_id) from ClosedPeriod p where (p.Complex_id = {0} or p.Complex_id = {2}) and p.Company_id = {1}", (object) Options.ComplexPasp.ComplexId, (object) this.Company_id, (object) Options.ComplexPrior.IdFk)).UniqueResult()));
    }

    private Kvartplata.Classes.Period GetOpenPeriod()
    {
      return this.session.Get<Kvartplata.Classes.Period>((object) (Convert.ToInt32(this.session.CreateQuery(string.Format("select max(p.Period_id) from ClosedPeriod p where (p.Complex_id = {0} or p.Complex_id = {2}) and p.Company_id = {1}", (object) Options.ComplexPasp.ComplexId, (object) this.Company_id, (object) Options.ComplexPrior.IdFk)).UniqueResult()) + 1));
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmReceipt));
      this.dataColumn2 = new DataColumn();
      this.dataColumn1 = new DataColumn();
      this.dtYesNo = new DataTable();
      this.dsMain = new DataSet();
      this.toolStrip1 = new ToolStrip();
      this.tsbReceipt = new ToolStripButton();
      this.tsbCmpReceipt = new ToolStripButton();
      this.tsbHmReceipt = new ToolStripButton();
      this.tdbReceiptParam = new ToolStripButton();
      this.tsbExit = new ToolStripButton();
      this.pnlReceipt = new Panel();
      this.ucReceipt1 = new UCReceipt();
      this.pnlHmReceipt = new Panel();
      this.dgvHmReceipt = new DataGridView();
      this.toolStrip2 = new ToolStrip();
      this.tsbAdd = new ToolStripButton();
      this.tsbApply = new ToolStripButton();
      this.tsbCancel = new ToolStripButton();
      this.tsbDelete = new ToolStripButton();
      this.pnUp = new Panel();
      this.cmbPerfomer = new ComboBox();
      this.lblPerfomer = new Label();
      this.cmbComplex = new ComboBox();
      this.lblComplex = new Label();
      this.cbArchive = new CheckBox();
      this.cmbRecipient = new ComboBox();
      this.lblCompany = new Label();
      this.cmbSost = new ComboBox();
      this.lblReceipt = new Label();
      this.cmbService = new ComboBox();
      this.lblHome = new Label();
      this.cmbHome = new ComboBox();
      this.lblService = new Label();
      this.cmbReceipt = new ComboBox();
      this.lblSost = new Label();
      this.cmbCompany = new ComboBox();
      this.lblSupplier = new Label();
      this.pnlCmpReceipt = new Panel();
      this.ucCmpReceipt1 = new UCCmpReceipt();
      this.groupBox1 = new GroupBox();
      this.cbCompany = new ComboBox();
      this.label1 = new Label();
      this.hp = new HelpProvider();
      this.pnReceiptParam = new Panel();
      this.dgvReceiptParam = new DataGridView();
      this.Receipt = new DataGridViewComboBoxColumn();
      this.Param = new DataGridViewComboBoxColumn();
      this.PValue = new DataGridViewTextBoxColumn();
      this.DBeg = new MaskDateColumn();
      this.Period = new MaskDateColumn();
      this.DEnd = new MaskDateColumn();
      this.UName = new DataGridViewTextBoxColumn();
      this.DEdit = new DataGridViewTextBoxColumn();
      this.toolStrip3 = new ToolStrip();
      this.tsbAddReceiptParam = new ToolStripButton();
      this.tsbApplayReceiptParam = new ToolStripButton();
      this.tsbCancelReceiptParam = new ToolStripButton();
      this.tsbDeleteReceiptParam = new ToolStripButton();
      this.toolStripButton4 = new ToolStripButton();
      this.groupBox2 = new GroupBox();
      this.lblPastTime = new Label();
      this.btnPastTime = new Button();
      this.chbArhiv = new CheckBox();
      this.cmbCompanyParam = new ComboBox();
      this.label2 = new Label();
      this.maskDateColumn1 = new MaskDateColumn();
      this.maskDateColumn2 = new MaskDateColumn();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.tmr = new Timer(this.components);
      this.dtYesNo.BeginInit();
      this.dsMain.BeginInit();
      this.toolStrip1.SuspendLayout();
      this.pnlReceipt.SuspendLayout();
      this.pnlHmReceipt.SuspendLayout();
      ((ISupportInitialize) this.dgvHmReceipt).BeginInit();
      this.toolStrip2.SuspendLayout();
      this.pnUp.SuspendLayout();
      this.pnlCmpReceipt.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.pnReceiptParam.SuspendLayout();
      ((ISupportInitialize) this.dgvReceiptParam).BeginInit();
      this.toolStrip3.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      this.dataColumn2.ColumnName = "Name";
      this.dataColumn1.ColumnName = "ID";
      this.dataColumn1.DataType = typeof (short);
      this.dtYesNo.Columns.AddRange(new DataColumn[2]
      {
        this.dataColumn1,
        this.dataColumn2
      });
      this.dtYesNo.TableName = "Table1";
      this.dsMain.DataSetName = "NewDataSet";
      this.dsMain.Tables.AddRange(new DataTable[1]
      {
        this.dtYesNo
      });
      this.toolStrip1.Dock = DockStyle.Left;
      this.toolStrip1.Font = new Font("Tahoma", 10f);
      this.toolStrip1.ImageScalingSize = new Size(48, 48);
      this.toolStrip1.Items.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.tsbReceipt,
        (ToolStripItem) this.tsbCmpReceipt,
        (ToolStripItem) this.tsbHmReceipt,
        (ToolStripItem) this.tdbReceiptParam,
        (ToolStripItem) this.tsbExit
      });
      this.toolStrip1.Location = new Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Padding = new Padding(0, 0, 2, 0);
      this.toolStrip1.Size = new Size(189, 597);
      this.toolStrip1.TabIndex = 0;
      this.toolStrip1.Text = "toolStrip1";
      this.toolStrip1.ItemClicked += new ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
      this.tsbReceipt.Checked = true;
      this.tsbReceipt.CheckOnClick = true;
      this.tsbReceipt.CheckState = CheckState.Checked;
      this.tsbReceipt.Font = new Font("Tahoma", 10f);
      this.tsbReceipt.Image = (Image) Resources.allClasses;
      this.tsbReceipt.ImageTransparentColor = Color.Magenta;
      this.tsbReceipt.Name = "tsbReceipt";
      this.tsbReceipt.Size = new Size(184, 69);
      this.tsbReceipt.Text = "Словарь квитанций";
      this.tsbReceipt.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tsbReceipt.Click += new EventHandler(this.tsbReceipt_Click);
      this.tsbCmpReceipt.CheckOnClick = true;
      this.tsbCmpReceipt.Image = (Image) Resources.documents;
      this.tsbCmpReceipt.ImageTransparentColor = Color.Magenta;
      this.tsbCmpReceipt.Name = "tsbCmpReceipt";
      this.tsbCmpReceipt.Size = new Size(184, 69);
      this.tsbCmpReceipt.Text = "Квитанции";
      this.tsbCmpReceipt.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tsbCmpReceipt.Click += new EventHandler(this.tsbCmpReceipt_Click);
      this.tsbHmReceipt.CheckOnClick = true;
      this.tsbHmReceipt.Image = (Image) componentResourceManager.GetObject("tsbHmReceipt.Image");
      this.tsbHmReceipt.ImageTransparentColor = Color.Magenta;
      this.tsbHmReceipt.Name = "tsbHmReceipt";
      this.tsbHmReceipt.Size = new Size(184, 69);
      this.tsbHmReceipt.Text = "Разделение по квитанциям";
      this.tsbHmReceipt.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tsbHmReceipt.Click += new EventHandler(this.toolStripButton1_Click);
      this.tdbReceiptParam.CheckOnClick = true;
      this.tdbReceiptParam.Image = (Image) Resources.percent;
      this.tdbReceiptParam.ImageTransparentColor = Color.Magenta;
      this.tdbReceiptParam.Name = "tdbReceiptParam";
      this.tdbReceiptParam.Size = new Size(184, 69);
      this.tdbReceiptParam.Text = "Параметры по квитанции";
      this.tdbReceiptParam.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tdbReceiptParam.Click += new EventHandler(this.tdbReceiptParam_Click);
      this.tsbExit.Image = (Image) Resources.Exit;
      this.tsbExit.ImageTransparentColor = Color.Magenta;
      this.tsbExit.Name = "tsbExit";
      this.tsbExit.Size = new Size(184, 69);
      this.tsbExit.Text = "Выход";
      this.tsbExit.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tsbExit.Click += new EventHandler(this.tsbExit_Click);
      this.pnlReceipt.Controls.Add((Control) this.ucReceipt1);
      this.pnlReceipt.Dock = DockStyle.Fill;
      this.pnlReceipt.Location = new Point(189, 0);
      this.pnlReceipt.Margin = new Padding(4);
      this.pnlReceipt.Name = "pnlReceipt";
      this.pnlReceipt.Size = new Size(1075, 597);
      this.pnlReceipt.TabIndex = 1;
      this.ucReceipt1.Dock = DockStyle.Fill;
      this.ucReceipt1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.ucReceipt1.Location = new Point(0, 0);
      this.ucReceipt1.Margin = new Padding(5);
      this.ucReceipt1.Name = "ucReceipt1";
      this.ucReceipt1.Size = new Size(1075, 597);
      this.ucReceipt1.TabIndex = 0;
      this.ucReceipt1.ObjectsListChanged += new EventHandler(this.ucReceipt1_ObjectsListChanged);
      this.ucReceipt1.CurObjectChanged += new EventHandler(this.ucReceipt1_CurObjectChanged);
      this.pnlHmReceipt.Controls.Add((Control) this.dgvHmReceipt);
      this.pnlHmReceipt.Controls.Add((Control) this.toolStrip2);
      this.pnlHmReceipt.Controls.Add((Control) this.pnUp);
      this.pnlHmReceipt.Dock = DockStyle.Fill;
      this.pnlHmReceipt.Location = new Point(189, 0);
      this.pnlHmReceipt.Name = "pnlHmReceipt";
      this.pnlHmReceipt.Size = new Size(1075, 597);
      this.pnlHmReceipt.TabIndex = 1;
      this.dgvHmReceipt.BackgroundColor = Color.AliceBlue;
      this.dgvHmReceipt.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvHmReceipt.Dock = DockStyle.Fill;
      this.dgvHmReceipt.Location = new Point(0, 305);
      this.dgvHmReceipt.Name = "dgvHmReceipt";
      this.dgvHmReceipt.Size = new Size(1075, 292);
      this.dgvHmReceipt.TabIndex = 12;
      this.dgvHmReceipt.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvHmReceipt_CellBeginEdit);
      this.dgvHmReceipt.CellEndEdit += new DataGridViewCellEventHandler(this.dgvHmReceipt_CellEndEdit);
      this.dgvHmReceipt.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvHmReceipt_CellFormatting);
      this.dgvHmReceipt.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvHmReceipt_ColumnWidthChanged);
      this.dgvHmReceipt.CurrentCellDirtyStateChanged += new EventHandler(this.dgvHmReceipt_CurrentCellDirtyStateChanged);
      this.dgvHmReceipt.DataError += new DataGridViewDataErrorEventHandler(this.dgvHmReceipt_DataError);
      this.toolStrip2.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.tsbAdd,
        (ToolStripItem) this.tsbApply,
        (ToolStripItem) this.tsbCancel,
        (ToolStripItem) this.tsbDelete
      });
      this.toolStrip2.Location = new Point(0, 280);
      this.toolStrip2.Name = "toolStrip2";
      this.toolStrip2.Size = new Size(1075, 25);
      this.toolStrip2.TabIndex = 14;
      this.toolStrip2.Text = "toolStrip2";
      this.tsbAdd.Font = new Font("Tahoma", 10f);
      this.tsbAdd.Image = (Image) Resources.add_var;
      this.tsbAdd.ImageTransparentColor = Color.Magenta;
      this.tsbAdd.Name = "tsbAdd";
      this.tsbAdd.Size = new Size(91, 22);
      this.tsbAdd.Text = "Добавить";
      this.tsbAdd.Click += new EventHandler(this.tsbAdd_Click);
      this.tsbApply.Font = new Font("Tahoma", 10f);
      this.tsbApply.Image = (Image) Resources.Applay_var;
      this.tsbApply.ImageTransparentColor = Color.Magenta;
      this.tsbApply.Name = "tsbApply";
      this.tsbApply.Size = new Size(99, 22);
      this.tsbApply.Text = "Сохранить";
      this.tsbApply.Click += new EventHandler(this.tsbApply_Click);
      this.tsbCancel.Font = new Font("Tahoma", 10f);
      this.tsbCancel.Image = (Image) Resources.undo;
      this.tsbCancel.ImageTransparentColor = Color.Magenta;
      this.tsbCancel.Name = "tsbCancel";
      this.tsbCancel.Size = new Size(77, 22);
      this.tsbCancel.Text = "Отмена";
      this.tsbCancel.Click += new EventHandler(this.tsbCancel_Click);
      this.tsbDelete.Font = new Font("Tahoma", 10f);
      this.tsbDelete.Image = (Image) Resources.delete_var;
      this.tsbDelete.ImageTransparentColor = Color.Magenta;
      this.tsbDelete.Name = "tsbDelete";
      this.tsbDelete.Size = new Size(82, 22);
      this.tsbDelete.Text = "Удалить";
      this.tsbDelete.Click += new EventHandler(this.tsbDelete_Click);
      this.pnUp.Controls.Add((Control) this.cmbPerfomer);
      this.pnUp.Controls.Add((Control) this.lblPerfomer);
      this.pnUp.Controls.Add((Control) this.cmbComplex);
      this.pnUp.Controls.Add((Control) this.lblComplex);
      this.pnUp.Controls.Add((Control) this.cbArchive);
      this.pnUp.Controls.Add((Control) this.cmbRecipient);
      this.pnUp.Controls.Add((Control) this.lblCompany);
      this.pnUp.Controls.Add((Control) this.cmbSost);
      this.pnUp.Controls.Add((Control) this.lblReceipt);
      this.pnUp.Controls.Add((Control) this.cmbService);
      this.pnUp.Controls.Add((Control) this.lblHome);
      this.pnUp.Controls.Add((Control) this.cmbHome);
      this.pnUp.Controls.Add((Control) this.lblService);
      this.pnUp.Controls.Add((Control) this.cmbReceipt);
      this.pnUp.Controls.Add((Control) this.lblSost);
      this.pnUp.Controls.Add((Control) this.cmbCompany);
      this.pnUp.Controls.Add((Control) this.lblSupplier);
      this.pnUp.Dock = DockStyle.Top;
      this.pnUp.Location = new Point(0, 0);
      this.pnUp.Name = "pnUp";
      this.pnUp.Size = new Size(1075, 280);
      this.pnUp.TabIndex = 13;
      this.cmbPerfomer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbPerfomer.DisplayMember = "NameOrgMin";
      this.cmbPerfomer.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbPerfomer.FormattingEnabled = true;
      this.cmbPerfomer.Location = new Point(120, 218);
      this.cmbPerfomer.Name = "cmbPerfomer";
      this.cmbPerfomer.Size = new Size(950, 24);
      this.cmbPerfomer.TabIndex = 15;
      this.cmbPerfomer.ValueMember = "BaseOrgId";
      this.cmbPerfomer.SelectionChangeCommitted += new EventHandler(this.cmbReceipt_SelectionChangeCommitted);
      this.lblPerfomer.AutoSize = true;
      this.lblPerfomer.Location = new Point(7, 221);
      this.lblPerfomer.Name = "lblPerfomer";
      this.lblPerfomer.Size = new Size(95, 17);
      this.lblPerfomer.TabIndex = 14;
      this.lblPerfomer.Text = "Исполнитель";
      this.cmbComplex.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbComplex.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbComplex.FormattingEnabled = true;
      this.cmbComplex.Location = new Point(120, 5);
      this.cmbComplex.Name = "cmbComplex";
      this.cmbComplex.Size = new Size(950, 24);
      this.cmbComplex.TabIndex = 13;
      this.cmbComplex.SelectionChangeCommitted += new EventHandler(this.cmbComplex_SelectionChangeCommitted);
      this.lblComplex.AutoSize = true;
      this.lblComplex.Location = new Point(7, 8);
      this.lblComplex.Name = "lblComplex";
      this.lblComplex.Size = new Size(72, 17);
      this.lblComplex.TabIndex = 12;
      this.lblComplex.Text = "Комплекс";
      this.cbArchive.AutoSize = true;
      this.cbArchive.Location = new Point(10, 241);
      this.cbArchive.Name = "cbArchive";
      this.cbArchive.Size = new Size(65, 21);
      this.cbArchive.TabIndex = 0;
      this.cbArchive.Text = "Архив";
      this.cbArchive.UseVisualStyleBackColor = true;
      this.cbArchive.CheckedChanged += new EventHandler(this.cbArchive_CheckedChanged);
      this.cmbRecipient.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbRecipient.DisplayMember = "NameOrgMin";
      this.cmbRecipient.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbRecipient.FormattingEnabled = true;
      this.cmbRecipient.Location = new Point(120, 187);
      this.cmbRecipient.Name = "cmbRecipient";
      this.cmbRecipient.Size = new Size(950, 24);
      this.cmbRecipient.TabIndex = 11;
      this.cmbRecipient.ValueMember = "BaseOrgId";
      this.cmbRecipient.SelectionChangeCommitted += new EventHandler(this.cmbReceipt_SelectionChangeCommitted);
      this.lblCompany.AutoSize = true;
      this.lblCompany.Location = new Point(7, 38);
      this.lblCompany.Name = "lblCompany";
      this.lblCompany.Size = new Size(74, 17);
      this.lblCompany.TabIndex = 0;
      this.lblCompany.Text = "Компания";
      this.cmbSost.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbSost.DisplayMember = "ServiceName";
      this.cmbSost.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbSost.FormattingEnabled = true;
      this.cmbSost.Location = new Point(120, 156);
      this.cmbSost.Name = "cmbSost";
      this.cmbSost.Size = new Size(950, 24);
      this.cmbSost.TabIndex = 10;
      this.cmbSost.ValueMember = "ServiceId";
      this.cmbSost.SelectionChangeCommitted += new EventHandler(this.cmbReceipt_SelectionChangeCommitted);
      this.lblReceipt.AutoSize = true;
      this.lblReceipt.Location = new Point(7, 68);
      this.lblReceipt.Name = "lblReceipt";
      this.lblReceipt.Size = new Size(79, 17);
      this.lblReceipt.TabIndex = 1;
      this.lblReceipt.Text = "Квитанция";
      this.cmbService.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbService.DisplayMember = "ServiceName";
      this.cmbService.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbService.FormattingEnabled = true;
      this.cmbService.Location = new Point(120, 126);
      this.cmbService.Name = "cmbService";
      this.cmbService.Size = new Size(950, 24);
      this.cmbService.TabIndex = 9;
      this.cmbService.ValueMember = "ServiceId";
      this.cmbService.SelectionChangeCommitted += new EventHandler(this.cmbService_SelectionChangeCommitted);
      this.lblHome.AutoSize = true;
      this.lblHome.Location = new Point(7, 98);
      this.lblHome.Name = "lblHome";
      this.lblHome.Size = new Size(36, 17);
      this.lblHome.TabIndex = 2;
      this.lblHome.Text = "Дом";
      this.cmbHome.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbHome.DisplayMember = "Address";
      this.cmbHome.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbHome.FormattingEnabled = true;
      this.cmbHome.Location = new Point(120, 96);
      this.cmbHome.Name = "cmbHome";
      this.cmbHome.Size = new Size(950, 24);
      this.cmbHome.TabIndex = 8;
      this.cmbHome.ValueMember = "IdHome";
      this.cmbHome.SelectionChangeCommitted += new EventHandler(this.cmbReceipt_SelectionChangeCommitted);
      this.lblService.AutoSize = true;
      this.lblService.Location = new Point(7, 129);
      this.lblService.Name = "lblService";
      this.lblService.Size = new Size(52, 17);
      this.lblService.TabIndex = 3;
      this.lblService.Text = "Услуга";
      this.cmbReceipt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbReceipt.DisplayMember = "ReceiptName";
      this.cmbReceipt.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbReceipt.FormattingEnabled = true;
      this.cmbReceipt.Location = new Point(120, 65);
      this.cmbReceipt.Name = "cmbReceipt";
      this.cmbReceipt.Size = new Size(950, 24);
      this.cmbReceipt.TabIndex = 7;
      this.cmbReceipt.ValueMember = "ReceiptId";
      this.cmbReceipt.SelectionChangeCommitted += new EventHandler(this.cmbReceipt_SelectionChangeCommitted);
      this.lblSost.AutoSize = true;
      this.lblSost.Location = new Point(7, 159);
      this.lblSost.Name = "lblSost";
      this.lblSost.Size = new Size(107, 17);
      this.lblSost.TabIndex = 4;
      this.lblSost.Text = "Составляющая";
      this.cmbCompany.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbCompany.DisplayMember = "CompanyName";
      this.cmbCompany.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCompany.FormattingEnabled = true;
      this.cmbCompany.Location = new Point(120, 35);
      this.cmbCompany.Name = "cmbCompany";
      this.cmbCompany.Size = new Size(950, 24);
      this.cmbCompany.TabIndex = 6;
      this.cmbCompany.ValueMember = "CompanyId";
      this.cmbCompany.SelectionChangeCommitted += new EventHandler(this.cmbCompany_SelectionChangeCommitted);
      this.lblSupplier.AutoSize = true;
      this.lblSupplier.Location = new Point(7, 190);
      this.lblSupplier.Name = "lblSupplier";
      this.lblSupplier.Size = new Size(87, 17);
      this.lblSupplier.TabIndex = 5;
      this.lblSupplier.Text = "Получатель";
      this.pnlCmpReceipt.Controls.Add((Control) this.ucCmpReceipt1);
      this.pnlCmpReceipt.Controls.Add((Control) this.groupBox1);
      this.pnlCmpReceipt.Dock = DockStyle.Fill;
      this.pnlCmpReceipt.Location = new Point(189, 0);
      this.pnlCmpReceipt.Margin = new Padding(4);
      this.pnlCmpReceipt.Name = "pnlCmpReceipt";
      this.pnlCmpReceipt.Size = new Size(1075, 597);
      this.pnlCmpReceipt.TabIndex = 2;
      this.ucCmpReceipt1.Company_id = (short) 0;
      this.ucCmpReceipt1.Dock = DockStyle.Fill;
      this.ucCmpReceipt1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.ucCmpReceipt1.Location = new Point(0, 54);
      this.ucCmpReceipt1.Margin = new Padding(5);
      this.ucCmpReceipt1.Name = "ucCmpReceipt1";
      this.ucCmpReceipt1.Size = new Size(1075, 543);
      this.ucCmpReceipt1.TabIndex = 1;
      this.groupBox1.Controls.Add((Control) this.cbCompany);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Dock = DockStyle.Top;
      this.groupBox1.Location = new Point(0, 0);
      this.groupBox1.Margin = new Padding(4);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new Padding(4);
      this.groupBox1.Size = new Size(1075, 54);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.cbCompany.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cbCompany.FormattingEnabled = true;
      this.cbCompany.Location = new Point(99, 21);
      this.cbCompany.Margin = new Padding(4);
      this.cbCompany.Name = "cbCompany";
      this.cbCompany.Size = new Size(967, 24);
      this.cbCompany.TabIndex = 1;
      this.cbCompany.SelectedIndexChanged += new EventHandler(this.cbCompany_SelectedIndexChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(13, 25);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(74, 17);
      this.label1.TabIndex = 0;
      this.label1.Text = "Компания";
      this.hp.HelpNamespace = "Help.chm";
      this.pnReceiptParam.Controls.Add((Control) this.dgvReceiptParam);
      this.pnReceiptParam.Controls.Add((Control) this.toolStrip3);
      this.pnReceiptParam.Controls.Add((Control) this.groupBox2);
      this.pnReceiptParam.Dock = DockStyle.Fill;
      this.pnReceiptParam.Location = new Point(189, 0);
      this.pnReceiptParam.Name = "pnReceiptParam";
      this.pnReceiptParam.Size = new Size(1075, 597);
      this.pnReceiptParam.TabIndex = 2;
      this.dgvReceiptParam.AllowUserToAddRows = false;
      this.dgvReceiptParam.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      this.dgvReceiptParam.BackgroundColor = Color.AliceBlue;
      this.dgvReceiptParam.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvReceiptParam.Columns.AddRange((DataGridViewColumn) this.Receipt, (DataGridViewColumn) this.Param, (DataGridViewColumn) this.PValue, (DataGridViewColumn) this.DBeg, (DataGridViewColumn) this.Period, (DataGridViewColumn) this.DEnd, (DataGridViewColumn) this.UName, (DataGridViewColumn) this.DEdit);
      this.dgvReceiptParam.Dock = DockStyle.Fill;
      this.dgvReceiptParam.Location = new Point(0, 118);
      this.dgvReceiptParam.Name = "dgvReceiptParam";
      this.dgvReceiptParam.Size = new Size(1075, 479);
      this.dgvReceiptParam.TabIndex = 8;
      this.dgvReceiptParam.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvReceiptParam_CellBeginEdit);
      this.dgvReceiptParam.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvReceiptParam_CellFormatting);
      this.dgvReceiptParam.CellMouseDown += new DataGridViewCellMouseEventHandler(this.dgvReceiptParam_CellMouseDown);
      this.dgvReceiptParam.CurrentCellDirtyStateChanged += new EventHandler(this.dgvReceiptParam_CurrentCellDirtyStateChanged);
      this.dgvReceiptParam.SelectionChanged += new EventHandler(this.dgvReceiptParam_SelectionChanged);
      this.Receipt.HeaderText = "Квитанция";
      this.Receipt.Name = "Receipt";
      this.Param.HeaderText = "Параметр";
      this.Param.Name = "Param";
      this.PValue.HeaderText = "Значение";
      this.PValue.Name = "PValue";
      this.DBeg.DataPropertyName = "DBeg";
      this.DBeg.HeaderText = "Дата начала действия";
      this.DBeg.Name = "DBeg";
      this.Period.HeaderText = "Период";
      this.Period.Name = "Period";
      this.Period.Visible = false;
      this.DEnd.DataPropertyName = "DEnd";
      this.DEnd.HeaderText = "Дата окончания действия";
      this.DEnd.Name = "DEnd";
      this.UName.DataPropertyName = "UName";
      this.UName.HeaderText = "Пользователь";
      this.UName.Name = "UName";
      this.UName.ReadOnly = true;
      this.DEdit.DataPropertyName = "DEdit";
      this.DEdit.HeaderText = "Дата редактирования";
      this.DEdit.Name = "DEdit";
      this.DEdit.ReadOnly = true;
      this.toolStrip3.Font = new Font("Tahoma", 10f);
      this.toolStrip3.Items.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.tsbAddReceiptParam,
        (ToolStripItem) this.tsbApplayReceiptParam,
        (ToolStripItem) this.tsbCancelReceiptParam,
        (ToolStripItem) this.tsbDeleteReceiptParam,
        (ToolStripItem) this.toolStripButton4
      });
      this.toolStrip3.LayoutStyle = ToolStripLayoutStyle.Flow;
      this.toolStrip3.Location = new Point(0, 94);
      this.toolStrip3.Name = "toolStrip3";
      this.toolStrip3.Size = new Size(1075, 24);
      this.toolStrip3.TabIndex = 2;
      this.toolStrip3.Text = "toolStrip3";
      this.tsbAddReceiptParam.Image = (Image) Resources.add_var;
      this.tsbAddReceiptParam.ImageTransparentColor = Color.Magenta;
      this.tsbAddReceiptParam.Name = "tsbAddReceiptParam";
      this.tsbAddReceiptParam.Size = new Size(91, 21);
      this.tsbAddReceiptParam.Text = "Добавить";
      this.tsbAddReceiptParam.Click += new EventHandler(this.tsbAddReceiptParam_Click);
      this.tsbApplayReceiptParam.Enabled = false;
      this.tsbApplayReceiptParam.Image = (Image) Resources.Applay_var;
      this.tsbApplayReceiptParam.ImageTransparentColor = Color.Magenta;
      this.tsbApplayReceiptParam.Name = "tsbApplayReceiptParam";
      this.tsbApplayReceiptParam.Size = new Size(99, 21);
      this.tsbApplayReceiptParam.Text = "Сохранить";
      this.tsbApplayReceiptParam.Click += new EventHandler(this.tsbApplayReceiptParam_Click);
      this.tsbCancelReceiptParam.Enabled = false;
      this.tsbCancelReceiptParam.Image = (Image) Resources.undo;
      this.tsbCancelReceiptParam.ImageTransparentColor = Color.Magenta;
      this.tsbCancelReceiptParam.Name = "tsbCancelReceiptParam";
      this.tsbCancelReceiptParam.Size = new Size(77, 21);
      this.tsbCancelReceiptParam.Text = "Отмена";
      this.tsbCancelReceiptParam.Click += new EventHandler(this.tsbCancelReceiptParam_Click);
      this.tsbDeleteReceiptParam.Image = (Image) Resources.delete_var;
      this.tsbDeleteReceiptParam.ImageTransparentColor = Color.Magenta;
      this.tsbDeleteReceiptParam.Name = "tsbDeleteReceiptParam";
      this.tsbDeleteReceiptParam.Size = new Size(82, 21);
      this.tsbDeleteReceiptParam.Text = "Удалить";
      this.tsbDeleteReceiptParam.Click += new EventHandler(this.tsbDeleteReceiptParam_Click);
      this.toolStripButton4.Image = (Image) Resources.Exit;
      this.toolStripButton4.ImageTransparentColor = Color.Magenta;
      this.toolStripButton4.Name = "toolStripButton4";
      this.toolStripButton4.Size = new Size(70, 21);
      this.toolStripButton4.Text = "Выход";
      this.toolStripButton4.Visible = false;
      this.groupBox2.Controls.Add((Control) this.lblPastTime);
      this.groupBox2.Controls.Add((Control) this.btnPastTime);
      this.groupBox2.Controls.Add((Control) this.chbArhiv);
      this.groupBox2.Controls.Add((Control) this.cmbCompanyParam);
      this.groupBox2.Controls.Add((Control) this.label2);
      this.groupBox2.Dock = DockStyle.Top;
      this.groupBox2.Location = new Point(0, 0);
      this.groupBox2.Margin = new Padding(4);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Padding = new Padding(4);
      this.groupBox2.Size = new Size(1075, 94);
      this.groupBox2.TabIndex = 9;
      this.groupBox2.TabStop = false;
      this.lblPastTime.AutoSize = true;
      this.lblPastTime.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblPastTime.ForeColor = Color.DarkOrange;
      this.lblPastTime.Location = new Point(268, 65);
      this.lblPastTime.Name = "lblPastTime";
      this.lblPastTime.Size = new Size(199, 16);
      this.lblPastTime.TabIndex = 21;
      this.lblPastTime.Text = "Режим прошлого времени";
      this.lblPastTime.Visible = false;
      this.btnPastTime.Image = (Image) Resources.time_24;
      this.btnPastTime.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnPastTime.Location = new Point(99, 52);
      this.btnPastTime.Name = "btnPastTime";
      this.btnPastTime.Size = new Size(152, 30);
      this.btnPastTime.TabIndex = 20;
      this.btnPastTime.Text = "Прошлое время";
      this.btnPastTime.TextAlign = ContentAlignment.MiddleRight;
      this.btnPastTime.UseVisualStyleBackColor = true;
      this.btnPastTime.Visible = false;
      this.btnPastTime.Click += new EventHandler(this.btnPastTime_Click);
      this.chbArhiv.AutoSize = true;
      this.chbArhiv.Location = new Point(16, 61);
      this.chbArhiv.Name = "chbArhiv";
      this.chbArhiv.Size = new Size(65, 21);
      this.chbArhiv.TabIndex = 19;
      this.chbArhiv.Text = "Архив";
      this.chbArhiv.UseVisualStyleBackColor = true;
      this.chbArhiv.CheckedChanged += new EventHandler(this.chbArhiv_CheckedChanged);
      this.cmbCompanyParam.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbCompanyParam.FormattingEnabled = true;
      this.cmbCompanyParam.Location = new Point(99, 21);
      this.cmbCompanyParam.Margin = new Padding(4);
      this.cmbCompanyParam.Name = "cmbCompanyParam";
      this.cmbCompanyParam.Size = new Size(967, 24);
      this.cmbCompanyParam.TabIndex = 1;
      this.cmbCompanyParam.SelectedIndexChanged += new EventHandler(this.cmbCompanyParam_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(13, 25);
      this.label2.Margin = new Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(74, 17);
      this.label2.TabIndex = 0;
      this.label2.Text = "Компания";
      this.maskDateColumn1.DataPropertyName = "DBeg";
      this.maskDateColumn1.HeaderText = "Дата начала действия";
      this.maskDateColumn1.Name = "maskDateColumn1";
      this.maskDateColumn1.Width = 207;
      this.maskDateColumn2.DataPropertyName = "DEnd";
      this.maskDateColumn2.HeaderText = "Дата окончания действия";
      this.maskDateColumn2.Name = "maskDateColumn2";
      this.maskDateColumn2.Width = 206;
      this.dataGridViewTextBoxColumn1.DataPropertyName = "UName";
      this.dataGridViewTextBoxColumn1.HeaderText = "Пользователь";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.ReadOnly = true;
      this.dataGridViewTextBoxColumn1.Width = 207;
      this.dataGridViewTextBoxColumn2.DataPropertyName = "DEdit";
      this.dataGridViewTextBoxColumn2.HeaderText = "Дата";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn2.ReadOnly = true;
      this.dataGridViewTextBoxColumn2.Width = 206;
      this.tmr.Interval = 1000;
      this.tmr.Tick += new EventHandler(this.tmr_Tick);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1264, 597);
      this.Controls.Add((Control) this.pnlReceipt);
      this.Controls.Add((Control) this.pnReceiptParam);
      this.Controls.Add((Control) this.pnlHmReceipt);
      this.Controls.Add((Control) this.pnlCmpReceipt);
      this.Controls.Add((Control) this.toolStrip1);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.hp.SetHelpKeyword((Control) this, "kv511.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmReceipt";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Квитанция";
      this.Load += new EventHandler(this.FrmReceipt_Load);
      this.dtYesNo.EndInit();
      this.dsMain.EndInit();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.pnlReceipt.ResumeLayout(false);
      this.pnlHmReceipt.ResumeLayout(false);
      this.pnlHmReceipt.PerformLayout();
      ((ISupportInitialize) this.dgvHmReceipt).EndInit();
      this.toolStrip2.ResumeLayout(false);
      this.toolStrip2.PerformLayout();
      this.pnUp.ResumeLayout(false);
      this.pnUp.PerformLayout();
      this.pnlCmpReceipt.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.pnReceiptParam.ResumeLayout(false);
      this.pnReceiptParam.PerformLayout();
      ((ISupportInitialize) this.dgvReceiptParam).EndInit();
      this.toolStrip3.ResumeLayout(false);
      this.toolStrip3.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
