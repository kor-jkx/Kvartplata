// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.Controls.UCNorm
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
  public class UCNorm : UCBase
  {
    private bool isEdit = false;
    private IContainer components = (IContainer) null;
    private BaseOrg manager;
    private Service curService;
    public DateTime? closedPeriod;

    public Norm CurNorm
    {
      get
      {
        return (Norm) this.curObject;
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

    public UCNorm()
    {
      this.InitializeComponent();
      this.dgvBase.ContextMenuStrip = (ContextMenuStrip) null;
      DataGridViewColumn dataGridViewColumn1 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn1.Name = "numColumn";
      dataGridViewColumn1.HeaderText = "Номер норматива";
      dataGridViewColumn1.Width = 80;
      this.dgvBase.Columns.Add(dataGridViewColumn1);
      if (Options.ViewEdit)
      {
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 1, "Пользователь", "UName", 80, true);
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 2, "Дата редактирования", "DEdit", 100, true);
      }
      DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn2.Name = "nameColumn";
      dataGridViewColumn2.HeaderText = "Наименование";
      dataGridViewColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgvBase.Columns.Insert(1, dataGridViewColumn2);
      this.MySettings.GridName = "Norm";
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
        this.objectsList = this.session.CreateQuery(string.Format("from Norm where Manager.BaseOrgId={1} and service_id = {0} order by Norm_num", (object) this.curService.ServiceId, (object) this.manager.BaseOrgId)).List();
      else
        this.objectsList = this.session.CreateQuery(string.Format("from Norm where service_id = {0} order by Norm_num", (object) this.curService.ServiceId)).List();
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
        e.Value = (object) ((Norm) this.objectsList[e.RowIndex]).Norm_name;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "numColumn")
        e.Value = (object) ((Norm) this.objectsList[e.RowIndex]).Norm_num;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "UName")
        e.Value = (object) ((Norm) this.objectsList[e.RowIndex]).Uname;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "DEdit")
        e.Value = (object) ((Norm) this.objectsList[e.RowIndex]).Dedit.ToShortDateString();
    }

    protected override void dgvBase_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      int int32_1 = Convert.ToInt32(this.session.CreateQuery(string.Format("select count(*) from CmpNorm where Norm.Norm_id={0}", (object) ((Norm) this.curObject).Norm_id)).UniqueResult());
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "nameColumn")
        ((Norm) this.objectsList[e.RowIndex]).Norm_name = e.Value.ToString();
      else if (int32_1 == 0)
      {
        if (this.dgvBase.Columns[e.ColumnIndex].Name == "numColumn")
        {
          try
          {
            IList list = this.session.CreateCriteria(typeof (Norm)).Add((ICriterion) Restrictions.Eq("Norm_num", (object) Convert.ToInt32(e.Value))).Add((ICriterion) Restrictions.Eq("Service", (object) this.curService)).Add((ICriterion) Restrictions.Eq("Manager", (object) this.manager)).List();
            int int32_2 = Convert.ToInt32(e.Value);
            if (list.Count > 0)
            {
              int num = (int) MessageBox.Show("Норматив с таким номером уже заведен!", "Внимание!", MessageBoxButtons.OK);
              this.isEdit = false;
              return;
            }
            ((Norm) this.objectsList[e.RowIndex]).Norm_num = int32_2;
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
        int num = (int) MessageBox.Show("Изменение невозможно! Существуют данные, заведенные на этот норматив.", "Внимание!", MessageBoxButtons.OK);
        this.isEdit = false;
        return;
      }
      if (this.newObject == null && this.curObject != null)
      {
        ((Norm) this.curObject).Uname = Options.Login;
        ((Norm) this.curObject).Dedit = DateTime.Now;
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
      object obj = this.session.CreateQuery(string.Format("select max(t.Norm_num) from Norm t where t.Service=:serv and t.Manager.BaseOrgId=:org")).SetEntity("serv", (object) this.curService).SetParameter<int>("org", this.Manager.BaseOrgId).UniqueResult();
      this.newObject = (object) new Norm(this.curService, "Новый норматив", this.session.CreateSQLQuery("Select DBA.Gen_id('dcNorm',1)").UniqueResult<int>(), obj != null ? (int) obj + 1 : 1);
      ((Norm) this.newObject).Manager = this.Manager;
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
          ((Norm) this.newObject).Uname = Options.Login;
          ((Norm) this.newObject).Dedit = DateTime.Now;
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
      if (MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      bool flag = true;
      IList list = this.session.CreateQuery("from CmpNorm where Norm.Norm_id=:id").SetInt32("id", ((Norm) this.curObject).Norm_id).List();
      if (list.Count == 0)
        flag = true;
      foreach (CmpNorm cmpNorm in (IEnumerable) list)
      {
        DateTime? periodName;
        int num;
        if (cmpNorm.Dbeg < this.closedPeriod.Value.AddMonths(1))
        {
          periodName = cmpNorm.Period.PeriodName;
          if (!periodName.HasValue)
          {
            num = 1;
            goto label_19;
          }
        }
        periodName = cmpNorm.Period.PeriodName;
        if (periodName.HasValue)
        {
          periodName = cmpNorm.Period.PeriodName;
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
        foreach (CmpNorm cmpNorm in (IEnumerable) list)
        {
          this.session.Delete((object) cmpNorm);
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
      this.Name = "UCNorm";
      this.Size = new Size(574, 478);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
