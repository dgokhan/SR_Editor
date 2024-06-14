/*
<copyright file="BGFieldLocaleI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    public partial interface BGFieldLocaleI : BGFieldLocalizationI
    {
        void EnsureStore();
        void DestroyStore();
    }

    public partial interface BGFieldLocaleWithDelegateI : BGFieldLocaleI
    {
        BGField FieldDelegate { get; }

        // void ResetFieldDelegate();
        int EnsureDelegateEntity(BGId entityId);
    }
}