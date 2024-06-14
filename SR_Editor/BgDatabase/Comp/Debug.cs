// Decompiled with JetBrains decompiler
// Type: BansheeGz.BGDatabase.Debug
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

using System;

#nullable enable
namespace BansheeGz.BGDatabase
{
  public class Debug
  {
    public static void LogError(string message) => Console.WriteLine(message);

    public static void LogException(Exception exception) => Console.WriteLine(exception.ToString());

    public static void Log(string message) => Console.WriteLine(message);

    public static void Log(object val) => Console.WriteLine(val == null ? "null" : val.ToString());

    public static void Assert(bool b)
    {
      if (!b)
        throw new Exception("Incorrect value, expected true, but actual value is false");
    }
  }
}
