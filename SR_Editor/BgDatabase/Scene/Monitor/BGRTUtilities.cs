/*
<copyright file="BGRTUtilities.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public static class BGRTUtilities
    {
        public static int MinHeight = 22;

        public static readonly GUILayoutOption[] OptionsMinRect = GetOptions(MinHeight, MinHeight);

        public static bool EventIsRepaint => Event.current.type == EventType.Repaint;

        public static void Vertical(Action callback, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(options);
            callback();
            GUILayout.EndVertical();
        }

        public static void Vertical(GUIStyle style, Action callback)
        {
            GUILayout.BeginVertical(style);
            callback();
            GUILayout.EndVertical();
        }

        public static void Vertical(GUIStyle style, Action callback, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(style, options);
            callback();
            GUILayout.EndVertical();
        }

        public static void Horizontal(Action callback, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(options);
            callback();
            GUILayout.EndHorizontal();
        }

        public static void Horizontal(GUIStyle style, Action callback)
        {
            GUILayout.BeginHorizontal(style);
            callback();
            GUILayout.EndHorizontal();
        }

        public static void Horizontal(GUIStyle style, Action callback, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(style, options);
            callback();
            GUILayout.EndHorizontal();
        }

        public static GUILayoutOption[] GetOptions(float width, float height) => new[] { GUILayout.Width(width), GUILayout.Height(height) };


        public static T GetAttribute<T>(Type type) where T : Attribute => (T)Attribute.GetCustomAttribute(type, typeof(T));

        public static bool Log(bool condition, string message, params object[] parameters)
        {
            if (!condition) return false;

            Debug.Log(BGUtil.Format(message, parameters));
            return true;
        }

        public static void DrawResizeCursor(Vector2 mousePosition, Texture2D texture)
        {
            Cursor.visible = false;
            GUI.DrawTexture(new Rect
            {
                x = mousePosition.x - texture.width * .5f,
                y = mousePosition.y - texture.height * .5f,
                width = texture.width,
                height = texture.height
            }, texture);
        }

        public static void Label(string label, int width) => Label(label, GUILayout.Width(width));

        public static void Label(string label) => Label(label, null);

        public static void Label(string label, params GUILayoutOption[] @params) => GUILayout.Label(label, BGRTStyle.Editor_label, @params);

        public static void Label(GUIContent label, int width) => Label(label, GUILayout.Width(width));

        public static void Label(GUIContent label) => Label(label, null);

        public static void Label(GUIContent label, params GUILayoutOption[] @params) => GUILayout.Label(label, BGRTStyle.Editor_label, @params);

        public static bool Button(string message, int width = 0) => width == 0 ? GUILayout.Button(message, BGRTStyle.Button) : GUILayout.Button(message, BGRTStyle.Button, GUILayout.Width(width));

        public static void ResetHotControl()
        {
            GUIUtility.hotControl = 0;
            GUIUtility.keyboardControl = 0;
        }

        /*
        public class ValueWrapper<T>
        {
            private readonly Func<T> getter;
            private readonly Action<T> setter;

            public T Value
            {
                get => getter();
                set => setter(value);
            }

            public ValueWrapper(Func<T> getter, Action<T> setter)
            {
                this.getter = getter;
                this.setter = setter;
            }
            
            public static implicit operator T(ValueWrapper<T> v) => v.getter();
        }
    */
        public static void Try(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                BGRTDialog.Info("Error! " + e.Message);
            }
        }

        /*public static GUIStyle PopupLabelStyle
        {
            get
            {
                var labelStyle = BGRuntimeDatabaseEditor.I == null || BGRuntimeDatabaseEditor.I.GUICustomization == null
                    ? BGRuntimeStyle.Editor_label
                    : BGRuntimeDatabaseEditor.I.GUICustomization.GetPopupStyle(BGRuntimeStyle.Editor_label);
                return labelStyle;
            }
        }
        public static GUIStyle PopupTextStyle
        {
            get
            {
                var textStyle = BGRuntimeDatabaseEditor.I == null || BGRuntimeDatabaseEditor.I.GUICustomization == null
                    ? BGRuntimeStyle.Editor_textField
                    : BGRuntimeDatabaseEditor.I.GUICustomization.GetPopupStyle(BGRuntimeStyle.Editor_textField);
                return textStyle;
            }
        }
        public static GUIStyle PopupButtonStyle
        {
            get
            {
                var buttonStyle = BGRuntimeDatabaseEditor.I == null || BGRuntimeDatabaseEditor.I.GUICustomization == null
                    ? BGRuntimeStyle.Button
                    : BGRuntimeDatabaseEditor.I.GUICustomization.GetPopupStyle(BGRuntimeStyle.Button);
                return buttonStyle;
            }
        }*/
    }
}