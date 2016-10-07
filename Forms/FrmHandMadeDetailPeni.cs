// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmHandMadeDetailPeni
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
  public class FrmHandMadeDetailPeni : Form
  {
    private readonly FormStateSaver _fss = new FormStateSaver(FrmHandMadeDetailPeni.ic);
    private IContainer components = (IContainer) null;
    private static IContainer ic;
    private readonly string argument;
    private readonly LsClient _client;
    private readonly short code;
    private readonly bool handMadeMSP;
    private readonly bool insertRecord;
    private readonly Period _period;
    private readonly DcMSP msp;
    private readonly CorrectPeni _old;
    private readonly RentMSP oldMSP;
    private readonly short oldType;
    private readonly Person person;
    private readonly short rentType;
    private readonly Service _service;
    private ISession session;
    private double _summa;
    private double summaEO;
    private double volume;
    private Panel pnUp;
    private Panel pnBtn;
    private Panel pnMain;
    private DataGridView dgvDetailPeni;
    private Button butCancel;
    private Button butOk;
    private Label lblItogo;

    public FrmHandMadeDetailPeni()
    {
      this.InitializeComponent();
      this._fss.ParentForm = (Form) this;
    }

    public FrmHandMadeDetailPeni(LsClient client, Service service, Period period, double newSum, CorrectPeni correctPeni, bool insertrec, string note)
    {
      this.InitializeComponent();
      this._fss.ParentForm = (Form) this;
      this._client = client;
      this._service = service;
      this._period = period;
      this._summa = newSum;
      this._old = correctPeni;
      this.insertRecord = insertrec;
      this.argument = note;
    }

    private void FrmHandMadeDetailPeni_Shown(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      short num1 = 0;
      this.dgvDetailPeni.AutoGenerateColumns = false;
      IList<Service> serviceList = this.session.CreateCriteria(typeof (Service)).Add((ICriterion) Restrictions.Eq("Root", (object) this._service.ServiceId)).AddOrder(Order.Asc("ServiceId")).List<Service>();
      IList<BaseOrg> baseOrgList1 = this.session.CreateQuery(string.Format("select distinct b.Recipient from Supplier b,CmpSupplier s where b=s.SupplierOrg and b.Recipient.BaseOrgId<>0 and s.Service.ServiceId={0} and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={1} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{2}' and Param.ParamId=211)", (object) this._service.ServiceId, (object) this._client.Company.CompanyId, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value))).List<BaseOrg>();
      baseOrgList1.Insert(0, new BaseOrg(0, ""));
      if ((int) this.code == 5)
        this.butOk.Enabled = false;
      if (baseOrgList1.Count == 0)
      {
        int num2 = (int) MessageBox.Show("У данной услуги отсутствуют поставщики", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.Close();
      }
      else
      {
        if ((int) Options.FillCorrectSum == 1)
          num1 = serviceList[0].ServiceId;
        DateTime? periodName;
        if ((int) Options.FillCorrectSum == 2)
        {
          IList list1 = (IList) new ArrayList();
          ISession session1 = this.session;
          string format1 = "select ct.Service.ServiceId from LsService ls,cmpTariffCost ct where ls.Tariff.Tariff_id=ct.Tariff_id and ls.Period.PeriodId=0 and ls.Client.ClientId={0} and ls.Service.ServiceId={1} and ls.DBeg<='{2}' and ls.DEnd>='{2}' and ct.Company_id=(select ParamValue from CompanyParam where Company.CompanyId={3} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{2}' and Param.ParamId=211)  and ct.Period.PeriodId=0and ct.Dbeg<='{2}' and ct.Dend>='{2}' and ct.Service.ServiceId<>{1} order by ct.Service.ServiceId";
          object[] objArray = new object[4]{ (object) this._client.ClientId, (object) this._service.ServiceId, null, null };
          int index1 = 2;
          periodName = Options.Period.PeriodName;
          string baseFormat1 = KvrplHelper.DateToBaseFormat(periodName.Value);
          objArray[index1] = (object) baseFormat1;
          int index2 = 3;
          // ISSUE: variable of a boxed type
          short companyId = this._client.Company.CompanyId;
          objArray[index2] = (object) companyId;
          string queryString1 = string.Format(format1, objArray);
          IList list2 = session1.CreateQuery(queryString1).List();
          if (list2.Count > 0)
          {
            Convert.ToInt16(list2[0]);
          }
          else
          {
            ISession session2 = this.session;
            string format2 = "select Service.ServiceId from LsSupplier where Period.PeriodId=0 and LsClient.ClientId={0} and Service.ServiceId in (select ServiceId from Service where Root={1}) and DBeg<='{2}' and DEnd>='{2}' order by Service.ServiceId";
            // ISSUE: variable of a boxed type
            int clientId = this._client.ClientId;
            // ISSUE: variable of a boxed type
            short serviceId = this._service.ServiceId;
            periodName = Options.Period.PeriodName;
            string baseFormat2 = KvrplHelper.DateToBaseFormat(periodName.Value);
            string queryString2 = string.Format(format2, (object) clientId, (object) serviceId, (object) baseFormat2);
            list1 = session2.CreateQuery(queryString2).List();
          }
          num1 = list1.Count <= 0 ? serviceList[0].ServiceId : Convert.ToInt16(list1[0]);
        }
        if (this.insertRecord)
        {
          foreach (Service service in (IEnumerable<Service>) serviceList)
          {
            CorrectPeni correctPeni = new CorrectPeni();
            correctPeni.Period = Options.Period;
            correctPeni.LsClient = this._client;
            correctPeni.Service = service;
            correctPeni.Note = this.argument;
            int num2 = (int) Options.FillCorrectSum == 0 ? 0 : ((int) service.ServiceId == (int) num1 ? 1 : 0);
            correctPeni.Correct = num2 == 0 ? 0.0 : this._summa;
            IList<CorrectPeni> correctPeniList = (IList<CorrectPeni>) new List<CorrectPeni>();
            if (this.dgvDetailPeni.DataSource != null)
              correctPeniList = this.dgvDetailPeni.DataSource as IList<CorrectPeni>;
            correctPeniList.Add(correctPeni);
            this.dgvDetailPeni.Columns.Clear();
            this.dgvDetailPeni.DataSource = (object) null;
            this.dgvDetailPeni.DataSource = (object) correctPeniList;
          }
        }
        else
        {
          this.dgvDetailPeni.Columns.Clear();
          this.dgvDetailPeni.DataSource = (object) null;
          IList<CorrectPeni> correctPeniList = (IList<CorrectPeni>) new List<CorrectPeni>();
          this.dgvDetailPeni.DataSource = (object) this.session.CreateQuery(string.Format("select r from CorrectPeni r, Service s where r.Period.PeriodId={0} and r.LsClient.ClientId={1} and r.Period.PeriodId={2} and s.Root={3} and r.Note='{4}' and r.Service=s ", (object) Options.Period.PeriodId, (object) this._client.ClientId, (object) this._period.PeriodId, (object) this._service.ServiceId, (object) this.argument)).List<CorrectPeni>();
        }
        DataGridViewTextBoxColumn viewTextBoxColumn1 = new DataGridViewTextBoxColumn();
        viewTextBoxColumn1.Width = 200;
        viewTextBoxColumn1.HeaderText = "Составляющая";
        viewTextBoxColumn1.Name = "Service";
        viewTextBoxColumn1.ReadOnly = true;
        this.dgvDetailPeni.Columns.Insert(0, (DataGridViewColumn) viewTextBoxColumn1);
        DataGridViewTextBoxColumn viewTextBoxColumn2 = new DataGridViewTextBoxColumn();
        viewTextBoxColumn2.Width = 80;
        viewTextBoxColumn2.HeaderText = "Сумма";
        viewTextBoxColumn2.Name = "Correct";
        this.dgvDetailPeni.Columns.Insert(1, (DataGridViewColumn) viewTextBoxColumn2);
        DataGridViewComboBoxColumn viewComboBoxColumn1 = new DataGridViewComboBoxColumn();
        DataGridViewComboBoxColumn viewComboBoxColumn2 = new DataGridViewComboBoxColumn();
        viewComboBoxColumn2.DropDownWidth = 200;
        viewComboBoxColumn2.Width = 200;
        viewComboBoxColumn2.MaxDropDownItems = 7;
        viewComboBoxColumn2.DataSource = (object) baseOrgList1;
        viewComboBoxColumn2.ValueMember = "BaseOrgId";
        viewComboBoxColumn2.DisplayMember = "BaseOrgName";
        viewComboBoxColumn2.HeaderText = "Получатель";
        viewComboBoxColumn2.Name = "Recipient";
        this.dgvDetailPeni.Columns.Insert(2, (DataGridViewColumn) viewComboBoxColumn2);
        KvrplHelper.AddComboBoxColumn(this.dgvDetailPeni, 3, (IList) null, "BaseOrgId", "BaseOrgName", "Исполнитель", "Perfomer", 200, 200);
        Decimal num3 = new Decimal();
        foreach (DataGridViewRow row in (IEnumerable) this.dgvDetailPeni.Rows)
        {
          row.Cells["Service"].Value = (object) ((CorrectPeni) row.DataBoundItem).Service.ServiceName;
          row.Cells["Correct"].Value = (object) ((CorrectPeni) row.DataBoundItem).Correct;
          if (row.Cells["Correct"].Value != null)
            num3 += Convert.ToDecimal(KvrplHelper.ChangeSeparator(row.Cells["Correct"].Value.ToString()));
          Supplier supplier = this.session.Get<Supplier>((object) ((CorrectPeni) row.DataBoundItem).Supplier);
          if (!this.insertRecord && supplier != null)
          {
            row.Cells["Recipient"].Value = (object) supplier.Recipient.BaseOrgId;
            row.Cells["Perfomer"].Value = (object) supplier.Perfomer.BaseOrgId;
          }
          if (supplier != null)
          {
            int baseOrgId1 = supplier.Recipient.BaseOrgId;
            int baseOrgId2 = supplier.Perfomer.BaseOrgId;
            ISession session1 = this.session;
            string format1 = "select distinct d.Perfomer from CmpSupplier s, Supplier d where s.SupplierOrg.SupplierId = d.SupplierId and s.Service.ServiceId={0} and d.Recipient.BaseOrgId={4} and d.Perfomer.BaseOrgId<>0  and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={1} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{3}' and Param.ParamId=211)";
            object[] objArray1 = new object[5];
            objArray1[0] = (object) this._service.ServiceId;
            objArray1[1] = (object) this._client.Company.CompanyId;
            int index1 = 2;
            periodName = Options.Period.PeriodName;
            string baseFormat1 = KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(periodName.Value));
            objArray1[index1] = (object) baseFormat1;
            int index2 = 3;
            periodName = Options.Period.PeriodName;
            string baseFormat2 = KvrplHelper.DateToBaseFormat(periodName.Value);
            objArray1[index2] = (object) baseFormat2;
            int index3 = 4;
            // ISSUE: variable of a boxed type
            int local1 = baseOrgId1;
            objArray1[index3] = (object) local1;
            string queryString1 = string.Format(format1, objArray1);
            IList<BaseOrg> baseOrgList2 = session1.CreateQuery(queryString1).List<BaseOrg>();
            if (baseOrgId1 == 0)
            {
              baseOrgList2.Insert(0, new BaseOrg(0, ""));
            }
            else
            {
              ISession session2 = this.session;
              string format2 = "select distinct d.Perfomer from CmpSupplier s, Supplier d where s.SupplierOrg.SupplierId = d.SupplierId and s.Service.ServiceId={0} and d.Recipient.BaseOrgId={4} and d.Perfomer.BaseOrgId=0  and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={1} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{3}' and Param.ParamId=211)";
              object[] objArray2 = new object[5];
              objArray2[0] = (object) this._service.ServiceId;
              objArray2[1] = (object) this._client.Company.CompanyId;
              int index4 = 2;
              periodName = Options.Period.PeriodName;
              string baseFormat3 = KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(periodName.Value));
              objArray2[index4] = (object) baseFormat3;
              int index5 = 3;
              periodName = Options.Period.PeriodName;
              string baseFormat4 = KvrplHelper.DateToBaseFormat(periodName.Value);
              objArray2[index5] = (object) baseFormat4;
              int index6 = 4;
              // ISSUE: variable of a boxed type
              int local2 = baseOrgId1;
              objArray2[index6] = (object) local2;
              string queryString2 = string.Format(format2, objArray2);
              IList<BaseOrg> baseOrgList3 = session2.CreateQuery(queryString2).List<BaseOrg>();
              if (baseOrgList3.Count > 0)
                baseOrgList2.Insert(0, baseOrgList3[0]);
            }
            row.Cells["Perfomer"] = (DataGridViewCell) new DataGridViewComboBoxCell()
            {
              DisplayStyleForCurrentCellOnly = true,
              ValueMember = "BaseOrgId",
              DisplayMember = "NameOrgMinDop",
              DataSource = (object) baseOrgList2
            };
            row.Cells["Perfomer"].Value = (object) baseOrgId2;
          }
          else
          {
            ISession session = this.session;
            string format = "select distinct d.Perfomer from CmpSupplier s, Supplier d where s.SupplierOrg.SupplierId = d.SupplierId and s.Service.ServiceId={0} and d.Recipient.BaseOrgId=0 and d.Perfomer.BaseOrgId<>0  and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={1} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{3}' and Param.ParamId=211)";
            object[] objArray = new object[4]{ (object) this._service.ServiceId, (object) this._client.Company.CompanyId, null, null };
            int index1 = 2;
            periodName = Options.Period.PeriodName;
            string baseFormat1 = KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(periodName.Value));
            objArray[index1] = (object) baseFormat1;
            int index2 = 3;
            periodName = Options.Period.PeriodName;
            string baseFormat2 = KvrplHelper.DateToBaseFormat(periodName.Value);
            objArray[index2] = (object) baseFormat2;
            string queryString = string.Format(format, objArray);
            IList<BaseOrg> baseOrgList2 = session.CreateQuery(queryString).List<BaseOrg>();
            baseOrgList2.Insert(0, new BaseOrg(0, ""));
            row.Cells["Perfomer"] = (DataGridViewCell) new DataGridViewComboBoxCell()
            {
              DisplayStyleForCurrentCellOnly = true,
              ValueMember = "BaseOrgId",
              DisplayMember = "NameOrgMin",
              DataSource = (object) baseOrgList2
            };
          }
        }
        this.lblItogo.Text = "Итого: " + num3.ToString();
        this.session.Clear();
      }
    }

    private void butCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void dgvDetailPeni_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (!this.dgvDetailPeni.IsCurrentCellDirty)
        return;
      this.dgvDetailPeni.CommitEdit(DataGridViewDataErrorContexts.Commit);
      if (this.dgvDetailPeni.CurrentCell.ColumnIndex == this.dgvDetailPeni.Rows[this.dgvDetailPeni.CurrentRow.Index].Cells["Recipient"].ColumnIndex)
      {
        this.session = Domain.CurrentSession;
        IList<BaseOrg> baseOrgList = this.session.CreateQuery(string.Format("select distinct d.Perfomer from CmpSupplier s, Supplier d where s.SupplierOrg.SupplierId = d.SupplierId and s.Service.ServiceId={0} and d.Perfomer.BaseOrgId<>0 and d.Recipient.BaseOrgId={4}   and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={1} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{3}' and Param.ParamId=211) order by d.Perfomer.NameOrgMin", (object) this._service.ServiceId, (object) this._client.Company.CompanyId, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(Options.Period.PeriodName.Value)), (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), (object) Convert.ToInt32(this.dgvDetailPeni.CurrentRow.Cells["Recipient"].Value))).List<BaseOrg>();
        if (Convert.ToInt32(this.dgvDetailPeni.CurrentRow.Cells["Recipient"].Value) == 0)
          baseOrgList.Insert(0, new BaseOrg(0, ""));
        this.session.Clear();
        this.dgvDetailPeni.CurrentRow.Cells["Perfomer"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "BaseOrgId",
          DisplayMember = "NameOrgMinDop",
          DataSource = (object) baseOrgList
        };
        this.dgvDetailPeni.CurrentRow.Cells["Perfomer"].Value = (object) baseOrgList[0].BaseOrgId;
      }
    }

    private void dgvDetailPeni_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgvDetailPeni.CurrentCell != this.dgvDetailPeni.CurrentRow.Cells["Correct"])
        return;
      Decimal num = new Decimal();
      foreach (DataGridViewRow row in (IEnumerable) this.dgvDetailPeni.Rows)
      {
        if (row.Cells["Correct"].Value != null)
          num += Convert.ToDecimal(KvrplHelper.ChangeSeparator(row.Cells["Correct"].Value.ToString()));
      }
      this.lblItogo.Text = "Итого: " + num.ToString();
    }

    private void butOk_Click(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      this._summa = 0.0;
      ITransaction transaction = this.session.BeginTransaction();
      foreach (DataGridViewRow row in (IEnumerable) this.dgvDetailPeni.Rows)
      {
        if (Options.Period.PeriodName.Value <= KvrplHelper.GetKvrClose(this._client.ClientId, Options.ComplexPasp, Options.ComplexPrior).PeriodName.Value)
        {
          int num = (int) MessageBox.Show("Невозможно внести изменения в закрытом месяце", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.FrmHandMadeDetailPeni_Shown((object) null, (EventArgs) null);
          return;
        }
        CorrectPeni dataBoundItem = (CorrectPeni) row.DataBoundItem;
        try
        {
          dataBoundItem.Correct = row.Cells["Correct"].Value == null ? 0.0 : Convert.ToDouble(KvrplHelper.ChangeSeparator(row.Cells["Correct"].Value.ToString()));
          dataBoundItem.UName = Options.Login;
          dataBoundItem.DEdit = DateTime.Now.Date;
          this._summa = this._summa + dataBoundItem.Correct;
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, this._client);
        }
        try
        {
          if (this.insertRecord)
          {
            dataBoundItem.Supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(row.Cells["Recipient"].Value), (object) Convert.ToInt32(row.Cells["Perfomer"].Value))).List<Supplier>()[0].SupplierId;
            if (dataBoundItem.Correct != 0.0 && dataBoundItem.Supplier == 0 && MessageBox.Show("Вы уверены, что хотите сохранить сумму с невыбранными исполнителем и получателем?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
              return;
            if (dataBoundItem.Correct != 0.0)
            {
              this.session.Save((object) dataBoundItem);
              this.session.Flush();
            }
          }
          else
          {
            Supplier supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(row.Cells["Recipient"].Value), (object) Convert.ToInt32(row.Cells["Perfomer"].Value))).List<Supplier>()[0];
            if (dataBoundItem.Correct != 0.0 && supplier.SupplierId == 0 && MessageBox.Show("Вы уверены, что хотите сохранить сумму с невыбранными исполнителем и получателем?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
              return;
            if (dataBoundItem.Correct == 0.0)
            {
              this.session.Delete((object) dataBoundItem);
              this.session.Flush();
            }
            else
              this.session.CreateQuery("update CorrectPeni r set r.Correct=:sum, r.Note=:note, r.UName=:uname, r.DEdit=:dedit where r.Period.PeriodId=:pId and r.Service.ServiceId=:servId and r.LsClient.ClientId=:cId and r.Supplier=:supId").SetParameter<double>("sum", dataBoundItem.Correct).SetParameter<string>("note", dataBoundItem.Note).SetParameter<string>("uname", Options.Login).SetParameter<string>("dedit", KvrplHelper.DateToBaseFormat(DateTime.Now)).SetParameter<int>("pId", Options.Period.PeriodId).SetParameter<short>("servId", dataBoundItem.Service.ServiceId).SetParameter<int>("cId", this._client.ClientId).SetParameter<int>("supId", dataBoundItem.Supplier).ExecuteUpdate();
          }
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, this._client);
        }
      }
      try
      {
        transaction.Commit();
        CorrectPeni peni = new CorrectPeni();
        peni.LsClient = this._client;
        peni.Note = this.argument;
        peni.Service = this._service;
        peni.Period = Options.Period;
        peni.Correct = this._summa;
        if (Convert.ToInt32(KvrplHelper.BaseValue(32, this._client.Company)) == 1)
        {
          if (this.insertRecord)
            KvrplHelper.SaveCorrectPeniToNoteBook(peni, this._client, (short) 1, KvrplHelper.GetKvrClose(this._client.ClientId, Options.ComplexPasp, Options.ComplexPrior).PeriodName.Value);
          else
            KvrplHelper.ChangeCorrectPeniToNoteBook(peni, this._old, this._client, (short) 2, KvrplHelper.GetKvrClose(this._client.ClientId, Options.ComplexPasp, Options.ComplexPrior).PeriodName.Value);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Не удалось сохранить изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      this.session.Clear();
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmHandMadeDetailPeni));
      this.pnUp = new Panel();
      this.pnBtn = new Panel();
      this.butCancel = new Button();
      this.butOk = new Button();
      this.pnMain = new Panel();
      this.dgvDetailPeni = new DataGridView();
      this.lblItogo = new Label();
      this.pnUp.SuspendLayout();
      this.pnBtn.SuspendLayout();
      this.pnMain.SuspendLayout();
      ((ISupportInitialize) this.dgvDetailPeni).BeginInit();
      this.SuspendLayout();
      this.pnUp.Controls.Add((Control) this.lblItogo);
      this.pnUp.Dock = DockStyle.Top;
      this.pnUp.Location = new Point(0, 0);
      this.pnUp.Name = "pnUp";
      this.pnUp.Size = new Size(734, 47);
      this.pnUp.TabIndex = 0;
      this.pnBtn.Controls.Add((Control) this.butCancel);
      this.pnBtn.Controls.Add((Control) this.butOk);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 251);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(734, 69);
      this.pnBtn.TabIndex = 1;
      this.butCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.butCancel.Image = (Image) Resources.delete;
      this.butCancel.ImageAlign = ContentAlignment.MiddleLeft;
      this.butCancel.Location = new Point(623, 19);
      this.butCancel.Name = "butCancel";
      this.butCancel.Size = new Size(99, 40);
      this.butCancel.TabIndex = 1;
      this.butCancel.Text = "Отмена";
      this.butCancel.TextAlign = ContentAlignment.MiddleRight;
      this.butCancel.UseVisualStyleBackColor = true;
      this.butCancel.Click += new EventHandler(this.butCancel_Click);
      this.butOk.Image = (Image) Resources.Tick;
      this.butOk.ImageAlign = ContentAlignment.MiddleLeft;
      this.butOk.Location = new Point(12, 19);
      this.butOk.Name = "butOk";
      this.butOk.Size = new Size(60, 40);
      this.butOk.TabIndex = 0;
      this.butOk.Text = "OK";
      this.butOk.TextAlign = ContentAlignment.MiddleRight;
      this.butOk.UseVisualStyleBackColor = true;
      this.butOk.Click += new EventHandler(this.butOk_Click);
      this.pnMain.Controls.Add((Control) this.dgvDetailPeni);
      this.pnMain.Dock = DockStyle.Fill;
      this.pnMain.Location = new Point(0, 47);
      this.pnMain.Name = "pnMain";
      this.pnMain.Size = new Size(734, 204);
      this.pnMain.TabIndex = 2;
      this.dgvDetailPeni.AllowUserToAddRows = false;
      this.dgvDetailPeni.AllowUserToDeleteRows = false;
      this.dgvDetailPeni.BackgroundColor = Color.AliceBlue;
      this.dgvDetailPeni.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvDetailPeni.Dock = DockStyle.Fill;
      this.dgvDetailPeni.Location = new Point(0, 0);
      this.dgvDetailPeni.Name = "dgvDetailPeni";
      this.dgvDetailPeni.Size = new Size(734, 204);
      this.dgvDetailPeni.TabIndex = 0;
      this.dgvDetailPeni.CellEndEdit += new DataGridViewCellEventHandler(this.dgvDetailPeni_CellEndEdit);
      this.dgvDetailPeni.CurrentCellDirtyStateChanged += new EventHandler(this.dgvDetailPeni_CurrentCellDirtyStateChanged);
      this.lblItogo.AutoSize = true;
      this.lblItogo.Location = new Point(9, 9);
      this.lblItogo.Name = "lblItogo";
      this.lblItogo.Size = new Size(0, 16);
      this.lblItogo.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(734, 320);
      this.Controls.Add((Control) this.pnMain);
      this.Controls.Add((Control) this.pnBtn);
      this.Controls.Add((Control) this.pnUp);
      this.Font = new Font("Microsoft Sans Serif", 9.75f);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = "FrmHandMadeDetailPeni";
      this.Text = "Разбивка по состовляющим пени";
      this.Shown += new EventHandler(this.FrmHandMadeDetailPeni_Shown);
      this.pnUp.ResumeLayout(false);
      this.pnUp.PerformLayout();
      this.pnBtn.ResumeLayout(false);
      this.pnMain.ResumeLayout(false);
      ((ISupportInitialize) this.dgvDetailPeni).EndInit();
      this.ResumeLayout(false);
    }
  }
}
