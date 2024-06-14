// Decompiled with JetBrains decompiler
// Type: BansheeGz.BGDatabase.Object
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

#nullable enable
namespace BansheeGz.BGDatabase
{
  public class Object
  {
    public string name;

    public static void DontDestroyOnLoad(GameObject go)
    {
    }

    public static void Destroy(Object go)
    {
    }

    public static T[] FindObjectsOfType<T>() where T : MonoBehaviour => (T[]) null;

    public static Object[] FindObjectsOfType(System.Type type) => (Object[]) null;
  }
}
