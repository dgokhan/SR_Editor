// Decompiled with JetBrains decompiler
// Type: BansheeGz.BGDatabase.Vector4
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

using System.Runtime.CompilerServices;

#nullable enable
namespace BansheeGz.BGDatabase
{
  public struct Vector4
  {
    public float x;
    public float y;
    public float z;
    public float w;
    public static Vector4 zero;

    public Vector4(float x, float y, float z, float w)
    {
      this.x = x;
      this.y = y;
      this.z = z;
      this.w = w;
    }

    public static implicit operator Vector2(Vector4 v) => new Vector2(v.x, v.y);

    public static implicit operator Vector3(Vector4 v) => new Vector3(v.x, v.y, v.z);

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 4);
      interpolatedStringHandler.AppendLiteral("(");
      interpolatedStringHandler.AppendFormatted(__S.F(this.x));
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(__S.F(this.y));
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(__S.F(this.z));
      interpolatedStringHandler.AppendLiteral("), ");
      interpolatedStringHandler.AppendFormatted(__S.F(this.w));
      interpolatedStringHandler.AppendLiteral(")");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
