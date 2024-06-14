/*
<copyright file="BGCodedFieldDelegateLifeCycleI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// interface for programmable delegate to implement to receive life cycle events
    /// </summary>
    public interface BGCodedFieldDelegateLifeCycleI
    {
        void OnLoad(BGCodedFieldDelegateLifeCycleContext context);
        void OnUnload(BGCodedFieldDelegateLifeCycleContext context);
    }
}