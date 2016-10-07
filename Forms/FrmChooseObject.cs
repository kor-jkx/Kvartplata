// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmChooseObject
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
  public class FrmChooseObject : Form
  {
    private bool checkAll = true;
    private bool checkAllVariant = true;
    private bool _createOrDelHmPhones = true;
    private IContainer components = (IContainer) null;
    private readonly ClientParam ClientParam;
    private readonly CounterRelation cntrRelation;
    private readonly Counter counter;
    private readonly CompanyParam currentParam;
    private readonly LsQuality currentQuality;
    private readonly HomeParam hmParam;
    private readonly HomesPhones hmPhones;
    private readonly Home home;
    private readonly IList<CorrectRent> listCorrectRent;
    private readonly LsService lsService;
    private readonly LsServiceParam lsServiceParam;
    private readonly LsSupplier lsSupplier;
    private readonly Period month;
    private readonly NoteBook noteBook;
    private readonly Rent rent;
    private readonly int serviceId;
    private readonly hmWorkDistribute workDistribute;
    private int ClientId;
    private string argument;
    private int city;
    private Company company;
    private CorrectRent correctRent;
    private int countVariantChecked;
    private DateTime dateArchive;
    private string paramValue;
    private ISession session;
    private Panel pnButton;
    private Button btnExit;
    private Button btnStart;
    private GroupBox gbChoice;
    private RadioButton rbtnFlat;
    private RadioButton rbtnLs;
    private RadioButton rbtnCompany;
    private RadioButton rbtnEntrance;
    private Label lblTxt;
    private MaskedTextBox tbEnd;
    private MaskedTextBox tbBegin;
    private RadioButton rbtnHome;
    private CheckBox chbArhive;
    private GroupBox gpbFlat;
    private RadioButton rbtnNotPrivat;
    private RadioButton rbtnPrivat;
    private RadioButton rbtnFloor;
    private CheckBox chbUchet;
    private GroupBox gpVariant;
    private GroupBox groupBox2;
    private Panel panel1;
    private Label lblCaption;
    private Button btnCheck;
    private CheckedListBox chlbObject;
    private CheckBox chbVariant;
    private Button btnCheckVariant;
    private CheckedListBox chlbVariant;
    private MonthCalendar mcArchive;
    private GroupBox gbLs;
    private RadioButton rbKv;
    private RadioButton rbAr;
    private GroupBox gbScheme;
    private RadioButton rbWithout;
    private RadioButton rbWith;
    public HelpProvider hp;
    private ComboBox cmbType;
    private GroupBox grWorkDistrib;
    private ComboBox cmbPerformer;
    private ComboBox cmbRecipient;
    private Label lblRecipient;
    private Label lblPerformer;
    private Label lblComission;
    private TextBox txbComission;
    private Label lblRate;
    private TextBox txbRate;
    private Label lblMonthCount;
    private TextBox txbMonthCount;
    private Label lblSheme;
    private ComboBox cmbShema;

    public bool Save { get; set; }

    public short CodeOperation { get; set; }

    public short HomeSave { get; set; }

    public Period MonthClosed { get; set; }

    public IList<LsClient> LsClientList { get; set; }

    public Decimal Rate { get; set; }

    public Decimal Comission { get; set; }

    public int MonthCount { get; set; }

    public int SchemeId { get; set; }

    public int performerId { get; set; }

    public int recipientId { get; set; }

    public FrmChooseObject(int clientId, ClientParam clientParam)
    {
      this.InitializeComponent();
      this.ClientId = clientId;
      this.ClientParam = clientParam;
    }

    public FrmChooseObject(int clientId, ClientParam clientParam, string paramValue)
    {
      this.InitializeComponent();
      this.ClientId = clientId;
      this.ClientParam = clientParam;
      this.paramValue = paramValue;
    }

    public FrmChooseObject(CompanyParam companyParam)
    {
      this.InitializeComponent();
      this.currentParam = companyParam;
    }

    public FrmChooseObject(LsQuality quality)
    {
      this.InitializeComponent();
      this.currentQuality = quality;
      this.ClientId = quality.LsClient.ClientId;
    }

    public FrmChooseObject(LsService lsService)
    {
      this.InitializeComponent();
      this.lsService = lsService;
      this.ClientId = lsService.Client.ClientId;
      this.serviceId = (int) lsService.Service.ServiceId;
      this.ClientParam = (ClientParam) null;
      this.currentParam = (CompanyParam) null;
      this.currentQuality = (LsQuality) null;
      this.LoadVariant();
      this.chbVariant.Visible = true;
      this.gbScheme.Visible = true;
    }

    public FrmChooseObject(LsSupplier lsSupplier)
    {
      this.InitializeComponent();
      this.lsService = (LsService) null;
      this.ClientId = lsSupplier.LsClient.ClientId;
      this.ClientParam = (ClientParam) null;
      this.currentParam = (CompanyParam) null;
      this.currentQuality = (LsQuality) null;
      this.lsSupplier = lsSupplier;
    }

    public FrmChooseObject(Rent rent)
    {
      this.InitializeComponent();
      this.lsService = (LsService) null;
      this.ClientId = rent.LsClient.ClientId;
      this.ClientParam = (ClientParam) null;
      this.currentParam = (CompanyParam) null;
      this.currentQuality = (LsQuality) null;
      this.rent = rent;
    }

    public FrmChooseObject(Period period, LsClient client, Service service, Period month, string note)
    {
      this.InitializeComponent();
      this.lsService = (LsService) null;
      this.ClientId = client.ClientId;
      this.ClientParam = (ClientParam) null;
      this.currentParam = (CompanyParam) null;
      this.currentQuality = (LsQuality) null;
      this.month = month;
      this.serviceId = (int) service.ServiceId;
      this.session = Domain.CurrentSession;
      this.listCorrectRent = this.session.CreateQuery(string.Format("select r from CorrectRent r, Service s where r.Period.PeriodId={0} and r.LsClient.ClientId={1} and r.Month.PeriodId={2} and s.Root={3} and r.Note='{4}' and r.Service=s ", (object) Options.Period.PeriodId, (object) client.ClientId, (object) month.PeriodId, (object) service.ServiceId, (object) note)).List<CorrectRent>();
    }

    public FrmChooseObject(Home home)
    {
      this.InitializeComponent();
      this.lsService = (LsService) null;
      this.ClientId = 0;
      this.ClientParam = (ClientParam) null;
      this.currentParam = (CompanyParam) null;
      this.currentQuality = (LsQuality) null;
      this.lsSupplier = (LsSupplier) null;
      this.home = home;
    }

    public FrmChooseObject(hmWorkDistribute workDistribute, IList<BaseOrg> recipientList, IList<BaseOrg> perforemerList)
    {
      this.InitializeComponent();
      this.lsService = (LsService) null;
      this.ClientId = 0;
      this.ClientParam = (ClientParam) null;
      this.currentParam = (CompanyParam) null;
      this.currentQuality = (LsQuality) null;
      this.lsSupplier = (LsSupplier) null;
      this.workDistribute = workDistribute;
      this.grWorkDistrib.Visible = true;
      this.cmbRecipient.DataSource = (object) recipientList;
      this.cmbRecipient.DisplayMember = "NameOrgMin";
      this.cmbRecipient.ValueMember = "BaseOrgId";
      this.cmbPerformer.DataSource = (object) perforemerList;
      this.cmbPerformer.DisplayMember = "NameOrgMin";
      this.cmbPerformer.ValueMember = "BaseOrgId";
      this.session = Domain.CurrentSession;
      IList<Scheme> schemeList = (IList<Scheme>) new List<Scheme>();
      this.cmbShema.DataSource = (object) this.session.CreateCriteria(typeof (Scheme)).Add((ICriterion) Restrictions.Eq("SchemeType", (object) (short) 13)).AddOrder(Order.Asc("Sorter")).AddOrder(Order.Asc("SchemeId")).List<Scheme>();
      this.cmbShema.DisplayMember = "SchemeName";
      this.cmbShema.ValueMember = "SchemeId";
    }

    public FrmChooseObject(hmWorkDistribute workDistribute)
    {
      this.InitializeComponent();
      this.lsService = (LsService) null;
      this.ClientId = 0;
      this.ClientParam = (ClientParam) null;
      this.currentParam = (CompanyParam) null;
      this.currentQuality = (LsQuality) null;
      this.lsSupplier = (LsSupplier) null;
      this.workDistribute = workDistribute;
    }

    public FrmChooseObject(HomeParam hmParam)
    {
      this.InitializeComponent();
      this.lsService = (LsService) null;
      this.ClientId = 0;
      this.ClientParam = (ClientParam) null;
      this.currentParam = (CompanyParam) null;
      this.currentQuality = (LsQuality) null;
      this.lsSupplier = (LsSupplier) null;
      this.home = hmParam.Home;
      this.hmParam = hmParam;
    }

    public FrmChooseObject(HomesPhones hmPhones, int option)
    {
      this.InitializeComponent();
      this.lsService = (LsService) null;
      this.ClientId = 0;
      this.ClientParam = (ClientParam) null;
      this.currentParam = (CompanyParam) null;
      this.currentQuality = (LsQuality) null;
      this.lsSupplier = (LsSupplier) null;
      this.home = hmPhones.Home;
      this.hmParam = (HomeParam) null;
      this.hmPhones = hmPhones;
      if (option != 1)
        return;
      this.ClientId = hmPhones.ClientId;
    }

    public FrmChooseObject(HomesPhones hmPhones, bool cORd)
    {
      this.InitializeComponent();
      this.lsService = (LsService) null;
      this.ClientId = 0;
      this.ClientParam = (ClientParam) null;
      this.currentParam = (CompanyParam) null;
      this.currentQuality = (LsQuality) null;
      this.lsSupplier = (LsSupplier) null;
      this.home = (Home) null;
      this.hmParam = (HomeParam) null;
      this.hmPhones = hmPhones;
      this._createOrDelHmPhones = cORd;
      this.HomeSave = (short) -1;
    }

    public FrmChooseObject(CounterRelation cntrRelation)
    {
      this.InitializeComponent();
      this.lsService = (LsService) null;
      this.ClientId = cntrRelation.LsClient.ClientId;
      this.ClientParam = (ClientParam) null;
      this.currentParam = (CompanyParam) null;
      this.currentQuality = (LsQuality) null;
      this.lsSupplier = (LsSupplier) null;
      this.home = (Home) null;
      this.hmParam = (HomeParam) null;
      this.cntrRelation = cntrRelation;
    }

    public FrmChooseObject(Counter counter)
    {
      this.InitializeComponent();
      this.lsService = (LsService) null;
      this.ClientId = counter.LsClient.ClientId;
      this.ClientParam = (ClientParam) null;
      this.currentParam = (CompanyParam) null;
      this.currentQuality = (LsQuality) null;
      this.lsSupplier = (LsSupplier) null;
      this.home = (Home) null;
      this.hmParam = (HomeParam) null;
      this.cntrRelation = (CounterRelation) null;
      this.counter = counter;
    }

    public FrmChooseObject(LsServiceParam lsServiceParam, string paramValue)
    {
      this.InitializeComponent();
      this.lsServiceParam = lsServiceParam;
      this.serviceId = (int) lsServiceParam.Service.ServiceId;
      this.ClientId = lsServiceParam.LsClient.ClientId;
      this.ClientParam = (ClientParam) null;
      this.currentParam = (CompanyParam) null;
      this.currentQuality = (LsQuality) null;
      this.LoadVariant();
      this.chbVariant.Visible = true;
      this.gbScheme.Visible = true;
      this.paramValue = paramValue;
    }

    public FrmChooseObject(NoteBook noteBook)
    {
      this.InitializeComponent();
      this.lsService = (LsService) null;
      this.ClientId = noteBook.ClientId;
      this.ClientParam = (ClientParam) null;
      this.currentParam = (CompanyParam) null;
      this.currentQuality = (LsQuality) null;
      this.hmParam = (HomeParam) null;
      this.cntrRelation = (CounterRelation) null;
      this.noteBook = noteBook;
    }

    private void FrmChooseObject_Load(object sender, EventArgs e)
    {
      this.LoadChLbObject();
      switch (this.CodeOperation)
      {
        case 1:
          this.Text = this.Text + ". Копирование";
          break;
        case 2:
          this.Text = this.Text + ". Удаление";
          break;
        case 3:
          this.Text = this.Text + ". Обновление";
          break;
      }
      if ((uint) this.ClientId > 0U)
        this.company = this.session.CreateQuery(string.Format("select c.Company from LsClient c where c.ClientId={0}", (object) this.ClientId)).List<Company>()[0];
      else if (this.home != null)
        this.company = this.home.Company;
      else if (this.currentParam != null)
        this.company = this.currentParam.Company;
      if (this.home == null && this.currentParam == null && this.hmPhones != null)
        this.company = this.hmPhones.Company;
      this.city = Convert.ToInt32(KvrplHelper.BaseValue(1, this.company));
      IList<Scheme> schemeList = (IList<Scheme>) new List<Scheme>();
      try
      {
        schemeList = this.session.CreateCriteria(typeof (Scheme)).Add((ICriterion) Restrictions.Eq("SchemeType", (object) Convert.ToInt16(6))).AddOrder(Order.Asc("SchemeId")).List<Scheme>();
        schemeList.Insert(0, new Scheme((short) 0, ""));
      }
      catch
      {
      }
      this.cmbType.DataSource = (object) schemeList;
      this.cmbType.DisplayMember = "SchemeName";
      this.cmbType.ValueMember = "SchemeId";
    }

    private void LoadVariant()
    {
      this.session = Domain.CurrentSession;
      LsClient lsClient = this.session.Get<LsClient>((object) this.ClientId);
      this.session.Clear();
      Period nextPeriod = KvrplHelper.GetNextPeriod(KvrplHelper.GetKvrClose(this.ClientId, Options.ComplexPasp, Options.ComplexPrior));
      foreach (Tariff tariff in (IEnumerable<Tariff>) this.session.CreateQuery(string.Format("select distinct t from Tariff t, LsService c where c.Tariff.Tariff_id = t.Tariff_id  and t.Service.ServiceId = {0} and t.Service.ServiceId= c.Service.ServiceId  and c.Client.Home.IdHome = {1} and c.DBeg <= '{2}' and c.DEnd >= '{3}' order by t.Tariff_num ", (object) this.serviceId, (object) lsClient.Home.IdHome, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(nextPeriod.PeriodName.Value)), (object) KvrplHelper.DateToBaseFormat(nextPeriod.PeriodName.Value))).List<Tariff>())
        this.chlbVariant.Items.Add((object) tariff.Tariff_num);
    }

    private void LoadChLbObject()
    {
      this.session = Domain.CurrentSession;
      this.chbArhive.Checked = true;
      this.chbUchet.Checked = true;
      if ((uint) this.ClientId > 0U)
      {
        foreach (LsClient lsClient in (IEnumerable<LsClient>) this.GetListLsClient(""))
          this.chlbObject.Items.Add((object) lsClient.GetStrFlat());
        this.rbtnLs.Checked = true;
        this.rbtnCompany.Enabled = false;
        this.rbtnHome.Enabled = false;
      }
      else if ((this.home != null || this.hmParam != null || this.hmPhones != null) && (int) this.HomeSave != -1)
      {
        int num = 0;
        switch (this.HomeSave)
        {
          case 1:
            num = (int) this.hmParam.Company.CompanyId;
            break;
          case 2:
            num = (int) this.home.Company.CompanyId;
            break;
          case 3:
            num = (int) this.hmPhones.Company.CompanyId;
            break;
        }
        this.chlbObject.DataSource = (object) this.session.CreateQuery(string.Format("from Home h where h.Company.CompanyId={0} order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome),h.HomeKorp", (object) num)).List<Home>();
        this.chlbObject.DisplayMember = "Address";
        this.chlbObject.ValueMember = "IdHome";
        this.lblCaption.Text = "Дом";
        this.rbtnLs.Enabled = false;
        this.rbtnCompany.Enabled = false;
        this.rbtnHome.Checked = true;
        this.rbtnFlat.Enabled = false;
        this.rbtnEntrance.Enabled = false;
        this.rbtnFloor.Enabled = false;
        this.gpbFlat.Visible = false;
        this.chbArhive.Visible = false;
        this.chbUchet.Visible = false;
        this.DisableFlatTextBox();
      }
      else if (this.workDistribute != null)
      {
        this.ClientId = this.session.CreateQuery("select w from LsClient w where w.Company=:com and w.Home=:h").SetParameter<Company>("com", this.workDistribute.Company).SetParameter<Home>("h", this.workDistribute.Home).List<LsClient>()[0].ClientId;
        IList<LsClient> lsClientList = (IList<LsClient>) new List<LsClient>();
        foreach (LsClient lsClient in (IEnumerable<LsClient>) this.GetListLsClient(""))
          this.chlbObject.Items.Add((object) lsClient.GetStrFlat());
        this.rbtnLs.Checked = true;
        this.rbtnCompany.Enabled = false;
        this.rbtnHome.Enabled = false;
      }
      else
      {
        this.chlbObject.DataSource = (object) this.session.CreateCriteria(typeof (Company)).AddOrder(Order.Asc("CompanyId")).List<Company>();
        this.chlbObject.DisplayMember = "CompanyName";
        this.chlbObject.ValueMember = "CompanyId";
        this.lblCaption.Text = "Участок";
        this.DisableFlatTextBox();
        this.rbtnLs.Enabled = false;
        this.rbtnFlat.Enabled = false;
        this.rbtnEntrance.Enabled = false;
        this.rbtnCompany.Checked = true;
        this.rbtnHome.Checked = false;
        this.rbtnHome.Enabled = false;
        this.rbtnFloor.Enabled = false;
        this.gpbFlat.Visible = false;
        this.chbArhive.Visible = false;
        this.chbUchet.Visible = false;
      }
      this.session.Clear();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      this.btnStart.Enabled = false;
      if ((int) this.CodeOperation == 4)
      {
        this.mcArchive.MinDate = KvrplHelper.GetCmpKvrClose(this.session.Get<LsClient>((object) this.ClientId).Company, Options.ComplexPasp.ComplexId, Options.ComplexPrior.ComplexId).PeriodName.Value.AddMonths(1);
        this.mcArchive.Parent = (Control) Form.ActiveForm;
        this.mcArchive.Visible = true;
        this.mcArchive.BringToFront();
        this.mcArchive.Show();
      }
      else
        this.SaveCheckedItems();
      this.btnStart.Enabled = true;
    }

    private void SaveCheckedItems()
    {
      int[] numArray = new int[1000];
      int index1 = 0;
      Company company = new Company();
      LsClient lsClient1 = this.session.Get<LsClient>((object) this.ClientId);
      Company cmp = (uint) this.ClientId <= 0U ? (this.home != null ? this.home.Company : (Company) null) : this.session.CreateQuery(string.Format("select ls.Company from LsClient ls where ls.ClientId={0}", (object) this.ClientId)).List<Company>()[0];
      bool flag = false;
      string str1 = "";
      if (this.rbtnLs.Checked)
      {
        if (this.currentQuality != null)
        {
          try
          {
            Service service = this.session.Get<Service>((object) this.currentQuality.Quality.Service_id);
            IList<NoteBook> noteBookList = this.session.CreateQuery(string.Format("from NoteBook where Company.CompanyId={0} and IdHome={1} and ClientId={2} and Text=:notetext and DBeg>=:notedate", (object) cmp.CompanyId, (object) lsClient1.Home.IdHome, (object) this.ClientId)).SetParameter<string>("notetext", "Качество с " + this.currentQuality.DBeg.ToShortDateString() + " по " + this.currentQuality.DEnd.Value.ToShortDateString() + ". Услуга: " + service.ServiceName).SetDateTime("notedate", this.MonthClosed.PeriodName.Value.AddMonths(1)).List<NoteBook>();
            if (noteBookList.Count > 0)
              this.argument = noteBookList[0].Note;
            else if (this.city == 28 && MessageBox.Show("Внести основание?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
              FrmArgument frmArgument = new FrmArgument();
              int num = (int) frmArgument.ShowDialog();
              this.argument = frmArgument.Argument();
              frmArgument.Dispose();
            }
          }
          catch (Exception ex)
          {
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
        }
        if (this.lsSupplier != null && (uint) this.lsSupplier.Period.PeriodId > 0U)
        {
          try
          {
            this.lsSupplier.Supplier = this.session.Get<Supplier>((object) this.lsSupplier.Supplier.SupplierId);
            Service service = this.session.Get<Service>((object) this.lsSupplier.Service.Root);
            IList<NoteBook> noteBookList = this.session.CreateQuery(string.Format("from NoteBook where Company.CompanyId={0} and IdHome={1} and ClientId={2} and Text=:notetext and DBeg>=:notedate", (object) cmp.CompanyId, (object) lsClient1.Home.IdHome, (object) this.ClientId)).SetParameter<string>("notetext", "В прошлом времени по услуге " + service.ServiceName + ", составляющей " + this.lsSupplier.Service.ServiceName + " c " + this.lsSupplier.DBeg.ToShortDateString() + " по " + this.lsSupplier.DEnd.ToShortDateString() + " занесен поставщик " + this.lsSupplier.Supplier.Recipient.NameOrgMin + " - " + this.lsSupplier.Supplier.Perfomer.NameOrgMin).SetDateTime("notedate", this.MonthClosed.PeriodName.Value.AddMonths(1)).List<NoteBook>();
            if (noteBookList.Count > 0)
              this.argument = noteBookList[0].Note;
            else if (this.city == 28 && MessageBox.Show("Внести основание?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
              FrmArgument frmArgument = new FrmArgument();
              int num = (int) frmArgument.ShowDialog();
              this.argument = frmArgument.Argument();
              frmArgument.Dispose();
            }
          }
          catch (Exception ex)
          {
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
        }
        if (this.lsService != null && (uint) this.lsService.Period.PeriodId > 0U)
        {
          try
          {
            ISession session = this.session;
            string format = "from NoteBook where Company.CompanyId={0} and IdHome={1} and ClientId={2} and Text like '%{3}%' and DBeg>=:notedate";
            object[] objArray = new object[4]{ (object) cmp.CompanyId, (object) lsClient1.Home.IdHome, (object) this.ClientId, null };
            int index2 = 3;
            string[] strArray = new string[6]{ "В прошлом времени занесена услуга ", this.lsService.ServiceName, " c ", null, null, null };
            int index3 = 3;
            DateTime dateTime = this.lsService.DBeg;
            string shortDateString1 = dateTime.ToShortDateString();
            strArray[index3] = shortDateString1;
            int index4 = 4;
            string str2 = " по ";
            strArray[index4] = str2;
            int index5 = 5;
            dateTime = this.lsService.DEnd;
            string shortDateString2 = dateTime.ToShortDateString();
            strArray[index5] = shortDateString2;
            string str3 = string.Concat(strArray);
            objArray[index2] = (object) str3;
            string queryString = string.Format(format, objArray);
            IQuery query = session.CreateQuery(queryString);
            string name = "notedate";
            dateTime = this.MonthClosed.PeriodName.Value;
            DateTime val = dateTime.AddMonths(1);
            IList<NoteBook> noteBookList = query.SetDateTime(name, val).List<NoteBook>();
            if (noteBookList.Count > 0)
              this.argument = noteBookList[0].Note;
            else if (this.city == 28 && MessageBox.Show("Внести основание?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
              FrmArgument frmArgument = new FrmArgument();
              int num = (int) frmArgument.ShowDialog();
              this.argument = frmArgument.Argument();
              frmArgument.Dispose();
            }
          }
          catch (Exception ex)
          {
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
        }
        if (this.workDistribute != null & this.grWorkDistrib.Visible)
        {
          try
          {
            this.Rate = Convert.ToDecimal(this.txbRate.Text);
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Введите процент", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          try
          {
            this.Comission = Convert.ToDecimal(this.txbComission.Text);
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Введите комиссию", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          this.MonthCount = !(this.txbMonthCount.Text != "") ? 1 : Convert.ToInt32(this.txbMonthCount.Text);
          this.SchemeId = (int) ((Scheme) this.cmbShema.SelectedItem).SchemeId;
          this.performerId = ((BaseOrg) this.cmbPerformer.SelectedItem).BaseOrgId;
          this.recipientId = ((BaseOrg) this.cmbRecipient.SelectedItem).BaseOrgId;
        }
        this.LsClientList = (IList<LsClient>) new List<LsClient>();
        foreach (string checkedItem in this.chlbObject.CheckedItems)
        {
          string sidlic = checkedItem.Remove(checkedItem.IndexOf(" "), checkedItem.Length - checkedItem.IndexOf(" "));
          numArray[index1] = Convert.ToInt32(sidlic);
          ++index1;
          if (this.workDistribute != null)
          {
            LsClient lsClient2 = new LsClient();
            this.LsClientList.Add(this.session.Get<LsClient>((object) Convert.ToInt32(sidlic)));
          }
          if (Convert.ToInt32(sidlic) != Convert.ToInt32(this.ClientId))
          {
            if (this.ClientParam != null)
              this.MakeActionLsParam(sidlic);
            if (this.currentQuality != null)
            {
              if (!KvrplHelper.CheckProxy(43, 2, cmp, true))
                return;
              this.MakeActionLsQuality(sidlic);
            }
            if (this.lsService != null)
            {
              if (!KvrplHelper.CheckProxy(40, 2, cmp, true))
                return;
              this.MakeActionLsService(sidlic);
            }
            if (this.lsServiceParam != null)
            {
              if (!KvrplHelper.CheckProxy(40, 2, cmp, true))
                return;
              this.MakeActionLsServiceParam(sidlic);
            }
            if (this.lsSupplier != null)
            {
              if (!KvrplHelper.CheckProxy(40, 2, cmp, true))
                return;
              this.MakeActionLsSupplier(sidlic);
            }
            if (this.rent != null)
            {
              if (!KvrplHelper.CheckProxy(40, 2, cmp, true))
                return;
              this.MakeActionRent(sidlic);
            }
            if (this.cntrRelation != null)
            {
              if (!KvrplHelper.CheckProxy(40, 2, cmp, true))
                return;
              this.MakeActionCounterRelation(sidlic);
            }
            if (this.counter != null)
            {
              if (!KvrplHelper.CheckProxy(40, 2, cmp, true))
                return;
              if (!this.MakeActionCounter(sidlic))
              {
                str1 = str1 + sidlic + "  ";
                flag = true;
              }
            }
            if (this.hmPhones != null)
            {
              if (!KvrplHelper.CheckProxy(40, 2, cmp, true))
                return;
              if (!this.MakeActionHmPhones(sidlic))
              {
                str1 = str1 + sidlic + "  ";
                flag = true;
              }
            }
            if (this.noteBook != null)
            {
              if (!KvrplHelper.CheckProxy(38, 2, cmp, true))
                return;
              if (!this.MakeActionNoteBook(sidlic))
              {
                str1 = str1 + sidlic + "  ";
                flag = true;
              }
            }
            if (this.listCorrectRent != null && !this.MakeActionCorrectRent(sidlic))
            {
              str1 = str1 + sidlic + "  ";
              flag = true;
            }
          }
        }
      }
      if (this.rbtnFlat.Checked)
      {
        str1 = this.SaveFlat();
        flag = str1 != "";
      }
      if (this.rbtnFloor.Checked)
      {
        str1 = this.SaveFloor();
        flag = str1 != "";
      }
      if (this.rbtnEntrance.Checked)
      {
        str1 = this.SaveEntrance();
        flag = str1 != "";
      }
      if (this.rbtnHome.Checked)
        this.SaveHome();
      if (this.rbtnCompany.Checked)
      {
        if (this.currentParam != null)
        {
          if (this.Save)
          {
            foreach (Company checkedItem in this.chlbObject.CheckedItems)
            {
              this.session = Domain.CurrentSession;
              this.session.Save((object) new CompanyParam()
              {
                Company = checkedItem,
                DBeg = this.currentParam.DBeg,
                DEnd = this.currentParam.DEnd,
                Param = this.currentParam.Param,
                ParamValue = this.currentParam.ParamValue,
                Period = this.currentParam.Period,
                UName = Options.Login,
                DEdit = DateTime.Now
              });
              try
              {
                this.session.Flush();
              }
              catch (Exception ex)
              {
                flag = true;
              }
              this.session.Clear();
            }
          }
          else
          {
            foreach (Company checkedItem in this.chlbObject.CheckedItems)
            {
              this.session = Domain.CurrentSession;
              CompanyParam companyParam = new CompanyParam();
              companyParam.Company = checkedItem;
              companyParam.DBeg = this.currentParam.DBeg;
              companyParam.DEnd = this.currentParam.DEnd;
              companyParam.Param = this.currentParam.Param;
              companyParam.ParamValue = this.currentParam.ParamValue;
              companyParam.Period = this.currentParam.Period;
              companyParam.UName = Options.Login;
              companyParam.DEdit = DateTime.Now;
              try
              {
                this.session.Delete((object) companyParam);
                this.session.Flush();
              }
              catch (Exception ex)
              {
                KvrplHelper.WriteLog(ex, (LsClient) null);
                flag = true;
              }
              this.session.Clear();
            }
          }
        }
        if (this.currentParam == null && this.home == null && this.hmPhones != null)
        {
          if (this.Save)
          {
            foreach (Company checkedItem in this.chlbObject.CheckedItems)
            {
              this.session = Domain.CurrentSession;
              this.session.Save((object) new HomesPhones()
              {
                Company = checkedItem,
                DBeg = this.hmPhones.DBeg,
                DEnd = this.hmPhones.DEnd,
                Note = this.hmPhones.Note,
                Phone = this.hmPhones.Phone,
                PhonesServ = this.hmPhones.PhonesServ,
                Receipt = this.hmPhones.Receipt,
                ClientId = this.hmPhones.ClientId,
                UName = Options.Login,
                DEdit = DateTime.Now,
                Home = this.hmPhones.Home
              });
              try
              {
                this.session.Flush();
              }
              catch (Exception ex)
              {
                flag = true;
              }
              this.session.Clear();
            }
          }
          else
          {
            foreach (Company checkedItem in this.chlbObject.CheckedItems)
            {
              this.session = Domain.CurrentSession;
              HomesPhones homesPhones = new HomesPhones();
              homesPhones.Company = checkedItem;
              homesPhones.DBeg = this.hmPhones.DBeg;
              homesPhones.DEnd = this.hmPhones.DEnd;
              homesPhones.Note = this.hmPhones.Note;
              homesPhones.Phone = this.hmPhones.Phone;
              homesPhones.PhonesServ = this.hmPhones.PhonesServ;
              homesPhones.Receipt = this.hmPhones.Receipt;
              homesPhones.ClientId = this.hmPhones.ClientId;
              homesPhones.UName = Options.Login;
              homesPhones.DEdit = DateTime.Now;
              homesPhones.Home = this.hmPhones.Home;
              try
              {
                this.session.Delete((object) homesPhones);
                this.session.Flush();
              }
              catch (Exception ex)
              {
                KvrplHelper.WriteLog(ex, (LsClient) null);
                flag = true;
              }
              this.session.Clear();
            }
          }
        }
      }
      if (!flag)
      {
        int num1 = (int) MessageBox.Show("Операция завершена", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        DialogResult dialogResult = str1 != "" ? MessageBox.Show(string.Format("Возникли ошибки на следующих лицевых счетах {0}", (object) str1), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk) : MessageBox.Show("Операция выполнена с ошибками", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private string SaveFloor()
    {
      if (!(this.tbBegin.Text != ""))
        return "";
      int int32 = Convert.ToInt32(this.tbBegin.Text);
      int num1 = this.tbEnd.Text != "" ? Convert.ToInt32(this.tbEnd.Text) : 0;
      int num2 = int32;
      string str = "";
      do
      {
        foreach (LsClient lsClient in (IEnumerable<LsClient>) this.GetListLsClient(string.Format(" and ls.Floor={0} ", (object) num2)))
        {
          if (this.ClientId != lsClient.ClientId)
          {
            if (this.ClientParam != null)
              this.MakeActionLsParam(lsClient.ClientId.ToString());
            if (this.lsService != null)
              this.MakeActionLsService(lsClient.ClientId.ToString());
            if (this.lsServiceParam != null)
              this.MakeActionLsServiceParam(lsClient.ClientId.ToString());
            if (this.lsSupplier != null)
              this.MakeActionLsSupplier(lsClient.ClientId.ToString());
            if (this.currentQuality != null)
              this.MakeActionLsQuality(lsClient.ClientId.ToString());
            if (this.cntrRelation != null)
              this.MakeActionCounterRelation(lsClient.ClientId.ToString());
            if (this.counter != null && !this.MakeActionCounter(lsClient.ClientId.ToString()))
              str = str + lsClient.ClientId.ToString() + "  ";
            if (this.hmPhones != null && !this.MakeActionHmPhones(lsClient.ClientId.ToString()))
              str = str + lsClient.ClientId.ToString() + "  ";
            if (this.noteBook != null && !this.MakeActionNoteBook(lsClient.ClientId.ToString()))
              str = str + lsClient.ClientId.ToString() + "  ";
          }
        }
        ++num2;
      }
      while (num2 != num1 + 1 && (uint) num1 > 0U);
      return str;
    }

    private void SaveHome()
    {
      if ((int) this.HomeSave == 2)
      {
        foreach (Home checkedItem in this.chlbObject.CheckedItems)
        {
          checkedItem.Division = this.home.Division;
          try
          {
            this.session.Update((object) checkedItem);
            this.session.Flush();
          }
          catch (Exception ex)
          {
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
        }
      }
      if ((int) this.HomeSave == 1)
      {
        foreach (Home checkedItem in this.chlbObject.CheckedItems)
          this.MakeActionHmParam(checkedItem);
      }
      if ((int) this.HomeSave != 3)
        return;
      foreach (Home checkedItem in this.chlbObject.CheckedItems)
      {
        if (this.Save)
          this.SaveHmPhones(checkedItem);
        else
          this.DeleteHmPhones(checkedItem);
      }
    }

    private bool DeleteHmPhones(Home hm)
    {
      this.session.Clear();
      HomesPhones homesPhones = new HomesPhones();
      homesPhones.Home = hm;
      homesPhones.Company = hm.Company;
      homesPhones.PhonesServ = this.hmPhones.PhonesServ;
      homesPhones.Phone = this.hmPhones.Phone;
      homesPhones.Note = this.hmPhones.Note;
      homesPhones.DBeg = this.hmPhones.DBeg;
      homesPhones.DEnd = this.hmPhones.DEnd;
      homesPhones.Receipt = this.session.Get<Receipt>((object) this.hmPhones.Receipt.ReceiptId);
      try
      {
        this.session.Delete((object) homesPhones);
        this.session.Flush();
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
    }

    private bool SaveHmPhones(Home hm)
    {
      this.session.Clear();
      HomesPhones homesPhones = new HomesPhones();
      homesPhones.Home = hm;
      homesPhones.Company = hm.Company;
      homesPhones.PhonesServ = this.hmPhones.PhonesServ;
      homesPhones.Phone = this.hmPhones.Phone;
      homesPhones.Note = this.hmPhones.Note;
      homesPhones.DBeg = this.hmPhones.DBeg;
      homesPhones.DEdit = DateTime.Now;
      homesPhones.DEnd = this.hmPhones.DEnd;
      homesPhones.UName = Options.Login;
      homesPhones.Receipt = this.session.Get<Receipt>((object) this.hmPhones.Receipt.ReceiptId);
      try
      {
        this.session.Save((object) homesPhones);
        this.session.Flush();
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
    }

    private bool SaveHmPhones(string sidlic)
    {
      this.session.Clear();
      HomesPhones homesPhones = new HomesPhones();
      homesPhones.ClientId = Convert.ToInt32(sidlic);
      homesPhones.Home = this.session.Get<Home>((object) this.hmPhones.Home.IdHome);
      homesPhones.Company = this.session.Get<Company>((object) this.hmPhones.Company.CompanyId);
      homesPhones.PhonesServ = this.hmPhones.PhonesServ;
      homesPhones.Phone = this.hmPhones.Phone;
      homesPhones.Note = this.hmPhones.Note;
      homesPhones.DBeg = this.hmPhones.DBeg;
      homesPhones.DEdit = DateTime.Now;
      homesPhones.DEnd = this.hmPhones.DEnd;
      homesPhones.UName = Options.Login;
      homesPhones.Receipt = this.session.Get<Receipt>((object) this.hmPhones.Receipt.ReceiptId);
      this.session.Clear();
      try
      {
        this.session.Save((object) homesPhones);
        this.session.Flush();
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
    }

    private bool DeleteHmPhones(string sidlic)
    {
      this.session.Clear();
      HomesPhones homesPhones = new HomesPhones();
      homesPhones.ClientId = Convert.ToInt32(sidlic);
      homesPhones.Home = this.session.Get<Home>((object) this.hmPhones.Home.IdHome);
      homesPhones.Company = this.session.Get<Company>((object) this.hmPhones.Company.CompanyId);
      homesPhones.PhonesServ = this.hmPhones.PhonesServ;
      homesPhones.Phone = this.hmPhones.Phone;
      homesPhones.Note = this.hmPhones.Note;
      homesPhones.DBeg = this.hmPhones.DBeg;
      homesPhones.DEnd = this.hmPhones.DEnd;
      homesPhones.Receipt = this.session.Get<Receipt>((object) this.hmPhones.Receipt.ReceiptId);
      this.session.Clear();
      try
      {
        this.session.CreateSQLQuery("delete from DBA.hmReceipt where  client_id=:client_id and idhome=:idhome and idservice=:service_id and dBeg=:dbeg  and company_id=:company_id and dEnd=:dend and receipt_id=:receipt").SetParameter<int>("client_id", homesPhones.ClientId).SetParameter<int>("idhome", homesPhones.Home.IdHome).SetParameter<int>("service_id", homesPhones.PhonesServ.Idservice).SetParameter<DateTime>("dbeg", homesPhones.DBeg).SetParameter<short>("company_id", homesPhones.Company.CompanyId).SetParameter<DateTime>("dend", homesPhones.DEnd).SetParameter<short>("receipt", homesPhones.Receipt.ReceiptId).ExecuteUpdate();
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
    }

    private bool UpdateHmPhones(string sidlic)
    {
      this.session.Clear();
      HomesPhones homesPhones = new HomesPhones();
      homesPhones.ClientId = Convert.ToInt32(sidlic);
      homesPhones.Home = this.session.Get<Home>((object) this.hmPhones.Home.IdHome);
      homesPhones.Company = this.session.Get<Company>((object) this.hmPhones.Company.CompanyId);
      homesPhones.PhonesServ = this.hmPhones.PhonesServ;
      homesPhones.Phone = this.hmPhones.Phone;
      homesPhones.Note = this.hmPhones.Note;
      homesPhones.DBeg = this.hmPhones.DBeg;
      homesPhones.DEdit = DateTime.Now;
      homesPhones.DEnd = this.hmPhones.DEnd;
      homesPhones.UName = Options.Login;
      homesPhones.Receipt = this.session.Get<Receipt>((object) this.hmPhones.Receipt.ReceiptId);
      this.session.Clear();
      try
      {
        this.session.CreateSQLQuery("update DBA.hmReceipt cmp set DEnd=:dend,phone=:phone, note=:note, Uname=:uname, Dedit=:dedit where cmp.Client_Id=:clientid and cmp.IdHome=:idhome and cmp.Company_id=:company_id  and cmp.IdService = :service_id  and cmp.Dbeg = :olddbeg and cmp.Receipt_id=:receipt ").SetParameter<DateTime>("dend", homesPhones.DEnd.Date).SetParameter<string>("phone", homesPhones.Phone).SetParameter<string>("note", homesPhones.Note).SetParameter<string>("uname", Options.Login).SetParameter<DateTime>("dedit", DateTime.Now).SetParameter<int>("clientid", homesPhones.ClientId).SetParameter<int>("idhome", homesPhones.Home.IdHome).SetParameter<short>("company_id", homesPhones.Company.CompanyId).SetParameter<int>("service_id", homesPhones.PhonesServ.Idservice).SetParameter<DateTime>("olddbeg", homesPhones.DBeg.Date).SetParameter<short>("receipt", homesPhones.Receipt.ReceiptId).ExecuteUpdate();
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
    }

    private string SaveFlat()
    {
      if (!(this.tbBegin.Text != ""))
        return "";
      int int32 = Convert.ToInt32(this.tbBegin.Text);
      int num1 = this.tbEnd.Text != "" ? Convert.ToInt32(this.tbEnd.Text) : 0;
      int num2 = int32;
      string str = "";
      do
      {
        this.session.Get<LsClient>((object) this.ClientId);
        foreach (LsClient lsClient in (IEnumerable<LsClient>) this.GetListLsClient(string.Format(" and ls.Flat.NFlat='{0}' ", (object) num2)))
        {
          if (this.ClientId != lsClient.ClientId)
          {
            if (this.ClientParam != null)
              this.MakeActionLsParam(lsClient.ClientId.ToString());
            if (this.lsService != null)
              this.MakeActionLsService(lsClient.ClientId.ToString());
            if (this.lsServiceParam != null)
              this.MakeActionLsServiceParam(lsClient.ClientId.ToString());
            if (this.lsSupplier != null)
              this.MakeActionLsSupplier(lsClient.ClientId.ToString());
            if (this.currentQuality != null)
              this.MakeActionLsQuality(lsClient.ClientId.ToString());
            if (this.cntrRelation != null)
              this.MakeActionCounterRelation(lsClient.ClientId.ToString());
            if (this.counter != null && !this.MakeActionCounter(lsClient.ClientId.ToString()))
              str = str + lsClient.ClientId.ToString() + "  ";
            if (this.hmPhones != null && !this.MakeActionHmPhones(lsClient.ClientId.ToString()))
              str = str + lsClient.ClientId.ToString() + "  ";
            if (this.noteBook != null && !this.MakeActionNoteBook(lsClient.ClientId.ToString()))
              str = str + lsClient.ClientId.ToString() + "  ";
          }
        }
        ++num2;
      }
      while (num2 != num1 + 1 && (uint) num1 > 0U);
      return str;
    }

    private string SaveEntrance()
    {
      int int32 = Convert.ToInt32(this.tbBegin.Text);
      int num1 = this.tbEnd.Text != "" ? Convert.ToInt32(this.tbEnd.Text) : 0;
      int num2 = int32;
      string str1 = "";
      do
      {
        this.session.Get<LsClient>((object) this.ClientId);
        foreach (LsClient lsClient in (IEnumerable<LsClient>) this.GetListLsClient(string.Format(" and ls.Entrance={0} ", (object) num2)))
        {
          if (this.ClientId != lsClient.ClientId)
          {
            if (this.ClientParam != null)
              this.MakeActionLsParam(lsClient.ClientId.ToString());
            if (this.lsService != null)
              this.MakeActionLsService(lsClient.ClientId.ToString());
            if (this.lsServiceParam != null)
              this.MakeActionLsServiceParam(lsClient.ClientId.ToString());
            if (this.lsSupplier != null)
              this.MakeActionLsSupplier(lsClient.ClientId.ToString());
            if (this.currentQuality != null)
              this.MakeActionLsQuality(lsClient.ClientId.ToString());
            if (this.cntrRelation != null)
              this.MakeActionCounterRelation(lsClient.ClientId.ToString());
            if (this.counter != null)
            {
              int clientId = lsClient.ClientId;
              if (!this.MakeActionCounter(clientId.ToString()))
              {
                string str2 = str1;
                clientId = lsClient.ClientId;
                string str3 = clientId.ToString();
                string str4 = "  ";
                str1 = str2 + str3 + str4;
              }
            }
            if (this.hmPhones != null)
            {
              int clientId = lsClient.ClientId;
              if (!this.MakeActionHmPhones(clientId.ToString()))
              {
                string str2 = str1;
                clientId = lsClient.ClientId;
                string str3 = clientId.ToString();
                string str4 = "  ";
                str1 = str2 + str3 + str4;
              }
            }
            if (this.noteBook != null)
            {
              int clientId = lsClient.ClientId;
              if (!this.MakeActionNoteBook(clientId.ToString()))
              {
                string str2 = str1;
                clientId = lsClient.ClientId;
                string str3 = clientId.ToString();
                string str4 = "  ";
                str1 = str2 + str3 + str4;
              }
            }
          }
        }
        ++num2;
      }
      while (num2 != num1 + 1 && (uint) num1 > 0U);
      return str1;
    }

    private void btnCheck_Click(object sender, EventArgs e)
    {
      if (this.checkAll)
      {
        KvrplHelper.CheckAll(this.chlbObject);
        this.btnCheck.Text = "Снять все";
        this.checkAll = false;
      }
      else
      {
        KvrplHelper.UnCheckAll(this.chlbObject);
        this.btnCheck.Text = "Выделить все";
        this.checkAll = true;
      }
    }

    private void groupBox1_Enter(object sender, EventArgs e)
    {
    }

    private void radioButton2_CheckedChanged(object sender, EventArgs e)
    {
      this.chlbObject.Items.Clear();
      this.EnableFlatTextBox();
      this.tbBegin.Focus();
    }

    private void EnableFlatTextBox()
    {
      this.tbBegin.Text = "";
      this.tbEnd.Text = "";
      this.tbBegin.Visible = true;
      this.tbEnd.Visible = true;
      this.lblTxt.Visible = true;
    }

    private void DisableFlatTextBox()
    {
      this.tbBegin.Text = "";
      this.tbEnd.Text = "";
      this.tbBegin.Visible = false;
      this.tbEnd.Visible = false;
      this.lblTxt.Visible = false;
    }

    private bool SaveLsService(string sidlic)
    {
      this.session.Clear();
      LsService service = new LsService();
      service.Client = this.session.Get<LsClient>((object) Convert.ToInt32(sidlic));
      this.session.Clear();
      service.Complex = this.session.Get<Complex>((object) 100);
      service.DBeg = this.lsService.DBeg;
      service.DEnd = this.lsService.DEnd;
      service.Dedit = DateTime.Now;
      service.Norm = this.lsService.Norm;
      service.Period = this.session.Get<Period>((object) this.lsService.Period.PeriodId);
      service.Service = this.lsService.Service;
      service.Tariff = this.lsService.Tariff;
      service.Uname = Options.Login;
      try
      {
        this.session.Save((object) service);
        this.session.Flush();
        if ((uint) service.Period.PeriodId > 0U)
        {
          if (Convert.ToInt32(KvrplHelper.BaseValue(32, service.Client.Company)) == 1)
          {
            if (this.city == 28 && this.argument != "" || this.city != 28)
              KvrplHelper.SaveServiceToNoteBook(service, (short) 1, this.argument, true, this.MonthClosed.PeriodName.Value);
          }
        }
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
      return true;
    }

    private bool SaveLsServiceParam(string sidlic)
    {
      this.session.Clear();
      LsServiceParam lsServiceParam = new LsServiceParam();
      lsServiceParam.LsClient = this.session.Get<LsClient>((object) Convert.ToInt32(sidlic));
      this.session.Clear();
      lsServiceParam.DBeg = this.lsServiceParam.DBeg;
      lsServiceParam.DEnd = this.lsServiceParam.DEnd;
      lsServiceParam.DEdit = DateTime.Now;
      lsServiceParam.Period = this.session.Get<Period>((object) this.lsServiceParam.Period.PeriodId);
      lsServiceParam.Service = this.lsServiceParam.Service;
      lsServiceParam.UName = Options.Login;
      lsServiceParam.Param = this.lsServiceParam.Param;
      lsServiceParam.ParamValue = this.lsServiceParam.ParamValue;
      try
      {
        this.session.Save((object) lsServiceParam);
        this.session.Flush();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
      return true;
    }

    private bool SaveLsSupplier(string sidlic)
    {
      this.session.Clear();
      LsSupplier supplier = new LsSupplier();
      supplier.LsClient = this.session.Get<LsClient>((object) Convert.ToInt32(sidlic));
      this.session.Clear();
      supplier.DBeg = this.lsSupplier.DBeg;
      supplier.DEnd = this.lsSupplier.DEnd;
      supplier.Dedit = DateTime.Now;
      supplier.Uname = Options.Login;
      supplier.Period = this.lsSupplier.Period;
      supplier.Service = this.lsSupplier.Service;
      supplier.Supplier = this.lsSupplier.Supplier;
      try
      {
        this.session.Save((object) supplier);
        this.session.Flush();
        if ((uint) this.lsSupplier.Period.PeriodId > 0U)
        {
          if (Convert.ToInt32(KvrplHelper.BaseValue(32, supplier.LsClient.Company)) == 1)
          {
            if (this.city == 28 && this.argument != "" || this.city != 28)
              KvrplHelper.SaveSupplierToNoteBook(supplier, supplier.LsClient, (short) 1, this.argument, true, this.MonthClosed.PeriodName.Value);
          }
        }
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
      return true;
    }

    private bool SaveRent(string sidlic)
    {
      this.session.Clear();
      Rent rent = new Rent();
      rent.LsClient = this.session.Get<LsClient>((object) Convert.ToInt32(sidlic));
      this.session.Clear();
      rent.Month = this.rent.Month;
      rent.Code = this.rent.Code;
      rent.Motive = 1;
      rent.Volume = this.rent.Volume;
      rent.Period = this.rent.Period;
      rent.Service = this.rent.Service;
      rent.Supplier = this.rent.Supplier;
      rent.RentEO = 0.0;
      rent.RentMain = this.rent.RentMain;
      try
      {
        this.session.Save((object) rent);
        this.session.Flush();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
      return true;
    }

    private bool SaveLsQuality(string sidlic)
    {
      this.session.Clear();
      LsQuality quality = new LsQuality();
      quality.LsClient = this.session.Get<LsClient>((object) Convert.ToInt32(sidlic));
      this.session.Clear();
      quality.DBeg = this.currentQuality.DBeg;
      quality.DEnd = this.currentQuality.DEnd;
      quality.Period = this.currentQuality.Period;
      quality.Quality = this.currentQuality.Quality;
      quality.UName = Options.Login;
      quality.DEdit = DateTime.Now;
      try
      {
        this.session.Save((object) quality);
        this.session.Flush();
        if (Convert.ToInt32(KvrplHelper.BaseValue(32, quality.LsClient.Company)) == 1 && (this.city == 28 && this.argument != "" || this.city != 28))
          KvrplHelper.SaveQualityToNoteBook(quality, (LsQuality) null, quality.LsClient, (short) 1, this.MonthClosed.PeriodName.Value);
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
    }

    private bool SaveLsParam(string sidlic)
    {
      this.session.Clear();
      ClientParam clientParam = new ClientParam();
      LsClient lsClient = this.session.Get<LsClient>((object) Convert.ToInt32(sidlic));
      clientParam.ClientId = lsClient.ClientId;
      this.session.Clear();
      clientParam.DBeg = this.ClientParam.DBeg;
      clientParam.Dedit = DateTime.Now.Date;
      clientParam.DEnd = this.ClientParam.DEnd;
      clientParam.Param = this.ClientParam.Param;
      clientParam.ParamValue = this.ClientParam.ParamValue;
      clientParam.Period = this.ClientParam.Period;
      clientParam.Uname = Options.Login;
      try
      {
        this.session.Save((object) clientParam);
        this.session.Flush();
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
    }

    private bool SaveCounterRelation(string sidlic)
    {
      this.session.Clear();
      CounterRelation counterRelation = new CounterRelation();
      counterRelation.LsClient = this.session.Get<LsClient>((object) Convert.ToInt32(sidlic));
      this.session.Clear();
      counterRelation.DBeg = this.cntrRelation.DBeg;
      counterRelation.DEnd = this.cntrRelation.DEnd;
      counterRelation.Period = this.cntrRelation.Period;
      counterRelation.DEdit = DateTime.Now.Date;
      counterRelation.Counter = this.cntrRelation.Counter;
      counterRelation.OnOff = this.cntrRelation.OnOff;
      counterRelation.UName = Options.Login;
      try
      {
        this.session.Save((object) counterRelation);
        this.session.Flush();
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
    }

    private bool SaveCounter(string sidlic)
    {
      this.session.Clear();
      Counter counter = new Counter();
      counter.LsClient = this.session.Get<LsClient>((object) Convert.ToInt32(sidlic));
      this.session.Clear();
      counter.ArchivesDate = this.counter.ArchivesDate;
      counter.AuditDate = this.counter.AuditDate;
      counter.BaseCounter = this.session.Get<BaseCounter>((object) this.counter.BaseCounter.Id);
      counter.Company = this.session.Get<Company>((object) this.counter.Company.CompanyId);
      counter.Complex = this.session.Get<Complex>((object) this.counter.Complex.IdFk);
      counter.CounterNum = this.counter.CounterNum;
      counter.DEdit = DateTime.Now.Date;
      counter.EvidenceStart = this.counter.EvidenceStart;
      counter.Home = this.session.Get<Home>((object) this.counter.Home.IdHome);
      counter.Notice = this.counter.Notice;
      counter.RemoveDate = this.counter.RemoveDate;
      counter.Service = this.session.Get<Service>((object) this.counter.Service.ServiceId);
      counter.Series = this.counter.Series;
      counter.SetDate = this.counter.SetDate;
      counter.TypeCounter = this.counter.TypeCounter != null ? this.session.Get<TypeCounter>((object) this.counter.TypeCounter.TypeCounter_id) : (TypeCounter) null;
      counter.UName = Options.Login;
      counter.CoeffTrans = this.counter.CoeffTrans;
      if (this.session.CreateQuery(string.Format("from Counter c where c.LsClient.ClientId = {0} and c.Service.ServiceId = {1} and c.CounterNum={2}", (object) counter.LsClient.ClientId, (object) counter.Service.ServiceId, (object) counter.CounterNum)).List<Counter>().Count > 0)
        return false;
      IList<int> intList = this.session.CreateSQLQuery("select DBA.gen_id('cntrCounter',1)").List<int>();
      counter.CounterId = intList[0];
      try
      {
        this.session.Save((object) counter);
        this.session.Flush();
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
    }

    private bool SaveNoteBook(string sidlic)
    {
      this.session.Clear();
      NoteBook noteBook = new NoteBook();
      noteBook.ClientId = Convert.ToInt32(sidlic);
      this.session.Clear();
      short int16 = Convert.ToInt16(this.session.CreateQuery(string.Format("select max(NoteId) from NoteBook where ClientId={0}", (object) Convert.ToInt32(sidlic))).UniqueResult());
      noteBook.NoteId = (int) Convert.ToInt16((int) int16 + 1);
      noteBook.Company = this.noteBook.Company;
      noteBook.IdHome = this.noteBook.IdHome;
      noteBook.Note = this.noteBook.Note;
      noteBook.DBeg = DateTime.Now;
      noteBook.DEnd = Convert.ToDateTime("31.12.2999");
      noteBook.Text = this.noteBook.Text;
      noteBook.UName = Options.Login;
      noteBook.DEdit = DateTime.Now;
      noteBook.TypeNoteBook = this.session.Get<TypeNoteBook>((object) 1);
      try
      {
        this.session.Save((object) noteBook);
        this.session.Flush();
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
    }

    private bool SaveCorrectRent(string sidlic)
    {
      try
      {
        this.session.Clear();
        double num1 = 0.0;
        double num2 = 0.0;
        double num3 = 0.0;
        LsClient lsClient = this.session.Get<LsClient>((object) Convert.ToInt32(sidlic));
        foreach (CorrectRent correctRent in (IEnumerable<CorrectRent>) this.listCorrectRent)
        {
          num3 += correctRent.RentMain;
          num1 += correctRent.Volume;
          num2 += correctRent.RentEO;
        }
        IList<Rent> rentList = this.session.CreateQuery(string.Format("select r from Rent r,Service s where r.Period.PeriodId={0} and r.LsClient.ClientId = {1} and r.Service=s and s.Root={2} and r.Code=0", (object) this.month.PeriodId, (object) lsClient.ClientId, (object) this.serviceId)).List<Rent>();
        if (rentList.Count == 0)
        {
          ISession session1 = this.session;
          string format1 = "select ct from LsService ls,cmpTariffCost ct where ls.Client.ClientId={0} and ls.Period.PeriodId=0 and ls.Service.ServiceId={1} and ls.DBeg<='{2}' and ls.DEnd>='{3}'and ct.Company_id=(select ParamValue from CompanyParam where Company.CompanyId={4} and Period.PeriodId=0 and DBeg<='{2}' and DEnd>='{3}' and Param.ParamId=201) and ct.Period.PeriodId=0 and ct.Tariff_id=ls.Tariff.Tariff_id and ct.Dbeg<='{2}' and ct.Dend>='{3}'and ct.Service.ServiceId in (select ServiceId from Service where Root={1}) and Cost<>0";
          object[] objArray1 = new object[5]{ (object) sidlic, (object) this.serviceId, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(this.month.PeriodName.Value)), null, null };
          int index1 = 3;
          DateTime? periodName = this.month.PeriodName;
          string baseFormat1 = KvrplHelper.DateToBaseFormat(periodName.Value);
          objArray1[index1] = (object) baseFormat1;
          int index2 = 4;
          // ISSUE: variable of a boxed type
          short companyId = this.company.CompanyId;
          objArray1[index2] = (object) companyId;
          string queryString1 = string.Format(format1, objArray1);
          IList<cmpTariffCost> cmpTariffCostList = session1.CreateQuery(queryString1).List<cmpTariffCost>();
          if (cmpTariffCostList.Count > 1 || cmpTariffCostList.Count == 0)
            return false;
          ISession session2 = this.session;
          string format2 = "from LsSupplier l where l.LsClient.ClientId={0} and l.Period.PeriodId=0 and l.Service.ServiceId={1} and l.DBeg<='{2}' and l.DEnd>='{3}'";
          object[] objArray2 = new object[4]{ (object) lsClient.ClientId, (object) cmpTariffCostList[0].Service.ServiceId, null, null };
          int index3 = 2;
          periodName = this.month.PeriodName;
          string baseFormat2 = KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(periodName.Value));
          objArray2[index3] = (object) baseFormat2;
          int index4 = 3;
          periodName = this.month.PeriodName;
          string baseFormat3 = KvrplHelper.DateToBaseFormat(periodName.Value);
          objArray2[index4] = (object) baseFormat3;
          string queryString2 = string.Format(format2, objArray2);
          IList<LsSupplier> lsSupplierList = session2.CreateQuery(queryString2).List<LsSupplier>();
          if (lsSupplierList.Count > 1)
            return false;
          try
          {
            this.session.Save((object) new CorrectRent()
            {
              Period = Options.Period,
              LsClient = lsClient,
              Service = cmpTariffCostList[0].Service,
              Supplier = lsSupplierList[0].Supplier,
              Month = this.month,
              Note = this.listCorrectRent[0].Note,
              Volume = num1,
              RentEO = num2,
              RentMain = num3,
              RentType = this.listCorrectRent[0].RentType,
              UName = Options.Login,
              DEdit = new DateTime?(DateTime.Now.Date)
            });
            this.session.Flush();
          }
          catch (Exception ex)
          {
            return false;
          }
        }
        else
        {
          double num4 = Convert.ToDouble(this.session.CreateQuery(string.Format("select sum(r.RentMain) from Rent r,Service s where r.Period.PeriodId={0} and r.LsClient.ClientId = {1} and r.Service=s and s.Root={2} and r.Code=0", (object) this.month.PeriodId, (object) sidlic, (object) this.serviceId)).List()[0]);
          ITransaction transaction = this.session.BeginTransaction();
          foreach (Rent rent in (IEnumerable<Rent>) rentList)
          {
            try
            {
              this.session.Save((object) new CorrectRent()
              {
                Period = Options.Period,
                LsClient = lsClient,
                Service = rent.Service,
                Supplier = rent.Supplier,
                Month = this.month,
                Note = this.listCorrectRent[0].Note,
                Volume = (rent.RentMain * num1 / num4),
                RentEO = (rent.RentEO * num2 / num4),
                RentMain = (rent.RentMain * num3 / num4),
                RentType = this.listCorrectRent[0].RentType,
                UName = Options.Login,
                DEdit = new DateTime?(DateTime.Now.Date)
              });
              this.session.Flush();
            }
            catch (Exception ex)
            {
              return false;
            }
          }
          IList list1 = (IList) new ArrayList();
          IList list2 = this.session.CreateQuery(string.Format("select sum(r.RentMain),sum(r.Volume) from CorrectRent r,Service s where r.Period.PeriodId={0} and r.LsClient.ClientId = {1} and r.Service=s and r.Month.PeriodId={4} and s.Root={2} and r.Note='{5}'", (object) Options.Period.PeriodId, (object) lsClient.ClientId, (object) this.serviceId, (object) 4, (object) this.month.PeriodId, (object) this.argument)).List();
          double num5 = Convert.ToDouble(KvrplHelper.ChangeSeparator(((object[]) list2[0])[0].ToString()));
          double num6 = Convert.ToDouble(KvrplHelper.ChangeSeparator(((object[]) list2[0])[1].ToString()));
          if (num5 != num3 || num1 != num6)
          {
            Rent rent = rentList[0];
            try
            {
              this.session.CreateQuery(string.Format("update CorrectRent r set r.RentMain=r.RentMain+:rent,r.Volume=r.Volume+:volume,r.RentType=:type  where r.Period.PeriodId={0} and r.LsClient.ClientId={1} and r.Service.ServiceId={2} and r.Supplier.BaseOrgId={3}  and r.Month.PeriodId={4} and r.Note='{6}'", (object) Options.Period.PeriodId, (object) lsClient.ClientId, (object) rent.Service.ServiceId, (object) rent.Supplier.SupplierId, (object) this.month.PeriodId, (object) 4, (object) this.argument)).SetParameter<double>("rent", num3 - num5).SetParameter<double>("volume", num1 - num6).SetParameter<short>("type", rent.RentType).ExecuteUpdate();
            }
            catch (Exception ex)
            {
              KvrplHelper.WriteLog(ex, (LsClient) null);
              return false;
            }
          }
          transaction.Commit();
          if (Convert.ToInt32(KvrplHelper.BaseValue(32, this.company)) == 1)
          {
            CorrectRent rent = new CorrectRent();
            rent.LsClient = lsClient;
            rent.Period = Options.Period;
            rent.Service = this.session.Get<Service>((object) Convert.ToInt16(this.serviceId));
            rent.Month = this.month;
            rent.Note = this.listCorrectRent[0].Note;
            rent.Volume = num1;
            rent.RentMain = num3;
            rent.RentType = this.listCorrectRent[0].RentType;
            KvrplHelper.SaveCorrectRentToNoteBook(rent, rent.LsClient, (short) 1, KvrplHelper.GetKvrClose(rent.LsClient.ClientId, Options.ComplexPasp, Options.ComplexPrior).PeriodName.Value);
          }
        }
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        return false;
      }
    }

    private bool DeleteLsService(string sidlic)
    {
      LsService service = new LsService();
      service.Client = this.session.Get<LsClient>((object) Convert.ToInt32(sidlic));
      this.session.Clear();
      service.Complex = this.lsService.Complex;
      service.DBeg = this.lsService.DBeg;
      service.DEnd = this.lsService.DEnd;
      service.Norm = this.lsService.Norm;
      service.Period = this.lsService.Period;
      service.Service = this.lsService.Service;
      service.Tariff = this.lsService.Tariff;
      try
      {
        this.session.Delete((object) service);
        this.session.Flush();
        if ((uint) service.Period.PeriodId > 0U)
        {
          if (Convert.ToInt32(KvrplHelper.BaseValue(32, service.Client.Company)) == 1)
            KvrplHelper.DeleteServiceFromNoteBook(service, (short) 3, true, this.MonthClosed.PeriodName.Value);
        }
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
      return true;
    }

    private bool DeleteLsServiceParam(string sidlic)
    {
      LsServiceParam lsServiceParam = new LsServiceParam();
      lsServiceParam.LsClient = this.session.Get<LsClient>((object) Convert.ToInt32(sidlic));
      this.session.Clear();
      lsServiceParam.Param = this.lsServiceParam.Param;
      lsServiceParam.DBeg = this.lsServiceParam.DBeg;
      lsServiceParam.DEnd = this.lsServiceParam.DEnd;
      lsServiceParam.Period = this.lsServiceParam.Period;
      lsServiceParam.Service = this.lsServiceParam.Service;
      try
      {
        this.session.Delete((object) lsServiceParam);
        this.session.Flush();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
      return true;
    }

    private bool DeleteLsSupplier(string sidlic)
    {
      LsSupplier supplier = new LsSupplier();
      supplier.LsClient = this.session.Get<LsClient>((object) Convert.ToInt32(sidlic));
      this.session.Clear();
      supplier.DBeg = this.lsSupplier.DBeg;
      supplier.DEnd = this.lsSupplier.DEnd;
      supplier.Period = this.lsSupplier.Period;
      supplier.Service = this.lsSupplier.Service;
      supplier.Supplier = this.lsSupplier.Supplier;
      try
      {
        this.session.Delete((object) supplier);
        this.session.Flush();
        if ((uint) this.lsSupplier.Period.PeriodId > 0U)
        {
          if (Convert.ToInt32(KvrplHelper.BaseValue(32, supplier.LsClient.Company)) == 1)
            KvrplHelper.DeleteSupplierFromNoteBook(supplier, supplier.LsClient, (short) 3, true, this.MonthClosed.PeriodName.Value);
        }
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
      return true;
    }

    private bool DeleteLsQuality(string sidlic)
    {
      LsQuality quality = new LsQuality();
      quality.LsClient = this.session.Get<LsClient>((object) Convert.ToInt32(sidlic));
      this.session.Clear();
      quality.DBeg = this.currentQuality.DBeg;
      quality.DEnd = this.currentQuality.DEnd;
      quality.Period = this.currentQuality.Period;
      quality.Quality = this.currentQuality.Quality;
      try
      {
        this.session.Delete((object) quality);
        this.session.Flush();
        if (Convert.ToInt32(KvrplHelper.BaseValue(32, quality.LsClient.Company)) == 1)
          KvrplHelper.DeleteQualityFromNoteBook(quality, quality.LsClient, (short) 3, this.MonthClosed.PeriodName.Value);
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
    }

    private bool DeleteLsParam(string sidlic)
    {
      ClientParam clientParam = new ClientParam();
      LsClient lsClient = this.session.Get<LsClient>((object) Convert.ToInt32(sidlic));
      clientParam.ClientId = lsClient.ClientId;
      this.session.Clear();
      clientParam.DBeg = this.ClientParam.DBeg;
      clientParam.Dedit = DateTime.Now.Date;
      clientParam.DEnd = this.ClientParam.DEnd;
      clientParam.Param = this.ClientParam.Param;
      clientParam.ParamValue = this.ClientParam.ParamValue;
      clientParam.Period = this.ClientParam.Period;
      try
      {
        this.session.Delete((object) clientParam);
        this.session.Flush();
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
    }

    private bool DeleteCounterRelation(string sidlic)
    {
      this.session.Clear();
      CounterRelation counterRelation = new CounterRelation();
      counterRelation.LsClient = this.session.Get<LsClient>((object) Convert.ToInt32(sidlic));
      this.session.Clear();
      counterRelation.DBeg = this.cntrRelation.DBeg;
      counterRelation.DEnd = this.cntrRelation.DEnd;
      counterRelation.Period = this.cntrRelation.Period;
      counterRelation.DEdit = DateTime.Now.Date;
      counterRelation.Counter = this.cntrRelation.Counter;
      counterRelation.OnOff = this.cntrRelation.OnOff;
      try
      {
        this.session.Delete((object) counterRelation);
        this.session.Flush();
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
    }

    private bool DeleteNoteBook(string sidlic)
    {
      this.session.Clear();
      NoteBook noteBook = new NoteBook();
      noteBook.ClientId = Convert.ToInt32(sidlic);
      this.session.Clear();
      noteBook.Company = this.noteBook.Company;
      noteBook.IdHome = this.noteBook.IdHome;
      noteBook.Text = this.noteBook.Text;
      noteBook.TypeNoteBook = this.noteBook.TypeNoteBook;
      try
      {
        this.session.CreateQuery(string.Format("delete from NoteBook where Company.CompanyId={0} and IdHome={1} and ClientId={2} and Text=:text and DBeg>='{4}' and TypeNoteBook.TypeNoteBookId={5}", (object) noteBook.Company.CompanyId, (object) noteBook.IdHome, (object) noteBook.ClientId, (object) noteBook.Text, (object) KvrplHelper.DateToBaseFormat(this.MonthClosed.PeriodName.Value.AddMonths(1)), (object) noteBook.TypeNoteBook.TypeNoteBookId)).SetParameter<string>("text", noteBook.Text).ExecuteUpdate();
        this.session.Flush();
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
    }

    private bool SaveHmParam(Home hm)
    {
      this.session.Clear();
      HomeParam homeParam = new HomeParam();
      homeParam.Home = hm;
      homeParam.Company = hm.Company;
      homeParam.DBeg = this.hmParam.DBeg;
      homeParam.DEnd = this.hmParam.DEnd;
      homeParam.Param = this.hmParam.Param;
      homeParam.ParamValue = this.hmParam.ParamValue;
      homeParam.Period = this.hmParam.Period;
      homeParam.Dedit = DateTime.Now;
      homeParam.Uname = Options.Login;
      try
      {
        this.session.Save((object) homeParam);
        this.session.Flush();
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
    }

    private bool DeleteHmParam(Home hm)
    {
      HomeParam homeParam = new HomeParam();
      homeParam.Home = hm;
      homeParam.Company = hm.Company;
      homeParam.DBeg = this.hmParam.DBeg;
      homeParam.DEnd = this.hmParam.DEnd;
      homeParam.Param = this.hmParam.Param;
      homeParam.ParamValue = this.hmParam.ParamValue;
      homeParam.Period = this.hmParam.Period;
      try
      {
        this.session.Delete((object) homeParam);
        this.session.Flush();
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
    }

    private bool UpdateHmParam(Home hm)
    {
      this.session.Clear();
      HomeParam homeParam = new HomeParam();
      homeParam.Home = hm;
      homeParam.Company = hm.Company;
      homeParam.DBeg = this.hmParam.DBeg;
      homeParam.DEnd = this.hmParam.DEnd;
      homeParam.Param = this.hmParam.Param;
      homeParam.ParamValue = this.hmParam.ParamValue;
      homeParam.Period = this.hmParam.Period;
      homeParam.Uname = this.hmParam.Uname;
      try
      {
        this.session.CreateSQLQuery("update DBA.hmParam cmp set DEnd=:dend,Uname=:uname, Dedit=:dedit where cmp.Company_Id=:companyid and cmp.Period_Id=:periodid and cmp.Param_Id = :oldparamid and cmp.idhome= :idhome  and cmp.Dbeg = (select max(dBeg) from DBA.hmParam where  Company_Id=cmp.Company_Id and param_id=cmp.param_id and period_id=cmp.period_id and idhome=cmp.idhome ) and cmp.dEnd >= :lastDayMonthClosed and Dbeg <= :dend ").SetParameter<DateTime>("dend", homeParam.DEnd.Date).SetParameter<string>("uname", Options.Login).SetParameter<DateTime>("dedit", DateTime.Now).SetParameter<short>("companyid", homeParam.Company.CompanyId).SetParameter<int>("periodid", homeParam.Period.PeriodId).SetParameter<short>("oldparamid", homeParam.Param.ParamId).SetParameter<int>("idhome", homeParam.Home.IdHome).SetParameter<DateTime>("lastDayMonthClosed", KvrplHelper.GetLastDayPeriod(this.MonthClosed.PeriodName.Value)).ExecuteUpdate();
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
    }

    private bool UpdateLsParam(string sidlic)
    {
      this.session.Clear();
      ClientParam clientParam = new ClientParam();
      clientParam.ClientId = Convert.ToInt32(sidlic);
      this.session.Clear();
      clientParam.DBeg = this.ClientParam.DBeg;
      clientParam.Dedit = DateTime.Now.Date;
      clientParam.DEnd = this.ClientParam.DEnd;
      clientParam.Param = this.ClientParam.Param;
      clientParam.ParamValue = this.ClientParam.ParamValue;
      clientParam.Period = this.ClientParam.Period;
      clientParam.Uname = Options.Login;
      try
      {
        this.session.CreateSQLQuery("update DBA.lsParam cmp set DEnd=:dend,Uname=:uname, Dedit=:dedit where cmp.Client_Id=:clientid and cmp.Period_Id=:periodid  and cmp.Param_Id = :oldparamid  and cmp.Dbeg = (select max(dBeg) from DBA.lsParam where  client_id=cmp.client_id and param_id=cmp.param_id and period_id=cmp.period_id) and cmp.dEnd >= :lastDayMonthClosed and Dbeg <= :dend ").SetParameter<DateTime>("dend", this.ClientParam.DEnd.Date).SetParameter<string>("uname", Options.Login).SetParameter<DateTime>("dedit", DateTime.Now).SetParameter<int>("clientid", clientParam.ClientId).SetParameter<int>("periodid", this.ClientParam.Period.PeriodId).SetParameter<short>("oldparamid", this.ClientParam.Param.ParamId).SetParameter<DateTime>("lastDayMonthClosed", KvrplHelper.GetLastDayPeriod(this.MonthClosed.PeriodName.Value)).ExecuteUpdate();
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
    }

    private bool UpdateLsService(string sidlic)
    {
      this.session.Clear();
      LsService lsService = new LsService();
      lsService.Client = this.session.Get<LsClient>((object) Convert.ToInt32(sidlic));
      this.session.Clear();
      lsService.Complex = this.lsService.Complex;
      lsService.DBeg = this.lsService.DBeg;
      lsService.DEnd = this.lsService.DEnd;
      lsService.Dedit = DateTime.Now;
      lsService.Norm = this.lsService.Norm;
      lsService.Period = this.lsService.Period;
      lsService.Service = this.lsService.Service;
      lsService.Tariff = this.lsService.Tariff;
      lsService.Uname = Options.Login;
      try
      {
        this.session.CreateSQLQuery("update DBA.LsService s set  dend=:dend, uname=:uname, dedit=:dedit  where client_id=:client_id and period_id=:period_id and service_id=:service_id  and Dbeg = (select max(dBeg) from DBA.LsService where  client_id=s.client_id and service_id=s.service_id and period_id=s.period_id) and dEnd >= :lastDayMonthClosed and Dbeg <= :dend").SetParameter<short>("service_id", this.lsService.Service.ServiceId).SetParameter<string>("dend", KvrplHelper.DateToBaseFormat(this.lsService.DEnd)).SetParameter<string>("uname", Options.Login).SetParameter<string>("dedit", KvrplHelper.DateToBaseFormat(DateTime.Now)).SetParameter<int>("client_id", lsService.Client.ClientId).SetParameter<int>("period_id", this.lsService.Period.PeriodId).SetParameter<DateTime>("lastDayMonthClosed", KvrplHelper.GetLastDayPeriod(this.MonthClosed.PeriodName.Value)).ExecuteUpdate();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
      return true;
    }

    private bool UpdateLsServiceParam(string sidlic)
    {
      this.session.Clear();
      LsServiceParam lsServiceParam = new LsServiceParam();
      lsServiceParam.LsClient = this.session.Get<LsClient>((object) Convert.ToInt32(sidlic));
      this.session.Clear();
      lsServiceParam.Param = this.lsServiceParam.Param;
      lsServiceParam.DBeg = this.lsServiceParam.DBeg;
      lsServiceParam.DEnd = this.lsServiceParam.DEnd;
      lsServiceParam.DEdit = DateTime.Now;
      lsServiceParam.ParamValue = this.lsServiceParam.ParamValue;
      lsServiceParam.Period = this.lsServiceParam.Period;
      lsServiceParam.Service = this.lsServiceParam.Service;
      lsServiceParam.UName = Options.Login;
      try
      {
        this.session.CreateSQLQuery("update DBA.LsServiceParam s set  dend=:dend, uname=:uname, dedit=:dedit  where client_id=:client_id and period_id=:period_id and service_id=:service_id and param_id=:param_id  and Dbeg = (select max(dBeg) from DBA.LsServiceParam where  client_id=s.client_id and service_id=s.service_id and period_id=s.period_id) and dEnd >= :lastDayMonthClosed and Dbeg <= :dend").SetParameter<short>("service_id", this.lsServiceParam.Service.ServiceId).SetParameter<string>("dend", KvrplHelper.DateToBaseFormat(this.lsServiceParam.DEnd)).SetParameter<string>("uname", Options.Login).SetParameter<string>("dedit", KvrplHelper.DateToBaseFormat(DateTime.Now)).SetParameter<int>("client_id", lsServiceParam.LsClient.ClientId).SetParameter<int>("period_id", this.lsServiceParam.Period.PeriodId).SetParameter<short>("param_id", this.lsServiceParam.Param.ParamId).SetParameter<DateTime>("lastDayMonthClosed", KvrplHelper.GetLastDayPeriod(this.MonthClosed.PeriodName.Value)).ExecuteUpdate();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
      return true;
    }

    private bool UpdateLsQuality(string sidlic)
    {
      this.session.Clear();
      LsQuality lsQuality = new LsQuality();
      lsQuality.LsClient = this.session.Get<LsClient>((object) Convert.ToInt32(sidlic));
      this.session.Clear();
      lsQuality.DBeg = this.currentQuality.DBeg;
      lsQuality.DEnd = this.currentQuality.DEnd;
      lsQuality.Period = this.currentQuality.Period;
      lsQuality.Quality = this.currentQuality.Quality;
      lsQuality.UName = Options.Login;
      lsQuality.DEdit = DateTime.Now;
      try
      {
        this.session.CreateSQLQuery("update DBA.lsQuality cmp set DEnd=:dend,Uname=:uname, Dedit=:dedit where cmp.Client_Id=:clientid and cmp.Period_Id=:periodid and cmp.Dbeg = :dbeg and cmp.Quality_id = :qualityid").SetParameter<DateTime?>("dend", lsQuality.DEnd).SetParameter<string>("uname", Options.Login).SetParameter<DateTime>("dedit", DateTime.Now).SetParameter<int>("clientid", lsQuality.LsClient.ClientId).SetParameter<int>("periodid", lsQuality.Period.PeriodId).SetParameter<DateTime>("dbeg", lsQuality.DBeg.Date).SetParameter<int>("qualityid", lsQuality.Quality.Quality_id).ExecuteUpdate();
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
    }

    private bool UpdateLsSupplier(string sidlic)
    {
      this.session.Clear();
      LsSupplier lsSupplier = new LsSupplier();
      lsSupplier.LsClient = this.session.Get<LsClient>((object) Convert.ToInt32(sidlic));
      this.session.Clear();
      lsSupplier.DBeg = this.lsSupplier.DBeg;
      lsSupplier.DEnd = this.lsSupplier.DEnd;
      lsSupplier.Dedit = DateTime.Now;
      lsSupplier.Uname = Options.Login;
      lsSupplier.Period = this.lsSupplier.Period;
      lsSupplier.Service = this.lsSupplier.Service;
      lsSupplier.Supplier = this.lsSupplier.Supplier;
      try
      {
        this.session.CreateSQLQuery("update DBA.LsSupplier s set  dend=:dend, uname=:uname, dedit=:dedit  where client_id=:client_id and period_id=:period_id and service_id=:service_id  and Dbeg = (select max(dBeg) from DBA.LsSupplier where  client_id=s.client_id and service_id=s.service_id and period_id=s.period_id) and dEnd >= :lastDayMonthClosed and Dbeg <= :dend ").SetParameter<short>("service_id", lsSupplier.Service.ServiceId).SetParameter<DateTime>("dend", lsSupplier.DEnd).SetParameter<string>("uname", Options.Login).SetParameter<string>("dedit", KvrplHelper.DateToBaseFormat(DateTime.Now)).SetParameter<int>("client_id", lsSupplier.LsClient.ClientId).SetParameter<int>("period_id", lsSupplier.Period.PeriodId).SetParameter<DateTime>("lastDayMonthClosed", KvrplHelper.GetLastDayPeriod(this.MonthClosed.PeriodName.Value)).ExecuteUpdate();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
      return true;
    }

    private bool UpdateCounterRelation(string sidlic)
    {
      this.session.Clear();
      CounterRelation counterRelation = new CounterRelation();
      counterRelation.LsClient = this.session.Get<LsClient>((object) Convert.ToInt32(sidlic));
      this.session.Clear();
      counterRelation.DBeg = this.cntrRelation.DBeg;
      counterRelation.DEnd = this.cntrRelation.DEnd;
      counterRelation.Period = this.cntrRelation.Period;
      counterRelation.DEdit = DateTime.Now.Date;
      counterRelation.Counter = this.cntrRelation.Counter;
      counterRelation.OnOff = this.cntrRelation.OnOff;
      counterRelation.UName = Options.Login;
      try
      {
        this.session.CreateSQLQuery("update DBA.cntrRelation cmp set DEnd=:dend,OnOff=:onoff,Uname=:uname, Dedit=:dedit where cmp.Client_Id=:clientid and cmp.Period_Id=:periodid and cmp.Dbeg = :olddbeg and cmp.Dend>:dend and cmp.Counter_Id = :oldcounterid").SetParameter<DateTime>("dend", counterRelation.DEnd.Date).SetParameter<YesNo>("onoff", counterRelation.OnOff).SetParameter<string>("uname", Options.Login).SetParameter<DateTime>("dedit", DateTime.Now).SetParameter<int>("clientid", counterRelation.LsClient.ClientId).SetParameter<int>("periodid", counterRelation.Period.PeriodId).SetParameter<DateTime>("olddbeg", counterRelation.DBeg.Date).SetParameter<int>("oldcounterid", counterRelation.Counter.CounterId).ExecuteUpdate();
        return true;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        this.session.Clear();
        return false;
      }
    }

    private void rbtnLs_CheckedChanged(object sender, EventArgs e)
    {
      this.lblCaption.Text = "Номер л/с      Номер кв.    Номер комн.";
      this.DisableFlatTextBox();
      this.chlbObject.Items.Clear();
      if ((uint) this.ClientId <= 0U)
        return;
      foreach (LsClient lsClient in (IEnumerable<LsClient>) this.GetListLsClient(""))
        this.chlbObject.Items.Add((object) lsClient.GetStrFlat());
    }

    private void rbtnEntrance_CheckedChanged(object sender, EventArgs e)
    {
      this.chlbObject.Items.Clear();
      this.EnableFlatTextBox();
      this.tbBegin.Focus();
    }

    private void rbtnFloor_CheckedChanged(object sender, EventArgs e)
    {
      this.chlbObject.Items.Clear();
      this.EnableFlatTextBox();
      this.tbBegin.Focus();
    }

    private void rbtnCompany_CheckedChanged(object sender, EventArgs e)
    {
      this.lblCaption.Text = "Компании";
      this.DisableFlatTextBox();
      this.chlbObject.Items.Clear();
    }

    private void chbArhive_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.rbtnLs.Checked)
        return;
      this.rbtnLs_CheckedChanged(sender, e);
    }

    private void chbUchet_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.rbtnLs.Checked)
        return;
      this.rbtnLs_CheckedChanged(sender, e);
    }

    private void rbtnPrivat_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.rbtnLs.Checked)
        return;
      this.rbtnLs_CheckedChanged(sender, e);
    }

    private void rbtnNotPrivat_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.rbtnLs.Checked)
        return;
      this.rbtnLs_CheckedChanged(sender, e);
    }

    private IList<LsClient> GetListLsClient(string condition)
    {
      LsClient lsClient = this.session.Get<LsClient>((object) this.ClientId);
      Period nextPeriod = KvrplHelper.GetNextPeriod(KvrplHelper.GetCmpKvrClose(lsClient.Company, Options.ComplexPasp.ComplexId, Options.ComplexPrior.ComplexId));
      string str1 = this.chbArhive.Checked || this.rbtnPrivat.Checked || (this.rbtnNotPrivat.Checked || this.chbUchet.Checked) ? string.Format(" and p.DBeg <= today() and p.DEnd >= today() ", (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(nextPeriod.PeriodName.Value)), (object) KvrplHelper.DateToBaseFormat(nextPeriod.PeriodName.Value)) : "";
      string str2 = this.chbArhive.Checked ? string.Format(" and isnull((select p.ParamValue from ClientParam p where p.Param.ParamId=107 and p.ClientId=ls.ClientId {0}),0) not in (4,5)", (object) str1) : "";
      string str3 = this.rbtnPrivat.Checked ? string.Format(" and exists (select p.ClientId from ClientParam p where p.Param.ParamId=104 and p.ClientId=ls.ClientId and p.ParamValue in (2,5,6,7,8,12,14,15,17,18,21,25,28,29,31,33,34,35) {0})", (object) str1) : "";
      string str4 = this.rbtnNotPrivat.Checked ? string.Format(" and exists (select p.ClientId from ClientParam p where p.Param.ParamId=104 and p.ClientId=ls.ClientId and p.ParamValue not in (2,5,6,7,8,12,14,15,17,18,21,25,28,29,31,33,34,35) {0})", (object) str1) : "";
      string str5 = this.chbUchet.Checked ? string.Format(" and isnull((select p.ParamValue from ClientParam p where p.Param.ParamId=107 and p.ClientId=ls.ClientId {0}),0) not in (3)", (object) str1) : "";
      string str6 = this.rbKv.Checked ? string.Format(" and ls.Complex.IdFk={0}", (object) Options.Complex.ComplexId) : "";
      string str7 = this.rbAr.Checked ? string.Format(" and ls.Complex.IdFk={0}", (object) Options.ComplexArenda.ComplexId) : "";
      string str8;
      if (!this.chbVariant.Checked)
        str8 = "";
      else
        str8 = string.Format(" and ls.ClientId in (select c.Client.ClientId from LsService c, Tariff t where  c.Tariff.Tariff_id = t.Tariff_id and t.Service.ServiceId = {0} and t.Service.ServiceId= c.Service.ServiceId  and c.Client.Home.IdHome = {1} and c.DBeg <= '{2}' and c.DEnd >= '{3}' and t.Tariff_num in ({4}) )", (object) this.serviceId, (object) lsClient.Home.IdHome, (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(nextPeriod.PeriodName.Value)), (object) KvrplHelper.DateToBaseFormat(nextPeriod.PeriodName.Value), (object) this.GenListVariant());
      string str9 = str8;
      string str10 = this.rbWith.Checked || this.rbWithout.Checked ? string.Format(" (select p.LsClient.ClientId from LsServiceParam p where p.Period.PeriodId=0 {0} and p.Service.ServiceId={1}  and p.Param.ParamId=405 " + (Convert.ToInt32(this.cmbType.SelectedValue) != 0 ? string.Format(" and ParamValue={0}", (object) ((Scheme) this.cmbType.SelectedItem).SchemeId) : "") + ")", (object) str1, (object) this.serviceId) : "";
      if (this.rbWith.Checked)
        str10 = "  and ls.ClientId in" + str10;
      if (this.rbWithout.Checked)
        str10 = "  and ls.ClientId not in" + str10;
      IList<LsClient> lsClientList;
      if (!this.chbArhive.Checked && !this.rbtnPrivat.Checked && (!this.rbtnNotPrivat.Checked && !this.chbUchet.Checked) && (!this.chbVariant.Checked && !this.rbKv.Checked && !this.rbAr.Checked))
        lsClientList = this.session.CreateQuery(string.Format("from LsClient ls where ls.Home.IdHome = {0} {1} " + Options.MainConditions1 + " order by ls.Complex.IdFk,DBA.LENGTHHOME(ls.Flat.NFlat)", (object) lsClient.Home.IdHome, (object) condition)).List<LsClient>();
      else
        lsClientList = this.session.CreateQuery(string.Format("select new LsClient(ls.ClientId,ls.Flat,ls.SurFlat) from LsClient ls where ls.Home.IdHome = {0} and ls.Company.CompanyId={9}" + Options.MainConditions1 + " {1} {2} {3} {4} {5} {6} {7} {8}" + str10 + " order by ls.Complex.IdFk,DBA.LENGTHHOME(ls.Flat.NFlat),dba.lengthhome(ls.SurFlat)", (object) lsClient.Home.IdHome, (object) str2, (object) str3, (object) str4, (object) str5, (object) str9, (object) condition, (object) str6, (object) str7, (object) lsClient.Company.CompanyId)).List<LsClient>();
      return lsClientList;
    }

    private void MakeActionLsParam(string sidlic)
    {
      switch (this.CodeOperation)
      {
        case 1:
          this.SaveLsParam(sidlic);
          break;
        case 2:
          this.DeleteLsParam(sidlic);
          break;
        case 3:
          this.UpdateLsParam(sidlic);
          break;
      }
    }

    private void MakeActionLsService(string sidlic)
    {
      switch (this.CodeOperation)
      {
        case 1:
          this.SaveLsService(sidlic);
          break;
        case 2:
          this.DeleteLsService(sidlic);
          break;
        case 3:
          this.UpdateLsService(sidlic);
          break;
      }
    }

    private void MakeActionLsServiceParam(string sidlic)
    {
      switch (this.CodeOperation)
      {
        case 1:
          this.SaveLsServiceParam(sidlic);
          break;
        case 2:
          this.DeleteLsServiceParam(sidlic);
          break;
        case 3:
          this.UpdateLsServiceParam(sidlic);
          break;
      }
    }

    private void MakeActionLsQuality(string sidlic)
    {
      switch (this.CodeOperation)
      {
        case 1:
          this.SaveLsQuality(sidlic);
          break;
        case 2:
          this.DeleteLsQuality(sidlic);
          break;
        case 3:
          this.UpdateLsQuality(sidlic);
          break;
      }
    }

    private void MakeActionLsSupplier(string sidlic)
    {
      switch (this.CodeOperation)
      {
        case 1:
          this.SaveLsSupplier(sidlic);
          break;
        case 2:
          this.DeleteLsSupplier(sidlic);
          break;
        case 3:
          this.UpdateLsSupplier(sidlic);
          break;
      }
    }

    private void MakeActionRent(string sidlic)
    {
      if ((int) this.CodeOperation != 1)
        return;
      this.SaveRent(sidlic);
    }

    private void MakeActionHmParam(Home home)
    {
      switch (this.CodeOperation)
      {
        case 1:
          this.SaveHmParam(home);
          break;
        case 2:
          this.DeleteHmParam(home);
          break;
        case 3:
          this.UpdateHmParam(home);
          break;
      }
    }

    private void MakeActionCounterRelation(string sidlic)
    {
      switch (this.CodeOperation)
      {
        case 1:
          this.SaveCounterRelation(sidlic);
          break;
        case 2:
          this.DeleteCounterRelation(sidlic);
          break;
        case 3:
          this.UpdateCounterRelation(sidlic);
          break;
      }
    }

    private bool MakeActionCounter(string sidlic)
    {
      switch (this.CodeOperation)
      {
        case 1:
          return this.SaveCounter(sidlic);
        default:
          return true;
      }
    }

    private bool MakeActionHmPhones(string sidlic)
    {
      switch (this.CodeOperation)
      {
        case 1:
          return this.SaveHmPhones(sidlic);
        case 2:
          this.DeleteHmPhones(sidlic);
          break;
        case 3:
          this.UpdateHmPhones(sidlic);
          break;
      }
      return true;
    }

    private bool MakeActionNoteBook(string sidlic)
    {
      switch (this.CodeOperation)
      {
        case 1:
          return this.SaveNoteBook(sidlic);
        case 2:
          return this.DeleteNoteBook(sidlic);
        default:
          return true;
      }
    }

    private bool MakeActionCorrectRent(string sidlic)
    {
      if ((int) this.CodeOperation == 1)
        return this.SaveCorrectRent(sidlic);
      return true;
    }

    private void btnCheckVariant_Click(object sender, EventArgs e)
    {
      if (this.checkAllVariant)
      {
        KvrplHelper.CheckAll(this.chlbVariant);
        this.btnCheckVariant.Text = "Снять все";
        this.checkAllVariant = false;
      }
      else
      {
        KvrplHelper.UnCheckAll(this.chlbVariant);
        this.btnCheckVariant.Text = "Выделить все";
        this.checkAllVariant = true;
      }
    }

    private void chbVariant_CheckedChanged(object sender, EventArgs e)
    {
      this.gpVariant.Visible = this.chbVariant.Checked;
      if (!this.chbVariant.Checked || !this.rbtnLs.Checked)
        return;
      this.rbtnLs_CheckedChanged(sender, e);
    }

    private string GenListVariant()
    {
      string str = "";
      foreach (int checkedItem in this.chlbVariant.CheckedItems)
        str = str + (object) checkedItem + ",";
      return str.Length != 0 ? str.Remove(str.Length - 1, 1) : "-1";
    }

    private void chlbVariant_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.chlbVariant.CheckedItems.Count == this.countVariantChecked)
        return;
      this.chlbObject.Items.Clear();
      foreach (LsClient lsClient in (IEnumerable<LsClient>) this.GetListLsClient(""))
        this.chlbObject.Items.Add((object) lsClient.GetStrFlat());
      this.countVariantChecked = this.chlbVariant.CheckedItems.Count;
    }

    private void mcArchive_DateSelected(object sender, DateRangeEventArgs e)
    {
      this.dateArchive = this.mcArchive.SelectionRange.End;
      this.mcArchive.Visible = false;
      bool flag = false;
      string str1 = "";
      if (MessageBox.Show("Закрыть карточки c " + this.dateArchive.ToShortDateString() + "?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
      {
        int[] numArray = new int[1000];
        int index = 0;
        foreach (string checkedItem in this.chlbObject.CheckedItems)
        {
          string str2 = checkedItem.Remove(checkedItem.IndexOf(" "), checkedItem.Length - checkedItem.IndexOf(" "));
          numArray[index] = Convert.ToInt32(str2);
          ++index;
          if (!KvrplHelper.CloseCard(this.session.Get<LsClient>((object) Convert.ToInt32(str2)), this.dateArchive))
          {
            str1 = str1 + str2 + "  ";
            flag = true;
          }
        }
      }
      if (!flag)
      {
        int num1 = (int) MessageBox.Show("Операция завершена", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        int num2 = (int) MessageBox.Show(string.Format("Возникли ошибки на следующих лицевых счетах {0}", (object) str1), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void FrmChooseObject_Shown(object sender, EventArgs e)
    {
      if (Options.Kvartplata && Options.Arenda)
        return;
      this.gbLs.Visible = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmChooseObject));
      this.pnButton = new Panel();
      this.btnExit = new Button();
      this.btnStart = new Button();
      this.gbChoice = new GroupBox();
      this.gbScheme = new GroupBox();
      this.cmbType = new ComboBox();
      this.rbWithout = new RadioButton();
      this.rbWith = new RadioButton();
      this.gbLs = new GroupBox();
      this.rbAr = new RadioButton();
      this.rbKv = new RadioButton();
      this.mcArchive = new MonthCalendar();
      this.chbVariant = new CheckBox();
      this.chbUchet = new CheckBox();
      this.rbtnFloor = new RadioButton();
      this.gpbFlat = new GroupBox();
      this.rbtnNotPrivat = new RadioButton();
      this.rbtnPrivat = new RadioButton();
      this.chbArhive = new CheckBox();
      this.rbtnHome = new RadioButton();
      this.tbEnd = new MaskedTextBox();
      this.tbBegin = new MaskedTextBox();
      this.lblTxt = new Label();
      this.rbtnEntrance = new RadioButton();
      this.rbtnCompany = new RadioButton();
      this.rbtnFlat = new RadioButton();
      this.rbtnLs = new RadioButton();
      this.gpVariant = new GroupBox();
      this.btnCheckVariant = new Button();
      this.chlbVariant = new CheckedListBox();
      this.groupBox2 = new GroupBox();
      this.panel1 = new Panel();
      this.lblCaption = new Label();
      this.btnCheck = new Button();
      this.chlbObject = new CheckedListBox();
      this.hp = new HelpProvider();
      this.grWorkDistrib = new GroupBox();
      this.lblSheme = new Label();
      this.cmbShema = new ComboBox();
      this.lblComission = new Label();
      this.txbComission = new TextBox();
      this.lblRate = new Label();
      this.txbRate = new TextBox();
      this.lblMonthCount = new Label();
      this.txbMonthCount = new TextBox();
      this.lblPerformer = new Label();
      this.lblRecipient = new Label();
      this.cmbPerformer = new ComboBox();
      this.cmbRecipient = new ComboBox();
      this.pnButton.SuspendLayout();
      this.gbChoice.SuspendLayout();
      this.gbScheme.SuspendLayout();
      this.gbLs.SuspendLayout();
      this.gpbFlat.SuspendLayout();
      this.gpVariant.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.grWorkDistrib.SuspendLayout();
      this.SuspendLayout();
      this.pnButton.Controls.Add((Control) this.btnExit);
      this.pnButton.Controls.Add((Control) this.btnStart);
      this.pnButton.Dock = DockStyle.Bottom;
      this.pnButton.Location = new Point(0, 520);
      this.pnButton.Margin = new Padding(4);
      this.pnButton.Name = "pnButton";
      this.pnButton.Size = new Size(829, 40);
      this.pnButton.TabIndex = 2;
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(732, 7);
      this.btnExit.Margin = new Padding(4);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(84, 30);
      this.btnExit.TabIndex = 1;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.button2_Click);
      this.btnStart.Image = (Image) Resources.Configure;
      this.btnStart.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnStart.Location = new Point(13, 5);
      this.btnStart.Margin = new Padding(4);
      this.btnStart.Name = "btnStart";
      this.btnStart.Size = new Size(112, 30);
      this.btnStart.TabIndex = 0;
      this.btnStart.Text = "Выполнить";
      this.btnStart.TextAlign = ContentAlignment.MiddleRight;
      this.btnStart.UseVisualStyleBackColor = true;
      this.btnStart.Click += new EventHandler(this.btnSave_Click);
      this.gbChoice.Controls.Add((Control) this.gbScheme);
      this.gbChoice.Controls.Add((Control) this.gbLs);
      this.gbChoice.Controls.Add((Control) this.mcArchive);
      this.gbChoice.Controls.Add((Control) this.chbVariant);
      this.gbChoice.Controls.Add((Control) this.chbUchet);
      this.gbChoice.Controls.Add((Control) this.rbtnFloor);
      this.gbChoice.Controls.Add((Control) this.gpbFlat);
      this.gbChoice.Controls.Add((Control) this.chbArhive);
      this.gbChoice.Controls.Add((Control) this.rbtnHome);
      this.gbChoice.Controls.Add((Control) this.tbEnd);
      this.gbChoice.Controls.Add((Control) this.tbBegin);
      this.gbChoice.Controls.Add((Control) this.lblTxt);
      this.gbChoice.Controls.Add((Control) this.rbtnEntrance);
      this.gbChoice.Controls.Add((Control) this.rbtnCompany);
      this.gbChoice.Controls.Add((Control) this.rbtnFlat);
      this.gbChoice.Controls.Add((Control) this.rbtnLs);
      this.gbChoice.Dock = DockStyle.Right;
      this.gbChoice.Location = new Point(463, 0);
      this.gbChoice.Name = "gbChoice";
      this.gbChoice.Size = new Size(366, 520);
      this.gbChoice.TabIndex = 3;
      this.gbChoice.TabStop = false;
      this.gbChoice.Text = "Выбор";
      this.gbChoice.Enter += new EventHandler(this.groupBox1_Enter);
      this.gbScheme.Controls.Add((Control) this.cmbType);
      this.gbScheme.Controls.Add((Control) this.rbWithout);
      this.gbScheme.Controls.Add((Control) this.rbWith);
      this.gbScheme.Location = new Point(6, 441);
      this.gbScheme.Name = "gbScheme";
      this.gbScheme.Size = new Size(354, 70);
      this.gbScheme.TabIndex = 17;
      this.gbScheme.TabStop = false;
      this.gbScheme.Text = "Тип расчет";
      this.gbScheme.Visible = false;
      this.cmbType.FormattingEnabled = true;
      this.cmbType.Location = new Point(157, 17);
      this.cmbType.Name = "cmbType";
      this.cmbType.Size = new Size(190, 24);
      this.cmbType.TabIndex = 2;
      this.cmbType.SelectionChangeCommitted += new EventHandler(this.rbtnPrivat_CheckedChanged);
      this.rbWithout.AutoSize = true;
      this.rbWithout.Location = new Point(14, 45);
      this.rbWithout.Name = "rbWithout";
      this.rbWithout.Size = new Size(143, 21);
      this.rbWithout.TabIndex = 1;
      this.rbWithout.TabStop = true;
      this.rbWithout.Text = "Без типа расчета";
      this.rbWithout.UseVisualStyleBackColor = true;
      this.rbWithout.CheckedChanged += new EventHandler(this.rbtnPrivat_CheckedChanged);
      this.rbWith.AutoSize = true;
      this.rbWith.Location = new Point(14, 20);
      this.rbWith.Name = "rbWith";
      this.rbWith.Size = new Size(137, 21);
      this.rbWith.TabIndex = 0;
      this.rbWith.TabStop = true;
      this.rbWith.Text = "С типом расчета";
      this.rbWith.UseVisualStyleBackColor = true;
      this.rbWith.CheckedChanged += new EventHandler(this.rbtnPrivat_CheckedChanged);
      this.gbLs.Controls.Add((Control) this.rbAr);
      this.gbLs.Controls.Add((Control) this.rbKv);
      this.gbLs.Location = new Point(7, 301);
      this.gbLs.Name = "gbLs";
      this.gbLs.Size = new Size(353, 68);
      this.gbLs.TabIndex = 16;
      this.gbLs.TabStop = false;
      this.gbLs.Text = "Лицевые";
      this.rbAr.AutoSize = true;
      this.rbAr.Location = new Point(14, 45);
      this.rbAr.Name = "rbAr";
      this.rbAr.Size = new Size(77, 21);
      this.rbAr.TabIndex = 1;
      this.rbAr.TabStop = true;
      this.rbAr.Text = "Аренды";
      this.rbAr.UseVisualStyleBackColor = true;
      this.rbAr.CheckedChanged += new EventHandler(this.rbtnPrivat_CheckedChanged);
      this.rbKv.AutoSize = true;
      this.rbKv.Location = new Point(14, 20);
      this.rbKv.Name = "rbKv";
      this.rbKv.Size = new Size(106, 21);
      this.rbKv.TabIndex = 0;
      this.rbKv.TabStop = true;
      this.rbKv.Text = "Квартплаты";
      this.rbKv.UseVisualStyleBackColor = true;
      this.rbKv.CheckedChanged += new EventHandler(this.rbtnPrivat_CheckedChanged);
      this.mcArchive.BackColor = Color.AliceBlue;
      this.mcArchive.Location = new Point(20, 18);
      this.mcArchive.Name = "mcArchive";
      this.mcArchive.TabIndex = 3;
      this.mcArchive.TitleBackColor = Color.DarkOrange;
      this.mcArchive.Visible = false;
      this.mcArchive.DateSelected += new DateRangeEventHandler(this.mcArchive_DateSelected);
      this.chbVariant.AutoSize = true;
      this.chbVariant.Location = new Point(6, 423);
      this.chbVariant.Name = "chbVariant";
      this.chbVariant.Size = new Size(158, 21);
      this.chbVariant.TabIndex = 15;
      this.chbVariant.Text = "По вариантам услуг";
      this.chbVariant.UseVisualStyleBackColor = true;
      this.chbVariant.Visible = false;
      this.chbVariant.CheckedChanged += new EventHandler(this.chbVariant_CheckedChanged);
      this.chbUchet.AutoSize = true;
      this.chbUchet.Location = new Point(6, 399);
      this.chbUchet.Name = "chbUchet";
      this.chbUchet.Size = new Size(136, 21);
      this.chbUchet.TabIndex = 14;
      this.chbUchet.Text = "Без учетных л.с.";
      this.chbUchet.UseVisualStyleBackColor = true;
      this.chbUchet.CheckedChanged += new EventHandler(this.chbUchet_CheckedChanged);
      this.rbtnFloor.AutoSize = true;
      this.rbtnFloor.Location = new Point(20, 82);
      this.rbtnFloor.Name = "rbtnFloor";
      this.rbtnFloor.Size = new Size(96, 21);
      this.rbtnFloor.TabIndex = 13;
      this.rbtnFloor.TabStop = true;
      this.rbtnFloor.Text = "По этажам";
      this.rbtnFloor.UseVisualStyleBackColor = true;
      this.rbtnFloor.CheckedChanged += new EventHandler(this.rbtnFloor_CheckedChanged);
      this.gpbFlat.Controls.Add((Control) this.rbtnNotPrivat);
      this.gpbFlat.Controls.Add((Control) this.rbtnPrivat);
      this.gpbFlat.Location = new Point(7, 223);
      this.gpbFlat.Name = "gpbFlat";
      this.gpbFlat.Size = new Size(353, 70);
      this.gpbFlat.TabIndex = 12;
      this.gpbFlat.TabStop = false;
      this.gpbFlat.Text = "Квартира";
      this.rbtnNotPrivat.AutoSize = true;
      this.rbtnNotPrivat.Location = new Point(13, 45);
      this.rbtnNotPrivat.Name = "rbtnNotPrivat";
      this.rbtnNotPrivat.Size = new Size(178, 21);
      this.rbtnNotPrivat.TabIndex = 1;
      this.rbtnNotPrivat.TabStop = true;
      this.rbtnNotPrivat.Text = "Неприватизированные";
      this.rbtnNotPrivat.UseVisualStyleBackColor = true;
      this.rbtnNotPrivat.CheckedChanged += new EventHandler(this.rbtnNotPrivat_CheckedChanged);
      this.rbtnPrivat.AutoSize = true;
      this.rbtnPrivat.Location = new Point(13, 20);
      this.rbtnPrivat.Name = "rbtnPrivat";
      this.rbtnPrivat.Size = new Size(162, 21);
      this.rbtnPrivat.TabIndex = 0;
      this.rbtnPrivat.TabStop = true;
      this.rbtnPrivat.Text = "Приватизированные";
      this.rbtnPrivat.UseVisualStyleBackColor = true;
      this.rbtnPrivat.CheckedChanged += new EventHandler(this.rbtnPrivat_CheckedChanged);
      this.chbArhive.AutoSize = true;
      this.chbArhive.Location = new Point(6, 375);
      this.chbArhive.Name = "chbArhive";
      this.chbArhive.Size = new Size(222, 21);
      this.chbArhive.TabIndex = 11;
      this.chbArhive.Text = "Без закрытых и архивных л.с.";
      this.chbArhive.UseVisualStyleBackColor = true;
      this.chbArhive.CheckedChanged += new EventHandler(this.chbArhive_CheckedChanged);
      this.rbtnHome.AutoSize = true;
      this.rbtnHome.Location = new Point(20, 138);
      this.rbtnHome.Name = "rbtnHome";
      this.rbtnHome.Size = new Size(90, 21);
      this.rbtnHome.TabIndex = 10;
      this.rbtnHome.TabStop = true;
      this.rbtnHome.Text = "По домам";
      this.rbtnHome.UseVisualStyleBackColor = true;
      this.tbEnd.Location = new Point(100, 194);
      this.tbEnd.Mask = "00000";
      this.tbEnd.Name = "tbEnd";
      this.tbEnd.PromptChar = ' ';
      this.tbEnd.Size = new Size(55, 23);
      this.tbEnd.TabIndex = 9;
      this.tbEnd.ValidatingType = typeof (int);
      this.tbBegin.Location = new Point(20, 194);
      this.tbBegin.Mask = "00000";
      this.tbBegin.Name = "tbBegin";
      this.tbBegin.PromptChar = ' ';
      this.tbBegin.Size = new Size(55, 23);
      this.tbBegin.TabIndex = 8;
      this.tbBegin.ValidatingType = typeof (int);
      this.lblTxt.AutoSize = true;
      this.lblTxt.Location = new Point(81, 197);
      this.lblTxt.Name = "lblTxt";
      this.lblTxt.Size = new Size(13, 17);
      this.lblTxt.TabIndex = 7;
      this.lblTxt.Text = "-";
      this.rbtnEntrance.AutoSize = true;
      this.rbtnEntrance.Location = new Point(20, 110);
      this.rbtnEntrance.Name = "rbtnEntrance";
      this.rbtnEntrance.Size = new Size(121, 21);
      this.rbtnEntrance.TabIndex = 3;
      this.rbtnEntrance.TabStop = true;
      this.rbtnEntrance.Text = "По подъездам";
      this.rbtnEntrance.UseVisualStyleBackColor = true;
      this.rbtnEntrance.CheckedChanged += new EventHandler(this.rbtnEntrance_CheckedChanged);
      this.rbtnCompany.AutoSize = true;
      this.rbtnCompany.Location = new Point(20, 166);
      this.rbtnCompany.Name = "rbtnCompany";
      this.rbtnCompany.Size = new Size(109, 21);
      this.rbtnCompany.TabIndex = 2;
      this.rbtnCompany.TabStop = true;
      this.rbtnCompany.Text = "По участкам";
      this.rbtnCompany.UseVisualStyleBackColor = true;
      this.rbtnFlat.AutoSize = true;
      this.rbtnFlat.Location = new Point(20, 54);
      this.rbtnFlat.Name = "rbtnFlat";
      this.rbtnFlat.Size = new Size(118, 21);
      this.rbtnFlat.TabIndex = 1;
      this.rbtnFlat.TabStop = true;
      this.rbtnFlat.Text = "По квартирам";
      this.rbtnFlat.UseVisualStyleBackColor = true;
      this.rbtnFlat.CheckedChanged += new EventHandler(this.radioButton2_CheckedChanged);
      this.rbtnLs.AutoSize = true;
      this.rbtnLs.Location = new Point(20, 26);
      this.rbtnLs.Name = "rbtnLs";
      this.rbtnLs.Size = new Size(106, 21);
      this.rbtnLs.TabIndex = 0;
      this.rbtnLs.TabStop = true;
      this.rbtnLs.Text = "По лицевым";
      this.rbtnLs.UseVisualStyleBackColor = true;
      this.rbtnLs.CheckedChanged += new EventHandler(this.rbtnLs_CheckedChanged);
      this.gpVariant.Controls.Add((Control) this.btnCheckVariant);
      this.gpVariant.Controls.Add((Control) this.chlbVariant);
      this.gpVariant.Dock = DockStyle.Bottom;
      this.gpVariant.Location = new Point(0, 227);
      this.gpVariant.Name = "gpVariant";
      this.gpVariant.Size = new Size(463, 119);
      this.gpVariant.TabIndex = 4;
      this.gpVariant.TabStop = false;
      this.gpVariant.Text = "Варианты услуг";
      this.gpVariant.Visible = false;
      this.btnCheckVariant.Dock = DockStyle.Bottom;
      this.btnCheckVariant.Image = (Image) Resources.properties;
      this.btnCheckVariant.Location = new Point(3, 88);
      this.btnCheckVariant.Name = "btnCheckVariant";
      this.btnCheckVariant.Size = new Size(457, 28);
      this.btnCheckVariant.TabIndex = 2;
      this.btnCheckVariant.Text = "Выделить все";
      this.btnCheckVariant.TextAlign = ContentAlignment.MiddleRight;
      this.btnCheckVariant.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.btnCheckVariant.UseVisualStyleBackColor = true;
      this.btnCheckVariant.Click += new EventHandler(this.btnCheckVariant_Click);
      this.chlbVariant.CheckOnClick = true;
      this.chlbVariant.FormattingEnabled = true;
      this.chlbVariant.Location = new Point(13, 22);
      this.chlbVariant.Name = "chlbVariant";
      this.chlbVariant.Size = new Size(336, 58);
      this.chlbVariant.TabIndex = 0;
      this.chlbVariant.SelectedIndexChanged += new EventHandler(this.chlbVariant_SelectedIndexChanged);
      this.groupBox2.Controls.Add((Control) this.panel1);
      this.groupBox2.Dock = DockStyle.Fill;
      this.groupBox2.Location = new Point(0, 0);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(463, 227);
      this.groupBox2.TabIndex = 5;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Список для выбора";
      this.panel1.Controls.Add((Control) this.lblCaption);
      this.panel1.Controls.Add((Control) this.btnCheck);
      this.panel1.Controls.Add((Control) this.chlbObject);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.ImeMode = ImeMode.NoControl;
      this.panel1.Location = new Point(3, 19);
      this.panel1.Margin = new Padding(4);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(457, 205);
      this.panel1.TabIndex = 5;
      this.lblCaption.AutoSize = true;
      this.lblCaption.Location = new Point(13, 5);
      this.lblCaption.Name = "lblCaption";
      this.lblCaption.Size = new Size(262, 17);
      this.lblCaption.TabIndex = 2;
      this.lblCaption.Text = "Номер л/с      Номер кв.    Номер комн.";
      this.btnCheck.Dock = DockStyle.Bottom;
      this.btnCheck.Image = (Image) Resources.properties;
      this.btnCheck.Location = new Point(0, 177);
      this.btnCheck.Name = "btnCheck";
      this.btnCheck.Size = new Size(457, 28);
      this.btnCheck.TabIndex = 1;
      this.btnCheck.Text = "Выделить все";
      this.btnCheck.TextAlign = ContentAlignment.MiddleRight;
      this.btnCheck.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.btnCheck.UseVisualStyleBackColor = true;
      this.btnCheck.Click += new EventHandler(this.btnCheck_Click);
      this.chlbObject.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.chlbObject.CheckOnClick = true;
      this.chlbObject.FormattingEnabled = true;
      this.chlbObject.Location = new Point(14, 26);
      this.chlbObject.Margin = new Padding(4);
      this.chlbObject.Name = "chlbObject";
      this.chlbObject.Size = new Size(430, 76);
      this.chlbObject.TabIndex = 0;
      this.hp.HelpNamespace = "Help.chm";
      this.grWorkDistrib.Controls.Add((Control) this.lblSheme);
      this.grWorkDistrib.Controls.Add((Control) this.cmbShema);
      this.grWorkDistrib.Controls.Add((Control) this.lblComission);
      this.grWorkDistrib.Controls.Add((Control) this.txbComission);
      this.grWorkDistrib.Controls.Add((Control) this.lblRate);
      this.grWorkDistrib.Controls.Add((Control) this.txbRate);
      this.grWorkDistrib.Controls.Add((Control) this.lblMonthCount);
      this.grWorkDistrib.Controls.Add((Control) this.txbMonthCount);
      this.grWorkDistrib.Controls.Add((Control) this.lblPerformer);
      this.grWorkDistrib.Controls.Add((Control) this.lblRecipient);
      this.grWorkDistrib.Controls.Add((Control) this.cmbPerformer);
      this.grWorkDistrib.Controls.Add((Control) this.cmbRecipient);
      this.grWorkDistrib.Dock = DockStyle.Bottom;
      this.grWorkDistrib.Location = new Point(0, 346);
      this.grWorkDistrib.Name = "grWorkDistrib";
      this.grWorkDistrib.Size = new Size(463, 174);
      this.grWorkDistrib.TabIndex = 6;
      this.grWorkDistrib.TabStop = false;
      this.grWorkDistrib.Visible = false;
      this.lblSheme.AutoSize = true;
      this.lblSheme.Location = new Point(3, 60);
      this.lblSheme.Name = "lblSheme";
      this.lblSheme.Size = new Size(48, 17);
      this.lblSheme.TabIndex = 31;
      this.lblSheme.Text = "Схема";
      this.cmbShema.FormattingEnabled = true;
      this.cmbShema.Location = new Point(6, 85);
      this.cmbShema.Name = "cmbShema";
      this.cmbShema.Size = new Size(200, 24);
      this.cmbShema.TabIndex = 2;
      this.lblComission.AutoSize = true;
      this.lblComission.Location = new Point(74, 112);
      this.lblComission.Name = "lblComission";
      this.lblComission.Size = new Size(72, 17);
      this.lblComission.TabIndex = 29;
      this.lblComission.Text = "Комиссия";
      this.txbComission.Location = new Point(77, 138);
      this.txbComission.Name = "txbComission";
      this.txbComission.Size = new Size(33, 23);
      this.txbComission.TabIndex = 4;
      this.lblRate.AutoSize = true;
      this.lblRate.Location = new Point(3, 112);
      this.lblRate.Name = "lblRate";
      this.lblRate.Size = new Size(65, 17);
      this.lblRate.TabIndex = 4;
      this.lblRate.Text = "Процент";
      this.txbRate.Location = new Point(6, 138);
      this.txbRate.Name = "txbRate";
      this.txbRate.Size = new Size(33, 23);
      this.txbRate.TabIndex = 3;
      this.lblMonthCount.AutoSize = true;
      this.lblMonthCount.Location = new Point(152, 112);
      this.lblMonthCount.Name = "lblMonthCount";
      this.lblMonthCount.Size = new Size(112, 17);
      this.lblMonthCount.TabIndex = 1;
      this.lblMonthCount.Text = "Кол-во месяцев";
      this.txbMonthCount.Location = new Point(147, 138);
      this.txbMonthCount.Name = "txbMonthCount";
      this.txbMonthCount.Size = new Size(33, 23);
      this.txbMonthCount.TabIndex = 5;
      this.lblPerformer.AutoSize = true;
      this.lblPerformer.Location = new Point((int) byte.MaxValue, 13);
      this.lblPerformer.Name = "lblPerformer";
      this.lblPerformer.Size = new Size(95, 17);
      this.lblPerformer.TabIndex = 23;
      this.lblPerformer.Text = "Исполнитель";
      this.lblRecipient.AutoSize = true;
      this.lblRecipient.Location = new Point(3, 13);
      this.lblRecipient.Name = "lblRecipient";
      this.lblRecipient.Size = new Size(87, 17);
      this.lblRecipient.TabIndex = 22;
      this.lblRecipient.Text = "Получатель";
      this.cmbPerformer.FormattingEnabled = true;
      this.cmbPerformer.Location = new Point(258, 33);
      this.cmbPerformer.Name = "cmbPerformer";
      this.cmbPerformer.Size = new Size(189, 24);
      this.cmbPerformer.TabIndex = 21;
      this.cmbRecipient.FormattingEnabled = true;
      this.cmbRecipient.Location = new Point(6, 33);
      this.cmbRecipient.Name = "cmbRecipient";
      this.cmbRecipient.Size = new Size(200, 24);
      this.cmbRecipient.TabIndex = 20;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(829, 560);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.gpVariant);
      this.Controls.Add((Control) this.grWorkDistrib);
      this.Controls.Add((Control) this.gbChoice);
      this.Controls.Add((Control) this.pnButton);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.hp.SetHelpKeyword((Control) this, "kv151.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmChooseObject";
      this.hp.SetShowHelp((Control) this, true);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Массовые операции";
      this.Load += new EventHandler(this.FrmChooseObject_Load);
      this.Shown += new EventHandler(this.FrmChooseObject_Shown);
      this.pnButton.ResumeLayout(false);
      this.gbChoice.ResumeLayout(false);
      this.gbChoice.PerformLayout();
      this.gbScheme.ResumeLayout(false);
      this.gbScheme.PerformLayout();
      this.gbLs.ResumeLayout(false);
      this.gbLs.PerformLayout();
      this.gpbFlat.ResumeLayout(false);
      this.gpbFlat.PerformLayout();
      this.gpVariant.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.grWorkDistrib.ResumeLayout(false);
      this.grWorkDistrib.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
