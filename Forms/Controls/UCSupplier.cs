// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.Controls.UCSupplier
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
  public class UCSupplier : UCBase
  {
    private IContainer components = (IContainer) null;
    private Company company;
    private Supplier curOrg;

    public Supplier CurOrg
    {
      get
      {
        return this.curOrg;
      }
      set
      {
        if (this.curOrg != value && this.tsbApplay.Enabled)
          this.tsbCancel_Click((object) null, (EventArgs) null);
        this.curOrg = value;
        this.GetList();
      }
    }

    public Company Company
    {
      get
      {
        return this.company;
      }
      set
      {
        if (this.company != value && this.tsbApplay.Enabled)
        {
          if (MessageBox.Show("Изменения не сохранены! Вернуться к предыдущей компании и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
          {
            this.tsbApplay_Click((object) null, (EventArgs) null);
            return;
          }
          this.tsbCancel_Click((object) null, (EventArgs) null);
        }
        this.company = value;
        this.GetList();
      }
    }

    public UCSupplier()
    {
      this.InitializeComponent();
      this.dgvBase.ContextMenuStrip = (ContextMenuStrip) null;
      DataGridViewColumn dataGridViewColumn1 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn1.Name = "Service";
      dataGridViewColumn1.HeaderText = "Услуга";
      ((DataGridViewComboBoxColumn) dataGridViewColumn1).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      dataGridViewColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgvBase.Columns.Add(dataGridViewColumn1);
      DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn2.Name = "Priority";
      dataGridViewColumn2.HeaderText = "Приоритет";
      this.dgvBase.Columns.Add(dataGridViewColumn2);
      DataGridViewColumn dataGridViewColumn3 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn3.Name = "Vat";
      dataGridViewColumn3.HeaderText = "НДС";
      ((DataGridViewComboBoxColumn) dataGridViewColumn3).DisplayMember = "Name";
      ((DataGridViewComboBoxColumn) dataGridViewColumn3).ValueMember = "ID";
      ((DataGridViewComboBoxColumn) dataGridViewColumn3).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      this.dgvBase.Columns.Add(dataGridViewColumn3);
      DataGridViewColumn dataGridViewColumn4 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn4.Name = "Recipient";
      dataGridViewColumn4.HeaderText = "Получатель для пеней";
      ((DataGridViewComboBoxColumn) dataGridViewColumn4).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      dataGridViewColumn4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgvBase.Columns.Add(dataGridViewColumn4);
      DataGridViewColumn dataGridViewColumn5 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn5.Name = "Perfomer";
      dataGridViewColumn5.HeaderText = "Исполнитель для пеней";
      ((DataGridViewComboBoxColumn) dataGridViewColumn5).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      dataGridViewColumn5.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgvBase.Columns.Add(dataGridViewColumn5);
      ((DataGridViewComboBoxColumn) this.dgvBase.Columns["Service"]).DisplayMember = "ServiceName";
      ((DataGridViewComboBoxColumn) this.dgvBase.Columns["Service"]).ValueMember = "ServiceId";
      ((DataGridViewComboBoxColumn) this.dgvBase.Columns["Recipient"]).DisplayMember = "NameOrgMin";
      ((DataGridViewComboBoxColumn) this.dgvBase.Columns["Recipient"]).ValueMember = "BaseOrgId";
      ((DataGridViewComboBoxColumn) this.dgvBase.Columns["Perfomer"]).DisplayMember = "NameOrgMin";
      ((DataGridViewComboBoxColumn) this.dgvBase.Columns["Perfomer"]).ValueMember = "BaseOrgId";
      if (Options.ViewEdit)
      {
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 5, "Пользователь", "UName", 100, true);
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 6, "Дата редактирования", "DEdit", 100, true);
      }
      this.MySettings.GridName = "CmpSupplier";
      this.dgvBase.CurrentCellDirtyStateChanged += new EventHandler(this.dgvBase_CurrentCellDirtyStateChanged);
    }

    protected override void GetList()
    {
      if (this.curOrg == null)
        return;
      this.objectsList.Clear();
      this.objectsList = this.session.CreateQuery(string.Format("select sp from CmpSupplier sp,Service s where sp.Service.ServiceId=s.ServiceId and sp.SupplierOrg=:org and sp.Company.CompanyId={0} order by {1}", (object) this.company.CompanyId, (object) Options.SortService)).SetEntity("org", (object) this.curOrg).List();
      foreach (CmpSupplier objects in (IEnumerable) this.objectsList)
      {
        if (objects.SupplierPeni != null)
        {
          objects.RecipientPeni = objects.SupplierPeni.Recipient;
          objects.PerfomerPeni = objects.SupplierPeni.Perfomer;
        }
      }
      if (this.objectsList.Count == 0)
        this.curObject = (object) null;
      this.dgvBase.RowCount = this.objectsList.Count;
      this.LoadPerfomersList();
      this.dgvBase.Refresh();
      this.SelectRow();
    }

    public void FillGrid()
    {
      foreach (DataGridViewRow row in (IEnumerable) this.dgvBase.Rows)
      {
        IList<BaseOrg> baseOrgList = this.session.CreateQuery(string.Format("select new BaseOrg(BaseOrgId,NameOrgMin) from BaseOrg where BaseOrgId in (SELECT IDBASEORG FROM Postaver) order by NameOrgMin")).List<BaseOrg>();
        DataGridViewComboBoxCell viewComboBoxCell1 = new DataGridViewComboBoxCell() { DisplayStyleForCurrentCellOnly = true, ValueMember = "BaseOrgId", DisplayMember = "NameOrgMin", DataSource = (object) baseOrgList };
        DataGridViewComboBoxCell viewComboBoxCell2 = new DataGridViewComboBoxCell() { DisplayStyleForCurrentCellOnly = true, ValueMember = "BaseOrgId", DisplayMember = "NameOrgMin", DataSource = (object) baseOrgList };
        if (((CmpSupplier) this.objectsList[row.Index]).RecipientId == null);
        if (((CmpSupplier) this.objectsList[row.Index]).RecipientPeniId == null);
      }
    }

    private void LoadPerfomersList()
    {
      foreach (DataGridViewRow row in (IEnumerable) this.dgvBase.Rows)
      {
        int num = 0;
        if (((CmpSupplier) this.objectsList[row.Index]).SupplierPeni != null)
          num = ((CmpSupplier) this.objectsList[row.Index]).SupplierPeni.Recipient.BaseOrgId;
        IList<BaseOrg> baseOrgList1 = this.session.CreateQuery(string.Format("select distinct s.Perfomer from Supplier s where s.Perfomer.BaseOrgId<>0 and s.Recipient.BaseOrgId={0} ", (object) num)).List<BaseOrg>();
        if (num == 0)
        {
          baseOrgList1.Insert(0, new BaseOrg(0, ""));
        }
        else
        {
          IList<BaseOrg> baseOrgList2 = this.session.CreateQuery(string.Format("select distinct s.Perfomer from Supplier s where s.Perfomer.BaseOrgId=0 and s.Recipient.BaseOrgId={0} ", (object) num)).List<BaseOrg>();
          if (baseOrgList2.Count > 0)
            baseOrgList1.Insert(0, baseOrgList2[0]);
        }
        row.Cells["Perfomer"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "BaseOrgId",
          DisplayMember = "NameOrgMinDop",
          DataSource = (object) baseOrgList1
        };
        if (((CmpSupplier) this.objectsList[row.Index]).SupplierPeni != null)
          row.Cells["Perfomer"].Value = (object) ((CmpSupplier) this.objectsList[row.Index]).SupplierPeni.Perfomer.BaseOrgId;
      }
    }

    protected override void dgvBase_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.objectsList.Count <= 0)
        return;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "Service" && ((CmpSupplier) this.objectsList[e.RowIndex]).Service != null && (int) ((CmpSupplier) this.objectsList[e.RowIndex]).Service.ServiceId > 0)
        e.Value = (object) ((CmpSupplier) this.objectsList[e.RowIndex]).Service.ServiceId;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Priority")
        e.Value = (object) ((CmpSupplier) this.objectsList[e.RowIndex]).Priority;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Vat")
        e.Value = (object) ((CmpSupplier) this.objectsList[e.RowIndex]).Vat;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "UName")
        e.Value = (object) ((CmpSupplier) this.objectsList[e.RowIndex]).UName;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "DEdit")
        e.Value = (object) ((CmpSupplier) this.objectsList[e.RowIndex]).DEdit.ToShortDateString();
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Recipient")
      {
        if (((CmpSupplier) this.objectsList[e.RowIndex]).RecipientPeni != null)
          e.Value = (object) ((CmpSupplier) this.objectsList[e.RowIndex]).RecipientPeni.BaseOrgId;
      }
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Perfomer")
      {
        if (((CmpSupplier) this.objectsList[e.RowIndex]).PerfomerPeni != null)
          e.Value = (object) ((CmpSupplier) this.objectsList[e.RowIndex]).PerfomerPeni.BaseOrgId;
      }
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "RecipientId")
        e.Value = ((CmpSupplier) this.objectsList[e.RowIndex]).RecipientId == null ? (object) 0 : (object) ((CmpSupplier) this.objectsList[e.RowIndex]).RecipientId.BaseOrgId;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "RecipientPeniId")
        e.Value = ((CmpSupplier) this.objectsList[e.RowIndex]).RecipientPeniId == null ? (object) 0 : (object) ((CmpSupplier) this.objectsList[e.RowIndex]).RecipientPeniId.BaseOrgId;
    }

    protected override void dgvBase_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      try
      {
        if (this.dgvBase.Columns[e.ColumnIndex].Name == "Service" && e.Value != null)
        {
          try
          {
            if (this.newObject != null && e.RowIndex == this.objectsList.Count - 1)
              ((CmpSupplier) this.objectsList[e.RowIndex]).Service = this.session.Get<Service>((object) Convert.ToInt16(e.Value));
          }
          catch
          {
          }
        }
        if (this.dgvBase.Columns[e.ColumnIndex].Name == "Priority" && e.Value != null)
          ((CmpSupplier) this.objectsList[e.RowIndex]).Priority = Convert.ToInt32(e.Value);
        if (this.dgvBase.Columns[e.ColumnIndex].Name == "Vat" && e.Value != null)
          ((CmpSupplier) this.objectsList[e.RowIndex]).Vat = Convert.ToInt32(e.Value);
        else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Recipient")
          ((CmpSupplier) this.objectsList[e.RowIndex]).RecipientPeni = this.session.Get<BaseOrg>((object) Convert.ToInt32(e.Value));
        else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Perfomer")
          ((CmpSupplier) this.objectsList[e.RowIndex]).PerfomerPeni = this.session.Get<BaseOrg>((object) Convert.ToInt32(e.Value));
        else if (this.dgvBase.Columns[e.ColumnIndex].Name == "RecipientId")
          ((CmpSupplier) this.objectsList[e.RowIndex]).RecipientId = this.session.Get<BaseOrg>((object) Convert.ToInt32(e.Value ?? (object) 0));
        else if (this.dgvBase.Columns[e.ColumnIndex].Name == "RecipientPeniId")
          ((CmpSupplier) this.objectsList[e.RowIndex]).RecipientPeniId = this.session.Get<BaseOrg>((object) Convert.ToInt32(e.Value ?? (object) 0));
      }
      catch
      {
        int num = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
      }
    }

    protected override void tsbAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, this.company, true))
        return;
      this.newObject = (object) new CmpSupplier();
      ((CmpSupplier) this.newObject).SupplierOrg = this.curOrg;
      ((CmpSupplier) this.newObject).Company = this.company;
      ((CmpSupplier) this.newObject).Priority = 1;
      base.tsbAdd_Click(sender, e);
      this.FillGrid();
    }

    protected override void tsbApplay_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, this.company, true))
        return;
      this.dgvBase.EndEdit();
      Supplier supplier = new Supplier();
      try
      {
        supplier = this.session.CreateQuery(string.Format("from Supplier where Recipient.BaseOrgId={0} and Perfomer.BaseOrgId={1}", (object) Convert.ToInt32(this.dgvBase.CurrentRow.Cells["Recipient"].Value), (object) Convert.ToInt32(this.dgvBase.CurrentRow.Cells["Perfomer"].Value))).List<Supplier>()[0];
      }
      catch
      {
        int num = (int) MessageBox.Show("Пара получатель-исполнитель не найдена", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      if (this.newObject != null)
      {
        if (((CmpSupplier) this.newObject).Service == null || (int) ((CmpSupplier) this.newObject).Service.ServiceId == 0)
        {
          int num = (int) MessageBox.Show("Выберите услугу!", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        ((CmpSupplier) this.newObject).UName = Options.Login;
        ((CmpSupplier) this.newObject).DEdit = DateTime.Now;
        if (this.dgvBase.CurrentRow.Cells["Recipient"].Value == null && this.dgvBase.CurrentRow.Cells["Perfomer"].Value == null)
          ((CmpSupplier) this.newObject).SupplierPeni = (Supplier) null;
        else
          ((CmpSupplier) this.newObject).SupplierPeni = supplier;
        if (((CmpSupplier) this.newObject).RecipientId == null)
          ((CmpSupplier) this.newObject).RecipientId = this.session.Get<BaseOrg>((object) Convert.ToInt32(0));
        if (((CmpSupplier) this.newObject).RecipientPeniId == null)
          ((CmpSupplier) this.newObject).RecipientPeniId = this.session.Get<BaseOrg>((object) Convert.ToInt32(0));
      }
      else
      {
        ((CmpSupplier) this.curObject).UName = Options.Login;
        ((CmpSupplier) this.curObject).DEdit = DateTime.Now;
        if (this.dgvBase.CurrentRow.Cells["Recipient"].Value == null && this.dgvBase.CurrentRow.Cells["Perfomer"].Value == null)
          ((CmpSupplier) this.curObject).SupplierPeni = (Supplier) null;
        else
          ((CmpSupplier) this.curObject).SupplierPeni = supplier;
        if (((CmpSupplier) this.curObject).RecipientId == null)
          ((CmpSupplier) this.curObject).RecipientId = this.session.Get<BaseOrg>((object) Convert.ToInt32(0));
        if (((CmpSupplier) this.curObject).RecipientPeniId == null)
          ((CmpSupplier) this.curObject).RecipientPeniId = this.session.Get<BaseOrg>((object) Convert.ToInt32(0));
      }
      base.tsbApplay_Click(sender, e);
    }

    protected override void tsbDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, this.company, true))
        return;
      if (this.curObject != null)
      {
        this.Cursor = Cursors.WaitCursor;
        IList list = this.session.CreateSQLQuery(string.Format("select first client_id from dba.lsBalance where client_id in (select client_id from lsClient where company_id={2}) and service_id in (select service_id from dcService where root={0}) and supplier_id={1}", (object) ((CmpSupplier) this.curObject).Service.ServiceId, (object) ((CmpSupplier) this.curObject).SupplierOrg.SupplierId, (object) ((CmpSupplier) this.curObject).Company.CompanyId)).List();
        this.Cursor = Cursors.Default;
        if (list.Count > 0)
        {
          int num = (int) MessageBox.Show("Невозможно удалить запись. По поставщику есть начисления", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return;
        }
      }
      base.tsbDelete_Click(sender, e);
    }

    private void dgvBase_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (!this.dgvBase.IsCurrentCellDirty || this.dgvBase.CurrentCell.ColumnIndex != this.dgvBase.Rows[this.dgvBase.CurrentRow.Index].Cells["Recipient"].ColumnIndex)
        return;
      this.dgvBase.CommitEdit(DataGridViewDataErrorContexts.Commit);
      IList<BaseOrg> baseOrgList1 = this.session.CreateQuery(string.Format("select distinct s.Perfomer from Supplier s where s.Perfomer.BaseOrgId<>0 and s.Recipient.BaseOrgId={0} ", (object) Convert.ToInt32(this.dgvBase.CurrentRow.Cells["Recipient"].Value))).List<BaseOrg>();
      if (Convert.ToInt32(this.dgvBase.CurrentRow.Cells["Recipient"].Value) == 0)
      {
        baseOrgList1.Insert(0, new BaseOrg(0, ""));
      }
      else
      {
        IList<BaseOrg> baseOrgList2 = this.session.CreateQuery(string.Format("select distinct s.Perfomer from Supplier s where s.Perfomer.BaseOrgId=0 and s.Recipient.BaseOrgId={0}", (object) Convert.ToInt32(this.dgvBase.CurrentRow.Cells["Recipient"].Value))).List<BaseOrg>();
        if (baseOrgList2.Count > 0)
          baseOrgList1.Insert(0, baseOrgList2[0]);
      }
      this.dgvBase.CurrentRow.Cells["Perfomer"] = (DataGridViewCell) new DataGridViewComboBoxCell()
      {
        DisplayStyleForCurrentCellOnly = true,
        ValueMember = "BaseOrgId",
        DisplayMember = "NameOrgMinDop",
        DataSource = (object) baseOrgList1
      };
      this.dgvBase.CurrentRow.Cells["Perfomer"].Value = (object) baseOrgList1[0].BaseOrgId;
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
      this.Name = "UCSupplier";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
