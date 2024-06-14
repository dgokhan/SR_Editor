/*
<copyright file="BGDBLocaleFieldBinder.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


namespace BansheeGz.BGDatabase
{
    public class BGDBLocaleFieldBinder : BGDBTextBinderField.BGDBTextBinderFieldSpecial
    {
        private const string ERROR_CANNOT_FIND_META = "Can not find meta with specified id/name";
        private const string ERROR_CANNOT_FIND_ENTITY = "Can not find entity with specified id";

        private BGDBTextBinderField.Pointer pointer;

        public override BGDBTextBinderField.Pointer Pointer
        {
            set => pointer = value;
        }

        public override BGDBTextBinder Create(BGDBTextBinderField.Pointer pointer)
        {
            return new BGDBLocaleFieldBinder { Pointer = pointer };
        }

        public override void Bind(BGDBTextBinderContext context)
        {
            var repo = BGRepo.I;
            BGEntity entity;
            if (pointer.MetaId.IsEmpty && pointer.MetaName == null)
                //no meta
                entity = repo.GetEntity(pointer.EntityId);
            else
            {
                var meta = pointer.MetaId.IsEmpty ? repo.GetMeta(pointer.MetaName) : repo.GetMeta(pointer.MetaId);
                Assert(meta != null, ERROR_CANNOT_FIND_META);
                entity = meta.GetEntity(pointer.EntityId);
            }

            Assert(entity != null, ERROR_CANNOT_FIND_ENTITY);

            // var addon = repo.Addons.Get<BGAddonLocalization>();
            // Assert(addon != null, "no localization addon");
            // var locale = addon.CurrentLocale;
            var locale = BGAddonLocalization.DefaultRepoCurrentLocale;
            Assert(!string.IsNullOrEmpty(locale), "current locale is not available!");
            // Assert(entity.Meta.HasField(locale), "no field " + locale + " at " + entity.Meta.Name + " meta");
            var field = entity.Meta.GetField(locale, false);
            Assert(field != null, "no field " + locale + " at " + entity.Meta.Name + " meta");
            context.Add(field.ToString(entity.Index));

            context.Root.Fields.Add(new BGDBTextBinderRoot.DBFieldInfo { EntityId = entity.Id, SpecialField = BGLocalizationUglyHacks.DataBinderLocale, MetaId = field.MetaId });
        }

        private void Assert(bool condition, string message)
        {
            if (condition) return;
            throw new BGDBTextBinderRoot.BindingException(message);
        }
    }
}