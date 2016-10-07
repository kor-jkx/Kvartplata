// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmClientCardChoose
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
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
  public class FrmClientCardChoose : Form
  {
    private FormStateSaver formStateSaver = new FormStateSaver(FrmClientCardChoose.container);
    private IContainer components = (IContainer) null;
    private short ShowIndex;
    private Service service;
    private Norm norm;
    private Tariff tariff;
    private ISession Session;
    private int CompanyId;
    private int id;
    private DateTime? dbeg;
    private static IContainer container;
    private Panel panel1;
    private Button btnOk;
    private Button btnExit;
    private GroupBox groupBox1;
    private DataGridView dgvObj;

    public int Id
    {
      get
      {
        return this.id;
      }
      set
      {
        this.id = value;
      }
    }

    public int Num { get; set; }

    public double Value { get; set; }

    public bool PastTime { get; set; }

    public FrmClientCardChoose()
    {
      this.formStateSaver.ParentForm = (Form) this;
      this.InitializeComponent();
    }

    public FrmClientCardChoose(Service service, Tariff tariff, int companyId, DateTime? dbeg)
    {
      this.formStateSaver.ParentForm = (Form) this;
      this.InitializeComponent();
      this.ShowIndex = (short) 1;
      this.tariff = tariff;
      this.service = service;
      this.id = -1;
      this.Num = -1;
      this.Value = 0.0;
      this.Text = "Выбор тарифа";
      this.CompanyId = companyId;
      this.dbeg = dbeg;
    }

    public FrmClientCardChoose(Service service, Norm norm, int companyId, DateTime? dbeg)
    {
      this.formStateSaver.ParentForm = (Form) this;
      this.InitializeComponent();
      this.ShowIndex = (short) 2;
      this.norm = norm;
      this.service = service;
      this.id = -1;
      this.Num = -1;
      this.Value = 0.0;
      this.Text = "Выбор норматива";
      this.CompanyId = companyId;
      this.dbeg = dbeg;
    }

    private void FrmClientCardChoose_Load(object sender, EventArgs e)
    {
      if ((int) this.ShowIndex == 1)
        this.LoadTariff();
      if ((int) this.ShowIndex != 2)
        return;
      this.LoadNorm();
    }

    private void LoadNorm()
    {
      this.Session = Domain.CurrentSession;
      Period nextPeriod = KvrplHelper.GetNextPeriod(KvrplHelper.GetCmpKvrClose(this.Session.Get<Company>((object) (short) this.CompanyId), Options.ComplexPasp.ComplexId, Options.ComplexPrior.ComplexId));
      CmpParam cmpParam1 = new CmpParam();
      cmpParam1.Company_id = 0;
      try
      {
        ISession session1 = this.Session;
        string format1 = " from CmpParam cm where cm.Company_id = {0} and cm.Period.PeriodName=(select max(c.Period.PeriodName) from CmpParam c where c.Period.PeriodId <> 0  and c.Param_id={2} and c.Dbeg <= '{3}' and c.Dend >= '{4}'  and Period.PeriodName >= '{4}')  and cm.Param_id={2} and cm.Dbeg <= '{3}' and cm.Dend >= '{4}' ";
        object[] objArray1 = new object[5]{ (object) this.CompanyId, (object) 0, (object) 204, null, null };
        int index1 = 3;
        DateTime? periodName = nextPeriod.PeriodName;
        string baseFormat1 = KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(periodName.Value));
        objArray1[index1] = (object) baseFormat1;
        int index2 = 4;
        periodName = nextPeriod.PeriodName;
        string baseFormat2 = KvrplHelper.DateToBaseFormat(periodName.Value);
        objArray1[index2] = (object) baseFormat2;
        string queryString1 = string.Format(format1, objArray1);
        IList<CmpParam> cmpParamList = session1.CreateQuery(queryString1).List<CmpParam>();
        CmpParam cmpParam2;
        if (cmpParamList.Count <= 0)
        {
          ISession session2 = this.Session;
          string format2 = " from CmpParam where Company_id = {0} and Period.PeriodId={1}  and Param_id={2} and Dbeg <= '{3}' and Dend >= '{4}'";
          object[] objArray2 = new object[5]{ (object) this.CompanyId, (object) 0, (object) 204, null, null };
          int index3 = 3;
          periodName = nextPeriod.PeriodName;
          string baseFormat3 = KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(periodName.Value));
          objArray2[index3] = (object) baseFormat3;
          int index4 = 4;
          periodName = nextPeriod.PeriodName;
          string baseFormat4 = KvrplHelper.DateToBaseFormat(periodName.Value);
          objArray2[index4] = (object) baseFormat4;
          string queryString2 = string.Format(format2, objArray2);
          cmpParam2 = session2.CreateQuery(queryString2).List<CmpParam>()[0];
        }
        else
          cmpParam2 = cmpParamList[0];
        cmpParam1 = cmpParam2;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      DateTime? nullable = nextPeriod.PeriodName;
      DateTime dt = nullable.Value;
      int num;
      if (this.dbeg.HasValue)
      {
        nullable = this.dbeg;
        DateTime dateTime = dt;
        num = nullable.HasValue ? (nullable.GetValueOrDefault() > dateTime ? 1 : 0) : 0;
      }
      else
        num = 0;
      if (num != 0)
        dt = this.dbeg.Value;
      IQuery query;
      if (!this.PastTime)
        query = this.Session.CreateQuery(string.Format("from CmpNorm c, Norm n where c.Norm.Norm_id = n.Norm_id  and n.Service.ServiceId = {0} and c.Company_id = {1}  and Dbeg <= '{2}' and Dend >= '{3}' order by n.Norm_num", (object) this.service.ServiceId, (object) Convert.ToInt32((object) cmpParam1.Param_value), (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(dt)), (object) KvrplHelper.DateToBaseFormat(KvrplHelper.FirstDay(dt))));
      else
        query = this.Session.CreateQuery(string.Format("from CmpNorm c, Norm n where c.Norm.Norm_id = n.Norm_id  and n.Service.ServiceId = {0} and c.Company_id = {1} and c.Dbeg = (select max(cc.Dbeg) from CmpNorm cc where cc.Norm.Norm_id = c.Norm.Norm_id and cc.Company_id=c.Company_id) order by n.Norm_num", (object) this.service.ServiceId, (object) Convert.ToInt32((object) cmpParam1.Param_value)));
      IList list = query.List();
      DataTable dataTable = new DataTable("temp");
      dataTable.Columns.Add("Номер", System.Type.GetType("System.Int32"));
      dataTable.Columns.Add("Наименование", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Значение", System.Type.GetType("System.Double"));
      dataTable.Columns.Add("Id", System.Type.GetType("System.Int32"));
      foreach (object[] objArray in (IEnumerable) list)
      {
        string normName = ((Norm) objArray[1]).Norm_name;
        dataTable.Rows.Add((object) ((Norm) objArray[1]).Norm_num, (object) normName, (object) ((CmpNorm) objArray[0]).Norm_value, (object) ((CmpNorm) objArray[0]).Norm.Norm_id);
      }
      this.dgvObj.DataSource = (object) dataTable;
      this.dgvObj.Columns["Id"].Visible = false;
    }

    private void LoadTariff()
    {
      this.Session = Domain.CurrentSession;
      Period nextPeriod = KvrplHelper.GetNextPeriod(KvrplHelper.GetCmpKvrClose(this.Session.Get<Company>((object) (short) this.CompanyId), Options.ComplexPasp.ComplexId, Options.ComplexPrior.ComplexId));
      CmpParam cmpParam1 = new CmpParam();
      cmpParam1.Company_id = 0;
      try
      {
        ISession session1 = this.Session;
        string format1 = " from CmpParam cm where cm.Company_id = {0} and cm.Period.PeriodName=(select max(c.Period.PeriodName) from CmpParam c where c.Period.PeriodId <> 0  and c.Param_id={2} and c.Dbeg <= '{3}' and c.Dend >= '{4}'  and Period.PeriodName >= '{4}')  and cm.Param_id={2} and cm.Dbeg <= '{3}' and cm.Dend >= '{4}' ";
        object[] objArray1 = new object[5]{ (object) this.CompanyId, (object) 0, (object) 201, null, null };
        int index1 = 3;
        DateTime? periodName = nextPeriod.PeriodName;
        string baseFormat1 = KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(periodName.Value));
        objArray1[index1] = (object) baseFormat1;
        int index2 = 4;
        periodName = nextPeriod.PeriodName;
        string baseFormat2 = KvrplHelper.DateToBaseFormat(periodName.Value);
        objArray1[index2] = (object) baseFormat2;
        string queryString1 = string.Format(format1, objArray1);
        IList<CmpParam> cmpParamList = session1.CreateQuery(queryString1).List<CmpParam>();
        CmpParam cmpParam2;
        if (cmpParamList.Count <= 0)
        {
          ISession session2 = this.Session;
          string format2 = " from CmpParam where Company_id = {0} and Period.PeriodId={1}  and Param_id={2} and Dbeg <= '{3}' and Dend >= '{4}'";
          object[] objArray2 = new object[5]{ (object) this.CompanyId, (object) 0, (object) 201, null, null };
          int index3 = 3;
          periodName = nextPeriod.PeriodName;
          string baseFormat3 = KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(periodName.Value));
          objArray2[index3] = (object) baseFormat3;
          int index4 = 4;
          periodName = nextPeriod.PeriodName;
          string baseFormat4 = KvrplHelper.DateToBaseFormat(periodName.Value);
          objArray2[index4] = (object) baseFormat4;
          string queryString2 = string.Format(format2, objArray2);
          cmpParam2 = session2.CreateQuery(queryString2).List<CmpParam>()[0];
        }
        else
          cmpParam2 = cmpParamList[0];
        cmpParam1 = cmpParam2;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      DateTime? nullable = nextPeriod.PeriodName;
      DateTime dt = nullable.Value;
      int num;
      if (this.dbeg.HasValue)
      {
        nullable = this.dbeg;
        DateTime dateTime = dt;
        num = nullable.HasValue ? (nullable.GetValueOrDefault() > dateTime ? 1 : 0) : 0;
      }
      else
        num = 0;
      if (num != 0)
        dt = this.dbeg.Value;
      IList<cmpTariffCost> cmpTariffCostList;
      if (!this.PastTime)
        cmpTariffCostList = this.Session.CreateQuery(string.Format("select new cmpTariffCost(tc.Tariff_id,sum(tc.Cost),(select Scheme from cmpTariffCost c where c.Tariff_id=tc.Tariff_id and c.Service.ServiceId={0} and c.Company_id={1} and c.Period.PeriodId=0 and c.Dbeg=tc.Dbeg and c.Dend=tc.Dend),(select SchemeParam from cmpTariffCost c where c.Tariff_id=tc.Tariff_id and c.Service.ServiceId={0} and c.Company_id={1} and c.Period.PeriodId=0 and c.Dbeg=tc.Dbeg and c.Dend=tc.Dend)) from cmpTariffCost tc where tc.Service.ServiceId in (select ServiceId from Service where Root={0}) and tc.Company_id={1} and tc.Dbeg<='{2}' and tc.Dend>='{3}' and isnull(tc.Scheme,0)<>3 group by tc.Tariff_id,tc.Dbeg,tc.Dend,tc.SchemeParam", (object) this.service.ServiceId, (object) Convert.ToInt32((object) cmpParam1.Param_value), (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(dt)), (object) KvrplHelper.DateToBaseFormat(KvrplHelper.FirstDay(dt)))).List<cmpTariffCost>();
      else
        cmpTariffCostList = this.Session.CreateQuery(string.Format("from cmpTariffCost c where c.Service.ServiceId={0} and c.Company_id={1} and  c.Dbeg = (select max(cc.Dbeg) from cmpTariffCost cc where cc.Service.ServiceId={0} and cc.Company_id={1} and cc.Tariff_id = c.Tariff_id)  order by c.Tariff_id", (object) this.service.ServiceId, (object) Convert.ToInt32((object) cmpParam1.Param_value))).List<cmpTariffCost>();
      DataTable dataTable = new DataTable("temp") { Columns = { { "Номер", System.Type.GetType("System.Int32") }, { "Наименование", System.Type.GetType("System.String") }, { "Цена", System.Type.GetType("System.Double") }, { "Id", System.Type.GetType("System.Int32") } } };
      IList<FrmClientCardChoose.tmpTariff> source = (IList<FrmClientCardChoose.tmpTariff>) new List<FrmClientCardChoose.tmpTariff>();
      foreach (cmpTariffCost cmpTariffCost in (IEnumerable<cmpTariffCost>) cmpTariffCostList)
      {
        Tariff tariff = this.Session.Get<Tariff>((object) cmpTariffCost.Tariff_id);
        string tariffName = tariff.Tariff_name;
        source.Add(new FrmClientCardChoose.tmpTariff()
        {
          Tariff_num = tariff.Tariff_num,
          TariffName = tariffName,
          Cost = cmpTariffCost.Cost,
          Tariff_id = cmpTariffCost.Tariff_id,
          Scheme = cmpTariffCost.Scheme,
          SchemeParam = cmpTariffCost.SchemeParam
        });
      }
      this.dgvObj.AutoGenerateColumns = true;
      this.dgvObj.DataSource = (object) source.OrderBy<FrmClientCardChoose.tmpTariff, int>((Func<FrmClientCardChoose.tmpTariff, int>) (c => c.Tariff_num)).ToList<FrmClientCardChoose.tmpTariff>();
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.id = -1;
      this.Close();
    }

    private void dgvObj_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.RowIndex >= this.dgvObj.Rows.Count)
        return;
      try
      {
        this.id = (int) this.ShowIndex == 1 ? ((FrmClientCardChoose.tmpTariff) this.dgvObj.Rows[e.RowIndex].DataBoundItem).Tariff_id : (int) this.dgvObj.Rows[e.RowIndex].Cells["Id"].Value;
        this.Num = (int) this.ShowIndex == 1 ? ((FrmClientCardChoose.tmpTariff) this.dgvObj.Rows[e.RowIndex].DataBoundItem).Tariff_num : (int) this.dgvObj.Rows[e.RowIndex].Cells["Номер"].Value;
        this.Value = (int) this.ShowIndex == 1 ? ((FrmClientCardChoose.tmpTariff) this.dgvObj.Rows[e.RowIndex].DataBoundItem).Cost.Value : (double) this.dgvObj.Rows[e.RowIndex].Cells["Значение"].Value;
      }
      catch (Exception ex)
      {
        this.id = -1;
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void dgvObj_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      this.dgvObj_CellMouseClick(sender, e);
      this.Close();
    }

    private void dgvObj_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmClientCardChoose));
      this.panel1 = new Panel();
      this.btnOk = new Button();
      this.btnExit = new Button();
      this.groupBox1 = new GroupBox();
      this.dgvObj = new DataGridView();
      this.panel1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      ((ISupportInitialize) this.dgvObj).BeginInit();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.btnOk);
      this.panel1.Controls.Add((Control) this.btnExit);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 282);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(502, 45);
      this.panel1.TabIndex = 1;
      this.btnOk.Image = (Image) Resources.Tick;
      this.btnOk.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnOk.Location = new Point(12, 10);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(64, 28);
      this.btnOk.TabIndex = 1;
      this.btnOk.Text = "ОК";
      this.btnOk.TextAlign = ContentAlignment.MiddleRight;
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.delete;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(405, 10);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(90, 28);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Отмена";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.groupBox1.Controls.Add((Control) this.dgvObj);
      this.groupBox1.Dock = DockStyle.Fill;
      this.groupBox1.Location = new Point(0, 0);
      this.groupBox1.Margin = new Padding(4);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new Padding(4);
      this.groupBox1.Size = new Size(502, 282);
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Выбор объекта";
      this.dgvObj.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.dgvObj.BackgroundColor = Color.AliceBlue;
      this.dgvObj.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvObj.Dock = DockStyle.Fill;
      this.dgvObj.Location = new Point(4, 20);
      this.dgvObj.Margin = new Padding(4);
      this.dgvObj.Name = "dgvObj";
      this.dgvObj.ReadOnly = true;
      this.dgvObj.Size = new Size(494, 258);
      this.dgvObj.TabIndex = 0;
      this.dgvObj.VirtualMode = true;
      this.dgvObj.CellMouseClick += new DataGridViewCellMouseEventHandler(this.dgvObj_CellMouseClick);
      this.dgvObj.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dgvObj_CellMouseDoubleClick);
      this.dgvObj.DataError += new DataGridViewDataErrorEventHandler(this.dgvObj_DataError);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(502, 327);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.panel1);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmClientCardChoose";
      this.Text = "FrmClientCardChoose";
      this.Load += new EventHandler(this.FrmClientCardChoose_Load);
      this.panel1.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      ((ISupportInitialize) this.dgvObj).EndInit();
      this.ResumeLayout(false);
    }

    public class tmpTariff
    {
      [DisplayName("Номер")]
      [Browsable(true)]
      public int Tariff_num { get; set; }

      [DisplayName("Наименование тарифа")]
      [Browsable(true)]
      public string TariffName { get; set; }

      [DisplayName("Цена")]
      [Browsable(true)]
      public double? Cost { get; set; }

      [DisplayName("Алгоритм")]
      [Browsable(true)]
      public short? Scheme { get; set; }

      [DisplayName("Схема пар-ов")]
      [Browsable(true)]
      public short? SchemeParam { get; set; }

      [Browsable(false)]
      public int Tariff_id { get; set; }
    }
  }
}
