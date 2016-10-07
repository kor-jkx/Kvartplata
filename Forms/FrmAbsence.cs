// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmAbsence
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
  public class FrmAbsence : Form
  {
    private IList absenceList = (IList) new ArrayList();
    private List<AbsenceCoeff> absenceCoeffList = new List<AbsenceCoeff>();
    private IList serviceList = (IList) new ArrayList();
    private List<AbsenceCoeff> newAbsenceCoeffList = new List<AbsenceCoeff>();
    private int curIndex = -1;
    private int curAbsenceIdx = -1;
    private FormStateSaver fss = new FormStateSaver(FrmAbsence.ic);
    private bool isEdit = false;
    private bool _readOnly = false;
    private Dictionary<short, Absence> _updId = new Dictionary<short, Absence>();
    private IContainer components = (IContainer) null;
    private ISession session;
    private Absence curAbsence;
    private Absence newAbsence;
    private AbsenceCoeff curAbsenceCoeff;
    private AbsenceCoeff newAbsenceCoeff;
    private int oldServiceId;
    private static IContainer ic;
    private Company _company;
    private DataSet dsMain;
    private DataTable dtServ;
    private DataColumn dataColumn1;
    private DataColumn dataColumn2;
    private Panel pnBtn;
    private SplitContainer splitContainer1;
    private GroupBox groupBox2;
    private DataGridView dgvdcAbsence;
    private ToolStrip toolStrip1;
    private ToolStripButton tsbAddAbsence;
    private ToolStripButton tsbApplayAbsence;
    private ToolStripButton tsbCancelAbsence;
    private ToolStripButton tsbDeleteAbsence;
    private GroupBox groupBox1;
    private DataGridView dgvdcAbsenceCoeff;
    private ToolStrip toolStrip2;
    private ToolStripButton tsbAddAbsenceCoeff;
    private ToolStripButton tsbApplayAbsenceCoeff;
    private ToolStripButton tsbCancelAbsenceCoeff;
    private ToolStripButton tsbDeleteAbsenceCoeff;
    private ToolStripButton tsbExit;
    private Button bntExit;
    public HelpProvider hp;
    private DataGridViewTextBoxColumn ID;
    private DataGridViewTextBoxColumn nameColumn;
    private DataGridViewTextBoxColumn UName;
    private DataGridViewTextBoxColumn DEdit;
    private DataGridViewComboBoxColumn Service_id;
    private DataGridViewTextBoxColumn Coeff;
    private DataGridViewTextBoxColumn UNameCoeff;
    private DataGridViewTextBoxColumn DEditCoeff;

    private Absence CurAbsence
    {
      get
      {
        return this.curAbsence;
      }
      set
      {
        if (this.curAbsence != value && this.tsbApplayAbsenceCoeff.Enabled)
        {
          if (MessageBox.Show("Изменения не сохранены! Вернуться к предыдущей причине и сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
          {
            this.tsbApplayAbsenceCoeff_Click((object) null, (EventArgs) null);
            return;
          }
          this.dgvdcAbsenceCoeff.EndEdit();
          if (this.newAbsenceCoeff != null)
          {
            if (this.newAbsenceCoeff != null)
            {
              this.absenceCoeffList.Remove(this.newAbsenceCoeff);
              this.newAbsenceCoeff = (AbsenceCoeff) null;
            }
          }
          else if (this.CurAbsence != null)
            this.session.Refresh((object) this.CurAbsence);
          this.tsbAddAbsenceCoeff.Enabled = true;
          this.tsbApplayAbsenceCoeff.Enabled = false;
          this.tsbCancelAbsenceCoeff.Enabled = false;
          this.tsbDeleteAbsenceCoeff.Enabled = true;
          this.toolStrip1.Enabled = true;
        }
        this.curAbsence = value;
        this.absenceCoeffList.Clear();
        if (value.AbsenceCoeff.Count > 0)
        {
          foreach (AbsenceCoeff absenceCoeff in (IEnumerable<AbsenceCoeff>) this.session.CreateQuery(string.Format("select ac from AbsenceCoeff ac,Service s where ac.Service_id=s.ServiceId and Absence_id={0} order by " + Options.SortService, (object) this.curAbsence.Absence_id)).List<AbsenceCoeff>())
            this.absenceCoeffList.Add(absenceCoeff);
        }
        if (this.absenceCoeffList.Count > 0)
        {
          this.curIndex = 0;
          this.curAbsenceCoeff = this.absenceCoeffList[0];
        }
        else
        {
          this.curIndex = -1;
          this.curAbsenceCoeff = (AbsenceCoeff) null;
        }
        this.dgvdcAbsenceCoeff.RowCount = this.absenceCoeffList.Count;
        this.dgvdcAbsenceCoeff.Refresh();
      }
    }

    public FrmAbsence(Company company)
    {
      this.InitializeComponent();
      this._company = company;
      this.CheckAccess();
      this.fss.ParentForm = (Form) this;
      this.session = Domain.CurrentSession;
      this.serviceList = this.session.CreateQuery("from Service s where s.Root=0 and s.ServiceId>0 order by " + Options.SortService).List();
      foreach (Service service in (IEnumerable) this.serviceList)
        this.AddRow(this.dtServ, (int) service.ServiceId, service.ServiceName);
      if (Options.ViewEdit)
        return;
      this.dgvdcAbsence.Columns[2].Visible = false;
      this.dgvdcAbsence.Columns[3].Visible = false;
      this.dgvdcAbsenceCoeff.Columns[2].Visible = false;
      this.dgvdcAbsenceCoeff.Columns[3].Visible = false;
    }

    private void CheckAccess()
    {
      this._readOnly = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(32, this._company, false));
      this.toolStrip1.Visible = this._readOnly;
      this.dgvdcAbsence.ReadOnly = !this._readOnly;
      this.toolStrip2.Visible = this._readOnly;
      this.dgvdcAbsenceCoeff.ReadOnly = !this._readOnly;
    }

    public static int FindCurObject(IList list, Absence curObject)
    {
      int num = 0;
      foreach (Absence absence in (IEnumerable) list)
      {
        if (absence.Absence_id.Equals(curObject.Absence_id))
          return num;
        ++num;
      }
      return -1;
    }

    private void GetAbsence()
    {
      if (this.session != null && this.session.IsOpen)
        this.session.Clear();
      this.absenceList.Clear();
      this.absenceList = this.session.CreateQuery("from Absence order by Absence_id").List();
      this.dgvdcAbsence.RowCount = this.absenceList.Count;
      if (this.curAbsence != null)
      {
        int curObject = FrmAbsence.FindCurObject(this.absenceList, this.curAbsence);
        if (curObject >= 0)
        {
          this.dgvdcAbsence.CurrentCell = this.dgvdcAbsence.Rows[curObject].Cells["Id"];
          this.dgvdcAbsence.Rows[curObject].Selected = true;
          this.curAbsence = (Absence) this.absenceList[this.dgvdcAbsence.CurrentRow.Index];
        }
      }
      else if (this.dgvdcAbsence.CurrentRow != null && this.dgvdcAbsence.CurrentRow.Index < this.absenceList.Count)
        this.CurAbsence = (Absence) this.absenceList[this.dgvdcAbsence.CurrentRow.Index];
      this.dgvdcAbsence.Refresh();
    }

    private bool FindByID(short id)
    {
      foreach (Absence absence in (IEnumerable) this.absenceList)
      {
        if ((int) absence.Absence_id == (int) id)
          return true;
      }
      return false;
    }

    private void AddRow(DataTable table, int id, string name)
    {
      DataRow row = table.NewRow();
      row[0] = (object) id;
      row[1] = (object) name;
      table.Rows.Add(row);
    }

    private void FrmAbsence_Load(object sender, EventArgs e)
    {
      this.GetAbsence();
      this._updId.Clear();
    }

    private void dgvdcAbsence_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.absenceList.Count <= 0)
        return;
      if (this.dgvdcAbsence.Columns[e.ColumnIndex].Name == "ID")
        e.Value = (object) ((Absence) this.absenceList[e.RowIndex]).Absence_id;
      if (this.dgvdcAbsence.Columns[e.ColumnIndex].Name == "nameColumn")
        e.Value = (object) ((Absence) this.absenceList[e.RowIndex]).Absence_name;
      if (this.dgvdcAbsence.Columns[e.ColumnIndex].Name == "UName")
        e.Value = (object) ((Absence) this.absenceList[e.RowIndex]).Uname;
      if (this.dgvdcAbsence.Columns[e.ColumnIndex].Name == "DEdit")
        e.Value = (object) ((Absence) this.absenceList[e.RowIndex]).Dedit.ToShortDateString();
    }

    private void dgvdcAbsence_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      if (this.dgvdcAbsence.Columns[e.ColumnIndex].Name == "nameColumn")
        ((Absence) this.absenceList[e.RowIndex]).Absence_name = e.Value.ToString();
      else if (this.curAbsence.AbsenceCoeff.Count == 0)
      {
        if (this.dgvdcAbsence.Columns[e.ColumnIndex].Name == "ID")
        {
          short int16;
          try
          {
            int16 = Convert.ToInt16(e.Value);
          }
          catch
          {
            int num = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
            this.isEdit = false;
            return;
          }
          if (this.FindByID(Convert.ToInt16(e.Value)))
          {
            this.isEdit = false;
            int num = (int) MessageBox.Show("Причина с таким номером уже заведена! Выберите другой номер!", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          this._updId.Add(((Absence) this.absenceList[e.RowIndex]).Absence_id, (Absence) this.absenceList[e.RowIndex]);
          ((Absence) this.absenceList[e.RowIndex]).Absence_id = int16;
          this.dgvdcAbsence.Refresh();
        }
      }
      else
      {
        int num = (int) MessageBox.Show("Изменение невозможно! Существуют данные, заведенные на данную причину.", "Внимание!", MessageBoxButtons.OK);
        this.isEdit = false;
        return;
      }
      if (this.curAbsence != null)
      {
        this.curAbsence.Uname = Options.Login;
        this.curAbsence.Dedit = DateTime.Now.Date;
      }
      this.isEdit = true;
    }

    private void dgvdcAbsence_SelectionChanged(object sender, EventArgs e)
    {
      if (this.dgvdcAbsence.CurrentRow == null || this.dgvdcAbsence.CurrentRow.Index >= this.absenceList.Count)
        return;
      if (this.curAbsenceIdx != this.dgvdcAbsence.CurrentRow.Index)
      {
        this.session.Refresh((object) (Absence) this.absenceList[this.dgvdcAbsence.CurrentRow.Index]);
        this.CurAbsence = (Absence) this.absenceList[this.dgvdcAbsence.CurrentRow.Index];
      }
      if (this.CurAbsence != (Absence) this.absenceList[this.dgvdcAbsence.CurrentRow.Index])
      {
        int curAbsenceIdx = this.curAbsenceIdx;
        this.curAbsenceIdx = this.dgvdcAbsence.CurrentRow.Index;
        this.dgvdcAbsence.CurrentCell = this.dgvdcAbsence.Rows[curAbsenceIdx].Cells[0];
        this.curAbsenceIdx = curAbsenceIdx;
        this.dgvdcAbsence.Rows[curAbsenceIdx].Selected = true;
      }
      this.curAbsenceIdx = this.dgvdcAbsence.CurrentRow.Index;
    }

    private void dgvdcAbsenceCoeff_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.absenceCoeffList.Count <= 0)
        return;
      if (this.dgvdcAbsenceCoeff.Columns[e.ColumnIndex].Name == "Service_id" && (int) this.absenceCoeffList[e.RowIndex].Service_id > 0)
        e.Value = (object) this.absenceCoeffList[e.RowIndex].Service_id;
      else if (this.dgvdcAbsenceCoeff.Columns[e.ColumnIndex].Name == "Coeff")
        e.Value = (object) this.absenceCoeffList[e.RowIndex].Coeff;
      else if (this.dgvdcAbsenceCoeff.Columns[e.ColumnIndex].Name == "UNameCoeff")
        e.Value = (object) this.absenceCoeffList[e.RowIndex].Uname;
      else if (this.dgvdcAbsenceCoeff.Columns[e.ColumnIndex].Name == "DEditCoeff")
        e.Value = (object) this.absenceCoeffList[e.RowIndex].Dedit.ToShortDateString();
    }

    private void dgvdcAbsenceCoeff_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      if (this.dgvdcAbsenceCoeff.Columns[e.ColumnIndex].Name == "Service_id" && e.Value != null)
      {
        try
        {
          this.oldServiceId = (int) this.absenceCoeffList[e.RowIndex].Service_id;
          this.absenceCoeffList[e.RowIndex].Service_id = Convert.ToInt16(e.Value);
        }
        catch
        {
          int num = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
          this.isEdit = false;
          return;
        }
      }
      if (this.dgvdcAbsenceCoeff.Columns[e.ColumnIndex].Name == "Coeff" && e.Value != null)
      {
        try
        {
          this.absenceCoeffList[e.RowIndex].Coeff = Convert.ToDouble(KvrplHelper.ChangeSeparator(e.Value.ToString()));
        }
        catch
        {
          int num = (int) MessageBox.Show("Неверный формат данных!", "Внимание!", MessageBoxButtons.OK);
          this.isEdit = false;
          return;
        }
      }
      this.absenceCoeffList[e.RowIndex].Uname = Options.Login;
      this.absenceCoeffList[e.RowIndex].Dedit = DateTime.Now.Date;
    }

    private void tsbAddAbsence_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      object obj = this.session.CreateQuery("select max(Absence_id) from Absence").UniqueResult();
      int num1 = obj != null ? Convert.ToInt32(obj) + 1 : 1;
      short num2;
      try
      {
        num2 = Convert.ToInt16(num1);
      }
      catch
      {
        num2 = (short) 0;
      }
      this.newAbsence = new Absence();
      this.newAbsence.Absence_id = num2;
      this.newAbsence.Absence_name = "Наименование причины";
      this.absenceList.Add((object) this.newAbsence);
      this.isEdit = true;
      this.dgvdcAbsence.RowCount = this.absenceList.Count;
      this.absenceCoeffList.Clear();
      this.dgvdcAbsenceCoeff.RowCount = 0;
      this.tsbAddAbsence.Enabled = false;
      this.tsbApplayAbsence.Enabled = true;
      this.tsbCancelAbsence.Enabled = true;
      this.tsbDeleteAbsence.Enabled = false;
      this.toolStrip2.Enabled = false;
      this.dgvdcAbsence.CurrentCell = this.dgvdcAbsence.Rows[this.dgvdcAbsence.Rows.Count - 1].Cells[0];
      this.dgvdcAbsence.CurrentRow.Selected = true;
    }

    private void tsbApplayAbsence_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      this.dgvdcAbsence.EndEdit();
      if (!this.isEdit)
        this.isEdit = true;
      else if (!this.isEdit)
      {
        this.isEdit = true;
      }
      else
      {
        if (this.newAbsence != null)
        {
          if (this.newAbsence.Absence_name == null)
          {
            int num = (int) MessageBox.Show("Введите наименование.", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          this.newAbsence.Uname = Options.Login;
          this.newAbsence.Dedit = DateTime.Now.Date;
          try
          {
            this.session.Save((object) this.newAbsence);
            this.session.Flush();
            try
            {
              this.session.Refresh((object) this.newAbsence);
            }
            catch (Exception ex)
            {
              KvrplHelper.WriteLog(ex, (LsClient) null);
            }
            this.GetAbsence();
            this.newAbsence = (Absence) null;
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Ошибка вставки! Проверьте правильность ввода данных.", "Внимание!", MessageBoxButtons.OK);
            KvrplHelper.WriteLog(ex, (LsClient) null);
            this.session.Clear();
            this.session = Domain.CurrentSession;
            return;
          }
        }
        else
        {
          try
          {
            try
            {
              foreach (short key in this._updId.Keys)
              {
                this.session.CreateSQLQuery("update DBA.dcAbsence set Absence_id=:newid,Absence_name=:name,uname=:uname,dedit=:d where Absence_id=:id ").SetInt16("newid", this._updId[key].Absence_id).SetString("name", this._updId[key].Absence_name).SetString("uname", this._updId[key].Uname).SetDateTime("d", this._updId[key].Dedit).SetInt16("id", key).ExecuteUpdate();
                this.session.Evict((object) this._updId[key]);
              }
            }
            catch (Exception ex)
            {
              int num = (int) MessageBox.Show("Изменения внесены не полностью!", "Внимание!", MessageBoxButtons.OK);
              KvrplHelper.WriteLog(ex, (LsClient) null);
            }
            this._updId.Clear();
            this.session.Flush();
            this.GetAbsence();
          }
          catch
          {
            int num = (int) MessageBox.Show("Не удалось внести изменения! Проверьте правильность ввода данных.", "Внимание!", MessageBoxButtons.OK);
            this.session.Clear();
            this.session = Domain.CurrentSession;
            return;
          }
        }
        this.tsbAddAbsence.Enabled = true;
        this.tsbApplayAbsence.Enabled = false;
        this.tsbCancelAbsence.Enabled = false;
        this.tsbDeleteAbsence.Enabled = true;
        this.toolStrip2.Enabled = true;
      }
    }

    private void tsbCancelAbsence_Click(object sender, EventArgs e)
    {
      this.dgvdcAbsence.EndEdit();
      if (this.newAbsence != null)
      {
        if (this.newAbsence != null)
          this.absenceList.Remove((object) this.newAbsence);
      }
      else
      {
        foreach (Absence absence in (IEnumerable) this.absenceList)
          this.session.Refresh((object) absence);
      }
      this.GetAbsence();
      this.tsbAddAbsence.Enabled = true;
      this.tsbApplayAbsence.Enabled = false;
      this.tsbCancelAbsence.Enabled = false;
      this.tsbDeleteAbsence.Enabled = true;
      this.toolStrip2.Enabled = true;
    }

    private void tsbDeleteAbsence_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true) || MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes || this.curAbsence == null)
        return;
      if (Convert.ToInt32(this.session.CreateQuery("select count(*) from LsAbsence where Absence=:abs").SetEntity("abs", (object) this.curAbsence).UniqueResult()) == 0)
      {
        if (this.curAbsence.AbsenceCoeff.Count == 0)
        {
          try
          {
            foreach (AbsenceCoeff absenceCoeff in (IEnumerable) this.curAbsence.AbsenceCoeff)
              this.session.Delete((object) absenceCoeff);
            this.curAbsence.AbsenceCoeff.Clear();
            this.session.Delete((object) this.curAbsence);
            this.session.Flush();
            this.curAbsence = (Absence) null;
            this.GetAbsence();
          }
          catch
          {
            int num = (int) MessageBox.Show("Не удалось удалить запись!", "Внимание!", MessageBoxButtons.OK);
            this.session.Clear();
            this.session = Domain.CurrentSession;
          }
        }
        else
        {
          int num1 = (int) MessageBox.Show("Невозможно удалить запись, так как есть связанные с ней данные!", "Внимание!", MessageBoxButtons.OK);
        }
      }
      else
      {
        int num2 = (int) MessageBox.Show("Невозможно удалить запись, так как есть связанные с ней данные!", "Внимание!", MessageBoxButtons.OK);
      }
    }

    private void tsbExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void tsbAddAbsenceCoeff_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      this.newAbsenceCoeff = new AbsenceCoeff();
      this.newAbsenceCoeff.Absence_id = this.CurAbsence.Absence_id;
      this.newAbsenceCoeff.Coeff = 1.0;
      this.absenceCoeffList.Add(this.newAbsenceCoeff);
      this.dgvdcAbsenceCoeff.RowCount = this.absenceCoeffList.Count;
      try
      {
        this.dgvdcAbsenceCoeff.CurrentCell = this.dgvdcAbsenceCoeff.Rows[this.dgvdcAbsenceCoeff.RowCount - 1].Cells["Service_id"];
      }
      catch
      {
      }
      this.curIndex = this.absenceCoeffList.Count - 1;
      this.tsbAddAbsenceCoeff.Enabled = false;
      this.tsbApplayAbsenceCoeff.Enabled = true;
      this.tsbCancelAbsenceCoeff.Enabled = true;
      this.tsbDeleteAbsenceCoeff.Enabled = false;
      this.toolStrip1.Enabled = false;
    }

    private void tsbApplayAbsenceCoeff_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      this.dgvdcAbsenceCoeff.EndEdit();
      if (this.newAbsenceCoeff != null)
      {
        if ((int) this.newAbsenceCoeff.Service_id == 0)
        {
          int num = (int) MessageBox.Show("Выберите услугу!", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        if (this.newAbsenceCoeff.Coeff < 0.0 || this.newAbsenceCoeff.Coeff > 1.0)
        {
          int num = (int) MessageBox.Show("Значение коэффициента должно быть от 0 до 1", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        this.newAbsenceCoeff.Uname = Options.Login;
        this.newAbsenceCoeff.Dedit = DateTime.Now.Date;
        try
        {
          this.CurAbsence.AbsenceCoeff.Add((object) this.newAbsenceCoeff);
          this.session.Save((object) this.newAbsenceCoeff);
          this.session.Flush();
          this.session.Refresh((object) this.CurAbsence);
          this.tsbApplayAbsenceCoeff.Enabled = false;
          this.GetAbsence();
          this.dgvdcAbsenceCoeff.RowCount = this.absenceCoeffList.Count;
          this.curAbsenceCoeff = this.newAbsenceCoeff;
          this.newAbsenceCoeff = (AbsenceCoeff) null;
        }
        catch
        {
          int num = (int) MessageBox.Show("Ошибка вставки! Проверьте правильность ввода данных.", "Внимание!", MessageBoxButtons.OK);
          return;
        }
      }
      else
      {
        if (this.curAbsenceCoeff.Coeff < 0.0 || this.curAbsenceCoeff.Coeff > 1.0)
        {
          int num = (int) MessageBox.Show("Значение коэффициента должно быть от 0 до 1", "Внимание!", MessageBoxButtons.OK);
          return;
        }
        try
        {
          this.session.CreateQuery("update AbsenceCoeff set Service_id=:serv1,Coeff=:coeff,Uname=:user,Dedit=:dedit where Absence_id=:abs and Service_id=:serv2").SetParameter<short>("serv1", this.curAbsenceCoeff.Service_id).SetParameter<double>("coeff", this.curAbsenceCoeff.Coeff).SetParameter<string>("user", Options.Login).SetParameter<DateTime>("dedit", DateTime.Now.Date).SetParameter<short>("abs", this.curAbsence.Absence_id).SetParameter<int>("serv2", this.oldServiceId).ExecuteUpdate();
          this.session.Flush();
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Не удалось внести изменения! Проверьте правильность ввода данных.");
          KvrplHelper.WriteLog(ex, (LsClient) null);
          return;
        }
      }
      if (this.curAbsenceCoeff != null)
      {
        this.absenceCoeffList.Clear();
        this.dgvdcAbsenceCoeff.RowCount = 0;
        if (this.CurAbsence.AbsenceCoeff.Count > 0)
        {
          foreach (AbsenceCoeff absenceCoeff in (IEnumerable<AbsenceCoeff>) this.session.CreateQuery(string.Format("select ac from AbsenceCoeff ac,Service s where ac.Service_id=s.ServiceId and Absence_id={0} order by " + Options.SortService, (object) this.curAbsence.Absence_id)).List<AbsenceCoeff>())
            this.absenceCoeffList.Add(absenceCoeff);
          if (this.dgvdcAbsenceCoeff.CurrentRow != null && this.absenceCoeffList.Count > this.dgvdcAbsenceCoeff.CurrentRow.Index && this.absenceCoeffList.Count > 0 && this.dgvdcAbsenceCoeff.CurrentRow.Index >= 0)
            this.curAbsenceCoeff = this.absenceCoeffList[this.dgvdcAbsenceCoeff.CurrentRow.Index];
        }
        this.dgvdcAbsenceCoeff.RowCount = this.absenceCoeffList.Count;
      }
      this.tsbAddAbsenceCoeff.Enabled = true;
      this.tsbApplayAbsenceCoeff.Enabled = false;
      this.tsbCancelAbsenceCoeff.Enabled = false;
      this.tsbDeleteAbsenceCoeff.Enabled = true;
      this.toolStrip1.Enabled = true;
    }

    private void tsbCancelAbsenceCoeff_Click(object sender, EventArgs e)
    {
      this.dgvdcAbsenceCoeff.EndEdit();
      if (this.newAbsenceCoeff != null)
      {
        if (this.newAbsenceCoeff != null)
        {
          this.absenceCoeffList.Remove(this.newAbsenceCoeff);
          this.newAbsenceCoeff = (AbsenceCoeff) null;
        }
      }
      else if (this.CurAbsence != null)
        this.session.Refresh((object) this.CurAbsence);
      this.GetAbsence();
      this.absenceCoeffList.Clear();
      this.dgvdcAbsenceCoeff.RowCount = 0;
      if (this.CurAbsence.AbsenceCoeff.Count > 0)
      {
        foreach (AbsenceCoeff absenceCoeff in (IEnumerable<AbsenceCoeff>) this.session.CreateQuery(string.Format("select ac from AbsenceCoeff ac,Service s where ac.Service_id=s.ServiceId and Absence_id={0} order by " + Options.SortService, (object) this.curAbsence.Absence_id)).List<AbsenceCoeff>())
          this.absenceCoeffList.Add(absenceCoeff);
      }
      this.dgvdcAbsenceCoeff.RowCount = this.absenceCoeffList.Count;
      this.tsbAddAbsenceCoeff.Enabled = true;
      this.tsbApplayAbsenceCoeff.Enabled = false;
      this.tsbCancelAbsenceCoeff.Enabled = false;
      this.tsbDeleteAbsenceCoeff.Enabled = true;
      this.toolStrip1.Enabled = true;
    }

    private void tsbDeleteAbsenceCoeff_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true) || MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes || this.curAbsenceCoeff == null)
        return;
      this.session.Clear();
      this.CurAbsence.AbsenceCoeff.Remove((object) this.curAbsenceCoeff);
      this.session.Delete((object) this.curAbsenceCoeff);
      this.session.Flush();
      this.GetAbsence();
      this.absenceCoeffList.Clear();
      if (this.CurAbsence.AbsenceCoeff.Count > 0)
      {
        foreach (AbsenceCoeff absenceCoeff in (IEnumerable<AbsenceCoeff>) this.session.CreateQuery(string.Format("select ac from AbsenceCoeff ac,Service s where ac.Service_id=s.ServiceId and Absence_id={0} order by " + Options.SortService, (object) this.curAbsence.Absence_id)).List<AbsenceCoeff>())
          this.absenceCoeffList.Add(absenceCoeff);
        if (this.dgvdcAbsenceCoeff.CurrentRow != null && this.absenceCoeffList.Count > this.dgvdcAbsenceCoeff.CurrentRow.Index && this.absenceCoeffList.Count > 0 && this.dgvdcAbsenceCoeff.CurrentRow.Index >= 0)
          this.curAbsenceCoeff = this.absenceCoeffList[this.dgvdcAbsenceCoeff.CurrentRow.Index];
      }
      this.dgvdcAbsenceCoeff.RowCount = this.absenceCoeffList.Count;
    }

    private void dgvdcAbsenceCoeff_SelectionChanged(object sender, EventArgs e)
    {
      if (this.dgvdcAbsenceCoeff.CurrentRow == null || this.dgvdcAbsenceCoeff.CurrentRow.Index == this.curIndex)
        return;
      if (this.curIndex >= 0 && this.newAbsenceCoeff != null)
      {
        if (MessageBox.Show("Сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
          this.tsbApplayAbsenceCoeff_Click(sender, e);
        }
        else
        {
          this.tsbCancelAbsenceCoeff_Click(sender, e);
          this.curIndex = this.dgvdcAbsenceCoeff.CurrentRow.Index;
          if (this.absenceCoeffList.Count > 0 && this.dgvdcAbsenceCoeff.CurrentRow.Index < this.absenceCoeffList.Count)
            this.curAbsenceCoeff = this.absenceCoeffList[this.dgvdcAbsenceCoeff.CurrentRow.Index];
        }
      }
      else if (this.curIndex >= 0)
      {
        this.curIndex = this.dgvdcAbsenceCoeff.CurrentRow.Index;
        if (this.absenceCoeffList.Count > 0 && this.dgvdcAbsenceCoeff.CurrentRow.Index < this.absenceCoeffList.Count)
          this.curAbsenceCoeff = this.absenceCoeffList[this.dgvdcAbsenceCoeff.CurrentRow.Index];
      }
    }

    private void dgvdcAbsence_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.tsbAddAbsence.Enabled = false;
      this.tsbApplayAbsence.Enabled = true;
      this.tsbCancelAbsence.Enabled = true;
      this.tsbDeleteAbsence.Enabled = false;
    }

    private void dgvdcAbsenceCoeff_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      this.tsbAddAbsenceCoeff.Enabled = false;
      this.tsbApplayAbsenceCoeff.Enabled = true;
      this.tsbCancelAbsenceCoeff.Enabled = true;
      this.tsbDeleteAbsenceCoeff.Enabled = false;
    }

    private void bntExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void dgvdcAbsence_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      KvrplHelper.WriteError(this.Name, ((Control) sender).Name, e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmAbsence));
      this.dsMain = new DataSet();
      this.dtServ = new DataTable();
      this.dataColumn1 = new DataColumn();
      this.dataColumn2 = new DataColumn();
      this.pnBtn = new Panel();
      this.bntExit = new Button();
      this.splitContainer1 = new SplitContainer();
      this.groupBox2 = new GroupBox();
      this.dgvdcAbsence = new DataGridView();
      this.ID = new DataGridViewTextBoxColumn();
      this.nameColumn = new DataGridViewTextBoxColumn();
      this.UName = new DataGridViewTextBoxColumn();
      this.DEdit = new DataGridViewTextBoxColumn();
      this.toolStrip1 = new ToolStrip();
      this.tsbAddAbsence = new ToolStripButton();
      this.tsbApplayAbsence = new ToolStripButton();
      this.tsbCancelAbsence = new ToolStripButton();
      this.tsbDeleteAbsence = new ToolStripButton();
      this.groupBox1 = new GroupBox();
      this.dgvdcAbsenceCoeff = new DataGridView();
      this.Service_id = new DataGridViewComboBoxColumn();
      this.Coeff = new DataGridViewTextBoxColumn();
      this.UNameCoeff = new DataGridViewTextBoxColumn();
      this.DEditCoeff = new DataGridViewTextBoxColumn();
      this.toolStrip2 = new ToolStrip();
      this.tsbAddAbsenceCoeff = new ToolStripButton();
      this.tsbApplayAbsenceCoeff = new ToolStripButton();
      this.tsbCancelAbsenceCoeff = new ToolStripButton();
      this.tsbDeleteAbsenceCoeff = new ToolStripButton();
      this.tsbExit = new ToolStripButton();
      this.hp = new HelpProvider();
      this.dsMain.BeginInit();
      this.dtServ.BeginInit();
      this.pnBtn.SuspendLayout();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      ((ISupportInitialize) this.dgvdcAbsence).BeginInit();
      this.toolStrip1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      ((ISupportInitialize) this.dgvdcAbsenceCoeff).BeginInit();
      this.toolStrip2.SuspendLayout();
      this.SuspendLayout();
      this.dsMain.DataSetName = "NewDataSet";
      this.dsMain.Tables.AddRange(new DataTable[1]
      {
        this.dtServ
      });
      this.dtServ.Columns.AddRange(new DataColumn[2]
      {
        this.dataColumn1,
        this.dataColumn2
      });
      this.dtServ.TableName = "Table1";
      this.dataColumn1.ColumnName = "ID";
      this.dataColumn1.DataType = typeof (short);
      this.dataColumn2.ColumnName = "Name";
      this.pnBtn.Controls.Add((Control) this.bntExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 447);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(1071, 40);
      this.pnBtn.TabIndex = 0;
      this.bntExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.bntExit.DialogResult = DialogResult.Cancel;
      this.bntExit.Image = (Image) Resources.Exit;
      this.bntExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.bntExit.Location = new Point(975, 5);
      this.bntExit.Name = "bntExit";
      this.bntExit.Size = new Size(84, 30);
      this.bntExit.TabIndex = 0;
      this.bntExit.Text = "Выход";
      this.bntExit.TextAlign = ContentAlignment.MiddleRight;
      this.bntExit.UseVisualStyleBackColor = true;
      this.bntExit.Click += new EventHandler(this.bntExit_Click);
      this.splitContainer1.Dock = DockStyle.Fill;
      this.splitContainer1.Location = new Point(0, 0);
      this.splitContainer1.Margin = new Padding(4);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.groupBox2);
      this.splitContainer1.Panel2.Controls.Add((Control) this.groupBox1);
      this.splitContainer1.Size = new Size(1071, 447);
      this.splitContainer1.SplitterDistance = 449;
      this.splitContainer1.SplitterWidth = 5;
      this.splitContainer1.TabIndex = 1;
      this.groupBox2.Controls.Add((Control) this.dgvdcAbsence);
      this.groupBox2.Controls.Add((Control) this.toolStrip1);
      this.groupBox2.Dock = DockStyle.Fill;
      this.groupBox2.Location = new Point(0, 0);
      this.groupBox2.Margin = new Padding(4);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Padding = new Padding(4);
      this.groupBox2.Size = new Size(449, 447);
      this.groupBox2.TabIndex = 0;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Причины отсутствия";
      this.dgvdcAbsence.AllowUserToAddRows = false;
      this.dgvdcAbsence.AllowUserToDeleteRows = false;
      this.dgvdcAbsence.BackgroundColor = Color.AliceBlue;
      this.dgvdcAbsence.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvdcAbsence.Columns.AddRange((DataGridViewColumn) this.ID, (DataGridViewColumn) this.nameColumn, (DataGridViewColumn) this.UName, (DataGridViewColumn) this.DEdit);
      this.dgvdcAbsence.Dock = DockStyle.Fill;
      this.dgvdcAbsence.Location = new Point(4, 44);
      this.dgvdcAbsence.Margin = new Padding(4);
      this.dgvdcAbsence.MultiSelect = false;
      this.dgvdcAbsence.Name = "dgvdcAbsence";
      this.dgvdcAbsence.Size = new Size(441, 399);
      this.dgvdcAbsence.TabIndex = 1;
      this.dgvdcAbsence.VirtualMode = true;
      this.dgvdcAbsence.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvdcAbsence_CellBeginEdit);
      this.dgvdcAbsence.CellValueNeeded += new DataGridViewCellValueEventHandler(this.dgvdcAbsence_CellValueNeeded);
      this.dgvdcAbsence.CellValuePushed += new DataGridViewCellValueEventHandler(this.dgvdcAbsence_CellValuePushed);
      this.dgvdcAbsence.DataError += new DataGridViewDataErrorEventHandler(this.dgvdcAbsence_DataError);
      this.dgvdcAbsence.SelectionChanged += new EventHandler(this.dgvdcAbsence_SelectionChanged);
      gridViewCellStyle1.NullValue = (object) null;
      this.ID.DefaultCellStyle = gridViewCellStyle1;
      this.ID.HeaderText = "№";
      this.ID.Name = "ID";
      this.ID.Width = 70;
      this.nameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.nameColumn.HeaderText = "Наименование причины";
      this.nameColumn.Name = "nameColumn";
      this.UName.HeaderText = "Пользователь";
      this.UName.Name = "UName";
      this.UName.ReadOnly = true;
      this.DEdit.HeaderText = "Дата редактирования";
      this.DEdit.Name = "DEdit";
      this.DEdit.ReadOnly = true;
      this.toolStrip1.Font = new Font("Tahoma", 10f);
      this.toolStrip1.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.tsbAddAbsence,
        (ToolStripItem) this.tsbApplayAbsence,
        (ToolStripItem) this.tsbCancelAbsence,
        (ToolStripItem) this.tsbDeleteAbsence
      });
      this.toolStrip1.LayoutStyle = ToolStripLayoutStyle.Flow;
      this.toolStrip1.Location = new Point(4, 20);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new Size(441, 24);
      this.toolStrip1.TabIndex = 0;
      this.toolStrip1.Text = "toolStrip1";
      this.tsbAddAbsence.Image = (Image) Resources.add_var;
      this.tsbAddAbsence.ImageTransparentColor = Color.Magenta;
      this.tsbAddAbsence.Name = "tsbAddAbsence";
      this.tsbAddAbsence.Size = new Size(91, 21);
      this.tsbAddAbsence.Text = "Добавить";
      this.tsbAddAbsence.Click += new EventHandler(this.tsbAddAbsence_Click);
      this.tsbApplayAbsence.Enabled = false;
      this.tsbApplayAbsence.Image = (Image) Resources.Applay_var;
      this.tsbApplayAbsence.ImageTransparentColor = Color.Magenta;
      this.tsbApplayAbsence.Name = "tsbApplayAbsence";
      this.tsbApplayAbsence.Size = new Size(99, 21);
      this.tsbApplayAbsence.Text = "Сохранить";
      this.tsbApplayAbsence.Click += new EventHandler(this.tsbApplayAbsence_Click);
      this.tsbCancelAbsence.Enabled = false;
      this.tsbCancelAbsence.Image = (Image) Resources.undo;
      this.tsbCancelAbsence.ImageTransparentColor = Color.Magenta;
      this.tsbCancelAbsence.Name = "tsbCancelAbsence";
      this.tsbCancelAbsence.Size = new Size(93, 21);
      this.tsbCancelAbsence.Text = "Отменить";
      this.tsbCancelAbsence.Click += new EventHandler(this.tsbCancelAbsence_Click);
      this.tsbDeleteAbsence.Image = (Image) Resources.delete_var;
      this.tsbDeleteAbsence.ImageTransparentColor = Color.Magenta;
      this.tsbDeleteAbsence.Name = "tsbDeleteAbsence";
      this.tsbDeleteAbsence.Size = new Size(82, 21);
      this.tsbDeleteAbsence.Text = "Удалить";
      this.tsbDeleteAbsence.Click += new EventHandler(this.tsbDeleteAbsence_Click);
      this.groupBox1.Controls.Add((Control) this.dgvdcAbsenceCoeff);
      this.groupBox1.Controls.Add((Control) this.toolStrip2);
      this.groupBox1.Dock = DockStyle.Fill;
      this.groupBox1.Location = new Point(0, 0);
      this.groupBox1.Margin = new Padding(4);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new Padding(4);
      this.groupBox1.Size = new Size(617, 447);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Услуги, подлежащие снятию при отсутствии";
      this.dgvdcAbsenceCoeff.AllowUserToAddRows = false;
      this.dgvdcAbsenceCoeff.AllowUserToDeleteRows = false;
      this.dgvdcAbsenceCoeff.BackgroundColor = Color.AliceBlue;
      this.dgvdcAbsenceCoeff.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvdcAbsenceCoeff.Columns.AddRange((DataGridViewColumn) this.Service_id, (DataGridViewColumn) this.Coeff, (DataGridViewColumn) this.UNameCoeff, (DataGridViewColumn) this.DEditCoeff);
      this.dgvdcAbsenceCoeff.Dock = DockStyle.Fill;
      this.dgvdcAbsenceCoeff.Location = new Point(4, 44);
      this.dgvdcAbsenceCoeff.Margin = new Padding(4);
      this.dgvdcAbsenceCoeff.MultiSelect = false;
      this.dgvdcAbsenceCoeff.Name = "dgvdcAbsenceCoeff";
      this.dgvdcAbsenceCoeff.Size = new Size(609, 399);
      this.dgvdcAbsenceCoeff.TabIndex = 2;
      this.dgvdcAbsenceCoeff.VirtualMode = true;
      this.dgvdcAbsenceCoeff.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dgvdcAbsenceCoeff_CellBeginEdit);
      this.dgvdcAbsenceCoeff.CellValueNeeded += new DataGridViewCellValueEventHandler(this.dgvdcAbsenceCoeff_CellValueNeeded);
      this.dgvdcAbsenceCoeff.CellValuePushed += new DataGridViewCellValueEventHandler(this.dgvdcAbsenceCoeff_CellValuePushed);
      this.dgvdcAbsenceCoeff.DataError += new DataGridViewDataErrorEventHandler(this.dgvdcAbsence_DataError);
      this.dgvdcAbsenceCoeff.SelectionChanged += new EventHandler(this.dgvdcAbsenceCoeff_SelectionChanged);
      this.Service_id.DataSource = (object) this.dsMain;
      this.Service_id.DisplayMember = "Table1.Name";
      this.Service_id.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
      this.Service_id.HeaderText = "Услуга";
      this.Service_id.Name = "Service_id";
      this.Service_id.Resizable = DataGridViewTriState.True;
      this.Service_id.SortMode = DataGridViewColumnSortMode.Automatic;
      this.Service_id.ValueMember = "Table1.ID";
      this.Service_id.Width = 300;
      gridViewCellStyle2.Format = "N2";
      gridViewCellStyle2.NullValue = (object) null;
      this.Coeff.DefaultCellStyle = gridViewCellStyle2;
      this.Coeff.HeaderText = "Коэффициент";
      this.Coeff.Name = "Coeff";
      this.Coeff.Width = 110;
      this.UNameCoeff.HeaderText = "Пользователь";
      this.UNameCoeff.Name = "UNameCoeff";
      this.UNameCoeff.ReadOnly = true;
      this.DEditCoeff.HeaderText = "Дата редактирования";
      this.DEditCoeff.Name = "DEditCoeff";
      this.DEditCoeff.ReadOnly = true;
      this.toolStrip2.Font = new Font("Tahoma", 10f);
      this.toolStrip2.Items.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.tsbAddAbsenceCoeff,
        (ToolStripItem) this.tsbApplayAbsenceCoeff,
        (ToolStripItem) this.tsbCancelAbsenceCoeff,
        (ToolStripItem) this.tsbDeleteAbsenceCoeff,
        (ToolStripItem) this.tsbExit
      });
      this.toolStrip2.LayoutStyle = ToolStripLayoutStyle.Flow;
      this.toolStrip2.Location = new Point(4, 20);
      this.toolStrip2.Name = "toolStrip2";
      this.toolStrip2.Size = new Size(609, 24);
      this.toolStrip2.TabIndex = 1;
      this.toolStrip2.Text = "toolStrip2";
      this.tsbAddAbsenceCoeff.Image = (Image) Resources.add_var;
      this.tsbAddAbsenceCoeff.ImageTransparentColor = Color.Magenta;
      this.tsbAddAbsenceCoeff.Name = "tsbAddAbsenceCoeff";
      this.tsbAddAbsenceCoeff.Size = new Size(91, 21);
      this.tsbAddAbsenceCoeff.Text = "Добавить";
      this.tsbAddAbsenceCoeff.Click += new EventHandler(this.tsbAddAbsenceCoeff_Click);
      this.tsbApplayAbsenceCoeff.Enabled = false;
      this.tsbApplayAbsenceCoeff.Image = (Image) Resources.Applay_var;
      this.tsbApplayAbsenceCoeff.ImageTransparentColor = Color.Magenta;
      this.tsbApplayAbsenceCoeff.Name = "tsbApplayAbsenceCoeff";
      this.tsbApplayAbsenceCoeff.Size = new Size(99, 21);
      this.tsbApplayAbsenceCoeff.Text = "Сохранить";
      this.tsbApplayAbsenceCoeff.Click += new EventHandler(this.tsbApplayAbsenceCoeff_Click);
      this.tsbCancelAbsenceCoeff.Enabled = false;
      this.tsbCancelAbsenceCoeff.Image = (Image) Resources.undo;
      this.tsbCancelAbsenceCoeff.ImageTransparentColor = Color.Magenta;
      this.tsbCancelAbsenceCoeff.Name = "tsbCancelAbsenceCoeff";
      this.tsbCancelAbsenceCoeff.Size = new Size(93, 21);
      this.tsbCancelAbsenceCoeff.Text = "Отменить";
      this.tsbCancelAbsenceCoeff.Click += new EventHandler(this.tsbCancelAbsenceCoeff_Click);
      this.tsbDeleteAbsenceCoeff.Image = (Image) Resources.delete_var;
      this.tsbDeleteAbsenceCoeff.ImageTransparentColor = Color.Magenta;
      this.tsbDeleteAbsenceCoeff.Name = "tsbDeleteAbsenceCoeff";
      this.tsbDeleteAbsenceCoeff.Size = new Size(82, 21);
      this.tsbDeleteAbsenceCoeff.Text = "Удалить";
      this.tsbDeleteAbsenceCoeff.Click += new EventHandler(this.tsbDeleteAbsenceCoeff_Click);
      this.tsbExit.Image = (Image) Resources.Exit;
      this.tsbExit.ImageTransparentColor = Color.Magenta;
      this.tsbExit.Name = "tsbExit";
      this.tsbExit.Size = new Size(70, 21);
      this.tsbExit.Text = "Выход";
      this.tsbExit.Visible = false;
      this.tsbExit.Click += new EventHandler(this.tsbExit_Click);
      this.hp.HelpNamespace = "Help.chm";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.bntExit;
      this.ClientSize = new Size(1071, 487);
      this.Controls.Add((Control) this.splitContainer1);
      this.Controls.Add((Control) this.pnBtn);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.hp.SetHelpKeyword((Control) this, "kv53.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmAbsence";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Отсутствие";
      this.Load += new EventHandler(this.FrmAbsence_Load);
      this.dsMain.EndInit();
      this.dtServ.EndInit();
      this.pnBtn.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      ((ISupportInitialize) this.dgvdcAbsence).EndInit();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((ISupportInitialize) this.dgvdcAbsenceCoeff).EndInit();
      this.toolStrip2.ResumeLayout(false);
      this.toolStrip2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
