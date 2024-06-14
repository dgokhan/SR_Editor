/*
<copyright file="BGFieldValidatorNested.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// validator for nested field
    /// </summary>
    [Validator(Target = typeof(BGFieldNested))]
    public partial class BGFieldValidatorNested : BGFieldValidator
    {
        private BGField _field;
        private BGFieldValidator[] fieldValidators;

        protected override BGField field
        {
            get => _field;
            set
            {
                _field = value;
                var nestedMeta = ((BGFieldNested)_field).NestedMeta;
                fieldValidators = new BGFieldValidator[nestedMeta.CountFields];
                for (var i = 0; i < fieldValidators.Length; i++)
                {
                    var validator = GetValidator(nestedMeta.GetField(i));
                    fieldValidators[i] = validator;
                }
            }
        }

        /// <inheritdoc />
        public override void Validate(BGEntity entity, Func<BGValidationLog[]> logsProvider)
        {
            base.Validate(entity, logsProvider);

            var nestedField = (BGFieldNested)field;

            var nestedList = nestedField[entity.Index];
            if (BGUtil.IsEmpty(nestedList)) return;


            for (var i = 0; i < nestedList.Count; i++)
            {
                var nested = nestedList[i];
                for (var j = 0; j < fieldValidators.Length; j++)
                {
                    var validator = fieldValidators[j];
                    validator.Validate(nested, logsProvider);
                }
            }
        }
    }
}