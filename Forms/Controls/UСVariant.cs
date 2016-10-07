// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.Controls.UСVariant
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms.Controls
{
  public class UСVariant : UCBase
  {
    private bool isEdit = false;
    private IContainer components = (IContainer) null;
    private BaseOrg manager;
    private Service curService;
    public DateTime? closedPeriod;

    public Tariff CurVar
    {
      get
      {
        return (Tariff) this.curObject;
      }
    }

    public IList ObjectsList
    {
      get
      {
        return this.objectsList;
      }
    }

    public Service CurService
    {
      set
      {
        this.curService = value;
      }
    }

    public BaseOrg Manager
    {
      get
      {
        return this.manager;
      }
      set
      {
        if (this.manager != value)
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
          this.manager = value;
          this.curObject = (object) null;
          this.GetList();
          if (this.objectsList.Count <= 0)
            return;
          this.CurIndex = 0;
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

    public UСVariant()
    {
      this.InitializeComponent();
      this.dgvBase.ContextMenuStrip = (ContextMenuStrip) null;
      DataGridViewColumn dataGridViewColumn1 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn1.Name = "numColumn";
      dataGridViewColumn1.HeaderText = "Номер варианта";
      dataGridViewColumn1.Width = 100;
      this.dgvBase.Columns.Add(dataGridViewColumn1);
      if (Options.ViewEdit)
      {
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 1, "Пользователь", "UName", 80, true);
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 2, "Дата редактирования", "DEdit", 80, true);
      }
      DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn2.Name = "nameColumn";
      dataGridViewColumn2.HeaderText = "Наименование";
      dataGridViewColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgvBase.Columns.Insert(1, dataGridViewColumn2);
      this.MySettings.GridName = "Variant";
    }

    protected override void GetList()
    {
      this.session = Domain.CurrentSession;
      if (this.session.IsOpen)
        this.session.Clear();
      if (this.curService == null)
        return;
      this.objectsList.Clear();
      if (this.manager != null)
        this.objectsList = this.session.CreateQuery(string.Format("from Tariff where Manager.BaseOrgId={1} and service_id = {0} order by Tariff_num", (object) this.curService.ServiceId, (object) this.manager.BaseOrgId)).List();
      else
        this.objectsList = this.session.CreateQuery(string.Format("from Tariff where service_id = {0} order by Tariff_num", (object) this.curService.ServiceId)).List();
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
        e.Value = (object) ((Tariff) this.objectsList[e.RowIndex]).Tariff_name;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "numColumn")
        e.Value = (object) ((Tariff) this.objectsList[e.RowIndex]).Tariff_num;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "CounterColumn")
        e.Value = (object) ((Tariff) this.objectsList[e.RowIndex]).Counter_id;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "UName")
        e.Value = (object) ((Tariff) this.objectsList[e.RowIndex]).Uname;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "DEdit")
        e.Value = (object) ((Tariff) this.objectsList[e.RowIndex]).Dedit.ToShortDateString();
    }

    protected override void dgvBase_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      try
      {
        int int32_1 = Convert.ToInt32(this.session.CreateQuery(string.Format("select count(*) from cmpTariffCost where tariff_id={0}", (object) ((Tariff) this.curObject).Tariff_id)).UniqueResult());
        if (this.dgvBase.Columns[e.ColumnIndex].Name == "nameColumn")
          ((Tariff) this.objectsList[e.RowIndex]).Tariff_name = e.Value.ToString();
        else if (int32_1 == 0)
        {
          if (this.dgvBase.Columns[e.ColumnIndex].Name == "numColumn")
          {
            try
            {
              IList list = this.session.CreateCriteria(typeof (Tariff)).Add((ICriterion) Restrictions.Eq("Tariff_num", (object) Convert.ToInt32(e.Value))).Add((ICriterion) Restrictions.Eq("Service", (object) this.curService)).Add((ICriterion) Restrictions.Eq("Manager", (object) this.manager)).List();
              int int32_2 = Convert.ToInt32(e.Value);
              if (list.Count > 0)
              {
                int num = (int) MessageBox.Show("Тариф с таким номером уже заведен!", "Внимание!", MessageBoxButtons.OK);
                this.isEdit = false;
                return;
              }
              ((Tariff) this.objectsList[e.RowIndex]).Tariff_num = int32_2;
              this.dgvBase.Refresh();
            }
            catch
            {
              int num = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
              this.isEdit = false;
              return;
            }
          }
          else if (this.dgvBase.Columns[e.ColumnIndex].Name == "CounterColumn")
          {
            if (Convert.ToInt32(e.Value) == -1)
              ((Tariff) this.objectsList[e.RowIndex]).Counter_id = new int?();
            else
              ((Tariff) this.objectsList[e.RowIndex]).Counter_id = new int?(Convert.ToInt32(e.Value));
          }
        }
        else
        {
          int num = (int) MessageBox.Show("Изменение невозможно! Существуют тарифы, заведенные на этот вариант.", "Внимание!", MessageBoxButtons.OK);
          this.isEdit = false;
          return;
        }
        if (this.newObject == null && this.curObject != null)
        {
          ((Tariff) this.curObject).Uname = Options.Login;
          ((Tariff) this.curObject).Dedit = DateTime.Now;
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
      try
      {
        if (Options.Company.Manager.BaseOrgId == this.manager.BaseOrgId)
        {
          if (!KvrplHelper.CheckProxy(67, 2, Options.Company, true))
            return;
        }
        else if (!KvrplHelper.CheckProxy(67, 2, (Company) null, true))
          return;
      }
      catch
      {
        if (!KvrplHelper.CheckProxy(67, 2, (Company) null, true))
          return;
      }
      object obj = this.session.CreateQuery(string.Format("select max(t.Tariff_num) from Tariff t where t.Service=:serv and t.Manager.BaseOrgId=:org")).SetEntity("serv", (object) this.curService).SetParameter<int>("org", this.Manager.BaseOrgId).UniqueResult();
      this.newObject = (object) new Tariff(this.curService, "Новый вариант", this.session.CreateSQLQuery("Select DBA.Gen_id('dcTariff',1)").UniqueResult<int>(), obj != null ? (int) obj + 1 : 1);
      ((Tariff) this.newObject).Manager = this.Manager;
      this.isEdit = true;
      base.tsbAdd_Click(sender, e);
    }

    protected override void tsbApplay_Click(object sender, EventArgs e)
    {
      try
      {
        if (Options.Company.Manager.BaseOrgId == this.manager.BaseOrgId)
        {
          if (!KvrplHelper.CheckProxy(67, 2, Options.Company, true))
            return;
        }
        else if (!KvrplHelper.CheckProxy(67, 2, (Company) null, true))
          return;
      }
      catch
      {
        if (!KvrplHelper.CheckProxy(67, 2, (Company) null, true))
          return;
      }
      this.session.Clear();
      this.dgvBase.EndEdit();
      if (!this.isEdit)
      {
        this.isEdit = true;
      }
      else
      {
        if (this.newObject != null)
        {
          ((Tariff) this.newObject).Uname = Options.Login;
          ((Tariff) this.newObject).Dedit = DateTime.Now;
        }
        else
        {
          int? nullable1 = ((Tariff) this.curObject).Counter_id;
          int num = 0;
          if (nullable1.GetValueOrDefault() == num && nullable1.HasValue)
          {
            Tariff curObject = (Tariff) this.curObject;
            nullable1 = new int?();
            int? nullable2 = nullable1;
            curObject.Counter_id = nullable2;
          }
        }
        base.tsbApplay_Click(sender, e);
      }
    }

    protected override void tsbDelete_Click(object sender, EventArgs e)
    {
      try
      {
        if (Options.Company.Manager.BaseOrgId == this.manager.BaseOrgId)
        {
          if (!KvrplHelper.CheckProxy(67, 2, Options.Company, true))
            return;
        }
        else if (!KvrplHelper.CheckProxy(67, 2, (Company) null, true))
          return;
      }
      catch
      {
        if (!KvrplHelper.CheckProxy(67, 2, (Company) null, true))
          return;
      }
      this.session.Clear();
      if (MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      bool flag = true;
      IList list = this.session.CreateQuery("from cmpTariffCost where tariff_id=:id").SetInt32("id", ((Tariff) this.curObject).Tariff_id).List();
      if (list.Count == 0)
        flag = true;
      foreach (cmpTariffCost cmpTariffCost in (IEnumerable) list)
      {
        DateTime? periodName;
        int num;
        if (cmpTariffCost.Dbeg < this.closedPeriod.Value.AddMonths(1))
        {
          periodName = cmpTariffCost.Period.PeriodName;
          if (!periodName.HasValue)
          {
            num = 1;
            goto label_19;
          }
        }
        periodName = cmpTariffCost.Period.PeriodName;
        if (periodName.HasValue)
        {
          periodName = cmpTariffCost.Period.PeriodName;
          num = periodName.Value <= this.closedPeriod.Value ? 1 : 0;
        }
        else
          num = 0;
label_19:
        if (num != 0)
          flag = false;
      }
      if (flag)
      {
        foreach (cmpTariffCost cmpTariffCost in (IEnumerable) list)
        {
          this.session.Delete((object) cmpTariffCost);
          this.session.Flush();
        }
        base.tsbDelete_Click(sender, e);
      }
      else
      {
        int num1 = (int) MessageBox.Show("Удаление невозможно! Существуют данные, ссылающиеся на эту запись.", "Внимание!", MessageBoxButtons.OK);
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
      this.Name = "UСVariant";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
