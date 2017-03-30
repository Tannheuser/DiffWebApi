using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DiffWebApi.Core.Models;
using DiffWebApi.Core.Repositories.Abstract;

namespace DiffWebApi.Core.Repositories
{
    /// <summary>
    /// In memory repository for position pairs
    /// </summary>
    public class ComparisonDataRepository : IDataRepository
    {
        private readonly IDictionary<int, PositionPair> _positionPairs = new ConcurrentDictionary<int, PositionPair>();

        /// <summary>
        /// Gets position pair by identifier
        /// </summary>
        /// <param name="id">Position pair identifier</param>
        /// <returns>Position pair</returns>
        public PositionPair Get(int id)
        {
            _positionPairs.TryGetValue(id, out PositionPair pair);
            return pair;
        }

        /// <summary>
        /// Creates new position pair or updates if it is already exist
        /// </summary>
        /// <param name="pair">Position pair to be created or updated</param>
        public void AddOrUpdate(PositionPair pair)
        {
            if (_positionPairs.ContainsKey(pair.Id))
            {
                Update(pair);
            }
            else
            {
                Add(pair);
            }
        }

        /// <summary>
        /// Deletes position pair with spicified identifier
        /// </summary>
        /// <param name="id">Position pair identifier</param>
        public void Delete(int id)
        {
            if (!_positionPairs.ContainsKey(id))
            {
                throw new Exception($"Position pair with id = {id} was not found.");
            }

            _positionPairs.Remove(id);
        }

        private void Add(PositionPair pair)
        {
            _positionPairs.Add(pair.Id, pair);
        }

        private void Update(PositionPair pair)
        {
            _positionPairs[pair.Id] = pair;
        }
    }
}
