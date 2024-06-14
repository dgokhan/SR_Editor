// Decompiled with JetBrains decompiler
// Type: BansheeGz.BGDatabase.Bounds
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

using System.Runtime.CompilerServices;

#nullable enable
namespace BansheeGz.BGDatabase
{
  public struct Bounds
  {
    public Vector3 center;
    public Vector3 extents;

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 4);
      interpolatedStringHandler.AppendLiteral("(");
      interpolatedStringHandler.AppendFormatted(__S.F(this.center.x));
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(__S.F(this.center.y));
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(__S.F(this.extents.x));
      interpolatedStringHandler.AppendLiteral("), ");
      interpolatedStringHandler.AppendFormatted(__S.F(this.extents.y));
      interpolatedStringHandler.AppendLiteral(")");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
