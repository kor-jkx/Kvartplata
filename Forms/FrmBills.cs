// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmBills
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using FastReport;
using FastReport.Data;
using FastReport.Export;
using FastReport.Export.Pdf;
using Kvartplata.Classes;
using Kvartplata.Properties;
using Kvartplata.Smirnov;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmBills : FrmBaseForm
  {
    private FormStateSaver fss = new FormStateSaver(FrmBills.container);
    private int setupPeriod = 1201;
    private int setupValue = 0;
    private int idBaseOrg = 0;
    private string pathSaveReceipt = "";
    private bool pathSave = false;
    private bool onlySave = false;
    private bool archiveAllDoc = false;
    private FastReport.PrintSettings _settings = (PrintSettings) null;
    private Dictionary<int, Report> _reportDictionary = new Dictionary<int, Report>();
    private List<ReportArenda> _reportList = new List<ReportArenda>();
    private List<int> _licClientArenda = new List<int>();
    private List<string> FileListReport = new List<string>();
    private bool _korTamplate = false;
    private string _pathSaveEmail = "";
    private IContainer components = (IContainer) null;
    private static IContainer container;
    private short level;
    private int city;
    private Raion raion;
    private Company company;
    private Home home;
    private LsClient client;
    private ISession session;
    private short days;
    private short billType;
    private IList objectsList;
    private const string NS_RightDoc_id = "(2,5,6,7,8,12,14,15,17,18,21,25,28,29,31,33,34)";
    private string rukName;
    private string buhName;
    private string rukPost;
    private string buhPost;
    private string connectionString;
    private TableDataSource _main;
    private TableDataSource _tarif;
    private BaseOrg baseOrg;
    private bool onClientCard;
    private LsClient clienttemp;
    private Panel pnBtn;
    private Panel pnPeriod;
    private MonthPicker mpCurrentPeriod;
    private Label lblPeriod;
    private GroupBox gbBill;
    private Button btnExit;
    private Button btnView;
    private Button btnPrint;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private Report report;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private Button button1;
    private ToolTip tTTemplate;
    private Button butSaveReport;
    private RadioButton rbAllArchive;
    private RadioButton rbAllDoc;
    private RadioButton rbAktDontInvoice;
    private RadioButton rbInvoice;
    private RadioButton rbBill;
    private Label lblNum;
    private Label lblDate;
    private TextBox txbNum;
    private DateTimePicker dtpDate;
    private Label lblKol;
    private NumericUpDown nudCountBill;
    private CheckBox chbIsAkt;
    private NumericUpDown nudCountAkt;
    private CheckBox chbIsBlank;
    private CheckBox chbSost;
    private Label lblSrok;
    private DateTimePicker dtpSrok;
    private CheckBox chbMec;
    private CheckBox chbCorrect;
    private NumericUpDown nudNumCorrect;
    private DataGridViewTextBoxColumn Prepare;
    private DataGridViewTextBoxColumn Num;
    private DataGridViewTextBoxColumn DocNum;
    private DataGridViewTextBoxColumn Id;
    private DataGridView dgvList;
    private TextBox txbNumCor;
    private ComboBox cbReceipt;
    private Label lblReceipt;
    private TextBox textBox1;
    private Label lblTypeAddress;
    private ComboBox cbTypeAddress;
    private CheckBox RoGbVisible;
    private CheckBox chB_Offer_FR;
    private ComboBox cbShablon;
    private Label labShablon;
    private Label labAkt;
    private ComboBox cbAkt;
    private CheckBox cbSaveReport;
    private CheckBox checkBoxMinusOff;
    private RadioButton rbFullPeniPeriod;
    private GroupBox gbPeni;
    private RadioButton rbPeniPeriod;
    private CheckBox chbPeni;
    private MonthPicker mpToPeriod;
    private GroupBox gbMountBreak;
    private MonthPicker mpFromPeriod;
    private Label label1;
    private Label label2;
    private CheckBox cbLetterCover;
    private CheckBox chbOnlyNew;
    private CheckBox chbEmail;
    private CheckBox chbStampDisplay;
    private CheckBox chbCorrectMonth;

    public FrmBills()
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
    }

    public FrmBills(short l, int city, Raion r, Company c, Home h, LsClient lc)
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
      this.level = l;
      this.city = city;
      this.raion = r;
      this.company = c;
      this.home = h;
      this.client = lc;
      this.onClientCard = false;
    }

    public FrmBills(short l, int city, Raion r, Company c, Home h, LsClient lc, BaseOrg bo)
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
      this.level = l;
      this.city = city;
      this.raion = r;
      this.company = c;
      this.home = h;
      this.client = lc;
      this.baseOrg = bo;
      this.onClientCard = true;
    }

    private void FrmBills_Load(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      BaseOrg baseOrg = new BaseOrg();
      try
      {
        baseOrg = this.session.Get<BaseOrg>((object) this.company.Manager.BaseOrgId);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, this.client);
      }
      if (baseOrg != null)
      {
        this.idBaseOrg = baseOrg.BaseOrgId;
        if (baseOrg.INN != null)
        {
          string inn = baseOrg.INN;
          if (inn.Equals("7604244281") || inn.Equals("7604196101") || inn.Equals("7604228988") || inn.Equals("7604179890"))
            this.chB_Offer_FR.Visible = true;
        }
      }
      foreach (FileSystemInfo directory in new DirectoryInfo("Шаблоны_ар\\").GetDirectories())
        this.FileListReport.Add(directory.Name);
      List<string> list1 = ((IEnumerable<string>) Directory.GetFiles("Шаблоны_ар\\", "Bill*.frx")).ToList<string>();
      list1.Insert(0, "Шаблон по умолчанию");
      this.cbShablon.DataSource = (object) list1;
      List<string> list2 = ((IEnumerable<string>) Directory.GetFiles("Шаблоны_ар\\", "Statement*.frx")).ToList<string>();
      list2.Insert(0, "Шаблон по умолчанию");
      this.cbAkt.DataSource = (object) list2;
      this.billType = (short) 1;
      this.LoadCbReceipts();
      this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
      this.cbTypeAddress.SelectedIndex = Options.AddressSending;
      try
      {
        File.Delete(Options.CurrentDomainPath + "Шаблоны_ар\\Акт.fr3");
        File.Delete(Options.CurrentDomainPath + "Шаблоны_ар\\Извещение.fr3");
        File.Delete(Options.CurrentDomainPath + "Шаблоны_ар\\Счет.fr3");
        File.Delete(Options.CurrentDomainPath + "Шаблоны_ар\\Корректировочный.fr3");
        File.Delete(Options.CurrentDomainPath + "Шаблоны_ар\\Фактура.fr3");
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void FrmBills_Shown(object sender, EventArgs e)
    {
      if (this.company != null && (uint) this.company.CompanyId > 0U)
      {
        IList<CompanyParam> companyParamList = this.session.CreateQuery(string.Format(" from CompanyParam where Company.CompanyId={0} and Period.PeriodId=0 and DBeg<='{1}' and Param.ParamId=216 and DEnd>='{1}'", (object) this.company.CompanyId, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value))).List<CompanyParam>();
        this.days = companyParamList.Count <= 0 ? (short) 9 : Convert.ToInt16(companyParamList[0].ParamValue - 1.0);
      }
      else
        this.days = (short) 9;
      this.UpdateData();
      if ((int) this.level == 4)
        this.btnView.Enabled = true;
      if ((int) this.level == 4)
        this.dgvList.Visible = false;
      if (this.city == 23)
      {
        this.chbIsAkt.Enabled = true;
        this.checkBoxMinusOff.Visible = true;
      }
      this.chbIsAkt.Enabled = true;
      this.nudCountAkt.Enabled = true;
      string str = Options.CurrentDomainPath + "Configuration\\" + Options.Login + "\\report.ini";
      if (!File.Exists(str))
        str = Options.CurrentDomainPath + "\\report.ini";
      IniFile iniFile = new IniFile(str);
      this.rukName = iniFile.IniReadValue("Setup", "NameCheff");
      this.rukPost = iniFile.IniReadValue("Setup", "RangeCheff1");
      this.buhName = iniFile.IniReadValue("Setup", "NameBux");
      this.buhPost = iniFile.IniReadValue("Setup", "RangeCheff2");
      this._pathSaveEmail = iniFile.IniReadValue("Setup", "PathEmail");
      if (this._pathSaveEmail == "")
      {
        this._pathSaveEmail = new IniFile(Options.CurrentDomainPath + "\\update.ini").IniReadValue("options", "updates");
        this._pathSaveEmail = this._pathSaveEmail + "\\Receipts";
      }
      if (!Directory.Exists(this._pathSaveEmail))
        Directory.CreateDirectory(this._pathSaveEmail);
      this.connectionString = string.Format("Dsn={0};uid={1};pwd={2}", (object) Options.Alias, (object) Options.Login, (object) Options.Pwd);
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      if (this.rbAllDoc.Checked || this.rbAllArchive.Checked)
      {
        if (MessageBox.Show("Сохранить квитанции?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
          FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
          int num = (int) folderBrowserDialog.ShowDialog();
          this.pathSaveReceipt = folderBrowserDialog.SelectedPath;
          this.pathSave = true;
        }
        else
          this.pathSave = false;
        this.onlySave = false;
        this.PrintAllDocument(true);
      }
      else if (this.rbAllDoc.Checked)
        this.PrintAllDocument(true);
      else
        this.PrintOrShowReports(true);
    }

    private void btnView_Click(object sender, EventArgs e)
    {
      if (this.rbAllDoc.Checked || this.rbAllArchive.Checked)
      {
        if (MessageBox.Show("Сохранить квитанции?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
          FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
          int num = (int) folderBrowserDialog.ShowDialog();
          this.pathSaveReceipt = folderBrowserDialog.SelectedPath;
          this.pathSave = true;
        }
        else
          this.pathSave = false;
        this.onlySave = false;
        this.PrintAllDocument(false);
      }
      else
        this.PrintOrShowReports(false);
    }

    private void mpCurrentPeriod_ValueChanged(object sender, EventArgs e)
    {
      Options.Period = KvrplHelper.SaveCurrentPeriod(this.mpCurrentPeriod.Value);
      this.UpdateData();
      this.ShowList();
    }

    private void rbBill_Click(object sender, EventArgs e)
    {
      if (this.rbBill.Checked)
      {
        this.butSaveReport.Enabled = false;
        this.billType = (short) 1;
        this.lblSrok.Enabled = true;
        this.dtpSrok.Enabled = true;
        this.chbCorrect.Checked = false;
        this.chbCorrect.Enabled = false;
        this.chbIsBlank.Enabled = true;
        this.gbMountBreak.Visible = false;
      }
      this.UpdateData();
      this.ShowList();
    }

    private void rbInvoice_Click(object sender, EventArgs e)
    {
      if (this.rbInvoice.Checked)
      {
        this.butSaveReport.Enabled = false;
        this.billType = (short) 2;
        this.lblSrok.Enabled = false;
        this.dtpSrok.Enabled = false;
        this.chbIsAkt.Enabled = true;
        this.nudCountAkt.Enabled = true;
        this.chbCorrect.Enabled = true;
        this.chbIsBlank.Enabled = false;
        this.chbIsBlank.Checked = false;
        this.gbMountBreak.Visible = false;
      }
      this.UpdateData();
      this.ShowList();
    }

    private void dtpDate_ValueChanged(object sender, EventArgs e)
    {
      DateTime dateTime1 = this.dtpDate.Value;
      DateTime? periodName;
      int num;
      if (!(dateTime1 < Options.Period.PeriodName.Value))
      {
        DateTime dateTime2 = dateTime1;
        periodName = Options.Period.PeriodName;
        DateTime dateTime3 = KvrplHelper.LastDay(periodName.Value);
        num = dateTime2 > dateTime3 ? 1 : 0;
      }
      else
        num = 1;
      if (num == 0)
        return;
      DateTimePicker dtpDate = this.dtpDate;
      periodName = Options.Period.PeriodName;
      DateTime dateTime4 = KvrplHelper.LastDay(periodName.Value);
      dtpDate.Value = dateTime4;
    }

    private void dgvList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.objectsList.Count <= 0)
        return;
      if (this.dgvList.Columns[e.ColumnIndex].Name == "Id")
        e.Value = ((object[]) this.objectsList[e.RowIndex])[0];
      if (this.dgvList.Columns[e.ColumnIndex].Name == "DocNum")
        e.Value = ((object[]) this.objectsList[e.RowIndex])[1];
      if (this.dgvList.Columns[e.ColumnIndex].Name == "Num")
        e.Value = ((object[]) this.objectsList[e.RowIndex])[2];
      if (this.dgvList.Columns[e.ColumnIndex].Name == "Prepare")
        e.Value = ((object[]) this.objectsList[e.RowIndex])[3];
    }

    private void chbCorrect_Click(object sender, EventArgs e)
    {
      if (!this.chbCorrect.Checked || this.rbAllDoc.Checked)
        return;
      this.chbMec.Checked = false;
    }

    private void chbMec_Click(object sender, EventArgs e)
    {
      if (!this.chbMec.Checked || this.rbAllDoc.Checked)
        return;
      this.chbCorrect.Checked = false;
    }

    private string NS_lsAddressHome(int code, string flat)
    {
      string str = "";
      switch (code)
      {
        case 1:
          str = "(select trim(str)";
          break;
        case 2:
          str = "(select (case s_region  when '' then '' else ', р-н.'||s_region end)||s_city||', '||trim(ulc)";
          break;
      }
      return str + "||', д.'||home||(case home_korp when '' then '' else ', корп.'||home_korp end) from homes ah,flats af,di_str ad where af.idflat=" + flat + " and ah.idhome=af.idhome and ah.idstr=ad.idstr)";
    }

    private string NS_lsParam_value(int param_id, string client, string fdat)
    {
      return "isnull((select first ms_p.param_value         from lsParam ms_p         where ms_p.period_id=0 and               ms_p.client_id=" + client + " and " + fdat + " between ms_p.dbeg and ms_p.dend and ms_p.param_id=" + param_id.ToString() + "),0)";
    }

    private void cbReceipt_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.UpdateData();
      this.ShowList();
    }

    private void LoadCbReceipts()
    {
      this.cbReceipt.DataSource = (object) this.session.CreateCriteria(typeof (Receipt)).Add((ICriterion) NHibernate.Criterion.Restrictions.Not((ICriterion) NHibernate.Criterion.Restrictions.Eq("ReceiptId", (object) Convert.ToInt16(0)))).AddOrder(Order.Asc("ReceiptId")).List<Receipt>();
      this.cbReceipt.ValueMember = "ReceiptId";
      this.cbReceipt.DisplayMember = "ReceiptName";
    }

    private void ShowList()
    {
      string str1 = Options.Period.PeriodId.ToString();
      string str2 = (int) this.billType != 1 ? " and bill_type in (2,3)" : " and bill_type = 1";
      if ((int) this.billType == -1)
        str2 = " and bill_type in (1,2,3)";
      string str3 = this.MainUsl();
      string[] strArray = new string[12];
      strArray[0] = "select ls.Client_id,dogovor_num,(select min(bill_num) from lsBill where period_id=b.period_id and client_id=b.client_id and bill_type=b.bill_type and receipt_id=";
      int index1 = 1;
      short receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
      string str4 = receiptId.ToString();
      strArray[index1] = str4;
      int index2 = 2;
      string str5 = ") as num,      if num is not null then 'Выписан' else 'Не выписан' endif as exist from lsClient ls left outer join lsBill b on b.client_id=ls.Client_id and b.period_id=";
      strArray[index2] = str5;
      int index3 = 3;
      string str6 = str1;
      strArray[index3] = str6;
      int index4 = 4;
      string str7 = str2;
      strArray[index4] = str7;
      int index5 = 5;
      string str8 = " and receipt_id=";
      strArray[index5] = str8;
      int index6 = 6;
      receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
      string str9 = receiptId.ToString();
      strArray[index6] = str9;
      int index7 = 7;
      string str10 = ",dcCompany c      ,di_str d,homes h,homelink hl,flats f,LsArenda a where ls.company_id=c.company_id and ls.complex_id=110 and ";
      strArray[index7] = str10;
      int index8 = 8;
      string str11 = str3;
      strArray[index8] = str11;
      int index9 = 9;
      string str12 = "     and hl.idhome=h.idhome and hl.codeu=ls.company_id and h.idhome=ls.idhome and h.idstr=d.idstr and ls.idflat=f.idflat and ls.client_id=a.client_id      and (select sum(rent) from lsRent where period_id=";
      strArray[index9] = str12;
      int index10 = 10;
      string str13 = str1;
      strArray[index10] = str13;
      int index11 = 11;
      string str14 = " and client_id=ls.client_id )<>0 ";
      strArray[index11] = str14;
      string str15 = string.Concat(strArray);
      if (this.chbOnlyNew.Checked)
        str15 += " and num is null";
      this.objectsList = this.session.CreateSQLQuery(str15 + " order by dogovor_num").List();
      this.dgvList.RowCount = this.objectsList.Count;
      this.dgvList.Refresh();
    }

    private void ShowArchiveList()
    {
    }

    private void UpdateData()
    {
      this.dtpDate.Value = KvrplHelper.LastDay(Options.Period.PeriodName.Value);
      DateTimePicker dtpSrok = this.dtpSrok;
      DateTime dateTime1 = Options.Period.PeriodName.Value;
      dateTime1 = dateTime1.AddMonths(1);
      DateTime dateTime2 = dateTime1.AddDays((double) this.days);
      dtpSrok.Value = dateTime2;
      this.txbNum.Clear();
      if ((int) this.level == 4)
      {
        IList<Bill> billList1 = this.session.CreateQuery(string.Format("from Bill where Period.PeriodId={0} and client_id={1} and BillType={2} and Receipt.ReceiptId={3}", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) this.billType, (object) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId)).List<Bill>();
        if (billList1.Count > 0)
        {
          this.txbNum.Text = billList1[0].BillNum.ToString();
          this.dtpDate.Value = billList1[0].BillDate;
          this.lblNum.Enabled = false;
          this.txbNum.Enabled = false;
          this.lblDate.Enabled = false;
          this.dtpDate.Enabled = false;
        }
        else
        {
          this.lblNum.Enabled = true;
          this.txbNum.Enabled = true;
          this.lblDate.Enabled = true;
          this.dtpDate.Enabled = true;
        }
        IList<Bill> billList2 = this.session.CreateQuery(string.Format("from Bill where Period.PeriodId={0} and client_id={1} and BillType=3 and Receipt.ReceiptId={3}", (object) Options.Period.PeriodId, (object) this.client.ClientId, (object) this.billType, (object) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId)).List<Bill>();
        if (billList2.Count > 0)
        {
          this.txbNumCor.Text = billList2[0].BillNum.ToString();
          this.chbCorrect.Enabled = false;
        }
        else
        {
          if ((int) this.billType != 2)
            return;
          this.chbCorrect.Enabled = true;
        }
      }
      else
      {
        this.lblNum.Visible = false;
        this.txbNum.Visible = false;
      }
    }

    private string Address(string org)
    {
      return "       (select (case uaind when '' then '' else uaind||', ' end)||                cast((if uastreet<>0 then                       (select (case isnull(s_area,'') when '' then '' else s_area||'.,' end)||                              (case isnull(s_region,'') when '' then '' else s_region||',' end)||                              (case isnull(s_city,'') when '' then '' else s_city||',' end)||                              (case isnull(s_np,'') when '' then '' else s_np||'.,' end)||' ' from di_str_all where idstr=uastreet)                     else                       (if isnull(uacity,0)<>0 then                         (select (case isnull(s_area,'') when '' then '' else s_area||'.,' end)||                                (case isnull(s_region,'') when '' then '' else s_region||',' end)||                                (case isnull(s_city,'') when '' then '' else s_city||',' end)||                                (case isnull(s_np,'') when '' then '' else s_np||'.,' end)||' '                         from (select idregion as id3,idprin as id2,isnull((select prinnumstr from di_soato_ru where numstr=id2),-1) as id1,                                      (select level from di_soato_ru where numstr=id2) as l2,(select level from di_soato_ru where numstr=id1) as l1,                                      (if urov=1 then r.region else (if l2=2 then (select region from di_region_all where idregion=id1) else (select region from di_region_all where idregion=id2) endif) endif) as s_area,                                      isnull((case urov when 1 then '' when 2 then r.region else (if l2=2 then (select region from di_region_all where idregion=id2) else '' endif) end),'') as s_region,                                      (if s_region='' then (if urov=3 then r.region else '' endif) else '' endif) as s_city,                                      (if s_region<>'' then (if urov=3 then r.region else '' endif) else '' endif) as s_np                                from di_region_all r) as itog where id3=uacity)                       else '' endif) endif) as char(100))||               (if uastreet<>0 then (select ulc from di_str_all where idstr=uastreet) else '' endif)||               (case uahouse when '' then '' else ', д.'||uahouse end)||               (case uakorp when '' then '' else ' ,корп.'||uakorp end)||               (case uaflat when '' then '' else ' ,кв.'||uaflat end)||               (case uadop when '' then '' else ' ,'||uadop end)         from base_org where idbaseorg=" + org + ")";
    }

    private string AddressPost(string client)
    {
      return this.session.CreateSQLQuery("       select (case paind when '' then '' else paind||', ' end)||                cast((if pastreet<>0 then                       (select (case isnull(s_area,'') when '' then '' else s_area||'.,' end)||                              (case isnull(s_region,'') when '' then '' else s_region||',' end)||                              (case isnull(s_city,'') when '' then '' else s_city||',' end)||                              (case isnull(s_np,'') when '' then '' else s_np||'.,' end)||' ' from di_str_all where idstr=pastreet)                     else                       (if isnull(pacity,0)<>0 then                         (select (case isnull(s_area,'') when '' then '' else s_area||'.,' end)||                                (case isnull(s_region,'') when '' then '' else s_region||',' end)||                                (case isnull(s_city,'') when '' then '' else s_city||',' end)||                                (case isnull(s_np,'') when '' then '' else s_np||'.,' end)||' '                         from (select idregion as id3,idprin as id2,isnull((select prinnumstr from di_soato_ru where numstr=id2),-1) as id1,                                      (select level from di_soato_ru where numstr=id2) as l2,(select level from di_soato_ru where numstr=id1) as l1,                                      (if urov=1 then r.region else (if l2=2 then (select region from di_region_all where idregion=id1) else (select region from di_region_all where idregion=id2) endif) endif) as s_area,                                      isnull((case urov when 1 then '' when 2 then r.region else (if l2=2 then (select region from di_region_all where idregion=id2) else '' endif) end),'') as s_region,                                      (if s_region='' then (if urov=3 then r.region else '' endif) else '' endif) as s_city,                                      (if s_region<>'' then (if urov=3 then r.region else '' endif) else '' endif) as s_np                                from di_region_all r) as itog where id3=pacity)                       else '' endif) endif) as char(100))||               (if pastreet<>0 then (select ulc from di_str_all where idstr=pastreet) else '' endif)||               (case pahouse when '' then '' else ', д.'||pahouse end)||               (case pakorp when '' then '' else ' ,корп.'||pakorp end)||               (case paflat when '' then '' else ' ,кв.'||paflat end)||               (case padop when '' then '' else ' ,'||padop end)         from LsArenda la            left outer join base_org borg on la.idbaseorg=borg.idbaseorg where la.client_id=" + client).List()[0].ToString();
    }

    private string AddressLegal(string client)
    {
      return this.session.CreateSQLQuery("       select (case uaind when '' then '' else uaind||', ' end)||                cast((if uastreet<>0 then                       (select (case isnull(s_area,'') when '' then '' else s_area||'.,' end)||                              (case isnull(s_region,'') when '' then '' else s_region||',' end)||                              (case isnull(s_city,'') when '' then '' else s_city||',' end)||                              (case isnull(s_np,'') when '' then '' else s_np||'.,' end)||' ' from di_str_all where idstr=uastreet)                     else                       (if isnull(uacity,0)<>0 then                         (select (case isnull(s_area,'') when '' then '' else s_area||'.,' end)||                                (case isnull(s_region,'') when '' then '' else s_region||',' end)||                                (case isnull(s_city,'') when '' then '' else s_city||',' end)||                                (case isnull(s_np,'') when '' then '' else s_np||'.,' end)||' '                         from (select idregion as id3,idprin as id2,isnull((select prinnumstr from di_soato_ru where numstr=id2),-1) as id1,                                      (select level from di_soato_ru where numstr=id2) as l2,(select level from di_soato_ru where numstr=id1) as l1,                                      (if urov=1 then r.region else (if l2=2 then (select region from di_region_all where idregion=id1) else (select region from di_region_all where idregion=id2) endif) endif) as s_area,                                      isnull((case urov when 1 then '' when 2 then r.region else (if l2=2 then (select region from di_region_all where idregion=id2) else '' endif) end),'') as s_region,                                      (if s_region='' then (if urov=3 then r.region else '' endif) else '' endif) as s_city,                                      (if s_region<>'' then (if urov=3 then r.region else '' endif) else '' endif) as s_np                                from di_region_all r) as itog where id3=uacity)                       else '' endif) endif) as char(100))||               (if uastreet<>0 then (select ulc from di_str_all where idstr=uastreet) else '' endif)||               (case uahouse when '' then '' else ', д.'||uahouse end)||               (case uakorp when '' then '' else ' ,корп.'||uakorp end)||               (case uaflat when '' then '' else ' ,кв.'||uaflat end)||               (case uadop when '' then '' else ' ,'||uadop end)         from LsArenda la            left outer join base_org borg on la.idbaseorg=borg.idbaseorg where la.client_id=" + client).List()[0].ToString();
    }

    private string MainUsl()
    {
      string str = " c.rnn_id=" + this.raion.IdRnn.ToString();
      if ((int) this.level > 1)
        str = str + " and ls.company_id=" + this.company.CompanyId.ToString();
      if ((int) this.level > 2)
        str = str + " and ls.idhome=" + this.home.IdHome.ToString();
      if ((int) this.level > 3)
        str = str + " and ls.client_id=" + this.client.ClientId.ToString();
      return str;
    }

    private void ReportLoad(Report report, int idCompany, int typeBill)
    {
      int num = 0;
      string path = "";
      foreach (string str in this.FileListReport)
      {
        try
        {
          if (Convert.ToInt32(str) == idCompany)
            num = idCompany;
        }
        catch
        {
        }
      }
      string str1 = "\\" + (object) num;
      switch (typeBill)
      {
        case 1:
          path = Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\Bill.frx";
          break;
        case 2:
          path = Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\Invoice.frx";
          break;
        case 3:
          string str2 = Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\StatementKor.frx";
          path = Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\Statement.frx";
          break;
        case 4:
          path = Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\Notice.frx";
          break;
        case 5:
          path = Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\BillWD.frx";
          break;
        case 6:
          path = Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\InvoiceWD.frx";
          break;
        case 11:
          path = Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\Offer_FR.frx";
          break;
        case 12:
          path = Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\Offer_FR2.frx";
          break;
        case 13:
          path = Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\Offer_FR3.frx";
          break;
        case 14:
          path = Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\Offer_FR4.frx";
          break;
        case 21:
          path = Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\CorrectInvoice.frx";
          break;
        case 31:
          path = Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\StatementCorrect.frx";
          break;
      }
      if (!File.Exists(path))
        str1 = "";
      switch (typeBill)
      {
        case 1:
          switch (this.city)
          {
            case 1:
              report.Load(Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\Bill.frx");
              break;
            case 23:
              report.Load(Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\BillKor.frx");
              break;
            default:
              report.Load(Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\Bill.frx");
              break;
          }
          if ((uint) this.cbShablon.SelectedIndex <= 0U)
            break;
          report.Load(Options.CurrentDomainPath + this.cbShablon.SelectedValue);
          if (this.cbShablon.SelectedValue.Equals((object) ("Шаблоны_ар" + str1 + "\\BillKor.frx")))
            this._korTamplate = true;
          break;
        case 2:
          report.Load(Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\Invoice.frx");
          break;
        case 3:
          if (this.city == 23)
            report.Load(Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\StatementKor.frx");
          else
            report.Load(Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\Statement.frx");
          if ((uint) this.cbAkt.SelectedIndex <= 0U)
            break;
          report.Load(Options.CurrentDomainPath + this.cbAkt.SelectedValue);
          break;
        case 4:
          report.Load(Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\Notice.frx");
          break;
        case 5:
          report.Load(Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\BillWD.frx");
          break;
        case 6:
          report.Load(Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\InvoiceWD.frx");
          break;
        case 11:
          report.Load(Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\Offer_FR.frx");
          break;
        case 12:
          report.Load(Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\Offer_FR2.frx");
          break;
        case 13:
          report.Load(Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\Offer_FR3.frx");
          break;
        case 14:
          report.Load(Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\Offer_FR4.frx");
          break;
        case 21:
          report.Load(Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\CorrectInvoice.frx");
          break;
        case 31:
          report.Load(Options.CurrentDomainPath + "Шаблоны_ар" + str1 + "\\StatementCorrect.frx");
          if ((uint) this.cbAkt.SelectedIndex <= 0U)
            break;
          report.Load(Options.CurrentDomainPath + this.cbAkt.SelectedValue);
          break;
        case 101:
          report.Load(Options.CurrentDomainPath + "Шаблоны_ар\\dept" + str1 + "\\Bill.frx");
          break;
        case 102:
          report.Load(Options.CurrentDomainPath + "Шаблоны_ар\\dept" + str1 + "\\Invoice.frx");
          break;
        case 103:
          report.Load(Options.CurrentDomainPath + "Шаблоны_ар\\dept" + str1 + "\\Statement.frx");
          break;
      }
    }

    private void ShowCorrect(bool print, string strPeriod, string strPeriodId, string ls, FastReport.PrintSettings settings, int lsClientArenda, bool many)
    {
      Options.Period.PeriodName.Value.AddMonths(1);
      string baseFormat1 = KvrplHelper.DateToBaseFormat(KvrplHelper.FirstDay(Options.Period.PeriodName.Value));
      string baseFormat2 = KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(Options.Period.PeriodName.Value));
      string str1 = Options.Login + ".tmpReceipt";
      this.ReportLoad(this.report, this.idBaseOrg, 21);
      this.report.Dictionary.Connections[0].ConnectionString = this.connectionString;
      Home home = this.session.CreateQuery("select l.Home from LsClient l where l.ClientId=:la").SetParameter<int>("la", lsClientArenda).UniqueResult<Home>();
      this._main = this.report.GetDataSource("Main") as TableDataSource;
      string str2 = "select dogovor_num,dogovor_date,ls.client_id,c.company_id,r.receipt_id,r.seller_id,       borg.nameorg_min as buyer,borg.idbaseorg as idbuyer,       " + this.Address("borg.idbaseorg") + " as adrbuyer,(if (r.seller_id is not null and r.seller_id<>0) then " + this.Address("r.seller_id") + " else '' endif) as adrseller,       borg.inn,borg.kpp,if b.month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=b.month_id) endif as period,b.month_id as month_id,       (select sum(rent) from lsRent lb," + str1 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + " and lb.client_id=prc.client_id and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and code<>0) as buyerpast,        (select sum(rent_vat) from lsRent lb," + str1 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + " and lb.client_id=prc.client_id and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and code<>0) as buyervatpast,        (select bill_num from lsBill where period_id=" + strPeriodId + " and client_id=ls.client_id and bill_type=3 and receipt_id=" + ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString() + ") as numcorrect,        (select bill_date from lsBill where period_id=" + strPeriodId + " and client_id=ls.client_id and bill_type=3 and receipt_id=" + ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString() + ")  as datecorrect,        (select first bill_num from lsBill where period_id=b.month_id and client_id=ls.client_id and bill_type=2 and receipt_id=" + ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString() + ") as numbef,        (select first bill_date from lsBill where period_id=b.month_id and client_id=ls.client_id and bill_type=2 and receipt_id=" + ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString() + ") as datebef,        (if (r.seller_id is not null and r.seller_id<>0) then (select nameorg from base_org where idbaseorg=r.seller_id) else '-----' endif) as nameseller,        (if (r.seller_id is not null and r.seller_id<>0) then (select nameorg_min from base_org where idbaseorg=r.seller_id) else '' endif) as nameminseller,        (select inn from base_org where idbaseorg=r.seller_id) as innseller,       (select kpp from base_org where idbaseorg=r.seller_id) as kppseller from lsClient ls,dcCompany c,LsArenda la left outer join base_org borg on la.idbaseorg=borg.idbaseorg left outer join di_bank db on db.idbank=borg.bank,lsBill b,cmpReceipt r where ls.company_id=c.company_id and la.Client_id=ls.Client_id and ls.complex_id=110 and b.client_id=ls.Client_id and r.company_id=c.company_id and b.period_id=" + strPeriodId + " and b.bill_type=3 and ls.client_id=" + ls + " and r.receipt_id in (" + ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString() + ") and b.receipt_id=r.receipt_id";
      if (this.chbMec.Checked)
      {
        string[] strArray = new string[37]{ "select dogovor_num,dogovor_date,ls.client_id,c.company_id,r.receipt_id,r.seller_id,       borg.nameorg_min as buyer,borg.idbaseorg as idbuyer,       ", this.Address("borg.idbaseorg"), " as adrbuyer,(if (r.seller_id is not null and r.seller_id<>0) then ", this.Address("r.seller_id"), " else '' endif) as adrseller,       borg.inn,borg.kpp,if b.month_id = 0 then ", strPeriod, " else (select period_value from dcPeriod where period_id=b.month_id) endif as period,b.month_id as month_id,       (select sum(rent) from lsRent lb,", str1, "printserv prc where lb.client_id=ls.client_id and period_id=", strPeriodId, " and lb.client_id=prc.client_id and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and code<>0 and lb.month_id=b.month_id) as buyerpast,        (select sum(rent_vat) from lsRent lb,", str1, "printserv prc where lb.client_id=ls.client_id and period_id=", strPeriodId, " and lb.client_id=prc.client_id and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and code<>0 and lb.month_id=b.month_id) as buyervatpast, (select bill_num from lsBill where b.month_id=month_id and period_id=", strPeriodId, " and client_id=ls.client_id and bill_type=3 and receipt_id=", ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString(), ") as numcorrect,        (select bill_date from lsBill where b.month_id=month_id and period_id=", strPeriodId, " and client_id=ls.client_id and bill_type=3 and receipt_id=", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
        int index1 = 21;
        short receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
        string str3 = receiptId.ToString();
        strArray[index1] = str3;
        int index2 = 22;
        string str4 = ")  as datecorrect,        (select first bill_num from lsBill where  b.month_id=month_id and period_id=";
        strArray[index2] = str4;
        int index3 = 23;
        string str5 = strPeriodId;
        strArray[index3] = str5;
        int index4 = 24;
        string str6 = "  and client_id=ls.client_id and bill_type=2 and receipt_id=";
        strArray[index4] = str6;
        int index5 = 25;
        receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
        string str7 = receiptId.ToString();
        strArray[index5] = str7;
        int index6 = 26;
        string str8 = ") as numbef,        (select first bill_date from lsBill where b.month_id=month_id and period_id=";
        strArray[index6] = str8;
        int index7 = 27;
        string str9 = strPeriodId;
        strArray[index7] = str9;
        int index8 = 28;
        string str10 = "  and client_id=ls.client_id and bill_type=2 and receipt_id=";
        strArray[index8] = str10;
        int index9 = 29;
        receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
        string str11 = receiptId.ToString();
        strArray[index9] = str11;
        int index10 = 30;
        string str12 = ") as datebef,        (if (r.seller_id is not null and r.seller_id<>0) then (select nameorg from base_org where idbaseorg=r.seller_id) else '-----' endif) as nameseller,        (if (r.seller_id is not null and r.seller_id<>0) then (select nameorg_min from base_org where idbaseorg=r.seller_id) else '' endif) as nameminseller,        (select inn from base_org where idbaseorg=r.seller_id) as innseller,       (select kpp from base_org where idbaseorg=r.seller_id) as kppseller from lsClient ls,dcCompany c,LsArenda la left outer join base_org borg on la.idbaseorg=borg.idbaseorg left outer join di_bank db on db.idbank=borg.bank,lsBill b,cmpReceipt r where ls.company_id=c.company_id and la.Client_id=ls.Client_id and ls.complex_id=110 and b.client_id=ls.Client_id and r.company_id=c.company_id and b.period_id=";
        strArray[index10] = str12;
        int index11 = 31;
        string str13 = strPeriodId;
        strArray[index11] = str13;
        int index12 = 32;
        string str14 = " and b.bill_type=3 and ls.client_id=";
        strArray[index12] = str14;
        int index13 = 33;
        string str15 = ls;
        strArray[index13] = str15;
        int index14 = 34;
        string str16 = " and r.receipt_id in (";
        strArray[index14] = str16;
        int index15 = 35;
        receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
        string str17 = receiptId.ToString();
        strArray[index15] = str17;
        int index16 = 36;
        string str18 = ") and b.receipt_id=r.receipt_id";
        strArray[index16] = str18;
        str2 = string.Concat(strArray);
      }
      this._main.SelectCommand = str2;
      string str19 = "select (select client_id from lsClient where client_id=:client_id) as client_id,r.service_id,p.period_value,t.isvat, ";
      string str20;
      string str21;
      string str22;
      string str23;
      string str24;
      if (!this.chbSost.Checked)
      {
        str20 = "and b.service_id in (select service_id from dcService where root=r.service_id)";
        str21 = "";
        str22 = "and s.service_id=t.service_id";
        str23 = "and s.service_id=t.service_id";
        str24 = "and service_id in (select service_id from " + str1 + "printserv prc where client_id=:client_id and root=r.service_id)";
        if (this.chbMec.Checked)
          str20 = "and b.service_id in (select service_id from dcService where root=r.service_id) and b.month_id=:month_id";
      }
      else
      {
        str19 += "ds.service_id as sost,";
        str20 = "and b.service_id=ds.service_id";
        str21 = " left outer join dcService ds on r.service_id=ds.root";
        str22 = "and ds.service_id=t.service_id";
        str23 = "and ds.root=t.service_id";
        str24 = "and service_id in (select service_id from " + str1 + "printserv prc where client_id=b.client_id and root=r.service_id and service_id=b.service_id)";
      }
      string str25 = str19 + "         (if gr=0 then (if isnull((select unitmeasuring_name from dcUnitMeasuring where unitmeasuring_id=t.unitmeasuring_id),'')<>'' then          (select unitmeasuring_name from dcUnitMeasuring where unitmeasuring_id=t.unitmeasuring_id) else          (select unitmeasuring_name from dcUnitMeasuring where unitmeasuring_id=(select first unitmeasuring_id from cmpTariff ctr where service_id=r.service_id)) endif)          else if t.unitmeasuring_id is not null then (select unitmeasuring_name from dcUnitMeasuring where unitmeasuring_id=t.unitmeasuring_id)          else (select unitmeasuring_name from dcUnitMeasuring where unitmeasuring_id=(select first unitmeasuring_id from cmpTariff ctr where service_id=gr)) endif endif) as edizm,          (if (" + this.city.ToString() + "<>1 or t.service_id not in (71,72,73,74,78)) then isnull((select n.norm_value from cmpNorm n where s.norm_id=n.norm_id and n.company_id=(select param_value from cmpParam where company_id=r.company_id and period_id=0 and param_id=204 and dbeg<=cast('" + baseFormat1 + "' as date) and dend>=cast('" + baseFormat1 + "' as date))         and n.dbeg<=cast('" + baseFormat1 + "' as date) and n.dend>=cast('" + baseFormat1 + "' as date) and n.period_id=0),0) else 0 endif) as norm,          (select group_num from cmpServiceParam where company_id=r.company_id and complex_id=110 and service_id=r.service_id) as gr,          isnull((select param_value from lsParam where period_id=0 and client_id=:client_id and dbeg<=" + strPeriod + " and dend>=" + strPeriod + " and param_id=104),0) as doc,         isnull((if t.service_id not in (20,71,72,73,74,78) then (if doc not in (2,5,6,7,8,12,14,15,17,18,21,25,28,29,31,33,34) then (if r.service_id=3 then                    (select sum(cost) from cmpTariff b where company_id=t.company_id and period_id=0 and tariff_id=s.tariff_id and dbeg=t.dbeg and dend=t.dend                    " + str23 + " and scheme<>3) else                          (if t.scheme<>129 then t.cost else 0 endif)endif) else t.cost endif)                   else 0 endif),0) as tax,         (if (select first basetariff_id from lsService ls,cmpTariff ct where ls.client_id=:client_id and ls.service_id=r.service_id and ls.service_id=ct.service_id               and ls.tariff_id=ct.tariff_id and ls.dbeg<=" + strPeriod + " and ls.dend>=" + strPeriod + " and ls.period_id=0 and ct.period_id=0 and ct.company_id=r.company_id and ct.dbeg<=" + strPeriod + " and ct.dend>=" + strPeriod + ")=2 then " + "              (if (select group_num from cmpServiceParam where company_id=:company_id and complex_id=110 and service_id=r.service_id)=0 then                       isnull((select max(volume) from lsRent b where period_id=" + strPeriodId + " and client_id=:client_id " + str24 + "),0) else 0 endif)" + "                     else (if :month_id =0 then                       isnull((select max(volume) from lsRent b where period_id=" + strPeriodId + " and client_id=:client_id                       " + str24 + " and code=0),0)                       else isnull((select max(volume) from lsRent b where period_id=" + strPeriodId + " and client_id=:client_id                       " + str24 + " and month_id=:month_id),0) endif) endif) as volume" + ",if not exists(select * from lsBill where period_id=" + strPeriodId + "-1 and client_id=:client_id and bill_type=3) then isnull((select sum(isnull(rent,0)) from lsRent b," + str1 + "printserv prc where b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + "-1 and b.client_id=:client_id  " + str20 + "),0) else isnull((select sum(isnull(rent,0)) from lsRent b," + str1 + "printserv prc where b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + "-1 and b.client_id=:client_id  " + str20 + " and code=0),0) endif as calcbef,isnull((select sum(isnull(rent,0)) from lsRent b," + str1 + "printserv prc where b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id  " + str20 + " and code<>0),0) as past, if not exists(select * from lsBill where period_id=" + strPeriodId + "-1 and client_id=:client_id and bill_type=3) then isnull((select sum(isnull(rent_vat,0)) from lsRent b," + str1 + "printserv prc where b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + "-1 and b.client_id=:client_id  " + str20 + "),0) else isnull((select sum(isnull(rent_vat,0)) from lsRent b," + str1 + "printserv prc where b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + "-1 and b.client_id=:client_id  " + str20 + " and code=0),0) endif as vatbef,isnull((select sum(isnull(rent_vat,0)) from lsRent b," + str1 + "printserv prc where b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id  " + str20 + " and code<>0),0) as vatpast, (select first tax from cmpTariff t where s.tariff_id=t.tariff_id " + str23 + " and t.service_id=r.service_id and t.company_id=(select param_value from cmpParam where company_id=r.company_id and period_id=0 and param_id=201 and dbeg<=cast('" + baseFormat1 + "' as date) and dend>=cast('" + baseFormat1 + "' as date))          and t.period_id=0 and s.complex_id=t.complex_id           and t.dbeg<=isnull((select first dbeg from cmpTariff tt where s.tariff_id=tt.tariff_id and s.service_id=tt.service_id and tt.company_id=t.company_id and tt.complex_id=100 and tt.period_id=0                 and ((dbeg<=cast('" + baseFormat1 + "' as date) and dend>=cast('" + baseFormat1 + "' as date)) or (dbeg<=cast('" + baseFormat2 + "' as date) and dend>=cast('" + baseFormat2 + "' as date)))),cast('" + baseFormat1 + "' as date))          and t.dend>=isnull((select first dbeg from cmpTariff tt where s.tariff_id=tt.tariff_id and s.service_id=tt.service_id and tt.company_id=t.company_id and tt.complex_id=100 and tt.period_id=0                 and ((dbeg<=cast('" + baseFormat1 + "' as date) and dend>=cast('" + baseFormat1 + "' as date)) or (dbeg<=cast('" + baseFormat2 + "' as date) and dend>=cast('" + baseFormat2 + "' as date)))),cast('" + baseFormat1 + "' as date))) as taxbef, (select first isvat from cmpTariff t where s.tariff_id=t.tariff_id " + str23 + " and t.service_id=r.service_id and t.company_id=(select param_value from cmpParam where company_id=r.company_id and period_id=0 and param_id=201 and dbeg<=cast('" + baseFormat1 + "' as date) and dend>=cast('" + baseFormat1 + "' as date))          and t.period_id=0 and s.complex_id=t.complex_id           and t.dbeg<=isnull((select first dbeg from cmpTariff tt where s.tariff_id=tt.tariff_id and s.service_id=tt.service_id and tt.company_id=t.company_id and tt.complex_id=100 and tt.period_id=0                 and ((dbeg<=cast('" + baseFormat1 + "' as date) and dend>=cast('" + baseFormat1 + "' as date)) or (dbeg<=cast('" + baseFormat2 + "' as date) and dend>=cast('" + baseFormat2 + "' as date)))),cast('" + baseFormat1 + "' as date))          and t.dend>=isnull((select first dbeg from cmpTariff tt where s.tariff_id=tt.tariff_id and s.service_id=tt.service_id and tt.company_id=t.company_id and tt.complex_id=100 and tt.period_id=0                 and ((dbeg<=cast('" + baseFormat1 + "' as date) and dend>=cast('" + baseFormat1 + "' as date)) or (dbeg<=cast('" + baseFormat2 + "' as date) and dend>=cast('" + baseFormat2 + "' as date)))),cast('" + baseFormat1 + "' as date))) as isvatbef from dcPeriod p,cmpServiceParam r " + str21 + " left outer join lsService s on r.service_id=s.service_id and s.client_id=:client_id          and s.dbeg<=isnull((select first dbeg from lsService ss where r.service_id=ss.service_id and ss.client_id=:client_id and ss.complex_id=100 and ss.period_id=0                 and ((dbeg<=cast('" + baseFormat1 + "' as date) and dend>=cast('" + baseFormat1 + "' as date)) or (dbeg<=cast('" + baseFormat2 + "' as date) and dend>=cast('" + baseFormat1 + "' as date)))),cast('" + baseFormat1 + "' as date))          and s.dend>=isnull((select first dbeg from lsService ss where r.service_id=ss.service_id and ss.client_id=:client_id and ss.complex_id=100 and ss.period_id=0                 and ((dbeg<=cast('" + baseFormat1 + "' as date) and dend>=cast('" + baseFormat1 + "' as date)) or (dbeg<=cast('" + baseFormat2 + "' as date) and dend>=cast('" + baseFormat2 + "' as date)))),cast('" + baseFormat1 + "' as date))          and s.complex_id=100 and s.period_id=0           left outer join cmpTariff t on s.tariff_id=t.tariff_id " + str22 + " and t.company_id=(select param_value from cmpParam where company_id=r.company_id and period_id=0 and param_id=201 and dbeg<=cast('" + baseFormat1 + "' as date) and dend>=cast('" + baseFormat1 + "' as date))          and t.period_id=0 and s.complex_id=t.complex_id           and t.dbeg<=isnull((select first dbeg from cmpTariff tt where s.tariff_id=tt.tariff_id and s.service_id=tt.service_id and tt.company_id=t.company_id and tt.complex_id=100 and tt.period_id=0                 and ((dbeg<=cast('" + baseFormat1 + "' as date) and dend>=cast('" + baseFormat1 + "' as date)) or (dbeg<=cast('" + baseFormat2 + "' as date) and dend>=cast('" + baseFormat2 + "' as date)))),cast('" + baseFormat1 + "' as date))          and t.dend>=isnull((select first dbeg from cmpTariff tt where s.tariff_id=tt.tariff_id and s.service_id=tt.service_id and tt.company_id=t.company_id and tt.complex_id=100 and tt.period_id=0                 and ((dbeg<=cast('" + baseFormat1 + "' as date) and dend>=cast('" + baseFormat1 + "' as date)) or (dbeg<=cast('" + baseFormat2 + "' as date) and dend>=cast('" + baseFormat2 + "' as date)))),cast('" + baseFormat1 + "' as date))where r.company_id=:company_id and r.complex_id=110 and p.period_id=" + strPeriodId + " ";
      string str26;
      if (!this.chbSost.Checked)
        str26 = "select my.client_id,sp.service_id,sp.printshow as sname,sp.sorter,sp.group_num, sp.receipt_id, sum(my.tax) as tax,my.edizm,my.isvat,sum(my.norm) as quota, sum(my.volume) as volume,          sum(calcbef) as calcbef,sum(past) as past,sum(my.taxbef) as taxbef,(if isnull(taxbef,0)<>0 then calcbef/taxbef else 0 endif) as volumebef,          (if isnull(tax,0)<>0 then (calcbef+past)/tax else 0 endif) as volumepast,my.isvatbef,sum(my.vatbef) as vatbef,sum(my.vatpast) as vatpast from (" + str25 + ") as my, cmpServiceParam sp where sp.company_id=:company_id and sp.complex_id=110 and (my.service_id=sp.service_id or sp.service_id=my.gr)         and (group_num=0 or sp.calcalone=1)   and ( (sp.receipt_id=:receipt_id) or ( (select count(*) from cmphmReceipt hr, cmpServiceParam sp,(" + str25 + ") as my where sp.company_id=:company_id and sp.complex_id=110 and (my.service_id=sp.service_id or sp.service_id=my.gr)         and (group_num=0 or sp.calcalone=1)  and  hr.supplier_id=(select supplier_id from " + str1 + "printserv prc where sp.service_id=prc.root group by supplier_id) and hr.IdHome=" + (object) home.IdHome + " and hr.receipt_id=:receipt_id and hr.company_id=:company_id and hr.complex_id=110 and (hr.dbeg<=cast('" + baseFormat1 + "' as date)) and dend>=cast('" + baseFormat2 + "' as date)) > 0 )   or  ( (select count(*) from cmphmReceipt hr, cmpServiceParam sp,(" + str25 + ") as my where sp.company_id=:company_id and sp.complex_id=110 and (my.service_id=sp.service_id or sp.service_id=my.gr)         and (group_num=0 or sp.calcalone=1) and hr.supplier_id=(select supplier_id from " + str1 + "printserv prc where sp.service_id=prc.root group by supplier_id) and hr.IdHome=0 and hr.receipt_id=:receipt_id and hr.company_id=:company_id and hr.complex_id=110 and (hr.dbeg<=cast('" + baseFormat1 + "' as date)) and dend>=cast('" + baseFormat2 + "' as date)) > 0  ) ) group by my.client_id,sp.service_id,sp.printshow,sp.group_num,sp.receipt_id,edizm,sp.sorter,my.isvat,my.isvatbef having past<>0 order by client_id,sp.sorter ";
      else
        str26 = "select my.client_id,sp.service_id,(select service_name from dcService where service_id=my.sost) as sname,sp.sorter,sp.group_num,sum(my.tax) as tax,my.edizm,my.isvat,sum(my.norm) as quota,           sum(calcbef) as calcbef,sum(past) as past,sum(my.taxbef) as taxbef,(if isnull(taxbef,0)<>0 then calcbef/taxbef else 0 endif) as volumebef,          (if isnull(tax,0)<>0 then (calcbef+past)/tax else 0 endif) as volumepast,my.isvatbef,sum(my.vatbef) as vatbef,sum(my.vatpast) as vatpast from (" + str25 + ") as my,cmpServiceParam sp where sp.company_id=:company_id and sp.complex_id=110 and my.service_id=sp.service_id   and ( (sp.receipt_id=:receipt_id) or ( (select count(*) from cmphmReceipt hr, cmpServiceParam sp,(" + str25 + ") as my where sp.company_id=:company_id and sp.complex_id=110 and (my.service_id=sp.service_id or sp.service_id=my.gr)         and (group_num=0 or sp.calcalone=1)  and  hr.supplier_id=(select supplier_id from " + str1 + "printserv prc where sp.service_id=prc.root group by supplier_id) and hr.IdHome=" + (object) home.IdHome + " and hr.receipt_id=:receipt_id and hr.company_id=:company_id and hr.complex_id=110 and (hr.dbeg<=cast('" + baseFormat1 + "' as date)) and dend>=cast('" + baseFormat2 + "' as date)) > 0 )   or  ( (select count(*) from cmphmReceipt hr, cmpServiceParam sp,(" + str25 + ") as my where sp.company_id=:company_id and sp.complex_id=110 and (my.service_id=sp.service_id or sp.service_id=my.gr)         and (group_num=0 or sp.calcalone=1) and hr.supplier_id=(select supplier_id from " + str1 + "printserv prc where sp.service_id=prc.root group by supplier_id) and hr.IdHome=0 and hr.receipt_id=:receipt_id and hr.company_id=:company_id and hr.complex_id=110 and (hr.dbeg<=cast('" + baseFormat1 + "' as date)) and dend>=cast('" + baseFormat2 + "' as date)) > 0  ) ) group by my.client_id,sp.service_id,my.sost,sp.printshow,sp.group_num,edizm,sp.sorter,my.isvat,my.isvatbef having past<>0 order by client_id,sp.sorter,my.sost ";
      this._tarif = this.report.GetDataSource("Tarif") as TableDataSource;
      this._tarif.SelectCommand = str26;
      try
      {
        this.report.SetParameterValue("Lastopl", (object) this.dtpSrok.Value.ToShortDateString());
        this.report.SetParameterValue("ruk", (object) this.rukName);
        this.report.SetParameterValue("bux", (object) this.buhName);
        for (int index = 0; (Decimal) index < this.nudCountBill.Value; ++index)
        {
          if (many)
          {
            List<ReportArenda> reportList = this._reportList;
            ReportArenda reportArenda = new ReportArenda();
            reportArenda.LsArenda = lsClientArenda;
            reportArenda.BillType = 3;
            Report report = this.report;
            reportArenda.Report = report;
            int periodId = Options.Period.PeriodId;
            reportArenda.PeriodId = periodId;
            reportList.Add(reportArenda);
          }
          else
          {
            if (settings == null)
              settings = this.report.PrintSettings;
            this.SaveOrPrintOrForEmail(this.report, lsClientArenda, Options.Period.PeriodId, 3, print, settings, false);
          }
        }
      }
      catch
      {
      }
    }

    private void PrintOrShowReports(bool print)
    {
      try
      {
        if (new List<int>() { 70, 71, 72, 73, 74, 75, 76, 77, 78, 79 }.Contains((int) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId))
        {
          string str1 = this.MainUsl();
          string str2 = "";
          this.report = new Report();
          PrintSettings source = (PrintSettings) null;
          DateTime dt = Options.Period.PeriodName.Value.AddMonths(-Options.Period.PeriodName.Value.Month + 1);
          string str3 = "cast('" + KvrplHelper.DateToBaseFormat(this.dtpDate.Value).ToString() + "' as date) ";
          string str4 = (int) this.billType != 1 ? " and bill_type in (2,3)" : " and bill_type=1";
          if (!this.txbNum.Enabled || this.txbNum.Text == "")
          {
            str2 = "isnull((select max(bill_num) from lsBill where period_id>=(select period_id from dcPeriod where period_value=cast('" + KvrplHelper.DateToBaseFormat(dt).ToString() + "' as date)) " + str4 + "),0)+number()";
          }
          else
          {
            try
            {
              Convert.ToInt32(this.txbNum.Text);
              str2 = this.txbNum.Text + ",";
            }
            catch
            {
              int num = (int) MessageBox.Show("Некорректный номер счета!", "Внимание", MessageBoxButtons.OK);
            }
          }
          this.session.CreateSQLQuery(string.Format("insert into lsBill select ls.client_id, {0},0,{1},{2},{3}, user, today(),{4} from lsClient ls  inner join LsArenda ar on ar.client_id=ls.client_id inner join dcCompany c on ls.company_id=c.company_id inner join lsWorkDistribute wd on wd.Client_id=ls.Client_id and wd.Period_id={0} where {5} and ls.client_id not in (select client_id from lsBill where period_id={0} and bill_type={1} and receipt_id={4})", (object) Options.Period.PeriodId, (object) this.billType.ToString(), (object) str2, (object) str3, (object) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString(), (object) str1)).ExecuteUpdate();
          foreach (object[] objArray in (IEnumerable) this.session.CreateSQLQuery(string.Format("select bil.client_id,ar.dogovor_num from lsBill bil  inner join lsClient ls on bil.client_id=ls.client_id inner join LsArenda ar on ar.client_id=bil.client_id inner join dcCompany c on ls.company_id=c.company_id inner join lsWorkDistribute wd on wd.Client_id=ls.Client_id and wd.Period_id={0} where bil.period_id={0} and bill_type={1} and receipt_id={2} and {3}order by ar.dogovor_num", (object) Options.Period.PeriodId, (object) this.billType.ToString(), (object) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString(), (object) str1)).List())
          {
            this.client = this.session.Get<LsClient>((object) Convert.ToInt32(objArray[0]));
            if (this.company == null)
              this.company = this.client.Company;
            if (this.rbBill.Checked)
            {
              this.company = this.client.Company;
              this.ReportLoad(this.report, this.idBaseOrg, 5);
              this.LoadReportTemplateAndSetParameters(Options.CurrentDomainPath + "Шаблоны_ар\\BillWD.frx");
              if (print)
              {
                if (source == null)
                {
                  this.report.Prepare();
                  this.report.Print();
                  source = this.report.PrintSettings;
                }
                else
                {
                  source.ShowDialog = false;
                  this.report.PrintSettings.Assign(source);
                  this.report.Prepare();
                  this.report.Print();
                }
              }
              else
                this.report.Show();
            }
            if (this.rbInvoice.Checked)
            {
              this.ReportLoad(this.report, this.idBaseOrg, 6);
              this.LoadReportTemplateAndSetParameters(Options.CurrentDomainPath + "Шаблоны_ар\\InvoiceWD.frx");
              if (print)
              {
                if (source == null)
                {
                  this.report.Prepare();
                  this.report.Print();
                  source = this.report.PrintSettings;
                }
                else
                {
                  source.ShowDialog = false;
                  this.report.PrintSettings.Assign(source);
                  this.report.Prepare();
                  this.report.Print();
                }
              }
              else
                this.report.Show();
            }
          }
          this.ShowList();
        }
        else
        {
          this.session.Clear();
          string str1 = "tmpReceipt";
          string str2 = Options.Login + ".tmpReceipt";
          string str3 = "";
          string str4 = "";
          string str5 = "";
          string str6 = "";
          string str7 = "";
          string str8 = "";
          string queryString1 = "";
          IList list1 = (IList) new ArrayList();
          Options.Period.PeriodName.Value.AddMonths(1);
          DateTime dt = Options.Period.PeriodName.Value.AddMonths(-Options.Period.PeriodName.Value.Month + 1);
          string str9 = "";
          string strPeriod = "cast( '" + KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value).ToString() + "'as date)";
          string strPeriodId = Options.Period.PeriodId.ToString();
          string str10 = "(if :month_id=0 or not exists(select * from lsService where period_id=" + strPeriodId + " and client_id=:client_id and service_id=r.service_id and dbeg<=(select period_value from dcPeriod where period_id=(:month_id)) and dend>=(select period_value from dcPeriod where period_id=:month_id)) then 0 else " + strPeriodId + " endif)";
          if (this.chbCorrect.Checked)
            str9 = " and code=0";
          string str11 = (int) this.billType != 1 ? " and bill_type in (2,3)" : " and bill_type=1";
          try
          {
            Convert.ToInt32(this.nudCountBill.Text);
            Convert.ToInt32(this.nudCountAkt.Text);
          }
          catch
          {
            return;
          }
          switch (this.level)
          {
            case 0:
            case 1:
              queryString1 = "select min(period_id) as period_id from cmpPeriod where company_id in (select company_id from dcCompany where rnn_id=" + this.raion.IdRnn.ToString() + " and complex_id in (100,101,102,108))";
              break;
            case 2:
            case 3:
            case 4:
              queryString1 = "select min(period_id) as period_id from cmpPeriod where company_id=" + this.company.CompanyId.ToString() + " and complex_id in (100,101,102,108)";
              break;
          }
          if ((int) this.level < 4 && Options.Period.PeriodId > Convert.ToInt32(this.session.CreateSQLQuery(queryString1).List()[0]))
          {
            int num1 = (int) MessageBox.Show("Невозможно распечатать счета, месяц не закрыт", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          else
          {
            string str12 = this.MainUsl();
            try
            {
              this.session.CreateSQLQuery("call droptmp(1,'" + str1 + "arenda',current user);create " + str3 + " table " + str2 + "arenda(client_id integer)" + str4 + ";create index client_id on " + str2 + "arenda (client_id)").ExecuteUpdate();
              this.session.Flush();
            }
            catch (Exception ex)
            {
              KvrplHelper.WriteLog(ex, (LsClient) null);
            }
            if (this.dgvList.SelectedRows.Count == 0)
            {
              if (Options.BaseName == "uk1" || Options.BaseName == "ukaka")
                str7 = "and (select sum(balance_in)from lsBalance where period_id=" + strPeriodId + " and client_id=ls.client_id )<>0  and (select sum(balance_out)from lsBalance where period_id=" + strPeriodId + " and client_id=ls.client_id )<>0  ";
              else
                str7 = "      and (select sum(rent) from lsRent where period_id=" + strPeriodId + " and client_id=ls.client_id )<>0 ";
              string str13 = "insert into " + str2 + "arenda select ls.client_id from lsClient ls left outer join lsBill b on b.client_id=ls.Client_id and b.period_id=" + strPeriodId + " and b.bill_type=" + this.billType.ToString() + ",dcCompany c, di_str d, homes h, homelink hl, flats f, LsArenda a  where ls.company_id=c.company_id and ls.complex_id=110 and " + str12 + "      and hl.idhome=h.idhome and hl.codeu=ls.company_id and h.idhome=ls.idhome and h.idstr=d.idstr and ls.idflat=f.idflat and ls.client_id=a.client_id " + str7;
              if (this.chbOnlyNew.Checked)
                str13 += " and bill_num is null";
              this.session.CreateSQLQuery(str13 + " order by dogovor_num").ExecuteUpdate();
              this.session.Flush();
            }
            else
            {
              foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgvList.SelectedRows)
                this.session.CreateSQLQuery("insert into " + str2 + "arenda (client_id) values (" + selectedRow.Cells[0].Value.ToString() + ")").ExecuteUpdate();
              this.session.Flush();
            }
            try
            {
              short num2;
              string str13;
              if ((int) this.level > 1)
              {
                string str14 = str8;
                string str15 = " and sp.company_id=";
                num2 = this.company.CompanyId;
                string str16 = num2.ToString();
                str13 = str14 + str15 + str16;
              }
              else
                str13 = "";
              string queryString2 = "call droptmp(1,'" + str1 + "nabor',current user); call droptmp(1,'" + str1 + "printserv',current user); create " + str3 + " table " + str2 + "nabor(company_id integer not null,root integer,service_id integer,supplier_id integer)" + str4 + "; create index company_id on " + str2 + "nabor (company_id); create index supplier_id on " + str2 + "nabor (supplier_id); create index sost on " + str2 + "nabor (company_id,supplier_id); create index sost1 on " + str2 + "nabor (root,service_id,supplier_id); create " + str3 + " table " + str2 + "printserv(client_id integer,idhome integer,root integer,service_id integer,supplier_id integer,receipt_id integer)" + str4 + "; create index client_id on " + str2 + "printserv (client_id); create index root on " + str2 + "printserv (root); create index clr on " + str2 + "printserv (client_id,receipt_id); create index css on " + str2 + "printserv (client_id,service_id,supplier_id); create index crs on " + str2 + "printserv (client_id,root,service_id,supplier_id); insert into " + str2 + "nabor select distinct sp.company_id,s.root as root,s.service_id,ss.supplier_id   from dcService s,cmpServiceParam sp, cmpSupplier ss   where  sp.service_id=s.root and sp.complex_id=110 " + str13 + "    and ss.company_id=(select first param_value from cmpParam where period_id=0 and dbeg<=" + strPeriod + " and dend>=" + strPeriod + " and param_id=211 and company_id=sp.company_id)     and (ss.service_id=sp.service_id or ss.service_id=(select crossservice_id from cmpCrossService where company_id=sp.company_id and dbeg<=" + strPeriod + " and dend>=" + strPeriod + "    and service_id=sp.service_id and crosstype_id=6)) and ss.supplier_id<>0 union all select sp.company_id,s.root as root,s.service_id,0    from dcService s,cmpServiceParam sp   where  sp.service_id=s.root " + str13 + " and sp.complex_id=110;  insert into " + str2 + "printserv  select ls.client_id, ls.idhome, tmp.root as root, tmp.service_id, tmp.supplier_id, 0 from " + str2 + "nabor tmp, dcCompany c, lsClient ls, flats f where ls.company_id=tmp.company_id     and (exists (select 1 from lsBalance where client_id=ls.client_id and supplier_id=tmp.supplier_id) or    exists (select 1 from lsSupplier where client_id=ls.client_id and supplier_id=tmp.supplier_id) or supplier_id=0)    and ls.complex_id=110 and ls.company_id=c.company_id and ls.idflat=f.idflat  and " + str12 + ";";
              this.session.Clear();
              this.session.CreateSQLQuery(queryString2).ExecuteUpdate();
              this.session.CreateSQLQuery(KvrplHelper.Prc_FillReceipt(str2 + "printserv", strPeriod)).ExecuteUpdate();
              if ((Receipt) this.cbReceipt.SelectedItem != null)
              {
                ISession session = this.session;
                string[] strArray = new string[5]{ "delete from ", str2, "printserv where receipt_id  not in (", null, null };
                int index1 = 3;
                num2 = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
                string str14 = num2.ToString();
                strArray[index1] = str14;
                int index2 = 4;
                string str15 = ")";
                strArray[index2] = str15;
                string queryString3 = string.Concat(strArray);
                session.CreateSQLQuery(queryString3).ExecuteUpdate();
              }
              this.session.Flush();
            }
            catch (Exception ex)
            {
              KvrplHelper.WriteLog(ex, (LsClient) null);
            }
            short receiptId;
            if (this.chbMec.Checked)
            {
              try
              {
                this.session.CreateSQLQuery("insert into lsBill  select distinct ls.client_id, " + strPeriodId + ", month_id, " + this.billType.ToString() + ",        isnull((select max(bill_num) from lsBill where period_id>=(select period_id from dcPeriod where period_value=cast('" + KvrplHelper.DateToBaseFormat(dt).ToString() + "' as date)) and bill_type=" + this.billType.ToString() + "),0)+        (select count(distinct month_id) from LsRent where period_id=r.period_id and client_id=r.client_id and month_id<=r.month_id),        cast('" + KvrplHelper.DateToBaseFormat(this.dtpDate.Value).ToString() + "' as date), user, today(), " + ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString() + " from lsClient ls left outer join lsRent r on ls.client_id=r.client_id and r.period_id=" + strPeriodId + ", dcCompany c where ls.company_id=c.company_id and ls.complex_id=110 and " + str12 + " and ls.client_id not in (select client_id from lsBill where period_id=" + strPeriodId + "     and bill_type=" + this.billType.ToString() + ") and (select sum(rent) from lsRent where period_id=" + strPeriodId + " and client_id=ls.client_id )<>0      and ls.client_id in (select client_id from " + str2 + "arenda)").ExecuteUpdate();
                this.session.Flush();
              }
              catch (Exception ex)
              {
              }
              if (this.chbCorrect.Checked)
              {
                try
                {
                  list1 = this.session.CreateSQLQuery("select isnull(max(bill_num),0) as maxnum from lsBill where period_id>=(select period_id from dcPeriod where period_value=cast('" + KvrplHelper.DateToBaseFormat(dt).ToString() + "' as date))     and bill_type in (2,3) and receipt_id=" + ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString() + " ").List();
                  this.session.CreateSQLQuery("insert into dba.lsBill select ls.client_id," + strPeriodId + ", month_id, 3 ,isnull((select max(bill_num) from lsBill where period_id>=  (select period_id from dcPeriod where period_value=cast('" + KvrplHelper.DateToBaseFormat(dt) + "' as date)) " + str11 + "),0)+number(),   cast('" + KvrplHelper.DateToBaseFormat(this.dtpDate.Value).ToString() + "' as date), user, today(), " + ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString() + " from lsClient ls  left outer join lsRent lr on ls.client_id=lr.client_id and lr.period_id=" + strPeriodId + " and lr.code<>0 , LsArenda a where ls.client_id=a.client_id and ls.client_id in (select client_id from lsBill where period_id=" + strPeriodId + " and     (bill_type=3 or (bill_type=2 and bill_num<=" + list1[0].ToString() + ")) and receipt_id=" + ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString() + ")    and (select sum(rent) from lsRent lb," + str2 + "printserv prc where period_id=" + strPeriodId + " and lb.client_id=ls.client_id and     lb.client_id=prc.client_id and prc.service_id=lb.service_id and lb.supplier_id=prc.supplier_id and code<>0)<>0 and ls.client_id in (select client_id from " + str2 + "arenda)     and isnull((select first period_id from lsBill where period_id<=" + strPeriodId + " and client_id=ls.client_id and bill_type=2 and     (month_id=0 or month_id=period_id) and receipt_id=" + ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString() + " order by period_id desc),0)<>0 group by ls.client_id, month_id").ExecuteUpdate();
                  this.session.Flush();
                }
                catch (Exception ex)
                {
                }
              }
            }
            else
            {
              string str13;
              if (!this.txbNum.Enabled || this.txbNum.Text == "")
              {
                str13 = "isnull((select max(bill_num) from lsBill where period_id>=(select period_id from dcPeriod where period_value=cast('" + KvrplHelper.DateToBaseFormat(dt).ToString() + "' as date)) " + str11 + "),0)+number(),";
              }
              else
              {
                try
                {
                  Convert.ToInt32(this.txbNum.Text);
                  str13 = this.txbNum.Text + ",";
                }
                catch
                {
                  int num2 = (int) MessageBox.Show("Некорректный номер счета!", "Внимание", MessageBoxButtons.OK);
                  return;
                }
              }
              if (Options.BaseName != "uk1" && Options.BaseName != "ukaka")
                str7 = "      and (select sum(rent) from lsRent where period_id=" + strPeriodId + " and client_id=ls.client_id" + str9 + " )<>0 ";
              string str14 = "insert into lsBill select ls.client_id, " + strPeriodId + ", 0, " + this.billType.ToString() + ", " + str13 + "cast('" + KvrplHelper.DateToBaseFormat(this.dtpDate.Value).ToString() + "' as date), user, today(), " + ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString() + " from lsClient ls, LsArenda a where ls.client_id=a.client_id " + str7 + "               and ls.client_id in (select client_id from " + str2 + "arenda) ";
              string str15;
              if ((int) this.billType == 1)
              {
                string[] strArray = new string[6]{ str14, " and ls.client_id not in (select client_id from lsBill where period_id=", strPeriodId, " and bill_type=1 and receipt_id=", null, null };
                int index1 = 4;
                receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
                string str16 = receiptId.ToString();
                strArray[index1] = str16;
                int index2 = 5;
                string str17 = ")";
                strArray[index2] = str17;
                str15 = string.Concat(strArray);
              }
              else
                str15 = str14 + " and ls.client_id not in (select client_id from lsBill where period_id=" + strPeriodId + " and (bill_type=2 or (bill_type=3 and  bill_num<=isnull((select max(bill_num) from lsBill where period_id>=(select period_id from dcPeriod where period_value=cast('" + KvrplHelper.DateToBaseFormat(dt).ToString() + "' as date)) " + str11 + "),0))) and receipt_id=" + ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString() + ")";
              this.session.CreateSQLQuery(str15 + " order by dogovor_num").ExecuteUpdate();
              this.session.Flush();
              ISession session = this.session;
              string[] strArray1 = new string[5]{ "select isnull(max(bill_num),0) as maxnum from lsBill where period_id>=(select period_id from dcPeriod where period_value=cast('", KvrplHelper.DateToBaseFormat(dt).ToString(), "' as date))     and bill_type in (2,3) and receipt_id=", null, null };
              int index3 = 3;
              receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
              string str18 = receiptId.ToString();
              strArray1[index3] = str18;
              int index4 = 4;
              string str19 = " ";
              strArray1[index4] = str19;
              string queryString2 = string.Concat(strArray1);
              list1 = session.CreateSQLQuery(queryString2).List();
              if (this.chbCorrectMonth.Checked && this.chbCorrect.Checked)
              {
                try
                {
                  string[] strArray2 = new string[29];
                  strArray2[0] = "insert into dba.lsBill select ls.client_id,";
                  strArray2[1] = strPeriodId;
                  strArray2[2] = ", month_id, 3 ,isnull((select max(bill_num) from lsBill where period_id>=  (select period_id from dcPeriod where period_value=cast('";
                  strArray2[3] = KvrplHelper.DateToBaseFormat(dt);
                  strArray2[4] = "' as date)) ";
                  strArray2[5] = str11;
                  strArray2[6] = "),0)+number(),   cast('";
                  strArray2[7] = KvrplHelper.DateToBaseFormat(this.dtpDate.Value).ToString();
                  strArray2[8] = "' as date), user, today(), ";
                  int index1 = 9;
                  receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
                  string str16 = receiptId.ToString();
                  strArray2[index1] = str16;
                  int index2 = 10;
                  string str17 = " from lsClient ls  left outer join lsRent lr on ls.client_id=lr.client_id and lr.period_id=";
                  strArray2[index2] = str17;
                  int index5 = 11;
                  string str20 = strPeriodId;
                  strArray2[index5] = str20;
                  int index6 = 12;
                  string str21 = " and lr.code<>0 , LsArenda a where ls.client_id=a.client_id and ls.client_id in (select client_id from lsBill where period_id=";
                  strArray2[index6] = str21;
                  int index7 = 13;
                  string str22 = strPeriodId;
                  strArray2[index7] = str22;
                  int index8 = 14;
                  string str23 = " and     (bill_type=3 or (bill_type=2 and bill_num<=";
                  strArray2[index8] = str23;
                  int index9 = 15;
                  string str24 = list1[0].ToString();
                  strArray2[index9] = str24;
                  int index10 = 16;
                  string str25 = ")) and receipt_id=";
                  strArray2[index10] = str25;
                  int index11 = 17;
                  receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
                  string str26 = receiptId.ToString();
                  strArray2[index11] = str26;
                  int index12 = 18;
                  string str27 = ")    and (select sum(rent) from lsRent lb,";
                  strArray2[index12] = str27;
                  int index13 = 19;
                  string str28 = str2;
                  strArray2[index13] = str28;
                  int index14 = 20;
                  string str29 = "printserv prc where period_id=";
                  strArray2[index14] = str29;
                  int index15 = 21;
                  string str30 = strPeriodId;
                  strArray2[index15] = str30;
                  int index16 = 22;
                  string str31 = " and lb.client_id=ls.client_id and     lb.client_id=prc.client_id and prc.service_id=lb.service_id and lb.supplier_id=prc.supplier_id and code<>0)<>0 and ls.client_id in (select client_id from ";
                  strArray2[index16] = str31;
                  int index17 = 23;
                  string str32 = str2;
                  strArray2[index17] = str32;
                  int index18 = 24;
                  string str33 = "arenda)     and isnull((select first period_id from lsBill where period_id<=";
                  strArray2[index18] = str33;
                  int index19 = 25;
                  string str34 = strPeriodId;
                  strArray2[index19] = str34;
                  int index20 = 26;
                  string str35 = " and client_id=ls.client_id and bill_type=2 and     (month_id=0 or month_id=period_id) and receipt_id=";
                  strArray2[index20] = str35;
                  int index21 = 27;
                  receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
                  string str36 = receiptId.ToString();
                  strArray2[index21] = str36;
                  int index22 = 28;
                  string str37 = " order by period_id desc),0)<>0 group by ls.client_id, month_id";
                  strArray2[index22] = str37;
                  this.session.CreateSQLQuery(string.Concat(strArray2)).ExecuteUpdate();
                  this.session.Flush();
                }
                catch (Exception ex)
                {
                }
              }
            }
            string[] strArray3 = new string[8]{ "select distinct b.Client_id, a.dogovor_num from lsClient ls, lsBill b, LsArenda a where  b.client_id=ls.Client_id and ls.client_id=a.client_id and b.period_id=", strPeriodId, str11, "     and ls.client_id in (select client_id from ", str2, "arenda) and receipt_id=", null, null };
            int index23 = 6;
            receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
            string str38 = receiptId.ToString();
            strArray3[index23] = str38;
            int index24 = 7;
            string str39 = " order by a.dogovor_num";
            strArray3[index24] = str39;
            IList list2 = this.session.CreateSQLQuery(string.Concat(strArray3)).List();
            if (list2.Count == 0)
            {
              int num3 = (int) MessageBox.Show("Нет данных для отчета.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
              PrintSettings printSettings = (PrintSettings) null;
              foreach (object[] objArray1 in (IEnumerable) list2)
              {
                ISession session1 = this.session;
                string[] strArray1 = new string[6]{ "select * from lsBill where period_id=", strPeriodId, " and client_id=", objArray1[0].ToString(), " and bill_type=3 and receipt_id=", null };
                int index1 = 5;
                receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
                string str13 = receiptId.ToString();
                strArray1[index1] = str13;
                string queryString2 = string.Concat(strArray1);
                IList list3 = session1.CreateSQLQuery(queryString2).List();
                if (this.chbCorrect.Checked && !this.chbCorrectMonth.Checked && list3.Count == 0)
                {
                  ISession session2 = this.session;
                  string[] strArray2 = new string[31];
                  strArray2[0] = "insert into dba.lsBill select ls.client_id,";
                  strArray2[1] = strPeriodId;
                  strArray2[2] = ", (select first period_id from lsBill where period_id<=";
                  strArray2[3] = strPeriodId;
                  strArray2[4] = "-1 and client_id=ls.client_id and bill_type=2 and   (month_id=0 or month_id=period_id) order by period_id desc), 3 ,isnull((select max(bill_num) from lsBill where period_id>=  (select period_id from dcPeriod where period_value=cast('";
                  strArray2[5] = KvrplHelper.DateToBaseFormat(dt);
                  strArray2[6] = "' as date)) ";
                  strArray2[7] = str11;
                  strArray2[8] = "),0)+number(),   cast('";
                  strArray2[9] = KvrplHelper.DateToBaseFormat(this.dtpDate.Value).ToString();
                  strArray2[10] = "' as date), user, today(), ";
                  int index2 = 11;
                  receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
                  string str14 = receiptId.ToString();
                  strArray2[index2] = str14;
                  int index3 = 12;
                  string str15 = " from lsClient ls  , LsArenda a where ls.client_id=a.client_id and ls.client_id in (select client_id from lsBill where period_id=";
                  strArray2[index3] = str15;
                  int index4 = 13;
                  string str16 = strPeriodId;
                  strArray2[index4] = str16;
                  int index5 = 14;
                  string str17 = " and  client_id=";
                  strArray2[index5] = str17;
                  int index6 = 15;
                  string str18 = objArray1[0].ToString();
                  strArray2[index6] = str18;
                  int index7 = 16;
                  string str19 = " and     (bill_type=3 or (bill_type=2 and bill_num<=";
                  strArray2[index7] = str19;
                  int index8 = 17;
                  string str20 = list1[0].ToString();
                  strArray2[index8] = str20;
                  int index9 = 18;
                  string str21 = ")) and receipt_id=";
                  strArray2[index9] = str21;
                  int index10 = 19;
                  receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
                  string str22 = receiptId.ToString();
                  strArray2[index10] = str22;
                  int index11 = 20;
                  string str23 = ")    and (select sum(rent) from lsRent lb,";
                  strArray2[index11] = str23;
                  int index12 = 21;
                  string str24 = str2;
                  strArray2[index12] = str24;
                  int index13 = 22;
                  string str25 = "printserv prc where period_id=";
                  strArray2[index13] = str25;
                  int index14 = 23;
                  string str26 = strPeriodId;
                  strArray2[index14] = str26;
                  int index15 = 24;
                  string str27 = " and lb.client_id=ls.client_id and     lb.client_id=prc.client_id and prc.service_id=lb.service_id and lb.supplier_id=prc.supplier_id and code<>0)<>0 and ls.client_id in (select client_id from ";
                  strArray2[index15] = str27;
                  int index16 = 25;
                  string str28 = str2;
                  strArray2[index16] = str28;
                  int index17 = 26;
                  string str29 = "arenda)     and isnull((select first period_id from lsBill where period_id<=";
                  strArray2[index17] = str29;
                  int index18 = 27;
                  string str30 = strPeriodId;
                  strArray2[index18] = str30;
                  int index19 = 28;
                  string str31 = "-1 and client_id=ls.client_id and bill_type=2 and     (month_id=0 or month_id=period_id) and receipt_id=";
                  strArray2[index19] = str31;
                  int index20 = 29;
                  receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
                  string str32 = receiptId.ToString();
                  strArray2[index20] = str32;
                  int index21 = 30;
                  string str33 = " order by period_id desc),0)<>0 order by dogovor_num";
                  strArray2[index21] = str33;
                  string queryString3 = string.Concat(strArray2);
                  session2.CreateSQLQuery(queryString3).ExecuteUpdate();
                  this.session.Flush();
                }
                if (!this.chbCorrect.Enabled && this.rbInvoice.Checked)
                {
                  try
                  {
                    string[] strArray2 = new string[6]{ "select month_id from lsbill where period_id=", strPeriodId, " and client_id=", objArray1[0].ToString(), " and bill_type=3 and receipt_id=", null };
                    int index2 = 5;
                    receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
                    string str14 = receiptId.ToString();
                    strArray2[index2] = str14;
                    short num2 = this.session.CreateSQLQuery(string.Concat(strArray2)).UniqueResult<short>();
                    if ((int) num2 == (int) Convert.ToInt16(strPeriodId))
                    {
                      object[] objArray2 = new object[8]{ (object) "update lsbill set month_id=", (object) num2, (object) "-1 where period_id=", (object) strPeriodId, (object) " and client_id=", (object) objArray1[0].ToString(), (object) " and bill_type=3 and receipt_id=", null };
                      int index3 = 7;
                      receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
                      string str15 = receiptId.ToString();
                      objArray2[index3] = (object) str15;
                      this.session.CreateSQLQuery(string.Concat(objArray2)).ExecuteUpdate();
                      this.session.Flush();
                    }
                  }
                  catch (Exception ex)
                  {
                  }
                }
                this.clienttemp = this.session.Get<LsClient>((object) Convert.ToInt32(objArray1[0]));
                this.report = new Report();
                if (this.rbBill.Checked)
                  this.ReportLoad(this.report, this.idBaseOrg, 1);
                if (this.rbInvoice.Checked)
                  this.ReportLoad(this.report, this.idBaseOrg, 2);
                if (this.rbAktDontInvoice.Checked)
                  this.ReportLoad(this.report, this.idBaseOrg, 2);
                this.report.Dictionary.Connections[0].ConnectionString = this.connectionString;
                string str34 = list3.Count <= 0 ? "" : " and code=0 ";
                string str35 = "";
                string str36 = ", isnull((select receipt_id                   from dba.cmphmReceipt sp, dba.lsClient ls                   where ls.company_id=sp.company_id and ls.idhome=sp.idhome and sp.complex_id=ls.complex_id                   and  @date  between dbeg and dend and ls.client_id=t.client_id                   and (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.supplier_id=t.supplier_id),         isnull((select receipt_id                   from dba.cmphmReceipt sp, dba.lsClient ls                   where ls.company_id=sp.company_id and ls.idhome=sp.idhome and sp.complex_id=ls.complex_id                   and  @date  between dbeg and dend and ls.client_id=t.client_id and sp.service_id=0 and sp.supplier_id=t.supplier_id),         isnull((select receipt_id                  from dba.cmphmReceipt sp, dba.lsClient ls                   where ls.company_id=sp.company_id and ls.idhome=sp.idhome and sp.complex_id=ls.complex_id                   and  @date  between dbeg and dend and ls.client_id=t.client_id                   and (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.supplier_id=0),         isnull((select receipt_id                   from dba.cmphmReceipt sp, dba.lsClient ls                   where ls.company_id=sp.company_id and sp.idhome=0 and sp.complex_id=ls.complex_id                   and  @date  between dbeg and dend and ls.client_id=t.client_id                   and (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.supplier_id=t.supplier_id),         isnull((select receipt_id                  from dba.cmphmReceipt sp, dba.lsClient ls                   where ls.company_id=sp.company_id and sp.idhome=0 and sp.complex_id=ls.complex_id                   and  @date  between dbeg and dend and ls.client_id=t.client_id and sp.service_id=0 and sp.supplier_id=t.supplier_id),         isnull((select receipt_id                   from dba.cmphmReceipt sp, dba.lsClient ls                   where ls.company_id=sp.company_id and sp.idhome=0 and sp.complex_id=ls.complex_id                   and  @date  between dbeg and dend and ls.client_id=t.client_id                   and (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.supplier_id=0),         isnull((select receipt_id                   from dba.cmpServiceParam sp, dba.lsClient ls                   where ls.company_id=sp.company_id and ls.client_id=t.client_id and sp.complex_id=ls.complex_id                   and (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0))),         isnull((if t.service_id=0 and (select count(receipt_id) from dba.cmpReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and ls.client_id=t.client_id)=1 then          (select receipt_id from dba.cmpReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and ls.client_id=t.client_id) else 0 endif),0)))))))) rec";
                string str37;
                if (Options.BaseName == "uk1" || Options.BaseName == "ukaka")
                {
                  str37 = " qqq.nds as buyervat, qqq.calc as buyercalc, qqq.calc as buyerdolg, ";
                  string format = "left join (select qwe.client_id,sum(qwe.balout) calc,-round((sum(qwe.balout)/1.18)-sum(qwe.balout),2) nds,round((sum(qwe.balout)/1.18),2) rentWithoutNds            from (select t.* {3}               from (                         select serv.service_id, {0} client_id,balout,supplier_id                        from DBA.dcService serv                           left outer join (select root,b.service_id,sum(b.Balance_out) as balout,supplier_id                                            from DBA.lsBalance b, DBA.dcService s                                               where b.service_id=s.service_id and b.period_id={2} and b.client_id={0} group by root,supplier_id,b.service_id                                          ) as b on b.root=serv.service_id,                           DBA.cmpServiceParam sp                           where serv.root=0 and sp.service_id=serv.service_id and sp.company_id={1} and sp.complex_id=110                              and isnull(balout,0)<>0                             union all                          select s.service_id, {0} client_id, sum(b.Balance_out) as balout,supplier_id                        from DBA.dcService s                           left outer join DBA.lsBalance b on b.service_id=s.service_id and b.period_id={2} and b.client_id={0} where s.service_id=0                           group by s.service_id,supplier_id                      )t                    )qwe                where rec={4}                   group by qwe.client_id            )qqq on qqq.client_id=ls.client_id ";
                  object[] objArray2 = new object[5]{ (object) objArray1[0].ToString(), (object) this.clienttemp.Company.CompanyId, (object) strPeriodId, (object) str36, null };
                  int index2 = 4;
                  receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
                  string str14 = receiptId.ToString();
                  objArray2[index2] = (object) str14;
                  str35 = string.Format(format, objArray2);
                }
                else
                {
                  str37 = "       (select sum(payment) from lsBalance lb, " + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + "  and         lb.client_id=prc.client_id and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id) as buyerpayment,        (select sum(balance_in) from lsBalance lb, " + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + "  and         lb.client_id=prc.client_id and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id) as buyerdolg,        (if b.month_id=0 then          (select sum(rent) from lsRent lb," + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + " and lb.client_id=prc.client_id and            lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id " + str34 + ")         else (select sum(rent) from lsRent lb," + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + " and lb.client_id=prc.client_id and            month_id=b.month_id and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id " + str34 + ") endif) as buyercalc,        (if b.month_id=0 then           (select sum(rent_vat) from lsRent lb," + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + " and lb.client_id=prc.client_id and                lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id " + str34 + ")        else (select sum(rent_vat) from lsRent lb," + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + " and month_id=b.month_id and                lb.client_id=prc.client_id and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id " + str34 + ") endif) as buyervat, ";
                  string str14 = "";
                  string str15 = "";
                  if (this.checkBoxMinusOff.Checked)
                  {
                    str14 = " and lb.rent > 0 ";
                    str15 = " and (select sum(isnull(lb2.rent,0)) from lsRent lb2," + str2 + "printserv prc where lb2.client_id=prc.client_id and lb2.service_id=prc.service_id and lb2.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and lb2.client_id=ls.client_id                   " + str5 + " and lb2.month_id=b.month_id " + str34 + ") > 0 ";
                  }
                  if ((this.city == 23 || this._korTamplate) && this.checkBoxMinusOff.Checked)
                    str37 = "       (select sum(payment) from lsBalance lb, " + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + "  and         lb.client_id=prc.client_id and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id) as buyerpayment,        (select sum(balance_in) from lsBalance lb, " + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + "  and         lb.client_id=prc.client_id and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id) as buyerdolg,  (select sum(lb.rent) from dba.lsBalance lb, " + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + str14 + " and lb.client_id=prc.client_id and            lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id " + str34 + ") as buyercalc,        (if b.month_id=0 then           (select sum(rent_vat) from lsRent lb," + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + " and lb.client_id=prc.client_id and                lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id " + str34 + ")        else (select sum(rent_vat) from lsRent lb," + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + " and month_id=b.month_id and                lb.client_id=prc.client_id and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id " + str34 + ") endif) as buyervat, ";
                }
                this._main = this.report.GetDataSource("Main") as TableDataSource;
                string str40 = "";
                string str41 = "";
                string str42 = "";
                string str43 = "";
                string str44;
                string str45;
                string str46;
                string str47;
                string str48;
                string str49;
                string str50;
                string str51;
                if ((int) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId == 50)
                {
                  ohlAccounts ohlAccounts = this.session.Get<ohlAccounts>((object) Convert.ToInt32(this.session.CreateQuery("select hm from HomeParam hm where hm.Param.ParamId=309 and hm.Company.CompanyId=:cmp and hm.Home.IdHome=:home and DEnd=(select max(DEnd) from HomeParam hm where hm.Param.ParamId=309 and hm.Company.CompanyId=:cmp and hm.Home.IdHome=:home)").SetParameter<short>("cmp", this.clienttemp.Company.CompanyId).SetParameter<int>("home", this.clienttemp.Home.IdHome).UniqueResult<HomeParam>().ParamValue));
                  str44 = string.Format(" '{0}' as accountseller, ", (object) ohlAccounts.Account);
                  str45 = string.Format(" '{0}' as bikbank, ", (object) ohlAccounts.Bank.BIK);
                  str46 = string.Format(" '{0}' as namebank, ", (object) ohlAccounts.Bank.BankName);
                  str47 = string.Format(" '{0}' as korbank, ", (object) ohlAccounts.Bank.KorSch);
                  str48 = " (ls.client_id) as client_id ";
                  str49 = " (select csc.supplier_client from cmpSupplierClient csc, lsClient ls where ls.client_id = csc.client_id and ls.client_id=" + objArray1[0].ToString() + ") as supplier_client ";
                  str50 = "";
                  str51 = "";
                }
                else
                {
                  str44 = "       (select first account from cmpReceipt where company_id=c.company_id and receipt_id=r.receipt_id) as accountseller, ";
                  str45 = "       (select bik from di_bank where idbank=idbankseller) as bikbank, ";
                  str46 = "       (select namebank from di_bank where idbank=idbankseller) as namebank, ";
                  str47 = "       (select kor_sch from di_bank where idbank=idbankseller) as korbank, ";
                  str40 = "       (select first account from cmpReceipt where supplier_id = r.seller_id and receipt_id = r.receipt_id) as accountsellerreal, ";
                  str41 = "       (select bik from di_bank where idbank = idbanksellerreal) as bikbankseller, ";
                  str42 = "       (select namebank from di_bank where idbank = idbanksellerreal) as namebankseller, ";
                  str43 = "       (select kor_sch from di_bank where idbank = idbanksellerreal) as korbankseller, ";
                  str48 = " (ls.client_id) as client_id ";
                  str49 = " (convert(integer,0)) as supplier_client ";
                  str50 = "";
                  str51 = "";
                }
                string str52 = "";
                if (this.chbPeni.Checked && this.rbFullPeniPeriod.Checked)
                  str52 = "+ sum(rent_full)";
                TableDataSource main = this._main;
                string[] strArray4 = new string[69]{ "declare @date date  select @date=Period_value from dcPeriod where period_id= ", strPeriodId, " select b.bill_num, b.bill_date, dogovor_num, dogovor_date, ", str48, ", ", str49, ", c.company_id, r.receipt_id, isnull(r.seller_id,0) as seller_id, isnull(r.supplier_id,0) as supplier_id,        borg.nameorg_min as buyer, borg.nameorg as namebuyer, borg.r_sch as rschbuyer, borg.idbaseorg as idbuyer, ", this.Address("borg.idbaseorg"), " as adrbuyer,        (if (r.seller_id is not null and r.seller_id<>0) then ", this.Address("r.seller_id"), " else '' endif) as adrseller,        (if (r.supplier_id is not null and r.supplier_id<>0) then ", this.Address("r.supplier_id"), " else '' endif) as adrsupplier,        borg.inn, borg.kpp, borg.tel as buyertel, db.kor_sch as buyersch, db.namebank as buyerbank, db.bik as buyerbik,        if b.month_id = 0 then ", strPeriod, " else (select period_value from dcPeriod where period_id=b.month_id) endif as period, b.month_id as month_id,        (if '", this.chbPeni.Checked.ToString(), "'='True' then isnull((select sum(balance_out) ", str52, " from lsBalancePeni b where client_id=ls.client_id and        period_id=", strPeriodId, " and (service_id in (select root from ", str2, "printserv where client_id=ls.client_id and supplier_id=b.supplier_id and        receipt_id=r.receipt_id) or service_id in (select service_id from ", str2, "printserv where client_id=ls.client_id and supplier_id=b.supplier_id and        receipt_id=r.receipt_id)  or service_id=0 )),0) else 0 endif) as peni,", str37, "       (select sum(rent) from lsRent lb,", str2, "printserv prc where lb.client_id=ls.client_id and period_id=", strPeriodId, " and lb.client_id=prc.client_id            and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id and code<>0) as buyerpast,        (select sum(payment) from lsBalance lb,", str2, "printserv prc where lb.client_id=ls.client_id and period_id=", strPeriodId, " and lb.client_id=prc.client_id            and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id) as payment,        (select sum(rent_vat) from lsRent lb,", str2, "printserv prc where lb.client_id=ls.client_id and period_id=", strPeriodId, " and lb.client_id=prc.client_id and            lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id and code<>0) as buyervatpast,        (if (r.seller_id is not null and r.seller_id<>0) then (select nameorg from base_org where idbaseorg=r.seller_id) else '-----' endif) as nameseller,        (if (r.supplier_id is not null and r.supplier_id<>0) then (select nameorg from base_org where idbaseorg=r.supplier_id) else '-----' endif) as namesupplier,        (if (r.seller_id is not null and r.seller_id<>0) then (select nameorg_min from base_org where idbaseorg=r.seller_id) else '' endif) as nameminseller,        (select inn from base_org where idbaseorg=r.seller_id) as innseller,        (select kpp from base_org where idbaseorg=r.seller_id) as kppseller,        (select tel from base_org where idbaseorg=r.seller_id) as telseller,        (select inn from base_org where idbaseorg=r.supplier_id) as innsupplier,        (select kpp from base_org where idbaseorg=r.supplier_id) as kppsupplier,        (select tel from base_org where idbaseorg=r.supplier_id) as telsupplier,        (select fax from base_org where idbaseorg=r.supplier_id) as faxsupplier, ", str44, "       (select first bank from base_org where idbaseorg = r.seller_id) as idbanksellerreal, ", str42, str41, str43, str40, "       (select first bank_id from cmpReceipt where company_id=c.company_id and receipt_id=r.receipt_id) as idbankseller, ", str46, str45, str47, "       (if (r.consignor_id is not null and r.consignor_id<>0) then (select nameorg from base_org where idbaseorg=r.consignor_id) else '-----' endif) as nameconsignor,        (if (r.consignor_id is not null and r.consignor_id<>0) then ", this.Address("r.consignor_id"), " else '' endif) as adrconsignor,        ", this.NS_lsAddressHome(2, "ls.idflat"), " as adr,        ", this.NS_lsParam_value(2, "ls.client_id ", "cast( '" + KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(Options.Period.PeriodName.Value)).ToString() + "'as date)"), " as plflat,        c.manager_id from lsClient ls ", str35, ", dcCompany c,", str51, " LsArenda la left outer join base_org borg on la.idbaseorg=borg.idbaseorg left outer join di_bank db on db.idbank=borg.bank, lsBill b, cmpReceipt r where ls.company_id=c.company_id and la.Client_id=ls.Client_id and ls.complex_id=110 and b.client_id=ls.Client_id and r.company_id=c.company_id and b.period_id=", strPeriodId, str50, "       and b.bill_type=", this.billType.ToString(), " and ls.client_id=", objArray1[0].ToString(), "       and r.receipt_id in (", null, null };
                int index22 = 67;
                receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
                string str53 = receiptId.ToString();
                strArray4[index22] = str53;
                int index25 = 68;
                string str54 = ") and b.receipt_id=r.receipt_id";
                strArray4[index25] = str54;
                string str55 = string.Concat(strArray4);
                main.SelectCommand = str55;
                string selectCommand1 = this._main.SelectCommand;
                IList list4 = (IList) new ArrayList();
                if (this.chB_Offer_FR.Checked)
                  list4 = this.GetListBalanceCard(false, this.clienttemp);
                double num4 = 0.0;
                foreach (object[] objArray2 in (IEnumerable) list4)
                  num4 = Convert.ToDouble(objArray2[0]) - Convert.ToDouble(objArray2[10]);
                IList mounthList1 = this.session.CreateSQLQuery("select month_dept_id from dba.lsBalance lb where lb.period_id=" + strPeriodId + " and lb.client_id=" + objArray1[0].ToString() ?? "").List();
                string str56;
                if (Options.BaseName == "uk1" || Options.BaseName == "ukaka")
                {
                  str56 = string.Format("declare @date date  select @date=Period_value from dcPeriod where period_id= {2} select servpar.PrintShow sname,qwe.service_id,qwe.client_id,sum(qwe.balout) calc,'' edizm,'' tax,'' volume1,'' vat,servpar.Sorter,-round((sum(qwe.balout)/1.18)-sum(qwe.balout),2) nds,round((sum(qwe.balout)/1.18),2) rentWithoutNds  from (select t.* {3}   from (select serv.service_id, {0} client_id,balout,supplier_id            from DBA.dcService serv               left outer join (select root,b.service_id,sum(b.Balance_out) as balout,supplier_id                                from DBA.lsBalance b, DBA.dcService s                                   where b.service_id=s.service_id and b.period_id={2} and b.client_id={0} group by root,supplier_id,b.service_id                              ) as b on b.root=serv.service_id,                               DBA.cmpServiceParam sp               where serv.root=0 and sp.service_id=serv.service_id and sp.company_id={1} and sp.complex_id=110                  and isnull(balout,0)<>0                 union all          select s.service_id,{0} client_id, sum(b.Balance_out) as balout,supplier_id            from DBA.dcService s               left outer join DBA.lsBalance b on b.service_id=s.service_id and b.period_id={2} and b.client_id={0} where s.service_id=0               group by s.service_id,supplier_id            )t   )qwe inner join dba.dcService serv on serv.service_id=qwe.service_id inner join cmpServiceParam servpar on servpar.Service_id=serv.Service_id where isnull(qwe.balout,0)<>0 and servpar.Company_id={1} and servpar.Complex_id=110 and rec={4}group by  serv.service_name,servpar.PrintShow,qwe.service_id,qwe.client_id,servpar.Sorter order by servpar.Sorter ", (object) objArray1[0].ToString(), (object) this.clienttemp.Company.CompanyId, (object) strPeriodId, (object) str36, (object) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString());
                }
                else
                {
                  string str14 = "select (select client_id from lsClient where client_id=:client_id) as client_id, r.service_id, p.period_value, isnull(t.isvat,0) as isvat,";
                  string str15;
                  string str16;
                  if (this.chbSost.Checked)
                  {
                    str14 += "ds.service_id as sost,";
                    str5 = "and b.service_id=ds.service_id";
                    str6 = " left outer join dcService ds on r.service_id=ds.root";
                    str15 = "and ds.service_id=t.service_id";
                    str16 = "and service_id in (select service_id from " + str2 + "printserv prc where client_id=:client_id and root=r.service_id and service_id=b.service_id)";
                  }
                  else
                  {
                    str5 = "and b.service_id in (select service_id from dcService where root=r.service_id)";
                    str15 = "and s.service_id=t.service_id";
                    str16 = "and service_id in (select service_id from " + str2 + "printserv prc where client_id=:client_id and root=r.service_id)";
                  }
                  if ((this.city == 1 || this.city == 5 || (this.city == 9 || this.city == 36)) && !this._korTamplate)
                    str14 = str14 + "         isnull((if (t.service_id not in (20,71,72,73,74,78,95)                        or (t.service_id=20 and isnull((select group_num from cmpServiceParam where company_id=r.company_id and service_id=21 and complex_id=r.complex_id),0)=0)                       or (t.service_id=95 and isnull((select group_num from cmpServiceParam where company_id=r.company_id and service_id=96 and complex_id=r.complex_id),0)=0                                                            and exists (select 1 from lsService where Client_id=:client_id and Period_id=0 and Service_id=96))                       or (r.service_id=95 and isnull((select group_num from cmpServiceParam where company_id=r.company_id and service_id=96 and complex_id=r.complex_id),0)<>0                                                            and not exists (select 1 from lsService where Client_id=:client_id and Period_id=0 and Service_id=96)               )) then (if doc not in (2,5,6,7,8,12,14,15,17,18,21,25,28,29,31,33,34) then (if r.service_id=3 then                    (select sum(cost) from cmpTariff b where company_id=t.company_id and period_id=0 and tariff_id=s.tariff_id and dbeg=t.dbeg and dend=t.dend                    " + str16 + " and scheme<>3) else                          (if t.scheme<>129 then t.cost else 0 endif)endif) else t.cost endif)                   else 0 endif),0) as tax,";
                  if (this.city == 23 || this._korTamplate)
                    str14 = str14 + "         isnull((if t.service_id not in (20,34) then (if doc not in (2,5,6,7,8,12,14,15,17,18,21,25,28,29,31,33,34) then (if r.service_id=3 then                    (select sum(cost) from cmpTariff b where company_id=t.company_id and period_id=0 and tariff_id=s.tariff_id and dbeg=t.dbeg and dend=t.dend                    " + str16 + " and scheme<>3) else                          (if t.scheme<>129 then t.cost else 0 endif)endif) else t.cost endif)                   else 0 endif),0) as tax,";
                  string str17 = str14 + "         (if gr=0 then (if isnull((select unitmeasuring_name from dcUnitMeasuring where unitmeasuring_id=t.unitmeasuring_id),'')<>''            then (select unitmeasuring_name from dcUnitMeasuring where unitmeasuring_id=t.unitmeasuring_id)            else (select unitmeasuring_name from dcUnitMeasuring where unitmeasuring_id=(select first unitmeasuring_id from cmpTariff ctr where service_id=r.service_id and unitmeasuring_id is not null)) endif)          else if t.unitmeasuring_id is not null then (select unitmeasuring_name from dcUnitMeasuring where unitmeasuring_id=t.unitmeasuring_id)          else (select unitmeasuring_name from dcUnitMeasuring where unitmeasuring_id=(select first unitmeasuring_id from cmpTariff ctr where service_id=gr )) endif endif) as edizm,          isnull((if (" + this.city.ToString() + "<>1 or t.service_id not in (71,72,73,74,78)) then isnull((select first n.norm_value from cmpNorm n where s.norm_id=n.norm_id and            n.company_id=(select param_value from cmpParam where company_id=r.company_id and period_id=0 and param_id=204 and dbeg<=" + strPeriod + " and dend>=" + strPeriod + ")         and n.dbeg<=" + strPeriod + " and n.dend>=" + strPeriod + " and n.period_id=0),0) else 0 endif),0) as norm,          (select group_num from cmpServiceParam where company_id=r.company_id and complex_id=110 and service_id=r.service_id) as gr,          isnull((select param_value from lsParam where period_id=0 and client_id=:client_id and dbeg<=" + strPeriod + " and dend>=" + strPeriod + " and param_id=104),0) as doc,         isnull((if (select first basetariff_id from lsService ls,cmpTariff ct where ls.client_id=:client_id and ls.service_id=r.service_id and ls.service_id=ct.service_id               and ls.tariff_id=ct.tariff_id and ls.dbeg<=" + strPeriod + " and ls.dend>=" + strPeriod + " and ls.period_id=0 and ct.period_id=0 and ct.company_id=r.company_id and ct.dbeg<=" + strPeriod + " and ct.dend>=" + strPeriod + ")=2 then " + "         (if (select group_num from cmpServiceParam where company_id=:company_id and complex_id=110 and service_id=r.service_id)=0 then            isnull((select max(volume) from lsRent b where period_id=" + strPeriodId + " and client_id=:client_id " + str16 + "),0) else 0 endif)" + "                     else (if :month_id =0 then                       isnull((select max(volume) from lsRent b where period_id=" + strPeriodId + " and b.client_id=:client_id                       " + str16 + " and code=0),0)                       else isnull((select max(volume) from lsRent b where period_id=" + strPeriodId + " and b.client_id=:client_id                       " + str16 + " and month_id=:month_id),0) endif) endif),0) as volume,         (if :month_id = 0 then                  isnull((select sum(isnull(rent,0)) from lsRent b," + str2 + "printserv prc where b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id                   " + str5 + str34 + "),0) else                   isnull((select sum(isnull(rent,0)) from lsRent b," + str2 + "printserv prc where b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id                   " + str5 + " and month_id=:month_id " + str34 + "),0) endif) as calc,                   isnull((select sum(isnull(b.rent,0)) - sum(isnull(b.rent_past,0)) from dba.lsBalance b, " + str2 + "printserv prc where b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id                   " + str5 + " ),0) as rentC,                   isnull((select sum(isnull(b.rent_past,0)) from dba.lsBalance b, " + str2 + "printserv prc where b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id                   " + str5 + " ),0) as rentpastC,  (if :month_id = 0 then                  isnull((select sum(isnull(rent,0)) from lsRent b," + str2 + "printserv prc where b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id                   " + str5 + str34 + "),0) else                   isnull((select sum(isnull(rent,0)) from lsRent b," + str2 + "printserv prc where b.code=0 and b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id                   " + str5 + " and month_id=:month_id " + str34 + "),0) endif) as calc0,  (if :month_id = 0 then                  isnull((select sum(isnull(rent,0)) from lsRent b," + str2 + "printserv prc where b.code<>0 and b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id                   " + str5 + str34 + "),0) else                   isnull((select sum(isnull(rent,0)) from lsRent b," + str2 + "printserv prc where b.code<>0 and b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id                   " + str5 + " and month_id=:month_id " + str34 + "),0) endif) as calcrecal,          (if :month_id = 0 then                  isnull((select sum(isnull(rent_vat,0)) from lsRent b," + str2 + "printserv prc where b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id                   " + str5 + str34 + "),0) else           isnull((select sum(isnull(rent_vat,0)) from lsRent b," + str2 + "printserv prc where b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id                   " + str5 + " and month_id=:month_id" + str34 + "),0) endif) as vat from dcPeriod p,cmpServiceParam r" + str6 + " left outer join lsService s on r.service_id=s.service_id and s.client_id=:client_id          and s.dbeg<=isnull((select first dbeg from lsService ss where r.service_id=ss.service_id and ss.client_id=:client_id and ss.complex_id=100 and ss.period_id=" + str10 + "                and ((dbeg<=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif) and dend>=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif)) or (dbeg<=(if :month_id = 0 then months(" + strPeriod + ",1)-1 else (select period_value from dcPeriod where period_id=(:month_id +1))-1 endif) and dend>=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif)))),(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif))          and s.dend>=isnull((select first dbeg from lsService ss where r.service_id=ss.service_id and ss.client_id=:client_id and ss.complex_id=100 and ss.period_id=" + str10 + "                and ((dbeg<=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif) and dend>=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif)) or (dbeg<=(if :month_id = 0 then months(" + strPeriod + ",1)-1 else (select period_value from dcPeriod where period_id=(:month_id +1))-1 endif) and dend>=(if :month_id = 0 then months(" + strPeriod + ",1)-1 else (select period_value from dcPeriod where period_id=(:month_id +1))-1 endif)))),(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif))          and s.complex_id=100 and s.period_id=" + str10 + "          left outer join cmpTariff t on s.tariff_id=t.tariff_id " + str15 + " and t.company_id=(select param_value from cmpParam where company_id=r.company_id and period_id=0 and param_id=201 and dbeg<=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif) and dend>=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif))          and t.period_id=(if :month_id=0 or not exists(select * from cmpTariff where period_id=" + strPeriodId + " and company_id=t.company_id and complex_id=t.complex_id and service_id=r.service_id and tariff_id=t.tariff_id and dbeg<(select period_value from dcPeriod where period_id=(:month_id +1)) and dend>=(select period_value from dcPeriod where period_id=:month_id)) then 0 else " + strPeriodId + " endif) and s.complex_id=t.complex_id           and t.dbeg<=isnull((select first dbeg from cmpTariff tt where s.tariff_id=tt.tariff_id and s.service_id=tt.service_id and tt.company_id=t.company_id and tt.complex_id=100 and tt.period_id=0                 and ((dbeg<=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif) and dend>=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif)) or (dbeg<=(if :month_id = 0 then months(" + strPeriod + ",1)-1 else (select period_value from dcPeriod where period_id=(:month_id +1))-1 endif) and dend>=(if :month_id = 0 then months(" + strPeriod + ",1)-1 else (select period_value from dcPeriod where period_id=(:month_id +1))-1 endif)))),(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif))          and t.dend>=isnull((select first dbeg from cmpTariff tt where s.tariff_id=tt.tariff_id and s.service_id=tt.service_id and tt.company_id=t.company_id and tt.complex_id=100 and tt.period_id=0                 and ((dbeg<=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif) and dend>=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif)) or (dbeg<=(if :month_id = 0 then months(" + strPeriod + ",1)-1 else (select period_value from dcPeriod where period_id=(:month_id +1))-1 endif) and dend>=(if :month_id = 0 then months(" + strPeriod + ",1)-1 else (select period_value from dcPeriod where period_id=(:month_id +1))-1 endif)))),(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif))where r.company_id=:company_id and r.complex_id=110 and p.period_id=" + strPeriodId + " ";
                  string str18 = "having calc<>0 or vat<>0 ";
                  if (this.checkBoxMinusOff.Checked)
                    str18 = "having (calc<>0 or vat<>0) and calc>0 ";
                  if (this.chbSost.Checked)
                    str56 = "select my.client_id,sp.service_id,(select service_name from dcService where service_id=my.sost) as sname,sp.sorter,sp.group_num,sp.receipt_id,sum(my.tax) as tax,my.edizm,my.isvat,sum(my.norm) as quota,           sum(my.volume) as volume,sum(my.calc) as calc,(if isnull(tax,0)<>0 then calc/tax else volume endif) as volume1,sum(my.vat) as vat, sum(my.rentC) as rent, sum(my.rentpastC) as rentpast, sum(my.calc0) as calc0, sum(my.calcrecal) as calcrecal from (" + str17 + ") as my,cmpServiceParam sp where sp.company_id=:company_id and sp.complex_id=110 and my.service_id=sp.service_id  group by my.client_id,sp.service_id,my.sost,sp.printshow,sp.group_num,edizm,sp.sorter,sp.receipt_id,my.isvat " + str18 + "order by client_id,sp.sorter,my.sost ";
                  else
                    str56 = "select my.client_id,sp.service_id,sp.printshow as sname,sp.sorter,sp.group_num,sp.receipt_id, sum(my.tax) tax,my.edizm,my.isvat,sum(my.norm) as quota, sum(my.volume) as volume,sum(my.calc) as calc,          (if isnull(tax,0)<>0 then calc/tax else volume endif) as volume1,sum(my.vat) as vat, sum(my.rentC) as rent, sum(my.rentpastC) as rentpast,  sum(my.calc0) as calc0, sum(my.calcrecal) as calcrecal from (" + str17 + ") as my,cmpServiceParam sp where sp.company_id=:company_id and sp.complex_id=110 and (my.service_id=sp.service_id or sp.service_id=my.gr)         and (group_num=0 or sp.calcalone=1)  group by my.client_id,sp.service_id,sp.printshow,sp.group_num,edizm,sp.sorter,sp.receipt_id,my.isvat " + str18 + "order by client_id,sp.sorter ";
                }
                this._tarif = this.report.GetDataSource("Tarif") as TableDataSource;
                this._tarif.SelectCommand = str56;
                IList list5 = this.session.CreateSQLQuery("select * from lsClient ls, lsBill b where ls.complex_id=110 and b.client_id=ls.Client_id and b.period_id=" + strPeriodId + " and b.bill_type=" + this.billType.ToString() + " and receipt_id in (" + ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString() + ") and ls.client_id=" + objArray1[0].ToString()).List();
                string selectCommand2 = this._tarif.SelectCommand;
                if (this.onClientCard)
                {
                  this.baseOrg = this.session.Get<BaseOrg>((object) this.baseOrg.BaseOrgId);
                }
                else
                {
                  this.baseOrg = new BaseOrg();
                  this.baseOrg.AdressPriority = Convert.ToInt32(this.session.CreateSQLQuery("select borg.AdressPriority from LsArenda la            left outer join base_org borg on la.idbaseorg=borg.idbaseorg where la.client_id=" + objArray1[0].ToString()).UniqueResult());
                }
                Address address = (Address) this.session.CreateQuery(string.Format("select new Address(c.ClientId,d.NameStr,h.NHome,h.HomeKorp,f.NFlat,c.SurFlat) from Home h, Str d, Flat f,LsClient c where h.Str=d and c.Home=h and c.Flat=f and c.ClientId={0} ", (object) objArray1[0].ToString())).List()[0];
                string adrString;
                switch (this.city)
                {
                  case 1:
                    adrString = string.Format("{0} д.{1} {2} кв.{3} {4}", (object) address.Str, (object) address.Number, (object) (address.Korp == "" || address.Korp == "0" || address.Korp == null ? "" : "к." + address.Korp), (object) address.Flat, (object) (address.SurFlat == "" || address.SurFlat == "0" || address.SurFlat == null ? "" : "комн." + address.SurFlat));
                    break;
                  case 23:
                    adrString = string.Format("{0} д.{1} {2}", (object) address.Str, (object) address.Number, address.Korp == "" || address.Korp == "0" || address.Korp == null ? (object) "" : (object) ("к." + address.Korp));
                    break;
                  default:
                    if (this._korTamplate)
                    {
                      adrString = string.Format("{0} д.{1} {2}", (object) address.Str, (object) address.Number, address.Korp == "" || address.Korp == "0" || address.Korp == null ? (object) "" : (object) ("к." + address.Korp));
                      break;
                    }
                    adrString = string.Format("{0} д.{1} {2} кв.{3} {4}", (object) address.Str, (object) address.Number, (object) (address.Korp == "" || address.Korp == "0" || address.Korp == null ? "" : "к." + address.Korp), (object) address.Flat, (object) (address.SurFlat == "" || address.SurFlat == "0" || address.SurFlat == null ? "" : "комн." + address.SurFlat));
                    break;
                }
                string addrOrg = this.AddressPost(objArray1[0].ToString());
                string str57 = this.AddressLegal(objArray1[0].ToString());
                string adrDom = "";
                try
                {
                  this.report.SetParameterValue("dolg", (object) "за коммунальные услуги и содержание нежилых помещений:");
                  if (this.chbStampDisplay.Checked)
                    this.report.SetParameterValue("Stamp", (object) true);
                  else
                    this.report.SetParameterValue("Stamp", (object) false);
                  string numOrg = "";
                  DateTime dataDog = new DateTime(2008, 1, 1);
                  string nameOrg = "";
                  if (list5.Count > 0)
                  {
                    if (this.RoGbVisible.Checked)
                      this.report.SetParameterValue("ROandGB", (object) 1);
                    else
                      this.report.SetParameterValue("ROandGB", (object) 0);
                    if ((Receipt) this.cbReceipt.SelectedItem != null)
                    {
                      switch (((Receipt) this.cbReceipt.SelectedItem).ReceiptId)
                      {
                        case 1:
                          this.report.SetParameterValue("dolg", (object) "за коммунальные услуги и содержание нежилых помещений:");
                          break;
                        case 50:
                          this.report.SetParameterValue("dolg", (object) "за капитальный ремонт:");
                          break;
                      }
                    }
                    this.report.SetParameterValue("Lastopl", (object) this.dtpSrok.Value.ToShortDateString());
                    this.report.SetParameterValue("dateDolg", (object) KvrplHelper.FirstDay(this.mpCurrentPeriod.Value).ToShortDateString());
                    this.report.SetParameterValue("ruk", (object) this.rukName);
                    this.report.SetParameterValue("bux", (object) this.buhName);
                    this.report.SetParameterValue("adr", (object) adrString);
                    LsArenda lsArenda = this.session.CreateCriteria(typeof (LsArenda)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("LsClient.ClientId", (object) (int) objArray1[0])).UniqueResult<LsArenda>();
                    try
                    {
                      adrDom = lsArenda.LsClient.Flat.Home.Address;
                      numOrg = lsArenda.DogovorNum;
                      dataDog = lsArenda.DogovorDate;
                      nameOrg = lsArenda.NameOrg;
                    }
                    catch (Exception ex)
                    {
                      Console.WriteLine(ex.Message);
                    }
                    this.report.SetParameterValue("DocType", (object) lsArenda.TypeArendaDocument.TypeDocument_name);
                    if (this.cbTypeAddress.SelectedIndex == 2)
                      this.report.SetParameterValue("adrpost", (object) addrOrg);
                    if (this.cbTypeAddress.SelectedIndex == 3)
                      this.report.SetParameterValue("adrpost", (object) adrString);
                    if (this.cbTypeAddress.SelectedIndex == 4)
                      this.report.SetParameterValue("adrpost", (object) str57);
                    if (this.cbTypeAddress.SelectedIndex == 0)
                      this.report.SetParameterValue("adrpost", (object) "");
                    if (this.cbTypeAddress.SelectedIndex == 1)
                    {
                      if (this.baseOrg.AdressPriority == 0)
                        this.report.SetParameterValue("adrpost", (object) str57);
                      if (this.baseOrg.AdressPriority == 1)
                        this.report.SetParameterValue("adrpost", (object) addrOrg);
                    }
                    if ((int) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId == 50)
                    {
                      SupplierClient supplierClient = this.session.CreateQuery("from SupplierClient sc where sc.LsClient.ClientId=:cl and sc.Supplier.BaseOrgId=-39999859").SetParameter<int>("cl", (int) objArray1[0]).UniqueResult<SupplierClient>();
                      if (supplierClient == null)
                      {
                        int num2 = (int) MessageBox.Show("Невозможно распечатать! Нет лицевого капремонта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        break;
                      }
                      this.report.SetParameterValue("OhlLSPer", (object) (supplierClient.SupplierClientId.ToString() + "/" + strPeriodId));
                      this.report.SetParameterValue("OhlLS", (object) supplierClient.SupplierClientId);
                      this.report.SetParameterValue("Ohl", (object) true);
                    }
                    else
                      this.report.SetParameterValue("Ohl", (object) false);
                    if (!this.rbAktDontInvoice.Checked)
                    {
                      for (int index2 = 0; (Decimal) index2 < this.nudCountBill.Value; ++index2)
                      {
                        if (printSettings == null)
                          printSettings = this.report.PrintSettings;
                        if (this.rbBill.Checked)
                          this.SaveOrPrintOrForEmail(this.report, Convert.ToInt32(objArray1[0]), Options.Period.PeriodId, 0, print, printSettings, false);
                        if (this.rbInvoice.Checked)
                          this.SaveOrPrintOrForEmail(this.report, Convert.ToInt32(objArray1[0]), Options.Period.PeriodId, 1, print, printSettings, false);
                      }
                    }
                  }
                  this.PrintActs(print, false, printSettings, adrString, this.clienttemp, Convert.ToInt32(objArray1[0].ToString()), false);
                  if (this.rbInvoice.Checked && list3.Count > 0)
                  {
                    this.ShowCorrect(print, strPeriod, strPeriodId, objArray1[0].ToString(), printSettings, Convert.ToInt32(objArray1[0]), false);
                    this.PrintActs(print, true, printSettings, adrString, this.clienttemp, Convert.ToInt32(objArray1[0].ToString()), false);
                  }
                  if (this.chbIsBlank.Checked)
                  {
                    this.ReportLoad(this.report, this.idBaseOrg, 4);
                    this.report.Dictionary.Connections[0].ConnectionString = this.connectionString;
                    (this.report.GetDataSource("Main") as TableDataSource).SelectCommand = this._main.SelectCommand;
                    if (print)
                    {
                      if (printSettings == null)
                      {
                        this.report.Prepare();
                        this.report.Print();
                        printSettings = this.report.PrintSettings;
                      }
                      else
                      {
                        printSettings.ShowDialog = false;
                        this.report.PrintSettings.Assign(printSettings);
                        this.report.Prepare();
                        this.report.Print();
                      }
                    }
                    else
                      this.report.Show();
                  }
                  this.PrintMessageForFR(print, printSettings, (Decimal) num4, mounthList1, nameOrg, numOrg, dataDog, addrOrg, adrDom, Convert.ToInt32(objArray1[0]), false);
                }
                catch (Exception ex)
                {
                  int num2 = (int) MessageBox.Show(ex.Message);
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, this.client);
      }
    }

    private void PrintMessageForFR(bool print, PrintSettings settings, Decimal dolg, IList mounthList1, string nameOrg, string numOrg, DateTime dataDog, string addrOrg, string adrDom, int lsClientArenda, bool many)
    {
      if (!this.chB_Offer_FR.Checked || !(dolg > Decimal.Zero))
        return;
      List<int> intList = mounthList1 as List<int>;
      int[] numArray = new int[1000];
      int index = 0;
      int num1 = int.MaxValue;
      foreach (object obj in (IEnumerable) mounthList1)
      {
        numArray[index] = Convert.ToInt32(obj);
        ++index;
        if (num1 > Convert.ToInt32(obj))
          num1 = Convert.ToInt32(obj);
      }
      object obj1 = this.session.CreateSQLQuery("select period_value from dba.dcPeriod p where p.period_id=" + (object) num1 ?? "").UniqueResult();
      object obj2 = this.session.CreateSQLQuery("select period_value from dba.dcPeriod p where p.period_id=" + (object) Options.Period.PeriodId ?? "").UniqueResult();
      int num2 = Options.Period.PeriodId - num1;
      if (num2 > 1)
      {
        switch (num2)
        {
          case 2:
            this.ReportLoad(this.report, this.idBaseOrg, 11);
            break;
          case 3:
            this.ReportLoad(this.report, this.idBaseOrg, 12);
            break;
          case 4:
            this.ReportLoad(this.report, this.idBaseOrg, 13);
            break;
          case 5:
            this.ReportLoad(this.report, this.idBaseOrg, 14);
            break;
          default:
            this.ReportLoad(this.report, this.idBaseOrg, 14);
            break;
        }
        this.report.SetParameterValue("nameorg", (object) (nameOrg + "\n" + addrOrg + "\nРуководителю"));
        this.report.SetParameterValue("dolgInt", (object) dolg);
        this.report.SetParameterValue("orgname", (object) nameOrg);
        this.report.SetParameterValue("adrarend", (object) adrDom);
        if (numOrg.Equals(""))
          this.report.SetParameterValue("numdog", (object) " .");
        else
          this.report.SetParameterValue("numdog", (object) (" на основании договора управления №" + numOrg + " от «01» января 2008 года."));
        this.report.SetParameterValue("dolgperiod", (object) (" c " + RuDateAndMoneyConverter.DateToTextLong((DateTime) obj1) + " по " + RuDateAndMoneyConverter.DateToTextLong((DateTime) obj2) ?? ""));
        this.report.SetParameterValue("dolgsum", (object) RuDateAndMoneyConverter.CurrencyToTxtFull(Convert.ToDouble(dolg), false));
        if (print)
        {
          if (settings == null)
          {
            if (many)
            {
              this._reportDictionary.Add(lsClientArenda, this.report);
            }
            else
            {
              this.report.Prepare();
              this.report.Print();
              settings = this.report.PrintSettings;
            }
          }
          else if (many)
          {
            this._reportDictionary.Add(lsClientArenda, this.report);
          }
          else
          {
            settings.ShowDialog = false;
            this.report.PrintSettings.Assign(settings);
            this.report.Prepare();
            this.report.Print();
          }
        }
        else if (many)
          this._reportDictionary.Add(lsClientArenda, this.report);
        else
          this.report.Show();
      }
    }

    private void PrintActs(bool print, bool cor, PrintSettings settings, string adrString, LsClient clienttemp, int lsClientArenda, bool many)
    {
      if (!this.chbIsAkt.Checked)
        return;
      for (int index = 0; (Decimal) index < this.nudCountAkt.Value; ++index)
      {
        if (this.archiveAllDoc)
          this.ReportLoad(this.report, this.idBaseOrg, 103);
        else if (this.city == 1 && !cor)
          this.ReportLoad(this.report, this.idBaseOrg, 3);
        else if (this.city == 1 & cor)
          this.ReportLoad(this.report, this.idBaseOrg, 31);
        else
          this.ReportLoad(this.report, this.idBaseOrg, 3);
        this.report.Dictionary.Connections[0].ConnectionString = this.connectionString;
        (this.report.GetDataSource("Main") as TableDataSource).SelectCommand = this._main.SelectCommand;
        (this.report.GetDataSource("Tarif") as TableDataSource).SelectCommand = this._tarif.SelectCommand;
        this.report.SetParameterValue("ruk", (object) this.rukName);
        this.report.SetParameterValue("PostBuh", (object) this.buhPost);
        this.report.SetParameterValue("BuhName", (object) this.buhName);
        this.report.SetParameterValue("PostRuk", (object) this.rukPost);
        this.report.SetParameterValue("RukName", (object) this.rukName);
        this.report.SetParameterValue("adr", (object) adrString);
        if (this.chbStampDisplay.Checked)
          this.report.SetParameterValue("Stamp", (object) true);
        else
          this.report.SetParameterValue("Stamp", (object) false);
        this.report.SetParameterValue("DocType", (object) this.session.CreateCriteria(typeof (LsArenda)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("LsClient.ClientId", (object) clienttemp.ClientId)).UniqueResult<LsArenda>().TypeArendaDocument.TypeDocument_name);
        if (many)
        {
          List<ReportArenda> reportList = this._reportList;
          ReportArenda reportArenda = new ReportArenda();
          reportArenda.LsArenda = lsClientArenda;
          reportArenda.BillType = 2;
          Report report = this.report;
          reportArenda.Report = report;
          int periodId = Options.Period.PeriodId;
          reportArenda.PeriodId = periodId;
          reportList.Add(reportArenda);
        }
        else
        {
          if (settings == null)
            settings = this.report.PrintSettings;
          this.SaveOrPrintOrForEmail(this.report, lsClientArenda, Options.Period.PeriodId, 2, print, settings, false);
        }
      }
    }

    private void PrintCoverLetter(bool print, PrintSettings settings, DateTime periodFrom, DateTime periodTo, string nameOrg, string numOrg, DateTime dataDog, string addrOrg, string adrDom, int lsClientArenda, bool many)
    {
    }

    private void LoadReportTemplateAndSetParameters(string templateDir)
    {
      this.report.Dictionary.Connections[0].ConnectionString = this.connectionString;
      this.report.SetParameterValue("period", (object) Options.Period.PeriodId);
      this.report.SetParameterValue("client", (object) this.client.ClientId);
      this.report.SetParameterValue("buhName", (object) this.buhName);
      this.report.SetParameterValue("rukName", (object) this.rukName);
      this.report.SetParameterValue("comp", (object) this.company.CompanyId);
      this.report.SetParameterValue("receipt", (object) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString());
      this.report.Prepare();
    }

    private void cbShablon_SelectedValueChanged(object sender, EventArgs e)
    {
      if (this.cbShablon.SelectedValue.Equals((object) "Шаблоны_ар\\BillKor.frx"))
      {
        this.checkBoxMinusOff.Visible = true;
        this.chbIsAkt.Enabled = true;
        this.nudCountAkt.Enabled = true;
      }
      else
      {
        this.checkBoxMinusOff.Checked = false;
        this.checkBoxMinusOff.Visible = false;
        this.chbIsAkt.Enabled = false;
        this.nudCountAkt.Enabled = false;
      }
    }

    private IList GetListBalanceCard(bool type, LsClient clienttemp)
    {
      string str1 = "";
      string str2 = "";
      string str3 = "";
      string str4 = "qwe.service_id, serv.service_name,";
      string str5 = ",qwe.sorter";
      string str6 = "group by qwe.service_id, serv.service_name, qwe.sorter";
      string str7 = "";
      string str8 = "";
      string str9 = "";
      string str10;
      string str11;
      if (type)
      {
        try
        {
          this.setupPeriod = Convert.ToInt32(this.session.CreateSQLQuery(string.Format("select setup_value from dba.fksetup where setup_id=3 and manager_id=(select manager_id from dba.dcCompany where company_id={0})", (object) clienttemp.Company.CompanyId)).List()[0]);
        }
        catch
        {
          this.setupPeriod = 1201;
        }
        try
        {
          this.setupValue = Convert.ToInt32(this.session.CreateSQLQuery(string.Format("select setup_value from dba.fksetup where setup_id=4 and manager_id=(select manager_id from dba.dcCompany where company_id={0})", (object) clienttemp.Company.CompanyId)).List()[0]);
        }
        catch
        {
          this.setupValue = 0;
        }
        str10 = !(Options.SortService == " s.ServiceId") ? "order by 2" : "order by 1";
        if ((int) Options.SpacingSaldo == 1)
        {
          str1 = ",supplier_id ";
          str2 = ",(select nameorg_min from dba.base_org where idbaseorg=supplier_id) as supplier_name ";
          str3 = ",supplier_name";
          str6 = str6 + str1 + str2;
        }
        str11 = "(if (" + (object) this.setupPeriod + " <= {0} and " + (object) this.setupValue + "<>2) then if " + (object) this.setupValue + " =1 then isnull(rent,0)+isnull(rentpast,0)+isnull(rent_comp,0) else isnull(rent,0)+isnull(rentpast,0)-isnull(msppast,0)+isnull(msppastpay,0)+isnull(rent_comp,0) endif else  isnull(rent,0)+isnull(rentpast,0)-isnull(msp,0)+isnull(msppay,0)-isnull(msppast,0)+isnull(msppastpay,0)+isnull(rent_comp,0) endif) ";
      }
      else
      {
        str10 = "";
        str1 = "";
        str2 = "";
        str3 = "";
        str4 = "";
        str5 = "";
        str6 = "";
        str8 = "+sum(qwe.rentpast)";
        str7 = "group by b.service_id,serv.service_id,client_id, balin,rent,rentpast,pay,rent_comp,balout,sp.sorter,supplier_id";
        str11 = this.setupPeriod > Options.Period.PeriodId || this.setupValue == 2 ? "sum(b.rent)-isnull((select sum(rm.rent) from dba.lsRentMSP rm where rm.period_id={0} and rm.client_id={1} and rm.Code=0 and rm.MSP_id in (select MSP_id from dba.DcMSP where period_id>{0})),0)-isnull((select sum(rm.rent) from dba.lsRentMSP rm where rm.period_id={0} and rm.client_id={1} and rm.Code<>0 and rm.MSP_id in (select MSP_id from dba.DcMSP where period_id>{0})),0)+sum(b.rent_comp)" : (this.setupValue != 1 ? "sum(b.rent)-isnull((select sum(rm.rent) from dba.lsRentMSP rm where rm.period_id={0} and rm.client_id={1} and rm.Code<>0 and rm.MSP_id in (select MSP_id from DcMSP where period_id>{0})),0)+sum(b.rent_comp)" : "sum(b.rent)+sum(b.rent_comp)");
      }
      string str12 = "";
      return this.session.CreateSQLQuery(string.Format("declare @date date  select @date=Period_value from dba.dcPeriod where period_id={0}  select " + str4 + " sum(qwe.balin),sum(qwe.rent),sum(qwe.rentpast),sum(qwe.nds),sum(qwe.msp),sum(qwe.msppay),sum(qwe.msppast),sum(qwe.msppastpay),sum(qwe.preditog)" + str8 + ",sum(qwe.itog),sum(qwe.pay),sum(qwe.rent_comp),sum(qwe.balout)" + str5 + str1 + str3 + " from ( select *,isnull((select receipt_id                   from dba.cmphmReceipt sp, dba.lsClient ls                   where ls.company_id=sp.company_id and ls.idhome=sp.idhome and sp.complex_id=ls.complex_id                   and  @date  between dbeg and dend and ls.client_id=t.client_id                   and (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.supplier_id=t.supplier_id),         isnull((select receipt_id                   from dba.cmphmReceipt sp, dba.lsClient ls                   where ls.company_id=sp.company_id and ls.idhome=sp.idhome and sp.complex_id=ls.complex_id                   and  @date  between dbeg and dend and ls.client_id=t.client_id and sp.service_id=0 and sp.supplier_id=t.supplier_id),         isnull((select receipt_id                  from dba.cmphmReceipt sp, dba.lsClient ls                   where ls.company_id=sp.company_id and ls.idhome=sp.idhome and sp.complex_id=ls.complex_id                   and  @date  between dbeg and dend and ls.client_id=t.client_id                   and (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.supplier_id=0),         isnull((select receipt_id                   from dba.cmphmReceipt sp, dba.lsClient ls                   where ls.company_id=sp.company_id and sp.idhome=0 and sp.complex_id=ls.complex_id                   and  @date  between dbeg and dend and ls.client_id=t.client_id                   and (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.supplier_id=t.supplier_id),         isnull((select receipt_id                  from dba.cmphmReceipt sp, dba.lsClient ls                   where ls.company_id=sp.company_id and sp.idhome=0 and sp.complex_id=ls.complex_id                   and  @date  between dbeg and dend and ls.client_id=t.client_id and sp.service_id=0 and sp.supplier_id=t.supplier_id),         isnull((select receipt_id                   from dba.cmphmReceipt sp, dba.lsClient ls                   where ls.company_id=sp.company_id and sp.idhome=0 and sp.complex_id=ls.complex_id                   and  @date  between dbeg and dend and ls.client_id=t.client_id                   and (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.supplier_id=0),         isnull((select receipt_id                   from dba.cmpServiceParam sp, dba.lsClient ls                   where ls.company_id=sp.company_id and ls.client_id=t.client_id and sp.complex_id=ls.complex_id                   and (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0))),         isnull((if t.service_id=0 and (select count(receipt_id) from dba.cmpReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and ls.client_id=t.client_id)=1 then          (select receipt_id from dba.cmpReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and ls.client_id=t.client_id) else 0 endif),0)))))))) rec from (   select serv.service_id, {1} client_id, balin, rent, rentpast,       isnull((select sum(rent_vat) from DBA.lsRent rm where rm.period_id={0} and client_id={1} and rm.service_id in (select service_id from DBA.dcService where root=serv.service_id)),0) as nds,       0 as msp, 0 as msppay, 0 as msppast, 0 as msppastpay," + str11 + "       as preditog,(if preditog<>0 then preditog else null endif) as itog,pay,rent_comp,balout,sp.sorter,supplier_id  " + str2 + "       from DBA.dcService serv        left outer join (select root,b.service_id,sum(b.Balance_in) as balin, sum(b.Rent)-sum(b.Rent_past) as rent,sum(b.Rent_past) as rentpast,                           sum(b.Payment) as pay,sum(b.Subsidy) as subs,sum(b.Balance_out) as balout,sum(b.Rent_comp) as rent_comp,supplier_id                             from DBA.lsBalance b, DBA.dcService s                            where b.service_id=s.service_id and b.period_id={0} and b.client_id={1} group by root,supplier_id,b.service_id                       ) as b on b.root=serv.service_id,        DBA.cmpServiceParam sp        where serv.root=0 and sp.service_id=serv.service_id and sp.company_id={2} and sp.complex_id={3}" + str9 + "       and (isnull(balin,0)<>0 or isnull(rent,0)<>0  or isnull(rentpast,0)<>0 or isnull(msp,0)<>0 or isnull(msppay,0)<>0 or isnull(msppast,0)<>0 or isnull(msppastpay,0)<>0  or isnull(pay,0)<>0 or isnull(balout,0)<>0)" + str7 + "   union all       select s.service_id,{1} client_id, sum(b.Balance_in) as balin, sum(b.Rent)-sum(b.Rent_past) as rent,sum(b.Rent_past) as rentpast,       isnull((select sum(rent_vat) from DBA.lsRent rm where rm.period_id={0} and client_id={1} and rm.service_id=0),0) as nds,        0 as msp, 0 as msppay, 0 as msppast, 0 as msppastpay," + str11 + "       as preditog,(if preditog<>0 then preditog else null endif) as itog,sum(b.Payment) as pay,sum(b.Rent_comp) as rent_comp,sum(b.Balance_out) as balout,0 as sorter,supplier_id  " + str2 + "       from DBA.dcService s        left outer join DBA.lsBalance b on b.service_id=s.service_id and b.period_id={0} and b.client_id={1} where s.service_id=0        group by s.service_id,supplier_id     union all       select serv.root, {1} client_id, 0 as balin, 0 as rent, 0 as rentpast, 0 nds, msp.msp, msp.msppay, msp.msppast, msp.msppastpay, 0 preditog, 0 itog, 0 pay,0 rent_comp,               0 balout,sp.sorter,msp.Supplier_id        from dba.dcservice serv       left join (select sum(q.msp) msp,sum(q.msppay) msppay,sum(q.msppast) msppast,sum(q.msppastpay) msppastpay, q.sid, q.supplier_id                   from (                       select sum(rent) msp,0 msppay,0 msppast,0 msppastpay, rm.Service_id sid, rm.supplier_id                        from DBA.lsRentMSP rm, DBA.dcMSP m,DBA.dcService serv                        where rm.msp_id=m.msp_id and rm.period_id={0} and client_id={1} and code=0 and rm.service_id in (select service_id from DBA.dcService where root=serv.service_id)                        group by rm.Service_id, rm.supplier_id                       union                       select 0 msp, sum(rent) msppay,0 msppast,0 msppastpay, rm.Service_id sid , rm.supplier_id                        from DBA.lsRentMSP rm,DBA.dcMSP m, DBA.dcService serv                        where rm.msp_id=m.msp_id and rm.period_id={0} and client_id={1} and code=0 and m.mspperiod_id<={0} and rm.service_id in (select service_id from DBA.dcService where root=serv.service_id)                        group by rm.Service_id, rm.supplier_id                        union                       select 0 msp,0 msppay ,sum(rent) msppast,0 msppastpay, rm.Service_id sid , rm.supplier_id                        from DBA.lsRentMSP rm,DBA.dcMSP m, DBA.dcService serv                        where rm.msp_id=m.msp_id and rm.period_id={0} and client_id={1} and code<>0 and rm.service_id in (select service_id from DBA.dcService where root=serv.service_id)                        group by rm.Service_id, rm.supplier_id                        union                          select 0 msp,0 msppay ,0 msppast,sum(rent) msppastpay, rm.Service_id sid , rm.supplier_id                        from DBA.lsRentMSP rm,DBA.dcMSP m, DBA.dcService serv                        where rm.msp_id=m.msp_id and rm.period_id={0} and client_id={1} and code<>0 and m.mspperiod_id<={0} and rm.service_id in (select service_id from DBA.dcService where root=serv.service_id)                        group by rm.Service_id, rm.supplier_id                        ) q                   group by q.sid, q.supplier_id) msp on msp.sid=serv.service_id       , DBA.cmpServiceParam sp        where sp.service_id=serv.root and sp.company_id={2} and sp.complex_id={3} and sp.sendrent<>1 and (isnull(msp,0)<>0 or isnull(msppay,0)<>0 or isnull(msppast,0)<>0 or isnull(msppastpay,0)<>0)  ) t " + str12 + ") qwe inner join dba.dcService serv on serv.service_id=qwe.service_id where ((qwe.balin<>0 or qwe.rent<>0 or qwe.rentpast<>0 OR isnull(qwe.msp,0)<>0 OR isnull(qwe.msppay,0)<>0 OR isnull(qwe.msppast,0)<>0 OR isnull(qwe.msppastpay,0)<>0 OR isnull(qwe.pay,0)<>0 OR isnull(qwe.balout,0)<>0) OR qwe.service_id<>0)" + str6 + " " + str10, (object) Options.Period.PeriodId, (object) clienttemp.ClientId, (object) clienttemp.Company.CompanyId, (object) clienttemp.Complex.IdFk)).List();
    }

    private void rbAktDontInvoice_CheckedChanged(object sender, EventArgs e)
    {
      if (this.rbAktDontInvoice.Checked)
      {
        this.butSaveReport.Enabled = false;
        this.billType = (short) 2;
        this.lblSrok.Enabled = false;
        this.dtpSrok.Enabled = false;
        this.chbIsAkt.Enabled = true;
        this.nudCountAkt.Enabled = true;
        this.chbCorrect.Enabled = true;
        this.chbCorrect.Checked = false;
        this.chbIsBlank.Enabled = false;
        this.chbIsBlank.Checked = false;
        this.chbIsAkt.Checked = true;
        this.gbMountBreak.Visible = false;
      }
      this.UpdateData();
      this.ShowList();
    }

    private void rbAllDoc_Click(object sender, EventArgs e)
    {
      this.UpdateData();
      this.ShowList();
      if (!this.rbAllDoc.Checked)
        return;
      this.billType = (short) -1;
      this.archiveAllDoc = false;
      this.lblSrok.Enabled = true;
      this.dtpSrok.Enabled = true;
      this.chbIsAkt.Enabled = true;
      this.nudCountAkt.Enabled = true;
      this.chbCorrect.Enabled = true;
      this.chbIsBlank.Enabled = false;
      this.chbIsBlank.Checked = false;
      this.chbIsAkt.Checked = true;
      this.butSaveReport.Enabled = true;
      this.gbMountBreak.Visible = true;
    }

    private void PrintAllDocument(bool print)
    {
      this._licClientArenda.Clear();
      this._reportList.Clear();
      try
      {
        Period period1 = (Period) null;
        Period period2 = (Period) null;
        if (this.chbMec.Checked)
        {
          period1 = KvrplHelper.SaveCurrentPeriod(this.mpFromPeriod.Value);
          period2 = KvrplHelper.SaveCurrentPeriod(this.mpToPeriod.Value);
        }
        if (period1 == null && period2 == null)
        {
          period1 = new Period(Options.Period.PeriodId, Options.Period.PeriodName);
          period2 = new Period(Options.Period.PeriodId, Options.Period.PeriodName);
        }
        if (period1.PeriodId > period2.PeriodId)
        {
          int num1 = (int) MessageBox.Show("Неверно указан диапазон по месяцам");
        }
        else
        {
          for (int periodId = period1.PeriodId; periodId <= period2.PeriodId; ++periodId)
          {
            Options.Period = this.session.Get<Period>((object) periodId);
            if (!new List<int>() { 70, 71, 72, 73, 74, 75, 76, 77, 78, 79 }.Contains((int) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId))
            {
              this.session.Clear();
              string str1 = "tmpReceipt";
              string str2 = Options.Login + ".tmpReceipt";
              string str3 = "";
              string str4 = "";
              string str5 = "";
              string str6 = "";
              string str7 = "";
              string str8 = "";
              string queryString1 = "";
              IList list1 = (IList) new ArrayList();
              DateTime dateTime1 = Options.Period.PeriodName.Value;
              dateTime1.AddMonths(1);
              DateTime? periodName = Options.Period.PeriodName;
              dateTime1 = periodName.Value;
              // ISSUE: explicit reference operation
              // ISSUE: variable of a reference type
              DateTime local = dateTime1;
              periodName = Options.Period.PeriodName;
              int months = -periodName.Value.Month + 1;
              // ISSUE: explicit reference operation
              DateTime dt = local.AddMonths(months);
              string str9 = "";
              string str10 = "cast( '";
              periodName = Options.Period.PeriodName;
              string str11 = KvrplHelper.DateToBaseFormat(periodName.Value).ToString();
              string str12 = "'as date)";
              string strPeriod = str10 + str11 + str12;
              string strPeriodId = Options.Period.PeriodId.ToString();
              string str13 = "(if :month_id=0 or not exists(select * from lsService where period_id=" + strPeriodId + " and client_id=:client_id and service_id=r.service_id and dbeg<(select period_value from dcPeriod where period_id=(:month_id +1)) and dend>=(select period_value from dcPeriod where period_id=:month_id)) then 0 else " + strPeriodId + " endif)";
              string str14 = " and bill_type in (2,3,1)";
              try
              {
                Convert.ToInt32(this.nudCountBill.Text);
                Convert.ToInt32(this.nudCountAkt.Text);
              }
              catch
              {
                return;
              }
              switch (this.level)
              {
                case 0:
                case 1:
                  queryString1 = "select min(period_id) as period_id from cmpPeriod where company_id in (select company_id from dcCompany where rnn_id=" + this.raion.IdRnn.ToString() + " and complex_id in (100,101,102,108))";
                  break;
                case 2:
                case 3:
                case 4:
                  queryString1 = "select min(period_id) as period_id from cmpPeriod where company_id=" + this.company.CompanyId.ToString() + " and complex_id in (100,101,102,108)";
                  break;
              }
              if ((int) this.level < 4 && Options.Period.PeriodId > Convert.ToInt32(this.session.CreateSQLQuery(queryString1).List()[0]))
              {
                int num2 = (int) MessageBox.Show("Невозможно распечатать счета, месяц не закрыт", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
              }
              string str15 = this.MainUsl();
              try
              {
                this.session.CreateSQLQuery("call droptmp(1,'" + str1 + "arenda',current user);create " + str3 + " table " + str2 + "arenda(client_id integer)" + str4 + ";create index client_id on " + str2 + "arenda (client_id)").ExecuteUpdate();
                this.session.Flush();
              }
              catch (Exception ex)
              {
                KvrplHelper.WriteLog(ex, (LsClient) null);
              }
              if (this.dgvList.SelectedRows.Count == 0)
              {
                if (Options.BaseName == "uk1" || Options.BaseName == "ukaka" || this.archiveAllDoc)
                  str7 = "and (select sum(balance_in)from lsBalance where period_id=" + strPeriodId + " and client_id=ls.client_id )<>0  and (select sum(balance_out)from lsBalance where period_id=" + strPeriodId + " and client_id=ls.client_id )<>0  ";
                else
                  str7 = "      and (select sum(rent) from lsRent where period_id=" + strPeriodId + " and client_id=ls.client_id )<>0 ";
                string str16 = "insert into " + str2 + "arenda select ls.client_id from lsClient ls left outer join lsBill b on b.client_id=ls.Client_id and b.period_id=" + strPeriodId + " and b.bill_type=" + this.billType.ToString() + ",dcCompany c, di_str d, homes h, homelink hl, flats f, LsArenda a  where ls.company_id=c.company_id and ls.complex_id=110 and " + str15 + "      and hl.idhome=h.idhome and hl.codeu=ls.company_id and h.idhome=ls.idhome and h.idstr=d.idstr and ls.idflat=f.idflat and ls.client_id=a.client_id " + str7;
                if (this.chbOnlyNew.Checked)
                  str16 += " and bill_num is null";
                this.session.CreateSQLQuery(str16 + " order by dogovor_num").ExecuteUpdate();
                this.session.Flush();
              }
              else
              {
                foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgvList.SelectedRows)
                  this.session.CreateSQLQuery("insert into " + str2 + "arenda (client_id) values (" + selectedRow.Cells[0].Value.ToString() + ")").ExecuteUpdate();
                this.session.Flush();
              }
              try
              {
                string str16 = (int) this.level <= 1 ? "" : str8 + " and sp.company_id=" + this.company.CompanyId.ToString();
                string queryString2 = "call droptmp(1,'" + str1 + "nabor',current user); call droptmp(1,'" + str1 + "printserv',current user); create " + str3 + " table " + str2 + "nabor(company_id integer not null,root integer,service_id integer,supplier_id integer)" + str4 + "; create index company_id on " + str2 + "nabor (company_id); create index supplier_id on " + str2 + "nabor (supplier_id); create index sost on " + str2 + "nabor (company_id,supplier_id); create index sost1 on " + str2 + "nabor (root,service_id,supplier_id); create " + str3 + " table " + str2 + "printserv(client_id integer,idhome integer,root integer,service_id integer,supplier_id integer,receipt_id integer)" + str4 + "; create index client_id on " + str2 + "printserv (client_id); create index root on " + str2 + "printserv (root); create index clr on " + str2 + "printserv (client_id,receipt_id); create index css on " + str2 + "printserv (client_id,service_id,supplier_id); create index crs on " + str2 + "printserv (client_id,root,service_id,supplier_id); insert into " + str2 + "nabor select distinct sp.company_id,s.root as root,s.service_id,ss.supplier_id   from dcService s,cmpServiceParam sp, cmpSupplier ss   where  sp.service_id=s.root and sp.complex_id=110 " + str16 + "    and ss.company_id=(select first param_value from cmpParam where period_id=0 and dbeg<=" + strPeriod + " and dend>=" + strPeriod + " and param_id=211 and company_id=sp.company_id)     and (ss.service_id=sp.service_id or ss.service_id=(select crossservice_id from cmpCrossService where company_id=sp.company_id and dbeg<=" + strPeriod + " and dend>=" + strPeriod + "    and service_id=sp.service_id and crosstype_id=6)) and ss.supplier_id<>0 union all select sp.company_id,s.root as root,s.service_id,0    from dcService s,cmpServiceParam sp   where  sp.service_id=s.root " + str16 + " and sp.complex_id=110;  insert into " + str2 + "printserv  select ls.client_id, ls.idhome, tmp.root as root, tmp.service_id, tmp.supplier_id, 0 from " + str2 + "nabor tmp, dcCompany c, lsClient ls, flats f where ls.company_id=tmp.company_id     and (exists (select 1 from lsBalance where client_id=ls.client_id and supplier_id=tmp.supplier_id) or    exists (select 1 from lsSupplier where client_id=ls.client_id and supplier_id=tmp.supplier_id) or supplier_id=0)    and ls.complex_id=110 and ls.company_id=c.company_id and ls.idflat=f.idflat  and " + str15 + ";";
                this.session.Clear();
                this.session.CreateSQLQuery(queryString2).ExecuteUpdate();
                this.session.CreateSQLQuery(KvrplHelper.Prc_FillReceipt(str2 + "printserv", strPeriod)).ExecuteUpdate();
                if ((Receipt) this.cbReceipt.SelectedItem != null)
                  this.session.CreateSQLQuery("delete from " + str2 + "printserv where receipt_id  not in (" + ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString() + ")").ExecuteUpdate();
                this.session.Flush();
              }
              catch (Exception ex)
              {
                KvrplHelper.WriteLog(ex, (LsClient) null);
              }
              for (int index = 0; index != 4; ++index)
              {
                if (this.chbCorrect.Checked && index == 3)
                  str9 = " and code=0";
                if (index == 0)
                {
                  this.billType = (short) 1;
                  str14 = " and bill_type=1";
                }
                if ((uint) index > 0U)
                {
                  this.billType = (short) 2;
                  str14 = " and bill_type in (2,3)";
                }
                string str16;
                if (!this.txbNum.Enabled || this.txbNum.Text == "")
                {
                  str16 = "isnull((select max(bill_num) from lsBill where period_id>=(select period_id from dcPeriod where period_value=cast('" + KvrplHelper.DateToBaseFormat(dt).ToString() + "' as date)) " + str14 + "),0)+number(),";
                }
                else
                {
                  try
                  {
                    Convert.ToInt32(this.txbNum.Text);
                    str16 = this.txbNum.Text + ",";
                  }
                  catch
                  {
                    int num2 = (int) MessageBox.Show("Некорректный номер счета!", "Внимание", MessageBoxButtons.OK);
                    return;
                  }
                }
                if (Options.BaseName != "uk1" && Options.BaseName != "ukaka" && !this.archiveAllDoc)
                  str7 = "      and (select sum(rent) from lsRent where period_id=" + strPeriodId + " and client_id=ls.client_id" + str9 + " )<>0 ";
                string str17 = "insert into lsBill select ls.client_id, " + strPeriodId + ", 0, " + this.billType.ToString() + ", " + str16 + "cast('" + KvrplHelper.DateToBaseFormat(this.dtpDate.Value).ToString() + "' as date), user, today(), " + ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString() + " from lsClient ls, LsArenda a where ls.client_id=a.client_id " + str7 + "               and ls.client_id in (select client_id from " + str2 + "arenda) ";
                string str18;
                if ((int) this.billType == 1)
                  str18 = str17 + " and ls.client_id not in (select client_id from lsBill where period_id=" + strPeriodId + " and bill_type=1 and receipt_id=" + ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString() + ")";
                else
                  str18 = str17 + " and ls.client_id not in (select client_id from lsBill where period_id=" + strPeriodId + " and (bill_type=2 or (bill_type=3 and  bill_num<=isnull((select max(bill_num) from lsBill where period_id>=(select period_id from dcPeriod where period_value=cast('" + KvrplHelper.DateToBaseFormat(dt).ToString() + "' as date)) " + str14 + "),0))) and receipt_id=" + ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString() + ")";
                this.session.CreateSQLQuery(str18 + " order by dogovor_num").ExecuteUpdate();
                this.session.Flush();
                list1 = this.session.CreateSQLQuery("select isnull(max(bill_num),0) as maxnum from lsBill where period_id>=(select period_id from dcPeriod where period_value=cast('" + KvrplHelper.DateToBaseFormat(dt).ToString() + "' as date))     and bill_type in (2,3) and receipt_id=" + ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString() + " ").List();
              }
              string str19 = " and bill_type in (2,3,1)";
              string queryString3;
              if (this.archiveAllDoc)
                queryString3 = "select distinct b.Client_id, a.dogovor_num from lsClient ls, lsBill b, LsArenda a ,lsParam lp where lp.period_id=0 and lp.param_id=107 and lp.param_value in(4,5)  and lp.client_id=ls.Client_id and b.client_id=ls.Client_id and ls.client_id=a.client_id  and b.period_id=" + strPeriodId + str19 + "     and ls.client_id in (select client_id from " + str2 + "arenda) and receipt_id=" + ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString() + " order by a.dogovor_num";
              else
                queryString3 = "select distinct b.Client_id, a.dogovor_num from lsClient ls, lsBill b, LsArenda a where  b.client_id=ls.Client_id and ls.client_id=a.client_id and b.period_id=" + strPeriodId + str19 + "     and ls.client_id in (select client_id from " + str2 + "arenda) and receipt_id=" + ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString() + " order by a.dogovor_num";
              IList list2 = this.session.CreateSQLQuery(queryString3).List();
              if (list2.Count == 0)
              {
                int num2 = (int) MessageBox.Show("Нет данных для отчета.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
              }
              PrintSettings settings = (PrintSettings) null;
              foreach (object[] objArray1 in (IEnumerable) list2)
              {
                for (int index1 = 0; index1 != 4; ++index1)
                {
                  if (this.chbCorrect.Checked && index1 == 3)
                    ;
                  if (index1 == 0)
                  {
                    this.billType = (short) 1;
                    str19 = " and bill_type=1";
                  }
                  if ((uint) index1 > 0U)
                  {
                    this.billType = (short) 2;
                    str19 = " and bill_type in (2,3)";
                  }
                  IList<Bill> source = this.session.CreateQuery(string.Format("from Bill where Period.PeriodId={0} and client_id={1} and BillType=3 and Receipt.ReceiptId={3}", (object) Options.Period.PeriodId, (object) objArray1[0].ToString(), (object) this.billType, (object) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId)).List<Bill>();
                  short receiptId;
                  if (this.chbCorrect.Checked && index1 == 3 && source.Count<Bill>() == 0)
                  {
                    ISession session = this.session;
                    string[] strArray = new string[31];
                    strArray[0] = "insert into dba.lsBill select ls.client_id,";
                    strArray[1] = strPeriodId;
                    strArray[2] = ", (select first period_id from lsBill where period_id<=";
                    strArray[3] = strPeriodId;
                    strArray[4] = "-1 and client_id=ls.client_id and bill_type=2 and   (month_id=0 or month_id=period_id) order by period_id desc), 3 ,isnull((select max(bill_num) from lsBill where period_id>=  (select period_id from dcPeriod where period_value=cast('";
                    strArray[5] = KvrplHelper.DateToBaseFormat(dt);
                    strArray[6] = "' as date)) ";
                    strArray[7] = str19;
                    strArray[8] = "),0)+number(),   cast('";
                    strArray[9] = KvrplHelper.DateToBaseFormat(this.dtpDate.Value).ToString();
                    strArray[10] = "' as date), user, today(), ";
                    int index2 = 11;
                    receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
                    string str16 = receiptId.ToString();
                    strArray[index2] = str16;
                    int index3 = 12;
                    string str17 = " from lsClient ls  , LsArenda a where ls.client_id=a.client_id and ls.client_id in (select client_id from lsBill where period_id=";
                    strArray[index3] = str17;
                    int index4 = 13;
                    string str18 = strPeriodId;
                    strArray[index4] = str18;
                    int index5 = 14;
                    string str20 = " and  client_id=";
                    strArray[index5] = str20;
                    int index6 = 15;
                    string str21 = objArray1[0].ToString();
                    strArray[index6] = str21;
                    int index7 = 16;
                    string str22 = " and     (bill_type=3 or (bill_type=2 and bill_num<=";
                    strArray[index7] = str22;
                    int index8 = 17;
                    string str23 = list1[0].ToString();
                    strArray[index8] = str23;
                    int index9 = 18;
                    string str24 = ")) and receipt_id=";
                    strArray[index9] = str24;
                    int index10 = 19;
                    receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
                    string str25 = receiptId.ToString();
                    strArray[index10] = str25;
                    int index11 = 20;
                    string str26 = ")    and (select sum(rent) from lsRent lb,";
                    strArray[index11] = str26;
                    int index12 = 21;
                    string str27 = str2;
                    strArray[index12] = str27;
                    int index13 = 22;
                    string str28 = "printserv prc where period_id=";
                    strArray[index13] = str28;
                    int index14 = 23;
                    string str29 = strPeriodId;
                    strArray[index14] = str29;
                    int index15 = 24;
                    string str30 = " and lb.client_id=ls.client_id and     lb.client_id=prc.client_id and prc.service_id=lb.service_id and lb.supplier_id=prc.supplier_id and code<>0)<>0 and ls.client_id in (select client_id from ";
                    strArray[index15] = str30;
                    int index16 = 25;
                    string str31 = str2;
                    strArray[index16] = str31;
                    int index17 = 26;
                    string str32 = "arenda)     and isnull((select first period_id from lsBill where period_id<=";
                    strArray[index17] = str32;
                    int index18 = 27;
                    string str33 = strPeriodId;
                    strArray[index18] = str33;
                    int index19 = 28;
                    string str34 = "-1 and client_id=ls.client_id and bill_type=2 and     (month_id=0 or month_id=period_id) and receipt_id=";
                    strArray[index19] = str34;
                    int index20 = 29;
                    receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
                    string str35 = receiptId.ToString();
                    strArray[index20] = str35;
                    int index21 = 30;
                    string str36 = " order by period_id desc),0)<>0 order by dogovor_num";
                    strArray[index21] = str36;
                    string queryString2 = string.Concat(strArray);
                    session.CreateSQLQuery(queryString2).ExecuteUpdate();
                    this.session.Flush();
                  }
                  if (!this._licClientArenda.Contains(Convert.ToInt32(objArray1[0])))
                    this._licClientArenda.Add(Convert.ToInt32(objArray1[0]));
                  this.clienttemp = this.session.Get<LsClient>((object) Convert.ToInt32(objArray1[0]));
                  this.report = new Report();
                  if (this.archiveAllDoc)
                  {
                    if (index1 == 0)
                      this.ReportLoad(this.report, this.idBaseOrg, 101);
                    if (index1 == 1)
                      this.ReportLoad(this.report, this.idBaseOrg, 102);
                    if (index1 == 2)
                      this.ReportLoad(this.report, this.idBaseOrg, 103);
                    if (index1 == 3)
                      continue;
                  }
                  else
                  {
                    if (index1 == 0)
                      this.ReportLoad(this.report, this.idBaseOrg, 1);
                    if (index1 == 1)
                      this.ReportLoad(this.report, this.idBaseOrg, 2);
                    if (index1 == 2)
                      this.ReportLoad(this.report, this.idBaseOrg, 3);
                    if (index1 == 3)
                      this.ReportLoad(this.report, this.idBaseOrg, 21);
                  }
                  if (this.rbAktDontInvoice.Checked)
                    this.ReportLoad(this.report, this.idBaseOrg, 2);
                  this.report.Dictionary.Connections[0].ConnectionString = this.connectionString;
                  ISession session1 = this.session;
                  string[] strArray1 = new string[6]{ "select * from lsBill where period_id=", strPeriodId, " and client_id=", objArray1[0].ToString(), " and bill_type=3 and receipt_id=", null };
                  int index22 = 5;
                  receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
                  string str37 = receiptId.ToString();
                  strArray1[index22] = str37;
                  string queryString4 = string.Concat(strArray1);
                  IList list3 = session1.CreateSQLQuery(queryString4).List();
                  string str38 = list3.Count <= 0 ? "" : " and code=0";
                  string str39 = "";
                  string str40 = ", isnull((select receipt_id                   from dba.cmphmReceipt sp, dba.lsClient ls                   where ls.company_id=sp.company_id and ls.idhome=sp.idhome and sp.complex_id=ls.complex_id                   and  @date  between dbeg and dend and ls.client_id=t.client_id                   and (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.supplier_id=t.supplier_id),         isnull((select receipt_id                   from dba.cmphmReceipt sp, dba.lsClient ls                   where ls.company_id=sp.company_id and ls.idhome=sp.idhome and sp.complex_id=ls.complex_id                   and  @date  between dbeg and dend and ls.client_id=t.client_id and sp.service_id=0 and sp.supplier_id=t.supplier_id),         isnull((select receipt_id                  from dba.cmphmReceipt sp, dba.lsClient ls                   where ls.company_id=sp.company_id and ls.idhome=sp.idhome and sp.complex_id=ls.complex_id                   and  @date  between dbeg and dend and ls.client_id=t.client_id                   and (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.supplier_id=0),         isnull((select receipt_id                   from dba.cmphmReceipt sp, dba.lsClient ls                   where ls.company_id=sp.company_id and sp.idhome=0 and sp.complex_id=ls.complex_id                   and  @date  between dbeg and dend and ls.client_id=t.client_id                   and (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.supplier_id=t.supplier_id),         isnull((select receipt_id                  from dba.cmphmReceipt sp, dba.lsClient ls                   where ls.company_id=sp.company_id and sp.idhome=0 and sp.complex_id=ls.complex_id                   and  @date  between dbeg and dend and ls.client_id=t.client_id and sp.service_id=0 and sp.supplier_id=t.supplier_id),         isnull((select receipt_id                   from dba.cmphmReceipt sp, dba.lsClient ls                   where ls.company_id=sp.company_id and sp.idhome=0 and sp.complex_id=ls.complex_id                   and  @date  between dbeg and dend and ls.client_id=t.client_id                   and (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0)) and sp.supplier_id=0),         isnull((select receipt_id                   from dba.cmpServiceParam sp, dba.lsClient ls                   where ls.company_id=sp.company_id and ls.client_id=t.client_id and sp.complex_id=ls.complex_id                   and (sp.service_id=t.service_id or sp.service_id=(select root from dba.dcService where service_id=t.service_id and root<>0))),         isnull((if t.service_id=0 and (select count(receipt_id) from dba.cmpReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and ls.client_id=t.client_id)=1 then          (select receipt_id from dba.cmpReceipt sp,dba.lsClient ls where ls.company_id=sp.company_id and ls.client_id=t.client_id) else 0 endif),0)))))))) rec";
                  string str41;
                  if (Options.BaseName == "uk1" || Options.BaseName == "ukaka" || this.archiveAllDoc)
                  {
                    str41 = " qqq.nds as buyervat, qqq.calc as buyercalc, qqq.calc as buyerdolg, ";
                    string format = "left join (select qwe.client_id,sum(qwe.balout) calc,-round((sum(qwe.balout)/1.18)-sum(qwe.balout),2) nds,round((sum(qwe.balout)/1.18),2) rentWithoutNds            from (select t.* {3}               from (                         select serv.service_id, {0} client_id,balout,supplier_id                        from DBA.dcService serv                           left outer join (select root,b.service_id,sum(b.Balance_out) as balout,supplier_id                                            from DBA.lsBalance b, DBA.dcService s                                               where b.service_id=s.service_id and b.period_id={2} and b.client_id={0} group by root,supplier_id,b.service_id                                          ) as b on b.root=serv.service_id,                           DBA.cmpServiceParam sp                           where serv.root=0 and sp.service_id=serv.service_id and sp.company_id={1} and sp.complex_id=110                              and isnull(balout,0)<>0                             union all                          select s.service_id, {0} client_id, sum(b.Balance_out) as balout,supplier_id                        from DBA.dcService s                           left outer join DBA.lsBalance b on b.service_id=s.service_id and b.period_id={2} and b.client_id={0} where s.service_id=0                           group by s.service_id,supplier_id                      )t                    )qwe                where rec={4}                   group by qwe.client_id            )qqq on qqq.client_id=ls.client_id ";
                    object[] objArray2 = new object[5]{ (object) objArray1[0].ToString(), (object) this.clienttemp.Company.CompanyId, (object) strPeriodId, (object) str40, null };
                    int index2 = 4;
                    receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
                    string str16 = receiptId.ToString();
                    objArray2[index2] = (object) str16;
                    str39 = string.Format(format, objArray2);
                  }
                  else
                  {
                    str41 = "       (select sum(payment) from lsBalance lb, " + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + "  and         lb.client_id=prc.client_id and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id) as buyerpayment,        (select sum(balance_in) from lsBalance lb, " + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + "  and         lb.client_id=prc.client_id and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id) as buyerdolg,        (if b.month_id=0 then          (select sum(rent) from lsRent lb," + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + " and lb.client_id=prc.client_id and            lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id " + str38 + ")         else (select sum(rent) from lsRent lb," + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + " and lb.client_id=prc.client_id and            month_id=b.month_id and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id " + str38 + ") endif) as buyercalc,        (if b.month_id=0 then           (select sum(rent_vat) from lsRent lb," + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + " and lb.client_id=prc.client_id and                lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id " + str38 + ")        else (select sum(rent_vat) from lsRent lb," + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + " and month_id=b.month_id and                lb.client_id=prc.client_id and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id " + str38 + ") endif) as buyervat, ";
                    string str16 = "";
                    string str17 = "";
                    if (this.checkBoxMinusOff.Checked)
                    {
                      str16 = " and lb.rent > 0 ";
                      str17 = " and (select sum(isnull(lb2.rent,0)) from lsRent lb2," + str2 + "printserv prc where lb2.client_id=prc.client_id and lb2.service_id=prc.service_id and lb2.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and lb2.client_id=ls.client_id                   " + str5 + " and lb2.month_id=b.month_id " + str38 + ") > 0 ";
                    }
                    if ((this.city == 23 || this._korTamplate) && this.checkBoxMinusOff.Checked)
                      str41 = "       (select sum(payment) from lsBalance lb, " + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + "  and         lb.client_id=prc.client_id and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id) as buyerpayment,        (select sum(balance_in) from lsBalance lb, " + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + "  and         lb.client_id=prc.client_id and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id) as buyerdolg,  (select sum(lb.rent) from dba.lsBalance lb, " + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + str16 + " and lb.client_id=prc.client_id and            lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id " + str38 + ") as buyercalc,        (if b.month_id=0 then           (select sum(rent_vat) from lsRent lb," + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + " and lb.client_id=prc.client_id and                lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id " + str38 + ")        else (select sum(rent_vat) from lsRent lb," + str2 + "printserv prc where lb.client_id=ls.client_id and period_id=" + strPeriodId + " and month_id=b.month_id and                lb.client_id=prc.client_id and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id " + str38 + ") endif) as buyervat, ";
                  }
                  this._main = this.report.GetDataSource("Main") as TableDataSource;
                  string str42 = "";
                  string str43 = "";
                  string str44 = "";
                  string str45 = "";
                  string str46;
                  string str47;
                  string str48;
                  string str49;
                  string str50;
                  string str51;
                  string str52;
                  string str53;
                  if ((int) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId == 50)
                  {
                    ohlAccounts ohlAccounts = this.session.Get<ohlAccounts>((object) Convert.ToInt32(this.session.CreateQuery("select hm from HomeParam hm where hm.Param.ParamId=309 and hm.Company.CompanyId=:cmp and hm.Home.IdHome=:home and DEnd=(select max(DEnd) from HomeParam hm where hm.Param.ParamId=309 and hm.Company.CompanyId=:cmp and hm.Home.IdHome=:home)").SetParameter<short>("cmp", this.clienttemp.Company.CompanyId).SetParameter<int>("home", this.clienttemp.Home.IdHome).UniqueResult<HomeParam>().ParamValue));
                    str46 = string.Format(" '{0}' as accountseller, ", (object) ohlAccounts.Account);
                    str47 = string.Format(" '{0}' as bikbank, ", (object) ohlAccounts.Bank.BIK);
                    str48 = string.Format(" '{0}' as namebank, ", (object) ohlAccounts.Bank.BankName);
                    str49 = string.Format(" '{0}' as korbank, ", (object) ohlAccounts.Bank.KorSch);
                    str50 = " (ls.client_id) as client_id ";
                    str51 = " (select csc.supplier_client from cmpSupplierClient csc, lsClient ls where ls.client_id = csc.client_id and ls.client_id=" + objArray1[0].ToString() + ") as supplier_client ";
                    str52 = "";
                    str53 = "";
                  }
                  else
                  {
                    str46 = "       (select first account from cmpReceipt where company_id=c.company_id and receipt_id=r.receipt_id) as accountseller, ";
                    str47 = "       (select bik from di_bank where idbank=idbankseller) as bikbank, ";
                    str48 = "       (select namebank from di_bank where idbank=idbankseller) as namebank, ";
                    str49 = "       (select kor_sch from di_bank where idbank=idbankseller) as korbank, ";
                    str42 = "       (select first account from cmpReceipt where supplier_id = r.seller_id and receipt_id = r.receipt_id) as accountsellerreal, ";
                    str43 = "       (select bik from di_bank where idbank = idbanksellerreal) as bikbankseller, ";
                    str44 = "       (select namebank from di_bank where idbank = idbanksellerreal) as namebankseller, ";
                    str45 = "       (select kor_sch from di_bank where idbank = idbanksellerreal) as korbankseller, ";
                    str50 = " (ls.client_id) as client_id ";
                    str51 = " (convert(integer,0)) as supplier_client ";
                    str52 = "";
                    str53 = "";
                  }
                  string str54 = "";
                  if (this.chbPeni.Checked && this.rbFullPeniPeriod.Checked)
                    str54 = "+ sum(rent_full)";
                  TableDataSource main = this._main;
                  string[] strArray2 = new string[69]{ "declare @date date  select @date=Period_value from dcPeriod where period_id= ", strPeriodId, " select b.bill_num, b.bill_date, dogovor_num, dogovor_date, ", str50, ", ", str51, ", c.company_id, r.receipt_id, isnull(r.seller_id,0) as seller_id, isnull(r.supplier_id,0) as supplier_id,        borg.nameorg_min as buyer, borg.nameorg as namebuyer, borg.r_sch as rschbuyer, borg.idbaseorg as idbuyer, ", this.Address("borg.idbaseorg"), " as adrbuyer,        (if (r.seller_id is not null and r.seller_id<>0) then ", this.Address("r.seller_id"), " else '' endif) as adrseller,        (if (r.supplier_id is not null and r.supplier_id<>0) then ", this.Address("r.supplier_id"), " else '' endif) as adrsupplier,        borg.inn, borg.kpp, borg.tel as buyertel, db.kor_sch as buyersch, db.namebank as buyerbank, db.bik as buyerbik,        if b.month_id = 0 then ", strPeriod, " else (select period_value from dcPeriod where period_id=b.month_id) endif as period, b.month_id as month_id,        (if '", this.chbPeni.Checked.ToString(), "'='True' then isnull((select sum(balance_out) ", str54, " from lsBalancePeni b where client_id=ls.client_id and        period_id=", strPeriodId, " and (service_id in (select root from ", str2, "printserv where client_id=ls.client_id and supplier_id=b.supplier_id and        receipt_id=r.receipt_id) or service_id=0 or service_id in (select service_id from ", str2, "printserv where client_id=ls.client_id and supplier_id=b.supplier_id and        receipt_id=r.receipt_id))),0) else 0 endif) as peni,", str41, "       (select sum(rent) from lsRent lb,", str2, "printserv prc where lb.client_id=ls.client_id and period_id=", strPeriodId, " and lb.client_id=prc.client_id            and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id and code<>0) as buyerpast,        (select sum(payment) from lsBalance lb,", str2, "printserv prc where lb.client_id=ls.client_id and period_id=", strPeriodId, " and lb.client_id=prc.client_id            and lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id) as payment,        (select sum(rent_vat) from lsRent lb,", str2, "printserv prc where lb.client_id=ls.client_id and period_id=", strPeriodId, " and lb.client_id=prc.client_id and            lb.service_id=prc.service_id and lb.supplier_id=prc.supplier_id and prc.receipt_id=r.receipt_id and code<>0) as buyervatpast,        (if (r.seller_id is not null and r.seller_id<>0) then (select nameorg from base_org where idbaseorg=r.seller_id) else '-----' endif) as nameseller,        (if (r.supplier_id is not null and r.supplier_id<>0) then (select nameorg from base_org where idbaseorg=r.supplier_id) else '-----' endif) as namesupplier,        (if (r.seller_id is not null and r.seller_id<>0) then (select nameorg_min from base_org where idbaseorg=r.seller_id) else '' endif) as nameminseller,        (select inn from base_org where idbaseorg=r.seller_id) as innseller,        (select kpp from base_org where idbaseorg=r.seller_id) as kppseller,        (select tel from base_org where idbaseorg=r.seller_id) as telseller,        (select inn from base_org where idbaseorg=r.supplier_id) as innsupplier,        (select kpp from base_org where idbaseorg=r.supplier_id) as kppsupplier,        (select tel from base_org where idbaseorg=r.supplier_id) as telsupplier,        (select fax from base_org where idbaseorg=r.supplier_id) as faxsupplier, ", str46, "       (select first bank from base_org where idbaseorg = r.seller_id) as idbanksellerreal, ", str44, str43, str45, str42, "       (select first bank_id from cmpReceipt where company_id=c.company_id and receipt_id=r.receipt_id) as idbankseller, ", str48, str47, str49, "       (if (r.consignor_id is not null and r.consignor_id<>0) then (select nameorg from base_org where idbaseorg=r.consignor_id) else '-----' endif) as nameconsignor,        (if (r.consignor_id is not null and r.consignor_id<>0) then ", this.Address("r.consignor_id"), " else '' endif) as adrconsignor,        ", this.NS_lsAddressHome(2, "ls.idflat"), " as adr,        ", this.NS_lsParam_value(2, "ls.client_id ", "cast( '" + KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(Options.Period.PeriodName.Value)).ToString() + "'as date)"), " as plflat,        c.manager_id from lsClient ls ", str39, ", dcCompany c,", str53, " LsArenda la left outer join base_org borg on la.idbaseorg=borg.idbaseorg left outer join di_bank db on db.idbank=borg.bank, lsBill b, cmpReceipt r where ls.company_id=c.company_id and la.Client_id=ls.Client_id and ls.complex_id=110 and b.client_id=ls.Client_id and r.company_id=c.company_id and b.period_id=", strPeriodId, str52, "       and b.bill_type=", this.billType.ToString(), " and ls.client_id=", objArray1[0].ToString(), "       and r.receipt_id in (", null, null };
                  int index23 = 67;
                  receiptId = ((Receipt) this.cbReceipt.SelectedItem).ReceiptId;
                  string str55 = receiptId.ToString();
                  strArray2[index23] = str55;
                  int index24 = 68;
                  string str56 = ") and b.receipt_id=r.receipt_id";
                  strArray2[index24] = str56;
                  string str57 = string.Concat(strArray2);
                  main.SelectCommand = str57;
                  string selectCommand1 = this._main.SelectCommand;
                  IList list4 = (IList) new ArrayList();
                  if (this.chB_Offer_FR.Checked)
                    list4 = this.GetListBalanceCard(false, this.clienttemp);
                  double num2 = 0.0;
                  foreach (object[] objArray2 in (IEnumerable) list4)
                    num2 = Convert.ToDouble(objArray2[0]) - Convert.ToDouble(objArray2[10]);
                  this.session.CreateSQLQuery("select month_dept_id from dba.lsBalance lb where lb.period_id=" + strPeriodId + " and lb.client_id=" + objArray1[0].ToString() ?? "").List();
                  string str58;
                  if (Options.BaseName == "uk1" || Options.BaseName == "ukaka" || this.archiveAllDoc)
                  {
                    str58 = string.Format("declare @date date  select @date=Period_value from dcPeriod where period_id= {2} select servpar.PrintShow sname,qwe.service_id,qwe.client_id,sum(qwe.balout) calc,'' edizm,'' tax,'' volume1,'' vat, servpar.Sorter,-round((sum(qwe.balout)/1.18)-sum(qwe.balout),2) nds,round((sum(qwe.balout)/1.18),2) rentWithoutNds  from (select t.* {3}   from (select serv.service_id, {0} client_id,balout,supplier_id            from DBA.dcService serv               left outer join (select root,b.service_id,sum(b.Balance_out) as balout,supplier_id                                from DBA.lsBalance b, DBA.dcService s                                   where b.service_id=s.service_id and b.period_id={2} and b.client_id={0} group by root,supplier_id,b.service_id                              ) as b on b.root=serv.service_id,                               DBA.cmpServiceParam sp               where serv.root=0 and sp.service_id=serv.service_id and sp.company_id={1} and sp.complex_id=110                  and isnull(balout,0)<>0                 union all          select s.service_id,{0} client_id, sum(b.Balance_out) as balout,supplier_id            from DBA.dcService s               left outer join DBA.lsBalance b on b.service_id=s.service_id and b.period_id={2} and b.client_id={0} where s.service_id=0               group by s.service_id,supplier_id            )t   )qwe inner join dba.dcService serv on serv.service_id=qwe.service_id inner join cmpServiceParam servpar on servpar.Service_id=serv.Service_id where isnull(qwe.balout,0)<>0 and servpar.Company_id={1} and servpar.Complex_id=110 and rec={4}group by  serv.service_name,servpar.PrintShow,qwe.service_id,qwe.client_id,servpar.Sorter order by servpar.Sorter ", (object) objArray1[0].ToString(), (object) this.clienttemp.Company.CompanyId, (object) strPeriodId, (object) str40, (object) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString());
                  }
                  else
                  {
                    string str16 = "select (select client_id from lsClient where client_id=:client_id) as client_id, r.service_id, p.period_value, isnull(t.isvat,0) as isvat,";
                    string str17;
                    string str18;
                    if (this.chbSost.Checked)
                    {
                      str16 += "ds.service_id as sost,";
                      str5 = "and b.service_id=ds.service_id";
                      str6 = " left outer join dcService ds on r.service_id=ds.root";
                      str17 = "and ds.service_id=t.service_id";
                      str18 = "and service_id in (select service_id from " + str2 + "printserv prc where client_id=:client_id and root=r.service_id and service_id=b.service_id)";
                    }
                    else
                    {
                      str5 = "and b.service_id in (select service_id from dcService where root=r.service_id)";
                      str17 = "and s.service_id=t.service_id";
                      str18 = "and service_id in (select service_id from " + str2 + "printserv prc where client_id=:client_id and root=r.service_id)";
                    }
                    if ((this.city == 1 || this.city == 5 || (this.city == 9 || this.city == 36)) && !this._korTamplate)
                      str16 = str16 + "         isnull((if (t.service_id not in (20,71,72,73,74,78,95)                        or (t.service_id=20 and isnull((select group_num from cmpServiceParam where company_id=r.company_id and service_id=21 and complex_id=r.complex_id),0)=0)                       or (t.service_id=95 and isnull((select group_num from cmpServiceParam where company_id=r.company_id and service_id=96 and complex_id=r.complex_id),0)=0                                                            and exists (select 1 from lsService where Client_id=:client_id and Period_id=0 and Service_id=96))                       or (r.service_id=95 and isnull((select group_num from cmpServiceParam where company_id=r.company_id and service_id=96 and complex_id=r.complex_id),0)<>0                                                            and not exists (select 1 from lsService where Client_id=:client_id and Period_id=0 and Service_id=96)               )) then (if doc not in (2,5,6,7,8,12,14,15,17,18,21,25,28,29,31,33,34) then (if r.service_id=3 then                    (select sum(cost) from cmpTariff b where company_id=t.company_id and period_id=0 and tariff_id=s.tariff_id and dbeg=t.dbeg and dend=t.dend                    " + str18 + " and scheme<>3) else                          (if t.scheme<>129 then t.cost else 0 endif)endif) else t.cost endif)                   else 0 endif),0) as tax,";
                    if (this.city == 23 || this._korTamplate)
                      str16 = str16 + "         isnull((if t.service_id not in (20,34) then (if doc not in (2,5,6,7,8,12,14,15,17,18,21,25,28,29,31,33,34) then (if r.service_id=3 then                    (select sum(cost) from cmpTariff b where company_id=t.company_id and period_id=0 and tariff_id=s.tariff_id and dbeg=t.dbeg and dend=t.dend                    " + str18 + " and scheme<>3) else                          (if t.scheme<>129 then t.cost else 0 endif)endif) else t.cost endif)                   else 0 endif),0) as tax,";
                    string str20 = str16 + "         (if gr=0 then (if isnull((select unitmeasuring_name from dcUnitMeasuring where unitmeasuring_id=t.unitmeasuring_id),'')<>''            then (select unitmeasuring_name from dcUnitMeasuring where unitmeasuring_id=t.unitmeasuring_id)            else (select unitmeasuring_name from dcUnitMeasuring where unitmeasuring_id=(select first unitmeasuring_id from cmpTariff ctr where service_id=r.service_id and unitmeasuring_id is not null)) endif)          else if t.unitmeasuring_id is not null then (select unitmeasuring_name from dcUnitMeasuring where unitmeasuring_id=t.unitmeasuring_id)          else (select unitmeasuring_name from dcUnitMeasuring where unitmeasuring_id=(select first unitmeasuring_id from cmpTariff ctr where service_id=gr )) endif endif) as edizm,          isnull((if (" + this.city.ToString() + "<>1 or t.service_id not in (71,72,73,74,78)) then isnull((select first n.norm_value from cmpNorm n where s.norm_id=n.norm_id and            n.company_id=(select param_value from cmpParam where company_id=r.company_id and period_id=0 and param_id=204 and dbeg<=" + strPeriod + " and dend>=" + strPeriod + ")         and n.dbeg<=" + strPeriod + " and n.dend>=" + strPeriod + " and n.period_id=0),0) else 0 endif),0) as norm,          (select group_num from cmpServiceParam where company_id=r.company_id and complex_id=110 and service_id=r.service_id) as gr,          isnull((select param_value from lsParam where period_id=0 and client_id=:client_id and dbeg<=" + strPeriod + " and dend>=" + strPeriod + " and param_id=104),0) as doc,         isnull((if (select first basetariff_id from lsService ls,cmpTariff ct where ls.client_id=:client_id and ls.service_id=r.service_id and ls.service_id=ct.service_id               and ls.tariff_id=ct.tariff_id and ls.dbeg<=" + strPeriod + " and ls.dend>=" + strPeriod + " and ls.period_id=0 and ct.period_id=0 and ct.company_id=r.company_id and ct.dbeg<=" + strPeriod + " and ct.dend>=" + strPeriod + ")=2 then " + "         (if (select group_num from cmpServiceParam where company_id=:company_id and complex_id=110 and service_id=r.service_id)=0 then            isnull((select max(volume) from lsRent b where period_id=" + strPeriodId + " and client_id=:client_id " + str18 + "),0) else 0 endif)" + "                     else (if :month_id =0 then                       isnull((select max(volume) from lsRent b where period_id=" + strPeriodId + " and b.client_id=:client_id                       " + str18 + " and code=0),0)                       else isnull((select max(volume) from lsRent b where period_id=" + strPeriodId + " and b.client_id=:client_id                       " + str18 + " and month_id=:month_id),0) endif) endif),0) as volume,         (if :month_id = 0 then                  isnull((select sum(isnull(rent,0)) from lsRent b," + str2 + "printserv prc where b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id                   " + str5 + str38 + "),0) else                   isnull((select sum(isnull(rent,0)) from lsRent b," + str2 + "printserv prc where b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id                   " + str5 + " and month_id=:month_id " + str38 + "),0) endif) as calc,                   isnull((select sum(isnull(b.rent,0)) - sum(isnull(b.rent_past,0)) from dba.lsBalance b, " + str2 + "printserv prc where b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id                   " + str5 + " ),0) as rentC,                   isnull((select sum(isnull(b.rent_past,0)) from dba.lsBalance b, " + str2 + "printserv prc where b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id                   " + str5 + " ),0) as rentpastC,  (if :month_id = 0 then                  isnull((select sum(isnull(rent,0)) from lsRent b," + str2 + "printserv prc where b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id                   " + str5 + str38 + "),0) else                   isnull((select sum(isnull(rent,0)) from lsRent b," + str2 + "printserv prc where b.code=0 and b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id                   " + str5 + " and month_id=:month_id " + str38 + "),0) endif) as calc0,  (if :month_id = 0 then                  isnull((select sum(isnull(rent,0)) from lsRent b," + str2 + "printserv prc where b.code<>0 and b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id                   " + str5 + str38 + "),0) else                   isnull((select sum(isnull(rent,0)) from lsRent b," + str2 + "printserv prc where b.code<>0 and b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id                   " + str5 + " and month_id=:month_id " + str38 + "),0) endif) as calcrecal,          (if :month_id = 0 then                  isnull((select sum(isnull(rent_vat,0)) from lsRent b," + str2 + "printserv prc where b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id                   " + str5 + str38 + "),0) else           isnull((select sum(isnull(rent_vat,0)) from lsRent b," + str2 + "printserv prc where b.client_id=prc.client_id and b.service_id=prc.service_id and b.supplier_id=prc.supplier_id and period_id=" + strPeriodId + " and b.client_id=:client_id                   " + str5 + " and month_id=:month_id" + str38 + "),0) endif) as vat from dcPeriod p,cmpServiceParam r" + str6 + " left outer join lsService s on r.service_id=s.service_id and s.client_id=:client_id          and s.dbeg<=isnull((select first dbeg from lsService ss where r.service_id=ss.service_id and ss.client_id=:client_id and ss.complex_id=100 and ss.period_id=" + str13 + "                and ((dbeg<=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif) and dend>=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif)) or (dbeg<=(if :month_id = 0 then months(" + strPeriod + ",1)-1 else (select period_value from dcPeriod where period_id=(:month_id +1))-1 endif) and dend>=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif)))),(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif))          and s.dend>=isnull((select first dbeg from lsService ss where r.service_id=ss.service_id and ss.client_id=:client_id and ss.complex_id=100 and ss.period_id=" + str13 + "                and ((dbeg<=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif) and dend>=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif)) or (dbeg<=(if :month_id = 0 then months(" + strPeriod + ",1)-1 else (select period_value from dcPeriod where period_id=(:month_id +1))-1 endif) and dend>=(if :month_id = 0 then months(" + strPeriod + ",1)-1 else (select period_value from dcPeriod where period_id=(:month_id +1))-1 endif)))),(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif))          and s.complex_id=100 and s.period_id=" + str13 + "          left outer join cmpTariff t on s.tariff_id=t.tariff_id " + str17 + " and t.company_id=(select param_value from cmpParam where company_id=r.company_id and period_id=0 and param_id=201 and dbeg<=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif) and dend>=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif))          and t.period_id=(if :month_id=0 or not exists(select * from cmpTariff where period_id=" + strPeriodId + " and company_id=t.company_id and complex_id=t.complex_id and service_id=r.service_id and tariff_id=t.tariff_id and dbeg<(select period_value from dcPeriod where period_id=(:month_id +1)) and dend>=(select period_value from dcPeriod where period_id=:month_id)) then 0 else " + strPeriodId + " endif) and s.complex_id=t.complex_id           and t.dbeg<=isnull((select first dbeg from cmpTariff tt where s.tariff_id=tt.tariff_id and s.service_id=tt.service_id and tt.company_id=t.company_id and tt.complex_id=100 and tt.period_id=0                 and ((dbeg<=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif) and dend>=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif)) or (dbeg<=(if :month_id = 0 then months(" + strPeriod + ",1)-1 else (select period_value from dcPeriod where period_id=(:month_id +1))-1 endif) and dend>=(if :month_id = 0 then months(" + strPeriod + ",1)-1 else (select period_value from dcPeriod where period_id=(:month_id +1))-1 endif)))),(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif))          and t.dend>=isnull((select first dbeg from cmpTariff tt where s.tariff_id=tt.tariff_id and s.service_id=tt.service_id and tt.company_id=t.company_id and tt.complex_id=100 and tt.period_id=0                 and ((dbeg<=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif) and dend>=(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif)) or (dbeg<=(if :month_id = 0 then months(" + strPeriod + ",1)-1 else (select period_value from dcPeriod where period_id=(:month_id +1))-1 endif) and dend>=(if :month_id = 0 then months(" + strPeriod + ",1)-1 else (select period_value from dcPeriod where period_id=(:month_id +1))-1 endif)))),(if :month_id = 0 then " + strPeriod + " else (select period_value from dcPeriod where period_id=:month_id) endif))where r.company_id=:company_id and r.complex_id=110 and p.period_id=" + strPeriodId + " ";
                    string str21 = "having calc<>0 or vat<>0 ";
                    if (this.checkBoxMinusOff.Checked)
                      str21 = "having (calc <>0 or vat<>0) and calc>0 ";
                    if (this.chbSost.Checked)
                      str58 = "select my.client_id,sp.service_id,(select service_name from dcService where service_id=my.sost) as sname,sp.sorter,sp.group_num,sp.receipt_id,sum(my.tax) as tax,my.edizm,my.isvat,sum(my.norm) as quota,           sum(my.volume) as volume,sum(my.calc) as calc,(if isnull(tax,0)<>0 then calc/tax else volume endif) as volume1,sum(my.vat) as vat, sum(my.calc0) as calc0, sum(my.calcrecal) as calcrecal, sum(my.rentC) as rent, sum(my.rentpastC) as rentpast from (" + str20 + ") as my,cmpServiceParam sp where sp.company_id=:company_id and sp.complex_id=110 and my.service_id=sp.service_id  group by my.client_id,sp.service_id,my.sost,sp.printshow,sp.group_num,edizm,sp.sorter,sp.receipt_id,my.isvat " + str21 + "order by client_id,sp.sorter,my.sost ";
                    else
                      str58 = "select my.client_id,sp.service_id, sp.printshow as sname,sp.sorter,sp.group_num,sp.receipt_id, sum(my.tax) tax,my.edizm,my.isvat,sum(my.norm) as quota, sum(my.volume) as volume,sum(my.calc) as calc,          (if isnull(tax,0)<>0 then calc/tax else volume endif) as volume1,sum(my.vat) as vat, sum(my.calc0) as calc0, sum(my.calcrecal) as calcrecal, sum(my.rentC) as rent, sum(my.rentpastC) as rentpast  from (" + str20 + ") as my,cmpServiceParam sp where sp.company_id=:company_id and sp.complex_id=110 and (my.service_id=sp.service_id or sp.service_id=my.gr)         and (group_num=0 or sp.calcalone=1)  group by my.client_id,sp.service_id,sp.printshow,sp.group_num,edizm,sp.sorter,sp.receipt_id,my.isvat " + str21 + "order by client_id,sp.sorter ";
                  }
                  this._tarif = this.report.GetDataSource("Tarif") as TableDataSource;
                  this._tarif.SelectCommand = str58;
                  IList list5 = this.session.CreateSQLQuery("select * from lsClient ls, lsBill b where ls.complex_id=110 and b.client_id=ls.Client_id and b.period_id=" + strPeriodId + " and b.bill_type=" + this.billType.ToString() + " and receipt_id in (" + ((Receipt) this.cbReceipt.SelectedItem).ReceiptId.ToString() + ") and ls.client_id=" + objArray1[0].ToString()).List();
                  string selectCommand2 = this._tarif.SelectCommand;
                  if (this.onClientCard)
                  {
                    this.baseOrg = this.session.Get<BaseOrg>((object) this.baseOrg.BaseOrgId);
                  }
                  else
                  {
                    this.baseOrg = new BaseOrg();
                    this.baseOrg.AdressPriority = Convert.ToInt32(this.session.CreateSQLQuery("select borg.AdressPriority from LsArenda la            left outer join base_org borg on la.idbaseorg=borg.idbaseorg where la.client_id=" + objArray1[0].ToString()).UniqueResult());
                  }
                  Address address = (Address) this.session.CreateQuery(string.Format("select new Address(c.ClientId,d.NameStr,h.NHome,h.HomeKorp,f.NFlat,c.SurFlat) from Home h, Str d, Flat f,LsClient c where h.Str=d and c.Home=h and c.Flat=f and c.ClientId={0} ", (object) objArray1[0].ToString())).List()[0];
                  string adrString;
                  switch (this.city)
                  {
                    case 1:
                      adrString = string.Format("{0} д.{1} {2} кв.{3} {4}", (object) address.Str, (object) address.Number, (object) (address.Korp == "" || address.Korp == "0" || address.Korp == null ? "" : "к." + address.Korp), (object) address.Flat, (object) (address.SurFlat == "" || address.SurFlat == "0" || address.SurFlat == null ? "" : "комн." + address.SurFlat));
                      break;
                    case 23:
                      adrString = string.Format("{0} д.{1} {2}", (object) address.Str, (object) address.Number, address.Korp == "" || address.Korp == "0" || address.Korp == null ? (object) "" : (object) ("к." + address.Korp));
                      break;
                    default:
                      if (this._korTamplate)
                      {
                        adrString = string.Format("{0} д.{1} {2}", (object) address.Str, (object) address.Number, address.Korp == "" || address.Korp == "0" || address.Korp == null ? (object) "" : (object) ("к." + address.Korp));
                        break;
                      }
                      adrString = string.Format("{0} д.{1} {2} кв.{3} {4}", (object) address.Str, (object) address.Number, (object) (address.Korp == "" || address.Korp == "0" || address.Korp == null ? "" : "к." + address.Korp), (object) address.Flat, (object) (address.SurFlat == "" || address.SurFlat == "0" || address.SurFlat == null ? "" : "комн." + address.SurFlat));
                      break;
                  }
                  string str59 = this.AddressPost(objArray1[0].ToString());
                  string str60 = this.AddressLegal(objArray1[0].ToString());
                  string str61 = "";
                  try
                  {
                    this.report.SetParameterValue("dolg", (object) "за коммунальные услуги и содержание нежилых помещений:");
                    string str16 = "";
                    DateTime dateTime2 = new DateTime(2008, 1, 1);
                    string str17 = "";
                    if (list5.Count > 0)
                    {
                      if (this.RoGbVisible.Checked)
                        this.report.SetParameterValue("ROandGB", (object) 1);
                      else
                        this.report.SetParameterValue("ROandGB", (object) 0);
                      if ((Receipt) this.cbReceipt.SelectedItem != null)
                      {
                        switch (((Receipt) this.cbReceipt.SelectedItem).ReceiptId)
                        {
                          case 1:
                            this.report.SetParameterValue("dolg", (object) "за коммунальные услуги и содержание нежилых помещений:");
                            break;
                          case 55:
                            this.report.SetParameterValue("dolg", (object) "за капитальный ремонт:");
                            break;
                        }
                      }
                      Report report1 = this.report;
                      string complexName1 = "Lastopl";
                      DateTime dateTime3 = this.dtpSrok.Value;
                      string shortDateString1 = dateTime3.ToShortDateString();
                      report1.SetParameterValue(complexName1, (object) shortDateString1);
                      Report report2 = this.report;
                      string complexName2 = "dateDolg";
                      dateTime3 = KvrplHelper.FirstDay(this.mpCurrentPeriod.Value);
                      string shortDateString2 = dateTime3.ToShortDateString();
                      report2.SetParameterValue(complexName2, (object) shortDateString2);
                      this.report.SetParameterValue("ruk", (object) this.rukName);
                      this.report.SetParameterValue("bux", (object) this.buhName);
                      this.report.SetParameterValue("adr", (object) adrString);
                      if (this.chbStampDisplay.Checked)
                        this.report.SetParameterValue("Stamp", (object) true);
                      else
                        this.report.SetParameterValue("Stamp", (object) false);
                      LsArenda lsArenda = this.session.CreateCriteria(typeof (LsArenda)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("LsClient.ClientId", (object) (int) objArray1[0])).UniqueResult<LsArenda>();
                      try
                      {
                        str61 = lsArenda.LsClient.Flat.Home.Address;
                        str16 = lsArenda.DogovorNum;
                        dateTime2 = lsArenda.DogovorDate;
                        str17 = lsArenda.NameOrg;
                      }
                      catch (Exception ex)
                      {
                        Console.WriteLine(ex.Message);
                      }
                      this.report.SetParameterValue("DocType", (object) lsArenda.TypeArendaDocument.TypeDocument_name);
                      if (this.cbTypeAddress.SelectedIndex == 2)
                        this.report.SetParameterValue("adrpost", (object) str59);
                      if (this.cbTypeAddress.SelectedIndex == 3)
                        this.report.SetParameterValue("adrpost", (object) adrString);
                      if (this.cbTypeAddress.SelectedIndex == 4)
                        this.report.SetParameterValue("adrpost", (object) str60);
                      if (this.cbTypeAddress.SelectedIndex == 0)
                        this.report.SetParameterValue("adrpost", (object) "");
                      if (this.cbTypeAddress.SelectedIndex == 1)
                      {
                        if (this.baseOrg.AdressPriority == 0)
                          this.report.SetParameterValue("adrpost", (object) str60);
                        if (this.baseOrg.AdressPriority == 1)
                          this.report.SetParameterValue("adrpost", (object) str59);
                      }
                      if ((int) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId == 50)
                      {
                        SupplierClient supplierClient = this.session.CreateQuery("from SupplierClient sc where sc.LsClient.ClientId=:cl and sc.Supplier.BaseOrgId=-39999859").SetParameter<int>("cl", (int) objArray1[0]).UniqueResult<SupplierClient>();
                        if (supplierClient == null)
                        {
                          int num3 = (int) MessageBox.Show("Невозможно распечатать! Нет лицевого капремонта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                          return;
                        }
                        this.report.SetParameterValue("OhlLSPer", (object) (supplierClient.SupplierClientId.ToString() + "/" + strPeriodId));
                        this.report.SetParameterValue("OhlLS", (object) supplierClient.SupplierClientId);
                        this.report.SetParameterValue("Ohl", (object) true);
                      }
                      else
                        this.report.SetParameterValue("Ohl", (object) false);
                      if (!this.rbAktDontInvoice.Checked && (index1 == 1 || index1 == 0))
                        this._reportList.Add(new ReportArenda()
                        {
                          LsArenda = Convert.ToInt32(objArray1[0]),
                          BillType = index1,
                          Report = this.report,
                          PeriodId = Options.Period.PeriodId
                        });
                    }
                    if (index1 == 2)
                      this.PrintActs(print, false, settings, adrString, this.clienttemp, Convert.ToInt32(objArray1[0]), true);
                    if (index1 == 3 && list3.Count > 0 && (this.rbInvoice.Checked || this.rbAllDoc.Checked))
                      this.ShowCorrect(print, strPeriod, strPeriodId, objArray1[0].ToString(), settings, Convert.ToInt32(objArray1[0]), true);
                    if (!this.chbIsBlank.Checked)
                      ;
                  }
                  catch (Exception ex)
                  {
                    int num3 = (int) MessageBox.Show(ex.Message);
                  }
                }
                PrintSettings printSettings = (PrintSettings) null;
                foreach (int num2 in this._licClientArenda)
                {
                  int lsAr = num2;
                  foreach (ReportArenda reportArenda in (IEnumerable<ReportArenda>) this._reportList.Where<ReportArenda>((Func<ReportArenda, bool>) (x => x.LsArenda == lsAr)).OrderBy<ReportArenda, int>((Func<ReportArenda, int>) (x => x.BillType)).OrderBy<ReportArenda, int>((Func<ReportArenda, int>) (x => x.PeriodId)))
                  {
                    try
                    {
                      if (printSettings == null)
                        printSettings = this.report.PrintSettings;
                      this.SaveOrPrintOrForEmail(reportArenda.Report, reportArenda.LsArenda, reportArenda.PeriodId, reportArenda.BillType, print, printSettings, this.onlySave);
                    }
                    catch (Exception ex)
                    {
                      KvrplHelper.WriteLog(ex, this.client);
                    }
                  }
                }
                this._reportList.Clear();
                this._licClientArenda.Clear();
              }
            }
          }
          int num4 = (int) MessageBox.Show("Операция завершена.", "Внимание!");
        }
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, this.client);
      }
    }

    private void SaveForEmailPacket(ReportArenda temp)
    {
      try
      {
        if (!this.chbEmail.Checked)
          return;
        PDFExport pdfExport = new PDFExport();
        string fileName = this._pathSaveEmail;
        switch (temp.BillType)
        {
          case 0:
            fileName = fileName + "\\" + (object) temp.LsArenda + "_" + (object) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId + "_" + (object) temp.PeriodId + "_Счет.pdf";
            break;
          case 1:
            fileName = fileName + "\\" + (object) temp.LsArenda + "_" + (object) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId + "_" + (object) temp.PeriodId + "_СчетФактура.pdf";
            break;
          case 2:
            fileName = fileName + "\\" + (object) temp.LsArenda + "_" + (object) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId + "_" + (object) temp.PeriodId + "_Акт.pdf";
            break;
          case 3:
            fileName = fileName + "\\" + (object) temp.LsArenda + "_" + (object) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId + "_" + (object) temp.PeriodId + "_КорректСчетФактура.pdf";
            break;
        }
        temp.Report.Export((ExportBase) pdfExport, fileName);
      }
      catch (Exception ex)
      {
      }
    }

    private void SaveOrPrintOrForEmail(Report report, int ls, int period, int billType, bool print, PrintSettings printSettings, bool onlySave = false)
    {
      try
      {
        IList list = this.session.CreateSQLQuery("select idservice from hmreceipt where client_id=:cid and receipt_id=:rid and DEnd>NOW() and idservice in(6,7,8,9)").SetParameter<int>("cid", ls).SetParameter<short>("rid", ((Receipt) this.cbReceipt.SelectedItem).ReceiptId).List();
        bool flag1 = true;
        bool flag2 = false;
        foreach (object obj in (IEnumerable) list)
        {
          if (Convert.ToInt32(obj) == 7 || Convert.ToInt32(obj) == 9)
            flag1 = false;
          flag2 = true;
        }
        report.Prepare();
        try
        {
          if (this.chbEmail.Checked & flag2)
          {
            PDFExport pdfExport = new PDFExport();
            string fileName = this._pathSaveEmail;
            switch (billType)
            {
              case 0:
                fileName = fileName + "\\" + (object) ls + "_" + (object) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId + "_" + (object) period + "_Счет.pdf";
                break;
              case 1:
                fileName = fileName + "\\" + (object) ls + "_" + (object) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId + "_" + (object) period + "_СчетФактура.pdf";
                break;
              case 2:
                fileName = fileName + "\\" + (object) ls + "_" + (object) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId + "_" + (object) period + "_Акт.pdf";
                break;
              case 3:
                fileName = fileName + "\\" + (object) ls + "_" + (object) ((Receipt) this.cbReceipt.SelectedItem).ReceiptId + "_" + (object) period + "_КорректСчетФактура.pdf";
                break;
            }
            report.Export((ExportBase) pdfExport, fileName);
          }
          if (this.pathSave)
          {
            PDFExport pdfExport = new PDFExport();
            string fileName = this.pathSaveReceipt;
            switch (billType)
            {
              case 0:
                fileName = fileName + "\\" + (object) ls + "_" + Options.Period.PeriodName.Value.ToString("dd.MM.yyyy") + "_Счет.pdf";
                break;
              case 1:
                fileName = fileName + "\\" + (object) ls + "_" + Options.Period.PeriodName.Value.ToString("dd.MM.yyyy") + "_СчетФактура.pdf";
                break;
              case 2:
                fileName = fileName + "\\" + (object) ls + "_" + Options.Period.PeriodName.Value.ToString("dd.MM.yyyy") + "_Акт.pdf";
                break;
              case 3:
                fileName = fileName + "\\" + (object) ls + "_" + Options.Period.PeriodName.Value.ToString("dd.MM.yyyy") + "_КорректСчетФактура.pdf";
                break;
            }
            report.Export((ExportBase) pdfExport, fileName);
          }
        }
        catch (Exception ex)
        {
        }
        if (onlySave)
          return;
        if (print & flag1)
        {
          if (this._settings == null)
          {
            this._settings = report.PrintSettings;
            this._settings.ShowDialog = true;
            report.PrintSettings.Assign(this._settings);
          }
          else
            this._settings.ShowDialog = false;
          report.Print();
        }
        if (!print)
          report.Show();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, this.client);
      }
    }

    private void butSaveReport_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      int num = (int) folderBrowserDialog.ShowDialog();
      this.pathSaveReceipt = folderBrowserDialog.SelectedPath;
      this.pathSave = true;
      this.onlySave = true;
      if (this.rbAllDoc.Checked || this.rbAllArchive.Checked)
        this.PrintAllDocument(false);
      else
        this.PrintOrShowReports(false);
    }

    private void rbAllArchive_Click(object sender, EventArgs e)
    {
      if (this.rbAllArchive.Checked)
      {
        this.archiveAllDoc = true;
        this.lblSrok.Enabled = true;
        this.dtpSrok.Enabled = true;
        this.chbIsAkt.Enabled = true;
        this.nudCountAkt.Enabled = true;
        this.chbCorrect.Enabled = false;
        this.chbCorrect.Checked = false;
        this.chbIsBlank.Enabled = false;
        this.chbIsBlank.Checked = false;
        this.chbIsAkt.Checked = true;
        this.butSaveReport.Enabled = true;
        this.gbMountBreak.Visible = false;
      }
      this.UpdateData();
    }

    private void chbEmail_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.chbEmail.Checked || !(this._pathSaveEmail == ""))
        return;
      int num = (int) MessageBox.Show("Не указан путь сохранения счетов, для отправки на email!", "Внимание");
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmBills));
      this.report = new Report();
      this.tTTemplate = new ToolTip(this.components);
      this.RoGbVisible = new CheckBox();
      this.chB_Offer_FR = new CheckBox();
      this.cbShablon = new ComboBox();
      this.cbAkt = new ComboBox();
      this.checkBoxMinusOff = new CheckBox();
      this.btnExit = new Button();
      this.pnBtn = new Panel();
      this.butSaveReport = new Button();
      this.button1 = new Button();
      this.btnView = new Button();
      this.btnPrint = new Button();
      this.gbBill = new GroupBox();
      this.rbAllArchive = new RadioButton();
      this.rbAllDoc = new RadioButton();
      this.rbAktDontInvoice = new RadioButton();
      this.rbInvoice = new RadioButton();
      this.rbBill = new RadioButton();
      this.pnPeriod = new Panel();
      this.mpCurrentPeriod = new MonthPicker();
      this.lblPeriod = new Label();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
      this.lblNum = new Label();
      this.lblDate = new Label();
      this.txbNum = new TextBox();
      this.dtpDate = new DateTimePicker();
      this.lblKol = new Label();
      this.nudCountBill = new NumericUpDown();
      this.chbIsAkt = new CheckBox();
      this.nudCountAkt = new NumericUpDown();
      this.chbIsBlank = new CheckBox();
      this.chbSost = new CheckBox();
      this.lblSrok = new Label();
      this.dtpSrok = new DateTimePicker();
      this.chbMec = new CheckBox();
      this.chbCorrect = new CheckBox();
      this.nudNumCorrect = new NumericUpDown();
      this.Prepare = new DataGridViewTextBoxColumn();
      this.Num = new DataGridViewTextBoxColumn();
      this.DocNum = new DataGridViewTextBoxColumn();
      this.Id = new DataGridViewTextBoxColumn();
      this.dgvList = new DataGridView();
      this.txbNumCor = new TextBox();
      this.cbReceipt = new ComboBox();
      this.lblReceipt = new Label();
      this.textBox1 = new TextBox();
      this.lblTypeAddress = new Label();
      this.cbTypeAddress = new ComboBox();
      this.labShablon = new Label();
      this.labAkt = new Label();
      this.cbSaveReport = new CheckBox();
      this.rbFullPeniPeriod = new RadioButton();
      this.gbPeni = new GroupBox();
      this.rbPeniPeriod = new RadioButton();
      this.chbPeni = new CheckBox();
      this.mpToPeriod = new MonthPicker();
      this.gbMountBreak = new GroupBox();
      this.mpFromPeriod = new MonthPicker();
      this.label1 = new Label();
      this.label2 = new Label();
      this.cbLetterCover = new CheckBox();
      this.chbOnlyNew = new CheckBox();
      this.chbEmail = new CheckBox();
      this.chbStampDisplay = new CheckBox();
      this.chbCorrectMonth = new CheckBox();
      this.report.BeginInit();
      this.pnBtn.SuspendLayout();
      this.gbBill.SuspendLayout();
      this.pnPeriod.SuspendLayout();
      this.nudCountBill.BeginInit();
      this.nudCountAkt.BeginInit();
      this.nudNumCorrect.BeginInit();
      ((ISupportInitialize) this.dgvList).BeginInit();
      this.gbPeni.SuspendLayout();
      this.gbMountBreak.SuspendLayout();
      this.SuspendLayout();
      this.report.ReportResourceString = componentResourceManager.GetString("report.ReportResourceString");
      this.RoGbVisible.AutoSize = true;
      this.RoGbVisible.Location = new Point(6, 425);
      this.RoGbVisible.Name = "RoGbVisible";
      this.RoGbVisible.Size = new Size(194, 20);
      this.RoGbVisible.TabIndex = 28;
      this.RoGbVisible.Text = "Печать рук. орг.  и гл. бух.";
      this.tTTemplate.SetToolTip((Control) this.RoGbVisible, "Исключительно для Счета!!!");
      this.RoGbVisible.UseVisualStyleBackColor = true;
      this.chB_Offer_FR.AutoSize = true;
      this.chB_Offer_FR.Location = new Point(6, 477);
      this.chB_Offer_FR.Name = "chB_Offer_FR";
      this.chB_Offer_FR.Size = new Size(419, 20);
      this.chB_Offer_FR.TabIndex = 29;
      this.chB_Offer_FR.Text = "Сформировать сообщение в зависимости от периода долга";
      this.tTTemplate.SetToolTip((Control) this.chB_Offer_FR, "Внимание!!! Только для \"Управдом Фрунзенского района\"");
      this.chB_Offer_FR.UseVisualStyleBackColor = true;
      this.chB_Offer_FR.Visible = false;
      this.cbShablon.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cbShablon.FormattingEnabled = true;
      this.cbShablon.Location = new Point(200, 504);
      this.cbShablon.Name = "cbShablon";
      this.cbShablon.Size = new Size(720, 24);
      this.cbShablon.TabIndex = 30;
      this.tTTemplate.SetToolTip((Control) this.cbShablon, "Внимание при выборе!!! \r\nИспользуйте если только знаете какой шаблон вам нужен.");
      this.cbShablon.SelectedValueChanged += new EventHandler(this.cbShablon_SelectedValueChanged);
      this.cbAkt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cbAkt.FormattingEnabled = true;
      this.cbAkt.Location = new Point(200, 540);
      this.cbAkt.Name = "cbAkt";
      this.cbAkt.Size = new Size(720, 24);
      this.cbAkt.TabIndex = 33;
      this.tTTemplate.SetToolTip((Control) this.cbAkt, "Выберите шаблон для Акта");
      this.checkBoxMinusOff.AutoSize = true;
      this.checkBoxMinusOff.Location = new Point(6, 570);
      this.checkBoxMinusOff.Name = "checkBoxMinusOff";
      this.checkBoxMinusOff.Size = new Size(230, 20);
      this.checkBoxMinusOff.TabIndex = 34;
      this.checkBoxMinusOff.Text = "Не печатать минусовые суммы";
      this.tTTemplate.SetToolTip((Control) this.checkBoxMinusOff, "Только для Коряжма");
      this.checkBoxMinusOff.UseVisualStyleBackColor = true;
      this.checkBoxMinusOff.Visible = false;
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(851, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(79, 30);
      this.btnExit.TabIndex = 2;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.pnBtn.Controls.Add((Control) this.butSaveReport);
      this.pnBtn.Controls.Add((Control) this.button1);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Controls.Add((Control) this.btnView);
      this.pnBtn.Controls.Add((Control) this.btnPrint);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 826);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(942, 40);
      this.pnBtn.TabIndex = 0;
      this.butSaveReport.Enabled = false;
      this.butSaveReport.Location = new Point(250, 5);
      this.butSaveReport.Name = "butSaveReport";
      this.butSaveReport.Size = new Size(99, 30);
      this.butSaveReport.TabIndex = 4;
      this.butSaveReport.Text = "Сохранить";
      this.butSaveReport.UseVisualStyleBackColor = true;
      this.butSaveReport.Click += new EventHandler(this.butSaveReport_Click);
      this.button1.Location = new Point(484, 5);
      this.button1.Name = "button1";
      this.button1.Size = new Size(25, 23);
      this.button1.TabIndex = 3;
      this.button1.Text = "button1";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Visible = false;
      this.btnView.Location = new Point(125, 5);
      this.btnView.Name = "btnView";
      this.btnView.Size = new Size(89, 30);
      this.btnView.TabIndex = 1;
      this.btnView.Tag = (object) "2";
      this.btnView.Text = "На экран";
      this.btnView.UseVisualStyleBackColor = true;
      this.btnView.Click += new EventHandler(this.btnView_Click);
      this.btnPrint.Location = new Point(12, 5);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(75, 30);
      this.btnPrint.TabIndex = 0;
      this.btnPrint.Tag = (object) "1";
      this.btnPrint.Text = "Печать";
      this.btnPrint.UseVisualStyleBackColor = true;
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.gbBill.Controls.Add((Control) this.rbAllArchive);
      this.gbBill.Controls.Add((Control) this.rbAllDoc);
      this.gbBill.Controls.Add((Control) this.rbAktDontInvoice);
      this.gbBill.Controls.Add((Control) this.rbInvoice);
      this.gbBill.Controls.Add((Control) this.rbBill);
      this.gbBill.Dock = DockStyle.Top;
      this.gbBill.Location = new Point(0, 34);
      this.gbBill.Name = "gbBill";
      this.gbBill.Size = new Size(942, 49);
      this.gbBill.TabIndex = 2;
      this.gbBill.TabStop = false;
      this.gbBill.Text = "Вид счета";
      this.rbAllArchive.AutoSize = true;
      this.rbAllArchive.Location = new Point(622, 21);
      this.rbAllArchive.Name = "rbAllArchive";
      this.rbAllArchive.Size = new Size(302, 20);
      this.rbAllArchive.TabIndex = 4;
      this.rbAllArchive.TabStop = true;
      this.rbAllArchive.Text = "Пакет документов архивных арендаторов";
      this.rbAllArchive.UseVisualStyleBackColor = true;
      this.rbAllArchive.Click += new EventHandler(this.rbAllArchive_Click);
      this.rbAllDoc.AutoSize = true;
      this.rbAllDoc.Location = new Point(438, 21);
      this.rbAllDoc.Name = "rbAllDoc";
      this.rbAllDoc.Size = new Size(148, 20);
      this.rbAllDoc.TabIndex = 3;
      this.rbAllDoc.TabStop = true;
      this.rbAllDoc.Text = "Пакет документов";
      this.rbAllDoc.UseVisualStyleBackColor = true;
      this.rbAllDoc.Click += new EventHandler(this.rbAllDoc_Click);
      this.rbAktDontInvoice.AutoSize = true;
      this.rbAktDontInvoice.Location = new Point(241, 21);
      this.rbAktDontInvoice.Name = "rbAktDontInvoice";
      this.rbAktDontInvoice.Size = new Size(172, 20);
      this.rbAktDontInvoice.TabIndex = 2;
      this.rbAktDontInvoice.TabStop = true;
      this.rbAktDontInvoice.Text = "Акт без Счет-фактура";
      this.rbAktDontInvoice.UseVisualStyleBackColor = true;
      this.rbAktDontInvoice.CheckedChanged += new EventHandler(this.rbAktDontInvoice_CheckedChanged);
      this.rbInvoice.AutoSize = true;
      this.rbInvoice.Location = new Point(96, 21);
      this.rbInvoice.Name = "rbInvoice";
      this.rbInvoice.Size = new Size(119, 20);
      this.rbInvoice.TabIndex = 1;
      this.rbInvoice.Text = "Счет-фактура";
      this.rbInvoice.UseVisualStyleBackColor = true;
      this.rbInvoice.Click += new EventHandler(this.rbInvoice_Click);
      this.rbBill.AutoSize = true;
      this.rbBill.Checked = true;
      this.rbBill.Location = new Point(6, 21);
      this.rbBill.Name = "rbBill";
      this.rbBill.Size = new Size(58, 20);
      this.rbBill.TabIndex = 0;
      this.rbBill.TabStop = true;
      this.rbBill.Text = "Счет";
      this.rbBill.UseVisualStyleBackColor = true;
      this.rbBill.Click += new EventHandler(this.rbBill_Click);
      this.pnPeriod.Controls.Add((Control) this.mpCurrentPeriod);
      this.pnPeriod.Controls.Add((Control) this.lblPeriod);
      this.pnPeriod.Dock = DockStyle.Top;
      this.pnPeriod.Location = new Point(0, 0);
      this.pnPeriod.Name = "pnPeriod";
      this.pnPeriod.Size = new Size(942, 34);
      this.pnPeriod.TabIndex = 1;
      this.mpCurrentPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.mpCurrentPeriod.CustomFormat = "MMMM yyyy";
      this.mpCurrentPeriod.Format = DateTimePickerFormat.Custom;
      this.mpCurrentPeriod.Location = new Point(786, 6);
      this.mpCurrentPeriod.Name = "mpCurrentPeriod";
      this.mpCurrentPeriod.OldMonth = 7;
      this.mpCurrentPeriod.ShowUpDown = true;
      this.mpCurrentPeriod.Size = new Size(140, 22);
      this.mpCurrentPeriod.TabIndex = 1;
      this.mpCurrentPeriod.ValueChanged += new EventHandler(this.mpCurrentPeriod_ValueChanged);
      this.lblPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblPeriod.AutoSize = true;
      this.lblPeriod.Location = new Point(685, 11);
      this.lblPeriod.Name = "lblPeriod";
      this.lblPeriod.Size = new Size(99, 16);
      this.lblPeriod.TabIndex = 0;
      this.lblPeriod.Text = "Месяц печати";
      this.dataGridViewTextBoxColumn1.HeaderText = "Лицевой счет";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.Visible = false;
      this.dataGridViewTextBoxColumn2.HeaderText = "№ Счета";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn3.HeaderText = "Готовность";
      this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
      this.dataGridViewTextBoxColumn4.HeaderText = "Готовность";
      this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
      this.lblNum.AutoSize = true;
      this.lblNum.Location = new Point(3, 118);
      this.lblNum.Name = "lblNum";
      this.lblNum.Size = new Size(51, 16);
      this.lblNum.TabIndex = 3;
      this.lblNum.Text = "Номер";
      this.lblDate.AutoSize = true;
      this.lblDate.Location = new Point(162, 118);
      this.lblDate.Name = "lblDate";
      this.lblDate.Size = new Size(40, 16);
      this.lblDate.TabIndex = 4;
      this.lblDate.Text = "Дата";
      this.txbNum.Location = new Point(64, 117);
      this.txbNum.Name = "txbNum";
      this.txbNum.Size = new Size(71, 22);
      this.txbNum.TabIndex = 5;
      this.dtpDate.Format = DateTimePickerFormat.Short;
      this.dtpDate.Location = new Point(208, 115);
      this.dtpDate.Name = "dtpDate";
      this.dtpDate.Size = new Size(107, 22);
      this.dtpDate.TabIndex = 6;
      this.dtpDate.ValueChanged += new EventHandler(this.dtpDate_ValueChanged);
      this.lblKol.AutoSize = true;
      this.lblKol.Location = new Point(3, 177);
      this.lblKol.Name = "lblKol";
      this.lblKol.Size = new Size(176, 16);
      this.lblKol.TabIndex = 8;
      this.lblKol.Text = "Количество экземпляров";
      this.nudCountBill.Location = new Point(271, 177);
      this.nudCountBill.Name = "nudCountBill";
      this.nudCountBill.Size = new Size(44, 22);
      this.nudCountBill.TabIndex = 10;
      NumericUpDown nudCountBill = this.nudCountBill;
      int[] bits1 = new int[4];
      bits1[0] = 1;
      Decimal num1 = new Decimal(bits1);
      nudCountBill.Value = num1;
      this.chbIsAkt.AutoSize = true;
      this.chbIsAkt.Location = new Point(6, 229);
      this.chbIsAkt.Name = "chbIsAkt";
      this.chbIsAkt.Size = new Size(248, 20);
      this.chbIsAkt.TabIndex = 12;
      this.chbIsAkt.Text = "Печатать акт выполненных работ";
      this.chbIsAkt.UseVisualStyleBackColor = true;
      this.nudCountAkt.Location = new Point(271, 228);
      this.nudCountAkt.Name = "nudCountAkt";
      this.nudCountAkt.Size = new Size(44, 22);
      this.nudCountAkt.TabIndex = 13;
      NumericUpDown nudCountAkt = this.nudCountAkt;
      int[] bits2 = new int[4];
      bits2[0] = 1;
      Decimal num2 = new Decimal(bits2);
      nudCountAkt.Value = num2;
      this.chbIsBlank.AutoSize = true;
      this.chbIsBlank.Location = new Point(6, (int) byte.MaxValue);
      this.chbIsBlank.Name = "chbIsBlank";
      this.chbIsBlank.Size = new Size(208, 20);
      this.chbIsBlank.TabIndex = 14;
      this.chbIsBlank.Text = "Печатать бланк-квитанцию";
      this.chbIsBlank.UseVisualStyleBackColor = true;
      this.chbSost.AutoSize = true;
      this.chbSost.Location = new Point(6, 281);
      this.chbSost.Name = "chbSost";
      this.chbSost.Size = new Size(228, 20);
      this.chbSost.TabIndex = 15;
      this.chbSost.Text = "С разбивкой по составляющим";
      this.chbSost.UseVisualStyleBackColor = true;
      this.lblSrok.AutoSize = true;
      this.lblSrok.Location = new Point(3, 304);
      this.lblSrok.Name = "lblSrok";
      this.lblSrok.Size = new Size(91, 16);
      this.lblSrok.TabIndex = 16;
      this.lblSrok.Text = "Срок оплаты";
      this.dtpSrok.Location = new Point(100, 299);
      this.dtpSrok.Name = "dtpSrok";
      this.dtpSrok.Size = new Size(142, 22);
      this.dtpSrok.TabIndex = 17;
      this.chbMec.AutoSize = true;
      this.chbMec.Location = new Point(6, 342);
      this.chbMec.Name = "chbMec";
      this.chbMec.Size = new Size(188, 20);
      this.chbMec.TabIndex = 18;
      this.chbMec.Text = "С разбивкой по месяцам";
      this.chbMec.UseVisualStyleBackColor = true;
      this.chbMec.Click += new EventHandler(this.chbMec_Click);
      this.chbCorrect.AutoSize = true;
      this.chbCorrect.Enabled = false;
      this.chbCorrect.Location = new Point(6, 377);
      this.chbCorrect.Name = "chbCorrect";
      this.chbCorrect.Size = new Size(248, 20);
      this.chbCorrect.TabIndex = 19;
      this.chbCorrect.Text = "Корректировочный счет-фактура";
      this.chbCorrect.UseVisualStyleBackColor = true;
      this.chbCorrect.Click += new EventHandler(this.chbCorrect_Click);
      this.nudNumCorrect.Enabled = false;
      this.nudNumCorrect.Location = new Point(271, 254);
      this.nudNumCorrect.Name = "nudNumCorrect";
      this.nudNumCorrect.Size = new Size(44, 22);
      this.nudNumCorrect.TabIndex = 20;
      NumericUpDown nudNumCorrect = this.nudNumCorrect;
      int[] bits3 = new int[4];
      bits3[0] = 1;
      Decimal num3 = new Decimal(bits3);
      nudNumCorrect.Value = num3;
      this.nudNumCorrect.Visible = false;
      this.Prepare.HeaderText = "Готовность";
      this.Prepare.Name = "Prepare";
      this.Num.HeaderText = "№ Счета";
      this.Num.Name = "Num";
      this.DocNum.HeaderText = "№ Договора";
      this.DocNum.Name = "DocNum";
      this.Id.HeaderText = "Лицевой";
      this.Id.Name = "Id";
      this.Id.Visible = false;
      this.dgvList.AllowUserToAddRows = false;
      this.dgvList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dgvList.BackgroundColor = Color.AliceBlue;
      this.dgvList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvList.Columns.AddRange((DataGridViewColumn) this.Id, (DataGridViewColumn) this.DocNum, (DataGridViewColumn) this.Num, (DataGridViewColumn) this.Prepare);
      this.dgvList.Location = new Point(0, 642);
      this.dgvList.Name = "dgvList";
      this.dgvList.Size = new Size(942, 178);
      this.dgvList.TabIndex = 21;
      this.dgvList.VirtualMode = true;
      this.dgvList.CellValueNeeded += new DataGridViewCellValueEventHandler(this.dgvList_CellValueNeeded);
      this.txbNumCor.Enabled = false;
      this.txbNumCor.Location = new Point(271, 375);
      this.txbNumCor.Name = "txbNumCor";
      this.txbNumCor.Size = new Size(60, 22);
      this.txbNumCor.TabIndex = 22;
      this.cbReceipt.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.cbReceipt.FormattingEnabled = true;
      this.cbReceipt.Location = new Point(64, 83);
      this.cbReceipt.Name = "cbReceipt";
      this.cbReceipt.Size = new Size(862, 24);
      this.cbReceipt.TabIndex = 23;
      this.cbReceipt.SelectionChangeCommitted += new EventHandler(this.cbReceipt_SelectionChangeCommitted);
      this.lblReceipt.AutoSize = true;
      this.lblReceipt.Location = new Point(3, 86);
      this.lblReceipt.Name = "lblReceipt";
      this.lblReceipt.Size = new Size(55, 16);
      this.lblReceipt.TabIndex = 24;
      this.lblReceipt.Text = "Группа";
      this.textBox1.Location = new Point(248, 299);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(205, 22);
      this.textBox1.TabIndex = 25;
      this.lblTypeAddress.AutoSize = true;
      this.lblTypeAddress.Location = new Point(3, 406);
      this.lblTypeAddress.Name = "lblTypeAddress";
      this.lblTypeAddress.Size = new Size(159, 16);
      this.lblTypeAddress.TabIndex = 26;
      this.lblTypeAddress.Text = "Тип адреса для письма";
      this.cbTypeAddress.FormattingEnabled = true;
      this.cbTypeAddress.Items.AddRange(new object[5]
      {
        (object) "",
        (object) "По умолчанию",
        (object) "Почтовый",
        (object) "Адрес лицевого",
        (object) "Юридический"
      });
      this.cbTypeAddress.Location = new Point(210, 403);
      this.cbTypeAddress.Name = "cbTypeAddress";
      this.cbTypeAddress.Size = new Size(121, 24);
      this.cbTypeAddress.TabIndex = 27;
      this.labShablon.AutoSize = true;
      this.labShablon.Location = new Point(3, 507);
      this.labShablon.Name = "labShablon";
      this.labShablon.Size = new Size(194, 16);
      this.labShablon.TabIndex = 31;
      this.labShablon.Text = "Выберите шаблон для Счета";
      this.labAkt.AutoSize = true;
      this.labAkt.Location = new Point(3, 543);
      this.labAkt.Name = "labAkt";
      this.labAkt.Size = new Size(185, 16);
      this.labAkt.TabIndex = 32;
      this.labAkt.Text = "Выберите шаблон для Акта";
      this.cbSaveReport.AutoSize = true;
      this.cbSaveReport.Location = new Point(6, 451);
      this.cbSaveReport.Name = "cbSaveReport";
      this.cbSaveReport.Size = new Size(146, 20);
      this.cbSaveReport.TabIndex = 35;
      this.cbSaveReport.Text = "Сохранить отчеты";
      this.cbSaveReport.UseVisualStyleBackColor = true;
      this.rbFullPeniPeriod.AutoSize = true;
      this.rbFullPeniPeriod.Location = new Point(345, 9);
      this.rbFullPeniPeriod.Name = "rbFullPeniPeriod";
      this.rbFullPeniPeriod.Size = new Size(182, 20);
      this.rbFullPeniPeriod.TabIndex = 37;
      this.rbFullPeniPeriod.Text = "Полные пени за период";
      this.rbFullPeniPeriod.UseVisualStyleBackColor = true;
      this.gbPeni.Controls.Add((Control) this.rbPeniPeriod);
      this.gbPeni.Controls.Add((Control) this.rbFullPeniPeriod);
      this.gbPeni.Controls.Add((Control) this.chbPeni);
      this.gbPeni.Location = new Point(6, 142);
      this.gbPeni.Name = "gbPeni";
      this.gbPeni.Size = new Size(920, 32);
      this.gbPeni.TabIndex = 38;
      this.gbPeni.TabStop = false;
      this.rbPeniPeriod.AutoSize = true;
      this.rbPeniPeriod.Checked = true;
      this.rbPeniPeriod.Location = new Point(179, 9);
      this.rbPeniPeriod.Name = "rbPeniPeriod";
      this.rbPeniPeriod.Size = new Size(130, 20);
      this.rbPeniPeriod.TabIndex = 36;
      this.rbPeniPeriod.TabStop = true;
      this.rbPeniPeriod.Text = "Пени за период";
      this.rbPeniPeriod.UseVisualStyleBackColor = true;
      this.chbPeni.AutoSize = true;
      this.chbPeni.Checked = true;
      this.chbPeni.CheckState = CheckState.Checked;
      this.chbPeni.Location = new Point(6, 10);
      this.chbPeni.Name = "chbPeni";
      this.chbPeni.Size = new Size(125, 20);
      this.chbPeni.TabIndex = 7;
      this.chbPeni.Text = "Печатать пени";
      this.chbPeni.UseVisualStyleBackColor = true;
      this.mpToPeriod.CustomFormat = "MMMM yyyy";
      this.mpToPeriod.Format = DateTimePickerFormat.Custom;
      this.mpToPeriod.Location = new Point(201, 12);
      this.mpToPeriod.Name = "mpToPeriod";
      this.mpToPeriod.OldMonth = 9;
      this.mpToPeriod.ShowUpDown = true;
      this.mpToPeriod.Size = new Size(140, 22);
      this.mpToPeriod.TabIndex = 42;
      this.gbMountBreak.Controls.Add((Control) this.mpFromPeriod);
      this.gbMountBreak.Controls.Add((Control) this.mpToPeriod);
      this.gbMountBreak.Controls.Add((Control) this.label1);
      this.gbMountBreak.Controls.Add((Control) this.label2);
      this.gbMountBreak.Location = new Point(200, 326);
      this.gbMountBreak.Name = "gbMountBreak";
      this.gbMountBreak.Size = new Size(351, 36);
      this.gbMountBreak.TabIndex = 43;
      this.gbMountBreak.TabStop = false;
      this.gbMountBreak.Visible = false;
      this.mpFromPeriod.CustomFormat = "MMMM yyyy";
      this.mpFromPeriod.Format = DateTimePickerFormat.Custom;
      this.mpFromPeriod.Location = new Point(25, 12);
      this.mpFromPeriod.Name = "mpFromPeriod";
      this.mpFromPeriod.OldMonth = 9;
      this.mpFromPeriod.ShowUpDown = true;
      this.mpFromPeriod.Size = new Size(140, 22);
      this.mpFromPeriod.TabIndex = 39;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(4, 17);
      this.label1.Name = "label1";
      this.label1.Size = new Size(15, 16);
      this.label1.TabIndex = 40;
      this.label1.Text = "с";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(171, 17);
      this.label2.Name = "label2";
      this.label2.Size = new Size(24, 16);
      this.label2.TabIndex = 41;
      this.label2.Text = "по";
      this.cbLetterCover.AutoSize = true;
      this.cbLetterCover.Location = new Point(210, 451);
      this.cbLetterCover.Name = "cbLetterCover";
      this.cbLetterCover.Size = new Size(204, 20);
      this.cbLetterCover.TabIndex = 44;
      this.cbLetterCover.Text = "Сопроводительное письмо";
      this.cbLetterCover.UseVisualStyleBackColor = true;
      this.chbOnlyNew.AutoSize = true;
      this.chbOnlyNew.Location = new Point(6, 203);
      this.chbOnlyNew.Name = "chbOnlyNew";
      this.chbOnlyNew.Size = new Size(209, 20);
      this.chbOnlyNew.TabIndex = 11;
      this.chbOnlyNew.Text = "Только несформированные";
      this.chbOnlyNew.UseVisualStyleBackColor = true;
      this.chbEmail.AutoSize = true;
      this.chbEmail.Location = new Point(6, 597);
      this.chbEmail.Name = "chbEmail";
      this.chbEmail.Size = new Size(146, 20);
      this.chbEmail.TabIndex = 45;
      this.chbEmail.Text = "Отправка на email";
      this.chbEmail.UseVisualStyleBackColor = true;
      this.chbEmail.CheckedChanged += new EventHandler(this.chbEmail_CheckedChanged);
      this.chbStampDisplay.AutoSize = true;
      this.chbStampDisplay.Location = new Point(6, 619);
      this.chbStampDisplay.Name = "chbStampDisplay";
      this.chbStampDisplay.Size = new Size(223, 20);
      this.chbStampDisplay.TabIndex = 46;
      this.chbStampDisplay.Text = "Отображать печать компании";
      this.chbStampDisplay.UseVisualStyleBackColor = true;
      this.chbCorrectMonth.AutoSize = true;
      this.chbCorrectMonth.Location = new Point(351, 377);
      this.chbCorrectMonth.Name = "chbCorrectMonth";
      this.chbCorrectMonth.Size = new Size(328, 20);
      this.chbCorrectMonth.TabIndex = 47;
      this.chbCorrectMonth.Text = "Сразбивкой по месецам для коррю счет факт.";
      this.chbCorrectMonth.UseVisualStyleBackColor = true;
      this.chbCorrectMonth.Visible = false;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(942, 866);
      this.Controls.Add((Control) this.chbCorrectMonth);
      this.Controls.Add((Control) this.chbStampDisplay);
      this.Controls.Add((Control) this.chbEmail);
      this.Controls.Add((Control) this.cbLetterCover);
      this.Controls.Add((Control) this.gbMountBreak);
      this.Controls.Add((Control) this.gbPeni);
      this.Controls.Add((Control) this.cbSaveReport);
      this.Controls.Add((Control) this.checkBoxMinusOff);
      this.Controls.Add((Control) this.cbAkt);
      this.Controls.Add((Control) this.labAkt);
      this.Controls.Add((Control) this.labShablon);
      this.Controls.Add((Control) this.cbShablon);
      this.Controls.Add((Control) this.chB_Offer_FR);
      this.Controls.Add((Control) this.RoGbVisible);
      this.Controls.Add((Control) this.cbTypeAddress);
      this.Controls.Add((Control) this.lblTypeAddress);
      this.Controls.Add((Control) this.textBox1);
      this.Controls.Add((Control) this.lblReceipt);
      this.Controls.Add((Control) this.cbReceipt);
      this.Controls.Add((Control) this.txbNumCor);
      this.Controls.Add((Control) this.dgvList);
      this.Controls.Add((Control) this.nudNumCorrect);
      this.Controls.Add((Control) this.chbCorrect);
      this.Controls.Add((Control) this.chbMec);
      this.Controls.Add((Control) this.dtpSrok);
      this.Controls.Add((Control) this.lblSrok);
      this.Controls.Add((Control) this.chbSost);
      this.Controls.Add((Control) this.chbIsBlank);
      this.Controls.Add((Control) this.nudCountAkt);
      this.Controls.Add((Control) this.chbIsAkt);
      this.Controls.Add((Control) this.chbOnlyNew);
      this.Controls.Add((Control) this.nudCountBill);
      this.Controls.Add((Control) this.lblKol);
      this.Controls.Add((Control) this.dtpDate);
      this.Controls.Add((Control) this.txbNum);
      this.Controls.Add((Control) this.lblDate);
      this.Controls.Add((Control) this.lblNum);
      this.Controls.Add((Control) this.pnBtn);
      this.Controls.Add((Control) this.gbBill);
      this.Controls.Add((Control) this.pnPeriod);
      this.Name = "FrmBills";
      this.Text = "Печать счетов";
      this.Load += new EventHandler(this.FrmBills_Load);
      this.Shown += new EventHandler(this.FrmBills_Shown);
      this.report.EndInit();
      this.pnBtn.ResumeLayout(false);
      this.gbBill.ResumeLayout(false);
      this.gbBill.PerformLayout();
      this.pnPeriod.ResumeLayout(false);
      this.pnPeriod.PerformLayout();
      this.nudCountBill.EndInit();
      this.nudCountAkt.EndInit();
      this.nudNumCorrect.EndInit();
      ((ISupportInitialize) this.dgvList).EndInit();
      this.gbPeni.ResumeLayout(false);
      this.gbPeni.PerformLayout();
      this.gbMountBreak.ResumeLayout(false);
      this.gbMountBreak.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
