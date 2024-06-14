// Decompiled with JetBrains decompiler
// Type: BansheeGz.BGDatabase.Mathf
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

using System;

#nullable disable
namespace BansheeGz.BGDatabase
{
  public class Mathf
  {
    public static readonly float Epsilon = float.Epsilon;
    public const float NegativeInfinity = float.NegativeInfinity;
    public const float Infinity = float.PositiveInfinity;
    public const float PI = 3.141593f;
    public const float Deg2Rad = 0.01745329f;
    public const float Rad2Deg = 57.29578f;

    public static float Abs(float f) => Math.Abs(f);

    public static float Max(float a, float b) => (double) a <= (double) b ? b : a;

    public static bool Approximately(float a, float b)
    {
      return (double) Mathf.Abs(b - a) < (double) Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a), Mathf.Abs(b)), Mathf.Epsilon * 8f);
    }

    public static int Clamp(int value, int min, int max)
    {
      if (value < min)
        value = min;
      else if (value > max)
        value = max;
      return value;
    }

    public static int CeilToInt(float f) => (int) Math.Ceiling((double) f);

    public static float Ceil(float f) => (float) Math.Ceiling((double) f);

    public static float Floor(float f) => (float) Math.Floor((double) f);

    public static int FloorToInt(float f) => (int) Math.Floor((double) f);

    public static float Min(float a, float b) => (double) a >= (double) b ? b : a;

    public static float Pow(float f, float p) => (float) Math.Pow((double) f, (double) p);

    public static int RoundToInt(float f) => (int) Math.Round((double) f);

    public static float Sqrt(float f) => (float) Math.Sqrt((double) f);

    public static float Acos(float f) => (float) Math.Acos((double) f);

    public static float Asin(float f) => (float) Math.Asin((double) f);

    public static float Atan2(float y, float x) => (float) Math.Atan2((double) y, (double) x);

    public static float Atan(float f) => (float) Math.Atan((double) f);

    public static float Cos(float f) => (float) Math.Cos((double) f);

    public static float Sin(float f) => (float) Math.Sin((double) f);

    public static float Tan(float f) => (float) Math.Tan((double) f);
  }
}
