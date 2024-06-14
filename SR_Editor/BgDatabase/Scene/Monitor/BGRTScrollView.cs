/*
<copyright file="BGRTScrollView.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public class BGRTScrollView
    {
        private Vector2 position;
        public event Action OnChange;

        public Vector2 Position
        {
            get => position;
            set
            {
                position = value;
                OnChange?.Invoke();
            }
        }

        private readonly Action view;
        private readonly bool alwaysShowHorizontal;
        private readonly bool alwaysShowVertical;

        public BGRTScrollView(Action view, bool alwaysShowHorizontal = false, bool alwaysShowVertical = false)
        {
            this.view = view;
            this.alwaysShowHorizontal = alwaysShowHorizontal;
            this.alwaysShowVertical = alwaysShowVertical;
        }

        public void Gui()
        {
            BeginScroll();
            view();
            GUILayout.EndScrollView();
        }

        private void BeginScroll(params GUILayoutOption[] options)
        {
            var newPosition = GUILayout.BeginScrollView(position, alwaysShowHorizontal, alwaysShowVertical, options);

            if (OnChange != null && newPosition != position)
            {
                position = newPosition;
                OnChange();
            }
            else position = newPosition;
        }
    }
}