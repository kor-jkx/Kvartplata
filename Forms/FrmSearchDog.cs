// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmSearchDog
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
using System.Globalization;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmSearchDog : FrmBaseForm
  {
    private FormStateSaver fss = new FormStateSaver(FrmSearchDog.container);
    private IContainer components = (IContainer) null;
    private ISession session;
    private static IContainer container;
    private LsClient client;
    private InputLanguage il;
    private Panel pnBtn;
    private Button btnExit;
    private Label lblNum;
    private TextBox txbNum;
    private DataGridView dgvDogovor;
    private Label lblOrg;
    private TextBox txbOrg;
    private CheckBox cbActive;
    private CheckBox cbArchive;
    private Label lblDate;
    private MaskedTextBox mtbDate;
    private MaskedTextBox mtbDBeg;
    private Label lblDBeg;
    private MaskedTextBox mtbDEnd;
    private Label lblDEnd;
    private Label label1;
    private Label lblkol;
    private Button btnОК;
    private ComboBox cmbHome;
    private Label lblHome;
    private Label lblStreet;
    private ComboBox cmbStreet;
    private ComboBox cmbBuiltIn;
    private Label lblBuiltin;
    private Label lblUsesType;
    private ComboBox cmbUsesType;
    private Label lblRightDoc;
    private ComboBox cmbRightDoc;
    private Button btnClear;

    public LsClient Client
    {
      get
      {
        return this.client;
      }
    }

    public FrmSearchDog()
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      InputLanguage.CurrentInputLanguage = this.il;
      this.Close();
    }

    private void FrmSearchDog_Shown(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      this.il = InputLanguage.CurrentInputLanguage;
      InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("ru-RU"));
      this.LoadAdr();
      this.LoadBuiltIn();
      this.LoadUsesType();
      this.LoadRightDoc();
      this.txbNum_TextChanged(sender, e);
      this.txbNum.Focus();
    }

    private void txbNum_TextChanged(object sender, EventArgs e)
    {
      string str = "";
      DateTime dt1 = DateTime.Now;
      DateTime dt2 = DateTime.Now;
      DateTime dt3 = DateTime.Now;
      if (this.cbActive.Checked)
        str += " and (select p.ParamValue from ClientParam p where p.ClientId=a.LsClient.ClientId and p.Param.ParamId=107 and p.DBeg<='{0}' and p.DEnd>'{0}' and p.Period.PeriodId=0) not in (4,5)";
      if (this.cbArchive.Checked)
        str += " and (select p.ParamValue from ClientParam p where p.ClientId=a.LsClient.ClientId and p.Param.ParamId=107 and p.DBeg<='{0}' and p.DEnd>'{0}' and p.Period.PeriodId=0) in (4,5)";
      if (this.mtbDate.Text != "  .  ." && this.mtbDate.Text.Length == 10)
      {
        try
        {
          dt1 = Convert.ToDateTime(this.mtbDate.Text);
          str += " and a.DogovorDate='{1}'";
          dt2 = Convert.ToDateTime(this.mtbDBeg.Text);
          dt3 = Convert.ToDateTime(this.mtbDEnd.Text);
        }
        catch
        {
          return;
        }
      }
      if (this.mtbDBeg.Text != "  .  ." && this.mtbDBeg.Text.Length == 10)
      {
        try
        {
          dt2 = Convert.ToDateTime(this.mtbDBeg.Text);
          str += " and a.DBeg='{2}'";
        }
        catch
        {
          return;
        }
      }
      if (this.mtbDEnd.Text != "  .  ." && this.mtbDEnd.Text.Length == 10)
      {
        try
        {
          dt3 = Convert.ToDateTime(this.mtbDEnd.Text);
          str += " and a.DEnd='{3}'";
        }
        catch
        {
          return;
        }
      }
      if (this.cmbStreet.SelectedIndex > 0)
        str = this.cmbHome.SelectedIndex != 0 ? str + string.Format(" and a.LsClient.ClientId in (select ls.ClientId from LsClient ls,Home h where ls.Home=h and h.Str.IdStr={0} and h.IdHome={1})", this.cmbStreet.SelectedValue, this.cmbHome.SelectedValue) : str + string.Format(" and a.LsClient.ClientId in (select ls.ClientId from LsClient ls,Home h where ls.Home=h and h.Str.IdStr={0})", this.cmbStreet.SelectedValue);
      if (this.cmbUsesType.SelectedIndex > 0)
        str += string.Format(" and (select p.ParamValue from ClientParam p where p.ClientId=a.LsClient.ClientId and p.Param.ParamId=512 and p.DBeg<='{0}' and p.DEnd>'{0}' and p.Period.PeriodId=0)={1}", (object) KvrplHelper.DateToBaseFormat(DateTime.Now), this.cmbUsesType.SelectedValue);
      if (this.cmbBuiltIn.SelectedIndex > 0)
        str += string.Format(" and (select p.ParamValue from ClientParam p where p.ClientId=a.LsClient.ClientId and p.Param.ParamId=513 and p.DBeg<='{0}' and p.DEnd>'{0}' and p.Period.PeriodId=0)={1}", (object) KvrplHelper.DateToBaseFormat(DateTime.Now), this.cmbBuiltIn.SelectedValue);
      if (this.cmbRightDoc.SelectedIndex > 0)
        str += string.Format(" and (select p.ParamValue from ClientParam p where p.ClientId=a.LsClient.ClientId and p.Param.ParamId=104 and p.DBeg<='{0}' and p.DEnd>'{0}' and p.Period.PeriodId=0)={1}", (object) KvrplHelper.DateToBaseFormat(DateTime.Now), this.cmbRightDoc.SelectedValue);
      IList<LsArenda> lsArendaList = this.session.CreateQuery(string.Format("select new LsArenda(a.LsClient,a.DogovorNum,a.BaseOrg,a.DogovorDate) from LsArenda a,BaseOrg b where a.BaseOrg.BaseOrgId=b.BaseOrgId and a.DogovorNum like '%" + this.txbNum.Text + "%' and b.NameOrgMin like '%" + this.txbOrg.Text + "%'" + str + " order by a.DogovorNum,a.LsClient.ClientId", (object) KvrplHelper.DateToBaseFormat(DateTime.Now), (object) KvrplHelper.DateToBaseFormat(dt1), (object) KvrplHelper.DateToBaseFormat(dt2), (object) KvrplHelper.DateToBaseFormat(dt3))).List<LsArenda>();
      IList list = this.session.CreateQuery(string.Format("select (select p.ParamValue from ClientParam p where p.ClientId=a.LsClient.ClientId and p.Param.ParamId=107 and p.DBeg<='{0}' and p.DEnd>'{0}' and p.Period.PeriodId=0) as pr from LsArenda a ,BaseOrg b where a.BaseOrg.BaseOrgId=b.BaseOrgId and a.DogovorNum like '%" + this.txbNum.Text + "%' and b.NameOrgMin like '%" + this.txbOrg.Text + "%'" + str + " order by a.DogovorNum,a.LsClient.ClientId", (object) KvrplHelper.DateToBaseFormat(DateTime.Now), (object) KvrplHelper.DateToBaseFormat(dt1), (object) KvrplHelper.DateToBaseFormat(dt2), (object) KvrplHelper.DateToBaseFormat(dt3))).List();
      short num = 0;
      foreach (LsArenda lsArenda in (IEnumerable<LsArenda>) lsArendaList)
      {
        lsArenda.Status = Convert.ToInt32(list[(int) num]);
        ++num;
      }
      this.lblkol.Text = lsArendaList.Count.ToString();
      this.dgvDogovor.DataSource = (object) null;
      this.dgvDogovor.Columns.Clear();
      this.dgvDogovor.DataSource = (object) lsArendaList;
      this.SetViewDogovor();
    }

    private void SetViewDogovor()
    {
      this.dgvDogovor.Columns["DogovorNum"].HeaderText = "Номер договора";
      this.dgvDogovor.Columns["NameOrg"].HeaderText = "Организация";
      this.dgvDogovor.Columns["NumLs"].HeaderText = "Лицевой";
      this.dgvDogovor.Columns["NumLs"].DisplayIndex = 1;
      this.dgvDogovor.Columns["KumiNum"].Visible = false;
      this.dgvDogovor.Columns["Status"].Visible = false;
      this.dgvDogovor.Columns["Balance"].Visible = false;
      this.dgvDogovor.Columns["Peni"].Visible = false;
      this.dgvDogovor.Columns["Rent"].Visible = false;
      this.dgvDogovor.Columns["RentPeni"].Visible = false;
      this.dgvDogovor.Columns["Months"].Visible = false;
      KvrplHelper.AddCalendarColumn(this.dgvDogovor, 2, "Дата заключения", "Date");
      this.dgvDogovor.Columns["NameOrg"].Width = 400;
      foreach (DataGridViewRow row in (IEnumerable) this.dgvDogovor.Rows)
        row.Cells["Date"].Value = (object) ((LsArenda) row.DataBoundItem).DogovorDate;
    }

    private void dgvDogovor_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      this.ShowLsClient();
    }

    private void ShowLsClient()
    {
      if (this.dgvDogovor.Rows.Count <= 0 || this.dgvDogovor.CurrentRow == null)
        return;
      try
      {
        this.session = Domain.CurrentSession;
        this.client = this.session.Get<LsClient>((object) ((LsArenda) this.dgvDogovor.CurrentRow.DataBoundItem).LsClient.ClientId);
        this.session.Clear();
        this.Close();
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void dgvDogovor_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      DataGridViewRow row = (sender as DataGridView).Rows[e.RowIndex];
      try
      {
        if (((LsArenda) row.DataBoundItem).Status != 4 && ((LsArenda) row.DataBoundItem).Status != 5)
          row.DefaultCellStyle.ForeColor = Color.Black;
        else
          row.DefaultCellStyle.ForeColor = Color.Gray;
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void dgvDogovor_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.ShowLsClient();
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
      this.ShowLsClient();
    }

    private void label1_Click(object sender, EventArgs e)
    {
    }

    private void LoadAdr()
    {
      IList<Str> strList = this.session.CreateQuery(string.Format("select s from Str s where IdStr in (select Str.IdStr from Home where IdHome in (select Home.IdHome from LsClient ls where 1=1 " + Options.MainConditions1 + ")) order by NameStr")).List<Str>();
      strList.Insert(0, new Str()
      {
        IdStr = 0,
        NameStr = ""
      });
      this.cmbStreet.DataSource = (object) strList;
      this.cmbStreet.DisplayMember = "NameStr";
      this.cmbStreet.ValueMember = "IdStr";
    }

    private void cmbStreet_SelectedValueChanged(object sender, EventArgs e)
    {
      if (!(this.cmbStreet.Text != ""))
        return;
      this.cmbStreet_SelectionChangeCommitted((object) null, (EventArgs) null);
      this.txbNum_TextChanged((object) null, (EventArgs) null);
      this.cmbHome.Focus();
    }

    private void cmbStreet_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.cmbHome.DataSource = (object) null;
      this.session = Domain.CurrentSession;
      Str selectedItem = (Str) this.cmbStreet.SelectedItem;
      IList<Home> homeList = this.session.CreateQuery(string.Format("select distinct new Home(h.IdHome,h.NHome,h.HomeKorp) from Home h where h.Str.IdStr={0} order by DBA.LENGTHHOME(h.NHome),DBA.LENGTHHOME(h.HomeKorp)", (object) (selectedItem != null ? selectedItem.IdStr : 0))).List<Home>();
      homeList.Insert(0, new Home()
      {
        IdHome = 0,
        NHome = ""
      });
      this.cmbHome.DataSource = (object) homeList;
      this.cmbHome.DisplayMember = "NHome";
      this.cmbHome.ValueMember = "IdHome";
      this.session.Clear();
    }

    private void cmbHome_SelectedValueChanged(object sender, EventArgs e)
    {
      if (!(this.cmbHome.Text != "") || !(this.cmbHome.Text != "Kvartplata.Classes.Home"))
        return;
      this.cmbHome_SelectionChangeCommitted((object) null, (EventArgs) null);
    }

    private void cmbHome_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.txbNum_TextChanged((object) null, (EventArgs) null);
    }

    private void LoadBuiltIn()
    {
      IList<BuiltIn> builtInList = this.session.CreateCriteria(typeof (BuiltIn)).AddOrder(Order.Asc("BuiltInId")).List<BuiltIn>();
      builtInList.Insert(0, new BuiltIn()
      {
        BuiltInId = (short) 0,
        BuiltInName = ""
      });
      this.cmbBuiltIn.ValueMember = "BuiltInId";
      this.cmbBuiltIn.DisplayMember = "BuiltInName";
      this.cmbBuiltIn.DataSource = (object) builtInList;
    }

    private void LoadUsesType()
    {
      IList<UsesType> usesTypeList = this.session.CreateCriteria(typeof (UsesType)).AddOrder(Order.Asc("UsesTypeName")).List<UsesType>();
      usesTypeList.Insert(0, new UsesType()
      {
        UsesTypeId = (short) 0,
        UsesTypeName = ""
      });
      this.cmbUsesType.ValueMember = "UsesTypeId";
      this.cmbUsesType.DisplayMember = "UsesTypeName";
      this.cmbUsesType.DataSource = (object) usesTypeList;
    }

    private void LoadRightDoc()
    {
      IList<RightDoc> rightDocList = this.session.CreateCriteria(typeof (RightDoc)).AddOrder(Order.Asc("RightDocId")).List<RightDoc>();
      rightDocList.Insert(0, new RightDoc()
      {
        RightDocId = (short) 0,
        RightDocName = ""
      });
      this.cmbRightDoc.ValueMember = "RightDocId";
      this.cmbRightDoc.DisplayMember = "RightDocName";
      this.cmbRightDoc.DataSource = (object) rightDocList;
    }

    private void cmbUsesType_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.txbNum_TextChanged(sender, e);
    }

    private void cmbBuiltIn_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.txbNum_TextChanged(sender, e);
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      this.txbNum.Clear();
      this.txbOrg.Clear();
      this.cbActive.Checked = false;
      this.cbArchive.Checked = false;
      this.mtbDate.Clear();
      this.mtbDBeg.Clear();
      this.mtbDBeg.Clear();
      this.cmbStreet.SelectedValue = (object) 0;
      this.cmbHome.SelectedValue = (object) 0;
      this.cmbRightDoc.SelectedValue = (object) 0;
      this.cmbUsesType.SelectedValue = (object) 0;
      this.cmbBuiltIn.SelectedValue = (object) 0;
      this.txbNum_TextChanged(sender, e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmSearchDog));
      this.pnBtn = new Panel();
      this.btnОК = new Button();
      this.btnExit = new Button();
      this.lblNum = new Label();
      this.txbNum = new TextBox();
      this.dgvDogovor = new DataGridView();
      this.lblOrg = new Label();
      this.txbOrg = new TextBox();
      this.cbActive = new CheckBox();
      this.cbArchive = new CheckBox();
      this.lblDate = new Label();
      this.mtbDate = new MaskedTextBox();
      this.mtbDBeg = new MaskedTextBox();
      this.lblDBeg = new Label();
      this.mtbDEnd = new MaskedTextBox();
      this.lblDEnd = new Label();
      this.label1 = new Label();
      this.lblkol = new Label();
      this.cmbHome = new ComboBox();
      this.lblHome = new Label();
      this.lblStreet = new Label();
      this.cmbStreet = new ComboBox();
      this.cmbBuiltIn = new ComboBox();
      this.lblBuiltin = new Label();
      this.lblUsesType = new Label();
      this.cmbUsesType = new ComboBox();
      this.lblRightDoc = new Label();
      this.cmbRightDoc = new ComboBox();
      this.btnClear = new Button();
      this.pnBtn.SuspendLayout();
      ((ISupportInitialize) this.dgvDogovor).BeginInit();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnClear);
      this.pnBtn.Controls.Add((Control) this.btnОК);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 497);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(797, 40);
      this.pnBtn.TabIndex = 11;
      this.btnОК.Image = (Image) Resources.Tick;
      this.btnОК.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnОК.Location = new Point(12, 5);
      this.btnОК.Name = "btnОК";
      this.btnОК.Size = new Size(59, 30);
      this.btnОК.TabIndex = 1;
      this.btnОК.Text = "ОК";
      this.btnОК.TextAlign = ContentAlignment.MiddleRight;
      this.btnОК.UseVisualStyleBackColor = true;
      this.btnОК.Click += new EventHandler(this.btnSearch_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(702, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(83, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.lblNum.AutoSize = true;
      this.lblNum.Location = new Point(9, 9);
      this.lblNum.Name = "lblNum";
      this.lblNum.Size = new Size(116, 16);
      this.lblNum.TabIndex = 1;
      this.lblNum.Text = "Номер договора";
      this.txbNum.Location = new Point(134, 6);
      this.txbNum.Name = "txbNum";
      this.txbNum.Size = new Size(168, 22);
      this.txbNum.TabIndex = 0;
      this.txbNum.TextChanged += new EventHandler(this.txbNum_TextChanged);
      this.dgvDogovor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dgvDogovor.BackgroundColor = Color.AliceBlue;
      this.dgvDogovor.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvDogovor.Location = new Point(0, 164);
      this.dgvDogovor.Name = "dgvDogovor";
      this.dgvDogovor.ReadOnly = true;
      this.dgvDogovor.Size = new Size(797, 331);
      this.dgvDogovor.TabIndex = 10;
      this.dgvDogovor.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvDogovor_CellFormatting);
      this.dgvDogovor.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dgvDogovor_CellMouseDoubleClick);
      this.dgvDogovor.KeyDown += new KeyEventHandler(this.dgvDogovor_KeyDown);
      this.lblOrg.AutoSize = true;
      this.lblOrg.Location = new Point(12, 34);
      this.lblOrg.Name = "lblOrg";
      this.lblOrg.Size = new Size(80, 16);
      this.lblOrg.TabIndex = 4;
      this.lblOrg.Text = "Арендатор";
      this.txbOrg.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txbOrg.Location = new Point(134, 31);
      this.txbOrg.Name = "txbOrg";
      this.txbOrg.Size = new Size(651, 22);
      this.txbOrg.TabIndex = 1;
      this.txbOrg.TextChanged += new EventHandler(this.txbNum_TextChanged);
      this.cbActive.AutoSize = true;
      this.cbActive.Location = new Point(12, 58);
      this.cbActive.Name = "cbActive";
      this.cbActive.Size = new Size(117, 20);
      this.cbActive.TabIndex = 20;
      this.cbActive.Text = "Действующие";
      this.cbActive.UseVisualStyleBackColor = true;
      this.cbActive.CheckedChanged += new EventHandler(this.txbNum_TextChanged);
      this.cbArchive.AutoSize = true;
      this.cbArchive.Location = new Point(135, 58);
      this.cbArchive.Name = "cbArchive";
      this.cbArchive.Size = new Size(91, 20);
      this.cbArchive.TabIndex = 21;
      this.cbArchive.Text = "Архивные";
      this.cbArchive.UseVisualStyleBackColor = true;
      this.cbArchive.CheckedChanged += new EventHandler(this.txbNum_TextChanged);
      this.lblDate.AutoSize = true;
      this.lblDate.Location = new Point(9, 81);
      this.lblDate.Name = "lblDate";
      this.lblDate.Size = new Size(188, 16);
      this.lblDate.TabIndex = 7;
      this.lblDate.Text = "Дата заключения договора";
      this.mtbDate.Location = new Point(193, 78);
      this.mtbDate.Mask = "00/00/0000";
      this.mtbDate.Name = "mtbDate";
      this.mtbDate.Size = new Size(78, 22);
      this.mtbDate.TabIndex = 2;
      this.mtbDate.ValidatingType = typeof (DateTime);
      this.mtbDate.TextChanged += new EventHandler(this.txbNum_TextChanged);
      this.mtbDBeg.Location = new Point(438, 78);
      this.mtbDBeg.Mask = "00/00/0000";
      this.mtbDBeg.Name = "mtbDBeg";
      this.mtbDBeg.Size = new Size(78, 22);
      this.mtbDBeg.TabIndex = 3;
      this.mtbDBeg.ValidatingType = typeof (DateTime);
      this.mtbDBeg.TextChanged += new EventHandler(this.txbNum_TextChanged);
      this.lblDBeg.AutoSize = true;
      this.lblDBeg.Location = new Point(287, 81);
      this.lblDBeg.Name = "lblDBeg";
      this.lblDBeg.Size = new Size(155, 16);
      this.lblDBeg.TabIndex = 10;
      this.lblDBeg.Text = "Дата начала действия";
      this.mtbDEnd.Location = new Point(709, 78);
      this.mtbDEnd.Mask = "00/00/0000";
      this.mtbDEnd.Name = "mtbDEnd";
      this.mtbDEnd.Size = new Size(78, 22);
      this.mtbDEnd.TabIndex = 4;
      this.mtbDEnd.ValidatingType = typeof (DateTime);
      this.mtbDEnd.TextChanged += new EventHandler(this.txbNum_TextChanged);
      this.lblDEnd.AutoSize = true;
      this.lblDEnd.Location = new Point(536, 81);
      this.lblDEnd.Name = "lblDEnd";
      this.lblDEnd.Size = new Size(177, 16);
      this.lblDEnd.TabIndex = 12;
      this.lblDEnd.Text = "Дата окончания действия";
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.label1.Location = new Point(581, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(168, 16);
      this.label1.TabIndex = 13;
      this.label1.Text = "Показано договоров:";
      this.label1.Click += new EventHandler(this.label1_Click);
      this.lblkol.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblkol.AutoSize = true;
      this.lblkol.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblkol.Location = new Point(745, 9);
      this.lblkol.Name = "lblkol";
      this.lblkol.Size = new Size(16, 16);
      this.lblkol.TabIndex = 14;
      this.lblkol.Text = "0";
      this.cmbHome.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
      this.cmbHome.AutoCompleteSource = AutoCompleteSource.ListItems;
      this.cmbHome.FormattingEnabled = true;
      this.cmbHome.Location = new Point(362, 106);
      this.cmbHome.Name = "cmbHome";
      this.cmbHome.Size = new Size(93, 24);
      this.cmbHome.TabIndex = 6;
      this.cmbHome.SelectionChangeCommitted += new EventHandler(this.cmbHome_SelectionChangeCommitted);
      this.cmbHome.SelectedValueChanged += new EventHandler(this.cmbHome_SelectedValueChanged);
      this.lblHome.AutoSize = true;
      this.lblHome.Location = new Point(322, 109);
      this.lblHome.Name = "lblHome";
      this.lblHome.Size = new Size(34, 16);
      this.lblHome.TabIndex = 17;
      this.lblHome.Text = "Дом";
      this.lblStreet.AutoSize = true;
      this.lblStreet.Location = new Point(9, 109);
      this.lblStreet.Name = "lblStreet";
      this.lblStreet.Size = new Size(49, 16);
      this.lblStreet.TabIndex = 16;
      this.lblStreet.Text = "Улица";
      this.cmbStreet.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
      this.cmbStreet.AutoCompleteSource = AutoCompleteSource.ListItems;
      this.cmbStreet.Location = new Point(64, 106);
      this.cmbStreet.Name = "cmbStreet";
      this.cmbStreet.Size = new Size(249, 24);
      this.cmbStreet.TabIndex = 5;
      this.cmbStreet.SelectionChangeCommitted += new EventHandler(this.cmbStreet_SelectionChangeCommitted);
      this.cmbStreet.SelectedValueChanged += new EventHandler(this.cmbStreet_SelectedValueChanged);
      this.cmbBuiltIn.FormattingEnabled = true;
      this.cmbBuiltIn.Location = new Point(591, 133);
      this.cmbBuiltIn.Name = "cmbBuiltIn";
      this.cmbBuiltIn.Size = new Size(187, 24);
      this.cmbBuiltIn.TabIndex = 9;
      this.cmbBuiltIn.SelectionChangeCommitted += new EventHandler(this.cmbBuiltIn_SelectionChangeCommitted);
      this.lblBuiltin.AutoSize = true;
      this.lblBuiltin.Location = new Point(457, 136);
      this.lblBuiltin.Name = "lblBuiltin";
      this.lblBuiltin.Size = new Size(128, 16);
      this.lblBuiltin.TabIndex = 20;
      this.lblBuiltin.Text = "Тип встроенности";
      this.lblUsesType.AutoSize = true;
      this.lblUsesType.Location = new Point(9, 136);
      this.lblUsesType.Name = "lblUsesType";
      this.lblUsesType.Size = new Size(137, 16);
      this.lblUsesType.TabIndex = 21;
      this.lblUsesType.Text = "Вид использования";
      this.cmbUsesType.FormattingEnabled = true;
      this.cmbUsesType.Location = new Point(152, 133);
      this.cmbUsesType.Name = "cmbUsesType";
      this.cmbUsesType.Size = new Size(303, 24);
      this.cmbUsesType.TabIndex = 8;
      this.cmbUsesType.SelectionChangeCommitted += new EventHandler(this.cmbUsesType_SelectionChangeCommitted);
      this.lblRightDoc.AutoSize = true;
      this.lblRightDoc.Location = new Point(457, 109);
      this.lblRightDoc.Name = "lblRightDoc";
      this.lblRightDoc.Size = new Size(135, 16);
      this.lblRightDoc.TabIndex = 22;
      this.lblRightDoc.Text = "Вид собственности";
      this.cmbRightDoc.FormattingEnabled = true;
      this.cmbRightDoc.Location = new Point(591, 106);
      this.cmbRightDoc.Name = "cmbRightDoc";
      this.cmbRightDoc.Size = new Size(187, 24);
      this.cmbRightDoc.TabIndex = 7;
      this.cmbRightDoc.SelectedValueChanged += new EventHandler(this.cmbBuiltIn_SelectionChangeCommitted);
      this.btnClear.Image = (Image) componentResourceManager.GetObject("btnClear.Image");
      this.btnClear.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnClear.Location = new Point(87, 5);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new Size(139, 30);
      this.btnClear.TabIndex = 2;
      this.btnClear.Text = "Очистить поля";
      this.btnClear.TextAlign = ContentAlignment.MiddleRight;
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new EventHandler(this.btnClear_Click);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(797, 537);
      this.Controls.Add((Control) this.cmbRightDoc);
      this.Controls.Add((Control) this.lblRightDoc);
      this.Controls.Add((Control) this.cmbUsesType);
      this.Controls.Add((Control) this.lblUsesType);
      this.Controls.Add((Control) this.lblBuiltin);
      this.Controls.Add((Control) this.cmbBuiltIn);
      this.Controls.Add((Control) this.cmbHome);
      this.Controls.Add((Control) this.lblHome);
      this.Controls.Add((Control) this.lblStreet);
      this.Controls.Add((Control) this.cmbStreet);
      this.Controls.Add((Control) this.lblkol);
      this.Controls.Add((Control) this.mtbDEnd);
      this.Controls.Add((Control) this.lblDEnd);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.mtbDBeg);
      this.Controls.Add((Control) this.lblDBeg);
      this.Controls.Add((Control) this.mtbDate);
      this.Controls.Add((Control) this.lblDate);
      this.Controls.Add((Control) this.cbArchive);
      this.Controls.Add((Control) this.cbActive);
      this.Controls.Add((Control) this.txbOrg);
      this.Controls.Add((Control) this.lblOrg);
      this.Controls.Add((Control) this.dgvDogovor);
      this.Controls.Add((Control) this.txbNum);
      this.Controls.Add((Control) this.lblNum);
      this.Controls.Add((Control) this.pnBtn);
      this.Name = "FrmSearchDog";
      this.Text = "Поиск договора";
      this.Shown += new EventHandler(this.FrmSearchDog_Shown);
      this.pnBtn.ResumeLayout(false);
      ((ISupportInitialize) this.dgvDogovor).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
