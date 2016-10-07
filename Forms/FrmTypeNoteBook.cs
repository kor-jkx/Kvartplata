// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmTypeNoteBook
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
  public class FrmTypeNoteBook : FrmBase
  {
    private Dictionary<short, TypeNoteBook> _updId = new Dictionary<short, TypeNoteBook>();
    private Dictionary<short, TypeNoteBook> _updAdmId = new Dictionary<short, TypeNoteBook>();
    private bool _readOnly = false;
    private IContainer components = (IContainer) null;
    private Company _company;
    public HelpProvider hp;

    public FrmTypeNoteBook(Company company)
    {
      this.InitializeComponent();
      this._company = company;
      this.CheckAccess();
      this.session = Domain.CurrentSession;
      this.dgvBase.Columns[2].Visible = false;
      this.dgvBase.Columns[3].Visible = false;
    }

    private void CheckAccess()
    {
      this._readOnly = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(32, this._company, false));
      this.toolStrip1.Visible = this._readOnly;
      this.dgvBase.ReadOnly = !this._readOnly;
    }

    protected override void GetList()
    {
      this.objectsList.Clear();
      this.objectsList = this.session.CreateQuery("from TypeNoteBook order by TypeNoteBookId").List();
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
        e.Value = (object) ((TypeNoteBook) this.objectsList[e.RowIndex]).TypeNoteBookName;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "ID")
        e.Value = (object) ((TypeNoteBook) this.objectsList[e.RowIndex]).TypeNoteBookId;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "UName")
        e.Value = (object) ((RightDoc) this.objectsList[e.RowIndex]).Uname;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "DEdit")
        e.Value = (object) ((RightDoc) this.objectsList[e.RowIndex]).Dedit.ToShortDateString();
    }

    protected override void dgvBase_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      ((TypeNoteBook) this.objectsList[e.RowIndex]).UName = Options.Login;
      ((TypeNoteBook) this.objectsList[e.RowIndex]).DEdit = DateTime.Now;
      int int32 = Convert.ToInt32(this.session.CreateQuery(string.Format("select count(*) FROM NoteBook where TypeNoteBook.TypeNoteBookId={0}", (object) ((TypeNoteBook) this.curObject).TypeNoteBookId)).UniqueResult());
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "nameColumn")
        ((TypeNoteBook) this.objectsList[e.RowIndex]).TypeNoteBookName = e.Value.ToString();
      else if (int32 == 0)
      {
        if (this.dgvBase.Columns[e.ColumnIndex].Name == "ID")
        {
          short int16;
          try
          {
            int16 = Convert.ToInt16(e.Value);
          }
          catch
          {
            int num = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
            this.isEdit = false;
            return;
          }
          if (this.FindByID(int16))
          {
            this.isEdit = false;
            int num = (int) MessageBox.Show("Тип с таким номером уже заведен! Выберите другой номер!", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          ((TypeNoteBook) this.objectsList[e.RowIndex]).TypeNoteBookId = (int) int16;
          this.dgvBase.Refresh();
        }
      }
      else
      {
        int num = (int) MessageBox.Show("Изменение невозможно! Существуют данные, связанные с этим типом записи.", "Внимание!", MessageBoxButtons.OK);
        this.isEdit = false;
        return;
      }
      this.isEdit = true;
    }

    protected override void tsbAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, (Company) null, true))
        return;
      this.newObject = (object) new TypeNoteBook();
      object obj = this.session.CreateQuery("select max(TypeNoteBookId) from TypeNoteBook").UniqueResult();
      int num1 = obj != null ? Convert.ToInt32(obj) + 1 : 1;
      short num2;
      try
      {
        num2 = Convert.ToInt16(num1);
      }
      catch
      {
        num2 = (short) 0;
      }
      this.newObject = (object) new TypeNoteBook();
      ((TypeNoteBook) this.newObject).TypeNoteBookId = (int) num2;
      ((TypeNoteBook) this.newObject).TypeNoteBookName = "Наименование типа";
      this.isEdit = true;
      base.tsbAdd_Click(sender, e);
    }

    protected override void tsbApplay_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, (Company) null, true))
        return;
      if (this.newObject != null)
      {
        if (((TypeNoteBook) this.newObject).TypeNoteBookName == null)
        {
          int num = (int) MessageBox.Show("Введите наименование.", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        ((TypeNoteBook) this.newObject).UName = Options.Login;
        ((TypeNoteBook) this.newObject).DEdit = DateTime.Now;
      }
      base.tsbApplay_Click(sender, e);
    }

    protected override void tsbDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, (Company) null, true))
        return;
      if (this.curObject != null && Convert.ToInt32(this.session.CreateQuery(string.Format("select count(*) FROM NoteBook where TypeNoteBook.TypeNoteBookId={0}", (object) ((TypeNoteBook) this.curObject).TypeNoteBookId)).UniqueResult()) == 0)
      {
        base.tsbDelete_Click(sender, e);
      }
      else
      {
        int num = (int) MessageBox.Show("Удаление невозможно! Существуют данные, ссылающиеся на эту запись.", "Внимание!", MessageBoxButtons.OK);
      }
    }

    private bool FindByID(short id)
    {
      foreach (TypeNoteBook objects in (IEnumerable) this.objectsList)
      {
        if (objects.TypeNoteBookId == (int) id)
          return true;
      }
      return false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.hp = new HelpProvider();
      this.SuspendLayout();
      this.hp.HelpNamespace = "Help.chm";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(581, 506);
      this.hp.SetHelpKeyword((Control) this, "kv519.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      this.Name = "FrmTypeNoteBook";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Типы записей в блокноте";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
