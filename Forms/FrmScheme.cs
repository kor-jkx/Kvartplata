// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmScheme
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
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmScheme : FrmBaseForm
  {
    private FormStateSaver fss = new FormStateSaver(FrmScheme.ic);
    protected GridSettings MySettingsScheme = new GridSettings();
    private IContainer components = (IContainer) null;
    private short schemeType;
    private short schemeId;
    private ISession session;
    private static IContainer ic;
    private Panel pnBtn;
    private Button btnCancel;
    private Button btnOk;
    private Panel pnUp;
    private DataGridView dgvScheme;
    private TextBox txtSearch;
    private Label lblSearch;

    public FrmScheme()
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
    }

    public FrmScheme(short type, short id)
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
      this.schemeType = type;
      this.schemeId = id;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void FrmScheme_Load(object sender, EventArgs e)
    {
      this.MySettingsScheme.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
      this.LoadSettings();
      this.session = Domain.CurrentSession;
      IList<Scheme> schemeList1 = (IList<Scheme>) new List<Scheme>();
      IList<Scheme> schemeList2 = this.session.CreateCriteria(typeof (Scheme)).Add((ICriterion) Restrictions.Eq("SchemeType", (object) this.schemeType)).AddOrder(Order.Asc("Sorter")).AddOrder(Order.Asc("SchemeId")).List<Scheme>();
      foreach (Scheme scheme in (IEnumerable<Scheme>) schemeList2)
        scheme.SchemeNote = this.session.CreateSQLQuery(string.Format("select if scheme_note<>'' then scheme_note else ' ' endif from dcScheme where scheme={0} and scheme_type={1}", (object) scheme.SchemeId, (object) scheme.SchemeType)).List()[0].ToString();
      this.dgvScheme.DataSource = (object) schemeList2;
      this.dgvScheme.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
      this.dgvScheme.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      this.dgvScheme.Columns["SchemeId"].HeaderText = "№";
      this.dgvScheme.Columns["SchemeId"].Frozen = true;
      this.dgvScheme.Columns["SchemeName"].HeaderText = "Схема";
      this.dgvScheme.Columns["SchemeNote"].HeaderText = "Описание";
      this.dgvScheme.Columns["SchemeNote"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
      this.dgvScheme.Columns["SchemeType"].Visible = false;
      this.dgvScheme.Columns["Sorter"].Visible = false;
      this.dgvScheme.Columns["UName"].Visible = false;
      this.dgvScheme.Columns["DEdit"].Visible = false;
      foreach (DataGridViewRow row in (IEnumerable) this.dgvScheme.Rows)
      {
        if ((int) Convert.ToInt16(row.Cells["SchemeId"].Value) == (int) this.schemeId)
          this.dgvScheme.CurrentCell = row.Cells[0];
      }
      this.MySettingsScheme.GridName = "Scheme";
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (this.dgvScheme.CurrentRow != null)
      {
        this.schemeId = Convert.ToInt16(this.dgvScheme.CurrentRow.Cells["SchemeId"].Value);
        this.schemeType = Convert.ToInt16(this.dgvScheme.CurrentRow.Cells["SchemeType"].Value);
      }
      else
      {
        this.schemeId = (short) 0;
        this.schemeType = (short) 0;
      }
    }

    public short CurrentId()
    {
      return this.schemeId;
    }

    public short CurrentShemaType()
    {
      return this.schemeType;
    }

    private void dgvScheme_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      this.btnOk_Click(sender, (EventArgs) e);
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
      foreach (DataGridViewRow row in (IEnumerable) this.dgvScheme.Rows)
      {
        if (row.Cells["SchemeId"].Value.ToString().IndexOf(this.txtSearch.Text) != -1)
        {
          this.dgvScheme.CurrentCell = row.Cells["SchemeId"];
          break;
        }
      }
    }

    private void dgvScheme_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    private void dgvScheme_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsScheme.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsScheme.Columns[this.MySettingsScheme.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsScheme.Save();
    }

    public void LoadSettings()
    {
      this.MySettingsScheme.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvScheme.Columns)
        this.MySettingsScheme.GetMySettings(column);
    }

    private void FrmScheme_Shown(object sender, EventArgs e)
    {
      this.LoadSettings();
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
      this.btnCancel = new Button();
      this.btnOk = new Button();
      this.pnUp = new Panel();
      this.txtSearch = new TextBox();
      this.lblSearch = new Label();
      this.dgvScheme = new DataGridView();
      this.pnBtn.SuspendLayout();
      this.pnUp.SuspendLayout();
      ((ISupportInitialize) this.dgvScheme).BeginInit();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnCancel);
      this.pnBtn.Controls.Add((Control) this.btnOk);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 596);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(972, 40);
      this.pnBtn.TabIndex = 0;
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Image = (Image) Resources.delete;
      this.btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCancel.Location = new Point(867, 5);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(93, 30);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Отмена";
      this.btnCancel.TextAlign = ContentAlignment.MiddleRight;
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOk.DialogResult = DialogResult.OK;
      this.btnOk.Image = (Image) Resources.Tick;
      this.btnOk.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnOk.Location = new Point(12, 5);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(63, 30);
      this.btnOk.TabIndex = 0;
      this.btnOk.Text = "ОК";
      this.btnOk.TextAlign = ContentAlignment.MiddleRight;
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.pnUp.Controls.Add((Control) this.txtSearch);
      this.pnUp.Controls.Add((Control) this.lblSearch);
      this.pnUp.Dock = DockStyle.Top;
      this.pnUp.Location = new Point(0, 0);
      this.pnUp.Name = "pnUp";
      this.pnUp.Size = new Size(972, 40);
      this.pnUp.TabIndex = 1;
      this.txtSearch.Location = new Point(137, 6);
      this.txtSearch.Name = "txtSearch";
      this.txtSearch.Size = new Size(100, 22);
      this.txtSearch.TabIndex = 1;
      this.txtSearch.TextChanged += new EventHandler(this.txtSearch_TextChanged);
      this.lblSearch.AutoSize = true;
      this.lblSearch.Location = new Point(12, 9);
      this.lblSearch.Name = "lblSearch";
      this.lblSearch.Size = new Size(119, 16);
      this.lblSearch.TabIndex = 0;
      this.lblSearch.Text = "Поиск по номеру";
      this.dgvScheme.BackgroundColor = Color.AliceBlue;
      this.dgvScheme.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgvScheme.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvScheme.Dock = DockStyle.Fill;
      this.dgvScheme.Location = new Point(0, 40);
      this.dgvScheme.Name = "dgvScheme";
      this.dgvScheme.ReadOnly = true;
      this.dgvScheme.Size = new Size(972, 556);
      this.dgvScheme.TabIndex = 2;
      this.dgvScheme.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dgvScheme_CellMouseDoubleClick);
      this.dgvScheme.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvScheme_ColumnWidthChanged);
      this.dgvScheme.DataError += new DataGridViewDataErrorEventHandler(this.dgvScheme_DataError);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(972, 636);
      this.Controls.Add((Control) this.dgvScheme);
      this.Controls.Add((Control) this.pnUp);
      this.Controls.Add((Control) this.pnBtn);
      this.Margin = new Padding(5);
      this.Name = "FrmScheme";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Схемы";
      this.Load += new EventHandler(this.FrmScheme_Load);
      this.Shown += new EventHandler(this.FrmScheme_Shown);
      this.pnBtn.ResumeLayout(false);
      this.pnUp.ResumeLayout(false);
      this.pnUp.PerformLayout();
      ((ISupportInitialize) this.dgvScheme).EndInit();
      this.ResumeLayout(false);
    }
  }
}
