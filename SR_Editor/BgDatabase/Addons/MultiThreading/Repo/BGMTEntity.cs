/*
<copyright file="BGMTEntity.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Multi-threaded row
    /// </summary>
    public struct BGMTEntity
    {
        /// <summary>
        /// entity's table 
        /// </summary>
        public readonly BGMTMeta Meta;

        private readonly int index;

        /// <summary>
        /// entity's id
        /// </summary>
        public BGId Id => Meta.GetEntityId(index);

        /// <summary>
        /// is this entity marked for removal. Removal takes place at the end of transaction 
        /// </summary>
        public bool IsDeleted => Meta.IsDeleted(index);

        /// <summary>
        /// entity index 
        /// </summary>
        public int Index => index;

        public string Name
        {
            get => Get<string>(BGFieldEntityName.NameFieldName);
            set => Set(BGFieldEntityName.NameFieldName, value);
        }

        //readonly
        public BGMTEntity(BGMTMeta meta, int index)
        {
            this.index = index;
            Meta = meta;
        }

        //================================================================================================
        //                                              GetValue
        //================================================================================================
        /// <summary>
        /// get field's value
        /// </summary>
        public T Get<T>(string fieldName)
        {
            return Meta.GetField<T>(fieldName)[index];
        }

        /// <summary>
        /// get field's value
        /// </summary>
        public T Get<T>(int fieldIndex)
        {
            return Meta.GetField<T>(fieldIndex)[index];
        }

        /// <summary>
        /// get field's value
        /// </summary>
        public T Get<T>(BGId fieldId)
        {
            return Meta.GetField<T>(fieldId)[index];
        }

        //================================================================================================
        //                                              SetValue
        //================================================================================================
        /// <summary>
        /// Set field's value
        /// </summary>
        public void Set<T>(string fieldName, T value)
        {
            Meta.Set(Meta.GetField(fieldName).Index, index, value);
        }

        /// <summary>
        /// Set field's value
        /// </summary>
        public void Set<T>(int fieldIndex, T value)
        {
            Meta.Set(fieldIndex, index, value);
        }

        /// <summary>
        /// Set field's value
        /// </summary>
        public void Set<T>(BGId fieldId, T value)
        {
            Meta.Set(Meta.GetField(fieldId).Index, index, value);
        }

        //================================================================================================
        //                                              Delete
        //================================================================================================
        /// <summary>
        /// Mark entity for removal. The entity is not removed immediately, the actual removal from database takes place at the end of transaction
        /// </summary>
        public void Delete()
        {
            Meta.Delete(index);
        }

        //================================================================================================
        //                                              Misc
        //================================================================================================

        public override string ToString()
        {
            return Meta.Name + ".Entity #" + index;
        }
    }
}