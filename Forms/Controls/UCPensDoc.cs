// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.Controls.UCPensDoc
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

namespace Kvartplata.Forms.Controls
{
  public class UCPensDoc : UserControl
  {
    private bool isNew = false;
    private IContainer components = (IContainer) null;
    private ISession session;
    private Panel panel1;
    private Label label1;
    private ComboBox cmbPensType;
    private Label label2;
    private MaskedTextBox mtbDBeg;
    private Label label3;
    private Label label4;
    private MaskedTextBox mtbDEnd;
    private Label label5;
    private MaskedTextBox mtbDateIssue;
    private Label label6;
    private TextBox tbSource;
    private TextBox tbSeriaPens;
    private Label label7;
    private Button btnSave;
    private Panel panelInf;
    private TextBox txbDedit;
    private Label lblDedit;
    private TextBox txbUname;
    private Label lblUname;

    public bool Selected { get; set; }

    public PensDoc PensDoc { get; set; }

    public event EventHandler SelectedChanged;

    public UCPensDoc(PensDoc pensDoc, bool isnew)
    {
      this.InitializeComponent();
      this.PensDoc = pensDoc;
      this.session = Domain.CurrentSession;
      this.cmbPensType.DataSource = (object) this.session.CreateQuery(string.Format("from Pens order by PensTypeId")).List<Kvartplata.Classes.Pens>();
      this.cmbPensType.DisplayMember = "PensTypeName";
      this.cmbPensType.ValueMember = "PensTypeId";
      this.isNew = isnew;
      this.LoadData();
    }

    private void LoadData()
    {
      if (this.PensDoc != null)
      {
        if (this.PensDoc.Pens != null)
          this.cmbPensType.SelectedValue = (object) this.PensDoc.Pens.PensTypeId;
        this.tbSource.Text = this.PensDoc.OutPens;
        this.tbSeriaPens.Text = this.PensDoc.SeriaPens;
        DateTime? datePens = this.PensDoc.DatePens;
        DateTime dateTime;
        if (datePens.HasValue)
        {
          MaskedTextBox mtbDateIssue = this.mtbDateIssue;
          datePens = this.PensDoc.DatePens;
          dateTime = datePens.Value;
          string shortDateString = dateTime.ToShortDateString();
          mtbDateIssue.Text = shortDateString;
        }
        else
          this.mtbDateIssue.Text = "";
        MaskedTextBox mtbDbeg = this.mtbDBeg;
        dateTime = this.PensDoc.DBeg;
        string shortDateString1 = dateTime.ToShortDateString();
        mtbDbeg.Text = shortDateString1;
        MaskedTextBox mtbDend = this.mtbDEnd;
        dateTime = this.PensDoc.DEnd;
        string shortDateString2 = dateTime.ToShortDateString();
        mtbDend.Text = shortDateString2;
        this.txbUname.Text = this.PensDoc.Uname;
        dateTime = this.PensDoc.Dedit;
        dateTime = dateTime.Date;
        if (dateTime.ToShortDateString() != "01.01.0001" && this.PensDoc != null)
        {
          TextBox txbDedit = this.txbDedit;
          dateTime = this.PensDoc.Dedit;
          dateTime = dateTime.Date;
          string shortDateString3 = dateTime.ToShortDateString();
          txbDedit.Text = shortDateString3;
        }
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
      PensDoc pensDoc = new PensDoc();
      if ((Kvartplata.Classes.Pens) this.cmbPensType.SelectedItem == null)
      {
        int num1 = (int) MessageBox.Show("Не проставлен тип пенсии!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        pensDoc.Pens = this.session.Get<Kvartplata.Classes.Pens>((object) ((Kvartplata.Classes.Pens) this.cmbPensType.SelectedItem).PensTypeId);
        pensDoc.IdPensDoc = this.PensDoc.IdPensDoc;
        pensDoc.Person = this.PensDoc.Person;
        pensDoc.SeriaPens = this.tbSeriaPens.Text;
        pensDoc.OutPens = this.tbSource.Text;
        DateTime? datePens;
        if (this.mtbDateIssue.Text != "  .  .")
        {
          try
          {
            pensDoc.DatePens = new DateTime?(Convert.ToDateTime(this.mtbDateIssue.Text));
            datePens = pensDoc.DatePens;
            DateTime dateTime1 = Convert.ToDateTime("1.1.1900");
            int num2;
            if ((datePens.HasValue ? (datePens.GetValueOrDefault() <= dateTime1 ? 1 : 0) : 0) == 0)
            {
              datePens = pensDoc.DatePens;
              DateTime dateTime2 = Convert.ToDateTime("31.12.2999");
              num2 = datePens.HasValue ? (datePens.GetValueOrDefault() > dateTime2 ? 1 : 0) : 0;
            }
            else
              num2 = 1;
            if (num2 != 0)
            {
              int num3 = (int) MessageBox.Show("Проверьте корректность даты выдачи!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.mtbDateIssue.Focus();
              return;
            }
          }
          catch
          {
            int num2 = (int) MessageBox.Show("Некорректная дата выдачи!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.mtbDateIssue.Focus();
            return;
          }
        }
        else
          pensDoc.DatePens = new DateTime?();
        try
        {
          pensDoc.DBeg = Convert.ToDateTime(this.mtbDBeg.Text);
          if (pensDoc.DBeg <= Convert.ToDateTime("1.1.1900") || pensDoc.DBeg > Convert.ToDateTime("31.12.2999"))
          {
            int num2 = (int) MessageBox.Show("Проверьте корректность даты назначения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.mtbDBeg.Focus();
            return;
          }
        }
        catch
        {
          int num2 = (int) MessageBox.Show("Некорректная дата назначения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.mtbDBeg.Focus();
          return;
        }
        try
        {
          pensDoc.DEnd = Convert.ToDateTime(this.mtbDEnd.Text);
          if (pensDoc.DEnd <= Convert.ToDateTime("1.1.1900") || pensDoc.DEnd > Convert.ToDateTime("31.12.2999"))
          {
            int num2 = (int) MessageBox.Show("Проверьте корректность даты окончания!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.mtbDEnd.Focus();
            return;
          }
        }
        catch
        {
          int num2 = (int) MessageBox.Show("Некорректная дата окончания!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.mtbDEnd.Focus();
          return;
        }
        if (pensDoc.DBeg >= pensDoc.DEnd)
        {
          int num2 = (int) MessageBox.Show("Дата назначения больше даты окончания!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.mtbDEnd.Focus();
        }
        else
        {
          datePens = pensDoc.DatePens;
          DateTime dend = pensDoc.DEnd;
          if (datePens.HasValue && datePens.GetValueOrDefault() >= dend)
          {
            int num2 = (int) MessageBox.Show("Дата выдачи больше даты окончания!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.mtbDateIssue.Focus();
          }
          else
          {
            datePens = pensDoc.DatePens;
            DateTime dbeg = pensDoc.DBeg;
            if (datePens.HasValue && datePens.GetValueOrDefault() < dbeg)
            {
              int num2 = (int) MessageBox.Show("Дата выдачи меньше даты назначения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.mtbDateIssue.Focus();
            }
            else
            {
              pensDoc.Dedit = DateTime.Now;
              pensDoc.Uname = Options.Login;
              if (this.isNew)
              {
                try
                {
                  this.session.Save((object) pensDoc);
                  this.session.Flush();
                  this.isNew = false;
                }
                catch
                {
                  int num2 = (int) MessageBox.Show("Не могу сохранить текущую запись!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  return;
                }
              }
              else
              {
                try
                {
                  this.session.CreateSQLQuery("update DBA.PensDoc set  idpens=:idpens,  startday=:startday, last=:last, seriapens=:seriapens, datepens=:datepens, outpens=:outpens, uname=:uname,  dedit=:dedit  where idpensdoc=:idpensdoc").SetParameter<int>("idpens", pensDoc.Pens.PensTypeId).SetParameter<string>("startday", KvrplHelper.DateToBaseFormat(pensDoc.DBeg)).SetParameter<DateTime>("last", pensDoc.DEnd).SetParameter<string>("seriapens", pensDoc.SeriaPens).SetParameter<DateTime?>("datepens", pensDoc.DatePens).SetParameter<string>("outpens", pensDoc.OutPens).SetParameter<string>("uname", Options.Login).SetParameter<string>("dedit", KvrplHelper.DateToBaseFormat(DateTime.Now)).SetParameter<int>("idpensdoc", pensDoc.IdPensDoc).ExecuteUpdate();
                  this.session.Flush();
                }
                catch
                {
                  int num2 = (int) MessageBox.Show("Не могу сохранить текущую запись!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  return;
                }
              }
              this.btnSave.Enabled = false;
              this.txbUname.Text = pensDoc.Uname;
              if (!(pensDoc.Dedit.Date.ToShortDateString() != "01.01.0001") || this.PensDoc == null)
                return;
              TextBox txbDedit = this.txbDedit;
              DateTime dateTime = pensDoc.Dedit;
              dateTime = dateTime.Date;
              string shortDateString = dateTime.ToShortDateString();
              txbDedit.Text = shortDateString;
            }
          }
        }
      }
    }

    private void UCPensDoc_Paint(object sender, PaintEventArgs e)
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

    private void tbSeriaPens_TextChanged(object sender, EventArgs e)
    {
      this.btnSave.Enabled = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.label1 = new Label();
      this.cmbPensType = new ComboBox();
      this.label2 = new Label();
      this.mtbDBeg = new MaskedTextBox();
      this.label3 = new Label();
      this.label4 = new Label();
      this.mtbDEnd = new MaskedTextBox();
      this.label5 = new Label();
      this.mtbDateIssue = new MaskedTextBox();
      this.label6 = new Label();
      this.tbSource = new TextBox();
      this.tbSeriaPens = new TextBox();
      this.label7 = new Label();
      this.btnSave = new Button();
      this.panelInf = new Panel();
      this.txbDedit = new TextBox();
      this.lblDedit = new Label();
      this.txbUname = new TextBox();
      this.lblUname = new Label();
      this.panel1.SuspendLayout();
      this.panelInf.SuspendLayout();
      this.SuspendLayout();
      this.panel1.BackColor = SystemColors.GradientInactiveCaption;
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(435, 25);
      this.panel1.TabIndex = 0;
      this.panel1.Click += new EventHandler(this.UCMspDocument_Click);
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(4, 4);
      this.label1.Name = "label1";
      this.label1.Size = new Size(122, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Пенсионный документ";
      this.label1.Click += new EventHandler(this.UCMspDocument_Click);
      this.cmbPensType.FormattingEnabled = true;
      this.cmbPensType.Location = new Point(75, 4);
      this.cmbPensType.Name = "cmbPensType";
      this.cmbPensType.Size = new Size(191, 21);
      this.cmbPensType.TabIndex = 1;
      this.cmbPensType.TextChanged += new EventHandler(this.tbSeriaPens_TextChanged);
      this.cmbPensType.Click += new EventHandler(this.UCMspDocument_Click);
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(4, 12);
      this.label2.Name = "label2";
      this.label2.Size = new Size(65, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Тип пенсии";
      this.label2.Click += new EventHandler(this.UCMspDocument_Click);
      this.mtbDBeg.Location = new Point(105, 63);
      this.mtbDBeg.Mask = "00/00/0000";
      this.mtbDBeg.Name = "mtbDBeg";
      this.mtbDBeg.Size = new Size(62, 20);
      this.mtbDBeg.TabIndex = 3;
      this.mtbDBeg.ValidatingType = typeof (DateTime);
      this.mtbDBeg.TextChanged += new EventHandler(this.tbSeriaPens_TextChanged);
      this.mtbDBeg.Click += new EventHandler(this.UCMspDocument_Click);
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Location = new Point(4, 70);
      this.label3.Name = "label3";
      this.label3.Size = new Size(95, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Дата назначения";
      this.label3.Click += new EventHandler(this.UCMspDocument_Click);
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(177, 70);
      this.label4.Name = "label4";
      this.label4.Size = new Size(89, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Дата окончания";
      this.label4.Click += new EventHandler(this.UCMspDocument_Click);
      this.mtbDEnd.Location = new Point(272, 63);
      this.mtbDEnd.Mask = "00/00/0000";
      this.mtbDEnd.Name = "mtbDEnd";
      this.mtbDEnd.Size = new Size(62, 20);
      this.mtbDEnd.TabIndex = 5;
      this.mtbDEnd.ValidatingType = typeof (DateTime);
      this.mtbDEnd.TextChanged += new EventHandler(this.tbSeriaPens_TextChanged);
      this.mtbDEnd.Click += new EventHandler(this.UCMspDocument_Click);
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Location = new Point(288, 39);
      this.label5.Name = "label5";
      this.label5.Size = new Size(73, 13);
      this.label5.TabIndex = 8;
      this.label5.Text = "Дата выдачи";
      this.label5.Click += new EventHandler(this.UCMspDocument_Click);
      this.mtbDateIssue.Location = new Point(367, 32);
      this.mtbDateIssue.Mask = "00/00/0000";
      this.mtbDateIssue.Name = "mtbDateIssue";
      this.mtbDateIssue.Size = new Size(62, 20);
      this.mtbDateIssue.TabIndex = 7;
      this.mtbDateIssue.ValidatingType = typeof (DateTime);
      this.mtbDateIssue.TextChanged += new EventHandler(this.tbSeriaPens_TextChanged);
      this.mtbDateIssue.Click += new EventHandler(this.UCMspDocument_Click);
      this.label6.AutoSize = true;
      this.label6.BackColor = Color.Transparent;
      this.label6.Location = new Point(4, 39);
      this.label6.Name = "label6";
      this.label6.Size = new Size(46, 13);
      this.label6.TabIndex = 9;
      this.label6.Text = "Выдано";
      this.label6.Click += new EventHandler(this.UCMspDocument_Click);
      this.tbSource.Location = new Point(75, 35);
      this.tbSource.Name = "tbSource";
      this.tbSource.Size = new Size(191, 20);
      this.tbSource.TabIndex = 10;
      this.tbSource.TextChanged += new EventHandler(this.tbSeriaPens_TextChanged);
      this.tbSource.Click += new EventHandler(this.UCMspDocument_Click);
      this.tbSeriaPens.Location = new Point(367, 3);
      this.tbSeriaPens.Name = "tbSeriaPens";
      this.tbSeriaPens.Size = new Size(62, 20);
      this.tbSeriaPens.TabIndex = 12;
      this.tbSeriaPens.TextChanged += new EventHandler(this.tbSeriaPens_TextChanged);
      this.tbSeriaPens.Click += new EventHandler(this.UCMspDocument_Click);
      this.label7.AutoSize = true;
      this.label7.BackColor = Color.Transparent;
      this.label7.Location = new Point(288, 12);
      this.label7.Name = "label7";
      this.label7.Size = new Size(41, 13);
      this.label7.TabIndex = 11;
      this.label7.Text = "Номер";
      this.label7.Click += new EventHandler(this.UCMspDocument_Click);
      this.btnSave.Image = (Image) Resources.Tick;
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.Location = new Point(340, 89);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(89, 23);
      this.btnSave.TabIndex = 13;
      this.btnSave.Text = "Сохранить";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.panelInf.BackColor = Color.PapayaWhip;
      this.panelInf.Controls.Add((Control) this.txbDedit);
      this.panelInf.Controls.Add((Control) this.lblDedit);
      this.panelInf.Controls.Add((Control) this.txbUname);
      this.panelInf.Controls.Add((Control) this.lblUname);
      this.panelInf.Controls.Add((Control) this.label2);
      this.panelInf.Controls.Add((Control) this.btnSave);
      this.panelInf.Controls.Add((Control) this.cmbPensType);
      this.panelInf.Controls.Add((Control) this.tbSeriaPens);
      this.panelInf.Controls.Add((Control) this.mtbDBeg);
      this.panelInf.Controls.Add((Control) this.label7);
      this.panelInf.Controls.Add((Control) this.label3);
      this.panelInf.Controls.Add((Control) this.tbSource);
      this.panelInf.Controls.Add((Control) this.mtbDEnd);
      this.panelInf.Controls.Add((Control) this.label6);
      this.panelInf.Controls.Add((Control) this.label4);
      this.panelInf.Controls.Add((Control) this.label5);
      this.panelInf.Controls.Add((Control) this.mtbDateIssue);
      this.panelInf.Dock = DockStyle.Fill;
      this.panelInf.Location = new Point(0, 25);
      this.panelInf.Name = "panelInf";
      this.panelInf.Size = new Size(435, 117);
      this.panelInf.TabIndex = 14;
      this.panelInf.Paint += new PaintEventHandler(this.panelInf_Paint);
      this.panelInf.Click += new EventHandler(this.UCMspDocument_Click);
      this.txbDedit.Enabled = false;
      this.txbDedit.Location = new Point(256, 90);
      this.txbDedit.Name = "txbDedit";
      this.txbDedit.Size = new Size(73, 20);
      this.txbDedit.TabIndex = 20;
      this.lblDedit.AutoSize = true;
      this.lblDedit.BackColor = Color.Transparent;
      this.lblDedit.Location = new Point(158, 94);
      this.lblDedit.Name = "lblDedit";
      this.lblDedit.Size = new Size(92, 13);
      this.lblDedit.TabIndex = 19;
      this.lblDedit.Text = "Дата редактир-я";
      this.txbUname.Enabled = false;
      this.txbUname.Location = new Point(86, 90);
      this.txbUname.Name = "txbUname";
      this.txbUname.Size = new Size(65, 20);
      this.txbUname.TabIndex = 18;
      this.lblUname.AutoSize = true;
      this.lblUname.BackColor = Color.Transparent;
      this.lblUname.Location = new Point(4, 94);
      this.lblUname.Name = "lblUname";
      this.lblUname.Size = new Size(80, 13);
      this.lblUname.TabIndex = 17;
      this.lblUname.Text = "Пользователь";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Info;
      this.Controls.Add((Control) this.panelInf);
      this.Controls.Add((Control) this.panel1);
      this.Name = "UCPensDoc";
      this.Size = new Size(435, 142);
      this.Paint += new PaintEventHandler(this.UCPensDoc_Paint);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panelInf.ResumeLayout(false);
      this.panelInf.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
