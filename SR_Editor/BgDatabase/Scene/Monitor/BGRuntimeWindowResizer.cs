/*
<copyright file="BGRuntimeWindowResizer.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public class BGRuntimeWindowResizer
    {
        private const int MinWidth = 200;
        private const int MinHeight = 100;

        private enum ResizeMode
        {
            None,
            Horizontal,
            Vertical,
            Diagonal
        }

        private Texture2D resizeIcon;

        private Texture2D ResizeIcon
        {
            get
            {
                if (resizeIcon != null) return resizeIcon;
                resizeIcon = BGRTStyle.Resizer;
                return resizeIcon;
            }
        }

        private readonly BGRuntimeWindow window;
        private ResizeMode resizeMode;
        private Vector2 draggingStartPosition;

        public BGRuntimeWindowResizer(BGRuntimeWindow window)
        {
            this.window = window;
        }

        public void Process()
        {
            if (window.WindowParameters.resizingIsDisabled) return;
            if (Event.current.type == EventType.MouseUp) resizeMode = ResizeMode.None;

            var mousePosition = Event.current.mousePosition;
            var area = window.Area;
            var resizeDTexture = ResizeIcon;

            if (new Rect(area)
            {
                x = area.xMax - resizeDTexture.width * .5f,
                y = area.yMax - resizeDTexture.height * .5f,
                width = resizeDTexture.width,
                height = resizeDTexture.height
            }.Contains(mousePosition))
            {
                DrawResizeCursor(mousePosition, resizeDTexture);

                if (Event.current.type == EventType.MouseDown) resizeMode = ResizeMode.Diagonal;
                Event.current.Use();
            }
            else
            {
                Cursor.visible = true;
            }


            switch (resizeMode)
            {
                case ResizeMode.None:
                    break;
                case ResizeMode.Horizontal:
                    AdjustWidth(mousePosition);
                    break;
                case ResizeMode.Vertical:
                    AdjustHeight(mousePosition);
                    break;
                case ResizeMode.Diagonal:
                    AdjustWidth(mousePosition);
                    AdjustHeight(mousePosition);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AdjustHeight(Vector2 mousePosition)
        {
            if (mousePosition.y - window.Area.y > MinHeight) window.Area = new Rect(window.Area) { height = mousePosition.y - window.Area.y };
        }

        private void AdjustWidth(Vector2 mousePosition)
        {
            if (mousePosition.x - window.Area.x > MinWidth) window.Area = new Rect(window.Area) { width = mousePosition.x - window.Area.x };
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

    }
}