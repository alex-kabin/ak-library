using System.Collections.Generic;

namespace AK.Toolbox.BruteForce
{
	/// <summary>
	/// ��������� �������� (���������) ��������
	/// </summary>
	public interface ISpecimenGenerator
	{
		/// <summary>
		/// ���������� �������
		/// </summary>
		/// <returns>��������� ��������</returns>
		IEnumerable<Specimen> Generate();
		
		/// <summary>
		/// ������ ���������� ���������
		/// </summary>
		ulong? Total { get; }
	}
}