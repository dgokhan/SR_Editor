/*
<copyright file="BGDBTextTagProcessor.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract class for processing template special tags 
    /// </summary>
    public abstract class BGDBTextTagProcessor
    {
        public abstract string Tag { get; }

        public abstract void Process(BGDBTextProcessorContext context, string parameter);
    }
}