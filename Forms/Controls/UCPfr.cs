// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.Controls.UCPfr
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
  public class UCPfr : UserControl
  {
    private bool isNew = false;
    private IContainer components = (IContainer) null;
    private ISession session;
    private Panel panel1;
    private Label label1;
    private Label label2;
    private Panel panelInf;
    private Button btnSave;
    private MaskedTextBox mtbNPfr;
    private Label label3;

    public bool Selected { get; set; }

    public Person Person { get; set; }

    public event EventHandler SelectedChanged;

    public UCPfr()
    {
      this.InitializeComponent();
    }

    public UCPfr(Person person, bool isnew)
    {
      this.InitializeComponent();
      this.Person = person;
      this.session = Domain.CurrentSession;
      this.isNew = isnew;
      this.LoadData();
    }

    public void LoadData()
    {
      if (this.Person != null)
        this.mtbNPfr.Text = this.Person.Snils;
      if (!this.isNew)
        this.btnSave.Enabled = false;
      else
        this.btnSave.Enabled = true;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      this.session.Clear();
      this.session = Domain.CurrentSession;
      Person person1 = new Person();
      Person person2 = this.Person;
      if (this.mtbNPfr.Text == "   -   -   -")
        person2.Snils = "";
      else if (this.mtbNPfr.Text.Length == 14)
      {
        person2.Snils = this.mtbNPfr.Text;
      }
      else
      {
        int num = (int) MessageBox.Show("Номер страхового свидетельства введен некорректно!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }
      if (this.isNew)
      {
        try
        {
          this.session.Save((object) person2);
          this.session.Flush();
          this.isNew = false;
        }
        catch
        {
          int num = (int) MessageBox.Show("Не могу сохранить текущую запись!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
      }
      else
      {
        try
        {
          this.session.CreateSQLQuery("update DBA.Form_a set  idstrah=:idstrah  where idform=:idform").SetParameter<string>("idstrah", person2.Snils).SetParameter<int>("idform", person2.PersonId).ExecuteUpdate();
          this.session.Flush();
        }
        catch
        {
          int num = (int) MessageBox.Show("Не могу сохранить текущую запись!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
      }
      this.btnSave.Enabled = false;
    }

    private void UCPfr_Paint(object sender, PaintEventArgs e)
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

    private void UCPfr_Click(object sender, EventArgs e)
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

    private void mtbNPfr_TextChanged(object sender, EventArgs e)
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
      this.label2 = new Label();
      this.panelInf = new Panel();
      this.label3 = new Label();
      this.mtbNPfr = new MaskedTextBox();
      this.btnSave = new Button();
      this.panel1.SuspendLayout();
      this.panelInf.SuspendLayout();
      this.SuspendLayout();
      this.panel1.BackColor = SystemColors.GradientInactiveCaption;
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Margin = new Padding(4);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(329, 30);
      this.panel1.TabIndex = 0;
      this.panel1.Click += new EventHandler(this.UCPfr_Click);
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(5, 5);
      this.label1.Name = "label1";
      this.label1.Size = new Size(178, 17);
      this.label1.TabIndex = 0;
      this.label1.Text = "Страховое свидетельство";
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(17, 22);
      this.label2.Name = "label2";
      this.label2.Size = new Size(51, 17);
      this.label2.TabIndex = 1;
      this.label2.Text = "Номер";
      this.panelInf.BackColor = Color.PapayaWhip;
      this.panelInf.Controls.Add((Control) this.label3);
      this.panelInf.Controls.Add((Control) this.mtbNPfr);
      this.panelInf.Controls.Add((Control) this.btnSave);
      this.panelInf.Controls.Add((Control) this.label2);
      this.panelInf.Dock = DockStyle.Fill;
      this.panelInf.Location = new Point(0, 30);
      this.panelInf.Name = "panelInf";
      this.panelInf.Size = new Size(329, 57);
      this.panelInf.TabIndex = 2;
      this.panelInf.Paint += new PaintEventHandler(this.panelInf_Paint);
      this.panelInf.Click += new EventHandler(this.UCPfr_Click);
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label3.Location = new Point(93, 42);
      this.label3.Name = "label3";
      this.label3.Size = new Size(77, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "(xxx-xxx-xxx-xx)";
      this.mtbNPfr.Location = new Point(84, 16);
      this.mtbNPfr.Mask = "000-000-000-00";
      this.mtbNPfr.Name = "mtbNPfr";
      this.mtbNPfr.Size = new Size(99, 23);
      this.mtbNPfr.TabIndex = 4;
      this.mtbNPfr.TextChanged += new EventHandler(this.mtbNPfr_TextChanged);
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Image = (Image) Resources.Tick;
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.Location = new Point(209, 11);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(107, 28);
      this.btnSave.TabIndex = 3;
      this.btnSave.Text = "Сохранить";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panelInf);
      this.Controls.Add((Control) this.panel1);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.Margin = new Padding(4);
      this.Name = "UCPfr";
      this.Size = new Size(329, 87);
      this.Paint += new PaintEventHandler(this.UCPfr_Paint);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panelInf.ResumeLayout(false);
      this.panelInf.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
