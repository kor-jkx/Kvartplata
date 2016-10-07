// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.InputBox
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kvartplata.Classes
{
  public class InputBox : Form
  {
    private Label label;
    private TextBox textValue;
    private Button buttonOK;
    private Button buttonCancel;

    private InputBox(string Caption, string Text)
    {
      this.label = new Label();
      this.textValue = new TextBox();
      this.buttonOK = new Button();
      this.buttonCancel = new Button();
      this.SuspendLayout();
      this.label.AutoSize = true;
      this.label.Location = new Point(9, 13);
      this.label.Name = "label";
      this.label.Size = new Size(31, 13);
      this.label.TabIndex = 1;
      this.label.Text = Text;
      this.label.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.textValue.Location = new Point(12, 31);
      this.textValue.Name = "textValue";
      this.textValue.Size = new Size(245, 20);
      this.textValue.TabIndex = 2;
      this.textValue.WordWrap = false;
      this.textValue.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.buttonOK.DialogResult = DialogResult.OK;
      this.buttonOK.Location = new Point(12, 67);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new Size(100, 30);
      this.buttonOK.TabIndex = 3;
      this.buttonOK.Text = "Принять";
      this.buttonOK.Image = (Image) Resources.Tick;
      this.buttonOK.ImageAlign = ContentAlignment.MiddleLeft;
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.TextAlign = ContentAlignment.MiddleRight;
      this.buttonOK.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.buttonCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonCancel.Image = (Image) Resources.Exit;
      this.buttonCancel.ImageAlign = ContentAlignment.MiddleLeft;
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.Location = new Point(157, 67);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(100, 30);
      this.buttonCancel.TabIndex = 4;
      this.buttonCancel.Text = "Выход";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.TextAlign = ContentAlignment.MiddleRight;
      this.buttonCancel.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.AcceptButton = (IButtonControl) this.buttonOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.buttonCancel;
      this.ClientSize = new Size(270, 103);
      this.Controls.Add((Control) this.buttonCancel);
      this.Controls.Add((Control) this.buttonOK);
      this.Controls.Add((Control) this.textValue);
      this.Controls.Add((Control) this.label);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "InputBox";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = Caption;
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public static bool Query(string Caption, string Text, ref string s_val)
    {
      InputBox inputBox = new InputBox(Caption, Text);
      inputBox.textValue.Text = s_val;
      if (inputBox.ShowDialog() != DialogResult.OK)
        return false;
      s_val = inputBox.textValue.Text;
      return true;
    }

    public static bool Query(string Caption, string Text, ref DateTime s_val)
    {
      InputBox inputBox = new InputBox(Caption, Text);
      inputBox.textValue.Text = s_val.ToShortDateString();
      bool flag = false;
      while (inputBox.ShowDialog() == DialogResult.OK)
      {
        try
        {
          s_val = Convert.ToDateTime(inputBox.textValue.Text);
          flag = true;
        }
        catch
        {
        }
        if (flag)
          return true;
      }
      return false;
    }

    public static bool InputValue(string Caption, string Text, string prefix, string format, ref string value, int min, int max)
    {
      string s_val = value;
      bool flag;
      do
      {
        flag = true;
        if (!InputBox.Query(Caption, Text, ref s_val))
          return false;
      }
      while (!flag);
      value = s_val;
      return true;
    }

    public static bool InputValue(string Caption, string Text, string prefix, string format, ref string value)
    {
      string str1 = value;
      string s_val = prefix + value;
      bool flag;
      do
      {
        flag = true;
        if (!InputBox.Query(Caption, Text, ref s_val))
          return false;
        try
        {
          string str2 = s_val.Trim();
          str1 = str2.Length <= 0 || (int) str2[0] != 35 ? (str2.Length <= 1 || ((int) str2[1] != 120 || (int) str2[0] != 48) ? str2 : str2.Remove(0, 2)) : str2.Remove(0, 1);
        }
        catch
        {
          int num = (int) MessageBox.Show("Данные некорректны!");
          flag = false;
        }
      }
      while (!flag);
      value = str1;
      return true;
    }
  }
}
