// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmParam
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Forms.Controls;
using Kvartplata.Properties;
using NHibernate;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmParam : Form
  {
    private bool isPast = false;
    private short company_id = 0;
    private IList companyList = (IList) new ArrayList();
    private FormStateSaver fss = new FormStateSaver(FrmParam.ic);
    private bool _readOnlyService = false;
    private bool _readOnlyCmp = false;
    private IContainer components = (IContainer) null;
    private ISession session;
    private DateTime? closedPeriod;
    private static IContainer ic;
    private Company _company;
    private ToolStrip toolStrip1;
    private ToolStripButton tsbParam;
    private ToolStripButton tsbCmpParam;
    private ToolStripButton tsbExit;
    private Panel pnlParam;
    private Panel pnlCmpParam;
    private GroupBox groupBox1;
    private Button btnPast;
    private ComboBox cbCompany;
    private Label label1;
    private DataSet dsMain;
    private DataTable dtType;
    private DataColumn dataColumn1;
    private DataColumn dataColumn2;
    private Label label3;
    private Panel panel1;
    private Button btnDown;
    private Button btnUp;
    private CalendarColumn calendarColumn1;
    private CalendarColumn calendarColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private CalendarColumn calendarColumn3;
    private CalendarColumn calendarColumn4;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private UCCmpParam ucCmpParam1;
    private CheckBox cbArchive;
    private DataTable dtAreal;
    private DataColumn dataColumn3;
    private DataColumn dataColumn4;
    public HelpProvider hp;
    private Timer tmr;
    private UCParam ucParam1;

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
        this.ucParam1.closedPeriod = this.closedPeriod;
        this.ucParam1.LoadData();
        this.ucCmpParam1.closedPeriod = this.closedPeriod;
        if (this.cbCompany.SelectedItem == null)
          return;
        this.ucCmpParam1.Company_id = this.company_id;
      }
    }

    public FrmParam(Company company)
    {
      this.InitializeComponent();
      this._company = company;
      this.CheckAccess();
      this.fss.ParentForm = (Form) this;
      this.session = Domain.CurrentSession;
      this.ucParam1.session = Domain.CurrentSession;
      this.ucCmpParam1.session = Domain.CurrentSession;
      this.ucParam1.tsbExit.Visible = false;
      this.ucCmpParam1.tsbExit.Visible = false;
      this.companyList = this.session.CreateQuery("from Company order by CompanyId").List();
      this.cbCompany.DataSource = (object) this.companyList;
      this.cbCompany.DisplayMember = "CompanyNameAndNum";
      this.cbCompany.ValueMember = "CompanyId";
      this.cbCompany.SelectedValue = (object) Options.Company.CompanyId;
      KvrplHelper.AddRow(this.dtType, 1, "компания");
      KvrplHelper.AddRow(this.dtType, 2, "дом");
      KvrplHelper.AddRow(this.dtType, 3, "лицевой");
      KvrplHelper.AddRow(this.dtType, 4, "услуги лицевого");
      KvrplHelper.AddRow(this.dtType, 5, "аренда");
      KvrplHelper.AddRow(this.dtType, 6, "аренда услуги лицевого");
      KvrplHelper.AddRow(this.dtType, 7, "параметры по квитанции");
      KvrplHelper.AddRow(this.dtAreal, 0, "для всех");
      KvrplHelper.AddRow(this.dtAreal, 1, "ограничено");
    }

    private void CheckAccess()
    {
      this._readOnlyService = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(32, this._company, false));
      this._readOnlyCmp = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(33, this._company, false));
      this.ucParam1.toolStrip1.Visible = this._readOnlyService;
      this.ucParam1.dgvBase.ReadOnly = !this._readOnlyService;
      this.ucCmpParam1.toolStrip1.Visible = this._readOnlyCmp;
      this.ucCmpParam1.dgvBase.ReadOnly = !this._readOnlyCmp;
    }

    private Period GetClosedPeriod()
    {
      return this.session.Get<Period>((object) Convert.ToInt32(this.session.CreateQuery(string.Format("select max(p.Period_id) from ClosedPeriod p where (p.Complex_id = {0} or p.Complex_id = {2}) and p.Company_id = {1}", (object) Options.ComplexPasp.ComplexId, (object) this.Company_id, (object) Options.ComplexPrior.IdFk)).UniqueResult()));
    }

    private void FrmParam_Load(object sender, EventArgs e)
    {
      if (this.session != null && this.session.IsOpen)
        this.session.Clear();
      ((DataGridViewComboBoxColumn) this.ucParam1.dgvBase.Columns["Param_type"]).DataSource = (object) this.dtType;
      ((DataGridViewComboBoxColumn) this.ucParam1.dgvBase.Columns["Areal"]).DataSource = (object) this.dtAreal;
      Period closedPeriod = this.GetClosedPeriod();
      DateTime? periodName = closedPeriod.PeriodName;
      if (periodName.HasValue)
      {
        periodName = closedPeriod.PeriodName;
        this.closedPeriod = new DateTime?(periodName.Value);
      }
      else
        this.closedPeriod = new DateTime?();
      this.ucParam1.closedPeriod = this.closedPeriod;
      this.ucParam1.LoadData();
      this.ucCmpParam1.closedPeriod = this.closedPeriod;
    }

    private void tsbParam_Click(object sender, EventArgs e)
    {
      if (this.ucCmpParam1.tsbApplay.Enabled)
      {
        if (MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
          this.ucCmpParam1.SaveData();
          this.tsbParam.Checked = false;
          return;
        }
        this.ucCmpParam1.LoadData();
        this.ucCmpParam1.CancelEnabled();
      }
      else
      {
        this.ucParam1.LoadData();
        this.ucCmpParam1.LoadData();
      }
      this.tsbCmpParam.Checked = false;
      this.pnlParam.BringToFront();
    }

    private void tsbCmpParam_Click(object sender, EventArgs e)
    {
      if (this.ucParam1.tsbApplay.Enabled)
      {
        if (MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
          this.ucParam1.SaveData();
          this.tsbCmpParam.Checked = false;
          return;
        }
        this.ucParam1.LoadData();
        this.ucParam1.CancelEnabled();
      }
      else
      {
        this.ucParam1.LoadData();
        this.ucCmpParam1.LoadData();
      }
      this.tsbParam.Checked = false;
      this.pnlCmpParam.BringToFront();
    }

    private void tsbExit_Click(object sender, EventArgs e)
    {
      if ((this.ucCmpParam1.tsbApplay.Enabled || this.ucParam1.tsbApplay.Enabled) && MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
      {
        this.tsbExit.Checked = false;
        if (this.ucCmpParam1.tsbApplay.Enabled)
        {
          this.ucCmpParam1.SaveData();
        }
        else
        {
          if (!this.ucParam1.tsbApplay.Enabled)
            return;
          this.ucParam1.SaveData();
        }
      }
      else
        this.Close();
    }

    private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cbCompany.SelectedItem == null)
        return;
      if (this.tsbCmpParam.Checked && !KvrplHelper.CheckProxy(32, 1, (Company) this.cbCompany.SelectedItem, true))
        this.cbCompany.SelectedItem = (object) this.session.Get<Company>((object) this.Company_id);
      else
        this.Company_id = ((Company) this.cbCompany.SelectedItem).CompanyId;
      if ((int) this.ucCmpParam1.Company_id != (int) this.Company_id)
        this.cbCompany.SelectedItem = (object) this.session.Get<Company>((object) this.ucCmpParam1.Company_id);
    }

    private void btnPast_Click(object sender, EventArgs e)
    {
      this.isPast = !this.isPast;
      this.ucCmpParam1.IsPast = this.isPast;
      if (this.ucCmpParam1.IsPast == this.isPast)
      {
        if (this.isPast)
        {
          this.tmr.Start();
          this.groupBox1.Height += 18;
          this.btnPast.BackColor = Color.DarkOrange;
        }
        else
        {
          this.tmr.Stop();
          this.groupBox1.Height -= 18;
          this.btnPast.Text = "Прошлое время";
          this.btnPast.BackColor = this.BackColor;
        }
      }
      else
        this.isPast = !this.isPast;
    }

    private void ucParam1_ObjectsListChanged(object sender, EventArgs e)
    {
      string str = "";
      if (Options.Kvartplata && !Options.Arenda)
        str = " and Param_id not in (216,217,218,219)";
      if (!Options.Kvartplata && Options.Arenda)
        str = " and Param_id not in (206,207,208,214)";
      IList list = this.session.CreateQuery("from Param where param_type=1 " + str + " order by ParamName").List();
      list.Insert(0, (object) new Param());
      ((DataGridViewComboBoxColumn) this.ucCmpParam1.dgvBase.Columns["ParamColumn"]).DataSource = (object) list;
    }

    private void ucParam1_CurObjectChanged(object sender, EventArgs e)
    {
    }

    private void ucCmpParam1_ObjectsListChanged(object sender, EventArgs e)
    {
      if (!this.ucCmpParam1.IsCopy)
        return;
      this.isPast = !this.isPast;
      this.groupBox1.Height += 18;
      this.ucCmpParam1.IsCopy = false;
    }

    private void btnUp_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true) || (this.ucParam1.dgvBase.CurrentRow.Index <= 0 || this.ucParam1.tsbApplay.Enabled))
        return;
      this.Cursor = Cursors.WaitCursor;
      using (ITransaction transaction = this.session.BeginTransaction())
      {
        try
        {
          short sorter = ((Param) this.ucParam1.ObjectsList[this.ucParam1.dgvBase.CurrentRow.Index - 1]).Sorter;
          this.ucParam1.CurParam.Sorter = (short) -10;
          this.session.Update((object) this.ucParam1.CurParam);
          this.session.Flush();
          ++((Param) this.ucParam1.ObjectsList[this.ucParam1.dgvBase.CurrentRow.Index - 1]).Sorter;
          this.session.Update((object) (Param) this.ucParam1.ObjectsList[this.ucParam1.dgvBase.CurrentRow.Index - 1]);
          this.session.Flush();
          this.ucParam1.CurParam.Sorter = sorter;
          this.session.Update((object) this.ucParam1.CurParam);
          this.session.Flush();
          this.ucParam1.LoadData();
          this.ucParam1.dgvBase.Refresh();
          transaction.Commit();
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
          transaction.Rollback();
        }
      }
      this.Cursor = Cursors.Default;
    }

    private void btnDown_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true) || (this.ucParam1.dgvBase.CurrentRow.Index + 1 >= this.ucParam1.ObjectsList.Count || this.ucParam1.tsbApplay.Enabled))
        return;
      this.Cursor = Cursors.WaitCursor;
      using (ITransaction transaction = this.session.BeginTransaction())
      {
        try
        {
          short sorter = ((Param) this.ucParam1.ObjectsList[this.ucParam1.dgvBase.CurrentRow.Index + 1]).Sorter;
          this.ucParam1.CurParam.Sorter = (short) -10;
          this.session.Update((object) this.ucParam1.CurParam);
          this.session.Flush();
          --((Param) this.ucParam1.ObjectsList[this.ucParam1.dgvBase.CurrentRow.Index + 1]).Sorter;
          this.session.Update((object) (Param) this.ucParam1.ObjectsList[this.ucParam1.dgvBase.CurrentRow.Index + 1]);
          this.session.Flush();
          this.ucParam1.CurParam.Sorter = sorter;
          this.session.Update((object) this.ucParam1.CurParam);
          this.session.Flush();
          this.ucParam1.LoadData();
          this.ucParam1.dgvBase.Refresh();
          transaction.Commit();
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
          transaction.Rollback();
        }
        this.Cursor = Cursors.Default;
      }
    }

    private void cbArchive_CheckedChanged(object sender, EventArgs e)
    {
      this.ucCmpParam1.IsArchive = this.cbArchive.Checked;
      this.ucCmpParam1.LoadData();
    }

    private void tmr_Tick(object sender, EventArgs e)
    {
      if (this.isPast)
      {
        if (this.label3.ForeColor == Color.DarkOrange)
          this.label3.ForeColor = this.BackColor;
        else
          this.label3.ForeColor = Color.DarkOrange;
      }
      else
        this.label3.ForeColor = this.BackColor;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmParam));
      this.toolStrip1 = new ToolStrip();
      this.tsbParam = new ToolStripButton();
      this.tsbCmpParam = new ToolStripButton();
      this.tsbExit = new ToolStripButton();
      this.pnlParam = new Panel();
      this.ucParam1 = new UCParam();
      this.panel1 = new Panel();
      this.btnDown = new Button();
      this.btnUp = new Button();
      this.dsMain = new DataSet();
      this.dtType = new DataTable();
      this.dataColumn1 = new DataColumn();
      this.dataColumn2 = new DataColumn();
      this.dtAreal = new DataTable();
      this.dataColumn3 = new DataColumn();
      this.dataColumn4 = new DataColumn();
      this.pnlCmpParam = new Panel();
      this.ucCmpParam1 = new UCCmpParam();
      this.groupBox1 = new GroupBox();
      this.cbArchive = new CheckBox();
      this.label3 = new Label();
      this.btnPast = new Button();
      this.cbCompany = new ComboBox();
      this.label1 = new Label();
      this.calendarColumn1 = new CalendarColumn();
      this.calendarColumn2 = new CalendarColumn();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.calendarColumn3 = new CalendarColumn();
      this.calendarColumn4 = new CalendarColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.hp = new HelpProvider();
      this.tmr = new Timer(this.components);
      this.toolStrip1.SuspendLayout();
      this.pnlParam.SuspendLayout();
      this.panel1.SuspendLayout();
      this.dsMain.BeginInit();
      this.dtType.BeginInit();
      this.dtAreal.BeginInit();
      this.pnlCmpParam.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.toolStrip1.Dock = DockStyle.Left;
      this.toolStrip1.Font = new Font("Tahoma", 10f);
      this.toolStrip1.ImageScalingSize = new Size(48, 48);
      this.toolStrip1.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.tsbParam,
        (ToolStripItem) this.tsbCmpParam,
        (ToolStripItem) this.tsbExit
      });
      this.toolStrip1.Location = new Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Padding = new Padding(0, 0, 2, 0);
      this.toolStrip1.Size = new Size(173, 375);
      this.toolStrip1.TabIndex = 0;
      this.toolStrip1.Text = "toolStrip1";
      this.tsbParam.Checked = true;
      this.tsbParam.CheckOnClick = true;
      this.tsbParam.CheckState = CheckState.Checked;
      this.tsbParam.Image = (Image) Resources.allClasses;
      this.tsbParam.ImageTransparentColor = Color.Magenta;
      this.tsbParam.Name = "tsbParam";
      this.tsbParam.Size = new Size(168, 69);
      this.tsbParam.Text = "Параметры";
      this.tsbParam.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tsbParam.Click += new EventHandler(this.tsbParam_Click);
      this.tsbCmpParam.CheckOnClick = true;
      this.tsbCmpParam.Font = new Font("Tahoma", 10f);
      this.tsbCmpParam.Image = (Image) Resources.percent;
      this.tsbCmpParam.ImageTransparentColor = Color.Magenta;
      this.tsbCmpParam.Name = "tsbCmpParam";
      this.tsbCmpParam.Size = new Size(168, 69);
      this.tsbCmpParam.Text = "Параметры на компании";
      this.tsbCmpParam.TextDirection = ToolStripTextDirection.Horizontal;
      this.tsbCmpParam.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tsbCmpParam.Click += new EventHandler(this.tsbCmpParam_Click);
      this.tsbExit.CheckOnClick = true;
      this.tsbExit.Image = (Image) Resources.Exit;
      this.tsbExit.ImageTransparentColor = Color.Magenta;
      this.tsbExit.Name = "tsbExit";
      this.tsbExit.Size = new Size(168, 69);
      this.tsbExit.Text = "Выход";
      this.tsbExit.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tsbExit.Click += new EventHandler(this.tsbExit_Click);
      this.pnlParam.Controls.Add((Control) this.ucParam1);
      this.pnlParam.Controls.Add((Control) this.panel1);
      this.pnlParam.Dock = DockStyle.Fill;
      this.pnlParam.Location = new Point(173, 0);
      this.pnlParam.Margin = new Padding(4);
      this.pnlParam.Name = "pnlParam";
      this.pnlParam.Size = new Size(865, 375);
      this.pnlParam.TabIndex = 1;
      this.ucParam1.Dock = DockStyle.Fill;
      this.ucParam1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.ucParam1.Location = new Point(0, 0);
      this.ucParam1.Margin = new Padding(5);
      this.ucParam1.Name = "ucParam1";
      this.ucParam1.Size = new Size(813, 375);
      this.ucParam1.TabIndex = 5;
      this.ucParam1.ObjectsListChanged += new EventHandler(this.ucParam1_ObjectsListChanged);
      this.ucParam1.CurObjectChanged += new EventHandler(this.ucParam1_CurObjectChanged);
      this.panel1.Controls.Add((Control) this.btnDown);
      this.panel1.Controls.Add((Control) this.btnUp);
      this.panel1.Dock = DockStyle.Right;
      this.panel1.Location = new Point(813, 0);
      this.panel1.Margin = new Padding(4);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(52, 375);
      this.panel1.TabIndex = 2;
      this.btnDown.Image = (Image) Resources.arrow_down;
      this.btnDown.Location = new Point(6, 236);
      this.btnDown.Margin = new Padding(4);
      this.btnDown.Name = "btnDown";
      this.btnDown.Size = new Size(40, 39);
      this.btnDown.TabIndex = 1;
      this.btnDown.UseVisualStyleBackColor = true;
      this.btnDown.Click += new EventHandler(this.btnDown_Click);
      this.btnUp.Image = (Image) Resources.arrow_up;
      this.btnUp.Location = new Point(6, 169);
      this.btnUp.Margin = new Padding(4);
      this.btnUp.Name = "btnUp";
      this.btnUp.Size = new Size(40, 39);
      this.btnUp.TabIndex = 0;
      this.btnUp.UseVisualStyleBackColor = true;
      this.btnUp.Click += new EventHandler(this.btnUp_Click);
      this.dsMain.DataSetName = "NewDataSet";
      this.dsMain.Tables.AddRange(new DataTable[2]
      {
        this.dtType,
        this.dtAreal
      });
      this.dtType.Columns.AddRange(new DataColumn[2]
      {
        this.dataColumn1,
        this.dataColumn2
      });
      this.dtType.TableName = "Table1";
      this.dataColumn1.ColumnName = "Id";
      this.dataColumn1.DataType = typeof (short);
      this.dataColumn2.ColumnName = "Name";
      this.dtAreal.Columns.AddRange(new DataColumn[2]
      {
        this.dataColumn3,
        this.dataColumn4
      });
      this.dtAreal.TableName = "Table2";
      this.dataColumn3.ColumnName = "Id";
      this.dataColumn3.DataType = typeof (short);
      this.dataColumn4.ColumnName = "Name";
      this.pnlCmpParam.Controls.Add((Control) this.ucCmpParam1);
      this.pnlCmpParam.Controls.Add((Control) this.groupBox1);
      this.pnlCmpParam.Dock = DockStyle.Fill;
      this.pnlCmpParam.Location = new Point(173, 0);
      this.pnlCmpParam.Margin = new Padding(4);
      this.pnlCmpParam.Name = "pnlCmpParam";
      this.pnlCmpParam.Size = new Size(865, 375);
      this.pnlCmpParam.TabIndex = 2;
      this.ucCmpParam1.Company_id = (short) 0;
      this.ucCmpParam1.Dock = DockStyle.Fill;
      this.ucCmpParam1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.ucCmpParam1.IsPast = false;
      this.ucCmpParam1.Location = new Point(0, 49);
      this.ucCmpParam1.Margin = new Padding(5);
      this.ucCmpParam1.Name = "ucCmpParam1";
      this.ucCmpParam1.Size = new Size(865, 326);
      this.ucCmpParam1.TabIndex = 4;
      this.ucCmpParam1.ObjectsListChanged += new EventHandler(this.ucCmpParam1_ObjectsListChanged);
      this.groupBox1.Controls.Add((Control) this.cbArchive);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.btnPast);
      this.groupBox1.Controls.Add((Control) this.cbCompany);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Dock = DockStyle.Top;
      this.groupBox1.Location = new Point(0, 0);
      this.groupBox1.Margin = new Padding(4);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new Padding(4);
      this.groupBox1.Size = new Size(865, 49);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.cbArchive.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cbArchive.AutoSize = true;
      this.cbArchive.Location = new Point(632, 22);
      this.cbArchive.Name = "cbArchive";
      this.cbArchive.Size = new Size(65, 21);
      this.cbArchive.TabIndex = 6;
      this.cbArchive.Text = "Архив";
      this.cbArchive.UseVisualStyleBackColor = true;
      this.cbArchive.CheckedChanged += new EventHandler(this.cbArchive_CheckedChanged);
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.label3.ForeColor = Color.DarkOrange;
      this.label3.Location = new Point(239, 48);
      this.label3.Margin = new Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(199, 16);
      this.label3.TabIndex = 5;
      this.label3.Text = "Режим прошлого времени";
      this.btnPast.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPast.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.btnPast.Image = (Image) Resources.time_24;
      this.btnPast.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnPast.Location = new Point(704, 18);
      this.btnPast.Margin = new Padding(4);
      this.btnPast.Name = "btnPast";
      this.btnPast.Size = new Size(158, 30);
      this.btnPast.TabIndex = 2;
      this.btnPast.Text = "Прошлое время";
      this.btnPast.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.btnPast.UseVisualStyleBackColor = true;
      this.btnPast.Click += new EventHandler(this.btnPast_Click);
      this.cbCompany.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cbCompany.FormattingEnabled = true;
      this.cbCompany.Location = new Point(87, 20);
      this.cbCompany.Margin = new Padding(4);
      this.cbCompany.Name = "cbCompany";
      this.cbCompany.Size = new Size(538, 24);
      this.cbCompany.TabIndex = 1;
      this.cbCompany.SelectedIndexChanged += new EventHandler(this.cbCompany_SelectedIndexChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 23);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(74, 17);
      this.label1.TabIndex = 0;
      this.label1.Text = "Компания";
      gridViewCellStyle1.Format = "d";
      this.calendarColumn1.DefaultCellStyle = gridViewCellStyle1;
      this.calendarColumn1.HeaderText = "Дата начала действия";
      this.calendarColumn1.Name = "calendarColumn1";
      this.calendarColumn1.Resizable = DataGridViewTriState.True;
      this.calendarColumn1.SortMode = DataGridViewColumnSortMode.Automatic;
      this.calendarColumn1.Width = 120;
      gridViewCellStyle2.Format = "d";
      this.calendarColumn2.DefaultCellStyle = gridViewCellStyle2;
      this.calendarColumn2.HeaderText = "Дата окончания действия";
      this.calendarColumn2.Name = "calendarColumn2";
      this.calendarColumn2.Resizable = DataGridViewTriState.True;
      this.calendarColumn2.SortMode = DataGridViewColumnSortMode.Automatic;
      this.calendarColumn2.Width = 120;
      gridViewCellStyle3.Format = "d";
      gridViewCellStyle3.NullValue = (object) null;
      this.dataGridViewTextBoxColumn1.DefaultCellStyle = gridViewCellStyle3;
      this.dataGridViewTextBoxColumn1.HeaderText = "Период";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.ReadOnly = true;
      this.dataGridViewTextBoxColumn1.Visible = false;
      gridViewCellStyle4.Format = "d";
      this.calendarColumn3.DefaultCellStyle = gridViewCellStyle4;
      this.calendarColumn3.HeaderText = "Дата начала действия";
      this.calendarColumn3.Name = "calendarColumn3";
      this.calendarColumn3.Resizable = DataGridViewTriState.True;
      this.calendarColumn3.SortMode = DataGridViewColumnSortMode.Automatic;
      this.calendarColumn3.Width = 120;
      gridViewCellStyle5.Format = "d";
      this.calendarColumn4.DefaultCellStyle = gridViewCellStyle5;
      this.calendarColumn4.HeaderText = "Дата окончания действия";
      this.calendarColumn4.Name = "calendarColumn4";
      this.calendarColumn4.Resizable = DataGridViewTriState.True;
      this.calendarColumn4.SortMode = DataGridViewColumnSortMode.Automatic;
      this.calendarColumn4.Width = 120;
      gridViewCellStyle6.Format = "N4";
      gridViewCellStyle6.NullValue = (object) null;
      this.dataGridViewTextBoxColumn2.DefaultCellStyle = gridViewCellStyle6;
      this.dataGridViewTextBoxColumn2.HeaderText = "Значение";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.hp.HelpNamespace = "Help.chm";
      this.tmr.Interval = 1000;
      this.tmr.Tick += new EventHandler(this.tmr_Tick);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1038, 375);
      this.Controls.Add((Control) this.pnlParam);
      this.Controls.Add((Control) this.pnlCmpParam);
      this.Controls.Add((Control) this.toolStrip1);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.hp.SetHelpKeyword((Control) this, "kv57.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmParam";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Параметры";
      this.Load += new EventHandler(this.FrmParam_Load);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.pnlParam.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.dsMain.EndInit();
      this.dtType.EndInit();
      this.dtAreal.EndInit();
      this.pnlCmpParam.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
