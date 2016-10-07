// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmCorrectPeni
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
using System.Linq;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmCorrectPeni : Form
  {
    private IContainer components = (IContainer) null;
    private ISession session;
    private LsClient client;
    private Period period;
    private IList<Service> services;
    private bool insertRecord;
    private CorrectPeni oldCorrect;
    private IList<CorrectPeni> oldListCorrect;
    private ActionPeni oldAction;
    private IList<ActionPeni> oldListAction;
    private IList _listObject;
    private IList _oldList;
    private CorrectPeni oldPeni;
    private Panel pnBtn;
    private Button btnExit;
    private TabControl tcntrl;
    private TabPage pgCorrect;
    private TabPage pgAction;
    private DataGridView dgvCorrect;
    private DataGridView dgvAction;
    private Button btnAdd;
    private Button btnDelete;
    private Button btnSave;

    public FrmCorrectPeni(LsClient client, Period period)
    {
      this.InitializeComponent();
      this.client = client;
      this.period = period;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void FrmCorrectPeni_Shown(object sender, EventArgs e)
    {
      if (this.client.Complex.IdFk == Options.ComplexArenda.IdFk)
        this.tcntrl.TabPages.Remove(this.pgAction);
      this.session = Domain.CurrentSession;
      this.services = this.session.CreateQuery(string.Format("select s from Service s,ServiceParam sp where sp.Service_id=s.ServiceId and s.Root=0 and s.ServiceId<>0 and sp.Company_id={0} and sp.Complex.IdFk={1} order by " + Options.SortService, (object) this.client.Company.CompanyId, (object) this.client.Complex.IdFk)).List<Service>();
      this.services.Insert(0, new Service()
      {
        ServiceId = (short) 0,
        ServiceName = "Общая услуга"
      });
      this.session.Clear();
      this.LoadCorrect();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(45, 2, this.client.Company, true))
        return;
      this.btnDelete.Enabled = false;
      this.btnSave.Enabled = true;
      if (this.tcntrl.SelectedIndex == 0)
        this.AddCorrect();
      if (this.tcntrl.SelectedIndex != 1)
        return;
      this.AddAction();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(45, 2, this.client.Company, true))
        return;
      this.btnAdd.Enabled = true;
      this.btnDelete.Enabled = true;
      this.btnSave.Enabled = false;
      if (this.tcntrl.SelectedIndex == 0)
        this.SaveAllCorrect();
      else
        this.SaveAllAction();
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(45, 2, this.client.Company, true))
        return;
      if (this.tcntrl.SelectedIndex == 0)
        this.DeleteCorrect();
      else
        this.DeleteAction();
    }

    private void LoadCorrect()
    {
      this.btnAdd.Enabled = true;
      this.btnDelete.Enabled = true;
      this.btnSave.Enabled = false;
      this.dgvCorrect.AutoGenerateColumns = false;
      this.dgvCorrect.Columns.Clear();
      this.dgvCorrect.DataSource = (object) null;
      this.session = Domain.CurrentSession;
      this._listObject = this.session.CreateQuery(string.Format("select (select ServiceId from Service where ServiceId = s.Root) as serv, sum(r.Correct) as correct, r.Note, r.UName, r.DEdit,0,0 from CorrectPeni r, Service s where r.Period.PeriodId={0} and r.LsClient.ClientId={1} and r.Service=s group by s.Root, r.Note, r.UName, r.DEdit order by s.Root", (object) Options.Period.PeriodId, (object) this.client.ClientId)).List();
      this._oldList = this.session.CreateQuery(string.Format("select (select ServiceId from Service where ServiceId = s.Root) as serv, sum(r.Correct) as correct, r.Note, r.UName, r.DEdit,0,0 from CorrectPeni r, Service s where r.Period.PeriodId={0} and r.LsClient.ClientId={1} and r.Service=s group by s.Root, r.Note, r.UName, r.DEdit order by s.Root", (object) Options.Period.PeriodId, (object) this.client.ClientId)).List();
      this.session.Clear();
      this.dgvCorrect.DataSource = (object) this._listObject;
      this.session.Clear();
      this.SetViewCorrect();
    }

    private void SetViewCorrect()
    {
      DataGridViewComboBoxColumn viewComboBoxColumn = new DataGridViewComboBoxColumn();
      viewComboBoxColumn.DropDownWidth = 160;
      viewComboBoxColumn.Width = 200;
      viewComboBoxColumn.MaxDropDownItems = 7;
      viewComboBoxColumn.DataSource = (object) this.services;
      viewComboBoxColumn.ValueMember = "ServiceId";
      viewComboBoxColumn.DisplayMember = "ServiceName";
      viewComboBoxColumn.HeaderText = "Услуга";
      viewComboBoxColumn.Name = "Service";
      this.dgvCorrect.Columns.Insert(0, (DataGridViewColumn) viewComboBoxColumn);
      DataGridViewTextBoxColumn viewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      viewTextBoxColumn1.Width = 100;
      viewTextBoxColumn1.HeaderText = "Сумма";
      viewTextBoxColumn1.Name = "Correct";
      this.dgvCorrect.Columns.Insert(1, (DataGridViewColumn) viewTextBoxColumn1);
      DataGridViewTextBoxColumn viewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      viewTextBoxColumn2.Width = 200;
      viewTextBoxColumn2.HeaderText = "Примечание";
      viewTextBoxColumn2.Name = "Note";
      this.dgvCorrect.Columns.Insert(2, (DataGridViewColumn) viewTextBoxColumn2);
      DataGridViewTextBoxColumn viewTextBoxColumn3 = new DataGridViewTextBoxColumn();
      viewTextBoxColumn3.Width = 100;
      viewTextBoxColumn3.HeaderText = "Пользователь";
      viewTextBoxColumn3.Name = "UName";
      viewTextBoxColumn3.ReadOnly = true;
      this.dgvCorrect.Columns.Insert(3, (DataGridViewColumn) viewTextBoxColumn3);
      DataGridViewTextBoxColumn viewTextBoxColumn4 = new DataGridViewTextBoxColumn();
      viewTextBoxColumn4.Width = 100;
      viewTextBoxColumn4.HeaderText = "Дата редактирования";
      viewTextBoxColumn4.Name = "DEdit";
      viewTextBoxColumn4.ReadOnly = true;
      this.dgvCorrect.Columns.Insert(4, (DataGridViewColumn) viewTextBoxColumn4);
      KvrplHelper.AddTextBoxColumn(this.dgvCorrect, 5, "Edit", "Edit", 1, false);
      KvrplHelper.AddTextBoxColumn(this.dgvCorrect, 6, "Insert", "Insert", 1, false);
      this.dgvCorrect.Columns["Insert"].Visible = false;
      this.dgvCorrect.Columns["Edit"].Visible = false;
      IList<BaseOrg> baseOrgList1 = (IList<BaseOrg>) new List<BaseOrg>();
      IList<BaseOrg> baseOrgList2 = (IList<BaseOrg>) new List<BaseOrg>();
      KvrplHelper.ViewEdit(this.dgvCorrect);
      foreach (DataGridViewRow row in (IEnumerable) this.dgvCorrect.Rows)
      {
        row.Cells["Service"].Value = ((object[]) this._listObject[row.Index])[0];
        if ((int) Convert.ToInt16(((object[]) this._listObject[row.Index])[6]) == 0)
          row.Cells["Service"].ReadOnly = true;
        row.Cells["Correct"].Value = ((object[]) this._listObject[row.Index])[1];
        row.Cells["Note"].Value = ((object[]) this._listObject[row.Index])[2];
        if ((int) Convert.ToInt16(((object[]) this._listObject[row.Index])[6]) == 0)
          row.Cells["Note"].ReadOnly = true;
        row.Cells["UName"].Value = ((object[]) this._listObject[row.Index])[3];
        row.Cells["DEdit"].Value = ((object[]) this._listObject[row.Index])[4];
        row.Cells["Edit"].Value = ((object[]) this._listObject[row.Index])[5];
        row.Cells["Insert"].Value = ((object[]) this._listObject[row.Index])[6];
      }
    }

    private void AddCorrect()
    {
      this.insertRecord = true;
      object[] objArray = new object[7]{ (object) 0, (object) 0, (object) "", (object) null, (object) null, (object) 0, (object) 1 };
      this._listObject = (IList) new ArrayList();
      if ((uint) this.dgvCorrect.Rows.Count > 0U)
        this._listObject = (IList) (this.dgvCorrect.DataSource as ArrayList);
      this._listObject.Add((object) objArray);
      this.dgvCorrect.Columns.Clear();
      this.dgvCorrect.DataSource = (object) null;
      this.dgvCorrect.DataSource = (object) this._listObject;
      this.SetViewCorrect();
    }

    private void SaveAllCorrect()
    {
      bool flag = false;
      int index1 = 0;
      int index2 = 0;
      foreach (DataGridViewRow row in (IEnumerable) this.dgvCorrect.Rows)
      {
        if ((int) Convert.ToInt16(row.Cells["Edit"].Value) == 1)
        {
          index2 = row.Index;
          this.insertRecord = (int) Convert.ToInt16(row.Cells["Insert"].Value.ToString()) == 1;
          this.dgvCorrect.Rows[index2].Selected = true;
          this.dgvCorrect.CurrentCell = row.Cells[0];
          if (this.NewSaveCorrect(index1))
          {
            this.dgvCorrect.Rows[index2].Cells["Insert"].Value = (object) 0;
            this.dgvCorrect.Rows[index2].Cells["Edit"].Value = (object) 0;
          }
          else
            flag = true;
        }
        if ((int) Convert.ToInt16(this.dgvCorrect.Rows[index2].Cells["Insert"].Value) != 1)
          ++index1;
      }
      if (flag)
        return;
      this.LoadCorrect();
    }

    private bool SaveCorrect()
    {
      if (this.dgvCorrect.Rows.Count <= 0 || this.dgvCorrect.CurrentRow == null)
        return true;
      if (this.period.PeriodName.Value <= KvrplHelper.GetCmpKvrClose(this.client.Company, 101, Options.ComplexPrior.IdFk).PeriodName.Value)
      {
        int num = (int) MessageBox.Show("Невозможно внести запись в закрытый период", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      this.session = Domain.CurrentSession;
      CorrectPeni dataBoundItem = (CorrectPeni) this.dgvCorrect.CurrentRow.DataBoundItem;
      this.insertRecord = dataBoundItem.UName == null;
      if (this.dgvCorrect.CurrentRow.Cells["Correct"].Value == null)
      {
        int num = (int) MessageBox.Show("Введите сумму", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      try
      {
        dataBoundItem.Correct = Convert.ToDouble(KvrplHelper.ChangeSeparator(this.dgvCorrect.CurrentRow.Cells["Correct"].Value.ToString()));
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Проверьте правильность введенной суммы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.dgvCorrect.CurrentRow.Cells["Service"].Value == null)
      {
        int num = (int) MessageBox.Show("Введите услугу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      dataBoundItem.Service = this.session.Get<Service>(this.dgvCorrect.CurrentRow.Cells["Service"].Value);
      if (dataBoundItem.Note == null)
        dataBoundItem.Note = " ";
      Supplier supplier1 = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(this.dgvCorrect.CurrentRow.Cells["Recipient"].Value), (object) Convert.ToInt32(this.dgvCorrect.CurrentRow.Cells["Perfomer"].Value))).List<Supplier>()[0];
      int supplier2 = dataBoundItem.Supplier;
      dataBoundItem.Supplier = supplier1.SupplierId;
      dataBoundItem.UName = Options.Login;
      dataBoundItem.DEdit = DateTime.Now.Date;
      try
      {
        if (this.insertRecord)
        {
          this.insertRecord = false;
          this.session.Save((object) dataBoundItem);
        }
        else
          this.session.CreateQuery(string.Format("update CorrectPeni set Service.ServiceId={0},Correct=:correct, Note='{2}',UName=:uname,DEdit=:dedit,Supplier=:supp where Period.PeriodId={3} and LsClient.ClientId={4} and Service.ServiceId={5} and Supplier={7} and Note='{6}'", (object) dataBoundItem.Service.ServiceId, (object) dataBoundItem.Correct, (object) dataBoundItem.Note, (object) this.period.PeriodId, (object) this.client.ClientId, (object) this.oldCorrect.Service.ServiceId, (object) this.oldCorrect.Note, (object) supplier2)).SetParameter<string>("uname", dataBoundItem.UName).SetParameter<DateTime>("dedit", dataBoundItem.DEdit).SetParameter<double>("correct", dataBoundItem.Correct).SetParameter<int>("supp", dataBoundItem.Supplier).ExecuteUpdate();
        this.session.Flush();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Не удалось сохранить изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      this.session.Clear();
      return true;
    }

    private bool NewSaveCorrect(int index)
    {
      if (this.dgvCorrect.Rows.Count > 0 && this.dgvCorrect.CurrentRow != null)
      {
        IList<CorrectPeni> correctPeniList = (IList<CorrectPeni>) new List<CorrectPeni>();
        IList<RentPeni> rentPeniList1 = (IList<RentPeni>) new List<RentPeni>();
        if (this.period.PeriodName.Value <= KvrplHelper.GetCmpKvrClose(this.client.Company, 101, Options.ComplexPrior.IdFk).PeriodName.Value)
        {
          int num = (int) MessageBox.Show("Невозможно внести запись в закрытый период", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
        if (this.dgvCorrect.CurrentRow.Cells["Correct"].Value == null)
        {
          int num = (int) MessageBox.Show("Введите сумму", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        double newSum;
        try
        {
          newSum = Convert.ToDouble(KvrplHelper.ChangeSeparator(this.dgvCorrect.CurrentRow.Cells["Correct"].Value.ToString()));
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Проверьте правильность введенной суммы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (this.dgvCorrect.CurrentRow.Cells["Service"].Value == null)
        {
          int num = (int) MessageBox.Show("Введите услугу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        string note = this.dgvCorrect.CurrentRow.Cells["Note"].Value.ToString();
        Service service1 = this.session.Get<Service>(this.dgvCorrect.Rows[index].Cells["Service"].Value);
        IList<RentPeni> rentPeniList2 = this.session.CreateQuery(string.Format("select r from RentPeni r,Service s where r.Period.PeriodId={0} and r.LsClient.ClientId = {1} and r.Service=s and s.Root={2} and r.Code=0 ", (object) this.period.PeriodId, (object) this.client.ClientId, (object) service1.ServiceId)).List<RentPeni>();
        if (rentPeniList2.Count > 0)
        {
          try
          {
            ITransaction transaction = this.session.BeginTransaction();
            if (this.insertRecord)
            {
              foreach (RentPeni rentPeni in (IEnumerable<RentPeni>) rentPeniList2)
              {
                this.session.Save((object) new CorrectPeni()
                {
                  Period = Options.Period,
                  LsClient = this.client,
                  Service = rentPeni.Service,
                  Supplier = rentPeni.Supplier.SupplierId,
                  Note = this.dgvCorrect.Rows[index].Cells["Note"].Value.ToString(),
                  UName = Options.Login,
                  DEdit = DateTime.Now
                });
                this.session.Flush();
              }
            }
            else
            {
              Service service2 = this.session.Get<Service>(((object[]) this._listObject[index])[0]);
              IList<CorrectPeni> source = this.session.CreateQuery(string.Format("select r from CorrectPeni r,Service s where r.Period.PeriodId={0} and r.LsClient.ClientId = {1} and r.Service=s and s.Root={2} ", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) service2.ServiceId)).List<CorrectPeni>();
              double num1 = Convert.ToDouble(this.session.CreateQuery(string.Format("select sum(r.Correct) from CorrectPeni r, Service s where r.Period.PeriodId={0} and r.LsClient.ClientId={1} and r.Service = s and s.Root={2} ", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) service2.ServiceId)).List()[0]);
              if (num1 == 0.0)
                num1 = 1.0;
              double correct = source.OrderByDescending<CorrectPeni, double>((Func<CorrectPeni, double>) (x => x.Correct)).First<CorrectPeni>().Correct;
              double num2 = 0.0;
              int count = source.Count;
              foreach (CorrectPeni correctPeni in (IEnumerable<CorrectPeni>) source)
              {
                try
                {
                  double val = Math.Round(correctPeni.Correct * newSum / num1, 2);
                  num2 += val;
                  if (count == 1 && Math.Round(num2, 2) != Math.Round(newSum, 2))
                    val = newSum - (num2 - val);
                  --count;
                  this.session.CreateQuery("update CorrectPeni r set r.Correct=:sum, r.Note=:note, r.UName=:uname, r.DEdit=:dedit where r.Period.PeriodId=:pId and r.Service.ServiceId=:servId and r.LsClient.ClientId=:cId and r.Supplier=:supId").SetParameter<double>("sum", val).SetParameter("note", this.dgvCorrect.CurrentRow.Cells["Note"].Value).SetParameter<string>("uname", Options.Login).SetParameter<string>("dedit", KvrplHelper.DateToBaseFormat(DateTime.Now)).SetParameter<int>("pId", Options.Period.PeriodId).SetParameter<short>("servId", correctPeni.Service.ServiceId).SetParameter<int>("cId", this.client.ClientId).SetParameter<int>("supId", correctPeni.Supplier).ExecuteUpdate();
                }
                catch (Exception ex)
                {
                  KvrplHelper.WriteLog(ex, (LsClient) null);
                }
              }
            }
            transaction.Commit();
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Не удалось сохранить изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
        }
        else if (this.insertRecord)
        {
          FrmHandMadeDetailPeni handMadeDetailPeni = new FrmHandMadeDetailPeni(this.client, service1, this.period, newSum, this.oldPeni, true, note);
          int num = (int) handMadeDetailPeni.ShowDialog();
          handMadeDetailPeni.Dispose();
          this.insertRecord = false;
          this.oldPeni = (CorrectPeni) null;
        }
        else if (this.tcntrl.SelectedIndex == 0)
          this.DetailCorrect(this.dgvCorrect);
      }
      return true;
    }

    private void DetailCorrect(DataGridView dataGridView)
    {
      this.session = Domain.CurrentSession;
      Service service = this.session.Get<Service>(dataGridView.CurrentRow.Cells["Service"].Value);
      this.session.Clear();
      string note = dataGridView.CurrentRow.Cells["Note"].Value.ToString();
      this.GetOld();
      FrmHandMadeDetailPeni handMadeDetailPeni = new FrmHandMadeDetailPeni(this.client, service, this.period, 0.0, this.oldPeni, false, note);
      int num = (int) handMadeDetailPeni.ShowDialog();
      handMadeDetailPeni.Dispose();
      this.LoadCorrect();
    }

    private void DeleteCorrect()
    {
      if (this.dgvCorrect.Rows.Count <= 0 || this.dgvCorrect.CurrentRow == null || this.dgvCorrect.CurrentRow.Index < 0)
        return;
      this.session = Domain.CurrentSession;
      if (Options.Period.PeriodName.Value <= KvrplHelper.GetCmpKvrClose(this.client.Company, 101, Options.ComplexPrior.IdFk).PeriodName.Value)
      {
        int num1 = (int) MessageBox.Show("Невозможно удалить запись в закрытом месяце", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        Service service = this.session.Get<Service>(this.dgvCorrect.CurrentRow.Cells["Service"].Value);
        if (MessageBox.Show("Вы уверены, что хотите удалить запись", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
        {
          try
          {
            this.session.CreateQuery("delete from CorrectPeni where Period.PeriodId=" + (object) Options.Period.PeriodId + " and Service in (select s from Service s where s.Root=" + (object) service.ServiceId + ") and  LsClient.ClientId=" + (object) this.client.ClientId ?? "").ExecuteUpdate();
          }
          catch (Exception ex)
          {
            int num2 = (int) MessageBox.Show("Невозможно удалить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
          this.session.Clear();
          this.LoadCorrect();
        }
      }
    }

    private void dgvCorrect_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
    }

    private void dgvCorrect_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.btnDelete.Enabled = false;
      this.btnSave.Enabled = true;
      ((DataGridView) sender).CurrentRow.Cells["Edit"].Value = (object) 1;
      this.GetOld();
    }

    private void GetOld()
    {
      try
      {
        this.oldPeni = new CorrectPeni();
        this.oldPeni.LsClient = this.client;
        this.oldPeni.Period = Options.Period;
        this.oldPeni.Service = this.session.Get<Service>(this.dgvCorrect.CurrentRow.Cells["Service"].Value);
        this.oldPeni.Note = this.dgvCorrect.CurrentRow.Cells["Note"].Value == null ? "" : this.dgvCorrect.CurrentRow.Cells["Note"].Value.ToString();
        this.oldPeni.Correct = Convert.ToDouble(this.dgvCorrect.CurrentRow.Cells["Correct"].Value);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void LoadAction()
    {
      this.dgvAction.Columns.Clear();
      this.dgvAction.DataSource = (object) null;
      this.session = Domain.CurrentSession;
      IList<ActionPeni> actionPeniList = (IList<ActionPeni>) new List<ActionPeni>();
      this.dgvAction.DataSource = (object) this.session.CreateCriteria(typeof (ActionPeni)).Add((ICriterion) Restrictions.Eq("Period", (object) this.period)).Add((ICriterion) Restrictions.Eq("LsClient", (object) this.client)).AddOrder(Order.Asc("Service.ServiceId")).List<ActionPeni>();
      this.session.Clear();
      this.oldListAction = (IList<ActionPeni>) new List<ActionPeni>();
      this.oldListAction = this.session.CreateCriteria(typeof (ActionPeni)).Add((ICriterion) Restrictions.Eq("Period", (object) this.period)).Add((ICriterion) Restrictions.Eq("LsClient", (object) this.client)).AddOrder(Order.Asc("Service.ServiceId")).List<ActionPeni>();
      int index = 0;
      foreach (ActionPeni actionPeni in (List<ActionPeni>) this.dgvAction.DataSource)
      {
        actionPeni.OldHashCode = actionPeni.GetHashCode();
        actionPeni.IsEdit = false;
        this.oldListAction[index].OldHashCode = actionPeni.OldHashCode;
        ++index;
      }
      this.SetViewAction();
    }

    private void SetViewAction()
    {
      this.dgvAction.Columns["Service"].Visible = false;
      this.dgvAction.Columns["Note"].HeaderText = "Примечания";
      this.dgvAction.Columns["Correct"].Visible = false;
      this.dgvAction.Columns["Note"].DisplayIndex = 0;
      this.dgvAction.Columns["Note"].Width = 250;
      this.dgvAction.Columns["Code"].Visible = false;
      KvrplHelper.AddComboBoxColumn(this.dgvAction, 0, (IList) this.services, "ServiceId", "ServiceName", "Услуга", "Service", 160, 160);
      KvrplHelper.AddTextBoxColumn(this.dgvAction, 1, "Сумма", "Correct", 100, false);
      foreach (DataGridViewRow row in (IEnumerable) this.dgvAction.Rows)
      {
        row.Cells["Correct"].Value = (object) ((ActionPeni) row.DataBoundItem).Correct;
        if (((ActionPeni) row.DataBoundItem).Service != null)
          row.Cells["Service"].Value = (object) ((ActionPeni) row.DataBoundItem).Service.ServiceId;
      }
    }

    private void AddAction()
    {
      this.insertRecord = true;
      ActionPeni actionPeni = new ActionPeni();
      actionPeni.Period = this.period;
      actionPeni.Code = (short) 1;
      IList<ActionPeni> actionPeniList = (IList<ActionPeni>) new List<ActionPeni>();
      if ((uint) this.dgvAction.Rows.Count > 0U)
        actionPeniList = (IList<ActionPeni>) (this.dgvAction.DataSource as List<ActionPeni>);
      actionPeniList.Add(actionPeni);
      this.dgvAction.Columns.Clear();
      this.dgvAction.DataSource = (object) null;
      this.dgvAction.DataSource = (object) actionPeniList;
      this.SetViewAction();
      this.dgvAction.CurrentCell = this.dgvAction.Rows[this.dgvAction.Rows.Count - 1].Cells[0];
    }

    private void SaveAllAction()
    {
      bool flag = false;
      foreach (DataGridViewRow row in (IEnumerable) this.dgvAction.Rows)
      {
        if (((ActionPeni) row.DataBoundItem).IsEdit)
        {
          this.oldAction = new ActionPeni();
          foreach (ActionPeni actionPeni in (IEnumerable<ActionPeni>) this.oldListAction)
          {
            if (actionPeni.OldHashCode == ((ActionPeni) row.DataBoundItem).OldHashCode)
            {
              this.oldAction = actionPeni;
              break;
            }
          }
          this.dgvAction.Rows[row.Index].Selected = true;
          this.dgvAction.CurrentCell = row.Cells[0];
          if (!this.SaveAction())
            flag = true;
          else
            ((ActionPeni) row.DataBoundItem).IsEdit = false;
        }
      }
      if (flag)
        return;
      this.LoadAction();
    }

    private bool SaveAction()
    {
      if (this.dgvAction.Rows.Count <= 0 || this.dgvAction.CurrentRow == null)
        return true;
      this.session = Domain.CurrentSession;
      ActionPeni dataBoundItem = (ActionPeni) this.dgvAction.CurrentRow.DataBoundItem;
      this.insertRecord = dataBoundItem.LsClient == null;
      dataBoundItem.LsClient = this.client;
      DateTime? periodName = this.period.PeriodName;
      DateTime dateTime1 = periodName.Value;
      periodName = KvrplHelper.GetCmpKvrClose(this.client.Company, 101, Options.ComplexPrior.IdFk).PeriodName;
      DateTime dateTime2 = periodName.Value;
      if (dateTime1 <= dateTime2)
      {
        int num = (int) MessageBox.Show("Невозможно внести запись в закрытый период", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (this.dgvAction.CurrentRow.Cells["Correct"].Value == null)
      {
        int num = (int) MessageBox.Show("Введите сумму", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      try
      {
        dataBoundItem.Correct = Convert.ToDouble(KvrplHelper.ChangeSeparator(this.dgvAction.CurrentRow.Cells["Correct"].Value.ToString()));
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Проверьте правильность введенной суммы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.dgvAction.CurrentRow.Cells["Service"].Value == null)
      {
        int num = (int) MessageBox.Show("Введите услугу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      dataBoundItem.Service = this.session.Get<Service>(this.dgvAction.CurrentRow.Cells["Service"].Value);
      if (dataBoundItem.Note == null)
        dataBoundItem.Note = " ";
      dataBoundItem.Supplier = this.session.Get<Supplier>((object) 0);
      try
      {
        if (this.insertRecord)
        {
          this.insertRecord = false;
          this.session.Save((object) dataBoundItem);
        }
        else
          this.session.CreateQuery(string.Format("update ActionPeni set Service.ServiceId={0},Correct={1}, Note='{2}' where Period.PeriodId={3} and LsClient.ClientId={4} and Service.ServiceId={5} and Supplier.BaseOrgId=0 and Code=1", (object) dataBoundItem.Service.ServiceId, (object) dataBoundItem.Correct, (object) dataBoundItem.Note, (object) this.period.PeriodId, (object) this.client.ClientId, (object) this.oldAction.Service.ServiceId)).ExecuteUpdate();
        this.session.Flush();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Не удалось сохранить изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      this.session.Clear();
      return true;
    }

    private void DeleteAction()
    {
      if (this.dgvAction.Rows.Count <= 0 || this.dgvAction.CurrentRow == null)
        return;
      this.session = Domain.CurrentSession;
      DateTime? periodName = Options.Period.PeriodName;
      DateTime dateTime1 = periodName.Value;
      periodName = KvrplHelper.GetCmpKvrClose(this.client.Company, 101, Options.ComplexPrior.IdFk).PeriodName;
      DateTime dateTime2 = periodName.Value;
      if (dateTime1 <= dateTime2)
      {
        int num1 = (int) MessageBox.Show("Невозможно удалить запись в закрытом месяце", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        ActionPeni dataBoundItem = (ActionPeni) this.dgvAction.CurrentRow.DataBoundItem;
        if (MessageBox.Show("Вы уверены, что хотите удалить запись", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
        {
          try
          {
            this.session.Delete((object) dataBoundItem);
            this.session.Flush();
          }
          catch (Exception ex)
          {
            int num2 = (int) MessageBox.Show("Невозможно удалить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
          this.session.Clear();
          this.LoadAction();
        }
      }
    }

    private void dgvAction_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgvAction.CurrentCell == null || this.dgvAction.CurrentCell.Value == null)
        return;
      ActionPeni dataBoundItem = (ActionPeni) this.dgvAction.CurrentRow.DataBoundItem;
      dataBoundItem.IsEdit = true;
      try
      {
        string name = this.dgvAction.Columns[e.ColumnIndex].Name;
        if (!(name == "Service"))
        {
          if (name == "Correct")
          {
            try
            {
              dataBoundItem.Correct = Convert.ToDouble(KvrplHelper.ChangeSeparator(this.dgvAction.CurrentRow.Cells["Correct"].Value.ToString()));
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
            dataBoundItem.Service = this.session.Get<Service>(this.dgvAction.CurrentRow.Cells["Service"].Value);
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

    private void tcntrl_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnAdd.Enabled = true;
      this.btnDelete.Enabled = true;
      this.btnSave.Enabled = false;
      if (this.tcntrl.SelectedIndex != 1)
        return;
      this.LoadAction();
    }

    private void dgvCorrect_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e, this.client.ClientId);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmCorrectPeni));
      this.pnBtn = new Panel();
      this.btnDelete = new Button();
      this.btnSave = new Button();
      this.btnAdd = new Button();
      this.btnExit = new Button();
      this.tcntrl = new TabControl();
      this.pgCorrect = new TabPage();
      this.dgvCorrect = new DataGridView();
      this.pgAction = new TabPage();
      this.dgvAction = new DataGridView();
      this.pnBtn.SuspendLayout();
      this.tcntrl.SuspendLayout();
      this.pgCorrect.SuspendLayout();
      ((ISupportInitialize) this.dgvCorrect).BeginInit();
      this.pgAction.SuspendLayout();
      ((ISupportInitialize) this.dgvAction).BeginInit();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnDelete);
      this.pnBtn.Controls.Add((Control) this.btnSave);
      this.pnBtn.Controls.Add((Control) this.btnAdd);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 282);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(569, 40);
      this.pnBtn.TabIndex = 0;
      this.btnDelete.Image = (Image) Resources.minus;
      this.btnDelete.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnDelete.Location = new Point(121, 5);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(98, 30);
      this.btnDelete.TabIndex = 3;
      this.btnDelete.Text = "Удалить";
      this.btnDelete.TextAlign = ContentAlignment.MiddleRight;
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnSave.Image = (Image) Resources.Tick;
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.Location = new Point(225, 5);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(108, 30);
      this.btnSave.TabIndex = 2;
      this.btnSave.Text = "Сохранить";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnAdd.Image = (Image) Resources.plus;
      this.btnAdd.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnAdd.Location = new Point(12, 5);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(103, 30);
      this.btnAdd.TabIndex = 1;
      this.btnAdd.Text = "Добавить";
      this.btnAdd.TextAlign = ContentAlignment.MiddleRight;
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(482, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(75, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.tcntrl.Controls.Add((Control) this.pgCorrect);
      this.tcntrl.Controls.Add((Control) this.pgAction);
      this.tcntrl.Dock = DockStyle.Fill;
      this.tcntrl.Location = new Point(0, 0);
      this.tcntrl.Name = "tcntrl";
      this.tcntrl.SelectedIndex = 0;
      this.tcntrl.Size = new Size(569, 282);
      this.tcntrl.TabIndex = 1;
      this.tcntrl.SelectedIndexChanged += new EventHandler(this.tcntrl_SelectedIndexChanged);
      this.pgCorrect.Controls.Add((Control) this.dgvCorrect);
      this.pgCorrect.Location = new Point(4, 25);
      this.pgCorrect.Name = "pgCorrect";
      this.pgCorrect.Padding = new Padding(3);
      this.pgCorrect.Size = new Size(561, 253);
      this.pgCorrect.TabIndex = 0;
      this.pgCorrect.Text = "Корректировки";
      this.pgCorrect.UseVisualStyleBackColor = true;
      this.dgvCorrect.AllowUserToAddRows = false;
      this.dgvCorrect.AllowUserToDeleteRows = false;
      this.dgvCorrect.BackgroundColor = Color.AliceBlue;
      this.dgvCorrect.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvCorrect.Dock = DockStyle.Fill;
      this.dgvCorrect.Location = new Point(3, 3);
      this.dgvCorrect.Name = "dgvCorrect";
      this.dgvCorrect.Size = new Size(555, 247);
      this.dgvCorrect.TabIndex = 0;
      this.dgvCorrect.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvCorrect_CellBeginEdit);
      this.dgvCorrect.CellEndEdit += new DataGridViewCellEventHandler(this.dgvCorrect_CellEndEdit);
      this.dgvCorrect.DataError += new DataGridViewDataErrorEventHandler(this.dgvCorrect_DataError);
      this.pgAction.Controls.Add((Control) this.dgvAction);
      this.pgAction.Location = new Point(4, 25);
      this.pgAction.Name = "pgAction";
      this.pgAction.Padding = new Padding(3);
      this.pgAction.Size = new Size(561, 253);
      this.pgAction.TabIndex = 1;
      this.pgAction.Text = "Корректировки по акции";
      this.pgAction.UseVisualStyleBackColor = true;
      this.dgvAction.BackgroundColor = Color.AliceBlue;
      this.dgvAction.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvAction.Dock = DockStyle.Fill;
      this.dgvAction.Location = new Point(3, 3);
      this.dgvAction.Name = "dgvAction";
      this.dgvAction.Size = new Size(555, 247);
      this.dgvAction.TabIndex = 0;
      this.dgvAction.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvCorrect_CellBeginEdit);
      this.dgvAction.CellEndEdit += new DataGridViewCellEventHandler(this.dgvAction_CellEndEdit);
      this.dgvAction.DataError += new DataGridViewDataErrorEventHandler(this.dgvCorrect_DataError);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(569, 322);
      this.Controls.Add((Control) this.tcntrl);
      this.Controls.Add((Control) this.pnBtn);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmCorrectPeni";
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Корректировки по пеням";
      this.Shown += new EventHandler(this.FrmCorrectPeni_Shown);
      this.pnBtn.ResumeLayout(false);
      this.tcntrl.ResumeLayout(false);
      this.pgCorrect.ResumeLayout(false);
      ((ISupportInitialize) this.dgvCorrect).EndInit();
      this.pgAction.ResumeLayout(false);
      ((ISupportInitialize) this.dgvAction).EndInit();
      this.ResumeLayout(false);
    }
  }
}
