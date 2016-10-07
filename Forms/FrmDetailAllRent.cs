// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmDetailAllRent
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using NHibernate;
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
  public class FrmDetailAllRent : FrmBaseForm1
  {
    private FormStateSaver fss = new FormStateSaver(FrmDetailAllRent.ic);
    protected GridSettings MySettingsAllDetailGrid = new GridSettings();
    protected GridSettings MySettingsMSPGrid = new GridSettings();
    private IContainer components = (IContainer) null;
    private static IContainer ic;
    private Service service;
    private ISession session;
    private LsClient client;
    private Panel pnUp;
    private MonthPicker mpCurrentPeriod;
    private ComboBox cbServ;
    private Label label1;
    private ComboBox cbMonth;
    private Label label2;
    private TabControl tcDetailAll;
    private TabPage tpDetail;
    private DataGridView dgvRentAllDetail;
    private TabPage tpMSP;
    private DataGridView dgvMSP;
    private ComboBox cbPersonMSP;
    private Label lblPersonMSP;

    public short Service_id { get; set; }

    public int Period_id { get; set; }

    public int MSP_id { get; set; }

    public FrmDetailAllRent()
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
    }

    public FrmDetailAllRent(LsClient client, Service service)
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
      this.service = service;
      this.client = client;
      this.Service_id = service.ServiceId;
    }

    private void FrmDetailAllRent_Load(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
      this.MySettingsAllDetailGrid.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
      this.MySettingsMSPGrid.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
      this.LoadSettingsAllDetailGrid();
      this.LoadSettingsMSPGrid();
      this.LoadCBService();
      this.LoadCBMonth();
      this.LoadCBPersonMSP();
    }

    private void FrmDetailAllRent_Shown(object sender, EventArgs e)
    {
      this.LoadSettingsAllDetailGrid();
      this.LoadSettingsMSPGrid();
    }

    private void LoadCBPersonMSP()
    {
      IList<DcMSP> dcMspList1 = (IList<DcMSP>) new List<DcMSP>();
      IList<DcMSP> dcMspList2 = this.session.CreateQuery(string.Format("select distinct l.MSPId from LsMSPGku l,SocSaldo s where s.Person.PersonId=l.Person.PersonId and l.MSPId.MSP_id=s.MSP.MSP_id and s.Period='{1}' and s.LsClient.ClientId={0} and l.LsClient.ClientId={0} and l.OnOff=1", (object) this.client.ClientId, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value))).List<DcMSP>();
      dcMspList2.Insert(0, new DcMSP()
      {
        MSP_id = 0,
        MSP_name = ""
      });
      this.cbPersonMSP.DataSource = (object) dcMspList2;
      this.cbPersonMSP.ValueMember = "MSP_id";
      this.cbPersonMSP.DisplayMember = "MSP_name";
      this.MSP_id = 0;
    }

    private void LoadCBMonth()
    {
      IList list = this.session.CreateQuery("from Period p where p.PeriodId<=(select max(Period) from Rent where LsClient=:client) and p.PeriodId<>0 order by PeriodId desc").SetParameter<LsClient>("client", this.client).List();
      list.Insert(0, (object) new Period(0, new DateTime?(Convert.ToDateTime("01.01.0001"))));
      this.cbMonth.DataSource = (object) list;
      this.cbMonth.DisplayMember = "PeriodName";
      this.cbMonth.ValueMember = "PeriodId";
      this.Period_id = 0;
      this.cbMonth.SelectedItem = (object) this.session.Get<Period>((object) this.Period_id);
    }

    private void LoadCBService()
    {
      IList list = this.session.CreateQuery(string.Format("select s from Service s where s.ServiceId in (select Service_id from ServiceParam where Company_id=:company) and s.ServiceId<>0  order by s.ServiceId")).SetParameter<short>("company", this.client.Company.CompanyId).List();
      list.Insert(0, (object) new Service((short) 0, ""));
      this.cbServ.DataSource = (object) list;
      this.cbServ.DisplayMember = "servicename";
      this.cbServ.ValueMember = "serviceid";
      this.Service_id = this.service.ServiceId;
      this.cbServ.SelectedItem = (object) this.session.Get<Service>((object) this.Service_id);
    }

    private void LoadDataAllDetailGrid()
    {
      this.dgvRentAllDetail.Columns.Clear();
      this.dgvRentAllDetail.DataSource = (object) null;
      string str = "select distinct (select service_name from dcService where service_id=(select root from dcservice where service_id=rent.service_id)) as nameUsl, (select service_name from dcService where service_id=rent.service_id) as name, (select period_value from dcPeriod where period_id=month_id) as month, case when rent.code = 0 then 'По тарифам' when rent.code in (-1,-2,1,11) then 'Временная регистрация (изменение тарифа)' when rent.code in (2,12) then 'Отсутствие' when rent.code in (3,13) then 'Снятие по актам' when rent.code in (4,7) then 'Ручные изменения' when rent.code in (5,6) then 'Компенсация'  else 'Другие' end code, case rent.Rent_type when 0 then 'по нормативу' else 'по счетчику' end Rent_type, (select case NAMEORG_MIN when 'Не определено' then '' else NAMEORG_MIN end NAMEORG_MIN  from dcSupplier dcsup inner join base_org bo on  bo.idbaseorg=dcsup.recipient_id where supplier_id=rent.supplier_id ) [Получатель], (select NAMEORG_MIN from dcSupplier dcsup inner join base_org bo on  bo.idbaseorg=dcsup.Perfomer_id where supplier_id=rent.supplier_id ) [Исполнитель], sum(rent.volume) volume, sum(rent.rent) rent, sum(rent.Rent_eo) Rent_eo from dba.lsrent rent where rent.client_id=:client and rent.period_id=:period";
      if ((uint) this.Service_id > 0U)
        str += string.Format(" and service_id in (select service_id from dcService where root={0})", (object) this.Service_id);
      if ((uint) this.Period_id > 0U)
        str += string.Format(" and rent.Month_id = {0}", (object) this.Period_id);
      IList list = this.session.CreateSQLQuery(str + " group by name, month, nameUsl, code, Rent_type, [Получатель], [Исполнитель] order by nameUsl, month desc, name").SetParameter<int>("client", this.client.ClientId).SetParameter<int>("period", Options.Period.PeriodId).List();
      DataTable dataTable = new DataTable("DetailAll");
      dataTable.Columns.Add("Услуга", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Составляющая", System.Type.GetType("System.String"));
      dataTable.Columns.Add("За месяц", System.Type.GetType("System.DateTime"));
      dataTable.Columns.Add("Тип начисления", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Тип", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Получатель", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Исполнитель", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Объем", System.Type.GetType("System.Double"));
      dataTable.Columns.Add("Сумма", System.Type.GetType("System.Double"));
      dataTable.Columns.Add("Сумма ЭО", System.Type.GetType("System.Double"));
      foreach (object[] objArray in (IEnumerable) list)
        dataTable.Rows.Add(objArray);
      this.dgvRentAllDetail.DataSource = (object) dataTable;
      this.dgvRentAllDetail.Focus();
      this.dgvRentAllDetail.ReadOnly = true;
      this.MySettingsAllDetailGrid.GridName = "DetailAll";
    }

    private void LoadDataMSPGrid()
    {
      this.dgvMSP.Columns.Clear();
      this.dgvMSP.DataSource = (object) null;
      string str = "select distinct (select service_name from dcService where service_id=(select root from dcservice where service_id=rent.service_id)) as nameUsl, (select service_name from dcService where service_id=rent.service_id) as nameSos, (select period_value from dcPeriod where period_id=month_id) as month, case when rent.code = 0 then 'По тарифам' when rent.code in (-1,-2,1,11) then 'Временная регистрация (изменение тарифа)' when rent.code in (2,12) then 'Отсутствие' when rent.code in (3,13) then 'Снятие по актам' when rent.code in (4,7) then 'Ручные изменения' when rent.code in (5,6) then 'Компенсация'  else 'Другие' end code, case rent.Rent_type when 0 then 'по нормативу' else 'по счетчику' end Rent_type, (select case NAMEORG_MIN when 'Не определено' then '' else NAMEORG_MIN end NAMEORG_MIN  from dcSupplier dcsup inner join base_org bo on  bo.idbaseorg=dcsup.recipient_id where supplier_id=rent.supplier_id ) [Получатель], (select NAMEORG_MIN from dcSupplier dcsup inner join base_org bo on  bo.idbaseorg=dcsup.Perfomer_id where supplier_id=rent.supplier_id ) [Исполнитель], (select msp_name from dcmsp where msp_id=rent.msp_id) msp_name, (select case when per.family is null then f.family else per.family end from frpers per inner join Form_a f on per.id=f.idform where f.idform=rent.idform_msp and per.code=1) f, (select case when per.name is null then f.name else per.name end from frpers per inner join Form_a f on per.id=f.idform where f.idform=rent.idform_msp and per.code=1) n, (select case when per.lastname is null then f.lastname else per.lastname end from frpers per inner join Form_a f on per.id=f.idform where f.idform=rent.idform_msp and per.code=1) l, sum(rent.MSP_People) MSP_People, sum(rent.volume) volume, sum(rent.rent) rent,idform_msp from dba.lsrentMSP rent where rent.client_id=:client and rent.period_id=:period";
      if ((uint) this.Service_id > 0U)
        str += string.Format(" and service_id in (select service_id from dcService where root={0})", (object) this.Service_id);
      if ((uint) this.Period_id > 0U)
        str += string.Format(" and rent.Month_id = {0}", (object) this.Period_id);
      if ((uint) this.MSP_id > 0U)
        str += string.Format(" and MSP_id = {0}", (object) this.MSP_id);
      IList list = this.session.CreateSQLQuery(str + " group by nameSos, month, nameUsl, code, Rent_type, msp_name, [Получатель], [Исполнитель],l,f,n,idform_msp order by nameUsl, month desc, nameSos").SetParameter<int>("client", this.client.ClientId).SetParameter<int>("period", Options.Period.PeriodId).List();
      DataTable dataTable = new DataTable("MSP");
      dataTable.Columns.Add("Услуга", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Составляющая", System.Type.GetType("System.String"));
      dataTable.Columns.Add("За месяц", System.Type.GetType("System.DateTime"));
      dataTable.Columns.Add("Тип начисления", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Тип", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Получатель", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Исполнитель", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Код льготы", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Фамилия", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Имя", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Отчество", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Кол-во пользующихся", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Объем", System.Type.GetType("System.Double"));
      dataTable.Columns.Add("Сумма", System.Type.GetType("System.Double"));
      dataTable.Columns.Add("id", System.Type.GetType("System.Double"));
      foreach (object[] objArray in (IEnumerable) list)
        dataTable.Rows.Add(objArray);
      this.dgvMSP.DataSource = (object) dataTable;
      if (!KvrplHelper.CheckProxy(48, 1, this.client.Company, false))
      {
        for (int index = 0; index < this.dgvMSP.RowCount - 1; ++index)
        {
          this.dgvMSP.Rows[index].Cells["Фамилия"].Value = this.dgvMSP.Rows[index].Cells["id"].Value;
          this.dgvMSP.Rows[index].Cells["Имя"].Value = (object) "";
          this.dgvMSP.Rows[index].Cells["Отчество"].Value = (object) "";
        }
      }
      else
      {
        this.dgvMSP.Columns["Фамилия"].Visible = true;
        this.dgvMSP.Columns["Имя"].Visible = true;
        this.dgvMSP.Columns["Отчество"].Visible = true;
      }
      this.dgvMSP.Columns["id"].Visible = false;
      this.dgvMSP.Focus();
      this.dgvMSP.ReadOnly = true;
      this.MySettingsMSPGrid.GridName = "MSP";
    }

    private void mpCurrentPeriod_ValueChanged(object sender, EventArgs e)
    {
      Options.Period = KvrplHelper.SaveCurrentPeriod(this.mpCurrentPeriod.Value);
      this.Cursor = Cursors.WaitCursor;
      this.LoadDataAllDetailGrid();
      this.LoadDataMSPGrid();
      this.LoadCBPersonMSP();
      this.Cursor = Cursors.Default;
    }

    private void cbServ_SelectedIndexChanged(object sender, EventArgs e)
    {
      if ((Service) this.cbServ.SelectedItem != null && (int) ((Service) this.cbServ.SelectedItem).ServiceId != (int) this.Service_id)
        this.Service_id = ((Service) this.cbServ.SelectedItem).ServiceId;
      this.LoadDataAllDetailGrid();
      this.LoadDataMSPGrid();
    }

    private void cbMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
      if ((Period) this.cbMonth.SelectedItem != null && ((Period) this.cbMonth.SelectedItem).PeriodId != this.Period_id)
        this.Period_id = ((Period) this.cbMonth.SelectedItem).PeriodId;
      this.LoadDataAllDetailGrid();
      this.LoadDataMSPGrid();
    }

    private void dgvMSP_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsMSPGrid.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsMSPGrid.Columns[this.MySettingsMSPGrid.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsMSPGrid.Save();
    }

    private void dgvRentAllDetail_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsAllDetailGrid.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsAllDetailGrid.Columns[this.MySettingsAllDetailGrid.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsAllDetailGrid.Save();
    }

    private void cbPersonMSP_SelectedIndexChanged(object sender, EventArgs e)
    {
      if ((DcMSP) this.cbPersonMSP.SelectedItem != null && ((DcMSP) this.cbPersonMSP.SelectedItem).MSP_id != this.MSP_id)
        this.MSP_id = ((DcMSP) this.cbPersonMSP.SelectedItem).MSP_id;
      this.LoadDataMSPGrid();
    }

    private void tcDetailAll_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.tcDetailAll.SelectedIndex == 0)
        this.cbPersonMSP.Enabled = false;
      else
        this.cbPersonMSP.Enabled = true;
    }

    public void LoadSettingsAllDetailGrid()
    {
      this.MySettingsAllDetailGrid.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvRentAllDetail.Columns)
        this.MySettingsAllDetailGrid.GetMySettings(column);
    }

    public void LoadSettingsMSPGrid()
    {
      this.MySettingsMSPGrid.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvMSP.Columns)
        this.MySettingsMSPGrid.GetMySettings(column);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnUp = new Panel();
      this.cbPersonMSP = new ComboBox();
      this.lblPersonMSP = new Label();
      this.cbMonth = new ComboBox();
      this.label2 = new Label();
      this.cbServ = new ComboBox();
      this.label1 = new Label();
      this.mpCurrentPeriod = new MonthPicker();
      this.tcDetailAll = new TabControl();
      this.tpDetail = new TabPage();
      this.dgvRentAllDetail = new DataGridView();
      this.tpMSP = new TabPage();
      this.dgvMSP = new DataGridView();
      this.pnUp.SuspendLayout();
      this.tcDetailAll.SuspendLayout();
      this.tpDetail.SuspendLayout();
      ((ISupportInitialize) this.dgvRentAllDetail).BeginInit();
      this.tpMSP.SuspendLayout();
      ((ISupportInitialize) this.dgvMSP).BeginInit();
      this.SuspendLayout();
      this.pnUp.Controls.Add((Control) this.cbPersonMSP);
      this.pnUp.Controls.Add((Control) this.lblPersonMSP);
      this.pnUp.Controls.Add((Control) this.cbMonth);
      this.pnUp.Controls.Add((Control) this.label2);
      this.pnUp.Controls.Add((Control) this.cbServ);
      this.pnUp.Controls.Add((Control) this.label1);
      this.pnUp.Controls.Add((Control) this.mpCurrentPeriod);
      this.pnUp.Dock = DockStyle.Top;
      this.pnUp.Location = new Point(0, 0);
      this.pnUp.Name = "pnUp";
      this.pnUp.Size = new Size(1027, 79);
      this.pnUp.TabIndex = 1;
      this.cbPersonMSP.Enabled = false;
      this.cbPersonMSP.FormattingEnabled = true;
      this.cbPersonMSP.Location = new Point(72, 42);
      this.cbPersonMSP.Name = "cbPersonMSP";
      this.cbPersonMSP.Size = new Size(310, 24);
      this.cbPersonMSP.TabIndex = 7;
      this.cbPersonMSP.SelectedIndexChanged += new EventHandler(this.cbPersonMSP_SelectedIndexChanged);
      this.lblPersonMSP.AutoSize = true;
      this.lblPersonMSP.Location = new Point(13, 45);
      this.lblPersonMSP.Name = "lblPersonMSP";
      this.lblPersonMSP.Size = new Size(53, 16);
      this.lblPersonMSP.TabIndex = 6;
      this.lblPersonMSP.Text = "Льгота";
      this.cbMonth.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.cbMonth.FormattingEnabled = true;
      this.cbMonth.Location = new Point(443, 12);
      this.cbMonth.Name = "cbMonth";
      this.cbMonth.Size = new Size(176, 24);
      this.cbMonth.TabIndex = 4;
      this.cbMonth.SelectedIndexChanged += new EventHandler(this.cbMonth_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(388, 17);
      this.label2.Name = "label2";
      this.label2.Size = new Size(49, 16);
      this.label2.TabIndex = 5;
      this.label2.Text = "Месяц";
      this.cbServ.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.cbServ.FormattingEnabled = true;
      this.cbServ.Location = new Point(72, 12);
      this.cbServ.Name = "cbServ";
      this.cbServ.Size = new Size(310, 24);
      this.cbServ.TabIndex = 2;
      this.cbServ.SelectedIndexChanged += new EventHandler(this.cbServ_SelectedIndexChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 15);
      this.label1.Name = "label1";
      this.label1.Size = new Size(54, 16);
      this.label1.TabIndex = 3;
      this.label1.Text = "Услуга";
      this.mpCurrentPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.mpCurrentPeriod.CustomFormat = "MMMM yyyy";
      this.mpCurrentPeriod.Format = DateTimePickerFormat.Custom;
      this.mpCurrentPeriod.Location = new Point(875, 12);
      this.mpCurrentPeriod.Name = "mpCurrentPeriod";
      this.mpCurrentPeriod.OldMonth = 0;
      this.mpCurrentPeriod.ShowUpDown = true;
      this.mpCurrentPeriod.Size = new Size(140, 22);
      this.mpCurrentPeriod.TabIndex = 1;
      this.mpCurrentPeriod.ValueChanged += new EventHandler(this.mpCurrentPeriod_ValueChanged);
      this.tcDetailAll.Controls.Add((Control) this.tpDetail);
      this.tcDetailAll.Controls.Add((Control) this.tpMSP);
      this.tcDetailAll.Dock = DockStyle.Fill;
      this.tcDetailAll.Location = new Point(0, 79);
      this.tcDetailAll.Name = "tcDetailAll";
      this.tcDetailAll.SelectedIndex = 0;
      this.tcDetailAll.Size = new Size(1027, 520);
      this.tcDetailAll.TabIndex = 2;
      this.tcDetailAll.SelectedIndexChanged += new EventHandler(this.tcDetailAll_SelectedIndexChanged);
      this.tpDetail.Controls.Add((Control) this.dgvRentAllDetail);
      this.tpDetail.Location = new Point(4, 25);
      this.tpDetail.Name = "tpDetail";
      this.tpDetail.Padding = new Padding(3);
      this.tpDetail.Size = new Size(1019, 491);
      this.tpDetail.TabIndex = 0;
      this.tpDetail.Text = "Основные";
      this.tpDetail.UseVisualStyleBackColor = true;
      this.dgvRentAllDetail.BackgroundColor = Color.AliceBlue;
      this.dgvRentAllDetail.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvRentAllDetail.Dock = DockStyle.Fill;
      this.dgvRentAllDetail.Location = new Point(3, 3);
      this.dgvRentAllDetail.Name = "dgvRentAllDetail";
      this.dgvRentAllDetail.ReadOnly = true;
      this.dgvRentAllDetail.RowHeadersWidth = 60;
      this.dgvRentAllDetail.Size = new Size(1013, 485);
      this.dgvRentAllDetail.TabIndex = 3;
      this.tpMSP.Controls.Add((Control) this.dgvMSP);
      this.tpMSP.Location = new Point(4, 25);
      this.tpMSP.Name = "tpMSP";
      this.tpMSP.Padding = new Padding(3);
      this.tpMSP.Size = new Size(1019, 491);
      this.tpMSP.TabIndex = 1;
      this.tpMSP.Text = "Льготы";
      this.tpMSP.UseVisualStyleBackColor = true;
      this.dgvMSP.BackgroundColor = Color.AliceBlue;
      this.dgvMSP.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvMSP.Dock = DockStyle.Fill;
      this.dgvMSP.Location = new Point(3, 3);
      this.dgvMSP.Name = "dgvMSP";
      this.dgvMSP.Size = new Size(1013, 485);
      this.dgvMSP.TabIndex = 0;
      this.dgvMSP.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvMSP_ColumnWidthChanged);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1027, 639);
      this.Controls.Add((Control) this.tcDetailAll);
      this.Controls.Add((Control) this.pnUp);
      this.Margin = new Padding(5);
      this.Name = "FrmDetailAllRent";
      this.Text = "Детализация начислений";
      this.Load += new EventHandler(this.FrmDetailAllRent_Load);
      this.Shown += new EventHandler(this.FrmDetailAllRent_Shown);
      this.Controls.SetChildIndex((Control) this.pnUp, 0);
      this.Controls.SetChildIndex((Control) this.tcDetailAll, 0);
      this.pnUp.ResumeLayout(false);
      this.pnUp.PerformLayout();
      this.tcDetailAll.ResumeLayout(false);
      this.tpDetail.ResumeLayout(false);
      ((ISupportInitialize) this.dgvRentAllDetail).EndInit();
      this.tpMSP.ResumeLayout(false);
      ((ISupportInitialize) this.dgvMSP).EndInit();
      this.ResumeLayout(false);
    }
  }
}
