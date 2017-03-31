using System;
using DiffWebApi.Core.Models;
using DiffWebApi.Core.Repositories.Abstract;
using DiffWebApi.Core.Services.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace DiffWebApi.Core.Services
{
    public class ComparisonService : IComparisonService
    {
        private readonly IDataRepository _repository;

        public ComparisonService(IDataRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Creates new or updates pair's position part data.
        /// </summary>
        /// <param name="id">Position pair identifier</param>
        /// <param name="position">Position part</param>
        /// <param name="data">Binary array of data</param>
        public void AddOrUpdateData(int id, PositionType position, byte[] data)
        {
            var pair = _repository.Get(id);

            if (pair == null)
            {
                pair = new PositionPair(id);
            }

            SetPositionPartData(pair, position, data);
            _repository.AddOrUpdate(pair);
        }

        /// <summary>
        /// Gets comparison pair by identifier and compares it positions
        /// </summary>
        /// <param name="id">Comparison pair identifier</param>
        /// <returns>Result of positions comparison</returns>
        public ComparisonResult Compare(int id)
        {
            var pair = _repository.Get(id);

            if (pair == null)
            {
                throw new Exception($"Comparison pair with id = {id} does not exist");
            }

            if (pair.LeftPart == null || pair.RightPart == null)
            {
                throw new Exception($"Comparison pair with id = {id} is incomplete");
            }

            if (pair.LeftPart.Length != pair.RightPart.Length)
            {
                return new ComparisonResult(EqualityType.SizeDoNotMatch);
            }

            var diffs = GetAllDiffs(pair);

            if (diffs.Any())
            {
                return new ComparisonResult(EqualityType.ContentDoNotMatch)
                {
                    Diff = diffs
                };
            }

            return new ComparisonResult(EqualityType.Equals);
        }

        private void SetPositionPartData(PositionPair pair, PositionType position, byte[] data)
        {
            switch (position)
            {
                case PositionType.Left:
                    pair.LeftPart = data;
                    break;
                case PositionType.Right:
                    pair.RightPart = data;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(position), position, null);
            }
        }

        private IList<ComparisonDiff> GetAllDiffs(PositionPair pair)
        {
            var defaultDiffPosition = pair.LeftPart.Length;
            var currentDiffStart = pair.LeftPart.Length;
            var diffs = new Dictionary<long, long>();
            for (var i = 0; i < pair.LeftPart.Length; i++)
            {
                if (pair.LeftPart[i] != pair.RightPart[i])
                {
                    if (currentDiffStart == defaultDiffPosition)
                    {
                        currentDiffStart = i;
                        diffs.Add(currentDiffStart, 1);
                    }
                    else
                    {
                        diffs[currentDiffStart] += 1;
                    }
                }
                else
                {
                    currentDiffStart = defaultDiffPosition;
                }
            }
            return diffs.Select(d => new ComparisonDiff(d.Key, d.Value)).ToList();
        }

        
    }
}