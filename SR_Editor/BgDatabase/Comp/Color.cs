// Decompiled with JetBrains decompiler
// Type: BansheeGz.BGDatabase.Color
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

using System.Runtime.CompilerServices;

#nullable enable
namespace BansheeGz.BGDatabase
{
  public struct Color
  {
    public float r;
    public float g;
    public float b;
    public float a;
    public static Color black = new Color(0.0f, 0.0f, 0.0f, 1f);
    public static Color clear;
    public static Color32 white = new Color32(1f, 1f, 1f, 1f);

    public Color(float r, float g, float b, float a)
    {
      this.r = r;
      this.g = g;
      this.b = b;
      this.a = a;
    }

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 4);
      interpolatedStringHandler.AppendLiteral("(");
      interpolatedStringHandler.AppendFormatted(__S.F(this.r));
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(__S.F(this.g));
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(__S.F(this.b));
      interpolatedStringHandler.AppendLiteral("), ");
      interpolatedStringHandler.AppendFormatted(__S.F(this.a));
      interpolatedStringHandler.AppendLiteral(")");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
