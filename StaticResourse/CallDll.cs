// Decompiled with JetBrains decompiler
// Type: Kvartplata.StaticResourse.CallDll
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System.Runtime.InteropServices;

namespace Kvartplata.StaticResourse
{
  public static class CallDll
  {
    [DllImport("Reports.dll", EntryPoint = "QRep", CharSet = CharSet.Ansi, SetLastError = true)]
    public static extern void Rep(int idcity, int Fond, int Rnn, int Codeu, int Home, int Lic, int idreport, int PeriodId, int admin, string Connect, string UserS, string Passwd);

    [DllImport("Receipts.dll", CharSet = CharSet.Ansi, SetLastError = true)]
    public static extern void WReceipt(int Level, int Raion, int Company, int City, int Home, int Lic, double GlMec, string connect, string UserS, string Passwd, int Compl);

    [DllImport("Compute.dll", EntryPoint = "QRent", CharSet = CharSet.Ansi, SetLastError = true)]
    public static extern void Rent(int period, string sclient, string connect, string UserS, string Passwd);

    [DllImport("View.dll", EntryPoint = "QView", CharSet = CharSet.Ansi, SetLastError = true)]
    public static extern void View(int idlic, double period);

    [DllImport("Soc.dll", EntryPoint = "QSocNew", CharSet = CharSet.Ansi, SetLastError = true)]
    public static extern void Soc(int idcity, int Rnn, double GlMec, string Connect, string UserS, string Passwd);
  }
}
