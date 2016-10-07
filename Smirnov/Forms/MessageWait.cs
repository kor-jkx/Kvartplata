// Decompiled with JetBrains decompiler
// Type: Kvartplata.Smirnov.Forms.MessageWait
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Smirnov.Forms
{
  public class MessageWait : Form
  {
    private IContainer components = (IContainer) null;
    private RichTextBox rtbMes;

    public MessageWait()
    {
      this.InitializeComponent();
    }

    public MessageWait(string mes)
    {
      this.InitializeComponent();
      this.rtbMes.Text = mes;
      this.rtbMes.SelectAll();
      this.rtbMes.SelectionAlignment = HorizontalAlignment.Center;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MessageWait));
      this.rtbMes = new RichTextBox();
      this.SuspendLayout();
      this.rtbMes.BackColor = Color.AliceBlue;
      this.rtbMes.BorderStyle = BorderStyle.None;
      this.rtbMes.Cursor = Cursors.Default;
      this.rtbMes.Dock = DockStyle.Fill;
      this.rtbMes.Enabled = false;
      this.rtbMes.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.rtbMes.Location = new Point(0, 0);
      this.rtbMes.Name = "rtbMes";
      this.rtbMes.ReadOnly = true;
      this.rtbMes.Size = new Size(677, 150);
      this.rtbMes.TabIndex = 0;
      this.rtbMes.Text = "Ожидайте";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(677, 150);
      this.Controls.Add((Control) this.rtbMes);
      this.Cursor = Cursors.Default;
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = "MessageWait";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Сообщение";
      this.ResumeLayout(false);
    }
  }
}
