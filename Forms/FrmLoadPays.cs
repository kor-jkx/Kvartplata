// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmLoadPays
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
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmLoadPays : FrmBaseForm
  {
    private IContainer components = (IContainer) null;
    private readonly string org;
    private readonly short type;
    private Period Per;
    private ISession session;
    private MonthPicker mpPeriodPay;
    private MonthPicker mpPeriod;
    private Label lblPacket;
    private Label lblPeriodPay;
    private ComboBox cmbPurpose;
    private Label lblPurpose;
    private ComboBox cmbSource;
    private Label lblSource;
    private Label lblPeriod;
    private Panel pnBtn;
    private Button btnLoadPays;
    private Button btnExit;
    private OpenFileDialog fdPayLoad;
    private TextBox txbPacket;
    private Label lblPaymentDate;
    private DateTimePicker dtpPaymentDate;

    public virtual Period Period
    {
      get
      {
        return this.Per;
      }
    }

    public virtual DateTime PaymentDate
    {
      get
      {
        return this.dtpPaymentDate.Value;
      }
    }

    public virtual SourcePay SPay
    {
      get
      {
        return (SourcePay) this.cmbSource.SelectedItem;
      }
    }

    public virtual PurposePay PPay
    {
      get
      {
        return (PurposePay) this.cmbPurpose.SelectedItem;
      }
    }

    public virtual string Packet
    {
      get
      {
        return this.txbPacket.Text;
      }
    }

    public FrmLoadPays()
    {
      this.InitializeComponent();
    }

    public FrmLoadPays(short type)
    {
      this.InitializeComponent();
      this.type = type;
    }

    public FrmLoadPays(short type, string org)
    {
      this.InitializeComponent();
      this.type = type;
      this.org = org;
    }

    private void FrmLoadPays_Load(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      IList list1 = this.session.CreateCriteria(typeof (SourcePay)).AddOrder(Order.Asc("SourcePayName")).List();
      if ((uint) list1.Count > 0U)
      {
        this.cmbSource.DataSource = (object) list1;
        this.cmbSource.ValueMember = "SourcePayId";
        this.cmbSource.DisplayMember = "SourcePayName";
        if ((int) this.type == 2)
          this.cmbSource.SelectedIndex = this.cmbSource.FindString(this.org);
        IList list2 = this.session.CreateCriteria(typeof (PurposePay)).AddOrder(Order.Asc("PurposePayId")).List();
        if ((uint) list2.Count > 0U)
        {
          this.cmbPurpose.DataSource = (object) list2;
          this.cmbPurpose.ValueMember = "PurposePayId";
          this.cmbPurpose.DisplayMember = "PurposePayName";
          this.mpPeriod.Value = Options.Period.PeriodName.Value;
          this.mpPeriodPay.Value = Options.Period.PeriodName.Value.AddMonths(-1);
          if ((int) this.type == 1)
          {
            this.lblPaymentDate.Visible = false;
            this.dtpPaymentDate.Visible = false;
          }
          if ((int) this.type != 2)
            return;
          this.lblPeriod.Text = "В какой месяц закачать платежи";
          this.lblPeriodPay.Visible = false;
          this.mpPeriodPay.Visible = false;
          this.UpdatePacket();
        }
        else
        {
          int num = (int) MessageBox.Show("Внесение записей невозможно. Заполните словарь назначений", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.Close();
        }
      }
      else
      {
        int num = (int) MessageBox.Show("Внесение записей невозможно. Заполните словарь источников", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.Close();
      }
    }

    private void btnLoadPays_Click(object sender, EventArgs e)
    {
      if (this.txbPacket.Text == "")
      {
        int num1 = (int) MessageBox.Show("Пачка не введена", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        int int32 = Convert.ToInt32(this.session.CreateQuery(string.Format("select max(Period.PeriodId) from CompanyPeriod where Complex.IdFk={0}", (object) Options.Complex.IdFk)).List()[0]);
        IList list1 = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) this.mpPeriod.Value)).List();
        IList list2 = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) this.mpPeriodPay.Value)).List();
        if (list1.Count == 0 || list2.Count == 0)
        {
          int num2 = (int) MessageBox.Show("Введен некорректный период", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (int32 > ((Period) list1[0]).PeriodId)
        {
          int num3 = (int) MessageBox.Show("Невозможно внести записи в закрытый период", "Внимание", MessageBoxButtons.OK);
        }
        else if ((int) this.type == 2)
        {
          this.Per = (Period) list1[0];
          this.Close();
        }
        else
        {
          if (MessageBox.Show("Закачать платежи из файла? ", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK || this.fdPayLoad.ShowDialog() != DialogResult.OK)
            return;
          this.Cursor = Cursors.WaitCursor;
          ITransaction transaction = this.session.BeginTransaction();
          string fileName = this.fdPayLoad.FileName;
          string str1 = fileName.Remove(0, fileName.LastIndexOf("\\") + 1);
          string str2 = fileName.Remove(fileName.LastIndexOf("\\"), fileName.Length - fileName.LastIndexOf("\\"));
          string connectionString = string.Format("Provider={4};Eng={0};Uid={1};Pwd={2}; Links={3}", (object) Options.BaseName, (object) Options.Login, (object) Options.Pwd, (object) "tcpip{}", (object) Options.Provider);
          try
          {
            DataSet dataSet = OleDbHelper.ExecuteDataset(new OleDbConnection(string.Format("Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; Extended Properties=dBASE III", (object) str2)), CommandType.Text, "select * from " + str1, 10000);
            try
            {
              OleDbHelper.ExecuteNonQuery(connectionString, CommandType.Text, "drop table tmppay", 1000);
            }
            catch (Exception ex)
            {
            }
            OleDbHelper.ExecuteNonQuery(connectionString, CommandType.Text, "create table tmppay(client integer,summa numeric(15,2),dukop date) ", 1000);
            OleDbHelper.ExecuteNonQuery(connectionString, CommandType.Text, "delete from tmppay ", 1000);
            if ((uint) dataSet.Tables[0].Rows.Count > 0U)
            {
              foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
              {
                OleDbCommand oleDbCommand = new OleDbCommand();
                oleDbCommand.Connection = new OleDbConnection(connectionString);
                oleDbCommand.CommandType = CommandType.Text;
                oleDbCommand.CommandText = "insert into tmppay values(:client,:summa,:dukop)";
                oleDbCommand.Parameters.Add("client", OleDbType.Integer).Value = row["lic"];
                oleDbCommand.Parameters.Add("summa", OleDbType.Currency).Value = row["sum"];
                oleDbCommand.Parameters.Add("dukop", OleDbType.Date).Value = row["dpay"];
                oleDbCommand.Connection.Open();
                oleDbCommand.ExecuteNonQuery();
                oleDbCommand.Connection.Close();
              }
            }
            OleDbDataReader oleDbDataReader = OleDbHelper.ExecuteReader(connectionString, CommandType.Text, "select client from tmppay where client not in (select client_id from lsClient)", 1000);
            IList<string> stringList = (IList<string>) new List<string>();
            string str3 = "";
            while (oleDbDataReader.Read())
              str3 = Convert.ToString(oleDbDataReader[0]) + ", ";
            oleDbDataReader.Close();
            if (str3 != "")
            {
              this.Cursor = Cursors.Default;
              int num4 = (int) MessageBox.Show("Невозможно закачать файл. Лицевые " + str3.Substring(0, str3.Length - 2) + " отсутствуют в списке лицевых", "", MessageBoxButtons.OK);
              return;
            }
            double num5 = Convert.ToDouble(OleDbHelper.ExecuteScalar(connectionString, CommandType.Text, "select sum(summa) from tmppay", 1000));
            OleDbHelper.ExecuteNonQuery(connectionString, CommandType.Text, string.Format("insert into dba.lsPayment (Payment_id, Period_id, Client_id, SourcePay_id, PurposePay_id, Receipt_id, Service_id, Payment_date, Month_id, Packet_num, Payment_value, Payment_peni, Uname, Dedit, supplier_id, PayDoc_id) select gen_id('lsPayment',number()),{0},client,{1},{2},1,0,dukop,{3},{4},summa,0,user,today(),0,1 from tmppay", (object) ((Period) list1[0]).PeriodId, (object) ((SourcePay) this.cmbSource.SelectedItem).SourcePayId, (object) ((PurposePay) this.cmbPurpose.SelectedItem).PurposePayId, (object) ((Period) list2[0]).PeriodId, (object) this.txbPacket.Text), 10000);
            OleDbHelper.ExecuteNonQuery(connectionString, CommandType.Text, "drop table tmppay", 1000);
            int num6 = (int) MessageBox.Show("Закачка прошла успешно. Сумма по пачке: " + num5.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            transaction.Commit();
          }
          catch (Exception ex1)
          {
            int num4 = (int) MessageBox.Show("Не удалось закачать данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            try
            {
              transaction.Rollback();
              OleDbHelper.ExecuteNonQuery(connectionString, CommandType.Text, "drop table tmppay", 1000);
            }
            catch (Exception ex2)
            {
            }
          }
          this.Cursor = Cursors.Default;
        }
      }
    }

    private void cmbSource_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if ((int) this.type != 2)
        return;
      this.UpdatePacket();
    }

    private void UpdatePacket()
    {
      if (this.dtpPaymentDate == null)
        return;
      DateTime dateTime = this.dtpPaymentDate.Value;
      DateTime? periodName = Options.Period.PeriodName;
      if (periodName.HasValue && dateTime < periodName.GetValueOrDefault())
        this.txbPacket.Text = Convert.ToString(Convert.ToInt32(this.cmbSource.SelectedValue) * 100 + this.dtpPaymentDate.Value.Day + 50);
      else
        this.txbPacket.Text = Convert.ToString(Convert.ToInt32(this.cmbSource.SelectedValue) * 100 + this.dtpPaymentDate.Value.Day);
    }

    private void txbPacket_KeyPress(object sender, KeyPressEventArgs e)
    {
      if ((int) e.KeyChar == 8 || (int) e.KeyChar == 13 || (int) e.KeyChar >= 48 && (int) e.KeyChar <= 57)
        return;
      e.Handled = true;
    }

    private void dtpPaymentDate_ValueChanged(object sender, EventArgs e)
    {
      if ((int) this.type != 2)
        return;
      this.UpdatePacket();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.mpPeriodPay = new MonthPicker();
      this.mpPeriod = new MonthPicker();
      this.lblPacket = new Label();
      this.lblPeriodPay = new Label();
      this.cmbPurpose = new ComboBox();
      this.lblPurpose = new Label();
      this.cmbSource = new ComboBox();
      this.lblSource = new Label();
      this.lblPeriod = new Label();
      this.pnBtn = new Panel();
      this.btnLoadPays = new Button();
      this.btnExit = new Button();
      this.fdPayLoad = new OpenFileDialog();
      this.txbPacket = new TextBox();
      this.lblPaymentDate = new Label();
      this.dtpPaymentDate = new DateTimePicker();
      this.pnBtn.SuspendLayout();
      this.SuspendLayout();
      this.mpPeriodPay.CustomFormat = "MMMM yyyy";
      this.mpPeriodPay.Format = DateTimePickerFormat.Custom;
      this.mpPeriodPay.Location = new Point(12, 74);
      this.mpPeriodPay.MaxDate = new DateTime(2999, 12, 31, 0, 0, 0, 0);
      this.mpPeriodPay.Name = "mpPeriodPay";
      this.mpPeriodPay.OldMonth = 0;
      this.mpPeriodPay.ShowUpDown = true;
      this.mpPeriodPay.Size = new Size(140, 22);
      this.mpPeriodPay.TabIndex = 25;
      this.mpPeriod.CustomFormat = "MMMM yyyy";
      this.mpPeriod.Format = DateTimePickerFormat.Custom;
      this.mpPeriod.Location = new Point(12, 27);
      this.mpPeriod.MaxDate = new DateTime(2999, 12, 31, 0, 0, 0, 0);
      this.mpPeriod.Name = "mpPeriod";
      this.mpPeriod.OldMonth = 0;
      this.mpPeriod.ShowUpDown = true;
      this.mpPeriod.Size = new Size(140, 22);
      this.mpPeriod.TabIndex = 0;
      this.lblPacket.AutoSize = true;
      this.lblPacket.Location = new Point(10, 156);
      this.lblPacket.Name = "lblPacket";
      this.lblPacket.Size = new Size(49, 16);
      this.lblPacket.TabIndex = 33;
      this.lblPacket.Text = "Пачка";
      this.lblPeriodPay.AutoSize = true;
      this.lblPeriodPay.Location = new Point(9, 55);
      this.lblPeriodPay.Name = "lblPeriodPay";
      this.lblPeriodPay.Size = new Size(77, 16);
      this.lblPeriodPay.TabIndex = 32;
      this.lblPeriodPay.Text = "Платеж за";
      this.cmbPurpose.FormattingEnabled = true;
      this.cmbPurpose.Location = new Point(13, 221);
      this.cmbPurpose.Name = "cmbPurpose";
      this.cmbPurpose.Size = new Size(265, 24);
      this.cmbPurpose.TabIndex = 4;
      this.lblPurpose.AutoSize = true;
      this.lblPurpose.Location = new Point(10, 202);
      this.lblPurpose.Name = "lblPurpose";
      this.lblPurpose.Size = new Size(149, 16);
      this.lblPurpose.TabIndex = 31;
      this.lblPurpose.Text = "Назначение платежа";
      this.cmbSource.AutoCompleteMode = AutoCompleteMode.Append;
      this.cmbSource.AutoCompleteSource = AutoCompleteSource.CustomSource;
      this.cmbSource.FormattingEnabled = true;
      this.cmbSource.Location = new Point(12, 128);
      this.cmbSource.Name = "cmbSource";
      this.cmbSource.Size = new Size(265, 24);
      this.cmbSource.TabIndex = 2;
      this.cmbSource.SelectionChangeCommitted += new EventHandler(this.cmbSource_SelectionChangeCommitted);
      this.lblSource.AutoSize = true;
      this.lblSource.Location = new Point(9, 109);
      this.lblSource.Name = "lblSource";
      this.lblSource.Size = new Size(71, 16);
      this.lblSource.TabIndex = 30;
      this.lblSource.Text = "Источник";
      this.lblPeriod.AutoSize = true;
      this.lblPeriod.Location = new Point(9, 9);
      this.lblPeriod.Margin = new Padding(4, 0, 4, 0);
      this.lblPeriod.Name = "lblPeriod";
      this.lblPeriod.Size = new Size(58, 16);
      this.lblPeriod.TabIndex = 29;
      this.lblPeriod.Text = "Период";
      this.pnBtn.Controls.Add((Control) this.btnLoadPays);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 267);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(295, 40);
      this.pnBtn.TabIndex = 34;
      this.btnLoadPays.Location = new Point(12, 5);
      this.btnLoadPays.Name = "btnLoadPays";
      this.btnLoadPays.Size = new Size(101, 30);
      this.btnLoadPays.TabIndex = 5;
      this.btnLoadPays.Text = "Закачать";
      this.btnLoadPays.UseVisualStyleBackColor = true;
      this.btnLoadPays.Click += new EventHandler(this.btnLoadPays_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(208, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(75, 30);
      this.btnExit.TabIndex = 4;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.fdPayLoad.Filter = "dbf files (*.dbf)|*.dbf";
      this.fdPayLoad.InitialDirectory = "C:\\\\";
      this.txbPacket.Location = new Point(12, 175);
      this.txbPacket.Name = "txbPacket";
      this.txbPacket.Size = new Size(100, 22);
      this.txbPacket.TabIndex = 3;
      this.txbPacket.KeyPress += new KeyPressEventHandler(this.txbPacket_KeyPress);
      this.lblPaymentDate.AutoSize = true;
      this.lblPaymentDate.Location = new Point(12, 55);
      this.lblPaymentDate.Name = "lblPaymentDate";
      this.lblPaymentDate.Size = new Size(91, 16);
      this.lblPaymentDate.TabIndex = 36;
      this.lblPaymentDate.Text = "Дата оплаты";
      this.dtpPaymentDate.Location = new Point(12, 74);
      this.dtpPaymentDate.Name = "dtpPaymentDate";
      this.dtpPaymentDate.Size = new Size(140, 22);
      this.dtpPaymentDate.TabIndex = 1;
      this.dtpPaymentDate.ValueChanged += new EventHandler(this.dtpPaymentDate_ValueChanged);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(295, 307);
      this.Controls.Add((Control) this.dtpPaymentDate);
      this.Controls.Add((Control) this.lblPaymentDate);
      this.Controls.Add((Control) this.txbPacket);
      this.Controls.Add((Control) this.pnBtn);
      this.Controls.Add((Control) this.mpPeriodPay);
      this.Controls.Add((Control) this.mpPeriod);
      this.Controls.Add((Control) this.lblPacket);
      this.Controls.Add((Control) this.lblPeriodPay);
      this.Controls.Add((Control) this.cmbPurpose);
      this.Controls.Add((Control) this.lblPurpose);
      this.Controls.Add((Control) this.cmbSource);
      this.Controls.Add((Control) this.lblSource);
      this.Controls.Add((Control) this.lblPeriod);
      this.Margin = new Padding(5);
      this.Name = "FrmLoadPays";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Закачка платежей";
      this.Load += new EventHandler(this.FrmLoadPays_Load);
      this.pnBtn.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
