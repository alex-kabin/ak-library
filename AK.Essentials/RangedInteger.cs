using System;
using AK.Essentials.Extensions;

namespace AK.Essentials
{
	public struct RangedInteger
	{
		public enum OutOfRangeBehavior : byte
		{
			Throw,
			Clip,
			Circulate
		}

		public OutOfRangeBehavior Behavior { get; private set; }
		public int Value { get; private set; }
		public Range<int> Range { get; private set; }

		public RangedInteger(Range<int> range, int value, OutOfRangeBehavior behavior = OutOfRangeBehavior.Throw) : this()
		{
			Range = range;
			Behavior = behavior;

			SetValue(value);
		}

		private void SetValue(int newValue)
		{
			if (newValue > Range.Max)
			{
				switch (Behavior)
				{
					case OutOfRangeBehavior.Clip: Value = Range.Max; return;
					case OutOfRangeBehavior.Circulate: Value = Range.Min + (newValue - Range.Max) % Range.Length(); return;
					default: throw new OutOfRangeException();
				}
			}
			if (newValue < Range.Min)
			{
				switch (Behavior)
				{
					case OutOfRangeBehavior.Clip: Value = Range.Min; return;
					case OutOfRangeBehavior.Circulate: Value = Range.Max - (Range.Min - newValue) % Range.Length(); return;
					default: throw new OutOfRangeException();
				}
			}
			Value = newValue;
		}

		public static implicit operator int(RangedInteger rint)
		{
			return rint.Value;
		}

		public static RangedInteger operator +(RangedInteger rint, int number)
		{
			return new RangedInteger(rint.Range, rint.Value + number, rint.Behavior);
		}

		public static RangedInteger operator -(RangedInteger rint, int number)
		{
			return new RangedInteger(rint.Range, rint.Value - number, rint.Behavior);
		}

		public static RangedInteger operator ++(RangedInteger rint)
		{
			return rint + 1;
		}

		public static RangedInteger operator --(RangedInteger rint)
		{
			return rint - 1;
		}


	}
}