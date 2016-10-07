// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmBigSearch
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
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmBigSearch : Form
  {
    private static IContainer container = (IContainer) null;
    private FormStateSaver formStateSaver = new FormStateSaver(FrmBigSearch.container);
    private IContainer components = (IContainer) null;
    public LsClient client;
    private ICriteria criteria;
    private ISession session;
    private Panel pnBtn;
    private Button btnExit;
    private Button btnOk;
    private GroupBox gbAddress;
    private GroupBox gbFIO;
    private DataGridView dgvResult;
    private Label lblStreet;
    private ComboBox cmbStreet;
    private ComboBox cmbHome;
    private Label lblHome;
    private ComboBox cmbFlat;
    private Label lblFlat;
    private Button btnSearch;
    private CheckBox cbClose;
    private TextBox txtName;
    private Label lblName;
    private Label lblFamily;
    private TextBox txtFamily;
    private CheckBox cbArchive;
    private TextBox txtFName;
    private Label lblFName;
    private GroupBox gbType;
    private RadioButton rbVhod;
    private RadioButton rbZnach;
    public HelpProvider hp;
    private Label lblRoom;
    private ComboBox cmbRoom;
    private GroupBox gbHomes;
    private RadioButton rbAll;
    private RadioButton rbActive;
    private RadioButton rbArchive;
    private Button btnJumpToHome;
    private ComboBox cmbCity;
    private Label lblCity;
    private Label cityError;

    public Home CurrentHome { get; set; }

    public IList<Str> listStreet { get; set; }

    public FrmBigSearch()
    {
      this.InitializeComponent();
      this.formStateSaver.ParentForm = (Form) this;
    }

    private void FrmBigSearch_Shown(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      IList<City> cityList = (IList<City>) new List<City>();
      cityList.Add(new City()
      {
        CityId = 0,
        CityName = ""
      });
      foreach (IList list in (IEnumerable) this.session.CreateSQLQuery("select distinct ATO +' '+ABBR,NUMSTR from DI_SOATO_RU where NUMSTR in ( select distinct idcity from DI_STR) order by 1").List())
        cityList.Add(new City()
        {
          CityId = Convert.ToInt32(list[1].ToString()),
          CityName = list[0].ToString()
        });
      this.cmbCity.DataSource = (object) cityList;
      this.cmbCity.DisplayMember = "CityName";
      this.cmbCity.ValueMember = "CityId";
      this.session.Clear();
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
      Str str1 = new Str();
      Home home = new Home();
      Flat flat = new Flat();
      string str2 = "";
      string str3 = "";
      string str4 = "";
      string str5 = "";
      string str6 = "";
      if (Convert.ToInt32(this.cmbStreet.SelectedValue) == 0 && this.txtFamily.Text == "" && this.txtName.Text == "" && this.txtFName.Text == "")
      {
        int num = (int) MessageBox.Show("Нет данных для поиска", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.Cursor = Cursors.WaitCursor;
        this.session = Domain.CurrentSession;
        DataTable dataTable = new DataTable("Result");
        dataTable.Columns.Add("Улица", System.Type.GetType("System.String"));
        dataTable.Columns.Add("Дом", System.Type.GetType("System.String"));
        dataTable.Columns.Add("Корпус", System.Type.GetType("System.String"));
        dataTable.Columns.Add("Квартира", System.Type.GetType("System.String"));
        dataTable.Columns.Add("Комната", System.Type.GetType("System.String"));
        dataTable.Columns.Add("Лицевой счет", System.Type.GetType("System.Int32"));
        dataTable.Columns.Add("Статус", System.Type.GetType("System.Int32"));
        dataTable.Columns.Add("", System.Type.GetType("System.String"));
        dataTable.Columns.Add("ФИО", System.Type.GetType("System.String"));
        if ((uint) Convert.ToInt32(this.cmbStreet.SelectedValue) > 0U)
        {
          str1 = (Str) this.cmbStreet.SelectedItem;
          str2 = " and h.idstr={0}";
        }
        if ((uint) Convert.ToInt32(this.cmbHome.SelectedValue) > 0U)
        {
          home = (Home) this.cmbHome.SelectedItem;
          str2 += " and h.idhome={1}";
        }
        if ((uint) Convert.ToInt32(this.cmbFlat.SelectedValue) > 0U)
        {
          flat = (Flat) this.cmbFlat.SelectedItem;
          str2 += " and ls.idflat={2}";
        }
        if ((LsClient) this.cmbRoom.SelectedItem != null && ((LsClient) this.cmbRoom.SelectedItem).SurFlat != "")
          str2 += string.Format(" and ls.numberroom={0}", (object) ((LsClient) this.cmbRoom.SelectedItem).SurFlat);
        string str7 = !this.cbClose.Checked ? str2 + " and cp.param_value in (0,1,2,3)" : str2 + " and cp.param_value in (0,1,2,3,4,5)";
        string str8;
        if (this.txtFamily.Text != "" || this.txtName.Text != "" || this.txtFName.Text != "")
        {
          if (this.session.CreateSQLQuery("select * from systable where table_name='frPers'").List().Count == 0)
          {
            str5 = " ,dba.form_a p";
            str6 = " ,dba.owners p";
            str3 = "  and p.idlic=ls.client_id ";
            str4 = "  and p.idlic=ls.client_id ";
            str8 = ",p.family||' '||p.name||' '||p.lastName,p.archive ";
            if (!this.cbArchive.Checked)
              str7 += " and p.archive in (0,5) ";
          }
          else
          {
            str5 = " ,dba.frPers p,dba.form_a fr";
            str6 = " ,dba.frPers p,dba.owners fr";
            str3 = " and fr.idlic=ls.client_id and p.id=fr.idform and p.code=1";
            str4 = " and fr.idlic=ls.client_id and p.id=fr.owner and p.code=2";
            str8 = ",p.family||' '||p.name||' '||p.lastName,fr.archive";
            if (!this.cbArchive.Checked)
              str7 += " and fr.archive in (0,5) ";
          }
          dataTable.Columns.Add("Архив", System.Type.GetType("System.String"));
          if (this.rbZnach.Checked)
          {
            if (this.txtFamily.Text != "")
              str7 = str7 + " and p.family='" + this.txtFamily.Text + "'";
            if (this.txtName.Text != "")
              str7 = str7 + " and p.name='" + this.txtName.Text + "'";
            if (this.txtFName.Text != "")
              str7 = str7 + " and p.lastName='" + this.txtFName.Text + "'";
          }
          else
            str7 = str7 + " and p.family like '%" + this.txtFamily.Text + "%'" + " and p.name like '%" + this.txtName.Text + "%'" + " and p.lastName like '%" + this.txtFName.Text + "%'";
        }
        else
          str8 = ",null as fio";
        dataTable.Columns.Add("К оплате на текущий момент", System.Type.GetType("System.Decimal"));
        dataTable.Columns.Add("Управляющая компания", System.Type.GetType("System.String"));
        IList<LsClient> lsClientList = (IList<LsClient>) new List<LsClient>();
        foreach (object[] objArray in (IEnumerable) this.session.CreateSQLQuery(string.Format("select s.str,h.home,dba.lengthhome(h.home_korp),dba.lengthhome(f.nflat),dba.lengthhome(ls.numberroom),ls.client_id,cp.param_value,dba.lengthhome(h.home)" + str8 + ",isnull((select sum(balance_out) from lsBalance where period_id=(select period_id from cmpPeriod where company_id=ls.Company_id and complex_id=100) and client_id=ls.client_id),0) as debt, (select nameorg_min from Base_Org b,dcCompany dc where dc.manager_id=b.idbaseorg and company_id=ls.company_id) as cmpname  from dba.lsClient ls,dba.Homes h,dba.Flats f,dba.di_Str s,dba.lsParam cp" + str5 + "  where h.idStr=s.idstr and ls.idhome=h.idhome and ls.idflat=f.idflat and cp.client_id=ls.client_id and cp.param_id=107 and cp.DBeg<='{3}' and cp.DEnd>'{3}' " + str3 + str7 + Options.MainConditions2 + " union select s.str,h.home,dba.lengthhome(h.home_korp),dba.lengthhome(f.nflat),dba.lengthhome(ls.numberroom),ls.client_id,cp.param_value,dba.lengthhome(h.home)" + str8 + ",isnull((select sum(balance_out) from lsBalance where period_id=(select period_id from cmpPeriod where company_id=ls.Company_id and complex_id=100) and client_id=ls.client_id),0) as debt, (select nameorg_min from Base_Org b,dcCompany dc where dc.manager_id=b.idbaseorg and company_id=ls.company_id) as cmpname  from dba.lsClient ls,dba.Homes h,dba.Flats f,dba.di_Str s,dba.lsParam cp" + str6 + "  where h.idStr=s.idstr and ls.idhome=h.idhome and ls.idflat=f.idflat and cp.client_id=ls.client_id and cp.param_id=107 and cp.DBeg<='{3}' and cp.DEnd>'{3}' " + str4 + str7 + Options.MainConditions2 + " order by 1,8,4,5,6", (object) str1.IdStr, (object) home.IdHome, (object) flat.IdFlat, (object) KvrplHelper.DateToBaseFormat(DateTime.Now))).List())
          dataTable.Rows.Add(objArray);
        this.dgvResult.Columns.Clear();
        this.dgvResult.DataSource = (object) null;
        this.dgvResult.DataSource = (object) dataTable;
        DataGridViewCellStyle gridViewCellStyle = new DataGridViewCellStyle();
        gridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        foreach (DataGridViewColumn column in (BaseCollection) this.dgvResult.Columns)
        {
          column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
          column.HeaderCell.Style = gridViewCellStyle;
          column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        if (this.txtFamily.Text == "" && this.txtName.Text == "" && this.txtFName.Text == "")
        {
          foreach (DataGridViewRow row in (IEnumerable) this.dgvResult.Rows)
          {
            if (row.Cells[5].Value != null)
              row.Cells[8].Value = (object) KvrplHelper.GetFio1(Convert.ToInt32(row.Cells[5].Value));
          }
        }
        this.dgvResult.Columns[0].Width = 250;
        this.dgvResult.Columns[8].Width = 250;
        this.dgvResult.Columns[this.dgvResult.Columns.Count - 1].Width = 300;
        this.dgvResult.Columns[6].Visible = false;
        this.dgvResult.Columns[7].Visible = false;
        if (this.dgvResult.Columns.Count == 12)
          this.dgvResult.Columns[9].Visible = false;
        if (!KvrplHelper.CheckProxy(48, 1, (Company) null, false))
          this.dgvResult.Columns[8].Visible = false;
        this.session.Clear();
        this.Cursor = Cursors.Default;
      }
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      this.ShowLsClient();
    }

    private void dgvResult_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.ShowLsClient();
    }

    private void dgvResult_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      this.ShowLsClient();
    }

    private void dgvResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      DataGridViewRow row = this.dgvResult.Rows[e.RowIndex];
      if (e.RowIndex >= this.dgvResult.Rows.Count - 1)
        return;
      if (Convert.ToInt32(row.Cells[6].Value) == 4 || Convert.ToInt32(row.Cells[6].Value) == 5)
        row.DefaultCellStyle.ForeColor = Color.DimGray;
      if (this.dgvResult.Columns.Count == 10 && Convert.ToInt32(row.Cells[9].Value) != 0 && Convert.ToInt32(row.Cells[9].Value) != 5)
        row.DefaultCellStyle.ForeColor = Color.DimGray;
    }

    private void dgvResult_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    private void cmbStreet_KeyPress(object sender, KeyPressEventArgs e)
    {
    }

    private void cmbStreet_KeyDown(object sender, KeyEventArgs e)
    {
    }

    private void cmbStreet_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.cmbHome.DataSource = (object) null;
      this.cmbFlat.DataSource = (object) null;
      this.session = Domain.CurrentSession;
      string str = "";
      Str selectedItem = (Str) this.cmbStreet.SelectedItem;
      if (this.rbActive.Checked)
        str += " and h.Archive=0";
      if (this.rbArchive.Checked)
        str += " and h.Archive=1";
      IList<Home> homeList = this.session.CreateQuery(string.Format("select distinct new Home(h.IdHome,h.NHome,h.HomeKorp) from Home h where h.Str.IdStr={0}  " + Options.HomeType + str + "order by DBA.LENGTHHOME(h.NHome),DBA.LENGTHHOME(h.HomeKorp)", (object) (selectedItem != null ? selectedItem.IdStr : 0))).List<Home>();
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

    private void cmbStreet_SelectedValueChanged(object sender, EventArgs e)
    {
      if (!(this.cmbStreet.Text != ""))
        return;
      this.cmbStreet_SelectionChangeCommitted((object) null, (EventArgs) null);
      this.cmbHome.Focus();
    }

    private void cmbHome_KeyPress(object sender, KeyPressEventArgs e)
    {
    }

    private void cmbHome_SelectedValueChanged(object sender, EventArgs e)
    {
      if (!(this.cmbHome.Text != "") || !(this.cmbHome.Text != "Kvartplata.Classes.Home"))
        return;
      this.cmbHome_SelectionChangeCommitted((object) null, (EventArgs) null);
      this.btnSearch_Click((object) null, (EventArgs) null);
      this.cmbFlat.Focus();
    }

    private void cmbHome_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.cmbFlat.DataSource = (object) null;
      this.session = Domain.CurrentSession;
      Home selectedItem = (Home) this.cmbHome.SelectedItem;
      IList<Flat> flatList = this.session.CreateQuery(string.Format("select f from Flat f where f.Home.IdHome={0} order by DBA.LENGTHHOME(f.NFlat)", (object) (selectedItem != null ? selectedItem.IdHome : 0))).List<Flat>();
      flatList.Insert(0, new Flat()
      {
        IdFlat = 0,
        NFlat = ""
      });
      this.cmbFlat.DataSource = (object) flatList;
      this.cmbFlat.DisplayMember = "NFlat";
      this.cmbFlat.ValueMember = "IdFlat";
      this.session.Clear();
    }

    private void cmbFlat_SelectedValueChanged(object sender, EventArgs e)
    {
      if (!(this.cmbFlat.Text != "") || !(this.cmbFlat.Text != "Kvartplata.Classes.Flat"))
        return;
      this.cmbFlat_SelectionChangeCommitted((object) null, (EventArgs) null);
      this.btnSearch_Click((object) null, (EventArgs) null);
      this.cmbRoom.Focus();
    }

    private void cmbFlat_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.cmbRoom.DataSource = (object) null;
      this.session = Domain.CurrentSession;
      Flat selectedItem = (Flat) this.cmbFlat.SelectedItem;
      IList<LsClient> lsClientList = this.session.CreateQuery(string.Format("from LsClient ls where ls.Flat.IdFlat={0} order by DBA.LENGTHHOME(ls.SurFlat)", (object) (selectedItem != null ? selectedItem.IdFlat : 0))).List<LsClient>();
      lsClientList.Insert(0, new LsClient(0, "", "", ""));
      this.cmbRoom.DataSource = (object) lsClientList;
      this.cmbRoom.DisplayMember = "SurFlat";
      this.cmbRoom.ValueMember = "ClientId";
      this.session.Clear();
    }

    private void cmbRoom_SelectedValueChanged(object sender, EventArgs e)
    {
      if (!(this.cmbRoom.Text != "") || !(this.cmbRoom.Text != "Kvartplata.Classes.LsClient"))
        return;
      this.btnSearch_Click((object) null, (EventArgs) null);
    }

    private void rbAll_CheckedChanged(object sender, EventArgs e)
    {
      this.cmbStreet_SelectionChangeCommitted((object) null, (EventArgs) null);
    }

    private void ShowLsClient()
    {
      if (this.dgvResult.Rows.Count <= 0 || this.dgvResult.CurrentRow == null)
        return;
      try
      {
        this.session = Domain.CurrentSession;
        this.client = this.session.Get<LsClient>(this.dgvResult.CurrentRow.Cells[5].Value);
        if ((this.client.Company == null ? Options.City : Convert.ToInt32(KvrplHelper.BaseValue(1, this.client.Company))) != 9)
        {
          this.session.Clear();
          this.Close();
        }
        else
        {
          FrmClientCard frmClientCard = new FrmClientCard(this.client);
          int num = (int) frmClientCard.ShowDialog();
          this.client = frmClientCard.Client;
          frmClientCard.Dispose();
        }
      }
      catch (Exception ex)
      {
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void btnJumpToHome_Click(object sender, EventArgs e)
    {
      this.CurrentHome = (Home) this.cmbHome.SelectedItem;
      this.Close();
    }

    private void cmbCity_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (((City) this.cmbCity.SelectedItem).CityId != 0)
        this.listStreet = this.session.CreateQuery("from Str s where s.IdCity=:idcity order by NameStr2").SetParameter<int>("idcity", ((City) this.cmbCity.SelectedItem).CityId).List<Str>();
      this.listStreet.Insert(0, new Str()
      {
        IdStr = 0,
        NameStr = ""
      });
      this.cmbStreet.DataSource = (object) this.listStreet;
      this.cmbStreet.DisplayMember = "NameStr2";
      this.cmbStreet.ValueMember = "IdStr";
      this.cmbStreet.Focus();
      this.session.Clear();
      this.cmbHome.DataSource = (object) null;
      this.cmbFlat.DataSource = (object) null;
    }

    private void cmbCity_SelectedValueChanged(object sender, EventArgs e)
    {
      if (!(this.cmbCity.Text != ""))
        return;
      if (((City) this.cmbCity.SelectedItem).CityId == 0)
      {
        this.cityError.Visible = true;
      }
      else
      {
        this.cmbCity_SelectionChangeCommitted((object) null, (EventArgs) null);
        this.cmbStreet.Focus();
        this.cityError.Visible = false;
      }
    }

    private void txtFamily_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.btnSearch_Click((object) null, (EventArgs) null);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmBigSearch));
      this.pnBtn = new Panel();
      this.btnSearch = new Button();
      this.btnOk = new Button();
      this.btnExit = new Button();
      this.gbAddress = new GroupBox();
      this.cmbCity = new ComboBox();
      this.lblCity = new Label();
      this.btnJumpToHome = new Button();
      this.gbHomes = new GroupBox();
      this.rbAll = new RadioButton();
      this.rbActive = new RadioButton();
      this.rbArchive = new RadioButton();
      this.cmbRoom = new ComboBox();
      this.lblRoom = new Label();
      this.cbClose = new CheckBox();
      this.cmbFlat = new ComboBox();
      this.lblFlat = new Label();
      this.cmbHome = new ComboBox();
      this.lblHome = new Label();
      this.lblStreet = new Label();
      this.cmbStreet = new ComboBox();
      this.gbFIO = new GroupBox();
      this.gbType = new GroupBox();
      this.rbVhod = new RadioButton();
      this.rbZnach = new RadioButton();
      this.cbArchive = new CheckBox();
      this.txtFName = new TextBox();
      this.lblFName = new Label();
      this.txtName = new TextBox();
      this.lblName = new Label();
      this.lblFamily = new Label();
      this.txtFamily = new TextBox();
      this.dgvResult = new DataGridView();
      this.hp = new HelpProvider();
      this.cityError = new Label();
      this.pnBtn.SuspendLayout();
      this.gbAddress.SuspendLayout();
      this.gbHomes.SuspendLayout();
      this.gbFIO.SuspendLayout();
      this.gbType.SuspendLayout();
      ((ISupportInitialize) this.dgvResult).BeginInit();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnSearch);
      this.pnBtn.Controls.Add((Control) this.btnOk);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 499);
      this.pnBtn.Margin = new Padding(4);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(897, 40);
      this.pnBtn.TabIndex = 0;
      this.btnSearch.Image = (Image) Resources.search_24;
      this.btnSearch.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSearch.Location = new Point(6, 5);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new Size(79, 30);
      this.btnSearch.TabIndex = 2;
      this.btnSearch.Text = "Поиск";
      this.btnSearch.TextAlign = ContentAlignment.MiddleRight;
      this.btnSearch.UseVisualStyleBackColor = true;
      this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
      this.btnOk.Image = (Image) Resources.Tick;
      this.btnOk.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnOk.Location = new Point(103, 5);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(63, 30);
      this.btnOk.TabIndex = 1;
      this.btnOk.Text = "ОК";
      this.btnOk.TextAlign = ContentAlignment.MiddleRight;
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Visible = false;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(805, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(80, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.gbAddress.Controls.Add((Control) this.cityError);
      this.gbAddress.Controls.Add((Control) this.cmbCity);
      this.gbAddress.Controls.Add((Control) this.lblCity);
      this.gbAddress.Controls.Add((Control) this.btnJumpToHome);
      this.gbAddress.Controls.Add((Control) this.gbHomes);
      this.gbAddress.Controls.Add((Control) this.cmbRoom);
      this.gbAddress.Controls.Add((Control) this.lblRoom);
      this.gbAddress.Controls.Add((Control) this.cbClose);
      this.gbAddress.Controls.Add((Control) this.cmbFlat);
      this.gbAddress.Controls.Add((Control) this.lblFlat);
      this.gbAddress.Controls.Add((Control) this.cmbHome);
      this.gbAddress.Controls.Add((Control) this.lblHome);
      this.gbAddress.Controls.Add((Control) this.lblStreet);
      this.gbAddress.Controls.Add((Control) this.cmbStreet);
      this.gbAddress.Dock = DockStyle.Top;
      this.gbAddress.Location = new Point(0, 0);
      this.gbAddress.Name = "gbAddress";
      this.gbAddress.Size = new Size(897, 124);
      this.gbAddress.TabIndex = 1;
      this.gbAddress.TabStop = false;
      this.gbAddress.Text = "Адрес";
      this.cmbCity.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
      this.cmbCity.Location = new Point(59, 15);
      this.cmbCity.Name = "cmbCity";
      this.cmbCity.Size = new Size(179, 24);
      this.cmbCity.TabIndex = 11;
      this.cmbCity.SelectionChangeCommitted += new EventHandler(this.cmbCity_SelectionChangeCommitted);
      this.cmbCity.SelectedValueChanged += new EventHandler(this.cmbCity_SelectedValueChanged);
      this.lblCity.AutoSize = true;
      this.lblCity.Location = new Point(6, 23);
      this.lblCity.Name = "lblCity";
      this.lblCity.Size = new Size(47, 16);
      this.lblCity.TabIndex = 10;
      this.lblCity.Text = "Город";
      this.btnJumpToHome.Location = new Point(716, 89);
      this.btnJumpToHome.Name = "btnJumpToHome";
      this.btnJumpToHome.Size = new Size(126, 29);
      this.btnJumpToHome.TabIndex = 9;
      this.btnJumpToHome.Text = "Перейти в дом";
      this.btnJumpToHome.UseVisualStyleBackColor = true;
      this.btnJumpToHome.Click += new EventHandler(this.btnJumpToHome_Click);
      this.gbHomes.Controls.Add((Control) this.rbAll);
      this.gbHomes.Controls.Add((Control) this.rbActive);
      this.gbHomes.Controls.Add((Control) this.rbArchive);
      this.gbHomes.Location = new Point(6, 78);
      this.gbHomes.Name = "gbHomes";
      this.gbHomes.Size = new Size(359, 40);
      this.gbHomes.TabIndex = 8;
      this.gbHomes.TabStop = false;
      this.gbHomes.Text = "Показывать дома";
      this.rbAll.AutoSize = true;
      this.rbAll.Location = new Point(9, 14);
      this.rbAll.Name = "rbAll";
      this.rbAll.Size = new Size(50, 20);
      this.rbAll.TabIndex = 2;
      this.rbAll.Text = "Все";
      this.rbAll.UseVisualStyleBackColor = true;
      this.rbAll.CheckedChanged += new EventHandler(this.rbAll_CheckedChanged);
      this.rbActive.AutoSize = true;
      this.rbActive.Checked = true;
      this.rbActive.Location = new Point(97, 14);
      this.rbActive.Name = "rbActive";
      this.rbActive.Size = new Size(116, 20);
      this.rbActive.TabIndex = 1;
      this.rbActive.TabStop = true;
      this.rbActive.Text = "Действующие";
      this.rbActive.UseVisualStyleBackColor = true;
      this.rbActive.CheckedChanged += new EventHandler(this.rbAll_CheckedChanged);
      this.rbArchive.AutoSize = true;
      this.rbArchive.Location = new Point(254, 14);
      this.rbArchive.Name = "rbArchive";
      this.rbArchive.Size = new Size(90, 20);
      this.rbArchive.TabIndex = 0;
      this.rbArchive.Text = "Архивные";
      this.rbArchive.UseVisualStyleBackColor = true;
      this.rbArchive.CheckedChanged += new EventHandler(this.rbAll_CheckedChanged);
      this.cmbRoom.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
      this.cmbRoom.FormattingEnabled = true;
      this.cmbRoom.Location = new Point(607, 45);
      this.cmbRoom.Name = "cmbRoom";
      this.cmbRoom.Size = new Size(90, 24);
      this.cmbRoom.TabIndex = 5;
      this.cmbRoom.SelectedValueChanged += new EventHandler(this.cmbRoom_SelectedValueChanged);
      this.lblRoom.AutoSize = true;
      this.lblRoom.Location = new Point(536, 48);
      this.lblRoom.Name = "lblRoom";
      this.lblRoom.Size = new Size(64, 16);
      this.lblRoom.TabIndex = 7;
      this.lblRoom.Text = "Комната";
      this.cbClose.AutoSize = true;
      this.cbClose.Location = new Point(381, 98);
      this.cbClose.Name = "cbClose";
      this.cbClose.Size = new Size(329, 20);
      this.cbClose.TabIndex = 6;
      this.cbClose.Text = "Включая архивные и закрытые лицевые счета";
      this.cbClose.UseVisualStyleBackColor = true;
      this.cmbFlat.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
      this.cmbFlat.FormattingEnabled = true;
      this.cmbFlat.Location = new Point(423, 45);
      this.cmbFlat.Name = "cmbFlat";
      this.cmbFlat.Size = new Size(94, 24);
      this.cmbFlat.TabIndex = 4;
      this.cmbFlat.SelectionChangeCommitted += new EventHandler(this.cmbFlat_SelectionChangeCommitted);
      this.cmbFlat.SelectedValueChanged += new EventHandler(this.cmbFlat_SelectedValueChanged);
      this.lblFlat.AutoSize = true;
      this.lblFlat.Location = new Point(346, 48);
      this.lblFlat.Name = "lblFlat";
      this.lblFlat.Size = new Size(71, 16);
      this.lblFlat.TabIndex = 4;
      this.lblFlat.Text = "Квартира";
      this.cmbHome.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
      this.cmbHome.FormattingEnabled = true;
      this.cmbHome.Location = new Point(605, 15);
      this.cmbHome.Name = "cmbHome";
      this.cmbHome.Size = new Size(92, 24);
      this.cmbHome.TabIndex = 3;
      this.cmbHome.SelectionChangeCommitted += new EventHandler(this.cmbHome_SelectionChangeCommitted);
      this.cmbHome.SelectedValueChanged += new EventHandler(this.cmbHome_SelectedValueChanged);
      this.cmbHome.KeyPress += new KeyPressEventHandler(this.cmbHome_KeyPress);
      this.lblHome.AutoSize = true;
      this.lblHome.Location = new Point(565, 18);
      this.lblHome.Name = "lblHome";
      this.lblHome.Size = new Size(34, 16);
      this.lblHome.TabIndex = 2;
      this.lblHome.Text = "Дом";
      this.lblStreet.AutoSize = true;
      this.lblStreet.Location = new Point((int) byte.MaxValue, 18);
      this.lblStreet.Name = "lblStreet";
      this.lblStreet.Size = new Size(49, 16);
      this.lblStreet.TabIndex = 1;
      this.lblStreet.Text = "Улица";
      this.cmbStreet.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
      this.cmbStreet.Location = new Point(310, 15);
      this.cmbStreet.Name = "cmbStreet";
      this.cmbStreet.Size = new Size(248, 24);
      this.cmbStreet.TabIndex = 0;
      this.cmbStreet.SelectionChangeCommitted += new EventHandler(this.cmbStreet_SelectionChangeCommitted);
      this.cmbStreet.SelectedValueChanged += new EventHandler(this.cmbStreet_SelectedValueChanged);
      this.cmbStreet.KeyPress += new KeyPressEventHandler(this.cmbStreet_KeyPress);
      this.cmbStreet.KeyUp += new KeyEventHandler(this.cmbStreet_KeyDown);
      this.gbFIO.Controls.Add((Control) this.gbType);
      this.gbFIO.Controls.Add((Control) this.cbArchive);
      this.gbFIO.Controls.Add((Control) this.txtFName);
      this.gbFIO.Controls.Add((Control) this.lblFName);
      this.gbFIO.Controls.Add((Control) this.txtName);
      this.gbFIO.Controls.Add((Control) this.lblName);
      this.gbFIO.Controls.Add((Control) this.lblFamily);
      this.gbFIO.Controls.Add((Control) this.txtFamily);
      this.gbFIO.Dock = DockStyle.Top;
      this.gbFIO.Location = new Point(0, 124);
      this.gbFIO.Name = "gbFIO";
      this.gbFIO.Size = new Size(897, (int) sbyte.MaxValue);
      this.gbFIO.TabIndex = 2;
      this.gbFIO.TabStop = false;
      this.gbFIO.Text = "ФИО";
      this.gbType.Controls.Add((Control) this.rbVhod);
      this.gbType.Controls.Add((Control) this.rbZnach);
      this.gbType.Location = new Point(9, 47);
      this.gbType.Name = "gbType";
      this.gbType.Size = new Size(281, 39);
      this.gbType.TabIndex = 8;
      this.gbType.TabStop = false;
      this.gbType.Text = "Выбирать ФИО";
      this.rbVhod.AutoSize = true;
      this.rbVhod.Location = new Point(141, 13);
      this.rbVhod.Name = "rbVhod";
      this.rbVhod.Size = new Size(120, 20);
      this.rbVhod.TabIndex = 1;
      this.rbVhod.Text = "По вхождению";
      this.rbVhod.UseVisualStyleBackColor = true;
      this.rbZnach.AutoSize = true;
      this.rbZnach.Checked = true;
      this.rbZnach.Location = new Point(6, 13);
      this.rbZnach.Name = "rbZnach";
      this.rbZnach.Size = new Size(113, 20);
      this.rbZnach.TabIndex = 0;
      this.rbZnach.TabStop = true;
      this.rbZnach.Text = "По значению";
      this.rbZnach.UseVisualStyleBackColor = true;
      this.cbArchive.AutoSize = true;
      this.cbArchive.Location = new Point(9, 92);
      this.cbArchive.Name = "cbArchive";
      this.cbArchive.Size = new Size(207, 20);
      this.cbArchive.TabIndex = 6;
      this.cbArchive.Text = "Включая архивных жильцов";
      this.cbArchive.UseVisualStyleBackColor = true;
      this.txtFName.Location = new Point(564, 19);
      this.txtFName.Name = "txtFName";
      this.txtFName.Size = new Size((int) sbyte.MaxValue, 22);
      this.txtFName.TabIndex = 5;
      this.txtFName.KeyDown += new KeyEventHandler(this.txtFamily_KeyDown);
      this.lblFName.AutoSize = true;
      this.lblFName.Location = new Point(487, 22);
      this.lblFName.Name = "lblFName";
      this.lblFName.Size = new Size(71, 16);
      this.lblFName.TabIndex = 4;
      this.lblFName.Text = "Отчество";
      this.txtName.Location = new Point(333, 19);
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(148, 22);
      this.txtName.TabIndex = 3;
      this.txtName.KeyDown += new KeyEventHandler(this.txtFamily_KeyDown);
      this.lblName.AutoSize = true;
      this.lblName.Location = new Point(293, 22);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(34, 16);
      this.lblName.TabIndex = 2;
      this.lblName.Text = "Имя";
      this.lblFamily.AutoSize = true;
      this.lblFamily.Location = new Point(6, 22);
      this.lblFamily.Name = "lblFamily";
      this.lblFamily.Size = new Size(67, 16);
      this.lblFamily.TabIndex = 1;
      this.lblFamily.Text = "Фамилия";
      this.txtFamily.Location = new Point(76, 19);
      this.txtFamily.Name = "txtFamily";
      this.txtFamily.Size = new Size(211, 22);
      this.txtFamily.TabIndex = 0;
      this.txtFamily.KeyDown += new KeyEventHandler(this.txtFamily_KeyDown);
      this.dgvResult.BackgroundColor = Color.AliceBlue;
      this.dgvResult.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvResult.Dock = DockStyle.Fill;
      this.dgvResult.Location = new Point(0, 251);
      this.dgvResult.Name = "dgvResult";
      this.dgvResult.ReadOnly = true;
      this.dgvResult.Size = new Size(897, 248);
      this.dgvResult.TabIndex = 3;
      this.dgvResult.VirtualMode = true;
      this.dgvResult.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvResult_CellFormatting);
      this.dgvResult.DataError += new DataGridViewDataErrorEventHandler(this.dgvResult_DataError);
      this.dgvResult.KeyDown += new KeyEventHandler(this.dgvResult_KeyDown);
      this.dgvResult.MouseDoubleClick += new MouseEventHandler(this.dgvResult_MouseDoubleClick);
      this.hp.HelpNamespace = "Help.chm";
      this.cityError.AutoSize = true;
      this.cityError.ForeColor = Color.Red;
      this.cityError.Location = new Point(9, 45);
      this.cityError.Name = "cityError";
      this.cityError.Size = new Size(114, 16);
      this.cityError.TabIndex = 12;
      this.cityError.Text = "Выберите город";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(897, 539);
      this.Controls.Add((Control) this.dgvResult);
      this.Controls.Add((Control) this.gbFIO);
      this.Controls.Add((Control) this.gbAddress);
      this.Controls.Add((Control) this.pnBtn);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.hp.SetHelpKeyword((Control) this, "kv113.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmBigSearch";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Расширенный поиск";
      this.Shown += new EventHandler(this.FrmBigSearch_Shown);
      this.pnBtn.ResumeLayout(false);
      this.gbAddress.ResumeLayout(false);
      this.gbAddress.PerformLayout();
      this.gbHomes.ResumeLayout(false);
      this.gbHomes.PerformLayout();
      this.gbFIO.ResumeLayout(false);
      this.gbFIO.PerformLayout();
      this.gbType.ResumeLayout(false);
      this.gbType.PerformLayout();
      ((ISupportInitialize) this.dgvResult).EndInit();
      this.ResumeLayout(false);
    }
  }
}
