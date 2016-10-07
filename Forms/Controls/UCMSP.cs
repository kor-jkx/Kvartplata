// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.Controls.UCMSP
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms.Controls
{
  public class UCMSP : UCBase
  {
    private bool isEdit = false;
    private Dictionary<int, DcMSP> _updId = new Dictionary<int, DcMSP>();
    private IContainer components = (IContainer) null;
    public DateTime? closedPeriod;

    public DcMSP CurMSP
    {
      get
      {
        return (DcMSP) this.curObject;
      }
    }

    public IList ObjectsList
    {
      get
      {
        return this.objectsList;
      }
    }

    public UCMSP()
    {
      this.InitializeComponent();
      this.dgvBase.ContextMenuStrip = (ContextMenuStrip) null;
      DataGridViewColumn dataGridViewColumn1 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn1.Name = "numColumn";
      dataGridViewColumn1.HeaderText = "Код";
      this.dgvBase.Columns.Add(dataGridViewColumn1);
      DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn2.Name = "nameColumn";
      dataGridViewColumn2.HeaderText = "Наименование категории льгот";
      this.dgvBase.Columns.Add(dataGridViewColumn2);
      DataGridViewColumn dataGridViewColumn3 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn3.Name = "socColumn";
      dataGridViewColumn3.HeaderText = "Код по соцзащите";
      this.dgvBase.Columns.Add(dataGridViewColumn3);
      DataGridViewColumn dataGridViewColumn4 = (DataGridViewColumn) new MaskDateColumn();
      dataGridViewColumn4.Name = "periodColumn";
      dataGridViewColumn4.HeaderText = "Монетизирована с";
      this.dgvBase.Columns.Add(dataGridViewColumn4);
      DataGridViewColumn dataGridViewColumn5 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn5.Name = "priorityColumn";
      dataGridViewColumn5.HeaderText = "Порядок применения";
      this.dgvBase.Columns.Add(dataGridViewColumn5);
      DataGridViewColumn dataGridViewColumn6 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn6.Name = "accountColumn";
      dataGridViewColumn6.HeaderText = "Учет предыдущих льгот";
      ((DataGridViewComboBoxColumn) dataGridViewColumn6).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      ((DataGridViewComboBoxColumn) dataGridViewColumn6).DisplayMember = "Name";
      ((DataGridViewComboBoxColumn) dataGridViewColumn6).ValueMember = "Id";
      this.dgvBase.Columns.Add(dataGridViewColumn6);
      if (Options.ViewEdit)
      {
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 6, "Пользователь", "UName", 80, true);
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 7, "Дата редактирования", "DEdit", 80, true);
      }
      this.MySettings.GridName = "MSP";
    }

    protected override void GetList()
    {
      if (this.session != null && this.session.IsOpen)
        this.session.Clear();
      this.objectsList.Clear();
      this.objectsList = this.session.CreateQuery("from DcMSP msp left join fetch msp.MSPPeriod order by msp.MSP_id").List();
      if (this.objectsList.Count == 0)
        this.curObject = (object) null;
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

    private bool FindByID(int id)
    {
      foreach (DcMSP objects in (IEnumerable) this.objectsList)
      {
        if (objects.MSP_id == id)
          return true;
      }
      return false;
    }

    protected override void dgvBase_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.objectsList.Count <= 0)
        return;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "numColumn")
        e.Value = (object) ((DcMSP) this.objectsList[e.RowIndex]).MSP_id;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "nameColumn")
        e.Value = (object) ((DcMSP) this.objectsList[e.RowIndex]).MSP_name;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "socColumn")
        e.Value = (object) ((DcMSP) this.objectsList[e.RowIndex]).CodeSoc;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "periodColumn")
        e.Value = (object) ((DcMSP) this.objectsList[e.RowIndex]).MSPPeriod.PeriodName.Value;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "priorityColumn")
        e.Value = (object) ((DcMSP) this.objectsList[e.RowIndex]).Priority;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "accountColumn")
        e.Value = (object) ((DcMSP) this.objectsList[e.RowIndex]).Account;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "UName")
        e.Value = (object) ((DcMSP) this.objectsList[e.RowIndex]).Uname;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "DEdit")
        e.Value = (object) ((DcMSP) this.objectsList[e.RowIndex]).Dedit.ToShortDateString();
    }

    protected override void dgvBase_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      int int32_1 = Convert.ToInt32(this.session.CreateQuery(string.Format("select count(*) from SdcMSPPercent where MSP_id={0}", (object) ((DcMSP) this.curObject).MSP_id)).UniqueResult());
      try
      {
        if (this.dgvBase.Columns[e.ColumnIndex].Name == "nameColumn")
          ((DcMSP) this.objectsList[e.RowIndex]).MSP_name = e.Value.ToString();
        else if (this.dgvBase.Columns[e.ColumnIndex].Name == "socColumn" && e.Value != null)
          ((DcMSP) this.objectsList[e.RowIndex]).CodeSoc = Convert.ToInt32(e.Value);
        if (int32_1 == 0 || this.dgvBase.Columns[e.ColumnIndex].Name == "periodColumn")
        {
          if (this.dgvBase.Columns[e.ColumnIndex].Name == "numColumn" && e.Value != null)
          {
            int int32_2 = Convert.ToInt32(e.Value);
            if (this.FindByID(int32_2))
            {
              this.isEdit = false;
              int num = (int) MessageBox.Show("Льгота с таким номером уже заведена! Выберите другой номер!", "Внимание!", MessageBoxButtons.OK);
              return;
            }
            this._updId.Add(((DcMSP) this.objectsList[e.RowIndex]).MSP_id, (DcMSP) this.objectsList[e.RowIndex]);
            ((DcMSP) this.objectsList[e.RowIndex]).MSP_id = int32_2;
            this.dgvBase.Refresh();
          }
          else if (this.dgvBase.Columns[e.ColumnIndex].Name == "periodColumn" && e.Value != null)
          {
            Period period1 = new Period();
            try
            {
              Period period2 = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) KvrplHelper.FirstDay(Convert.ToDateTime(e.Value)))).List<Period>()[0];
              if (Convert.ToInt32(this.session.CreateQuery(string.Format("select max(Period.PeriodId) from CompanyPeriod where Complex.ComplexId={0}", (object) Options.ComplexPasp.ComplexId)).UniqueResult()) < period2.PeriodId)
              {
                ((DcMSP) this.objectsList[e.RowIndex]).MSPPeriod = period2;
              }
              else
              {
                int num = (int) MessageBox.Show("Введенный месяц принадлежит закрытому периоду", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              }
            }
            catch (Exception ex)
            {
              int num = (int) MessageBox.Show("Проверьте корректность введенной даты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
          else if (this.dgvBase.Columns[e.ColumnIndex].Name == "priorityColumn" && e.Value != null)
            ((DcMSP) this.objectsList[e.RowIndex]).Priority = Convert.ToInt16(e.Value);
          else if (this.dgvBase.Columns[e.ColumnIndex].Name == "accountColumn" && e.Value != null)
            ((DcMSP) this.objectsList[e.RowIndex]).Account = Convert.ToInt16(e.Value);
          this.curObject = this.objectsList[e.RowIndex];
          if (this.newObject == null && this.curObject != null)
          {
            ((DcMSP) this.curObject).Uname = Options.Login;
            ((DcMSP) this.curObject).Dedit = DateTime.Now;
          }
        }
        else
        {
          int num = (int) MessageBox.Show("Изменение невозможно! Существуют данные, заведенные на эту льготу.", "Внимание!", MessageBoxButtons.OK);
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
      this.isEdit = true;
    }

    protected override void tsbAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      object obj = this.session.CreateQuery("select max(MSP_id) from DcMSP ").UniqueResult();
      int num = obj != null ? (int) obj + 1 : 1;
      this.newObject = (object) new DcMSP();
      ((DcMSP) this.newObject).MSP_id = num;
      ((DcMSP) this.newObject).MSP_name = "Новая категория льгот";
      ((DcMSP) this.newObject).MSPPeriod = this.session.Get<Period>((object) 1201);
      ((DcMSP) this.newObject).Priority = (short) 1;
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
          ((DcMSP) this.newObject).Uname = Options.Login;
          ((DcMSP) this.newObject).Dedit = DateTime.Now;
          if ((int) ((DcMSP) this.newObject).Priority < 1)
          {
            int num = (int) MessageBox.Show("Значение приоритета не может быть меньше 1", "Внимание!", MessageBoxButtons.OK);
            ((DcMSP) this.newObject).Priority = (short) 1;
          }
        }
        else
        {
          try
          {
            if ((int) ((DcMSP) this.curObject).Priority < 1)
            {
              int num = (int) MessageBox.Show("Значение приоритета не может быть меньше 1", "Внимание!", MessageBoxButtons.OK);
              ((DcMSP) this.curObject).Priority = (short) 1;
            }
            foreach (int key in this._updId.Keys)
            {
              this.UpdateQuery = "update DBA.dcMSP set msp_id=:newid,msp_name=:name,codeSoc=:cS,complex_id=:cId,priority=:p,account=:a,mspperiod_id=:mspPeriod,uname=:uname,dedit=:d where MSP_id=:id";
              if (this.UpdateQuery != "")
                this.session.CreateSQLQuery(this.UpdateQuery).SetInt32("newid", this._updId[key].MSP_id).SetString("name", this._updId[key].MSP_name).SetInt32("cS", this._updId[key].CodeSoc).SetInt32("cId", this._updId[key].Complex_id).SetInt16("p", this._updId[key].Priority).SetInt16("a", this._updId[key].Account).SetInt32("mspperiod", this._updId[key].MSPPeriod.PeriodId).SetString("uname", this._updId[key].Uname).SetDateTime("d", this._updId[key].Dedit).SetInt32("id", key).ExecuteUpdate();
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
    }

    protected override void tsbDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true) || MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      bool flag = true;
      IList list = this.session.CreateQuery("from SdcMSPPercent where MSP_id=:id").SetInt32("id", ((DcMSP) this.curObject).MSP_id).List();
      if (list.Count == 0)
        flag = true;
      this.session.Clear();
      foreach (SdcMSPPercent sdcMspPercent in (IEnumerable) list)
      {
        if (sdcMspPercent.Dbeg < this.closedPeriod.Value.AddMonths(1) && !sdcMspPercent.Period.PeriodName.HasValue || sdcMspPercent.Period.PeriodName.HasValue && sdcMspPercent.Period.PeriodName.Value <= this.closedPeriod.Value)
          flag = false;
      }
      if (flag)
      {
        foreach (SdcMSPPercent sdcMspPercent in (IEnumerable) list)
        {
          this.session.Delete((object) sdcMspPercent);
          this.session.Flush();
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
      foreach (int key in this._updId.Keys)
        this._updId[key].MSP_id = key;
      this._updId.Clear();
      base.tsbCancel_Click(sender, e);
    }

    protected override void dgvBase_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      base.dgvBase_ColumnWidthChanged(sender, e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.Margin = new Padding(0, 0, 0, 0);
      this.Name = "UCMSP";
      this.Size = new Size(438, 428);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
