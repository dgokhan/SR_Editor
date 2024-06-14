/*
<copyright file="BGDBTextBinderContext.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System.Text;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Data container for template binding operation
    /// </summary>
    public class BGDBTextBinderContext
    {
        private readonly BGDBTextBinderRoot root;
        private readonly StringBuilder result = new StringBuilder();

        public string Result => result.ToString();

        public BGDBTextBinderRoot Root => root;

        public BGDBTextBinderContext(BGDBTextBinderRoot root) => this.root = root;

        public void Add(string text) => result.Append(text);

        public void Add(BGField field, BGEntity entity) => root.Fields.Add(new BGDBTextBinderRoot.DBFieldInfo() { EntityId = entity.Id, FieldId = field.Id, MetaId = field.MetaId });
    }
}