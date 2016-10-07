// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmServiceParam
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
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Kvartplata.Forms
{
  public class FrmServiceParam : Form
  {
    private FormStateSaver fss = new FormStateSaver(FrmServiceParam.ic);
    private bool _readOnly = false;
    private IContainer components = (IContainer) null;
    private IList serviceList;
    private ISession session;
    private Kvartplata.Classes.Service curService;
    private IList serviceParamList;
    private Receipt curReceipt;
    private UCServParam curUCServParam;
    private static IContainer ic;
    private Company _company;
    private GroupBox groupBox1;
    private ComboBox cmbCompany;
    private Label lblCompany;
    private ComboBox cmbReceipt;
    private Label lblReceipt;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private Panel pnBtn;
    private Button btnExit;
    private ToolStrip toolStrip1;
    private ToolStripButton tsbAdd;
    private ToolStripButton tsbDelete;
    private ToolStripLabel toolStripLabel2;
    private ToolStripLabel toolStripLabel1;
    private ToolStripButton tsbUp;
    private ToolStripButton tsbDown;
    private SplitContainer splitContainer1;
    private DataGridView dgvService;
    private FlowLayoutPanel flpServ;
    private Panel panel2;
    private DataGridViewTextBoxColumn Number;
    private DataGridViewTextBoxColumn Service;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    public HelpProvider hp;
    private Label lblComplex;
    private ComboBox cmbComplex;

    private Kvartplata.Classes.Service CurService
    {
      get
      {
        return this.curService;
      }
      set
      {
        this.curService = value;
        this.tsbAdd.Enabled = true;
        this.tsbDelete.Enabled = true;
        this.tsbDown.Enabled = true;
        this.tsbUp.Enabled = true;
        if (this.curService == null)
          return;
        bool flag = false;
        foreach (UCServParam control in (ArrangedElementCollection) this.flpServ.Controls)
        {
          if ((int) control.SrvParam.Service_id == (int) this.curService.ServiceId)
          {
            control.Selected = true;
            this.UnSelect((object) control);
            control.Refresh();
            flag = true;
            this.curUCServParam = control;
          }
        }
        if (!flag)
          this.UnSelect((object) null);
      }
    }

    public short Company_id { get; set; }

    public Complex Complex { get; set; }

    private Receipt CurReceipt
    {
      get
      {
        return this.curReceipt;
      }
      set
      {
        IList receiptList = this.session.CreateQuery(string.Format("from Receipt r where r.ReceiptId in (select c.ReceiptId from CmpReceipt c where c.CompanyId={0}) order by r.ReceiptName", (object) this.Company_id)).List();
        receiptList.Insert(0, (object) new Receipt()
        {
          ReceiptId = (short) 0
        });
        this.tsbAdd.Enabled = true;
        this.tsbDelete.Enabled = true;
        this.tsbDown.Enabled = true;
        this.tsbUp.Enabled = true;
        this.curReceipt = value;
        if (value != null && (uint) value.ReceiptId > 0U)
        {
          this.serviceParamList = this.session.CreateQuery("from ServiceParam where Company_id=:cid and Complex.IdFk=:compid and receipt_id=:rid order by Sorter").SetInt16("cid", this.Company_id).SetInt16("rid", value.ReceiptId).SetInt32("compid", this.Complex.IdFk).List();
          this.flpServ.Controls.Clear();
          if (this.serviceParamList.Count > 0)
          {
            frmProgress frmProgress = new frmProgress(this.serviceParamList.Count);
            frmProgress.StartPosition = FormStartPosition.CenterScreen;
            frmProgress.Show();
            this.cmbReceipt.Enabled = false;
            this.cmbCompany.Enabled = false;
            foreach (ServiceParam serviceParam in (IEnumerable) this.serviceParamList)
            {
              ++frmProgress.Progress;
              this.session.Refresh((object) serviceParam);
              UCServParam ucServParam = new UCServParam(serviceParam, false, receiptList);
              ucServParam.SelectedChanged += new EventHandler(this.ucServParam_SelectedChanged);
              ucServParam.AddedServParam += new EventHandler(this.ucServParam_AddedServParam);
              this.flpServ.Controls.Add((Control) ucServParam);
              Application.DoEvents();
            }
            frmProgress.Dispose();
            this.cmbReceipt.Enabled = true;
            this.cmbCompany.Enabled = true;
          }
          bool flag = false;
          foreach (UCServParam control in (ArrangedElementCollection) this.flpServ.Controls)
          {
            if ((int) control.SrvParam.Service_id == (int) this.curService.ServiceId)
            {
              control.Selected = true;
              this.UnSelect((object) control);
              control.Refresh();
              flag = true;
              this.curUCServParam = control;
            }
          }
          if (flag)
            return;
          this.UnSelect((object) null);
        }
        else
        {
          this.flpServ.Controls.Clear();
          if (this.serviceParamList != null)
            this.serviceParamList.Clear();
          this.serviceParamList = this.session.CreateQuery("from ServiceParam where Company_id=:cid and Complex.IdFk=:compid order by Sorter").SetInt16("cid", this.Company_id).SetInt32("compid", this.Complex.ComplexId).List();
          this.flpServ.Controls.Clear();
          this.flpServ.SuspendLayout();
          if (this.serviceParamList.Count > 0)
          {
            frmProgress frmProgress = new frmProgress(this.serviceParamList.Count);
            frmProgress.StartPosition = FormStartPosition.CenterScreen;
            frmProgress.Show();
            this.cmbReceipt.Enabled = false;
            this.cmbCompany.Enabled = false;
            foreach (ServiceParam serviceParam in (IEnumerable) this.serviceParamList)
            {
              ++frmProgress.Progress;
              UCServParam ucServParam = new UCServParam(serviceParam, false, receiptList);
              ucServParam.SelectedChanged += new EventHandler(this.ucServParam_SelectedChanged);
              ucServParam.AddedServParam += new EventHandler(this.ucServParam_AddedServParam);
              this.flpServ.Controls.Add((Control) ucServParam);
              Application.DoEvents();
            }
            frmProgress.Dispose();
            this.cmbReceipt.Enabled = true;
            this.cmbCompany.Enabled = true;
          }
          this.flpServ.ResumeLayout();
          bool flag = false;
          if (this.curService != null)
          {
            foreach (UCServParam control in (ArrangedElementCollection) this.flpServ.Controls)
            {
              if ((int) control.SrvParam.Service_id == (int) this.curService.ServiceId)
              {
                control.Selected = true;
                this.UnSelect((object) control);
                control.Refresh();
                flag = true;
                this.curUCServParam = control;
              }
            }
          }
          if (!flag)
            this.UnSelect((object) null);
        }
      }
    }

    public FrmServiceParam()
    {
      this.InitializeComponent();
      this.fss.ParentForm = (Form) this;
    }

    public FrmServiceParam(Company company)
    {
      this.InitializeComponent();
      this._company = company;
      this.fss.ParentForm = (Form) this;
      this.session = Domain.CurrentSession;
      this.session.Clear();
      this.serviceList = this.session.CreateQuery("from Service s where s.Root=0 and s.ServiceId>0 order by " + Options.SortService).List();
      this.dgvService.RowCount = this.serviceList.Count;
      if (Options.Kvartplata && Options.Arenda)
      {
        IList<Complex> complexList = (IList<Complex>) new List<Complex>();
        complexList.Insert(0, new Complex(100, "Квартплата"));
        complexList.Insert(1, new Complex(110, "Аренда"));
        this.cmbComplex.DataSource = (object) complexList;
        this.cmbComplex.DisplayMember = "ComplexName";
        this.cmbComplex.ValueMember = "ComplexId";
        this.Complex = Options.Complex;
      }
      else
      {
        this.lblComplex.Visible = false;
        this.cmbComplex.Visible = false;
        this.lblReceipt.Top = this.lblCompany.Top;
        this.lblCompany.Top = this.lblComplex.Top;
        this.cmbReceipt.Top = this.cmbCompany.Top;
        this.cmbCompany.Top = this.cmbComplex.Top;
        this.groupBox1.Height = 80;
        this.Complex = !Options.Kvartplata ? Options.ComplexArenda : Options.Complex;
      }
      this.cmbCompany.DataSource = (object) this.session.CreateQuery("from Company order by CompanyId").List();
      this.cmbCompany.DisplayMember = "CompanyName";
      this.cmbCompany.ValueMember = "CompanyId";
      this.Company_id = company.CompanyId;
      IList list = this.session.CreateQuery(string.Format("from Receipt r where r.ReceiptId in (select c.ReceiptId from CmpReceipt c where c.CompanyId={0}) order by r.ReceiptName", (object) this.Company_id)).List();
      list.Insert(0, (object) new Receipt()
      {
        ReceiptId = (short) 0
      });
      this.cmbReceipt.DataSource = (object) list;
      this.cmbReceipt.DisplayMember = "ReceiptName";
      this.cmbReceipt.ValueMember = "ReceiptId";
      this.CheckAccess();
    }

    private void CheckAccess()
    {
      this._readOnly = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(33, this._company, false));
      this.toolStrip1.Visible = this._readOnly;
      this.dgvService.ReadOnly = !this._readOnly;
      foreach (Control control in (ArrangedElementCollection) this.flpServ.Controls)
        control.Enabled = this._readOnly;
      this.flpServ.Refresh();
    }

    private void UnSelect(object sender)
    {
      this.flpServ.Enabled = false;
      foreach (UCServParam control in (ArrangedElementCollection) this.flpServ.Controls)
      {
        if (control != sender)
          control.Selected = false;
      }
      this.flpServ.Refresh();
      this.CheckAccess();
      this.flpServ.Enabled = true;
    }

    private void FrmServiceParam_Load(object sender, EventArgs e)
    {
      if (this.session == null)
        return;
      this.cmbCompany.SelectedItem = (object) this.session.Get<Company>((object) this.Company_id);
    }

    private void dgvService_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.serviceList.Count <= 0)
        return;
      if (this.dgvService.Columns[e.ColumnIndex].Name == "Service")
        e.Value = (object) ((Kvartplata.Classes.Service) this.serviceList[e.RowIndex]).ServiceName;
      if (this.dgvService.Columns[e.ColumnIndex].Name == "Number")
        e.Value = (object) ((Kvartplata.Classes.Service) this.serviceList[e.RowIndex]).ServiceId;
    }

    private void dgvService_SelectionChanged(object sender, EventArgs e)
    {
      if (this.dgvService.CurrentRow != null && this.dgvService.CurrentRow.Index < this.serviceList.Count)
        this.CurService = (Kvartplata.Classes.Service) this.serviceList[this.dgvService.CurrentRow.Index];
      this.flpServ.ScrollControlIntoView((Control) this.curUCServParam);
    }

    private void btmAdd_Click(object sender, EventArgs e)
    {
      ServiceParam srvParam = new ServiceParam(this.Company_id, this.CurService.ServiceId, this.Complex);
      ServiceParam serviceParam = this.session.Get<ServiceParam>((object) srvParam);
      if (serviceParam == null)
      {
        srvParam.Receipt_id = this.curReceipt.ReceiptId;
        srvParam.PrintShow = this.curService.ServiceName;
        srvParam.Sorter = Convert.ToInt16(this.flpServ.Controls.Count);
        IList receiptList = this.session.CreateQuery(string.Format("from Receipt r where r.ReceiptId in (select c.ReceiptId from CmpReceipt c where c.CompanyId={0}) order by r.ReceiptName", (object) this.Company_id)).List();
        receiptList.Insert(0, (object) new Receipt()
        {
          ReceiptId = (short) 0
        });
        UCServParam ucServParam = new UCServParam(srvParam, true, receiptList);
        ucServParam.SelectedChanged += new EventHandler(this.ucServParam_SelectedChanged);
        ucServParam.AddedServParam += new EventHandler(this.ucServParam_AddedServParam);
        ucServParam.Selected = true;
        this.curUCServParam = ucServParam;
        this.UnSelect((object) ucServParam);
        this.flpServ.Controls.Add((Control) ucServParam);
        this.flpServ.ScrollControlIntoView((Control) ucServParam);
        this.tsbUp.Enabled = false;
        this.tsbDown.Enabled = false;
      }
      else
      {
        int num = (int) MessageBox.Show("Данная услуга присутствует в квитанции '" + this.session.Get<Receipt>((object) serviceParam.Receipt_id).ReceiptName + "'", "Внимание!", MessageBoxButtons.OK);
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.curUCServParam == null || MessageBox.Show("Вы действительно хотите удалить услугу из списка?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      try
      {
        for (int index = this.flpServ.Controls.GetChildIndex((Control) this.curUCServParam) + 1; index < this.flpServ.Controls.Count; ++index)
        {
          --((UCServParam) this.flpServ.Controls[index]).SrvParam.Sorter;
          this.session.Update((object) ((UCServParam) this.flpServ.Controls[index]).SrvParam);
          ((UCServParam) this.flpServ.Controls[index]).UpdateSort();
        }
        this.session.Delete((object) this.curUCServParam.SrvParam);
        this.flpServ.Controls.Remove((Control) this.curUCServParam);
        this.session.Flush();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Не удалось удалить запись!", "Внимание!", MessageBoxButtons.OK);
        KvrplHelper.WriteLog(ex, (LsClient) null);
      }
    }

    private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void cbReceipt_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.CurReceipt = (Receipt) this.cmbReceipt.SelectedItem;
    }

    private void ucServParam_SelectedChanged(object sender, EventArgs e)
    {
      this.curUCServParam = (UCServParam) sender;
      int index = 0;
      foreach (Kvartplata.Classes.Service service in (IEnumerable) this.serviceList)
      {
        if ((int) ((UCServParam) sender).SrvParam.Service_id == (int) service.ServiceId)
        {
          this.dgvService.CurrentCell = this.dgvService.Rows[index].Cells[0];
          this.CurService = (Kvartplata.Classes.Service) this.serviceList[this.dgvService.CurrentRow.Index];
          break;
        }
        ++index;
      }
    }

    private void ucServParam_AddedServParam(object sender, EventArgs e)
    {
      this.tsbDown.Enabled = true;
      this.tsbUp.Enabled = true;
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void tsbAdd_Click(object sender, EventArgs e)
    {
      ServiceParam srvParam = new ServiceParam(this.Company_id, this.CurService.ServiceId, this.Complex);
      if (this.session.Get<ServiceParam>((object) srvParam) == null)
      {
        srvParam.Receipt_id = this.curReceipt.ReceiptId;
        srvParam.PrintShow = this.curService.ServiceName;
        srvParam.Sorter = Convert.ToInt16(this.flpServ.Controls.Count);
        srvParam.SubsidIn = this.session.Get<Kvartplata.Classes.Service>((object) Convert.ToInt16(0));
        srvParam.SaveOverpay = this.session.Get<YesNo>((object) Convert.ToInt16(0));
        srvParam.SendRent = this.session.Get<YesNo>((object) Convert.ToInt16(0));
        srvParam.DistrService = this.session.Get<YesNo>((object) Convert.ToInt16(1));
        srvParam.DublService = this.session.Get<YesNo>((object) Convert.ToInt16(1));
        srvParam.ShowService = this.session.Get<YesNo>((object) Convert.ToInt16(1));
        srvParam.ShowServiceInfo = this.session.Get<YesNo>((object) Convert.ToInt16(1));
        srvParam.BoilService = this.session.Get<YesNo>((object) Convert.ToInt16(0));
        srvParam.SpecialId = (short) 0;
        IList receiptList = this.session.CreateQuery(string.Format("from Receipt r where r.ReceiptId in (select c.ReceiptId from CmpReceipt c where c.CompanyId={0}) order by r.ReceiptName", (object) this.Company_id)).List();
        receiptList.Insert(0, (object) new Receipt()
        {
          ReceiptId = (short) 0
        });
        UCServParam ucServParam = new UCServParam(srvParam, true, receiptList);
        ucServParam.SelectedChanged += new EventHandler(this.ucServParam_SelectedChanged);
        ucServParam.AddedServParam += new EventHandler(this.ucServParam_AddedServParam);
        ucServParam.Selected = true;
        this.curUCServParam = ucServParam;
        this.UnSelect((object) ucServParam);
        this.flpServ.Controls.Add((Control) ucServParam);
        this.flpServ.ScrollControlIntoView((Control) ucServParam);
        this.tsbUp.Enabled = false;
        this.tsbDown.Enabled = false;
      }
      else
      {
        int num = (int) MessageBox.Show("Данная услуга присутствует в списке", "Внимание!", MessageBoxButtons.OK);
      }
    }

    private void tsbDelete_Click(object sender, EventArgs e)
    {
      if (this.curUCServParam == null || MessageBox.Show("Вы действительно хотите удалить услугу из списка?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      if (this.session.CreateSQLQuery(string.Format("select first b.client_id from lsBalance b where b.client_id in (select client_id from lsClient where company_id={1} and complex_id={2}) and b.service_id>={0}*100 and b.service_id<({0}+1)*100", (object) this.curUCServParam.SrvParam.Service_id, (object) this.Company_id, (object) this.Complex.IdFk)).List().Count > 0)
      {
        int num1 = (int) MessageBox.Show("Невозможно удалить запись. По услуге существует сальдо!", "Внимание!", MessageBoxButtons.OK);
      }
      else
      {
        try
        {
          for (int index = this.flpServ.Controls.GetChildIndex((Control) this.curUCServParam) + 1; index < this.flpServ.Controls.Count; ++index)
          {
            --((UCServParam) this.flpServ.Controls[index]).SrvParam.Sorter;
            this.session.Update((object) ((UCServParam) this.flpServ.Controls[index]).SrvParam);
            ((UCServParam) this.flpServ.Controls[index]).UpdateSort();
          }
          this.session.Delete((object) this.curUCServParam.SrvParam);
          this.flpServ.Controls.Remove((Control) this.curUCServParam);
          this.session.Flush();
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show("Не удалось удалить запись!", "Внимание!", MessageBoxButtons.OK);
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
    }

    private void tsbUp_Click(object sender, EventArgs e)
    {
      int childIndex = this.flpServ.Controls.GetChildIndex((Control) this.curUCServParam);
      if (childIndex <= 0)
        return;
      ServiceParam srvParam = ((UCServParam) this.flpServ.Controls[childIndex - 1]).SrvParam;
      this.curUCServParam.SrvParam.Sorter = srvParam.Sorter;
      this.session.Update((object) this.curUCServParam.SrvParam);
      ((UCServParam) this.flpServ.Controls[childIndex - 1]).SrvParam = this.curUCServParam.SrvParam;
      ((UCServParam) this.flpServ.Controls[childIndex - 1]).LoadData();
      UCServParam control = (UCServParam) this.flpServ.Controls[childIndex - 1];
      ++srvParam.Sorter;
      this.session.Update((object) srvParam);
      this.curUCServParam.SrvParam = srvParam;
      this.curUCServParam.LoadData();
      this.session.Flush();
      this.CurService = this.session.Get<Kvartplata.Classes.Service>((object) ((UCServParam) this.flpServ.Controls[childIndex - 1]).SrvParam.Service_id);
    }

    private void tsbDown_Click(object sender, EventArgs e)
    {
      int childIndex = this.flpServ.Controls.GetChildIndex((Control) this.curUCServParam);
      if (childIndex >= this.flpServ.Controls.Count)
        return;
      ServiceParam srvParam = ((UCServParam) this.flpServ.Controls[childIndex + 1]).SrvParam;
      this.curUCServParam.SrvParam.Sorter = srvParam.Sorter;
      this.session.Update((object) this.curUCServParam.SrvParam);
      ((UCServParam) this.flpServ.Controls[childIndex + 1]).SrvParam = this.curUCServParam.SrvParam;
      ((UCServParam) this.flpServ.Controls[childIndex + 1]).LoadData();
      UCServParam control = (UCServParam) this.flpServ.Controls[childIndex + 1];
      --srvParam.Sorter;
      this.session.Update((object) srvParam);
      this.curUCServParam.SrvParam = srvParam;
      this.curUCServParam.LoadData();
      this.session.Flush();
      this.CurService = this.session.Get<Kvartplata.Classes.Service>((object) ((UCServParam) this.flpServ.Controls[childIndex + 1]).SrvParam.Service_id);
    }

    private void cbCompany_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if ((Company) this.cmbCompany.SelectedItem == null || (int) ((Company) this.cmbCompany.SelectedItem).CompanyId == (int) this.Company_id)
        return;
      this.Company_id = ((Company) this.cmbCompany.SelectedItem).CompanyId;
      IList list = this.session.CreateQuery(string.Format("from Receipt r where r.ReceiptId in (select c.ReceiptId from CmpReceipt c where c.CompanyId={0}) order by r.ReceiptName", (object) this.Company_id)).List();
      list.Insert(0, (object) new Receipt()
      {
        ReceiptId = (short) 0
      });
      this.cmbReceipt.DataSource = (object) list;
    }

    private void dgvService_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    private void cmbComplex_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.Complex = (Complex) this.cmbComplex.SelectedItem;
      this.CurReceipt = (Receipt) this.cmbReceipt.SelectedItem;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmServiceParam));
      this.groupBox1 = new GroupBox();
      this.cmbComplex = new ComboBox();
      this.lblComplex = new Label();
      this.cmbReceipt = new ComboBox();
      this.lblReceipt = new Label();
      this.cmbCompany = new ComboBox();
      this.lblCompany = new Label();
      this.pnBtn = new Panel();
      this.btnExit = new Button();
      this.toolStrip1 = new ToolStrip();
      this.tsbAdd = new ToolStripButton();
      this.tsbDelete = new ToolStripButton();
      this.toolStripLabel2 = new ToolStripLabel();
      this.toolStripLabel1 = new ToolStripLabel();
      this.tsbUp = new ToolStripButton();
      this.tsbDown = new ToolStripButton();
      this.splitContainer1 = new SplitContainer();
      this.dgvService = new DataGridView();
      this.Number = new DataGridViewTextBoxColumn();
      this.Service = new DataGridViewTextBoxColumn();
      this.flpServ = new FlowLayoutPanel();
      this.panel2 = new Panel();
      this.hp = new HelpProvider();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.groupBox1.SuspendLayout();
      this.pnBtn.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      ((ISupportInitialize) this.dgvService).BeginInit();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.groupBox1.Controls.Add((Control) this.cmbComplex);
      this.groupBox1.Controls.Add((Control) this.lblComplex);
      this.groupBox1.Controls.Add((Control) this.cmbReceipt);
      this.groupBox1.Controls.Add((Control) this.lblReceipt);
      this.groupBox1.Controls.Add((Control) this.cmbCompany);
      this.groupBox1.Controls.Add((Control) this.lblCompany);
      this.groupBox1.Dock = DockStyle.Top;
      this.groupBox1.Location = new Point(0, 0);
      this.groupBox1.Margin = new Padding(4);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new Padding(4);
      this.groupBox1.Size = new Size(961, 106);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.cmbComplex.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbComplex.FormattingEnabled = true;
      this.cmbComplex.Location = new Point(92, 17);
      this.cmbComplex.Name = "cmbComplex";
      this.cmbComplex.Size = new Size(856, 24);
      this.cmbComplex.TabIndex = 6;
      this.cmbComplex.SelectionChangeCommitted += new EventHandler(this.cmbComplex_SelectionChangeCommitted);
      this.lblComplex.AutoSize = true;
      this.lblComplex.Location = new Point(7, 20);
      this.lblComplex.Name = "lblComplex";
      this.lblComplex.Size = new Size(72, 17);
      this.lblComplex.TabIndex = 5;
      this.lblComplex.Text = "Комплекс";
      this.cmbReceipt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbReceipt.FormattingEnabled = true;
      this.cmbReceipt.Location = new Point(92, 74);
      this.cmbReceipt.Margin = new Padding(4);
      this.cmbReceipt.Name = "cmbReceipt";
      this.cmbReceipt.Size = new Size(855, 24);
      this.cmbReceipt.TabIndex = 4;
      this.cmbReceipt.SelectedIndexChanged += new EventHandler(this.cbReceipt_SelectedIndexChanged);
      this.lblReceipt.AutoSize = true;
      this.lblReceipt.Location = new Point(7, 77);
      this.lblReceipt.Margin = new Padding(4, 0, 4, 0);
      this.lblReceipt.Name = "lblReceipt";
      this.lblReceipt.Size = new Size(79, 17);
      this.lblReceipt.TabIndex = 3;
      this.lblReceipt.Text = "Квитанция";
      this.cmbCompany.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbCompany.FormattingEnabled = true;
      this.cmbCompany.Location = new Point(92, 45);
      this.cmbCompany.Margin = new Padding(4);
      this.cmbCompany.Name = "cmbCompany";
      this.cmbCompany.Size = new Size(855, 24);
      this.cmbCompany.TabIndex = 1;
      this.cmbCompany.SelectedIndexChanged += new EventHandler(this.cbCompany_SelectedIndexChanged);
      this.cmbCompany.SelectionChangeCommitted += new EventHandler(this.cbCompany_SelectionChangeCommitted);
      this.lblCompany.AutoSize = true;
      this.lblCompany.Location = new Point(7, 49);
      this.lblCompany.Margin = new Padding(4, 0, 4, 0);
      this.lblCompany.Name = "lblCompany";
      this.lblCompany.Size = new Size(74, 17);
      this.lblCompany.TabIndex = 0;
      this.lblCompany.Text = "Компания";
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 532);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(961, 40);
      this.pnBtn.TabIndex = 1;
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.Location = new Point(862, 5);
      this.btnExit.Margin = new Padding(4);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(86, 30);
      this.btnExit.TabIndex = 3;
      this.btnExit.Text = "Выход";
      this.btnExit.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.toolStrip1.Dock = DockStyle.Fill;
      this.toolStrip1.Items.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.tsbAdd,
        (ToolStripItem) this.tsbDelete,
        (ToolStripItem) this.toolStripLabel2,
        (ToolStripItem) this.toolStripLabel1,
        (ToolStripItem) this.tsbUp,
        (ToolStripItem) this.tsbDown
      });
      this.toolStrip1.LayoutStyle = ToolStripLayoutStyle.Table;
      this.toolStrip1.Location = new Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Padding = new Padding(5, 120, 5, 0);
      this.toolStrip1.Size = new Size(56, 426);
      this.toolStrip1.TabIndex = 4;
      this.toolStrip1.Text = "toolStrip1";
      this.tsbAdd.Checked = true;
      this.tsbAdd.CheckState = CheckState.Checked;
      this.tsbAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.tsbAdd.Image = (Image) Resources.arrow_right;
      this.tsbAdd.ImageTransparentColor = Color.Magenta;
      this.tsbAdd.Name = "tsbAdd";
      this.tsbAdd.Padding = new Padding(10);
      this.tsbAdd.Size = new Size(40, 40);
      this.tsbAdd.Text = "Добавить";
      this.tsbAdd.Click += new EventHandler(this.tsbAdd_Click);
      this.tsbDelete.Checked = true;
      this.tsbDelete.CheckState = CheckState.Checked;
      this.tsbDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.tsbDelete.Image = (Image) Resources.arrow_left;
      this.tsbDelete.ImageTransparentColor = Color.Magenta;
      this.tsbDelete.Name = "tsbDelete";
      this.tsbDelete.Padding = new Padding(10);
      this.tsbDelete.Size = new Size(40, 40);
      this.tsbDelete.Text = "Удалить";
      this.tsbDelete.Click += new EventHandler(this.tsbDelete_Click);
      this.toolStripLabel2.Name = "toolStripLabel2";
      this.toolStripLabel2.Size = new Size(13, 15);
      this.toolStripLabel2.Text = "  ";
      this.toolStripLabel1.Name = "toolStripLabel1";
      this.toolStripLabel1.Size = new Size(13, 15);
      this.toolStripLabel1.Text = "  ";
      this.tsbUp.Checked = true;
      this.tsbUp.CheckState = CheckState.Checked;
      this.tsbUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.tsbUp.Image = (Image) Resources.arrow_up;
      this.tsbUp.ImageTransparentColor = Color.Magenta;
      this.tsbUp.Name = "tsbUp";
      this.tsbUp.Padding = new Padding(10);
      this.tsbUp.Size = new Size(40, 40);
      this.tsbUp.Text = "Вверх";
      this.tsbUp.Click += new EventHandler(this.tsbUp_Click);
      this.tsbDown.Checked = true;
      this.tsbDown.CheckState = CheckState.Checked;
      this.tsbDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.tsbDown.Image = (Image) Resources.arrow_down;
      this.tsbDown.ImageTransparentColor = Color.Magenta;
      this.tsbDown.Name = "tsbDown";
      this.tsbDown.Padding = new Padding(10);
      this.tsbDown.Size = new Size(40, 40);
      this.tsbDown.Text = "Вниз";
      this.tsbDown.Click += new EventHandler(this.tsbDown_Click);
      this.splitContainer1.Dock = DockStyle.Fill;
      this.splitContainer1.Location = new Point(0, 106);
      this.splitContainer1.Margin = new Padding(4);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.dgvService);
      this.splitContainer1.Panel2.Controls.Add((Control) this.flpServ);
      this.splitContainer1.Panel2.Controls.Add((Control) this.panel2);
      this.splitContainer1.Size = new Size(961, 426);
      this.splitContainer1.SplitterDistance = 204;
      this.splitContainer1.SplitterWidth = 5;
      this.splitContainer1.TabIndex = 2;
      this.dgvService.AllowUserToAddRows = false;
      this.dgvService.AllowUserToDeleteRows = false;
      this.dgvService.BackgroundColor = Color.AliceBlue;
      this.dgvService.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvService.Columns.AddRange((DataGridViewColumn) this.Number, (DataGridViewColumn) this.Service);
      this.dgvService.Dock = DockStyle.Fill;
      this.dgvService.Location = new Point(0, 0);
      this.dgvService.Margin = new Padding(4);
      this.dgvService.Name = "dgvService";
      this.dgvService.ReadOnly = true;
      this.dgvService.RowHeadersVisible = false;
      this.dgvService.Size = new Size(204, 426);
      this.dgvService.TabIndex = 0;
      this.dgvService.VirtualMode = true;
      this.dgvService.CellValueNeeded += new DataGridViewCellValueEventHandler(this.dgvService_CellValueNeeded);
      this.dgvService.DataError += new DataGridViewDataErrorEventHandler(this.dgvService_DataError);
      this.dgvService.SelectionChanged += new EventHandler(this.dgvService_SelectionChanged);
      this.Number.HeaderText = "№";
      this.Number.Name = "Number";
      this.Number.ReadOnly = true;
      this.Number.Width = 40;
      this.Service.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.Service.HeaderText = "Услуга";
      this.Service.Name = "Service";
      this.Service.ReadOnly = true;
      this.flpServ.AutoScroll = true;
      this.flpServ.AutoSize = true;
      this.flpServ.BackColor = Color.AliceBlue;
      this.flpServ.Dock = DockStyle.Fill;
      this.flpServ.Location = new Point(56, 0);
      this.flpServ.Margin = new Padding(4);
      this.flpServ.Name = "flpServ";
      this.flpServ.Size = new Size(696, 426);
      this.flpServ.TabIndex = 2;
      this.panel2.Controls.Add((Control) this.toolStrip1);
      this.panel2.Dock = DockStyle.Left;
      this.panel2.Location = new Point(0, 0);
      this.panel2.Margin = new Padding(4);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(56, 426);
      this.panel2.TabIndex = 1;
      this.hp.HelpNamespace = "Help.chm";
      this.dataGridViewTextBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn1.HeaderText = "Услуги";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.ReadOnly = true;
      this.dataGridViewTextBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn2.HeaderText = "Услуги";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn2.ReadOnly = true;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(961, 572);
      this.Controls.Add((Control) this.splitContainer1);
      this.Controls.Add((Control) this.pnBtn);
      this.Controls.Add((Control) this.groupBox1);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.hp.SetHelpKeyword((Control) this, "kv512.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmServiceParam";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Услуги организаций";
      this.Load += new EventHandler(this.FrmServiceParam_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.pnBtn.ResumeLayout(false);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.Panel2.PerformLayout();
      this.splitContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.dgvService).EndInit();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
