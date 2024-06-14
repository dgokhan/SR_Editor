/*
<copyright file="BGLocaleFieldAttribute.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    public partial class BGLocaleFieldAttribute : Attribute
    {
        public Type DelegateFieldType;
        public string Name;

        public static BGLocaleFieldAttribute Get(Type type) => BGUtil.GetAttribute<BGLocaleFieldAttribute>(type);
    }
}