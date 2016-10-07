// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.Controls.UCTypeSeal
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Kvartplata.Forms.Controls
{
  public class UCTypeSeal : UCBase
  {
    private bool isEdit = false;
    private IContainer components = (IContainer) null;
    public DateTime? closedPeriod;

    public TypeSeal CurTypeSeal
    {
      get
      {
        return (TypeSeal) this.curObject;
      }
    }

    public IList ObjectsList
    {
      get
      {
        return this.objectsList;
      }
    }

    public UCTypeSeal()
    {
      this.InitializeComponent();
      this.dgvBase.ContextMenuStrip = (ContextMenuStrip) null;
      DataGridViewColumn dataGridViewColumn1 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn1.Name = "numColumn";
      dataGridViewColumn1.HeaderText = "Номер";
      dataGridViewColumn1.Width = 100;
      this.dgvBase.Columns.Add(dataGridViewColumn1);
      this.dgvBase.Columns["numColumn"].Visible = false;
      DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn2.Name = "nameColumn";
      dataGridViewColumn2.HeaderText = "Тип пломбы";
      dataGridViewColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgvBase.Columns.Add(dataGridViewColumn2);
      if (Options.ViewEdit)
      {
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 2, "Пользователь", "UName", 100, true);
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 3, "Дата редактирования", "DEdit", 100, true);
      }
      this.MySettings.GridName = "TypeSeal";
    }

    protected override void GetList()
    {
      if (this.session.IsOpen)
        this.session.Clear();
      this.objectsList.Clear();
      this.objectsList = this.session.CreateQuery(string.Format("from TypeSeal order by TypeSealId")).List();
      if (this.objectsList.Count == 0)
        this.curObject = (object) null;
      this.dgvBase.RowCount = this.objectsList.Count;
      this.dgvBase.Refresh();
      this.SelectRow();
    }

    public void LoadData()
    {
      this.curObject = (object) null;
      this.GetList();
    }

    public void SaveData()
    {
      this.tsbApplay_Click((object) this.tsbApplay, EventArgs.Empty);
    }

    protected override void dgvBase_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.objectsList.Count <= 0)
        return;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "nameColumn")
        e.Value = (object) ((TypeSeal) this.objectsList[e.RowIndex]).TypeSealName;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "numColumn")
        e.Value = (object) ((TypeSeal) this.objectsList[e.RowIndex]).TypeSealId;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "UName")
        e.Value = (object) ((TypeSeal) this.objectsList[e.RowIndex]).Uname;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "DEdit")
        e.Value = (object) ((TypeSeal) this.objectsList[e.RowIndex]).Dedit.ToShortDateString();
    }

    protected override void dgvBase_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      int int32_1 = Convert.ToInt32(this.session.CreateQuery(string.Format("select count(*) from TypeSeal where  TypeSealId={0}", (object) ((TypeSeal) this.curObject).TypeSealId)).UniqueResult());
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "nameColumn")
        ((TypeSeal) this.objectsList[e.RowIndex]).TypeSealName = e.Value.ToString();
      else if (int32_1 == 0)
      {
        if (this.dgvBase.Columns[e.ColumnIndex].Name == "numColumn")
        {
          try
          {
            IList list = this.session.CreateCriteria(typeof (TypeSeal)).Add((ICriterion) Restrictions.Eq("TypeSealId", (object) Convert.ToInt32(e.Value))).List();
            int int32_2 = Convert.ToInt32(e.Value);
            if (list.Count > 0)
            {
              if (MessageBox.Show("Тип пломбы с таким номером уже заведен! Вы уверены что хотите оставить этот номер?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                ((TypeSeal) this.objectsList[e.RowIndex]).TypeSealId = int32_2;
            }
            else
              ((TypeSeal) this.objectsList[e.RowIndex]).TypeSealId = int32_2;
            this.dgvBase.Refresh();
          }
          catch
          {
            int num = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
            this.isEdit = false;
            return;
          }
        }
      }
      else
      {
        int num = (int) MessageBox.Show("Изменение невозможно! Существуют данные, которые привязаны к этому типу пломбы.", "Внимание!", MessageBoxButtons.OK);
        this.isEdit = false;
        return;
      }
      if (this.newObject == null && this.curObject != null)
      {
        ((TypeSeal) this.curObject).Uname = Options.Login;
        ((TypeSeal) this.curObject).Dedit = DateTime.Now.Date;
      }
      this.isEdit = true;
    }

    protected override void tsbAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      object obj = this.session.CreateQuery(string.Format("select max(t.TypeSealId) from TypeSeal t ")).UniqueResult();
      this.newObject = (object) new TypeSeal(obj != null ? (int) obj + 1 : 1, "Новый тип пломбы");
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
        if (this.newObject != null)
        {
          ((TypeSeal) this.newObject).Uname = Options.Login;
          ((TypeSeal) this.newObject).Dedit = DateTime.Now;
        }
        base.tsbApplay_Click(sender, e);
      }
    }

    protected override void tsbDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true) || MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      bool flag = true;
      IList list = this.session.CreateQuery("from Seal where TypeSeal.TypeSealId=:id").SetInt32("id", ((TypeSeal) this.curObject).TypeSealId).List();
      if (list.Count == 0)
        flag = true;
      foreach (Seal seal in (IEnumerable) list)
        flag = false;
      if (flag)
      {
        this.session.Clear();
        base.tsbDelete_Click(sender, e);
      }
      else
      {
        int num = (int) MessageBox.Show("Удаление невозможно! Существуют данные, ссылающиеся на эту запись.", "Внимание!", MessageBoxButtons.OK);
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
      this.components = (IContainer) new Container();
    }
  }
}
