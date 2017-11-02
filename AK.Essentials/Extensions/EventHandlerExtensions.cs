using System;
using System.Threading;

namespace AK.Essentials.Extensions
{
	public static class EventHandlerExtensions
	{
		public static void Fire<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, TEventArgs eventArgs) where TEventArgs : EventArgs
		{
			var tempHandler = Interlocked.CompareExchange(ref handler, null, null);
			if (tempHandler != null)
				tempHandler(sender, eventArgs);
		}

		public static void Fire<TValue>(this EventHandler<ValueEventArgs<TValue>> handler, object sender, TValue value)
		{
			var tempHandler = Interlocked.CompareExchange(ref handler, null, null);
			if (tempHandler != null)
				tempHandler(sender, new ValueEventArgs<TValue>(value));
		}

		public static void Fire(this EventHandler handler, object sender)
		{
			var tempHandler = Interlocked.CompareExchange(ref handler, null, null);
			if (tempHandler != null)
				tempHandler(sender, EventArgs.Empty);
		}
	}
}