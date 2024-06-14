/*
<copyright file="BGCalcPort.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract port implementation
    /// </summary>
    public abstract class BGCalcPort : BGCalcPortI
    {
        private readonly BGCalcUnitI unit;

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string Id { get; }

        /// <inheritdoc />
        public BGCalcPortTypeEnum PortType { get; }

        /// <inheritdoc />
        public BGCalcUnitI Unit => unit;

        /// <inheritdoc />
        public Type Type { get; }

        /// <inheritdoc />
        public abstract BGCalcTypeCode TypeCode { get; }

        /// <inheritdoc />
        public bool IsSingle
        {
            get
            {
                var portType = PortType;
                switch (portType)
                {
                    case BGCalcPortTypeEnum.ControlIn:
                    case BGCalcPortTypeEnum.ValueOut:
                        return false;
                    case BGCalcPortTypeEnum.ControlOut:
                    case BGCalcPortTypeEnum.ValueIn:
                        return true;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(portType));
                }
            }
        }

        /// <inheritdoc />
        public abstract bool IsConnected { get; }

        /// <inheritdoc />
        public bool IsInput => PortType == BGCalcPortTypeEnum.ControlIn || PortType == BGCalcPortTypeEnum.ValueIn;

        /// <inheritdoc />
        public abstract List<BGCalcPortI> ConnectedPorts { get; }

        //for new ports (slow with checks)
        protected BGCalcPort(BGCalcUnit unit, string name, string id, BGCalcPortTypeEnum portType, Type type)
        {
            if (id == null) throw new Exception("id can not be null");
            if (id.Length > 31) throw new Exception("id length is 31 chars maximum. incorrect value=" + id);

            //these checks are too slow
            // unit.CheckPortName(name);
            // if(!string.IsNullOrEmpty(id)) unit.CheckPortName(id);

            this.unit = unit ?? throw new Exception("Unit can not be null");
            Name = name;
            Id = id;
            PortType = portType;
            Type = type;
        }
        /*
        //for existing ports (fast no checks)
        protected BGCalcPort(BGCalcUnit unit, BGCalcPortTypeEnum portType, Type type, string id)
        {
            if (unit == null) throw new Exception("Unit can not be null");
            unit.CheckPortName(name);
            if(!string.IsNullOrEmpty(id)) unit.CheckPortName(id);
            this.unit = unit;
            Name = name;
            Id = id ?? name;
            PortType = portType;
            Type = type;
        }*/

        /// <inheritdoc />
        public abstract void Connect(BGCalcPortI port, bool connectBoth = true);

        /// <inheritdoc />
        public abstract void Disconnect(BGCalcPortI port, bool disconnectBoth = true);

        /// <inheritdoc />
        public abstract void DisconnectAll();

        /// <inheritdoc />
        public abstract bool CanConnectTo(BGCalcPortI toConnectPort);
        /*
        public virtual bool CanConnectTo(BGCalcPortI toConnectPort)
        {
            //the same node
            if (this == toConnectPort) return false;

            //the same direction
            switch (PortType)
            {
                case BGCalcPortTypeEnum.ControlIn:
                    if (toConnectPort.PortType != BGCalcPortTypeEnum.ControlOut) return false;
                    if (Unit == toConnectPort.Unit) return false;
                    break;
                case BGCalcPortTypeEnum.ControlOut:
                    if (toConnectPort.PortType != BGCalcPortTypeEnum.ControlIn) return false;
                    if (Unit == toConnectPort.Unit) return false;
                    break;
                case BGCalcPortTypeEnum.ValueIn:
                    if (toConnectPort.PortType != BGCalcPortTypeEnum.ValueOut) return false;
                    return ((BGCalcValueInputI)this).CanConnectToOutput((BGCalcValueOutputI)toConnectPort);
                case BGCalcPortTypeEnum.ValueOut:
                    if (toConnectPort.PortType != BGCalcPortTypeEnum.ValueIn) return false;
                    return ((BGCalcValueInputI)toConnectPort).CanConnectToOutput((BGCalcValueOutputI)this);
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return true;
        }
        */

        public override string ToString() => Unit.Title + '.' + Name + (TypeCode != null ? " [" + TypeCode.TypeTitle + "]" : " (" + Type.Name + ")") + " id=" + Id;

        public static bool ShouldPortBeSerialized(BGCalcPortI port) // where T : BGCalcPortI
        {
            if (port.IsConnected) return true;
            if (port.PortType == BGCalcPortTypeEnum.ValueIn && ((BGCalcValueInputI)port).HasDefaultValue) return true;
            return false;
        }

        /// <summary>
        /// Fire graph any change event
        /// </summary>
        public void FireOnAnyChange() => Unit.Graph.FireOnAnyChange();

        //=======================================================================
        //                          Equals
        //=======================================================================
        public virtual bool IsEqual(BGCalcPortI other)
        {
            if (other == null) return false;
            return Name == other.Name && Id == other.Id && PortType == other.PortType && Type == other.Type && Equals(TypeCode, other.TypeCode) && IsConnected == other.IsConnected;
        }
        
        //does 2 ports lists are equal. The order can be different! 
        protected static bool ListEqual<T>(List<T> list1, List<T> list2) where T : BGCalcPortI
        {
            if (list1.Count != list2.Count) return false;
            var bitArray = new BitArray(list1.Count);
            for (var i = 0; i < list1.Count; i++)
            {
                var port1 = list1[i];
                // var port2 = list2[i];
                BGCalcPortI port2 = null;
                for (var j = 0; j < list2.Count; j++)
                {
                    if (bitArray[j]) continue;
                    var portI = list2[j];
                    if (port1.Id != portI.Id) continue;
                    bitArray[j] = true;
                    port2 = portI;
                    break;
                }

                if (port2 == null) return false;
            }

            return true;
        }

        /*
        protected virtual bool Equals(BGCalcPort other)
        {
            return Name == other.Name && Id == other.Id && PortType == other.PortType && Type == other.Type && Equals(TypeCode, other.TypeCode) && IsConnected == other.IsConnected;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BGCalcPort)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Name != null ? Name.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)PortType;
                hashCode = (hashCode * 397) ^ (Type != null ? Type.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (TypeCode != null ? TypeCode.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsConnected.GetHashCode();
                return hashCode;
            }
        }
        */
    }
}