/*
<copyright file="BGSyncRowResolver.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


namespace BansheeGz.BGDatabase
{
    public interface BGSyncRowResolver
    {
        BGRowRef FromString(string value);
        string ToString(BGId rowId);

        BGId MetaId { get; }
        string MetaName { get; }
    }
}