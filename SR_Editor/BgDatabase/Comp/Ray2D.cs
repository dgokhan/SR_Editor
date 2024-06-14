// Decompiled with JetBrains decompiler
// Type: BansheeGz.BGDatabase.Ray2D
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

using System.Runtime.CompilerServices;

#nullable enable
namespace BansheeGz.BGDatabase
{
  public struct Ray2D
  {
    public Vector2 origin;
    public Vector2 direction;

    public Ray2D(Vector2 origin, Vector2 direction)
    {
      this.origin = origin;
      this.direction = direction;
    }

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
      interpolatedStringHandler.AppendLiteral("(");
      interpolatedStringHandler.AppendFormatted<Vector2>(this.origin);
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted<Vector2>(this.direction);
      interpolatedStringHandler.AppendLiteral(")");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
