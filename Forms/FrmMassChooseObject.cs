// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmMassChooseObject
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmMassChooseObject : Form
  {
    private bool checkAll = true;
    private FormStateSaver formStateSaver = new FormStateSaver(FrmMassChooseObject.container);
    private IContainer components = (IContainer) null;
    private ISession session;
    private static IContainer container;
    private bool PastTime;
    private Panel pnBtn;
    private Button btnExit;
    private Button btnStart;
    private Panel panelChoose;
    private Button btnCheck;
    private GroupBox groupBox2;
    private CheckedListBox chlbAddress;
    private GroupBox groupBox3;
    private RadioButton rbtnUpdate;
    private RadioButton rbtnDelete;
    private RadioButton rbtnInsert;
    private GroupBox groupBox4;
    private CheckBox chbUchet;
    private GroupBox gpbFlat;
    private RadioButton rbtnNotPrivat;
    private RadioButton rbtnPrivat;
    private CheckBox chbArhive;
    private GroupBox groupBox1;
    private RadioButton rbtnQuality;
    private RadioButton rbtnSupplier;
    private RadioButton rbtnService;
    private MonthPicker mpPeriod;
    private Panel panel3;
    private Label lblGrid1;
    private Label label1;
    private Label label3;
    private MaskedTextBox mtbDEnd;
    private ComboBox cbService;
    private MaskedTextBox mtbDBeg;
    private Label label2;
    private Panel pnPeriod;
    private Label label4;
    private Button btnPastTime;
    private HelpProvider hp;
    private Label lblSost;
    private ComboBox cmbSost;
    private RadioButton rbtnParam;
    private DataGridView dgvNorm;
    private DataGridView dgvService;
    private Label lblGrid2;
    private Panel pnNorm;
    private CheckBox cbNorm;
    private GroupBox gbLs;
    private RadioButton rbKv;
    private RadioButton rbAr;
    private GroupBox gbType;
    private RadioButton rbWithout;
    private RadioButton rbWith;
    private ComboBox cmbType;
    private SplitContainer splitContainer1;
    private SplitContainer splitContainer2;

    public Company CurrentCompany { get; set; }

    public Period MonthClosed { get; set; }

    public FrmMassChooseObject()
    {
      this.InitializeComponent();
      this.formStateSaver.ParentForm = (Form) this;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void FrmMassChooseObject_Load(object sender, EventArgs e)
    {
      this.LoadControls();
    }

    private void LoadControls()
    {
      this.session = Domain.CurrentSession;
      this.LoadAddress();
      this.LoadService();
      this.LoadDate();
      this.CheckControls();
      new ToolTip().SetToolTip((Control) this.rbtnUpdate, "Обновляет только дату окончания");
      this.rbtnService.Checked = true;
      if (Options.Arenda && Options.Kvartplata)
        this.gbLs.Visible = true;
      IList<Scheme> schemeList = (IList<Scheme>) new List<Scheme>();
      try
      {
        schemeList = this.session.CreateCriteria(typeof (Scheme)).Add((ICriterion) Restrictions.Eq("SchemeType", (object) Convert.ToInt16(6))).AddOrder(Order.Asc("SchemeId")).List<Scheme>();
        schemeList.Insert(0, new Scheme((short) 0, ""));
      }
      catch
      {
      }
      this.cmbType.DataSource = (object) schemeList;
      this.cmbType.DisplayMember = "SchemeName";
      this.cmbType.ValueMember = "SchemeId";
    }

    private void LoadLable()
    {
      if (this.rbtnService.Checked)
      {
        this.lblGrid1.Text = "Варианты услуг";
        this.lblGrid2.Text = "Нормативы";
        if (!this.PastTime)
          this.pnPeriod.Visible = false;
        else
          this.pnPeriod.Visible = true;
        this.lblGrid2.Visible = true;
      }
      else if (this.rbtnSupplier.Checked)
      {
        this.lblGrid1.Text = "Составляющие услуг";
        this.lblGrid2.Text = "Поставщики";
        if (!this.PastTime)
          this.pnPeriod.Visible = false;
        else
          this.pnPeriod.Visible = true;
        this.lblGrid2.Visible = true;
      }
      else if (this.rbtnQuality.Checked)
      {
        this.lblGrid1.Text = "Причины снижения качества";
        this.lblGrid2.Visible = false;
        this.pnPeriod.Visible = true;
        this.mpPeriod.Value = this.MonthClosed.PeriodName.Value.AddMonths(1);
      }
      else
      {
        if (!this.rbtnParam.Checked)
          return;
        this.lblGrid1.Text = "Параметры по услугам";
        this.lblGrid2.Visible = true;
        this.pnPeriod.Visible = false;
        this.mpPeriod.Value = this.MonthClosed.PeriodName.Value.AddMonths(1);
      }
    }

    private void LoadServiceVar()
    {
      this.btnPastTime.Enabled = true;
      this.dgvNorm.Visible = true;
      this.dgvNorm.ReadOnly = true;
      this.lblSost.Enabled = false;
      this.cmbSost.Enabled = false;
      this.LoadDgvService();
      this.LoadDgvNorm();
      this.LoadLable();
    }

    private void LoadSupplier()
    {
      this.btnPastTime.Enabled = true;
      this.dgvNorm.Visible = true;
      this.dgvNorm.ReadOnly = true;
      this.lblSost.Enabled = false;
      this.cmbSost.Enabled = false;
      this.LoadDgvSostService();
      this.LoadDgvSupplier();
      this.LoadLable();
    }

    private void LoadQuality()
    {
      this.PastTime = false;
      this.btnPastTime.BackColor = this.pnBtn.BackColor;
      this.btnPastTime.Enabled = false;
      this.dgvNorm.ReadOnly = true;
      this.lblSost.Enabled = true;
      this.cmbSost.Enabled = true;
      this.dgvNorm.Visible = false;
      this.LoadDate();
      this.LoadDgvQuality();
      this.LoadLable();
    }

    private void LoadServiceParam()
    {
      this.btnPastTime.Enabled = true;
      this.lblSost.Enabled = false;
      this.cmbSost.Enabled = false;
      this.dgvNorm.Visible = true;
      this.dgvNorm.ReadOnly = false;
      this.LoadService();
      this.LoadDgvParam();
      this.LoadDgvParamValue();
      this.LoadLable();
    }

    private void CheckControls()
    {
      this.rbtnInsert.Checked = true;
      this.chbArhive.Checked = true;
      this.chbUchet.Checked = true;
    }

    private void LoadDgvService()
    {
      if (this.cbService.SelectedItem == null)
        return;
      CmpParam mainCompanyParam = KvrplHelper.GetMainCompanyParam(this.CurrentCompany, 201);
      Period nextPeriod = KvrplHelper.GetNextPeriod(this.MonthClosed);
      IList list1 = (IList) new ArrayList();
      IList list2;
      if (!this.PastTime)
      {
        ISession session = this.session;
        string format = "from cmpTariffCost c, Tariff d where c.Tariff_id=d.Tariff_id and c.Service.ServiceId={0} and c.Company_id={1}  and Dbeg <= '{2}' and Dend >= '{3}'";
        object[] objArray = new object[4]{ (object) ((Service) this.cbService.SelectedItem).ServiceId, (object) Convert.ToInt32((object) mainCompanyParam.Param_value), null, null };
        int index1 = 2;
        DateTime? periodName = nextPeriod.PeriodName;
        string baseFormat1 = KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(periodName.Value));
        objArray[index1] = (object) baseFormat1;
        int index2 = 3;
        periodName = nextPeriod.PeriodName;
        string baseFormat2 = KvrplHelper.DateToBaseFormat(periodName.Value);
        objArray[index2] = (object) baseFormat2;
        string queryString = string.Format(format, objArray);
        list2 = session.CreateQuery(queryString).List();
      }
      else
        list2 = this.session.CreateQuery(string.Format("from cmpTariffCost c, Tariff d where c.Tariff_id=d.Tariff_id and c.Service.ServiceId={0} and c.Company_id={1}  and Dbeg <= '{2}' and Dend >= '{3}' and c.Period.PeriodId=(select max(ctt.Period.PeriodId) from cmpTariffCost ctt where ctt.Tariff_id=c.Tariff_id and ctt.Service.ServiceId={0} and ctt.Company_id={1} and ctt.Dbeg<='{2}' and ctt.Dend>='{3}')", (object) ((Service) this.cbService.SelectedItem).ServiceId, (object) Convert.ToInt32((object) mainCompanyParam.Param_value), (object) KvrplHelper.DateToBaseFormat(Convert.ToDateTime(this.mtbDBeg.Text)), (object) KvrplHelper.DateToBaseFormat(Convert.ToDateTime(this.mtbDEnd.Text)))).List();
      DataTable dataTable = new DataTable("temp");
      dataTable.Columns.Add("Номер", System.Type.GetType("System.Int32"));
      dataTable.Columns.Add("Наименование", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Цена", System.Type.GetType("System.Double"));
      dataTable.Columns.Add("Id", System.Type.GetType("System.Int32"));
      foreach (object[] objArray in (IEnumerable) list2)
      {
        string tariffName = ((Tariff) objArray[1]).Tariff_name;
        dataTable.Rows.Add((object) ((Tariff) objArray[1]).Tariff_num, (object) tariffName, (object) ((cmpTariffCost) objArray[0]).Cost, (object) ((Tariff) objArray[1]).Tariff_id);
      }
      this.dgvService.DataSource = (object) dataTable;
      this.dgvService.Columns["Id"].Visible = false;
    }

    private void LoadDgvNorm()
    {
      if (this.cbService.SelectedItem == null)
        return;
      CmpParam mainCompanyParam = KvrplHelper.GetMainCompanyParam(this.CurrentCompany, 204);
      Period nextPeriod = KvrplHelper.GetNextPeriod(this.MonthClosed);
      IQuery query;
      if (!this.PastTime)
      {
        ISession session = this.session;
        string format = "from CmpNorm c, Norm n where c.Norm.Norm_id = n.Norm_id  and n.Service.ServiceId = {0} and c.Company_id = {1}  and Dbeg <= '{2}' and Dend >= '{3}' ";
        object[] objArray = new object[4]{ (object) ((Service) this.cbService.SelectedItem).ServiceId, (object) Convert.ToInt32((object) mainCompanyParam.Param_value), null, null };
        int index1 = 2;
        DateTime? periodName = nextPeriod.PeriodName;
        string baseFormat1 = KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(periodName.Value));
        objArray[index1] = (object) baseFormat1;
        int index2 = 3;
        periodName = nextPeriod.PeriodName;
        string baseFormat2 = KvrplHelper.DateToBaseFormat(periodName.Value);
        objArray[index2] = (object) baseFormat2;
        string queryString = string.Format(format, objArray);
        query = session.CreateQuery(queryString);
      }
      else
        query = this.session.CreateQuery(string.Format("from CmpNorm c, Norm n where c.Norm.Norm_id = n.Norm_id  and n.Service.ServiceId = {0} and c.Company_id = {1}  and Dbeg <= '{2}' and Dend >= '{3}' and c.Period.PeriodId=(select max(ctt.Period.PeriodId) from CmpNorm ctt where ctt.Norm.Norm_id=c.Norm.Norm_id and ctt.Norm.Service.ServiceId={0} and ctt.Company_id={1} and ctt.Dbeg<='{2}' and ctt.Dend>='{3}')", (object) ((Service) this.cbService.SelectedItem).ServiceId, (object) Convert.ToInt32((object) mainCompanyParam.Param_value), (object) KvrplHelper.DateToBaseFormat(Convert.ToDateTime(this.mtbDBeg.Text)), (object) KvrplHelper.DateToBaseFormat(Convert.ToDateTime(this.mtbDEnd.Text))));
      DataTable dataTable = new DataTable("temp");
      dataTable.Columns.Add("Номер", System.Type.GetType("System.Int32"));
      dataTable.Columns.Add("Наименование", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Значение", System.Type.GetType("System.Double"));
      dataTable.Columns.Add("Id", System.Type.GetType("System.Int32"));
      foreach (object[] objArray in (IEnumerable) query.List())
      {
        string normName = ((Norm) objArray[1]).Norm_name;
        dataTable.Rows.Add((object) ((Norm) objArray[1]).Norm_num, (object) normName, (object) ((CmpNorm) objArray[0]).Norm_value, (object) ((Norm) objArray[1]).Norm_id);
      }
      this.dgvNorm.Columns.Clear();
      this.dgvNorm.DataSource = (object) null;
      this.dgvNorm.DataSource = (object) dataTable;
      this.dgvNorm.Columns["Id"].Visible = false;
    }

    private void LoadDate()
    {
      if (!this.PastTime)
      {
        this.mtbDBeg.Text = KvrplHelper.GetNextPeriod(this.MonthClosed).PeriodName.Value.ToShortDateString();
        this.mtbDEnd.Text = "31.12.2999";
      }
      else
      {
        MaskedTextBox mtbDbeg = this.mtbDBeg;
        DateTime? periodName = this.MonthClosed.PeriodName;
        string shortDateString1 = KvrplHelper.FirstDay(periodName.Value).ToShortDateString();
        mtbDbeg.Text = shortDateString1;
        MaskedTextBox mtbDend = this.mtbDEnd;
        periodName = this.MonthClosed.PeriodName;
        string shortDateString2 = KvrplHelper.LastDay(periodName.Value).ToShortDateString();
        mtbDend.Text = shortDateString2;
      }
    }

    private void LoadService()
    {
      string str = "";
      if (Options.Kvartplata && !Options.Arenda)
        str = string.Format("and sp.Complex.IdFk={0}", (object) Options.Complex.IdFk);
      if (!Options.Kvartplata && Options.Arenda)
        str = string.Format("and sp.Complex.IdFk={0}", (object) Options.ComplexArenda.IdFk);
      IList<Service> serviceList1 = this.session.CreateQuery(string.Format("select distinct s from Service s,ServiceParam sp where sp.Service_id=s.ServiceId and s.ServiceId<>0 and s.Root=0 and sp.Company_id={0} " + str + " order by " + Options.SortService, (object) this.CurrentCompany.CompanyId)).List<Service>();
      if (this.rbtnParam.Checked)
        serviceList1.Insert(0, new Service((short) 0, "Общая услуга"));
      this.cbService.DataSource = (object) serviceList1;
      this.cbService.DisplayMember = "ServiceName";
      this.cbService.ValueMember = "ServiceId";
      IList<Service> serviceList2 = this.session.CreateCriteria(typeof (Service)).Add((ICriterion) Restrictions.Eq("Root", (object) ((Service) this.cbService.SelectedItem).ServiceId)).List<Service>();
      serviceList2.Insert(0, new Service(Convert.ToInt16(0), ""));
      this.cmbSost.DataSource = (object) serviceList2;
      this.cmbSost.DisplayMember = "ServiceName";
      this.cmbSost.ValueMember = "ServiceId";
    }

    private void LoadAddress()
    {
      this.chlbAddress.DataSource = (object) this.session.CreateQuery(string.Format("select h from Home h left join fetch h.Str,Transfer t,HomeLink hl where hl.Company.CompanyId={0} and hl.Company=t.Company and t.KvrCmp is not null and hl.Home=h " + Options.HomeType + " and h.IdHome in (select ls.Home.IdHome from LsClient ls where 1=1 " + Options.MainConditions1 + ") order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome),h.HomeKorp", (object) this.CurrentCompany.CompanyId)).List<Home>();
      this.chlbAddress.DisplayMember = "Address";
      this.chlbAddress.ValueMember = "IdHome";
    }

    private void cbService_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.rbtnService.Checked)
      {
        this.LoadDgvService();
        this.LoadDgvNorm();
      }
      else if (this.rbtnSupplier.Checked)
      {
        this.LoadDgvSostService();
        this.LoadDgvSupplier();
      }
      else
      {
        if (!this.rbtnQuality.Checked)
          return;
        this.LoadDgvQuality();
        IList<Service> serviceList = this.session.CreateCriteria(typeof (Service)).Add((ICriterion) Restrictions.Eq("Root", (object) ((Service) this.cbService.SelectedItem).ServiceId)).List<Service>();
        serviceList.Insert(0, new Service(Convert.ToInt16(0), ""));
        this.cmbSost.DataSource = (object) serviceList;
        this.cmbSost.DisplayMember = "ServiceName";
        this.cmbSost.ValueMember = "ServiceId";
      }
    }

    private void CheckAll()
    {
      for (int index = 0; index < this.chlbAddress.Items.Count; ++index)
        this.chlbAddress.SetItemCheckState(index, CheckState.Checked);
    }

    private void UnCheckAll()
    {
      for (int index = 0; index < this.chlbAddress.Items.Count; ++index)
        this.chlbAddress.SetItemCheckState(index, CheckState.Unchecked);
    }

    private void btnCheck_Click(object sender, EventArgs e)
    {
      if (this.checkAll)
      {
        this.CheckAll();
        this.btnCheck.Text = "Снять все";
        this.checkAll = false;
      }
      else
      {
        this.UnCheckAll();
        this.btnCheck.Text = "Выделить все";
        this.checkAll = true;
      }
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
      if ((this.rbtnService.Checked || this.rbtnSupplier.Checked || this.rbtnParam.Checked) && !KvrplHelper.CheckProxy(40, 2, this.CurrentCompany, true) || this.rbtnQuality.Checked && !KvrplHelper.CheckProxy(43, 2, this.CurrentCompany, true))
        return;
      if (this.rbtnInsert.Checked)
        this.Insert();
      else if (this.rbtnDelete.Checked)
        this.Delete();
      else
        this.Update();
    }

    private void Insert()
    {
      if (MessageBox.Show("Вы действительно хотите скопировать записи в выбранные объекты?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      int tariff_id = 0;
      int norm_id = 0;
      double param_value = 0.0;
      DateTime dBeg = new DateTime();
      DateTime dEnd = new DateTime();
      if ((int) this.GetVariables(ref tariff_id, ref norm_id, ref dBeg, ref dEnd, ref param_value) == -1)
        return;
      DateTime now = DateTime.Now;
      string str1 = "";
      foreach (Home checkedItem in this.chlbAddress.CheckedItems)
      {
        string str2 = this.rbtnService.Checked ? this.InsertSqlService(tariff_id, norm_id, dBeg, dEnd, checkedItem) : (this.rbtnSupplier.Checked ? this.InsertSqlSupplier(tariff_id, norm_id, dBeg, dEnd, checkedItem) : (this.rbtnQuality.Checked ? this.InsertSqlQuality(tariff_id, this.mpPeriod.Value, dBeg, dEnd, checkedItem) : this.InsertSqlServiceParam(tariff_id, param_value, dBeg, dEnd, checkedItem)));
        if (str2 != "")
          str1 += string.Format("{0}\n {1}\n", (object) checkedItem.Address, (object) str2);
      }
      int num = (int) MessageBox.Show(string.Format("Операция завершена. Время выполнения {0}\n {1} ", (object) (DateTime.Now - now), str1 != "" ? (object) string.Format("Возникли ошибки в следующих лицевых счетах \n {0}", (object) str1) : (object) ""), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private string InsertSqlSupplier(int tariff_id, int norm_id, DateTime dBeg, DateTime dEnd, Home home)
    {
      this.session.Clear();
      string str = "";
      Period period = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) KvrplHelper.FirstDay(this.mpPeriod.Value))).List<Period>()[0];
      foreach (object obj in (IEnumerable) this.session.CreateSQLQuery(this.GetLsClient(home)).List())
      {
        string queryString = string.Format(" Insert into LsSupplier(client_id,period_id,service_id,dbeg,dend,supplier_id,uname,dedit) values({0}, {7}, {1}, '{2}', '{3}', {4}, '{5}', '{6}') ", (object) (int) obj, (object) tariff_id, (object) KvrplHelper.DateToBaseFormat(dBeg), (object) KvrplHelper.DateToBaseFormat(dEnd), (object) (norm_id != 0 ? norm_id.ToString() : "null"), (object) Options.Login, (object) KvrplHelper.DateToBaseFormat(DateTime.Now), (object) (!this.PastTime ? 0 : period.PeriodId));
        try
        {
          this.session.CreateSQLQuery(queryString).ExecuteUpdate();
          this.session.Flush();
        }
        catch (Exception ex)
        {
          str += string.Format("{0} ", obj);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      return str;
    }

    private string InsertSqlQuality(int quality_id, DateTime period, DateTime dBeg, DateTime dEnd, Home home)
    {
      this.session.Clear();
      string str = "";
      foreach (object obj in (IEnumerable) this.session.CreateSQLQuery(this.GetLsClient(home)).List())
      {
        string queryString = string.Format(" Insert into LsQuality(client_id,period_id,quality_id,dbeg,dend,uname,dedit) values({0}, {1}, {2}, '{3}', '{4}', '{5}', '{6}') ", (object) (int) obj, (object) KvrplHelper.GetPeriod(period).PeriodId, (object) quality_id, (object) KvrplHelper.DateToBaseFormat(dBeg), (object) KvrplHelper.DateToBaseFormat(dEnd), (object) Options.Login, (object) KvrplHelper.DateToBaseFormat(DateTime.Now));
        try
        {
          this.session.CreateSQLQuery(queryString).ExecuteUpdate();
          this.session.Flush();
        }
        catch (Exception ex)
        {
          str += string.Format("{0} ", obj);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      return str;
    }

    private short GetVariables(ref int tariff_id, ref int norm_id, ref DateTime dBeg, ref DateTime dEnd, ref double param_value)
    {
      tariff_id = this.dgvService.CurrentRow == null || this.dgvService.Rows.Count <= 0 ? 0 : (int) this.dgvService.CurrentRow.Cells["Id"].Value;
      if (tariff_id == 0)
      {
        int num = (int) MessageBox.Show(this.rbtnService.Checked ? "Не выбран вариант услуги!" : (this.rbtnSupplier.Checked ? "Не выбрана составляющая услуги!" : (this.rbtnQuality.Checked ? "Не выбрана причина снижения качества!" : "Не выбран параметр!")), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return -1;
      }
      if (!this.rbtnParam.Checked)
      {
        try
        {
          norm_id = this.dgvNorm.CurrentRow == null || this.dgvNorm.Rows.Count <= 0 ? 0 : (int) this.dgvNorm.CurrentRow.Cells["Id"].Value;
        }
        catch
        {
          if (this.rbtnSupplier.Checked)
          {
            int num = (int) MessageBox.Show("Не выбран поставщик!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
        }
      }
      else
      {
        if (this.dgvNorm.CurrentRow != null && this.dgvNorm.Rows.Count > 0 && this.dgvNorm.CurrentRow.Cells["Id"].Value == null)
        {
          int num = (int) MessageBox.Show("Не внесено значение параметра", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return -1;
        }
        param_value = this.dgvNorm.CurrentRow == null || this.dgvNorm.Rows.Count <= 0 ? 0.0 : Convert.ToDouble(KvrplHelper.ChangeSeparator(this.dgvNorm.CurrentRow.Cells["Id"].Value.ToString()));
        if (!this.PastTime && tariff_id != 405 && param_value == 0.0)
        {
          int num = (int) MessageBox.Show("Значение не может быть равно нулю", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return -1;
        }
        if (param_value < 0.0)
        {
          int num = (int) MessageBox.Show("Значение не может быть меньше нуля", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return -1;
        }
      }
      if (this.rbtnSupplier.Checked && norm_id == 0)
      {
        int num = (int) MessageBox.Show("Не выбран поставщик услуги!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return -1;
      }
      dBeg = new DateTime();
      try
      {
        dBeg = Convert.ToDateTime(this.mtbDBeg.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Некорректная дата начала действия!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return -1;
      }
      dEnd = new DateTime();
      try
      {
        dEnd = Convert.ToDateTime(this.mtbDEnd.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Некорректная дата окончания действия!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return -1;
      }
      if (dBeg > dEnd && !this.rbtnUpdate.Checked)
      {
        int num = (int) MessageBox.Show("Дата начала превосходит дату окончания!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return -1;
      }
      if (!this.rbtnQuality.Checked)
      {
        DateTime dateTime1;
        if (!this.PastTime && dBeg < this.MonthClosed.PeriodName.Value.AddMonths(1))
        {
          if (this.rbtnUpdate.Checked)
          {
            DateTime dateTime2 = dEnd;
            dateTime1 = this.MonthClosed.PeriodName.Value.AddMonths(1);
            DateTime dateTime3 = dateTime1.AddDays(-1.0);
            if (dateTime2 < dateTime3)
            {
              int num = (int) MessageBox.Show("Даты принадлежат закрытому периоду!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            }
          }
          else
          {
            int num = (int) MessageBox.Show("Даты принадлежат закрытому периоду!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return -1;
          }
        }
        int num1;
        if (this.PastTime)
        {
          DateTime dateTime2 = dEnd;
          dateTime1 = this.MonthClosed.PeriodName.Value;
          DateTime dateTime3 = dateTime1.AddMonths(1);
          num1 = dateTime2 >= dateTime3 ? 1 : 0;
        }
        else
          num1 = 0;
        if (num1 != 0)
        {
          int num2 = (int) MessageBox.Show("Даты принадлежат открытому периоду!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return -1;
        }
        if (this.PastTime && KvrplHelper.FirstDay(this.mpPeriod.Value) <= this.MonthClosed.PeriodName.Value)
        {
          int num2 = (int) MessageBox.Show("Невозможно внести запись в закрытый период!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return -1;
        }
      }
      else
      {
        if (this.mpPeriod.Value <= this.MonthClosed.PeriodName.Value)
        {
          int num = (int) MessageBox.Show("Не могу занести запись в закрытый период!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return -1;
        }
        if (dBeg >= this.mpPeriod.Value.AddMonths(1))
        {
          int num = (int) MessageBox.Show("Дата начала не должна превышать последнего дня периода, в который вносится запись!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return -1;
        }
        if (dEnd >= this.mpPeriod.Value.AddMonths(1))
        {
          int num = (int) MessageBox.Show("Дата окончания не должна превышать последнего дня периода, в который вносится запись!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return -1;
        }
      }
      return 1;
    }

    private string InsertSqlService(int tariff_id, int norm_id, DateTime dBeg, DateTime dEnd, Home home)
    {
      this.session.Clear();
      string str = "";
      Period period = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) KvrplHelper.FirstDay(this.mpPeriod.Value))).List<Period>()[0];
      foreach (object obj in (IEnumerable) this.session.CreateSQLQuery(this.GetLsClient(home)).List())
      {
        string queryString = string.Format(" Insert into LsService(client_id,period_id,service_id,dbeg,dend,tariff_id,norm_id,complex_id,uname,dedit) values({8}, {9}, {0}, '{1}', '{2}', {3}, if " + (object) Convert.ToInt32(this.cbNorm.Checked) + "=1 and exists(select norm_id from lsService where period_id=0 and client_id={8} and service_id={0} and dbeg<='{1}' and dend>='{1}') then (select first norm_id from lsService where period_id=0 and client_id={8} and service_id={0} and dbeg<='{1}' and dend>='{1}') else {4} endif , {5}, '{6}', '{7}') ", (object) ((Service) this.cbService.SelectedItem).ServiceId, (object) KvrplHelper.DateToBaseFormat(dBeg), (object) KvrplHelper.DateToBaseFormat(dEnd), (object) tariff_id, (object) (norm_id != 0 ? norm_id.ToString() : "null"), (object) 100, (object) Options.Login, (object) KvrplHelper.DateToBaseFormat(DateTime.Now), (object) (int) obj, (object) (!this.PastTime ? 0 : period.PeriodId));
        try
        {
          this.session.CreateSQLQuery(queryString).ExecuteUpdate();
          this.session.Flush();
        }
        catch (Exception ex)
        {
          str += string.Format("{0} ", obj);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      return str;
    }

    private string InsertSqlServiceParam(int tariff_id, double norm_id, DateTime dBeg, DateTime dEnd, Home home)
    {
      this.session.Clear();
      string str = "";
      Period period = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) KvrplHelper.FirstDay(this.mpPeriod.Value))).List<Period>()[0];
      foreach (object obj in (IEnumerable) this.session.CreateSQLQuery(this.GetLsClient(home)).List())
      {
        string queryString = string.Format("Insert into LsServiceParam(client_id,period_id,service_id,param_id,dbeg,dend,param_value,uname,dedit) values({8}, {9}, {0}, {1}, '{2}', '{3}', :param_value, '{6}', '{7}') ", (object) ((Service) this.cbService.SelectedItem).ServiceId, (object) tariff_id, (object) KvrplHelper.DateToBaseFormat(dBeg), (object) KvrplHelper.DateToBaseFormat(dEnd), (object) (norm_id != 0.0 ? norm_id.ToString() : "null"), (object) 100, (object) Options.Login, (object) KvrplHelper.DateToBaseFormat(DateTime.Now), (object) (int) obj, (object) (!this.PastTime ? 0 : period.PeriodId));
        try
        {
          this.session.CreateSQLQuery(queryString).SetParameter<double>("param_value", norm_id).ExecuteUpdate();
          this.session.Flush();
        }
        catch (Exception ex)
        {
          str += string.Format("{0} ", obj);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      return str;
    }

    private void Delete()
    {
      if (MessageBox.Show("Вы действительно хотите удалить записи из выбранных объектов?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      int tariff_id = 0;
      int norm_id = 0;
      double param_value = 0.0;
      DateTime dBeg = new DateTime();
      DateTime dEnd = new DateTime();
      if ((int) this.GetVariables(ref tariff_id, ref norm_id, ref dBeg, ref dEnd, ref param_value) == -1)
        return;
      DateTime now = DateTime.Now;
      string str1 = "";
      foreach (Home checkedItem in this.chlbAddress.CheckedItems)
      {
        string str2 = this.rbtnService.Checked ? this.DeleteSqlService(tariff_id, norm_id, dBeg, dEnd, checkedItem) : (this.rbtnSupplier.Checked ? this.DeleteSqlSupplier(tariff_id, norm_id, dBeg, dEnd, checkedItem) : (this.rbtnQuality.Checked ? this.DeleteSqlQuality(tariff_id, this.mpPeriod.Value, dBeg, dEnd, checkedItem) : this.DeleteSqlServiceParam(tariff_id, param_value, dBeg, dEnd, checkedItem)));
      }
      int num = (int) MessageBox.Show(string.Format("Операция завершена. Время выполнения {0}\n {1} ", (object) (DateTime.Now - now), str1 != "" ? (object) string.Format("Возникли ошибки в следующих лицевых счетах \n {0}", (object) str1) : (object) ""), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private string DeleteSqlSupplier(int tariff_id, int norm_id, DateTime dBeg, DateTime dEnd, Home home)
    {
      this.session.Clear();
      string queryString = string.Format(" delete from LsSupplier where client_id in ({0})and period_id = 0 and service_id = {1} and dbeg='{2}' and dend= '{3}'  and supplier_id = {4}", (object) this.GetLsClient(home), (object) tariff_id, (object) KvrplHelper.DateToBaseFormat(dBeg), (object) KvrplHelper.DateToBaseFormat(dEnd), (object) norm_id);
      try
      {
        this.session.CreateSQLQuery(queryString).ExecuteUpdate();
        this.session.Flush();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      return "";
    }

    private string DeleteSqlQuality(int tariff_id, DateTime period, DateTime dBeg, DateTime dEnd, Home home)
    {
      this.session.Clear();
      string queryString = string.Format(" delete from LsQuality where client_id in ({0})and period_id = {1} and quality_id = {2} and dbeg='{3}' and dend= '{4}' ", (object) this.GetLsClient(home), (object) KvrplHelper.GetPeriod(period).PeriodId, (object) tariff_id, (object) KvrplHelper.DateToBaseFormat(dBeg), (object) KvrplHelper.DateToBaseFormat(dEnd));
      try
      {
        this.session.CreateSQLQuery(queryString).ExecuteUpdate();
        this.session.Flush();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      return "";
    }

    private string DeleteSqlService(int tariff_id, int norm_id, DateTime dBeg, DateTime dEnd, Home home)
    {
      this.session.Clear();
      string queryString = string.Format(" delete from LsService where client_id in ({0})and period_id = 0 and service_id = {1} and dbeg='{2}' and dend= '{3}'  and tariff_id = {4} and {5} and complex_id = {6}", (object) this.GetLsClient(home), (object) ((Service) this.cbService.SelectedItem).ServiceId, (object) KvrplHelper.DateToBaseFormat(dBeg), (object) KvrplHelper.DateToBaseFormat(dEnd), (object) tariff_id, (object) (norm_id != 0 ? "norm_id=" + norm_id.ToString() : " norm_id is null"), (object) 100, (object) Options.Login);
      try
      {
        this.session.CreateSQLQuery(queryString).ExecuteUpdate();
        this.session.Flush();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      return "";
    }

    private string DeleteSqlServiceParam(int tariff_id, double norm_id, DateTime dBeg, DateTime dEnd, Home home)
    {
      this.session.Clear();
      string queryString = string.Format(" delete from LsServiceParam where client_id in ({0})and period_id = 0 and service_id = {1} and dbeg='{2}' and dend= '{3}'  and param_id = {4} and param_value=:param_value", (object) this.GetLsClient(home), (object) ((Service) this.cbService.SelectedItem).ServiceId, (object) KvrplHelper.DateToBaseFormat(dBeg), (object) KvrplHelper.DateToBaseFormat(dEnd), (object) tariff_id);
      try
      {
        this.session.CreateSQLQuery(queryString).SetParameter<double>("param_value", norm_id).ExecuteUpdate();
        this.session.Flush();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      return "";
    }

    private new void Update()
    {
      if (MessageBox.Show("Вы действительно хотите обновить записи в выбранных объектах?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      int tariff_id = 0;
      int norm_id = 0;
      double param_value = 0.0;
      DateTime dBeg = new DateTime();
      DateTime dEnd = new DateTime();
      if ((int) this.GetVariables(ref tariff_id, ref norm_id, ref dBeg, ref dEnd, ref param_value) == -1)
        return;
      DateTime now = DateTime.Now;
      string str1 = "";
      foreach (Home checkedItem in this.chlbAddress.CheckedItems)
      {
        string str2 = this.rbtnService.Checked ? this.UpdateSqlService(tariff_id, norm_id, dBeg, dEnd, checkedItem) : (this.rbtnSupplier.Checked ? this.UpdateSqlSupplier(tariff_id, norm_id, dBeg, dEnd, checkedItem) : (this.rbtnQuality.Checked ? "" : this.UpdateSqlServiceParam(tariff_id, param_value, dBeg, dEnd, checkedItem)));
      }
      int num = (int) MessageBox.Show(string.Format("Операция завершена. Время выполнения {0}\n {1} ", (object) (DateTime.Now - now), str1 != "" ? (object) string.Format("Возникли ошибки в следующих лицевых счетах \n {0}", (object) str1) : (object) ""), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private string UpdateSqlSupplier(int tariff_id, int norm_id, DateTime dBeg, DateTime dEnd, Home home)
    {
      this.session.Clear();
      string queryString = string.Format(" update LsSupplier set dend= '{3}',uname='{5}' where client_id in ({0}) and period_id = 0 and service_id = {1} and dbeg<='{3}' and dend>='{2}' and supplier_id = {4}", (object) this.GetLsClient(home), (object) tariff_id, (object) KvrplHelper.DateToBaseFormat(this.MonthClosed.PeriodName.Value.AddMonths(1)), (object) KvrplHelper.DateToBaseFormat(dEnd), (object) norm_id, (object) Options.Login);
      try
      {
        this.session.CreateSQLQuery(queryString).ExecuteUpdate();
        this.session.Flush();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      return "";
    }

    private string UpdateSqlService(int tariff_id, int norm_id, DateTime dBeg, DateTime dEnd, Home home)
    {
      this.session.Clear();
      string queryString = string.Format(" update LsService set dend= '{3}',uname='{7}' where client_id in ({0}) and period_id = 0 and service_id = {1} and dbeg<='{3}' and dend>='{2}' and tariff_id = {4} and {5}", (object) this.GetLsClient(home), (object) ((Service) this.cbService.SelectedItem).ServiceId, (object) KvrplHelper.DateToBaseFormat(this.MonthClosed.PeriodName.Value.AddMonths(1)), (object) KvrplHelper.DateToBaseFormat(dEnd), (object) tariff_id, (object) (norm_id != 0 ? "norm_id=" + norm_id.ToString() : " norm_id is null"), (object) 100, (object) Options.Login);
      try
      {
        this.session.CreateSQLQuery(queryString).ExecuteUpdate();
        this.session.Flush();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      return "";
    }

    private string UpdateSqlServiceParam(int tariff_id, double norm_id, DateTime dBeg, DateTime dEnd, Home home)
    {
      this.session.Clear();
      string queryString = string.Format("update LsServiceParam set dend= '{3}',uname='{7}',dedit='{8}' where client_id in ({0}) and period_id = 0 and service_id = {1} and dbeg<='{3}' and dend>='{2}' and param_id = {4}", (object) this.GetLsClient(home), (object) ((Service) this.cbService.SelectedItem).ServiceId, (object) KvrplHelper.DateToBaseFormat(this.MonthClosed.PeriodName.Value.AddMonths(1)), (object) KvrplHelper.DateToBaseFormat(dEnd), (object) tariff_id, (object) (norm_id != 0.0 ? "param_value=" + norm_id.ToString() : " param_value is null"), (object) 100, (object) Options.Login, (object) KvrplHelper.DateToBaseFormat(DateTime.Now));
      try
      {
        this.session.CreateSQLQuery(queryString).ExecuteUpdate();
        this.session.Flush();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      return "";
    }

    private string GetLsClient(Home home)
    {
      Period nextPeriod = KvrplHelper.GetNextPeriod(this.MonthClosed);
      string str1;
      if (!this.chbArhive.Checked && !this.rbtnPrivat.Checked && (!this.rbtnNotPrivat.Checked && !this.chbUchet.Checked))
      {
        str1 = "";
      }
      else
      {
        string format = " and p.DBeg <= '{0}' and p.DEnd >= '{1}' ";
        DateTime? periodName = nextPeriod.PeriodName;
        string baseFormat1 = KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(periodName.Value));
        periodName = nextPeriod.PeriodName;
        string baseFormat2 = KvrplHelper.DateToBaseFormat(periodName.Value);
        str1 = string.Format(format, (object) baseFormat1, (object) baseFormat2);
      }
      string str2 = str1;
      string str3 = this.chbArhive.Checked ? string.Format(" and exists (select p.Client_Id from DBA.lsParam p where p.Param_Id=107 and p.Client_Id=ls.Client_Id and p.Param_Value not in (4,5) {0})", (object) str2) : "";
      string str4 = this.rbtnPrivat.Checked ? string.Format(" and exists (select p.Client_Id from DBA.lsParam p where p.Param_Id=104 and p.Client_Id=ls.Client_Id and p.Param_Value in (2,5,6,7,8,12,14,15,17,18,21) {0})", (object) str2) : "";
      string str5 = this.rbtnNotPrivat.Checked ? string.Format(" and exists (select p.Client_Id from DBA.lsParam p where p.Param_Id=104 and p.Client_Id=ls.Client_Id and p.Param_Value not in (2,5,6,7,8,12,14,15,17,18,21) {0})", (object) str2) : "";
      string str6 = this.chbUchet.Checked ? string.Format(" and exists (select p.Client_Id from DBA.lsParam p where p.Param_Id=107 and p.Client_Id=ls.Client_Id and p.Param_Value not in (3) {0})", (object) str2) : "";
      string str7 = this.rbKv.Checked ? "and ls.Complex_id = 100" : "";
      string str8 = this.rbAr.Checked ? "and ls.Complex_id = 110" : "";
      string str9 = this.rbWith.Checked || this.rbWithout.Checked ? string.Format(" (select p.client_id from lsServiceParam p where p.period_id=0 {0} and p.service_id={1} and p.param_id=405 " + (Convert.ToInt32(this.cmbType.SelectedValue) != 0 ? string.Format(" and p.param_value={0}", (object) ((Scheme) this.cmbType.SelectedItem).SchemeId) : "") + ")", (object) str2, (object) ((Service) this.cbService.SelectedItem).ServiceId) : "";
      if (this.rbWith.Checked)
        str9 = "  and ls.client_id in" + str9;
      if (this.rbWithout.Checked)
        str9 = "  and ls.client_id not in" + str9;
      string str10;
      if (!this.chbArhive.Checked && !this.rbtnPrivat.Checked && (!this.rbtnNotPrivat.Checked && !this.chbUchet.Checked))
        str10 = string.Format("select client_id from DBA.lsClient ls where ls.IdHome = {0} and ls.company_id={1} " + Options.MainConditions2, (object) home.IdHome, (object) this.CurrentCompany.CompanyId);
      else
        str10 = string.Format("select client_id from DBA.lsClient ls  where ls.IdHome = {0} and ls.company_id={5} " + Options.MainConditions2 + " {1} {2} {3} {4} {6} {7}" + str9 ?? "", (object) home.IdHome, (object) str3, (object) str4, (object) str5, (object) str6, (object) this.CurrentCompany.CompanyId, (object) str7, (object) str8);
      return str10;
    }

    private void rbtnService_CheckedChanged(object sender, EventArgs e)
    {
      this.UpdateType(true);
      this.LoadServiceVar();
    }

    private void rbtnSupplier_CheckedChanged(object sender, EventArgs e)
    {
      this.UpdateType(false);
      this.LoadSupplier();
    }

    private void rbtnQuality_CheckedChanged(object sender, EventArgs e)
    {
      this.UpdateType(false);
      this.LoadQuality();
    }

    private void rbtnParam_CheckedChanged(object sender, EventArgs e)
    {
      this.UpdateType(true);
      if (!this.rbtnParam.Checked)
        return;
      this.LoadServiceParam();
    }

    private void UpdateType(bool enabled)
    {
      this.gbType.Enabled = enabled;
      this.rbWith.Checked = false;
      this.rbWithout.Checked = false;
    }

    private void LoadDgvSostService()
    {
      if (this.cbService.SelectedItem == null)
        return;
      IList<Service> serviceList = this.session.CreateCriteria(typeof (Service)).Add((ICriterion) Restrictions.Not((ICriterion) Restrictions.Eq("ServiceId", (object) ((Service) this.cbService.SelectedItem).ServiceId))).Add((ICriterion) Restrictions.Eq("Root", (object) ((Service) this.cbService.SelectedItem).ServiceId)).AddOrder(Order.Asc("Root")).List<Service>();
      DataTable dataTable = new DataTable("temp");
      dataTable.Columns.Add("Наименование", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Id", System.Type.GetType("System.Int32"));
      foreach (Service service in (IEnumerable<Service>) serviceList)
        dataTable.Rows.Add((object) service.ServiceName, (object) service.ServiceId);
      this.dgvService.DataSource = (object) dataTable;
      this.dgvService.Columns["Наименование"].Width = 200;
      this.dgvService.Columns["Id"].Visible = false;
    }

    private void LoadDgvSupplier()
    {
      if (this.cbService.SelectedItem == null)
        return;
      IList<Supplier> supplierList = this.session.CreateQuery(string.Format("select d from CmpSupplier s, Supplier d where s.SupplierOrg.SupplierId = d.SupplierId and s.Service.ServiceId={0} and s.Company.CompanyId={1}", (object) ((Service) this.cbService.SelectedItem).ServiceId, (object) Convert.ToInt32((object) KvrplHelper.GetMainCompanyParam(this.CurrentCompany, 211).Param_value))).List<Supplier>();
      DataTable dataTable = new DataTable("temp");
      dataTable.Columns.Add("Получатель", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Исполнитель", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Id", System.Type.GetType("System.Int32"));
      foreach (Supplier supplier in (IEnumerable<Supplier>) supplierList)
        dataTable.Rows.Add((object) supplier.Recipient.NameOrgMin, (object) supplier.Perfomer.NameOrgMin, (object) supplier.SupplierId);
      this.dgvNorm.DataSource = (object) dataTable;
      this.dgvNorm.Columns["Получатель"].Width = 160;
      this.dgvNorm.Columns["Исполнитель"].Width = 160;
      this.dgvNorm.Columns["Id"].Visible = false;
    }

    private void LoadDgvQuality()
    {
      int serviceId = (int) ((Service) this.cmbSost.SelectedItem).ServiceId;
      if (serviceId == 0)
        serviceId = (int) ((Service) this.cbService.SelectedItem).ServiceId;
      if ((uint) serviceId <= 0U)
        return;
      IList<Quality> qualityList = this.session.CreateQuery(string.Format("from Quality where Company_id = {0} and Service_id={1} order by Quality_id", (object) this.CurrentCompany.CompanyId, (object) serviceId)).List<Quality>();
      DataTable dataTable = new DataTable("temp");
      dataTable.Columns.Add("Наименование", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Коэффициент", System.Type.GetType("System.Double"));
      dataTable.Columns.Add("Id", System.Type.GetType("System.Int32"));
      foreach (Quality quality in (IEnumerable<Quality>) qualityList)
        dataTable.Rows.Add((object) quality.Quality_name, (object) quality.Coeff, (object) quality.Quality_id);
      this.dgvService.DataSource = (object) dataTable;
      this.dgvService.Columns["Наименование"].Width = 200;
      this.dgvService.Columns["Id"].Visible = false;
    }

    private void LoadDgvParam()
    {
      int serviceId = (int) ((Service) this.cbService.SelectedItem).ServiceId;
      IList<Param> objList = this.session.CreateQuery("from Param where Param_type = 4 order by Sorter").List<Param>();
      DataTable dataTable = new DataTable("temp");
      dataTable.Columns.Add("Наименование", System.Type.GetType("System.String"));
      dataTable.Columns.Add("Id", System.Type.GetType("System.Int32"));
      foreach (Param obj in (IEnumerable<Param>) objList)
        dataTable.Rows.Add((object) obj.ParamName, (object) obj.ParamId);
      this.dgvService.DataSource = (object) dataTable;
      this.dgvService.Columns["Наименование"].Width = 300;
      this.dgvService.Columns["Id"].Visible = false;
    }

    private void LoadDgvParamValue()
    {
      this.lblGrid2.Text = "Значение параметра";
      this.dgvNorm.Columns.Clear();
      this.dgvNorm.DataSource = (object) null;
      KvrplHelper.AddTextBoxColumn(this.dgvNorm, 0, "Значение", "Id", 300, false);
      IList<AdmTbl> admTblList = this.session.CreateQuery(string.Format("select s from AdmTbl s,ParamRelation r where s.TableId=r.TableId and r.ParamId={0}", this.dgvService.Rows[this.dgvService.CurrentRow.Index].Cells["Id"].Value)).List<AdmTbl>();
      if (admTblList.Count > 0)
      {
        if (admTblList[0].ClassName == null)
          return;
        try
        {
          DataGridViewComboBoxCell viewComboBoxCell = new DataGridViewComboBoxCell();
          viewComboBoxCell.DisplayStyleForCurrentCellOnly = true;
          viewComboBoxCell.ValueMember = admTblList[0].ClassNameId;
          viewComboBoxCell.DisplayMember = admTblList[0].ClassNameName;
          string str = "";
          if (Convert.ToInt32(this.dgvService.Rows[this.dgvService.CurrentRow.Index].Cells["Id"].Value) == 405)
            str = " where SchemeType=6";
          viewComboBoxCell.DataSource = (object) this.session.CreateQuery(string.Format("from {0}" + str, (object) admTblList[0].ClassName)).List();
          viewComboBoxCell.ValueType = typeof (int);
          this.session.CreateQuery(string.Format("from {0}" + str, (object) admTblList[0].ClassName)).List();
          this.dgvNorm.Rows[0].Cells["Id"] = (DataGridViewCell) viewComboBoxCell;
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      else
        this.dgvNorm.Rows[0].Cells["Id"] = (DataGridViewCell) new DataGridViewTextBoxCell();
    }

    private void btnPastTime_Click(object sender, EventArgs e)
    {
      if (!this.PastTime)
      {
        this.btnPastTime.BackColor = Color.DarkOrange;
        this.PastTime = true;
        this.pnPeriod.Visible = true;
      }
      else
      {
        this.btnPastTime.BackColor = this.pnBtn.BackColor;
        this.PastTime = false;
        this.pnPeriod.Visible = false;
      }
      this.LoadDate();
    }

    private void mpPeriod_ValueChanged(object sender, EventArgs e)
    {
    }

    private void dgvService_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    private void label5_Click(object sender, EventArgs e)
    {
    }

    private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (!this.rbtnQuality.Checked)
        return;
      this.LoadDgvQuality();
    }

    private void dgvService_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (!this.rbtnParam.Checked || this.dgvService.CurrentCell.ColumnIndex != this.dgvService.Rows[this.dgvService.CurrentRow.Index].Cells["Наименование"].ColumnIndex)
        return;
      if (Convert.ToInt32(this.dgvService.Rows[this.dgvService.CurrentRow.Index].Cells["Id"].Value) == 405)
      {
        short num = 0;
        if ((int) ((Service) this.cbService.SelectedItem).ServiceId != -1)
          num = ((Service) this.cbService.SelectedItem).ServiceId;
      }
      IList<AdmTbl> admTblList = this.session.CreateQuery(string.Format("select s from AdmTbl s,ParamRelation r where s.TableId=r.TableId and r.ParamId={0}", this.dgvService.Rows[this.dgvService.CurrentRow.Index].Cells["Id"].Value)).List<AdmTbl>();
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
            if (Convert.ToInt32(this.dgvService.Rows[this.dgvService.CurrentRow.Index].Cells["Id"].Value) == 405)
              str = " where SchemeType=6";
            viewComboBoxCell.DataSource = (object) this.session.CreateQuery(string.Format("from {0}" + str, (object) admTblList[0].ClassName)).List();
            viewComboBoxCell.ValueType = typeof (int);
            this.dgvNorm.Rows[0].Cells["Id"] = (DataGridViewCell) viewComboBoxCell;
          }
          catch (Exception ex)
          {
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
        }
      }
      else
        this.dgvNorm.Rows[0].Cells["Id"] = (DataGridViewCell) new DataGridViewTextBoxCell();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmMassChooseObject));
      this.pnBtn = new Panel();
      this.btnPastTime = new Button();
      this.btnExit = new Button();
      this.btnStart = new Button();
      this.panelChoose = new Panel();
      this.groupBox4 = new GroupBox();
      this.gbType = new GroupBox();
      this.rbWithout = new RadioButton();
      this.cmbType = new ComboBox();
      this.rbWith = new RadioButton();
      this.gbLs = new GroupBox();
      this.rbAr = new RadioButton();
      this.rbKv = new RadioButton();
      this.chbUchet = new CheckBox();
      this.chbArhive = new CheckBox();
      this.gpbFlat = new GroupBox();
      this.rbtnNotPrivat = new RadioButton();
      this.rbtnPrivat = new RadioButton();
      this.groupBox1 = new GroupBox();
      this.rbtnParam = new RadioButton();
      this.rbtnQuality = new RadioButton();
      this.rbtnSupplier = new RadioButton();
      this.rbtnService = new RadioButton();
      this.groupBox3 = new GroupBox();
      this.rbtnUpdate = new RadioButton();
      this.rbtnDelete = new RadioButton();
      this.rbtnInsert = new RadioButton();
      this.dgvService = new DataGridView();
      this.lblGrid2 = new Label();
      this.pnNorm = new Panel();
      this.cbNorm = new CheckBox();
      this.dgvNorm = new DataGridView();
      this.panel3 = new Panel();
      this.lblSost = new Label();
      this.cmbSost = new ComboBox();
      this.lblGrid1 = new Label();
      this.label1 = new Label();
      this.label3 = new Label();
      this.mtbDEnd = new MaskedTextBox();
      this.cbService = new ComboBox();
      this.mtbDBeg = new MaskedTextBox();
      this.label2 = new Label();
      this.pnPeriod = new Panel();
      this.label4 = new Label();
      this.mpPeriod = new MonthPicker();
      this.btnCheck = new Button();
      this.groupBox2 = new GroupBox();
      this.chlbAddress = new CheckedListBox();
      this.hp = new HelpProvider();
      this.splitContainer1 = new SplitContainer();
      this.splitContainer2 = new SplitContainer();
      this.pnBtn.SuspendLayout();
      this.panelChoose.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.gbType.SuspendLayout();
      this.gbLs.SuspendLayout();
      this.gpbFlat.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupBox3.SuspendLayout();
      ((ISupportInitialize) this.dgvService).BeginInit();
      this.pnNorm.SuspendLayout();
      ((ISupportInitialize) this.dgvNorm).BeginInit();
      this.panel3.SuspendLayout();
      this.pnPeriod.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnPastTime);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Controls.Add((Control) this.btnStart);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 653);
      this.pnBtn.Margin = new Padding(4);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(1201, 39);
      this.pnBtn.TabIndex = 0;
      this.btnPastTime.Image = (Image) Resources.time_24;
      this.btnPastTime.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnPastTime.Location = new Point(148, 4);
      this.btnPastTime.Name = "btnPastTime";
      this.btnPastTime.Size = new Size(147, 30);
      this.btnPastTime.TabIndex = 4;
      this.btnPastTime.Text = "Прошлое время";
      this.btnPastTime.TextAlign = ContentAlignment.MiddleRight;
      this.btnPastTime.UseVisualStyleBackColor = true;
      this.btnPastTime.Click += new EventHandler(this.btnPastTime_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(1104, 4);
      this.btnExit.Margin = new Padding(4);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(84, 30);
      this.btnExit.TabIndex = 3;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.btnStart.Image = (Image) Resources.Configure;
      this.btnStart.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnStart.Location = new Point(13, 4);
      this.btnStart.Margin = new Padding(4);
      this.btnStart.Name = "btnStart";
      this.btnStart.Size = new Size(112, 30);
      this.btnStart.TabIndex = 2;
      this.btnStart.Text = "Выполнить";
      this.btnStart.TextAlign = ContentAlignment.MiddleRight;
      this.btnStart.UseVisualStyleBackColor = true;
      this.btnStart.Click += new EventHandler(this.btnStart_Click);
      this.panelChoose.Controls.Add((Control) this.groupBox1);
      this.panelChoose.Controls.Add((Control) this.groupBox3);
      this.panelChoose.Controls.Add((Control) this.groupBox4);
      this.panelChoose.Dock = DockStyle.Fill;
      this.panelChoose.Location = new Point(0, 0);
      this.panelChoose.Name = "panelChoose";
      this.panelChoose.Size = new Size(319, 653);
      this.panelChoose.TabIndex = 2;
      this.groupBox4.Controls.Add((Control) this.gbType);
      this.groupBox4.Controls.Add((Control) this.gbLs);
      this.groupBox4.Controls.Add((Control) this.chbUchet);
      this.groupBox4.Controls.Add((Control) this.chbArhive);
      this.groupBox4.Controls.Add((Control) this.gpbFlat);
      this.groupBox4.Dock = DockStyle.Top;
      this.groupBox4.Location = new Point(0, 0);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new Size(319, 372);
      this.groupBox4.TabIndex = 3;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Выбор";
      this.gbType.Controls.Add((Control) this.rbWithout);
      this.gbType.Controls.Add((Control) this.cmbType);
      this.gbType.Controls.Add((Control) this.rbWith);
      this.gbType.Dock = DockStyle.Top;
      this.gbType.Location = new Point(3, 223);
      this.gbType.Name = "gbType";
      this.gbType.Size = new Size(313, 70);
      this.gbType.TabIndex = 19;
      this.gbType.TabStop = false;
      this.gbType.Text = "Тип расчета";
      this.rbWithout.AutoSize = true;
      this.rbWithout.Location = new Point(14, 45);
      this.rbWithout.Name = "rbWithout";
      this.rbWithout.Size = new Size(85, 21);
      this.rbWithout.TabIndex = 1;
      this.rbWithout.TabStop = true;
      this.rbWithout.Text = "Без типа";
      this.rbWithout.UseVisualStyleBackColor = true;
      this.cmbType.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.cmbType.FormattingEnabled = true;
      this.cmbType.Location = new Point(99, 20);
      this.cmbType.Name = "cmbType";
      this.cmbType.Size = new Size(211, 24);
      this.cmbType.TabIndex = 3;
      this.rbWith.AutoSize = true;
      this.rbWith.Location = new Point(14, 20);
      this.rbWith.Name = "rbWith";
      this.rbWith.Size = new Size(79, 21);
      this.rbWith.TabIndex = 0;
      this.rbWith.TabStop = true;
      this.rbWith.Text = "С типом";
      this.rbWith.UseVisualStyleBackColor = true;
      this.gbLs.Controls.Add((Control) this.rbAr);
      this.gbLs.Controls.Add((Control) this.rbKv);
      this.gbLs.Dock = DockStyle.Top;
      this.gbLs.Location = new Point(3, 145);
      this.gbLs.Name = "gbLs";
      this.gbLs.Size = new Size(313, 78);
      this.gbLs.TabIndex = 18;
      this.gbLs.TabStop = false;
      this.gbLs.Text = "Лицевые";
      this.gbLs.Visible = false;
      this.rbAr.AutoSize = true;
      this.rbAr.Location = new Point(13, 49);
      this.rbAr.Name = "rbAr";
      this.rbAr.Size = new Size(77, 21);
      this.rbAr.TabIndex = 1;
      this.rbAr.TabStop = true;
      this.rbAr.Text = "Аренды";
      this.rbAr.UseVisualStyleBackColor = true;
      this.rbKv.AutoSize = true;
      this.rbKv.Location = new Point(13, 22);
      this.rbKv.Name = "rbKv";
      this.rbKv.Size = new Size(106, 21);
      this.rbKv.TabIndex = 0;
      this.rbKv.TabStop = true;
      this.rbKv.Text = "Квартплаты";
      this.rbKv.UseVisualStyleBackColor = true;
      this.chbUchet.AutoSize = true;
      this.chbUchet.Dock = DockStyle.Top;
      this.chbUchet.Location = new Point(3, 124);
      this.chbUchet.Name = "chbUchet";
      this.chbUchet.Size = new Size(313, 21);
      this.chbUchet.TabIndex = 17;
      this.chbUchet.Text = "Без учетных л.с.";
      this.chbUchet.UseVisualStyleBackColor = true;
      this.chbArhive.AutoSize = true;
      this.chbArhive.Dock = DockStyle.Top;
      this.chbArhive.Location = new Point(3, 103);
      this.chbArhive.Name = "chbArhive";
      this.chbArhive.Size = new Size(313, 21);
      this.chbArhive.TabIndex = 15;
      this.chbArhive.Text = "Без закрытых и архивных л.с.";
      this.chbArhive.UseVisualStyleBackColor = true;
      this.gpbFlat.Controls.Add((Control) this.rbtnNotPrivat);
      this.gpbFlat.Controls.Add((Control) this.rbtnPrivat);
      this.gpbFlat.Dock = DockStyle.Top;
      this.gpbFlat.Location = new Point(3, 19);
      this.gpbFlat.Name = "gpbFlat";
      this.gpbFlat.Size = new Size(313, 84);
      this.gpbFlat.TabIndex = 16;
      this.gpbFlat.TabStop = false;
      this.gpbFlat.Text = "Квартира";
      this.rbtnNotPrivat.AutoSize = true;
      this.rbtnNotPrivat.Location = new Point(13, 51);
      this.rbtnNotPrivat.Name = "rbtnNotPrivat";
      this.rbtnNotPrivat.Size = new Size(178, 21);
      this.rbtnNotPrivat.TabIndex = 1;
      this.rbtnNotPrivat.TabStop = true;
      this.rbtnNotPrivat.Text = "Неприватизированные";
      this.rbtnNotPrivat.UseVisualStyleBackColor = true;
      this.rbtnPrivat.AutoSize = true;
      this.rbtnPrivat.Location = new Point(13, 23);
      this.rbtnPrivat.Name = "rbtnPrivat";
      this.rbtnPrivat.Size = new Size(162, 21);
      this.rbtnPrivat.TabIndex = 0;
      this.rbtnPrivat.TabStop = true;
      this.rbtnPrivat.Text = "Приватизированные";
      this.rbtnPrivat.UseVisualStyleBackColor = true;
      this.groupBox1.Controls.Add((Control) this.rbtnParam);
      this.groupBox1.Controls.Add((Control) this.rbtnQuality);
      this.groupBox1.Controls.Add((Control) this.rbtnSupplier);
      this.groupBox1.Controls.Add((Control) this.rbtnService);
      this.groupBox1.Dock = DockStyle.Top;
      this.groupBox1.Location = new Point(0, 474);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(319, (int) sbyte.MaxValue);
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Объекты";
      this.rbtnParam.AutoSize = true;
      this.rbtnParam.Location = new Point(8, 102);
      this.rbtnParam.Name = "rbtnParam";
      this.rbtnParam.Size = new Size(177, 21);
      this.rbtnParam.TabIndex = 3;
      this.rbtnParam.TabStop = true;
      this.rbtnParam.Text = "Параметры по услугам";
      this.rbtnParam.UseVisualStyleBackColor = true;
      this.rbtnParam.CheckedChanged += new EventHandler(this.rbtnParam_CheckedChanged);
      this.rbtnQuality.AutoSize = true;
      this.rbtnQuality.Location = new Point(8, 77);
      this.rbtnQuality.Name = "rbtnQuality";
      this.rbtnQuality.Size = new Size(88, 21);
      this.rbtnQuality.TabIndex = 2;
      this.rbtnQuality.TabStop = true;
      this.rbtnQuality.Text = "Качество";
      this.rbtnQuality.UseVisualStyleBackColor = true;
      this.rbtnQuality.CheckedChanged += new EventHandler(this.rbtnQuality_CheckedChanged);
      this.rbtnSupplier.AutoSize = true;
      this.rbtnSupplier.Location = new Point(8, 50);
      this.rbtnSupplier.Name = "rbtnSupplier";
      this.rbtnSupplier.Size = new Size(107, 21);
      this.rbtnSupplier.TabIndex = 1;
      this.rbtnSupplier.TabStop = true;
      this.rbtnSupplier.Text = "Поставщики";
      this.rbtnSupplier.UseVisualStyleBackColor = true;
      this.rbtnSupplier.CheckedChanged += new EventHandler(this.rbtnSupplier_CheckedChanged);
      this.rbtnService.AutoSize = true;
      this.rbtnService.Location = new Point(7, 23);
      this.rbtnService.Name = "rbtnService";
      this.rbtnService.Size = new Size(130, 21);
      this.rbtnService.TabIndex = 0;
      this.rbtnService.TabStop = true;
      this.rbtnService.Text = "Варианты услуг";
      this.rbtnService.UseVisualStyleBackColor = true;
      this.rbtnService.CheckedChanged += new EventHandler(this.rbtnService_CheckedChanged);
      this.groupBox3.Controls.Add((Control) this.rbtnUpdate);
      this.groupBox3.Controls.Add((Control) this.rbtnDelete);
      this.groupBox3.Controls.Add((Control) this.rbtnInsert);
      this.groupBox3.Dock = DockStyle.Top;
      this.groupBox3.Location = new Point(0, 372);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(319, 102);
      this.groupBox3.TabIndex = 1;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Вид операции";
      this.rbtnUpdate.AutoSize = true;
      this.rbtnUpdate.Location = new Point(7, 77);
      this.rbtnUpdate.Name = "rbtnUpdate";
      this.rbtnUpdate.Size = new Size(108, 21);
      this.rbtnUpdate.TabIndex = 2;
      this.rbtnUpdate.Text = "Обновление";
      this.rbtnUpdate.UseVisualStyleBackColor = true;
      this.rbtnDelete.AutoSize = true;
      this.rbtnDelete.Location = new Point(7, 50);
      this.rbtnDelete.Name = "rbtnDelete";
      this.rbtnDelete.Size = new Size(91, 21);
      this.rbtnDelete.TabIndex = 1;
      this.rbtnDelete.Text = "Удаление";
      this.rbtnDelete.UseVisualStyleBackColor = true;
      this.rbtnInsert.AutoSize = true;
      this.rbtnInsert.Checked = true;
      this.rbtnInsert.Location = new Point(7, 23);
      this.rbtnInsert.Name = "rbtnInsert";
      this.rbtnInsert.Size = new Size(79, 21);
      this.rbtnInsert.TabIndex = 0;
      this.rbtnInsert.TabStop = true;
      this.rbtnInsert.Text = "Вставка";
      this.rbtnInsert.UseVisualStyleBackColor = true;
      this.dgvService.BackgroundColor = Color.AliceBlue;
      this.dgvService.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvService.Dock = DockStyle.Fill;
      this.dgvService.Location = new Point(0, 193);
      this.dgvService.Name = "dgvService";
      this.dgvService.ReadOnly = true;
      this.dgvService.RowHeadersVisible = false;
      this.dgvService.Size = new Size(318, 133);
      this.dgvService.TabIndex = 17;
      this.dgvService.CellClick += new DataGridViewCellEventHandler(this.dgvService_CellClick);
      this.dgvService.DataError += new DataGridViewDataErrorEventHandler(this.dgvService_DataError);
      this.lblGrid2.Dock = DockStyle.Bottom;
      this.lblGrid2.Location = new Point(0, 326);
      this.lblGrid2.Name = "lblGrid2";
      this.lblGrid2.Size = new Size(318, 23);
      this.lblGrid2.TabIndex = 16;
      this.lblGrid2.Text = "Нормативы";
      this.pnNorm.Controls.Add((Control) this.cbNorm);
      this.pnNorm.Dock = DockStyle.Bottom;
      this.pnNorm.Location = new Point(0, 349);
      this.pnNorm.Name = "pnNorm";
      this.pnNorm.Size = new Size(318, 28);
      this.pnNorm.TabIndex = 14;
      this.cbNorm.AutoSize = true;
      this.cbNorm.Checked = true;
      this.cbNorm.CheckState = CheckState.Checked;
      this.cbNorm.Location = new Point(3, 4);
      this.cbNorm.Name = "cbNorm";
      this.cbNorm.Size = new Size(256, 21);
      this.cbNorm.TabIndex = 0;
      this.cbNorm.Text = "Сохранить действующий норматив";
      this.cbNorm.UseVisualStyleBackColor = true;
      this.dgvNorm.BackgroundColor = Color.AliceBlue;
      this.dgvNorm.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvNorm.Dock = DockStyle.Bottom;
      this.dgvNorm.Location = new Point(0, 377);
      this.dgvNorm.Name = "dgvNorm";
      this.dgvNorm.ReadOnly = true;
      this.dgvNorm.RowHeadersVisible = false;
      this.dgvNorm.Size = new Size(318, 276);
      this.dgvNorm.TabIndex = 13;
      this.dgvNorm.DataError += new DataGridViewDataErrorEventHandler(this.dgvService_DataError);
      this.panel3.Controls.Add((Control) this.lblSost);
      this.panel3.Controls.Add((Control) this.cmbSost);
      this.panel3.Controls.Add((Control) this.lblGrid1);
      this.panel3.Controls.Add((Control) this.label1);
      this.panel3.Controls.Add((Control) this.label3);
      this.panel3.Controls.Add((Control) this.mtbDEnd);
      this.panel3.Controls.Add((Control) this.cbService);
      this.panel3.Controls.Add((Control) this.mtbDBeg);
      this.panel3.Controls.Add((Control) this.label2);
      this.panel3.Dock = DockStyle.Top;
      this.panel3.Location = new Point(0, 31);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(318, 162);
      this.panel3.TabIndex = 12;
      this.lblSost.AutoSize = true;
      this.lblSost.Location = new Point(0, 52);
      this.lblSost.Name = "lblSost";
      this.lblSost.Size = new Size(107, 17);
      this.lblSost.TabIndex = 15;
      this.lblSost.Text = "Составляющая";
      this.lblSost.Click += new EventHandler(this.label5_Click);
      this.cmbSost.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbSost.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbSost.FormattingEnabled = true;
      this.cmbSost.Location = new Point(3, 72);
      this.cmbSost.Name = "cmbSost";
      this.cmbSost.Size = new Size(309, 24);
      this.cmbSost.TabIndex = 14;
      this.cmbSost.SelectionChangeCommitted += new EventHandler(this.comboBox1_SelectionChangeCommitted);
      this.lblGrid1.Dock = DockStyle.Bottom;
      this.lblGrid1.Location = new Point(0, 145);
      this.lblGrid1.Name = "lblGrid1";
      this.lblGrid1.Size = new Size(318, 17);
      this.lblGrid1.TabIndex = 13;
      this.lblGrid1.Text = "Варианты услуг";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(0, 3);
      this.label1.Name = "label1";
      this.label1.Size = new Size(52, 17);
      this.label1.TabIndex = 8;
      this.label1.Text = "Услуга";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(197, 99);
      this.label3.Name = "label3";
      this.label3.Size = new Size(117, 17);
      this.label3.TabIndex = 12;
      this.label3.Text = "Дата окончания";
      this.mtbDEnd.Location = new Point(200, 119);
      this.mtbDEnd.Mask = "00/00/0000";
      this.mtbDEnd.Name = "mtbDEnd";
      this.mtbDEnd.Size = new Size(114, 23);
      this.mtbDEnd.TabIndex = 3;
      this.mtbDEnd.ValidatingType = typeof (DateTime);
      this.cbService.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cbService.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbService.FormattingEnabled = true;
      this.cbService.Location = new Point(3, 23);
      this.cbService.Name = "cbService";
      this.cbService.Size = new Size(309, 24);
      this.cbService.TabIndex = 1;
      this.cbService.SelectedIndexChanged += new EventHandler(this.cbService_SelectedIndexChanged);
      this.mtbDBeg.Location = new Point(3, 119);
      this.mtbDBeg.Mask = "00/00/0000";
      this.mtbDBeg.Name = "mtbDBeg";
      this.mtbDBeg.Size = new Size(105, 23);
      this.mtbDBeg.TabIndex = 2;
      this.mtbDBeg.ValidatingType = typeof (DateTime);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(0, 99);
      this.label2.Name = "label2";
      this.label2.Size = new Size(94, 17);
      this.label2.TabIndex = 11;
      this.label2.Text = "Дата начала";
      this.pnPeriod.Controls.Add((Control) this.label4);
      this.pnPeriod.Controls.Add((Control) this.mpPeriod);
      this.pnPeriod.Dock = DockStyle.Top;
      this.pnPeriod.Location = new Point(0, 0);
      this.pnPeriod.Name = "pnPeriod";
      this.pnPeriod.Size = new Size(318, 31);
      this.pnPeriod.TabIndex = 11;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(0, 9);
      this.label4.Name = "label4";
      this.label4.Size = new Size(58, 17);
      this.label4.TabIndex = 11;
      this.label4.Text = "Период";
      this.mpPeriod.CustomFormat = "MMMM yyyy";
      this.mpPeriod.Format = DateTimePickerFormat.Custom;
      this.mpPeriod.Location = new Point(64, 6);
      this.mpPeriod.Name = "mpPeriod";
      this.mpPeriod.OldMonth = 0;
      this.mpPeriod.ShowUpDown = true;
      this.mpPeriod.Size = new Size(140, 23);
      this.mpPeriod.TabIndex = 0;
      this.mpPeriod.ValueChanged += new EventHandler(this.mpPeriod_ValueChanged);
      this.btnCheck.Dock = DockStyle.Bottom;
      this.btnCheck.FlatStyle = FlatStyle.Popup;
      this.btnCheck.Image = (Image) Resources.properties;
      this.btnCheck.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCheck.Location = new Point(3, 615);
      this.btnCheck.Name = "btnCheck";
      this.btnCheck.Size = new Size(550, 35);
      this.btnCheck.TabIndex = 2;
      this.btnCheck.Text = "Выделить все";
      this.btnCheck.UseVisualStyleBackColor = true;
      this.btnCheck.Click += new EventHandler(this.btnCheck_Click);
      this.groupBox2.Controls.Add((Control) this.chlbAddress);
      this.groupBox2.Controls.Add((Control) this.btnCheck);
      this.groupBox2.Dock = DockStyle.Fill;
      this.groupBox2.Location = new Point(0, 0);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(556, 653);
      this.groupBox2.TabIndex = 3;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Список домов";
      this.chlbAddress.CheckOnClick = true;
      this.chlbAddress.Dock = DockStyle.Fill;
      this.chlbAddress.FormattingEnabled = true;
      this.chlbAddress.Location = new Point(3, 19);
      this.chlbAddress.Name = "chlbAddress";
      this.chlbAddress.Size = new Size(550, 596);
      this.chlbAddress.TabIndex = 3;
      this.hp.HelpNamespace = "Help.chm";
      this.splitContainer1.Dock = DockStyle.Fill;
      this.splitContainer1.Location = new Point(0, 0);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.dgvService);
      this.splitContainer1.Panel1.Controls.Add((Control) this.lblGrid2);
      this.splitContainer1.Panel1.Controls.Add((Control) this.panel3);
      this.splitContainer1.Panel1.Controls.Add((Control) this.pnNorm);
      this.splitContainer1.Panel1.Controls.Add((Control) this.dgvNorm);
      this.splitContainer1.Panel1.Controls.Add((Control) this.pnPeriod);
      this.splitContainer1.Panel2.Controls.Add((Control) this.splitContainer2);
      this.splitContainer1.Size = new Size(1201, 653);
      this.splitContainer1.SplitterDistance = 318;
      this.splitContainer1.TabIndex = 4;
      this.splitContainer2.Dock = DockStyle.Fill;
      this.splitContainer2.Location = new Point(0, 0);
      this.splitContainer2.Name = "splitContainer2";
      this.splitContainer2.Panel1.Controls.Add((Control) this.groupBox2);
      this.splitContainer2.Panel2.Controls.Add((Control) this.panelChoose);
      this.splitContainer2.Size = new Size(879, 653);
      this.splitContainer2.SplitterDistance = 556;
      this.splitContainer2.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(1201, 692);
      this.Controls.Add((Control) this.splitContainer1);
      this.Controls.Add((Control) this.pnBtn);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.hp.SetHelpKeyword((Control) this, "kv132.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmMassChooseObject";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Операции";
      this.Load += new EventHandler(this.FrmMassChooseObject_Load);
      this.pnBtn.ResumeLayout(false);
      this.panelChoose.ResumeLayout(false);
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      this.gbType.ResumeLayout(false);
      this.gbType.PerformLayout();
      this.gbLs.ResumeLayout(false);
      this.gbLs.PerformLayout();
      this.gpbFlat.ResumeLayout(false);
      this.gpbFlat.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      ((ISupportInitialize) this.dgvService).EndInit();
      this.pnNorm.ResumeLayout(false);
      this.pnNorm.PerformLayout();
      ((ISupportInitialize) this.dgvNorm).EndInit();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.pnPeriod.ResumeLayout(false);
      this.pnPeriod.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel2.ResumeLayout(false);
      this.splitContainer2.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
