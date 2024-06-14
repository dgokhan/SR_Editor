/*
<copyright file="BGMonitorPageLiveUpdateStatus.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public class BGMonitorPageLiveUpdateStatus : BGMonitorPage
    {
        public override string Name => "LiveUpdate addon status";
        public BGRTScrollView scrollView;

        public override void Gui()
        {
            var addon = BGRepo.I.Addons.Get<BGAddonLiveUpdate>();
            if (addon == null) BGRTUtilities.Label("LiveUpdate plugin is not installed!");
            else
            {
                var log = addon.Log;
                BGRTUtilities.Label($"LiveUpdate plugin status: {log.Status}");
                scrollView = scrollView ?? new BGRTScrollView(() => ShowLog(addon));
                scrollView.Gui();
            }
        }

        private void ShowLog(BGAddonLiveUpdate addon)
        {
            var log = addon.Log.GetLog();
            GUILayout.TextArea(log, new GUIStyle("textArea")
            {
                fontSize = BGDatabaseMonitorGo.I.fontSize,
            },GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        }
    }
}