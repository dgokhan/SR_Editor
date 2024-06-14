// Decompiled with JetBrains decompiler
// Type: BansheeGz.BGDatabase.Keyframe
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

#nullable disable
namespace BansheeGz.BGDatabase
{
  public struct Keyframe
  {
    public int tangentMode;
    public float time;
    public float value;
    public float inTangent;
    public float outTangent;
    public WeightedMode weightedMode;
    public float inWeight;
    public float outWeight;

    public Keyframe(float time, float value, float inTangent, float outTangent)
    {
      this.tangentMode = 0;
      this.weightedMode = (WeightedMode) 0;
      this.inWeight = 0.0f;
      this.outWeight = 0.0f;
      this.time = time;
      this.value = value;
      this.inTangent = inTangent;
      this.outTangent = outTangent;
    }

    public Keyframe(
      float time,
      float value,
      float inTangent,
      float outTangent,
      float inWeight,
      float outWeight)
    {
      this.tangentMode = 0;
      this.weightedMode = (WeightedMode) 0;
      this.time = time;
      this.value = value;
      this.inTangent = inTangent;
      this.outTangent = outTangent;
      this.inWeight = inWeight;
      this.outWeight = outWeight;
    }
  }
}
