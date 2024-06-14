/*
<copyright file="BGRTDropDownList.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public class BGRTDropDownList<T>
    {
        public event Action OnChange;

        protected Func<List<T>> Provider;
        private Func<T, bool> gui;
        private Func<T, bool> itemGui;
        private T current;
        public bool ManualClosing;

        public T Current
        {
            get => current;
            set => current = value;
        }

        public BGRTDropDownList(Func<T, bool> gui, Func<T, bool> itemGui, Func<List<T>> provider)
        {
            this.gui = gui;
            this.itemGui = itemGui;
            Provider = provider;
        }

        public List<T> Provide()
        {
            return Provider();
        }
        
        public void Gui()
        {
            if (!gui(current)) return;
            var list = Provider();
            var exit = false;
            var scrollView = new BGRTScrollView(() =>
            {
                if (list == null || list.Count == 0) GUILayout.Label("No data", BGRTStyle.Editor_label);
                else
                {
                    foreach (var item in list)
                    {
                        if (!itemGui(item)) continue;
                        current = item;
                        exit = true;
                        OnChange?.Invoke();
                        return;
                    }
                }
            });

            BGDatabaseMonitorGo.Popup(400, 300, "Choose value", () =>
            {
                scrollView.Gui();
                return !ManualClosing && exit;
            });
        }

    }
}