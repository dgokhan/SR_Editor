/*
<copyright file="BGMergerA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract merger
    /// </summary>
    public abstract partial class BGMergerA
    {
        protected readonly BGLogger logger;
        protected readonly BGRepo From;
        protected readonly BGRepo To;

        protected BGMergerA(BGLogger logger, BGRepo @from, BGRepo to)
        {
            this.logger = logger;
            From = @from;
            To = to;
        }

        protected void AppendLine(string message, params object[] parameters) => logger?.AppendLine(message, parameters);

        protected void AppendWarning(string message, params object[] parameters) => logger?.AppendWarning(message, parameters);

        protected void SubSection(Action action, string message, params object[] parameters)
        {
            if (logger == null) action();
            else logger.SubSection(action, message, parameters);
        }

        protected void Section(string message, Action action)
        {
            if (logger == null) action();
            else logger.Section(message, action);
        }
    }
}