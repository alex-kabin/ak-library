using System;

namespace AK.Essentials
{
	public class ValueEventArgs<T> : EventArgs
	{
		public T Value { get; private set; }

		public ValueEventArgs(T value)
		{
			Value = value;
		}
	}
}