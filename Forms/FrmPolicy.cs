// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmPolicy
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using FastReport;
using FastReport.Data;
using Kvartplata.Classes;
using Kvartplata.Properties;
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmPolicy : FrmBaseForm
  {
    private FormStateSaver fss = new FormStateSaver(FrmPolicy.container);
    private IContainer components = (IContainer) null;
    private static IContainer container;
    private short level;
    private Raion raion;
    private Company company;
    private Home home;
    private LsClient client;
    private ISession session;
    private IList listGroup;
    private IList listDistrict;
    private Panel pnBtn;
    private Button btnExit;
    private Button btnPrint;
    private ComboBox cmbGroup;
    private CheckedListBox clbDistrict;
    private Label lblGroup;
    private Label lblDistrict;
    private Label lblCaption;
    private FastReport.Report reportPolicy;
    private MonthPicker mpCurrentPeriod;

    public FrmPolicy()
    {
      this.InitializeComponent();
    }

    public FrmPolicy(short l, Raion r, Company c, Home h, LsClient lc)
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
      this.level = l;
      this.raion = r;
      this.company = c;
      this.home = h;
      this.client = lc;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void FrmPolicy_Load(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
      switch (this.level)
      {
        case 0:
          this.lblCaption.Text = "Все районы";
          break;
        case 1:
          this.lblCaption.Text = this.raion.Rnn;
          break;
        case 2:
          this.lblCaption.Text = this.company.CompanyName;
          break;
        case 3:
          this.lblCaption.Text = this.home.Address;
          break;
      }
      if ((int) this.level >= 3)
      {
        this.lblDistrict.Enabled = false;
        this.lblGroup.Enabled = false;
        this.cmbGroup.Enabled = false;
        this.clbDistrict.Enabled = false;
      }
      try
      {
        this.listGroup = this.session.CreateSQLQuery("select idgr,grname from district_group where codpr in (4,103) and (kodbd=1 or kodbd in (select multiplkod from kodbd)) order by idgr").List();
        foreach (object[] objArray in (IEnumerable) this.listGroup)
          this.cmbGroup.Items.Add(objArray[1]);
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, this.client);
      }
    }

    private void cmbGroup_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.listDistrict = this.session.CreateSQLQuery("select iddistrict,districtname from district where idgr=" + Convert.ToInt32(((object[]) this.listGroup[this.cmbGroup.SelectedIndex])[0]).ToString() + " and (iddistrict between (select multiplkod from kodbd) and (select multiplkod from kodbd)+1000000) order by lengthhome(districtname),districtname ").List();
      this.clbDistrict.Items.Clear();
      foreach (object[] objArray in (IEnumerable) this.listDistrict)
        this.clbDistrict.Items.Add(objArray[1]);
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      string str1 = "";
      string strSelect = "";
      foreach (string checkedItem in this.clbDistrict.CheckedItems)
      {
        int int32 = Convert.ToInt32(((object[]) this.listDistrict[this.clbDistrict.FindString(checkedItem)])[0]);
        str1 = str1 + int32.ToString() + ",";
      }
      if (str1 != "")
      {
        string str2 = str1.Substring(0, str1.Length - 1);
        strSelect = "  and lc.idhome in (select idhome from districthome where iddistrict in (" + str2 + "))";
        str1 = "  and lc.Home.IdHome in (select Home.IdHome from DistrictHome where DistrictId in (" + str2 + "))";
      }
      int num1;
      if (this.raion != null)
      {
        string str2 = str1;
        string str3 = " and lc.Company.Raion.IdRnn=";
        num1 = this.raion.IdRnn;
        string str4 = num1.ToString();
        str1 = str2 + str3 + str4;
        string str5 = strSelect;
        string str6 = " and c.rnn_id=";
        num1 = this.raion.IdRnn;
        string str7 = num1.ToString();
        strSelect = str5 + str6 + str7;
      }
      if (this.company != null)
      {
        string str2 = str1;
        string str3 = " and lc.Company.CompanyId=";
        short companyId = this.company.CompanyId;
        string str4 = companyId.ToString();
        str1 = str2 + str3 + str4;
        string str5 = strSelect;
        string str6 = " and c.company_id=";
        companyId = this.company.CompanyId;
        string str7 = companyId.ToString();
        strSelect = str5 + str6 + str7;
      }
      if (this.home != null)
      {
        string str2 = str1;
        string str3 = " and lc.Home.IdHome=";
        num1 = this.home.IdHome;
        string str4 = num1.ToString();
        str1 = str2 + str3 + str4;
        string str5 = strSelect;
        string str6 = " and lc.idhome=";
        num1 = this.home.IdHome;
        string str7 = num1.ToString();
        strSelect = str5 + str6 + str7;
      }
      Period period1 = new Period();
      Period period2 = this.company == null ? (this.raion == null ? this.session.CreateQuery(string.Format("select max(Period) from CompanyPeriod cp ")).List<Period>()[0] : this.session.Get<Period>((object) Convert.ToInt32(this.session.CreateQuery(string.Format("select max(cp.Period.PeriodId) from CompanyPeriod cp,Company c where cp.Company.CompanyId=c.CompanyId and c.Raion.IdRnn = {0}", (object) this.raion.IdRnn)).List()[0]))) : KvrplHelper.GetCmpKvrClose(this.company, Options.Complex.ComplexId, Options.ComplexPasp.ComplexId);
      using (ITransaction transaction = this.session.BeginTransaction())
      {
        try
        {
          this.session.Clear();
          if (Options.Period.PeriodId == period2.PeriodId)
          {
            this.Cursor = Cursors.WaitCursor;
            int num2;
            try
            {
              DateTime? periodName = Options.Period.PeriodName;
              DateTime dateTime1 = periodName.Value;
              // ISSUE: explicit reference operation
              // ISSUE: variable of a reference type
              DateTime local = dateTime1;
              periodName = Options.Period.PeriodName;
              DateTime dateTime2 = periodName.Value;
              dateTime2 = dateTime2.AddMonths(1);
              int months = -dateTime2.Month + 2;
              // ISSUE: explicit reference operation
              num2 = Convert.ToInt32(this.session.CreateQuery(string.Format("select max(PolicyNum) from Policy where DBeg>='{0}'", (object) KvrplHelper.DateToBaseFormat(local.AddMonths(months)))).List()[0]);
            }
            catch
            {
              num2 = 0;
            }
            ISession session = this.session;
            string format = "select p from Balance b left outer join b.LsClient lc,Policy p where b.Period.PeriodId={5} and b.Service.ServiceId in (select ServiceId from Service where root=26) and b.Rent>0 and b.BalanceIn<=0 and b.LsClient.ClientId=p.LsClient.ClientId and p.DBeg<='{3}' and p.DEnd>='{4}'" + str1 + " and b.LsClient.ClientId not in (select LsClient.ClientId from Balance where Period.PeriodId in ({0},{1},{2}) and Service.ServiceId in (select ServiceId from Service where root=26))";
            object[] objArray = new object[6]{ (object) (Options.Period.PeriodId - 1), (object) (Options.Period.PeriodId - 2), (object) (Options.Period.PeriodId - 3), null, null, null };
            int index1 = 3;
            DateTime? periodName1 = Options.Period.PeriodName;
            DateTime dateTime3 = periodName1.Value;
            string baseFormat1 = KvrplHelper.DateToBaseFormat(dateTime3.AddMonths(-3));
            objArray[index1] = (object) baseFormat1;
            int index2 = 4;
            periodName1 = Options.Period.PeriodName;
            string baseFormat2 = KvrplHelper.DateToBaseFormat(periodName1.Value);
            objArray[index2] = (object) baseFormat2;
            int index3 = 5;
            // ISSUE: variable of a boxed type
            int periodId = Options.Period.PeriodId;
            objArray[index3] = (object) periodId;
            string queryString = string.Format(format, objArray);
            foreach (Kvartplata.Classes.Policy policy1 in (IEnumerable<Kvartplata.Classes.Policy>) session.CreateQuery(queryString).List<Kvartplata.Classes.Policy>())
            {
              this.session.CreateQuery(string.Format("update Policy set DEnd=(select PeriodName-1 from Period where PeriodId=(select max(Period.PeriodId)+4 from Balance where Period.PeriodId<:period and LsClient.ClientId=:cl and Service.ServiceId in (select ServiceId from Service where Root=26) and Rent<>0)) where LsClient.ClientId=:cl and DBeg=:dbeg")).SetParameter<int>("cl", policy1.LsClient.ClientId).SetParameter<int>("period", Options.Period.PeriodId).SetDateTime("dbeg", policy1.DBeg).ExecuteUpdate();
              ++num2;
              Kvartplata.Classes.Policy policy2 = new Kvartplata.Classes.Policy();
              policy2.LsClient = policy1.LsClient;
              policy2.PolicyNum = num2;
              Kvartplata.Classes.Policy policy3 = policy2;
              periodName1 = Options.Period.PeriodName;
              dateTime3 = periodName1.Value;
              DateTime dateTime1 = dateTime3.AddMonths(1);
              policy3.DBeg = dateTime1;
              Kvartplata.Classes.Policy policy4 = policy2;
              periodName1 = Options.Period.PeriodName;
              dateTime3 = periodName1.Value;
              dateTime3 = dateTime3.AddYears(1);
              dateTime3 = dateTime3.AddMonths(1);
              DateTime dateTime2 = dateTime3.AddDays(-1.0);
              policy4.DEnd = dateTime2;
              policy2.UName = Options.Login;
              policy2.DEdit = DateTime.Now;
              this.session.Save((object) policy2);
            }
            this.session.Flush();
            foreach (LsClient lsClient in (IEnumerable<LsClient>) this.session.CreateQuery(string.Format("select distinct lc from Balance b left outer join b.LsClient lc where b.Period.PeriodId={0} and b.Service.ServiceId in (select ServiceId from Service where root=26) and b.Rent>0 and b.BalanceOut<=0" + str1 + "and b.LsClient.ClientId not in (select LsClient.ClientId from Policy where DBeg<='{1}' and DEnd>'{1}') order by lc.ClientId", (object) Options.Period.PeriodId, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value.AddMonths(1)), (object) Options.Login, (object) KvrplHelper.DateToBaseFormat(DateTime.Now))).List<LsClient>())
            {
              ++num2;
              Kvartplata.Classes.Policy policy1 = new Kvartplata.Classes.Policy();
              policy1.LsClient = lsClient;
              policy1.PolicyNum = num2;
              Kvartplata.Classes.Policy policy2 = policy1;
              DateTime dateTime1 = Options.Period.PeriodName.Value;
              DateTime dateTime2 = dateTime1.AddMonths(1);
              policy2.DBeg = dateTime2;
              Kvartplata.Classes.Policy policy3 = policy1;
              dateTime1 = Options.Period.PeriodName.Value;
              dateTime1 = dateTime1.AddYears(1);
              dateTime1 = dateTime1.AddMonths(1);
              DateTime dateTime4 = dateTime1.AddDays(-1.0);
              policy3.DEnd = dateTime4;
              policy1.UName = Options.Login;
              policy1.DEdit = DateTime.Now;
              this.session.Save((object) policy1);
            }
            this.session.Flush();
          }
          else if (Options.Period.PeriodId > period2.PeriodId)
          {
            int num2 = (int) MessageBox.Show("Невозможно распечатать полисы в открытом месяце", "Ошибка", MessageBoxButtons.OK);
            return;
          }
          this.Print(strSelect);
        }
        catch (Exception ex)
        {
          this.Cursor = Cursors.Default;
          int num2 = (int) MessageBox.Show("Не удалось завести договоры и распечатать полисы", "Ошибка", MessageBoxButtons.OK);
          KvrplHelper.WriteLog(ex, (LsClient) null);
          transaction.Rollback();
          return;
        }
        transaction.Commit();
      }
      this.Cursor = Cursors.Default;
    }

    private void mpCurrentPeriod_ValueChanged(object sender, EventArgs e)
    {
      Options.Period = KvrplHelper.SaveCurrentPeriod(this.mpCurrentPeriod.Value);
    }

    private void Print(string strSelect)
    {
      string str = string.Format("cast('{0}' as date)", (object) KvrplHelper.DateTimeToBaseFormat(Options.Period.PeriodName.Value));
      string fdat = string.Format("cast('{0}' as date)", (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(Options.Period.PeriodName.Value)));
      this.reportPolicy = new Report();
      this.reportPolicy.Load(AppDomain.CurrentDomain.BaseDirectory + "Шаблоны_кв\\Policy.frx");
      TableDataSource dataSource = this.reportPolicy.GetDataSource("Policy") as TableDataSource;
      dataSource.Connection.ConnectionString = string.Format("Dsn={0};uid={1};pwd={2}", (object) Options.Alias, (object) Options.Login, (object) Options.Pwd);
      dataSource.SelectCommand = string.Format("select p.policy_num,p.client_id,p.dbeg,p.dend,lc.numberroom,lc.idhome,lc.floor,d.str,h.home,h.home_korp,f.nflat," + KvrplHelper.NS_lsOwner(1, 2, str, str, "lc.client_id", fdat) + " as fio from lsPolicy p, lsClient lc,dcCompany c,di_str d,homes h,homelink hl,flats f where p.client_id=lc.client_id and c.company_id=lc.company_id and hl.idhome=h.idhome and hl.codeu=lc.company_id and        h.idhome=lc.idhome and h.idstr=d.idstr and lc.idflat=f.idflat and p.dbeg='{0}' " + strSelect + " order by policy_num", (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value.AddMonths(1)));
      this.reportPolicy.Show();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmPolicy));
      this.lblCaption = new Label();
      this.lblDistrict = new Label();
      this.lblGroup = new Label();
      this.clbDistrict = new CheckedListBox();
      this.cmbGroup = new ComboBox();
      this.pnBtn = new Panel();
      this.btnPrint = new Button();
      this.btnExit = new Button();
      this.reportPolicy = new Report();
      this.mpCurrentPeriod = new MonthPicker();
      this.pnBtn.SuspendLayout();
      this.reportPolicy.BeginInit();
      this.SuspendLayout();
      this.lblCaption.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblCaption.Location = new Point(0, 9);
      this.lblCaption.Name = "lblCaption";
      this.lblCaption.Size = new Size(607, 47);
      this.lblCaption.TabIndex = 5;
      this.lblCaption.Text = "kl\r\n\r\n\r\n";
      this.lblDistrict.AutoSize = true;
      this.lblDistrict.Location = new Point(315, 59);
      this.lblDistrict.Name = "lblDistrict";
      this.lblDistrict.Size = new Size(62, 16);
      this.lblDistrict.TabIndex = 4;
      this.lblDistrict.Text = "Участок";
      this.lblGroup.AutoSize = true;
      this.lblGroup.Location = new Point(3, 62);
      this.lblGroup.Name = "lblGroup";
      this.lblGroup.Size = new Size(55, 16);
      this.lblGroup.TabIndex = 3;
      this.lblGroup.Text = "Группа";
      this.clbDistrict.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.clbDistrict.CheckOnClick = true;
      this.clbDistrict.FormattingEnabled = true;
      this.clbDistrict.Location = new Point(383, 56);
      this.clbDistrict.Name = "clbDistrict";
      this.clbDistrict.Size = new Size(330, 140);
      this.clbDistrict.TabIndex = 2;
      this.cmbGroup.FormattingEnabled = true;
      this.cmbGroup.Location = new Point(64, 59);
      this.cmbGroup.Name = "cmbGroup";
      this.cmbGroup.Size = new Size(236, 24);
      this.cmbGroup.TabIndex = 1;
      this.cmbGroup.SelectionChangeCommitted += new EventHandler(this.cmbGroup_SelectionChangeCommitted);
      this.pnBtn.Controls.Add((Control) this.btnPrint);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 197);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(725, 40);
      this.pnBtn.TabIndex = 0;
      this.btnPrint.Image = (Image) Resources.notepad_32;
      this.btnPrint.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnPrint.Location = new Point(12, 5);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(206, 30);
      this.btnPrint.TabIndex = 1;
      this.btnPrint.Text = "Сформировать полисы";
      this.btnPrint.TextAlign = ContentAlignment.MiddleRight;
      this.btnPrint.UseVisualStyleBackColor = true;
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(635, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(78, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.reportPolicy.ReportResourceString = componentResourceManager.GetString("reportPolicy.ReportResourceString");
      this.mpCurrentPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.mpCurrentPeriod.CustomFormat = "MMMM yyyy";
      this.mpCurrentPeriod.Format = DateTimePickerFormat.Custom;
      this.mpCurrentPeriod.Location = new Point(573, 7);
      this.mpCurrentPeriod.Name = "mpCurrentPeriod";
      this.mpCurrentPeriod.OldMonth = 0;
      this.mpCurrentPeriod.ShowUpDown = true;
      this.mpCurrentPeriod.Size = new Size(140, 22);
      this.mpCurrentPeriod.TabIndex = 6;
      this.mpCurrentPeriod.ValueChanged += new EventHandler(this.mpCurrentPeriod_ValueChanged);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(725, 237);
      this.Controls.Add((Control) this.mpCurrentPeriod);
      this.Controls.Add((Control) this.lblCaption);
      this.Controls.Add((Control) this.lblDistrict);
      this.Controls.Add((Control) this.lblGroup);
      this.Controls.Add((Control) this.clbDistrict);
      this.Controls.Add((Control) this.cmbGroup);
      this.Controls.Add((Control) this.pnBtn);
      this.Name = "FrmPolicy";
      this.Text = "Печать полисов";
      this.Load += new EventHandler(this.FrmPolicy_Load);
      this.pnBtn.ResumeLayout(false);
      this.reportPolicy.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
