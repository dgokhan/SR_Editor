/*
<copyright file="BGRTStyle.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public static class BGRTStyle
    {
        private static Texture2D GetTexture(Color color)
        {
            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

        private static Texture2D GetTexture(Color32[] rawData, int width, int height)
        {
            var texture = new Texture2D(width, height);
            texture.SetPixels32(rawData);
            texture.Apply();
            return texture;
        }

        public static GUIStyle Get(ref GUIStyle style, Func<GUIStyle> getter, bool noTexture = false)
        {
            if (style == null || (!noTexture && style.normal.background == null)) style = getter();
            return style;
        }

        private static GUIStyle box;

        public static GUIStyle Box
        {
            get
            {
                return Get(ref box, () => new GUIStyle
                {
                    border = new RectOffset(8, 8, 8, 8),
                    fontSize = BGDatabaseMonitorGo.I.fontSize,
                    normal =
                    {
                        background = GetTexture(new Color(0, 0, 0, 0.3f)),
                    },
                });
            }
        }

        private static GUIStyle windowTitle;

        public static GUIStyle WindowTitle
        {
            get
            {
                return Get(ref windowTitle, () => new GUIStyle
                {
                    padding = new RectOffset(4, 4, 4, 4),
                    border = new RectOffset(4, 4, 4, 4),
                    fontStyle = FontStyle.Bold,
                    fontSize = BGDatabaseMonitorGo.I.fontSize,
                    normal =
                    {
                        background = GetTexture(new Color(0, 0, 0, 0.8f)),
                        textColor = new Color32(255, 255, 255, 255)
                    },
                });
            }
        }


        private static GUIStyle editor_label;

        public static GUIStyle Editor_label
        {
            get
            {
                return Get(ref editor_label, () => new GUIStyle
                {
                    border = new RectOffset(0, 0, 0, 0),
                    padding = new RectOffset(2, 2, 1, 2),
                    margin = new RectOffset(4, 4, 2, 2),
                    overflow = new RectOffset(0, 0, 0, 0),
                    font = new GUIStyle("label").font,
                    fontSize = BGDatabaseMonitorGo.I.fontSize,
                    clipping = TextClipping.Clip,
                    richText = false,
                    normal =
                    {
                        textColor = Color.white,
                    },
                }, true);
            }
        }

        private static GUIStyle button;

        public static GUIStyle Button
        {
            get
            {
                return Get(ref button, () =>
                {
                    var data = new Color32[]
                    {
                        new Color32(93, 187, 200, 204), new Color32(157, 216, 223, 246), new Color32(158, 217, 223, 248), new Color32(158, 217, 223, 247), new Color32(158, 217, 223, 247),
                        new Color32(158, 217, 223, 247), new Color32(158, 217, 223, 247), new Color32(158, 217, 223, 247), new Color32(158, 217, 223, 247), new Color32(158, 217, 223, 247),
                        new Color32(158, 217, 223, 247), new Color32(158, 217, 223, 247), new Color32(158, 217, 223, 247), new Color32(158, 217, 223, 247), new Color32(162, 219, 225, 249),
                        new Color32(129, 204, 213, 228), new Color32(104, 191, 203, 239), new Color32(175, 221, 226, 255), new Color32(175, 221, 226, 255), new Color32(175, 221, 226, 255),
                        new Color32(175, 221, 226, 255), new Color32(175, 221, 226, 255), new Color32(175, 221, 226, 255), new Color32(175, 221, 226, 255), new Color32(175, 221, 226, 255),
                        new Color32(175, 221, 226, 255), new Color32(175, 221, 226, 255), new Color32(175, 221, 226, 255), new Color32(175, 221, 226, 255), new Color32(175, 221, 226, 255),
                        new Color32(180, 223, 228, 255), new Color32(145, 209, 216, 249), new Color32(103, 190, 202, 234), new Color32(172, 219, 225, 255), new Color32(172, 219, 225, 255),
                        new Color32(172, 219, 225, 255), new Color32(172, 219, 225, 255), new Color32(172, 219, 225, 255), new Color32(172, 219, 225, 255), new Color32(172, 219, 225, 255),
                        new Color32(172, 219, 225, 255), new Color32(172, 219, 225, 255), new Color32(172, 219, 225, 255), new Color32(172, 219, 225, 255), new Color32(172, 219, 225, 255),
                        new Color32(172, 219, 225, 255), new Color32(177, 221, 227, 255), new Color32(143, 207, 215, 246), new Color32(103, 190, 202, 234), new Color32(173, 219, 225, 255),
                        new Color32(173, 219, 225, 255), new Color32(173, 219, 225, 255), new Color32(173, 219, 225, 255), new Color32(173, 219, 225, 255), new Color32(173, 219, 225, 255),
                        new Color32(173, 219, 225, 255), new Color32(173, 219, 225, 255), new Color32(173, 219, 225, 255), new Color32(173, 219, 225, 255), new Color32(173, 219, 225, 255),
                        new Color32(173, 219, 225, 255), new Color32(173, 219, 225, 255), new Color32(178, 221, 227, 255), new Color32(143, 207, 215, 246), new Color32(104, 191, 202, 234),
                        new Color32(174, 220, 225, 255), new Color32(174, 220, 225, 255), new Color32(174, 220, 225, 255), new Color32(174, 220, 225, 255), new Color32(174, 220, 225, 255),
                        new Color32(174, 220, 225, 255), new Color32(174, 220, 225, 255), new Color32(174, 220, 225, 255), new Color32(174, 220, 225, 255), new Color32(174, 220, 225, 255),
                        new Color32(174, 220, 225, 255), new Color32(174, 220, 225, 255), new Color32(174, 220, 225, 255), new Color32(179, 222, 227, 255), new Color32(144, 208, 215, 246),
                        new Color32(105, 191, 202, 234), new Color32(176, 220, 225, 255), new Color32(176, 220, 225, 255), new Color32(176, 220, 225, 255), new Color32(176, 220, 225, 255),
                        new Color32(176, 220, 225, 255), new Color32(176, 220, 225, 255), new Color32(176, 220, 225, 255), new Color32(176, 220, 225, 255), new Color32(176, 220, 225, 255),
                        new Color32(176, 220, 225, 255), new Color32(176, 220, 225, 255), new Color32(176, 220, 225, 255), new Color32(176, 220, 225, 255), new Color32(181, 222, 227, 255),
                        new Color32(146, 208, 215, 246), new Color32(106, 191, 202, 234), new Color32(177, 220, 225, 255), new Color32(177, 220, 225, 255), new Color32(177, 220, 225, 255),
                        new Color32(177, 220, 225, 255), new Color32(177, 220, 225, 255), new Color32(177, 220, 225, 255), new Color32(177, 220, 225, 255), new Color32(177, 220, 225, 255),
                        new Color32(177, 220, 225, 255), new Color32(177, 220, 225, 255), new Color32(177, 220, 225, 255), new Color32(177, 220, 225, 255), new Color32(177, 220, 225, 255),
                        new Color32(182, 222, 227, 255), new Color32(147, 208, 215, 246), new Color32(107, 191, 202, 234), new Color32(178, 220, 225, 255), new Color32(178, 220, 225, 255),
                        new Color32(178, 220, 225, 255), new Color32(178, 220, 225, 255), new Color32(178, 220, 225, 255), new Color32(178, 220, 225, 255), new Color32(178, 220, 225, 255),
                        new Color32(178, 220, 225, 255), new Color32(178, 220, 225, 255), new Color32(178, 220, 225, 255), new Color32(178, 220, 225, 255), new Color32(178, 220, 225, 255),
                        new Color32(178, 220, 225, 255), new Color32(183, 222, 227, 255), new Color32(148, 208, 215, 246), new Color32(107, 191, 203, 234), new Color32(180, 220, 226, 255),
                        new Color32(180, 220, 226, 255), new Color32(180, 220, 226, 255), new Color32(180, 220, 226, 255), new Color32(180, 220, 226, 255), new Color32(180, 220, 226, 255),
                        new Color32(180, 220, 226, 255), new Color32(180, 220, 226, 255), new Color32(180, 220, 226, 255), new Color32(180, 220, 226, 255), new Color32(180, 220, 226, 255),
                        new Color32(180, 220, 226, 255), new Color32(180, 220, 226, 255), new Color32(185, 222, 228, 255), new Color32(149, 208, 216, 246), new Color32(108, 191, 203, 234),
                        new Color32(181, 220, 226, 255), new Color32(181, 220, 226, 255), new Color32(181, 220, 226, 255), new Color32(181, 220, 226, 255), new Color32(181, 220, 226, 255),
                        new Color32(181, 220, 226, 255), new Color32(181, 220, 226, 255), new Color32(181, 220, 226, 255), new Color32(181, 220, 226, 255), new Color32(181, 220, 226, 255),
                        new Color32(181, 220, 226, 255), new Color32(181, 220, 226, 255), new Color32(181, 220, 226, 255), new Color32(186, 222, 228, 255), new Color32(150, 208, 216, 246),
                        new Color32(109, 191, 203, 234), new Color32(182, 220, 226, 255), new Color32(182, 220, 226, 255), new Color32(182, 220, 226, 255), new Color32(182, 220, 226, 255),
                        new Color32(182, 220, 226, 255), new Color32(182, 220, 226, 255), new Color32(182, 220, 226, 255), new Color32(182, 220, 226, 255), new Color32(182, 220, 226, 255),
                        new Color32(182, 220, 226, 255), new Color32(182, 220, 226, 255), new Color32(182, 220, 226, 255), new Color32(182, 220, 226, 255), new Color32(187, 222, 228, 255),
                        new Color32(151, 208, 216, 246), new Color32(109, 191, 203, 234), new Color32(183, 221, 226, 255), new Color32(183, 221, 226, 255), new Color32(183, 221, 226, 255),
                        new Color32(183, 221, 226, 255), new Color32(183, 221, 226, 255), new Color32(183, 221, 226, 255), new Color32(183, 221, 226, 255), new Color32(183, 221, 226, 255),
                        new Color32(183, 221, 226, 255), new Color32(183, 221, 226, 255), new Color32(183, 221, 226, 255), new Color32(183, 221, 226, 255), new Color32(183, 221, 226, 255),
                        new Color32(188, 223, 228, 255), new Color32(152, 208, 216, 246), new Color32(110, 191, 203, 234), new Color32(185, 221, 226, 255), new Color32(185, 221, 226, 255),
                        new Color32(185, 221, 226, 255), new Color32(185, 221, 226, 255), new Color32(185, 221, 226, 255), new Color32(185, 221, 226, 255), new Color32(185, 221, 226, 255),
                        new Color32(185, 221, 226, 255), new Color32(185, 221, 226, 255), new Color32(185, 221, 226, 255), new Color32(185, 221, 226, 255), new Color32(185, 221, 226, 255),
                        new Color32(185, 221, 226, 255), new Color32(190, 223, 228, 255), new Color32(153, 208, 216, 246), new Color32(111, 191, 203, 234), new Color32(185, 220, 225, 255),
                        new Color32(185, 220, 225, 255), new Color32(185, 220, 225, 255), new Color32(185, 220, 225, 255), new Color32(185, 220, 225, 255), new Color32(185, 220, 225, 255),
                        new Color32(185, 220, 225, 255), new Color32(185, 220, 225, 255), new Color32(185, 220, 225, 255), new Color32(185, 220, 225, 255), new Color32(185, 220, 225, 255),
                        new Color32(185, 220, 225, 255), new Color32(185, 220, 225, 255), new Color32(190, 222, 227, 255), new Color32(153, 208, 216, 246), new Color32(114, 193, 206, 233),
                        new Color32(196, 226, 231, 255), new Color32(196, 226, 231, 255), new Color32(196, 226, 231, 255), new Color32(196, 226, 231, 255), new Color32(196, 226, 231, 255),
                        new Color32(196, 226, 231, 255), new Color32(196, 226, 231, 255), new Color32(196, 226, 231, 255), new Color32(196, 226, 231, 255), new Color32(196, 226, 231, 255),
                        new Color32(196, 226, 231, 255), new Color32(196, 226, 231, 255), new Color32(196, 226, 231, 255), new Color32(202, 229, 233, 255), new Color32(161, 212, 220, 245),
                        new Color32(77, 181, 195, 168), new Color32(126, 202, 212, 235), new Color32(129, 203, 213, 238), new Color32(128, 203, 213, 237), new Color32(128, 203, 213, 237),
                        new Color32(128, 203, 213, 237), new Color32(128, 203, 213, 237), new Color32(128, 203, 213, 237), new Color32(128, 203, 213, 237), new Color32(128, 203, 213, 237),
                        new Color32(128, 203, 213, 237), new Color32(128, 203, 213, 237), new Color32(128, 203, 213, 237), new Color32(128, 203, 213, 237), new Color32(132, 205, 214, 241),
                        new Color32(104, 192, 204, 205)
                    };

                    return new GUIStyle
                    {
                        border = new RectOffset(4, 4, 4, 4),
                        padding = new RectOffset(2, 2, 2, 2),
                        margin = new RectOffset(1, 1, 1, 1),
                        fontSize = BGDatabaseMonitorGo.I.fontSize,
                        alignment = TextAnchor.MiddleCenter,
                        clipping = TextClipping.Clip,
                        normal =
                        {
                            background = GetTexture(data, 16, 16),
                            textColor = new Color32(37, 54, 60, 255),
                        },
                    };
                });
            }
        }


        //BGResizeD.png
        public static Texture2D Resizer
        {
            get
            {
                var data = new Color32[]
                {
                    new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0),
                    new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 38), new Color32(0, 0, 0, 229), new Color32(44, 44, 44, 255), new Color32(68, 68, 68, 255), new Color32(68, 68, 68, 255),
                    new Color32(68, 68, 68, 255), new Color32(68, 68, 68, 255), new Color32(18, 18, 18, 255), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0),
                    new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 38),
                    new Color32(34, 34, 34, 232), new Color32(223, 223, 223, 255), new Color32(255, 255, 255, 255), new Color32(255, 255, 255, 255), new Color32(255, 255, 255, 255),
                    new Color32(68, 68, 68, 255), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0),
                    new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 47), new Color32(40, 40, 40, 253),
                    new Color32(250, 250, 250, 255), new Color32(255, 255, 255, 255), new Color32(255, 255, 255, 255), new Color32(68, 68, 68, 255), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0),
                    new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0),
                    new Color32(0, 0, 0, 6), new Color32(7, 7, 7, 180), new Color32(175, 175, 175, 255), new Color32(255, 255, 255, 255), new Color32(250, 250, 250, 255),
                    new Color32(255, 255, 255, 255), new Color32(68, 68, 68, 255), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0),
                    new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 8), new Color32(7, 7, 7, 183),
                    new Color32(174, 174, 174, 255), new Color32(255, 255, 255, 255), new Color32(180, 180, 180, 255), new Color32(38, 38, 38, 253), new Color32(223, 223, 223, 255),
                    new Color32(68, 68, 68, 255), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0),
                    new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 6), new Color32(7, 7, 7, 180), new Color32(174, 174, 174, 255), new Color32(255, 255, 255, 255), new Color32(174, 174, 174, 255),
                    new Color32(7, 7, 7, 186), new Color32(0, 0, 0, 47), new Color32(38, 38, 38, 233), new Color32(44, 44, 44, 255), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0),
                    new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 6), new Color32(4, 4, 4, 179),
                    new Color32(167, 167, 167, 255), new Color32(255, 255, 255, 255), new Color32(180, 180, 180, 255), new Color32(10, 10, 10, 187), new Color32(0, 0, 0, 8), new Color32(0, 0, 0, 0),
                    new Color32(0, 0, 0, 38), new Color32(0, 0, 0, 229), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0),
                    new Color32(0, 0, 0, 7), new Color32(4, 4, 4, 183), new Color32(167, 167, 167, 255), new Color32(255, 255, 255, 255), new Color32(180, 180, 180, 255), new Color32(10, 10, 10, 185),
                    new Color32(0, 0, 0, 8), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 40), new Color32(0, 0, 0, 40), new Color32(0, 0, 0, 0),
                    new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 7), new Color32(4, 4, 4, 183), new Color32(167, 167, 167, 255), new Color32(255, 255, 255, 255),
                    new Color32(181, 181, 181, 255), new Color32(10, 10, 10, 185), new Color32(0, 0, 0, 8), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0),
                    new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 232), new Color32(0, 0, 0, 43), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 6), new Color32(4, 4, 4, 178),
                    new Color32(167, 167, 167, 255), new Color32(255, 255, 255, 255), new Color32(181, 181, 181, 255), new Color32(10, 10, 10, 187), new Color32(0, 0, 0, 8), new Color32(0, 0, 0, 0),
                    new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(44, 44, 44, 255),
                    new Color32(37, 37, 37, 236), new Color32(0, 0, 0, 49), new Color32(7, 7, 7, 179), new Color32(173, 173, 173, 255), new Color32(255, 255, 255, 255),
                    new Color32(175, 175, 175, 255), new Color32(7, 7, 7, 186), new Color32(0, 0, 0, 8), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0),
                    new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(68, 68, 68, 255), new Color32(226, 226, 226, 255),
                    new Color32(39, 39, 39, 253), new Color32(166, 166, 166, 255), new Color32(255, 255, 255, 255), new Color32(181, 181, 181, 255), new Color32(10, 10, 10, 185),
                    new Color32(0, 0, 0, 8), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0),
                    new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(68, 68, 68, 255), new Color32(255, 255, 255, 255), new Color32(250, 250, 250, 255), new Color32(255, 255, 255, 255),
                    new Color32(175, 175, 175, 255), new Color32(7, 7, 7, 186), new Color32(0, 0, 0, 8), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0),
                    new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(68, 68, 68, 255),
                    new Color32(255, 255, 255, 255), new Color32(255, 255, 255, 255), new Color32(250, 250, 250, 255), new Color32(40, 40, 40, 253), new Color32(0, 0, 0, 47), new Color32(0, 0, 0, 0),
                    new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0),
                    new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(68, 68, 68, 255), new Color32(255, 255, 255, 255), new Color32(255, 255, 255, 255), new Color32(255, 255, 255, 255),
                    new Color32(226, 226, 226, 255), new Color32(38, 38, 38, 235), new Color32(0, 0, 0, 43), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0),
                    new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(18, 18, 18, 255),
                    new Color32(68, 68, 68, 255), new Color32(68, 68, 68, 255), new Color32(68, 68, 68, 255), new Color32(68, 68, 68, 255), new Color32(44, 44, 44, 255), new Color32(0, 0, 0, 232),
                    new Color32(0, 0, 0, 43), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0),
                    new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0)
                };
                return GetTexture(data, 16, 16);
            }
        }
        //parsing image 
        /*
            var fileData = File.ReadAllBytes("C:/Unity/BGButton.png");
            var tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            var data = tex.GetPixels32();
            var result = "private Color32[] data = new Color32[]{\n\r";
            var count = 0;
            for (var i = 0; i < data.Length; i++)
            {
                if (i != 0)
                {
                    result += ",";
                    count += 1;
                }

                var c = data[i];
                var color = $"new Color32({c.r},{c.g},{c.b},{c.a})";
                result += color;
                count += color.Length;
                if (count > 160)
                {
                    result+="\n\r";
                    count = 0;
                }
            }
            result += "\n\r};";
            Debug.Log(result);
        */
        public static void Reset()
        {
            box = null;
            windowTitle = null;
            editor_label = null;
            button = null;
        }
    }
}