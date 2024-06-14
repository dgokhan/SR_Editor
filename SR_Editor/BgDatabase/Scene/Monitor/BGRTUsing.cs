/*
<copyright file="BGRTUsing.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public class BGRTUsing
    {
        //=================================================================================================================
        //                      Disable GUI
        //=================================================================================================================
        public static IDisposable DisableGUI(bool condition)
        {
            return new BGDisabledGUI(condition);
        }

        private struct BGDisabledGUI : IDisposable
        {
            private readonly bool oldEnabled;
            private readonly bool condition;


            internal BGDisabledGUI(bool condition)
            {
                this.condition = condition;
                if (condition)
                {
                    oldEnabled = GUI.enabled;
                    GUI.enabled = false;
                }
                else oldEnabled = true;
            }

            public void Dispose()
            {
                if (!condition) return;
                GUI.enabled = oldEnabled;
            }
        }

        //=================================================================================================================
        //                      cursor color
        //=================================================================================================================
        public static IDisposable CursorColor(Color color)
        {
            return new BGCursorColor(color);
        }

        private struct BGCursorColor : IDisposable
        {
            private readonly Color color;

            internal BGCursorColor(Color color)
            {
                this.color = GUI.skin.settings.cursorColor;
                GUI.skin.settings.cursorColor = color;
            }

            public void Dispose()
            {
                GUI.skin.settings.cursorColor = color;
            }
        }
    }
}