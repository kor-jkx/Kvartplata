// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmFileClient
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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmFileClient : Form
  {
    private IContainer components = (IContainer) null;
    private ISession _session;
    private IList<NoteBook> _listNoteBook;
    private readonly LsClient _client;
    private readonly Company _company;
    private readonly int _idHome;
    private Panel panelControl;
    private Label label1;
    private Button butLoadFile;
    private Panel panelMain;
    private DataGridView dgvFile;
    private Button button1;

    public FrmFileClient()
    {
      this.InitializeComponent();
    }

    public FrmFileClient(Company company, LsClient client, int idhome)
    {
      this.InitializeComponent();
      this._client = client;
      this._company = company;
      this._idHome = idhome;
    }

    private void FrmFileClient_Load(object sender, EventArgs e)
    {
      this._session = Domain.CurrentSession;
      this.ConfigDGVFile();
      this.LoadDataDGVFile();
    }

    private void ConfigDGVFile()
    {
      DataGridViewTextBoxColumn viewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      viewTextBoxColumn1.Width = 100;
      viewTextBoxColumn1.HeaderText = "№";
      viewTextBoxColumn1.Name = "NoteId";
      viewTextBoxColumn1.DataPropertyName = "NoteId";
      viewTextBoxColumn1.ReadOnly = true;
      this.dgvFile.Columns.Insert(0, (DataGridViewColumn) viewTextBoxColumn1);
      DataGridViewButtonColumn viewButtonColumn = new DataGridViewButtonColumn();
      viewButtonColumn.HeaderText = "Открыть файл";
      viewButtonColumn.Name = "Open";
      viewButtonColumn.Text = "Открыть";
      viewButtonColumn.UseColumnTextForButtonValue = true;
      viewButtonColumn.Width = 100;
      this.dgvFile.Columns.Insert(1, (DataGridViewColumn) viewButtonColumn);
      DataGridViewTextBoxColumn viewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      viewTextBoxColumn2.Width = this.dgvFile.Width - 100;
      viewTextBoxColumn2.HeaderText = "Путь до файла";
      viewTextBoxColumn2.Name = "Text";
      viewTextBoxColumn2.DataPropertyName = "Text";
      viewTextBoxColumn2.ReadOnly = true;
      this.dgvFile.Columns.Insert(2, (DataGridViewColumn) viewTextBoxColumn2);
      this.dgvFile.CellClick += new DataGridViewCellEventHandler(this.dgvFile_CellClick);
      this.dgvFile.AutoGenerateColumns = false;
    }

    private void dgvFile_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      try
      {
        if (e.ColumnIndex != 1)
          return;
        Process.Start("explorer", this.dgvFile.Rows[e.RowIndex].Cells[2].Value.ToString());
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Не удалось открыть файл");
      }
    }

    private void LoadDataDGVFile()
    {
      try
      {
        this.dgvFile.DataSource = (object) null;
        IList<NoteBook> noteBookList1 = (IList<NoteBook>) new List<NoteBook>();
        IList<NoteBook> noteBookList2 = this._session.CreateQuery("select n from NoteBook n where Company.CompanyId=:cid and IdHome=:hid and ClientId=:lid and TypeNoteBook.TypeNoteBookId=4 ").SetParameter<short>("cid", this._company.CompanyId).SetParameter<int>("hid", this._idHome).SetParameter<int>("lid", this._client.ClientId).List<NoteBook>();
        if (noteBookList2 == null || noteBookList2.Count == 0)
          noteBookList2 = this._session.CreateQuery("select n from NoteBook n where Note=:tx and TypeNoteBook.TypeNoteBookId=4 ").SetParameter<string>("tx", this._client.Home.Str.IdStr.ToString() + ":" + (object) this._client.Home.IdHome + ":" + (object) this._client.Flat.IdFlat).List<NoteBook>();
        this.dgvFile.DataSource = (object) noteBookList2;
        this.dgvFile.Refresh();
      }
      catch (Exception ex)
      {
      }
    }

    private void butLoadFile_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.InitialDirectory = "c:\\";
      openFileDialog.Filter = "pdf file (*.pdf)|*pdf|doc files (*.doc)|*.doc|All files (*.*)|*.*";
      openFileDialog.FilterIndex = 0;
      openFileDialog.RestoreDirectory = true;
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      string fileName = openFileDialog.FileName;
      if (fileName == "")
      {
        int num1 = (int) MessageBox.Show("Не выбран файл!", "Внимание!");
      }
      else
      {
        try
        {
          int idStr = this._client.Home.Str.IdStr;
          int idHome = this._client.Home.IdHome;
          int idFlat = this._client.Flat.IdFlat;
          int num2 = 0;
          string nflat = this._session.Get<Flat>((object) idFlat).NFlat;
          string str1 = this._client.Home.NHome + this._client.Home.HomeKorp;
          string str2 = "";
          string str3 = "";
          IList list1 = this._session.CreateSQLQuery("select * from dba.di_str where idstr=:ids").SetParameter<int>("ids", idStr).List();
          if (list1 != null)
          {
            foreach (IList list2 in (IEnumerable) list1)
            {
              str2 = list2[8] != null && list2[8] != (object) "" ? list2[8].ToString() : list2[9].ToString();
              str3 = list2[2].ToString();
              num2 = Convert.ToInt32(list2[5].ToString());
            }
          }
          string str4 = Options.ConfigValueEmpty("path_folder_main");
          if (str4 == null || str4 == "")
          {
            int num3 = (int) MessageBox.Show("Укажите каталог сохранения файлов в настройках программы", "Ошибка!");
          }
          else
          {
            string str5 = str3.Replace("*", "").Replace("|", "").Replace("\\", "").Replace(":", "").Replace("<", "").Replace(">", "").Replace("?", "").Replace("/", "");
            string str6 = "(" + (object) num2 + ")" + str2.Trim();
            string str7 = str5.Trim();
            string str8 = "(" + Convert.ToString(idHome) + ")" + str1.Trim();
            string str9 = "(" + (object) idStr + ")" + nflat.Trim();
            string path = str4 + "\\" + str6 + "\\" + str7 + "\\" + str8 + "\\" + str9 + "\\";
            if (!Directory.Exists(path))
              Directory.CreateDirectory(path);
            string str10 = "(" + (object) this._client.ClientId + ")" + fileName.Remove(0, fileName.LastIndexOf("\\") + 1);
            string destFileName = path + str10;
            try
            {
              File.Copy(fileName, destFileName);
            }
            catch (Exception ex)
            {
              int num4 = (int) MessageBox.Show("Ошибка копирования файла, возможно такой файл уже существует.", "Ошибка!");
              return;
            }
            string str11 = idStr.ToString() + ":" + (object) idHome + ":" + (object) idFlat;
            string str12 = destFileName;
            NoteBook noteBook = new NoteBook();
            noteBook.Note = str11;
            noteBook.Text = str12;
            noteBook.ClientId = this._client.ClientId;
            noteBook.Company = this._client.Company;
            noteBook.IdHome = this._client.Home.IdHome;
            noteBook.TypeNoteBook = this._session.Get<TypeNoteBook>((object) 4);
            noteBook.DBeg = DateTime.Now;
            noteBook.DEnd = Convert.ToDateTime("31.12.2999");
            noteBook.UName = Options.Login;
            noteBook.DEdit = DateTime.Now;
            int num5 = this._client != null ? Convert.ToInt32(this._session.CreateQuery(string.Format("select max(NoteId) from NoteBook where ClientId={0}", (object) this._client.ClientId)).UniqueResult()) : Convert.ToInt32(this._session.CreateQuery(string.Format("select max(NoteId) from NoteBook where Company.CompanyId={0} and ClientId=0", (object) this._company.CompanyId)).UniqueResult());
            noteBook.NoteId = Convert.ToInt32(num5 + 1);
            this._session.Save((object) noteBook);
            this._session.Flush();
            this.LoadDataDGVFile();
          }
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show("Ошибка сохранения файла.", "Ошибка!");
        }
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      try
      {
        if (MessageBox.Show("Удалить запись и стереть файл?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.No || this.dgvFile.CurrentRow == null)
          return;
        NoteBook dataBoundItem = (NoteBook) this.dgvFile.CurrentRow.DataBoundItem;
        try
        {
          File.Delete(dataBoundItem.Text);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Ошибка удаления файла.", "Ошибка!");
          return;
        }
        this._session.Delete((object) dataBoundItem);
        this._session.Flush();
        this.LoadDataDGVFile();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Ошибка удаления записи.", "Ошибка!");
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmFileClient));
      this.panelControl = new Panel();
      this.button1 = new Button();
      this.label1 = new Label();
      this.butLoadFile = new Button();
      this.panelMain = new Panel();
      this.dgvFile = new DataGridView();
      this.panelControl.SuspendLayout();
      this.panelMain.SuspendLayout();
      ((ISupportInitialize) this.dgvFile).BeginInit();
      this.SuspendLayout();
      this.panelControl.BackColor = Color.AliceBlue;
      this.panelControl.Controls.Add((Control) this.button1);
      this.panelControl.Controls.Add((Control) this.label1);
      this.panelControl.Controls.Add((Control) this.butLoadFile);
      this.panelControl.Dock = DockStyle.Top;
      this.panelControl.Location = new Point(0, 0);
      this.panelControl.Name = "panelControl";
      this.panelControl.Size = new Size(847, 141);
      this.panelControl.TabIndex = 0;
      this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button1.Image = (Image) Resources.delete;
      this.button1.ImageAlign = ContentAlignment.MiddleLeft;
      this.button1.Location = new Point(695, 105);
      this.button1.Name = "button1";
      this.button1.Size = new Size(140, 30);
      this.button1.TabIndex = 2;
      this.button1.Text = "Удалить файл";
      this.button1.TextAlign = ContentAlignment.MiddleRight;
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 25);
      this.label1.Name = "label1";
      this.label1.Size = new Size(202, 17);
      this.label1.TabIndex = 1;
      this.label1.Text = "Выберете файл для загрузки";
      this.butLoadFile.Location = new Point(15, 45);
      this.butLoadFile.Name = "butLoadFile";
      this.butLoadFile.Size = new Size(105, 30);
      this.butLoadFile.TabIndex = 0;
      this.butLoadFile.Text = "Загрузить";
      this.butLoadFile.UseVisualStyleBackColor = true;
      this.butLoadFile.Click += new EventHandler(this.butLoadFile_Click);
      this.panelMain.BackColor = Color.AliceBlue;
      this.panelMain.Controls.Add((Control) this.dgvFile);
      this.panelMain.Dock = DockStyle.Fill;
      this.panelMain.Location = new Point(0, 141);
      this.panelMain.Name = "panelMain";
      this.panelMain.Size = new Size(847, 312);
      this.panelMain.TabIndex = 1;
      this.dgvFile.AllowUserToAddRows = false;
      this.dgvFile.BackgroundColor = Color.AliceBlue;
      this.dgvFile.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvFile.Dock = DockStyle.Fill;
      this.dgvFile.Location = new Point(0, 0);
      this.dgvFile.Name = "dgvFile";
      this.dgvFile.Size = new Size(847, 312);
      this.dgvFile.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(847, 453);
      this.Controls.Add((Control) this.panelMain);
      this.Controls.Add((Control) this.panelControl);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmFileClient";
      this.Text = "Документы лицевого счета";
      this.Load += new EventHandler(this.FrmFileClient_Load);
      this.panelControl.ResumeLayout(false);
      this.panelControl.PerformLayout();
      this.panelMain.ResumeLayout(false);
      ((ISupportInitialize) this.dgvFile).EndInit();
      this.ResumeLayout(false);
    }
  }
}
