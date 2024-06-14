/*
<copyright file="BGSyncRowResolverId.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


namespace BansheeGz.BGDatabase
{
    public class BGSyncRowResolverId : BGSyncRowResolver
    {
        private readonly string metaName;
        private readonly BGId metaId;

        public BGSyncRowResolverId(BGId metaId, string metaName)
        {
            this.metaId = metaId;
            this.metaName = metaName;
        }

        public BGRowRef FromString(string value) => !BGId.TryParse(value, out var id) ? null : new BGRowRef(metaId, id);

        public string ToString(BGId rowId) => rowId.IsEmpty ? null : rowId.ToString();
        public BGId MetaId => metaId;

        public string MetaName => metaName;

        public override string ToString() => "Resolver by id, table=" + metaName;
    }
}