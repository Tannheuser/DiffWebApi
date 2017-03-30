using DiffWebApi.Core.Models;

namespace DiffWebApi.Core.Repositories.Abstract
{
    public interface IDataRepository
    {
        /// <summary>
        /// Gets position pair by identifier
        /// </summary>
        /// <param name="id">Position pair identifier</param>
        /// <returns>Position pair</returns>
        PositionPair Get(int id);

        /// <summary>
        /// Creates new position pair or updates if it is already exist
        /// </summary>
        /// <param name="pair">Position pair to be created or updated</param>
        void AddOrUpdate(PositionPair pair);

        /// <summary>
        /// Deletes position pair with spicified identifier
        /// </summary>
        /// <param name="id">Position pair identifier</param>
        void Delete(int id);
    }
}
