// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmStrSearch
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Properties;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmStrSearch : Form
  {
    private IContainer components = (IContainer) null;
    public string strName;
    public string homeName;
    private DataGridView dgvMainList;
    private Panel pnBtn;
    private Button btnSearch;
    private Button btnExit;
    private Label lblStr;
    private Label lblHome;
    private TextBox txtStr;
    private TextBox txtHome;
    private HelpProvider hp;

    public FrmStrSearch()
    {
      this.InitializeComponent();
    }

    public FrmStrSearch(DataGridView dataGridView)
    {
      this.InitializeComponent();
      this.dgvMainList = dataGridView;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void txtHome_TextChanged(object sender, EventArgs e)
    {
      int index = 0;
      foreach (DataGridViewRow row in (IEnumerable) this.dgvMainList.Rows)
      {
        if (this.txtStr.Text.Length > 0)
        {
          string str = this.txtStr.Text.Substring(0, 1).ToUpper() + this.txtStr.Text.Substring(1).ToLower();
          if (row.Cells["NameStr"].Value.ToString().IndexOf(str) != -1)
          {
            if (this.txtHome.Text.Length > 0 && row.Cells["NHome"].Value.ToString().IndexOf(this.txtHome.Text) != -1 || this.txtHome.Text.Length == 0)
            {
              this.dgvMainList.CurrentCell = row.Cells["NameStr"];
              return;
            }
            if (index == 0)
              index = this.dgvMainList.CurrentRow.Index;
          }
        }
        else if (row.Cells["NHome"].Value.ToString().IndexOf(this.txtHome.Text) != -1)
        {
          this.dgvMainList.CurrentCell = row.Cells["NameStr"];
          return;
        }
      }
      this.dgvMainList.CurrentCell = this.dgvMainList.Rows[index].Cells["NameStr"];
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
      this.txtHome_TextChanged(sender, e);
      this.Close();
    }

    private void txtStr_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.Close();
    }

    private void txtHome_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmStrSearch));
      this.pnBtn = new Panel();
      this.btnSearch = new Button();
      this.btnExit = new Button();
      this.lblStr = new Label();
      this.lblHome = new Label();
      this.txtStr = new TextBox();
      this.txtHome = new TextBox();
      this.hp = new HelpProvider();
      this.pnBtn.SuspendLayout();
      this.SuspendLayout();
      this.pnBtn.Controls.Add((Control) this.btnSearch);
      this.pnBtn.Controls.Add((Control) this.btnExit);
      this.pnBtn.Dock = DockStyle.Bottom;
      this.pnBtn.Location = new Point(0, 79);
      this.pnBtn.Margin = new Padding(4);
      this.pnBtn.Name = "pnBtn";
      this.pnBtn.Size = new Size(295, 40);
      this.pnBtn.TabIndex = 2;
      this.btnSearch.Image = (Image) Resources.search_24;
      this.btnSearch.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSearch.Location = new Point(12, 5);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new Size(82, 30);
      this.btnSearch.TabIndex = 0;
      this.btnSearch.Text = "Поиск";
      this.btnSearch.TextAlign = ContentAlignment.MiddleRight;
      this.btnSearch.UseVisualStyleBackColor = true;
      this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
      this.btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExit.DialogResult = DialogResult.Cancel;
      this.btnExit.Image = (Image) Resources.Exit;
      this.btnExit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnExit.Location = new Point(208, 5);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(78, 30);
      this.btnExit.TabIndex = 1;
      this.btnExit.Text = "Выход";
      this.btnExit.TextAlign = ContentAlignment.MiddleRight;
      this.btnExit.UseVisualStyleBackColor = true;
      this.btnExit.Click += new EventHandler(this.button1_Click);
      this.lblStr.AutoSize = true;
      this.lblStr.Location = new Point(9, 9);
      this.lblStr.Name = "lblStr";
      this.lblStr.Size = new Size(49, 16);
      this.lblStr.TabIndex = 1;
      this.lblStr.Text = "Улица";
      this.lblHome.AutoSize = true;
      this.lblHome.Location = new Point(12, 41);
      this.lblHome.Name = "lblHome";
      this.lblHome.Size = new Size(34, 16);
      this.lblHome.TabIndex = 2;
      this.lblHome.Text = "Дом";
      this.txtStr.Location = new Point(64, 6);
      this.txtStr.Name = "txtStr";
      this.txtStr.Size = new Size(169, 22);
      this.txtStr.TabIndex = 0;
      this.txtStr.TextChanged += new EventHandler(this.txtHome_TextChanged);
      this.txtStr.KeyUp += new KeyEventHandler(this.txtStr_KeyUp);
      this.txtHome.Location = new Point(64, 38);
      this.txtHome.Name = "txtHome";
      this.txtHome.Size = new Size(169, 22);
      this.txtHome.TabIndex = 1;
      this.txtHome.TextChanged += new EventHandler(this.txtHome_TextChanged);
      this.txtHome.KeyUp += new KeyEventHandler(this.txtHome_KeyUp);
      this.hp.HelpNamespace = "Help.chm";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnExit;
      this.ClientSize = new Size(295, 119);
      this.Controls.Add((Control) this.txtHome);
      this.Controls.Add((Control) this.txtStr);
      this.Controls.Add((Control) this.lblHome);
      this.Controls.Add((Control) this.lblStr);
      this.Controls.Add((Control) this.pnBtn);
      this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.hp.SetHelpKeyword((Control) this, "kv114.html");
      this.hp.SetHelpNavigator((Control) this, HelpNavigator.Topic);
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4);
      this.Name = "FrmStrSearch";
      this.hp.SetShowHelp((Control) this, true);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Поиск улицы и дома";
      this.pnBtn.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
