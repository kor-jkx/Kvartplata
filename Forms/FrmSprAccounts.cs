// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmSprAccounts
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
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmSprAccounts : FrmBaseForm
  {
    private IList<Complex> _complexList = (IList<Complex>) new List<Complex>();
    private int _complexId = -1;
    private IContainer components = (IContainer) null;
    private readonly bool _readOnly;
    private ISession session;
    private ohlAccounts newObject;
    private ohlAccounts curObject;
    private IList<ohlAccounts> ohlAccountsList;
    private Company _company;
    private int _city;
    private ToolStrip toolStrip1;
    private ToolStripButton tsbAdd;
    private ToolStripButton tsbApplay;
    private ToolStripButton tsbCancel;
    private ToolStripButton tsbDelete;
    private ToolStripButton tsbExit;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridView dgvBase;
    private Panel pnFilter;
    private Button btnFilter;
    private Label lblOwner;
    private Label lblBank;
    private Label lblAccount;
    private TextBox txbOwner;
    private TextBox txbBank;
    private TextBox txbAccount;
    private Button btnClearFilter;
    private ComboBox cmbTypeAccount;
    private Label label1;
    private DataGridViewTextBoxColumn Account;
    private DataGridViewComboBoxColumn Bank;
        private new DataGridViewComboBoxColumn Owner;
        private DataGridViewComboBoxColumn TypeAccount;
    private DataGridViewTextBoxColumn CodeSbrf;
    private DataGridViewComboBoxColumn ComplexId;
    private DataGridViewTextBoxColumn UName;
    private DataGridViewTextBoxColumn DEdit;

    public int AccountSelect { get; set; }

    public FrmSprAccounts()
    {
      this.InitializeComponent();
      this._readOnly = false;
      this.dgvBase.ReadOnly = false;
      this.dgvBase.Columns["Account"].ReadOnly = false;
      this.dgvBase.Columns["Bank"].ReadOnly = false;
      this.dgvBase.Columns["Owner"].ReadOnly = false;
      this.dgvBase.Columns["TypeAccount"].ReadOnly = false;
      this.dgvBase.Columns["Owner"].ReadOnly = false;
      this.dgvBase.Columns["CodeSbrf"].ReadOnly = false;
    }

    public FrmSprAccounts(bool readOnly, Company company, int complexId)
    {
      this._readOnly = readOnly;
      this._company = company;
      this._complexId = complexId;
      if (this._company != null)
      {
        this._city = Convert.ToInt32(KvrplHelper.BaseValue(1, Options.Company));
        if (this._city == 0)
          this._city = Options.City;
      }
      this.InitializeComponent();
      this.toolStrip1.Visible = !this._readOnly;
      this.dgvBase.ReadOnly = this._readOnly;
      this.dgvBase.Columns["Account"].ReadOnly = this._readOnly;
      this.dgvBase.Columns["Bank"].ReadOnly = this._readOnly;
      this.dgvBase.Columns["Owner"].ReadOnly = this._readOnly;
      this.dgvBase.Columns["TypeAccount"].ReadOnly = this._readOnly;
      this.dgvBase.Columns["Owner"].ReadOnly = this._readOnly;
      this.dgvBase.Columns["CodeSbrf"].ReadOnly = this._readOnly;
      this.dgvBase.Columns["ComplexId"].ReadOnly = this._readOnly;
    }

    private void FrmSprAccounts_Load(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      if (this._complexId == -1)
        this._complexList = this.session.CreateQuery("select c from Complex c where c.ComplexId=100 or c.ComplexId=113").List<Complex>();
      if (this._complexId == 113)
        this._complexList = this.session.CreateQuery("select c from Complex c where c.ComplexId=113").List<Complex>();
      this._complexList.Add(new Complex(0, "Не выбран"));
      this.GetData();
      IList<dcohlTypeAccount> dcohlTypeAccountList = this.session.CreateQuery("from dcohlTypeAccount").List<dcohlTypeAccount>();
      dcohlTypeAccountList.Insert(0, new dcohlTypeAccount()
      {
        TypeAccountId = 0,
        TypeAccountName = ""
      });
      this.cmbTypeAccount.DataSource = (object) dcohlTypeAccountList;
      this.cmbTypeAccount.DisplayMember = "TypeAccountName";
      this.cmbTypeAccount.ValueMember = "TypeAccountId";
    }

    private void GetData()
    {
      if (this._complexId == -1)
        this.ohlAccountsList = this.session.CreateQuery("from ohlAccounts").List<ohlAccounts>();
      if (this._complexId == 113)
        this.ohlAccountsList = this.session.CreateQuery("from ohlAccounts where ComplexId=113").List<ohlAccounts>();
      this.dgvBase.AutoGenerateColumns = false;
      this.dgvBase.DataSource = (object) null;
      if (this.ohlAccountsList.Count<ohlAccounts>() > 0)
      {
        this.dgvBase.DataSource = (object) this.ohlAccountsList;
        this.dgvBase.Refresh();
      }
      if (!this._readOnly)
        this.dgvBase.ReadOnly = false;
      this.SelectRow();
      this.SetViewDgvBase();
    }

    private void SetViewDgvBase()
    {
      IList<Kvartplata.Classes.Bank> bankList = this.session.CreateQuery("from Bank").List<Kvartplata.Classes.Bank>();
      IList<BaseOrg> list = (IList<BaseOrg>) this.session.CreateQuery("from BaseOrg").List<BaseOrg>().OrderBy<BaseOrg, string>((Func<BaseOrg, string>) (x => x.NameOrgMin)).ToList<BaseOrg>();
      IList<dcohlTypeAccount> dcohlTypeAccountList = this.session.CreateQuery("from dcohlTypeAccount").List<dcohlTypeAccount>();
      for (int index = 0; index < this.dgvBase.Rows.Count; ++index)
      {
        DataGridViewRow row = this.dgvBase.Rows[index];
        ((DataGridViewComboBoxCell) this.dgvBase["Bank", row.Index]).DataSource = (object) bankList;
        ((DataGridViewComboBoxCell) this.dgvBase["Bank", row.Index]).DisplayMember = "BankName";
        ((DataGridViewComboBoxCell) this.dgvBase["Bank", row.Index]).ValueMember = "BankId";
        if (row.DataBoundItem != null)
        {
          Kvartplata.Classes.Bank bank = ((ohlAccounts) row.DataBoundItem).Bank;
          if (bank != null)
            row.Cells["Bank"].Value = (object) bank.BankId;
          dcohlTypeAccount typeAccount = ((ohlAccounts) row.DataBoundItem).TypeAccount;
          if (typeAccount != null)
            row.Cells["TypeAccount"].Value = (object) typeAccount.TypeAccountId;
          BaseOrg ownerAccount = ((ohlAccounts) row.DataBoundItem).OwnerAccount;
          if (ownerAccount != null)
            row.Cells["Owner"].Value = (object) ownerAccount.BaseOrgId;
          int complexId = ((ohlAccounts) row.DataBoundItem).ComplexId;
          row.Cells["ComplexId"].Value = (object) complexId;
        }
        ((DataGridViewComboBoxCell) this.dgvBase["TypeAccount", row.Index]).DataSource = (object) dcohlTypeAccountList;
        ((DataGridViewComboBoxCell) this.dgvBase["TypeAccount", row.Index]).DisplayMember = "TypeAccountName";
        ((DataGridViewComboBoxCell) this.dgvBase["TypeAccount", row.Index]).ValueMember = "TypeAccountId";
        ((DataGridViewComboBoxCell) this.dgvBase["Owner", row.Index]).DataSource = (object) list;
        ((DataGridViewComboBoxCell) this.dgvBase["Owner", row.Index]).DisplayMember = "NameOrgMin";
        ((DataGridViewComboBoxCell) this.dgvBase["Owner", row.Index]).ValueMember = "BaseOrgId";
        ((DataGridViewComboBoxCell) this.dgvBase["ComplexId", row.Index]).DataSource = (object) this._complexList;
        ((DataGridViewComboBoxCell) this.dgvBase["ComplexId", row.Index]).DisplayMember = "ComplexName";
        ((DataGridViewComboBoxCell) this.dgvBase["ComplexId", row.Index]).ValueMember = "ComplexId";
      }
    }

    private void tsbAdd_Click(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      this.newObject = new ohlAccounts();
      this.newObject.ohlAccountsId = (int) this.session.CreateSQLQuery("select dba.gen_id('OHLACCOUNTS',1)").UniqueResult();
      this.newObject.ComplexId = 113;
      if (Options.Kvartplata && !Options.Overhaul)
        this.newObject.ComplexId = 100;
      if (Options.Overhaul && Options.Kvartplata)
        this.newObject.ComplexId = this._city != 35 ? 0 : 113;
      if ((uint) this.dgvBase.Rows.Count > 0U)
        this.ohlAccountsList = (IList<ohlAccounts>) (this.dgvBase.DataSource as List<ohlAccounts>);
      if (this.ohlAccountsList != null)
      {
        this.ohlAccountsList.Add(this.newObject);
        this.dgvBase.DataSource = (object) null;
        if (this.ohlAccountsList.Count<ohlAccounts>() > 0)
        {
          this.dgvBase.DataSource = (object) this.ohlAccountsList;
          this.dgvBase.Refresh();
        }
      }
      this.dgvBase.ReadOnly = false;
      this.SetViewDgvBase();
      if (this.dgvBase.Rows.Count > 1)
      {
        this.dgvBase.CurrentCell = this.dgvBase.Rows[this.dgvBase.Rows.Count - 1].Cells[1];
        this.dgvBase.Rows[this.dgvBase.Rows.Count - 1].Selected = true;
      }
      this.tsbAdd.Enabled = false;
      this.tsbApplay.Enabled = true;
      this.tsbCancel.Enabled = true;
      this.tsbDelete.Enabled = false;
    }

    private void tsbApplay_Click(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      this.dgvBase.EndEdit();
      string input = "";
      if (this.dgvBase.CurrentRow.Cells["Account"].Value != null)
        input = this.dgvBase.CurrentRow.Cells["Account"].Value.ToString();
      if (!Regex.IsMatch(input, "^[0-9]{20,20}$"))
      {
        int num1 = (int) MessageBox.Show("Счет должен состоять только из цифр, и иметь 20 символов.", "Внимание!", MessageBoxButtons.OK);
      }
      else
      {
        if (this.newObject == null)
        {
          ohlAccounts dataBoundItem = (ohlAccounts) this.dgvBase.Rows[this.dgvBase.CurrentRow.Index].DataBoundItem;
          dataBoundItem.Account = input;
          dataBoundItem.Bank = this.session.Get<Kvartplata.Classes.Bank>((object) (int) this.dgvBase.CurrentRow.Cells["Bank"].Value);
          dataBoundItem.TypeAccount = this.session.Get<dcohlTypeAccount>((object) (int) this.dgvBase.CurrentRow.Cells["TypeAccount"].Value);
          dataBoundItem.OwnerAccount = this.session.Get<BaseOrg>((object) (int) this.dgvBase.CurrentRow.Cells["Owner"].Value);
          dataBoundItem.DEdit = DateTime.Now.Date;
          dataBoundItem.UName = Options.Login;
          dataBoundItem.CodeSbrf = Convert.ToInt32(this.dgvBase.CurrentRow.Cells["CodeSbrf"].Value);
          dataBoundItem.ComplexId = Convert.ToInt32(this.dgvBase.CurrentRow.Cells["ComplexId"].Value);
          if (Options.Kvartplata && !Options.Overhaul && dataBoundItem.ComplexId != 100)
          {
            int num2 = (int) MessageBox.Show("Выбирете комплекс \"Новая квартплата\"", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          if (Options.Overhaul && Options.Kvartplata)
          {
            if (this._city == 35)
            {
              if (dataBoundItem.ComplexId != 113)
              {
                int num2 = (int) MessageBox.Show("Выбирете комплекс \"Капитальный ремонт\"", "Внимание!", MessageBoxButtons.OK);
                return;
              }
            }
            else if (dataBoundItem.ComplexId == 0)
            {
              int num2 = (int) MessageBox.Show("Выбирете комплекс", "Внимание!", MessageBoxButtons.OK);
              return;
            }
          }
          this.session.Update((object) dataBoundItem);
          this.session.Flush();
          this.session.Refresh((object) dataBoundItem);
          this.curObject = dataBoundItem;
          this.GetData();
        }
        else
        {
          this.newObject.Bank = new Kvartplata.Classes.Bank();
          this.newObject.TypeAccount = new dcohlTypeAccount();
          this.newObject.OwnerAccount = new BaseOrg();
          if (this.dgvBase.CurrentRow.Cells["Bank"].Value == null)
          {
            int num2 = (int) MessageBox.Show("Выберите банк.", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          if (this.dgvBase.CurrentRow.Cells["Owner"].Value == null)
          {
            int num2 = (int) MessageBox.Show("Выберите собственника счета.", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          if (this.dgvBase.CurrentRow.Cells["TypeAccount"].Value == null)
          {
            int num2 = (int) MessageBox.Show("Выберите тип счета.", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          this.newObject.CodeSbrf = this.dgvBase.CurrentRow.Cells["CodeSbrf"].Value != null ? Convert.ToInt32(this.dgvBase.CurrentRow.Cells["CodeSbrf"].Value) : 0;
          this.newObject.Account = input;
          this.newObject.Bank = this.session.Get<Kvartplata.Classes.Bank>((object) (int) this.dgvBase.CurrentRow.Cells["Bank"].Value);
          this.newObject.TypeAccount = this.session.Get<dcohlTypeAccount>((object) (int) this.dgvBase.CurrentRow.Cells["TypeAccount"].Value);
          this.newObject.OwnerAccount = this.session.Get<BaseOrg>((object) (int) this.dgvBase.CurrentRow.Cells["Owner"].Value);
          this.newObject.ComplexId = Convert.ToInt32(this.dgvBase.CurrentRow.Cells["ComplexId"].Value);
          if (Options.Kvartplata && !Options.Overhaul && this.newObject.ComplexId != 100)
          {
            int num2 = (int) MessageBox.Show("Выбирете комплекс \"Новая квартплата\"", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          if (Options.Overhaul && Options.Kvartplata)
          {
            if (this._city == 35)
            {
              if (this.newObject.ComplexId != 113)
              {
                int num2 = (int) MessageBox.Show("Выбирете комплекс \"Капитальный ремонт\"", "Внимание!", MessageBoxButtons.OK);
                return;
              }
            }
            else if (this.newObject.ComplexId == 0)
            {
              int num2 = (int) MessageBox.Show("Выбирете комплекс", "Внимание!", MessageBoxButtons.OK);
              return;
            }
          }
          if (this.newObject.Account == null)
          {
            int num2 = (int) MessageBox.Show("Введите счет.", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          this.newObject.UName = Options.Login;
          this.newObject.DEdit = DateTime.Now.Date;
          this.session.Save((object) this.newObject);
          this.session.Flush();
          this.session.Refresh((object) this.newObject);
          this.curObject = this.newObject;
          this.GetData();
          this.newObject = (ohlAccounts) null;
        }
        this.tsbAdd.Enabled = true;
        this.tsbApplay.Enabled = false;
        this.tsbCancel.Enabled = false;
        this.tsbDelete.Enabled = true;
      }
    }

    private void tsbCancel_Click(object sender, EventArgs e)
    {
      this.dgvBase.EndEdit();
      if (this.newObject != null)
      {
        this.ohlAccountsList.Remove(this.newObject);
        this.newObject = (ohlAccounts) null;
      }
      foreach (object ohlAccounts in (IEnumerable<ohlAccounts>) this.ohlAccountsList)
        this.session.Refresh(ohlAccounts);
      this.tsbAdd.Enabled = true;
      this.tsbApplay.Enabled = false;
      this.tsbCancel.Enabled = false;
      this.tsbDelete.Enabled = true;
      this.GetData();
    }

    protected void SelectRow()
    {
      if (this.curObject != null)
      {
        int curObject = KvrplHelper.FindCurObject((IList) this.ohlAccountsList.ToList<ohlAccounts>(), (object) this.curObject);
        if (curObject < 0)
          return;
        this.dgvBase.CurrentCell = this.dgvBase.Rows[curObject].Cells[1];
        this.dgvBase.Rows[curObject].Selected = true;
      }
      else if (this.dgvBase.CurrentRow != null && this.dgvBase.CurrentRow.Index < this.ohlAccountsList.Count)
        this.curObject = (ohlAccounts) this.dgvBase.Rows[this.dgvBase.CurrentRow.Index].DataBoundItem;
    }

    private void dgvBase_SelectionChanged(object sender, EventArgs e)
    {
      if (this.dgvBase.CurrentRow == null || this.dgvBase.CurrentRow.Index >= this.ohlAccountsList.Count)
        return;
      this.curObject = (ohlAccounts) this.dgvBase.Rows[this.dgvBase.CurrentRow.Index].DataBoundItem;
      this.AccountSelect = this.curObject.ohlAccountsId;
    }

    private void dgvBase_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.tsbAdd.Enabled = false;
      this.tsbApplay.Enabled = true;
      this.tsbCancel.Enabled = true;
      this.tsbDelete.Enabled = false;
    }

    private void tsbDelete_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      if (this.curObject != null)
      {
        try
        {
          this.session.Delete((object) this.curObject);
          this.session.Flush();
          this.curObject = (ohlAccounts) null;
          this.GetData();
        }
        catch
        {
          int num = (int) MessageBox.Show("Удаление невозможно! Существуют данные, ссылающиеся на эту запись.", "Внимание!", MessageBoxButtons.OK);
          this.session.Clear();
          this.session = Domain.CurrentSession;
          this.curObject = (ohlAccounts) null;
          this.GetData();
        }
      }
    }

    private void btnFilter_Click(object sender, EventArgs e)
    {
      int typeacc = (int) this.cmbTypeAccount.SelectedValue;
      IList<ohlAccounts> list = (IList<ohlAccounts>) this.ohlAccountsList.Where<ohlAccounts>((Func<ohlAccounts, bool>) (ohlAccountse =>
      {
        if (!ohlAccountse.Account.Contains(this.txbAccount.Text) || !ohlAccountse.Bank.BankName.ToLower().Contains(this.txbBank.Text.ToLower()) || !ohlAccountse.OwnerAccount.BaseOrgName.ToLower().Contains(this.txbOwner.Text.ToLower()))
          return false;
        if (typeacc != 0)
          return ohlAccountse.TypeAccount.TypeAccountId.Equals(typeacc);
        return true;
      })).ToList<ohlAccounts>();
      this.dgvBase.AutoGenerateColumns = false;
      this.dgvBase.DataSource = (object) null;
      this.dgvBase.DataSource = (object) list;
      this.dgvBase.ReadOnly = false;
      this.SetViewDgvBase();
    }

    private void btnClearFilter_Click(object sender, EventArgs e)
    {
      this.txbAccount.Text = "";
      this.txbBank.Text = "";
      this.txbOwner.Text = "";
      this.dgvBase.AutoGenerateColumns = false;
      this.dgvBase.DataSource = (object) null;
      this.dgvBase.DataSource = (object) this.ohlAccountsList;
      this.dgvBase.ReadOnly = false;
      this.SelectRow();
      this.SetViewDgvBase();
    }

    private void dgvBase_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      if (!this._readOnly)
        return;
      this.Close();
    }

    private void txbAccount_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (this.txbAccount.Text.Count<char>() > 20)
        e.Handled = true;
      if ((int) e.KeyChar >= 48 && (int) e.KeyChar <= 58 || (int) e.KeyChar == 8)
        return;
      e.Handled = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmSprAccounts));
      this.toolStrip1 = new ToolStrip();
      this.tsbAdd = new ToolStripButton();
      this.tsbApplay = new ToolStripButton();
      this.tsbCancel = new ToolStripButton();
      this.tsbDelete = new ToolStripButton();
      this.tsbExit = new ToolStripButton();
      this.dgvBase = new DataGridView();
      this.pnFilter = new Panel();
      this.cmbTypeAccount = new ComboBox();
      this.label1 = new Label();
      this.btnClearFilter = new Button();
      this.btnFilter = new Button();
      this.lblOwner = new Label();
      this.lblBank = new Label();
      this.lblAccount = new Label();
      this.txbOwner = new TextBox();
      this.txbBank = new TextBox();
      this.txbAccount = new TextBox();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
      this.Account = new DataGridViewTextBoxColumn();
      this.Bank = new DataGridViewComboBoxColumn();
      this.Owner = new DataGridViewComboBoxColumn();
      this.TypeAccount = new DataGridViewComboBoxColumn();
      this.CodeSbrf = new DataGridViewTextBoxColumn();
      this.ComplexId = new DataGridViewComboBoxColumn();
      this.UName = new DataGridViewTextBoxColumn();
      this.DEdit = new DataGridViewTextBoxColumn();
      this.toolStrip1.SuspendLayout();
      ((ISupportInitialize) this.dgvBase).BeginInit();
      this.pnFilter.SuspendLayout();
      this.SuspendLayout();
      this.toolStrip1.Font = new Font("Tahoma", 10f);
      this.toolStrip1.Items.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.tsbAdd,
        (ToolStripItem) this.tsbApplay,
        (ToolStripItem) this.tsbCancel,
        (ToolStripItem) this.tsbDelete,
        (ToolStripItem) this.tsbExit
      });
      this.toolStrip1.LayoutStyle = ToolStripLayoutStyle.Flow;
      this.toolStrip1.Location = new Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new Size(1536, 24);
      this.toolStrip1.TabIndex = 1;
      this.toolStrip1.Text = "toolStrip1";
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
      this.tsbCancel.Size = new Size(77, 21);
      this.tsbCancel.Text = "Отмена";
      this.tsbCancel.Click += new EventHandler(this.tsbCancel_Click);
      this.tsbDelete.Image = (Image) Resources.delete_var;
      this.tsbDelete.ImageTransparentColor = Color.Magenta;
      this.tsbDelete.Name = "tsbDelete";
      this.tsbDelete.Size = new Size(82, 21);
      this.tsbDelete.Text = "Удалить";
      this.tsbDelete.Click += new EventHandler(this.tsbDelete_Click);
      this.tsbExit.Image = (Image) Resources.Exit;
      this.tsbExit.ImageTransparentColor = Color.Magenta;
      this.tsbExit.Name = "tsbExit";
      this.tsbExit.Size = new Size(70, 21);
      this.tsbExit.Text = "Выход";
      this.tsbExit.Visible = false;
      this.dgvBase.AllowUserToAddRows = false;
      this.dgvBase.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      this.dgvBase.BackgroundColor = Color.AliceBlue;
      this.dgvBase.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvBase.Columns.AddRange((DataGridViewColumn) this.Account, (DataGridViewColumn) this.Bank, (DataGridViewColumn) this.Owner, (DataGridViewColumn) this.TypeAccount, (DataGridViewColumn) this.CodeSbrf, (DataGridViewColumn) this.ComplexId, (DataGridViewColumn) this.UName, (DataGridViewColumn) this.DEdit);
      this.dgvBase.Dock = DockStyle.Fill;
      this.dgvBase.Location = new Point(0, 69);
      this.dgvBase.Name = "dgvBase";
      this.dgvBase.Size = new Size(1536, 549);
      this.dgvBase.TabIndex = 7;
      this.dgvBase.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvBase_CellBeginEdit);
      this.dgvBase.CellDoubleClick += new DataGridViewCellEventHandler(this.dgvBase_CellDoubleClick);
      this.dgvBase.SelectionChanged += new EventHandler(this.dgvBase_SelectionChanged);
      this.pnFilter.Controls.Add((Control) this.cmbTypeAccount);
      this.pnFilter.Controls.Add((Control) this.label1);
      this.pnFilter.Controls.Add((Control) this.btnClearFilter);
      this.pnFilter.Controls.Add((Control) this.btnFilter);
      this.pnFilter.Controls.Add((Control) this.lblOwner);
      this.pnFilter.Controls.Add((Control) this.lblBank);
      this.pnFilter.Controls.Add((Control) this.lblAccount);
      this.pnFilter.Controls.Add((Control) this.txbOwner);
      this.pnFilter.Controls.Add((Control) this.txbBank);
      this.pnFilter.Controls.Add((Control) this.txbAccount);
      this.pnFilter.Dock = DockStyle.Top;
      this.pnFilter.Location = new Point(0, 24);
      this.pnFilter.Name = "pnFilter";
      this.pnFilter.Size = new Size(1536, 45);
      this.pnFilter.TabIndex = 8;
      this.cmbTypeAccount.Location = new Point(1075, 8);
      this.cmbTypeAccount.Name = "cmbTypeAccount";
      this.cmbTypeAccount.Size = new Size(171, 24);
      this.cmbTypeAccount.TabIndex = 8;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(995, 11);
      this.label1.Name = "label1";
      this.label1.Size = new Size(74, 16);
      this.label1.TabIndex = 8;
      this.label1.Text = "Тип счета";
      this.btnClearFilter.Location = new Point(1393, 4);
      this.btnClearFilter.Name = "btnClearFilter";
      this.btnClearFilter.Size = new Size(131, 31);
      this.btnClearFilter.TabIndex = 7;
      this.btnClearFilter.Text = "Сбросить фильтр";
      this.btnClearFilter.UseVisualStyleBackColor = true;
      this.btnClearFilter.Click += new EventHandler(this.btnClearFilter_Click);
      this.btnFilter.Location = new Point(1256, 4);
      this.btnFilter.Name = "btnFilter";
      this.btnFilter.Size = new Size(131, 31);
      this.btnFilter.TabIndex = 6;
      this.btnFilter.Text = "Отфильтровать";
      this.btnFilter.UseVisualStyleBackColor = true;
      this.btnFilter.Click += new EventHandler(this.btnFilter_Click);
      this.lblOwner.AutoSize = true;
      this.lblOwner.Location = new Point(599, 11);
      this.lblOwner.Name = "lblOwner";
      this.lblOwner.Size = new Size(135, 16);
      this.lblOwner.TabIndex = 5;
      this.lblOwner.Text = "Собственник счета";
      this.lblBank.AutoSize = true;
      this.lblBank.Location = new Point(298, 11);
      this.lblBank.Name = "lblBank";
      this.lblBank.Size = new Size(40, 16);
      this.lblBank.TabIndex = 4;
      this.lblBank.Text = "Банк";
      this.lblAccount.AutoSize = true;
      this.lblAccount.Location = new Point(3, 11);
      this.lblAccount.Name = "lblAccount";
      this.lblAccount.Size = new Size(40, 16);
      this.lblAccount.TabIndex = 3;
      this.lblAccount.Text = "Счет";
      this.txbOwner.Location = new Point(740, 8);
      this.txbOwner.Name = "txbOwner";
      this.txbOwner.Size = new Size(249, 22);
      this.txbOwner.TabIndex = 2;
      this.txbBank.Location = new Point(344, 8);
      this.txbBank.Name = "txbBank";
      this.txbBank.Size = new Size(249, 22);
      this.txbBank.TabIndex = 1;
      this.txbAccount.Location = new Point(49, 8);
      this.txbAccount.Name = "txbAccount";
      this.txbAccount.Size = new Size(243, 22);
      this.txbAccount.TabIndex = 0;
      this.txbAccount.KeyPress += new KeyPressEventHandler(this.txbAccount_KeyPress);
      this.dataGridViewTextBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn1.DataPropertyName = "Account";
      this.dataGridViewTextBoxColumn1.HeaderText = "Счет";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn2.DataPropertyName = "UName";
      this.dataGridViewTextBoxColumn2.HeaderText = "Пользователь";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn2.ReadOnly = true;
      this.dataGridViewTextBoxColumn2.Width = 110;
      this.dataGridViewTextBoxColumn3.DataPropertyName = "DEdit";
      this.dataGridViewTextBoxColumn3.HeaderText = "Дата редактирования";
      this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
      this.dataGridViewTextBoxColumn3.ReadOnly = true;
      this.dataGridViewTextBoxColumn3.Width = 120;
      this.Account.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.Account.DataPropertyName = "Account";
      this.Account.HeaderText = "Счет";
      this.Account.Name = "Account";
      this.Bank.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.Bank.HeaderText = "Банк";
      this.Bank.Name = "Bank";
      this.Bank.Resizable = DataGridViewTriState.True;
      this.Owner.HeaderText = "Собственник счета";
      this.Owner.Name = "Owner";
      this.TypeAccount.HeaderText = "Тип счета";
      this.TypeAccount.Name = "TypeAccount";
      this.CodeSbrf.DataPropertyName = "CodeSbrf";
      this.CodeSbrf.HeaderText = "Код для выгрузки в СберРФ";
      this.CodeSbrf.Name = "CodeSbrf";
      this.ComplexId.HeaderText = "Комплекс";
      this.ComplexId.Name = "ComplexId";
      this.UName.DataPropertyName = "UName";
      this.UName.HeaderText = "Пользователь";
      this.UName.Name = "UName";
      this.UName.ReadOnly = true;
      this.DEdit.DataPropertyName = "DEdit";
      this.DEdit.HeaderText = "Дата";
      this.DEdit.Name = "DEdit";
      this.DEdit.ReadOnly = true;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1536, 618);
      this.Controls.Add((Control) this.dgvBase);
      this.Controls.Add((Control) this.pnFilter);
      this.Controls.Add((Control) this.toolStrip1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(5);
      this.Name = "FrmSprAccounts";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Справочник счетов";
      this.Load += new EventHandler(this.FrmSprAccounts_Load);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      ((ISupportInitialize) this.dgvBase).EndInit();
      this.pnFilter.ResumeLayout(false);
      this.pnFilter.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
