// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.Controls.UCNormCost
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms.Controls
{
  public class UCNormCost : UCBase
  {
    public bool IsCopy = false;
    private bool isEdit = false;
    private bool isPast = false;
    private IContainer components = (IContainer) null;
    public DateTime? closedPeriod;
    private short company_id;
    private Service curService;
    private bool isArchive;
    private IList<CmpNorm> oldListNorm;

    public bool IsPast
    {
      get
      {
        return this.isPast;
      }
      set
      {
        if (this.tsbApplay.Enabled)
        {
          string str = "Изменения не сохранены! ";
          if (MessageBox.Show(this.isPast ? str + "Остаться в прошлом времени и сохранить изменения?" : str + "Остаться в настоящем времени и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
          {
            this.tsbApplay_Click((object) null, (EventArgs) null);
            return;
          }
          this.tsbCancel_Click((object) null, (EventArgs) null);
        }
        this.isPast = value;
        this.dgvBase.Columns["Period"].Visible = this.isPast;
        this.curObject = (object) null;
        this.GetList();
        if (this.objectsList.Count <= 0)
          return;
        this.CurIndex = 0;
        this.dt = new DateTime?(((CmpNorm) this.objectsList[0]).Dbeg);
        this.dgvBase.CurrentCell = this.dgvBase.Rows[0].Cells[1];
        this.dgvBase.Rows[0].Selected = true;
      }
    }

    public short Company_id
    {
      get
      {
        return this.company_id;
      }
      set
      {
        if ((int) this.company_id != (int) value)
        {
          if (this.tsbApplay.Enabled)
          {
            if (MessageBox.Show("Изменения не сохранены! Вернуться к предыдущей компании и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
              this.tsbApplay_Click((object) null, (EventArgs) null);
              return;
            }
            this.tsbCancel_Click((object) null, (EventArgs) null);
          }
          this.company_id = value;
          if (this.curService != null)
          {
            IList list = this.session.CreateQuery(string.Format("from Norm where Service_id={0} and Manager.BaseOrgId=(select Manager.BaseOrgId from Company where CompanyId={1})", (object) this.curService.ServiceId, (object) this.company_id)).List();
            list.Insert(0, (object) new Norm()
            {
              Norm_id = int.MaxValue
            });
            ((DataGridViewComboBoxColumn) this.dgvBase.Columns["Norm"]).DataSource = (object) list;
          }
          this.curObject = (object) null;
          this.GetList();
          if (this.objectsList.Count <= 0)
            return;
          this.CurIndex = 0;
          this.dt = new DateTime?(((CmpNorm) this.objectsList[0]).Dbeg);
          this.dgvBase.CurrentCell = this.dgvBase.Rows[0].Cells[1];
          this.dgvBase.Rows[0].Selected = true;
        }
        else
        {
          if (this.tsbApplay.Enabled)
            return;
          this.GetList();
        }
      }
    }

    public Service CurService
    {
      get
      {
        return this.curService;
      }
      set
      {
        if (this.curService != value)
        {
          if (this.tsbApplay.Enabled)
          {
            if (MessageBox.Show("Изменения не сохранены! Вернуться к предыдущей услуге и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
              this.tsbApplay_Click((object) null, (EventArgs) null);
              return;
            }
            this.tsbCancel_Click((object) null, (EventArgs) null);
          }
          this.curService = value;
          if (this.curService != null)
          {
            IList list = this.session.CreateQuery(string.Format("from Norm where Service_id={0} and Manager.BaseOrgId=(select Manager.BaseOrgId from Company where CompanyId={1})", (object) this.curService.ServiceId, (object) this.company_id)).List();
            list.Insert(0, (object) new Norm()
            {
              Norm_id = int.MaxValue
            });
            ((DataGridViewComboBoxColumn) this.dgvBase.Columns["Norm"]).DataSource = (object) list;
          }
          this.curObject = (object) null;
          this.GetList();
          if (this.objectsList.Count <= 0)
            return;
          this.CurIndex = 0;
          this.dt = new DateTime?(((CmpNorm) this.objectsList[0]).Dbeg);
          this.dgvBase.CurrentCell = this.dgvBase.Rows[0].Cells[1];
          this.dgvBase.Rows[0].Selected = true;
        }
        else
        {
          if (this.tsbApplay.Enabled)
            return;
          this.GetList();
        }
      }
    }

    public bool IsArchive
    {
      set
      {
        this.isArchive = value;
        this.curObject = (object) null;
      }
    }

    public UCNormCost()
    {
      this.InitializeComponent();
      DataGridViewColumn dataGridViewColumn1 = (DataGridViewColumn) new MaskDateColumn();
      dataGridViewColumn1.Name = "Period";
      dataGridViewColumn1.HeaderText = "Период";
      dataGridViewColumn1.Visible = false;
      dataGridViewColumn1.ReadOnly = true;
      this.dgvBase.Columns.Add(dataGridViewColumn1);
      DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn2.Name = "NNorm";
      dataGridViewColumn2.HeaderText = "№";
      dataGridViewColumn2.DisplayIndex = 0;
      this.dgvBase.Columns.Add(dataGridViewColumn2);
      DataGridViewColumn dataGridViewColumn3 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn3.Name = "Norm";
      dataGridViewColumn3.HeaderText = "Норматив";
      dataGridViewColumn3.DisplayIndex = 1;
      ((DataGridViewComboBoxColumn) dataGridViewColumn3).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      ((DataGridViewComboBoxColumn) dataGridViewColumn3).DisplayMember = "Norm_name";
      ((DataGridViewComboBoxColumn) dataGridViewColumn3).ValueMember = "Norm_id";
      this.dgvBase.Columns.Add(dataGridViewColumn3);
      DataGridViewColumn dataGridViewColumn4 = (DataGridViewColumn) new MaskDateColumn();
      dataGridViewColumn4.Name = "Dbeg";
      dataGridViewColumn4.HeaderText = "Дата начала действия";
      dataGridViewColumn4.DisplayIndex = 2;
      this.dgvBase.Columns.Add(dataGridViewColumn4);
      DataGridViewColumn dataGridViewColumn5 = (DataGridViewColumn) new MaskDateColumn();
      dataGridViewColumn5.Name = "Dend";
      dataGridViewColumn5.HeaderText = "Дата окончания действия";
      dataGridViewColumn5.DisplayIndex = 3;
      this.dgvBase.Columns.Add(dataGridViewColumn5);
      DataGridViewColumn dataGridViewColumn6 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn6.Name = "Norm_value";
      dataGridViewColumn6.HeaderText = "Значение нормы для начислений";
      dataGridViewColumn6.DisplayIndex = 4;
      dataGridViewColumn6.DefaultCellStyle = new DataGridViewCellStyle()
      {
        Format = "N6"
      };
      this.dgvBase.Columns.Add(dataGridViewColumn6);
      DataGridViewColumn dataGridViewColumn7 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn7.Name = "NormMSP_value";
      dataGridViewColumn7.HeaderText = "Значение нормы для льгот";
      dataGridViewColumn7.DisplayIndex = 5;
      DataGridViewCellStyle gridViewCellStyle = new DataGridViewCellStyle();
      gridViewCellStyle.Format = "N6";
      dataGridViewColumn7.DefaultCellStyle = gridViewCellStyle;
      this.dgvBase.Columns.Add(dataGridViewColumn7);
      DataGridViewColumn dataGridViewColumn8 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn8.Name = "Norm_Value_C";
      dataGridViewColumn8.HeaderText = "Значение нормы для компенсации";
      dataGridViewColumn8.DisplayIndex = 6;
      gridViewCellStyle.Format = "N6";
      dataGridViewColumn8.ReadOnly = false;
      dataGridViewColumn8.DefaultCellStyle = gridViewCellStyle;
      this.dgvBase.Columns.Add(dataGridViewColumn8);
      if (Options.ViewEdit)
      {
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 7, "Пользователь", "UName", 80, true);
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 8, "Дата редактирования", "DEdit", 80, true);
      }
      this.MySettings.GridName = "NormCost";
    }

    protected override void GetList()
    {
      if (this.session != null && this.session.IsOpen)
        this.session.Clear();
      this.objectsList.Clear();
      if (this.session == null || this.curService == null)
        return;
      DateTime dateTime;
      if (this.closedPeriod.HasValue && !this.isArchive)
      {
        if (this.isPast)
        {
          this.objectsList = this.session.CreateQuery(string.Format("from CmpNorm cn left join fetch cn.Period where cn.Period.PeriodId<>{0} and cn.Company_id={1} and cn.Norm.Norm_id in (select Norm_id from Norm where Service_id={2}) and cn.Period.PeriodId=:id order by cn.Period.PeriodId,cn.Norm.Norm_num,cn.Dbeg", (object) 0, (object) this.Company_id, (object) this.curService.ServiceId)).SetInt32("id", Options.Period.PeriodId).List();
        }
        else
        {
          IQuery query = this.session.CreateQuery(string.Format("from CmpNorm cn left join fetch cn.Period where cn.Period.PeriodId={0} and cn.Company_id={1} and cn.Norm.Norm_id in (select Norm_id from Norm where Service_id={2}) and cn.Dend>=:dend order by cn.Norm.Norm_num,cn.Dbeg", (object) 0, (object) this.Company_id, (object) this.curService.ServiceId));
          string name = "dend";
          dateTime = this.closedPeriod.Value;
          DateTime val = dateTime.AddMonths(1);
          this.objectsList = query.SetDateTime(name, val).List();
        }
      }
      else if (this.isPast)
        this.objectsList = this.session.CreateQuery(string.Format("from CmpNorm cn left join fetch cn.Period where cn.Period.PeriodId<>{0} and cn.Company_id={1} and cn.Norm.Norm_id in (select Norm_id from Norm where Service_id={2}) order by cn.Period.PeriodId,cn.Norm.Norm_num,cn.Dbeg", (object) 0, (object) this.Company_id, (object) this.curService.ServiceId)).List();
      else
        this.objectsList = this.session.CreateQuery(string.Format("from CmpNorm cn left join fetch cn.Period where cn.Period.PeriodId={0} and cn.Company_id={1} and cn.Norm.Norm_id in (select Norm_id from Norm where Service_id={2}) order by cn.Norm.Norm_num,cn.Dbeg", (object) 0, (object) this.Company_id, (object) this.curService.ServiceId)).List();
      this.dgvBase.RowCount = this.objectsList.Count;
      this.dgvBase.Refresh();
      this.SelectRow();
      if ((uint) this.objectsList.Count > 0U)
      {
        this.session.Clear();
        if (this.closedPeriod.HasValue && !this.isArchive)
        {
          if (this.isPast)
          {
            this.oldListNorm = this.session.CreateQuery(string.Format("from CmpNorm cn left join fetch cn.Period where cn.Period.PeriodId<>{0} and cn.Company_id={1} and cn.Norm.Norm_id in (select Norm_id from Norm where Service_id={2}) and cn.Period.PeriodId=:id order by cn.Period.PeriodId,cn.Norm.Norm_num,cn.Dbeg", (object) 0, (object) this.Company_id, (object) this.curService.ServiceId)).SetInt32("id", Options.Period.PeriodId).List<CmpNorm>();
          }
          else
          {
            IQuery query = this.session.CreateQuery(string.Format("from CmpNorm cn left join fetch cn.Period where cn.Period.PeriodId={0} and cn.Company_id={1} and cn.Norm.Norm_id in (select Norm_id from Norm where Service_id={2}) and cn.Dend>=:dend order by cn.Norm.Norm_num,cn.Dbeg", (object) 0, (object) this.Company_id, (object) this.curService.ServiceId));
            string name = "dend";
            dateTime = this.closedPeriod.Value;
            DateTime val = dateTime.AddMonths(1);
            this.oldListNorm = query.SetDateTime(name, val).List<CmpNorm>();
          }
        }
        else
          this.oldListNorm = !this.isPast ? this.session.CreateQuery(string.Format("from CmpNorm cn left join fetch cn.Period where cn.Period.PeriodId={0} and cn.Company_id={1} and cn.Norm.Norm_id in (select Norm_id from Norm where Service_id={2}) order by cn.Norm.Norm_num,cn.Dbeg", (object) 0, (object) this.Company_id, (object) this.curService.ServiceId)).List<CmpNorm>() : this.session.CreateQuery(string.Format("from CmpNorm cn left join fetch cn.Period where cn.Period.PeriodId<>{0} and cn.Company_id={1} and cn.Norm.Norm_id in (select Norm_id from Norm where Service_id={2}) order by cn.Period.PeriodId,cn.Norm.Norm_num,cn.Dbeg", (object) 0, (object) this.Company_id, (object) this.curService.ServiceId)).List<CmpNorm>();
        int index = 0;
        foreach (CmpNorm cmpNorm in (IEnumerable<CmpNorm>) this.oldListNorm)
        {
          cmpNorm.OldHashCode = cmpNorm.GetHashCode();
          ((CmpNorm) this.objectsList[index]).OldHashCode = cmpNorm.OldHashCode;
          ++index;
        }
      }
    }

    public void LoadData()
    {
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
      if (this.closedPeriod.HasValue)
      {
        DateTime? nullable;
        int num;
        if (!this.isPast && ((CmpNorm) this.objectsList[e.RowIndex]).Dbeg < this.closedPeriod.Value.AddMonths(2))
        {
          nullable = ((CmpNorm) this.objectsList[e.RowIndex]).Dend;
          DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
          if ((nullable.HasValue ? (nullable.GetValueOrDefault() >= dateTime ? 1 : 0) : 0) != 0)
          {
            num = 1;
            goto label_8;
          }
        }
        if (this.isPast)
        {
          nullable = ((CmpNorm) this.objectsList[e.RowIndex]).Period.PeriodName;
          DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
          num = nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() == dateTime ? 1 : 0) : 1) : 0;
        }
        else
          num = 0;
label_8:
        if (num != 0)
          this.dgvBase.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.PapayaWhip;
        else
          this.dgvBase.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      }
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "NNorm")
      {
        Norm norm = this.session.Get<Norm>((object) ((CmpNorm) this.objectsList[e.RowIndex]).Norm.Norm_id);
        if (norm != null)
          e.Value = (object) norm.Norm_num;
      }
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Norm")
        e.Value = (object) Convert.ToInt32(((CmpNorm) this.objectsList[e.RowIndex]).Norm.Norm_id);
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dbeg")
        e.Value = (object) ((CmpNorm) this.objectsList[e.RowIndex]).Dbeg;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dend")
        e.Value = (object) ((CmpNorm) this.objectsList[e.RowIndex]).Dend;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Norm_value")
        e.Value = (object) ((CmpNorm) this.objectsList[e.RowIndex]).Norm_value;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "NormMSP_value")
        e.Value = (object) ((CmpNorm) this.objectsList[e.RowIndex]).NormMSP_value;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Norm_Value_C")
        e.Value = (object) ((CmpNorm) this.objectsList[e.RowIndex]).Norm_Value_C;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Period" && this.isPast)
        e.Value = (object) ((CmpNorm) this.objectsList[e.RowIndex]).Period.PeriodName;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "UName")
        e.Value = (object) ((CmpNorm) this.objectsList[e.RowIndex]).Uname;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "DEdit")
        e.Value = (object) ((CmpNorm) this.objectsList[e.RowIndex]).Dedit.ToShortDateString();
    }

    protected override void dgvBase_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dbeg" || this.dgvBase.Columns[e.ColumnIndex].Name == "Dend")
      {
        try
        {
          Convert.ToDateTime(e.Value);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Проверьте правильность введенных дат", "Внимание!", MessageBoxButtons.OK);
          this.isEdit = false;
          return;
        }
      }
      if (!this.isPast && this.dgvBase.Columns[e.ColumnIndex].Name == "Dbeg" && this.closedPeriod.Value.AddMonths(1) <= ((CmpNorm) this.objectsList[e.RowIndex]).Dbeg && this.closedPeriod.Value.AddMonths(1) > Convert.ToDateTime(e.Value))
      {
        int num = (int) MessageBox.Show("Попытка изменить дату начала на дату, принадлежащую закрытому периоду!", "Внимание!", MessageBoxButtons.OK);
        this.isEdit = false;
      }
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dend" && ((CmpNorm) this.objectsList[e.RowIndex]).Dbeg > Convert.ToDateTime(e.Value))
      {
        int num = (int) MessageBox.Show("Дата окончания не может быть меньше даты начала!", "Внимание!", MessageBoxButtons.OK);
        this.isEdit = false;
      }
      else
      {
        DateTime? nullable;
        int num1;
        if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dbeg")
        {
          nullable = ((CmpNorm) this.objectsList[e.RowIndex]).Dend;
          DateTime dateTime = Convert.ToDateTime(e.Value);
          num1 = nullable.HasValue ? (nullable.GetValueOrDefault() < dateTime ? 1 : 0) : 0;
        }
        else
          num1 = 0;
        if (num1 != 0)
        {
          int num2 = (int) MessageBox.Show("Дата начала не может быть больше даты окончания!", "Внимание!", MessageBoxButtons.OK);
          this.isEdit = false;
        }
        else if (this.session.CreateCriteria(typeof (ServiceParam)).Add((ICriterion) Restrictions.Eq("Company_id", (object) this.company_id)).Add((ICriterion) Restrictions.Eq("Complex.IdFk", (object) Options.Complex.IdFk)).Add((ICriterion) Restrictions.Eq("Service_id", (object) this.curService.ServiceId)).List<ServiceParam>().Count == 0)
        {
          int num3 = (int) MessageBox.Show("Невозможно внести изменения. Услуга отсутствует в списке услуг организаций", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          int num2;
          if (e.RowIndex < 0 || this.closedPeriod.HasValue)
          {
            if (this.objectsList.Count > 0 && this.closedPeriod.Value.AddMonths(1) <= ((CmpNorm) this.objectsList[e.RowIndex]).Dbeg || this.isPast)
            {
              if (this.isPast)
              {
                nullable = ((CmpNorm) this.objectsList[e.RowIndex]).Period.PeriodName;
                if (nullable.HasValue)
                {
                  DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
                  nullable = ((CmpNorm) this.objectsList[e.RowIndex]).Period.PeriodName;
                  if ((nullable.HasValue ? (dateTime <= nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0)
                    goto label_21;
                }
              }
              else
                goto label_21;
            }
            num2 = this.dgvBase.Columns[e.ColumnIndex].Name == "Dend" ? 1 : 0;
            goto label_22;
          }
label_21:
          num2 = 1;
label_22:
          if (num2 != 0)
          {
            if (e.RowIndex >= 0)
            {
              if (this.dgvBase.Columns[e.ColumnIndex].Name == "NNorm" && (this.newObject != null && e.RowIndex == this.objectsList.Count - 1))
              {
                Norm norm;
                try
                {
                  norm = this.session.CreateQuery("from Norm where norm_num=:num and Service=:srv and Manager.BaseOrgId=(select Manager.BaseOrgId from Company where CompanyId=:company)").SetParameter("num", e.Value).SetEntity("srv", (object) this.curService).SetParameter<short>("company", this.company_id).UniqueResult<Norm>();
                }
                catch
                {
                  int num4 = (int) MessageBox.Show("Данный номер норматива не уникален, выберите норматив из списка!", "Внимание!", MessageBoxButtons.OK);
                  return;
                }
                if (norm != null)
                  ((CmpNorm) this.objectsList[e.RowIndex]).Norm.Norm_id = norm.Norm_id;
                this.dgvBase.Refresh();
              }
              DateTime dateTime1;
              if (this.dgvBase.Columns[e.ColumnIndex].Name == "Norm")
              {
                if (this.newObject != null && e.RowIndex == this.objectsList.Count - 1)
                {
                  ((CmpNorm) this.objectsList[e.RowIndex]).Norm.Norm_id = Convert.ToInt32(e.Value);
                  this.dgvBase.Refresh();
                }
              }
              else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dbeg")
              {
                if (this.isPast && Convert.ToDateTime(e.Value) < Convert.ToDateTime("01.01.2005"))
                {
                  int num4 = (int) MessageBox.Show("Дата начала должна быть больше 01.01.2005 г.", "Внимание!", MessageBoxButtons.OK);
                  this.isEdit = false;
                  return;
                }
                int num5;
                if (this.isPast && this.closedPeriod.HasValue)
                {
                  DateTime dateTime2 = Convert.ToDateTime(e.Value);
                  dateTime1 = this.closedPeriod.Value;
                  DateTime dateTime3 = dateTime1.AddMonths(1);
                  num5 = dateTime2 >= dateTime3 ? 1 : 0;
                }
                else
                  num5 = 0;
                if (num5 != 0)
                {
                  int num4 = (int) MessageBox.Show("Дата начала не может быть больше даты окончания закрытого периода!", "Внимание!", MessageBoxButtons.OK);
                  this.isEdit = false;
                  return;
                }
                try
                {
                  ((CmpNorm) this.objectsList[e.RowIndex]).Dbeg = Convert.ToDateTime(e.Value);
                }
                catch (Exception ex)
                {
                  int num4 = (int) MessageBox.Show("Проверьте правильность ввода даты!", "Внимание!", MessageBoxButtons.OK);
                  this.isEdit = false;
                  return;
                }
              }
              else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dend")
              {
                try
                {
                  int num4;
                  if (this.closedPeriod.HasValue && !this.isPast)
                  {
                    DateTime dateTime2 = Convert.ToDateTime(e.Value);
                    dateTime1 = this.closedPeriod.Value;
                    dateTime1 = dateTime1.AddMonths(1);
                    DateTime dateTime3 = dateTime1.AddDays(-1.0);
                    if (dateTime2 >= dateTime3)
                    {
                      nullable = ((CmpNorm) this.objectsList[e.RowIndex]).Dend;
                      DateTime dateTime4 = this.closedPeriod.Value;
                      dateTime4 = dateTime4.AddMonths(1);
                      dateTime1 = dateTime4.AddDays(-1.0);
                      num4 = nullable.HasValue ? (nullable.GetValueOrDefault() >= dateTime1 ? 1 : 0) : 0;
                    }
                    else
                      num4 = 0;
                  }
                  else
                    num4 = 1;
                  if (num4 != 0)
                  {
                    int num5;
                    if (this.isPast && this.closedPeriod.HasValue)
                    {
                      DateTime dateTime2 = Convert.ToDateTime(e.Value);
                      dateTime1 = this.closedPeriod.Value;
                      DateTime dateTime3 = dateTime1.AddMonths(1);
                      num5 = dateTime2 >= dateTime3 ? 1 : 0;
                    }
                    else
                      num5 = 0;
                    if (num5 != 0)
                    {
                      int num6 = (int) MessageBox.Show("Дата окончания не может быть больше даты окончания закрытого периода!", "Внимание!", MessageBoxButtons.OK);
                      this.isEdit = false;
                      return;
                    }
                    ((CmpNorm) this.objectsList[e.RowIndex]).Dend = new DateTime?(Convert.ToDateTime(e.Value));
                  }
                  else
                  {
                    int num5 = (int) MessageBox.Show("Дата окончания принадлежит закрытому периоду, либо существует запись, принадлежащая закрытому периоду, которая перекрывается этой датой!", "Внимание!", MessageBoxButtons.OK);
                    this.isEdit = false;
                    return;
                  }
                }
                catch (Exception ex)
                {
                  int num4 = (int) MessageBox.Show("Проверьте правильность ввода даты!", "Внимание!", MessageBoxButtons.OK);
                  this.isEdit = false;
                  return;
                }
              }
              else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Norm_value" && e.Value != null)
              {
                try
                {
                  ((CmpNorm) this.objectsList[e.RowIndex]).Norm_value = Convert.ToDouble(KvrplHelper.ChangeSeparator(e.Value.ToString()));
                  ((CmpNorm) this.objectsList[e.RowIndex]).Norm_Value_C = Convert.ToDouble(KvrplHelper.ChangeSeparator(e.Value.ToString()));
                }
                catch
                {
                  int num4 = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
                  this.isEdit = false;
                  return;
                }
              }
              else if (this.dgvBase.Columns[e.ColumnIndex].Name == "NormMSP_value" && e.Value != null)
              {
                try
                {
                  ((CmpNorm) this.objectsList[e.RowIndex]).NormMSP_value = Convert.ToDouble(KvrplHelper.ChangeSeparator(e.Value.ToString()));
                }
                catch
                {
                  int num4 = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
                  this.isEdit = false;
                  return;
                }
              }
              else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Norm_Value_C" && e.Value != null)
              {
                try
                {
                  ((CmpNorm) this.objectsList[e.RowIndex]).Norm_Value_C = Convert.ToDouble(KvrplHelper.ChangeSeparator(e.Value.ToString()));
                }
                catch
                {
                  int num4 = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
                  this.isEdit = false;
                  return;
                }
              }
              this.curObject = this.objectsList[e.RowIndex];
              if (this.newObject == null && this.curObject != null)
              {
                ((CmpNorm) this.curObject).Uname = Options.Login;
                CmpNorm curObject = (CmpNorm) this.curObject;
                dateTime1 = DateTime.Now;
                DateTime date = dateTime1.Date;
                curObject.Dedit = date;
              }
            }
            this.isEdit = true;
          }
          else
          {
            int num4 = (int) MessageBox.Show("Попытка внести изменения в закрытом периоде!", "Внимание!", MessageBoxButtons.OK);
            this.isEdit = false;
          }
        }
      }
    }

    protected override void tsbAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, this.session.Get<Company>((object) this.company_id), true))
        return;
      this.newObject = (object) new CmpNorm();
      ((CmpNorm) this.newObject).Norm = new Norm((Service) null, "", int.MaxValue, 0);
      if (this.closedPeriod.HasValue)
      {
        if (this.isPast)
          ((CmpNorm) this.newObject).Dbeg = this.closedPeriod.Value;
        else
          ((CmpNorm) this.newObject).Dbeg = this.closedPeriod.Value.AddMonths(1);
      }
      else
        ((CmpNorm) this.newObject).Dbeg = DateTime.Now;
      if (!this.isPast)
        ((CmpNorm) this.newObject).Period = this.session.Get<Period>((object) Convert.ToInt32(this.session.CreateQuery("select p.PeriodId from Period p where PeriodName is null").UniqueResult()));
      else if (this.closedPeriod.HasValue)
      {
        ((CmpNorm) this.newObject).Period = (Period) this.session.CreateQuery("from Period p where p.PeriodName=:dt").SetDateTime("dt", this.closedPeriod.Value.AddMonths(1)).UniqueResult();
      }
      else
      {
        int num = (int) MessageBox.Show("Отсутствует закрытый период!", "Внимание!", MessageBoxButtons.OK);
        return;
      }
      ((CmpNorm) this.newObject).Company_id = (int) this.Company_id;
      ((CmpNorm) this.newObject).Norm_Value_C = ((CmpNorm) this.newObject).Norm_value;
      if (this.closedPeriod.HasValue && this.isPast)
        ((CmpNorm) this.newObject).Dend = new DateTime?(this.closedPeriod.Value.AddMonths(1).AddDays(-1.0));
      else
        ((CmpNorm) this.newObject).Dend = new DateTime?(Convert.ToDateTime("31.12.2999"));
      base.tsbAdd_Click(sender, e);
      this.dt = new DateTime?(((CmpNorm) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg);
      this.SelectRow();
    }

    protected override void tsbApplay_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, this.session.Get<Company>((object) this.company_id), true))
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
          if (this.closedPeriod.HasValue && ((CmpNorm) this.newObject).Dbeg < this.closedPeriod.Value.AddMonths(1) && !this.isPast)
          {
            int num1 = (int) MessageBox.Show("Дата начала принадлежит закрытому периоду!", "Внимание!", MessageBoxButtons.OK);
          }
          else
          {
            if (((CmpNorm) this.newObject).Norm.Norm_id == int.MaxValue)
            {
              int num2 = (int) MessageBox.Show("Выберите норматив!", "Внимание!", MessageBoxButtons.OK);
              return;
            }
            if ((((CmpNorm) this.newObject).Dbeg <= DateTime.Now.AddYears(-3) || ((CmpNorm) this.newObject).Dbeg >= DateTime.Now.AddYears(3)) && MessageBox.Show("Дата начала отличается от текущей более, чем на 3 года. Продолжить", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
              return;
            if (!this.isPast)
              ((CmpNorm) this.newObject).Period = this.session.Get<Period>((object) 0);
            ((CmpNorm) this.newObject).Uname = Options.Login;
            ((CmpNorm) this.newObject).Dedit = DateTime.Now.Date;
            if (Convert.ToInt32(KvrplHelper.BaseValue(32, this.session.Get<Company>((object) this.company_id))) == 1)
            {
              if (Convert.ToInt32(KvrplHelper.BaseValue(1, this.session.Get<Company>((object) this.company_id))) == 28 && MessageBox.Show("Внести основание?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
              {
                FrmArgument frmArgument = new FrmArgument();
                int num2 = (int) frmArgument.ShowDialog();
                frmArgument.Dispose();
                KvrplHelper.SaveNormFromNoteBook((CmpNorm) this.newObject, (CmpNorm) null, this.session.Get<Company>((object) this.company_id), (short) 1, this.curService.ServiceName + " варианту № " + this.dgvBase.CurrentRow.Cells["NNorm"].FormattedValue.ToString() + " (" + this.dgvBase.CurrentRow.Cells["Norm"].FormattedValue.ToString() + ")", frmArgument.Argument(), (this.isPast ? 1 : 0) != 0, this.closedPeriod.Value);
              }
              else
                KvrplHelper.SaveNormFromNoteBook((CmpNorm) this.newObject, (CmpNorm) null, this.session.Get<Company>((object) this.company_id), (short) 1, this.curService.ServiceName + " варианту № " + this.dgvBase.CurrentRow.Cells["NNorm"].FormattedValue.ToString() + " (" + this.dgvBase.CurrentRow.Cells["Norm"].FormattedValue.ToString() + ")", "", (this.isPast ? 1 : 0) != 0, this.closedPeriod.Value);
            }
          }
        }
        else
        {
          if ((((CmpNorm) this.curObject).Dbeg <= DateTime.Now.AddYears(-3) || ((CmpNorm) this.curObject).Dbeg >= DateTime.Now.AddYears(3)) && MessageBox.Show("Дата начала отличается от текущей более, чем на 3 года. Продолжить", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            return;
          if (!this.dt.Equals((object) ((CmpNorm) this.curObject).Dbeg))
          {
            using (ITransaction transaction = this.session.BeginTransaction())
            {
              try
              {
                this.session.CreateSQLQuery("update DBA.cmpNorm cmpN set Dbeg=:dbeg,Dend=:dend,Norm_value=:nv,NormMSP_value=:nmspv,Uname=:uname,Dedit=:dedit where cmpN.Company_id=:company_id and cmpN.Period_id=:period and cmpN.Dbeg=:dbeg2 and cmpN.Norm_id=:norm_id and cmpN.Norm_Value_C=:Norm_Value_C").SetDateTime("dbeg", ((CmpNorm) this.curObject).Dbeg).SetParameter<DateTime?>("dend", ((CmpNorm) this.curObject).Dend).SetParameter<double>("nv", ((CmpNorm) this.curObject).Norm_value).SetParameter<double>("nmspv", ((CmpNorm) this.curObject).NormMSP_value).SetDateTime("dbeg2", this.dt.Value).SetInt32("company_id", ((CmpNorm) this.curObject).Company_id).SetInt32("period", ((CmpNorm) this.curObject).Period.PeriodId).SetInt32("norm_id", ((CmpNorm) this.curObject).Norm.Norm_id).SetParameter<double>("Norm_Value_C", ((CmpNorm) this.curObject).Norm_Value_C).SetString("uname", Options.Login).SetDateTime("dedit", DateTime.Now.Date).ExecuteUpdate();
                transaction.Commit();
                this.session.Flush();
              }
              catch (Exception ex)
              {
                int num2 = (int) MessageBox.Show("Не удалось внести изменения! Проверьте правильность ввода данных.", "Внимание!", MessageBoxButtons.OK);
                KvrplHelper.WriteLog(ex, (LsClient) null);
                transaction.Rollback();
                return;
              }
            }
            this.CurIndex = -1;
          }
          CmpNorm oldNorm = new CmpNorm();
          foreach (CmpNorm cmpNorm in (IEnumerable<CmpNorm>) this.oldListNorm)
          {
            if (cmpNorm.OldHashCode == ((CmpNorm) this.objectsList[this.dgvBase.CurrentRow.Index]).OldHashCode)
            {
              oldNorm = cmpNorm;
              break;
            }
          }
          if (Convert.ToInt32(KvrplHelper.BaseValue(32, this.session.Get<Company>((object) this.company_id))) == 1)
            KvrplHelper.SaveNormFromNoteBook((CmpNorm) this.curObject, oldNorm, this.session.Get<Company>((object) this.company_id), (short) 2, this.curService.ServiceName + " варианту № " + this.dgvBase.CurrentRow.Cells["NNorm"].FormattedValue.ToString() + " (" + this.dgvBase.CurrentRow.Cells["Norm"].FormattedValue.ToString() + ")", "", (this.isPast ? 1 : 0) != 0, this.closedPeriod.Value);
        }
        base.tsbApplay_Click(sender, e);
        try
        {
          if (this.dgvBase.CurrentRow.Index > 0)
          {
            if (this.dgvBase.CurrentRow.Index < this.objectsList.Count)
              this.session.Refresh(this.objectsList[this.dgvBase.CurrentRow.Index]);
            this.session.Refresh(this.objectsList[this.dgvBase.CurrentRow.Index - 1]);
          }
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        if (this.dgvBase.CurrentRow != null)
          this.CurIndex = this.dgvBase.CurrentRow.Index;
        this.dgvBase.Refresh();
        this.dt = new DateTime?(((CmpNorm) this.curObject).Dbeg);
      }
    }

    protected override void tsbCancel_Click(object sender, EventArgs e)
    {
      if (this.curObject != null && !this.dt.Equals((object) ((CmpNorm) this.curObject).Dbeg))
        ((CmpNorm) this.curObject).Dbeg = this.dt.Value;
      base.tsbCancel_Click(sender, e);
      if (this.dgvBase.CurrentRow != null)
        this.CurIndex = this.dgvBase.CurrentRow.Index;
      this.IsCopy = false;
    }

    protected override void tsbDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, this.session.Get<Company>((object) this.company_id), true) || MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      if (this.objectsList.Count > 0)
      {
        int num1;
        if (this.closedPeriod.HasValue)
        {
          if (this.closedPeriod.Value.AddMonths(1) <= ((CmpNorm) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg || this.isPast)
          {
            if (this.isPast)
            {
              if (((CmpNorm) this.objectsList[this.dgvBase.CurrentRow.Index]).Period.PeriodName.HasValue)
              {
                DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
                DateTime? periodName = ((CmpNorm) this.objectsList[this.dgvBase.CurrentRow.Index]).Period.PeriodName;
                num1 = periodName.HasValue ? (dateTime <= periodName.GetValueOrDefault() ? 1 : 0) : 0;
              }
              else
                num1 = 0;
            }
            else
              num1 = 1;
          }
          else
            num1 = 0;
        }
        else
          num1 = 1;
        if (num1 != 0)
        {
          if (this.curObject != null)
          {
            try
            {
              this.session.Clear();
              if (((CmpNorm) this.curObject).Period != null && ((CmpNorm) this.curObject).Period.PeriodId == 0)
                ((CmpNorm) this.curObject).Period = this.session.Get<Period>((object) 0);
              if (Convert.ToInt32(KvrplHelper.BaseValue(32, this.session.Get<Company>((object) this.company_id))) == 1)
                KvrplHelper.SaveNormFromNoteBook((CmpNorm) this.curObject, (CmpNorm) null, this.session.Get<Company>((object) this.company_id), (short) 3, this.curService.ServiceName + " варианту № " + this.dgvBase.CurrentRow.Cells["NNorm"].FormattedValue.ToString() + " (" + this.dgvBase.CurrentRow.Cells["Norm"].FormattedValue.ToString() + ")", "", (this.isPast ? 1 : 0) != 0, this.closedPeriod.Value);
              base.tsbDelete_Click(sender, e);
              this.dgvBase.Refresh();
            }
            catch (Exception ex)
            {
              int num2 = (int) MessageBox.Show("Не удалось удалить запись!", "Внимание!", MessageBoxButtons.OK);
              KvrplHelper.WriteLog(ex, (LsClient) null);
            }
          }
        }
        else
        {
          int num3 = (int) MessageBox.Show("Попытка удалить запись в закрытом периоде!", "Внимание!", MessageBoxButtons.OK);
        }
      }
      if (this.curObject != null)
      {
        this.dt = new DateTime?(((CmpNorm) this.curObject).Dbeg);
        this.CurIndex = this.dgvBase.CurrentRow.Index;
      }
    }

    protected override void dgvBase_SelectionChanged(object sender, EventArgs e)
    {
      if (this.dgvBase.CurrentRow == null || this.dgvBase.CurrentRow.Index >= this.objectsList.Count)
        return;
      if (this.newObject == null && (this.CurIndex >= 0 && this.objectsList.Count > this.CurIndex && !this.dt.Equals((object) ((CmpNorm) this.objectsList[this.CurIndex]).Dbeg) && this.CurIndex != this.dgvBase.CurrentRow.Index))
      {
        try
        {
          ((CmpNorm) this.objectsList[this.CurIndex]).Dbeg = this.dt.Value;
          this.session.Refresh(this.objectsList[this.CurIndex]);
          this.curObject = this.objectsList[this.dgvBase.CurrentRow.Index];
          this.GetList();
          this.CancelEnabled();
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      if (this.CurIndex != this.dgvBase.CurrentRow.Index && this.objectsList.Count > 0 && this.objectsList.Count > this.CurIndex)
      {
        this.dt = new DateTime?(((CmpNorm) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg);
        this.CurIndex = this.dgvBase.CurrentRow.Index;
        this.curObject = this.objectsList[this.dgvBase.CurrentRow.Index];
      }
    }

    protected override void tsmCopy_Click(object sender, EventArgs e)
    {
      this.isEdit = true;
      this.newObject = (object) new CmpNorm();
      ((CmpNorm) this.newObject).Copy((CmpNorm) this.curObject);
      DateTime dateTime1;
      int num;
      if (!this.isPast && this.closedPeriod.HasValue)
      {
        DateTime dbeg = ((CmpNorm) this.newObject).Dbeg;
        dateTime1 = this.closedPeriod.Value;
        DateTime dateTime2 = dateTime1.AddMonths(1);
        num = dbeg < dateTime2 ? 1 : 0;
      }
      else
        num = 0;
      if (num != 0)
      {
        CmpNorm newObject = (CmpNorm) this.newObject;
        dateTime1 = this.closedPeriod.Value;
        DateTime dateTime2 = dateTime1.AddMonths(1);
        newObject.Dbeg = dateTime2;
      }
      else if (this.isPast)
      {
        ((CmpNorm) this.newObject).Dbeg = this.closedPeriod.Value;
      }
      else
      {
        CmpNorm newObject = (CmpNorm) this.newObject;
        dateTime1 = ((CmpNorm) this.newObject).Dbeg;
        dateTime1 = dateTime1.AddMonths(1);
        DateTime dateTime2 = dateTime1.AddDays((double) (-((CmpNorm) this.newObject).Dbeg.Day + 1));
        newObject.Dbeg = dateTime2;
      }
      if (this.closedPeriod.HasValue && this.isPast)
      {
        CmpNorm newObject = (CmpNorm) this.newObject;
        dateTime1 = this.closedPeriod.Value;
        dateTime1 = dateTime1.AddMonths(1);
        DateTime? nullable = new DateTime?(dateTime1.AddDays(-1.0));
        newObject.Dend = nullable;
      }
      base.tsbAdd_Click(sender, e);
      this.dt = new DateTime?(((CmpNorm) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg);
      base.tsmCopy_Click(sender, e);
    }

    protected override void tsmCopyInPast_Click(object sender, EventArgs e)
    {
      if (!this.isPast)
        this.IsCopy = true;
      this.newObject = (object) new CmpNorm();
      ((CmpNorm) this.newObject).Copy((CmpNorm) this.curObject);
      if (this.closedPeriod.HasValue)
      {
        ((CmpNorm) this.newObject).Period = (Period) this.session.CreateQuery("from Period p where p.PeriodName=:dt").SetDateTime("dt", this.closedPeriod.Value.AddMonths(1)).UniqueResult();
        if (!this.isPast)
          this.IsPast = !this.isPast;
        DateTime dateTime1;
        int num;
        if (!this.isPast && this.closedPeriod.HasValue)
        {
          DateTime dbeg = ((CmpNorm) this.newObject).Dbeg;
          dateTime1 = this.closedPeriod.Value;
          DateTime dateTime2 = dateTime1.AddMonths(1);
          num = dbeg < dateTime2 ? 1 : 0;
        }
        else
          num = 0;
        if (num != 0)
        {
          CmpNorm newObject = (CmpNorm) this.newObject;
          dateTime1 = this.closedPeriod.Value;
          DateTime dateTime2 = dateTime1.AddMonths(1);
          newObject.Dbeg = dateTime2;
        }
        else if (this.isPast)
          ((CmpNorm) this.newObject).Dbeg = this.closedPeriod.Value;
        if (this.closedPeriod.HasValue && this.isPast)
        {
          CmpNorm newObject = (CmpNorm) this.newObject;
          dateTime1 = this.closedPeriod.Value;
          dateTime1 = dateTime1.AddMonths(1);
          DateTime? nullable = new DateTime?(dateTime1.AddDays(-1.0));
          newObject.Dend = nullable;
        }
        base.tsbAdd_Click(sender, e);
        this.dt = new DateTime?(((CmpNorm) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg);
        base.tsmCopyInPast_Click(sender, e);
      }
      else
      {
        int num1 = (int) MessageBox.Show("Отсутствует закрытый период!", "Внимание!", MessageBoxButtons.OK);
      }
    }

    protected override void dgvBase_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
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
      this.components = (IContainer) new Container();
    }
  }
}
