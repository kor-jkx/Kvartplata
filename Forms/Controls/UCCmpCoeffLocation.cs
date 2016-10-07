// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.Controls.UCCmpCoeffLocation
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using NHibernate;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms.Controls
{
  public class UCCmpCoeffLocation : UCBase
  {
    private bool isPast = false;
    public bool IsCopy = false;
    private bool isEdit = false;
    private IContainer components = (IContainer) null;
    public DateTime? closedPeriod;
    private short company_id;
    private bool isArchive;

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
          this.dt = new DateTime?(((CmpCoeffLocation) this.objectsList[0]).DBeg);
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
          this.dt = new DateTime?(((CmpCoeffLocation) this.objectsList[0]).DBeg);
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

    public UCCmpCoeffLocation()
    {
      this.InitializeComponent();
      DataGridViewColumn dataGridViewColumn1 = (DataGridViewColumn) new MaskDateColumn();
      dataGridViewColumn1.Name = "Period";
      dataGridViewColumn1.HeaderText = "Период";
      dataGridViewColumn1.Visible = false;
      dataGridViewColumn1.ReadOnly = true;
      this.dgvBase.Columns.Add(dataGridViewColumn1);
      DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn2.Name = "CntrLocation";
      dataGridViewColumn2.HeaderText = "Тип расположения счетчика";
      ((DataGridViewComboBoxColumn) dataGridViewColumn2).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      ((DataGridViewComboBoxColumn) dataGridViewColumn2).DisplayMember = "CntrLocationName";
      ((DataGridViewComboBoxColumn) dataGridViewColumn2).ValueMember = "CntrLocationId";
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
      dataGridViewColumn5.Name = "Coeff";
      dataGridViewColumn5.HeaderText = "Коэффициент";
      dataGridViewColumn5.DefaultCellStyle = new DataGridViewCellStyle()
      {
        Format = "N5"
      };
      this.dgvBase.Columns.Add(dataGridViewColumn5);
      if (Options.ViewEdit)
      {
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 5, "Пользователь", "UName", 100, true);
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 6, "Дата редактирования", "DEdit", 100, true);
      }
      this.MySettings.GridName = "CmpCoeffLocation";
    }

    protected override void GetList()
    {
      if (this.session != null && this.session.IsOpen)
        this.session.Clear();
      this.objectsList.Clear();
      if (this.session == null)
        return;
      if (this.closedPeriod.HasValue && !this.isArchive)
      {
        if (this.isPast)
          this.objectsList = this.session.CreateQuery(string.Format("from CmpCoeffLocation where Period.PeriodId<>{0} and CompanyId={1} and CntrLocation.CntrLocationId in (select CntrLocationId from CounterLocation) and period_id=:id order by Period.PeriodId,CntrLocation.CntrLocationId,dbeg", (object) 0, (object) this.Company_id)).SetInt32("id", ((Period) this.session.CreateQuery("from Period where PeriodName=:value").SetDateTime("value", this.closedPeriod.Value.AddMonths(1)).UniqueResult()).PeriodId).List();
        else
          this.objectsList = this.session.CreateQuery(string.Format("from CmpCoeffLocation where Period.PeriodId={0} and CompanyId={1} and CntrLocation.CntrLocationId in (select CntrLocationId from CounterLocation) and dend>=:dend order by CntrLocation.CntrLocationId,dbeg", (object) 0, (object) this.Company_id)).SetDateTime("dend", this.closedPeriod.Value.AddMonths(1)).List();
      }
      else if (this.isPast)
        this.objectsList = this.session.CreateQuery(string.Format("from CmpCoeffLocation where Period.PeriodId<>{0} and CompanyId={1} and CntrLocation.CntrLocationId in (select CntrLocationId from CounterLocation) order by Period.PeriodId,CntrLocation.CntrLocationId,dbeg", (object) 0, (object) this.Company_id)).List();
      else
        this.objectsList = this.session.CreateQuery(string.Format("select c from CmpCoeffLocation c where c.Period.PeriodId={0} and c.CompanyId={1} and  c.CntrLocation.CntrLocationId in (select CntrLocationId from CounterLocation) order by c.CntrLocation.CntrLocationId,c.DBeg", (object) 0, (object) Convert.ToInt16(this.Company_id))).List();
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
        DateTime? nullable;
        int num;
        if (!this.isPast && ((CmpCoeffLocation) this.objectsList[e.RowIndex]).DBeg < this.closedPeriod.Value.AddMonths(2))
        {
          nullable = ((CmpCoeffLocation) this.objectsList[e.RowIndex]).Dend;
          DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
          if ((nullable.HasValue ? (nullable.GetValueOrDefault() >= dateTime ? 1 : 0) : 0) != 0)
          {
            num = 1;
            goto label_9;
          }
        }
        if (this.isPast)
        {
          nullable = ((CmpCoeffLocation) this.objectsList[e.RowIndex]).Period.PeriodName;
          DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
          num = nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() == dateTime ? 1 : 0) : 1) : 0;
        }
        else
          num = 0;
label_9:
        if (num != 0)
          this.dgvBase.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.PapayaWhip;
        else
          this.dgvBase.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      }
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "CntrLocation")
        e.Value = (object) ((CmpCoeffLocation) this.objectsList[e.RowIndex]).CntrLocation.CntrLocationId;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dbeg")
        e.Value = (object) ((CmpCoeffLocation) this.objectsList[e.RowIndex]).DBeg;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dend")
        e.Value = (object) ((CmpCoeffLocation) this.objectsList[e.RowIndex]).Dend;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Coeff")
        e.Value = (object) ((CmpCoeffLocation) this.objectsList[e.RowIndex]).Coeff;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Period" && this.isPast)
        e.Value = (object) ((CmpCoeffLocation) this.objectsList[e.RowIndex]).Period.PeriodName;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "UName")
        e.Value = (object) ((CmpCoeffLocation) this.objectsList[e.RowIndex]).Uname;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "DEdit")
        e.Value = (object) ((CmpCoeffLocation) this.objectsList[e.RowIndex]).Dedit.ToShortDateString();
    }

    protected override void dgvBase_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      if (!this.isPast && this.dgvBase.Columns[e.ColumnIndex].Name == "Dbeg" && this.closedPeriod.Value.AddMonths(1) <= ((CmpCoeffLocation) this.objectsList[e.RowIndex]).DBeg && this.closedPeriod.Value.AddMonths(1) > Convert.ToDateTime(e.Value))
      {
        int num = (int) MessageBox.Show("Попытка изменить дату начала на дату, принадлежащую закрытому периоду!", "Внимание!", MessageBoxButtons.OK);
        this.isEdit = false;
      }
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dend" && ((CmpCoeffLocation) this.objectsList[e.RowIndex]).DBeg > Convert.ToDateTime(e.Value))
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
          nullable = ((CmpCoeffLocation) this.objectsList[e.RowIndex]).Dend;
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
        else
        {
          int num2;
          if (e.RowIndex < 0 || this.closedPeriod.HasValue)
          {
            if (this.closedPeriod.Value.AddMonths(1) <= ((CmpCoeffLocation) this.objectsList[e.RowIndex]).DBeg || this.isPast)
            {
              if (this.isPast)
              {
                nullable = ((CmpCoeffLocation) this.objectsList[e.RowIndex]).Period.PeriodName;
                if (nullable.HasValue)
                {
                  DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
                  nullable = ((CmpCoeffLocation) this.objectsList[e.RowIndex]).Period.PeriodName;
                  if ((nullable.HasValue ? (dateTime <= nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0)
                    goto label_16;
                }
              }
              else
                goto label_16;
            }
            num2 = this.dgvBase.Columns[e.ColumnIndex].Name == "Dend" ? 1 : 0;
            goto label_17;
          }
label_16:
          num2 = 1;
label_17:
          if (num2 != 0)
          {
            if (e.RowIndex >= 0)
            {
              if (this.dgvBase.Columns[e.ColumnIndex].Name == "CntrLocation")
              {
                if (this.newObject != null && e.RowIndex == this.objectsList.Count - 1)
                {
                  ((CmpCoeffLocation) this.objectsList[e.RowIndex]).CntrLocation.CntrLocationId = Convert.ToInt32(e.Value);
                  this.dgvBase.Refresh();
                }
              }
              else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dbeg")
              {
                if (this.isPast && Convert.ToDateTime(e.Value) < Convert.ToDateTime("01.01.2005"))
                {
                  int num3 = (int) MessageBox.Show("Дата начала должна быть больше 01.01.2005 г.", "Внимание!", MessageBoxButtons.OK);
                  this.isEdit = false;
                  return;
                }
                if (this.isPast && this.closedPeriod.HasValue && Convert.ToDateTime(e.Value) >= this.closedPeriod.Value.AddMonths(1))
                {
                  int num3 = (int) MessageBox.Show("Дата начала не может быть больше даты окончания закрытого периода!", "Внимание!", MessageBoxButtons.OK);
                  this.isEdit = false;
                  return;
                }
                try
                {
                  ((CmpCoeffLocation) this.objectsList[e.RowIndex]).DBeg = Convert.ToDateTime(e.Value);
                }
                catch (Exception ex)
                {
                  int num3 = (int) MessageBox.Show("Проверьте правильность ввода даты!", "Внимание!", MessageBoxButtons.OK);
                  this.isEdit = false;
                  return;
                }
              }
              else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dend")
              {
                try
                {
                  DateTime dateTime1;
                  int num3;
                  if (this.closedPeriod.HasValue && !this.isPast)
                  {
                    DateTime dateTime2 = Convert.ToDateTime(e.Value);
                    dateTime1 = this.closedPeriod.Value;
                    DateTime dateTime3 = dateTime1.AddMonths(1);
                    if (dateTime2 >= dateTime3)
                    {
                      if (e.RowIndex < this.objectsList.Count - 1)
                      {
                        DateTime dbeg = ((CmpCoeffLocation) this.objectsList[e.RowIndex + 1]).DBeg;
                        dateTime1 = this.closedPeriod.Value;
                        DateTime dateTime4 = dateTime1.AddMonths(1);
                        if (dbeg >= dateTime4)
                        {
                          num3 = 1;
                          goto label_38;
                        }
                      }
                      num3 = e.RowIndex == this.objectsList.Count - 1 ? 1 : 0;
                    }
                    else
                      num3 = 0;
                  }
                  else
                    num3 = 1;
label_38:
                  if (num3 != 0)
                  {
                    int num4;
                    if (this.isPast && this.closedPeriod.HasValue)
                    {
                      DateTime dateTime2 = Convert.ToDateTime(e.Value);
                      dateTime1 = this.closedPeriod.Value;
                      DateTime dateTime3 = dateTime1.AddMonths(1);
                      num4 = dateTime2 >= dateTime3 ? 1 : 0;
                    }
                    else
                      num4 = 0;
                    if (num4 != 0)
                    {
                      int num5 = (int) MessageBox.Show("Дата окончания не может быть больше даты окончания закрытого периода!", "Внимание!", MessageBoxButtons.OK);
                      this.isEdit = false;
                      return;
                    }
                    ((CmpCoeffLocation) this.objectsList[e.RowIndex]).Dend = new DateTime?(Convert.ToDateTime(e.Value));
                  }
                  else
                  {
                    int num4 = (int) MessageBox.Show("Дата окончания принадлежит закрытому периоду, либо существует запись, принадлежащая закрытому периоду, которая перекрывается этой датой!", "Внимание!", MessageBoxButtons.OK);
                    this.isEdit = false;
                    return;
                  }
                }
                catch (Exception ex)
                {
                  int num3 = (int) MessageBox.Show("Проверьте правильность ввода даты!", "Внимание!", MessageBoxButtons.OK);
                  this.isEdit = false;
                  return;
                }
              }
              else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Coeff" && e.Value != null)
              {
                try
                {
                  ((CmpCoeffLocation) this.objectsList[e.RowIndex]).Coeff = Convert.ToDouble(KvrplHelper.ChangeSeparator(e.Value.ToString()));
                }
                catch
                {
                  int num3 = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
                  this.isEdit = false;
                  return;
                }
              }
              this.curObject = this.objectsList[e.RowIndex];
              if (this.newObject == null && this.curObject != null)
              {
                ((CmpCoeffLocation) this.curObject).Uname = Options.Login;
                ((CmpCoeffLocation) this.curObject).Dedit = DateTime.Now;
              }
            }
            this.isEdit = true;
          }
          else
          {
            int num3 = (int) MessageBox.Show("Попытка внести изменения в закрытом периоде!", "Внимание!", MessageBoxButtons.OK);
            this.isEdit = false;
          }
        }
      }
    }

    protected override void tsbAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, this.session.Get<Company>((object) this.company_id), true))
        return;
      this.newObject = (object) new CmpCoeffLocation();
      ((CmpCoeffLocation) this.newObject).CntrLocation = new CounterLocation()
      {
        CntrLocationId = int.MaxValue
      };
      if (this.closedPeriod.HasValue)
      {
        if (this.isPast)
          ((CmpCoeffLocation) this.newObject).DBeg = this.closedPeriod.Value;
        else
          ((CmpCoeffLocation) this.newObject).DBeg = this.closedPeriod.Value.AddMonths(1);
      }
      else
        ((CmpCoeffLocation) this.newObject).DBeg = KvrplHelper.FirstDay(DateTime.Now);
      if (!this.isPast)
        ((CmpCoeffLocation) this.newObject).Period = this.session.Get<Period>((object) Convert.ToInt32(this.session.CreateQuery("select p.PeriodId from Period p where PeriodName is null").UniqueResult()));
      else if (this.closedPeriod.HasValue)
      {
        ((CmpCoeffLocation) this.newObject).Period = (Period) this.session.CreateQuery("from Period p where p.PeriodName=:dt").SetDateTime("dt", this.closedPeriod.Value.AddMonths(1)).UniqueResult();
      }
      else
      {
        int num = (int) MessageBox.Show("Отсутствует закрытый период!", "Внимание!", MessageBoxButtons.OK);
        return;
      }
      ((CmpCoeffLocation) this.newObject).CompanyId = (int) this.Company_id;
      if (this.closedPeriod.HasValue && this.isPast)
        ((CmpCoeffLocation) this.newObject).Dend = new DateTime?(this.closedPeriod.Value.AddMonths(1).AddDays(-1.0));
      else
        ((CmpCoeffLocation) this.newObject).Dend = new DateTime?(Convert.ToDateTime("31.12.2999"));
      base.tsbAdd_Click(sender, e);
      this.dt = new DateTime?(((CmpCoeffLocation) this.objectsList[this.dgvBase.CurrentRow.Index]).DBeg);
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
          if (this.closedPeriod.HasValue && ((CmpCoeffLocation) this.newObject).DBeg < this.closedPeriod.Value.AddMonths(1) && !this.isPast)
          {
            int num1 = (int) MessageBox.Show("Дата начала принадлежит закрытому периоду!", "Внимание!", MessageBoxButtons.OK);
          }
          else
          {
            if (((CmpCoeffLocation) this.newObject).CntrLocation.CntrLocationId == int.MaxValue)
            {
              int num2 = (int) MessageBox.Show("Выберите тип!", "Внимание!", MessageBoxButtons.OK);
              return;
            }
            if (((CmpCoeffLocation) this.newObject).DBeg.Day != 1 || ((CmpCoeffLocation) this.newObject).Dend.Value.Day != KvrplHelper.LastDay(((CmpCoeffLocation) this.newObject).Dend.Value).Day)
            {
              int num2 = (int) MessageBox.Show("Даты начала и окончания действия должны быть первым и посленим числами месяца!", "Внимание!", MessageBoxButtons.OK);
              return;
            }
            if (((CmpCoeffLocation) this.newObject).DBeg <= DateTime.Now.AddYears(-3) && MessageBox.Show("Дата начала отличается от текущей более, чем на 3 года. Продолжить", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
              return;
            ((CmpCoeffLocation) this.newObject).Uname = Options.Login;
            ((CmpCoeffLocation) this.newObject).Dedit = DateTime.Now;
          }
        }
        else
        {
          DateTime? dend;
          int num2;
          if (((CmpCoeffLocation) this.curObject).DBeg.Day == 1)
          {
            int day1 = ((CmpCoeffLocation) this.curObject).Dend.Value.Day;
            dend = ((CmpCoeffLocation) this.curObject).Dend;
            int day2 = KvrplHelper.LastDay(dend.Value).Day;
            num2 = day1 != day2 ? 1 : 0;
          }
          else
            num2 = 1;
          if (num2 != 0)
          {
            int num3 = (int) MessageBox.Show("Даты начала и окончания действия должны быть первым и посленим числами месяца!", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          if ((((CmpCoeffLocation) this.curObject).DBeg <= DateTime.Now.AddYears(-3) || ((CmpCoeffLocation) this.curObject).DBeg >= DateTime.Now.AddYears(3)) && MessageBox.Show("Дата начала отличается от текущей более, чем на 3 года. Продолжить", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            return;
          if (!this.dt.Equals((object) ((CmpCoeffLocation) this.curObject).DBeg))
          {
            int num3;
            if (((CmpCoeffLocation) this.curObject).DBeg.Day == 1)
            {
              dend = ((CmpCoeffLocation) this.curObject).Dend;
              int day1 = dend.Value.Day;
              dend = ((CmpCoeffLocation) this.curObject).Dend;
              int day2 = KvrplHelper.LastDay(dend.Value).Day;
              num3 = day1 != day2 ? 1 : 0;
            }
            else
              num3 = 1;
            if (num3 != 0)
            {
              int num4 = (int) MessageBox.Show("Даты начала и окончания действия должны быть первым и посленим числами месяца!", "Внимание!", MessageBoxButtons.OK);
              return;
            }
            using (ITransaction transaction = this.session.BeginTransaction())
            {
              try
              {
                this.session.CreateSQLQuery("update DBA.CmpCoeffLocation cmpN set Dbeg=:dbeg,Dend=:dend,Coeff=:nv,Uname=:uname,Dedit=:dedit where cmpN.Company_id=:company_id and cmpN.Period_id=:period and cmpN.Dbeg=:dbeg2 and cmpN.CntrLocation_id=:cntrLocation_id").SetDateTime("dbeg", ((CmpCoeffLocation) this.curObject).DBeg).SetParameter<DateTime?>("dend", ((CmpCoeffLocation) this.curObject).Dend).SetParameter<double>("nv", ((CmpCoeffLocation) this.curObject).Coeff).SetDateTime("dbeg2", this.dt.Value).SetInt32("company_id", ((CmpCoeffLocation) this.curObject).CompanyId).SetInt32("period", ((CmpCoeffLocation) this.curObject).Period.PeriodId).SetInt32("cntrLocation_id", ((CmpCoeffLocation) this.curObject).CntrLocation.CntrLocationId).SetString("uname", Options.Login).SetDateTime("dedit", DateTime.Now).ExecuteUpdate();
                transaction.Commit();
                this.session.Flush();
              }
              catch
              {
                int num4 = (int) MessageBox.Show("Не удалось внести изменения! Проверьте правильность ввода данных.", "Внимание!", MessageBoxButtons.OK);
                transaction.Rollback();
                return;
              }
            }
            this.CurIndex = -1;
          }
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
        this.dt = new DateTime?(((CmpCoeffLocation) this.curObject).DBeg);
      }
    }

    protected override void tsbCancel_Click(object sender, EventArgs e)
    {
      if (this.curObject != null && !this.dt.Equals((object) ((CmpCoeffLocation) this.curObject).DBeg))
        ((CmpCoeffLocation) this.curObject).DBeg = this.dt.Value;
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
          if (this.closedPeriod.Value.AddMonths(1) <= ((CmpCoeffLocation) this.objectsList[this.dgvBase.CurrentRow.Index]).DBeg || this.isPast)
          {
            if (this.isPast)
            {
              if (((CmpCoeffLocation) this.objectsList[this.dgvBase.CurrentRow.Index]).Period.PeriodName.HasValue)
              {
                DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
                DateTime? periodName = ((CmpCoeffLocation) this.objectsList[this.dgvBase.CurrentRow.Index]).Period.PeriodName;
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
              ((CmpCoeffLocation) this.curObject).Period = this.session.Get<Period>((object) ((CmpCoeffLocation) this.objectsList[this.dgvBase.CurrentRow.Index]).Period.PeriodId);
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
        this.dt = new DateTime?(((CmpCoeffLocation) this.curObject).DBeg);
        this.CurIndex = this.dgvBase.CurrentRow.Index;
      }
    }

    protected override void dgvBase_SelectionChanged(object sender, EventArgs e)
    {
      if (this.dgvBase.CurrentRow == null || this.dgvBase.CurrentRow.Index >= this.objectsList.Count)
        return;
      if (this.newObject == null && (this.CurIndex >= 0 && this.objectsList.Count > this.CurIndex && !this.dt.Equals((object) ((CmpCoeffLocation) this.objectsList[this.CurIndex]).DBeg) && this.CurIndex != this.dgvBase.CurrentRow.Index))
      {
        try
        {
          ((CmpCoeffLocation) this.objectsList[this.CurIndex]).DBeg = this.dt.Value;
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
        this.dt = new DateTime?(((CmpCoeffLocation) this.objectsList[this.dgvBase.CurrentRow.Index]).DBeg);
        this.CurIndex = this.dgvBase.CurrentRow.Index;
        this.curObject = this.objectsList[this.dgvBase.CurrentRow.Index];
      }
    }

    protected override void tsmCopy_Click(object sender, EventArgs e)
    {
      this.newObject = (object) new CmpCoeffLocation();
      ((CmpCoeffLocation) this.newObject).Copy((CmpCoeffLocation) this.curObject);
      if (!this.isPast && this.closedPeriod.HasValue && ((CmpCoeffLocation) this.newObject).DBeg < this.closedPeriod.Value.AddMonths(1))
        ((CmpCoeffLocation) this.newObject).DBeg = this.closedPeriod.Value.AddMonths(1);
      else if (this.isPast)
        ((CmpCoeffLocation) this.newObject).DBeg = this.closedPeriod.Value;
      else
        ((CmpCoeffLocation) this.newObject).DBeg = ((CmpCoeffLocation) this.newObject).DBeg.AddMonths(1).AddDays((double) (-((CmpCoeffLocation) this.newObject).DBeg.Day + 1));
      if (this.closedPeriod.HasValue && this.isPast)
      {
        CmpCoeffLocation newObject = (CmpCoeffLocation) this.newObject;
        DateTime dateTime = this.closedPeriod.Value;
        dateTime = dateTime.AddMonths(1);
        DateTime? nullable = new DateTime?(dateTime.AddDays(-1.0));
        newObject.Dend = nullable;
      }
      base.tsbAdd_Click(sender, e);
      this.dt = new DateTime?(((CmpCoeffLocation) this.objectsList[this.dgvBase.CurrentRow.Index]).DBeg);
      base.tsmCopy_Click(sender, e);
    }

    protected override void tsmCopyInPast_Click(object sender, EventArgs e)
    {
      if (!this.isPast)
        this.IsCopy = true;
      this.newObject = (object) new CmpCoeffLocation();
      ((CmpCoeffLocation) this.newObject).Copy((CmpCoeffLocation) this.curObject);
      if (this.closedPeriod.HasValue)
      {
        ((CmpCoeffLocation) this.newObject).Period = (Period) this.session.CreateQuery("from Period p where p.PeriodName=:dt").SetDateTime("dt", this.closedPeriod.Value.AddMonths(1)).UniqueResult();
        if (!this.isPast)
          this.IsPast = !this.isPast;
        DateTime dateTime1;
        int num;
        if (!this.isPast && this.closedPeriod.HasValue)
        {
          DateTime dbeg = ((CmpCoeffLocation) this.newObject).DBeg;
          dateTime1 = this.closedPeriod.Value;
          DateTime dateTime2 = dateTime1.AddMonths(1);
          num = dbeg < dateTime2 ? 1 : 0;
        }
        else
          num = 0;
        if (num != 0)
        {
          CmpCoeffLocation newObject = (CmpCoeffLocation) this.newObject;
          dateTime1 = this.closedPeriod.Value;
          DateTime dateTime2 = dateTime1.AddMonths(1);
          newObject.DBeg = dateTime2;
        }
        else if (this.isPast)
          ((CmpCoeffLocation) this.newObject).DBeg = this.closedPeriod.Value;
        if (this.closedPeriod.HasValue && this.isPast)
        {
          CmpCoeffLocation newObject = (CmpCoeffLocation) this.newObject;
          dateTime1 = this.closedPeriod.Value;
          dateTime1 = dateTime1.AddMonths(1);
          DateTime? nullable = new DateTime?(dateTime1.AddDays(-1.0));
          newObject.Dend = nullable;
        }
        base.tsbAdd_Click(sender, e);
        this.dt = new DateTime?(((CmpCoeffLocation) this.objectsList[this.dgvBase.CurrentRow.Index]).DBeg);
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
