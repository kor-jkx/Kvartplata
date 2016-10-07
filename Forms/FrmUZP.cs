// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmUZP
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using Kvartplata.StaticResourse;
using NHibernate;
using SaveSettings;
using sbsit.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmUZP : FrmBaseForm
  {
    private FormStateSaver fss = new FormStateSaver(FrmUZP.ic);
    protected GridSettings MySettingsRegister = new GridSettings();
    private IList<Register> _registersList = (IList<Register>) new List<Register>();
    private IContainer components = (IContainer) null;
    private ISession session;
    private static IContainer ic;
    private Period monthClosed;
    private IList<DataGridViewCell> findCell;
    private int findInd;
    private int city;
    private Panel pnBtn;
    private Button btnExit;
    private Panel pnUp;
    private Label lblManager;
    private Label lblOrg;
    private MonthPicker mpCurrentPeriod;
    private ComboBox cmbContract;
    private ComboBox cmbGuild;
    private ComboBox cmbOrg;
    private ComboBox cmbManager;
    private Label lblContract;
    private Label lblGuild;
    private DataGridView dgvRegister;
    private Button btnMake;
    private Button btnLoadPay;
    private Button btnSave;
    private Button btnUnLoad;
    private SaveFileDialog sfdRegister;
    private ContextMenuStrip cmsUnLoad;
    private ToolStripMenuItem tsmiUnloadDBF;
    private ToolStripMenuItem mi2CSV;
    private Button btnControl;
    private SaveFileDialog sfd;
    private ToolStripMenuItem tsmiTab;
    private ToolStripMenuItem miLS;
    private Button btnReport;
    private ComboBox cbReceiptType;
    private Label lbReceiptType;

    private IList<Register> RegistersList
    {
      get
      {
        return this._registersList;
      }
      set
      {
        this._registersList = value;
      }
    }

    public short Receipt_id { get; set; }

    public FrmUZP()
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
      this.session = Domain.CurrentSession;
    }

    private void FrmUZP_Load(object sender, EventArgs e)
    {
      this.city = Convert.ToInt32(KvrplHelper.BaseValue(1, Options.Company));
      if (this.city == 0)
        this.city = Options.City;
      try
      {
        this.MySettingsRegister.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
        this.LoadCmbManager();
        this.LoadCmbReceiptType();
        try
        {
          this.monthClosed = this.session.CreateQuery(string.Format("select c.Period from CompanyPeriod c where c.Period.PeriodId=(select min(Period.PeriodId) from CompanyPeriod where Company.CompanyId in (select CompanyId from Company where Manager.BaseOrgId={0})) ", (object) ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId)).List<Period>()[0];
        }
        catch
        {
          this.monthClosed = this.session.CreateQuery(string.Format("select c.Period from CompanyPeriod c where c.Period.PeriodId=(select min(Period.PeriodId) from CompanyPeriod) ")).List<Period>()[0];
        }
        this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
        this.LoadCmbOrg();
        this.LoadUZP();
      }
      catch
      {
      }
    }

    private void FrmUZP_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Modifiers == Keys.Control && e.KeyValue == 70)
      {
        string str = "";
        this.findInd = 0;
        this.findCell = (IList<DataGridViewCell>) new List<DataGridViewCell>();
        InputBox.InputValue("Поиск", "Введите значение", "", "", ref str);
        foreach (DataGridViewRow row in (IEnumerable) this.dgvRegister.Rows)
        {
          foreach (DataGridViewCell cell in (BaseCollection) row.Cells)
          {
            if ((cell.ColumnIndex == 0 || cell.ColumnIndex >= 10 && cell.ColumnIndex <= 13 || cell.ColumnIndex == 5) && cell.Value.ToString().ToLower().IndexOf(str.ToLower()) != -1)
              this.findCell.Add(cell);
          }
        }
        if (this.findCell.Count == 0)
        {
          int num = (int) MessageBox.Show("Значения не найдены");
        }
        else
        {
          if (this.dgvRegister.CurrentCell != null)
          {
            while (this.findInd <= this.findCell.Count - 1 && this.findCell[this.findInd].RowIndex < this.dgvRegister.CurrentCell.RowIndex)
              this.findInd = this.findInd + 1;
            if (this.findInd > this.findCell.Count - 1)
              this.findInd = 0;
          }
          this.dgvRegister.CurrentCell = this.findCell[this.findInd];
        }
      }
      if (e.KeyValue != 114)
        return;
      if (this.dgvRegister.CurrentCell != null && this.findCell != null)
      {
        while (this.findInd <= this.findCell.Count - 1 && this.findCell[this.findInd].RowIndex <= this.dgvRegister.CurrentCell.RowIndex && this.findCell[this.findInd].ColumnIndex <= this.dgvRegister.CurrentCell.ColumnIndex)
          this.findInd = this.findInd + 1;
      }
      if (this.findInd > this.findCell.Count - 1)
        this.findInd = 0;
      this.dgvRegister.CurrentCell = this.findCell[this.findInd];
    }

    private void cmbOrg_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.LoadCmbGuild();
      this.LoadCmbContract();
      this.LoadUZP();
    }

    private void cmbManager_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.LoadCmbOrg();
      this.LoadUZP();
      try
      {
        this.monthClosed = this.session.CreateQuery(string.Format("select c.Period from CompanyPeriod c where c.Period.PeriodId=(select min(Period.PeriodId) from CompanyPeriod where Company.CompanyId in (select CompanyId from Company where Manager.BaseOrgId={0})) ", (object) ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId)).List<Period>()[0];
      }
      catch
      {
        this.monthClosed = this.session.CreateQuery(string.Format("select c.Period from CompanyPeriod c where c.Period.PeriodId=(select min(Period.PeriodId) from CompanyPeriod) ")).List<Period>()[0];
      }
      this.EnabledButtons();
    }

    private void cmbGuild_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.LoadUZP();
    }

    private void cmbContract_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.LoadUZP();
    }

    private void btnMake_Click(object sender, EventArgs e)
    {
      short int16 = Convert.ToInt16(KvrplHelper.BaseValue(37, Options.Company));
      this.Cursor = Cursors.WaitCursor;
      if ((int) ((Receipt) this.cbReceiptType.SelectedItem).ReceiptId == 0)
      {
        int num1 = (int) MessageBox.Show("Не выбран тип квитанции", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        string str1 = " and root in (select service_id from DBA.cmpServiceParam where company_id={0} and complex_id={1} and receipt_id=";
        short receiptId = ((Receipt) this.cbReceiptType.SelectedItem).ReceiptId;
        string str2 = receiptId.ToString();
        string str3 = ") ";
        string str4 = string.Format(str1 + str2 + str3, (object) Options.Company.CompanyId, (object) 100);
        if (this.session.CreateQuery(string.Format("select r from Register r where r.Period.PeriodId={0} and r.Manager.BaseOrgId={1} " + this.GetUsl("and r.Contract.BaseOrg.BaseOrgId={0}", " and r.Guild.GuildId={0}", "and r.Contract.ContractId={0}") + " and r.PaymentId <> 0", (object) Options.Period.PeriodId, (object) ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId)).List().Count > 0)
        {
          int num2 = (int) MessageBox.Show("Невозможно изменить реестр. Платежи закачены.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          if (MessageBox.Show("Вы действительно хотите сформировать реестр?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            return;
          try
          {
            ISession session1 = this.session;
            string format = "delete from lsRegister where period_id={0} and manager_id={1} and receipt_id={2}" + this.GetUsl(" and contract_id in (select contract_id from mngContract where idbaseorg={0})", " and guild_id={0}", " and contract_id={0}");
            // ISSUE: variable of a boxed type
            int periodId = Options.Period.PeriodId;
            // ISSUE: variable of a boxed type
            int baseOrgId = ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId;
            receiptId = ((Receipt) this.cbReceiptType.SelectedItem).ReceiptId;
            string str5 = receiptId.ToString();
            string queryString1 = string.Format(format, (object) periodId, (object) baseOrgId, (object) str5);
            session1.CreateSQLQuery(queryString1).ExecuteUpdate();
            switch (int16)
            {
              case 0:
                ISession session2 = this.session;
                string[] strArray1 = new string[13];
                strArray1[0] = "insert into lsRegister (period_id, manager_id, contract_id, client_id, idform, guild_id, tab_num, rent, rentpeni, deduction, deductionpeni, dept, deptpeni, payment_id, receipt_id)select fperiod, manager_id, contract_id, client_id, idform, guild_id, tabnum, rent, penirent, rent, penirent, debt, penidebt, 0, ";
                int index1 = 1;
                receiptId = ((Receipt) this.cbReceiptType.SelectedItem).ReceiptId;
                string str6 = receiptId.ToString();
                strArray1[index1] = str6;
                int index2 = 2;
                string str7 = " from(    select :periodid as fperiod, c.manager_id, b.contract_id, b.client_id, b.idform,        (select (if deesp=1 then null else deesp endif) from form_a where idform=b.idform) as guild_id,        isnull(cast((select percentsharestring from form_a where idform=b.idform) as char(22)),'') as tabnum,       isnull((select sum(rent)-sum(msp) from lsBalance ba                                           inner join dcService s on s.Service_id=ba.Service_id                                           where period_id=fperiod and ba.client_id=b.client_id ";
                strArray1[index2] = str7;
                int index3 = 3;
                string str8 = str4;
                strArray1[index3] = str8;
                int index4 = 4;
                string str9 = " ),0)+            isnull((select sum(i.dept) from lsInstalment i, lsAgreement a where i.agreement_id=a.agreement_id and i.period_id=fperiod and client_id=b.client_id),0) as rent1,        isnull((select sum(rent) from lsBalancePeni bp                                        inner join dcService s on s.Service_id=bp.Service_id                                            where period_id=fperiod and bp.client_id=b.client_id  ";
                strArray1[index4] = str9;
                int index5 = 5;
                string str10 = str4;
                strArray1[index5] = str10;
                int index6 = 6;
                string str11 = " ),0) +            isnull((select sum(i.deptpeni) from lsInstalment i,lsAgreement a where i.agreement_id=a.agreement_id and i.period_id=fperiod and client_id=b.client_id),0) as penirent1,       isnull((select sum(balance_in) from lsBalance ba                                           inner join dcService s on s.Service_id=ba.Service_id                                           where period_id=fperiod and ba.client_id=b.client_id ";
                strArray1[index6] = str11;
                int index7 = 7;
                string str12 = str4;
                strArray1[index7] = str12;
                int index8 = 8;
                string str13 = " ),0) as debt,        isnull((select sum(balance_in) from lsBalancePeni bp                                        inner join dcService s on s.Service_id=bp.Service_id                                            where period_id=fperiod and bp.client_id=b.client_id  ";
                strArray1[index8] = str13;
                int index9 = 9;
                string str14 = str4;
                strArray1[index9] = str14;
                int index10 = 10;
                string str15 = " ),0) as penidebt,        (if rent1>0 then (if debt>0 then rent1 else (if rent1+debt>0 then rent1+debt else 0 endif) endif) else 0 endif) as rent,       (if penirent1>0 then (if penidebt>0 then penirent1 else (if penirent1+penidebt>0 then penirent1+penidebt else 0 endif)endif) else 0 endif) as penirent    from lsBond b,mngContract c    where c.contract_id=b.contract_id and c.manager_id=:manager ";
                strArray1[index10] = str15;
                int index11 = 11;
                string usl1 = this.GetUsl(" and c.idbaseorg={0}", " and guild_id={0}", " and c.contract_id={0}");
                strArray1[index11] = usl1;
                int index12 = 12;
                string str16 = "   and c.dbeg<=:period and c.dend>=:period and b.dbeg<=:period and b.dend>=:period) as my";
                strArray1[index12] = str16;
                string queryString2 = string.Concat(strArray1);
                session2.CreateSQLQuery(queryString2).SetParameter<int>("periodid", Options.Period.PeriodId).SetParameter<int>("manager", ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId).SetParameter<string>("period", KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value)).ExecuteUpdate();
                break;
              case 1:
                ISession session3 = this.session;
                string[] strArray2 = new string[15];
                strArray2[0] = "insert into lsRegister (period_id, manager_id, contract_id, client_id, idform, guild_id, tab_num, rent, rentpeni, deduction, deductionpeni, dept, deptpeni, payment_id, receipt_id)select fperiod, manager_id, contract_id, client_id, idform, guild_id, tabnum, rent, penirent, rent, penirent, dept, penidept, 0, ";
                int index13 = 1;
                receiptId = ((Receipt) this.cbReceiptType.SelectedItem).ReceiptId;
                string str17 = receiptId.ToString();
                strArray2[index13] = str17;
                int index14 = 2;
                string str18 = " from(    select :periodid as fperiod, c.manager_id, b.contract_id, b.client_id, b.idform,        (select (if deesp=1 then null else deesp endif) from form_a where idform=b.idform) as guild_id,        isnull(cast((select percentsharestring from form_a where idform=b.idform) as char(22)),'') as tabnum,       isnull((select sum(rent)-sum(msp) from lsBalance ba                                           inner join dcService s on s.Service_id=ba.Service_id                                           where period_id=fperiod and ba.client_id=b.client_id ";
                strArray2[index14] = str18;
                int index15 = 3;
                string str19 = str4;
                strArray2[index15] = str19;
                int index16 = 4;
                string str20 = " ),0) as rentall,        isnull((select sum(payment) from lsBalance ba                                           inner join dcService s on s.Service_id=ba.Service_id                                           where period_id=fperiod and ba.client_id=b.client_id ";
                strArray2[index16] = str20;
                int index17 = 5;
                string str21 = str4;
                strArray2[index17] = str21;
                int index18 = 6;
                string str22 = " ),0) as payment,        isnull((select sum(i.dept) from lsInstalment i,lsAgreement a where i.agreement_id=a.agreement_id and i.period_id=fperiod            and client_id=b.client_id and isnull(a.dend,'2999-12-31')>=:period),0) as sogl,       isnull((select sum(i.deptpeni) from lsInstalment i,lsAgreement a where i.agreement_id=a.agreement_id and i.period_id=fperiod            and client_id=b.client_id and isnull(a.dend,'2999-12-31')>=:period),0) as penisogl,       isnull((select sum(isnull(rent,0)+isnull(correct,0)+isnull(payment,0)) from lsBalancePeni bp                                                                               inner join dcService s on s.Service_id=bp.Service_id                                                                               where period_id=fperiod  ";
                strArray2[index18] = str22;
                int index19 = 7;
                string str23 = str4;
                strArray2[index19] = str23;
                int index20 = 8;
                string str24 = "  and client_id=b.client_id),0)+penisogl as penirent1,       isnull((select sum(balance_in) from lsBalance ba                                           inner join dcService s on s.Service_id=ba.Service_id                                           where period_id=fperiod and ba.client_id=b.client_id ";
                strArray2[index20] = str24;
                int index21 = 9;
                string str25 = str4;
                strArray2[index21] = str25;
                int index22 = 10;
                string str26 = " ),0) as dept,       isnull((select sum(balance_in) from lsBalancePeni bp                                        inner join dcService s on s.Service_id=bp.Service_id                                            where period_id=fperiod and bp.client_id=b.client_id  ";
                strArray2[index22] = str26;
                int index23 = 11;
                string str27 = str4;
                strArray2[index23] = str27;
                int index24 = 12;
                string str28 = " ),0) as penidept,       if sogl > 0 then rentall+sogl else rentall-payment+sogl endif as rent1,       (if rent1>0 then rent1 else 0 endif) as rent, (if penirent1>0 then penirent1 else 0 endif) as penirent    from lsBond b, mngContract c    where c.contract_id=b.contract_id and c.manager_id=:manager ";
                strArray2[index24] = str28;
                int index25 = 13;
                string usl2 = this.GetUsl(" and c.idbaseorg={0}", " and guild_id={0}", " and c.contract_id={0}");
                strArray2[index25] = usl2;
                int index26 = 14;
                string str29 = "   and c.dbeg<=:period and c.dend>=:period and b.dbeg<=:period and b.dend>=:period) as my";
                strArray2[index26] = str29;
                string queryString3 = string.Concat(strArray2);
                session3.CreateSQLQuery(queryString3).SetParameter<int>("periodid", Options.Period.PeriodId).SetParameter<int>("manager", ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId).SetParameter<string>("period", KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value)).ExecuteUpdate();
                break;
              case 2:
                ISession session4 = this.session;
                string[] strArray3 = new string[15];
                strArray3[0] = "insert into lsRegister (period_id, manager_id, contract_id, client_id, idform, guild_id, tab_num, rent, rentpeni, deduction, deductionpeni, dept, deptpeni, payment_id, receipt_id)select fperiod, manager_id, contract_id, client_id, idform, guild_id, tabnum, rent, penirent, rent, penirent, dept, penidept, 0, ";
                int index27 = 1;
                receiptId = ((Receipt) this.cbReceiptType.SelectedItem).ReceiptId;
                string str30 = receiptId.ToString();
                strArray3[index27] = str30;
                int index28 = 2;
                string str31 = " from(    select :periodid as fperiod, c.manager_id, b.contract_id, b.client_id, b.idform,        (select (if deesp=1 then null else deesp endif) from form_a where idform=b.idform) as guild_id,        isnull(cast((select percentsharestring from form_a where idform=b.idform) as char(22)),'') as tabnum,       isnull((select sum(rent)-sum(msp) from lsBalance ba                                           inner join dcService s on s.Service_id=ba.Service_id                                           where period_id=fperiod and ba.client_id=b.client_id ";
                strArray3[index28] = str31;
                int index29 = 3;
                string str32 = str4;
                strArray3[index29] = str32;
                int index30 = 4;
                string str33 = " ),0) as rentall,        isnull((select sum(balance_out) from lsBalance ba                                           inner join dcService s on s.Service_id=ba.Service_id                                           where period_id=fperiod and ba.client_id=b.client_id ";
                strArray3[index30] = str33;
                int index31 = 5;
                string str34 = str4;
                strArray3[index31] = str34;
                int index32 = 6;
                string str35 = " ),0) as bout,       isnull((select sum(payment) from lsBalance ba                                           inner join dcService s on s.Service_id=ba.Service_id                                           where period_id=fperiod and ba.client_id=b.client_id ";
                strArray3[index32] = str35;
                int index33 = 7;
                string str36 = str4;
                strArray3[index33] = str36;
                int index34 = 8;
                string str37 = " ),0) as payment,       isnull((select sum(i.dept) from lsInstalment i,lsAgreement a where i.agreement_id=a.agreement_id and i.period_id=fperiod            and client_id=b.client_id and isnull(a.dend,'2999-12-31')>=:period),0) as sogl,       isnull((select sum(isnull(rent,0)+isnull(correct,0)) from lsBalancePeni where period_id=fperiod    and client_id=b.client_id),0)+           isnull((select sum(i.deptpeni) from lsInstalment i,lsAgreement a where i.agreement_id=a.agreement_id and i.period_id=fperiod                    and client_id=b.client_id and isnull(a.dend,'2999-12-31')>=:period),0) as penirent1,       isnull((select sum(balance_in) from lsBalance ba                                           inner join dcService s on s.Service_id=ba.Service_id                                           where period_id=fperiod and ba.client_id=b.client_id ";
                strArray3[index34] = str37;
                int index35 = 9;
                string str38 = str4;
                strArray3[index35] = str38;
                int index36 = 10;
                string str39 = " ),0) as dept,        isnull((select sum(balance_in) from lsBalancePeni bp                                        inner join dcService s on s.Service_id=bp.Service_id                                            where period_id=fperiod and bp.client_id=b.client_id  ";
                strArray3[index36] = str39;
                int index37 = 11;
                string str40 = str4;
                strArray3[index37] = str40;
                int index38 = 12;
                string str41 = " ),0) as penidept,       if sogl > 0 then rentall+sogl else bout endif as rent1,       (if rent1>0 then rent1 else 0 endif) as rent, (if penirent1>0 then penirent1+penidept else 0 endif) as penirent    from lsBond b, mngContract c    where c.contract_id=b.contract_id and c.manager_id=:manager ";
                strArray3[index38] = str41;
                int index39 = 13;
                string usl3 = this.GetUsl(" and c.idbaseorg={0}", " and guild_id={0}", " and c.contract_id={0}");
                strArray3[index39] = usl3;
                int index40 = 14;
                string str42 = " and c.dbeg<=:period and c.dend>=:period and b.dbeg<=:period and b.dend>=:period) as my";
                strArray3[index40] = str42;
                string queryString4 = string.Concat(strArray3);
                session4.CreateSQLQuery(queryString4).SetParameter<int>("periodid", Options.Period.PeriodId).SetParameter<int>("manager", ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId).SetParameter<string>("period", KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value)).ExecuteUpdate();
                break;
              case 3:
                ISession session5 = this.session;
                string[] strArray4 = new string[19];
                strArray4[0] = "insert into lsRegister (period_id, manager_id, contract_id, client_id, idform, guild_id, tab_num, rent, rentpeni, deduction, deductionpeni, dept, deptpeni, payment_id, receipt_id )select fperiod, manager_id, contract_id, client_id, idform, guild_id, tabnum, rent, penirent, rent, penirent, dept, penidept, 0, ";
                int index41 = 1;
                receiptId = ((Receipt) this.cbReceiptType.SelectedItem).ReceiptId;
                string str43 = receiptId.ToString();
                strArray4[index41] = str43;
                int index42 = 2;
                string str44 = " from(    select :periodid as fperiod, c.manager_id, b.contract_id, b.client_id, b.idform,        (select (if deesp=1 then null else deesp endif) from form_a where idform=b.idform) as guild_id,        isnull(cast((select percentsharestring from form_a where idform=b.idform) as char(22)),'') as tabnum,       isnull((select sum(rent)-sum(msp) from lsBalance ba                                           inner join dcService s on s.Service_id=ba.Service_id                                           where period_id=fperiod and ba.client_id=b.client_id ";
                strArray4[index42] = str44;
                int index43 = 3;
                string str45 = str4;
                strArray4[index43] = str45;
                int index44 = 4;
                string str46 = " ),0) as rentall,        isnull((select sum(balance_out) from lsBalance ba                                           inner join dcService s on s.Service_id=ba.Service_id                                           where period_id=fperiod and ba.client_id=b.client_id ";
                strArray4[index44] = str46;
                int index45 = 5;
                string str47 = str4;
                strArray4[index45] = str47;
                int index46 = 6;
                string str48 = " ),0) as bout,        isnull((select sum(payment) from lsBalance ba                                           inner join dcService s on s.Service_id=ba.Service_id                                           where period_id=fperiod and ba.client_id=b.client_id ";
                strArray4[index46] = str48;
                int index47 = 7;
                string str49 = str4;
                strArray4[index47] = str49;
                int index48 = 8;
                string str50 = " ),0) as payment,        isnull((select sum(balance_in) from lsBalance ba                                           inner join dcService s on s.Service_id=ba.Service_id                                           where period_id=fperiod and ba.client_id=b.client_id ";
                strArray4[index48] = str50;
                int index49 = 9;
                string str51 = str4;
                strArray4[index49] = str51;
                int index50 = 10;
                string str52 = " ),0) as dept,        isnull((select sum(i.dept) from lsInstalment i,lsAgreement a where i.agreement_id=a.agreement_id and i.period_id=fperiod            and client_id=b.client_id and isnull(a.dend,'2999-12-31')>=:period),0) as sogl,        isnull((select sum(isnull(rent,0)+isnull(correct,0)) from lsBalancePeni bp                                        inner join dcService s on s.Service_id=bp.Service_id                                            where period_id=fperiod and bp.client_id=b.client_id  ";
                strArray4[index50] = str52;
                int index51 = 11;
                string str53 = str4;
                strArray4[index51] = str53;
                int index52 = 12;
                string str54 = " ),0) as penirentall,        isnull((select sum(i.deptpeni) from lsInstalment i,lsAgreement a where i.agreement_id=a.agreement_id and i.period_id=fperiod            and client_id=b.client_id and isnull(a.dend,'2999-12-31')>=:period),0) as penisogl,        isnull((select sum(balance_in) from lsBalancePeni bp                                        inner join dcService s on s.Service_id=bp.Service_id                                            where period_id=fperiod and bp.client_id=b.client_id  ";
                strArray4[index52] = str54;
                int index53 = 13;
                string str55 = str4;
                strArray4[index53] = str55;
                int index54 = 14;
                string str56 = " ),0) as penidept,        isnull((select sum(payment) from lsBalancePeni bp                                        inner join dcService s on s.Service_id=bp.Service_id                                            where period_id=fperiod and bp.client_id=b.client_id  ";
                strArray4[index54] = str56;
                int index55 = 15;
                string str57 = str4;
                strArray4[index55] = str57;
                int index56 = 16;
                string str58 = " ),0) as penipay,        rentall-payment as raznica, penirentall-penipay as peniraznica,        (if sogl>0 then (if dept>=sogl then (if rentall>=0 then rentall+sogl else sogl endif) else (if dept+raznica>=0 then dept+raznica else 0 endif) endif)                    else (if dept>50 then (if rentall>0 then rentall else 0 endif) else (if dept+raznica>=0 then dept+raznica else 0 endif) endif) endif) as rent,        (if penisogl>0 then (if penidept>=penisogl then (if penirentall>=0 then penirentall+penisogl else penisogl endif) else (if penidept+peniraznica>=0 then penidept+peniraznica else 0 endif) endif)                        else (if penidept>50 then (if penirentall>0 then penirentall else 0 endif) else (if penidept+peniraznica>=0 then penidept+peniraznica else 0 endif) endif) endif) as penirent    from lsBond b, mngContract c    where c.contract_id=b.contract_id and c.manager_id=:manager ";
                strArray4[index56] = str58;
                int index57 = 17;
                string usl4 = this.GetUsl(" and c.idbaseorg={0}", " and guild_id={0}", " and c.contract_id={0}");
                strArray4[index57] = usl4;
                int index58 = 18;
                string str59 = "   and c.dbeg<=:period and c.dend>=:period and b.dbeg<=:period and b.dend>=:period) as my";
                strArray4[index58] = str59;
                string queryString5 = string.Concat(strArray4);
                session5.CreateSQLQuery(queryString5).SetParameter<int>("periodid", Options.Period.PeriodId).SetParameter<int>("manager", ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId).SetParameter<string>("period", KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value)).ExecuteUpdate();
                break;
            }
            this.LoadUZP();
            this.LoadCmbOrg();
          }
          catch (Exception ex)
          {
            int num3 = (int) MessageBox.Show("Невозможно сформировать реестр", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            KvrplHelper.WriteLog(ex, (LsClient) null);
            return;
          }
          this.Cursor = Cursors.Default;
        }
      }
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnLoadPay_Click(object sender, EventArgs e)
    {
      if ((BaseOrg) this.cmbOrg.SelectedItem == null || ((BaseOrg) this.cmbOrg.SelectedItem).BaseOrgId == 0)
      {
        int num1 = (int) MessageBox.Show("Выберите организацию.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.session.CreateQuery(string.Format("select r from Register r where r.Period.PeriodId={0} and r.Manager.BaseOrgId={1} and r.Receipt.ReceiptId={2}" + this.GetUsl("and r.Contract.BaseOrg.BaseOrgId={0}", " and r.Guild.GuildId={0}", "and r.Contract.ContractId={0}") + " and r.PaymentId <> 0", (object) Options.Period.PeriodId, (object) ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId, (object) this.Receipt_id)).List().Count > 0)
      {
        int num2 = (int) MessageBox.Show("Платежи уже закачаны.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        FrmLoadPays frmLoadPays = new FrmLoadPays((short) 2, ((BaseOrg) this.cmbOrg.SelectedItem).NameOrgMin);
        int num3 = (int) frmLoadPays.ShowDialog();
        if (frmLoadPays.Period == null)
          return;
        if (frmLoadPays.Period != null && frmLoadPays.Period.PeriodId <= this.session.CreateQuery(string.Format("select c.Period from CompanyPeriod c where c.Period.PeriodId=(select min(Period.PeriodId) from CompanyPeriod where Complex.IdFk=100 and Company.CompanyId in (select CompanyId from Company where Manager.BaseOrgId={0})) ", (object) ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId)).List<Period>()[0].PeriodId)
        {
          int num4 = (int) MessageBox.Show("Невозможно закачать платежи в закрытый месяц", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          string str1 = "";
          short receiptId;
          if ((uint) this.Receipt_id > 0U)
          {
            string str2 = " and receipt_id=";
            receiptId = this.Receipt_id;
            string str3 = receiptId.ToString();
            string str4 = " ";
            str1 = str2 + str3 + str4;
          }
          if (MessageBox.Show("Закачать платежи с введенными параметрами?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            return;
          using (ITransaction transaction = this.session.BeginTransaction())
          {
            try
            {
              ISession session = this.session;
              string[] strArray = new string[6];
              strArray[0] = "insert into lsPayment (Payment_id, Period_id, Client_id, SourcePay_id, PurposePay_id, Receipt_id, Service_id, Payment_date, Month_id, Packet_num, Payment_value, Payment_peni, Uname, Dedit, supplier_id, PayDoc_id) select gen_id('lsPayment',number()), :period, client_id, :source, :purpose,  ";
              int index1 = 1;
              receiptId = ((Receipt) this.cbReceiptType.SelectedItem).ReceiptId;
              string str2 = receiptId.ToString();
              strArray[index1] = str2;
              int index2 = 2;
              string str3 = ", 0, :day, :periodpay, :packet, deduction, deductionpeni, user ,today(), 0, 1 from lsRegister where period_id=:per and manager_id=:manager ";
              strArray[index2] = str3;
              int index3 = 3;
              string str4 = str1;
              strArray[index3] = str4;
              int index4 = 4;
              string usl = this.GetUsl(" and contract_id in (select contract_id from mngContract where idbaseorg={0})", " and guild_id={0}", " and contract_id={0}");
              strArray[index4] = usl;
              int index5 = 5;
              string str5 = " and (deduction<>0 or deductionPeni<>0) ";
              strArray[index5] = str5;
              string queryString = string.Concat(strArray);
              session.CreateSQLQuery(queryString).SetParameter<int>("period", frmLoadPays.Period.PeriodId).SetParameter<short>("source", frmLoadPays.SPay.SourcePayId).SetParameter<short>("purpose", frmLoadPays.PPay.PurposePayId).SetDateTime("day", frmLoadPays.PaymentDate).SetParameter<int>("periodpay", frmLoadPays.Period.PeriodId).SetParameter<string>("packet", frmLoadPays.Packet).SetParameter<int>("per", Options.Period.PeriodId).SetParameter<int>("manager", ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId).ExecuteUpdate();
              this.session.CreateSQLQuery(string.Format("update lsRegister set payment_id=(select payment_id from lsPayment where period_id={0} and client_id=lsRegister.client_id and payment_date='{1}' and month_id={2} and packet_num={3} " + str1 + ")where period_id={4} and manager_id={5} " + str1 + this.GetUsl(" and contract_id in (select contract_id from mngContract where idbaseorg={0})", " and guild_id={0}", " and contract_id={0}") + " and (deduction<>0 or deductionPeni<>0)", (object) frmLoadPays.Period.PeriodId, (object) KvrplHelper.DateToBaseFormat(frmLoadPays.PaymentDate), (object) frmLoadPays.Period.PeriodId, (object) frmLoadPays.Packet, (object) Options.Period.PeriodId, (object) ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId)).ExecuteUpdate();
            }
            catch (Exception ex)
            {
              int num5 = (int) MessageBox.Show("Не удалось закачать платежи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              KvrplHelper.WriteLog(ex, (LsClient) null);
              transaction.Rollback();
              return;
            }
            int num6 = (int) MessageBox.Show("Платежи закачаны", "", MessageBoxButtons.OK);
            transaction.Commit();
            frmLoadPays.Dispose();
          }
          this.LoadUZP();
        }
      }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Сохранить внесенные изменения?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      bool flag = false;
      foreach (DataGridViewRow row in (IEnumerable) this.dgvRegister.Rows)
      {
        this.dgvRegister.CurrentCell = row.Cells["OrgName"];
        row.Selected = true;
        if (((Register) row.DataBoundItem).IsEdit)
        {
          if (!this.SaveRegister())
            flag = true;
          else
            ((Register) row.DataBoundItem).IsEdit = false;
        }
      }
      this.btnSave.Enabled = false;
      if (!flag)
        this.LoadUZP();
    }

    private void btnUnLoad_Click(object sender, EventArgs e, short type)
    {
      if ((BaseOrg) this.cmbOrg.SelectedItem == null || ((BaseOrg) this.cmbOrg.SelectedItem).BaseOrgId == 0)
      {
        int num1 = (int) MessageBox.Show("Выберите организацию.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        this.sfdRegister.FileName = ((BaseOrg) this.cmbOrg.SelectedItem).NameOrgMin.Replace("\"", "'");
        this.sfdRegister.Filter = "dbf files (*.dbf)|*.dbf";
        if (this.sfdRegister.ShowDialog() != DialogResult.OK)
          return;
        try
        {
          string fileName = this.sfdRegister.FileName;
          string str1 = fileName.Remove(0, fileName.LastIndexOf("\\") + 1);
          string tblname = str1.Remove(str1.LastIndexOf("."));
          string Folder = fileName.Remove(fileName.LastIndexOf("\\"), fileName.Length - fileName.LastIndexOf("\\"));
          string connectionString = string.Format("Provider={4};Eng={0};Uid={1};Pwd={2}; Links={3}", (object) Options.BaseName, (object) Options.Login, (object) Options.Pwd, (object) "tcpip{}", (object) Options.Provider);
          string str2 = "tab_num as tano,";
          string str3 = ",tano";
          if ((int) type == 2)
          {
            str2 = "client_id as lic,";
            str3 = ",client_id";
          }
          string commandText = string.Format("select cast(year((select period_value from dcPeriod where period_id=r.period_id)) as integer) as god, month((select period_value from dcPeriod where period_id=r.period_id)) as mes, null as korg, " + str2 + "sum(isnull(deduction,0)) as sum1,null as sum2,sum(isnull(deductionpeni,0)) as sum3,null as sum4 from lsRegister r where period_id={0} and contract_id in (select contract_id from mngContract where idbaseorg={1}) and manager_id={2} " + this.GetUsl(" and contract_id in (select contract_id from mngContract where idbaseorg={0})", " and guild_id={0}", " and contract_id={0}") + "group by period_id" + str3, (object) Options.Period.PeriodId, (object) ((BaseOrg) this.cmbOrg.SelectedItem).BaseOrgId, (object) ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId);
          DataSet dataSet = new DataSet();
          OleDbHelper.FillDataset(connectionString, CommandType.Text, commandText, 1000, dataSet, new string[1]{ tblname });
          DBF.DataTableSaveToDBF(dataSet.Tables[0], Folder, tblname);
          int num2 = (int) MessageBox.Show("Выгрузка прошла успешно", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show("Выгрузка прошла c ошибками", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
    }

    private void btnUnLoad_Click_1(object sender, EventArgs e)
    {
      this.cmsUnLoad.Show((Control) this.pnBtn, this.btnUnLoad.Left + 100, 0);
    }

    private void btnControl_Click(object sender, EventArgs e)
    {
      IList<Agreement> agreementList = this.session.CreateQuery(string.Format("select a from Agreement a where DBeg<='{3}' and isnull(DEnd,'2999-12-31')>'{0}' and months(DBeg,MonthCount)>='{0}' and LsClient.ClientId in (select ClientId from LsClient where Company.Manager.BaseOrgId={1}) and (select sum(BalanceIn)-sum(Payment) from Balance where Period.PeriodId={2} and LsClient.ClientId=a.LsClient.ClientId)<=0", (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), (object) ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId, (object) Options.Period.PeriodId, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(Options.Period.PeriodName.Value)))).List<Agreement>();
      if (agreementList.Count > 0)
      {
        string text = "Обнаружены лицевые счета, где погашен долг, но есть действующее соглашение:\n";
        foreach (Agreement agreement in (IEnumerable<Agreement>) agreementList)
          text = text + agreement.LsClient.ClientId.ToString() + " ";
        int num1 = (int) MessageBox.Show(text, "", MessageBoxButtons.OK);
        if (MessageBox.Show("Сохранить лицевые в файл?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
          return;
        if (this.sfd.ShowDialog() == DialogResult.OK)
        {
          try
          {
            StreamWriter streamWriter = new StreamWriter((Stream) File.Open(this.sfd.FileName, FileMode.Append, FileAccess.Write), Encoding.Default);
            streamWriter.WriteLine(DateTime.Now.ToString() + ": Обнаружены лицевые счета, где погашен долг, но есть действующее соглашение");
            foreach (Agreement agreement in (IEnumerable<Agreement>) agreementList)
              streamWriter.WriteLine(agreement.LsClient.ClientId);
            streamWriter.Close();
          }
          catch (Exception ex)
          {
            int num2 = (int) MessageBox.Show("Не удалось сохранить данные в файл");
            KvrplHelper.WriteLog(ex, (LsClient) null);
            return;
          }
          int num3 = (int) MessageBox.Show("Данные сохранены");
        }
      }
      else
      {
        int num = (int) MessageBox.Show("Проверка прошла успешно");
      }
    }

    private void btnReport_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(34, 1, Options.Company, true))
        return;
      int num1 = !KvrplHelper.CheckProxy(32, 2, Options.Company, false) ? 0 : 1;
      int admin = !Options.Kvartplata || !Options.Arenda ? (!Options.Kvartplata ? num1 + 10 : num1 + 0) : num1 + 20;
      this.Cursor = Cursors.WaitCursor;
      this.session = Domain.CurrentSession;
      this.session.Clear();
      try
      {
        if (Options.Company == null)
          CallDll.Rep(this.city, 0, 0, 0, 0, 0, 145, Options.Period.PeriodId, admin, Options.Alias, Options.Login, Options.Pwd);
        else
          CallDll.Rep(this.city, 0, Options.Company.Raion.IdRnn, (int) Options.Company.CompanyId, 0, 0, 145, Options.Period.PeriodId, admin, Options.Alias, Options.Login, Options.Pwd);
      }
      catch (Exception ex)
      {
        int num2 = (int) MessageBox.Show("Невозможно вызвать библиотеку отчетов!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      this.Cursor = Cursors.Default;
    }

    private void dgvRegister_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsRegister.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsRegister.Columns[this.MySettingsRegister.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsRegister.Save();
    }

    private void dgvRegister_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e, 0);
    }

    private void dgvRegister_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (((DataGridView) sender).DataSource == null)
        return;
      DataGridViewRow row = ((DataGridView) sender).Rows[e.RowIndex];
      if ((uint) ((Register) row.DataBoundItem).PaymentId > 0U)
        row.DefaultCellStyle.ForeColor = Color.Gray;
      else
        row.DefaultCellStyle.ForeColor = Color.Black;
    }

    private void dgvRegister_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.btnSave.Enabled = true;
      ((Register) this.dgvRegister.CurrentRow.DataBoundItem).IsEdit = true;
    }

    private void mi2DBF(object sender, EventArgs e)
    {
      this.btnUnLoad_Click((object) null, (EventArgs) null, Convert.ToInt16(((ToolStripItem) sender).Tag));
    }

    private void mi2CSV_Click(object sender, EventArgs e)
    {
      this.UnloadToSCV();
    }

    private void mpCurrentPeriod_ValueChanged(object sender, EventArgs e)
    {
      Options.Period = KvrplHelper.SaveCurrentPeriod(this.mpCurrentPeriod.Value);
      this.LoadCmbOrg();
      this.LoadUZP();
      this.EnabledButtons();
    }

    private string GetUsl(string uslOrg, string uslGuild, string uslContract)
    {
      string str = "";
      if ((BaseOrg) this.cmbOrg.SelectedItem != null && (uint) ((BaseOrg) this.cmbOrg.SelectedItem).BaseOrgId > 0U)
        str += string.Format(uslOrg, (object) ((BaseOrg) this.cmbOrg.SelectedItem).BaseOrgId);
      if ((Guild) this.cmbGuild.SelectedItem != null && (uint) ((Guild) this.cmbGuild.SelectedItem).GuildId > 0U)
        str += string.Format(uslGuild, (object) ((Guild) this.cmbGuild.SelectedItem).GuildId);
      if ((Contract) this.cmbContract.SelectedItem != null && (uint) ((Contract) this.cmbContract.SelectedItem).ContractId > 0U)
        str += string.Format(uslContract, (object) ((Contract) this.cmbContract.SelectedItem).ContractId);
      return str;
    }

    private void LoadSettingsRegister()
    {
      this.MySettingsRegister.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvRegister.Columns)
        this.MySettingsRegister.GetMySettings(column);
    }

    private void LoadCmbManager()
    {
      IList<BaseOrg> baseOrgList = (IList<BaseOrg>) new List<BaseOrg>();
      this.cmbManager.DataSource = (object) this.session.CreateQuery("select new BaseOrg(b.BaseOrgId,b.NameOrgMin) from BaseOrg b where b.BaseOrgId in (select Manager.BaseOrgId from Company) and b.BaseOrgId<>0 order by b.BaseOrgName").List<BaseOrg>();
      this.cmbManager.DisplayMember = "NameOrgMin";
      this.cmbManager.ValueMember = "BaseOrgId";
      BaseOrg baseOrg = new BaseOrg();
      try
      {
        baseOrg = this.session.Get<BaseOrg>((object) Options.Company.Manager.BaseOrgId);
      }
      catch (Exception ex)
      {
        if (Options.Company != null)
          baseOrg = this.session.CreateQuery(string.Format("select c.Manager from Company c where c.CompanyId={0}", (object) Options.Company.CompanyId)).List<BaseOrg>()[0];
      }
      if (baseOrg == null)
        return;
      this.cmbManager.SelectedValue = (object) baseOrg.BaseOrgId;
    }

    private void LoadCmbOrg()
    {
      int num = 0;
      if (this.cmbOrg.SelectedValue != null)
        num = Convert.ToInt32(this.cmbOrg.SelectedValue);
      this.cmbOrg.DataSource = (object) null;
      IList<BaseOrg> baseOrgList1 = (IList<BaseOrg>) new List<BaseOrg>();
      IList<BaseOrg> baseOrgList2 = this.city == 23 ? this.session.CreateQuery(string.Format("select new BaseOrg(b.BaseOrgId,b.NameOrgMin) from BaseOrg b where b.BaseOrgId in (select BaseOrg.BaseOrgId from Contract where Manager.BaseOrgId={0} and ContractId in (select Contract.ContractId from Register where Period.PeriodId={1}))  order by b.BaseOrgName", (object) ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId, (object) Options.Period.PeriodId)).List<BaseOrg>() : this.session.CreateQuery(string.Format("select new BaseOrg(b.BaseOrgId,b.NameOrgMin) from BaseOrg b where b.BaseOrgId in (select BaseOrg.BaseOrgId from Contract where Manager.BaseOrgId={0}) order by b.BaseOrgName", (object) ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId)).List<BaseOrg>();
      baseOrgList2.Insert(0, new BaseOrg(0, ""));
      this.cmbOrg.DataSource = (object) baseOrgList2;
      this.cmbOrg.DisplayMember = "NameOrgMin";
      this.cmbOrg.ValueMember = "BaseOrgId";
      this.cmbOrg.SelectedValue = (object) num;
    }

    private void LoadCmbGuild()
    {
      this.cmbGuild.DataSource = (object) null;
      string str = "";
      if (this.cmbOrg.SelectedValue != null && (uint) Convert.ToInt32(this.cmbOrg.SelectedValue) > 0U)
        str = string.Format(" and BaseOrg.BaseOrgId={0}", this.cmbOrg.SelectedValue);
      IList<Guild> guildList = this.session.CreateQuery(string.Format("from Guild where BaseOrg.BaseOrgId={0} order by lengthhome(GuildName)", (object) ((BaseOrg) this.cmbOrg.SelectedItem).BaseOrgId)).List<Guild>();
      guildList.Insert(0, new Guild(0, ""));
      this.cmbGuild.DataSource = (object) guildList;
      this.cmbGuild.ValueMember = "GuildId";
      this.cmbGuild.DisplayMember = "GuildName";
    }

    private void LoadCmbContract()
    {
      this.cmbContract.DataSource = (object) null;
      this.session.Clear();
      string str = "";
      if (this.cmbOrg.SelectedValue != null && (uint) Convert.ToInt32(this.cmbOrg.SelectedValue) > 0U)
        str = string.Format(" and BaseOrg.BaseOrgId={0}", this.cmbOrg.SelectedValue);
      IList<Contract> contractList = this.session.CreateQuery(string.Format("select c from Contract c where Manager.BaseOrgId={0}" + str + " order by DBeg desc", (object) ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId)).List<Contract>();
      contractList.Insert(0, new Contract(0, ""));
      this.cmbContract.DataSource = (object) contractList;
      this.cmbContract.DisplayMember = "ContractNum";
      this.cmbContract.ValueMember = "ContractId";
    }

    private void LoadCmbReceiptType()
    {
      IList<Receipt> receiptList1 = (IList<Receipt>) new List<Receipt>();
      IList<Receipt> receiptList2 = this.session.CreateQuery("from Receipt where ReceiptId<>0").List<Receipt>();
      receiptList2.Insert(0, new Receipt()
      {
        ReceiptId = (short) 0,
        ReceiptName = ""
      });
      this.cbReceiptType.DataSource = (object) receiptList2;
      this.cbReceiptType.ValueMember = "ReceiptId";
      this.cbReceiptType.DisplayMember = "ReceiptName";
    }

    private void LoadUZP()
    {
      string str = "";
      if ((uint) this.Receipt_id > 0U)
        str = " and receipt_id=" + this.Receipt_id.ToString() + " ";
      this.session.Clear();
      this.dgvRegister.DataSource = (object) null;
      this.dgvRegister.Columns.Clear();
      if ((BaseOrg) this.cmbManager.SelectedItem == null)
        return;
      this.RegistersList = this.session.CreateQuery(string.Format("select r from Register r left join fetch r.Contract c left join fetch r.Guild g where r.Period.PeriodId={0} and r.Manager.BaseOrgId={1} " + str + this.GetUsl("and r.Contract.BaseOrg.BaseOrgId={0}", " and r.Guild.GuildId={0}", "and r.Contract.ContractId={0}") + " order by r.Contract.BaseOrg.BaseOrgId,r.Contract.ContractNum,lengthhome(r.Guild.GuildName),Lengthhome(r.TabNum)", (object) Options.Period.PeriodId, (object) ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId)).List<Register>();
      this.dgvRegister.DataSource = (object) this.RegistersList;
      this.MySettingsRegister.GridName = "Register";
      this.SetViewUZP();
    }

    private void SetViewUZP()
    {
      this.dgvRegister.Columns["OrgName"].HeaderText = "Организация";
      this.dgvRegister.Columns["OrgName"].ValueType = typeof (string);
      this.dgvRegister.Columns["OrgName"].DisplayIndex = 0;
      this.dgvRegister.Columns["GuildName"].HeaderText = "Цех";
      this.dgvRegister.Columns["GuildName"].DisplayIndex = 1;
      this.dgvRegister.Columns["TabNum"].HeaderText = "Табельный номер";
      this.dgvRegister.Columns["TabNum"].DisplayIndex = 2;
      this.dgvRegister.Columns["ContractNum"].HeaderText = "№ Договора";
      this.dgvRegister.Columns["ContractNum"].DisplayIndex = 3;
      this.dgvRegister.Columns["FIO"].HeaderText = "Плательщик";
      this.dgvRegister.Columns["FIO"].DisplayIndex = 4;
      this.dgvRegister.Columns["LS"].HeaderText = "Лицевой";
      this.dgvRegister.Columns["LS"].DisplayIndex = 5;
      KvrplHelper.AddTextBoxColumn(this.dgvRegister, 6, "Начислено", "Rent", 100, false);
      KvrplHelper.AddTextBoxColumn(this.dgvRegister, 7, "Нач.пени", "RentPeni", 100, false);
      KvrplHelper.AddTextBoxColumn(this.dgvRegister, 8, "Удержано", "Deduction", 100, false);
      KvrplHelper.AddTextBoxColumn(this.dgvRegister, 9, "Удерж. пени", "DeductionPeni", 100, false);
      this.dgvRegister.Columns["Rent"].SortMode = DataGridViewColumnSortMode.Automatic;
      this.dgvRegister.Columns["Debt"].HeaderText = "Задолженность";
      this.dgvRegister.Columns["DebtPeni"].HeaderText = "Задолж. пени";
      KvrplHelper.ViewEdit(this.dgvRegister);
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvRegister.Columns)
      {
        if (column.Name != "Rent" && column.Name != "RentPeni" && column.Name != "Deduction" && column.Name != "DeductionPeni")
          column.ReadOnly = true;
        else
          column.ReadOnly = false;
      }
      foreach (DataGridViewRow row in (IEnumerable) this.dgvRegister.Rows)
      {
        row.Cells["Rent"].Value = (object) ((Register) row.DataBoundItem).Rent;
        row.Cells["RentPeni"].Value = (object) ((Register) row.DataBoundItem).RentPeni;
        row.Cells["Deduction"].Value = (object) ((Register) row.DataBoundItem).Deduction;
        row.Cells["DeductionPeni"].Value = (object) ((Register) row.DataBoundItem).DeductionPeni;
        if ((uint) ((Register) row.DataBoundItem).PaymentId > 0U)
          row.ReadOnly = true;
        else
          row.ReadOnly = false;
      }
      this.LoadSettingsRegister();
    }

    private void EnabledButtons()
    {
      if (this.monthClosed != null && Options.Period.PeriodId > this.monthClosed.PeriodId)
        this.btnMake.Enabled = true;
      else
        this.btnMake.Enabled = false;
    }

    private bool SaveRegister()
    {
      this.session.Clear();
      Register dataBoundItem = (Register) this.dgvRegister.CurrentRow.DataBoundItem;
      if (this.dgvRegister.CurrentRow.Cells["Rent"].Value != null)
      {
        try
        {
          dataBoundItem.Rent = Convert.ToDouble(KvrplHelper.ChangeSeparator(this.dgvRegister.CurrentRow.Cells["Rent"].Value.ToString()));
        }
        catch
        {
          int num = (int) MessageBox.Show("Некорректная сумма", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
      }
      else
        dataBoundItem.Rent = 0.0;
      if (this.dgvRegister.CurrentRow.Cells["RentPeni"].Value != null)
      {
        try
        {
          dataBoundItem.RentPeni = Convert.ToDouble(KvrplHelper.ChangeSeparator(this.dgvRegister.CurrentRow.Cells["RentPeni"].Value.ToString()));
        }
        catch
        {
          int num = (int) MessageBox.Show("Некорректная сумма", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
      }
      else
        dataBoundItem.RentPeni = 0.0;
      if (this.dgvRegister.CurrentRow.Cells["Deduction"].Value != null)
      {
        try
        {
          dataBoundItem.Deduction = Convert.ToDouble(KvrplHelper.ChangeSeparator(this.dgvRegister.CurrentRow.Cells["Deduction"].Value.ToString()));
        }
        catch
        {
          int num = (int) MessageBox.Show("Некорректная сумма", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
      }
      else
        dataBoundItem.Deduction = 0.0;
      if (this.dgvRegister.CurrentRow.Cells["DeductionPeni"].Value != null)
      {
        try
        {
          dataBoundItem.DeductionPeni = Convert.ToDouble(KvrplHelper.ChangeSeparator(this.dgvRegister.CurrentRow.Cells["DeductionPeni"].Value.ToString()));
        }
        catch
        {
          int num = (int) MessageBox.Show("Некорректная сумма", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
      }
      else
        dataBoundItem.DeductionPeni = 0.0;
      try
      {
        this.session.Update((object) dataBoundItem);
        this.session.Flush();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        return false;
      }
      return true;
    }

    private void UnloadToSCV()
    {
      if ((BaseOrg) this.cmbOrg.SelectedItem == null || ((BaseOrg) this.cmbOrg.SelectedItem).BaseOrgId == 0)
      {
        int num1 = (int) MessageBox.Show("Выберите организацию.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        string str1 = "";
        string str2 = "";
        BaseOrg baseOrg = this.session.Get<BaseOrg>((object) ((BaseOrg) this.cmbOrg.SelectedItem).BaseOrgId);
        if (baseOrg.AdditionalCode == null || baseOrg.AdditionalCode == "")
        {
          int num2 = (int) MessageBox.Show("Выгрузка реестра для этой организации невозможна в данном формате", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          if (!this.IsExistTabNum())
            return;
          DateTime? periodName;
          switch (((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId)
          {
            case 1:
              str1 += "20011062";
              string str3 = "24.";
              periodName = Options.Period.PeriodName;
              string str4 = periodName.Value.ToString("MM.");
              periodName = Options.Period.PeriodName;
              string str5 = periodName.Value.Year.ToString();
              str2 = str3 + str4 + str5;
              break;
            case 2:
              str1 += "20011061";
              string str6 = "23.";
              periodName = Options.Period.PeriodName;
              string str7 = periodName.Value.ToString("MM.");
              periodName = Options.Period.PeriodName;
              string str8 = periodName.Value.Year.ToString();
              str2 = str6 + str7 + str8;
              break;
            case 3:
              str1 += "20011608";
              string str9 = "22.";
              periodName = Options.Period.PeriodName;
              string str10 = periodName.Value.ToString("MM.");
              periodName = Options.Period.PeriodName;
              string str11 = periodName.Value.Year.ToString();
              str2 = str9 + str10 + str11;
              break;
            case 42:
              str1 += "60000196";
              string str12 = "25.";
              periodName = Options.Period.PeriodName;
              string str13 = periodName.Value.ToString("MM.");
              periodName = Options.Period.PeriodName;
              string str14 = periodName.Value.Year.ToString();
              str2 = str12 + str13 + str14;
              break;
            case 210:
              str1 += "20013717";
              string str15 = "27.";
              periodName = Options.Period.PeriodName;
              string str16 = periodName.Value.ToString("MM.");
              periodName = Options.Period.PeriodName;
              string str17 = periodName.Value.Year.ToString();
              str2 = str15 + str16 + str17;
              break;
            case 213:
              str1 += "20011645";
              string str18 = "26.";
              periodName = Options.Period.PeriodName;
              string str19 = periodName.Value.ToString("MM.");
              periodName = Options.Period.PeriodName;
              string str20 = periodName.Value.Year.ToString();
              str2 = str18 + str19 + str20;
              break;
            case 214:
              str1 += "20012630";
              string str21 = "28.";
              periodName = Options.Period.PeriodName;
              string str22 = periodName.Value.ToString("MM.");
              periodName = Options.Period.PeriodName;
              string str23 = periodName.Value.Year.ToString();
              str2 = str21 + str22 + str23;
              break;
          }
          SaveFileDialog sfdRegister = this.sfdRegister;
          string[] strArray = new string[6]{ baseOrg.AdditionalCode, "_", str1, "_", null, null };
          int index1 = 4;
          periodName = Options.Period.PeriodName;
          DateTime dateTime = periodName.Value;
          string str24 = dateTime.ToString("MM");
          strArray[index1] = str24;
          int index2 = 5;
          periodName = Options.Period.PeriodName;
          dateTime = periodName.Value;
          string str25 = dateTime.Year.ToString();
          strArray[index2] = str25;
          string str26 = string.Concat(strArray);
          sfdRegister.FileName = str26;
          this.sfdRegister.Filter = "csv files (*.csv)|*.csv";
          if (this.sfdRegister.ShowDialog() != DialogResult.OK)
            return;
          try
          {
            string fileName = this.sfdRegister.FileName;
            string str27 = fileName.Remove(0, fileName.LastIndexOf("\\") + 1);
            string str28 = str27.Remove(str27.LastIndexOf("."));
            string connectionString = string.Format("Provider={4};Eng={0};Uid={1};Pwd={2}; Links={3}", (object) Options.BaseName, (object) Options.Login, (object) Options.Pwd, (object) "tcpip{}", (object) Options.Provider);
            string str29 = baseOrg.AdditionalCode.Substring(baseOrg.AdditionalCode.Length - 2, 2);
            string commandText = string.Format("select '" + str2 + "' as data,(if length(tab_num)= 8 then tab_num else cast(" + str29 + "*power(10,6-length(tab_num)) as varchar(60))||tab_num endif) as num, (select first (select family from frPers where code=1 and id=f.idform)||' '||(select name from frPers where code=1 and id=f.idform)||' '||(select lastname from frPers where code=1 and id=f.idform) from form_a f where (deesp=r.guild_id or (deesp is null and r.guild_id is null)) and percentsharestring=r.tab_num) as fio,7255,null, sum(isnull(deduction,0))+sum(isnull(deductionpeni,0)) as summ,null,null,null," + str1 + " from lsRegister r where period_id={0} and contract_id in (select contract_id from mngcontract where idbaseorg={1}) and manager_id={2} " + this.GetUsl(" and contract_id in (select contract_id from mngContract where idbaseorg={0})", " and guild_id={0}", " and contract_id={0}") + "group by contract_id,guild_id,tab_num having summ<>0", (object) Options.Period.PeriodId, (object) ((BaseOrg) this.cmbOrg.SelectedItem).BaseOrgId, (object) ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId);
            DataSet dataSet = new DataSet();
            OleDbHelper.FillDataset(connectionString, CommandType.Text, commandText, 1000, dataSet, new string[1]{ str28 });
            StreamWriter streamWriter = new StreamWriter(fileName, true, Encoding.Default);
            if ((uint) dataSet.Tables[0].Rows.Count > 0U)
            {
              foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
                streamWriter.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}", row["data"], row["num"], row["fio"], (object) 7255, row[4], (object) KvrplHelper.ChangeSeparator(row["summ"].ToString(), Options.Separator), row[6], row[7], row[8], row[9]));
            }
            streamWriter.Close();
            int num3 = (int) MessageBox.Show("Выгрузка прошла успешно", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          catch (Exception ex)
          {
            int num3 = (int) MessageBox.Show("Выгрузка прошла c ошибками", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
        }
      }
    }

    private bool IsExistTabNum()
    {
      IList list = this.session.CreateQuery(string.Format("select r.LsClient from Register r where r.Period.PeriodId={0} " + this.GetUsl("and r.Contract.BaseOrg.BaseOrgId={0}", " and r.Guild.GuildId={0}", "and r.Contract.ContractId={0}") + " and (r.TabNum='' or r.TabNum='0') ", (object) Options.Period.PeriodId, (object) ((BaseOrg) this.cmbOrg.SelectedItem).BaseOrgId, (object) ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId)).List();
      if (list.Count <= 0)
        return true;
      KvrplHelper.ViewErrorLic(list, "Найдены лицевые счета где не указан табельный номер. Показать?", "Лицевые счета где не указан табельный номер: ", (short) 3, this.sfd);
      return false;
    }

    private void cbReceiptType_SelectedValueChanged(object sender, EventArgs e)
    {
      if ((Receipt) this.cbReceiptType.SelectedItem != null && (int) ((Receipt) this.cbReceiptType.SelectedItem).ReceiptId != (int) this.Receipt_id)
        this.Receipt_id = ((Receipt) this.cbReceiptType.SelectedItem).ReceiptId;
      if ((uint) this.Receipt_id > 0U)
        this.btnLoadPay.Enabled = true;
      else
        this.btnLoadPay.Enabled = false;
      this.LoadUZP();
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
      this.cmsUnLoad = new ContextMenuStrip(this.components);
      this.tsmiUnloadDBF = new ToolStripMenuItem();
      this.tsmiTab = new ToolStripMenuItem();
      this.miLS = new ToolStripMenuItem();
      this.mi2CSV = new ToolStripMenuItem();
      this.sfdRegister = new SaveFileDialog();
      this.sfd = new SaveFileDialog();
      this.dgvRegister = new DataGridView();
      this.pnUp = new Panel();
      this.cbReceiptType = new ComboBox();
      this.lbReceiptType = new Label();
      this.mpCurrentPeriod = new MonthPicker();
      this.cmbContract = new ComboBox();
      this.cmbGuild = new ComboBox();
      this.cmbOrg = new ComboBox();
      this.cmbManager = new ComboBox();
      this.lblContract = new Label();
      this.lblGuild = new Label();
      this.lblOrg = new Label();
      this.lblManager = new Label();
      this.pnBtn = new Panel();
      this.btnReport = new Button();
      this.btnControl = new Button();
      this.btnUnLoad = new Button();
      this.btnSave = new Button();
      this.btnLoadPay = new Button();
      this.btnMake = new Button();
      this.btnExit = new Button();
      this.cmsUnLoad.SuspendLayout();
      ((ISupportInitialize) this.dgvRegister).BeginInit();
      this.pnUp.SuspendLayout();
      this.pnBtn.SuspendLayout();
      this.SuspendLayout();
      this.cmsUnLoad.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsmiUnloadDBF,
        (ToolStripItem) this.mi2CSV
      });
      this.cmsUnLoad.Name = "cmsUnLoad";
      this.cmsUnLoad.Size = new Size(162, 48);
      this.tsmiUnloadDBF.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsmiTab,
        (ToolStripItem) this.miLS
      });
      this.tsmiUnloadDBF.Name = "tsmiUnloadDBF";
      this.tsmiUnloadDBF.Size = new Size(161, 22);
      this.tsmiUnloadDBF.Tag = (object) "1";
      this.tsmiUnloadDBF.Text = "Выгрузить в dbf";
      this.tsmiTab.Name = "tsmiTab";
      this.tsmiTab.Size = new Size(175, 22);
      this.tsmiTab.Tag = (object) "1";
      this.tsmiTab.Text = "Табельный номер";
      this.tsmiTab.Click += new EventHandler(this.mi2DBF);
      this.miLS.Name = "miLS";
      this.miLS.Size = new Size(175, 22);
      this.miLS.Tag = (object) "2";
      this.miLS.Text = "Лицевой счет";
      this.miLS.Click += new EventHandler(this.mi2DBF);
      this.mi2CSV.Name = "mi2CSV";
      this.mi2CSV.Size = new Size(161, 22);
      this.mi2CSV.Tag = (object) "2";
      this.mi2CSV.Text = "Выгрузить в csv";
      this.mi2CSV.Click += new EventHandler(this.mi2CSV_Click);
      this.sfdRegister.Filter = "dbf files (*.dbf)|*.dbf";
      this.sfdRegister.InitialDirectory = "C:\\";
      this.sfd.Filter = "txt files (*.txt)|*.txt";
      this.dgvRegister.BackgroundColor = Color.AliceBlue;
      this.dgvRegister.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvRegister.Dock = DockStyle.Fill;
      this.dgvRegister.Location = new Point(0, 83);
      this.dgvRegister.Name = "dgvRegister";
      this.dgvRegister.ShowEditingIcon = false;
      this.dgvRegister.Size = new Size(1186, 313);
      this.dgvRegister.TabIndex = 2;
      this.dgvRegister.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvRegister_CellBeginEdit);
      this.dgvRegister.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvRegister_CellFormatting);
      this.dgvRegister.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvRegister_ColumnWidthChanged);
      this.dgvRegister.DataError += new DataGridViewDataErrorEventHandler(this.dgvRegister_DataError);
      this.pnUp.Controls.Add((Control) this.cbReceiptType);
      this.pnUp.Controls.Add((Control) this.lbReceiptType);
      this.pnUp.Controls.Add((Control) this.mpCurrentPeriod);
      this.pnUp.Controls.Add((Control) this.cmbContract);
      this.pnUp.Controls.Add((Control) this.cmbGuild);
      this.pnUp.Controls.Add((Control) this.cmbOrg);
      this.pnUp.Controls.Add((Control) this.cmbManager);
      this.pnUp.Controls.Add((Control) this.lblContract);
      this.pnUp.Controls.Add((Control) this.lblGuild);
      this.pnUp.Controls.Add((Control) this.lblOrg);
      this.pnUp.Controls.Add((Control) this.lblManager);
      this.pnUp.Dock = DockStyle.Top;
      this.pnUp.Location = new Point(0, 0);
      this.pnUp.Name = "pnUp";
      this.pnUp.Size = new Size(1186, 83);
      this.pnUp.TabIndex = 1;
      this.cbReceiptType.FormattingEnabled = true;
      this.cbReceiptType.Location = new Point(678, 56);
      this.cbReceiptType.Name = "cbReceiptType";
      this.cbReceiptType.Size = new Size(303, 24);
      this.cbReceiptType.TabIndex = 10;
      this.cbReceiptType.SelectedValueChanged += new EventHandler(this.cbReceiptType_SelectedValueChanged);
      this.lbReceiptType.AutoSize = true;
      this.lbReceiptType.Location = new Point(554, 59);
      this.lbReceiptType.Name = "lbReceiptType";
      this.lbReceiptType.Size = new Size(106, 16);
      this.lbReceiptType.TabIndex = 9;
      this.lbReceiptType.Text = "Тип квитанции";
      this.mpCurrentPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.mpCurrentPeriod.CustomFormat = "MMMM yyyy";
      this.mpCurrentPeriod.Format = DateTimePickerFormat.Custom;
      this.mpCurrentPeriod.Location = new Point(1031, 4);
      this.mpCurrentPeriod.Name = "mpCurrentPeriod";
      this.mpCurrentPeriod.OldMonth = 0;
      this.mpCurrentPeriod.ShowUpDown = true;
      this.mpCurrentPeriod.Size = new Size(140, 22);
      this.mpCurrentPeriod.TabIndex = 8;
      this.mpCurrentPeriod.ValueChanged += new EventHandler(this.mpCurrentPeriod_ValueChanged);
      this.cmbContract.FormattingEnabled = true;
      this.cmbContract.Location = new Point(200, 56);
      this.cmbContract.Name = "cmbContract";
      this.cmbContract.Size = new Size(321, 24);
      this.cmbContract.TabIndex = 7;
      this.cmbContract.SelectionChangeCommitted += new EventHandler(this.cmbContract_SelectionChangeCommitted);
      this.cmbGuild.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cmbGuild.FormattingEnabled = true;
      this.cmbGuild.Location = new Point(885, 31);
      this.cmbGuild.Name = "cmbGuild";
      this.cmbGuild.Size = new Size(121, 24);
      this.cmbGuild.TabIndex = 6;
      this.cmbGuild.SelectionChangeCommitted += new EventHandler(this.cmbGuild_SelectionChangeCommitted);
      this.cmbOrg.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbOrg.FormattingEnabled = true;
      this.cmbOrg.Location = new Point(200, 31);
      this.cmbOrg.Name = "cmbOrg";
      this.cmbOrg.Size = new Size(607, 24);
      this.cmbOrg.TabIndex = 5;
      this.cmbOrg.SelectionChangeCommitted += new EventHandler(this.cmbOrg_SelectionChangeCommitted);
      this.cmbManager.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbManager.FormattingEnabled = true;
      this.cmbManager.Location = new Point(200, 6);
      this.cmbManager.Name = "cmbManager";
      this.cmbManager.Size = new Size(806, 24);
      this.cmbManager.TabIndex = 4;
      this.cmbManager.SelectionChangeCommitted += new EventHandler(this.cmbManager_SelectionChangeCommitted);
      this.lblContract.AutoSize = true;
      this.lblContract.Location = new Point(30, 59);
      this.lblContract.Name = "lblContract";
      this.lblContract.Size = new Size(63, 16);
      this.lblContract.TabIndex = 3;
      this.lblContract.Text = "Договор";
      this.lblGuild.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblGuild.AutoSize = true;
      this.lblGuild.Location = new Point(848, 34);
      this.lblGuild.Name = "lblGuild";
      this.lblGuild.Size = new Size(31, 16);
      this.lblGuild.TabIndex = 2;
      this.lblGuild.Text = "Цех";
      this.lblOrg.AutoSize = true;
      this.lblOrg.Location = new Point(30, 34);
      this.lblOrg.Name = "lblOrg";
      this.lblOrg.Size = new Size(95, 16);
      this.lblOrg.TabIndex = 1;
      this.lblOrg.Text = "Организация";
      this.lblManager.AutoSize = true;
      this.lblManager.Location = new Point(30, 9);
      this.lblManager.Name = "lblManager";
      this.lblManager.Size = new Size(164, 16);
      this.lblManager.TabIndex = 0;
      this.lblManager.Text = "Управляющая компания";
      this.pnBtn.Controls.Add((Control) this.btnReport);
      this.pnBtn.Controls.Add((Control) this.btnControl);
      this.pnBtn.Controls.Add((Control) this.btnUnLoad);
      this.pnBtn.Controls.Add((Control) this.btnSave);
      this.pnBtn.Controls.Add((Control) this.btnLoadPay);
      this.pnBtn.Controls.Add((Control) this.btnMake);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 396);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(1186, 40);
      this.pnBtn.TabIndex = 0;
      this.btnReport.Image = (Image) Resources.file_text_32;
      this.btnReport.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnReport.Location = new Point(632, 6);
      this.btnReport.Name = "btnReport";
      this.btnReport.Size = new Size(88, 29);
      this.btnReport.TabIndex = 8;
      this.btnReport.Text = "Отчет";
      this.btnReport.TextAlign = ContentAlignment.MiddleRight;
      this.btnReport.UseVisualStyleBackColor = true;
      this.btnReport.Click += new EventHandler(this.btnReport_Click);
      this.btnControl.Location = new Point(539, 6);
      this.btnControl.Name = "btnControl";
      this.btnControl.Size = new Size(87, 29);
      this.btnControl.TabIndex = 7;
      this.btnControl.Text = "Проверка";
      this.btnControl.UseVisualStyleBackColor = true;
      this.btnControl.Click += new EventHandler(this.btnControl_Click);
      this.btnUnLoad.ContextMenuStrip = this.cmsUnLoad;
      this.btnUnLoad.Image = (Image) Resources.save;
      this.btnUnLoad.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnUnLoad.Location = new Point(420, 6);
      this.btnUnLoad.Name = "btnUnLoad";
      this.btnUnLoad.Size = new Size(113, 29);
      this.btnUnLoad.TabIndex = 6;
      this.btnUnLoad.Text = "Выгрузить";
      this.btnUnLoad.TextAlign = ContentAlignment.MiddleRight;
      this.btnUnLoad.UseVisualStyleBackColor = true;
      this.btnUnLoad.Click += new EventHandler(this.btnUnLoad_Click_1);
      this.btnSave.Enabled = false;
      this.btnSave.Image = (Image) Resources.Tick;
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.Location = new Point(303, 6);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(111, 29);
      this.btnSave.TabIndex = 5;
      this.btnSave.Text = "Сохранить";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnLoadPay.Enabled = false;
      this.btnLoadPay.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnLoadPay.Location = new Point(157, 6);
      this.btnLoadPay.Name = "btnLoadPay";
      this.btnLoadPay.Size = new Size(140, 29);
      this.btnLoadPay.TabIndex = 4;
      this.btnLoadPay.Text = "Закачать платежи";
      this.btnLoadPay.TextAlign = ContentAlignment.MiddleRight;
      this.btnLoadPay.UseVisualStyleBackColor = true;
      this.btnLoadPay.Click += new EventHandler(this.btnLoadPay_Click);
      this.btnMake.Image = (Image) Resources.Configure;
      this.btnMake.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnMake.Location = new Point(12, 5);
      this.btnMake.Name = "btnMake";
      this.btnMake.Size = new Size(139, 30);
      this.btnMake.TabIndex = 3;
      this.btnMake.Text = "Сформировать";
      this.btnMake.TextAlign = ContentAlignment.MiddleRight;
      this.btnMake.UseVisualStyleBackColor = true;
      this.btnMake.Click += new EventHandler(this.btnMake_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(1090, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(81, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1186, 436);
      this.Controls.Add((Control) this.dgvRegister);
      this.Controls.Add((Control) this.pnUp);
      this.Controls.Add((Control) this.pnBtn);
      this.KeyPreview = true;
      this.Name = "FrmUZP";
      this.Text = "Реестр удержаний из заработной платы";
      this.Load += new EventHandler(this.FrmUZP_Load);
      this.KeyDown += new KeyEventHandler(this.FrmUZP_KeyDown);
      this.cmsUnLoad.ResumeLayout(false);
      ((ISupportInitialize) this.dgvRegister).EndInit();
      this.pnUp.ResumeLayout(false);
      this.pnUp.PerformLayout();
      this.pnBtn.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
