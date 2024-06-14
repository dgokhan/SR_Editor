/*
<copyright file="BGDatabaseMonitorGo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public class BGDatabaseMonitorGo : MonoBehaviour
    {
        //serialized
        public int fontSize = 14;

        public BGRuntimeWindow.BGWindowParameters windowParameters;
        public BGRuntimeWindow.BGHotKey showHideKey;
        public BGRuntimeWindow.BGHotKey minimizeMaximizeKey;

        //non serialized
        public static BGDatabaseMonitorGo I;
        private static Vector2 lastMousePosition;
        public static float LabelHeight;
        private BGRuntimeWindow window;
        public static bool Disabled;
        private int page = 0;
        private BGRTDropDownList<BGMonitorPage> metaDropDown;

        private int oldFonSize;

        //============================== Unity callbacks
        private void Start()
        {
            if (I != null) return;
            I = this;
            DontDestroyOnLoad(gameObject);
            ResetFont();

            window = new BGRuntimeWindow(MyGui, windowParameters, minimizeMaximizeKey, showHideKey, null);

            var list = new List<BGMonitorPage>
            {
                new BGMonitorPageHome(),
                new BGMonitorPageLiveUpdateStatus()
            };
            metaDropDown = new BGRTDropDownList<BGMonitorPage>(
                page => GUILayout.Button(page.Name, BGRTStyle.Button),
                page => GUILayout.Button(page.Name, BGRTStyle.Button),
                () => list
            )
            {
                Current = list[0]
            };
        }

        private void ResetFont()
        {
            oldFonSize = fontSize;
            var style = new GUIStyle("label")
            {
                fontSize = fontSize
            };
            LabelHeight = style.CalcSize(new GUIContent("Q")).y;
            BGRTUtilities.MinHeight = (int)LabelHeight;
            BGRTStyle.Reset();
        }

        private void OnGUI()
        {
            if (I != this) return;
            if (Disabled) return;
            if (oldFonSize != fontSize) ResetFont();
            lastMousePosition = Event.current.mousePosition;
            window.Gui();
        }

        private void OnDestroy()
        {
            if (I != this) return;
            I = null;
            Disabled = false;
            BGRTPopup.Reset();
        }

        //============================== Methods
        private void MyGui()
        {
            if (!BGRepo.Ok) BGRTUtilities.Label("Database error: " + BGRepo.DefaultRepoErrorOnLoad);
            else
            {
                BGRTUtilities.Horizontal(() =>
                {
                    BGRTUtilities.Label("Database is loaded ok!");
                    GUILayout.Space(4);
                    BGRTUtilities.Label("Tools >>");
                    metaDropDown.Gui();
                    GUILayout.FlexibleSpace();
                });
                metaDropDown.Current.Gui();
            }
        }

        //============================== static Methods
        public static void Popup(int width, int height, string title, Func<bool> action, Action onClose = null)
        {
            float x, y;
            if (lastMousePosition.x < Screen.width * .5) x = lastMousePosition.x;
            else x = lastMousePosition.x - width;
            if (lastMousePosition.y < Screen.height * .5) y = lastMousePosition.y;
            else y = lastMousePosition.y - height;
            BGRTPopup.Popup(new Rect(new Vector2(x, y), new Vector2(width, height)), title, action, onClose);
        }
    }
}