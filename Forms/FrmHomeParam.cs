// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmHomeParam
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using SaveSettings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmHomeParam : Form
  {
    private static Logger log = LogManager.GetCurrentClassLogger();
    private readonly FormStateSaver fss = new FormStateSaver(FrmHomeParam.ic);
    protected GridSettings MySettingsDogovor = new GridSettings();
    protected GridSettings MySettingsHomeParam = new GridSettings();
    private IContainer components = (IContainer) null;
    private const short HomeParamType = 2;
    private const short CommonParamType = 0;
    private static IContainer ic;
    private readonly DateTime LastDayMonthClosed;
    private readonly Period MonthClosed;
    private readonly Period NextMonthClosed;
    private readonly Company company;
    private bool Arhiv;
    private Period CurrentPeriod;
    private bool InsertRecord;
    private HomeParam OldHomeParam;
    private HomesPhones OldHomesPhones;
    private bool PastTime;
    private int SelectedIndexHomeParam;
    private int SelectedIndexHomesPhones;
    private Home home;
    private Dogovor oldDogovor;
    private IList<Dogovor> oldListDogovors;
    private ISession session;
    private IList<ohlAccounts> ohlAccountsList;
    private Panel pnButtons;
    private Panel panel1;
    private Button btnDelRecord;
    private Button btnAddRecord;
    private Button btnSave;
    private Button butExit;
    private ProgressBar progressBar1;
    private Button btnPastTime;
    private Panel pnUp;
    private Label lblMonthCLosed;
    private Label lblClosed;
    private Label lblAddress;
    private CheckBox chbArhiv;
    private TabControl tcntrHome;
    private TabPage pgParam;
    private DataGridView dataGridView1;
    private TabPage pgMain;
    private TabPage pgReceipt;
    private Panel panel4;
    private Label label9;
    private TextBox tbWall;
    private Label label8;
    private TextBox tbYearBuild;
    private Label label7;
    private TextBox tbReu;
    private Label label6;
    private TextBox tbGroupHome;
    private Label label5;
    private TextBox tbIdHome;
    private Label label4;
    private TextBox tbKorp;
    private Label label3;
    private TextBox tbNHome;
    private Label label2;
    private TextBox tbStreet;
    private Label label1;
    private TextBox tbCompany;
    private Button btnCopy;
    private ContextMenuStrip cmsHomeParam;
    private ToolStripMenuItem скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem;
    private ToolStripMenuItem удалитьЗаписьИзВыбранныхОбъектовToolStripMenuItem;
    private DataGridView dgvHomesPhones;
    private Label lblPastTime;
    private System.Windows.Forms.Timer tmr;
    private TextBox tbIdStr;
    private Label label10;
    private MonthPicker mpCurrentPeriod;
    private ToolStripMenuItem обновитьЗаписьВВыбранныхОбъектовToolStripMenuItem;
    private HelpProvider hp;
    private TextBox txbNumFloor;
    private Label label11;
    private TabPage pgDogovor;
    private DataGridView dgvDogovor;
    private Button butLoadHomeDoc;

    public FrmHomeParam()
    {
      this.fss.ParentForm = (Form) this;
      this.InitializeComponent();
      this.SetGridConfigFileSettings();
    }

    public FrmHomeParam(Home home, Company company)
    {
      this.fss.ParentForm = (Form) this;
      this.InitializeComponent();
      this.home = home;
      this.company = company;
      this.session = Domain.CurrentSession;
      this.CurrentPeriod = Options.Period;
      this.MonthClosed = new Period();
      this.MonthClosed = KvrplHelper.GetKvrClose(home, Options.ComplexPasp, Options.ComplexPrior, (int) company.CompanyId);
      this.NextMonthClosed = new Period();
      this.NextMonthClosed = KvrplHelper.GetNextPeriod(this.MonthClosed);
      this.LastDayMonthClosed = new DateTime();
      this.LastDayMonthClosed = KvrplHelper.GetLastDayPeriod(this.MonthClosed.PeriodName.Value);
      this.mpCurrentPeriod.Value = this.CurrentPeriod.PeriodName.Value;
      this.lblMonthCLosed.Text = this.MonthClosed.PeriodName.Value.ToString("MM.yyyy");
      this.lblAddress.Text = home.Address;
      this.chbArhiv.Checked = false;
      this.chbArhiv.Visible = true;
      new ToolTip().SetToolTip((Control) this.btnCopy, "Скопировать запись в выбранные объекты");
      this.SetGridConfigFileSettings();
    }

    public void SetGridConfigFileSettings()
    {
      this.MySettingsHomeParam.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
      this.MySettingsDogovor.ConfigFile = Options.PathProfileAppData + "\\State\\config.xml";
    }

    private void btnAddRecord_Click(object sender, EventArgs e)
    {
      if (this.tcntrHome.SelectedTab == this.pgParam)
      {
        if (!KvrplHelper.CheckProxy(39, 2, this.home.Company, true))
          return;
        this.InsertParam();
      }
      if (this.tcntrHome.SelectedTab == this.pgReceipt)
        this.InsertHomesPhones();
      if (this.tcntrHome.SelectedTab != this.pgDogovor || !KvrplHelper.CheckProxy(68, 2, this.home.Company, true))
        return;
      this.InsertDogovor();
    }

    private void btnDelRecord_Click(object sender, EventArgs e)
    {
      if (this.tcntrHome.SelectedTab == this.pgParam)
      {
        if (!KvrplHelper.CheckProxy(39, 2, this.home.Company, true))
          return;
        if (this.DelParam())
          this.LoadParam();
      }
      if (this.tcntrHome.SelectedTab == this.pgReceipt && this.DelHomesPhones())
        this.LoadHomesPhones();
      if (this.tcntrHome.SelectedTab != this.pgDogovor || !KvrplHelper.CheckProxy(68, 2, this.home.Company, true))
        return;
      this.DeleteDogovor();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.tcntrHome.SelectedTab == this.pgParam && this.SaveParam())
        this.LoadParam();
      if (this.tcntrHome.SelectedTab == this.pgMain && this.SaveOptions())
        this.LoadHomeOptions();
      if (this.tcntrHome.SelectedTab == this.pgReceipt && this.SaveHomesPhones())
        this.LoadHomesPhones();
      if (this.tcntrHome.SelectedTab != this.pgDogovor || !KvrplHelper.CheckProxy(68, 2, this.home.Company, true) || !this.SaveAllDogovors())
        return;
      this.LoadDogovors();
    }

    private void butExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void FrmHomeParam_Shown(object sender, EventArgs e)
    {
      if (!Options.Kvartplata)
        this.tcntrHome.TabPages.Remove(this.pgReceipt);
      if (!KvrplHelper.CheckProxy(68, 1, this.home.Company, false))
        this.pgDogovor.Dispose();
      this.LoadHomeOptions();
    }

    private void btnPastTime_Click(object sender, EventArgs e)
    {
      if (!this.PastTime)
      {
        this.PastTime = true;
        this.lblPastTime.Visible = true;
        this.btnPastTime.BackColor = Color.DarkOrange;
        this.tmr.Start();
      }
      else
      {
        this.lblPastTime.Visible = false;
        this.btnPastTime.BackColor = this.pnUp.BackColor;
        this.PastTime = false;
        this.tmr.Stop();
      }
      this.LoadParam();
    }

    private void dtmpCurrentPeriod_ValueChanged(object sender, EventArgs e)
    {
      this.CurrentPeriod = KvrplHelper.SaveCurrentPeriod(this.mpCurrentPeriod.Value);
      Options.Period = this.CurrentPeriod;
      if (this.PastTime)
        this.LoadParam();
      this.dataGridView1.Refresh();
    }

    private void chbArhiv_CheckedChanged(object sender, EventArgs e)
    {
      if (this.tcntrHome.SelectedTab == this.pgParam)
      {
        this.Arhiv = this.chbArhiv.Checked;
        this.LoadParam();
      }
      if (this.tcntrHome.SelectedTab != this.pgReceipt)
        return;
      this.LoadHomesPhones();
    }

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.tcntrHome.SelectedTab == this.pgParam)
        this.LoadParam();
      if (this.tcntrHome.SelectedTab == this.pgMain)
        this.LoadHomeOptions();
      if (this.tcntrHome.SelectedTab == this.pgReceipt)
        this.LoadHomesPhones();
      if (this.tcntrHome.SelectedTab != this.pgDogovor)
        return;
      this.LoadDogovors();
    }

    private void скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem_Click(object sender, EventArgs e)
    {
      bool flag = true;
      short num1;
      if (sender.ToString() == "Удалить запись из выбранных объектов")
      {
        flag = false;
        num1 = (short) 2;
      }
      else if (sender.ToString() == "Обновить запись в выбранных объектах")
      {
        num1 = (short) 3;
      }
      else
      {
        flag = true;
        num1 = (short) 1;
      }
      if (this.tcntrHome.SelectedTab == this.pgParam && (this.dataGridView1.Rows.Count > 0 && this.dataGridView1.CurrentRow.Index >= 0))
      {
        HomeParam homeParam = new HomeParam();
        HomeParam dataBoundItem = (HomeParam) this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].DataBoundItem;
        if (!this.PastTime)
        {
          if (dataBoundItem.DBeg > this.LastDayMonthClosed || (int) num1 == 3 && dataBoundItem.DEnd >= this.LastDayMonthClosed)
          {
            FrmChooseObject frmChooseObject = new FrmChooseObject(dataBoundItem);
            frmChooseObject.Save = flag;
            frmChooseObject.CodeOperation = num1;
            frmChooseObject.HomeSave = (short) 1;
            frmChooseObject.MonthClosed = this.MonthClosed;
            int num2 = (int) frmChooseObject.ShowDialog();
            frmChooseObject.Dispose();
          }
          else
          {
            int num2 = (int) MessageBox.Show("Не могу скопировать запись, так как она принадлежит закрытому периоду!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return;
          }
        }
        else if (dataBoundItem.DBeg <= this.LastDayMonthClosed && dataBoundItem.DEnd <= this.LastDayMonthClosed || (int) num1 == 3)
        {
          FrmChooseObject frmChooseObject = new FrmChooseObject(dataBoundItem);
          frmChooseObject.Save = flag;
          frmChooseObject.HomeSave = (short) 1;
          frmChooseObject.CodeOperation = num1;
          frmChooseObject.MonthClosed = this.MonthClosed;
          int num2 = (int) frmChooseObject.ShowDialog();
          frmChooseObject.Dispose();
        }
        else
        {
          int num2 = (int) MessageBox.Show("Не могу скопировать запись, так как она принадлежит закрытому периоду!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return;
        }
      }
      if (this.tcntrHome.SelectedTab != this.pgReceipt || (this.dgvHomesPhones.Rows.Count <= 0 || this.dgvHomesPhones.CurrentRow.Index < 0))
        return;
      HomesPhones homesPhones = new HomesPhones();
      FrmChooseObject frmChooseObject1 = new FrmChooseObject((HomesPhones) this.dgvHomesPhones.Rows[this.dgvHomesPhones.CurrentRow.Index].DataBoundItem, 0);
      frmChooseObject1.Save = flag;
      frmChooseObject1.CodeOperation = num1;
      frmChooseObject1.HomeSave = (short) 3;
      frmChooseObject1.MonthClosed = this.MonthClosed;
      int num3 = (int) frmChooseObject1.ShowDialog();
      frmChooseObject1.Dispose();
    }

    private void tmr_Tick(object sender, EventArgs e)
    {
      if (this.lblPastTime.ForeColor == Color.DarkOrange)
        this.lblPastTime.ForeColor = this.BackColor;
      else
        this.lblPastTime.ForeColor = Color.DarkOrange;
    }

    private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    private void LoadHomeOptions()
    {
      this.home = this.session.Get<Home>((object) this.home.IdHome);
      try
      {
        this.home.NumFloor = this.session.CreateSQLQuery(string.Format("select list(numfloors) from homes_floor where idhome={0}", (object) this.home.IdHome)).List()[0].ToString();
      }
      catch
      {
      }
      this.tbCompany.Text = this.home.Company.CompanyId.ToString();
      this.tbIdHome.Text = this.home.IdHome.ToString();
      this.tbKorp.Text = this.home.HomeKorp;
      this.tbNHome.Text = this.home.NHome;
      this.tbReu.Text = this.home.Reu.ToString();
      this.tbStreet.Text = this.home.Str.NameStr;
      this.tbYearBuild.Text = this.home.YearBuild.ToString();
      this.tbWall.Text = this.home.Mwl != null ? this.home.Mwl.MWall : "";
      this.tbGroupHome.Text = this.home.Division.ToString();
      this.tbIdStr.Text = this.home.Str.IdStr.ToString();
      this.txbNumFloor.Text = this.home.NumFloor.ToString();
      this.DisableControls();
      this.btnDelRecord.Enabled = false;
      this.btnAddRecord.Enabled = false;
      this.btnSave.Enabled = false;
      this.btnPastTime.Enabled = false;
      this.chbArhiv.Visible = false;
    }

    private void DisableControls()
    {
      this.tbCompany.Enabled = false;
      this.tbIdHome.Enabled = false;
      this.tbKorp.Enabled = false;
      this.tbNHome.Enabled = false;
      this.tbReu.Enabled = false;
      this.tbStreet.Enabled = false;
      this.tbYearBuild.Enabled = false;
      this.tbWall.Enabled = false;
      this.tbIdStr.Enabled = false;
      this.txbNumFloor.Enabled = false;
    }

    private bool SaveOptions()
    {
      try
      {
        this.home.Division = Convert.ToInt32(this.tbGroupHome.Text);
        this.session.Clear();
        this.session.Update((object) this.home);
        this.session.Flush();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Не могу сохранить запись!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        KvrplHelper.WriteLog(ex, (LsClient) null);
        return false;
      }
      return true;
    }

    private void btnCopy_Click(object sender, EventArgs e)
    {
      FrmChooseObject frmChooseObject = new FrmChooseObject(this.home);
      frmChooseObject.HomeSave = (short) 2;
      frmChooseObject.CodeOperation = (short) 1;
      int num = (int) frmChooseObject.ShowDialog();
      frmChooseObject.Dispose();
    }

    private void tbGroupHome_TextChanged(object sender, EventArgs e)
    {
      this.btnSave.Enabled = true;
    }

    public void LoadHomesPhones()
    {
      this.btnDelRecord.Enabled = true;
      this.btnAddRecord.Enabled = true;
      this.btnSave.Enabled = false;
      this.btnPastTime.Enabled = false;
      this.chbArhiv.Visible = true;
      string str = "";
      if (!this.chbArhiv.Checked)
        str = " and DEnd>='{2}'";
      this.InsertRecord = false;
      this.session.Clear();
      this.session = Domain.CurrentSession;
      this.cmsHomeParam.Items[2].Visible = false;
      IList<HomesPhones> homesPhonesList = this.session.CreateQuery(string.Format("from HomesPhones where Company.CompanyId={0} and Home.IdHome={1} and ClientId=0" + str + " order by DBeg desc,PhonesServ.Idservice,Receipt.ReceiptId", (object) this.company.CompanyId, (object) this.home.IdHome, (object) KvrplHelper.DateToBaseFormat(this.MonthClosed.PeriodName.Value.AddMonths(1)))).List<HomesPhones>();
      this.dgvHomesPhones.Columns.Clear();
      this.dgvHomesPhones.DataSource = (object) null;
      this.dgvHomesPhones.DataSource = (object) homesPhonesList;
      this.dgvHomesPhonesLoadComboBoxField();
    }

    private void dgvHomesPhonesLoadComboBoxField()
    {
      KvrplHelper.AddMaskDateColumn(this.dgvHomesPhones, 0, "Дата начала", "DBeg");
      KvrplHelper.AddMaskDateColumn(this.dgvHomesPhones, 1, "Дата окончания", "DEnd");
      IList<Di_PhonesServ> diPhonesServList = this.session.CreateCriteria(typeof (Di_PhonesServ)).Add((ICriterion) Restrictions.Eq("ViewService", (object) Convert.ToInt16(0))).AddOrder(Order.Asc("Nameservice")).List<Di_PhonesServ>();
      DataGridViewComboBoxCell viewComboBoxCell = new DataGridViewComboBoxCell();
      viewComboBoxCell.DisplayStyleForCurrentCellOnly = true;
      viewComboBoxCell.ValueMember = "Idservice";
      viewComboBoxCell.DisplayMember = "Nameservice";
      viewComboBoxCell.DataSource = (object) diPhonesServList;
      DataGridViewColumn dataGridViewColumn = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn.CellTemplate = (DataGridViewCell) viewComboBoxCell;
      dataGridViewColumn.HeaderText = "Служба";
      dataGridViewColumn.Name = "SName";
      this.dgvHomesPhones.Columns.Insert(2, dataGridViewColumn);
      KvrplHelper.AddComboBoxColumn(this.dgvHomesPhones, 3, (IList) this.session.CreateCriteria(typeof (Receipt)).Add((ICriterion) Restrictions.Not((ICriterion) Restrictions.Eq("ReceiptId", (object) Convert.ToInt16(0)))).AddOrder(Order.Asc("ReceiptId")).List<Receipt>(), "ReceiptId", "ReceiptName", "Квитанция", "Receipt", 10, 100);
      this.dgvHomesPhones.Columns["Phone"].HeaderText = "Телефон";
      this.dgvHomesPhones.Columns["Note"].HeaderText = "Примечания";
      this.dgvHomesPhones.Columns["Phone"].DisplayIndex = 4;
      this.dgvHomesPhones.Columns["Note"].DisplayIndex = 5;
      this.dgvHomesPhones.Columns["UName"].DisplayIndex = 6;
      this.dgvHomesPhones.Columns["DEdit"].DisplayIndex = 7;
      KvrplHelper.ViewEdit(this.dgvHomesPhones);
      foreach (DataGridViewRow row in (IEnumerable) this.dgvHomesPhones.Rows)
      {
        if (((HomesPhones) row.DataBoundItem).PhonesServ != null)
          row.Cells["SName"].Value = (object) ((HomesPhones) row.DataBoundItem).PhonesServ.Idservice;
        if (((HomesPhones) row.DataBoundItem).Receipt != null)
          row.Cells["Receipt"].Value = (object) ((HomesPhones) row.DataBoundItem).Receipt.ReceiptId;
        row.Cells["Phone"].Value = (object) ((HomesPhones) row.DataBoundItem).Phone;
        row.Cells["Note"].Value = (object) ((HomesPhones) row.DataBoundItem).Note;
        row.Cells["DBeg"].Value = (object) ((HomesPhones) row.DataBoundItem).DBeg;
        row.Cells["DEnd"].Value = (object) ((HomesPhones) row.DataBoundItem).DEnd;
      }
      if (this.dgvHomesPhones.Rows.Count > 0 && this.SelectedIndexHomesPhones == -1)
        this.SelectedIndexHomesPhones = 0;
      if (this.SelectedIndexHomesPhones == -1 || this.SelectedIndexHomesPhones >= this.dgvHomesPhones.Rows.Count)
        return;
      if (!this.InsertRecord)
      {
        this.dgvHomesPhones.Rows[this.SelectedIndexHomesPhones].Selected = true;
        this.dgvHomesPhones.CurrentCell = this.dgvHomesPhones.Rows[this.SelectedIndexHomesPhones].Cells[0];
      }
      else
      {
        this.dgvHomesPhones.Rows[this.dgvHomesPhones.Rows.Count - 1].Selected = true;
        this.dgvHomesPhones.CurrentCell = this.dgvHomesPhones.Rows[this.dgvHomesPhones.Rows.Count - 1].Cells[0];
        this.SelectedIndexHomesPhones = this.dgvHomesPhones.Rows.Count - 1;
      }
    }

    private void InsertHomesPhones()
    {
      HomesPhones homesPhones = new HomesPhones();
      homesPhones.Home = this.home;
      homesPhones.Company = this.session.Get<Company>((object) this.company.CompanyId);
      homesPhones.ClientId = 0;
      homesPhones.DBeg = Options.Period.PeriodName.Value;
      homesPhones.DEnd = Convert.ToDateTime("31.12.2999");
      homesPhones.Receipt = this.session.Get<Receipt>((object) Convert.ToInt16(1));
      int num = this.dgvHomesPhones.CurrentRow == null ? 0 : (((HomesPhones) this.dgvHomesPhones.Rows[this.dgvHomesPhones.CurrentRow.Index].DataBoundItem).PhonesServ != null ? 1 : 0);
      homesPhones.PhonesServ = num == 0 ? (Di_PhonesServ) null : this.session.Get<Di_PhonesServ>((object) ((HomesPhones) this.dgvHomesPhones.Rows[this.dgvHomesPhones.CurrentRow.Index].DataBoundItem).PhonesServ.Idservice);
      IList<HomesPhones> homesPhonesList = (IList<HomesPhones>) new List<HomesPhones>();
      if ((uint) this.dgvHomesPhones.Rows.Count > 0U)
        homesPhonesList = (IList<HomesPhones>) (this.dgvHomesPhones.DataSource as List<HomesPhones>);
      homesPhonesList.Add(homesPhones);
      this.dgvHomesPhones.Columns.Clear();
      this.dgvHomesPhones.DataSource = (object) null;
      this.dgvHomesPhones.DataSource = (object) homesPhonesList;
      this.InsertRecord = true;
      this.dgvHomesPhonesLoadComboBoxField();
    }

    private bool DelHomesPhones()
    {
      if (this.dgvHomesPhones.Rows.Count > 0 && this.dgvHomesPhones.CurrentRow.Index >= 0)
      {
        if (MessageBox.Show("Вы уверены, что хотите удалить запись", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
          return false;
        DataGridViewRow dataGridViewRow = new DataGridViewRow();
        DataGridViewRow row = this.dgvHomesPhones.Rows[this.dgvHomesPhones.CurrentRow.Index];
        HomesPhones homesPhones = new HomesPhones();
        HomesPhones dataBoundItem = (HomesPhones) row.DataBoundItem;
        int index = this.dgvHomesPhones.CurrentRow.Index;
        if (true)
        {
          this.session.Clear();
          this.session = Domain.CurrentSession;
          using (ITransaction transaction = this.session.BeginTransaction())
          {
            try
            {
              this.session.Delete((object) dataBoundItem);
              this.session.Flush();
              transaction.Commit();
            }
            catch (Exception ex)
            {
              int num = (int) MessageBox.Show("Не могу удалить текущую запись!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              KvrplHelper.WriteLog(ex, (LsClient) null);
              transaction.Rollback();
              return false;
            }
          }
          this.session.Clear();
        }
      }
      return true;
    }

    private bool SaveHomesPhones()
    {
      this.session.Clear();
      if (this.dgvHomesPhones.CurrentRow == null)
        return true;
      DataGridViewRow dataGridViewRow = new DataGridViewRow();
      DataGridViewRow row = this.dgvHomesPhones.Rows[this.dgvHomesPhones.CurrentRow.Index];
      HomesPhones homesPhones1 = new HomesPhones();
      HomesPhones dataBoundItem = (HomesPhones) row.DataBoundItem;
      if (row.Cells["SName"].Value != null)
      {
        dataBoundItem.PhonesServ = this.session.Get<Di_PhonesServ>(row.Cells["SName"].Value);
        try
        {
          dataBoundItem.DBeg = Convert.ToDateTime(row.Cells["DBeg"].Value);
          dataBoundItem.DEnd = Convert.ToDateTime(row.Cells["DEnd"].Value);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Некорректно введены даты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (dataBoundItem.DBeg > dataBoundItem.DEnd)
        {
          int num = (int) MessageBox.Show("Дата начала больше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        DateTime dbeg = dataBoundItem.DBeg;
        DateTime dateTime1 = KvrplHelper.GetCmpKvrClose(dataBoundItem.Company, 101, 0).PeriodName.Value;
        DateTime dateTime2 = dateTime1.AddMonths(1);
        int num1;
        if (dbeg < dateTime2)
        {
          DateTime dend = dataBoundItem.DEnd;
          dateTime1 = KvrplHelper.GetCmpKvrClose(dataBoundItem.Company, 101, 0).PeriodName.Value;
          dateTime1 = dateTime1.AddMonths(1);
          DateTime dateTime3 = dateTime1.AddDays(-1.0);
          num1 = dend < dateTime3 ? 1 : 0;
        }
        else
          num1 = 0;
        if (num1 != 0)
        {
          int num2 = (int) MessageBox.Show("Невозможно ввести запись в закрытый период", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (row.Cells["Receipt"].Value != null)
          dataBoundItem.Receipt = this.session.Get<Receipt>((object) Convert.ToInt16(row.Cells["Receipt"].Value));
        dataBoundItem.Phone = row.Cells["Phone"].Value != null ? (string) row.Cells["Phone"].Value : "";
        dataBoundItem.Note = row.Cells["Note"].Value != null ? (string) row.Cells["Note"].Value : "";
        dataBoundItem.UName = Options.Login;
        HomesPhones homesPhones2 = dataBoundItem;
        dateTime1 = DateTime.Now;
        DateTime date = dateTime1.Date;
        homesPhones2.DEdit = date;
        if (this.InsertRecord)
        {
          using (ITransaction transaction = this.session.BeginTransaction())
          {
            try
            {
              this.session.Save((object) dataBoundItem);
              this.session.Flush();
              transaction.Commit();
            }
            catch (Exception ex)
            {
              this.session.Clear();
              this.session = Domain.CurrentSession;
              int num2 = (int) MessageBox.Show("Не могу сохранить текущую запись", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              KvrplHelper.WriteLog(ex, (LsClient) null);
              transaction.Rollback();
              return true;
            }
          }
        }
        if (!this.InsertRecord && this.OldHomesPhones != null)
        {
          using (ITransaction transaction = this.session.BeginTransaction())
          {
            try
            {
              this.session.CreateSQLQuery("update hmReceipt hp set IdService = :idservice, Phone=:phone, Note=:note, DBeg=:dbeg, DEnd=:dend, UName=:uname, DEdit=:dedit where hp.Company_id=:codeu and hp.IdHome=:idhome and hp.Client_Id=0  and hp.Idservice=:oldidservice and hp.DBeg=:olddbeg").SetParameter<int>("idservice", dataBoundItem.PhonesServ.Idservice).SetParameter<string>("phone", dataBoundItem.Phone).SetParameter<string>("note", dataBoundItem.Note).SetParameter<DateTime>("dbeg", dataBoundItem.DBeg).SetParameter<DateTime>("dend", dataBoundItem.DEnd).SetParameter<string>("uname", dataBoundItem.UName).SetParameter<DateTime>("dedit", dataBoundItem.DEdit).SetParameter<short>("codeu", dataBoundItem.Company.CompanyId).SetParameter<int>("idhome", dataBoundItem.Home.IdHome).SetParameter<int>("oldidservice", this.OldHomesPhones.PhonesServ.Idservice).SetParameter<DateTime>("olddbeg", this.OldHomesPhones.DBeg).ExecuteUpdate();
              transaction.Commit();
            }
            catch
            {
              this.session.Clear();
              this.session = Domain.CurrentSession;
              int num2 = (int) MessageBox.Show("Не могу сохранить текущую запись", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              transaction.Rollback();
              return true;
            }
          }
        }
        this.InsertRecord = false;
        return true;
      }
      int num3 = (int) MessageBox.Show("Не выбрана служба!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private void dgvHomesPhones_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      if (this.dgvHomesPhones.CurrentRow == null)
        return;
      this.OldHomesPhones = new HomesPhones();
      this.OldHomesPhones.Company = ((HomesPhones) this.dgvHomesPhones.Rows[this.dgvHomesPhones.CurrentRow.Index].DataBoundItem).Company;
      this.OldHomesPhones.Home = ((HomesPhones) this.dgvHomesPhones.Rows[this.dgvHomesPhones.CurrentRow.Index].DataBoundItem).Home;
      this.OldHomesPhones.PhonesServ = ((HomesPhones) this.dgvHomesPhones.Rows[this.dgvHomesPhones.CurrentRow.Index].DataBoundItem).PhonesServ;
      this.OldHomesPhones.Phone = ((HomesPhones) this.dgvHomesPhones.Rows[this.dgvHomesPhones.CurrentRow.Index].DataBoundItem).Phone;
      this.OldHomesPhones.Note = ((HomesPhones) this.dgvHomesPhones.Rows[this.dgvHomesPhones.CurrentRow.Index].DataBoundItem).Note;
      this.OldHomesPhones.DBeg = ((HomesPhones) this.dgvHomesPhones.Rows[this.dgvHomesPhones.CurrentRow.Index].DataBoundItem).DBeg;
      this.OldHomesPhones.DEnd = ((HomesPhones) this.dgvHomesPhones.Rows[this.dgvHomesPhones.CurrentRow.Index].DataBoundItem).DEnd;
      this.btnSave.Enabled = true;
    }

    private void dgvHomesPhones_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.RowIndex == -1 || e.ColumnIndex == -1)
        return;
      this.dgvHomesPhones.Rows[e.RowIndex].Selected = true;
      this.dgvHomesPhones.CurrentCell = this.dgvHomesPhones.Rows[e.RowIndex].Cells[e.ColumnIndex];
      this.SelectedIndexHomesPhones = this.dgvHomesPhones.Rows[e.RowIndex].Index;
    }

    private void dgvHomesPhones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (this.dataGridView1.DataSource == null)
        return;
      DataGridViewRow row = (sender as DataGridView).Rows[e.RowIndex];
      DateTime dbeg = ((HomesPhones) row.DataBoundItem).DBeg;
      DateTime? periodName = this.NextMonthClosed.PeriodName;
      DateTime dateTime1 = KvrplHelper.LastDay(periodName.Value);
      int num;
      if (dbeg <= dateTime1)
      {
        DateTime dend = ((HomesPhones) row.DataBoundItem).DEnd;
        periodName = this.NextMonthClosed.PeriodName;
        DateTime dateTime2 = periodName.Value;
        num = dend >= dateTime2 ? 1 : 0;
      }
      else
        num = 0;
      if (num != 0)
      {
        row.DefaultCellStyle.BackColor = Color.PapayaWhip;
        row.DefaultCellStyle.ForeColor = Color.Black;
      }
      else
      {
        row.DefaultCellStyle.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
        row.DefaultCellStyle.ForeColor = Color.Gray;
      }
    }

    private void LoadDogovors()
    {
      this.btnDelRecord.Enabled = true;
      this.btnAddRecord.Enabled = true;
      this.btnSave.Enabled = false;
      this.btnPastTime.Enabled = false;
      this.chbArhiv.Visible = false;
      this.InsertRecord = false;
      this.session.Clear();
      ICriteria criteria = this.session.CreateCriteria(typeof (Dogovor)).Add((ICriterion) Restrictions.Eq("Home.IdHome", (object) this.home.IdHome)).AddOrder(Order.Desc("DBeg"));
      IList<Dogovor> dogovorList = criteria.List<Dogovor>();
      this.dgvDogovor.Columns.Clear();
      this.dgvDogovor.DataSource = (object) null;
      this.dgvDogovor.DataSource = (object) dogovorList;
      foreach (Dogovor dogovor in (List<Dogovor>) this.dgvDogovor.DataSource)
      {
        dogovor.IsEdit = false;
        dogovor.OldHashCode = dogovor.GetHashCode();
      }
      this.session.Clear();
      this.oldListDogovors = (IList<Dogovor>) new List<Dogovor>();
      this.oldListDogovors = criteria.List<Dogovor>();
      int index = 0;
      foreach (Dogovor dogovor in (List<Dogovor>) this.dgvDogovor.DataSource)
      {
        this.oldListDogovors[index].OldHashCode = dogovor.OldHashCode;
        ++index;
      }
      this.SetViewDogovors();
      if (this.dgvDogovor.Rows.Count <= 0)
        return;
      this.dgvDogovor.CurrentCell = this.dgvDogovor.Rows[0].Cells["DBeg"];
    }

    private void SetViewDogovors()
    {
      IList<BaseOrg> baseOrgList = this.session.CreateQuery("select new BaseOrg(b.BaseOrgId,b.NameOrgMin) from BaseOrg b order by b.NameOrgMin").List<BaseOrg>();
      KvrplHelper.AddMaskDateColumn(this.dgvDogovor, 0, "Дата начала", "DBeg");
      KvrplHelper.AddMaskDateColumn(this.dgvDogovor, 1, "Дата окончания", "DEnd");
      KvrplHelper.AddComboBoxColumn(this.dgvDogovor, 2, (IList) baseOrgList, "BaseOrgId", "NameOrgMin", "Организация", "Manager", 7, 400);
      this.dgvDogovor.Columns["DogovorNum"].HeaderText = "Номер договора";
      KvrplHelper.AddMaskDateColumn(this.dgvDogovor, 3, "Дата заключения", "DogovorDate");
      KvrplHelper.ViewEdit(this.dgvDogovor);
      this.dgvDogovor.Columns["IsEdit"].Visible = false;
      this.dgvDogovor.Columns["OldHashCode"].Visible = false;
      foreach (DataGridViewRow row in (IEnumerable) this.dgvDogovor.Rows)
      {
        row.Cells["DBeg"].Value = (object) ((Dogovor) row.DataBoundItem).DBeg;
        row.Cells["DEnd"].Value = (object) ((Dogovor) row.DataBoundItem).DEnd;
        row.Cells["DogovorDate"].Value = (object) ((Dogovor) row.DataBoundItem).DogovorDate;
        if (((Dogovor) row.DataBoundItem).Manager != null)
          row.Cells["Manager"].Value = (object) ((Dogovor) row.DataBoundItem).Manager.BaseOrgId;
      }
      this.MySettingsDogovor.GridName = "Dogovor";
      this.LoadSettingsDogovor();
    }

    private void InsertDogovor()
    {
      this.btnDelRecord.Enabled = false;
      this.btnSave.Enabled = true;
      Dogovor dogovor = new Dogovor();
      dogovor.Home = this.home;
      dogovor.DBeg = KvrplHelper.GetKvrClose(this.home, Options.ComplexPasp, Options.ComplexPrior, (int) this.company.CompanyId).PeriodName.Value.AddMonths(1);
      dogovor.DEnd = Convert.ToDateTime("2999-12-31");
      dogovor.DogovorDate = new DateTime?();
      dogovor.IsEdit = true;
      IList<Dogovor> dogovorList = (IList<Dogovor>) new List<Dogovor>();
      if (this.dgvDogovor.Rows.Count > 0)
        dogovorList = this.dgvDogovor.DataSource as IList<Dogovor>;
      dogovorList.Add(dogovor);
      this.dgvDogovor.Columns.Clear();
      this.dgvDogovor.DataSource = (object) null;
      this.dgvDogovor.DataSource = (object) dogovorList;
      this.SetViewDogovors();
      this.dgvDogovor.Focus();
      if (this.dgvDogovor.Rows.Count <= 0)
        return;
      this.dgvDogovor.CurrentCell = this.dgvDogovor.Rows[this.dgvDogovor.Rows.Count - 1].Cells["DBeg"];
    }

    private bool SaveAllDogovors()
    {
      bool flag = true;
      foreach (DataGridViewRow row in (IEnumerable) this.dgvDogovor.Rows)
      {
        this.dgvDogovor.Rows[row.Index].Selected = true;
        this.dgvDogovor.CurrentCell = row.Cells[0];
        Application.DoEvents();
        if (((Dogovor) row.DataBoundItem).IsEdit)
        {
          this.oldDogovor = new Dogovor();
          foreach (Dogovor oldListDogovor in (IEnumerable<Dogovor>) this.oldListDogovors)
          {
            if (oldListDogovor.OldHashCode == ((Dogovor) row.DataBoundItem).OldHashCode)
            {
              this.oldDogovor = oldListDogovor;
              break;
            }
          }
          if (!this.SaveDogovor())
            flag = false;
        }
        ((Dogovor) row.DataBoundItem).IsEdit = false;
      }
      this.btnAddRecord.Enabled = true;
      this.btnDelRecord.Enabled = true;
      this.session.Clear();
      return flag;
    }

    private bool SaveDogovor()
    {
      Dogovor dataBoundItem = (Dogovor) this.dgvDogovor.CurrentRow.DataBoundItem;
      this.InsertRecord = dataBoundItem.UName == null;
      try
      {
        dataBoundItem.DBeg = Convert.ToDateTime(this.dgvDogovor.CurrentRow.Cells["DBeg"].Value);
        dataBoundItem.DEnd = Convert.ToDateTime(this.dgvDogovor.CurrentRow.Cells["DEnd"].Value);
        if (this.dgvDogovor.CurrentRow.Cells["DogovorDate"].Value == null)
        {
          int num = (int) MessageBox.Show("Введите дату заключения договора", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
        dataBoundItem.DogovorDate = new DateTime?(Convert.ToDateTime(this.dgvDogovor.CurrentRow.Cells["DogovorDate"].Value));
      }
      catch
      {
        int num = (int) MessageBox.Show("Проверьте правильности введенных дат", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (dataBoundItem.DBeg > dataBoundItem.DEnd)
      {
        int num = (int) MessageBox.Show("Дата начала больше даты окончания", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      DateTime dbeg = dataBoundItem.DBeg;
      DateTime? dogovorDate = dataBoundItem.DogovorDate;
      if (dogovorDate.HasValue && dbeg < dogovorDate.GetValueOrDefault())
      {
        int num = (int) MessageBox.Show("Дата начала меньше даты заключения договора", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (this.dgvDogovor.CurrentRow.Cells["Manager"].Value != null)
      {
        dataBoundItem.Manager = this.session.Get<BaseOrg>((object) Convert.ToInt32(this.dgvDogovor.CurrentRow.Cells["Manager"].Value));
        if (dataBoundItem.DogovorNum == null || dataBoundItem.DogovorNum == "")
        {
          int num = (int) MessageBox.Show("Введите номер договора", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
        dataBoundItem.UName = Options.Login;
        dataBoundItem.DEdit = Convert.ToDateTime(DateTime.Now.ToShortDateString());
        try
        {
          if (this.InsertRecord)
          {
            this.session.Save((object) dataBoundItem);
            this.session.Flush();
          }
          else
          {
            IQuery query = this.session.CreateQuery("update Dogovor set DBeg=:dbeg,DEnd=:dend,Manager.BaseOrgId=:org,DogovorNum=:num,DogovorDate=:date,UName=:user,DEdit=:dedit where Home.IdHome=:home and DBeg=:oldbeg and Manager.BaseOrgId=:oldorg").SetDateTime("dbeg", dataBoundItem.DBeg).SetDateTime("dend", dataBoundItem.DEnd).SetParameter<int>("org", dataBoundItem.Manager.BaseOrgId).SetParameter<string>("num", dataBoundItem.DogovorNum);
            string name = "date";
            dogovorDate = dataBoundItem.DogovorDate;
            DateTime val = dogovorDate.Value;
            query.SetDateTime(name, val).SetParameter<string>("user", dataBoundItem.UName).SetDateTime("dedit", dataBoundItem.DEnd).SetDateTime("oldbeg", this.oldDogovor.DBeg).SetParameter<int>("oldorg", this.oldDogovor.Manager.BaseOrgId).SetParameter<int>("home", this.home.IdHome).ExecuteUpdate();
          }
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Не удалось сохранить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        return true;
      }
      int num1 = (int) MessageBox.Show("Организация не выбрана", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return false;
    }

    private void DeleteDogovor()
    {
      if (MessageBox.Show("Вы хотите удалить текущую запись?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      this.session.Clear();
      Dogovor dataBoundItem = (Dogovor) this.dgvDogovor.CurrentRow.DataBoundItem;
      try
      {
        this.session.Delete((object) dataBoundItem);
        this.session.Flush();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Не удалось удалить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        KvrplHelper.WriteLog(ex, (LsClient) null);
        return;
      }
      this.session.Clear();
      this.LoadDogovors();
    }

    private void dgvDogovor_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.btnSave.Enabled = true;
      this.btnDelRecord.Enabled = false;
    }

    private void dgvDogovor_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgvDogovor.CurrentRow == null)
        return;
      Dogovor dataBoundItem = (Dogovor) this.dgvDogovor.CurrentRow.DataBoundItem;
      dataBoundItem.IsEdit = true;
      if (this.dgvDogovor.CurrentCell.Value != null)
      {
        try
        {
          string name = this.dgvDogovor.Columns[e.ColumnIndex].Name;
          if (!(name == "DBeg"))
          {
            if (!(name == "DEnd"))
            {
              if (!(name == "DogovorDate"))
              {
                if (name == "Manager")
                  dataBoundItem.Manager = this.session.Get<BaseOrg>(this.dgvDogovor.CurrentRow.Cells["Manager"].Value);
              }
              else
                dataBoundItem.DogovorDate = new DateTime?(Convert.ToDateTime(this.dgvDogovor.CurrentRow.Cells["DogovorDate"].Value));
            }
            else
              dataBoundItem.DEnd = Convert.ToDateTime(this.dgvDogovor.CurrentRow.Cells["DEnd"].Value);
          }
          else
            dataBoundItem.DBeg = Convert.ToDateTime(this.dgvDogovor.CurrentRow.Cells["DBeg"].Value);
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
    }

    private void LoadSettingsDogovor()
    {
      this.MySettingsDogovor.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvDogovor.Columns)
        this.MySettingsDogovor.GetMySettings(column);
    }

    private void dgvDogovor_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsDogovor.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsDogovor.Columns[this.MySettingsDogovor.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsDogovor.Save();
    }

    private void dgvDogovor_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (this.dgvDogovor.DataSource == null)
        return;
      DataGridViewRow row = (sender as DataGridView).Rows[e.RowIndex];
      DateTime dbeg = ((Dogovor) row.DataBoundItem).DBeg;
      DateTime? periodName = this.NextMonthClosed.PeriodName;
      DateTime dateTime1 = KvrplHelper.LastDay(periodName.Value);
      int num;
      if (dbeg <= dateTime1)
      {
        DateTime dend = ((Dogovor) row.DataBoundItem).DEnd;
        periodName = this.NextMonthClosed.PeriodName;
        DateTime dateTime2 = periodName.Value;
        num = dend >= dateTime2 ? 1 : 0;
      }
      else
        num = 0;
      if (num != 0)
      {
        row.DefaultCellStyle.BackColor = Color.PapayaWhip;
        row.DefaultCellStyle.ForeColor = Color.Black;
      }
      else
      {
        row.DefaultCellStyle.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
        row.DefaultCellStyle.ForeColor = Color.Gray;
      }
    }

    private void LoadParam()
    {
      this.btnDelRecord.Enabled = true;
      this.btnAddRecord.Enabled = true;
      this.btnSave.Enabled = false;
      this.btnPastTime.Enabled = true;
      this.chbArhiv.Visible = true;
      this.cmsHomeParam.Items[2].Visible = true;
      this.InsertRecord = false;
      this.session.Clear();
      this.session = Domain.CurrentSession;
      IQuery query;
      if (!this.PastTime)
      {
        if (this.Arhiv)
          query = this.session.CreateQuery(string.Format("select h from HomeParam h, Param p where h.Param.ParamId=p.ParamId and  h.Home.IdHome={0} and h.Company.CompanyId={1} and h.Period.PeriodId={2} and (p.Param_type={3} or p.Param_type={4}) order by h.Param.ParamId, h.DBeg ", (object) this.home.IdHome, (object) this.company.CompanyId, (object) 0, (object) (short) 2, (object) (short) 0));
        else
          query = this.session.CreateQuery(string.Format("select h from HomeParam h, Param p where h.Param.ParamId=p.ParamId and  h.Home.IdHome={0} and h.Company.CompanyId={1} and h.Period.PeriodId={2} and (p.Param_type={3} or p.Param_type={4}) and h.DEnd >= '{5}'  order by h.Param.ParamId, h.DBeg ", (object) this.home.IdHome, (object) this.company.CompanyId, (object) 0, (object) (short) 2, (object) (short) 0, (object) KvrplHelper.DateToBaseFormat(this.NextMonthClosed.PeriodName.Value)));
      }
      else if (this.Arhiv)
        query = this.session.CreateQuery(string.Format("select h from HomeParam h, Param p where h.Param.ParamId=p.ParamId and  h.Home.IdHome={0} and h.Company.CompanyId={1} and h.Period.PeriodId!=0 and (p.Param_type={3} or p.Param_type={4}) order by h.Param.ParamId, h.Period.PeriodName ", (object) this.home.IdHome, (object) this.company.CompanyId, (object) this.CurrentPeriod.PeriodId, (object) (short) 2, (object) (short) 0));
      else
        query = this.session.CreateQuery(string.Format("select h from HomeParam h, Param p where h.Param.ParamId=p.ParamId and  h.Home.IdHome={0} and h.Company.CompanyId={1} and h.Period.PeriodId={2} and (p.Param_type={3} or p.Param_type={4}) order by h.Param.ParamId, h.DBeg ", (object) this.home.IdHome, (object) this.company.CompanyId, (object) this.CurrentPeriod.PeriodId, (object) (short) 2, (object) (short) 0));
      IList<HomeParam> homeParamList = query.List<HomeParam>();
      this.dataGridView1.Columns.Clear();
      this.dataGridView1.DataSource = (object) null;
      this.dataGridView1.DataSource = (object) homeParamList;
      this.dgvParamLoadComboBoxField();
      this.MySettingsHomeParam.GridName = "HomeParam";
      this.LoadSettings();
    }

    private void dgvParamLoadComboBoxField()
    {
      try
      {
        KvrplHelper.AddMaskDateColumn(this.dataGridView1, 0, "Дата начала", "MDBeg");
        KvrplHelper.AddMaskDateColumn(this.dataGridView1, 1, "Дата окончания", "MDEnd");
        IList<Param> objList = this.session.CreateCriteria(typeof (Param)).Add((ICriterion) Restrictions.Or((ICriterion) Restrictions.Eq("Param_type", (object) (short) 2), (ICriterion) Restrictions.Eq("Param_type", (object) (short) 0))).List<Param>();
        DataGridViewComboBoxCell viewComboBoxCell1 = new DataGridViewComboBoxCell();
        viewComboBoxCell1.DisplayStyleForCurrentCellOnly = true;
        viewComboBoxCell1.ValueMember = "ParamId";
        viewComboBoxCell1.DisplayMember = "ParamName";
        viewComboBoxCell1.DataSource = (object) objList;
        DataGridViewColumn dataGridViewColumn1 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
        dataGridViewColumn1.CellTemplate = (DataGridViewCell) viewComboBoxCell1;
        dataGridViewColumn1.HeaderText = "Наименование";
        dataGridViewColumn1.Name = "PName";
        this.dataGridView1.Columns.Insert(2, dataGridViewColumn1);
        DataGridViewTextBoxCell gridViewTextBoxCell = new DataGridViewTextBoxCell();
        DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
        dataGridViewColumn2.HeaderText = "Значение";
        dataGridViewColumn2.Name = "PValue";
        dataGridViewColumn2.ReadOnly = false;
        this.dataGridView1.Columns.Insert(3, dataGridViewColumn2);
        KvrplHelper.ViewEdit(this.dataGridView1);
        if (this.PastTime)
          KvrplHelper.AddTextBoxColumn(this.dataGridView1, 0, "Период", "Period", 100, true);
        this.progressBar1.Visible = true;
        this.progressBar1.Value = 0;
        this.progressBar1.Step = 1;
        this.progressBar1.Minimum = 0;
        this.progressBar1.Maximum = this.dataGridView1.Rows.Count;
        foreach (DataGridViewRow row in (IEnumerable) this.dataGridView1.Rows)
        {
          DateTime dateTime;
          if (this.PastTime)
          {
            DataGridViewCell cell = row.Cells["Period"];
            dateTime = ((HomeParam) row.DataBoundItem).Period.PeriodName.Value;
            string shortDateString = dateTime.ToShortDateString();
            cell.Value = (object) shortDateString;
          }
          DataGridViewCell cell1 = row.Cells["MDBeg"];
          dateTime = ((HomeParam) row.DataBoundItem).DBeg;
          string shortDateString1 = dateTime.ToShortDateString();
          cell1.Value = (object) shortDateString1;
          DataGridViewCell cell2 = row.Cells["MDEnd"];
          dateTime = ((HomeParam) row.DataBoundItem).DEnd;
          string shortDateString2 = dateTime.ToShortDateString();
          cell2.Value = (object) shortDateString2;
          this.progressBar1.Value = this.progressBar1.Value + 1;
          if (((HomeParam) row.DataBoundItem).Param != null)
          {
            row.Cells["PName"].Value = (object) ((HomeParam) row.DataBoundItem).Param.ParamId;
            IList<AdmTbl> admTblList = this.session.CreateQuery(string.Format("select s from AdmTbl s,ParamRelation r where s.TableId=r.TableId and r.ParamId={0}", (object) ((HomeParam) row.DataBoundItem).Param.ParamId)).List<AdmTbl>();
            if (admTblList.Count > 0)
            {
              if (admTblList[0].ClassName != null)
              {
                try
                {
                  DataGridViewComboBoxCell viewComboBoxCell2 = new DataGridViewComboBoxCell();
                  viewComboBoxCell2.DisplayStyleForCurrentCellOnly = true;
                  viewComboBoxCell2.ValueMember = admTblList[0].ClassNameId;
                  viewComboBoxCell2.DisplayMember = admTblList[0].ClassNameName;
                  string str = "";
                  if ((int) ((HomeParam) row.DataBoundItem).Param.ParamId == 304)
                    str = " where SchemeType=11";
                  if ((int) ((HomeParam) row.DataBoundItem).Param.ParamId == 327 || (int) ((HomeParam) row.DataBoundItem).Param.ParamId == 328)
                    str = " where SchemeType=18";
                  viewComboBoxCell2.DataSource = (object) this.session.CreateQuery(string.Format("from {0}" + str, (object) admTblList[0].ClassName)).List();
                  viewComboBoxCell2.ValueType = typeof (short);
                  row.Cells["PValue"] = (DataGridViewCell) viewComboBoxCell2;
                }
                catch
                {
                }
              }
              int num = (int) ((HomeParam) row.DataBoundItem).Param.ParamId == 305 ? 1 : ((int) ((HomeParam) row.DataBoundItem).Param.ParamId == 309 ? 1 : 0);
              row.Cells["PValue"].Value = num == 0 ? (object) (short) ((HomeParam) row.DataBoundItem).ParamValue : ((int) ((HomeParam) row.DataBoundItem).Param.ParamId != 309 ? (object) (int) ((HomeParam) row.DataBoundItem).ParamValue : (object) (int) ((HomeParam) row.DataBoundItem).ParamValue);
            }
            else
              row.Cells["PValue"].Value = (object) ((HomeParam) row.DataBoundItem).ParamValue;
            if ((int) ((HomeParam) row.DataBoundItem).Param.ParamId == 312)
            {
              IList<CauseOut> causeOutList = this.session.CreateQuery("select c from CauseOut c ").List<CauseOut>();
              row.Cells["PValue"] = (DataGridViewCell) new DataGridViewComboBoxCell()
              {
                DisplayStyleForCurrentCellOnly = true,
                DataSource = (object) causeOutList,
                ValueMember = "CauseOut_id",
                DisplayMember = "CauseOut_name"
              };
              row.Cells["PValue"].Value = (object) (int) ((HomeParam) row.DataBoundItem).ParamValue;
            }
          }
        }
        this.progressBar1.Visible = false;
        this.SetNameGridColumnsHeader();
        if (this.dataGridView1.Rows.Count > 0 && this.SelectedIndexHomeParam == -1)
          this.SelectedIndexHomeParam = 0;
        if (this.SelectedIndexHomeParam == -1 || this.SelectedIndexHomeParam >= this.dataGridView1.Rows.Count)
          return;
        if (!this.InsertRecord)
        {
          this.dataGridView1.Rows[this.SelectedIndexHomeParam].Selected = true;
          this.dataGridView1.CurrentCell = this.dataGridView1.Rows[this.SelectedIndexHomeParam].Cells[0];
        }
        else
        {
          this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Selected = true;
          this.dataGridView1.CurrentCell = this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[0];
          this.SelectedIndexHomeParam = this.dataGridView1.Rows.Count - 1;
        }
      }
      catch (Exception ex)
      {
        FrmHomeParam.log.Error<Exception>(ex);
        FrmHomeParam.log.Error(ex.Message);
        FrmHomeParam.log.Error<Exception>(ex.InnerException);
      }
    }

    private bool SaveParam()
    {
      this.session.Clear();
      if (this.dataGridView1.CurrentRow == null)
        return true;
      DataGridViewRow dataGridViewRow = new DataGridViewRow();
      DataGridViewRow row = this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index];
      HomeParam homeParam = new HomeParam();
      HomeParam dataBoundItem = (HomeParam) row.DataBoundItem;
      if (row.Cells["PName"].Value == null)
      {
        int num = (int) MessageBox.Show("Не выбран параметр!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      dataBoundItem.Uname = Options.Login;
      dataBoundItem.Dedit = DateTime.Now.Date;
      int val = 0;
      if (dataBoundItem.Param != null)
        val = (int) dataBoundItem.Param.ParamId;
      dataBoundItem.Param = this.session.Get<Param>((object) Convert.ToInt16(row.Cells["PName"].Value));
      try
      {
        dataBoundItem.ParamValue = Convert.ToDouble(KvrplHelper.ChangeSeparator(row.Cells["PValue"].Value.ToString()));
      }
      catch
      {
        int num = (int) MessageBox.Show("Некорректный формат значения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      try
      {
        dataBoundItem.DBeg = Convert.ToDateTime(row.Cells["MDBeg"].Value);
      }
      catch
      {
        int num = (int) MessageBox.Show("Некорректный формат даты!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      try
      {
        dataBoundItem.DEnd = Convert.ToDateTime(row.Cells["MDEnd"].Value);
      }
      catch
      {
        int num = (int) MessageBox.Show("Некорректный формат даты!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if ((int) dataBoundItem.Param.ParamId == 302 || (int) dataBoundItem.Param.ParamId == 303 || (int) dataBoundItem.Param.ParamId == 327 || (int) dataBoundItem.Param.ParamId == 328)
      {
        dataBoundItem.DBeg = KvrplHelper.FirstDay(dataBoundItem.DBeg);
        dataBoundItem.DEnd = KvrplHelper.LastDay(dataBoundItem.DEnd);
      }
      if ((int) dataBoundItem.Param.ParamId == 310)
      {
        if (dataBoundItem.ParamValue.ToString().Length != 4)
        {
          int num1 = (int) MessageBox.Show("Для данного параметра значение должно быть 4 символа длинной!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        try
        {
          Convert.ToInt32(dataBoundItem.ParamValue);
        }
        catch
        {
          int num2 = (int) MessageBox.Show("Для данного параметра значение должно быть числом!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
      dataBoundItem.Period = !this.PastTime ? this.session.Get<Period>((object) 0) : this.CurrentPeriod;
      if (dataBoundItem.DBeg > dataBoundItem.DEnd)
      {
        int num = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. дата начала больше даты окончания", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.session.Clear();
        this.session = Domain.CurrentSession;
        return !this.InsertRecord;
      }
      if (!this.PastTime)
      {
        if (!this.InsertRecord && dataBoundItem.DBeg <= this.LastDayMonthClosed && dataBoundItem.DBeg != this.OldHomeParam.DBeg)
        {
          int num = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.session.Clear();
          this.session = Domain.CurrentSession;
          return true;
        }
        if (!this.InsertRecord && (this.OldHomeParam.DBeg < this.LastDayMonthClosed && this.OldHomeParam.DEnd < this.LastDayMonthClosed))
        {
          int num = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.session.Clear();
          this.session = Domain.CurrentSession;
          return true;
        }
        if (!this.InsertRecord && (this.OldHomeParam.DBeg < this.LastDayMonthClosed && (this.OldHomeParam.DBeg != dataBoundItem.DBeg || (int) this.OldHomeParam.Param.ParamId != (int) dataBoundItem.Param.ParamId || this.OldHomeParam.ParamValue != dataBoundItem.ParamValue) || this.OldHomeParam.DEnd < this.LastDayMonthClosed && (this.OldHomeParam.DEnd != dataBoundItem.DEnd || (int) this.OldHomeParam.Param.ParamId != (int) dataBoundItem.Param.ParamId || this.OldHomeParam.ParamValue != dataBoundItem.ParamValue)))
        {
          int num = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.session.Clear();
          this.session = Domain.CurrentSession;
          return true;
        }
        if (dataBoundItem.DEnd < this.LastDayMonthClosed)
        {
          int num = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. дата окончания принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.session.Clear();
          this.session = Domain.CurrentSession;
          return !this.InsertRecord;
        }
        int num1;
        if (this.InsertRecord)
        {
          DateTime dbeg = dataBoundItem.DBeg;
          DateTime? periodName = this.NextMonthClosed.PeriodName;
          num1 = periodName.HasValue ? (dbeg < periodName.GetValueOrDefault() ? 1 : 0) : 0;
        }
        else
          num1 = 0;
        if (num1 != 0)
        {
          int num2 = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.session.Clear();
          this.session = Domain.CurrentSession;
          return false;
        }
      }
      else
      {
        if ((int) dataBoundItem.Param.ParamId == 327 || (int) dataBoundItem.Param.ParamId == 328)
        {
          int num = (int) MessageBox.Show("Этот параметр нельзя менять в прошлом времени!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.session.Clear();
          this.session = Domain.CurrentSession;
          return false;
        }
        if (dataBoundItem.DBeg > KvrplHelper.GetLastDayPeriod(this.MonthClosed.PeriodName.Value) || dataBoundItem.DEnd > KvrplHelper.GetLastDayPeriod(this.MonthClosed.PeriodName.Value))
        {
          int num = (int) MessageBox.Show("Не могу сохранить текущую запись! Проверьте корректность введённых дат.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.session.Clear();
          this.session = Domain.CurrentSession;
          return false;
        }
        DateTime? periodName = this.CurrentPeriod.PeriodName;
        DateTime dateTime = periodName.Value;
        periodName = this.MonthClosed.PeriodName;
        DateTime lastDayPeriod = KvrplHelper.GetLastDayPeriod(periodName.Value);
        if (dateTime < lastDayPeriod)
        {
          int num = (int) MessageBox.Show("Не могу сохранить текущую запись! Проверьте корректность введённых дат.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.session.Clear();
          this.session = Domain.CurrentSession;
          return false;
        }
        periodName = this.MonthClosed.PeriodName;
        if (KvrplHelper.GetLastDayPeriod(periodName.Value) - dataBoundItem.DBeg > new TimeSpan(730, 0, 0, 0) && MessageBox.Show("Дата начала отличается от даты закрытого периода более, чем на 2 года. Продолжить сохранение? ", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        {
          this.session.Clear();
          this.session = Domain.CurrentSession;
          return false;
        }
      }
      bool flag = false;
      if ((int) dataBoundItem.Param.ParamId == 309)
      {
        if (!KvrplHelper.CheckProxy(84, 2, this.home.Company, true))
          return false;
        flag = true;
      }
      if (!flag && !KvrplHelper.CheckProxy(39, 2, this.home.Company, true))
        return false;
      if (this.InsertRecord)
      {
        using (ITransaction transaction = this.session.BeginTransaction())
        {
          try
          {
            this.session.Save((object) dataBoundItem);
            this.session.Flush();
            transaction.Commit();
          }
          catch (Exception ex)
          {
            this.session.Clear();
            this.session = Domain.CurrentSession;
            int num = (int) MessageBox.Show("Не могу сохранить текущую запись", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            KvrplHelper.WriteLog(ex, (LsClient) null);
            transaction.Rollback();
            return true;
          }
        }
      }
      if (this.OldHomeParam != null && !this.InsertRecord)
      {
        using (ITransaction transaction = this.session.BeginTransaction())
        {
          try
          {
            IQuery query1 = this.session.CreateSQLQuery("update hmParam cmp set DBeg= :dbeg, Param_Id= :paramid, DEnd= :dend, Param_Value= :paramvalue,  Uname=:uname,  Dedit=:dedit  where cmp.Company_Id= :companyid and cmp.Period_Id= :periodid  and cmp.Dbeg = :olddbeg and cmp.Param_Id = :oldparamid and cmp.Idhome= :idhome").SetParameter<string>("uname", dataBoundItem.Uname).SetParameter<DateTime>("dedit", dataBoundItem.Dedit);
            string name1 = "dbeg";
            DateTime dateTime = dataBoundItem.DBeg;
            DateTime date1 = dateTime.Date;
            IQuery query2 = query1.SetParameter<DateTime>(name1, date1).SetParameter<short>("paramid", dataBoundItem.Param.ParamId);
            string name2 = "dend";
            dateTime = dataBoundItem.DEnd;
            DateTime date2 = dateTime.Date;
            IQuery query3 = query2.SetParameter<DateTime>(name2, date2).SetParameter<double>("paramvalue", dataBoundItem.ParamValue).SetParameter<short>("companyid", dataBoundItem.Company.CompanyId).SetParameter<int>("periodid", dataBoundItem.Period.PeriodId);
            string name3 = "olddbeg";
            dateTime = this.OldHomeParam.DBeg;
            DateTime date3 = dateTime.Date;
            query3.SetParameter<DateTime>(name3, date3).SetParameter<int>("oldparamid", val).SetParameter<int>("idhome", this.home.IdHome).ExecuteUpdate();
            transaction.Commit();
          }
          catch
          {
            this.session.Clear();
            this.session = Domain.CurrentSession;
            int num = (int) MessageBox.Show("Не могу сохранить текущую запись", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            transaction.Rollback();
            return true;
          }
        }
      }
      this.InsertRecord = false;
      return true;
    }

    private void InsertParam()
    {
      HomeParam homeParam1 = new HomeParam();
      homeParam1.Home = this.home;
      homeParam1.Company = this.session.Get<Company>((object) this.company.CompanyId);
      homeParam1.Period = this.session.Get<Period>((object) 0);
      if (this.PastTime)
      {
        HomeParam homeParam2 = homeParam1;
        DateTime? periodName = this.MonthClosed.PeriodName;
        DateTime dateTime = periodName.Value;
        homeParam2.DBeg = dateTime;
        HomeParam homeParam3 = homeParam1;
        periodName = this.MonthClosed.PeriodName;
        DateTime lastDayPeriod = KvrplHelper.GetLastDayPeriod(periodName.Value);
        homeParam3.DEnd = lastDayPeriod;
      }
      else
      {
        homeParam1.DBeg = !(this.CurrentPeriod.PeriodName.Value <= this.MonthClosed.PeriodName.Value) ? this.CurrentPeriod.PeriodName.Value : this.NextMonthClosed.PeriodName.Value.Date;
        homeParam1.DEnd = Convert.ToDateTime("31.12.2999");
      }
      if (this.PastTime)
        homeParam1.Period = this.CurrentPeriod;
      int num = this.dataGridView1.CurrentRow == null ? 0 : (((HomeParam) this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].DataBoundItem).Param != null ? 1 : 0);
      homeParam1.Param = num == 0 ? (Param) null : this.session.Get<Param>((object) ((HomeParam) this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].DataBoundItem).Param.ParamId);
      IList<HomeParam> homeParamList = (IList<HomeParam>) new List<HomeParam>();
      if ((uint) this.dataGridView1.Rows.Count > 0U)
        homeParamList = (IList<HomeParam>) (this.dataGridView1.DataSource as List<HomeParam>);
      homeParamList.Add(homeParam1);
      this.dataGridView1.Columns.Clear();
      this.dataGridView1.DataSource = (object) null;
      this.dataGridView1.DataSource = (object) homeParamList;
      this.InsertRecord = true;
      this.dgvParamLoadComboBoxField();
      this.LoadSettings();
    }

    private bool DelParam()
    {
      if (this.dataGridView1.Rows.Count > 0 && this.dataGridView1.CurrentRow.Index >= 0)
      {
        if (MessageBox.Show("Вы уверены, что хотите удалить запись", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
          return false;
        DataGridViewRow dataGridViewRow = new DataGridViewRow();
        DataGridViewRow row1 = this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index];
        HomeParam homeParam = new HomeParam();
        HomeParam dataBoundItem = (HomeParam) row1.DataBoundItem;
        int index = this.dataGridView1.CurrentRow.Index;
        bool flag = true;
        foreach (DataGridViewRow row2 in (IEnumerable) this.dataGridView1.Rows)
        {
          if (!this.PastTime)
          {
            if (row2.Index != index && dataBoundItem.DBeg == Convert.ToDateTime(row2.Cells["MDBeg"].Value) && (int) dataBoundItem.Param.ParamId == (int) Convert.ToInt16(row2.Cells["PName"].Value))
            {
              flag = false;
              break;
            }
          }
          else if (row2.Index != index && dataBoundItem.DBeg == Convert.ToDateTime(row2.Cells["MDBeg"].Value) && (int) dataBoundItem.Param.ParamId == (int) Convert.ToInt16(row2.Cells["PName"].Value) && dataBoundItem.Period.PeriodName.Value == Convert.ToDateTime(row2.Cells["Period"].Value))
          {
            flag = false;
            break;
          }
        }
        if (flag)
        {
          if (!this.PastTime)
          {
            if (dataBoundItem.DBeg <= this.LastDayMonthClosed || dataBoundItem.DEnd <= this.LastDayMonthClosed)
            {
              int num = (int) MessageBox.Show("Не могу удалить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.session.Clear();
              this.session = Domain.CurrentSession;
              return false;
            }
          }
          else if (Convert.ToDateTime(row1.Cells["Period"].Value) <= this.MonthClosed.PeriodName.Value)
          {
            int num = (int) MessageBox.Show("Не могу удалить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.session.Clear();
            this.session = Domain.CurrentSession;
            return false;
          }
          this.session.Clear();
          this.session = Domain.CurrentSession;
          using (ITransaction transaction = this.session.BeginTransaction())
          {
            try
            {
              this.session.Delete((object) dataBoundItem);
              this.session.Flush();
              transaction.Commit();
            }
            catch (Exception ex)
            {
              int num = (int) MessageBox.Show("Не могу удалить текущую запись!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              KvrplHelper.WriteLog(ex, (LsClient) null);
              transaction.Rollback();
              return false;
            }
          }
          this.session.Clear();
        }
      }
      return true;
    }

    private void SetNameGridColumnsHeader()
    {
      if ((uint) this.dataGridView1.Columns.Count <= 0U)
        return;
      if (!this.PastTime)
      {
        this.dataGridView1.Columns[0].HeaderText = "Дата начала";
        this.dataGridView1.Columns[1].HeaderText = "Дата окончания";
        this.dataGridView1.Columns[2].HeaderText = "Наименование";
      }
      else
      {
        this.dataGridView1.Columns[0].HeaderText = "Период";
        this.dataGridView1.Columns[1].HeaderText = "Дата начала";
        this.dataGridView1.Columns[2].HeaderText = "Дата окончания";
        this.dataGridView1.Columns[3].HeaderText = "Наименование";
      }
    }

    public void LoadSettings()
    {
      this.MySettingsHomeParam.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dataGridView1.Columns)
        this.MySettingsHomeParam.GetMySettings(column);
    }

    private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (!this.dataGridView1.IsCurrentCellDirty)
        return;
      this.dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
      if (this.dataGridView1.CurrentCell.ColumnIndex == this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["PName"].ColumnIndex)
      {
        if ((int) (short) this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["PName"].Value == 312)
        {
          IList<CauseOut> causeOutList = this.session.CreateQuery("select c from CauseOut c ").List<CauseOut>();
          this.dataGridView1.CurrentRow.Cells["PValue"] = (DataGridViewCell) new DataGridViewComboBoxCell()
          {
            DisplayStyleForCurrentCellOnly = true,
            DataSource = (object) causeOutList,
            ValueMember = "CauseOut_id",
            DisplayMember = "CauseOut_name"
          };
        }
        else
        {
          IList<AdmTbl> admTblList = this.session.CreateQuery(string.Format("select s from AdmTbl s,ParamRelation r where s.TableId=r.TableId and r.ParamId={0}", this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["PName"].Value)).List<AdmTbl>();
          if (admTblList.Count > 0)
          {
            if (admTblList[0].ClassName != null)
            {
              try
              {
                DataGridViewComboBoxCell viewComboBoxCell = new DataGridViewComboBoxCell();
                viewComboBoxCell.DisplayStyleForCurrentCellOnly = true;
                viewComboBoxCell.ValueMember = admTblList[0].ClassNameId;
                viewComboBoxCell.DisplayMember = admTblList[0].ClassNameName;
                string str = "";
                if (Convert.ToInt32(this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["PName"].Value) == 304)
                  str = " where SchemeType=11";
                if (Convert.ToInt32(this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["PName"].Value) == 327 || Convert.ToInt32(this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["PName"].Value) == 328)
                  str = " where SchemeType=18";
                viewComboBoxCell.DataSource = (object) this.session.CreateQuery(string.Format("from {0}" + str, (object) admTblList[0].ClassName)).List();
                viewComboBoxCell.ValueType = typeof (short);
                if ((int) (short) this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["PName"].Value == 309)
                  this.ohlAccountsList = this.session.CreateQuery(string.Format("from {0}" + str, (object) admTblList[0].ClassName)).List<ohlAccounts>();
                this.dataGridView1.CurrentRow.Cells["PValue"] = (DataGridViewCell) viewComboBoxCell;
              }
              catch (Exception ex)
              {
                KvrplHelper.WriteLog(ex, (LsClient) null);
              }
            }
          }
          else
            this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["PValue"] = (DataGridViewCell) new DataGridViewTextBoxCell();
        }
      }
    }

    private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (this.dataGridView1.DataSource == null)
        return;
      DataGridViewRow row = (sender as DataGridView).Rows[e.RowIndex];
      DateTime dbeg = ((HomeParam) row.DataBoundItem).DBeg;
      DateTime? periodName = this.NextMonthClosed.PeriodName;
      DateTime dateTime1 = KvrplHelper.LastDay(periodName.Value);
      int num;
      if (dbeg <= dateTime1)
      {
        DateTime dend = ((HomeParam) row.DataBoundItem).DEnd;
        periodName = this.NextMonthClosed.PeriodName;
        DateTime dateTime2 = periodName.Value;
        num = dend >= dateTime2 ? 1 : 0;
      }
      else
        num = 0;
      if (num != 0)
      {
        row.DefaultCellStyle.BackColor = Color.PapayaWhip;
        row.DefaultCellStyle.ForeColor = Color.Black;
      }
      else
      {
        row.DefaultCellStyle.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
        row.DefaultCellStyle.ForeColor = Color.Gray;
      }
    }

    private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.RowIndex == -1 || e.ColumnIndex == -1)
        return;
      this.dataGridView1.Rows[e.RowIndex].Selected = true;
      this.dataGridView1.CurrentCell = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
      this.SelectedIndexHomeParam = this.dataGridView1.Rows[e.RowIndex].Index;
    }

    private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      if (this.dataGridView1.CurrentRow == null)
        return;
      this.OldHomeParam = new HomeParam();
      this.OldHomeParam.Company = ((HomeParam) this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].DataBoundItem).Company;
      this.OldHomeParam.Home = ((HomeParam) this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].DataBoundItem).Home;
      this.OldHomeParam.DBeg = ((HomeParam) this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].DataBoundItem).DBeg;
      this.OldHomeParam.DEnd = ((HomeParam) this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].DataBoundItem).DEnd;
      this.OldHomeParam.Param = ((HomeParam) this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].DataBoundItem).Param;
      this.OldHomeParam.Period = ((HomeParam) this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].DataBoundItem).Period;
      try
      {
        this.OldHomeParam.ParamValue = Convert.ToDouble(this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["PValue"].Value);
      }
      catch
      {
        this.OldHomeParam.ParamValue = 0.0;
        this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["PValue"].Value = (object) 0;
      }
      this.btnSave.Enabled = true;
    }

    private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsHomeParam.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsHomeParam.Columns[this.MySettingsHomeParam.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsHomeParam.Save();
    }

    private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
    {
    }

    private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex <= 0 || e.RowIndex < 0 || !(this.dataGridView1.Columns[e.ColumnIndex].Name == "PValue") || (int) (short) this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["PName"].Value != 309)
        return;
      this.btnSave.Enabled = true;
      this.OldHomeParam = new HomeParam();
      this.OldHomeParam.Company = ((HomeParam) this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].DataBoundItem).Company;
      this.OldHomeParam.Home = ((HomeParam) this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].DataBoundItem).Home;
      this.OldHomeParam.DBeg = ((HomeParam) this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].DataBoundItem).DBeg;
      this.OldHomeParam.DEnd = ((HomeParam) this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].DataBoundItem).DEnd;
      this.OldHomeParam.Param = ((HomeParam) this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].DataBoundItem).Param;
      this.OldHomeParam.Period = ((HomeParam) this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].DataBoundItem).Period;
      try
      {
        this.OldHomeParam.ParamValue = Convert.ToDouble(this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["PValue"].Value);
      }
      catch
      {
        this.OldHomeParam.ParamValue = 0.0;
        this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["PValue"].Value = (object) 0;
      }
      this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["PValue"].ReadOnly = true;
      FrmSprAccounts frmSprAccounts = new FrmSprAccounts(true, this.company, 113);
      int num = (int) frmSprAccounts.ShowDialog();
      this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["PValue"].Value = (object) frmSprAccounts.AccountSelect;
      frmSprAccounts.Dispose();
    }

    private void butLoadHomeDoc_Click(object sender, EventArgs e)
    {
      try
      {
        byte[] bytes = new WebClient().DownloadData(Options.DocHomeUri + "?idhome=" + (object) this.home.IdHome + "&sign=" + KvrplHelper.getMd5Hash(this.home.IdHome.ToString() + "doc123home"));
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "zip files (*.zip)|*.zip";
        saveFileDialog.FilterIndex = 2;
        saveFileDialog.RestoreDirectory = true;
        saveFileDialog.FileName = this.home.IdHome.ToString() + ".zip";
        if (saveFileDialog.ShowDialog() != DialogResult.OK)
          return;
        System.IO.File.WriteAllBytes(saveFileDialog.FileName, bytes);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Не найдены файлы на дом", "Ошибка");
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
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmHomeParam));
      this.pnButtons = new Panel();
      this.btnPastTime = new Button();
      this.progressBar1 = new ProgressBar();
      this.butExit = new Button();
      this.btnDelRecord = new Button();
      this.btnAddRecord = new Button();
      this.btnSave = new Button();
      this.panel1 = new Panel();
      this.tcntrHome = new TabControl();
      this.pgMain = new TabPage();
      this.panel4 = new Panel();
      this.butLoadHomeDoc = new Button();
      this.txbNumFloor = new TextBox();
      this.label11 = new Label();
      this.tbIdStr = new TextBox();
      this.label10 = new Label();
      this.btnCopy = new Button();
      this.label9 = new Label();
      this.tbWall = new TextBox();
      this.label8 = new Label();
      this.tbYearBuild = new TextBox();
      this.label7 = new Label();
      this.tbReu = new TextBox();
      this.label6 = new Label();
      this.tbGroupHome = new TextBox();
      this.label5 = new Label();
      this.tbIdHome = new TextBox();
      this.label4 = new Label();
      this.tbKorp = new TextBox();
      this.label3 = new Label();
      this.tbNHome = new TextBox();
      this.label2 = new Label();
      this.tbStreet = new TextBox();
      this.label1 = new Label();
      this.tbCompany = new TextBox();
      this.pgParam = new TabPage();
      this.dataGridView1 = new DataGridView();
      this.cmsHomeParam = new ContextMenuStrip(this.components);
      this.скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem = new ToolStripMenuItem();
      this.удалитьЗаписьИзВыбранныхОбъектовToolStripMenuItem = new ToolStripMenuItem();
      this.обновитьЗаписьВВыбранныхОбъектовToolStripMenuItem = new ToolStripMenuItem();
      this.pgReceipt = new TabPage();
      this.dgvHomesPhones = new DataGridView();
      this.pgDogovor = new TabPage();
      this.dgvDogovor = new DataGridView();
      this.pnUp = new Panel();
      this.mpCurrentPeriod = new MonthPicker();
      this.lblPastTime = new Label();
      this.chbArhiv = new CheckBox();
      this.lblAddress = new Label();
      this.lblMonthCLosed = new Label();
      this.lblClosed = new Label();
      this.tmr = new System.Windows.Forms.Timer(this.components);
      this.hp = new HelpProvider();
      this.pnButtons.SuspendLayout();
      this.panel1.SuspendLayout();
      this.tcntrHome.SuspendLayout();
      this.pgMain.SuspendLayout();
      this.panel4.SuspendLayout();
      this.pgParam.SuspendLayout();
      ((ISupportInitialize) this.dataGridView1).BeginInit();
      this.cmsHomeParam.SuspendLayout();
      this.pgReceipt.SuspendLayout();
      ((ISupportInitialize) this.dgvHomesPhones).BeginInit();
      this.pgDogovor.SuspendLayout();
      ((ISupportInitialize) this.dgvDogovor).BeginInit();
      this.pnUp.SuspendLayout();
      this.SuspendLayout();
      this.pnButtons.Controls.Add((Control) this.btnPastTime);
      this.pnButtons.Controls.Add((Control) this.progressBar1);
      this.pnButtons.Controls.Add((Control) this.butExit);
      this.pnButtons.Controls.Add((Control) this.btnDelRecord);
      this.pnButtons.Controls.Add((Control) this.btnAddRecord);
      this.pnButtons.Controls.Add((Control) this.btnSave);
      this.pnButtons.Dock = DockStyle.Bottom;
      this.pnButtons.Location = new Point(0, 440);
      this.pnButtons.Margin = new Padding(4);
      this.pnButtons.Name = "pnButtons";
      this.pnButtons.Size = new Size(788, 40);
      this.pnButtons.TabIndex = 0;
      this.btnPastTime.Image = (Image) Resources.time_24;
      this.btnPastTime.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnPastTime.Location = new Point(362, 5);
      this.btnPastTime.Name = "btnPastTime";
      this.btnPastTime.Size = new Size(152, 30);
      this.btnPastTime.TabIndex = 9;
      this.btnPastTime.Text = "Прошлое время";
      this.btnPastTime.TextAlign = ContentAlignment.MiddleRight;
      this.btnPastTime.UseVisualStyleBackColor = true;
      this.btnPastTime.Click += new EventHandler(this.btnPastTime_Click);
      this.progressBar1.Location = new Point(520, 5);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new Size(100, 28);
      this.progressBar1.TabIndex = 8;
      this.butExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.butExit.DialogResult = DialogResult.Cancel;
      this.butExit.Image = (Image) Resources.Exit;
      this.butExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.butExit.Location = new Point(692, 5);
      this.butExit.Margin = new Padding(4);
      this.butExit.Name = "butExit";
      this.butExit.Size = new Size(83, 30);
      this.butExit.TabIndex = 7;
      this.butExit.Text = "Выход";
      this.butExit.TextAlign = ContentAlignment.MiddleRight;
      this.butExit.UseVisualStyleBackColor = true;
      this.butExit.Click += new EventHandler(this.butExit_Click);
      this.btnDelRecord.Image = (Image) Resources.minus;
      this.btnDelRecord.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnDelRecord.Location = new Point(129, 5);
      this.btnDelRecord.Name = "btnDelRecord";
      this.btnDelRecord.Size = new Size(108, 30);
      this.btnDelRecord.TabIndex = 6;
      this.btnDelRecord.Text = "Удалить";
      this.btnDelRecord.TextAlign = ContentAlignment.MiddleRight;
      this.btnDelRecord.UseVisualStyleBackColor = true;
      this.btnDelRecord.Click += new EventHandler(this.btnDelRecord_Click);
      this.btnAddRecord.Image = (Image) Resources.plus;
      this.btnAddRecord.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnAddRecord.Location = new Point(12, 5);
      this.btnAddRecord.Name = "btnAddRecord";
      this.btnAddRecord.Size = new Size(111, 30);
      this.btnAddRecord.TabIndex = 5;
      this.btnAddRecord.Text = "Добавить";
      this.btnAddRecord.TextAlign = ContentAlignment.MiddleRight;
      this.btnAddRecord.UseVisualStyleBackColor = true;
      this.btnAddRecord.Click += new EventHandler(this.btnAddRecord_Click);
      this.btnSave.Image = (Image) Resources.Tick;
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.Location = new Point(243, 5);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(113, 30);
      this.btnSave.TabIndex = 4;
      this.btnSave.Text = "Сохранить";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.panel1.Controls.Add((Control) this.tcntrHome);
      this.panel1.Controls.Add((Control) this.pnUp);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Margin = new Padding(4);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(788, 440);
      this.panel1.TabIndex = 1;
      this.tcntrHome.Controls.Add((Control) this.pgMain);
      this.tcntrHome.Controls.Add((Control) this.pgParam);
      this.tcntrHome.Controls.Add((Control) this.pgReceipt);
      this.tcntrHome.Controls.Add((Control) this.pgDogovor);
      this.tcntrHome.Dock = DockStyle.Fill;
      this.tcntrHome.Location = new Point(0, 56);
      this.tcntrHome.Name = "tcntrHome";
      this.tcntrHome.SelectedIndex = 0;
      this.tcntrHome.Size = new Size(788, 384);
      this.tcntrHome.TabIndex = 1;
      this.tcntrHome.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
      this.pgMain.Controls.Add((Control) this.panel4);
      this.pgMain.Location = new Point(4, 25);
      this.pgMain.Name = "pgMain";
      this.pgMain.Padding = new Padding(3);
      this.pgMain.Size = new Size(780, 355);
      this.pgMain.TabIndex = 1;
      this.pgMain.Text = "Характеристики";
      this.pgMain.UseVisualStyleBackColor = true;
      this.panel4.Controls.Add((Control) this.butLoadHomeDoc);
      this.panel4.Controls.Add((Control) this.txbNumFloor);
      this.panel4.Controls.Add((Control) this.label11);
      this.panel4.Controls.Add((Control) this.tbIdStr);
      this.panel4.Controls.Add((Control) this.label10);
      this.panel4.Controls.Add((Control) this.btnCopy);
      this.panel4.Controls.Add((Control) this.label9);
      this.panel4.Controls.Add((Control) this.tbWall);
      this.panel4.Controls.Add((Control) this.label8);
      this.panel4.Controls.Add((Control) this.tbYearBuild);
      this.panel4.Controls.Add((Control) this.label7);
      this.panel4.Controls.Add((Control) this.tbReu);
      this.panel4.Controls.Add((Control) this.label6);
      this.panel4.Controls.Add((Control) this.tbGroupHome);
      this.panel4.Controls.Add((Control) this.label5);
      this.panel4.Controls.Add((Control) this.tbIdHome);
      this.panel4.Controls.Add((Control) this.label4);
      this.panel4.Controls.Add((Control) this.tbKorp);
      this.panel4.Controls.Add((Control) this.label3);
      this.panel4.Controls.Add((Control) this.tbNHome);
      this.panel4.Controls.Add((Control) this.label2);
      this.panel4.Controls.Add((Control) this.tbStreet);
      this.panel4.Controls.Add((Control) this.label1);
      this.panel4.Controls.Add((Control) this.tbCompany);
      this.panel4.Dock = DockStyle.Fill;
      this.panel4.Location = new Point(3, 3);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(774, 349);
      this.panel4.TabIndex = 6;
      this.butLoadHomeDoc.Location = new Point(15, 263);
      this.butLoadHomeDoc.Name = "butLoadHomeDoc";
      this.butLoadHomeDoc.Size = new Size(165, 30);
      this.butLoadHomeDoc.TabIndex = 23;
      this.butLoadHomeDoc.Text = "Документы на дом";
      this.butLoadHomeDoc.UseVisualStyleBackColor = true;
      this.butLoadHomeDoc.Click += new EventHandler(this.butLoadHomeDoc_Click);
      this.txbNumFloor.Location = new Point(445, 219);
      this.txbNumFloor.Name = "txbNumFloor";
      this.txbNumFloor.Size = new Size(68, 23);
      this.txbNumFloor.TabIndex = 22;
      this.label11.AutoSize = true;
      this.label11.Location = new Point(326, 222);
      this.label11.Name = "label11";
      this.label11.Size = new Size(78, 17);
      this.label11.TabIndex = 21;
      this.label11.Text = "Этажность";
      this.tbIdStr.Location = new Point(445, 50);
      this.tbIdStr.Name = "tbIdStr";
      this.tbIdStr.Size = new Size(68, 23);
      this.tbIdStr.TabIndex = 20;
      this.label10.AutoSize = true;
      this.label10.Location = new Point(326, 59);
      this.label10.Name = "label10";
      this.label10.Size = new Size(33, 17);
      this.label10.TabIndex = 19;
      this.label10.Text = "Код";
      this.btnCopy.Image = (Image) Resources.add_var;
      this.btnCopy.Location = new Point(223, 177);
      this.btnCopy.Name = "btnCopy";
      this.btnCopy.Size = new Size(35, 23);
      this.btnCopy.TabIndex = 18;
      this.btnCopy.UseVisualStyleBackColor = true;
      this.btnCopy.Click += new EventHandler(this.btnCopy_Click);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(14, 222);
      this.label9.Name = "label9";
      this.label9.Size = new Size(108, 17);
      this.label9.TabIndex = 17;
      this.label9.Text = "Материал стен";
      this.tbWall.Location = new Point(137, 219);
      this.tbWall.Name = "tbWall";
      this.tbWall.Size = new Size(183, 23);
      this.tbWall.TabIndex = 16;
      this.label8.AutoSize = true;
      this.label8.Location = new Point(326, 177);
      this.label8.Name = "label8";
      this.label8.Size = new Size(113, 17);
      this.label8.TabIndex = 15;
      this.label8.Text = "Год посторойки";
      this.tbYearBuild.Location = new Point(445, 174);
      this.tbYearBuild.Name = "tbYearBuild";
      this.tbYearBuild.Size = new Size(68, 23);
      this.tbYearBuild.TabIndex = 14;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(326, 135);
      this.label7.Name = "label7";
      this.label7.Size = new Size(72, 17);
      this.label7.TabIndex = 13;
      this.label7.Text = "ЖКО/РЭУ";
      this.tbReu.Location = new Point(445, 132);
      this.tbReu.Name = "tbReu";
      this.tbReu.Size = new Size(68, 23);
      this.tbReu.TabIndex = 12;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(14, 180);
      this.label6.Name = "label6";
      this.label6.Size = new Size(99, 17);
      this.label6.TabIndex = 11;
      this.label6.Text = "Группа домов";
      this.tbGroupHome.Location = new Point(137, 177);
      this.tbGroupHome.Name = "tbGroupHome";
      this.tbGroupHome.Size = new Size(77, 23);
      this.tbGroupHome.TabIndex = 10;
      this.tbGroupHome.TextChanged += new EventHandler(this.tbGroupHome_TextChanged);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(14, 138);
      this.label5.Name = "label5";
      this.label5.Size = new Size(99, 17);
      this.label5.TabIndex = 9;
      this.label5.Text = "Код строения";
      this.tbIdHome.Location = new Point(137, 135);
      this.tbIdHome.Name = "tbIdHome";
      this.tbIdHome.Size = new Size(93, 23);
      this.tbIdHome.TabIndex = 8;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(326, 94);
      this.label4.Name = "label4";
      this.label4.Size = new Size(55, 17);
      this.label4.TabIndex = 7;
      this.label4.Text = "Корпус";
      this.tbKorp.Location = new Point(445, 91);
      this.tbKorp.Name = "tbKorp";
      this.tbKorp.Size = new Size(68, 23);
      this.tbKorp.TabIndex = 6;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 100);
      this.label3.Name = "label3";
      this.label3.Size = new Size(88, 17);
      this.label3.TabIndex = 5;
      this.label3.Text = "Номер дома";
      this.tbNHome.Location = new Point(137, 94);
      this.tbNHome.Name = "tbNHome";
      this.tbNHome.Size = new Size(93, 23);
      this.tbNHome.TabIndex = 4;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 59);
      this.label2.Name = "label2";
      this.label2.Size = new Size(49, 17);
      this.label2.TabIndex = 3;
      this.label2.Text = "Улица";
      this.tbStreet.Location = new Point(137, 56);
      this.tbStreet.Name = "tbStreet";
      this.tbStreet.Size = new Size(183, 23);
      this.tbStreet.TabIndex = 2;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(119, 34);
      this.label1.TabIndex = 1;
      this.label1.Text = "Код\r\nдомоуправления";
      this.tbCompany.Location = new Point(137, 10);
      this.tbCompany.Name = "tbCompany";
      this.tbCompany.Size = new Size(93, 23);
      this.tbCompany.TabIndex = 0;
      this.pgParam.Controls.Add((Control) this.dataGridView1);
      this.pgParam.Location = new Point(4, 25);
      this.pgParam.Name = "pgParam";
      this.pgParam.Padding = new Padding(3);
      this.pgParam.Size = new Size(780, 355);
      this.pgParam.TabIndex = 0;
      this.pgParam.Text = "Параметры";
      this.pgParam.UseVisualStyleBackColor = true;
      this.dataGridView1.BackgroundColor = Color.AliceBlue;
      this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.ContextMenuStrip = this.cmsHomeParam;
      this.dataGridView1.Dock = DockStyle.Fill;
      this.dataGridView1.Location = new Point(3, 3);
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.Size = new Size(774, 349);
      this.dataGridView1.TabIndex = 4;
      this.dataGridView1.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
      this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);
      this.dataGridView1.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
      this.dataGridView1.CellMouseDown += new DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
      this.dataGridView1.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
      this.dataGridView1.CurrentCellDirtyStateChanged += new EventHandler(this.dataGridView1_CurrentCellDirtyStateChanged);
      this.dataGridView1.DataError += new DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
      this.dataGridView1.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dataGridView1_EditingControlShowing);
      this.cmsHomeParam.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem,
        (ToolStripItem) this.удалитьЗаписьИзВыбранныхОбъектовToolStripMenuItem,
        (ToolStripItem) this.обновитьЗаписьВВыбранныхОбъектовToolStripMenuItem
      });
      this.cmsHomeParam.Name = "cmsHomeParam";
      this.cmsHomeParam.Size = new Size(313, 70);
      this.скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem.Image = (Image) Resources.add_var;
      this.скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem.Name = "скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem";
      this.скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem.Size = new Size(312, 22);
      this.скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem.Text = "Скопировать запись в выбранные объекты";
      this.скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem.Click += new EventHandler(this.скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem_Click);
      this.удалитьЗаписьИзВыбранныхОбъектовToolStripMenuItem.Image = (Image) Resources.minus;
      this.удалитьЗаписьИзВыбранныхОбъектовToolStripMenuItem.Name = "удалитьЗаписьИзВыбранныхОбъектовToolStripMenuItem";
      this.удалитьЗаписьИзВыбранныхОбъектовToolStripMenuItem.Size = new Size(312, 22);
      this.удалитьЗаписьИзВыбранныхОбъектовToolStripMenuItem.Text = "Удалить запись из выбранных объектов";
      this.удалитьЗаписьИзВыбранныхОбъектовToolStripMenuItem.Click += new EventHandler(this.скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem_Click);
      this.обновитьЗаписьВВыбранныхОбъектовToolStripMenuItem.Image = (Image) Resources.redo;
      this.обновитьЗаписьВВыбранныхОбъектовToolStripMenuItem.Name = "обновитьЗаписьВВыбранныхОбъектовToolStripMenuItem";
      this.обновитьЗаписьВВыбранныхОбъектовToolStripMenuItem.Size = new Size(312, 22);
      this.обновитьЗаписьВВыбранныхОбъектовToolStripMenuItem.Text = "Обновить запись в выбранных объектах";
      this.обновитьЗаписьВВыбранныхОбъектовToolStripMenuItem.Click += new EventHandler(this.скопироватьЗаписьВВыбранныеОбъектыToolStripMenuItem_Click);
      this.pgReceipt.Controls.Add((Control) this.dgvHomesPhones);
      this.pgReceipt.Location = new Point(4, 25);
      this.pgReceipt.Name = "pgReceipt";
      this.pgReceipt.Padding = new Padding(3);
      this.pgReceipt.Size = new Size(780, 355);
      this.pgReceipt.TabIndex = 2;
      this.pgReceipt.Text = "Службы и телефоны";
      this.pgReceipt.UseVisualStyleBackColor = true;
      this.dgvHomesPhones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.dgvHomesPhones.BackgroundColor = Color.AliceBlue;
      this.dgvHomesPhones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvHomesPhones.ContextMenuStrip = this.cmsHomeParam;
      this.dgvHomesPhones.Dock = DockStyle.Fill;
      this.dgvHomesPhones.Location = new Point(3, 3);
      this.dgvHomesPhones.Name = "dgvHomesPhones";
      this.dgvHomesPhones.Size = new Size(774, 349);
      this.dgvHomesPhones.TabIndex = 0;
      this.dgvHomesPhones.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvHomesPhones_CellBeginEdit);
      this.dgvHomesPhones.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvHomesPhones_CellFormatting);
      this.dgvHomesPhones.CellMouseDown += new DataGridViewCellMouseEventHandler(this.dgvHomesPhones_CellMouseDown);
      this.dgvHomesPhones.DataError += new DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
      this.pgDogovor.Controls.Add((Control) this.dgvDogovor);
      this.pgDogovor.Location = new Point(4, 25);
      this.pgDogovor.Name = "pgDogovor";
      this.pgDogovor.Padding = new Padding(3);
      this.pgDogovor.Size = new Size(780, 355);
      this.pgDogovor.TabIndex = 3;
      this.pgDogovor.Text = "Договоры на управление";
      this.pgDogovor.UseVisualStyleBackColor = true;
      this.dgvDogovor.BackgroundColor = Color.AliceBlue;
      this.dgvDogovor.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvDogovor.Dock = DockStyle.Fill;
      this.dgvDogovor.Location = new Point(3, 3);
      this.dgvDogovor.Name = "dgvDogovor";
      this.dgvDogovor.Size = new Size(774, 349);
      this.dgvDogovor.TabIndex = 0;
      this.dgvDogovor.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvDogovor_CellBeginEdit);
      this.dgvDogovor.CellEndEdit += new DataGridViewCellEventHandler(this.dgvDogovor_CellEndEdit);
      this.dgvDogovor.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvDogovor_CellFormatting);
      this.dgvDogovor.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvDogovor_ColumnWidthChanged);
      this.pnUp.Controls.Add((Control) this.mpCurrentPeriod);
      this.pnUp.Controls.Add((Control) this.lblPastTime);
      this.pnUp.Controls.Add((Control) this.chbArhiv);
      this.pnUp.Controls.Add((Control) this.lblAddress);
      this.pnUp.Controls.Add((Control) this.lblMonthCLosed);
      this.pnUp.Controls.Add((Control) this.lblClosed);
      this.pnUp.Dock = DockStyle.Top;
      this.pnUp.Location = new Point(0, 0);
      this.pnUp.Name = "pnUp";
      this.pnUp.Size = new Size(788, 56);
      this.pnUp.TabIndex = 0;
      this.mpCurrentPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.mpCurrentPeriod.CustomFormat = "MMMM yyyy";
      this.mpCurrentPeriod.Format = DateTimePickerFormat.Custom;
      this.mpCurrentPeriod.Location = new Point(636, 7);
      this.mpCurrentPeriod.Name = "mpCurrentPeriod";
      this.mpCurrentPeriod.OldMonth = 0;
      this.mpCurrentPeriod.ShowUpDown = true;
      this.mpCurrentPeriod.Size = new Size(140, 23);
      this.mpCurrentPeriod.TabIndex = 21;
      this.mpCurrentPeriod.ValueChanged += new EventHandler(this.dtmpCurrentPeriod_ValueChanged);
      this.lblPastTime.AutoSize = true;
      this.lblPastTime.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblPastTime.ForeColor = Color.DarkOrange;
      this.lblPastTime.Location = new Point(87, 32);
      this.lblPastTime.Name = "lblPastTime";
      this.lblPastTime.Size = new Size(199, 16);
      this.lblPastTime.TabIndex = 19;
      this.lblPastTime.Text = "Режим прошлого времени";
      this.lblPastTime.Visible = false;
      this.chbArhiv.AutoSize = true;
      this.chbArhiv.Location = new Point(7, 32);
      this.chbArhiv.Name = "chbArhiv";
      this.chbArhiv.Size = new Size(65, 21);
      this.chbArhiv.TabIndex = 18;
      this.chbArhiv.Text = "Архив";
      this.chbArhiv.UseVisualStyleBackColor = true;
      this.chbArhiv.CheckedChanged += new EventHandler(this.chbArhiv_CheckedChanged);
      this.lblAddress.AutoSize = true;
      this.lblAddress.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblAddress.Location = new Point(12, 12);
      this.lblAddress.Name = "lblAddress";
      this.lblAddress.Size = new Size(52, 17);
      this.lblAddress.TabIndex = 17;
      this.lblAddress.Text = "label1";
      this.lblMonthCLosed.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblMonthCLosed.AutoSize = true;
      this.lblMonthCLosed.Location = new Point(712, 33);
      this.lblMonthCLosed.Name = "lblMonthCLosed";
      this.lblMonthCLosed.Size = new Size(46, 17);
      this.lblMonthCLosed.TabIndex = 16;
      this.lblMonthCLosed.Text = "label2";
      this.lblClosed.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblClosed.AutoSize = true;
      this.lblClosed.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblClosed.Location = new Point(566, 33);
      this.lblClosed.Margin = new Padding(4, 0, 4, 0);
      this.lblClosed.Name = "lblClosed";
      this.lblClosed.Size = new Size(139, 16);
      this.lblClosed.TabIndex = 15;
      this.lblClosed.Text = "Закрытый период";
      this.tmr.Interval = 1000;
      this.tmr.Tick += new EventHandler(this.tmr_Tick);
      this.hp.HelpNamespace = "Help.chm";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.butExit;
      this.ClientSize = new Size(788, 480);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.pnButtons);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.hp.SetHelpKeyword((Control) this, "kv141.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmHomeParam";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Параметры на дом";
      this.Shown += new EventHandler(this.FrmHomeParam_Shown);
      this.pnButtons.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.tcntrHome.ResumeLayout(false);
      this.pgMain.ResumeLayout(false);
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      this.pgParam.ResumeLayout(false);
      ((ISupportInitialize) this.dataGridView1).EndInit();
      this.cmsHomeParam.ResumeLayout(false);
      this.pgReceipt.ResumeLayout(false);
      ((ISupportInitialize) this.dgvHomesPhones).EndInit();
      this.pgDogovor.ResumeLayout(false);
      ((ISupportInitialize) this.dgvDogovor).EndInit();
      this.pnUp.ResumeLayout(false);
      this.pnUp.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
