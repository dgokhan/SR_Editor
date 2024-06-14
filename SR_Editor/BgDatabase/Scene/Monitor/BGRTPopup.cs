/*
<copyright file="BGRTPopup.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public class BGRTPopup
    {
        private static bool active;
        private static Func<bool> action;
        private static Action OnClose;
        private static Rect area;
        private static string title;

        public static bool Active => active;

        public static void Popup(Rect area, string title, Func<bool> action, Action OnClose = null)
        {
            active = true;
            BGRTPopup.area = area;
            BGRTPopup.action = action;
            BGRTPopup.OnClose = OnClose;
            BGRTPopup.title = title;
        }

        public static void Gui()
        {
            if (!active) return;
            if (Event.current.type == EventType.MouseDown && !area.Contains(Event.current.mousePosition))
            {
                active = false;
                OnClose?.Invoke();
                return;
            }

            GUILayout.Window(0, area, Gui, title);
        }

        private static void Gui(int id)
        {
            using (BGRTUsing.CursorColor(Color.black))
            {
                if(action()) active = false;
            }
        }

        public static void Reset()
        {
            active = false;
            OnClose = null;
            action = null;
            title = null;
        }

    }
}