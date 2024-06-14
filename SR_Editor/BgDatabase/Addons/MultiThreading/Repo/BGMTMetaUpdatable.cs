/*
<copyright file="BGMTMetaUpdatable.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Updatable Multi-threaded table
    /// </summary>
    public class BGMTMetaUpdatable : BGMTMeta
    {
        private bool[] updatedField;
        private bool fieldListsAreReplaced;
        private bool entitiesListsAreReplaced;

        private HashSet<int> deletedEntities;

        public HashSet<int> DeletedEntities => deletedEntities;

        internal BGMTMetaUpdatable(BGMTMeta meta) : base(meta)
        {
            updatedField = new bool[fields.Length];
        }

        //================================================================================================
        //                                              Write
        //================================================================================================
        protected internal override void Set<T>(int fieldIndex, int entityIndex, T value)
        {
            ReplaceField((BGMTField<T>)GetField(fieldIndex))[entityIndex] = value;
        }

        protected internal override void Delete(int entityIndex)
        {
            if (deletedEntities == null) deletedEntities = new HashSet<int>();
            deletedEntities.Add(entityIndex);
        }

        protected internal override bool IsDeleted(int entityIndex)
        {
            return deletedEntities != null && deletedEntities.Contains(entityIndex);
        }

        protected internal override void ApplyDelete()
        {
            if (deletedEntities == null) return;

            ReplaceFieldLists();
            ReplaceEntitiesLists();

            var toRemoveArray = new int[deletedEntities.Count];
            deletedEntities.CopyTo(toRemoveArray);
            Array.Sort(toRemoveArray);

            var from = 0;
            var numberToRemove = 0;
            var shift = 0;
            for (var i = 0; i < toRemoveArray.Length; i++)
            {
                var toRemove = toRemoveArray[i];
                if (from + numberToRemove == toRemove) numberToRemove++;
                else
                {
                    if (numberToRemove > 0)
                    {
                        RemoveRange(@from - shift, numberToRemove);
                        shift += numberToRemove;
                    }

                    from = toRemove;
                    numberToRemove = 1;
                }
            }

            if (numberToRemove > 0) RemoveRange(@from - shift, numberToRemove);
        }

        private void RemoveRange(int @from, int numberToRemove)
        {
            var upperLimit = @from + numberToRemove;
            for (var i = @from; i < upperLimit; i++) entityId2Index.Remove(entityIds[i]);
            var entityIdsCount = entityIds.Count;
            for (var i = upperLimit; i < entityIdsCount; i++) entityId2Index[entityIds[i]] = i - numberToRemove;

            entityIds.RemoveRange(@from, numberToRemove);
            for (var j = 0; j < fields.Length; j++) ReplaceField(fields[j]).RemoveRange(@from, numberToRemove);
        }

        protected internal override void Dispose()
        {
            base.Dispose();
            updatedField = null;
            deletedEntities = null;
        }

        public override int NewEntities(int numberOfEntities = 1)
        {
            if (numberOfEntities < 1) throw new BGException("Number of entities can not be zero or negative");

            ReplaceEntitiesLists();
            var oldCount = entityIds.Count;
            for (var i = 0; i < numberOfEntities; i++)
            {
                var newId = BGId.NewId;
                entityId2Index.Add(newId, oldCount + i);
                entityIds.Add(newId);
            }

            for (var i = 0; i < fields.Length; i++)
            {
                var field = ReplaceField(fields[i]);
                field.ResizeTo(oldCount + numberOfEntities);
            }

            return oldCount;
        }

        //================================================================================================
        //                                              Utilities
        //================================================================================================

        private BGMTField<T> ReplaceField<T>(BGMTField<T> existingField)
        {
            return (BGMTField<T>)ReplaceField((BGMTField)existingField);
        }

        private BGMTField ReplaceField(BGMTField existingField)
        {
            if (updatedField[existingField.Index]) return existingField;

            ReplaceFieldLists();
            var field = existingField.DeepClone(this);
            updatedField[field.Index] = true;
            fields[field.Index] = field;
            id2Field[field.Id] = field;
            name2Field[field.Name] = field;
            return field;
        }


        private void ReplaceFieldLists()
        {
            if (fieldListsAreReplaced) return;
            fieldListsAreReplaced = true;

            fields = CloneArray(fields);
            id2Field = new BGIdDictionary<BGMTField>(id2Field);
            name2Field = new Dictionary<string, BGMTField>(name2Field);
        }

        private void ReplaceEntitiesLists()
        {
            if (entitiesListsAreReplaced) return;
            entitiesListsAreReplaced = true;

            entityIds = new List<BGId>(entityIds);
            entityId2Index = new BGIdDictionary<int>(entityId2Index);
        }

        private T[] CloneArray<T>(T[] source)
        {
            return CloneArray(source, source.Length);
        }

        private T[] CloneArray<T>(T[] source, int newSize)
        {
            var result = new T[newSize];
            Array.Copy(source, result, source.Length);
            return result;
        }
    }
}