// Decompiled with JetBrains decompiler
// Type: BansheeGz.BGDatabase.Component
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

using System;

#nullable enable
namespace BansheeGz.BGDatabase
{
  public class Component : Object
  {
    public GameObject gameObject;

    public T[] GetComponents<T>() where T : Component => throw new NotImplementedException();

    public T GetComponent<T>() where T : Component => throw new NotImplementedException();
  }
}
