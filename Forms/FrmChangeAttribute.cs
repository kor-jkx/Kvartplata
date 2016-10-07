// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmChangeAttribute
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
  public class FrmChangeAttribute : Form
  {
    private IContainer components = (IContainer) null;
    private ISession session;
    private Company company;
    private int operation;
    private Raion raion;
    private Panel pnBtn;
    private Button btnExit;
    private Button btnStart;
    private GroupBox gbAreal;
    private RadioButton rbRaion;
    private RadioButton rbAll;
    private RadioButton rbCompany;
    private Panel pnChange;
    private Panel pnDelete;
    private Label lblPacket;
    private Label lblPurpose;
    private Label lblSource;
    private Label lblDate;
    private Label lblPeriod;
    private Label lblNew;
    private Label lblOld;
    private TextBox txtNew;
    private TextBox txtOld;
    private ComboBox cmbPurpNew;
    private ComboBox cmbPurpOld;
    private ComboBox cmbSourceNew;
    private ComboBox cmbSourceOld;
    private DateTimePicker dtmNew;
    private DateTimePicker dtmOld;
    private MonthPicker mpNew;
    private MonthPicker mpOld;
    private Label lblNumPacket;
    private TextBox txtPacket;

    public FrmChangeAttribute()
    {
      this.InitializeComponent();
    }

    public FrmChangeAttribute(Raion raion, Company company, int operation)
    {
      this.company = company;
      this.operation = operation;
      this.raion = raion;
      this.InitializeComponent();
    }

    private void ChangeAttribute_Shown(object sender, EventArgs e)
    {
      if (this.operation == 1)
      {
        this.Text = "Изменение атрибутов пачки";
        this.pnDelete.Visible = false;
        this.pnChange.Visible = true;
      }
      if (this.operation == 2)
      {
        this.pnDelete.BringToFront();
        this.Text = "Удаление пачки";
      }
      MonthPicker mpOld = this.mpOld;
      DateTime? periodName = Options.Period.PeriodName;
      DateTime dateTime1 = periodName.Value;
      mpOld.Value = dateTime1;
      MonthPicker mpNew = this.mpNew;
      periodName = Options.Period.PeriodName;
      DateTime dateTime2 = periodName.Value;
      mpNew.Value = dateTime2;
      DateTimePicker dtmOld = this.dtmOld;
      periodName = Options.Period.PeriodName;
      DateTime dateTime3 = periodName.Value;
      dtmOld.Value = dateTime3;
      DateTimePicker dtmNew = this.dtmNew;
      periodName = Options.Period.PeriodName;
      DateTime dateTime4 = periodName.Value;
      dtmNew.Value = dateTime4;
      this.session = Domain.CurrentSession;
      IList<SourcePay> sourcePayList1 = this.session.CreateCriteria(typeof (SourcePay)).AddOrder(Order.Asc("SourcePayName")).List<SourcePay>();
      IList<SourcePay> sourcePayList2 = this.session.CreateCriteria(typeof (SourcePay)).AddOrder(Order.Asc("SourcePayName")).List<SourcePay>();
      this.cmbSourceOld.DataSource = (object) sourcePayList1;
      this.cmbSourceOld.ValueMember = "SourcePayId";
      this.cmbSourceOld.DisplayMember = "SourcePayName";
      this.cmbSourceNew.DataSource = (object) sourcePayList2;
      this.cmbSourceNew.ValueMember = "SourcePayId";
      this.cmbSourceNew.DisplayMember = "SourcePayName";
      IList<PurposePay> purposePayList1 = this.session.CreateCriteria(typeof (PurposePay)).AddOrder(Order.Asc("PurposePayName")).List<PurposePay>();
      IList<PurposePay> purposePayList2 = this.session.CreateCriteria(typeof (PurposePay)).AddOrder(Order.Asc("PurposePayName")).List<PurposePay>();
      this.cmbPurpOld.DataSource = (object) purposePayList1;
      this.cmbPurpOld.ValueMember = "PurposePayId";
      this.cmbPurpOld.DisplayMember = "PurposePayName";
      this.cmbPurpNew.DataSource = (object) purposePayList2;
      this.cmbPurpNew.ValueMember = "PurposePayId";
      this.cmbPurpNew.DisplayMember = "PurposePayName";
      this.session.Clear();
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
      string str1 = "";
      string str2 = "";
      int num1 = 0;
      int num2 = 0;
      if (this.rbCompany.Checked)
      {
        if (this.company == null)
        {
          int num3 = (int) MessageBox.Show("Для изменения атрибутов внутри домоуправления выберите конкретное домоуправление", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return;
        }
        str1 = " and LsClient.ClientId in (select ClientId from LsClient ls where ls.Company.CompanyId=" + this.company.CompanyId.ToString() + Options.MainConditions1 + ")";
        str2 = " and Company_id=" + this.company.CompanyId.ToString();
      }
      if (this.rbRaion.Checked)
      {
        if (this.rbRaion.Checked && this.raion == null)
        {
          int num3 = (int) MessageBox.Show("Для изменения атрибутов внутри района выберите конкретный район", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return;
        }
        string[] strArray = new string[5];
        strArray[0] = " and LsClient.ClientId in (select ClientId from LsClient ls where ls.Company.CompanyId in (select CompanyId from Company where Raion=";
        int index1 = 1;
        int idRnn = this.raion.IdRnn;
        string str3 = idRnn.ToString();
        strArray[index1] = str3;
        int index2 = 2;
        string str4 = ")";
        strArray[index2] = str4;
        int index3 = 3;
        string mainConditions1 = Options.MainConditions1;
        strArray[index3] = mainConditions1;
        int index4 = 4;
        string str5 = ")";
        strArray[index4] = str5;
        str1 = string.Concat(strArray);
        string str6 = " and Company_id in (select CompanyId from Company where Raion=";
        idRnn = this.raion.IdRnn;
        string str7 = idRnn.ToString();
        string str8 = ")";
        str2 = str6 + str7 + str8;
      }
      Period period1 = new Period();
      IList list = this.session.CreateQuery("select min(Period_id) from ClosedPeriod where Complex_id=100" + str2).List();
      if (list.Count > 0)
        period1 = this.session.Get<Period>((object) Convert.ToInt32(list[0]));
      switch (this.operation)
      {
        case 1:
          Period period2 = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) this.mpOld.Value)).List<Period>()[0];
          Period period3 = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) this.mpNew.Value)).List<Period>()[0];
          DateTime? periodName = period2.PeriodName;
          DateTime dateTime1 = periodName.Value;
          periodName = period1.PeriodName;
          DateTime dateTime2 = periodName.Value;
          if (dateTime1 <= dateTime2 || period3.PeriodName.Value <= period1.PeriodName.Value)
          {
            int num3 = (int) MessageBox.Show("Невозможно внести изменения в закрытый период", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
          if (this.txtOld.Text == "" || this.txtNew.Text == "")
          {
            int num3 = (int) MessageBox.Show("Введите номер пачки", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
          try
          {
            num1 = Convert.ToInt32(this.txtOld.Text);
            num2 = Convert.ToInt32(this.txtNew.Text);
          }
          catch (Exception ex)
          {
            int num3 = (int) MessageBox.Show("Некорректный номер пачки", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          if (num1 < 0)
          {
            int num3 = (int) MessageBox.Show("Номер пачки не может быть отрицательным", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
          if (num2 < 0)
          {
            int num3 = (int) MessageBox.Show("Номер пачки не может быть отрицательным", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
          try
          {
            this.session = Domain.CurrentSession;
            this.session.CreateQuery(string.Format("update Payment set Period.PeriodId={10},SPay.SourcePayId={1},PPay.PurposePayId={2},PaymentDate='{3}',PacketNum={4} where Period.PeriodId={5} and SPay.SourcePayId={6} and PPay.PurposePayId={7} and PaymentDate='{8}' and PacketNum={9}" + str1, (object) 0, this.cmbSourceNew.SelectedValue, this.cmbPurpNew.SelectedValue, (object) KvrplHelper.DateToBaseFormat(this.dtmNew.Value), (object) num2, (object) period2.PeriodId, this.cmbSourceOld.SelectedValue, this.cmbPurpOld.SelectedValue, (object) KvrplHelper.DateToBaseFormat(this.dtmOld.Value), (object) num1, (object) period3.PeriodId)).ExecuteUpdate();
            this.session.Clear();
            int num3 = (int) MessageBox.Show("Операция завершена успешно", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            break;
          }
          catch (Exception ex)
          {
            int num3 = (int) MessageBox.Show("Невозможно обновить атрибуты пачки", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
        case 2:
          if (Options.Period.PeriodName.Value <= period1.PeriodName.Value)
          {
            int num3 = (int) MessageBox.Show("Невозможно внести изменения в закрытый период", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
          if (this.txtPacket.Text == "")
          {
            int num3 = (int) MessageBox.Show("Введите номер пачки", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
          try
          {
            this.session = Domain.CurrentSession;
            this.session.CreateQuery(string.Format("delete from Payment where Period.PeriodId={1} " + str1 + " and PacketNum={2}", (object) 0, (object) Options.Period.PeriodId, (object) Convert.ToInt32(this.txtPacket.Text))).ExecuteUpdate();
            this.session.Clear();
            int num3 = (int) MessageBox.Show("Операция завершена успешно", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            break;
          }
          catch (Exception ex)
          {
            int num3 = (int) MessageBox.Show("Невозможно удалить пачку", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmChangeAttribute));
      this.pnBtn = new Panel();
      this.btnStart = new Button();
      this.btnExit = new Button();
      this.gbAreal = new GroupBox();
      this.rbRaion = new RadioButton();
      this.rbAll = new RadioButton();
      this.rbCompany = new RadioButton();
      this.pnChange = new Panel();
      this.pnDelete = new Panel();
      this.lblNumPacket = new Label();
      this.txtPacket = new TextBox();
      this.lblPacket = new Label();
      this.lblPurpose = new Label();
      this.lblSource = new Label();
      this.lblDate = new Label();
      this.lblPeriod = new Label();
      this.lblNew = new Label();
      this.lblOld = new Label();
      this.txtNew = new TextBox();
      this.txtOld = new TextBox();
      this.cmbPurpNew = new ComboBox();
      this.cmbPurpOld = new ComboBox();
      this.cmbSourceNew = new ComboBox();
      this.cmbSourceOld = new ComboBox();
      this.dtmNew = new DateTimePicker();
      this.dtmOld = new DateTimePicker();
      this.mpNew = new MonthPicker();
      this.mpOld = new MonthPicker();
      this.pnBtn.SuspendLayout();
      this.gbAreal.SuspendLayout();
      this.pnChange.SuspendLayout();
      this.pnDelete.SuspendLayout();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnStart);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 226);
      this.pnBtn.Margin = new Padding(4);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(473, 40);
      this.pnBtn.TabIndex = 15;
      this.btnStart.Image = (Image) Resources.Configure;
      this.btnStart.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnStart.Location = new Point(12, 5);
      this.btnStart.Name = "btnStart";
      this.btnStart.Size = new Size(110, 30);
      this.btnStart.TabIndex = 0;
      this.btnStart.Text = "Выполнить";
      this.btnStart.TextAlign = ContentAlignment.MiddleRight;
      this.btnStart.UseVisualStyleBackColor = true;
      this.btnStart.Click += new EventHandler(this.btnStart_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(380, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(81, 30);
      this.btnExit.TabIndex = 1;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.gbAreal.Controls.Add((Control) this.rbRaion);
      this.gbAreal.Controls.Add((Control) this.rbAll);
      this.gbAreal.Controls.Add((Control) this.rbCompany);
      this.gbAreal.Dock = DockStyle.Top;
      this.gbAreal.Location = new Point(0, 0);
      this.gbAreal.Name = "gbAreal";
      this.gbAreal.Size = new Size(473, 54);
      this.gbAreal.TabIndex = 0;
      this.gbAreal.TabStop = false;
      this.gbAreal.Text = "Выполнить";
      this.rbRaion.AutoSize = true;
      this.rbRaion.Location = new Point(205, 21);
      this.rbRaion.Name = "rbRaion";
      this.rbRaion.Size = new Size(95, 20);
      this.rbRaion.TabIndex = 1;
      this.rbRaion.Text = "По району";
      this.rbRaion.UseVisualStyleBackColor = true;
      this.rbAll.AutoSize = true;
      this.rbAll.Location = new Point(350, 21);
      this.rbAll.Name = "rbAll";
      this.rbAll.Size = new Size(113, 20);
      this.rbAll.TabIndex = 2;
      this.rbAll.Text = "По всей базе";
      this.rbAll.UseVisualStyleBackColor = true;
      this.rbCompany.AutoSize = true;
      this.rbCompany.Checked = true;
      this.rbCompany.Location = new Point(6, 21);
      this.rbCompany.Name = "rbCompany";
      this.rbCompany.Size = new Size(162, 20);
      this.rbCompany.TabIndex = 0;
      this.rbCompany.TabStop = true;
      this.rbCompany.Text = "По домоуправлению";
      this.rbCompany.UseVisualStyleBackColor = true;
      this.pnChange.Controls.Add((Control) this.pnDelete);
      this.pnChange.Controls.Add((Control) this.lblPacket);
      this.pnChange.Controls.Add((Control) this.lblPurpose);
      this.pnChange.Controls.Add((Control) this.lblSource);
      this.pnChange.Controls.Add((Control) this.lblDate);
      this.pnChange.Controls.Add((Control) this.lblPeriod);
      this.pnChange.Controls.Add((Control) this.lblNew);
      this.pnChange.Controls.Add((Control) this.lblOld);
      this.pnChange.Controls.Add((Control) this.txtNew);
      this.pnChange.Controls.Add((Control) this.txtOld);
      this.pnChange.Controls.Add((Control) this.cmbPurpNew);
      this.pnChange.Controls.Add((Control) this.cmbPurpOld);
      this.pnChange.Controls.Add((Control) this.cmbSourceNew);
      this.pnChange.Controls.Add((Control) this.cmbSourceOld);
      this.pnChange.Controls.Add((Control) this.dtmNew);
      this.pnChange.Controls.Add((Control) this.dtmOld);
      this.pnChange.Controls.Add((Control) this.mpNew);
      this.pnChange.Controls.Add((Control) this.mpOld);
      this.pnChange.Dock = DockStyle.Fill;
      this.pnChange.Location = new Point(0, 54);
      this.pnChange.Name = "pnChange";
      this.pnChange.Size = new Size(473, 172);
      this.pnChange.TabIndex = 16;
      this.pnDelete.Controls.Add((Control) this.lblNumPacket);
      this.pnDelete.Controls.Add((Control) this.txtPacket);
      this.pnDelete.Dock = DockStyle.Fill;
      this.pnDelete.Location = new Point(0, 0);
      this.pnDelete.Name = "pnDelete";
      this.pnDelete.Size = new Size(473, 172);
      this.pnDelete.TabIndex = 35;
      this.lblNumPacket.AutoSize = true;
      this.lblNumPacket.Location = new Point(3, 12);
      this.lblNumPacket.Name = "lblNumPacket";
      this.lblNumPacket.Size = new Size(93, 16);
      this.lblNumPacket.TabIndex = 3;
      this.lblNumPacket.Text = "Номер пачки";
      this.txtPacket.Location = new Point(102, 9);
      this.txtPacket.Name = "txtPacket";
      this.txtPacket.Size = new Size(100, 22);
      this.txtPacket.TabIndex = 0;
      this.lblPacket.AutoSize = true;
      this.lblPacket.Location = new Point(13, 144);
      this.lblPacket.Name = "lblPacket";
      this.lblPacket.Size = new Size(49, 16);
      this.lblPacket.TabIndex = 34;
      this.lblPacket.Text = "Пачка";
      this.lblPurpose.AutoSize = true;
      this.lblPurpose.Location = new Point(12, 114);
      this.lblPurpose.Name = "lblPurpose";
      this.lblPurpose.Size = new Size(90, 16);
      this.lblPurpose.TabIndex = 33;
      this.lblPurpose.Text = "Назначение";
      this.lblSource.AutoSize = true;
      this.lblSource.Location = new Point(13, 84);
      this.lblSource.Name = "lblSource";
      this.lblSource.Size = new Size(71, 16);
      this.lblSource.TabIndex = 32;
      this.lblSource.Text = "Источник";
      this.lblDate.AutoSize = true;
      this.lblDate.Location = new Point(12, 56);
      this.lblDate.Name = "lblDate";
      this.lblDate.Size = new Size(91, 16);
      this.lblDate.TabIndex = 31;
      this.lblDate.Text = "Дата оплаты";
      this.lblPeriod.AutoSize = true;
      this.lblPeriod.Location = new Point(13, 28);
      this.lblPeriod.Name = "lblPeriod";
      this.lblPeriod.Size = new Size(58, 16);
      this.lblPeriod.TabIndex = 30;
      this.lblPeriod.Text = "Период";
      this.lblNew.AutoSize = true;
      this.lblNew.Location = new Point(303, 6);
      this.lblNew.Name = "lblNew";
      this.lblNew.Size = new Size(117, 16);
      this.lblNew.TabIndex = 29;
      this.lblNew.Text = "Новые атрибуты";
      this.lblOld.AutoSize = true;
      this.lblOld.Location = new Point(132, 6);
      this.lblOld.Name = "lblOld";
      this.lblOld.Size = new Size(123, 16);
      this.lblOld.TabIndex = 28;
      this.lblOld.Text = "Старые атрибуты";
      this.txtNew.Location = new Point(306, 141);
      this.txtNew.Name = "txtNew";
      this.txtNew.Size = new Size(156, 22);
      this.txtNew.TabIndex = 27;
      this.txtOld.Location = new Point(115, 141);
      this.txtOld.Name = "txtOld";
      this.txtOld.Size = new Size(152, 22);
      this.txtOld.TabIndex = 22;
      this.cmbPurpNew.DropDownWidth = 200;
      this.cmbPurpNew.FormattingEnabled = true;
      this.cmbPurpNew.Location = new Point(306, 111);
      this.cmbPurpNew.Name = "cmbPurpNew";
      this.cmbPurpNew.Size = new Size(156, 24);
      this.cmbPurpNew.TabIndex = 26;
      this.cmbPurpOld.DropDownWidth = 200;
      this.cmbPurpOld.FormattingEnabled = true;
      this.cmbPurpOld.Location = new Point(115, 111);
      this.cmbPurpOld.Name = "cmbPurpOld";
      this.cmbPurpOld.Size = new Size(152, 24);
      this.cmbPurpOld.TabIndex = 21;
      this.cmbSourceNew.DropDownWidth = 200;
      this.cmbSourceNew.FormattingEnabled = true;
      this.cmbSourceNew.Location = new Point(306, 81);
      this.cmbSourceNew.Name = "cmbSourceNew";
      this.cmbSourceNew.Size = new Size(156, 24);
      this.cmbSourceNew.TabIndex = 25;
      this.cmbSourceOld.DropDownWidth = 200;
      this.cmbSourceOld.FormattingEnabled = true;
      this.cmbSourceOld.Location = new Point(115, 81);
      this.cmbSourceOld.Name = "cmbSourceOld";
      this.cmbSourceOld.Size = new Size(153, 24);
      this.cmbSourceOld.TabIndex = 20;
      this.dtmNew.Location = new Point(306, 53);
      this.dtmNew.Name = "dtmNew";
      this.dtmNew.Size = new Size(156, 22);
      this.dtmNew.TabIndex = 24;
      this.dtmOld.Location = new Point(115, 53);
      this.dtmOld.Name = "dtmOld";
      this.dtmOld.Size = new Size(152, 22);
      this.dtmOld.TabIndex = 19;
      this.mpNew.CustomFormat = "MMMM yyyy";
      this.mpNew.Format = DateTimePickerFormat.Custom;
      this.mpNew.Location = new Point(306, 25);
      this.mpNew.Name = "mpNew";
      this.mpNew.OldMonth = 0;
      this.mpNew.ShowUpDown = true;
      this.mpNew.Size = new Size(156, 22);
      this.mpNew.TabIndex = 23;
      this.mpOld.CustomFormat = "MMMM yyyy";
      this.mpOld.Format = DateTimePickerFormat.Custom;
      this.mpOld.Location = new Point(115, 25);
      this.mpOld.Name = "mpOld";
      this.mpOld.OldMonth = 0;
      this.mpOld.ShowUpDown = true;
      this.mpOld.Size = new Size(152, 22);
      this.mpOld.TabIndex = 18;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(473, 266);
      this.Controls.Add((Control) this.pnChange);
      this.Controls.Add((Control) this.gbAreal);
      this.Controls.Add((Control) this.pnBtn);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmChangeAttribute";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Изменение атрибутов пачки";
      this.Shown += new EventHandler(this.ChangeAttribute_Shown);
      this.pnBtn.ResumeLayout(false);
      this.gbAreal.ResumeLayout(false);
      this.gbAreal.PerformLayout();
      this.pnChange.ResumeLayout(false);
      this.pnChange.PerformLayout();
      this.pnDelete.ResumeLayout(false);
      this.pnDelete.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
