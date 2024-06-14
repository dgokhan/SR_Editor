/*
<copyright file="BGCalcVarContainer.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// variables owner (container)
    /// </summary>
    public class BGCalcVarContainer : BGCalcVarContainerBaseA<BGCalcVar>
    {
        /// <summary>
        /// on variables removed event
        /// </summary>
        public event Action<List<BGCalcVar>> OnDelete;

        public BGCalcVarContainer(BGCalcVarsOwnerBaseI owner) : base(owner)
        {
        }

        /// <summary>
        /// Get variable by it's ID
        /// </summary>
        public BGCalcVar GetVar(BGId varId)
        {
            for (var i = 0; i < vars.Count; i++)
            {
                var var = vars[i];
                if (@var.Id == varId) return @var;
            }

            return null;
        }

        /// <summary>
        /// Does this container have a variable with provided ID
        /// </summary>
        public bool HasVar(BGId id) => GetVar(id) != null;

        /// <summary>
        /// Remove variables with provided ID
        /// </summary>
        public int RemoveVar(BGId id)
        {
            if (vars.Count == 0) return 0;
            var count = 0;
            var deleted = new List<BGCalcVar>();
            for (var i = vars.Count - 1; i >= 0; i--)
            {
                var var = vars[i];
                if (@var.Id != id) continue;
                deleted.Add(var);
                vars.RemoveAt(i);
                count++;
            }

            if (count > 0) OnDelete?.Invoke(deleted);
            FireOnAnyChange();
            return count;
        }

        /// <summary>
        /// Get variable with provided name
        /// </summary>
        public BGCalcVar GetVar(string varName)
        {
            for (var i = 0; i < vars.Count; i++)
            {
                var var = vars[i];
                if (@var.Name == varName) return @var;
            }

            return null;
        }

        //================================================================================================
        //                                              ICloneable
        //================================================================================================
        /// <summary>
        /// Clone variables to provided owner
        /// </summary>
        public void CloneTo(BGCalcVarsOwnerI owner)
        {
            for (var i = 0; i < vars.Count; i++) vars[i].CloneTo(owner, true, true);
        }

        //================================================================================================
        //                                              serialization
        //================================================================================================
        /// <summary>
        /// write all variables to byte array
        /// </summary>
        public static void ToBytes(BGBinaryWriter writer, BGCalcVarContainer container)
        {
            //vars
            var varsCount = container?.Count ?? 0;
            writer.AddByte((byte)varsCount);
            if (varsCount == 0) return;

            foreach (var variable in container.vars)
            {
                writer.AddId(variable.Id);
                writer.AddString(variable.Name);
                writer.AddBool(variable.IsPublic);
                writer.AddByte(variable.TypeCode.TypeCode);
                if (variable.TypeCode is BGCalcTypeCodeStateful stateful) stateful.WriteState(writer);
                if (variable.TypeCode.SupportDefaultValue) variable.TypeCode.ValueToBytes(writer, variable.Value);
            }
        }

        /// <summary>
        /// restore variables from binary array
        /// </summary>
        public static void FromBytes(BGBinaryReader reader, BGCalcVarsOwnerI owner)
        {
            //read array 
            var count = reader.ReadByte();
            if (count <= 0) return;

            for (var i = 0; i < count; i++)
            {
                var id = reader.ReadId();
                var name = reader.ReadString();
                var isPublic = reader.ReadBool();

                var code = BGCalcTypeCodeRegistry.Get(reader.ReadByte());
                if (code is BGCalcTypeCodeStateful stateful) stateful.ReadState(reader);
                var variable = BGCalcVar.Create(owner, id, name, code);
                if (variable.TypeCode.SupportDefaultValue) variable.Value = code.ValueFromBytes(reader);
                variable.IsPublic = isPublic;
            }
        }
    }
}