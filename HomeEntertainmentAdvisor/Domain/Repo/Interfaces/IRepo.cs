namespace HomeEntertainmentAdvisor.Domain.Repo.Interfaces
{
    public interface IRepo<TEntity, TId>
    {
        /// <summary>
        ///  Saves <paramref name="entity"></paramref> to db
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>
        /// Id of saved <paramref name="entity"></paramref>
        /// </returns>
        public Task<TId> Save(TEntity entity);
        /// <summary>
        ///  Removes <paramref name="entity"></paramref> from db
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>
        /// Is removal succesful
        /// </returns>
        public Task<bool> Delete(TEntity entity);
        /// <summary>
        ///  Gets all records from the db
        /// </summary>
        /// <param name="entity"></param>
        public Task<List<TEntity>> GetAll();
        /// <summary>
        ///  Gets record from db by <paramref name="id"></paramref>
        /// </summary>
        /// <param name="id"></param>
        public Task<TEntity?> GetById(TId id);
    }
}
