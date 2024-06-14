/*
<copyright file="BGDBTextProcessor.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Processor for converting string template to template model (BGDBTextBinderRoot)
    /// </summary>
    public class BGDBTextProcessor
    {
        private static readonly Dictionary<string, BGDBTextTagProcessor> Tag2Processor = new Dictionary<string, BGDBTextTagProcessor>();

        private static BGDBTextTagProcessor GetProcessor(string name)
        {
            if (Tag2Processor.Count == 0)
                //lets keep it max simple
                Register(new BGDBTextTagProcessorField());
/*
                var implementations = BGUtil.GetAllImplementations(typeof(BGDBTextTagProcessor));
                foreach (var type in implementations)
                {
                    var processor = BGUtil.Create<BGDBTextTagProcessor>(type, false);
                    Tag2Processor[processor.Tag] = processor;
                }
*/

            return BGUtil.Get(Tag2Processor, name);
        }

        private static void Register(BGDBTextTagProcessor processor) => Tag2Processor[processor.Tag] = processor;

        public BGDBTextBinderRoot Process(string template)
        {
            var result = new BGDBTextBinderRoot(template);
            var context = new BGDBTextProcessorContext(this, template, result);

            Process(context);

            return result;
        }

        //lets keep it max simple
        private void Process(BGDBTextProcessorContext context)
        {
            try
            {
                var template = context.Template;
                if (template == null) return;
                var count = template.Length;

                var pointer = 0;
                var staticContentPointer = 0;
                while (pointer < count)
                {
                    var tagIndex = template.IndexOf('#', pointer);
                    if (tagIndex == -1) break;

                    pointer = tagIndex + 1;

                    //test for tag
                    var @char = 'a';
                    var i = tagIndex;
                    var countMinus1 = count - 1;
                    for (; i < countMinus1 && char.IsLetter(@char); @char = template[++i]) { }

                    //end of template
                    if (i > count - 3) break;

                    if (@char == '(')
                    {
                        var tagLength = i - tagIndex - 1;
                        if (tagLength != 0)
                        {
                            var tag = template.Substring(tagIndex + 1, tagLength);
                            var processor = GetProcessor(tag);
                            if (processor != null)
                            {
                                var closingBracket = template.IndexOf(')', i);
                                if (closingBracket != -1)
                                {
                                    if (staticContentPointer < tagIndex) context.Root.Add(new BGDBTextBinderStatic(template.Substring(staticContentPointer, tagIndex - staticContentPointer)));
                                    staticContentPointer = closingBracket + 1;
                                    processor.Process(context, template.Substring(i + 1, closingBracket - i - 1));
                                }
                            }
                        }
                    }
                }

                if (staticContentPointer < count) context.Root.Add(new BGDBTextBinderStatic(template.Substring(staticContentPointer, count - staticContentPointer)));
            }
            catch (ExitException)
            {
            }
        }

        public class ExitException : Exception
        {
        }
    }
}