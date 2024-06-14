namespace BansheeGz.BGDatabase
{
    public interface BGFieldRelationSingleI
    {
        /// <summary>
        /// Get related entity for entity with index  entityIndex
        /// </summary>
        BGEntity GetRelatedEntity(int entityIndex);
        
        /// <summary>
        /// Set related entity for entity with index  entityIndex
        /// </summary>
        void SetRelatedEntity(int entityIndex, BGEntity entity);
    }
}