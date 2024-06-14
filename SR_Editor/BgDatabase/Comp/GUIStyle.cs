// Decompiled with JetBrains decompiler
// Type: BansheeGz.BGDatabase.GUIStyle
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

#nullable enable
namespace BansheeGz.BGDatabase
{
  public sealed class GUIStyle
  {
    public GUIStyle normal;
    public Texture2D background;
    public int fontSize;
    public RectOffset border;
    public FontStyle fontStyle;
    public RectOffset padding;
    public Color32 textColor;
    public RectOffset margin;
    public RectOffset overflow;
    public object font;
    public object clipping;
    public bool richText;
    public TextAnchor alignment;

    public GUIStyle(string textarea)
    {
    }

    public GUIStyle()
    {
    }

    public Vector2 CalcSize(GUIContent guiContent) => Vector2.zero;
  }
}
