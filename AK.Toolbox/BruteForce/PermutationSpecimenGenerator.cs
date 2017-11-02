using System;
using System.Collections.Generic;
using System.Linq;
using AK.Essentials;
using AK.Essentials.Extensions;

namespace AK.Toolbox.BruteForce
{
	public class PermutationSpecimenGenerator<T> : ISpecimenGenerator
	{
		private readonly T[] _array;
		private readonly Func<T[], Specimen> _specimenFactoryMethod;
		private readonly ulong _total;

		public PermutationSpecimenGenerator(T[] array, Func<T[], Specimen> specimenFactoryMethod)
		{
			_array = array;
			_specimenFactoryMethod = specimenFactoryMethod;
			_total = Maths.Factorial(array.Length);
		}

		public IEnumerable<Specimen> Generate()
		{
			return from p in _array.GeneratePermutations() select _specimenFactoryMethod(p);
		}

		public ulong? Total
		{
			get { return _total; }
		}
	}
}