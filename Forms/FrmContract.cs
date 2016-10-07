// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmContract
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
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmContract : FrmBaseForm
  {
    private FormStateSaver fss = new FormStateSaver(FrmContract.ic);
    private bool _readOnly = false;
    private IContainer components = (IContainer) null;
    private ISession session;
    private static IContainer ic;
    private Period monthClosed;
    private DateTime lastDayMonthClosed;
    private Contract oldContract;
    private IList<Contract> oldContracts;
    private Company _company;
    private Panel pnBtn;
    private Button btnExit;
    private Panel pnUp;
    private Label lblOrg;
    private ComboBox cmbOrg;
    private DataGridView dgvContract;
    private ToolStrip ts;
    private ToolStripButton tsbAdd;
    private ToolStripButton tsbApplay;
    private ToolStripButton tsbCancel;
    private ToolStripButton tsbDelete;
    private ComboBox cmbManager;
    private Label lblManager;

    public FrmContract(Company comapny)
    {
      this.InitializeComponent();
      this.CheckAccess();
      this._company = comapny;
      this.fss.ParentForm = (Form) this;
      this.session = Domain.CurrentSession;
    }

    private void CheckAccess()
    {
      this._readOnly = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(33, this._company, false));
      this.ts.Visible = this._readOnly;
      this.dgvContract.ReadOnly = !this._readOnly;
    }

    private void LoadManager()
    {
      IList<BaseOrg> baseOrgList = (IList<BaseOrg>) new List<BaseOrg>();
      this.cmbManager.DataSource = (object) this.session.CreateQuery("select new BaseOrg(b.BaseOrgId,b.NameOrgMin) from BaseOrg b where b.BaseOrgId in (select Manager.BaseOrgId from Company) and b.BaseOrgId<>0 order by b.BaseOrgName").List<BaseOrg>();
      this.cmbManager.DisplayMember = "NameOrgMin";
      this.cmbManager.ValueMember = "BaseOrgId";
      BaseOrg baseOrg1 = new BaseOrg();
      BaseOrg baseOrg2;
      try
      {
        baseOrg2 = this.session.Get<BaseOrg>((object) Options.Company.Manager.BaseOrgId);
      }
      catch (Exception ex)
      {
        baseOrg2 = this.session.CreateQuery(string.Format("select c.Manager from Company c where c.CompanyId={0}", (object) Options.Company.CompanyId)).List<BaseOrg>()[0];
      }
      if (baseOrg2 == null)
        return;
      this.cmbManager.SelectedValue = (object) baseOrg2.BaseOrgId;
    }

    private void LoadOrg()
    {
      IList<BaseOrg> baseOrgList1 = (IList<BaseOrg>) new List<BaseOrg>();
      IList<BaseOrg> baseOrgList2 = this.session.CreateQuery("select new BaseOrg(b.BaseOrgId,b.NameOrgMin) from BaseOrg b order by b.NameOrgMin").List<BaseOrg>();
      baseOrgList2.Insert(0, new BaseOrg(0, ""));
      this.cmbOrg.DataSource = (object) baseOrgList2;
      this.cmbOrg.DisplayMember = "NameOrgMin";
      this.cmbOrg.ValueMember = "BaseOrgId";
    }

    private void LoadContract()
    {
      this.dgvContract.DataSource = (object) null;
      this.dgvContract.Columns.Clear();
      this.session.Clear();
      string str = "";
      if (this.cmbOrg.SelectedValue != null && (uint) Convert.ToInt32(this.cmbOrg.SelectedValue) > 0U)
        str = string.Format(" and BaseOrg.BaseOrgId={0}", this.cmbOrg.SelectedValue);
      this.oldContracts = this.session.CreateQuery(string.Format("select c from Contract c where Manager.BaseOrgId={0}" + str + " order by DBeg desc", (object) ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId)).List<Contract>();
      this.session.Clear();
      this.dgvContract.DataSource = (object) this.session.CreateQuery(string.Format("select c from Contract c where Manager.BaseOrgId={0}" + str + " order by DBeg desc", (object) ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId)).List<Contract>();
      this.SetViewContract();
      if (this.dgvContract.Rows.Count <= 0 || this.dgvContract.CurrentRow == null)
        return;
      this.dgvContract.CurrentCell = this.dgvContract.Rows[0].Cells["Org"];
    }

    private void SetViewContract()
    {
      this.dgvContract.Columns["ContractId"].Visible = false;
      this.dgvContract.Columns["ContractNum"].HeaderText = "Номер договора";
      KvrplHelper.AddComboBoxColumn(this.dgvContract, 0, (IList) this.session.CreateQuery("select new BaseOrg(b.BaseOrgId,b.NameOrgMin) from BaseOrg b order by b.NameOrgMin").List<BaseOrg>(), "BaseOrgId", "NameOrgMin", "Организация", "Org", 7, 300);
      KvrplHelper.AddMaskDateColumn(this.dgvContract, 1, "Дата начала", "DBeg");
      KvrplHelper.AddMaskDateColumn(this.dgvContract, 2, "Дата окончания", "DEnd");
      KvrplHelper.ViewEdit(this.dgvContract);
      foreach (DataGridViewRow row in (IEnumerable) this.dgvContract.Rows)
      {
        row.Cells["DBeg"].Value = (object) ((Contract) row.DataBoundItem).DBeg;
        row.Cells["DEnd"].Value = (object) ((Contract) row.DataBoundItem).DEnd;
        if (((Contract) row.DataBoundItem).BaseOrg != null)
          row.Cells["Org"].Value = (object) ((Contract) row.DataBoundItem).BaseOrg.BaseOrgId;
      }
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void cmbOrg_SelectionChangeCommitted(object sender, EventArgs e)
    {
      try
      {
        this.monthClosed = this.session.CreateQuery(string.Format("select c.Period from CompanyPeriod c where c.Period.PeriodId=(select min(Period.PeriodId) from CompanyPeriod where Company.CompanyId in (select CompanyId from Company where Manager.BaseOrgId={0})) ", (object) ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId)).List<Period>()[0];
      }
      catch
      {
        this.monthClosed = this.session.CreateQuery(string.Format("select c.Period from CompanyPeriod c where c.Period.PeriodId=(select min(Period.PeriodId) from CompanyPeriod) ")).List<Period>()[0];
      }
      this.LoadContract();
    }

    private void tsbAdd_Click(object sender, EventArgs e)
    {
      Contract contract = new Contract();
      if (this.cmbOrg.SelectedValue != null && (uint) Convert.ToInt32(this.cmbOrg.SelectedValue) > 0U)
        contract.BaseOrg = (BaseOrg) this.cmbOrg.SelectedItem;
      contract.DBeg = this.monthClosed.PeriodName.Value.AddMonths(1);
      contract.DEnd = Convert.ToDateTime("2999-12-31");
      contract.IsEdit = true;
      IList<Contract> contractList = (IList<Contract>) new List<Contract>();
      if ((uint) this.dgvContract.Rows.Count > 0U)
        contractList = (IList<Contract>) (this.dgvContract.DataSource as List<Contract>);
      contractList.Add(contract);
      this.dgvContract.DataSource = (object) null;
      this.dgvContract.Columns.Clear();
      this.dgvContract.DataSource = (object) contractList;
      this.SetViewContract();
      this.dgvContract.CurrentCell = this.dgvContract.Rows[this.dgvContract.Rows.Count - 1].Cells["Org"];
      this.tsbApplay.Enabled = true;
      this.tsbCancel.Enabled = true;
      this.tsbDelete.Enabled = false;
    }

    private void tsbApplay_Click(object sender, EventArgs e)
    {
      bool flag = false;
      foreach (DataGridViewRow row in (IEnumerable) this.dgvContract.Rows)
      {
        this.dgvContract.CurrentCell = row.Cells["Org"];
        row.Selected = true;
        if (((Contract) row.DataBoundItem).IsEdit)
        {
          foreach (Contract oldContract in (IEnumerable<Contract>) this.oldContracts)
          {
            if (oldContract.ContractId == ((Contract) row.DataBoundItem).ContractId)
              this.oldContract = oldContract;
          }
          if (!this.SaveContract())
            flag = true;
          else
            ((Contract) row.DataBoundItem).IsEdit = false;
        }
      }
      if (flag)
        return;
      this.LoadContract();
    }

    private bool SaveContract()
    {
      if (this.dgvContract.Rows.Count > 0 && this.dgvContract.CurrentRow.Index >= 0)
      {
        this.dgvContract.CommitEdit(DataGridViewDataErrorContexts.Commit);
        bool flag1 = false;
        Contract dataBoundItem = (Contract) this.dgvContract.CurrentRow.DataBoundItem;
        if (dataBoundItem.ContractNum == null)
        {
          int num = (int) MessageBox.Show("Введите номер договора", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
        if (this.dgvContract.CurrentRow.Cells["DBeg"].Value != null || this.dgvContract.CurrentRow.Cells["DEnd"].Value != null)
        {
          try
          {
            dataBoundItem.DBeg = KvrplHelper.FirstDay(Convert.ToDateTime(this.dgvContract.CurrentRow.Cells["DBeg"].Value));
            dataBoundItem.DEnd = KvrplHelper.LastDay(Convert.ToDateTime(this.dgvContract.CurrentRow.Cells["DEnd"].Value));
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Проверьте правильность введенных дат", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
          }
          if (dataBoundItem.DBeg > dataBoundItem.DEnd)
          {
            int num = (int) MessageBox.Show("Дата начала больше даты окончания", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
          }
          if (this.dgvContract.CurrentRow.Cells["Org"].Value != null)
          {
            dataBoundItem.BaseOrg = this.session.Get<BaseOrg>((object) Convert.ToInt32(this.dgvContract.CurrentRow.Cells["Org"].Value));
            int contractId = dataBoundItem.ContractId;
            bool flag2 = (uint) dataBoundItem.ContractId <= 0U;
            if (flag2 && (dataBoundItem.DBeg <= this.lastDayMonthClosed || dataBoundItem.DEnd <= this.lastDayMonthClosed))
            {
              int num = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return true;
            }
            if (!flag2 && (this.oldContract.DBeg <= this.lastDayMonthClosed && this.oldContract.DEnd < this.lastDayMonthClosed || this.oldContract.DEnd < this.lastDayMonthClosed || this.oldContract.DBeg > this.lastDayMonthClosed && dataBoundItem.DBeg <= this.lastDayMonthClosed || this.oldContract.DBeg <= this.lastDayMonthClosed && (this.oldContract.DBeg != dataBoundItem.DBeg || this.oldContract.BaseOrg.BaseOrgId != dataBoundItem.BaseOrg.BaseOrgId)))
            {
              int num = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return true;
            }
            dataBoundItem.Manager = (BaseOrg) this.cmbManager.SelectedItem;
            dataBoundItem.UName = Options.Login;
            dataBoundItem.DEdit = DateTime.Now;
            using (ITransaction transaction = this.session.BeginTransaction())
            {
              try
              {
                if (flag2)
                {
                  try
                  {
                    IList<int> intList = this.session.CreateSQLQuery("select DBA.gen_id('mngContract',1)").List<int>();
                    dataBoundItem.ContractId = intList[0];
                    flag1 = false;
                  }
                  catch
                  {
                    int num = (int) MessageBox.Show("Не удалось сгенерировать новый код", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return true;
                  }
                  this.session.Save((object) dataBoundItem);
                }
                else if (!flag2 && this.oldContract.DEnd > dataBoundItem.DEnd && MessageBox.Show("Обновить все даты в договорах на лицевых, привязанных к данному?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                  this.session.CreateQuery(string.Format("update Bond set DEnd=:dend where Contract.ContractId=:cnt and DEnd>:dend")).SetDateTime("dend", dataBoundItem.DEnd).SetParameter<int>("cnt", dataBoundItem.ContractId).ExecuteUpdate();
                this.session.Update((object) dataBoundItem);
                this.session.Flush();
                transaction.Commit();
              }
              catch (Exception ex)
              {
                transaction.Rollback();
                int num = (int) MessageBox.Show("Невозможно сохранить изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                KvrplHelper.WriteLog(ex, (LsClient) null);
                return false;
              }
            }
            this.tsbAdd.Enabled = true;
            this.tsbApplay.Enabled = false;
            this.tsbCancel.Enabled = false;
            this.tsbDelete.Enabled = true;
          }
          else
          {
            int num = (int) MessageBox.Show("Организация не выбрана", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
          }
        }
        else
        {
          int num = (int) MessageBox.Show("Даты не введены", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
      }
      return true;
    }

    private void dgvContract_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.tsbApplay.Enabled = true;
      this.tsbCancel.Enabled = true;
      this.tsbDelete.Enabled = false;
      ((Contract) this.dgvContract.CurrentRow.DataBoundItem).IsEdit = true;
    }

    private void tsbCancel_Click(object sender, EventArgs e)
    {
      this.LoadContract();
      this.tsbAdd.Enabled = true;
      this.tsbApplay.Enabled = false;
      this.tsbCancel.Enabled = false;
      this.tsbDelete.Enabled = true;
    }

    private void tsbDelete_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Удалить договор?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      this.session = Domain.CurrentSession;
      Contract dataBoundItem = (Contract) this.dgvContract.CurrentRow.DataBoundItem;
      if (dataBoundItem.DBeg < this.monthClosed.PeriodName.Value.AddMonths(1))
      {
        int num1 = (int) MessageBox.Show("Запись принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
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
        this.LoadContract();
      }
    }

    private void FrmContract_Shown(object sender, EventArgs e)
    {
      this.LoadManager();
      try
      {
        this.monthClosed = this.session.CreateQuery(string.Format("select c.Period from CompanyPeriod c where c.Period.PeriodId=(select min(Period.PeriodId) from CompanyPeriod where Company.CompanyId in (select CompanyId from Company where Manager.BaseOrgId={0})) ", (object) ((BaseOrg) this.cmbManager.SelectedItem).BaseOrgId)).List<Period>()[0];
      }
      catch
      {
        this.monthClosed = this.session.CreateQuery(string.Format("select c.Period from CompanyPeriod c where c.Period.PeriodId=(select min(Period.PeriodId) from CompanyPeriod) ")).List<Period>()[0];
      }
      DateTime dateTime = this.monthClosed.PeriodName.Value;
      dateTime = dateTime.AddMonths(1);
      this.lastDayMonthClosed = dateTime.AddDays(-1.0);
      this.LoadOrg();
      this.LoadContract();
    }

    private void dgvContract_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgvContract.CurrentCell == null || this.dgvContract.CurrentCell.Value == null)
        return;
      Contract dataBoundItem = (Contract) this.dgvContract.CurrentRow.DataBoundItem;
      dataBoundItem.IsEdit = true;
      try
      {
        string name = this.dgvContract.Columns[e.ColumnIndex].Name;
        if (!(name == "DBeg"))
        {
          if (!(name == "DEnd"))
          {
            if (name == "Org")
            {
              try
              {
                dataBoundItem.BaseOrg = this.session.Get<BaseOrg>(this.dgvContract.CurrentRow.Cells["Org"].Value);
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
              dataBoundItem.DEnd = Convert.ToDateTime(this.dgvContract.CurrentRow.Cells["DEnd"].Value);
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
            dataBoundItem.DBeg = Convert.ToDateTime(this.dgvContract.CurrentRow.Cells["DBeg"].Value);
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

    private void dgvContract_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (((DataGridView) sender).DataSource == null)
        return;
      DataGridViewRow row = ((DataGridView) sender).Rows[e.RowIndex];
      DateTime dbeg = ((Contract) row.DataBoundItem).DBeg;
      DateTime? periodName = this.monthClosed.PeriodName;
      DateTime dateTime1 = periodName.Value;
      DateTime dateTime2 = dateTime1.AddMonths(2);
      int num;
      if (dbeg < dateTime2)
      {
        DateTime dend = ((Contract) row.DataBoundItem).DEnd;
        periodName = this.monthClosed.PeriodName;
        dateTime1 = periodName.Value;
        DateTime dateTime3 = dateTime1.AddMonths(1);
        num = dend >= dateTime3 ? 1 : 0;
      }
      else
        num = 0;
      if (num != 0)
        row.DefaultCellStyle.BackColor = Color.PapayaWhip;
      else
        row.DefaultCellStyle.ForeColor = Color.Gray;
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
      this.btnExit = new Button();
      this.pnUp = new Panel();
      this.cmbManager = new ComboBox();
      this.lblManager = new Label();
      this.cmbOrg = new ComboBox();
      this.lblOrg = new Label();
      this.dgvContract = new DataGridView();
      this.ts = new ToolStrip();
      this.tsbAdd = new ToolStripButton();
      this.tsbApplay = new ToolStripButton();
      this.tsbCancel = new ToolStripButton();
      this.tsbDelete = new ToolStripButton();
      this.pnBtn.SuspendLayout();
      this.pnUp.SuspendLayout();
      ((ISupportInitialize) this.dgvContract).BeginInit();
      this.ts.SuspendLayout();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 282);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(785, 40);
      this.pnBtn.TabIndex = 0;
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(692, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(81, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.pnUp.Controls.Add((Control) this.cmbManager);
      this.pnUp.Controls.Add((Control) this.lblManager);
      this.pnUp.Controls.Add((Control) this.cmbOrg);
      this.pnUp.Controls.Add((Control) this.lblOrg);
      this.pnUp.Dock = DockStyle.Top;
      this.pnUp.Location = new Point(0, 0);
      this.pnUp.Name = "pnUp";
      this.pnUp.Size = new Size(785, 72);
      this.pnUp.TabIndex = 1;
      this.cmbManager.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbManager.FormattingEnabled = true;
      this.cmbManager.Location = new Point(178, 6);
      this.cmbManager.Name = "cmbManager";
      this.cmbManager.Size = new Size(595, 24);
      this.cmbManager.TabIndex = 3;
      this.cmbManager.SelectionChangeCommitted += new EventHandler(this.cmbOrg_SelectionChangeCommitted);
      this.lblManager.AutoSize = true;
      this.lblManager.Location = new Point(12, 9);
      this.lblManager.Name = "lblManager";
      this.lblManager.Size = new Size(164, 16);
      this.lblManager.TabIndex = 2;
      this.lblManager.Text = "Управляющая компания";
      this.cmbOrg.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbOrg.FormattingEnabled = true;
      this.cmbOrg.Location = new Point(178, 38);
      this.cmbOrg.Name = "cmbOrg";
      this.cmbOrg.Size = new Size(595, 24);
      this.cmbOrg.TabIndex = 1;
      this.cmbOrg.SelectionChangeCommitted += new EventHandler(this.cmbOrg_SelectionChangeCommitted);
      this.lblOrg.AutoSize = true;
      this.lblOrg.Location = new Point(12, 41);
      this.lblOrg.Name = "lblOrg";
      this.lblOrg.Size = new Size(95, 16);
      this.lblOrg.TabIndex = 0;
      this.lblOrg.Text = "Организация";
      this.dgvContract.BackgroundColor = Color.AliceBlue;
      this.dgvContract.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvContract.Dock = DockStyle.Fill;
      this.dgvContract.Location = new Point(0, 96);
      this.dgvContract.Name = "dgvContract";
      this.dgvContract.Size = new Size(785, 186);
      this.dgvContract.TabIndex = 2;
      this.dgvContract.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvContract_CellBeginEdit);
      this.dgvContract.CellEndEdit += new DataGridViewCellEventHandler(this.dgvContract_CellEndEdit);
      this.dgvContract.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvContract_CellFormatting);
      this.ts.Font = new Font("Tahoma", 10f);
      this.ts.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.tsbAdd,
        (ToolStripItem) this.tsbApplay,
        (ToolStripItem) this.tsbCancel,
        (ToolStripItem) this.tsbDelete
      });
      this.ts.LayoutStyle = ToolStripLayoutStyle.Flow;
      this.ts.Location = new Point(0, 72);
      this.ts.Name = "ts";
      this.ts.Size = new Size(785, 24);
      this.ts.TabIndex = 3;
      this.ts.Text = "toolStrip2";
      this.tsbAdd.Image = (Image) Resources.add_var;
      this.tsbAdd.ImageTransparentColor = Color.Magenta;
      this.tsbAdd.Name = "tsbAdd";
      this.tsbAdd.Size = new Size(91, 21);
      this.tsbAdd.Text = "Добавить";
      this.tsbAdd.Click += new EventHandler(this.tsbAdd_Click);
      this.tsbApplay.Enabled = false;
      this.tsbApplay.Image = (Image) Resources.Applay_var;
      this.tsbApplay.ImageTransparentColor = Color.Magenta;
      this.tsbApplay.Name = "tsbApplay";
      this.tsbApplay.Size = new Size(99, 21);
      this.tsbApplay.Text = "Сохранить";
      this.tsbApplay.Click += new EventHandler(this.tsbApplay_Click);
      this.tsbCancel.Enabled = false;
      this.tsbCancel.Image = (Image) Resources.undo;
      this.tsbCancel.ImageTransparentColor = Color.Magenta;
      this.tsbCancel.Name = "tsbCancel";
      this.tsbCancel.Size = new Size(93, 21);
      this.tsbCancel.Text = "Отменить";
      this.tsbCancel.Click += new EventHandler(this.tsbCancel_Click);
      this.tsbDelete.Image = (Image) Resources.delete_var;
      this.tsbDelete.ImageTransparentColor = Color.Magenta;
      this.tsbDelete.Name = "tsbDelete";
      this.tsbDelete.Size = new Size(82, 21);
      this.tsbDelete.Text = "Удалить";
      this.tsbDelete.Click += new EventHandler(this.tsbDelete_Click);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(785, 322);
      this.Controls.Add((Control) this.dgvContract);
      this.Controls.Add((Control) this.ts);
      this.Controls.Add((Control) this.pnUp);
      this.Controls.Add((Control) this.pnBtn);
      this.Name = "FrmContract";
      this.Text = "Договоры организаций";
      this.Shown += new EventHandler(this.FrmContract_Shown);
      this.pnBtn.ResumeLayout(false);
      this.pnUp.ResumeLayout(false);
      this.pnUp.PerformLayout();
      ((ISupportInitialize) this.dgvContract).EndInit();
      this.ts.ResumeLayout(false);
      this.ts.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
