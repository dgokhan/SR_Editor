// Decompiled with JetBrains decompiler
// Type: BansheeGz.BGDatabase.JsonUtility
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll


#nullable enable

using System.Text.Json;

namespace BansheeGz.BGDatabase
{
  public class JsonUtility
  {
    public static string ToJson(object jsonConfig, bool pretty = false)
    {
      return JsonSerializer.Serialize<object>(jsonConfig, new JsonSerializerOptions()
      {
        WriteIndented = pretty
      });
    }

    public static T FromJson<T>(string config)
    {
      return JsonSerializer.Deserialize<T>(config) ?? default (T);
    }
  }
}
