/*
<copyright file="BGMonitorPage.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    public abstract class BGMonitorPage
    {
        public abstract string Name { get; }
        public abstract void Gui();
    }
}