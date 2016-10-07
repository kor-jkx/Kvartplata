// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmSearchTabNum
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
  public class FrmSearchTabNum : FrmBaseForm
  {
    private IContainer components = (IContainer) null;
    private ISession session;
    private LsClient client;
    private short numSearch;
    private Panel pnBtn;
    private Button btnSearch;
    private Button btnExit;
    private Label lblNumber;
    private TextBox txtNumber;
    private DataGridView dgvList;
    private MaskedTextBox mtbNPfr;

    public LsClient Client
    {
      get
      {
        return this.client;
      }
    }

    public FrmSearchTabNum(short numSearch)
    {
      this.InitializeComponent();
      this.session = Domain.CurrentSession;
      this.numSearch = numSearch;
    }

    private void txtNumber_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.btnSearch_Click(sender, (EventArgs) e);
    }

    private void txtNumber_KeyPress(object sender, KeyPressEventArgs e)
    {
      if ((int) e.KeyChar == 8 || (int) e.KeyChar == 13 || (int) e.KeyChar >= 48 && (int) e.KeyChar <= 57)
        return;
      e.Handled = true;
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
      if ((int) this.numSearch == 1 && this.txtNumber.Text == "" || (int) this.numSearch == 2 && this.mtbNPfr.Text == "   -   -   -")
      {
        int num1 = (int) MessageBox.Show("Данные для поиска не введены", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        this.session.Clear();
        this.dgvList.DataSource = (object) null;
        this.dgvList.Columns.Clear();
        IList<LsClient> lsClientList1 = (IList<LsClient>) new List<LsClient>();
        IList<LsClient> lsClientList2 = (int) this.numSearch != 1 ? this.session.CreateQuery(string.Format("select ls from Person p, LsClient ls left join fetch ls.Home h left join fetch h.Str where p.LsClient.ClientId=ls.ClientId and p.Snils like '%{0}%'", (object) this.mtbNPfr.Text.Substring(0, this.mtbNPfr.Text.IndexOf(" ") != -1 ? this.mtbNPfr.Text.IndexOf(" ") : this.mtbNPfr.Text.Length))).List<LsClient>() : this.session.CreateQuery(string.Format("select ls from Person p, LsClient ls left join fetch ls.Home h left join fetch h.Str where p.LsClient.ClientId=ls.ClientId and p.Number='{0}'", (object) this.txtNumber.Text)).List<LsClient>();
        if (lsClientList2.Count == 1)
        {
          this.client = lsClientList2[0];
          this.Close();
        }
        else if (lsClientList2.Count > 0)
        {
          this.dgvList.DataSource = (object) lsClientList2;
          this.SetViewList();
        }
        else
        {
          if ((int) this.numSearch == 1)
          {
            int num2 = (int) MessageBox.Show("Табельный номер не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          if ((int) this.numSearch != 2)
            return;
          int num3 = (int) MessageBox.Show("Такой номер не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
      }
    }

    private void SetViewList()
    {
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvList.Columns)
      {
        if (column.Name != "ClientId" && column.Name != "Family")
          column.Visible = false;
      }
      this.dgvList.Columns["ClientId"].HeaderText = "Лицевой счет";
      this.dgvList.Columns["Family"].HeaderText = "Адрес";
      this.dgvList.Columns["Family"].Width = 350;
      foreach (DataGridViewRow row in (IEnumerable) this.dgvList.Rows)
      {
        if (((LsClient) row.DataBoundItem).SurFlat != null && ((LsClient) row.DataBoundItem).SurFlat != "0" && ((LsClient) row.DataBoundItem).SurFlat != "" && ((LsClient) row.DataBoundItem).SurFlat != " ")
          row.Cells["Family"].Value = (object) (((LsClient) row.DataBoundItem).Home.Address + " кв." + ((LsClient) row.DataBoundItem).Flat.NFlat + " комн." + ((LsClient) row.DataBoundItem).SurFlat);
        else
          row.Cells["Family"].Value = (object) (((LsClient) row.DataBoundItem).Home.Address + " кв." + ((LsClient) row.DataBoundItem).Flat.NFlat);
      }
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.client = (LsClient) null;
      this.Close();
    }

    private void dgvList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (this.dgvList.Rows.Count <= 0 || this.dgvList.CurrentRow == null)
        return;
      this.client = (LsClient) this.dgvList.CurrentRow.DataBoundItem;
      this.Close();
    }

    private void FrmSearchTabNum_Shown(object sender, EventArgs e)
    {
      if ((int) this.numSearch != 2)
        return;
      this.Text = "Поиск по СНИЛС";
      this.lblNumber.Text = "СНИЛС";
      this.mtbNPfr.Visible = true;
      this.txtNumber.Visible = false;
      this.mtbNPfr.Focus();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnBtn = new Panel();
      this.btnSearch = new Button();
      this.btnExit = new Button();
      this.lblNumber = new Label();
      this.txtNumber = new TextBox();
      this.dgvList = new DataGridView();
      this.mtbNPfr = new MaskedTextBox();
      this.pnBtn.SuspendLayout();
      ((ISupportInitialize) this.dgvList).BeginInit();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnSearch);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 188);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(582, 40);
      this.pnBtn.TabIndex = 0;
      this.btnSearch.Image = (Image) Resources.search_24;
      this.btnSearch.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSearch.Location = new Point(12, 5);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new Size(80, 30);
      this.btnSearch.TabIndex = 2;
      this.btnSearch.Text = "Поиск";
      this.btnSearch.TextAlign = ContentAlignment.MiddleRight;
      this.btnSearch.UseVisualStyleBackColor = true;
      this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(489, 4);
      this.btnExit.Margin = new Padding(4);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(80, 30);
      this.btnExit.TabIndex = 3;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.lblNumber.AutoSize = true;
      this.lblNumber.Location = new Point(9, 15);
      this.lblNumber.Name = "lblNumber";
      this.lblNumber.Size = new Size(125, 16);
      this.lblNumber.TabIndex = 1;
      this.lblNumber.Text = "Табельный номер";
      this.txtNumber.Location = new Point(140, 12);
      this.txtNumber.Name = "txtNumber";
      this.txtNumber.Size = new Size(179, 22);
      this.txtNumber.TabIndex = 0;
      this.txtNumber.KeyDown += new KeyEventHandler(this.txtNumber_KeyDown);
      this.txtNumber.KeyPress += new KeyPressEventHandler(this.txtNumber_KeyPress);
      this.dgvList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dgvList.BackgroundColor = Color.AliceBlue;
      this.dgvList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvList.Location = new Point(0, 40);
      this.dgvList.Name = "dgvList";
      this.dgvList.Size = new Size(582, 148);
      this.dgvList.TabIndex = 3;
      this.dgvList.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dgvList_CellMouseDoubleClick);
      this.mtbNPfr.Location = new Point(91, 12);
      this.mtbNPfr.Mask = "000-000-000-00";
      this.mtbNPfr.Name = "mtbNPfr";
      this.mtbNPfr.Size = new Size(99, 22);
      this.mtbNPfr.TabIndex = 5;
      this.mtbNPfr.Visible = false;
      this.mtbNPfr.KeyDown += new KeyEventHandler(this.txtNumber_KeyDown);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(582, 228);
      this.Controls.Add((Control) this.lblNumber);
      this.Controls.Add((Control) this.txtNumber);
      this.Controls.Add((Control) this.mtbNPfr);
      this.Controls.Add((Control) this.dgvList);
      this.Controls.Add((Control) this.pnBtn);
      this.Name = "FrmSearchTabNum";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Поиск по табельному номеру";
      this.Shown += new EventHandler(this.FrmSearchTabNum_Shown);
      this.pnBtn.ResumeLayout(false);
      ((ISupportInitialize) this.dgvList).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
