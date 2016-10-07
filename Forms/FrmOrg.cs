// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmOrg
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using NHibernate;
using NHibernate.Criterion;
using SaveSettings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmOrg : FrmBaseForm
  {
    private FormStateSaver fss = new FormStateSaver(FrmOrg.container);
    protected GridSettings MySettingsOrg = new GridSettings();
    private bool _readOnly = false;
    private IContainer components = (IContainer) null;
    private static IContainer container;
    private ISession session;
    private InputLanguage il;
    private Company _company;
    private Panel pnUp;
    private Panel pnBtn;
    private Button btnExit;
    private TextBox txtSearch;
    private Label lblSearch;
    private Button btnEdit;
    private Button btnAdd;
    private ToolStrip ts;
    private DataGridView dgvOrg;
    private ToolStripButton tsbAdd;
    private ToolStripButton tsbEdit;
    private ToolStripButton tsbCancel;
    private ToolStripButton tsbDelete;
    public HelpProvider hp;
    private TextBox txbINN;
    private Label lblINN;

    public FrmOrg(Company company)
    {
      this.InitializeComponent();
      this._company = company;
      this.CheckAccess();
      this.fss.ParentForm = (Form) this;
      this.SetGridConfigFileSettings();
    }

    private void CheckAccess()
    {
      this._readOnly = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(75, this._company, false));
      this.ts.Visible = this._readOnly;
      this.dgvOrg.ReadOnly = !this._readOnly;
    }

    public void SetGridConfigFileSettings()
    {
      this.MySettingsOrg.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      InputLanguage.CurrentInputLanguage = this.il;
      this.Close();
    }

    private void FrmOrg_Load(object sender, EventArgs e)
    {
      this.il = InputLanguage.CurrentInputLanguage;
      InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("ru-RU"));
      this.MySettingsOrg.GridName = "Org";
      this.LoadOrg();
    }

    private void LoadOrg()
    {
      this.session = Domain.CurrentSession;
      IList<BaseOrg> baseOrgList1 = (IList<BaseOrg>) new List<BaseOrg>();
      IList<BaseOrg> baseOrgList2 = this.session.CreateQuery("select new BaseOrg(b.BaseOrgId,b.BaseOrgName,b.NameOrgMin,(select bn from Bank bn where bn=b.Bank),b.INN,b.KPP,(select min(p.IDPOSTAVER) from Postaver p where p.IDBASEORG=b.BaseOrgId)) from BaseOrg b where b.BaseOrgName like '%" + this.txtSearch.Text + "%' and b.INN like '%" + this.txbINN.Text + "%' order by b.BaseOrgName").List<BaseOrg>();
      this.dgvOrg.DataSource = (object) null;
      this.dgvOrg.Columns.Clear();
      this.dgvOrg.DataSource = (object) baseOrgList2;
      this.session.Clear();
      this.dgvOrg.Focus();
      this.SetViewOrg();
      if (this.dgvOrg.Rows.Count <= 0)
        return;
      this.dgvOrg.CurrentCell = this.dgvOrg.Rows[0].Cells[1];
    }

    private void SetViewOrg()
    {
      this.dgvOrg.Columns["BaseOrgName"].HeaderText = "Наименование организации";
      this.dgvOrg.Columns["NameOrgMin"].HeaderText = "Краткое наименование";
      this.dgvOrg.Columns["INN"].HeaderText = "ИНН";
      this.dgvOrg.Columns["KPP"].HeaderText = "КПП";
      this.dgvOrg.Columns["RSch"].HeaderText = "Расчетный счет";
      this.dgvOrg.Columns["BaseOrgId"].Visible = false;
      this.dgvOrg.Columns["OGRN"].Visible = false;
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvOrg.Columns)
      {
        if (column.Index >= 5)
          column.Visible = false;
      }
      this.dgvOrg.Columns["BaseOrgId"].Visible = false;
      IList<Bank> bankList = (IList<Bank>) new List<Bank>();
      KvrplHelper.AddComboBoxColumn(this.dgvOrg, 6, (IList) this.session.CreateCriteria(typeof (Bank)).AddOrder(Order.Asc("BankName")).List<Bank>(), "BankId", "BankName", "Банк", "Bank", 10, 150);
      foreach (DataGridViewRow row in (IEnumerable) this.dgvOrg.Rows)
      {
        if (((BaseOrg) row.DataBoundItem).Bank != null)
          row.Cells["Bank"].Value = (object) ((BaseOrg) row.DataBoundItem).Bank.BankId;
      }
      this.LoadSettingsOrg();
    }

    private void dgvOrg_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsOrg.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsOrg.Columns[this.MySettingsOrg.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsOrg.Save();
    }

    private void LoadSettingsOrg()
    {
      this.MySettingsOrg.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvOrg.Columns)
        this.MySettingsOrg.GetMySettings(column);
    }

    private void dgvOrg_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (((DataGridView) sender).DataSource == null)
        return;
      DataGridViewRow row = ((DataGridView) sender).Rows[e.RowIndex];
      int postaver = ((BaseOrg) row.DataBoundItem).Postaver;
      if ((uint) ((BaseOrg) row.DataBoundItem).Postaver > 0U)
        row.DefaultCellStyle.BackColor = Color.PapayaWhip;
      else
        row.DefaultCellStyle.BackColor = Color.White;
    }

    private void txtSearch_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Down)
      {
        this.dgvOrg.Focus();
        if (e.KeyCode == Keys.Return)
        {
          this.dgvOrg_KeyDown(sender, e);
        }
        else
        {
          if (this.dgvOrg.Rows.Count <= 1)
            return;
          this.dgvOrg.CurrentCell = this.dgvOrg.Rows[1].Cells["BaseOrgName"];
        }
      }
      else
      {
        this.session = Domain.CurrentSession;
        IList<BaseOrg> baseOrgList1 = (IList<BaseOrg>) new List<BaseOrg>();
        IList<BaseOrg> baseOrgList2 = this.session.CreateQuery("select new BaseOrg(b.BaseOrgId,b.BaseOrgName,b.NameOrgMin,(select bn from Bank bn where bn=b.Bank),b.INN,b.KPP,(select min(p.IDPOSTAVER) from Postaver p where p.IDBASEORG=b.BaseOrgId)) from BaseOrg b where b.BaseOrgName like '%" + this.txtSearch.Text + "%' and b.INN like '%" + this.txbINN.Text + "%' order by b.BaseOrgName").List<BaseOrg>();
        this.dgvOrg.DataSource = (object) null;
        this.dgvOrg.Columns.Clear();
        this.dgvOrg.DataSource = (object) baseOrgList2;
        this.session.Clear();
        this.SetViewOrg();
        if (this.dgvOrg.Rows.Count > 0)
          this.dgvOrg.CurrentCell = this.dgvOrg.Rows[0].Cells[1];
      }
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(75, 2, Options.Company, true))
        return;
      FrmEditOrg frmEditOrg = new FrmEditOrg();
      int num = (int) frmEditOrg.ShowDialog();
      frmEditOrg.Dispose();
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.dgvOrg.Rows.Count <= 0 || this.dgvOrg.CurrentRow == null)
        return;
      FrmEditOrg frmEditOrg = new FrmEditOrg((BaseOrg) this.dgvOrg.CurrentRow.DataBoundItem);
      int num = (int) frmEditOrg.ShowDialog();
      frmEditOrg.Dispose();
      this.LoadOrg();
    }

    private void dgvOrg_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      this.btnEdit_Click(sender, (EventArgs) e);
    }

    private void dgvOrg_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, this.dgvOrg.Name, e);
    }

    private void tsbDelete_Click(object sender, EventArgs e)
    {
    }

    private void FrmOrg_Shown(object sender, EventArgs e)
    {
      this.txtSearch.Focus();
    }

    private void dgvOrg_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.btnEdit_Click(sender, (EventArgs) e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnUp = new Panel();
      this.txtSearch = new TextBox();
      this.lblSearch = new Label();
      this.pnBtn = new Panel();
      this.btnEdit = new Button();
      this.btnAdd = new Button();
      this.btnExit = new Button();
      this.ts = new ToolStrip();
      this.tsbAdd = new ToolStripButton();
      this.tsbEdit = new ToolStripButton();
      this.tsbCancel = new ToolStripButton();
      this.tsbDelete = new ToolStripButton();
      this.dgvOrg = new DataGridView();
      this.hp = new HelpProvider();
      this.lblINN = new Label();
      this.txbINN = new TextBox();
      this.pnUp.SuspendLayout();
      this.pnBtn.SuspendLayout();
      this.ts.SuspendLayout();
      ((ISupportInitialize) this.dgvOrg).BeginInit();
      this.SuspendLayout();
      this.pnUp.Controls.Add((Control) this.txbINN);
      this.pnUp.Controls.Add((Control) this.lblINN);
      this.pnUp.Controls.Add((Control) this.txtSearch);
      this.pnUp.Controls.Add((Control) this.lblSearch);
      this.pnUp.Dock = DockStyle.Top;
      this.pnUp.Location = new Point(0, 0);
      this.pnUp.Name = "pnUp";
      this.pnUp.Size = new Size(896, 39);
      this.pnUp.TabIndex = 1;
      this.txtSearch.Location = new Point(113, 9);
      this.txtSearch.Name = "txtSearch";
      this.txtSearch.Size = new Size(517, 22);
      this.txtSearch.TabIndex = 3;
      this.txtSearch.KeyUp += new KeyEventHandler(this.txtSearch_KeyUp);
      this.lblSearch.AutoSize = true;
      this.lblSearch.Location = new Point(12, 12);
      this.lblSearch.Name = "lblSearch";
      this.lblSearch.Size = new Size(95, 16);
      this.lblSearch.TabIndex = 2;
      this.lblSearch.Text = "Организация";
      this.pnBtn.Controls.Add((Control) this.btnEdit);
      this.pnBtn.Controls.Add((Control) this.btnAdd);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 400);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(896, 40);
      this.pnBtn.TabIndex = 1;
      this.btnEdit.Image = (Image) Resources.edit;
      this.btnEdit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnEdit.Location = new Point(122, 5);
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(139, 30);
      this.btnEdit.TabIndex = 2;
      this.btnEdit.Text = "Редактировать";
      this.btnEdit.TextAlign = ContentAlignment.MiddleRight;
      this.btnEdit.UseVisualStyleBackColor = true;
      this.btnEdit.Visible = false;
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnAdd.Image = (Image) Resources.Tick;
      this.btnAdd.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnAdd.Location = new Point(12, 5);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(104, 30);
      this.btnAdd.TabIndex = 1;
      this.btnAdd.Text = "Добавить";
      this.btnAdd.TextAlign = ContentAlignment.MiddleRight;
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Visible = false;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(809, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(75, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.ts.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.tsbAdd,
        (ToolStripItem) this.tsbEdit,
        (ToolStripItem) this.tsbCancel,
        (ToolStripItem) this.tsbDelete
      });
      this.ts.Location = new Point(0, 39);
      this.ts.Name = "ts";
      this.ts.Size = new Size(896, 25);
      this.ts.TabIndex = 2;
      this.ts.Text = "toolStrip1";
      this.tsbAdd.Font = new Font("Tahoma", 10f);
      this.tsbAdd.Image = (Image) Resources.add_var;
      this.tsbAdd.ImageTransparentColor = Color.Magenta;
      this.tsbAdd.Name = "tsbAdd";
      this.tsbAdd.Size = new Size(91, 22);
      this.tsbAdd.Text = "Добавить";
      this.tsbAdd.Click += new EventHandler(this.btnAdd_Click);
      this.tsbEdit.Font = new Font("Tahoma", 10f);
      this.tsbEdit.Image = (Image) Resources.edit_item;
      this.tsbEdit.ImageTransparentColor = Color.Magenta;
      this.tsbEdit.Name = "tsbEdit";
      this.tsbEdit.Size = new Size(126, 22);
      this.tsbEdit.Text = "Редактировать";
      this.tsbEdit.Click += new EventHandler(this.btnEdit_Click);
      this.tsbCancel.Enabled = false;
      this.tsbCancel.Image = (Image) Resources.undo;
      this.tsbCancel.ImageTransparentColor = Color.Magenta;
      this.tsbCancel.Name = "tsbCancel";
      this.tsbCancel.Size = new Size(81, 22);
      this.tsbCancel.Text = "Отменить";
      this.tsbCancel.Visible = false;
      this.tsbDelete.Image = (Image) Resources.delete_var;
      this.tsbDelete.ImageTransparentColor = Color.Magenta;
      this.tsbDelete.Name = "tsbDelete";
      this.tsbDelete.Size = new Size(71, 22);
      this.tsbDelete.Text = "Удалить";
      this.tsbDelete.Visible = false;
      this.tsbDelete.Click += new EventHandler(this.tsbDelete_Click);
      this.dgvOrg.BackgroundColor = Color.AliceBlue;
      this.dgvOrg.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvOrg.Dock = DockStyle.Fill;
      this.dgvOrg.Location = new Point(0, 64);
      this.dgvOrg.Name = "dgvOrg";
      this.dgvOrg.ReadOnly = true;
      this.dgvOrg.Size = new Size(896, 336);
      this.dgvOrg.TabIndex = 3;
      this.dgvOrg.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvOrg_CellFormatting);
      this.dgvOrg.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dgvOrg_CellMouseDoubleClick);
      this.dgvOrg.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvOrg_ColumnWidthChanged);
      this.dgvOrg.DataError += new DataGridViewDataErrorEventHandler(this.dgvOrg_DataError);
      this.dgvOrg.KeyDown += new KeyEventHandler(this.dgvOrg_KeyDown);
      this.hp.HelpNamespace = "Help.chm";
      this.lblINN.AutoSize = true;
      this.lblINN.Location = new Point(648, 14);
      this.lblINN.Name = "lblINN";
      this.lblINN.Size = new Size(38, 16);
      this.lblINN.TabIndex = 4;
      this.lblINN.Text = "ИНН";
      this.txbINN.Location = new Point(692, 9);
      this.txbINN.Name = "txbINN";
      this.txbINN.Size = new Size(122, 22);
      this.txbINN.TabIndex = 5;
      this.txbINN.KeyUp += new KeyEventHandler(this.txtSearch_KeyUp);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(896, 440);
      this.Controls.Add((Control) this.dgvOrg);
      this.Controls.Add((Control) this.ts);
      this.Controls.Add((Control) this.pnBtn);
      this.Controls.Add((Control) this.pnUp);
      this.hp.SetHelpKeyword((Control) this, "kv515.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      this.Name = "FrmOrg";
      this.hp.SetShowHelp((Control) this, true);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Организации";
      this.Load += new EventHandler(this.FrmOrg_Load);
      this.Shown += new EventHandler(this.FrmOrg_Shown);
      this.pnUp.ResumeLayout(false);
      this.pnUp.PerformLayout();
      this.pnBtn.ResumeLayout(false);
      this.ts.ResumeLayout(false);
      this.ts.PerformLayout();
      ((ISupportInitialize) this.dgvOrg).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
