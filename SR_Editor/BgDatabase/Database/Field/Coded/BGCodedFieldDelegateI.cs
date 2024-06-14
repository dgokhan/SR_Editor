/*
<copyright file="BGCodedFieldDelegateI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// interface for context delegate implementation to calculate value
    /// </summary>
    public interface BGCodedFieldDelegateI
    {
        
    }
    public interface BGCodedFieldDelegateI<T> : BGCodedFieldDelegateI
    {
        T Get(BGCodedFieldContext context);
    }
}