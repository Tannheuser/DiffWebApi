using System.Collections.Generic;
using DiffWebApi.Core.Models;

namespace DiffWebApi.Web.ViewModels
{
    public class ComparisonResultDto
    {
        public ComparisonResultDto()
        {
            
        }

        public ComparisonResultDto(string resultType, IList<ComparisonDiff> diffs)
        {
            DiffResultType = resultType;
            Diffs = diffs;
        }

        public string DiffResultType { get; set; }
        public IList<ComparisonDiff> Diffs { get; set; }
    }
}
