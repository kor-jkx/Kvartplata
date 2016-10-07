// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.Controls.UCCmpReceipt
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using System;
using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Kvartplata.Forms.Controls
{
  public class UCCmpReceipt : UCBase
  {
    private IContainer components = (IContainer) null;
    private short company_id;

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

    public UCCmpReceipt()
    {
      this.InitializeComponent();
      this.dgvBase.ContextMenuStrip = (ContextMenuStrip) null;
      DataGridViewColumn dataGridViewColumn1 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn1.Name = "Receipt_id";
      dataGridViewColumn1.HeaderText = "Квитанция";
      ((DataGridViewComboBoxColumn) dataGridViewColumn1).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      dataGridViewColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      ((DataGridViewComboBoxColumn) dataGridViewColumn1).DisplayMember = "ReceiptName";
      ((DataGridViewComboBoxColumn) dataGridViewColumn1).ValueMember = "ReceiptId";
      this.dgvBase.Columns.Add(dataGridViewColumn1);
      DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn2.Name = "Supplier_id";
      dataGridViewColumn2.HeaderText = "Поставщик";
      ((DataGridViewComboBoxColumn) dataGridViewColumn2).DisplayMember = "NAMEORG";
      ((DataGridViewComboBoxColumn) dataGridViewColumn2).ValueMember = "IDBASEORG";
      ((DataGridViewComboBoxColumn) dataGridViewColumn2).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      dataGridViewColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgvBase.Columns.Add(dataGridViewColumn2);
      DataGridViewColumn dataGridViewColumn3 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      dataGridViewColumn3.Name = "PrintShow";
      dataGridViewColumn3.HeaderText = "Наименование в квитанции";
      this.dgvBase.Columns.Add(dataGridViewColumn3);
      DataGridViewColumn dataGridViewColumn4 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn4.Name = "Bank_id";
      dataGridViewColumn4.HeaderText = "Банк";
      ((DataGridViewComboBoxColumn) dataGridViewColumn4).DisplayMember = "BankName";
      ((DataGridViewComboBoxColumn) dataGridViewColumn4).ValueMember = "BankId";
      ((DataGridViewComboBoxColumn) dataGridViewColumn4).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      dataGridViewColumn4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgvBase.Columns.Add(dataGridViewColumn4);
      DataGridViewColumn dataGridViewColumn5 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn5.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      dataGridViewColumn5.Name = "Account";
      dataGridViewColumn5.HeaderText = "Счет";
      this.dgvBase.Columns.Add(dataGridViewColumn5);
      DataGridViewColumn dataGridViewColumn6 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn6.Name = "Seller";
      dataGridViewColumn6.HeaderText = "Продавец";
      ((DataGridViewComboBoxColumn) dataGridViewColumn6).DisplayMember = "NAMEORG";
      ((DataGridViewComboBoxColumn) dataGridViewColumn6).ValueMember = "IDBASEORG";
      ((DataGridViewComboBoxColumn) dataGridViewColumn6).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      dataGridViewColumn6.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgvBase.Columns.Add(dataGridViewColumn6);
      DataGridViewColumn dataGridViewColumn7 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn7.Name = "Consignor";
      dataGridViewColumn7.HeaderText = "Грузоотправитель";
      ((DataGridViewComboBoxColumn) dataGridViewColumn7).DisplayMember = "NAMEORG";
      ((DataGridViewComboBoxColumn) dataGridViewColumn7).ValueMember = "IDBASEORG";
      ((DataGridViewComboBoxColumn) dataGridViewColumn7).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      dataGridViewColumn7.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgvBase.Columns.Add(dataGridViewColumn7);
      DataGridViewColumn dataGridViewColumn8 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn8.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      dataGridViewColumn8.Name = "Recipient";
      dataGridViewColumn8.HeaderText = "Код компании для выгрузки в банк";
      this.dgvBase.Columns.Add(dataGridViewColumn8);
      if (Options.ViewEdit)
      {
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 7, "Пользователь", "UName", 100, true);
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 8, "Дата редактирования", "DEdit", 100, true);
      }
      this.MySettings.GridName = "CmpReceipt";
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
      this.objectsList.Clear();
      this.objectsList = this.session.CreateQuery(string.Format("from CmpReceipt where company_id={0} order by receipt_id", (object) this.company_id)).List();
      foreach (object objects in (IEnumerable) this.objectsList)
        this.session.Refresh(objects);
      this.dgvBase.RowCount = this.objectsList.Count;
      this.dgvBase.Refresh();
      this.SelectRow();
    }

    protected override void dgvBase_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.objectsList.Count <= 0)
        return;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "Supplier_id")
        e.Value = (object) ((CmpReceipt) this.objectsList[e.RowIndex]).SupplierId;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Receipt_id")
        e.Value = (object) ((CmpReceipt) this.objectsList[e.RowIndex]).ReceiptId;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Bank_id" && ((CmpReceipt) this.objectsList[e.RowIndex]).Bank != null)
        e.Value = (object) ((CmpReceipt) this.objectsList[e.RowIndex]).Bank.BankId;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Account")
        e.Value = (object) ((CmpReceipt) this.objectsList[e.RowIndex]).Account;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Seller" && ((CmpReceipt) this.objectsList[e.RowIndex]).Seller != null)
        e.Value = (object) ((CmpReceipt) this.objectsList[e.RowIndex]).Seller.BaseOrgId;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Consignor" && ((CmpReceipt) this.objectsList[e.RowIndex]).Consignor != null)
        e.Value = (object) ((CmpReceipt) this.objectsList[e.RowIndex]).Consignor.BaseOrgId;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Recipient")
        e.Value = (object) ((CmpReceipt) this.objectsList[e.RowIndex]).RecipientId;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "PrintShow")
        e.Value = (object) ((CmpReceipt) this.objectsList[e.RowIndex]).PrintShow;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "UName")
        e.Value = (object) ((CmpReceipt) this.objectsList[e.RowIndex]).UName;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "DEdit")
        e.Value = (object) ((CmpReceipt) this.objectsList[e.RowIndex]).DEdit.ToShortDateString();
    }

    protected override void dgvBase_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      try
      {
        if (this.dgvBase.Columns[e.ColumnIndex].Name == "Receipt_id")
          ((CmpReceipt) this.objectsList[e.RowIndex]).ReceiptId = Convert.ToInt16(e.Value);
        else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Supplier_id" && e.Value != null)
          ((CmpReceipt) this.objectsList[e.RowIndex]).SupplierId = Convert.ToInt32(e.Value);
        else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Seller" && e.Value != null)
          ((CmpReceipt) this.objectsList[e.RowIndex]).Seller = this.session.Get<BaseOrg>((object) Convert.ToInt32(e.Value));
        else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Consignor" && e.Value != null)
          ((CmpReceipt) this.objectsList[e.RowIndex]).Consignor = this.session.Get<BaseOrg>((object) Convert.ToInt32(e.Value));
        else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Bank_id" && e.Value != null)
        {
          Bank bank = this.session.Get<Bank>((object) Convert.ToInt32(e.Value));
          ((CmpReceipt) this.objectsList[e.RowIndex]).Bank = bank;
        }
        else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Account")
          ((CmpReceipt) this.objectsList[e.RowIndex]).Account = e.Value.ToString();
        else if (this.dgvBase.Columns[e.ColumnIndex].Name == "PrintShow")
          ((CmpReceipt) this.objectsList[e.RowIndex]).PrintShow = e.Value.ToString();
        else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Recipient")
          ((CmpReceipt) this.objectsList[e.RowIndex]).RecipientId = Convert.ToInt32(e.Value);
      }
      catch
      {
        int num = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
        return;
      }
      ((CmpReceipt) this.objectsList[e.RowIndex]).UName = Options.Login;
      ((CmpReceipt) this.objectsList[e.RowIndex]).DEdit = DateTime.Now;
    }

    protected override void tsbAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, this.session.Get<Company>((object) this.Company_id), true))
        return;
      this.newObject = (object) new CmpReceipt();
      ((CmpReceipt) this.newObject).CompanyId = this.Company_id;
      ((CmpReceipt) this.newObject).Bank = this.session.Get<Bank>((object) 0);
      base.tsbAdd_Click(sender, e);
    }

    protected override void tsbApplay_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, this.session.Get<Company>((object) this.Company_id), true))
        return;
      this.dgvBase.EndEdit();
      if (this.newObject != null)
      {
        if ((int) ((CmpReceipt) this.newObject).ReceiptId == 0)
        {
          int num = (int) MessageBox.Show("Выберите квитанцию.", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        if (((CmpReceipt) this.newObject).Account == null)
        {
          int num = (int) MessageBox.Show("Введите номер счета.", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        if (((CmpReceipt) this.newObject).SupplierId == 0)
        {
          int num = (int) MessageBox.Show("Выберите поставщика.", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        if (!Regex.IsMatch(((CmpReceipt) this.newObject).Account, "^[0-9]{20,20}$"))
        {
          int num = (int) MessageBox.Show("Счет должен состоять только из цифр, и иметь 20 символов.", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        int recipientId = ((CmpReceipt) this.newObject).RecipientId;
        if (false)
          ((CmpReceipt) this.newObject).RecipientId = 0;
        ((CmpReceipt) this.newObject).UName = Options.Login;
        ((CmpReceipt) this.newObject).DEdit = DateTime.Now;
      }
      else if (this.curObject != null)
      {
        if (!Regex.IsMatch(((CmpReceipt) this.curObject).Account, "^[0-9]{20,20}$"))
        {
          int num = (int) MessageBox.Show("Счет должен состоять только из цифр, и иметь 20 символов.", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        ((CmpReceipt) this.curObject).UName = Options.Login;
        ((CmpReceipt) this.curObject).DEdit = DateTime.Now;
      }
      base.tsbApplay_Click(sender, e);
    }

    protected override void tsbDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, this.session.Get<Company>((object) this.Company_id), true) || this.curObject == null || MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      if (Convert.ToInt32(this.session.CreateQuery("select count(*) from ServiceParam where Receipt_id=:rid and Company_id=:cid").SetInt16("rid", ((CmpReceipt) this.curObject).ReceiptId).SetInt16("cid", ((CmpReceipt) this.curObject).CompanyId).UniqueResult()) == 0)
      {
        base.tsbDelete_Click(sender, e);
      }
      else
      {
        int num = (int) MessageBox.Show("Невозможно удалить запись, так как есть связанные с ней данные!", "Внимание!", MessageBoxButtons.OK);
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
      this.components = (IContainer) new Container();
    }
  }
}
