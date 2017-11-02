using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics.Contracts;
using System.Linq;
using AK.Essentials.Extensions;

namespace AK.Essentials.Collections
{
	public class CircularBuffer<T> : ICollection<T>
	{
		private T[] _data;
		private int _startIndex;
		private int _endIndex;
		private int _count;
		private int _version;

		public CircularBuffer(int capacity)
		{
			//Contract.Requires<ArgumentOutOfRangeException>(capacity > 0, "Capacity should be >= 1");
			_data = new T[capacity];
		}

		#region Privates
		private void IncrementIndex(ref int index)
		{
			if (++index >= Capacity)
				index = 0;
		}

		private void DecrementIndex(ref int index)
		{
			if (--index < 0)
				index = Capacity - 1;
		}

		private void RefreshCount()
		{
			_count = _endIndex >= _startIndex ? _endIndex - _startIndex : _data.Length - _startIndex + _endIndex;
		}

		private void ThrowIfModified(int version)
		{
			if(_version != version)
				throw new InvalidOperationException("The collection was modified after the enumerator was created");
		}

		private void SetModified()
		{
			_version++;
		}

		private IEnumerable<int> EnumerateIndicesForward()
		{
			var version = _version;
			var index = _startIndex;
			while (index != _endIndex)
			{
				ThrowIfModified(version);
				yield return index;
				IncrementIndex(ref index);
			}
		}

		private IEnumerable<int> EnumerateIndicesBackward()
		{
			var version = _version;
			var index = _endIndex;
			while (index != _startIndex)
			{
				ThrowIfModified(version);
				DecrementIndex(ref index);
				yield return index;
			}
		}
		#endregion // Privates

		#region Implementation of IEnumerable
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public IEnumerator<T> GetEnumerator()
		{
			return EnumerateIndicesForward().Select(index => _data[index]).GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion

		public IEnumerator<T> GetBackwardEnumerator()
		{
			return EnumerateIndicesBackward().Select(index => _data[index]).GetEnumerator();
		}

		public IEnumerable<T> Reversed()
		{
			return EnumerateIndicesBackward().Select(index => _data[index]);
		}

		#region Implementation of ICollection<T>
		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		public void Add(T item)
		{
			if (Count >= Capacity)
			{
				_data[_startIndex] = default(T);
				IncrementIndex(ref _startIndex);
			}

			_data[_endIndex] = item;
			IncrementIndex(ref _endIndex);

			RefreshCount();
			SetModified();
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
		public void Clear()
		{
			if (IsEmpty)
				return;

			int count = Count;
			while (count-- > 0)
				RemoveFirst();
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
		/// </summary>
		/// <returns>
		/// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
		/// </returns>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		public bool Contains(T item)
		{
			return EnumerateIndicesForward().Any(index => _data[index].Equals(item));
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type <paramref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
		public void CopyTo(T[] array, int arrayIndex)
		{
			if(array.Length - arrayIndex < Count)
				throw new ArgumentException("Not enough space in destination array");

			foreach (var index in EnumerateIndicesForward())
			{
				array[arrayIndex + index] = _data[index];
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <returns>
		/// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </returns>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		public bool Remove(T item)
		{
			throw new NotSupportedException("Can remove first or last item only");
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <returns>
		/// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </returns>
		public int Count
		{
			get { return _count; }
		}
		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		/// </summary>
		/// <returns>
		/// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
		/// </returns>
		public bool IsReadOnly
		{
			get { return false; }
		}
		#endregion

		#region Events
		protected virtual void OnItemRemoved(T item)
		{
			ItemRemoved.Fire(this, new ValueEventArgs<T>(item));
		}

		public event EventHandler<ValueEventArgs<T>> ItemRemoved;
		#endregion // Events

		public bool RemoveFirst()
		{
			if (Count > 0)
			{
				var item = _data[_startIndex];

				_data[_startIndex] = default(T);
				IncrementIndex(ref _startIndex);
				RefreshCount();
				SetModified();

				OnItemRemoved(item);
				return true;
			}
			return false;
		}

		public bool RemoveLast()
		{
			if (Count > 0)
			{
				var item = _data[_startIndex];

				_data[_endIndex] = default(T);
				DecrementIndex(ref _endIndex);
				RefreshCount();
				SetModified();

				OnItemRemoved(item);
				return true;
			}
			return false;
		}

		public bool IsEmpty
		{
			get { return _endIndex == _startIndex; }
		}

		public bool IsFull
		{
			get { return Count == _data.Length; }
		}

		public int Capacity
		{
			get { return _data.Length; }
			set
			{
				// TODO: implement CircularBuffer capacity change
				throw new NotImplementedException();
			}
		}

//		[ContractInvariantMethod]
//		private void ObjectInvariant()
//		{
//			Contract.Invariant(Capacity > 0);
//			Contract.Invariant(Count >= 0);
//			Contract.Invariant(Count <= Capacity);
//		}
	}
}