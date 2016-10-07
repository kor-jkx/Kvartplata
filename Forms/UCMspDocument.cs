// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.UCMspDocument
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using NHibernate;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class UCMspDocument : UserControl
  {
    private bool isNew = false;
    private IContainer components = (IContainer) null;
    private ISession session;
    private Panel panelInf;
    private Label label1;
    private Panel panel2;
    private ComboBox cmbDocument;
    private TextBox tbSeria;
    private Label label3;
    private Label label2;
    private TextBox tbNumber;
    private Label label4;
    private PictureBox pb;
    private Label label6;
    private TextBox tbSource;
    private Label label5;
    private Button btnSave;
    private MaskedTextBox mtbDateIssue;
    private Label lblDedit;
    private TextBox txbUname;
    private Label lblUname;
    private TextBox txbDedit;
    private Button btnChange;
    private OpenFileDialog ofdPicture;

    public bool Selected { get; set; }

    public MspDocument MspDocument { get; set; }

    public event EventHandler SelectedChanged;

    public UCMspDocument(MspDocument mspDocument, bool isnew)
    {
      this.InitializeComponent();
      this.MspDocument = mspDocument;
      this.session = Domain.CurrentSession;
      this.cmbDocument.DataSource = (object) this.session.CreateQuery("from MSPDoc").List<MSPDoc>();
      this.cmbDocument.DisplayMember = "MSPDocName";
      this.cmbDocument.ValueMember = "MSPDocId";
      this.isNew = isnew;
      this.LoadData();
    }

    public void LoadData()
    {
      if (this.MspDocument != null)
      {
        if (this.MspDocument.MSPDoc != null)
          this.cmbDocument.SelectedValue = (object) this.MspDocument.MSPDoc.MSPDocId;
        this.tbSeria.Text = this.MspDocument.Series;
        this.tbNumber.Text = this.MspDocument.Number;
        this.tbSource.Text = this.MspDocument.Source;
        this.txbUname.Text = this.MspDocument.Uname;
        if (this.MspDocument.PicPath != "")
          this.pb.ImageLocation = this.MspDocument.PicPath;
        DateTime dateTime = this.MspDocument.Dedit.Date;
        if (dateTime.ToShortDateString() != "01.01.0001" && this.txbDedit != null)
        {
          TextBox txbDedit = this.txbDedit;
          dateTime = this.MspDocument.Dedit;
          dateTime = dateTime.Date;
          string shortDateString = dateTime.ToShortDateString();
          txbDedit.Text = shortDateString;
        }
        MaskedTextBox mtbDateIssue = this.mtbDateIssue;
        dateTime = this.MspDocument.DateIssue;
        string shortDateString1 = dateTime.ToShortDateString();
        mtbDateIssue.Text = shortDateString1;
      }
      if (!this.isNew)
        this.btnSave.Enabled = false;
      else
        this.btnSave.Enabled = true;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      this.session.Clear();
      this.session = Domain.CurrentSession;
      MspDocument mspDocument = new MspDocument();
      mspDocument.MSPDoc = this.session.Get<MSPDoc>((object) ((MSPDoc) this.cmbDocument.SelectedItem).MSPDocId);
      mspDocument.MSPDocumentId = !this.isNew ? this.MspDocument.MSPDocumentId : (int) this.session.CreateSQLQuery("select DBA.gen_id('mspDocument',1)").UniqueResult();
      mspDocument.Number = this.tbNumber.Text;
      mspDocument.Person = this.MspDocument.Person;
      mspDocument.Series = this.tbSeria.Text;
      mspDocument.Source = this.tbSource.Text;
      try
      {
        mspDocument.DateIssue = Convert.ToDateTime(this.mtbDateIssue.Text);
        if (mspDocument.DateIssue <= Convert.ToDateTime("1.1.1900") || mspDocument.DateIssue >= Convert.ToDateTime("31.12.2999"))
        {
          int num = (int) MessageBox.Show("Проверьте корректность даты выдачи!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.mtbDateIssue.Focus();
          return;
        }
      }
      catch
      {
        int num = (int) MessageBox.Show("Некорректная дата выдачи!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.mtbDateIssue.Focus();
        return;
      }
      mspDocument.PicPath = string.IsNullOrEmpty(this.pb.ImageLocation) ? "" : this.pb.ImageLocation;
      mspDocument.Dedit = DateTime.Now;
      mspDocument.Uname = Options.Login;
      if (this.isNew)
      {
        try
        {
          this.session.Save((object) mspDocument);
          this.session.Flush();
          this.isNew = false;
        }
        catch (Exception ex)
        {
          if (ex.InnerException.Message.ToLower().IndexOf("primary key for table 'mspdocument' is not unique") != -1)
          {
            KvrplHelper.ResetGeners("mspDocument", "mspDocument_id");
            int num = (int) MessageBox.Show("Была устранена ошибка генерации уникального поля! Введите документ заново!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return;
          }
          int num1 = (int) MessageBox.Show("Не могу сохранить текущую запись!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
      }
      else
      {
        try
        {
          this.session.CreateSQLQuery("update DBA.mspDocument set  mspdoc_id=:mspdoc_id,  series=:series, number=:number, source=:source, dateissue=:dateissue, picpath=:picpath,  uname=:uname,  dedit=:dedit  where mspdocument_id=:mspdocument_id").SetParameter<short>("mspdoc_id", mspDocument.MSPDoc.MSPDocId).SetParameter<string>("series", mspDocument.Series).SetParameter<string>("number", mspDocument.Number).SetParameter<string>("source", mspDocument.Source).SetParameter<string>("dateissue", KvrplHelper.DateToBaseFormat(mspDocument.DateIssue)).SetParameter<string>("picpath", mspDocument.PicPath).SetParameter<string>("uname", Options.Login).SetParameter<string>("dedit", KvrplHelper.DateToBaseFormat(DateTime.Now)).SetParameter<int>("mspdocument_id", mspDocument.MSPDocumentId).ExecuteUpdate();
          this.session.Flush();
        }
        catch
        {
          int num = (int) MessageBox.Show("Не могу сохранить текущую запись!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
      }
      this.btnSave.Enabled = false;
      this.txbUname.Text = mspDocument.Uname;
      DateTime dateTime1 = mspDocument.Dedit;
      dateTime1 = dateTime1.Date;
      if (!(dateTime1.ToShortDateString() != "01.01.0001"))
        return;
      TextBox txbDedit = this.txbDedit;
      DateTime dateTime2 = mspDocument.Dedit;
      dateTime2 = dateTime2.Date;
      string shortDateString = dateTime2.ToShortDateString();
      txbDedit.Text = shortDateString;
    }

    private void UCMspDocument_Paint(object sender, PaintEventArgs e)
    {
      SolidBrush solidBrush = new SolidBrush(this.Selected ? Color.Gainsboro : Color.OldLace);
      e.Graphics.FillRectangle((Brush) solidBrush, this.DisplayRectangle);
      solidBrush.Dispose();
    }

    private void panelInf_Paint(object sender, PaintEventArgs e)
    {
      LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Point(0, 0), new Point(this.Width), this.Selected ? Color.Gainsboro : Color.PapayaWhip, this.Selected ? Color.Gainsboro : Color.PapayaWhip);
      e.Graphics.FillRectangle((Brush) linearGradientBrush, this.DisplayRectangle);
      linearGradientBrush.Dispose();
    }

    private void UCMspDocument_Click(object sender, EventArgs e)
    {
      if (this.Selected)
        return;
      this.Selected = true;
      this.Invalidate();
      // ISSUE: reference to a compiler-generated field
      if (this.SelectedChanged != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.SelectedChanged((object) this, EventArgs.Empty);
      }
    }

    private void mtbDateIssue_TextChanged(object sender, EventArgs e)
    {
      this.btnSave.Enabled = true;
    }

    private void cmbDocument_TextChanged(object sender, EventArgs e)
    {
      this.btnSave.Enabled = true;
    }

    private void tbSeria_TextChanged(object sender, EventArgs e)
    {
      this.btnSave.Enabled = true;
    }

    private void tbNumber_TextChanged(object sender, EventArgs e)
    {
      this.btnSave.Enabled = true;
    }

    private void tbSource_TextChanged(object sender, EventArgs e)
    {
      this.btnSave.Enabled = true;
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
    }

    private void label7_Click(object sender, EventArgs e)
    {
    }

    private void btnChange_Click(object sender, EventArgs e)
    {
      this.ofdPicture.FileName = this.MspDocument.PicPath;
      if (this.ofdPicture.ShowDialog() != DialogResult.OK)
        return;
      this.MspDocument.PicPath = this.ofdPicture.FileName;
      this.pb.ImageLocation = this.ofdPicture.FileName;
      this.btnSave.Enabled = true;
    }

    private void pb_Click(object sender, EventArgs e)
    {
      Form form = new Form();
      if (Form.ActiveForm != null)
        form.Icon = Form.ActiveForm.Icon;
      form.Height = Form.ActiveForm.Height / 2;
      form.Width = Form.ActiveForm.Width / 2;
      form.StartPosition = FormStartPosition.CenterScreen;
      form.AutoScroll = true;
      PictureBox pictureBox = new PictureBox();
      pictureBox.Image = this.pb.Image;
      pictureBox.Parent = (Control) form;
      pictureBox.Height = this.pb.Image.Height;
      pictureBox.Width = this.pb.Image.Width;
      int num = (int) form.ShowDialog((IWin32Window) this);
      form.Dispose();
    }

    private void UCMspDocument_Load(object sender, EventArgs e)
    {
      new ToolTip().SetToolTip((Control) this.pb, "Нажмите для просмотра");
    }

    private void pb_MouseMove(object sender, MouseEventArgs e)
    {
      this.Cursor = Cursors.Hand;
    }

    private void pb_MouseLeave(object sender, EventArgs e)
    {
      this.Cursor = Cursors.Default;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panelInf = new Panel();
      this.label1 = new Label();
      this.panel2 = new Panel();
      this.txbDedit = new TextBox();
      this.lblDedit = new Label();
      this.txbUname = new TextBox();
      this.lblUname = new Label();
      this.mtbDateIssue = new MaskedTextBox();
      this.btnSave = new Button();
      this.pb = new PictureBox();
      this.label6 = new Label();
      this.tbSource = new TextBox();
      this.label5 = new Label();
      this.tbNumber = new TextBox();
      this.label4 = new Label();
      this.tbSeria = new TextBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.cmbDocument = new ComboBox();
      this.btnChange = new Button();
      this.ofdPicture = new OpenFileDialog();
      this.panelInf.SuspendLayout();
      this.panel2.SuspendLayout();
      ((ISupportInitialize) this.pb).BeginInit();
      this.SuspendLayout();
      this.panelInf.BackColor = SystemColors.GradientInactiveCaption;
      this.panelInf.Controls.Add((Control) this.label1);
      this.panelInf.Dock = DockStyle.Top;
      this.panelInf.Location = new Point(0, 0);
      this.panelInf.Name = "panelInf";
      this.panelInf.Size = new Size(447, 23);
      this.panelInf.TabIndex = 0;
      this.panelInf.Click += new EventHandler(this.UCMspDocument_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(3, 5);
      this.label1.Name = "label1";
      this.label1.Size = new Size(58, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Документ";
      this.label1.Click += new EventHandler(this.UCMspDocument_Click);
      this.panel2.BackColor = Color.PapayaWhip;
      this.panel2.Controls.Add((Control) this.btnChange);
      this.panel2.Controls.Add((Control) this.txbDedit);
      this.panel2.Controls.Add((Control) this.lblDedit);
      this.panel2.Controls.Add((Control) this.txbUname);
      this.panel2.Controls.Add((Control) this.lblUname);
      this.panel2.Controls.Add((Control) this.mtbDateIssue);
      this.panel2.Controls.Add((Control) this.btnSave);
      this.panel2.Controls.Add((Control) this.pb);
      this.panel2.Controls.Add((Control) this.label6);
      this.panel2.Controls.Add((Control) this.tbSource);
      this.panel2.Controls.Add((Control) this.label5);
      this.panel2.Controls.Add((Control) this.tbNumber);
      this.panel2.Controls.Add((Control) this.label4);
      this.panel2.Controls.Add((Control) this.tbSeria);
      this.panel2.Controls.Add((Control) this.label3);
      this.panel2.Controls.Add((Control) this.label2);
      this.panel2.Controls.Add((Control) this.cmbDocument);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 23);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(447, 126);
      this.panel2.TabIndex = 1;
      this.panel2.Click += new EventHandler(this.UCMspDocument_Click);
      this.panel2.Paint += new PaintEventHandler(this.panelInf_Paint);
      this.txbDedit.Enabled = false;
      this.txbDedit.Location = new Point((int) byte.MaxValue, 98);
      this.txbDedit.Name = "txbDedit";
      this.txbDedit.Size = new Size(73, 20);
      this.txbDedit.TabIndex = 16;
      this.lblDedit.AutoSize = true;
      this.lblDedit.BackColor = Color.Transparent;
      this.lblDedit.Location = new Point(157, 102);
      this.lblDedit.Name = "lblDedit";
      this.lblDedit.Size = new Size(92, 13);
      this.lblDedit.TabIndex = 15;
      this.lblDedit.Text = "Дата редактир-я";
      this.txbUname.Enabled = false;
      this.txbUname.Location = new Point(85, 98);
      this.txbUname.Name = "txbUname";
      this.txbUname.Size = new Size(65, 20);
      this.txbUname.TabIndex = 14;
      this.txbUname.TextChanged += new EventHandler(this.textBox1_TextChanged);
      this.lblUname.AutoSize = true;
      this.lblUname.BackColor = Color.Transparent;
      this.lblUname.Location = new Point(3, 102);
      this.lblUname.Name = "lblUname";
      this.lblUname.Size = new Size(80, 13);
      this.lblUname.TabIndex = 13;
      this.lblUname.Text = "Пользователь";
      this.lblUname.Click += new EventHandler(this.label7_Click);
      this.mtbDateIssue.Location = new Point(160, 35);
      this.mtbDateIssue.Mask = "00/00/0000";
      this.mtbDateIssue.Name = "mtbDateIssue";
      this.mtbDateIssue.Size = new Size(64, 20);
      this.mtbDateIssue.TabIndex = 12;
      this.mtbDateIssue.ValidatingType = typeof (DateTime);
      this.mtbDateIssue.Click += new EventHandler(this.UCMspDocument_Click);
      this.mtbDateIssue.TextChanged += new EventHandler(this.mtbDateIssue_TextChanged);
      this.btnSave.Image = (Image) Resources.Applay_var;
      this.btnSave.Location = new Point(347, 96);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(93, 23);
      this.btnSave.TabIndex = 11;
      this.btnSave.Text = "Сохранить";
      this.btnSave.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.pb.BackColor = Color.Transparent;
      this.pb.Image = (Image) Resources.img_question;
      this.pb.Location = new Point(6, 12);
      this.pb.Name = "pb";
      this.pb.Size = new Size(62, 65);
      this.pb.TabIndex = 10;
      this.pb.TabStop = false;
      this.pb.Click += new EventHandler(this.pb_Click);
      this.pb.MouseLeave += new EventHandler(this.pb_MouseLeave);
      this.pb.MouseMove += new MouseEventHandler(this.pb_MouseMove);
      this.label6.AutoSize = true;
      this.label6.BackColor = Color.Transparent;
      this.label6.Location = new Point(71, 37);
      this.label6.Name = "label6";
      this.label6.Size = new Size(73, 13);
      this.label6.TabIndex = 8;
      this.label6.Text = "Дата выдачи";
      this.label6.Click += new EventHandler(this.UCMspDocument_Click);
      this.tbSource.Location = new Point(160, 60);
      this.tbSource.Name = "tbSource";
      this.tbSource.Size = new Size(280, 20);
      this.tbSource.TabIndex = 7;
      this.tbSource.Click += new EventHandler(this.UCMspDocument_Click);
      this.tbSource.TextChanged += new EventHandler(this.tbSource_TextChanged);
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Location = new Point(74, 64);
      this.label5.Name = "label5";
      this.label5.Size = new Size(40, 13);
      this.label5.TabIndex = 6;
      this.label5.Text = "Выдан";
      this.label5.Click += new EventHandler(this.UCMspDocument_Click);
      this.tbNumber.Location = new Point(384, 34);
      this.tbNumber.Name = "tbNumber";
      this.tbNumber.Size = new Size(56, 20);
      this.tbNumber.TabIndex = 5;
      this.tbNumber.Click += new EventHandler(this.UCMspDocument_Click);
      this.tbNumber.TextChanged += new EventHandler(this.tbNumber_TextChanged);
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(337, 37);
      this.label4.Name = "label4";
      this.label4.Size = new Size(41, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "Номер";
      this.label4.Click += new EventHandler(this.UCMspDocument_Click);
      this.tbSeria.Location = new Point(274, 34);
      this.tbSeria.Name = "tbSeria";
      this.tbSeria.Size = new Size(57, 20);
      this.tbSeria.TabIndex = 3;
      this.tbSeria.Click += new EventHandler(this.UCMspDocument_Click);
      this.tbSeria.TextChanged += new EventHandler(this.tbSeria_TextChanged);
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Location = new Point(230, 37);
      this.label3.Name = "label3";
      this.label3.Size = new Size(38, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Серия";
      this.label3.Click += new EventHandler(this.UCMspDocument_Click);
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(71, 12);
      this.label2.Name = "label2";
      this.label2.Size = new Size(83, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Вид документа";
      this.label2.Click += new EventHandler(this.UCMspDocument_Click);
      this.cmbDocument.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbDocument.FormattingEnabled = true;
      this.cmbDocument.Location = new Point(160, 8);
      this.cmbDocument.Name = "cmbDocument";
      this.cmbDocument.Size = new Size(280, 21);
      this.cmbDocument.TabIndex = 0;
      this.cmbDocument.TextChanged += new EventHandler(this.cmbDocument_TextChanged);
      this.cmbDocument.Click += new EventHandler(this.UCMspDocument_Click);
      this.btnChange.Location = new Point(3, 80);
      this.btnChange.Name = "btnChange";
      this.btnChange.Size = new Size(76, 21);
      this.btnChange.TabIndex = 17;
      this.btnChange.Text = "Изменить";
      this.btnChange.UseVisualStyleBackColor = true;
      this.btnChange.Click += new EventHandler(this.btnChange_Click);
      this.ofdPicture.FileName = "openFileDialog1";
      this.ofdPicture.Filter = "Image files (*.jpg,*.img)|*.jpg; *.img";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panelInf);
      this.Name = "UCMspDocument";
      this.Size = new Size(447, 149);
      this.Load += new EventHandler(this.UCMspDocument_Load);
      this.Paint += new PaintEventHandler(this.UCMspDocument_Paint);
      this.panelInf.ResumeLayout(false);
      this.panelInf.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      ((ISupportInitialize) this.pb).EndInit();
      this.ResumeLayout(false);
    }
  }
}
