// Decompiled with JetBrains decompiler
// Type: BansheeGz.BGDatabase.Vector2
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

using System.Runtime.CompilerServices;

#nullable enable
namespace BansheeGz.BGDatabase
{
  public struct Vector2
  {
    public float x;
    public float y;
    public static Vector2 zero;

    public Vector2(float x, float y)
    {
      this.x = x;
      this.y = y;
    }

    public static bool operator ==(Vector2 lhs, Vector2 rhs)
    {
      float num1 = lhs.x - rhs.x;
      float num2 = lhs.y - rhs.y;
      return (double) num1 * (double) num1 + (double) num2 * (double) num2 < 9.99999943962493E-11;
    }

    public static implicit operator Vector3(Vector2 v) => new Vector3(v.x, v.y, 0.0f);

    public static implicit operator Vector4(Vector2 v) => new Vector4(v.x, v.y, 0.0f, 0.0f);

    public static bool operator !=(Vector2 lhs, Vector2 rhs) => !(lhs == rhs);

    public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.x - b.x, a.y - b.y);

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
      interpolatedStringHandler.AppendLiteral("(");
      interpolatedStringHandler.AppendFormatted(__S.F(this.x));
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(__S.F(this.y));
      interpolatedStringHandler.AppendLiteral(")");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
