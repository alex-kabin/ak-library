using System;
using System.Runtime.Serialization;

namespace AK.Essentials
{
	public class OutOfRangeException : Exception
	{
		public OutOfRangeException()
		{
		}

		public OutOfRangeException(string message) : base(message)
		{
		}

		public OutOfRangeException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}