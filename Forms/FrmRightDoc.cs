// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmRightDoc
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmRightDoc : FrmBase
  {
    private Dictionary<short, RightDoc> _updId = new Dictionary<short, RightDoc>();
    private Dictionary<short, RightDoc> _updAdmId = new Dictionary<short, RightDoc>();
    private bool _readOnly = false;
    private IContainer components = (IContainer) null;
    private Company _company;
    public HelpProvider hp;

    public FrmRightDoc(Company company)
    {
      this.InitializeComponent();
      this._company = company;
      this.CheckAccess();
      this.session = Domain.CurrentSession;
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
      this.objectsList = this.session.CreateQuery("from RightDoc order by RightDocId").List();
      foreach (object objects in (IEnumerable) this.objectsList)
        this.session.Refresh(objects);
      this.dgvBase.RowCount = this.objectsList.Count;
      this.dgvBase.Refresh();
      this.SelectRow();
    }

    private bool FindByID(short id)
    {
      foreach (RightDoc objects in (IEnumerable) this.objectsList)
      {
        if ((int) objects.RightDocId == (int) id)
          return true;
      }
      return false;
    }

    protected override void dgvBase_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.objectsList.Count <= 0)
        return;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "ID")
        e.Value = (object) ((RightDoc) this.objectsList[e.RowIndex]).RightDocId;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "nameColumn")
        e.Value = (object) ((RightDoc) this.objectsList[e.RowIndex]).RightDocName;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "UName")
        e.Value = (object) ((RightDoc) this.objectsList[e.RowIndex]).Uname;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "DEdit")
        e.Value = (object) ((RightDoc) this.objectsList[e.RowIndex]).Dedit.ToShortDateString();
    }

    protected override void dgvBase_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      int int32 = Convert.ToInt32(this.session.CreateQuery(string.Format("select count(*) FROM ClientParam where Param_Id=104 and Param_Value={0}", (object) ((RightDoc) this.curObject).RightDocId)).UniqueResult());
      ((RightDoc) this.objectsList[e.RowIndex]).Uname = Options.Login;
      ((RightDoc) this.objectsList[e.RowIndex]).Dedit = DateTime.Now;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "nameColumn")
        ((RightDoc) this.objectsList[e.RowIndex]).RightDocName = e.Value.ToString();
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
            int num = (int) MessageBox.Show("Вид документа с таким номером уже заведен! Выберите другой номер!", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          if (!this._updAdmId.ContainsKey(((RightDoc) this.objectsList[e.RowIndex]).RightDocId))
            this._updAdmId.Remove(((RightDoc) this.objectsList[e.RowIndex]).RightDocId);
          this._updId.Add(((RightDoc) this.objectsList[e.RowIndex]).RightDocId, (RightDoc) this.objectsList[e.RowIndex]);
          ((RightDoc) this.objectsList[e.RowIndex]).RightDocId = int16;
          this.dgvBase.Refresh();
        }
      }
      else
      {
        int num = (int) MessageBox.Show("Изменение невозможно! Существуют данные, связанные с этим видом документов.", "Внимание!", MessageBoxButtons.OK);
        this.isEdit = false;
        return;
      }
      if (!this._updAdmId.ContainsKey(((RightDoc) this.objectsList[e.RowIndex]).RightDocId))
        this._updAdmId.Add(((RightDoc) this.objectsList[e.RowIndex]).RightDocId, (RightDoc) this.objectsList[e.RowIndex]);
      this.isEdit = true;
    }

    protected override void tsbAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      object obj = this.session.CreateQuery("select max(RightDocId) from RightDoc").UniqueResult();
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
      this.newObject = (object) new RightDoc();
      ((RightDoc) this.newObject).RightDocId = num2;
      ((RightDoc) this.newObject).RightDocName = "Документ";
      this.isEdit = true;
      base.tsbAdd_Click(sender, e);
    }

    protected override void tsbApplay_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      this.dgvBase.EndEdit();
      if (!this.isEdit)
      {
        this.isEdit = true;
      }
      else
      {
        string str = "";
        if (this.newObject != null)
        {
          if (((RightDoc) this.newObject).RightDocName == null)
          {
            int num = (int) MessageBox.Show("Введите наименование.", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          ((RightDoc) this.newObject).Uname = Options.Login;
          ((RightDoc) this.newObject).Dedit = DateTime.Now;
          str = "insert into DBA.dcRightDoc(RightDoc_id,RightDoc_name,Uname,Dedit) values(" + (object) ((RightDoc) this.newObject).RightDocId + ",'" + ((RightDoc) this.newObject).RightDocName + "','" + ((RightDoc) this.newObject).Uname + "','" + (object) ((RightDoc) this.newObject).Dedit + "')";
        }
        else
        {
          try
          {
            foreach (short key in this._updId.Keys)
            {
              string queryString = "update DBA.dcRightDoc set RightDoc_id=:newid,RightDoc_name=:name,uname=:uname,dedit=:d where RightDoc_id=:id";
              if (queryString != "")
                this.session.CreateSQLQuery(queryString).SetInt16("newid", this._updId[key].RightDocId).SetString("name", this._updId[key].RightDocName).SetString("uname", this._updId[key].Uname).SetDateTime("d", this._updId[key].Dedit).SetInt16("id", key).ExecuteUpdate();
              this.session.Evict((object) this._updId[key]);
              string ede = "update DBA.dcRightDoc set RightDoc_id=" + (object) this._updId[key].RightDocId + ",RightDoc_name='" + this._updId[key].RightDocName + "',uname='" + this._updId[key].Uname + "',dedit='" + (object) this._updId[key].Dedit + "' where RightDoc_id=" + (object) key;
            }
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Изменения внесены не полностью!", "Внимание!", MessageBoxButtons.OK);
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
        }
        base.tsbApplay_Click(sender, e);
        this._updId.Clear();
        this._updAdmId.Clear();
      }
    }

    protected override void tsbCancel_Click(object sender, EventArgs e)
    {
      this.dgvBase.EndEdit();
      foreach (short key in this._updId.Keys)
        this._updId[key].RightDocId = key;
      this._updId.Clear();
      this._updAdmId.Clear();
      base.tsbCancel_Click(sender, e);
    }

    protected override void tsbDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      if (this.curObject != null && Convert.ToInt32(this.session.CreateQuery(string.Format("select count(*) FROM ClientParam where Param_Id=104 and Param_Value={0}", (object) ((RightDoc) this.curObject).RightDocId)).UniqueResult()) == 0)
      {
        using (ITransaction transaction = this.session.BeginTransaction())
        {
          try
          {
            this.Adm_DelQuery = "delete from DBA.dcRightDoc where RightDoc_id=" + (object) ((RightDoc) this.curObject).RightDocId;
            base.tsbDelete_Click(sender, e);
            transaction.Commit();
          }
          catch (Exception ex)
          {
            KvrplHelper.WriteLog(ex, (LsClient) null);
            transaction.Rollback();
          }
        }
      }
      else
      {
        int num = (int) MessageBox.Show("Удаление невозможно! Существуют данные, ссылающиеся на эту запись.", "Внимание!", MessageBoxButtons.OK);
      }
    }

    protected override void FrmBase_Load(object sender, EventArgs e)
    {
      this._updId.Clear();
      this._updAdmId.Clear();
      this._updAdmId.Clear();
      base.FrmBase_Load(sender, e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmRightDoc));
      this.hp = new HelpProvider();
      this.SuspendLayout();
      this.hp.HelpNamespace = "Help.chm";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(555, 502);
      this.hp.SetHelpKeyword((Control) this, "kv58.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Location = new Point(0, 0);
      this.Margin = new Padding(5);
      this.Name = "FrmRightDoc";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Виды документов на право пользования";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
