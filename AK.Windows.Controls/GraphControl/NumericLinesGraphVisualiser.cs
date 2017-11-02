using System;
using System.Windows;
using System.Windows.Media;

namespace AK.Windows.Controls.GraphControl
{
	public class NumericLinesGraphVisualiser : NumericGraphVisualiserBase
	{
		public Pen LinePen { get; set; }

		public NumericLinesGraphVisualiser()
		{
			LinePen = new Pen(Brushes.Red, 2);
		}

		protected override void RenderValues(DrawingContext dc, Matrix matrix, double[] values, int count)
		{
			double x = 0.0;
			int i = 0;
			for (; i < count; i++, x += 1.0)
			{
				if(i + 1 < count)
				{
					double value0 = values[i];
					double value1 = values[i+1];

					Point p0 = matrix.Transform(new Point(x, value0));
					Point p1 = matrix.Transform(new Point(x + 1.0, value1));
					dc.DrawLine(LinePen, p0, p1);
				}
			}
		} 
	}
}