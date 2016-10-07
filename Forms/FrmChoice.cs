// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmChoice
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
  public class FrmChoice : Form
  {
    private IContainer components = (IContainer) null;
    private ISession session;
    private LsClient client;
    private Person person;
    private int num;
    private Button btnExit;
    private Panel pnBtn;
    private Button btnOk;
    private DataGridView dgvChoice;

    public FrmChoice()
    {
      this.InitializeComponent();
    }

    public FrmChoice(LsClient client, Person person, int num)
    {
      this.InitializeComponent();
      this.client = client;
      this.person = person;
      this.num = num;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void FrmChoice_Shown(object sender, EventArgs e)
    {
      this.session = Domain.CurrentSession;
      bool flag = false;
      if (!KvrplHelper.CheckProxy(48, 1, this.client.Company, false))
        flag = true;
      DataGridViewCellStyle gridViewCellStyle = new DataGridViewCellStyle();
      gridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
      switch (this.num)
      {
        case 1:
          this.dgvChoice.DataSource = (object) this.session.CreateCriteria(typeof (Owner)).Add((ICriterion) Restrictions.Eq("LsClient", (object) this.client)).Add((ICriterion) Restrictions.Not((ICriterion) Restrictions.Eq("Archive", (object) 3))).AddOrder(Order.Asc("Archive")).AddOrder(Order.Asc("Relation.RelationId")).AddOrder(Order.Asc("FirstPropDate")).List<Owner>();
          this.dgvChoice.Columns["Family"].HeaderText = "Фамилия";
          this.dgvChoice.Columns["Name"].HeaderText = "Имя";
          this.dgvChoice.Columns["LastName"].HeaderText = "Отчество";
          this.dgvChoice.Columns["FirstPropDate"].HeaderText = "Дата приобретения статуса";
          this.dgvChoice.Columns["FamilyNum"].Visible = false;
          this.dgvChoice.Columns["Note"].Visible = false;
          foreach (DataGridViewColumn column in (BaseCollection) this.dgvChoice.Columns)
          {
            column.Width = column.Index == 3 ? 100 : 150;
            column.HeaderCell.Style = gridViewCellStyle;
          }
          if (flag)
          {
            foreach (DataGridViewRow row in (IEnumerable) this.dgvChoice.Rows)
            {
              row.Cells["Family"].Value = (object) ((Owner) row.DataBoundItem).OwnerId;
              row.Cells["Name"].Value = (object) "***";
              row.Cells["LastName"].Value = (object) "***";
            }
          }
          else
          {
            foreach (DataGridViewRow row in (IEnumerable) this.dgvChoice.Rows)
            {
              Pers pers;
              try
              {
                pers = this.session.CreateQuery("select pe from Pers pe where pe.Id=" + (object) ((Owner) row.DataBoundItem).OwnerId + " and pe.Code=2 ").UniqueResult<Pers>();
              }
              catch (Exception ex)
              {
                pers = new Pers();
                pers.Family = "Не найдено";
                pers.Name = "Не найдено";
                pers.LastName = "Не найдено";
              }
              row.Cells["Family"].Value = (object) pers.Family;
              row.Cells["Name"].Value = (object) pers.Name;
              row.Cells["LastName"].Value = (object) pers.LastName;
            }
          }
          this.dgvChoice.ReadOnly = true;
          break;
        case 2:
          this.dgvChoice.DataSource = (object) this.session.CreateQuery(string.Format("select p from Person p,Registration r where p.LsClient.ClientId = {0} and p.Reg = r and (r.RegId = 1 or r.RegId =2) and p.FirstPropDate <= '{1}' and ((p.Archive=0 or p.Archive=5) or ((p.Archive=1 or p.Archive=2) and (p.OutToDate>='{1}' or p.OutToDate is null) and (p.DieDate>='{1}' or p.DieDate is null))) order by p.Archive,p.Relation.RelationId,p.FirstPropDate", (object) this.client.ClientId, (object) KvrplHelper.DateToBaseFormat(DateTime.Now))).List<Person>();
          this.dgvChoice.Columns["Family"].HeaderText = "Фамилия";
          this.dgvChoice.Columns["Name"].HeaderText = "Имя";
          this.dgvChoice.Columns["LastName"].HeaderText = "Отчество";
          this.dgvChoice.Columns["FirstPropDate"].HeaderText = "Дата приобретения статуса";
          foreach (DataGridViewColumn column in (BaseCollection) this.dgvChoice.Columns)
          {
            if (column.Index > 0 && column.Index <= 4)
            {
              column.Width = column.Index == 4 ? 100 : 150;
              column.HeaderCell.Style = gridViewCellStyle;
            }
            else
              column.Visible = false;
          }
          if (flag)
          {
            foreach (DataGridViewRow row in (IEnumerable) this.dgvChoice.Rows)
            {
              row.Cells["Family"].Value = (object) ((Person) row.DataBoundItem).PersonId;
              row.Cells["Name"].Value = (object) "***";
              row.Cells["LastName"].Value = (object) "***";
            }
          }
          else
          {
            foreach (DataGridViewRow row in (IEnumerable) this.dgvChoice.Rows)
            {
              Pers pers;
              try
              {
                pers = this.session.CreateQuery("select pe from Pers pe where pe.Id=" + (object) ((Person) row.DataBoundItem).PersonId + " and pe.Code=1 ").UniqueResult<Pers>();
              }
              catch (Exception ex)
              {
                pers = new Pers();
                pers.Family = "Не найдено";
                pers.Name = "Не найдено";
                pers.LastName = "Не найдено";
              }
              row.Cells["Family"].Value = (object) pers.Family;
              row.Cells["Name"].Value = (object) pers.Name;
              row.Cells["LastName"].Value = (object) pers.LastName;
            }
          }
          this.dgvChoice.ReadOnly = true;
          break;
        case 3:
          IList list = this.session.CreateQuery(string.Format("select ls.ClientId,cr.Service.Root,(select ServiceName from Service where ServiceId=cr.Service.Root) as serv,sum(cr.RentMain),cr.Month.PeriodId,cr.RentType,cr.Note,ls.Complex.IdFk,ls.Flat.NFlat,ls.Flat.IdFlat,ls.SurFlat from LsClient ls,CorrectRent cr where ls=cr.LsClient and ls.Home.IdHome = {0} and ls.Company.CompanyId={1} " + Options.MainConditions1 + " and cr.Period.PeriodId={2} group by ls.ClientId,cr.Month.PeriodId,cr.RentType,cr.Note,ls.Complex.IdFk,ls.Flat.NFlat,ls.Flat.IdFlat,ls.SurFlat,cr.Service.Root,cr.Month.PeriodId,cr.RentType order by ls.Complex.IdFk,DBA.LENGTHHOME(ls.Flat.NFlat),ls.Flat.IdFlat,dba.LengthHome(ls.SurFlat)", (object) this.client.Home.IdHome, (object) this.client.Company.CompanyId, (object) Options.Period.PeriodId)).List();
          this.dgvChoice.DataSource = (object) list;
          foreach (DataGridViewBand column in (BaseCollection) this.dgvChoice.Columns)
            column.Visible = false;
          KvrplHelper.AddTextBoxColumn(this.dgvChoice, 1, "Лицевой", "Client", 250, false);
          KvrplHelper.AddTextBoxColumn(this.dgvChoice, 2, "Услуга", "Service", 100, false);
          KvrplHelper.AddTextBoxColumn(this.dgvChoice, 3, "Сумма", "Rent", 100, false);
          foreach (DataGridViewRow row in (IEnumerable) this.dgvChoice.Rows)
          {
            if (((object[]) list[row.Index])[10] != null && ((object[]) list[row.Index])[10].ToString() != "0" && ((object[]) list[row.Index])[10].ToString() != "" && ((object[]) list[row.Index])[10].ToString() != " ")
              row.Cells["Client"].Value = (object) (((object[]) list[row.Index])[0].ToString() + "   кв." + ((object[]) list[row.Index])[8] + " комн." + ((object[]) list[row.Index])[10]);
            else
              row.Cells["Client"].Value = (object) (((object[]) list[row.Index])[0].ToString() + "   кв." + ((object[]) list[row.Index])[8]);
            row.Cells["Service"].Value = ((object[]) list[row.Index])[2];
            row.Cells["Rent"].Value = ((object[]) list[row.Index])[3];
          }
          this.dgvChoice.ReadOnly = true;
          break;
      }
      this.session.Clear();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (this.dgvChoice.Rows.Count > 0 && this.dgvChoice.CurrentRow != null)
      {
        this.session = Domain.CurrentSession;
        switch (this.num)
        {
          case 1:
            Owner dataBoundItem1 = (Owner) this.dgvChoice.CurrentRow.DataBoundItem;
            try
            {
              this.session.CreateSQLQuery(string.Format("update dba.form_a set owner={0} where idform={1}", (object) dataBoundItem1.OwnerId, (object) this.person.PersonId)).ExecuteUpdate();
              break;
            }
            catch (Exception ex)
            {
              int num = (int) MessageBox.Show("Невозможно присвоить статус", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              break;
            }
          case 2:
            Owner owner = new Owner();
            Person dataBoundItem2 = (Person) this.dgvChoice.CurrentRow.DataBoundItem;
            owner.BornDate = dataBoundItem2.BornDate;
            owner.Family = dataBoundItem2.Family;
            owner.FamilyNum = dataBoundItem2.FamilyNum;
            owner.FirstPropDate = dataBoundItem2.FirstPropDate;
            owner.LastName = dataBoundItem2.LastName;
            owner.LsClient = this.client;
            owner.Name = dataBoundItem2.Name;
            owner.Relation = dataBoundItem2.Relation;
            owner.Consent = dataBoundItem2.Consent;
            IList<DateTime> dateTimeList = this.session.CreateSQLQuery(string.Format("select DBA.form_reg({0},'{1}')", (object) this.client.ClientId, (object) KvrplHelper.DateToBaseFormat(dataBoundItem2.FirstPropDate.Value))).List<DateTime>();
            owner.RegDate = new DateTime?(dateTimeList[0]);
            owner.RegDEdit = new DateTime?(DateTime.Now);
            owner.UNameReg = Options.Login;
            owner.Note = " ";
            owner.UNameUnReg = " ";
            owner.Archive = 0;
            try
            {
              ITransaction transaction = this.session.BeginTransaction();
              this.session.Save((object) owner);
              this.session.Flush();
              IList<Owner> ownerList = this.session.CreateQuery(string.Format("from Owner o where o.OwnerId=(select max(own.OwnerId) from Owner own where own.LsClient.ClientId = {0})", (object) this.client.ClientId)).List<Owner>();
              if (ownerList.Count > 0)
                this.session.CreateSQLQuery(string.Format("update dba.form_a set owner={0} where idform={1}", (object) ownerList[0].OwnerId, (object) dataBoundItem2.PersonId)).ExecuteUpdate();
              transaction.Commit();
              break;
            }
            catch (Exception ex)
            {
              int num = (int) MessageBox.Show("Невозможно присвоить статус", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              break;
            }
          case 3:
            this.session.Clear();
            IList dataBoundItem3 = (IList) (object[]) this.dgvChoice.CurrentRow.DataBoundItem;
            IList<CorrectRent> correctRentList = (IList<CorrectRent>) new List<CorrectRent>();
            foreach (CorrectRent correctRent in (IEnumerable<CorrectRent>) this.session.CreateQuery(string.Format("from CorrectRent where Period.PeriodId={0} and LsClient.ClientId={1} and Service.ServiceId in (Select ServiceId from Service where Root={2})  and Month.PeriodId={3} and RentType={4} and Note='{5}'", (object) Options.Period.PeriodId, dataBoundItem3[0], dataBoundItem3[1], dataBoundItem3[4], dataBoundItem3[5], dataBoundItem3[6])).List<CorrectRent>())
              this.session.Save((object) new CorrectRent()
              {
                LsClient = this.client,
                Month = correctRent.Month,
                Note = correctRent.Note,
                Period = Options.Period,
                Service = correctRent.Service,
                Supplier = correctRent.Supplier,
                RentEO = correctRent.RentEO,
                RentMain = correctRent.RentMain,
                RentType = correctRent.RentType,
                RentVat = correctRent.RentVat,
                Volume = correctRent.Volume,
                UName = Options.Login,
                DEdit = new DateTime?(DateTime.Now)
              });
            this.session.Flush();
            break;
        }
      }
      this.session.Clear();
      this.Close();
    }

    private void dgvChoice_DataError(object sender, DataGridViewDataErrorEventArgs e)
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmChoice));
      this.pnBtn = new Panel();
      this.btnOk = new Button();
      this.btnExit = new Button();
      this.dgvChoice = new DataGridView();
      this.pnBtn.SuspendLayout();
      ((ISupportInitialize) this.dgvChoice).BeginInit();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnOk);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 168);
      this.pnBtn.Margin = new Padding(4);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(612, 40);
      this.pnBtn.TabIndex = 1;
      this.btnOk.Image = (Image) Resources.Tick;
      this.btnOk.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnOk.Location = new Point(12, 5);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(58, 30);
      this.btnOk.TabIndex = 1;
      this.btnOk.Text = "ОК";
      this.btnOk.TextAlign = ContentAlignment.MiddleRight;
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.delete;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(513, 5);
      this.btnExit.Margin = new Padding(4);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(86, 30);
      this.btnExit.TabIndex = 0;
      this.btnExit.Text = "Отмена";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.button1_Click);
      this.dgvChoice.BackgroundColor = Color.AliceBlue;
      this.dgvChoice.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvChoice.Dock = DockStyle.Fill;
      this.dgvChoice.Location = new Point(0, 0);
      this.dgvChoice.Name = "dgvChoice";
      this.dgvChoice.Size = new Size(612, 168);
      this.dgvChoice.TabIndex = 2;
      this.dgvChoice.DataError += new DataGridViewDataErrorEventHandler(this.dgvChoice_DataError);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(612, 208);
      this.Controls.Add((Control) this.dgvChoice);
      this.Controls.Add((Control) this.pnBtn);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmChoice";
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Выбор";
      this.Shown += new EventHandler(this.FrmChoice_Shown);
      this.pnBtn.ResumeLayout(false);
      ((ISupportInitialize) this.dgvChoice).EndInit();
      this.ResumeLayout(false);
    }
  }
}
