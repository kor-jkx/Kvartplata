// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmAgreement
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
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmAgreement : FrmBaseForm
  {
    private FormStateSaver fss = new FormStateSaver(FrmAgreement.ic);
    private IContainer components = (IContainer) null;
    private LsClient client;
    private static IContainer ic;
    private ISession session;
    private Period monthClosed;
    private Panel pnBtn;
    private Button btnExit;
    private DataGridView dgvAgreement;
    private Panel pn;
    private DataGridView dgvInstalment;
    private Button btnAdd;
    private Button btnDelete;
    private Button btnSave;
    private Button btnCalc;
    private Label lblInstalment;
    private Button btnSaveInstallment;

    public FrmAgreement()
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
    }

    public FrmAgreement(LsClient client)
    {
      this.InitializeComponent();
      this.client = client;
      this.fss.ParentForm = (Form) this;
      this.session = Domain.CurrentSession;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void FrmAgreement_Load(object sender, EventArgs e)
    {
      this.monthClosed = KvrplHelper.GetKvrClose(this.client.ClientId, Options.ComplexPasp, Options.ComplexPrior);
      this.LoadAgreement();
      this.LoadInstalment();
    }

    private void LoadAgreement()
    {
      this.session.Clear();
      this.dgvAgreement.DataSource = (object) null;
      this.dgvAgreement.Columns.Clear();
      this.dgvAgreement.DataSource = (object) this.session.CreateCriteria(typeof (Agreement)).Add((ICriterion) Restrictions.Eq("LsClient.ClientId", (object) this.client.ClientId)).AddOrder(Order.Desc("DBeg")).List<Agreement>();
      this.SetViewAgreement();
    }

    private void SetViewAgreement()
    {
      KvrplHelper.AddTextBoxColumn(this.dgvAgreement, 1, "Количество месяцев рассрочки", "MonthCount", 80, false);
      KvrplHelper.AddTextBoxColumn(this.dgvAgreement, 2, "Долг", "Dept", 80, false);
      KvrplHelper.AddTextBoxColumn(this.dgvAgreement, 3, "Пени", "DeptPeni", 80, false);
      KvrplHelper.AddMaskDateColumn(this.dgvAgreement, 4, "Дата заключения соглашения", "DBeg");
      KvrplHelper.AddMaskDateColumn(this.dgvAgreement, 5, "Дата расторжения", "DEnd");
      this.dgvAgreement.Columns["AgreementNum"].HeaderText = "Номер соглашения";
      this.dgvAgreement.Columns["Note"].HeaderText = "Примечание";
      this.dgvAgreement.Columns["Note"].DisplayIndex = 6;
      foreach (DataGridViewRow row in (IEnumerable) this.dgvAgreement.Rows)
      {
        row.Cells["MonthCount"].Value = (object) ((Agreement) row.DataBoundItem).MonthCount;
        row.Cells["Dept"].Value = (object) ((Agreement) row.DataBoundItem).Dept;
        row.Cells["DeptPeni"].Value = (object) ((Agreement) row.DataBoundItem).DeptPeni;
        row.Cells["DBeg"].Value = (object) ((Agreement) row.DataBoundItem).DBeg;
        row.Cells["DEnd"].Value = (object) ((Agreement) row.DataBoundItem).DEnd;
      }
      KvrplHelper.ViewEdit(this.dgvAgreement);
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      this.session.Clear();
      if (this.session.CreateQuery(string.Format("select a from Agreement a where a.LsClient.ClientId={0} and a.DEnd is null", (object) this.client.ClientId)).List<Agreement>().Count > 0)
      {
        int num = (int) MessageBox.Show("Существует действующее соглашение", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.btnAdd.Enabled = false;
        this.btnDelete.Enabled = false;
        this.btnSave.Enabled = true;
        Agreement agreement = new Agreement();
        agreement.LsClient = this.client;
        agreement.DBeg = DateTime.Now;
        try
        {
          agreement.Dept = Convert.ToDecimal(this.session.CreateQuery(string.Format("select sum(BalanceIn) - sum(Payment) from Balance b where b.Period.PeriodId={0} and b.LsClient.ClientId={1}", (object) (this.monthClosed.PeriodId + 1), (object) this.client.ClientId)).List()[0]);
        }
        catch
        {
          agreement.Dept = Decimal.Zero;
        }
        try
        {
          agreement.DeptPeni = Convert.ToDecimal(this.session.CreateQuery(string.Format("select sum(BalanceIn) - sum(Payment) from BalancePeni where Period.PeriodId={0} and LsClient.ClientId={1}", (object) (this.monthClosed.PeriodId + 1), (object) this.client.ClientId)).List()[0]);
        }
        catch
        {
          agreement.DeptPeni = Decimal.Zero;
        }
        IList<Agreement> agreementList = (IList<Agreement>) new List<Agreement>();
        if ((uint) this.dgvAgreement.Rows.Count > 0U)
          agreementList = (IList<Agreement>) (this.dgvAgreement.DataSource as List<Agreement>);
        agreementList.Add(agreement);
        this.dgvAgreement.DataSource = (object) null;
        this.dgvAgreement.Columns.Clear();
        this.dgvAgreement.DataSource = (object) agreementList;
        this.SetViewAgreement();
        this.dgvAgreement.CurrentCell = this.dgvAgreement.Rows[this.dgvAgreement.Rows.Count - 1].Cells["AgreementNum"];
      }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.dgvAgreement.Rows.Count <= 0 || this.dgvAgreement.CurrentRow == null)
        return;
      Agreement agreement1 = new Agreement();
      int agreementId1 = ((Agreement) this.dgvAgreement.CurrentRow.DataBoundItem).AgreementId;
      if ((uint) ((Agreement) this.dgvAgreement.CurrentRow.DataBoundItem).AgreementId > 0U)
        agreement1 = this.session.CreateCriteria(typeof (Agreement)).Add((ICriterion) Restrictions.Eq("AgreementId", (object) ((Agreement) this.dgvAgreement.CurrentRow.DataBoundItem).AgreementId)).List<Agreement>()[0];
      bool flag1 = false;
      this.session.Clear();
      Agreement agreement2 = new Agreement();
      Agreement dataBoundItem = (Agreement) this.dgvAgreement.CurrentRow.DataBoundItem;
      int agreementId2 = dataBoundItem.AgreementId;
      bool flag2 = (uint) dataBoundItem.AgreementId <= 0U;
      if (this.dgvAgreement.CurrentRow.Cells["DBeg"].Value != null)
      {
        try
        {
          dataBoundItem.DBeg = Convert.ToDateTime(this.dgvAgreement.CurrentRow.Cells["DBeg"].Value);
        }
        catch
        {
          int num = (int) MessageBox.Show("Введенная дата начала некорректна", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        DateTime? nullable;
        if (this.dgvAgreement.CurrentRow.Cells["DEnd"].Value != null)
        {
          try
          {
            if (this.dgvAgreement.CurrentRow.Cells["DEnd"].Value.ToString() != "  .  .")
            {
              dataBoundItem.DEnd = new DateTime?(Convert.ToDateTime(this.dgvAgreement.CurrentRow.Cells["DEnd"].Value));
              nullable = this.monthClosed.PeriodName;
              DateTime dateTime1 = nullable.Value;
              dateTime1 = dateTime1.AddMonths(1);
              DateTime dateTime2 = dateTime1.AddDays(-1.0);
              int num1;
              if (flag2)
              {
                if (!(dataBoundItem.DBeg <= dateTime2))
                {
                  nullable = dataBoundItem.DEnd;
                  DateTime dateTime3 = dateTime2;
                  num1 = nullable.HasValue ? (nullable.GetValueOrDefault() <= dateTime3 ? 1 : 0) : 0;
                }
                else
                  num1 = 1;
              }
              else
                num1 = 0;
              if (num1 != 0)
              {
                int num2 = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
              }
              int num3;
              if (!flag2)
              {
                if (agreement1.DBeg <= dateTime2)
                {
                  nullable = agreement1.DEnd;
                  DateTime dateTime3 = dateTime2;
                  if ((nullable.HasValue ? (nullable.GetValueOrDefault() < dateTime3 ? 1 : 0) : 0) != 0)
                    goto label_21;
                }
                nullable = dataBoundItem.DEnd;
                DateTime dateTime4 = dateTime2;
                if ((nullable.HasValue ? (nullable.GetValueOrDefault() < dateTime4 ? 1 : 0) : 0) == 0 && (!(agreement1.DBeg > dateTime2) || !(dataBoundItem.DBeg <= dateTime2)))
                {
                  num3 = !(agreement1.DBeg <= dateTime2) ? 0 : (agreement1.DBeg != dataBoundItem.DBeg || agreement1.AgreementNum != dataBoundItem.AgreementNum || agreement1.Dept != dataBoundItem.Dept ? 1 : ((int) agreement1.MonthCount != (int) dataBoundItem.MonthCount ? 1 : 0));
                  goto label_23;
                }
label_21:
                num3 = 1;
              }
              else
                num3 = 0;
label_23:
              if (num3 != 0)
              {
                int num2 = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
              }
              nullable = dataBoundItem.DEnd;
              DateTime dbeg = dataBoundItem.DBeg;
              if (nullable.HasValue && nullable.GetValueOrDefault() < dbeg)
              {
                int num2 = (int) MessageBox.Show("Дата окончания меньше даты начала", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
              }
            }
            else
              dataBoundItem.DEnd = new DateTime?();
          }
          catch
          {
            int num = (int) MessageBox.Show("Введенная дата окончания некорректна", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
        }
        DateTime dateTime5;
        int num4;
        if (flag2)
        {
          DateTime dbeg = dataBoundItem.DBeg;
          nullable = this.monthClosed.PeriodName;
          dateTime5 = nullable.Value;
          DateTime dateTime1 = dateTime5.AddMonths(1);
          num4 = dbeg < dateTime1 ? 1 : 0;
        }
        else
          num4 = 0;
        if (num4 != 0)
        {
          int num5 = (int) MessageBox.Show("Дата начала принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else if (this.dgvAgreement.CurrentRow.Cells["AgreementNum"].Value != null)
        {
          dataBoundItem.AgreementNum = this.dgvAgreement.CurrentRow.Cells["AgreementNum"].Value.ToString();
          if (this.dgvAgreement.CurrentRow.Cells["MonthCount"].Value != null && (uint) Convert.ToInt16(this.dgvAgreement.CurrentRow.Cells["MonthCount"].Value) > 0U)
          {
            try
            {
              dataBoundItem.MonthCount = Convert.ToInt16(this.dgvAgreement.CurrentRow.Cells["MonthCount"].Value);
              if ((int) dataBoundItem.MonthCount <= 0)
              {
                int num1 = (int) MessageBox.Show("Количество месяцев введено некорректно", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
              }
            }
            catch
            {
              int num1 = (int) MessageBox.Show("Количество месяцев введено некорректно", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return;
            }
            if (this.dgvAgreement.CurrentRow.Cells["Dept"].Value != null && this.dgvAgreement.CurrentRow.Cells["DeptPeni"].Value != null)
            {
              try
              {
                dataBoundItem.Dept = Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.dgvAgreement.CurrentRow.Cells["Dept"].Value.ToString()));
                dataBoundItem.DeptPeni = Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.dgvAgreement.CurrentRow.Cells["DeptPeni"].Value.ToString()));
              }
              catch
              {
                int num1 = (int) MessageBox.Show("Введенные суммы не корректны", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
              }
              dataBoundItem.Note = this.dgvAgreement.CurrentRow.Cells["Note"].Value == null ? "" : this.dgvAgreement.CurrentRow.Cells["Dept"].Value.ToString();
              if (!flag2)
              {
                DateTime dbeg = dataBoundItem.DBeg;
                nullable = this.monthClosed.PeriodName;
                dateTime5 = nullable.Value;
                DateTime dateTime1 = dateTime5.AddMonths(1);
                if (dbeg < dateTime1 && (dataBoundItem.AgreementNum != agreement1.AgreementNum || dataBoundItem.Dept != agreement1.Dept || dataBoundItem.DeptPeni != agreement1.DeptPeni || (int) dataBoundItem.MonthCount != (int) agreement1.MonthCount))
                {
                  int num1 = (int) MessageBox.Show("Запись принадлежит закрытому периоду. Невозможно внести изменения кроме даты окончания и примечаний!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  return;
                }
                if ((dataBoundItem.Dept != agreement1.Dept || dataBoundItem.DeptPeni != agreement1.DeptPeni || (int) dataBoundItem.MonthCount != (int) agreement1.MonthCount) && MessageBox.Show("Пересчитать рассрочку долга?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                  flag1 = true;
              }
              dataBoundItem.UName = Options.Login;
              dataBoundItem.DEdit = DateTime.Now;
              dataBoundItem.LsClient = this.client;
              this.btnSave.Enabled = false;
              this.btnDelete.Enabled = true;
              this.btnAdd.Enabled = true;
              try
              {
                if (flag2)
                {
                  try
                  {
                    IList<int> intList = this.session.CreateSQLQuery("select DBA.gen_id('lsAgreement',1)").List<int>();
                    dataBoundItem.AgreementId = intList[0];
                  }
                  catch
                  {
                    int num1 = (int) MessageBox.Show("Не удалось сгенерировать код соглашения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                  }
                  this.session.Save((object) dataBoundItem);
                  if (MessageBox.Show("Рассчитать рассрочку долга?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    flag1 = true;
                }
                else
                  this.session.Update((object) dataBoundItem);
                this.session.Flush();
                if (flag1)
                  this.btnCalc_Click((object) null, (EventArgs) null);
              }
              catch (Exception ex)
              {
                int num1 = (int) MessageBox.Show("Не удалось сохранить изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                KvrplHelper.WriteLog(ex, this.client);
              }
              this.LoadAgreement();
            }
            else
            {
              int num2 = (int) MessageBox.Show("Введите суммы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
          }
          else
          {
            int num3 = (int) MessageBox.Show("Введите количество месяцев", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
        }
        else
        {
          int num6 = (int) MessageBox.Show("Введите номер соглашения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
      else
      {
        int num7 = (int) MessageBox.Show("Введите дату начала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Удалить договор?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      this.session = Domain.CurrentSession;
      Agreement dataBoundItem = (Agreement) this.dgvAgreement.CurrentRow.DataBoundItem;
      if (dataBoundItem.DBeg < this.monthClosed.PeriodName.Value.AddMonths(1))
      {
        int num1 = (int) MessageBox.Show("Запись принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        try
        {
          this.session.CreateQuery(string.Format("delete from Instalment where Agreement.AgreementId={0}", (object) ((Agreement) this.dgvAgreement.CurrentRow.DataBoundItem).AgreementId)).ExecuteUpdate();
          this.session.Delete((object) dataBoundItem);
          this.session.Flush();
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show("Невозможно удалить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        this.LoadAgreement();
        this.LoadInstalment();
      }
    }

    private void dgvAgreement_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (((DataGridView) sender).DataSource == null)
        return;
      DataGridViewRow row = ((DataGridView) sender).Rows[e.RowIndex];
      if (((Agreement) row.DataBoundItem).DEnd.HasValue)
        row.DefaultCellStyle.ForeColor = Color.Gray;
      else
        row.DefaultCellStyle.ForeColor = Color.Black;
    }

    private void dgvAgreement_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.btnSave.Enabled = true;
      this.btnAdd.Enabled = false;
      this.btnDelete.Enabled = false;
    }

    private void LoadInstalment()
    {
      this.session.Clear();
      if (this.dgvAgreement.Rows.Count <= 0 || this.dgvAgreement.CurrentRow == null)
        return;
      this.dgvInstalment.DataSource = (object) null;
      this.dgvInstalment.Columns.Clear();
      IList<Instalment> instalmentList = (IList<Instalment>) new List<Instalment>();
      try
      {
        instalmentList = this.session.CreateQuery(string.Format("select i from Instalment i left join fetch i.Period p where i.Agreement.AgreementId={0} order by i.Period.PeriodId asc", (object) ((Agreement) this.dgvAgreement.CurrentRow.DataBoundItem).AgreementId)).List<Instalment>();
      }
      catch (Exception ex)
      {
      }
      this.dgvInstalment.DataSource = (object) instalmentList;
      this.SetViewInstalment();
    }

    private void SetViewInstalment()
    {
      KvrplHelper.AddTextBoxColumn(this.dgvInstalment, 0, "Период", "Period", 80, true);
      KvrplHelper.AddTextBoxColumn(this.dgvInstalment, 1, "Долг", "Debt", 80, false);
      KvrplHelper.AddTextBoxColumn(this.dgvInstalment, 2, "Пени", "DebtPeni", 80, false);
      KvrplHelper.ViewEdit(this.dgvInstalment);
      foreach (DataGridViewRow row in (IEnumerable) this.dgvInstalment.Rows)
      {
        row.Cells["Debt"].Value = (object) ((Instalment) row.DataBoundItem).Debt;
        row.Cells["DebtPeni"].Value = (object) ((Instalment) row.DataBoundItem).DebtPeni;
        if (((Instalment) row.DataBoundItem).Period != null)
          row.Cells["Period"].Value = (object) ((Instalment) row.DataBoundItem).Period.PeriodName.Value.ToShortDateString();
      }
    }

    private void btnCalc_Click(object sender, EventArgs e)
    {
      if (this.btnSave.Enabled)
        this.btnSave_Click((object) null, (EventArgs) null);
      if (this.dgvAgreement.Rows.Count <= 0 || this.dgvAgreement.CurrentRow == null || ((Agreement) this.dgvAgreement.CurrentRow.DataBoundItem).DEnd.HasValue)
        return;
      this.session.Clear();
      Agreement dataBoundItem = (Agreement) this.dgvAgreement.CurrentRow.DataBoundItem;
      Decimal num1 = new Decimal();
      Decimal num2 = new Decimal();
      Period period = new Period();
      try
      {
        period = this.session.CreateQuery(string.Format("from Period where PeriodName='{0}'", (object) KvrplHelper.DateToBaseFormat(KvrplHelper.FirstDay(dataBoundItem.DBeg)))).List<Period>()[0];
      }
      catch (Exception ex)
      {
        int num3 = (int) MessageBox.Show("Не удалось сохранить изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        KvrplHelper.WriteLog(ex, this.client);
      }
      this.session.CreateQuery(string.Format("delete from Instalment where Agreement.AgreementId={0}", (object) ((Agreement) this.dgvAgreement.CurrentRow.DataBoundItem).AgreementId)).ExecuteUpdate();
      for (int index = 0; index < (int) dataBoundItem.MonthCount; ++index)
      {
        Instalment instalment = new Instalment();
        instalment.Agreement = dataBoundItem;
        instalment.Period = this.session.Get<Period>((object) (period.PeriodId + index));
        instalment.Debt = Math.Round(dataBoundItem.Dept / (Decimal) dataBoundItem.MonthCount, 2);
        num1 += instalment.Debt;
        instalment.DebtPeni = Math.Round(dataBoundItem.DeptPeni / (Decimal) dataBoundItem.MonthCount, 2);
        num2 += instalment.DebtPeni;
        instalment.UName = Options.Login;
        instalment.DEdit = DateTime.Now;
        if (index == (int) dataBoundItem.MonthCount - 1 && num1 != dataBoundItem.Dept)
          instalment.Debt += dataBoundItem.Dept - num1;
        if (index == (int) dataBoundItem.MonthCount - 1 && num2 != dataBoundItem.DeptPeni)
          instalment.DebtPeni += dataBoundItem.DeptPeni - num2;
        this.session.Save((object) instalment);
      }
      try
      {
        this.session.Flush();
      }
      catch (Exception ex)
      {
        int num3 = (int) MessageBox.Show("Не удалось сохранить изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        KvrplHelper.WriteLog(ex, this.client);
      }
      this.LoadInstalment();
    }

    private void dgvAgreement_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      this.LoadInstalment();
    }

    private void btnSaveInstallment_Click(object sender, EventArgs e)
    {
      if (this.dgvInstalment.Rows.Count <= 0)
        return;
      Decimal num1 = new Decimal();
      Decimal num2 = new Decimal();
      foreach (DataGridViewRow row in (IEnumerable) this.dgvInstalment.Rows)
      {
        try
        {
          num1 += Convert.ToDecimal(KvrplHelper.ChangeSeparator(row.Cells["Debt"].Value.ToString()));
          num2 += Convert.ToDecimal(KvrplHelper.ChangeSeparator(row.Cells["DebtPeni"].Value.ToString()));
        }
        catch
        {
          int num3 = (int) MessageBox.Show("Введенные суммы не корректны", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
      }
      if ((num1 != ((Agreement) this.dgvAgreement.CurrentRow.DataBoundItem).Dept || num2 != ((Agreement) this.dgvAgreement.CurrentRow.DataBoundItem).DeptPeni) && MessageBox.Show("Сумма рассрочки не равна сумме долга. Все равно сохранить?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
      {
        this.LoadInstalment();
      }
      else
      {
        foreach (DataGridViewRow row in (IEnumerable) this.dgvInstalment.Rows)
        {
          Instalment dataBoundItem = (Instalment) row.DataBoundItem;
          dataBoundItem.Debt = Convert.ToDecimal(KvrplHelper.ChangeSeparator(row.Cells["Debt"].Value.ToString()));
          dataBoundItem.DebtPeni = Convert.ToDecimal(KvrplHelper.ChangeSeparator(row.Cells["DebtPeni"].Value.ToString()));
          this.session.Update((object) dataBoundItem);
        }
        try
        {
          this.session.Flush();
        }
        catch (Exception ex)
        {
          int num3 = (int) MessageBox.Show("Невозможно сохранить записи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          KvrplHelper.WriteLog(ex, this.client);
        }
        this.btnSaveInstallment.Enabled = false;
      }
    }

    private void dgvInstalment_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.btnSaveInstallment.Enabled = true;
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
      this.btnSave = new Button();
      this.btnDelete = new Button();
      this.btnAdd = new Button();
      this.btnExit = new Button();
      this.dgvAgreement = new DataGridView();
      this.pn = new Panel();
      this.dgvInstalment = new DataGridView();
      this.btnCalc = new Button();
      this.lblInstalment = new Label();
      this.btnSaveInstallment = new Button();
      this.pnBtn.SuspendLayout();
      ((ISupportInitialize) this.dgvAgreement).BeginInit();
      this.pn.SuspendLayout();
      ((ISupportInitialize) this.dgvInstalment).BeginInit();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnSaveInstallment);
      this.pnBtn.Controls.Add((Control) this.btnCalc);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 465);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(818, 40);
      this.pnBtn.TabIndex = 0;
      this.btnSave.Image = (Image) Resources.Tick;
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.Location = new Point(228, 5);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(108, 30);
      this.btnSave.TabIndex = 3;
      this.btnSave.Text = "Сохранить";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnDelete.Image = (Image) Resources.minus;
      this.btnDelete.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnDelete.Location = new Point(124, 5);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(98, 30);
      this.btnDelete.TabIndex = 2;
      this.btnDelete.Text = "Удалить";
      this.btnDelete.TextAlign = ContentAlignment.MiddleRight;
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnAdd.Image = (Image) Resources.plus;
      this.btnAdd.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnAdd.Location = new Point(12, 5);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(106, 30);
      this.btnAdd.TabIndex = 1;
      this.btnAdd.Text = "Добавить";
      this.btnAdd.TextAlign = ContentAlignment.MiddleRight;
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(726, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(80, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.dgvAgreement.BackgroundColor = Color.AliceBlue;
      this.dgvAgreement.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvAgreement.Dock = DockStyle.Top;
      this.dgvAgreement.Location = new Point(0, 0);
      this.dgvAgreement.Name = "dgvAgreement";
      this.dgvAgreement.Size = new Size(818, 206);
      this.dgvAgreement.TabIndex = 1;
      this.dgvAgreement.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvAgreement_CellBeginEdit);
      this.dgvAgreement.CellClick += new DataGridViewCellEventHandler(this.dgvAgreement_CellClick);
      this.dgvAgreement.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvAgreement_CellFormatting);
      this.pn.Controls.Add((Control) this.btnAdd);
      this.pn.Controls.Add((Control) this.btnSave);
      this.pn.Controls.Add((Control) this.btnDelete);
      this.pn.Dock = DockStyle.Top;
      this.pn.Location = new Point(0, 206);
      this.pn.Name = "pn";
      this.pn.Size = new Size(818, 40);
      this.pn.TabIndex = 2;
      this.dgvInstalment.BackgroundColor = Color.AliceBlue;
      this.dgvInstalment.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvInstalment.Dock = DockStyle.Fill;
      this.dgvInstalment.Location = new Point(0, 268);
      this.dgvInstalment.Name = "dgvInstalment";
      this.dgvInstalment.Size = new Size(818, 197);
      this.dgvInstalment.TabIndex = 3;
      this.dgvInstalment.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvInstalment_CellBeginEdit);
      this.btnCalc.Location = new Point(12, 5);
      this.btnCalc.Name = "btnCalc";
      this.btnCalc.Size = new Size(191, 30);
      this.btnCalc.TabIndex = 4;
      this.btnCalc.Text = "Расcчитать рассрочку";
      this.btnCalc.UseVisualStyleBackColor = true;
      this.btnCalc.Click += new EventHandler(this.btnCalc_Click);
      this.lblInstalment.BackColor = Color.White;
      this.lblInstalment.Dock = DockStyle.Top;
      this.lblInstalment.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblInstalment.Location = new Point(0, 246);
      this.lblInstalment.Name = "lblInstalment";
      this.lblInstalment.Size = new Size(818, 22);
      this.lblInstalment.TabIndex = 4;
      this.lblInstalment.Text = "Рассрочка по соглашению";
      this.btnSaveInstallment.Enabled = false;
      this.btnSaveInstallment.Image = (Image) Resources.Tick;
      this.btnSaveInstallment.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSaveInstallment.Location = new Point(209, 5);
      this.btnSaveInstallment.Name = "btnSaveInstallment";
      this.btnSaveInstallment.Size = new Size(108, 29);
      this.btnSaveInstallment.TabIndex = 5;
      this.btnSaveInstallment.Text = "Сохранить";
      this.btnSaveInstallment.TextAlign = ContentAlignment.MiddleRight;
      this.btnSaveInstallment.UseVisualStyleBackColor = true;
      this.btnSaveInstallment.Click += new EventHandler(this.btnSaveInstallment_Click);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(818, 505);
      this.Controls.Add((Control) this.dgvInstalment);
      this.Controls.Add((Control) this.lblInstalment);
      this.Controls.Add((Control) this.pn);
      this.Controls.Add((Control) this.dgvAgreement);
      this.Controls.Add((Control) this.pnBtn);
      this.Name = "FrmAgreement";
      this.Text = "Соглашения";
      this.Load += new EventHandler(this.FrmAgreement_Load);
      this.pnBtn.ResumeLayout(false);
      ((ISupportInitialize) this.dgvAgreement).EndInit();
      this.pn.ResumeLayout(false);
      ((ISupportInitialize) this.dgvInstalment).EndInit();
      this.ResumeLayout(false);
    }
  }
}
