// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmCorrect
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
  public class FrmCorrect : Form
  {
    private readonly FormStateSaver fss = new FormStateSaver(FrmCorrect.ic);
    protected GridSettings SettingsCorrect = new GridSettings();
    protected GridSettings SettingsCorrectCmp = new GridSettings();
    protected GridSettings SettingsCorrectMSP = new GridSettings();
    protected GridSettings SettingsCorrectOld = new GridSettings();
    private IContainer components = (IContainer) null;
    private static IContainer ic;
    private readonly LsClient client;
    private readonly DataTable dataTable;
    private readonly short type;
    private int index;
    private bool insertRecord;
    private IList listRent;
    private CorrectRent old;
    private IList oldList;
    private IList<CorrectRent> oldListCorrect;
    private RentMSP oldMSP;
    private ISession session;
    private Panel pnBtn;
    private Button btnExit;
    private Button btnDelete;
    private Button btnSave;
    private Button btnAdd;
    private Panel pnUp;
    private TabControl tcntrl;
    private TabPage pgCorrect;
    private DataGridView dgvCorrect;
    private TabPage pgCorrectMSP;
    private DataGridView dgvCorrectMSP;
    private TabPage pgCorrectCmp;
    private DataGridView dgvCorrectCmp;
    private MonthPicker mpPeriod;
    private ContextMenuStrip cmsMenu;
    private ToolStripMenuItem miCopy;
    private TabPage pgOldCorrect;
    private DataGridView dgvOldCorrect;
    private Label lblItogo;
    private ToolStripMenuItem miCopyFromAnotherLS;

    public FrmCorrect()
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
    }

    public FrmCorrect(LsClient client, short type)
    {
      this.client = client;
      this.InitializeComponent();
      this.mpPeriod.Value = Options.Period.PeriodName.Value;
      this.fss.ParentForm = (Form) this;
      this.dataTable = new DataTable();
      this.dataTable.Columns.Add("Id", typeof (short));
      this.dataTable.Columns.Add("Name", typeof (string));
      KvrplHelper.AddRow(this.dataTable, 0, "По нормативу");
      KvrplHelper.AddRow(this.dataTable, 1, "По счетчику");
      this.SettingsCorrect.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
      this.SettingsCorrectMSP.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
      this.SettingsCorrectCmp.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
      this.SettingsCorrectOld.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
      this.type = type;
    }

    private void FrmCorrect_Shown(object sender, EventArgs e)
    {
      if ((int) this.type == 1)
      {
        if (this.client.Complex.IdFk == Options.ComplexArenda.IdFk)
          this.tcntrl.TabPages.Remove(this.pgCorrectMSP);
        this.LoadCorrect(this.dgvCorrect, false, (short) 4);
      }
      if ((int) this.type != 2)
        return;
      this.tcntrl.TabPages.Remove(this.pgCorrect);
      this.tcntrl.TabPages.Remove(this.pgCorrectCmp);
      this.tcntrl.TabPages.Remove(this.pgOldCorrect);
      this.LoadCorrect(this.dgvCorrectMSP, true, (short) 4);
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(45, 2, this.client.Company, true))
        return;
      if (this.tcntrl.SelectedTab == this.pgCorrect && (int) this.type == 1)
        this.InsertCorrect(this.dgvCorrect, false, (short) 4);
      if (this.tcntrl.SelectedTab == this.pgCorrectMSP || (int) this.type == 2)
        this.InsertCorrect(this.dgvCorrectMSP, true, (short) 4);
      if (this.tcntrl.SelectedTab != this.pgCorrectCmp && (int) this.type != 2)
        return;
      this.InsertCorrect(this.dgvCorrectCmp, false, (short) 6);
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(45, 2, this.client.Company, true))
        return;
      if (this.tcntrl.SelectedTab == this.pgCorrect && (int) this.type == 1)
        this.SaveAllCorrect(this.dgvCorrect, false, (short) 4);
      if (this.tcntrl.SelectedTab == this.pgCorrectMSP || (int) this.type == 2)
      {
        this.SaveAllCorrect(this.dgvCorrectMSP, true, (short) 4);
        if (!this.insertRecord)
          this.LoadCorrect(this.dgvCorrectMSP, true, (short) 4);
      }
      if (this.tcntrl.SelectedTab != this.pgCorrectCmp)
        return;
      this.SaveAllCorrect(this.dgvCorrectCmp, false, (short) 6);
      if (!this.insertRecord)
        this.LoadCorrect(this.dgvCorrectCmp, false, (short) 6);
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(45, 2, this.client.Company, true))
        return;
      if (this.tcntrl.SelectedTab == this.pgCorrect && (int) this.type == 1)
      {
        this.DeleteCorrect(this.dgvCorrect, false, (short) 4);
        this.LoadCorrect(this.dgvCorrect, false, (short) 4);
      }
      if (this.tcntrl.SelectedTab == this.pgCorrectMSP || (int) this.type == 2)
      {
        this.DeleteCorrect(this.dgvCorrectMSP, true, (short) 4);
        this.LoadCorrect(this.dgvCorrectMSP, true, (short) 4);
      }
      if (this.tcntrl.SelectedTab != this.pgCorrectCmp)
        return;
      this.DeleteCorrect(this.dgvCorrectCmp, false, (short) 6);
      this.LoadCorrect(this.dgvCorrectCmp, false, (short) 6);
    }

    private void LoadCorrect(DataGridView dataGridView, bool msp, short code)
    {
      if (this.tcntrl.SelectedTab != this.pgOldCorrect)
      {
        this.btnSave.Enabled = false;
        this.btnAdd.Enabled = true;
        this.btnDelete.Enabled = true;
      }
      else
      {
        this.btnSave.Enabled = false;
        this.btnAdd.Enabled = false;
        this.btnDelete.Enabled = false;
      }
      this.listRent = (IList) new ArrayList();
      this.session = Domain.CurrentSession;
      this.session.Clear();
      dataGridView.Columns.Clear();
      dataGridView.DataSource = (object) null;
      IQuery query = msp ? this.session.CreateQuery(string.Format("select sum(r.RentMain) as rent, r.Month.PeriodName as month, (select ServiceId from Service where ServiceId = s.Root) as serv, sum(r.Volume) as volume, 0, 0, r.RentType as renttype, r.MSP.MSP_id, r.Person.PersonId, r.MSP.CodeSoc from RentMSP r, Service s where r.Period.PeriodId={0} and r.LsClient.ClientId={1} and r.Code={2} and r.Service = s group by s.Root, r.Month.PeriodName, r.MSP.MSP_id, r.Person.PersonId, r.MSP.CodeSoc, r.RentType order by s.Root, r.Month.PeriodName desc", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) code)) : ((int) code != 4 ? this.session.CreateQuery(string.Format("select sum(r.RentMain) as rent,r.Month.PeriodName as month,(select ServiceId from Service where ServiceId = s.Root) as serv,sum(r.Volume) as volume,0,0,r.RentType as renttype from Rent r, Service s where r.Period.PeriodId={0} and r.LsClient.ClientId={1} and r.Code={2} and r.Service=s group by s.Root,r.Month.PeriodName,r.RentType order by s.Root,r.Month.PeriodName desc", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) code)) : (this.tcntrl.SelectedTab != this.pgCorrect ? this.session.CreateQuery(string.Format("select sum(r.RentMain) as rent,r.Month.PeriodName as month,(select ServiceId from Service where ServiceId = s.Root) as serv,sum(r.Volume) as volume,0,0,r.RentType as renttype from Rent r, Service s where r.Period.PeriodId={0} and r.LsClient.ClientId={1} and r.Code=4 and r.Service=s group by s.Root,r.Month.PeriodName,r.RentType order by s.Root,r.Month.PeriodName desc", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) code)) : this.session.CreateQuery(string.Format("select sum(r.RentMain) as rent,r.Month.PeriodName as month,(select ServiceId from Service where ServiceId = s.Root) as serv,sum(r.Volume) as volume,0,0,r.RentType as renttype,r.Note,r.UName,r.DEdit,sum(r.RentVat) as rentvat,sum(r.RentEO) as renteo from CorrectRent r, Service s where r.Period.PeriodId={0} and r.LsClient.ClientId={1} and r.Service=s group by s.Root,r.Month.PeriodName,r.Note,r.UName,r.DEdit,r.RentType order by s.Root,r.Month.PeriodName desc ", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) code))));
      this.listRent = query.List();
      Decimal num = new Decimal();
      foreach (object[] objArray in (IEnumerable) this.listRent)
        num += Convert.ToDecimal(objArray[0]);
      this.lblItogo.Text = "Итого: " + num.ToString();
      dataGridView.DataSource = (object) this.listRent;
      this.session.Clear();
      this.oldList = (IList) new ArrayList();
      this.oldList = query.List();
      this.SetViewCorrect(dataGridView, msp, code);
      dataGridView.ReadOnly = false;
      dataGridView.Focus();
      this.session.Clear();
    }

    private void SetViewCorrect(DataGridView dataGridView, bool msp, short code)
    {
      foreach (DataGridViewBand column in (BaseCollection) dataGridView.Columns)
        column.Visible = false;
      IList<Service> serviceList = this.session.CreateQuery(string.Format("select distinct s from Service s,ServiceParam sp where sp.Service_id=s.ServiceId and s.Root=0 and s.ServiceId<>0 and sp.Company_id={0} order by " + Options.SortService, (object) this.client.Company.CompanyId)).List<Service>();
      KvrplHelper.AddComboBoxColumn(dataGridView, 0, (IList) serviceList, "ServiceId", "ServiceName", "Услуга", "Service", 140, 140);
      KvrplHelper.AddTextBoxColumn(dataGridView, 1, "Сумма изменений", "Rent", 100, false);
      KvrplHelper.AddTextBoxColumn(dataGridView, 2, "Объём", "Volume", 100, false);
      KvrplHelper.AddMaskDateColumn(dataGridView, 3, "Месяц перерасчета", "Month");
      KvrplHelper.AddTextBoxColumn(dataGridView, 4, "Edit", "Edit", 1, false);
      KvrplHelper.AddTextBoxColumn(dataGridView, 5, "Insert", "Insert", 1, false);
      KvrplHelper.AddComboBoxColumn(dataGridView, 4, this.dataTable, "Id", "Name", "Тип", "RentType", 2);
      dataGridView.Columns["Insert"].Visible = false;
      dataGridView.Columns["Edit"].Visible = false;
      if (!msp && (int) code == 4 && this.tcntrl.SelectedTab == this.pgCorrect)
      {
        KvrplHelper.AddTextBoxColumn(dataGridView, 2, "Сумма по ЭО тарифу", "RentEO", 100, false);
        KvrplHelper.AddTextBoxColumn(dataGridView, 7, "Основание", "Note", 150, false);
        KvrplHelper.AddTextBoxColumn(dataGridView, 8, "Пользователь", "UName", 100, true);
        KvrplHelper.AddTextBoxColumn(dataGridView, 9, "Дата редактирования", "DEdit", 100, true);
        if (this.client.Complex.IdFk == Options.ComplexArenda.IdFk)
          KvrplHelper.AddTextBoxColumn(dataGridView, 3, "Сумма НДС", "RentVat", 100, false);
      }
      if (msp)
      {
        IList<Person> personList1 = (IList<Person>) new List<Person>();
        IList<Person> personList2 = this.session.CreateCriteria(typeof (Person)).Add((ICriterion) Restrictions.Eq("LsClient", (object) this.client)).Add((ICriterion) Restrictions.In("Reg.RegId", (ICollection) new int[2]{ 1, 2 })).Add((ICriterion) Restrictions.Or((ICriterion) Restrictions.Lt("Archive", (object) 3), (ICriterion) Restrictions.Eq("Archive", (object) 5))).AddOrder(Order.Asc("Archive")).AddOrder(Order.Asc("Relation.RelationId")).List<Person>();
        KvrplHelper.AddComboBoxColumn(dataGridView, 0, (IList) personList2, "PersonId", "FIO", "ФИО", "FIO", 180, 180);
        foreach (Person person in (IEnumerable<Person>) personList2)
          KvrplHelper.GetFamily(person, 1, false);
        KvrplHelper.AddTextBoxColumn(dataGridView, 1, "Номер льготы", "MSPId", 60, false);
        KvrplHelper.AddTextBoxColumn(dataGridView, 2, "Номер льготы ОСЗН", "MSPIdSoc", 60, false);
        IList<DcMSP> dcMspList = this.session.CreateCriteria(typeof (DcMSP)).AddOrder(Order.Asc("MSP_name")).List<DcMSP>();
        KvrplHelper.AddComboBoxColumn(dataGridView, 3, (IList) dcMspList, "MSP_id", "MSP_name", "Льгота", "MSP", 200, 200);
      }
      foreach (DataGridViewRow row in (IEnumerable) dataGridView.Rows)
      {
        row.Cells["Rent"].Value = ((object[]) this.listRent[row.Index])[0];
        row.Cells["Month"].Value = ((object[]) this.listRent[row.Index])[1];
        row.Cells["Service"].Value = ((object[]) this.listRent[row.Index])[2];
        row.Cells["Volume"].Value = ((object[]) this.listRent[row.Index])[3];
        row.Cells["Edit"].Value = ((object[]) this.listRent[row.Index])[4];
        row.Cells["Insert"].Value = ((object[]) this.listRent[row.Index])[5];
        row.Cells["RentType"].Value = ((object[]) this.listRent[row.Index])[6];
        if (!msp && (int) code == 4 && this.tcntrl.SelectedTab == this.pgCorrect)
        {
          row.Cells["Note"].Value = ((object[]) this.listRent[row.Index])[7];
          row.Cells["UName"].Value = ((object[]) this.listRent[row.Index])[8];
          row.Cells["DEdit"].Value = ((object[]) this.listRent[row.Index])[9];
          row.Cells["RentEO"].Value = ((object[]) this.listRent[row.Index])[11];
        }
        if (msp)
        {
          row.Cells["FIO"].Value = ((object[]) this.listRent[row.Index])[8];
          row.Cells["MSP"].Value = ((object[]) this.listRent[row.Index])[7];
          row.Cells["MSPId"].Value = ((object[]) this.listRent[row.Index])[7];
          row.Cells["MSPIdSoc"].Value = ((object[]) this.listRent[row.Index])[9];
        }
      }
      this.SettingsCorrect.GridName = "Correct";
      this.LoadSettingsCorrect();
      this.SettingsCorrectMSP.GridName = "CorrectMSP";
      this.LoadSettingsCorrectMSP();
      this.SettingsCorrectCmp.GridName = "CorrectCmp";
      this.LoadSettingsCorrectCmp();
      this.SettingsCorrectOld.GridName = "CorrectOld";
      this.LoadSettingsCorrectOld();
    }

    private void InsertCorrect(DataGridView dataGridView, bool msp, short code)
    {
      this.insertRecord = true;
      object[] objArray;
      if (msp)
        objArray = new object[10]
        {
          (object) 0,
          (object) null,
          (object) null,
          (object) 0,
          (object) 1,
          (object) 1,
          (object) null,
          (object) null,
          (object) null,
          (object) null
        };
      else if ((int) code == 4)
        objArray = new object[12]
        {
          (object) 0,
          (object) null,
          (object) null,
          (object) 0,
          (object) 1,
          (object) 1,
          (object) null,
          (object) null,
          (object) null,
          (object) null,
          (object) 0,
          (object) 0
        };
      else
        objArray = new object[7]
        {
          (object) 0,
          (object) null,
          (object) null,
          (object) 0,
          (object) 1,
          (object) 1,
          (object) null
        };
      this.listRent = (IList) new ArrayList();
      if ((uint) dataGridView.Rows.Count > 0U)
        this.listRent = (IList) (dataGridView.DataSource as ArrayList);
      this.listRent.Add((object) objArray);
      dataGridView.Columns.Clear();
      dataGridView.DataSource = (object) null;
      dataGridView.DataSource = (object) this.listRent;
      this.SetViewCorrect(dataGridView, msp, code);
    }

    private void SaveAllCorrect(DataGridView dataGridView, bool msp, short code)
    {
      bool flag = false;
      this.index = 0;
      int index = 0;
      foreach (DataGridViewRow row in (IEnumerable) dataGridView.Rows)
      {
        if ((int) Convert.ToInt16(row.Cells["Edit"].Value) == 1)
        {
          this.insertRecord = (int) Convert.ToInt16(row.Cells["Insert"].Value.ToString()) == 1;
          dataGridView.Rows[row.Index].Selected = true;
          dataGridView.CurrentCell = row.Cells[0];
          index = row.Index;
          if (this.SaveCorrect(dataGridView, msp, code))
          {
            dataGridView.Rows[index].Cells["Insert"].Value = (object) 0;
            dataGridView.Rows[index].Cells["Edit"].Value = (object) 0;
          }
          else
            flag = true;
        }
        if ((int) Convert.ToInt16(dataGridView.Rows[index].Cells["Insert"].Value) != 1)
          this.index = this.index + 1;
      }
      if (flag)
        return;
      this.LoadCorrect(dataGridView, msp, code);
    }

    private bool SaveCorrect(DataGridView dataGridView, bool msp, short code)
    {
      if (dataGridView.Rows.Count <= 0 || dataGridView.CurrentRow == null)
        return true;
      DateTime dateTime1 = Options.Period.PeriodName.Value;
      DateTime? periodName = KvrplHelper.GetKvrClose(this.client.ClientId, Options.ComplexPasp, Options.ComplexPrior).PeriodName;
      DateTime dateTime2 = periodName.Value;
      if (dateTime1 <= dateTime2)
      {
        int num = (int) MessageBox.Show("Невозможно внести изменения в закрытом месяце", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      this.session = Domain.CurrentSession;
      if (dataGridView.CurrentRow.Cells["Service"].Value == null)
      {
        int num = (int) MessageBox.Show("Выберите услугу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (dataGridView.CurrentRow.Cells["Rent"].Value == null)
      {
        int num = (int) MessageBox.Show("Введите сумму", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      Person person = new Person();
      DcMSP msp1 = new DcMSP();
      if (msp)
      {
        if (dataGridView.CurrentRow.Cells["FIO"].Value == null)
        {
          int num = (int) MessageBox.Show("Выберите льготника", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (dataGridView.CurrentRow.Cells["MSP"].Value == null)
        {
          int num = (int) MessageBox.Show("Выберите льготу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        person = this.session.Get<Person>(dataGridView.CurrentRow.Cells["FIO"].Value);
        msp1 = this.session.Get<DcMSP>(dataGridView.CurrentRow.Cells["MSP"].Value);
        this.session.Clear();
      }
      string str = "";
      if (!msp && (int) code == 4 && dataGridView.CurrentRow.Cells["Note"].Value != null)
        str = dataGridView.CurrentRow.Cells["Note"].Value.ToString();
      Service service1 = this.session.Get<Service>(dataGridView.CurrentRow.Cells["Service"].Value);
      short num1 = 0;
      if (dataGridView.CurrentRow.Cells["RentType"].Value != null)
        num1 = Convert.ToInt16(dataGridView.CurrentRow.Cells["RentType"].Value);
      if (dataGridView.CurrentRow.Cells["Month"].Value == null)
      {
        DataGridViewCell cell = dataGridView.CurrentRow.Cells["Month"];
        periodName = Options.Period.PeriodName;
        // ISSUE: variable of a boxed type
        DateTime local = periodName.Value;
        cell.Value = (object) local;
      }
      else
        dataGridView.CurrentRow.Cells["Month"].Value = (object) KvrplHelper.FirstDay(Convert.ToDateTime(dataGridView.CurrentRow.Cells["Month"].Value));
      Period month = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", dataGridView.CurrentRow.Cells["Month"].Value)).List<Period>()[0];
      double sumEO = 0.0;
      double sum;
      try
      {
        sum = Convert.ToDouble(KvrplHelper.ChangeSeparator(dataGridView.CurrentRow.Cells["Rent"].Value.ToString()));
      }
      catch (Exception ex)
      {
        int num2 = (int) MessageBox.Show("Введенная сумма некорректна", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (!msp)
      {
        try
        {
          sumEO = Convert.ToDouble(KvrplHelper.ChangeSeparator(dataGridView.CurrentRow.Cells["RentEO"].Value.ToString()));
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show("Введенная сумма по ЭО тарифу некорректна", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
      double vol;
      try
      {
        vol = Convert.ToDouble(KvrplHelper.ChangeSeparator(dataGridView.CurrentRow.Cells["Volume"].Value.ToString()));
      }
      catch (Exception ex)
      {
        int num2 = (int) MessageBox.Show("Введенный объем некорректен", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      IList<Rent> rentList = (IList<Rent>) new List<Rent>();
      IList<CorrectRent> correctRentList1 = (IList<CorrectRent>) new List<CorrectRent>();
      IList<RentMSP> rentMspList = (IList<RentMSP>) new List<RentMSP>();
      if (msp)
      {
        if (dataGridView.CurrentRow.Cells["FIO"].Value == null)
        {
          int num2 = (int) MessageBox.Show("Выберите жильца", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (dataGridView.CurrentRow.Cells["MSP"].Value == null)
        {
          int num2 = (int) MessageBox.Show("Выберите льготу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (this.client.Complex.IdFk == 100)
          rentMspList = this.session.CreateQuery(string.Format("select r from RentMSP r,Service s where r.Period.PeriodId={0} and r.LsClient.ClientId = {1} and r.Service=s and s.Root={2} and r.Code=0 and r.MSP.MSP_id={3} and r.Person.PersonId={4}", (object) month.PeriodId, (object) this.client.ClientId, (object) service1.ServiceId, (object) msp1.MSP_id, (object) person.PersonId)).List<RentMSP>();
        if (this.client.Complex.IdFk == 110)
          rentMspList = this.session.CreateQuery(string.Format("select r from RentMSP r,Service s where r.Period.PeriodId={0} and r.LsClient.ClientId = {1} and r.Service=s and s.Root={2} and r.Code=0 and r.MSP.MSP_id={3}", (object) month.PeriodId, (object) this.client.ClientId, (object) service1.ServiceId, (object) msp1.MSP_id)).List<RentMSP>();
      }
      else
        rentList = this.session.CreateQuery(string.Format("select r from Rent r,Service s where r.Period.PeriodId={0} and r.LsClient.ClientId = {1} and r.Service=s and s.Root={2} and r.Code=0", (object) month.PeriodId, (object) this.client.ClientId, (object) service1.ServiceId)).List<Rent>();
      if (!msp && rentList.Count > 0 || msp && rentMspList.Count > 0)
      {
        if (!msp)
        {
          double num2 = Convert.ToDouble(this.session.CreateQuery(string.Format("select sum(r.RentMain) from Rent r,Service s where r.Period.PeriodId={0} and r.LsClient.ClientId = {1} and r.Service=s and s.Root={2} and r.Code=0", (object) month.PeriodId, (object) this.client.ClientId, (object) service1.ServiceId)).List()[0]);
          double num3 = Convert.ToDouble(this.session.CreateQuery(string.Format("select sum(r.RentEO) from Rent r,Service s where r.Period.PeriodId={0} and r.LsClient.ClientId = {1} and r.Service=s and s.Root={2} and r.Code=0", (object) month.PeriodId, (object) this.client.ClientId, (object) service1.ServiceId)).List()[0]);
          if ((int) code == 4)
          {
            try
            {
              sumEO = Convert.ToDouble(KvrplHelper.ChangeSeparator(dataGridView.CurrentRow.Cells["RentEO"].Value.ToString()));
              if (sumEO == 0.0 && MessageBox.Show("Сумма по ЭО тарифу равной 0. Продолжить?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return false;
              if (sum != 0.0 && sumEO == 0.0 && num3 != 0.0)
              {
                if (MessageBox.Show("Взять сумму по ЭО тарифу равной сумме корректировки?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                  sumEO = sum;
              }
            }
            catch (Exception ex)
            {
              int num4 = (int) MessageBox.Show("Введенная сумма по ЭО тарифу некорректна", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
          }
          ITransaction transaction = this.session.BeginTransaction();
          if (this.insertRecord)
          {
            foreach (Rent rent in (IEnumerable<Rent>) rentList)
            {
              try
              {
                if ((int) code == 4)
                  this.session.Save((object) new CorrectRent()
                  {
                    Period = Options.Period,
                    LsClient = this.client,
                    Service = rent.Service,
                    Supplier = rent.Supplier,
                    Month = month,
                    Note = (dataGridView.CurrentRow.Cells["Note"].Value == null ? "" : dataGridView.CurrentRow.Cells["Note"].Value.ToString()),
                    Volume = (rent.RentMain * vol / num2),
                    RentEO = (num3 == 0.0 ? 0.0 : rent.RentEO * sumEO / num3),
                    RentMain = (rent.RentMain * sum / num2),
                    RentType = num1,
                    UName = Options.Login,
                    DEdit = new DateTime?(DateTime.Now.Date)
                  });
                else
                  this.session.Save((object) new Rent()
                  {
                    Period = Options.Period,
                    LsClient = this.client,
                    Service = rent.Service,
                    Supplier = rent.Supplier,
                    Month = month,
                    Code = code,
                    Motive = 1,
                    Volume = (rent.RentMain * vol / num2),
                    RentEO = 0.0,
                    RentMain = (rent.RentMain * sum / num2),
                    RentType = num1
                  });
                this.session.Flush();
              }
              catch (Exception ex)
              {
                int num4 = (int) MessageBox.Show("Невозможно сохранить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                KvrplHelper.WriteLog(ex, (LsClient) null);
                return true;
              }
            }
          }
          else
          {
            IList list = (IList) new ArrayList();
            list = (int) code != 4 ? this.session.CreateQuery(string.Format("select sum(r.RentMain) as rent,r.Month.PeriodName as month,(select ServiceId from Service where ServiceId = s.Root) as serv,r.RentType  from Rent r, Service s where r.Period.PeriodId={0} and r.LsClient.ClientId={1} and r.Code={2} and r.Service=s group by s.Root,r.Month.PeriodName,r.RentType", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) code)).List() : this.session.CreateQuery(string.Format("select sum(r.RentMain) as rent,r.Month.PeriodName as month,(select ServiceId from Service where ServiceId = s.Root) as serv,r.RentType,r.Note from CorrectRent r, Service s where r.Period.PeriodId={0} and r.LsClient.ClientId={1} and r.Note='{2}' and r.Service=s group by s.Root,r.Month.PeriodName,r.Note,r.RentType", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) str)).List();
            Period period = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", ((object[]) this.oldList[this.index])[1])).List<Period>()[0];
            Service service2 = this.session.Get<Service>(((object[]) this.oldList[this.index])[2]);
            if ((int) code == 4)
            {
              IList<CorrectRent> correctRentList2 = this.session.CreateQuery(string.Format("select r from CorrectRent r,Service s where r.Period.PeriodId={0} and r.LsClient.ClientId = {1} and r.Service=s and s.Root={2} ", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) service2.ServiceId)).List<CorrectRent>();
              double num4 = Convert.ToDouble(this.session.CreateQuery(string.Format("select sum(r.RentMain) from CorrectRent r,Service s where r.Period.PeriodId={0} and r.LsClient.ClientId = {1} and r.Service=s and s.Root={2} ", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) service2.ServiceId)).List()[0]);
              double num5 = Convert.ToDouble(this.session.CreateQuery(string.Format("select sum(r.RentEO) from CorrectRent r,Service s where r.Period.PeriodId={0} and r.LsClient.ClientId = {1} and r.Service=s and s.Root={2} ", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) service2.ServiceId)).List()[0]);
              bool flag = false;
              if (sum == sumEO || num5 == 0.0 && sumEO != 0.0)
              {
                flag = true;
                num5 = num4;
              }
              if (num5 == 0.0)
                num5 = 1.0;
              if (num4 == 0.0)
                num4 = 1.0;
              foreach (CorrectRent correctRent in (IEnumerable<CorrectRent>) correctRentList2)
              {
                double num6 = correctRent.RentEO;
                if (flag)
                  num6 = correctRent.RentMain;
                try
                {
                  this.session.CreateQuery(string.Format("update CorrectRent r set r.RentMain=:rent,r.RentEO=:renteo,r.Month.PeriodId={0},r.Volume=:volume,r.Note='{6}',r.UName='{8}',r.DEdit='{9}', r.RentType=:type  where r.Period.PeriodId={1} and r.LsClient.ClientId={2} and r.Service.ServiceId={3} and r.Supplier.SupplierId={4} and r.RentType=:oldtype and r.Note='{7}' and r.Month.PeriodId={5}", (object) month.PeriodId, (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) correctRent.Service.ServiceId, (object) correctRent.Supplier.SupplierId, (object) period.PeriodId, (object) this.dgvCorrect.CurrentRow.Cells["Note"].Value.ToString(), ((object[]) this.oldList[this.index])[7], (object) Options.Login, (object) KvrplHelper.DateToBaseFormat(DateTime.Now))).SetParameter<double>("rent", correctRent.RentMain * sum / num4).SetParameter<double>("renteo", num6 * sumEO / num5).SetParameter<double>("volume", correctRent.Volume * sum / num4).SetParameter<short>("type", num1).SetParameter<short>("oldtype", Convert.ToInt16(((object[]) this.oldList[this.index])[6])).ExecuteUpdate();
                }
                catch (Exception ex)
                {
                  KvrplHelper.WriteLog(ex, (LsClient) null);
                }
              }
            }
            else
            {
              foreach (Rent rent in (IEnumerable<Rent>) rentList)
              {
                try
                {
                  this.session.CreateQuery(string.Format("update Rent r set r.RentMain=:rent,r.Month.PeriodId={0},r.Volume=:volume,r.RentType=:type  where r.Period.PeriodId={1} and r.LsClient.ClientId={2} and r.Service.ServiceId={3} and r.Supplier.SupplierId={4} and r.RentType=:oldtype and r.Code={6} and r.Month.PeriodId={5}", (object) month.PeriodId, (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) rent.Service.ServiceId, (object) rent.Supplier.SupplierId, (object) period.PeriodId, (object) code)).SetParameter<double>("rent", rent.RentMain * sum / num2).SetParameter<double>("volume", rent.Volume * sum / num2).SetParameter<short>("type", num1).SetParameter<short>("oldtype", Convert.ToInt16(((object[]) this.oldList[this.index])[6])).ExecuteUpdate();
                }
                catch (Exception ex)
                {
                  KvrplHelper.WriteLog(ex, (LsClient) null);
                }
              }
            }
          }
          IList list1 = (IList) new ArrayList();
          IList list2;
          if ((int) code == 4)
            list2 = this.session.CreateQuery(string.Format("select sum(r.RentMain),sum(r.RentEO),sum(r.Volume) from CorrectRent r,Service s where r.Period.PeriodId={0} and r.LsClient.ClientId = {1} and r.Service=s and r.Month.PeriodId={4} and s.Root={2} and r.Note='{5}'", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) service1.ServiceId, (object) code, (object) month.PeriodId, (object) str)).List();
          else
            list2 = this.session.CreateQuery(string.Format("select sum(r.RentMain),sum(r.RentEO),sum(r.Volume) from Rent r,Service s where r.Period.PeriodId={0} and r.LsClient.ClientId = {1} and r.Service=s and s.Root={2} and r.Code={3}", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) service1.ServiceId, (object) code)).List();
          double num7 = Convert.ToDouble(KvrplHelper.ChangeSeparator(((object[]) list2[0])[0].ToString()));
          double num8 = Convert.ToDouble(KvrplHelper.ChangeSeparator(((object[]) list2[0])[1].ToString()));
          double num9 = Convert.ToDouble(KvrplHelper.ChangeSeparator(((object[]) list2[0])[2].ToString()));
          if (num7 != sum || num8 != sumEO || vol != num9)
          {
            Rent rent = rentList[0];
            try
            {
              if ((int) code == 4)
                this.session.CreateQuery(string.Format("update CorrectRent r set r.RentMain=r.RentMain+:rent,r.RentEO=r.RentEO+:renteo,r.Volume=r.Volume+:volume,r.RentType=:type  where r.Period.PeriodId={0} and r.LsClient.ClientId={1} and r.Service.ServiceId={2} and r.Supplier.SupplierId={3}  and r.Month.PeriodId={4} and r.Note='{6}'", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) rent.Service.ServiceId, (object) rent.Supplier.SupplierId, (object) month.PeriodId, (object) code, (object) str)).SetParameter<double>("rent", sum - num7).SetParameter<double>("renteo", sumEO - num8).SetParameter<double>("volume", vol - num9).SetParameter<short>("type", rent.RentType).ExecuteUpdate();
              else
                this.session.CreateQuery(string.Format("update Rent r set r.RentMain=r.RentMain+:rent,r.Volume=r.Volume+:volume,r.RentType=:type where r.Period.PeriodId={0} and r.LsClient.ClientId={1} and r.Service.ServiceId={2} and r.Supplier.SupplierId={3} and r.Code={5} and r.Month.PeriodId={4}", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) rent.Service.ServiceId, (object) rent.Supplier.SupplierId, (object) month.PeriodId, (object) code)).SetParameter<double>("rent", sum - num7).SetParameter<short>("type", rent.RentType).SetParameter<double>("volume", vol - num9).ExecuteUpdate();
            }
            catch (Exception ex)
            {
              KvrplHelper.WriteLog(ex, (LsClient) null);
            }
          }
          transaction.Commit();
          if ((int) code == 4)
          {
            CorrectRent rent = new CorrectRent();
            rent.LsClient = this.client;
            rent.Period = Options.Period;
            rent.Service = service1;
            rent.Month = month;
            rent.Note = dataGridView.CurrentRow.Cells["Note"].Value == null ? "" : dataGridView.CurrentRow.Cells["Note"].Value.ToString();
            rent.Volume = vol;
            rent.RentMain = sum;
            rent.RentEO = sumEO;
            rent.RentType = Convert.ToInt16(dataGridView.CurrentRow.Cells["RentType"].Value);
            if (Convert.ToInt32(KvrplHelper.BaseValue(32, this.client.Company)) == 1)
            {
              if (this.insertRecord)
                KvrplHelper.SaveCorrectRentToNoteBook(rent, this.client, (short) 1, KvrplHelper.GetKvrClose(this.client.ClientId, Options.ComplexPasp, Options.ComplexPrior).PeriodName.Value);
              else
                KvrplHelper.ChangeCorrectRentToNoteBook(rent, this.old, this.client, (short) 2, KvrplHelper.GetKvrClose(this.client.ClientId, Options.ComplexPasp, Options.ComplexPrior).PeriodName.Value);
            }
            this.old = (CorrectRent) null;
          }
          this.insertRecord = false;
        }
        else
        {
          double num2 = Convert.ToDouble(this.session.CreateQuery(string.Format("select sum(r.RentMain) from RentMSP r,Service s where r.Period.PeriodId={5} and r.LsClient.ClientId = {1} and r.Service=s and s.Root={2} and r.Code=0 and r.MSP.MSP_id = {3} and r.Person.PersonId = {4}", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) service1.ServiceId, (object) msp1.MSP_id, (object) person.PersonId, (object) month.PeriodId)).List()[0]);
          ITransaction transaction = this.session.BeginTransaction();
          if (this.insertRecord)
          {
            foreach (RentMSP rentMsp1 in (IEnumerable<RentMSP>) rentMspList)
            {
              RentMSP rentMsp2 = new RentMSP();
              rentMsp2.Period = Options.Period;
              rentMsp2.LsClient = this.client;
              rentMsp2.Service = rentMsp1.Service;
              rentMsp2.Supplier = rentMsp1.Supplier;
              rentMsp2.Month = month;
              rentMsp2.Code = code;
              rentMsp2.MSP = rentMsp1.MSP;
              rentMsp2.Person = rentMsp1.Person;
              rentMsp2.Motive = 1;
              rentMsp2.RentType = Convert.ToInt16(dataGridView.CurrentRow.Cells["RentType"].Value);
              rentMsp2.Volume = rentMsp1.RentMain * vol / num2;
              rentMsp2.RentMain = rentMsp1.RentMain * sum / num2;
              try
              {
                this.session.Save((object) rentMsp2);
                this.session.Flush();
              }
              catch (Exception ex)
              {
                int num3 = (int) MessageBox.Show("Невозможно сохранить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                KvrplHelper.WriteLog(ex, (LsClient) null);
                return true;
              }
            }
          }
          else
          {
            IList list1 = (IList) new ArrayList();
            IList list2 = this.session.CreateQuery(string.Format("select sum(r.RentMain) as rent,r.Month.PeriodName as month,(select ServiceId from Service where ServiceId = s.Root) as serv,r.MSP.MSP_id,r.Person.PersonId,r.RentType from RentMSP r, Service s where r.Period.PeriodId={0} and r.LsClient.ClientId={1} and r.Code={2} and r.Month.PeriodId={3} and r.Service=s group by s.Root,r.Month.PeriodName,r.MSP.MSP_id,r.Person.PersonId,r.RentType ", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) code, (object) month.PeriodId)).List();
            Period period = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", ((object[]) list2[dataGridView.CurrentRow.Index])[1])).List<Period>()[0];
            this.session.Get<Service>(((object[]) list2[dataGridView.CurrentRow.Index])[2]);
            foreach (RentMSP rentMsp in (IEnumerable<RentMSP>) rentMspList)
            {
              try
              {
                this.session.CreateQuery(string.Format("update RentMSP r set r.RentMain=:rent,r.Month.PeriodId={0},r.Volume=:volume,r.RentType=:type  where r.Period.PeriodId={1} and r.LsClient.ClientId={2} and r.Service.ServiceId={3} and r.Supplier.SupplierId={4} and r.Code={8} and r.Month.PeriodId={5} and r.MSP.MSP_id={6} and r.Person.PersonId={7} and r.RentType=:oldtype", (object) month.PeriodId, (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) rentMsp.Service.ServiceId, (object) rentMsp.Supplier.SupplierId, (object) period.PeriodId, (object) msp1.MSP_id, (object) person.PersonId, (object) code, (object) service1.ServiceId)).SetParameter<double>("rent", rentMsp.RentMain * sum / num2).SetParameter<double>("volume", rentMsp.Volume * vol / num2).SetParameter<short>("type", num1).SetParameter<short>("oldtype", Convert.ToInt16(((object[]) this.oldList[dataGridView.CurrentRow.Index])[6])).ExecuteUpdate();
              }
              catch (Exception ex)
              {
                KvrplHelper.WriteLog(ex, (LsClient) null);
              }
            }
          }
          IList list = this.session.CreateQuery(string.Format("select sum(r.RentMain),sum(r.Volume) from RentMSP r,Service s where r.Period.PeriodId={0} and r.LsClient.ClientId = {1} and r.Service=s and r.Month.PeriodId={6} and s.Root={2} and r.Code={5} and r.MSP.MSP_id={3} and r.Person.PersonId={4}", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) service1.ServiceId, (object) msp1.MSP_id, (object) person.PersonId, (object) code, (object) month.PeriodId)).List();
          double num4 = Convert.ToDouble(KvrplHelper.ChangeSeparator(((object[]) list[0])[0].ToString()));
          double num5 = Convert.ToDouble(KvrplHelper.ChangeSeparator(((object[]) list[0])[1].ToString()));
          if (num4 != sum || num5 != vol)
          {
            RentMSP rentMsp = rentMspList[0];
            try
            {
              this.session.CreateQuery(string.Format("update RentMSP r set r.RentMain=r.RentMain+:rent,r.Volume=r.Volume+:volume,r.RentType=:type  where r.Period.PeriodId={0} and r.LsClient.ClientId={1} and r.Service.ServiceId={2} and r.Supplier.SupplierId={3} and r.Code={7} and r.Month.PeriodId={4} and r.MSP.MSP_id={5} and r.Person.PersonId={6}", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) rentMsp.Service.ServiceId, (object) rentMsp.Supplier.SupplierId, (object) month.PeriodId, (object) msp1.MSP_id, (object) person.PersonId, (object) code)).SetParameter<double>("rent", sum - num4).SetParameter<short>("type", rentMsp.RentType).SetParameter<double>("volume", vol - num5).ExecuteUpdate();
            }
            catch (Exception ex)
            {
              KvrplHelper.WriteLog(ex, (LsClient) null);
            }
          }
          transaction.Commit();
          RentMSP rent = new RentMSP();
          rent.LsClient = this.client;
          rent.Period = Options.Period;
          rent.MSP = msp1;
          rent.Service = service1;
          rent.Month = month;
          rent.Person = person;
          rent.Volume = vol;
          rent.RentMain = sum;
          if (Convert.ToInt32(KvrplHelper.BaseValue(32, this.client.Company)) == 1)
          {
            if (this.insertRecord)
              KvrplHelper.SaveRentMSPToNoteBook(rent, this.client, (short) 1, KvrplHelper.GetKvrClose(this.client.ClientId, Options.ComplexPasp, Options.ComplexPrior).PeriodName.Value);
            else
              KvrplHelper.ChangeRentMSPToNoteBook(rent, this.oldMSP, this.client, (short) 2, KvrplHelper.GetKvrClose(this.client.ClientId, Options.ComplexPasp, Options.ComplexPrior).PeriodName.Value);
          }
          this.oldMSP = (RentMSP) null;
          this.insertRecord = false;
        }
      }
      else if (this.insertRecord)
      {
        short oldType = this.insertRecord || this.oldList.Count <= 0 ? num1 : Convert.ToInt16(((object[]) this.oldList[dataGridView.CurrentRow.Index])[6]);
        FrmHandMadeDetail frmHandMadeDetail = new FrmHandMadeDetail(this.client, service1, month, msp1, person, msp, true, code, str, this.old, this.oldMSP, num1, oldType, sum, sumEO, vol);
        int num2 = (int) frmHandMadeDetail.ShowDialog();
        frmHandMadeDetail.Dispose();
        this.insertRecord = false;
        this.old = (CorrectRent) null;
        this.oldMSP = (RentMSP) null;
      }
      else
      {
        if (this.tcntrl.SelectedIndex == 0)
          this.DetailCorrect(this.dgvCorrect, false, (short) 4);
        if (this.tcntrl.SelectedIndex == 1)
          this.DetailCorrect(this.dgvCorrectMSP, true, (short) 4);
        if (this.tcntrl.SelectedIndex == 2)
          this.DetailCorrect(this.dgvCorrectCmp, false, (short) 6);
      }
      this.session.Clear();
      return true;
    }

    private void DeleteCorrect(DataGridView dataGridView, bool msp, short code)
    {
      DateTime? periodName1 = Options.Period.PeriodName;
      DateTime dateTime1 = periodName1.Value;
      periodName1 = KvrplHelper.GetKvrClose(this.client.ClientId, Options.ComplexPasp, Options.ComplexPrior).PeriodName;
      DateTime dateTime2 = periodName1.Value;
      if (dateTime1 <= dateTime2)
      {
        int num1 = (int) MessageBox.Show("Невозможно внести изменения в закрытом месяце", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (dataGridView.Rows.Count <= 0 || dataGridView.CurrentRow == null || MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
          return;
        this.session = Domain.CurrentSession;
        DcMSP dcMsp = new DcMSP();
        Person person = new Person();
        string str = "";
        if (msp)
        {
          dcMsp = this.session.Get<DcMSP>(dataGridView.CurrentRow.Cells["MSP"].Value);
          person = this.session.Get<Person>(dataGridView.CurrentRow.Cells["FIO"].Value);
        }
        if (!msp && (int) code == 4 && dataGridView.CurrentRow.Cells["Note"].Value != null)
          str = dataGridView.CurrentRow.Cells["Note"].Value.ToString();
        this.session.Clear();
        Service service = this.session.Get<Service>(dataGridView.CurrentRow.Cells["Service"].Value);
        DateTime? periodName2;
        if (dataGridView.CurrentRow.Cells["Month"].Value == null)
        {
          DataGridViewCell cell = dataGridView.CurrentRow.Cells["Month"];
          periodName2 = Options.Period.PeriodName;
          // ISSUE: variable of a boxed type
          DateTime local = periodName2.Value;
          cell.Value = (object) local;
        }
        Period period = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", dataGridView.CurrentRow.Cells["Month"].Value)).List<Period>()[0];
        try
        {
          if (!msp)
          {
            if ((int) code == 4)
            {
              this.session.CreateQuery(string.Format("delete from CorrectRent r where Period.PeriodId={0} and LsClient.ClientId={1} and Month.PeriodId={2} and Service in (select s from Service s where s.Root={3}) and Note='{4}'", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) period.PeriodId, (object) service.ServiceId, (object) str)).ExecuteUpdate();
              CorrectRent correctRent = new CorrectRent();
              correctRent.LsClient = this.client;
              correctRent.Month = period;
              correctRent.Service = service;
              correctRent.Note = this.dgvCorrect.CurrentRow.Cells["Note"].Value == null ? "" : this.dgvCorrect.CurrentRow.Cells["Note"].Value.ToString();
              correctRent.Volume = Convert.ToDouble(this.dgvCorrect.CurrentRow.Cells["Volume"].Value);
              correctRent.RentMain = Convert.ToDouble(this.dgvCorrect.CurrentRow.Cells["Rent"].Value);
              if (Convert.ToInt32(KvrplHelper.BaseValue(32, this.client.Company)) == 1)
              {
                CorrectRent rent = correctRent;
                LsClient client = this.client;
                int num2 = 3;
                periodName2 = KvrplHelper.GetKvrClose(this.client.ClientId, Options.ComplexPasp, Options.ComplexPrior).PeriodName;
                DateTime monthClosed = periodName2.Value;
                KvrplHelper.DeleteCorrectRentfromNoteBook(rent, client, (short) num2, monthClosed);
              }
            }
            else
              this.session.CreateQuery(string.Format("delete from Rent r where Period.PeriodId={0} and LsClient.ClientId={1} and Code={4} and Month.PeriodId={2} and Service in (select s from Service s where s.Root={3})", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) period.PeriodId, (object) service.ServiceId, (object) code)).ExecuteUpdate();
          }
          else
          {
            this.session.CreateQuery(string.Format("delete from RentMSP r where Period.PeriodId={0} and LsClient.ClientId={1} and Code={6} and Month.PeriodId={2} and Service in (select s from Service s where s.Root={3}) and MSP.MSP_id={4} and Person.PersonId={5}", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) period.PeriodId, (object) service.ServiceId, (object) dcMsp.MSP_id, (object) person.PersonId, (object) code)).ExecuteUpdate();
            RentMSP rentMsp = new RentMSP();
            rentMsp.LsClient = this.client;
            rentMsp.Period = Options.Period;
            rentMsp.Service = this.session.Get<Service>(this.dgvCorrectMSP.CurrentRow.Cells["Service"].Value);
            rentMsp.MSP = this.session.Get<DcMSP>(this.dgvCorrectMSP.CurrentRow.Cells["MSP"].Value);
            rentMsp.Month = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", this.dgvCorrectMSP.CurrentRow.Cells["Month"].Value)).List<Period>()[0];
            rentMsp.Person = this.session.Get<Person>(this.dgvCorrectMSP.CurrentRow.Cells["FIO"].Value);
            rentMsp.Volume = Convert.ToDouble(this.dgvCorrectMSP.CurrentRow.Cells["Volume"].Value);
            rentMsp.RentMain = Convert.ToDouble(this.dgvCorrectMSP.CurrentRow.Cells["Rent"].Value);
            if (Convert.ToInt32(KvrplHelper.BaseValue(32, this.client.Company)) == 1)
            {
              RentMSP rent = rentMsp;
              LsClient client = this.client;
              int num2 = 3;
              periodName2 = KvrplHelper.GetKvrClose(this.client.ClientId, Options.ComplexPasp, Options.ComplexPrior).PeriodName;
              DateTime monthClosed = periodName2.Value;
              KvrplHelper.SaveRentMSPToNoteBook(rent, client, (short) num2, monthClosed);
            }
          }
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show("Невозможно удалить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        this.session.Clear();
      }
    }

    private void GetOld()
    {
      if (this.tcntrl.SelectedTab == this.pgCorrect && this.old == null)
      {
        try
        {
          this.old = new CorrectRent();
          this.old.LsClient = this.client;
          this.old.Period = Options.Period;
          this.old.Service = this.session.Get<Service>(this.dgvCorrect.CurrentRow.Cells["Service"].Value);
          this.old.Month = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", this.dgvCorrect.CurrentRow.Cells["Month"].Value)).List<Period>()[0];
          this.old.Note = this.dgvCorrect.CurrentRow.Cells["Note"].Value == null ? "" : this.dgvCorrect.CurrentRow.Cells["Note"].Value.ToString();
          this.old.Volume = Convert.ToDouble(this.dgvCorrect.CurrentRow.Cells["Volume"].Value);
          this.old.RentMain = Convert.ToDouble(this.dgvCorrect.CurrentRow.Cells["Rent"].Value);
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      if (this.tcntrl.SelectedTab != this.pgCorrectMSP || this.oldMSP != null)
        return;
      try
      {
        this.oldMSP = new RentMSP();
        this.oldMSP.LsClient = this.client;
        this.oldMSP.Period = Options.Period;
        this.oldMSP.Service = this.session.Get<Service>(this.dgvCorrectMSP.CurrentRow.Cells["Service"].Value);
        this.oldMSP.MSP = this.session.Get<DcMSP>(this.dgvCorrectMSP.CurrentRow.Cells["MSP"].Value);
        this.oldMSP.Month = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", this.dgvCorrectMSP.CurrentRow.Cells["Month"].Value)).List<Period>()[0];
        this.oldMSP.Person = this.session.Get<Person>(this.dgvCorrectMSP.CurrentRow.Cells["FIO"].Value);
        this.oldMSP.Volume = Convert.ToDouble(this.dgvCorrectMSP.CurrentRow.Cells["Volume"].Value);
        this.oldMSP.RentMain = Convert.ToDouble(this.dgvCorrectMSP.CurrentRow.Cells["Rent"].Value);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void DetailCorrect(DataGridView dataGridView, bool msp, short code)
    {
      this.session = Domain.CurrentSession;
      Service service = this.session.Get<Service>(dataGridView.CurrentRow.Cells["Service"].Value);
      Period month = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", dataGridView.CurrentRow.Cells["Month"].Value)).List<Period>()[0];
      this.session.Clear();
      Person person = new Person();
      DcMSP msp1 = new DcMSP();
      short int16 = Convert.ToInt16(dataGridView.CurrentRow.Cells["RentType"].Value);
      string str = "";
      if (!msp && (int) code == 4 && this.tcntrl.SelectedTab == this.pgCorrect)
        str = dataGridView.CurrentRow.Cells["Note"].Value.ToString();
      if (msp)
      {
        person = this.session.Get<Person>(dataGridView.CurrentRow.Cells["FIO"].Value);
        msp1 = this.session.Get<DcMSP>(dataGridView.CurrentRow.Cells["MSP"].Value);
      }
      this.GetOld();
      short code1 = code;
      if (this.tcntrl.SelectedTab == this.pgOldCorrect)
        code1 = (short) 5;
      FrmHandMadeDetail frmHandMadeDetail = new FrmHandMadeDetail(this.client, service, month, msp1, person, msp, false, code1, str, this.old, this.oldMSP, int16, Convert.ToInt16(((object[]) this.oldList[dataGridView.CurrentRow.Index])[6]), 0.0, 0.0, 0.0);
      int num = (int) frmHandMadeDetail.ShowDialog();
      frmHandMadeDetail.Dispose();
      this.LoadCorrect(dataGridView, msp, code);
    }

    private void dgvCorrect_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (this.insertRecord)
        return;
      if (this.tcntrl.SelectedIndex == 0)
        this.DetailCorrect(this.dgvCorrect, false, (short) 4);
      if (this.tcntrl.SelectedIndex == 1)
        this.DetailCorrect(this.dgvCorrectMSP, true, (short) 4);
      if (this.tcntrl.SelectedIndex == 2)
        this.DetailCorrect(this.dgvCorrectCmp, false, (short) 6);
      if (this.tcntrl.SelectedIndex == 3)
        this.DetailCorrect(this.dgvOldCorrect, false, (short) 4);
    }

    private void dgvCorrect_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      DataGridView dataGridView = (DataGridView) sender;
      this.btnDelete.Enabled = false;
      this.btnSave.Enabled = true;
      dataGridView.CurrentRow.Cells["Edit"].Value = (object) 1;
      this.GetOld();
    }

    private void dgvCorrectMSP_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgvCorrectMSP.CurrentCell.ColumnIndex == this.dgvCorrectMSP.Rows[this.dgvCorrectMSP.CurrentRow.Index].Cells["MSPId"].ColumnIndex)
      {
        int num = 0;
        try
        {
          num = Convert.ToInt32(this.dgvCorrectMSP.CurrentCell.Value);
        }
        catch
        {
        }
        IList<DcMSP> dcMspList = this.session.CreateQuery(string.Format("from DcMSP c where c.MSP_id={0}", (object) num)).List<DcMSP>();
        if (dcMspList.Count > 0)
        {
          this.dgvCorrectMSP.Rows[this.dgvCorrectMSP.CurrentRow.Index].Cells["MSP"].Value = (object) dcMspList[0].MSP_id;
          this.dgvCorrectMSP.CurrentRow.Cells["MSPIdSoc"].Value = (object) dcMspList[0].CodeSoc;
        }
        else
        {
          this.dgvCorrectMSP.CurrentRow.Cells["MSPIdSoc"].Value = (object) "";
          this.dgvCorrectMSP.Rows[this.dgvCorrectMSP.CurrentRow.Index].Cells["MSP"].Value = (object) null;
        }
        this.session.Clear();
      }
      if (this.dgvCorrectMSP.CurrentCell.ColumnIndex == this.dgvCorrectMSP.Rows[this.dgvCorrectMSP.CurrentRow.Index].Cells["MSPIdSoc"].ColumnIndex)
      {
        int num = 0;
        try
        {
          num = Convert.ToInt32(this.dgvCorrectMSP.CurrentCell.Value);
        }
        catch
        {
        }
        IList<DcMSP> dcMspList = this.session.CreateQuery(string.Format("from DcMSP c where c.CodeSoc={0}", (object) num)).List<DcMSP>();
        if (dcMspList.Count > 0)
        {
          this.dgvCorrectMSP.Rows[this.dgvCorrectMSP.CurrentRow.Index].Cells["MSP"].Value = (object) dcMspList[0].MSP_id;
          this.dgvCorrectMSP.CurrentRow.Cells["MSPId"].Value = (object) dcMspList[0].MSP_id;
        }
        else
        {
          this.dgvCorrectMSP.CurrentRow.Cells["MSPId"].Value = (object) "";
          this.dgvCorrectMSP.Rows[this.dgvCorrectMSP.CurrentRow.Index].Cells["MSP"].Value = (object) null;
        }
      }
      this.dgvCorrect_CellEndEdit(sender, e);
    }

    private void dgvCorrectMSP_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (!this.dgvCorrectMSP.IsCurrentCellDirty)
        return;
      this.dgvCorrectMSP.CommitEdit(DataGridViewDataErrorContexts.Commit);
      if (this.dgvCorrectMSP.CurrentCell.ColumnIndex == this.dgvCorrectMSP.Rows[this.dgvCorrectMSP.CurrentRow.Index].Cells["MSP"].ColumnIndex)
      {
        this.dgvCorrectMSP.Rows[this.dgvCorrectMSP.CurrentRow.Index].Cells["MSPId"].Value = this.dgvCorrectMSP.Rows[this.dgvCorrectMSP.CurrentRow.Index].Cells["MSP"].Value;
        if (this.dgvCorrectMSP.Rows[this.dgvCorrectMSP.CurrentRow.Index].Cells["MSP"].Value != null)
        {
          this.dgvCorrectMSP.Rows[this.dgvCorrectMSP.CurrentRow.Index].Cells["MSPIdSoc"].Value = (object) this.session.Get<DcMSP>((object) (int) this.dgvCorrectMSP.Rows[this.dgvCorrectMSP.CurrentRow.Index].Cells["MSP"].Value).CodeSoc;
          this.session.Clear();
        }
        else
          this.dgvCorrectMSP.Rows[this.dgvCorrectMSP.CurrentRow.Index].Cells["MSPIdSoc"].Value = (object) "";
      }
    }

    private void dgvCorrect_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    private void dgvCorrect_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      DataGridView dataGridView = (DataGridView) sender;
      if (dataGridView.CurrentCell == null || dataGridView.CurrentCell.Value == null)
        return;
      ((object[]) dataGridView.CurrentRow.DataBoundItem)[4] = (object) 1;
      try
      {
        string name = dataGridView.Columns[e.ColumnIndex].Name;
        if (!(name == "Service"))
        {
          if (!(name == "Rent"))
          {
            if (!(name == "Volume"))
            {
              if (!(name == "Month"))
              {
                if (name == "Note")
                {
                  try
                  {
                    ((object[]) dataGridView.CurrentRow.DataBoundItem)[6] = dataGridView.CurrentRow.Cells["Note"].Value;
                  }
                  catch
                  {
                  }
                }
              }
              else
              {
                try
                {
                  ((object[]) dataGridView.CurrentRow.DataBoundItem)[1] = dataGridView.CurrentRow.Cells["Month"].Value;
                }
                catch
                {
                }
              }
            }
            else
            {
              try
              {
                ((object[]) dataGridView.CurrentRow.DataBoundItem)[3] = dataGridView.CurrentRow.Cells["Volume"].Value;
              }
              catch
              {
              }
            }
          }
          else
          {
            try
            {
              ((object[]) dataGridView.CurrentRow.DataBoundItem)[0] = dataGridView.CurrentRow.Cells["Rent"].Value;
            }
            catch
            {
            }
          }
        }
        else
        {
          try
          {
            ((object[]) dataGridView.CurrentRow.DataBoundItem)[2] = dataGridView.CurrentRow.Cells["Service"].Value;
          }
          catch
          {
          }
        }
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void dgvCorrect_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.SettingsCorrect.FindByName(e.Column.Name) < 0)
        return;
      this.SettingsCorrect.Columns[this.SettingsCorrect.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.SettingsCorrect.Save();
    }

    private void dgvCorrectMSP_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.SettingsCorrectMSP.FindByName(e.Column.Name) < 0)
        return;
      this.SettingsCorrectMSP.Columns[this.SettingsCorrectMSP.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.SettingsCorrectMSP.Save();
    }

    private void dgvCorrectCmp_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.SettingsCorrectCmp.FindByName(e.Column.Name) < 0)
        return;
      this.SettingsCorrectCmp.Columns[this.SettingsCorrectCmp.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.SettingsCorrectCmp.Save();
    }

    private void dgvOldCorrect_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.SettingsCorrectOld.FindByName(e.Column.Name) < 0)
        return;
      this.SettingsCorrectOld.Columns[this.SettingsCorrectOld.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.SettingsCorrectOld.Save();
    }

    private void tcntrl_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.tcntrl.SelectedTab == this.pgCorrect)
        this.LoadCorrect(this.dgvCorrect, false, (short) 4);
      else if (this.tcntrl.SelectedTab == this.pgCorrectMSP)
        this.LoadCorrect(this.dgvCorrectMSP, true, (short) 4);
      else if (this.tcntrl.SelectedTab == this.pgCorrectCmp)
      {
        this.LoadCorrect(this.dgvCorrectCmp, false, (short) 6);
      }
      else
      {
        if (this.tcntrl.SelectedTab != this.pgOldCorrect)
          return;
        this.LoadCorrect(this.dgvOldCorrect, false, (short) 4);
      }
    }

    private void miCopyFromAnotherLS_Click(object sender, EventArgs e)
    {
      FrmChoice frmChoice = new FrmChoice(this.client, (Person) null, 3);
      int num = (int) frmChoice.ShowDialog();
      frmChoice.Dispose();
      this.LoadCorrect(this.dgvCorrect, false, (short) 4);
    }

    private void miCopy_Click(object sender, EventArgs e)
    {
      if (this.btnSave.Enabled)
      {
        int num1 = (int) MessageBox.Show("Сначала сохраните изменения");
      }
      else
      {
        bool flag = true;
        short num2 = 1;
        if (this.tcntrl.SelectedTab != this.pgCorrect || (this.dgvCorrect.Rows.Count <= 0 || this.dgvCorrect.CurrentRow.Index < 0))
          return;
        FrmChooseObject frmChooseObject = new FrmChooseObject(Options.Period, this.client, this.session.Get<Service>((object) Convert.ToInt16(this.dgvCorrect.CurrentRow.Cells["Service"].Value)), this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", this.dgvCorrect.CurrentRow.Cells["Month"].Value)).List<Period>()[0], this.dgvCorrect.CurrentRow.Cells["Note"].Value.ToString());
        frmChooseObject.Save = flag;
        frmChooseObject.CodeOperation = num2;
        frmChooseObject.MonthClosed = KvrplHelper.GetKvrClose(this.client.ClientId, Options.ComplexPasp, Options.ComplexPrior);
        int num3 = (int) frmChooseObject.ShowDialog();
        frmChooseObject.Dispose();
      }
    }

    private void mpPeriod_ValueChanged(object sender, EventArgs e)
    {
      Options.Period = KvrplHelper.SaveCurrentPeriod(this.mpPeriod.Value);
      this.tcntrl_SelectedIndexChanged(sender, e);
    }

    public void LoadSettingsCorrect()
    {
      this.SettingsCorrect.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvCorrect.Columns)
        this.SettingsCorrect.GetMySettings(column);
    }

    public void LoadSettingsCorrectMSP()
    {
      this.SettingsCorrectMSP.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvCorrectMSP.Columns)
        this.SettingsCorrectMSP.GetMySettings(column);
    }

    public void LoadSettingsCorrectCmp()
    {
      this.SettingsCorrectCmp.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvCorrectCmp.Columns)
        this.SettingsCorrectCmp.GetMySettings(column);
    }

    public void LoadSettingsCorrectOld()
    {
      this.SettingsCorrectOld.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvOldCorrect.Columns)
        this.SettingsCorrectOld.GetMySettings(column);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmCorrect));
      this.pnBtn = new Panel();
      this.btnDelete = new Button();
      this.btnSave = new Button();
      this.btnAdd = new Button();
      this.btnExit = new Button();
      this.pnUp = new Panel();
      this.lblItogo = new Label();
      this.mpPeriod = new MonthPicker();
      this.tcntrl = new TabControl();
      this.pgCorrect = new TabPage();
      this.dgvCorrect = new DataGridView();
      this.cmsMenu = new ContextMenuStrip(this.components);
      this.miCopy = new ToolStripMenuItem();
      this.miCopyFromAnotherLS = new ToolStripMenuItem();
      this.pgCorrectMSP = new TabPage();
      this.dgvCorrectMSP = new DataGridView();
      this.pgCorrectCmp = new TabPage();
      this.dgvCorrectCmp = new DataGridView();
      this.pgOldCorrect = new TabPage();
      this.dgvOldCorrect = new DataGridView();
      this.pnBtn.SuspendLayout();
      this.pnUp.SuspendLayout();
      this.tcntrl.SuspendLayout();
      this.pgCorrect.SuspendLayout();
      ((ISupportInitialize) this.dgvCorrect).BeginInit();
      this.cmsMenu.SuspendLayout();
      this.pgCorrectMSP.SuspendLayout();
      ((ISupportInitialize) this.dgvCorrectMSP).BeginInit();
      this.pgCorrectCmp.SuspendLayout();
      ((ISupportInitialize) this.dgvCorrectCmp).BeginInit();
      this.pgOldCorrect.SuspendLayout();
      ((ISupportInitialize) this.dgvOldCorrect).BeginInit();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnDelete);
      this.pnBtn.Controls.Add((Control) this.btnSave);
      this.pnBtn.Controls.Add((Control) this.btnAdd);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 282);
      this.pnBtn.Margin = new Padding(4);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(768, 40);
      this.pnBtn.TabIndex = 0;
      this.btnDelete.Image = (Image) Resources.minus;
      this.btnDelete.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnDelete.Location = new Point(121, 5);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(98, 30);
      this.btnDelete.TabIndex = 6;
      this.btnDelete.Text = "Удалить";
      this.btnDelete.TextAlign = ContentAlignment.MiddleRight;
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnSave.Image = (Image) Resources.Tick;
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.Location = new Point(225, 5);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(108, 30);
      this.btnSave.TabIndex = 5;
      this.btnSave.Text = "Сохранить";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnAdd.Image = (Image) Resources.plus;
      this.btnAdd.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnAdd.Location = new Point(12, 5);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(103, 30);
      this.btnAdd.TabIndex = 4;
      this.btnAdd.Text = "Добавить";
      this.btnAdd.TextAlign = ContentAlignment.MiddleRight;
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(674, 5);
      this.btnExit.Margin = new Padding(4);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(81, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.pnUp.Controls.Add((Control) this.lblItogo);
      this.pnUp.Controls.Add((Control) this.mpPeriod);
      this.pnUp.Dock = DockStyle.Top;
      this.pnUp.Location = new Point(0, 0);
      this.pnUp.Name = "pnUp";
      this.pnUp.Size = new Size(768, 32);
      this.pnUp.TabIndex = 1;
      this.lblItogo.AutoSize = true;
      this.lblItogo.Location = new Point(3, 9);
      this.lblItogo.Name = "lblItogo";
      this.lblItogo.Size = new Size(0, 16);
      this.lblItogo.TabIndex = 1;
      this.mpPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.mpPeriod.CustomFormat = "MMMM yyyy";
      this.mpPeriod.Format = DateTimePickerFormat.Custom;
      this.mpPeriod.Location = new Point(621, 3);
      this.mpPeriod.Name = "mpPeriod";
      this.mpPeriod.OldMonth = 0;
      this.mpPeriod.ShowUpDown = true;
      this.mpPeriod.Size = new Size(140, 22);
      this.mpPeriod.TabIndex = 0;
      this.mpPeriod.ValueChanged += new EventHandler(this.mpPeriod_ValueChanged);
      this.tcntrl.Controls.Add((Control) this.pgCorrect);
      this.tcntrl.Controls.Add((Control) this.pgCorrectMSP);
      this.tcntrl.Controls.Add((Control) this.pgCorrectCmp);
      this.tcntrl.Controls.Add((Control) this.pgOldCorrect);
      this.tcntrl.Dock = DockStyle.Fill;
      this.tcntrl.Location = new Point(0, 32);
      this.tcntrl.Name = "tcntrl";
      this.tcntrl.SelectedIndex = 0;
      this.tcntrl.Size = new Size(768, 250);
      this.tcntrl.TabIndex = 2;
      this.tcntrl.SelectedIndexChanged += new EventHandler(this.tcntrl_SelectedIndexChanged);
      this.pgCorrect.Controls.Add((Control) this.dgvCorrect);
      this.pgCorrect.Location = new Point(4, 25);
      this.pgCorrect.Name = "pgCorrect";
      this.pgCorrect.Padding = new Padding(3);
      this.pgCorrect.Size = new Size(760, 221);
      this.pgCorrect.TabIndex = 0;
      this.pgCorrect.Text = "Начисления";
      this.pgCorrect.UseVisualStyleBackColor = true;
      this.dgvCorrect.BackgroundColor = Color.AliceBlue;
      this.dgvCorrect.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvCorrect.ContextMenuStrip = this.cmsMenu;
      this.dgvCorrect.Dock = DockStyle.Fill;
      this.dgvCorrect.Location = new Point(3, 3);
      this.dgvCorrect.Name = "dgvCorrect";
      this.dgvCorrect.Size = new Size(754, 215);
      this.dgvCorrect.TabIndex = 3;
      this.dgvCorrect.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvCorrect_CellBeginEdit);
      this.dgvCorrect.CellEndEdit += new DataGridViewCellEventHandler(this.dgvCorrect_CellEndEdit);
      this.dgvCorrect.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dgvCorrect_CellMouseDoubleClick);
      this.dgvCorrect.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvCorrect_ColumnWidthChanged);
      this.dgvCorrect.DataError += new DataGridViewDataErrorEventHandler(this.dgvCorrect_DataError);
      this.cmsMenu.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.miCopy,
        (ToolStripItem) this.miCopyFromAnotherLS
      });
      this.cmsMenu.Name = "cmsMenu";
      this.cmsMenu.Size = new Size(313, 70);
      this.miCopy.Image = (Image) Resources.add_var;
      this.miCopy.Name = "miCopy";
      this.miCopy.Size = new Size(312, 22);
      this.miCopy.Text = "Скопировать запись в выбранные объекты";
      this.miCopy.Click += new EventHandler(this.miCopy_Click);
      this.miCopyFromAnotherLS.Image = (Image) Resources.add_var;
      this.miCopyFromAnotherLS.Name = "miCopyFromAnotherLS";
      this.miCopyFromAnotherLS.Size = new Size(312, 22);
      this.miCopyFromAnotherLS.Text = "Скопировать из другого лицевого счета";
      this.miCopyFromAnotherLS.Click += new EventHandler(this.miCopyFromAnotherLS_Click);
      this.pgCorrectMSP.Controls.Add((Control) this.dgvCorrectMSP);
      this.pgCorrectMSP.Location = new Point(4, 25);
      this.pgCorrectMSP.Name = "pgCorrectMSP";
      this.pgCorrectMSP.Padding = new Padding(3);
      this.pgCorrectMSP.Size = new Size(760, 221);
      this.pgCorrectMSP.TabIndex = 1;
      this.pgCorrectMSP.Text = "Льготы";
      this.pgCorrectMSP.UseVisualStyleBackColor = true;
      this.dgvCorrectMSP.BackgroundColor = Color.AliceBlue;
      this.dgvCorrectMSP.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvCorrectMSP.Dock = DockStyle.Fill;
      this.dgvCorrectMSP.Location = new Point(3, 3);
      this.dgvCorrectMSP.Name = "dgvCorrectMSP";
      this.dgvCorrectMSP.Size = new Size(754, 215);
      this.dgvCorrectMSP.TabIndex = 0;
      this.dgvCorrectMSP.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvCorrect_CellBeginEdit);
      this.dgvCorrectMSP.CellEndEdit += new DataGridViewCellEventHandler(this.dgvCorrectMSP_CellEndEdit);
      this.dgvCorrectMSP.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dgvCorrect_CellMouseDoubleClick);
      this.dgvCorrectMSP.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvCorrectMSP_ColumnWidthChanged);
      this.dgvCorrectMSP.CurrentCellDirtyStateChanged += new EventHandler(this.dgvCorrectMSP_CurrentCellDirtyStateChanged);
      this.dgvCorrectMSP.DataError += new DataGridViewDataErrorEventHandler(this.dgvCorrect_DataError);
      this.pgCorrectCmp.Controls.Add((Control) this.dgvCorrectCmp);
      this.pgCorrectCmp.Location = new Point(4, 25);
      this.pgCorrectCmp.Name = "pgCorrectCmp";
      this.pgCorrectCmp.Padding = new Padding(3);
      this.pgCorrectCmp.Size = new Size(760, 221);
      this.pgCorrectCmp.TabIndex = 2;
      this.pgCorrectCmp.Text = "Компенсация";
      this.pgCorrectCmp.UseVisualStyleBackColor = true;
      this.dgvCorrectCmp.BackgroundColor = Color.AliceBlue;
      this.dgvCorrectCmp.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvCorrectCmp.Dock = DockStyle.Fill;
      this.dgvCorrectCmp.Location = new Point(3, 3);
      this.dgvCorrectCmp.Name = "dgvCorrectCmp";
      this.dgvCorrectCmp.Size = new Size(754, 215);
      this.dgvCorrectCmp.TabIndex = 0;
      this.dgvCorrectCmp.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvCorrect_CellBeginEdit);
      this.dgvCorrectCmp.CellEndEdit += new DataGridViewCellEventHandler(this.dgvCorrect_CellEndEdit);
      this.dgvCorrectCmp.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dgvCorrect_CellMouseDoubleClick);
      this.dgvCorrectCmp.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvCorrectCmp_ColumnWidthChanged);
      this.dgvCorrectCmp.DataError += new DataGridViewDataErrorEventHandler(this.dgvCorrect_DataError);
      this.pgOldCorrect.Controls.Add((Control) this.dgvOldCorrect);
      this.pgOldCorrect.Location = new Point(4, 25);
      this.pgOldCorrect.Name = "pgOldCorrect";
      this.pgOldCorrect.Padding = new Padding(3);
      this.pgOldCorrect.Size = new Size(760, 221);
      this.pgOldCorrect.TabIndex = 3;
      this.pgOldCorrect.Text = "Коррективки прошлых начислений";
      this.pgOldCorrect.UseVisualStyleBackColor = true;
      this.dgvOldCorrect.BackgroundColor = Color.AliceBlue;
      this.dgvOldCorrect.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvOldCorrect.Dock = DockStyle.Fill;
      this.dgvOldCorrect.Location = new Point(3, 3);
      this.dgvOldCorrect.Name = "dgvOldCorrect";
      this.dgvOldCorrect.Size = new Size(754, 215);
      this.dgvOldCorrect.TabIndex = 0;
      this.dgvOldCorrect.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dgvCorrect_CellMouseDoubleClick);
      this.dgvOldCorrect.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvOldCorrect_ColumnWidthChanged);
      this.dgvOldCorrect.DataError += new DataGridViewDataErrorEventHandler(this.dgvCorrect_DataError);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(768, 322);
      this.Controls.Add((Control) this.tcntrl);
      this.Controls.Add((Control) this.pnUp);
      this.Controls.Add((Control) this.pnBtn);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmCorrect";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Корректировки";
      this.Shown += new EventHandler(this.FrmCorrect_Shown);
      this.pnBtn.ResumeLayout(false);
      this.pnUp.ResumeLayout(false);
      this.pnUp.PerformLayout();
      this.tcntrl.ResumeLayout(false);
      this.pgCorrect.ResumeLayout(false);
      ((ISupportInitialize) this.dgvCorrect).EndInit();
      this.cmsMenu.ResumeLayout(false);
      this.pgCorrectMSP.ResumeLayout(false);
      ((ISupportInitialize) this.dgvCorrectMSP).EndInit();
      this.pgCorrectCmp.ResumeLayout(false);
      ((ISupportInitialize) this.dgvCorrectCmp).EndInit();
      this.pgOldCorrect.ResumeLayout(false);
      ((ISupportInitialize) this.dgvOldCorrect).EndInit();
      this.ResumeLayout(false);
    }
  }
}
