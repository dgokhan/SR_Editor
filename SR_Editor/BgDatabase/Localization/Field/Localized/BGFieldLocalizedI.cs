/*
<copyright file="BGFieldLocalizedI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    public partial interface BGFieldLocalizedI : BGFieldLocalizationI, BGRelationI, BGFieldWithCustomConfigI, BGFieldRelationSingleI
    {
        BGEntity GetTo(int entityIndex);
    }
}