/*
<copyright file="BGFBFuntionToString.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    public class BGFBFuntionToString : BGFBFuntion
    {
        public const short Code = 2;
        public override string Name => "ToString";
        public override Type ReturnType => typeof(string);
        public override bool Supports(BGField field) => true;
        public override object Convert(BGField field, BGEntity e, object value) => value?.ToString();
    }
}