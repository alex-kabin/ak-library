using System.Windows.Media;

namespace AK.Windows.Controls.GraphControl
{
	public interface INumericValuesDrawer
	{
		//void Render(DrawingContext dc, Matrix matrix, double[] values, int count);
		Drawing Draw(Transform transform, double[] values, int count);
	}
}