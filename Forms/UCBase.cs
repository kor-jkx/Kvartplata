// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.UCBase
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using NHibernate;
using SaveSettings;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class UCBase : UserControl
  {
    protected GridSettings MySettings = new GridSettings();
    protected IList objectsList = (IList) new ArrayList();
    protected object newObject = (object) null;
    protected DateTime? dt = new DateTime?();
    protected int CurIndex = -1;
    protected string UpdateQuery = "";
    protected string Adm_DelQuery = "";
    private IContainer components = (IContainer) null;
    protected object curObject;
    public ISession session;
    public DataGridView dgvBase;
    public ToolStripButton tsbExit;
    private ToolStripMenuItem tsmCopy;
    private ToolStripMenuItem tsmCopyInPast;
    public ToolStripButton tsbApplay;
    private ContextMenuStrip Menu;
    public ToolStrip toolStrip1;
    public ToolStripButton tsbCancel;
    public ToolStripButton tsbAdd;
    public ToolStripButton tsbDelete;

    public virtual event EventHandler ObjectsListChanged;

    public virtual event EventHandler CurObjectChanged;

    public UCBase()
    {
      this.InitializeComponent();
      this.SetGridConfigFileSettings();
    }

    protected void SelectRow()
    {
      if (this.curObject != null)
      {
        int curObject = KvrplHelper.FindCurObject(this.objectsList, this.curObject);
        if (curObject < 0)
          return;
        this.dgvBase.CurrentCell = !this.dgvBase.Rows[curObject].Cells[0].Visible ? this.dgvBase.Rows[curObject].Cells[1] : this.dgvBase.Rows[curObject].Cells[0];
        this.dgvBase.Rows[curObject].Selected = true;
      }
      else if (this.dgvBase.CurrentRow != null && this.dgvBase.CurrentRow.Index < this.objectsList.Count)
        this.curObject = this.objectsList[this.dgvBase.CurrentRow.Index];
    }

    protected virtual void GetList()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.ObjectsListChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.ObjectsListChanged((object) this, EventArgs.Empty);
    }

    public void LoadSettings()
    {
      try
      {
        this.MySettings.Load();
        foreach (DataGridViewColumn column in (BaseCollection) this.dgvBase.Columns)
          this.MySettings.GetMySettings(column);
      }
      catch
      {
      }
    }

    public void CancelEnabled()
    {
      this.tsbAdd.Enabled = true;
      this.tsbApplay.Enabled = false;
      this.tsbCancel.Enabled = false;
      this.tsbDelete.Enabled = true;
    }

    public void SetGridConfigFileSettings()
    {
      this.MySettings.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
    }

    public void dgvBase_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.tsbAdd.Enabled = false;
      this.tsbApplay.Enabled = true;
      this.tsbCancel.Enabled = true;
      this.tsbDelete.Enabled = false;
    }

    protected virtual void dgvBase_SelectionChanged(object sender, EventArgs e)
    {
      if (this.dgvBase.CurrentRow == null || this.dgvBase.CurrentRow.Index >= this.objectsList.Count)
        return;
      this.curObject = this.objectsList[this.dgvBase.CurrentRow.Index];
      // ISSUE: reference to a compiler-generated field
      if (this.CurObjectChanged != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.CurObjectChanged((object) this, EventArgs.Empty);
      }
    }

    protected virtual void tsbAdd_Click(object sender, EventArgs e)
    {
      this.objectsList.Add(this.newObject);
      this.dgvBase.RowCount = this.objectsList.Count;
      this.dgvBase.CurrentCell = this.dgvBase.Rows[this.dgvBase.Rows.Count - 1].Cells[this.dgvBase.Columns.Count - 1];
      this.dgvBase.Rows[this.dgvBase.Rows.Count - 1].Selected = true;
      this.tsbAdd.Enabled = false;
      this.tsbApplay.Enabled = true;
      this.tsbCancel.Enabled = true;
      this.tsbDelete.Enabled = false;
    }

    protected virtual void tsbApplay_Click(object sender, EventArgs e)
    {
      this.Menu.Enabled = true;
      this.dgvBase.EndEdit();
      if (this.newObject != null)
      {
        try
        {
          this.session.Save(this.newObject);
          this.session.Flush();
          this.curObject = this.newObject;
        }
        catch (Exception ex)
        {
          if (ex.InnerException != null && ex.InnerException.Message.ToLower().IndexOf("primary key for table 'dcnorm' is not unique") != -1)
          {
            KvrplHelper.ResetGeners("dcNorm", "Norm_id");
            int num = (int) MessageBox.Show("Была устранена ошибка генерации уникального поля! Введите запись заново!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return;
          }
          if (ex.InnerException != null && ex.InnerException.Message.ToLower().IndexOf("primary key for table 'dctariff' is not unique") != -1)
          {
            KvrplHelper.ResetGeners("dcTariff", "Tariff_id");
            int num = (int) MessageBox.Show("Была устранена ошибка генерации уникального поля! Введите запись заново!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return;
          }
          int num1 = (int) MessageBox.Show("Ошибка вставки! Проверьте правильность ввода данных.", "Внимание!", MessageBoxButtons.OK);
          return;
        }
      }
      else
      {
        try
        {
          this.session.Update(this.curObject);
          this.session.Flush();
        }
        catch
        {
          int num = (int) MessageBox.Show("Не удалось внести изменения! Проверьте правильность ввода данных.", "Внимание!", MessageBoxButtons.OK);
          return;
        }
      }
      this.tsbAdd.Enabled = true;
      this.tsbApplay.Enabled = false;
      this.tsbCancel.Enabled = false;
      this.tsbDelete.Enabled = true;
      this.GetList();
      // ISSUE: reference to a compiler-generated field
      if (this.ObjectsListChanged != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.ObjectsListChanged((object) this, EventArgs.Empty);
      }
      this.newObject = (object) null;
    }

    protected virtual void tsbCancel_Click(object sender, EventArgs e)
    {
      this.Menu.Enabled = true;
      this.dgvBase.EndEdit();
      bool flag = this.newObject == this.curObject;
      if (this.newObject != null)
      {
        this.objectsList.Remove(this.newObject);
        this.curObject = this.objectsList.Count <= 0 ? (object) null : this.objectsList[this.objectsList.Count - 1];
        if (flag)
          this.newObject = (object) null;
      }
      try
      {
        foreach (object objects in (IEnumerable) this.objectsList)
          this.session.Refresh(objects);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      this.GetList();
      this.tsbAdd.Enabled = true;
      this.tsbApplay.Enabled = false;
      this.tsbCancel.Enabled = false;
      this.tsbDelete.Enabled = true;
    }

    protected virtual void tsbDelete_Click(object sender, EventArgs e)
    {
      if (this.curObject == null)
        return;
      try
      {
        this.session.Delete(this.curObject);
        this.session.Flush();
        this.curObject = (object) null;
        this.GetList();
        // ISSUE: reference to a compiler-generated field
        if (this.ObjectsListChanged != null)
        {
          // ISSUE: reference to a compiler-generated field
          this.ObjectsListChanged((object) this, EventArgs.Empty);
        }
      }
      catch
      {
        int num = (int) MessageBox.Show("Не удалось удалить запись!", "Внимание!", MessageBoxButtons.OK);
        this.session.Clear();
        this.session = Domain.CurrentSession;
        this.Adm_DelQuery = "";
      }
      if (this.dgvBase.CurrentRow != null)
      {
        if (this.dgvBase.CurrentRow.Index < this.objectsList.Count)
          this.session.Refresh(this.objectsList[this.dgvBase.CurrentRow.Index]);
        if (this.dgvBase.CurrentRow.Index > 0)
          this.session.Refresh(this.objectsList[this.dgvBase.CurrentRow.Index - 1]);
        if (this.dgvBase.CurrentRow.Index == 0 && this.objectsList.Count > 0)
          this.session.Refresh(this.objectsList[this.dgvBase.CurrentRow.Index]);
      }
    }

    protected virtual void dgvBase_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
    }

    protected virtual void dgvBase_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
    }

    private void FrmBase_Load(object sender, EventArgs e)
    {
    }

    protected virtual void tsmCopy_Click(object sender, EventArgs e)
    {
      this.Menu.Enabled = false;
    }

    protected virtual void tsmCopyInPast_Click(object sender, EventArgs e)
    {
      this.Menu.Enabled = false;
      // ISSUE: reference to a compiler-generated field
      if (this.ObjectsListChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.ObjectsListChanged((object) this, EventArgs.Empty);
    }

    protected virtual void dgvBase_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
    }

    protected virtual void dgvBase_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettings.FindByName(e.Column.Name) < 0)
        return;
      this.MySettings.Columns[this.MySettings.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettings.Save();
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
      this.dgvBase = new DataGridView();
      this.Menu = new ContextMenuStrip(this.components);
      this.tsmCopy = new ToolStripMenuItem();
      this.tsmCopyInPast = new ToolStripMenuItem();
      this.toolStrip1 = new ToolStrip();
      this.tsbAdd = new ToolStripButton();
      this.tsbApplay = new ToolStripButton();
      this.tsbCancel = new ToolStripButton();
      this.tsbDelete = new ToolStripButton();
      this.tsbExit = new ToolStripButton();
      ((ISupportInitialize) this.dgvBase).BeginInit();
      this.Menu.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      this.dgvBase.AllowUserToAddRows = false;
      this.dgvBase.AllowUserToDeleteRows = false;
      this.dgvBase.BackgroundColor = Color.AliceBlue;
      this.dgvBase.CausesValidation = false;
      this.dgvBase.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvBase.ContextMenuStrip = this.Menu;
      this.dgvBase.Dock = DockStyle.Fill;
      this.dgvBase.Location = new Point(0, 24);
      this.dgvBase.Margin = new Padding(4);
      this.dgvBase.MultiSelect = false;
      this.dgvBase.Name = "dgvBase";
      this.dgvBase.Size = new Size(494, 454);
      this.dgvBase.TabIndex = 3;
      this.dgvBase.VirtualMode = true;
      this.dgvBase.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvBase_CellBeginEdit);
      this.dgvBase.CellValueNeeded += new DataGridViewCellValueEventHandler(this.dgvBase_CellValueNeeded);
      this.dgvBase.CellValuePushed += new DataGridViewCellValueEventHandler(this.dgvBase_CellValuePushed);
      this.dgvBase.DataError += new DataGridViewDataErrorEventHandler(this.dgvBase_DataError);
      this.dgvBase.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvBase_ColumnWidthChanged);
      this.dgvBase.SelectionChanged += new EventHandler(this.dgvBase_SelectionChanged);
      this.Menu.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsmCopy,
        (ToolStripItem) this.tsmCopyInPast
      });
      this.Menu.Name = "MenuForTariffs";
      this.Menu.Size = new Size(266, 48);
      this.tsmCopy.Name = "tsmCopy";
      this.tsmCopy.Size = new Size(265, 22);
      this.tsmCopy.Text = "Копировать запись";
      this.tsmCopy.Click += new EventHandler(this.tsmCopy_Click);
      this.tsmCopyInPast.Name = "tsmCopyInPast";
      this.tsmCopyInPast.Size = new Size(265, 22);
      this.tsmCopyInPast.Text = "Копировать в прошлое время";
      this.tsmCopyInPast.Click += new EventHandler(this.tsmCopyInPast_Click);
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
      this.toolStrip1.Padding = new Padding(0, 0, 2, 0);
      this.toolStrip1.Size = new Size(494, 24);
      this.toolStrip1.TabIndex = 2;
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
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.dgvBase);
      this.Controls.Add((Control) this.toolStrip1);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.Margin = new Padding(4);
      this.Name = "UCBase";
      this.Size = new Size(494, 478);
      this.Load += new EventHandler(this.FrmBase_Load);
      ((ISupportInitialize) this.dgvBase).EndInit();
      this.Menu.ResumeLayout(false);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
