﻿// Decompiled with JetBrains decompiler
// Type: BansheeGz.BGDatabase.AssetBundle
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

#nullable enable
namespace BansheeGz.BGDatabase
{
  public class AssetBundle : Object
  {
    public T LoadAsset<T>(string path) where T : Object => default (T);

    public T[] LoadAssetWithSubAssets<T>(string path) where T : Object => (T[]) null;
  }
}