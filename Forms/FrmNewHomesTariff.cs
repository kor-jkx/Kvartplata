// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmNewHomesTariff
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmNewHomesTariff : FrmBaseForm
  {
    private FormStateSaver fss = new FormStateSaver(FrmNewHomesTariff.ic);
    private string homesList = "";
    private IContainer components = (IContainer) null;
    private ISession session;
    private Company company;
    private static IContainer ic;
    private IList<cmpTariffCost> listCost;
    private cmpTariffCost tariffCost;
    private Tariff tariff;
    private ITransaction transaction;
    private Period monthClosed;
    private Norm norm;
    private string uslovie;
    private Panel pnBtn;
    private Button btnExit;
    private Label lblService;
    private ComboBox cmbService;
    private Label lblDate;
    private DateTimePicker dtpDate;
    private Label lblHomes;
    private CheckedListBox clbHomes;
    private DataGridView dgvTariff;
    private Label lblTariffs;
    private DataGridView dgvOldTariffs;
    private GroupBox gbTariff;
    private RadioButton rbOld;
    private RadioButton rbNew;
    private Button btnTariff;
    private DataGridView dgvSost;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private Panel pn;
    private TextBox txbNameTariff;
    private Label lblNameTariff;
    private TextBox txbTariff;
    private Label lblTariff;
    private Label lblNumTariff;
    private TextBox txbNumTariff;
    private ComboBox cmbBaseTariff;
    private Label lblBaseTariff;
    private ComboBox cmbIzm;
    private Label lblIzm;
    private Label lblSchemeParam;
    private Label lblScheme;
    private Button btnSchemeParam;
    private Button btnScheme;
    private ComboBox cmbBaseTariffMSP;
    private Label lblBaseTariffMSP;
    private Label lblOldTariff;
    private Label lblCalc;
    private RadioButton rbNewTariff;
    private Button btnCheckHomes;
    private Label lblNorm;
    private Button btnNorm;
    private DataGridViewTextBoxColumn Service;
    private DataGridViewButtonColumn Scheme;
    private DataGridViewTextBoxColumn Cost;
    private DataGridViewTextBoxColumn Cost_eo;
    private DataGridViewTextBoxColumn Cost_c;
    private CheckBox cbUchet;
    private CheckBox cbArhive;
    private GroupBox gbUsl;
    private GroupBox gbTarif;
    private GroupBox gpbFlat;
    private RadioButton rbtnNotPrivat;
    private RadioButton rbtnPrivat;
    private Label lblStreet;
    private ComboBox cmbStreet;
    private RadioButton rbAll;
    private CheckBox cbNorm;
    private Label lblVat;
    private ComboBox cmbVat;
    public HelpProvider hp;

    public FrmNewHomesTariff()
    {
      this.InitializeComponent();
    }

    public FrmNewHomesTariff(Company company)
    {
      this.InitializeComponent();
      this.company = company;
      this.fss.ParentForm = (Form) this;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void FrmNewHomesTariff_Load(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      this.dtpDate.Value = KvrplHelper.GetCmpKvrClose(this.company, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk).PeriodName.Value.AddMonths(1);
      this.LoadServices();
      this.LoadHomes();
      this.LoadSost();
      this.cmbBaseTariff.DataSource = (object) this.session.CreateCriteria(typeof (BaseTariff)).List<BaseTariff>();
      this.cmbBaseTariff.DisplayMember = "BaseTariff_name";
      this.cmbBaseTariff.ValueMember = "BaseTariff_id";
      this.cmbBaseTariffMSP.DataSource = (object) this.session.CreateQuery("from BaseTariff where BaseTariff_id in (1,2,3)").List<BaseTariff>();
      this.cmbBaseTariffMSP.DisplayMember = "BaseTariff_name";
      this.cmbBaseTariffMSP.ValueMember = "BaseTariff_id";
      this.cmbIzm.DataSource = (object) this.session.CreateQuery("from dcUnitMeasuring").List<dcUnitMeasuring>();
      this.cmbIzm.DisplayMember = "UnitMeasuring_name";
      this.cmbIzm.ValueMember = "UnitMeasuring_id";
      this.cmbVat.DataSource = (object) this.session.CreateQuery("from YesNo").List<YesNo>();
      this.cmbVat.DisplayMember = "YesNoName";
      this.cmbVat.ValueMember = "YesNoId";
      this.monthClosed = KvrplHelper.GetCmpKvrClose(this.company, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk);
      this.cmbIzm.SelectedValue = (object) 0;
      this.cmbBaseTariff.SelectedValue = (object) -1;
      this.cmbBaseTariffMSP.SelectedValue = (object) -1;
      this.cmbVat.SelectedValue = (object) 0;
      this.LoadStreet();
    }

    private void LoadServices()
    {
      string str = "";
      if (Options.Kvartplata && !Options.Arenda)
        str = string.Format("and Complex.IdFk={0}", (object) Options.Complex.IdFk);
      if (!Options.Kvartplata && Options.Arenda)
        str = string.Format("and Complex.IdFk={0}", (object) Options.ComplexArenda.IdFk);
      this.cmbService.DataSource = (object) this.session.CreateQuery("select s from Service s where ServiceId in (select Service_id from ServiceParam where Company_id=:company " + str + ") order by" + Options.SortService).SetParameter<short>("company", this.company.CompanyId).List<Kvartplata.Classes.Service>();
      this.cmbService.DisplayMember = "ServiceName";
      this.cmbService.ValueMember = "ServiceId";
    }

    private void LoadHomes()
    {
      string str = "";
      if (this.cmbStreet.SelectedIndex != -1 && (uint) this.cmbStreet.SelectedIndex > 0U)
        str = "and h.Str.IdStr=" + ((Str) this.cmbStreet.SelectedItem).IdStr.ToString();
      this.clbHomes.DataSource = (object) this.session.CreateQuery(string.Format("select h from Home h left join fetch h.Str,Transfer t,HomeLink hl where hl.Company.CompanyId={0} and hl.Company=t.Company and t.KvrCmp is not null and hl.Home=h " + Options.HomeType + " and h.IdHome in (select ls.Home.IdHome from LsClient ls where 1=1 " + Options.MainConditions1 + ") " + str + " order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome),h.HomeKorp,h.IdHome", (object) this.company.CompanyId)).List<Home>();
      this.clbHomes.DisplayMember = "Address";
      this.clbHomes.ValueMember = "IdHome";
    }

    private void LoadStreet()
    {
      IList<Str> strList = this.session.CreateQuery(string.Format("select s from Str s where IdStr in (select Str.IdStr from Home where IdHome in (select Home.IdHome from HomeLink where Company.CompanyId={0}))", (object) this.company.CompanyId)).List<Str>();
      strList.Insert(0, new Str()
      {
        IdStr = 0,
        NameStr = ""
      });
      this.cmbStreet.DataSource = (object) strList;
      this.cmbStreet.DisplayMember = "NameStr";
      this.cmbStreet.ValueMember = "IdStr";
    }

    private void clbHomes_MouseUp(object sender, MouseEventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      this.LoadTariffs();
      this.dgvTariff_CellClick((object) null, (DataGridViewCellEventArgs) null);
      this.Cursor = Cursors.Default;
    }

    private void LoadTariffs()
    {
      this.homesList = "";
      this.dgvTariff.Columns.Clear();
      this.dgvTariff.DataSource = (object) null;
      foreach (Home checkedItem in this.clbHomes.CheckedItems)
        this.homesList = this.homesList + checkedItem.IdHome.ToString() + ",";
      if (!(this.homesList != ""))
        return;
      this.homesList = this.homesList.Substring(0, this.homesList.Length - 1);
      IList<cmpTariffCost> cmpTariffCostList1 = (IList<cmpTariffCost>) new List<cmpTariffCost>();
      IList<cmpTariffCost> cmpTariffCostList2 = this.session.CreateQuery(string.Format("select new cmpTariffCost(ct.Tariff_id,ct.Cost,(select count(Client) from LsService where Client.ClientId in (select ClientId from LsClient where Company.CompanyId={2} and IdHome not in (" + this.homesList + ")) and Period.PeriodId=0 and Service.ServiceId=ct.Service.ServiceId and DBeg<='{0}' and DEnd>='{0}' and Tariff.Tariff_id=ct.Tariff_id),0) from cmpTariffCost ct,Tariff t where t.Tariff_id=ct.Tariff_id and ct.Company_id=(select ParamValue from CompanyParam where Company.CompanyId={2} and Period.PeriodId=0 and DBeg<='{0}' and DEnd>='{0}' and Param.ParamId=201) and ct.Period.PeriodId=0 and ct.Tariff_id in (select Tariff.Tariff_id from LsService where Client.ClientId in (select ls.ClientId from LsClient ls where ls.Company.CompanyId={2} and ls.Home.IdHome in (" + this.homesList + ")" + Options.MainConditions1 + ") and Period.PeriodId=0 and Service.ServiceId=ct.Service.ServiceId and DBeg<='{0}' and DEnd>='{0}') and ct.Dbeg<='{0}' and ct.Dend>='{0}' and ct.Service.ServiceId={1} order by t.Tariff_num", (object) KvrplHelper.DateToBaseFormat(this.dtpDate.Value), (object) ((Kvartplata.Classes.Service) this.cmbService.SelectedItem).ServiceId, (object) this.company.CompanyId)).List<cmpTariffCost>();
      foreach (cmpTariffCost cmpTariffCost in (IEnumerable<cmpTariffCost>) cmpTariffCostList2)
        cmpTariffCost.AllInfo = KvrplHelper.GetTariffInfo(cmpTariffCost.Tariff_id);
      this.dgvTariff.DataSource = (object) cmpTariffCostList2;
      for (int index = 0; index < this.dgvTariff.Columns.Count; ++index)
      {
        if (this.dgvTariff.Columns[index].Name != "AllInfo" && this.dgvTariff.Columns[index].Name != "Cost")
          this.dgvTariff.Columns[index].Visible = false;
      }
      this.dgvTariff.Columns["Cost"].HeaderText = "Цена";
      this.dgvTariff.Columns["AllInfo"].HeaderText = "Вариант";
      this.dgvTariff.Columns["AllInfo"].DisplayIndex = 0;
      this.dgvTariff.Columns["AllInfo"].Width = 300;
    }

    private void cmbService_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.tariff = (Tariff) null;
      this.norm = (Norm) null;
      this.txbTariff.Clear();
      this.btnNorm.Text = "";
      this.ClearFields();
      this.LoadTariffs();
      this.LoadSost();
    }

    private void dgvTariff_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (this.dgvTariff.DataSource == null)
        return;
      DataGridViewRow row = (sender as DataGridView).Rows[e.RowIndex];
      short? scheme = ((cmpTariffCost) row.DataBoundItem).Scheme;
      int? nullable = scheme.HasValue ? new int?((int) scheme.GetValueOrDefault()) : new int?();
      int num = 0;
      if (nullable.GetValueOrDefault() != num || !nullable.HasValue)
        row.DefaultCellStyle.ForeColor = Color.Gray;
      else
        row.DefaultCellStyle.ForeColor = Color.Black;
    }

    private void LoadSost()
    {
      IList<Kvartplata.Classes.Service> serviceList = this.session.CreateCriteria(typeof (Kvartplata.Classes.Service)).Add((ICriterion) Restrictions.Eq("Root", (object) ((Kvartplata.Classes.Service) this.cmbService.SelectedItem).ServiceId)).AddOrder(Order.Asc("ServiceId")).List<Kvartplata.Classes.Service>();
      this.listCost = (IList<cmpTariffCost>) new List<cmpTariffCost>();
      foreach (Kvartplata.Classes.Service service in (IEnumerable<Kvartplata.Classes.Service>) serviceList)
        this.listCost.Add(new cmpTariffCost()
        {
          Service = service,
          Cost = new double?(0.0),
          Cost_c = new double?(0.0),
          Cost_eo = new double?(0.0),
          Company_id = (int) this.company.CompanyId,
          Complex_id = Options.Complex.ComplexId,
          AllInfo = service.ServiceName,
          Scheme = new short?(Convert.ToInt16(0))
        });
      this.dgvSost.RowCount = this.listCost.Count;
      this.dgvSost.Refresh();
    }

    private void btnTariff_Click(object sender, EventArgs e)
    {
      if (this.rbOld.Checked && !KvrplHelper.CheckProxy(40, 2, this.company, true) || this.rbNewTariff.Checked && !KvrplHelper.CheckProxy(33, 2, this.company, true) || (this.rbNew.Checked && (!KvrplHelper.CheckProxy(33, 2, this.company, true) || !KvrplHelper.CheckProxy(40, 2, this.company, true)) || MessageBox.Show("Вы уверены, что хотите изменить выбранные тарифы в указанных домах?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK))
        return;
      bool flag = false;
      double num1 = 0.0;
      int period = 0;
      this.session.Clear();
      this.dgvSost.EndEdit();
      IList<cmpTariffCost> cmpTariffCostList = (IList<cmpTariffCost>) new List<cmpTariffCost>();
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgvTariff.SelectedRows)
      {
        short? scheme = ((cmpTariffCost) selectedRow.DataBoundItem).Scheme;
        int? nullable = scheme.HasValue ? new int?((int) scheme.GetValueOrDefault()) : new int?();
        int num2 = 0;
        if (nullable.GetValueOrDefault() != num2 || !nullable.HasValue)
          flag = true;
        cmpTariffCostList.Add((cmpTariffCost) selectedRow.DataBoundItem);
      }
      if (cmpTariffCostList.Count == 0 && this.rbNewTariff.Checked)
      {
        int num3 = (int) MessageBox.Show("Выберите варианты которые необходимо заменить", "", MessageBoxButtons.OK);
      }
      else if (this.clbHomes.CheckedItems.Count == 0)
      {
        int num4 = (int) MessageBox.Show("Выберите дома", "", MessageBoxButtons.OK);
      }
      else if (this.txbTariff.Text == "")
      {
        int num5 = (int) MessageBox.Show("Введите разбивку по составляющим и тариф", "Внимание", MessageBoxButtons.OK);
      }
      else
      {
        foreach (cmpTariffCost cmpTariffCost in (IEnumerable<cmpTariffCost>) this.listCost)
        {
          double? cost = cmpTariffCost.Cost;
          if (cost.HasValue)
          {
            double num2 = num1;
            cost = cmpTariffCost.Cost;
            double num6 = cost.Value;
            num1 = num2 + num6;
          }
        }
        if (Math.Round(num1, 4) != Convert.ToDouble(KvrplHelper.ChangeSeparator(this.txbTariff.Text)))
        {
          int num7 = (int) MessageBox.Show("Сумма тарифа не равна разбивке по составляющим", "Внимание", MessageBoxButtons.OK);
        }
        else if (this.rbNew.Checked && this.dgvOldTariffs.SelectedRows.Count == 0 && (this.txbNumTariff.Text == "" || this.txbNameTariff.Text == "" || (this.cmbIzm.SelectedValue == null || this.cmbBaseTariff.SelectedValue == null) || (this.cmbBaseTariffMSP.SelectedValue == null || this.btnScheme.Text == "" || this.btnSchemeParam.Text == "") || this.cmbVat.SelectedValue == null))
        {
          int num8 = (int) MessageBox.Show("Не все поля заполнены", "Внимание", MessageBoxButtons.OK);
        }
        else
        {
          DateTime dateTime1 = this.dtpDate.Value;
          DateTime dateTime2 = this.monthClosed.PeriodName.Value;
          DateTime dateTime3 = dateTime2.AddMonths(1);
          DateTime dBeg;
          DateTime dateTime4;
          DateTime dateTime5;
          DateTime dEnd;
          if (dateTime1 < dateTime3)
          {
            if (MessageBox.Show("Введенная дата принадлежит закрытому периоду. Внести записи в настоящее и прошлое время?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
              return;
            period = this.monthClosed.PeriodId + 1;
            DateTime? periodName = this.monthClosed.PeriodName;
            dateTime2 = periodName.Value;
            dBeg = dateTime2.AddMonths(1);
            dateTime4 = this.dtpDate.Value;
            dateTime5 = Convert.ToDateTime("2999-12-31");
            periodName = this.monthClosed.PeriodName;
            dateTime2 = periodName.Value;
            dateTime2 = dateTime2.AddMonths(1);
            dEnd = dateTime2.AddDays(-1.0);
          }
          else
          {
            dBeg = this.dtpDate.Value;
            dateTime5 = Convert.ToDateTime("2999-12-31");
            dateTime4 = Convert.ToDateTime("2999-12-31");
            dEnd = Convert.ToDateTime("2999-12-31");
          }
          string str = this.cbArhive.Checked || this.cbUchet.Checked || (this.rbtnPrivat.Checked || this.rbtnNotPrivat.Checked) ? string.Format(" and p.DBeg <= today() and p.DEnd >= today() ") : "";
          this.uslovie = "";
          if (this.cbUchet.Checked && this.cbArhive.Checked)
          {
            this.uslovie = "3,4,5";
          }
          else
          {
            if (this.cbUchet.Checked)
              this.uslovie = "3";
            if (this.cbArhive.Checked)
              this.uslovie = "4,5";
          }
          if (this.uslovie != "")
            this.uslovie = string.Format(" and isnull((select p.param_value from LsParam p where p.param_id=107 and p.client_id=ls.client_id {0}),0) not in (" + this.uslovie + ")", (object) str);
          this.uslovie = this.uslovie + (this.rbtnPrivat.Checked ? string.Format(" and exists (select p.Client_Id from DBA.lsParam p where p.Param_Id=104 and p.Client_Id=ls.Client_Id and p.Param_Value in (2,5,6,7,8,12,14,15,17,18,21,25,28,29,31,33,34,35) {0})", (object) str) : "");
          this.uslovie = this.uslovie + (this.rbtnNotPrivat.Checked ? string.Format(" and exists (select p.Client_Id from DBA.lsParam p where p.Param_Id=104 and p.Client_Id=ls.Client_Id and p.Param_Value not in (2,5,6,7,8,12,14,15,17,18,21,25,28,29,31,33,34,35) {0})", (object) str) : "");
          using (this.transaction = this.session.BeginTransaction())
          {
            if (this.rbNew.Checked && this.tariff.Tariff_id == 0)
            {
              if (!this.AddVariant())
              {
                this.transaction.Rollback();
                this.tariff.Tariff_id = 0;
                return;
              }
              if (!this.AddTariff(0, dBeg, dateTime5))
              {
                this.transaction.Rollback();
                this.tariff.Tariff_id = 0;
                return;
              }
              if (period != 0 && !this.AddTariff(period, dateTime4, dEnd))
              {
                this.transaction.Rollback();
                this.tariff.Tariff_id = 0;
                return;
              }
            }
            try
            {
              if (cmpTariffCostList.Count > 0)
              {
                foreach (cmpTariffCost tcost in (IEnumerable<cmpTariffCost>) cmpTariffCostList)
                {
                  if (this.rbOld.Checked || this.rbNew.Checked)
                  {
                    if (!this.NewLsService(tcost, 0, dBeg, dateTime5))
                    {
                      this.transaction.Rollback();
                      return;
                    }
                    if (period != 0 && !this.NewLsService(tcost, period, dateTime4, dEnd))
                    {
                      this.transaction.Rollback();
                      return;
                    }
                  }
                  else
                  {
                    if (!this.AddTariffInsteadOld(tcost, 0, dBeg, dateTime5))
                    {
                      this.transaction.Rollback();
                      return;
                    }
                    if (period != 0 && !this.AddTariffInsteadOld(tcost, period, dateTime4, dEnd))
                    {
                      this.transaction.Rollback();
                      return;
                    }
                  }
                }
              }
              else if (this.rbOld.Checked || this.rbNew.Checked)
              {
                if (!this.NewLsService1(0, dBeg, dateTime5))
                {
                  this.transaction.Rollback();
                  return;
                }
                if (period != 0 && !this.NewLsService1(period, dateTime4, dEnd))
                {
                  this.transaction.Rollback();
                  return;
                }
              }
            }
            catch (Exception ex)
            {
              int num2 = (int) MessageBox.Show("Не удалось сохранить тариф и составляющие", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              KvrplHelper.WriteLog(ex, (LsClient) null);
              this.transaction.Rollback();
              return;
            }
            this.transaction.Commit();
            int num6 = (int) MessageBox.Show("Операция успешно завершена", "", MessageBoxButtons.OK);
          }
        }
      }
    }

    private void dataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.listCost.Count <= 0 || e.RowIndex >= this.listCost.Count)
        return;
      if (this.dgvSost.Columns[e.ColumnIndex].Name == "Service")
        e.Value = (object) this.listCost[e.RowIndex].Service.ServiceName;
      else if (this.dgvSost.Columns[e.ColumnIndex].Name == "Scheme")
        e.Value = (object) this.listCost[e.RowIndex].Scheme;
      else if (this.dgvSost.Columns[e.ColumnIndex].Name == "Cost")
        e.Value = (object) this.listCost[e.RowIndex].Cost;
      else if (this.dgvSost.Columns[e.ColumnIndex].Name == "Cost_eo")
        e.Value = (object) this.listCost[e.RowIndex].Cost_eo;
      else if (this.dgvSost.Columns[e.ColumnIndex].Name == "Cost_c")
        e.Value = (object) this.listCost[e.RowIndex].Cost_c;
    }

    private void dataGridView1_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      if (this.dgvSost.Columns[e.ColumnIndex].Name == "Cost" && e.Value != null)
      {
        try
        {
          this.listCost[e.RowIndex].Cost = new double?(Convert.ToDouble(KvrplHelper.ChangeSeparator(e.Value.ToString())));
        }
        catch
        {
          int num = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
        }
      }
      else if (this.dgvSost.Columns[e.ColumnIndex].Name == "Scheme")
      {
        try
        {
          this.listCost[e.RowIndex].Scheme = e.Value != null ? new short?(Convert.ToInt16(e.Value)) : new short?();
        }
        catch
        {
          int num = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
        }
      }
      else if (this.dgvSost.Columns[e.ColumnIndex].Name == "Cost_eo" && e.Value != null)
      {
        try
        {
          this.listCost[e.RowIndex].Cost_eo = new double?(Convert.ToDouble(e.Value));
        }
        catch
        {
          int num = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
        }
      }
      else if (this.dgvSost.Columns[e.ColumnIndex].Name == "Cost_c" && e.Value != null)
      {
        try
        {
          this.listCost[e.RowIndex].Cost_c = new double?(Convert.ToDouble(e.Value));
        }
        catch
        {
          int num = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
        }
      }
    }

    private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      short id = 0;
      if (e.ColumnIndex <= 0 || e.RowIndex < 0 || !(this.dgvSost.Columns[e.ColumnIndex].Name == "Scheme"))
        return;
      if (this.listCost[e.RowIndex].Scheme.HasValue)
        id = this.listCost[e.RowIndex].Scheme.Value;
      FrmScheme frmScheme = new FrmScheme((short) 5, id);
      if (frmScheme.ShowDialog() == DialogResult.OK)
        id = frmScheme.CurrentId();
      this.listCost[e.RowIndex].Scheme = new short?(id);
      frmScheme.Dispose();
    }

    private void rbOld_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.rbOld.Checked)
        return;
      this.Cursor = Cursors.WaitCursor;
      this.txbTariff.Clear();
      this.ClearFields();
      this.dgvOldTariffs.Columns.Clear();
      this.dgvOldTariffs.DataSource = (object) null;
      FrmClientCardChoose clientCardChoose = new FrmClientCardChoose((Kvartplata.Classes.Service) this.cmbService.SelectedItem, new Tariff(), (int) this.company.CompanyId, new DateTime?(this.dtpDate.Value));
      int num = (int) clientCardChoose.ShowDialog();
      this.Cursor = Cursors.Default;
      if (clientCardChoose.Id != -1)
      {
        this.tariff = this.session.Get<Tariff>((object) clientCardChoose.Id);
        try
        {
          this.tariffCost = this.session.CreateQuery(string.Format("from cmpTariffCost where Tariff_id={0} and ((Period.PeriodId=0 and dbeg<='{1}' and dend>='{1}') or (Period.PeriodId<>0 and dbeg<='{1}' and dend>='{1}' )) and Service.ServiceId={2} and Company_Id=((select ParamValue from CompanyParam where Company.CompanyId={3} and Period.PeriodId=0 and DBeg<='{1}' and DEnd>='{1}' and Param.ParamId=201))", (object) this.tariff.Tariff_id, (object) KvrplHelper.DateToBaseFormat(this.dtpDate.Value), (object) ((Kvartplata.Classes.Service) this.cmbService.SelectedItem).ServiceId, (object) this.company.CompanyId)).List<cmpTariffCost>()[0];
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        this.LoadTariff();
        this.cbNorm.Enabled = true;
      }
      clientCardChoose.Dispose();
    }

    private void rbNew_CheckedChanged(object sender, EventArgs e)
    {
      if (this.rbNew.Checked)
      {
        this.pn.Enabled = true;
        this.txbTariff.Clear();
        this.ClearFields();
        this.txbTariff.Focus();
      }
      else
        this.pn.Enabled = false;
    }

    private void txbTariff_KeyPress(object sender, KeyPressEventArgs e)
    {
      if ((int) e.KeyChar == 8 || (int) e.KeyChar == 13 || ((int) e.KeyChar == 44 || (int) e.KeyChar == 46) || (int) e.KeyChar >= 48 && (int) e.KeyChar <= 57)
        return;
      e.Handled = true;
    }

    private void ClearFields()
    {
      this.txbNumTariff.Clear();
      this.txbNameTariff.Clear();
      this.btnScheme.Text = "";
      this.btnSchemeParam.Text = "";
      this.cmbIzm.SelectedValue = (object) -1;
      this.cmbBaseTariff.SelectedValue = (object) -1;
      this.cmbBaseTariffMSP.SelectedValue = (object) -1;
      this.cmbVat.SelectedValue = (object) -1;
      this.listCost = (IList<cmpTariffCost>) new List<cmpTariffCost>();
      this.LoadSost();
    }

    private void txbTariff_Leave(object sender, EventArgs e)
    {
      try
      {
        if (this.listCost.Count == 1)
          this.listCost[0].Cost = new double?(Convert.ToDouble(this.txbTariff.Text));
        this.EnterTariff();
      }
      catch
      {
      }
    }

    private void txbTariff_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return && e.KeyCode != Keys.Tab || (!(this.txbTariff.Text != "") || this.dgvSost.Rows.Count <= 0))
        return;
      this.dgvSost.CurrentCell = this.dgvSost.Rows[0].Cells[2];
      this.dgvSost.Focus();
    }

    private void EnterTariff()
    {
      if (!this.rbNew.Checked)
        return;
      this.LoadOldTariffs();
      this.EnterNewTariff();
    }

    private void EnterNewTariff()
    {
      if (this.txbTariff.Text != "")
      {
        try
        {
          this.tariff = new Tariff();
          object obj = this.session.CreateQuery(string.Format("select max(t.Tariff_num) from Tariff t where t.Service=:serv and t.Manager.BaseOrgId=:org")).SetEntity("serv", (object) (Kvartplata.Classes.Service) this.cmbService.SelectedItem).SetParameter<int>("org", this.company.Manager.BaseOrgId).UniqueResult();
          this.tariff.Tariff_num = obj != null ? (int) obj + 1 : 1;
          this.tariff.Tariff_name = "Новый вариант";
          this.tariff.Manager = this.company.Manager;
          this.tariffCost = new cmpTariffCost();
          this.tariffCost.Tariff_id = this.tariff.Tariff_id;
          this.tariffCost.Cost = new double?(Convert.ToDouble(KvrplHelper.ChangeSeparator(this.txbTariff.Text)));
          this.tariffCost.Cost_c = new double?(0.0);
          this.tariffCost.Cost_eo = new double?(0.0);
          this.tariffCost.Scheme = new short?((short) 0);
          this.tariffCost.SchemeParam = new short?((short) 0);
          this.LoadTariff();
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      else
      {
        this.tariff = (Tariff) null;
        this.ClearFields();
      }
    }

    private void LoadOldTariffs()
    {
      try
      {
        IList<cmpTariffCost> cmpTariffCostList = this.session.CreateQuery("select ct from cmpTariffCost ct,Tariff t where ct.Tariff_id=t.Tariff_id and ct.Company_id=(select ParamValue from CompanyParam where Company.CompanyId=:company and Period.PeriodId=0 and DBeg<=:dbeg and DEnd>=:dend and Param.ParamId=201) and ct.Period.PeriodId=0 and ct.Service.ServiceId=:service and ct.Dbeg<=:dbeg and ct.Dend>=:dend and ct.Cost=:cost order by t.Tariff_num").SetParameter<short>("company", Options.Company.CompanyId).SetDateTime("dbeg", Options.Period.PeriodName.Value).SetDateTime("dend", Options.Period.PeriodName.Value).SetDouble("cost", Convert.ToDouble(KvrplHelper.ChangeSeparator(this.txbTariff.Text))).SetParameter<short>("service", ((Kvartplata.Classes.Service) this.cmbService.SelectedItem).ServiceId).List<cmpTariffCost>();
        if (cmpTariffCostList.Count > 0)
        {
          int num = (int) MessageBox.Show("В справочнике найдены записи с аналогичным тарифом. Выберите запись из списка, если она подходит.", "", MessageBoxButtons.OK);
        }
        foreach (cmpTariffCost cmpTariffCost in (IEnumerable<cmpTariffCost>) cmpTariffCostList)
          cmpTariffCost.AllInfo = KvrplHelper.GetTariffInfo(cmpTariffCost.Tariff_id);
        this.dgvOldTariffs.Columns.Clear();
        this.dgvOldTariffs.DataSource = (object) null;
        this.dgvOldTariffs.DataSource = (object) cmpTariffCostList;
        for (int index = 0; index < this.dgvOldTariffs.Columns.Count; ++index)
        {
          if (this.dgvOldTariffs.Columns[index].Name != "AllInfo")
            this.dgvOldTariffs.Columns[index].Visible = false;
        }
        this.dgvOldTariffs.Columns["AllInfo"].HeaderText = "Вариант";
        this.dgvOldTariffs.Columns["AllInfo"].Width = 300;
        this.txbNumTariff.Enabled = true;
        this.txbNameTariff.Enabled = true;
        this.cmbIzm.Enabled = true;
        this.cmbBaseTariff.Enabled = true;
        this.cmbBaseTariffMSP.Enabled = true;
        this.cmbVat.Enabled = true;
        this.btnScheme.Enabled = true;
        this.btnSchemeParam.Enabled = true;
        this.cbNorm.Enabled = true;
        this.ClearFields();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void dgvOldTariffs_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgvOldTariffs.Rows.Count <= 0)
        return;
      this.tariffCost = (cmpTariffCost) this.dgvOldTariffs.CurrentRow.DataBoundItem;
      this.tariff = this.session.Get<Tariff>((object) this.tariffCost.Tariff_id);
      this.LoadTariff();
      this.txbNumTariff.Enabled = false;
      this.txbNameTariff.Enabled = false;
      this.cmbIzm.Enabled = false;
      this.cmbBaseTariff.Enabled = false;
      this.cmbBaseTariffMSP.Enabled = false;
      this.cmbVat.Enabled = false;
      this.btnScheme.Enabled = false;
      this.btnSchemeParam.Enabled = false;
    }

    private void LoadTariff()
    {
      if (this.tariffCost == null)
        return;
      this.txbTariff.Text = this.tariffCost.Cost.Value.ToString();
      this.txbNameTariff.Text = this.tariff.Tariff_name;
      this.txbNumTariff.Text = this.tariff.Tariff_num.ToString();
      int tariffId = this.tariff.Tariff_id;
      if ((uint) this.tariff.Tariff_id > 0U)
        this.listCost = this.session.CreateQuery(string.Format("from cmpTariffCost where Tariff_id={0} and dbeg<='{1}' and dend>='{1}' and Service.ServiceId in (select ServiceId from Service where Root={2}) and Company_id=(select ParamValue from CompanyParam where Company.CompanyId={3} and Period.PeriodId=0 and DBeg<='{1}' and DEnd>='{1}' and Param.ParamId=201) order by Service.ServiceId", (object) this.tariff.Tariff_id, (object) KvrplHelper.DateToBaseFormat(this.dtpDate.Value), (object) ((Kvartplata.Classes.Service) this.cmbService.SelectedItem).ServiceId, (object) this.company.CompanyId)).List<cmpTariffCost>();
      this.btnScheme.Text = this.tariffCost.Scheme.ToString();
      this.btnSchemeParam.Text = this.tariffCost.SchemeParam.ToString();
      if (this.tariffCost.UnitMeasuring_id.HasValue)
        this.cmbIzm.SelectedValue = (object) this.tariffCost.UnitMeasuring_id.Value;
      if (this.tariffCost.BaseTariff_id.HasValue)
        this.cmbBaseTariff.SelectedValue = (object) this.tariffCost.BaseTariff_id;
      if (this.tariffCost.BaseTariffMSP_id.HasValue)
        this.cmbBaseTariffMSP.SelectedValue = (object) this.tariffCost.BaseTariffMSP_id;
      if (this.tariffCost.IsVat != null)
        this.cmbVat.SelectedValue = (object) this.tariffCost.IsVat.YesNoId;
      this.dgvSost.Refresh();
    }

    private void dgvTariff_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      bool flag = false;
      if (this.dgvTariff.Rows.Count == 0)
        this.rbNewTariff.Enabled = false;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgvTariff.SelectedRows)
      {
        short? scheme = ((cmpTariffCost) selectedRow.DataBoundItem).Scheme;
        int? nullable = scheme.HasValue ? new int?((int) scheme.GetValueOrDefault()) : new int?();
        int num = 0;
        if (nullable.GetValueOrDefault() != num || !nullable.HasValue)
          flag = true;
      }
      if (flag || this.dgvTariff.Rows.Count == 0)
      {
        this.rbNewTariff.Checked = false;
        this.rbNewTariff.Enabled = false;
        this.tariff = (Tariff) null;
        this.txbTariff.Clear();
        this.ClearFields();
      }
      else
        this.rbNewTariff.Enabled = true;
    }

    private void btnScheme_Click(object sender, EventArgs e)
    {
      short id = 0;
      if (this.tariffCost.Scheme.HasValue)
        id = this.tariffCost.Scheme.Value;
      FrmScheme frmScheme = new FrmScheme((short) 1, id);
      if (frmScheme.ShowDialog() == DialogResult.OK)
        id = frmScheme.CurrentId();
      this.tariffCost.Scheme = new short?(id);
      this.btnScheme.Text = id.ToString();
      frmScheme.Dispose();
    }

    private void btnSchemeParam_Click(object sender, EventArgs e)
    {
      short id = 0;
      if (this.tariffCost.SchemeParam.HasValue)
        id = this.tariffCost.SchemeParam.Value;
      FrmScheme frmScheme = new FrmScheme((short) 2, id);
      if (frmScheme.ShowDialog() == DialogResult.OK)
        id = frmScheme.CurrentId();
      this.tariffCost.SchemeParam = new short?(id);
      this.btnSchemeParam.Text = id.ToString();
      frmScheme.Dispose();
    }

    private void cmbIzm_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.tariffCost.UnitMeasuring_id = new int?(((dcUnitMeasuring) this.cmbIzm.SelectedItem).UnitMeasuring_id);
    }

    private void cmbBaseTariff_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.tariffCost.BaseTariff_id = new int?(((BaseTariff) this.cmbBaseTariff.SelectedItem).BaseTariff_id);
    }

    private void cmbBaseTariffMSP_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.tariffCost.BaseTariffMSP_id = new int?(((BaseTariff) this.cmbBaseTariffMSP.SelectedItem).BaseTariff_id);
    }

    private void rbNewTariff_CheckedChanged(object sender, EventArgs e)
    {
      if (this.rbNewTariff.Checked)
      {
        this.pn.Enabled = true;
        this.txbTariff.Clear();
        this.dgvOldTariffs.Columns.Clear();
        this.dgvOldTariffs.DataSource = (object) null;
        this.ClearFields();
        this.txbNumTariff.Enabled = false;
        this.txbNameTariff.Enabled = false;
        this.cmbIzm.Enabled = false;
        this.cmbBaseTariff.Enabled = false;
        this.cmbBaseTariffMSP.Enabled = false;
        this.cmbVat.Enabled = false;
        this.btnScheme.Enabled = false;
        this.btnSchemeParam.Enabled = false;
        this.cbNorm.Enabled = true;
      }
      else
        this.pn.Enabled = false;
    }

    private bool AddVariant()
    {
      try
      {
        this.tariff.Tariff_id = this.session.CreateSQLQuery("Select DBA.Gen_id('dcTariff',1)").UniqueResult<int>();
        this.tariff.Service = (Kvartplata.Classes.Service) this.cmbService.SelectedItem;
        this.tariff.Uname = Options.Login;
        this.tariff.Dedit = DateTime.Now;
        this.session.Save((object) this.tariff);
        this.session.Flush();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Не удалось сохранить новый вариант услуги", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        KvrplHelper.WriteLog(ex, (LsClient) null);
        return false;
      }
      return true;
    }

    private bool AddTariff(int period, DateTime dBeg, DateTime dEnd)
    {
      try
      {
        double val1 = 0.0;
        double val2 = 0.0;
        double val3 = 0.0;
        foreach (cmpTariffCost cmpTariffCost in (IEnumerable<cmpTariffCost>) this.listCost)
        {
          IQuery query1 = this.session.CreateSQLQuery("insert into cmpTariff values (:tariff,:service,(select first param_value from cmpParam where company_id=:company and period_id=0 and DBeg<=:dbeg and DEnd>=:dend and param_id=201),:period,:dbeg,:dend,:unitmeasuring,:basetariff,:scheme,:cost,:cost_eo,:cost_c,:complex,user,today(),:basetariffmsp,:scheme_param,:vat) ").SetDateTime("dbeg", dBeg).SetDateTime("dend", dEnd).SetParameter<int>("tariff", this.tariff.Tariff_id).SetParameter<short>("service", cmpTariffCost.Service.ServiceId).SetParameter<int>("period", period).SetParameter<int>("unitmeasuring", ((dcUnitMeasuring) this.cmbIzm.SelectedItem).UnitMeasuring_id).SetParameter<int>("basetariff", ((BaseTariff) this.cmbBaseTariff.SelectedItem).BaseTariff_id).SetParameter<int>("basetariffmsp", ((BaseTariff) this.cmbBaseTariffMSP.SelectedItem).BaseTariff_id).SetParameter<short>("vat", ((YesNo) this.cmbVat.SelectedItem).YesNoId).SetParameter<string>("scheme", this.btnScheme.Text).SetParameter<string>("scheme_param", this.btnSchemeParam.Text).SetParameter<int>("complex", Options.Complex.ComplexId).SetParameter<short>("company", this.company.CompanyId);
          string name1 = "cost";
          double? nullable = cmpTariffCost.Cost;
          double val4 = nullable.Value;
          IQuery query2 = query1.SetDouble(name1, val4);
          string name2 = "cost_eo";
          nullable = cmpTariffCost.Cost_eo;
          double val5 = nullable.Value;
          IQuery query3 = query2.SetDouble(name2, val5);
          string name3 = "cost_c";
          nullable = cmpTariffCost.Cost_c;
          double val6 = nullable.Value;
          query3.SetDouble(name3, val6).ExecuteUpdate();
          val1 += Convert.ToDouble((object) cmpTariffCost.Cost);
          val3 += Convert.ToDouble((object) cmpTariffCost.Cost_eo);
          val2 += Convert.ToDouble((object) cmpTariffCost.Cost_c);
        }
        this.session.CreateSQLQuery("insert into cmpTariff values (:tariff,:service,(select first param_value from cmpParam where company_id=:company and period_id=0 and DBeg<=:dbeg and DEnd>=:dend and param_id=201),:period,:dbeg,:dend,:unitmeasuring,:basetariff,:scheme,:cost,:cost_eo,:cost_c,:complex,user,today(),:basetariffmsp,:scheme_param,:vat) ").SetDateTime("dbeg", dBeg).SetDateTime("dend", dEnd).SetParameter<int>("tariff", this.tariff.Tariff_id).SetParameter<short>("service", ((Kvartplata.Classes.Service) this.cmbService.SelectedItem).ServiceId).SetParameter<int>("period", period).SetParameter<int>("unitmeasuring", ((dcUnitMeasuring) this.cmbIzm.SelectedItem).UnitMeasuring_id).SetParameter<int>("basetariff", ((BaseTariff) this.cmbBaseTariff.SelectedItem).BaseTariff_id).SetParameter<int>("basetariffmsp", ((BaseTariff) this.cmbBaseTariffMSP.SelectedItem).BaseTariff_id).SetParameter<short>("vat", ((YesNo) this.cmbVat.SelectedItem).YesNoId).SetParameter<string>("scheme", this.btnScheme.Text).SetParameter<string>("scheme_param", this.btnSchemeParam.Text).SetParameter<int>("complex", Options.Complex.ComplexId).SetParameter<short>("company", this.company.CompanyId).SetDouble("cost", val1).SetDouble("cost_eo", val3).SetDouble("cost_c", val2).ExecuteUpdate();
        try
        {
          Company company = this.session.Get<Company>((object) Convert.ToInt16(this.session.CreateQuery(string.Format("select ParamValue from CompanyParam where Company.CompanyId={0} and Period.PeriodId=0 and DBeg<='{1}' and DEnd>='{2}' and Param.ParamId=201", (object) this.company.CompanyId, (object) KvrplHelper.DateToBaseFormat(dBeg), (object) KvrplHelper.DateToBaseFormat(dEnd))).List()[0]));
          if (period != 0 && Convert.ToInt32(KvrplHelper.BaseValue(32, this.company)) == 1)
          {
            this.tariffCost.Dbeg = dBeg;
            this.tariffCost.Dend = dEnd;
            this.tariffCost.Tariff_id = this.tariff.Tariff_id;
            this.tariffCost.Service = (Kvartplata.Classes.Service) this.cmbService.SelectedItem;
            if (Convert.ToInt32(KvrplHelper.BaseValue(1, this.company)) == 28 && MessageBox.Show("Внести основание?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
              FrmArgument frmArgument = new FrmArgument();
              int num = (int) frmArgument.ShowDialog();
              frmArgument.Dispose();
              KvrplHelper.SaveTariffFromNoteBook(this.tariffCost, (cmpTariffCost) null, company, (short) 1, this.tariff.Tariff_num.ToString() + " (" + this.tariff.Tariff_name + ")", frmArgument.Argument(), 1 != 0, this.monthClosed.PeriodName.Value);
            }
            else
              KvrplHelper.SaveTariffFromNoteBook(this.tariffCost, (cmpTariffCost) null, company, (short) 1, this.tariff.Tariff_num.ToString() + " (" + this.tariff.Tariff_name + ")", "", 1 != 0, this.monthClosed.PeriodName.Value);
          }
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Не удалось ввести новый тариф", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        KvrplHelper.WriteLog(ex, (LsClient) null);
        return false;
      }
      return true;
    }

    private bool NewLsService(cmpTariffCost tcost, int period, DateTime dBeg, DateTime dEnd)
    {
      try
      {
        string str = "null";
        object obj = (object) null;
        if (this.cbNorm.Checked)
          str = "norm_id";
        else if (this.norm != null)
        {
          obj = (object) this.norm.Norm_id;
          str = "{0}";
        }
        this.session.CreateSQLQuery(string.Format("insert into LsService select client_id,:period,service_id,:ndbeg,:ndend,:ntariff," + str + ",complex_id,user,today() from lsService where client_id in (select client_id from lsClient ls where ls.company_id=:company and idhome in (" + this.homesList + ") " + Options.MainConditions2 + this.uslovie + ") and period_id=0 and service_id=:service and dbeg<=:dbeg and dend>=:dend and tariff_id=:tariff and complex_id=:complex", obj)).SetDateTime("ndbeg", dBeg).SetDateTime("ndend", dEnd).SetParameter<int>("tariff", tcost.Tariff_id).SetParameter<short>("service", ((Kvartplata.Classes.Service) this.cmbService.SelectedItem).ServiceId).SetDateTime("dbeg", dBeg).SetDateTime("dend", dBeg).SetParameter<int>("period", period).SetParameter<int>("ntariff", this.tariff.Tariff_id).SetParameter<int>("complex", Options.Complex.ComplexId).SetParameter<short>("company", this.company.CompanyId).ExecuteUpdate();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Не удалось ввести новый тариф на карточки", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        KvrplHelper.WriteLog(ex, (LsClient) null);
        return false;
      }
      return true;
    }

    private bool NewLsService1(int period, DateTime dBeg, DateTime dEnd)
    {
      try
      {
        string str = "null";
        object obj = (object) null;
        if (this.cbNorm.Checked)
          str = "(select first norm_id from lsService where client_id=ls.client_id and period_id=0 and service_id=:service and dbeg<=:dbeg and dend>=:dend)";
        else if (this.norm != null)
        {
          obj = (object) this.norm.Norm_id;
          str = "{0}";
        }
        this.session.CreateSQLQuery(string.Format("insert into LsService select client_id,:period,:service,:dbeg,:dend,:ntariff," + str + ",:complex,user,today() from lsClient ls where ls.company_id=:company and idhome in (" + this.homesList + ") " + Options.MainConditions2 + this.uslovie, obj)).SetParameter<int>("period", period).SetParameter<short>("service", ((Kvartplata.Classes.Service) this.cmbService.SelectedItem).ServiceId).SetDateTime("dbeg", dBeg).SetDateTime("dend", dEnd).SetParameter<int>("ntariff", this.tariff.Tariff_id).SetParameter<short>("service", ((Kvartplata.Classes.Service) this.cmbService.SelectedItem).ServiceId).SetParameter<int>("ntariff", this.tariff.Tariff_id).SetParameter<int>("complex", Options.Complex.ComplexId).SetParameter<short>("company", this.company.CompanyId).ExecuteUpdate();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Не удалось ввести новый тариф на карточки", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        KvrplHelper.WriteLog(ex, (LsClient) null);
        return false;
      }
      return true;
    }

    private bool AddTariffInsteadOld(cmpTariffCost tcost, int period, DateTime dBeg, DateTime dEnd)
    {
      try
      {
        double val1 = 0.0;
        double val2 = 0.0;
        double val3 = 0.0;
        this.session.Clear();
        this.session = Domain.CurrentSession;
        DateTime val4 = !(this.dtpDate.Value < this.monthClosed.PeriodName.Value.AddMonths(1)) ? this.dtpDate.Value : this.monthClosed.PeriodName.Value.AddMonths(1);
        foreach (cmpTariffCost cmpTariffCost in (IEnumerable<cmpTariffCost>) this.listCost)
        {
          IQuery query1 = this.session.CreateSQLQuery("insert into cmpTariff select tariff_id,service_id,company_id,:period,:ndbeg,:ndend,unitmeasuring_id,basetariff_id,scheme,:cost,:cost_eo,:cost_c,complex_id,user,today(),basetariffmsp_id,scheme_param,isvat from cmpTariff where company_id=(select param_value from cmpParam where company_id=:company and period_id=0 and DBeg<=:dbeg and DEnd>=:dend and Param_Id=201) and period_id=0 and tariff_id=:tariff and dbeg<=:dbeg and dend>=:dend and service_id=:service and complex_id=:complex").SetDateTime("ndbeg", dBeg).SetDateTime("ndend", dEnd).SetParameter<int>("tariff", tcost.Tariff_id).SetParameter<short>("service", cmpTariffCost.Service.ServiceId).SetParameter<int>("period", period).SetDateTime("dbeg", val4).SetDateTime("dend", val4).SetParameter<int>("complex", Options.Complex.ComplexId).SetParameter<short>("company", this.company.CompanyId);
          string name1 = "cost";
          double? nullable = cmpTariffCost.Cost;
          double val5 = nullable.Value;
          IQuery query2 = query1.SetDouble(name1, val5);
          string name2 = "cost_eo";
          nullable = cmpTariffCost.Cost_eo;
          double val6 = nullable.Value;
          IQuery query3 = query2.SetDouble(name2, val6);
          string name3 = "cost_c";
          nullable = cmpTariffCost.Cost_c;
          double val7 = nullable.Value;
          query3.SetDouble(name3, val7).ExecuteUpdate();
          val1 += Convert.ToDouble((object) cmpTariffCost.Cost);
          val3 += Convert.ToDouble((object) cmpTariffCost.Cost_eo);
          val2 += Convert.ToDouble((object) cmpTariffCost.Cost_c);
        }
        this.session.CreateSQLQuery("insert into cmpTariff select tariff_id,service_id,company_id,:period,:ndbeg,:ndend,unitmeasuring_id,basetariff_id,scheme,:cost,:cost_eo,:cost_c,complex_id,user,today(),basetariffmsp_id,scheme_param,isvat from cmpTariff where company_id=(select param_value from cmpParam where company_id=:company and period_id=0 and DBeg<=:dbeg and DEnd>=:dend and Param_Id=201) and period_id=0 and tariff_id=:tariff and dbeg<=:dbeg and dend>=:dend and service_id=:service and complex_id=:complex").SetDateTime("ndbeg", dBeg).SetDateTime("ndend", dEnd).SetParameter<int>("tariff", tcost.Tariff_id).SetParameter<short>("service", ((Kvartplata.Classes.Service) this.cmbService.SelectedItem).ServiceId).SetParameter<int>("period", period).SetDateTime("dbeg", val4).SetDateTime("dend", val4).SetParameter<int>("complex", Options.Complex.ComplexId).SetParameter<short>("company", this.company.CompanyId).SetParameter<double>("cost", val1).SetParameter<double>("cost_eo", val3).SetParameter<double>("cost_c", val2).ExecuteUpdate();
        try
        {
          Company company = this.session.Get<Company>((object) Convert.ToInt16(this.session.CreateQuery(string.Format("select ParamValue from CompanyParam where Company.CompanyId={0} and Period.PeriodId=0 and DBeg<='{1}' and DEnd>='{2}' and Param.ParamId=201", (object) this.company.CompanyId, (object) KvrplHelper.DateToBaseFormat(dBeg), (object) KvrplHelper.DateToBaseFormat(dEnd))).List()[0]));
          if (period != 0 && Convert.ToInt32(KvrplHelper.BaseValue(32, this.company)) == 1)
          {
            tcost.Dbeg = dBeg;
            tcost.Dend = dEnd;
            tcost.Cost = new double?(val1);
            tcost.Service = (Kvartplata.Classes.Service) this.cmbService.SelectedItem;
            Tariff tariff = this.session.Get<Tariff>((object) tcost.Tariff_id);
            if (Convert.ToInt32(KvrplHelper.BaseValue(1, this.company)) == 28 && MessageBox.Show("Внести основание?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
              FrmArgument frmArgument = new FrmArgument();
              int num = (int) frmArgument.ShowDialog();
              frmArgument.Dispose();
              KvrplHelper.SaveTariffFromNoteBook(tcost, (cmpTariffCost) null, company, (short) 1, tariff.Tariff_num.ToString() + " (" + tariff.Tariff_name + ")", frmArgument.Argument(), 1 != 0, this.monthClosed.PeriodName.Value);
            }
            else
              KvrplHelper.SaveTariffFromNoteBook(tcost, (cmpTariffCost) null, company, (short) 1, tariff.Tariff_num.ToString() + " (" + tariff.Tariff_name + ")", "", 1 != 0, this.monthClosed.PeriodName.Value);
          }
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Не удалось ввести новый тариф", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        KvrplHelper.WriteLog(ex, (LsClient) null);
        return false;
      }
      return true;
    }

    private void btnCheckHomes_Click(object sender, EventArgs e)
    {
      if (this.btnCheckHomes.Text == "Выделить все")
      {
        KvrplHelper.CheckAll(this.clbHomes);
        this.btnCheckHomes.Text = "Снять все";
      }
      else
      {
        KvrplHelper.UnCheckAll(this.clbHomes);
        this.btnCheckHomes.Text = "Выделить все";
      }
      this.clbHomes_MouseUp((object) null, (MouseEventArgs) null);
    }

    private void txbTariff_KeyUp(object sender, KeyEventArgs e)
    {
    }

    private void txbNumTariff_Leave(object sender, EventArgs e)
    {
      try
      {
        this.tariff.Tariff_num = Convert.ToInt32(this.txbNumTariff.Text);
      }
      catch
      {
        int num = (int) MessageBox.Show("Некорректное значение номера варианта");
        this.txbNumTariff.Text = "";
      }
    }

    private void txbNameTariff_Leave(object sender, EventArgs e)
    {
      try
      {
        this.tariff.Tariff_name = this.txbNameTariff.Text;
      }
      catch
      {
      }
    }

    private void btnNorm_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      FrmClientCardChoose clientCardChoose = new FrmClientCardChoose((Kvartplata.Classes.Service) this.cmbService.SelectedItem, new Norm(), (int) this.company.CompanyId, new DateTime?(this.dtpDate.Value));
      int num = (int) clientCardChoose.ShowDialog();
      this.Cursor = Cursors.Default;
      if (clientCardChoose.Id != -1)
      {
        this.norm = this.session.Get<Norm>((object) clientCardChoose.Id);
        try
        {
          CmpNorm cmpNorm = new CmpNorm();
          this.btnNorm.Text = "№ " + (object) this.norm.Norm_num + " " + this.norm.Norm_name + "     " + (object) this.session.CreateQuery(string.Format("from CmpNorm where Norm.Norm_id={0} and Period.PeriodId=0 and dbeg<='{1}' and dend>='{1}'", (object) this.norm.Norm_id, (object) KvrplHelper.DateToBaseFormat(this.dtpDate.Value), (object) ((Kvartplata.Classes.Service) this.cmbService.SelectedItem).ServiceId)).List<CmpNorm>()[0].Norm_value;
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      clientCardChoose.Dispose();
    }

    private void dgvTariff_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.RowIndex != -1 && e.ColumnIndex != -1 && e.Button == MouseButtons.Left)
        ((DataGridView) sender).Rows[e.RowIndex].Selected = true;
      this.dgvTariff_CellClick((object) null, (DataGridViewCellEventArgs) null);
    }

    private void cmbStreet_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.LoadHomes();
    }

    private void cmbStreet_SelectedValueChanged(object sender, EventArgs e)
    {
      if (!(this.cmbStreet.Text != ""))
        return;
      this.cmbStreet_SelectionChangeCommitted((object) null, (EventArgs) null);
    }

    private void cbNorm_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.cbNorm.Checked)
      {
        this.btnNorm.Enabled = true;
        this.lblNorm.Enabled = true;
      }
      else
      {
        this.btnNorm.Enabled = false;
        this.lblNorm.Enabled = false;
      }
    }

    private void cmbVat_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.tariffCost.IsVat = (YesNo) this.cmbVat.SelectedItem;
    }

    private void clbHomes_Click(object sender, EventArgs e)
    {
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
      this.btnTariff = new Button();
      this.btnExit = new Button();
      this.lblService = new Label();
      this.cmbService = new ComboBox();
      this.lblDate = new Label();
      this.dtpDate = new DateTimePicker();
      this.lblHomes = new Label();
      this.clbHomes = new CheckedListBox();
      this.dgvTariff = new DataGridView();
      this.lblTariffs = new Label();
      this.dgvOldTariffs = new DataGridView();
      this.gbTariff = new GroupBox();
      this.rbNewTariff = new RadioButton();
      this.rbOld = new RadioButton();
      this.rbNew = new RadioButton();
      this.dgvSost = new DataGridView();
      this.Service = new DataGridViewTextBoxColumn();
      this.Scheme = new DataGridViewButtonColumn();
      this.Cost = new DataGridViewTextBoxColumn();
      this.Cost_eo = new DataGridViewTextBoxColumn();
      this.Cost_c = new DataGridViewTextBoxColumn();
      this.pn = new Panel();
      this.cmbVat = new ComboBox();
      this.lblVat = new Label();
      this.lblCalc = new Label();
      this.lblOldTariff = new Label();
      this.cmbBaseTariffMSP = new ComboBox();
      this.lblBaseTariffMSP = new Label();
      this.cmbBaseTariff = new ComboBox();
      this.lblBaseTariff = new Label();
      this.cmbIzm = new ComboBox();
      this.lblIzm = new Label();
      this.lblSchemeParam = new Label();
      this.lblScheme = new Label();
      this.btnSchemeParam = new Button();
      this.btnScheme = new Button();
      this.txbNumTariff = new TextBox();
      this.lblNumTariff = new Label();
      this.txbTariff = new TextBox();
      this.lblTariff = new Label();
      this.txbNameTariff = new TextBox();
      this.lblNameTariff = new Label();
      this.lblNorm = new Label();
      this.btnNorm = new Button();
      this.btnCheckHomes = new Button();
      this.cbUchet = new CheckBox();
      this.cbArhive = new CheckBox();
      this.gbUsl = new GroupBox();
      this.gpbFlat = new GroupBox();
      this.rbAll = new RadioButton();
      this.rbtnNotPrivat = new RadioButton();
      this.rbtnPrivat = new RadioButton();
      this.gbTarif = new GroupBox();
      this.lblStreet = new Label();
      this.cmbStreet = new ComboBox();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
      this.cbNorm = new CheckBox();
      this.hp = new HelpProvider();
      this.pnBtn.SuspendLayout();
      ((ISupportInitialize) this.dgvTariff).BeginInit();
      ((ISupportInitialize) this.dgvOldTariffs).BeginInit();
      this.gbTariff.SuspendLayout();
      ((ISupportInitialize) this.dgvSost).BeginInit();
      this.pn.SuspendLayout();
      this.gbUsl.SuspendLayout();
      this.gpbFlat.SuspendLayout();
      this.gbTarif.SuspendLayout();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnTariff);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 663);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(1220, 40);
      this.pnBtn.TabIndex = 0;
      this.btnTariff.Image = (Image) Resources.Configure;
      this.btnTariff.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnTariff.Location = new Point(19, 5);
      this.btnTariff.Name = "btnTariff";
      this.btnTariff.Size = new Size(148, 30);
      this.btnTariff.TabIndex = 1;
      this.btnTariff.Text = "Заменить тариф";
      this.btnTariff.TextAlign = ContentAlignment.MiddleRight;
      this.btnTariff.UseVisualStyleBackColor = true;
      this.btnTariff.Click += new EventHandler(this.btnTariff_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(1123, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(85, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.lblService.AutoSize = true;
      this.lblService.Location = new Point(9, 9);
      this.lblService.Name = "lblService";
      this.lblService.Size = new Size(121, 16);
      this.lblService.TabIndex = 1;
      this.lblService.Text = "Выберите услугу";
      this.cmbService.FormattingEnabled = true;
      this.cmbService.Location = new Point(139, 6);
      this.cmbService.Name = "cmbService";
      this.cmbService.Size = new Size(250, 24);
      this.cmbService.TabIndex = 0;
      this.cmbService.SelectionChangeCommitted += new EventHandler(this.cmbService_SelectionChangeCommitted);
      this.lblDate.AutoSize = true;
      this.lblDate.Location = new Point(9, 40);
      this.lblDate.Name = "lblDate";
      this.lblDate.Size = new Size(204, 16);
      this.lblDate.TabIndex = 3;
      this.lblDate.Text = "Выберите дату смены тарифа";
      this.dtpDate.Location = new Point(222, 37);
      this.dtpDate.Name = "dtpDate";
      this.dtpDate.Size = new Size(167, 22);
      this.dtpDate.TabIndex = 1;
      this.lblHomes.AutoSize = true;
      this.lblHomes.Location = new Point(12, 95);
      this.lblHomes.Name = "lblHomes";
      this.lblHomes.Size = new Size(99, 16);
      this.lblHomes.TabIndex = 5;
      this.lblHomes.Text = "Список домов";
      this.clbHomes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.clbHomes.CheckOnClick = true;
      this.clbHomes.FormattingEnabled = true;
      this.clbHomes.Location = new Point(12, 114);
      this.clbHomes.Name = "clbHomes";
      this.clbHomes.Size = new Size(377, 276);
      this.clbHomes.TabIndex = 3;
      this.clbHomes.ThreeDCheckBoxes = true;
      this.clbHomes.Click += new EventHandler(this.clbHomes_Click);
      this.clbHomes.MouseUp += new MouseEventHandler(this.clbHomes_MouseUp);
      this.dgvTariff.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.dgvTariff.BackgroundColor = Color.AliceBlue;
      this.dgvTariff.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvTariff.Location = new Point(12, 492);
      this.dgvTariff.Name = "dgvTariff";
      this.dgvTariff.Size = new Size(377, 165);
      this.dgvTariff.TabIndex = 4;
      this.dgvTariff.CellClick += new DataGridViewCellEventHandler(this.dgvTariff_CellClick);
      this.dgvTariff.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvTariff_CellFormatting);
      this.dgvTariff.CellMouseClick += new DataGridViewCellMouseEventHandler(this.dgvTariff_CellMouseClick);
      this.lblTariffs.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblTariffs.AutoSize = true;
      this.lblTariffs.Location = new Point(12, 457);
      this.lblTariffs.Name = "lblTariffs";
      this.lblTariffs.Size = new Size(334, 32);
      this.lblTariffs.TabIndex = 9;
      this.lblTariffs.Text = "Список тарифов в выбранных домах \r\n(серые - те которые не только в выбранных домах)";
      this.dgvOldTariffs.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dgvOldTariffs.BackgroundColor = Color.AliceBlue;
      this.dgvOldTariffs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvOldTariffs.Location = new Point(9, 51);
      this.dgvOldTariffs.MultiSelect = false;
      this.dgvOldTariffs.Name = "dgvOldTariffs";
      this.dgvOldTariffs.Size = new Size(597, 124);
      this.dgvOldTariffs.TabIndex = 11;
      this.dgvOldTariffs.CellClick += new DataGridViewCellEventHandler(this.dgvOldTariffs_CellClick);
      this.gbTariff.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gbTariff.Controls.Add((Control) this.rbNewTariff);
      this.gbTariff.Controls.Add((Control) this.rbOld);
      this.gbTariff.Controls.Add((Control) this.rbNew);
      this.gbTariff.Location = new Point(395, 6);
      this.gbTariff.Name = "gbTariff";
      this.gbTariff.Size = new Size(615, 95);
      this.gbTariff.TabIndex = 5;
      this.gbTariff.TabStop = false;
      this.gbTariff.Text = "Внесите тариф";
      this.rbNewTariff.AutoSize = true;
      this.rbNewTariff.Location = new Point(17, 43);
      this.rbNewTariff.Name = "rbNewTariff";
      this.rbNewTariff.Size = new Size(321, 17);
      this.rbNewTariff.TabIndex = 15;
      this.rbNewTariff.TabStop = true;
      this.rbNewTariff.Text = "Внести новое значение тарифа для выбранных вариантов";
      this.rbNewTariff.UseVisualStyleBackColor = true;
      this.rbNewTariff.CheckedChanged += new EventHandler(this.rbNewTariff_CheckedChanged);
      this.rbOld.AutoSize = true;
      this.rbOld.Location = new Point(17, 21);
      this.rbOld.Name = "rbOld";
      this.rbOld.Size = new Size(123, 17);
      this.rbOld.TabIndex = 14;
      this.rbOld.TabStop = true;
      this.rbOld.Text = "Выбрать из списка";
      this.rbOld.UseVisualStyleBackColor = true;
      this.rbOld.CheckedChanged += new EventHandler(this.rbOld_CheckedChanged);
      this.rbNew.AutoSize = true;
      this.rbNew.Location = new Point(17, 65);
      this.rbNew.Name = "rbNew";
      this.rbNew.Size = new Size(130, 17);
      this.rbNew.TabIndex = 14;
      this.rbNew.TabStop = true;
      this.rbNew.Text = "Ввести новый тариф";
      this.rbNew.UseVisualStyleBackColor = true;
      this.rbNew.CheckedChanged += new EventHandler(this.rbNew_CheckedChanged);
      this.dgvSost.AllowUserToAddRows = false;
      this.dgvSost.AllowUserToDeleteRows = false;
      this.dgvSost.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dgvSost.BackgroundColor = Color.AliceBlue;
      this.dgvSost.CausesValidation = false;
      this.dgvSost.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvSost.Columns.AddRange((DataGridViewColumn) this.Service, (DataGridViewColumn) this.Scheme, (DataGridViewColumn) this.Cost, (DataGridViewColumn) this.Cost_eo, (DataGridViewColumn) this.Cost_c);
      this.dgvSost.Location = new Point(9, 231);
      this.dgvSost.Name = "dgvSost";
      this.dgvSost.Size = new Size(597, 145);
      this.dgvSost.TabIndex = 3;
      this.dgvSost.VirtualMode = true;
      this.dgvSost.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);
      this.dgvSost.CellValueNeeded += new DataGridViewCellValueEventHandler(this.dataGridView1_CellValueNeeded);
      this.dgvSost.CellValuePushed += new DataGridViewCellValueEventHandler(this.dataGridView1_CellValuePushed);
      this.Service.HeaderText = "Услуга";
      this.Service.Name = "Service";
      this.Service.ReadOnly = true;
      this.Service.Width = 200;
      this.Scheme.HeaderText = "Схема";
      this.Scheme.Name = "Scheme";
      this.Scheme.Width = 80;
      this.Cost.HeaderText = "Тариф";
      this.Cost.Name = "Cost";
      this.Cost.Width = 80;
      this.Cost_eo.HeaderText = "ЭО Тариф";
      this.Cost_eo.Name = "Cost_eo";
      this.Cost_eo.Width = 80;
      this.Cost_c.HeaderText = "Тариф для компенсации";
      this.Cost_c.Name = "Cost_c";
      this.Cost_c.Width = 95;
      this.pn.Controls.Add((Control) this.cmbVat);
      this.pn.Controls.Add((Control) this.lblVat);
      this.pn.Controls.Add((Control) this.lblCalc);
      this.pn.Controls.Add((Control) this.dgvOldTariffs);
      this.pn.Controls.Add((Control) this.lblOldTariff);
      this.pn.Controls.Add((Control) this.cmbBaseTariffMSP);
      this.pn.Controls.Add((Control) this.lblBaseTariffMSP);
      this.pn.Controls.Add((Control) this.cmbBaseTariff);
      this.pn.Controls.Add((Control) this.lblBaseTariff);
      this.pn.Controls.Add((Control) this.cmbIzm);
      this.pn.Controls.Add((Control) this.lblIzm);
      this.pn.Controls.Add((Control) this.lblSchemeParam);
      this.pn.Controls.Add((Control) this.lblScheme);
      this.pn.Controls.Add((Control) this.btnSchemeParam);
      this.pn.Controls.Add((Control) this.btnScheme);
      this.pn.Controls.Add((Control) this.txbNumTariff);
      this.pn.Controls.Add((Control) this.lblNumTariff);
      this.pn.Controls.Add((Control) this.txbTariff);
      this.pn.Controls.Add((Control) this.lblTariff);
      this.pn.Controls.Add((Control) this.txbNameTariff);
      this.pn.Controls.Add((Control) this.dgvSost);
      this.pn.Controls.Add((Control) this.lblNameTariff);
      this.pn.Dock = DockStyle.Fill;
      this.pn.Enabled = false;
      this.pn.Location = new Point(3, 18);
      this.pn.Name = "pn";
      this.pn.Size = new Size(609, 473);
      this.pn.TabIndex = 6;
      this.cmbVat.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cmbVat.FormattingEnabled = true;
      this.cmbVat.Location = new Point(122, 445);
      this.cmbVat.Name = "cmbVat";
      this.cmbVat.Size = new Size(93, 24);
      this.cmbVat.TabIndex = 34;
      this.cmbVat.SelectionChangeCommitted += new EventHandler(this.cmbVat_SelectionChangeCommitted);
      this.lblVat.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblVat.AutoSize = true;
      this.lblVat.Location = new Point(6, 448);
      this.lblVat.Name = "lblVat";
      this.lblVat.Size = new Size(110, 16);
      this.lblVat.TabIndex = 33;
      this.lblVat.Text = "Учитывать НДС";
      this.lblCalc.AutoSize = true;
      this.lblCalc.Location = new Point(6, 212);
      this.lblCalc.Name = "lblCalc";
      this.lblCalc.Size = new Size(92, 16);
      this.lblCalc.TabIndex = 32;
      this.lblCalc.Text = "Калькуляция";
      this.lblOldTariff.AutoSize = true;
      this.lblOldTariff.Location = new Point(6, 32);
      this.lblOldTariff.Name = "lblOldTariff";
      this.lblOldTariff.Size = new Size(258, 16);
      this.lblOldTariff.TabIndex = 30;
      this.lblOldTariff.Text = "Аналогичные тарифы из справочника";
      this.cmbBaseTariffMSP.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cmbBaseTariffMSP.FormattingEnabled = true;
      this.cmbBaseTariffMSP.Location = new Point(499, 416);
      this.cmbBaseTariffMSP.Name = "cmbBaseTariffMSP";
      this.cmbBaseTariffMSP.Size = new Size(105, 24);
      this.cmbBaseTariffMSP.TabIndex = 6;
      this.cmbBaseTariffMSP.SelectionChangeCommitted += new EventHandler(this.cmbBaseTariffMSP_SelectionChangeCommitted);
      this.lblBaseTariffMSP.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblBaseTariffMSP.AutoSize = true;
      this.lblBaseTariffMSP.Location = new Point(373, 419);
      this.lblBaseTariffMSP.Name = "lblBaseTariffMSP";
      this.lblBaseTariffMSP.Size = new Size(120, 16);
      this.lblBaseTariffMSP.TabIndex = 28;
      this.lblBaseTariffMSP.Text = "Обл. применения";
      this.cmbBaseTariff.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cmbBaseTariff.FormattingEnabled = true;
      this.cmbBaseTariff.Location = new Point(266, 416);
      this.cmbBaseTariff.Name = "cmbBaseTariff";
      this.cmbBaseTariff.Size = new Size(100, 24);
      this.cmbBaseTariff.TabIndex = 5;
      this.cmbBaseTariff.SelectionChangeCommitted += new EventHandler(this.cmbBaseTariff_SelectionChangeCommitted);
      this.lblBaseTariff.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblBaseTariff.AutoSize = true;
      this.lblBaseTariff.Location = new Point(206, 419);
      this.lblBaseTariff.Name = "lblBaseTariff";
      this.lblBaseTariff.Size = new Size(58, 16);
      this.lblBaseTariff.TabIndex = 26;
      this.lblBaseTariff.Text = "На кого";
      this.cmbIzm.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cmbIzm.FormattingEnabled = true;
      this.cmbIzm.Location = new Point(122, 416);
      this.cmbIzm.Name = "cmbIzm";
      this.cmbIzm.Size = new Size(73, 24);
      this.cmbIzm.TabIndex = 4;
      this.cmbIzm.SelectionChangeCommitted += new EventHandler(this.cmbIzm_SelectionChangeCommitted);
      this.lblIzm.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblIzm.AutoSize = true;
      this.lblIzm.Location = new Point(6, 419);
      this.lblIzm.Name = "lblIzm";
      this.lblIzm.Size = new Size(110, 16);
      this.lblIzm.TabIndex = 24;
      this.lblIzm.Text = "Един-а измер-я";
      this.lblSchemeParam.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblSchemeParam.AutoSize = true;
      this.lblSchemeParam.Location = new Point(224, 390);
      this.lblSchemeParam.Name = "lblSchemeParam";
      this.lblSchemeParam.Size = new Size(188, 16);
      this.lblSchemeParam.TabIndex = 23;
      this.lblSchemeParam.Text = "Схема параметров расчета";
      this.lblScheme.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblScheme.AutoSize = true;
      this.lblScheme.Location = new Point(6, 390);
      this.lblScheme.Name = "lblScheme";
      this.lblScheme.Size = new Size(128, 16);
      this.lblScheme.TabIndex = 22;
      this.lblScheme.Text = "Алгоритм расчета";
      this.btnSchemeParam.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnSchemeParam.Location = new Point(418, 387);
      this.btnSchemeParam.Name = "btnSchemeParam";
      this.btnSchemeParam.Size = new Size(75, 23);
      this.btnSchemeParam.TabIndex = 8;
      this.btnSchemeParam.UseVisualStyleBackColor = true;
      this.btnSchemeParam.Click += new EventHandler(this.btnSchemeParam_Click);
      this.btnScheme.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnScheme.Location = new Point(140, 387);
      this.btnScheme.Name = "btnScheme";
      this.btnScheme.Size = new Size(75, 23);
      this.btnScheme.TabIndex = 7;
      this.btnScheme.UseVisualStyleBackColor = true;
      this.btnScheme.Click += new EventHandler(this.btnScheme_Click);
      this.txbNumTariff.Location = new Point(126, 181);
      this.txbNumTariff.Name = "txbNumTariff";
      this.txbNumTariff.Size = new Size(54, 22);
      this.txbNumTariff.TabIndex = 1;
      this.txbNumTariff.Leave += new EventHandler(this.txbNumTariff_Leave);
      this.lblNumTariff.AutoSize = true;
      this.lblNumTariff.Location = new Point(6, 184);
      this.lblNumTariff.Name = "lblNumTariff";
      this.lblNumTariff.Size = new Size(117, 16);
      this.lblNumTariff.TabIndex = 18;
      this.lblNumTariff.Text = "Номер варианта";
      this.txbTariff.Location = new Point(277, 7);
      this.txbTariff.Name = "txbTariff";
      this.txbTariff.Size = new Size(100, 22);
      this.txbTariff.TabIndex = 0;
      this.txbTariff.KeyDown += new KeyEventHandler(this.txbTariff_KeyDown);
      this.txbTariff.KeyPress += new KeyPressEventHandler(this.txbTariff_KeyPress);
      this.txbTariff.KeyUp += new KeyEventHandler(this.txbTariff_KeyUp);
      this.txbTariff.Leave += new EventHandler(this.txbTariff_Leave);
      this.lblTariff.AutoSize = true;
      this.lblTariff.Location = new Point(6, 10);
      this.lblTariff.Name = "lblTariff";
      this.lblTariff.Size = new Size(265, 16);
      this.lblTariff.TabIndex = 16;
      this.lblTariff.Text = "Внесите сумму тарифа и нажмите Enter";
      this.txbNameTariff.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txbNameTariff.Location = new Point(302, 180);
      this.txbNameTariff.Name = "txbNameTariff";
      this.txbNameTariff.Size = new Size(304, 22);
      this.txbNameTariff.TabIndex = 2;
      this.txbNameTariff.Leave += new EventHandler(this.txbNameTariff_Leave);
      this.lblNameTariff.AutoSize = true;
      this.lblNameTariff.Location = new Point(189, 183);
      this.lblNameTariff.Name = "lblNameTariff";
      this.lblNameTariff.Size = new Size(107, 16);
      this.lblNameTariff.TabIndex = 0;
      this.lblNameTariff.Text = "Наименование";
      this.lblNorm.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblNorm.AutoSize = true;
      this.lblNorm.Enabled = false;
      this.lblNorm.Location = new Point(403, 633);
      this.lblNorm.Name = "lblNorm";
      this.lblNorm.Size = new Size(74, 16);
      this.lblNorm.TabIndex = 34;
      this.lblNorm.Text = "Норматив";
      this.btnNorm.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.btnNorm.Enabled = false;
      this.btnNorm.Location = new Point(483, 630);
      this.btnNorm.Name = "btnNorm";
      this.btnNorm.Size = new Size(529, 23);
      this.btnNorm.TabIndex = 7;
      this.btnNorm.UseVisualStyleBackColor = true;
      this.btnNorm.Click += new EventHandler(this.btnNorm_Click);
      this.btnCheckHomes.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnCheckHomes.Image = (Image) Resources.properties;
      this.btnCheckHomes.Location = new Point(12, 417);
      this.btnCheckHomes.Name = "btnCheckHomes";
      this.btnCheckHomes.Size = new Size(377, 28);
      this.btnCheckHomes.TabIndex = 17;
      this.btnCheckHomes.Text = "Выделить все";
      this.btnCheckHomes.TextAlign = ContentAlignment.MiddleRight;
      this.btnCheckHomes.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.btnCheckHomes.UseVisualStyleBackColor = true;
      this.btnCheckHomes.Click += new EventHandler(this.btnCheckHomes_Click);
      this.cbUchet.AutoSize = true;
      this.cbUchet.Checked = true;
      this.cbUchet.CheckState = CheckState.Checked;
      this.cbUchet.Location = new Point(6, 51);
      this.cbUchet.Name = "cbUchet";
      this.cbUchet.Size = new Size(88, 17);
      this.cbUchet.TabIndex = 16;
      this.cbUchet.Text = "Без учетных";
      this.cbUchet.UseVisualStyleBackColor = true;
      this.cbArhive.AutoSize = true;
      this.cbArhive.Checked = true;
      this.cbArhive.CheckState = CheckState.Checked;
      this.cbArhive.Location = new Point(6, 27);
      this.cbArhive.Name = "cbArhive";
      this.cbArhive.Size = new Size(158, 17);
      this.cbArhive.TabIndex = 15;
      this.cbArhive.Text = "Без закрытых и архивных";
      this.cbArhive.UseVisualStyleBackColor = true;
      this.gbUsl.Controls.Add((Control) this.gpbFlat);
      this.gbUsl.Controls.Add((Control) this.cbUchet);
      this.gbUsl.Controls.Add((Control) this.cbArhive);
      this.gbUsl.Dock = DockStyle.Right;
      this.gbUsl.Location = new Point(1016, 0);
      this.gbUsl.Name = "gbUsl";
      this.gbUsl.Size = new Size(204, 663);
      this.gbUsl.TabIndex = 35;
      this.gbUsl.TabStop = false;
      this.gbUsl.Text = "Условия";
      this.gpbFlat.Controls.Add((Control) this.rbAll);
      this.gpbFlat.Controls.Add((Control) this.rbtnNotPrivat);
      this.gpbFlat.Controls.Add((Control) this.rbtnPrivat);
      this.gpbFlat.Location = new Point(6, 80);
      this.gpbFlat.Name = "gpbFlat";
      this.gpbFlat.Size = new Size(192, 108);
      this.gpbFlat.TabIndex = 17;
      this.gpbFlat.TabStop = false;
      this.gpbFlat.Text = "Квартиры";
      this.rbAll.AutoSize = true;
      this.rbAll.Checked = true;
      this.rbAll.Location = new Point(15, 21);
      this.rbAll.Name = "rbAll";
      this.rbAll.Size = new Size(44, 17);
      this.rbAll.TabIndex = 2;
      this.rbAll.TabStop = true;
      this.rbAll.Text = "Все";
      this.rbAll.UseVisualStyleBackColor = true;
      this.rbtnNotPrivat.AutoSize = true;
      this.rbtnNotPrivat.Location = new Point(15, 76);
      this.rbtnNotPrivat.Name = "rbtnNotPrivat";
      this.rbtnNotPrivat.Size = new Size((int) sbyte.MaxValue, 17);
      this.rbtnNotPrivat.TabIndex = 1;
      this.rbtnNotPrivat.Text = "Не в собственности";
      this.rbtnNotPrivat.UseVisualStyleBackColor = true;
      this.rbtnPrivat.AutoSize = true;
      this.rbtnPrivat.Location = new Point(15, 48);
      this.rbtnPrivat.Name = "rbtnPrivat";
      this.rbtnPrivat.Size = new Size(111, 17);
      this.rbtnPrivat.TabIndex = 0;
      this.rbtnPrivat.Text = "В собственности";
      this.rbtnPrivat.UseVisualStyleBackColor = true;
      this.gbTarif.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gbTarif.Controls.Add((Control) this.pn);
      this.gbTarif.Location = new Point(395, 107);
      this.gbTarif.Name = "gbTarif";
      this.gbTarif.Size = new Size(615, 494);
      this.gbTarif.TabIndex = 33;
      this.gbTarif.TabStop = false;
      this.gbTarif.Text = "Тариф";
      this.lblStreet.AutoSize = true;
      this.lblStreet.Location = new Point(9, 70);
      this.lblStreet.Name = "lblStreet";
      this.lblStreet.Size = new Size(49, 16);
      this.lblStreet.TabIndex = 37;
      this.lblStreet.Text = "Улица";
      this.cmbStreet.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
      this.cmbStreet.AutoCompleteSource = AutoCompleteSource.ListItems;
      this.cmbStreet.Location = new Point(67, 67);
      this.cmbStreet.Name = "cmbStreet";
      this.cmbStreet.Size = new Size(322, 24);
      this.cmbStreet.TabIndex = 36;
      this.cmbStreet.SelectionChangeCommitted += new EventHandler(this.cmbStreet_SelectionChangeCommitted);
      this.cmbStreet.SelectedValueChanged += new EventHandler(this.cmbStreet_SelectedValueChanged);
      this.dataGridViewTextBoxColumn1.HeaderText = "Услуга";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.ReadOnly = true;
      this.dataGridViewTextBoxColumn1.Width = 200;
      this.dataGridViewTextBoxColumn2.HeaderText = "Тариф";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn2.Width = 80;
      this.dataGridViewTextBoxColumn3.HeaderText = "ЭО Тариф";
      this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
      this.dataGridViewTextBoxColumn3.Width = 80;
      this.dataGridViewTextBoxColumn4.HeaderText = "Тариф для компенсации";
      this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
      this.dataGridViewTextBoxColumn4.Width = 95;
      this.cbNorm.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cbNorm.AutoSize = true;
      this.cbNorm.Checked = true;
      this.cbNorm.CheckState = CheckState.Checked;
      this.cbNorm.Enabled = false;
      this.cbNorm.Location = new Point(404, 607);
      this.cbNorm.Name = "cbNorm";
      this.cbNorm.Size = new Size((int) byte.MaxValue, 20);
      this.cbNorm.TabIndex = 38;
      this.cbNorm.Text = "Сохранить действующий норматив";
      this.cbNorm.UseVisualStyleBackColor = true;
      this.cbNorm.CheckedChanged += new EventHandler(this.cbNorm_CheckedChanged);
      this.hp.HelpNamespace = "Help.chm";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1220, 703);
      this.Controls.Add((Control) this.cbNorm);
      this.Controls.Add((Control) this.lblStreet);
      this.Controls.Add((Control) this.cmbStreet);
      this.Controls.Add((Control) this.gbTarif);
      this.Controls.Add((Control) this.gbUsl);
      this.Controls.Add((Control) this.lblNorm);
      this.Controls.Add((Control) this.btnNorm);
      this.Controls.Add((Control) this.btnCheckHomes);
      this.Controls.Add((Control) this.gbTariff);
      this.Controls.Add((Control) this.lblTariffs);
      this.Controls.Add((Control) this.dgvTariff);
      this.Controls.Add((Control) this.clbHomes);
      this.Controls.Add((Control) this.lblHomes);
      this.Controls.Add((Control) this.dtpDate);
      this.Controls.Add((Control) this.lblDate);
      this.Controls.Add((Control) this.cmbService);
      this.Controls.Add((Control) this.lblService);
      this.Controls.Add((Control) this.pnBtn);
      this.hp.SetHelpKeyword((Control) this, "kv134.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      this.Name = "FrmNewHomesTariff";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Ввод новых тарифов";
      this.Load += new EventHandler(this.FrmNewHomesTariff_Load);
      this.pnBtn.ResumeLayout(false);
      ((ISupportInitialize) this.dgvTariff).EndInit();
      ((ISupportInitialize) this.dgvOldTariffs).EndInit();
      this.gbTariff.ResumeLayout(false);
      this.gbTariff.PerformLayout();
      ((ISupportInitialize) this.dgvSost).EndInit();
      this.pn.ResumeLayout(false);
      this.pn.PerformLayout();
      this.gbUsl.ResumeLayout(false);
      this.gbUsl.PerformLayout();
      this.gpbFlat.ResumeLayout(false);
      this.gpbFlat.PerformLayout();
      this.gbTarif.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
