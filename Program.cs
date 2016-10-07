// Decompiled with JetBrains decompiler
// Type: Kvartplata.Program
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Kvartplata.Classes;
using Kvartplata.Forms;
using NLog;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Kvartplata
{
  internal static class Program
  {
    private static Logger log = LogManager.GetCurrentClassLogger();

    [STAThread]
    private static void Main()
    {
      Program.log.Info("Запуск программы");
      Application.ThreadException += new ThreadExceptionEventHandler(Program.Form1_UIThreadException);
      Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
      AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.CurrentDomain_UnhandledException);
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new MainForm());
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      StreamWriter streamWriter = new StreamWriter((Stream) File.Open(Options.PathProfile + "\\Kvartplata_error.log", FileMode.Append, FileAccess.Write), Encoding.Default);
      streamWriter.WriteLine("\n" + DateTime.Now.ToString() + ": Возникла ошибка:\n\n" + e.ToString() + "\n\n" + (object) e.GetType());
      streamWriter.Close();
      Options.Error = true;
    }

    private static void Form1_UIThreadException(object sender, ThreadExceptionEventArgs t)
    {
      string str1 = "";
      string str2 = "";
      string executablePath = Application.ExecutablePath;
      string str3 = executablePath.Remove(0, executablePath.LastIndexOf("\\") + 1);
      str2 = str3.Remove(str3.LastIndexOf("."), str3.Length - str3.LastIndexOf("."));
      str1 = executablePath.Remove(executablePath.LastIndexOf("\\"), executablePath.Length - executablePath.LastIndexOf("\\"));
      StreamWriter streamWriter = new StreamWriter((Stream) File.Open(Options.PathProfile + "\\Kvartplata_error.log", FileMode.Append, FileAccess.Write), Encoding.Default);
      streamWriter.WriteLine("\n" + DateTime.Now.ToString() + ": Возникла ошибка:\n\n" + t.Exception.Message + "\n\n" + (object) t.Exception.GetType());
      streamWriter.Close();
      Options.Error = true;
    }
  }
}
