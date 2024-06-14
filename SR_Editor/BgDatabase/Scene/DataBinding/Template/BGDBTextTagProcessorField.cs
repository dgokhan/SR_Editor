/*
<copyright file="BGDBTextTagProcessorField.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract class for processing template field tag (reference to a field) 
    /// </summary>
    public class BGDBTextTagProcessorField : BGDBTextTagProcessor
    {
        public override string Tag => "FIELD";

        public override void Process(BGDBTextProcessorContext context, string parameter)
        {
            if (!string.IsNullOrEmpty(parameter))
            {
                var tokens = parameter.Split('@');
                if (tokens.Length == 2 || tokens.Length == 3)
                {
                    var fieldIdStr = tokens[0];
                    var entityIdStr = tokens[1];
                    string metaIdStr = null;
                    if (tokens.Length == 3) metaIdStr = tokens[2];

                    //fieldId
                    var fieldId = BGId.Empty;
                    string fieldName = null;
                    //this is very intrusive way of doing things , but it's done for the sake of performance
                    if (fieldIdStr != null && BGLocalizationUglyHacks.DataBinderLocale.Equals(fieldIdStr)) fieldName = fieldIdStr;
                    else
                    {
                        fieldId = BGId.Parse(fieldIdStr);
                        if (fieldId.IsEmpty)
                        {
                            fieldName = fieldIdStr;
                            Assert(context, BGMetaObject.CheckName(fieldName) == null, "#FIELD tag parameter contains invalid Field name:[ " + fieldName + "]");
                        }
                    }

                    //entityId
                    var entityId = BGId.Parse(entityIdStr);
                    Assert(context, !entityId.IsEmpty, "#FIELD tag parameter contains invalid Entity id:[ " + entityIdStr + "]");

                    //metaId
                    var metaId = BGId.Empty;
                    string metaName = null;
                    if (metaIdStr != null)
                    {
                        metaId = BGId.Parse(fieldIdStr);
                        if (metaId.IsEmpty)
                        {
                            metaName = metaIdStr;
                            Assert(context, BGMetaObject.CheckName(metaName) == null, "#FIELD tag parameter contains invalid Meta name:[ " + metaName + "]");
                        }
                    }

                    var pointer = new BGDBTextBinderField.Pointer
                    {
                        FieldId = fieldId,
                        FieldName = fieldName,
                        EntityId = entityId,
                        MetaId = metaId,
                        MetaName = metaName
                    };

                    //this is very intrusive way of doing things , but it's done for the sake of performance
                    if (!BGLocalizationUglyHacks.DataBindingBind(fieldName, context.Root, pointer)) context.Root.Add(new BGDBTextBinderField(pointer));
                    return;
                }
            }

            Assert(context, false, "#FIELD tag parameter is invalid:[" + parameter + "]");
        }


        private void Assert(BGDBTextProcessorContext context, bool condition, string reason)
        {
            if (condition) return;
            context.Root.Error = reason;
            throw new BGDBTextProcessor.ExitException();
        }
    }
}