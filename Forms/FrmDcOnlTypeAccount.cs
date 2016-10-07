// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmDcOnlTypeAccount
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmDcOnlTypeAccount : Form
  {
    private bool _readOnly = false;
    private IContainer components = (IContainer) null;
    private ISession _session;
    private IList<dcohlTypeAccount> ohlAccountsList;
    private dcohlTypeAccount newObject;
    private dcohlTypeAccount curObject;
    private Company _company;
    private ToolStrip tsMenu;
    private ToolStripButton tsbAdd;
    private ToolStripButton tsbSave;
    private ToolStripButton tsbCancel;
    private ToolStripButton tsbDelete;
    private ToolStripButton tsbExit;
    private DataGridView dgvTypeAccount;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private MaskDateColumn maskDateColumn1;
    private DataGridViewTextBoxColumn TypeAccountStr;
    private DataGridViewTextBoxColumn TypeAccountName;
    private DataGridViewTextBoxColumn Uname;
    private DataGridViewTextBoxColumn Dedit;

    public FrmDcOnlTypeAccount()
    {
      this.InitializeComponent();
    }

    public FrmDcOnlTypeAccount(Company company)
    {
      this.InitializeComponent();
      this._company = company;
      this.CheckAccess();
      this.dgvTypeAccount.AutoGenerateColumns = false;
    }

    private void CheckAccess()
    {
      this._readOnly = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(32, this._company, false));
      this.tsMenu.Visible = this._readOnly;
      this.dgvTypeAccount.ReadOnly = !this._readOnly;
    }

    private void FrmDcOnlTypeAccount_Load(object sender, EventArgs e)
    {
      this._session = Domain.CurrentSession;
      this._session.Clear();
      this.GetData();
    }

    private void GetData()
    {
      this.ohlAccountsList = this._session.CreateQuery("from dcohlTypeAccount").List<dcohlTypeAccount>();
      this.dgvTypeAccount.DataSource = (object) this.ohlAccountsList;
    }

    private void tsbAdd_Click(object sender, EventArgs e)
    {
      this.newObject = new dcohlTypeAccount();
      this.newObject.Uname = Options.Login;
      this.newObject.Dedit = DateTime.Now.Date;
      this.newObject.TypeAccountName = "";
      this.newObject.TypeAccountStr = "0";
      if ((uint) this.dgvTypeAccount.Rows.Count > 0U)
        this.ohlAccountsList = this.dgvTypeAccount.DataSource as IList<dcohlTypeAccount>;
      if (this.ohlAccountsList != null)
      {
        this.ohlAccountsList.Add(this.newObject);
        this.dgvTypeAccount.DataSource = (object) null;
        if (this.ohlAccountsList.Count<dcohlTypeAccount>() > 0)
        {
          this.dgvTypeAccount.DataSource = (object) this.ohlAccountsList;
          this.dgvTypeAccount.CurrentCell = this.dgvTypeAccount.Rows[this.dgvTypeAccount.Rows.Count - 1].Cells[this.dgvTypeAccount.Columns.Count - 1];
          this.dgvTypeAccount.Rows[this.dgvTypeAccount.Rows.Count - 1].Selected = true;
          this.dgvTypeAccount.Refresh();
        }
      }
      this.tsbAdd.Enabled = false;
      this.tsbSave.Enabled = true;
      this.tsbCancel.Enabled = true;
      this.tsbDelete.Enabled = false;
    }

    private void tsbSave_Click(object sender, EventArgs e)
    {
      this.dgvTypeAccount.EndEdit();
      this._session.Clear();
      if (this.newObject != null)
      {
        try
        {
          try
          {
            Convert.ToInt32(this.dgvTypeAccount.CurrentRow.Cells["TypeAccountStr"].Value);
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Введите номер", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          if (this.dgvTypeAccount.CurrentRow.Cells["TypeAccountStr"].Value == null)
          {
            int num = (int) MessageBox.Show("Введите номер", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          if (this._session.CreateQuery("select ta from dcohlTypeAccount ta where ta.TypeAccountId=:id").SetParameter<int>("id", Convert.ToInt32(this.dgvTypeAccount.CurrentRow.Cells["TypeAccountStr"].Value)).UniqueResult() != null)
          {
            int num = (int) MessageBox.Show("Запись с таким номером существует", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          this.newObject.Uname = Options.Login;
          this.newObject.Dedit = DateTime.Now.Date;
          this.newObject.TypeAccountId = Convert.ToInt32(this.dgvTypeAccount.CurrentRow.Cells["TypeAccountStr"].Value);
          if (this.dgvTypeAccount.CurrentRow.Cells["TypeAccountName"].Value == null)
          {
            int num = (int) MessageBox.Show("Введите \"Наименование типа\"", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          if (this.dgvTypeAccount.CurrentRow.Cells["TypeAccountName"].Value.ToString().Length > 30)
          {
            int num = (int) MessageBox.Show("Введите \"Наименование типа\" не более 30 символов", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          this.newObject.TypeAccountName = this.dgvTypeAccount.CurrentRow.Cells["TypeAccountName"].Value.ToString();
          this._session.Save((object) this.newObject);
          this._session.Flush();
          this._session.Refresh((object) this.newObject);
          this.GetData();
          this.newObject = (dcohlTypeAccount) null;
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(ex.Message, "Внимание!", MessageBoxButtons.OK);
          return;
        }
      }
      else
      {
        try
        {
          if (this.dgvTypeAccount.CurrentRow.Cells["TypeAccountName"].Value == null)
          {
            int num = (int) MessageBox.Show("Введите \"Наименование типа\"", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          if (this.dgvTypeAccount.CurrentRow.Cells["TypeAccountName"].Value.ToString().Length > 30)
          {
            int num = (int) MessageBox.Show("Введите \"Наименование типа\" не более 30 символов", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          dcohlTypeAccount dataBoundItem = (dcohlTypeAccount) this.dgvTypeAccount.Rows[this.dgvTypeAccount.CurrentRow.Index].DataBoundItem;
          dataBoundItem.TypeAccountName = this.dgvTypeAccount.CurrentRow.Cells["TypeAccountName"].Value.ToString();
          dataBoundItem.Uname = Options.Login;
          dataBoundItem.Dedit = DateTime.Now.Date;
          this._session.Update((object) dataBoundItem);
          this._session.Flush();
          this._session.Refresh((object) dataBoundItem);
          this.GetData();
          this.dgvTypeAccount.Columns["TypeAccountStr"].ReadOnly = false;
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(ex.Message, "Внимание!", MessageBoxButtons.OK);
          return;
        }
      }
      this.tsbAdd.Enabled = true;
      this.tsbSave.Enabled = false;
      this.tsbCancel.Enabled = false;
      this.tsbDelete.Enabled = true;
    }

    private void tsbCancel_Click(object sender, EventArgs e)
    {
      this.dgvTypeAccount.EndEdit();
      if (this.newObject != null)
      {
        this.ohlAccountsList.Remove(this.newObject);
        this.newObject = (dcohlTypeAccount) null;
      }
      foreach (object ohlAccounts in (IEnumerable<dcohlTypeAccount>) this.ohlAccountsList)
        this._session.Refresh(ohlAccounts);
      this.tsbAdd.Enabled = true;
      this.tsbSave.Enabled = false;
      this.tsbCancel.Enabled = false;
      this.tsbDelete.Enabled = true;
      this.dgvTypeAccount.Columns["TypeAccountStr"].ReadOnly = false;
      this.GetData();
    }

    private void tsbExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void dgvTypeAccount_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.tsbAdd.Enabled = false;
      this.tsbSave.Enabled = true;
      this.tsbCancel.Enabled = true;
      this.tsbDelete.Enabled = false;
      this.dgvTypeAccount.Columns["TypeAccountStr"].ReadOnly = true;
    }

    private void dgvTypeAccount_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (!this._readOnly)
        return;
      this.Close();
    }

    private void dgvTypeAccount_SelectionChanged(object sender, EventArgs e)
    {
      if (this.dgvTypeAccount.CurrentRow == null || this.dgvTypeAccount.CurrentRow.Index >= this.ohlAccountsList.Count)
        return;
      this.curObject = (dcohlTypeAccount) this.dgvTypeAccount.Rows[this.dgvTypeAccount.CurrentRow.Index].DataBoundItem;
    }

    private void tsbDelete_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      if (this.curObject != null)
      {
        try
        {
          this._session.Delete((object) this.curObject);
          this._session.Flush();
          this.curObject = (dcohlTypeAccount) null;
          this.GetData();
        }
        catch
        {
          int num = (int) MessageBox.Show("Удаление невозможно! Существуют данные, ссылающиеся на эту запись.", "Внимание!", MessageBoxButtons.OK);
          this._session.Clear();
          this._session = Domain.CurrentSession;
          this.curObject = (dcohlTypeAccount) null;
          this.GetData();
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmDcOnlTypeAccount));
      this.tsMenu = new ToolStrip();
      this.tsbAdd = new ToolStripButton();
      this.tsbSave = new ToolStripButton();
      this.tsbCancel = new ToolStripButton();
      this.tsbDelete = new ToolStripButton();
      this.tsbExit = new ToolStripButton();
      this.dgvTypeAccount = new DataGridView();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
      this.TypeAccountStr = new DataGridViewTextBoxColumn();
      this.TypeAccountName = new DataGridViewTextBoxColumn();
      this.Uname = new DataGridViewTextBoxColumn();
      this.Dedit = new DataGridViewTextBoxColumn();
      this.maskDateColumn1 = new MaskDateColumn();
      this.tsMenu.SuspendLayout();
      ((ISupportInitialize) this.dgvTypeAccount).BeginInit();
      this.SuspendLayout();
      this.tsMenu.Items.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.tsbAdd,
        (ToolStripItem) this.tsbSave,
        (ToolStripItem) this.tsbCancel,
        (ToolStripItem) this.tsbDelete,
        (ToolStripItem) this.tsbExit
      });
      this.tsMenu.Location = new Point(0, 0);
      this.tsMenu.Name = "tsMenu";
      this.tsMenu.Size = new Size(840, 25);
      this.tsMenu.TabIndex = 0;
      this.tsMenu.Text = "toolStrip1";
      this.tsbAdd.Font = new Font("Tahoma", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.tsbAdd.Image = (Image) Resources.add_var;
      this.tsbAdd.ImageTransparentColor = Color.Magenta;
      this.tsbAdd.Name = "tsbAdd";
      this.tsbAdd.Size = new Size(91, 22);
      this.tsbAdd.Text = "Добавить";
      this.tsbAdd.Click += new EventHandler(this.tsbAdd_Click);
      this.tsbSave.Enabled = false;
      this.tsbSave.Font = new Font("Tahoma", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.tsbSave.Image = (Image) Resources.Applay_var;
      this.tsbSave.ImageTransparentColor = Color.Magenta;
      this.tsbSave.Name = "tsbSave";
      this.tsbSave.Size = new Size(99, 22);
      this.tsbSave.Text = "Сохранить";
      this.tsbSave.Click += new EventHandler(this.tsbSave_Click);
      this.tsbCancel.Enabled = false;
      this.tsbCancel.Font = new Font("Tahoma", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.tsbCancel.Image = (Image) Resources.undo;
      this.tsbCancel.ImageTransparentColor = Color.Magenta;
      this.tsbCancel.Name = "tsbCancel";
      this.tsbCancel.Size = new Size(77, 22);
      this.tsbCancel.Text = "Отмена";
      this.tsbCancel.Click += new EventHandler(this.tsbCancel_Click);
      this.tsbDelete.Font = new Font("Tahoma", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.tsbDelete.Image = (Image) Resources.delete_var;
      this.tsbDelete.ImageTransparentColor = Color.Magenta;
      this.tsbDelete.Name = "tsbDelete";
      this.tsbDelete.Size = new Size(82, 22);
      this.tsbDelete.Text = "Удалить";
      this.tsbDelete.Click += new EventHandler(this.tsbDelete_Click);
      this.tsbExit.Font = new Font("Tahoma", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tsbExit.Image = (Image) Resources.Exit;
      this.tsbExit.ImageTransparentColor = Color.Magenta;
      this.tsbExit.Name = "tsbExit";
      this.tsbExit.Size = new Size(70, 22);
      this.tsbExit.Text = "Выход";
      this.tsbExit.Visible = false;
      this.tsbExit.Click += new EventHandler(this.tsbExit_Click);
      this.dgvTypeAccount.AllowUserToAddRows = false;
      this.dgvTypeAccount.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      this.dgvTypeAccount.BackgroundColor = Color.AliceBlue;
      this.dgvTypeAccount.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvTypeAccount.Columns.AddRange((DataGridViewColumn) this.TypeAccountStr, (DataGridViewColumn) this.TypeAccountName, (DataGridViewColumn) this.Uname, (DataGridViewColumn) this.Dedit);
      this.dgvTypeAccount.Dock = DockStyle.Fill;
      this.dgvTypeAccount.Location = new Point(0, 25);
      this.dgvTypeAccount.MultiSelect = false;
      this.dgvTypeAccount.Name = "dgvTypeAccount";
      this.dgvTypeAccount.Size = new Size(840, 523);
      this.dgvTypeAccount.TabIndex = 1;
      this.dgvTypeAccount.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvTypeAccount_CellBeginEdit);
      this.dgvTypeAccount.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dgvTypeAccount_CellMouseDoubleClick);
      this.dgvTypeAccount.SelectionChanged += new EventHandler(this.dgvTypeAccount_SelectionChanged);
      this.dataGridViewTextBoxColumn1.DataPropertyName = "TypeAccountId";
      this.dataGridViewTextBoxColumn1.HeaderText = "№";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.ReadOnly = true;
      this.dataGridViewTextBoxColumn1.Width = 199;
      this.dataGridViewTextBoxColumn2.DataPropertyName = "TypeAccountName";
      this.dataGridViewTextBoxColumn2.HeaderText = "Наименование типа";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn2.Width = 150;
      this.dataGridViewTextBoxColumn3.DataPropertyName = "Uname";
      this.dataGridViewTextBoxColumn3.HeaderText = "Пользователь";
      this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
      this.dataGridViewTextBoxColumn3.ReadOnly = true;
      this.dataGridViewTextBoxColumn3.Width = 150;
      this.dataGridViewTextBoxColumn4.DataPropertyName = "Dedit";
      this.dataGridViewTextBoxColumn4.HeaderText = "Дата редактирования";
      this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
      this.dataGridViewTextBoxColumn4.ReadOnly = true;
      this.dataGridViewTextBoxColumn4.Resizable = DataGridViewTriState.True;
      this.dataGridViewTextBoxColumn4.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.dataGridViewTextBoxColumn4.Width = 150;
      this.TypeAccountStr.DataPropertyName = "TypeAccountStr";
      this.TypeAccountStr.HeaderText = "№";
      this.TypeAccountStr.Name = "TypeAccountStr";
      this.TypeAccountName.DataPropertyName = "TypeAccountName";
      this.TypeAccountName.HeaderText = "Наименование типа";
      this.TypeAccountName.Name = "TypeAccountName";
      this.Uname.DataPropertyName = "Uname";
      this.Uname.HeaderText = "Пользователь";
      this.Uname.Name = "Uname";
      this.Uname.ReadOnly = true;
      this.Dedit.DataPropertyName = "Dedit";
      this.Dedit.HeaderText = "Дата редактирования";
      this.Dedit.Name = "Dedit";
      this.Dedit.ReadOnly = true;
      this.Dedit.Resizable = DataGridViewTriState.True;
      this.Dedit.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.maskDateColumn1.DataPropertyName = "Dedit";
      this.maskDateColumn1.HeaderText = "Дата редактирования";
      this.maskDateColumn1.Name = "maskDateColumn1";
      this.maskDateColumn1.ReadOnly = true;
      this.maskDateColumn1.Resizable = DataGridViewTriState.True;
      this.maskDateColumn1.SortMode = DataGridViewColumnSortMode.Automatic;
      this.maskDateColumn1.Width = 199;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(840, 548);
      this.Controls.Add((Control) this.dgvTypeAccount);
      this.Controls.Add((Control) this.tsMenu);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmDcOnlTypeAccount";
      this.Text = "Типы счетов";
      this.Load += new EventHandler(this.FrmDcOnlTypeAccount_Load);
      this.tsMenu.ResumeLayout(false);
      this.tsMenu.PerformLayout();
      ((ISupportInitialize) this.dgvTypeAccount).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
