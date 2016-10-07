// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmBank
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
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmBank : FrmBaseForm1
  {
    private FormStateSaver fss = new FormStateSaver(FrmBank.container);
    protected GridSettings MySettingsBank = new GridSettings();
    private bool _readOnly = false;
    private IContainer components = (IContainer) null;
    private static IContainer container;
    private ISession session;
    private bool insertRecord;
    private Reg region;
    private IList<Reg> city;
    private Company company;
    private Panel pnUp;
    private TextBox txtSearch;
    private Label lblSearch;
    private ToolStrip ts;
    private DataGridView dgvBank;
    protected ToolStripButton tsbAdd;
    public ToolStripButton tsbApplay;
    public ToolStripButton tsbCancel;
    protected ToolStripButton tsbDelete;
    public HelpProvider hp;

    public FrmBank()
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
      this.MySettingsBank.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
    }

    public FrmBank(Company company)
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
      this.company = company;
      this.CheckAccess();
      this.MySettingsBank.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
    }

    private void CheckAccess()
    {
      this._readOnly = (Options.City == 35 || Options.City == 36) && KvrplHelper.CheckProxy(75, 2, this.company, false) || KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(32, this.company, false)) && KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(75, this.company, false));
      this.ts.Visible = this._readOnly;
      this.dgvBank.ReadOnly = !this._readOnly;
    }

    private void FrmBank_Load(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      this.session = Domain.CurrentSession;
      this.city = (IList<Reg>) new List<Reg>();
      this.city = this.session.CreateCriteria(typeof (Reg)).AddOrder(Order.Asc("RegionName")).List<Reg>();
      this.session.Clear();
      this.MySettingsBank.GridName = "Bank";
      this.LoadBank();
      this.Cursor = Cursors.Default;
    }

    private void LoadBank()
    {
      this.session = Domain.CurrentSession;
      IList<Bank> bankList1 = (IList<Bank>) new List<Bank>();
      IList<Bank> bankList2 = this.session.CreateCriteria(typeof (Bank)).Add((ICriterion) Restrictions.Not((ICriterion) Restrictions.Eq("BankId", (object) 0))).AddOrder(Order.Asc("BankName")).List<Bank>();
      this.dgvBank.DataSource = (object) null;
      this.dgvBank.Columns.Clear();
      this.dgvBank.DataSource = (object) bankList2;
      this.session.Clear();
      this.SetViewBank();
    }

    private void SetViewBank()
    {
      this.dgvBank.Columns["BankName"].HeaderText = "Наименование банка";
      this.dgvBank.Columns["BIK"].HeaderText = "БИК";
      this.dgvBank.Columns["INN"].HeaderText = "ИНН";
      this.dgvBank.Columns["NameMin"].HeaderText = "Краткое наименование";
      this.dgvBank.Columns["KorSch"].HeaderText = "Кор. счет";
      this.dgvBank.Columns["BankId"].Visible = false;
      KvrplHelper.AddComboBoxColumn(this.dgvBank, 6, (IList) this.city, "RegionId", "RegionName", "Населенный пункт", "City", 10, 150);
      this.dgvBank.Columns["City"].ReadOnly = true;
      foreach (DataGridViewRow row in (IEnumerable) this.dgvBank.Rows)
      {
        if (((Bank) row.DataBoundItem).Reg != null)
          row.Cells["City"].Value = (object) ((Bank) row.DataBoundItem).Reg.RegionId;
      }
      this.LoadSettingsBank();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, this.company, true))
        return;
      this.insertRecord = true;
      Bank bank = new Bank();
      this.session = Domain.CurrentSession;
      IList<int> intList = this.session.CreateSQLQuery("select DBA.gen_id('di_bank',1)").List<int>();
      bank.BankId = intList[0];
      IList<Bank> bankList = (IList<Bank>) new List<Bank>();
      if ((uint) this.dgvBank.Rows.Count > 0U)
        bankList = this.dgvBank.DataSource as IList<Bank>;
      bankList.Add(bank);
      this.dgvBank.DataSource = (object) null;
      this.dgvBank.Columns.Clear();
      this.dgvBank.DataSource = (object) bankList;
      this.session.Clear();
      this.SetViewBank();
      this.dgvBank.CurrentCell = this.dgvBank.Rows[this.dgvBank.Rows.Count - 1].Cells[1];
    }

    private void dgvBank_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, this.dgvBank.Name, e);
    }

    private void dgvBank_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.RowIndex == -1 || e.ColumnIndex == -1 || e.ColumnIndex != 6)
        return;
      FrmRegion frmRegion = new FrmRegion();
      int num = (int) frmRegion.ShowDialog();
      this.region = frmRegion.ReturnRegion();
      frmRegion.Dispose();
      if (this.region != null)
        this.dgvBank.CurrentRow.Cells["City"].Value = (object) this.region.RegionId;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, this.company, true))
        return;
      this.tsbApplay.Enabled = false;
      this.tsbCancel.Enabled = false;
      this.tsbAdd.Enabled = true;
      this.tsbDelete.Enabled = true;
      this.session.Clear();
      this.dgvBank.CommitEdit(DataGridViewDataErrorContexts.Commit);
      if (this.dgvBank.Rows.Count <= 0 || this.dgvBank.CurrentRow == null)
        return;
      Bank dataBoundItem = (Bank) this.dgvBank.CurrentRow.DataBoundItem;
      if (this.dgvBank.CurrentRow.Cells["BankName"].Value != null)
      {
        dataBoundItem.BankName = this.dgvBank.CurrentRow.Cells["BankName"].Value.ToString();
        dataBoundItem.NameMin = this.dgvBank.CurrentRow.Cells["NameMin"].Value == null ? "" : this.dgvBank.CurrentRow.Cells["NameMin"].Value.ToString();
        dataBoundItem.BIK = this.dgvBank.CurrentRow.Cells["BIK"].Value == null ? "" : this.dgvBank.CurrentRow.Cells["BIK"].Value.ToString();
        dataBoundItem.INN = this.dgvBank.CurrentRow.Cells["INN"].Value == null ? "" : this.dgvBank.CurrentRow.Cells["INN"].Value.ToString();
        dataBoundItem.KorSch = this.dgvBank.CurrentRow.Cells["KorSch"].Value == null ? "" : this.dgvBank.CurrentRow.Cells["KorSch"].Value.ToString();
        this.session = Domain.CurrentSession;
        if (this.dgvBank.CurrentRow.Cells["City"].Value != null)
        {
          Reg reg = this.session.Get<Reg>(this.dgvBank.CurrentRow.Cells["City"].Value);
          dataBoundItem.Reg = reg;
        }
        else
          dataBoundItem.Reg = (Reg) null;
        try
        {
          if (this.insertRecord)
          {
            this.insertRecord = false;
            this.session.Save((object) dataBoundItem);
          }
          else
            this.session.Update((object) dataBoundItem);
          this.session.Flush();
        }
        catch
        {
          int num = (int) MessageBox.Show("Не удалось сохранить изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        this.session.Clear();
        this.LoadBank();
      }
      else
      {
        int num1 = (int) MessageBox.Show("Введите название банка", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, this.company, true) || (this.dgvBank.Rows.Count <= 0 || this.dgvBank.CurrentRow == null || MessageBox.Show("Вы уверены, что хотите удалить запись", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK))
        return;
      this.session = Domain.CurrentSession;
      Bank dataBoundItem = (Bank) this.dgvBank.CurrentRow.DataBoundItem;
      if (this.session.CreateQuery(string.Format("from CmpReceipt where Bank.BankId={0}", (object) dataBoundItem.BankId)).List().Count > 0)
      {
        int num1 = (int) MessageBox.Show("Удаление невозможно. Существуют привязанные записи", "Внимание");
      }
      else
      {
        try
        {
          this.session.Delete((object) dataBoundItem);
          this.session.Flush();
        }
        catch
        {
          int num2 = (int) MessageBox.Show("Не удалось удалить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        this.session.Clear();
        this.LoadBank();
      }
    }

    private void dgvBank_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.tsbAdd.Enabled = false;
      this.tsbApplay.Enabled = true;
      this.tsbDelete.Enabled = false;
      this.tsbCancel.Enabled = true;
    }

    private void txtSearch_KeyUp(object sender, KeyEventArgs e)
    {
      this.session = Domain.CurrentSession;
      IList<Bank> bankList1 = (IList<Bank>) new List<Bank>();
      IList<Bank> bankList2 = this.session.CreateCriteria(typeof (Bank)).Add((ICriterion) Restrictions.Not((ICriterion) Restrictions.Eq("BankId", (object) 0))).Add((ICriterion) Restrictions.Like("BankName", (object) ("%" + this.txtSearch.Text + "%"))).AddOrder(Order.Asc("BankName")).List<Bank>();
      this.dgvBank.DataSource = (object) null;
      this.dgvBank.Columns.Clear();
      this.dgvBank.DataSource = (object) bankList2;
      this.session.Clear();
      this.SetViewBank();
    }

    private void dgvBank_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsBank.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsBank.Columns[this.MySettingsBank.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsBank.Save();
    }

    private void LoadSettingsBank()
    {
      this.MySettingsBank.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvBank.Columns)
        this.MySettingsBank.GetMySettings(column);
    }

    private void tsbCancel_Click(object sender, EventArgs e)
    {
      this.tsbAdd.Enabled = true;
      this.tsbApplay.Enabled = false;
      this.tsbDelete.Enabled = true;
      this.tsbCancel.Enabled = false;
      this.LoadBank();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnUp = new Panel();
      this.txtSearch = new TextBox();
      this.lblSearch = new Label();
      this.ts = new ToolStrip();
      this.tsbAdd = new ToolStripButton();
      this.tsbApplay = new ToolStripButton();
      this.tsbCancel = new ToolStripButton();
      this.tsbDelete = new ToolStripButton();
      this.dgvBank = new DataGridView();
      this.hp = new HelpProvider();
      this.pnUp.SuspendLayout();
      this.ts.SuspendLayout();
      ((ISupportInitialize) this.dgvBank).BeginInit();
      this.SuspendLayout();
      this.pnUp.Controls.Add((Control) this.txtSearch);
      this.pnUp.Controls.Add((Control) this.lblSearch);
      this.pnUp.Dock = DockStyle.Top;
      this.pnUp.Location = new Point(0, 0);
      this.pnUp.Name = "pnUp";
      this.pnUp.Size = new Size(765, 36);
      this.pnUp.TabIndex = 1;
      this.txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSearch.Location = new Point(92, 6);
      this.txtSearch.Name = "txtSearch";
      this.txtSearch.Size = new Size(661, 22);
      this.txtSearch.TabIndex = 1;
      this.txtSearch.KeyUp += new KeyEventHandler(this.txtSearch_KeyUp);
      this.lblSearch.AutoSize = true;
      this.lblSearch.Location = new Point(12, 9);
      this.lblSearch.Name = "lblSearch";
      this.lblSearch.Size = new Size(74, 16);
      this.lblSearch.TabIndex = 0;
      this.lblSearch.Text = "Название";
      this.ts.Font = new Font("Tahoma", 10f);
      this.ts.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.tsbAdd,
        (ToolStripItem) this.tsbApplay,
        (ToolStripItem) this.tsbCancel,
        (ToolStripItem) this.tsbDelete
      });
      this.ts.Location = new Point(0, 36);
      this.ts.Name = "ts";
      this.ts.Size = new Size(765, 25);
      this.ts.TabIndex = 2;
      this.ts.Text = "toolStrip1";
      this.tsbAdd.Image = (Image) Resources.add_var;
      this.tsbAdd.ImageTransparentColor = Color.Magenta;
      this.tsbAdd.Name = "tsbAdd";
      this.tsbAdd.Size = new Size(91, 22);
      this.tsbAdd.Text = "Добавить";
      this.tsbAdd.Click += new EventHandler(this.btnAdd_Click);
      this.tsbApplay.Enabled = false;
      this.tsbApplay.Image = (Image) Resources.Applay_var;
      this.tsbApplay.ImageTransparentColor = Color.Magenta;
      this.tsbApplay.Name = "tsbApplay";
      this.tsbApplay.Size = new Size(99, 22);
      this.tsbApplay.Text = "Сохранить";
      this.tsbApplay.Click += new EventHandler(this.btnSave_Click);
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
      this.tsbDelete.Click += new EventHandler(this.btnDelete_Click);
      this.dgvBank.BackgroundColor = Color.AliceBlue;
      this.dgvBank.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvBank.Dock = DockStyle.Fill;
      this.dgvBank.Location = new Point(0, 61);
      this.dgvBank.Name = "dgvBank";
      this.dgvBank.Size = new Size(765, 221);
      this.dgvBank.TabIndex = 3;
      this.dgvBank.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvBank_CellBeginEdit);
      this.dgvBank.CellMouseClick += new DataGridViewCellMouseEventHandler(this.dgvBank_CellMouseClick);
      this.dgvBank.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvBank_ColumnWidthChanged);
      this.dgvBank.DataError += new DataGridViewDataErrorEventHandler(this.dgvBank_DataError);
      this.hp.HelpNamespace = "Help.chm";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(765, 322);
      this.Controls.Add((Control) this.dgvBank);
      this.Controls.Add((Control) this.ts);
      this.Controls.Add((Control) this.pnUp);
      this.hp.SetHelpKeyword((Control) this, "kv514.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      this.Name = "FrmBank";
      this.hp.SetShowHelp((Control) this, true);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Банки";
      this.Load += new EventHandler(this.FrmBank_Load);
      this.Controls.SetChildIndex((Control) this.pnUp, 0);
      this.Controls.SetChildIndex((Control) this.ts, 0);
      this.Controls.SetChildIndex((Control) this.dgvBank, 0);
      this.pnUp.ResumeLayout(false);
      this.pnUp.PerformLayout();
      this.ts.ResumeLayout(false);
      this.ts.PerformLayout();
      ((ISupportInitialize) this.dgvBank).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
