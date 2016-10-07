// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmHandMadeDetail
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
  public class FrmHandMadeDetail : Form
  {
    private readonly FormStateSaver fss = new FormStateSaver(FrmHandMadeDetail.ic);
    private IContainer components = (IContainer) null;
    private static IContainer ic;
    private readonly string argument;
    private readonly LsClient client;
    private readonly short code;
    private readonly bool handMadeMSP;
    private readonly bool insertRecord;
    private readonly Period month;
    private readonly DcMSP msp;
    private readonly CorrectRent old;
    private readonly RentMSP oldMSP;
    private readonly short oldType;
    private readonly Person person;
    private readonly short rentType;
    private readonly Service service;
    private ISession session;
    private double summa;
    private double summaEO;
    private double volume;
    private Panel pnBtn;
    private Button btnCancel;
    private DataGridView dgvDetail;
    private Button btnOk;
    private Panel pnUp;
    private Label lblItogo;

    public FrmHandMadeDetail()
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
    }

    public FrmHandMadeDetail(LsClient client, Service service, Period month, DcMSP msp, Person person, bool handMadeMSP, bool insertRecord, short code, string argument, CorrectRent old, RentMSP oldMSP, short rentType, short oldType, double sum, double sumEO, double vol)
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
      this.client = client;
      this.service = service;
      this.month = month;
      this.msp = msp;
      this.person = person;
      this.handMadeMSP = handMadeMSP;
      this.insertRecord = insertRecord;
      this.code = code;
      this.argument = argument;
      this.old = old;
      this.oldMSP = oldMSP;
      this.rentType = rentType;
      this.oldType = oldType;
      this.summa = sum;
      this.summaEO = sumEO;
      this.volume = vol;
    }

    private void FrmHandMadeDetail_Shown(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      short num1 = 0;
      IList<Service> serviceList = this.session.CreateCriteria(typeof (Service)).Add((ICriterion) Restrictions.Eq("Root", (object) this.service.ServiceId)).AddOrder(Order.Asc("ServiceId")).List<Service>();
      IList<BaseOrg> baseOrgList1 = this.session.CreateQuery(string.Format("select distinct b.Recipient from Supplier b,CmpSupplier s where b=s.SupplierOrg and b.Recipient.BaseOrgId<>0 and s.Service.ServiceId={0} and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={1} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{2}' and Param.ParamId=211)", (object) this.service.ServiceId, (object) this.client.Company.CompanyId, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value))).List<BaseOrg>();
      baseOrgList1.Insert(0, new BaseOrg(0, ""));
      if ((int) this.code == 5)
        this.btnOk.Enabled = false;
      if (baseOrgList1.Count == 0)
      {
        int num2 = (int) MessageBox.Show("У данной услуги отсутствуют поставщики", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.Close();
      }
      else
      {
        if ((int) Options.FillCorrectSum == 1)
          num1 = serviceList[0].ServiceId;
        if ((int) Options.FillCorrectSum == 2)
        {
          IList list1 = (IList) new ArrayList();
          IList list2 = this.session.CreateQuery(string.Format("select ct.Service.ServiceId from LsService ls,cmpTariffCost ct where ls.Tariff.Tariff_id=ct.Tariff_id and ls.Period.PeriodId=0 and ls.Client.ClientId={0} and ls.Service.ServiceId={1} and ls.DBeg<='{2}' and ls.DEnd>='{2}' and ct.Company_id=(select ParamValue from CompanyParam where Company.CompanyId={3} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{2}' and Param.ParamId=211)  and ct.Period.PeriodId=0and ct.Dbeg<='{2}' and ct.Dend>='{2}' and ct.Service.ServiceId<>{1} order by ct.Service.ServiceId", (object) this.client.ClientId, (object) this.service.ServiceId, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), (object) this.client.Company.CompanyId)).List();
          if (list2.Count > 0)
            Convert.ToInt16(list2[0]);
          else
            list1 = this.session.CreateQuery(string.Format("select Service.ServiceId from LsSupplier where Period.PeriodId=0 and LsClient.ClientId={0} and Service.ServiceId in (select ServiceId from Service where Root={1}) and DBeg<='{2}' and DEnd>='{2}' order by Service.ServiceId", (object) this.client.ClientId, (object) this.service.ServiceId, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value))).List();
          num1 = list1.Count <= 0 ? serviceList[0].ServiceId : Convert.ToInt16(list1[0]);
        }
        if (this.insertRecord)
        {
          if (!this.handMadeMSP)
          {
            foreach (Service service in (IEnumerable<Service>) serviceList)
            {
              if ((int) this.code == 4)
              {
                CorrectRent correctRent = new CorrectRent();
                correctRent.Period = Options.Period;
                correctRent.LsClient = this.client;
                correctRent.Service = service;
                correctRent.Month = this.month;
                correctRent.Note = this.argument;
                if ((int) Options.FillCorrectSum != 0 && (int) service.ServiceId == (int) num1)
                {
                  correctRent.Volume = this.volume;
                  correctRent.RentEO = this.summaEO;
                  correctRent.RentMain = this.summa;
                }
                else
                {
                  correctRent.Volume = 0.0;
                  correctRent.RentEO = 0.0;
                  correctRent.RentMain = 0.0;
                }
                correctRent.RentType = this.rentType;
                IList<CorrectRent> correctRentList = (IList<CorrectRent>) new List<CorrectRent>();
                if (this.dgvDetail.DataSource != null)
                  correctRentList = this.dgvDetail.DataSource as IList<CorrectRent>;
                correctRentList.Add(correctRent);
                this.dgvDetail.Columns.Clear();
                this.dgvDetail.DataSource = (object) null;
                this.dgvDetail.DataSource = (object) correctRentList;
              }
              else
              {
                Rent rent = new Rent();
                rent.Period = Options.Period;
                rent.LsClient = this.client;
                rent.Service = service;
                rent.Month = this.month;
                rent.Code = this.code;
                rent.Motive = 1;
                if ((int) Options.FillCorrectSum != 0 && (int) service.ServiceId == (int) num1)
                {
                  rent.Volume = this.volume;
                  rent.RentEO = this.summaEO;
                  rent.RentMain = this.summa;
                }
                else
                {
                  rent.Volume = 0.0;
                  rent.RentEO = 0.0;
                  rent.RentMain = 0.0;
                }
                rent.RentType = this.rentType;
                IList<Rent> rentList = (IList<Rent>) new List<Rent>();
                if (this.dgvDetail.DataSource != null)
                  rentList = this.dgvDetail.DataSource as IList<Rent>;
                rentList.Add(rent);
                this.dgvDetail.Columns.Clear();
                this.dgvDetail.DataSource = (object) null;
                this.dgvDetail.DataSource = (object) rentList;
              }
            }
          }
          else
          {
            foreach (Service service in (IEnumerable<Service>) serviceList)
            {
              RentMSP rentMsp = new RentMSP();
              rentMsp.Period = Options.Period;
              rentMsp.LsClient = this.client;
              rentMsp.Service = service;
              rentMsp.Month = this.month;
              rentMsp.Code = this.code;
              rentMsp.MSP = this.msp;
              rentMsp.Person = this.person;
              rentMsp.Motive = 1;
              if ((int) Options.FillCorrectSum != 0 && (int) service.ServiceId == (int) num1)
              {
                rentMsp.Volume = this.volume;
                rentMsp.RentMain = this.summa;
              }
              else
              {
                rentMsp.Volume = 0.0;
                rentMsp.RentMain = 0.0;
              }
              rentMsp.RentType = this.rentType;
              IList<RentMSP> rentMspList = (IList<RentMSP>) new List<RentMSP>();
              if (this.dgvDetail.DataSource != null)
                rentMspList = this.dgvDetail.DataSource as IList<RentMSP>;
              rentMspList.Add(rentMsp);
              this.dgvDetail.Columns.Clear();
              this.dgvDetail.DataSource = (object) null;
              this.dgvDetail.DataSource = (object) rentMspList;
            }
          }
        }
        else
        {
          this.dgvDetail.Columns.Clear();
          this.dgvDetail.DataSource = (object) null;
          if (!this.handMadeMSP)
          {
            if ((int) this.code == 4)
            {
              IList<CorrectRent> correctRentList = (IList<CorrectRent>) new List<CorrectRent>();
              this.dgvDetail.DataSource = (object) this.session.CreateQuery(string.Format("select r from CorrectRent r, Service s where r.Period.PeriodId={0} and r.LsClient.ClientId={1} and r.Month.PeriodId={2} and s.Root={3} and r.Note='{4}' and r.Service=s ", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) this.month.PeriodId, (object) this.service.ServiceId, (object) this.argument)).List<CorrectRent>();
            }
            else if ((int) this.code == 5)
            {
              IList<Rent> rentList = (IList<Rent>) new List<Rent>();
              this.dgvDetail.DataSource = (object) this.session.CreateQuery(string.Format("select r from Rent r, Service s where r.Period.PeriodId={0} and r.LsClient.ClientId={1} and r.Month.PeriodId={2} and s.Root={3} and r.Service=s and r.Code=4 ", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) this.month.PeriodId, (object) this.service.ServiceId)).List<Rent>();
            }
            else
            {
              IList<Rent> rentList = (IList<Rent>) new List<Rent>();
              this.dgvDetail.DataSource = (object) this.session.CreateQuery(string.Format("select r from Rent r, Service s where r.Period.PeriodId={0} and r.LsClient.ClientId={1} and r.Month.PeriodId={2} and s.Root={3} and r.Code={4} and r.Service=s ", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) this.month.PeriodId, (object) this.service.ServiceId, (object) this.code)).List<Rent>();
            }
          }
          else
          {
            IList<RentMSP> rentMspList = (IList<RentMSP>) new List<RentMSP>();
            this.dgvDetail.DataSource = (object) this.session.CreateQuery(string.Format("select r from RentMSP r, Service s where r.Period.PeriodId={0} and r.LsClient.ClientId={1} and r.Month.PeriodId={2} and s.Root={3} and r.Code={6} and r.Service=s and r.Person.PersonId={4} and r.MSP.MSP_id={5}", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) this.month.PeriodId, (object) this.service.ServiceId, (object) this.person.PersonId, (object) this.msp.MSP_id, (object) this.code)).List<RentMSP>();
          }
        }
        DataGridViewTextBoxColumn viewTextBoxColumn1 = new DataGridViewTextBoxColumn();
        viewTextBoxColumn1.Width = 200;
        viewTextBoxColumn1.HeaderText = "Составляющая";
        viewTextBoxColumn1.Name = "Service";
        viewTextBoxColumn1.ReadOnly = true;
        this.dgvDetail.Columns.Insert(0, (DataGridViewColumn) viewTextBoxColumn1);
        DataGridViewComboBoxColumn viewComboBoxColumn1 = new DataGridViewComboBoxColumn();
        DataGridViewComboBoxColumn viewComboBoxColumn2 = new DataGridViewComboBoxColumn();
        viewComboBoxColumn2.DropDownWidth = 200;
        viewComboBoxColumn2.Width = 200;
        viewComboBoxColumn2.MaxDropDownItems = 7;
        viewComboBoxColumn2.DataSource = (object) baseOrgList1;
        viewComboBoxColumn2.ValueMember = "BaseOrgId";
        viewComboBoxColumn2.DisplayMember = "NameOrgMin";
        viewComboBoxColumn2.HeaderText = "Получатель";
        viewComboBoxColumn2.Name = "Recipient";
        this.dgvDetail.Columns.Insert(1, (DataGridViewColumn) viewComboBoxColumn2);
        KvrplHelper.AddComboBoxColumn(this.dgvDetail, 2, (IList) null, "BaseOrgId", "BaseOrgName", "Исполнитель", "Perfomer", 200, 200);
        DataGridViewTextBoxColumn viewTextBoxColumn2 = new DataGridViewTextBoxColumn();
        viewTextBoxColumn2.Width = 80;
        viewTextBoxColumn2.HeaderText = "Сумма";
        viewTextBoxColumn2.Name = "Rent";
        this.dgvDetail.Columns.Insert(3, (DataGridViewColumn) viewTextBoxColumn2);
        if (!this.handMadeMSP && (int) this.code == 4)
        {
          KvrplHelper.AddTextBoxColumn(this.dgvDetail, 4, "Сумма по ЭО тарифу", "RentEO", 80, false);
          KvrplHelper.AddTextBoxColumn(this.dgvDetail, 5, "Объём", "Volume", 80, false);
        }
        else
          KvrplHelper.AddTextBoxColumn(this.dgvDetail, 4, "Объём", "Volume", 80, false);
        Decimal num2 = new Decimal();
        foreach (DataGridViewRow row in (IEnumerable) this.dgvDetail.Rows)
        {
          if (!this.handMadeMSP)
          {
            if ((int) this.code == 4)
            {
              row.Cells["Rent"].Value = (object) ((CorrectRent) row.DataBoundItem).RentMain;
              row.Cells["RentEO"].Value = (object) ((CorrectRent) row.DataBoundItem).RentEO;
              row.Cells["Volume"].Value = (object) ((CorrectRent) row.DataBoundItem).Volume;
              row.Cells["Service"].Value = (object) ((CorrectRent) row.DataBoundItem).Service.ServiceName;
              if (!this.insertRecord)
              {
                row.Cells["Recipient"].Value = (object) ((CorrectRent) row.DataBoundItem).Supplier.Recipient.BaseOrgId;
                row.Cells["Perfomer"].Value = (object) ((CorrectRent) row.DataBoundItem).Supplier.Perfomer.BaseOrgId;
              }
            }
            else if ((int) this.code == 4)
            {
              row.Cells["Rent"].Value = (object) ((Rent) row.DataBoundItem).RentMain;
              row.Cells["Volume"].Value = (object) ((Rent) row.DataBoundItem).Volume;
              row.Cells["Service"].Value = (object) ((Rent) row.DataBoundItem).Service.ServiceName;
              if (!this.insertRecord)
              {
                row.Cells["Recipient"].Value = (object) ((Rent) row.DataBoundItem).Supplier.Recipient.BaseOrgId;
                row.Cells["Perfomer"].Value = (object) ((Rent) row.DataBoundItem).Supplier.Perfomer.BaseOrgId;
              }
            }
            else
            {
              row.Cells["Rent"].Value = (object) ((Rent) row.DataBoundItem).RentMain;
              row.Cells["Volume"].Value = (object) ((Rent) row.DataBoundItem).Volume;
              row.Cells["Service"].Value = (object) ((Rent) row.DataBoundItem).Service.ServiceName;
              if (!this.insertRecord)
              {
                row.Cells["Recipient"].Value = (object) ((Rent) row.DataBoundItem).Supplier.Recipient.BaseOrgId;
                row.Cells["Perfomer"].Value = (object) ((Rent) row.DataBoundItem).Supplier.Perfomer.BaseOrgId;
              }
            }
          }
          else
          {
            row.Cells["Rent"].Value = (object) ((RentMSP) row.DataBoundItem).RentMain;
            row.Cells["Volume"].Value = (object) ((RentMSP) row.DataBoundItem).Volume;
            row.Cells["Service"].Value = (object) ((RentMSP) row.DataBoundItem).Service.ServiceName;
            if (!this.insertRecord)
            {
              row.Cells["Recipient"].Value = (object) ((RentMSP) row.DataBoundItem).Supplier.Recipient.BaseOrgId;
              row.Cells["Perfomer"].Value = (object) ((RentMSP) row.DataBoundItem).Supplier.Perfomer.BaseOrgId;
            }
          }
          if (!this.handMadeMSP && (int) this.code == 4 && ((CorrectRent) row.DataBoundItem).Supplier != null || !this.handMadeMSP && (int) this.code != 4 && ((Rent) row.DataBoundItem).Supplier != null || this.handMadeMSP && ((RentMSP) row.DataBoundItem).Supplier != null)
          {
            int baseOrgId1;
            int baseOrgId2;
            if (!this.handMadeMSP)
            {
              if ((int) this.code == 4)
              {
                baseOrgId1 = ((CorrectRent) row.DataBoundItem).Supplier.Recipient.BaseOrgId;
                baseOrgId2 = ((CorrectRent) row.DataBoundItem).Supplier.Perfomer.BaseOrgId;
              }
              else
              {
                baseOrgId1 = ((Rent) row.DataBoundItem).Supplier.Recipient.BaseOrgId;
                baseOrgId2 = ((Rent) row.DataBoundItem).Supplier.Perfomer.BaseOrgId;
              }
            }
            else
            {
              baseOrgId1 = ((RentMSP) row.DataBoundItem).Supplier.Recipient.BaseOrgId;
              baseOrgId2 = ((RentMSP) row.DataBoundItem).Supplier.Perfomer.BaseOrgId;
            }
            ISession session1 = this.session;
            string format1 = "select distinct d.Perfomer from CmpSupplier s, Supplier d where s.SupplierOrg.SupplierId = d.SupplierId and s.Service.ServiceId={0} and d.Recipient.BaseOrgId={4} and d.Perfomer.BaseOrgId<>0  and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={1} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{3}' and Param.ParamId=211)";
            object[] objArray1 = new object[5];
            objArray1[0] = (object) this.service.ServiceId;
            objArray1[1] = (object) this.client.Company.CompanyId;
            int index1 = 2;
            DateTime? periodName1 = Options.Period.PeriodName;
            string baseFormat1 = KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(periodName1.Value));
            objArray1[index1] = (object) baseFormat1;
            int index2 = 3;
            periodName1 = Options.Period.PeriodName;
            string baseFormat2 = KvrplHelper.DateToBaseFormat(periodName1.Value);
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
              objArray2[0] = (object) this.service.ServiceId;
              objArray2[1] = (object) this.client.Company.CompanyId;
              int index4 = 2;
              DateTime? periodName2 = Options.Period.PeriodName;
              string baseFormat3 = KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(periodName2.Value));
              objArray2[index4] = (object) baseFormat3;
              int index5 = 3;
              periodName2 = Options.Period.PeriodName;
              string baseFormat4 = KvrplHelper.DateToBaseFormat(periodName2.Value);
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
            object[] objArray = new object[4]{ (object) this.service.ServiceId, (object) this.client.Company.CompanyId, null, null };
            int index1 = 2;
            DateTime? periodName = Options.Period.PeriodName;
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
          if (row.Cells["Rent"].Value != null)
            num2 += Convert.ToDecimal(KvrplHelper.ChangeSeparator(row.Cells["Rent"].Value.ToString()));
        }
        this.lblItogo.Text = "Итого: " + num2.ToString();
        this.session.Clear();
      }
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      this.summa = 0.0;
      this.summaEO = 0.0;
      this.volume = 0.0;
      ITransaction transaction = this.session.BeginTransaction();
      foreach (DataGridViewRow row in (IEnumerable) this.dgvDetail.Rows)
      {
        if (Options.Period.PeriodName.Value <= KvrplHelper.GetKvrClose(this.client.ClientId, Options.ComplexPasp, Options.ComplexPrior).PeriodName.Value)
        {
          int num = (int) MessageBox.Show("Невозможно внести изменения в закрытом месяце", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.FrmHandMadeDetail_Shown((object) null, (EventArgs) null);
          return;
        }
        if (this.handMadeMSP)
        {
          RentMSP dataBoundItem = (RentMSP) row.DataBoundItem;
          try
          {
            dataBoundItem.RentMain = Convert.ToDouble(KvrplHelper.ChangeSeparator(row.Cells["Rent"].Value.ToString()));
            dataBoundItem.Volume = Convert.ToDouble(KvrplHelper.ChangeSeparator(row.Cells["Volume"].Value.ToString()));
            this.volume = this.volume + dataBoundItem.Volume;
            this.summa = this.summa + dataBoundItem.RentMain;
          }
          catch (Exception ex)
          {
            KvrplHelper.WriteLog(ex, this.client);
          }
          try
          {
            if (this.insertRecord)
            {
              dataBoundItem.Supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(row.Cells["Recipient"].Value), (object) Convert.ToInt32(row.Cells["Perfomer"].Value))).List<Supplier>()[0];
              if ((dataBoundItem.RentMain != 0.0 || dataBoundItem.Volume != 0.0) && dataBoundItem.Supplier.SupplierId == 0 && MessageBox.Show("Вы уверены, что хотите сохранить сумму с невыбранными исполнителем и получателем?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;
              this.session.Save((object) dataBoundItem);
              this.session.Flush();
            }
            else
            {
              Supplier supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(row.Cells["Recipient"].Value), (object) Convert.ToInt32(row.Cells["Perfomer"].Value))).List<Supplier>()[0];
              if ((dataBoundItem.RentMain != 0.0 || dataBoundItem.Volume != 0.0) && supplier.SupplierId == 0 && MessageBox.Show("Вы уверены, что хотите сохранить сумму с невыбранными исполнителем и получателем?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;
              this.session.CreateQuery(string.Format("update RentMSP set RentMain=:rent,Supplier.SupplierId=:supplier,Volume=:volume,RentType=:type where Period.PeriodId={0} and LsClient.ClientId={1} and Service.ServiceId={2} and Supplier.SupplierId={3} and Month.PeriodId={4} and Person.PersonId={5} and MSP.MSP_id={6} and Code={7} and RentType=:oldtype", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) dataBoundItem.Service.ServiceId, (object) dataBoundItem.Supplier.SupplierId, (object) dataBoundItem.Month.PeriodId, (object) dataBoundItem.Person.PersonId, (object) dataBoundItem.MSP.MSP_id, (object) this.code)).SetParameter<double>("rent", dataBoundItem.RentMain).SetParameter<int>("supplier", supplier.SupplierId).SetParameter<double>("volume", dataBoundItem.Volume).SetParameter<short>("type", this.rentType).SetParameter<short>("oldtype", this.oldType).ExecuteUpdate();
            }
          }
          catch (Exception ex)
          {
            KvrplHelper.WriteLog(ex, this.client);
          }
        }
        else if ((int) this.code == 4)
        {
          CorrectRent dataBoundItem = (CorrectRent) row.DataBoundItem;
          try
          {
            dataBoundItem.RentMain = row.Cells["Rent"].Value == null ? 0.0 : Convert.ToDouble(KvrplHelper.ChangeSeparator(row.Cells["Rent"].Value.ToString()));
            dataBoundItem.RentEO = row.Cells["RentEO"].Value == null ? 0.0 : Convert.ToDouble(KvrplHelper.ChangeSeparator(row.Cells["RentEO"].Value.ToString()));
            dataBoundItem.Volume = row.Cells["Volume"].Value == null ? 0.0 : Convert.ToDouble(KvrplHelper.ChangeSeparator(row.Cells["Volume"].Value.ToString()));
            dataBoundItem.UName = Options.Login;
            dataBoundItem.DEdit = new DateTime?(DateTime.Now.Date);
            this.volume = this.volume + dataBoundItem.Volume;
            this.summa = this.summa + dataBoundItem.RentMain;
            this.summaEO = this.summaEO + dataBoundItem.RentEO;
          }
          catch (Exception ex)
          {
            KvrplHelper.WriteLog(ex, this.client);
          }
          try
          {
            if (this.insertRecord)
            {
              dataBoundItem.Supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(row.Cells["Recipient"].Value), (object) Convert.ToInt32(row.Cells["Perfomer"].Value))).List<Supplier>()[0];
              if ((dataBoundItem.RentMain != 0.0 || dataBoundItem.RentEO != 0.0 || dataBoundItem.Volume != 0.0) && dataBoundItem.Supplier.SupplierId == 0 && MessageBox.Show("Вы уверены, что хотите сохранить сумму с невыбранными исполнителем и получателем?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;
              if (dataBoundItem.RentMain != 0.0)
              {
                if (dataBoundItem.RentMain != 0.0 && dataBoundItem.RentEO == 0.0)
                {
                  if (dataBoundItem.RentEO == 0.0)
                  {
                    if (MessageBox.Show("Сумма по ЭО тарифу равной 0. Продолжить?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                      continue;
                  }
                  if (dataBoundItem.RentMain != 0.0 && dataBoundItem.RentEO == 0.0 && MessageBox.Show("Взять сумму по ЭО тарифу равной сумме корректировки?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    dataBoundItem.RentEO = dataBoundItem.RentMain;
                }
                this.session.Save((object) dataBoundItem);
                this.session.Flush();
              }
            }
            else
            {
              Supplier supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(row.Cells["Recipient"].Value), (object) Convert.ToInt32(row.Cells["Perfomer"].Value))).List<Supplier>()[0];
              if ((dataBoundItem.RentMain != 0.0 || dataBoundItem.RentEO != 0.0 || dataBoundItem.Volume != 0.0) && supplier.SupplierId == 0 && MessageBox.Show("Вы уверены, что хотите сохранить сумму с невыбранными исполнителем и получателем?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;
              if (dataBoundItem.RentMain == 0.0 || dataBoundItem.RentEO == 0.0)
              {
                this.session.Delete((object) dataBoundItem);
                this.session.Flush();
              }
              else
                this.session.CreateQuery(string.Format("update CorrectRent set RentMain=:rent,RentEO=:renteo,Supplier.SupplierId=:supplier,Volume=:volume, UName='{6}',DEdit='{7}', RentType=:type where Period.PeriodId={0} and LsClient.ClientId={1} and Service.ServiceId={2} and Supplier.SupplierId={3} and Month.PeriodId={4} and Note='{5}' and RentType=:oldtype ", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) dataBoundItem.Service.ServiceId, (object) dataBoundItem.Supplier.SupplierId, (object) dataBoundItem.Month.PeriodId, (object) dataBoundItem.Note, (object) dataBoundItem.UName, (object) KvrplHelper.DateToBaseFormat(dataBoundItem.DEdit.Value))).SetParameter<double>("rent", dataBoundItem.RentMain).SetParameter<double>("renteo", dataBoundItem.RentEO).SetParameter<int>("supplier", supplier.SupplierId).SetParameter<double>("volume", dataBoundItem.Volume).SetParameter<short>("type", this.rentType).SetParameter<short>("oldtype", this.oldType).ExecuteUpdate();
            }
          }
          catch (Exception ex)
          {
            KvrplHelper.WriteLog(ex, this.client);
          }
        }
        else
        {
          Rent dataBoundItem = (Rent) row.DataBoundItem;
          try
          {
            dataBoundItem.RentMain = Convert.ToDouble(KvrplHelper.ChangeSeparator(row.Cells["Rent"].Value.ToString()));
            dataBoundItem.Volume = Convert.ToDouble(KvrplHelper.ChangeSeparator(row.Cells["Volume"].Value.ToString()));
          }
          catch (Exception ex)
          {
            KvrplHelper.WriteLog(ex, this.client);
          }
          try
          {
            if (this.insertRecord)
            {
              dataBoundItem.Supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(row.Cells["Recipient"].Value), (object) Convert.ToInt32(row.Cells["Perfomer"].Value))).List<Supplier>()[0];
              if ((dataBoundItem.RentMain != 0.0 || dataBoundItem.RentEO != 0.0 || dataBoundItem.Volume != 0.0) && dataBoundItem.Supplier.SupplierId == 0 && MessageBox.Show("Вы уверены, что хотите сохранить сумму с невыбранными исполнителем и получателем?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;
              this.session.Save((object) dataBoundItem);
              this.session.Flush();
            }
            else
            {
              Supplier supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(row.Cells["Recipient"].Value), (object) Convert.ToInt32(row.Cells["Perfomer"].Value))).List<Supplier>()[0];
              if ((dataBoundItem.RentMain != 0.0 || dataBoundItem.RentEO != 0.0 || dataBoundItem.Volume != 0.0) && supplier.SupplierId == 0 && MessageBox.Show("Вы уверены, что хотите сохранить сумму с невыбранными исполнителем и получателем?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;
              this.session.CreateQuery(string.Format("update Rent set RentMain=:rent,Supplier.SupplierId=:supplier,Volume=:volume,RentType=:type where Period.PeriodId={0} and LsClient.ClientId={1} and Service.ServiceId={2} and Supplier.SupplierId={3} and Month.PeriodId={4} and Code={5} and RentType=:oldtype", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) dataBoundItem.Service.ServiceId, (object) dataBoundItem.Supplier.SupplierId, (object) dataBoundItem.Month.PeriodId, (object) this.code)).SetParameter<double>("rent", dataBoundItem.RentMain).SetParameter<int>("supplier", supplier.SupplierId).SetParameter<double>("volume", dataBoundItem.Volume).SetParameter<short>("type", this.rentType).SetParameter<short>("oldtype", this.oldType).ExecuteUpdate();
            }
          }
          catch (Exception ex)
          {
            KvrplHelper.WriteLog(ex, this.client);
          }
        }
      }
      try
      {
        transaction.Commit();
        if (!this.handMadeMSP)
        {
          if ((int) this.code == 4)
          {
            CorrectRent rent = new CorrectRent();
            rent.LsClient = this.client;
            rent.Note = this.argument;
            rent.Service = this.service;
            rent.Period = Options.Period;
            rent.Month = this.month;
            rent.Volume = this.volume;
            rent.RentMain = this.summa;
            rent.RentEO = this.summaEO;
            rent.RentType = this.rentType;
            if (Convert.ToInt32(KvrplHelper.BaseValue(32, this.client.Company)) == 1)
            {
              if (this.insertRecord)
                KvrplHelper.SaveCorrectRentToNoteBook(rent, this.client, (short) 1, KvrplHelper.GetKvrClose(this.client.ClientId, Options.ComplexPasp, Options.ComplexPrior).PeriodName.Value);
              else
                KvrplHelper.ChangeCorrectRentToNoteBook(rent, this.old, this.client, (short) 2, KvrplHelper.GetKvrClose(this.client.ClientId, Options.ComplexPasp, Options.ComplexPrior).PeriodName.Value);
            }
          }
        }
        else
        {
          RentMSP rent = new RentMSP();
          rent.LsClient = this.client;
          rent.Person = this.person;
          rent.MSP = this.msp;
          rent.Service = this.service;
          rent.Period = Options.Period;
          rent.Month = this.month;
          rent.Volume = this.volume;
          rent.RentMain = this.summa;
          rent.RentType = this.rentType;
          if (Convert.ToInt32(KvrplHelper.BaseValue(32, this.client.Company)) == 1)
          {
            if (this.insertRecord)
              KvrplHelper.SaveRentMSPToNoteBook(rent, this.client, (short) 1, KvrplHelper.GetKvrClose(this.client.ClientId, Options.ComplexPasp, Options.ComplexPrior).PeriodName.Value);
            else
              KvrplHelper.ChangeRentMSPToNoteBook(rent, this.oldMSP, this.client, (short) 2, KvrplHelper.GetKvrClose(this.client.ClientId, Options.ComplexPasp, Options.ComplexPrior).PeriodName.Value);
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Не удалось сохранить изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      this.session.Clear();
      this.Close();
    }

    private void dgvDetail_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    private void dgvDetail_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (!this.dgvDetail.IsCurrentCellDirty)
        return;
      this.dgvDetail.CommitEdit(DataGridViewDataErrorContexts.Commit);
      if (this.dgvDetail.CurrentCell.ColumnIndex == this.dgvDetail.Rows[this.dgvDetail.CurrentRow.Index].Cells["Recipient"].ColumnIndex)
      {
        this.session = Domain.CurrentSession;
        IList<BaseOrg> baseOrgList = this.session.CreateQuery(string.Format("select distinct d.Perfomer from CmpSupplier s, Supplier d where s.SupplierOrg.SupplierId = d.SupplierId and s.Service.ServiceId={0} and d.Perfomer.BaseOrgId<>0 and d.Recipient.BaseOrgId={4}   and s.Company.CompanyId=(select ParamValue from CompanyParam where Company.CompanyId={1} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{3}' and Param.ParamId=211) order by d.Perfomer.NameOrgMin", (object) this.service.ServiceId, (object) this.client.Company.CompanyId, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(Options.Period.PeriodName.Value)), (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), (object) Convert.ToInt32(this.dgvDetail.CurrentRow.Cells["Recipient"].Value))).List<BaseOrg>();
        if (Convert.ToInt32(this.dgvDetail.CurrentRow.Cells["Recipient"].Value) == 0)
          baseOrgList.Insert(0, new BaseOrg(0, ""));
        this.session.Clear();
        this.dgvDetail.CurrentRow.Cells["Perfomer"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "BaseOrgId",
          DisplayMember = "NameOrgMinDop",
          DataSource = (object) baseOrgList
        };
        this.dgvDetail.CurrentRow.Cells["Perfomer"].Value = (object) baseOrgList[0].BaseOrgId;
      }
    }

    private void dgvDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgvDetail.CurrentCell != this.dgvDetail.CurrentRow.Cells["Rent"])
        return;
      Decimal num = new Decimal();
      foreach (DataGridViewRow row in (IEnumerable) this.dgvDetail.Rows)
      {
        if (row.Cells["Rent"].Value != null)
          num += Convert.ToDecimal(KvrplHelper.ChangeSeparator(row.Cells["Rent"].Value.ToString()));
      }
      this.lblItogo.Text = "Итого: " + num.ToString();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmHandMadeDetail));
      this.pnBtn = new Panel();
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.dgvDetail = new DataGridView();
      this.pnUp = new Panel();
      this.lblItogo = new Label();
      this.pnBtn.SuspendLayout();
      ((ISupportInitialize) this.dgvDetail).BeginInit();
      this.pnUp.SuspendLayout();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnOk);
      this.pnBtn.Controls.Add((Control) this.btnCancel);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 282);
      this.pnBtn.Margin = new Padding(4);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(744, 40);
      this.pnBtn.TabIndex = 0;
      this.btnOk.Image = (Image) Resources.Tick;
      this.btnOk.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnOk.Location = new Point(12, 5);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(62, 30);
      this.btnOk.TabIndex = 1;
      this.btnOk.Text = "ОК";
      this.btnOk.TextAlign = ContentAlignment.MiddleRight;
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Image = (Image) Resources.delete;
      this.btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCancel.Location = new Point(645, 5);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(88, 30);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "Отмена";
      this.btnCancel.TextAlign = ContentAlignment.MiddleRight;
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnExit_Click);
      this.dgvDetail.BackgroundColor = Color.AliceBlue;
      this.dgvDetail.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvDetail.Dock = DockStyle.Fill;
      this.dgvDetail.Location = new Point(0, 32);
      this.dgvDetail.Name = "dgvDetail";
      this.dgvDetail.Size = new Size(744, 250);
      this.dgvDetail.TabIndex = 1;
      this.dgvDetail.CellEndEdit += new DataGridViewCellEventHandler(this.dgvDetail_CellEndEdit);
      this.dgvDetail.CurrentCellDirtyStateChanged += new EventHandler(this.dgvDetail_CurrentCellDirtyStateChanged);
      this.dgvDetail.DataError += new DataGridViewDataErrorEventHandler(this.dgvDetail_DataError);
      this.pnUp.Controls.Add((Control) this.lblItogo);
      this.pnUp.Dock = DockStyle.Top;
      this.pnUp.Location = new Point(0, 0);
      this.pnUp.Name = "pnUp";
      this.pnUp.Size = new Size(744, 32);
      this.pnUp.TabIndex = 2;
      this.lblItogo.AutoSize = true;
      this.lblItogo.Location = new Point(9, 9);
      this.lblItogo.Name = "lblItogo";
      this.lblItogo.Size = new Size(0, 16);
      this.lblItogo.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(744, 322);
      this.Controls.Add((Control) this.dgvDetail);
      this.Controls.Add((Control) this.pnUp);
      this.Controls.Add((Control) this.pnBtn);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmHandMadeDetail";
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Разбивка по составляющим";
      this.Shown += new EventHandler(this.FrmHandMadeDetail_Shown);
      this.pnBtn.ResumeLayout(false);
      ((ISupportInitialize) this.dgvDetail).EndInit();
      this.pnUp.ResumeLayout(false);
      this.pnUp.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
