// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.Controls.UCParam
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Kvartplata.Forms.Controls
{
  public class UCParam : UCBase
  {
    private bool isEdit = false;
    private Dictionary<short, Param> _updId = new Dictionary<short, Param>();
    private Dictionary<short, Param> _updAdmId = new Dictionary<short, Param>();
    private IContainer components = (IContainer) null;
    public DateTime? closedPeriod;

    public Param CurParam
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

    public UCParam()
    {
      this.InitializeComponent();
      this.dgvBase.ContextMenuStrip = (ContextMenuStrip) null;
      DataGridViewColumn dataGridViewColumn1 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn1.Name = "ID";
      dataGridViewColumn1.HeaderText = "№";
      dataGridViewColumn1.Width = 60;
      this.dgvBase.Columns.Add(dataGridViewColumn1);
      DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn2.Name = "Param_name";
      dataGridViewColumn2.HeaderText = "Наименование";
      dataGridViewColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgvBase.Columns.Add(dataGridViewColumn2);
      DataGridViewColumn dataGridViewColumn3 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn3.Name = "Param_type";
      dataGridViewColumn3.HeaderText = "Тип";
      dataGridViewColumn3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      ((DataGridViewComboBoxColumn) dataGridViewColumn3).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      ((DataGridViewComboBoxColumn) dataGridViewColumn3).DisplayMember = "Name";
      ((DataGridViewComboBoxColumn) dataGridViewColumn3).ValueMember = "Id";
      this.dgvBase.Columns.Add(dataGridViewColumn3);
      DataGridViewColumn dataGridViewColumn4 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn4.Name = "Areal";
      dataGridViewColumn4.HeaderText = "Область применения";
      dataGridViewColumn4.Width = 100;
      ((DataGridViewComboBoxColumn) dataGridViewColumn4).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      ((DataGridViewComboBoxColumn) dataGridViewColumn4).DisplayMember = "Name";
      ((DataGridViewComboBoxColumn) dataGridViewColumn4).ValueMember = "Id";
      this.dgvBase.Columns.Add(dataGridViewColumn4);
      DataGridViewColumn dataGridViewColumn5 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn5.Name = "Sorter";
      dataGridViewColumn5.HeaderText = "Порядок";
      dataGridViewColumn5.Width = 70;
      dataGridViewColumn5.ReadOnly = true;
      this.dgvBase.Columns.Add(dataGridViewColumn5);
      if (Options.ViewEdit)
      {
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 5, "Пользователь", "UName", 100, true);
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 6, "Дата редактирования", "DEdit", 100, true);
      }
      this.MySettings.GridName = "Param";
    }

    protected override void GetList()
    {
      if (this.session != null && this.session.IsOpen)
        this.session.Clear();
      this.objectsList.Clear();
      this.objectsList = this.session.CreateQuery("from Param order by ParamId,Param_name").List();
      if (this.objectsList.Count == 0)
        this.curObject = (object) null;
      this.dgvBase.RowCount = this.objectsList.Count;
      this.dgvBase.Refresh();
      this.SelectRow();
    }

    public void LoadData()
    {
      this._updId.Clear();
      this._updAdmId.Clear();
      this.GetList();
      base.GetList();
    }

    public void SaveData()
    {
      this.tsbApplay_Click((object) this.tsbApplay, EventArgs.Empty);
    }

    private bool FindByID(short id)
    {
      foreach (Param objects in (IEnumerable) this.objectsList)
      {
        if ((int) objects.ParamId == (int) id)
          return true;
      }
      return false;
    }

    protected override void dgvBase_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.objectsList.Count <= 0)
        return;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "ID")
        e.Value = (object) ((Param) this.objectsList[e.RowIndex]).ParamId;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Param_name")
        e.Value = (object) ((Param) this.objectsList[e.RowIndex]).ParamName;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Param_type")
        e.Value = (object) ((Param) this.objectsList[e.RowIndex]).Param_type;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Sorter")
        e.Value = (object) ((Param) this.objectsList[e.RowIndex]).Sorter;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Areal")
        e.Value = (object) ((Param) this.objectsList[e.RowIndex]).Areal;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "UName")
        e.Value = (object) ((Param) this.objectsList[e.RowIndex]).Uname;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "DEdit")
        e.Value = (object) ((Param) this.objectsList[e.RowIndex]).Dedit.ToShortDateString();
    }

    protected override void dgvBase_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      try
      {
        int int32 = Convert.ToInt32(this.session.CreateQuery(string.Format("select count(*) from CmpParam where Param_id={0}", (object) ((Param) this.curObject).ParamId)).UniqueResult());
        if (int32 == 0)
          int32 = Convert.ToInt32(this.session.CreateQuery(string.Format("select count(*) FROM ClientParam where Param_Id={0}", (object) ((Param) this.curObject).ParamId)).UniqueResult());
        if (this.dgvBase.Columns[e.ColumnIndex].Name == "Param_name")
          ((Param) this.objectsList[e.RowIndex]).ParamName = e.Value.ToString();
        else if (int32 == 0)
        {
          if (this.dgvBase.Columns[e.ColumnIndex].Name == "Param_type" && e.Value != null)
            ((Param) this.objectsList[e.RowIndex]).Param_type = Convert.ToInt16(e.Value);
          else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Sorter")
            ((Param) this.objectsList[e.RowIndex]).Sorter = Convert.ToInt16(e.Value);
          else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Areal" && e.Value != null)
            ((Param) this.objectsList[e.RowIndex]).Areal = Convert.ToInt16(e.Value);
          else if (this.dgvBase.Columns[e.ColumnIndex].Name == "ID")
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
              int num = (int) MessageBox.Show("Параметр с таким номером уже заведен! Выберите другой номер!", "Внимание!", MessageBoxButtons.OK);
              return;
            }
            if (this._updAdmId.ContainsKey(((Param) this.objectsList[e.RowIndex]).ParamId))
              this._updAdmId.Remove(((Param) this.objectsList[e.RowIndex]).ParamId);
            this._updId.Add(((Param) this.objectsList[e.RowIndex]).ParamId, (Param) this.objectsList[e.RowIndex]);
            ((Param) this.objectsList[e.RowIndex]).ParamId = int16;
            this.dgvBase.Refresh();
          }
        }
        else
        {
          int num = (int) MessageBox.Show("Изменение невозможно! Существуют данные, связанные с этим видом документов.", "Внимание!", MessageBoxButtons.OK);
          this.isEdit = false;
          return;
        }
      }
      catch
      {
        int num = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
        this.isEdit = false;
        return;
      }
      if (this.curObject != null)
      {
        ((Param) this.curObject).Uname = Options.Login;
        ((Param) this.curObject).Dedit = DateTime.Now;
      }
      if (!this._updAdmId.ContainsKey(((Param) this.objectsList[e.RowIndex]).ParamId))
        this._updAdmId.Add(((Param) this.objectsList[e.RowIndex]).ParamId, (Param) this.objectsList[e.RowIndex]);
      this.isEdit = true;
    }

    protected override void tsbAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      object obj = this.session.CreateQuery("select max(ParamId) from Param").UniqueResult();
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
      this.newObject = (object) new Param();
      ((Param) this.newObject).ParamId = num2;
      ((Param) this.newObject).ParamName = "Наименование";
      ((Param) this.newObject).Param_type = (short) 0;
      ((Param) this.newObject).Sorter = Convert.ToInt16(this.objectsList.Count + 1);
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
          if (((Param) this.newObject).ParamName == null)
          {
            int num = (int) MessageBox.Show("Введите наименование!", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          int areal = (int) ((Param) this.newObject).Areal;
          if (false)
          {
            int num = (int) MessageBox.Show("Укажите область применения!", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          ((Param) this.newObject).Uname = Options.Login;
          ((Param) this.newObject).Dedit = DateTime.Now;
          str = "insert into DBA.dcParam(Param_id,param_name,Param_type,Sorter,Areal,Uname,Dedit) values(" + (object) ((Param) this.newObject).ParamId + ",'" + ((Param) this.newObject).ParamName + "'," + (object) ((Param) this.newObject).Param_type + "," + (object) ((Param) this.newObject).Sorter + "," + (object) ((Param) this.newObject).Areal + ",'" + ((Param) this.newObject).Uname + "','" + (object) ((Param) this.newObject).Dedit + "')";
        }
        else
        {
          try
          {
            foreach (short key in this._updId.Keys)
            {
              this.UpdateQuery = "update DBA.dcParam set param_id=:newid,param_name=:name,param_type=:type,sorter=:s,areal=:a,uname=:uname,dedit=:d where param_id=:id";
              if (this.UpdateQuery != "")
                this.session.CreateSQLQuery(this.UpdateQuery).SetInt32("newid", (int) this._updId[key].ParamId).SetString("name", this._updId[key].ParamName).SetInt16("type", this._updId[key].Param_type).SetInt16("a", this._updId[key].Areal).SetInt16("s", this._updId[key].Sorter).SetString("uname", this._updId[key].Uname).SetDateTime("d", this._updId[key].Dedit).SetInt32("id", (int) key).ExecuteUpdate();
              this.session.Evict((object) this._updId[key]);
              string erf = "update DBA.dcParam set Param_id=" + (object) this._updId[key].ParamId + ",Param_name='" + this._updId[key].ParamName + "',Param_type=" + (object) this._updId[key].Param_type + ",sorter=" + (object) this._updId[key].Sorter + ",areal=" + (object) this._updId[key].Areal + ",uname='" + this._updId[key].Uname + "',dedit='" + (object) this._updId[key].Dedit + "' where param_id=" + (object) key;
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

    protected override void tsbDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true) || MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      bool flag1 = true;
      bool flag2 = true;
      IList list = this.session.CreateQuery(string.Format("from CmpParam where Param_id={0} order by dbeg", (object) ((Param) this.curObject).ParamId)).List();
      if (list.Count == 0)
        flag1 = true;
      foreach (CmpParam cmpParam in (IEnumerable) list)
      {
        if (cmpParam.Dbeg < this.closedPeriod.Value.AddMonths(1) && !cmpParam.Period.PeriodName.HasValue || cmpParam.Period.PeriodName.HasValue && cmpParam.Period.PeriodName.Value <= this.closedPeriod.Value)
          flag1 = false;
      }
      if (flag1)
        flag2 = this.session.CreateQuery(string.Format("FROM ClientParam where Param_Id={0} order by dbeg", (object) ((Param) this.curObject).ParamId)).List().Count == 0;
      if (flag1 & flag2)
      {
        this.Adm_DelQuery = "delete from DBA.dcParam where param_id=" + (object) ((Param) this.curObject).ParamId;
        using (ITransaction transaction = this.session.BeginTransaction())
        {
          try
          {
            foreach (CmpParam cmpParam in (IEnumerable) list)
            {
              this.session.Delete((object) cmpParam);
              this.session.Flush();
            }
            ((Param) this.curObject).Sorter = (short) -10;
            this.session.Update((object) (Param) this.curObject);
            this.session.Flush();
            for (int index = this.dgvBase.CurrentRow.Index + 1; index < this.objectsList.Count; ++index)
            {
              --((Param) this.objectsList[index]).Sorter;
              this.session.Update((object) (Param) this.objectsList[index]);
              this.session.Flush();
            }
            transaction.Commit();
          }
          catch (Exception ex)
          {
            KvrplHelper.WriteLog(ex, (LsClient) null);
            transaction.Rollback();
          }
        }
        base.tsbDelete_Click(sender, e);
      }
      else
      {
        int num = (int) MessageBox.Show("Удаление невозможно! Существуют данные, ссылающиеся на эту запись.", "Внимание!", MessageBoxButtons.OK);
      }
    }

    protected override void tsbCancel_Click(object sender, EventArgs e)
    {
      this.dgvBase.EndEdit();
      foreach (short key in this._updId.Keys)
        this._updId[key].ParamId = key;
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
