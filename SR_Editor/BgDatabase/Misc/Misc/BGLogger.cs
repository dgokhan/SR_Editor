/*
<copyright file="BGLogger.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// log container
    /// </summary>
    public partial class BGLogger
    {
        private readonly Stack<SubSectionInfo> subsections = new Stack<SubSectionInfo>();
        private StringBuilder builder = new StringBuilder();

        public string Log => builder.ToString();

        private readonly bool useRichText;
        private int tab;
        private int warnings;

        private SubSectionInfo subSectionInfo;

        public int Warnings => warnings;

        public int Indent
        {
            get => tab;
            set => tab = value;
        }

        public BGLogger(bool useRichText = true)
        {
            this.useRichText = useRichText;
        }

        /// <summary>
        /// Clear the log
        /// </summary>
        public void Clear()
        {
            builder.Length = 0;
            subsections.Clear();
            tab = 0;
            warnings = 0;
            subSectionInfo = null;
        }

        /// <summary>
        /// append line
        /// </summary>
        public void AppendLine(string message, params object[] parameters)
        {
            if (message == null) return;
            message = Highlight(message, null, parameters);
            builder.AppendLine(Tab + BGUtil.Format(message, parameters));
        }

        /// <summary>
        /// tab symbol
        /// </summary>
        public string Tab => tab == 0 ? "" : new string('\t', tab);

        /// <summary>
        /// append info from another logger
        /// </summary>
        public void Append(BGLogger logger)
        {
            builder.AppendLine(logger.Log);
        }

        /// <summary>
        /// append line
        /// </summary>
        public bool AppendLine(bool condition, string message, params object[] parameters)
        {
            if (!condition) return false;
            AppendLine(message, parameters);
            return true;
        }

        /// <summary>
        /// append section
        /// </summary>
        public void Section(string message, Action action)
        {
            tab = 0;
            var oldBuilder = builder;
            builder = new StringBuilder();
            tab++;
            var stopwatch = Stopwatch.StartNew();
            try
            {
                action();
            }
            finally
            {
                var currentBuilder = builder;
                builder = oldBuilder;

                stopwatch.Stop();
                AppendLine("");
                AppendLine("============ [[$, executed in $ mls]] ===========================", message, stopwatch.ElapsedMilliseconds);
                builder.Append(currentBuilder);

                tab = 0;
            }
        }

        /// <summary>
        /// append subsection
        /// </summary>
        public void SubSection(Action action, string message, params object[] parameters)
        {
            SubSectionStart(message, parameters);
            try
            {
                action();
            }
            finally
            {
                SubSectionEnd();
            }
        }

        /// <summary>
        /// Start subsection
        /// </summary>
        public void SubSectionStart(string message, params object[] parameters)
        {
            subsections.Push(subSectionInfo);
            subSectionInfo = new SubSectionInfo(this, message, parameters);
        }

        /// <summary>
        /// End subsection
        /// </summary>
        public void SubSectionEnd()
        {
            subSectionInfo?.End();
            subSectionInfo = subsections.Count > 0 ? subsections.Pop() : null;
        }

        /// <summary>
        /// append warning
        /// </summary>
        public void AppendWarning(string message, params object[] parameters)
        {
            if (message == null) return;
            warnings++;
            message = Highlight(message, "red", parameters);
            builder.AppendLine(Tab + BGUtil.Format("WARNING: " + message, parameters));
        }

        /// <summary>
        /// append warning
        /// </summary>
        public bool AppendWarning(bool condition, string message, params object[] parameters)
        {
            if (!condition) return false;
            AppendWarning(message, parameters);
            return true;
        }

        //highlight the message, using provided color
        private string Highlight(string message, string color, object[] parameters)
        {
            if (!useRichText || parameters == null || parameters.Length == 0) return message;
            return message.Replace("$", BGRichText.Highlight("$", color));
        }

        /// <summary>
        /// throw exception if condition is met
        /// </summary>
        public void Exception(bool condition, string reason, params object[] parameters)
        {
            if (!condition) return;

            AppendWarning(reason, parameters);
            throw new BGException(reason, parameters);
        }

        //subsection data container
        private class SubSectionInfo
        {
            private readonly BGLogger logger;
            private readonly StringBuilder oldBuilder;
            private readonly Stopwatch stopwatch;
            private readonly string message;
            private readonly object[] parameters;
            private readonly StringBuilder myBuilder = new StringBuilder();

            public SubSectionInfo(BGLogger logger, string message, object[] parameters)
            {
                this.logger = logger;
                oldBuilder = logger.builder;
                logger.builder = myBuilder;
                logger.tab++;
                stopwatch = Stopwatch.StartNew();
                this.message = message;
                this.parameters = parameters;
            }

            /// <summary>
            /// Called on subsection end
            /// </summary>
            public void End()
            {
                stopwatch.Stop();

                logger.builder = oldBuilder;

                object[] finalParams;
                if (parameters == null) finalParams = new object[] { stopwatch.ElapsedMilliseconds };
                else
                {
                    finalParams = new object[parameters.Length + 1];
                    parameters.CopyTo(finalParams, 0);
                    finalParams[finalParams.Length - 1] = stopwatch.ElapsedMilliseconds;
                }

                logger.AppendLine("-----[" + message + ", executed in $ mls]---------------------------", finalParams);
                logger.builder.Append(myBuilder);
                logger.tab--;
            }
        }
    }
}