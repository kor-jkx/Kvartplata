// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmAudit
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using NHibernate;
using SaveSettings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmAudit : FrmBaseForm
  {
    private FormStateSaver fss = new FormStateSaver(FrmAudit.container);
    protected GridSettings MySettingsAudits = new GridSettings();
    private IContainer components = (IContainer) null;
    private static IContainer container;
    private short level;
    private int typeCounter;
    private Company company;
    private Home home;
    private Service service;
    private ISession session;
    private Panel pnBtn;
    private Button btnExit;
    private Panel pnUp;
    private Label lblService;
    private ComboBox cmbService;
    private MonthPicker mpCurrentPeriod;
    private Button btnCheck;
    private DataGridView dgvAudit;
    private Label lblDEnd;
    private Label lblDBeg;
    private DateTimePicker dtpDEnd;
    private DateTimePicker dtpDBeg;
    private Button btnScheme;
    private Label lblScheme;
    private Button btnStart;
    private CheckBox cbDates;

    public FrmAudit()
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
    }

    public FrmAudit(Company company, Home home, short level, int type, Service service)
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
      this.company = company;
      this.home = home;
      this.level = level;
      this.typeCounter = type;
      this.service = service;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void FrmAudit_Load(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      this.session.Clear();
      IList<Service> serviceList1 = (IList<Service>) new List<Service>();
      IList<Service> serviceList2 = this.session.CreateQuery(string.Format("select distinct s from Service s,ServiceParam sp where sp.Service_id=s.ServiceId and s.Root=0 and s.ServiceId<>0 and sp.Company_id={0} order by " + Options.SortService, (object) Options.Company.CompanyId)).List<Service>();
      serviceList2.Insert(0, new Service(Convert.ToInt16(0), ""));
      this.cmbService.DataSource = (object) serviceList2;
      this.cmbService.DisplayMember = "ServiceName";
      this.cmbService.ValueMember = "ServiceId";
      if ((uint) this.service.ServiceId > 0U)
        this.cmbService.SelectedValue = (object) this.service.ServiceId;
      this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
      this.session.Clear();
    }

    private void LoadAudit()
    {
      this.session.Clear();
      this.session = Domain.CurrentSession;
      IList<Counter> counterList = (IList<Counter>) new List<Counter>();
      string str = "";
      if ((uint) ((Service) this.cmbService.SelectedItem).ServiceId > 0U)
        str = "and c.Service.ServiceId={5}";
      try
      {
        if ((int) this.level == 2)
        {
          if (this.typeCounter != 2)
            counterList = this.session.CreateQuery(string.Format("select new Counter(c.CounterId,c.CounterNum,c.LsClient,c.Home,c.Service,c.AuditDate,(select max(e.DEnd) from Evidence e where e.Counter=c)) from Counter c, Home h, Str s, HomeLink hl where c.Home=h and h.Str=s and hl.Home=h and hl.Company.CompanyId={0} and c.BaseCounter.Id={1} and c.Complex.ComplexId={2} and c.AuditDate>='{3}' and c.AuditDate<='{4}' and isnull(c.ArchivesDate,'2999-12-31')>='{4}'" + str + " and isnull((select max(DBeg) from Audit where Period.PeriodId=0 and Counter.CounterId=c.CounterId),'2000-01-01')<'{3}' order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)", (object) this.company.CompanyId, (object) this.typeCounter, (object) Options.Complex.ComplexId, (object) KvrplHelper.DateToBaseFormat(this.mpCurrentPeriod.Value), (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(this.mpCurrentPeriod.Value)), this.cmbService.SelectedValue)).List<Counter>();
          else
            counterList = this.session.CreateQuery(string.Format("select new Counter(c.CounterId,c.CounterNum,c.LsClient,c.Home,c.Service,c.AuditDate,(select max(e.DEnd) from Evidence e where e.Counter=c) ) from Counter c, LsClient ls where c.LsClient=ls and ls.Company.CompanyId={0} and c.BaseCounter.Id={1} and c.Complex.ComplexId={2} and c.AuditDate>='{3}' and c.AuditDate<='{4}' and isnull(c.ArchivesDate,'2999-12-31')>='{4}'" + str + " and isnull((select max(DBeg) from Audit where Period.PeriodId=0 and Counter.CounterId=c.CounterId),'2000-01-01')<'{3}' order by DBA.lengthhome(ls.Flat.NFlat)", (object) this.company.CompanyId, (object) this.typeCounter, (object) Options.Complex.ComplexId, (object) KvrplHelper.DateToBaseFormat(this.mpCurrentPeriod.Value), (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(this.mpCurrentPeriod.Value)), this.cmbService.SelectedValue)).List<Counter>();
        }
        if ((int) this.level == 3)
        {
          if (this.typeCounter != 2)
            counterList = this.session.CreateQuery(string.Format("select new Counter(c.CounterId,c.CounterNum,c.LsClient,c.Home,c.Service,c.AuditDate,(select max(e.DEnd) from Evidence e where e.Counter=c)) from Counter c, Home h, Str s, HomeLink hl where c.Home=h and h.Str=s and hl.Home=h and hl.Company.CompanyId={0} and c.Home.IdHome={6} and c.BaseCounter.Id={1} and c.Complex.ComplexId={2} and c.AuditDate>='{3}' and c.AuditDate<='{4}' and isnull(c.ArchivesDate,'2999-12-31')>='{4}' " + str + " and isnull((select max(DBeg) from Audit where Period.PeriodId=0 and Counter.CounterId=c.CounterId),'2000-01-01')<'{3}' order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)", (object) this.company.CompanyId, (object) this.typeCounter, (object) Options.Complex.ComplexId, (object) KvrplHelper.DateToBaseFormat(this.mpCurrentPeriod.Value), (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(this.mpCurrentPeriod.Value)), this.cmbService.SelectedValue, (object) this.home.IdHome)).List<Counter>();
          else
            counterList = this.session.CreateQuery(string.Format("select new Counter(c.CounterId,c.CounterNum,c.LsClient,c.Home,c.Service,c.AuditDate,(select max(e.DEnd) from Evidence e where e.Counter=c)) from Counter c, LsClient ls where c.LsClient=ls and ls.Company.CompanyId={0} and c.Home.IdHome={6} and c.BaseCounter.Id={1} and c.Complex.ComplexId={2} and c.AuditDate>='{3}' and c.AuditDate<='{4}' and isnull(c.ArchivesDate,'2999-12-31')>='{4}' " + str + " and isnull((select max(DBeg) from Audit where Period.PeriodId=0 and Counter.CounterId=c.CounterId),'2000-01-01')<'{3}' order by DBA.lengthhome(ls.Flat.NFlat)", (object) this.company.CompanyId, (object) this.typeCounter, (object) Options.Complex.ComplexId, (object) KvrplHelper.DateToBaseFormat(this.mpCurrentPeriod.Value), (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(this.mpCurrentPeriod.Value)), this.cmbService.SelectedValue, (object) this.home.IdHome)).List<Counter>();
        }
      }
      catch
      {
      }
      foreach (Counter counter in (IEnumerable<Counter>) counterList)
        counter.Notice = this.typeCounter == 2 ? counter.FlatAndNum : counter.AdrAndNum;
      this.dgvAudit.DataSource = (object) counterList;
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvAudit.Columns)
      {
        if (column.Name != "Notice" && column.Name != "AuditDate")
          column.Visible = false;
      }
      this.dgvAudit.Columns["Notice"].HeaderText = "Счетчик";
      this.dgvAudit.Columns["AuditDate"].HeaderText = "Дата поверки";
      KvrplHelper.AddMaskDateColumn(this.dgvAudit, 0, "Дата начала", "DBeg");
      KvrplHelper.AddMaskDateColumn(this.dgvAudit, 1, "Дата окончания", "DEnd");
      KvrplHelper.AddButtonColumn(this.dgvAudit, 2, "Схема", "Scheme", 80);
      DataGridViewCheckBoxColumn viewCheckBoxColumn = new DataGridViewCheckBoxColumn();
      viewCheckBoxColumn.HeaderText = "Вставить";
      viewCheckBoxColumn.Name = "Check";
      viewCheckBoxColumn.FlatStyle = FlatStyle.Standard;
      this.dgvAudit.Columns.Insert(0, (DataGridViewColumn) viewCheckBoxColumn);
      this.dgvAudit.Columns["Notice"].DisplayIndex = 1;
      this.dgvAudit.Columns["AuditDate"].DisplayIndex = 2;
      this.dgvAudit.Columns["AuditDate"].ReadOnly = true;
      this.dgvAudit.Columns["Notice"].ReadOnly = true;
      foreach (DataGridViewRow row in (IEnumerable) this.dgvAudit.Rows)
      {
        DateTime? dopDate = ((Counter) row.DataBoundItem).DopDate;
        DateTime? nullable;
        int num;
        if (dopDate.HasValue)
        {
          dopDate = ((Counter) row.DataBoundItem).DopDate;
          nullable = ((Counter) row.DataBoundItem).AuditDate;
          if ((dopDate.HasValue & nullable.HasValue ? (dopDate.GetValueOrDefault() > nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0)
          {
            nullable = ((Counter) row.DataBoundItem).DopDate;
            DateTime dateTime = KvrplHelper.LastDay(this.mpCurrentPeriod.Value);
            num = nullable.HasValue ? (nullable.GetValueOrDefault() <= dateTime ? 1 : 0) : 0;
            goto label_36;
          }
        }
        num = 0;
label_36:
        if (num != 0)
        {
          DataGridViewCell cell1 = row.Cells["DBeg"];
          nullable = ((Counter) row.DataBoundItem).DopDate;
          // ISSUE: variable of a boxed type
          DateTime local1 = nullable.Value.AddDays(1.0);
          cell1.Value = (object) local1;
          DataGridViewCell cell2 = row.Cells["DEnd"];
          nullable = ((Counter) row.DataBoundItem).DopDate;
          // ISSUE: variable of a boxed type
          DateTime local2 = nullable.Value.AddDays(1.0).AddMonths(1);
          cell2.Value = (object) local2;
        }
        else
        {
          DataGridViewCell cell1 = row.Cells["DEnd"];
          nullable = ((Counter) row.DataBoundItem).AuditDate;
          DateTime dateTime = nullable.Value;
          dateTime = dateTime.AddMonths(3);
          // ISSUE: variable of a boxed type
          DateTime local1 = dateTime.AddDays(-1.0);
          cell1.Value = (object) local1;
          DataGridViewCell cell2 = row.Cells["DBeg"];
          nullable = ((Counter) row.DataBoundItem).AuditDate;
          // ISSUE: variable of a boxed type
          DateTime local2 = nullable.Value;
          cell2.Value = (object) local2;
        }
      }
      this.MySettingsAudits.GridName = "Audits";
      this.LoadSettingsAudits();
    }

    private void LoadSettingsAudits()
    {
      this.MySettingsAudits.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvAudit.Columns)
        this.MySettingsAudits.GetMySettings(column);
    }

    private void cmbService_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.LoadAudit();
    }

    private void mpCurrentPeriod_ValueChanged(object sender, EventArgs e)
    {
      Options.Period = KvrplHelper.SaveCurrentPeriod(this.mpCurrentPeriod.Value);
      this.LoadAudit();
    }

    private void btnCheck_Click(object sender, EventArgs e)
    {
      KvrplHelper.CheckAll(this.dgvAudit, this.btnCheck);
    }

    private void dtpDBeg_ValueChanged(object sender, EventArgs e)
    {
      foreach (DataGridViewRow row in (IEnumerable) this.dgvAudit.Rows)
        row.Cells["DBeg"].Value = (object) this.dtpDBeg.Value;
    }

    private void dtpDEnd_ValueChanged(object sender, EventArgs e)
    {
      foreach (DataGridViewRow row in (IEnumerable) this.dgvAudit.Rows)
        row.Cells["DEnd"].Value = (object) this.dtpDEnd.Value;
    }

    private void btnScheme_Click(object sender, EventArgs e)
    {
      short id = 0;
      FrmScheme frmScheme = new FrmScheme((short) 4, id);
      if (frmScheme.ShowDialog() == DialogResult.OK)
        id = frmScheme.CurrentId();
      this.btnScheme.Text = id.ToString();
      frmScheme.Dispose();
      foreach (DataGridViewRow row in (IEnumerable) this.dgvAudit.Rows)
        row.Cells["Scheme"].Value = (object) id;
    }

    private void dgvAudit_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      short id = 0;
      if (e.ColumnIndex <= 0 || e.RowIndex < 0 || !(this.dgvAudit.Columns[e.ColumnIndex].Name == "Scheme"))
        return;
      if (this.dgvAudit.CurrentRow.Cells["Scheme"].Value != null)
        id = Convert.ToInt16(this.dgvAudit.CurrentRow.Cells["Scheme"].Value);
      FrmScheme frmScheme = new FrmScheme((short) 4, id);
      if (frmScheme.ShowDialog() == DialogResult.OK)
        id = frmScheme.CurrentId();
      this.dgvAudit.CurrentRow.Cells["Scheme"].Value = (object) id;
      frmScheme.Dispose();
    }

    private void dgvAudit_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
      bool flag1 = true;
      foreach (DataGridViewRow row in (IEnumerable) this.dgvAudit.Rows)
      {
        if (Convert.ToBoolean(row.Cells["Check"].Value))
        {
          bool flag2 = false;
          if (Convert.ToDateTime(row.Cells["DEnd"].Value) < Convert.ToDateTime(row.Cells["DBeg"].Value))
          {
            if (MessageBox.Show("У счетчика  " + row.Cells["Notice"].Value + " дата начала больше даты окончания. Запись не будет вставлена. Продолжить операцию?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand) == DialogResult.Cancel)
              return;
            flag2 = true;
          }
          if (Convert.ToDateTime(row.Cells["Dbeg"].Value) < KvrplHelper.GetCmpKvrClose(this.company, Options.Complex.ComplexId, Options.ComplexPrior.ComplexId).PeriodName.Value)
          {
            if (MessageBox.Show("У счетчика  " + row.Cells["Notice"].Value + " дата начала принадлежит закрытому периоду. Запись не будет вставлена. Продолжить операцию?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand) == DialogResult.Cancel)
              return;
            flag2 = true;
          }
          if (row.Cells["Scheme"].Value == null || (int) Convert.ToInt16(row.Cells["Scheme"].Value) == 0)
          {
            if (MessageBox.Show("У счетчика  " + row.Cells["Notice"].Value + " схема не проставлена либо проставлена схема 0. Запись не будет вставлена. Продолжить операцию?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand) == DialogResult.Cancel)
              return;
            flag2 = true;
          }
          if (!flag2)
          {
            this.session = Domain.CurrentSession;
            this.session.Clear();
            Audit audit = new Audit();
            audit.Period = this.session.Get<Period>((object) 0);
            audit.Counter = (Counter) row.DataBoundItem;
            audit.DBeg = Convert.ToDateTime(row.Cells["DBeg"].Value);
            audit.DEnd = Convert.ToDateTime(row.Cells["DEnd"].Value);
            audit.Scheme = Convert.ToInt16(row.Cells["Scheme"].Value);
            audit.Note = "Запись занесена при массовом занесении поверок по дате поверки " + row.Cells["AuditDate"].Value.ToString();
            audit.UName = Options.Login;
            audit.DEdit = DateTime.Now;
            try
            {
              this.session.Save((object) audit);
              this.session.Flush();
            }
            catch (Exception ex)
            {
              flag1 = false;
              KvrplHelper.WriteLog(ex, (LsClient) null);
            }
          }
        }
      }
      if (flag1)
      {
        int num1 = (int) MessageBox.Show("Операция завершилась успешно", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        int num2 = (int) MessageBox.Show("Операция прошла с ошибками", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void cbDates_CheckedChanged(object sender, EventArgs e)
    {
      if (this.cbDates.Checked)
      {
        this.lblDBeg.Enabled = true;
        this.lblDEnd.Enabled = true;
        this.dtpDBeg.Enabled = true;
        this.dtpDEnd.Enabled = true;
      }
      else
      {
        this.lblDBeg.Enabled = false;
        this.lblDEnd.Enabled = false;
        this.dtpDBeg.Enabled = false;
        this.dtpDEnd.Enabled = false;
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
      this.pnBtn = new Panel();
      this.btnStart = new Button();
      this.btnCheck = new Button();
      this.btnExit = new Button();
      this.pnUp = new Panel();
      this.cbDates = new CheckBox();
      this.btnScheme = new Button();
      this.lblScheme = new Label();
      this.lblDEnd = new Label();
      this.lblDBeg = new Label();
      this.dtpDEnd = new DateTimePicker();
      this.dtpDBeg = new DateTimePicker();
      this.mpCurrentPeriod = new MonthPicker();
      this.lblService = new Label();
      this.cmbService = new ComboBox();
      this.dgvAudit = new DataGridView();
      this.pnBtn.SuspendLayout();
      this.pnUp.SuspendLayout();
      ((ISupportInitialize) this.dgvAudit).BeginInit();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnStart);
      this.pnBtn.Controls.Add((Control) this.btnCheck);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 383);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(913, 40);
      this.pnBtn.TabIndex = 0;
      this.btnStart.Image = (Image) Resources.Configure;
      this.btnStart.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnStart.Location = new Point(152, 5);
      this.btnStart.Name = "btnStart";
      this.btnStart.Size = new Size(111, 30);
      this.btnStart.TabIndex = 4;
      this.btnStart.Text = "Выполнить";
      this.btnStart.TextAlign = ContentAlignment.MiddleRight;
      this.btnStart.UseVisualStyleBackColor = true;
      this.btnStart.Click += new EventHandler(this.btnStart_Click);
      this.btnCheck.Image = (Image) Resources.properties;
      this.btnCheck.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCheck.Location = new Point(10, 5);
      this.btnCheck.Name = "btnCheck";
      this.btnCheck.Size = new Size(135, 30);
      this.btnCheck.TabIndex = 3;
      this.btnCheck.Text = "Выделить все";
      this.btnCheck.TextAlign = ContentAlignment.MiddleRight;
      this.btnCheck.UseVisualStyleBackColor = true;
      this.btnCheck.Click += new EventHandler(this.btnCheck_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(821, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(80, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.pnUp.Controls.Add((Control) this.cbDates);
      this.pnUp.Controls.Add((Control) this.btnScheme);
      this.pnUp.Controls.Add((Control) this.lblScheme);
      this.pnUp.Controls.Add((Control) this.lblDEnd);
      this.pnUp.Controls.Add((Control) this.lblDBeg);
      this.pnUp.Controls.Add((Control) this.dtpDEnd);
      this.pnUp.Controls.Add((Control) this.dtpDBeg);
      this.pnUp.Controls.Add((Control) this.mpCurrentPeriod);
      this.pnUp.Controls.Add((Control) this.lblService);
      this.pnUp.Controls.Add((Control) this.cmbService);
      this.pnUp.Dock = DockStyle.Top;
      this.pnUp.Location = new Point(0, 0);
      this.pnUp.Name = "pnUp";
      this.pnUp.Size = new Size(913, 61);
      this.pnUp.TabIndex = 1;
      this.cbDates.AutoSize = true;
      this.cbDates.Location = new Point(11, 35);
      this.cbDates.Name = "cbDates";
      this.cbDates.Size = new Size(133, 20);
      this.cbDates.TabIndex = 12;
      this.cbDates.Text = "Исправить даты";
      this.cbDates.UseVisualStyleBackColor = true;
      this.cbDates.CheckedChanged += new EventHandler(this.cbDates_CheckedChanged);
      this.btnScheme.Location = new Point(339, 4);
      this.btnScheme.Name = "btnScheme";
      this.btnScheme.Size = new Size(78, 23);
      this.btnScheme.TabIndex = 11;
      this.btnScheme.UseVisualStyleBackColor = true;
      this.btnScheme.Click += new EventHandler(this.btnScheme_Click);
      this.lblScheme.AutoSize = true;
      this.lblScheme.Location = new Point(285, 7);
      this.lblScheme.Name = "lblScheme";
      this.lblScheme.Size = new Size(48, 16);
      this.lblScheme.TabIndex = 10;
      this.lblScheme.Text = "Схема";
      this.lblDEnd.AutoSize = true;
      this.lblDEnd.Enabled = false;
      this.lblDEnd.Location = new Point(425, 36);
      this.lblDEnd.Name = "lblDEnd";
      this.lblDEnd.Size = new Size(113, 16);
      this.lblDEnd.TabIndex = 9;
      this.lblDEnd.Text = "Дата окончания";
      this.lblDBeg.AutoSize = true;
      this.lblDBeg.Enabled = false;
      this.lblDBeg.Location = new Point(160, 36);
      this.lblDBeg.Name = "lblDBeg";
      this.lblDBeg.Size = new Size(91, 16);
      this.lblDBeg.TabIndex = 8;
      this.lblDBeg.Text = "Дата начала";
      this.dtpDEnd.Enabled = false;
      this.dtpDEnd.Location = new Point(544, 33);
      this.dtpDEnd.Name = "dtpDEnd";
      this.dtpDEnd.Size = new Size(148, 22);
      this.dtpDEnd.TabIndex = 7;
      this.dtpDEnd.ValueChanged += new EventHandler(this.dtpDEnd_ValueChanged);
      this.dtpDBeg.Enabled = false;
      this.dtpDBeg.Location = new Point(257, 33);
      this.dtpDBeg.Name = "dtpDBeg";
      this.dtpDBeg.Size = new Size(149, 22);
      this.dtpDBeg.TabIndex = 6;
      this.dtpDBeg.ValueChanged += new EventHandler(this.dtpDBeg_ValueChanged);
      this.mpCurrentPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.mpCurrentPeriod.CustomFormat = "MMMM yyyy";
      this.mpCurrentPeriod.Format = DateTimePickerFormat.Custom;
      this.mpCurrentPeriod.Location = new Point(761, 3);
      this.mpCurrentPeriod.Name = "mpCurrentPeriod";
      this.mpCurrentPeriod.OldMonth = 0;
      this.mpCurrentPeriod.ShowUpDown = true;
      this.mpCurrentPeriod.Size = new Size(140, 22);
      this.mpCurrentPeriod.TabIndex = 5;
      this.mpCurrentPeriod.ValueChanged += new EventHandler(this.mpCurrentPeriod_ValueChanged);
      this.lblService.AutoSize = true;
      this.lblService.Location = new Point(8, 7);
      this.lblService.Name = "lblService";
      this.lblService.Size = new Size(54, 16);
      this.lblService.TabIndex = 4;
      this.lblService.Text = "Услуга";
      this.cmbService.FormattingEnabled = true;
      this.cmbService.Location = new Point(71, 3);
      this.cmbService.Name = "cmbService";
      this.cmbService.Size = new Size(192, 24);
      this.cmbService.TabIndex = 3;
      this.cmbService.SelectionChangeCommitted += new EventHandler(this.cmbService_SelectionChangeCommitted);
      this.dgvAudit.BackgroundColor = Color.AliceBlue;
      this.dgvAudit.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvAudit.Dock = DockStyle.Fill;
      this.dgvAudit.Location = new Point(0, 61);
      this.dgvAudit.Name = "dgvAudit";
      this.dgvAudit.Size = new Size(913, 322);
      this.dgvAudit.TabIndex = 3;
      this.dgvAudit.CellClick += new DataGridViewCellEventHandler(this.dgvAudit_CellClick);
      this.dgvAudit.DataError += new DataGridViewDataErrorEventHandler(this.dgvAudit_DataError);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(913, 423);
      this.Controls.Add((Control) this.dgvAudit);
      this.Controls.Add((Control) this.pnUp);
      this.Controls.Add((Control) this.pnBtn);
      this.Name = "FrmAudit";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Массовый ввод поверок";
      this.Load += new EventHandler(this.FrmAudit_Load);
      this.pnBtn.ResumeLayout(false);
      this.pnUp.ResumeLayout(false);
      this.pnUp.PerformLayout();
      ((ISupportInitialize) this.dgvAudit).EndInit();
      this.ResumeLayout(false);
    }
  }
}
