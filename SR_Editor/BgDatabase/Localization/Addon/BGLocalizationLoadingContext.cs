/*
<copyright file="BGLocalizationLoadingContext.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    public class BGLocalizationLoadingContext
    {
        private readonly string locale;

        private byte[] repoData;
        private bool errorIfNotFound;

        private bool notMain;
        private string basePath;

        private PushFieldValuesConfig pushFieldsConfig;

        public string Locale => locale;

        public byte[] RepoData
        {
            get => repoData;
            set => repoData = value;
        }

        public bool ErrorIfNotFound
        {
            get => errorIfNotFound;
            set => errorIfNotFound = value;
        }

        public PushFieldValuesConfig PushFieldsConfig
        {
            get => pushFieldsConfig;
            set => pushFieldsConfig = value;
        }

        public bool NotMain
        {
            get => notMain;
            set => notMain = value;
        }

        public string BasePath
        {
            get => basePath;
            set => basePath = value;
        }

        public BGLocalizationLoadingContext(string locale)
        {
            this.locale = locale;
        }

        public override string ToString()
        {
            return locale;
        }

        public enum LoadPartitionsModeEnum
        {
            NotLoad,
            LoadLoaded,
            LoadAll
        }

        public class PushFieldValuesConfig
        {
            private LoadPartitionsModeEnum loadPartitionsMode;

            public LoadPartitionsModeEnum LoadPartitionsMode
            {
                get => loadPartitionsMode;
                set => loadPartitionsMode = value;
            }
        }
    }
}