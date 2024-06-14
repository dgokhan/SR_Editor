/*
<copyright file="BGFieldLocalizationTypeCodeFactory.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    public class BGFieldLocalizationTypeCodeFactory : BGFieldTypeCodeFactory.BGFieldTypeCodeFactoryI
    {
        public BGField Create(BGMetaEntity meta, ushort typeCode, BGId id, string name)
        {
            BGField field = null;
            switch (typeCode)
            {
                case BGFieldLocaleAudioClip.CodeType:
                {
                    field = new BGFieldLocaleAudioClip(meta, id, name);
                    break;
                }
                case BGFieldLocaleMaterial.CodeType:
                {
                    field = new BGFieldLocaleMaterial(meta, id, name);
                    break;
                }
                case BGFieldLocaleObject.CodeType:
                {
                    field = new BGFieldLocaleObject(meta, id, name);
                    break;
                }
                case BGFieldLocalePrefab.CodeType:
                {
                    field = new BGFieldLocalePrefab(meta, id, name);
                    break;
                }
                case BGFieldLocaleSprite.CodeType:
                {
                    field = new BGFieldLocaleSprite(meta, id, name);
                    break;
                }
                case BGFieldLocaleString.CodeType:
                {
                    field = new BGFieldLocaleString(meta, id, name);
                    break;
                }
                case BGFieldLocaleText.CodeType:
                {
                    field = new BGFieldLocaleText(meta, id, name);
                    break;
                }
                case BGFieldLocaleTexture.CodeType:
                {
                    field = new BGFieldLocaleTexture(meta, id, name);
                    break;
                }
                case BGFieldLocalizedAudioClip.CodeType:
                {
                    field = new BGFieldLocalizedAudioClip(meta, id, name);
                    break;
                }
                case BGFieldLocalizedMaterial.CodeType:
                {
                    field = new BGFieldLocalizedMaterial(meta, id, name);
                    break;
                }
                case BGFieldLocalizedObject.CodeType:
                {
                    field = new BGFieldLocalizedObject(meta, id, name);
                    break;
                }
                case BGFieldLocalizedPrefab.CodeType:
                {
                    field = new BGFieldLocalizedPrefab(meta, id, name);
                    break;
                }
                case BGFieldLocalizedSprite.CodeType:
                {
                    field = new BGFieldLocalizedSprite(meta, id, name);
                    break;
                }
                case BGFieldLocalizedString.CodeType:
                {
                    field = new BGFieldLocalizedString(meta, id, name);
                    break;
                }
                case BGFieldLocalizedText.CodeType:
                {
                    field = new BGFieldLocalizedText(meta, id, name);
                    break;
                }
                case BGFieldLocalizedTexture.CodeType:
                {
                    field = new BGFieldLocalizedTexture(meta, id, name);
                    break;
                }
                /*
                //check new constant here
                case 96:
                {
                    break;
                }
            */
            }

            return field;
        }
    }
}