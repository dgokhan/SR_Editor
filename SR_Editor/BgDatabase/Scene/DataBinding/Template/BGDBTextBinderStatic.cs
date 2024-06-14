/*
<copyright file="BGDBTextBinderStatic.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Data container for the static (constant) template data
    /// </summary>
    public class BGDBTextBinderStatic : BGDBTextBinder
    {
        private readonly string text;

        public BGDBTextBinderStatic(string text) => this.text = text;

        public override void Bind(BGDBTextBinderContext context) => context.Add(text);
    }
}