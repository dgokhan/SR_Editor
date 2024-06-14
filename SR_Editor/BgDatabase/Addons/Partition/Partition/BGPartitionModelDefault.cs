/*
<copyright file="BGPartitionModelDefault.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System.IO;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// model for partition
    /// </summary>
    public class BGPartitionModelDefault : BGPartitionModelA
    {
        private readonly BGEntity entity;

        public BGEntity Entity => entity;

        public BGPartitionModelDefault(BGEntity entity)
        {
            this.entity = entity;
        }
    }
}