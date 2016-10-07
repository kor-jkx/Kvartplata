// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmOrgPersons
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
  public class FrmOrgPersons : FrmBaseForm1
  {
    private IContainer components = (IContainer) null;
    private BaseOrg org;
    private ISession session;
    private ToolStrip toolStrip1;
    private ToolStripButton tsbAddPerson;
    private ToolStripButton tsbSavePerson;
    private ToolStripButton tsbDeletePerson;
    private DataGridView dgvPersons;

    public FrmOrgPersons()
    {
      this.InitializeComponent();
    }

    public FrmOrgPersons(BaseOrg org)
    {
      this.InitializeComponent();
      this.org = org;
    }

    private void FrmOrgPersons_Shown(object sender, EventArgs e)
    {
      this.LoadPersons();
    }

    private void FrmOrgPersons_Load(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
    }

    private void LoadPersons()
    {
      this.tsbAddPerson.Enabled = true;
      this.tsbSavePerson.Enabled = false;
      this.tsbDeletePerson.Enabled = true;
      this.dgvPersons.DataSource = (object) null;
      this.dgvPersons.Columns.Clear();
      try
      {
        if ((uint) this.org.BaseOrgId > 0U)
          this.dgvPersons.DataSource = (object) this.session.CreateCriteria(typeof (BaseOrgPerson)).Add((ICriterion) Restrictions.Eq("BaseOrg", (object) this.org)).List<BaseOrgPerson>();
        this.SetViewPersons();
      }
      catch
      {
      }
    }

    private void SetViewPersons()
    {
      this.dgvPersons.Columns["Family"].HeaderText = "Фамилия";
      this.dgvPersons.Columns["Name"].HeaderText = "Имя";
      this.dgvPersons.Columns["LastName"].HeaderText = "Отчество";
      this.dgvPersons.Columns["PlaceWork"].HeaderText = "Должность";
      this.dgvPersons.Columns["Phone"].HeaderText = "Телефон";
      this.dgvPersons.Columns["ICQ"].HeaderText = "ICQ";
      this.dgvPersons.Columns["EMail"].HeaderText = "Почта";
      this.dgvPersons.Columns["Note"].HeaderText = "Примечания";
      this.dgvPersons.Columns["BasedOn"].HeaderText = "На основании";
      KvrplHelper.AddComboBoxColumn(this.dgvPersons, 7, (IList) this.session.CreateCriteria<YesNo>().List<YesNo>(), "YesNoId", "YesNoName", "Печатать в отчетах", "IncRep", 2, 80);
      foreach (DataGridViewRow row in (IEnumerable) this.dgvPersons.Rows)
      {
        if (((BaseOrgPerson) row.DataBoundItem).IncRep != null)
          row.Cells["IncRep"].Value = (object) ((BaseOrgPerson) row.DataBoundItem).IncRep.YesNoId;
      }
    }

    private void btnAddPerson_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(75, 2, Options.Company, true))
        return;
      BaseOrgPerson baseOrgPerson = new BaseOrgPerson();
      baseOrgPerson.BaseOrg = this.org;
      baseOrgPerson.IncRep = this.session.Get<YesNo>((object) Convert.ToInt16(0));
      IList<BaseOrgPerson> baseOrgPersonList = (IList<BaseOrgPerson>) new List<BaseOrgPerson>();
      if ((uint) this.dgvPersons.Rows.Count > 0U)
        baseOrgPersonList = (IList<BaseOrgPerson>) (this.dgvPersons.DataSource as List<BaseOrgPerson>);
      baseOrgPersonList.Add(baseOrgPerson);
      this.dgvPersons.Columns.Clear();
      this.dgvPersons.DataSource = (object) null;
      this.dgvPersons.DataSource = (object) baseOrgPersonList;
      this.SetViewPersons();
      this.dgvPersons.CurrentCell = this.dgvPersons.Rows[this.dgvPersons.Rows.Count - 1].Cells[0];
    }

    private void btnSavePerson_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(75, 2, Options.Company, true))
        return;
      bool flag = false;
      foreach (DataGridViewRow row in (IEnumerable) this.dgvPersons.Rows)
      {
        this.dgvPersons.CurrentCell = row.Cells["Family"];
        row.Selected = true;
        if (((BaseOrgPerson) row.DataBoundItem).IsEdit && !this.SavePerson())
          flag = true;
        ((BaseOrgPerson) row.DataBoundItem).IsEdit = false;
      }
      this.tsbAddPerson.Enabled = true;
      this.tsbDeletePerson.Enabled = true;
      if (flag)
        return;
      this.LoadPersons();
    }

    private bool SavePerson()
    {
      if (this.dgvPersons.Rows.Count > 0 && this.dgvPersons.CurrentRow.Index >= 0)
      {
        this.session = Domain.CurrentSession;
        BaseOrgPerson dataBoundItem = (BaseOrgPerson) this.dgvPersons.CurrentRow.DataBoundItem;
        if (dataBoundItem.Family == null || dataBoundItem.Name == null)
        {
          int num = (int) MessageBox.Show("Введите имя и фамилию", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
        int personId = dataBoundItem.PersonId;
        bool flag;
        if ((uint) dataBoundItem.PersonId > 0U)
        {
          flag = false;
        }
        else
        {
          IList<int> intList = this.session.CreateSQLQuery("select DBA.gen_id('base_org_persons',1)").List<int>();
          dataBoundItem.PersonId = intList[0];
          flag = true;
        }
        if (this.dgvPersons.CurrentRow.Cells["IncRep"].Value != null)
          dataBoundItem.IncRep = this.session.Get<YesNo>((object) Convert.ToInt16(this.dgvPersons.CurrentRow.Cells["IncRep"].Value));
        if (dataBoundItem.EMail == null)
          dataBoundItem.EMail = "";
        if (dataBoundItem.ICQ == null)
          dataBoundItem.ICQ = "";
        if (dataBoundItem.LastName == null)
          dataBoundItem.LastName = "";
        if (dataBoundItem.Note == null)
          dataBoundItem.Note = "";
        if (dataBoundItem.BasedOn == null)
          dataBoundItem.BasedOn = "";
        if (dataBoundItem.Phone == null)
          dataBoundItem.Phone = "";
        if (dataBoundItem.PlaceWork == null)
          dataBoundItem.PlaceWork = "";
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
        }
        this.session.Clear();
      }
      return true;
    }

    private void dgvPersons_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      ((BaseOrgPerson) this.dgvPersons.CurrentRow.DataBoundItem).IsEdit = true;
      this.tsbDeletePerson.Enabled = false;
      this.tsbSavePerson.Enabled = true;
    }

    private void dgvPersons_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      BaseOrgPerson dataBoundItem = (BaseOrgPerson) this.dgvPersons.CurrentRow.DataBoundItem;
      dataBoundItem.IsEdit = true;
      if (this.dgvPersons.CurrentCell.Value == null || !(this.dgvPersons.Columns[e.ColumnIndex].Name == "IncRep"))
        return;
      try
      {
        dataBoundItem.IncRep = this.session.Get<YesNo>(this.dgvPersons.CurrentRow.Cells["IncRep"].Value);
      }
      catch
      {
      }
    }

    private void btnDeletePerson_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(75, 2, Options.Company, true))
        return;
      this.session = Domain.CurrentSession;
      if (this.dgvPersons.Rows.Count > 0 && this.dgvPersons.CurrentRow.Index >= 0)
      {
        BaseOrgPerson dataBoundItem = (BaseOrgPerson) this.dgvPersons.CurrentRow.DataBoundItem;
        if (MessageBox.Show("Вы уверены, что хотите удалить запись", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
        {
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
          this.LoadPersons();
        }
      }
      this.session.Clear();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.toolStrip1 = new ToolStrip();
      this.tsbAddPerson = new ToolStripButton();
      this.tsbSavePerson = new ToolStripButton();
      this.tsbDeletePerson = new ToolStripButton();
      this.dgvPersons = new DataGridView();
      this.toolStrip1.SuspendLayout();
      ((ISupportInitialize) this.dgvPersons).BeginInit();
      this.SuspendLayout();
      this.toolStrip1.Font = new Font("Tahoma", 10f);
      this.toolStrip1.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.tsbAddPerson,
        (ToolStripItem) this.tsbSavePerson,
        (ToolStripItem) this.tsbDeletePerson
      });
      this.toolStrip1.Location = new Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new Size(521, 25);
      this.toolStrip1.TabIndex = 5;
      this.toolStrip1.Text = "toolStrip1";
      this.tsbAddPerson.Image = (Image) Resources.add_var;
      this.tsbAddPerson.ImageTransparentColor = Color.Magenta;
      this.tsbAddPerson.Name = "tsbAddPerson";
      this.tsbAddPerson.Size = new Size(91, 22);
      this.tsbAddPerson.Text = "Добавить";
      this.tsbAddPerson.Click += new EventHandler(this.btnAddPerson_Click);
      this.tsbSavePerson.Enabled = false;
      this.tsbSavePerson.Image = (Image) Resources.Applay_var;
      this.tsbSavePerson.ImageTransparentColor = Color.Magenta;
      this.tsbSavePerson.Name = "tsbSavePerson";
      this.tsbSavePerson.Size = new Size(99, 22);
      this.tsbSavePerson.Text = "Сохранить";
      this.tsbSavePerson.Click += new EventHandler(this.btnSavePerson_Click);
      this.tsbDeletePerson.Image = (Image) Resources.delete_var;
      this.tsbDeletePerson.ImageTransparentColor = Color.Magenta;
      this.tsbDeletePerson.Name = "tsbDeletePerson";
      this.tsbDeletePerson.Size = new Size(82, 22);
      this.tsbDeletePerson.Text = "Удалить";
      this.tsbDeletePerson.Click += new EventHandler(this.btnDeletePerson_Click);
      this.dgvPersons.BackgroundColor = Color.AliceBlue;
      this.dgvPersons.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvPersons.Dock = DockStyle.Fill;
      this.dgvPersons.Location = new Point(0, 25);
      this.dgvPersons.Name = "dgvPersons";
      this.dgvPersons.Size = new Size(521, 257);
      this.dgvPersons.TabIndex = 4;
      this.dgvPersons.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvPersons_CellBeginEdit);
      this.dgvPersons.CellEndEdit += new DataGridViewCellEventHandler(this.dgvPersons_CellEndEdit);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(521, 322);
      this.Controls.Add((Control) this.dgvPersons);
      this.Controls.Add((Control) this.toolStrip1);
      this.Name = "FrmOrgPersons";
      this.Text = "Список сотрудников";
      this.Load += new EventHandler(this.FrmOrgPersons_Load);
      this.Shown += new EventHandler(this.FrmOrgPersons_Shown);
      this.Controls.SetChildIndex((Control) this.toolStrip1, 0);
      this.Controls.SetChildIndex((Control) this.dgvPersons, 0);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      ((ISupportInitialize) this.dgvPersons).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
