// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmLegislation
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Properties;
using NHibernate;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmLegislation : FrmBaseForm
  {
    private IList legislationList = (IList) new ArrayList();
    public string Legislation_name = "";
    private IContainer components = (IContainer) null;
    private ISession session;
    private DataGridView dgvLegislation;
    private Button btnCancel;
    private Button btnOK;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
    private DataGridViewTextBoxColumn Service_name;
    private DataGridViewTextBoxColumn Id;
    private DataGridViewTextBoxColumn OverShoot;
    private DataGridViewTextBoxColumn Normal;
    private DataGridViewTextBoxColumn Description;
    private DataGridViewTextBoxColumn Measure;
    private DataGridViewTextBoxColumn Standart;
    private DataGridViewTextBoxColumn Percent;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

    public short Legislation_id { get; set; }

    public FrmLegislation()
    {
      this.InitializeComponent();
      this.session = Domain.CurrentSession;
      this.Legislation_id = (short) -1;
    }

    private void FrmLegislation_Load(object sender, EventArgs e)
    {
      this.legislationList = this.session.CreateQuery("from Legislation order by Service_name,Legislation_id").List();
      this.legislationList.Insert(0, (object) new Legislation()
      {
        Legislation_id = (short) -1
      });
      int num = 0;
      bool flag = false;
      foreach (Legislation legislation in (IEnumerable) this.legislationList)
      {
        ++num;
        if ((int) legislation.Legislation_id == (int) this.Legislation_id)
        {
          flag = true;
          break;
        }
      }
      this.dgvLegislation.RowCount = this.legislationList.Count;
      if (!(this.legislationList.Count > 0 & flag))
        return;
      this.dgvLegislation.CurrentCell = this.dgvLegislation.Rows[num - 1].Cells[0];
    }

    private void dataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.legislationList.Count <= 0)
        return;
      if (this.dgvLegislation.Columns[e.ColumnIndex].Name == "Service_name")
        e.Value = (object) ((Legislation) this.legislationList[e.RowIndex]).Service_name;
      else if (this.dgvLegislation.Columns[e.ColumnIndex].Name == "Id")
        e.Value = (int) ((Legislation) this.legislationList[e.RowIndex]).Legislation_id == -1 ? (object) "" : (object) ((Legislation) this.legislationList[e.RowIndex]).Legislation_id;
      else if (this.dgvLegislation.Columns[e.ColumnIndex].Name == "OverShoot")
        e.Value = (object) ((Legislation) this.legislationList[e.RowIndex]).OverShoot;
      else if (this.dgvLegislation.Columns[e.ColumnIndex].Name == "Normal")
        e.Value = (object) ((Legislation) this.legislationList[e.RowIndex]).Normal;
      else if (this.dgvLegislation.Columns[e.ColumnIndex].Name == "Description")
        e.Value = (object) ((Legislation) this.legislationList[e.RowIndex]).Description;
      else if (this.dgvLegislation.Columns[e.ColumnIndex].Name == "Measure")
        e.Value = (object) ((Legislation) this.legislationList[e.RowIndex]).Measure;
      else if (this.dgvLegislation.Columns[e.ColumnIndex].Name == "Standart")
        e.Value = (object) ((Legislation) this.legislationList[e.RowIndex]).Standart;
      else if (this.dgvLegislation.Columns[e.ColumnIndex].Name == "Percent")
        e.Value = (object) ((Legislation) this.legislationList[e.RowIndex]).Percent;
    }

    private void dgvLegislation_SelectionChanged(object sender, EventArgs e)
    {
      this.Legislation_id = ((Legislation) this.legislationList[this.dgvLegislation.CurrentRow.Index]).Legislation_id;
      this.Legislation_name = ((Legislation) this.legislationList[this.dgvLegislation.CurrentRow.Index]).OverShoot;
    }

    private void dgvLegislation_DataError(object sender, DataGridViewDataErrorEventArgs e)
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
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmLegislation));
      this.dgvLegislation = new DataGridView();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
      this.Service_name = new DataGridViewTextBoxColumn();
      this.Id = new DataGridViewTextBoxColumn();
      this.OverShoot = new DataGridViewTextBoxColumn();
      this.Normal = new DataGridViewTextBoxColumn();
      this.Description = new DataGridViewTextBoxColumn();
      this.Measure = new DataGridViewTextBoxColumn();
      this.Standart = new DataGridViewTextBoxColumn();
      this.Percent = new DataGridViewTextBoxColumn();
      ((ISupportInitialize) this.dgvLegislation).BeginInit();
      this.SuspendLayout();
      this.dgvLegislation.AllowUserToAddRows = false;
      this.dgvLegislation.AllowUserToDeleteRows = false;
      this.dgvLegislation.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dgvLegislation.BackgroundColor = Color.AliceBlue;
      this.dgvLegislation.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvLegislation.Columns.AddRange((DataGridViewColumn) this.Service_name, (DataGridViewColumn) this.Id, (DataGridViewColumn) this.OverShoot, (DataGridViewColumn) this.Normal, (DataGridViewColumn) this.Description, (DataGridViewColumn) this.Measure, (DataGridViewColumn) this.Standart, (DataGridViewColumn) this.Percent);
      this.dgvLegislation.Location = new Point(1, -1);
      this.dgvLegislation.Margin = new Padding(4);
      this.dgvLegislation.Name = "dgvLegislation";
      this.dgvLegislation.RowHeadersVisible = false;
      this.dgvLegislation.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
      this.dgvLegislation.RowTemplate.Height = 54;
      this.dgvLegislation.RowTemplate.Resizable = DataGridViewTriState.True;
      this.dgvLegislation.Size = new Size(1282, 557);
      this.dgvLegislation.TabIndex = 0;
      this.dgvLegislation.VirtualMode = true;
      this.dgvLegislation.CellValueNeeded += new DataGridViewCellValueEventHandler(this.dataGridView1_CellValueNeeded);
      this.dgvLegislation.DataError += new DataGridViewDataErrorEventHandler(this.dgvLegislation_DataError);
      this.dgvLegislation.SelectionChanged += new EventHandler(this.dgvLegislation_SelectionChanged);
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Image = (Image) Resources.Applay_var;
      this.btnOK.Location = new Point(1097, 564);
      this.btnOK.Margin = new Padding(4);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(64, 28);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "ОК";
      this.btnOK.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Image = (Image) Resources.delete_var;
      this.btnCancel.Location = new Point(1187, 564);
      this.btnCancel.Margin = new Padding(4);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(84, 28);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Отмена";
      this.btnCancel.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.btnCancel.UseVisualStyleBackColor = true;
      this.dataGridViewTextBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dataGridViewTextBoxColumn1.Frozen = true;
      this.dataGridViewTextBoxColumn1.HeaderText = "Наименование услуги";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.ReadOnly = true;
      this.dataGridViewTextBoxColumn1.Width = 178;
      this.dataGridViewTextBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn2.HeaderText = "Характеристики недопоставки";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn3.HeaderText = "Показатели качества услуг";
      this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
      this.dataGridViewTextBoxColumn4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn4.HeaderText = "Условия снижения";
      this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
      this.dataGridViewTextBoxColumn5.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn5.HeaderText = "Расчетная единица снижения";
      this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
      gridViewCellStyle1.Format = "N4";
      gridViewCellStyle1.NullValue = (object) null;
      this.dataGridViewTextBoxColumn6.DefaultCellStyle = gridViewCellStyle1;
      this.dataGridViewTextBoxColumn6.HeaderText = "Описание нормы";
      this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
      this.dataGridViewTextBoxColumn6.Width = 70;
      gridViewCellStyle2.Format = "N4";
      gridViewCellStyle2.NullValue = (object) null;
      this.dataGridViewTextBoxColumn7.DefaultCellStyle = gridViewCellStyle2;
      this.dataGridViewTextBoxColumn7.HeaderText = "Процент снятия";
      this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
      this.dataGridViewTextBoxColumn7.Width = 70;
      gridViewCellStyle3.Format = "N4";
      gridViewCellStyle3.NullValue = (object) null;
      this.dataGridViewTextBoxColumn8.DefaultCellStyle = gridViewCellStyle3;
      this.dataGridViewTextBoxColumn8.HeaderText = "Процент снятия";
      this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
      this.dataGridViewTextBoxColumn8.Width = 70;
      this.Service_name.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.Service_name.HeaderText = "Наименование услуги";
      this.Service_name.Name = "Service_name";
      this.Service_name.Width = 178;
      this.Id.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.Id.HeaderText = "Код основания";
      this.Id.Name = "Id";
      this.Id.ReadOnly = true;
      this.OverShoot.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.OverShoot.HeaderText = "Характеристики недопоставки";
      this.OverShoot.Name = "OverShoot";
      this.Normal.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.Normal.HeaderText = "Показатели качества услуг";
      this.Normal.Name = "Normal";
      this.Description.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.Description.HeaderText = "Условия снижения";
      this.Description.Name = "Description";
      this.Measure.HeaderText = "Расчетная единица снижения";
      this.Measure.Name = "Measure";
      this.Measure.Width = 120;
      gridViewCellStyle4.Format = "N4";
      gridViewCellStyle4.NullValue = (object) null;
      this.Standart.DefaultCellStyle = gridViewCellStyle4;
      this.Standart.HeaderText = "Описание нормы";
      this.Standart.Name = "Standart";
      this.Standart.Width = 70;
      gridViewCellStyle5.Format = "N4";
      gridViewCellStyle5.NullValue = (object) null;
      this.Percent.DefaultCellStyle = gridViewCellStyle5;
      this.Percent.HeaderText = "Процент снятия";
      this.Percent.Name = "Percent";
      this.Percent.Width = 70;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(1284, 597);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.dgvLegislation);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = "FrmLegislation";
      this.Text = "Основания для перерасчета за недопоставку";
      this.WindowState = FormWindowState.Maximized;
      this.Load += new EventHandler(this.FrmLegislation_Load);
      ((ISupportInitialize) this.dgvLegislation).EndInit();
      this.ResumeLayout(false);
    }
  }
}
