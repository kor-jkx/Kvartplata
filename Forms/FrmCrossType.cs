// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmCrossType
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmCrossType : FrmBase
  {
    private IContainer components = (IContainer) null;
    private Button btnExit;
    public HelpProvider hp;

    public FrmCrossType()
    {
      this.InitializeComponent();
      this.session = Domain.CurrentSession;
      this.dgvBase.Columns[0].Visible = false;
      if (!Options.ViewEdit)
      {
        this.dgvBase.Columns[2].Visible = false;
        this.dgvBase.Columns[3].Visible = false;
      }
      else
      {
        this.dgvBase.Columns[2].ReadOnly = true;
        this.dgvBase.Columns[3].ReadOnly = true;
      }
    }

    protected override void GetList()
    {
      this.objectsList.Clear();
      this.objectsList = this.session.CreateQuery("from CrossType order by CrossTypeName").List();
      foreach (object objects in (IEnumerable) this.objectsList)
        this.session.Refresh(objects);
      this.dgvBase.RowCount = this.objectsList.Count;
      this.dgvBase.Refresh();
      this.SelectRow();
    }

    protected override void dgvBase_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.objectsList.Count <= 0)
        return;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "nameColumn")
        e.Value = (object) ((CrossType) this.objectsList[e.RowIndex]).CrossTypeName;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "UName")
        e.Value = (object) ((CrossType) this.objectsList[e.RowIndex]).UName;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "DEdit")
        e.Value = (object) ((CrossType) this.objectsList[e.RowIndex]).DEdit.ToShortDateString();
    }

    protected override void dgvBase_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "nameColumn")
        ((CrossType) this.objectsList[e.RowIndex]).CrossTypeName = e.Value.ToString();
      ((CrossType) this.objectsList[e.RowIndex]).UName = Options.Login;
      ((CrossType) this.objectsList[e.RowIndex]).DEdit = DateTime.Now;
      this.isEdit = true;
    }

    protected override void tsbAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      this.newObject = (object) new CrossType();
      object obj = this.session.CreateQuery(string.Format("select max(t.CrossTypeId) from CrossType t ")).UniqueResult();
      this.newObject = (object) new CrossType(obj != null ? (short) ((int) (short) obj + 1) : (short) 1, "Новый тип");
      this.isEdit = true;
      base.tsbAdd_Click(sender, e);
    }

    protected override void tsbApplay_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      if (this.newObject != null)
      {
        if (((CrossType) this.newObject).CrossTypeName == null)
        {
          int num = (int) MessageBox.Show("Введите название типа привязки", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        ((CrossType) this.newObject).UName = Options.Login;
        ((CrossType) this.newObject).DEdit = DateTime.Now;
      }
      base.tsbApplay_Click(sender, e);
    }

    protected override void tsbDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      base.tsbDelete_Click(sender, e);
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
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
      this.btnExit = new Button();
      this.hp = new HelpProvider();
      this.SuspendLayout();
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(424, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(79, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.hp.HelpNamespace = "Help.chm";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) null;
      this.ClientSize = new Size(515, 478);
      this.hp.SetHelpKeyword((Control) this, "kv518.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      this.Margin = new Padding(5);
      this.Name = "FrmCrossType";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Типы связывания услуг";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
