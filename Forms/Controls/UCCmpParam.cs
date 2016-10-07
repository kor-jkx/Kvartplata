// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.Controls.UCCmpParam
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

namespace Kvartplata.Forms.Controls
{
  public class UCCmpParam : UCBase
  {
    public bool IsCopy = false;
    private bool isArchive = false;
    private bool isEdit = false;
    private bool isPast = false;
    private Dictionary<int, AdmTbl> tablesList = new Dictionary<int, AdmTbl>();
    private IContainer components = (IContainer) null;
    public DateTime? closedPeriod;
    private short company_id;
    private SndAddress sndAddress;
    private Param curParam;
    private IList valueList;

    public bool IsPast
    {
      get
      {
        return this.isPast;
      }
      set
      {
        if (this.isPast != value)
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
          this.dt = new DateTime?(((CmpParam) this.objectsList[0]).Dbeg);
          this.dgvBase.CurrentCell = this.dgvBase.Rows[0].Cells[1];
          this.dgvBase.Rows[0].Selected = true;
        }
        else
        {
          if (!this.tsbApplay.Enabled)
            return;
          this.GetList();
        }
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
          this.curObject = (object) null;
          this.GetList();
          if (this.objectsList.Count <= 0)
            return;
          this.CurIndex = 0;
          this.dt = new DateTime?(((CmpParam) this.objectsList[0]).Dbeg);
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

    public UCCmpParam()
    {
      this.InitializeComponent();
      this.dgvBase.CellClick += new DataGridViewCellEventHandler(this.dgvBase_CellClick);
      DataGridViewColumn dataGridViewColumn1 = (DataGridViewColumn) new MaskDateColumn();
      dataGridViewColumn1.Name = "Period";
      dataGridViewColumn1.HeaderText = "Период";
      dataGridViewColumn1.Visible = false;
      dataGridViewColumn1.ReadOnly = true;
      this.dgvBase.Columns.Add(dataGridViewColumn1);
      DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn2.Name = "ParamColumn";
      dataGridViewColumn2.HeaderText = "Параметр";
      dataGridViewColumn2.Width = 250;
      ((DataGridViewComboBoxColumn) dataGridViewColumn2).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      ((DataGridViewComboBoxColumn) dataGridViewColumn2).DisplayMember = "ParamName";
      ((DataGridViewComboBoxColumn) dataGridViewColumn2).ValueMember = "ParamId";
      this.dgvBase.Columns.Add(dataGridViewColumn2);
      DataGridViewColumn dataGridViewColumn3 = (DataGridViewColumn) new MaskDateColumn();
      dataGridViewColumn3.Name = "Dbeg";
      dataGridViewColumn3.HeaderText = "Дата начала действия";
      this.dgvBase.Columns.Add(dataGridViewColumn3);
      DataGridViewColumn dataGridViewColumn4 = (DataGridViewColumn) new MaskDateColumn();
      dataGridViewColumn4.Name = "Dend";
      dataGridViewColumn4.HeaderText = "Дата окончания действия";
      this.dgvBase.Columns.Add(dataGridViewColumn4);
      DataGridViewColumn dataGridViewColumn5 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn5.Name = "Param_value";
      dataGridViewColumn5.HeaderText = "Значение";
      new DataGridViewCellStyle().Format = "N5";
      dataGridViewColumn5.Width = 120;
      this.dgvBase.Columns.Add(dataGridViewColumn5);
      if (!Options.ViewEdit)
        return;
      KvrplHelper.AddTextBoxColumn(this.dgvBase, 5, "Пользователь", "UName", 100, true);
      KvrplHelper.AddTextBoxColumn(this.dgvBase, 6, "Дата редактирования", "DEdit", 100, true);
    }

    protected override void GetList()
    {
      string str = "";
      if (Options.Kvartplata && !Options.Arenda)
        str = " and Param_id not in (216,217,218,219)";
      if (!Options.Kvartplata && Options.Arenda)
        str = " and Param_id not in (206,207,208,214)";
      if (this.session != null && this.session.IsOpen)
        this.session.Clear();
      this.objectsList.Clear();
      if (this.session == null)
        return;
      if (this.closedPeriod.HasValue && !this.isArchive)
      {
        if (this.isPast)
        {
          Period period = (Period) this.session.CreateQuery("from Period where PeriodName=:value").SetDateTime("value", this.closedPeriod.Value.AddMonths(1)).UniqueResult();
          this.objectsList = this.session.CreateQuery(string.Format("from CmpParam where Period_id<>{0} and Company_id={1} and period_id=:period " + str + " order by period_id,param_id,dbeg", (object) 0, (object) this.Company_id)).SetInt32("period", period.PeriodId).List();
        }
        else
          this.objectsList = this.session.CreateQuery(string.Format("from CmpParam where Period_id={0} and Company_id={1} and dend>=:dend  " + str + " order by param_id,dbeg", (object) 0, (object) this.Company_id)).SetDateTime("dend", this.closedPeriod.Value.AddMonths(1)).List();
      }
      else if (this.isPast)
        this.objectsList = this.session.CreateQuery(string.Format("from CmpParam where Period_id<>{0} and Company_id={1} " + str + " order by period_id,param_id,dbeg", (object) 0, (object) this.Company_id)).List();
      else
        this.objectsList = this.session.CreateQuery(string.Format("from CmpParam where Period_id={0} and Company_id={1} " + str + " order by param_id,dbeg", (object) 0, (object) this.Company_id)).List();
      this.dgvBase.RowCount = this.objectsList.Count;
      this.dgvBase.Refresh();
      this.SelectRow();
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
        int num;
        if (this.isPast || !(((CmpParam) this.objectsList[e.RowIndex]).Dbeg < this.closedPeriod.Value.AddMonths(2)) || !(((CmpParam) this.objectsList[e.RowIndex]).Dend >= this.closedPeriod.Value.AddMonths(1)))
        {
          if (this.isPast)
          {
            DateTime? periodName = ((CmpParam) this.objectsList[e.RowIndex]).Period.PeriodName;
            DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
            num = periodName.HasValue ? (periodName.HasValue ? (periodName.GetValueOrDefault() == dateTime ? 1 : 0) : 1) : 0;
          }
          else
            num = 0;
        }
        else
          num = 1;
        if (num != 0)
          this.dgvBase.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.PapayaWhip;
        else
          this.dgvBase.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      }
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "ParamColumn")
        e.Value = (object) Convert.ToInt16(((CmpParam) this.objectsList[e.RowIndex]).Param_id);
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dbeg")
        e.Value = (object) ((CmpParam) this.objectsList[e.RowIndex]).Dbeg;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dend")
        e.Value = (object) ((CmpParam) this.objectsList[e.RowIndex]).Dend;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Param_value")
      {
        try
        {
          if (this.tablesList.ContainsKey(((CmpParam) this.objectsList[e.RowIndex]).Param_id))
          {
            AdmTbl tables = this.tablesList[((CmpParam) this.objectsList[e.RowIndex]).Param_id];
            string str1 = "";
            object obj1 = new object();
            if (((CmpParam) this.objectsList[e.RowIndex]).Param_id == 214)
              str1 = " and SchemeType=8";
            if (((CmpParam) this.objectsList[e.RowIndex]).Param_id == 224 || ((CmpParam) this.objectsList[e.RowIndex]).Param_id == 225)
              str1 = " and SchemeType=16";
            if (((CmpParam) this.objectsList[e.RowIndex]).Param_id == 227 || ((CmpParam) this.objectsList[e.RowIndex]).Param_id == 228)
              str1 = " and SchemeType=18";
            object obj2;
            if (((CmpParam) this.objectsList[e.RowIndex]).Param_id == 223)
            {
              string str2 = " and Account=" + (object) this.company_id + " and Account_Type=0 ";
              if (this.newObject != null)
                obj2 = this.session.CreateQuery("select UploadDir from " + tables.ClassName + " where Account=" + (object) this.company_id + " and Account_Type=0 ").UniqueResult();
              else
                obj2 = this.session.CreateQuery("select UploadDir from " + tables.ClassName + " where " + tables.ClassNameId + "=:id" + str2).SetParameter<double?>("id", ((CmpParam) this.objectsList[e.RowIndex]).Param_value).UniqueResult();
            }
            else
              obj2 = this.session.CreateQuery("select " + tables.ClassNameName + " from " + tables.ClassName + " where " + tables.ClassNameId + "=:id" + str1).SetInt16("id", Convert.ToInt16((object) ((CmpParam) this.objectsList[e.RowIndex]).Param_value)).UniqueResult();
            e.Value = obj2.GetType() != typeof (DateTime) ? obj2 : (object) string.Format("{0:d}", obj2);
          }
          else
            e.Value = (object) Decimal.Round(Convert.ToDecimal((object) ((CmpParam) this.objectsList[e.RowIndex]).Param_value), 5);
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Period" && this.isPast)
        e.Value = (object) ((CmpParam) this.objectsList[e.RowIndex]).Period.PeriodName;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "UName")
        e.Value = (object) ((CmpParam) this.objectsList[e.RowIndex]).Uname;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "DEdit")
        e.Value = (object) ((CmpParam) this.objectsList[e.RowIndex]).Dedit.ToShortDateString();
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
      if (!this.isPast && this.dgvBase.Columns[e.ColumnIndex].Name == "Dbeg" && this.closedPeriod.Value.AddMonths(1) <= ((CmpParam) this.objectsList[e.RowIndex]).Dbeg && this.closedPeriod.Value.AddMonths(1) > Convert.ToDateTime(e.Value))
      {
        int num = (int) MessageBox.Show("Попытка изменить дату начала на дату, принадлежащую закрытому периоду!", "Внимание!", MessageBoxButtons.OK);
        this.isEdit = false;
      }
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dend" && ((CmpParam) this.objectsList[e.RowIndex]).Dbeg > Convert.ToDateTime(e.Value))
      {
        int num = (int) MessageBox.Show("Дата окончания не может быть меньше даты начала!", "Внимание!", MessageBoxButtons.OK);
        this.isEdit = false;
      }
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dbeg" && ((CmpParam) this.objectsList[e.RowIndex]).Dend < Convert.ToDateTime(e.Value))
      {
        int num = (int) MessageBox.Show("Дата начала не может быть больше даты окончания!", "Внимание!", MessageBoxButtons.OK);
        this.isEdit = false;
      }
      else
      {
        int num1;
        if (e.RowIndex < 0 || this.closedPeriod.HasValue)
        {
          if (this.closedPeriod.Value.AddMonths(1) <= ((CmpParam) this.objectsList[e.RowIndex]).Dbeg || this.isPast)
          {
            if (this.isPast)
            {
              DateTime? periodName = ((CmpParam) this.objectsList[e.RowIndex]).Period.PeriodName;
              if (periodName.HasValue)
              {
                DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
                periodName = ((CmpParam) this.objectsList[e.RowIndex]).Period.PeriodName;
                if ((periodName.HasValue ? (dateTime <= periodName.GetValueOrDefault() ? 1 : 0) : 0) != 0)
                  goto label_16;
              }
            }
            else
              goto label_16;
          }
          num1 = this.dgvBase.Columns[e.ColumnIndex].Name == "Dend" ? 1 : 0;
          goto label_17;
        }
label_16:
        num1 = 1;
label_17:
        if (num1 != 0)
        {
          if (e.RowIndex >= 0)
          {
            try
            {
              if (this.dgvBase.Columns[e.ColumnIndex].Name == "ParamColumn")
              {
                if (this.newObject != null && e.RowIndex == this.objectsList.Count - 1)
                  ((CmpParam) this.objectsList[e.RowIndex]).Param_id = (int) Convert.ToInt16(e.Value);
              }
              else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dbeg")
              {
                if (this.isPast && Convert.ToDateTime(e.Value) < Convert.ToDateTime("01.01.2005"))
                {
                  int num2 = (int) MessageBox.Show("Дата начала должна быть больше 01.01.2005 г.", "Внимание!", MessageBoxButtons.OK);
                  this.isEdit = false;
                  return;
                }
                if (this.isPast && this.closedPeriod.HasValue && Convert.ToDateTime(e.Value) >= this.closedPeriod.Value.AddMonths(1))
                {
                  int num2 = (int) MessageBox.Show("Дата начала не может быть больше даты окончания закрытого периода!", "Внимание!", MessageBoxButtons.OK);
                  this.isEdit = false;
                  return;
                }
                try
                {
                  ((CmpParam) this.objectsList[e.RowIndex]).Dbeg = Convert.ToDateTime(e.Value);
                }
                catch (Exception ex)
                {
                  int num2 = (int) MessageBox.Show("Проверьте правильность ввода даты!", "Внимание!", MessageBoxButtons.OK);
                  this.isEdit = false;
                  return;
                }
              }
              else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dend")
              {
                try
                {
                  if (!this.isPast)
                  {
                    DateTime dateTime1 = Convert.ToDateTime(e.Value);
                    DateTime dateTime2 = this.closedPeriod.Value.AddMonths(1);
                    DateTime dateTime3 = dateTime2.AddDays(-1.0);
                    if (dateTime1 < dateTime3)
                    {
                      int num2 = (int) MessageBox.Show("Дата окончания не может быть больше даты окончания закрытого периода!", "Внимание!", MessageBoxButtons.OK);
                      this.isEdit = false;
                      return;
                    }
                    if (this.newObject == null)
                    {
                      DateTime dend = ((CmpParam) this.objectsList[e.RowIndex]).Dend;
                      dateTime2 = this.closedPeriod.Value;
                      dateTime2 = dateTime2.AddMonths(1);
                      DateTime dateTime4 = dateTime2.AddDays(-1.0);
                      if (dend < dateTime4)
                      {
                        int num2 = (int) MessageBox.Show("Дата окончания принадлежит закрытому периоду и не подлежит изменению!", "Внимание!", MessageBoxButtons.OK);
                        this.isEdit = false;
                        return;
                      }
                    }
                    ((CmpParam) this.objectsList[e.RowIndex]).Dend = Convert.ToDateTime(e.Value);
                  }
                  else
                  {
                    if (Convert.ToDateTime(e.Value) >= this.closedPeriod.Value.AddMonths(1))
                    {
                      int num2 = (int) MessageBox.Show("Дата окончания не может принадлежать открытому периоду", "Внимание!", MessageBoxButtons.OK);
                      this.isEdit = false;
                      return;
                    }
                    if (newObject == null);
                    ((CmpParam) this.objectsList[e.RowIndex]).Dend = Convert.ToDateTime(e.Value);
                  }
                }
                catch (Exception ex)
                {
                  int num2 = (int) MessageBox.Show("Проверьте правильность ввода даты!", "Внимание!", MessageBoxButtons.OK);
                  this.isEdit = false;
                  return;
                }
              }
              else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Param_value")
              {
                if (e.Value == null || e.Value.ToString() == "")
                  ((CmpParam) this.objectsList[e.RowIndex]).Param_value = new double?();
                else
                  ((CmpParam) this.objectsList[e.RowIndex]).Param_value = new double?(Convert.ToDouble(KvrplHelper.ChangeSeparator(e.Value.ToString())));
              }
              this.curObject = this.objectsList[e.RowIndex];
              if (this.curObject != null)
              {
                ((CmpParam) this.curObject).Uname = Options.Login;
                ((CmpParam) this.curObject).Dedit = DateTime.Now;
              }
            }
            catch
            {
              int num2 = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
              this.isEdit = false;
              return;
            }
          }
          this.isEdit = true;
        }
        else
        {
          int num2 = (int) MessageBox.Show("Попытка внести изменения в закрытом периоде!", "Внимание!", MessageBoxButtons.OK);
          this.isEdit = false;
        }
      }
    }

    protected override void tsbAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, this.session.Get<Company>((object) this.Company_id), true))
        return;
      this.newObject = (object) new CmpParam();
      if (this.closedPeriod.HasValue)
      {
        if (this.isPast)
          ((CmpParam) this.newObject).Dbeg = this.closedPeriod.Value;
        else
          ((CmpParam) this.newObject).Dbeg = this.closedPeriod.Value.AddMonths(1);
      }
      else
        ((CmpParam) this.newObject).Dbeg = DateTime.Now;
      if (!this.isPast)
        ((CmpParam) this.newObject).Period = this.session.Get<Period>((object) Convert.ToInt32(this.session.CreateQuery("select p.PeriodId from Period p where PeriodName is null").UniqueResult()));
      else if (this.closedPeriod.HasValue)
      {
        ((CmpParam) this.newObject).Period = (Period) this.session.CreateQuery("from Period p where p.PeriodName=:dt").SetDateTime("dt", this.closedPeriod.Value.AddMonths(1)).UniqueResult();
      }
      else
      {
        int num = (int) MessageBox.Show("Отсутствует закрытый период!", "Внимание!", MessageBoxButtons.OK);
        return;
      }
      ((CmpParam) this.newObject).Company_id = (int) this.Company_id;
      if (this.closedPeriod.HasValue && this.isPast)
        ((CmpParam) this.newObject).Dend = this.closedPeriod.Value.AddMonths(1).AddDays(-1.0);
      else
        ((CmpParam) this.newObject).Dend = Convert.ToDateTime("31.12.2999");
      base.tsbAdd_Click(sender, e);
      this.dt = new DateTime?(((CmpParam) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg);
    }

    protected override void tsbApplay_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, this.session.Get<Company>((object) this.Company_id), true))
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
          if (this.closedPeriod.HasValue && ((CmpParam) this.newObject).Dbeg < this.closedPeriod.Value.AddMonths(1) && !this.isPast)
          {
            int num1 = (int) MessageBox.Show("Дата начала принадлежит закрытому периоду!", "Внимание!", MessageBoxButtons.OK);
          }
          else
          {
            if (((CmpParam) this.newObject).Param_id == 0)
            {
              int num2 = (int) MessageBox.Show("Выберите параметр!", "Внимание!", MessageBoxButtons.OK);
              return;
            }
            if (((CmpParam) this.newObject).Dbeg <= DateTime.Now.AddYears(-3) && MessageBox.Show("Дата начала отличается от текущей более, чем на 3 года. Продолжить", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
              return;
            if (this.isPast && (((CmpParam) this.newObject).Param_id == 201 || ((CmpParam) this.newObject).Param_id == 202 || (((CmpParam) this.newObject).Param_id == 203 || ((CmpParam) this.newObject).Param_id == 204) || (((CmpParam) this.newObject).Param_id == 206 || ((CmpParam) this.newObject).Param_id == 207 || (((CmpParam) this.newObject).Param_id == 208 || ((CmpParam) this.newObject).Param_id == 210)) || (((CmpParam) this.newObject).Param_id == 211 || ((CmpParam) this.newObject).Param_id == 216 || (((CmpParam) this.newObject).Param_id == 217 || ((CmpParam) this.newObject).Param_id == 218) || (((CmpParam) this.newObject).Param_id == 219 || ((CmpParam) this.newObject).Param_id == 224 || (((CmpParam) this.newObject).Param_id == 225 || ((CmpParam) this.newObject).Param_id == 227))) || ((CmpParam) this.newObject).Param_id == 228))
            {
              int num2 = (int) MessageBox.Show("Этот параметр нельзя менять в прошлом времени!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            if (((CmpParam) this.newObject).Dbeg.Day != 1)
            {
              int num2 = (int) MessageBox.Show("Дата начала действия должно быть первое число месяца!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            if (this.isPast && ((CmpParam) this.newObject).Param_id == 226 && (((CmpParam) this.newObject).Dbeg.Day != 1 || ((CmpParam) this.newObject).Dend.Day != KvrplHelper.LastDay(((CmpParam) this.newObject).Dend).Day))
            {
              int num2 = (int) MessageBox.Show("Дата начала действия должно быть первое число месяца!\nДата окончания последнее число месяца", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            if (((CmpParam) this.newObject).Param_id == 201 || ((CmpParam) this.newObject).Param_id == 202 || (((CmpParam) this.newObject).Param_id == 203 || ((CmpParam) this.newObject).Param_id == 204) || (((CmpParam) this.newObject).Param_id == 210 || ((CmpParam) this.newObject).Param_id == 211 || (((CmpParam) this.newObject).Param_id == 213 || ((CmpParam) this.newObject).Param_id == 221)) || (((CmpParam) this.newObject).Param_id == 222 || ((CmpParam) this.newObject).Param_id == 227) || ((CmpParam) this.newObject).Param_id == 228)
            {
              ((CmpParam) this.newObject).Dbeg = KvrplHelper.FirstDay(((CmpParam) this.newObject).Dbeg);
              ((CmpParam) this.newObject).Dend = KvrplHelper.LastDay(((CmpParam) this.newObject).Dend);
            }
            if ((((CmpParam) this.newObject).Param_id == 203 || ((CmpParam) this.newObject).Param_id == 211 || ((CmpParam) this.newObject).Param_id == 223) && this.session.CreateQuery(string.Format("from CmpParam where Period_id=0 and Company_id={0} and Param_id={1} ", (object) this.Company_id, (object) ((CmpParam) this.newObject).Param_id.ToString())).List().Count > 0)
            {
              int num2 = (int) MessageBox.Show("Этот параметр не может быть внесен дважды", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            if (((CmpParam) this.newObject).Param_id == 223)
            {
              foreach (SndAddress sndAddress in (IEnumerable<SndAddress>) this.session.CreateQuery("from SndAddress where Account_Type=1 and Account in (select lc.ClientId from LsClient lc where lc.Company.CompanyId=:cl)").SetParameter<short>("cl", this.company_id).List<SndAddress>())
              {
                sndAddress.UploadDir = this.sndAddress.UploadDir + "\\" + (object) sndAddress.Account;
                this.session.SaveOrUpdate((object) sndAddress);
                this.session.Flush();
              }
            }
            ((CmpParam) this.newObject).Uname = Options.Login;
            ((CmpParam) this.newObject).Dedit = DateTime.Now;
          }
        }
        else if (!this.dt.Equals((object) ((CmpParam) this.curObject).Dbeg))
        {
          if (this.isPast && (((CmpParam) this.curObject).Param_id == 201 || ((CmpParam) this.curObject).Param_id == 202 || (((CmpParam) this.curObject).Param_id == 203 || ((CmpParam) this.curObject).Param_id == 204) || (((CmpParam) this.curObject).Param_id == 206 || ((CmpParam) this.curObject).Param_id == 207 || (((CmpParam) this.curObject).Param_id == 208 || ((CmpParam) this.curObject).Param_id == 210)) || (((CmpParam) this.curObject).Param_id == 211 || ((CmpParam) this.curObject).Param_id == 224 || (((CmpParam) this.curObject).Param_id == 225 || ((CmpParam) this.curObject).Param_id == 227)) || ((CmpParam) this.curObject).Param_id == 228))
          {
            int num2 = (int) MessageBox.Show("Этот параметр нельзя менять в прошлом времени!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          if (((CmpParam) this.newObject).Dbeg.Day != 1)
          {
            int num2 = (int) MessageBox.Show("Дата начала действия должно быть первое число месяца!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          DateTime dateTime1;
          if (this.isPast && ((CmpParam) this.newObject).Param_id == 226)
          {
            int num2;
            if (((CmpParam) this.newObject).Dbeg.Day == 1)
            {
              int day1 = ((CmpParam) this.newObject).Dend.Day;
              dateTime1 = KvrplHelper.LastDay(((CmpParam) this.newObject).Dend);
              int day2 = dateTime1.Day;
              num2 = day1 != day2 ? 1 : 0;
            }
            else
              num2 = 1;
            if (num2 != 0)
            {
              int num3 = (int) MessageBox.Show("Дата начала действия должно быть первое число месяца!\nДата окончания последнее число месяца", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
          }
          if (((CmpParam) this.curObject).Param_id == 201 || ((CmpParam) this.curObject).Param_id == 202 || (((CmpParam) this.curObject).Param_id == 203 || ((CmpParam) this.curObject).Param_id == 204) || (((CmpParam) this.curObject).Param_id == 210 || ((CmpParam) this.curObject).Param_id == 211 || (((CmpParam) this.curObject).Param_id == 213 || ((CmpParam) this.curObject).Param_id == 221)) || (((CmpParam) this.curObject).Param_id == 222 || ((CmpParam) this.curObject).Param_id == 227) || ((CmpParam) this.curObject).Param_id == 228)
          {
            ((CmpParam) this.curObject).Dbeg = KvrplHelper.FirstDay(((CmpParam) this.curObject).Dbeg);
            ((CmpParam) this.curObject).Dend = KvrplHelper.LastDay(((CmpParam) this.curObject).Dend);
          }
          DateTime dbeg1 = ((CmpParam) this.curObject).Dbeg;
          dateTime1 = DateTime.Now;
          DateTime dateTime2 = dateTime1.AddYears(-3);
          int num4;
          if (!(dbeg1 <= dateTime2))
          {
            DateTime dbeg2 = ((CmpParam) this.curObject).Dbeg;
            dateTime1 = DateTime.Now;
            DateTime dateTime3 = dateTime1.AddYears(3);
            num4 = dbeg2 >= dateTime3 ? 1 : 0;
          }
          else
            num4 = 1;
          if (num4 != 0 && MessageBox.Show("Дата начала отличается от текущей более, чем на 3 года. Продолжить", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            return;
          if (((CmpParam) this.curObject).Param_id == 223)
          {
            foreach (SndAddress sndAddress in (IEnumerable<SndAddress>) this.session.CreateQuery("from SndAddress where Account_Type=1 and Account in (select lc.ClientId from LsClient lc where lc.Company.CompanyId=:cl)").SetParameter<short>("cl", this.company_id).List<SndAddress>())
            {
              sndAddress.UploadDir = this.sndAddress.UploadDir + "\\" + (object) sndAddress.Account;
              this.session.SaveOrUpdate((object) sndAddress);
              this.session.Flush();
            }
          }
          using (ITransaction transaction = this.session.BeginTransaction())
          {
            try
            {
              this.session.CreateSQLQuery("update DBA.cmpParam cmpP set Dbeg=:dbeg,Dend=:dend,Param_value=:pv,Uname=:uname,Dedit=:dedit where cmpP.Company_id=:company_id and cmpP.Period_id=:period and cmpP.Dbeg=:dbeg2 and cmpP.Param_id=:param_id").SetDateTime("dbeg", ((CmpParam) this.curObject).Dbeg).SetParameter<DateTime>("dend", ((CmpParam) this.curObject).Dend).SetInt32("param_id", ((CmpParam) this.curObject).Param_id).SetParameter<double?>("pv", ((CmpParam) this.curObject).Param_value).SetDateTime("dbeg2", this.dt.Value).SetInt32("company_id", (int) this.Company_id).SetInt32("period", ((CmpParam) this.curObject).Period.PeriodId).SetDateTime("dedit", DateTime.Now).SetString("uname", Options.Login).ExecuteUpdate();
              transaction.Commit();
            }
            catch
            {
              int num2 = (int) MessageBox.Show("Не удалось внести изменения! Проверьте правильность ввода данных.", "Внимание!", MessageBoxButtons.OK);
              transaction.Rollback();
            }
          }
          this.CurIndex = -1;
          this.GetList();
        }
        base.tsbApplay_Click(sender, e);
        if (this.dgvBase.CurrentRow.Index > 0)
        {
          if (this.dgvBase.CurrentRow.Index < this.objectsList.Count - 1)
            this.session.Refresh(this.objectsList[this.dgvBase.CurrentRow.Index]);
          this.session.Refresh(this.objectsList[this.dgvBase.CurrentRow.Index - 1]);
        }
        if (this.dgvBase.CurrentRow != null)
          this.CurIndex = this.dgvBase.CurrentRow.Index;
        this.dgvBase.Refresh();
        this.dt = new DateTime?(((CmpParam) this.curObject).Dbeg);
      }
    }

    protected override void tsbCancel_Click(object sender, EventArgs e)
    {
      if (this.curObject != null && !this.dt.Equals((object) ((CmpParam) this.curObject).Dbeg))
        ((CmpParam) this.curObject).Dbeg = this.dt.Value;
      base.tsbCancel_Click(sender, e);
      if (this.dgvBase.CurrentRow != null)
        this.CurIndex = this.dgvBase.CurrentRow.Index;
      this.IsCopy = false;
    }

    protected override void tsbDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, this.session.Get<Company>((object) this.Company_id), true) || MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      if (this.objectsList.Count > 0)
      {
        int num1;
        if (this.closedPeriod.HasValue)
        {
          if (this.closedPeriod.Value.AddMonths(1) <= ((CmpParam) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg || this.isPast)
          {
            if (this.isPast)
            {
              if (((CmpParam) this.objectsList[this.dgvBase.CurrentRow.Index]).Period.PeriodName.HasValue)
              {
                DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
                DateTime? periodName = ((CmpParam) this.objectsList[this.dgvBase.CurrentRow.Index]).Period.PeriodName;
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
        this.dt = new DateTime?(((CmpParam) this.curObject).Dbeg);
        this.CurIndex = this.dgvBase.CurrentRow.Index;
      }
    }

    protected override void dgvBase_SelectionChanged(object sender, EventArgs e)
    {
      if (this.dgvBase.CurrentRow == null || this.dgvBase.CurrentRow.Index >= this.objectsList.Count)
        return;
      if (this.newObject == null && (this.CurIndex >= 0 && this.objectsList.Count > this.CurIndex && !this.dt.Equals((object) ((CmpParam) this.objectsList[this.CurIndex]).Dbeg) && this.CurIndex != this.dgvBase.CurrentRow.Index))
      {
        try
        {
          ((CmpParam) this.objectsList[this.CurIndex]).Dbeg = this.dt.Value;
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
        this.dt = new DateTime?(((CmpParam) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg);
        this.CurIndex = this.dgvBase.CurrentRow.Index;
        this.curObject = this.objectsList[this.dgvBase.CurrentRow.Index];
      }
    }

    protected override void tsmCopy_Click(object sender, EventArgs e)
    {
      this.isEdit = true;
      this.newObject = (object) new CmpParam();
      ((CmpParam) this.newObject).Copy((object) (CmpParam) this.curObject);
      DateTime dateTime1;
      int num;
      if (!this.isPast && this.closedPeriod.HasValue)
      {
        DateTime dbeg = ((CmpParam) this.newObject).Dbeg;
        dateTime1 = this.closedPeriod.Value;
        DateTime dateTime2 = dateTime1.AddMonths(1);
        num = dbeg < dateTime2 ? 1 : 0;
      }
      else
        num = 0;
      if (num != 0)
      {
        CmpParam newObject = (CmpParam) this.newObject;
        dateTime1 = this.closedPeriod.Value;
        DateTime dateTime2 = dateTime1.AddMonths(1);
        newObject.Dbeg = dateTime2;
      }
      else if (this.isPast)
      {
        ((CmpParam) this.newObject).Dbeg = this.closedPeriod.Value;
      }
      else
      {
        CmpParam newObject = (CmpParam) this.newObject;
        dateTime1 = ((CmpParam) this.newObject).Dbeg;
        dateTime1 = dateTime1.AddMonths(1);
        DateTime dateTime2 = dateTime1.AddDays((double) (-((CmpParam) this.newObject).Dbeg.Day + 1));
        newObject.Dbeg = dateTime2;
      }
      if (this.closedPeriod.HasValue && this.isPast)
      {
        CmpParam newObject = (CmpParam) this.newObject;
        dateTime1 = this.closedPeriod.Value;
        dateTime1 = dateTime1.AddMonths(1);
        DateTime dateTime2 = dateTime1.AddDays(-1.0);
        newObject.Dend = dateTime2;
      }
      base.tsbAdd_Click(sender, e);
      this.dt = new DateTime?(((CmpParam) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg);
      base.tsmCopy_Click(sender, e);
    }

    private void UCMSPPercent_CurObjectChanged(object sender, EventArgs e)
    {
    }

    protected override void tsmCopyInPast_Click(object sender, EventArgs e)
    {
      if (!this.isPast)
        this.IsCopy = true;
      this.newObject = (object) new CmpParam();
      ((CmpParam) this.newObject).Copy((object) (CmpParam) this.curObject);
      if (this.closedPeriod.HasValue)
      {
        ((CmpParam) this.newObject).Period = (Period) this.session.CreateQuery("from Period p where p.PeriodName=:dt").SetDateTime("dt", this.closedPeriod.Value.AddMonths(1)).UniqueResult();
        if (!this.isPast)
          this.IsPast = !this.isPast;
        DateTime dateTime1;
        int num;
        if (!this.isPast && this.closedPeriod.HasValue)
        {
          DateTime dbeg = ((CmpParam) this.newObject).Dbeg;
          dateTime1 = this.closedPeriod.Value;
          DateTime dateTime2 = dateTime1.AddMonths(1);
          num = dbeg < dateTime2 ? 1 : 0;
        }
        else
          num = 0;
        if (num != 0)
        {
          CmpParam newObject = (CmpParam) this.newObject;
          dateTime1 = this.closedPeriod.Value;
          DateTime dateTime2 = dateTime1.AddMonths(1);
          newObject.Dbeg = dateTime2;
        }
        else if (this.isPast)
          ((CmpParam) this.newObject).Dbeg = this.closedPeriod.Value;
        if (this.closedPeriod.HasValue && this.isPast)
        {
          CmpParam newObject = (CmpParam) this.newObject;
          dateTime1 = this.closedPeriod.Value;
          dateTime1 = dateTime1.AddMonths(1);
          DateTime dateTime2 = dateTime1.AddDays(-1.0);
          newObject.Dend = dateTime2;
        }
        base.tsbAdd_Click(sender, e);
        this.dt = new DateTime?(((CmpParam) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg);
        base.tsmCopyInPast_Click(sender, e);
      }
      else
      {
        int num1 = (int) MessageBox.Show("Отсутствует закрытый период!", "Внимание!", MessageBoxButtons.OK);
      }
    }

    private void dgvBase_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex > 0 && e.RowIndex >= 0 && this.dgvBase.Columns[e.ColumnIndex].Name == "Param_value")
      {
        try
        {
          if (this.tablesList.ContainsKey(((CmpParam) this.objectsList[e.RowIndex]).Param_id))
          {
            this.session = Domain.CurrentSession;
            AdmTbl tables = this.tablesList[((CmpParam) this.objectsList[e.RowIndex]).Param_id];
            DataGridViewComboBoxCell viewComboBoxCell = new DataGridViewComboBoxCell();
            viewComboBoxCell.DisplayStyleForCurrentCellOnly = true;
            string str1 = "";
            string str2;
            if (((CmpParam) this.objectsList[e.RowIndex]).Param_id == 223)
            {
              viewComboBoxCell.ValueMember = "SndAddressId";
              viewComboBoxCell.DisplayMember = "UploadDir";
              str1 = " where Account=" + (object) this.company_id + " and Account_Type=0 ";
              str2 = "UploadDir";
            }
            else
            {
              viewComboBoxCell.ValueMember = tables.ClassNameId;
              viewComboBoxCell.DisplayMember = tables.ClassNameName;
              str2 = tables.ClassNameName;
            }
            if (((CmpParam) this.objectsList[e.RowIndex]).Param_id == 214)
              str1 = " where SchemeType=8";
            if (((CmpParam) this.objectsList[e.RowIndex]).Param_id == 224 || ((CmpParam) this.objectsList[e.RowIndex]).Param_id == 225)
              str1 = " where SchemeType=16";
            if (((CmpParam) this.objectsList[e.RowIndex]).Param_id == 227 || ((CmpParam) this.objectsList[e.RowIndex]).Param_id == 228)
              str1 = " where SchemeType=18";
            this.valueList = this.session.CreateQuery("from " + tables.ClassName + str1 + " order by " + str2).List();
            if (((CmpParam) this.objectsList[e.RowIndex]).Param_id == 223 && this.valueList.Count == 0)
              this.valueList.Insert(0, (object) new SndAddress()
              {
                SndAddressId = 0,
                UploadDir = "..."
              });
            viewComboBoxCell.DataSource = (object) this.valueList;
            viewComboBoxCell.ValueType = typeof (short);
            this.dgvBase.Rows[e.RowIndex].Cells["Param_value"] = (DataGridViewCell) viewComboBoxCell;
          }
          if (((CmpParam) this.objectsList[e.RowIndex]).Param_id == 223)
          {
            this.toolStrip1.Items["btnPath"].Visible = true;
            this.sndAddress = this.session.CreateQuery("from SndAddress s where s.Account=:cl and Account_Type=0").SetParameter<short>("cl", this.company_id).UniqueResult<SndAddress>();
          }
          else
            this.toolStrip1.Items["btnPath"].Visible = false;
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      else
        this.toolStrip1.Items["btnPath"].Visible = false;
    }

    private void UCCmpParam_Load(object sender, EventArgs e)
    {
      IList list = this.session.CreateQuery("from ParamRelation r, AdmTbl s  where s.TableId=r.TableId order by r.ParamId").List();
      this.tablesList.Clear();
      foreach (object[] objArray in (IEnumerable) list)
        this.tablesList.Add((int) ((ParamRelation) objArray[0]).ParamId, (AdmTbl) objArray[1]);
      ToolStripButton toolStripButton = new ToolStripButton("Путь");
      toolStripButton.Visible = false;
      toolStripButton.Name = "btnPath";
      toolStripButton.Click += new EventHandler(this.BtnOnClick);
      this.toolStrip1.Items.Add((ToolStripItem) toolStripButton);
    }

    private void BtnOnClick(object sender, EventArgs eventArgs)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      this.sndAddress = this.session.CreateQuery("from SndAddress s where s.Account=:cl and Account_Type=0").SetParameter<short>("cl", this.company_id).UniqueResult<SndAddress>();
      if (this.sndAddress == null)
      {
        this.sndAddress = new SndAddress();
        this.sndAddress.Account = (int) this.Company_id;
        this.sndAddress.Account_Type = 0;
        this.sndAddress.DownloadDir = "";
        this.sndAddress.FTPAdress = "";
        this.sndAddress.Active = 1;
        this.sndAddress.EmailAdress = "";
        this.sndAddress.SndAddressId = this.session.CreateSQLQuery("Select DBA.Gen_id('SndAddress',1)").UniqueResult<int>();
      }
      else
        folderBrowserDialog.SelectedPath = this.sndAddress.UploadDir;
      int num = (int) folderBrowserDialog.ShowDialog();
      this.sndAddress.UploadDir = folderBrowserDialog.SelectedPath;
      this.session.SaveOrUpdate((object) this.sndAddress);
      this.session.Flush();
      if (this.tablesList.ContainsKey(((CmpParam) this.objectsList[this.dgvBase.SelectedCells[0].RowIndex]).Param_id))
      {
        this.session = Domain.CurrentSession;
        AdmTbl tables = this.tablesList[((CmpParam) this.objectsList[this.dgvBase.SelectedCells[0].RowIndex]).Param_id];
        DataGridViewComboBoxCell viewComboBoxCell = new DataGridViewComboBoxCell();
        viewComboBoxCell.DisplayStyleForCurrentCellOnly = true;
        string str1 = "";
        string str2;
        if (((CmpParam) this.objectsList[this.dgvBase.SelectedCells[0].RowIndex]).Param_id == 223)
        {
          viewComboBoxCell.ValueMember = "SndAddressId";
          viewComboBoxCell.DisplayMember = "UploadDir";
          str1 = " where Account=" + (object) this.company_id + " and Account_Type=0 ";
          str2 = "UploadDir";
        }
        else
        {
          viewComboBoxCell.ValueMember = tables.ClassNameId;
          viewComboBoxCell.DisplayMember = tables.ClassNameName;
          str2 = tables.ClassNameName;
        }
        if (((CmpParam) this.objectsList[this.dgvBase.SelectedCells[0].RowIndex]).Param_id == 214)
          str1 = " where SchemeType=8";
        if (((CmpParam) this.objectsList[this.dgvBase.SelectedCells[0].RowIndex]).Param_id == 224 || ((CmpParam) this.objectsList[this.dgvBase.SelectedCells[0].RowIndex]).Param_id == 225)
          str1 = " where SchemeType=16";
        if (((CmpParam) this.objectsList[this.dgvBase.SelectedCells[0].RowIndex]).Param_id == 227 || ((CmpParam) this.objectsList[this.dgvBase.SelectedCells[0].RowIndex]).Param_id == 228)
          str1 = " where SchemeType=18";
        this.valueList = this.session.CreateQuery("from " + tables.ClassName + str1 + " order by " + str2).List();
        if (((CmpParam) this.objectsList[this.dgvBase.SelectedCells[0].RowIndex]).Param_id != 223)
          ;
        viewComboBoxCell.DataSource = (object) this.valueList;
        viewComboBoxCell.ValueType = typeof (short);
        this.dgvBase.Rows[this.dgvBase.SelectedCells[0].RowIndex].Cells["Param_value"] = (DataGridViewCell) viewComboBoxCell;
      }
      if (this.newObject != null)
        ((CmpParam) this.newObject).Param_value = new double?((double) this.sndAddress.SndAddressId);
      else
        ((CmpParam) this.curObject).Param_value = new double?((double) this.sndAddress.SndAddressId);
      this.tsbApplay_Click((object) null, (EventArgs) null);
      foreach (SndAddress sndAddress in (IEnumerable<SndAddress>) this.session.CreateQuery("from SndAddress where Account_Type=1 and Account in (select lc.ClientId from LsClient lc where lc.Company.CompanyId=:cl)").SetParameter<short>("cl", this.company_id).List<SndAddress>())
      {
        sndAddress.UploadDir = this.sndAddress.UploadDir + "\\" + (object) sndAddress.Account;
        this.session.SaveOrUpdate((object) sndAddress);
        this.session.Flush();
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
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.Name = "UCCmpParam";
      this.Load += new EventHandler(this.UCCmpParam_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
