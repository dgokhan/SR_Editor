/*
<copyright file="BGKeyValidator.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Validator for the database key
    /// </summary>
    public class BGKeyValidator : BGValidator
    {
        private readonly BGKey key;
        private readonly List<int> list = new List<int>();
        private readonly Layer root;

        public BGKeyValidator(BGKey key)
        {
            this.key = key;
            root = new Layer(key.FindFields(), 0);
        }

        /// <summary>
        /// validate entity 
        /// </summary>
        public void Validate(BGEntity entity, Func<BGValidationLog[]> provider)
        {
            if (!key.IsUnique) return;

            if (!root.Process(entity))
            {
                list.Add(entity.Index);
                Add(provider(), "Unique Key [$] is violated", key.FullName);
            }
        }

        /// <summary>
        /// Called when all entities are validated using Validate method
        /// </summary>
        public void Finish(params BGValidationLog[] logs)
        {
            if (list.Count > 0) Add(logs, "Key [$] is unique, but following entities violates it: $", key.FullName, GetIndexesString(list));
        }

        private string GetIndexesString(List<int> list)
        {
            var builder = new StringBuilder();
            int i;
            for (i = 0; i < list.Count && i < 20; i++)
            {
                if (builder.Length != 0) builder.Append(", ");
                builder.Append(list[i]);
            }

            if (i < list.Count) builder.Append(" and (" + (list.Count - i) + ") more rows...");

            return builder.ToString();
        }

        private class Layer
        {
            private readonly List<BGField> fields;
            private readonly Dictionary<object, object> key2Value = new Dictionary<object, object>();
            private object nullables;
            private readonly int index;

            private bool IsLeaf => index == fields.Count - 1;

            internal Layer(List<BGField> fields, int index)
            {
                this.fields = fields;
                this.index = index;
            }

            internal bool Process(BGEntity entity)
            {
                var value = fields[index].GetValue(entity.Index);
                if (IsLeaf)
                {
                    if (value == null)
                    {
                        if (nullables != null) return false;
                        nullables = entity;
                        return true;
                    }

                    object existingEntity;
                    if (key2Value.TryGetValue(value, out existingEntity)) return false;
                    key2Value.Add(value, entity);
                    return true;
                }

                Layer layer;
                if (value == null)
                {
                    if (nullables == null)
                    {
                        layer = new Layer(fields, index + 1);
                        nullables = layer;
                    }
                    else layer = (Layer)nullables;
                }
                else
                {
                    if (!key2Value.TryGetValue(value, out var layerObject))
                    {
                        layer = new Layer(fields, index + 1);
                        key2Value.Add(value, layer);
                    }
                    else layer = (Layer)layerObject;
                }

                return layer.Process(entity);
            }
        }
    }
}