// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmCounters
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using FastReport;
using FastReport.Utils;
using Kvartplata.Classes;
using Kvartplata.Properties;
using Kvartplata.StaticResourse;
using NHibernate;
using NHibernate.Criterion;
using SaveSettings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmCounters : Form
  {
    private FormStateSaver formState = new FormStateSaver(FrmCounters.container);
    protected GridSettings MySettingsCounters = new GridSettings();
    protected GridSettings MySettingsEvidence = new GridSettings();
    protected GridSettings MySettingsAudit = new GridSettings();
    protected GridSettings MySettingsSeal = new GridSettings();
    protected GridSettings MySettingsDetailEvidence = new GridSettings();
    protected GridSettings MySettingsScheme = new GridSettings();
    private IContainer components = (IContainer) null;
    private ISession session;
    private short level;
    private IList<Counter> counters;
    private IList<Evidence> evidences;
    private Home home;
    private Company company;
    private Period monthClosed;
    private static IContainer container;
    private bool insertRecord;
    private IList<TypeCounter> typeCountersList;
    private IList<Service> serviceList;
    private IList<Home> homes;
    private IList<LsClient> clients;
    private IList<Period> periods;
    private IList<Period> months;
    private string uslovie;
    private double lastPast;
    private double lastCurrent;
    private DateTime lastDBeg;
    private DateTime lastDEnd;
    protected bool pastTime;
    private bool editEvidence;
    private DateTime dateArchive;
    private int oldBase;
    private int city;
    private int setup31;
    private Panel pnBtn;
    private Button bntExit;
    private Panel pnTools;
    private ComboBox cmbService;
    private Label lblService;
    private ComboBox cmbBase;
    private Label lblBase;
    private Button btnAdd;
    private Button btnSave;
    private Button btnDelete;
    private Button btnQuarterSum;
    private ProgressBar pbar;
    private Button btnArchive;
    private TabControl tcntrlCounters;
    private TabPage tpCounters;
    private DataGridView dgvCounters;
    private TabPage tpPeriod;
    private DataGridView dgvEvidence;
    private TabPage tpDetail;
    private DataGridView dgvDetail;
    private MonthPicker mpCurrentPeriod;
    private TabPage tpDistribute;
    private DataGridView dgvDistribute;
    private Label lblCounterDay;
    private DateTimePicker dtmCounterDay;
    private Label lblCaption;
    private Button btnReport;
    private ContextMenuStrip cmReport;
    private ToolStripMenuItem miClient;
    private ToolStripMenuItem miHome;
    private TabPage tpDstrDetail;
    private DataGridView dgvDstrDetail;
    public HelpProvider hp;
    private TabPage tpAudit;
    private DataGridView dgvAudit;
    private Button btnPastTime;
    private Label lblCounter;
    private Label lblMonth;
    private Label lblPeriod;
    private ComboBox cmbCounter;
    private ComboBox cmbMonth;
    private ComboBox cmbPeriod;
    private Panel pnDetail;
    private Button btnAudit;
    private TabPage tpSeal;
    private DataGridView dgvSeal;
    private Label lblPastTime;
    private Timer tmr;
    private TabPage tpDetailEvidence;
    private DataGridView dgvDetailEvidence;
    private CheckBox cbArchive;
    private Button btnEdit;
    private Timer tmrEvidence;
    private Label lblEdit;
    private MonthCalendar mcArchive;
    private TabPage tpScheme;
    private DataGridView dgvScheme;
    private DataSet dsReport;
    private Button btnPrint;
    private ToolStripMenuItem детализацияРаспределенныхСуммToolStripMenuItem;
    private Button btnLoad;
    private Button btnCloseScheme;
    private TabPage tpWorkDistribute;
    private DataGridView dgvWorkDistribute;

    public FrmCounters()
    {
      this.InitializeComponent();
      this.formState.ParentForm = (Form) this;
      this.SetGridConfigFileSettings();
    }

    public FrmCounters(short level, Company company, Home home)
    {
      this.InitializeComponent();
      this.formState.ParentForm = (Form) this;
      this.level = level;
      this.company = company;
      this.home = home;
      this.SetGridConfigFileSettings();
    }

    public void SetGridConfigFileSettings()
    {
      this.MySettingsCounters.ConfigFile = Options.PathProfile + "\\State\\config.xml";
      this.MySettingsEvidence.ConfigFile = Options.PathProfile + "\\State\\config.xml";
      this.MySettingsAudit.ConfigFile = Options.PathProfile + "\\State\\config.xml";
      this.MySettingsSeal.ConfigFile = Options.PathProfile + "\\State\\config.xml";
      this.MySettingsDetailEvidence.ConfigFile = Options.PathProfile + "\\State\\config.xml";
      this.MySettingsScheme.ConfigFile = Options.PathProfile + "\\State\\config.xml";
    }

    private void bntExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void dtmpCurrentPeriod_ValueChanged(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      Options.Period = KvrplHelper.SaveCurrentPeriod(this.mpCurrentPeriod.Value);
      if (this.tcntrlCounters.SelectedTab == this.tpCounters)
        this.LoadCounters();
      if (this.tcntrlCounters.SelectedTab == this.tpPeriod)
      {
        if (this.setup31 == 1)
          this.dtmCounterDay.Value = KvrplHelper.LastDay(this.mpCurrentPeriod.Value);
        if (this.setup31 == 2)
          this.dtmCounterDay.Value = DateTime.Now.AddDays(-1.0);
        this.PrepareDetail();
        this.LoadEvidence();
      }
      if (this.tcntrlCounters.SelectedTab == this.tpAudit)
        this.LoadAudit();
      if (this.tcntrlCounters.SelectedTab == this.tpDetail)
        this.LoadDetail();
      if (this.tcntrlCounters.SelectedTab == this.tpDetailEvidence)
        this.LoadDetailEvidence();
      if (this.tcntrlCounters.SelectedTab == this.tpDistribute)
        this.LoadDistribute();
      if (this.tcntrlCounters.SelectedTab == this.tpDstrDetail)
        this.LoadDstrDetail();
      if (this.tcntrlCounters.SelectedTab == this.tpScheme)
        this.LoadScheme();
      if (this.tcntrlCounters.SelectedTab == this.tpWorkDistribute)
        this.LoadWorkDistribute();
      this.Cursor = Cursors.Default;
    }

    private void cmbService_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (this.tcntrlCounters.SelectedTab == this.tpCounters)
        this.LoadCounters();
      if (this.tcntrlCounters.SelectedTab == this.tpPeriod)
        this.LoadEvidence();
      if (this.tcntrlCounters.SelectedTab == this.tpAudit)
        this.LoadAudit();
      if (this.tcntrlCounters.SelectedTab == this.tpSeal)
        this.LoadSeal();
      if (this.tcntrlCounters.SelectedTab == this.tpDetailEvidence)
        this.LoadDetailEvidence();
      if (this.tcntrlCounters.SelectedTab == this.tpDetail && (Convert.ToInt32(this.cmbBase.SelectedValue) == 1 || Convert.ToInt32(this.cmbBase.SelectedValue) == 4))
        this.LoadDetail();
      if (this.tcntrlCounters.SelectedTab == this.tpDistribute)
        this.LoadDistribute();
      if (this.tcntrlCounters.SelectedTab == this.tpDstrDetail)
        this.LoadDstrDetail();
      if (this.tcntrlCounters.SelectedTab == this.tpScheme)
        this.LoadScheme();
      if (this.tcntrlCounters.SelectedTab != this.tpWorkDistribute)
        return;
      this.LoadWorkDistribute();
    }

    private void cmbBase_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (this.tcntrlCounters.SelectedTab == this.tpCounters)
        this.LoadCounters();
      if (this.tcntrlCounters.SelectedTab == this.tpPeriod)
        this.LoadEvidence();
      if (this.tcntrlCounters.SelectedTab == this.tpAudit)
        this.LoadAudit();
      if (this.tcntrlCounters.SelectedTab == this.tpSeal)
        this.LoadSeal();
      if (this.tcntrlCounters.SelectedTab == this.tpDetailEvidence)
        this.LoadDetailEvidence();
      if (this.tcntrlCounters.SelectedTab == this.tpDetail)
      {
        if ((int) this.level == 2 && Convert.ToInt32(this.cmbBase.SelectedValue) != 1 && Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
          this.cmbBase.SelectedValue = (object) this.oldBase;
        else
          this.LoadDetail();
      }
      if (this.tcntrlCounters.SelectedTab == this.tpDistribute)
      {
        if ((int) this.level == 2 && Convert.ToInt32(this.cmbBase.SelectedValue) != 1 && Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
          this.cmbBase.SelectedValue = (object) this.oldBase;
        else
          this.LoadDistribute();
      }
      if (this.tcntrlCounters.SelectedTab == this.tpDstrDetail)
      {
        if ((int) this.level == 2 && Convert.ToInt32(this.cmbBase.SelectedValue) != 1 && Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
          this.cmbBase.SelectedValue = (object) this.oldBase;
        else
          this.LoadDstrDetail();
      }
      if (this.tcntrlCounters.SelectedTab == this.tpScheme)
      {
        if ((int) this.level == 2 && Convert.ToInt32(this.cmbBase.SelectedValue) != 1 && Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
          this.cmbBase.SelectedValue = (object) this.oldBase;
        else
          this.LoadScheme();
      }
      this.oldBase = Convert.ToInt32(this.cmbBase.SelectedValue);
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      this.btnSave.Enabled = true;
      if (!KvrplHelper.CheckProxy(42, 2, this.company, true))
        return;
      DateTime dateTime = this.monthClosed.PeriodName.Value;
      DateTime? periodName = Options.Period.PeriodName;
      if (periodName.HasValue && dateTime >= periodName.GetValueOrDefault())
      {
        int num1 = (int) MessageBox.Show("Ввод данных возможен только в открытом периоде", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (this.tcntrlCounters.SelectedTab == this.tpCounters)
        {
          if ((int) Convert.ToInt16(this.cmbService.SelectedValue) == 0)
          {
            int num2 = (int) MessageBox.Show("Выберите услугу!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return;
          }
          this.InsertCounter();
        }
        if (this.tcntrlCounters.SelectedTab == this.tpPeriod)
          this.InsertEvidence();
        if (this.tcntrlCounters.SelectedTab == this.tpAudit)
          this.InsertAudit();
        if (this.tcntrlCounters.SelectedTab == this.tpSeal)
          this.InsertSeal();
        if (this.tcntrlCounters.SelectedTab == this.tpDistribute)
          this.InsertDistribute();
        if (this.tcntrlCounters.SelectedTab == this.tpScheme)
          this.InsertScheme();
        if (this.tcntrlCounters.SelectedTab != this.tpWorkDistribute)
          return;
        this.InsertWorkDistribute();
      }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      this.btnAdd.Enabled = true;
      this.btnDelete.Enabled = true;
      this.btnSave.Enabled = false;
      if (!KvrplHelper.CheckProxy(42, 2, this.company, true))
        return;
      if (this.tcntrlCounters.SelectedTab == this.tpCounters)
      {
        this.SaveAllCounters();
        if (!this.insertRecord)
          this.LoadCounters();
      }
      if (this.tcntrlCounters.SelectedTab == this.tpPeriod)
      {
        this.SaveEvidence();
        if (!this.insertRecord)
          this.LoadEvidence();
      }
      if (this.tcntrlCounters.SelectedTab == this.tpAudit)
      {
        this.SaveAudit();
        if (!this.insertRecord)
          this.LoadAudit();
      }
      if (this.tcntrlCounters.SelectedTab == this.tpSeal)
      {
        this.SaveSeal();
        if (!this.insertRecord)
          this.LoadSeal();
      }
      if (this.tcntrlCounters.SelectedTab == this.tpDistribute)
      {
        this.SaveDistribute();
        if (!this.insertRecord)
          this.LoadDistribute();
      }
      if (this.tcntrlCounters.SelectedTab == this.tpScheme)
      {
        this.SaveScheme();
        if (!this.insertRecord)
          this.LoadScheme();
      }
      if (this.tcntrlCounters.SelectedTab != this.tpWorkDistribute)
        return;
      this.SaveWorkDistribute();
      if (!this.insertRecord)
        this.LoadWorkDistribute();
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(42, 2, this.company, true))
        return;
      if (this.monthClosed.PeriodName.Value >= Options.Period.PeriodName.Value)
      {
        int num = (int) MessageBox.Show("Невозможно удалить запись в закрытом периоде!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (this.tcntrlCounters.SelectedTab == this.tpCounters)
        {
          this.DeleteCounter();
          this.LoadCounters();
        }
        if (this.tcntrlCounters.SelectedTab == this.tpPeriod)
        {
          this.DeleteEvidence();
          this.LoadEvidence();
        }
        if (this.tcntrlCounters.SelectedTab == this.tpAudit)
        {
          this.DeleteAudit();
          this.LoadAudit();
        }
        if (this.tcntrlCounters.SelectedTab == this.tpSeal)
        {
          this.DeleteSeal();
          this.LoadSeal();
        }
        if (this.tcntrlCounters.SelectedTab == this.tpDistribute)
        {
          this.DeleteDistribute();
          this.LoadDistribute();
        }
        if (this.tcntrlCounters.SelectedTab == this.tpScheme)
        {
          this.DeleteScheme();
          this.LoadScheme();
        }
        if (this.tcntrlCounters.SelectedTab != this.tpWorkDistribute)
          return;
        this.DeleteWorkDitribute();
        this.LoadWorkDistribute();
      }
    }

    private void btnQuarter_Click(object sender, EventArgs e)
    {
      FrmQuarterSum frmQuarterSum = new FrmQuarterSum((int) this.level, this.company, this.home, (BaseCounter) this.cmbBase.SelectedItem);
      int num = (int) frmQuarterSum.ShowDialog();
      frmQuarterSum.Dispose();
      this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
      this.LoadDistribute();
    }

    private void tcntrlCounters_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnLoad.Visible = false;
      this.pnTools.Height = 56;
      this.editEvidence = false;
      this.btnEdit.BackColor = this.pnBtn.BackColor;
      this.tmrEvidence.Stop();
      this.lblEdit.Visible = false;
      if (this.tcntrlCounters.SelectedTab == this.tpCounters)
      {
        this.mpCurrentPeriod.Enabled = true;
        this.pnDetail.Visible = false;
        this.cmbBase.Enabled = true;
        this.btnAdd.Enabled = true;
        this.btnDelete.Enabled = true;
        this.btnSave.Enabled = true;
        this.btnArchive.Enabled = true;
        this.cbArchive.Enabled = true;
        this.btnEdit.Visible = false;
        this.btnPastTime.Visible = true;
        this.btnPastTime.Enabled = false;
        this.lblCounterDay.Enabled = false;
        this.dtmCounterDay.Enabled = false;
        this.btnQuarterSum.Visible = false;
        this.btnAudit.Visible = false;
        this.btnPrint.Visible = false;
        this.btnCloseScheme.Visible = false;
        this.LoadCounters();
      }
      if (this.tcntrlCounters.SelectedTab == this.tpPeriod)
      {
        this.mpCurrentPeriod.Enabled = true;
        this.PrepareDetail();
        this.cmbBase.Enabled = true;
        this.btnAdd.Enabled = true;
        this.btnDelete.Enabled = true;
        this.btnSave.Enabled = true;
        this.btnArchive.Enabled = true;
        this.btnPastTime.Enabled = false;
        this.btnEdit.Visible = true;
        this.btnPastTime.Visible = false;
        this.lblCounterDay.Enabled = true;
        this.dtmCounterDay.Enabled = true;
        this.btnQuarterSum.Visible = false;
        this.btnAudit.Visible = false;
        this.btnPrint.Visible = false;
        this.btnCloseScheme.Visible = false;
        this.LoadEvidence();
      }
      if (this.tcntrlCounters.SelectedTab == this.tpDetailEvidence)
      {
        this.mpCurrentPeriod.Enabled = true;
        this.PrepareDetail();
        this.pnDetail.Visible = true;
        this.cmbBase.Enabled = true;
        this.btnArchive.Enabled = false;
        this.btnAdd.Enabled = false;
        this.btnDelete.Enabled = false;
        this.btnSave.Enabled = false;
        this.btnPastTime.Enabled = false;
        this.lblCounterDay.Enabled = false;
        this.dtmCounterDay.Enabled = false;
        this.btnQuarterSum.Visible = false;
        this.btnAudit.Visible = false;
        this.btnEdit.Visible = false;
        this.btnCloseScheme.Visible = false;
        this.btnPastTime.Visible = true;
        this.btnPrint.Visible = false;
        this.LoadDetailEvidence();
      }
      if (this.tcntrlCounters.SelectedTab == this.tpAudit)
      {
        this.mpCurrentPeriod.Enabled = true;
        this.pnDetail.Visible = false;
        this.cmbBase.Enabled = true;
        this.btnArchive.Enabled = false;
        this.btnAdd.Enabled = true;
        this.btnDelete.Enabled = true;
        this.btnSave.Enabled = true;
        this.btnArchive.Enabled = false;
        this.btnPastTime.Enabled = true;
        this.lblCounterDay.Enabled = false;
        this.dtmCounterDay.Enabled = false;
        this.btnQuarterSum.Visible = false;
        this.btnAudit.Visible = true;
        this.btnEdit.Visible = false;
        this.btnCloseScheme.Visible = true;
        this.btnPastTime.Visible = true;
        this.btnPrint.Visible = false;
        this.LoadAudit();
      }
      if (this.tcntrlCounters.SelectedTab == this.tpSeal)
      {
        this.mpCurrentPeriod.Enabled = true;
        this.pnDetail.Visible = false;
        this.cmbBase.Enabled = true;
        this.btnArchive.Enabled = false;
        this.btnAdd.Enabled = true;
        this.btnDelete.Enabled = true;
        this.btnSave.Enabled = true;
        this.btnArchive.Enabled = false;
        this.btnPastTime.Enabled = false;
        this.lblCounterDay.Enabled = false;
        this.dtmCounterDay.Enabled = false;
        this.btnQuarterSum.Visible = false;
        this.btnAudit.Visible = false;
        this.btnCloseScheme.Visible = false;
        this.btnEdit.Visible = false;
        this.btnPastTime.Visible = true;
        this.btnPrint.Visible = false;
        this.LoadSeal();
      }
      if (this.tcntrlCounters.SelectedTab == this.tpDetail)
      {
        this.mpCurrentPeriod.Enabled = true;
        this.cmbBase.SelectedValue = (object) 1;
        if ((int) this.level == 3)
          this.cmbBase.Enabled = false;
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 1)
        {
          this.pnDetail.Visible = true;
          this.cmbMonth.Visible = true;
          this.lblMonth.Visible = true;
          this.PrepareDetail();
          this.LoadDetail();
          this.btnAdd.Enabled = false;
          this.btnDelete.Enabled = false;
          this.btnSave.Enabled = false;
          this.btnArchive.Enabled = false;
          this.btnPastTime.Enabled = false;
          this.lblCounterDay.Enabled = false;
          this.dtmCounterDay.Enabled = false;
          this.btnQuarterSum.Visible = false;
          this.btnAudit.Visible = false;
          this.btnCloseScheme.Visible = false;
          this.btnEdit.Visible = false;
          this.btnPastTime.Visible = true;
          this.btnPrint.Visible = false;
        }
      }
      if (this.tcntrlCounters.SelectedTab == this.tpDistribute)
      {
        this.mpCurrentPeriod.Enabled = true;
        this.cmbBase.SelectedValue = (object) 1;
        this.pnDetail.Visible = false;
        this.LoadDistribute();
        this.btnAdd.Enabled = true;
        this.btnDelete.Enabled = true;
        this.btnSave.Enabled = true;
        this.btnArchive.Enabled = false;
        this.btnPastTime.Enabled = false;
        this.lblCounterDay.Enabled = false;
        this.dtmCounterDay.Enabled = false;
        this.btnQuarterSum.Visible = true;
        this.btnAudit.Visible = false;
        this.btnCloseScheme.Visible = false;
        this.btnEdit.Visible = false;
        this.btnPastTime.Visible = true;
        this.btnPrint.Visible = false;
      }
      if (this.tcntrlCounters.SelectedTab == this.tpDstrDetail)
      {
        this.mpCurrentPeriod.Enabled = true;
        this.cmbBase.SelectedValue = (object) 1;
        if ((int) this.level == 3)
          this.cmbBase.Enabled = false;
        this.btnPrint.Visible = true;
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 1)
        {
          this.pnDetail.Visible = true;
          this.cmbMonth.Visible = true;
          this.lblMonth.Visible = true;
          this.PrepareDetail();
          this.LoadDstrDetail();
          this.btnAdd.Enabled = false;
          this.btnDelete.Enabled = false;
          this.btnSave.Enabled = false;
          this.btnArchive.Enabled = false;
          this.btnPastTime.Enabled = false;
          this.lblCounterDay.Enabled = false;
          this.dtmCounterDay.Enabled = false;
          this.btnQuarterSum.Visible = false;
          this.btnAudit.Visible = false;
          this.btnCloseScheme.Visible = false;
          this.btnEdit.Visible = false;
          this.btnPastTime.Visible = true;
        }
      }
      if (this.tcntrlCounters.SelectedTab == this.tpScheme)
      {
        this.cmbBase.SelectedValue = (object) 1;
        if ((int) this.level == 3)
          this.cmbBase.Enabled = false;
        this.mpCurrentPeriod.Enabled = true;
        this.pnDetail.Visible = false;
        this.btnArchive.Enabled = false;
        this.btnAdd.Enabled = true;
        this.btnDelete.Enabled = true;
        this.btnSave.Enabled = true;
        this.btnPastTime.Enabled = true;
        this.lblCounterDay.Enabled = false;
        this.dtmCounterDay.Enabled = false;
        this.btnQuarterSum.Visible = false;
        this.btnAudit.Visible = false;
        this.btnCloseScheme.Visible = false;
        this.btnEdit.Visible = false;
        this.btnPastTime.Visible = true;
        this.btnPrint.Visible = false;
        this.LoadScheme();
      }
      if (this.tcntrlCounters.SelectedTab != this.tpWorkDistribute)
        return;
      this.mpCurrentPeriod.Enabled = true;
      this.cmbBase.SelectedValue = (object) 1;
      this.pnDetail.Visible = false;
      this.cbArchive.Visible = true;
      this.btnAdd.Enabled = KvrplHelper.CheckProxy(78, 2, this.company, false);
      this.btnDelete.Enabled = KvrplHelper.CheckProxy(78, 2, this.company, false);
      this.btnSave.Enabled = KvrplHelper.CheckProxy(78, 2, this.company, false);
      this.btnArchive.Enabled = false;
      this.btnPastTime.Enabled = false;
      this.lblCounterDay.Enabled = false;
      this.dtmCounterDay.Enabled = false;
      this.btnQuarterSum.Visible = false;
      this.btnAudit.Visible = false;
      this.btnCloseScheme.Visible = false;
      this.btnEdit.Visible = false;
      this.btnPastTime.Visible = false;
      this.btnPrint.Visible = false;
      this.btnReport.Visible = false;
      this.LoadWorkDistribute();
    }

    private void UpdateVolume()
    {
      Decimal num1 = Convert.ToDecimal(this.dgvEvidence.CurrentRow.Cells["Current"].Value);
      Decimal num2 = Convert.ToDecimal(this.dgvEvidence.CurrentRow.Cells["Past"].Value);
      if (num1 - num2 >= Decimal.Zero)
        this.dgvEvidence.CurrentRow.Cells["Volume"].Value = (object) (num1 - num2);
      else if (num2 - num1 <= new Decimal(5000))
      {
        this.dgvEvidence.CurrentRow.Cells["Volume"].Value = (object) (num1 - num2);
      }
      else
      {
        int num3 = 0;
        Decimal num4 = Convert.ToDecimal(num2);
        while (num4 > Decimal.One)
        {
          ++num3;
          num4 /= new Decimal(10);
        }
        this.dgvEvidence.CurrentRow.Cells["Volume"].Value = (object) (Convert.ToDecimal(Convert.ToDecimal(Math.Pow(10.0, (double) num3))) + Convert.ToDecimal(num1) - Convert.ToDecimal(num2));
      }
    }

    private void FrmCounters_Shown(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      this.monthClosed = KvrplHelper.GetCmpKvrClose(this.company, Options.ComplexPasp.ComplexId, Options.ComplexPrior.IdFk);
      string str = "";
      if (Options.Kvartplata && !Options.Arenda)
        str = string.Format("and sp.Complex.IdFk={0}", (object) Options.Complex.IdFk);
      if (!Options.Kvartplata && Options.Arenda)
        str = string.Format("and sp.Complex.IdFk={0}", (object) Options.ComplexArenda.IdFk);
      this.serviceList = this.session.CreateQuery(string.Format("select distinct s from Service s,ServiceParam sp where sp.Service_id=s.ServiceId and s.Root=0 and s.ServiceId<>0 and sp.Company_id={0}  order by " + Options.SortService, (object) this.company.CompanyId)).List<Service>();
      object obj1 = this.session.CreateQuery("select max(Period.PeriodId) from CounterDetail").UniqueResult();
      object obj2 = this.session.CreateQuery("select max(Period.PeriodId) from DistributeDetail").UniqueResult();
      object obj3 = this.session.CreateQuery("select max(Period.PeriodId) from Evidence").UniqueResult();
      object obj4 = Convert.ToInt32(obj1) > Convert.ToInt32(obj2) ? obj1 : obj2;
      object obj5 = Convert.ToInt32(obj3) > Convert.ToInt32(obj3) ? obj4 : obj3;
      this.periods = this.session.CreateCriteria(typeof (Period)).AddOrder(Order.Desc("PeriodId")).Add((ICriterion) NHibernate.Criterion.Restrictions.Not((ICriterion) NHibernate.Criterion.Restrictions.Eq("PeriodId", (object) 0))).Add((ICriterion) NHibernate.Criterion.Restrictions.Le("PeriodId", (object) Convert.ToInt32(obj5))).List<Period>();
      this.months = this.session.CreateCriteria(typeof (Period)).AddOrder(Order.Desc("PeriodId")).Add((ICriterion) NHibernate.Criterion.Restrictions.Not((ICriterion) NHibernate.Criterion.Restrictions.Eq("PeriodId", (object) 0))).Add((ICriterion) NHibernate.Criterion.Restrictions.Le("PeriodId", (object) Convert.ToInt32(obj5))).List<Period>();
      this.city = Convert.ToInt32(KvrplHelper.BaseValue(1, this.company));
      this.serviceList.Insert(0, new Service()
      {
        ServiceId = (short) 0,
        ServiceName = ""
      });
      this.cmbService.DataSource = (object) this.serviceList;
      this.cmbService.DisplayMember = "ServiceName";
      this.cmbService.ValueMember = "ServiceId";
      IList<BaseCounter> baseCounterList1 = (IList<BaseCounter>) new List<BaseCounter>();
      IList<BaseCounter> baseCounterList2 = this.session.CreateCriteria(typeof (BaseCounter)).AddOrder(Order.Asc("Id")).List<BaseCounter>();
      if ((int) this.level == 3)
        baseCounterList2.RemoveAt(3);
      this.cmbBase.DataSource = (object) baseCounterList2;
      this.cmbBase.DisplayMember = "Name";
      this.cmbBase.ValueMember = "Id";
      if ((int) this.level == 3)
      {
        this.cmbBase.SelectedValue = (object) 2;
        this.btnQuarterSum.Enabled = true;
        this.lblCaption.Text = this.home.Address;
      }
      if ((int) this.level == 2)
      {
        this.cmbBase.SelectedValue = (object) 1;
        this.lblCaption.Text = this.company.CompanyName;
      }
      this.oldBase = 1;
      this.typeCountersList = this.session.CreateCriteria(typeof (TypeCounter)).List<TypeCounter>();
      this.typeCountersList.Insert(0, new TypeCounter()
      {
        TypeCounter_id = (short) 0,
        TypeCounter_name = ""
      });
      this.session.Clear();
      MonthPicker mpCurrentPeriod = this.mpCurrentPeriod;
      DateTime? periodName = Options.Period.PeriodName;
      DateTime dateTime1 = periodName.Value;
      mpCurrentPeriod.Value = dateTime1;
      if (this.setup31 == 1)
      {
        DateTimePicker dtmCounterDay = this.dtmCounterDay;
        periodName = Options.Period.PeriodName;
        DateTime dateTime2 = KvrplHelper.LastDay(periodName.Value);
        dtmCounterDay.Value = dateTime2;
      }
      else
        this.dtmCounterDay.Value = this.setup31 != 2 ? DateTime.Now : DateTime.Now.AddDays(-1.0);
      if (!KvrplHelper.CheckProxy(78, 1, this.company, false) || !Options.CollectiveDevice)
        this.tpWorkDistribute.Parent = (Control) null;
      if (KvrplHelper.CheckProxy(78, 2, this.company, false))
        return;
      this.dgvWorkDistribute.ReadOnly = true;
    }

    private void ведомостьИндивидуальныхИКвартирныхСчетчиковToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(34, 1, this.company, true))
        return;
      int Home = 0;
      if (this.home != null)
        Home = this.home.IdHome;
      int num1 = !KvrplHelper.CheckProxy(32, 2, this.company, false) ? 0 : 1;
      int admin = !Options.Kvartplata || !Options.Arenda ? (!Options.Kvartplata ? num1 + 10 : num1 + 0) : num1 + 20;
      this.Cursor = Cursors.WaitCursor;
      Company company = this.session.Get<Company>((object) this.company.CompanyId);
      try
      {
        CallDll.Rep(this.city, 0, company.Raion.IdRnn, (int) company.CompanyId, Home, 0, Convert.ToInt32(((ToolStripItem) sender).Tag), Options.Period.PeriodId, admin, Options.Alias, Options.Login, Options.Pwd);
      }
      catch (Exception ex)
      {
        int num2 = (int) MessageBox.Show("Невозможно вызвать библиотеку отчетов!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      this.Cursor = Cursors.Default;
    }

    private void btnReport_Click(object sender, EventArgs e)
    {
      this.cmReport.Show((Control) this, 0, 0);
    }

    private void btnPastTime_Click(object sender, EventArgs e)
    {
      if (!this.pastTime)
      {
        this.pastTime = true;
        this.lblPastTime.Visible = true;
        this.btnPastTime.BackColor = Color.DarkOrange;
        this.tmr.Start();
      }
      else
      {
        this.pastTime = false;
        this.tmr.Stop();
        this.lblPastTime.Visible = false;
        this.btnPastTime.BackColor = this.BackColor;
      }
      if (this.tcntrlCounters.SelectedTab == this.tpAudit)
        this.LoadAudit();
      if (this.tcntrlCounters.SelectedTab != this.tpScheme)
        return;
      this.LoadScheme();
    }

    private void tmr_Tick(object sender, EventArgs e)
    {
      if (this.lblPastTime.ForeColor == Color.DarkOrange)
        this.lblPastTime.ForeColor = this.BackColor;
      else
        this.lblPastTime.ForeColor = Color.DarkOrange;
    }

    private void cbArchive_CheckedChanged(object sender, EventArgs e)
    {
      if (this.tcntrlCounters.SelectedTab == this.tpCounters)
        this.LoadCounters();
      if (this.tcntrlCounters.SelectedTab == this.tpAudit)
        this.LoadAudit();
      if (this.tcntrlCounters.SelectedTab == this.tpPeriod)
        this.LoadEvidence();
      if (this.tcntrlCounters.SelectedTab == this.tpDetail)
        this.LoadDetail();
      if (this.tcntrlCounters.SelectedTab == this.tpDetailEvidence)
        this.LoadDetailEvidence();
      if (this.tcntrlCounters.SelectedTab == this.tpDistribute)
        this.LoadDistribute();
      if (this.tcntrlCounters.SelectedTab == this.tpDstrDetail)
        this.LoadDstrDetail();
      if (this.tcntrlCounters.SelectedTab != this.tpScheme)
        return;
      this.LoadScheme();
    }

    private void tmrEvidence_Tick(object sender, EventArgs e)
    {
      if (this.editEvidence)
      {
        if (this.lblEdit.ForeColor == Color.DarkOrange)
          this.lblEdit.ForeColor = this.pnBtn.BackColor;
        else
          this.lblEdit.ForeColor = Color.DarkOrange;
      }
      else
        this.lblEdit.ForeColor = this.pnBtn.BackColor;
    }

    private void FrmCounters_Load(object sender, EventArgs e)
    {
      this.setup31 = Convert.ToInt32(KvrplHelper.BaseValue(31, this.company));
    }

    private void LoadCounters()
    {
      this.btnAdd.Enabled = true;
      this.btnDelete.Enabled = true;
      this.btnSave.Enabled = false;
      this.cbArchive.Visible = true;
      this.pnTools.Height = 73;
      this.LoadCountersList(false);
      this.dgvCounters.Columns.Clear();
      this.dgvCounters.DataSource = (object) null;
      this.dgvCounters.DataSource = (object) this.counters;
      this.session.Clear();
      this.SetViewCounters();
      this.MySettingsCounters.GridName = "Counters";
      this.LoadSettingsCounters();
    }

    private void LoadSettingsCounters()
    {
      this.MySettingsCounters.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvCounters.Columns)
        this.MySettingsCounters.GetMySettings(column);
    }

    private void dgvCounters_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsCounters.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsCounters.Columns[this.MySettingsCounters.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsCounters.Save();
    }

    private void SetViewCounters()
    {
      this.session = Domain.CurrentSession;
      this.dgvCounters.Columns["Series"].HeaderText = "Серия";
      this.dgvCounters.Columns["Notice"].HeaderText = "Примечания";
      this.dgvCounters.Columns["ServiceName"].HeaderText = "Услуга";
      this.dgvCounters.Columns["ArchivesDate"].HeaderText = "Дата поступления в архив";
      this.dgvCounters.Columns["ServiceName"].DisplayIndex = 0;
      this.dgvCounters.Columns["Series"].DisplayIndex = 2;
      this.dgvCounters.Columns["Notice"].DisplayIndex = 3;
      this.dgvCounters.Columns["ArchivesDate"].DisplayIndex = 4;
      this.dgvCounters.Columns["ArchivesDate"].ReadOnly = true;
      this.dgvCounters.Columns["Notice"].Width = 300;
      this.dgvCounters.Columns["AllInfo"].Visible = false;
      this.dgvCounters.Columns["CounterId"].Visible = false;
      this.dgvCounters.Columns["CounterNum"].Visible = false;
      this.dgvCounters.Columns["AdrAndNum"].Visible = false;
      this.dgvCounters.Columns["FlatAndNum"].Visible = false;
      this.dgvCounters.Columns["AuditDate"].Visible = false;
      this.dgvCounters.Columns["CounterNum1"].Visible = false;
      this.dgvCounters.Columns["MainCounterInfo"].Visible = false;
      KvrplHelper.ViewEdit(this.dgvCounters);
      if ((uint) Convert.ToInt16(this.cmbService.SelectedValue) > 0U)
        this.dgvCounters.Columns["ServiceName"].Visible = false;
      DataGridViewComboBoxColumn viewComboBoxColumn1 = new DataGridViewComboBoxColumn();
      IList<CounterLocation> counterLocationList = this.session.CreateCriteria(typeof (CounterLocation)).List<CounterLocation>();
      counterLocationList.Insert(0, new CounterLocation()
      {
        CntrLocationId = 0,
        CntrLocationName = ""
      });
      KvrplHelper.AddComboBoxColumn(this.dgvCounters, 1, (IList) this.typeCountersList, "TypeCounter_id", "TypeCounter_name", "Тип счетчика", "Type", 140, 140);
      KvrplHelper.AddTextBoxColumn(this.dgvCounters, 2, "Номер счетчика", "CounterNum", 90, false);
      KvrplHelper.AddComboBoxColumn(this.dgvCounters, 4, (IList) new List<Counter>(), "CounterId", "MainCounterInfo", "Головной счетчик", "MainCounters", 120, 120);
      KvrplHelper.AddTextBoxColumn(this.dgvCounters, 5, "Коэфф-т трансфор-и", "CoeffTrans", 90, false);
      KvrplHelper.AddComboBoxColumn(this.dgvCounters, 6, (IList) counterLocationList, "CntrLocationId", "CntrLocationName", "Тип расположения", "Location", 140, 140);
      KvrplHelper.AddMaskDateColumn(this.dgvCounters, 8, "Дата установки", "SetDate");
      KvrplHelper.AddTextBoxColumn(this.dgvCounters, 9, "Начальные показания", "EvidenceStart", 90, false);
      KvrplHelper.AddMaskDateColumn(this.dgvCounters, 10, "Дата поверки", "AuditDate");
      KvrplHelper.AddMaskDateColumn(this.dgvCounters, 11, "Дата снятия", "RemoveDate");
      DataGridViewComboBoxColumn viewComboBoxColumn2 = new DataGridViewComboBoxColumn();
      viewComboBoxColumn2.DropDownWidth = 180;
      viewComboBoxColumn2.Width = 180;
      viewComboBoxColumn2.MaxDropDownItems = 7;
      viewComboBoxColumn2.DisplayStyleForCurrentCellOnly = true;
      this.homes = (IList<Home>) new List<Home>();
      if ((int) this.level == 2)
        this.homes = this.session.CreateQuery(string.Format("select h from Home h left join fetch h.Str, HomeLink hl where hl.Home=h and hl.Company.CompanyId={0} order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)", (object) this.company.CompanyId)).List<Home>();
      if ((int) this.level == 3)
        this.homes = this.session.CreateQuery(string.Format("select h from Home h left join fetch h.Str, HomeLink hl where hl.Home=h and hl.Company.CompanyId={0} and hl.Home.IdHome={1} order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)", (object) this.company.CompanyId, (object) this.home.IdHome)).List<Home>();
      string str1 = string.Format(" and ls.Complex.IdFk in ({0},{1})", (object) Options.Complex.IdFk, (object) Options.ComplexArenda.IdFk);
      if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
      {
        if (!Options.Kvartplata)
          str1 = string.Format(" and ls.Complex.IdFk={0}", (object) Options.ComplexArenda.IdFk);
        if (!Options.Arenda)
          str1 = string.Format(" and ls.Complex.IdFk={0}", (object) Options.Complex.IdFk);
        this.clients = (IList<LsClient>) new List<LsClient>();
        if ((int) this.level == 2)
          this.clients = this.session.CreateQuery(string.Format("select distinct ls from LsClient ls left join fetch ls.Flat where ls.Company.CompanyId = {0} " + str1 + " order by DBA.lengthhome(ls.Flat.NFlat)", (object) this.company.CompanyId)).List<LsClient>();
        if ((int) this.level == 3)
          this.clients = this.session.CreateQuery(string.Format("select distinct ls from LsClient ls left join fetch ls.Flat where ls.Home.IdHome = {0} " + str1 + " order by DBA.lengthhome(ls.Flat.NFlat)", (object) this.home.IdHome)).List<LsClient>();
        viewComboBoxColumn2.DataSource = (object) this.clients;
        viewComboBoxColumn2.ValueMember = "ClientId";
        viewComboBoxColumn2.DisplayMember = "SmallAddress";
        viewComboBoxColumn2.HeaderText = "Лицевой";
        viewComboBoxColumn2.Name = "LicAddress";
        this.dgvCounters.Columns.Insert(0, (DataGridViewColumn) viewComboBoxColumn2);
      }
      if ((Convert.ToInt32(this.cmbBase.SelectedValue) != 2 || (int) this.level == 2) && Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
        KvrplHelper.AddComboBoxColumn(this.dgvCounters, 0, (IList) this.homes, "IdHome", "Address", "Адрес", "Address", 180, 180);
      IList<LsClient> lsClientList1 = (IList<LsClient>) new List<LsClient>();
      foreach (DataGridViewRow row1 in (IEnumerable) this.dgvCounters.Rows)
      {
        DataGridViewRow row = row1;
        if (((Counter) row.DataBoundItem).TypeCounter != null)
          row.Cells["Type"].Value = (object) ((Counter) row.DataBoundItem).TypeCounter.TypeCounter_id;
        if (((Counter) row.DataBoundItem).Location != null)
          row.Cells["Location"].Value = (object) ((Counter) row.DataBoundItem).Location.CntrLocationId;
        if (Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
        {
          if (((int) this.level == 2 || Convert.ToInt32(this.cmbBase.SelectedValue) != 2) && ((Counter) row.DataBoundItem).Home != null)
          {
            row.Cells["Address"].Value = (object) ((Counter) row.DataBoundItem).Home.IdHome;
            if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
            {
              if (lsClientList1.Count == 0 || lsClientList1[0].Home.IdHome != ((Counter) row.DataBoundItem).Home.IdHome)
              {
                IList<LsClient> lsClientList2 = (IList<LsClient>) new List<LsClient>();
                lsClientList1 = this.session.CreateQuery(string.Format("select distinct ls from LsClient ls left join fetch ls.Flat where ls.Home.IdHome = {0} " + str1 + " order by DBA.lengthhome(ls.Flat.NFlat)", (object) ((Counter) row.DataBoundItem).Home.IdHome)).List<LsClient>();
              }
              row.Cells["LicAddress"] = (DataGridViewCell) new DataGridViewComboBoxCell()
              {
                DisplayStyleForCurrentCellOnly = true,
                ValueMember = "ClientId",
                DisplayMember = "SmallAddress",
                DataSource = (object) lsClientList1
              };
              row.Cells["LicAddress"].Value = (object) ((Counter) row.DataBoundItem).LsClient.ClientId;
            }
            if (Convert.ToInt32(this.cmbBase.SelectedValue) == 3)
            {
              this.dgvCounters.Columns.Add("Flat", "Квартиры");
              this.dgvCounters.Columns["Flat"].DisplayIndex = 1;
              IList list = this.session.CreateSQLQuery(string.Format("select distinct f.NFLAT  from cntrCounter coun inner join cntrRelation rel on rel.Counter_id=coun.Counter_id inner join lsClient cl on cl.Client_id=rel.Client_id inner join FLATS f on f.IDFLAT=cl.IdFlat  where coun.Counter_id={0} and f.idhome={1}", (object) ((Counter) row.DataBoundItem).CounterId, (object) ((Counter) row.DataBoundItem).Home.IdHome)).List();
              string str2 = "";
              foreach (object obj in (IEnumerable) list)
                str2 = str2 + " " + obj.ToString();
              row.Cells["Flat"].Value = (object) str2;
            }
          }
          if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2 && ((Counter) row.DataBoundItem).LsClient != null)
            row.Cells["LicAddress"].Value = (object) ((Counter) row.DataBoundItem).LsClient.ClientId;
        }
        row.Cells["CounterNum"].Value = (object) ((Counter) row.DataBoundItem).CounterNum;
        row.Cells["SetDate"].Value = (object) ((Counter) row.DataBoundItem).SetDate;
        row.Cells["RemoveDate"].Value = (object) ((Counter) row.DataBoundItem).RemoveDate;
        row.Cells["AuditDate"].Value = (object) ((Counter) row.DataBoundItem).AuditDate;
        row.Cells["EvidenceStart"].Value = (object) ((Counter) row.DataBoundItem).EvidenceStart;
        row.Cells["CoeffTrans"].Value = (object) ((Counter) row.DataBoundItem).CoeffTrans;
        List<Counter> counterList = new List<Counter>();
        counterList.Add(new Counter()
        {
          MainCounter = (Counter) null
        });
        counterList.AddRange((IEnumerable<Counter>) this.counters.Where<Counter>((Func<Counter, bool>) (x => x.CounterId != ((Counter) row.DataBoundItem).CounterId)).ToList<Counter>());
        ((DataGridViewComboBoxCell) this.dgvCounters["MainCounters", row.Index]).DataSource = (object) counterList;
        Counter dataBoundItem = (Counter) row.DataBoundItem;
        if (dataBoundItem.MainCounter != null)
          row.Cells["MainCounters"].Value = (object) dataBoundItem.MainCounter.CounterId;
      }
      this.session.Clear();
    }

    private void dgvCounters_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (Convert.ToInt32(this.cmbBase.SelectedValue) == 4 || (int) this.level != 2 || this.dgvCounters.CurrentCell.ColumnIndex != this.dgvCounters.Rows[this.dgvCounters.CurrentRow.Index].Cells["Address"].ColumnIndex || Convert.ToInt32(this.cmbBase.SelectedValue) != 2)
        return;
      this.dgvCounters.CommitEdit(DataGridViewDataErrorContexts.Commit);
      this.session = Domain.CurrentSession;
      IList<LsClient> lsClientList1 = (IList<LsClient>) new List<LsClient>();
      string str = string.Format(" and ls.Complex.IdFk in ({0},{1})", (object) Options.Complex.IdFk, (object) Options.ComplexArenda.IdFk);
      if (!Options.Kvartplata)
        str = string.Format(" and ls.Complex.IdFk={0}", (object) Options.ComplexArenda.IdFk);
      if (!Options.Arenda)
        str = string.Format(" and ls.Complex.IdFk={0}", (object) Options.Complex.IdFk);
      IList<LsClient> lsClientList2 = this.session.CreateQuery(string.Format("select distinct ls from LsClient ls left join fetch ls.Flat where ls.Home.IdHome = {0} " + str + " order by DBA.lengthhome(ls.Flat.NFlat)", this.dgvCounters.CurrentRow.Cells["Address"].Value)).List<LsClient>();
      this.dgvCounters.CurrentRow.Cells["LicAddress"] = (DataGridViewCell) new DataGridViewComboBoxCell()
      {
        DisplayStyleForCurrentCellOnly = true,
        ValueMember = "ClientId",
        DisplayMember = "SmallAddress",
        DataSource = (object) lsClientList2
      };
    }

    private void LoadCountersList(bool evidence)
    {
      this.session.Clear();
      this.session = Domain.CurrentSession;
      this.counters = (IList<Counter>) new List<Counter>();
      string str1 = "";
      string str2 = "";
      string str3 = "";
      string str4 = "";
      if ((uint) Convert.ToInt16(this.cmbService.SelectedValue) > 0U)
        str1 = " and s.Service.ServiceId={3}";
      if (this.tcntrlCounters.SelectedTab == this.tpCounters)
      {
        str2 = " isnull(s.ArchivesDate,'2999-12-31') desc,";
        if (this.tcntrlCounters.SelectedTab == this.tpCounters && !this.cbArchive.Checked)
          str3 = " and isnull(s.ArchivesDate,'2999-12-31')>='{5}'";
      }
      if ((int) this.level == 2)
        str2 += " st.NameStr,DBA.lengthhome(s.Home.NHome),s.Home.HomeKorp,";
      if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
      {
        str4 = string.Format(" and s.LsClient.Complex.IdFk in ({0},{1})", (object) Options.Complex.IdFk, (object) Options.ComplexArenda.IdFk);
        if (!Options.Kvartplata)
          str4 = string.Format(" and s.LsClient.Complex.IdFk={0}", (object) Options.ComplexArenda.IdFk);
        if (!Options.Arenda)
          str4 = string.Format(" and s.LsClient.Complex.IdFk={0}", (object) Options.Complex.IdFk);
        str2 += " DBA.lengthhome(s.LsClient.Flat.NFlat),";
      }
      string str5 = !(Options.SortService == " s.ServiceId") ? " s.Service.ServiceName" : " s.Service.ServiceId";
      if (evidence)
        str1 += " and s in (select e.Counter from Evidence e where e.Period.PeriodId={4})";
      this.session = Domain.CurrentSession;
      DateTime dateTime;
      if ((int) this.level == 3)
      {
        ISession session = this.session;
        string format = "select s from Counter s left join fetch s.Service left join fetch s.TypeCounter where s.Complex.ComplexId={0} and s.Company.CompanyId={6} and s.Home.IdHome={1} and s.BaseCounter.Id={2}" + str4 + str3 + str1 + " order by" + str2 + str5 + ",regulatefld(s.CounterNum)";
        object[] objArray = new object[7]{ (object) Options.Complex.ComplexId, (object) this.home.IdHome, this.cmbBase.SelectedValue, this.cmbService.SelectedValue, (object) Options.Period.PeriodId, null, null };
        int index1 = 5;
        dateTime = this.monthClosed.PeriodName.Value;
        string baseFormat = KvrplHelper.DateToBaseFormat(dateTime.AddMonths(2));
        objArray[index1] = (object) baseFormat;
        int index2 = 6;
        // ISSUE: variable of a boxed type
        short companyId = this.company.CompanyId;
        objArray[index2] = (object) companyId;
        string queryString = string.Format(format, objArray);
        this.counters = session.CreateQuery(queryString).List<Counter>();
      }
      if ((int) this.level != 2)
        return;
      ISession session1 = this.session;
      string format1 = "select s from Counter s left join fetch s.Service left join fetch s.TypeCounter left join fetch s.Home h left join fetch h.Str st where s.Complex.ComplexId={0} and s.Company.CompanyId={1} and s.BaseCounter.Id={2} " + str4 + str3 + str1 + " order by" + str2 + str5 + ",regulatefld(s.CounterNum)";
      object[] objArray1 = new object[6]{ (object) Options.Complex.ComplexId, (object) this.company.CompanyId, this.cmbBase.SelectedValue, this.cmbService.SelectedValue, (object) Options.Period.PeriodId, null };
      int index = 5;
      dateTime = this.monthClosed.PeriodName.Value;
      string baseFormat1 = KvrplHelper.DateToBaseFormat(dateTime.AddMonths(2));
      objArray1[index] = (object) baseFormat1;
      string queryString1 = string.Format(format1, objArray1);
      this.counters = session1.CreateQuery(queryString1).List<Counter>();
    }

    private void InsertCounter()
    {
      this.insertRecord = true;
      Counter counter = new Counter();
      counter.BaseCounter = (BaseCounter) this.cmbBase.SelectedItem;
      counter.Service = (Service) this.cmbService.SelectedItem;
      counter.CoeffTrans = 1.0;
      IList<Counter> counterList = (IList<Counter>) new List<Counter>();
      if ((uint) this.dgvCounters.Rows.Count > 0U)
        counterList = (IList<Counter>) (this.dgvCounters.DataSource as List<Counter>);
      counterList.Add(counter);
      this.dgvCounters.Columns.Clear();
      this.dgvCounters.DataSource = (object) null;
      this.dgvCounters.DataSource = (object) counterList;
      this.SetViewCounters();
      this.LoadSettingsCounters();
      if (Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
        this.dgvCounters.CurrentCell = this.dgvCounters.Rows[this.dgvCounters.Rows.Count - 1].Cells[0];
      else
        this.dgvCounters.CurrentCell = this.dgvCounters.Rows[this.dgvCounters.Rows.Count - 1].Cells[2];
    }

    private void SaveAllCounters()
    {
      this.dgvCounters.EndEdit();
      foreach (DataGridViewRow row in (IEnumerable) this.dgvCounters.Rows)
      {
        this.dgvCounters.CurrentCell = row.Cells["CounterNum"];
        row.Selected = true;
        if (((Counter) row.DataBoundItem).IsEdit)
          this.SaveCounter();
        ((Counter) row.DataBoundItem).IsEdit = false;
      }
      this.btnAdd.Enabled = true;
      this.btnDelete.Enabled = true;
    }

    private void SaveCounter()
    {
      if (this.dgvCounters.Rows.Count <= 0 || this.dgvCounters.CurrentRow == null)
        return;
      Counter dataBoundItem = (Counter) this.dgvCounters.CurrentRow.DataBoundItem;
      this.insertRecord = dataBoundItem.CounterId == 0;
      if (this.dgvCounters.CurrentRow.Cells["CounterNum"].Value == null)
      {
        int num1 = (int) MessageBox.Show("Введите номер", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (dataBoundItem.Series == null)
          dataBoundItem.Series = "";
        if (dataBoundItem.Notice == null)
          dataBoundItem.Notice = "";
        int num2 = this.dgvCounters.CurrentRow.Cells["Type"].Value == null ? 0 : ((uint) Convert.ToInt32(this.dgvCounters.CurrentRow.Cells["Type"].Value) > 0U ? 1 : 0);
        dataBoundItem.TypeCounter = num2 == 0 ? (TypeCounter) null : this.session.Get<TypeCounter>(this.dgvCounters.CurrentRow.Cells["Type"].Value);
        DateTime? nullable1 = dataBoundItem.RemoveDate;
        DateTime dateTime1 = KvrplHelper.LastDay(DateTime.Now);
        if (nullable1.HasValue && nullable1.GetValueOrDefault() > dateTime1)
        {
          int num3 = (int) MessageBox.Show("Дата снятия не может быть проставлена в будущие периоды", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.session.Clear();
        }
        else
        {
          nullable1 = dataBoundItem.SetDate;
          DateTime dateTime2 = DateTime.Now.AddYears(-3);
          int num3;
          if ((nullable1.HasValue ? (nullable1.GetValueOrDefault() <= dateTime2 ? 1 : 0) : 0) == 0)
          {
            nullable1 = dataBoundItem.SetDate;
            DateTime dateTime3 = DateTime.Now.AddYears(3);
            num3 = nullable1.HasValue ? (nullable1.GetValueOrDefault() >= dateTime3 ? 1 : 0) : 0;
          }
          else
            num3 = 1;
          if (num3 != 0 && MessageBox.Show("Дата установки отличается от текущей более, чем на 3 года. Продолжить сохранение? ", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
          {
            this.session.Clear();
          }
          else
          {
            nullable1 = dataBoundItem.AuditDate;
            DateTime dateTime3 = DateTime.Now.AddYears(-3);
            int num4;
            if ((nullable1.HasValue ? (nullable1.GetValueOrDefault() <= dateTime3 ? 1 : 0) : 0) == 0)
            {
              nullable1 = dataBoundItem.AuditDate;
              DateTime dateTime4 = DateTime.Now.AddYears(3);
              num4 = nullable1.HasValue ? (nullable1.GetValueOrDefault() >= dateTime4 ? 1 : 0) : 0;
            }
            else
              num4 = 1;
            if (num4 != 0 && MessageBox.Show("Дата поверки отличается от текущей более, чем на 3 года. Продолжить сохранение? ", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
              this.session.Clear();
            }
            else
            {
              nullable1 = dataBoundItem.RemoveDate;
              DateTime dateTime4 = DateTime.Now.AddYears(-3);
              int num5;
              if ((nullable1.HasValue ? (nullable1.GetValueOrDefault() <= dateTime4 ? 1 : 0) : 0) == 0)
              {
                nullable1 = dataBoundItem.RemoveDate;
                DateTime dateTime5 = DateTime.Now.AddYears(3);
                num5 = nullable1.HasValue ? (nullable1.GetValueOrDefault() >= dateTime5 ? 1 : 0) : 0;
              }
              else
                num5 = 1;
              if (num5 != 0 && MessageBox.Show("Дата снятия отличается от текущей более, чем на 3 года. Продолжить сохранение? ", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
              {
                this.session.Clear();
              }
              else
              {
                dataBoundItem.Company = this.company;
                dataBoundItem.UName = Options.Login;
                dataBoundItem.DEdit = DateTime.Now.Date;
                this.session = Domain.CurrentSession;
                if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
                {
                  if (this.dgvCounters.CurrentRow.Cells["LicAddress"].Value != null)
                  {
                    dataBoundItem.LsClient = this.session.Get<LsClient>(this.dgvCounters.CurrentRow.Cells["LicAddress"].Value);
                    dataBoundItem.Home = dataBoundItem.LsClient.Home;
                  }
                  else
                  {
                    int num6 = (int) MessageBox.Show("Введите адрес", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                  }
                }
                if (Convert.ToInt32(this.cmbBase.SelectedValue) == 1 || Convert.ToInt32(this.cmbBase.SelectedValue) == 3)
                {
                  if (this.dgvCounters.CurrentRow.Cells["Address"].Value != null)
                  {
                    dataBoundItem.Home = this.session.Get<Home>(this.dgvCounters.CurrentRow.Cells["Address"].Value);
                  }
                  else
                  {
                    int num6 = (int) MessageBox.Show("Введите адрес", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                  }
                }
                this.session.Clear();
                dataBoundItem.Complex = Options.Complex;
                try
                {
                  if (this.insertRecord || !this.insertRecord && (object) dataBoundItem.CounterNum != this.dgvCounters.CurrentRow.Cells["CounterNum"].Value)
                  {
                    IList<Counter> counterList = (IList<Counter>) new List<Counter>();
                    if ((Convert.ToInt32(this.cmbBase.SelectedValue) == 2 ? (ICollection<Counter>) this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("LsClient", (object) dataBoundItem.LsClient)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) dataBoundItem.Service)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("CounterNum", (object) Convert.ToString(this.dgvCounters.CurrentRow.Cells["CounterNum"].Value))).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter.Id", (object) ((BaseCounter) this.cmbBase.SelectedItem).Id)).Add((ICriterion) NHibernate.Criterion.Restrictions.IsNull("ArchivesDate")).List<Counter>() : (ICollection<Counter>) this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.IsNull("LsClient")).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Home", (object) dataBoundItem.Home)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) dataBoundItem.Service)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("CounterNum", (object) Convert.ToString(this.dgvCounters.CurrentRow.Cells["CounterNum"].Value))).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter.Id", (object) ((BaseCounter) this.cmbBase.SelectedItem).Id)).Add((ICriterion) NHibernate.Criterion.Restrictions.IsNull("ArchivesDate")).List<Counter>()).Count > 0)
                    {
                      int num6 = (int) MessageBox.Show("Уже есть счетчик с таким номером и адресом по данной услуге!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                      return;
                    }
                  }
                  dataBoundItem.CounterNum = this.dgvCounters.CurrentRow.Cells["CounterNum"].Value.ToString().Replace(" ", string.Empty);
                }
                catch (Exception ex)
                {
                  ex.GetType();
                  int num6 = (int) MessageBox.Show("Некорректный номер счетчика", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  return;
                }
                short? nullable2 = new short?(Convert.ToInt16(this.session.CreateQuery("select CrossServ.ServiceId from CrossService where Company.CompanyId=:cmp and Service.ServiceId=:serv and CrossType.CrossTypeId=1").SetParameter<short>("cmp", this.company.CompanyId).SetParameter<short>("serv", dataBoundItem.Service.ServiceId).UniqueResult()));
                int num7;
                if (dataBoundItem.BaseCounter.Id != 1 && dataBoundItem.BaseCounter.Id != 4)
                {
                  short? nullable3 = nullable2;
                  int? nullable4 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
                  int num6 = 0;
                  if ((nullable4.GetValueOrDefault() == num6 ? (!nullable4.HasValue ? 1 : 0) : 1) != 0)
                  {
                    num7 = Convert.ToInt32(this.cmbBase.SelectedValue) != 3 ? 1 : 0;
                    goto label_42;
                  }
                }
                num7 = 0;
label_42:
                if (num7 != 0)
                {
                  int num8 = (int) MessageBox.Show("Невозможно сохранить изменения. Существует связанная услуга по показаниям.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                  IList<Evidence> evidenceList1 = (IList<Evidence>) new List<Evidence>();
                  IList<Evidence> evidenceList2 = this.session.CreateCriteria(typeof (Evidence)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Counter", (object) dataBoundItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Le("Period", (object) KvrplHelper.GetCmpKvrClose(this.company, Options.ComplexPasp.ComplexId, Options.ComplexPrior.IdFk))).List<Evidence>();
                  try
                  {
                    if (!this.insertRecord && this.session.Get<Counter>((object) dataBoundItem.CounterId).CoeffTrans != Convert.ToDouble(KvrplHelper.ChangeSeparator(this.dgvCounters.CurrentRow.Cells["CoeffTrans"].Value.ToString())) && evidenceList2.Count > 0)
                    {
                      int num6 = (int) MessageBox.Show("Невозможно поменять коэффициент. Есть показания в закрытом периоде", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                      return;
                    }
                    dataBoundItem.CoeffTrans = Convert.ToDouble(KvrplHelper.ChangeSeparator(this.dgvCounters.CurrentRow.Cells["CoeffTrans"].Value.ToString()));
                    if (dataBoundItem.CoeffTrans == 0.0)
                      dataBoundItem.CoeffTrans = 1.0;
                  }
                  catch (Exception ex)
                  {
                    int num6 = (int) MessageBox.Show("Некорректный коэффициент", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                  }
                  this.session.Clear();
                  if (this.insertRecord || evidenceList2.Count == 0)
                  {
                    int num6 = this.dgvCounters.CurrentRow.Cells["Location"].Value == null ? 0 : ((uint) Convert.ToInt32(this.dgvCounters.CurrentRow.Cells["Location"].Value) > 0U ? 1 : 0);
                    dataBoundItem.Location = num6 == 0 ? (CounterLocation) null : this.session.Get<CounterLocation>(this.dgvCounters.CurrentRow.Cells["Location"].Value);
                  }
                  if (this.dgvCounters.CurrentRow.Cells["EvidenceStart"].Value == null)
                  {
                    int num9 = (int) MessageBox.Show("Введите начальные показания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  }
                  else
                  {
                    try
                    {
                      dataBoundItem.EvidenceStart = Convert.ToDouble(KvrplHelper.ChangeSeparator(this.dgvCounters.CurrentRow.Cells["EvidenceStart"].Value.ToString()));
                    }
                    catch (Exception ex)
                    {
                    }
                    try
                    {
                      if (this.dgvCounters.CurrentRow.Cells["SetDate"].Value != null)
                        dataBoundItem.SetDate = !(this.dgvCounters.CurrentRow.Cells["SetDate"].Value.ToString() != "  .  .") ? new DateTime?() : new DateTime?(Convert.ToDateTime(this.dgvCounters.CurrentRow.Cells["SetDate"].Value));
                      if (this.dgvCounters.CurrentRow.Cells["RemoveDate"].Value != null)
                        dataBoundItem.RemoveDate = !(this.dgvCounters.CurrentRow.Cells["RemoveDate"].Value.ToString() != "  .  .") ? new DateTime?() : new DateTime?(Convert.ToDateTime(this.dgvCounters.CurrentRow.Cells["RemoveDate"].Value));
                      if (this.dgvCounters.CurrentRow.Cells["AuditDate"].Value != null)
                        dataBoundItem.AuditDate = !(this.dgvCounters.CurrentRow.Cells["AuditDate"].Value.ToString() != "  .  .") ? new DateTime?() : new DateTime?(Convert.ToDateTime(this.dgvCounters.CurrentRow.Cells["AuditDate"].Value));
                    }
                    catch (Exception ex)
                    {
                      int num6 = (int) MessageBox.Show("Некорректно введена дата", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                      return;
                    }
                    try
                    {
                      ITransaction transaction = this.session.BeginTransaction();
                      if (this.insertRecord)
                      {
                        this.insertRecord = false;
                        IList<int> intList = this.session.CreateSQLQuery("select DBA.gen_id('cntrCounter',1)").List<int>();
                        dataBoundItem.CounterId = intList[0];
                        this.session.Save((object) dataBoundItem);
                      }
                      else
                        this.session.Update((object) dataBoundItem);
                      this.session.Flush();
                      transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                      int num6 = (int) MessageBox.Show("Невозможно сохранить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                      KvrplHelper.WriteLog(ex, (LsClient) null);
                    }
                    this.session.Clear();
                  }
                }
              }
            }
          }
        }
      }
    }

    private void DeleteCounter()
    {
      if (this.dgvCounters.Rows.Count <= 0 || this.dgvCounters.CurrentRow == null || MessageBox.Show("Удалить счетчик?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      this.session = Domain.CurrentSession;
      Counter dataBoundItem = (Counter) this.dgvCounters.CurrentRow.DataBoundItem;
      if (this.session.CreateCriteria(typeof (Evidence)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Counter", (object) dataBoundItem)).List<Evidence>().Count > 0)
      {
        int num1 = (int) MessageBox.Show("Удаление невозможно. У счетчика есть показания.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        try
        {
          this.session.Delete((object) dataBoundItem);
          this.session.Flush();
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show("невозможно удалить счетчик", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
    }

    private void dgvCounters_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (((DataGridView) sender).DataSource == null)
        return;
      DataGridViewRow row = ((DataGridView) sender).Rows[e.RowIndex];
      int num;
      if (((Counter) row.DataBoundItem).ArchivesDate.HasValue)
      {
        DateTime? archivesDate = ((Counter) row.DataBoundItem).ArchivesDate;
        DateTime dateTime = Options.Period.PeriodName.Value;
        num = archivesDate.HasValue ? (archivesDate.GetValueOrDefault() > dateTime ? 1 : 0) : 0;
      }
      else
        num = 1;
      if (num != 0)
        row.DefaultCellStyle.BackColor = Color.PapayaWhip;
      else
        row.DefaultCellStyle.ForeColor = Color.Gray;
    }

    private void btnArchive_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(42, 2, this.company, true) || (this.dgvCounters.Rows.Count <= 0 || this.dgvCounters.CurrentRow == null || this.dgvCounters.CurrentRow.Index < 0))
        return;
      if (((Counter) this.dgvCounters.CurrentRow.DataBoundItem).ArchivesDate.HasValue)
      {
        int num = (int) MessageBox.Show("Счетчик уже занесен в архив", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.mcArchive.MinDate = this.monthClosed.PeriodName.Value.AddMonths(1);
        this.mcArchive.Parent = (Control) this.dgvCounters;
        this.mcArchive.Visible = true;
        this.mcArchive.Show();
      }
    }

    private void ToArchive()
    {
      Counter dataBoundItem = (Counter) this.dgvCounters.CurrentRow.DataBoundItem;
      if (MessageBox.Show("Занести текущий счетчик в архив c " + this.dateArchive.ToShortDateString() + "?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK || dataBoundItem.BaseCounter.Id != 2 && MessageBox.Show("Записи по всем привязанным лицевым будут закрыты. Занести в архив?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
        return;
      this.session = Domain.CurrentSession;
      Period period = Options.Period;
      if (this.session.CreateCriteria(typeof (Evidence)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Counter", (object) dataBoundItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Ge("DEnd", (object) this.dateArchive)).List<Evidence>().Count > 0)
      {
        int num1 = (int) MessageBox.Show("Невозможно сделать счетчик архивным с выбранной даты. Существуют показания", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        dataBoundItem.ArchivesDate = new DateTime?(this.dateArchive);
        dataBoundItem.UName = Options.Login;
        dataBoundItem.DEdit = DateTime.Now;
        if (dataBoundItem.BaseCounter.Id == 3)
        {
          try
          {
            ITransaction transaction = this.session.BeginTransaction();
            this.session.Update((object) dataBoundItem);
            this.session.Flush();
            this.session.CreateQuery(string.Format("update CounterRelation set DEnd='{0}',UName='{2}',DEdit=today() where Counter.CounterId={1} and DEnd>'{0}'", (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(this.dateArchive.AddMonths(-1))), (object) dataBoundItem.CounterId, (object) Options.Login)).ExecuteUpdate();
            transaction.Commit();
          }
          catch (Exception ex)
          {
            int num2 = (int) MessageBox.Show("Невозможно занести счетчик в архив", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
        }
        else
        {
          CounterScheme counterScheme = this.session.CreateQuery("select cs from CounterScheme cs where cs.Counter.CounterId=:cid and DEnd=:date ").SetParameter<int>("cid", dataBoundItem.CounterId).SetParameter<DateTime>("date", new DateTime(2999, 12, 31)).UniqueResult<CounterScheme>();
          if (counterScheme != null)
            counterScheme.DEnd = this.dateArchive.AddDays(-1.0);
          try
          {
            ITransaction transaction = this.session.BeginTransaction();
            this.session.Update((object) dataBoundItem);
            if (counterScheme != null)
              this.session.Update((object) counterScheme);
            this.session.Flush();
            this.session.CreateQuery(string.Format("update CounterRelation set DEnd='{0}',UName='{2}',DEdit=today() where Counter.CounterId={1} and DEnd>'{0}'", (object) KvrplHelper.DateToBaseFormat(KvrplHelper.LastDay(this.dateArchive.AddMonths(-1))), (object) dataBoundItem.CounterId, (object) Options.Login)).ExecuteUpdate();
            transaction.Commit();
          }
          catch (Exception ex)
          {
            int num2 = (int) MessageBox.Show("Невозможно занести счетчик в архив", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
        }
      }
    }

    private void mcArchive_DateChanged(object sender, DateRangeEventArgs e)
    {
    }

    private void mcArchive_DateSelected(object sender, DateRangeEventArgs e)
    {
      this.dateArchive = this.mcArchive.SelectionRange.End;
      this.mcArchive.Visible = false;
      this.ToArchive();
    }

    private void dgvCounters_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.btnSave.Enabled = true;
      if (this.dgvCounters.CurrentCell == this.dgvCounters.CurrentRow.Cells["CounterNum"] && !this.insertRecord)
      {
        this.session = Domain.CurrentSession;
        IList<Evidence> evidenceList = this.session.CreateCriteria(typeof (Evidence)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Counter", (object) (Counter) this.dgvCounters.CurrentRow.DataBoundItem)).List<Evidence>();
        this.session.Clear();
        if (evidenceList.Count > 0)
        {
          int num = (int) MessageBox.Show("Невозможно изменить номер счетчика. У данного счетчика есть показания", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.LoadCounters();
          return;
        }
      }
      ((Counter) this.dgvCounters.CurrentRow.DataBoundItem).IsEdit = true;
    }

    private void dgvCounters_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      Counter dataBoundItem = (Counter) this.dgvCounters.CurrentRow.DataBoundItem;
      dataBoundItem.IsEdit = true;
      if (this.dgvCounters.CurrentCell.Value == null)
        return;
      try
      {
        string name = this.dgvCounters.Columns[e.ColumnIndex].Name;
        // ISSUE: reference to a compiler-generated method
        uint stringHash = PrivateImplementationDetails.ComputeStringHash(name);
        if (stringHash <= 1879400691U)
        {
          if (stringHash <= 898969975U)
          {
            if ((int) stringHash != 29994109)
            {
              if ((int) stringHash != 747694909)
              {
                if ((int) stringHash == 898969975)
                {
                  if (name == "SetDate")
                  {
                    try
                    {
                      dataBoundItem.SetDate = new DateTime?(Convert.ToDateTime(this.dgvCounters.CurrentRow.Cells["SetDate"].Value));
                    }
                    catch
                    {
                    }
                  }
                }
              }
              else if (name == "LicAddress")
              {
                dataBoundItem.LsClient = this.session.Get<LsClient>(this.dgvCounters.CurrentRow.Cells["LicAddress"].Value);
                dataBoundItem.Home = dataBoundItem.LsClient.Home;
              }
            }
            else if (name == "MainCounters")
              dataBoundItem.MainCounter = this.session.Get<Counter>(this.dgvCounters.CurrentRow.Cells["MainCounters"].Value);
          }
          else if ((int) stringHash != 1467563121)
          {
            if ((int) stringHash != 1539345862)
            {
              if ((int) stringHash == 1879400691 && name == "Address")
                dataBoundItem.Home = this.session.Get<Home>(this.dgvCounters.CurrentRow.Cells["Address"].Value);
            }
            else if (name == "Location")
              dataBoundItem.Location = this.session.Get<CounterLocation>(this.dgvCounters.CurrentRow.Cells["Location"].Value);
          }
          else if (name == "RemoveDate")
          {
            try
            {
              dataBoundItem.RemoveDate = new DateTime?(Convert.ToDateTime(this.dgvCounters.CurrentRow.Cells["RemoveDate"].Value));
            }
            catch
            {
            }
          }
        }
        else if (stringHash <= 2651540156U)
        {
          if ((int) stringHash != 2044719548)
          {
            if ((int) stringHash != -1899798424)
            {
              if ((int) stringHash == -1643427140)
              {
                if (name == "AuditDate")
                {
                  try
                  {
                    dataBoundItem.AuditDate = new DateTime?(Convert.ToDateTime(this.dgvCounters.CurrentRow.Cells["AuditDate"].Value));
                  }
                  catch
                  {
                  }
                }
              }
            }
            else if (name == "EvidenceStart")
            {
              try
              {
                dataBoundItem.EvidenceStart = Convert.ToDouble(KvrplHelper.ChangeSeparator(this.dgvCounters.CurrentRow.Cells["EvidenceStart"].Value.ToString()));
              }
              catch
              {
              }
            }
          }
          else if (name == "CoeffTrans")
          {
            try
            {
              dataBoundItem.CoeffTrans = (double) Convert.ToInt32(this.dgvCounters.CurrentRow.Cells["CoeffTrans"].Value);
            }
            catch
            {
            }
          }
        }
        else if ((int) stringHash != -1240729199)
        {
          if ((int) stringHash != -1237072124)
          {
            if ((int) stringHash == -782905235 && name == "Type")
              dataBoundItem.TypeCounter = this.session.Get<TypeCounter>(this.dgvCounters.CurrentRow.Cells["Type"].Value);
          }
          else if (name == "Service")
            dataBoundItem.Service = this.session.Get<Service>(this.dgvCounters.CurrentRow.Cells["Service"].Value);
        }
        else if (name == "CounterNum")
          dataBoundItem.CounterNum = this.dgvCounters.CurrentRow.Cells["CounterNum"].Value.ToString();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
        throw;
      }
    }

    private void dgvCounters_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
    }

    private void LoadEvidence()
    {
      this.Cursor = Cursors.WaitCursor;
      this.cbArchive.Visible = true;
      this.pnTools.Height = 73;
      if (this.city == 3 || this.city == 7 || this.city == 28)
        this.btnLoad.Visible = true;
      this.evidences = (IList<Evidence>) new List<Evidence>();
      if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2 || (int) this.level == 2)
      {
        this.cbArchive.Enabled = false;
        this.cbArchive.Checked = false;
      }
      else if (!this.editEvidence)
        this.cbArchive.Enabled = true;
      DateTime dateTime1;
      int num1;
      if (Options.Period.PeriodId <= this.monthClosed.PeriodId)
      {
        if (this.mpCurrentPeriod.OldMonth == 12)
        {
          dateTime1 = this.mpCurrentPeriod.Value;
          if (dateTime1.Month == 1)
            goto label_9;
        }
        num1 = 1;
        goto label_11;
      }
label_9:
      num1 = this.cbArchive.Checked ? 1 : 0;
label_11:
      if (num1 != 0)
      {
        this.editEvidence = false;
        this.btnEdit.BackColor = this.pnBtn.BackColor;
        this.tmrEvidence.Stop();
        this.lblEdit.Visible = false;
        this.btnEdit.Enabled = false;
      }
      else
        this.btnEdit.Enabled = true;
      if (!this.editEvidence)
      {
        this.btnAdd.Enabled = false;
        this.btnDelete.Enabled = false;
        this.btnSave.Enabled = false;
        this.dgvEvidence.ReadOnly = true;
        string str1 = "";
        string str2 = "";
        string str3 = "";
        string str4 = "";
        int num2 = 0;
        if ((uint) Convert.ToInt16(this.cmbService.SelectedValue) > 0U)
          str1 = " and s.Service.ServiceId={3}";
        if ((int) this.level == 2)
          str2 += " st.NameStr,lengthhome(s.Home.NHome),s.Home.HomeKorp,";
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
        {
          str2 += " lengthhome(s.LsClient.Flat.NFlat),";
          str4 = string.Format(" and s.LsClient.Complex.IdFk in ({0},{1})", (object) Options.Complex.IdFk, (object) Options.ComplexArenda.IdFk);
          if (!Options.Kvartplata)
            str4 = string.Format(" and s.LsClient.Complex.IdFk={0}", (object) Options.ComplexArenda.IdFk);
          if (!Options.Arenda)
            str4 = string.Format(" and s.LsClient.Complex.IdFk={0}", (object) Options.Complex.IdFk);
        }
        string str5 = !(Options.SortService == " s.ServiceId") ? " s.Service.ServiceName" : " s.Service.ServiceId";
        if (!this.cbArchive.Checked)
        {
          this.pnDetail.Visible = false;
          str3 = " and e.Period.PeriodId={4}";
          num2 = Options.Period.PeriodId;
        }
        else
        {
          this.pnDetail.Visible = true;
          this.cmbMonth.Visible = false;
          this.lblMonth.Visible = false;
          if ((Period) this.cmbPeriod.SelectedItem != null && (uint) ((Period) this.cmbPeriod.SelectedItem).PeriodId > 0U)
          {
            str3 = " and e.Period.PeriodId={4}";
            num2 = ((Period) this.cmbPeriod.SelectedItem).PeriodId;
          }
        }
        if ((int) this.level == 3)
          this.evidences = this.session.CreateQuery(string.Format("select e from Evidence e join fetch e.Counter s left join fetch s.Service left join fetch s.TypeCounter where s.Complex.ComplexId={0} " + str4 + str3 + " and s.Home.IdHome={1} and s.BaseCounter.Id={2} and s.Company.CompanyId={5} " + str1 + " order by e.Period.PeriodId desc," + str2 + str5, (object) Options.Complex.ComplexId, (object) this.home.IdHome, this.cmbBase.SelectedValue, this.cmbService.SelectedValue, (object) num2, (object) this.company.CompanyId)).List<Evidence>();
        if ((int) this.level == 2)
          this.evidences = this.session.CreateQuery(string.Format("select e from Evidence e join fetch e.Counter s left join fetch s.Service left join fetch s.TypeCounter left join fetch s.Home h left join fetch h.Str st where s.Complex.ComplexId={0} " + str4 + str3 + " and s.Company.CompanyId={1} and s.BaseCounter.Id={2} " + str1 + " order by e.Period.PeriodId desc," + str2 + str5, (object) Options.Complex.ComplexId, (object) this.company.CompanyId, this.cmbBase.SelectedValue, this.cmbService.SelectedValue, (object) num2)).List<Evidence>();
      }
      else
      {
        this.btnAdd.Enabled = true;
        this.btnDelete.Enabled = true;
        this.btnSave.Enabled = false;
        this.cbArchive.Enabled = false;
        this.dgvEvidence.ReadOnly = false;
        DateTime? nullable = Options.Period.PeriodName;
        DateTime dateTime2 = nullable.Value;
        nullable = this.monthClosed.PeriodName;
        DateTime dateTime3 = nullable.Value;
        if (dateTime2 > dateTime3)
          this.LoadCountersList(false);
        else
          this.LoadCountersList(true);
        this.session = Domain.CurrentSession;
        foreach (Counter counter in (IEnumerable<Counter>) this.counters)
        {
          IList<Evidence> evidenceList1 = this.session.CreateCriteria(typeof (Evidence)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Period", (object) Options.Period)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Counter", (object) counter)).List<Evidence>();
          if (evidenceList1.Count > 0)
          {
            foreach (Evidence evidence in (IEnumerable<Evidence>) evidenceList1)
              this.evidences.Add(evidence);
          }
          else
          {
            nullable = Options.Period.PeriodName;
            DateTime dateTime4 = nullable.Value;
            nullable = this.monthClosed.PeriodName;
            DateTime dateTime5 = nullable.Value;
            int num2;
            if (dateTime4 > dateTime5)
            {
              nullable = counter.ArchivesDate;
              if (nullable.HasValue)
              {
                nullable = counter.ArchivesDate;
                dateTime1 = Options.Period.PeriodName.Value;
                num2 = nullable.HasValue ? (nullable.GetValueOrDefault() > dateTime1 ? 1 : 0) : 0;
              }
              else
                num2 = 1;
            }
            else
              num2 = 0;
            if (num2 != 0)
            {
              IList<Evidence> evidenceList2 = this.session.CreateQuery(string.Format("select cp from Evidence cp where cp.Counter.CounterId={1} and cp.DBeg=(select max(DBeg) from Evidence where Counter.CounterId=cp.Counter.CounterId and Period.PeriodId<{0}) order by cp.Period.PeriodId desc,cp.DEnd desc", (object) Options.Period.PeriodId, (object) counter.CounterId)).List<Evidence>();
              Evidence evidence1 = new Evidence();
              if (evidenceList2.Count > 0)
              {
                evidence1 = evidenceList2[0];
                evidence1.Period = (Period) null;
                Evidence evidence2 = evidence1;
                dateTime1 = evidence1.DEnd;
                DateTime dateTime6 = dateTime1.AddDays(1.0);
                evidence2.DBeg = dateTime6;
                evidence1.Past = evidence1.Current;
              }
              else
              {
                evidence1.Counter = counter;
                Evidence evidence2 = evidence1;
                nullable = counter.SetDate;
                DateTime dateTime6;
                if (!nullable.HasValue)
                {
                  nullable = Options.Period.PeriodName;
                  dateTime6 = KvrplHelper.FirstDay(nullable.Value);
                }
                else
                {
                  nullable = counter.SetDate;
                  dateTime6 = nullable.Value;
                }
                evidence2.DBeg = dateTime6;
                evidence1.Current = counter.EvidenceStart;
                evidence1.Past = counter.EvidenceStart;
              }
              Evidence evidence3 = evidence1;
              DateTime dateTime7;
              if (this.setup31 != 1)
              {
                if (this.setup31 != 2)
                {
                  dateTime7 = DateTime.Now;
                }
                else
                {
                  dateTime1 = DateTime.Now;
                  dateTime7 = dateTime1.AddDays(-1.0);
                }
              }
              else
              {
                nullable = Options.Period.PeriodName;
                dateTime7 = KvrplHelper.LastDay(nullable.Value);
              }
              evidence3.DEnd = dateTime7;
              this.evidences.Add(evidence1);
            }
          }
        }
      }
      this.dgvEvidence.Columns.Clear();
      this.dgvEvidence.DataSource = (object) null;
      this.dgvEvidence.DataSource = (object) this.evidences;
      this.session.Clear();
      this.SetViewEvidence();
      this.MySettingsEvidence.GridName = "Evidence";
      this.LoadSettingsEvidence();
      this.Cursor = Cursors.Default;
    }

    private void LoadSettingsEvidence()
    {
      this.MySettingsEvidence.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvEvidence.Columns)
        this.MySettingsEvidence.GetMySettings(column);
    }

    private void dgvEvidence_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsEvidence.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsEvidence.Columns[this.MySettingsEvidence.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsEvidence.Save();
    }

    private void SetViewEvidence()
    {
      this.pbar.Value = 0;
      this.pbar.Step = 1;
      this.pbar.Minimum = 0;
      this.pbar.Maximum = this.dgvEvidence.Rows.Count;
      KvrplHelper.AddMaskDateColumn(this.dgvEvidence, 0, "Дата наст-го", "DEnd");
      KvrplHelper.AddMaskDateColumn(this.dgvEvidence, 1, "Дата пред-го", "DBeg");
      KvrplHelper.AddTextBoxColumn(this.dgvEvidence, 0, "Наст-е показания", "Current", 80, false);
      KvrplHelper.AddTextBoxColumn(this.dgvEvidence, 0, "Пред-е показания", "Past", 80, false);
      this.dgvEvidence.Columns["CounterNum"].HeaderText = "Номер счетчика";
      this.dgvEvidence.Columns["Volume"].HeaderText = "Расход";
      this.dgvEvidence.Columns["ServiceName"].HeaderText = "Услуга";
      this.dgvEvidence.Columns["ServiceName"].DisplayIndex = 0;
      this.dgvEvidence.Columns["CounterNum"].DisplayIndex = 1;
      this.dgvEvidence.Columns["DBeg"].DisplayIndex = 2;
      this.dgvEvidence.Columns["Past"].DisplayIndex = 3;
      this.dgvEvidence.Columns["DEnd"].DisplayIndex = 4;
      this.dgvEvidence.Columns["Current"].DisplayIndex = 5;
      this.dgvEvidence.Columns["Volume"].DisplayIndex = 6;
      this.dgvEvidence.Columns["UName"].DisplayIndex = 7;
      this.dgvEvidence.Columns["Dedit"].DisplayIndex = 8;
      this.dgvEvidence.Columns["ServiceName"].ReadOnly = true;
      this.dgvEvidence.Columns["CounterNum"].ReadOnly = true;
      this.dgvEvidence.Columns["Volume"].ReadOnly = true;
      this.dgvEvidence.Columns["CounterNum"].Visible = false;
      this.dgvEvidence.Columns["ServiceName"].Visible = false;
      KvrplHelper.ViewEdit(this.dgvEvidence);
      DataGridViewComboBoxColumn viewComboBoxColumn = new DataGridViewComboBoxColumn();
      viewComboBoxColumn.DropDownWidth = 180;
      viewComboBoxColumn.Width = 180;
      viewComboBoxColumn.MaxDropDownItems = 7;
      viewComboBoxColumn.DisplayStyleForCurrentCellOnly = true;
      if ((uint) Convert.ToInt16(this.cmbService.SelectedValue) > 0U)
      {
        this.dgvEvidence.Columns["ServiceName"].Visible = false;
        this.uslovie = "and c.Service.ServiceId={3}";
      }
      else
        this.uslovie = "";
      string str1 = string.Format(" and ls.Complex.IdFk in ({0},{1})", (object) Options.Complex.IdFk, (object) Options.ComplexArenda.IdFk);
      if (!Options.Kvartplata)
        str1 = string.Format(" and ls.Complex.IdFk={0}", (object) Options.ComplexArenda.IdFk);
      if (!Options.Arenda)
        str1 = string.Format(" and ls.Complex.IdFk={0}", (object) Options.Complex.IdFk);
      this.session = Domain.CurrentSession;
      if ((int) Convert.ToInt16(this.cmbService.SelectedValue) == 0)
      {
        IList<Service> serviceList = (IList<Service>) new List<Service>();
        KvrplHelper.AddComboBoxColumn(this.dgvEvidence, 0, (IList) this.session.CreateQuery(string.Format("select distinct s from Service s,ServiceParam sp where sp.Service_id=s.ServiceId and s.Root=0 and s.ServiceId<>0 and sp.Company_id={0} order by " + Options.SortService, (object) this.company.CompanyId)).List<Service>(), "ServiceId", "ServiceName", "Услуга", "Service", 140, 140);
      }
      KvrplHelper.AddComboBoxColumn(this.dgvEvidence, 1, (IList) null, (string) null, (string) null, "Номер счетчика", "Num", 100, 100);
      DateTime? periodName;
      if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
      {
        this.clients = (IList<LsClient>) new List<LsClient>();
        if ((int) this.level == 2)
        {
          ISession session = this.session;
          string format = "select distinct new LsClient(ls.ClientId,ls.SurFlat,ls.Flat,(select a.DogovorNum from LsArenda a where a.LsClient.ClientId=ls.ClientId),ls.Complex) from LsClient ls,Counter c,Flat f where c.LsClient=ls and ls.Company.CompanyId = {0}  and ls.Flat=f and c.BaseCounter.Id={2} " + this.uslovie + str1;
          object[] objArray = new object[4];
          objArray[0] = (object) this.company.CompanyId;
          int index1 = 1;
          periodName = Options.Period.PeriodName;
          string baseFormat = KvrplHelper.DateToBaseFormat(periodName.Value);
          objArray[index1] = (object) baseFormat;
          int index2 = 2;
          object selectedValue = this.cmbBase.SelectedValue;
          objArray[index2] = selectedValue;
          int index3 = 3;
          // ISSUE: variable of a boxed type
          short int16 = Convert.ToInt16(this.cmbService.SelectedValue);
          objArray[index3] = (object) int16;
          string queryString = string.Format(format, objArray);
          this.clients = session.CreateQuery(queryString).List<LsClient>();
        }
        if ((int) this.level == 3)
        {
          ISession session = this.session;
          string format = "select distinct new LsClient(ls.ClientId,ls.SurFlat,ls.Flat,(select a.DogovorNum from LsArenda a where a.LsClient.ClientId=ls.ClientId),ls.Complex) from LsClient ls ,Counter c,Flat f where c.LsClient=ls and ls.Home.IdHome = {0}  and ls.Flat=f and c.BaseCounter.Id={2} " + this.uslovie + str1;
          object[] objArray = new object[4];
          objArray[0] = (object) this.home.IdHome;
          int index1 = 1;
          periodName = Options.Period.PeriodName;
          string baseFormat = KvrplHelper.DateToBaseFormat(periodName.Value);
          objArray[index1] = (object) baseFormat;
          int index2 = 2;
          object selectedValue = this.cmbBase.SelectedValue;
          objArray[index2] = selectedValue;
          int index3 = 3;
          // ISSUE: variable of a boxed type
          short int16 = Convert.ToInt16(this.cmbService.SelectedValue);
          objArray[index3] = (object) int16;
          string queryString = string.Format(format, objArray);
          this.clients = session.CreateQuery(queryString).List<LsClient>();
        }
        viewComboBoxColumn.DataSource = (object) this.clients;
        viewComboBoxColumn.ValueMember = "ClientId";
        viewComboBoxColumn.DisplayMember = "SmallAddress";
        viewComboBoxColumn.HeaderText = "Адрес";
        viewComboBoxColumn.Name = "LicAddress";
        this.dgvEvidence.Columns.Insert(0, (DataGridViewColumn) viewComboBoxColumn);
      }
      if ((Convert.ToInt32(this.cmbBase.SelectedValue) != 2 || (int) this.level == 2) && Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
      {
        this.homes = (IList<Home>) new List<Home>();
        if ((int) this.level == 2)
        {
          ISession session = this.session;
          string format = "select distinct h from Home h left join fetch h.Str, HomeLink hl,Counter c where c.Home=h and hl.Home=h and hl.Company.CompanyId={0} and c.BaseCounter.Id={2} " + this.uslovie + " order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)";
          object[] objArray = new object[4];
          objArray[0] = (object) this.company.CompanyId;
          int index1 = 1;
          periodName = Options.Period.PeriodName;
          string baseFormat = KvrplHelper.DateToBaseFormat(periodName.Value);
          objArray[index1] = (object) baseFormat;
          int index2 = 2;
          object selectedValue = this.cmbBase.SelectedValue;
          objArray[index2] = selectedValue;
          int index3 = 3;
          // ISSUE: variable of a boxed type
          short int16 = Convert.ToInt16(this.cmbService.SelectedValue);
          objArray[index3] = (object) int16;
          string queryString = string.Format(format, objArray);
          this.homes = session.CreateQuery(queryString).List<Home>();
        }
        if ((int) this.level == 3)
        {
          ISession session = this.session;
          string format = "select distinct h from Home h left join fetch h.Str, HomeLink hl,Counter c where c.Home=h and c.Home.IdHome={4} and c.BaseCounter.Id={2} and hl.Home=h and hl.Company.CompanyId={0} " + this.uslovie + " order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)";
          object[] objArray = new object[5];
          objArray[0] = (object) this.company.CompanyId;
          int index1 = 1;
          periodName = Options.Period.PeriodName;
          string baseFormat = KvrplHelper.DateToBaseFormat(periodName.Value);
          objArray[index1] = (object) baseFormat;
          int index2 = 2;
          object selectedValue = this.cmbBase.SelectedValue;
          objArray[index2] = selectedValue;
          int index3 = 3;
          // ISSUE: variable of a boxed type
          short int16 = Convert.ToInt16(this.cmbService.SelectedValue);
          objArray[index3] = (object) int16;
          int index4 = 4;
          // ISSUE: variable of a boxed type
          int idHome = this.home.IdHome;
          objArray[index4] = (object) idHome;
          string queryString = string.Format(format, objArray);
          this.homes = session.CreateQuery(queryString).List<Home>();
        }
        KvrplHelper.AddComboBoxColumn(this.dgvEvidence, 0, (IList) this.homes, "IdHome", "Address", "Адрес", "Address", 180, 180);
      }
      if (this.cbArchive.Checked)
        KvrplHelper.AddComboBoxColumn(this.dgvEvidence, 0, (IList) this.session.CreateQuery(string.Format("select p from Period p where PeriodId in (select Period.PeriodId from Evidence where Counter.Home.IdHome={0})", (object) this.home.IdHome)).List<Period>(), "PeriodId", "PeriodName", "Период", "Period", 100, 100);
      IList<LsClient> lsClientList1 = (IList<LsClient>) new List<LsClient>();
      IList<Counter> counterList1 = (IList<Counter>) new List<Counter>();
      foreach (DataGridViewRow row in (IEnumerable) this.dgvEvidence.Rows)
      {
        ++this.pbar.Value;
        row.Cells["DBeg"].Value = (object) ((Evidence) row.DataBoundItem).DBeg;
        row.Cells["DEnd"].Value = (object) ((Evidence) row.DataBoundItem).DEnd;
        row.Cells["Past"].Value = (object) ((Evidence) row.DataBoundItem).Past;
        row.Cells["Current"].Value = (object) ((Evidence) row.DataBoundItem).Current;
        row.Cells["Dedit"].Value = (object) ((Evidence) row.DataBoundItem).DEdit.ToShortDateString();
        if (this.cbArchive.Checked)
          row.Cells["Period"].Value = (object) ((Evidence) row.DataBoundItem).Period.PeriodId;
        IList<Counter> counterList2;
        if (Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
        {
          if (((int) this.level == 2 || (int) this.level == 3 && Convert.ToInt32(this.cmbBase.SelectedValue) != 2) && ((Evidence) row.DataBoundItem).Home != null)
          {
            if (Convert.ToInt32(this.cmbBase.SelectedValue) != 2)
            {
              row.Cells["Address"].Value = (object) ((Evidence) row.DataBoundItem).Home.IdHome;
              if (counterList1.Count == 0 || counterList1[0].Home.IdHome != ((Evidence) row.DataBoundItem).Home.IdHome)
              {
                counterList2 = (IList<Counter>) new List<Counter>();
                counterList1 = (uint) Convert.ToInt16(this.cmbService.SelectedValue) <= 0U ? this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Home.IdHome", (object) ((Evidence) row.DataBoundItem).Home.IdHome)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Company.CompanyId", (object) this.company.CompanyId)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>() : this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Home.IdHome", (object) ((Evidence) row.DataBoundItem).Home.IdHome)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) (Service) this.cmbService.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>();
              }
              row.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
              {
                DisplayStyleForCurrentCellOnly = true,
                ValueMember = "CounterId",
                DisplayMember = "CounterNum1",
                DataSource = (object) counterList1
              };
              row.Cells["Num"].Value = (object) ((Evidence) row.DataBoundItem).Counter.CounterId;
            }
            else
            {
              row.Cells["Address"].Value = (object) ((Evidence) row.DataBoundItem).Home.IdHome;
              if (lsClientList1.Count == 0 || lsClientList1[0].Home.IdHome != ((Evidence) row.DataBoundItem).Home.IdHome)
              {
                IList<LsClient> lsClientList2 = (IList<LsClient>) new List<LsClient>();
                ISession session = this.session;
                string format = "select distinct ls from LsClient ls,Counter c,Flat f where c.LsClient=ls and ls.Home.IdHome = {0} and (c.ArchivesDate is null or c.ArchivesDate > '{1}') and ls.Flat=f and c.BaseCounter.Id={2} and c.Complex.ComplexId={4} " + this.uslovie + str1;
                object[] objArray = new object[5];
                objArray[0] = (object) ((Evidence) row.DataBoundItem).Home.IdHome;
                int index1 = 1;
                periodName = Options.Period.PeriodName;
                string baseFormat = KvrplHelper.DateToBaseFormat(periodName.Value);
                objArray[index1] = (object) baseFormat;
                int index2 = 2;
                object selectedValue = this.cmbBase.SelectedValue;
                objArray[index2] = selectedValue;
                int index3 = 3;
                // ISSUE: variable of a boxed type
                short int16 = Convert.ToInt16(this.cmbService.SelectedValue);
                objArray[index3] = (object) int16;
                int index4 = 4;
                // ISSUE: variable of a boxed type
                int complexId = Options.Complex.ComplexId;
                objArray[index4] = (object) complexId;
                string queryString = string.Format(format, objArray);
                lsClientList1 = session.CreateQuery(queryString).List<LsClient>();
              }
              row.Cells["LicAddress"] = (DataGridViewCell) new DataGridViewComboBoxCell()
              {
                DisplayStyleForCurrentCellOnly = true,
                ValueMember = "ClientId",
                DisplayMember = "SmallAddress",
                DataSource = (object) lsClientList1
              };
              row.Cells["LicAddress"].Value = (object) ((Evidence) row.DataBoundItem).ClientId;
            }
          }
          int? clientId1;
          int num1;
          if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
          {
            clientId1 = ((Evidence) row.DataBoundItem).ClientId;
            num1 = clientId1.HasValue ? 1 : 0;
          }
          else
            num1 = 0;
          if (num1 != 0)
          {
            row.Cells["LicAddress"].Value = (object) ((Evidence) row.DataBoundItem).ClientId;
            int num2;
            if (counterList1.Count != 0)
            {
              int clientId2 = counterList1[0].LsClient.ClientId;
              clientId1 = ((Evidence) row.DataBoundItem).ClientId;
              int valueOrDefault = clientId1.GetValueOrDefault();
              num2 = clientId2 == valueOrDefault ? (!clientId1.HasValue ? 1 : 0) : 1;
            }
            else
              num2 = 1;
            if (num2 != 0)
            {
              counterList2 = (IList<Counter>) new List<Counter>();
              counterList1 = (uint) Convert.ToInt16(this.cmbService.SelectedValue) <= 0U ? this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("LsClient.ClientId", (object) ((Evidence) row.DataBoundItem).ClientId)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>() : this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("LsClient.ClientId", (object) ((Evidence) row.DataBoundItem).ClientId)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) (Service) this.cmbService.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>();
            }
            row.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
            {
              DisplayStyleForCurrentCellOnly = true,
              ValueMember = "CounterId",
              DisplayMember = "CounterNum1",
              DataSource = (object) counterList1
            };
            row.Cells["Num"].Value = (object) ((Evidence) row.DataBoundItem).Counter.CounterId;
          }
        }
        else
        {
          if (counterList1.Count == 0)
          {
            counterList2 = (IList<Counter>) new List<Counter>();
            counterList1 = (uint) Convert.ToInt16(this.cmbService.SelectedValue) <= 0U ? this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>() : this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) (Service) this.cmbService.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>();
          }
          row.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
          {
            DisplayStyleForCurrentCellOnly = true,
            ValueMember = "CounterId",
            DisplayMember = "CounterNum1",
            DataSource = (object) counterList1
          };
          if (((Evidence) row.DataBoundItem).Counter != null)
            row.Cells["Num"].Value = (object) ((Evidence) row.DataBoundItem).Counter.CounterId;
        }
        if ((int) Convert.ToInt16(this.cmbService.SelectedValue) == 0 && ((Evidence) row.DataBoundItem).Counter != null)
          row.Cells["Service"].Value = (object) ((Evidence) row.DataBoundItem).Counter.Service.ServiceId;
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 3)
        {
          this.dgvEvidence.Columns.Add("Flat", "Квартиры");
          this.dgvEvidence.Columns["Flat"].DisplayIndex = 1;
          IList list = this.session.CreateSQLQuery(string.Format("select distinct f.NFLAT  from cntrCounter coun inner join cntrRelation rel on rel.Counter_id=coun.Counter_id inner join lsClient cl on cl.Client_id=rel.Client_id inner join FLATS f on f.IDFLAT=cl.IdFlat  where coun.Counter_id={0} and f.idhome={1}", (object) ((Evidence) row.DataBoundItem).Counter.CounterId, (object) ((Evidence) row.DataBoundItem).Home.IdHome)).List();
          string str2 = "";
          foreach (object obj in (IEnumerable) list)
            str2 = str2 + " " + obj.ToString();
          row.Cells["Flat"].Value = (object) str2;
        }
      }
      this.pbar.Visible = false;
    }

    private void dgvEvidence_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (!this.dgvEvidence.IsCurrentCellDirty)
        return;
      this.dgvEvidence.CommitEdit(DataGridViewDataErrorContexts.Commit);
      if ((Convert.ToInt32(this.cmbBase.SelectedValue) == 1 || Convert.ToInt32(this.cmbBase.SelectedValue) == 3) && (int) Convert.ToInt16(this.cmbService.SelectedValue) != 0 && this.dgvEvidence.CurrentCell.ColumnIndex == this.dgvEvidence.Rows[this.dgvEvidence.CurrentRow.Index].Cells["Address"].ColumnIndex)
      {
        this.session = Domain.CurrentSession;
        IList<Counter> counterList = (IList<Counter>) new List<Counter>();
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 1 || Convert.ToInt32(this.cmbBase.SelectedValue) == 3)
          counterList = this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Home.IdHome", this.dgvEvidence.CurrentRow.Cells["Address"].Value)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) (Service) this.cmbService.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).List<Counter>();
        this.session.Clear();
        this.dgvEvidence.CurrentRow.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "CounterId",
          DisplayMember = "CounterNum1",
          DataSource = (object) counterList
        };
      }
      if ((int) Convert.ToInt16(this.cmbBase.SelectedValue) == 2 && (int) Convert.ToInt16(this.cmbService.SelectedValue) != 0 && this.dgvEvidence.CurrentCell.ColumnIndex == this.dgvEvidence.Rows[this.dgvEvidence.CurrentRow.Index].Cells["LicAddress"].ColumnIndex)
      {
        IList<LsClient> lsClientList = (IList<LsClient>) new List<LsClient>();
        this.session = Domain.CurrentSession;
        IList<Counter> counterList1 = (IList<Counter>) new List<Counter>();
        IList<Counter> counterList2 = this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("LsClient.ClientId", this.dgvEvidence.CurrentRow.Cells["LicAddress"].Value)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) (Service) this.cmbService.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).List<Counter>();
        this.session.Clear();
        this.dgvEvidence.CurrentRow.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "CounterId",
          DisplayMember = "CounterNum1",
          DataSource = (object) counterList2
        };
      }
      if ((int) Convert.ToInt16(this.cmbService.SelectedValue) == 0 && this.dgvEvidence.CurrentCell.ColumnIndex == this.dgvEvidence.Rows[this.dgvEvidence.CurrentRow.Index].Cells["Service"].ColumnIndex)
      {
        this.session = Domain.CurrentSession;
        IList<Counter> counterList = (IList<Counter>) new List<Counter>();
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
          counterList = (uint) Convert.ToInt16(this.dgvEvidence.CurrentRow.Cells["Service"].Value) <= 0U ? this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("LsClient.ClientId", this.dgvEvidence.CurrentRow.Cells["LicAddress"].Value)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).List<Counter>() : this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("LsClient.ClientId", this.dgvEvidence.CurrentRow.Cells["LicAddress"].Value)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service.ServiceId", (object) Convert.ToInt16(this.dgvEvidence.CurrentRow.Cells["Service"].Value))).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).List<Counter>();
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 1 || Convert.ToInt32(this.cmbBase.SelectedValue) == 3)
          counterList = (uint) Convert.ToInt16(this.dgvEvidence.CurrentRow.Cells["Service"].Value) <= 0U ? this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Home.IdHome", this.dgvEvidence.CurrentRow.Cells["Address"].Value)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).List<Counter>() : this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Home.IdHome", this.dgvEvidence.CurrentRow.Cells["Address"].Value)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service.ServiceId", (object) Convert.ToInt16(this.dgvEvidence.CurrentRow.Cells["Service"].Value))).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Company", (object) this.company)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).List<Counter>();
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 4)
          counterList = (uint) Convert.ToInt16(this.dgvEvidence.CurrentRow.Cells["Service"].Value) <= 0U ? this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).List<Counter>() : this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service.ServiceId", (object) Convert.ToInt16(this.dgvEvidence.CurrentRow.Cells["Service"].Value))).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).List<Counter>();
        this.session.Clear();
        this.dgvEvidence.CurrentRow.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "CounterId",
          DisplayMember = "CounterNum1",
          DataSource = (object) counterList
        };
      }
      if ((int) this.level == 2 && Convert.ToInt32(this.cmbBase.SelectedValue) == 2 && this.dgvEvidence.CurrentCell.ColumnIndex == this.dgvEvidence.Rows[this.dgvEvidence.CurrentRow.Index].Cells["Address"].ColumnIndex)
      {
        this.session = Domain.CurrentSession;
        IList<LsClient> lsClientList1 = (IList<LsClient>) new List<LsClient>();
        string str1;
        if ((uint) Convert.ToInt16(this.cmbService.SelectedValue) > 0U)
        {
          this.dgvEvidence.Columns["ServiceName"].Visible = false;
          str1 = "and c.Service.ServiceId={3}";
        }
        else
          str1 = "";
        string str2 = string.Format(" and ls.Complex.IdFk in ({0},{1})", (object) Options.Complex.IdFk, (object) Options.ComplexArenda.IdFk);
        if (!Options.Kvartplata)
          str2 = string.Format(" and ls.Complex.IdFk={0}", (object) Options.ComplexArenda.IdFk);
        if (!Options.Arenda)
          str2 = string.Format(" and ls.Complex.IdFk={0}", (object) Options.Complex.IdFk);
        IList<LsClient> lsClientList2 = this.session.CreateQuery(string.Format("select distinct ls from LsClient ls,Counter c,Flat f where c.LsClient=ls and ls.Home.IdHome = {0} and (c.ArchivesDate is null or c.ArchivesDate > '{1}') and ls.Flat=f and c.BaseCounter.Id={2} " + str1 + str2, this.dgvEvidence.CurrentRow.Cells["Address"].Value, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), this.cmbBase.SelectedValue, (object) Convert.ToInt16(this.cmbService.SelectedValue))).List<LsClient>();
        this.dgvEvidence.CurrentRow.Cells["LicAddress"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "ClientId",
          DisplayMember = "SmallAddress",
          DataSource = (object) lsClientList2
        };
      }
    }

    private void InsertEvidence()
    {
      this.insertRecord = true;
      Evidence evidence = new Evidence();
      evidence.DBeg = DateTime.Now;
      evidence.DEnd = this.dtmCounterDay.Value;
      IList<Evidence> evidenceList = (IList<Evidence>) new List<Evidence>();
      if ((uint) this.dgvEvidence.Rows.Count > 0U)
        evidenceList = (IList<Evidence>) (this.dgvEvidence.DataSource as List<Evidence>);
      evidenceList.Add(evidence);
      this.dgvEvidence.Columns.Clear();
      this.dgvEvidence.DataSource = (object) null;
      this.dgvEvidence.DataSource = (object) evidenceList;
      this.SetViewEvidence();
      this.LoadSettingsEvidence();
      this.dgvEvidence.CurrentCell = this.dgvEvidence.Rows[this.dgvEvidence.Rows.Count - 1].Cells[0];
    }

    private bool SaveEvidence()
    {
      if (this.dgvEvidence.Rows.Count > 0 && this.dgvEvidence.CurrentRow != null)
      {
        Evidence dataBoundItem = (Evidence) this.dgvEvidence.CurrentRow.DataBoundItem;
        if (this.dgvEvidence.CurrentRow.Cells["DBeg"].Value != null)
        {
          try
          {
            dataBoundItem.DBeg = Convert.ToDateTime(this.dgvEvidence.CurrentRow.Cells["DBeg"].Value);
          }
          catch
          {
            int num = (int) MessageBox.Show("Дата предыдущих показаний введена некорректно", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
          if (this.dgvEvidence.CurrentRow.Cells["DEnd"].Value != null)
          {
            dataBoundItem.DEnd = this.dtmCounterDay.Value;
            DateTime dend = dataBoundItem.DEnd;
            DateTime now = Options.Period.PeriodName.Value;
            DateTime dateTime = now.AddMonths(1);
            if (dend >= dateTime)
            {
              int num = (int) MessageBox.Show("Невозможно внести показания в будущее время", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
            if (this.dgvEvidence.CurrentRow.Cells["Num"].Value == null)
            {
              int num = (int) MessageBox.Show("Не выбран счетчик!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
            dataBoundItem.Counter = this.session.Get<Counter>(this.dgvEvidence.CurrentRow.Cells["Num"].Value);
            if (this.dgvEvidence.CurrentRow.Cells["Past"].Value != null && this.dgvEvidence.CurrentRow.Cells["Current"].Value != null)
            {
              try
              {
                dataBoundItem.Past = Convert.ToDouble(KvrplHelper.ChangeSeparator(this.dgvEvidence.CurrentRow.Cells["Past"].Value.ToString()));
                dataBoundItem.Current = Convert.ToDouble(KvrplHelper.ChangeSeparator(this.dgvEvidence.CurrentRow.Cells["Current"].Value.ToString()));
                if (this.insertRecord)
                {
                  if (this.session.CreateCriteria(typeof (Evidence)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Counter", (object) dataBoundItem.Counter)).List<Evidence>().Count == 0 && dataBoundItem.Past != dataBoundItem.Counter.EvidenceStart && MessageBox.Show("Взять начальные показания с закладки 'Счетчики'?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    dataBoundItem.Past = dataBoundItem.Counter.EvidenceStart;
                }
              }
              catch (Exception ex)
              {
                int num = (int) MessageBox.Show("Некорректный формат показаний", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
              }
              if (!this.Control(dataBoundItem))
                return false;
              dataBoundItem.UName = Options.Login;
              Evidence evidence = dataBoundItem;
              now = DateTime.Now;
              DateTime date = now.Date;
              evidence.DEdit = date;
              try
              {
                if (dataBoundItem.Period == null)
                {
                  dataBoundItem.Period = Options.Period;
                  this.insertRecord = false;
                  this.session.Save((object) dataBoundItem);
                }
                else
                  this.session.Update((object) dataBoundItem);
                this.session.Flush();
              }
              catch (Exception ex)
              {
                int num = (int) MessageBox.Show("Невозможно сохранить изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                KvrplHelper.WriteLog(ex, (LsClient) null);
              }
            }
            else
            {
              int num = (int) MessageBox.Show("Показания не введены", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
          }
          else
          {
            int num = (int) MessageBox.Show("Дата настоящих показаний не введена", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
        }
        else
        {
          int num = (int) MessageBox.Show("Дата предыдущих показаний не введена", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
      return true;
    }

    private void DeleteEvidence()
    {
      if (this.dgvEvidence.Rows.Count <= 0 || this.dgvEvidence.CurrentRow == null || MessageBox.Show("Удалить показания счетчика из этого периода?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      this.session.Clear();
      this.session = Domain.CurrentSession;
      Evidence dataBoundItem = (Evidence) this.dgvEvidence.CurrentRow.DataBoundItem;
      try
      {
        this.session.Delete((object) dataBoundItem);
        this.session.Flush();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Невозможно удалить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      this.session.Clear();
    }

    private void btnLoad_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(42, 2, this.company, true))
        return;
      bool flag = true;
      DateTime dateTime1 = this.monthClosed.PeriodName.Value;
      DateTime? periodName = Options.Period.PeriodName;
      DateTime dateTime2 = periodName.Value;
      if (dateTime1 >= dateTime2)
      {
        int num1 = (int) MessageBox.Show("Невозможно перенести счетчики в закрытый период", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (MessageBox.Show("Взять счетчики из предыдущего периода", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
          return;
        if (MessageBox.Show("Взять с показаниями?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
          flag = false;
        IList<Evidence> evidenceList1 = (IList<Evidence>) new List<Evidence>();
        if ((int) this.level == 3)
          evidenceList1 = this.session.CreateQuery(string.Format("select e from Evidence e join fetch e.Counter s where e.Period.PeriodId={3} and s.Complex.ComplexId={0} and s.Home.IdHome={1} and s.BaseCounter.Id={2} and s.Company.CompanyId={4}", (object) Options.Complex.ComplexId, (object) this.home.IdHome, this.cmbBase.SelectedValue, (object) Options.Period.PeriodId, (object) this.company.CompanyId)).List<Evidence>();
        if ((int) this.level == 2)
          evidenceList1 = this.session.CreateQuery(string.Format("select e from Evidence e join fetch e.Counter s where e.Period.PeriodId={3} and s.Complex.ComplexId={0} and s.Company.CompanyId={1} and s.BaseCounter.Id={2}", (object) Options.Complex.ComplexId, (object) this.company.CompanyId, this.cmbBase.SelectedValue, (object) Options.Period.PeriodId)).List<Evidence>();
        if (evidenceList1.Count > 0)
        {
          if (MessageBox.Show("В текущем периоде обнаружены показания счетчиков. Все равно внести показания?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            return;
          foreach (DataGridViewRow row in (IEnumerable) this.dgvEvidence.Rows)
          {
            this.session = Domain.CurrentSession;
            Evidence evidence = new Evidence();
            Evidence dataBoundItem = (Evidence) row.DataBoundItem;
            try
            {
              this.session.Delete((object) dataBoundItem);
              this.session.Flush();
            }
            catch (Exception ex)
            {
              KvrplHelper.WriteLog(ex, (LsClient) null);
            }
            this.session.Clear();
          }
        }
        FrmSelectPeriod frmSelectPeriod = new FrmSelectPeriod();
        int num2 = (int) frmSelectPeriod.ShowDialog();
        Period period = frmSelectPeriod.period;
        frmSelectPeriod.Visible = false;
        frmSelectPeriod.Dispose();
        if ((uint) Convert.ToInt16(this.cmbService.SelectedValue) > 0U)
          this.uslovie = " and c.Service.ServiceId = {5}";
        int int32 = Convert.ToInt32(this.cmbBase.SelectedValue);
        if (period != null)
        {
          this.session = Domain.CurrentSession;
          IList<Evidence> evidenceList2 = (IList<Evidence>) new List<Evidence>();
          if (int32 == 2)
          {
            if (this.home != null)
            {
              ISession session = this.session;
              string format = "select cp from Evidence cp, Counter c where cp.Counter=c and cp.Period.PeriodId={0} and c.BaseCounter.Id ={1} and cp.DEnd=(select max(DEnd) from Evidence where Counter=c and Period.PeriodId={0}) and (c.ArchivesDate is null or c.ArchivesDate>'{2}') and c.Home.IdHome={3} and c.Complex.ComplexId={4} and c.Service.ServiceId not in (select Service_id from ServiceParam where SendRent=1)" + this.uslovie;
              object[] objArray = new object[6];
              objArray[0] = (object) period.PeriodId;
              objArray[1] = (object) int32;
              int index1 = 2;
              periodName = Options.Period.PeriodName;
              string baseFormat = KvrplHelper.DateToBaseFormat(periodName.Value);
              objArray[index1] = (object) baseFormat;
              int index2 = 3;
              // ISSUE: variable of a boxed type
              int idHome = this.home.IdHome;
              objArray[index2] = (object) idHome;
              int index3 = 4;
              // ISSUE: variable of a boxed type
              int complexId = Options.Complex.ComplexId;
              objArray[index3] = (object) complexId;
              int index4 = 5;
              // ISSUE: variable of a boxed type
              short int16 = Convert.ToInt16(this.cmbService.SelectedValue);
              objArray[index4] = (object) int16;
              string queryString = string.Format(format, objArray);
              evidenceList2 = session.CreateQuery(queryString).List<Evidence>();
            }
            else
            {
              ISession session = this.session;
              string format = "select cp from Evidence cp, Counter c where cp.Counter=c and cp.Period.PeriodId={0} and c.BaseCounter.Id ={1} and cp.DEnd=(select max(DEnd) from Evidence where Counter=c and Period.PeriodId={0}) and (c.ArchivesDate is null or c.ArchivesDate>'{2}') and c.Complex.ComplexId={4} and c.Service.ServiceId not in (select Service_id from ServiceParam where SendRent=1)" + this.uslovie;
              object[] objArray = new object[6];
              objArray[0] = (object) period.PeriodId;
              objArray[1] = (object) int32;
              int index1 = 2;
              periodName = Options.Period.PeriodName;
              string baseFormat = KvrplHelper.DateToBaseFormat(periodName.Value);
              objArray[index1] = (object) baseFormat;
              int index2 = 3;
              // ISSUE: variable of a boxed type
              int local = 0;
              objArray[index2] = (object) local;
              int index3 = 4;
              // ISSUE: variable of a boxed type
              int complexId = Options.Complex.ComplexId;
              objArray[index3] = (object) complexId;
              int index4 = 5;
              // ISSUE: variable of a boxed type
              short int16 = Convert.ToInt16(this.cmbService.SelectedValue);
              objArray[index4] = (object) int16;
              string queryString = string.Format(format, objArray);
              evidenceList2 = session.CreateQuery(queryString).List<Evidence>();
            }
          }
          if (int32 == 1 || int32 == 3)
          {
            ISession session = this.session;
            string format = "select cp from Evidence cp, Counter c where cp.Counter=c and cp.Period.PeriodId={0} and c.BaseCounter.Id ={1} and cp.DEnd=(select max(DEnd) from Evidence where Counter=c and Period.PeriodId={0}) and (c.ArchivesDate is null or c.ArchivesDate>'{2}') and c.Company.CompanyId={3} and c.Complex.ComplexId={4} and c.Service.ServiceId not in (select Service_id from ServiceParam where SendRent=1)" + this.uslovie;
            object[] objArray = new object[6];
            objArray[0] = (object) period.PeriodId;
            objArray[1] = (object) int32;
            int index1 = 2;
            periodName = Options.Period.PeriodName;
            string baseFormat = KvrplHelper.DateToBaseFormat(periodName.Value);
            objArray[index1] = (object) baseFormat;
            int index2 = 3;
            // ISSUE: variable of a boxed type
            short companyId = this.company.CompanyId;
            objArray[index2] = (object) companyId;
            int index3 = 4;
            // ISSUE: variable of a boxed type
            int complexId = Options.Complex.ComplexId;
            objArray[index3] = (object) complexId;
            int index4 = 5;
            // ISSUE: variable of a boxed type
            short int16 = Convert.ToInt16(this.cmbService.SelectedValue);
            objArray[index4] = (object) int16;
            string queryString = string.Format(format, objArray);
            evidenceList2 = session.CreateQuery(queryString).List<Evidence>();
          }
          foreach (Evidence evidence1 in (IEnumerable<Evidence>) evidenceList2)
          {
            this.session.Clear();
            Evidence evidence2 = new Evidence();
            evidence2.Counter = evidence1.Counter;
            evidence2.Period = Options.Period;
            if (flag)
              evidence2.Past = evidence1.Current;
            evidence2.DBeg = evidence1.DEnd.AddDays(1.0);
            evidence2.DEnd = this.dtmCounterDay.Value;
            evidence2.Current = evidence2.Past;
            evidence2.UName = Options.Login;
            evidence2.DEdit = DateTime.Now;
            try
            {
              this.session.Save((object) evidence2);
              this.session.Flush();
            }
            catch (Exception ex)
            {
              KvrplHelper.WriteLog(ex, (LsClient) null);
            }
          }
          this.session.Clear();
          this.LoadEvidence();
        }
      }
    }

    private void dgvEvidence_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.btnSave.Enabled = true;
      this.btnAdd.Enabled = false;
      this.btnDelete.Enabled = false;
      if (this.dgvEvidence.CurrentRow == null || this.dgvEvidence.CurrentRow.Cells["Past"].ColumnIndex != e.ColumnIndex && this.dgvEvidence.CurrentRow.Cells["Current"].ColumnIndex != e.ColumnIndex && this.dgvEvidence.CurrentRow.Cells["DBeg"].ColumnIndex != e.ColumnIndex && this.dgvEvidence.CurrentRow.Cells["DEnd"].ColumnIndex != e.ColumnIndex)
        return;
      this.lastCurrent = Convert.ToDouble(KvrplHelper.ChangeSeparator(this.dgvEvidence.CurrentRow.Cells["Current"].Value.ToString()));
      this.lastPast = Convert.ToDouble(KvrplHelper.ChangeSeparator(this.dgvEvidence.CurrentRow.Cells["Past"].Value.ToString()));
      this.lastDBeg = Convert.ToDateTime(KvrplHelper.ChangeSeparator(this.dgvEvidence.CurrentRow.Cells["DBeg"].Value.ToString()));
      this.lastDEnd = Convert.ToDateTime(KvrplHelper.ChangeSeparator(this.dgvEvidence.CurrentRow.Cells["DEnd"].Value.ToString()));
    }

    private void dgvEvidence_CellLeave(object sender, DataGridViewCellEventArgs e)
    {
      if (!this.btnSave.Enabled || this.insertRecord || this.dgvEvidence.CurrentRow.Cells["Current"].ColumnIndex != e.ColumnIndex && this.dgvEvidence.CurrentRow.Cells["DEnd"].ColumnIndex != e.ColumnIndex && this.dgvEvidence.CurrentRow.Cells["Past"].ColumnIndex != e.ColumnIndex && this.dgvEvidence.CurrentRow.Cells["DBeg"].ColumnIndex != e.ColumnIndex)
        return;
      this.btnAdd.Enabled = true;
      this.btnDelete.Enabled = true;
      this.btnSave.Enabled = false;
      Evidence dataBoundItem = (Evidence) this.dgvEvidence.CurrentRow.DataBoundItem;
      DateTime dbeg = dataBoundItem.DBeg;
      try
      {
        dataBoundItem.Past = Convert.ToDouble(KvrplHelper.ChangeSeparator(this.dgvEvidence.CurrentRow.Cells["Past"].Value.ToString()));
        dataBoundItem.Current = Convert.ToDouble(KvrplHelper.ChangeSeparator(this.dgvEvidence.CurrentRow.Cells["Current"].Value.ToString()));
        dataBoundItem.DBeg = Convert.ToDateTime(this.dgvEvidence.CurrentRow.Cells["DBeg"].Value);
        dataBoundItem.DEnd = this.dtmCounterDay.Value;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Некорректный формат показаний", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        //bool flag = true;
      }
      if (this.dgvEvidence.CurrentRow.Cells["Num"].Value == null)
      {
        int num = (int) MessageBox.Show("Не выбран счетчик!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        //bool flag = true;
      }
      if (!this.Control(dataBoundItem))
      {
        this.dgvEvidence.CurrentRow.Cells["Current"].Value = (object) this.lastCurrent;
        this.dgvEvidence.CurrentRow.Cells["Past"].Value = (object) this.lastPast;
        this.dgvEvidence.CurrentRow.Cells["DBeg"].Value = (object) this.lastDBeg;
        this.dgvEvidence.CurrentRow.Cells["DEnd"].Value = (object) this.lastDEnd;
        dataBoundItem.Current = this.lastCurrent;
        dataBoundItem.Past = this.lastPast;
        dataBoundItem.DBeg = this.lastDBeg;
        dataBoundItem.DEnd = this.lastDEnd;
      }
      else
      {
        dataBoundItem.UName = Options.Login;
        dataBoundItem.DEdit = DateTime.Now;
        try
        {
          if (dataBoundItem.Period == null)
          {
            dataBoundItem.Period = Options.Period;
            this.insertRecord = false;
            this.session.Save((object) dataBoundItem);
            this.session.Flush();
          }
          else
            this.session.CreateQuery("update Evidence set DBeg=:dbeg,DEnd=:dend,Past=:past,Current=:current,UName=:user,DEdit=:dedit where Period.PeriodId=:period and Counter.CounterId=:counter and DBeg=:oldbeg").SetParameter<DateTime>("dbeg", dataBoundItem.DBeg).SetParameter<DateTime>("dend", dataBoundItem.DEnd).SetDouble("past", dataBoundItem.Past).SetParameter<double>("current", dataBoundItem.Current).SetParameter<int>("period", Options.Period.PeriodId).SetParameter<Counter>("counter", dataBoundItem.Counter).SetParameter<DateTime>("oldbeg", dbeg).SetParameter<string>("user", Options.Login).SetDateTime("dedit", DateTime.Now).ExecuteUpdate();
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Невозможно сохранить изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      this.dgvEvidence.CurrentRow.Cells["DBeg"].Value = (object) ((Evidence) this.dgvEvidence.CurrentRow.DataBoundItem).DBeg;
      this.dgvEvidence.CurrentRow.Cells["DEnd"].Value = (object) ((Evidence) this.dgvEvidence.CurrentRow.DataBoundItem).DEnd;
      this.dgvEvidence.CurrentRow.Cells["Past"].Value = (object) ((Evidence) this.dgvEvidence.CurrentRow.DataBoundItem).Past;
      this.dgvEvidence.CurrentRow.Cells["Current"].Value = (object) ((Evidence) this.dgvEvidence.CurrentRow.DataBoundItem).Current;
      this.dgvEvidence.CurrentRow.Cells["Volume"].Value = (object) ((Evidence) this.dgvEvidence.CurrentRow.DataBoundItem).Volume;
      this.dgvEvidence.Refresh();
    }

    private bool Control(Evidence evidence)
    {
      DateTime? periodName = Options.Period.PeriodName;
      DateTime dateTime1 = periodName.Value;
      periodName = this.monthClosed.PeriodName;
      DateTime dateTime2 = periodName.Value;
      if (dateTime1 <= dateTime2)
      {
        int num = (int) MessageBox.Show("Невозможно внести показания в закрытый месяц", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (evidence.DEnd <= evidence.DBeg)
      {
        int num = (int) MessageBox.Show("Дата настоящего меньше или равна дате предыдущего", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (evidence.Current < evidence.Past)
      {
        if (this.city == 3 && evidence.Volume < Decimal.Zero)
        {
          int num = (int) MessageBox.Show("Невозможно ввести отрицательный расход.Введите сначала настоящие показания", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (MessageBox.Show("Настоящие показания меньше предыдущих. Продолжить?", "Внимание!!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand) == DialogResult.Cancel || !KvrplHelper.IsGoodEvidence(evidence, this.session))
          return false;
      }
      if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2 && (evidence.Current - 500.0 > evidence.Past && (this.city != 1 || (int) evidence.Counter.Service.ServiceId != 23) || this.city == 1 && (int) evidence.Counter.Service.ServiceId == 23 && evidence.Current - 1000.0 > evidence.Past) && MessageBox.Show("Слишком большой расход. Продолжить?", "Внимание!!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand) == DialogResult.Cancel)
        return false;
      if (!(evidence.DEnd >= Options.Period.PeriodName.Value.AddMonths(1)))
        return true;
      int num1 = (int) MessageBox.Show("Невозможно внести показания в будущее время", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private void dgvEvidence_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (((DataGridView) sender).DataSource == null)
        return;
      DataGridViewRow row = ((DataGridView) sender).Rows[e.RowIndex];
      if (((Evidence) row.DataBoundItem).Period != null && ((Evidence) row.DataBoundItem).Period.PeriodId == this.monthClosed.PeriodId + 1)
      {
        row.DefaultCellStyle.BackColor = Color.PapayaWhip;
        row.DefaultCellStyle.ForeColor = Color.Black;
        row.DefaultCellStyle.Font = new Font(this.dgvEvidence.Font, FontStyle.Regular);
      }
      else if (((Evidence) row.DataBoundItem).Period != null)
      {
        row.DefaultCellStyle.BackColor = Color.White;
        row.DefaultCellStyle.ForeColor = Color.Gray;
        row.DefaultCellStyle.Font = new Font(this.dgvEvidence.Font, FontStyle.Regular);
      }
      else
      {
        row.DefaultCellStyle.BackColor = Color.White;
        row.DefaultCellStyle.ForeColor = Color.Black;
        row.DefaultCellStyle.Font = new Font(this.dgvEvidence.Font, FontStyle.Italic);
      }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      this.editEvidence = !this.editEvidence;
      if (this.editEvidence)
      {
        this.lblEdit.Visible = true;
        this.btnEdit.BackColor = Color.DarkOrange;
        this.tmrEvidence.Start();
      }
      else
      {
        this.lblEdit.Visible = false;
        this.btnEdit.BackColor = this.pnBtn.BackColor;
        this.tmrEvidence.Stop();
      }
      this.LoadEvidence();
    }

    private void cbArhive_CheckedChanged(object sender, EventArgs e)
    {
      this.LoadEvidence();
    }

    private void LoadDetailEvidence()
    {
      this.Cursor = Cursors.WaitCursor;
      this.cbArchive.Visible = true;
      this.pnTools.Height = 73;
      if ((Convert.ToInt32(this.cmbBase.SelectedValue) == 2 || (int) this.level == 2) && Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
      {
        this.cbArchive.Enabled = false;
        this.cbArchive.Checked = false;
      }
      else
        this.cbArchive.Enabled = true;
      string str1 = "";
      string str2 = "";
      string str3 = "";
      string str4 = "";
      IList<DetailEvidence> detailEvidenceList = (IList<DetailEvidence>) new List<DetailEvidence>();
      if ((uint) Convert.ToInt16(this.cmbService.SelectedValue) > 0U)
        str1 = " and s.Service.ServiceId={4}";
      if (this.tcntrlCounters.SelectedTab == this.tpCounters)
        str2 = " isnull(s.ArchivesDate,'2999-12-31') desc,";
      if ((int) this.level == 2)
        str2 += " st.NameStr,DBA.lengthhome(s.Home.NHome),s.Home.HomeKorp,";
      if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
      {
        str2 += " DBA.lengthhome(s.LsClient.Flat.NFlat),";
        str4 = string.Format(" and s.LsClient.Complex.IdFk in ({0},{1})", (object) Options.Complex.IdFk, (object) Options.ComplexArenda.IdFk);
        if (!Options.Kvartplata)
          str4 = string.Format(" and s.LsClient.Complex.IdFk={0}", (object) Options.ComplexArenda.IdFk);
        if (!Options.Arenda)
          str4 = string.Format(" and s.LsClient.Complex.IdFk={0}", (object) Options.Complex.IdFk);
      }
      string str5 = !(Options.SortService == " s.ServiceId") ? " s.Service.ServiceName" : " s.Service.ServiceId";
      string str6 = (uint) Convert.ToInt32(this.cmbCounter.SelectedValue) <= 0U ? "" : " and de.Counter.Home.IdHome = {2}";
      string str7 = (uint) Convert.ToInt32(((Period) this.cmbMonth.SelectedItem).PeriodId) <= 0U ? "" : " and de.Month.PeriodId = {6}";
      string str8 = (uint) Convert.ToInt32(((Period) this.cmbPeriod.SelectedItem).PeriodId) <= 0U ? "" : " and de.Period.PeriodId = {5}";
      int periodId;
      if (!this.cbArchive.Checked)
      {
        str3 = " and de.Period.PeriodId={5}";
        periodId = Options.Period.PeriodId;
      }
      else
        periodId = ((Period) this.cmbPeriod.SelectedItem).PeriodId;
      if ((int) this.level == 3)
        detailEvidenceList = this.session.CreateQuery(string.Format("select de from DetailEvidence de join fetch de.Counter s left join fetch s.Service left join fetch s.TypeCounter where s.Complex.ComplexId={0} and s.Home.IdHome={2} and s.BaseCounter.Id={3} and s.Company.CompanyId={1} " + str4 + str1 + str6 + str3 + str8 + str7 + " order by " + str2 + "de.Period.PeriodId desc,de.Month.PeriodId desc," + str5 + ",regulatefld(s.CounterNum)", (object) Options.Complex.ComplexId, (object) this.company.CompanyId, (object) this.home.IdHome, this.cmbBase.SelectedValue, this.cmbService.SelectedValue, (object) periodId, (object) ((Period) this.cmbMonth.SelectedItem).PeriodId)).List<DetailEvidence>();
      if ((int) this.level == 2)
        detailEvidenceList = this.session.CreateQuery(string.Format("select de from DetailEvidence de join fetch de.Counter s left join fetch s.Service left join fetch s.TypeCounter left join fetch s.Home h left join fetch h.Str st where s.Complex.ComplexId={0} and s.Company.CompanyId={1} and s.BaseCounter.Id={3} " + str4 + str1 + str6 + str3 + str8 + str7 + " order by de.Period.PeriodId desc,de.Month.PeriodId desc" + str2 + str5 + ",regulatefld(s.CounterNum)", (object) Options.Complex.ComplexId, (object) this.company.CompanyId, (object) Convert.ToInt32(this.cmbCounter.SelectedValue), this.cmbBase.SelectedValue, this.cmbService.SelectedValue, (object) periodId, (object) ((Period) this.cmbMonth.SelectedItem).PeriodId)).List<DetailEvidence>();
      this.dgvDetailEvidence.Columns.Clear();
      this.dgvDetailEvidence.DataSource = (object) null;
      this.dgvDetailEvidence.DataSource = (object) detailEvidenceList;
      this.session.Clear();
      this.MySettingsDetailEvidence.GridName = "DetailEvidence";
      this.SetViewDetailEvidence();
      this.Cursor = Cursors.Default;
    }

    private void LoadSettingsDetailEvidence()
    {
      this.MySettingsDetailEvidence.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvDetailEvidence.Columns)
        this.MySettingsDetailEvidence.GetMySettings(column);
    }

    private void dgvDetailEvidence_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsDetailEvidence.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsDetailEvidence.Columns[this.MySettingsDetailEvidence.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsDetailEvidence.Save();
    }

    private void SetViewDetailEvidence()
    {
      KvrplHelper.AddComboBoxColumn(this.dgvDetailEvidence, 0, (IList) null, (string) null, (string) null, "Номер счетчика", "Num", 100, 100);
      KvrplHelper.AddComboBoxColumn(this.dgvDetailEvidence, 0, (IList) this.session.CreateCriteria(typeof (Period)).AddOrder(Order.Desc("PeriodId")).Add((ICriterion) NHibernate.Criterion.Restrictions.Not((ICriterion) NHibernate.Criterion.Restrictions.Eq("PeriodId", (object) 0))).Add((ICriterion) NHibernate.Criterion.Restrictions.Le("PeriodId", (object) Convert.ToInt32(this.session.CreateQuery("select max(Period.PeriodId) from DetailEvidence").UniqueResult()))).List<Period>(), "PeriodId", "PeriodName", "Период", "Period", 100, 100);
      KvrplHelper.AddComboBoxColumn(this.dgvDetailEvidence, 2, (IList) this.months, "PeriodId", "PeriodName", "Месяц", "Month", 100, 100);
      KvrplHelper.AddTextBoxColumn(this.dgvDetailEvidence, 3, "Тип", "Type", 100, true);
      this.dgvDetailEvidence.Columns["Evidence"].HeaderText = "Расход";
      this.dgvDetailEvidence.Columns["Evidence"].DefaultCellStyle.Format = "F8";
      if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
      {
        string str = string.Format(" and ls.Complex.IdFk in ({0},{1})", (object) Options.Complex.IdFk, (object) Options.ComplexArenda.IdFk);
        if (!Options.Kvartplata)
          str = string.Format(" and ls.Complex.IdFk={0}", (object) Options.ComplexArenda.IdFk);
        if (!Options.Arenda)
          str = string.Format(" and ls.Complex.IdFk={0}", (object) Options.Complex.IdFk);
        this.clients = (IList<LsClient>) new List<LsClient>();
        if ((int) this.level == 2)
          this.clients = this.session.CreateQuery(string.Format("select distinct ls from LsClient ls,Counter c,Flat f where c.LsClient=ls and ls.Company.CompanyId = {0} and ls.Flat=f and c.BaseCounter.Id={2} " + this.uslovie + str, (object) this.company.CompanyId, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), this.cmbBase.SelectedValue, (object) Convert.ToInt16(this.cmbService.SelectedValue))).List<LsClient>();
        if ((int) this.level == 3)
          this.clients = this.session.CreateQuery(string.Format("select distinct ls from LsClient ls,Counter c,Flat f where c.LsClient=ls and ls.Home.IdHome = {0} and ls.Flat=f and c.BaseCounter.Id={2} " + this.uslovie + str, (object) this.home.IdHome, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), this.cmbBase.SelectedValue, (object) Convert.ToInt16(this.cmbService.SelectedValue))).List<LsClient>();
        KvrplHelper.AddComboBoxColumn(this.dgvDetailEvidence, 0, (IList) this.clients, "ClientId", "SmallAddress", "Адрес", "LicAddress", 100, 100);
      }
      if ((Convert.ToInt32(this.cmbBase.SelectedValue) != 2 || (int) this.level == 2) && Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
      {
        this.homes = (IList<Home>) new List<Home>();
        if ((int) this.level == 2)
          this.homes = this.session.CreateQuery(string.Format("select distinct h from Home h left join fetch h.Str, HomeLink hl,Counter c where c.Home=h and hl.Home=h and hl.Company.CompanyId={0} and c.BaseCounter.Id={2} " + this.uslovie + " order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)", (object) this.company.CompanyId, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), this.cmbBase.SelectedValue, (object) Convert.ToInt16(this.cmbService.SelectedValue))).List<Home>();
        if ((int) this.level == 3)
          this.homes = this.session.CreateQuery(string.Format("select distinct h from Home h left join fetch h.Str, HomeLink hl,Counter c where c.Home=h and c.Home.IdHome={4} and c.BaseCounter.Id={2} and hl.Home=h and hl.Company.CompanyId={0} " + this.uslovie + " order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)", (object) this.company.CompanyId, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), this.cmbBase.SelectedValue, (object) Convert.ToInt16(this.cmbService.SelectedValue), (object) this.home.IdHome)).List<Home>();
        KvrplHelper.AddComboBoxColumn(this.dgvDetailEvidence, 0, (IList) this.homes, "IdHome", "Address", "Адрес", "Address", 180, 180);
      }
      IList<LsClient> lsClientList1 = (IList<LsClient>) new List<LsClient>();
      IList<Counter> counterList1 = (IList<Counter>) new List<Counter>();
      foreach (DataGridViewRow row in (IEnumerable) this.dgvDetailEvidence.Rows)
      {
        if (((DetailEvidence) row.DataBoundItem).Period != null)
          row.Cells["Period"].Value = (object) ((DetailEvidence) row.DataBoundItem).Period.PeriodId;
        if (((DetailEvidence) row.DataBoundItem).Month != null)
          row.Cells["Month"].Value = (object) ((DetailEvidence) row.DataBoundItem).Month.PeriodId;
        if ((int) ((DetailEvidence) row.DataBoundItem).Type == 0)
          row.Cells["Type"].Value = (object) "Показания";
        else if ((int) ((DetailEvidence) row.DataBoundItem).Type == 1)
          row.Cells["Type"].Value = (object) "Поверка";
        else if ((int) ((DetailEvidence) row.DataBoundItem).Type == 3)
        {
          row.Cells["Type"].Value = (object) "Расход по среднему";
        }
        else
        {
          string str1 = this.session.CreateSQLQuery("select scheme_name from dcScheme where scheme_type=16 and Scheme=:t").SetParameter<short>("t", ((DetailEvidence) row.DataBoundItem).Type).UniqueResult<string>();
          string str2 = str1 != null ? str1.Replace("расход=", "") : "";
          row.Cells["Type"].Value = (object) ("Расход по среднему(" + str2 + ")");
        }
        IList<Counter> counterList2;
        if (((int) this.level == 2 || (int) this.level == 3 && Convert.ToInt32(this.cmbBase.SelectedValue) != 2) && ((DetailEvidence) row.DataBoundItem).Home != null)
        {
          if (Convert.ToInt32(this.cmbBase.SelectedValue) != 2)
          {
            row.Cells["Address"].Value = (object) ((DetailEvidence) row.DataBoundItem).Home.IdHome;
            if (counterList1.Count == 0 || counterList1[0].Home.IdHome != ((DetailEvidence) row.DataBoundItem).Home.IdHome)
            {
              counterList2 = (IList<Counter>) new List<Counter>();
              counterList1 = (uint) Convert.ToInt16(this.cmbService.SelectedValue) <= 0U ? this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Home.IdHome", (object) ((DetailEvidence) row.DataBoundItem).Home.IdHome)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>() : this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Home.IdHome", (object) ((DetailEvidence) row.DataBoundItem).Home.IdHome)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) (Service) this.cmbService.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>();
            }
            row.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
            {
              DisplayStyleForCurrentCellOnly = true,
              ValueMember = "CounterId",
              DisplayMember = "AllInfo",
              DataSource = (object) counterList1
            };
            row.Cells["Num"].Value = (object) ((DetailEvidence) row.DataBoundItem).Counter.CounterId;
          }
          else
          {
            row.Cells["Address"].Value = (object) ((DetailEvidence) row.DataBoundItem).Home.IdHome;
            if (lsClientList1.Count == 0 || lsClientList1[0].Home.IdHome != ((DetailEvidence) row.DataBoundItem).Home.IdHome)
            {
              IList<LsClient> lsClientList2 = (IList<LsClient>) new List<LsClient>();
              lsClientList1 = this.session.CreateQuery(string.Format("select distinct ls from LsClient ls,Counter c,Flat f where c.LsClient=ls and ls.Home.IdHome = {0} and (c.ArchivesDate is null or c.ArchivesDate > '{1}') and ls.Flat=f and c.BaseCounter.Id={2} and c.Complex.ComplexId={4} " + this.uslovie, (object) ((DetailEvidence) row.DataBoundItem).Home.IdHome, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), this.cmbBase.SelectedValue, (object) Convert.ToInt16(this.cmbService.SelectedValue), (object) Options.Complex.ComplexId)).List<LsClient>();
            }
            row.Cells["LicAddress"] = (DataGridViewCell) new DataGridViewComboBoxCell()
            {
              DisplayStyleForCurrentCellOnly = true,
              ValueMember = "ClientId",
              DisplayMember = "SmallAddress",
              DataSource = (object) lsClientList1
            };
            row.Cells["LicAddress"].Value = (object) ((DetailEvidence) row.DataBoundItem).ClientId;
          }
        }
        int? clientId1;
        int num1;
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
        {
          clientId1 = ((DetailEvidence) row.DataBoundItem).ClientId;
          num1 = clientId1.HasValue ? 1 : 0;
        }
        else
          num1 = 0;
        if (num1 != 0)
        {
          row.Cells["LicAddress"].Value = (object) ((DetailEvidence) row.DataBoundItem).ClientId;
          int num2;
          if (counterList1.Count != 0)
          {
            int clientId2 = counterList1[0].LsClient.ClientId;
            clientId1 = ((DetailEvidence) row.DataBoundItem).ClientId;
            int valueOrDefault = clientId1.GetValueOrDefault();
            num2 = clientId2 == valueOrDefault ? (!clientId1.HasValue ? 1 : 0) : 1;
          }
          else
            num2 = 1;
          if (num2 != 0)
          {
            counterList2 = (IList<Counter>) new List<Counter>();
            counterList1 = (uint) Convert.ToInt16(this.cmbService.SelectedValue) <= 0U ? this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("LsClient.ClientId", (object) ((DetailEvidence) row.DataBoundItem).ClientId)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>() : this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("LsClient.ClientId", (object) ((DetailEvidence) row.DataBoundItem).ClientId)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) (Service) this.cmbService.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>();
          }
          row.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
          {
            DisplayStyleForCurrentCellOnly = true,
            ValueMember = "CounterId",
            DisplayMember = "AllInfo",
            DataSource = (object) counterList1
          };
          row.Cells["Num"].Value = (object) ((DetailEvidence) row.DataBoundItem).Counter.CounterId;
        }
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 4)
        {
          if (counterList1.Count == 0)
          {
            counterList2 = (IList<Counter>) new List<Counter>();
            counterList1 = (uint) Convert.ToInt16(this.cmbService.SelectedValue) <= 0U ? this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>() : this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) (Service) this.cmbService.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>();
          }
          row.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
          {
            DisplayStyleForCurrentCellOnly = true,
            ValueMember = "CounterId",
            DisplayMember = "AllInfo",
            DataSource = (object) counterList1
          };
          if (((DetailEvidence) row.DataBoundItem).Counter != null)
            row.Cells["Num"].Value = (object) ((DetailEvidence) row.DataBoundItem).Counter.CounterId;
        }
      }
      this.LoadSettingsDetailEvidence();
    }

    private void dgvDetailEvidence_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (((DataGridView) sender).DataSource == null)
        return;
      DataGridViewRow row = ((DataGridView) sender).Rows[e.RowIndex];
      if (((DetailEvidence) row.DataBoundItem).Period != null && ((DetailEvidence) row.DataBoundItem).Period.PeriodId == this.monthClosed.PeriodId + 1)
      {
        row.DefaultCellStyle.BackColor = Color.PapayaWhip;
        row.DefaultCellStyle.ForeColor = Color.Black;
      }
      else
      {
        row.DefaultCellStyle.BackColor = Color.White;
        row.DefaultCellStyle.ForeColor = Color.Gray;
      }
    }

    private void LoadAudit()
    {
      this.Cursor = Cursors.WaitCursor;
      this.btnAdd.Enabled = true;
      this.btnDelete.Enabled = true;
      this.btnSave.Enabled = false;
      this.pnTools.Height = 73;
      this.cbArchive.Visible = true;
      this.cbArchive.Enabled = true;
      IList<Audit> auditList = (IList<Audit>) new List<Audit>();
      this.session.Clear();
      this.session = Domain.CurrentSession;
      this.counters = (IList<Counter>) new List<Counter>();
      string str1 = "";
      string str2 = "";
      string str3 = "";
      string str4 = "";
      if ((uint) Convert.ToInt16(this.cmbService.SelectedValue) > 0U)
        str1 = " and s.Service.ServiceId={3}";
      if (this.tcntrlCounters.SelectedTab == this.tpCounters)
        str2 = " isnull(s.ArchivesDate,'2999-12-31') desc,";
      if ((int) this.level == 2)
        str2 += " st.NameStr,DBA.lengthhome(s.Home.NHome),s.Home.HomeKorp,";
      if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
      {
        str4 = string.Format(" and s.LsClient.Complex.IdFk in ({0},{1})", (object) Options.Complex.IdFk, (object) Options.ComplexArenda.IdFk);
        if (!Options.Kvartplata)
          str4 = string.Format(" and s.LsClient.Complex.IdFk={0}", (object) Options.ComplexArenda.IdFk);
        if (!Options.Arenda)
          str4 = string.Format(" and s.LsClient.Complex.IdFk={0}", (object) Options.Complex.IdFk);
        str2 += " DBA.lengthhome(s.LsClient.Flat.NFlat),";
      }
      string str5 = !(Options.SortService == " s.ServiceId") ? " s.Service.ServiceName" : " s.Service.ServiceId";
      int num = 0;
      if (this.pastTime)
        num = Options.Period.PeriodId;
      if (!this.cbArchive.Checked && !this.pastTime)
        str3 = " and a.DEnd>='{5}'";
      DateTime dateTime;
      if ((int) this.level == 3)
      {
        ISession session = this.session;
        string format = "select a from Audit a join fetch a.Counter s left join fetch s.Service left join fetch s.TypeCounter where a.Period.PeriodId={4} and s.Complex.ComplexId={0} and s.Home.IdHome={1} and s.BaseCounter.Id={2} and s.Company.CompanyId={6} " + str4 + str1 + str3 + " order by" + str2 + str5;
        object[] objArray = new object[7]{ (object) Options.Complex.ComplexId, (object) this.home.IdHome, this.cmbBase.SelectedValue, this.cmbService.SelectedValue, (object) num, null, null };
        int index1 = 5;
        dateTime = this.monthClosed.PeriodName.Value;
        string baseFormat = KvrplHelper.DateToBaseFormat(dateTime.AddMonths(1));
        objArray[index1] = (object) baseFormat;
        int index2 = 6;
        // ISSUE: variable of a boxed type
        short companyId = this.company.CompanyId;
        objArray[index2] = (object) companyId;
        string queryString = string.Format(format, objArray);
        auditList = session.CreateQuery(queryString).List<Audit>();
      }
      if ((int) this.level == 2)
      {
        ISession session = this.session;
        string format = "select a from Audit a join fetch a.Counter s left join fetch s.Service left join fetch s.TypeCounter left join fetch s.Home h left join fetch h.Str st where a.Period.PeriodId={4} and s.Complex.ComplexId={0} and s.Company.CompanyId={1} and s.BaseCounter.Id={2} " + str4 + str1 + str3 + " order by" + str2 + str5;
        object[] objArray = new object[6]{ (object) Options.Complex.ComplexId, (object) this.company.CompanyId, this.cmbBase.SelectedValue, this.cmbService.SelectedValue, (object) num, null };
        int index = 5;
        dateTime = this.monthClosed.PeriodName.Value;
        string baseFormat = KvrplHelper.DateToBaseFormat(dateTime.AddMonths(1));
        objArray[index] = (object) baseFormat;
        string queryString = string.Format(format, objArray);
        auditList = session.CreateQuery(queryString).List<Audit>();
      }
      this.dgvAudit.Columns.Clear();
      this.dgvAudit.DataSource = (object) null;
      this.dgvAudit.DataSource = (object) auditList;
      this.session.Clear();
      this.SetViewAudit();
      this.MySettingsAudit.GridName = "Audit";
      this.LoadSettingsAudit();
      this.Cursor = Cursors.Default;
    }

    private void LoadSettingsAudit()
    {
      this.MySettingsAudit.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvAudit.Columns)
        this.MySettingsAudit.GetMySettings(column);
    }

    private void dgvAudit_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsAudit.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsAudit.Columns[this.MySettingsAudit.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsAudit.Save();
    }

    private void SetViewAudit()
    {
      KvrplHelper.AddMaskDateColumn(this.dgvAudit, 0, "Дата начала", "DBeg");
      KvrplHelper.AddMaskDateColumn(this.dgvAudit, 1, "Дата окончания", "DEnd");
      this.dgvAudit.Columns["Note"].HeaderText = "Примечания";
      KvrplHelper.AddComboBoxColumn(this.dgvAudit, 0, (IList) null, (string) null, (string) null, "Номер счетчика", "Num", 100, 100);
      KvrplHelper.AddButtonColumn(this.dgvAudit, 3, "Схема", "Scheme", 100);
      DataGridViewComboBoxColumn viewComboBoxColumn = new DataGridViewComboBoxColumn();
      string str = string.Format(" and ls.Complex.IdFk in ({0},{1})", (object) Options.Complex.IdFk, (object) Options.ComplexArenda.IdFk);
      DateTime? periodName;
      if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
      {
        if (!Options.Kvartplata)
          str = string.Format(" and ls.Complex.IdFk={0}", (object) Options.ComplexArenda.IdFk);
        if (!Options.Arenda)
          str = string.Format(" and ls.Complex.IdFk={0}", (object) Options.Complex.IdFk);
        this.clients = (IList<LsClient>) new List<LsClient>();
        if ((int) this.level == 2)
        {
          ISession session = this.session;
          string format = "select distinct ls from LsClient ls,Counter c,Flat f where c.LsClient=ls and ls.Company.CompanyId = {0} and (c.ArchivesDate is null or c.ArchivesDate > '{1}') and ls.Flat=f and c.BaseCounter.Id={2} " + this.uslovie + str;
          object[] objArray = new object[4];
          objArray[0] = (object) this.company.CompanyId;
          int index1 = 1;
          periodName = Options.Period.PeriodName;
          string baseFormat = KvrplHelper.DateToBaseFormat(periodName.Value);
          objArray[index1] = (object) baseFormat;
          int index2 = 2;
          object selectedValue = this.cmbBase.SelectedValue;
          objArray[index2] = selectedValue;
          int index3 = 3;
          // ISSUE: variable of a boxed type
          short int16 = Convert.ToInt16(this.cmbService.SelectedValue);
          objArray[index3] = (object) int16;
          string queryString = string.Format(format, objArray);
          this.clients = session.CreateQuery(queryString).List<LsClient>();
        }
        if ((int) this.level == 3)
        {
          ISession session = this.session;
          string format = "select distinct ls from LsClient ls,Counter c,Flat f where c.LsClient=ls and ls.Home.IdHome = {0} and (c.ArchivesDate is null or c.ArchivesDate > '{1}') and ls.Flat=f and c.BaseCounter.Id={2} " + this.uslovie + str;
          object[] objArray = new object[4];
          objArray[0] = (object) this.home.IdHome;
          int index1 = 1;
          periodName = Options.Period.PeriodName;
          string baseFormat = KvrplHelper.DateToBaseFormat(periodName.Value);
          objArray[index1] = (object) baseFormat;
          int index2 = 2;
          object selectedValue = this.cmbBase.SelectedValue;
          objArray[index2] = selectedValue;
          int index3 = 3;
          // ISSUE: variable of a boxed type
          short int16 = Convert.ToInt16(this.cmbService.SelectedValue);
          objArray[index3] = (object) int16;
          string queryString = string.Format(format, objArray);
          this.clients = session.CreateQuery(queryString).List<LsClient>();
        }
        viewComboBoxColumn.DataSource = (object) this.clients;
        viewComboBoxColumn.ValueMember = "ClientId";
        viewComboBoxColumn.DisplayMember = "SmallAddress";
        viewComboBoxColumn.HeaderText = "Адрес";
        viewComboBoxColumn.Name = "LicAddress";
        this.dgvAudit.Columns.Insert(0, (DataGridViewColumn) viewComboBoxColumn);
      }
      if ((Convert.ToInt32(this.cmbBase.SelectedValue) != 2 || (int) this.level == 2) && Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
      {
        this.homes = (IList<Home>) new List<Home>();
        if ((int) this.level == 2)
        {
          ISession session = this.session;
          string format = "select distinct h from Home h left join fetch h.Str, HomeLink hl,Counter c where c.Home=h and hl.Home=h and hl.Company.CompanyId={0} and c.BaseCounter.Id={2} " + this.uslovie + " order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)";
          object[] objArray = new object[4];
          objArray[0] = (object) this.company.CompanyId;
          int index1 = 1;
          periodName = Options.Period.PeriodName;
          string baseFormat = KvrplHelper.DateToBaseFormat(periodName.Value);
          objArray[index1] = (object) baseFormat;
          int index2 = 2;
          object selectedValue = this.cmbBase.SelectedValue;
          objArray[index2] = selectedValue;
          int index3 = 3;
          // ISSUE: variable of a boxed type
          short int16 = Convert.ToInt16(this.cmbService.SelectedValue);
          objArray[index3] = (object) int16;
          string queryString = string.Format(format, objArray);
          this.homes = session.CreateQuery(queryString).List<Home>();
        }
        if ((int) this.level == 3)
        {
          ISession session = this.session;
          string format = "select distinct h from Home h left join fetch h.Str, HomeLink hl,Counter c where c.Home=h and c.Home.IdHome={4} and c.BaseCounter.Id={2} and hl.Home=h and hl.Company.CompanyId={0} " + this.uslovie + " order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)";
          object[] objArray = new object[5];
          objArray[0] = (object) this.company.CompanyId;
          int index1 = 1;
          periodName = Options.Period.PeriodName;
          string baseFormat = KvrplHelper.DateToBaseFormat(periodName.Value);
          objArray[index1] = (object) baseFormat;
          int index2 = 2;
          object selectedValue = this.cmbBase.SelectedValue;
          objArray[index2] = selectedValue;
          int index3 = 3;
          // ISSUE: variable of a boxed type
          short int16 = Convert.ToInt16(this.cmbService.SelectedValue);
          objArray[index3] = (object) int16;
          int index4 = 4;
          // ISSUE: variable of a boxed type
          int idHome = this.home.IdHome;
          objArray[index4] = (object) idHome;
          string queryString = string.Format(format, objArray);
          this.homes = session.CreateQuery(queryString).List<Home>();
        }
        KvrplHelper.AddComboBoxColumn(this.dgvAudit, 0, (IList) this.homes, "IdHome", "Address", "Адрес", "Address", 180, 180);
      }
      KvrplHelper.ViewEdit(this.dgvAudit);
      IList<LsClient> lsClientList1 = (IList<LsClient>) new List<LsClient>();
      IList<Counter> counterList1 = (IList<Counter>) new List<Counter>();
      foreach (DataGridViewRow row in (IEnumerable) this.dgvAudit.Rows)
      {
        row.Cells["DBeg"].Value = (object) ((Audit) row.DataBoundItem).DBeg;
        row.Cells["DEnd"].Value = (object) ((Audit) row.DataBoundItem).DEnd;
        row.Cells["Scheme"].Value = (object) ((Audit) row.DataBoundItem).Scheme;
        IList<Counter> counterList2;
        if (((int) this.level == 2 || (int) this.level == 3 && Convert.ToInt32(this.cmbBase.SelectedValue) != 2) && ((Audit) row.DataBoundItem).Home != null)
        {
          if (Convert.ToInt32(this.cmbBase.SelectedValue) != 2)
          {
            row.Cells["Address"].Value = (object) ((Audit) row.DataBoundItem).Home.IdHome;
            if (counterList1.Count == 0 || counterList1[0].Home.IdHome != ((Audit) row.DataBoundItem).Home.IdHome)
            {
              counterList2 = (IList<Counter>) new List<Counter>();
              counterList1 = (uint) Convert.ToInt16(this.cmbService.SelectedValue) <= 0U ? this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.IsNull("MainCounter")).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Home.IdHome", (object) ((Audit) row.DataBoundItem).Home.IdHome)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>() : this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.IsNull("MainCounter")).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Home.IdHome", (object) ((Audit) row.DataBoundItem).Home.IdHome)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) (Service) this.cmbService.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>();
            }
            row.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
            {
              DisplayStyleForCurrentCellOnly = true,
              ValueMember = "CounterId",
              DisplayMember = "AllInfo",
              DataSource = (object) counterList1
            };
            row.Cells["Num"].Value = (object) ((Audit) row.DataBoundItem).Counter.CounterId;
          }
          else
          {
            row.Cells["Address"].Value = (object) ((Audit) row.DataBoundItem).Home.IdHome;
            if (lsClientList1.Count == 0 || lsClientList1[0].Home.IdHome != ((Audit) row.DataBoundItem).Home.IdHome)
            {
              IList<LsClient> lsClientList2 = (IList<LsClient>) new List<LsClient>();
              ISession session = this.session;
              string format = "select distinct ls from LsClient ls,Counter c,Flat f where c.LsClient=ls and ls.Home.IdHome = {0} and (c.ArchivesDate is null or c.ArchivesDate > '{1}') and ls.Flat=f and c.BaseCounter.Id={2} and c.Complex.ComplexId={4} " + this.uslovie + str;
              object[] objArray = new object[5];
              objArray[0] = (object) ((Audit) row.DataBoundItem).Home.IdHome;
              int index1 = 1;
              periodName = Options.Period.PeriodName;
              string baseFormat = KvrplHelper.DateToBaseFormat(periodName.Value);
              objArray[index1] = (object) baseFormat;
              int index2 = 2;
              object selectedValue = this.cmbBase.SelectedValue;
              objArray[index2] = selectedValue;
              int index3 = 3;
              // ISSUE: variable of a boxed type
              short int16 = Convert.ToInt16(this.cmbService.SelectedValue);
              objArray[index3] = (object) int16;
              int index4 = 4;
              // ISSUE: variable of a boxed type
              int complexId = Options.Complex.ComplexId;
              objArray[index4] = (object) complexId;
              string queryString = string.Format(format, objArray);
              lsClientList1 = session.CreateQuery(queryString).List<LsClient>();
            }
            row.Cells["LicAddress"] = (DataGridViewCell) new DataGridViewComboBoxCell()
            {
              DisplayStyleForCurrentCellOnly = true,
              ValueMember = "ClientId",
              DisplayMember = "SmallAddress",
              DataSource = (object) lsClientList1
            };
            row.Cells["LicAddress"].Value = (object) ((Audit) row.DataBoundItem).ClientId;
          }
        }
        int? clientId1;
        int num1;
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
        {
          clientId1 = ((Audit) row.DataBoundItem).ClientId;
          num1 = clientId1.HasValue ? 1 : 0;
        }
        else
          num1 = 0;
        if (num1 != 0)
        {
          row.Cells["LicAddress"].Value = (object) ((Audit) row.DataBoundItem).ClientId;
          int num2;
          if (counterList1.Count != 0)
          {
            int clientId2 = counterList1[0].LsClient.ClientId;
            clientId1 = ((Audit) row.DataBoundItem).ClientId;
            int valueOrDefault = clientId1.GetValueOrDefault();
            num2 = clientId2 == valueOrDefault ? (!clientId1.HasValue ? 1 : 0) : 1;
          }
          else
            num2 = 1;
          if (num2 != 0)
          {
            counterList2 = (IList<Counter>) new List<Counter>();
            counterList1 = (uint) Convert.ToInt16(this.cmbService.SelectedValue) <= 0U ? this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.IsNull("MainCounter")).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("LsClient.ClientId", (object) ((Audit) row.DataBoundItem).ClientId)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>() : this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.IsNull("MainCounter")).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("LsClient.ClientId", (object) ((Audit) row.DataBoundItem).ClientId)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) (Service) this.cmbService.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>();
          }
          row.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
          {
            DisplayStyleForCurrentCellOnly = true,
            ValueMember = "CounterId",
            DisplayMember = "AllInfo",
            DataSource = (object) counterList1
          };
          row.Cells["Num"].Value = (object) ((Audit) row.DataBoundItem).Counter.CounterId;
        }
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 4)
        {
          if (counterList1.Count == 0)
          {
            counterList2 = (IList<Counter>) new List<Counter>();
            counterList1 = (uint) Convert.ToInt16(this.cmbService.SelectedValue) <= 0U ? this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>() : this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) (Service) this.cmbService.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>();
          }
          row.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
          {
            DisplayStyleForCurrentCellOnly = true,
            ValueMember = "CounterId",
            DisplayMember = "AllInfo",
            DataSource = (object) counterList1
          };
          if (((Audit) row.DataBoundItem).Counter != null)
            row.Cells["Num"].Value = (object) ((Audit) row.DataBoundItem).Counter.CounterId;
        }
      }
    }

    private void dgvAudit_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (!this.dgvAudit.IsCurrentCellDirty)
        return;
      this.dgvAudit.CommitEdit(DataGridViewDataErrorContexts.Commit);
      if ((Convert.ToInt32(this.cmbBase.SelectedValue) == 1 || Convert.ToInt32(this.cmbBase.SelectedValue) == 3) && (int) Convert.ToInt16(this.cmbService.SelectedValue) != 0 && this.dgvAudit.CurrentCell.ColumnIndex == this.dgvAudit.Rows[this.dgvAudit.CurrentRow.Index].Cells["Address"].ColumnIndex)
      {
        this.session = Domain.CurrentSession;
        IList<Counter> counterList = (IList<Counter>) new List<Counter>();
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 1 || Convert.ToInt32(this.cmbBase.SelectedValue) == 3)
          counterList = this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.IsNull("MainCounter")).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Home.IdHome", this.dgvAudit.CurrentRow.Cells["Address"].Value)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) (Service) this.cmbService.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).List<Counter>();
        this.dgvAudit.CurrentRow.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "CounterId",
          DisplayMember = "AllInfo",
          DataSource = (object) counterList
        };
      }
      if ((int) Convert.ToInt16(this.cmbBase.SelectedValue) == 2 && (int) Convert.ToInt16(this.cmbService.SelectedValue) != 0 && this.dgvAudit.CurrentCell.ColumnIndex == this.dgvAudit.Rows[this.dgvAudit.CurrentRow.Index].Cells["LicAddress"].ColumnIndex)
      {
        this.session = Domain.CurrentSession;
        IList<Counter> counterList1 = (IList<Counter>) new List<Counter>();
        IList<Counter> counterList2 = this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.IsNull("MainCounter")).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("LsClient.ClientId", this.dgvAudit.CurrentRow.Cells["LicAddress"].Value)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) (Service) this.cmbService.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).List<Counter>();
        this.dgvAudit.CurrentRow.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "CounterId",
          DisplayMember = "AllInfo",
          DataSource = (object) counterList2
        };
      }
      if ((int) Convert.ToInt16(this.cmbService.SelectedValue) == 0 && Convert.ToInt32(this.cmbBase.SelectedValue) != 4 && ((int) this.level == 2 && (Convert.ToInt32(this.cmbBase.SelectedValue) != 2 && this.dgvAudit.CurrentCell.ColumnIndex == this.dgvAudit.Rows[this.dgvAudit.CurrentRow.Index].Cells["Address"].ColumnIndex || Convert.ToInt32(this.cmbBase.SelectedValue) == 2 && this.dgvAudit.CurrentCell.ColumnIndex == this.dgvAudit.Rows[this.dgvAudit.CurrentRow.Index].Cells["LicAddress"].ColumnIndex) || (int) this.level == 3 && (Convert.ToInt32(this.cmbBase.SelectedValue) == 2 && this.dgvAudit.CurrentCell.ColumnIndex == this.dgvAudit.Rows[this.dgvAudit.CurrentRow.Index].Cells["LicAddress"].ColumnIndex || Convert.ToInt32(this.cmbBase.SelectedValue) != 2 && this.dgvAudit.CurrentCell.ColumnIndex == this.dgvAudit.Rows[this.dgvAudit.CurrentRow.Index].Cells["Address"].ColumnIndex)))
      {
        this.session = Domain.CurrentSession;
        IList<Counter> counterList = (IList<Counter>) new List<Counter>();
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
          counterList = this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.IsNull("MainCounter")).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("LsClient.ClientId", this.dgvAudit.CurrentRow.Cells["LicAddress"].Value)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).List<Counter>();
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 1 || Convert.ToInt32(this.cmbBase.SelectedValue) == 3)
          counterList = this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.IsNull("MainCounter")).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Home.IdHome", this.dgvAudit.CurrentRow.Cells["Address"].Value)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).List<Counter>();
        this.dgvAudit.CurrentRow.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "CounterId",
          DisplayMember = "AllInfo",
          DataSource = (object) counterList
        };
      }
      if ((int) this.level == 2 && Convert.ToInt32(this.cmbBase.SelectedValue) == 2 && this.dgvAudit.CurrentCell.ColumnIndex == this.dgvAudit.Rows[this.dgvAudit.CurrentRow.Index].Cells["Address"].ColumnIndex)
      {
        this.session = Domain.CurrentSession;
        IList<LsClient> lsClientList1 = (IList<LsClient>) new List<LsClient>();
        string str1;
        if ((uint) Convert.ToInt16(this.cmbService.SelectedValue) > 0U)
        {
          try
          {
            this.dgvAudit.Columns["ServiceName"].Visible = false;
          }
          catch
          {
          }
          str1 = "and c.Service.ServiceId={3}";
        }
        else
          str1 = "";
        string str2 = string.Format(" and ls.Complex.IdFk in ({0},{1})", (object) Options.Complex.IdFk, (object) Options.ComplexArenda.IdFk);
        if (!Options.Kvartplata)
          str2 = string.Format(" and ls.Complex.IdFk={0}", (object) Options.ComplexArenda.IdFk);
        if (!Options.Arenda)
          str2 = string.Format(" and ls.Complex.IdFk={0}", (object) Options.Complex.IdFk);
        IList<LsClient> lsClientList2 = this.session.CreateQuery(string.Format("select distinct ls from LsClient ls,Counter c,Flat f where c.LsClient=ls and ls.Home.IdHome = {0} and (c.ArchivesDate is null or c.ArchivesDate > '{1}') and ls.Flat=f and c.BaseCounter.Id={2} " + str1 + str2, this.dgvAudit.CurrentRow.Cells["Address"].Value, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), this.cmbBase.SelectedValue, (object) Convert.ToInt16(this.cmbService.SelectedValue))).List<LsClient>();
        this.dgvAudit.CurrentRow.Cells["LicAddress"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "ClientId",
          DisplayMember = "SmallAddress",
          DataSource = (object) lsClientList2
        };
      }
    }

    private void dgvAudit_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex <= 0 || e.RowIndex < 0 || !(this.dgvAudit.Columns[e.ColumnIndex].Name == "Scheme"))
        return;
      short id = ((Audit) this.dgvAudit.CurrentRow.DataBoundItem).Scheme;
      FrmScheme frmScheme = new FrmScheme((short) 4, id);
      if (frmScheme.ShowDialog() == DialogResult.OK)
        id = frmScheme.CurrentId();
      this.dgvAudit.CurrentRow.Cells["Scheme"].Value = (object) id;
      frmScheme.Dispose();
      this.btnAdd.Enabled = false;
      this.btnSave.Enabled = true;
      this.btnDelete.Enabled = false;
    }

    private void InsertAudit()
    {
      this.insertRecord = true;
      Audit audit1 = new Audit();
      if (!this.pastTime)
      {
        DateTime? periodName = Options.Period.PeriodName;
        DateTime dateTime1 = periodName.Value;
        periodName = this.monthClosed.PeriodName;
        DateTime dateTime2 = periodName.Value;
        if (dateTime1 <= dateTime2)
        {
          Audit audit2 = audit1;
          periodName = this.monthClosed.PeriodName;
          DateTime dateTime3 = periodName.Value.AddMonths(1);
          audit2.DBeg = dateTime3;
        }
        else
        {
          Audit audit2 = audit1;
          periodName = Options.Period.PeriodName;
          DateTime dateTime3 = periodName.Value;
          audit2.DBeg = dateTime3;
        }
      }
      else
        audit1.DBeg = this.monthClosed.PeriodName.Value;
      audit1.DEnd = audit1.DBeg.AddMonths(3).AddDays(-1.0);
      audit1.Scheme = (short) 0;
      IList<Audit> auditList = (IList<Audit>) new List<Audit>();
      if ((uint) this.dgvAudit.Rows.Count > 0U)
        auditList = (IList<Audit>) (this.dgvAudit.DataSource as List<Audit>);
      auditList.Add(audit1);
      this.dgvAudit.Columns.Clear();
      this.dgvAudit.DataSource = (object) null;
      this.dgvAudit.DataSource = (object) auditList;
      this.SetViewAudit();
      this.LoadSettingsAudit();
      this.dgvAudit.CurrentCell = this.dgvAudit.Rows[this.dgvAudit.Rows.Count - 1].Cells[0];
    }

    private bool SaveAudit()
    {
      if (this.dgvAudit.Rows.Count > 0 && this.dgvAudit.CurrentRow != null)
      {
        Audit dataBoundItem = (Audit) this.dgvAudit.CurrentRow.DataBoundItem;
        DateTime? periodName = this.monthClosed.PeriodName;
        DateTime dateTime1 = periodName.Value;
        dateTime1 = dateTime1.AddMonths(1);
        DateTime dateTime2 = dateTime1.AddDays(-1.0);
        if (this.dgvAudit.CurrentRow.Cells["DBeg"].Value != null)
        {
          try
          {
            dataBoundItem.DBeg = Convert.ToDateTime(this.dgvAudit.CurrentRow.Cells["DBeg"].Value);
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Дата начала введена некорректно", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
          if (this.dgvAudit.CurrentRow.Cells["DEnd"].Value != null)
          {
            try
            {
              dataBoundItem.DEnd = Convert.ToDateTime(this.dgvAudit.CurrentRow.Cells["DEnd"].Value);
            }
            catch (Exception ex)
            {
              int num = (int) MessageBox.Show("Дата окончания введена некорректно", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
            if (dataBoundItem.DBeg > dataBoundItem.DEnd)
            {
              int num = (int) MessageBox.Show("Дата начала больше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
            if (!this.pastTime && (this.insertRecord && (dataBoundItem.DBeg <= dateTime2 || dataBoundItem.DEnd <= dateTime2)))
            {
              int num = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return false;
            }
            int num1;
            if (this.pastTime)
            {
              DateTime dend = dataBoundItem.DEnd;
              periodName = this.monthClosed.PeriodName;
              dateTime1 = periodName.Value;
              DateTime dateTime3 = dateTime1.AddMonths(1);
              num1 = dend >= dateTime3 ? 1 : 0;
            }
            else
              num1 = 0;
            if (num1 != 0)
            {
              int num2 = (int) MessageBox.Show("Невозможно внести запись в открытом периоде", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
            DateTime dbeg1 = dataBoundItem.DBeg;
            dateTime1 = DateTime.Now;
            DateTime dateTime4 = dateTime1.AddYears(-3);
            int num3;
            if (!(dbeg1 <= dateTime4))
            {
              DateTime dbeg2 = dataBoundItem.DBeg;
              dateTime1 = DateTime.Now;
              DateTime dateTime3 = dateTime1.AddYears(3);
              num3 = dbeg2 >= dateTime3 ? 1 : 0;
            }
            else
              num3 = 1;
            if (num3 != 0 && MessageBox.Show("Дата начала отличается от текущей более, чем на 3 года. Продолжить", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
              return false;
            if (this.dgvAudit.CurrentRow.Cells["Note"].Value == null)
              dataBoundItem.Note = "";
            if (this.dgvAudit.CurrentRow.Cells["Num"].Value == null)
            {
              int num2 = (int) MessageBox.Show("Не выбран счетчик!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
            dataBoundItem.Counter = this.session.Get<Counter>(this.dgvAudit.CurrentRow.Cells["Num"].Value);
            dataBoundItem.Scheme = Convert.ToInt16(this.dgvAudit.CurrentRow.Cells["Scheme"].Value);
            dataBoundItem.Period = this.pastTime ? Options.Period : this.session.Get<Period>((object) 0);
            dataBoundItem.UName = Options.Login;
            Audit audit1 = dataBoundItem;
            dateTime1 = DateTime.Now;
            DateTime date = dateTime1.Date;
            audit1.DEdit = date;
            IList<CmpParam> cmpParamList = this.session.CreateQuery("select c from CmpParam c where c.Param_id=226 and c.Company_id=:cid").SetParameter<short>("cid", this.company.CompanyId).List<CmpParam>();
            if (cmpParamList != null && cmpParamList.Count > 0)
            {
              foreach (CmpParam cmpParam in (IEnumerable<CmpParam>) cmpParamList)
              {
                if (cmpParam.Dbeg <= dataBoundItem.DBeg && cmpParam.Dend >= dataBoundItem.DBeg)
                {
                  int num2 = (int) MessageBox.Show("Показания прибора учета не будут учитываться на период поверки!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                  break;
                }
                if (cmpParam.Dbeg <= dataBoundItem.DBeg && dataBoundItem.DBeg <= cmpParam.Dend)
                {
                  int num2 = (int) MessageBox.Show("Показания прибора учета не будут учитываться на период поверки!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                  break;
                }
              }
            }
            try
            {
              if (this.insertRecord)
              {
                this.insertRecord = false;
                this.session.Save((object) dataBoundItem);
                this.session.Flush();
              }
              else
              {
                IList<Audit> auditList = (IList<Audit>) new List<Audit>();
                string str1 = "";
                string str2 = "";
                string str3 = "";
                int num2 = 0;
                if (this.pastTime)
                  num2 = Options.Period.PeriodId;
                if ((uint) Convert.ToInt16(this.cmbService.SelectedValue) > 0U)
                  str1 = " and s.Service.ServiceId={3}";
                if (this.tcntrlCounters.SelectedTab == this.tpCounters)
                  str2 = " isnull(s.ArchivesDate,'2999-12-31') desc,";
                if ((int) this.level == 2)
                  str2 += " st.NameStr,DBA.lengthhome(s.Home.NHome),s.Home.HomeKorp,";
                if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
                  str2 += " DBA.lengthhome(s.LsClient.Flat.NFlat),";
                string str4 = !(Options.SortService == " s.ServiceId") ? " s.Service.ServiceName" : " s.Service.ServiceId";
                if (!this.cbArchive.Checked)
                  str3 = " and a.DEnd>='{5}'";
                if ((int) this.level == 3)
                {
                  ISession session = this.session;
                  string format = "select a from Audit a join fetch a.Counter s left join fetch s.Service left join fetch s.TypeCounter where a.Period.PeriodId={4} and s.Complex.ComplexId={0} and s.Home.IdHome={1} and s.BaseCounter.Id={2} and s.Company.CompanyId={6} " + str1 + str3 + " order by" + str2 + str4;
                  object[] objArray = new object[7]{ (object) Options.Complex.ComplexId, (object) this.home.IdHome, this.cmbBase.SelectedValue, this.cmbService.SelectedValue, (object) num2, null, null };
                  int index1 = 5;
                  periodName = this.monthClosed.PeriodName;
                  dateTime1 = periodName.Value;
                  string baseFormat = KvrplHelper.DateToBaseFormat(dateTime1.AddMonths(1));
                  objArray[index1] = (object) baseFormat;
                  int index2 = 6;
                  // ISSUE: variable of a boxed type
                  short companyId = this.company.CompanyId;
                  objArray[index2] = (object) companyId;
                  string queryString = string.Format(format, objArray);
                  auditList = session.CreateQuery(queryString).List<Audit>();
                }
                if ((int) this.level == 2)
                {
                  ISession session = this.session;
                  string format = "select a from Audit a join fetch a.Counter s left join fetch s.Service left join fetch s.TypeCounter left join fetch s.Home h left join fetch h.Str st where a.Period.PeriodId={4} and s.Complex.ComplexId={0} and s.Company.CompanyId={1} and s.BaseCounter.Id={2} " + str1 + str3 + " order by" + str2 + str4;
                  object[] objArray = new object[6]{ (object) Options.Complex.ComplexId, (object) this.company.CompanyId, this.cmbBase.SelectedValue, this.cmbService.SelectedValue, (object) num2, null };
                  int index = 5;
                  periodName = this.monthClosed.PeriodName;
                  dateTime1 = periodName.Value;
                  string baseFormat = KvrplHelper.DateToBaseFormat(dateTime1.AddMonths(1));
                  objArray[index] = (object) baseFormat;
                  string queryString = string.Format(format, objArray);
                  auditList = session.CreateQuery(queryString).List<Audit>();
                }
                Audit audit2 = auditList[this.dgvAudit.CurrentRow.Index];
                if (!this.pastTime && (!this.insertRecord && (audit2.DBeg <= dateTime2 && audit2.DEnd < dateTime2 || dataBoundItem.DEnd < dateTime2 || audit2.DBeg > dateTime2 && dataBoundItem.DBeg <= dateTime2 || audit2.DBeg <= dateTime2 && (audit2.DBeg != dataBoundItem.DBeg || audit2.Counter != dataBoundItem.Counter || (int) audit2.Scheme != (int) dataBoundItem.Scheme || audit2.Note != dataBoundItem.Note))))
                {
                  int num4 = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  return false;
                }
                this.session.CreateQuery("update Audit set DBeg=:dbeg,DEnd=:dend,Counter.CounterId=:counter,Scheme=:scheme,Note=:note,UName=:uname,DEdit=:dedit where Period.PeriodId=:period and Counter.CounterId=:oldcounter and DBeg=:olddbeg").SetParameter<DateTime>("dbeg", dataBoundItem.DBeg).SetParameter<DateTime>("dend", dataBoundItem.DEnd).SetParameter<int>("counter", dataBoundItem.Counter.CounterId).SetParameter<short>("scheme", dataBoundItem.Scheme).SetParameter<string>("uname", dataBoundItem.UName).SetParameter<DateTime>("dedit", dataBoundItem.DEdit).SetParameter<string>("note", dataBoundItem.Note).SetParameter<int>("period", audit2.Period.PeriodId).SetParameter<int>("oldcounter", audit2.Counter.CounterId).SetParameter<DateTime>("olddbeg", audit2.DBeg).ExecuteUpdate();
              }
            }
            catch (Exception ex)
            {
              int num2 = (int) MessageBox.Show("Невозможно сохранить изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              KvrplHelper.WriteLog(ex, (LsClient) null);
            }
          }
          else
          {
            int num = (int) MessageBox.Show("Дата окончания не введена", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
        }
        else
        {
          int num = (int) MessageBox.Show("Дата начала не введена", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
      return true;
    }

    private void DeleteAudit()
    {
      if (this.dgvAudit.Rows.Count <= 0 || this.dgvAudit.CurrentRow == null)
        return;
      Audit dataBoundItem = (Audit) this.dgvAudit.CurrentRow.DataBoundItem;
      if (!this.pastTime)
      {
        DateTime dateTime1 = this.monthClosed.PeriodName.Value;
        dateTime1 = dateTime1.AddMonths(1);
        DateTime dateTime2 = dateTime1.AddDays(-1.0);
        if (dataBoundItem.DBeg <= dateTime2 || dataBoundItem.DEnd <= dateTime2)
        {
          int num = (int) MessageBox.Show("Не могу удалить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.session.Clear();
          return;
        }
      }
      else if (Options.Period.PeriodName.Value <= this.monthClosed.PeriodName.Value)
      {
        int num = (int) MessageBox.Show("Не могу удалить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.session.Clear();
        return;
      }
      if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
      {
        this.session = Domain.CurrentSession;
        try
        {
          this.session.Delete((object) dataBoundItem);
          this.session.Flush();
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Невозможно удалить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        this.session.Clear();
      }
    }

    private void dgvAudit_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (this.dgvAudit.DataSource == null)
        return;
      DataGridViewRow row = (sender as DataGridView).Rows[e.RowIndex];
      DateTime dbeg = ((Audit) row.DataBoundItem).DBeg;
      DateTime? periodName = this.monthClosed.PeriodName;
      DateTime dateTime1 = periodName.Value;
      DateTime dateTime2 = KvrplHelper.LastDay(dateTime1.AddMonths(1));
      int num;
      if (dbeg <= dateTime2)
      {
        DateTime dend = ((Audit) row.DataBoundItem).DEnd;
        periodName = this.monthClosed.PeriodName;
        dateTime1 = periodName.Value;
        DateTime dateTime3 = dateTime1.AddMonths(1);
        num = dend >= dateTime3 ? 1 : 0;
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

    private void btnCloseScheme_Click(object sender, EventArgs e)
    {
      FrmCloseScheme frmCloseScheme = new FrmCloseScheme(this.company, this.home, (int) this.level, (Service) this.cmbService.SelectedItem);
      int num = (int) frmCloseScheme.ShowDialog();
      frmCloseScheme.Dispose();
      this.mpCurrentPeriod.Value = Options.Period.PeriodName.Value;
    }

    private void btnAudit_Click(object sender, EventArgs e)
    {
      try
      {
        FrmAudit frmAudit = new FrmAudit(this.company, this.home, this.level, ((BaseCounter) this.cmbBase.SelectedItem).Id, (Service) this.cmbService.SelectedItem);
        int num = (int) frmAudit.ShowDialog();
        frmAudit.Dispose();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Невозможно вызвать форму массового ввода поверок", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      this.LoadAudit();
    }

    private void LoadSeal()
    {
      this.Cursor = Cursors.WaitCursor;
      this.btnAdd.Enabled = true;
      this.btnDelete.Enabled = true;
      this.btnSave.Enabled = false;
      this.cbArchive.Visible = false;
      IList<Seal> sealList = (IList<Seal>) new List<Seal>();
      this.session.Clear();
      this.session = Domain.CurrentSession;
      this.counters = (IList<Counter>) new List<Counter>();
      string str1 = "";
      string str2 = "";
      string str3 = "";
      if ((uint) Convert.ToInt16(this.cmbService.SelectedValue) > 0U)
        str1 = " and s.Service.ServiceId={3}";
      if (this.tcntrlCounters.SelectedTab == this.tpCounters)
        str2 = " isnull(s.ArchivesDate,'2999-12-31') desc,";
      if ((int) this.level == 2)
        str2 += " st.NameStr,DBA.lengthhome(s.Home.NHome),s.Home.HomeKorp,";
      if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
      {
        str2 += " DBA.lengthhome(s.LsClient.Flat.NFlat),";
        str3 = string.Format(" and s.LsClient.Complex.IdFk in ({0},{1})", (object) Options.Complex.IdFk, (object) Options.ComplexArenda.IdFk);
        if (!Options.Kvartplata)
          str3 = string.Format(" and s.LsClient.Complex.IdFk={0}", (object) Options.ComplexArenda.IdFk);
        if (!Options.Arenda)
          str3 = string.Format(" and s.LsClient.Complex.IdFk={0}", (object) Options.Complex.IdFk);
      }
      string str4 = !(Options.SortService == " s.ServiceId") ? " s.Service.ServiceName" : " s.Service.ServiceId";
      int num = 0;
      if (this.pastTime)
        num = Options.Period.PeriodId;
      if ((int) this.level == 3)
        sealList = this.session.CreateQuery(string.Format("select sl from Seal sl join fetch sl.Counter s left join fetch s.Service left join fetch s.TypeCounter where s.Complex.ComplexId={0} and s.Home.IdHome={1} and s.BaseCounter.Id={2} and s.Company.CompanyId={5} " + str3 + str1 + " order by" + str2 + str4, (object) Options.Complex.ComplexId, (object) this.home.IdHome, this.cmbBase.SelectedValue, this.cmbService.SelectedValue, (object) num, (object) this.company.CompanyId)).List<Seal>();
      if ((int) this.level == 2)
        sealList = this.session.CreateQuery(string.Format("select sl from Seal sl join fetch sl.Counter s left join fetch s.Service left join fetch s.TypeCounter left join fetch s.Home h left join fetch h.Str st where s.Complex.ComplexId={0} and s.Company.CompanyId={1} and s.BaseCounter.Id={2} " + str3 + str1 + " order by" + str2 + str4, (object) Options.Complex.ComplexId, (object) this.company.CompanyId, this.cmbBase.SelectedValue, this.cmbService.SelectedValue, (object) num)).List<Seal>();
      this.dgvSeal.Columns.Clear();
      this.dgvSeal.DataSource = (object) null;
      this.dgvSeal.DataSource = (object) sealList;
      this.session.Clear();
      this.MySettingsSeal.GridName = "Seal";
      this.SetViewSeal();
      this.Cursor = Cursors.Default;
    }

    private void LoadSettingsSeal()
    {
      this.MySettingsSeal.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvSeal.Columns)
        this.MySettingsSeal.GetMySettings(column);
    }

    private void dgvSeal_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsSeal.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsSeal.Columns[this.MySettingsSeal.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsSeal.Save();
    }

    private void SetViewSeal()
    {
      KvrplHelper.AddComboBoxColumn(this.dgvSeal, 0, (IList) null, (string) null, (string) null, "Номер счетчика", "Num", 100, 100);
      IList<TypeSeal> typeSealList = (IList<TypeSeal>) new List<TypeSeal>();
      KvrplHelper.AddComboBoxColumn(this.dgvSeal, 1, (IList) this.session.CreateCriteria(typeof (TypeSeal)).List<TypeSeal>(), "TypeSealId", "TypeSealName", "Тип пломбы", "TypeSeal", 320, 220);
      KvrplHelper.AddMaskDateColumn(this.dgvSeal, 2, "Дата", "Date");
      KvrplHelper.AddMaskDateColumn(this.dgvSeal, 3, "Дата снятия", "RemoveDate");
      this.dgvSeal.Columns["Inspector"].HeaderText = "Пломбировщик";
      this.dgvSeal.Columns["Note"].HeaderText = "Примечания";
      this.dgvSeal.Columns["Number"].HeaderText = "Номер пломбы";
      this.dgvSeal.Columns["Inspector"].DisplayIndex = 1;
      this.dgvSeal.Columns["Note"].DisplayIndex = 5;
      this.dgvSeal.Columns["Number"].DisplayIndex = 3;
      KvrplHelper.ViewEdit(this.dgvSeal);
      DataGridViewComboBoxColumn viewComboBoxColumn = new DataGridViewComboBoxColumn();
      string str = string.Format(" and ls.Complex.IdFk in ({0},{1})", (object) Options.Complex.IdFk, (object) Options.ComplexArenda.IdFk);
      if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
      {
        if (!Options.Kvartplata)
          str = string.Format(" and ls.Complex.IdFk={0}", (object) Options.ComplexArenda.IdFk);
        if (!Options.Arenda)
          str = string.Format(" and ls.Complex.IdFk={0}", (object) Options.Complex.IdFk);
        this.clients = (IList<LsClient>) new List<LsClient>();
        if ((int) this.level == 2)
          this.clients = this.session.CreateQuery(string.Format("select distinct ls from LsClient ls,Counter c,Flat f where c.LsClient=ls and ls.Company.CompanyId = {0} and (c.ArchivesDate is null or c.ArchivesDate > '{1}') and ls.Flat=f and c.BaseCounter.Id={2} " + this.uslovie + str, (object) this.company.CompanyId, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), this.cmbBase.SelectedValue, (object) Convert.ToInt16(this.cmbService.SelectedValue))).List<LsClient>();
        if ((int) this.level == 3)
          this.clients = this.session.CreateQuery(string.Format("select distinct ls from LsClient ls,Counter c,Flat f where c.LsClient=ls and ls.Home.IdHome = {0} and (c.ArchivesDate is null or c.ArchivesDate > '{1}') and ls.Flat=f and c.BaseCounter.Id={2} " + this.uslovie + str, (object) this.home.IdHome, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), this.cmbBase.SelectedValue, (object) Convert.ToInt16(this.cmbService.SelectedValue))).List<LsClient>();
        viewComboBoxColumn.DataSource = (object) this.clients;
        viewComboBoxColumn.ValueMember = "ClientId";
        viewComboBoxColumn.DisplayMember = "SmallAddress";
        viewComboBoxColumn.HeaderText = "Адрес";
        viewComboBoxColumn.Name = "LicAddress";
        this.dgvSeal.Columns.Insert(0, (DataGridViewColumn) viewComboBoxColumn);
      }
      if ((Convert.ToInt32(this.cmbBase.SelectedValue) != 2 || (int) this.level == 2) && Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
      {
        this.homes = (IList<Home>) new List<Home>();
        if ((int) this.level == 2)
          this.homes = this.session.CreateQuery(string.Format("select distinct h from Home h left join fetch h.Str, HomeLink hl,Counter c where c.Home=h and hl.Home=h and hl.Company.CompanyId={0} and c.BaseCounter.Id={2} " + this.uslovie + " order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)", (object) this.company.CompanyId, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), this.cmbBase.SelectedValue, (object) Convert.ToInt16(this.cmbService.SelectedValue))).List<Home>();
        if ((int) this.level == 3)
          this.homes = this.session.CreateQuery(string.Format("select distinct h from Home h left join fetch h.Str, HomeLink hl,Counter c where c.Home=h and c.Home.IdHome={4} and c.BaseCounter.Id={2} and hl.Home=h and hl.Company.CompanyId={0} " + this.uslovie + " order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)", (object) this.company.CompanyId, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), this.cmbBase.SelectedValue, (object) Convert.ToInt16(this.cmbService.SelectedValue), (object) this.home.IdHome)).List<Home>();
        KvrplHelper.AddComboBoxColumn(this.dgvSeal, 0, (IList) this.homes, "IdHome", "Address", "Адрес", "Address", 180, 180);
      }
      IList<LsClient> lsClientList1 = (IList<LsClient>) new List<LsClient>();
      IList<Counter> counterList1 = (IList<Counter>) new List<Counter>();
      foreach (DataGridViewRow row in (IEnumerable) this.dgvSeal.Rows)
      {
        row.Cells["Date"].Value = (object) ((Seal) row.DataBoundItem).Date;
        row.Cells["RemoveDate"].Value = (object) ((Seal) row.DataBoundItem).RemoveDate;
        if (((Seal) row.DataBoundItem).TypeSeal != null)
          row.Cells["TypeSeal"].Value = (object) ((Seal) row.DataBoundItem).TypeSeal.TypeSealId;
        IList<Counter> counterList2;
        if (((int) this.level == 2 || (int) this.level == 3 && Convert.ToInt32(this.cmbBase.SelectedValue) != 2) && ((Seal) row.DataBoundItem).Home != null)
        {
          if (Convert.ToInt32(this.cmbBase.SelectedValue) != 2)
          {
            row.Cells["Address"].Value = (object) ((Seal) row.DataBoundItem).Home.IdHome;
            if (counterList1.Count == 0 || counterList1[0].Home.IdHome != ((Seal) row.DataBoundItem).Home.IdHome)
            {
              counterList2 = (IList<Counter>) new List<Counter>();
              counterList1 = (uint) Convert.ToInt16(this.cmbService.SelectedValue) <= 0U ? this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Home.IdHome", (object) ((Seal) row.DataBoundItem).Home.IdHome)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>() : this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Home.IdHome", (object) ((Seal) row.DataBoundItem).Home.IdHome)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) (Service) this.cmbService.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>();
            }
            row.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
            {
              DisplayStyleForCurrentCellOnly = true,
              ValueMember = "CounterId",
              DisplayMember = "AllInfo",
              DataSource = (object) counterList1
            };
            row.Cells["Num"].Value = (object) ((Seal) row.DataBoundItem).Counter.CounterId;
          }
          else
          {
            row.Cells["Address"].Value = (object) ((Seal) row.DataBoundItem).Home.IdHome;
            if (lsClientList1.Count == 0 || lsClientList1[0].Home.IdHome != ((Seal) row.DataBoundItem).Home.IdHome)
            {
              IList<LsClient> lsClientList2 = (IList<LsClient>) new List<LsClient>();
              lsClientList1 = this.session.CreateQuery(string.Format("select distinct ls from LsClient ls,Counter c,Flat f where c.LsClient=ls and ls.Home.IdHome = {0} and (c.ArchivesDate is null or c.ArchivesDate > '{1}') and ls.Flat=f and c.BaseCounter.Id={2} and c.Complex.ComplexId={4} " + str + this.uslovie, (object) ((Seal) row.DataBoundItem).Home.IdHome, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), this.cmbBase.SelectedValue, (object) Convert.ToInt16(this.cmbService.SelectedValue), (object) Options.Complex.ComplexId)).List<LsClient>();
            }
            row.Cells["LicAddress"] = (DataGridViewCell) new DataGridViewComboBoxCell()
            {
              DisplayStyleForCurrentCellOnly = true,
              ValueMember = "ClientId",
              DisplayMember = "SmallAddress",
              DataSource = (object) lsClientList1
            };
            row.Cells["LicAddress"].Value = (object) ((Seal) row.DataBoundItem).ClientId;
          }
        }
        int? clientId1;
        int num1;
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
        {
          clientId1 = ((Seal) row.DataBoundItem).ClientId;
          num1 = clientId1.HasValue ? 1 : 0;
        }
        else
          num1 = 0;
        if (num1 != 0)
        {
          row.Cells["LicAddress"].Value = (object) ((Seal) row.DataBoundItem).ClientId;
          int num2;
          if (counterList1.Count != 0)
          {
            int clientId2 = counterList1[0].LsClient.ClientId;
            clientId1 = ((Seal) row.DataBoundItem).ClientId;
            int valueOrDefault = clientId1.GetValueOrDefault();
            num2 = clientId2 == valueOrDefault ? (!clientId1.HasValue ? 1 : 0) : 1;
          }
          else
            num2 = 1;
          if (num2 != 0)
          {
            counterList2 = (IList<Counter>) new List<Counter>();
            counterList1 = (uint) Convert.ToInt16(this.cmbService.SelectedValue) <= 0U ? this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("LsClient.ClientId", (object) ((Seal) row.DataBoundItem).ClientId)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>() : this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("LsClient.ClientId", (object) ((Seal) row.DataBoundItem).ClientId)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) (Service) this.cmbService.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>();
          }
          row.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
          {
            DisplayStyleForCurrentCellOnly = true,
            ValueMember = "CounterId",
            DisplayMember = "AllInfo",
            DataSource = (object) counterList1
          };
          row.Cells["Num"].Value = (object) ((Seal) row.DataBoundItem).Counter.CounterId;
        }
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 4)
        {
          if (counterList1.Count == 0)
          {
            counterList2 = (IList<Counter>) new List<Counter>();
            counterList1 = (uint) Convert.ToInt16(this.cmbService.SelectedValue) <= 0U ? this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>() : this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) (Service) this.cmbService.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>();
          }
          row.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
          {
            DisplayStyleForCurrentCellOnly = true,
            ValueMember = "CounterId",
            DisplayMember = "AllInfo",
            DataSource = (object) counterList1
          };
          if (((Seal) row.DataBoundItem).Counter != null)
            row.Cells["Num"].Value = (object) ((Seal) row.DataBoundItem).Counter.CounterId;
        }
      }
      this.LoadSettingsSeal();
    }

    private void dgvSeal_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (!this.dgvSeal.IsCurrentCellDirty)
        return;
      this.dgvSeal.CommitEdit(DataGridViewDataErrorContexts.Commit);
      if ((Convert.ToInt32(this.cmbBase.SelectedValue) == 1 || Convert.ToInt32(this.cmbBase.SelectedValue) == 3) && (int) Convert.ToInt16(this.cmbService.SelectedValue) != 0 && this.dgvSeal.CurrentCell.ColumnIndex == this.dgvSeal.Rows[this.dgvSeal.CurrentRow.Index].Cells["Address"].ColumnIndex)
      {
        this.session = Domain.CurrentSession;
        IList<Counter> counterList = (IList<Counter>) new List<Counter>();
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 1 || Convert.ToInt32(this.cmbBase.SelectedValue) == 3)
          counterList = this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Home.IdHome", this.dgvSeal.CurrentRow.Cells["Address"].Value)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) (Service) this.cmbService.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).List<Counter>();
        this.dgvSeal.CurrentRow.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "CounterId",
          DisplayMember = "AllInfo",
          DataSource = (object) counterList
        };
      }
      if ((int) Convert.ToInt16(this.cmbBase.SelectedValue) == 2 && (int) Convert.ToInt16(this.cmbService.SelectedValue) != 0 && this.dgvSeal.CurrentCell.ColumnIndex == this.dgvSeal.Rows[this.dgvSeal.CurrentRow.Index].Cells["LicAddress"].ColumnIndex)
      {
        this.session = Domain.CurrentSession;
        IList<Counter> counterList1 = (IList<Counter>) new List<Counter>();
        IList<Counter> counterList2 = this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("LsClient.ClientId", this.dgvSeal.CurrentRow.Cells["LicAddress"].Value)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) (Service) this.cmbService.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).List<Counter>();
        this.dgvSeal.CurrentRow.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "CounterId",
          DisplayMember = "AllInfo",
          DataSource = (object) counterList2
        };
      }
      if ((int) Convert.ToInt16(this.cmbService.SelectedValue) == 0 && Convert.ToInt32(this.cmbBase.SelectedValue) != 4 && ((int) this.level == 2 && (Convert.ToInt32(this.cmbBase.SelectedValue) != 2 && this.dgvSeal.CurrentCell.ColumnIndex == this.dgvSeal.Rows[this.dgvSeal.CurrentRow.Index].Cells["Address"].ColumnIndex || Convert.ToInt32(this.cmbBase.SelectedValue) == 2 && this.dgvSeal.CurrentCell.ColumnIndex == this.dgvSeal.Rows[this.dgvSeal.CurrentRow.Index].Cells["LicAddress"].ColumnIndex) || (int) this.level == 3 && (Convert.ToInt32(this.cmbBase.SelectedValue) == 2 && this.dgvSeal.CurrentCell.ColumnIndex == this.dgvSeal.Rows[this.dgvSeal.CurrentRow.Index].Cells["LicAddress"].ColumnIndex || Convert.ToInt32(this.cmbBase.SelectedValue) != 2 && this.dgvSeal.CurrentCell.ColumnIndex == this.dgvSeal.Rows[this.dgvSeal.CurrentRow.Index].Cells["Address"].ColumnIndex)))
      {
        this.session = Domain.CurrentSession;
        IList<Counter> counterList = (IList<Counter>) new List<Counter>();
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
          counterList = this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("LsClient.ClientId", this.dgvSeal.CurrentRow.Cells["LicAddress"].Value)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).List<Counter>();
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 1 || Convert.ToInt32(this.cmbBase.SelectedValue) == 3)
          counterList = this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Home.IdHome", this.dgvSeal.CurrentRow.Cells["Address"].Value)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).List<Counter>();
        this.dgvSeal.CurrentRow.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "CounterId",
          DisplayMember = "AllInfo",
          DataSource = (object) counterList
        };
        if ((int) this.level == 2 && Convert.ToInt32(this.cmbBase.SelectedValue) != 2)
        {
          IList list = this.session.CreateSQLQuery(string.Format("select districtname from districthome dh,district d where d.iddistrict=dh.iddistrict and idhome={0} and idgr=2", this.dgvSeal.CurrentRow.Cells["Address"].Value)).List();
          this.dgvSeal.CurrentRow.Cells["Inspector"].Value = list.Count <= 0 ? (object) "" : (object) Convert.ToString(list[0]);
        }
      }
      if ((int) this.level == 2 && Convert.ToInt32(this.cmbBase.SelectedValue) == 2 && this.dgvSeal.CurrentCell.ColumnIndex == this.dgvSeal.Rows[this.dgvSeal.CurrentRow.Index].Cells["Address"].ColumnIndex)
      {
        this.session = Domain.CurrentSession;
        IList<LsClient> lsClientList1 = (IList<LsClient>) new List<LsClient>();
        string str1;
        if ((uint) Convert.ToInt16(this.cmbService.SelectedValue) > 0U)
        {
          this.dgvSeal.Columns["ServiceName"].Visible = false;
          str1 = "and c.Service.ServiceId={3}";
        }
        else
          str1 = "";
        string str2 = string.Format(" and ls.Complex.IdFk in ({0},{1})", (object) Options.Complex.IdFk, (object) Options.ComplexArenda.IdFk);
        if (!Options.Kvartplata)
          str2 = string.Format(" and ls.Complex.IdFk={0}", (object) Options.ComplexArenda.IdFk);
        if (!Options.Arenda)
          str2 = string.Format(" and ls.Complex.IdFk={0}", (object) Options.Complex.IdFk);
        IList<LsClient> lsClientList2 = this.session.CreateQuery(string.Format("select distinct ls from LsClient ls,Counter c,Flat f where c.LsClient=ls and ls.Home.IdHome = {0} and (c.ArchivesDate is null or c.ArchivesDate > '{1}') and ls.Flat=f and c.BaseCounter.Id={2} " + str1 + str2, this.dgvSeal.CurrentRow.Cells["Address"].Value, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), this.cmbBase.SelectedValue, (object) Convert.ToInt16(this.cmbService.SelectedValue))).List<LsClient>();
        this.dgvSeal.CurrentRow.Cells["LicAddress"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "ClientId",
          DisplayMember = "SmallAddress",
          DataSource = (object) lsClientList2
        };
        IList list = this.session.CreateSQLQuery(string.Format("select districtname from districthome dh,district d where d.iddistrict=dh.iddistrict and idhome={0} and idgr=2", this.dgvSeal.CurrentRow.Cells["Address"].Value)).List();
        this.dgvSeal.CurrentRow.Cells["Inspector"].Value = list.Count <= 0 ? (object) "" : (object) Convert.ToString(list[0]);
      }
    }

    private void InsertSeal()
    {
      this.btnSave.Enabled = true;
      this.btnAdd.Enabled = false;
      this.btnDelete.Enabled = false;
      this.insertRecord = true;
      Seal seal = new Seal();
      seal.Date = DateTime.Now;
      this.session = Domain.CurrentSession;
      if ((int) this.level == 3)
      {
        IList list = this.session.CreateSQLQuery(string.Format("select districtname from districthome dh,district d where d.iddistrict=dh.iddistrict and idhome={0} and idgr=2", (object) this.home.IdHome)).List();
        if (list.Count > 0)
          seal.Inspector = Convert.ToString(list[0]);
      }
      IList<Seal> sealList = (IList<Seal>) new List<Seal>();
      if ((uint) this.dgvSeal.Rows.Count > 0U)
        sealList = (IList<Seal>) (this.dgvSeal.DataSource as List<Seal>);
      sealList.Add(seal);
      this.dgvSeal.Columns.Clear();
      this.dgvSeal.DataSource = (object) null;
      this.dgvSeal.DataSource = (object) sealList;
      this.SetViewSeal();
      this.dgvSeal.CurrentCell = this.dgvSeal.Rows[this.dgvSeal.Rows.Count - 1].Cells[0];
    }

    private bool SaveSeal()
    {
      if (this.dgvSeal.Rows.Count > 0 && this.dgvSeal.CurrentRow != null)
      {
        this.session = Domain.CurrentSession;
        Seal dataBoundItem = (Seal) this.dgvSeal.CurrentRow.DataBoundItem;
        DateTime? nullable;
        int num1;
        if (!this.insertRecord)
        {
          nullable = dataBoundItem.RemoveDate;
          if (nullable.HasValue)
          {
            nullable = dataBoundItem.RemoveDate;
            DateTime dateTime = this.monthClosed.PeriodName.Value.AddMonths(1);
            num1 = nullable.HasValue ? (nullable.GetValueOrDefault() < dateTime ? 1 : 0) : 0;
            goto label_5;
          }
        }
        num1 = 0;
label_5:
        if (num1 != 0)
        {
          int num2 = (int) MessageBox.Show("Пломба снята. Запись не подлежит редактированию", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        try
        {
          dataBoundItem.Date = Convert.ToDateTime(this.dgvSeal.CurrentRow.Cells["Date"].Value);
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show("Некорректное значение даты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (this.dgvSeal.CurrentRow.Cells["RemoveDate"].Value != null)
        {
          try
          {
            dataBoundItem.RemoveDate = new DateTime?(Convert.ToDateTime(this.dgvSeal.CurrentRow.Cells["RemoveDate"].Value));
          }
          catch (Exception ex)
          {
          }
        }
        DateTime date1 = dataBoundItem.Date;
        nullable = this.monthClosed.PeriodName;
        DateTime dateTime1 = nullable.Value;
        DateTime dateTime2 = dateTime1.AddMonths(1);
        if (date1 < dateTime2)
        {
          int num2 = (int) MessageBox.Show("Запись принадлежит закрытому периоду", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        dateTime1 = dataBoundItem.Date;
        nullable = dataBoundItem.RemoveDate;
        if (nullable.HasValue && dateTime1 > nullable.GetValueOrDefault())
        {
          int num2 = (int) MessageBox.Show("Дата установки больше даты снятия", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        DateTime date2 = dataBoundItem.Date;
        dateTime1 = DateTime.Now;
        DateTime dateTime3 = dateTime1.AddYears(-3);
        int num3;
        if (!(date2 <= dateTime3))
        {
          DateTime date3 = dataBoundItem.Date;
          dateTime1 = DateTime.Now;
          DateTime dateTime4 = dateTime1.AddYears(3);
          num3 = date3 >= dateTime4 ? 1 : 0;
        }
        else
          num3 = 1;
        if (num3 != 0 && MessageBox.Show("Дата установки отличается от текущей более, чем на 3 года. Продолжить", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
          return false;
        if (this.dgvSeal.CurrentRow.Cells["Num"].Value == null)
        {
          int num2 = (int) MessageBox.Show("Не выбран счетчик", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        Counter counter = this.session.Get<Counter>(this.dgvSeal.CurrentRow.Cells["Num"].Value);
        dataBoundItem.Counter = counter;
        if (this.dgvSeal.CurrentRow.Cells["TypeSeal"].Value == null)
        {
          int num2 = (int) MessageBox.Show("Не выбран тип пломбы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        TypeSeal typeSeal = this.session.Get<TypeSeal>(this.dgvSeal.CurrentRow.Cells["TypeSeal"].Value);
        dataBoundItem.TypeSeal = typeSeal;
        if (this.dgvSeal.CurrentRow.Cells["Inspector"].Value == null)
          dataBoundItem.Inspector = "";
        if (this.dgvSeal.CurrentRow.Cells["Number"].Value == null)
        {
          int num2 = (int) MessageBox.Show("Не введен номер пломбы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (this.dgvSeal.CurrentRow.Cells["Note"].Value == null)
          dataBoundItem.Note = "";
        dataBoundItem.UName = Options.Login;
        Seal seal = dataBoundItem;
        dateTime1 = DateTime.Now;
        DateTime date4 = dateTime1.Date;
        seal.DEdit = date4;
        try
        {
          if (this.insertRecord)
          {
            short int16 = Convert.ToInt16(this.session.CreateQuery(string.Format("select max(SealId) from Seal where Counter.CounterId={0}", (object) dataBoundItem.Counter.CounterId)).UniqueResult());
            dataBoundItem.SealId = Convert.ToInt16((int) int16 + 1);
            this.insertRecord = false;
            this.session.Save((object) dataBoundItem);
          }
          else
            this.session.Update((object) dataBoundItem);
          this.session.Flush();
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show("Невозможно сохранить изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        this.session.Clear();
      }
      return true;
    }

    private void DeleteSeal()
    {
      if (this.dgvSeal.Rows.Count <= 0 || this.dgvSeal.CurrentRow == null)
        return;
      Seal dataBoundItem = (Seal) this.dgvSeal.CurrentRow.DataBoundItem;
      if (dataBoundItem.Date < this.monthClosed.PeriodName.Value.AddMonths(1))
      {
        int num1 = (int) MessageBox.Show("Невозможно удалить запись в закрытом периоде!", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
      }
      else if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
      {
        this.session = Domain.CurrentSession;
        try
        {
          this.session.Delete((object) dataBoundItem);
          this.session.Flush();
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show("Невозможно удалить запись", "Ошибка", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        this.session.Clear();
      }
    }

    private void PrepareDetail()
    {
      this.session = Domain.CurrentSession;
      IList<Home> homeList = (IList<Home>) new List<Home>();
      if ((int) this.level == 2)
        homeList = this.session.CreateQuery(string.Format("select h from Home h left join fetch h.Str, HomeLink hl where hl.Home=h and hl.Company.CompanyId={0} order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)", (object) this.company.CompanyId)).List<Home>();
      if ((int) this.level == 3)
        homeList = this.session.CreateQuery(string.Format("select h from Home h left join fetch h.Str, HomeLink hl where hl.Home=h and hl.Company.CompanyId={0} and hl.Home.IdHome={1} order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)", (object) this.company.CompanyId, (object) this.home.IdHome)).List<Home>();
      homeList.Insert(0, new Home() { IdHome = 0 });
      this.cmbCounter.DataSource = (object) homeList;
      this.cmbCounter.DisplayMember = "Address";
      this.cmbCounter.ValueMember = "IdHome";
      if ((int) this.level == 3)
      {
        this.cmbCounter.SelectedValue = (object) this.home.IdHome;
        this.cmbCounter.Enabled = false;
      }
      Period period = new Period();
      period.PeriodId = 0;
      period.PeriodName = new DateTime?();
      this.months.Insert(0, period);
      this.periods.Insert(0, period);
      this.cmbMonth.DataSource = (object) this.months;
      this.cmbMonth.ValueMember = "PeriodId";
      this.cmbMonth.DisplayMember = "PeriodName";
      this.cmbMonth.SelectedValue = (object) 0;
      this.cmbPeriod.DataSource = (object) this.periods;
      this.cmbPeriod.ValueMember = "PeriodId";
      this.cmbPeriod.DisplayMember = "PeriodName";
      this.cmbPeriod.SelectedValue = (object) 0;
      this.session.Clear();
    }

    private void LoadDetail()
    {
      this.pnTools.Height = 73;
      if ((Convert.ToInt32(this.cmbBase.SelectedValue) == 2 || (int) this.level == 2) && Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
      {
        this.cbArchive.Enabled = false;
        this.cbArchive.Checked = false;
      }
      else
        this.cbArchive.Enabled = true;
      this.dgvDetail.Columns.Clear();
      this.dgvDetail.DataSource = (object) null;
      this.LoadCountersList(false);
      this.session = Domain.CurrentSession;
      IList<CounterDetail> counterDetailList = (IList<CounterDetail>) new List<CounterDetail>();
      string str1 = "";
      string str2 = (uint) Convert.ToInt32(this.cmbService.SelectedValue) <= 0U ? "" : " and cd.Service.ServiceId={0}";
      string str3 = (uint) Convert.ToInt32(this.cmbCounter.SelectedValue) <= 0U ? "" : " and cd.Counter.Home.IdHome = {1}";
      string str4 = (Period) this.cmbMonth.SelectedItem == null || (uint) Convert.ToInt32(((Period) this.cmbMonth.SelectedItem).PeriodId) <= 0U ? "" : " and cd.Month.PeriodId = {2}";
      string str5 = (Period) this.cmbPeriod.SelectedItem == null || (uint) Convert.ToInt32(((Period) this.cmbPeriod.SelectedItem).PeriodId) <= 0U ? "" : " and cd.Period.PeriodId = {3}";
      int periodId;
      if (!this.cbArchive.Checked)
      {
        str1 = " and cd.Period.PeriodId={3}";
        periodId = Options.Period.PeriodId;
      }
      else
        periodId = ((Period) this.cmbPeriod.SelectedItem).PeriodId;
      if ((int) this.level == 2)
        counterDetailList = this.session.CreateQuery(string.Format("select cd from CounterDetail cd left join fetch cd.Counter c left join fetch c.Home h left join fetch h.Str where c.Company.CompanyId={4} and c.BaseCounter.Id={5} " + str2 + str1 + str3 + str4 + str5 + " order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome),cd.Period.PeriodId desc,cd.Month.PeriodId desc,cd.Service.ServiceId asc,cd.Counter.CounterId asc", (object) Convert.ToInt32(this.cmbService.SelectedValue), (object) Convert.ToInt32(this.cmbCounter.SelectedValue), (object) Convert.ToInt32(((Period) this.cmbMonth.SelectedItem).PeriodId), (object) periodId, (object) this.company.CompanyId, (object) Convert.ToInt32(this.cmbBase.SelectedValue))).List<CounterDetail>();
      if ((int) this.level == 3)
        counterDetailList = this.session.CreateQuery(string.Format("select cd from CounterDetail cd left join fetch cd.Counter c where  c.Company.CompanyId={5} and c.Home.IdHome={4} " + str2 + str1 + str3 + str4 + str5 + " order by cd.Period.PeriodId desc,cd.Month.PeriodId desc,cd.Service.ServiceId asc,cd.Counter.CounterId asc", (object) Convert.ToInt32(this.cmbService.SelectedValue), (object) Convert.ToInt32(this.cmbCounter.SelectedValue), (object) Convert.ToInt32(((Period) this.cmbMonth.SelectedItem).PeriodId), (object) periodId, (object) this.home.IdHome, (object) this.company.CompanyId)).List<CounterDetail>();
      this.dgvDetail.DataSource = (object) counterDetailList;
      this.session.Clear();
      this.SetViewDetail();
      this.dgvDetail.ReadOnly = true;
      this.dgvDetail.Focus();
      this.insertRecord = false;
    }

    private void SetViewDetail()
    {
      this.session = Domain.CurrentSession;
      this.dgvDetail.Columns["Evidence"].HeaderText = "Расход";
      this.dgvDetail.Columns["EvidenceCross"].HeaderText = "Расход по связанной услуге";
      if (Options.Arenda && Options.Kvartplata)
      {
        this.dgvDetail.Columns["EvidenceNorm"].HeaderText = "Объем по нормативу (квартплата)";
        this.dgvDetail.Columns["EvidenceCntr"].HeaderText = "Объем по счетчикам (квартплата)";
        this.dgvDetail.Columns["EvidenceNormAr"].HeaderText = "Объем по нормативу (аренда)";
        this.dgvDetail.Columns["EvidenceCntrAr"].HeaderText = "Объем по счетчикам (аренда)";
        this.dgvDetail.Columns["EvidenceOdnNorm"].HeaderText = "Расход по ОДН по нормативу без ИПУ";
        this.dgvDetail.Columns["EvidenceOdnCntr"].HeaderText = "Расход по ОДН по нормативу с ИПУ";
        this.dgvDetail.Columns["EvidenceOdnNorm110"].HeaderText = "Расход по ОДН по нормативу без ИПУ(аренда)";
        this.dgvDetail.Columns["EvidenceOdnCntr110"].HeaderText = "Расход по ОДН по нормативу с ИПУ(аренда)";
      }
      else
      {
        if (Options.Kvartplata)
        {
          this.dgvDetail.Columns["EvidenceNorm"].HeaderText = "Объем по нормативу";
          this.dgvDetail.Columns["EvidenceCntr"].HeaderText = "Объем по счетчикам";
          this.dgvDetail.Columns["EvidenceOdnNorm"].HeaderText = "Расход по ОДН по нормативу без ИПУ";
          this.dgvDetail.Columns["EvidenceOdnCntr"].HeaderText = "Расход по ОДН по нормативу с ИПУ";
          this.dgvDetail.Columns["EvidenceNormAr"].Visible = false;
          this.dgvDetail.Columns["EvidenceCntrAr"].Visible = false;
          this.dgvDetail.Columns["EvidenceOdnNorm110"].Visible = false;
          this.dgvDetail.Columns["EvidenceOdnCntr110"].Visible = false;
        }
        if (Options.Arenda)
        {
          this.dgvDetail.Columns["EvidenceNormAr"].HeaderText = "Объем по нормативу";
          this.dgvDetail.Columns["EvidenceCntrAr"].HeaderText = "Объем по счетчикам";
          this.dgvDetail.Columns["EvidenceOdnNorm110"].HeaderText = "Расход по ОДН по нормативу без ИПУ(аренда)";
          this.dgvDetail.Columns["EvidenceOdnCntr110"].HeaderText = "Расход по ОДН по нормативу с ИПУ(аренда)";
          this.dgvDetail.Columns["EvidenceNorm"].Visible = false;
          this.dgvDetail.Columns["EvidenceCntr"].Visible = false;
          this.dgvDetail.Columns["EvidenceOdnNorm"].Visible = false;
          this.dgvDetail.Columns["EvidenceOdnCntr"].Visible = false;
        }
      }
      this.dgvDetail.Columns["Coeff"].HeaderText = "Коэффициент к инд. счетчикам";
      this.dgvDetail.Columns["NormCount"].HeaderText = "Человеко/ площаде - дни";
      this.dgvDetail.Columns["NormUnit"].HeaderText = "Объем к счетчику на единицу";
      this.homes = (IList<Home>) new List<Home>();
      if ((int) this.level == 2)
      {
        this.homes = this.session.CreateQuery(string.Format("select h from Home h left join fetch h.Str, HomeLink hl where hl.Home=h and hl.Company.CompanyId={0} order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)", (object) this.company.CompanyId)).List<Home>();
        this.counters = this.session.CreateQuery(string.Format("select c from Counter c where c.Company.CompanyId={0} and c.BaseCounter.Id={2} and c.Complex.ComplexId={1}", (object) this.company.CompanyId, (object) Options.Complex.ComplexId, (object) ((BaseCounter) this.cmbBase.SelectedItem).Id)).List<Counter>();
      }
      if ((int) this.level == 3)
      {
        this.homes = this.session.CreateQuery(string.Format("select h from Home h left join fetch h.Str, HomeLink hl where hl.Home=h and hl.Company.CompanyId={0} and hl.Home.IdHome={1} order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)", (object) this.company.CompanyId, (object) this.home.IdHome)).List<Home>();
        this.counters = this.session.CreateQuery(string.Format("select c from Counter c where c.Home.IdHome={0} and c.BaseCounter.Id=1 and c.Complex.ComplexId={1}", (object) this.home.IdHome, (object) Options.Complex.ComplexId)).List<Counter>();
      }
      KvrplHelper.AddComboBoxColumn(this.dgvDetail, 0, (IList) this.periods, "PeriodId", "PeriodName", "Период расчета", "Period", 100, 100);
      KvrplHelper.AddComboBoxColumn(this.dgvDetail, 1, (IList) this.serviceList, "ServiceId", "ServiceName", "Услуга", "Service", 160, 160);
      KvrplHelper.AddComboBoxColumn(this.dgvDetail, 2, (IList) this.counters, "CounterId", "CounterNum", "Номер счетчика", "Num", 100, 100);
      KvrplHelper.AddComboBoxColumn(this.dgvDetail, 3, (IList) this.periods, "PeriodId", "PeriodName", "За месяц", "Month", 100, 100);
      if (Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
        KvrplHelper.AddComboBoxColumn(this.dgvDetail, 0, (IList) this.homes, "IdHome", "Address", "Адрес", "Address", 200, 200);
      foreach (DataGridViewRow row in (IEnumerable) this.dgvDetail.Rows)
      {
        if (Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
          row.Cells["Address"].Value = (object) ((CounterDetail) row.DataBoundItem).Home.IdHome;
        row.Cells["Period"].Value = (object) ((CounterDetail) row.DataBoundItem).Period.PeriodId;
        row.Cells["Service"].Value = (object) ((CounterDetail) row.DataBoundItem).Service.ServiceId;
        row.Cells["Month"].Value = (object) ((CounterDetail) row.DataBoundItem).Month.PeriodId;
        if (((CounterDetail) row.DataBoundItem).Counter != null)
          row.Cells["Num"].Value = (object) ((CounterDetail) row.DataBoundItem).Counter.CounterId;
      }
      this.session.Clear();
    }

    private void cmbMonth_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (this.tcntrlCounters.SelectedTab == this.tpDetail)
        this.LoadDetail();
      if (this.tcntrlCounters.SelectedTab == this.tpDstrDetail)
        this.LoadDstrDetail();
      if (this.tcntrlCounters.SelectedTab != this.tpDetailEvidence)
        return;
      this.LoadDetailEvidence();
    }

    private void cmbPeriod_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (this.tcntrlCounters.SelectedTab == this.tpPeriod)
        this.LoadEvidence();
      if (this.tcntrlCounters.SelectedTab == this.tpDetail)
        this.LoadDetail();
      if (this.tcntrlCounters.SelectedTab == this.tpDstrDetail)
        this.LoadDstrDetail();
      if (this.tcntrlCounters.SelectedTab != this.tpDetailEvidence)
        return;
      this.LoadDetailEvidence();
    }

    private void cmbCounter_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (this.tcntrlCounters.SelectedTab == this.tpDetail)
        this.LoadDetail();
      if (this.tcntrlCounters.SelectedTab == this.tpDstrDetail)
        this.LoadDstrDetail();
      if (this.tcntrlCounters.SelectedTab != this.tpDetailEvidence)
        return;
      this.LoadDetailEvidence();
    }

    private void dgvDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (((DataGridView) sender).DataSource == null)
        return;
      DataGridViewRow row = ((DataGridView) sender).Rows[e.RowIndex];
      if (((CounterDetail) row.DataBoundItem).Period != null && ((CounterDetail) row.DataBoundItem).Period.PeriodId == this.monthClosed.PeriodId + 1)
      {
        row.DefaultCellStyle.BackColor = Color.PapayaWhip;
        row.DefaultCellStyle.ForeColor = Color.Black;
      }
      else
      {
        row.DefaultCellStyle.BackColor = Color.White;
        row.DefaultCellStyle.ForeColor = Color.Gray;
      }
    }

    private void LoadDistribute()
    {
      this.cbArchive.Visible = true;
      this.pnTools.Height = 73;
      this.btnSave.Enabled = false;
      this.btnAdd.Enabled = true;
      this.btnDelete.Enabled = true;
      if ((Convert.ToInt32(this.cmbBase.SelectedValue) == 2 || (int) this.level == 2) && Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
      {
        this.cbArchive.Enabled = false;
        this.cbArchive.Checked = false;
      }
      else
        this.cbArchive.Enabled = true;
      this.dgvDistribute.Columns.Clear();
      this.dgvDistribute.DataSource = (object) null;
      this.session = Domain.CurrentSession;
      IList<Distribute> distributeList = (IList<Distribute>) new List<Distribute>();
      string str1 = "";
      string str2 = (uint) Convert.ToInt32(this.cmbService.SelectedValue) <= 0U ? "" : " and d.Service.ServiceId={2}";
      string str3 = !(Options.SortService == " s.ServiceId") ? "d.Service.ServiceName," : "d.Service.ServiceId,";
      if (!this.cbArchive.Checked)
        str1 = " and d.Period.PeriodId={1}";
      if ((int) this.level == 2)
      {
        if (Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
          distributeList = this.session.CreateQuery(string.Format("select d from Distribute d left join fetch d.Period,Home h where d.Company.CompanyId={0} and d.Home=h " + str1 + str2 + " order by d.Period.PeriodId desc,h.Str.NameStr,DBA.LENGTHHOME(h.NHome),h.HomeKorp,d.Month.PeriodId desc," + str3 + "d.CounterId", (object) this.company.CompanyId, (object) Options.Period.PeriodId, (object) Convert.ToInt32(this.cmbService.SelectedValue))).List<Distribute>();
        else
          distributeList = this.session.CreateQuery(string.Format("select d from Distribute d left join fetch d.Period where d.Company.CompanyId={0} and d.Home is null " + str1 + str2 + " order by d.Period.PeriodId desc,d.Month.PeriodId desc," + str3 + "d.CounterId", (object) this.company.CompanyId, (object) Options.Period.PeriodId, (object) Convert.ToInt32(this.cmbService.SelectedValue))).List<Distribute>();
      }
      if ((int) this.level == 3)
        distributeList = this.session.CreateQuery(string.Format("select d from Distribute d left join fetch d.Period where d.Home.IdHome={0} " + str1 + str2 + "  order by d.Period.PeriodId desc,d.Month.PeriodId desc," + str3 + "d.CounterId", (object) this.home.IdHome, (object) Options.Period.PeriodId, (object) Convert.ToInt32(this.cmbService.SelectedValue))).List<Distribute>();
      this.dgvDistribute.DataSource = (object) distributeList;
      this.SetViewDistribute();
      this.dgvDistribute.Focus();
      this.insertRecord = false;
    }

    private void SetViewDistribute()
    {
      this.session = Domain.CurrentSession;
      this.dgvDistribute.Columns["Note"].HeaderText = "Примечания";
      this.dgvDistribute.Columns["Note"].Width = 250;
      this.homes = (IList<Home>) new List<Home>();
      if ((int) this.level == 2)
        this.homes = this.session.CreateQuery(string.Format("select h from Home h left join fetch h.Str, HomeLink hl where hl.Home=h and hl.Company.CompanyId={0} order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)", (object) this.company.CompanyId)).List<Home>();
      if ((int) this.level == 3)
        this.homes = this.session.CreateQuery(string.Format("select h from Home h left join fetch h.Str, HomeLink hl where hl.Home=h and hl.Company.CompanyId={0} and hl.Home.IdHome={1} order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)", (object) this.company.CompanyId, (object) this.home.IdHome)).List<Home>();
      KvrplHelper.AddMaskDateColumn(this.dgvDistribute, 0, "Месяц", "Month");
      KvrplHelper.AddComboBoxColumn(this.dgvDistribute, 1, (IList) null, (string) null, (string) null, "Номер счетчика", "Num", 100, 100);
      KvrplHelper.AddTextBoxColumn(this.dgvDistribute, 2, "Сумма", "Rent", 100, false);
      if ((int) Convert.ToInt16(this.cmbService.SelectedValue) == 0)
      {
        IList<Service> serviceList1 = (IList<Service>) new List<Service>();
        IList<Service> serviceList2 = this.session.CreateQuery(string.Format("select distinct s from Service s,ServiceParam sp where sp.Service_id=s.ServiceId and s.Root=0 and s.ServiceId<>0 and sp.Company_id={0} order by " + Options.SortService, (object) this.company.CompanyId)).List<Service>();
        serviceList2.Insert(0, new Service()
        {
          ServiceId = (short) 0,
          ServiceName = ""
        });
        KvrplHelper.AddComboBoxColumn(this.dgvDistribute, 0, (IList) serviceList2, "ServiceId", "ServiceName", "Услуга", "Service", 160, 160);
      }
      if (Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
        KvrplHelper.AddComboBoxColumn(this.dgvDistribute, 0, (IList) this.homes, "IdHome", "Address", "Адрес", "Address", 300, 300);
      DataGridViewColumn dataGridViewColumn = (DataGridViewColumn) new DataGridViewButtonColumn();
      dataGridViewColumn.Name = "Scheme";
      dataGridViewColumn.HeaderText = "Схема";
      this.dgvDistribute.Columns.Insert(2, dataGridViewColumn);
      if (this.cbArchive.Checked)
      {
        IList<Period> periodList = (IList<Period>) new List<Period>();
        KvrplHelper.AddComboBoxColumn(this.dgvDistribute, 0, Convert.ToInt32(this.cmbBase.SelectedValue) == 4 ? (IList) this.session.CreateQuery(string.Format("select p from Period p where PeriodId < (select max(Period.PeriodId) from Distribute where Home is null) or PeriodId<={0} order by PeriodId desc", (object) Options.Period.PeriodId)).List<Period>() : (IList) this.session.CreateQuery(string.Format("select p from Period p where PeriodId < (select max(Period.PeriodId) from Distribute where Home.IdHome={0}) or PeriodId<={1} order by PeriodId desc", (object) this.home.IdHome, (object) Options.Period.PeriodId)).List<Period>(), "PeriodId", "PeriodName", "Период", "Period", 100, 100);
      }
      KvrplHelper.ViewEdit(this.dgvDistribute);
      foreach (DataGridViewRow row in (IEnumerable) this.dgvDistribute.Rows)
      {
        if ((int) Convert.ToInt16(this.cmbService.SelectedValue) == 0 && ((Distribute) row.DataBoundItem).Service != null)
          row.Cells["Service"].Value = (object) ((Distribute) row.DataBoundItem).Service.ServiceId;
        DateTime? periodName;
        if (((Distribute) row.DataBoundItem).Month != null)
        {
          DataGridViewCell cell = row.Cells["Month"];
          periodName = ((Distribute) row.DataBoundItem).Month.PeriodName;
          // ISSUE: variable of a boxed type
          DateTime local = periodName.Value;
          cell.Value = (object) local;
        }
        int scheme = (int) ((Distribute) row.DataBoundItem).Scheme;
        if (true)
          row.Cells["Scheme"].Value = (object) ((Distribute) row.DataBoundItem).Scheme;
        if (this.cbArchive.Checked)
          row.Cells["Period"].Value = (object) ((Distribute) row.DataBoundItem).Period.PeriodId;
        row.Cells["Rent"].Value = (object) ((Distribute) row.DataBoundItem).Rent;
        int num1 = 0;
        try
        {
          ISession session = this.session;
          string format = "select ParamValue from CompanyParam where Period.PeriodId=0 and Company.CompanyId={0} and DBeg<='{1}' and DEnd>='{1}' and Param.ParamId=213";
          // ISSUE: variable of a boxed type
          short companyId = this.company.CompanyId;
          periodName = ((Distribute) row.DataBoundItem).Month.PeriodName;
          string baseFormat = KvrplHelper.DateToBaseFormat(periodName.Value);
          string queryString = string.Format(format, (object) companyId, (object) baseFormat);
          num1 = Convert.ToInt32(session.CreateQuery(queryString).UniqueResult());
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        if (((Distribute) row.DataBoundItem).Home != null)
        {
          row.Cells["Address"].Value = (object) ((Distribute) row.DataBoundItem).Home.IdHome;
          IList<Counter> counterList1 = (IList<Counter>) new List<Counter>();
          IList<Counter> counterList2;
          if ((uint) Convert.ToInt16(this.cmbService.SelectedValue) > 0U)
            counterList2 = this.session.CreateQuery(string.Format("select c from Counter c where c.Company.CompanyId={1} and c.Home.IdHome={0} and c.BaseCounter.Id=1 and isnull(c.ArchivesDate,'2999-12-31')>='{3}' and (c.Service.ServiceId=(select sp.CrossServ.ServiceId from CrossService sp where sp.Company.CompanyId={1} and sp.Service.ServiceId={2} and CrossType.CrossTypeId=2 and ((isnull((select ParamValue from HomeParam where Period.PeriodId=0 and Home.IdHome={0} and DBeg<='{3}' and DEnd>='{3}' and Param.ParamId=302),0)<>1 and {4}<>1) or {2} not in (73,74))) or c.Service.ServiceId=(select spr.CrossServ.ServiceId from CrossService spr where spr.Company.CompanyId={1} and spr.Service.ServiceId=(select sp.CrossServ.ServiceId from CrossService sp where sp.Company.CompanyId={1} and sp.Service.ServiceId={2} and CrossType.CrossTypeId=2) and CrossType.CrossTypeId=1 and (isnull((select ParamValue from HomeParam where Period.PeriodId=0 and Home.IdHome={0} and Company.CompanyId={1} and DBeg<='{3}' and DEnd>='{3}' and Param.ParamId=302),0)=1 or {4}=1))) order by regulatefld(c.CounterNum)", (object) ((Distribute) row.DataBoundItem).Home.IdHome, (object) this.company.CompanyId, (object) ((Service) this.cmbService.SelectedItem).ServiceId, (object) KvrplHelper.DateToBaseFormat(((Distribute) row.DataBoundItem).Month.PeriodName.Value), (object) num1)).List<Counter>();
          else
            counterList2 = this.session.CreateQuery(string.Format("select c from Counter c where c.Company.CompanyId={1} and c.Home.IdHome={0} and c.BaseCounter.Id=1 and isnull(c.ArchivesDate,'2999-12-31')>='{3}' and (c.Service.ServiceId=(select sp.CrossServ.ServiceId from CrossService sp where sp.Company.CompanyId={1} and sp.Service.ServiceId={2} and CrossType.CrossTypeId=2 and ((isnull((select ParamValue from HomeParam where Period.PeriodId=0 and Home.IdHome={0} and DBeg<='{3}' and DEnd>='{3}' and Param.ParamId=302),0)<>1 and {4}<>1) or {2} not in (73,74))) or c.Service.ServiceId=(select spr.CrossServ.ServiceId from CrossService spr where spr.Company.CompanyId={1} and spr.Service.ServiceId=(select sp.CrossServ.ServiceId from CrossService sp where sp.Company.CompanyId={1} and sp.Service.ServiceId={2} and CrossType.CrossTypeId=2) and CrossType.CrossTypeId=1 and (isnull((select ParamValue from HomeParam where Period.PeriodId=0 and Home.IdHome={0} and Company.CompanyId={1} and DBeg<='{3}' and DEnd>='{3}' and Param.ParamId=302),0)=1 or {4}=1))) order by regulatefld(c.CounterNum)", (object) ((Distribute) row.DataBoundItem).Home.IdHome, (object) this.company.CompanyId, (object) ((Distribute) row.DataBoundItem).Service.ServiceId, (object) KvrplHelper.DateToBaseFormat(((Distribute) row.DataBoundItem).Month.PeriodName.Value), (object) num1)).List<Counter>();
          counterList2.Insert(0, new Counter(0, "0"));
          row.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
          {
            DisplayStyleForCurrentCellOnly = true,
            ValueMember = "CounterId",
            DisplayMember = "CounterNum",
            DataSource = (object) counterList2
          };
          Counter counter = this.session.Get<Counter>((object) ((Distribute) row.DataBoundItem).CounterId);
          if (counterList2.IndexOf(counter) != -1 || ((Distribute) row.DataBoundItem).CounterId == 0)
            row.Cells["Num"].Value = (object) ((Distribute) row.DataBoundItem).CounterId;
        }
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 4)
        {
          IList<Counter> counterList1 = (IList<Counter>) new List<Counter>();
          counterList1 = (IList<Counter>) new List<Counter>();
          IList<Counter> counterList2;
          if ((uint) Convert.ToInt16(this.cmbService.SelectedValue) > 0U)
            counterList2 = this.session.CreateQuery(string.Format("select c from Counter c where c.Company.CompanyId={1} and c.BaseCounter.Id={5} and isnull(c.ArchivesDate,'2999-12-31')>='{3}' and (c.Service.ServiceId=(select sp.CrossServ.ServiceId from CrossService sp where sp.Company.CompanyId={1} and sp.Service.ServiceId={2} and CrossType.CrossTypeId=2 and {2} not in (73,74)) or c.Service.ServiceId=(select spr.CrossServ.ServiceId from CrossService spr where spr.Company.CompanyId={1} and spr.Service.ServiceId=(select sp.CrossServ.ServiceId from CrossService sp where sp.Company.CompanyId={1} and sp.Service.ServiceId={2} and CrossType.CrossTypeId=2) and CrossType.CrossTypeId=1)) order by regulatefld(c.CounterNum)", (object) 0, (object) this.company.CompanyId, (object) ((Service) this.cmbService.SelectedItem).ServiceId, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), (object) num1, (object) Convert.ToInt32(this.cmbBase.SelectedValue))).List<Counter>();
          else
            counterList2 = this.session.CreateQuery(string.Format("select c from Counter c where c.Company.CompanyId={1} and c.BaseCounter.Id=4 and isnull(c.ArchivesDate,'2999-12-31')>='{3}' and (c.Service.ServiceId=(select sp.CrossServ.ServiceId from CrossService sp where sp.Company.CompanyId={1} and sp.Service.ServiceId={2} and CrossType.CrossTypeId=2 and {2} not in (73,74))) or c.Service.ServiceId=(select spr.CrossServ.ServiceId from CrossService spr where spr.Company.CompanyId={1} and spr.Service.ServiceId=(select sp.CrossServ.ServiceId from CrossService sp where sp.Company.CompanyId={1} and sp.Service.ServiceId={2} and CrossType.CrossTypeId=2) and CrossType.CrossTypeId=1)) order by regulatefld(c.CounterNum)", (object) 0, (object) this.company.CompanyId, (object) ((Distribute) row.DataBoundItem).Service.ServiceId, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), (object) num1)).List<Counter>();
          counterList2.Insert(0, new Counter(0, "0"));
          row.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
          {
            DisplayStyleForCurrentCellOnly = true,
            ValueMember = "CounterId",
            DisplayMember = "CounterNum",
            DataSource = (object) counterList2
          };
          int num2;
          if ((int) ((Distribute) row.DataBoundItem).Service.ServiceId != 0)
          {
            int counterId = ((Distribute) row.DataBoundItem).CounterId;
            num2 = 1;
          }
          else
            num2 = 0;
          if (num2 != 0)
            row.Cells["Num"].Value = (object) ((Distribute) row.DataBoundItem).CounterId;
        }
      }
      this.session.Clear();
    }

    private void InsertDistribute()
    {
      this.insertRecord = true;
      Distribute distribute = new Distribute();
      distribute.DistributeId = (int) this.session.CreateSQLQuery("select DBA.gen_id('hmDistribute',1)").UniqueResult();
      distribute.Period = Options.Period;
      distribute.Service = (Service) this.cmbService.SelectedItem;
      IList<Distribute> distributeList = (IList<Distribute>) new List<Distribute>();
      if ((uint) this.dgvDistribute.Rows.Count > 0U)
        distributeList = (IList<Distribute>) (this.dgvDistribute.DataSource as List<Distribute>);
      distributeList.Add(distribute);
      this.dgvDistribute.Columns.Clear();
      this.dgvDistribute.DataSource = (object) null;
      this.dgvDistribute.DataSource = (object) distributeList;
      this.SetViewDistribute();
      this.dgvDistribute.CurrentCell = this.dgvDistribute.Rows[this.dgvDistribute.Rows.Count - 1].Cells[0];
    }

    private void SaveDistribute()
    {
      if (this.dgvDistribute.Rows.Count > 0 && this.dgvDistribute.CurrentRow != null)
      {
        this.session.Clear();
        this.session = Domain.CurrentSession;
        Distribute dataBoundItem = (Distribute) this.dgvDistribute.CurrentRow.DataBoundItem;
        dataBoundItem.Company = this.company;
        if (dataBoundItem.Period.PeriodId <= this.monthClosed.PeriodId)
        {
          int num = (int) MessageBox.Show("Невозможно внести изменения в закрытом периоде", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        if (Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
        {
          if (this.dgvDistribute.CurrentRow.Cells["Address"].Value != null)
          {
            dataBoundItem.Home = this.session.Get<Home>(this.dgvDistribute.CurrentRow.Cells["Address"].Value);
          }
          else
          {
            int num = (int) MessageBox.Show("Введите адрес", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        if (this.dgvDistribute.CurrentRow.Cells["Rent"].Value != null)
        {
          try
          {
            dataBoundItem.Rent = Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.dgvDistribute.CurrentRow.Cells["Rent"].Value.ToString()));
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Некорректный формат суммы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          if (this.dgvDistribute.CurrentRow.Cells["Month"].Value != null)
          {
            try
            {
              dataBoundItem.Month = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("PeriodName", (object) KvrplHelper.FirstDay(Convert.ToDateTime(this.dgvDistribute.CurrentRow.Cells["Month"].Value)))).List<Period>()[0];
            }
            catch (Exception ex)
            {
              int num = (int) MessageBox.Show("Некорректный формат месяца", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
          }
          else
            dataBoundItem.Month = Options.Period;
          if ((int) Convert.ToInt16(this.cmbService.SelectedValue) == 0)
          {
            if (this.dgvDistribute.CurrentRow.Cells["Service"].Value != null && (uint) Convert.ToInt32(this.dgvDistribute.CurrentRow.Cells["Service"].Value) > 0U)
            {
              dataBoundItem.Service = this.session.Get<Service>(this.dgvDistribute.CurrentRow.Cells["Service"].Value);
            }
            else
            {
              int num = (int) MessageBox.Show("Выберите услугу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
          }
          else
            dataBoundItem.Service = (Service) this.cmbService.SelectedItem;
          if (dataBoundItem.Note == null)
            dataBoundItem.Note = "";
          dataBoundItem.CounterId = Convert.ToInt32(this.dgvDistribute.CurrentRow.Cells["Num"].Value);
          dataBoundItem.Scheme = Convert.ToInt16(this.dgvDistribute.CurrentRow.Cells["Scheme"].Value);
          IList<Distribute> distributeList1 = (IList<Distribute>) new List<Distribute>();
          IList<Distribute> distributeList2;
          if (Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
            distributeList2 = this.session.CreateQuery(string.Format("from Distribute where Company.CompanyId={0} and Home.IdHome={1} and Month.PeriodId={2} and CounterId={3} and Service.ServiceId={4} and DistributeId<>{5}", (object) dataBoundItem.Company.CompanyId, (object) dataBoundItem.Home.IdHome, (object) dataBoundItem.Month.PeriodId, (object) dataBoundItem.CounterId, (object) dataBoundItem.Service.ServiceId, (object) dataBoundItem.DistributeId)).List<Distribute>();
          else
            distributeList2 = this.session.CreateQuery(string.Format("from Distribute where Company.CompanyId={0} and Month.PeriodId={1} and CounterId={2} and Service.ServiceId={3} and DistributeId<>{4}", (object) dataBoundItem.Company.CompanyId, (object) dataBoundItem.Month.PeriodId, (object) dataBoundItem.CounterId, (object) dataBoundItem.Service.ServiceId, (object) dataBoundItem.DistributeId)).List<Distribute>();
          if (distributeList2.Count > 0 && MessageBox.Show("Уже существует запись по этому счетчику за выбраный месяц. Суммы будут сложены. Все равно продолжить?", "Внимание!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
          {
            this.insertRecord = false;
            return;
          }
          dataBoundItem.UName = Options.Login;
          dataBoundItem.DEdit = DateTime.Now.Date;
          this.session.Clear();
          try
          {
            if (this.insertRecord)
            {
              this.insertRecord = false;
              this.session.Save((object) dataBoundItem);
            }
            else
              this.session.Update((object) dataBoundItem);
            this.session.Flush();
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Невозможно сохранить изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
        }
        else
        {
          int num = (int) MessageBox.Show("Введите сумму", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
      }
      this.session.Clear();
    }

    private void DeleteDistribute()
    {
      if (this.dgvDistribute.Rows.Count <= 0 || this.dgvDistribute.CurrentRow == null || MessageBox.Show("Удалить запись?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      this.session = Domain.CurrentSession;
      Distribute dataBoundItem = (Distribute) this.dgvDistribute.CurrentRow.DataBoundItem;
      if (dataBoundItem.Period.PeriodId <= this.monthClosed.PeriodId)
      {
        int num1 = (int) MessageBox.Show("Невозможно удалить запись из закрытого месяца", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        try
        {
          this.session.Delete((object) dataBoundItem);
          this.session.Flush();
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show("Невозможно удалить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        this.session.Clear();
      }
    }

    private void dgvDistribute_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.btnSave.Enabled = true;
      this.btnAdd.Enabled = false;
      this.btnDelete.Enabled = false;
    }

    private void dgvDistribute_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (!this.dgvDistribute.IsCurrentCellDirty)
        return;
      this.dgvDistribute.CommitEdit(DataGridViewDataErrorContexts.Commit);
      int num = 0;
      try
      {
        num = Convert.ToInt32(this.session.CreateQuery(string.Format("select ParamValue from CompanyParam where Period.PeriodId=0 and Company.CompanyId={0} and DBeg<='{1}' and DEnd>='{1}' and Param.ParamId=213", (object) this.company.CompanyId, (object) KvrplHelper.DateToBaseFormat(Convert.ToDateTime(this.dgvDistribute.CurrentRow.Cells["Month"].Value)))).UniqueResult());
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      if ((int) Convert.ToInt16(this.cmbService.SelectedValue) != 0 && Convert.ToInt32(this.cmbBase.SelectedValue) != 4 && (this.dgvDistribute.CurrentCell.ColumnIndex == this.dgvDistribute.Rows[this.dgvDistribute.CurrentRow.Index].Cells["Address"].ColumnIndex || this.dgvDistribute.CurrentCell.ColumnIndex == this.dgvDistribute.Rows[this.dgvDistribute.CurrentRow.Index].Cells["Month"].ColumnIndex) && this.dgvDistribute.CurrentRow.Cells["Address"].Value != null && this.dgvDistribute.CurrentRow.Cells["Month"].Value != null)
      {
        try
        {
          Convert.ToDateTime(this.dgvDistribute.CurrentRow.Cells["Month"].Value);
        }
        catch (Exception ex)
        {
          return;
        }
        this.session = Domain.CurrentSession;
        IList<Counter> counterList1 = (IList<Counter>) new List<Counter>();
        IList<Counter> counterList2 = this.session.CreateQuery(string.Format("select c from Counter c where c.Company.CompanyId={1} and c.Home.IdHome={0} and c.BaseCounter.Id=1 and isnull(c.ArchivesDate,'2999-12-31')>='{3}' and (c.Service.ServiceId=(select sp.CrossServ.ServiceId from CrossService sp where sp.Company.CompanyId={1} and sp.Service.ServiceId={2} and CrossType.CrossTypeId=2 and ((isnull((select ParamValue from HomeParam where Period.PeriodId=0 and Home.IdHome={0} and DBeg<='{3}' and DEnd>='{3}' and Param.ParamId=302),0)<>1 and {4}<>1) or {2} not in (73,74))) or c.Service.ServiceId=(select spr.CrossServ.ServiceId from CrossService spr where spr.Company.CompanyId={1} and spr.Service.ServiceId=(select sp.CrossServ.ServiceId from CrossService sp where sp.Company.CompanyId={1} and sp.Service.ServiceId={2} and CrossType.CrossTypeId=2) and CrossType.CrossTypeId=1 and (isnull((select ParamValue from HomeParam where Period.PeriodId=0 and Home.IdHome={0} and DBeg<='{3}' and DEnd>='{3}' and Param.ParamId=302),0)=1 or {4}=1))) order by regulatefld(c.CounterNum)", this.dgvDistribute.CurrentRow.Cells["Address"].Value, (object) this.company.CompanyId, (object) ((Service) this.cmbService.SelectedItem).ServiceId, (object) KvrplHelper.DateToBaseFormat(Convert.ToDateTime(this.dgvDistribute.CurrentRow.Cells["Month"].Value)), (object) num)).List<Counter>();
        this.session.Clear();
        counterList2.Insert(0, new Counter(0, "0"));
        this.dgvDistribute.CurrentRow.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "CounterId",
          DisplayMember = "CounterNum",
          DataSource = (object) counterList2
        };
      }
      if ((int) Convert.ToInt16(this.cmbService.SelectedValue) == 0 && (this.dgvDistribute.CurrentCell.ColumnIndex == this.dgvDistribute.Rows[this.dgvDistribute.CurrentRow.Index].Cells["Service"].ColumnIndex || this.dgvDistribute.CurrentCell.ColumnIndex == this.dgvDistribute.Rows[this.dgvDistribute.CurrentRow.Index].Cells["Month"].ColumnIndex) && this.dgvDistribute.CurrentRow.Cells["Service"].Value != null && this.dgvDistribute.CurrentRow.Cells["Month"].Value != null)
      {
        try
        {
          Convert.ToDateTime(this.dgvDistribute.CurrentRow.Cells["Month"].Value);
        }
        catch (Exception ex)
        {
          return;
        }
        this.session = Domain.CurrentSession;
        IList<Counter> counterList1 = (IList<Counter>) new List<Counter>();
        IList<Counter> counterList2;
        if (Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
        {
          if ((uint) Convert.ToInt16(this.dgvDistribute.CurrentRow.Cells["Service"].Value) > 0U)
            counterList2 = this.session.CreateQuery(string.Format("select c from Counter c where c.Company.CompanyId={1} and c.Home.IdHome={0} and c.BaseCounter.Id=1 and isnull(c.ArchivesDate,'2999-12-31')>='{3}' and (c.Service.ServiceId=(select sp.CrossServ.ServiceId from CrossService sp where sp.Company.CompanyId={1} and sp.Service.ServiceId={2} and CrossType.CrossTypeId=2 and ((isnull((select ParamValue from HomeParam where Period.PeriodId=0 and Home.IdHome={0} and DBeg<='{3}' and DEnd>='{3}' and Param.ParamId=302),0)<>1 and {4}<>1) or {2} not in (73,74))) or c.Service.ServiceId=(select spr.CrossServ.ServiceId from CrossService spr where spr.Company.CompanyId={1} and spr.Service.ServiceId=(select sp.CrossServ.ServiceId from CrossService sp where sp.Company.CompanyId={1} and sp.Service.ServiceId={2} and CrossType.CrossTypeId=2) and (isnull((select ParamValue from HomeParam where Period.PeriodId=0 and Home.IdHome={0} and DBeg<='{3}' and DEnd>='{3}' and Param.ParamId=302),0)=1 or {4}=1))) order by regulatefld(c.CounterNum)", this.dgvDistribute.CurrentRow.Cells["Address"].Value, (object) this.company.CompanyId, (object) Convert.ToInt16(this.dgvDistribute.CurrentRow.Cells["Service"].Value), (object) KvrplHelper.DateToBaseFormat(Convert.ToDateTime(this.dgvDistribute.CurrentRow.Cells["Month"].Value)), (object) num)).List<Counter>();
          else
            counterList2 = this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Home.IdHome", this.dgvDistribute.CurrentRow.Cells["Address"].Value)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter.Id", (object) 1)).AddOrder(Order.Asc("CounterNum")).List<Counter>();
        }
        else if ((uint) Convert.ToInt16(this.dgvDistribute.CurrentRow.Cells["Service"].Value) > 0U)
          counterList2 = this.session.CreateQuery(string.Format("select c from Counter c where c.Company.CompanyId={1} and c.BaseCounter.Id={5} and isnull(c.ArchivesDate,'2999-12-31')>='{3}' and (c.Service.ServiceId=(select sp.CrossServ.ServiceId from CrossService sp where sp.Company.CompanyId={1} and sp.Service.ServiceId={2} and CrossType.CrossTypeId=2) or c.Service.ServiceId=(select spr.CrossServ.ServiceId from CrossService spr where spr.Company.CompanyId={1} and spr.Service.ServiceId=(select sp.CrossServ.ServiceId from CrossService sp where sp.Company.CompanyId={1} and sp.Service.ServiceId={2} and CrossType.CrossTypeId=2) and CrossType.CrossTypeId=1 and {4}=1)) order by regulatefld(c.CounterNum)", (object) 0, (object) this.company.CompanyId, (object) Convert.ToInt16(this.dgvDistribute.CurrentRow.Cells["Service"].Value), (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), (object) num, (object) Convert.ToInt32(this.cmbBase.SelectedValue))).List<Counter>();
        else
          counterList2 = this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter.Id", (object) Convert.ToInt32(this.cmbBase.SelectedValue))).AddOrder(Order.Asc("CounterNum")).List<Counter>();
        this.session.Clear();
        counterList2.Insert(0, new Counter(0, "0"));
        this.dgvDistribute.CurrentRow.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "CounterId",
          DisplayMember = "CounterNum",
          DataSource = (object) counterList2
        };
      }
    }

    private void dgvDistribute_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (((DataGridView) sender).DataSource == null)
        return;
      DataGridViewRow row = ((DataGridView) sender).Rows[e.RowIndex];
      if (((Distribute) row.DataBoundItem).Period != null && ((Distribute) row.DataBoundItem).Period.PeriodId == this.monthClosed.PeriodId + 1)
      {
        row.DefaultCellStyle.BackColor = Color.PapayaWhip;
        row.DefaultCellStyle.ForeColor = Color.Black;
      }
      else
      {
        row.DefaultCellStyle.BackColor = Color.White;
        row.DefaultCellStyle.ForeColor = Color.Gray;
      }
    }

    private void dgvDistribute_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex <= 0 || e.RowIndex < 0 || !(this.dgvDistribute.Columns[e.ColumnIndex].Name == "Scheme"))
        return;
      short id = ((Distribute) this.dgvDistribute.CurrentRow.DataBoundItem).Scheme;
      FrmScheme frmScheme = new FrmScheme((short) 9, id);
      if (frmScheme.ShowDialog() == DialogResult.OK)
        id = frmScheme.CurrentId();
      this.dgvDistribute.CurrentRow.Cells["Scheme"].Value = (object) id;
      frmScheme.Dispose();
      this.btnAdd.Enabled = false;
      this.btnSave.Enabled = true;
      this.btnDelete.Enabled = false;
    }

    private void dgvDistribute_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    private void LoadDstrDetail()
    {
      this.pnTools.Height = 73;
      if ((Convert.ToInt32(this.cmbBase.SelectedValue) == 2 || (int) this.level == 2) && Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
      {
        this.cbArchive.Enabled = false;
        this.cbArchive.Checked = false;
      }
      else
        this.cbArchive.Enabled = true;
      this.dgvDstrDetail.Columns.Clear();
      this.dgvDstrDetail.DataSource = (object) null;
      this.session = Domain.CurrentSession;
      IList<DistributeDetail> distributeDetailList = (IList<DistributeDetail>) new List<DistributeDetail>();
      string str1 = "";
      string str2 = (uint) Convert.ToInt32(this.cmbService.SelectedValue) <= 0U ? "" : " and d.Service.ServiceId={2}";
      string str3 = !(Options.SortService == " s.ServiceId") ? "d.Service.ServiceName," : "d.Service.ServiceId,";
      string str4 = (uint) Convert.ToInt32(this.cmbCounter.SelectedValue) <= 0U ? "" : " and d.Home.IdHome = {3}";
      string str5 = Convert.ToInt32(this.cmbBase.SelectedValue) == 4 ? str4 + " and d.Home is null " : str4 + " and d.Home is not null ";
      string str6 = (uint) ((Period) this.cmbPeriod.SelectedItem).PeriodId <= 0U ? "" : " and d.Period.PeriodId = {4}";
      string str7 = (uint) ((Period) this.cmbMonth.SelectedItem).PeriodId <= 0U ? "" : " and d.Month.PeriodId = {5}";
      if (!this.cbArchive.Checked)
        str1 = " and dd.Period.PeriodId={1}";
      if ((int) this.level == 2)
      {
        if (Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
          distributeDetailList = this.session.CreateQuery(string.Format("select dd from DistributeDetail dd left join fetch dd.Distribute d left join fetch d.Home h where d.Company.CompanyId={0} " + str1 + str2 + str5 + str6 + str7 + " order by dd.Period.PeriodId desc,h.Str.NameStr,DBA.LENGTHHOME(h.NHome),d.Period.PeriodId desc,d.Month.PeriodId desc, " + str3 + "d.CounterId ", (object) this.company.CompanyId, (object) Options.Period.PeriodId, (object) Convert.ToInt32(this.cmbService.SelectedValue), (object) Convert.ToInt32(this.cmbCounter.SelectedValue), (object) ((Period) this.cmbPeriod.SelectedItem).PeriodId, (object) ((Period) this.cmbMonth.SelectedItem).PeriodId)).List<DistributeDetail>();
        else
          distributeDetailList = this.session.CreateQuery(string.Format("select dd from DistributeDetail dd left join fetch dd.Distribute d where d.Company.CompanyId={0} " + str1 + str2 + str5 + str6 + str7 + " order by dd.Period.PeriodId desc,d.Period.PeriodId desc,d.Month.PeriodId desc, " + str3 + "d.CounterId ", (object) this.company.CompanyId, (object) Options.Period.PeriodId, (object) Convert.ToInt32(this.cmbService.SelectedValue), (object) Convert.ToInt32(this.cmbCounter.SelectedValue), (object) ((Period) this.cmbPeriod.SelectedItem).PeriodId, (object) ((Period) this.cmbMonth.SelectedItem).PeriodId)).List<DistributeDetail>();
      }
      if ((int) this.level == 3)
        distributeDetailList = this.session.CreateQuery(string.Format("select dd from DistributeDetail dd left join fetch dd.Distribute d left join fetch d.Period where d.Home.IdHome={0} " + str1 + str2 + str5 + str6 + str7 + "  order by dd.Period.PeriodId desc,d.Period.PeriodId desc,d.Month.PeriodId desc," + str3 + "d.CounterId ", (object) this.home.IdHome, (object) Options.Period.PeriodId, (object) Convert.ToInt32(this.cmbService.SelectedValue), (object) Convert.ToInt32(this.cmbCounter.SelectedValue), (object) ((Period) this.cmbPeriod.SelectedItem).PeriodId, (object) ((Period) this.cmbMonth.SelectedItem).PeriodId)).List<DistributeDetail>();
      this.dgvDstrDetail.DataSource = (object) distributeDetailList;
      this.SetViewDstrDetail();
      this.dgvDstrDetail.Focus();
      this.insertRecord = false;
    }

    private void SetViewDstrDetail()
    {
      this.session = Domain.CurrentSession;
      this.dgvDstrDetail.Columns["Rent"].HeaderText = "Сумма распределения";
      this.dgvDstrDetail.Columns["RentDstr"].HeaderText = "Сумма по связанной услуге";
      this.dgvDstrDetail.Columns["ParamDstr"].HeaderText = "Сумма значений параметров";
      this.dgvDstrDetail.Columns["Coeff"].HeaderText = "Коэффициент";
      this.dgvDstrDetail.Columns["Coeff"].DefaultCellStyle.Format = "F8";
      this.homes = (IList<Home>) new List<Home>();
      this.counters = (IList<Counter>) new List<Counter>();
      if ((int) this.level == 2)
      {
        this.homes = this.session.CreateQuery(string.Format("select h from Home h left join fetch h.Str, HomeLink hl where hl.Home=h and hl.Company.CompanyId={0} order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)", (object) this.company.CompanyId)).List<Home>();
        this.counters = this.session.CreateQuery(string.Format("select c from Counter c where c.Company.CompanyId={0} and c.BaseCounter.Id={2} and c.Complex.ComplexId={1}", (object) this.company.CompanyId, (object) Options.Complex.ComplexId, (object) Convert.ToInt32(this.cmbBase.SelectedValue))).List<Counter>();
      }
      if ((int) this.level == 3)
      {
        this.homes = this.session.CreateQuery(string.Format("select h from Home h left join fetch h.Str, HomeLink hl where hl.Home=h and hl.Company.CompanyId={0} and hl.Home.IdHome={1} order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)", (object) this.company.CompanyId, (object) this.home.IdHome)).List<Home>();
        this.counters = this.session.CreateQuery(string.Format("select c from Counter c where c.Home.IdHome={0} and c.BaseCounter.Id=1 and c.Complex.ComplexId={1}", (object) this.home.IdHome, (object) Options.Complex.ComplexId)).List<Counter>();
      }
      this.counters.Insert(0, new Counter(0, "0"));
      KvrplHelper.AddComboBoxColumn(this.dgvDstrDetail, 0, (IList) this.periods, "PeriodId", "PeriodName", "Период расчета", "PeriodDstr", 100, 100);
      KvrplHelper.AddComboBoxColumn(this.dgvDstrDetail, 1, (IList) this.counters, "CounterId", "CounterNum", "Номер счетчика", "Num", 100, 100);
      KvrplHelper.AddComboBoxColumn(this.dgvDstrDetail, 2, (IList) this.periods, "PeriodId", "PeriodName", "Период", "Period", 100, 100);
      KvrplHelper.AddComboBoxColumn(this.dgvDstrDetail, 3, (IList) this.periods, "PeriodId", "PeriodName", "За месяц", "Month", 100, 100);
      if ((int) Convert.ToInt16(this.cmbService.SelectedValue) == 0)
      {
        IList<Service> serviceList1 = (IList<Service>) new List<Service>();
        IList<Service> serviceList2 = this.session.CreateQuery(string.Format("select distinct s from Service s,ServiceParam sp where sp.Service_id=s.ServiceId and s.Root=0 and s.ServiceId<>0 and sp.Company_id={0} order by " + Options.SortService, (object) this.company.CompanyId)).List<Service>();
        serviceList2.Insert(0, new Service()
        {
          ServiceId = (short) 0,
          ServiceName = ""
        });
        KvrplHelper.AddComboBoxColumn(this.dgvDstrDetail, 1, (IList) serviceList2, "ServiceId", "ServiceName", "Услуга", "Service", 160, 160);
      }
      if (Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
        KvrplHelper.AddComboBoxColumn(this.dgvDstrDetail, 0, (IList) this.homes, "IdHome", "Address", "Адрес", "Address", 300, 300);
      foreach (DataGridViewRow row in (IEnumerable) this.dgvDstrDetail.Rows)
      {
        if ((int) Convert.ToInt16(this.cmbService.SelectedValue) == 0 && ((DistributeDetail) row.DataBoundItem).Distribute.Service != null)
          row.Cells["Service"].Value = (object) ((DistributeDetail) row.DataBoundItem).Distribute.Service.ServiceId;
        if (((DistributeDetail) row.DataBoundItem).Distribute.Period != null)
          row.Cells["Period"].Value = (object) ((DistributeDetail) row.DataBoundItem).Distribute.Period.PeriodId;
        if (((DistributeDetail) row.DataBoundItem).Distribute.Month != null)
          row.Cells["Month"].Value = (object) ((DistributeDetail) row.DataBoundItem).Distribute.Month.PeriodId;
        if (((DistributeDetail) row.DataBoundItem).Period != null)
          row.Cells["PeriodDstr"].Value = (object) ((DistributeDetail) row.DataBoundItem).Period.PeriodId;
        row.Cells["Rent"].Value = (object) ((DistributeDetail) row.DataBoundItem).Rent;
        row.Cells["RentDstr"].Value = (object) ((DistributeDetail) row.DataBoundItem).RentDstr;
        row.Cells["ParamDstr"].Value = (object) ((DistributeDetail) row.DataBoundItem).ParamDstr;
        row.Cells["Coeff"].Value = (object) ((DistributeDetail) row.DataBoundItem).Coeff;
        row.Cells["Num"].Value = (object) ((DistributeDetail) row.DataBoundItem).Distribute.CounterId;
        if (((DistributeDetail) row.DataBoundItem).Distribute.Home != null)
          row.Cells["Address"].Value = (object) ((DistributeDetail) row.DataBoundItem).Distribute.Home.IdHome;
      }
      this.session.Clear();
    }

    private void dgvDstrDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (((DataGridView) sender).DataSource == null)
        return;
      DataGridViewRow row = ((DataGridView) sender).Rows[e.RowIndex];
      if (((DistributeDetail) row.DataBoundItem).Period != null && ((DistributeDetail) row.DataBoundItem).Period.PeriodId == this.monthClosed.PeriodId + 1)
      {
        row.DefaultCellStyle.BackColor = Color.PapayaWhip;
        row.DefaultCellStyle.ForeColor = Color.Black;
      }
      else
      {
        row.DefaultCellStyle.BackColor = Color.White;
        row.DefaultCellStyle.ForeColor = Color.Gray;
      }
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      string commandText = "";
      string str1 = "";
      string str2 = (uint) Convert.ToInt32(this.cmbService.SelectedValue) <= 0U ? "" : " and hd.Service_Id={2}";
      string str3 = !(Options.SortService == " s.Service_Id") ? "service," : "hd.Service_Id,";
      string str4 = (uint) Convert.ToInt32(this.cmbCounter.SelectedValue) <= 0U ? "" : " and hd.IdHome = {3}";
      string str5 = Convert.ToInt32(this.cmbBase.SelectedValue) == 4 ? str4 + " and hd.idHome is null " : str4 + " and hd.idHome is not null ";
      string str6 = (uint) ((Period) this.cmbPeriod.SelectedItem).PeriodId <= 0U ? "" : " and hd.Period_Id = {4}";
      string str7 = (uint) ((Period) this.cmbMonth.SelectedItem).PeriodId <= 0U ? "" : " and hd.Month_Id = {5}";
      if (!this.cbArchive.Checked)
        str1 = " and d.Period_Id={1}";
      if ((int) this.level == 2)
      {
        if (Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
          commandText = string.Format("select d.*,(select trim(str)||', д.'||home||(case home_korp when '' then '' else ', корп.'||home_korp end) from homes ah,di_str ad where ah.idhome=hd.idhome and ah.idstr=ad.idstr) as adr, (select period_value from dcPeriod where period_id=d.period_id) as rentperiod,(select period_value from dcPeriod where period_id=hd.period_id) as period,(select period_value from dcPeriod where period_id=hd.month_id) as month, (select service_name from dcService where service_id=hd.service_id) as service, (select counter_num from cntrCounter where counter_id=hd.counter_id) as num from DBA.dstrDetail d left outer join DBA.hmDistribute hd on d.Distribute_Id=hd.Distribute_id left outer join DBA.Homes h on hd.IdHome=h.Idhome, DBA.DI_Str s where h.IdStr=s.IdStr and hd.Company_Id={0} " + str1 + str2 + str5 + str6 + str7 + "order by d.Period_Id desc,s.Str, DBA.lengthhome(h.Home), hd.Period_Id desc, hd.Month_Id desc," + str3 + "hd.Counter_Id ", (object) this.company.CompanyId, (object) Options.Period.PeriodId, (object) Convert.ToInt32(this.cmbService.SelectedValue), (object) Convert.ToInt32(this.cmbCounter.SelectedValue), (object) ((Period) this.cmbPeriod.SelectedItem).PeriodId, (object) ((Period) this.cmbMonth.SelectedItem).PeriodId);
        else
          commandText = string.Format("select d.*,(select company_name from dcCompany where company_id={0}) as adr, (select period_value from dcPeriod where period_id=d.period_id) as rentperiod,(select period_value from dcPeriod where period_id=hd.period_id) as period,(select period_value from dcPeriod where period_id=hd.month_id) as month, (select service_name from dcService where service_id=hd.service_id) as service, (select counter_num from cntrCounter where counter_id=hd.counter_id) as num from DBA.dstrDetail d left outer join DBA.hmDistribute hd on d.Distribute_Id=hd.Distribute_id where hd.idhome is null and hd.Company_Id={0} " + str1 + str2 + str5 + str6 + str7 + "order by d.Period_Id desc, hd.Period_Id desc, hd.Month_Id desc," + str3 + "hd.Counter_Id ", (object) this.company.CompanyId, (object) Options.Period.PeriodId, (object) Convert.ToInt32(this.cmbService.SelectedValue), (object) Convert.ToInt32(this.cmbCounter.SelectedValue), (object) ((Period) this.cmbPeriod.SelectedItem).PeriodId, (object) ((Period) this.cmbMonth.SelectedItem).PeriodId);
      }
      if ((int) this.level == 3)
        commandText = string.Format("select d.*,(select trim(str)||', д.'||home||(case home_korp when '' then '' else ', корп.'||home_korp end) from homes ah,di_str ad where ah.idhome=hd.idhome and ah.idstr=ad.idstr) as adr, (select period_value from dcPeriod where period_id=d.period_id) as rentperiod,(select period_value from dcPeriod where period_id=hd.period_id) as period,(select period_value from dcPeriod where period_id=hd.month_id) as month, (select service_name from dcService where service_id=hd.service_id) as service, (select counter_num from cntrCounter where counter_id=hd.counter_id) as num from DBA.dstrDetail d left outer join DBA.hmDistribute hd on d.Distribute_Id=hd.Distribute_id left outer join DBA.Homes h on hd.IdHome=h.Idhome, DBA.DI_Str s where h.IdStr=s.IdStr and hd.idhome={0} " + str1 + str2 + str5 + str6 + str7 + "order by d.Period_Id desc, s.Str, DBA.lengthhome(h.Home), hd.Period_Id desc, hd.Month_Id desc," + str3 + "hd.counter_id ", (object) this.home.IdHome, (object) Options.Period.PeriodId, (object) Convert.ToInt32(this.cmbService.SelectedValue), (object) Convert.ToInt32(this.cmbCounter.SelectedValue), (object) ((Period) this.cmbPeriod.SelectedItem).PeriodId, (object) ((Period) this.cmbMonth.SelectedItem).PeriodId);
      this.dsReport = OleDbHelper.ExecuteDataset(new OleDbConnection(string.Format("Provider={4};Eng={0};Uid={1};Pwd={2}; Links={3}", (object) Options.BaseName, (object) Options.Login, (object) Options.Pwd, (object) ("tcpip{" + Options.Host + "}"), (object) Options.Provider)), CommandType.Text, commandText, 10000);
      try
      {
        Report report = new Report();
        report.RegisterData(this.dsReport.Tables[0], "dstrDetail");
        report.GetDataSource("dstrDetail").Enabled = true;
        ReportPage reportPage = new ReportPage();
        reportPage.Name = "Page1";
        reportPage.Landscape = true;
        report.Pages.Add((Base) reportPage);
        reportPage.ReportTitle = new ReportTitleBand();
        reportPage.ReportTitle.Name = "ReportTitle1";
        reportPage.ReportTitle.Height = 100f;
        reportPage.ReportTitle.CanGrow = true;
        DataBand dataBand = new DataBand();
        dataBand.Name = "Data1";
        dataBand.Height = Units.Centimeters * 0.5f;
        dataBand.DataSource = report.GetDataSource("dstrDetail");
        dataBand.CanGrow = true;
        reportPage.Bands.Add((Base) dataBand);
        TextObject textObject1 = new TextObject();
        textObject1.Name = "Title";
        textObject1.Bounds = new RectangleF(0.0f, Units.Centimeters * 0.5f, 1050f, Units.Centimeters * 1f);
        textObject1.Text = "Детализация распределения сумм за " + string.Format("{0:MMMM yyyy}", (object) Options.Period.PeriodName.Value) + " \n " + this.company.CompanyName + " " + this.cmbCounter.Text;
        textObject1.HorzAlign = HorzAlign.Center;
        textObject1.Font = new Font("Tahoma", 12f, FontStyle.Bold);
        textObject1.TextColor = Color.White;
        textObject1.FillColor = Color.Green;
        textObject1.CanGrow = true;
        reportPage.ReportTitle.Objects.Add((Base) textObject1);
        TextObject textObject2 = new TextObject();
        textObject2.Name = "Title";
        textObject2.Bounds = new RectangleF(0.0f, 0.0f, 1050f, Units.Centimeters * 0.5f);
        textObject2.Text = "[FormatDateTime([Date])]";
        textObject2.HorzAlign = HorzAlign.Right;
        textObject2.Font = new Font("Tahoma", 10f);
        reportPage.ReportTitle.Objects.Add((Base) textObject2);
        reportPage.PageHeader = new PageHeaderBand();
        reportPage.PageHeader.Name = "PageHeaderName1";
        reportPage.PageHeader.Height = Units.Centimeters * 1.5f;
        TextObject textObject3 = new TextObject();
        textObject3.Name = "Text2";
        textObject3.Bounds = new RectangleF(0.0f, 0.0f, 200f, Units.Centimeters * 1.5f);
        textObject3.Text = "Адрес";
        textObject3.HorzAlign = HorzAlign.Center;
        textObject3.Font = new Font("Tahoma", 10f, FontStyle.Bold);
        textObject3.Border.LeftLine.Width = 0.1f;
        textObject3.Border.Color = Color.Black;
        textObject3.Border.Lines = BorderLines.All;
        reportPage.PageHeader.Objects.Add((Base) textObject3);
        TextObject textObject4 = new TextObject();
        textObject4.Name = "Text14";
        textObject4.Bounds = new RectangleF(200f, 0.0f, 80f, Units.Centimeters * 1.5f);
        textObject4.Text = "Период расчета";
        textObject4.HorzAlign = HorzAlign.Center;
        textObject4.Font = new Font("Tahoma", 10f, FontStyle.Bold);
        textObject4.Border.LeftLine.Width = 0.1f;
        textObject4.Border.Color = Color.Black;
        textObject4.Border.Lines = BorderLines.All;
        reportPage.PageHeader.Objects.Add((Base) textObject4);
        TextObject textObject5 = new TextObject();
        textObject5.Name = "Text10";
        textObject5.Bounds = new RectangleF(280f, 0.0f, 130f, Units.Centimeters * 1.5f);
        textObject5.Text = "Услуга";
        textObject5.HorzAlign = HorzAlign.Center;
        textObject5.Font = new Font("Tahoma", 10f, FontStyle.Bold);
        textObject5.Border.LeftLine.Width = 0.1f;
        textObject5.Border.Color = Color.Black;
        textObject5.Border.Lines = BorderLines.All;
        reportPage.PageHeader.Objects.Add((Base) textObject5);
        TextObject textObject6 = new TextObject();
        textObject6.Name = "Text11";
        textObject6.Bounds = new RectangleF(410f, 0.0f, 120f, Units.Centimeters * 1.5f);
        textObject6.Text = "Номер счетчика";
        textObject6.HorzAlign = HorzAlign.Center;
        textObject6.Font = new Font("Tahoma", 10f, FontStyle.Bold);
        textObject6.Border.LeftLine.Width = 0.1f;
        textObject6.Border.Color = Color.Black;
        textObject6.Border.Lines = BorderLines.All;
        reportPage.PageHeader.Objects.Add((Base) textObject6);
        TextObject textObject7 = new TextObject();
        textObject7.Name = "Text12";
        textObject7.Bounds = new RectangleF(530f, 0.0f, 80f, Units.Centimeters * 1.5f);
        textObject7.Text = "Период";
        textObject7.HorzAlign = HorzAlign.Center;
        textObject7.Font = new Font("Tahoma", 10f, FontStyle.Bold);
        textObject7.Border.LeftLine.Width = 0.1f;
        textObject7.Border.Color = Color.Black;
        textObject7.Border.Lines = BorderLines.All;
        reportPage.PageHeader.Objects.Add((Base) textObject7);
        TextObject textObject8 = new TextObject();
        textObject8.Name = "Text13";
        textObject8.Bounds = new RectangleF(610f, 0.0f, 80f, Units.Centimeters * 1.5f);
        textObject8.Text = "За месяц";
        textObject8.HorzAlign = HorzAlign.Center;
        textObject8.Font = new Font("Tahoma", 10f, FontStyle.Bold);
        textObject8.Border.LeftLine.Width = 0.1f;
        textObject8.Border.Color = Color.Black;
        textObject8.Border.Lines = BorderLines.All;
        reportPage.PageHeader.Objects.Add((Base) textObject8);
        TextObject textObject9 = new TextObject();
        textObject9.Name = "Text15";
        textObject9.Bounds = new RectangleF(690f, 0.0f, 80f, Units.Centimeters * 1.5f);
        textObject9.Text = "Сумма распределения";
        textObject9.HorzAlign = HorzAlign.Center;
        textObject9.Font = new Font("Tahoma", 10f, FontStyle.Bold);
        textObject9.Border.LeftLine.Width = 0.1f;
        textObject9.Border.Color = Color.Black;
        textObject9.Border.Lines = BorderLines.All;
        reportPage.PageHeader.Objects.Add((Base) textObject9);
        TextObject textObject10 = new TextObject();
        textObject10.Name = "Text16";
        textObject10.Bounds = new RectangleF(770f, 0.0f, 90f, Units.Centimeters * 1.5f);
        textObject10.Text = "Сумма по связанной услуге";
        textObject10.HorzAlign = HorzAlign.Center;
        textObject10.Font = new Font("Tahoma", 10f, FontStyle.Bold);
        textObject10.Border.LeftLine.Width = 0.1f;
        textObject10.Border.Color = Color.Black;
        textObject10.Border.Lines = BorderLines.All;
        reportPage.PageHeader.Objects.Add((Base) textObject10);
        TextObject textObject11 = new TextObject();
        textObject11.Name = "Text17";
        textObject11.Bounds = new RectangleF(860f, 0.0f, 100f, Units.Centimeters * 1.5f);
        textObject11.Text = "Сумма значений параметров";
        textObject11.HorzAlign = HorzAlign.Center;
        textObject11.Font = new Font("Tahoma", 10f, FontStyle.Bold);
        textObject11.Border.LeftLine.Width = 0.1f;
        textObject11.Border.Color = Color.Black;
        textObject11.Border.Lines = BorderLines.All;
        reportPage.PageHeader.Objects.Add((Base) textObject11);
        TextObject textObject12 = new TextObject();
        textObject12.Name = "Text18";
        textObject12.Bounds = new RectangleF(960f, 0.0f, 90f, Units.Centimeters * 1.5f);
        textObject12.Text = "Коэффициент";
        textObject12.HorzAlign = HorzAlign.Center;
        textObject12.Font = new Font("Tahoma", 10f, FontStyle.Bold);
        textObject12.Border.LeftLine.Width = 0.1f;
        textObject12.Border.Color = Color.Black;
        textObject12.Border.Lines = BorderLines.All;
        reportPage.PageHeader.Objects.Add((Base) textObject12);
        TextObject textObject13 = new TextObject();
        textObject13.Name = "Text3";
        textObject13.Bounds = new RectangleF(0.0f, 0.0f, 200f, Units.Centimeters * 0.5f);
        textObject13.Text = "[dstrDetail.adr]";
        textObject13.Font = new Font("Tahoma", 8f);
        textObject13.Border.LeftLine.Width = 0.1f;
        textObject13.Border.Color = Color.Black;
        textObject13.Border.Lines = BorderLines.All;
        textObject13.CanGrow = true;
        textObject13.GrowToBottom = true;
        dataBand.Objects.Add((Base) textObject13);
        TextObject textObject14 = new TextObject();
        textObject14.Name = "Text8";
        textObject14.Bounds = new RectangleF(200f, 0.0f, 80f, Units.Centimeters * 0.5f);
        textObject14.Text = "[FormatDateTime([dstrDetail.rentperiod])]";
        textObject14.HorzAlign = HorzAlign.Center;
        textObject14.Font = new Font("Tahoma", 8f);
        textObject14.Border.LeftLine.Width = 0.1f;
        textObject14.Border.Color = Color.Black;
        textObject14.Border.Lines = BorderLines.All;
        textObject14.CanGrow = true;
        textObject14.GrowToBottom = true;
        dataBand.Objects.Add((Base) textObject14);
        TextObject textObject15 = new TextObject();
        textObject15.Name = "Text4";
        textObject15.Bounds = new RectangleF(280f, 0.0f, 130f, Units.Centimeters * 0.5f);
        textObject15.Text = "[dstrDetail.service]";
        textObject15.Font = new Font("Tahoma", 8f);
        textObject15.Border.LeftLine.Width = 0.1f;
        textObject15.Border.Color = Color.Black;
        textObject15.Border.Lines = BorderLines.All;
        textObject15.CanGrow = true;
        textObject15.GrowToBottom = true;
        dataBand.Objects.Add((Base) textObject15);
        TextObject textObject16 = new TextObject();
        textObject16.Name = "Text5";
        textObject16.Bounds = new RectangleF(410f, 0.0f, 120f, Units.Centimeters * 0.5f);
        textObject16.Text = "[dstrDetail.num]";
        textObject16.Font = new Font("Tahoma", 8f);
        textObject16.HorzAlign = HorzAlign.Center;
        textObject16.Border.LeftLine.Width = 0.1f;
        textObject16.Border.Color = Color.Black;
        textObject16.Border.Lines = BorderLines.All;
        textObject16.CanGrow = true;
        textObject16.GrowToBottom = true;
        dataBand.Objects.Add((Base) textObject16);
        TextObject textObject17 = new TextObject();
        textObject17.Name = "Text6";
        textObject17.Bounds = new RectangleF(530f, 0.0f, 80f, Units.Centimeters * 0.5f);
        textObject17.Text = "[FormatDateTime([dstrDetail.period])]";
        textObject17.Font = new Font("Tahoma", 8f);
        textObject17.HorzAlign = HorzAlign.Center;
        textObject17.Border.LeftLine.Width = 0.1f;
        textObject17.Border.Color = Color.Black;
        textObject17.Border.Lines = BorderLines.All;
        textObject17.CanGrow = true;
        textObject17.GrowToBottom = true;
        dataBand.Objects.Add((Base) textObject17);
        TextObject textObject18 = new TextObject();
        textObject18.Name = "Text7";
        textObject18.Bounds = new RectangleF(610f, 0.0f, 80f, Units.Centimeters * 0.5f);
        textObject18.Text = "[FormatDateTime([dstrDetail.month])]";
        textObject18.Font = new Font("Tahoma", 8f);
        textObject18.HorzAlign = HorzAlign.Center;
        textObject18.Border.LeftLine.Width = 0.1f;
        textObject18.Border.Color = Color.Black;
        textObject18.Border.Lines = BorderLines.All;
        textObject18.CanGrow = true;
        textObject18.GrowToBottom = true;
        dataBand.Objects.Add((Base) textObject18);
        TextObject textObject19 = new TextObject();
        textObject19.Name = "Text19";
        textObject19.Bounds = new RectangleF(690f, 0.0f, 80f, Units.Centimeters * 0.5f);
        textObject19.Text = "[dstrDetail.rent]";
        textObject19.Font = new Font("Tahoma", 8f);
        textObject19.HorzAlign = HorzAlign.Right;
        textObject19.Border.LeftLine.Width = 0.1f;
        textObject19.Border.Color = Color.Black;
        textObject19.Border.Lines = BorderLines.All;
        textObject19.CanGrow = true;
        textObject19.GrowToBottom = true;
        dataBand.Objects.Add((Base) textObject19);
        TextObject textObject20 = new TextObject();
        textObject20.Name = "Text20";
        textObject20.Bounds = new RectangleF(770f, 0.0f, 90f, Units.Centimeters * 0.5f);
        textObject20.Text = "[dstrDetail.rent_dstr]";
        textObject20.Font = new Font("Tahoma", 8f);
        textObject20.HorzAlign = HorzAlign.Right;
        textObject20.Border.LeftLine.Width = 0.1f;
        textObject20.Border.Color = Color.Black;
        textObject20.Border.Lines = BorderLines.All;
        textObject20.CanGrow = true;
        textObject20.GrowToBottom = true;
        dataBand.Objects.Add((Base) textObject20);
        TextObject textObject21 = new TextObject();
        textObject21.Name = "Text21";
        textObject21.Bounds = new RectangleF(860f, 0.0f, 100f, Units.Centimeters * 0.5f);
        textObject21.Text = "[dstrDetail.param_dstr]";
        textObject21.Font = new Font("Tahoma", 8f);
        textObject21.HorzAlign = HorzAlign.Right;
        textObject21.Border.LeftLine.Width = 0.1f;
        textObject21.Border.Color = Color.Black;
        textObject21.Border.Lines = BorderLines.All;
        textObject21.CanGrow = true;
        textObject21.GrowToBottom = true;
        dataBand.Objects.Add((Base) textObject21);
        TextObject textObject22 = new TextObject();
        textObject22.Name = "Text22";
        textObject22.Bounds = new RectangleF(960f, 0.0f, 90f, Units.Centimeters * 0.5f);
        textObject22.Text = "[dstrDetail.coeff]";
        textObject22.Font = new Font("Tahoma", 8f);
        textObject22.HorzAlign = HorzAlign.Center;
        textObject22.Border.LeftLine.Width = 0.1f;
        textObject22.Border.Color = Color.Black;
        textObject22.Border.Lines = BorderLines.All;
        textObject22.CanGrow = true;
        textObject22.GrowToBottom = true;
        dataBand.Objects.Add((Base) textObject22);
        report.Show();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Невозможно сформировать отчет", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void LoadScheme()
    {
      this.Cursor = Cursors.WaitCursor;
      this.btnAdd.Enabled = true;
      this.btnDelete.Enabled = true;
      this.btnSave.Enabled = false;
      this.pnTools.Height = 73;
      this.cbArchive.Visible = true;
      if ((int) this.level == 2 && Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
      {
        this.cbArchive.Enabled = false;
        this.cbArchive.Checked = false;
      }
      else
        this.cbArchive.Enabled = true;
      IList<CounterScheme> counterSchemeList = (IList<CounterScheme>) new List<CounterScheme>();
      this.session.Clear();
      this.session = Domain.CurrentSession;
      this.counters = (IList<Counter>) new List<Counter>();
      string str1 = "";
      string str2 = "";
      string str3 = "";
      string str4 = "";
      if ((uint) Convert.ToInt16(this.cmbService.SelectedValue) > 0U)
        str1 = " and s.Service.ServiceId={3}";
      if (this.tcntrlCounters.SelectedTab == this.tpCounters)
        str2 = " isnull(s.ArchivesDate,'2999-12-31') desc,";
      if ((int) this.level == 2)
        str2 += " st.NameStr,DBA.lengthhome(s.Home.NHome),s.Home.HomeKorp,";
      if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
      {
        str4 = string.Format(" and s.LsClient.Complex.IdFk in ({0},{1})", (object) Options.Complex.IdFk, (object) Options.ComplexArenda.IdFk);
        if (!Options.Kvartplata)
          str4 = string.Format(" and s.LsClient.Complex.IdFk={0}", (object) Options.ComplexArenda.IdFk);
        if (!Options.Arenda)
          str4 = string.Format(" and s.LsClient.Complex.IdFk={0}", (object) Options.Complex.IdFk);
        str2 += " DBA.lengthhome(s.LsClient.Flat.NFlat),";
      }
      string str5 = !(Options.SortService == " s.ServiceId") ? " s.Service.ServiceName" : " s.Service.ServiceId";
      int num1 = 0;
      if (this.pastTime)
        num1 = Options.Period.PeriodId;
      if (!this.cbArchive.Checked && !this.pastTime)
        str3 = " and cs.DEnd>='{5}'";
      if ((int) this.level == 3)
        counterSchemeList = this.session.CreateQuery(string.Format("select cs from CounterScheme cs join fetch cs.Counter s left join fetch s.Service left join fetch s.TypeCounter where cs.Period.PeriodId={4} and s.Complex.ComplexId={0} and s.Home.IdHome={1} and s.BaseCounter.Id={2} and s.Company.CompanyId={6} " + str4 + str1 + str3 + " order by" + str2 + str5, (object) Options.Complex.ComplexId, (object) this.home.IdHome, this.cmbBase.SelectedValue, this.cmbService.SelectedValue, (object) num1, (object) KvrplHelper.DateToBaseFormat(this.monthClosed.PeriodName.Value.AddMonths(1)), (object) this.company.CompanyId)).List<CounterScheme>();
      if ((int) this.level == 2)
        counterSchemeList = this.session.CreateQuery(string.Format("select cs from CounterScheme cs join fetch cs.Counter s left join fetch s.Service left join fetch s.TypeCounter left join fetch s.Home h left join fetch h.Str st where cs.Period.PeriodId={4} and s.Complex.ComplexId={0} and s.Company.CompanyId={1} and s.BaseCounter.Id={2} " + str4 + str1 + str3 + " order by" + str2 + str5, (object) Options.Complex.ComplexId, (object) this.company.CompanyId, this.cmbBase.SelectedValue, this.cmbService.SelectedValue, (object) num1, (object) KvrplHelper.DateToBaseFormat(this.monthClosed.PeriodName.Value.AddMonths(1)))).List<CounterScheme>();
      if (this.pastTime && this.cbArchive.Checked)
      {
        int num2 = 0;
        if ((int) this.level == 3)
          counterSchemeList = this.session.CreateQuery(string.Format("select cs from CounterScheme cs join fetch cs.Counter s left join fetch s.Service left join fetch s.TypeCounter where cs.Period.PeriodId!={4} and s.Complex.ComplexId={0} and s.Home.IdHome={1} and s.BaseCounter.Id={2} and s.Company.CompanyId={6} " + str4 + str1 + str3 + " order by" + str2 + str5, (object) Options.Complex.ComplexId, (object) this.home.IdHome, this.cmbBase.SelectedValue, this.cmbService.SelectedValue, (object) num2, (object) KvrplHelper.DateToBaseFormat(this.monthClosed.PeriodName.Value.AddMonths(1)), (object) this.company.CompanyId)).List<CounterScheme>();
        if ((int) this.level == 2)
          counterSchemeList = this.session.CreateQuery(string.Format("select cs from CounterScheme cs join fetch cs.Counter s left join fetch s.Service left join fetch s.TypeCounter left join fetch s.Home h left join fetch h.Str st where cs.Period.PeriodId!={4} and s.Complex.ComplexId={0} and s.Company.CompanyId={1} and s.BaseCounter.Id={2} " + str4 + str1 + str3 + " order by" + str2 + str5, (object) Options.Complex.ComplexId, (object) this.company.CompanyId, this.cmbBase.SelectedValue, this.cmbService.SelectedValue, (object) num2, (object) KvrplHelper.DateToBaseFormat(this.monthClosed.PeriodName.Value.AddMonths(1)))).List<CounterScheme>();
      }
      this.dgvScheme.Columns.Clear();
      this.dgvScheme.DataSource = (object) null;
      this.dgvScheme.DataSource = (object) counterSchemeList;
      this.session.Clear();
      this.SetViewScheme();
      this.MySettingsScheme.GridName = "Scheme";
      this.LoadSettingsScheme();
      this.Cursor = Cursors.Default;
    }

    private void LoadSettingsScheme()
    {
      this.MySettingsScheme.Load();
      foreach (DataGridViewColumn column in (BaseCollection) this.dgvScheme.Columns)
        this.MySettingsScheme.GetMySettings(column);
    }

    private void dgvScheme_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      if (this.MySettingsScheme.FindByName(e.Column.Name) < 0)
        return;
      this.MySettingsScheme.Columns[this.MySettingsScheme.FindByName(e.Column.Name)].Width = e.Column.Width;
      this.MySettingsScheme.Save();
    }

    private void SetViewScheme()
    {
      KvrplHelper.AddMaskDateColumn(this.dgvScheme, 0, "Дата начала", "DBeg");
      KvrplHelper.AddMaskDateColumn(this.dgvScheme, 1, "Дата окончания", "DEnd");
      KvrplHelper.AddComboBoxColumn(this.dgvScheme, 0, (IList) null, (string) null, (string) null, "Номер счетчика", "Num", 100, 100);
      KvrplHelper.AddButtonColumn(this.dgvScheme, 3, "Схема", "Scheme", 100);
      KvrplHelper.AddButtonColumn(this.dgvScheme, 4, "Признак ОДН", "SchemeODN", 100);
      KvrplHelper.ViewEdit(this.dgvScheme);
      DataGridViewComboBoxColumn viewComboBoxColumn = new DataGridViewComboBoxColumn();
      if ((uint) Convert.ToInt16(this.cmbService.SelectedValue) > 0U)
      {
        this.dgvEvidence.Columns["ServiceName"].Visible = false;
        this.uslovie = "and c.Service.ServiceId={3}";
      }
      else
        this.uslovie = "";
      if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
      {
        this.clients = (IList<LsClient>) new List<LsClient>();
        if ((int) this.level == 2)
          this.clients = this.session.CreateQuery(string.Format("select distinct ls from LsClient ls,Counter c,Flat f where c.LsClient=ls and ls.Company.CompanyId = {0} and (c.ArchivesDate is null or c.ArchivesDate > '{1}') and ls.Flat=f and c.BaseCounter.Id={2} " + this.uslovie, (object) this.company.CompanyId, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), this.cmbBase.SelectedValue, (object) Convert.ToInt16(this.cmbService.SelectedValue))).List<LsClient>();
        if ((int) this.level == 3)
          this.clients = this.session.CreateQuery(string.Format("select distinct ls from LsClient ls,Counter c,Flat f where c.LsClient=ls and ls.Home.IdHome = {0} and (c.ArchivesDate is null or c.ArchivesDate > '{1}') and ls.Flat=f and c.BaseCounter.Id={2} " + this.uslovie, (object) this.home.IdHome, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), this.cmbBase.SelectedValue, (object) Convert.ToInt16(this.cmbService.SelectedValue))).List<LsClient>();
        viewComboBoxColumn.DataSource = (object) this.clients;
        viewComboBoxColumn.ValueMember = "ClientId";
        viewComboBoxColumn.DisplayMember = "SmallAddress";
        viewComboBoxColumn.HeaderText = "Адрес";
        viewComboBoxColumn.Name = "LicAddress";
        this.dgvScheme.Columns.Insert(0, (DataGridViewColumn) viewComboBoxColumn);
      }
      if ((Convert.ToInt32(this.cmbBase.SelectedValue) != 2 || (int) this.level == 2) && Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
      {
        this.homes = (IList<Home>) new List<Home>();
        if ((int) this.level == 2)
          this.homes = this.session.CreateQuery(string.Format("select distinct h from Home h left join fetch h.Str, HomeLink hl,Counter c where c.Home=h and hl.Home=h and hl.Company.CompanyId={0} and c.BaseCounter.Id={2} " + this.uslovie + " order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)", (object) this.company.CompanyId, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), this.cmbBase.SelectedValue, (object) Convert.ToInt16(this.cmbService.SelectedValue))).List<Home>();
        if ((int) this.level == 3)
          this.homes = this.session.CreateQuery(string.Format("select distinct h from Home h left join fetch h.Str, HomeLink hl,Counter c where c.Home=h and c.Home.IdHome={4} and c.BaseCounter.Id={2} and hl.Home=h and hl.Company.CompanyId={0} " + this.uslovie + " order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)", (object) this.company.CompanyId, (object) KvrplHelper.DateToBaseFormat(Options.Period.PeriodName.Value), this.cmbBase.SelectedValue, (object) Convert.ToInt16(this.cmbService.SelectedValue), (object) this.home.IdHome)).List<Home>();
        KvrplHelper.AddComboBoxColumn(this.dgvScheme, 0, (IList) this.homes, "IdHome", "Address", "Адрес", "Address", 180, 180);
      }
      IList<LsClient> lsClientList = (IList<LsClient>) new List<LsClient>();
      IList<Counter> counterList1 = (IList<Counter>) new List<Counter>();
      foreach (DataGridViewRow row in (IEnumerable) this.dgvScheme.Rows)
      {
        row.Cells["DBeg"].Value = (object) ((CounterScheme) row.DataBoundItem).DBeg;
        row.Cells["DEnd"].Value = (object) ((CounterScheme) row.DataBoundItem).DEnd;
        row.Cells["Scheme"].Value = (object) ((CounterScheme) row.DataBoundItem).Scheme;
        row.Cells["SchemeODN"].Value = (object) ((CounterScheme) row.DataBoundItem).SchemeODN;
        IList<Counter> counterList2;
        if (((int) this.level == 2 || (int) this.level == 3 && Convert.ToInt32(this.cmbBase.SelectedValue) != 2) && ((CounterScheme) row.DataBoundItem).Home != null)
        {
          if (Convert.ToInt32(this.cmbBase.SelectedValue) != 2)
          {
            row.Cells["Address"].Value = (object) ((CounterScheme) row.DataBoundItem).Home.IdHome;
            if (counterList1.Count == 0 || counterList1[0].Home.IdHome != ((CounterScheme) row.DataBoundItem).Home.IdHome)
            {
              counterList2 = (IList<Counter>) new List<Counter>();
              counterList1 = (uint) Convert.ToInt16(this.cmbService.SelectedValue) <= 0U ? this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Home.IdHome", (object) ((CounterScheme) row.DataBoundItem).Home.IdHome)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Company.CompanyId", (object) this.company.CompanyId)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>() : this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Home.IdHome", (object) ((CounterScheme) row.DataBoundItem).Home.IdHome)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Company.CompanyId", (object) this.company.CompanyId)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) (Service) this.cmbService.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>();
            }
            row.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
            {
              DisplayStyleForCurrentCellOnly = true,
              ValueMember = "CounterId",
              DisplayMember = "AllInfo",
              DataSource = (object) counterList1
            };
            row.Cells["Num"].Value = (object) ((CounterScheme) row.DataBoundItem).Counter.CounterId;
          }
          else
            row.Cells["Address"].Value = (object) ((CounterScheme) row.DataBoundItem).Home.IdHome;
        }
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 4)
        {
          if (counterList1.Count == 0)
          {
            counterList2 = (IList<Counter>) new List<Counter>();
            counterList1 = (uint) Convert.ToInt16(this.cmbService.SelectedValue) <= 0U ? this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Company.CompanyId", (object) this.company.CompanyId)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>() : this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Company.CompanyId", (object) this.company.CompanyId)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) (Service) this.cmbService.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Complex", (object) Options.Complex)).List<Counter>();
          }
          row.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
          {
            DisplayStyleForCurrentCellOnly = true,
            ValueMember = "CounterId",
            DisplayMember = "AllInfo",
            DataSource = (object) counterList1
          };
          if (((CounterScheme) row.DataBoundItem).Counter != null)
            row.Cells["Num"].Value = (object) ((CounterScheme) row.DataBoundItem).Counter.CounterId;
        }
      }
    }

    private void dgvScheme_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (!this.dgvScheme.IsCurrentCellDirty)
        return;
      this.dgvScheme.CommitEdit(DataGridViewDataErrorContexts.Commit);
      if ((Convert.ToInt32(this.cmbBase.SelectedValue) == 1 || Convert.ToInt32(this.cmbBase.SelectedValue) == 3) && (int) Convert.ToInt16(this.cmbService.SelectedValue) != 0 && this.dgvScheme.CurrentCell.ColumnIndex == this.dgvScheme.Rows[this.dgvScheme.CurrentRow.Index].Cells["Address"].ColumnIndex)
      {
        this.session = Domain.CurrentSession;
        IList<Counter> counterList = (IList<Counter>) new List<Counter>();
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 1 || Convert.ToInt32(this.cmbBase.SelectedValue) == 3)
          counterList = this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Home.IdHome", this.dgvScheme.CurrentRow.Cells["Address"].Value)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Company.CompanyId", (object) this.company.CompanyId)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Service", (object) (Service) this.cmbService.SelectedItem)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).List<Counter>();
        this.dgvScheme.CurrentRow.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "CounterId",
          DisplayMember = "AllInfo",
          DataSource = (object) counterList
        };
      }
      if ((int) Convert.ToInt16(this.cmbService.SelectedValue) == 0 && Convert.ToInt32(this.cmbBase.SelectedValue) != 2 && Convert.ToInt32(this.cmbBase.SelectedValue) != 4 && this.dgvScheme.CurrentCell.ColumnIndex == this.dgvScheme.Rows[this.dgvScheme.CurrentRow.Index].Cells["Address"].ColumnIndex)
      {
        this.session = Domain.CurrentSession;
        IList<Counter> counterList = (IList<Counter>) new List<Counter>();
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
          counterList = this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("LsClient.ClientId", this.dgvScheme.CurrentRow.Cells["LicAddress"].Value)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Company.CompanyId", (object) this.company.CompanyId)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).List<Counter>();
        if (Convert.ToInt32(this.cmbBase.SelectedValue) == 1 || Convert.ToInt32(this.cmbBase.SelectedValue) == 3)
          counterList = this.session.CreateCriteria(typeof (Counter)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Home.IdHome", this.dgvScheme.CurrentRow.Cells["Address"].Value)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("Company.CompanyId", (object) this.company.CompanyId)).Add((ICriterion) NHibernate.Criterion.Restrictions.Eq("BaseCounter", (object) (BaseCounter) this.cmbBase.SelectedItem)).List<Counter>();
        this.dgvScheme.CurrentRow.Cells["Num"] = (DataGridViewCell) new DataGridViewComboBoxCell()
        {
          DisplayStyleForCurrentCellOnly = true,
          ValueMember = "CounterId",
          DisplayMember = "AllInfo",
          DataSource = (object) counterList
        };
      }
    }

    private void dgvScheme_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex > 0 && e.RowIndex >= 0 && this.dgvScheme.Columns[e.ColumnIndex].Name == "Scheme")
      {
        short id = ((CounterScheme) this.dgvScheme.CurrentRow.DataBoundItem).Scheme;
        FrmScheme frmScheme = new FrmScheme((short) 7, id);
        if (frmScheme.ShowDialog() == DialogResult.OK)
          id = frmScheme.CurrentId();
        this.dgvScheme.CurrentRow.Cells["Scheme"].Value = (object) id;
        frmScheme.Dispose();
        this.btnAdd.Enabled = false;
        this.btnSave.Enabled = true;
        this.btnDelete.Enabled = false;
      }
      if (e.ColumnIndex <= 0 || e.RowIndex < 0 || !(this.dgvScheme.Columns[e.ColumnIndex].Name == "SchemeODN"))
        return;
      short id1 = ((CounterScheme) this.dgvScheme.CurrentRow.DataBoundItem).SchemeODN;
      FrmScheme frmScheme1 = new FrmScheme((short) 10, id1);
      if (frmScheme1.ShowDialog() == DialogResult.OK)
        id1 = frmScheme1.CurrentId();
      this.dgvScheme.CurrentRow.Cells["SchemeODN"].Value = (object) id1;
      frmScheme1.Dispose();
      this.btnAdd.Enabled = false;
      this.btnSave.Enabled = true;
      this.btnDelete.Enabled = false;
    }

    private void InsertScheme()
    {
      this.insertRecord = true;
      CounterScheme counterScheme1 = new CounterScheme();
      if (!this.pastTime)
      {
        DateTime dateTime1 = Options.Period.PeriodName.Value;
        DateTime? periodName = this.monthClosed.PeriodName;
        DateTime dateTime2 = periodName.Value;
        if (dateTime1 <= dateTime2)
        {
          CounterScheme counterScheme2 = counterScheme1;
          periodName = this.monthClosed.PeriodName;
          DateTime dateTime3 = periodName.Value.AddMonths(1);
          counterScheme2.DBeg = dateTime3;
        }
        else
        {
          CounterScheme counterScheme2 = counterScheme1;
          periodName = Options.Period.PeriodName;
          DateTime dateTime3 = periodName.Value;
          counterScheme2.DBeg = dateTime3;
        }
        counterScheme1.DEnd = Convert.ToDateTime("31.12.2999");
      }
      else
      {
        counterScheme1.DBeg = this.monthClosed.PeriodName.Value;
        counterScheme1.DEnd = counterScheme1.DBeg.AddMonths(1).AddDays(-1.0);
      }
      counterScheme1.Scheme = (short) 0;
      IList<CounterScheme> counterSchemeList = (IList<CounterScheme>) new List<CounterScheme>();
      if ((uint) this.dgvScheme.Rows.Count > 0U)
        counterSchemeList = (IList<CounterScheme>) (this.dgvScheme.DataSource as List<CounterScheme>);
      counterSchemeList.Add(counterScheme1);
      this.dgvScheme.Columns.Clear();
      this.dgvScheme.DataSource = (object) null;
      this.dgvScheme.DataSource = (object) counterSchemeList;
      this.SetViewScheme();
      this.LoadSettingsScheme();
      this.dgvScheme.CurrentCell = this.dgvScheme.Rows[this.dgvScheme.Rows.Count - 1].Cells[0];
    }

    private bool SaveScheme()
    {
      if (this.dgvScheme.Rows.Count > 0 && this.dgvScheme.CurrentRow != null)
      {
        CounterScheme dataBoundItem = (CounterScheme) this.dgvScheme.CurrentRow.DataBoundItem;
        CounterScheme counterScheme1 = new CounterScheme();
        if (!this.insertRecord)
        {
          IList<CounterScheme> counterSchemeList = (IList<CounterScheme>) new List<CounterScheme>();
          string str1 = "";
          string str2 = "";
          string str3 = "";
          int num = 0;
          if (this.pastTime)
            num = Options.Period.PeriodId;
          if ((uint) Convert.ToInt16(this.cmbService.SelectedValue) > 0U)
            str1 = " and s.Service.ServiceId={3}";
          if (this.tcntrlCounters.SelectedTab == this.tpCounters)
            str2 = " isnull(s.ArchivesDate,'2999-12-31') desc,";
          if ((int) this.level == 2)
            str2 += " st.NameStr,DBA.lengthhome(s.Home.NHome),s.Home.HomeKorp,";
          if (Convert.ToInt32(this.cmbBase.SelectedValue) == 2)
            str2 += " DBA.lengthhome(s.LsClient.Flat.NFlat),";
          string str4 = !(Options.SortService == " s.ServiceId") ? " s.Service.ServiceName" : " s.Service.ServiceId";
          if (!this.cbArchive.Checked && !this.pastTime)
            str3 = " and cs.DEnd>='{5}'";
          if ((int) this.level == 3)
            counterSchemeList = this.session.CreateQuery(string.Format("select cs from CounterScheme cs join fetch cs.Counter s left join fetch s.Service left join fetch s.TypeCounter where cs.Period.PeriodId={4} and s.Complex.ComplexId={0} and s.Home.IdHome={1} and s.BaseCounter.Id={2} and s.Company.CompanyId={6} " + str1 + str3 + " order by" + str2 + str4, (object) Options.Complex.ComplexId, (object) this.home.IdHome, this.cmbBase.SelectedValue, this.cmbService.SelectedValue, (object) num, (object) KvrplHelper.DateToBaseFormat(this.monthClosed.PeriodName.Value.AddMonths(1)), (object) this.company.CompanyId)).List<CounterScheme>();
          if ((int) this.level == 2)
            counterSchemeList = this.session.CreateQuery(string.Format("select cs from CounterScheme cs join fetch cs.Counter s left join fetch s.Service left join fetch s.TypeCounter left join fetch s.Home h left join fetch h.Str st where cs.Period.PeriodId={4} and s.Complex.ComplexId={0} and s.Company.CompanyId={1} and s.BaseCounter.Id={2} " + str1 + str3 + " order by" + str2 + str4, (object) Options.Complex.ComplexId, (object) this.company.CompanyId, this.cmbBase.SelectedValue, this.cmbService.SelectedValue, (object) num, (object) KvrplHelper.DateToBaseFormat(this.monthClosed.PeriodName.Value.AddMonths(1)))).List<CounterScheme>();
          counterScheme1 = counterSchemeList[this.dgvScheme.CurrentRow.Index];
        }
        if (this.dgvScheme.CurrentRow.Cells["DBeg"].Value != null)
        {
          try
          {
            dataBoundItem.DBeg = Convert.ToDateTime(this.dgvScheme.CurrentRow.Cells["DBeg"].Value);
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Дата начала введена некорректно", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
          if (this.dgvScheme.CurrentRow.Cells["DEnd"].Value != null)
          {
            try
            {
              dataBoundItem.DEnd = Convert.ToDateTime(this.dgvScheme.CurrentRow.Cells["DEnd"].Value);
            }
            catch (Exception ex)
            {
              int num = (int) MessageBox.Show("Дата окончания введена некорректно", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
            if (dataBoundItem.DBeg > dataBoundItem.DEnd)
            {
              int num = (int) MessageBox.Show("Дата начала больше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
            dataBoundItem.Counter = this.session.Get<Counter>(this.dgvScheme.CurrentRow.Cells["Num"].Value);
            DateTime dateTime1;
            if (!this.pastTime)
            {
              dateTime1 = this.monthClosed.PeriodName.Value;
              dateTime1 = dateTime1.AddMonths(1);
              DateTime dateTime2 = dateTime1.AddDays(-1.0);
              if (this.insertRecord && (dataBoundItem.DBeg <= dateTime2 || dataBoundItem.DEnd <= dateTime2))
              {
                int num = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
              }
              if (!this.insertRecord && (counterScheme1.DBeg <= dateTime2 && counterScheme1.DEnd < dateTime2 || counterScheme1.DEnd < dateTime2 || counterScheme1.DBeg > dateTime2 && dataBoundItem.DBeg <= dateTime2 || counterScheme1.DBeg <= dateTime2 && (counterScheme1.DBeg != dataBoundItem.DBeg || counterScheme1.Counter.CounterId != dataBoundItem.Counter.CounterId)))
              {
                int num = (int) MessageBox.Show("Не могу сохранить текущую запись, т.к. она принадлежит закрытому периоду", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
              }
            }
            DateTime? periodName;
            int num1;
            if (this.pastTime)
            {
              DateTime dend = dataBoundItem.DEnd;
              periodName = this.monthClosed.PeriodName;
              dateTime1 = periodName.Value;
              DateTime dateTime2 = dateTime1.AddMonths(1);
              num1 = dend >= dateTime2 ? 1 : 0;
            }
            else
              num1 = 0;
            if (num1 != 0)
            {
              int num2 = (int) MessageBox.Show("Невозможно внести запись в открытом периоде", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
            DateTime dbeg1 = dataBoundItem.DBeg;
            dateTime1 = DateTime.Now;
            DateTime dateTime3 = dateTime1.AddYears(-3);
            int num3;
            if (!(dbeg1 <= dateTime3))
            {
              DateTime dbeg2 = dataBoundItem.DBeg;
              dateTime1 = DateTime.Now;
              DateTime dateTime2 = dateTime1.AddYears(3);
              num3 = dbeg2 >= dateTime2 ? 1 : 0;
            }
            else
              num3 = 1;
            if (num3 != 0 && MessageBox.Show("Дата начала отличается от текущей более, чем на 3 года. Продолжить", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
              return false;
            if (this.dgvScheme.CurrentRow.Cells["Num"].Value == null)
            {
              int num2 = (int) MessageBox.Show("Не выбран счетчик!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
            dataBoundItem.Scheme = Convert.ToInt16(this.dgvScheme.CurrentRow.Cells["Scheme"].Value);
            dataBoundItem.SchemeODN = Convert.ToInt16(this.dgvScheme.CurrentRow.Cells["SchemeODN"].Value);
            dataBoundItem.Period = this.pastTime ? Options.Period : this.session.Get<Period>((object) 0);
            dataBoundItem.UName = Options.Login;
            CounterScheme counterScheme2 = dataBoundItem;
            dateTime1 = DateTime.Now;
            DateTime date = dateTime1.Date;
            counterScheme2.DEdit = date;
            int num4;
            if (!this.pastTime)
            {
              DateTime dbeg2 = dataBoundItem.DBeg;
              periodName = this.monthClosed.PeriodName;
              dateTime1 = periodName.Value;
              DateTime dateTime2 = dateTime1.AddMonths(1);
              num4 = dbeg2 < dateTime2 ? 1 : 0;
            }
            else
              num4 = 0;
            if (num4 != 0)
            {
              int num2;
              if (!this.insertRecord)
              {
                if (!this.insertRecord)
                {
                  if (!(dataBoundItem.DBeg != counterScheme1.DBeg) && dataBoundItem.Counter.CounterId == counterScheme1.Counter.CounterId && (int) dataBoundItem.Scheme == (int) counterScheme1.Scheme)
                  {
                    DateTime dend = dataBoundItem.DEnd;
                    periodName = this.monthClosed.PeriodName;
                    dateTime1 = periodName.Value;
                    dateTime1 = dateTime1.AddMonths(1);
                    DateTime dateTime2 = dateTime1.AddDays(-1.0);
                    num2 = dend < dateTime2 ? 1 : 0;
                  }
                  else
                    num2 = 1;
                }
                else
                  num2 = 0;
              }
              else
                num2 = 1;
              if (num2 != 0)
              {
                int num5 = (int) MessageBox.Show("Невозможно внести изменения в закрытом периоде", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
              }
            }
            try
            {
              if (this.insertRecord)
              {
                this.insertRecord = false;
                this.session.Save((object) dataBoundItem);
                this.session.Flush();
              }
              else
                this.session.CreateQuery("update CounterScheme set DBeg=:dbeg,DEnd=:dend,Counter.CounterId=:counter,Scheme=:scheme,SchemeODN=:schemeodn,UName=:uname,DEdit=:dedit where Period.PeriodId=:period and Counter.CounterId=:oldcounter and DBeg=:olddbeg").SetParameter<DateTime>("dbeg", dataBoundItem.DBeg).SetParameter<DateTime>("dend", dataBoundItem.DEnd).SetParameter<int>("counter", dataBoundItem.Counter.CounterId).SetParameter<short>("scheme", dataBoundItem.Scheme).SetParameter<short>("schemeodn", dataBoundItem.SchemeODN).SetParameter<string>("uname", dataBoundItem.UName).SetParameter<DateTime>("dedit", dataBoundItem.DEdit).SetParameter<int>("period", counterScheme1.Period.PeriodId).SetParameter<int>("oldcounter", counterScheme1.Counter.CounterId).SetParameter<DateTime>("olddbeg", counterScheme1.DBeg).ExecuteUpdate();
            }
            catch (Exception ex)
            {
              int num2 = (int) MessageBox.Show("Невозможно сохранить изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              KvrplHelper.WriteLog(ex, (LsClient) null);
            }
          }
          else
          {
            int num = (int) MessageBox.Show("Дата окончания не введена", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
        }
        else
        {
          int num = (int) MessageBox.Show("Дата начала не введена", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
      return true;
    }

    private void DeleteScheme()
    {
      if (this.dgvScheme.Rows.Count <= 0 || this.dgvScheme.CurrentRow == null || MessageBox.Show("Удалить запись?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      this.session.Clear();
      this.session = Domain.CurrentSession;
      CounterScheme dataBoundItem = (CounterScheme) this.dgvScheme.CurrentRow.DataBoundItem;
      try
      {
        this.session.Delete((object) dataBoundItem);
        this.session.Flush();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Невозможно удалить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      this.session.Clear();
    }

    private void dgvScheme_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (this.dgvScheme.DataSource == null)
        return;
      DataGridViewRow row = (sender as DataGridView).Rows[e.RowIndex];
      DateTime dbeg = ((CounterScheme) row.DataBoundItem).DBeg;
      DateTime? periodName = this.monthClosed.PeriodName;
      DateTime dateTime1 = periodName.Value;
      DateTime dateTime2 = KvrplHelper.LastDay(dateTime1.AddMonths(1));
      int num;
      if (dbeg <= dateTime2)
      {
        DateTime dend = ((CounterScheme) row.DataBoundItem).DEnd;
        periodName = this.monthClosed.PeriodName;
        dateTime1 = periodName.Value;
        DateTime dateTime3 = dateTime1.AddMonths(1);
        num = dend >= dateTime3 ? 1 : 0;
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

    private void InsertWorkDistribute()
    {
      this.insertRecord = true;
      hmWorkDistribute hmWorkDistribute = new hmWorkDistribute();
      hmWorkDistribute.WorkDistribute = (int) this.session.CreateSQLQuery("select DBA.gen_id('hmWorkDistribute',1)").UniqueResult();
      hmWorkDistribute.Period = Options.Period;
      IList<hmWorkDistribute> hmWorkDistributeList = (IList<hmWorkDistribute>) new List<hmWorkDistribute>();
      if ((uint) this.dgvWorkDistribute.Rows.Count > 0U)
        hmWorkDistributeList = (IList<hmWorkDistribute>) (this.dgvWorkDistribute.DataSource as List<hmWorkDistribute>);
      hmWorkDistributeList.Add(hmWorkDistribute);
      this.dgvWorkDistribute.Columns.Clear();
      this.dgvWorkDistribute.DataSource = (object) null;
      this.dgvWorkDistribute.DataSource = (object) hmWorkDistributeList;
      this.SetViewWorkDistribute();
      this.dgvWorkDistribute.CurrentCell = this.dgvWorkDistribute.Rows[this.dgvWorkDistribute.Rows.Count - 1].Cells[1];
      this.dgvWorkDistribute.Columns["Services"].ReadOnly = false;
    }

    private void SaveWorkDistribute()
    {
      if (this.dgvWorkDistribute.Rows.Count > 0 && this.dgvWorkDistribute.CurrentRow != null)
      {
        this.session.Clear();
        this.session = Domain.CurrentSession;
        hmWorkDistribute dataBoundItem = (hmWorkDistribute) this.dgvWorkDistribute.CurrentRow.DataBoundItem;
        dataBoundItem.Company = this.company;
        if (dataBoundItem.Period.PeriodId <= this.monthClosed.PeriodId)
        {
          int num = (int) MessageBox.Show("Невозможно внести изменения в закрытом периоде", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        if (Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
        {
          if (this.dgvWorkDistribute.CurrentRow.Cells["Address"].Value != null)
          {
            dataBoundItem.Home = this.session.Get<Home>(this.dgvWorkDistribute.CurrentRow.Cells["Address"].Value);
          }
          else
          {
            int num = (int) MessageBox.Show("Введите адрес", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        if (this.dgvWorkDistribute.CurrentRow.Cells["Rent"].Value != null)
        {
          try
          {
            dataBoundItem.Rent = Convert.ToDecimal(KvrplHelper.ChangeSeparator(this.dgvWorkDistribute.CurrentRow.Cells["Rent"].Value.ToString()));
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Некорректный формат суммы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          if (this.dgvWorkDistribute.CurrentRow.Cells["Period"].Value != null)
          {
            try
            {
              dataBoundItem.Period = Options.Period;
            }
            catch (Exception ex)
            {
              int num = (int) MessageBox.Show("Некорректный формат месяца", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
          }
          else
            dataBoundItem.Period = Options.Period;
          if (this.dgvWorkDistribute.CurrentRow.Cells["Services"].Value != null && (uint) Convert.ToInt32(this.dgvWorkDistribute.CurrentRow.Cells["Services"].Value) > 0U)
          {
            if ((uint) this.session.CreateQuery("from lsWorkDistribute ls where ls.WorkDistribute=:wd").SetParameter<hmWorkDistribute>("wd", dataBoundItem).List<lsWorkDistribute>().Count > 0U)
            {
              int num = (int) MessageBox.Show("Для прибора уже занесены лицевые счета. Изменения Услуги приведет к ошибке - несогласованность данных о поставщике.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return;
            }
            dataBoundItem.Service = this.session.Get<Service>(this.dgvWorkDistribute.CurrentRow.Cells["Services"].Value);
            dataBoundItem.Scheme = (int) Convert.ToInt16(this.dgvWorkDistribute.CurrentRow.Cells["Scheme"].Value);
            IList<hmWorkDistribute> hmWorkDistributeList1 = (IList<hmWorkDistribute>) new List<hmWorkDistribute>();
            IList<hmWorkDistribute> hmWorkDistributeList2;
            if (Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
              hmWorkDistributeList2 = this.session.CreateQuery(string.Format("from hmWorkDistribute where Company.CompanyId={0} and Home.IdHome={1} and Period.PeriodId={2} and Service.ServiceId={3} and WorkDistribute<>{4}", (object) dataBoundItem.Company.CompanyId, (object) dataBoundItem.Home.IdHome, (object) dataBoundItem.Period.PeriodId, (object) dataBoundItem.Service.ServiceId, (object) dataBoundItem.WorkDistribute)).List<hmWorkDistribute>();
            else
              hmWorkDistributeList2 = this.session.CreateQuery(string.Format("from hmWorkDistribute where Company.CompanyId={0} and Period.PeriodId={1} and Service.ServiceId={2} and WorkDistribute<>{3}", (object) dataBoundItem.Company.CompanyId, (object) dataBoundItem.Period.PeriodId, (object) dataBoundItem.Service.ServiceId, (object) dataBoundItem.WorkDistribute)).List<hmWorkDistribute>();
            if (hmWorkDistributeList2.Count > 0 && MessageBox.Show("Уже существует запись по этому счетчику за выбраный месяц. Записи будут объединены. Все равно продолжить?", "Внимание!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
              this.insertRecord = false;
              return;
            }
            dataBoundItem.Uname = Options.Login;
            dataBoundItem.Dedit = new DateTime?(DateTime.Now.Date);
            this.session.Clear();
            try
            {
              if (this.insertRecord)
              {
                this.insertRecord = false;
                this.session.Save((object) dataBoundItem);
              }
              else
                this.session.Update((object) dataBoundItem);
              this.session.Flush();
            }
            catch (Exception ex)
            {
              int num = (int) MessageBox.Show("Невозможно сохранить изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              KvrplHelper.WriteLog(ex, (LsClient) null);
            }
          }
          else
          {
            int num = (int) MessageBox.Show("Выберите услугу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        else
        {
          int num = (int) MessageBox.Show("Введите сумму", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
      }
      this.session.Clear();
    }

    private void LoadWorkDistribute()
    {
      IList<hmWorkDistribute> hmWorkDistributeList = (IList<hmWorkDistribute>) new List<hmWorkDistribute>();
      IList<hmWorkDistribute> source = (int) this.level != 3 ? this.session.CreateQuery("select w from hmWorkDistribute w where w.Company=:company ").SetParameter<Company>("company", this.company).List<hmWorkDistribute>() : this.session.CreateQuery("select w from hmWorkDistribute w where w.Company=:company and w.Home=:hom").SetParameter<Company>("company", this.company).SetParameter<Home>("hom", this.home).List<hmWorkDistribute>();
      if ((uint) Convert.ToInt32(this.cmbService.SelectedValue) > 0U)
        source = (IList<hmWorkDistribute>) source.Where<hmWorkDistribute>((Func<hmWorkDistribute, bool>) (hmWD => hmWD.Service.Equals((object) (Service) this.cmbService.SelectedItem))).ToList<hmWorkDistribute>();
      this.dgvWorkDistribute.Columns.Clear();
      this.dgvWorkDistribute.DataSource = (object) null;
      this.dgvWorkDistribute.DataSource = (object) source;
      this.SetViewWorkDistribute();
    }

    private void SetViewWorkDistribute()
    {
      this.session = Domain.CurrentSession;
      this.dgvWorkDistribute.Columns["WorkDistribute"].HeaderText = "Код записи";
      this.dgvWorkDistribute.Columns["WorkDistribute"].Visible = false;
      this.dgvWorkDistribute.Columns["Company"].Visible = false;
      this.dgvWorkDistribute.Columns["Home"].Visible = false;
      this.dgvWorkDistribute.Columns["Period"].Visible = false;
      this.dgvWorkDistribute.Columns["Service"].Visible = false;
      this.dgvWorkDistribute.Columns["Scheme"].HeaderText = "Схема";
      this.dgvWorkDistribute.Columns["Period"].HeaderText = "Период";
      this.dgvWorkDistribute.Columns["Rent"].HeaderText = "Сумма";
      this.dgvWorkDistribute.Columns["ParamValue"].HeaderText = "Сумма параметров";
      this.dgvWorkDistribute.Columns["ParamValue"].ReadOnly = true;
      this.dgvWorkDistribute.Columns["Scheme"].ReadOnly = true;
      KvrplHelper.AddMaskDateColumn(this.dgvWorkDistribute, 0, "Период", "Periods");
      KvrplHelper.ViewEdit(this.dgvWorkDistribute);
      KvrplHelper.AddComboBoxColumn(this.dgvWorkDistribute, 1, (IList) null, "IdHome", "Address", "Адрес", "Address", 180, 180);
      KvrplHelper.AddComboBoxColumn(this.dgvWorkDistribute, 3, (IList) null, "ServiceId", "Services", "Услуга", "Services", 180, 180);
      this.dgvWorkDistribute.Columns["Periods"].ReadOnly = true;
      if ((uint) Convert.ToInt32(this.cmbService.SelectedValue) > 0U)
        this.dgvWorkDistribute.Columns["Services"].ReadOnly = true;
      foreach (DataGridViewRow row in (IEnumerable) this.dgvWorkDistribute.Rows)
      {
        DataGridViewComboBoxCell viewComboBoxCell1 = new DataGridViewComboBoxCell();
        viewComboBoxCell1.DisplayStyleForCurrentCellOnly = true;
        viewComboBoxCell1.ValueMember = "IdHome";
        viewComboBoxCell1.DisplayMember = "Address";
        IList<Home> homeList = (IList<Home>) new List<Home>();
        if ((int) this.level == 2)
          homeList = this.session.CreateQuery(string.Format("select h from Home h left join fetch h.Str, HomeLink hl where hl.Home=h and hl.Company.CompanyId={0} order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome) ", (object) this.company.CompanyId)).List<Home>();
        if ((int) this.level == 3)
          homeList = this.session.CreateQuery(string.Format("select h from Home h left join fetch h.Str, HomeLink hl where hl.Home=h and hl.Company.CompanyId={0} and hl.Home.IdHome={1}  order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome) ", (object) this.company.CompanyId, (object) this.home.IdHome)).List<Home>();
        viewComboBoxCell1.DataSource = (object) homeList;
        row.Cells["Address"] = (DataGridViewCell) viewComboBoxCell1;
        if (((hmWorkDistribute) row.DataBoundItem).Home != null)
          row.Cells["Address"].Value = (object) ((hmWorkDistribute) row.DataBoundItem).Home.IdHome;
        DataGridViewComboBoxCell viewComboBoxCell2 = new DataGridViewComboBoxCell();
        viewComboBoxCell2.DisplayStyleForCurrentCellOnly = true;
        viewComboBoxCell2.ValueMember = "ServiceId";
        viewComboBoxCell2.DisplayMember = "ServiceName";
        IList<Service> serviceList1 = (IList<Service>) new List<Service>();
        IList<Service> serviceList2 = this.session.CreateQuery(string.Format("select distinct s from Service s, ServiceParam sp where sp.Service_id=s.ServiceId and sp.SpecialId=2 and s.Root=0 and s.ServiceId<>0 and sp.Company_id={0} order by " + Options.SortService, (object) this.company.CompanyId)).List<Service>();
        viewComboBoxCell2.DataSource = (object) serviceList2;
        row.Cells["Services"] = (DataGridViewCell) viewComboBoxCell2;
        if (((hmWorkDistribute) row.DataBoundItem).Service != null)
          row.Cells["Services"].Value = (object) ((hmWorkDistribute) row.DataBoundItem).Service.ServiceId;
        int scheme = ((hmWorkDistribute) row.DataBoundItem).Scheme;
        if (true)
          row.Cells["Scheme"].Value = (object) ((hmWorkDistribute) row.DataBoundItem).Scheme;
        row.Cells["Periods"].Value = (object) ((hmWorkDistribute) row.DataBoundItem).Period.PeriodName.Value;
      }
    }

    private void DeleteWorkDitribute()
    {
      if (this.dgvWorkDistribute.Rows.Count <= 0 || this.dgvWorkDistribute.CurrentRow == null || MessageBox.Show("Удалить запись?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      this.session = Domain.CurrentSession;
      hmWorkDistribute dataBoundItem = (hmWorkDistribute) this.dgvWorkDistribute.CurrentRow.DataBoundItem;
      if (dataBoundItem.Period.PeriodId <= this.monthClosed.PeriodId)
      {
        int num1 = (int) MessageBox.Show("Невозможно удалить запись из закрытого месяца", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        try
        {
          IList<lsWorkPayRent> lsWorkPayRentList = (IList<lsWorkPayRent>) new List<lsWorkPayRent>();
          if (this.session.CreateQuery("select w from lsWorkPayRent w where w.WorkDistribute=:wd").SetParameter<hmWorkDistribute>("wd", dataBoundItem).List<lsWorkPayRent>().Count > 0)
          {
            if (MessageBox.Show("Есть расчеты по этой работе. Все равно удалить?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
              return;
            this.session.CreateSQLQuery("delete from lsWorkPayRent where WorkDistribute_id=:wd").SetParameter<int>("wd", dataBoundItem.WorkDistribute).ExecuteUpdate();
            this.session.CreateSQLQuery("delete from lsWorkDistribute where WorkDistribute_id=:wd").SetParameter<int>("wd", dataBoundItem.WorkDistribute).ExecuteUpdate();
            this.session.CreateSQLQuery("delete from hmWorkDistribute where WorkDistribute_id=:wd").SetParameter<int>("wd", dataBoundItem.WorkDistribute).ExecuteUpdate();
            return;
          }
          this.session.Delete((object) dataBoundItem);
          this.session.Flush();
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show("Невозможно удалить запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
        this.session.Clear();
      }
    }

    private void dgvWorkDistribute_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (((DataGridView) sender).DataSource == null)
        return;
      DataGridViewRow row = ((DataGridView) sender).Rows[e.RowIndex];
      if (((hmWorkDistribute) row.DataBoundItem).Period != null && ((hmWorkDistribute) row.DataBoundItem).Period.PeriodId == this.monthClosed.PeriodId + 1)
      {
        row.DefaultCellStyle.BackColor = Color.PapayaWhip;
        row.DefaultCellStyle.ForeColor = Color.Black;
      }
      else
      {
        row.DefaultCellStyle.BackColor = Color.White;
        row.DefaultCellStyle.ForeColor = Color.Gray;
      }
    }

    private void dgvWorkDistribute_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex > 0 && e.RowIndex >= 0 && this.dgvWorkDistribute.Columns[e.ColumnIndex].Name == "Scheme")
      {
        int num = ((hmWorkDistribute) this.dgvWorkDistribute.CurrentRow.DataBoundItem).Scheme;
        FrmScheme frmScheme = new FrmScheme((short) 12, (short) num);
        if (frmScheme.ShowDialog() == DialogResult.OK)
          num = (int) frmScheme.CurrentId();
        if (KvrplHelper.CheckProxy(78, 2, this.company, false))
          this.dgvWorkDistribute.CurrentRow.Cells["Scheme"].Value = (object) num;
        frmScheme.Dispose();
      }
      if (!KvrplHelper.CheckProxy(78, 2, this.company, false))
        return;
      this.btnSave.Enabled = true;
    }

    private void dgvWorkDistribute_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      int num = (int) new FrmLsWorkDistribute((hmWorkDistribute) this.dgvWorkDistribute.CurrentRow.DataBoundItem, this.company, ((hmWorkDistribute) this.dgvWorkDistribute.CurrentRow.DataBoundItem).Home).ShowDialog();
    }

    private void dgvCounters_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
    {
      if (this.dgvCounters.CurrentCell.ColumnIndex != 3)
        return;
      e.Control.KeyPress -= new KeyPressEventHandler(this.Control_KeyPress);
      e.Control.KeyPress += new KeyPressEventHandler(this.Control_KeyPress);
    }

    private void Control_KeyPress(object sender, KeyPressEventArgs e)
    {
      if ((int) e.KeyChar != 95)
        return;
      e.Handled = true;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmCounters));
      this.pnBtn = new Panel();
      this.btnCloseScheme = new Button();
      this.btnLoad = new Button();
      this.btnPrint = new Button();
      this.btnEdit = new Button();
      this.btnAudit = new Button();
      this.btnPastTime = new Button();
      this.btnReport = new Button();
      this.btnArchive = new Button();
      this.pbar = new ProgressBar();
      this.btnQuarterSum = new Button();
      this.btnSave = new Button();
      this.btnDelete = new Button();
      this.btnAdd = new Button();
      this.bntExit = new Button();
      this.pnTools = new Panel();
      this.lblEdit = new Label();
      this.cbArchive = new CheckBox();
      this.lblPastTime = new Label();
      this.lblCaption = new Label();
      this.dtmCounterDay = new DateTimePicker();
      this.lblCounterDay = new Label();
      this.mpCurrentPeriod = new MonthPicker();
      this.cmbBase = new ComboBox();
      this.lblBase = new Label();
      this.cmbService = new ComboBox();
      this.lblService = new Label();
      this.tcntrlCounters = new TabControl();
      this.tpPeriod = new TabPage();
      this.mcArchive = new MonthCalendar();
      this.dgvEvidence = new DataGridView();
      this.tpCounters = new TabPage();
      this.dgvCounters = new DataGridView();
      this.tpScheme = new TabPage();
      this.dgvScheme = new DataGridView();
      this.tpAudit = new TabPage();
      this.dgvAudit = new DataGridView();
      this.tpSeal = new TabPage();
      this.dgvSeal = new DataGridView();
      this.tpDetailEvidence = new TabPage();
      this.dgvDetailEvidence = new DataGridView();
      this.tpDetail = new TabPage();
      this.dgvDetail = new DataGridView();
      this.tpDistribute = new TabPage();
      this.dgvDistribute = new DataGridView();
      this.tpDstrDetail = new TabPage();
      this.dgvDstrDetail = new DataGridView();
      this.tpWorkDistribute = new TabPage();
      this.dgvWorkDistribute = new DataGridView();
      this.cmReport = new ContextMenuStrip(this.components);
      this.miClient = new ToolStripMenuItem();
      this.miHome = new ToolStripMenuItem();
      this.детализацияРаспределенныхСуммToolStripMenuItem = new ToolStripMenuItem();
      this.hp = new HelpProvider();
      this.lblCounter = new Label();
      this.lblMonth = new Label();
      this.lblPeriod = new Label();
      this.cmbCounter = new ComboBox();
      this.cmbMonth = new ComboBox();
      this.cmbPeriod = new ComboBox();
      this.pnDetail = new Panel();
      this.tmr = new Timer(this.components);
      this.tmrEvidence = new Timer(this.components);
      this.dsReport = new DataSet();
      this.pnBtn.SuspendLayout();
      this.pnTools.SuspendLayout();
      this.tcntrlCounters.SuspendLayout();
      this.tpPeriod.SuspendLayout();
      ((ISupportInitialize) this.dgvEvidence).BeginInit();
      this.tpCounters.SuspendLayout();
      ((ISupportInitialize) this.dgvCounters).BeginInit();
      this.tpScheme.SuspendLayout();
      ((ISupportInitialize) this.dgvScheme).BeginInit();
      this.tpAudit.SuspendLayout();
      ((ISupportInitialize) this.dgvAudit).BeginInit();
      this.tpSeal.SuspendLayout();
      ((ISupportInitialize) this.dgvSeal).BeginInit();
      this.tpDetailEvidence.SuspendLayout();
      ((ISupportInitialize) this.dgvDetailEvidence).BeginInit();
      this.tpDetail.SuspendLayout();
      ((ISupportInitialize) this.dgvDetail).BeginInit();
      this.tpDistribute.SuspendLayout();
      ((ISupportInitialize) this.dgvDistribute).BeginInit();
      this.tpDstrDetail.SuspendLayout();
      ((ISupportInitialize) this.dgvDstrDetail).BeginInit();
      this.tpWorkDistribute.SuspendLayout();
      ((ISupportInitialize) this.dgvWorkDistribute).BeginInit();
      this.cmReport.SuspendLayout();
      this.pnDetail.SuspendLayout();
      this.dsReport.BeginInit();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnCloseScheme);
      this.pnBtn.Controls.Add((Control) this.btnLoad);
      this.pnBtn.Controls.Add((Control) this.btnPrint);
      this.pnBtn.Controls.Add((Control) this.btnEdit);
      this.pnBtn.Controls.Add((Control) this.btnAudit);
      this.pnBtn.Controls.Add((Control) this.btnPastTime);
      this.pnBtn.Controls.Add((Control) this.btnReport);
      this.pnBtn.Controls.Add((Control) this.btnArchive);
      this.pnBtn.Controls.Add((Control) this.pbar);
      this.pnBtn.Controls.Add((Control) this.btnQuarterSum);
      this.pnBtn.Controls.Add((Control) this.btnSave);
      this.pnBtn.Controls.Add((Control) this.btnDelete);
      this.pnBtn.Controls.Add((Control) this.btnAdd);
      this.pnBtn.Controls.Add((Control) this.bntExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 370);
      this.pnBtn.Margin = new Padding(4);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(1114, 40);
      this.pnBtn.TabIndex = 0;
      this.btnCloseScheme.Location = new Point(820, 6);
      this.btnCloseScheme.Name = "btnCloseScheme";
      this.btnCloseScheme.Size = new Size(182, 28);
      this.btnCloseScheme.TabIndex = 14;
      this.btnCloseScheme.Text = "Закрытие типов расчета";
      this.btnCloseScheme.UseVisualStyleBackColor = true;
      this.btnCloseScheme.Visible = false;
      this.btnCloseScheme.Click += new EventHandler(this.btnCloseScheme_Click);
      this.btnLoad.Image = (Image) Resources.DateTime;
      this.btnLoad.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnLoad.Location = new Point(693, 5);
      this.btnLoad.Name = "btnLoad";
      this.btnLoad.Size = new Size(153, 28);
      this.btnLoad.TabIndex = 13;
      this.btnLoad.Text = "Взять показания";
      this.btnLoad.TextAlign = ContentAlignment.MiddleRight;
      this.btnLoad.UseVisualStyleBackColor = true;
      this.btnLoad.Visible = false;
      this.btnLoad.Click += new EventHandler(this.btnLoad_Click);
      this.btnPrint.Location = new Point(693, 6);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(92, 28);
      this.btnPrint.TabIndex = 12;
      this.btnPrint.Text = "На печать";
      this.btnPrint.UseVisualStyleBackColor = true;
      this.btnPrint.Visible = false;
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.btnEdit.Image = (Image) Resources.edit;
      this.btnEdit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnEdit.Location = new Point(443, 6);
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(148, 29);
      this.btnEdit.TabIndex = 11;
      this.btnEdit.Text = "Редактировать";
      this.btnEdit.TextAlign = ContentAlignment.MiddleRight;
      this.btnEdit.UseVisualStyleBackColor = true;
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnAudit.Location = new Point(693, 5);
      this.btnAudit.Name = "btnAudit";
      this.btnAudit.Size = new Size(121, 29);
      this.btnAudit.TabIndex = 10;
      this.btnAudit.Text = "Ввод поверок";
      this.btnAudit.UseVisualStyleBackColor = true;
      this.btnAudit.Visible = false;
      this.btnAudit.Click += new EventHandler(this.btnAudit_Click);
      this.btnPastTime.Enabled = false;
      this.btnPastTime.Image = (Image) Resources.time_24;
      this.btnPastTime.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnPastTime.Location = new Point(443, 5);
      this.btnPastTime.Name = "btnPastTime";
      this.btnPastTime.Size = new Size(148, 30);
      this.btnPastTime.TabIndex = 9;
      this.btnPastTime.Text = "Прошлое время";
      this.btnPastTime.TextAlign = ContentAlignment.MiddleRight;
      this.btnPastTime.UseVisualStyleBackColor = true;
      this.btnPastTime.Click += new EventHandler(this.btnPastTime_Click);
      this.btnReport.Image = (Image) Resources.file_text_32;
      this.btnReport.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnReport.Location = new Point(597, 5);
      this.btnReport.Name = "btnReport";
      this.btnReport.Size = new Size(90, 29);
      this.btnReport.TabIndex = 8;
      this.btnReport.Text = "Отчет";
      this.btnReport.TextAlign = ContentAlignment.MiddleRight;
      this.btnReport.UseVisualStyleBackColor = true;
      this.btnReport.Click += new EventHandler(this.btnReport_Click);
      this.btnArchive.Enabled = false;
      this.btnArchive.Image = (Image) Resources.Card_File;
      this.btnArchive.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnArchive.Location = new Point(343, 5);
      this.btnArchive.Name = "btnArchive";
      this.btnArchive.Size = new Size(94, 30);
      this.btnArchive.TabIndex = 6;
      this.btnArchive.Text = "В архив";
      this.btnArchive.TextAlign = ContentAlignment.MiddleRight;
      this.btnArchive.UseVisualStyleBackColor = true;
      this.btnArchive.Click += new EventHandler(this.btnArchive_Click);
      this.pbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pbar.Location = new Point(914, 6);
      this.pbar.Name = "pbar";
      this.pbar.Size = new Size(100, 23);
      this.pbar.TabIndex = 5;
      this.pbar.Visible = false;
      this.btnQuarterSum.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnQuarterSum.Location = new Point(693, 5);
      this.btnQuarterSum.Name = "btnQuarterSum";
      this.btnQuarterSum.Size = new Size(177, 30);
      this.btnQuarterSum.TabIndex = 4;
      this.btnQuarterSum.Text = "Суммы по квартальному";
      this.btnQuarterSum.TextAlign = ContentAlignment.MiddleRight;
      this.btnQuarterSum.UseVisualStyleBackColor = true;
      this.btnQuarterSum.Visible = false;
      this.btnQuarterSum.Click += new EventHandler(this.btnQuarter_Click);
      this.btnSave.Image = (Image) Resources.Tick;
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.Location = new Point(229, 5);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(108, 30);
      this.btnSave.TabIndex = 3;
      this.btnSave.Text = "Сохранить";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnDelete.Image = (Image) Resources.minus;
      this.btnDelete.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnDelete.Location = new Point(124, 5);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(99, 30);
      this.btnDelete.TabIndex = 2;
      this.btnDelete.Text = "Удалить";
      this.btnDelete.TextAlign = ContentAlignment.MiddleRight;
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnAdd.Image = (Image) Resources.plus;
      this.btnAdd.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnAdd.Location = new Point(12, 5);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(106, 30);
      this.btnAdd.TabIndex = 1;
      this.btnAdd.Text = "Добавить";
      this.btnAdd.TextAlign = ContentAlignment.MiddleRight;
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.bntExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.bntExit.DialogResult = DialogResult.Cancel;
      this.bntExit.Image = (Image) Resources.Exit;
      this.bntExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.bntExit.Location = new Point(1020, 5);
      this.bntExit.Name = "bntExit";
      this.bntExit.Size = new Size(82, 30);
      this.bntExit.TabIndex = 0;
      this.bntExit.Text = "Выход";
      this.bntExit.TextAlign = ContentAlignment.MiddleRight;
      this.bntExit.UseVisualStyleBackColor = true;
      this.bntExit.Click += new EventHandler(this.bntExit_Click);
      this.pnTools.Controls.Add((Control) this.lblEdit);
      this.pnTools.Controls.Add((Control) this.cbArchive);
      this.pnTools.Controls.Add((Control) this.lblPastTime);
      this.pnTools.Controls.Add((Control) this.lblCaption);
      this.pnTools.Controls.Add((Control) this.dtmCounterDay);
      this.pnTools.Controls.Add((Control) this.lblCounterDay);
      this.pnTools.Controls.Add((Control) this.mpCurrentPeriod);
      this.pnTools.Controls.Add((Control) this.cmbBase);
      this.pnTools.Controls.Add((Control) this.lblBase);
      this.pnTools.Controls.Add((Control) this.cmbService);
      this.pnTools.Controls.Add((Control) this.lblService);
      this.pnTools.Dock = DockStyle.Top;
      this.pnTools.Location = new Point(0, 0);
      this.pnTools.Name = "pnTools";
      this.pnTools.Size = new Size(1114, 56);
      this.pnTools.TabIndex = 1;
      this.lblEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblEdit.AutoSize = true;
      this.lblEdit.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblEdit.ForeColor = Color.DarkOrange;
      this.lblEdit.Location = new Point(919, 31);
      this.lblEdit.Name = "lblEdit";
      this.lblEdit.Size = new Size(183, 16);
      this.lblEdit.TabIndex = 20;
      this.lblEdit.Text = "Режим редактирования";
      this.lblEdit.Visible = false;
      this.cbArchive.AutoSize = true;
      this.cbArchive.Location = new Point(7, 53);
      this.cbArchive.Name = "cbArchive";
      this.cbArchive.Size = new Size(66, 20);
      this.cbArchive.TabIndex = 19;
      this.cbArchive.Text = "Архив";
      this.cbArchive.UseVisualStyleBackColor = true;
      this.cbArchive.CheckedChanged += new EventHandler(this.cbArchive_CheckedChanged);
      this.lblPastTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblPastTime.AutoSize = true;
      this.lblPastTime.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblPastTime.ForeColor = Color.DarkOrange;
      this.lblPastTime.Location = new Point(903, 31);
      this.lblPastTime.Name = "lblPastTime";
      this.lblPastTime.Size = new Size(199, 16);
      this.lblPastTime.TabIndex = 18;
      this.lblPastTime.Text = "Режим прошлого времени";
      this.lblPastTime.Visible = false;
      this.lblCaption.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblCaption.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblCaption.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblCaption.Location = new Point(4, 3);
      this.lblCaption.Margin = new Padding(4, 0, 4, 0);
      this.lblCaption.Name = "lblCaption";
      this.lblCaption.Size = new Size(956, 22);
      this.lblCaption.TabIndex = 17;
      this.dtmCounterDay.Location = new Point(652, 28);
      this.dtmCounterDay.MaxDate = new DateTime(2999, 12, 31, 0, 0, 0, 0);
      this.dtmCounterDay.Name = "dtmCounterDay";
      this.dtmCounterDay.Size = new Size(149, 22);
      this.dtmCounterDay.TabIndex = 16;
      this.lblCounterDay.AutoSize = true;
      this.lblCounterDay.Location = new Point(513, 31);
      this.lblCounterDay.Name = "lblCounterDay";
      this.lblCounterDay.Size = new Size(133, 16);
      this.lblCounterDay.TabIndex = 15;
      this.lblCounterDay.Text = "Показания на дату";
      this.mpCurrentPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.mpCurrentPeriod.CustomFormat = "MMMM yyyy";
      this.mpCurrentPeriod.Format = DateTimePickerFormat.Custom;
      this.mpCurrentPeriod.Location = new Point(967, 3);
      this.mpCurrentPeriod.Name = "mpCurrentPeriod";
      this.mpCurrentPeriod.OldMonth = 0;
      this.mpCurrentPeriod.ShowUpDown = true;
      this.mpCurrentPeriod.Size = new Size(135, 22);
      this.mpCurrentPeriod.TabIndex = 14;
      this.mpCurrentPeriod.ValueChanged += new EventHandler(this.dtmpCurrentPeriod_ValueChanged);
      this.cmbBase.FormattingEnabled = true;
      this.cmbBase.Location = new Point(104, 28);
      this.cmbBase.Name = "cmbBase";
      this.cmbBase.Size = new Size(139, 24);
      this.cmbBase.TabIndex = 12;
      this.cmbBase.SelectionChangeCommitted += new EventHandler(this.cmbBase_SelectionChangeCommitted);
      this.lblBase.AutoSize = true;
      this.lblBase.Location = new Point(4, 31);
      this.lblBase.Name = "lblBase";
      this.lblBase.Size = new Size(97, 16);
      this.lblBase.TabIndex = 11;
      this.lblBase.Text = "Вид счетчика";
      this.cmbService.FormattingEnabled = true;
      this.cmbService.Location = new Point(309, 28);
      this.cmbService.Name = "cmbService";
      this.cmbService.Size = new Size(198, 24);
      this.cmbService.TabIndex = 1;
      this.cmbService.SelectionChangeCommitted += new EventHandler(this.cmbService_SelectionChangeCommitted);
      this.lblService.AutoSize = true;
      this.lblService.Location = new Point(249, 31);
      this.lblService.Name = "lblService";
      this.lblService.Size = new Size(54, 16);
      this.lblService.TabIndex = 0;
      this.lblService.Text = "Услуга";
      this.tcntrlCounters.Controls.Add((Control) this.tpPeriod);
      this.tcntrlCounters.Controls.Add((Control) this.tpCounters);
      this.tcntrlCounters.Controls.Add((Control) this.tpScheme);
      this.tcntrlCounters.Controls.Add((Control) this.tpAudit);
      this.tcntrlCounters.Controls.Add((Control) this.tpSeal);
      this.tcntrlCounters.Controls.Add((Control) this.tpDetailEvidence);
      this.tcntrlCounters.Controls.Add((Control) this.tpDetail);
      this.tcntrlCounters.Controls.Add((Control) this.tpDistribute);
      this.tcntrlCounters.Controls.Add((Control) this.tpDstrDetail);
      this.tcntrlCounters.Controls.Add((Control) this.tpWorkDistribute);
      this.tcntrlCounters.Dock = DockStyle.Fill;
      this.tcntrlCounters.Location = new Point(0, 86);
      this.tcntrlCounters.Multiline = true;
      this.tcntrlCounters.Name = "tcntrlCounters";
      this.tcntrlCounters.SelectedIndex = 0;
      this.tcntrlCounters.Size = new Size(1114, 284);
      this.tcntrlCounters.TabIndex = 4;
      this.tcntrlCounters.SelectedIndexChanged += new EventHandler(this.tcntrlCounters_SelectedIndexChanged);
      this.tpPeriod.Controls.Add((Control) this.mcArchive);
      this.tpPeriod.Controls.Add((Control) this.dgvEvidence);
      this.hp.SetHelpKeyword((Control) this.tpPeriod, "kv82.html");
      this.hp.SetHelpNavigator((Control) this.tpPeriod, HelpNavigator.Topic);
      this.tpPeriod.Location = new Point(4, 46);
      this.tpPeriod.Name = "tpPeriod";
      this.tpPeriod.Padding = new Padding(3);
      this.hp.SetShowHelp((Control) this.tpPeriod, true);
      this.tpPeriod.Size = new Size(1106, 234);
      this.tpPeriod.TabIndex = 1;
      this.tpPeriod.Text = "Показания";
      this.tpPeriod.UseVisualStyleBackColor = true;
      this.mcArchive.Location = new Point(487, 49);
      this.mcArchive.Name = "mcArchive";
      this.mcArchive.TabIndex = 1;
      this.mcArchive.Visible = false;
      this.mcArchive.DateChanged += new DateRangeEventHandler(this.mcArchive_DateChanged);
      this.mcArchive.DateSelected += new DateRangeEventHandler(this.mcArchive_DateSelected);
      this.dgvEvidence.BackgroundColor = Color.AliceBlue;
      this.dgvEvidence.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvEvidence.Dock = DockStyle.Fill;
      this.dgvEvidence.Location = new Point(3, 3);
      this.dgvEvidence.Name = "dgvEvidence";
      this.dgvEvidence.Size = new Size(1100, 228);
      this.dgvEvidence.TabIndex = 0;
      this.dgvEvidence.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvEvidence_CellBeginEdit);
      this.dgvEvidence.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvEvidence_CellFormatting);
      this.dgvEvidence.CellLeave += new DataGridViewCellEventHandler(this.dgvEvidence_CellLeave);
      this.dgvEvidence.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvEvidence_ColumnWidthChanged);
      this.dgvEvidence.CurrentCellDirtyStateChanged += new EventHandler(this.dgvEvidence_CurrentCellDirtyStateChanged);
      this.dgvEvidence.DataError += new DataGridViewDataErrorEventHandler(this.dgvDistribute_DataError);
      this.tpCounters.Controls.Add((Control) this.dgvCounters);
      this.hp.SetHelpKeyword((Control) this.tpCounters, "kv81.html");
      this.hp.SetHelpNavigator((Control) this.tpCounters, HelpNavigator.Topic);
      this.tpCounters.Location = new Point(4, 25);
      this.tpCounters.Name = "tpCounters";
      this.tpCounters.Padding = new Padding(3);
      this.hp.SetShowHelp((Control) this.tpCounters, true);
      this.tpCounters.Size = new Size(1106, (int) byte.MaxValue);
      this.tpCounters.TabIndex = 0;
      this.tpCounters.Text = "Счетчики";
      this.tpCounters.UseVisualStyleBackColor = true;
      this.dgvCounters.BackgroundColor = Color.AliceBlue;
      this.dgvCounters.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvCounters.Dock = DockStyle.Fill;
      this.dgvCounters.Location = new Point(3, 3);
      this.dgvCounters.Name = "dgvCounters";
      this.dgvCounters.Size = new Size(1100, 249);
      this.dgvCounters.TabIndex = 3;
      this.dgvCounters.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvCounters_CellBeginEdit);
      this.dgvCounters.CellEndEdit += new DataGridViewCellEventHandler(this.dgvCounters_CellEndEdit);
      this.dgvCounters.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvCounters_CellFormatting);
      this.dgvCounters.CellValueChanged += new DataGridViewCellEventHandler(this.dgvCounters_CellValueChanged);
      this.dgvCounters.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvCounters_ColumnWidthChanged);
      this.dgvCounters.CurrentCellDirtyStateChanged += new EventHandler(this.dgvCounters_CurrentCellDirtyStateChanged);
      this.dgvCounters.DataError += new DataGridViewDataErrorEventHandler(this.dgvDistribute_DataError);
      this.dgvCounters.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgvCounters_EditingControlShowing);
      this.tpScheme.Controls.Add((Control) this.dgvScheme);
      this.hp.SetHelpKeyword((Control) this.tpScheme, "kv86.html");
      this.hp.SetHelpNavigator((Control) this.tpScheme, HelpNavigator.Topic);
      this.tpScheme.Location = new Point(4, 25);
      this.tpScheme.Name = "tpScheme";
      this.tpScheme.Padding = new Padding(3);
      this.hp.SetShowHelp((Control) this.tpScheme, true);
      this.tpScheme.Size = new Size(1106, (int) byte.MaxValue);
      this.tpScheme.TabIndex = 8;
      this.tpScheme.Text = "Тип расчета";
      this.tpScheme.UseVisualStyleBackColor = true;
      this.dgvScheme.BackgroundColor = Color.AliceBlue;
      this.dgvScheme.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvScheme.Dock = DockStyle.Fill;
      this.dgvScheme.Location = new Point(3, 3);
      this.dgvScheme.Name = "dgvScheme";
      this.dgvScheme.Size = new Size(1100, 249);
      this.dgvScheme.TabIndex = 0;
      this.dgvScheme.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvDistribute_CellBeginEdit);
      this.dgvScheme.CellClick += new DataGridViewCellEventHandler(this.dgvScheme_CellClick);
      this.dgvScheme.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvScheme_CellFormatting);
      this.dgvScheme.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvScheme_ColumnWidthChanged);
      this.dgvScheme.CurrentCellDirtyStateChanged += new EventHandler(this.dgvScheme_CurrentCellDirtyStateChanged);
      this.tpAudit.Controls.Add((Control) this.dgvAudit);
      this.tpAudit.Location = new Point(4, 25);
      this.tpAudit.Name = "tpAudit";
      this.tpAudit.Padding = new Padding(3);
      this.tpAudit.Size = new Size(1106, (int) byte.MaxValue);
      this.tpAudit.TabIndex = 5;
      this.tpAudit.Text = "Поверки";
      this.tpAudit.UseVisualStyleBackColor = true;
      this.dgvAudit.BackgroundColor = Color.AliceBlue;
      this.dgvAudit.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvAudit.Dock = DockStyle.Fill;
      this.hp.SetHelpKeyword((Control) this.dgvAudit, "kv87.html");
      this.hp.SetHelpNavigator((Control) this.dgvAudit, HelpNavigator.Topic);
      this.dgvAudit.Location = new Point(3, 3);
      this.dgvAudit.Name = "dgvAudit";
      this.hp.SetShowHelp((Control) this.dgvAudit, true);
      this.dgvAudit.Size = new Size(1100, 249);
      this.dgvAudit.TabIndex = 0;
      this.dgvAudit.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvEvidence_CellBeginEdit);
      this.dgvAudit.CellClick += new DataGridViewCellEventHandler(this.dgvAudit_CellClick);
      this.dgvAudit.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvAudit_CellFormatting);
      this.dgvAudit.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvAudit_ColumnWidthChanged);
      this.dgvAudit.CurrentCellDirtyStateChanged += new EventHandler(this.dgvAudit_CurrentCellDirtyStateChanged);
      this.dgvAudit.DataError += new DataGridViewDataErrorEventHandler(this.dgvDistribute_DataError);
      this.tpSeal.Controls.Add((Control) this.dgvSeal);
      this.hp.SetHelpKeyword((Control) this.tpSeal, "kv88.html");
      this.hp.SetHelpNavigator((Control) this.tpSeal, HelpNavigator.Topic);
      this.tpSeal.Location = new Point(4, 25);
      this.tpSeal.Name = "tpSeal";
      this.tpSeal.Padding = new Padding(3);
      this.hp.SetShowHelp((Control) this.tpSeal, true);
      this.tpSeal.Size = new Size(1106, (int) byte.MaxValue);
      this.tpSeal.TabIndex = 6;
      this.tpSeal.Text = "Пломбирование";
      this.tpSeal.UseVisualStyleBackColor = true;
      this.dgvSeal.BackgroundColor = Color.AliceBlue;
      this.dgvSeal.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvSeal.Dock = DockStyle.Fill;
      this.dgvSeal.Location = new Point(3, 3);
      this.dgvSeal.Name = "dgvSeal";
      this.dgvSeal.Size = new Size(1100, 249);
      this.dgvSeal.TabIndex = 0;
      this.dgvSeal.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvEvidence_CellBeginEdit);
      this.dgvSeal.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvSeal_ColumnWidthChanged);
      this.dgvSeal.CurrentCellDirtyStateChanged += new EventHandler(this.dgvSeal_CurrentCellDirtyStateChanged);
      this.dgvSeal.DataError += new DataGridViewDataErrorEventHandler(this.dgvDistribute_DataError);
      this.tpDetailEvidence.Controls.Add((Control) this.dgvDetailEvidence);
      this.hp.SetHelpKeyword((Control) this.tpDetailEvidence, "kv89.html");
      this.hp.SetHelpNavigator((Control) this.tpDetailEvidence, HelpNavigator.Topic);
      this.tpDetailEvidence.Location = new Point(4, 25);
      this.tpDetailEvidence.Name = "tpDetailEvidence";
      this.tpDetailEvidence.Padding = new Padding(3);
      this.hp.SetShowHelp((Control) this.tpDetailEvidence, true);
      this.tpDetailEvidence.Size = new Size(1106, (int) byte.MaxValue);
      this.tpDetailEvidence.TabIndex = 7;
      this.tpDetailEvidence.Text = "Детализация показаний";
      this.tpDetailEvidence.UseVisualStyleBackColor = true;
      this.dgvDetailEvidence.BackgroundColor = Color.AliceBlue;
      this.dgvDetailEvidence.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvDetailEvidence.Dock = DockStyle.Fill;
      this.dgvDetailEvidence.Location = new Point(3, 3);
      this.dgvDetailEvidence.Name = "dgvDetailEvidence";
      this.dgvDetailEvidence.ReadOnly = true;
      this.dgvDetailEvidence.Size = new Size(1100, 249);
      this.dgvDetailEvidence.TabIndex = 0;
      this.dgvDetailEvidence.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvDetailEvidence_CellFormatting);
      this.dgvDetailEvidence.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgvDetailEvidence_ColumnWidthChanged);
      this.dgvDetailEvidence.DataError += new DataGridViewDataErrorEventHandler(this.dgvDistribute_DataError);
      this.tpDetail.Controls.Add((Control) this.dgvDetail);
      this.hp.SetHelpKeyword((Control) this.tpDetail, "kv83.html");
      this.hp.SetHelpNavigator((Control) this.tpDetail, HelpNavigator.Topic);
      this.tpDetail.Location = new Point(4, 25);
      this.tpDetail.Name = "tpDetail";
      this.tpDetail.Padding = new Padding(3);
      this.hp.SetShowHelp((Control) this.tpDetail, true);
      this.tpDetail.Size = new Size(1106, (int) byte.MaxValue);
      this.tpDetail.TabIndex = 2;
      this.tpDetail.Text = "Детализация домовых счетчиков";
      this.tpDetail.UseVisualStyleBackColor = true;
      this.dgvDetail.BackgroundColor = Color.AliceBlue;
      this.dgvDetail.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvDetail.Dock = DockStyle.Fill;
      this.dgvDetail.Location = new Point(3, 3);
      this.dgvDetail.Name = "dgvDetail";
      this.dgvDetail.Size = new Size(1100, 249);
      this.dgvDetail.TabIndex = 0;
      this.dgvDetail.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvDetail_CellFormatting);
      this.dgvDetail.DataError += new DataGridViewDataErrorEventHandler(this.dgvDistribute_DataError);
      this.tpDistribute.Controls.Add((Control) this.dgvDistribute);
      this.hp.SetHelpKeyword((Control) this.tpDistribute, "kv84.html");
      this.hp.SetHelpNavigator((Control) this.tpDistribute, HelpNavigator.Topic);
      this.tpDistribute.Location = new Point(4, 25);
      this.tpDistribute.Name = "tpDistribute";
      this.tpDistribute.Padding = new Padding(3);
      this.hp.SetShowHelp((Control) this.tpDistribute, true);
      this.tpDistribute.Size = new Size(1106, (int) byte.MaxValue);
      this.tpDistribute.TabIndex = 3;
      this.tpDistribute.Text = "Распределение сумм на дом";
      this.tpDistribute.UseVisualStyleBackColor = true;
      this.dgvDistribute.BackgroundColor = Color.AliceBlue;
      this.dgvDistribute.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvDistribute.Dock = DockStyle.Fill;
      this.dgvDistribute.Location = new Point(3, 3);
      this.dgvDistribute.Name = "dgvDistribute";
      this.dgvDistribute.Size = new Size(1100, 249);
      this.dgvDistribute.TabIndex = 0;
      this.dgvDistribute.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvDistribute_CellBeginEdit);
      this.dgvDistribute.CellClick += new DataGridViewCellEventHandler(this.dgvDistribute_CellClick);
      this.dgvDistribute.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvDistribute_CellFormatting);
      this.dgvDistribute.CurrentCellDirtyStateChanged += new EventHandler(this.dgvDistribute_CurrentCellDirtyStateChanged);
      this.dgvDistribute.DataError += new DataGridViewDataErrorEventHandler(this.dgvDistribute_DataError);
      this.tpDstrDetail.Controls.Add((Control) this.dgvDstrDetail);
      this.hp.SetHelpKeyword((Control) this.tpDstrDetail, "kv85.html");
      this.hp.SetHelpNavigator((Control) this.tpDstrDetail, HelpNavigator.Topic);
      this.tpDstrDetail.Location = new Point(4, 46);
      this.tpDstrDetail.Name = "tpDstrDetail";
      this.tpDstrDetail.Padding = new Padding(3);
      this.hp.SetShowHelp((Control) this.tpDstrDetail, true);
      this.tpDstrDetail.Size = new Size(1106, 234);
      this.tpDstrDetail.TabIndex = 4;
      this.tpDstrDetail.Text = "Детализация распределения сумм";
      this.tpDstrDetail.UseVisualStyleBackColor = true;
      this.dgvDstrDetail.BackgroundColor = Color.AliceBlue;
      this.dgvDstrDetail.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvDstrDetail.Dock = DockStyle.Fill;
      this.dgvDstrDetail.Location = new Point(3, 3);
      this.dgvDstrDetail.Name = "dgvDstrDetail";
      this.dgvDstrDetail.ReadOnly = true;
      this.dgvDstrDetail.Size = new Size(1100, 228);
      this.dgvDstrDetail.TabIndex = 0;
      this.dgvDstrDetail.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvDstrDetail_CellFormatting);
      this.dgvDstrDetail.DataError += new DataGridViewDataErrorEventHandler(this.dgvDistribute_DataError);
      this.tpWorkDistribute.Controls.Add((Control) this.dgvWorkDistribute);
      this.tpWorkDistribute.Location = new Point(4, 46);
      this.tpWorkDistribute.Name = "tpWorkDistribute";
      this.tpWorkDistribute.Padding = new Padding(3);
      this.tpWorkDistribute.Size = new Size(1106, 234);
      this.tpWorkDistribute.TabIndex = 9;
      this.tpWorkDistribute.Text = "Оплата выполненных работ";
      this.tpWorkDistribute.UseVisualStyleBackColor = true;
      this.dgvWorkDistribute.AllowDrop = true;
      this.dgvWorkDistribute.BackgroundColor = Color.AliceBlue;
      this.dgvWorkDistribute.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvWorkDistribute.Dock = DockStyle.Fill;
      this.dgvWorkDistribute.Location = new Point(3, 3);
      this.dgvWorkDistribute.Name = "dgvWorkDistribute";
      this.dgvWorkDistribute.Size = new Size(1100, 228);
      this.dgvWorkDistribute.TabIndex = 0;
      this.dgvWorkDistribute.CellClick += new DataGridViewCellEventHandler(this.dgvWorkDistribute_CellClick);
      this.dgvWorkDistribute.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvWorkDistribute_CellFormatting);
      this.dgvWorkDistribute.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dgvWorkDistribute_CellMouseDoubleClick);
      this.cmReport.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.miClient,
        (ToolStripItem) this.miHome,
        (ToolStripItem) this.детализацияРаспределенныхСуммToolStripMenuItem
      });
      this.cmReport.Name = "cmReport";
      this.cmReport.Size = new Size(367, 70);
      this.miClient.Name = "miClient";
      this.miClient.Size = new Size(366, 22);
      this.miClient.Tag = (object) "72";
      this.miClient.Text = "Ведомость индивидуальных и квартирных счетчиков";
      this.miClient.Click += new EventHandler(this.ведомостьИндивидуальныхИКвартирныхСчетчиковToolStripMenuItem_Click);
      this.miHome.Name = "miHome";
      this.miHome.Size = new Size(366, 22);
      this.miHome.Tag = (object) "76";
      this.miHome.Text = "Ведомость домовых счетчиков";
      this.miHome.Click += new EventHandler(this.ведомостьИндивидуальныхИКвартирныхСчетчиковToolStripMenuItem_Click);
      this.детализацияРаспределенныхСуммToolStripMenuItem.Name = "детализацияРаспределенныхСуммToolStripMenuItem";
      this.детализацияРаспределенныхСуммToolStripMenuItem.Size = new Size(366, 22);
      this.детализацияРаспределенныхСуммToolStripMenuItem.Text = "Детализация распределенных сумм";
      this.детализацияРаспределенныхСуммToolStripMenuItem.Visible = false;
      this.детализацияРаспределенныхСуммToolStripMenuItem.Click += new EventHandler(this.btnPrint_Click);
      this.hp.HelpNamespace = "Help.chm";
      this.lblCounter.AutoSize = true;
      this.lblCounter.Location = new Point(3, 6);
      this.lblCounter.Name = "lblCounter";
      this.lblCounter.Size = new Size(48, 16);
      this.lblCounter.TabIndex = 0;
      this.lblCounter.Text = "Адрес";
      this.lblMonth.AutoSize = true;
      this.lblMonth.Location = new Point(509, 6);
      this.lblMonth.Name = "lblMonth";
      this.lblMonth.Size = new Size(49, 16);
      this.lblMonth.TabIndex = 1;
      this.lblMonth.Text = "Месяц";
      this.lblPeriod.AutoSize = true;
      this.lblPeriod.Location = new Point(283, 6);
      this.lblPeriod.Name = "lblPeriod";
      this.lblPeriod.Size = new Size(58, 16);
      this.lblPeriod.TabIndex = 2;
      this.lblPeriod.Text = "Период";
      this.cmbCounter.FormattingEnabled = true;
      this.cmbCounter.Location = new Point(58, 3);
      this.cmbCounter.Name = "cmbCounter";
      this.cmbCounter.Size = new Size(219, 24);
      this.cmbCounter.TabIndex = 3;
      this.cmbCounter.SelectionChangeCommitted += new EventHandler(this.cmbCounter_SelectionChangeCommitted);
      this.cmbMonth.FormatString = "MMMM   yyyy";
      this.cmbMonth.FormattingEnabled = true;
      this.cmbMonth.Location = new Point(564, 3);
      this.cmbMonth.Name = "cmbMonth";
      this.cmbMonth.Size = new Size(137, 24);
      this.cmbMonth.TabIndex = 6;
      this.cmbMonth.SelectionChangeCommitted += new EventHandler(this.cmbMonth_SelectionChangeCommitted);
      this.cmbPeriod.FormatString = "MMMM   yyyy";
      this.cmbPeriod.FormattingEnabled = true;
      this.cmbPeriod.Location = new Point(347, 3);
      this.cmbPeriod.Name = "cmbPeriod";
      this.cmbPeriod.Size = new Size(137, 24);
      this.cmbPeriod.TabIndex = 7;
      this.cmbPeriod.SelectionChangeCommitted += new EventHandler(this.cmbPeriod_SelectionChangeCommitted);
      this.pnDetail.Controls.Add((Control) this.cmbPeriod);
      this.pnDetail.Controls.Add((Control) this.cmbMonth);
      this.pnDetail.Controls.Add((Control) this.cmbCounter);
      this.pnDetail.Controls.Add((Control) this.lblPeriod);
      this.pnDetail.Controls.Add((Control) this.lblMonth);
      this.pnDetail.Controls.Add((Control) this.lblCounter);
      this.pnDetail.Dock = DockStyle.Top;
      this.pnDetail.Location = new Point(0, 56);
      this.pnDetail.Name = "pnDetail";
      this.pnDetail.Size = new Size(1114, 30);
      this.pnDetail.TabIndex = 0;
      this.tmr.Interval = 1000;
      this.tmr.Tick += new EventHandler(this.tmr_Tick);
      this.tmrEvidence.Interval = 1000;
      this.tmrEvidence.Tick += new EventHandler(this.tmrEvidence_Tick);
      this.dsReport.DataSetName = "NewDataSet";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.bntExit;
      this.ClientSize = new Size(1114, 410);
      this.Controls.Add((Control) this.tcntrlCounters);
      this.Controls.Add((Control) this.pnDetail);
      this.Controls.Add((Control) this.pnTools);
      this.Controls.Add((Control) this.pnBtn);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmCounters";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Счетчики";
      this.Load += new EventHandler(this.FrmCounters_Load);
      this.Shown += new EventHandler(this.FrmCounters_Shown);
      this.pnBtn.ResumeLayout(false);
      this.pnTools.ResumeLayout(false);
      this.pnTools.PerformLayout();
      this.tcntrlCounters.ResumeLayout(false);
      this.tpPeriod.ResumeLayout(false);
      ((ISupportInitialize) this.dgvEvidence).EndInit();
      this.tpCounters.ResumeLayout(false);
      ((ISupportInitialize) this.dgvCounters).EndInit();
      this.tpScheme.ResumeLayout(false);
      ((ISupportInitialize) this.dgvScheme).EndInit();
      this.tpAudit.ResumeLayout(false);
      ((ISupportInitialize) this.dgvAudit).EndInit();
      this.tpSeal.ResumeLayout(false);
      ((ISupportInitialize) this.dgvSeal).EndInit();
      this.tpDetailEvidence.ResumeLayout(false);
      ((ISupportInitialize) this.dgvDetailEvidence).EndInit();
      this.tpDetail.ResumeLayout(false);
      ((ISupportInitialize) this.dgvDetail).EndInit();
      this.tpDistribute.ResumeLayout(false);
      ((ISupportInitialize) this.dgvDistribute).EndInit();
      this.tpDstrDetail.ResumeLayout(false);
      ((ISupportInitialize) this.dgvDstrDetail).EndInit();
      this.tpWorkDistribute.ResumeLayout(false);
      ((ISupportInitialize) this.dgvWorkDistribute).EndInit();
      this.cmReport.ResumeLayout(false);
      this.pnDetail.ResumeLayout(false);
      this.pnDetail.PerformLayout();
      this.dsReport.EndInit();
      this.ResumeLayout(false);
    }
  }
}
