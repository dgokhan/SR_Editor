/*
<copyright file="BGControllerOnEntityAdd.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// interface should be implemented by meta object controller. OnEntityAdd method is called when new entity is called
    /// </summary>
    public interface BGControllerOnEntityAdd
    {
        void OnEntityAdd(BGMetaObject source, BGEntity entity);
    }
}