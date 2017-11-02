using System;
using System.Runtime.InteropServices;

namespace AK.Essentials
{
	/// <summary>
	/// Интервал
	/// </summary>
	/// <typeparam name="T">Тип данных</typeparam>
	[StructLayout(LayoutKind.Auto)]
	public struct Range<T> where T : IEquatable<T>, IComparable<T>
	{
		public static readonly Range<T> Empty = new Range<T>(default(T), default(T));

		public T Min { get; private set; }
		public T Max { get; private set; }

		public Range(T min, T max) : this()
		{
			Min = min.CompareTo(max) < 0 ? min : max;
			Max = min.CompareTo(max) < 0 ? max : min;
		}

		public override string ToString()
		{
			return String.Format("RANGE: {0}-{1}", Min, Max);
		}

		public override bool Equals(object obj)
		{
			if (obj is Range<T>)
				return Equals((Range<T>)obj); // use Equals method below
			
			return false;
		}

		public bool Equals(Range<T> other)
		{
			return this.Min.Equals(other.Min) && this.Max.Equals(other.Max);
		}

		public override int GetHashCode()
		{
			return (int)(Min.GetHashCode() ^ (Max.GetHashCode()));
		}

		public static bool operator ==(Range<T> left, Range<T> right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Range<T> left, Range<T> right)
		{
			return !left.Equals(right);
		}

		public bool Contains(Range<T> other)
		{
			return Min.CompareTo(other.Min) <= 0 && Max.CompareTo(other.Max) >= 0;
		}

		public bool Contains(T point)
		{
			return Min.CompareTo(point) <= 0 && Max.CompareTo(point) >= 0;
		}

		public bool Overlaps(Range<T> other)
		{
			return (Contains(other.Min) && other.Max.CompareTo(Max) > 0) || (Contains(other.Max) && other.Min.CompareTo(Min) < 0);
		}

		public static Range<T> Union(Range<T> r1, Range<T> r2)
		{
			if (r1.Min.CompareTo(r2.Min) <= 0)
			{
				if (r1.Max.CompareTo(r2.Min) >= 0)
					return new Range<T>(r1.Min, Comparables.Max(r1.Max, r2.Max));
			}
			else
			{
				if (r2.Max.CompareTo(r1.Min) >= 0)
					return new Range<T>(r2.Min, Comparables.Max(r1.Max, r2.Max));
			}
			return Empty;
		}

		public static Range<T> Intersect(Range<T> r1, Range<T> r2)
		{
			if (r1.Min.CompareTo(r2.Min) <= 0)
			{
				if (r1.Max.CompareTo(r2.Min) >= 0)
					return new Range<T>(r2.Min, Comparables.Min(r1.Max, r2.Max));
			}
			else
			{
				if (r2.Max.CompareTo(r1.Min) >= 0)
					return new Range<T>(r1.Min, Comparables.Min(r1.Max, r2.Max));
			}
			return Empty;
		}
	}
}