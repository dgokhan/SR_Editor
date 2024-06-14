/*
<copyright file="BGValidationLog.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// log for validation
    /// </summary>
    public partial class BGValidationLog
    {
        private readonly List<string> errors = new List<string>();

        /// <summary>
        /// has any error
        /// </summary>
        public bool HasError => errors.Count > 0;

        /// <summary>
        /// number of errors
        /// </summary>

        public int Errors => errors.Count;

        /// <summary>
        /// clear log
        /// </summary>
        public void Clear()
        {
            errors.Clear();
        }


        /// <summary>
        /// add error
        /// </summary>
        public void Add(string error, params object[] parameters)
        {
            if (string.IsNullOrEmpty(error)) return;
            errors.Add(BGUtil.Format(error, parameters));
        }

        /*
        /// <summary>
        /// add info from another log
        /// </summary>
        public void Add(BGValidationLog log)
        {
            if (log.errors == 0) return;
            if (HasError) builder.Append('\n');
            errors += log.errors;
            builder.Append(log.builder);
        }
        */

        /// <summary>
        /// log as string
        /// </summary>
        public override string ToString()
        {
            return ToString(errors.Count);
        }

        /// <summary>
        /// log to string, limited by number of lines
        /// </summary>
        public string ToString(int maxLines)
        {
            if (errors.Count == 0) return "";
            var builder = new StringBuilder();
            int i;
            for (i = 0; i < maxLines && i < errors.Count; i++)
            {
                var error = errors[i];
                builder.Append(i + 1).Append(") ").Append(error).Append(Environment.NewLine);
            }

            if (i < errors.Count) builder.Append(errors.Count - maxLines + " more errors...");

            return builder.ToString();
        }
    }
}