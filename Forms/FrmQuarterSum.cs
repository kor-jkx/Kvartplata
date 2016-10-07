// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmQuarterSum
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
  public class FrmQuarterSum : FrmBaseForm
  {
    private FormStateSaver fss = new FormStateSaver(FrmQuarterSum.container);
    private IContainer components = (IContainer) null;
    private ISession session;
    private static IContainer container;
    private Company company;
    private int level;
    private Home home;
    private BaseCounter bc;
    private Panel pnBtn;
    private Button btnExit;
    private Panel pnUp;
    private Button btnStart;
    private MonthPicker mpCurrentPeriod;
    private Label lblService;
    private ComboBox cmbService;
    private MonthPicker mpMonth;
    private Label lblMonth;
    private CheckedListBox clbListHomes;
    private Button btnCheck;
    private Button btnScheme;
    private Label lblScheme;
    private ComboBox cmbBase;
    private Label lblBase;

    public FrmQuarterSum(int level, Company company, Home home, BaseCounter bc)
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
      this.level = level;
      this.company = company;
      this.home = home;
      this.bc = bc;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void FrmQuarterSum_Shown(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      IList<Service> serviceList = (IList<Service>) new List<Service>();
      this.cmbService.DataSource = (object) this.session.CreateQuery(string.Format("select s from Service s,ServiceParam sp where sp.Service_id=s.ServiceId and s.Root=0 and s.ServiceId<>0 and sp.Company_id={0} order by " + Options.SortService, (object) Options.Company.CompanyId)).List<Service>();
      this.cmbService.DisplayMember = "ServiceName";
      this.cmbService.ValueMember = "ServiceId";
      IList<BaseCounter> baseCounterList = (IList<BaseCounter>) new List<BaseCounter>();
      this.cmbBase.DataSource = (object) this.session.CreateCriteria(typeof (BaseCounter)).Add((ICriterion) Restrictions.In("Id", (ICollection) new int[2]{ 1, 4 })).AddOrder(Order.Asc("Id")).List<BaseCounter>();
      this.cmbBase.DisplayMember = "Name";
      this.cmbBase.ValueMember = "Id";
      try
      {
        this.cmbBase.SelectedValue = (object) this.bc.Id;
      }
      catch
      {
      }
      MonthPicker mpCurrentPeriod = this.mpCurrentPeriod;
      DateTime? periodName = Options.Period.PeriodName;
      DateTime dateTime1 = periodName.Value;
      mpCurrentPeriod.Value = dateTime1;
      MonthPicker mpMonth = this.mpMonth;
      periodName = Options.Period.PeriodName;
      DateTime dateTime2 = periodName.Value.AddMonths(-1);
      mpMonth.Value = dateTime2;
      this.LoadCounters();
      this.session.Clear();
    }

    private void LoadCounters()
    {
      this.session.Clear();
      this.session = Domain.CurrentSession;
      IList<Counter> counterList = (IList<Counter>) new List<Counter>();
      int num = 0;
      try
      {
        num = Convert.ToInt32(this.session.CreateQuery(string.Format("select ParamValue from CompanyParam where Period.PeriodId=0 and Company.CompanyId={0} and DBeg<='{1}' and DEnd>='{1}' and Param.ParamId=213", (object) this.company.CompanyId, (object) KvrplHelper.DateToBaseFormat(this.mpMonth.Value))).UniqueResult());
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
      if (Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
      {
        if (this.level == 2)
          counterList = this.session.CreateQuery(string.Format("select c from Counter c left join fetch c.Home h left join fetch h.Str, HomeLink hl where hl.Home=h and hl.Company.CompanyId={1} and c.BaseCounter.Id=1 and (c.Service.ServiceId=(select sp.CrossServ.ServiceId from CrossService sp where sp.Company.CompanyId={1} and sp.Service.ServiceId={0} and CrossType.CrossTypeId=2 and ((isnull((select ParamValue from HomeParam where Period.PeriodId=0 and Home.IdHome={2} and DBeg<='{3}' and DEnd>='{3}' and Param.ParamId=302),0)<>1 and {4}<>1) or {0} not in (73,74)))  or c.Service.ServiceId=(select spr.CrossServ.ServiceId from CrossService spr where spr.Company.CompanyId={1} and spr.Service.ServiceId=(select sp.CrossServ.ServiceId from CrossService sp where sp.Company.CompanyId={1} and sp.Service.ServiceId={0} and CrossType.CrossTypeId=2) and CrossType.CrossTypeId=1 and (isnull((select ParamValue from HomeParam where Period.PeriodId=0 and Home=c.Home and Company=c.Company and DBeg<='{3}' and DEnd>='{3}' and Param.ParamId=302),0)=1 or {4}=1))) and c.Company.CompanyId={1} and    c.Complex.ComplexId={2} and isnull(c.ArchivesDate,'2999-12-31')>='{3}' order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)", this.cmbService.SelectedValue, (object) this.company.CompanyId, (object) Options.Complex.ComplexId, (object) KvrplHelper.DateToBaseFormat(this.mpMonth.Value), (object) num)).List<Counter>();
        if (this.level == 3)
          counterList = this.session.CreateQuery(string.Format("select c from Counter c left join fetch c.Home h left join fetch h.Str, HomeLink hl where hl.Home=h and hl.Company.CompanyId={1} and c.BaseCounter.Id=1 and (c.Service.ServiceId=(select sp.CrossServ.ServiceId from CrossService sp where sp.Company.CompanyId={1} and sp.Service.ServiceId={0} and CrossType.CrossTypeId=2 and ((isnull((select ParamValue from HomeParam where Period.PeriodId=0 and Home.IdHome={2} and DBeg<='{4}' and DEnd>='{4}' and Param.ParamId=302),0)<>1 and {5}<>1) or {0} not in (73,74)))  or c.Service.ServiceId=(select spr.CrossServ.ServiceId from CrossService spr where spr.Company.CompanyId={1} and spr.Service.ServiceId=(select sp.CrossServ.ServiceId from CrossService sp where sp.Company.CompanyId={1} and sp.Service.ServiceId={0} and CrossType.CrossTypeId=2) and CrossType.CrossTypeId=1 and (isnull((select ParamValue from HomeParam where Period.PeriodId=0 and Home=c.Home and Company=c.Company and DBeg<='{4}' and DEnd>='{4}' and Param.ParamId=302),0)=1 or {5}=1))) and c.Company.CompanyId={1} and    c.Home.IdHome={2} and c.Complex.ComplexId={3} and isnull(c.ArchivesDate,'2999-12-31')>='{4}' order by h.Str.NameStr,DBA.LENGTHHOME(h.NHome)", this.cmbService.SelectedValue, (object) this.company.CompanyId, (object) this.home.IdHome, (object) Options.Complex.ComplexId, (object) KvrplHelper.DateToBaseFormat(this.mpMonth.Value), (object) num)).List<Counter>();
      }
      else
        counterList = this.session.CreateQuery(string.Format("select c from Counter c where c.Company.CompanyId={1} and c.BaseCounter.Id=4 and (c.Service.ServiceId=(select sp.CrossServ.ServiceId from CrossService sp where sp.Company.CompanyId={1} and sp.Service.ServiceId={0} and CrossType.CrossTypeId=2 and {0} not in (73,74))  or c.Service.ServiceId=(select spr.CrossServ.ServiceId from CrossService spr where spr.Company.CompanyId={1} and spr.Service.ServiceId=(select sp.CrossServ.ServiceId from CrossService sp where sp.Company.CompanyId={1} and sp.Service.ServiceId={0} and CrossType.CrossTypeId=2) and CrossType.CrossTypeId=1)) and c.Company.CompanyId={1} and    c.Complex.ComplexId={2} and isnull(c.ArchivesDate,'2999-12-31')>='{3}' order by c.Service.ServiceId,c.CounterNum", this.cmbService.SelectedValue, (object) this.company.CompanyId, (object) Options.Complex.ComplexId, (object) KvrplHelper.DateToBaseFormat(this.mpMonth.Value))).List<Counter>();
      foreach (Counter counter in (IEnumerable<Counter>) counterList)
        counter.Notice = counter.AdrAndNum;
      this.clbListHomes.DataSource = (object) counterList;
      this.clbListHomes.DisplayMember = "Notice";
      this.clbListHomes.ValueMember = "CounterId";
      this.session.Clear();
    }

    private void btnCheck_Click(object sender, EventArgs e)
    {
      if (this.btnCheck.Text == "Выделить все")
      {
        for (int index = 0; index < this.clbListHomes.Items.Count; ++index)
          this.clbListHomes.SetItemCheckState(index, CheckState.Checked);
        this.btnCheck.Text = "Снять все";
      }
      else
      {
        for (int index = 0; index < this.clbListHomes.Items.Count; ++index)
          this.clbListHomes.SetItemCheckState(index, CheckState.Unchecked);
        this.btnCheck.Text = "Выделить все";
      }
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Внести записи для выбранных счетчиков? Записи, внесенные ранее, будут удалены.", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      if (KvrplHelper.GetCmpKvrClose(this.company, Options.ComplexPasp.IdFk, Options.ComplexPrior.IdFk).PeriodName.Value.AddMonths(1) > this.mpCurrentPeriod.Value)
      {
        int num = (int) MessageBox.Show("Невозможно внести записи в закрытый период", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.mpMonth.Value = this.mpCurrentPeriod.Value.AddMonths(-1);
      }
      else if (this.mpMonth.Value >= this.mpCurrentPeriod.Value)
      {
        int num = (int) MessageBox.Show("Месяц, за который вносятся суммы, должен быть меньше периода", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.mpMonth.Value = this.mpCurrentPeriod.Value.AddMonths(-1);
      }
      else
      {
        Decimal num1 = new Decimal();
        bool flag = false;
        this.session = Domain.CurrentSession;
        Period period1 = new Period();
        Period period2 = this.session.CreateCriteria(typeof (Period)).Add((ICriterion) Restrictions.Eq("PeriodName", (object) this.mpMonth.Value)).List<Period>()[0];
        foreach (Counter checkedItem in this.clbListHomes.CheckedItems)
        {
          Decimal num2 = new Decimal();
          if (Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
            this.session.CreateQuery(string.Format("delete from Distribute where Company.CompanyId={0} and Home.IdHome={1} and Period.PeriodId={2} and Service.ServiceId={3} and Month.PeriodId={4} and CounterId={5}", (object) this.company.CompanyId, (object) checkedItem.Home.IdHome, (object) Options.Period.PeriodId, this.cmbService.SelectedValue, (object) period2.PeriodId, (object) checkedItem.CounterId)).ExecuteUpdate();
          else
            this.session.CreateQuery(string.Format("delete from Distribute where Company.CompanyId={0} and Home.IdHome is null and Period.PeriodId={1} and Service.ServiceId={2} and Month.PeriodId={3} and CounterId={4}", (object) this.company.CompanyId, (object) Options.Period.PeriodId, this.cmbService.SelectedValue, (object) period2.PeriodId, (object) checkedItem.CounterId)).ExecuteUpdate();
          Distribute distribute = new Distribute();
          distribute.DistributeId = (int) this.session.CreateSQLQuery("select DBA.gen_id('hmDistribute',1)").UniqueResult();
          distribute.Company = this.company;
          distribute.Home = checkedItem.Home;
          distribute.Period = Options.Period;
          distribute.Service = this.cmbService.SelectedItem as Service;
          distribute.Month = period2;
          distribute.Rent = num2;
          distribute.Scheme = Convert.ToInt16(this.btnScheme.Text);
          distribute.Note = "";
          distribute.CounterId = checkedItem.CounterId;
          distribute.UName = Options.Login;
          distribute.DEdit = DateTime.Now;
          IList<Distribute> distributeList1 = (IList<Distribute>) new List<Distribute>();
          IList<Distribute> distributeList2;
          if (Convert.ToInt32(this.cmbBase.SelectedValue) != 4)
            distributeList2 = this.session.CreateQuery(string.Format("from Distribute where Company.CompanyId={0} and Home.IdHome={1} and Month.PeriodId={2} and CounterId={3} and Service.ServiceId={4}", (object) distribute.Company.CompanyId, (object) distribute.Home.IdHome, (object) distribute.Month.PeriodId, (object) distribute.CounterId, (object) distribute.Service.ServiceId)).List<Distribute>();
          else
            distributeList2 = this.session.CreateQuery(string.Format("from Distribute where Company.CompanyId={0} and Home.IdHome is null and Month.PeriodId={1} and CounterId={2} and Service.ServiceId={3}", (object) distribute.Company.CompanyId, (object) distribute.Month.PeriodId, (object) distribute.CounterId, (object) distribute.Service.ServiceId)).List<Distribute>();
          if (distributeList2.Count > 0 && MessageBox.Show("Уже существует запись по счетчику " + checkedItem.AdrAndNum + " за выбраный месяц. Суммы будут сложены. Все равно занести?", "Внимание!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK || distributeList2.Count == 0)
          {
            this.session.Save((object) distribute);
            try
            {
              this.session.Flush();
            }
            catch (Exception ex)
            {
              flag = true;
              KvrplHelper.WriteLog(ex, (LsClient) null);
            }
          }
        }
        this.session.Clear();
        if (flag)
        {
          int num3 = (int) MessageBox.Show("Операция завершилась с ошибками", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          int num4 = (int) MessageBox.Show("Операция завершена", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
    }

    private void mpCurrentPeriod_ValueChanged(object sender, EventArgs e)
    {
      Options.Period = KvrplHelper.SaveCurrentPeriod(this.mpCurrentPeriod.Value);
    }

    private void cmbService_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.LoadCounters();
    }

    private void mpMonth_ValueChanged(object sender, EventArgs e)
    {
      this.LoadCounters();
    }

    private void btnScheme_Click(object sender, EventArgs e)
    {
      short id = Convert.ToInt16(this.btnScheme.Text);
      FrmScheme frmScheme = new FrmScheme((short) 9, id);
      if (frmScheme.ShowDialog() == DialogResult.OK)
        id = frmScheme.CurrentId();
      this.btnScheme.Text = id.ToString();
      frmScheme.Dispose();
    }

    private void cmbBase_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.LoadCounters();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnBtn = new Panel();
      this.btnCheck = new Button();
      this.btnStart = new Button();
      this.btnExit = new Button();
      this.pnUp = new Panel();
      this.cmbBase = new ComboBox();
      this.lblBase = new Label();
      this.btnScheme = new Button();
      this.lblScheme = new Label();
      this.mpMonth = new MonthPicker();
      this.lblMonth = new Label();
      this.lblService = new Label();
      this.cmbService = new ComboBox();
      this.mpCurrentPeriod = new MonthPicker();
      this.clbListHomes = new CheckedListBox();
      this.pnBtn.SuspendLayout();
      this.pnUp.SuspendLayout();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnCheck);
      this.pnBtn.Controls.Add((Control) this.btnStart);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 296);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(1038, 40);
      this.pnBtn.TabIndex = 1;
      this.btnCheck.Image = (Image) Resources.properties;
      this.btnCheck.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCheck.Location = new Point(12, 5);
      this.btnCheck.Name = "btnCheck";
      this.btnCheck.Size = new Size(135, 30);
      this.btnCheck.TabIndex = 2;
      this.btnCheck.Text = "Выделить все";
      this.btnCheck.TextAlign = ContentAlignment.MiddleRight;
      this.btnCheck.UseVisualStyleBackColor = true;
      this.btnCheck.Click += new EventHandler(this.btnCheck_Click);
      this.btnStart.Image = (Image) Resources.Configure;
      this.btnStart.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnStart.Location = new Point(168, 5);
      this.btnStart.Name = "btnStart";
      this.btnStart.Size = new Size(111, 30);
      this.btnStart.TabIndex = 1;
      this.btnStart.Text = "Выполнить";
      this.btnStart.TextAlign = ContentAlignment.MiddleRight;
      this.btnStart.UseVisualStyleBackColor = true;
      this.btnStart.Click += new EventHandler(this.btnStart_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(944, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(82, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.pnUp.Controls.Add((Control) this.cmbBase);
      this.pnUp.Controls.Add((Control) this.lblBase);
      this.pnUp.Controls.Add((Control) this.btnScheme);
      this.pnUp.Controls.Add((Control) this.lblScheme);
      this.pnUp.Controls.Add((Control) this.mpMonth);
      this.pnUp.Controls.Add((Control) this.lblMonth);
      this.pnUp.Controls.Add((Control) this.lblService);
      this.pnUp.Controls.Add((Control) this.cmbService);
      this.pnUp.Controls.Add((Control) this.mpCurrentPeriod);
      this.pnUp.Dock = DockStyle.Top;
      this.pnUp.Location = new Point(0, 0);
      this.pnUp.Name = "pnUp";
      this.pnUp.Size = new Size(1038, 70);
      this.pnUp.TabIndex = 2;
      this.cmbBase.FormattingEnabled = true;
      this.cmbBase.Location = new Point(103, 6);
      this.cmbBase.Name = "cmbBase";
      this.cmbBase.Size = new Size(107, 24);
      this.cmbBase.TabIndex = 14;
      this.cmbBase.SelectionChangeCommitted += new EventHandler(this.cmbBase_SelectionChangeCommitted);
      this.lblBase.AutoSize = true;
      this.lblBase.Location = new Point(3, 9);
      this.lblBase.Name = "lblBase";
      this.lblBase.Size = new Size(97, 16);
      this.lblBase.TabIndex = 13;
      this.lblBase.Text = "Вид счетчика";
      this.btnScheme.Location = new Point(63, 36);
      this.btnScheme.Name = "btnScheme";
      this.btnScheme.Size = new Size(75, 23);
      this.btnScheme.TabIndex = 6;
      this.btnScheme.Text = "0";
      this.btnScheme.UseVisualStyleBackColor = true;
      this.btnScheme.Click += new EventHandler(this.btnScheme_Click);
      this.lblScheme.AutoSize = true;
      this.lblScheme.Location = new Point(3, 39);
      this.lblScheme.Name = "lblScheme";
      this.lblScheme.Size = new Size(48, 16);
      this.lblScheme.TabIndex = 5;
      this.lblScheme.Text = "Схема";
      this.mpMonth.CustomFormat = "MMMM yyyy";
      this.mpMonth.Format = DateTimePickerFormat.Custom;
      this.mpMonth.Location = new Point(569, 6);
      this.mpMonth.Name = "mpMonth";
      this.mpMonth.OldMonth = 0;
      this.mpMonth.ShowUpDown = true;
      this.mpMonth.Size = new Size((int) sbyte.MaxValue, 22);
      this.mpMonth.TabIndex = 4;
      this.mpMonth.ValueChanged += new EventHandler(this.mpMonth_ValueChanged);
      this.lblMonth.AutoSize = true;
      this.lblMonth.Location = new Point(496, 9);
      this.lblMonth.Name = "lblMonth";
      this.lblMonth.Size = new Size(67, 16);
      this.lblMonth.TabIndex = 3;
      this.lblMonth.Text = "За месяц";
      this.lblService.AutoSize = true;
      this.lblService.Location = new Point(234, 9);
      this.lblService.Name = "lblService";
      this.lblService.Size = new Size(54, 16);
      this.lblService.TabIndex = 2;
      this.lblService.Text = "Услуга";
      this.cmbService.FormattingEnabled = true;
      this.cmbService.Location = new Point(294, 6);
      this.cmbService.Name = "cmbService";
      this.cmbService.Size = new Size(184, 24);
      this.cmbService.TabIndex = 1;
      this.cmbService.SelectionChangeCommitted += new EventHandler(this.cmbService_SelectionChangeCommitted);
      this.mpCurrentPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.mpCurrentPeriod.CustomFormat = "MMMM yyyy";
      this.mpCurrentPeriod.Format = DateTimePickerFormat.Custom;
      this.mpCurrentPeriod.Location = new Point(895, 6);
      this.mpCurrentPeriod.Name = "mpCurrentPeriod";
      this.mpCurrentPeriod.OldMonth = 0;
      this.mpCurrentPeriod.ShowUpDown = true;
      this.mpCurrentPeriod.Size = new Size(131, 22);
      this.mpCurrentPeriod.TabIndex = 0;
      this.mpCurrentPeriod.ValueChanged += new EventHandler(this.mpCurrentPeriod_ValueChanged);
      this.clbListHomes.Dock = DockStyle.Fill;
      this.clbListHomes.FormattingEnabled = true;
      this.clbListHomes.Location = new Point(0, 70);
      this.clbListHomes.Name = "clbListHomes";
      this.clbListHomes.Size = new Size(1038, 226);
      this.clbListHomes.TabIndex = 3;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(1038, 336);
      this.Controls.Add((Control) this.clbListHomes);
      this.Controls.Add((Control) this.pnUp);
      this.Controls.Add((Control) this.pnBtn);
      this.Margin = new Padding(3, 2, 3, 2);
      this.Name = "FrmQuarterSum";
      this.StartPosition = FormStartPosition.CenterParent;
      this.Shown += new EventHandler(this.FrmQuarterSum_Shown);
      this.pnBtn.ResumeLayout(false);
      this.pnUp.ResumeLayout(false);
      this.pnUp.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
