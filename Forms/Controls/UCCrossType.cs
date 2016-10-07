// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.Controls.UCCrossType
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Kvartplata.Forms.Controls
{
  public class UCCrossType : UCBase
  {
    private bool isEdit = false;
    private Dictionary<short, CrossType> _updId = new Dictionary<short, CrossType>();
    private IContainer components = (IContainer) null;

    public Param CurCrossType
    {
      get
      {
        return (Param) this.curObject;
      }
    }

    public IList ObjectsList
    {
      get
      {
        return this.objectsList;
      }
    }

    public UCCrossType()
    {
      this.InitializeComponent();
      this.dgvBase.ContextMenuStrip = (ContextMenuStrip) null;
      this.tsbExit.Visible = false;
      DataGridViewColumn dataGridViewColumn1 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn2.Name = "nameColumn";
      dataGridViewColumn2.HeaderText = "Наименование";
      dataGridViewColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgvBase.Columns.Add(dataGridViewColumn2);
      if (Options.ViewEdit)
      {
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 1, "Пользователь", "UName", 100, true);
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 2, "Дата редактирования", "DEdit", 100, true);
      }
      this.MySettings.GridName = "CrossType";
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
      base.GetList();
    }

    public void LoadData()
    {
      this._updId.Clear();
      this.GetList();
    }

    public void SaveData()
    {
      this.tsbApplay_Click((object) this.tsbApplay, EventArgs.Empty);
    }

    private bool FindByID(short id)
    {
      foreach (CrossType objects in (IEnumerable) this.objectsList)
      {
        if ((int) objects.CrossTypeId == (int) id)
          return true;
      }
      return false;
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
      int int32 = Convert.ToInt32(this.session.CreateQuery(string.Format("select count(*) from CrossService where CrossType.CrossTypeId={0}", (object) ((CrossType) this.curObject).CrossTypeId)).UniqueResult());
      ((CrossType) this.objectsList[e.RowIndex]).UName = Options.Login;
      ((CrossType) this.objectsList[e.RowIndex]).DEdit = DateTime.Now;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "nameColumn")
        ((CrossType) this.objectsList[e.RowIndex]).CrossTypeName = e.Value.ToString();
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
            int num = (int) MessageBox.Show("Квитанция с таким номером уже заведена! Выберите другой номер!", "Внимание!", MessageBoxButtons.OK);
          }
          else
          {
            this._updId.Add(((CrossType) this.objectsList[e.RowIndex]).CrossTypeId, (CrossType) this.objectsList[e.RowIndex]);
            ((CrossType) this.objectsList[e.RowIndex]).CrossTypeId = int16;
            this.dgvBase.Refresh();
          }
        }
      }
      else
      {
        int num = (int) MessageBox.Show("Изменение невозможно! Существуют данные, связанные с этим типом привязки.", "Внимание!", MessageBoxButtons.OK);
        this.isEdit = false;
      }
    }

    protected override void tsbAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      object obj = this.session.CreateQuery("select max(CrossTypeId) from CrossType").UniqueResult();
      int num = obj != null ? Convert.ToInt32(obj) + 1 : 1;
      short crossTypeId;
      try
      {
        crossTypeId = Convert.ToInt16(num);
      }
      catch
      {
        crossTypeId = (short) 0;
      }
      this.newObject = (object) new CrossType(crossTypeId, "Новый тип");
      base.tsbAdd_Click(sender, e);
    }

    protected override void tsbApplay_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      this.dgvBase.EndEdit();
      if (this.newObject != null)
      {
        if (((CrossType) this.newObject).CrossTypeName == null)
        {
          int num = (int) MessageBox.Show("Введите наименование.", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        ((CrossType) this.newObject).UName = Options.Login;
        ((CrossType) this.newObject).DEdit = DateTime.Now;
      }
      else if (this.curObject != null)
      {
        try
        {
          foreach (short key in this._updId.Keys)
          {
            string queryString = "update CrossType set CrossTypeId=:newid,CrossTypeName=:name,UName=:uname,DEdit=:d where CrossTypeId=:id";
            if (queryString != "")
              this.session.CreateQuery(queryString).SetInt16("newid", this._updId[key].CrossTypeId).SetString("name", this._updId[key].CrossTypeName).SetString("uname", this._updId[key].UName).SetDateTime("d", this._updId[key].DEdit).SetInt16("id", key).ExecuteUpdate();
            this.session.Evict((object) this._updId[key]);
          }
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Изменения внесены не полностью!", "Внимание!", MessageBoxButtons.OK);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        this._updId.Clear();
      }
      base.tsbApplay_Click(sender, e);
    }

    protected override void tsbDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true) || MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      this.session.Clear();
      if (Convert.ToInt32(this.session.CreateQuery("select count(*) from CrossService where CrossType.CrossTypeId=:rid").SetInt16("rid", ((CrossType) this.curObject).CrossTypeId).UniqueResult()) == 0)
      {
        base.tsbDelete_Click(sender, e);
      }
      else
      {
        int num = (int) MessageBox.Show("Невозможно удалить запись, так как есть связанные с ней данные!", "Внимание!", MessageBoxButtons.OK);
      }
    }

    protected override void tsbCancel_Click(object sender, EventArgs e)
    {
      this.dgvBase.EndEdit();
      foreach (short key in this._updId.Keys)
        this._updId[key].CrossTypeId = key;
      this._updId.Clear();
      base.tsbCancel_Click(sender, e);
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
      this.AutoScaleMode = AutoScaleMode.Font;
    }
  }
}
