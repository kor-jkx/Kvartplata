// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmSelectAccountForOverhaul
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmSelectAccountForOverhaul : FrmBaseForm
  {
    private IContainer components = (IContainer) null;
    private readonly Company _currentCompany;
    private DataGridView dgvBase;
    private DataGridViewTextBoxColumn ClientId;
    private DataGridViewTextBoxColumn FullAddress;
    private DataGridViewTextBoxColumn UName;
    private DataGridViewTextBoxColumn DEdit;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private ToolStrip toolStrip1;
    private ToolStripButton tsbSelectAll;
    private ToolStripButton tsbAdd;

    public IList<LsClient> lsClients { get; set; }

    public ISession session { get; set; }

    public FrmSelectAccountForOverhaul(Company currentCompany)
    {
      this._currentCompany = currentCompany;
      this.InitializeComponent();
      this.session = Domain.CurrentSession;
      this.GetData(currentCompany);
    }

    private void GetData(Company currentCompany)
    {
      this.lsClients = currentCompany == null ? this.session.CreateSQLQuery("select distinct ls.* from lsClient ls inner join lsService serv on serv.Client_id=ls.Client_id left join (select Company_id, Client_id from cmpSupplierClient where Supplier_id=-39999859) cmp on cmp.Client_id=ls.Client_id where serv.Service_id=150 and cmp.Company_id is null ").AddEntity(typeof (LsClient)).List<LsClient>() : this.session.CreateSQLQuery("select distinct ls.* from lsClient ls inner join lsService serv on serv.Client_id=ls.Client_id left join (select Company_id, Client_id from cmpSupplierClient where Supplier_id=-39999859) cmp on cmp.Client_id=ls.Client_id where ls.Company_id=:cmp and serv.Service_id=150 and cmp.Company_id is null ").AddEntity(typeof (LsClient)).SetParameter<short>("cmp", currentCompany.CompanyId).List<LsClient>();
      this.dgvBase.AutoGenerateColumns = false;
      this.dgvBase.DataSource = (object) null;
      if (this.lsClients.Count<LsClient>() <= 0)
        return;
      foreach (LsClient lsClient in (IEnumerable<LsClient>) this.lsClients)
      {
        Address address = (Address) this.session.CreateQuery(string.Format("select new Address(c.ClientId,d.NameStr,h.NHome,h.HomeKorp,f.NFlat,c.SurFlat) from Home h, Str d, Flat f,LsClient c where h.Str=d and c.Home=h and c.Flat=f and c.ClientId={0} ", (object) lsClient.ClientId)).List()[0];
        lsClient.FullAddress = string.Format("{0} д.{1} {2} кв.{3} {4}", (object) address.Str, (object) address.Number, (object) (address.Korp == "" || address.Korp == "0" || address.Korp == null ? "" : "к." + address.Korp), (object) address.Flat, (object) (address.SurFlat == "" || address.SurFlat == "0" || address.SurFlat == null ? "" : "комн." + address.SurFlat));
      }
      this.dgvBase.DataSource = (object) this.lsClients.OrderBy<LsClient, int>((Func<LsClient, int>) (x => x.Flat.NFlat.Length)).ToList<LsClient>();
      this.dgvBase.Refresh();
    }

    private void FrmSelectAccountForOverhaul_Load(object sender, EventArgs e)
    {
    }

    private void tsbSelectAll_Click(object sender, EventArgs e)
    {
      if (this.dgvBase.AreAllCellsSelected(true))
      {
        this.dgvBase.ClearSelection();
        this.tsbSelectAll.Text = "Выделить все";
      }
      else
      {
        this.dgvBase.SelectAll();
        this.tsbSelectAll.Text = "Сбросить выделение";
      }
    }

    private void tsbAdd_Click(object sender, EventArgs e)
    {
      IList<LsClient> lsClientList1 = (IList<LsClient>) new List<LsClient>();
      IList<LsClient> lsClientList2 = (IList<LsClient>) new List<LsClient>();
      string connectionString = string.Format("Provider={4};Eng={0};Uid={1};Pwd={2}; Links={3}", (object) Options.BaseName, (object) Options.Login, (object) Options.Pwd, (object) "tcpip{}", (object) Options.Provider);
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgvBase.SelectedRows)
      {
        lsClientList1.Add((LsClient) selectedRow.DataBoundItem);
        lsClientList2.Add((LsClient) selectedRow.DataBoundItem);
      }
      if (Options.City == 9);
      foreach (LsClient lsClient in (IEnumerable<LsClient>) lsClientList2)
      {
        this.session.BeginTransaction();
        Transfer transfer = this.session.CreateQuery("from Transfer tf where tf.Company.CompanyId=:cmp").SetParameter<short>("cmp", lsClient.Company.CompanyId).UniqueResult<Transfer>();
        string str = "";
        object obj1 = this.session.CreateQuery("select max(ls.SupplierClientId) from SupplierClient ls where ls.Supplier.BaseOrgId=-39999859 and ls.SupplierClientId between :beg and :end").SetParameter<int?>("beg", transfer.OhlBeg).SetParameter<int?>("end", transfer.OhlEnd).UniqueResult();
        int? nullable;
        object obj2;
        if (obj1 == null)
        {
          nullable = transfer.OhlBeg;
          int num = 10;
          obj2 = (object) (nullable.HasValue ? new int?(nullable.GetValueOrDefault() + num) : new int?());
        }
        else
          obj2 = (object) (Convert.ToInt32(obj1.ToString().Substring(0, obj1.ToString().Length - 1) + "0") + 10);
        int num1 = (int) obj2 % 100000000 / 10000000 * 1 + (int) obj2 % 10000000 / 1000000 * 2 + (int) obj2 % 1000000 / 100000 * 3 + (int) obj2 % 100000 / 10000 * 4 + (int) obj2 % 10000 / 1000 * 5 + (int) obj2 % 1000 / 100 * 6 + (int) obj2 % 100 / 10 * 7;
        object obj3 = (object) ((int) obj2 + Convert.ToInt32(num1.ToString().Remove(0, num1.ToString().Length - 1)));
        int int32 = Convert.ToInt32(obj3);
        nullable = transfer.OhlEnd;
        int valueOrDefault = nullable.GetValueOrDefault();
        if (int32 > valueOrDefault && nullable.HasValue)
        {
          int num2 = (int) MessageBox.Show("Исчерпан лимит лицевых для компании!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
          str = obj3.ToString();
        string commandText = string.Format("INSERT INTO DBA.cmpSupplierClient(Company_id, Client_id, Supplier_id, Supplier_client, Uname, Dedit) VALUES({0}, {1}, {2}, {3}, '{4}', getdate())", (object) lsClient.Company.CompanyId, (object) lsClient.ClientId, (object) -39999859, (object) str, (object) Options.Login);
        try
        {
          OleDbHelper.ExecuteNonQuery(connectionString, CommandType.Text, commandText, 1000);
          this.session.Transaction.Commit();
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
          this.session.Transaction.Rollback();
        }
      }
      if (lsClientList1.Count <= 0)
        return;
      this.GetData(this._currentCompany);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.dgvBase = new DataGridView();
      this.ClientId = new DataGridViewTextBoxColumn();
      this.FullAddress = new DataGridViewTextBoxColumn();
      this.UName = new DataGridViewTextBoxColumn();
      this.DEdit = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
      this.toolStrip1 = new ToolStrip();
      this.tsbSelectAll = new ToolStripButton();
      this.tsbAdd = new ToolStripButton();
      ((ISupportInitialize) this.dgvBase).BeginInit();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      this.dgvBase.AllowUserToAddRows = false;
      this.dgvBase.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      this.dgvBase.BackgroundColor = Color.AliceBlue;
      this.dgvBase.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvBase.Columns.AddRange((DataGridViewColumn) this.ClientId, (DataGridViewColumn) this.FullAddress, (DataGridViewColumn) this.UName, (DataGridViewColumn) this.DEdit);
      this.dgvBase.Dock = DockStyle.Fill;
      this.dgvBase.Location = new Point(0, 25);
      this.dgvBase.Name = "dgvBase";
      this.dgvBase.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgvBase.Size = new Size(1115, 584);
      this.dgvBase.TabIndex = 8;
      this.ClientId.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.ClientId.DataPropertyName = "ClientId";
      this.ClientId.HeaderText = "Лицевой";
      this.ClientId.Name = "ClientId";
      this.FullAddress.DataPropertyName = "FullAddress";
      this.FullAddress.HeaderText = "Адрес";
      this.FullAddress.Name = "FullAddress";
      this.UName.DataPropertyName = "UName";
      this.UName.HeaderText = "Пользователь";
      this.UName.Name = "UName";
      this.UName.ReadOnly = true;
      this.DEdit.DataPropertyName = "DEdit";
      this.DEdit.HeaderText = "Дата";
      this.DEdit.Name = "DEdit";
      this.DEdit.ReadOnly = true;
      this.dataGridViewTextBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn1.DataPropertyName = "ClientId";
      this.dataGridViewTextBoxColumn1.HeaderText = "Лицевой";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn2.DataPropertyName = "SmallAddress";
      this.dataGridViewTextBoxColumn2.HeaderText = "Адрес";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn2.Width = 268;
      this.dataGridViewTextBoxColumn3.DataPropertyName = "UName";
      this.dataGridViewTextBoxColumn3.HeaderText = "Пользователь";
      this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
      this.dataGridViewTextBoxColumn3.ReadOnly = true;
      this.dataGridViewTextBoxColumn3.Width = 268;
      this.dataGridViewTextBoxColumn4.DataPropertyName = "DEdit";
      this.dataGridViewTextBoxColumn4.HeaderText = "Дата";
      this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
      this.dataGridViewTextBoxColumn4.ReadOnly = true;
      this.dataGridViewTextBoxColumn4.Width = 268;
      this.toolStrip1.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsbSelectAll,
        (ToolStripItem) this.tsbAdd
      });
      this.toolStrip1.Location = new Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new Size(1115, 25);
      this.toolStrip1.TabIndex = 10;
      this.toolStrip1.Text = "toolStrip1";
      this.tsbSelectAll.ImageTransparentColor = Color.Magenta;
      this.tsbSelectAll.Name = "tsbSelectAll";
      this.tsbSelectAll.Size = new Size(85, 22);
      this.tsbSelectAll.Text = "Выделить все";
      this.tsbSelectAll.Click += new EventHandler(this.tsbSelectAll_Click);
      this.tsbAdd.ImageTransparentColor = Color.Magenta;
      this.tsbAdd.Name = "tsbAdd";
      this.tsbAdd.Size = new Size(258, 22);
      this.tsbAdd.Text = "Добавить выделенным лицевые капремонта";
      this.tsbAdd.Click += new EventHandler(this.tsbAdd_Click);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1115, 609);
      this.Controls.Add((Control) this.dgvBase);
      this.Controls.Add((Control) this.toolStrip1);
      this.Name = "FrmSelectAccountForOverhaul";
      this.Text = "Лицевые капремонта";
      this.Load += new EventHandler(this.FrmSelectAccountForOverhaul_Load);
      ((ISupportInitialize) this.dgvBase).EndInit();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
