/*
<copyright file="BGLocalizedFieldAttribute.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    public partial class BGLocalizedFieldAttribute : Attribute
    {
        public string Name;
        public Type TargetFieldType;

        public static BGLocalizedFieldAttribute Get(Type type)
        {
            return BGUtil.GetAttribute<BGLocalizedFieldAttribute>(type);
        }
    }
}