// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.Controls.UCRegion
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms.Controls
{
  public class UCRegion : UserControl
  {
    private IContainer components = (IContainer) null;
    private ISession session;
    private ICriteria cCity;
    private ICriteria cRnn;
    private int type;
    private Label lblObl;
    private Label lblCity;
    private Label lblRnn;
    private ComboBox cmbObl;
    private ComboBox cmbRnn;
    private ComboBox cmbCity;
    private Label lblStreet;
    private ComboBox cmbStreet;

    public UCRegion()
    {
      this.InitializeComponent();
    }

    public UCRegion(int type)
    {
      this.InitializeComponent();
      this.type = type;
    }

    public void LoadSoato()
    {
      this.session = Domain.CurrentSession;
      IList<Reg> regList1 = this.session.CreateCriteria(typeof (Reg)).Add((ICriterion) Restrictions.Eq("Level", (object) 1)).Add((ICriterion) Restrictions.Eq("PrinRegion", (object) -1)).AddOrder(Order.Asc("RegionName")).List<Reg>();
      regList1.Insert(0, new Reg(0, ""));
      this.cmbObl.DataSource = (object) regList1;
      this.cmbObl.ValueMember = "RegionId";
      this.cmbObl.DisplayMember = "RegionName";
      this.cRnn = this.session.CreateCriteria(typeof (Reg)).Add((ICriterion) Restrictions.Eq("Level", (object) 2)).Add((ICriterion) Restrictions.Eq("PrinRegion", (object) -1)).AddOrder(Order.Asc("RegionName"));
      IList<Reg> regList2 = this.cRnn.List<Reg>();
      regList2.Insert(0, new Reg(0, ""));
      this.cmbRnn.DataSource = (object) regList2;
      this.cmbRnn.ValueMember = "RegionId";
      this.cmbRnn.DisplayMember = "RegionName";
      this.cCity = this.session.CreateCriteria(typeof (Reg)).Add((ICriterion) Restrictions.Eq("Level", (object) 3)).Add((ICriterion) Restrictions.Eq("PrinRegion", (object) -1)).AddOrder(Order.Asc("RegionName"));
      IList<Reg> regList3 = this.cCity.List<Reg>();
      regList3.Insert(0, new Reg(0, ""));
      this.cmbCity.DataSource = (object) regList3;
      this.cmbCity.ValueMember = "RegionId";
      this.cmbCity.DisplayMember = "RegionName";
      if (this.type == 1)
      {
        this.lblStreet.Visible = false;
        this.cmbStreet.Visible = false;
      }
      this.session.Clear();
    }

    private void UCSoato_Load(object sender, EventArgs e)
    {
    }

    private void cmbObl_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.cmbRnn.SelectedValue = (object) 0;
      this.cmbCity.SelectedValue = (object) 0;
      this.session = Domain.CurrentSession;
      IList<Reg> regList1 = (IList<Reg>) new List<Reg>();
      IList<Reg> regList2 = (IList<Reg>) new List<Reg>();
      if (this.cmbObl.SelectedIndex == 0 || this.cmbObl.SelectedIndex == -1)
      {
        IList<Reg> regList3 = this.cCity.List<Reg>();
        regList3.Insert(0, new Reg(0, ""));
        this.cmbCity.DataSource = (object) regList3;
        IList<Reg> regList4 = this.cRnn.List<Reg>();
        regList4.Insert(0, new Reg(0, ""));
        this.cmbRnn.DataSource = (object) regList4;
      }
      else
      {
        IList<Reg> regList3 = this.session.CreateCriteria(typeof (Reg)).Add((ICriterion) Restrictions.Eq("Level", (object) 3)).Add((ICriterion) Restrictions.Eq("PrinRegion", (object) ((Reg) this.cmbObl.SelectedItem).RegionId)).AddOrder(Order.Asc("RegionName")).List<Reg>();
        regList3.Insert(0, new Reg(0, ""));
        this.cmbCity.DataSource = (object) regList3;
        IList<Reg> regList4 = this.session.CreateCriteria(typeof (Reg)).Add((ICriterion) Restrictions.Eq("Level", (object) 2)).Add((ICriterion) Restrictions.Eq("PrinRegion", (object) ((Reg) this.cmbObl.SelectedItem).RegionId)).AddOrder(Order.Asc("RegionName")).List<Reg>();
        regList4.Insert(0, new Reg(0, ""));
        this.cmbRnn.DataSource = (object) regList4;
      }
      this.session.Clear();
    }

    private void cmbRnn_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.cmbCity.SelectedValue = (object) 0;
      this.session = Domain.CurrentSession;
      IList<Reg> regList1 = (IList<Reg>) new List<Reg>();
      if (this.cmbRnn.SelectedIndex == 0 || this.cmbRnn.SelectedIndex == -1)
      {
        if (this.cmbObl.SelectedIndex == 0 || this.cmbObl.SelectedIndex == -1)
        {
          IList<Reg> regList2 = this.cCity.List<Reg>();
          regList2.Insert(0, new Reg(0, ""));
          this.cmbCity.DataSource = (object) regList2;
        }
        else
        {
          IList<Reg> regList2 = this.session.CreateCriteria(typeof (Reg)).Add((ICriterion) Restrictions.Eq("Level", (object) 3)).Add((ICriterion) Restrictions.Eq("PrinRegion", (object) ((Reg) this.cmbObl.SelectedItem).RegionId)).AddOrder(Order.Asc("RegionName")).List<Reg>();
          regList2.Insert(0, new Reg(0, ""));
          this.cmbCity.DataSource = (object) regList2;
        }
      }
      else
      {
        IList<Reg> regList2 = this.session.CreateCriteria(typeof (Reg)).Add((ICriterion) Restrictions.Eq("Level", (object) 3)).Add((ICriterion) Restrictions.Eq("PrinRegion", (object) ((Reg) this.cmbRnn.SelectedItem).RegionId)).AddOrder(Order.Asc("RegionName")).List<Reg>();
        regList2.Insert(0, new Reg(0, ""));
        this.cmbCity.DataSource = (object) regList2;
      }
      this.session.Clear();
    }

    public Reg ReturnRegion()
    {
      if ((uint) this.cmbCity.SelectedIndex > 0U)
        return (Reg) this.cmbCity.SelectedItem;
      if ((uint) this.cmbRnn.SelectedIndex > 0U)
        return (Reg) this.cmbRnn.SelectedItem;
      if ((uint) this.cmbObl.SelectedIndex > 0U)
        return (Reg) this.cmbObl.SelectedItem;
      return (Reg) null;
    }

    public int ReturnStreet()
    {
      if (this.cmbStreet.SelectedIndex != 0 && this.cmbStreet.SelectedIndex != -1)
        return ((StreetRu) this.cmbStreet.SelectedItem).NumStr;
      return 0;
    }

    public void Clear()
    {
      this.cmbObl.SelectedValue = (object) 0;
      this.cmbRnn.SelectedValue = (object) 0;
      this.cmbCity.SelectedValue = (object) 0;
      this.cmbStreet.SelectedValue = (object) 0;
    }

    public void Fix(int indexObl, int indexRnn, int indexCity, int indexStr)
    {
      this.cmbObl.SelectedValue = (object) indexObl;
      this.cmbObl_SelectionChangeCommitted((object) this, (EventArgs) null);
      this.cmbRnn.SelectedValue = (object) indexRnn;
      this.cmbRnn_SelectionChangeCommitted((object) this, (EventArgs) null);
      this.cmbCity.SelectedValue = (object) indexCity;
      this.cmbCity_SelectionChangeCommitted((object) this, (EventArgs) null);
      this.cmbStreet.SelectedValue = (object) indexStr;
    }

    private void cmbCity_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.cmbStreet.SelectedValue = (object) 0;
      this.session = Domain.CurrentSession;
      IList<StreetRu> streetRuList1 = (IList<StreetRu>) new List<StreetRu>();
      if (this.cmbCity.SelectedIndex != 0 && this.cmbCity.SelectedIndex != -1)
      {
        IList<StreetRu> streetRuList2 = this.session.CreateQuery(string.Format("select s from StreetRu s where PrinNumStr={0} order by StrName", (object) ((Reg) this.cmbCity.SelectedItem).RegionId)).List<StreetRu>();
        streetRuList2.Insert(0, new StreetRu("0", ""));
        this.cmbStreet.DataSource = (object) streetRuList2;
        this.cmbStreet.ValueMember = "NumStr";
        this.cmbStreet.DisplayMember = "FullName";
      }
      else
        this.cmbStreet.DataSource = (object) null;
      this.session.Clear();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblObl = new Label();
      this.lblCity = new Label();
      this.lblRnn = new Label();
      this.cmbObl = new ComboBox();
      this.cmbRnn = new ComboBox();
      this.cmbCity = new ComboBox();
      this.lblStreet = new Label();
      this.cmbStreet = new ComboBox();
      this.SuspendLayout();
      this.lblObl.AutoSize = true;
      this.lblObl.Location = new Point(4, 11);
      this.lblObl.Margin = new Padding(4, 0, 4, 0);
      this.lblObl.Name = "lblObl";
      this.lblObl.Size = new Size(63, 16);
      this.lblObl.TabIndex = 0;
      this.lblObl.Text = "Область";
      this.lblCity.AutoSize = true;
      this.lblCity.Location = new Point(4, 44);
      this.lblCity.Name = "lblCity";
      this.lblCity.Size = new Size(80, 16);
      this.lblCity.TabIndex = 1;
      this.lblCity.Text = "Город (н.п.)";
      this.lblRnn.AutoSize = true;
      this.lblRnn.Location = new Point(305, 11);
      this.lblRnn.Name = "lblRnn";
      this.lblRnn.Size = new Size(49, 16);
      this.lblRnn.TabIndex = 2;
      this.lblRnn.Text = "Район";
      this.cmbObl.FormattingEnabled = true;
      this.cmbObl.Location = new Point(84, 8);
      this.cmbObl.Name = "cmbObl";
      this.cmbObl.Size = new Size(215, 24);
      this.cmbObl.TabIndex = 3;
      this.cmbObl.SelectionChangeCommitted += new EventHandler(this.cmbObl_SelectionChangeCommitted);
      this.cmbRnn.FormattingEnabled = true;
      this.cmbRnn.Location = new Point(360, 8);
      this.cmbRnn.Name = "cmbRnn";
      this.cmbRnn.Size = new Size(253, 24);
      this.cmbRnn.TabIndex = 4;
      this.cmbRnn.SelectionChangeCommitted += new EventHandler(this.cmbRnn_SelectionChangeCommitted);
      this.cmbCity.FormattingEnabled = true;
      this.cmbCity.Location = new Point(84, 41);
      this.cmbCity.Name = "cmbCity";
      this.cmbCity.Size = new Size(215, 24);
      this.cmbCity.TabIndex = 5;
      this.cmbCity.SelectionChangeCommitted += new EventHandler(this.cmbCity_SelectionChangeCommitted);
      this.lblStreet.AutoSize = true;
      this.lblStreet.Location = new Point(305, 44);
      this.lblStreet.Name = "lblStreet";
      this.lblStreet.Size = new Size(49, 16);
      this.lblStreet.TabIndex = 6;
      this.lblStreet.Text = "Улица";
      this.cmbStreet.FormattingEnabled = true;
      this.cmbStreet.Location = new Point(360, 38);
      this.cmbStreet.Name = "cmbStreet";
      this.cmbStreet.Size = new Size(253, 24);
      this.cmbStreet.TabIndex = 7;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.cmbStreet);
      this.Controls.Add((Control) this.lblStreet);
      this.Controls.Add((Control) this.cmbCity);
      this.Controls.Add((Control) this.cmbRnn);
      this.Controls.Add((Control) this.cmbObl);
      this.Controls.Add((Control) this.lblRnn);
      this.Controls.Add((Control) this.lblCity);
      this.Controls.Add((Control) this.lblObl);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.Margin = new Padding(4);
      this.Name = "UCRegion";
      this.Size = new Size(637, 70);
      this.Load += new EventHandler(this.UCSoato_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
