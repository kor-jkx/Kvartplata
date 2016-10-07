// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmSourcePay
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmSourcePay : FrmBase
  {
    private Dictionary<short, SourcePay> _updId = new Dictionary<short, SourcePay>();
    private bool _readOnly = false;
    private IContainer components = (IContainer) null;
    private Company _company;
    public HelpProvider hp;

    public FrmSourcePay(Company company)
    {
      this.InitializeComponent();
      this._company = company;
      this.CheckAccess();
      this.session = Domain.CurrentSession;
    }

    private void CheckAccess()
    {
      this._readOnly = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(32, this._company, false));
      this.toolStrip1.Visible = this._readOnly;
      this.dgvBase.ReadOnly = !this._readOnly;
    }

    protected override void GetList()
    {
      this.objectsList.Clear();
      this.objectsList = this.session.CreateQuery("from SourcePay order by SourcePayId").List();
      this.dgvBase.RowCount = this.objectsList.Count;
      this.dgvBase.Refresh();
      this.SelectRow();
    }

    private bool FindByID(short id)
    {
      foreach (SourcePay objects in (IEnumerable) this.objectsList)
      {
        if ((int) objects.SourcePayId == (int) id)
          return true;
      }
      return false;
    }

    protected override void dgvBase_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.objectsList.Count <= 0)
        return;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "ID")
        e.Value = (object) ((SourcePay) this.objectsList[e.RowIndex]).SourcePayId;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "nameColumn")
        e.Value = (object) ((SourcePay) this.objectsList[e.RowIndex]).SourcePayName;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "UName")
        e.Value = (object) ((SourcePay) this.objectsList[e.RowIndex]).Uname;
      else if (this.dgvBase.Columns[e.ColumnIndex].Name == "DEdit")
        e.Value = (object) ((SourcePay) this.objectsList[e.RowIndex]).Dedit.ToShortDateString();
    }

    protected override void dgvBase_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      int int32 = Convert.ToInt32(this.session.CreateQuery(string.Format("select count(*) from Payment where SourcePay_Id={0}", (object) ((SourcePay) this.curObject).SourcePayId)).UniqueResult());
      ((SourcePay) this.objectsList[e.RowIndex]).Uname = Options.Login;
      ((SourcePay) this.objectsList[e.RowIndex]).Dedit = DateTime.Now;
      if (this.dgvBase.Columns[e.ColumnIndex].Name == "nameColumn")
        ((SourcePay) this.objectsList[e.RowIndex]).SourcePayName = e.Value.ToString();
      else if (int32 == 0)
      {
        if (this.dgvBase.Columns[e.ColumnIndex].Name == "ID")
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
          if (this.FindByID(int16))
          {
            this.isEdit = false;
            int num = (int) MessageBox.Show("Источник поступлений с таким номером уже заведен! Выберите другой номер!", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          this._updId.Add(((SourcePay) this.objectsList[e.RowIndex]).SourcePayId, (SourcePay) this.objectsList[e.RowIndex]);
          ((SourcePay) this.objectsList[e.RowIndex]).SourcePayId = int16;
          this.dgvBase.Refresh();
        }
      }
      else
      {
        int num = (int) MessageBox.Show("Изменение невозможно! Существуют данные, связанные с данным источником.", "Внимание!", MessageBoxButtons.OK);
        this.isEdit = false;
        return;
      }
      this.isEdit = true;
    }

    protected override void tsbAdd_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      object obj = this.session.CreateQuery("select max(SourcePayId) from SourcePay").UniqueResult();
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
      this.newObject = (object) new SourcePay();
      ((SourcePay) this.newObject).SourcePayId = num2;
      ((SourcePay) this.newObject).SourcePayName = "Наименование источника";
      this.isEdit = true;
      base.tsbAdd_Click(sender, e);
    }

    protected override void tsbApplay_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      this.dgvBase.EndEdit();
      if (!this.isEdit)
      {
        this.isEdit = true;
      }
      else
      {
        if (this.newObject != null)
        {
          if (((SourcePay) this.newObject).SourcePayName == null)
          {
            int num = (int) MessageBox.Show("Введите наименование.", "Внимание!", MessageBoxButtons.OK);
            return;
          }
          ((SourcePay) this.newObject).Uname = Options.Login;
          ((SourcePay) this.newObject).Dedit = DateTime.Now;
        }
        else
        {
          try
          {
            foreach (short key in this._updId.Keys)
            {
              string queryString = "update DBA.dcSourcePay set SourcePay_id=:newid,SourcePay_name=:name,uname=:uname,dedit=:d where SourcePay_id=:id";
              if (queryString != "")
                this.session.CreateSQLQuery(queryString).SetInt16("newid", this._updId[key].SourcePayId).SetString("name", this._updId[key].SourcePayName).SetString("uname", this._updId[key].Uname).SetDateTime("d", this._updId[key].Dedit).SetInt16("id", key).ExecuteUpdate();
              this.session.Evict((object) this._updId[key]);
            }
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Изменения внесены не полностью!", "Внимание!", MessageBoxButtons.OK);
            KvrplHelper.WriteLog(ex, (LsClient) null);
          }
          this._updId.Clear();
        }
        base.tsbApplay_Click(sender, e);
      }
    }

    protected override void tsbDelete_Click(object sender, EventArgs e)
    {
      if (!KvrplHelper.CheckProxy(32, 2, (Company) null, true))
        return;
      if (this.curObject != null && Convert.ToInt32(this.session.CreateQuery(string.Format("select count(*) from Payment where SourcePay_Id={0}", (object) ((SourcePay) this.curObject).SourcePayId)).UniqueResult()) == 0)
      {
        base.tsbDelete_Click(sender, e);
      }
      else
      {
        int num = (int) MessageBox.Show("Удаление невозможно! Существуют данные, ссылающиеся на эту запись.", "Внимание!", MessageBoxButtons.OK);
      }
    }

    protected override void tsbCancel_Click(object sender, EventArgs e)
    {
      this.dgvBase.EndEdit();
      foreach (short key in this._updId.Keys)
        this._updId[key].SourcePayId = key;
      this._updId.Clear();
      base.tsbCancel_Click(sender, e);
    }

    protected override void FrmBase_Load(object sender, EventArgs e)
    {
      this._updId.Clear();
      base.FrmBase_Load(sender, e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmSourcePay));
      this.hp = new HelpProvider();
      this.SuspendLayout();
      this.hp.HelpNamespace = "Help.chm";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(483, 434);
      this.hp.SetHelpKeyword((Control) this, "kv55.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Location = new Point(0, 0);
      this.Margin = new Padding(5);
      this.Name = "FrmSourcePay";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Источники поступлений";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
