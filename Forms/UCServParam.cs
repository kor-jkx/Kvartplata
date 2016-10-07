// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.UCServParam
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
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class UCServParam : UserControl
  {
    private short Receipt_id = 0;
    private bool isNew = false;
    private IContainer components = (IContainer) null;
    private ISession session;
    private Label lblName;
    private TextBox tbPrintShow;
    private Label lblGroup;
    private TextBox tbGroup;
    private Label lblSocId;
    private TextBox tbSocId;
    private Label lblPeni;
    private ComboBox cbPeni;
    private Label lblBalance;
    private Label lblShowService;
    private Label label2;
    private Panel panel;
    private Label lblHeader;
    private DataSet dsMain;
    private DataTable dPeni;
    private DataColumn dataColumn1;
    private DataColumn dataColumn2;
    private ComboBox cbBal;
    private ComboBox cbSubs;
    private ComboBox cmbShowService;
    private Label label3;
    private ComboBox cbReceipt;
    private Button btnCancel;
    private Button btnSave;
    private DataTable dtCalc;
    private DataColumn dataColumn3;
    private DataColumn dataColumn4;
    private DataTable dtBalance;
    private DataColumn dataColumn5;
    private DataColumn dataColumn6;
    private DataTable dtSubsid;
    private DataColumn dataColumn7;
    private DataColumn dataColumn8;
    private Label label4;
    private TextBox tbSort;
    private Panel panelInf;
    private ComboBox cmbDistrService;
    private Label lblDistrService;
    private Label lblSendRent;
    private ComboBox cmbSendRent;
    private Label lblDedit;
    private TextBox txbDedit;
    private Label lblUname;
    private TextBox txbUname;
    private ComboBox cmbDublService;
    private Label lblDublService;
    private ComboBox cmbSaveOverpay;
    private Label lblSaveOverpay;
    private ComboBox cmbShowServiceInfo;
    private Label lblShowServiceInfo;
    private ComboBox cmbKomServ;
    private Label lblKomServ;
    private ComboBox cmbSpecial;
    private Label lblSpecial;

    public bool Selected { get; set; }

    public ServiceParam SrvParam { get; set; }

    public event EventHandler SelectedChanged;

    public event EventHandler AddedServParam;

    public UCServParam()
    {
      this.InitializeComponent();
    }

    public UCServParam(ServiceParam srvParam, bool isnew, IList receiptList)
    {
      this.InitializeComponent();
      this.session = Domain.CurrentSession;
      this.session.CreateQuery("from Service where Root=0 and ServiceId>0").List().Insert(0, (object) new Service((short) -1, (string) null));
      IList<YesNo> yesNoList1 = (IList<YesNo>) new List<YesNo>();
      this.cmbDistrService.DataSource = (object) this.session.CreateCriteria(typeof (YesNo)).List<YesNo>();
      this.cmbDistrService.DisplayMember = "YesNoName";
      this.cmbDistrService.ValueMember = "YesNoId";
      IList<YesNo> yesNoList2 = (IList<YesNo>) new List<YesNo>();
      this.cmbDublService.DataSource = (object) this.session.CreateCriteria(typeof (YesNo)).List<YesNo>();
      this.cmbDublService.DisplayMember = "YesNoName";
      this.cmbDublService.ValueMember = "YesNoId";
      IList<YesNo> yesNoList3 = (IList<YesNo>) new List<YesNo>();
      this.cmbSaveOverpay.DataSource = (object) this.session.CreateCriteria(typeof (YesNo)).List<YesNo>();
      this.cmbSaveOverpay.DisplayMember = "YesNoName";
      this.cmbSaveOverpay.ValueMember = "YesNoId";
      this.cbReceipt.DataSource = (object) receiptList;
      this.cbReceipt.DisplayMember = "ReceiptName";
      this.cbReceipt.ValueMember = "ReceiptId";
      IList<Service> serviceList1 = (IList<Service>) new List<Service>();
      IList<Service> serviceList2 = this.session.CreateQuery("from Service where Root=0 and ServiceId>0").List<Service>();
      serviceList2.Insert(0, new Service((short) 0, "Не субсидируемая"));
      this.cbSubs.DataSource = (object) serviceList2;
      this.cbSubs.DisplayMember = "ServiceName";
      this.cbSubs.ValueMember = "ServiceId";
      IList<YesNo> yesNoList4 = (IList<YesNo>) new List<YesNo>();
      this.cmbSendRent.DataSource = (object) this.session.CreateCriteria(typeof (YesNo)).List<YesNo>();
      this.cmbSendRent.DisplayMember = "YesNoName";
      this.cmbSendRent.ValueMember = "YesNoId";
      IList<YesNo> yesNoList5 = (IList<YesNo>) new List<YesNo>();
      this.cmbShowService.DataSource = (object) this.session.CreateCriteria(typeof (YesNo)).List<YesNo>();
      this.cmbShowService.DisplayMember = "YesNoName";
      this.cmbShowService.ValueMember = "YesNoId";
      IList<YesNo> yesNoList6 = (IList<YesNo>) new List<YesNo>();
      this.cmbShowServiceInfo.DataSource = (object) this.session.CreateCriteria(typeof (YesNo)).List<YesNo>();
      this.cmbShowServiceInfo.DisplayMember = "YesNoName";
      this.cmbShowServiceInfo.ValueMember = "YesNoId";
      IList<YesNo> yesNoList7 = (IList<YesNo>) new List<YesNo>();
      this.cmbKomServ.DataSource = (object) this.session.CreateCriteria(typeof (YesNo)).List<YesNo>();
      this.cmbKomServ.DisplayMember = "YesNoName";
      this.cmbKomServ.ValueMember = "YesNoId";
      KvrplHelper.AddRow(this.dPeni, 0, "нет");
      KvrplHelper.AddRow(this.dPeni, 1, "да");
      KvrplHelper.AddRow(this.dtBalance, 0, "нет");
      KvrplHelper.AddRow(this.dtBalance, 1, "да");
      this.SrvParam = srvParam;
      this.isNew = isnew;
      if (!Options.ViewEdit)
      {
        this.lblUname.Visible = false;
        this.lblDedit.Visible = false;
        this.txbUname.Visible = false;
        this.txbDedit.Visible = false;
      }
      this.LoadData();
    }

    public void LoadData()
    {
      if (this.SrvParam != null)
      {
        TextBox tbGroup = this.tbGroup;
        short num = this.SrvParam.Group_num;
        string str1 = num.ToString();
        tbGroup.Text = str1;
        this.tbPrintShow.Text = this.SrvParam.PrintShow;
        this.tbSocId.Text = this.SrvParam.CodeSoc_id.ToString();
        this.cbBal.SelectedValue = (object) this.SrvParam.BalanceIn;
        this.cbPeni.SelectedValue = (object) this.SrvParam.AcceptPeni;
        TextBox tbSort = this.tbSort;
        num = this.SrvParam.Sorter;
        string str2 = num.ToString();
        tbSort.Text = str2;
        this.Receipt_id = this.SrvParam.Receipt_id;
        this.cbReceipt.SelectedValue = (object) this.SrvParam.Receipt_id;
        if (this.SrvParam.ShowService != null)
          this.cmbShowService.SelectedValue = (object) this.SrvParam.ShowService.YesNoId;
        if (this.SrvParam.ShowServiceInfo != null)
          this.cmbShowServiceInfo.SelectedValue = (object) this.SrvParam.ShowServiceInfo.YesNoId;
        if (this.SrvParam.DistrService != null)
          this.cmbDistrService.SelectedValue = (object) this.SrvParam.DistrService.YesNoId;
        if (this.SrvParam.DublService != null)
          this.cmbDublService.SelectedValue = (object) this.SrvParam.DublService.YesNoId;
        if (this.SrvParam.SaveOverpay != null)
          this.cmbSaveOverpay.SelectedValue = (object) this.SrvParam.SaveOverpay.YesNoId;
        if (this.SrvParam.SubsidIn != null)
          this.cbSubs.SelectedValue = (object) this.SrvParam.SubsidIn.ServiceId;
        if (this.SrvParam.SendRent != null)
          this.cmbSendRent.SelectedValue = (object) this.SrvParam.SendRent.YesNoId;
        if (this.SrvParam.BoilService != null)
          this.cmbKomServ.SelectedValue = (object) this.SrvParam.BoilService.YesNoId;
        int specialId = (int) this.SrvParam.SpecialId;
        if (true)
          this.cmbSpecial.SelectedIndex = (int) this.SrvParam.SpecialId;
        this.lblHeader.Text = this.session.Get<Service>((object) this.SrvParam.Service_id).ServiceName;
        this.txbUname.Text = this.SrvParam.Uname;
        this.txbDedit.Text = this.SrvParam.Dedit.ToShortDateString();
      }
      if (!this.isNew)
      {
        this.btnCancel.Enabled = false;
        this.btnSave.Enabled = false;
      }
      else
      {
        this.btnCancel.Enabled = true;
        this.btnSave.Enabled = true;
      }
    }

    public void UpdateSort()
    {
      this.tbSort.Text = this.SrvParam.Sorter.ToString();
      this.btnCancel.Enabled = false;
      this.btnSave.Enabled = false;
    }

    private void UCServParam_Paint(object sender, PaintEventArgs e)
    {
      SolidBrush solidBrush = new SolidBrush(this.Selected ? Color.Gainsboro : Color.OldLace);
      e.Graphics.FillRectangle((Brush) solidBrush, this.DisplayRectangle);
      solidBrush.Dispose();
    }

    private void panelInf_Paint(object sender, PaintEventArgs e)
    {
      LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Point(0, 0), new Point(this.Width), this.Selected ? Color.Gainsboro : Color.OldLace, this.Selected ? Color.White : Color.White);
      e.Graphics.FillRectangle((Brush) linearGradientBrush, this.DisplayRectangle);
      linearGradientBrush.Dispose();
    }

    private void UCServParam_Click(object sender, EventArgs e)
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

    private void btnSave_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.cbReceipt.SelectedItem == null || (int) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId == -1 || (int) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId == 0)
        {
          int num = (int) MessageBox.Show("Укажите квитанцию!", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        this.SrvParam.Uname = Options.Login;
        this.SrvParam.Dedit = DateTime.Now;
        this.session.SaveOrUpdate((object) this.SrvParam);
        this.session.Flush();
      }
      catch
      {
        int num = (int) MessageBox.Show("Не удалось сохранить! Проверьте правильность введенных данных!", "Внимание!", MessageBoxButtons.OK);
        return;
      }
      this.btnCancel.Enabled = false;
      this.btnSave.Enabled = false;
      this.isNew = false;
      // ISSUE: reference to a compiler-generated field
      if (this.AddedServParam != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.AddedServParam((object) this, EventArgs.Empty);
      }
      this.txbUname.Text = this.SrvParam.Uname;
      this.txbDedit.Text = this.SrvParam.Dedit.ToShortDateString();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      if (this.session.Get<ServiceParam>((object) this.SrvParam) != null)
        this.session.Refresh((object) this.SrvParam);
      else
        this.Dispose();
      this.LoadData();
      this.btnCancel.Enabled = false;
      this.btnSave.Enabled = false;
    }

    private void tbPrintShow_TextChanged(object sender, EventArgs e)
    {
      this.btnCancel.Enabled = true;
      this.btnSave.Enabled = true;
      this.SrvParam.PrintShow = this.tbPrintShow.Text;
    }

    private void cbReceipt_SelectedValueChanged(object sender, EventArgs e)
    {
    }

    private void tbGroup_TextChanged(object sender, EventArgs e)
    {
      try
      {
        this.SrvParam.Group_num = Convert.ToInt16(this.tbGroup.Text);
        this.btnCancel.Enabled = true;
        this.btnSave.Enabled = true;
        if ((int) this.SrvParam.Group_num == 0)
        {
          this.lblShowService.Enabled = false;
          this.cmbShowService.Enabled = false;
          this.lblShowServiceInfo.Enabled = true;
          this.cmbShowServiceInfo.Enabled = true;
          this.cmbShowService.SelectedIndex = 1;
        }
        else
        {
          this.lblShowService.Enabled = true;
          this.cmbShowService.Enabled = true;
          this.lblShowServiceInfo.Enabled = false;
          this.cmbShowServiceInfo.Enabled = false;
          this.cmbShowServiceInfo.SelectedIndex = 1;
        }
      }
      catch
      {
        int num = (int) MessageBox.Show("Проверьте правильность ввода данных!", "Внимание!", MessageBoxButtons.OK);
      }
    }

    private void tbSocId_TextChanged(object sender, EventArgs e)
    {
      try
      {
        this.SrvParam.CodeSoc_id = Convert.ToInt32(this.tbSocId.Text);
        this.btnCancel.Enabled = true;
        this.btnSave.Enabled = true;
      }
      catch
      {
        int num = (int) MessageBox.Show("Проверьте правильность ввода данных!", "Внимание!", MessageBoxButtons.OK);
      }
    }

    private void cbPeni_SelectedValueChanged(object sender, EventArgs e)
    {
      this.btnCancel.Enabled = true;
      this.btnSave.Enabled = true;
      this.SrvParam.AcceptPeni = Convert.ToInt16(this.cbPeni.SelectedValue);
    }

    private void cbBal_SelectedValueChanged(object sender, EventArgs e)
    {
      this.btnCancel.Enabled = true;
      this.btnSave.Enabled = true;
      this.SrvParam.BalanceIn = Convert.ToInt16(this.cbBal.SelectedValue);
    }

    private void cbSubs_SelectedValueChanged(object sender, EventArgs e)
    {
    }

    private void cbSubs_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.btnCancel.Enabled = true;
      this.btnSave.Enabled = true;
      this.SrvParam.SubsidIn = (Service) this.cbSubs.SelectedItem;
    }

    private void tbSort_TextChanged(object sender, EventArgs e)
    {
      try
      {
        this.SrvParam.Sorter = Convert.ToInt16(this.tbSort.Text);
      }
      catch
      {
        int num = (int) MessageBox.Show("Проверьте правильность ввода данных!", "Внимание!", MessageBoxButtons.OK);
      }
    }

    private void cbCrossServ_SelectedValueChanged(object sender, EventArgs e)
    {
    }

    private void cbReceipt_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.btnCancel.Enabled = true;
      this.btnSave.Enabled = true;
      this.SrvParam.Receipt_id = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
      this.tbSort.Text = this.session.CreateQuery("select count(*) from ServiceParam where Company_id=:cid and Complex.ComplexId=:compid and Receipt_id=:rid").SetInt16("cid", this.SrvParam.Company_id).SetInt32("compid", this.SrvParam.Complex.ComplexId).SetInt16("rid", this.SrvParam.Receipt_id).UniqueResult().ToString();
    }

    private void cmbDistrService_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.btnCancel.Enabled = true;
      this.btnSave.Enabled = true;
      if (this.SrvParam == null)
        return;
      this.SrvParam.DistrService = (YesNo) this.cmbDistrService.SelectedItem;
    }

    private void cmbDublService_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.btnCancel.Enabled = true;
      this.btnSave.Enabled = true;
      if (this.SrvParam == null)
        return;
      this.SrvParam.DublService = (YesNo) this.cmbDublService.SelectedItem;
    }

    private void cmbSaveOverpay_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.btnCancel.Enabled = true;
      this.btnSave.Enabled = true;
      if (this.SrvParam == null)
        return;
      this.SrvParam.SaveOverpay = (YesNo) this.cmbSaveOverpay.SelectedItem;
    }

    private void cmbSendRent_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.btnCancel.Enabled = true;
      this.btnSave.Enabled = true;
      if (this.SrvParam == null)
        return;
      this.SrvParam.SendRent = (YesNo) this.cmbSendRent.SelectedItem;
    }

    private void cbCalc_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.btnCancel.Enabled = true;
      this.btnSave.Enabled = true;
      if (this.SrvParam == null)
        return;
      this.SrvParam.ShowService = (YesNo) this.cmbShowService.SelectedItem;
    }

    private void cmbShowServiceInfo_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.btnCancel.Enabled = true;
      this.btnSave.Enabled = true;
      if (this.SrvParam == null)
        return;
      this.SrvParam.ShowServiceInfo = (YesNo) this.cmbShowServiceInfo.SelectedItem;
    }

    private void cmbKomServ_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.btnCancel.Enabled = true;
      this.btnSave.Enabled = true;
      if (this.SrvParam == null)
        return;
      this.SrvParam.BoilService = (YesNo) this.cmbKomServ.SelectedItem;
    }

    private void cmbSpecial_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.btnCancel.Enabled = true;
      this.btnSave.Enabled = true;
      if (this.SrvParam == null)
        return;
      this.SrvParam.SpecialId = (short) this.cmbSpecial.SelectedIndex;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblName = new Label();
      this.tbPrintShow = new TextBox();
      this.lblGroup = new Label();
      this.tbGroup = new TextBox();
      this.lblSocId = new Label();
      this.tbSocId = new TextBox();
      this.lblPeni = new Label();
      this.cbPeni = new ComboBox();
      this.dsMain = new DataSet();
      this.dPeni = new DataTable();
      this.dataColumn1 = new DataColumn();
      this.dataColumn2 = new DataColumn();
      this.dtCalc = new DataTable();
      this.dataColumn3 = new DataColumn();
      this.dataColumn4 = new DataColumn();
      this.dtBalance = new DataTable();
      this.dataColumn5 = new DataColumn();
      this.dataColumn6 = new DataColumn();
      this.dtSubsid = new DataTable();
      this.dataColumn7 = new DataColumn();
      this.dataColumn8 = new DataColumn();
      this.lblBalance = new Label();
      this.lblShowService = new Label();
      this.label2 = new Label();
      this.panel = new Panel();
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.lblHeader = new Label();
      this.cbBal = new ComboBox();
      this.cbSubs = new ComboBox();
      this.cmbShowService = new ComboBox();
      this.label3 = new Label();
      this.cbReceipt = new ComboBox();
      this.label4 = new Label();
      this.tbSort = new TextBox();
      this.panelInf = new Panel();
      this.cmbSpecial = new ComboBox();
      this.lblSpecial = new Label();
      this.cmbKomServ = new ComboBox();
      this.lblKomServ = new Label();
      this.cmbShowServiceInfo = new ComboBox();
      this.lblShowServiceInfo = new Label();
      this.cmbSaveOverpay = new ComboBox();
      this.lblSaveOverpay = new Label();
      this.cmbDublService = new ComboBox();
      this.lblDublService = new Label();
      this.lblDedit = new Label();
      this.txbDedit = new TextBox();
      this.lblUname = new Label();
      this.txbUname = new TextBox();
      this.cmbSendRent = new ComboBox();
      this.lblSendRent = new Label();
      this.cmbDistrService = new ComboBox();
      this.lblDistrService = new Label();
      this.dsMain.BeginInit();
      this.dPeni.BeginInit();
      this.dtCalc.BeginInit();
      this.dtBalance.BeginInit();
      this.dtSubsid.BeginInit();
      this.panel.SuspendLayout();
      this.panelInf.SuspendLayout();
      this.SuspendLayout();
      this.lblName.AutoSize = true;
      this.lblName.BackColor = Color.Transparent;
      this.lblName.Location = new Point(4, 7);
      this.lblName.Margin = new Padding(4, 0, 4, 0);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(190, 17);
      this.lblName.TabIndex = 0;
      this.lblName.Text = "Наименование в квитанции";
      this.lblName.Click += new EventHandler(this.UCServParam_Click);
      this.tbPrintShow.Location = new Point(202, 4);
      this.tbPrintShow.Margin = new Padding(4);
      this.tbPrintShow.Name = "tbPrintShow";
      this.tbPrintShow.Size = new Size(355, 23);
      this.tbPrintShow.TabIndex = 1;
      this.tbPrintShow.Click += new EventHandler(this.UCServParam_Click);
      this.tbPrintShow.TextChanged += new EventHandler(this.tbPrintShow_TextChanged);
      this.lblGroup.AutoSize = true;
      this.lblGroup.BackColor = Color.Transparent;
      this.lblGroup.Location = new Point(4, 37);
      this.lblGroup.Margin = new Padding(4, 0, 4, 0);
      this.lblGroup.Name = "lblGroup";
      this.lblGroup.Size = new Size(55, 17);
      this.lblGroup.TabIndex = 2;
      this.lblGroup.Text = "Группа";
      this.lblGroup.Click += new EventHandler(this.UCServParam_Click);
      this.tbGroup.Location = new Point(61, 33);
      this.tbGroup.Margin = new Padding(4);
      this.tbGroup.Name = "tbGroup";
      this.tbGroup.Size = new Size(40, 23);
      this.tbGroup.TabIndex = 3;
      this.tbGroup.Click += new EventHandler(this.UCServParam_Click);
      this.tbGroup.TextChanged += new EventHandler(this.tbGroup_TextChanged);
      this.lblSocId.AutoSize = true;
      this.lblSocId.BackColor = Color.Transparent;
      this.lblSocId.Location = new Point(121, 36);
      this.lblSocId.Margin = new Padding(4, 0, 4, 0);
      this.lblSocId.Name = "lblSocId";
      this.lblSocId.Size = new Size(161, 17);
      this.lblSocId.TabIndex = 4;
      this.lblSocId.Text = "Номер в соцподдержке";
      this.lblSocId.Click += new EventHandler(this.UCServParam_Click);
      this.tbSocId.Location = new Point(285, 33);
      this.tbSocId.Margin = new Padding(4);
      this.tbSocId.Name = "tbSocId";
      this.tbSocId.Size = new Size(88, 23);
      this.tbSocId.TabIndex = 5;
      this.tbSocId.Click += new EventHandler(this.UCServParam_Click);
      this.tbSocId.TextChanged += new EventHandler(this.tbSocId_TextChanged);
      this.lblPeni.AutoSize = true;
      this.lblPeni.BackColor = Color.Transparent;
      this.lblPeni.Location = new Point(4, 65);
      this.lblPeni.Margin = new Padding(0);
      this.lblPeni.Name = "lblPeni";
      this.lblPeni.Size = new Size(115, 17);
      this.lblPeni.TabIndex = 6;
      this.lblPeni.Text = "Начислять пени";
      this.lblPeni.Click += new EventHandler(this.UCServParam_Click);
      this.cbPeni.DataSource = (object) this.dsMain;
      this.cbPeni.DisplayMember = "Table1.Name";
      this.cbPeni.FormattingEnabled = true;
      this.cbPeni.Location = new Point(120, 62);
      this.cbPeni.Margin = new Padding(4);
      this.cbPeni.Name = "cbPeni";
      this.cbPeni.Size = new Size(50, 24);
      this.cbPeni.TabIndex = 7;
      this.cbPeni.ValueMember = "Table1.ID";
      this.cbPeni.SelectedValueChanged += new EventHandler(this.cbPeni_SelectedValueChanged);
      this.cbPeni.Click += new EventHandler(this.UCServParam_Click);
      this.dsMain.DataSetName = "NewDataSet";
      this.dsMain.Tables.AddRange(new DataTable[4]
      {
        this.dPeni,
        this.dtCalc,
        this.dtBalance,
        this.dtSubsid
      });
      this.dPeni.Columns.AddRange(new DataColumn[2]
      {
        this.dataColumn1,
        this.dataColumn2
      });
      this.dPeni.TableName = "Table1";
      this.dataColumn1.ColumnName = "ID";
      this.dataColumn1.DataType = typeof (short);
      this.dataColumn2.ColumnName = "Name";
      this.dtCalc.Columns.AddRange(new DataColumn[2]
      {
        this.dataColumn3,
        this.dataColumn4
      });
      this.dtCalc.TableName = "Table2";
      this.dataColumn3.ColumnName = "ID";
      this.dataColumn3.DataType = typeof (short);
      this.dataColumn4.ColumnName = "Name";
      this.dtBalance.Columns.AddRange(new DataColumn[2]
      {
        this.dataColumn5,
        this.dataColumn6
      });
      this.dtBalance.TableName = "Table3";
      this.dataColumn5.ColumnName = "ID";
      this.dataColumn5.DataType = typeof (short);
      this.dataColumn6.ColumnName = "Name";
      this.dtSubsid.Columns.AddRange(new DataColumn[2]
      {
        this.dataColumn7,
        this.dataColumn8
      });
      this.dtSubsid.TableName = "Table4";
      this.dataColumn7.ColumnName = "ID";
      this.dataColumn7.DataType = typeof (short);
      this.dataColumn8.ColumnName = "Name";
      this.lblBalance.AutoSize = true;
      this.lblBalance.BackColor = Color.Transparent;
      this.lblBalance.Location = new Point(185, 65);
      this.lblBalance.Margin = new Padding(4, 0, 4, 0);
      this.lblBalance.Name = "lblBalance";
      this.lblBalance.Size = new Size(140, 17);
      this.lblBalance.TabIndex = 8;
      this.lblBalance.Text = "Учитывать в сальдо";
      this.lblBalance.Click += new EventHandler(this.UCServParam_Click);
      this.lblShowService.AutoSize = true;
      this.lblShowService.BackColor = Color.Transparent;
      this.lblShowService.Location = new Point(392, 65);
      this.lblShowService.Margin = new Padding(4, 0, 4, 0);
      this.lblShowService.Name = "lblShowService";
      this.lblShowService.Size = new Size(171, 17);
      this.lblShowService.TabIndex = 10;
      this.lblShowService.Text = "Показывать в квитанции";
      this.lblShowService.Click += new EventHandler(this.UCServParam_Click);
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(562, 36);
      this.label2.Margin = new Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(111, 17);
      this.label2.TabIndex = 12;
      this.label2.Text = "Субсидируемая";
      this.label2.Click += new EventHandler(this.UCServParam_Click);
      this.panel.BackColor = SystemColors.GradientInactiveCaption;
      this.panel.Controls.Add((Control) this.btnCancel);
      this.panel.Controls.Add((Control) this.btnSave);
      this.panel.Controls.Add((Control) this.lblHeader);
      this.panel.Dock = DockStyle.Top;
      this.panel.Location = new Point(0, 0);
      this.panel.Margin = new Padding(4);
      this.panel.Name = "panel";
      this.panel.Size = new Size(898, 31);
      this.panel.TabIndex = 14;
      this.panel.Click += new EventHandler(this.UCServParam_Click);
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.Enabled = false;
      this.btnCancel.Image = (Image) Resources.undo;
      this.btnCancel.Location = new Point(782, 0);
      this.btnCancel.Margin = new Padding(4);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(109, 28);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Отмена";
      this.btnCancel.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSave.Enabled = false;
      this.btnSave.Image = (Image) Resources.Applay_var;
      this.btnSave.Location = new Point(681, 0);
      this.btnSave.Margin = new Padding(4);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(103, 28);
      this.btnSave.TabIndex = 1;
      this.btnSave.Text = "Сохранить";
      this.btnSave.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.lblHeader.AutoSize = true;
      this.lblHeader.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.lblHeader.Location = new Point(13, 5);
      this.lblHeader.Margin = new Padding(4, 0, 4, 0);
      this.lblHeader.Name = "lblHeader";
      this.lblHeader.Size = new Size(52, 17);
      this.lblHeader.TabIndex = 0;
      this.lblHeader.Text = "Услуга";
      this.lblHeader.Click += new EventHandler(this.UCServParam_Click);
      this.cbBal.DataSource = (object) this.dsMain;
      this.cbBal.DisplayMember = "Table3.Name";
      this.cbBal.FormattingEnabled = true;
      this.cbBal.Location = new Point(324, 62);
      this.cbBal.Margin = new Padding(4);
      this.cbBal.Name = "cbBal";
      this.cbBal.Size = new Size(49, 24);
      this.cbBal.TabIndex = 15;
      this.cbBal.ValueMember = "Table3.ID";
      this.cbBal.SelectedValueChanged += new EventHandler(this.cbBal_SelectedValueChanged);
      this.cbBal.Click += new EventHandler(this.UCServParam_Click);
      this.cbSubs.DataSource = (object) this.dsMain;
      this.cbSubs.DisplayMember = "Table4.Name";
      this.cbSubs.FormattingEnabled = true;
      this.cbSubs.Location = new Point(681, 33);
      this.cbSubs.Margin = new Padding(4);
      this.cbSubs.Name = "cbSubs";
      this.cbSubs.Size = new Size(213, 24);
      this.cbSubs.TabIndex = 16;
      this.cbSubs.ValueMember = "Table4.ID";
      this.cbSubs.SelectionChangeCommitted += new EventHandler(this.cbSubs_SelectionChangeCommitted);
      this.cbSubs.SelectedValueChanged += new EventHandler(this.cbSubs_SelectedValueChanged);
      this.cbSubs.Click += new EventHandler(this.UCServParam_Click);
      this.cmbShowService.DataSource = (object) this.dsMain;
      this.cmbShowService.DisplayMember = "Table2.Name";
      this.cmbShowService.FormattingEnabled = true;
      this.cmbShowService.Location = new Point(565, 62);
      this.cmbShowService.Margin = new Padding(4);
      this.cmbShowService.Name = "cmbShowService";
      this.cmbShowService.Size = new Size(55, 24);
      this.cmbShowService.TabIndex = 17;
      this.cmbShowService.ValueMember = "Table2.ID";
      this.cmbShowService.SelectionChangeCommitted += new EventHandler(this.cbCalc_SelectionChangeCommitted);
      this.cmbShowService.Click += new EventHandler(this.UCServParam_Click);
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Location = new Point(585, 7);
      this.label3.Margin = new Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(79, 17);
      this.label3.TabIndex = 18;
      this.label3.Text = "Квитанция";
      this.label3.Click += new EventHandler(this.UCServParam_Click);
      this.cbReceipt.FormattingEnabled = true;
      this.cbReceipt.Location = new Point(672, 4);
      this.cbReceipt.Margin = new Padding(4);
      this.cbReceipt.Name = "cbReceipt";
      this.cbReceipt.Size = new Size(222, 24);
      this.cbReceipt.TabIndex = 19;
      this.cbReceipt.SelectionChangeCommitted += new EventHandler(this.cbReceipt_SelectionChangeCommitted);
      this.cbReceipt.SelectedValueChanged += new EventHandler(this.cbReceipt_SelectedValueChanged);
      this.cbReceipt.Click += new EventHandler(this.UCServParam_Click);
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(384, 36);
      this.label4.Margin = new Padding(4, 0, 4, 0);
      this.label4.Name = "label4";
      this.label4.Size = new Size(65, 17);
      this.label4.TabIndex = 20;
      this.label4.Text = "Порядок";
      this.label4.Click += new EventHandler(this.UCServParam_Click);
      this.tbSort.BackColor = SystemColors.Window;
      this.tbSort.Location = new Point(457, 34);
      this.tbSort.Margin = new Padding(4);
      this.tbSort.Name = "tbSort";
      this.tbSort.ReadOnly = true;
      this.tbSort.Size = new Size(83, 23);
      this.tbSort.TabIndex = 21;
      this.tbSort.Click += new EventHandler(this.UCServParam_Click);
      this.tbSort.TextChanged += new EventHandler(this.tbSort_TextChanged);
      this.panelInf.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panelInf.BackColor = Color.OldLace;
      this.panelInf.Controls.Add((Control) this.cmbSpecial);
      this.panelInf.Controls.Add((Control) this.lblSpecial);
      this.panelInf.Controls.Add((Control) this.cmbKomServ);
      this.panelInf.Controls.Add((Control) this.lblKomServ);
      this.panelInf.Controls.Add((Control) this.cmbShowServiceInfo);
      this.panelInf.Controls.Add((Control) this.lblShowServiceInfo);
      this.panelInf.Controls.Add((Control) this.cmbSaveOverpay);
      this.panelInf.Controls.Add((Control) this.lblSaveOverpay);
      this.panelInf.Controls.Add((Control) this.cmbDublService);
      this.panelInf.Controls.Add((Control) this.lblDublService);
      this.panelInf.Controls.Add((Control) this.lblDedit);
      this.panelInf.Controls.Add((Control) this.txbDedit);
      this.panelInf.Controls.Add((Control) this.lblUname);
      this.panelInf.Controls.Add((Control) this.txbUname);
      this.panelInf.Controls.Add((Control) this.cmbSendRent);
      this.panelInf.Controls.Add((Control) this.lblSendRent);
      this.panelInf.Controls.Add((Control) this.cmbDistrService);
      this.panelInf.Controls.Add((Control) this.lblDistrService);
      this.panelInf.Controls.Add((Control) this.lblName);
      this.panelInf.Controls.Add((Control) this.tbSort);
      this.panelInf.Controls.Add((Control) this.tbPrintShow);
      this.panelInf.Controls.Add((Control) this.label4);
      this.panelInf.Controls.Add((Control) this.lblGroup);
      this.panelInf.Controls.Add((Control) this.cbReceipt);
      this.panelInf.Controls.Add((Control) this.tbGroup);
      this.panelInf.Controls.Add((Control) this.label3);
      this.panelInf.Controls.Add((Control) this.lblSocId);
      this.panelInf.Controls.Add((Control) this.cmbShowService);
      this.panelInf.Controls.Add((Control) this.tbSocId);
      this.panelInf.Controls.Add((Control) this.cbSubs);
      this.panelInf.Controls.Add((Control) this.lblPeni);
      this.panelInf.Controls.Add((Control) this.cbBal);
      this.panelInf.Controls.Add((Control) this.cbPeni);
      this.panelInf.Controls.Add((Control) this.lblBalance);
      this.panelInf.Controls.Add((Control) this.label2);
      this.panelInf.Controls.Add((Control) this.lblShowService);
      this.panelInf.Location = new Point(0, 30);
      this.panelInf.Margin = new Padding(4);
      this.panelInf.Name = "panelInf";
      this.panelInf.Size = new Size(898, 178);
      this.panelInf.TabIndex = 22;
      this.panelInf.Click += new EventHandler(this.UCServParam_Click);
      this.panelInf.Paint += new PaintEventHandler(this.panelInf_Paint);
      this.cmbSpecial.FormattingEnabled = true;
      this.cmbSpecial.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "водоотведение",
        (object) "оплата работ",
        (object) "не делать  перерасчеты"
      });
      this.cmbSpecial.Location = new Point(754, 122);
      this.cmbSpecial.Name = "cmbSpecial";
      this.cmbSpecial.Size = new Size(140, 24);
      this.cmbSpecial.TabIndex = 47;
      this.cmbSpecial.SelectionChangeCommitted += new EventHandler(this.cmbSpecial_SelectionChangeCommitted);
      this.lblSpecial.AutoSize = true;
      this.lblSpecial.BackColor = Color.Transparent;
      this.lblSpecial.Location = new Point(596, 125);
      this.lblSpecial.Margin = new Padding(4, 0, 4, 0);
      this.lblSpecial.Name = "lblSpecial";
      this.lblSpecial.Size = new Size(154, 17);
      this.lblSpecial.TabIndex = 46;
      this.lblSpecial.Text = "Особенности расчета";
      this.cmbKomServ.FormattingEnabled = true;
      this.cmbKomServ.Location = new Point(509, 122);
      this.cmbKomServ.Name = "cmbKomServ";
      this.cmbKomServ.Size = new Size(68, 24);
      this.cmbKomServ.TabIndex = 45;
      this.cmbKomServ.SelectionChangeCommitted += new EventHandler(this.cmbKomServ_SelectionChangeCommitted);
      this.lblKomServ.AutoSize = true;
      this.lblKomServ.BackColor = Color.Transparent;
      this.lblKomServ.Location = new Point(351, 125);
      this.lblKomServ.Margin = new Padding(4, 0, 4, 0);
      this.lblKomServ.Name = "lblKomServ";
      this.lblKomServ.Size = new Size(151, 17);
      this.lblKomServ.TabIndex = 44;
      this.lblKomServ.Text = "Коммунальная услуга";
      this.cmbShowServiceInfo.DataSource = (object) this.dsMain;
      this.cmbShowServiceInfo.DisplayMember = "Table2.Name";
      this.cmbShowServiceInfo.FormattingEnabled = true;
      this.cmbShowServiceInfo.Location = new Point(844, 62);
      this.cmbShowServiceInfo.Margin = new Padding(4);
      this.cmbShowServiceInfo.Name = "cmbShowServiceInfo";
      this.cmbShowServiceInfo.Size = new Size(50, 24);
      this.cmbShowServiceInfo.TabIndex = 43;
      this.cmbShowServiceInfo.ValueMember = "Table2.ID";
      this.cmbShowServiceInfo.SelectionChangeCommitted += new EventHandler(this.cmbShowServiceInfo_SelectionChangeCommitted);
      this.lblShowServiceInfo.AutoSize = true;
      this.lblShowServiceInfo.BackColor = Color.Transparent;
      this.lblShowServiceInfo.Location = new Point(639, 65);
      this.lblShowServiceInfo.Margin = new Padding(4, 0, 4, 0);
      this.lblShowServiceInfo.Name = "lblShowServiceInfo";
      this.lblShowServiceInfo.Size = new Size(208, 17);
      this.lblShowServiceInfo.TabIndex = 42;
      this.lblShowServiceInfo.Text = "Показ-ть информ-ю об услуге ";
      this.cmbSaveOverpay.FormattingEnabled = true;
      this.cmbSaveOverpay.Location = new Point(442, 91);
      this.cmbSaveOverpay.Name = "cmbSaveOverpay";
      this.cmbSaveOverpay.Size = new Size(68, 24);
      this.cmbSaveOverpay.TabIndex = 41;
      this.cmbSaveOverpay.SelectionChangeCommitted += new EventHandler(this.cmbSaveOverpay_SelectionChangeCommitted);
      this.lblSaveOverpay.AutoSize = true;
      this.lblSaveOverpay.BackColor = Color.Transparent;
      this.lblSaveOverpay.Location = new Point(282, 94);
      this.lblSaveOverpay.Name = "lblSaveOverpay";
      this.lblSaveOverpay.Size = new Size(154, 17);
      this.lblSaveOverpay.TabIndex = 40;
      this.lblSaveOverpay.Text = "Сохранять переплаты";
      this.cmbDublService.FormattingEnabled = true;
      this.cmbDublService.Location = new Point(256, 122);
      this.cmbDublService.Margin = new Padding(4);
      this.cmbDublService.Name = "cmbDublService";
      this.cmbDublService.Size = new Size(75, 24);
      this.cmbDublService.TabIndex = 39;
      this.cmbDublService.SelectionChangeCommitted += new EventHandler(this.cmbDublService_SelectionChangeCommitted);
      this.lblDublService.AutoSize = true;
      this.lblDublService.BackColor = Color.Transparent;
      this.lblDublService.Location = new Point(5, 125);
      this.lblDublService.Margin = new Padding(4, 0, 4, 0);
      this.lblDublService.Name = "lblDublService";
      this.lblDublService.Size = new Size(246, 17);
      this.lblDublService.TabIndex = 38;
      this.lblDublService.Text = "Учитывать долг для выплат по МСП";
      this.lblDedit.AutoSize = true;
      this.lblDedit.BackColor = Color.Transparent;
      this.lblDedit.Location = new Point(653, 151);
      this.lblDedit.Margin = new Padding(4, 0, 4, 0);
      this.lblDedit.Name = "lblDedit";
      this.lblDedit.Size = new Size(155, 17);
      this.lblDedit.TabIndex = 36;
      this.lblDedit.Text = "Дата редактирования";
      this.txbDedit.Enabled = false;
      this.txbDedit.Location = new Point(816, 148);
      this.txbDedit.Margin = new Padding(4);
      this.txbDedit.Name = "txbDedit";
      this.txbDedit.Size = new Size(78, 23);
      this.txbDedit.TabIndex = 37;
      this.lblUname.AutoSize = true;
      this.lblUname.BackColor = Color.Transparent;
      this.lblUname.Location = new Point(439, 151);
      this.lblUname.Margin = new Padding(4, 0, 4, 0);
      this.lblUname.Name = "lblUname";
      this.lblUname.Size = new Size(101, 17);
      this.lblUname.TabIndex = 34;
      this.lblUname.Text = "Пользователь";
      this.txbUname.Enabled = false;
      this.txbUname.Location = new Point(548, 148);
      this.txbUname.Margin = new Padding(4);
      this.txbUname.Name = "txbUname";
      this.txbUname.Size = new Size(96, 23);
      this.txbUname.TabIndex = 35;
      this.cmbSendRent.FormattingEnabled = true;
      this.cmbSendRent.Location = new Point(819, 91);
      this.cmbSendRent.Name = "cmbSendRent";
      this.cmbSendRent.Size = new Size(75, 24);
      this.cmbSendRent.TabIndex = 33;
      this.cmbSendRent.SelectionChangeCommitted += new EventHandler(this.cmbSendRent_SelectionChangeCommitted);
      this.lblSendRent.AutoSize = true;
      this.lblSendRent.BackColor = Color.Transparent;
      this.lblSendRent.Location = new Point(545, 94);
      this.lblSendRent.Name = "lblSendRent";
      this.lblSendRent.Size = new Size(268, 17);
      this.lblSendRent.TabIndex = 32;
      this.lblSendRent.Text = "Начисления от сторонних организаций";
      this.cmbDistrService.FormattingEnabled = true;
      this.cmbDistrService.Location = new Point(192, 91);
      this.cmbDistrService.Margin = new Padding(4);
      this.cmbDistrService.Name = "cmbDistrService";
      this.cmbDistrService.Size = new Size(75, 24);
      this.cmbDistrService.TabIndex = 25;
      this.cmbDistrService.SelectionChangeCommitted += new EventHandler(this.cmbDistrService_SelectionChangeCommitted);
      this.lblDistrService.AutoSize = true;
      this.lblDistrService.BackColor = Color.Transparent;
      this.lblDistrService.Location = new Point(4, 94);
      this.lblDistrService.Margin = new Padding(4, 0, 4, 0);
      this.lblDistrService.Name = "lblDistrService";
      this.lblDistrService.Size = new Size(176, 17);
      this.lblDistrService.TabIndex = 24;
      this.lblDistrService.Text = "Формировать переплаты";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.OldLace;
      this.Controls.Add((Control) this.panel);
      this.Controls.Add((Control) this.panelInf);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.Margin = new Padding(4);
      this.Name = "UCServParam";
      this.Size = new Size(898, 208);
      this.Click += new EventHandler(this.UCServParam_Click);
      this.Paint += new PaintEventHandler(this.UCServParam_Paint);
      this.dsMain.EndInit();
      this.dPeni.EndInit();
      this.dtCalc.EndInit();
      this.dtBalance.EndInit();
      this.dtSubsid.EndInit();
      this.panel.ResumeLayout(false);
      this.panel.PerformLayout();
      this.panelInf.ResumeLayout(false);
      this.panelInf.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
