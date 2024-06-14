/*
<copyright file="BGRuntimeWindow.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    //copied from RuntimeEditor plugin BGRuntimeDatabaseWindow
    public class BGRuntimeWindow
    {
        // private Rect area;

        private bool dragging;
        private Vector2 dragPosition;

        private readonly Queue<Action> contentActions = new Queue<Action>();
        private readonly BGWindowParameters windowParameters;
        private readonly BGRuntimeWindowResizer windowResizer;
        private readonly BGHotKey minimizeKey;
        private readonly BGHotKey showHideKey;
        private Action contentAction;
        private readonly Action onMouseUp;

        internal BGWindowParameters WindowParameters => windowParameters;

        public Rect Area
        {
            get => windowParameters.area;
            set => windowParameters.area = value;
        }

        private bool InsideTitle =>
            new Rect(windowParameters.area) {height = BGRTUtilities.MinHeight, width = windowParameters.area.width - BGRTUtilities.MinHeight * 2}.Contains(Event.current.mousePosition);

        public BGRuntimeWindow(Action contentAction, BGWindowParameters windowParameters, BGHotKey minimizeKey, BGHotKey showHideKey, Action onMouseUp)
        {
            this.contentAction = contentAction;
            this.windowParameters = windowParameters;
            this.minimizeKey = minimizeKey;
            this.showHideKey = showHideKey;
            this.onMouseUp = onMouseUp;
            windowResizer = new BGRuntimeWindowResizer(this);
        }

        public void Gui()
        {
            using (BGRTUsing.CursorColor(Color.black))
            {
                using (BGRTUsing.DisableGUI(BGRTPopup.Active))
                {
                    ProcessInput();
                    Window();
                    AfterGui();
                }

                BGRTPopup.Gui();
            }
        }

        private void Window()
        {
            if (windowParameters.closed) return;

            GUILayout.BeginArea(!windowParameters.minimized ? windowParameters.area : new Rect(windowParameters.area) {height = BGRTUtilities.MinHeight, width = 200});

            //title
            Title();

            if (!windowParameters.minimized)
            {
                //content
                BGRTUtilities.Vertical(BGRTStyle.Box, () =>
                {
                    contentAction();
                }, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            }

            GUILayout.EndArea();
        }

        private void Title()
        {
           
            BGRTUtilities.Horizontal(() =>
            {
                var titleHeight = BGDatabaseMonitorGo.LabelHeight;

                GUILayout.Label( GetHeader(), BGRTStyle.WindowTitle, GUILayout.Height(titleHeight));
                var lastRect = GUILayoutUtility.GetLastRect();

                if (GUI.Button(new Rect(lastRect)
                {
                    x = lastRect.xMax - (titleHeight * 2) + 1,
                    width = titleHeight - 2,
                    y = lastRect.y + 1,
                    height = lastRect.yMax - 2
                }, windowParameters.minimized ? "O" : "_", BGRTStyle.Button))
                {
                    windowParameters.minimized = !windowParameters.minimized;
                }

                if (GUI.Button(new Rect(lastRect)
                {
                    x = lastRect.xMax - titleHeight + 1,
                    width = titleHeight - 2,
                    y = lastRect.y + 1,
                    height = lastRect.yMax - 2
                }, "X", BGRTStyle.Button))
                {
                    windowParameters.closed = true;
                }
            });
        }

        private string GetHeader() => "BGDatabaseMonitor";

        private void ProcessInput()
        {
            if (!windowParameters.closed)
            {
                var mousePosition = Event.current.mousePosition;

                //dragndrop
                if (Event.current.type == EventType.MouseDown && InsideTitle && !windowParameters.movingIsDisabled)
                {
                    dragging = true;
                    dragPosition = mousePosition;
                }

                if (Event.current.type == EventType.MouseUp)
                {
                    if (dragging) dragging = false;
                    else onMouseUp?.Invoke();
                }

                if (dragging)
                {
                    var delta = mousePosition - dragPosition;
                    windowParameters.area.x += delta.x;
                    windowParameters.area.y += delta.y;
                    dragPosition = mousePosition;
                }
            }

            if (minimizeKey.Pressed) windowParameters.minimized = !windowParameters.minimized;
            if (showHideKey.Pressed) windowParameters.closed = !windowParameters.closed;
        }

        private void AfterGui()
        {
            if (!windowParameters.closed && !windowParameters.minimized)
            {
                windowResizer.Process();
            }
        }

        public void Push(Action gui)
        {
            contentActions.Enqueue(contentAction);
            contentAction = gui;
        }

        public void Pop()
        {
            contentAction = contentActions.Dequeue();
        }


        [Serializable]
        public class BGWindowParameters
        {
            [Tooltip("Screen rectangle for editor window")]
            public Rect area = new Rect(100, 100, 670, 400);

            [Tooltip("Should editor be minimized on start")]
            public bool minimized;

            [Tooltip("Should editor be hidden on start")]
            public bool closed;

            [Tooltip("Windows can not be moved if it's true")]
            public bool movingIsDisabled;

            [Tooltip("Windows can not be resized if it's true")]
            public bool resizingIsDisabled;
        }

        [Serializable]
        public class BGHotKey
        {
            [Tooltip("Shortcut key")] public KeyCode key;

            [Tooltip("Should Shift key also be pressed")]
            public bool keyShift;

            [Tooltip("Should Ctrl key also be pressed")]
            public bool keyCtrl;

            [Tooltip("Should Alt key also be pressed")]
            public bool keyAlt;

            public bool Pressed
            {
                get
                {
                    if (key == KeyCode.None) return false;
                    if (Event.current.type != EventType.KeyDown) return false;
                    if (Event.current.keyCode != key) return false;
                    if (keyShift && !Event.current.shift) return false;
                    if (keyCtrl && !Event.current.control) return false;
                    if (keyAlt && !Event.current.alt) return false;
                    return true;
                }
            }
        }
    }
}