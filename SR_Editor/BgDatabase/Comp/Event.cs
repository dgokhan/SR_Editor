// Decompiled with JetBrains decompiler
// Type: BansheeGz.BGDatabase.Event
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

#nullable enable
namespace BansheeGz.BGDatabase
{
  public sealed class Event
  {
    public static Event current;
    public EventType type;
    public Vector2 mousePosition;
    public KeyCode keyCode;
    public bool shift;
    public bool control;
    public bool alt;

    public void Use()
    {
    }
  }
}
