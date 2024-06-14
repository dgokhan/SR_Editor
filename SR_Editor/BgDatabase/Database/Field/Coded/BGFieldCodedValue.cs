using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public class BGFieldCodedValue : BGFieldDictionaryClonebleValueI
    {
        private readonly BGField field;
        private readonly BGEntity entity;

        private string delegateClass;

        public string DelegateClass
        {
            get => delegateClass;
            set
            {
                if (value == delegateClass) return;
                delegateClass = value;
                @delegate = null;
                FireChange();
            }
        }

        private BGCodedFieldDelegateI @delegate;

        private BGCodedFieldDelegateI Delegate
        {
            get
            {
                if (@delegate != null) return @delegate;
                var type = BGUtil.GetType(delegateClass);
                if (type == null) return null;
                try
                {
                    @delegate = (BGCodedFieldDelegateI)Activator.CreateInstance(type);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                return @delegate;
            }
        }

        public BGCodedFieldDelegateI DelegateInstance => Delegate;
        
        public BGFieldCodedValue(BGField field, BGEntity entity)
        {
            this.field = field ?? throw new Exception("field can not be null");
            this.entity = entity ?? throw new Exception("entity can not be null");
        }

        //================================================================================================
        //                                              Methods
        //================================================================================================
        /// <inheritdoc/>
        public object CloneTo(BGEntity e)
        {
            return new BGFieldCodedValue(field, e)
            {
                delegateClass = delegateClass
            };
        }


        public T Call<T>(BGFieldCodedA<T> fieldCodedA, BGEntity entity)
        {
            var manager = Delegate;
            if (manager == null) throw new Exception($"Can not create delegate class for programmable field value, class name is [{delegateClass}]");
            if (!(manager is BGCodedFieldDelegateI<T> typedManager))
                throw new Exception($"Can not cast delegate instance to generic interface BGCodedFieldDelegateI<T>, " +
                                    $"generic type is is [{typeof(T).FullName}]");

            using (var context = BGCodedFieldContext.Get())
            {
                context.Field = fieldCodedA;
                context.Entity = entity;
                return typedManager.Get(context);
            }
        }
        private void FireChange() => field.FireValueChanged(entity);
        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <summary>
        /// convert this object state to binary array 
        /// </summary>
        public byte[] ToBytes()
        {
            var writer = new BGBinaryWriter();

            //version
            writer.AddByte(1);

            //delegate class
            writer.AddString(delegateClass);

            return writer.ToArray();
        }

        /// <summary>
        /// restore this object state from binary array 
        /// </summary>
        public void FromBytes(ArraySegment<byte> content)
        {
            var reader = new BGBinaryReader(content);

            var version = reader.ReadByte();
            switch (version)
            {
                case 1:
                {
                    delegateClass = reader.ReadString();
                    @delegate = null;
                    break;
                }
                default:
                    throw new Exception("Unsupported version " + version);
            }
        }

        /// <summary>
        /// convert this object to JSON string 
        /// </summary>
        public string ToJsonString()
        {
            var result = new JsonConfig(){DelegateClass = delegateClass};
            return JsonUtility.ToJson(result);
        }

        /// <summary>
        /// restore this objects state from JSON string 
        /// </summary>
        public void FromJsonString(string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            var config = JsonUtility.FromJson<JsonConfig>(value);
            delegateClass = config.DelegateClass;
            @delegate = null;
        }
        
        //================================================================================================
        //                                              Equality members
        //================================================================================================
        protected bool Equals(BGFieldCodedValue other)
        {
            return delegateClass == other.delegateClass;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BGFieldCodedValue)obj);
        }

        public override int GetHashCode()
        {
            return (delegateClass != null ? delegateClass.GetHashCode() : 0);
        }

        public static bool operator ==(BGFieldCodedValue left, BGFieldCodedValue right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BGFieldCodedValue left, BGFieldCodedValue right)
        {
            return !Equals(left, right);
        }

        //================================================================================================
        //                                              Nested classes
        //================================================================================================
        [Serializable]
        private class JsonConfig
        {
            public string DelegateClass;
        }

    }
}