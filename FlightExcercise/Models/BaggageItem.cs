using System;
using System.Diagnostics.CodeAnalysis;

namespace FlightExcercise.Models
{
    public class BaggageItem : IEquatable<BaggageItem>
    {
        public int WeightInKg { get; }

        public BaggageItem(int weightInKg)
        {
            WeightInKg = weightInKg;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaggageItem);
        }

        public override int GetHashCode()
        {
            return WeightInKg.GetHashCode();
        }

        public bool Equals([AllowNull] BaggageItem other)
        {
            if (other == null) return false;
            return other.WeightInKg == WeightInKg;
        }
    }
}
