// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmCoeffLocation
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
  public class FrmCoeffLocation : Form
  {
    private short company_id = 0;
    private IList companyList = (IList) new ArrayList();
    private FormStateSaver fss = new FormStateSaver(FrmCoeffLocation.ic);
    private bool _readOnlyService = false;
    private bool _readOnly = false;
    private IContainer components = (IContainer) null;
    private ISession session;
    private DateTime? closedPeriod;
    private static IContainer ic;
    private Company _company;
    private DataColumn dataColumn2;
    private DataColumn dataColumn1;
    private DataTable dtYesNo;
    private DataSet dsMain;
    private ToolStrip toolStrip1;
    private ToolStripButton tsbReceipt;
    private ToolStripButton tsbCmpReceipt;
    private ToolStripButton tsbExit;
    private Panel pnlCounterLocation;
    private Panel pnlCmpCoeffLocation;
    private GroupBox groupBox1;
    private ComboBox cbCompany;
    private Label label1;
    public HelpProvider hp;
    private UCCounterLocation ucCounterLocation1;
    private UCCmpCoeffLocation ucCmpCoeffLocation1;
    private Button btnPastTime;
    private CheckBox chbArhiv;
    private Timer tmr;
    private Label lblPastTime;

    public short Company_id
    {
      get
      {
        return this.company_id;
      }
      set
      {
        this.company_id = value;
        Company cmp = this.session.Get<Company>((object) value);
        this.cbCompany.SelectedValue = (object) cmp.CompanyId;
        this.ucCounterLocation1.LoadData();
        if (this.cbCompany.SelectedItem != null)
          this.ucCmpCoeffLocation1.Company_id = this.company_id;
        Period cmpKvrClose = KvrplHelper.GetCmpKvrClose(cmp, 100, Options.ComplexPrior.ComplexId);
        DateTime? periodName = cmpKvrClose.PeriodName;
        if (periodName.HasValue)
        {
          periodName = cmpKvrClose.PeriodName;
          this.closedPeriod = new DateTime?(periodName.Value);
        }
        else
          this.closedPeriod = new DateTime?();
        this.ucCmpCoeffLocation1.closedPeriod = this.closedPeriod;
      }
    }

    public FrmCoeffLocation(Company company)
    {
      this.InitializeComponent();
      this._company = company;
      this.CheckAccess();
      this.fss.ParentForm = (Form) this;
      this.session = Domain.CurrentSession;
      this.ucCmpCoeffLocation1.session = Domain.CurrentSession;
      this.ucCounterLocation1.session = Domain.CurrentSession;
      this.ucCounterLocation1.tsbExit.Visible = false;
      this.ucCounterLocation1.LoadSettings();
      this.ucCmpCoeffLocation1.tsbExit.Visible = false;
      this.ucCmpCoeffLocation1.LoadSettings();
      this.ucCmpCoeffLocation1.IsArchive = false;
      this.companyList = this.session.CreateQuery("from Company order by CompanyId").List();
      this.cbCompany.DataSource = (object) this.companyList;
      this.cbCompany.DisplayMember = "CompanyName";
      this.cbCompany.ValueMember = "CompanyId";
      this.Company_id = Options.Company.CompanyId;
    }

    private void CheckAccess()
    {
      this._readOnlyService = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(32, this._company, false));
      this._readOnly = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(33, this._company, false));
      this.ucCounterLocation1.toolStrip1.Visible = this._readOnlyService;
      this.ucCounterLocation1.dgvBase.ReadOnly = !this._readOnlyService;
      this.ucCmpCoeffLocation1.toolStrip1.Visible = this._readOnly;
      this.ucCmpCoeffLocation1.dgvBase.ReadOnly = !this._readOnly;
    }

    private void FrmCoeffLocation_Load(object sender, EventArgs e)
    {
      if (this.session != null && this.session.IsOpen)
        this.session.Clear();
      IList list = this.session.CreateQuery("from CounterLocation order by CntrLocationName").List();
      list.Insert(0, (object) new CounterLocation());
      ((DataGridViewComboBoxColumn) this.ucCmpCoeffLocation1.dgvBase.Columns["CntrLocation"]).DataSource = (object) list;
    }

    private void tsbReceipt_Click(object sender, EventArgs e)
    {
      if (this.ucCmpCoeffLocation1.tsbApplay.Enabled)
      {
        if (MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
          this.ucCmpCoeffLocation1.SaveData();
          this.tsbReceipt.Checked = false;
          return;
        }
        this.ucCmpCoeffLocation1.LoadData();
        this.ucCmpCoeffLocation1.CancelEnabled();
      }
      else
      {
        this.ucCounterLocation1.LoadData();
        this.ucCmpCoeffLocation1.LoadData();
      }
      this.tsbCmpReceipt.Checked = false;
      this.pnlCounterLocation.BringToFront();
    }

    private void tsbCmpReceipt_Click(object sender, EventArgs e)
    {
      if (this.ucCounterLocation1.tsbApplay.Enabled)
      {
        if (MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
          this.ucCounterLocation1.SaveData();
          this.tsbCmpReceipt.Checked = false;
          return;
        }
        this.ucCounterLocation1.LoadData();
        this.ucCounterLocation1.CancelEnabled();
      }
      else
      {
        this.ucCounterLocation1.LoadData();
        this.ucCmpCoeffLocation1.LoadData();
      }
      this.tsbReceipt.Checked = false;
      this.pnlCmpCoeffLocation.BringToFront();
    }

    private void tsbExit_Click(object sender, EventArgs e)
    {
      if ((this.ucCounterLocation1.tsbApplay.Enabled || this.ucCmpCoeffLocation1.tsbApplay.Enabled) && MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
      {
        this.tsbExit.Checked = false;
        if (this.ucCounterLocation1.tsbApplay.Enabled)
        {
          this.ucCounterLocation1.SaveData();
        }
        else
        {
          if (!this.ucCmpCoeffLocation1.tsbApplay.Enabled)
            return;
          this.ucCmpCoeffLocation1.SaveData();
        }
      }
      else
        this.Close();
    }

    private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cbCompany.SelectedItem == null)
        return;
      if (this.tsbCmpReceipt.Checked && !KvrplHelper.CheckProxy(32, 1, (Company) this.cbCompany.SelectedItem, true))
        this.cbCompany.SelectedItem = (object) this.session.Get<Company>((object) this.Company_id);
      else
        this.ucCmpCoeffLocation1.Company_id = ((Company) this.cbCompany.SelectedItem).CompanyId;
      if ((int) this.ucCmpCoeffLocation1.Company_id != (int) ((Company) this.cbCompany.SelectedItem).CompanyId)
        this.cbCompany.SelectedItem = (object) this.session.Get<Company>((object) this.ucCmpCoeffLocation1.Company_id);
    }

    private void btnPastTime_Click(object sender, EventArgs e)
    {
      if (this.ucCmpCoeffLocation1.IsPast)
      {
        this.ucCmpCoeffLocation1.IsPast = false;
        this.ucCmpCoeffLocation1.LoadData();
        if (this.ucCmpCoeffLocation1.IsPast)
          return;
        this.lblPastTime.Visible = false;
        this.btnPastTime.BackColor = this.groupBox1.BackColor;
        this.tmr.Stop();
      }
      else
      {
        this.ucCmpCoeffLocation1.IsPast = true;
        this.ucCmpCoeffLocation1.LoadData();
        if (this.ucCmpCoeffLocation1.IsPast)
        {
          this.lblPastTime.Visible = true;
          this.btnPastTime.BackColor = Color.DarkOrange;
          this.tmr.Start();
        }
      }
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
      this.ucCmpCoeffLocation1.IsArchive = this.chbArhiv.Checked;
      this.ucCmpCoeffLocation1.LoadData();
    }

    private void ucCounterLocation1_ObjectsListChanged(object sender, EventArgs e)
    {
      IList list = this.session.CreateQuery("from CounterLocation order by CntrLocationName").List();
      list.Insert(0, (object) new CounterLocation());
      ((DataGridViewComboBoxColumn) this.ucCmpCoeffLocation1.dgvBase.Columns["CntrLocation"]).DataSource = (object) list;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmCoeffLocation));
      this.dataColumn2 = new DataColumn();
      this.dataColumn1 = new DataColumn();
      this.dtYesNo = new DataTable();
      this.dsMain = new DataSet();
      this.toolStrip1 = new ToolStrip();
      this.tsbReceipt = new ToolStripButton();
      this.tsbCmpReceipt = new ToolStripButton();
      this.tsbExit = new ToolStripButton();
      this.pnlCounterLocation = new Panel();
      this.ucCounterLocation1 = new UCCounterLocation();
      this.pnlCmpCoeffLocation = new Panel();
      this.ucCmpCoeffLocation1 = new UCCmpCoeffLocation();
      this.groupBox1 = new GroupBox();
      this.lblPastTime = new Label();
      this.chbArhiv = new CheckBox();
      this.btnPastTime = new Button();
      this.cbCompany = new ComboBox();
      this.label1 = new Label();
      this.hp = new HelpProvider();
      this.tmr = new Timer(this.components);
      this.dtYesNo.BeginInit();
      this.dsMain.BeginInit();
      this.toolStrip1.SuspendLayout();
      this.pnlCounterLocation.SuspendLayout();
      this.pnlCmpCoeffLocation.SuspendLayout();
      this.groupBox1.SuspendLayout();
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
      this.toolStrip1.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.tsbReceipt,
        (ToolStripItem) this.tsbCmpReceipt,
        (ToolStripItem) this.tsbExit
      });
      this.toolStrip1.Location = new Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Padding = new Padding(0, 0, 2, 0);
      this.toolStrip1.Size = new Size(114, 322);
      this.toolStrip1.TabIndex = 0;
      this.toolStrip1.Text = "toolStrip1";
      this.tsbReceipt.Checked = true;
      this.tsbReceipt.CheckOnClick = true;
      this.tsbReceipt.CheckState = CheckState.Checked;
      this.tsbReceipt.Font = new Font("Tahoma", 10f);
      this.tsbReceipt.Image = (Image) Resources.allClasses;
      this.tsbReceipt.ImageTransparentColor = Color.Magenta;
      this.tsbReceipt.Name = "tsbReceipt";
      this.tsbReceipt.Size = new Size(109, 69);
      this.tsbReceipt.Text = "Словарь типов";
      this.tsbReceipt.TextDirection = ToolStripTextDirection.Horizontal;
      this.tsbReceipt.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tsbReceipt.Click += new EventHandler(this.tsbReceipt_Click);
      this.tsbCmpReceipt.CheckOnClick = true;
      this.tsbCmpReceipt.Image = (Image) Resources.documents;
      this.tsbCmpReceipt.ImageTransparentColor = Color.Magenta;
      this.tsbCmpReceipt.Name = "tsbCmpReceipt";
      this.tsbCmpReceipt.Size = new Size(109, 69);
      this.tsbCmpReceipt.Text = "Коэффициенты";
      this.tsbCmpReceipt.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tsbCmpReceipt.Click += new EventHandler(this.tsbCmpReceipt_Click);
      this.tsbExit.Image = (Image) Resources.Exit;
      this.tsbExit.ImageTransparentColor = Color.Magenta;
      this.tsbExit.Name = "tsbExit";
      this.tsbExit.Size = new Size(109, 69);
      this.tsbExit.Text = "Выход";
      this.tsbExit.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tsbExit.Click += new EventHandler(this.tsbExit_Click);
      this.pnlCounterLocation.Controls.Add((Control) this.ucCounterLocation1);
      this.pnlCounterLocation.Dock = DockStyle.Fill;
      this.pnlCounterLocation.Location = new Point(114, 0);
      this.pnlCounterLocation.Margin = new Padding(4);
      this.pnlCounterLocation.Name = "pnlCounterLocation";
      this.pnlCounterLocation.Size = new Size(645, 322);
      this.pnlCounterLocation.TabIndex = 1;
      this.ucCounterLocation1.Dock = DockStyle.Fill;
      this.ucCounterLocation1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.ucCounterLocation1.Location = new Point(0, 0);
      this.ucCounterLocation1.Margin = new Padding(4);
      this.ucCounterLocation1.Name = "ucCounterLocation1";
      this.ucCounterLocation1.Size = new Size(645, 322);
      this.ucCounterLocation1.TabIndex = 0;
      this.ucCounterLocation1.ObjectsListChanged += new EventHandler(this.ucCounterLocation1_ObjectsListChanged);
      this.pnlCmpCoeffLocation.Controls.Add((Control) this.ucCmpCoeffLocation1);
      this.pnlCmpCoeffLocation.Controls.Add((Control) this.groupBox1);
      this.pnlCmpCoeffLocation.Dock = DockStyle.Fill;
      this.pnlCmpCoeffLocation.Location = new Point(114, 0);
      this.pnlCmpCoeffLocation.Margin = new Padding(4);
      this.pnlCmpCoeffLocation.Name = "pnlCmpCoeffLocation";
      this.pnlCmpCoeffLocation.Size = new Size(645, 322);
      this.pnlCmpCoeffLocation.TabIndex = 2;
      this.ucCmpCoeffLocation1.Company_id = (short) 0;
      this.ucCmpCoeffLocation1.Dock = DockStyle.Fill;
      this.ucCmpCoeffLocation1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.ucCmpCoeffLocation1.IsPast = false;
      this.ucCmpCoeffLocation1.Location = new Point(0, 77);
      this.ucCmpCoeffLocation1.Margin = new Padding(4);
      this.ucCmpCoeffLocation1.Name = "ucCmpCoeffLocation1";
      this.ucCmpCoeffLocation1.Size = new Size(645, 245);
      this.ucCmpCoeffLocation1.TabIndex = 1;
      this.groupBox1.Controls.Add((Control) this.lblPastTime);
      this.groupBox1.Controls.Add((Control) this.chbArhiv);
      this.groupBox1.Controls.Add((Control) this.btnPastTime);
      this.groupBox1.Controls.Add((Control) this.cbCompany);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Dock = DockStyle.Top;
      this.groupBox1.Location = new Point(0, 0);
      this.groupBox1.Margin = new Padding(4);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new Padding(4);
      this.groupBox1.Size = new Size(645, 77);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.lblPastTime.AutoSize = true;
      this.lblPastTime.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblPastTime.ForeColor = Color.DarkOrange;
      this.lblPastTime.Location = new Point(99, 53);
      this.lblPastTime.Name = "lblPastTime";
      this.lblPastTime.Size = new Size(199, 16);
      this.lblPastTime.TabIndex = 19;
      this.lblPastTime.Text = "Режим прошлого времени";
      this.lblPastTime.Visible = false;
      this.chbArhiv.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chbArhiv.AutoSize = true;
      this.chbArhiv.Location = new Point(497, 53);
      this.chbArhiv.Name = "chbArhiv";
      this.chbArhiv.Size = new Size(56, 17);
      this.chbArhiv.TabIndex = 18;
      this.chbArhiv.Text = "Архив";
      this.chbArhiv.UseVisualStyleBackColor = true;
      this.chbArhiv.CheckedChanged += new EventHandler(this.chbArhiv_CheckedChanged);
      this.btnPastTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPastTime.Image = (Image) Resources.time_24;
      this.btnPastTime.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnPastTime.Location = new Point(488, 17);
      this.btnPastTime.Name = "btnPastTime";
      this.btnPastTime.Size = new Size(145, 30);
      this.btnPastTime.TabIndex = 10;
      this.btnPastTime.Text = "Прошлое время";
      this.btnPastTime.TextAlign = ContentAlignment.MiddleRight;
      this.btnPastTime.UseVisualStyleBackColor = true;
      this.btnPastTime.Click += new EventHandler(this.btnPastTime_Click);
      this.cbCompany.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cbCompany.FormattingEnabled = true;
      this.cbCompany.Location = new Point(99, 21);
      this.cbCompany.Margin = new Padding(4);
      this.cbCompany.Name = "cbCompany";
      this.cbCompany.Size = new Size(367, 24);
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
      this.tmr.Interval = 1000;
      this.tmr.Tick += new EventHandler(this.tmr_Tick);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(759, 322);
      this.Controls.Add((Control) this.pnlCounterLocation);
      this.Controls.Add((Control) this.pnlCmpCoeffLocation);
      this.Controls.Add((Control) this.toolStrip1);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.hp.SetHelpKeyword((Control) this, "kv516.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmCoeffLocation";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Типы расположения счетчиков";
      this.Load += new EventHandler(this.FrmCoeffLocation_Load);
      this.dtYesNo.EndInit();
      this.dsMain.EndInit();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.pnlCounterLocation.ResumeLayout(false);
      this.pnlCmpCoeffLocation.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
