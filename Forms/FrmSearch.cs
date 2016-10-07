// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmSearch
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmSearch : Form
  {
    private IContainer components = (IContainer) null;
    public LsClient lsClient;
    public Home home;
    public Company company;
    private int type;
    private Panel pnButton;
    private Button btnExit;
    private Button btnSearch;
    private Label lblClient;
    private Label lblCompany;
    private TextBox txtClientId;
    private TextBox txtCompany;
    public HelpProvider hp;
    private DataGridView dgvList;
    private CheckBox chbOhl;
    private Label label1;
    private TextBox tbInnOrg;

    public FrmSearch()
    {
      this.InitializeComponent();
    }

    public FrmSearch(int type, Company company, Home home)
    {
      this.lsClient = this.lsClient;
      this.InitializeComponent();
      if (type == 2)
      {
        this.Text = "Поиск по квартире";
        this.lblClient.Text = "Квартира";
        this.lblCompany.Text = "Комната";
      }
      this.type = type;
      this.company = company;
      this.home = home;
      if (Options.City != 35)
        return;
      this.label1.Visible = true;
      this.tbInnOrg.Visible = true;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.company = (Company) null;
      this.home = (Home) null;
      this.lsClient = (LsClient) null;
      this.Close();
    }

    private void btnSearchFlat_Click(object sender, EventArgs e)
    {
      if (this.txtClientId.Text == "" && this.txtCompany.Text == "")
      {
        int num1 = (int) MessageBox.Show("Данные для поиска не введены", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        ISession currentSession = Domain.CurrentSession;
        if (this.txtClientId.Text != "")
        {
          string text = this.txtClientId.Text;
          string str = "";
          if (this.txtCompany.Text != "")
            str = string.Format(" and ls.SurFlat='{0}'", (object) this.txtCompany.Text);
          this.home = currentSession.Get<Home>((object) this.home.IdHome);
          try
          {
            this.lsClient = currentSession.CreateQuery(string.Format("select ls from LsClient ls, Flat f where ls.Home.IdHome={0} and ls.Flat.IdFlat=f.IdFlat and ls.Home.IdHome=f.Home.IdHome and f.NFlat = '{1}' " + str + " and ls.Company.CompanyId={2} " + Options.MainConditions1, (object) this.home.IdHome, (object) text, (object) this.company.CompanyId)).List<LsClient>()[0];
          }
          catch
          {
            this.lsClient = (LsClient) null;
          }
          if (this.lsClient != null)
          {
            this.Close();
          }
          else
          {
            int num2 = (int) MessageBox.Show("Квартира не найдена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
        }
        else
        {
          if (!(this.txtCompany.Text != ""))
            return;
          this.home = currentSession.Get<Home>((object) this.home.IdHome);
          try
          {
            this.lsClient = currentSession.CreateQuery(string.Format("select ls from LsClient ls where ls.Home.IdHome={0} and ls.SurFlat = '{1}' and ls.Company.CompanyId={2} " + Options.MainConditions1, (object) this.home.IdHome, (object) this.txtCompany.Text, (object) this.company.CompanyId)).List<LsClient>()[0];
          }
          catch
          {
            this.lsClient = (LsClient) null;
          }
          if (this.lsClient != null)
          {
            this.Close();
          }
          else
          {
            int num2 = (int) MessageBox.Show("Комната не найдена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
        }
      }
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
      if (this.type == 2)
        this.btnSearchFlat_Click(sender, e);
      else if (this.txtClientId.Text == "" && this.txtCompany.Text == "" && this.tbInnOrg.Text == "")
      {
        int num1 = (int) MessageBox.Show("Данные для поиска не введены", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.txtClientId.Text != "" && this.txtClientId.Text.Length > 9)
      {
        int num2 = (int) MessageBox.Show("Лицевой счет введен неверно", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.txtClientId.Text = "";
      }
      else
      {
        if (this.txtClientId.Text != "")
        {
          if (this.chbOhl.Checked)
          {
            IList<SupplierClient> supplierClientList = Domain.CurrentSession.CreateCriteria(typeof (SupplierClient)).Add((ICriterion) Restrictions.Eq("SupplierClientId", (object) Convert.ToDouble(this.txtClientId.Text))).Add((ICriterion) Restrictions.Eq("Supplier.BaseOrgId", (object) -39999859)).List<SupplierClient>();
            if (supplierClientList.Count > 0)
            {
              this.lsClient = supplierClientList[0].LsClient;
              this.Close();
              return;
            }
          }
          else if (this.txtClientId.Text.Length == 9)
          {
            this.Width = 260;
            this.lsClient = KvrplHelper.FindLs(Convert.ToInt32(this.txtClientId.Text));
            if (this.lsClient != null)
            {
              this.Close();
            }
            else
            {
              int num2 = (int) MessageBox.Show("Лицевой счет не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              return;
            }
          }
          else if (this.txtClientId.Text.Length <= 8)
          {
            ISession currentSession = Domain.CurrentSession;
            if (Options.City == 3)
            {
              IList<SupplierClient> supplierClientList = currentSession.CreateCriteria(typeof (SupplierClient)).Add((ICriterion) Restrictions.Eq("SupplierClientId", (object) Convert.ToDouble(this.txtClientId.Text))).Add((ICriterion) Restrictions.Eq("Supplier.BaseOrgId", (object) 1000015)).List<SupplierClient>();
              if (supplierClientList.Count > 0)
              {
                this.lsClient = supplierClientList[0].LsClient;
                this.Close();
                return;
              }
            }
            if (this.company != null)
            {
              if (Convert.ToInt32(KvrplHelper.BaseValue(1, this.company)) == 36)
              {
                IList<LsClient> lsClientList = currentSession.CreateQuery(string.Format("select ls From LsClient ls where ls.ClientId={0}  " + Options.MainConditions1, (object) Convert.ToInt32(this.txtClientId.Text))).List<LsClient>();
                if (lsClientList.Count > 0)
                {
                  this.lsClient = lsClientList[0];
                  this.Close();
                  return;
                }
              }
            }
            else if (Options.City == 36)
            {
              IList<LsClient> lsClientList = currentSession.CreateQuery(string.Format("select ls From LsClient ls where ls.ClientId={0}  " + Options.MainConditions1, (object) Convert.ToInt32(this.txtClientId.Text))).List<LsClient>();
              if (lsClientList.Count > 0)
              {
                this.lsClient = lsClientList[0];
                this.Close();
                return;
              }
            }
            try
            {
              Convert.ToInt32(this.txtClientId.Text);
            }
            catch
            {
              int num2 = (int) MessageBox.Show("Лицевой счет введен неверно", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return;
            }
            IList<LsClient> lsClientList1 = currentSession.CreateQuery(string.Format("select ls From LsClient ls where ls.OldId={0}  " + Options.MainConditions1, (object) Convert.ToInt32(this.txtClientId.Text))).List<LsClient>();
            if (lsClientList1.Count == 1)
            {
              this.lsClient = lsClientList1[0];
              this.Close();
            }
            else if (lsClientList1.Count > 0)
            {
              this.dgvList.DataSource = (object) lsClientList1;
              this.Height = 300;
              this.SetViewList();
            }
            else
            {
              int num2 = (int) MessageBox.Show("Лицевой счет не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              return;
            }
          }
          else
          {
            int num2 = (int) MessageBox.Show("Лицевой счет не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return;
          }
        }
        if (this.txtCompany.Text != "")
        {
          ISession currentSession = Domain.CurrentSession;
          IList list1 = (IList) new ArrayList();
          IList list2;
          try
          {
            list2 = currentSession.CreateCriteria(typeof (Company)).Add((ICriterion) Restrictions.Eq("CompanyId", (object) Convert.ToInt16(this.txtCompany.Text))).List();
          }
          catch (Exception ex)
          {
            int num2 = (int) MessageBox.Show("Некорректно введен номер участка.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return;
          }
          if ((uint) list2.Count > 0U)
          {
            this.company = (Company) list2[0];
            this.Close();
          }
          else
          {
            int num2 = (int) MessageBox.Show("Участок с таким номером не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return;
          }
        }
        if (!(this.tbInnOrg.Text != ""))
          return;
        IList<LsArenda> lsArendaList = Domain.CurrentSession.CreateQuery("from LsArenda where BaseOrg.INN=:inn ").SetParameter<string>("inn", this.tbInnOrg.Text).List<LsArenda>();
        if (lsArendaList == null)
        {
          int num3 = (int) MessageBox.Show("Лицевой счет не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else if (lsArendaList.Count == 0)
        {
          int num4 = (int) MessageBox.Show("Лицевой счет не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          LsClient lsClient = (LsClient) null;
          using (IEnumerator<LsArenda> enumerator = lsArendaList.GetEnumerator())
          {
            if (enumerator.MoveNext())
              lsClient = enumerator.Current.LsClient;
          }
          if (lsClient != null)
          {
            this.lsClient = lsClient;
            this.Close();
          }
          else
          {
            int num2 = (int) MessageBox.Show("Лицевой счет не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
        }
      }
    }

    private void mtxtClientId_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.btnSearch_Click(sender, (EventArgs) e);
    }

    private void lblCompany_Click(object sender, EventArgs e)
    {
    }

    private void mtxtCompany_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      if (this.lsClient == null)
        this.btnSearch_Click(sender, (EventArgs) e);
      else
        this.btnSearchFlat_Click(sender, (EventArgs) e);
    }

    private void txtClientId_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (this.type == 2 || ((int) e.KeyChar == 8 || (int) e.KeyChar == 13 || (int) e.KeyChar >= 48 && (int) e.KeyChar <= 57))
        return;
      e.Handled = true;
    }

    private void txtCompany_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (this.type != 2)
      {
        if ((int) e.KeyChar == 8 || (int) e.KeyChar == 13 || (int) e.KeyChar >= 48 && (int) e.KeyChar <= 57)
          return;
        e.Handled = true;
      }
      else
      {
        if ((int) e.KeyChar == 8 || (int) e.KeyChar == 13 || ((int) e.KeyChar == 44 || (int) e.KeyChar == 46) || (int) e.KeyChar >= 48 && (int) e.KeyChar <= 57)
          return;
        e.Handled = true;
      }
    }

    private void SetViewList()
    {
      this.dgvList.Visible = true;
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

    private void dgvList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (this.dgvList.Rows.Count <= 0 || this.dgvList.CurrentRow == null)
        return;
      this.lsClient = (LsClient) this.dgvList.CurrentRow.DataBoundItem;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmSearch));
      this.pnButton = new Panel();
      this.btnSearch = new Button();
      this.btnExit = new Button();
      this.lblClient = new Label();
      this.lblCompany = new Label();
      this.txtClientId = new TextBox();
      this.txtCompany = new TextBox();
      this.hp = new HelpProvider();
      this.dgvList = new DataGridView();
      this.chbOhl = new CheckBox();
      this.label1 = new Label();
      this.tbInnOrg = new TextBox();
      this.pnButton.SuspendLayout();
      ((ISupportInitialize) this.dgvList).BeginInit();
      this.SuspendLayout();
      this.pnButton.Controls.Add((Control) this.btnSearch);
      this.pnButton.Controls.Add((Control) this.btnExit);
      this.pnButton.Dock = DockStyle.Bottom;
      this.pnButton.Location = new Point(0, 138);
      this.pnButton.Margin = new Padding(4);
      this.pnButton.Name = "pnButton";
      this.pnButton.Size = new Size(278, 40);
      this.pnButton.TabIndex = 1;
      this.btnSearch.Image = (Image) Resources.search_24;
      this.btnSearch.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSearch.Location = new Point(5, 5);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new Size(80, 30);
      this.btnSearch.TabIndex = 0;
      this.btnSearch.Text = "Поиск";
      this.btnSearch.TextAlign = ContentAlignment.MiddleRight;
      this.btnSearch.UseVisualStyleBackColor = true;
      this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(194, 5);
      this.btnExit.Margin = new Padding(4);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(80, 30);
      this.btnExit.TabIndex = 1;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.lblClient.AutoSize = true;
      this.lblClient.Location = new Point(4, 12);
      this.lblClient.Name = "lblClient";
      this.lblClient.Size = new Size(98, 16);
      this.lblClient.TabIndex = 2;
      this.lblClient.Text = "Лицевой счет";
      this.lblCompany.AutoSize = true;
      this.lblCompany.Location = new Point(4, 40);
      this.lblCompany.Name = "lblCompany";
      this.lblCompany.Size = new Size(79, 16);
      this.lblCompany.TabIndex = 5;
      this.lblCompany.Text = "№ Участка";
      this.lblCompany.Click += new EventHandler(this.lblCompany_Click);
      this.txtClientId.Location = new Point(108, 6);
      this.txtClientId.Name = "txtClientId";
      this.txtClientId.Size = new Size(128, 22);
      this.txtClientId.TabIndex = 0;
      this.txtClientId.KeyDown += new KeyEventHandler(this.mtxtClientId_KeyDown);
      this.txtClientId.KeyPress += new KeyPressEventHandler(this.txtClientId_KeyPress);
      this.txtCompany.Location = new Point(108, 37);
      this.txtCompany.Name = "txtCompany";
      this.txtCompany.Size = new Size(128, 22);
      this.txtCompany.TabIndex = 1;
      this.txtCompany.KeyDown += new KeyEventHandler(this.mtxtCompany_KeyDown);
      this.txtCompany.KeyPress += new KeyPressEventHandler(this.txtCompany_KeyPress);
      this.hp.HelpNamespace = "Help.chm";
      this.dgvList.BackgroundColor = Color.AliceBlue;
      this.dgvList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvList.Location = new Point(7, 122);
      this.dgvList.Name = "dgvList";
      this.dgvList.Size = new Size(259, 10);
      this.dgvList.TabIndex = 6;
      this.dgvList.Visible = false;
      this.dgvList.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dgvList_CellMouseDoubleClick);
      this.chbOhl.AutoSize = true;
      this.chbOhl.Location = new Point(7, 96);
      this.chbOhl.Name = "chbOhl";
      this.chbOhl.Size = new Size(166, 20);
      this.chbOhl.TabIndex = 7;
      this.chbOhl.Text = "Лицевой капремонта";
      this.chbOhl.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(4, 71);
      this.label1.Name = "label1";
      this.label1.Size = new Size(62, 16);
      this.label1.TabIndex = 8;
      this.label1.Text = "Инн орг.";
      this.label1.Visible = false;
      this.tbInnOrg.Location = new Point(108, 68);
      this.tbInnOrg.Name = "tbInnOrg";
      this.tbInnOrg.Size = new Size(128, 22);
      this.tbInnOrg.TabIndex = 9;
      this.tbInnOrg.Visible = false;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(278, 178);
      this.Controls.Add((Control) this.tbInnOrg);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.dgvList);
      this.Controls.Add((Control) this.chbOhl);
      this.Controls.Add((Control) this.txtCompany);
      this.Controls.Add((Control) this.txtClientId);
      this.Controls.Add((Control) this.lblCompany);
      this.Controls.Add((Control) this.lblClient);
      this.Controls.Add((Control) this.pnButton);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.hp.SetHelpKeyword((Control) this, "kv111.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmSearch";
      this.hp.SetShowHelp((Control) this, true);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Поиск";
      this.pnButton.ResumeLayout(false);
      ((ISupportInitialize) this.dgvList).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
