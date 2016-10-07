// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmMSP
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Forms.Controls;
using Kvartplata.Properties;
using NHibernate;
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
  public class FrmMSP : Form
  {
    private readonly FormStateSaver fss = new FormStateSaver(FrmMSP.ic);
    private bool isPast = false;
    private bool _readOnlyService = false;
    private bool _readOnlyMSP = false;
    private IContainer components = (IContainer) null;
    private static IContainer ic;
    private readonly ISession session;
    private DateTime? closedPeriod;
    private Company _company;
    private ToolStrip toolStrip1;
    private Panel pnldcMPS;
    private Panel pnlsdcMPSPersent;
    private GroupBox groupBox1;
    private ComboBox cbMSP;
    private Label label2;
    private ToolStripButton tsbdcMSP;
    private ToolStripButton tsbsdcMPSPersent;
    private Button btnPast;
    private Label lblPast;
    private DataSet dsMain;
    private DataTable dtSpreading;
    private DataColumn dataColumn1;
    private DataColumn dataColumn2;
    private ToolStripButton tsbExit;
    private DataTable dtShare;
    private DataColumn dataColumn3;
    private DataColumn dataColumn4;
    private DataTable dtAccount;
    private DataColumn dataColumn5;
    private DataColumn dataColumn6;
    private CalendarColumn calendarColumn1;
    private CalendarColumn calendarColumn2;
    private CalendarColumn calendarColumn3;
    private CalendarColumn calendarColumn4;
    private CalendarColumn calendarColumn5;
    private CalendarColumn calendarColumn6;
    private CalendarColumn calendarColumn7;
    private CalendarColumn calendarColumn8;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private CalendarColumn calendarColumn9;
    private CalendarColumn calendarColumn10;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private UCMSPPercent ucmspPercent1;
    private UCMSP ucmsp1;
    private CheckBox cbArchive;
    public HelpProvider hp;
    private Timer tmr;

    public short Company_id { get; set; }

    public FrmMSP(Company company)
    {
      this.InitializeComponent();
      this._company = company;
      this.CheckAccess();
      this.fss.ParentForm = (Form) this;
      this.session = Domain.CurrentSession;
      this.ucmsp1.session = Domain.CurrentSession;
      this.ucmsp1.tsbExit.Visible = false;
      this.ucmsp1.LoadSettings();
      this.ucmspPercent1.session = Domain.CurrentSession;
      this.ucmspPercent1.tsbExit.Visible = false;
      this.ucmspPercent1.tsbExit.Visible = false;
      this.ucmspPercent1.LoadSettings();
      KvrplHelper.AddRow(this.dtAccount, 0, "Нет");
      KvrplHelper.AddRow(this.dtAccount, 1, "Да");
      this.cbMSP.DisplayMember = "MSP_name";
      this.cbMSP.ValueMember = "MSP_id";
      this.Company_id = company.CompanyId;
    }

    private void CheckAccess()
    {
      this._readOnlyService = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(32, this._company, false));
      this._readOnlyMSP = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(80, this._company, false));
      this.ucmsp1.dgvBase.ReadOnly = !this._readOnlyService;
      this.ucmsp1.toolStrip1.Visible = this._readOnlyService;
      this.ucmspPercent1.dgvBase.ReadOnly = !this._readOnlyMSP;
      this.ucmspPercent1.toolStrip1.Visible = this._readOnlyMSP;
    }

    private Period GetClosedPeriod()
    {
      return this.session.Get<Period>((object) Convert.ToInt32(this.session.CreateQuery(string.Format("select max(p.Period_id) from ClosedPeriod p where (p.Complex_id = {0} or p.Complex_id = {2}) ", (object) Options.ComplexPasp.ComplexId, (object) this.Company_id, (object) Options.ComplexPrior.IdFk)).UniqueResult()));
    }

    private void FrmMSP_Load(object sender, EventArgs e)
    {
      if (this.session != null && this.session.IsOpen)
        this.session.Clear();
      IList list1 = this.session.CreateQuery("from Service s where s.Root=0 and s.ServiceId>0 order by " + Options.SortService).List();
      list1.Insert(0, (object) new Service());
      ((DataGridViewComboBoxColumn) this.ucmspPercent1.dgvBase.Columns["serviceColumn"]).DataSource = (object) list1;
      ((DataGridViewComboBoxColumn) this.ucmspPercent1.dgvBase.Columns["spreadingColumn"]).DataSource = (object) this.session.CreateQuery("from Spreading").List();
      IList list2 = this.session.CreateQuery("from dcShare").List();
      list2.Insert(0, (object) new dcShare()
      {
        Share_id = (short) -1
      });
      ((DataGridViewComboBoxColumn) this.ucmspPercent1.dgvBase.Columns["Share_id"]).DataSource = (object) list2;
      ((DataGridViewComboBoxColumn) this.ucmsp1.dgvBase.Columns["accountColumn"]).DataSource = (object) this.dtAccount;
      Period closedPeriod = this.GetClosedPeriod();
      DateTime? periodName = closedPeriod.PeriodName;
      if (periodName.HasValue)
      {
        periodName = closedPeriod.PeriodName;
        this.closedPeriod = new DateTime?(periodName.Value);
      }
      else
        this.closedPeriod = new DateTime?();
      this.ucmsp1.closedPeriod = this.closedPeriod;
      this.ucmsp1.LoadData();
      this.ucmspPercent1.closedPeriod = this.closedPeriod;
      if (this.ucmsp1.ObjectsList.Count <= 0)
        return;
      this.ucmspPercent1.CurMSP = (DcMSP) this.ucmsp1.ObjectsList[0];
    }

    private void tsbdcMSP_Click(object sender, EventArgs e)
    {
      if (this.ucmspPercent1.tsbApplay.Enabled)
      {
        if (MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
          this.ucmspPercent1.SaveData();
          this.tsbdcMSP.Checked = false;
          return;
        }
        this.ucmspPercent1.LoadData();
        this.ucmspPercent1.CancelEnabled();
      }
      else
      {
        this.ucmsp1.LoadData();
        this.ucmspPercent1.LoadData();
      }
      this.tsbsdcMPSPersent.Checked = false;
      this.pnldcMPS.BringToFront();
    }

    private void tsbsdcMPSPersent_Click(object sender, EventArgs e)
    {
      if (this.ucmsp1.tsbApplay.Enabled)
      {
        if (MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
          this.ucmsp1.SaveData();
          this.tsbsdcMPSPersent.Checked = false;
          return;
        }
        this.ucmsp1.LoadData();
        this.ucmsp1.CancelEnabled();
      }
      else
      {
        this.ucmsp1.LoadData();
        this.ucmspPercent1.LoadData();
      }
      this.tsbdcMSP.Checked = false;
      this.pnlsdcMPSPersent.BringToFront();
    }

    private void btnPast_Click(object sender, EventArgs e)
    {
      this.isPast = !this.isPast;
      this.ucmspPercent1.IsPast = this.isPast;
      if (this.ucmspPercent1.IsPast == this.isPast)
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

    private void cbMSP_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cbMSP.SelectedItem == null)
        return;
      this.ucmspPercent1.CurMSP = (DcMSP) this.cbMSP.SelectedItem;
      if (this.ucmspPercent1.CurMSP != (DcMSP) this.cbMSP.SelectedItem)
        this.cbMSP.SelectedItem = (object) this.ucmspPercent1.CurMSP;
    }

    private void tsbExit_Click(object sender, EventArgs e)
    {
      if ((this.ucmsp1.tsbApplay.Enabled || this.ucmspPercent1.tsbApplay.Enabled) && MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
      {
        this.tsbExit.Checked = false;
        if (this.ucmsp1.tsbApplay.Enabled)
        {
          this.ucmsp1.SaveData();
        }
        else
        {
          if (!this.ucmspPercent1.tsbApplay.Enabled)
            return;
          this.ucmspPercent1.SaveData();
        }
      }
      else
        this.Close();
    }

    private void ucmsp1_CurObjectChanged(object sender, EventArgs e)
    {
      this.cbMSP.SelectedItem = (object) this.ucmsp1.CurMSP;
    }

    private void ucmsp1_ObjectsListChanged(object sender, EventArgs e)
    {
      this.cbMSP.Items.Clear();
      List<DcMSP> source = new List<DcMSP>();
      foreach (DcMSP objects in (IEnumerable) this.ucmsp1.ObjectsList)
        source.Add(objects);
      foreach (object obj in (IEnumerable<DcMSP>) source.OrderBy<DcMSP, string>((Func<DcMSP, string>) (_l => _l.MSP_name)))
        this.cbMSP.Items.Add(obj);
      this.cbMSP.SelectedItem = (object) this.ucmsp1.CurMSP;
    }

    private void ucmspPercent1_ObjectsListChanged(object sender, EventArgs e)
    {
      if (!this.ucmspPercent1.IsCopy)
        return;
      this.isPast = !this.isPast;
      this.groupBox1.Height += 18;
      this.ucmspPercent1.IsCopy = false;
    }

    private void cbArchive_CheckedChanged(object sender, EventArgs e)
    {
      this.ucmspPercent1.IsArchive = this.cbArchive.Checked;
      this.ucmspPercent1.LoadData();
    }

    private void tmr_Tick(object sender, EventArgs e)
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmMSP));
      this.dsMain = new DataSet();
      this.dtSpreading = new DataTable();
      this.dataColumn1 = new DataColumn();
      this.dataColumn2 = new DataColumn();
      this.dtShare = new DataTable();
      this.dataColumn3 = new DataColumn();
      this.dataColumn4 = new DataColumn();
      this.dtAccount = new DataTable();
      this.dataColumn5 = new DataColumn();
      this.dataColumn6 = new DataColumn();
      this.toolStrip1 = new ToolStrip();
      this.tsbdcMSP = new ToolStripButton();
      this.tsbsdcMPSPersent = new ToolStripButton();
      this.tsbExit = new ToolStripButton();
      this.pnldcMPS = new Panel();
      this.ucmsp1 = new UCMSP();
      this.pnlsdcMPSPersent = new Panel();
      this.ucmspPercent1 = new UCMSPPercent();
      this.groupBox1 = new GroupBox();
      this.cbArchive = new CheckBox();
      this.lblPast = new Label();
      this.btnPast = new Button();
      this.cbMSP = new ComboBox();
      this.label2 = new Label();
      this.hp = new HelpProvider();
      this.tmr = new Timer(this.components);
      this.calendarColumn1 = new CalendarColumn();
      this.calendarColumn2 = new CalendarColumn();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.calendarColumn3 = new CalendarColumn();
      this.calendarColumn4 = new CalendarColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
      this.calendarColumn5 = new CalendarColumn();
      this.calendarColumn6 = new CalendarColumn();
      this.calendarColumn7 = new CalendarColumn();
      this.calendarColumn8 = new CalendarColumn();
      this.calendarColumn9 = new CalendarColumn();
      this.calendarColumn10 = new CalendarColumn();
      this.dsMain.BeginInit();
      this.dtSpreading.BeginInit();
      this.dtShare.BeginInit();
      this.dtAccount.BeginInit();
      this.toolStrip1.SuspendLayout();
      this.pnldcMPS.SuspendLayout();
      this.pnlsdcMPSPersent.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.dsMain.DataSetName = "NewDataSet";
      this.dsMain.Tables.AddRange(new DataTable[3]
      {
        this.dtSpreading,
        this.dtShare,
        this.dtAccount
      });
      this.dtSpreading.Columns.AddRange(new DataColumn[2]
      {
        this.dataColumn1,
        this.dataColumn2
      });
      this.dtSpreading.TableName = "Table1";
      this.dataColumn1.ColumnName = "Id";
      this.dataColumn1.DataType = typeof (int);
      this.dataColumn2.ColumnName = "Name";
      this.dtShare.Columns.AddRange(new DataColumn[2]
      {
        this.dataColumn3,
        this.dataColumn4
      });
      this.dtShare.TableName = "Table2";
      this.dataColumn3.ColumnName = "id";
      this.dataColumn3.DataType = typeof (short);
      this.dataColumn4.ColumnName = "name";
      this.dtAccount.Columns.AddRange(new DataColumn[2]
      {
        this.dataColumn5,
        this.dataColumn6
      });
      this.dtAccount.TableName = "Table3";
      this.dataColumn5.ColumnName = "id";
      this.dataColumn5.DataType = typeof (short);
      this.dataColumn6.ColumnName = "name";
      this.toolStrip1.Dock = DockStyle.Left;
      this.toolStrip1.Font = new Font("Tahoma", 10f);
      this.toolStrip1.ImageScalingSize = new Size(42, 42);
      this.toolStrip1.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.tsbdcMSP,
        (ToolStripItem) this.tsbsdcMPSPersent,
        (ToolStripItem) this.tsbExit
      });
      this.toolStrip1.Location = new Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Padding = new Padding(0, 0, 2, 0);
      this.toolStrip1.Size = new Size(91, 427);
      this.toolStrip1.TabIndex = 1;
      this.toolStrip1.Text = "toolStrip1";
      this.tsbdcMSP.Checked = true;
      this.tsbdcMSP.CheckOnClick = true;
      this.tsbdcMSP.CheckState = CheckState.Checked;
      this.tsbdcMSP.Image = (Image) Resources.allClasses;
      this.tsbdcMSP.ImageTransparentColor = Color.Magenta;
      this.tsbdcMSP.Name = "tsbdcMSP";
      this.tsbdcMSP.Size = new Size(86, 63);
      this.tsbdcMSP.Text = "Словарь";
      this.tsbdcMSP.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tsbdcMSP.Click += new EventHandler(this.tsbdcMSP_Click);
      this.tsbsdcMPSPersent.CheckOnClick = true;
      this.tsbsdcMPSPersent.Image = (Image) Resources.percent;
      this.tsbsdcMPSPersent.ImageTransparentColor = Color.Magenta;
      this.tsbsdcMPSPersent.Name = "tsbsdcMPSPersent";
      this.tsbsdcMPSPersent.Size = new Size(86, 63);
      this.tsbsdcMPSPersent.Text = "Размер МСП";
      this.tsbsdcMPSPersent.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tsbsdcMPSPersent.Click += new EventHandler(this.tsbsdcMPSPersent_Click);
      this.tsbExit.Image = (Image) Resources.Exit;
      this.tsbExit.ImageTransparentColor = Color.Magenta;
      this.tsbExit.Name = "tsbExit";
      this.tsbExit.Size = new Size(86, 63);
      this.tsbExit.Text = "Выход";
      this.tsbExit.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tsbExit.Click += new EventHandler(this.tsbExit_Click);
      this.pnldcMPS.Controls.Add((Control) this.ucmsp1);
      this.pnldcMPS.Dock = DockStyle.Fill;
      this.pnldcMPS.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.pnldcMPS.Location = new Point(91, 0);
      this.pnldcMPS.Margin = new Padding(4);
      this.pnldcMPS.Name = "pnldcMPS";
      this.pnldcMPS.Size = new Size(650, 427);
      this.pnldcMPS.TabIndex = 3;
      this.ucmsp1.Dock = DockStyle.Fill;
      this.ucmsp1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.ucmsp1.Location = new Point(0, 0);
      this.ucmsp1.Margin = new Padding(0);
      this.ucmsp1.Name = "ucmsp1";
      this.ucmsp1.Size = new Size(650, 427);
      this.ucmsp1.TabIndex = 0;
      this.ucmsp1.ObjectsListChanged += new EventHandler(this.ucmsp1_ObjectsListChanged);
      this.ucmsp1.CurObjectChanged += new EventHandler(this.ucmsp1_CurObjectChanged);
      this.pnlsdcMPSPersent.Controls.Add((Control) this.ucmspPercent1);
      this.pnlsdcMPSPersent.Controls.Add((Control) this.groupBox1);
      this.pnlsdcMPSPersent.Dock = DockStyle.Fill;
      this.pnlsdcMPSPersent.Location = new Point(91, 0);
      this.pnlsdcMPSPersent.Margin = new Padding(4);
      this.pnlsdcMPSPersent.Name = "pnlsdcMPSPersent";
      this.pnlsdcMPSPersent.Size = new Size(650, 427);
      this.pnlsdcMPSPersent.TabIndex = 4;
      this.ucmspPercent1.CurMSP = (DcMSP) null;
      this.ucmspPercent1.Dock = DockStyle.Fill;
      this.ucmspPercent1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.ucmspPercent1.IsPast = false;
      this.ucmspPercent1.Location = new Point(0, 46);
      this.ucmspPercent1.Margin = new Padding(4);
      this.ucmspPercent1.Name = "ucmspPercent1";
      this.ucmspPercent1.Size = new Size(650, 381);
      this.ucmspPercent1.TabIndex = 4;
      this.ucmspPercent1.ObjectsListChanged += new EventHandler(this.ucmspPercent1_ObjectsListChanged);
      this.groupBox1.Controls.Add((Control) this.cbArchive);
      this.groupBox1.Controls.Add((Control) this.lblPast);
      this.groupBox1.Controls.Add((Control) this.btnPast);
      this.groupBox1.Controls.Add((Control) this.cbMSP);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Dock = DockStyle.Top;
      this.groupBox1.Location = new Point(0, 0);
      this.groupBox1.Margin = new Padding(4);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new Padding(4);
      this.groupBox1.Size = new Size(650, 46);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.cbArchive.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cbArchive.AutoSize = true;
      this.cbArchive.Location = new Point(407, 20);
      this.cbArchive.Name = "cbArchive";
      this.cbArchive.Size = new Size(65, 21);
      this.cbArchive.TabIndex = 10;
      this.cbArchive.Text = "Архив";
      this.cbArchive.UseVisualStyleBackColor = true;
      this.cbArchive.CheckedChanged += new EventHandler(this.cbArchive_CheckedChanged);
      this.lblPast.AutoSize = true;
      this.lblPast.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblPast.ForeColor = Color.DarkOrange;
      this.lblPast.Location = new Point(274, 46);
      this.lblPast.Margin = new Padding(4, 0, 4, 0);
      this.lblPast.Name = "lblPast";
      this.lblPast.Size = new Size(199, 16);
      this.lblPast.TabIndex = 9;
      this.lblPast.Text = "Режим прошлого времени";
      this.btnPast.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPast.Image = (Image) Resources.time_24;
      this.btnPast.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnPast.Location = new Point(479, 14);
      this.btnPast.Margin = new Padding(4);
      this.btnPast.Name = "btnPast";
      this.btnPast.Size = new Size(163, 30);
      this.btnPast.TabIndex = 4;
      this.btnPast.Text = "Прошлое время";
      this.btnPast.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.btnPast.UseVisualStyleBackColor = true;
      this.btnPast.Click += new EventHandler(this.btnPast_Click);
      this.cbMSP.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cbMSP.FormattingEnabled = true;
      this.cbMSP.Location = new Point(131, 18);
      this.cbMSP.Margin = new Padding(4);
      this.cbMSP.Name = "cbMSP";
      this.cbMSP.Size = new Size(269, 24);
      this.cbMSP.TabIndex = 3;
      this.cbMSP.SelectedIndexChanged += new EventHandler(this.cbMSP_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(2, 21);
      this.label2.Margin = new Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(116, 17);
      this.label2.TabIndex = 1;
      this.label2.Text = "Категория льгот";
      this.hp.HelpNamespace = "Help.chm";
      this.tmr.Interval = 1000;
      this.tmr.Tick += new EventHandler(this.tmr_Tick);
      gridViewCellStyle1.Format = "d";
      this.calendarColumn1.DefaultCellStyle = gridViewCellStyle1;
      this.calendarColumn1.HeaderText = "Дата начала действия";
      this.calendarColumn1.Name = "calendarColumn1";
      this.calendarColumn1.Resizable = DataGridViewTriState.True;
      this.calendarColumn1.SortMode = DataGridViewColumnSortMode.Automatic;
      gridViewCellStyle2.Format = "d";
      this.calendarColumn2.DefaultCellStyle = gridViewCellStyle2;
      this.calendarColumn2.HeaderText = "Дата окончания действия";
      this.calendarColumn2.Name = "calendarColumn2";
      this.calendarColumn2.Resizable = DataGridViewTriState.True;
      this.calendarColumn2.SortMode = DataGridViewColumnSortMode.Automatic;
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
      gridViewCellStyle5.Format = "d";
      this.calendarColumn4.DefaultCellStyle = gridViewCellStyle5;
      this.calendarColumn4.HeaderText = "Дата окончания действия";
      this.calendarColumn4.Name = "calendarColumn4";
      this.calendarColumn4.Resizable = DataGridViewTriState.True;
      this.calendarColumn4.SortMode = DataGridViewColumnSortMode.Automatic;
      gridViewCellStyle6.Format = "N0";
      gridViewCellStyle6.NullValue = (object) null;
      this.dataGridViewTextBoxColumn2.DefaultCellStyle = gridViewCellStyle6;
      this.dataGridViewTextBoxColumn2.HeaderText = "Процент";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn3.HeaderText = "Схема";
      this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
      gridViewCellStyle7.Format = "d";
      this.calendarColumn5.DefaultCellStyle = gridViewCellStyle7;
      this.calendarColumn5.HeaderText = "Дата начала действия";
      this.calendarColumn5.Name = "calendarColumn5";
      this.calendarColumn5.Resizable = DataGridViewTriState.True;
      this.calendarColumn5.SortMode = DataGridViewColumnSortMode.Automatic;
      gridViewCellStyle8.Format = "d";
      this.calendarColumn6.DefaultCellStyle = gridViewCellStyle8;
      this.calendarColumn6.HeaderText = "Дата окончания действия";
      this.calendarColumn6.Name = "calendarColumn6";
      this.calendarColumn6.Resizable = DataGridViewTriState.True;
      this.calendarColumn6.SortMode = DataGridViewColumnSortMode.Automatic;
      gridViewCellStyle9.Format = "d";
      this.calendarColumn7.DefaultCellStyle = gridViewCellStyle9;
      this.calendarColumn7.HeaderText = "Дата начала действия";
      this.calendarColumn7.Name = "calendarColumn7";
      this.calendarColumn7.Resizable = DataGridViewTriState.True;
      this.calendarColumn7.SortMode = DataGridViewColumnSortMode.Automatic;
      gridViewCellStyle10.Format = "d";
      this.calendarColumn8.DefaultCellStyle = gridViewCellStyle10;
      this.calendarColumn8.HeaderText = "Дата окончания действия";
      this.calendarColumn8.Name = "calendarColumn8";
      this.calendarColumn8.Resizable = DataGridViewTriState.True;
      this.calendarColumn8.SortMode = DataGridViewColumnSortMode.Automatic;
      gridViewCellStyle11.Format = "d";
      this.calendarColumn9.DefaultCellStyle = gridViewCellStyle11;
      this.calendarColumn9.HeaderText = "Дата начала действия";
      this.calendarColumn9.Name = "calendarColumn9";
      this.calendarColumn9.Resizable = DataGridViewTriState.True;
      this.calendarColumn9.SortMode = DataGridViewColumnSortMode.Automatic;
      gridViewCellStyle12.Format = "d";
      this.calendarColumn10.DefaultCellStyle = gridViewCellStyle12;
      this.calendarColumn10.HeaderText = "Дата окончания действия";
      this.calendarColumn10.Name = "calendarColumn10";
      this.calendarColumn10.Resizable = DataGridViewTriState.True;
      this.calendarColumn10.SortMode = DataGridViewColumnSortMode.Automatic;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(741, 427);
      this.Controls.Add((Control) this.pnldcMPS);
      this.Controls.Add((Control) this.pnlsdcMPSPersent);
      this.Controls.Add((Control) this.toolStrip1);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.hp.SetHelpKeyword((Control) this, "kv52.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmMSP";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Справочник льгот";
      this.Load += new EventHandler(this.FrmMSP_Load);
      this.dsMain.EndInit();
      this.dtSpreading.EndInit();
      this.dtShare.EndInit();
      this.dtAccount.EndInit();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.pnldcMPS.ResumeLayout(false);
      this.pnlsdcMPSPersent.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
