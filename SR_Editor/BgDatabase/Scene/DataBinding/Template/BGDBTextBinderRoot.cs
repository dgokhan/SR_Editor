/*
<copyright file="BGDBTextBinderRoot.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Data container for the parsed template data
    /// </summary>
    public class BGDBTextBinderRoot
    {
        private readonly List<BGDBTextBinder> children = new List<BGDBTextBinder>();
        private readonly List<DBFieldInfo> fields = new List<DBFieldInfo>();
        private readonly string template;

        public string Error { get; set; }

        public List<DBFieldInfo> Fields => fields;

        public string Template => template;

        public BGDBTextBinderRoot(string template) => this.template = template;

        public string Bind()
        {
            if (Error != null) return Error;

            fields.Clear();
            try
            {
                var context = new BGDBTextBinderContext(this);
                foreach (var binder in children) binder.Bind(context);
                return context.Result;
            }
            catch (BindingException e)
            {
                Error = e.Message;
                return Error;
            }
        }

        public void Add(BGDBTextBinder binder)
        {
            if (Error != null) return;
            children.Add(binder);
        }

        public class BindingException : Exception
        {
            public BindingException(string message) : base(message)
            {
            }
        }

        public class DBFieldInfo
        {
            public BGId MetaId;
            public BGId EntityId;
            public BGId FieldId;
            public string SpecialField;
        }
    }
}