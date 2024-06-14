/*
<copyright file="BGDBTextBinderField.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Data container for the field inside template data
    /// </summary>
    public class BGDBTextBinderField : BGDBTextBinder
    {
        private const string ERROR_CANNOT_FIND_META = "Can not find meta with specified id/name";
        private const string ERROR_CANNOT_FIND_ENTITY = "Can not find entity with specified id";
        private const string ERROR_CANNOT_FIND_FIELD = "Can not find field with specified id/name";

        private readonly Pointer pointer;

        public BGDBTextBinderField(Pointer pointer)
        {
            this.pointer = pointer;
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

            var field = pointer.FieldId.IsEmpty ? entity.Meta.GetField(pointer.FieldName, false) : entity.Meta.GetField(pointer.FieldId, false);
            Assert(field != null, ERROR_CANNOT_FIND_FIELD);

            if (field.ValueType == typeof(string)) context.Add(entity.Get<string>(field));
            else context.Add(field.ToString(entity.Index));

            context.Add(field, entity);
        }

        private void Assert(bool condition, string message)
        {
            if (condition) return;
            throw new BGDBTextBinderRoot.BindingException(message);
        }

        public class Pointer
        {
            public BGId MetaId = BGId.Empty;
            public string MetaName;

            public BGId EntityId = BGId.Empty;

            public BGId FieldId = BGId.Empty;
            public string FieldName;
        }

        public abstract class BGDBTextBinderFieldSpecial : BGDBTextBinder
        {
            public abstract Pointer Pointer { set; }
            public abstract BGDBTextBinder Create(Pointer pointer);
        }
    }
}