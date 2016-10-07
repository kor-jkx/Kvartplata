// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmTypeSeal
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Forms.Controls;
using Kvartplata.Properties;
using NHibernate;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmTypeSeal : Form
  {
    private FormStateSaver fss = new FormStateSaver(FrmTypeSeal.ic);
    private bool _readOnly = false;
    private IContainer components = (IContainer) null;
    private ISession session;
    private static IContainer ic;
    private Company _company;
    private Panel panel1;
    private UCTypeSeal ucTypeSeal1;
    private Button btnExit;
    public HelpProvider hp;

    public FrmTypeSeal(Company company)
    {
      this.InitializeComponent();
      this._company = company;
      this.CheckAccess();
      this.fss.ParentForm = (Form) this;
      this.session = Domain.CurrentSession;
      this.ucTypeSeal1.session = Domain.CurrentSession;
      this.ucTypeSeal1.tsbExit.Visible = false;
      this.ucTypeSeal1.LoadData();
      this.ucTypeSeal1.LoadSettings();
    }

    private void CheckAccess()
    {
      this._readOnly = KvrplHelper.AccessToReadOnly(KvrplHelper.CheckReadOnly(32, this._company, false));
      this.ucTypeSeal1.toolStrip1.Visible = this._readOnly;
      this.ucTypeSeal1.dgvBase.ReadOnly = !this._readOnly;
    }

    private void FrmTypeSeal_Load(object sender, EventArgs e)
    {
      if (this.session == null || !this.session.IsOpen)
        return;
      this.session.Clear();
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmTypeSeal));
      this.panel1 = new Panel();
      this.btnExit = new Button();
      this.ucTypeSeal1 = new UCTypeSeal();
      this.hp = new HelpProvider();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.btnExit);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 422);
      this.panel1.Margin = new Padding(4);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(430, 47);
      this.panel1.TabIndex = 0;
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(334, 9);
      this.btnExit.Margin = new Padding(4);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(83, 30);
      this.btnExit.TabIndex = 1;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.ucTypeSeal1.Dock = DockStyle.Fill;
      this.ucTypeSeal1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.ucTypeSeal1.Location = new Point(0, 0);
      this.ucTypeSeal1.Margin = new Padding(5);
      this.ucTypeSeal1.Name = "ucTypeSeal1";
      this.ucTypeSeal1.Size = new Size(430, 422);
      this.ucTypeSeal1.TabIndex = 2;
      this.hp.HelpNamespace = "Help.chm";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(430, 469);
      this.Controls.Add((Control) this.ucTypeSeal1);
      this.Controls.Add((Control) this.panel1);
      this.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.hp.SetHelpKeyword((Control) this, "kv517.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmTypeSeal";
      this.hp.SetShowHelp((Control) this, true);
      this.Text = "Типы пломб";
      this.Load += new EventHandler(this.FrmTypeSeal_Load);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
