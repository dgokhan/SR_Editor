/*
<copyright file="BGRTDialog.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public static class BGRTDialog
    {
        public static void Info(string message)
        {
            BGDatabaseMonitorGo.Popup(300,140, "Info", () =>
            {
                var messageStyle = GUI.skin.textArea;
                var buttonStyle = GUI.skin.button;
                GUILayout.TextArea(message, messageStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                return GUILayout.Button("Ok", buttonStyle);
            });
        }
    }
}