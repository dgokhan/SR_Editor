/*
<copyright file="BGFieldViewRelationI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    public interface BGFieldViewRelationI
    {
        BGId ViewId { get; }
        BGMetaView View { get; }
    }
}