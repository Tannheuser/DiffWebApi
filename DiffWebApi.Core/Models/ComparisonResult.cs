using System.Collections.Generic;
using System.Linq;

namespace DiffWebApi.Core.Models
{
    public class ComparisonResult
    {
        public ComparisonResult(EqualityType equality)
        {
            Equality = equality;
        }

        public EqualityType Equality { get; set; }
        public IList<ComparisonDiff> Diff { get; set; } = new List<ComparisonDiff>();
        
        protected bool Equals(ComparisonResult result)
        {
            if (result == null)
            {
                return false;
            }

            return Equality == result.Equality && Diff.SequenceEqual(result.Diff);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == this.GetType() && Equals((ComparisonResult)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 125537;
                const int multiplier = 424242;

                hash = (hash * multiplier) ^ Equality.GetHashCode();
                hash = (hash * multiplier) ^ Diff.GetHashCode();

                return hash;
            }
        }
    }
}
