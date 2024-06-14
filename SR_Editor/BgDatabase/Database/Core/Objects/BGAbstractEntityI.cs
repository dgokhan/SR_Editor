/*
<copyright file="BGAbstractEntityI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Abstract entity interface
    /// </summary>
    public interface BGAbstractEntityI : BGObjectWithNameI
    {
        /// <summary>
        /// Row's name
        /// </summary>
        new string Name { get; set; }
        /// <summary>
        /// Row's physical index
        /// </summary>
        int Index { get; }
        /// <summary>
        /// Row's table
        /// </summary>
        BGMetaEntity Meta { get; }
        /// <summary>
        /// Rows table ID
        /// </summary>
        BGId MetaId { get; }
        /// <summary>
        /// Rows table name
        /// </summary>
        string MetaName { get; }
        /// <summary>
        /// Full row name (MetaName.RowName) 
        /// </summary>
        string FullName { get; }
        /// <summary>
        /// Database, this row belongs to 
        /// </summary>
        BGRepo Repo { get; }
        /// <summary>
        /// delete the entity 
        /// </summary>
        void Delete();
        /// <summary>
        /// Get field value  
        /// </summary>
        T Get<T>(BGField field);
        /// <summary>
        /// Get field value by field id  
        /// </summary>
        T Get<T>(BGId fieldId);
        /// <summary>
        /// Get field value by field name  
        /// </summary>
        T Get<T>(string fieldName);
        /// <summary>
        /// Set field value   
        /// </summary>
        void Set<T>(BGField field, T value);
        /// <summary>
        /// Set field value by field name   
        /// </summary>
        void Set<T>(string fieldName, T value);
        /// <summary>
        /// Set field value by field ID   
        /// </summary>
        void Set<T>(BGId fieldId, T value);
    }
}