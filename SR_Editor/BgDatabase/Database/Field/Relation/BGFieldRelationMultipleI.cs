using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    public interface BGFieldRelationMultipleI
    {
        /// <summary>
        /// Get related entities for entity with index  entityIndex
        /// </summary>
        List<BGEntity> GetRelatedEntity(int entityIndex);
        
        /// <summary>
        /// Set related entities for entity with index  entityIndex
        /// </summary>
        void SetRelatedEntity(int entityIndex, List<BGEntity> entity);
    }
}