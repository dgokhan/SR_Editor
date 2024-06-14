/*
<copyright file="BGMergeSettingsA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract merge settings
    /// </summary>
    [Serializable]
    public abstract partial class BGMergeSettingsA
    {
        public event Action OnChange;

        [SerializeField] protected BGMergeModeEnum mode = BGMergeModeEnum.Merge;

        [SerializeField] protected bool addMissing;
        [SerializeField] protected bool updateMatching;
        [SerializeField] protected bool removeOrphaned;

        public bool IncludedByDefault => addMissing || updateMatching || removeOrphaned;

        /// <summary>
        /// Merging mode used
        /// </summary>
        public BGMergeModeEnum Mode
        {
            get => mode;
            set
            {
                if (mode == value) return;
                mode = value;
                FireOnChange();
            }
        }

        /// <summary>
        /// Should new entities be added?
        /// </summary>
        public bool AddMissing
        {
            get => addMissing;
            set
            {
                if (addMissing == value) return;
                addMissing = value;
                FireOnChange();
            }
        }

        /// <summary>
        /// Should matching entities be updated?
        /// </summary>
        public bool UpdateMatching
        {
            get => updateMatching;
            set
            {
                if (updateMatching == value) return;
                updateMatching = value;
                FireOnChange();
            }
        }

        /// <summary>
        /// Should old entities be removed?
        /// </summary>
        public bool RemoveOrphaned
        {
            get => removeOrphaned;
            set
            {
                if (removeOrphaned == value) return;
                removeOrphaned = value;
                FireOnChange();
            }
        }

        protected void FireOnChange() => OnChange?.Invoke();
    }
}