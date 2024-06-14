/*
<copyright file="BGMetaLocalizationTypeCodeFactory.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    public class BGMetaLocalizationTypeCodeFactory : BGMetaTypeCodeFactory.BGMetaTypeCodeFactoryI
    {
        public BGMetaEntity Create(BGRepo repo, ushort typeCode, BGId id, string name)
        {
            BGMetaEntity meta = null;
            switch (typeCode)
            {
                case BGMetaLocalizationSettings.CodeType:
                {
                    meta = new BGMetaLocalizationSettings(repo, id, name);
                    break;
                }
                case BGMetaLocalization.CodeType:
                {
                    meta = new BGMetaLocalization(repo, id, name);
                    break;
                }
                case BGMetaLocalizationSingleValue.CodeType:
                {
                    meta = new BGMetaLocalizationSingleValue(repo, id, name);
                    break;
                }
            }

            return meta;
        }
    }
}