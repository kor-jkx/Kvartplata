// Decompiled with JetBrains decompiler
// Type: <PrivateImplementationDetails>
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[CompilerGenerated]
internal sealed class PrivateImplementationDetails
{
/*internal static readonly \u003CPrivateImplementationDetails\u003E.__StaticArrayInitTypeSize\u003D12 \u00316F4A80AE1E8A3D99BF7DB5B559DFFCE9B4BEB74;
  internal static readonly int \u0032C61413D5A141995BD655C08B27FA9332BC99293;
  internal static readonly \u003CPrivateImplementationDetails\u003E.__StaticArrayInitTypeSize\u003D36 \u0035E8CBE2DA60543DACDE6CCBD0B445FB3E5FD50B4;
  internal static readonly \u003CPrivateImplementationDetails\u003E.__StaticArrayInitTypeSize\u003D12 \u0038D00B2A3DF0C96F058EBB780CEF9CD8B3C51EB18;
  internal static readonly \u003CPrivateImplementationDetails\u003E.__StaticArrayInitTypeSize\u003D6 C080795F75FE6F121824EC23E61CBE8758A813C3;
*/
  internal static uint ComputeStringHash(string s)
  {
    uint num = 0;
    if (s != null)
    {
      num = 2166136261U;
      for (int index = 0; index < s.Length; ++index)
        num = (uint) (((int) s[index] ^ (int) num) * 16777619);
    }
    return num;
  }
/*
  [StructLayout(LayoutKind.Explicit, Size = 6, Pack = 1)]
  private struct __StaticArrayInitTypeSize\u003D6
  {
  }

  [StructLayout(LayoutKind.Explicit, Size = 12, Pack = 1)]
  private struct __StaticArrayInitTypeSize\u003D12
  {
  }

  [StructLayout(LayoutKind.Explicit, Size = 36, Pack = 1)]
  private struct __StaticArrayInitTypeSize\u003D36
  {
  }
*/
}
