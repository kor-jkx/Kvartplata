// Decompiled with JetBrains decompiler
// Type: Kvartplata.Forms.FrmAboutProgr
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Kvartplata.Forms
{
  public class FrmAboutProgr : FrmBaseForm1
  {
    private IContainer components = (IContainer) null;
    private const string REG_PRODUCT_KEY = "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows NT\\CurrentVersion";
    private Label lblName;
    private Label label3;
    private Label lblVersion;
    private Label label4;

    public FrmAboutProgr()
    {
      this.InitializeComponent();
    }

    private string GetOSVersion()
    {
      OperatingSystem osVersion = Environment.OSVersion;
      string str1 = "Не определено";
      if (osVersion.Platform == PlatformID.Win32NT)
      {
        switch (osVersion.Version.Major)
        {
          case 5:
            if (osVersion.Version.Minor == 0)
            {
              str1 = "Windows 2000";
              break;
            }
            if (osVersion.Version.Minor == 1)
            {
              str1 = "Windows XP";
              break;
            }
            if (osVersion.Version.Minor == 2)
            {
              str1 = "Windows Server 2003";
              break;
            }
            break;
          case 6:
            string str2 = Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows NT\\CurrentVersion", "ProductName", (object) "").ToString();
            if (str2.ToLower().Contains("vista"))
              str1 = "Windows Vista";
            if (str2.ToLower().Contains("server"))
            {
              str1 = "Windows 2008";
              break;
            }
            break;
        }
      }
      return str1;
    }

    private void FrmAboutProgr_Shown(object sender, EventArgs e)
    {
      this.lblVersion.Text = AssemblyName.GetAssemblyName(Assembly.Load("Kvartplata").Location).Version.ToString();
    }

    private void FrmAboutProgr_Load(object sender, EventArgs e)
    {
      if (Options.Kvartplata)
        return;
      this.lblName.Text = "Начисление и сбор платежей за \n коммунальные услуги арендаторам";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmAboutProgr));
      this.lblName = new Label();
      this.label3 = new Label();
      this.lblVersion = new Label();
      this.label4 = new Label();
      this.SuspendLayout();
      this.lblName.AllowDrop = true;
      this.lblName.AutoEllipsis = true;
      this.lblName.AutoSize = true;
      this.lblName.BackColor = SystemColors.Control;
      this.lblName.FlatStyle = FlatStyle.Popup;
      this.lblName.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.lblName.ForeColor = System.Drawing.Color.Black;
      this.lblName.Location = new Point(104, 19);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(282, 38);
      this.lblName.TabIndex = 2;
      this.lblName.Text = "\"Расчет жилищно-коммунальных \r\nплатежей - \"Квартплата\"";
      this.lblName.TextAlign = ContentAlignment.MiddleCenter;
      this.label3.Location = new Point(12, 118);
      this.label3.Name = "label3";
      this.label3.Size = new Size(427, 56);
      this.label3.TabIndex = 3;
      this.label3.Text = "Техническая поддержка:\r\nООО \"БИТ\"\r\nтел. (4852)40-43-64, 40-43-65, 40-43-76, 72-80-76, 40-43-70\r\n";
      this.lblVersion.AutoSize = true;
      this.lblVersion.Location = new Point(76, 90);
      this.lblVersion.Name = "lblVersion";
      this.lblVersion.Size = new Size(45, 16);
      this.lblVersion.TabIndex = 4;
      this.lblVersion.Text = "label4";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(12, 90);
      this.label4.Name = "label4";
      this.label4.Size = new Size(58, 16);
      this.label4.TabIndex = 5;
      this.label4.Text = "Версия:";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(477, 220);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.lblVersion);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.lblName);
      this.FormBorderStyle = FormBorderStyle.Fixed3D;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = "FrmAboutProgr";
      this.SizeGripStyle = SizeGripStyle.Show;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "О программе";
      this.Load += new EventHandler(this.FrmAboutProgr_Load);
      this.Shown += new EventHandler(this.FrmAboutProgr_Shown);
      this.Controls.SetChildIndex((Control) this.lblName, 0);
      this.Controls.SetChildIndex((Control) this.label3, 0);
      this.Controls.SetChildIndex((Control) this.lblVersion, 0);
      this.Controls.SetChildIndex((Control) this.label4, 0);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
