// Decompiled with JetBrains decompiler
// Type: BansheeGz.BGDatabase.Vector3
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

using System.Runtime.CompilerServices;

#nullable enable
namespace BansheeGz.BGDatabase
{
  public struct Vector3
  {
    public float x;
    public float y;
    public float z;
    public static Vector3 zero;

    public Vector3(float x, float y, float z)
    {
      this.x = x;
      this.y = y;
      this.z = z;
    }

    public static implicit operator Vector2(Vector3 v) => new Vector2(v.x, v.y);

    public static implicit operator Vector4(Vector3 v) => new Vector4(v.x, v.y, v.z, 0.0f);

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 3);
      interpolatedStringHandler.AppendLiteral("(");
      interpolatedStringHandler.AppendFormatted(__S.F(this.x));
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(__S.F(this.y));
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(__S.F(this.z));
      interpolatedStringHandler.AppendLiteral(")");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
