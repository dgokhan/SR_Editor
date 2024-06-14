/*
<copyright file="BGCalcVarLiteContainer.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Container for lite variables
    /// </summary>
    public class BGCalcVarLiteContainer : BGCalcVarContainerBaseA<BGCalcVarLite>
    {
        // public event Action<List<BGCalcVarLite>> OnDelete;

        public BGCalcVarLiteContainer(BGCalcVarsOwnerBaseI owner) : base(owner)
        {
        }

        /// <summary>
        /// Get variable with provided ID
        /// </summary>
        public BGCalcVarLite GetVar(byte varId)
        {
            for (var i = 0; i < vars.Count; i++)
            {
                var var = vars[i];
                if (@var.Id == varId) return @var;
            }

            return null;
        }

        /// <summary>
        /// Does container have a variable with provided ID
        /// </summary>
        public bool HasVar(byte varId) => GetVar(varId) != null;

        /// <summary>
        /// Remove variable with provided ID
        /// </summary>
        public int RemoveVar(byte varId)
        {
            if (vars.Count == 0) return 0;
            var count = 0;
            var deleted = new List<BGCalcVarLite>();
            for (var i = vars.Count - 1; i >= 0; i--)
            {
                var var = vars[i];
                if (@var.Id != varId) continue;
                deleted.Add(var);
                vars.RemoveAt(i);
                count++;
            }

            // if (count > 0) OnDelete?.Invoke(deleted);
            FireOnAnyChange();
            return count;
        }


        //================================================================================================
        //                                              ICloneable
        //================================================================================================
        /// <summary>
        /// clone all variables to provided variables owner
        /// </summary>
        public void CloneTo(BGCalcVarsLiteOwnerI owner)
        {
            for (var i = 0; i < vars.Count; i++) vars[i].CloneTo(owner, true);
        }

        //================================================================================================
        //                                              serialization
        //================================================================================================
        /// <summary>
        /// write all variables to binary array
        /// </summary>
        public static void ToBytes(BGBinaryWriter writer, BGCalcVarLiteContainer container)
        {
            //vars
            var varsCount = container?.Count ?? 0;
            writer.AddByte((byte)varsCount);
            if (varsCount == 0) return;

            foreach (var variable in container.vars)
            {
                writer.AddByte(variable.Id);
                writer.AddByte(variable.TypeCode.TypeCode);
                if (variable.TypeCode is BGCalcTypeCodeStateful stateful) stateful.WriteState(writer);
                if (variable.TypeCode.SupportDefaultValue) variable.TypeCode.ValueToBytes(writer, variable.Value);
            }
        }

        /// <summary>
        /// restore variables from binary array
        /// </summary>
        public static void FromBytes(BGBinaryReader reader, BGCalcVarsLiteOwnerI owner)
        {
            //read array 
            var count = reader.ReadByte();
            if (count <= 0) return;

            for (var i = 0; i < count; i++)
            {
                var id = reader.ReadByte();
                var code = BGCalcTypeCodeRegistry.Get(reader.ReadByte());
                if (code is BGCalcTypeCodeStateful stateful) stateful.ReadState(reader);
                var variable = BGCalcVarLite.Create(owner, id, code);
                if (code.SupportDefaultValue) variable.Value = code.ValueFromBytes(reader);
            }
        }
    }
}