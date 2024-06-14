
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// AnimationCurve Field for Unity 2020 
    /// </summary>
    [FieldDescriptor(Name = "animationCurve2020", Folder = "Unity Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerAnimationCurve2020")]
    public class BGFieldAnimationCurve2020 : BGFieldUnityClassA<AnimationCurve>
    {
        // THIS CLASS DOES NOT SUPPORT MULTI-THREADED loading
        private static readonly BGBinaryReader reader = new BGBinaryReader(null);
        private static readonly List<Keyframe> reusableList = new List<Keyframe>();

        public const ushort CodeType = 107;

        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        public override bool SupportMultiThreadedLoading => false;

        public BGFieldAnimationCurve2020(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        internal BGFieldAnimationCurve2020(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldAnimationCurve2020(meta, id, name);

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        protected override AnimationCurve FromBytes(ArraySegment<byte> segment)
        {
            reusableList.Clear();
            reader.Reset(segment);

            var preWrapMode = (WrapMode)reader.ReadInt();
            var postWrapMode = (WrapMode)reader.ReadInt();

            reader.ReadArray(() =>
            {
                var keyframe = new Keyframe(
                    reader.ReadFloat(),
                    reader.ReadFloat(),
                    reader.ReadFloat(),
                    reader.ReadFloat(),
                    reader.ReadFloat(),
                    reader.ReadFloat()
                );
#pragma warning disable 618
                keyframe.tangentMode = reader.ReadInt();
#pragma warning restore 618
                keyframe.weightedMode = (WeightedMode)reader.ReadInt();
                reusableList.Add(keyframe);
            });

            var curve = new AnimationCurve(reusableList.ToArray()) { preWrapMode = preWrapMode, postWrapMode = postWrapMode };
            reusableList.Clear();
            reader.Dispose();
            return curve;
        }

        protected override void ToBytes(BGBinaryWriter writer, AnimationCurve value)
        {
            var valueKeys = value.keys;
            var keysLength = valueKeys == null ? 0 : valueKeys.Length;

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
                    writer.AddFloat(key.inWeight);
                    writer.AddFloat(key.outWeight);
#pragma warning disable 618
                    writer.AddInt(key.tangentMode);
#pragma warning restore 618
                    writer.AddInt((int)key.weightedMode);
                }
            }, keysLength);
        }

        public override int MinValueSize => 12;

        protected override AnimationCurve FromString(string value)
        {
            var model = JsonUtility.FromJson<JsonModel>(value);
            var curve = new AnimationCurve { preWrapMode = model.preWrapMode, postWrapMode = model.postWrapMode };
            if (model.keys != null && model.keys.Length > 0)
            {
                foreach (var key in model.keys) curve.AddKey(key.ToKeyframe());
            }

            return curve;
        }

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

            //these fields were added in Unity 2018
            public int weightedMode;
            public float inWeight;
            public float outWeight;

            public KeyframeModel(Keyframe key)
            {
                time = key.time;
                value = key.value;
                inTangent = key.inTangent;
                outTangent = key.outTangent;
#pragma warning disable 618
                tangentMode = key.tangentMode;
#pragma warning restore 618
                inWeight = key.inWeight;
                outWeight = key.outWeight;
                weightedMode = (int)key.weightedMode;
            }

            public Keyframe ToKeyframe()
            {
#pragma warning disable 618
                return new Keyframe(time, value, inTangent, outTangent, inWeight, outWeight)
                {
                    tangentMode = tangentMode,
                    weightedMode = (WeightedMode)weightedMode
                };
#pragma warning restore 618
            }
        }

        //================================================================================================
        //                                              Value
        //================================================================================================
        public override AnimationCurve CloneValue(AnimationCurve value) => Clone(value);

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

                if (!Mathf.Approximately(myKey.inWeight, otherKey.inWeight)) return false;
                if (!Mathf.Approximately(myKey.outWeight, otherKey.outWeight)) return false;

#pragma warning disable 618
                if (myKey.tangentMode != otherKey.tangentMode) return false;
#pragma warning restore 618
                if (myKey.weightedMode != otherKey.weightedMode) return false;
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
#pragma warning disable 618
                    keys[i] = new Keyframe(key.time, key.value, key.inTangent, key.outTangent, key.inWeight, key.outWeight)
                    {
                        tangentMode = key.tangentMode,
                        weightedMode = key.weightedMode
                    };
#pragma warning restore 618
                }
            }

            return new AnimationCurve(keys) { preWrapMode = curve.preWrapMode, postWrapMode = curve.postWrapMode };
        }
    }
}