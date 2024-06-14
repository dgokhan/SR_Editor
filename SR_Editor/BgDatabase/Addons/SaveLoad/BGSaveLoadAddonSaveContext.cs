/*
<copyright file="BGSaveLoadAddonSaveContext.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    public class BGSaveLoadAddonSaveContext
    {
        public readonly string ConfigName = BGAddonSaveLoad.DefaultSettingsName;
        public bool FireBeforeSaveEvents = true;
        public bool MergeDataFromMTAddon = true;
        public BGSaveLoadAddonSaveContext()
        {
        }

        public BGSaveLoadAddonSaveContext(string configName)
        {
            ConfigName = configName;
        }
    }
}