/*
<copyright file="BGFieldEnumTypeNameMapper.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    public interface BGEnumTypeNameMapper
    {
        string Map(string oldName);
    }
}