// Decompiled with JetBrains decompiler
// Type: BansheeGz.BGDatabase.Rect
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

using System.Runtime.CompilerServices;

#nullable enable
namespace BansheeGz.BGDatabase
{
  public struct Rect
  {
    public float x;
    public float y;
    public float width;
    public float height;
    public static Rect zero;

    public float xMax => this.x + this.width;

    public float yMax => this.y + this.height;

    public Rect(float x, float y, float width, float height)
    {
      this.x = x;
      this.y = y;
      this.width = width;
      this.height = height;
    }

    public Rect(Rect rect)
      : this(rect.x, rect.y, rect.width, rect.height)
    {
    }

    public Rect(Vector2 pos, Vector2 size)
    {
      this.x = pos.x;
      this.y = pos.y;
      this.width = size.x;
      this.height = size.y;
    }

    public bool Contains(Vector2 point)
    {
      return (double) point.x >= (double) this.x && (double) point.x < (double) this.xMax && (double) point.y >= (double) this.y && (double) point.y < (double) this.yMax;
    }

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 4);
      interpolatedStringHandler.AppendLiteral("(");
      interpolatedStringHandler.AppendFormatted(__S.F(this.x));
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(__S.F(this.y));
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(__S.F(this.width));
      interpolatedStringHandler.AppendLiteral("), ");
      interpolatedStringHandler.AppendFormatted(__S.F(this.height));
      interpolatedStringHandler.AppendLiteral(")");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
