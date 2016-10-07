// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmCloseScheme
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
  public class FrmCloseScheme : FrmBaseForm
  {
    private FormStateSaver fss = new FormStateSaver(FrmCloseScheme.container);
    protected GridSettings MySettingsListClients = new GridSettings();
    private IContainer components = (IContainer) null;
    private static IContainer container;
    private int level;
    private Company company;
    private Home home;
    private Service service;
    private ISession session;
    private Panel pnBtn;
    private Button btnExit;
    private Panel pnUp;
    private DataGridView dgvListClient;
    private Button btnStart;
    private Button btnCheck;
    private MonthPicker mpCurrentPeriod;
    private Label lblService;
    private ComboBox cmbService;

    public FrmCloseScheme()
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
    }

    public FrmCloseScheme(Company company, Home home, int level, Service service)
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
      this.company = company;
      this.home = home;
      this.service = service;
      this.level = level;
      this.MySettingsListClients.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void dgvListClient_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    private void dgvListClient_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsListClients.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsListClients.Columns[this.MySettingsListClients.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsListClients.Save();
    }

    private void mpCurrentPeriod_ValueChanged(object sender, EventArgs e)
    {
      Options.Period = KvrplHelper.SaveCurrentPeriod(this.mpCurrentPeriod.Value);
      this.LoadList();
    }

    private void FrmCloseScheme_Load(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      IList<Service> serviceList1 = (IList<Service>) new List<Service>();
      IList<Service> serviceList2 = this.session.CreateQuery(string.Format("select distinct s from Service s,ServiceParam sp where sp.Service_id=s.ServiceId and s.Root=0 and s.ServiceId<>0 and sp.Company_id={0} order by " + Options.SortService, (object) Options.Company.CompanyId)).List<Service>();
      serviceList2.Insert(0, new Service(Convert.ToInt16(0), ""));
      this.cmbService.DataSource = (object) serviceList2;
      this.cmbService.DisplayMember = "ServiceName";
      this.cmbService.ValueMember = "ServiceId";
      if ((uint) this.service.ServiceId > 0U)
        this.cmbService.SelectedValue = (object) this.service.ServiceId;
      this.MySettingsListClients.GridName = "ListClient";
      this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
    }

    private void LoadSettingsAudits()
    {
      this.MySettingsListClients.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvListClient.Columns)
        this.MySettingsListClients.GetMySettings(column);
    }

    private void LoadList()
    {
      try
      {
        this.session.Clear();
        this.dgvListClient.Columns.Clear();
        this.dgvListClient.DataSource = (object) null;
        int num = 2;
        IList<Counter> counterList = (IList<Counter>) new List<Counter>();
        IList<Audit> auditList1 = (IList<Audit>) new List<Audit>();
        string str1 = "";
        string str2 = "";
        if ((uint) ((Service) this.cmbService.SelectedItem).ServiceId > 0U)
          str1 = "and c.Service.ServiceId={5}";
        if (this.level == 3)
          str2 = string.Format("and h.IdHome={0}", (object) this.home.IdHome);
        IList<Audit> auditList2 = this.session.CreateQuery(string.Format("select a from Audit a,Counter c,LsClient ls, Home h where a.Counter=c and c.LsClient=ls and ls.Home=h and ls.Company.CompanyId={0} " + str2 + " and c.BaseCounter.Id={1} and c.Complex.ComplexId={2} and a.DEnd>='{3}' and a.DEnd<='{4}' and ls.ClientId in (select LsClient.ClientId from LsServiceParam where Period.PeriodId=0 and DBeg<=a.DEnd and DEnd>a.DEnd and Service.ServiceId=c.Service.ServiceId and Param.ParamId=405) " + str1 + " and ls.ClientId not in (select c1.LsClient.ClientId from Counter c1 where c1.Service.ServiceId=c.Service.ServiceId and c1.LsClient.ClientId=c.LsClient.ClientId and c1.BaseCounter.Id={1} and ArchivesDate is null and c1.CounterId not in (select Counter.CounterId from Audit where Period.PeriodId=0 and DEnd>='{3}' and DEnd<='{4}')) order by h.Str.NameStr,DBA.lengthhome(h.NHome),DBA.lengthhome(h.HomeKorp),DBA.lengthhome(ls.Flat.NFlat) ", (object) this.company.CompanyId, (object) num, (object) Options.Complex.ComplexId, (object) KvrplHelper.DateToBaseFormat(this.mpCurrentPeriod.Value), (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(this.mpCurrentPeriod.Value)), this.cmbService.SelectedValue)).List<Audit>();
        foreach (Audit audit in (IEnumerable<Audit>) auditList2)
          audit.Note = audit.Home.Address + audit.Counter.LsClient.SmallAddress.Substring(10) + " (" + audit.ClientId.Value.ToString() + ")";
        this.dgvListClient.DataSource = (object) auditList2;
        this.dgvListClient.Columns["UName"].Visible = false;
        this.dgvListClient.Columns["DEdit"].Visible = false;
        KvrplHelper.AddTextBoxColumn(this.dgvListClient, 0, "Счетчик", "CounterInfo", 100, true);
        KvrplHelper.AddMaskDateColumn(this.dgvListClient, 1, "Дата начала", "DBeg");
        KvrplHelper.AddMaskDateColumn(this.dgvListClient, 2, "Дата окончания", "DEnd");
        DataGridViewCheckBoxColumn viewCheckBoxColumn = new DataGridViewCheckBoxColumn();
        viewCheckBoxColumn.HeaderText = "Закрыть";
        viewCheckBoxColumn.Name = "Check";
        viewCheckBoxColumn.FlatStyle = FlatStyle.Standard;
        this.dgvListClient.Columns.Insert(0, (DataGridViewColumn) viewCheckBoxColumn);
        this.dgvListClient.Columns["Note"].DisplayIndex = 1;
        this.dgvListClient.Columns["Note"].HeaderText = "Адрес";
        foreach (DataGridViewRow row in (IEnumerable) this.dgvListClient.Rows)
        {
          row.Cells["DEnd"].Value = (object) ((Audit) row.DataBoundItem).DEnd;
          row.Cells["DBeg"].Value = (object) ((Audit) row.DataBoundItem).DBeg;
          if (((Audit) row.DataBoundItem).Counter != null)
            row.Cells["CounterInfo"].Value = (object) ((Audit) row.DataBoundItem).Counter.AllInfo;
        }
        this.LoadSettingsAudits();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void cmbService_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.LoadList();
    }

    private void btnCheck_Click(object sender, EventArgs e)
    {
      KvrplHelper.CheckAll(this.dgvListClient, this.btnCheck);
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
      if (this.dgvListClient.Rows.Count <= 0 || MessageBox.Show("Закрыть тип расчета по выбранным счетчикам?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      bool flag = true;
      if (Options.Period.PeriodId <= KvrplHelper.GetCmpKvrClose(this.company, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk).PeriodId)
      {
        int num1 = (int) MessageBox.Show("Невозможно изменить записи в закрытом месяце", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        foreach (DataGridViewRow row in (IEnumerable) this.dgvListClient.Rows)
        {
          if (Convert.ToBoolean(row.Cells["Check"].Value))
          {
            try
            {
              Audit dataBoundItem = (Audit) row.DataBoundItem;
              this.session.CreateQuery("update LsServiceParam set DEnd=:dend where LsClient.ClientId=:id and Period.PeriodId=0 and DBeg<=:dend and DEnd>=:dend and Service.ServiceId=:serv and Param.ParamId=405").SetInt32("id", dataBoundItem.ClientId.Value).SetDateTime("dend", dataBoundItem.DEnd).SetParameter<short>("serv", dataBoundItem.Counter.Service.ServiceId).ExecuteUpdate();
            }
            catch (Exception ex)
            {
              flag = false;
              KvrplHelper.WriteLog(ex, (LsClient) null);
            }
          }
        }
        if (flag)
        {
          int num2 = (int) MessageBox.Show("Операция завершилась успешно", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          int num3 = (int) MessageBox.Show("Операция прошла с ошибками", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        this.LoadList();
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
      this.btnExit = new Button();
      this.pnUp = new Panel();
      this.dgvListClient = new DataGridView();
      this.btnStart = new Button();
      this.btnCheck = new Button();
      this.mpCurrentPeriod = new MonthPicker();
      this.lblService = new Label();
      this.cmbService = new ComboBox();
      this.pnBtn.SuspendLayout();
      this.pnUp.SuspendLayout();
      ((ISupportInitialize) this.dgvListClient).BeginInit();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnStart);
      this.pnBtn.Controls.Add((Control) this.btnCheck);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 377);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(765, 40);
      this.pnBtn.TabIndex = 0;
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(671, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(82, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.pnUp.Controls.Add((Control) this.lblService);
      this.pnUp.Controls.Add((Control) this.cmbService);
      this.pnUp.Controls.Add((Control) this.mpCurrentPeriod);
      this.pnUp.Dock = DockStyle.Top;
      this.pnUp.Location = new Point(0, 0);
      this.pnUp.Name = "pnUp";
      this.pnUp.Size = new Size(765, 42);
      this.pnUp.TabIndex = 1;
      this.dgvListClient.BackgroundColor = Color.AliceBlue;
      this.dgvListClient.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvListClient.Dock = DockStyle.Fill;
      this.dgvListClient.Location = new Point(0, 42);
      this.dgvListClient.Name = "dgvListClient";
      this.dgvListClient.Size = new Size(765, 335);
      this.dgvListClient.TabIndex = 2;
      this.dgvListClient.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvListClient_ColumnWidthChanged);
      this.dgvListClient.DataError += new DataGridViewDataErrorEventHandler(this.dgvListClient_DataError);
      this.btnStart.Image = (Image) Resources.Configure;
      this.btnStart.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnStart.Location = new Point(154, 5);
      this.btnStart.Name = "btnStart";
      this.btnStart.Size = new Size(111, 30);
      this.btnStart.TabIndex = 6;
      this.btnStart.Text = "Выполнить";
      this.btnStart.TextAlign = ContentAlignment.MiddleRight;
      this.btnStart.UseVisualStyleBackColor = true;
      this.btnStart.Click += new EventHandler(this.btnStart_Click);
      this.btnCheck.Image = (Image) Resources.properties;
      this.btnCheck.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCheck.Location = new Point(12, 5);
      this.btnCheck.Name = "btnCheck";
      this.btnCheck.Size = new Size(135, 30);
      this.btnCheck.TabIndex = 5;
      this.btnCheck.Text = "Выделить все";
      this.btnCheck.TextAlign = ContentAlignment.MiddleRight;
      this.btnCheck.UseVisualStyleBackColor = true;
      this.btnCheck.Click += new EventHandler(this.btnCheck_Click);
      this.mpCurrentPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.mpCurrentPeriod.CustomFormat = "MMMM yyyy";
      this.mpCurrentPeriod.Format = DateTimePickerFormat.Custom;
      this.mpCurrentPeriod.Location = new Point(613, 12);
      this.mpCurrentPeriod.Name = "mpCurrentPeriod";
      this.mpCurrentPeriod.OldMonth = 0;
      this.mpCurrentPeriod.ShowUpDown = true;
      this.mpCurrentPeriod.Size = new Size(140, 22);
      this.mpCurrentPeriod.TabIndex = 0;
      this.mpCurrentPeriod.ValueChanged += new EventHandler(this.mpCurrentPeriod_ValueChanged);
      this.lblService.AutoSize = true;
      this.lblService.Location = new Point(10, 14);
      this.lblService.Name = "lblService";
      this.lblService.Size = new Size(54, 16);
      this.lblService.TabIndex = 6;
      this.lblService.Text = "Услуга";
      this.cmbService.FormattingEnabled = true;
      this.cmbService.Location = new Point(73, 10);
      this.cmbService.Name = "cmbService";
      this.cmbService.Size = new Size(192, 24);
      this.cmbService.TabIndex = 5;
      this.cmbService.SelectionChangeCommitted += new EventHandler(this.cmbService_SelectionChangeCommitted);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(765, 417);
      this.Controls.Add((Control) this.dgvListClient);
      this.Controls.Add((Control) this.pnUp);
      this.Controls.Add((Control) this.pnBtn);
      this.Name = "FrmCloseScheme";
      this.Text = "Массовое закрытие типов расчета";
      this.Load += new EventHandler(this.FrmCloseScheme_Load);
      this.pnBtn.ResumeLayout(false);
      this.pnUp.ResumeLayout(false);
      this.pnUp.PerformLayout();
      ((ISupportInitialize) this.dgvListClient).EndInit();
      this.ResumeLayout(false);
    }
  }
}
