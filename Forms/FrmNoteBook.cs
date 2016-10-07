// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmNoteBook
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
  public class FrmNoteBook : FrmBaseForm
  {
    private readonly FormStateSaver fss = new FormStateSaver(FrmNoteBook.ic);
    protected GridSettings MySettingsNoteBook = new GridSettings();
    private IContainer components = (IContainer) null;
    private static IContainer ic;
    private readonly LsClient client;
    private readonly Company company;
    private Period monthClosed;
    private IList<NoteBook> oldListNoteBook;
    private NoteBook oldNoteBook;
    private ISession session;
    private Panel pnBtn;
    private Button btnExit;
    private Button btnAdd;
    private Button btnSave;
    private Button btnDel;
    private ContextMenuStrip cmsNoteBook;
    private ToolStripMenuItem miAdd;
    private ToolStripMenuItem miDelete;
    private Panel pnUp;
    private DataGridView dgvNoteBook;
    private ComboBox cmbFilter;
    private Label lblFilter;
    private Label lblMonth;
    private ComboBox cmbMonth;
    public HelpProvider hp;

    public FrmNoteBook()
    {
      this.InitializeComponent();
    }

    public FrmNoteBook(Company company, LsClient client)
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
      this.client = client;
      this.company = company;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void FrmNoteBook_Load(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      this.session.Clear();
      this.MySettingsNoteBook.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
      this.monthClosed = KvrplHelper.GetCmpKvrClose(this.company, Options.ComplexPasp.ComplexId, Options.ComplexPrior.ComplexId);
      if (this.client == null)
        this.cmbFilter.Enabled = false;
      this.cmbFilter.SelectedIndex = 0;
      IList<Period> periodList = this.session.CreateCriteria(typeof (Period)).AddOrder(Order.Desc("PeriodId")).Add((ICriterion) Restrictions.Not((ICriterion) Restrictions.Eq("PeriodId", (object) 0))).Add((ICriterion) Restrictions.Le("PeriodId", (object) (this.monthClosed.PeriodId + 1))).List<Period>();
      periodList.Insert(0, new Period()
      {
        PeriodId = 0,
        PeriodName = new DateTime?()
      });
      this.cmbMonth.DataSource = (object) periodList;
      this.cmbMonth.ValueMember = "PeriodId";
      this.cmbMonth.DisplayMember = "PeriodName";
      this.cmbMonth.SelectedValue = (object) 0;
      this.LoadNoteBook(0, (Period) this.cmbMonth.SelectedItem);
    }

    private void LoadNoteBook(int code, Period period)
    {
      this.session = Domain.CurrentSession;
      this.dgvNoteBook.Columns.Clear();
      this.dgvNoteBook.DataSource = (object) null;
      this.btnSave.Enabled = false;
      this.btnAdd.Enabled = true;
      this.btnDel.Enabled = true;
      IList<NoteBook> noteBookList1 = (IList<NoteBook>) new List<NoteBook>();
      string str = "";
      if (this.client != null)
      {
        switch (code)
        {
          case 0:
            str = "(ClientId={0} or (Company.CompanyId={1} and ClientId=0)) and TypeNoteBook.TypeNoteBookId!=4 ";
            break;
          case 1:
            str = "ClientId={0} and TypeNoteBook.TypeNoteBookId!=4 ";
            break;
          case 2:
            str = "(Company.CompanyId={1} and ClientId=0) and TypeNoteBook.TypeNoteBookId!=4 ";
            break;
        }
      }
      if (period != null && (uint) period.PeriodId > 0U)
        str += string.Format(" and DBeg>='{0}' and DBeg<='{1}'", (object) KvrplHelper.DateToBaseFormat(period.PeriodName.Value), (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(period.PeriodName.Value)));
      IList<NoteBook> noteBookList2 = this.client != null ? this.session.CreateQuery(string.Format("select n from NoteBook n where " + str + " order by DBeg desc", (object) this.client.ClientId, (object) this.client.Company.CompanyId)).List<NoteBook>() : this.session.CreateQuery(string.Format("select n from NoteBook n where Company.CompanyId={0} and IdHome=0 and ClientId=0 " + str + " order by DBeg desc", (object) this.company.CompanyId)).List<NoteBook>();
      if (this.client != null && !KvrplHelper.CheckProxy(48, 1, this.company, false))
      {
        foreach (NoteBook noteBook in (IEnumerable<NoteBook>) noteBookList2)
        {
          IList<Person> personList = this.session.CreateCriteria(typeof (Person)).Add((ICriterion) Restrictions.Eq("LsClient", (object) this.client)).Add((ICriterion) Restrictions.In("Reg.RegId", (ICollection) new int[2]{ 1, 2 })).Add((ICriterion) Restrictions.Or((ICriterion) Restrictions.Lt("Archive", (object) 3), (ICriterion) Restrictions.Eq("Archive", (object) 5))).AddOrder(Order.Asc("Archive")).AddOrder(Order.Asc("Relation.RelationId")).List<Person>();
          foreach (Person person in (IEnumerable<Person>) personList)
            KvrplHelper.GetFamily(person, 1, true);
          foreach (Person person in (IEnumerable<Person>) personList)
          {
            if (noteBook.Text.IndexOf(person.FIO) >= 0)
              noteBook.Text = noteBook.Text.Replace(person.FIO, person.PersonId.ToString());
          }
        }
      }
      this.dgvNoteBook.DataSource = (object) noteBookList2;
      this.oldListNoteBook = (IList<NoteBook>) new List<NoteBook>();
      foreach (NoteBook noteBook in (List<NoteBook>) this.dgvNoteBook.DataSource)
      {
        noteBook.OldHashCode = noteBook.GetHashCode();
        noteBook.IsEdit = false;
        this.oldListNoteBook.Add(new NoteBook()
        {
          Note = noteBook.Note,
          DBeg = noteBook.DBeg,
          OldHashCode = noteBook.OldHashCode
        });
      }
      this.MySettingsNoteBook.GridName = "NoteBook";
      this.SetViewNoteBook();
      this.dgvNoteBook.Focus();
      this.session.Clear();
    }

    private void LoadSettingsNoteBook()
    {
      this.MySettingsNoteBook.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvNoteBook.Columns)
        this.MySettingsNoteBook.GetMySettings(column);
    }

    private void dgvNoteBook_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsNoteBook.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsNoteBook.Columns[this.MySettingsNoteBook.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsNoteBook.Save();
    }

    private void SetViewNoteBook()
    {
      KvrplHelper.AddMaskDateColumn(this.dgvNoteBook, 0, "Дата ввода", "DBeg");
      KvrplHelper.AddMaskDateColumn(this.dgvNoteBook, 1, "Дата окончания", "DEnd");
      this.dgvNoteBook.Columns["Text"].HeaderText = "Запись";
      this.dgvNoteBook.Columns["Note"].HeaderText = "Примечание";
      this.dgvNoteBook.Columns["Text"].ReadOnly = true;
      KvrplHelper.AddComboBoxColumn(this.dgvNoteBook, 4, (IList) this.session.CreateCriteria(typeof (TypeNoteBook)).AddOrder(Order.Asc("TypeNoteBookId")).List<TypeNoteBook>(), "TypeNoteBookId", "TypeNoteBookName", "Тип", "TypeNoteBook", 7, 120);
      KvrplHelper.ViewEdit(this.dgvNoteBook);
      foreach (DataGridViewRow row in (IEnumerable) this.dgvNoteBook.Rows)
      {
        DataGridViewCell cell1 = row.Cells["DBeg"];
        DateTime dateTime = ((NoteBook) row.DataBoundItem).DBeg;
        string shortDateString1 = dateTime.ToShortDateString();
        cell1.Value = (object) shortDateString1;
        DataGridViewCell cell2 = row.Cells["DEnd"];
        dateTime = ((NoteBook) row.DataBoundItem).DEnd;
        string shortDateString2 = dateTime.ToShortDateString();
        cell2.Value = (object) shortDateString2;
        if (((NoteBook) row.DataBoundItem).TypeNoteBook != null)
          row.Cells["TypeNoteBook"].Value = (object) ((NoteBook) row.DataBoundItem).TypeNoteBook.TypeNoteBookId;
      }
      this.LoadSettingsNoteBook();
    }

    private void dgvNoteBook_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e, this.client.ClientId);
    }

    private void InsertNote()
    {
      this.btnSave.Enabled = true;
      NoteBook noteBook = new NoteBook();
      if (this.client != null)
      {
        noteBook.ClientId = this.client.ClientId;
        noteBook.IdHome = this.client.Home.IdHome;
      }
      else
      {
        noteBook.ClientId = 0;
        noteBook.IdHome = 0;
      }
      noteBook.Company = this.company;
      noteBook.DBeg = DateTime.Now;
      noteBook.DEnd = Convert.ToDateTime("31.12.2999");
      noteBook.Text = "Запись бухгалтера";
      noteBook.TypeNoteBook = this.session.Get<TypeNoteBook>((object) 1);
      IList<NoteBook> noteBookList = (IList<NoteBook>) new List<NoteBook>();
      if ((uint) this.dgvNoteBook.Rows.Count > 0U)
        noteBookList = (IList<NoteBook>) (this.dgvNoteBook.DataSource as List<NoteBook>);
      noteBookList.Add(noteBook);
      this.dgvNoteBook.Columns.Clear();
      this.dgvNoteBook.DataSource = (object) null;
      this.dgvNoteBook.DataSource = (object) noteBookList;
      this.SetViewNoteBook();
      this.dgvNoteBook.CurrentCell = this.dgvNoteBook.Rows[this.dgvNoteBook.Rows.Count - 1].Cells[0];
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      this.InsertNote();
    }

    private void SaveAllNote()
    {
      bool flag = false;
      foreach (DataGridViewRow row in (IEnumerable) this.dgvNoteBook.Rows)
      {
        if (((NoteBook) row.DataBoundItem).IsEdit)
        {
          this.oldNoteBook = new NoteBook();
          foreach (NoteBook noteBook in (IEnumerable<NoteBook>) this.oldListNoteBook)
          {
            if (noteBook.OldHashCode == ((NoteBook) row.DataBoundItem).OldHashCode)
            {
              this.oldNoteBook = noteBook;
              break;
            }
          }
          this.dgvNoteBook.Rows[row.Index].Selected = true;
          this.dgvNoteBook.CurrentCell = row.Cells[0];
          if (!this.SaveNoteBook())
            flag = true;
          else
            ((NoteBook) row.DataBoundItem).IsEdit = false;
        }
      }
      if (flag)
        return;
      this.LoadNoteBook(this.cmbFilter.SelectedIndex, (Period) this.cmbMonth.SelectedItem);
    }

    private bool SaveNoteBook()
    {
      NoteBook dataBoundItem = (NoteBook) this.dgvNoteBook.CurrentRow.DataBoundItem;
      try
      {
        dataBoundItem.DBeg = Convert.ToDateTime(this.dgvNoteBook.CurrentRow.Cells["DBeg"].Value);
        dataBoundItem.DEnd = Convert.ToDateTime(this.dgvNoteBook.CurrentRow.Cells["DEnd"].Value);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Даты внесены некорректно", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (dataBoundItem.DBeg < this.monthClosed.PeriodName.Value.AddMonths(1))
      {
        int num = (int) MessageBox.Show("Запись принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand);
        return true;
      }
      int noteId = dataBoundItem.NoteId;
      bool flag = dataBoundItem.NoteId == 0;
      if (this.dgvNoteBook.CurrentRow.Cells["Text"].Value == null)
      {
        int num = (int) MessageBox.Show("Внесите текст записи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.dgvNoteBook.CurrentRow.Cells["Note"].Value == null)
        this.dgvNoteBook.CurrentRow.Cells["Note"].Value = (object) "";
      if (dataBoundItem.DBeg > dataBoundItem.DEnd)
      {
        int num = (int) MessageBox.Show("Дата начала больше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.dgvNoteBook.CurrentRow.Cells["TypeNoteBook"].Value != null)
        dataBoundItem.TypeNoteBook = this.session.Get<TypeNoteBook>((object) Convert.ToInt32(this.dgvNoteBook.CurrentRow.Cells["TypeNoteBook"].Value));
      this.session = Domain.CurrentSession;
      this.session.Clear();
      dataBoundItem.UName = Options.Login;
      dataBoundItem.DEdit = DateTime.Now;
      try
      {
        if (flag)
        {
          int num = this.client != null ? Convert.ToInt32(this.session.CreateQuery(string.Format("select max(NoteId) from NoteBook where ClientId={0}", (object) this.client.ClientId)).UniqueResult()) : Convert.ToInt32(this.session.CreateQuery(string.Format("select max(NoteId) from NoteBook where Company.CompanyId={0} and ClientId=0", (object) this.company.CompanyId)).UniqueResult());
          dataBoundItem.NoteId = Convert.ToInt32(num + 1);
          this.session.Save((object) dataBoundItem);
          this.session.Flush();
        }
        else
          this.session.CreateQuery("update NoteBook set Text=:text,Note=:note,DBeg=:dbeg,DEnd=:dend, UName=:uname, DEdit=:dedit, TypeNoteBook.TypeNoteBookId=:type where Company.CompanyId=:company and IdHome=:home and ClientId=:client and NoteId=:noteid ").SetParameter<string>("text", dataBoundItem.Text).SetParameter<string>("note", dataBoundItem.Note).SetParameter<string>("uname", dataBoundItem.UName).SetParameter<DateTime>("dedit", dataBoundItem.DEdit).SetParameter<short>("company", this.company.CompanyId).SetParameter<int>("home", dataBoundItem.IdHome).SetParameter<int>("client", dataBoundItem.ClientId).SetParameter<int>("noteid", dataBoundItem.NoteId).SetParameter<DateTime>("dbeg", dataBoundItem.DBeg).SetParameter<DateTime>("dend", dataBoundItem.DEnd).SetParameter<int>("type", dataBoundItem.TypeNoteBook.TypeNoteBookId).ExecuteUpdate();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Невозможно сохранить изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      this.session.Clear();
      return true;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      this.SaveAllNote();
    }

    private void dgvNoteBook_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.btnDel.Enabled = false;
      this.btnSave.Enabled = true;
      ((NoteBook) this.dgvNoteBook.CurrentRow.DataBoundItem).IsEdit = true;
    }

    private void DelNote()
    {
      if (this.dgvNoteBook.Rows.Count <= 0 || this.dgvNoteBook.CurrentRow == null)
        return;
      NoteBook dataBoundItem = (NoteBook) this.dgvNoteBook.CurrentRow.DataBoundItem;
      if (dataBoundItem.DBeg < this.monthClosed.PeriodName.Value.AddMonths(1))
      {
        int num1 = (int) MessageBox.Show("Запись принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.client != null && dataBoundItem.ClientId == 0)
      {
        int num2 = (int) MessageBox.Show("Запись по домоуправлению можно удалить только из списка домов", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand);
      }
      else if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
      {
        this.session = Domain.CurrentSession;
        try
        {
          this.session.Delete((object) dataBoundItem);
          this.session.Flush();
        }
        catch (Exception ex)
        {
          int num3 = (int) MessageBox.Show("Невозможно удалить запись", "Ошибка", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand);
        }
        this.session.Clear();
      }
    }

    private void btnDel_Click(object sender, EventArgs e)
    {
      this.DelNote();
      this.LoadNoteBook(this.cmbFilter.SelectedIndex, (Period) this.cmbMonth.SelectedItem);
    }

    private void dgvNoteBook_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.RowIndex == -1 || e.ColumnIndex == -1)
        return;
      ((DataGridView) sender).Rows[e.RowIndex].Selected = true;
      this.dgvNoteBook.CurrentCell = this.dgvNoteBook.Rows[e.RowIndex].Cells[e.ColumnIndex];
    }

    private void miAdd_Click(object sender, EventArgs e)
    {
      bool flag = true;
      short int16 = Convert.ToInt16(((ToolStripItem) sender).Tag);
      if (this.dgvNoteBook.Rows.Count <= 0 || this.dgvNoteBook.CurrentRow.Index < 0)
        return;
      NoteBook noteBook = new NoteBook();
      NoteBook dataBoundItem = (NoteBook) this.dgvNoteBook.Rows[this.dgvNoteBook.CurrentRow.Index].DataBoundItem;
      if (dataBoundItem.ClientId == 0)
      {
        if ((int) int16 == 1)
        {
          int num1 = (int) MessageBox.Show("Запись по участку присутсвует по всех лицевых и не требует копирования", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        if ((int) int16 != 2)
          return;
        int num2 = (int) MessageBox.Show("Запись по участку, для удаления зайдите в блокнот по участку", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (dataBoundItem.DBeg >= this.monthClosed.PeriodName.Value.AddMonths(1))
      {
        FrmChooseObject frmChooseObject = new FrmChooseObject(dataBoundItem);
        frmChooseObject.Save = flag;
        frmChooseObject.CodeOperation = int16;
        frmChooseObject.MonthClosed = this.monthClosed;
        int num = (int) frmChooseObject.ShowDialog();
        frmChooseObject.Dispose();
      }
      else
      {
        int num3 = (int) MessageBox.Show("Не могу выполнить операцию, так как запись принадлежит закрытому периоду!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void cmbFilter_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.LoadNoteBook(this.cmbFilter.SelectedIndex, (Period) this.cmbMonth.SelectedItem);
    }

    private void cmbMonth_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.LoadNoteBook(this.cmbFilter.SelectedIndex, (Period) this.cmbMonth.SelectedItem);
    }

    private void cmsNoteBook_Opening(object sender, CancelEventArgs e)
    {
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
      this.pnBtn = new Panel();
      this.btnDel = new Button();
      this.btnSave = new Button();
      this.btnAdd = new Button();
      this.btnExit = new Button();
      this.cmsNoteBook = new ContextMenuStrip(this.components);
      this.miAdd = new ToolStripMenuItem();
      this.miDelete = new ToolStripMenuItem();
      this.pnUp = new Panel();
      this.cmbMonth = new ComboBox();
      this.lblMonth = new Label();
      this.cmbFilter = new ComboBox();
      this.lblFilter = new Label();
      this.dgvNoteBook = new DataGridView();
      this.hp = new HelpProvider();
      this.pnBtn.SuspendLayout();
      this.cmsNoteBook.SuspendLayout();
      this.pnUp.SuspendLayout();
      ((ISupportInitialize) this.dgvNoteBook).BeginInit();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnDel);
      this.pnBtn.Controls.Add((Control) this.btnSave);
      this.pnBtn.Controls.Add((Control) this.btnAdd);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 391);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(877, 40);
      this.pnBtn.TabIndex = 0;
      this.btnDel.Image = (Image) Resources.minus;
      this.btnDel.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnDel.Location = new Point(118, 5);
      this.btnDel.Name = "btnDel";
      this.btnDel.Size = new Size(100, 29);
      this.btnDel.TabIndex = 3;
      this.btnDel.Text = "Удалить";
      this.btnDel.TextAlign = ContentAlignment.MiddleRight;
      this.btnDel.UseVisualStyleBackColor = true;
      this.btnDel.Click += new EventHandler(this.btnDel_Click);
      this.btnSave.Image = (Image) Resources.Tick;
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.Location = new Point(224, 5);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(108, 29);
      this.btnSave.TabIndex = 2;
      this.btnSave.Text = "Сохранить";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnAdd.Image = (Image) Resources.plus;
      this.btnAdd.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnAdd.Location = new Point(12, 5);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(100, 30);
      this.btnAdd.TabIndex = 1;
      this.btnAdd.Text = "Добавить";
      this.btnAdd.TextAlign = ContentAlignment.MiddleRight;
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(785, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(80, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.cmsNoteBook.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.miAdd,
        (ToolStripItem) this.miDelete
      });
      this.cmsNoteBook.Name = "cmsNoteBook";
      this.cmsNoteBook.Size = new Size(345, 70);
      this.cmsNoteBook.Opening += new CancelEventHandler(this.cmsNoteBook_Opening);
      this.miAdd.Image = (Image) Resources.add_var;
      this.miAdd.Name = "miAdd";
      this.miAdd.Size = new Size(344, 22);
      this.miAdd.Tag = (object) "1";
      this.miAdd.Text = "Скопировать запись в выбранные объекты";
      this.miAdd.Click += new EventHandler(this.miAdd_Click);
      this.miDelete.Image = (Image) Resources.minus;
      this.miDelete.Name = "miDelete";
      this.miDelete.Size = new Size(344, 22);
      this.miDelete.Tag = (object) "2";
      this.miDelete.Text = "Удалить запись из выбранных объектов";
      this.miDelete.Click += new EventHandler(this.miAdd_Click);
      this.pnUp.Controls.Add((Control) this.cmbMonth);
      this.pnUp.Controls.Add((Control) this.lblMonth);
      this.pnUp.Controls.Add((Control) this.cmbFilter);
      this.pnUp.Controls.Add((Control) this.lblFilter);
      this.pnUp.Dock = DockStyle.Top;
      this.pnUp.Location = new Point(0, 0);
      this.pnUp.Name = "pnUp";
      this.pnUp.Size = new Size(877, 52);
      this.pnUp.TabIndex = 1;
      this.cmbMonth.FormatString = "MMMM   yyyy";
      this.cmbMonth.FormattingEnabled = true;
      this.cmbMonth.Location = new Point(416, 15);
      this.cmbMonth.Name = "cmbMonth";
      this.cmbMonth.Size = new Size(137, 24);
      this.cmbMonth.TabIndex = 3;
      this.cmbMonth.SelectionChangeCommitted += new EventHandler(this.cmbMonth_SelectionChangeCommitted);
      this.lblMonth.AutoSize = true;
      this.lblMonth.Location = new Point(361, 18);
      this.lblMonth.Name = "lblMonth";
      this.lblMonth.Size = new Size(49, 16);
      this.lblMonth.TabIndex = 2;
      this.lblMonth.Text = "Месяц";
      this.cmbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbFilter.FormattingEnabled = true;
      this.cmbFilter.Items.AddRange(new object[3]
      {
        (object) "Все",
        (object) "Только по лицевому счету",
        (object) "Только по компании"
      });
      this.cmbFilter.Location = new Point(71, 15);
      this.cmbFilter.Name = "cmbFilter";
      this.cmbFilter.Size = new Size(261, 24);
      this.cmbFilter.TabIndex = 1;
      this.cmbFilter.SelectionChangeCommitted += new EventHandler(this.cmbFilter_SelectionChangeCommitted);
      this.lblFilter.AutoSize = true;
      this.lblFilter.Location = new Point(12, 18);
      this.lblFilter.Name = "lblFilter";
      this.lblFilter.Size = new Size(56, 16);
      this.lblFilter.TabIndex = 0;
      this.lblFilter.Text = "Записи";
      this.dgvNoteBook.BackgroundColor = Color.AliceBlue;
      this.dgvNoteBook.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvNoteBook.ContextMenuStrip = this.cmsNoteBook;
      this.dgvNoteBook.Dock = DockStyle.Fill;
      this.dgvNoteBook.Location = new Point(0, 52);
      this.dgvNoteBook.Name = "dgvNoteBook";
      this.dgvNoteBook.Size = new Size(877, 339);
      this.dgvNoteBook.TabIndex = 2;
      this.dgvNoteBook.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvNoteBook_CellBeginEdit);
      this.dgvNoteBook.CellMouseDown += new DataGridViewCellMouseEventHandler(this.dgvNoteBook_CellMouseDown);
      this.dgvNoteBook.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvNoteBook_ColumnWidthChanged);
      this.dgvNoteBook.DataError += new DataGridViewDataErrorEventHandler(this.dgvNoteBook_DataError);
      this.hp.HelpNamespace = "Help.chm";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(877, 431);
      this.Controls.Add((Control) this.dgvNoteBook);
      this.Controls.Add((Control) this.pnUp);
      this.Controls.Add((Control) this.pnBtn);
      this.hp.SetHelpKeyword((Control) this, "kv133.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      this.Margin = new Padding(5);
      this.Name = "FrmNoteBook";
      this.hp.SetShowHelp((Control) this, true);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Блокнот бухгалтера";
      this.Load += new EventHandler(this.FrmNoteBook_Load);
      this.pnBtn.ResumeLayout(false);
      this.cmsNoteBook.ResumeLayout(false);
      this.pnUp.ResumeLayout(false);
      this.pnUp.PerformLayout();
      ((ISupportInitialize) this.dgvNoteBook).EndInit();
      this.ResumeLayout(false);
    }
  }
}
