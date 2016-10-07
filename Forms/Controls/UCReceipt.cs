// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.Controls.UCReceipt
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
  public class UCReceipt : UCBase
  {
    private Dictionary<short, Receipt> _updId = new Dictionary<short, Receipt>();
    private bool isEdit = false;
    private IContainer components = (IContainer) null;

    public Param CurReceipt
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

    public UCReceipt()
    {
      this.InitializeComponent();
      this.dgvBase.ContextMenuStrip = (ContextMenuStrip) null;
      this.tsbExit.Visible = false;
      DataGridViewColumn dataGridViewColumn1 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn1.Name = "ID";
      dataGridViewColumn1.HeaderText = "№";
      dataGridViewColumn1.Width = 60;
      this.dgvBase.Columns.Add(dataGridViewColumn1);
      DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn2.Name = "nameColumn";
      dataGridViewColumn2.HeaderText = "Наименование";
      dataGridViewColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgvBase.Columns.Add(dataGridViewColumn2);
      if (Options.ViewEdit)
      {
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 2, "Пользователь", "UName", 100, true);
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 3, "Дата редактирования", "DEdit", 100, true);
      }
      this.MySettings.GridName = "Receipt";
    }

    protected override void GetList()
    {
      this.objectsList.Clear();
      this.objectsList = this.session.CreateQuery("from Receipt order by ReceiptId").List();
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
      foreach (Receipt objects in (IEnumerable) this.objectsList)
      {
        if ((int) objects.ReceiptId == (int) id)
          return true;
      }
      return false;
    }

    protected override void dgvBase_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.objectsList.Count <= 0)
        return;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "ID")
        e.Value = (object) ((Receipt) this.objectsList[e.RowIndex]).ReceiptId;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "nameColumn")
        e.Value = (object) ((Receipt) this.objectsList[e.RowIndex]).ReceiptName;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "UName")
        e.Value = (object) ((Receipt) this.objectsList[e.RowIndex]).UName;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "DEdit")
        e.Value = (object) ((Receipt) this.objectsList[e.RowIndex]).DEdit.ToShortDateString();
    }

    protected override void dgvBase_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      int int32 = Convert.ToInt32(this.session.CreateQuery(string.Format("select count(*) from CmpReceipt where Receipt_id={0}", (object) ((Receipt) this.curObject).ReceiptId)).UniqueResult());
      ((Receipt) this.objectsList[e.RowIndex]).UName = Options.Login;
      ((Receipt) this.objectsList[e.RowIndex]).DEdit = DateTime.Now;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "nameColumn")
        ((Receipt) this.objectsList[e.RowIndex]).ReceiptName = e.Value.ToString();
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
            this._updId.Add(((Receipt) this.objectsList[e.RowIndex]).ReceiptId, (Receipt) this.objectsList[e.RowIndex]);
            ((Receipt) this.objectsList[e.RowIndex]).ReceiptId = int16;
            this.dgvBase.Refresh();
          }
        }
      }
      else
      {
        int num = (int) MessageBox.Show("Изменение невозможно! Существуют данные, связанные с этой квитанцией.", "Внимание!", MessageBoxButtons.OK);
        this.isEdit = false;
      }
    }

    protected override void tsbAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      object obj = this.session.CreateQuery("select max(ReceiptId) from Receipt").UniqueResult();
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
      this.newObject = (object) new Receipt();
      ((Receipt) this.newObject).ReceiptId = num2;
      ((Receipt) this.newObject).ReceiptName = "Новая квитанция";
      base.tsbAdd_Click(sender, e);
    }

    protected override void tsbApplay_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      this.dgvBase.EndEdit();
      if (this.newObject != null)
      {
        if (((Receipt) this.newObject).ReceiptName == null)
        {
          int num = (int) MessageBox.Show("Введите наименование.", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        ((Receipt) this.newObject).UName = Options.Login;
        ((Receipt) this.newObject).DEdit = DateTime.Now;
      }
      else if (this.curObject != null)
      {
        try
        {
          foreach (short key in this._updId.Keys)
          {
            string queryString = "update DBA.dcReceipt set Receipt_id=:newid,Receipt_name=:name,uname=:uname,dedit=:d where Receipt_id=:id";
            if (queryString != "")
              this.session.CreateSQLQuery(queryString).SetInt16("newid", this._updId[key].ReceiptId).SetString("name", this._updId[key].ReceiptName).SetString("uname", this._updId[key].UName).SetDateTime("d", this._updId[key].DEdit).SetInt16("id", key).ExecuteUpdate();
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
      if (Convert.ToInt32(this.session.CreateQuery("select count(*) from CmpReceipt where Receipt_id=:rid").SetInt16("rid", ((Receipt) this.curObject).ReceiptId).UniqueResult()) == 0)
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
        this._updId[key].ReceiptId = key;
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
    }
  }
}
