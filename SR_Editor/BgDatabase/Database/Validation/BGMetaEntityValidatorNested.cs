/*
<copyright file="BGMetaEntityValidatorNested.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// validator for nested meta
    /// </summary>
    [Validator(Target = typeof(BGMetaNested))]
    public partial class BGMetaEntityValidatorNested : BGMetaEntityValidator
    {
        /// <inheritdoc />
        public override void Start(params BGValidationLog[] logs)
        {
            base.Start(logs);

            var metaNested = (BGMetaNested)meta;

            var ownerRelation = metaNested.OwnerRelation;
            if (ownerRelation == null) Add(logs, "Nested Meta $ is broken- no owner relation", meta.Name);
        }

        /// <inheritdoc/>        
        public override void Validate(BGEntity entity, params BGValidationLog[] logs)
        {
            base.Validate(entity, logs);
            var metaNested = (BGMetaNested)meta;
            var ownerRelation = metaNested.OwnerRelation;
            if (ownerRelation == null) return;

            if (ownerRelation[entity.Index] == null) Add(logs, "Nested Entity [$] is missing owner relation", entity.FullName);
        }
    }
}