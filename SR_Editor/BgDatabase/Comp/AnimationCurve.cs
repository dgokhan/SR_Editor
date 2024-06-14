// Decompiled with JetBrains decompiler
// Type: BansheeGz.BGDatabase.AnimationCurve
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

using System.Collections.Generic;

#nullable enable
namespace BansheeGz.BGDatabase
{
  public class AnimationCurve
  {
    private readonly List<Keyframe> keysList = new List<Keyframe>();
    public WrapMode preWrapMode;
    public WrapMode postWrapMode;

    public Keyframe[] keys
    {
      get => this.keysList.ToArray();
      set
      {
        this.keysList.Clear();
        if (value == null)
          return;
        this.keysList.AddRange((IEnumerable<Keyframe>) value);
      }
    }

    public AnimationCurve(Keyframe[] keys) => this.keys = keys;

    public AnimationCurve()
    {
    }

    public void AddKey(Keyframe key) => this.keysList.Add(key);
  }
}
