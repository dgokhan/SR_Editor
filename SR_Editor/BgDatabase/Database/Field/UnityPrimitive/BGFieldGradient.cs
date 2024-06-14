/*
<copyright file="BGFieldGradient.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Gradient Field 
    /// </summary>
    [FieldDescriptor(Name = "gradient", Folder = "Unity Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerGradient")]
    public class BGFieldGradient : BGFieldUnityClassA<Gradient>
    {
        public const ushort CodeType = 62;
        
        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        // THIS CLASS DOES NOT SUPPORT MULTI-THREADED loading
        //safe-to-use in multi-threaded environment
        private static readonly BGBinaryReader reader = new BGBinaryReader(null);
        //safe-to-use in multi-threaded environment
        private static readonly List<GradientColorKey> reusableList = new List<GradientColorKey>();
        //safe-to-use in multi-threaded environment
        private static readonly List<GradientAlphaKey> reusableList2 = new List<GradientAlphaKey>();

        public override bool SupportMultiThreadedLoading => false;

        public BGFieldGradient(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        internal BGFieldGradient(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldGradient(meta, id, name);

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        protected override Gradient FromBytes(ArraySegment<byte> segment)
        {
            reusableList.Clear();
            reusableList2.Clear();
            reader.Reset(segment);

            var mode = (GradientMode)reader.ReadInt();
            reader.ReadArray(() =>
            {
                reusableList.Add(new GradientColorKey(
                        new Color(
                            reader.ReadFloat(),
                            reader.ReadFloat(),
                            reader.ReadFloat(),
                            reader.ReadFloat()
                        ),
                        reader.ReadFloat()
                    )
                );
            });

            reader.ReadArray(() =>
            {
                reusableList2.Add(new GradientAlphaKey(
                        reader.ReadFloat(),
                        reader.ReadFloat()
                    )
                );
            });

            var gradient = new Gradient { mode = mode, colorKeys = reusableList.ToArray(), alphaKeys = reusableList2.ToArray() };
            reusableList.Clear();
            reusableList2.Clear();
            reader.Dispose();
            return gradient;
        }

        /// <inheritdoc />
        protected override void ToBytes(BGBinaryWriter writer, Gradient value)
        {
            var colorKeys = value.colorKeys;
            var alphaKeys = value.alphaKeys;
            var colorKeysLength = colorKeys?.Length ?? 0;
            var alphaKeysLength = alphaKeys?.Length ?? 0;

            writer.AddInt((int)value.mode);

            writer.AddArray(() =>
            {
                foreach (var key in colorKeys)
                {
                    var keyColor = key.color;
                    writer.AddFloat(keyColor.r);
                    writer.AddFloat(keyColor.g);
                    writer.AddFloat(keyColor.b);
                    writer.AddFloat(keyColor.a);
                    writer.AddFloat(key.time);
                }
            }, colorKeysLength);

            writer.AddArray(() =>
            {
                foreach (var key in alphaKeys)
                {
                    writer.AddFloat(key.alpha);
                    writer.AddFloat(key.time);
                }
            }, alphaKeysLength);
        }

        /// <inheritdoc />
        public override int MinValueSize => 12;

        /// <inheritdoc />
        protected override Gradient FromString(string value)
        {
            var model = JsonUtility.FromJson<JsonModel>(value);
            var gradient = new Gradient
            {
                mode = model.mode
            };
            if (model.colorKeys != null) gradient.colorKeys = GradientColorKeyModel.To(model.colorKeys);
            if (model.alphaKeys != null) gradient.alphaKeys = GradientAlphaKeyModel.To(model.alphaKeys);
            return gradient;
        }

        /// <inheritdoc />
        protected override string ToString(Gradient value)
        {
            var jsonModel = new JsonModel
            {
                mode = value.mode,
                colorKeys = GradientColorKeyModel.From(value.colorKeys),
                alphaKeys = GradientAlphaKeyModel.From(value.alphaKeys)
            };
            var json = JsonUtility.ToJson(jsonModel);
            return json;
        }


        //native  GradientColorKey does not work
        [Serializable]
        private class JsonModel
        {
            public GradientMode mode;
            public GradientColorKeyModel[] colorKeys;
            public GradientAlphaKeyModel[] alphaKeys;
        }

        [Serializable]
        private struct GradientColorKeyModel
        {
            public Color color;
            public float time;

            private GradientColorKeyModel(GradientColorKey valueColorKey)
            {
                color = valueColorKey.color;
                time = valueColorKey.time;
            }

            internal static GradientColorKeyModel[] From(GradientColorKey[] valueColorKeys)
            {
                if (valueColorKeys == null) return null;
                var result = new GradientColorKeyModel[valueColorKeys.Length];
                for (var i = 0; i < valueColorKeys.Length; i++)
                {
                    var valueColorKey = valueColorKeys[i];
                    result[i] = new GradientColorKeyModel(valueColorKey);
                }

                return result;
            }

            public static GradientColorKey[] To(GradientColorKeyModel[] modelColorKeys)
            {
                if (modelColorKeys == null) return null;

                var result = new GradientColorKey[modelColorKeys.Length];
                for (var i = 0; i < modelColorKeys.Length; i++)
                {
                    var modelColorKey = modelColorKeys[i];
                    result[i] = new GradientColorKey(modelColorKey.color, modelColorKey.time);
                }

                return result;
            }
        }

        [Serializable]
        private struct GradientAlphaKeyModel
        {
            public float alpha;
            public float time;

            private GradientAlphaKeyModel(GradientAlphaKey valueAlphaKey)
            {
                alpha = valueAlphaKey.alpha;
                time = valueAlphaKey.time;
            }

            internal static GradientAlphaKeyModel[] From(GradientAlphaKey[] valueAlphaKeys)
            {
                if (valueAlphaKeys == null) return null;
                var result = new GradientAlphaKeyModel[valueAlphaKeys.Length];
                for (var i = 0; i < valueAlphaKeys.Length; i++)
                {
                    var valueColorKey = valueAlphaKeys[i];
                    result[i] = new GradientAlphaKeyModel(valueColorKey);
                }

                return result;
            }

            public static GradientAlphaKey[] To(GradientAlphaKeyModel[] modelColorKeys)
            {
                if (modelColorKeys == null) return null;

                var result = new GradientAlphaKey[modelColorKeys.Length];
                for (var i = 0; i < modelColorKeys.Length; i++)
                {
                    var modelColorKey = modelColorKeys[i];
                    result[i] = new GradientAlphaKey(modelColorKey.alpha, modelColorKey.time);
                }

                return result;
            }
        }


        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        public override Gradient CloneValue(Gradient value) => Clone(value);

        /// <inheritdoc />
        public override bool AreEqual(Gradient myValue, Gradient otherValue) => Equals(myValue, otherValue);


        public static bool Equals(Gradient myValue, Gradient otherValue)
        {
            if (myValue == null && otherValue == null) return true;
            if (myValue == null || otherValue == null) return false;

            if (myValue.mode != otherValue.mode) return false;

            var myKeys = myValue.colorKeys;
            var otherKeys = otherValue.colorKeys;
            if (myKeys == null && otherKeys == null) return true;
            if (myKeys == null || otherKeys == null) return false;

            if (myKeys.Length != otherKeys.Length) return false;

            for (var i = 0; i < myKeys.Length; i++)
            {
                var myKey = myKeys[i];
                var otherKey = otherKeys[i];

                if (!Mathf.Approximately(myKey.time, otherKey.time)) return false;
                var myKeyColor = myKey.color;
                var otherKeyColor = otherKey.color;
                if (!Mathf.Approximately(myKeyColor.r, otherKeyColor.r)) return false;
                if (!Mathf.Approximately(myKeyColor.g, otherKeyColor.g)) return false;
                if (!Mathf.Approximately(myKeyColor.b, otherKeyColor.b)) return false;
                if (!Mathf.Approximately(myKeyColor.a, otherKeyColor.a)) return false;
            }


            var myAlphaKeys = myValue.alphaKeys;
            var otherAlphaKeys = otherValue.alphaKeys;
            if (myAlphaKeys == null && otherAlphaKeys == null) return true;
            if (myAlphaKeys == null || otherAlphaKeys == null) return false;

            if (myAlphaKeys.Length != otherAlphaKeys.Length) return false;

            for (var i = 0; i < myAlphaKeys.Length; i++)
            {
                var myKey = myAlphaKeys[i];
                var otherKey = otherAlphaKeys[i];

                if (!Mathf.Approximately(myKey.time, otherKey.time)) return false;
                if (!Mathf.Approximately(myKey.alpha, otherKey.alpha)) return false;
            }


            return true;
        }

        public static Gradient Clone(Gradient gradient)
        {
            if (gradient == null) return null;

            var colorKeys = gradient.colorKeys;
            GradientColorKey[] newColorKeys;
            if (colorKeys == null) newColorKeys = null;
            else
            {
                newColorKeys = new GradientColorKey[colorKeys.Length];
                for (var i = 0; i < colorKeys.Length; i++)
                {
                    var key = colorKeys[i];
                    newColorKeys[i] = new GradientColorKey(key.color, key.time);
                }
            }

            var alphaKeys = gradient.alphaKeys;
            GradientAlphaKey[] newAlphaKeys;
            if (alphaKeys == null) newAlphaKeys = Array.Empty<GradientAlphaKey>();
            else
            {
                newAlphaKeys = new GradientAlphaKey[alphaKeys.Length];
                for (var i = 0; i < alphaKeys.Length; i++)
                {
                    var key = alphaKeys[i];
                    newAlphaKeys[i] = new GradientAlphaKey(key.alpha, key.time);
                }
            }

            return new Gradient { mode = gradient.mode, colorKeys = newColorKeys, alphaKeys = newAlphaKeys };
        }
    }
}