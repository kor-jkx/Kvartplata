// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmDetailCorrectRent
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
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
  public class FrmDetailCorrectRent : FrmBaseForm
  {
    private FormStateSaver fss = new FormStateSaver(FrmDetailCorrectRent.ic);
    protected GridSettings MySettingsDetailCorrect = new GridSettings();
    protected GridSettings MySettingsNB = new GridSettings();
    private IContainer components = (IContainer) null;
    private Service service;
    private ISession session;
    private LsClient client;
    private static IContainer ic;
    private Panel pnBtn;
    private Button btnExit;
    private Panel pnUp;
    private DataGridView dgvNoteBook;
    private Label lblNoteBook;
    private DataGridView dgvDetail;
    private MonthPicker mpCurrentPeriod;

    public FrmDetailCorrectRent()
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
    }

    public FrmDetailCorrectRent(LsClient client, Service service)
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
      this.service = service;
      this.client = client;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void FrmDetailCorrectRent_Load(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      this.MySettingsDetailCorrect.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
      this.MySettingsNB.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
      this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
    }

    private void LoadDetailCorrect()
    {
      IList list = this.session.CreateSQLQuery("select distinct (select service_name from dcService where service_id=:service) as name, (select period_value from dcPeriod where period_id=month_id) as month,       (select sum(rent) from lsRent where period_id=r.period_id and month_id=r.month_id and client_id=r.client_id and service_id in (select service_id from dcService where root=:service) and code in (-1,-2,1,11)) as code1,       (select sum(rent) from lsRent where period_id=r.period_id and month_id=r.month_id and client_id=r.client_id and service_id in (select service_id from dcService where root=:service) and code in (2,12)) as code2,       (select sum(rent) from lsRent where period_id=r.period_id and month_id=r.month_id and client_id=r.client_id and service_id in (select service_id from dcService where root=:service) and code in (3,13)) as code3,       (select sum(rent) from lsRent where period_id=r.period_id and month_id=r.month_id and client_id=r.client_id and service_id in (select service_id from dcService where root=:service) and code in (4,7)) as code4,       (select sum(rent) from lsRent where period_id=r.period_id and month_id=r.month_id and client_id=r.client_id and service_id in (select service_id from dcService where root=:service) and code in (5,6)) as code5,       (select sum(rent) from lsRent where period_id=r.period_id and month_id=r.month_id and client_id=r.client_id and service_id in (select service_id from dcService where root=:service) and code<>0) as codeall        from lsRent r where period_id=:period and client_id=:client and service_id in (select service_id from dcService where root=:service) and code<>0 ").SetParameter<int>("period", Options.Period.PeriodId).SetParameter<LsClient>("client", this.client).SetParameter<short>("service", this.service.ServiceId).List();
      DataTable dataTable = new DataTable("Detail");
      dataTable.Columns.Add("Услуга", System.Type.GetType("System.String"));
      dataTable.Columns.Add("За месяц", System.Type.GetType("System.DateTime"));
      dataTable.Columns.Add("Временная регистрация (изменение тарифа)", System.Type.GetType("System.Double"));
      dataTable.Columns.Add("Отсутствие", System.Type.GetType("System.Double"));
      dataTable.Columns.Add("Снятие по актам", System.Type.GetType("System.Double"));
      dataTable.Columns.Add("Ручные изменения", System.Type.GetType("System.Double"));
      dataTable.Columns.Add("Компенсация", System.Type.GetType("System.Double"));
      dataTable.Columns.Add("Всего", System.Type.GetType("System.Double"));
      foreach (object[] objArray in (IEnumerable) list)
        dataTable.Rows.Add(objArray);
      this.dgvDetail.Columns.Clear();
      this.dgvDetail.DataSource = (object) null;
      this.dgvDetail.DataSource = (object) dataTable;
      this.dgvDetail.ReadOnly = true;
      this.dgvDetail.Focus();
      this.MySettingsDetailCorrect.GridName = "DetailCorrectRent";
      IList<NoteBook> noteBookList = this.session.CreateQuery("from NoteBook where (ClientId=:client or (ClientId=0 and Idhome=0 and Company.CompanyId=:company)) and DBeg>=:dbeg and DBeg<=:dend").SetParameter<int>("client", this.client.ClientId).SetDateTime("dend", KvrplHelper.LastDay(Options.Period.PeriodName.Value)).SetParameter<DateTime>("dbeg", Options.Period.PeriodName.Value).SetParameter<short>("company", this.client.Company.CompanyId).List<NoteBook>();
      this.dgvNoteBook.Columns.Clear();
      this.dgvNoteBook.DataSource = (object) null;
      if (!KvrplHelper.CheckProxy(48, 1, this.client.Company, false))
      {
        foreach (NoteBook noteBook in (IEnumerable<NoteBook>) noteBookList)
        {
          IList<Person> personList = this.session.CreateCriteria(typeof (Person)).Add((ICriterion) Restrictions.Eq("LsClient", (object) this.client)).Add((ICriterion) Restrictions.In("Reg.RegId", (ICollection) new int[2]{ 1, 2 })).Add((ICriterion) Restrictions.Or((ICriterion) Restrictions.Lt("Archive", (object) 3), (ICriterion) Restrictions.Eq("Archive", (object) 5))).AddOrder(Order.Asc("Archive")).AddOrder(Order.Asc("Relation.RelationId")).List<Person>();
          foreach (Person person in (IEnumerable<Person>) personList)
            KvrplHelper.GetFamily(person, 1, true);
          foreach (Person person in (IEnumerable<Person>) personList)
          {
            if (noteBook.Text.IndexOf(person.FIO) >= 0)
              noteBook.Text = noteBook.Text.Replace(person.FIO, person.PersonId.ToString());
          }
        }
      }
      this.dgvNoteBook.DataSource = (object) noteBookList;
      this.dgvNoteBook.ReadOnly = true;
      this.LoadSettingsDetailCorrect();
      this.MySettingsNB.GridName = "NoteBook";
      this.SetViewNoteBook();
    }

    private void SetViewNoteBook()
    {
      KvrplHelper.AddMaskDateColumn(this.dgvNoteBook, 0, "Дата ввода", "DBeg");
      KvrplHelper.AddMaskDateColumn(this.dgvNoteBook, 1, "Дата окончания", "DEnd");
      this.dgvNoteBook.Columns["Text"].HeaderText = "Запись";
      this.dgvNoteBook.Columns["Note"].HeaderText = "Примечание";
      KvrplHelper.ViewEdit(this.dgvNoteBook);
      foreach (DataGridViewRow row in (IEnumerable) this.dgvNoteBook.Rows)
      {
        DataGridViewCell cell1 = row.Cells["DBeg"];
        DateTime dateTime = ((NoteBook) row.DataBoundItem).DBeg;
        string shortDateString1 = dateTime.ToShortDateString();
        cell1.Value = (object) shortDateString1;
        DataGridViewCell cell2 = row.Cells["DEnd"];
        dateTime = ((NoteBook) row.DataBoundItem).DEnd;
        string shortDateString2 = dateTime.ToShortDateString();
        cell2.Value = (object) shortDateString2;
      }
      this.LoadSettingsNoteBook();
    }

    public void LoadSettingsDetailCorrect()
    {
      this.MySettingsDetailCorrect.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvDetail.Columns)
        this.MySettingsDetailCorrect.GetMySettings(column);
    }

    public void LoadSettingsNoteBook()
    {
      this.MySettingsNB.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvNoteBook.Columns)
        this.MySettingsNB.GetMySettings(column);
    }

    private void dgvDetail_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsDetailCorrect.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsDetailCorrect.Columns[this.MySettingsDetailCorrect.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsDetailCorrect.Save();
    }

    private void dgvDetail_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e, this.client.ClientId);
    }

    private void mpCurrentPeriod_ValueChanged(object sender, EventArgs e)
    {
      Options.Period = KvrplHelper.SaveCurrentPeriod(this.mpCurrentPeriod.Value);
      this.Cursor = Cursors.WaitCursor;
      this.LoadDetailCorrect();
      this.Cursor = Cursors.Default;
    }

    private void dgvNoteBook_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsNB.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsNB.Columns[this.MySettingsNB.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsNB.Save();
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
      this.mpCurrentPeriod = new MonthPicker();
      this.dgvNoteBook = new DataGridView();
      this.lblNoteBook = new Label();
      this.dgvDetail = new DataGridView();
      this.pnBtn.SuspendLayout();
      this.pnUp.SuspendLayout();
      ((ISupportInitialize) this.dgvNoteBook).BeginInit();
      ((ISupportInitialize) this.dgvDetail).BeginInit();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 447);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(966, 40);
      this.pnBtn.TabIndex = 0;
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(873, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(81, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.pnUp.Controls.Add((Control) this.mpCurrentPeriod);
      this.pnUp.Dock = DockStyle.Top;
      this.pnUp.Location = new Point(0, 0);
      this.pnUp.Name = "pnUp";
      this.pnUp.Size = new Size(966, 45);
      this.pnUp.TabIndex = 1;
      this.mpCurrentPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.mpCurrentPeriod.CustomFormat = "MMMM yyyy";
      this.mpCurrentPeriod.Format = DateTimePickerFormat.Custom;
      this.mpCurrentPeriod.Location = new Point(814, 12);
      this.mpCurrentPeriod.Name = "mpCurrentPeriod";
      this.mpCurrentPeriod.OldMonth = 0;
      this.mpCurrentPeriod.ShowUpDown = true;
      this.mpCurrentPeriod.Size = new Size(140, 22);
      this.mpCurrentPeriod.TabIndex = 0;
      this.mpCurrentPeriod.ValueChanged += new EventHandler(this.mpCurrentPeriod_ValueChanged);
      this.dgvNoteBook.BackgroundColor = Color.AliceBlue;
      this.dgvNoteBook.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvNoteBook.Dock = DockStyle.Fill;
      this.dgvNoteBook.Location = new Point(0, 230);
      this.dgvNoteBook.Name = "dgvNoteBook";
      this.dgvNoteBook.Size = new Size(966, 217);
      this.dgvNoteBook.TabIndex = 7;
      this.dgvNoteBook.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvNoteBook_ColumnWidthChanged);
      this.dgvNoteBook.DataError += new DataGridViewDataErrorEventHandler(this.dgvDetail_DataError);
      this.lblNoteBook.BackColor = Color.Transparent;
      this.lblNoteBook.Dock = DockStyle.Top;
      this.lblNoteBook.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblNoteBook.Location = new Point(0, 202);
      this.lblNoteBook.Name = "lblNoteBook";
      this.lblNoteBook.Size = new Size(966, 28);
      this.lblNoteBook.TabIndex = 6;
      this.lblNoteBook.Text = "Основания для перерасчетов";
      this.dgvDetail.BackgroundColor = Color.AliceBlue;
      this.dgvDetail.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvDetail.Dock = DockStyle.Top;
      this.dgvDetail.Location = new Point(0, 45);
      this.dgvDetail.Name = "dgvDetail";
      this.dgvDetail.Size = new Size(966, 157);
      this.dgvDetail.TabIndex = 5;
      this.dgvDetail.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvDetail_ColumnWidthChanged);
      this.dgvDetail.DataError += new DataGridViewDataErrorEventHandler(this.dgvDetail_DataError);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(966, 487);
      this.Controls.Add((Control) this.dgvNoteBook);
      this.Controls.Add((Control) this.lblNoteBook);
      this.Controls.Add((Control) this.dgvDetail);
      this.Controls.Add((Control) this.pnUp);
      this.Controls.Add((Control) this.pnBtn);
      this.Name = "FrmDetailCorrectRent";
      this.Text = "Детализация перерасчета";
      this.Load += new EventHandler(this.FrmDetailCorrectRent_Load);
      this.pnBtn.ResumeLayout(false);
      this.pnUp.ResumeLayout(false);
      ((ISupportInitialize) this.dgvNoteBook).EndInit();
      ((ISupportInitialize) this.dgvDetail).EndInit();
      this.ResumeLayout(false);
    }
  }
}
