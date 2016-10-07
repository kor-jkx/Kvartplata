// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.Controls.UCTariffCost
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms.Controls
{
  public class UCTariffCost : UCBase
  {
    private bool isPast = false;
    private bool isArchive = false;
    private bool isEdit = false;
    private bool isCopy = false;
    public bool IsCopy = false;
    private IContainer components = (IContainer) null;
    public DateTime? closedPeriod;
    private short company_id;
    private Service curService;
    private IList<cmpTariffCost> listTariff;
    private int oldTariffId;

    public bool IsPast
    {
      get
      {
        return this.isPast;
      }
      set
      {
        if (this.tsbApplay.Enabled)
        {
          string str = "Изменения не сохранены! ";
          if (MessageBox.Show(this.isPast ? str + "Остаться в прошлом времени и сохранить изменения?" : str + "Остаться в настоящем времени и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
          {
            this.newObject = (object) null;
            this.tsbApplay_Click((object) null, (EventArgs) null);
            return;
          }
          this.tsbCancel_Click((object) null, (EventArgs) null);
        }
        this.isPast = value;
        this.dgvBase.Columns["Period"].Visible = this.isPast;
        this.curObject = (object) null;
        this.GetList();
        if (this.objectsList.Count > 0)
        {
          this.CurIndex = 0;
          this.dt = new DateTime?(((cmpTariffCost) this.objectsList[0]).Dbeg);
          this.oldTariffId = ((cmpTariffCost) this.objectsList[0]).Tariff_id;
          this.dgvBase.Rows[0].Selected = true;
        }
        this.GetTariffCostPartList();
      }
    }

    public bool IsArchive
    {
      set
      {
        this.isArchive = value;
        this.curObject = (object) null;
      }
    }

    public short Company_id
    {
      get
      {
        return this.company_id;
      }
      set
      {
        if ((int) this.company_id != (int) value)
        {
          if (this.tsbApplay.Enabled)
          {
            if (MessageBox.Show("Изменения не сохранены! Вернуться к предыдущей компании и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
              this.tsbApplay_Click((object) null, (EventArgs) null);
              return;
            }
            this.tsbCancel_Click((object) null, (EventArgs) null);
          }
          this.company_id = value;
          if (this.curService != null)
          {
            IList list = this.session.CreateQuery(string.Format("from Tariff where Service_id={0} and Manager.BaseOrgId=(select Manager.BaseOrgId from Company where CompanyId={1})", (object) this.curService.ServiceId, (object) this.company_id)).List();
            list.Insert(0, (object) new Tariff()
            {
              Tariff_id = int.MaxValue
            });
            ((DataGridViewComboBoxColumn) this.dgvBase.Columns["Variant"]).DataSource = (object) list;
          }
          this.curObject = (object) null;
          this.GetList();
          if (this.objectsList.Count > 0)
          {
            this.CurIndex = 0;
            this.dt = new DateTime?(((cmpTariffCost) this.objectsList[0]).Dbeg);
            this.dgvBase.CurrentCell = this.dgvBase.Rows[0].Cells[1];
            this.dgvBase.Rows[0].Selected = true;
          }
          this.GetTariffCostPartList();
        }
        else
        {
          if (this.tsbApplay.Enabled)
            return;
          this.GetList();
        }
      }
    }

    public Service CurService
    {
      get
      {
        return this.curService;
      }
      set
      {
        if (this.curService != value)
        {
          if (this.tsbApplay.Enabled)
          {
            if (MessageBox.Show("Изменения не сохранены! Вернуться к предыдущей услуге и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
              this.tsbApplay_Click((object) null, (EventArgs) null);
              return;
            }
            this.tsbCancel_Click((object) null, (EventArgs) null);
          }
          this.curService = value;
          if (this.curService != null)
          {
            IList list = this.session.CreateQuery(string.Format("from Tariff where Service_id={0} and Manager.BaseOrgId=(select Manager.BaseOrgId from Company where CompanyId={1})", (object) this.curService.ServiceId, (object) this.company_id)).List();
            list.Insert(0, (object) new Tariff()
            {
              Tariff_id = int.MaxValue
            });
            ((DataGridViewComboBoxColumn) this.dgvBase.Columns["Variant"]).DataSource = (object) list;
          }
          this.curObject = (object) null;
          this.GetList();
          if (this.objectsList.Count > 0)
          {
            this.CurIndex = 0;
            this.dt = new DateTime?(((cmpTariffCost) this.objectsList[0]).Dbeg);
            this.dgvBase.CurrentCell = this.dgvBase.Rows[0].Cells[1];
            this.dgvBase.Rows[0].Selected = true;
          }
          this.GetTariffCostPartList();
        }
        else
        {
          if (this.tsbApplay.Enabled)
            return;
          this.GetList();
        }
      }
    }

    public cmpTariffCost CurTCost
    {
      get
      {
        return (cmpTariffCost) this.curObject;
      }
    }

    public IList TariffCostPartList { get; set; }

    public virtual event EventHandler TariffCostPartListChanged;

    public virtual event EventHandler ApplayClick;

    public virtual event EventHandler CancelClick;

    public UCTariffCost()
    {
      this.InitializeComponent();
      DataGridViewColumn dataGridViewColumn1 = (DataGridViewColumn) new MaskDateColumn();
      dataGridViewColumn1.Name = "Period";
      dataGridViewColumn1.HeaderText = "Период";
      dataGridViewColumn1.Visible = false;
      dataGridViewColumn1.ReadOnly = true;
      this.dgvBase.Columns.Add(dataGridViewColumn1);
      DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn2.Name = "NVariant";
      dataGridViewColumn2.HeaderText = "№";
      this.dgvBase.Columns.Add(dataGridViewColumn2);
      DataGridViewColumn dataGridViewColumn3 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn3.Name = "Variant";
      dataGridViewColumn3.HeaderText = "Вариант";
      ((DataGridViewComboBoxColumn) dataGridViewColumn3).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      ((DataGridViewComboBoxColumn) dataGridViewColumn3).DisplayMember = "Tariff_name";
      ((DataGridViewComboBoxColumn) dataGridViewColumn3).ValueMember = "Tariff_id";
      this.dgvBase.Columns.Add(dataGridViewColumn3);
      DataGridViewColumn dataGridViewColumn4 = (DataGridViewColumn) new MaskDateColumn();
      dataGridViewColumn4.Name = "Dbeg";
      dataGridViewColumn4.HeaderText = "Дата начала действия";
      this.dgvBase.Columns.Add(dataGridViewColumn4);
      DataGridViewColumn dataGridViewColumn5 = (DataGridViewColumn) new MaskDateColumn();
      dataGridViewColumn5.Name = "Dend";
      dataGridViewColumn5.HeaderText = "Дата окончания действия";
      this.dgvBase.Columns.Add(dataGridViewColumn5);
      DataGridViewColumn dataGridViewColumn6 = (DataGridViewColumn) new DataGridViewButtonColumn();
      dataGridViewColumn6.Name = "Scheme";
      dataGridViewColumn6.HeaderText = "Алгоритм расчета";
      this.dgvBase.Columns.Add(dataGridViewColumn6);
      DataGridViewColumn dataGridViewColumn7 = (DataGridViewColumn) new DataGridViewButtonColumn();
      dataGridViewColumn7.Name = "SchemeParam";
      dataGridViewColumn7.HeaderText = "Схема параметров расчета";
      this.dgvBase.Columns.Add(dataGridViewColumn7);
      DataGridViewColumn dataGridViewColumn8 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn8.Name = "UnitMeasuring_id";
      dataGridViewColumn8.HeaderText = "Единица измерения";
      ((DataGridViewComboBoxColumn) dataGridViewColumn8).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      ((DataGridViewComboBoxColumn) dataGridViewColumn8).DisplayMember = "UnitMeasuring_name";
      ((DataGridViewComboBoxColumn) dataGridViewColumn8).ValueMember = "UnitMeasuring_id";
      this.dgvBase.Columns.Add(dataGridViewColumn8);
      DataGridViewColumn dataGridViewColumn9 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn9.Name = "BaseTariff_id";
      dataGridViewColumn9.HeaderText = "На кого";
      ((DataGridViewComboBoxColumn) dataGridViewColumn9).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      ((DataGridViewComboBoxColumn) dataGridViewColumn9).DisplayMember = "BaseTariff_name";
      ((DataGridViewComboBoxColumn) dataGridViewColumn9).ValueMember = "BaseTariff_id";
      this.dgvBase.Columns.Add(dataGridViewColumn9);
      DataGridViewColumn dataGridViewColumn10 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn10.Name = "BaseTariffMSP_id";
      dataGridViewColumn10.HeaderText = "Область применения";
      ((DataGridViewComboBoxColumn) dataGridViewColumn10).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      ((DataGridViewComboBoxColumn) dataGridViewColumn10).DisplayMember = "BaseTariff_name";
      ((DataGridViewComboBoxColumn) dataGridViewColumn10).ValueMember = "BaseTariffMSP_id";
      this.dgvBase.Columns.Add(dataGridViewColumn10);
      DataGridViewColumn dataGridViewColumn11 = (DataGridViewColumn) new DataGridViewComboBoxColumn();
      dataGridViewColumn11.Name = "IsVat";
      dataGridViewColumn11.HeaderText = "Выделять НДС";
      ((DataGridViewComboBoxColumn) dataGridViewColumn11).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      ((DataGridViewComboBoxColumn) dataGridViewColumn11).DisplayMember = "YesNoName";
      ((DataGridViewComboBoxColumn) dataGridViewColumn11).ValueMember = "YesNoId";
      this.dgvBase.Columns.Add(dataGridViewColumn11);
      DataGridViewColumn dataGridViewColumn12 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn12.Name = "Cost";
      dataGridViewColumn12.HeaderText = "Тариф";
      DataGridViewCellStyle gridViewCellStyle = new DataGridViewCellStyle();
      gridViewCellStyle.Format = "N4";
      dataGridViewColumn12.ReadOnly = true;
      dataGridViewColumn12.DefaultCellStyle = gridViewCellStyle;
      this.dgvBase.Columns.Add(dataGridViewColumn12);
      DataGridViewColumn dataGridViewColumn13 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn13.Name = "Cost_eo";
      dataGridViewColumn13.HeaderText = "Экономически обоснованный тариф";
      dataGridViewColumn13.DefaultCellStyle = new DataGridViewCellStyle()
      {
        Format = "N4"
      };
      dataGridViewColumn13.ReadOnly = true;
      this.dgvBase.Columns.Add(dataGridViewColumn13);
      DataGridViewColumn dataGridViewColumn14 = (DataGridViewColumn) new DataGridViewTextBoxColumn();
      dataGridViewColumn14.Name = "Cost_c";
      dataGridViewColumn14.ReadOnly = true;
      dataGridViewColumn14.HeaderText = "Тариф для компенсаций";
      dataGridViewColumn14.DefaultCellStyle = new DataGridViewCellStyle()
      {
        Format = "N4"
      };
      this.dgvBase.Columns.Add(dataGridViewColumn14);
      if (Options.ViewEdit)
      {
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 14, "Пользователь", "UName", 80, true);
        KvrplHelper.AddTextBoxColumn(this.dgvBase, 15, "Дата редактирования", "DEdit", 100, true);
      }
      this.dgvBase.CellValueChanged += new DataGridViewCellEventHandler(this.dgvBase_CellValueChanged);
      this.dgvBase.CellClick += new DataGridViewCellEventHandler(this.dgvBase_CellClick);
      this.MySettings.GridName = "TariffCost";
    }

    protected override void GetList()
    {
      if (this.session != null && this.session.IsOpen)
        this.session.Clear();
      this.objectsList.Clear();
      if (this.session == null || this.curService == null)
        return;
      DateTime dateTime;
      if (this.closedPeriod.HasValue && !this.isArchive)
      {
        if (this.isPast)
        {
          this.objectsList = this.session.CreateQuery(string.Format("select tc from cmpTariffCost tc,Tariff t where t.Tariff_id=tc.Tariff_id and Period_id<>{0} and tc.Service.ServiceId={1} and company_id={2} and Complex_id={3} and period_id=:period order by period_id,Tariff_num,Dbeg", (object) 0, (object) this.CurService.ServiceId, (object) this.Company_id, (object) Options.Complex.IdFk)).SetInt32("period", Options.Period.PeriodId).List();
        }
        else
        {
          IQuery query = this.session.CreateQuery(string.Format("select tc from cmpTariffCost tc,Tariff t where t.Tariff_id=tc.Tariff_id and Period_id={0} and tc.Service.ServiceId={1} and Complex_id={2} and company_id=:id and dend>=:dend order by Tariff_num,Dbeg", (object) 0, (object) this.CurService.ServiceId, (object) Options.Complex.IdFk)).SetInt16("id", this.Company_id);
          string name = "dend";
          dateTime = this.closedPeriod.Value;
          DateTime val = dateTime.AddMonths(1);
          this.objectsList = query.SetDateTime(name, val).List();
        }
      }
      else if (this.isPast)
        this.objectsList = this.session.CreateQuery(string.Format("select tc from cmpTariffCost tc,Tariff t where t.Tariff_id=tc.Tariff_id and Period_id<>{0} and tc.Service.ServiceId={1} and company_id={2} and Complex_id={3} order by period_id,Tariff_num,Dbeg", (object) 0, (object) this.CurService.ServiceId, (object) this.Company_id, (object) Options.Complex.IdFk)).List();
      else
        this.objectsList = this.session.CreateQuery(string.Format("select tc from cmpTariffCost tc,Tariff t where t.Tariff_id=tc.Tariff_id and Period_id={0} and tc.Service.ServiceId={1} and Complex_id={2} and company_id=:id order by Tariff_num,Dbeg", (object) 0, (object) this.CurService.ServiceId, (object) Options.Complex.IdFk)).SetInt16("id", this.Company_id).List();
      this.dgvBase.RowCount = this.objectsList.Count;
      this.dgvBase.Refresh();
      this.SelectRow();
      if (this.objectsList.Count > 0)
      {
        this.session.Clear();
        if (this.closedPeriod.HasValue && !this.isArchive)
        {
          if (this.isPast)
          {
            this.listTariff = this.session.CreateQuery(string.Format("select tc from cmpTariffCost tc,Tariff t where t.Tariff_id=tc.Tariff_id and Period_id<>{0} and tc.Service.ServiceId={1} and company_id={2} and Complex_id={3} and period_id=:period order by period_id,Tariff_num,Dbeg", (object) 0, (object) this.CurService.ServiceId, (object) this.Company_id, (object) Options.Complex.IdFk)).SetInt32("period", Options.Period.PeriodId).List<cmpTariffCost>();
          }
          else
          {
            IQuery query = this.session.CreateQuery(string.Format("select tc from cmpTariffCost tc,Tariff t where t.Tariff_id=tc.Tariff_id and Period_id={0} and tc.Service.ServiceId={1} and Complex_id={2} and company_id=:id and dend>=:dend order by Tariff_num,Dbeg", (object) 0, (object) this.CurService.ServiceId, (object) Options.Complex.IdFk)).SetInt16("id", this.Company_id);
            string name = "dend";
            dateTime = this.closedPeriod.Value;
            DateTime val = dateTime.AddMonths(1);
            this.listTariff = query.SetDateTime(name, val).List<cmpTariffCost>();
          }
        }
        else if (this.isPast)
          this.listTariff = this.session.CreateQuery(string.Format("select tc from cmpTariffCost tc,Tariff t where t.Tariff_id=tc.Tariff_id and Period_id<>{0} and tc.Service.ServiceId={1} and company_id={2} and Complex_id={3} order by period_id,Tariff_num,Dbeg", (object) 0, (object) this.CurService.ServiceId, (object) this.Company_id, (object) Options.Complex.IdFk)).List<cmpTariffCost>();
        else
          this.listTariff = this.session.CreateQuery(string.Format("select tc from cmpTariffCost tc,Tariff t where t.Tariff_id=tc.Tariff_id and Period_id={0} and tc.Service.ServiceId={1} and Complex_id={2} and company_id=:id order by Tariff_num,Dbeg", (object) 0, (object) this.CurService.ServiceId, (object) Options.Complex.IdFk)).SetInt16("id", this.Company_id).List<cmpTariffCost>();
        int index = 0;
        foreach (cmpTariffCost cmpTariffCost in (IEnumerable<cmpTariffCost>) this.listTariff)
        {
          cmpTariffCost.OldHashCode = cmpTariffCost.GetHashCode();
          ((cmpTariffCost) this.objectsList[index]).OldHashCode = cmpTariffCost.OldHashCode;
          ++index;
        }
      }
    }

    private void GetTariffCostPartList()
    {
      if (this.curObject != null)
      {
        if (this.isPast)
          this.TariffCostPartList = this.session.CreateQuery(string.Format("from cmpTariffCost where Period_id={0} and Service_id<>{1} and Tariff_id={2} and Dbeg=:dt and company_id={3} order by Service_id", (object) Options.Period.PeriodId, (object) this.curService.ServiceId, (object) ((cmpTariffCost) this.curObject).Tariff_id, (object) this.company_id)).SetDateTime("dt", ((cmpTariffCost) this.curObject).Dbeg).List();
        else
          this.TariffCostPartList = this.session.CreateQuery(string.Format("from cmpTariffCost where Period_id={0} and Service_id<>{1} and tariff_id={2} and Dbeg=:dt and company_id={3} order by Service_id", (object) 0, (object) this.curService.ServiceId, (object) ((cmpTariffCost) this.curObject).Tariff_id, (object) this.company_id)).SetDateTime("dt", ((cmpTariffCost) this.curObject).Dbeg).List();
      }
      else if (this.TariffCostPartList != null && !this.isCopy)
        this.TariffCostPartList.Clear();
      // ISSUE: reference to a compiler-generated field
      if (this.TariffCostPartListChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.TariffCostPartListChanged((object) this, EventArgs.Empty);
    }

    public void LoadData()
    {
      this.GetList();
      this.GetTariffCostPartList();
    }

    public void SaveData()
    {
      this.tsbApplay_Click((object) this.tsbApplay, EventArgs.Empty);
    }

    protected override void dgvBase_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.objectsList.Count <= 0)
        return;
      if (this.isPast)
        ((cmpTariffCost) this.objectsList[e.RowIndex]).Period = this.session.Get<Period>((object) ((cmpTariffCost) this.objectsList[e.RowIndex]).Period.PeriodId);
      if (this.closedPeriod.HasValue)
      {
        int num;
        if (this.isPast || !(((cmpTariffCost) this.objectsList[e.RowIndex]).Dbeg < this.closedPeriod.Value.AddMonths(2)) || !(((cmpTariffCost) this.objectsList[e.RowIndex]).Dend >= this.closedPeriod.Value.AddMonths(1)))
        {
          if (this.isPast)
          {
            DateTime? periodName = ((cmpTariffCost) this.objectsList[e.RowIndex]).Period.PeriodName;
            DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
            num = periodName.HasValue ? (periodName.HasValue ? (periodName.GetValueOrDefault() == dateTime ? 1 : 0) : 1) : 0;
          }
          else
            num = 0;
        }
        else
          num = 1;
        if (num != 0)
          this.dgvBase.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.PapayaWhip;
        else
          this.dgvBase.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      }
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "NVariant")
      {
        Tariff tariff = this.session.Get<Tariff>((object) ((cmpTariffCost) this.objectsList[e.RowIndex]).Tariff_id);
        if (tariff != null)
          e.Value = (object) tariff.Tariff_num;
      }
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Variant")
        e.Value = (object) Convert.ToInt32(((cmpTariffCost) this.objectsList[e.RowIndex]).Tariff_id);
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dbeg")
        e.Value = (object) ((cmpTariffCost) this.objectsList[e.RowIndex]).Dbeg;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dend")
        e.Value = (object) ((cmpTariffCost) this.objectsList[e.RowIndex]).Dend;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Cost")
        e.Value = (object) ((cmpTariffCost) this.objectsList[e.RowIndex]).Cost;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Scheme")
        e.Value = (object) ((cmpTariffCost) this.objectsList[e.RowIndex]).Scheme;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "SchemeParam")
        e.Value = (object) ((cmpTariffCost) this.objectsList[e.RowIndex]).SchemeParam;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "UnitMeasuring_id")
        e.Value = (object) ((cmpTariffCost) this.objectsList[e.RowIndex]).UnitMeasuring_id;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "BaseTariff_id")
        e.Value = (object) ((cmpTariffCost) this.objectsList[e.RowIndex]).BaseTariff_id;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "BaseTariffMSP_id")
        e.Value = (object) ((cmpTariffCost) this.objectsList[e.RowIndex]).BaseTariffMSP_id;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "IsVat" && ((cmpTariffCost) this.objectsList[e.RowIndex]).IsVat != null)
        e.Value = (object) ((cmpTariffCost) this.objectsList[e.RowIndex]).IsVat.YesNoId;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Cost_eo")
        e.Value = (object) ((cmpTariffCost) this.objectsList[e.RowIndex]).Cost_eo;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Cost_c")
        e.Value = (object) ((cmpTariffCost) this.objectsList[e.RowIndex]).Cost_c;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Period" && this.isPast)
        e.Value = (object) ((cmpTariffCost) this.objectsList[e.RowIndex]).Period.PeriodName;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "UName")
        e.Value = (object) ((cmpTariffCost) this.objectsList[e.RowIndex]).Uname;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "DEdit")
        e.Value = (object) ((cmpTariffCost) this.objectsList[e.RowIndex]).Dedit.ToShortDateString();
    }

    protected override void dgvBase_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dbeg" || this.dgvBase.Columns[e.ColumnIndex].Name == "Dend")
      {
        try
        {
          Convert.ToDateTime(e.Value);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Проверьте правильность введенных дат", "Внимание!", MessageBoxButtons.OK);
          this.isEdit = false;
          return;
        }
      }
      if (!this.isPast && this.dgvBase.Columns[e.ColumnIndex].Name == "Dbeg" && this.closedPeriod.Value.AddMonths(1) <= ((cmpTariffCost) this.objectsList[e.RowIndex]).Dbeg && this.closedPeriod.Value.AddMonths(1) > Convert.ToDateTime(e.Value))
      {
        int num = (int) MessageBox.Show("Попытка изменить дату начала на дату, принадлежащую закрытому периоду!", "Внимание!", MessageBoxButtons.OK);
        this.isEdit = false;
      }
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dend" && ((cmpTariffCost) this.objectsList[e.RowIndex]).Dbeg > Convert.ToDateTime(e.Value))
      {
        int num = (int) MessageBox.Show("Дата окончания не может быть меньше даты начала!", "Внимание!", MessageBoxButtons.OK);
        this.isEdit = false;
      }
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dbeg" && ((cmpTariffCost) this.objectsList[e.RowIndex]).Dend < Convert.ToDateTime(e.Value))
      {
        int num = (int) MessageBox.Show("Дата начала не может быть больше даты окончания!", "Внимание!", MessageBoxButtons.OK);
        this.isEdit = false;
      }
      else if (this.session.CreateCriteria(typeof (ServiceParam)).Add((ICriterion) Restrictions.Eq("Company_id", (object) this.company_id)).Add((ICriterion) Restrictions.Eq("Complex.IdFk", (object) Options.Complex.IdFk)).Add((ICriterion) Restrictions.Eq("Service_id", (object) this.curService.ServiceId)).List<ServiceParam>().Count == 0)
      {
        int num1 = (int) MessageBox.Show("Невозможно внести изменения. Услуга отсутствует в списке услуг организаций", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        int num2;
        if (e.RowIndex < 0 || this.objectsList.Count <= 0 || this.closedPeriod.HasValue)
        {
          if (this.objectsList.Count > 0 && this.closedPeriod.Value.AddMonths(1) <= ((cmpTariffCost) this.objectsList[e.RowIndex]).Dbeg || this.isPast)
          {
            if (this.isPast)
            {
              DateTime? periodName = ((cmpTariffCost) this.objectsList[e.RowIndex]).Period.PeriodName;
              if (periodName.HasValue)
              {
                DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
                periodName = ((cmpTariffCost) this.objectsList[e.RowIndex]).Period.PeriodName;
                if ((periodName.HasValue ? (dateTime <= periodName.GetValueOrDefault() ? 1 : 0) : 0) != 0)
                  goto label_18;
              }
            }
            else
              goto label_18;
          }
          num2 = this.dgvBase.Columns[e.ColumnIndex].Name == "Dend" ? 1 : 0;
          goto label_19;
        }
label_18:
        num2 = 1;
label_19:
        if (num2 != 0)
        {
          if (e.RowIndex >= 0)
          {
            this.tsbApplay.Enabled = true;
            this.tsbCancel.Enabled = true;
            this.tsbDelete.Enabled = false;
            this.tsbAdd.Enabled = false;
            if (this.dgvBase.Columns[e.ColumnIndex].Name == "NVariant")
            {
              Tariff tariff;
              try
              {
                tariff = this.session.CreateQuery("from Tariff where tariff_num=:num and Service=:srv and Manager.BaseOrgId=(select Manager.BaseOrgId from Company where CompanyId=:company)").SetParameter("num", e.Value).SetEntity("srv", (object) this.curService).SetParameter<short>("company", this.company_id).UniqueResult<Tariff>();
              }
              catch
              {
                int num3 = (int) MessageBox.Show("Данный номер варианта не уникален, выберите вариант из списка!", "Внимание!", MessageBoxButtons.OK);
                return;
              }
              if (tariff != null)
                ((cmpTariffCost) this.objectsList[e.RowIndex]).Tariff_id = tariff.Tariff_id;
              this.dgvBase.Refresh();
            }
            else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Variant")
            {
              ((cmpTariffCost) this.objectsList[e.RowIndex]).Tariff_id = Convert.ToInt32(e.Value);
              this.dgvBase.Refresh();
            }
            else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dbeg")
            {
              if (this.isPast && Convert.ToDateTime(e.Value) < Convert.ToDateTime("01.01.2005"))
              {
                int num3 = (int) MessageBox.Show("Дата начала должна быть больше 01.01.2005 г.", "Внимание!", MessageBoxButtons.OK);
                this.isEdit = false;
                return;
              }
              if (this.isPast && this.closedPeriod.HasValue && Convert.ToDateTime(e.Value) >= this.closedPeriod.Value.AddMonths(1))
              {
                int num3 = (int) MessageBox.Show("Дата начала не может быть больше даты окончания закрытого периода!", "Внимание!", MessageBoxButtons.OK);
                this.isEdit = false;
                return;
              }
              try
              {
                ((cmpTariffCost) this.objectsList[e.RowIndex]).Dbeg = Convert.ToDateTime(e.Value);
              }
              catch (Exception ex)
              {
                int num3 = (int) MessageBox.Show("Проверьте правильность ввода даты!", "Внимание!", MessageBoxButtons.OK);
                this.isEdit = false;
                return;
              }
            }
            else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Dend")
            {
              try
              {
                DateTime dateTime1;
                int num3;
                if (this.closedPeriod.HasValue && !this.isPast)
                {
                  DateTime dateTime2 = Convert.ToDateTime(e.Value);
                  dateTime1 = this.closedPeriod.Value;
                  dateTime1 = dateTime1.AddMonths(1);
                  DateTime dateTime3 = dateTime1.AddDays(-1.0);
                  if (dateTime2 >= dateTime3)
                  {
                    DateTime dend = ((cmpTariffCost) this.objectsList[e.RowIndex]).Dend;
                    dateTime1 = this.closedPeriod.Value;
                    dateTime1 = dateTime1.AddMonths(1);
                    DateTime dateTime4 = dateTime1.AddDays(-1.0);
                    num3 = dend >= dateTime4 ? 1 : 0;
                  }
                  else
                    num3 = 0;
                }
                else
                  num3 = 1;
                if (num3 != 0)
                {
                  int num4;
                  if (this.isPast && this.closedPeriod.HasValue)
                  {
                    DateTime dateTime2 = Convert.ToDateTime(e.Value);
                    dateTime1 = this.closedPeriod.Value;
                    DateTime dateTime3 = dateTime1.AddMonths(1);
                    num4 = dateTime2 >= dateTime3 ? 1 : 0;
                  }
                  else
                    num4 = 0;
                  if (num4 != 0)
                  {
                    int num5 = (int) MessageBox.Show("Дата окончания не может быть больше даты окончания закрытого периода!", "Внимание!", MessageBoxButtons.OK);
                    this.isEdit = false;
                    return;
                  }
                  ((cmpTariffCost) this.objectsList[e.RowIndex]).Dend = Convert.ToDateTime(e.Value);
                }
                else
                {
                  int num4 = (int) MessageBox.Show("Дата окончания принадлежит закрытому периоду, либо существует запись, принадлежащая закрытому периоду, которая перекрывается этой датой!", "Внимание!", MessageBoxButtons.OK);
                  this.isEdit = false;
                  return;
                }
              }
              catch
              {
                int num3 = (int) MessageBox.Show("Проверьте правильность ввода даты!", "Внимание!", MessageBoxButtons.OK);
                this.isEdit = false;
                return;
              }
            }
            else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Cost" && e.Value != null)
            {
              try
              {
                ((cmpTariffCost) this.objectsList[e.RowIndex]).Cost = new double?(Convert.ToDouble(e.Value));
              }
              catch
              {
                int num3 = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
                this.isEdit = false;
                return;
              }
            }
            else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Scheme")
            {
              try
              {
                if (e.Value == null)
                  ((cmpTariffCost) this.objectsList[e.RowIndex]).Scheme = new short?();
                else
                  ((cmpTariffCost) this.objectsList[e.RowIndex]).Scheme = new short?(Convert.ToInt16(e.Value));
              }
              catch
              {
                int num3 = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
                this.isEdit = false;
                return;
              }
            }
            else if (this.dgvBase.Columns[e.ColumnIndex].Name == "SchemeParam")
            {
              try
              {
                if (e.Value == null)
                  ((cmpTariffCost) this.objectsList[e.RowIndex]).SchemeParam = new short?();
                else
                  ((cmpTariffCost) this.objectsList[e.RowIndex]).SchemeParam = new short?(Convert.ToInt16(e.Value));
              }
              catch
              {
                int num3 = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
                this.isEdit = false;
                return;
              }
            }
            else if (this.dgvBase.Columns[e.ColumnIndex].Name == "UnitMeasuring_id" && e.Value != null)
              ((cmpTariffCost) this.objectsList[e.RowIndex]).UnitMeasuring_id = new int?(Convert.ToInt32(e.Value));
            else if (this.dgvBase.Columns[e.ColumnIndex].Name == "BaseTariff_id" && e.Value != null)
              ((cmpTariffCost) this.objectsList[e.RowIndex]).BaseTariff_id = new int?(Convert.ToInt32(e.Value));
            else if (this.dgvBase.Columns[e.ColumnIndex].Name == "BaseTariffMSP_id" && e.Value != null)
              ((cmpTariffCost) this.objectsList[e.RowIndex]).BaseTariffMSP_id = new int?(Convert.ToInt32(e.Value));
            else if (this.dgvBase.Columns[e.ColumnIndex].Name == "IsVat" && e.Value != null)
              ((cmpTariffCost) this.objectsList[e.RowIndex]).IsVat = this.session.Get<YesNo>((object) Convert.ToInt16(e.Value));
            else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Cost_eo" && e.Value != null)
            {
              try
              {
                ((cmpTariffCost) this.objectsList[e.RowIndex]).Cost_eo = new double?(Convert.ToDouble(e.Value));
              }
              catch
              {
                int num3 = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
                this.isEdit = false;
                return;
              }
            }
            else if (this.dgvBase.Columns[e.ColumnIndex].Name == "Cost_c" && e.Value != null)
            {
              try
              {
                ((cmpTariffCost) this.objectsList[e.RowIndex]).Cost_c = new double?(Convert.ToDouble(e.Value));
              }
              catch
              {
                int num3 = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
                this.isEdit = false;
                return;
              }
            }
            this.curObject = this.objectsList[e.RowIndex];
          }
          this.isEdit = true;
        }
        else
        {
          int num3 = (int) MessageBox.Show("Попытка внести изменения в закрытом периоде!", "Внимание!", MessageBoxButtons.OK);
          this.isEdit = false;
        }
      }
    }

    protected override void tsbAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, this.session.Get<Company>((object) this.company_id), true))
        return;
      this.newObject = (object) new cmpTariffCost();
      ((cmpTariffCost) this.newObject).Tariff_id = int.MaxValue;
      ((cmpTariffCost) this.newObject).IsVat = this.session.Get<YesNo>((object) Convert.ToInt16(0));
      this.CurIndex = -1;
      if (this.closedPeriod.HasValue)
      {
        if (this.isPast)
          ((cmpTariffCost) this.newObject).Dbeg = this.closedPeriod.Value;
        else
          ((cmpTariffCost) this.newObject).Dbeg = this.closedPeriod.Value.AddMonths(1);
      }
      else
        ((cmpTariffCost) this.newObject).Dbeg = DateTime.Now;
      if (!this.isPast)
        ((cmpTariffCost) this.newObject).Period = this.session.Get<Period>((object) Convert.ToInt32(this.session.CreateQuery("select p.PeriodId from Period p where PeriodName is null").UniqueResult()));
      else if (this.closedPeriod.HasValue)
      {
        ((cmpTariffCost) this.newObject).Period = Options.Period;
      }
      else
      {
        int num = (int) MessageBox.Show("Отсутствует закрытый период!", "Внимание!", MessageBoxButtons.OK);
        return;
      }
      ((cmpTariffCost) this.newObject).Company_id = (int) this.Company_id;
      ((cmpTariffCost) this.newObject).Service = this.CurService;
      cmpTariffCost newObject1 = (cmpTariffCost) this.newObject;
      cmpTariffCost newObject2 = (cmpTariffCost) this.newObject;
      cmpTariffCost newObject3 = (cmpTariffCost) this.newObject;
      double? nullable1 = new double?(0.0);
      double? nullable2 = nullable1;
      newObject3.Cost_eo = nullable2;
      double? nullable3;
      double? nullable4 = nullable3 = nullable1;
      newObject2.Cost_c = nullable3;
      double? nullable5 = nullable4;
      newObject1.Cost = nullable5;
      cmpTariffCost newObject4 = (cmpTariffCost) this.newObject;
      cmpTariffCost newObject5 = (cmpTariffCost) this.newObject;
      short? nullable6 = new short?((short) 0);
      short? nullable7 = nullable6;
      newObject5.SchemeParam = nullable7;
      short? nullable8 = nullable6;
      newObject4.Scheme = nullable8;
      if (this.closedPeriod.HasValue && this.isPast)
        ((cmpTariffCost) this.newObject).Dend = this.closedPeriod.Value.AddMonths(1).AddDays(-1.0);
      else
        ((cmpTariffCost) this.newObject).Dend = Convert.ToDateTime("31.12.2999");
      base.tsbAdd_Click(sender, e);
      this.dt = new DateTime?(((cmpTariffCost) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg);
      this.SelectRow();
    }

    protected override void tsbApplay_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, this.session.Get<Company>((object) this.company_id), true))
        return;
      this.dgvBase.EndEdit();
      if (!this.isEdit)
      {
        this.isEdit = true;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.ApplayClick != null)
        {
          // ISSUE: reference to a compiler-generated field
          this.ApplayClick((object) this, EventArgs.Empty);
        }
        Decimal num1 = new Decimal();
        Decimal num2 = new Decimal();
        Decimal num3 = new Decimal();
        foreach (cmpTariffCost tariffCostPart in (IEnumerable) this.TariffCostPartList)
        {
          double? nullable = tariffCostPart.Cost;
          if (nullable.HasValue)
          {
            Decimal num4 = num1;
            nullable = tariffCostPart.Cost;
            Decimal num5 = Convert.ToDecimal(nullable.Value);
            num1 = num4 + num5;
          }
          nullable = tariffCostPart.Cost_eo;
          if (nullable.HasValue)
          {
            Decimal num4 = num2;
            nullable = tariffCostPart.Cost_eo;
            Decimal num5 = Convert.ToDecimal(nullable.Value);
            num2 = num4 + num5;
          }
          nullable = tariffCostPart.Cost_c;
          if (nullable.HasValue)
          {
            Decimal num4 = num3;
            nullable = tariffCostPart.Cost_c;
            Decimal num5 = Convert.ToDecimal(nullable.Value);
            num3 = num4 + num5;
          }
        }
        if (this.newObject != null)
        {
          if (num1 == Decimal.Zero && num2 == Decimal.Zero && num3 == Decimal.Zero)
          {
            int num4 = (int) MessageBox.Show("Сумма тарифов не должна быть равной 0", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          double? nullable1;
          int num5;
          if (!(num1 == Convert.ToDecimal((object) ((cmpTariffCost) this.newObject).Cost)))
          {
            nullable1 = ((cmpTariffCost) this.newObject).Cost;
            num5 = nullable1.HasValue ? 0 : (num1 == Decimal.Zero ? 1 : 0);
          }
          else
            num5 = 1;
          if (num5 != 0)
          {
            int num4;
            if (!(num2 == Convert.ToDecimal((object) ((cmpTariffCost) this.newObject).Cost_eo)))
            {
              nullable1 = ((cmpTariffCost) this.newObject).Cost_eo;
              num4 = nullable1.HasValue ? 0 : (num2 == Decimal.Zero ? 1 : 0);
            }
            else
              num4 = 1;
            if (num4 != 0)
            {
              int num6;
              if (!(num3 == Convert.ToDecimal((object) ((cmpTariffCost) this.newObject).Cost_c)))
              {
                nullable1 = ((cmpTariffCost) this.newObject).Cost_c;
                num6 = nullable1.HasValue ? 0 : (num3 == Decimal.Zero ? 1 : 0);
              }
              else
                num6 = 1;
              if (num6 != 0)
              {
                if (this.closedPeriod.HasValue && ((cmpTariffCost) this.newObject).Dbeg < this.closedPeriod.Value.AddMonths(1) && !this.isPast)
                {
                  int num7 = (int) MessageBox.Show("Дата начала принадлежит закрытому периоду!", "Внимание!", MessageBoxButtons.OK);
                }
                else
                {
                  if (((cmpTariffCost) this.newObject).Tariff_id == int.MaxValue)
                  {
                    int num8 = (int) MessageBox.Show("Выберите вариант!", "Внимание!", MessageBoxButtons.OK);
                    return;
                  }
                  if (!((cmpTariffCost) this.newObject).BaseTariff_id.HasValue)
                  {
                    int num8 = (int) MessageBox.Show("Не выбрано на кого распространяется тариф!", "Внимание!", MessageBoxButtons.OK);
                    return;
                  }
                  if ((((cmpTariffCost) this.newObject).Dbeg <= DateTime.Now.AddYears(-3) || ((cmpTariffCost) this.newObject).Dbeg >= DateTime.Now.AddYears(3)) && MessageBox.Show("Дата начала отличается от текущей более, чем на 3 года. Продолжить", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                    return;
                  int? nullable2 = ((cmpTariffCost) this.newObject).BaseTariff_id;
                  int num9 = 1;
                  int? nullable3;
                  int num10;
                  if ((nullable2.GetValueOrDefault() >= num9 ? (nullable2.HasValue ? 1 : 0) : 0) != 0)
                  {
                    nullable2 = ((cmpTariffCost) this.newObject).BaseTariff_id;
                    int num8 = 3;
                    if ((nullable2.GetValueOrDefault() <= num8 ? (nullable2.HasValue ? 1 : 0) : 0) != 0)
                    {
                      nullable2 = ((cmpTariffCost) this.newObject).BaseTariffMSP_id;
                      nullable3 = ((cmpTariffCost) this.newObject).BaseTariff_id;
                      if ((nullable2.GetValueOrDefault() == nullable3.GetValueOrDefault() ? (nullable2.HasValue != nullable3.HasValue ? 1 : 0) : 1) != 0)
                      {
                        num10 = 1;
                        goto label_45;
                      }
                    }
                  }
                  nullable3 = ((cmpTariffCost) this.newObject).BaseTariffMSP_id;
                  num10 = !nullable3.HasValue ? 1 : 0;
label_45:
                  if (num10 != 0)
                  {
                    int num8 = (int) MessageBox.Show("Поле 'Область применения' будет изменено аналогично полю 'На кого'. В случаях 'на человека','на площадь' и 'на семью' данные поля не могут различаться.", "Внимание!", MessageBoxButtons.OK);
                    ((cmpTariffCost) this.newObject).BaseTariffMSP_id = ((cmpTariffCost) this.newObject).BaseTariff_id;
                  }
                  nullable3 = ((cmpTariffCost) this.newObject).UnitMeasuring_id;
                  if (!nullable3.HasValue)
                  {
                    int num8 = (int) MessageBox.Show("Не выбрана единица измерения!", "Внимание!", MessageBoxButtons.OK);
                    return;
                  }
                  short? nullable4 = ((cmpTariffCost) this.newObject).SchemeParam;
                  int? nullable5;
                  if (!nullable4.HasValue)
                  {
                    nullable2 = new int?();
                    nullable5 = nullable2;
                  }
                  else
                    nullable5 = new int?((int) nullable4.GetValueOrDefault());
                  nullable3 = nullable5;
                  int num11 = 131;
                  int num12;
                  if ((nullable3.GetValueOrDefault() == num11 ? (nullable3.HasValue ? 1 : 0) : 0) == 0)
                  {
                    nullable4 = ((cmpTariffCost) this.newObject).SchemeParam;
                    int? nullable6;
                    if (!nullable4.HasValue)
                    {
                      nullable2 = new int?();
                      nullable6 = nullable2;
                    }
                    else
                      nullable6 = new int?((int) nullable4.GetValueOrDefault());
                    nullable3 = nullable6;
                    int num8 = 132;
                    if ((nullable3.GetValueOrDefault() == num8 ? (nullable3.HasValue ? 1 : 0) : 0) == 0)
                    {
                      nullable4 = ((cmpTariffCost) this.newObject).SchemeParam;
                      int? nullable7;
                      if (!nullable4.HasValue)
                      {
                        nullable2 = new int?();
                        nullable7 = nullable2;
                      }
                      else
                        nullable7 = new int?((int) nullable4.GetValueOrDefault());
                      nullable3 = nullable7;
                      int num13 = 141;
                      if ((nullable3.GetValueOrDefault() == num13 ? (nullable3.HasValue ? 1 : 0) : 0) == 0)
                      {
                        nullable4 = ((cmpTariffCost) this.newObject).SchemeParam;
                        int? nullable8;
                        if (!nullable4.HasValue)
                        {
                          nullable2 = new int?();
                          nullable8 = nullable2;
                        }
                        else
                          nullable8 = new int?((int) nullable4.GetValueOrDefault());
                        nullable3 = nullable8;
                        int num14 = 142;
                        if ((nullable3.GetValueOrDefault() == num14 ? (nullable3.HasValue ? 1 : 0) : 0) == 0)
                        {
                          nullable4 = ((cmpTariffCost) this.newObject).SchemeParam;
                          int? nullable9;
                          if (!nullable4.HasValue)
                          {
                            nullable2 = new int?();
                            nullable9 = nullable2;
                          }
                          else
                            nullable9 = new int?((int) nullable4.GetValueOrDefault());
                          nullable3 = nullable9;
                          int num15 = 172;
                          if ((nullable3.GetValueOrDefault() == num15 ? (nullable3.HasValue ? 1 : 0) : 0) == 0)
                          {
                            num12 = 0;
                            goto label_74;
                          }
                        }
                      }
                    }
                  }
                  nullable4 = ((cmpTariffCost) this.newObject).Scheme;
                  int? nullable10;
                  if (!nullable4.HasValue)
                  {
                    nullable2 = new int?();
                    nullable10 = nullable2;
                  }
                  else
                    nullable10 = new int?((int) nullable4.GetValueOrDefault());
                  nullable3 = nullable10;
                  int num16 = 141;
                  num12 = nullable3.GetValueOrDefault() == num16 ? (!nullable3.HasValue ? 1 : 0) : 1;
label_74:
                  if (num12 != 0)
                  {
                    int num8 = (int) MessageBox.Show("Для поля 'Схема параметров' имеющего значения 131,132,141,142,172 значение поля 'Схема' должно быть 141", "Внимание!", MessageBoxButtons.OK);
                    ((cmpTariffCost) this.newObject).Scheme = new short?((short) 141);
                  }
                  nullable4 = ((cmpTariffCost) this.newObject).Scheme;
                  int? nullable11;
                  if (!nullable4.HasValue)
                  {
                    nullable2 = new int?();
                    nullable11 = nullable2;
                  }
                  else
                    nullable11 = new int?((int) nullable4.GetValueOrDefault());
                  nullable3 = nullable11;
                  int num17 = 141;
                  int num18;
                  if ((nullable3.GetValueOrDefault() == num17 ? (nullable3.HasValue ? 1 : 0) : 0) != 0)
                  {
                    nullable3 = ((cmpTariffCost) this.newObject).BaseTariff_id;
                    int num8 = 4;
                    if ((nullable3.GetValueOrDefault() == num8 ? (!nullable3.HasValue ? 1 : 0) : 1) == 0)
                    {
                      nullable3 = ((cmpTariffCost) this.newObject).BaseTariffMSP_id;
                      int num13 = 1;
                      num18 = nullable3.GetValueOrDefault() == num13 ? (!nullable3.HasValue ? 1 : 0) : 1;
                    }
                    else
                      num18 = 1;
                  }
                  else
                    num18 = 0;
                  if (num18 != 0)
                  {
                    int num8 = (int) MessageBox.Show("Для схемы 141 поле 'На кого' должно иметь значение 'На единицу', поле 'Область применения' значение 'На человека'", "Внимание!", MessageBoxButtons.OK);
                    ((cmpTariffCost) this.newObject).BaseTariffMSP_id = new int?(1);
                    ((cmpTariffCost) this.newObject).BaseTariff_id = new int?(4);
                  }
                  using (ITransaction transaction = this.session.BeginTransaction())
                  {
                    try
                    {
                      ((cmpTariffCost) this.newObject).Uname = Options.Login;
                      ((cmpTariffCost) this.newObject).Dedit = DateTime.Now.Date;
                      this.session.Save(this.newObject);
                      this.session.Flush();
                      foreach (cmpTariffCost tariffCostPart in (IEnumerable) this.TariffCostPartList)
                      {
                        tariffCostPart.Uname = Options.Login;
                        tariffCostPart.Dedit = DateTime.Now.Date;
                        nullable4 = tariffCostPart.Scheme;
                        if (!nullable4.HasValue)
                          tariffCostPart.Scheme = new short?((short) 0);
                        tariffCostPart.SchemeParam = ((cmpTariffCost) this.newObject).SchemeParam;
                        tariffCostPart.Period = ((cmpTariffCost) this.newObject).Period;
                        tariffCostPart.Dbeg = ((cmpTariffCost) this.newObject).Dbeg;
                        tariffCostPart.Dend = ((cmpTariffCost) this.newObject).Dend;
                        tariffCostPart.BaseTariff_id = ((cmpTariffCost) this.newObject).BaseTariff_id;
                        tariffCostPart.IsVat = ((cmpTariffCost) this.newObject).IsVat;
                        tariffCostPart.BaseTariffMSP_id = ((cmpTariffCost) this.newObject).BaseTariffMSP_id;
                        tariffCostPart.UnitMeasuring_id = ((cmpTariffCost) this.newObject).UnitMeasuring_id;
                        this.session.Save((object) tariffCostPart);
                      }
                      this.session.Flush();
                      transaction.Commit();
                      if (Convert.ToInt32(KvrplHelper.BaseValue(32, this.session.Get<Company>((object) this.company_id))) == 1)
                      {
                        if (Convert.ToInt32(KvrplHelper.BaseValue(1, this.session.Get<Company>((object) this.company_id))) == 28 && MessageBox.Show("Внести основание?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                          FrmArgument frmArgument = new FrmArgument();
                          int num8 = (int) frmArgument.ShowDialog();
                          frmArgument.Dispose();
                          KvrplHelper.SaveTariffFromNoteBook((cmpTariffCost) this.newObject, (cmpTariffCost) null, this.session.Get<Company>((object) this.company_id), (short) 1, this.dgvBase.CurrentRow.Cells["NVariant"].FormattedValue.ToString() + " (" + this.dgvBase.CurrentRow.Cells["Variant"].FormattedValue.ToString() + ")", frmArgument.Argument(), this.isPast, this.closedPeriod.Value);
                        }
                        else
                          KvrplHelper.SaveTariffFromNoteBook((cmpTariffCost) this.newObject, (cmpTariffCost) null, this.session.Get<Company>((object) this.company_id), (short) 1, this.dgvBase.CurrentRow.Cells["NVariant"].FormattedValue.ToString() + " (" + this.dgvBase.CurrentRow.Cells["Variant"].FormattedValue.ToString() + ")", "", this.isPast, this.closedPeriod.Value);
                      }
                    }
                    catch (Exception ex)
                    {
                      transaction.Rollback();
                      int num8 = (int) MessageBox.Show("Не удалось сохранить запись! Проверьте правильность ввода данных.", "Внимание!", MessageBoxButtons.OK);
                      KvrplHelper.WriteLog(ex, (LsClient) null);
                      this.session.Clear();
                      this.session = Domain.CurrentSession;
                      return;
                    }
                  }
                  base.tsbApplay_Click(sender, e);
                }
              }
              else
              {
                int num19 = (int) MessageBox.Show("Сумма тарифа для компенсаций по составляющим не равна общей сумме тарифа для компенсаций!", "Внимание!", MessageBoxButtons.OK);
              }
            }
            else
            {
              int num20 = (int) MessageBox.Show("Сумма экономически обоснованного тарифа по составляющим не равна общей сумме экономически обоснованного тарифа!", "Внимание!", MessageBoxButtons.OK);
            }
          }
          else
          {
            int num21 = (int) MessageBox.Show("Сумма тарифа по составляющим не равна общей сумме тарифа!", "Внимание!", MessageBoxButtons.OK);
          }
        }
        else
        {
          double? nullable1;
          int num4;
          if (!(num1 == Convert.ToDecimal((object) ((cmpTariffCost) this.curObject).Cost)))
          {
            nullable1 = ((cmpTariffCost) this.curObject).Cost;
            num4 = nullable1.HasValue ? 0 : (num1 == Decimal.Zero ? 1 : 0);
          }
          else
            num4 = 1;
          if (num4 != 0)
          {
            int num5;
            if (!(num2 == Convert.ToDecimal((object) ((cmpTariffCost) this.curObject).Cost_eo)))
            {
              nullable1 = ((cmpTariffCost) this.curObject).Cost_eo;
              num5 = nullable1.HasValue ? 0 : (num2 == Decimal.Zero ? 1 : 0);
            }
            else
              num5 = 1;
            if (num5 != 0)
            {
              int num6;
              if (!(num3 == Convert.ToDecimal((object) ((cmpTariffCost) this.curObject).Cost_c)))
              {
                nullable1 = ((cmpTariffCost) this.curObject).Cost_c;
                num6 = nullable1.HasValue ? 0 : (num3 == Decimal.Zero ? 1 : 0);
              }
              else
                num6 = 1;
              if (num6 != 0)
              {
                int? nullable2 = ((cmpTariffCost) this.curObject).BaseTariff_id;
                int num7 = 1;
                int? nullable3;
                int num8;
                if ((nullable2.GetValueOrDefault() >= num7 ? (nullable2.HasValue ? 1 : 0) : 0) != 0)
                {
                  nullable2 = ((cmpTariffCost) this.curObject).BaseTariff_id;
                  int num9 = 3;
                  if ((nullable2.GetValueOrDefault() <= num9 ? (nullable2.HasValue ? 1 : 0) : 0) != 0)
                  {
                    nullable2 = ((cmpTariffCost) this.curObject).BaseTariffMSP_id;
                    nullable3 = ((cmpTariffCost) this.curObject).BaseTariff_id;
                    if ((nullable2.GetValueOrDefault() == nullable3.GetValueOrDefault() ? (nullable2.HasValue != nullable3.HasValue ? 1 : 0) : 1) != 0)
                    {
                      num8 = 1;
                      goto label_129;
                    }
                  }
                }
                nullable3 = ((cmpTariffCost) this.curObject).BaseTariffMSP_id;
                num8 = !nullable3.HasValue ? 1 : 0;
label_129:
                if (num8 != 0)
                {
                  int num9 = (int) MessageBox.Show("Поле 'Область применения' будет изменено аналогично полю 'На кого'. В случаях 'на человека','на площадь' и 'на семью' данные поля не могут различаться.", "Внимание!", MessageBoxButtons.OK);
                  ((cmpTariffCost) this.curObject).BaseTariffMSP_id = ((cmpTariffCost) this.curObject).BaseTariff_id;
                }
                short? nullable4 = ((cmpTariffCost) this.curObject).SchemeParam;
                int? nullable5;
                if (!nullable4.HasValue)
                {
                  nullable2 = new int?();
                  nullable5 = nullable2;
                }
                else
                  nullable5 = new int?((int) nullable4.GetValueOrDefault());
                nullable3 = nullable5;
                int num10 = 131;
                int num11;
                if ((nullable3.GetValueOrDefault() == num10 ? (nullable3.HasValue ? 1 : 0) : 0) == 0)
                {
                  nullable4 = ((cmpTariffCost) this.curObject).SchemeParam;
                  int? nullable6;
                  if (!nullable4.HasValue)
                  {
                    nullable2 = new int?();
                    nullable6 = nullable2;
                  }
                  else
                    nullable6 = new int?((int) nullable4.GetValueOrDefault());
                  nullable3 = nullable6;
                  int num9 = 132;
                  if ((nullable3.GetValueOrDefault() == num9 ? (nullable3.HasValue ? 1 : 0) : 0) == 0)
                  {
                    nullable4 = ((cmpTariffCost) this.curObject).SchemeParam;
                    int? nullable7;
                    if (!nullable4.HasValue)
                    {
                      nullable2 = new int?();
                      nullable7 = nullable2;
                    }
                    else
                      nullable7 = new int?((int) nullable4.GetValueOrDefault());
                    nullable3 = nullable7;
                    int num12 = 141;
                    if ((nullable3.GetValueOrDefault() == num12 ? (nullable3.HasValue ? 1 : 0) : 0) == 0)
                    {
                      nullable4 = ((cmpTariffCost) this.curObject).SchemeParam;
                      int? nullable8;
                      if (!nullable4.HasValue)
                      {
                        nullable2 = new int?();
                        nullable8 = nullable2;
                      }
                      else
                        nullable8 = new int?((int) nullable4.GetValueOrDefault());
                      nullable3 = nullable8;
                      int num13 = 142;
                      if ((nullable3.GetValueOrDefault() == num13 ? (nullable3.HasValue ? 1 : 0) : 0) == 0)
                      {
                        nullable4 = ((cmpTariffCost) this.curObject).SchemeParam;
                        int? nullable9;
                        if (!nullable4.HasValue)
                        {
                          nullable2 = new int?();
                          nullable9 = nullable2;
                        }
                        else
                          nullable9 = new int?((int) nullable4.GetValueOrDefault());
                        nullable3 = nullable9;
                        int num14 = 172;
                        if ((nullable3.GetValueOrDefault() == num14 ? (nullable3.HasValue ? 1 : 0) : 0) == 0)
                        {
                          num11 = 0;
                          goto label_157;
                        }
                      }
                    }
                  }
                }
                nullable4 = ((cmpTariffCost) this.curObject).Scheme;
                int? nullable10;
                if (!nullable4.HasValue)
                {
                  nullable2 = new int?();
                  nullable10 = nullable2;
                }
                else
                  nullable10 = new int?((int) nullable4.GetValueOrDefault());
                nullable3 = nullable10;
                int num15 = 141;
                num11 = nullable3.GetValueOrDefault() == num15 ? (!nullable3.HasValue ? 1 : 0) : 1;
label_157:
                if (num11 != 0)
                {
                  int num9 = (int) MessageBox.Show("Для поля 'Схема параметров' имеющего значения 131,132,141,142,172 значение поля 'Схема' должно быть 141", "Внимание!", MessageBoxButtons.OK);
                  ((cmpTariffCost) this.curObject).BaseTariffMSP_id = new int?(141);
                }
                nullable4 = ((cmpTariffCost) this.curObject).Scheme;
                int? nullable11;
                if (!nullable4.HasValue)
                {
                  nullable2 = new int?();
                  nullable11 = nullable2;
                }
                else
                  nullable11 = new int?((int) nullable4.GetValueOrDefault());
                nullable3 = nullable11;
                int num16 = 141;
                int num17;
                if ((nullable3.GetValueOrDefault() == num16 ? (nullable3.HasValue ? 1 : 0) : 0) != 0)
                {
                  nullable3 = ((cmpTariffCost) this.curObject).BaseTariff_id;
                  int num9 = 4;
                  if ((nullable3.GetValueOrDefault() == num9 ? (!nullable3.HasValue ? 1 : 0) : 1) == 0)
                  {
                    nullable3 = ((cmpTariffCost) this.curObject).BaseTariffMSP_id;
                    int num12 = 1;
                    num17 = nullable3.GetValueOrDefault() == num12 ? (!nullable3.HasValue ? 1 : 0) : 1;
                  }
                  else
                    num17 = 1;
                }
                else
                  num17 = 0;
                if (num17 != 0)
                {
                  int num9 = (int) MessageBox.Show("Для схемы 141 поле 'На кого' должно иметь значение 'На единицу', поле 'Область применения' значение 'На человека'", "Внимание!", MessageBoxButtons.OK);
                  ((cmpTariffCost) this.curObject).BaseTariffMSP_id = new int?(1);
                  ((cmpTariffCost) this.curObject).BaseTariff_id = new int?(4);
                }
                if ((((cmpTariffCost) this.curObject).Dbeg <= DateTime.Now.AddYears(-3) || ((cmpTariffCost) this.curObject).Dbeg >= DateTime.Now.AddYears(3)) && MessageBox.Show("Дата начала отличается от текущей более, чем на 3 года. Продолжить", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                  return;
                cmpTariffCost oldTariff = new cmpTariffCost();
                foreach (cmpTariffCost cmpTariffCost in (IEnumerable<cmpTariffCost>) this.listTariff)
                {
                  if (cmpTariffCost.OldHashCode == ((cmpTariffCost) this.curObject).OldHashCode)
                    oldTariff = cmpTariffCost;
                }
                string str;
                try
                {
                  this.session.CreateSQLQuery("update DBA.cmpTariff cmp set Tariff_id=:tariff_id1,Dbeg=:dbeg,Scheme=:scheme,Scheme_Param=:schemeparam,Dend=:dend,Cost=:cost,Cost_eo=:c_eo,Cost_c=:c_c,UnitMeasuring_id=:u_id,BaseTariff_id=:b_id,BaseTariffMSP_id=:bm_id,IsVat=:vat,Uname=:uname,Dedit=:dedit where cmp.Company_id=:company_id and cmp.Period_id=:period and cmp.Dbeg = :dbeg2 and cmp.Service_id = :service and cmp.Tariff_id=:tariff_id2").SetDateTime("dbeg", ((cmpTariffCost) this.curObject).Dbeg).SetParameter<short?>("scheme", ((cmpTariffCost) this.curObject).Scheme).SetParameter<short?>("schemeparam", ((cmpTariffCost) this.curObject).SchemeParam).SetParameter<DateTime>("dend", ((cmpTariffCost) this.curObject).Dend).SetParameter<double?>("cost", ((cmpTariffCost) this.curObject).Cost).SetParameter<double?>("c_eo", ((cmpTariffCost) this.curObject).Cost_eo).SetParameter<double?>("c_c", ((cmpTariffCost) this.curObject).Cost_c).SetParameter<int?>("u_id", ((cmpTariffCost) this.curObject).UnitMeasuring_id).SetParameter<int?>("b_id", ((cmpTariffCost) this.curObject).BaseTariff_id).SetParameter<int?>("bm_id", ((cmpTariffCost) this.curObject).BaseTariffMSP_id).SetParameter<short>("vat", ((cmpTariffCost) this.curObject).IsVat.YesNoId).SetDateTime("dbeg2", this.dt.Value).SetInt32("company_id", ((cmpTariffCost) this.curObject).Company_id).SetInt32("service", (int) ((cmpTariffCost) this.curObject).Service.ServiceId).SetInt32("period", ((cmpTariffCost) this.curObject).Period.PeriodId).SetInt32("tariff_id1", ((cmpTariffCost) this.curObject).Tariff_id).SetInt32("tariff_id2", oldTariff.Tariff_id).SetString("uname", Options.Login).SetDateTime("dedit", DateTime.Now.Date).ExecuteUpdate();
                  foreach (cmpTariffCost tariffCostPart in (IEnumerable) this.TariffCostPartList)
                    this.session.CreateSQLQuery("update DBA.cmpTariff cmp set Tariff_id=:tariff_id1,Dbeg=:dbeg,Scheme=:scheme,Scheme_Param=:schemeparam,Dend=:dend,Cost=:cost,Cost_eo=:c_eo,Cost_c=:c_c,UnitMeasuring_id=:u_id,BaseTariff_id=:b_id,BaseTariffMSP_id=:bm_id,IsVat=:vat,Uname=:uname,Dedit=:dedit where cmp.Company_id=:company_id and cmp.Period_id=:period and cmp.Dbeg = :dbeg2 and cmp.Service_id = :service and cmp.Tariff_id=:tariff_id2").SetDateTime("dbeg", ((cmpTariffCost) this.curObject).Dbeg).SetParameter<short?>("scheme", tariffCostPart.Scheme).SetParameter<short?>("schemeparam", ((cmpTariffCost) this.curObject).SchemeParam).SetParameter<DateTime>("dend", ((cmpTariffCost) this.curObject).Dend).SetParameter<double?>("cost", tariffCostPart.Cost).SetParameter<double?>("c_eo", tariffCostPart.Cost_eo).SetParameter<double?>("c_c", tariffCostPart.Cost_c).SetParameter<int?>("u_id", ((cmpTariffCost) this.curObject).UnitMeasuring_id).SetParameter<int?>("b_id", ((cmpTariffCost) this.curObject).BaseTariff_id).SetParameter<int?>("bm_id", ((cmpTariffCost) this.curObject).BaseTariffMSP_id).SetParameter<short>("vat", ((cmpTariffCost) this.curObject).IsVat.YesNoId).SetDateTime("dbeg2", this.dt.Value).SetInt32("company_id", tariffCostPart.Company_id).SetInt32("service", (int) tariffCostPart.Service.ServiceId).SetInt32("period", tariffCostPart.Period.PeriodId).SetInt32("tariff_id1", ((cmpTariffCost) this.curObject).Tariff_id).SetInt32("tariff_id2", oldTariff.Tariff_id).SetString("uname", Options.Login).SetDateTime("dedit", DateTime.Now.Date).ExecuteUpdate();
                  foreach (cmpTariffCost cmpTariffCost in (IEnumerable<cmpTariffCost>) this.listTariff)
                  {
                    if (cmpTariffCost.OldHashCode == ((cmpTariffCost) this.objectsList[this.dgvBase.CurrentRow.Index]).OldHashCode)
                    {
                      oldTariff = cmpTariffCost;
                      break;
                    }
                  }
                  str = this.dgvBase.CurrentRow.Cells["NVariant"].FormattedValue.ToString() + " (" + this.dgvBase.CurrentRow.Cells["Variant"].FormattedValue.ToString() + ")";
                }
                catch (Exception ex)
                {
                  int num9 = (int) MessageBox.Show("Не удалось внести изменения! Проверьте правильность ввода данных.", "Внимание!", MessageBoxButtons.OK);
                  KvrplHelper.WriteLog(ex, (LsClient) null);
                  return;
                }
                this.CurIndex = -1;
                this.GetList();
                if (this.dgvBase.Rows.Count > 0 && this.dgvBase.CurrentRow != null)
                {
                  this.dt = new DateTime?(((cmpTariffCost) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg);
                  this.CurIndex = this.dgvBase.CurrentRow.Index;
                  this.curObject = this.objectsList[this.dgvBase.CurrentRow.Index];
                }
                this.GetTariffCostPartList();
                if (Convert.ToInt32(KvrplHelper.BaseValue(32, this.session.Get<Company>((object) this.company_id))) == 1)
                  KvrplHelper.SaveTariffFromNoteBook((cmpTariffCost) this.curObject, oldTariff, this.session.Get<Company>((object) this.company_id), (short) 2, str, "", this.isPast, this.closedPeriod.Value);
                this.tsbAdd.Enabled = true;
                this.tsbApplay.Enabled = false;
                this.tsbCancel.Enabled = false;
                this.tsbDelete.Enabled = true;
              }
              else
              {
                int num18 = (int) MessageBox.Show("Сумма тарифа для компенсаций по составляющим не равна общей сумме тарифа для компенсаций!", "Внимание!", MessageBoxButtons.OK);
              }
            }
            else
            {
              int num19 = (int) MessageBox.Show("Сумма экономически обоснованного тарифа по составляющим не равна общей сумме экономически обоснованного тарифа!", "Внимание!", MessageBoxButtons.OK);
            }
          }
          else
          {
            int num20 = (int) MessageBox.Show("Сумма тарифа по составляющим не равна общей сумме тарифа!", "Внимание!", MessageBoxButtons.OK);
          }
        }
        if (this.dgvBase.CurrentRow != null && this.dgvBase.CurrentRow.Index > 0)
        {
          if (this.dgvBase.CurrentRow.Index < this.objectsList.Count)
            this.session.Refresh(this.objectsList[this.dgvBase.CurrentRow.Index]);
          this.session.Refresh(this.objectsList[this.dgvBase.CurrentRow.Index - 1]);
        }
        else if (this.objectsList.Count > 0)
          this.session.Refresh(this.objectsList[0]);
        if (this.dgvBase.CurrentRow != null)
          this.CurIndex = this.dgvBase.CurrentRow.Index;
        this.dgvBase.Refresh();
        if (this.curObject != null)
          this.dt = new DateTime?(((cmpTariffCost) this.curObject).Dbeg);
        // ISSUE: reference to a compiler-generated field
        if (this.ApplayClick != null)
        {
          // ISSUE: reference to a compiler-generated field
          this.ApplayClick((object) this, EventArgs.Empty);
        }
        this.isCopy = false;
      }
    }

    protected override void tsbCancel_Click(object sender, EventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.CancelClick != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.CancelClick((object) this, EventArgs.Empty);
      }
      if (this.curObject != null && !this.dt.Equals((object) ((cmpTariffCost) this.curObject).Dbeg))
      {
        ((cmpTariffCost) this.curObject).Dbeg = this.dt.Value;
        foreach (cmpTariffCost tariffCostPart in (IEnumerable) this.TariffCostPartList)
          tariffCostPart.Dbeg = this.dt.Value;
      }
      base.tsbCancel_Click(sender, e);
      if (this.dgvBase.CurrentRow != null)
      {
        this.dt = new DateTime?(((cmpTariffCost) this.curObject).Dbeg);
        this.CurIndex = this.dgvBase.CurrentRow.Index;
      }
      try
      {
        foreach (cmpTariffCost tariffCostPart in (IEnumerable) this.TariffCostPartList)
          this.session.Refresh((object) tariffCostPart);
      }
      catch
      {
      }
      this.isCopy = false;
      this.GetTariffCostPartList();
    }

    protected override void tsbDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(33, 2, this.session.Get<Company>((object) this.company_id), true) || MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      if (this.objectsList.Count > 0)
      {
        int num1;
        if (this.closedPeriod.HasValue)
        {
          if (this.closedPeriod.Value.AddMonths(1) <= ((cmpTariffCost) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg || this.isPast)
          {
            if (this.isPast)
            {
              if (((cmpTariffCost) this.objectsList[this.dgvBase.CurrentRow.Index]).Period.PeriodName.HasValue)
              {
                DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
                DateTime? periodName = ((cmpTariffCost) this.objectsList[this.dgvBase.CurrentRow.Index]).Period.PeriodName;
                num1 = periodName.HasValue ? (dateTime <= periodName.GetValueOrDefault() ? 1 : 0) : 0;
              }
              else
                num1 = 0;
            }
            else
              num1 = 1;
          }
          else
            num1 = 0;
        }
        else
          num1 = 1;
        if (num1 != 0)
        {
          if (this.curObject != null)
          {
            try
            {
              this.session.Clear();
              this.session = Domain.CurrentSession;
              Period period = this.session.Get<Period>((object) 0);
              foreach (cmpTariffCost tariffCostPart in (IEnumerable) this.TariffCostPartList)
              {
                if (tariffCostPart.Period != null && tariffCostPart.Period.PeriodId == 0)
                  tariffCostPart.Period = period;
                this.session.Delete((object) tariffCostPart);
                this.session.Flush();
              }
              if (((cmpTariffCost) this.curObject).Period != null && ((cmpTariffCost) this.curObject).Period.PeriodId == 0)
                ((cmpTariffCost) this.curObject).Period = period;
              if (Convert.ToInt32(KvrplHelper.BaseValue(32, this.session.Get<Company>((object) this.company_id))) == 1)
                KvrplHelper.SaveTariffFromNoteBook((cmpTariffCost) this.curObject, (cmpTariffCost) null, this.session.Get<Company>((object) this.company_id), (short) 3, this.dgvBase.CurrentRow.Cells["NVariant"].FormattedValue.ToString() + " (" + this.dgvBase.CurrentRow.Cells["Variant"].FormattedValue.ToString() + ")", "", this.isPast, this.closedPeriod.Value);
              base.tsbDelete_Click(sender, e);
              foreach (cmpTariffCost objects in (IEnumerable) this.objectsList)
                this.session.Refresh((object) objects);
              this.dgvBase.Refresh();
            }
            catch
            {
              int num2 = (int) MessageBox.Show("Не удалось удалить запись!", "Внимание!", MessageBoxButtons.OK);
              this.session.Clear();
              this.session = Domain.CurrentSession;
            }
          }
        }
        else
        {
          int num3 = (int) MessageBox.Show("Попытка удалить запись в закрытом периоде!", "Внимание!", MessageBoxButtons.OK);
        }
      }
      if (this.curObject != null)
      {
        this.dt = new DateTime?(((cmpTariffCost) this.curObject).Dbeg);
        this.CurIndex = this.dgvBase.CurrentRow.Index;
      }
      this.GetTariffCostPartList();
    }

    protected override void dgvBase_SelectionChanged(object sender, EventArgs e)
    {
      if (this.dgvBase.CurrentRow == null || this.dgvBase.CurrentRow.Index >= this.objectsList.Count)
        return;
      if (this.newObject == null && (this.CurIndex >= 0 && this.objectsList.Count > this.CurIndex && !this.dt.Equals((object) ((cmpTariffCost) this.objectsList[this.CurIndex]).Dbeg) && this.CurIndex != this.dgvBase.CurrentRow.Index))
      {
        try
        {
          this.CancelEnabled();
          ((cmpTariffCost) this.objectsList[this.CurIndex]).Dbeg = this.dt.Value;
          this.session.Refresh(this.objectsList[this.CurIndex]);
          this.curObject = this.objectsList[this.dgvBase.CurrentRow.Index];
          this.GetList();
          this.GetTariffCostPartList();
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      else if (this.tsbApplay.Enabled && this.newObject != null && (this.CurIndex >= 0 && this.CurIndex != this.dgvBase.CurrentRow.Index) && this.dgvBase.CurrentRow.Index < this.dgvBase.Rows.Count - 1)
      {
        if (MessageBox.Show("Изменения не сохранены! Вернуться к предыдущей записи и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
          this.tsbApplay_Click((object) null, (EventArgs) null);
          return;
        }
        try
        {
          this.objectsList.Remove(this.newObject);
          if (this.objectsList.Count > 0)
          {
            this.curObject = this.objectsList[this.objectsList.Count - 1];
            this.dt = new DateTime?(((cmpTariffCost) this.curObject).Dbeg);
          }
          else
            this.curObject = (object) null;
          this.isCopy = false;
          this.GetList();
          this.GetTariffCostPartList();
          this.CancelEnabled();
          this.newObject = (object) null;
          this.CurIndex = this.dgvBase.CurrentRow.Index;
        }
        catch (Exception ex)
        {
          KvrplHelper.WriteLog(ex, (LsClient) null);
        }
      }
      else if (this.newObject == null && this.tsbApplay.Enabled && (this.CurIndex >= 0 && this.CurIndex != this.dgvBase.CurrentRow.Index) && this.dgvBase.CurrentRow.Index <= this.dgvBase.Rows.Count - 1 && this.CurIndex < this.objectsList.Count)
      {
        this.CancelEnabled();
        ((cmpTariffCost) this.objectsList[this.CurIndex]).Dbeg = this.dt.Value;
        this.session.Refresh(this.objectsList[this.CurIndex]);
        foreach (cmpTariffCost tariffCostPart in (IEnumerable) this.TariffCostPartList)
          this.session.Refresh((object) tariffCostPart);
        this.curObject = this.objectsList[this.dgvBase.CurrentRow.Index];
        this.GetList();
        this.GetTariffCostPartList();
      }
      if (this.CurIndex != this.dgvBase.CurrentRow.Index && this.objectsList.Count > 0 && this.objectsList.Count > this.CurIndex)
      {
        this.dt = new DateTime?(((cmpTariffCost) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg);
        this.CurIndex = this.dgvBase.CurrentRow.Index;
        this.curObject = this.objectsList[this.dgvBase.CurrentRow.Index];
        this.GetTariffCostPartList();
      }
    }

    private void dgvBase_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex < 0 || this.newObject == null || !(this.dgvBase.Columns[e.ColumnIndex].Name == "Variant") && !(this.dgvBase.Columns[e.ColumnIndex].Name == "NVariant"))
        return;
      if (this.TariffCostPartList != null && !this.isCopy)
        this.TariffCostPartList.Clear();
      DateTime dateTime1;
      DateTime dateTime2;
      if (this.closedPeriod.HasValue)
      {
        if (this.isPast)
        {
          dateTime1 = this.closedPeriod.Value;
        }
        else
        {
          dateTime2 = this.closedPeriod.Value;
          dateTime1 = dateTime2.AddMonths(1);
        }
      }
      else
        dateTime1 = DateTime.Now;
      Period period;
      if (!this.isPast)
        period = this.session.Get<Period>((object) Convert.ToInt32(this.session.CreateQuery("select p.PeriodId from Period p where PeriodName is null").UniqueResult()));
      else if (this.closedPeriod.HasValue)
      {
        IQuery query = this.session.CreateQuery("from Period p where p.PeriodName=:dt");
        string name = "dt";
        dateTime2 = this.closedPeriod.Value;
        DateTime val = dateTime2.AddMonths(1);
        period = (Period) query.SetDateTime(name, val).UniqueResult();
      }
      else
      {
        int num = (int) MessageBox.Show("Отсутствует закрытый период!", "Внимание!", MessageBoxButtons.OK);
        return;
      }
      if (!this.isCopy)
      {
        foreach (Service service in (IEnumerable) this.curService.ChildService)
        {
          cmpTariffCost cmpTariffCost1 = new cmpTariffCost();
          cmpTariffCost1.Company_id = (int) this.Company_id;
          cmpTariffCost1.Dbeg = dateTime1;
          cmpTariffCost1.Period = period;
          DateTime? periodName;
          int num;
          if (this.isPast)
          {
            periodName = period.PeriodName;
            num = periodName.HasValue ? 1 : 0;
          }
          else
            num = 0;
          if (num != 0)
          {
            cmpTariffCost cmpTariffCost2 = cmpTariffCost1;
            periodName = period.PeriodName;
            dateTime2 = periodName.Value;
            dateTime2 = dateTime2.AddMonths(1);
            DateTime dateTime3 = dateTime2.AddDays(-1.0);
            cmpTariffCost2.Dend = dateTime3;
          }
          else
            cmpTariffCost1.Dend = Convert.ToDateTime("31.12.2999");
          cmpTariffCost1.Service = service;
          cmpTariffCost1.Tariff_id = ((cmpTariffCost) this.newObject).Tariff_id;
          if (this.curService.ChildService.Count == 0)
          {
            cmpTariffCost1.Cost = ((cmpTariffCost) this.newObject).Cost;
            cmpTariffCost1.Cost_eo = ((cmpTariffCost) this.newObject).Cost_eo;
            cmpTariffCost1.Cost_c = ((cmpTariffCost) this.newObject).Cost_c;
          }
          else if (this.newObject != null)
          {
            cmpTariffCost1.Cost = cmpTariffCost1.Cost_eo = cmpTariffCost1.Cost_c = new double?(0.0);
            cmpTariffCost1.Scheme = cmpTariffCost1.SchemeParam = new short?((short) 0);
          }
          this.TariffCostPartList.Add((object) cmpTariffCost1);
        }
      }
      else
      {
        foreach (cmpTariffCost tariffCostPart in (IEnumerable) this.TariffCostPartList)
          tariffCostPart.Tariff_id = ((cmpTariffCost) this.newObject).Tariff_id;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.TariffCostPartListChanged != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.TariffCostPartListChanged((object) this, EventArgs.Empty);
      }
    }

    protected override void tsmCopy_Click(object sender, EventArgs e)
    {
      this.isCopy = true;
      this.isEdit = true;
      IList tariffCostPartList = this.TariffCostPartList;
      this.newObject = (object) new cmpTariffCost();
      this.CurIndex = -1;
      ((cmpTariffCost) this.newObject).Copy((cmpTariffCost) this.curObject);
      DateTime dateTime1;
      if (!this.isPast && this.closedPeriod.HasValue && ((cmpTariffCost) this.newObject).Dbeg < this.closedPeriod.Value.AddMonths(1))
        ((cmpTariffCost) this.newObject).Dbeg = this.closedPeriod.Value.AddMonths(1);
      else if (this.isPast)
      {
        ((cmpTariffCost) this.newObject).Dbeg = this.closedPeriod.Value;
      }
      else
      {
        cmpTariffCost newObject = (cmpTariffCost) this.newObject;
        dateTime1 = ((cmpTariffCost) this.newObject).Dbeg.AddMonths(1);
        DateTime dateTime2 = dateTime1.AddDays((double) (-((cmpTariffCost) this.newObject).Dbeg.Day + 1));
        newObject.Dbeg = dateTime2;
      }
      if (this.closedPeriod.HasValue && this.isPast)
      {
        cmpTariffCost newObject = (cmpTariffCost) this.newObject;
        dateTime1 = this.closedPeriod.Value;
        dateTime1 = dateTime1.AddMonths(1);
        DateTime dateTime2 = dateTime1.AddDays(-1.0);
        newObject.Dend = dateTime2;
      }
      base.tsbAdd_Click(sender, e);
      int int32 = Convert.ToInt32(this.session.CreateQuery(string.Format("select p.Period_id from ClosedPeriod p where p.Complex_id = {0} and p.Company_id = {1}", (object) Options.Complex.ComplexId, (object) this.Company_id)).UniqueResult());
      Period period1 = this.session.Get<Period>((object) int32);
      IQuery query = this.session.CreateQuery("from Period p where p.PeriodName=:dt");
      string name = "dt";
      DateTime? periodName = period1.PeriodName;
      dateTime1 = periodName.Value;
      DateTime val = dateTime1.AddMonths(1);
      Period period2 = (Period) query.SetDateTime(name, val).UniqueResult();
      periodName = period2.PeriodName;
      DateTime dateTime3;
      if (periodName.HasValue)
      {
        periodName = period2.PeriodName;
        if (periodName.HasValue)
        {
          if (this.isPast)
          {
            periodName = this.session.Get<Period>((object) int32).PeriodName;
            dateTime3 = periodName.Value;
          }
          else
            dateTime3 = ((cmpTariffCost) this.curObject).Dbeg;
        }
        else
          dateTime3 = DateTime.Now;
      }
      else
        dateTime3 = DateTime.Now;
      if (this.TariffCostPartList != null)
        this.TariffCostPartList.Clear();
      foreach (Service service in (IEnumerable) this.curService.ChildService)
      {
        cmpTariffCost cmpTariffCost1 = new cmpTariffCost();
        cmpTariffCost1.Company_id = (int) this.Company_id;
        cmpTariffCost1.Dbeg = dateTime3;
        cmpTariffCost1.Period = period2;
        int num;
        if (this.isPast)
        {
          periodName = period2.PeriodName;
          num = periodName.HasValue ? 1 : 0;
        }
        else
          num = 0;
        if (num != 0)
        {
          cmpTariffCost cmpTariffCost2 = cmpTariffCost1;
          periodName = period2.PeriodName;
          dateTime1 = periodName.Value;
          dateTime1 = dateTime1.AddMonths(1);
          DateTime dateTime2 = dateTime1.AddDays(-1.0);
          cmpTariffCost2.Dend = dateTime2;
        }
        else
          cmpTariffCost1.Dend = Convert.ToDateTime("31.12.2999");
        cmpTariffCost1.Service = service;
        cmpTariffCost1.Tariff_id = ((cmpTariffCost) this.newObject).Tariff_id;
        if (this.newObject != null)
          cmpTariffCost1.Cost = cmpTariffCost1.Cost_eo = cmpTariffCost1.Cost_c = new double?(0.0);
        foreach (cmpTariffCost cmpTariffCost2 in (IEnumerable) tariffCostPartList)
        {
          if ((int) cmpTariffCost1.Service.ServiceId == (int) cmpTariffCost2.Service.ServiceId)
          {
            cmpTariffCost1.Cost = cmpTariffCost2.Cost;
            cmpTariffCost1.Cost_eo = cmpTariffCost2.Cost_eo;
            cmpTariffCost1.Cost_c = cmpTariffCost2.Cost_c;
            cmpTariffCost1.Scheme = cmpTariffCost2.Scheme;
          }
        }
        this.TariffCostPartList.Add((object) cmpTariffCost1);
      }
      this.dt = new DateTime?(((cmpTariffCost) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg);
      // ISSUE: reference to a compiler-generated field
      if (this.TariffCostPartListChanged != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.TariffCostPartListChanged((object) this, EventArgs.Empty);
      }
      base.tsmCopy_Click(sender, e);
    }

    protected override void tsmCopyInPast_Click(object sender, EventArgs e)
    {
      this.isCopy = true;
      this.CurIndex = -1;
      IList tariffCostPartList = this.TariffCostPartList;
      if (!this.isPast)
        this.IsCopy = true;
      this.newObject = (object) new cmpTariffCost();
      ((cmpTariffCost) this.newObject).Copy((cmpTariffCost) this.curObject);
      if (this.closedPeriod.HasValue)
      {
        Period period = (Period) this.session.CreateQuery("from Period p where p.PeriodName=:dt").SetDateTime("dt", this.closedPeriod.Value.AddMonths(1)).UniqueResult();
        ((cmpTariffCost) this.newObject).Period = period;
        if (!this.isPast)
          this.IsPast = !this.isPast;
        if (this.newObject == null)
          return;
        if (this.isPast && this.closedPeriod.HasValue)
          ((cmpTariffCost) this.newObject).Dbeg = this.closedPeriod.Value;
        if (this.closedPeriod.HasValue && this.isPast)
          ((cmpTariffCost) this.newObject).Dend = this.closedPeriod.Value.AddMonths(1).AddDays(-1.0);
        else
          ((cmpTariffCost) this.newObject).Dend = Convert.ToDateTime("31.12.2999");
        base.tsbAdd_Click(sender, e);
        base.tsmCopyInPast_Click(sender, e);
        this.dt = new DateTime?(((cmpTariffCost) this.objectsList[this.dgvBase.CurrentRow.Index]).Dbeg);
        List<cmpTariffCost> cmpTariffCostList = new List<cmpTariffCost>();
        foreach (cmpTariffCost cmpTariffCost1 in (IEnumerable) tariffCostPartList)
        {
          cmpTariffCost cmpTariffCost2 = new cmpTariffCost();
          cmpTariffCost2.Copy(cmpTariffCost1);
          cmpTariffCost2.Period = period;
          cmpTariffCost2.Dbeg = ((cmpTariffCost) this.newObject).Dbeg;
          cmpTariffCostList.Add(cmpTariffCost2);
        }
        this.TariffCostPartList.Clear();
        foreach (object obj in cmpTariffCostList)
          this.TariffCostPartList.Add(obj);
        // ISSUE: reference to a compiler-generated field
        if (this.TariffCostPartListChanged != null)
        {
          // ISSUE: reference to a compiler-generated field
          this.TariffCostPartListChanged((object) this, EventArgs.Empty);
        }
      }
      else
      {
        int num = (int) MessageBox.Show("Отсутствует закрытый период!", "Внимание!", MessageBoxButtons.OK);
      }
    }

    protected override void dgvBase_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    private void dgvBase_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      short id = 0;
      DateTime? periodName;
      if (e.ColumnIndex > 0 && e.RowIndex >= 0 && this.dgvBase.Columns[e.ColumnIndex].Name == "Scheme")
      {
        if (((cmpTariffCost) this.objectsList[e.RowIndex]).Scheme.HasValue)
          id = ((cmpTariffCost) this.objectsList[e.RowIndex]).Scheme.Value;
        FrmScheme frmScheme = new FrmScheme((short) 1, id);
        if (frmScheme.ShowDialog() == DialogResult.OK)
        {
          int num1;
          if (e.RowIndex < 0 || this.objectsList.Count <= 0 || this.closedPeriod.HasValue)
          {
            if (this.closedPeriod.Value.AddMonths(1) <= ((cmpTariffCost) this.objectsList[e.RowIndex]).Dbeg || this.isPast)
            {
              if (this.isPast)
              {
                if (((cmpTariffCost) this.objectsList[e.RowIndex]).Period.PeriodName.HasValue)
                {
                  DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
                  periodName = ((cmpTariffCost) this.objectsList[e.RowIndex]).Period.PeriodName;
                  if ((periodName.HasValue ? (dateTime <= periodName.GetValueOrDefault() ? 1 : 0) : 0) != 0)
                    goto label_10;
                }
              }
              else
                goto label_10;
            }
            num1 = this.dgvBase.Columns[e.ColumnIndex].Name == "Dend" ? 1 : 0;
            goto label_11;
          }
label_10:
          num1 = 1;
label_11:
          if (num1 != 0)
          {
            id = frmScheme.CurrentId();
          }
          else
          {
            int num2 = (int) MessageBox.Show("Попытка внести изменения в закрытом периоде!", "Внимание!", MessageBoxButtons.OK);
            this.isEdit = false;
            return;
          }
        }
        ((cmpTariffCost) this.objectsList[e.RowIndex]).Scheme = new short?(id);
        frmScheme.Dispose();
        this.tsbAdd.Enabled = false;
        this.tsbApplay.Enabled = true;
        this.tsbCancel.Enabled = true;
        this.tsbDelete.Enabled = false;
      }
      if (e.ColumnIndex <= 0 || e.RowIndex < 0 || !(this.dgvBase.Columns[e.ColumnIndex].Name == "SchemeParam"))
        return;
      if (((cmpTariffCost) this.objectsList[e.RowIndex]).SchemeParam.HasValue)
        id = ((cmpTariffCost) this.objectsList[e.RowIndex]).SchemeParam.Value;
      FrmScheme frmScheme1 = new FrmScheme((short) 2, id);
      if (frmScheme1.ShowDialog() == DialogResult.OK)
      {
        int num1;
        if (e.RowIndex < 0 || this.objectsList.Count <= 0 || this.closedPeriod.HasValue)
        {
          if (this.closedPeriod.Value.AddMonths(1) <= ((cmpTariffCost) this.objectsList[e.RowIndex]).Dbeg || this.isPast)
          {
            if (this.isPast)
            {
              periodName = ((cmpTariffCost) this.objectsList[e.RowIndex]).Period.PeriodName;
              if (periodName.HasValue)
              {
                DateTime dateTime = this.closedPeriod.Value.AddMonths(1);
                periodName = ((cmpTariffCost) this.objectsList[e.RowIndex]).Period.PeriodName;
                if ((periodName.HasValue ? (dateTime <= periodName.GetValueOrDefault() ? 1 : 0) : 0) != 0)
                  goto label_25;
              }
            }
            else
              goto label_25;
          }
          num1 = this.dgvBase.Columns[e.ColumnIndex].Name == "Dend" ? 1 : 0;
          goto label_26;
        }
label_25:
        num1 = 1;
label_26:
        if (num1 != 0)
        {
          id = frmScheme1.CurrentId();
        }
        else
        {
          int num2 = (int) MessageBox.Show("Попытка внести изменения в закрытом периоде!", "Внимание!", MessageBoxButtons.OK);
          this.isEdit = false;
          return;
        }
      }
      ((cmpTariffCost) this.objectsList[e.RowIndex]).SchemeParam = new short?(id);
      frmScheme1.Dispose();
      this.tsbAdd.Enabled = false;
      this.tsbApplay.Enabled = true;
      this.tsbCancel.Enabled = true;
      this.tsbDelete.Enabled = false;
    }

    protected override void dgvBase_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      base.dgvBase_ColumnWidthChanged(sender, e);
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
    }
  }
}
