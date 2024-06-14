// Decompiled with JetBrains decompiler
// Type: BansheeGz.BGDatabase.GradientColorKey
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

using System.Runtime.CompilerServices;

#nullable enable
namespace BansheeGz.BGDatabase
{
  public struct GradientColorKey
  {
    public Color color;
    public float time;

    public GradientColorKey(Color color, float time)
    {
      this.color = color;
      this.time = time;
    }

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
      interpolatedStringHandler.AppendLiteral("(");
      interpolatedStringHandler.AppendFormatted(this.color.ToString());
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(__S.F(this.time));
      interpolatedStringHandler.AppendLiteral(")");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
