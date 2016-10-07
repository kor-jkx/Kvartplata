// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmCrossService
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
  public class FrmCrossService : FrmBaseForm
  {
    private short company_id = 0;
    private IList companyList = (IList) new ArrayList();
    private FormStateSaver fss = new FormStateSaver(FrmCrossService.ic);
    private bool _readOnly = false;
    private bool _readOnlyService = false;
    private IContainer components = (IContainer) null;
    private ISession session;
    private static IContainer ic;
    private Company _company;
    private Panel pnBtn;
    private Button bntExit;
    private ToolStrip tsCrossService;
    private ToolStripButton tsbCrossType;
    private ToolStripButton tsbCrossService;
    private Panel pnCrossType;
    private UCCrossType ucCrossType1;
    private Panel pnCrossService;
    private Panel pnUp;
    private ComboBox cmbCompany;
    private Label lblCompany;
    private UCCrossService ucCrossService1;
    public HelpProvider hp;
    private CheckBox cbArchive;

    public short Company_id
    {
      get
      {
        return this.company_id;
      }
      set
      {
        this.company_id = value;
        this.cmbCompany.SelectedItem = (object) this.session.Get<Company>((object) value);
        this.ucCrossType1.LoadData();
        if (this.cmbCompany.SelectedItem == null)
          return;
        this.ucCrossService1.Company_id = this.company_id;
      }
    }

    public FrmCrossService(Company company)
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
      this.session = Domain.CurrentSession;
      this._company = company;
      this.CheckAccess();
      this.ucCrossType1.session = Domain.CurrentSession;
      this.ucCrossService1.session = Domain.CurrentSession;
      this.ucCrossType1.tsbExit.Visible = false;
      this.ucCrossType1.LoadSettings();
      this.ucCrossService1.tsbExit.Visible = false;
      this.ucCrossService1.MonthClosed = KvrplHelper.GetCmpKvrClose(Options.Company, Options.ComplexPasp.ComplexId, Options.ComplexPrior.ComplexId);
      this.ucCrossService1.LoadSettings();
      this.companyList = this.session.CreateQuery("from Company order by CompanyId").List();
      this.cmbCompany.DataSource = (object) this.companyList;
      this.cmbCompany.DisplayMember = "CompanyName";
      this.cmbCompany.ValueMember = "CompanyId";
      this.Company_id = Options.Company.CompanyId;
    }

    private void CheckAccess()
    {
      this._readOnlyService = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(32, this._company, false));
      this._readOnly = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(33, this._company, false));
      this.ucCrossType1.toolStrip1.Visible = this._readOnlyService;
      this.ucCrossType1.dgvBase.ReadOnly = !this._readOnlyService;
      this.ucCrossService1.toolStrip1.Visible = this._readOnly;
      this.ucCrossService1.dgvBase.ReadOnly = !this._readOnly;
    }

    private void bntExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void FrmCrossService_Load(object sender, EventArgs e)
    {
      if (this.session != null && this.session.IsOpen)
        this.session.Clear();
      IList<Service> serviceList1 = this.session.CreateQuery(string.Format("select new Service(s.ServiceId,s.ServiceName) from Service s where s.Root=0 and s.ServiceId<>0 order by " + Options.SortService)).List<Service>();
      serviceList1.Insert(0, new Service((short) 0, ""));
      ((DataGridViewComboBoxColumn) this.ucCrossService1.dgvBase.Columns["Service"]).DataSource = (object) serviceList1;
      IList<Service> serviceList2 = this.session.CreateQuery(string.Format("select new Service(s.ServiceId,s.ServiceName) from Service s where s.Root=0 and s.ServiceId<>0 order by " + Options.SortService)).List<Service>();
      serviceList2.Insert(0, new Service((short) 0, ""));
      ((DataGridViewComboBoxColumn) this.ucCrossService1.dgvBase.Columns["CrossServ"]).DataSource = (object) serviceList2;
      IList<CrossType> crossTypeList = this.session.CreateCriteria(typeof (CrossType)).AddOrder(Order.Asc("CrossTypeName")).List<CrossType>();
      crossTypeList.Insert(0, new CrossType((short) 0, ""));
      ((DataGridViewComboBoxColumn) this.ucCrossService1.dgvBase.Columns["CrossType"]).DataSource = (object) crossTypeList;
      if (KvrplHelper.CheckProxy(32, 1, (Company) null, false))
        return;
      this.tsbCrossService.Checked = true;
      this.tsbCrossService_Click(sender, e);
    }

    private void tsbCrossType_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 1, (Company) null, true))
      {
        this.tsbCrossType.Checked = false;
      }
      else
      {
        if (this.ucCrossService1.tsbApplay.Enabled)
        {
          if (MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
          {
            this.ucCrossService1.SaveData();
            this.tsbCrossType.Checked = false;
            return;
          }
          this.ucCrossService1.LoadData();
          this.ucCrossService1.CancelEnabled();
        }
        else
        {
          this.ucCrossType1.LoadData();
          this.ucCrossService1.LoadData();
        }
        this.tsbCrossService.Checked = false;
        this.pnCrossType.BringToFront();
        this.pnCrossService.SendToBack();
      }
    }

    private void tsbCrossService_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 1, this.session.Get<Company>((object) this.company_id), true))
      {
        this.tsbCrossService.Checked = false;
      }
      else
      {
        if (this.ucCrossType1.tsbApplay.Enabled)
        {
          if (MessageBox.Show("Изменения не сохранены! Вернуться и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
          {
            this.ucCrossType1.SaveData();
            this.tsbCrossService.Checked = false;
            return;
          }
          this.ucCrossType1.LoadData();
          this.ucCrossType1.CancelEnabled();
        }
        else
        {
          this.ucCrossType1.LoadData();
          this.ucCrossService1.LoadData();
        }
        this.tsbCrossType.Checked = false;
        this.pnCrossService.BringToFront();
      }
    }

    private void ucCrossType1_ObjectsListChanged(object sender, EventArgs e)
    {
      IList<CrossType> crossTypeList = this.session.CreateCriteria(typeof (CrossType)).AddOrder(Order.Asc("CrossTypeName")).List<CrossType>();
      crossTypeList.Insert(0, new CrossType((short) 0, ""));
      ((DataGridViewComboBoxColumn) this.ucCrossService1.dgvBase.Columns["CrossType"]).DataSource = (object) crossTypeList;
    }

    private void ucCrossService1_Load(object sender, EventArgs e)
    {
    }

    private void cmbCompany_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (this.cmbCompany.SelectedItem == null)
        return;
      if (this.tsbCrossService.Checked && !KvrplHelper.CheckProxy(32, 1, (Company) this.cmbCompany.SelectedItem, true))
        this.cmbCompany.SelectedItem = (object) this.session.Get<Company>((object) this.Company_id);
      else
        this.ucCrossService1.Company_id = ((Company) this.cmbCompany.SelectedItem).CompanyId;
      if ((int) this.ucCrossService1.Company_id != (int) ((Company) this.cmbCompany.SelectedItem).CompanyId)
        this.cmbCompany.SelectedItem = (object) this.session.Get<Company>((object) this.ucCrossService1.Company_id);
    }

    private void cbArchive_CheckedChanged(object sender, EventArgs e)
    {
      this.ucCrossService1.IsArchive = this.cbArchive.Checked;
      this.ucCrossService1.LoadData();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnBtn = new Panel();
      this.bntExit = new Button();
      this.tsCrossService = new ToolStrip();
      this.tsbCrossType = new ToolStripButton();
      this.tsbCrossService = new ToolStripButton();
      this.pnCrossType = new Panel();
      this.ucCrossType1 = new UCCrossType();
      this.pnCrossService = new Panel();
      this.ucCrossService1 = new UCCrossService();
      this.pnUp = new Panel();
      this.cbArchive = new CheckBox();
      this.cmbCompany = new ComboBox();
      this.lblCompany = new Label();
      this.hp = new HelpProvider();
      this.pnBtn.SuspendLayout();
      this.tsCrossService.SuspendLayout();
      this.pnCrossType.SuspendLayout();
      this.pnCrossService.SuspendLayout();
      this.pnUp.SuspendLayout();
      this.SuspendLayout();
      this.pnBtn.BackColor = Color.Transparent;
      this.pnBtn.Controls.Add((Control) this.bntExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(131, 455);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(942, 40);
      this.pnBtn.TabIndex = 0;
      this.bntExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.bntExit.Image = (Image) Resources.Exit;
      this.bntExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.bntExit.Location = new Point(848, 5);
      this.bntExit.Name = "bntExit";
      this.bntExit.Size = new Size(82, 30);
      this.bntExit.TabIndex = 0;
      this.bntExit.Text = "Выход";
      this.bntExit.TextAlign = ContentAlignment.MiddleRight;
      this.bntExit.UseVisualStyleBackColor = true;
      this.bntExit.Click += new EventHandler(this.bntExit_Click);
      this.tsCrossService.Dock = DockStyle.Left;
      this.tsCrossService.ImageScalingSize = new Size(48, 48);
      this.tsCrossService.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsbCrossType,
        (ToolStripItem) this.tsbCrossService
      });
      this.tsCrossService.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
      this.tsCrossService.Location = new Point(0, 0);
      this.tsCrossService.Name = "tsCrossService";
      this.tsCrossService.Size = new Size(131, 495);
      this.tsCrossService.TabIndex = 1;
      this.tsCrossService.Text = "toolStrip1";
      this.tsbCrossType.Checked = true;
      this.tsbCrossType.CheckOnClick = true;
      this.tsbCrossType.CheckState = CheckState.Checked;
      this.tsbCrossType.Font = new Font("Tahoma", 10f);
      this.tsbCrossType.Image = (Image) Resources.allClasses;
      this.tsbCrossType.ImageTransparentColor = Color.Magenta;
      this.tsbCrossType.Name = "tsbCrossType";
      this.tsbCrossType.Size = new Size(128, 69);
      this.tsbCrossType.Text = "Типы связи";
      this.tsbCrossType.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tsbCrossType.Click += new EventHandler(this.tsbCrossType_Click);
      this.tsbCrossService.CheckOnClick = true;
      this.tsbCrossService.Font = new Font("Tahoma", 10f);
      this.tsbCrossService.Image = (Image) Resources.documents;
      this.tsbCrossService.ImageTransparentColor = Color.Magenta;
      this.tsbCrossService.Name = "tsbCrossService";
      this.tsbCrossService.Size = new Size(128, 69);
      this.tsbCrossService.Text = "Связанные услуги";
      this.tsbCrossService.TextImageRelation = TextImageRelation.ImageAboveText;
      this.tsbCrossService.Click += new EventHandler(this.tsbCrossService_Click);
      this.pnCrossType.Controls.Add((Control) this.ucCrossType1);
      this.pnCrossType.Controls.Add((Control) this.pnCrossService);
      this.pnCrossType.Dock = DockStyle.Fill;
      this.pnCrossType.Location = new Point(131, 0);
      this.pnCrossType.Name = "pnCrossType";
      this.pnCrossType.Size = new Size(942, 455);
      this.pnCrossType.TabIndex = 2;
      this.ucCrossType1.Dock = DockStyle.Fill;
      this.ucCrossType1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.ucCrossType1.Location = new Point(0, 0);
      this.ucCrossType1.Margin = new Padding(4);
      this.ucCrossType1.Name = "ucCrossType1";
      this.ucCrossType1.Size = new Size(942, 455);
      this.ucCrossType1.TabIndex = 0;
      this.ucCrossType1.CurObjectChanged += new EventHandler(this.ucCrossType1_ObjectsListChanged);
      this.pnCrossService.Controls.Add((Control) this.ucCrossService1);
      this.pnCrossService.Controls.Add((Control) this.pnUp);
      this.pnCrossService.Dock = DockStyle.Fill;
      this.pnCrossService.Location = new Point(0, 0);
      this.pnCrossService.Name = "pnCrossService";
      this.pnCrossService.Size = new Size(942, 455);
      this.pnCrossService.TabIndex = 1;
      this.ucCrossService1.Company_id = (short) 0;
      this.ucCrossService1.Dock = DockStyle.Fill;
      this.ucCrossService1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.ucCrossService1.Location = new Point(0, 43);
      this.ucCrossService1.Margin = new Padding(4);
      this.ucCrossService1.Name = "ucCrossService1";
      this.ucCrossService1.Size = new Size(942, 412);
      this.ucCrossService1.TabIndex = 1;
      this.ucCrossService1.Load += new EventHandler(this.ucCrossService1_Load);
      this.pnUp.Controls.Add((Control) this.cbArchive);
      this.pnUp.Controls.Add((Control) this.cmbCompany);
      this.pnUp.Controls.Add((Control) this.lblCompany);
      this.pnUp.Dock = DockStyle.Top;
      this.pnUp.Location = new Point(0, 0);
      this.pnUp.Name = "pnUp";
      this.pnUp.Size = new Size(942, 43);
      this.pnUp.TabIndex = 0;
      this.cbArchive.AutoSize = true;
      this.cbArchive.Location = new Point(848, 13);
      this.cbArchive.Name = "cbArchive";
      this.cbArchive.Size = new Size(66, 20);
      this.cbArchive.TabIndex = 2;
      this.cbArchive.Text = "Архив";
      this.cbArchive.UseVisualStyleBackColor = true;
      this.cbArchive.CheckedChanged += new EventHandler(this.cbArchive_CheckedChanged);
      this.cmbCompany.FormattingEnabled = true;
      this.cmbCompany.Location = new Point(81, 11);
      this.cmbCompany.Name = "cmbCompany";
      this.cmbCompany.Size = new Size(750, 24);
      this.cmbCompany.TabIndex = 1;
      this.cmbCompany.SelectionChangeCommitted += new EventHandler(this.cmbCompany_SelectionChangeCommitted);
      this.lblCompany.AutoSize = true;
      this.lblCompany.Location = new Point(3, 14);
      this.lblCompany.Name = "lblCompany";
      this.lblCompany.Size = new Size(72, 16);
      this.lblCompany.TabIndex = 0;
      this.lblCompany.Text = "Компания";
      this.hp.HelpNamespace = "Help.chm";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSize = true;
      this.ClientSize = new Size(1073, 495);
      this.Controls.Add((Control) this.pnCrossType);
      this.Controls.Add((Control) this.pnBtn);
      this.Controls.Add((Control) this.tsCrossService);
      this.hp.SetHelpKeyword((Control) this, "kv518.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      this.Margin = new Padding(5);
      this.Name = "FrmCrossService";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Связанные услуги";
      this.Load += new EventHandler(this.FrmCrossService_Load);
      this.pnBtn.ResumeLayout(false);
      this.tsCrossService.ResumeLayout(false);
      this.tsCrossService.PerformLayout();
      this.pnCrossType.ResumeLayout(false);
      this.pnCrossService.ResumeLayout(false);
      this.pnUp.ResumeLayout(false);
      this.pnUp.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
