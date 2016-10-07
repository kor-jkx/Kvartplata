// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmDi_PhonesServ
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmDi_PhonesServ : FrmBase
  {
    private bool _readOnly = false;
    private IContainer components = (IContainer) null;
    private Company _company;
    public HelpProvider hp;

    public FrmDi_PhonesServ(Company company)
    {
      this.InitializeComponent();
      this._company = company;
      this.CheckAccess();
      this.session = Domain.CurrentSession;
      this.dgvBase.Columns[2].Visible = false;
      this.dgvBase.Columns[3].Visible = false;
      this.AddColumns();
    }

    private void CheckAccess()
    {
      this._readOnly = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(32, this._company, false));
      this.toolStrip1.Visible = this._readOnly;
      this.dgvBase.ReadOnly = !this._readOnly;
    }

    private void AddColumns()
    {
      KvrplHelper.AddComboBoxColumn(this.dgvBase, 4, (IList) new List<ViewService>()
      {
        new ViewService((short) -1, "Не показывать"),
        new ViewService((short) 0, "Показывать"),
        new ViewService((short) 0, "Показывать на л/с")
      }, "Id", "Name", "Параметр отображения", "ViewService", 7, 200);
    }

    protected override void GetList()
    {
      this.objectsList.Clear();
      this.objectsList = this.session.CreateQuery("from Di_PhonesServ order by Nameservice").List();
      foreach (object objects in (IEnumerable) this.objectsList)
        this.session.Refresh(objects);
      this.dgvBase.RowCount = this.objectsList.Count;
      this.dgvBase.Refresh();
      this.SelectRow();
    }

    private bool FindByID(int id)
    {
      foreach (Di_PhonesServ objects in (IEnumerable) this.objectsList)
      {
        if (objects.Idservice == id)
          return true;
      }
      return false;
    }

    protected override void dgvBase_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.objectsList.Count <= 0)
        return;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "nameColumn")
        e.Value = (object) ((Di_PhonesServ) this.objectsList[e.RowIndex]).Nameservice;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "ViewService")
        e.Value = (object) ((Di_PhonesServ) this.objectsList[e.RowIndex]).ViewService;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "ID")
        e.Value = (object) ((Di_PhonesServ) this.objectsList[e.RowIndex]).Idservice;
    }

    protected override void dgvBase_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "nameColumn")
        ((Di_PhonesServ) this.objectsList[e.RowIndex]).Nameservice = e.Value.ToString();
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "ViewService")
        ((Di_PhonesServ) this.objectsList[e.RowIndex]).ViewService = Convert.ToInt16(e.Value.ToString());
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "ID")
        ((Di_PhonesServ) this.objectsList[e.RowIndex]).Idservice = Convert.ToInt32(e.Value.ToString());
      ((Di_PhonesServ) this.objectsList[e.RowIndex]).Uname = Options.Login;
      ((Di_PhonesServ) this.objectsList[e.RowIndex]).Dedit = DateTime.Now;
      this.isEdit = true;
    }

    protected override void tsbAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, (Company) null, true))
        return;
      this.newObject = (object) new Di_PhonesServ();
      ((Di_PhonesServ) this.newObject).Idservice = this.session.CreateSQLQuery("Select DBA.Gen_id('DI_PHONESSERV',1)").UniqueResult<int>();
      ((Di_PhonesServ) this.newObject).Nameservice = "Название службы";
      this.isEdit = true;
      base.tsbAdd_Click(sender, e);
    }

    protected override void tsbApplay_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, (Company) null, true))
        return;
      if (this.newObject != null)
      {
        if (((Di_PhonesServ) this.newObject).Nameservice == null)
        {
          int num = (int) MessageBox.Show("Введите название службы.", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        ((Di_PhonesServ) this.newObject).Uname = Options.Login;
        ((Di_PhonesServ) this.newObject).Dedit = DateTime.Now;
      }
      base.tsbApplay_Click(sender, e);
    }

    protected override void tsbDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, (Company) null, true))
        return;
      base.tsbDelete_Click(sender, e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmDi_PhonesServ));
      this.hp = new HelpProvider();
      this.SuspendLayout();
      this.hp.HelpNamespace = "Help.chm";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.ClientSize = new Size(507, 506);
      this.hp.SetHelpKeyword((Control) this, "kv513.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Location = new Point(0, 0);
      this.Name = "FrmDi_PhonesServ";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Службы";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
