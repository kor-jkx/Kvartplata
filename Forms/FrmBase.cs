// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmBase
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using NHibernate;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmBase : Form
  {
    protected IList objectsList = (IList) new ArrayList();
    protected object newObject = (object) null;
    protected bool isEdit = false;
    private FormStateSaver fss = new FormStateSaver(FrmBase.ic);
    protected string Adm_DelQuery = "";
    private IContainer components = (IContainer) null;
    protected object curObject;
    protected ISession session;
    private static IContainer ic;
    private ToolStripButton tsbAdd;
    private ToolStripButton tsbApplay;
    private ToolStripButton tsbCancel;
    private ToolStripButton tsbDelete;
    private ToolStripButton tsbExit;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private Panel pnBtn;
    private Button btnExit;
    public DataGridView dgvBase;
    public ToolStrip toolStrip1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private DataGridViewTextBoxColumn ID;
    private DataGridViewTextBoxColumn nameColumn;
    private DataGridViewTextBoxColumn UName;
    private DataGridViewTextBoxColumn DEdit;

    public FrmBase()
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
      if (Options.ViewEdit)
        return;
      this.dgvBase.Columns[2].Visible = false;
      this.dgvBase.Columns[3].Visible = false;
    }

    protected void SelectRow()
    {
      if (this.curObject != null)
      {
        int curObject = KvrplHelper.FindCurObject(this.objectsList, this.curObject);
        if (curObject < 0)
          return;
        this.dgvBase.CurrentCell = this.dgvBase.Rows[curObject].Cells[1];
        this.dgvBase.Rows[curObject].Selected = true;
      }
      else if (this.dgvBase.CurrentRow != null && this.dgvBase.CurrentRow.Index < this.objectsList.Count)
        this.curObject = this.objectsList[this.dgvBase.CurrentRow.Index];
    }

    protected virtual void GetList()
    {
    }

    private void dgvBase_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.tsbAdd.Enabled = false;
      this.tsbApplay.Enabled = true;
      this.tsbCancel.Enabled = true;
      this.tsbDelete.Enabled = false;
    }

    private void dgvBase_SelectionChanged(object sender, EventArgs e)
    {
      if (this.dgvBase.CurrentRow == null || this.dgvBase.CurrentRow.Index >= this.objectsList.Count)
        return;
      this.curObject = this.objectsList[this.dgvBase.CurrentRow.Index];
    }

    protected virtual void tsbAdd_Click(object sender, EventArgs e)
    {
      this.objectsList.Add(this.newObject);
      this.dgvBase.RowCount = this.objectsList.Count;
      this.dgvBase.CurrentCell = this.dgvBase.Rows[this.dgvBase.Rows.Count - 1].Cells[1];
      this.dgvBase.Rows[this.dgvBase.Rows.Count - 1].Selected = true;
      this.tsbAdd.Enabled = false;
      this.tsbApplay.Enabled = true;
      this.tsbCancel.Enabled = true;
      this.tsbDelete.Enabled = false;
    }

    protected virtual void tsbApplay_Click(object sender, EventArgs e)
    {
      this.dgvBase.EndEdit();
      if (!this.isEdit)
      {
        this.isEdit = true;
      }
      else
      {
        if (this.newObject != null)
        {
          try
          {
            this.session.Save(this.newObject);
            this.session.Flush();
            this.session.Refresh(this.newObject);
            this.curObject = this.newObject;
            this.GetList();
            this.newObject = (object) null;
          }
          catch (Exception ex)
          {
            if (ex.Message.ToLower().IndexOf("a different object with the same identifier value was already associated with the session") != -1 || ex.InnerException.Message.ToLower().IndexOf("primary key for table 'di_phonesserv' is not unique") != -1)
            {
              KvrplHelper.ResetGeners("DI_PHONESSERV", "idservice");
              int num = (int) MessageBox.Show("Была устранена ошибка генерации уникального поля! Введите запись заново!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
              int num1 = (int) MessageBox.Show("Ошибка вставки! Проверьте правильность ввода данных.", "Внимание!", MessageBoxButtons.OK);
            }
            this.session.Clear();
            this.session = Domain.CurrentSession;
            return;
          }
        }
        else
        {
          try
          {
            if (this.session.IsDirty())
              this.session.Flush();
          }
          catch
          {
            int num = (int) MessageBox.Show("Не удалось внести изменения! Проверьте правильность ввода данных.", "Внимание!", MessageBoxButtons.OK);
            this.session.Clear();
            this.session = Domain.CurrentSession;
          }
        }
        this.GetList();
        this.tsbAdd.Enabled = true;
        this.tsbApplay.Enabled = false;
        this.tsbCancel.Enabled = false;
        this.tsbDelete.Enabled = true;
      }
    }

    protected virtual void tsbCancel_Click(object sender, EventArgs e)
    {
      this.dgvBase.EndEdit();
      if (this.newObject != null)
      {
        this.objectsList.Remove(this.newObject);
        this.newObject = (object) null;
      }
      foreach (object objects in (IEnumerable) this.objectsList)
        this.session.Refresh(objects);
      this.GetList();
      this.tsbAdd.Enabled = true;
      this.tsbApplay.Enabled = false;
      this.tsbCancel.Enabled = false;
      this.tsbDelete.Enabled = true;
    }

    protected virtual void tsbDelete_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      if (this.curObject != null)
      {
        try
        {
          this.session.Delete(this.curObject);
          this.session.Flush();
          this.curObject = (object) null;
          this.GetList();
        }
        catch
        {
          int num = (int) MessageBox.Show("Удаление невозможно! Существуют данные, ссылающиеся на эту запись.", "Внимание!", MessageBoxButtons.OK);
          this.session.Clear();
          this.session = Domain.CurrentSession;
          this.curObject = (object) null;
          this.GetList();
          this.Adm_DelQuery = "";
        }
      }
    }

    private void tsbExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    protected virtual void dgvBase_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
    }

    protected virtual void dgvBase_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
    }

    protected virtual void FrmBase_Load(object sender, EventArgs e)
    {
      this.GetList();
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void dgvBase_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    protected virtual void SetEnable()
    {
      this.tsbAdd.Enabled = false;
      this.tsbDelete.Enabled = false;
      this.tsbApplay.Enabled = false;
      this.tsbCancel.Enabled = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmBase));
      this.toolStrip1 = new ToolStrip();
      this.tsbAdd = new ToolStripButton();
      this.tsbApplay = new ToolStripButton();
      this.tsbCancel = new ToolStripButton();
      this.tsbDelete = new ToolStripButton();
      this.tsbExit = new ToolStripButton();
      this.pnBtn = new Panel();
      this.btnExit = new Button();
      this.dgvBase = new DataGridView();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
      this.ID = new DataGridViewTextBoxColumn();
      this.nameColumn = new DataGridViewTextBoxColumn();
      this.UName = new DataGridViewTextBoxColumn();
      this.DEdit = new DataGridViewTextBoxColumn();
      this.toolStrip1.SuspendLayout();
      this.pnBtn.SuspendLayout();
      ((ISupportInitialize) this.dgvBase).BeginInit();
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
      this.toolStrip1.Size = new Size(581, 24);
      this.toolStrip1.TabIndex = 0;
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
      this.tsbExit.Click += new EventHandler(this.tsbExit_Click);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 466);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(581, 40);
      this.pnBtn.TabIndex = 1;
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(487, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(82, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.dgvBase.AllowUserToAddRows = false;
      this.dgvBase.AllowUserToDeleteRows = false;
      this.dgvBase.BackgroundColor = Color.AliceBlue;
      this.dgvBase.CausesValidation = false;
      this.dgvBase.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvBase.Columns.AddRange((DataGridViewColumn) this.ID, (DataGridViewColumn) this.nameColumn, (DataGridViewColumn) this.UName, (DataGridViewColumn) this.DEdit);
      this.dgvBase.Dock = DockStyle.Fill;
      this.dgvBase.Location = new Point(0, 24);
      this.dgvBase.Margin = new Padding(4);
      this.dgvBase.Name = "dgvBase";
      this.dgvBase.Size = new Size(581, 442);
      this.dgvBase.TabIndex = 3;
      this.dgvBase.VirtualMode = true;
      this.dgvBase.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvBase_CellBeginEdit);
      this.dgvBase.CellValueNeeded += new DataGridViewCellValueEventHandler(this.dgvBase_CellValueNeeded);
      this.dgvBase.CellValuePushed += new DataGridViewCellValueEventHandler(this.dgvBase_CellValuePushed);
      this.dgvBase.DataError += new DataGridViewDataErrorEventHandler(this.dgvBase_DataError);
      this.dgvBase.SelectionChanged += new EventHandler(this.dgvBase_SelectionChanged);
      this.dataGridViewTextBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn1.DataPropertyName = "ID";
      this.dataGridViewTextBoxColumn1.HeaderText = "Наименование";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn2.DataPropertyName = "nameColumn";
      this.dataGridViewTextBoxColumn2.HeaderText = "Наименование";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn3.DataPropertyName = "UName";
      this.dataGridViewTextBoxColumn3.HeaderText = "Пользователь";
      this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
      this.dataGridViewTextBoxColumn3.ReadOnly = true;
      this.dataGridViewTextBoxColumn3.Width = 110;
      this.dataGridViewTextBoxColumn4.DataPropertyName = "DEdit";
      this.dataGridViewTextBoxColumn4.HeaderText = "Дата редактирования";
      this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
      this.dataGridViewTextBoxColumn4.ReadOnly = true;
      this.dataGridViewTextBoxColumn4.Width = 120;
      this.ID.HeaderText = "№";
      this.ID.Name = "ID";
      this.ID.Width = 70;
      this.nameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.nameColumn.HeaderText = "Наименование";
      this.nameColumn.Name = "nameColumn";
      this.UName.HeaderText = "Пользователь";
      this.UName.Name = "UName";
      this.UName.ReadOnly = true;
      this.UName.Width = 110;
      this.DEdit.HeaderText = "Дата редактирования";
      this.DEdit.Name = "DEdit";
      this.DEdit.ReadOnly = true;
      this.DEdit.Width = 120;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(581, 506);
      this.Controls.Add((Control) this.dgvBase);
      this.Controls.Add((Control) this.pnBtn);
      this.Controls.Add((Control) this.toolStrip1);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmBase";
      this.Text = "FrmBase";
      this.Load += new EventHandler(this.FrmBase_Load);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.pnBtn.ResumeLayout(false);
      ((ISupportInitialize) this.dgvBase).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
