/*
<copyright file="BGMetaViewMappings.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// all view mappings
    /// </summary>
    public class BGMetaViewMappings
    {
        private readonly HashSet<BGId> includedMetas = new HashSet<BGId>();
        private readonly BGMetaView view;

        public BGMetaView View => view;

        /// <summary>
        /// included metas IDs
        /// </summary>
        public BGId[] IncludedMetas
        {
            get => includedMetas.ToArray();
            /*
            private set
            {
                if (value == null)
                {
                    var fireEvent = includedMetas.Count != 0;
                    includedMetas.Clear();
                    if (fireEvent) view.FireViewChanged();
                }
                else
                {
                    var anyChange = includedMetas.Count != value.Length;
                    if (!anyChange) anyChange = !includedMetas.SetEquals(value);
                    if (!anyChange) return;
                    includedMetas.Clear();
                    foreach (var id in value) includedMetas.Add(id);
                    View.FireViewChanged();
                }
            }
        */
        }

        public BGMetaViewMappings(BGMetaView view) => this.view = view;

        //==========================================================================
        //                          Mappings
        //==========================================================================
        /// <summary>
        /// table mappings count
        /// </summary>
        public int MappingsCount => includedMetas.Count;


        /// <summary>
        /// iterate over each table mapping
        /// </summary>
        public bool IsIncluded(BGId metaId) => includedMetas.Contains(metaId);

        /// <summary>
        /// add table to the view
        /// </summary>
        public void Add(BGId metaId)
        {
            if (includedMetas.Add(metaId)) View.FireViewChanged();
        }

        /// <summary>
        /// remove table from the view
        /// </summary>
        public void Remove(BGId metaId)
        {
            if (!includedMetas.Remove(metaId)) return;

            var meta = View.Repo.GetMeta(metaId);
            if (meta != null)
            {
                var relations = View.RelationsInbound;
                foreach (var relation in relations)
                {
                    switch (relation)
                    {
                        case BGFieldViewRelationSingle relationSingle:
                        {
                            relationSingle.RemoveRelatedMeta(meta);
                            break;
                        }
                        case BGFieldViewRelationMultiple relationMultiple:
                        {
                            relationMultiple.RemoveRelatedMeta(meta);
                            break;
                        }
                    }
                }
            }
            
            View.FireViewChanged();
        }

        //==========================================================================
        //                          Synchronization
        //==========================================================================
        /// <summary>
        /// remove settings for all non-existent members 
        /// </summary>
        public void Trim()
        {
            List<BGId> toRemove = null;
            foreach (var metaId in includedMetas)
            {
                if (view.Repo.HasMeta(metaId)) continue;
                toRemove = toRemove ?? new List<BGId>();
                toRemove.Add(metaId);
            }

            if (toRemove != null)
            {
                foreach (var metaId in toRemove) Remove(metaId);
            }
        }

        /// <summary>
        /// Check the current status and throws exception is mapping have any error
        /// </summary>
        public void CheckStatus(BGMetaEntity meta)
        {
            if (!IsIncluded(meta.Id)) return;

            view.DelegateMeta.ForEachField(viewField =>
            {
                var field = meta.GetField(viewField.Name, false);
                if (field == null) throw new Exception($"View [{view.Name}] mapping error for table [{meta.Name}]: field [{viewField.Name}] is not found ");
                if (field.ValueType != viewField.ValueType)
                    throw new Exception($"View [{view.Name}] mapping error for table [{meta.Name}]: " +
                                        $"field [{viewField.Name}] has wrong type, expected [{viewField.ValueType.FullName}] actual [{field.ValueType.FullName}]");
                if (!viewField.ReadonlyFinal && field.ReadonlyFinal)
                    throw new Exception($"View [{view.Name}] mapping error for table [{meta.Name}]: " +
                                        $"field [{field.FullName}] should not be readonly");

                if (!(viewField is BGFieldUnityAssetI))
                {
                    var viewConfig = viewField.ConfigToString();
                    var fieldConfig = field.ConfigToString();
                    if (!string.Equals(viewConfig, fieldConfig))
                        throw new Exception($"View [{view.Name}] mapping error for table [{meta.Name}]: " +
                                            $"field [{field.FullName}] has incompatible configuration (field's settings)");
                }
            });
        }

        /// <summary>
        /// clone mappings 
        /// </summary>
        public void CloneTo(BGMetaViewMappings cloneMappings)
        {
            cloneMappings.Clear();
            foreach (var metaId in includedMetas) cloneMappings.Add(metaId);
        }

        private void Clear() => includedMetas.Clear();

        /// <summary>
        /// are mappings parameters equal? 
        /// </summary>
        public bool DeepEqual(BGMetaViewMappings t2) => includedMetas.SetEquals(t2.includedMetas);

        
        /// <summary>
        /// ensures the mapping are equal 
        /// </summary>
        public void TransferFrom(BGMetaViewMappings t2)
        {
            List<BGId> toRemove = null;
            foreach (var metaId in includedMetas)
            {
                if(t2.IsIncluded(metaId)) continue;
                toRemove = toRemove ?? new List<BGId>();
                toRemove.Add(metaId);
            }

            //add
            foreach (var metaId in t2.includedMetas)
            {
                if(IsIncluded(metaId)) continue;
                Add(metaId);
            }

            //remove
            if (toRemove != null)
            {
                foreach (var metaId in toRemove) Remove(metaId);
            }
        }

        public string MappingsToString()
        {
            var result = "";
            foreach (var metaId in includedMetas)
            {
                if (result.Length != 0) result += ", ";
                result += metaId;
            }

            return result;
        }
    }
}