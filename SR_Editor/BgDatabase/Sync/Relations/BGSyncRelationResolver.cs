/*
<copyright file="BGSyncRelationResolver.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


namespace BansheeGz.BGDatabase
{
    public interface BGSyncRelationResolver
    {
        void ToDatabase(int index, string value);
        string ToExternalFormat(int index);
    }
}