using System;
using System.Collections;
using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.Diagnostics.Contracts;
using AK.Essentials.Extensions;

namespace AK.Essentials.Collections
{
	public class CircularCollection<T> : ICollection<T>
	{
		private readonly LinkedList<T> _linkedList;
		private int _capacity;
		private int _version;

		public CircularCollection(int capacity)
		{
			//Contract.Requires<ArgumentOutOfRangeException>(capacity > 0, "Capacity should be > 0");

			_linkedList = new LinkedList<T>();
			_capacity = capacity;
		}

		private void SetModified()
		{
			_version++;
		}

		private void ThrowIfModified(int version)
		{
			if (_version != version)
				throw new InvalidOperationException("The collection was modified after the enumerator was created");
		}

		public IEnumerator<T> GetBackwardEnumerator()
		{
			var version = _version;
			var node = _linkedList.Last;
			while (node != null)
			{
				ThrowIfModified(version);		
				
				yield return node.Value;

				node = node.Previous;
			}
		}

		public int Capacity
		{
			get { return _capacity; }
			set
			{
//				Contract.Requires<ArgumentOutOfRangeException>(value > 0, "Capacity should be > 0");

				if (value < Count)
				{
					int itemsToRemove = Count - value;
					while (itemsToRemove-- > 0)
						RemoveExcessItem();
					SetModified();
				}
				_capacity = value;
			}
		}

		protected virtual void RemoveExcessItem()
		{
			var firstNode = _linkedList.First;
			if (firstNode != null)
			{
				var excessItem = firstNode.Value;
				_linkedList.Remove(firstNode);
				OnItemRemoved(excessItem);
			}
		}
		
		protected virtual void OnItemRemoved(T item)
		{
			ItemRemoved.Fire(this, new ValueEventArgs<T>(item));
		}
		
		public event EventHandler<ValueEventArgs<T>> ItemRemoved;

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
			return _linkedList.GetEnumerator();
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

		#region Implementation of ICollection<T>
		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		public void Add(T item)
		{
			if (_linkedList.Count >= _capacity)
				RemoveExcessItem();
			
			_linkedList.AddLast(item);
			SetModified();
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
		public void Clear()
		{
			while(Count > 0) RemoveExcessItem();
			SetModified();
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
			return _linkedList.Contains(item);
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type <paramref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
		public void CopyTo(T[] array, int arrayIndex)
		{
			_linkedList.CopyTo(array, arrayIndex);
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
			var result = _linkedList.Remove(item);
			if (result)
				SetModified();

			return result;
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <returns>
		/// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </returns>
		public int Count
		{
			get { return _linkedList.Count; }
		}
		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		/// </summary>
		/// <returns>
		/// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
		/// </returns>
		bool ICollection<T>.IsReadOnly
		{
			get { return false; }
		}
		#endregion

//		[ContractInvariantMethod]
//		private void ObjectInvariant()
//		{
//			Contract.Invariant(Capacity > 0);
//			Contract.Invariant(Count >= 0);
//			Contract.Invariant(Count <= Capacity);
//		}
	}
}