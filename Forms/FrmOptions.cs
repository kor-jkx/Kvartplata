// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmOptions
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmOptions : Form
  {
    private FormStateSaver fss = new FormStateSaver(FrmOptions.ic);
    private IContainer components = (IContainer) null;
    private static IContainer ic;
    private IList<City> cityList;
    private IList<Company> cmpList;
    private IList<Raion> raionList;
    private ISession session;
    private Panel panel1;
    private Button btnExit;
    private Button btnSave;
    private TabControl tCntrl;
    private TabPage pgMain;
    private ComboBox cmbCity;
    private ComboBox cmbCompany;
    private ComboBox cmbRaion;
    private Label lblCompany;
    private Label lblRaion;
    private Label lblCity;
    private Label lblServer;
    private TextBox txtServer;
    private CheckBox cbArea;
    private GroupBox gbSortService;
    private RadioButton rbSortAlph;
    private RadioButton rbSortNumber;
    public HelpProvider hp;
    private GroupBox gbViewEdit;
    private RadioButton rbNo;
    private RadioButton rbYes;
    private GroupBox gbSpacingSaldo;
    private RadioButton rbService;
    private RadioButton rbSupplier;
    private TextBox txbSeparator;
    private Label lblSeparator;
    private CheckBox cbCountersInPays;
    private CheckBox cbShowOldPays;
    private CheckBox cbOfferSum;
    private GroupBox gbFillCorrectSum;
    private RadioButton rbFillCorrect1;
    private RadioButton rbFillCorrect2;
    private RadioButton rbFillCorrect3;
    private ComboBox cbTypeAddress;
    private Label lblTypeAddress;
    private TabPage tbDop;
    private GroupBox groupBox1;
    private Button butOpenDir;
    private TextBox tbPathDirMain;
    private Label label1;

    public FrmOptions()
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void FrmOptions_Shown(object sender, EventArgs e)
    {
      this.txtServer.Text = Options.BaseName;
      this.session = Domain.CurrentSession;
      Period period = new Period();
      this.cityList = this.session.CreateCriteria(typeof (City)).AddOrder(Order.Asc("CityId")).List<City>();
      this.cmbCity.DataSource = (object) this.cityList;
      this.cmbCity.ValueMember = "CityId";
      this.cmbCity.DisplayMember = "CityName";
      string str1 = Options.ConfigValue("city");
      if (str1 != "")
        this.cmbCity.SelectedValue = (object) Convert.ToInt32(str1);
      this.raionList = this.session.CreateCriteria(typeof (Raion)).AddOrder(Order.Asc("IdRnn")).List<Raion>();
      this.cmbRaion.DataSource = (object) this.raionList;
      this.cmbRaion.ValueMember = "IdRnn";
      this.cmbRaion.DisplayMember = "Rnn";
      string str2 = Options.ConfigValue("raion");
      if (str2 != "")
        this.cmbRaion.SelectedValue = (object) Convert.ToInt32(str2);
      this.cmpList = this.session.CreateCriteria(typeof (Company)).AddOrder(Order.Asc("CompanyId")).CreateCriteria("Raion").Add((ICriterion) Restrictions.Eq("IdRnn", (object) Options.Raion)).List<Company>();
      this.cmpList.Insert(0, new Company((short) 0, ""));
      this.cmbCompany.DataSource = (object) this.cmpList;
      this.cmbCompany.ValueMember = "CompanyId";
      this.cmbCompany.DisplayMember = "CompanyName";
      string str3 = Options.ConfigValue("company");
      if (str3 != "")
        this.cmbCompany.SelectedValue = (object) Convert.ToInt16(str3);
      string str4 = Options.ConfigValue("round_area");
      if (str4 != "")
        this.cbArea.Checked = Convert.ToBoolean(str4);
      string str5 = Options.ConfigValue("sort_service");
      if (str5 != "")
        ((RadioButton) this.gbSortService.Controls[Convert.ToInt32(str5)]).Checked = true;
      else
        this.rbSortNumber.Checked = true;
      string str6 = Options.ConfigValue("view_edit");
      if (str6 != "")
        ((RadioButton) this.gbViewEdit.Controls[Convert.ToInt32(str6)]).Checked = true;
      else
        this.rbNo.Checked = true;
      string str7 = Options.ConfigValue("spacing_saldo");
      if (str7 != "")
        ((RadioButton) this.gbSpacingSaldo.Controls[Convert.ToInt32(str7)]).Checked = true;
      else
        this.rbService.Checked = true;
      this.txbSeparator.Text = Options.Separator;
      string str8 = Options.ConfigValue("counters_in_pays");
      if (str8 != "")
        this.cbCountersInPays.Checked = Convert.ToBoolean(str8);
      string str9 = Options.ConfigValue("show_old_pays");
      if (str9 != "")
        this.cbShowOldPays.Checked = Convert.ToBoolean(str9);
      string str10 = Options.ConfigValue("offer_sum");
      if (str10 != "")
        this.cbOfferSum.Checked = Convert.ToBoolean(str10);
      string str11 = Options.ConfigValue("fill_correctsum");
      if (str11 != "")
        ((RadioButton) this.gbFillCorrectSum.Controls[Convert.ToInt32(str11)]).Checked = true;
      else
        this.rbFillCorrect1.Checked = true;
      this.cbTypeAddress.SelectedIndex = Options.AddressSending;
      this.session.Clear();
      this.tbPathDirMain.Text = Options.ConfigValue("path_folder_main");
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      AppSettingsWriter appSettingsWriter = new AppSettingsWriter(Options.PathProfile + "\\" + Application.ProductName + ".exe.config");
      if (this.tCntrl.SelectedIndex == 0)
      {
        this.session = Domain.CurrentSession;
        Options.BaseName = this.txtServer.Text;
        Options.City = Convert.ToInt32(this.cmbCity.SelectedValue);
        Options.Raion = this.cmbRaion.SelectedValue == null ? 0 : Convert.ToInt32(this.cmbRaion.SelectedValue);
        Options.Company = this.cmbCompany.SelectedValue == null ? (Company) null : this.session.Get<Company>(this.cmbCompany.SelectedValue);
        appSettingsWriter["city"] = this.cmbCity.SelectedValue.ToString();
        appSettingsWriter["raion"] = this.cmbRaion.SelectedValue == null ? "0" : this.cmbRaion.SelectedValue.ToString();
        appSettingsWriter["company"] = this.cmbCompany.SelectedValue == null ? "0" : this.cmbCompany.SelectedValue.ToString();
        Options.RoundArea = this.cbArea.Checked;
        appSettingsWriter["round_area"] = this.cbArea.Checked.ToString().ToLower();
        if (this.rbSortAlph.Checked)
        {
          appSettingsWriter["sort_service"] = Convert.ToString(1);
          Options.SortService = " s.ServiceName";
        }
        else
        {
          appSettingsWriter["sort_service"] = Convert.ToString(0);
          Options.SortService = " s.ServiceId";
        }
        if (this.rbYes.Checked)
        {
          appSettingsWriter["view_edit"] = Convert.ToString(1);
          Options.ViewEdit = true;
        }
        else
        {
          appSettingsWriter["view_edit"] = Convert.ToString(0);
          Options.ViewEdit = false;
        }
        if (this.rbService.Checked)
        {
          appSettingsWriter["spacing_saldo"] = Convert.ToString(0);
          Options.SpacingSaldo = (short) 0;
        }
        else
        {
          appSettingsWriter["spacing_saldo"] = Convert.ToString(1);
          Options.SpacingSaldo = (short) 1;
        }
        if (this.txbSeparator.Text != "")
        {
          appSettingsWriter["separator"] = this.txbSeparator.Text;
          Options.Separator = this.txbSeparator.Text;
        }
        Options.CountersInPays = this.cbCountersInPays.Checked;
        appSettingsWriter["counters_in_pays"] = this.cbCountersInPays.Checked.ToString().ToLower();
        Options.ShowOldPays = this.cbShowOldPays.Checked;
        appSettingsWriter["show_old_pays"] = this.cbShowOldPays.Checked.ToString().ToLower();
        Options.OfferSum = this.cbOfferSum.Checked;
        appSettingsWriter["offer_sum"] = this.cbOfferSum.Checked.ToString().ToLower();
        if (this.rbFillCorrect1.Checked)
        {
          appSettingsWriter["fill_correctsum"] = Convert.ToString(0);
          Options.FillCorrectSum = (short) 0;
        }
        if (this.rbFillCorrect2.Checked)
        {
          appSettingsWriter["fill_correctsum"] = Convert.ToString(1);
          Options.FillCorrectSum = (short) 1;
        }
        if (this.rbFillCorrect3.Checked)
        {
          appSettingsWriter["fill_correctsum"] = Convert.ToString(2);
          Options.FillCorrectSum = (short) 2;
        }
        if (this.cbTypeAddress.SelectedIndex != -1)
        {
          appSettingsWriter["address_sending"] = Convert.ToString(this.cbTypeAddress.SelectedIndex);
          Options.AddressSending = this.cbTypeAddress.SelectedIndex;
        }
        appSettingsWriter.Save();
      }
      if (this.tCntrl.SelectedIndex != 1 || Options.ChangeConfigValue("path_folder_main", this.tbPathDirMain.Text, true))
        return;
      Options.AddNewConfigValue("path_folder_main", this.tbPathDirMain.Text, true);
    }

    private void cmbRaion_SelectionChangeCommitted(object sender, EventArgs e)
    {
      Options.Raion = Convert.ToInt32(this.cmbRaion.SelectedValue);
      this.cmbCompany.DataSource = (object) null;
      this.cmpList = this.session.CreateCriteria(typeof (Company)).AddOrder(Order.Asc("CompanyId")).CreateCriteria("Raion").Add((ICriterion) Restrictions.Eq("IdRnn", (object) Options.Raion)).List<Company>();
      this.cmpList.Insert(0, new Company((short) 0, ""));
      this.cmbCompany.DataSource = (object) this.cmpList;
      this.cmbCompany.ValueMember = "CompanyId";
      this.cmbCompany.DisplayMember = "CompanyName";
    }

    private void txbDivision_KeyPress(object sender, KeyPressEventArgs e)
    {
      if ((int) e.KeyChar == 8 || (int) e.KeyChar == 13 || (int) e.KeyChar == 44 || (int) e.KeyChar == 46)
        return;
      e.Handled = true;
    }

    private void butOpenDir_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      this.tbPathDirMain.Text = folderBrowserDialog.SelectedPath;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmOptions));
      this.panel1 = new Panel();
      this.btnSave = new Button();
      this.btnExit = new Button();
      this.tCntrl = new TabControl();
      this.pgMain = new TabPage();
      this.cbTypeAddress = new ComboBox();
      this.lblTypeAddress = new Label();
      this.gbFillCorrectSum = new GroupBox();
      this.rbFillCorrect1 = new RadioButton();
      this.rbFillCorrect2 = new RadioButton();
      this.rbFillCorrect3 = new RadioButton();
      this.cbOfferSum = new CheckBox();
      this.cbShowOldPays = new CheckBox();
      this.cbCountersInPays = new CheckBox();
      this.txbSeparator = new TextBox();
      this.lblSeparator = new Label();
      this.gbSpacingSaldo = new GroupBox();
      this.rbService = new RadioButton();
      this.rbSupplier = new RadioButton();
      this.gbViewEdit = new GroupBox();
      this.rbNo = new RadioButton();
      this.rbYes = new RadioButton();
      this.gbSortService = new GroupBox();
      this.rbSortNumber = new RadioButton();
      this.rbSortAlph = new RadioButton();
      this.cbArea = new CheckBox();
      this.lblServer = new Label();
      this.txtServer = new TextBox();
      this.lblCompany = new Label();
      this.lblRaion = new Label();
      this.lblCity = new Label();
      this.cmbCompany = new ComboBox();
      this.cmbRaion = new ComboBox();
      this.cmbCity = new ComboBox();
      this.tbDop = new TabPage();
      this.groupBox1 = new GroupBox();
      this.butOpenDir = new Button();
      this.tbPathDirMain = new TextBox();
      this.label1 = new Label();
      this.hp = new HelpProvider();
      this.panel1.SuspendLayout();
      this.tCntrl.SuspendLayout();
      this.pgMain.SuspendLayout();
      this.gbFillCorrectSum.SuspendLayout();
      this.gbSpacingSaldo.SuspendLayout();
      this.gbViewEdit.SuspendLayout();
      this.gbSortService.SuspendLayout();
      this.tbDop.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.btnSave);
      this.panel1.Controls.Add((Control) this.btnExit);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 606);
      this.panel1.Margin = new Padding(4);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(833, 40);
      this.panel1.TabIndex = 0;
      this.btnSave.Image = (Image) Resources.Tick;
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.Location = new Point(16, 5);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(108, 30);
      this.btnSave.TabIndex = 1;
      this.btnSave.Text = "Сохранить";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(741, 5);
      this.btnExit.Margin = new Padding(4);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(79, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.tCntrl.Controls.Add((Control) this.pgMain);
      this.tCntrl.Controls.Add((Control) this.tbDop);
      this.tCntrl.Dock = DockStyle.Fill;
      this.tCntrl.Location = new Point(0, 0);
      this.tCntrl.Name = "tCntrl";
      this.tCntrl.SelectedIndex = 0;
      this.tCntrl.Size = new Size(833, 606);
      this.tCntrl.TabIndex = 1;
      this.pgMain.Controls.Add((Control) this.cbTypeAddress);
      this.pgMain.Controls.Add((Control) this.lblTypeAddress);
      this.pgMain.Controls.Add((Control) this.gbFillCorrectSum);
      this.pgMain.Controls.Add((Control) this.cbOfferSum);
      this.pgMain.Controls.Add((Control) this.cbShowOldPays);
      this.pgMain.Controls.Add((Control) this.cbCountersInPays);
      this.pgMain.Controls.Add((Control) this.txbSeparator);
      this.pgMain.Controls.Add((Control) this.lblSeparator);
      this.pgMain.Controls.Add((Control) this.gbSpacingSaldo);
      this.pgMain.Controls.Add((Control) this.gbViewEdit);
      this.pgMain.Controls.Add((Control) this.gbSortService);
      this.pgMain.Controls.Add((Control) this.cbArea);
      this.pgMain.Controls.Add((Control) this.lblServer);
      this.pgMain.Controls.Add((Control) this.txtServer);
      this.pgMain.Controls.Add((Control) this.lblCompany);
      this.pgMain.Controls.Add((Control) this.lblRaion);
      this.pgMain.Controls.Add((Control) this.lblCity);
      this.pgMain.Controls.Add((Control) this.cmbCompany);
      this.pgMain.Controls.Add((Control) this.cmbRaion);
      this.pgMain.Controls.Add((Control) this.cmbCity);
      this.hp.SetHelpKeyword((Control) this.pgMain, "kv61.html");
      this.hp.SetHelpNavigator((Control) this.pgMain, HelpNavigator.Topic);
      this.pgMain.Location = new Point(4, 25);
      this.pgMain.Name = "pgMain";
      this.pgMain.Padding = new Padding(3);
      this.hp.SetShowHelp((Control) this.pgMain, true);
      this.pgMain.Size = new Size(825, 577);
      this.pgMain.TabIndex = 0;
      this.pgMain.Text = "Локализация и интерфейс";
      this.pgMain.UseVisualStyleBackColor = true;
      this.cbTypeAddress.FormattingEnabled = true;
      this.cbTypeAddress.Items.AddRange(new object[5]
      {
        (object) "",
        (object) "По умолчанию",
        (object) "Почтовый",
        (object) "Адрес лицевого",
        (object) "Юридический"
      });
      this.cbTypeAddress.Location = new Point(221, 221);
      this.cbTypeAddress.Name = "cbTypeAddress";
      this.cbTypeAddress.Size = new Size(121, 24);
      this.cbTypeAddress.TabIndex = 29;
      this.lblTypeAddress.AutoSize = true;
      this.lblTypeAddress.Location = new Point(9, 224);
      this.lblTypeAddress.Name = "lblTypeAddress";
      this.lblTypeAddress.Size = new Size(202, 16);
      this.lblTypeAddress.TabIndex = 28;
      this.lblTypeAddress.Text = "Тип адреса для счета аренды";
      this.gbFillCorrectSum.Controls.Add((Control) this.rbFillCorrect1);
      this.gbFillCorrectSum.Controls.Add((Control) this.rbFillCorrect2);
      this.gbFillCorrectSum.Controls.Add((Control) this.rbFillCorrect3);
      this.gbFillCorrectSum.Location = new Point(414, 454);
      this.gbFillCorrectSum.Name = "gbFillCorrectSum";
      this.gbFillCorrectSum.Size = new Size(343, 117);
      this.gbFillCorrectSum.TabIndex = 26;
      this.gbFillCorrectSum.TabStop = false;
      this.gbFillCorrectSum.Text = "При ручной разбивке корректировок";
      this.rbFillCorrect1.AutoSize = true;
      this.rbFillCorrect1.Location = new Point(6, 24);
      this.rbFillCorrect1.Name = "rbFillCorrect1";
      this.rbFillCorrect1.Size = new Size(333, 20);
      this.rbFillCorrect1.TabIndex = 2;
      this.rbFillCorrect1.TabStop = true;
      this.rbFillCorrect1.Text = "Оставить разбивку на усмотрение бухгалтера";
      this.rbFillCorrect1.UseVisualStyleBackColor = true;
      this.rbFillCorrect2.AutoSize = true;
      this.rbFillCorrect2.Location = new Point(6, 50);
      this.rbFillCorrect2.Name = "rbFillCorrect2";
      this.rbFillCorrect2.Size = new Size(316, 20);
      this.rbFillCorrect2.TabIndex = 1;
      this.rbFillCorrect2.TabStop = true;
      this.rbFillCorrect2.Text = "Кидать всю сумму на первую составляющую";
      this.rbFillCorrect2.UseVisualStyleBackColor = true;
      this.rbFillCorrect3.AutoSize = true;
      this.rbFillCorrect3.Location = new Point(6, 74);
      this.rbFillCorrect3.Name = "rbFillCorrect3";
      this.rbFillCorrect3.Size = new Size(312, 36);
      this.rbFillCorrect3.TabIndex = 0;
      this.rbFillCorrect3.TabStop = true;
      this.rbFillCorrect3.Text = "Кидать всю сумму на первую составлющую \r\nиз привязанных к счету";
      this.rbFillCorrect3.UseVisualStyleBackColor = true;
      this.cbOfferSum.AutoSize = true;
      this.cbOfferSum.Location = new Point(414, 428);
      this.cbOfferSum.Name = "cbOfferSum";
      this.cbOfferSum.Size = new Size(338, 20);
      this.cbOfferSum.TabIndex = 25;
      this.cbOfferSum.Text = "Предлагать сумму к оплате при вводе платежа";
      this.cbOfferSum.UseVisualStyleBackColor = true;
      this.cbShowOldPays.AutoSize = true;
      this.cbShowOldPays.Location = new Point(414, 401);
      this.cbShowOldPays.Name = "cbShowOldPays";
      this.cbShowOldPays.Size = new Size(378, 20);
      this.cbShowOldPays.TabIndex = 24;
      this.cbShowOldPays.Text = "Показывать существующие платежи при вводе новых";
      this.cbShowOldPays.UseVisualStyleBackColor = true;
      this.cbCountersInPays.AutoSize = true;
      this.cbCountersInPays.Location = new Point(414, 375);
      this.cbCountersInPays.Name = "cbCountersInPays";
      this.cbCountersInPays.Size = new Size(343, 20);
      this.cbCountersInPays.TabIndex = 23;
      this.cbCountersInPays.Text = "Ввод показаний счетчиков при вводе платежей";
      this.cbCountersInPays.UseVisualStyleBackColor = true;
      this.txbSeparator.Location = new Point(638, 338);
      this.txbSeparator.Name = "txbSeparator";
      this.txbSeparator.Size = new Size(39, 22);
      this.txbSeparator.TabIndex = 22;
      this.txbSeparator.KeyPress += new KeyPressEventHandler(this.txbDivision_KeyPress);
      this.lblSeparator.AutoSize = true;
      this.lblSeparator.Location = new Point(411, 341);
      this.lblSeparator.Name = "lblSeparator";
      this.lblSeparator.Size = new Size(221, 16);
      this.lblSeparator.TabIndex = 21;
      this.lblSeparator.Text = "Разделитель для выгрузки в csv";
      this.gbSpacingSaldo.Controls.Add((Control) this.rbService);
      this.gbSpacingSaldo.Controls.Add((Control) this.rbSupplier);
      this.gbSpacingSaldo.Location = new Point(414, 234);
      this.gbSpacingSaldo.Name = "gbSpacingSaldo";
      this.gbSpacingSaldo.Size = new Size(343, 93);
      this.gbSpacingSaldo.TabIndex = 20;
      this.gbSpacingSaldo.TabStop = false;
      this.gbSpacingSaldo.Text = "Разбивка на закладке \"Сальдо по счету\"";
      this.rbService.AutoSize = true;
      this.rbService.Location = new Point(6, 39);
      this.rbService.Name = "rbService";
      this.rbService.Size = new Size(101, 20);
      this.rbService.TabIndex = 1;
      this.rbService.TabStop = true;
      this.rbService.Text = "По услугам";
      this.rbService.UseVisualStyleBackColor = true;
      this.rbSupplier.AutoSize = true;
      this.rbSupplier.Location = new Point(6, 65);
      this.rbSupplier.Name = "rbSupplier";
      this.rbSupplier.Size = new Size(134, 20);
      this.rbSupplier.TabIndex = 0;
      this.rbSupplier.TabStop = true;
      this.rbSupplier.Text = "По поставщикам";
      this.rbSupplier.UseVisualStyleBackColor = true;
      this.gbViewEdit.Controls.Add((Control) this.rbNo);
      this.gbViewEdit.Controls.Add((Control) this.rbYes);
      this.gbViewEdit.Location = new Point(414, 131);
      this.gbViewEdit.Name = "gbViewEdit";
      this.gbViewEdit.Size = new Size(343, 88);
      this.gbViewEdit.TabIndex = 19;
      this.gbViewEdit.TabStop = false;
      this.gbViewEdit.Text = "Показывать польз-ля и дату редактирования";
      this.rbNo.AutoSize = true;
      this.rbNo.Location = new Point(6, 35);
      this.rbNo.Name = "rbNo";
      this.rbNo.Size = new Size(51, 20);
      this.rbNo.TabIndex = 1;
      this.rbNo.TabStop = true;
      this.rbNo.Text = "Нет";
      this.rbNo.UseVisualStyleBackColor = true;
      this.rbYes.AutoSize = true;
      this.rbYes.Location = new Point(6, 61);
      this.rbYes.Name = "rbYes";
      this.rbYes.Size = new Size(43, 20);
      this.rbYes.TabIndex = 0;
      this.rbYes.TabStop = true;
      this.rbYes.Text = "Да";
      this.rbYes.UseVisualStyleBackColor = true;
      this.gbSortService.Controls.Add((Control) this.rbSortNumber);
      this.gbSortService.Controls.Add((Control) this.rbSortAlph);
      this.gbSortService.Location = new Point(414, 52);
      this.gbSortService.Name = "gbSortService";
      this.gbSortService.Size = new Size(343, 73);
      this.gbSortService.TabIndex = 18;
      this.gbSortService.TabStop = false;
      this.gbSortService.Text = "Сортировать услуги";
      this.rbSortNumber.AutoSize = true;
      this.rbSortNumber.Location = new Point(6, 21);
      this.rbSortNumber.Name = "rbSortNumber";
      this.rbSortNumber.Size = new Size(96, 20);
      this.rbSortNumber.TabIndex = 2;
      this.rbSortNumber.TabStop = true;
      this.rbSortNumber.Text = "По номеру";
      this.rbSortNumber.UseVisualStyleBackColor = true;
      this.rbSortAlph.AutoSize = true;
      this.rbSortAlph.Location = new Point(6, 47);
      this.rbSortAlph.Name = "rbSortAlph";
      this.rbSortAlph.Size = new Size(113, 20);
      this.rbSortAlph.TabIndex = 1;
      this.rbSortAlph.TabStop = true;
      this.rbSortAlph.Text = "По алфавиту";
      this.rbSortAlph.UseVisualStyleBackColor = true;
      this.cbArea.AutoSize = true;
      this.cbArea.Location = new Point(414, 14);
      this.cbArea.Name = "cbArea";
      this.cbArea.Size = new Size(249, 20);
      this.cbArea.TabIndex = 17;
      this.cbArea.Text = "Округлять площадь до 2 - х знаков";
      this.cbArea.UseVisualStyleBackColor = true;
      this.lblServer.AutoSize = true;
      this.lblServer.Location = new Point(9, 163);
      this.lblServer.Margin = new Padding(4, 0, 4, 0);
      this.lblServer.Name = "lblServer";
      this.lblServer.Size = new Size(78, 16);
      this.lblServer.TabIndex = 15;
      this.lblServer.Text = "Сервер БД";
      this.txtServer.Enabled = false;
      this.txtServer.Location = new Point(12, 183);
      this.txtServer.Margin = new Padding(4);
      this.txtServer.Name = "txtServer";
      this.txtServer.Size = new Size(224, 22);
      this.txtServer.TabIndex = 14;
      this.lblCompany.AutoSize = true;
      this.lblCompany.Location = new Point(6, 100);
      this.lblCompany.Name = "lblCompany";
      this.lblCompany.Size = new Size(95, 16);
      this.lblCompany.TabIndex = 5;
      this.lblCompany.Text = "Организация";
      this.lblRaion.AutoSize = true;
      this.lblRaion.Location = new Point(9, 52);
      this.lblRaion.Name = "lblRaion";
      this.lblRaion.Size = new Size(49, 16);
      this.lblRaion.TabIndex = 4;
      this.lblRaion.Text = "Район";
      this.lblCity.AutoSize = true;
      this.lblCity.Location = new Point(9, 4);
      this.lblCity.Name = "lblCity";
      this.lblCity.Size = new Size(47, 16);
      this.lblCity.TabIndex = 3;
      this.lblCity.Text = "Город";
      this.cmbCompany.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCompany.FormattingEnabled = true;
      this.cmbCompany.Location = new Point(12, 116);
      this.cmbCompany.Name = "cmbCompany";
      this.cmbCompany.Size = new Size(360, 24);
      this.cmbCompany.TabIndex = 2;
      this.cmbRaion.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbRaion.FormattingEnabled = true;
      this.cmbRaion.Location = new Point(12, 68);
      this.cmbRaion.Name = "cmbRaion";
      this.cmbRaion.Size = new Size(360, 24);
      this.cmbRaion.TabIndex = 1;
      this.cmbRaion.SelectionChangeCommitted += new EventHandler(this.cmbRaion_SelectionChangeCommitted);
      this.cmbCity.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCity.FormattingEnabled = true;
      this.cmbCity.Location = new Point(12, 22);
      this.cmbCity.Name = "cmbCity";
      this.cmbCity.Size = new Size(360, 24);
      this.cmbCity.TabIndex = 0;
      this.tbDop.Controls.Add((Control) this.groupBox1);
      this.tbDop.Location = new Point(4, 25);
      this.tbDop.Name = "tbDop";
      this.tbDop.Size = new Size(825, 577);
      this.tbDop.TabIndex = 1;
      this.tbDop.Text = "Дополнительно";
      this.tbDop.UseVisualStyleBackColor = true;
      this.groupBox1.Controls.Add((Control) this.butOpenDir);
      this.groupBox1.Controls.Add((Control) this.tbPathDirMain);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Location = new Point(12, 20);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(804, 100);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Каталоги";
      this.butOpenDir.Image = (Image) Resources.add_item;
      this.butOpenDir.Location = new Point(667, 25);
      this.butOpenDir.Name = "butOpenDir";
      this.butOpenDir.Size = new Size(38, 23);
      this.butOpenDir.TabIndex = 2;
      this.butOpenDir.UseVisualStyleBackColor = true;
      this.butOpenDir.Click += new EventHandler(this.butOpenDir_Click);
      this.tbPathDirMain.Location = new Point(270, 25);
      this.tbPathDirMain.Name = "tbPathDirMain";
      this.tbPathDirMain.Size = new Size(391, 22);
      this.tbPathDirMain.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 28);
      this.label1.Name = "label1";
      this.label1.Size = new Size(257, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "Корневая папка для хранения файлов";
      this.hp.HelpNamespace = "Help.chm";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(833, 646);
      this.Controls.Add((Control) this.tCntrl);
      this.Controls.Add((Control) this.panel1);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.hp.SetHelpKeyword((Control) this, "kv61.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmOptions";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Настройки";
      this.Shown += new EventHandler(this.FrmOptions_Shown);
      this.panel1.ResumeLayout(false);
      this.tCntrl.ResumeLayout(false);
      this.pgMain.ResumeLayout(false);
      this.pgMain.PerformLayout();
      this.gbFillCorrectSum.ResumeLayout(false);
      this.gbFillCorrectSum.PerformLayout();
      this.gbSpacingSaldo.ResumeLayout(false);
      this.gbSpacingSaldo.PerformLayout();
      this.gbViewEdit.ResumeLayout(false);
      this.gbViewEdit.PerformLayout();
      this.gbSortService.ResumeLayout(false);
      this.gbSortService.PerformLayout();
      this.tbDop.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
