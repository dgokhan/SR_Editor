/*
<copyright file="BGFieldCalcValue.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// calculated field value
    /// </summary>
    public class BGFieldCalcValue : BGFieldDictionaryClonebleValueI, BGCalcVarsProvider
    {
        private readonly BGField field;
        private readonly BGEntity entity;

        private BGCalcGraph graph;
        private BGFieldCalcVarRef.VarRefContainer vars;

        /*
        public List<BGFieldCalcVarRef> Vars
        {
            get => vars;
            set => vars = value;
        }
        */

        /// <summary>
        /// Get the graph if any
        /// </summary>
        public BGCalcGraph Graph
        {
            get => graph;
            set
            {
                if (graph == value) return;
                if (graph != null) graph.OnAnyChange -= OnGraphChange;
                graph = value;
                if (graph != null) graph.OnAnyChange += OnGraphChange;
                FireChange();
            }
        }

        private void OnGraphChange() => FireChange();

        /// <summary>
        /// Does this value override default graph or variables
        /// </summary>
        public bool IsEmpty => (vars == null || vars.Count == 0) && Graph == null;

        /// <summary>
        /// database field
        /// </summary>
        public BGField Field => field;

        /// <summary>
        /// database row
        /// </summary>
        public BGEntity Entity => entity;

        public BGFieldCalcValue(BGField field, BGEntity entity)
        {
            this.field = field ?? throw new Exception("field can not be null");
            this.entity = entity ?? throw new Exception("entity can not be null");
        }

        //================================================================================================
        //                                              Variables
        //================================================================================================

        //BGCalcVarsProvider
        /// <inheritdoc/>
        public bool TryGet(BGId variableId, out object value)
        {
            value = null;
            if (vars == null) return false;
            for (var i = 0; i < vars.Count; i++)
            {
                var variable = vars[i];
                if (variable.Id != variableId) continue;
                value = variable.Value;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Remove the variable with provided ID
        /// </summary>
        public int RemoveVar(BGId id)
        {
            if (vars == null) return 0;
            var counter = 0;
            for (var i = vars.Count - 1; i >= 0; i--)
            {
                var varRef = vars[i];
                if (varRef.Id != id) continue;
                vars.RemoveAt(i);
                counter++;
            }

            return counter;
        }

        /// <summary>
        /// add variable with provided ID
        /// </summary>
        public BGFieldCalcVarRef AddVar(BGId id)
        {
            if (vars == null) vars = BGFieldCalcVarRef.NewContainer();
            if (GetVar(id) != null) throw new Exception("Var with such id=" + id + " already added");
            return vars.NewVar(id);
        }

        /// <summary>
        /// Find the first variable using provided filter 
        /// </summary>
        public BGFieldCalcVarRef FindVar(Predicate<BGFieldCalcVarRef> filter)
        {
            if (vars == null) return null;
            for (var i = 0; i < vars.Count; i++)
            {
                var varRef = vars[i];
                if (filter == null || filter(varRef)) return varRef;
            }

            return null;
        }

        /// <summary>
        /// Get variable by its ID
        /// </summary>
        public BGFieldCalcVarRef GetVar(BGId id)
        {
            if (vars == null) return null;
            for (var i = 0; i < vars.Count; i++)
            {
                var varRef = vars[i];
                if (varRef.Id == id) return varRef;
            }

            return null;
        }

        private List<Tuple<BGFieldCalcVarRef, BGCalcTypeCode>> GetExistingVarsOverrides(BGCalcGraph parentGraph)
        {
            if (parentGraph?.GetVars() == null || vars == null) return null;

            List<Tuple<BGFieldCalcVarRef, BGCalcTypeCode>> existingVarsOverrides = null;
            var parentGraphVars = parentGraph.GetVars();
            vars.ForEach(varRef =>
            {
                var parentVar = parentGraphVars.GetVar(varRef.Id);
                if (parentVar == null) return;
                existingVarsOverrides = existingVarsOverrides ?? new List<Tuple<BGFieldCalcVarRef, BGCalcTypeCode>>();
                existingVarsOverrides.Add(Tuple.Create(varRef, parentVar.TypeCode));
            });

            return existingVarsOverrides;
        }

        private bool CheckVar(BGId id, BGCalcTypeCode typeCode, BGCalcVarContainer parentVars)
        {
            var parentVar = parentVars.GetVar(id);
            if (parentVar == null)
            {
                Debug.Log("Can not find variable with id " + id + ", the overriden var value will be lost");
                return false;
            }

            if (!Equals(parentVar.TypeCode, typeCode))
            {
                Debug.Log("Variable " + parentVar.Name + " changed type from " + typeCode.Name + " to " + parentVar.TypeCode.Name + ". Variable override value will be lost");
                return false;
            }

            return true;
        }

        //================================================================================================
        //                                              BGFieldDictionaryClonebleValueI
        //================================================================================================
        /// <inheritdoc/>
        public object CloneTo(BGEntity e)
        {
            var clone = new BGFieldCalcValue(field, e);
            if (vars != null && vars.Count > 0)
            {
                clone.vars = new BGFieldCalcVarRef.VarRefContainer();
                vars.ForEach(varRef => varRef.CloneTo(clone.vars));
            }

            if (graph != null) clone.graph = (BGCalcGraph)graph.Clone();
            return clone;
        }

        /*public T Execute<T>(BGEntity entity)
        {
            if (graph != null) return graph.Execute<T>(entity);
            return default(T);
        }*/

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <summary>
        /// convert this object state to binary array 
        /// </summary>
        public byte[] ToBytes(BGCalcGraph parentGraph)
        {
            var existingVarsOverrides = GetExistingVarsOverrides(parentGraph);
            if (existingVarsOverrides == null && graph == null) return null;

            var writer = new BGBinaryWriter();

            //version
            writer.AddByte(1);

            if (graph != null)
            {
                //has graph
                writer.AddByte(1);
                //graph
                writer.AddByteArray(graph.ToBytes());
            }
            else
            {
                //no graph but vars overrides
                writer.AddByte(2);
                //vars overrides
                writer.AddArray(() =>
                {
                    foreach (var tuple in existingVarsOverrides)
                    {
                        writer.AddId(tuple.Item1.Id);
                        writer.AddByte(tuple.Item2.TypeCode);
                        tuple.Item2.ValueToBytes(writer, tuple.Item1.Value);
                    }
                }, existingVarsOverrides.Count);
            }

            return writer.ToArray();
        }

        /// <summary>
        /// restore this object state from binary array 
        /// </summary>
        public void FromBytes(ArraySegment<byte> content, BGCalcGraph parentGraph)
        {
            var reader = new BGBinaryReader(content);

            var version = reader.ReadByte();
            switch (version)
            {
                case 1:
                {
                    var state = reader.ReadByte();
                    switch (state)
                    {
                        case 1:
                        {
                            //graph
                            graph = BGCalcGraph.ExistingGraph();
                            graph.FromBytes(reader.ReadByteArray());
                            graph.OnAnyChange += OnGraphChange;
                            break;
                        }
                        case 2:
                        {
                            //vars overrides
                            reader.ReadArray(() =>
                            {
                                var id = reader.ReadId();
                                var typeCodeByte = reader.ReadByte();
                                var typeCode = BGCalcTypeCodeRegistry.Get(typeCodeByte);
                                var value = typeCode.ValueFromBytes(reader);
                                if (!CheckVar(id, typeCode, parentGraph.GetVars(true))) return;

                                vars = EnsureContainer();
                                vars.NewVar(id, value);
                            });

                            break;
                        }
                        default:
                            throw new Exception("Unknown graph value state " + state);
                    }

                    break;
                }
                default:
                    throw new Exception("Unsupported version " + version);
            }
        }

        private BGFieldCalcVarRef.VarRefContainer EnsureContainer()
        {
            if (vars != null) return vars;
            vars = BGFieldCalcVarRef.NewContainer();
            vars.OnAnyChange += FireChange;
            return vars;
        }

        /// <summary>
        /// convert this object to JSON string 
        /// </summary>
        public string ToJsonString(BGCalcGraph parentGraph)
        {
            var result = new JsonConfig();
            if (graph != null) result.Graph = new BGCalcGraphModel(graph);
            if (vars != null && vars.Count > 0 && parentGraph != null && parentGraph.GetVars() != null)
            {
                var existingVarsOverrides = GetExistingVarsOverrides(parentGraph);
                if (existingVarsOverrides != null)
                {
                    result.Vars = new List<CalcVarModel>();
                    foreach (var tuple in existingVarsOverrides)
                        result.Vars.Add(new CalcVarModel
                        {
                            Id = tuple.Item1.Id.ToString(),
                            CodeRef = tuple.Item2.TypeCode,
                            Value = tuple.Item2.ValueToString(tuple.Item1.Value)
                        });
                }
            }

            return JsonUtility.ToJson(result);
        }

        /// <summary>
        /// restore this objects state from JSON string 
        /// </summary>
        public void FromJsonString(string value, BGCalcGraph parentGraph)
        {
            if (string.IsNullOrEmpty(value)) return;
            var config = JsonUtility.FromJson<JsonConfig>(value);
            if (config.Vars != null && config.Vars.Count > 0 && parentGraph != null && parentGraph.GetVars() != null)
            {
                vars = BGFieldCalcVarRef.NewContainer();
                var parentVars = parentGraph.GetVars();
                foreach (var variable in config.Vars)
                {
                    var id = BGId.Parse(variable.Id);
                    var typeCode = BGCalcTypeCodeRegistry.Get(variable.CodeRef);
                    if (!CheckVar(id, typeCode, parentVars)) return;
                    vars.NewVar(id, typeCode.ValueFromString(variable.Value));
                }
            }

            if (config.Graph?.units != null && config.Graph.units.Count > 0)
            {
                graph = BGCalcGraph.ExistingGraph();
                config.Graph.ToGraph(graph);
                graph.OnAnyChange += OnGraphChange;
            }
        }

        private void FireChange() => field.FireValueChanged(entity);

        //================================================================================================
        //                                              Equality members
        //================================================================================================
        protected bool Equals(BGFieldCalcValue other)
        {
            return Equals(field, other.field) && Equals(entity, other.entity) && Equals(graph, other.graph) && Equals(vars, other.vars);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BGFieldCalcValue)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = field != null ? field.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (entity != null ? entity.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (graph != null ? graph.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (vars != null ? vars.GetHashCode() : 0);
                return hashCode;
            }
        }


        //================================================================================================
        //                                              Nested classes
        //================================================================================================
        [Serializable]
        private class JsonConfig
        {
            public List<CalcVarModel> Vars;
            public BGCalcGraphModel Graph;
        }

        [Serializable]
        private class CalcVarModel //: BGFieldCalcVarRef
        {
            public byte CodeRef;
            public string Id;
            public string Value;
        }
    }
}