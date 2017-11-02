using System.Windows;
using System.Windows.Media;

namespace AK.Windows.Controls
{
	public static class MatrixExtensions
	{
		public static Point TransformPoint(this Matrix matrix, double x, double y)
		{
			return matrix.Transform(new Point(x, y));
		}
	}
}