// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.Controls.UCCrossService
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms.Controls
{
  public class UCCrossService : UCBase
  {
    private IContainer components = (IContainer) null;
    private short company_id;
    private IList<CrossService> oldList;
    private Period monthClosed;
    private bool isArchive;

    public Period MonthClosed
    {
      set
      {
        this.monthClosed = value;
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
          this.dgvBase.CurrentCell = this.dgvBase.Rows[0].Cells[this.dgvBase.Columns.Count - 1];
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

    public UCCrossService()
    {
      this.InitializeComponent();
      this.dgvBase.ContextMenuStrip = (ContextMenuStrip) null;
      DataGridViewColumn dataGridViewColumn1 = (DataGridViewColumn) new MaskDateColumn();
      dataGridViewColumn1.Name = "DBeg";
      dataGridViewColumn1.HeaderText = "Дата начала действия";
      this.dgvBase.Columns.Add(dataGridViewColumn1);
      DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn) new MaskDateColumn();
      dataGridViewColumn2.Name = "DEnd";
      dataGridViewColumn2.HeaderText = "Дата окончания действия";
      this.dgvBase.Columns.Add(dataGridViewColumn2);
      DataGridViewColumn dataGridViewColumn3 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn3.Name = "Service";
      dataGridViewColumn3.HeaderText = "Услуга";
      dataGridViewColumn3.Width = 200;
      ((DataGridViewComboBoxColumn) dataGridViewColumn3).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      dataGridViewColumn3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      ((DataGridViewComboBoxColumn) dataGridViewColumn3).DisplayMember = "ServiceName";
      ((DataGridViewComboBoxColumn) dataGridViewColumn3).ValueMember = "ServiceId";
      this.dgvBase.Columns.Add(dataGridViewColumn3);
      DataGridViewColumn dataGridViewColumn4 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn4.Name = "CrossType";
      dataGridViewColumn4.HeaderText = "Тип связи";
      dataGridViewColumn4.Width = 150;
      ((DataGridViewComboBoxColumn) dataGridViewColumn4).DisplayMember = "CrossTypeName";
      ((DataGridViewComboBoxColumn) dataGridViewColumn4).ValueMember = "CrossTypeId";
      ((DataGridViewComboBoxColumn) dataGridViewColumn4).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      dataGridViewColumn4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgvBase.Columns.Add(dataGridViewColumn4);
      DataGridViewColumn dataGridViewColumn5 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn5.Name = "CrossServ";
      dataGridViewColumn5.HeaderText = "Связанная услуга";
      ((DataGridViewComboBoxColumn) dataGridViewColumn5).DisplayMember = "ServiceName";
      ((DataGridViewComboBoxColumn) dataGridViewColumn5).ValueMember = "ServiceId";
      ((DataGridViewComboBoxColumn) dataGridViewColumn5).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      dataGridViewColumn5.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgvBase.Columns.Add(dataGridViewColumn5);
      if (Options.ViewEdit)
      {
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 5, "Пользователь", "UName", 100, true);
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 6, "Дата редактирования", "DEdit", 100, true);
      }
      this.MySettings.GridName = "CrossService";
    }

    public void LoadData()
    {
      this.GetList();
    }

    public void SaveData()
    {
      this.tsbApplay_Click((object) this.tsbApplay, EventArgs.Empty);
    }

    protected override void GetList()
    {
      if (this.session == null)
        return;
      string str = "";
      if (!this.isArchive)
        str = string.Format("and DEnd>='{0}'", (object) KvrplHelper.DateToBaseFormat(this.monthClosed.PeriodName.Value.AddMonths(1)));
      string queryString = string.Format("from CrossService where Company.CompanyId={0} " + str + " order by Service.ServiceId,CrossType.CrossTypeName,DBeg", (object) this.company_id);
      this.objectsList.Clear();
      this.objectsList = this.session.CreateQuery(queryString).List();
      foreach (object objects in (IEnumerable) this.objectsList)
        this.session.Refresh(objects);
      this.dgvBase.RowCount = this.objectsList.Count;
      this.dgvBase.Refresh();
      this.SelectRow();
      this.session.Clear();
      this.oldList = this.session.CreateQuery(queryString).List<CrossService>();
      int index = 0;
      foreach (CrossService old in (IEnumerable<CrossService>) this.oldList)
      {
        old.OldHashCode = old.GetHashCode();
        ((CrossService) this.objectsList[index]).OldHashCode = old.OldHashCode;
        ++index;
      }
    }

    protected override void dgvBase_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.objectsList.Count <= 0)
        return;
      DateTime dedit;
      if (this.monthClosed != null)
      {
        DateTime dbeg = ((CrossService) this.objectsList[e.RowIndex]).DBeg;
        dedit = this.monthClosed.PeriodName.Value;
        DateTime dateTime1 = dedit.AddMonths(2);
        int num;
        if (dbeg < dateTime1)
        {
          DateTime dend = ((CrossService) this.objectsList[e.RowIndex]).DEnd;
          dedit = this.monthClosed.PeriodName.Value;
          DateTime dateTime2 = dedit.AddMonths(1);
          num = dend >= dateTime2 ? 1 : 0;
        }
        else
          num = 0;
        if (num != 0)
          this.dgvBase.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.PapayaWhip;
        else
          this.dgvBase.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      }
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "Service" && ((CrossService) this.objectsList[e.RowIndex]).Service != null)
        e.Value = (object) ((CrossService) this.objectsList[e.RowIndex]).Service.ServiceId;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "CrossType" && ((CrossService) this.objectsList[e.RowIndex]).CrossType != null)
        e.Value = (object) ((CrossService) this.objectsList[e.RowIndex]).CrossType.CrossTypeId;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "CrossServ" && ((CrossService) this.objectsList[e.RowIndex]).CrossServ != null)
        e.Value = (object) ((CrossService) this.objectsList[e.RowIndex]).CrossServ.ServiceId;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "DBeg")
        e.Value = (object) ((CrossService) this.objectsList[e.RowIndex]).DBeg;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "DEnd")
        e.Value = (object) ((CrossService) this.objectsList[e.RowIndex]).DEnd;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "UName")
        e.Value = (object) ((CrossService) this.objectsList[e.RowIndex]).UName;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "DEdit")
      {
        DataGridViewCellValueEventArgs cellValueEventArgs = e;
        dedit = ((CrossService) this.objectsList[e.RowIndex]).DEdit;
        string shortDateString = dedit.ToShortDateString();
        cellValueEventArgs.Value = (object) shortDateString;
      }
    }

    protected override void dgvBase_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      try
      {
        if (this.dgvBase.Columns[e.ColumnIndex].Name == "Service" && e.Value != null)
          ((CrossService) this.objectsList[e.RowIndex]).Service = this.session.Get<Service>((object) Convert.ToInt16(e.Value));
        else if (this.dgvBase.Columns[e.ColumnIndex].Name == "CrossType" && e.Value != null)
          ((CrossService) this.objectsList[e.RowIndex]).CrossType = this.session.Get<CrossType>((object) Convert.ToInt16(e.Value));
        else if (this.dgvBase.Columns[e.ColumnIndex].Name == "CrossServ" && e.Value != null)
          ((CrossService) this.objectsList[e.RowIndex]).CrossServ = this.session.Get<Service>((object) Convert.ToInt16(e.Value));
        else if (this.dgvBase.Columns[e.ColumnIndex].Name == "DBeg" && e.Value != null)
          ((CrossService) this.objectsList[e.RowIndex]).DBeg = Convert.ToDateTime(e.Value);
        else if (this.dgvBase.Columns[e.ColumnIndex].Name == "DEnd" && e.Value != null)
          ((CrossService) this.objectsList[e.RowIndex]).DEnd = Convert.ToDateTime(e.Value);
      }
      catch
      {
        int num = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
        return;
      }
      ((CrossService) this.objectsList[e.RowIndex]).UName = Options.Login;
      ((CrossService) this.objectsList[e.RowIndex]).DEdit = DateTime.Now;
    }

    protected override void tsbAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, this.session.Get<Company>((object) this.Company_id), true))
        return;
      this.newObject = (object) new CrossService();
      ((CrossService) this.newObject).Company = this.session.Get<Company>((object) this.Company_id);
      ((CrossService) this.newObject).Service = this.session.Get<Service>((object) Convert.ToInt16(0));
      ((CrossService) this.newObject).CrossServ = this.session.Get<Service>((object) Convert.ToInt16(0));
      ((CrossService) this.newObject).DBeg = this.monthClosed.PeriodName.Value.AddMonths(1);
      ((CrossService) this.newObject).DEnd = Convert.ToDateTime("2999-12-31");
      ((CrossService) this.newObject).CrossType = new CrossType()
      {
        CrossTypeId = short.MaxValue
      };
      base.tsbAdd_Click(sender, e);
    }

    protected override void tsbApplay_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, this.session.Get<Company>((object) this.Company_id), true))
        return;
      this.dgvBase.EndEdit();
      CrossService crossService1 = new CrossService();
      CrossService crossService2 = new CrossService();
      DateTime dateTime1 = this.monthClosed.PeriodName.Value;
      dateTime1 = dateTime1.AddMonths(1);
      DateTime dateTime2 = dateTime1.AddDays(-1.0);
      if (this.newObject != null)
      {
        if (((CrossService) this.newObject).Service == null || (int) ((CrossService) this.newObject).Service.ServiceId == 0)
        {
          int num1 = (int) MessageBox.Show("Выберите услугу.", "Внимание!", MessageBoxButtons.OK);
        }
        else if (((CrossService) this.newObject).CrossType == null)
        {
          int num2 = (int) MessageBox.Show("Выберите тип.", "Внимание!", MessageBoxButtons.OK);
        }
        else if (((CrossService) this.newObject).CrossServ == null || (int) ((CrossService) this.newObject).CrossServ.ServiceId == 0)
        {
          int num3 = (int) MessageBox.Show("Выберите связанную услугу.", "Внимание!", MessageBoxButtons.OK);
        }
        else
        {
          if ((int) ((CrossService) this.newObject).CrossType.CrossTypeId == 6)
          {
            ((CrossService) this.newObject).DBeg = KvrplHelper.FirstDay(((CrossService) this.newObject).DBeg);
            ((CrossService) this.newObject).DEnd = KvrplHelper.LastDay(((CrossService) this.newObject).DEnd);
          }
          ((CrossService) this.newObject).UName = Options.Login;
          ((CrossService) this.newObject).DEdit = DateTime.Now;
          CrossService newObject1 = (CrossService) this.newObject;
          CrossService newObject2 = (CrossService) this.newObject;
          if (((CrossService) this.newObject).DBeg <= dateTime2 || ((CrossService) this.newObject).DEnd <= dateTime2)
          {
            int num4 = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            if (!this.Control(newObject1, newObject2))
              return;
            base.tsbApplay_Click(sender, e);
          }
        }
      }
      else if (this.curObject != null)
      {
        if ((int) ((CrossService) this.curObject).CrossType.CrossTypeId == 6)
        {
          ((CrossService) this.curObject).DBeg = KvrplHelper.FirstDay(((CrossService) this.curObject).DBeg);
          ((CrossService) this.curObject).DEnd = KvrplHelper.LastDay(((CrossService) this.curObject).DEnd);
        }
        ((CrossService) this.curObject).UName = Options.Login;
        ((CrossService) this.curObject).DEdit = DateTime.Now;
        if (!this.Control((CrossService) this.curObject, (CrossService) this.curObject))
          return;
        CrossService crossService3 = new CrossService();
        foreach (CrossService old in (IEnumerable<CrossService>) this.oldList)
        {
          if (old.OldHashCode == ((CrossService) this.curObject).OldHashCode)
            crossService3 = old;
        }
        if (crossService3.DBeg <= dateTime2 && crossService3.DEnd < dateTime2 || ((CrossService) this.curObject).DEnd < dateTime2 || crossService3.DBeg > dateTime2 && ((CrossService) this.curObject).DBeg <= dateTime2 || crossService3.DBeg <= dateTime2 && (crossService3.DBeg != ((CrossService) this.curObject).DBeg || (int) crossService3.CrossServ.ServiceId != (int) ((CrossService) this.curObject).CrossServ.ServiceId || (int) crossService3.Service.ServiceId != (int) ((CrossService) this.curObject).Service.ServiceId || (int) crossService3.CrossType.CrossTypeId != (int) ((CrossService) this.curObject).CrossType.CrossTypeId))
        {
          int num4 = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          try
          {
            this.session.CreateQuery("update CrossService set Service.ServiceId=:service,CrossType.CrossTypeId=:type, CrossServ.ServiceId=:crossserv,DBeg=:dbeg,DEnd=:dend where Company.CompanyId=:oldcompany and Service.ServiceId=:oldservice and CrossType.CrossTypeId=:oldtype and CrossServ.ServiceId=:oldcrossserv and DBeg=:oldbeg").SetParameter<short>("service", ((CrossService) this.curObject).Service.ServiceId).SetParameter<short>("type", ((CrossService) this.curObject).CrossType.CrossTypeId).SetParameter<short>("crossserv", ((CrossService) this.curObject).CrossServ.ServiceId).SetDateTime("dbeg", ((CrossService) this.curObject).DBeg).SetDateTime("dend", ((CrossService) this.curObject).DEnd).SetParameter<short>("oldcompany", this.company_id).SetParameter<short>("oldservice", crossService3.Service.ServiceId).SetParameter<short>("oldtype", crossService3.CrossType.CrossTypeId).SetParameter<short>("oldcrossserv", crossService3.CrossServ.ServiceId).SetParameter<DateTime>("oldbeg", crossService3.DBeg).ExecuteUpdate();
          }
          catch (Exception ex)
          {
            int num5 = (int) MessageBox.Show("Не удалось внести изменения! Проверьте правильность ввода данных.", "Внимание!", MessageBoxButtons.OK);
            KvrplHelper.WriteLog(ex, (LsClient) null);
            return;
          }
          base.tsbApplay_Click(sender, e);
        }
      }
    }

    protected override void tsbDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, this.session.Get<Company>((object) this.Company_id), true) || this.curObject == null || MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      if (((CrossService) this.curObject).DBeg < this.monthClosed.PeriodName.Value.AddMonths(1))
      {
        int num1 = (int) MessageBox.Show("Запись принадлежит закрытому периоду", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        try
        {
          base.tsbDelete_Click(sender, e);
        }
        catch
        {
          int num2 = (int) MessageBox.Show("Невозможно удалить запись!", "Внимание!", MessageBoxButtons.OK);
        }
        this.GetList();
      }
    }

    private bool Control(CrossService crossService, CrossService crService)
    {
      bool flag = true;
      while (flag)
      {
        IList<CrossService> crossServiceList1 = (IList<CrossService>) new List<CrossService>();
        IList<CrossService> crossServiceList2 = this.session.CreateQuery(string.Format("from CrossService where Company.CompanyId={0} and CrossType.CrossTypeId={1} and Service.ServiceId={2}", (object) crossService.Company.CompanyId, (object) crossService.CrossType.CrossTypeId, (object) crossService.CrossServ.ServiceId)).List<CrossService>();
        if (crossServiceList2.Count == 0 && (int) crService.Service.ServiceId != (int) crService.CrossServ.ServiceId)
        {
          flag = false;
        }
        else
        {
          if ((int) crService.Service.ServiceId == (int) crService.CrossServ.ServiceId || crossServiceList2.Count > 0 && (int) crossServiceList2[0].CrossServ.ServiceId == (int) crService.Service.ServiceId)
          {
            int num = (int) MessageBox.Show("Услуга ссылается на саму себя", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
          crossService = crossServiceList2[0];
        }
      }
      if (!(((CrossService) this.curObject).DBeg > ((CrossService) this.curObject).DEnd))
        return true;
      int num1 = (int) MessageBox.Show("Дата начала меньше даты окончания", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
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
