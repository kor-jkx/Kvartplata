// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmTypeDelivery
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmTypeDelivery : Form
  {
    private Dictionary<int, string> dic = new Dictionary<int, string>() { { 0, "Обычная почта" }, { 1, "Электронный почтовый ящик" } };
    private IContainer components = (IContainer) null;
    private LsClient _lsClient;
    private ISession session;
    private IList<SndAddress> sndAddressList;
    private SndAddress oldSndAddress;
    private Button btnSave;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private Button btnAdd;
    private DataGridView dg;
    private DataGridViewComboBoxColumn Receipt;
    private DataGridViewComboBoxColumn Type;
    private DataGridViewTextBoxColumn EmailAdress;
    private Panel panelDGV;
    private Panel panel1;
    private Button butDel;
    private Button btnExit;

    public FrmTypeDelivery()
    {
      this.InitializeComponent();
    }

    public FrmTypeDelivery(LsClient client)
    {
      this.InitializeComponent();
      this._lsClient = client;
    }

    private void FrmTypeDelivery_Load(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      this.LoadData();
      if (this.session.CreateQuery("from SndAddress s where s.Account=" + (object) this._lsClient.Company.CompanyId + " and Account_Type=0").UniqueResult<SndAddress>() != null);
    }

    private void LoadData()
    {
      this.session.Clear();
      this.sndAddressList = this.session.CreateQuery("select s from SndAddress s where s.Account=:cl and Account_Type=1").SetParameter<int>("cl", this._lsClient.ClientId).List<SndAddress>();
      foreach (SndAddress sndAddress in (IEnumerable<SndAddress>) this.sndAddressList)
        sndAddress.Company = false;
      foreach (SndAddress sndAddress in (IEnumerable<SndAddress>) this.session.CreateQuery("from SndAddress s where s.Account=" + (object) this._lsClient.Company.CompanyId + " and Account_Type = 0").List<SndAddress>())
      {
        sndAddress.Company = true;
        this.sndAddressList.Add(sndAddress);
      }
      this.dg.AutoGenerateColumns = false;
      this.dg.DataSource = (object) null;
      this.dg.DataSource = (object) this.sndAddressList;
      this.SetView();
    }

    private void SetView()
    {
      IList<Kvartplata.Classes.Receipt> receiptList1 = (IList<Kvartplata.Classes.Receipt>) new List<Kvartplata.Classes.Receipt>();
      IList<Kvartplata.Classes.Receipt> receiptList2 = this.session.CreateQuery("from Receipt").List<Kvartplata.Classes.Receipt>();
      foreach (DataGridViewRow row in (IEnumerable) this.dg.Rows)
      {
        ((DataGridViewComboBoxCell) this.dg["Receipt", row.Index]).DataSource = (object) receiptList2;
        ((DataGridViewComboBoxCell) this.dg["Receipt", row.Index]).DisplayMember = "ReceiptName";
        ((DataGridViewComboBoxCell) this.dg["Receipt", row.Index]).ValueMember = "ReceiptId";
        row.Cells["Receipt"].Value = (object) (short) ((SndAddress) row.DataBoundItem).Receipt_id;
        row.Cells["Type"].Value = (object) this.dic[((SndAddress) row.DataBoundItem).Active];
        row.Cells["EmailAdress"].Value = (object) ((SndAddress) row.DataBoundItem).EmailAdress;
      }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      this.session.Clear();
      this.session = Domain.CurrentSession;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dg.SelectedRows)
      {
        SndAddress dataBoundItem = (SndAddress) selectedRow.DataBoundItem;
        if (!dataBoundItem.Company)
        {
          int num1 = 0;
          foreach (KeyValuePair<int, string> keyValuePair in this.dic)
          {
            if (keyValuePair.Value.Equals(selectedRow.Cells["Type"].Value))
              num1 = keyValuePair.Key;
          }
          dataBoundItem.Receipt_id = (int) (short) selectedRow.Cells["Receipt"].Value;
          dataBoundItem.Active = num1;
          dataBoundItem.Account = this._lsClient.ClientId;
          if ((string) selectedRow.Cells["EmailAdress"].Value == "" && num1 > 0)
          {
            int num2 = (int) MessageBox.Show("Введите адрес электронной почты!", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          ((SndAddress) selectedRow.DataBoundItem).EmailAdress = (string) selectedRow.Cells["EmailAdress"].Value;
          dataBoundItem.EmailAdress = (string) selectedRow.Cells["EmailAdress"].Value;
          this.session.SaveOrUpdate((object) dataBoundItem);
          this.session.Flush();
        }
      }
      int num = (int) MessageBox.Show("Сохранено!", "", MessageBoxButtons.OK);
      this.btnSave.Enabled = false;
    }

    private void dg_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      if (!(this.dg.Columns[e.ColumnIndex].Name == "Type"))
        return;
      int num = 0;
      foreach (KeyValuePair<int, string> keyValuePair in this.dic)
      {
        if (keyValuePair.Value.Equals(this.dg[e.ColumnIndex, e.RowIndex].Value))
          num = keyValuePair.Key;
      }
      this.dg.Rows[e.RowIndex].Cells["EmailAdress"].ReadOnly = num <= 0;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if ((uint) this.dg.Rows.Count > 0U)
        this.sndAddressList = (IList<SndAddress>) (this.dg.DataSource as List<SndAddress>);
      if (this.sndAddressList != null)
      {
        IList<SndAddress> sndAddressList = this.sndAddressList;
        SndAddress sndAddress = new SndAddress();
        sndAddress.Account_Type = 1;
        sndAddress.Active = 0;
        sndAddress.Receipt_id = 0;
        sndAddress.FTPAdress = "";
        sndAddress.DownloadDir = "";
        sndAddress.EmailAdress = "";
        sndAddress.UploadDir = "";
        int num1 = this.session.CreateSQLQuery("Select DBA.Gen_id('SndAddress',1)").UniqueResult<int>();
        sndAddress.SndAddressId = num1;
        int num2 = 0;
        sndAddress.Company = num2 != 0;
        sndAddressList.Add(sndAddress);
        this.dg.DataSource = (object) null;
        this.dg.DataSource = (object) this.sndAddressList;
        this.dg.EditMode = DataGridViewEditMode.EditOnEnter;
      }
      this.SetView();
      if (this.dg.Rows.Count <= 1)
        return;
      this.dg.CurrentCell = this.dg[0, this.dg.Rows.Count - 1];
    }

    private void dg_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      if (!(this.dg.Columns[e.ColumnIndex].Name == "Receipt") && !(this.dg.Columns[e.ColumnIndex].Name == "Type"))
        return;
      this.btnSave.Enabled = true;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void dg_SelectionChanged(object sender, EventArgs e)
    {
      this.butDel.Enabled = true;
    }

    private void butDel_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Удалить данные?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        ;
      DataGridViewSelectedRowCollection selectedRows = this.dg.SelectedRows;
      this.session.Clear();
      this.session = Domain.CurrentSession;
      foreach (DataGridViewRow dataGridViewRow in (BaseCollection) selectedRows)
      {
        SndAddress dataBoundItem = (SndAddress) dataGridViewRow.DataBoundItem;
        if (!dataBoundItem.Company)
        {
          this.session.Delete((object) dataBoundItem);
          this.session.Flush();
        }
      }
      this.LoadData();
    }

    private void dg_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (((DataGridView) sender).DataSource == null)
        return;
      DataGridViewRow row = ((DataGridView) sender).Rows[e.RowIndex];
      if (((SndAddress) row.DataBoundItem).Company)
      {
        row.DefaultCellStyle.BackColor = Color.Gray;
        row.ReadOnly = true;
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
      DataGridViewCellStyle gridViewCellStyle = new DataGridViewCellStyle();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmTypeDelivery));
      this.btnSave = new Button();
      this.btnAdd = new Button();
      this.dg = new DataGridView();
      this.Receipt = new DataGridViewComboBoxColumn();
      this.Type = new DataGridViewComboBoxColumn();
      this.panelDGV = new Panel();
      this.panel1 = new Panel();
      this.butDel = new Button();
      this.btnExit = new Button();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.EmailAdress = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      ((ISupportInitialize) this.dg).BeginInit();
      this.panelDGV.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.btnSave.Enabled = false;
      this.btnSave.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.btnSave.Image = (Image) Resources.Tick;
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.Location = new Point(224, 7);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(105, 30);
      this.btnSave.TabIndex = 1;
      this.btnSave.Text = "Сохранить";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnAdd.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.btnAdd.Image = (Image) Resources.plus;
      this.btnAdd.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnAdd.Location = new Point(12, 7);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(104, 30);
      this.btnAdd.TabIndex = 3;
      this.btnAdd.Text = "Добавить";
      this.btnAdd.TextAlign = ContentAlignment.MiddleRight;
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.dg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      this.dg.BackgroundColor = Color.AliceBlue;
      gridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle.BackColor = SystemColors.Control;
      gridViewCellStyle.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      gridViewCellStyle.ForeColor = SystemColors.WindowText;
      gridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle.WrapMode = DataGridViewTriState.True;
      this.dg.ColumnHeadersDefaultCellStyle = gridViewCellStyle;
      this.dg.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dg.Columns.AddRange((DataGridViewColumn) this.Receipt, (DataGridViewColumn) this.Type, (DataGridViewColumn) this.EmailAdress);
      this.dg.Dock = DockStyle.Fill;
      this.dg.Location = new Point(0, 0);
      this.dg.Name = "dg";
      this.dg.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dg.Size = new Size(1053, 315);
      this.dg.TabIndex = 4;
      this.dg.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dg_CellBeginEdit);
      this.dg.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dg_CellFormatting);
      this.dg.CellValueChanged += new DataGridViewCellEventHandler(this.dg_CellValueChanged);
      this.dg.SelectionChanged += new EventHandler(this.dg_SelectionChanged);
      this.Receipt.HeaderText = "Квитанция";
      this.Receipt.Name = "Receipt";
      this.Type.HeaderText = "Способ доставки";
      this.Type.Items.AddRange((object) "Обычная почта", (object) "Электронный почтовый ящик");
      this.Type.Name = "Type";
      this.panelDGV.Controls.Add((Control) this.dg);
      this.panelDGV.Dock = DockStyle.Fill;
      this.panelDGV.Location = new Point(0, 0);
      this.panelDGV.Name = "panelDGV";
      this.panelDGV.Size = new Size(1053, 315);
      this.panelDGV.TabIndex = 5;
      this.panel1.Controls.Add((Control) this.btnExit);
      this.panel1.Controls.Add((Control) this.butDel);
      this.panel1.Controls.Add((Control) this.btnAdd);
      this.panel1.Controls.Add((Control) this.btnSave);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 264);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(1053, 51);
      this.panel1.TabIndex = 6;
      this.butDel.Enabled = false;
      this.butDel.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.butDel.Image = (Image) Resources.minus;
      this.butDel.ImageAlign = ContentAlignment.MiddleLeft;
      this.butDel.Location = new Point(122, 7);
      this.butDel.Name = "butDel";
      this.butDel.Size = new Size(96, 30);
      this.butDel.TabIndex = 4;
      this.butDel.Text = "Удалить";
      this.butDel.TextAlign = ContentAlignment.MiddleRight;
      this.butDel.UseVisualStyleBackColor = true;
      this.butDel.Click += new EventHandler(this.butDel_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      //this.btnExit.Image = (Image) componentResourceManager.GetObject("btnExit.Image");
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(957, 8);
      this.btnExit.Margin = new Padding(4);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(83, 30);
      this.btnExit.TabIndex = 5;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.dataGridViewTextBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn1.DataPropertyName = "SndAddressId";
      this.dataGridViewTextBoxColumn1.FillWeight = 152.2843f;
      this.dataGridViewTextBoxColumn1.HeaderText = "id";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.ReadOnly = true;
      this.dataGridViewTextBoxColumn1.Visible = false;
      this.EmailAdress.HeaderText = "Адрес электронной почты";
      this.EmailAdress.Name = "EmailAdress";
      this.dataGridViewTextBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
      this.dataGridViewTextBoxColumn2.DataPropertyName = "EmailAdress";
      this.dataGridViewTextBoxColumn2.FillWeight = 10.30928f;
      this.dataGridViewTextBoxColumn2.HeaderText = "Адрес электронной почты";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn2.ReadOnly = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1053, 315);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.panelDGV);
      ////this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = "FrmTypeDelivery";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Способ доставки";
      this.Load += new EventHandler(this.FrmTypeDelivery_Load);
      ((ISupportInitialize) this.dg).EndInit();
      this.panelDGV.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
