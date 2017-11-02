using System.Collections.Generic;

namespace AK.Toolbox.BruteForce
{
	/// <summary>
	/// Генератор образцов (вариантов) перебора
	/// </summary>
	public interface ISpecimenGenerator
	{
		/// <summary>
		/// Генерирует образцы
		/// </summary>
		/// <returns>Коллекция образцов</returns>
		IEnumerable<Specimen> Generate();
		
		/// <summary>
		/// Полное количество элементов
		/// </summary>
		ulong? Total { get; }
	}
}