// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmGuild
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
  public class FrmGuild : FrmBaseForm
  {
    private int curIdx = -1;
    private FormStateSaver fss = new FormStateSaver(FrmGuild.ic);
    private bool _readOnly = false;
    private IContainer components = (IContainer) null;
    private ISession session;
    private static IContainer ic;
    private string oldGuild;
    private Company _company;
    private Panel pnBtn;
    private Button btnExit;
    private Panel pnUp;
    private TextBox txtSearch;
    private Label lblOrg;
    private SplitContainer splitContainer1;
    private Panel pnFiltr;
    private CheckBox cbINN;
    private DataGridView dgvBaseOrg;
    private ToolStrip ts;
    private ToolStripButton tsbAdd;
    private ToolStripButton tsbApplay;
    private ToolStripButton tsbCancel;
    private ToolStripButton tsbDelete;
    private DataGridView dgvGuild;

    public FrmGuild(Company company)
    {
      this.InitializeComponent();
      this._company = company;
      this.CheckAccess();
      this.fss.ParentForm = (Form) this;
      this.session = Domain.CurrentSession;
      this.LoadOrg();
    }

    private void CheckAccess()
    {
      this._readOnly = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(32, this._company, false));
      this.dgvBaseOrg.ReadOnly = !this._readOnly;
      this.ts.Visible = this._readOnly;
      this.dgvGuild.ReadOnly = !this._readOnly;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void LoadOrg()
    {
      this.session.Clear();
      this.Cursor = Cursors.WaitCursor;
      this.dgvBaseOrg.Columns.Clear();
      this.dgvBaseOrg.DataSource = (object) null;
      string str = "";
      if (this.cbINN.Checked)
        str = " and Inn<>''";
      IList<BaseOrg> baseOrgList = (IList<BaseOrg>) new List<BaseOrg>();
      this.dgvBaseOrg.DataSource = (object) this.session.CreateQuery("select new BaseOrg(b.BaseOrgId,b.BaseOrgName,b.NameOrgMin,(select bn from Bank bn where bn=b.Bank),b.INN,b.KPP,(select min(p.IDPOSTAVER) from Postaver p where p.IDBASEORG=b.BaseOrgId)) from BaseOrg b where b.NameOrgMin like '%" + this.txtSearch.Text + "%'" + str + "order by b.NameOrgMin").List<BaseOrg>();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvBaseOrg.Columns)
      {
        if (column.Name != "NameOrgMin")
        {
          column.Visible = false;
        }
        else
        {
          column.HeaderText = "Организация";
          column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
      }
      this.curIdx = 1;
      if (this.dgvBaseOrg.Rows.Count > 0 && this.dgvBaseOrg.CurrentRow != null)
        this.dgvBaseOrg.CurrentCell = this.dgvBaseOrg.Rows[0].Cells["NameOrgMin"];
      this.dgvBaseOrg_SelectionChanged((object) null, (EventArgs) null);
      this.Cursor = Cursors.Default;
    }

    private void LoadGuild()
    {
    }

    private void cbINN_Click(object sender, EventArgs e)
    {
      this.LoadOrg();
    }

    private void dgvBaseOrg_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    private void dgvBaseOrg_SelectionChanged(object sender, EventArgs e)
    {
      if (this.dgvBaseOrg.CurrentRow == null || this.dgvBaseOrg.CurrentRow == null)
        return;
      this.session.Clear();
      IList<Guild> guildList = this.session.CreateQuery(string.Format("from Guild where BaseOrg.BaseOrgId={0} order by lengthhome(GuildName)", (object) ((BaseOrg) this.dgvBaseOrg.CurrentRow.DataBoundItem).BaseOrgId)).List<Guild>();
      this.dgvGuild.DataSource = (object) null;
      this.dgvGuild.Columns.Clear();
      this.dgvGuild.DataSource = (object) guildList;
      this.SetViewGuild();
      this.curIdx = this.dgvBaseOrg.CurrentRow.Index;
    }

    private void SetViewGuild()
    {
      this.dgvGuild.Columns["GuildName"].HeaderText = "Наименование цеха";
      this.dgvGuild.Columns["GuildName"].Width = 300;
      this.dgvGuild.Columns["GuildId"].Visible = false;
    }

    private void tsbAdd_Click(object sender, EventArgs e)
    {
      Guild guild = new Guild();
      guild.BaseOrg = (BaseOrg) this.dgvBaseOrg.CurrentRow.DataBoundItem;
      IList<Guild> guildList = (IList<Guild>) new List<Guild>();
      if ((uint) this.dgvGuild.Rows.Count > 0U)
        guildList = (IList<Guild>) (this.dgvGuild.DataSource as List<Guild>);
      guildList.Add(guild);
      this.dgvGuild.DataSource = (object) null;
      this.dgvGuild.Columns.Clear();
      this.dgvGuild.DataSource = (object) guildList;
      this.SetViewGuild();
      this.dgvGuild.CurrentCell = this.dgvGuild.Rows[this.dgvGuild.Rows.Count - 1].Cells["GuildName"];
      this.tsbApplay.Enabled = true;
      this.tsbCancel.Enabled = true;
      this.tsbDelete.Enabled = false;
    }

    private bool SaveGuild()
    {
      if (this.dgvGuild.Rows.Count > 0 && this.dgvGuild.CurrentRow.Index >= 0)
      {
        this.dgvGuild.CommitEdit(DataGridViewDataErrorContexts.Commit);
        Guild dataBoundItem = (Guild) this.dgvGuild.CurrentRow.DataBoundItem;
        if (dataBoundItem.GuildName == null)
          dataBoundItem.GuildName = "";
        dataBoundItem.UName = Options.Login;
        dataBoundItem.DEdit = DateTime.Now;
        if (this.session.CreateQuery(string.Format("from Guild where BaseOrg.BaseOrgId={0} and GuildName='{1}' and GuildId<>{2}", (object) ((BaseOrg) this.dgvBaseOrg.CurrentRow.DataBoundItem).BaseOrgId, (object) dataBoundItem.GuildName, (object) dataBoundItem.GuildId)).List<Guild>().Count > 0)
          return false;
        int guildId = dataBoundItem.GuildId;
        bool flag;
        if ((uint) dataBoundItem.GuildId > 0U)
        {
          flag = false;
        }
        else
        {
          try
          {
            IList<int> intList = this.session.CreateSQLQuery("select DBA.gen_id('sdcGuild',1)").List<int>();
            dataBoundItem.GuildId = intList[0];
            flag = true;
          }
          catch
          {
            int num = (int) MessageBox.Show("Не удалось сгенерировать новый код", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
          }
        }
        try
        {
          if (flag)
            this.session.Save((object) dataBoundItem);
          else
            this.session.Update((object) dataBoundItem);
          this.session.Flush();
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Невозможно сохранить изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          KvrplHelper.WriteLog(ex, (LsClient) null);
          return false;
        }
        this.tsbAdd.Enabled = true;
        this.tsbApplay.Enabled = false;
        this.tsbCancel.Enabled = false;
        this.tsbDelete.Enabled = true;
      }
      return true;
    }

    private void tsbApplay_Click(object sender, EventArgs e)
    {
      foreach (DataGridViewRow row in (IEnumerable) this.dgvGuild.Rows)
      {
        this.dgvGuild.CurrentCell = row.Cells["GuildName"];
        row.Selected = true;
        if (((Guild) row.DataBoundItem).IsEdit)
        {
          this.SaveGuild();
          ((Guild) row.DataBoundItem).IsEdit = false;
        }
      }
      this.LoadGuild();
    }

    private void tsbCancel_Click(object sender, EventArgs e)
    {
      this.LoadGuild();
      this.tsbAdd.Enabled = true;
      this.tsbApplay.Enabled = false;
      this.tsbCancel.Enabled = false;
      this.tsbDelete.Enabled = true;
      this.dgvBaseOrg_SelectionChanged((object) null, (EventArgs) null);
    }

    private void dgvGuild_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.tsbApplay.Enabled = true;
      this.tsbCancel.Enabled = true;
      this.tsbDelete.Enabled = false;
      ((Guild) this.dgvGuild.CurrentRow.DataBoundItem).IsEdit = true;
      this.oldGuild = ((Guild) this.dgvGuild.CurrentRow.DataBoundItem).GuildName;
    }

    private void tsbDelete_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Удалить цех?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      this.session = Domain.CurrentSession;
      Guild dataBoundItem = (Guild) this.dgvGuild.CurrentRow.DataBoundItem;
      try
      {
        this.session.Delete((object) dataBoundItem);
        this.session.Flush();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Невозможно удалить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      this.session.Clear();
      this.dgvBaseOrg_SelectionChanged((object) null, (EventArgs) null);
    }

    private void dgvGuild_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      Guild dataBoundItem = (Guild) this.dgvGuild.CurrentRow.DataBoundItem;
      if (dataBoundItem.GuildName == null)
        dataBoundItem.GuildName = "";
      if (this.session.CreateQuery(string.Format("from Guild where BaseOrg.BaseOrgId={0} and GuildName='{1}' and GuildId<>{2}", (object) ((BaseOrg) this.dgvBaseOrg.CurrentRow.DataBoundItem).BaseOrgId, (object) dataBoundItem.GuildName, (object) dataBoundItem.GuildId)).List<Guild>().Count <= 0 || !(this.oldGuild != dataBoundItem.GuildName))
        return;
      int num = (int) MessageBox.Show("Такой номер цеха уже есть", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      dataBoundItem.GuildName = this.oldGuild;
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
      this.btnExit = new Button();
      this.pnUp = new Panel();
      this.txtSearch = new TextBox();
      this.lblOrg = new Label();
      this.splitContainer1 = new SplitContainer();
      this.dgvBaseOrg = new DataGridView();
      this.pnFiltr = new Panel();
      this.cbINN = new CheckBox();
      this.dgvGuild = new DataGridView();
      this.ts = new ToolStrip();
      this.tsbAdd = new ToolStripButton();
      this.tsbApplay = new ToolStripButton();
      this.tsbCancel = new ToolStripButton();
      this.tsbDelete = new ToolStripButton();
      this.pnBtn.SuspendLayout();
      this.pnUp.SuspendLayout();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      ((ISupportInitialize) this.dgvBaseOrg).BeginInit();
      this.pnFiltr.SuspendLayout();
      ((ISupportInitialize) this.dgvGuild).BeginInit();
      this.ts.SuspendLayout();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 416);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(955, 40);
      this.pnBtn.TabIndex = 0;
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(863, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(80, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.pnUp.Controls.Add((Control) this.txtSearch);
      this.pnUp.Controls.Add((Control) this.lblOrg);
      this.pnUp.Dock = DockStyle.Top;
      this.pnUp.Location = new Point(0, 0);
      this.pnUp.Name = "pnUp";
      this.pnUp.Size = new Size(955, 48);
      this.pnUp.TabIndex = 1;
      this.txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSearch.Location = new Point(113, 6);
      this.txtSearch.Name = "txtSearch";
      this.txtSearch.Size = new Size(830, 22);
      this.txtSearch.TabIndex = 10;
      this.txtSearch.TextChanged += new EventHandler(this.cbINN_Click);
      this.lblOrg.AutoSize = true;
      this.lblOrg.Location = new Point(12, 9);
      this.lblOrg.Name = "lblOrg";
      this.lblOrg.Size = new Size(95, 16);
      this.lblOrg.TabIndex = 9;
      this.lblOrg.Text = "Организация";
      this.splitContainer1.Dock = DockStyle.Fill;
      this.splitContainer1.Location = new Point(0, 48);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.dgvBaseOrg);
      this.splitContainer1.Panel1.Controls.Add((Control) this.pnFiltr);
      this.splitContainer1.Panel2.Controls.Add((Control) this.dgvGuild);
      this.splitContainer1.Panel2.Controls.Add((Control) this.ts);
      this.splitContainer1.Size = new Size(955, 368);
      this.splitContainer1.SplitterDistance = 518;
      this.splitContainer1.TabIndex = 2;
      this.dgvBaseOrg.AllowUserToAddRows = false;
      this.dgvBaseOrg.AllowUserToDeleteRows = false;
      this.dgvBaseOrg.BackgroundColor = Color.AliceBlue;
      this.dgvBaseOrg.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvBaseOrg.Dock = DockStyle.Fill;
      this.dgvBaseOrg.Location = new Point(0, 33);
      this.dgvBaseOrg.Margin = new Padding(4);
      this.dgvBaseOrg.MultiSelect = false;
      this.dgvBaseOrg.Name = "dgvBaseOrg";
      this.dgvBaseOrg.ReadOnly = true;
      this.dgvBaseOrg.Size = new Size(518, 335);
      this.dgvBaseOrg.TabIndex = 3;
      this.dgvBaseOrg.DataError += new DataGridViewDataErrorEventHandler(this.dgvBaseOrg_DataError);
      this.dgvBaseOrg.SelectionChanged += new EventHandler(this.dgvBaseOrg_SelectionChanged);
      this.pnFiltr.Controls.Add((Control) this.cbINN);
      this.pnFiltr.Dock = DockStyle.Top;
      this.pnFiltr.Location = new Point(0, 0);
      this.pnFiltr.Name = "pnFiltr";
      this.pnFiltr.Size = new Size(518, 33);
      this.pnFiltr.TabIndex = 2;
      this.cbINN.AutoSize = true;
      this.cbINN.Location = new Point(12, 3);
      this.cbINN.Name = "cbINN";
      this.cbINN.Size = new Size(170, 20);
      this.cbINN.TabIndex = 0;
      this.cbINN.Text = "Только имеющие ИНН";
      this.cbINN.UseVisualStyleBackColor = true;
      this.cbINN.Click += new EventHandler(this.cbINN_Click);
      this.dgvGuild.BackgroundColor = Color.AliceBlue;
      this.dgvGuild.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvGuild.Dock = DockStyle.Fill;
      this.dgvGuild.Location = new Point(0, 24);
      this.dgvGuild.Name = "dgvGuild";
      this.dgvGuild.Size = new Size(433, 344);
      this.dgvGuild.TabIndex = 3;
      this.dgvGuild.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvGuild_CellBeginEdit);
      this.dgvGuild.CellEndEdit += new DataGridViewCellEventHandler(this.dgvGuild_CellEndEdit);
      this.ts.Font = new Font("Tahoma", 10f);
      this.ts.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.tsbAdd,
        (ToolStripItem) this.tsbApplay,
        (ToolStripItem) this.tsbCancel,
        (ToolStripItem) this.tsbDelete
      });
      this.ts.LayoutStyle = ToolStripLayoutStyle.Flow;
      this.ts.Location = new Point(0, 0);
      this.ts.Name = "ts";
      this.ts.Size = new Size(433, 24);
      this.ts.TabIndex = 2;
      this.ts.Text = "toolStrip2";
      this.tsbAdd.Image = (Image) Resources.add_var;
      this.tsbAdd.ImageTransparentColor = Color.Magenta;
      this.tsbAdd.Name = "tsbAdd";
      this.tsbAdd.Size = new Size(91, 21);
      this.tsbAdd.Text = "Добавить";
      this.tsbAdd.Click += new EventHandler(this.tsbAdd_Click);
      this.tsbApplay.Enabled = false;
      this.tsbApplay.Image = (Image) Resources.Applay_var;
      this.tsbApplay.ImageTransparentColor = Color.Magenta;
      this.tsbApplay.Name = "tsbApplay";
      this.tsbApplay.Size = new Size(99, 21);
      this.tsbApplay.Text = "Сохранить";
      this.tsbApplay.Click += new EventHandler(this.tsbApplay_Click);
      this.tsbCancel.Enabled = false;
      this.tsbCancel.Image = (Image) Resources.undo;
      this.tsbCancel.ImageTransparentColor = Color.Magenta;
      this.tsbCancel.Name = "tsbCancel";
      this.tsbCancel.Size = new Size(93, 21);
      this.tsbCancel.Text = "Отменить";
      this.tsbCancel.Click += new EventHandler(this.tsbCancel_Click);
      this.tsbDelete.Image = (Image) Resources.delete_var;
      this.tsbDelete.ImageTransparentColor = Color.Magenta;
      this.tsbDelete.Name = "tsbDelete";
      this.tsbDelete.Size = new Size(82, 21);
      this.tsbDelete.Text = "Удалить";
      this.tsbDelete.Click += new EventHandler(this.tsbDelete_Click);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(955, 456);
      this.Controls.Add((Control) this.splitContainer1);
      this.Controls.Add((Control) this.pnUp);
      this.Controls.Add((Control) this.pnBtn);
      this.Margin = new Padding(5);
      this.Name = "FrmGuild";
      this.Text = "Цеха";
      this.pnBtn.ResumeLayout(false);
      this.pnUp.ResumeLayout(false);
      this.pnUp.PerformLayout();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.Panel2.PerformLayout();
      this.splitContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.dgvBaseOrg).EndInit();
      this.pnFiltr.ResumeLayout(false);
      this.pnFiltr.PerformLayout();
      ((ISupportInitialize) this.dgvGuild).EndInit();
      this.ts.ResumeLayout(false);
      this.ts.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
