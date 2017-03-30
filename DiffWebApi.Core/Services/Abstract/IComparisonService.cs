using DiffWebApi.Core.Models;

namespace DiffWebApi.Core.Services.Abstract
{
    public interface IComparisonService
    {
        /// <summary>
        /// Creates new or updates pair's position part data.
        /// </summary>
        /// <param name="id">Position pair identifier</param>
        /// <param name="position">Position part</param>
        /// <param name="data">Binary array of data</param>
        void AddOrUpdateData(int id, PositionType position, byte[] data);

        /// <summary>
        /// Gets comparison pair by identifier and compares it positions
        /// </summary>
        /// <param name="id">Comparison pair identifier</param>
        /// <returns>Result of positions comparison</returns>
        ComparisonResult Compare(int id);
    }
}