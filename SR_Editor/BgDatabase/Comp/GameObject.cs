// Decompiled with JetBrains decompiler
// Type: BansheeGz.BGDatabase.GameObject
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

using System;

#nullable enable
namespace BansheeGz.BGDatabase
{
  public class GameObject : Object
  {
    public object hideFlags;
    public Transform transform;
    public Scene scene;

    public GameObject(string name)
    {
    }

    public T AddComponent<T>() where T : Component => throw new NotImplementedException();

    public Array GetComponents(System.Type type) => throw new NotImplementedException();

    public Component GetComponent(string typeName) => throw new NotImplementedException();

    public bool CompareTag(string includeTag) => throw new NotImplementedException();
  }
}
