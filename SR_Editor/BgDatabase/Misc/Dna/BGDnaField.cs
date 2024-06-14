/*
<copyright file="BGDnaField.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Dna for a field
    /// </summary>
    public abstract partial class BGDnaField : BGDnaDescriptor
    {
        public BGField Field { get; set; }
        private BGDnaMeta metaDna;

        public BGDnaMeta MetaDna
        {
            get => metaDna;
            set
            {
                if (metaDna == value) return;
                metaDna?.Fields.Remove(this);

                metaDna = value;

                metaDna?.Fields.Add(this);
            }
        }

        protected BGDnaField(BGDnaMeta metaDna, string dnaName) : base(dnaName)
        {
            MetaDna = metaDna;
        }

        //================================================================================================
        //                                              Bind
        //================================================================================================

        /// <summary>
        /// Bind field dna to specific meta
        /// </summary>
        public virtual void Bind(BGMetaEntity meta)
        {
            Field = meta.GetField(DnaName, false);
            if (Field == null) throw new BGException("Error while dna binding: Can not find field with name ($) at meta with name ($)", DnaName, meta.Name);
        }
    }

    //================================================================================================
    //                                              Typed field
    //================================================================================================

    /// <summary>
    /// Dna for a field with type
    /// </summary>
    public partial class BGDnaField<T> : BGDnaField
    {
        public BGDnaField(BGDnaMeta metaDna, string dnaName) : base(metaDna, dnaName)
        {
        }

        public T Get(BGEntity entity)
        {
            return entity.Get<T>(Field);
        }

        public void Set(BGEntity entity, T value)
        {
            entity.Set(Field, value);
        }
    }
}