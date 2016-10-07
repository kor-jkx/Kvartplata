// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.Controls.UCMSPPercent
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
  public class UCMSPPercent : UCBase
  {
    private bool isPast = false;
    public bool IsCopy = false;
    private bool isEdit = false;
    private bool isArchive = false;
    private IContainer components = (IContainer) null;
    public DateTime? closedPeriod;
    private DcMSP curMSP;

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
              if (this.IsCopy)
              {
                this.IsCopy = false;
                if (((SdcMSPPercent) this.curObject).Uname != null)
                  this.newObject = (object) null;
              }
              this.tsbApplay_Click((object) null, (EventArgs) null);
              return;
            }
            this.tsbCancel_Click((object) null, (EventArgs) null);
            this.IsCopy = true;
          }
          this.isPast = value;
          this.dgvBase.Columns["periodColumn"].Visible = this.isPast;
          this.curObject = (object) null;
          this.GetList();
          if (this.objectsList.Count <= 0)
            return;
          this.CurIndex = 0;
          this.dt = new DateTime?(((SdcMSPPercent) this.objectsList[0]).Dbeg);
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

    public DcMSP CurMSP
    {
      get
      {
        return this.curMSP;
      }
      set
      {
        if (this.curMSP != value)
        {
          if (this.tsbApplay.Enabled)
          {
            if (MessageBox.Show("Изменения не сохранены! Вернуться к предыдущей категории льгои и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
              this.tsbApplay_Click((object) null, (EventArgs) null);
              return;
            }
            this.tsbCancel_Click((object) null, (EventArgs) null);
          }
          this.curMSP = value;
          this.curObject = (object) null;
          this.GetList();
          if (this.objectsList.Count <= 0)
            return;
          this.CurIndex = 0;
          this.dt = new DateTime?(((SdcMSPPercent) this.objectsList[0]).Dbeg);
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

    public UCMSPPercent()
    {
      this.InitializeComponent();
      DataGridViewColumn dataGridViewColumn1 = (DataGridViewColumn) new MaskDateColumn();
      dataGridViewColumn1.Name = "periodColumn";
      dataGridViewColumn1.HeaderText = "Период";
      dataGridViewColumn1.Visible = false;
      dataGridViewColumn1.ReadOnly = true;
      this.dgvBase.Columns.Add(dataGridViewColumn1);
      DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn2.Name = "serviceColumn";
      dataGridViewColumn2.HeaderText = "Услуга";
      ((DataGridViewComboBoxColumn) dataGridViewColumn2).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      ((DataGridViewComboBoxColumn) dataGridViewColumn2).DisplayMember = "ServiceName";
      ((DataGridViewComboBoxColumn) dataGridViewColumn2).ValueMember = "ServiceId";
      this.dgvBase.Columns.Add(dataGridViewColumn2);
      DataGridViewColumn dataGridViewColumn3 = (DataGridViewColumn) new MaskDateColumn();
      dataGridViewColumn3.Name = "dbegColumn";
      dataGridViewColumn3.HeaderText = "Дата начала действия";
      this.dgvBase.Columns.Add(dataGridViewColumn3);
      DataGridViewColumn dataGridViewColumn4 = (DataGridViewColumn) new MaskDateColumn();
      dataGridViewColumn4.Name = "dendColumn";
      dataGridViewColumn4.HeaderText = "Дата окончания действия";
      this.dgvBase.Columns.Add(dataGridViewColumn4);
      DataGridViewColumn dataGridViewColumn5 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn5.Name = "spreadingColumn";
      dataGridViewColumn5.HeaderText = "На кого распространяется";
      ((DataGridViewComboBoxColumn) dataGridViewColumn5).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      ((DataGridViewComboBoxColumn) dataGridViewColumn5).DisplayMember = "Name";
      ((DataGridViewComboBoxColumn) dataGridViewColumn5).ValueMember = "Id";
      this.dgvBase.Columns.Add(dataGridViewColumn5);
      DataGridViewColumn dataGridViewColumn6 = (DataGridViewColumn) new DataGridViewButtonColumn();
      dataGridViewColumn6.Name = "schemeColumn";
      dataGridViewColumn6.HeaderText = "Схема";
      this.dgvBase.Columns.Add(dataGridViewColumn6);
      DataGridViewColumn dataGridViewColumn7 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn7.Name = "PercentColumn";
      dataGridViewColumn7.HeaderText = "Процент";
      this.dgvBase.Columns.Add(dataGridViewColumn7);
      DataGridViewColumn dataGridViewColumn8 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn8.Name = "Share_id";
      dataGridViewColumn8.HeaderText = "Доля";
      ((DataGridViewComboBoxColumn) dataGridViewColumn8).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      ((DataGridViewComboBoxColumn) dataGridViewColumn8).DisplayMember = "Share_name";
      ((DataGridViewComboBoxColumn) dataGridViewColumn8).ValueMember = "Share_id";
      this.dgvBase.Columns.Add(dataGridViewColumn8);
      if (Options.ViewEdit)
      {
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 8, "Пользователь", "UName", 80, true);
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 9, "Дата редактирования", "DEdit", 80, true);
      }
      this.dgvBase.CellClick += new DataGridViewCellEventHandler(this.dgvBase_CellClick);
      this.MySettings.GridName = "MSPPersent";
    }

    protected override void GetList()
    {
      if (this.session != null && this.session.IsOpen)
        this.session.Clear();
      this.objectsList.Clear();
      string str = "";
      str = !(Options.SortService == " s.ServiceId") ? "" : "";
      if (this.curMSP != null)
      {
        if (this.closedPeriod.HasValue && !this.isArchive)
        {
          if (this.isPast)
            this.objectsList = this.session.CreateQuery(string.Format("select p from SdcMSPPercent p,Service s where p.Service=s.ServiceId and p.MSP_id={1} and p.Period.PeriodId<>{0} and p.Period.PeriodId=:period order by " + Options.SortService + ",dbeg", (object) 0, (object) this.curMSP.MSP_id)).SetInt32("period", ((Period) this.session.CreateQuery("from Period where PeriodName=:value").SetDateTime("value", this.closedPeriod.Value.AddMonths(1)).UniqueResult()).PeriodId).List();
          else
            this.objectsList = this.session.CreateQuery(string.Format("select p from SdcMSPPercent p,Service s where p.Service=s.ServiceId and p.MSP_id={1} and p.Period.PeriodId<={0} and dend>=:dend order by " + Options.SortService + ",dbeg", (object) 0, (object) this.curMSP.MSP_id)).SetDateTime("dend", this.closedPeriod.Value.AddMonths(1)).List();
        }
        else if (this.isPast)
          this.objectsList = this.session.CreateQuery(string.Format("select p from SdcMSPPercent p,Service s where p.Service=s.ServiceId and p.MSP_id={1} and p.Period.PeriodId<>{0} order by " + Options.SortService + ",dbeg", (object) 0, (object) this.curMSP.MSP_id)).List();
        else
          this.objectsList = this.session.CreateQuery(string.Format("select p from SdcMSPPercent p,Service s where p.Service=s.ServiceId and p.MSP_id={1} and p.Period.PeriodId<={0} order by " + Options.SortService + ",dbeg", (object) 0, (object) this.curMSP.MSP_id)).List();
      }
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
      if (this.closedPeriod.HasValue && (SdcMSPPercent) this.objectsList[e.RowIndex] != null)
      {
        DateTime? nullable;
        int num;
        if (!this.isPast && ((SdcMSPPercent) this.objectsList[e.RowIndex]).Dbeg < this.closedPeriod.Value.AddMonths(2))
        {
          nullable = ((SdcMSPPercent) this.objectsList[e.RowIndex]).Dend;
          DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
          if ((nullable.HasValue ? (nullable.GetValueOrDefault() >= dateTime ? 1 : 0) : 0) != 0)
          {
            num = 1;
            goto label_9;
          }
        }
        if (this.isPast)
        {
          nullable = ((SdcMSPPercent) this.objectsList[e.RowIndex]).Period.PeriodName;
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
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "serviceColumn")
        e.Value = (object) ((SdcMSPPercent) this.objectsList[e.RowIndex]).Service;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "dbegColumn")
        e.Value = (object) ((SdcMSPPercent) this.objectsList[e.RowIndex]).Dbeg;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "dendColumn")
        e.Value = (object) ((SdcMSPPercent) this.objectsList[e.RowIndex]).Dend;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "spreadingColumn")
        e.Value = (object) ((SdcMSPPercent) this.objectsList[e.RowIndex]).Spreading_id;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "schemeColumn")
        e.Value = (object) ((SdcMSPPercent) this.objectsList[e.RowIndex]).Scheme;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "PercentColumn")
        e.Value = (object) ((SdcMSPPercent) this.objectsList[e.RowIndex]).Percent;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "Share_id")
        e.Value = (object) ((SdcMSPPercent) this.objectsList[e.RowIndex]).Share_id;
      if (this.isPast && this.dgvBase.Columns[e.ColumnIndex].Name == "periodColumn")
        e.Value = (object) ((SdcMSPPercent) this.objectsList[e.RowIndex]).Period.PeriodName;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "UName")
        e.Value = (object) ((SdcMSPPercent) this.objectsList[e.RowIndex]).Uname;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "DEdit")
        e.Value = (object) ((SdcMSPPercent) this.objectsList[e.RowIndex]).Dedit.ToShortDateString();
    }

    protected override void dgvBase_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      if (!this.isPast && this.dgvBase.Columns[e.ColumnIndex].Name == "dbegColumn" && this.closedPeriod.Value.AddMonths(1) <= ((SdcMSPPercent) this.objectsList[e.RowIndex]).Dbeg && this.closedPeriod.Value.AddMonths(1) > Convert.ToDateTime(e.Value))
      {
        int num = (int) MessageBox.Show("Попытка изменить дату начала на дату, принадлежащую закрытому периоду!", "Внимание!", MessageBoxButtons.OK);
        this.isEdit = false;
      }
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "dendColumn" && ((SdcMSPPercent) this.objectsList[e.RowIndex]).Dbeg > Convert.ToDateTime(e.Value))
      {
        int num = (int) MessageBox.Show("Дата окончания не может быть меньше даты начала!", "Внимание!", MessageBoxButtons.OK);
        this.isEdit = false;
      }
      else
      {
        DateTime? nullable;
        int num1;
        if (this.dgvBase.Columns[e.ColumnIndex].Name == "dbegColumn")
        {
          nullable = ((SdcMSPPercent) this.objectsList[e.RowIndex]).Dend;
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
            if (this.closedPeriod.Value.AddMonths(1) <= ((SdcMSPPercent) this.objectsList[e.RowIndex]).Dbeg || this.isPast)
            {
              if (this.isPast)
              {
                nullable = ((SdcMSPPercent) this.objectsList[e.RowIndex]).Period.PeriodName;
                if (nullable.HasValue)
                {
                  DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
                  nullable = ((SdcMSPPercent) this.objectsList[e.RowIndex]).Period.PeriodName;
                  if ((nullable.HasValue ? (dateTime <= nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0)
                    goto label_16;
                }
              }
              else
                goto label_16;
            }
            num2 = this.dgvBase.Columns[e.ColumnIndex].Name == "dendColumn" ? 1 : 0;
            goto label_17;
          }
label_16:
          num2 = 1;
label_17:
          if (num2 != 0)
          {
            if (e.RowIndex >= 0)
            {
              try
              {
                if (this.dgvBase.Columns[e.ColumnIndex].Name == "serviceColumn")
                {
                  if (this.newObject != null && e.RowIndex == this.objectsList.Count - 1)
                    ((SdcMSPPercent) this.objectsList[e.RowIndex]).Service = Convert.ToInt16(e.Value);
                }
                else if (this.dgvBase.Columns[e.ColumnIndex].Name == "dbegColumn")
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
                    ((SdcMSPPercent) this.objectsList[e.RowIndex]).Dbeg = Convert.ToDateTime(e.Value);
                  }
                  catch (Exception ex)
                  {
                    int num3 = (int) MessageBox.Show("Проверьте правильность ввода даты!");
                    this.isEdit = false;
                    return;
                  }
                }
                else if (this.dgvBase.Columns[e.ColumnIndex].Name == "dendColumn" && e.Value != null)
                {
                  DateTime dateTime1;
                  int num3;
                  if (this.closedPeriod.HasValue && !this.isPast)
                  {
                    DateTime dateTime2 = Convert.ToDateTime(e.Value);
                    dateTime1 = this.closedPeriod.Value;
                    dateTime1 = dateTime1.AddMonths(1);
                    DateTime dateTime3 = dateTime1.AddDays(-1.0);
                    if (dateTime2 >= dateTime3)
                    {
                      nullable = ((SdcMSPPercent) this.objectsList[e.RowIndex]).Dend;
                      dateTime1 = this.closedPeriod.Value.AddMonths(1).AddDays(-1.0);
                      num3 = nullable.HasValue ? (nullable.GetValueOrDefault() >= dateTime1 ? 1 : 0) : 0;
                    }
                    else
                      num3 = 0;
                  }
                  else
                    num3 = 1;
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
                    try
                    {
                      ((SdcMSPPercent) this.objectsList[e.RowIndex]).Dend = new DateTime?(Convert.ToDateTime(e.Value));
                    }
                    catch (Exception ex)
                    {
                      int num5 = (int) MessageBox.Show("Проверьте правильность ввода даты!");
                      this.isEdit = false;
                      return;
                    }
                  }
                  else
                  {
                    int num4 = (int) MessageBox.Show("Дата окончания принадлежит закрытому периоду, либо существует запись, принадлежащая закрытому периоду, которая перекрывается этой датой!", "Внимание!", MessageBoxButtons.OK);
                    this.isEdit = false;
                    return;
                  }
                }
                else if (this.dgvBase.Columns[e.ColumnIndex].Name == "spreadingColumn")
                {
                  try
                  {
                    ((SdcMSPPercent) this.objectsList[e.RowIndex]).Spreading_id = Convert.ToInt32(e.Value);
                  }
                  catch
                  {
                  }
                }
                else if (this.dgvBase.Columns[e.ColumnIndex].Name == "schemeColumn")
                {
                  if (e.Value == null || e.Value.ToString() == "")
                  {
                    ((SdcMSPPercent) this.objectsList[e.RowIndex]).Scheme = new short?();
                  }
                  else
                  {
                    try
                    {
                      ((SdcMSPPercent) this.objectsList[e.RowIndex]).Scheme = new short?(Convert.ToInt16(e.Value));
                    }
                    catch
                    {
                      int num3 = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
                      this.isEdit = false;
                      return;
                    }
                  }
                }
                else if (this.dgvBase.Columns[e.ColumnIndex].Name == "PercentColumn" && e.Value != (object) "")
                  ((SdcMSPPercent) this.objectsList[e.RowIndex]).Percent = Convert.ToInt32(e.Value);
                else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Share_id")
                {
                  try
                  {
                    if (e.Value == null)
                      ((SdcMSPPercent) this.objectsList[e.RowIndex]).Share_id = (short) -1;
                    else
                      ((SdcMSPPercent) this.objectsList[e.RowIndex]).Share_id = Convert.ToInt16(e.Value);
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
                  ((SdcMSPPercent) this.curObject).Uname = Options.Login;
                  ((SdcMSPPercent) this.curObject).Dedit = DateTime.Now;
                }
              }
              catch
              {
                int num3 = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
                this.isEdit = false;
                return;
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
      if (KvrplHelper.CheckProxy(32, 2, (Company) null, false))
      {
        this.newObject = (object) new SdcMSPPercent();
        if (this.closedPeriod.HasValue)
        {
          if (this.isPast)
            ((SdcMSPPercent) this.newObject).Dbeg = this.closedPeriod.Value;
          else
            ((SdcMSPPercent) this.newObject).Dbeg = this.closedPeriod.Value.AddMonths(1);
        }
        else
          ((SdcMSPPercent) this.newObject).Dbeg = DateTime.Now;
        if (!this.isPast)
          ((SdcMSPPercent) this.newObject).Period = this.session.Get<Period>((object) Convert.ToInt32(this.session.CreateQuery("select p.PeriodId from Period p where PeriodName is null").UniqueResult()));
        else if (this.closedPeriod.HasValue)
        {
          ((SdcMSPPercent) this.newObject).Period = (Period) this.session.CreateQuery("from Period p where p.PeriodName=:dt").SetDateTime("dt", this.closedPeriod.Value.AddMonths(1)).UniqueResult();
        }
        else
        {
          int num = (int) MessageBox.Show("Отсутствует закрытый период!", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        ((SdcMSPPercent) this.newObject).MSP_id = this.curMSP.MSP_id;
        ((SdcMSPPercent) this.newObject).Share_id = (short) -1;
        if (this.closedPeriod.HasValue && this.isPast)
          ((SdcMSPPercent) this.newObject).Dend = new DateTime?(this.closedPeriod.Value.AddMonths(1).AddDays(-1.0));
        else
          ((SdcMSPPercent) this.newObject).Dend = new DateTime?(Convert.ToDateTime("31.12.2999"));
        base.tsbAdd_Click(sender, e);
        this.dt = new DateTime?(((SdcMSPPercent) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg);
      }
      else
      {
        if (!KvrplHelper.CheckProxy(80, 1, (Company) null, false))
          return;
        this.newObject = (object) new SdcMSPPercent();
        if (this.closedPeriod.HasValue)
        {
          if (this.isPast)
            ((SdcMSPPercent) this.newObject).Dbeg = this.closedPeriod.Value;
          else
            ((SdcMSPPercent) this.newObject).Dbeg = this.closedPeriod.Value.AddMonths(1);
        }
        else
          ((SdcMSPPercent) this.newObject).Dbeg = DateTime.Now;
        if (!this.isPast)
          ((SdcMSPPercent) this.newObject).Period = this.session.Get<Period>((object) Convert.ToInt32(this.session.CreateQuery("select p.PeriodId from Period p where PeriodName is null").UniqueResult()));
        else if (this.closedPeriod.HasValue)
        {
          ((SdcMSPPercent) this.newObject).Period = (Period) this.session.CreateQuery("from Period p where p.PeriodName=:dt").SetDateTime("dt", this.closedPeriod.Value.AddMonths(1)).UniqueResult();
        }
        else
        {
          int num = (int) MessageBox.Show("Отсутствует закрытый период!", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        ((SdcMSPPercent) this.newObject).MSP_id = this.curMSP.MSP_id;
        ((SdcMSPPercent) this.newObject).Share_id = (short) -1;
        if (this.closedPeriod.HasValue && this.isPast)
          ((SdcMSPPercent) this.newObject).Dend = new DateTime?(this.closedPeriod.Value.AddMonths(1).AddDays(-1.0));
        else
          ((SdcMSPPercent) this.newObject).Dend = new DateTime?(Convert.ToDateTime("31.12.2999"));
        base.tsbAdd_Click(sender, e);
        this.dt = new DateTime?(((SdcMSPPercent) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg);
      }
    }

    protected override void tsbApplay_Click(object sender, EventArgs e)
    {
      if (KvrplHelper.CheckProxy(32, 2, (Company) null, false))
      {
        this.ApplyChanges(sender, e);
      }
      else
      {
        if (!KvrplHelper.CheckProxy(80, 1, (Company) null, false))
          return;
        this.ApplyChanges(sender, e);
      }
    }

    private void ApplyChanges(object sender, EventArgs e)
    {
      this.dgvBase.EndEdit();
      if (!this.isEdit)
      {
        this.isEdit = true;
      }
      else
      {
        if (this.newObject != null)
        {
          if ((int) ((SdcMSPPercent) this.newObject).Service == 0)
          {
            int num = (int) MessageBox.Show("Выберите услугу!", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          if ((int) ((SdcMSPPercent) this.newObject).Share_id == -1)
          {
            int num = (int) MessageBox.Show("Укажите на какую часть распространяется льгота!", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          if (((SdcMSPPercent) this.newObject).Dbeg <= DateTime.Now.AddYears(-3) && MessageBox.Show("Дата начала отличается от текущей более, чем на 3 года. Продолжить", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            return;
          if (this.closedPeriod.HasValue && ((SdcMSPPercent) this.newObject).Dbeg < this.closedPeriod.Value.AddMonths(1) && !this.isPast)
          {
            int num1 = (int) MessageBox.Show("Дата начала принадлежит закрытому периоду!", "Внимание!", MessageBoxButtons.OK);
          }
          else
          {
            ((SdcMSPPercent) this.newObject).Uname = Options.Login;
            ((SdcMSPPercent) this.newObject).Dedit = DateTime.Now;
          }
        }
        else
        {
          if ((int) ((SdcMSPPercent) this.curObject).Share_id == -1)
          {
            int num = (int) MessageBox.Show("Укажите на какую часть распространяется льгота!", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          if ((((SdcMSPPercent) this.curObject).Dbeg <= DateTime.Now.AddYears(-3) || ((SdcMSPPercent) this.curObject).Dbeg >= DateTime.Now.AddYears(3)) && MessageBox.Show("Дата начала отличается от текущей более, чем на 3 года. Продолжить", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            return;
          if (!this.dt.Equals((object) ((SdcMSPPercent) this.curObject).Dbeg))
          {
            using (ITransaction transaction = this.session.BeginTransaction())
            {
              try
              {
                this.session.CreateSQLQuery("update DBA.sdcMSPPercent cmp set Dbeg=:dbeg,Scheme=:scheme,Dend=:dend,Spreading_id=:s_id, Percent=:percent,Share_id=:share_id,Uname=:uname,Dedit=:dedit where cmp.Period_id=:period and cmp.Dbeg = :dbeg2 and cmp.Service_id = :service and cmp.MSP_id=:MSP_id").SetDateTime("dbeg", ((SdcMSPPercent) this.curObject).Dbeg).SetParameter<short?>("scheme", ((SdcMSPPercent) this.curObject).Scheme).SetParameter<DateTime?>("dend", ((SdcMSPPercent) this.curObject).Dend).SetInt32("s_id", ((SdcMSPPercent) this.curObject).Spreading_id).SetInt32("percent", ((SdcMSPPercent) this.curObject).Percent).SetDateTime("dbeg2", this.dt.Value).SetInt16("service", ((SdcMSPPercent) this.curObject).Service).SetInt32("period", ((SdcMSPPercent) this.curObject).Period.PeriodId).SetInt32("MSP_id", ((SdcMSPPercent) this.curObject).MSP_id).SetParameter<short>("share_id", ((SdcMSPPercent) this.curObject).Share_id).SetDateTime("dedit", DateTime.Now).SetString("uname", Options.Login).ExecuteUpdate();
                transaction.Commit();
              }
              catch (Exception ex)
              {
                int num = (int) MessageBox.Show("Не удалось внести изменения! Проверьте правильность ввода данных.", "Внимание!", MessageBoxButtons.OK);
                KvrplHelper.WriteLog(ex, (LsClient) null);
                transaction.Rollback();
              }
            }
            this.CurIndex = -1;
            this.GetList();
          }
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
        this.dt = new DateTime?(((SdcMSPPercent) this.curObject).Dbeg);
      }
    }

    protected override void tsbCancel_Click(object sender, EventArgs e)
    {
      if (this.curObject != null && !this.dt.Equals((object) ((SdcMSPPercent) this.curObject).Dbeg))
        ((SdcMSPPercent) this.curObject).Dbeg = this.dt.Value;
      base.tsbCancel_Click(sender, e);
      if (this.dgvBase.CurrentRow != null)
        this.CurIndex = this.dgvBase.CurrentRow.Index;
      this.IsCopy = false;
    }

    protected override void tsbDelete_Click(object sender, EventArgs e)
    {
      if (KvrplHelper.CheckProxy(32, 2, (Company) null, false))
      {
        if (MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
          return;
        if (this.objectsList.Count > 0)
        {
          int num1;
          if (this.closedPeriod.HasValue)
          {
            if (this.closedPeriod.Value.AddMonths(1) <= ((SdcMSPPercent) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg || this.isPast)
            {
              if (this.isPast)
              {
                if (((SdcMSPPercent) this.objectsList[this.dgvBase.CurrentRow.Index]).Period.PeriodName.HasValue)
                {
                  DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
                  DateTime? periodName = ((SdcMSPPercent) this.objectsList[this.dgvBase.CurrentRow.Index]).Period.PeriodName;
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
              this.session.Clear();
              try
              {
                if (((SdcMSPPercent) this.curObject).Period != null && ((SdcMSPPercent) this.curObject).Period.PeriodId == 0)
                  ((SdcMSPPercent) this.curObject).Period = this.session.Get<Period>((object) 0);
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
          this.dt = new DateTime?(((SdcMSPPercent) this.curObject).Dbeg);
          this.CurIndex = this.dgvBase.CurrentRow.Index;
        }
      }
      else
      {
        if (!KvrplHelper.CheckProxy(80, 1, (Company) null, false) || MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
          return;
        if (this.objectsList.Count > 0)
        {
          int num1;
          if (this.closedPeriod.HasValue)
          {
            if (this.closedPeriod.Value.AddMonths(1) <= ((SdcMSPPercent) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg || this.isPast)
            {
              if (this.isPast)
              {
                if (((SdcMSPPercent) this.objectsList[this.dgvBase.CurrentRow.Index]).Period.PeriodName.HasValue)
                {
                  DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
                  DateTime? periodName = ((SdcMSPPercent) this.objectsList[this.dgvBase.CurrentRow.Index]).Period.PeriodName;
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
              this.session.Clear();
              try
              {
                if (((SdcMSPPercent) this.curObject).Period != null && ((SdcMSPPercent) this.curObject).Period.PeriodId == 0)
                  ((SdcMSPPercent) this.curObject).Period = this.session.Get<Period>((object) 0);
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
          this.dt = new DateTime?(((SdcMSPPercent) this.curObject).Dbeg);
          this.CurIndex = this.dgvBase.CurrentRow.Index;
        }
      }
    }

    protected override void dgvBase_SelectionChanged(object sender, EventArgs e)
    {
      if (this.dgvBase.CurrentRow == null || this.dgvBase.CurrentRow.Index >= this.objectsList.Count)
        return;
      if (this.newObject == null && (this.CurIndex >= 0 && this.objectsList.Count > this.CurIndex && !this.dt.Equals((object) ((SdcMSPPercent) this.objectsList[this.CurIndex]).Dbeg) && this.CurIndex != this.dgvBase.CurrentRow.Index))
      {
        try
        {
          ((SdcMSPPercent) this.objectsList[this.CurIndex]).Dbeg = this.dt.Value;
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
        this.dt = new DateTime?(((SdcMSPPercent) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg);
        this.CurIndex = this.dgvBase.CurrentRow.Index;
        this.curObject = this.objectsList[this.dgvBase.CurrentRow.Index];
      }
    }

    protected override void tsmCopy_Click(object sender, EventArgs e)
    {
      this.isEdit = true;
      this.newObject = (object) new SdcMSPPercent();
      ((SdcMSPPercent) this.newObject).Copy((SdcMSPPercent) this.curObject);
      DateTime dateTime1;
      int num;
      if (!this.isPast && this.closedPeriod.HasValue)
      {
        DateTime dbeg = ((SdcMSPPercent) this.newObject).Dbeg;
        dateTime1 = this.closedPeriod.Value;
        DateTime dateTime2 = dateTime1.AddMonths(1);
        num = dbeg < dateTime2 ? 1 : 0;
      }
      else
        num = 0;
      if (num != 0)
      {
        SdcMSPPercent newObject = (SdcMSPPercent) this.newObject;
        dateTime1 = this.closedPeriod.Value;
        DateTime dateTime2 = dateTime1.AddMonths(1);
        newObject.Dbeg = dateTime2;
      }
      else if (this.isPast)
      {
        ((SdcMSPPercent) this.newObject).Dbeg = this.closedPeriod.Value;
      }
      else
      {
        SdcMSPPercent newObject = (SdcMSPPercent) this.newObject;
        dateTime1 = ((SdcMSPPercent) this.newObject).Dbeg;
        dateTime1 = dateTime1.AddMonths(1);
        DateTime dateTime2 = dateTime1.AddDays((double) (-((SdcMSPPercent) this.newObject).Dbeg.Day + 1));
        newObject.Dbeg = dateTime2;
      }
      base.tsbAdd_Click(sender, e);
      this.dt = new DateTime?(((SdcMSPPercent) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg);
      base.tsmCopy_Click(sender, e);
    }

    private void UCMSPPercent_CurObjectChanged(object sender, EventArgs e)
    {
    }

    protected override void tsmCopyInPast_Click(object sender, EventArgs e)
    {
      if (this.tsbApplay.Enabled)
        this.isEdit = true;
      if (!this.isPast)
        this.IsCopy = true;
      this.dgvBase.EndEdit();
      this.newObject = (object) new SdcMSPPercent();
      ((SdcMSPPercent) this.newObject).Copy((SdcMSPPercent) this.curObject);
      if (!this.isPast)
        this.IsPast = !this.isPast;
      if (!this.IsCopy)
        return;
      if (this.closedPeriod.HasValue)
      {
        ((SdcMSPPercent) this.newObject).Period = (Period) this.session.CreateQuery("from Period p where p.PeriodName=:dt").SetDateTime("dt", this.closedPeriod.Value.AddMonths(1)).UniqueResult();
        if (this.isPast && this.closedPeriod.HasValue)
        {
          ((SdcMSPPercent) this.newObject).Dbeg = this.closedPeriod.Value;
          SdcMSPPercent newObject = (SdcMSPPercent) this.newObject;
          DateTime dateTime = this.closedPeriod.Value;
          dateTime = dateTime.AddMonths(1);
          DateTime? nullable = new DateTime?(dateTime.AddDays(-1.0));
          newObject.Dend = nullable;
        }
        base.tsbAdd_Click(sender, e);
        this.dt = new DateTime?(((SdcMSPPercent) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg);
        base.tsmCopyInPast_Click(sender, e);
      }
      else
      {
        int num = (int) MessageBox.Show("Отсутствует закрытый период!", "Внимание!", MessageBoxButtons.OK);
      }
    }

    private void dgvBase_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex <= 0 || e.RowIndex < 0 || !(this.dgvBase.Columns[e.ColumnIndex].Name == "schemeColumn"))
        return;
      short id = 0;
      short? scheme = ((SdcMSPPercent) this.objectsList[e.RowIndex]).Scheme;
      if (scheme.HasValue)
      {
        scheme = ((SdcMSPPercent) this.objectsList[e.RowIndex]).Scheme;
        id = scheme.Value;
      }
      FrmScheme frmScheme = new FrmScheme((short) 3, id);
      if (frmScheme.ShowDialog() == DialogResult.OK)
        id = frmScheme.CurrentId();
      ((SdcMSPPercent) this.objectsList[e.RowIndex]).Scheme = new short?(id);
      frmScheme.Dispose();
      this.tsbAdd.Enabled = false;
      this.tsbApplay.Enabled = true;
      this.tsbCancel.Enabled = true;
      this.tsbDelete.Enabled = false;
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
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.Name = "UCMSPPercent";
      this.CurObjectChanged += new EventHandler(this.UCMSPPercent_CurObjectChanged);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
