/*
<copyright file="BGFieldAnimationCurve2017.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// AnimationCurve Field 
    /// </summary>
    [FieldDescriptor(Name = "animationCurve2017", Folder = "Unity Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerAnimationCurve2017")]
    public class BGFieldAnimationCurve2017 : BGFieldUnityClassA<AnimationCurve>
    {
        public const ushort CodeType = 59;
        
        /// <inheritdoc />
        public override ushort TypeCode => CodeType;
        
        // THIS CLASS DOES NOT SUPPORT MULTI-THREADED loading
        //safe-to-use in multi-threaded environment
        private static readonly BGBinaryReader reader = new BGBinaryReader(null);
        //safe-to-use in multi-threaded environment
        private static readonly List<Keyframe> reusableList = new List<Keyframe>();

        public override bool SupportMultiThreadedLoading => false;

        public BGFieldAnimationCurve2017(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        internal BGFieldAnimationCurve2017(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldAnimationCurve2017(meta, id, name);

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        protected override AnimationCurve FromBytes(ArraySegment<byte> segment)
        {
            reusableList.Clear();
            reader.Reset(segment);

            var preWrapMode = (WrapMode)reader.ReadInt();
            var postWrapMode = (WrapMode)reader.ReadInt();

            reader.ReadArray(() =>
            {
                reusableList.Add(new Keyframe(
                        reader.ReadFloat(),
                        reader.ReadFloat(),
                        reader.ReadFloat(),
                        reader.ReadFloat()
                    )
                    {
                        tangentMode = reader.ReadInt()
                    }
                );
            });
            var curve = new AnimationCurve(reusableList.ToArray()) { preWrapMode = preWrapMode, postWrapMode = postWrapMode };
            reusableList.Clear();
            reader.Dispose();
            return curve;
        }

        /// <inheritdoc />
        protected override void ToBytes(BGBinaryWriter writer, AnimationCurve value)
        {
            var valueKeys = value.keys;
            var keysLength = valueKeys?.Length ?? 0;

            writer.AddInt((int)value.preWrapMode);
            writer.AddInt((int)value.postWrapMode);

            writer.AddArray(() =>
            {
                foreach (var key in value.keys)
                {
                    writer.AddFloat(key.time);
                    writer.AddFloat(key.value);
                    writer.AddFloat(key.inTangent);
                    writer.AddFloat(key.outTangent);
                    writer.AddInt(key.tangentMode);
                }
            }, keysLength);
        }

        /// <inheritdoc />
        public override int MinValueSize => 12;

        /// <inheritdoc />
        protected override AnimationCurve FromString(string value)
        {
            var model = JsonUtility.FromJson<JsonModel>(value);
            var curve = new AnimationCurve { preWrapMode = model.preWrapMode, postWrapMode = model.postWrapMode };
            if (model.keys != null && model.keys.Length > 0)
                foreach (var key in model.keys)
                    curve.AddKey(key.ToKeyframe());

            return curve;
        }

        /// <inheritdoc />
        protected override string ToString(AnimationCurve value)
        {
            var jsonModel = new JsonModel { preWrapMode = value.preWrapMode, postWrapMode = value.postWrapMode };
            var keys = value.keys;
            if (keys != null && keys.Length > 0)
            {
                jsonModel.keys = new KeyframeModel[keys.Length];
                for (var i = 0; i < keys.Length; i++)
                {
                    var key = keys[i];
                    jsonModel.keys[i] = new KeyframeModel(key);
                }
            }

            return JsonUtility.ToJson(jsonModel);
        }


        [Serializable]
        private class JsonModel
        {
            public WrapMode preWrapMode;
            public WrapMode postWrapMode;
            public KeyframeModel[] keys;
        }

        [Serializable]
        private struct KeyframeModel
        {
            public float time;
            public float value;
            public float inTangent;
            public float outTangent;
            public int tangentMode;

            public KeyframeModel(Keyframe key)
            {
                time = key.time;
                value = key.value;
                inTangent = key.inTangent;
                outTangent = key.outTangent;
                tangentMode = key.tangentMode;
            }

            public Keyframe ToKeyframe() => new Keyframe(time, value, inTangent, outTangent) { tangentMode = tangentMode };
        }

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        public override AnimationCurve CloneValue(AnimationCurve value) => Clone(value);

        /// <inheritdoc />
        public override bool AreEqual(AnimationCurve myValue, AnimationCurve otherValue) => Equals(myValue, otherValue);


        public static bool Equals(AnimationCurve myValue, AnimationCurve otherValue)
        {
            if (myValue == null && otherValue == null) return true;
            if (myValue == null || otherValue == null) return false;

            if (myValue.preWrapMode != otherValue.preWrapMode) return false;
            if (myValue.postWrapMode != otherValue.postWrapMode) return false;

            var myKeys = myValue.keys;
            var otherKeys = otherValue.keys;
            if (myKeys == null && otherKeys == null) return true;
            if (myKeys == null || otherKeys == null) return false;

            if (myKeys.Length != otherKeys.Length) return false;

            for (var i = 0; i < myKeys.Length; i++)
            {
                var myKey = myKeys[i];
                var otherKey = otherKeys[i];

                if (!Mathf.Approximately(myKey.time, otherKey.time)) return false;
                if (!Mathf.Approximately(myKey.value, otherKey.value)) return false;
                if (!Mathf.Approximately(myKey.inTangent, otherKey.inTangent)) return false;
                if (!Mathf.Approximately(myKey.outTangent, otherKey.outTangent)) return false;
                if (myKey.tangentMode != otherKey.tangentMode) return false;
            }

            return true;
        }

        public static AnimationCurve Clone(AnimationCurve curve)
        {
            if (curve == null) return null;

            var curveKeys = curve.keys;
            Keyframe[] keys;
            if (curveKeys == null) keys = null;
            else
            {
                keys = new Keyframe[curveKeys.Length];
                for (var i = 0; i < curveKeys.Length; i++)
                {
                    var key = curveKeys[i];
                    keys[i] = new Keyframe(key.time, key.value, key.inTangent, key.outTangent) { tangentMode = key.tangentMode };
                }
            }

            return new AnimationCurve(keys) { preWrapMode = curve.preWrapMode, postWrapMode = curve.postWrapMode };
        }
    }
}