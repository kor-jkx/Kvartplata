// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmRcptAccounts
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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmRcptAccounts : Form
  {
    private IList<RcptAccounts> _rcptAccountsList = (IList<RcptAccounts>) new List<RcptAccounts>();
    private BindingSource bsRcptAccounts = new BindingSource();
    private IList<Kvartplata.Classes.Receipt> _receipts = (IList<Kvartplata.Classes.Receipt>) new List<Kvartplata.Classes.Receipt>();
    private IList<Kvartplata.Classes.Complex> _complexes = (IList<Kvartplata.Classes.Complex>) new List<Kvartplata.Classes.Complex>();
    private IList<Kvartplata.Classes.ohlAccounts> _ohlAccounts = (IList<Kvartplata.Classes.ohlAccounts>) new List<Kvartplata.Classes.ohlAccounts>();
    private bool _isPast = false;
    private bool serviceReadOnly = false;
    private IContainer components = (IContainer) null;
    private ISession _session;
    private Kvartplata.Classes.Company _company;
    private int _homeId;
    protected RcptAccounts curObject;
    private RcptAccounts newObject;
    public DateTime? _openPeriod;
    private ToolStrip tsMenu;
    private ToolStripButton tsbAdd;
    private ToolStripButton tsbApplay;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private MaskDateColumn maskDateColumn1;
    private MaskDateColumn maskDateColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    private DataGridView dgvRcptAccounts;
    private ToolStripButton tsbCancel;
    private ToolStripButton tsbDelete;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripButton tsbPast;
    private DataGridViewTextBoxColumn Company;
    private DataGridViewTextBoxColumn Address;
    private DataGridViewComboBoxColumn Receipt;
    private DataGridViewComboBoxColumn Complex;
    private DataGridViewTextBoxColumn Priority;
    private MaskDateColumn DBeg;
    private MaskDateColumn DEnd;
    private DataGridViewComboBoxColumn ohlAccounts;
    private DataGridViewTextBoxColumn UName;
    private DataGridViewTextBoxColumn DEdit;
    private Panel panelManag;
    private Button butSearchAcc;
    private Label label1;
    private TextBox tbAccountId;
    private Button butCancelF;

    public FrmRcptAccounts(Kvartplata.Classes.Company company, int homeid)
    {
      this.InitializeComponent();
      this._homeId = homeid;
      this._company = company;
      this._session = Domain.CurrentSession;
      this.CheckAccess();
    }

    private void CheckAccess()
    {
      this.serviceReadOnly = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(33, this._company, false));
      this.tsMenu.Visible = this.serviceReadOnly;
      this.dgvRcptAccounts.ReadOnly = !this.serviceReadOnly;
    }

    private void GetData()
    {
      this._session.Clear();
      string str = " where ra.Company.CompanyId=" + (object) this._company.CompanyId + " ";
      if ((uint) this._homeId > 0U)
        str = str + " and ra.IdHome=" + (object) this._homeId + " ";
      this._rcptAccountsList = this._session.CreateQuery("select ra from RcptAccounts ra " + (!this._isPast ? str + " and ra.DEnd>=:dend" : str + " and (ra.DEnd>=:dend or ra.DEnd<:dend) ") ?? "").SetDateTime("dend", DateTime.Now).List<RcptAccounts>();
      foreach (RcptAccounts rcptAccounts in (IEnumerable<RcptAccounts>) this._rcptAccountsList)
        rcptAccounts.Address = rcptAccounts.IdHome != 0 ? this._session.Get<Home>((object) rcptAccounts.IdHome).Address : "0";
      this.dgvRcptAccounts.AutoGenerateColumns = false;
      this.dgvRcptAccounts.DataSource = (object) null;
      this.dgvRcptAccounts.DataSource = (object) null;
      this.bsRcptAccounts.DataSource = (object) this._rcptAccountsList;
      this.dgvRcptAccounts.DataSource = (object) this.bsRcptAccounts;
      Period closedPeriod = this.GetClosedPeriod();
      this._openPeriod = !closedPeriod.PeriodName.HasValue ? new DateTime?() : new DateTime?(closedPeriod.PeriodName.Value.AddMonths(1));
      this.LoadData();
    }

    protected void SelectRow()
    {
      if (this.curObject != null)
      {
        int curObject = KvrplHelper.FindCurObject((IList) this._rcptAccountsList.ToList<RcptAccounts>(), (object) this.curObject);
        if (curObject < 0)
          return;
        this.dgvRcptAccounts.CurrentCell = this.dgvRcptAccounts.Rows[curObject].Cells[1];
        this.dgvRcptAccounts.Rows[curObject].Selected = true;
      }
      else if (this.dgvRcptAccounts.CurrentRow != null && this.dgvRcptAccounts.CurrentRow.Index < this._rcptAccountsList.Count)
        this.curObject = (RcptAccounts) this.dgvRcptAccounts.Rows[this.dgvRcptAccounts.CurrentRow.Index].DataBoundItem;
    }

    private void LoadData()
    {
      if (this._session == null)
        return;
      try
      {
        this._receipts = this._session.CreateQuery("from Receipt").List<Kvartplata.Classes.Receipt>();
        string str = "where ";
        if (Options.ComplexKapRemont != null && Options.Complex != null && Options.Complex.ComplexId == 100)
          str += "c.ComplexId=100 or c.ComplexId=113";
        if (Options.ComplexKapRemont == null && Options.Complex != null && Options.Complex.ComplexId == 100)
          str += "c.ComplexId=100";
        if (Options.ComplexKapRemont != null && Options.Complex != null && Options.Complex.ComplexId != 100)
          str += "c.ComplexId=113";
        this._complexes = this._session.CreateQuery("select c from Complex c " + str ?? "").List<Kvartplata.Classes.Complex>();
        this._ohlAccounts = this._session.CreateQuery("from ohlAccounts").List<Kvartplata.Classes.ohlAccounts>();
        for (int index = 0; index < this.dgvRcptAccounts.Rows.Count; ++index)
        {
          try
          {
            DataGridViewRow row = this.dgvRcptAccounts.Rows[index];
            ((DataGridViewComboBoxCell) this.dgvRcptAccounts["Receipt", row.Index]).DataSource = (object) this._receipts;
            ((DataGridViewComboBoxCell) this.dgvRcptAccounts["Receipt", row.Index]).DisplayMember = "ReceiptName";
            ((DataGridViewComboBoxCell) this.dgvRcptAccounts["Receipt", row.Index]).ValueMember = "ReceiptId";
            if (row.DataBoundItem != null)
            {
              Kvartplata.Classes.Receipt receipt = ((RcptAccounts) row.DataBoundItem).Receipt;
              if (receipt != null)
                row.Cells["Receipt"].Value = (object) receipt.ReceiptId;
              Kvartplata.Classes.Complex comp = ((RcptAccounts) row.DataBoundItem).Complex;
              if (comp != null)
                row.Cells["Complex"].Value = (object) comp.ComplexId;
              Kvartplata.Classes.ohlAccounts ohlAccounts = ((RcptAccounts) row.DataBoundItem).ohlAccounts;
              if (ohlAccounts != null)
                row.Cells["ohlAccounts"].Value = (object) ohlAccounts.ohlAccountsId;
              ((DataGridViewComboBoxCell) this.dgvRcptAccounts["Complex", row.Index]).DataSource = (object) this._complexes;
              ((DataGridViewComboBoxCell) this.dgvRcptAccounts["Complex", row.Index]).DisplayMember = "ComplexName";
              ((DataGridViewComboBoxCell) this.dgvRcptAccounts["Complex", row.Index]).ValueMember = "ComplexId";
              ((DataGridViewComboBoxCell) this.dgvRcptAccounts["ohlAccounts", row.Index]).DataSource = (object) this._ohlAccounts.Where<Kvartplata.Classes.ohlAccounts>((Func<Kvartplata.Classes.ohlAccounts, bool>) (x => x.ComplexId == comp.ComplexId)).ToList<Kvartplata.Classes.ohlAccounts>();
              ((DataGridViewComboBoxCell) this.dgvRcptAccounts["ohlAccounts", row.Index]).DisplayMember = "ohlAccountsName";
              ((DataGridViewComboBoxCell) this.dgvRcptAccounts["ohlAccounts", row.Index]).ValueMember = "ohlAccountsId";
            }
            if (((RcptAccounts) row.DataBoundItem).DEnd < DateTime.Now)
              row.DefaultCellStyle.ForeColor = Color.Gray;
            else
              row.DefaultCellStyle.BackColor = Color.PapayaWhip;
            row.Selected = true;
          }
          catch (Exception ex)
          {
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
        }
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private Period GetClosedPeriod()
    {
      return this._session.Get<Period>((object) Convert.ToInt32(this._session.CreateQuery(string.Format("select max(p.Period_id) from ClosedPeriod p where (p.Complex_id = {0} or p.Complex_id = {2}) and p.Company_id = {1}", (object) Options.ComplexPasp.ComplexId, (object) this._company.CompanyId, (object) Options.ComplexPrior.IdFk)).UniqueResult()));
    }

    private void FrmRcptAccounts_Load(object sender, EventArgs e)
    {
      this.GetData();
    }

    private void tsbAdd_Click(object sender, EventArgs e)
    {
      this.newObject = new RcptAccounts();
      this.newObject.Company = this._company;
      if (Options.ComplexKapRemont != null && Options.Complex != null && Options.Complex.ComplexId == 100)
        this.newObject.Complex = this._complexes.First<Kvartplata.Classes.Complex>((Func<Kvartplata.Classes.Complex, bool>) (x => x.ComplexId == 100));
      if (Options.ComplexKapRemont == null && Options.Complex != null && Options.Complex.ComplexId == 100)
        this.newObject.Complex = this._complexes.First<Kvartplata.Classes.Complex>((Func<Kvartplata.Classes.Complex, bool>) (x => x.ComplexId == 100));
      if (Options.ComplexKapRemont != null && Options.Complex != null && Options.Complex.ComplexId != 100)
        this.newObject.Complex = this._complexes.First<Kvartplata.Classes.Complex>((Func<Kvartplata.Classes.Complex, bool>) (x => x.ComplexId == 113));
      this.newObject.Receipt = this._receipts.FirstOrDefault<Kvartplata.Classes.Receipt>();
      this.newObject.ohlAccounts = this._ohlAccounts.Where<Kvartplata.Classes.ohlAccounts>((Func<Kvartplata.Classes.ohlAccounts, bool>) (x => x.ComplexId == this.newObject.Complex.ComplexId)).FirstOrDefault<Kvartplata.Classes.ohlAccounts>();
      this.newObject.DBeg = !this._openPeriod.HasValue ? DateTime.Now : this._openPeriod.Value;
      this.newObject.DEnd = Convert.ToDateTime("31.12.2999");
      this.newObject.IdHome = this._homeId;
      this.newObject.UName = Options.Login;
      this.newObject.DEdit = DateTime.Now.Date;
      this.newObject.Address = this.newObject.IdHome != 0 ? this._session.Get<Home>((object) this.newObject.IdHome).Address : "0";
      this.bsRcptAccounts.Add((object) this.newObject);
      this.LoadData();
      this.tsbAdd.Enabled = false;
      this.tsbApplay.Enabled = true;
      this.tsbCancel.Enabled = true;
      this.tsbDelete.Enabled = false;
      this.tsbPast.Enabled = false;
    }

    private void tsbApplay_Click(object sender, EventArgs e)
    {
      this.dgvRcptAccounts.EndEdit();
      if (this.newObject != null)
      {
        try
        {
          RcptAccounts dataBoundItem = (RcptAccounts) this.dgvRcptAccounts.CurrentRow.DataBoundItem;
          dataBoundItem.Receipt = this._session.Get<Kvartplata.Classes.Receipt>((object) Convert.ToInt16(this.dgvRcptAccounts.CurrentRow.Cells["Receipt"].Value));
          if (dataBoundItem.Receipt == null)
          {
            int num = (int) MessageBox.Show("Выберете квитанцию", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          dataBoundItem.Complex = this._session.Get<Kvartplata.Classes.Complex>((object) Convert.ToInt32(this.dgvRcptAccounts.CurrentRow.Cells["Complex"].Value));
          if (dataBoundItem.Complex == null)
          {
            int num = (int) MessageBox.Show("Выберете комплекс", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          dataBoundItem.ohlAccounts = this._session.Get<Kvartplata.Classes.ohlAccounts>((object) Convert.ToInt32(this.dgvRcptAccounts.CurrentRow.Cells["ohlAccounts"].Value));
          if (dataBoundItem.ohlAccounts == null)
          {
            int num = (int) MessageBox.Show("Выберете счет", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          IList<RcptAccounts> rcptAccountsList = this._session.CreateQuery("select r from RcptAccounts r where r.Company=:comp and r.Receipt=:rec and r.IdHome=:idh and r.Complex=:com and r.Priorites=:p and r.DBeg=:db and r.DEnd=:de").SetParameter<Kvartplata.Classes.Company>("comp", dataBoundItem.Company).SetParameter<Kvartplata.Classes.Receipt>("rec", dataBoundItem.Receipt).SetParameter<int>("idh", dataBoundItem.IdHome).SetParameter<Kvartplata.Classes.Complex>("com", dataBoundItem.Complex).SetParameter<short>("p", dataBoundItem.Priorites).SetParameter<DateTime>("db", dataBoundItem.DBeg).SetParameter<DateTime>("de", dataBoundItem.DEnd).List<RcptAccounts>();
          if (rcptAccountsList != null && rcptAccountsList.Count > 0)
          {
            int num = (int) MessageBox.Show("Запись существует!", "Ошибка!");
            return;
          }
          DateTime dateTime;
          int num1;
          if (dataBoundItem.DBeg.Day != 1)
          {
            int month1 = dataBoundItem.DBeg.Month;
            dateTime = this._openPeriod.Value;
            int month2 = dateTime.Month;
            if (month1 < month2)
            {
              dateTime = dataBoundItem.DBeg;
              int year1 = dateTime.Year;
              dateTime = this._openPeriod.Value;
              int year2 = dateTime.Year;
              num1 = year1 < year2 ? 1 : 0;
              goto label_13;
            }
          }
          num1 = 0;
label_13:
          if (num1 != 0)
          {
            int num2 = (int) MessageBox.Show("Неверная дата начала", "Ошибка!");
            return;
          }
          if (dataBoundItem.DBeg > dataBoundItem.DEnd)
          {
            int num2 = (int) MessageBox.Show("Неверная дата окончания", "Ошибка!");
            return;
          }
          dataBoundItem.UName = Options.Login;
          RcptAccounts rcptAccounts1 = dataBoundItem;
          dateTime = DateTime.Now;
          DateTime date = dateTime.Date;
          rcptAccounts1.DEdit = date;
          this._session.Save((object) dataBoundItem);
          RcptAccounts rcptAccounts2 = this._session.CreateQuery("select r from RcptAccounts r where r.Company=:comp and r.Receipt=:rec and r.IdHome=:idh and r.Complex=:com and r.Priorites=:p and r.DBeg<:db and r.DEnd>:de ").SetParameter<Kvartplata.Classes.Company>("comp", dataBoundItem.Company).SetParameter<Kvartplata.Classes.Receipt>("rec", dataBoundItem.Receipt).SetParameter<int>("idh", dataBoundItem.IdHome).SetParameter<Kvartplata.Classes.Complex>("com", dataBoundItem.Complex).SetParameter<short>("p", dataBoundItem.Priorites).SetParameter<DateTime>("db", dataBoundItem.DBeg).SetParameter<DateTime>("de", dataBoundItem.DBeg).UniqueResult<RcptAccounts>();
          if (rcptAccounts2 != null)
          {
            ISQLQuery sqlQuery = this._session.CreateSQLQuery("update DBA.rcptAccounts rcpt set DEnd=:de,UName=:un,Dedit=:edt where Company_id=:comp_id1 and IdHome=:idH1 and Receipt_id=:rid1 and Complex_id=:cid1 and Priority=:p1 and DBeg=:db1 ");
            string name = "de";
            dateTime = dataBoundItem.DBeg;
            DateTime val = KvrplHelper.LastDay(dateTime.AddMonths(-1));
            sqlQuery.SetParameter<DateTime>(name, val).SetParameter<string>("un", dataBoundItem.UName).SetParameter<DateTime>("edt", dataBoundItem.DEdit).SetDateTime("db1", rcptAccounts2.DBeg).SetParameter<short>("comp_id1", rcptAccounts2.Company.CompanyId).SetParameter<int>("idH1", rcptAccounts2.IdHome).SetParameter<short>("rid1", rcptAccounts2.Receipt.ReceiptId).SetParameter<int>("cid1", rcptAccounts2.Complex.ComplexId).SetParameter<short>("p1", rcptAccounts2.Priorites).ExecuteUpdate();
          }
          this._session.Flush();
          this.newObject = (RcptAccounts) null;
        }
        catch (Exception ex)
        {
          this._session.Clear();
          int num = (int) MessageBox.Show("Ошибка сохранения!", "Ошибка!");
          KvrplHelper.WriteLog(ex, (LsClient) null);
          this.bsRcptAccounts.RemoveAt(this.bsRcptAccounts.Count - 1);
          return;
        }
      }
      else
      {
        try
        {
          RcptAccounts dataBoundItem = (RcptAccounts) this.dgvRcptAccounts.CurrentRow.DataBoundItem;
          dataBoundItem.Receipt = this._session.Get<Kvartplata.Classes.Receipt>((object) Convert.ToInt16(this.dgvRcptAccounts.CurrentRow.Cells["Receipt"].Value));
          if (dataBoundItem.Receipt == null)
          {
            int num = (int) MessageBox.Show("Выберете квитанцию", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          dataBoundItem.Complex = this._session.Get<Kvartplata.Classes.Complex>((object) Convert.ToInt32(this.dgvRcptAccounts.CurrentRow.Cells["Complex"].Value));
          if (dataBoundItem.Complex == null)
          {
            int num = (int) MessageBox.Show("Выберете комплекс", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          dataBoundItem.ohlAccounts = this._session.Get<Kvartplata.Classes.ohlAccounts>((object) Convert.ToInt32(this.dgvRcptAccounts.CurrentRow.Cells["ohlAccounts"].Value));
          if (dataBoundItem.ohlAccounts == null)
          {
            int num = (int) MessageBox.Show("Выберете счет", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          if (dataBoundItem.ohlAccounts.ComplexId != dataBoundItem.Complex.ComplexId)
          {
            int num = (int) MessageBox.Show("Выберете счет", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          IList<RcptAccounts> rcptAccountsList = this._session.CreateQuery("select r from RcptAccounts r where r.Company=:comp and r.Receipt=:rec and r.IdHome=:idh and r.Complex=:com and r.Priorites=:p and r.DBeg=:db").SetParameter<Kvartplata.Classes.Company>("comp", dataBoundItem.Company).SetParameter<Kvartplata.Classes.Receipt>("rec", dataBoundItem.Receipt).SetParameter<int>("idh", dataBoundItem.IdHome).SetParameter<Kvartplata.Classes.Complex>("com", dataBoundItem.Complex).SetParameter<short>("p", dataBoundItem.Priorites).SetParameter<DateTime>("db", dataBoundItem.DBeg).List<RcptAccounts>();
          if (rcptAccountsList != null && rcptAccountsList.Count > 0)
          {
            int num = (int) MessageBox.Show("Запись существует!", "Ошибка!");
            return;
          }
          DateTime dateTime;
          int num1;
          if (dataBoundItem.DBeg.Day != 1)
          {
            int month1 = dataBoundItem.DBeg.Month;
            dateTime = this._openPeriod.Value;
            int month2 = dateTime.Month;
            if (month1 < month2)
            {
              dateTime = dataBoundItem.DBeg;
              int year1 = dateTime.Year;
              dateTime = this._openPeriod.Value;
              int year2 = dateTime.Year;
              num1 = year1 < year2 ? 1 : 0;
              goto label_35;
            }
          }
          num1 = 0;
label_35:
          if (num1 != 0)
          {
            int num2 = (int) MessageBox.Show("Неверная дата начала", "Ошибка!");
            return;
          }
          if (dataBoundItem.DBeg > dataBoundItem.DEnd)
          {
            int num2 = (int) MessageBox.Show("Неверная дата окончания", "Ошибка!");
            return;
          }
          dataBoundItem.UName = Options.Login;
          RcptAccounts rcptAccounts = dataBoundItem;
          dateTime = DateTime.Now;
          DateTime date = dateTime.Date;
          rcptAccounts.DEdit = date;
          this._session.CreateSQLQuery("update DBA.rcptAccounts rcpt set Company_id=:comp_id, IdHome=:idH, Receipt_id=:rid, Complex_id=:cid, Priority=:p,DBeg=:db,DEnd=:de, Account_id=:accid,UName=:un,Dedit=:edt where Company_id=:comp_id1 and IdHome=:idH1 and Receipt_id=:rid1 and Complex_id=:cid1 and Priority=:p1 and DBeg=:db1 ").SetDateTime("db", dataBoundItem.DBeg).SetParameter<short>("comp_id", dataBoundItem.Company.CompanyId).SetParameter<int>("idH", dataBoundItem.IdHome).SetParameter<short>("rid", dataBoundItem.Receipt.ReceiptId).SetParameter<int>("cid", dataBoundItem.Complex.ComplexId).SetParameter<short>("p", dataBoundItem.Priorites).SetParameter<DateTime>("de", dataBoundItem.DEnd).SetParameter<int>("accid", dataBoundItem.ohlAccounts.ohlAccountsId).SetParameter<string>("un", dataBoundItem.UName).SetParameter<DateTime>("edt", dataBoundItem.DEdit).SetDateTime("db1", this.curObject.DBeg).SetParameter<short>("comp_id1", this.curObject.Company.CompanyId).SetParameter<int>("idH1", this.curObject.IdHome).SetParameter<short>("rid1", this.curObject.Receipt.ReceiptId).SetParameter<int>("cid1", this.curObject.Complex.ComplexId).SetParameter<short>("p1", this.curObject.Priorites).ExecuteUpdate();
          this.curObject = (RcptAccounts) null;
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Ошибка сохранения!", "Ошибка!");
          KvrplHelper.WriteLog(ex, (LsClient) null);
          return;
        }
      }
      this.GetData();
      this.tsbAdd.Enabled = true;
      this.tsbApplay.Enabled = false;
      this.tsbCancel.Enabled = false;
      this.tsbDelete.Enabled = true;
      this.tsbPast.Enabled = true;
    }

    private void tsbCancel_Click(object sender, EventArgs e)
    {
      this.dgvRcptAccounts.EndEdit();
      try
      {
        if (this.newObject != null)
        {
          this.bsRcptAccounts.RemoveAt(this.bsRcptAccounts.Count - 1);
          this.newObject = (RcptAccounts) null;
        }
        else
          this.GetData();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      this.tsbAdd.Enabled = true;
      this.tsbApplay.Enabled = false;
      this.tsbCancel.Enabled = false;
      this.tsbDelete.Enabled = true;
      this.tsbPast.Enabled = true;
    }

    private void tsbDelete_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.No)
        return;
      try
      {
        this._session.Delete((object) (RcptAccounts) this.dgvRcptAccounts.CurrentRow.DataBoundItem);
        this._session.Flush();
        this.bsRcptAccounts.RemoveCurrent();
        this.bsRcptAccounts.EndEdit();
        this.dgvRcptAccounts.Update();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void dgvRcptAccounts_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      if (this.curObject == null)
        this.curObject = ((RcptAccounts) this.dgvRcptAccounts.CurrentRow.DataBoundItem).Clone();
      this.tsbAdd.Enabled = false;
      this.tsbApplay.Enabled = true;
      this.tsbCancel.Enabled = true;
      this.tsbDelete.Enabled = false;
      this.tsbPast.Enabled = false;
    }

    private void dgvRcptAccounts_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      try
      {
        if (e.ColumnIndex != this.dgvRcptAccounts.Columns["Complex"].Index)
          return;
        this.dgvRcptAccounts.EndEdit();
        Kvartplata.Classes.Complex TempComplex = this._session.Get<Kvartplata.Classes.Complex>((object) Convert.ToInt32(this.dgvRcptAccounts.CurrentRow.Cells["Complex"].Value));
        ((DataGridViewComboBoxCell) this.dgvRcptAccounts["ohlAccounts", e.RowIndex]).DataSource = (object) null;
        ((DataGridViewComboBoxCell) this.dgvRcptAccounts["ohlAccounts", e.RowIndex]).DataSource = (object) this._ohlAccounts.Where<Kvartplata.Classes.ohlAccounts>((Func<Kvartplata.Classes.ohlAccounts, bool>) (x => x.ComplexId == TempComplex.ComplexId)).ToList<Kvartplata.Classes.ohlAccounts>();
        ((DataGridViewComboBoxCell) this.dgvRcptAccounts["ohlAccounts", e.RowIndex]).DisplayMember = "ohlAccountsName";
        ((DataGridViewComboBoxCell) this.dgvRcptAccounts["ohlAccounts", e.RowIndex]).ValueMember = "ohlAccountsId";
        if (this._ohlAccounts.FirstOrDefault<Kvartplata.Classes.ohlAccounts>() == null)
          return;
        this.bsRcptAccounts.ResetItem(e.RowIndex);
        this.dgvRcptAccounts.Refresh();
      }
      catch (Exception ex)
      {
      }
    }

    private void dgvRcptAccounts_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
    }

    private void tsbPast_Click(object sender, EventArgs e)
    {
      if (!this._isPast)
      {
        this.tsbPast.BackColor = Color.DarkOrange;
        this._isPast = true;
      }
      else
      {
        this.tsbPast.BackColor = this.BackColor;
        this._isPast = false;
      }
      this.GetData();
    }

    private void butSearchAcc_Click(object sender, EventArgs e)
    {
      int index = this.dgvRcptAccounts.CurrentRow.Index;
      Kvartplata.Classes.Complex TempComplex = this._session.Get<Kvartplata.Classes.Complex>((object) Convert.ToInt32(this.dgvRcptAccounts.CurrentRow.Cells["Complex"].Value));
      List<Kvartplata.Classes.ohlAccounts> list = ((IEnumerable<Kvartplata.Classes.ohlAccounts>) this._ohlAccounts.Where<Kvartplata.Classes.ohlAccounts>((Func<Kvartplata.Classes.ohlAccounts, bool>) (x =>
      {
        if (x.ohlAccountsName.Contains(this.tbAccountId.Text))
          return x.ComplexId == TempComplex.ComplexId;
        return false;
      })).ToArray<Kvartplata.Classes.ohlAccounts>()).ToList<Kvartplata.Classes.ohlAccounts>();
      if (list.Count == 0)
      {
        int num = (int) MessageBox.Show("Не найдено соответствие.", "Ошибка!");
      }
      else
      {
        ((DataGridViewComboBoxCell) this.dgvRcptAccounts["ohlAccounts", index]).DataSource = (object) null;
        ((DataGridViewComboBoxCell) this.dgvRcptAccounts["ohlAccounts", index]).DataSource = (object) list;
        ((DataGridViewComboBoxCell) this.dgvRcptAccounts["ohlAccounts", index]).DisplayMember = "ohlAccountsName";
        ((DataGridViewComboBoxCell) this.dgvRcptAccounts["ohlAccounts", index]).ValueMember = "ohlAccountsId";
      }
    }

    private void butCancelF_Click(object sender, EventArgs e)
    {
      this.tbAccountId.Text = "";
      this.butSearchAcc_Click((object) null, (EventArgs) null);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmRcptAccounts));
      this.dgvRcptAccounts = new DataGridView();
      this.Company = new DataGridViewTextBoxColumn();
      this.Address = new DataGridViewTextBoxColumn();
      this.Receipt = new DataGridViewComboBoxColumn();
      this.Complex = new DataGridViewComboBoxColumn();
      this.Priority = new DataGridViewTextBoxColumn();
      this.DBeg = new MaskDateColumn();
      this.DEnd = new MaskDateColumn();
      this.ohlAccounts = new DataGridViewComboBoxColumn();
      this.UName = new DataGridViewTextBoxColumn();
      this.DEdit = new DataGridViewTextBoxColumn();
      this.tsMenu = new ToolStrip();
      this.tsbAdd = new ToolStripButton();
      this.tsbApplay = new ToolStripButton();
      this.tsbCancel = new ToolStripButton();
      this.tsbDelete = new ToolStripButton();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.tsbPast = new ToolStripButton();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
      this.maskDateColumn1 = new MaskDateColumn();
      this.maskDateColumn2 = new MaskDateColumn();
      this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
      this.panelManag = new Panel();
      this.butSearchAcc = new Button();
      this.label1 = new Label();
      this.tbAccountId = new TextBox();
      this.butCancelF = new Button();
      ((ISupportInitialize) this.dgvRcptAccounts).BeginInit();
      this.tsMenu.SuspendLayout();
      this.panelManag.SuspendLayout();
      this.SuspendLayout();
      this.dgvRcptAccounts.AllowUserToAddRows = false;
      this.dgvRcptAccounts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dgvRcptAccounts.BackgroundColor = Color.AliceBlue;
      this.dgvRcptAccounts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvRcptAccounts.Columns.AddRange((DataGridViewColumn) this.Company, (DataGridViewColumn) this.Address, (DataGridViewColumn) this.Receipt, (DataGridViewColumn) this.Complex, (DataGridViewColumn) this.Priority, (DataGridViewColumn) this.DBeg, (DataGridViewColumn) this.DEnd, (DataGridViewColumn) this.ohlAccounts, (DataGridViewColumn) this.UName, (DataGridViewColumn) this.DEdit);
      this.dgvRcptAccounts.Location = new Point(0, 65);
      this.dgvRcptAccounts.MultiSelect = false;
      this.dgvRcptAccounts.Name = "dgvRcptAccounts";
      this.dgvRcptAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgvRcptAccounts.Size = new Size(1184, 568);
      this.dgvRcptAccounts.TabIndex = 3;
      this.dgvRcptAccounts.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvRcptAccounts_CellBeginEdit);
      this.dgvRcptAccounts.CellValueChanged += new DataGridViewCellEventHandler(this.dgvRcptAccounts_CellValueChanged);
      this.dgvRcptAccounts.DataError += new DataGridViewDataErrorEventHandler(this.dgvRcptAccounts_DataError);
      this.Company.DataPropertyName = "CompanyName";
      this.Company.HeaderText = "Компания";
      this.Company.Name = "Company";
      this.Company.ReadOnly = true;
      this.Address.DataPropertyName = "Address";
      this.Address.HeaderText = "Адрес дома";
      this.Address.Name = "Address";
      this.Address.ReadOnly = true;
      this.Receipt.DisplayStyleForCurrentCellOnly = true;
      this.Receipt.HeaderText = "Квитанция";
      this.Receipt.Name = "Receipt";
      this.Complex.DisplayStyleForCurrentCellOnly = true;
      this.Complex.HeaderText = "Комплекс";
      this.Complex.Name = "Complex";
      this.Priority.DataPropertyName = "Priorites";
      this.Priority.HeaderText = "Приоритет";
      this.Priority.Name = "Priority";
      this.DBeg.DataPropertyName = "DBeg";
      this.DBeg.HeaderText = "Дата начала";
      this.DBeg.Name = "DBeg";
      this.DEnd.DataPropertyName = "DEnd";
      this.DEnd.HeaderText = "Дата окончания";
      this.DEnd.Name = "DEnd";
      this.ohlAccounts.DisplayStyleForCurrentCellOnly = true;
      this.ohlAccounts.HeaderText = "Код счета";
      this.ohlAccounts.Name = "ohlAccounts";
      this.UName.DataPropertyName = "UName";
      this.UName.HeaderText = "Пользователь";
      this.UName.Name = "UName";
      this.UName.ReadOnly = true;
      this.DEdit.DataPropertyName = "DEdit";
      this.DEdit.HeaderText = "Дата редактирования";
      this.DEdit.Name = "DEdit";
      this.DEdit.ReadOnly = true;
      this.tsMenu.Font = new Font("Tahoma", 10f);
      this.tsMenu.Items.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.tsbAdd,
        (ToolStripItem) this.tsbApplay,
        (ToolStripItem) this.tsbCancel,
        (ToolStripItem) this.tsbDelete,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.tsbPast
      });
      this.tsMenu.Location = new Point(0, 0);
      this.tsMenu.Name = "tsMenu";
      this.tsMenu.Size = new Size(1184, 25);
      this.tsMenu.TabIndex = 2;
      this.tsMenu.Text = "toolStrip1";
      this.tsbAdd.Image = (Image) Resources.add_var;
      this.tsbAdd.ImageTransparentColor = Color.Magenta;
      this.tsbAdd.Name = "tsbAdd";
      this.tsbAdd.Size = new Size(91, 22);
      this.tsbAdd.Text = "Добавить";
      this.tsbAdd.Click += new EventHandler(this.tsbAdd_Click);
      this.tsbApplay.Enabled = false;
      this.tsbApplay.Image = (Image) Resources.Applay_var;
      this.tsbApplay.ImageTransparentColor = Color.Magenta;
      this.tsbApplay.Name = "tsbApplay";
      this.tsbApplay.Size = new Size(99, 22);
      this.tsbApplay.Text = "Сохранить";
      this.tsbApplay.Click += new EventHandler(this.tsbApplay_Click);
      this.tsbCancel.Enabled = false;
      this.tsbCancel.Image = (Image) Resources.undo;
      this.tsbCancel.ImageTransparentColor = Color.Magenta;
      this.tsbCancel.Name = "tsbCancel";
      this.tsbCancel.Size = new Size(77, 22);
      this.tsbCancel.Text = "Отмена";
      this.tsbCancel.Click += new EventHandler(this.tsbCancel_Click);
      this.tsbDelete.Image = (Image) Resources.delete_var;
      this.tsbDelete.ImageTransparentColor = Color.Magenta;
      this.tsbDelete.Name = "tsbDelete";
      this.tsbDelete.Size = new Size(82, 22);
      this.tsbDelete.Text = "Удалить";
      this.tsbDelete.Click += new EventHandler(this.tsbDelete_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(6, 25);
      this.tsbPast.ImageTransparentColor = Color.Magenta;
      this.tsbPast.Name = "tsbPast";
      this.tsbPast.Size = new Size(51, 22);
      this.tsbPast.Text = "Архив";
      this.tsbPast.Click += new EventHandler(this.tsbPast_Click);
      this.dataGridViewTextBoxColumn1.DataPropertyName = "CompanyName";
      this.dataGridViewTextBoxColumn1.HeaderText = "Компания";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.ReadOnly = true;
      this.dataGridViewTextBoxColumn2.DataPropertyName = "IdHome";
      this.dataGridViewTextBoxColumn2.HeaderText = "Код дома";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn2.ReadOnly = true;
      this.dataGridViewTextBoxColumn3.DataPropertyName = "Priority";
      this.dataGridViewTextBoxColumn3.HeaderText = "Приоритет";
      this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
      this.maskDateColumn1.DataPropertyName = "DBeg";
      this.maskDateColumn1.HeaderText = "Дата начала";
      this.maskDateColumn1.Name = "maskDateColumn1";
      this.maskDateColumn2.DataPropertyName = "DEnd";
      this.maskDateColumn2.HeaderText = "Дата окончания";
      this.maskDateColumn2.Name = "maskDateColumn2";
      this.dataGridViewTextBoxColumn4.DataPropertyName = "UName";
      this.dataGridViewTextBoxColumn4.HeaderText = "Пользователь";
      this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
      this.dataGridViewTextBoxColumn4.ReadOnly = true;
      this.dataGridViewTextBoxColumn5.DataPropertyName = "DEdit";
      this.dataGridViewTextBoxColumn5.HeaderText = "Дата редактирования";
      this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
      this.dataGridViewTextBoxColumn5.ReadOnly = true;
      this.panelManag.Controls.Add((Control) this.butCancelF);
      this.panelManag.Controls.Add((Control) this.butSearchAcc);
      this.panelManag.Controls.Add((Control) this.label1);
      this.panelManag.Controls.Add((Control) this.tbAccountId);
      this.panelManag.Dock = DockStyle.Top;
      this.panelManag.Location = new Point(0, 25);
      this.panelManag.Name = "panelManag";
      this.panelManag.Size = new Size(1184, 42);
      this.panelManag.TabIndex = 4;
      this.butSearchAcc.Image = (Image) Resources.search_24;
      this.butSearchAcc.Location = new Point(442, 6);
      this.butSearchAcc.Name = "butSearchAcc";
      this.butSearchAcc.Size = new Size(40, 30);
      this.butSearchAcc.TabIndex = 2;
      this.butSearchAcc.UseVisualStyleBackColor = true;
      this.butSearchAcc.Click += new EventHandler(this.butSearchAcc_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(153, 17);
      this.label1.TabIndex = 1;
      this.label1.Text = "Фильтр по коду счета";
      this.tbAccountId.Location = new Point(171, 10);
      this.tbAccountId.Name = "tbAccountId";
      this.tbAccountId.Size = new Size(265, 24);
      this.tbAccountId.TabIndex = 0;
      this.butCancelF.Image = (Image) Resources.delete;
      this.butCancelF.Location = new Point(488, 6);
      this.butCancelF.Name = "butCancelF";
      this.butCancelF.Size = new Size(40, 30);
      this.butCancelF.TabIndex = 3;
      this.butCancelF.UseVisualStyleBackColor = true;
      this.butCancelF.Click += new EventHandler(this.butCancelF_Click);
      this.AutoScaleDimensions = new SizeF(7f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1184, 633);
      this.Controls.Add((Control) this.panelManag);
      this.Controls.Add((Control) this.dgvRcptAccounts);
      this.Controls.Add((Control) this.tsMenu);
      this.Font = new Font("Tahoma", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = "FrmRcptAccounts";
      this.Text = "Квитанции и счета";
      this.Load += new EventHandler(this.FrmRcptAccounts_Load);
      ((ISupportInitialize) this.dgvRcptAccounts).EndInit();
      this.tsMenu.ResumeLayout(false);
      this.tsMenu.PerformLayout();
      this.panelManag.ResumeLayout(false);
      this.panelManag.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
