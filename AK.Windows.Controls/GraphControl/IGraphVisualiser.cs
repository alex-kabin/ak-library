using System.Collections;
using System.Windows;
using System.Windows.Media;

namespace AK.Windows.Controls.GraphControl
{
	public interface IGraphVisualiser
	{
		void Visualise(DrawingContext dc, IEnumerator valuesEnumerator, Size size);
	}
}