// Decompiled with JetBrains decompiler
// Type: BansheeGz.BGDatabase.GUILayout
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

using System;

#nullable enable
namespace BansheeGz.BGDatabase
{
  public class GUILayout
  {
    public static void EndScrollView()
    {
    }

    public static Vector2 BeginScrollView(
      Vector2 position,
      bool alwaysShowHorizontal,
      bool alwaysShowVertical,
      GUILayoutOption[] options)
    {
      return new Vector2();
    }

    public static GUILayoutOption Width(int width) => (GUILayoutOption) null;

    public static void Label(string label, GUIStyle editorLabel, params GUILayoutOption[] @params)
    {
    }

    public static void BeginVertical(GUILayoutOption[] options)
    {
    }

    public static bool Button(string message, GUIStyle button) => false;

    public static void Label(
      GUIContent label,
      GUIStyle editorLabel,
      params GUILayoutOption[] @params)
    {
    }

    public static bool Button(string message, GUIStyle button, GUILayoutOption width) => false;

    public static void EndVertical()
    {
    }

    public static void BeginVertical(GUIStyle options)
    {
    }

    public static void BeginVertical(GUIStyle options, GUILayoutOption[] guiLayoutOptions)
    {
    }

    public static void BeginHorizontal(GUILayoutOption[] options)
    {
    }

    public static void EndHorizontal()
    {
    }

    public static void BeginHorizontal(GUIStyle options)
    {
    }

    public static void BeginHorizontal(GUIStyle options, GUILayoutOption[] guiLayoutOptions)
    {
    }

    public static GUILayoutOption Width(float width) => (GUILayoutOption) null;

    public static GUILayoutOption Height(float height) => (GUILayoutOption) null;

    public static void Window(int i, Rect area, Action<int> gui, string title)
    {
    }

    public static GUILayoutOption ExpandWidth(bool b) => (GUILayoutOption) null;

    public static GUILayoutOption ExpandHeight(bool b) => (GUILayoutOption) null;

    public static void TextArea(string log, GUIStyle guiStyle, params GUILayoutOption[] options)
    {
    }

    public static void BeginArea(Rect windowParametersArea)
    {
    }

    public static void EndArea()
    {
    }

    public static void Space(int i)
    {
    }

    public static void FlexibleSpace()
    {
    }
  }
}
